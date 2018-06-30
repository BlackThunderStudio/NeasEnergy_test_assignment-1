using Caliburn.Micro;
using ClientApp.ApiController;
using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    class ShellViewModel : Conductor<object>
    {

        public ShellViewModel()
        {

        }

        public void ViewSalespeople()
        {
            ActivateItem(new SalespeopleViewModel());
        }

        public void ViewDistricts()
        {
            ActivateItem(new DistrictsViewModel());
        }

        public void ViewStores()
        {
            ActivateItem(new StoresViewModel());
        }
    }
}
