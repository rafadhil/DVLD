using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class DriverData
    {
        public static DataTable GetAllDrivers()
        {
            string query = @"SELECT 
	                            DriverID  [Driver ID],
	                            d.PersonID [Person ID],
	                            p.NationalNo,
	                            [Full Name] =
	                            p.FirstName + ' ' + p.SecondName + ' ' + p.ThirdName + ' ' + p.LastName  
	                            ,
	                            CreatedDate [Creation Date],
	                            [Active Licenses] = (
						                            SELECT COUNT(LicenseID)
						                            FROM Licenses l
						                            WHERE IsActive = 1 AND l.DriverID = d.DriverID)
                            FROM Drivers d
                            JOIN People p on d.PersonID = p.PersonID";

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

            }
            finally
            {
                connection.Close();
            }
            return DT;
        }

        public static DataTable GetAllDriversByDriverIDLike(String DriverID)
        {
            string query = @"SELECT 
	                            DriverID  [Driver ID],
	                            d.PersonID [Person ID],
	                            p.NationalNo,
	                            [Full Name] =
	                            p.FirstName + ' ' + p.SecondName + ' ' + p.ThirdName + ' ' + p.LastName  
	                            ,
	                            CreatedDate [Creation Date],
	                            [Active Licenses] = (
						                            SELECT COUNT(LicenseID)
						                            FROM Licenses l
						                            WHERE IsActive = 1 AND l.DriverID = d.DriverID)
                            FROM Drivers d
                            JOIN People p on d.PersonID = p.PersonID
	                        WHERE DriverID Like @DriverID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID + "%");

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

            }
            finally
            {
                connection.Close();
            }
            return DT;
        }


        public static DataTable GetAllDriversByPersonIDLike(String PersonID)
        {
            string query = @"SELECT 
	                            DriverID  [Driver ID],
	                            d.PersonID [Person ID],
	                            p.NationalNo,
	                            [Full Name] =
	                            p.FirstName + ' ' + p.SecondName + ' ' + p.ThirdName + ' ' + p.LastName  
	                            ,
	                            CreatedDate [Creation Date],
	                            [Active Licenses] = (
						                            SELECT COUNT(LicenseID)
						                            FROM Licenses l
						                            WHERE IsActive = 1 AND l.DriverID = d.DriverID)
                            FROM Drivers d
                            JOIN People p on d.PersonID = p.PersonID
	                        WHERE d.PersonID Like @PersonID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID + "%");

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

            }
            finally
            {
                connection.Close();
            }
            return DT;
        }


        public static DataTable GetAllDriversByNationalNumberLike(String NationalNumber)
        {
            string query = @"SELECT 
	                            DriverID  [Driver ID],
	                            d.PersonID [Person ID],
	                            p.NationalNo,
	                            [Full Name] =
	                            p.FirstName + ' ' + p.SecondName + ' ' + p.ThirdName + ' ' + p.LastName  
	                            ,
	                            CreatedDate [Creation Date],
	                            [Active Licenses] = (
						                            SELECT COUNT(LicenseID)
						                            FROM Licenses l
						                            WHERE IsActive = 1 AND l.DriverID = d.DriverID)
                            FROM Drivers d
                            JOIN People p on d.PersonID = p.PersonID
	                        WHERE p.NationalNo Like @NationalNumber ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNumber", NationalNumber + "%");

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

            }
            finally
            {
                connection.Close();
            }
            return DT;
        }


        public static DataTable GetAllDriversByFullNameLike(String FullName)
        {
            string query = @"SELECT * FROM
                                (SELECT 
	                                    DriverID  [Driver ID],
	                                    d.PersonID [Person ID],
	                                    p.NationalNo,
	                                    [Full Name] =
	                                    p.FirstName + ' ' + p.SecondName + ' ' + p.ThirdName + ' ' + p.LastName  
	                                    ,
	                                    CreatedDate [Creation Date],
	                                    [Active Licenses] = (
						                                    SELECT COUNT(LicenseID)
						                                    FROM Licenses l
						                                    WHERE IsActive = 1 AND l.DriverID = d.DriverID)
                                    FROM Drivers d
                                    JOIN People p on d.PersonID = p.PersonID ) R1
	                                WHERE R1.[Full Name] Like @FullName ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FullName", FullName + "%");
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

            }
            finally
            {
                connection.Close();
            }
            return DT;
        }


        public static bool GetDriverByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreationDate)
        {
            string query = @"SELECT PersonID, CreatedByUserID, CreatedDate
                                FROM Drivers
                                WHERE DriverID = @DriverID ;";
            bool Found = false;

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreationDate = Convert.ToDateTime(reader["CreatedDate"]);
                    Found = true;

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

        public static bool GetDriverByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreationDate)
        {
            string query = @"SELECT DriverID, CreatedByUserID, CreatedDate
                                FROM Drivers
                                WHERE PersonID = @PersonID ;";
            bool Found = false;

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    DriverID = Convert.ToInt32(reader["DriverID"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreationDate = Convert.ToDateTime(reader["CreatedDate"]);
                    Found = true;

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

        public static bool DoesDriverExistByDriverID(int DriverID)
        {
            bool Found = false;
            string Query = @"SELECT Found=1
                                FROM Drivers D
                                WHERE D.DriverID = @DriverID;";

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
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return Found;
        }

        public static bool DoesDriverExistByPersonID(int PersonID)
        {
            bool Found = false;
            string Query = @"SELECT Found=1
                                FROM Drivers D  
                                WHERE D.PersonID = @PersonID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {

            String Query = @"INSERT INTO Drivers(PersonID, CreatedByUserID, CreatedDate)
                            Values (@PersonID, @CreatedByUserID, GETDATE()); 

                            SELECT SCOPE_IDENTITY() AS NewID;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


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

    }
}
