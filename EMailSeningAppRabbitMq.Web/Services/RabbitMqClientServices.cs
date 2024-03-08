using RabbitMQ.Client;

namespace EMailSeningAppRabbitMq.Web.Services
{
    public class RabbitMqClientServices:IDisposable
    {
        // readonly sadece yapıcıda set edilebilr.
        private readonly ILogger<RabbitMqClientServices> _logger;
        private readonly ConnectionFactory _connectionFactory; // rabbitmq bağlantısı için nesne
        private IConnection _connection; // baglantı
        private IModel _channel; //kanal

        public static string ExchangeName = "MailSenderDirectExchange";
        public static string RoutingExel = "MailSender-route";
        public static string QueueName = "queue-mailsender";


        public RabbitMqClientServices(ConnectionFactory connectionFactory, ILogger<RabbitMqClientServices> logger)
        {

            _connectionFactory = connectionFactory;
            _logger = logger;

        }

        
        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen:true}) 
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);
            _channel.QueueDeclare(QueueName, true, false, false, null);
            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingExel); // MAPLAMA
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
