using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public static class TestData
    {
        public static int AddNewTest(int TestAppointmentID, bool TestResult, String Notes, int CreatedByUserID)
        {
            String Query = @"INSERT INTO Tests
                                (TestAppointmentID, TestResult, Notes, CreatedByUserID)
                                VALUES
                                (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);

                                SELECT SCOPE_IDENTITY();";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult? "1" : "0");
            command.Parameters.AddWithValue("@Notes", String.IsNullOrEmpty(Notes)? DBNull.Value.ToString() : Notes);
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

        public static int GetNumberOfFailedTests(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            String Query = @"SELECT [Number Of Failed Tests]
                            FROM
                            (
	                            SELECT TestTypes.TestTypeID,
	                             [Number Of Failed Tests] = SUM(CASE WHEN R1.TestResult = 0 THEN 1 ELSE 0 END)  
	                             FROM 
	                             (
		                             SELECT LocalDrivingLicenseApplicationID, TestTypeID, TestResult
		                              FROM Tests JOIN TestAppointments 
		                              ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
		                              WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
	                              ) R1
	                              RIGHT JOIN TestTypes ON R1.TestTypeID = TestTypes.TestTypeID
	                              GROUP BY R1.LocalDrivingLicenseApplicationID, TestTypes.TestTypeID
                            )R2
                            WHERE R2.TestTypeID = @TestTypeID";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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

        public static int GetNumberOfPassedTests(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            String Query = @"SELECT [Number Of Passed Tests]
                                FROM
                                (
	                                SELECT TestTypes.TestTypeID,
	                                 [Number Of Passed Tests] = SUM(CASE WHEN R1.TestResult = 1 THEN 1 ELSE 0 END)  
	                                 FROM 
	                                 (
		                                 SELECT LocalDrivingLicenseApplicationID, TestTypeID, TestResult
		                                  FROM Tests JOIN TestAppointments 
		                                  ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
		                                  WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
	                                  ) R1
	                                  RIGHT JOIN TestTypes ON R1.TestTypeID = TestTypes.TestTypeID
	                                  GROUP BY R1.LocalDrivingLicenseApplicationID, TestTypes.TestTypeID
                                )R2
                                WHERE R2.TestTypeID = @TestTypeID ";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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

        public static bool SetRetakeTestApplicationToCompleted(int TestAppointmentID)
        {
            int RowsAffected = -1;
            string Query = @"UPDATE Applications 
                             SET ApplicationStatus = 3
                             WHERE 
                             ApplicationID IN (
						                        SELECT RetakeTestApplicationID 
						                        FROM TestAppointments
						                        WHERE TestAppointmentID = @TestAppointmentID ); ";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
