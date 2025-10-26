using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DataAccessLayer
{
    public static class LicenseClassData
    {
        public static DataTable GetLicenseClassses()
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT LicenseClassID as ID,
                             ClassName as Name,
                             ClassDescription as Description,
                             MinimumAllowedAge,
                             DefaultValidityLength,
                             ClassFees as Fees
                             FROM LicenseClasses";
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
                EventLog.WriteEntry(DataLayerSettings.EventViewerSourceName, e.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();

            }
            return DT;
        }

        

        public static bool GetClassByID(int LicenseClassID, ref String ClassName, ref String ClassDescription,
                ref byte MinimumAllowedAge, ref byte DefaultValidityLength,
                ref decimal ClassFees)
        {
            bool Found = false;
            string Query = @"SELECT LicenseClassID as ID,
                             ClassName as Name,
                             ClassDescription as Description,
                             MinimumAllowedAge,
                             DefaultValidityLength,
                             ClassFees as Fees
                             FROM LicenseClasses
                             WHERE LicenseClassID = @ID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", Convert.ToString(LicenseClassID));

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ClassName = (String)reader["Name"];
                    ClassDescription = (String)reader["Description"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = (decimal)reader["Fees"];
                    Found = true;
                }

                reader.Close();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(DataLayerSettings.EventViewerSourceName, e.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();

            }
            return Found;
        }

        
    }
}
