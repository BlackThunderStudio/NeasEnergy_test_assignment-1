using DatabaseLink.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink.mapper
{
    public interface ISecondarySalesman
    {
        void AssignSecondary(Salesperson person, District district);
        void DeleteSecondary(Salesperson person, District district);
    }
}
