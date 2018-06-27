using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink.model
{
    public class District
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public Salesperson PrimarySalesperson { get; set; }
        public IEnumerable<Salesperson> SecondarySalespeople { get; set; }
    }
}
