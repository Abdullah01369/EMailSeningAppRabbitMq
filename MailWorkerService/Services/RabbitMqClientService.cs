using EMailSeningAppRabbitMq.Web.Services;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailWorkerService.Services
{
    public class RabbitMqClientService:IDisposable
    {
        private readonly ConnectionFactory _connectionFactory; // rabbitmq bağlantısı için nesne
        private IConnection _connection; // baglantı
        private IModel _channel; //kanal


        public static string QueueName = "queue-mailsender";

        private readonly ILogger<RabbitMqClientServices> _logger;


        public RabbitMqClientService(ConnectionFactory connectionFactory, ILogger<RabbitMqClientServices> logger)
        {

            _connectionFactory = connectionFactory;
            _logger = logger;

        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection(); // BAGLANTI OLUŞTUR
            if (_channel is { IsOpen: true })  // BAĞLANTI AÇIK MI KONTROLU YAPIYOR, AÇIKSA CHANNELİ DONDUR
            {
                return _channel;
            }
            _channel = _connection.CreateModel();

            _logger.LogInformation("Rabbitmq ile baglantı kuruldu...");

            return _channel;

        }

        public void Dispose() // bellekten boşaltma işlemi gerçekleştırır
        {
            _channel?.Close();
            _channel?.Dispose();
            _channel = default;
            _connection?.Close();
            _connection?.Dispose();
            _logger.LogInformation("baglantı koptu");

        }
    }
}
