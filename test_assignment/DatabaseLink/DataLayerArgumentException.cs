using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink
{
    [DataContract]
    public class DataLayerArgumentException : DataLayerException
    {
        [DataMember]
        public string ExceptionMessage { get; set; }
        [DataMember]
        public string InnerExceptionMessage { get; set; }

        public DataLayerArgumentException()
        {
        }

        public DataLayerArgumentException(string message) : base(message)
        {
            this.ExceptionMessage = message;
        }

        public DataLayerArgumentException(string message, Exception innerException) : base(message, innerException)
        {
            this.ExceptionMessage = message;
            this.InnerExceptionMessage = innerException.Message;
        }
    }
}
