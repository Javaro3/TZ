using System.ComponentModel.DataAnnotations;

namespace DataProcessor.Models {
    public class ModuleModel {
        [Key]
        public string ModuleCategoryID { get; set; }
        public string ModuleState {  get; set; }
    }
}
