using System;
using System.Collections.Generic;
using System.Text;

namespace Json.Protector
{
    public class JsonProtectorOptions
    {
        public bool UseDefaultKey { get; set; } = true;
        public string Key { get; set; }
        public string IV { get; set; }

        public TimeSpan? ValidityPeriod { get; set; }
        public bool ThrowExceptionIfTimeExpired { get; set; } = true;

    }
}
