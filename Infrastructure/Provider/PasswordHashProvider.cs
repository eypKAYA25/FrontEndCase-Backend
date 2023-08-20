using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Provider;

public class PasswordHashProvider
{
    private static readonly byte[] _key;

    public static byte[] Key
    {
        get { return _key; }
    }

    static PasswordHashProvider()
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals(Environments.Production, StringComparison.Ordinal))
        {
            byte[] bytes = new byte[512];
            Random.Shared.NextBytes(bytes);
            _key = bytes;
        }
        else
        {
            _key = Encoding.UTF8.GetBytes("Development++".PadLeft(128));
        }
    }

    public byte[] Hash(string password)
    {
        using SHA512 sha512 = SHA512.Create();
        using MD5 md5 = MD5.Create();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] sha521HashBytes = sha512.ComputeHash(passwordBytes);
        byte[] md5HashBytes = md5.ComputeHash(sha521HashBytes);
        return md5HashBytes;
    }

    public bool CompareHash(byte[] hash1, byte[] hash2)
    {
        if (hash1.Length != hash2.Length)
        {
            return false;
        }

        bool matched = true;
        for (int i = 0; i < 16; i++)
        {
            if (hash1[i] != hash2[i])
            {
                matched = false;
                break;
            }
        }

        return matched;
    }
}