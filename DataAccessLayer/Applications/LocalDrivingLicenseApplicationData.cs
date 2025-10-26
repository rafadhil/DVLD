using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public static class LocalDrivingLicenseApplicationData
    {

        public static int GetActiveApplicationID_ForPersonAndLicenseClass(int PersonID, int LicenseClassID)
        {
            string Query = @"SELECT Applications.ApplicationID
                            FROM 
                            Applications JOIN LocalDrivingLicenseApplications 
                            ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE 
                            ApplicationStatus = 1 AND 
                            LicenseClassID = @ClassID AND 
                            ApplicantPersonID = @PersonID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ClassID", LicenseClassID);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool DoesPersonHaveLicenseOfLicenseClass(int PersonID, int LicenseClassID)
        {
            bool HasLicense = false;

            string Query = @"SELECT TOP 1 FOUND = 1 
                            FROM Licenses JOIN Drivers
                            ON Licenses.DriverID = Drivers.DriverID
                            WHERE 
                            Drivers.PersonID = @PersonID AND
                            LicenseClass = @ClassID";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ClassID", LicenseClassID);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                HasLicense = reader.HasRows;
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
            return HasLicense;
        }

        public static bool CanPersonApplyForLicenseWithClass(int PersonID, int LicenseClassID)
        {
            bool CanApply = false;
            string Query = @"SELECT Accepted = 1
                            FROM LicenseClasses
                            WHERE 
                            LicenseClassID = @LicenseClassID AND 
                            MinimumAllowedAge <= (SELECT  DATEDIFF(YEAR, DateOfBirth, GETDATE())
						                            FROM People
						                            WHERE PersonID = @PersonID)";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                CanApply = reader.HasRows;
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
            return CanApply;
        }

        public static int AddNewApplication(int OriginalApplicationID, int LicenseClassID)
        {
            string Query = @"INSERT INTO LocalDrivingLicenseApplications 
                            (ApplicationID, LicenseClassID)
                            Values
                            (@ApplicationID, @LicenseClassID);
                            SELECT SCOPE_IDENTITY();";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@ApplicationID", OriginalApplicationID);
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

        public static int GetOriginalApplicationID(int LDLapplicationID)
        {
            string Query = @"SELECT ApplicationID
                             FROM LocalDrivingLicenseApplications
                             WHERE LocalDrivingLicenseApplicationID = @ID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", LDLapplicationID);

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

        public static bool GetApplicationByID(int LDLapplicationID, ref int OriginalApplicationID,
            ref int LicenseClassID)
        {
            string query = @"SELECT ApplicationID,
                             LicenseClassID
                             FROM LocalDrivingLicenseApplications
                             WHERE LocalDrivingLicenseApplicationID = @ID;";
            bool Found = false;

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", LDLapplicationID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    OriginalApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                    LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
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

        public static DataTable GetAll_LDL_Applications()
        {
            string query = @"SELECT
                                    l.LocalDrivingLicenseApplicationID AS [L.D.L.AppID],
	                                lc.ClassName AS [Driving Class],
	                                p.NationalNo,
                                    CONCAT_WS(' ', p.FirstName, p.SecondName, p.ThirdName, p.LastName)  AS [Full Name],
	                                a.ApplicationDate AS [Application Date],
                                    SUM(CASE WHEN t.TestResult = 1 THEN 1 ELSE 0 END)                 AS [Passed Tests],
	                                [Status] = (CASE WHEN a.ApplicationStatus = 1 THEN 'New' WHEN a.ApplicationStatus = 2 THEN 'Cancelled' ELSE 'Completed' END)

                                FROM  LocalDrivingLicenseApplications l
                                JOIN  LicenseClasses       lc  ON l.LicenseClassID                 = lc.LicenseClassID
                                JOIN  Applications         a   ON l.ApplicationID                  = a.ApplicationID
                                JOIN  People               p   ON a.ApplicantPersonID              = p.PersonID
                                LEFT JOIN  TestAppointments     ta  ON l.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
                                LEFT JOIN  Tests                t   ON ta.TestAppointmentID             = t.TestAppointmentID
                                GROUP BY
                                    l.LocalDrivingLicenseApplicationID, lc.ClassName, p.NationalNo,
                                    p.FirstName, p.SecondName, p.ThirdName, p.LastName,
	                                a.ApplicationDate, a.ApplicationStatus;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();


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

        public static DataTable GetAll_LDL_ApplicationsByApplicationIDLike(int LDL_ApplicationID)
        {
            string query = @"SELECT * FROM
                                (
                                SELECT
                                    l.LocalDrivingLicenseApplicationID AS [L.D.L.AppID],
	                                lc.ClassName AS [Driving Class],
	                                p.NationalNo,
                                    CONCAT_WS(' ', p.FirstName, p.SecondName, p.ThirdName, p.LastName)  AS [Full Name],
	                                a.ApplicationDate AS [Application Date],
                                    SUM(CASE WHEN t.TestResult = 1 THEN 1 ELSE 0 END)                 AS [Passed Tests],
	                                [Status] = (CASE WHEN a.ApplicationStatus = 1 THEN 'New' WHEN a.ApplicationStatus = 2 THEN 'Cancelled' ELSE 'Completed' END)

                                FROM  LocalDrivingLicenseApplications l
                                JOIN  LicenseClasses       lc  ON l.LicenseClassID                 = lc.LicenseClassID
                                JOIN  Applications         a   ON l.ApplicationID                  = a.ApplicationID
                                JOIN  People               p   ON a.ApplicantPersonID              = p.PersonID
                                FULL JOIN  TestAppointments     ta  ON l.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
                                LEFT JOIN  Tests                t   ON ta.TestAppointmentID             = t.TestAppointmentID
                                GROUP BY
                                    l.LocalDrivingLicenseApplicationID, lc.ClassName, p.NationalNo,
                                    p.FirstName, p.SecondName, p.ThirdName, p.LastName,
	                                a.ApplicationDate, a.ApplicationStatus
	                                ) R1

                                WHERE R1.[L.D.L.AppID] LIKE @ID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();
            command.Parameters.AddWithValue("@ID", LDL_ApplicationID + "%");

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

        public static DataTable GetAll_LDL_ApplicationsByPersonNationalNumberLike(String NationalNumber)
        {
            string query = @"SELECT * FROM
                                (
                                SELECT
                                    l.LocalDrivingLicenseApplicationID AS [L.D.L.AppID],
	                                lc.ClassName AS [Driving Class],
	                                p.NationalNo,
                                    CONCAT_WS(' ', p.FirstName, p.SecondName, p.ThirdName, p.LastName)  AS [Full Name],
	                                a.ApplicationDate AS [Application Date],
                                    SUM(CASE WHEN t.TestResult = 1 THEN 1 ELSE 0 END)                 AS [Passed Tests],
	                                [Status] = (CASE WHEN a.ApplicationStatus = 1 THEN 'New' WHEN a.ApplicationStatus = 2 THEN 'Cancelled' ELSE 'Completed' END)

                                FROM  LocalDrivingLicenseApplications l
                                JOIN  LicenseClasses       lc  ON l.LicenseClassID                 = lc.LicenseClassID
                                JOIN  Applications         a   ON l.ApplicationID                  = a.ApplicationID
                                JOIN  People               p   ON a.ApplicantPersonID              = p.PersonID
                                FULL JOIN  TestAppointments     ta  ON l.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
                                LEFT JOIN  Tests                t   ON ta.TestAppointmentID             = t.TestAppointmentID
                                GROUP BY
                                    l.LocalDrivingLicenseApplicationID, lc.ClassName, p.NationalNo,
                                    p.FirstName, p.SecondName, p.ThirdName, p.LastName,
	                                a.ApplicationDate, a.ApplicationStatus
	                                ) R1

                                WHERE R1.[NationalNo] LIKE @NationalNumber ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();
            command.Parameters.AddWithValue("@NationalNumber", NationalNumber + "%");

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

        public static bool DeleteApplication(int ApplicationID)
        {
            int RowsAffected = 0;
            string Query = @"DELETE FROM LocalDrivingLicenseApplications 
                        Where LocalDrivingLicenseApplicationID = @ApplicationID";
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

        public static DataTable GetAll_LDL_ApplicationsByPersonFullNameLike(String FullName)
        {
            string query = @"SELECT * FROM
                                (
                                SELECT
                                    l.LocalDrivingLicenseApplicationID AS [L.D.L.AppID],
	                                lc.ClassName AS [Driving Class],
	                                p.NationalNo,
                                    CONCAT_WS(' ', p.FirstName, p.SecondName, p.ThirdName, p.LastName)  AS [Full Name],
	                                a.ApplicationDate AS [Application Date],
                                    SUM(CASE WHEN t.TestResult = 1 THEN 1 ELSE 0 END)                 AS [Passed Tests],
	                                [Status] = (CASE WHEN a.ApplicationStatus = 1 THEN 'New' WHEN a.ApplicationStatus = 2 THEN 'Cancelled' ELSE 'Completed' END)

                                FROM  LocalDrivingLicenseApplications l
                                JOIN  LicenseClasses       lc  ON l.LicenseClassID                 = lc.LicenseClassID
                                JOIN  Applications         a   ON l.ApplicationID                  = a.ApplicationID
                                JOIN  People               p   ON a.ApplicantPersonID              = p.PersonID
                                FULL JOIN  TestAppointments     ta  ON l.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
                                LEFT JOIN  Tests                t   ON ta.TestAppointmentID             = t.TestAppointmentID
                                GROUP BY
                                    l.LocalDrivingLicenseApplicationID, lc.ClassName, p.NationalNo,
                                    p.FirstName, p.SecondName, p.ThirdName, p.LastName,
	                                a.ApplicationDate, a.ApplicationStatus
	                                ) R1

                                WHERE R1.[Full Name] LIKE @FullName ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();
            command.Parameters.AddWithValue("@FullName", FullName + "%");

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

        public static DataTable GetAllNew_LDL_Applications()
        {
            string query = @"SELECT * FROM
                                (
                                SELECT
                                    l.LocalDrivingLicenseApplicationID AS [L.D.L.AppID],
	                                lc.ClassName AS [Driving Class],
	                                p.NationalNo,
                                    CONCAT_WS(' ', p.FirstName, p.SecondName, p.ThirdName, p.LastName)  AS [Full Name],
	                                a.ApplicationDate AS [Application Date],
                                    SUM(CASE WHEN t.TestResult = 1 THEN 1 ELSE 0 END)                 AS [Passed Tests],
	                                [Status] = (CASE WHEN a.ApplicationStatus = 1 THEN 'New' WHEN a.ApplicationStatus = 2 THEN 'Cancelled' ELSE 'Completed' END)

                                FROM  LocalDrivingLicenseApplications l
                                JOIN  LicenseClasses       lc  ON l.LicenseClassID                 = lc.LicenseClassID
                                JOIN  Applications         a   ON l.ApplicationID                  = a.ApplicationID
                                JOIN  People               p   ON a.ApplicantPersonID              = p.PersonID
                                FULL JOIN  TestAppointments     ta  ON l.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
                                LEFT JOIN  Tests                t   ON ta.TestAppointmentID             = t.TestAppointmentID
                                GROUP BY
                                    l.LocalDrivingLicenseApplicationID, lc.ClassName, p.NationalNo,
                                    p.FirstName, p.SecondName, p.ThirdName, p.LastName,
	                                a.ApplicationDate, a.ApplicationStatus
	                                ) R1

                                WHERE R1.Status = 'New';";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();

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

        public static DataTable GetAllCancelled_LDL_Applications()
        {
            string query = @"SELECT * FROM
                                (
                                SELECT
                                    l.LocalDrivingLicenseApplicationID AS [L.D.L.AppID],
	                                lc.ClassName AS [Driving Class],
	                                p.NationalNo,
                                    CONCAT_WS(' ', p.FirstName, p.SecondName, p.ThirdName, p.LastName)  AS [Full Name],
	                                a.ApplicationDate AS [Application Date],
                                    SUM(CASE WHEN t.TestResult = 1 THEN 1 ELSE 0 END)                 AS [Passed Tests],
	                                [Status] = (CASE WHEN a.ApplicationStatus = 1 THEN 'New' WHEN a.ApplicationStatus = 2 THEN 'Cancelled' ELSE 'Completed' END)

                                FROM  LocalDrivingLicenseApplications l
                                JOIN  LicenseClasses       lc  ON l.LicenseClassID                 = lc.LicenseClassID
                                JOIN  Applications         a   ON l.ApplicationID                  = a.ApplicationID
                                JOIN  People               p   ON a.ApplicantPersonID              = p.PersonID
                                FULL JOIN  TestAppointments     ta  ON l.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
                                LEFT JOIN  Tests                t   ON ta.TestAppointmentID             = t.TestAppointmentID
                                GROUP BY
                                    l.LocalDrivingLicenseApplicationID, lc.ClassName, p.NationalNo,
                                    p.FirstName, p.SecondName, p.ThirdName, p.LastName,
	                                a.ApplicationDate, a.ApplicationStatus
	                                ) R1

                                WHERE R1.Status = 'Cancelled';";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();

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

        public static DataTable GetAllCompleted_LDL_Applications()
        {
            string query = @"SELECT * FROM
                                (
                                SELECT
                                    l.LocalDrivingLicenseApplicationID AS [L.D.L.AppID],
	                                lc.ClassName AS [Driving Class],
	                                p.NationalNo,
                                    CONCAT_WS(' ', p.FirstName, p.SecondName, p.ThirdName, p.LastName)  AS [Full Name],
	                                a.ApplicationDate AS [Application Date],
                                    SUM(CASE WHEN t.TestResult = 1 THEN 1 ELSE 0 END)                 AS [Passed Tests],
	                                [Status] = (CASE WHEN a.ApplicationStatus = 1 THEN 'New' WHEN a.ApplicationStatus = 2 THEN 'Cancelled' ELSE 'Completed' END)

                                FROM  LocalDrivingLicenseApplications l
                                JOIN  LicenseClasses       lc  ON l.LicenseClassID                 = lc.LicenseClassID
                                JOIN  Applications         a   ON l.ApplicationID                  = a.ApplicationID
                                JOIN  People               p   ON a.ApplicantPersonID              = p.PersonID
                                FULL JOIN  TestAppointments     ta  ON l.LocalDrivingLicenseApplicationID = ta.LocalDrivingLicenseApplicationID
                                LEFT JOIN  Tests                t   ON ta.TestAppointmentID             = t.TestAppointmentID
                                GROUP BY
                                    l.LocalDrivingLicenseApplicationID, lc.ClassName, p.NationalNo,
                                    p.FirstName, p.SecondName, p.ThirdName, p.LastName,
	                                a.ApplicationDate, a.ApplicationStatus
	                                ) R1

                                WHERE R1.Status = 'Completed';";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();

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

        public static int GetNumberOfPassedTests(int LDL_ApplicationID)
        {
            string Query = @"SELECT SUM(CASE WHEN T.TestResult = 1 THEN 1 ELSE 0 END)
                                        FROM LocalDrivingLicenseApplications LDL_APP
                                        LEFT JOIN TestAppointments TA
                                        ON LDL_APP.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID
                                        LEFT JOIN Tests T ON TA.TestAppointmentID = T.TestAppointmentID
                                        WHERE LDL_APP.LocalDrivingLicenseApplicationID = @ID
                                        GROUP BY LDL_APP.LocalDrivingLicenseApplicationID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", LDL_ApplicationID);
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
        //public static bool DeleteApplication()
        //{

        //}
    }
}
