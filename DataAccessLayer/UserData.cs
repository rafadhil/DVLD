using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

namespace DataAccessLayer
{
    public static class UserData
    {

        public static DataTable GetUsersByUserIDLike(String UserID)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT UserID, People.PersonID, \r\n'Full Name' = \r\nFirstName + ' ' + SecondName + ' ' +\r\nCASE \r\nWHEN ThirdName IS NOT NULL THEN ThirdName \r\nELSE '' END + LastName, \r\nUserName,\r\nIsActive = \r\nCASE \r\nWhen IsActive = 1 THEN 'true'\r\nWhen IsActive = 0 THEN 'false'\r\nend\r\nFROM Users JOIN People on Users.PersonID = People.PersonID\r\nWHERE UserID Like '' + @ID +'%'";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", Convert.ToString(UserID));

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


        public static DataTable GetUsersByPersonIDLike(int PersonID)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT UserID, People.PersonID,  " +
                "'Full Name' = " +
                "FirstName + ' ' + SecondName + ' ' + " +
                "CASE \r\n" +
                "WHEN ThirdName IS NOT NULL THEN ThirdName \r\n" +
                "ELSE '' END + ' ' + LastName,\r\n" +
                "UserName, " +
                "IsActive = \r\n" +
                "CASE \r\n" +
                "When IsActive = 1 THEN 'true'\r\n" +
                "When IsActive = 0 THEN 'false'\r\n" +
                "end\r\n" +
                "FROM Users JOIN People on Users.PersonID = People.PersonID\r\n" +
                "WHERE People.PersonID Like '' + @ID +'%'";

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
                EventLog.WriteEntry(DataLayerSettings.EventViewerSourceName, e.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();

            }
            return DT;

        }

        public static DataTable GetAllUsers()
        {
            string query = "SELECT UserID, People.PersonID, \r\n'Full Name' = \r\nFirstName + ' ' + SecondName + ' ' +\r\nCASE \r\nWHEN ThirdName IS NOT NULL THEN ThirdName \r\nELSE '' END + ' ' + LastName,\r\nUserName,\r\nIsActive = \r\nCASE \r\nWhen IsActive = 1 THEN 'true'\r\nWhen IsActive = 0 THEN 'false'\r\nend\r\nFROM Users JOIN People on Users.PersonID = People.PersonID";

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

        public static DataTable GetActiveUsers()
        {
            string query = "SELECT UserID, People.PersonID, \r\n" +
                "'Full Name' = \r\n" +
                "FirstName + ' ' + SecondName + ' ' +\r\n" +
                "CASE \r\n" +
                "WHEN ThirdName IS NOT NULL THEN ThirdName \r\n" +
                "ELSE '' END + ' ' + LastName,\r\n" +
                "UserName,\r\n" +
                "IsActive = \r\n" +
                "CASE \r\n" +
                "When IsActive = 1 THEN 'true'\r\n" +
                "When IsActive = 0 THEN 'false'\r\n" +
                "end\r\n" +
                "FROM Users JOIN People on Users.PersonID = People.PersonID\r\n" +
                "WHERE IsActive = 1;";

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
        public static DataTable GetInactiveUsers()
        {
            string query = "SELECT UserID, People.PersonID, \r\n'Full Name' = \r\nFirstName + ' ' + SecondName + ' ' +\r\nCASE \r\nWHEN ThirdName IS NOT NULL THEN ThirdName \r\nELSE '' END + ' ' + LastName,\r\nUserName,\r\nIsActive = \r\nCASE \r\nWhen IsActive = 1 THEN 'true'\r\nWhen IsActive = 0 THEN 'false'\r\nend\r\nFROM Users JOIN People on Users.PersonID = People.PersonID\r\nWHERE IsActive = 0;";

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

        public static bool GetUserByUserID(int UserID, ref int PersonID, ref String UserName, ref bool IsActive)
        {
            string query = @"SELECT PersonID, UserName, IsActive =
                            CASE  When IsActive = 1 THEN 'true' When IsActive = 0 THEN 'false' end
                            FROM Users 
                            WHERE UserID = @ID ;";
            bool Found = false;

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", UserID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    UserName = (String)reader["UserName"];
                    IsActive = Convert.ToBoolean(reader["IsActive"]);
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

        public static bool GetUserByUsername(String UserName, ref int UserID, ref int PersonID, ref bool IsActive)
        {
            string query = @"SELECT UserID, PersonID, UserName, IsActive =
                            CASE  When IsActive = 1 THEN 'true' When IsActive = 0 THEN 'false' end
                            FROM Users 
                            WHERE UserName = @UserName ;";
            bool Found = false;

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    UserID = (int)reader["UserID"];
                    UserName = (String)reader["UserName"];
                    IsActive = Convert.ToBoolean(reader["IsActive"]);
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

        public static bool GetUserByPersonID(int PersonID, ref int UserID, ref String UserName, ref bool IsActive)
        {
            string query = "SELECT UserID, People.PersonID, \r\n'Full Name' = \r\nFirstName + ' ' + SecondName + ' ' +\r\nCASE \r\nWHEN ThirdName IS NOT NULL THEN ThirdName \r\nELSE '' END + ' ' + LastName,\r\nUserName,\r\nIsActive = \r\nCASE \r\nWhen IsActive = 1 THEN 'true'\r\nWhen IsActive = 0 THEN 'false'\r\nend\r\nFROM Users JOIN People on Users.PersonID = People.PersonID\r\nWHERE Users.PersonID = @ID ;\r\n";
            bool Found = false;

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", UserID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    Found = true;
                    UserID = (int)reader["UserID"];
                    UserName = (String)reader["UserName"];
                    IsActive = Convert.ToBoolean(reader["IsActive"]);
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


        public static DataTable GetUsersByFullnameLike(String Fullname)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT * FROM\r\n" +
                "(SELECT UserID, People.PersonID, \r\n" +
                "'Full Name' = \r\n" +
                "FirstName + ' ' + SecondName + ' ' +\r\nCASE \r\n" +
                "WHEN ThirdName IS NOT NULL THEN ThirdName \r\n" +
                "ELSE '' END + ' ' + LastName,\r\n" +
                "UserName,\r\n" +
                "IsActive = \r\n" +
                "CASE \r\n" +
                "When IsActive = 1 THEN 'true'\r\n" +
                "When IsActive = 0 THEN 'false'\r\n" +
                "end\r\n" +
                "FROM Users JOIN People on Users.PersonID = People.PersonID)\r\n" +
                "R1\r\n" +
                "Where R1.[Full Name] Like '' + @Name +'%'";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Name", Fullname);

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


        public static DataTable GetUsersByUsernameLike(String Username)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT UserID, People.PersonID, \r\n'Full Name' = \r\nFirstName + ' ' + SecondName + ' ' +\r\nCASE \r\nWHEN ThirdName IS NOT NULL THEN ThirdName \r\n ELSE '' END + ' ' + LastName,\r\nUserName,\r\nIsActive = \r\nCASE \r\nWhen IsActive = 1 THEN 'true'\r\nWhen IsActive = 0 THEN 'false'\r\nend\r\nFROM Users JOIN People on Users.PersonID = People.PersonID\r\nWHERE UserName Like '' + @Username +'%'";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Username", Username);

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

        public static int AddNewUser(int PersonID, String UserName, String Password, bool IsActive)
        {

            String Query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive)
                                Values (@PersonID, @UserName, @Password, @IsActive); SELECT SCOPE_IDENTITY() AS NewID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive ? "1" : "0");

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

        public static bool UpdateUser(int UserID, String UserName, String Password, bool IsActive)
        {
            int RowsAffected = -1;
            string Query = @"UPDATE Users
                                SET
                                    UserName = @UserName,
                                    Password = @Password,
                                    IsActive = @IsActive
                                WHERE
                                    UserID = @UserID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive ? "1" : "0");
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

        public static bool DeleteUser(int UserID)
        {
            int RowsAffected = 0;
            string Query = @"DELETE FROM Users 
                            Where UserID = @UserID";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@UserID", Convert.ToString(UserID));

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

        public static bool ValidateLoginCredentials(String Username, String Password)
        {
            bool IsAccessGranted = false;
            String Query = @"SELECT [Access Granted] = 1
                            FROM Users
                            Where UserName = @Username COLLATE Latin1_General_CS_AS
                            AND Password = @Password COLLATE Latin1_General_CS_AS;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsAccessGranted = reader.HasRows;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(DataLayerSettings.EventViewerSourceName, e.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return IsAccessGranted;
        }


        public static bool ChangePassword(int UserID, String NewPassword)
        {
            int RowsAffected = 0;
            string Query = @"UPDATE Users
                            SET Password = @NewPassword
                            WHERE UserID = @UserID;";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@NewPassword", NewPassword);
            command.Parameters.AddWithValue("@UserID", Convert.ToString(UserID));

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

    }
}
