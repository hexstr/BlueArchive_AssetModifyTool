namespace ExcelCore.Model
{
    public class ScenarioScript
    {
        public long GroupId { get; set; }
        public long SelectionGroup { get; set; }
        public long BGMId { get; set; }
        public string Sound { get; set; }
        public uint Transition { get; set; }
        public uint BGName { get; set; }
        public uint BGEffect { get; set; }
        public string PopupFileName { get; set; }
        public string ScriptKr { get; set; }
        public string TextJp { get; set; }
        public string TextTh { get; set; }
        public string TextTw { get; set; }
        public string TextEn { get; set; }
        public uint VoiceId { get; set; }
        public bool TeenMode { get; set; }
    }
}
