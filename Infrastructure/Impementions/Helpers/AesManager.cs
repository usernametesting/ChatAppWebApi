using Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Impementions.Helpers;

public class AesManager : IAesManager
{
    private readonly IConfiguration configuration;
    private readonly byte[] _key;
    public AesManager(IConfiguration configuration)
    {
        this.configuration = configuration;
        _key = Convert.FromBase64String(configuration["AesKey"]!);
    }
    public string Decrypt(byte[] data)
    {
        string plaintext = null;
        using (AesManaged aes = new AesManaged())
        {
            ICryptoTransform decryptor = aes.CreateDecryptor(_key, aes.IV);
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                        plaintext = reader.ReadToEnd();
                }
            }
        }
        return plaintext;
    }

    public byte[] Encrypt(string data)
    {
        byte[] encrypted;
        using (AesManaged aes = new AesManaged())
        {
            aes.Key = _key;
            aes.GenerateIV();
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);

                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(data);
                    }
                    encrypted = ms.ToArray();
                }
            }
        }
        return encrypted;
    }
}
