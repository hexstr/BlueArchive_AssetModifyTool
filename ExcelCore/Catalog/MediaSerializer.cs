using ExcelCore.Cryptography;
using ExcelCore.Model;
using ExcelCore.Utility;
using MemoryPack;
using System.Text.Json;

namespace ExcelCore.Catalog
{
    public class MediaSerializer
    {
        public static MediaCatalog Deserialize(string file_name)
        {
            var buffer = File.ReadAllBytes(file_name);
            return MemoryPackSerializer.Deserialize<MediaCatalog>(buffer);
        }

        public static byte[] Serialize(string file_name)
        {
            var json_data = File.ReadAllText(file_name);
            var catalog = JsonSerializer.Deserialize<Dictionary<string, Media>>(json_data);
            if (JsonUtils.TryUnmarshal<Dictionary<string, Media>>("add/Table.json", out var add_data))
            {
                foreach (var add_item in add_data)
                {
                    if (catalog.ContainsKey(add_item.Key))
                    {
                        catalog[add_item.Key] = add_item.Value;
                    }
                    else
                    {
                        catalog.Add(add_item.Key, add_item.Value);
                    }
                }
            }
            var media_catalog = new MediaCatalog
            {
                Catalog = [],
                Table = catalog
            };
            return MemoryPackSerializer.Serialize(media_catalog);
        }

        public static void Dump(MediaCatalog catalog)
        {
            Directory.CreateDirectory("catalog");
            if (catalog.Catalog.Count > 0)
            {
                JsonUtils.TryMarshal(catalog.Catalog, @"catalog/Catalog.json");
            }
            if (catalog.Table.Count > 0)
            {
                JsonUtils.TryMarshal(catalog.Table, @"catalog/Table.json");
            }
        }

        public static void Write(byte[] data)
        {
            Directory.CreateDirectory("build_catalog");
            File.WriteAllBytes("build_catalog/MediaCatalog.bytes", data);
            var hash = xxhash.CalculateHash(data);
            File.WriteAllText("build_catalog/MediaCatalog.hash", $"{hash}\r\n");
        }
    }
}
