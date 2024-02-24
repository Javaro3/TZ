using DataProcessor.Models;

namespace DataProcessor.Services {
    public class ModuleRepository {
        private readonly ApplicationContext _context;

        public ModuleRepository(ApplicationContext context) { 
            _context = context;
        }

        public void Add(ModuleModel model) {
            if(_context.Modules.Any(e => e.ModuleCategoryID == model.ModuleCategoryID)) {
                var databaseModel = _context.Modules.FirstOrDefault(e => e.ModuleCategoryID == model.ModuleCategoryID);
                databaseModel.ModuleState = model.ModuleState;
            }
            else {
                _context.Modules.Add(model);
            }
            _context.SaveChanges();
        }
    }
}
