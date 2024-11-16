using System;
using System.Collections.Generic;
using System.Text;

namespace Json.Protector.Interfaces
{
    public interface IEncryptionProvider
    {
        string Encrypt(string plainText);
        string Encrypt<T>(T Model);
        string Decrypt(string cipherText);
        T Decrypt<T>(string cipherText);
        T DecryptWithValidation<T>(string cipherText);
        List<T> DecryptListWithValidation<T>(string cipherText);
    }
}
