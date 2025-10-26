using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Licenses
{
    public static class DetainedLicenseData
    {
        public static bool IsLicenseDetained(int LicenseID)
        {
            bool Found = false;
            string Query = @"SELECT TOP 1 'Found'
                                From DetainedLicenses
                                WHERE LicenseID = @LicenseID
                                AND IsReleased = 0
                                ORDER BY DetainID DESC;";
            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

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

        public static bool ReleaseDetainedLicense(int LicenseID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int RowsAffected = -1;
            string Query = @"UPDATE DetainedLicenses
                            SET IsReleased = 1, 
                            ReleaseDate = GETDATE(), 
                            ReleasedByUserID = @ReleasedByUserID,
                            ReleaseApplicationID = @ReleaseApplicationID 
                            WHERE DetainID IN (
					                            SELECT TOP 1 DetainID
						                            From DetainedLicenses
						                            WHERE LicenseID = @LicenseID
						                            AND IsReleased = 0
						                            ORDER BY DetainID DESC) ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
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

        public static bool GetDetentionInfoForDetainedLicense(int LicenseID, ref int DetainID, ref DateTime DetainDate, 
            ref decimal FineFees, ref int CreatedByUserID
            )
        {
            string query = @"SELECT TOP 1 *
                                From DetainedLicenses
                                WHERE LicenseID = @LicenseID
                                AND IsReleased = 0
                                ORDER BY DetainID DESC;";
            bool Found = false;

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    DetainID = Convert.ToInt32(reader["DetainID"]);
                    DetainDate = (DateTime)reader["DetainDate"];
                    FineFees = Convert.ToDecimal(reader["FineFees"]);
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

        public static DataTable GetAllRecords()
        {
            string query = @"SELECT 
	                        DetainID as [D.ID],
	                        DL.LicenseID as [L.ID],
	                        DetainDate as [D.Date],
	                        IsReleased as [Is Released],
	                        FineFees as [Fine Fees],
	                        ReleaseDate as [Release Date],
	                        NationalNo as [Nat.No.],
	                        [Full Name] = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName,
	                        ReleaseApplicationID as [Release App.ID]
                        FROM DetainedLicenses DL
                        JOIN Licenses L ON DL.LicenseID = L.LicenseID
                        JOIN Drivers D on L.DriverID = D.DriverID
                        JOIN People P ON D.PersonID = P.PersonID";

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

        public static DataTable GetAllRecordsByDetainIDLike(String DetainID)
        {
            string query = @"SELECT 
	                            DetainID as [D.ID],
	                            DL.LicenseID as [L.ID],
	                            DetainDate as [D.Date],
	                            IsReleased as [Is Released],
	                            FineFees as [Fine Fees],
	                            ReleaseDate as [Release Date],
	                            NationalNo as [Nat.No.],
	                            [Full Name] = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName,
	                            ReleaseApplicationID as [Release App.ID]
                            FROM DetainedLicenses DL
                            JOIN Licenses L ON DL.LicenseID = L.LicenseID
                            JOIN Drivers D on L.DriverID = D.DriverID
                            JOIN People P ON D.PersonID = P.PersonID
                        WHERE DetainID LIKE @DetainID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();
            command.Parameters.AddWithValue("@DetainID", DetainID + "%");


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
        public static DataTable GetAllRecordsByNationalNumberLike(String NationalNo)
        {
            string query = @"SELECT 
	                            DetainID as [D.ID],
	                            DL.LicenseID as [L.ID],
	                            DetainDate as [D.Date],
	                            IsReleased as [Is Released],
	                            FineFees as [Fine Fees],
	                            ReleaseDate as [Release Date],
	                            NationalNo as [Nat.No.],
	                            [Full Name] = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName,
	                            ReleaseApplicationID as [Release App.ID]
                            FROM DetainedLicenses DL
                            JOIN Licenses L ON DL.LicenseID = L.LicenseID
                            JOIN Drivers D on L.DriverID = D.DriverID
                            JOIN People P ON D.PersonID = P.PersonID
                        WHERE NationalNo LIKE @NationalNo ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();
            command.Parameters.AddWithValue("@NationalNo", NationalNo + "%");


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


        public static DataTable GetAllRecordsByFullNameLike(String FullName)
        {
            string query = @"SELECT * FROM
                                (SELECT 
	                                    DetainID as [D.ID],
	                                    DL.LicenseID as [L.ID],
	                                    DetainDate as [D.Date],
	                                    IsReleased as [Is Released],
	                                    FineFees as [Fine Fees],
	                                    ReleaseDate as [Release Date],
	                                    NationalNo as [Nat.No.],
	                                    [Full Name] = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName,
	                                    ReleaseApplicationID as [Release App.ID]
                                    FROM DetainedLicenses DL
                                    JOIN Licenses L ON DL.LicenseID = L.LicenseID
                                    JOIN Drivers D on L.DriverID = D.DriverID
                                    JOIN People P ON D.PersonID = P.PersonID) R1
                                WHERE [Full Name] LIKE @FullName ;";

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

        public static DataTable GetAllReleasedRecords()
        {
            string query = @"SELECT 
	                            DetainID as [D.ID],
	                            DL.LicenseID as [L.ID],
	                            DetainDate as [D.Date],
	                            IsReleased as [Is Released],
	                            FineFees as [Fine Fees],
	                            ReleaseDate as [Release Date],
	                            NationalNo as [Nat.No.],
	                            [Full Name] = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName,
	                            ReleaseApplicationID as [Release App.ID]
                            FROM DetainedLicenses DL
                            JOIN Licenses L ON DL.LicenseID = L.LicenseID
                            JOIN Drivers D on L.DriverID = D.DriverID
                            JOIN People P ON D.PersonID = P.PersonID
                        WHERE IsReleased = 1 ;";

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

        public static DataTable GetAllUnreleasedRecords()
        {
            string query = @"SELECT 
	                            DetainID as [D.ID],
	                            DL.LicenseID as [L.ID],
	                            DetainDate as [D.Date],
	                            IsReleased as [Is Released],
	                            FineFees as [Fine Fees],
	                            ReleaseDate as [Release Date],
	                            NationalNo as [Nat.No.],
	                            [Full Name] = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName,
	                            ReleaseApplicationID as [Release App.ID]
                            FROM DetainedLicenses DL
                            JOIN Licenses L ON DL.LicenseID = L.LicenseID
                            JOIN Drivers D on L.DriverID = D.DriverID
                            JOIN People P ON D.PersonID = P.PersonID
                        WHERE IsReleased = 0 ;";

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


        public static DataTable GetAllReleasedRecordsByReleaseApplicationIDLike(String ReleaseApplicationID)
        {
            string query = @"SELECT 
	                            DetainID as [D.ID],
	                            DL.LicenseID as [L.ID],
	                            DetainDate as [D.Date],
	                            IsReleased as [Is Released],
	                            FineFees as [Fine Fees],
	                            ReleaseDate as [Release Date],
	                            NationalNo as [Nat.No.],
	                            [Full Name] = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName,
	                            ReleaseApplicationID as [Release App.ID]
                            FROM DetainedLicenses DL
                            JOIN Licenses L ON DL.LicenseID = L.LicenseID
                            JOIN Drivers D on L.DriverID = D.DriverID
                            JOIN People P ON D.PersonID = P.PersonID
                        WHERE ReleaseApplicationID LIKE @ReleaseApplicationID ;";

            SqlConnection connection = new SqlConnection(DataLayerSettings.connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            DataTable DT = new DataTable();
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID + "%");


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
