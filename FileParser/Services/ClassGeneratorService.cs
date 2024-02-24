using FileParser.Models;
using System.Reflection.Emit;
using System.Reflection;
using NLog;

namespace FileParser.Services {
    public class ClassGeneratorService {
        private readonly Logger _logger;

        public ClassGeneratorService(Logger logger) {
            _logger = logger;
        }

        public Type GenerateType(ClassGenerateModel model) {
            AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(model.ClassName, TypeAttributes.Public);

            foreach (var field in model.Fields) {
                typeBuilder.DefineField(field.Name, field.Type, FieldAttributes.Public);
            }
            Type dynamicType = typeBuilder.CreateType();
            _logger.Info($"Class {model.ClassName} generated");
            return dynamicType;
        }

        public object GenerateObject(Type type, ClassGenerateModel model) {
            object dynamicObject = Activator.CreateInstance(type);

            foreach (var field in model.Fields) {
                type.GetField(field.Name).SetValue(dynamicObject, field.Value);
            }

            _logger.Info($"Object {model.ClassName} created");
            return dynamicObject;
        }
    }
}