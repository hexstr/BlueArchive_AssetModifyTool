using ExcelCore.Db;
using ExcelCore.Model;
using FlatDataGlobal;
using Google.FlatBuffers;

namespace ExcelCore.Converter
{
    public static class LocalizeExcelConverter
    {
        public static List<object> Build(Localize[] raw_data)
        {
            var result = new List<object>(raw_data.Length);
            var row_id = 1;
            foreach (var raw in raw_data)
            {
                var fbb = new FlatBufferBuilder(1024);

                var offset = LocalizeExcel.CreateLocalizeExcel(fbb,
                    raw.Key,
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(raw.Tw),
                    fbb.CreateString(null));
                fbb.Finish(offset.Value);

                var localized_item = new LocalizeDBSchema
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
