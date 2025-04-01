using MemoryPack;

namespace ExcelCore.Model
{
    [MemoryPackable]
    public partial class MediaCatalog
    {
        public Dictionary<string, Media> Table { get; set; }
        public Dictionary<string, Media> Catalog { get; set; }
    }
}
