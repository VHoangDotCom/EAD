using HtmlAgilityPack;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotRecieve_RabbitMQ
{
    class QueueRecieve
    {
        public static void Consume(IModel channel)
        {
            channel.QueueDeclare(queue: "news_send",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            HtmlWeb web = new HtmlWeb();

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var link = Encoding.UTF8.GetString(body);
                HtmlDocument doc = web.Load(link);
                var title = doc.QuerySelector("h1.title-detail").InnerText;
                var description = doc.QuerySelector("p.description").InnerText;
                var imageNode = doc.QuerySelector("img").Attributes["src"].Value;
                var content = doc.QuerySelector("article.fck_detail").InnerText;
                Console.WriteLine(title.Trim());
                Console.WriteLine(description.Trim());
                Console.WriteLine(imageNode);
                Console.WriteLine(content);
            };
            channel.BasicConsume(queue: "news_send",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
