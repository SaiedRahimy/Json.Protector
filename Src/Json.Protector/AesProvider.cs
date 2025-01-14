using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Json.Protector.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Json.Protector.Models;
using System.Reflection;

namespace Json.Protector
{
    public class AesProvider : IEncryptionProvider
    {
        private JsonProtectorOptions _options;
        private readonly byte[] Key = null;
        private readonly byte[] IV = null;
        public AesProvider(IOptions<JsonProtectorOptions>? options)
        {
            Key = Encoding.UTF8.GetBytes("QaD3DCsdsfwffJGGHG#wrwfsCDDFD$#@");
            IV = Encoding.UTF8.GetBytes("TWSSFqeqeF3RFE!$");

            if (options is not null)
            {
                _options = options.Value;

                if (_options.UseDefaultKey == false)
                {
                    Key = Encoding.UTF8.GetBytes(_options.Key);
                    if (Key.Length > 32)
                    {
                        Console.WriteLine($"Key is too long. Trimming to 256 bits.");
                        Array.Resize(ref Key, 32);
                    }

                    IV = Encoding.UTF8.GetBytes(_options.IV);
                    if (IV.Length > 16)
                    {
                        Console.WriteLine($"IV is too long. Trimming to 128 bits.");
                        Array.Resize(ref IV, 16);
                    }
                }
            }


        }
        private string AddTimeBoundData(string plainText)
        {
            if (_options.ValidityPeriod.HasValue)
            {

                var model = new ExpirationGuardDto
                {
                    Data = plainText,
                    ExpireDate = DateTime.Now.AddMilliseconds(_options.ValidityPeriod.Value.TotalMilliseconds)
                };

                plainText = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });


            }

            return plainText;
        }

        private string CheckTimeBoundExpiration(string strData)
        {
            if (_options.ValidityPeriod.HasValue)
            {
                var data = JsonConvert.DeserializeObject<ExpirationGuardDto>(strData, new JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });


                if (data.ExpireDate >= DateTime.Now)
                {

                    return data.Data;
                }
                else if (_options.ThrowExceptionIfTimeExpired == false)
                {
                    return null;
                }

                throw new DataValidityExpiredException();

            }

            return strData;
        }



        public string Encrypt(string plainText)
        {
            using (var aesAlg = AesManaged.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(AddTimeBoundData(plainText));
                        }
                    }
                    var encryptedBytes = msEncrypt.ToArray();
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public string Encrypt<T>(T Model)
        {
            var plainText = JsonConvert.SerializeObject(Model, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(AddTimeBoundData(plainText));
                        }
                    }
                    var encryptedBytes = msEncrypt.ToArray();
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            var strData = srDecrypt.ReadToEnd();

                            strData = CheckTimeBoundExpiration(strData);

                            return strData;
                        }
                    }
                }
            }
        }

        public T Decrypt<T>(string cipherText)
        {
            try
            {
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                var strData = srDecrypt.ReadToEnd();

                                strData = CheckTimeBoundExpiration(strData);

                                var data = JsonConvert.DeserializeObject<T>(strData, new JsonSerializerSettings
                                {
                                    Formatting = Newtonsoft.Json.Formatting.None,
                                    NullValueHandling = NullValueHandling.Ignore,
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                });

                                return data;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Decrypt : {ex}");
            }
            return default(T);
        }
        public T DecryptWithValidation<T>(string cipherText)
        {
            try
            {
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                var strData = srDecrypt.ReadToEnd();


                                var obj = JObject.Parse(strData);

                                var propertyNames = typeof(T).GetProperties().Select(c => new { c.Name, c.PropertyType }).ToArray();

                                foreach (var property in propertyNames)
                                {
                                    var returnTypeName = typeof(T).GetProperty(property.Name).GetGetMethod().ReturnType.Name;
                                    var isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null;
                                    if (isNullable || returnTypeName == "String")
                                    {
                                        continue;
                                    }
                                    if (obj.ContainsKey(property.Name) == false)
                                    {
                                        Console.WriteLine($"Error in Decrypt ,Property ({property.Name}) Not Found");

                                        return default(T);
                                    }
                                }


                                var data = JsonConvert.DeserializeObject<T>(strData, new JsonSerializerSettings
                                {
                                    Formatting = Newtonsoft.Json.Formatting.None,
                                    NullValueHandling = NullValueHandling.Ignore,
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                });

                                return data;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Decrypt : {ex}");
            }
            return default(T);
        }
        public List<T> DecryptListWithValidation<T>(string cipherText)
        {
            try
            {
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                var strData = srDecrypt.ReadToEnd();


                                var objectList = JArray.Parse(strData);


                                var obj = objectList.FirstOrDefault() as JObject;

                                var propertyNames = typeof(T).GetProperties().Select(c => new { c.Name, c.PropertyType }).ToArray();

                                foreach (var property in propertyNames)
                                {
                                    var returnTypeName = typeof(T).GetProperty(property.Name).GetGetMethod().ReturnType.Name;
                                    var isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null;
                                    if (isNullable || returnTypeName == "String" || property.PropertyType.IsClass)
                                    {
                                        continue;
                                    }
                                    if (obj.ContainsKey(property.Name) == false)
                                    {
                                        Console.WriteLine($"Error in Decrypt ,Property ({property.Name}) Not Found");

                                        return null;
                                    }
                                }




                                var data = JsonConvert.DeserializeObject<List<T>>(strData, new JsonSerializerSettings
                                {
                                    Formatting = Newtonsoft.Json.Formatting.None,
                                    NullValueHandling = NullValueHandling.Ignore,
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                });

                                return data;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Decrypt : {ex}");
            }
            return null;
        }
    }
}

