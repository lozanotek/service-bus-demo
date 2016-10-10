using System;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;
using ServiceBusDemo.Messages;

namespace ServiceBusDemo.Client
{
    class Program
    {
        static void Main()
        {
            var connString = ConfigurationManager
                .ConnectionStrings["azure.ServiceBus"];

            var queueClient =
              QueueClient.CreateFromConnectionString(connString.ConnectionString, "demo");

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
