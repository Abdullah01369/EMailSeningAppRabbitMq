using EMailSeningAppRabbitMq.Web.Models;
using EMailSeningAppRabbitMq.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EMailSeningAppRabbitMq.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        

        public HomeController(ILogger<HomeController> logger,RabbitMqPublisher rabbitMqPublisher)
        {
            _rabbitMqPublisher = rabbitMqPublisher;
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Index(MailMessageClient mailMessage)
        {
            _rabbitMqPublisher.Publish(new MailMessageClient { MessageContent=mailMessage.MessageContent, ReceiverMail="aaaa"});
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}