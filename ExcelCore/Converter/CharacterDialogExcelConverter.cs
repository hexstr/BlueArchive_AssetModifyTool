using ExcelCore.Db;
using ExcelCore.Model;
using FlatDataGlobal;
using Google.FlatBuffers;

namespace ExcelCore.Converter
{
    public static class CharacterDialogExcelConverter
    {
        public static List<object> Build(CharacterDialog[] raw_data)
        {
            var result = new List<object>(raw_data.Length);

            for (int i = 0; i < raw_data.Length; ++i)
            {
                FlatBufferBuilder fbb = new(1024);
                var raw = raw_data[i];
                var offset = CharacterDialogExcel.CreateCharacterDialogExcel(fbb,
                    raw.CharacterId,
                    raw.CostumeUniqueId,
                    raw.DisplayOrder,
                    (ProductionStep)raw.ProductionStep,
                    (DialogCategory)raw.DialogCategory,
                    (DialogCondition)raw.DialogCondition,
                    (Anniversary)raw.Anniversary,
                    fbb.CreateString(raw.StartDate),
                    fbb.CreateString(raw.EndDate),
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
                    raw.ApplyPosition,
                    raw.PosX,
                    raw.PosY,
                    raw.CollectionVisible,
                    (CVCollectionType)raw.CVCollectionType,
                    raw.UnlockFavorRank,
                    raw.UnlockEquipWeapon,
                    fbb.CreateString(raw.LocalizeCVGroup),
                    raw.TeenMode
                );
                fbb.Finish(offset.Value);

                var localized_item = new CharacterDialogDBSchema
                {
                    RowId = i + 1,
                    Bytes = fbb.SizedByteArray(),
                    CharacterId = raw.CharacterId,
                    DialogCategory = raw.DialogCategory,
                    DialogCondition = raw.DialogCondition,
                };
                result.Add(localized_item);
            }

            return result;
        }
    }
}
