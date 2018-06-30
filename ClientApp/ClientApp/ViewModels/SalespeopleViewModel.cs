using Caliburn.Micro;
using ClientApp.ApiController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    public class SalespeopleViewModel : Screen
    {
        public class Salesperson : PropertyChangedBase
        {
            private int _id;

            public int Id
            {
                get { return _id; }
                set
                {
                    _id = value;
                    NotifyOfPropertyChange(() => Id);
                }
            }

            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }

            private string _lastName;

            public string LastName
            {
                get { return _lastName; }
                set
                {
                    _lastName = value;
                    NotifyOfPropertyChange(() => LastName);
                }
            }
        }

        private IEnumerable<Salesperson> _salespeople;
        public IEnumerable<Salesperson> Salespeople
        {
            get { return _salespeople; }
            set
            {
                _salespeople = value;
                NotifyOfPropertyChange(() => Salespeople);
            }
        }

        private async Task<IEnumerable<Salesperson>> LoadPeople()
        {
            SalespersonController controller = new SalespersonController();
            var people = await controller.GetAllAsync();
            var converted = new List<Salesperson>();
            foreach(var person in people.ToList())
            {
                converted.Add(new Salesperson()
                {
                    Id = person.Id,
                    Name = person.Name,
                    LastName = person.LastName
                });
            }
            return converted;
        }

        public SalespeopleViewModel()
        {
            Task.Run(async () =>
            {
                Salespeople = await LoadPeople();
            });
        }
    }
}
