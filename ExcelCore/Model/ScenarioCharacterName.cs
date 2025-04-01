using FlatDataGlobal;

namespace ExcelCore.Model
{
    public class ScenarioCharacterName
    {
        public uint CharacterName { get; set; }
        public ProductionStep ProductionStep { get; set; }
        public string NameKR { get; set; }
        public string NicknameKR { get; set; }
        public string NameJP { get; set; }
        public string NicknameJP { get; set; }
        public string NameTH { get; set; }
        public string NicknameTH { get; set; }
        public string NameTW { get; set; }
        public string NicknameTW { get; set; }
        public string NameEN { get; set; }
        public string NicknameEN { get; set; }
        public ScenarioCharacterShapes Shape { get; set; }
        public string SpinePrefabName { get; set; }
        public string SmallPortrait { get; set; }
    }
}
