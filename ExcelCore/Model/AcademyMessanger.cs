using FlatDataGlobal;

namespace ExcelCore.Model
{
    public class AcademyMessanger
    {
        public long MessageGroupId { get; set; }
        public long Id { get; set; }
        public long CharacterId { get; set; }
        public AcademyMessageConditions MessageCondition { get; set; }
        public long ConditionValue { get; set; }
        public long PreConditionGroupId { get; set; }
        public long PreConditionFavorScheduleId { get; set; }
        public long FavorScheduleId { get; set; }
        public long NextGroupId { get; set; }
        public long FeedbackTimeMillisec { get; set; }
        public AcademyMessageTypes MessageType { get; set; }
        public string ImagePath { get; set; }
        public string MessageKR { get; set; }
        public string MessageJP { get; set; }
        public string MessageTH { get; set; }
        public string MessageTW { get; set; }
        public string MessageEN { get; set; }
    }
}
