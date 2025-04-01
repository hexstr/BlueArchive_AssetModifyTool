using ExcelCore.Utility;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ExcelExtractor
{
    internal class NxVersionRequest
    {
        [JsonPropertyName("market_game_id")]
        public string MarketGameId { get; set; }

        [JsonPropertyName("curr_build_version")]
        public string CurrVer { get; set; }

        [JsonPropertyName("curr_build_number")]
        public string CurrVerNum { get; set; }

        [JsonPropertyName("market_code")]
        public string MarketCode { get; set; }
    }

    internal class NxVersionResponse
    {
        [JsonPropertyName("market_game_id")]
        public string MarketGameId { get; set; }

        [JsonPropertyName("latest_build_version")]
        public string CurrVer { get; set; }

        [JsonPropertyName("latest_build_number")]
        public string CurrVerNum { get; set; }

        [JsonPropertyName("patch")]
        public PatchData Patch { get; set; }

        internal class PatchData
        {
            [JsonPropertyName("patch_version")]
            public int PatchVersion { get; set; }

            [JsonPropertyName("resource_path")]
            public string ResourcePath { get; set; }
        }
    }

    internal class VersionResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("market_game_id")]
        public string MarketGameID { get; set; }

        [JsonPropertyName("build_id")]
        public long[] BuildID { get; set; }

        [JsonPropertyName("patch_version")]
        public long PatchVersion { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("patch_state")]
        public string PatchState { get; set; }

        [JsonPropertyName("security_checked")]
        public bool SecurityChecked { get; set; }

        [JsonPropertyName("multi_language")]
        public bool MultiLanguage { get; set; }

        [JsonPropertyName("multi_texture_encode")]
        public bool MultiTextureEncode { get; set; }

        [JsonPropertyName("multi_texture_quality")]
        public bool MultiTextureQuality { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("register")]
        public string Register { get; set; }

        [JsonPropertyName("register_date")]
        public string RegisterDate { get; set; }

        [JsonPropertyName("updater")]
        public string Updater { get; set; }

        [JsonPropertyName("update_date")]
        public string UpdateDate { get; set; }

        [JsonPropertyName("compress")]
        public bool Compress { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("use_multi_resource")]
        public bool UseMultiResource { get; set; }

        [JsonPropertyName("category")]
        public Category Category { get; set; }

        [JsonPropertyName("category_mapping")]
        public CategoryMap[] CategoryMapping { get; set; }

        [JsonPropertyName("resources")]
        public Resource[] Resources { get; set; }
    }

    internal class Category
    {
        [JsonPropertyName("group")]
        public string[] Group { get; set; }
    }

    internal class CategoryMap
    {
        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("paths")]
        public string[] Paths { get; set; }
    }

    internal class Resource
    {
        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("resource_path")]
        public string ResourcePath { get; set; }

        [JsonPropertyName("resource_size")]
        public long ResourceSize { get; set; }

        [JsonPropertyName("resource_hash")]
        public string ResourceHash { get; set; }
    }

    internal class ResourceDownloader
    {
        private const string market_game_id = "com.nexon.bluearchive";
        public string CurrBuildVersion { get; set; } = "1.63.277251";
        public string CurrBuildNumber { get; set; } = "277251";
        private const string market_code = "playstore";
        private const string api_url = "https://api-pub.nexon.com/patch/v1.1/version-check";
        public NxVersionResponse LatestResponse = null;
        public Resource[] ResourceFiles = [];

        public ResourceDownloader()
        {
            JsonUtils.TryUnmarshal("info.json", out LatestResponse);

            if (!JsonUtils.TryUnmarshal("hash_info.json", out ResourceFiles))
            {
                ResourceFiles = [];
            }
        }

        public async Task GetLatestVersionAsync()
        {
            var new_request = new NxVersionRequest()
            {
                MarketGameId = market_game_id,
                MarketCode = market_code,
                CurrVer = string.IsNullOrEmpty(LatestResponse?.CurrVer) ? CurrBuildVersion : LatestResponse?.CurrVer,
                CurrVerNum = string.IsNullOrEmpty(LatestResponse?.CurrVerNum) ? CurrBuildNumber : LatestResponse?.CurrVerNum,
            };

            var client = new HttpClient();
            var rsp = await client.PostAsJsonAsync(api_url, new_request);
            rsp.EnsureSuccessStatusCode();
            var result = await rsp.Content.ReadFromJsonAsync<NxVersionResponse>();

            if (LatestResponse == null
                || result.CurrVer != LatestResponse.CurrVer
                || result.CurrVerNum != LatestResponse.CurrVerNum
                || result.Patch.PatchVersion != LatestResponse.Patch.PatchVersion)
            {
                Console.WriteLine($"[GetLatestVersion] new patch version: {result.Patch.PatchVersion}");
                LatestResponse = result;
                JsonUtils.TryMarshal(result, "info.json");
                await GetLatestVersionAsync();
                return;
            }

            Console.WriteLine($"[GetLatestVersion] resource_path: {result.Patch.ResourcePath}");
        }

        public async Task DownloadExcel()
        {
            if (LatestResponse != null)
            {
                var resource_path = LatestResponse.Patch.ResourcePath;
                var last_slash_index = resource_path.LastIndexOf('/');
                var main_path = resource_path[..last_slash_index];

                Console.WriteLine($"[DownloadExcel] main_path: {main_path}");

                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromMinutes(5),
                };

                var rsp = await client.GetFromJsonAsync<VersionResponse>(resource_path);

                var remote_files = rsp.Resources.Where(x => x.ResourcePath.Contains("TableBundles/ExcelDB")).ToArray();

                var new_files = remote_files.ExceptBy(ResourceFiles.Select(x => x.ResourceHash), y => y.ResourceHash).ToArray();

                if (new_files.Length == 0)
                {
                    Console.WriteLine("[DownloadExcel] they have the same hash.");
                    return;
                }

                ResourceFiles = remote_files;

                foreach (var resource in new_files)
                {
                    Console.WriteLine($"[DownloadExcel] new file hash {resource.ResourceHash}");
                    var excel_url = $"{main_path}/{resource.ResourcePath}";
                    var file_name = Path.GetFileName(resource.ResourcePath);
                    Console.WriteLine($"[DownloadExcel] {file_name} {excel_url}");

                    try
                    {
                        var response = await client.GetAsync(excel_url);
                        if (response.IsSuccessStatusCode == false)
                        {
                            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                            return;
                        }

                        using (var input_stream = await response.Content.ReadAsStreamAsync())
                        {
                            using (var output_stream = File.OpenWrite(file_name))
                            {
                                await input_stream.CopyToAsync(output_stream);
                            }
                        }
                        Console.WriteLine($"{file_name} saved successfully.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[DownloadExcel] download error: {e.Message}");
                        return;
                    }
                }

                JsonUtils.TryMarshal(ResourceFiles, "hash_info.json");
            }
        }
    }
}
