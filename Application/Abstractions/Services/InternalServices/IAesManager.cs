using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services;

public interface IAesManager
{
    byte[] Encrypt(string plainText);
    string Decrypt(byte[] cipherText);
}
