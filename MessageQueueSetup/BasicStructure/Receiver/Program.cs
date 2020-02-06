using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
                channel.QueueDeclare(queue: "sample", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] Received: {0}", message);
                };

                while (true)
                    channel.BasicConsume(queue: "sample", autoAck: true, consumer: consumer);
            }

            Console.WriteLine("Finished!");
            Console.ReadKey();
        }
    }
}
