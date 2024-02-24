namespace FileParser.Models {
    public class ClassGenerateModel(string className, IEnumerable<Field> fields) {
        public string ClassName { get; } = className;
        public IEnumerable<Field> Fields { get; } = fields;
    }
}
