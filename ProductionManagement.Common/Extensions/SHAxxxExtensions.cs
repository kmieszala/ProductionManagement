namespace ProductionManagement.Common.Extensions
{
    using System.Security.Cryptography;
    using System.Text;

    public static class SHAxxxExtensions
    {
        public static string ComputeSha256Hash(this string @this)
        {
            using SHA256 sha256Hash = SHA256.Create();

            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(@this));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
