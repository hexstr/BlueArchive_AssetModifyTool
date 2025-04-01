using System.Text.Json;

namespace ExcelCore.Utility
{
    public class JsonUtils
    {
        private static readonly JsonSerializerOptions json_config = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
            IncludeFields = true
        };

        public static bool TryUnmarshal<T>(string file_name, out T value) where T : class
        {
            var asm_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            file_name = Path.Combine(asm_path, file_name);
            if (File.Exists(file_name))
            {
                var raw = File.ReadAllText(file_name);
                value = JsonSerializer.Deserialize<T>(raw);
                return true;
            }
            value = null;
            Console.WriteLine($"File {file_name} not found.");
            return false;
        }

        public static void TryMarshal(object data, string file_name)
        {
            var asm_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            file_name = Path.Combine(asm_path, file_name);
            var content = JsonSerializer.Serialize(data, json_config);
            File.WriteAllText(file_name, content);
        }

        public static T Unmarshal<T>(string file_name) where T : class
        {
            if (File.Exists(file_name))
            {
                var raw = File.ReadAllText(file_name);
                return JsonSerializer.Deserialize<T>(raw);
            }
            Console.WriteLine($"File {file_name} not found.");
            return null;
        }

        public static void Marshal(object data, string file_name)
        {
            var content = JsonSerializer.Serialize(data, json_config);
            File.WriteAllText(file_name, content);
        }
    }
}
