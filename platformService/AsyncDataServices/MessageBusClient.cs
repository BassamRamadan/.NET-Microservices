using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _Configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _Configuration = configuration;
            var factory = new ConnectionFactory
            {
                HostName = _Configuration["RabbitMQHost"] ?? "localhost",
                Port = int.Parse(_Configuration["RabbitMQPort"] ?? "5672")
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel =  _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("Connected to the message bus successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect to the message bus: {ex.Message}");

            }
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var msg = JsonSerializer.Serialize(platformPublishedDto);
            var body = Encoding.UTF8.GetBytes(msg);

            if (_connection.IsOpen && _channel != null)
            {
                _channel.BasicPublish(
                    exchange: "trigger",
                    routingKey: "",
                    basicProperties: null,
                    body: body);
                Console.WriteLine($"Published message: {msg}");
            }
            else
            {
                Console.WriteLine("Connection to the message bus is closed. Cannot publish message.");
            }
        }

        private void Dispose()
        {
            if (_connection.IsOpen)
            {
                _channel?.Close();
                _connection?.Close();
            }
        }

        // This method is called when the connection to RabbitMQ is shut down as event handler
        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection to RabbitMQ has been shut down.");
        }
    }
}