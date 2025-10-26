using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public static class ApplicationData
    {

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate,
            int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate,
            decimal PaidFees, int CreatedByUserID)
        {

            String Query = @"INSERT INTO Applications 
                             (
                             ApplicantPersonID,
                             ApplicationDate,
                             ApplicationTypeID,
                             ApplicationStatus,
                             LastStatusDate,
                             PaidFees,
                             CreatedByUserID
                             )
                             VALUES
                             (
                             @ApplicantPersonID,
                             @ApplicationDate,
                             @ApplicationTypeID,
                             @ApplicationStatus,
                             @LastStatusDate,
                             @PaidFees,
                             @CreatedByUserID
                             );
                           
                             SELECT SCOPE_IDENTITY();";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            object result = null;

            try
            {
                connection.Open();
                result = command.ExecuteScalar();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(DataLayerSettings.EventViewerSourceName, e.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return result != null ? Convert.ToInt32(result) : -1;
        }

        public static bool UpdateApplicationStatus(int ApplicationID, byte ApplicationStatus)
        {
            int RowsAffected = -1;
            string Query = @"UPDATE Applications
                             SET
                             ApplicationStatus = @ApplicationStatus,
                             LastStatusDate = @TodayDate
                             WHERE ApplicationID = @ApplicationID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@TodayDate", DateTime.Now);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
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

        public static bool DeleteApplication(int ApplicationID)
        {
            int RowsAffected = 0;
            string Query = @"DELETE FROM Applications 
                        Where ApplicationID = @ApplicationID";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(DataLayerSettings.EventViewerSourceName, e.Message, EventLogEntryType.Error);
            }

            connection.Close();
            return RowsAffected > 0;
        }

        public static bool GetApplicationByID(int ApplicationID, ref int ApplicantID, 
            ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte ApplicationStatus,
            ref DateTime LastStatusDate, ref decimal PaidFees, ref int CreatedByUserID)
        {
            string query = @"SELECT ApplicantPersonID,
                             ApplicationDate, ApplicationTypeID, ApplicationStatus,
                             LastStatusDate, PaidFees, CreatedByUserID
                             FROM Applications
                             WHERE ApplicationID = @ID;";
            bool Found = false;

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", ApplicationID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ApplicantID = (int)reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]);
                    ApplicationStatus = Convert.ToByte(reader["ApplicationStatus"]);
                    LastStatusDate = Convert.ToDateTime(reader["LastStatusDate"]);
                    PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
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
