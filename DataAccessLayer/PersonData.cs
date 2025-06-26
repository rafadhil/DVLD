using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Security.Policy;

namespace DataAccessLayer
{
    public class PersonData
    {

        public static DataTable GetAllPersons()
        {
            string query = "SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, " +
                           "Gendor =                    " +
                           "case                        " +
                           "when Gendor = 0 then 'M'    " +
                           "when Gendor = 1 then 'F'    " +
                           "end                         " +
                           ",                           " +
                           "Address, Phone, Email,      " +
                           "Countries.CountryName, ImagePath " +
                           "from People Join Countries On People.NationalityCountryID = Countries.CountryID; ";

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

        public static DataTable GetPersonsByPersonIDLike(int PersonID)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT * FROM People Where PersonID Like '' + @ID +'%'";
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

        public static DataTable GetPersonsByFirstNameLike(String FirstName)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT * FROM People Where FirstName Like '' + @FirstName +'%'";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@FirstName", FirstName);

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

        public static DataTable GetPersonsBySecondNameLike(String SecondName)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT * FROM People Where SecondName Like '' + @SecondName +'%'";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@SecondName", SecondName);

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

        public static DataTable GetPersonsByThirdNameLike(String ThirdName)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT * FROM People Where ThirdName Like '' + @ThirdName +'%'";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);

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

        public static DataTable GetPersonsByLastNameLike(String LastName)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT * FROM People Where LastName Like '' + @LastName +'%'";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LastName", LastName);

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

        public static DataTable GetPersonsByPhoneike(String PhoneNumber)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT * FROM People Where Phone Like '' + @PhoneNumber +'%'";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);

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


        public static DataTable GetPersonsByEmailLike(String Email)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT * FROM People Where Email Like '' + @Email +'%'";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Email", Email);

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


        public static DataTable GetPersonsByGender(bool Gender)
        {
            int Gen = Gender ? 1 : 0;
            DataTable DT = new DataTable();
            string Query = "SELECT * FROM People Where Gendor = @Gender";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Gender", Convert.ToString(Gen));

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

        public static DataTable GetPersonsByNationalNumberLike(String NationalNumber)
        {

            DataTable DT = new DataTable();
            string Query = "SELECT * FROM People Where NationalNo Like '' + @NatNO +'%'";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@NatNO", Convert.ToString(NationalNumber));

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

        public static bool GetPersonByID(int PersonID, ref String NationalNo, ref String FirstName,
            ref String SecondName, ref String ThirdName, ref String LastName, ref DateTime DateOfBirth,
            ref bool Gender, ref String Address, ref String PhoneNumber, ref String Email,
            ref int NationalityCountryID,
            ref String ImagePath)
        {
            bool Found = false;
            string Query = "SELECT * FROM People Where PersonID = @ID";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", Convert.ToString(PersonID));

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    NationalNo = reader.GetString(1);
                    FirstName = reader.GetString(2);
                    SecondName = reader.GetString(3);
                    ThirdName = Convert.IsDBNull(reader["ThirdName"]) ? null : reader["ThirdName"].ToString();
                    LastName = reader.GetString(5);
                    DateOfBirth = reader.GetDateTime(6);
                    Gender = Convert.ToBoolean(reader["Gendor"]) ;
                    Address = reader.GetString(8);
                    PhoneNumber = reader.GetString(9);
                    Email = Convert.IsDBNull(reader["Email"]) ? null : reader["Email"].ToString();
                    NationalityCountryID = reader.GetInt32(11);
                    ImagePath = Convert.IsDBNull(reader["ImagePath"]) ? null : reader["ImagePath"].ToString(); 
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
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return Found;
        }

        public static bool GetPersonByNationalNumber(String NationalNo , ref int PersonID, ref String FirstName,
            ref String SecondName, ref String ThirdName, ref String LastName, ref DateTime DateOfBirth,
            ref bool Gender, ref String Address, ref String PhoneNumber, ref String Email,
            ref int NationalityCountryID,
            ref String ImagePath)
        {
            bool Found = false;
            string Query = "SELECT * FROM People Where NationalNo = @Number";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Number", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    FirstName = reader.GetString(2);
                    SecondName = reader.GetString(3);
                    ThirdName = Convert.IsDBNull(reader["ThirdName"]) ? null : reader["ThirdName"].ToString();
                    LastName = reader.GetString(5);
                    DateOfBirth = reader.GetDateTime(6);
                    Gender = Convert.ToBoolean(reader["Gendor"]);
                    Address = reader.GetString(8);
                    PhoneNumber = reader.GetString(9);
                    Email = Convert.IsDBNull(reader["Email"]) ? null : reader["Email"].ToString();
                    NationalityCountryID = reader.GetInt32(11);
                    ImagePath = Convert.IsDBNull(reader["ImagePath"]) ? null : reader["ImagePath"].ToString();
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
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return Found;
        }

        public static bool IsPersonExist(String NationalNo)
        {
            bool Found = false;
            string Query = "SELECT Found=1 FROM People Where NationalNo = @NationalNo ;";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

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

        public static bool UpdatePerson(int PersonID, String NationalNo, String FirstName,
            String SecondName, String ThirdName, String LastName, DateTime DateOfBirth,
            bool Gender, String Address, String PhoneNumber, String Email,
            int NationalityCountryID, String ImagePath)
        {
            int RowsAffected = -1;
            string Query = 
                "UPDATE  People " +
                "SET  " +
                "NationalNo = @NationalNo,\r\n    " +
                "FirstName = @FirstName,\r\n    " +
                "SecondName = @SecondName,\r\n    " +
                "ThirdName = @ThirdName,\r\n    " +
                "LastName = @LastName,\r\n    " +
                "DateOfBirth = @DateOfBirth,\r\n    " +
                "Gendor = @Gender,\r\n    " +
                "Address = @Address,\r\n    " +
                "Phone = @PhoneNumber,\r\n    " +
                "Email = @Email,\r\n    " +
                "NationalityCountryID = @NationalityCountryID,\r\n    " +
                "ImagePath = @ImagePath\r\n" +
                "WHERE\r\n    PersonID = @PersonID;";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender? "1": "0");
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            command.Parameters.AddWithValue("@Email", Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@ImagePath", ImagePath ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            
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

        public static int AddNewPerson(String NationalNo, String FirstName,
           String SecondName, String ThirdName, String LastName, DateTime DateOfBirth,
            bool Gender, String Address, String PhoneNumber, String Email,
            int NationalityCountryID,
            String ImagePath)
        {
            String Query = @"
            INSERT INTO People (
                NationalNo,
                FirstName,
                SecondName,
                ThirdName,
                LastName,
                DateOfBirth,
                Gendor,
                Address,
                Phone,
                Email,
                NationalityCountryID,
                ImagePath
            )
            VALUES (
                @NationalNo,
                @FirstName,
                @SecondName,
                @ThirdName,
                @LastName,
                @DateOfBirth,
                @Gender,
                @Address,
                @PhoneNumber,
                @Email,
                @NationalityCountryID,
                @ImagePath
            );
            SELECT SCOPE_IDENTITY();";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender ? "1" : "0");
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            command.Parameters.AddWithValue("@Email", Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@ImagePath", ImagePath ?? (object)DBNull.Value);
            object result = null;

            try
            {
                connection.Open();
                result = command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return result != null ? Convert.ToInt32(result) : -1;

        }

        public static bool DeletePerson(int PersonID)
        {
            int RowsAffected = 0;
            string Query = @"DELETE FROM People 
                            Where PersonID = @ID";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", Convert.ToString(PersonID));

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


        public static DataTable GetPersonsByCountryID(int CountryID)
        {
    
            string Query = @"SELECT * FROM People
                            Where NationalityCountryID = @ID";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@ID", Convert.ToString(CountryID));
            DataTable Records = new DataTable();



            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Records.Load(reader);

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
            return Records;
        }

        public static bool HasUserAccount(int PersonID)
        {

            bool Found = false;
            string Query = "SELECT Found=1 FROM Users Where PersonID = @PersonID ;";
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
    }
}
