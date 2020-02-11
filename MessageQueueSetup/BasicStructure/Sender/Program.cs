using RabbitMQ.Client;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Models.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
                channel.QueueDeclare(queue: "sample", durable: true, exclusive: false, autoDelete: false, arguments: null);

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

                    var body = GetByteArray(basicMessageTransfer);

                    channel.BasicPublish(exchange: "", "sample", basicProperties: properties, body: body);
                    Console.WriteLine("sent");
                }
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
