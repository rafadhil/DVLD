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
    public class DetainedLicense
    {
        public DetainedLicense(int detainID, License licenseInfo, 
            DateTime detainTime, decimal fineFees, User createdBy, 
            bool isReleased, DateTime releaseDate, User releasedBy,
            Application releaseApplication)
        {
            DetainID = detainID;
            LicenseInfo = licenseInfo;
            DetainTime = detainTime;
            FineFees = fineFees;
            CreatedBy = createdBy;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedBy = releasedBy;
            ReleaseApplication = releaseApplication;
        }

        public int DetainID { get; private set; }
        public License LicenseInfo { get; private set; }
        public DateTime DetainTime { get; private set; }
        public decimal FineFees { get; private set; }
        public User CreatedBy { get; private set; }
        public bool IsReleased { get; private set; }
        public DateTime ReleaseDate { private get; set; }
        public User ReleasedBy { get; private set; }
        public Application ReleaseApplication { get; private set; }


        public static Result ReleaseLicense(License DetainedLicense, Application ReleaseApplication)
        {
            if(!DetainedLicense.IsDetained())
            {
                return Result.Failure("License is not detained");
            }
            
            if(DetainedLicense == null)
            {
                return Result.Failure("param detained license cannot be empty");
            }

            if (ReleaseApplication == null)
            {
                return Result.Failure("param release application cannot be empty");
            }

            if(DetainedLicenseData.ReleaseDetainedLicense(
                DetainedLicense.LicenseID, 
                UserSettings.LoggedInUser.UserID, 
                ReleaseApplication.ApplicationID))
            {
                return Result.Success();
            }

            return Result.Failure("An error has occured while trying to release the license, contact the system administrator");
        }

        public static DataTable GetAllRecords()
        {
            return DetainedLicenseData.GetAllRecords();
        }
        public static DataTable GetAllRecordsByDetainIDLike(String DetainID)
        {
            return DetainedLicenseData.GetAllRecordsByDetainIDLike(DetainID);
        }
        public static DataTable GetAllRecordsByFullNameLike(String FullName)
        {
            return DetainedLicenseData.GetAllRecordsByFullNameLike(FullName);
        }
        public static DataTable GetAllRecordsByNationalNumberLike(String NationalNumber)
        {
            return DetainedLicenseData.GetAllRecordsByNationalNumberLike(NationalNumber);
        }
        public static DataTable GetAllUnreleasedRecords()
        {
            return DetainedLicenseData.GetAllUnreleasedRecords();
        }
        public static DataTable GetAllReleasedRecords()
        {
            return DetainedLicenseData.GetAllReleasedRecords();
        }

        public static DataTable GetAllReleasedRecordsByReleaseApplicationIDLike(String ReleaseApplicationID)
        {
            return DetainedLicenseData.GetAllReleasedRecordsByReleaseApplicationIDLike(ReleaseApplicationID);
        }

        public static DetainedLicense GetDetentionInfoForDetainedLicense(License DetainedLicense)
        {
            int DetainID = -1, CreatedByUserID = -1;
            DateTime DetainTime = DateTime.MinValue;
            decimal FineFees = 0M;



            if (DetainedLicenseData.GetDetentionInfoForDetainedLicense(DetainedLicense.LicenseID, ref DetainID,
                ref DetainTime, ref FineFees, ref CreatedByUserID))
            {
                return new DetainedLicense(
                    DetainID, 
                    DetainedLicense,
                    DetainTime, 
                    FineFees, 
                    User.GetUserByUserID(CreatedByUserID),
                    false, 
                    DateTime.MinValue, 
                    null,
                    null
                    );
            }
            else
                return null;
        }
    }

}
