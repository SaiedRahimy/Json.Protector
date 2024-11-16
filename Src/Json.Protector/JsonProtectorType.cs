using System;
using System.Collections.Generic;
using System.Text;

namespace Json.Protector
{
    public class JsonProtectorType
    {
        private readonly string _value;

        public JsonProtectorType(string value)
        {
            _value = value;
        }

        public static implicit operator string(JsonProtectorType myString) => myString._value;

        public static implicit operator JsonProtectorType(string value) => new JsonProtectorType(value);

        public override string ToString() => _value;

        public int Length => _value.Length;

        public char this[int index] => _value[index];

        public JsonProtectorType ToUpper() => new JsonProtectorType(_value.ToUpper());

        public JsonProtectorType ToLower() => new JsonProtectorType(_value.ToLower());

        public bool Contains(string value) => _value.Contains(value);

        public static JsonProtectorType operator +(JsonProtectorType a, JsonProtectorType b)
        {
            return new JsonProtectorType(a._value + b._value);
        }
    }
}
