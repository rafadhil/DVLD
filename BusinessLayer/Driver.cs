using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DataAccessLayer;

namespace BusinessLayer
{
    public class Driver
    {
        public int DriverID { get; private set; }
        public Person PersonalInfo { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreationDate { get; private set; }

        private Driver(int DriverID, Person personalInfo, User createdBy, DateTime CreationDate)
        {
            this.DriverID = DriverID;
            PersonalInfo = personalInfo;
            CreatedBy = createdBy;
            this.CreationDate = CreationDate;
        }

        public static DataTable GetAllDrivers()
        {
            return DriverData.GetAllDrivers();
        }

        public static DataTable GetAllDriversByDriverIDLike(String DriverID)
        {
            return DriverData.GetAllDriversByDriverIDLike(DriverID);
        }
        public static DataTable GetAllDriversByPersonIDLike(String PersonID)
        {
            return DriverData.GetAllDriversByPersonIDLike(PersonID);

        }
        public static DataTable GetAllDriversByNationalNumberLike(String NationalNumber)
        {
            return DriverData.GetAllDriversByNationalNumberLike(NationalNumber);

        }
        public static DataTable GetAllDriversByFullNameLike(String FullName)
        {
            return DriverData.GetAllDriversByFullNameLike(FullName);

        }



        public static Driver GetDriverByDriverID(int DriverID)
        {
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime CreationDate = DateTime.MinValue;

            if (DriverData.GetDriverByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreationDate))
            {
                return new Driver(DriverID, Person.GetPersonByID(PersonID), User.GetUserByUserID(CreatedByUserID), CreationDate);
            }
            else
                return null;
        }
        public static Driver GetDriverByPersonID(int PersonID)
        {
            int DriverID = -1, CreatedByUserID = -1;
            DateTime CreationDate = DateTime.MinValue;

            if (DriverData.GetDriverByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreationDate))
            {
                return new Driver(DriverID, Person.GetPersonByID(PersonID), User.GetUserByUserID(CreatedByUserID), CreationDate);
            }
            else
                return null;
        }



        public static bool AddNewDriver(int PersonID)
        {
            int DriverAdditionFailedCode = -1;

            if (DriverData.DoesDriverExistByPersonID(PersonID))
            {
                return false;
            }
            else
                return DriverData.AddNewDriver(PersonID, UserSettings.LoggedInUser.UserID) != DriverAdditionFailedCode;
        }
    }
}
