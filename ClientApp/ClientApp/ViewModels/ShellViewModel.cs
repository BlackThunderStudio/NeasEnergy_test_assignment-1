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
    class ShellViewModel : Screen
    {
        private Salesperson _person;
        private string _fname;
        private string _lname;
        private SalespersonController api;

        public ShellViewModel()
        {
            api = new SalespersonController();
            api.Endpoint = "http://localhost:50209/";
        }

        public Salesperson Person
        {
            get { return _person; }
            set
            {
                _person = value;
                NotifyOfPropertyChange(() => Person);
            }
        }

        public string FirstName
        {
            get { return _fname; }
            set
            {
                _fname = value;
                NotifyOfPropertyChange(() => FirstName);
            }
        }

        public string LastName
        {
            get { return _lname; }
            set
            {
                _lname = value;
                NotifyOfPropertyChange(() => LastName);
            }
        }

        public async void GetPersonAsync()
        {
            Person = await api.GetAsync(1);
            FirstName = Person.Name;
            LastName = Person.LastName;
        }
    }
}
