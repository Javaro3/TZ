namespace FileParser.Models {
    public class ObjectWithTypeModel(Type dynamicType, object dynamicObject) {
        public Type Type { get; } = dynamicType;
        public object Object { get; } = dynamicObject;
    }
}