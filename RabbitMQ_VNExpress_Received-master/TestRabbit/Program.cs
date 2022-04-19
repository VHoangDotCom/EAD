using HtmlAgilityPack;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TestRabbit
{
    class Program
    {
        static void Main(string[] args)
        {


            //////////////////////////////////////
            //Phần lấy thông tin từ web

            Console.OutputEncoding = Encoding.UTF8;
            //using (var cnn = DBConnect.connect())
            //{
            //    try
            //    {
            //        Console.WriteLine("Openning Connection ...");
            //        //open connection
            //        cnn.Open();
            //        Console.WriteLine("Connection successful!");



            //        string query = "INSERT INTO VNExpressReceived (title, description, image) VALUES (@title, @description, @image)";

            //        //create a new SQL Query using StringBuilder
            //        //StringBuilder strBuilder = new StringBuilder();
            //        //strBuilder.Append("INSERT INTO VNExpressReceived (title, description, image) VALUES ");
            //        //strBuilder.Append("(N'Báo lá cải', N'Báo test', N'Ảnh báo lá cải')");
            //        //string sqlQuery = strBuilder.ToString();
            //        //using (SqlCommand command = cnn.CreateCommand()) //pass SQL query created above and connection
            //        //{

            //        //    command.ExecuteNonQuery(); //execute the Query
            //        //    Console.WriteLine("Query Executed.");
            //        //}

            //        //strBuilder.Clear(); // clear all the string

            //        var source = new Source()
            //        {
            //            title = "Báo lá cải",
            //            description = "Báo test",
            //            image = "Ảnh báo lá cải",
            //        };

            //        SqlCommand command = new SqlCommand(query, cnn);
            //        command.Prepare();
            //        command.Parameters.AddWithValue("@title", source.title);
            //        command.Parameters.AddWithValue("@description", source.description);
            //        command.Parameters.AddWithValue("@image", source.image);
            //        command.ExecuteNonQuery();

            //        Console.WriteLine("Add ok");
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("Error: " + e.Message);
            //    }
            //}


            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())

            using (var cnn = DBConnect.connect())
            {
                cnn.Open();
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "task_queue",
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
                        var description = doc.QuerySelector("p.description").InnerHtml;
                        var image = doc.QuerySelector("img").Attributes["src"].Value;
                        // Sau khi lấy được dữ liệu thì bắn lên Database
                        try
                        {
                            Console.WriteLine("Openning Connection ...");
                            //open connection
                            
                            Console.WriteLine("Connection successful!");

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

                            Console.WriteLine("Add ok");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                        }
                    };
                    channel.BasicConsume(queue: "task_queue",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }


            
        }
    }
}
