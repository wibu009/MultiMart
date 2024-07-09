using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace MultiMart.Infrastructure.Common.Extensions;

public static class EncryptionExtensions
{
    public static string ToAesEncrypt<T>(this T data, string key, string iv)
    {
        using var ms = new MemoryStream();
        using (var aes = Aes.Create()) // Create AES instance inside using block
        {
            aes.Key = Convert.FromBase64String(key);
            aes.IV = Convert.FromBase64String(iv);

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                // Serialize data to JSON
                string json = JsonConvert.SerializeObject(data);
                byte[] bytes = Encoding.Unicode.GetBytes(json);

                // Write entire byte array to stream
                cs.Write(bytes, 0, bytes.Length);
            }
        }

        // Get complete byte array from MemoryStream
        return Convert.ToBase64String(ms.ToArray());
    }

    public static T FromAesDecrypt<T>(this string base64, string key, string iv)
    {
        using var ms = new MemoryStream(Convert.FromBase64String(base64));
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(key);
        aes.IV = Convert.FromBase64String(iv);

        using var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
        using var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read);

        // Read all bytes from stream into a List<byte> for efficiency
        var decryptedBytes = new List<byte>();
        int readCount;
        do
        {
            byte[] buffer = new byte[1024]; // Adjust buffer size as needed
            readCount = cs.Read(buffer, 0, buffer.Length);
            decryptedBytes.AddRange(buffer.Take(readCount));
        }
        while (readCount > 0);

        // Convert decrypted bytes to string
        string json = Encoding.Unicode.GetString(decryptedBytes.ToArray());

        // Deserialize JSON into object
        return JsonConvert.DeserializeObject<T>(json) ?? throw new InvalidOperationException();
    }

    public static T FromBase64String<T>(this string base64)
        where T : class
    {
        byte[] bytes = Convert.FromBase64String(base64);
        string json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<T>(json) ?? throw new InvalidOperationException();
    }

    public static string ToBase64String<T>(this T data)
        where T : class
    {
        string json = JsonConvert.SerializeObject(data);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }

    public static string ToSha256Hash(this string input)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));

        // Convert the byte array to a hexadecimal string.
        var builder = new StringBuilder();
        foreach (byte t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }
}