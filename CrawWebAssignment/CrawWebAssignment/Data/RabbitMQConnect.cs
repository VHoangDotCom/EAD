using HtmlAgilityPack;
using Quartz;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CrawWebAssignment.Data
{
    public class RabbitMQConnect
    {
        public IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.Port = 5672;
            factory.HostName = "localhost";
            factory.VirtualHost = "/";

            return factory.CreateConnection();
        }

        //Send data
        public bool send(IConnection con, string message, string queueName)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
                channel.QueueDeclare(queueName, true, false, false, null);
                channel.QueueBind(queueName, "messageexchange", queueName, null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("messageexchange", queueName, null, msg);

            }
            catch (Exception)
            {

            }
            return true;
        }

        //Recieve Data
        public string receive(IConnection con, string myqueue)
        {
            try
            {
                string queue = myqueue;
                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: queue, autoAck: true);
                if (result != null)
                {
                    var body = result.Body.ToArray();
                    return Encoding.UTF8.GetString(body);
                }
                else { return null; }

            }
            catch (Exception)
            {
                return null;

            }
        }

    }

    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://vnexpress.net/bong-da");

            var nodes = document.DocumentNode.SelectNodes("//h2/a");

            channel.QueueDeclare(queue: "news",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            foreach (var item in nodes)
            {
                var message = item.GetAttributeValue("href", "");

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "news",
                                     basicProperties: null,
                                     body: body);
                await Console.Out.WriteLineAsync(message);
                Thread.Sleep(50);
            }

        }
    }
}