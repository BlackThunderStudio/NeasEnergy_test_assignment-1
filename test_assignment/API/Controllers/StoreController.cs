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
    public class StoreController : ApiController
    {

        private DBStore db = null;

        public StoreController()
        {
            db = new DBStore();
        }

        // GET api/<controller>
        public IEnumerable<Store> Get()
        {
            return db.GetAll();
        }

        // GET api/<controller>/5
        public Store Get(int id)
        {
            return db.Get(id);
        }

        // POST api/<controller>
        public void Post([FromBody]Store value)
        {
            if(value != null)
            {
                db.Persist(value);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]Store value)
        {
            if(value != null)
            {
                value.Id = id;
                db.Update(value);
            }
        }

        // DELETE api/<controller>/5
        public void Delete([FromBody]Store value)
        {
            if(value != null)
            {
                db.Delete(value);
            }
        }
    }
}