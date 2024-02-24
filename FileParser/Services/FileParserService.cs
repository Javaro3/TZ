using FileParser.Models;
using NLog;
using System.Xml.Linq;

namespace FileParser.Services {
    public class FileParserService {
        private readonly string _filePath;
        private readonly ClassGeneratorService _classGenerator;
        private const string XML_HEADER = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        private readonly Logger _logger;
        public FileParserService(string filePath, ClassGeneratorService classGenerator, Logger logger) {
            _filePath = filePath;
            _classGenerator = classGenerator;
            _logger = logger;
        }

        public IEnumerable<ObjectWithTypeModel> Read() {
            string targetTag = "DeviceStatus";
            var fileData = File.ReadAllText(_filePath).Replace(XML_HEADER, "");
            var xdoc = XDocument.Parse(fileData);
            var elements = xdoc.Descendants(targetTag);
            List<ObjectWithTypeModel> result = [];

            foreach (var element in elements) {
                string className = element.Value;
                var fields = GetFields(element);
                var classModel = new ClassGenerateModel(className, fields);

                var dynamicType = _classGenerator.GenerateType(classModel);
                var dynamicObject = _classGenerator.GenerateObject(dynamicType, classModel);

                var model = new ObjectWithTypeModel(dynamicType, dynamicObject);
                result.Add(model);
            }
            _logger.Info($"File {_filePath} readed");
            return result;
        }

        private List<Field> GetFields(XElement root) {
            var fields = new List<Field>();
            foreach (var element in root.Descendants()) {
                GetFields(element, fields);
            }
            return fields;
        }

        private void GetFields(XElement root, List<Field> fields) {
            if (!root.HasElements) {
                string fieldName = root.Name.ToString();
                Type fieldType = GetFieldType(root.Value);
                var fieldValue = fieldType.IsEnum ? Enum.Parse(fieldType, root.Value) : Convert.ChangeType(root.Value, fieldType);

                var field = new Field(fieldName, fieldType, fieldValue);
                fields.Add(field);
            }
        }

        private Type GetFieldType(string value) {
            if (int.TryParse(value, out _)) {
                return typeof(int);
            }
            else if (double.TryParse(value, out _)) {
                return typeof(double);
            }
            else if (bool.TryParse(value, out _)) {
                return typeof(bool);
            }
            if (Enum.TryParse(value, out ModuleState _)) {
                return typeof(ModuleState);
            }
            return typeof(string);
        }
    }
}