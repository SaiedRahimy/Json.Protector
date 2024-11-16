using Json.Protector.Interfaces;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Json.Protector.Converter
{
    public class SystemTextJsonJsonProtectorTypeConverter : JsonConverter<JsonProtectorType>
    {
        private readonly IEncryptionProvider _protectionProvider;

        public SystemTextJsonJsonProtectorTypeConverter(IEncryptionProvider protectionProvider)
        {
            _protectionProvider = protectionProvider ?? throw new ArgumentNullException(nameof(protectionProvider));
        }

        public override JsonProtectorType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException("Expected string token for JsonProtectorType.");

            string? encryptedValue = reader.GetString();

            if (string.IsNullOrEmpty(encryptedValue))
                throw new JsonException("Empty or null value cannot be decrypted to a JsonProtectorType.");

            try
            {
                string decryptedValue = _protectionProvider.Decrypt(encryptedValue);
                return new JsonProtectorType(decryptedValue);
            }
            catch (Exception ex)
            {
                throw new JsonException("Invalid JsonProtectorType format.", ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, JsonProtectorType value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            string encryptedValue = _protectionProvider.Encrypt(value.ToString());
            writer.WriteStringValue(encryptedValue);
        }
    }
}

