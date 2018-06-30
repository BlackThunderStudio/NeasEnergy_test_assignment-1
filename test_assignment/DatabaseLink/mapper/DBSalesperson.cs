using DatabaseLink.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DatabaseLink.mapper
{
    public class DBSalesperson : IDataAccessObject<Salesperson>
    {
        private DBConnect conn = null;

        public DBSalesperson()
        {
            conn = new DBConnect();
        }

        public void Delete(int id)
        {
            if(id < 1) throw new DataLayerArgumentException("Illegal ID value. ID value cannot be less or equal zero!", new ArgumentOutOfRangeException());

            string qry = $"exec spSalespersonDeleteById {id}";
            try
            {
                var link = conn.GetSqlConnection();
                SqlCommand cmd = new SqlCommand(qry, link);
                link.Open();
                var response = cmd.ExecuteReader();
                link.Close();
            }
            catch (Exception e)
            {
                throw new DataLayerException("Delete operation failed!", e);
            }
        }

        public Salesperson Get(int id)
        {
            string qry = $"exec spSalespersonGetById {id}";

            if (id < 1) throw new DataLayerArgumentException("ID cannot be less than zero!", new ArgumentException());
            Salesperson person = new Salesperson();
            try
            {
                //Database interaction
                var link = conn.GetSqlConnection();
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        try
                        {
                            person.Id = reader.GetInt32(0);
                            person.Name = reader.GetString(1);
                            person.LastName = reader.GetString(2);
                        }
                        catch (InvalidCastException e)
                        {
                            throw new DataLayerException($"Could not map a salesperson of ID: {id}", e);
                        }
                    }
                    link.Close();
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
            string qry = "exec spSalespersonGetAll";
            List<Salesperson> salespeople = new List<Salesperson>();
            try
            {
                var link = conn.GetSqlConnection();
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Salesperson temp = new Salesperson();
                            try
                            {
                                temp.Id = reader.GetInt32(0);
                                temp.Name = reader.GetString(1);
                                temp.LastName = reader.GetString(2);
                                salespeople.Add(temp);
                            }
                            catch (InvalidCastException e)
                            {
                                throw new DataLayerException($"Could not map a salesperson from the response.", e);
                            }
                        }
                    }
                    link.Close();
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
            if (t.LastName == null) t.LastName = String.Empty;

            string qry = $"exec spSalespersonCreate '{t.Name}','{t.LastName}'";
            try
            {
                var link = conn.GetSqlConnection();
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    var response = cmd.ExecuteReader();
                    link.Close();
                }
            }
            catch(Exception e)
            {
                throw new DataLayerException("Upload failure!", e);
            }
        }

        public void Update(Salesperson t)
        {
            if (t.Name == null || t.LastName == null) throw new DataLayerArgumentException("NULL values not allowed", new ArgumentNullException());
            if (t.Name.Equals(String.Empty) || t.LastName.Equals(String.Empty)) throw new DataLayerArgumentException("None of the fields can be empty!");           
            if (t.Id < 1) throw new DataLayerArgumentException("Illegal ID value. ID value cannot be less or equal zero!", new ArgumentOutOfRangeException());

            string qry = $"exec spSalespersonUpdate {t.Id},'{t.Name}','{t.LastName}'";
            try
            {
                var link = conn.GetSqlConnection();
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    var response = cmd.ExecuteReader();
                    link.Close();
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException("Update failure!", e);
            }
        }

    }
}
