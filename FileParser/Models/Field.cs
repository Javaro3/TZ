namespace FileParser.Models {
    public class Field(string fieldName, Type fieldType, object Value) {
        public string Name { get; } = fieldName;
        public Type Type { get; } = fieldType;
        public object Value { get; } = Value;
    }
}
