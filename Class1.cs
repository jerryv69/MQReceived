using Quartz;
using Quartz.Impl;

using System.IO;
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
        private static readonly ISchedulerFactory SchedulerFactory;
        private static readonly IScheduler Scheduler;
        private static IJobDetail _emailJobDetail;

        static Program()
        {

            // Create a regular old Quartz scheduler
            SchedulerFactory = new StdSchedulerFactory();
            Scheduler = SchedulerFactory.GetScheduler();

        }

        static void Main(string[] args)
        {

            // Now let's start our scheduler; you could perform
            // any processing or bootstrapping code here before
            // you start it but it must be started to schedule
            // any jobs
            Scheduler.Start();

            // Let's generate our email job detail now
            CreateJob();

            // And finally, schedule the job
            ScheduleJob();

            //jjsjsjs
        }
         
        private static void CreateJob()
        {

            // The job builder uses a fluent interface to
            // make it easier to build and generate an
            // IJobDetail object
            _emailJobDetail = JobBuilder.Create<EmailJob>()
                .WithIdentity("SendToMyself")   // Here we can assign a friendly name to our job        
                .Build();                       // And now we build the job detail

        }

        private static void ScheduleJob()
        {

            // Ask the scheduler to schedule our EmailJob
            ITrigger trigger = TriggerBuilder.Create()

                // A description helps other people understand what you want
                .WithDescription("Every 60 seconds")

                // A simple schedule is the easiest to build
                // It takes an Action<SimpleScheduleBuilder>
                // that creates a schedule according to your
                // specifications
                .WithSimpleSchedule(x => x

                    // Here we specify the interval
                    .WithIntervalInMinutes(3600)

                    // And how often to repeat it
                    .RepeatForever())

                // Finally, we take the schedule and build a trigger
                .Build();

            // Ask the scheduler to schedule our EmailJob
            Scheduler.ScheduleJob(_emailJobDetail, trigger);

        }

    }

    /// <summary>
    /// Our email job, yet to be implemented
    /// </summary>
    public class EmailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine(" [x] Execute");

            //// Let's start simple, write to the console
            //Console.WriteLine("Hello World 3600! " + DateTime.Now.ToString("h:mm:ss tt"));
            //ConnectionFactory connFactory = new ConnectionFactory
            //{
            //    // AppSettings["CLOUDAMQP_URL"] contains the connection string
            //    // when you've added the CloudAMQP Addon
            //    //Uri = System.Configuration.ConfigurationManager.AppSettings["CLOUDAMQP_URL"];
            //    Uri = "amqp://rryotcdm:DPRstPbeaC4SNGsFKzhrJR0MaTfw-rEB@owl.rmq.cloudamqp.com/rryotcdm"
            //};

            //using (var connection = connFactory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare("queue1", false, false, false, null);

            //        var consumer = new QueueingBasicConsumer(channel);
            //        channel.BasicConsume("queue1", true, consumer);

            //        Console.WriteLine(" [*] Waiting for messages." +
            //                                 "To exit press CTRL+C");
            //        while (true)
            //        {
            //            var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

            //            var body = ea.Body;
            //            var message = Encoding.UTF8.GetString(body);
            //            Console.WriteLine(" [x] Received {0}", message);
            //        }
            //    }
            //}
        }
    }
}