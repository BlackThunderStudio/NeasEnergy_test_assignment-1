using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Models.DatabaseModels;

namespace ClientApp.Models
{
    public class Salesperson : Model<DatabaseModels.Salesperson, Salesperson>, INotifyPropertyChanged
    {
        private int _id;
        private string _name, _lastName;
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged("Id"); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        public string LastName { get { return _lastName; } set { _lastName = value; OnPropertyChanged("LastName"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public override Salesperson FromDatabaseModel(DatabaseModels.Salesperson databaseModel)
        {
            if(databaseModel != null)
            {
                return new Salesperson()
                {
                    Id = databaseModel.Id,
                    Name = databaseModel.Name,
                    LastName = databaseModel.LastName
                };
            }
            return new Salesperson();
        }

        public override DatabaseModels.Salesperson ToDatabaseModel(Salesperson clientModel)
        {
            if(clientModel != null)
            {
                return new DatabaseModels.Salesperson()
                {
                    Id = clientModel.Id,
                    Name = clientModel.Name,
                    LastName = clientModel.LastName
                };
            }
            return new DatabaseModels.Salesperson();
        }
    }
}
