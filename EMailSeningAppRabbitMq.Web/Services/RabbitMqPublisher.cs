using System.Text.Json;
using System.Text;
using EMailSeningAppRabbitMq.Web.Models;
using RabbitMQ.Client;

namespace EMailSeningAppRabbitMq.Web.Services
{
    public class RabbitMqPublisher
    {
        private readonly RabbitMqClientServices _rabbitMQServices;

        public RabbitMqPublisher(RabbitMqClientServices rabbitMQServices)
        {
            _rabbitMQServices = rabbitMQServices;

        }

        public void Publish(MailMessageClient mailmessage)
        {
            var channel = _rabbitMQServices.Connect();
            var bodystring = JsonSerializer.Serialize(mailmessage);
            var bodybyte = Encoding.UTF8.GetBytes(bodystring);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMqClientServices.ExchangeName, routingKey: RabbitMqClientServices.RoutingExel, basicProperties: properties, body: bodybyte);


        }
    }
}
