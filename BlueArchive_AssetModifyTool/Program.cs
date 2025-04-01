using CommandLine;
using ExcelCore.Catalog;
using ExcelCore.Db;
using ExcelCore.Utility;

namespace ExcelExtractor
{
    internal class Program
    {
        public class Options
        {
            [Option('c', "catalog", Required = false, HelpText = "Dump catalog file.")]
            public string IsCatalog { get; set; }

            [Option("bc", Required = false, HelpText = "Build catalog file.")]
            public string IsBuildCatalog { get; set; }

            [Option('s', "sql", Required = false, HelpText = "Build new sqlite database.")]
            public string IsBuildDatabase { get; set; }

            [Option('g', "global", Required = false, HelpText = "Global version.")]
            public string IsGlobal { get; set; }

            [Option('j', "jp", Required = false, HelpText = "Japan version.")]
            public string IsJp { get; set; }

            [Option('r', "raw", Required = false, HelpText = "Dump raw assets.")]
            public bool IsRaw { get; set; }

            [Option('t', "t2s", Required = false, HelpText = "Convert traditional to simplified.")]
            public bool IsT2S { get; set; }

            [Option('d', "download", Required = false, HelpText = "Download the latest Exce.zip file.")]
            public bool IsDownloadExcel { get; set; }
        }

        private static void DumpExcel(bool is_global, bool is_raw, string excel_path)
        {
            if (string.IsNullOrEmpty(excel_path))
            {
                throw new ArgumentNullException(nameof(excel_path));
            }

            if (excel_path == "null")
            {
                excel_path = "ExcelDB.db";
            }

            Console.WriteLine($"Reading from: {excel_path}");

            if (excel_path.EndsWith(".db"))
            {
                var excel_context = new ExcelContext(excel_path);
                var excel_reader = new DatabaseReader(excel_context, is_global);
                excel_reader.Dump(is_raw);
            }
            else
            {
                throw new Exception("Invalid file format.");
            }
        }

        private static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(async o =>
                {
                    opencc.Enable = o.IsT2S;
                    if (!string.IsNullOrEmpty(o.IsGlobal))
                    {
                        DumpExcel(true, o.IsRaw, o.IsGlobal);
                    }
                    else if (!string.IsNullOrEmpty(o.IsJp))
                    {
                        DumpExcel(false, o.IsRaw, o.IsJp);
                    }
                    else if (!string.IsNullOrEmpty(o.IsCatalog))
                    {
                        var catalog = MediaSerializer.Deserialize(o.IsCatalog);
                        MediaSerializer.Dump(catalog);
                    }
                    else if (!string.IsNullOrEmpty(o.IsBuildCatalog))
                    {
                        var catalog = MediaSerializer.Serialize(o.IsBuildCatalog);
                        MediaSerializer.Write(catalog);
                    }
                    else if (o.IsDownloadExcel)
                    {
                        var downloader = new ResourceDownloader();
                        await downloader.GetLatestVersionAsync();
                        await downloader.DownloadExcel();
                    }
                    else if (!string.IsNullOrEmpty(o.IsBuildDatabase))
                    {
                        var new_file = o.IsBuildDatabase + DateTime.Now.Date.ToString("yyyyMMdd");
                        File.Copy(o.IsBuildDatabase, new_file);
                        DatabaseWriter.Write(new_file);
                        return;
                    }
                });
        }
    }
}