using System.IO.Hashing;
using System.Text;

namespace ExcelCore.Cryptography
{
    public static class xxhash
    {
        public static uint CalculateHash(string name)
        {
            var result = XxHash32.Hash(Encoding.ASCII.GetBytes(name));
            Array.Reverse(result);
            return BitConverter.ToUInt32(result, 0);
        }

        public static uint CalculateHash(byte[] array)
        {
            var result = XxHash32.Hash(array);
            Array.Reverse(result);
            return BitConverter.ToUInt32(result, 0);
        }
    }
}
