using ExcelCore.Cryptography;
using System.Text.Json.Nodes;

namespace ExcelCore.Strategy
{
    public class LocalizeDataDumpStrategy : IDataDumpStrategy
    {
        public LocalizeDataDumpStrategy(bool is_global)
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

                var postfix = name[^2..].ToLower();
                if (postfix == "kr" || postfix == "en" || postfix == "th")
                {
                    continue;
                }

                if (name.StartsWith("SkillInvoke"))
                {
                    continue;
                }

                if (IsGlobal)
                {
                    if (name.Equals("Tw"))
                    {
                        name = "Value";
                    }
                    else if (postfix == "tw")
                    {
                        name = name[..^2];
                    }
                    if (postfix == "jp")
                    {
                        continue;
                    }
                }
                else
                {
                    if (name.Equals("Jp"))
                    {
                        name = "Value";
                    }
                    else if (postfix == "jp")
                    {
                        name = name[..^2];
                    }
                }

                jobject.Add(name, obj.Value.DeepClone());
            }

            return jobject;
        }
    }
}
