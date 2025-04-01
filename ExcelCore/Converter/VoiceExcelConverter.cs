using ExcelCore.Db;
using ExcelCore.Model;
using FlatDataGlobal;
using Google.FlatBuffers;

namespace ExcelCore.Converter
{
    public static class VoiceExcelConverter
    {
        public static List<object> Build(Voice[] raw_data)
        {
            var result = new List<object>(raw_data.Length);
            var row_id = 1;
            foreach (var raw in raw_data)
            {
                var fbb = new FlatBufferBuilder(1024);

                var path_offset = new StringOffset[raw.Path.Length];
                for (int i = 0; i < raw.Path.Length; ++i)
                {
                    path_offset[i] = fbb.CreateString(raw.Path[i]);
                }

                var offset = VoiceExcel.CreateVoiceExcel(fbb,
                    raw.UniqueId,
                    raw.Id,
                    VoiceExcel.CreateNationVector(fbb, raw.Nation),
                    VoiceExcel.CreatePathVector(fbb, path_offset),
                    VoiceExcel.CreateVolumeVector(fbb, raw.Volume));
                fbb.Finish(offset.Value);

                var localized_item = new VoiceDBSchema
                {
                    RowId = row_id++,
                    Id = raw.Id,
                    Bytes = fbb.SizedByteArray()
                };
                result.Add(localized_item);
            }

            return result;
        }
    }
}
