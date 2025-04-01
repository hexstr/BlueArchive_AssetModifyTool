namespace ExcelCore.Model
{
    public class Voice
    {
        public long UniqueId { get; set; }
        public uint Id { get; set; }
        public FlatDataGlobal.Nation[] Nation { get; set; }
        public string[] Path { get; set; }
        public float[] Volume { get; set; }
    }
}
