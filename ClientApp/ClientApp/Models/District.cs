using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Models.DatabaseModels;

namespace ClientApp.Models
{
    class District : Model<DatabaseModels.District, District>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Salesperson PrimarySalesperson { get; set; }
        public IEnumerable<Salesperson> SecondarySalespeople { get; set; }

        public override District FromDatabaseModel(DatabaseModels.District databaseModel)
        {
            IEnumerable<Salesperson> secondarySales = databaseModel.SecondarySalespeople.Select(x => new Salesperson().FromDatabaseModel(x));
            return new District()
            {
                Id = databaseModel.Id,
                Name = databaseModel.Name,
                PrimarySalesperson = new Salesperson().FromDatabaseModel(databaseModel.PrimarySalesperson),
                SecondarySalespeople = secondarySales
            };
        }

        public override DatabaseModels.District ToDatabaseModel(District clientModel)
        {
            IEnumerable<DatabaseModels.Salesperson> secondarySales = clientModel.SecondarySalespeople.Select(x => new Salesperson().ToDatabaseModel(x));
            return new DatabaseModels.District()
            {
                Id = clientModel.Id,
                Name = clientModel.Name,
                PrimarySalesperson = new Salesperson().ToDatabaseModel(clientModel.PrimarySalesperson),
                SecondarySalespeople = secondarySales
            };
        }
    }
}
