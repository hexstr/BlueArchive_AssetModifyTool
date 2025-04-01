using FlatDataGlobal;

namespace ExcelCore.Model
{
    public class Character
    {
        public long Id { get; set; }
        public string DevName { get; set; }
        public long CostumeGroupId { get; set; }
        public bool IsPlayable { get; set; }
        public int ProductionStep { get; set; }
        public bool CollectionVisible { get; set; }
        public string ReleaseDate { get; set; }
        public string CollectionVisibleStartDate { get; set; }
        public string CollectionVisibleEndDate { get; set; }
        public bool IsPlayableCharacter { get; set; }
        public uint LocalizeEtcId { get; set; }
        public int Rarity { get; set; }
        public bool IsNPC { get; set; }
        public int TacticEntityType { get; set; }
        public bool CanSurvive { get; set; }
        public bool IsDummy { get; set; }
        public int SubPartsCount { get; set; }
        public int TacticRole { get; set; }
        public int WeaponType { get; set; }
        public int TacticRange { get; set; }
        public int BulletType { get; set; }
        public int ArmorType { get; set; }
        public int AimIKType { get; set; }
        public int School { get; set; }
        public int Club { get; set; }
        public int DefaultStarGrade { get; set; }
        public int MaxStarGrade { get; set; }
        public int StatLevelUpType { get; set; }
        public int SquadType { get; set; }
        public bool Jumpable { get; set; }
        public long PersonalityId { get; set; }
        public long CharacterAIId { get; set; }
        public long ExternalBTId { get; set; }
        public long MainCombatStyleId { get; }
        public int CombatStyleIndex { get; }
        public string ScenarioCharacter { get; set; }
        public uint SpawnTemplateId { get; set; }
        public int FavorLevelupType { get; set; }
        public int EquipmentSlotLength { get; set; }
        public EquipmentCategory[] EquipmentSlot { get; set; }
        public uint WeaponLocalizeId { get; set; }
        public bool DisplayEnemyInfo { get; set; }
        public long BodyRadius { get; set; }
        public long RandomEffectRadius { get; set; }
        public bool HPBarHide { get; set; }
        public float HpBarHeight { get; set; }
        public float HighlightFloaterHeight { get; set; }
        public float EmojiOffsetX { get; set; }
        public float EmojiOffsetY { get; set; }
        public int MoveStartFrame { get; set; }
        public int MoveEndFrame { get; set; }
        public int JumpMotionFrame { get; set; }
        public int AppearFrame { get; set; }
        public bool CanMove { get; set; }
        public bool CanFix { get; set; }
        public bool CanCrowdControl { get; set; }
        public bool CanBattleItemMove { get; set; }
        public bool IsAirUnit { get; set; }
        public long AirUnitHeight { get; set; }
        public int TagsLength { get; set; }
        public Tag[] Tags { get; set; }
        public long SecretStoneItemId { get; set; }
        public int SecretStoneItemAmount { get; set; }
        public long CharacterPieceItemId { get; set; }
        public int CharacterPieceItemAmount { get; set; }
        public long CombineRecipeId { get; set; }
    }
}
