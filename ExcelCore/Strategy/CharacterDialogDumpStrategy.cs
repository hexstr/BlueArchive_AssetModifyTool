using ExcelCore.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace ExcelCore.Strategy
{
    public class CharacterDialogDumpStrategy : IDataDumpStrategy
    {
        public CharacterDialogDumpStrategy(bool is_global)
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
                name = name[..^2];
                if (postfix != "jp" && postfix != "tw")
                {
                    continue;
                }

                if (name == "Localize")
                {
                    var value = obj.Value.ToString();
                    if (postfix == "jp")
                    {
                        var text_jp = value.Replace("\n", "").Replace(" ", "");
                        if (text_jp.Length != 0)
                        {
                            byte[] utf8 = Encoding.UTF8.GetBytes(text_jp);
                            var hash = xxhash.CalculateHash(utf8);
                            jobject.Add("Hash", hash);
                        }
                        if (IsGlobal)
                        {
                            continue;
                        }
                    }

                    jobject.Add(name, value);
                }
            }

            return jobject;
        }
    }
}
