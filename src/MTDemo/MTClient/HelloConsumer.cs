using System;
using System.Threading.Tasks;
using MassTransit;
using MTCommands;

namespace MTClient
{
    public class HelloConsumer : IConsumer<HelloCommand>
    {
        public async Task Consume(ConsumeContext<HelloCommand> context)
        {
            await Console.Out.WriteLineAsync($"Date: {context.Message.Date}");
            await Console.Out.WriteLineAsync($"Message: {context.Message.Text}");
        }
    }
}