using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer.Licenses
{
    public static class InternationalLicenseData
    {
        public static int IssueNewLicense(int applicationID, int driverID, int localLicenseID, int createdByUserID)
        {
            String Query = @"
                                INSERT INTO InternationalLicenses
                                (
	                                ApplicationID,
	                                DriverID,
	                                IssuedUsingLocalLicenseID,
	                                IssueDate,
	                                ExpirationDate,
	                                IsActive,
	                                CreatedByUserID
                                )
                                VALUES
                                (
	                                @ApplicationID,
	                                @DriverID,
	                                @LocalLicenseID,
	                                GETDATE(),
                                    DATEADD(YEAR, 1, GETDATE()),
	                                1,
	                                @CreatedByUserID
                                );
                                SELECT SCOPE_IDENTITY();";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@DriverID", driverID);
            command.Parameters.AddWithValue("@LocalLicenseID", localLicenseID);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

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

        public static bool HasActiveInternationalLicense(int DriverID)
        {
            bool Found = false;
            string Query = @"SELECT Found = 1
                                FROM InternationalLicenses
                                WHERE DriverID = @DriverID
                                AND IsActive = 1;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Found = reader.HasRows;
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

        public static bool HasInternationalLicense(int DriverID)
        {
            bool Found = false;
            string Query = @"SELECT Found = 1
                                FROM InternationalLicenses
                                WHERE DriverID = @DriverID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Found = reader.HasRows;
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

        public static bool GetLicenseByLicenseID(int LicenseID, ref int applicationID, ref int driverID, ref int issuedUsingLocalLicenseID,
            ref DateTime issueDate, ref DateTime expirationDate, ref bool isActive, ref int createdByUserID)
        {
            bool Found = false;
            string Query = @"SELECT *
                                FROM InternationalLicenses
                                WHERE InternationalLicenseID = @InternationalLicenseID  ;";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    applicationID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    issuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    isActive = (bool)reader["IsActive"];
                    createdByUserID = (int)reader["CreatedByUserID"];

                    Found = true;
                }
                else
                {
                    Found = false;
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

        public static bool GetLicenseByLocalLicenseID(int issuedUsingLocalLicenseID, ref int LicenseID, ref int applicationID, ref int driverID,
          ref DateTime issueDate, ref DateTime expirationDate, ref bool isActive, ref int createdByUserID)
        {
            bool Found = false;
            string Query = @"SELECT *
                                    FROM InternationalLicenses
                                    WHERE IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID ;";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", issuedUsingLocalLicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    applicationID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    LicenseID = (int)reader["InternationalLicenseID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    isActive = (bool)reader["IsActive"];
                    createdByUserID = (int)reader["CreatedByUserID"];

                    Found = true;
                }
                else
                {
                    Found = false;
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

        public static bool GetLicenseByApplicationID(int applicationID , ref int LicenseID, ref int driverID, ref int issuedUsingLocalLicenseID,
         ref DateTime issueDate, ref DateTime expirationDate, ref bool isActive, ref int createdByUserID)
        {
            bool Found = false;
            string Query = @"SELECT *
                                FROM InternationalLicenses
                                WHERE ApplicationID = @ApplicationID ;";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ApplicationID", applicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    LicenseID = (int)reader["InternationalLicenseID"];
                    driverID = (int)reader["DriverID"];
                    issuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    isActive = (bool)reader["IsActive"];
                    createdByUserID = (int)reader["CreatedByUserID"];

                    Found = true;
                }
                else
                {
                    Found = false;
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

        public static DataTable GetAllLicensesForDriver(int DriverID)
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT 
                                InternationalLicenseID as [License ID],
	                            ApplicationID, 
	                            IssueDate as [Issue Date],
	                            ExpirationDate as [Expiration Date],
	                            IsActive as [Is Active]
                            FROM InternationalLicenses
                            Where DriverID = @DriverID";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

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

        public static DataTable GetAllLicenses()
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT 
                                InternationalLicenseID as [License ID],
                                ApplicationID as [Application ID],
                                DriverID as [Driver ID], 
	                            IssuedUsingLocalLicenseID as [Loc. LicenseID],                     
                                IssueDate as [Issue Date],
                                ExpirationDate as [Expiration Date],
                                IsActive as [Is Active]
                                FROM InternationalLicenses ;";

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


        public static DataTable GetAllLicensesByLicenseIDLike(string LicenseID)
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT 
                                InternationalLicenseID as [License ID],
                                ApplicationID as [Application ID],
                                DriverID as [Driver ID], 
	                            IssuedUsingLocalLicenseID as [Loc. LicenseID],                     
                                IssueDate as [Issue Date],
                                ExpirationDate as [Expiration Date],
                                IsActive as [Is Active]
                                FROM InternationalLicenses
                                WHERE InternationalLicenseID LIKE @LicenseID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID + "%");


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


        public static DataTable GetAllLicensesByLocalLicenseIDLike(string LocalLicenseID)
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT 
                                InternationalLicenseID as [License ID],
                                ApplicationID as [Application ID],
                                DriverID as [Driver ID], 
	                            IssuedUsingLocalLicenseID as [Loc. LicenseID],                     
                                IssueDate as [Issue Date],
                                ExpirationDate as [Expiration Date],
                                IsActive as [Is Active]
                                FROM InternationalLicenses 
                                WHERE IssuedUsingLocalLicenseID LIKE @LicenseID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseID", LocalLicenseID + "%");


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

        public static DataTable GetAllLicensesByDriverIDLike(string DriverID)
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT 
                                InternationalLicenseID as [License ID],
                                ApplicationID as [Application ID],
                                DriverID as [Driver ID], 
	                            IssuedUsingLocalLicenseID as [Loc. LicenseID],                     
                                IssueDate as [Issue Date],
                                ExpirationDate as [Expiration Date],
                                IsActive as [Is Active]
                                FROM InternationalLicenses 
                                WHERE DriverID LIKE @DriverID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID + "%");


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



    }
}
