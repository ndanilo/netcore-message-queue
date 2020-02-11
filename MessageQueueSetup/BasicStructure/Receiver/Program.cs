using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Models.DTO;
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
                channel.QueueDeclare(queue: "sample", durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var basicMessageTransfer = GetObject(body);
                    //var basicMessageTransfer = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] Received: {0}", basicMessageTransfer);
                };

                while (true)
                    channel.BasicConsume(queue: "sample", autoAck: true, consumer: consumer);
            }
        }

        static byte[] GetByteArray(object obj)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        static BasicMessageTransfer GetObject(byte[] obj)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(obj))
                return (BasicMessageTransfer)bf.Deserialize(ms); ;
        }
    }
}
