using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory connFactory = new ConnectionFactory
            {
                // AppSettings["CLOUDAMQP_URL"] contains the connection string
                // when you've added the CloudAMQP Addon
                //Uri = System.Configuration.ConfigurationManager.AppSettings["CLOUDAMQP_URL"];
                Uri = "amqp://rryotcdm:DPRstPbeaC4SNGsFKzhrJR0MaTfw-rEB@owl.rmq.cloudamqp.com/rryotcdm"
            };

            using (var connection = connFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("queue1", false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("queue1", true, consumer);

                    Console.WriteLine(" [*] Waiting for messages." +
                                             "To exit press CTRL+C");
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    }
                }
            }

        }
    }
}
