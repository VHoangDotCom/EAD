using HtmlAgilityPack;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Send
{
    public  class BotQueue
    {
        public static  void QureryUrl()
        {
            var web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://vnexpress.net/bong-da");

           

        }
    }
}
