using DataProcessor.Services;
using NLog;
using System.Configuration;

namespace DataProcessor {
    public class Program {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        static void Main() {
            string rabbitMQHostName = ConfigurationManager.AppSettings["RabbitMQHostName"];
            string queueName = ConfigurationManager.AppSettings["QueueName"];
            string nlogConfig = ConfigurationManager.AppSettings["NLogConfig"];
            LogManager.LoadConfiguration(nlogConfig);
            logger.Info("Application started");

            var context = new ApplicationContext();
            var repository = new ModuleRepository(context);
            var dataProcessor = new DataProcessorService(repository, logger);
            var dataTransfer = new DataTransferService(rabbitMQHostName, queueName, dataProcessor, logger);
            try {
                dataTransfer.GetData();
            }
            catch (Exception ex) {
                logger.Error(ex.Message);
            }
            logger.Info("Application finished");
        }
    }
}