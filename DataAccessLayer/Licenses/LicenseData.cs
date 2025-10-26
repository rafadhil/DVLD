using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public static class LicenseData
    {
        public static int IssueNewLicense(int applicationID, int driverID, int licenseClassID, string notes,
            decimal paidFees, bool isActive, byte issueReason, int createdByUserID)
        {
            String Query = @"

            INSERT INTO Licenses
                (
	                ApplicationID,
	                DriverID,
	                LicenseClass,
	                IssueDate,
	                ExpirationDate,
	                Notes,
	                PaidFees,
	                IsActive,
	                IssueReason,
	                CreatedByUserID
                )
                VALUES
                (
	                @ApplicationID,
	                @DriverID,
	                @LicenseClassID,
	                GETDATE(),
                    DATEADD(YEAR, (SELECT
					                DefaultValidityLength
					                FROM LicenseClasses
					                WHERE LicenseClassID = @LicenseClassID2 ), GETDATE()),
	                @Notes,
	                @PaidFees,
	                @IsActive,
	                @IssueReason,
	                @CreatedByUserID
                );
                SELECT SCOPE_IDENTITY();";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@DriverID", driverID);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@LicenseClassID2", licenseClassID);
            command.Parameters.AddWithValue("@Notes", notes ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PaidFees", paidFees);
            command.Parameters.AddWithValue("@IsActive", isActive ? "1" : "0");
            command.Parameters.AddWithValue("@IssueReason", issueReason);
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


        public static int DetainLicense(int LicenseID, decimal FineFees, int CreatedByUserID)
        {

            String Query = @"INSERT INTO DetainedLicenses
                                (
	                                LicenseID,
	                                DetainDate,
	                                FineFees,
	                                CreatedByUserID,
	                                IsReleased,
	                                ReleaseDate,
	                                ReleasedByUserID,
	                                ReleaseApplicationID
                                ) VALUES
                                (
	                                @LicenseID,
	                                GETDATE(),
	                                @FineFees,
	                                @CreatedByUserID,
	                                0,
	                                NULL,
	                                NULL,
	                                NULL
                                ) ;

                                SELECT SCOPE_IDENTITY(); ";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@FineFees", FineFees);
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

        public static bool HasActiveLicenseOfClassType(int PersonID, int DriverLicenseClassID)
        {
            bool Found = false;
            string Query = @"SELECT Found=1
                            FROM Licenses L JOIN Drivers D ON L.DriverID = D.DriverID
                            WHERE D.PersonID = @PersonID
                            AND IsActive = 1
                            AND L.LicenseClass = @LicenseClassID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", DriverLicenseClassID);

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

        public static bool HasLicenseOfClassType(int PersonID, int DriverLicenseClassID)
        {
            bool Found = false;
            string Query = @"SELECT Found=1
                            FROM Licenses L JOIN Drivers D ON L.DriverID = D.DriverID
                            WHERE D.PersonID = @PersonID
                            AND L.LicenseClass = @LicenseClassID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", DriverLicenseClassID);

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

        public static bool GetLicenseByLicenseID(int LicenseID, ref int applicationID, ref int driverID, ref int licenseClassID, ref DateTime issueDate,
            ref DateTime expirationDate, ref string notes,
           ref decimal paidFees, ref bool isActive, ref byte issueReason, ref int createdByUserID)
        {
            bool Found = false;
            string Query = @"SELECT * FROM Licenses
                                WHERE LicenseID = @LicenseID;";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    applicationID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    licenseClassID = (int)reader["LicenseClass"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    notes = Convert.IsDBNull(reader["Notes"]) ? null : reader["Notes"].ToString();
                    paidFees = (decimal)reader["PaidFees"];
                    isActive = (bool)reader["IsActive"];
                    issueReason = (byte)reader["IssueReason"];
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

        public static bool GetLicenseByApplicationID(int applicationID , ref int LicenseID, ref int driverID, ref int licenseClassID, ref DateTime issueDate,
            ref DateTime expirationDate, ref string notes,
           ref decimal paidFees, ref bool isActive, ref byte issueReason, ref int createdByUserID)
        {
            bool Found = false;
            string Query = @"SELECT * FROM Licenses
                                WHERE ApplicationID = @ApplicationID;";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ApplicationID", applicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    LicenseID = (int)reader["LicenseID"];
                    driverID = (int)reader["DriverID"];
                    licenseClassID = (int)reader["LicenseClass"];
                    notes = Convert.IsDBNull(reader["Notes"]) ? null : reader["Notes"].ToString();
                    paidFees = (decimal)reader["PaidFees"];
                    isActive = (bool)reader["IsActive"];
                    issueReason = (byte)reader["IssueReason"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
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

        public static DataTable GetAllLicensesForPerson(int PersonID)
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT LicenseID as [License ID], ApplicationID, ClassName, IssueDate, ExpirationDate, IsActive
                            FROM Licenses L 
                            JOIN LicenseClasses LC
                            ON L.LicenseClass = LC.LicenseClassID
                            JOIN Drivers D
                            ON L.DriverID = D.DriverID 
                            WHERE D.PersonID = @PersonID; ";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool ActivateLicense(int DriverLicenseID)
        {
            int RowsAffected = -1;
            string Query = @"Update Licenses
                                SET IsActive = 1
                                WHERE LicenseID = @DriverLicenseID ;";


            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@DriverLicenseID", DriverLicenseID);


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

        public static bool DeactivateLicense(int DriverLicenseID)
        {
            int RowsAffected = -1;
            string Query = @"Update Licenses
                                SET IsActive = 0
                                WHERE LicenseID = @DriverLicenseID ;";


            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@DriverLicenseID", DriverLicenseID);


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
    }

   
}
