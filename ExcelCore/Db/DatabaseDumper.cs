using ExcelCore.Cryptography;
using ExcelCore.Strategy;
using ExcelCore.Utility;
using Google.FlatBuffers;
using System.Text.Json.Nodes;

namespace ExcelCore.Db
{
    public class DatabaseDumper
    {
        private readonly string namespace_ = "FlatDataGlobal";
        private readonly bool is_global_ = true;

        public DatabaseDumper(bool is_global)
        {
            if (is_global == false)
            {
                namespace_ = "FlatDataJp";
            }
        }

        public Task Dump(List<byte[]> bytes_list, string class_name, bool is_raw)
        {
            return Task.Run(() =>
            {
                var dumper = new NormalTable();
                var class_type = Type.GetType($"{namespace_}.{class_name}");
                var class_method = class_type.GetMethod($"GetRootAs{class_name}", [typeof(ByteBuffer)]);

                IDataDumpStrategy dump_strategy = new();

                string output_dir = "dump";
                if (is_raw)
                {
                    output_dir = "raw";
                }
                else
                {
                    if (class_name.StartsWith("ScenarioScript"))
                    {
                        dump_strategy = new ScenarioScriptDumpStrategy(is_global_);
                    }
                    else if (class_name.StartsWith("ScenarioCharacterName"))
                    {
                        dump_strategy = new ScenarioCharacterNameDumpStrategy(is_global_);
                    }
                    else if (class_name.StartsWith("AcademyMessanger"))
                    {
                        dump_strategy = new AcademyMessangerDumpStrategy(is_global_);
                    }
                    else if (class_name.Contains("CharacterDialog"))
                    {
                        dump_strategy = new CharacterDialogDumpStrategy(is_global_);
                    }
                    else if (class_name.StartsWith("Localize"))
                    {
                        dump_strategy = new LocalizeDataDumpStrategy(is_global_);
                    }
                }

                var jarray = new JsonArray();

                foreach (var item in bytes_list)
                {
                    var excel_table = class_method.Invoke(null, [new ByteBuffer(item)]);
                    var jobject = dump_strategy.Dump(excel_table, dumper);
                    jarray.Add(jobject);
                }

                JsonUtils.Marshal(jarray, Path.Join(Directory.GetCurrentDirectory(), output_dir, $"{class_name}.json"));
                Console.WriteLine($"Exporting: {class_name}");
            });
        }
    }
}
