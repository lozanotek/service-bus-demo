using System;
using System.Configuration;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus.Messaging;

namespace MTClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = ConfigureBus();
            bus.Start();

            Console.Write("Press any key to exit...");
            Console.ReadKey(true);

            bus.Stop();
        }

        private static IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host(
                    ConfigurationManager.AppSettings["servicebus.Connection"],
                    h =>
                    {
                        h.TransportType = TransportType.Amqp;
                    });

                cfg.ReceiveEndpoint(host, "consumers", c =>
                {
                    c.Consumer<HelloConsumer>();
                });
            });
        }
    }
}
