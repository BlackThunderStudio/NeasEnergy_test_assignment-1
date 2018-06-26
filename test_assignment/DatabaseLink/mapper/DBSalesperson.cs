using DatabaseLink.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink.mapper
{
    public class DBSalesperson : IDataAccessObject<Salesperson>
    {
        private DBConnect conn = null;

        public DBSalesperson()
        {
            conn = new DBConnect();
        }

        public void Delete(Salesperson t)
        {
            if(t.Id < 1) throw new DataLayerException("Illegal ID value. ID value cannot be less or equal zero!", new ArgumentOutOfRangeException());

            string qry = $"if exists ( select 1 from salespersons where Id={t.Id} )\n begin\n delete from salespersons where Id={t.Id}\n end";
            try
            {
                var response = conn.ExecuteSqlStatement(qry);
            }
            catch (Exception e)
            {
                throw new DataLayerException("Delete operation failed!", e);
            }
        }

        public Salesperson Get(int id)
        {
            string qry = $"select * from salespersons where Id={id}";

            if (id < 1) throw new DataLayerException("ID cannot be less than zero!", new ArgumentException());
            SqlDataReader response = null;
            Salesperson person = new Salesperson();
            try
            {
                response = conn.ExecuteSqlStatement(qry);
                try
                {
                    person.Name = response.GetString(1);
                    person.Surname = response.GetString(2);
                }
                catch (InvalidCastException e)
                {
                    throw new DataLayerException($"Could not map a salesperson of ID: {id}", e);
                }
            }
            catch(Exception e)
            {
                throw new DataLayerException($"Could not retrieve a salesperson of ID: {id}!\n", e);
            }
            return person;
        }

        public IEnumerable<Salesperson> GetAll()
        {
            string qry = "select * from salespersons";
            List<Salesperson> salespeople = new List<Salesperson>();
            SqlDataReader response = null;
            try
            {
                response = conn.ExecuteSqlStatement(qry);
                while (response.Read())
                {
                    Salesperson temp = new Salesperson();
                    try
                    {
                        temp.Name = response[1].ToString();
                        temp.Surname = response[2].ToString();
                        salespeople.Add(temp);
                    }
                    catch(InvalidCastException e)
                    {
                        throw new DataLayerException($"Could not map a salesperson from the response.", e);
                    }
                }

            }
            catch (Exception e)
            {
                throw new DataLayerException($"Could not retrieve the list of salespeople!\n", e);
            }
            return salespeople.AsEnumerable();
        }

        public void Persist(Salesperson t)
        {
            if (t.Name == null) t.Name = String.Empty;
            if (t.Surname == null) t.Surname = String.Empty;

            string qry = $"if not exists (select 1 from salespersons where Name={t.Name} and Surname={t.Surname})\n begin\n insert into salespersons (name,surname) values ('{t.Name}','{t.Surname}')\n end";
            try
            {
                var response = conn.ExecuteSqlStatement(qry);
            }
            catch(Exception e)
            {
                throw new DataLayerException("Upload failure!", e);
            }
        }

        public void Update(Salesperson t)
        {
            if (t.Name.Equals(String.Empty) || t.Surname.Equals(String.Empty)) throw new DataLayerException("None of the fields can be empty!");
            if (t.Name == null || t.Surname == null) throw new DataLayerException("NULL values not allowed", new ArgumentNullException());
            if (t.Id < 1) throw new DataLayerException("Illegal ID value. ID value cannot be less or equal zero!", new ArgumentOutOfRangeException());

            string qry = $"if exists ( select 1 from salespersons where Id={t.Id} )\n begin\n update salespersons set Name='{t.Name}',Surname='{t.Surname}' where Id={t.Id}\n end";
            try
            {
                var response = conn.ExecuteSqlStatement(qry);
            }
            catch (Exception e)
            {
                throw new DataLayerException("Update failure!", e);
            }
        }

    }
}
