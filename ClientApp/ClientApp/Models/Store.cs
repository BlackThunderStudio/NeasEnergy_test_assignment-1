using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Models.DatabaseModels;

namespace ClientApp.Models
{
    public class Store : Model<DatabaseModels.Store, Store>, INotifyPropertyChanged
    {
        private int _id;
        private string _name, _address;
        private District _district;
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged("Id"); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        public string Address { get { return _address; } set { _address = value; OnPropertyChanged("Address"); } }
        public District District { get { return _district; } set { _district = value; OnPropertyChanged("District"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public override Store FromDatabaseModel(DatabaseModels.Store databaseModel)
        {
            if(databaseModel != null)
            {
                return new Store()
                {
                    Id = databaseModel.Id,
                    Name = databaseModel.Name,
                    Address = databaseModel.Address,
                    District = new District().FromDatabaseModel(databaseModel.District)
                };
            }
            return new Store();
        }

        public override DatabaseModels.Store ToDatabaseModel(Store clientModel)
        {
            if(clientModel != null)
            {
                return new DatabaseModels.Store()
                {
                    Id = clientModel.Id,
                    Name = clientModel.Name,
                    Address = clientModel.Address,
                    District = new District().ToDatabaseModel(clientModel.District)
                };
            }
            return new DatabaseModels.Store();
        }
    }
}
