using CommandLine;
using ExcelCore.Catalog;
using ExcelCore.Db;
using ExcelCore.Excel;
using ExcelCore.Utility;
using System.Net.Http.Json;
using System.Text.Json;

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

            [Option('b', "build", Required = false, HelpText = "Build byte files.")]
            public bool IsBuild { get; set; }

            [Option('s', "sql", Required = false, HelpText = "Build new sqlite database.")]
            public string IsBuildDatabase { get; set; }

            [Option("test", Required = false, HelpText = "Test built byte files.")]
            public string IsTest { get; set; }

            [Option('e', "excel", Required = false, HelpText = "Build excel file.")]
            public string IsExcel { get; set; }

            [Option('g', "global", Required = false, HelpText = "Global version.")]
            public string IsGlobal { get; set; }

            [Option('j', "jp", Required = false, HelpText = "Japan version.")]
            public string IsJp { get; set; }

            [Option('r', "raw", Required = false, HelpText = "Dump raw assets.")]
            public bool IsRaw { get; set; }

            [Option('p', "pack", Required = false, HelpText = "Pack Localized files.")]
            public string IsPack { get; set; }

            [Option('u', "url", Required = false, HelpText = "Extract download url from .obb.")]
            public string IsExtractUrl { get; set; }

            [Option('o', "obb", Required = false, HelpText = "Download obb file.")]
            public bool IsDownloadObb { get; set; }

            [Option('q', "qoo", Required = false, HelpText = "Output download info for Jp version.")]
            public bool IsOutputInfo { get; set; }

            [Option('f', "fbs", Required = false, HelpText = "Extract fbs from dll file.")]
            public string IsExtractFbs { get; set; }

            [Option('t', "t2s", Required = false, HelpText = "Convert traditional to simplified.")]
            public bool IsT2S { get; set; }

            [Option('d', "download", Required = false, HelpText = "Download the latest Exce.zip file.")]
            public bool IsDownloadExcel { get; set; }

            [Option("dump", Required = false, HelpText = "Dump excel table name.")]
            public string IsDumpExcelTableName { get; set; }

            [Option("obfs", Required = false, HelpText = "Dump obfuscated class name.")]
            public string IsObfuscatedName { get; set; }

            [Option("rec", Required = false, HelpText = "Trying to recover obfuscated class name.")]
            public bool IsRecoverObfuscatedName { get; set; }
        }

        private static void DumpExcel(bool is_global, bool is_raw, string excel_path)
        {
            if (string.IsNullOrEmpty(excel_path))
            {
                throw new ArgumentNullException(nameof(excel_path));
            }

            var input_name_list = new List<string> { excel_path };
            if (excel_path == "null")
            {
                input_name_list[0] = "Excel.zip";
                input_name_list.Add("ExcelDB.db");
            }

            foreach (var input_file in input_name_list)
            {
                Console.WriteLine($"Reading from: {input_file}");

                if (input_file.EndsWith(".zip"))
                {
                    var excel_reader = new ExcelReader(input_file, is_global);
                    excel_reader.Dump(is_raw);
                }
                else if (input_file.EndsWith(".db"))
                {
                    var excel_context = new ExcelContext(input_file);
                    var excel_reader = new DatabaseReader(excel_context, is_global);
                    excel_reader.Dump(is_raw);
                }
                else
                {
                    throw new Exception("Invalid file format.");
                }
            }
        }

        private static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(async o =>
                {
                    opencc.Enable = o.IsT2S || o.IsBuild;
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
                    else if (!string.IsNullOrEmpty(o.IsExtractFbs))
                    {
                        dll_analyzer.ExtractFbs(o.IsExtractFbs);
                    }
                    else if (!string.IsNullOrEmpty(o.IsPack))
                    {
                        if (Directory.Exists(o.IsPack))
                        {
                            ScriptPacker.Pack(o.IsPack);
                        }
                        else
                        {
                            Console.WriteLine($"Dir {o.IsPack} not exists.");
                        }
                    }
                    else if (o.IsOutputInfo)
                    {
                        qoo_downloader downloader = new();
                        await downloader.Init();
                        var app_info = await downloader.GetAppInfo("com.YostarJP.BlueArchive", 22, ["arm64-v8a"]);
                        var apk_info = app_info["apk"].AsObject();
                        var qoo_app_id = (int)app_info["id"];
                        var version_code = (int)apk_info["version_code"];
                        var obb_filename = $"main.{version_code}.com.YostarJP.BlueArchive.obb";
                        var obb_link = $"https://d.qoo-apk.com/{qoo_app_id}/obb/{obb_filename}";
                        Console.WriteLine($"version_code: {version_code}");
                        Console.WriteLine($"obb_filename: {obb_filename}");
                        Console.WriteLine($"obb_link: {obb_link}");
                    }
                    else if (!string.IsNullOrEmpty(o.IsExtractUrl))
                    {
                        var server_info_data_url = config_dumper.Read(o.IsExtractUrl);
                        Console.WriteLine($"ServerInfoDataUrl: {server_info_data_url}");
                        var client = new HttpClient();
                        client.DefaultRequestHeaders.UserAgent.ParseAdd("BestHTTP/2 v2.4.0");
                        var server_data = await client.GetFromJsonAsync<ServerData>(server_info_data_url);
                        Console.WriteLine($"BundleVersion: {server_data.ConnectionGroups[0].BundleVersion}");
                        var latest_connection_group = server_data.ConnectionGroups[0].OverrideConnectionGroups[^1];
                        Console.WriteLine($"Name: {latest_connection_group.Name}");
                        Console.WriteLine($"Url: {latest_connection_group.AddressablesCatalogUrlRoot}");
                        Console.WriteLine($"Excel: {latest_connection_group.AddressablesCatalogUrlRoot}/TableBundles/Excel.zip");
                        Console.WriteLine($"Excel: {latest_connection_group.AddressablesCatalogUrlRoot}/TableBundles/ExcelDB.db");
                    }
                    else if (o.IsDownloadObb)
                    {
                        qoo_downloader downloader = new();
                        await downloader.Init();
                        await downloader.DownloadObb();
                    }
                    else if (o.IsT2S)
                    {
                        Directory.CreateDirectory("t2s");
                        var tasks = new List<Task>();

                        FileNameDict.name_pair["CharacterDialogExcel"] = "CharacterDialogExcel";
                        FileNameDict.name_pair["CharacterDialogEventExcel"] = "CharacterDialogEventExcel";
                        FileNameDict.name_pair["LocalizeErrorExcel"] = "LocalizeErrorExcel";
                        FileNameDict.name_pair["LocalizeExcel"] = "LocalizeExcel";
                        FileNameDict.name_pair["LocalizeEtcExcel"] = "LocalizeEtcExcel";
                        FileNameDict.name_pair["LocalizeSkillExcel"] = "LocalizeSkillExcel";
                        FileNameDict.name_pair["ScenarioCharacterNameExcel"] = "ScenarioCharacterNameExcel";
                        FileNameDict.name_pair["ScenarioScriptExcel"] = "ScenarioScriptExcel";

                        foreach (var file_pair in FileNameDict.name_pair)
                        {
                            var file_name = Path.Join("dump", $"{file_pair.Value}.json");
                            if (File.Exists(file_name))
                            {
                                Console.WriteLine($"Converting {file_name}");
                                tasks.Add(ExcelDumper.DumpFromJson(file_name));
                            }
                            else
                            {
                                Console.WriteLine($"{file_name} not exists.");
                            }
                        }
                        Task.WaitAll([.. tasks]);
                    }
                    else if (o.IsDownloadExcel)
                    {
                        var downloader = new ResourceDownloader();
                        await downloader.GetLatestVersionAsync();
                        await downloader.DownloadExcel();
                    }
                    else if (o.IsBuild)
                    {
                        if (Directory.Exists("build"))
                        {
                            Directory.Delete("build", true);
                        }
                        Directory.CreateDirectory("build");

                        string[] buildable_type_list = ["AcademyMessanger1ExcelTable", "AcademyMessanger2ExcelTable",
                            "CostumeExcelTable", "LocalizeCharProfileExcelTable", "CharacterSkillListExcelTable", "CharacterExcelTable"];
                        var assembly = typeof(ExcelDumper).Assembly;

                        foreach (var buildable_type_name in buildable_type_list)
                        {
                            if (File.Exists($"raw/{buildable_type_name}.json"))
                            {
                                var pure_name = buildable_type_name.Replace("1", "").Replace("2", "");
                                var builder_name = $"ExcelCore.Builder.{pure_name}Builder";
                                var builder_type = assembly.GetType(builder_name);
                                var create_method = builder_type.GetMethod("Create");
                                var deserialization_name = pure_name.Replace("ExcelTable", "");
                                var deserialization_type = assembly.GetType($"ExcelCore.Model.{deserialization_name}");
                                var deserialization_array_type = deserialization_type.MakeArrayType();

                                var list_type = typeof(List<>).MakeGenericType(deserialization_type);
                                var merged_data = Activator.CreateInstance(list_type);
                                var add_range_method = list_type.GetMethod("AddRange");
                                var to_array_method = list_type.GetMethod("ToArray");

                                if (File.Exists($"add/{buildable_type_name}.json"))
                                {
                                    Console.WriteLine($"Adding: {buildable_type_name}");
                                    var add_text = File.ReadAllText(@$"add/{buildable_type_name}.json");
                                    var add_raw_data = JsonSerializer.Deserialize(add_text, deserialization_array_type);
                                    add_range_method.Invoke(merged_data, [add_raw_data]);
                                }

                                Console.WriteLine($"Building: {buildable_type_name}");
                                var text = File.ReadAllText(@$"raw/{buildable_type_name}.json");
                                var raw_data = JsonSerializer.Deserialize(text, deserialization_array_type);
                                add_range_method.Invoke(merged_data, [raw_data]);

                                var result = to_array_method.Invoke(merged_data, []);

                                object data = null;
                                if (pure_name == "AcademyMessangerExcelTable")
                                {
                                    data = create_method.Invoke(Activator.CreateInstance(builder_type, [buildable_type_name]), [result]);
                                }
                                else
                                {
                                    data = create_method.Invoke(Activator.CreateInstance(builder_type), [result]);
                                }

                                File.WriteAllBytes($"build/{buildable_type_name}.bytes".ToLower(), (byte[])data);
                            }
                        }

                        return;
                    }
                    else if (!string.IsNullOrEmpty(o.IsBuildDatabase))
                    {
                        var new_file = o.IsBuildDatabase + DateTime.Now.Date.ToString("yyyyMMdd");
                        File.Copy(o.IsBuildDatabase, new_file);
                        DatabaseWriter.Write(new_file);
                        return;
                    }
                    else if (!string.IsNullOrEmpty(o.IsExcel))
                    {
                        var new_file = o.IsExcel + DateTime.Now.Date.ToString("yyyyMMdd");
                        File.Copy(o.IsExcel, new_file);
                        var excel_reader = new ExcelReader(new_file, true);
                        excel_reader.UpdateFiles(Directory.EnumerateFiles("build", "*.bytes"));
                    }
                    else if (!string.IsNullOrEmpty(o.IsDumpExcelTableName))
                    {
                        dll_analyzer.DumpExcelTable(o.IsDumpExcelTableName);
                    }
                    else if (o.IsRecoverObfuscatedName)
                    {
                        dll_analyzer.RecoverObfuscatedClass();
                    }
                    else if (!string.IsNullOrEmpty(o.IsTest))
                    {
                        Directory.CreateDirectory("test");
                        var file_name = Path.GetFileNameWithoutExtension(o.IsTest);
                        var excel_dumper = new ExcelDumper(true);
                        if (FileNameDict.name_pair.TryGetValue(file_name, out string class_name) == true)
                        {
                            await excel_dumper.Dump(class_name, true, o.IsTest, null, "test");
                        }
                    }
                });
        }
    }
}