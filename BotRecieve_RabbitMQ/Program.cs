using RabbitMQ.Client;
using System;

namespace BotRecieve_RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            QueueRecieve.Consume(channel);
        }
    }
}
