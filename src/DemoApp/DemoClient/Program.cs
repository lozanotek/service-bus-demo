using System;
using System.Configuration;
using DemoMessages;
using Microsoft.ServiceBus.Messaging;

namespace DemoClient
{
    class Program
    {
        static void Main()
        {
            var connectionString = ConfigurationManager.AppSettings["serviceBus.ConnectionString"];
            var queueClient =
              QueueClient.CreateFromConnectionString(connectionString, "demo");

            // Configure the callback options.
            var options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1),
                MaxConcurrentCalls = 4
            };

            // Callback to handle received messages.
            queueClient.OnMessage(message =>
            {
                try
                {
                    // Process message from queue.
                    Console.WriteLine("Message ID: " + message.MessageId);
                    Console.WriteLine("Body: " + message.GetBody<HelloCommand>());
                    Console.WriteLine("Expires At (UTC): " + message.ExpiresAtUtc);

                    // Remove message from queue.
                    message.Complete();
                }
                catch (Exception)
                {
                    // Indicates a problem, unlock message in queue.
                    message.Abandon();
                }
            }, options);

            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
