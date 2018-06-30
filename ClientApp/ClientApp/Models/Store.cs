using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Models.DatabaseModels;

namespace ClientApp.Models
{
    class Store : Model<DatabaseModels.Store, Store>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public District District { get; set; }

        public override Store FromDatabaseModel(DatabaseModels.Store databaseModel)
        {
            return new Store()
            {
                Id = databaseModel.Id,
                Name = databaseModel.Name,
                Address = databaseModel.Address,
                District = new District().FromDatabaseModel(databaseModel.District)
            };
        }

        public override DatabaseModels.Store ToDatabaseModel(Store clientModel)
        {
            return new DatabaseModels.Store()
            {
                Id = clientModel.Id,
                Name = clientModel.Name,
                Address = clientModel.Address,
                District = new District().ToDatabaseModel(clientModel.District)
            };
        }
    }
}
