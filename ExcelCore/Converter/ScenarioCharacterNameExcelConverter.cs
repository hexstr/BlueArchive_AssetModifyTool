using ExcelCore.Db;
using ExcelCore.Model;
using FlatDataGlobal;
using Google.FlatBuffers;

namespace ExcelCore.Converter
{
    public static class ScenarioCharacterNameExcelConverter
    {
        public static List<object> Build(ScenarioCharacterName[] raw_data)
        {
            var result = new List<object>(raw_data.Length);
            var row_id = 1;
            foreach (var raw in raw_data)
            {
                var fbb = new FlatBufferBuilder(1024);

                var offset = ScenarioCharacterNameExcel.CreateScenarioCharacterNameExcel(fbb,
                    raw.CharacterName,
                    raw.ProductionStep,
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(raw.NameTW),
                    fbb.CreateString(raw.NicknameTW),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    raw.Shape,
                    fbb.CreateString(raw.SpinePrefabName),
                    fbb.CreateString(raw.SmallPortrait));
                fbb.Finish(offset.Value);

                var localized_item = new ScenarioCharacterNameDBSchema
                {
                    RowId = row_id++,
                    Bytes = fbb.SizedByteArray(),
                    CharacterName = raw.CharacterName
                };
                result.Add(localized_item);
            }

            return result;
        }
    }
}
