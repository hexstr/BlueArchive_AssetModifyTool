using FlatDataGlobal;

namespace ExcelCore.Model
{
    public class Costume
    {
        public long CostumeGroupId { get; set; }
        public long CostumeUniqueId { get; set; }
        public string DevName { get; set; }
        public ProductionStep ProductionStep { get; set; }
        public bool IsDefault { get; set; }
        public bool CollectionVisible { get; set; }
        public string ReleaseDate { get; set; }
        public string CollectionVisibleStartDate { get; set; }
        public string CollectionVisibleEndDate { get; set; }
        public Rarity Rarity { get; set; }
        public long CharacterSkillListGroupId { get; set; }
        public string SpineResourceName { get; set; }
        public string SpineResourceNameDiorama { get; set; }
        public int SpineResourceNameDioramaForFormConversionLength { get; set; }
        public string[] SpineResourceNameDioramaForFormConversion { get; set; }
        public EntityMaterialType EntityMaterialType { get; set; }
        public string ModelPrefabName { get; set; }
        public string CafeModelPrefabName { get; set; }
        public string EchelonModelPrefabName { get; set; }
        public string StrategyModelPrefabName { get; set; }
        public string TextureDir { get; set; }
        public string CollectionTexturePath { get; set; }
        public string CollectionBGTexturePath { get; set; }
        public string CombatStyleTexturePath { get; }
        public bool UseObjectHPBAR { get; set; }
        public string TextureBoss { get; set; }
        public int TextureSkillCardLength { get; set; }
        public string[] TextureSkillCard { get; set; }
        public string InformationPacel { get; set; }
        public string AnimationSSR { get; set; }
        public string EnterStrategyAnimationName { get; set; }
        public bool AnimationValidator { get; set; }
        public long CharacterVoiceGroupId { get; set; }
    }
}
