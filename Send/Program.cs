using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using System.Text.Json;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Threading;

namespace Send
{
    class Program
    {

        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://vnexpress.net/bong-da");

            var nodes = document.DocumentNode.SelectNodes("//h2/a");

            channel.QueueDeclare(queue: "task_queue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            foreach (var item in nodes)
            {
                var message = item.GetAttributeValue("href", "");

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(exchange: "",
                                     routingKey: "task_queue",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
                Thread.Sleep(1000);
            }
        }
    }

    public class Student
    {
        public string UserName { get; set; }
        public int Age { get; set; }
    }
}