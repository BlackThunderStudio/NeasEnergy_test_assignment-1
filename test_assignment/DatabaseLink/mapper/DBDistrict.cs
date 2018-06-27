using DatabaseLink.model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink.mapper
{
    public class DBDistrict : IDataAccessObject<District>, ISecondarySalesman
    {

        private DBConnect conn = null;

        public DBDistrict()
        {
            conn = new DBConnect();
        }

        public void AssignSecondary(Salesperson person, District district)
        {
            if (person.Id < 1) throw new DataLayerException("Invalid Salesperson ID!", new ArgumentOutOfRangeException());
            if (district.Id < 1) throw new DataLayerException("Invalid District ID!", new ArgumentOutOfRangeException());

            string qry = $"if exists (select 1 from people where id={person.Id})\n and exists (select 1 from districts where id={district.Id})\n and not exists (select 1 from districtSecondarySalesperson where personid={person.Id} and districtid={district.Id})\n begin\n insert into districtSecondarySalesperson (PersonId,DistrictId) values ({person.Id},{district.Id})\n end";
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
                throw new DataLayerException("Cannot assign a secondary salesperson.", e);
            }
        }

        public void Delete(District t)
        {
            if (t.Id < 1) throw new DataLayerException("Illegal ID value. ID value cannot be less or equal zero!", new ArgumentOutOfRangeException());

            //delete the district itself
            var link = conn.GetSqlConnection();
            string qry = $"if exists (select 1 from districts where id={t.Id})\n begin\n delete from districts where id={t.Id}\n end";
            try
            {
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    var response = cmd.ExecuteReader();
                    link.Close();
                }
                //when successful, delete the stores within the district
                qry = $"delete from stores where districtid={t.Id}";
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    var response = cmd.ExecuteReader();
                    link.Close();
                }
                //then delete all the assigned secondary salespeople
                qry = $"delete from districtSecondarySalesperson where districtid={t.Id}";
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    var response = cmd.ExecuteReader();
                    link.Close();
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException("Delete operation failed!", e);
            }
        }

        public void DeleteSecondary(Salesperson person, District district)
        {
            if (person.Id < 1 || district.Id < 1) throw new DataLayerException("Illegal ID value. ID value cannot be less or equal zero!", new ArgumentOutOfRangeException());

            string qry = $"delete from districtSecondarySalesperson where personid={person.Id} and districtid={district.Id}";

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
                throw new DataLayerException("Delete operation failed!", e);
            }
        }

        public District Get(int id)
        {
            if (id < 1) throw new DataLayerException("ID cannot be less than zero!", new ArgumentException());
            string qry = $"if exists (select 1 from districts where Id={id})\n begin\n select d.*,p.* from districts as d inner join people as p on d.PrimarySalesId=p.Id where d.Id={id}\n end";

            District district = new District();
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
                            district.Id = reader.GetInt32(0);
                            district.Name = reader.GetString(1);
                            district.PrimarySalesperson = new Salesperson() { Id = reader.GetInt32(2), Name = reader.GetString(4), LastName = reader.GetString(5) };
                        }
                        catch (InvalidCastException e)
                        {
                            throw new DataLayerException($"Could not map a district of ID: {id}", e);
                        }
                    }
                    link.Close();
                }
                qry = $"if exists (select 1 from districts where Id={id})\n begin\n select p.* from people as p inner join districtSecondarySalesperson as ds on p.Id=ds.PersonId where ds.DistrictId={id}\n end";
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Salesperson> people = new List<Salesperson>();
                        while (reader.Read())
                        {
                            var person = new Salesperson();
                            try
                            {
                                person.Id = reader.GetInt32(0);
                                person.Name = reader.GetString(1);
                                person.LastName = reader.GetString(2);
                                people.Add(person);
                            }
                            catch (InvalidCastException e)
                            {
                                throw new DataLayerException($"Could not map a district from the response.", e);
                            }
                        }
                        district.SecondarySalespeople = people.AsEnumerable();
                    }
                    link.Close();
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException($"Could not retrieve a district of ID: {id}!\n", e);
            }
            return district;
        }

        public IEnumerable<District> GetAll()
        {
            string qry = $"select d.Id from districts as d";
            List<int> ids = new List<int>();
            List<District> districts = new List<District>();
            try
            {
                var link = conn.GetSqlConnection();
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tmp = 0;
                            try
                            {
                                tmp = reader.GetInt32(0);
                                ids.Add(tmp);
                            }
                            catch (InvalidCastException e)
                            {
                                throw new DataLayerException($"Could not map a response.", e);
                            }
                        }
                    }
                    link.Close();
                }
                foreach(int id in ids)
                {
                    districts.Add(Get(id));
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException($"Could not retrieve the list of districts!\n", e);
            }
            return districts.AsEnumerable();
        }

        public void Persist(District t)
        {
            if (t.PrimarySalesperson == null) throw new DataLayerException("Primary salesperson missing!", new ArgumentNullException());
            if (t.Name == null || t.Name.Equals(String.Empty)) throw new DataLayerException("District name missing or empty!", new ArgumentException());

            string qry = $"if not exists (select 1 from districts where id={t.Id})\n begin\n insert into districts (name,primarysalesid) values ('{t.Name}','{t.PrimarySalesperson.Id}')\n end";
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

        public void Update(District t)
        {
            if (t.Id < 1) throw new DataLayerException("Invalid district ID!", new ArgumentOutOfRangeException());
            if (t.Name == null) t.Name = String.Empty;
            if (t.PrimarySalesperson == null) throw new DataLayerException("Primary salesperson missing!", new ArgumentNullException());
            if (t.PrimarySalesperson.Id < 1) throw new DataLayerException("Invalid primary salesperson ID!", new ArgumentOutOfRangeException());

            string qry = $"if exists (select 1 from districts where id={t.Id})\n begin\n update districts set name='{t.Name}',primarysalesid={t.PrimarySalesperson.Id} where id={t.Id}\n end";
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
