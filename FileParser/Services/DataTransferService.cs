using Newtonsoft.Json;
using NLog;
using RabbitMQ.Client;
using System.Text;

namespace FileParser.Services {
    public class DataTransferService {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;
        private readonly FileParserService _fileParser;
        private readonly DataParserService _dataParser;
        private readonly Logger _logger;

        public DataTransferService(string hostName, string queueName, FileParserService fileParser, DataParserService dataParser, Logger logger) {
            _factory = new ConnectionFactory() { HostName = hostName };
            _queueName = queueName;
            _fileParser = fileParser;
            _dataParser = dataParser;
            _logger = logger;
        }

        public void SendData(int timeOut) {
            using (var connection = _factory.CreateConnection()) {
                _logger.Info("Connection created");
                using (var chanel = connection.CreateModel()) {
                    chanel.QueueDeclare(queue: _queueName,
                        exclusive: false,
                        durable: true,
                        autoDelete: false,
                    arguments: null);

                    while (true) {
                        byte[] body = [];
                        Thread thread = new Thread(() => {
                            var data = _fileParser.Read();
                            _dataParser.SetRandomModuleState(data);
                            string json = JsonConvert.SerializeObject(data.Select(e => e.Object));
                            body = Encoding.UTF8.GetBytes(json);
                        });
                        thread.Start();
                        Thread.Sleep(timeOut);

                        chanel.BasicPublish(exchange: "",
                            routingKey: _queueName,
                            basicProperties: null,
                            body: body);

                        _logger.Info("Data added to queue");
                    }
                }
            }
        }
    }
}
