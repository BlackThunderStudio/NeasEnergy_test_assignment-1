using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Models
{
    [DataContract]
    public class District
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Salesperson PrimarySalesperson { get; set; }
        [DataMember]
        public IEnumerable<Salesperson> SecondarySalespeople { get; set; }

        //for error control
        [DataMember]
        public bool IsFaulted { get; set; }
        [DataMember]
        public string DataLayerException { get; set; }
        [DataMember]
        public string DataLayerArgumentException { get; set; }
    }
}
