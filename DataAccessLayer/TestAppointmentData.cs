using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TestAppointmentData
    {
        public static DataTable GetAppointmentsForPersonWithID(int PersonID)
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT TA.TestTypeID, TA.LocalDrivingLicenseApplicationID,
                                TA.AppointmentDate, TA.PaidFees, TA.CreatedByUserID, TA.IsLocked
                                FROM TestAppointments TA
                                JOIN  LocalDrivingLicenseApplications LDL_App 
                                ON TA.LocalDrivingLicenseApplicationID = LDL_App.LocalDrivingLicenseApplicationID
                                JOIN Applications A ON LDL_App.ApplicationID = A.ApplicationID
                                JOIN People ON A.ApplicantPersonID = People.PersonID
                                WHERE PersonID = @ID";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", Convert.ToString(PersonID));
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

        public static bool UpdateAppointment(int AppointmentID,
            DateTime AppointmentDate, bool IsLocked)
        {
            int RowsAffected = -1;
            string Query = @"UPDATE TestAppointments
                            SET 
                            AppointmentDate = @Date,
                            IsLocked = @IsLocked
                            WHERE TestAppointmentID = @ID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@Date", AppointmentDate);
            command.Parameters.AddWithValue("@IsLocked", IsLocked? "1": "0");
            command.Parameters.AddWithValue("@ID", AppointmentID);

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return RowsAffected > 0;
        }

        public static bool GetAppointmentByAppointmentID(int AppointmentID, ref int TestTypeID,
            ref int LDL_ApplicationID, ref DateTime AppointmentDate, ref decimal PaidFees,
            ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool Found = false;
            string Query = @"SELECT TestTypeID, LocalDrivingLicenseApplicationID,
                                AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID
                                FROM TestAppointments
                                WHERE TestAppointmentID = @ID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", Convert.ToString(AppointmentID));

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    TestTypeID = (int)reader["TestTypeID"];
                    LDL_ApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = Convert.ToBoolean(reader["IsActive"]);
                    RetakeTestApplicationID = reader["RetakeTestApplicationID"] == DBNull.Value ? -1 : (int)reader["RetakeTestApplicationID"]; 
                }

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
            return Found;
        }

        public static int AddNewAppointment(int TestTypeID, int LDL_AppID,
            DateTime Date, decimal PaidFees, int CreatedByUserID,
            bool IsLocked, int RetakeTestApplicationID = -1)
        {
            String Query = @"INSERT INTO TestAppointments
                                (TestTypeID, LocalDrivingLicenseApplicationID,
                                AppointmentDate, PaidFees, CreatedByUserID,
                                IsLocked, RetakeTestApplicationID)
                                VALUES
                                (@TestTypeID, @LDL_AppID, @Date,
                                @PaidFees, @UserID, @IsLocked, @RetakeTestApplicationID);
                                SELECT SCOPE_IDENTITY();";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);
            command.Parameters.AddWithValue("@Date", Date);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@UserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked ? "1" : "0");
            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID == -1 ? (object)DBNull.Value : RetakeTestApplicationID);

            object result = null;

            try
            {
                connection.Open();
                result = command.ExecuteScalar();
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                connection.Close();
            }

            return result != null ? Convert.ToInt32(result) : -1;
        }

        public static bool DeleteAppointment(int AppointmentID)
        {
            int RowsAffected = 0;
            string Query = @"DELETE FROM TestAppointments 
                            Where AppointmentID = @AppointmentID";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@AppointmentID", Convert.ToString(AppointmentID));

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                connection.Close();
                return false;
            }

            connection.Close();
            return RowsAffected > 0;
        }

        public static DataTable GetAppointmentsForApplicationIDAndTestTypeID(int LDL_ApplicationID, int TestTypeID)
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT 
                                [Appointment ID] = TestAppointmentID,
                                [Appointment Date] = AppointmentDate,
                                [Paid Fees] = PaidFees,
                                [Is Locked] = IsLocked
                                FROM TestAppointments
                                WHERE LocalDrivingLicenseApplicationID = @ApplicationID
                                AND TestTypeID = @TypeID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ApplicationID", Convert.ToString(LDL_ApplicationID));
            command.Parameters.AddWithValue("@TypeID", Convert.ToString(TestTypeID));
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
        public static bool DoesActiveAppointmentExist(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Found = false;
            String Query = @"SELECT 'Found'
                                FROM TestAppointments
                                WHERE
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                AND TestTypeID = @TestTypeID
                                AND IsLocked = 0";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Found = reader.HasRows;
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
            return Found;
        }

    }
}
