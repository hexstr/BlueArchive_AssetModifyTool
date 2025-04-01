using System.Runtime.InteropServices;
using System.Text;

namespace ExcelCore.Utility
{
    public class opencc
    {
        [DllImport("opencc", CharSet = CharSet.Ansi)]
        private static extern nint opencc_open(string config_file_name);

        [DllImport("opencc", CharSet = CharSet.Ansi)]
        private static extern long opencc_convert_utf8_to_buffer(nint opencc_ptr, [MarshalAs(UnmanagedType.LPUTF8Str)] string input, long length, [MarshalAs(UnmanagedType.LPUTF8Str)] StringBuilder output);

        [DllImport("opencc", CharSet = CharSet.Ansi)]
        private static extern string opencc_error();

        private static nint opencc_ptr_ = nint.Zero;

        public static bool Enable { get; set; }
        public static bool Loaded { get => opencc_ptr_ != nint.Zero && Enable; }

        static opencc()
        {
            try
            {
                var asm_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                opencc_ptr_ = opencc_open(Path.Combine(asm_path, "opencc/tw2sp.json"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string Convert(string origin)
        {
            if (opencc_ptr_ != nint.Zero)
            {
                var buffer = new StringBuilder(2048);
                if (opencc_convert_utf8_to_buffer(opencc_ptr_, origin, -1, buffer) == -1)
                {
                    throw new Exception(opencc_error());
                }
                return buffer.ToString();
            }
            return string.Empty;
        }
    }
}
