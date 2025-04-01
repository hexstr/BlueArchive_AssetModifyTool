using ExcelCore.Cryptography;
using System.Text.Json.Nodes;

namespace ExcelCore.Strategy
{
    public class ScenarioCharacterNameDumpStrategy : IDataDumpStrategy
    {
        public ScenarioCharacterNameDumpStrategy(bool is_global)
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

                if (name == "CharacterName")
                {
                    jobject.Add(obj.Key, obj.Value.DeepClone());
                }
                else if (name == "NameTW")
                {
                    name = "Name";
                    jobject.Add(name, obj.Value.DeepClone());
                }
                else if (name == "NicknameTW")
                {
                    name = "Nickname";
                    jobject.Add(name, obj.Value.DeepClone());
                }
            }

            return jobject;
        }
    }
}
