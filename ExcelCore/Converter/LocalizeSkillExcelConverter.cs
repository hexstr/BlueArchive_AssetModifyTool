using ExcelCore.Db;
using ExcelCore.Model;
using FlatDataGlobal;
using Google.FlatBuffers;

namespace ExcelCore.Converter
{
    public static class LocalizeSkillExcelConverter
    {
        public static List<object> Build(LocalizeSkill[] raw_data)
        {
            var result = new List<object>(raw_data.Length);
            var row_id = 1;
            foreach (var raw in raw_data)
            {
                var fbb = new FlatBufferBuilder(1024);

                var offset = LocalizeSkillExcel.CreateLocalizeSkillExcel(fbb,
                    raw.Key,
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(raw.NameTw),
                    fbb.CreateString(raw.DescriptionTw),
                    fbb.CreateString(raw.SkillInvokeLocalizeTw),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null));
                fbb.Finish(offset.Value);

                var localized_item = new LocalizeSkillDBSchema
                {
                    RowId = row_id++,
                    Key = raw.Key,
                    Bytes = fbb.SizedByteArray()
                };
                result.Add(localized_item);
            }

            return result;
        }
    }
}
