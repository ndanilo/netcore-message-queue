using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using CrossCutting.Utils;
using Models.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.Impl;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Project - Receiver");

            var factory = new ConnectionFactory { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("logs", ExchangeType.Fanout);
                //channel.QueueDeclare(queue: "sample", durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var basicMessageTransfer = body.ToObject<BasicMessageTransfer>();

                    //var basicMessageTransfer = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] Received: {0}", basicMessageTransfer);
                };

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "", null);

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                Console.ReadKey();
            }
        }
    }
}
