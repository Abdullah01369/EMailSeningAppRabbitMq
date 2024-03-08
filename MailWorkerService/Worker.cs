using EMailSeningAppRabbitMq.Web.Services;
using MailWorkerService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data;
using System.Text.Json;
using System.Text;
using EMailSeningAppRabbitMq.Web.Models;


namespace MailWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<Worker> _logger;
        private RabbitMqClientService _rabbitMqClientServices;
        private readonly IServiceProvider _serviceProvider;
        private IModel _channel;

        public Worker(IServiceProvider provider,ILogger<Worker> logger,RabbitMqClientService rabbitMqClientService,IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
            _serviceProvider = provider;
            _rabbitMqClientServices = rabbitMqClientService;
            _logger = logger;
        }

 


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMqClientServices.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(RabbitMqClientServices.QueueName, false, consumer);
            consumer.Received += Consumer_Received;
            return Task.CompletedTask;

        }




        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {

            var response = JsonSerializer.Deserialize<MailMessageClient>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            await  _emailSenderService.SendMailAsync(response.MessageContent, response.ReceiverMail);

            _channel.BasicAck(@event.DeliveryTag, false);
            // calýsýyorrrrrrrr

        }



     

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}