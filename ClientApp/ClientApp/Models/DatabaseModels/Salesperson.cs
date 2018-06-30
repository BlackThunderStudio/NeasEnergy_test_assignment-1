using ClientApp.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace ClientApp.Models.DatabaseModels
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
        public string DataLayerException { get; set; }
        [DataMember]
        public string DataLayerArgumentException { get; set; }
    }
}
