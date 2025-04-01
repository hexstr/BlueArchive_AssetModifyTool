using ExcelCore.Db;
using ExcelCore.Model;
using FlatDataGlobal;
using Google.FlatBuffers;

namespace ExcelCore.Converter
{
    public static class CharacterDialogEventExcelConverter
    {
        public static List<object> Build(CharacterDialogEvent[] raw_data)
        {
            var result = new List<object>(raw_data.Length);

            for (int i = 0; i < raw_data.Length; ++i)
            {
                FlatBufferBuilder fbb = new(1024);
                var raw = raw_data[i];
                var offset = CharacterDialogEventExcel.CreateCharacterDialogEventExcel(fbb,
                    raw.CostumeUniqueId,
                    raw.OriginalCharacterId,
                    raw.DisplayOrder,
                    raw.EventID,
                    (ProductionStep)raw.ProductionStep,
                    (DialogCategory)raw.DialogCategory,
                    (DialogCondition)raw.DialogCondition,
                    (DialogConditionDetail)raw.DialogConditionDetail,
                    raw.DialogConditionDetailValue,
                    raw.GroupId,
                    (DialogType)raw.DialogType,
                    fbb.CreateString(raw.ActionName),
                    raw.Duration,
                    raw.DurationKr,
                    fbb.CreateString(raw.AnimationName),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(null),
                    fbb.CreateString(raw.LocalizeTW),
                    fbb.CreateString(null),
                    CharacterDialogEventExcel.CreateVoiceIdVector(fbb, raw.VoiceId),
                    raw.CollectionVisible,
                    (CVCollectionType)raw.CVCollectionType,
                    raw.UnlockEventSeason,
                    raw.ScenarioGroupId,
                    fbb.CreateString(raw.LocalizeCVGroup)
                );
                fbb.Finish(offset.Value);

                var localized_item = new CharacterDialogEventDBSchema
                {
                    RowId = i + 1,
                    Bytes = fbb.SizedByteArray(),
                    CostumeUniqueId = raw.CostumeUniqueId,
                    OriginalCharacterId = raw.OriginalCharacterId,
                    EventID = raw.EventID,
                    DialogCategory = raw.DialogCategory,
                    DialogCondition = raw.DialogCondition
                };
                result.Add(localized_item);
            }

            return result;
        }
    }
}
