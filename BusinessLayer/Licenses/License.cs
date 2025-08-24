using DataAccessLayer;
using DataAccessLayer.Licenses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class License
    {

        public int LicenseID { get; private set; }
        public Application Application { get;private set; }
        public Driver DriverInfo { get;private set; }
        public LicenseClass LicenseClass { get;private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public String Notes {private set; get; }
        public decimal PaidFees {private set; get; }
        public bool IsActive {private set; get; }
        public byte IssueReason { get; private set; }
        public User CreatedByUser {private set; get; }

        private License(int licenseID, Application application, Driver driver,
        LicenseClass licenseClass, DateTime issueDate, DateTime expirationDate,
        string notes, decimal paidFees, bool isActive, byte issueReason, User createdByUser)
            {
                LicenseID = licenseID;
                Application = application;
                DriverInfo = driver;
                LicenseClass = licenseClass;
                IssueDate = issueDate;
                ExpirationDate = expirationDate;
                Notes = notes;
                PaidFees = paidFees;
                IsActive = isActive;
                IssueReason = issueReason;
                CreatedByUser = createdByUser;
            }

        public bool IsDetained()
        {
            return DetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FineFees"></param>
        /// <returns> DetentionID </returns>
        public int Detain(decimal FineFees)
        {
            return Detain(this, FineFees);
        }
        public static int Detain(License LicenseToDetain, decimal FineFees)
        {
            return LicenseData.DetainLicense(LicenseToDetain.LicenseID, FineFees, UserSettings.LoggedInUser.UserID);
        }

        public bool IsExpired()
        {
            return ExpirationDate.Date <= DateTime.Today;
        }

        public static bool HasActiveLicenseOfClassType(int PersonID, int DriverLicenseClassID)
        {
            return LicenseData.HasActiveLicenseOfClassType(PersonID, DriverLicenseClassID);
        }

        public static bool HasLicenseOfClassType(int PersonID, int DriverLicenseClassID)
        {
            return LicenseData.HasLicenseOfClassType(PersonID, DriverLicenseClassID);
        }

        public static License GetLicenseByLicenseID(int LicenseID)
        {
            int applicationID = -1, driverID = -1, licenseClassID = -1, createdByUserID = -1;
            string notes = "";
            decimal paidFees = 0.0M;
            bool isActive = false;
            byte issueReason = 0;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;

            if (LicenseData.GetLicenseByLicenseID(
                LicenseID, ref applicationID, ref driverID, ref licenseClassID, ref issueDate, ref expirationDate,
                ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID
                ))
            {
                return new License(
                    LicenseID,
                    Application.GetApplicationByID(applicationID),
                    Driver.GetDriverByDriverID(driverID),
                    LicenseClass.GetClassByID(licenseClassID),
                    issueDate,
                    expirationDate,
                    notes,
                    paidFees,
                    isActive,
                    issueReason, User.GetUserByUserID(createdByUserID)
                    );
            }

            return null;
        }

        public static License GetLicenseByApplicationID(int applicationID )
        {
            int LicenseID = -1, driverID = -1, licenseClassID = -1, createdByUserID = -1;
            string notes = "";
            decimal paidFees = 0.0M;
            bool isActive = false;
            byte issueReason = 0;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;

            if (LicenseData.GetLicenseByApplicationID(
               applicationID , ref LicenseID, ref driverID, ref licenseClassID, ref issueDate, ref expirationDate,
                ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID
                ))
            {
                return new License(
                    LicenseID,
                    Application.GetApplicationByID(applicationID),
                    Driver.GetDriverByDriverID(driverID),
                    LicenseClass.GetClassByID(licenseClassID),
                    issueDate,
                    expirationDate,
                    notes,
                    paidFees,
                    isActive,
                    issueReason, User.GetUserByUserID(createdByUserID)
                    );
            }

            return null;
        }

        public static DataTable GetAllLicensesForPerson(int PersonID)
        {
            return LicenseData.GetAllLicensesForPerson(PersonID);
        }

        public static Result IssueNewLicense(int applicationID, int licenseClassID, string notes,
        decimal paidFees, byte issueReason, int createdByUserID)
        {
            Application ApplicationInfo = Application.GetApplicationByID(applicationID);

            if (issueReason == 1 &&
                HasLicenseOfClassType(ApplicationInfo.ApplicantInfo.PersonID, licenseClassID))
            {
                return Result.Failure("ERROR: Driver already has a driving license of this license class");
            }

            if (String.IsNullOrWhiteSpace(notes))
            {
                notes = null;
            }

            Driver.AddNewDriver(ApplicationInfo.ApplicantInfo.PersonID);    // Registers the person as a driver in the system (if not already registered)
            Driver DriverInfo = Driver.GetDriverByPersonID(ApplicationInfo.ApplicantInfo.PersonID);

            bool IsLicenseActive = true;
            int NewLicenseID = LicenseData.IssueNewLicense(
                applicationID,
                DriverInfo.DriverID,
                licenseClassID,
                notes,
                paidFees,
                IsLicenseActive,
                issueReason,
                createdByUserID);

            if(NewLicenseID == -1)
            {
                return Result.Failure("An error has occured while trying to issue the license, contact the system administrator");
            }

            ApplicationInfo.MarkAsCompleted();
            ApplicationInfo.Save();
            return Result.Success($"License has been issued successfully, License ID: {NewLicenseID}");

        }


        public static Result RenewLicense(Application RenewLicenseApplication, License OldLicense, String Notes, decimal PaidFees)
        {
            if (!OldLicense.IsExpired())
            {
                RenewLicenseApplication.CancelApplication();
                RenewLicenseApplication.Save();
                return Result.Failure($"ERROR: Old license is not expired. Expiration date is: {OldLicense.ExpirationDate.ToString("dd/MMM/yyyy")}. Application has been cancelled.");
            }

            if (!OldLicense.IsActive)
            {
                RenewLicenseApplication.CancelApplication();
                RenewLicenseApplication.Save();
                return Result.Failure("ERROR: Old license is not active, application has been cancelled.");
            }

            OldLicense.DeactivateLicense();
            Result NewLicenseResult = IssueNewLicense(
                RenewLicenseApplication.ApplicationID, 
                OldLicense.LicenseClass.LicenseClassID,
                Notes,
                PaidFees,
                2,
                UserSettings.LoggedInUser.UserID);


            return NewLicenseResult;
        }


        public static Result ReplaceDamagedLicense(Application RenewLicenseApplication, License OldLicense, decimal PaidFees)
        {

            if (!OldLicense.IsActive)
            {
                RenewLicenseApplication.CancelApplication();
                RenewLicenseApplication.Save();
                return Result.Failure("ERROR: Old license is not active, application has been cancelled.");
            }

            OldLicense.DeactivateLicense();
            Result NewLicenseResult = IssueNewLicense(
                RenewLicenseApplication.ApplicationID,
                OldLicense.LicenseClass.LicenseClassID,
                null,
                PaidFees,
                3,
                UserSettings.LoggedInUser.UserID);


            return NewLicenseResult;
        }


        public static Result ReplaceLostLicense(Application RenewLicenseApplication, License OldLicense, decimal PaidFees)
        {

            if (!OldLicense.IsActive)
            {
                RenewLicenseApplication.CancelApplication();
                RenewLicenseApplication.Save();
                return Result.Failure("ERROR: Old license is not active, application has been cancelled.");
            }

            OldLicense.DeactivateLicense();
            Result NewLicenseResult = IssueNewLicense(
                RenewLicenseApplication.ApplicationID,
                OldLicense.LicenseClass.LicenseClassID,
                null,
                PaidFees,
                4,
                UserSettings.LoggedInUser.UserID);


            return NewLicenseResult;
        }

        public bool ActivateLicense()
        {
            return ActivateLicense(this);
        }

        public bool DeactivateLicense()
        {
            return DeactivateLicense(this);
        }

        public static bool ActivateLicense(License DriverLicense)
        {
            return LicenseData.ActivateLicense(DriverLicense.LicenseID);
        }

        public static bool DeactivateLicense(License DriverLicense)
        {
            return LicenseData.DeactivateLicense(DriverLicense.LicenseID);
        }
    }
}
