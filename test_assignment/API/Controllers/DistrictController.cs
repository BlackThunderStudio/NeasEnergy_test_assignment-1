using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DatabaseLink.mapper;
using DatabaseLink.model;
using DatabaseLink;

namespace API.Controllers
{
    public class DistrictController : ApiController
    {
        private DBDistrict db = null;

        public DistrictController()
        {
            db = new DBDistrict();
        }

        // GET api/<controller>
        public IEnumerable<District> Get()
        {
            return db.GetAll();
        }

        // GET api/<controller>/5
        public District Get(int id)
        {
            return db.Get(id);
        }

        // POST api/<controller>
        public void Post([FromBody]District value)
        {
            if(value != null)
            {
                db.Persist(value);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]District value)
        {
            if(value != null)
            {
                value.Id = id;
                db.Update(value);
            }
        }

        // DELETE api/<controller>/5
        public void Delete([FromBody]District value)
        {
            if(value != null)
            {
                db.Delete(value);
            }
        }

        [Route("api/district/{districtId}/secondary-sales/add/{personId}")]
        [HttpPost]
        public void AssignSecondaryToDistrict(int districtId, int personId)
        {
            DBSalesperson personDb = new DBSalesperson();
            var person = personDb.Get(personId);
            var district = db.Get(districtId);
            db.AssignSecondary(person, district);
        }

        [Route("api/district/{districtId}/secondary-sales/delete/{personId}")]
        [HttpPost]
        public void DeleteSecondaryFromDistrict(int districtId, int personId)
        {
            DBSalesperson personDb = new DBSalesperson();
            var person = personDb.Get(personId);
            var district = db.Get(districtId);
            db.DeleteSecondary(person, district);
        }
    }
}