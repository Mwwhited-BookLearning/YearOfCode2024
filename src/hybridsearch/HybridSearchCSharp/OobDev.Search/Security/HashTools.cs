using System;
using System.Security.Cryptography;
using System.Text;

namespace OobDev.Search.Security;

public static class HashTools
{
    public static string GetHash(string input) =>
        Convert.ToBase64String(input.GetHashBytes());
    public static byte[] GetHashBytes(this string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        using var hash = MD5.Create();
        var hashed = hash.ComputeHash(bytes, 0, bytes.Length);
        return hashed;
    }

    public static string AdjustHash(this byte[] hash, int offset)
    {
        var bytes = BitConverter.GetBytes(offset);

        var buffer = new byte[hash.Length];
        Array.Copy(hash, buffer, buffer.Length);

        buffer[^1] += bytes[0];
        buffer[^2] += bytes[1];
        buffer[^3] += bytes[2];
        buffer[^4] += bytes[3];

        return new Guid(buffer).ToString();
    }
}
