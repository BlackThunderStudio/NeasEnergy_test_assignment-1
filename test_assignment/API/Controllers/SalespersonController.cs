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
            var faulted = new List<Salesperson>();
            try
            {
                return db.GetAll();
            }
            catch (DatabaseLink.DataLayerArgumentException e)
            {
                faulted.Add(new Salesperson()
                {
                    IsFaulted = true,
                    DataLayerArgumentException = e.Message
                });
            }
            catch (DatabaseLink.DataLayerException e)
            {
                if (faulted.Count == 0) faulted.Add(new Salesperson() { IsFaulted = true });
                faulted[0].DataLayerException = e.Message;
            }
            return faulted.AsEnumerable();
        }

        // GET: api/Salesperson/5
        public Salesperson Get(int id)
        {
            var faulted = new Salesperson();
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

        // POST: api/Salesperson
        public Salesperson Post([FromBody]Salesperson value)
        {
            if (value != null) {
                var faulted = new Salesperson();
                faulted.IsFaulted = false;
                try
                {
                    db.Persist(value);
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

        // PUT: api/Salesperson/5
        public Salesperson Put(int id, [FromBody]Salesperson value)
        {
            if (value != null)
            {
                var faulted = new Salesperson();
                faulted.IsFaulted = false;
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

        // DELETE: api/Salesperson/5
        public Salesperson Delete(int id)
        {
            var faulted = new Salesperson();
            faulted.IsFaulted = false;
            try
            {
                db.Delete(id);
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
    }
}
