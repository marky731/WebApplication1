using System;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        byte[] keyBytes = new byte[64]; // 64 bytes = 512 bits (recommended for HMACSHA512)
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }
        string base64Key = Convert.ToBase64String(keyBytes);
        Console.WriteLine(base64Key); // Copy this output to your appsettings.json
    }
}
