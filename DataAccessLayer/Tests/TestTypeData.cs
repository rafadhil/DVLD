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
    public static class TestTypeData
    {
        public static DataTable GetTestTypes()
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT TestTypeID as ID, 
                             TestTypeTitle as Title,
                             TestTypeDescription as Description,
                             TestTypeFees as Fees
                             FROM TestTypes";
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

        public static bool UpdateTestType(int TypeID, String TypeTitle, String TypeDescription, decimal TypeFees)
        {
            int RowsAffected = -1;
            string Query = @"UPDATE TestTypes
                            SET 
                            TestTypeTitle = @Title,
                            TestTypeDescription = @Description,
                            TestTypeFees = @Fees
                            WHERE TestTypeID = @TypeID";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@Title", TypeTitle);
            command.Parameters.AddWithValue("@Description", TypeDescription);
            command.Parameters.AddWithValue("@Fees", TypeFees);
            command.Parameters.AddWithValue("@TypeID", TypeID);

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(DataLayerSettings.EventViewerSourceName, e.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return RowsAffected > 0;
        }

        public static bool GetTestTypeByID(int TypeID, ref String TypeTitle, ref String TypeDesciption, ref decimal TypeFees)
        {
            bool Found = false;
            string Query= @"SELECT TestTypeID as ID, 
                            TestTypeTitle as Title,
                            TestTypeDescription as Description,
                            TestTypeFees as Fees
                            FROM TestTypes
                            WHERE TestTypeID = @ID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", Convert.ToString(TypeID));

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    TypeTitle = (String)reader["Title"];
                    TypeDesciption = (String)reader["Description"];
                    TypeFees = (decimal)reader["Fees"];
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
