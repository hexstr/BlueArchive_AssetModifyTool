using ExcelCore.Db;
using ExcelCore.Model;
using FlatDataGlobal;
using Google.FlatBuffers;

namespace ExcelCore.Converter
{
    public static class ScenarioScriptExcelConverter
    {
        public static List<object> Build(ScenarioScript[] raw_data)
        {
            var result = new List<object>(raw_data.Length);
            var row_id = 1;
            foreach (var raw in raw_data)
            {
                var fbb = new FlatBufferBuilder(1024);

                var offset = ScenarioScriptExcel.CreateScenarioScriptExcel(fbb,
                    raw.GroupId,
                    raw.SelectionGroup,
                    raw.BGMId,
                    fbb.CreateString(raw.Sound),
                    raw.Transition,
                    raw.BGName,
                    raw.BGEffect,
                    fbb.CreateString(raw.PopupFileName),
                    fbb.CreateString(raw.ScriptKr),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(raw.TextTw),
                    fbb.CreateString(null),
                    raw.VoiceId,
                    raw.TeenMode);
                fbb.Finish(offset.Value);

                var localized_item = new ScenarioScriptDBSchema
                {
                    RowId = row_id++,
                    Bytes = fbb.SizedByteArray(),
                    GroupId = raw.GroupId
                };
                result.Add(localized_item);
            }

            return result;
        }
    }
}
