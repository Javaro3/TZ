using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DataProcessor.Services {
    public class DataTransferService {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;
        private readonly DataProcessorService _dataProcessor;
        private readonly Logger _logger;

        public DataTransferService(string hostName, string queueName, DataProcessorService dataProcessor, Logger logger) {
            _factory = new ConnectionFactory() { HostName = hostName };
            _queueName = queueName;
            _dataProcessor = dataProcessor;
            _logger = logger;
        }

        public void GetData() {
            using (var connection = _factory.CreateConnection()) {

                _logger.Info("Connnection opened");
                using (var chanel = connection.CreateModel()) {
                    chanel.QueueDeclare(queue: _queueName,
                        exclusive: false,
                        durable: true,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(chanel);
                    consumer.Received += (model, es) => {
                        var body = es.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        _dataProcessor.SaveModules(json);
                    };

                    chanel.BasicConsume(queue: _queueName,
                        autoAck: true,
                        consumer: consumer);
                    Console.Read();
                }
            }
        }
    }
}