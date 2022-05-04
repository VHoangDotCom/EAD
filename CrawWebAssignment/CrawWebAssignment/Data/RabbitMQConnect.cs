using HtmlAgilityPack;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        //Crawl data + send + schedule
        public async Task JobCrawler()
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .Build();



            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);

            // some sleep to show what's happening
            await Task.Delay(TimeSpan.FromSeconds(50));

            // and last shut down the scheduler when you are ready to close your program
            await scheduler.Shutdown();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }

        // simple log provider to get something to the console
        private class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
            {
                throw new NotImplementedException();
            }
        }

        //Recieve data from JobCrawler
       /* public void RecieveData()
        {
            Console.OutputEncoding = Encoding.UTF8;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())

            using (var cnn = DBConnect.connect())
            {
                cnn.Open();
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "news",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    Console.WriteLine(" [*] Waiting for messages. ");

                    string truncateSQL = "Truncate TABLE [dbo].[VNExpress]";
                    SqlCommand commandTruncate = new SqlCommand(truncateSQL, cnn);
                    commandTruncate.ExecuteNonQuery();

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var link = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received: {0}", link);
                        var web = new HtmlWeb();
                        HtmlDocument doc = web.Load(link); // Lấy nội dung bên trong link đó
                        var title = doc.QuerySelector("h1.title-detail").InnerHtml; // tìm đến những h1 có class= title-detail
                        var description = doc.QuerySelector("p.description").InnerText;
                        var image = doc.QuerySelector("img").Attributes["src"].Value;
                        // Sau khi lấy được dữ liệu thì bắn lên Database
                        try
                        {
                            Console.WriteLine("Openning Connection ...");
                            //open connection

                            Console.WriteLine("Connection successful!");
                            //VNExpress table
                            string query = "INSERT INTO VNExpress (title, description, image) VALUES (@title, @description, @image)";
                            var source = new Source()
                            {
                                title = title,
                                description = description,
                                image = image,
                            };

                            SqlCommand command = new SqlCommand(query, cnn);
                            command.Prepare();
                            command.Parameters.AddWithValue("@title", source.title);
                            command.Parameters.AddWithValue("@description", source.description);
                            command.Parameters.AddWithValue("@image", source.image);
                            command.ExecuteNonQuery();

                            //Source table
                            var title1 = doc.QuerySelector("h1.title-detail").InnerHtml;
                            var url = doc.QuerySelector("a").Attributes["href"].Value;
                            var title_detail = doc.QuerySelector("p.description").InnerText;
                            var content_detail = doc.QuerySelector("article.fck_detail").InnerText;
                            var image_detail = doc.QuerySelector(".fig-picture img").Attributes["src"].Value;
                            var date = DateTime.Now;
                            string query1 = "INSERT INTO Source (name, link, LinkSelector, TitleDetail, ContentDetail, ImageDetail, RemoteSelector, status, CreatedAt, UpdatedAt)" +
                            " VALUES (@title1, @url, null,@title_detail,@content_detail,@image_detail,null,1,getDate().Now,getDate().Now )";
                            var detailSource = new DetailSource()
                            {
                                name = title1,
                                link = url,
                                LinkSelector = null,
                                TitleDetail = title_detail,
                                ContentDetail = content_detail,
                                ImageDetail = image_detail,
                                RemoteSelector = null,
                                status = 1,
                                CreatedAt = date,
                                UpdatedAt = date,
                            };

                            SqlCommand command1 = new SqlCommand(query1, cnn);
                            command1.Prepare();
                            command1.Parameters.AddWithValue("@title1", detailSource.name);
                            command1.Parameters.AddWithValue("@url", detailSource.link);
                            command1.Parameters.AddWithValue("@title_detail", detailSource.TitleDetail);
                            command1.Parameters.AddWithValue("@content_detail", detailSource.ContentDetail);
                            command1.Parameters.AddWithValue("@image_detail", detailSource.ImageDetail);
                            command1.ExecuteNonQuery();

                            Console.WriteLine("Insert successful!");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                        }
                    };
                    channel.BasicConsume(queue: "news",
                                         autoAck: true,
                                         consumer: consumer);



                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }

        }*/

    }

    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var factory = new ConnectionFactory() { HostName = "localhost" };
             var connection = factory.CreateConnection();
             var channel = connection.CreateModel();
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