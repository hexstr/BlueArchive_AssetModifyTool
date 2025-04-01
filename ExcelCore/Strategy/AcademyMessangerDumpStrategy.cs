using ExcelCore.Cryptography;
using System.Text.Json.Nodes;

namespace ExcelCore.Strategy
{
    public class AcademyMessangerDumpStrategy : IDataDumpStrategy
    {
        public AcademyMessangerDumpStrategy(bool is_global)
        {
            IsGlobal = is_global;
        }

        public override JsonObject Dump(object data_list, NormalTable dumper)
        {
            var base_jobject = base.Dump(data_list, dumper);
            var jobject = new JsonObject();

            foreach (var obj in base_jobject)
            {
                if (obj.Key == "MessageGroupId")
                {
                    jobject.Add(obj.Key, obj.Value.DeepClone());
                }

                if (IsGlobal)
                {
                    if (obj.Key == "MessageTW")
                    {
                        jobject.Add("Message", obj.Value.DeepClone());
                    }
                }
                else if (obj.Key.StartsWith("MessageJP"))
                {
                    jobject.Add("Message", obj.Value.DeepClone());
                }
            }

            return jobject;
        }
    }
}
