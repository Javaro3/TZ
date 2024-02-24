using DataProcessor.Models;
using Newtonsoft.Json;
using NLog;

namespace DataProcessor.Services {
    public class DataProcessorService {
        private readonly ModuleRepository _moduleRepository;
        private readonly Logger _logger;

        public DataProcessorService(ModuleRepository moduleRepository, Logger logger) {
            _moduleRepository = moduleRepository;
            _logger = logger;
        }

        public void SaveModules(string json) {
            List<dynamic> entities = JsonConvert.DeserializeObject<List<dynamic>>(json);
            if(entities == null) {
                return;
            }
            foreach (var entity in entities) {
                string moduleCategoryID = (string)entity.ModuleCategoryID;
                var moduleState = ((ModuleState)entity.ModuleState).ToString();
                var module = new ModuleModel() {
                    ModuleCategoryID = moduleCategoryID,
                    ModuleState = moduleState
                };
                _moduleRepository.Add(module);
                _logger.Info($"Module[{module.ModuleCategoryID}, {module.ModuleState}] add to database");
            }
        }
    }
}