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

        private DBSalesperson db = null;

        public SalespersonController()
        {
            db = new DBSalesperson();
        }

        // GET: api/Salesperson
        public IEnumerable<Salesperson> Get()
        {
            return db.GetAll();
        }

        // GET: api/Salesperson/5
        public Salesperson Get(int id)
        {
            return db.Get(id);
        }

        // POST: api/Salesperson
        public void Post([FromBody]Salesperson value)
        {
            if (value != null) {
                db.Persist(value);
            }
        }

        // PUT: api/Salesperson/5
        public void Put(int id, [FromBody]Salesperson value)
        {
            if (value != null)
            {
                value.Id = id;
                db.Update(value);
            }
        }

        // DELETE: api/Salesperson/5
        public void Delete([FromBody]Salesperson value)
        {
            if (value != null)
            {
                db.Delete(value);
            }
        }
    }
}
