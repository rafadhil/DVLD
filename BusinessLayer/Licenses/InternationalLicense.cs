using DataAccessLayer;
using DataAccessLayer.Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Licenses
{
    public class InternationalLicense
    {
 

        public int LicenseID { private set; get; }
        public Application OriginalApplication { get; set; }
        public Driver DriverInfo { get; set; }
        public License IssuedUsingLocalLicense { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public User CreatedByUser { get; set; }

        private InternationalLicense(int licenseID, Application originalApplication,
        Driver driverInfo, License issuedUsingLocalLicense, DateTime issueDate,
        DateTime expirationDate, bool isActive, User createdByUser)
        {
            LicenseID = licenseID;
            OriginalApplication = originalApplication;
            DriverInfo = driverInfo;
            IssuedUsingLocalLicense = issuedUsingLocalLicense;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            CreatedByUser = createdByUser;
        }

        public static DataTable GetAllLicensesForDriver(Driver DriverInfo)
        {
            return InternationalLicenseData.GetAllLicensesForDriver(DriverInfo.DriverID);
        }

        public static DataTable GetAllLicenses()
        {
            return InternationalLicenseData.GetAllLicenses();
        }

        public static DataTable GetAllLicensesByLicenseIDLike(string LicenseID)
        {
            return InternationalLicenseData.GetAllLicensesByLicenseIDLike(LicenseID);
        }

        public static DataTable GetAllLicensesByLocalLicenseIDLike(string LocalLicenseID)
        {
            return InternationalLicenseData.GetAllLicensesByLocalLicenseIDLike(LocalLicenseID);

        }

        public static DataTable GetAllLicensesByDriverIDLike(string DriverID)
        {
            return InternationalLicenseData.GetAllLicensesByDriverIDLike(DriverID);
        }

        public static InternationalLicense GetLicenseByLicenseID(int licenseID)
        {
            int applicationID = -1, driverID = -1, issuedUsingLocalLicenseID= -1, createdByUserID = -1;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            bool isActive = false;

            if(InternationalLicenseData.GetLicenseByLicenseID(licenseID, ref applicationID, ref driverID, 
                ref issuedUsingLocalLicenseID, ref issueDate, ref expirationDate, ref isActive, ref createdByUserID))
            {
                return new InternationalLicense(
                    licenseID,
                    Application.GetApplicationByID(applicationID),
                    Driver.GetDriverByDriverID(driverID), 
                    License.GetLicenseByLicenseID(licenseID),
                    issueDate, 
                    expirationDate, 
                    isActive,
                    User.GetUserByUserID(createdByUserID)
                    );
            }
            else
            {
                return null;
            }
        }

        public static InternationalLicense GetLicenseByLocalLicenseID(int localLicenseID)
        {
            int applicationID = -1, driverID = -1, licenseID = -1, createdByUserID = -1;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            bool isActive = false;

            if (InternationalLicenseData.GetLicenseByLocalLicenseID(localLicenseID, ref licenseID, ref applicationID, ref driverID,
                ref issueDate, ref expirationDate, ref isActive, ref createdByUserID))
            {
                return new InternationalLicense(
                    licenseID,
                    Application.GetApplicationByID(applicationID),
                    Driver.GetDriverByDriverID(driverID),
                    License.GetLicenseByLicenseID(licenseID),
                    issueDate,
                    expirationDate,
                    isActive,
                    User.GetUserByUserID(createdByUserID)
                    );
            }
            else
            {
                return null;
            }
        }

        public static InternationalLicense GetLicenseByApplicationID(int applicationID)
        {
            int licenseID = -1, driverID = -1, issuedUsingLocalLicenseID = -1, createdByUserID = -1;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            bool isActive = false;

            if (InternationalLicenseData.GetLicenseByApplicationID(applicationID, ref licenseID, ref driverID,
                ref issuedUsingLocalLicenseID, ref issueDate, ref expirationDate, ref isActive, ref createdByUserID))
            {
                return new InternationalLicense(
                    licenseID,
                    Application.GetApplicationByID(applicationID),
                    Driver.GetDriverByDriverID(driverID),
                    License.GetLicenseByLicenseID(licenseID),
                    issueDate,
                    expirationDate,
                    isActive,
                    User.GetUserByUserID(createdByUserID)
                    );
            }
            else
            {
                return null;
            }
        }

        public static bool HasActiveInternationalLicense(Driver Driver)
        {
            if(Driver == null)
            {
                return false;
            }

            return InternationalLicenseData.HasActiveInternationalLicense(Driver.DriverID);
        }

        public static bool HasInternationalLicense(Driver Driver)
        {
            if (Driver == null)
            {
                return false;
            }

            return InternationalLicenseData.HasInternationalLicense(Driver.DriverID);
        }

        public bool IsExpired()
        {
            return ExpirationDate.Date <= DateTime.Today;
        }

        public static Result IssueNewLicense(Application application, Driver driver, License issuedUsingLocalLicense)
        {

            if (HasActiveInternationalLicense(driver))
            {
                return Result.Failure("ERROR: Driver already has an active international driving license");
            }

            int NewLicenseID = InternationalLicenseData.IssueNewLicense(
                application.ApplicationID,
                driver.DriverID,
                issuedUsingLocalLicense.LicenseID,
                UserSettings.LoggedInUser.UserID
                );

            if (NewLicenseID == -1)
            {
                return Result.Failure("An error has occured while trying to issue the license, contact the system administrator");
            }

            application.MarkAsCompleted();
            application.Save();
            return Result.Success($"License has been issued successfully, License ID: {NewLicenseID}");

        }


    }
}
