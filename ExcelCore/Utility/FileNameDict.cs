namespace ExcelCore.Utility
{
    public static class FileNameDict
    {
        public static readonly Dictionary<string, string> name_pair = [];

        static FileNameDict()
        {
            using (var stream_reader = new StreamReader(File.OpenRead("struct_name.txt")))
            {
                while (!stream_reader.EndOfStream)
                {
                    var line = stream_reader.ReadLine();
                    name_pair.Add(line.ToLower(), line);
                }
            }
        }
    }
}
