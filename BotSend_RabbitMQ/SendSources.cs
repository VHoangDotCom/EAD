using HtmlAgilityPack;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotSend_RabbitMQ
{
    class SendSources
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

        public static void Publish(IModel channel)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://vnexpress.net/bong-da");
            var docs = document.QuerySelectorAll("h2.title-news a");

            channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
            channel.QueueDeclare(queue: "hello",
                                    durable: true,
                                    exclusive: true,
                                    autoDelete: false,
                                    arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            foreach (var item in docs)
            {
                var message = item.Attributes["href"].Value;

                Console.WriteLine(message);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                        routingKey: "hello",
                                        basicProperties: null,
                                        body: body);
                Thread.Sleep(1000);
            }
        }
    }
}
