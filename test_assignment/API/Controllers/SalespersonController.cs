using DatabaseLink.mapper;
using DatabaseLink.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class SalespersonController : ApiController
    {
        // GET: api/Salesperson
        public IEnumerable<Salesperson> Get()
        {
            DBSalesperson db = new DBSalesperson();
            return db.GetAll();
        }

        // GET: api/Salesperson/5
        public Salesperson Get(int id)
        {
            DBSalesperson db = new DBSalesperson();
            return db.Get(id);
        }

        // POST: api/Salesperson
        public void Post([FromBody]Salesperson value)
        {
            DBSalesperson db = new DBSalesperson();
            if (value != null) {
                db.Persist(value);
            }
        }

        // PUT: api/Salesperson/5
        public void Put(int id, [FromBody]Salesperson value)
        {
            DBSalesperson db = new DBSalesperson();
            if (value != null)
            {
                value.Id = id;
                db.Update(value);
            }
        }

        // DELETE: api/Salesperson/5
        public void Delete([FromBody]Salesperson value)
        {
            DBSalesperson db = new DBSalesperson();
            if (value != null)
            {
                db.Delete(value);
            }
        }
    }
}
