using RabbitMQ.Client;
using System;
using System.Text;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "sample", durable: false, exclusive: false, autoDelete: false, arguments: null);


                while (true)
                {
                    Console.Write("message: ");
                    var message = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", "sample", basicProperties: null, body: body);
                    Console.WriteLine("sent");
                }
            }

            Console.WriteLine("Finished!");
            Console.ReadKey();
        }
    }
}
