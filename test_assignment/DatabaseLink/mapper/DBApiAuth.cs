using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink.mapper
{
    public class DBApiAuth : IApiAuth
    {
        public bool Validate(string key)
        {
            DBConnect db = new DBConnect();
            string qry = $"exec spValidateApiKey '{key}'";
            int response;
            try
            {
                var link = db.GetSqlConnection();
                using(SqlCommand cmd = new SqlCommand(qry, link))
                {
                    link.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        try
                        {
                            response = reader.GetInt32(0);
                        }
                        catch (InvalidCastException e)
                        {
                            throw new DataLayerException($"Could not map the result!", e);
                        }
                    }
                    link.Close();
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException($"Could not validate the API Key. Server error.", e);
            }
            return response == 1;
        }
    }
}
