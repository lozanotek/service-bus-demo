using System;
using System.Configuration;
using System.Threading;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus.Messaging;
using MTCommands;

namespace MTDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = ConfigureBus();
            bus.Start();

            Console.WriteLine("Press ESC to stop");
            var today = DateTime.Now;
            for (var i=0; i<1000; i++)
            {
                var command = new HelloCommand
                {
                    Text = "Hello from Mass Transit!",
                    Date = today
                };

                bus.Publish(command);
                Thread.Sleep(1000);

                today = DateTime.Now;
            }

            bus.Stop();
        }

        private static IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                cfg.Host(
                    ConfigurationManager.AppSettings["servicebus.Connection"],
                    h =>
                    {
                        h.TransportType = TransportType.Amqp;
                    });
            });
        }
    }
}
