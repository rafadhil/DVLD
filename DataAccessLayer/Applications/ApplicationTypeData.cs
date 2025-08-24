using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class ApplicationTypeData
    {
        public static DataTable GetApplicationTypes()
        {
            DataTable DT = new DataTable();
            string Query = @"SELECT ApplicationTypeID as ID, 
                             ApplicationTypeTitle as Title,
                             ApplicationFees as Fees
                             FROM ApplicationTypes  ";
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
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();

            }
            return DT;
        }

        public static bool UpdateApplicationType(int TypeID, String TypeTitle, decimal TypeFees)
        {
            int RowsAffected = -1;
            string Query = @"UPDATE ApplicationTypes
                             SET 
                             ApplicationTypeTitle = @Title,
                             ApplicationFees = @Fees
                             WHERE ApplicationTypeID = @TypeID";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@Title", TypeTitle);
            command.Parameters.AddWithValue("@Fees", TypeFees);
            command.Parameters.AddWithValue("@TypeID", TypeID);
   
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

        public static bool GetApplicationTypeByID(int TypeID, ref String TypeTitle, ref decimal TypeFees)
        {
            bool Found = false;
            string Query = @"SELECT ApplicationTypeID as ID, 
                             ApplicationTypeTitle as Title,
                             ApplicationFees as Fees
                             FROM ApplicationTypes
                             WHERE ApplicationTypeID = @ID;";
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
                    TypeFees = (decimal)reader["Fees"];
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

    }
}
