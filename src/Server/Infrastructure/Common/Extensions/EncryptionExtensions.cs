using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace BookStack.Infrastructure.Common.Extensions;

public static class EncryptionExtensions
{
    // Encrypts the data using AES encryption
    public static string Encrypt<T>(this T data, string key, string iv)
    {
        string json = JsonConvert.SerializeObject(data);
        byte[] bytes = Encoding.Unicode.GetBytes(json);

        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(key);
        aes.IV = Convert.FromBase64String(iv);

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        cs.Write(bytes, 0, bytes.Length);
        cs.Close();

        return Convert.ToBase64String(ms.ToArray());
    }

    public static T Decrypt<T>(this string base64, string key, string iv)
    {
        byte[] cipherText = Convert.FromBase64String(base64);

        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(key);
        aes.IV = Convert.FromBase64String(iv);

        using var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipherText);
        using var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read);
        byte[] plainText = new byte[cipherText.Length];
        int decryptedByteCount = cs.Read(plainText, 0, plainText.Length);

        string json = Encoding.Unicode.GetString(plainText, 0, decryptedByteCount);
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
}