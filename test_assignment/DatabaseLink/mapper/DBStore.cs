using DatabaseLink.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink.mapper
{
    public class DBStore : IDataAccessObject<Store>
    {

        private DBConnect conn = null;

        public DBStore()
        {
            conn = new DBConnect();
        }

        public void Delete(Store t)
        {
            if (t.Id < 1) throw new DataLayerArgumentException("Illegal ID value. ID value cannot be less or equal zero!", new ArgumentOutOfRangeException());

            string qry = $"exec spStoreDeleteById {t.Id}";
            try
            {
                var link = conn.GetSqlConnection();
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    var respoonse = cmd.ExecuteReader();
                    link.Close();
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException("Delete operation failed!", e);
            }
        }

        public Store Get(int id)
        {
            if (id < 1) throw new DataLayerArgumentException("ID cannot be less than zero!", new ArgumentException());
            string qry = $"exec spStoreGetById {id}";

            Store store = new Store();
            try
            {
                var link = conn.GetSqlConnection();
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        try
                        {
                            store.Id = reader.GetInt32(0);
                            store.Name = reader.GetString(1);
                            store.Address = reader.GetString(2);
                            store.District = new District();
                            store.District.Id = reader.GetInt32(3);
                            store.District.Name = reader.GetString(4);
                            store.District.PrimarySalesperson = new Salesperson();
                            store.District.PrimarySalesperson.Id = reader.GetInt32(5);
                            store.District.PrimarySalesperson.Name = reader.GetString(6);
                            store.District.PrimarySalesperson.LastName = reader.GetString(7);
                        }
                        catch (InvalidCastException e)
                        {
                            throw new DataLayerException($"Could not map a store of ID: {id}", e);
                        }
                    }
                    link.Close();
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException($"Could not retrieve a store of ID: {id}!\n", e);
            }
            return store;
        }

        public IEnumerable<Store> GetAll()
        {
            string qry = "exec spStoreGetAll";
            List<Store> stores = new List<Store>();
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
                            Store temp = new Store();
                            try
                            {
                                temp.Id = reader.GetInt32(0);
                                temp.Name = reader.GetString(1);
                                temp.Address = reader.GetString(2);
                                temp.District = new District();
                                temp.District.Id = reader.GetInt32(3);
                                temp.District.Name = reader.GetString(4);
                                temp.District.PrimarySalesperson = new Salesperson();
                                temp.District.PrimarySalesperson.Id = reader.GetInt32(5);
                                temp.District.PrimarySalesperson.Name = reader.GetString(6);
                                temp.District.PrimarySalesperson.LastName = reader.GetString(7);
                                stores.Add(temp);
                            }
                            catch (InvalidCastException e)
                            {
                                throw new DataLayerException($"Could not map a store from the response.", e);
                            }
                        }
                    }
                    link.Close();
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException($"Could not retrieve the list of stores!\n", e);
            }
            return stores.AsEnumerable();
        }

        public void Persist(Store t)
        {
            if (t.District == null) throw new DataLayerArgumentException("District information missing!", new ArgumentNullException());
            if (t.District.Id < 1) throw new DataLayerArgumentException("Invalid district ID!", new ArgumentOutOfRangeException());

            string qry = $"exec spStoreCreate '{t.Name}','{t.Address}',{t.District.Id}";

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
                throw new DataLayerException("Upload failure!", e);
            }
        }

        public void Update(Store t)
        {
            if (t.Id < 1) throw new DataLayerArgumentException("Invalid store ID!", new ArgumentOutOfRangeException());
            if (t.District == null) throw new DataLayerArgumentException("District information missing!", new ArgumentNullException());
            if (t.District.Id < 1) throw new DataLayerArgumentException("Invalid district ID!", new ArgumentOutOfRangeException());
            if (t.Name == null) t.Name = String.Empty;
            if (t.Address == null) t.Address = String.Empty;

            string qry = $"exec spStoreUpdate {t.Id},'{t.Name}','{t.Address}',{t.District.Id}";
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
