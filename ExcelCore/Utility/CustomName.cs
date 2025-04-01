namespace ExcelCore.Utility
{
    internal static class CustomName
    {
        private static readonly Dictionary<string, string> name_pair = [];
        private static readonly Dictionary<string, string> text_pair = [];

        static CustomName()
        {
            if (JsonUtils.TryUnmarshal<Dictionary<string, string>>("custom_name.json", out var custom_name_list))
            {
                foreach (var custom_name in custom_name_list)
                {
                    var original = custom_name.Key;
                    var replace = custom_name.Value;
                    name_pair.Add($"{original[0]}、{original}", $"{replace[0]}、{replace}");
                    name_pair.Add($"{original[0]}，{original}", $"{replace[0]}，{replace}");
                    name_pair.Add(original, replace);
                }
            }

            if (JsonUtils.TryUnmarshal<Dictionary<string, string>>("custom_text.json", out var custom_text_list))
            {
                foreach (var custom_text in custom_text_list)
                {
                    var original = custom_text.Key;
                    var replace = custom_text.Value;
                    text_pair.Add(original, replace);
                }
            }
        }

        public static string Translate(this string input)
        {
            foreach (var pair in name_pair)
            {
                input = input.Replace(pair.Key, pair.Value);
            }

            foreach (var pair in text_pair)
            {
                input = input.Replace(pair.Key, pair.Value);
            }
            return input;
        }
    }
}
