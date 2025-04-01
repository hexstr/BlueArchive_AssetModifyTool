namespace ExcelCore.Model
{
    public class CharacterSkillList
    {
        public long CharacterSkillListGroupId { get; set; }
        public int MinimumGradeCharacterWeapon { get; set; }
        public int MinimumTierCharacterGear { get; set; }
        public int FormIndex { get; set; }
        public bool IsRootMotion { get; set; }
        public bool IsMoveLeftRight { get; set; }
        public bool UseRandomAnimation { get; set; }
        public long TSAInteractionId { get; set; }
        public int NormalSkillGroupIdLength { get; set; }
        public string[] NormalSkillGroupId { get; set; }
        public int NormalSkillTimeLineIndexLength { get; set; }
        public int[] NormalSkillTimeLineIndex { get; set; }
        public int ExSkillGroupIdLength { get; set; }
        public string[] ExSkillGroupId { get; set; }
        public int ExSkillCutInTimeLineIndexLength { get; set; }
        public string[] ExSkillCutInTimeLineIndex { get; set; }
        public int ExSkillLevelTimeLineIndexLength { get; set; }
        public string[] ExSkillLevelTimeLineIndex { get; set; }
        public int PublicSkillGroupIdLength { get; set; }
        public string[] PublicSkillGroupId { get; set; }
        public int PublicSkillTimeLineIndexLength { get; set; }
        public int[] PublicSkillTimeLineIndex { get; set; }
        public int PassiveSkillGroupIdLength { get; set; }
        public string[] PassiveSkillGroupId { get; set; }
        public int LeaderSkillGroupIdLength { get; set; }
        public string[] LeaderSkillGroupId { get; set; }
        public int ExtraPassiveSkillGroupIdLength { get; set; }
        public string[] ExtraPassiveSkillGroupId { get; set; }
        public int HiddenPassiveSkillGroupIdLength { get; set; }
        public string[] HiddenPassiveSkillGroupId { get; set; }
    }
}
