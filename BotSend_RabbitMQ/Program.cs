using RabbitMQ.Client;
using System;

namespace BotSend_RabbitMQ
{
    class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            // HtmlPack.HtmlPackLink();
            SendSources.Publish(channel);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
