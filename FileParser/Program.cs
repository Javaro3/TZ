using FileParser.Services;
using NLog;
using System.Configuration;

namespace FileParser {
    public class Program {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        static void Main() {
            string xmlPath = ConfigurationManager.AppSettings["XmlPath"];
            string rabbitMQHostName = ConfigurationManager.AppSettings["RabbitMQHostName"];
            string queueName = ConfigurationManager.AppSettings["QueueName"];
            string nlogConfig = ConfigurationManager.AppSettings["NLogConfig"];
            LogManager.LoadConfiguration(nlogConfig);
            logger.Info("Application started");

            int timeOut = 1000;

            var classGenerator = new ClassGeneratorService(logger);
            var fileParser = new FileParserService(xmlPath, classGenerator, logger);
            var dataParser = new DataParserService(logger);
            var dataTransfer = new DataTransferService(rabbitMQHostName, queueName, fileParser, dataParser, logger);
            
            try {
                dataTransfer.SendData(timeOut);
            }
            catch(Exception ex) {
                logger.Error(ex.Message);
            }
            logger.Info("Application finished");
        }
    }
}