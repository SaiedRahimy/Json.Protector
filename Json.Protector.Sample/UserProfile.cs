using Json.Protector.Converter;
using Newtonsoft.Json;

namespace Json.Protector.Sample
{
    public class UserProfile
    {
        public string Name { get; set; }
        public JsonProtectorType SensitiveInfo { get; set; }
    }
    
    public class UserProfileAttribute
    {
        public string Name { get; set; }

        [JsonConverter(typeof(NewtonsoftDataProtector))]
        public string SensitiveInfo { get; set; }
    }

    public class UserProfileWithOutEncrypt
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
