using EFCore.BulkExtensions;
using System.Text.Json;

namespace ExcelCore.Db
{
    public static class DatabaseWriter
    {
        public static void Write(string output_file)
        {
            var assembly = typeof(DatabaseWriter).Assembly;
            var buildable_type_list = assembly.ExportedTypes.Where(x => x.Namespace == "ExcelCore.Converter");
            var excel_context = new ExcelContext(output_file);

            foreach (var buildable_type in buildable_type_list)
            {
                var resource_file = buildable_type.Name.Replace("Converter", "");
                var asm_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var raw_file_name = Path.Combine(asm_path, $"raw/{resource_file}.json");
                var add_file_name = Path.Combine(asm_path, $"add/{resource_file}.json");

                if (File.Exists(raw_file_name))
                {
                    var create_method = buildable_type.GetMethod("Build");
                    var deserialization_type = assembly.GetType($"ExcelCore.Model.{resource_file.Replace("Excel", "")}");
                    var deserialization_array_type = deserialization_type.MakeArrayType();

                    Console.WriteLine($"Building: {resource_file}");
                    var text = File.ReadAllText(raw_file_name);
                    var raw_data = JsonSerializer.Deserialize(text, deserialization_array_type);
                    var data = (List<object>)create_method.Invoke(null, [raw_data]);
                    excel_context.UpdateRange(data);

                    if (File.Exists(add_file_name))
                    {
                        Console.WriteLine($"Adding: {resource_file}");
                        var add_text = File.ReadAllText(add_file_name);
                        var add_raw_data = JsonSerializer.Deserialize(add_text, deserialization_array_type);
                        var add_data = (List<object>)create_method.Invoke(null, [add_raw_data]);

                        int idx = data.Count + 1;
                        foreach (BaseDBSchema add in add_data)
                        {
                            add.RowId = idx++;
                        }

                        excel_context.BulkInsertOrUpdate(add_data);
                    }
                }

                excel_context.SaveChanges();
            }
        }
    }
}
