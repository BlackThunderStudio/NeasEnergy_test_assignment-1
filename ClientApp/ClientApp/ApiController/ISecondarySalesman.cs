using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ApiController
{
    interface ISecondarySalesman
    {
        Task AssignSecondaryAsync(Salesperson person, District district);
        Task DeleteSecondaryAsync(Salesperson person, District district);
    }
}
