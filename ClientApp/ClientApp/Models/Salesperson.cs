using ClientApp.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Models
{
    [DataContract]
    public class Salesperson
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LastName { get; set; }

        //for error control
        [DataMember]
        public bool IsFaulted { get; set; }
        [DataMember]
        public DataLayerException DataLayerException { get; set; }
        [DataMember]
        public DataLayerArgumentException DataLayerArgumentException { get; set; }
    }
}
