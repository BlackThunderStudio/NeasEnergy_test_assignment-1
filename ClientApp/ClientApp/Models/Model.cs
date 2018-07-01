using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Models
{
    public abstract class Model<DatabaseT, ClientT>
    {
        public abstract ClientT FromDatabaseModel(DatabaseT databaseModel);
        public abstract DatabaseT ToDatabaseModel(ClientT clientModel);
    }
}
