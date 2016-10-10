using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.ServiceBus.Messaging;
using ServiceBusDemo.Messages;

namespace ServiceBusDemo.Web.Controllers
{
    public class CommandController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Index(string text)
        {
            var connString = ConfigurationManager
                .ConnectionStrings["azure.ServiceBus"];

            var queueClient =
              QueueClient.CreateFromConnectionString(connString.ConnectionString, "demo");
            
            var message = new BrokeredMessage(new HelloCommand { Text = text } );
            await queueClient.SendAsync(message);

            return RedirectToAction("index", "home");
        }
    }
}
