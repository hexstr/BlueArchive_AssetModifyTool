namespace ExcelCore.Model
{
    public class CharacterDialog
    {
        public long CharacterId { get; set; }
        public long CostumeUniqueId { get; set; }
        public long DisplayOrder { get; set; }
        public int ProductionStep { get; set; }
        public int DialogCategory { get; set; }
        public int DialogCondition { get; set; }
        public int Anniversary { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
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
        public bool ApplyPosition { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public bool CollectionVisible { get; set; }
        public int CVCollectionType { get; set; }
        public long UnlockFavorRank { get; set; }
        public bool UnlockEquipWeapon { get; set; }
        public string LocalizeCVGroup { get; set; }
        public bool TeenMode { get; set; }
    }
}
