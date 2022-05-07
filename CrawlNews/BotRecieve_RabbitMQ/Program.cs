using RabbitMQ.Client;
using System;

namespace BotRecieve_RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
           
            QueueRecieve.Consume();
        }
    }
}
