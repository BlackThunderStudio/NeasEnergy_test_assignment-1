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
            var faulted = new List<District>();
            try
            {
                return db.GetAll();
            }
            catch (DataLayerArgumentException e)
            {
                faulted.Add(new District()
                {
                    IsFaulted = true,
                    DataLayerArgumentException = e.Message
                });
            }
            catch (DataLayerException e)
            {
                if (faulted.Count == 0) faulted.Add(new District() { IsFaulted = true });
                faulted[0].DataLayerException = e.Message;
            }
            return faulted.AsEnumerable();
        }

        // GET api/<controller>/5
        public District Get(int id)
        {
            var faulted = new District();
            try
            {
                return db.Get(id);
            }
            catch(DataLayerArgumentException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerArgumentException = e.Message;
            }
            catch(DataLayerException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerException = e.Message;
            }
            return faulted;
        }

        // POST api/<controller>
        public District Post([FromBody]District value)
        {
            if(value != null)
            {
                var faulted = new District();
                try
                {
                    db.Persist(value);
                    return new District() { IsFaulted = false };
                }
                catch (DataLayerArgumentException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerArgumentException = e.Message;
                }
                catch (DataLayerException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerException = e.Message;
                }
                return faulted;
            }
            return null;
        }

        // PUT api/<controller>/5
        public District Put(int id, [FromBody]District value)
        {
            if(value != null)
            {
                var faulted = new District();
                try
                {
                    value.Id = id;
                    db.Update(value);
                    return new District() { IsFaulted = false };
                }
                catch (DataLayerArgumentException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerArgumentException = e.Message;
                }
                catch (DataLayerException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerException = e.Message;
                }
                return faulted;
            }
            return null;
        }

        // DELETE api/<controller>/5
        public District Delete(int id)
        {
            var faulted = new District();
            try
            {
                db.Delete(id);
                return new District() { IsFaulted = false };
            }
            catch (DataLayerArgumentException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerArgumentException = e.Message;
            }
            catch (DataLayerException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerException = e.Message;
            }
            return faulted;
        }

        [Route("api/district/{districtId}/secondary-sales/add/{personId}")]
        [HttpPost]
        public District AssignSecondaryToDistrict(int districtId, int personId)
        {
            var faulted = new District();
            try
            {
                DBSalesperson personDb = new DBSalesperson();
                var person = personDb.Get(personId);
                var district = db.Get(districtId);
                db.AssignSecondary(person, district);
                return new District() { IsFaulted = false };
            }
            catch (DataLayerArgumentException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerArgumentException = e.Message;
            }
            catch (DataLayerException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerException = e.Message;
            }
            return faulted;
        }

        [Route("api/district/{districtId}/secondary-sales/delete/{personId}")]
        [HttpPost]
        public District DeleteSecondaryFromDistrict(int districtId, int personId)
        {
            var faulted = new District();
            try
            {
                DBSalesperson personDb = new DBSalesperson();
                var person = personDb.Get(personId);
                var district = db.Get(districtId);
                db.DeleteSecondary(person, district);
                return new District() { IsFaulted = false };
            }
            catch (DataLayerArgumentException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerArgumentException = e.Message;
            }
            catch (DataLayerException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerException = e.Message;
            }
            return faulted;
        }
    }
}