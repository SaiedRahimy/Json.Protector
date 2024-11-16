using Json.Protector.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Json.Protector.Converter
{
    public class NewtonsoftJsonProtectorTypeConverter : JsonConverter<JsonProtectorType>
    {
        private readonly IEncryptionProvider _protectionProvider;
        public NewtonsoftJsonProtectorTypeConverter(IEncryptionProvider protectionProvider)
        {
            _protectionProvider = protectionProvider;
        }

        public override void WriteJson(JsonWriter writer, JsonProtectorType value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(_protectionProvider.Encrypt(value.ToString()));
        }


        public override JsonProtectorType? ReadJson(JsonReader reader, Type objectType, JsonProtectorType? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                return new JsonProtectorType(_protectionProvider.Decrypt(reader.Value?.ToString()));
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException("Invalid JsonProtectorType format");
            }
        }
    }
}
