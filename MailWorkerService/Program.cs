using EMailSeningAppRabbitMq.Web.Services;
using MailWorkerService;
using MailWorkerService.Models;
using MailWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;




IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        IConfiguration Configuration = context.Configuration;
        services.AddSingleton<RabbitMqClientService>();
        services.AddSingleton<IEmailSenderService, EmailServiceClient>();
        services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));  // options pattern
        services.AddSingleton(sp => new ConnectionFactory()
        {
            Uri = new Uri
   (Configuration.GetConnectionString("RabbitMqConnection")),
            DispatchConsumersAsync = true
        });
        services.AddHostedService<Worker>();
    })
    .Build();



host.Run();

