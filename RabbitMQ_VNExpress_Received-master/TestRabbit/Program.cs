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
            /*string connectionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            SqlDataReader dataReader;
            connectionString = @"Data Source=(localDb)\MSSQLLocalDB;Initial Catalog=RabbitDB;Persist Security Info=True";
            sql = "Select * from Source";
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader.GetValue(0) + " - " + dataReader.GetValue(1));
                }
                dataReader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot open connection !" + ex.Message);
            }
            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();*/

            Console.OutputEncoding = Encoding.UTF8;
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



        }
    }
}
