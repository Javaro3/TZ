using FileParser.Models;
using NLog;

namespace FileParser.Services {
    public class DataParserService {
        private readonly Logger _logger;

        public DataParserService(Logger logger) {
            _logger = logger;
        }

        public void SetRandomModuleState(IEnumerable<ObjectWithTypeModel> data) {
            var rand = new Random();
            ModuleState[] states = (ModuleState[])Enum.GetValues(typeof(ModuleState));

            foreach (var entity in data) {
                ModuleState randomModuleState = states[rand.Next(states.Length)];
                entity.Type.GetField(nameof(ModuleState)).SetValue(entity.Object, randomModuleState);
            }
            _logger.Info("ModuleStates were randomly generated");
        }
    }
}
