using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using DemoMessages;
using Microsoft.ServiceBus.Messaging;

namespace DemoApp.Controllers
{
    public class CommandController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Index(string text)
        {
            var connectionString = ConfigurationManager.AppSettings["serviceBus.ConnectionString"];

            var queueClient =
              QueueClient.CreateFromConnectionString(connectionString, "demo");
            
            var message = new BrokeredMessage(new HelloCommand { Text = text } );
            await queueClient.SendAsync(message);

            return RedirectToAction("index", "home");
        }
    }
}
