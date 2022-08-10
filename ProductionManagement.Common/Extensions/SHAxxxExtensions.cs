namespace ProductionManagement.Common.Extensions
{
    using System.Security.Cryptography;
    using System.Text;

    public static class SHAxxxExtensions
    {
        public static string ToSHA1String(this string @this)
        {
            byte[] buffer = Encoding.Default.GetBytes(@this + new string(@this.Reverse().ToArray()));
            using var sha1 = SHA1.Create();

            sha1.Initialize();

            var bytes = sha1.ComputeHash(buffer);
            return buffer.ToString();
        }
    }
}
