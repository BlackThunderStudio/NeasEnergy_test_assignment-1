using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Models.DatabaseModels;

namespace ClientApp.Models
{
    class Salesperson : Model<DatabaseModels.Salesperson, Salesperson>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public override Salesperson FromDatabaseModel(DatabaseModels.Salesperson databaseModel)
        {
            return new Salesperson()
            {
                Id = databaseModel.Id,
                Name = databaseModel.Name,
                LastName = databaseModel.LastName
            };
        }

        public override DatabaseModels.Salesperson ToDatabaseModel(Salesperson clientModel)
        {
            return new DatabaseModels.Salesperson()
            {
                Id = clientModel.Id,
                Name = clientModel.Name,
                LastName = clientModel.LastName
            };
        }
    }
}
