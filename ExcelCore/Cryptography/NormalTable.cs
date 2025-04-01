namespace ExcelCore.Cryptography
{
    public class NormalTable
    {
        public virtual long GetLong(long value) => value;
        public virtual int GetInt(int value) => value;
        public virtual uint GetUInt(long value) => (uint)value;
        public virtual string GetString(string value) => value;
        public virtual float GetFloat(float value) => value;
    }
}
