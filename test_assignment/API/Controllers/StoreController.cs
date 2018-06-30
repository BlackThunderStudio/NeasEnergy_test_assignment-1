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
            var faulted = new List<Store>();
            try
            {
                return db.GetAll();
            }
            catch(DatabaseLink.DataLayerArgumentException e)
            {
                faulted.Add(new Store()
                {
                    IsFaulted = true,
                    DataLayerArgumentException = e.Message
                });
            }
            catch(DatabaseLink.DataLayerException e)
            {
                if (faulted.Count == 0) faulted.Add(new Store() { IsFaulted = true });
                faulted[0].DataLayerException = e.Message;
            }
            return faulted;
        }

        // GET api/<controller>/5
        public Store Get(int id)
        {
            var faulted = new Store();
            faulted.IsFaulted = false;
            try
            {
                return db.Get(id);
            }
            catch(DatabaseLink.DataLayerArgumentException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerArgumentException = e.Message;
            }
            catch(DatabaseLink.DataLayerException e)
            {
                faulted.IsFaulted = true;
                faulted.DataLayerException = e.Message;
            }
            return faulted;         
        }

        // POST api/<controller>
        public Store Post([FromBody]Store value)
        {
            if(value != null)
            {
                var faulted = new Store();
                faulted.IsFaulted = false;
                try
                {
                    db.Persist(value);
                    return new Store() { IsFaulted = false };
                }
                catch (DatabaseLink.DataLayerArgumentException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerArgumentException = e.Message;
                }
                catch (DatabaseLink.DataLayerException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerException = e.Message;
                }
                return faulted;
            }
            return null;
        }

        // PUT api/<controller>/5
        public Store Put(int id, [FromBody]Store value)
        {
            if(value != null)
            {
                var faulted = new Store() { IsFaulted = false };
                try
                {
                    value.Id = id;
                    db.Update(value);
                    return faulted;
                }
                catch (DatabaseLink.DataLayerArgumentException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerArgumentException = e.Message;
                }
                catch (DatabaseLink.DataLayerException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerException = e.Message;
                }
                return faulted;
            }
            return null;
        }

        // DELETE api/<controller>/5
        public Store Delete([FromBody]Store value)
        {
            if(value != null)
            {
                var faulted = new Store() { IsFaulted = false };
                try
                {
                    db.Delete(value);
                    return faulted;
                }
                catch (DatabaseLink.DataLayerArgumentException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerArgumentException = e.Message;
                }
                catch (DatabaseLink.DataLayerException e)
                {
                    faulted.IsFaulted = true;
                    faulted.DataLayerException = e.Message;
                }
                return faulted;
            }
            return null;
        }
    }
}