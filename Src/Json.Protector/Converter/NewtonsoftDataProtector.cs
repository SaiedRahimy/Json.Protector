using Json.Protector.Interfaces;
using Newtonsoft.Json;
using System;

namespace Json.Protector.Converter
{

    public class NewtonsoftDataProtector : JsonConverter
    {
        private static IEncryptionProvider _protectionProvider;

        public NewtonsoftDataProtector()
        {

        }
        public NewtonsoftDataProtector(IEncryptionProvider protectionProvider)
        {
            _protectionProvider = protectionProvider ?? throw new ArgumentNullException(nameof(protectionProvider));

        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override string ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();

            if (value == null)
            {
                return null;
            }

            try
            {
                var decryptText = _protectionProvider.Decrypt(value);
                return decryptText;
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException("Invalid DataProtector format");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {            
            var stringValue = value?.ToString();
            writer.WriteValue(_protectionProvider.Encrypt(stringValue.ToString()));

        }
    }
}
