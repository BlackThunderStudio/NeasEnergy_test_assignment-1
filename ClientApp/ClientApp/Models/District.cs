using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Models.DatabaseModels;

namespace ClientApp.Models
{
    public class District : Model<DatabaseModels.District, District>, INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private Salesperson _primarySalesperson;
        private IEnumerable<Salesperson> _secondarySalespeople;
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged("Id"); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        public Salesperson PrimarySalesperson { get { return _primarySalesperson; } set { _primarySalesperson = value; OnPropertyChanged("PrimarySalesperson"); } }
        public IEnumerable<Salesperson> SecondarySalespeople { get { return _secondarySalespeople; } set { _secondarySalespeople = value; OnPropertyChanged("SecondarySalespeople"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public override District FromDatabaseModel(DatabaseModels.District databaseModel)
        {
            if(databaseModel != null)
            {
                IEnumerable<Salesperson> secondarySales = new List<Salesperson>();
                if (databaseModel.SecondarySalespeople != null)
                {
                    secondarySales = databaseModel.SecondarySalespeople.Select(x => new Salesperson().FromDatabaseModel(x));
                }
                return new District()
                {
                    Id = databaseModel.Id,
                    Name = databaseModel.Name,
                    PrimarySalesperson = new Salesperson().FromDatabaseModel(databaseModel.PrimarySalesperson),
                    SecondarySalespeople = secondarySales
                };
            }
            return new District();
        }

        public override DatabaseModels.District ToDatabaseModel(District clientModel)
        {
            if(clientModel != null)
            {
                IEnumerable<DatabaseModels.Salesperson> secondarySales = new List<DatabaseModels.Salesperson>();
                if (clientModel.SecondarySalespeople != null)
                {
                    secondarySales = clientModel.SecondarySalespeople.Select(x => new Salesperson().ToDatabaseModel(x));
                }
                return new DatabaseModels.District()
                {
                    Id = clientModel.Id,
                    Name = clientModel.Name,
                    PrimarySalesperson = new Salesperson().ToDatabaseModel(clientModel.PrimarySalesperson),
                    SecondarySalespeople = secondarySales
                };
            }
            return new DatabaseModels.District();
        }
    }
}
