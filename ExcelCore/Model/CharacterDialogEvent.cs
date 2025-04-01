namespace ExcelCore.Model
{
    public class CharacterDialogEvent
    {
        public long CostumeUniqueId { get; set; }
        public long OriginalCharacterId { get; set; }
        public long DisplayOrder { get; set; }
        public long EventID { get; set; }
        public int ProductionStep { get; set; }
        public int DialogCategory { get; set; }
        public int DialogCondition { get; set; }
        public int DialogConditionDetail { get; set; }
        public long DialogConditionDetailValue { get; set; }
        public long GroupId { get; set; }
        public int DialogType { get; set; }
        public string ActionName { get; set; }
        public long Duration { get; set; }
        public long DurationKr { get; set; }
        public string AnimationName { get; set; }
        public string LocalizeKR { get; set; }
        public string LocalizeJP { get; set; }
        public string LocalizeTH { get; set; }
        public string LocalizeTW { get; set; }
        public string LocalizeEN { get; set; }
        public uint[] VoiceId { get; set; }
        public int VoiceIdLength { get; set; }
        public bool CollectionVisible { get; set; }
        public int CVCollectionType { get; set; }
        public long UnlockEventSeason { get; set; }
        public long ScenarioGroupId { get; set; }
        public string LocalizeCVGroup { get; set; }
    }
}
