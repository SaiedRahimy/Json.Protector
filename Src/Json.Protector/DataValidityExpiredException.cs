using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json.Protector
{
    public class DataValidityExpiredException : Exception
    {
        public DataValidityExpiredException()
            : base("The validity of the data has expired. Please refrain from resubmitting this data.")
        {
        }

        public DataValidityExpiredException(string message)
            : base(message)
        {
        }

        public DataValidityExpiredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
