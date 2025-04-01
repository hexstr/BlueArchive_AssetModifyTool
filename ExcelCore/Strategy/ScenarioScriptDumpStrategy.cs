using ExcelCore.Cryptography;
using System.Text.Json.Nodes;

namespace ExcelCore.Strategy
{
    public class ScenarioScriptDumpStrategy : IDataDumpStrategy
    {
        public ScenarioScriptDumpStrategy(bool is_global)
        {
            IsGlobal = is_global;
        }

        public override JsonObject Dump(object data_list, NormalTable dumper)
        {
            var base_jobject = base.Dump(data_list, dumper);
            var jobject = new JsonObject();

            foreach (var obj in base_jobject)
            {
                var name = obj.Key;

                if (name == "GroupId")
                {
                    jobject.Add(obj.Key, obj.Value.DeepClone());
                }

                if (IsGlobal)
                {
                    if (name == "TextTw")
                    {
                        name = "Text";
                        jobject.Add(name, obj.Value.DeepClone());
                    }
                }
                else if (name.StartsWith("TextJp"))
                {
                    name = "Text";
                    jobject.Add(name, obj.Value.DeepClone());
                }
            }

            return jobject;
        }
    }
}
