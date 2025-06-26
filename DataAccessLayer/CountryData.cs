using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CountryData
    {
        public static DataTable GetAllCountries()
        {

            DataTable DT = new DataTable();
            string Query = "SELECT CountryName From Countries;";
            //string Query = "SELECT " + (IncludeCountryID? "*" : "CountryName") +  " FROM Countries; ";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DT.Load(reader);


                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();

            }
            return DT;

        }

        public static String GetCountryByID(int CountryID)
        {
            String CountryName = "";
            string Query = "SELECT CountryName FROM Countries Where CountryID = @ID ; ";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", Convert.ToString(CountryID));

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    CountryName = reader.GetString(0);
                }
                else
                    ;


                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();

            }
            return CountryName;

        }
    }
    
}
