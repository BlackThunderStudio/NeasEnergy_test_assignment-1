using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Models.Exceptions
{
    [DataContract]
    public class DataLayerException : Exception
    {

        [DataMember]
        public string ExceptionMessage { get; set; }
        [DataMember]
        public string InnerExceptionMessage { get; set; }

        public DataLayerException()
        {
        }

        public DataLayerException(string message) : base(message)
        {
            this.ExceptionMessage = message;
        }

        public DataLayerException(string message, Exception innerException) : base(message, innerException)
        {
            this.ExceptionMessage = message;
            this.InnerExceptionMessage = innerException.Message;
        }
    }
}
