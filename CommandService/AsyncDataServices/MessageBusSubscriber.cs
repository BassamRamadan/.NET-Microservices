
using System.Text;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection? _connection;
        private IModel? _channel;
        private string? _queue;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            IntialRabbitMQ();
        }

        private void IntialRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"] ?? "5672")
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queue = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queue, exchange: "trigger", routingKey: "");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            Console.WriteLine("Connected to the message bus successfully.");
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection to the message bus has been shut down.");
        }

        public override void Dispose()
        {
            if (_connection is not null && _connection.IsOpen)
            {
                _channel?.Close();
                _connection?.Close();
            }
            base.Dispose();
            Console.WriteLine("Message bus subscriber disposed.");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"Message received: {message}");

                _eventProcessor.ProcessEvent(message);
            };

            _channel.BasicConsume(queue: _queue, autoAck: true, consumer: consumer);
            Console.WriteLine("Listening for messages...");
            
            return Task.CompletedTask;
        }
    }
}