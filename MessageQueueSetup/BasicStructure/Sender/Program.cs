using RabbitMQ.Client;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using CrossCutting.Utils;
using Models.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Project - Sender");

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("logs", ExchangeType.Fanout);
                //channel.QueueDeclare(queue: "sample", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                while (true)
                {
                    Console.Write("message: ");
                    var message = Console.ReadLine();

                    var basicMessageTransfer = new BasicMessageTransfer
                    {
                        Message = message
                    };

                    var body = basicMessageTransfer.ToByteArray();

                    channel.BasicPublish(exchange: "logs", "", basicProperties: properties, body: body);
                    Console.WriteLine("sent");
                }
            }
        }
    }
}
