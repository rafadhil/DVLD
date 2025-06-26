using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
namespace BusinessLayer
{
    public class LocalDrivingLicenseApplication
    {
        public enum enMode {AddNew, Update, Delete }
        public int LocalDrivingLicenseApplicationID { set; get; }
        public Application OriginalApplicationInfo { set; get; }
        public LicenseClass LicenseClass { set; get; }
        private enMode Mode;
        public LocalDrivingLicenseApplication()
        {
            Mode = enMode.AddNew;
            LocalDrivingLicenseApplicationID = -1;
            OriginalApplicationInfo = new Application();
            LicenseClass = LicenseClass.GetClassByID(1);
        }

        private LocalDrivingLicenseApplication(int localDrivingLicenseApplicationID, 
            Application originalApplicationInfo, LicenseClass licenseClass)
        {
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            OriginalApplicationInfo = originalApplicationInfo;
            LicenseClass = licenseClass;
            Mode = enMode.Update;
        }

        public static LocalDrivingLicenseApplication GetApplicationByID(int ApplicationID)
        {
            int OriginalApplicationID = -1, LicenseClassID = -1;

            if (LocalDrivingLicenseApplicationData.GetApplicationByID(ApplicationID, ref OriginalApplicationID, ref LicenseClassID))
            {
                return new LocalDrivingLicenseApplication(ApplicationID, Application.GetApplicationByID(OriginalApplicationID),
                    LicenseClass.GetClassByID(LicenseClassID));
            }
            else
                return null;
        }


        public Result CancelApplication()
        {
            if(Mode == enMode.Delete || Mode == enMode.AddNew )
            {
                return Result.Failure("ERROR: Cancellation failed, the application is either not present in the database or it has been deleted");
            }

            if (OriginalApplicationInfo.ApplicationStatus == 2)
            {
                return Result.Failure("ERROR: Application has already been cancelled");
            }

             if(!OriginalApplicationInfo.CancelApplication())
            {
                return Result.Failure("ERROR: Could not cancel application");
            }

            return Result.Success();
        }

        public Result DeleteApplication()
        {
            if (Mode == enMode.Delete || Mode == enMode.AddNew)
                return Result.Failure("ERROR: Application has either already been deleted or it never existed in the database");

            if (!LocalDrivingLicenseApplicationData.DeleteApplication(this.LocalDrivingLicenseApplicationID))
            {
                return Result.Failure("Could not delete application from database table: [LocalDrivingLicenseApplications], it might have data linked to it");
            }

            if (!OriginalApplicationInfo.DeleteApplication())
            {
                return Result.Failure("ERROR: Application has either already been deleted or it never existed in the database");
            }


            this.Mode = enMode.Delete;
            return Result.Success();
        }

        public int GetNumberOfPassedTests()
        {
            return GetNumberOfPassedTests(this.LocalDrivingLicenseApplicationID);
        }

        public static int GetNumberOfPassedTests(int LDL_ApplicationID)
        {
            return LocalDrivingLicenseApplicationData.GetNumberOfPassedTests(LDL_ApplicationID);
        }

        public static DataTable GetAll_LDL_Applications()
        {
            return LocalDrivingLicenseApplicationData.GetAll_LDL_Applications();
        }

        public static DataTable GetAll_LDL_ApplicationsByApplicationIDLike(int LDL_ApplicationID)
        {
            return LocalDrivingLicenseApplicationData.GetAll_LDL_ApplicationsByApplicationIDLike(LDL_ApplicationID);
        }

        public static DataTable GetAll_LDL_ApplicationsByPersonNationalNumberLike(String NationalNumber)
        {
            return LocalDrivingLicenseApplicationData.GetAll_LDL_ApplicationsByPersonNationalNumberLike(NationalNumber);
        }

        public static DataTable GetAllNew_LDL_Applications()
        {
            return LocalDrivingLicenseApplicationData.GetAllNew_LDL_Applications();
        }
        public static DataTable GetAllCompleted_LDL_Applications()
        {
            return LocalDrivingLicenseApplicationData.GetAllCompleted_LDL_Applications();
        }
        public static DataTable GetAllCancelled_LDL_Applications()
        {
            return LocalDrivingLicenseApplicationData.GetAllCancelled_LDL_Applications();
        }

        public static DataTable GetAll_LDL_ApplicationsByPersonFullNameLike(String FullName)
        {
            return LocalDrivingLicenseApplicationData.GetAll_LDL_ApplicationsByPersonFullNameLike(FullName);
        }

        public Result Save()
        {
            int PersonID = this.OriginalApplicationInfo.ApplicantInfo.PersonID;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (LocalDrivingLicenseApplicationData.DoesPersonHaveLicenseOfLicenseClass(
                        PersonID, LicenseClass.LicenseClassID))
                        return Result.Failure("ERROR: Cannot apply for a new local license for this license class, person already has a license of this class");

                    if(!LocalDrivingLicenseApplicationData.CanPersonApplyForLicenseWithClass(
                        PersonID, LicenseClass.LicenseClassID))
                        return Result.Failure("ERROR: Person is not old enough to apply for a license of this class");

                    int ActiveApplicationID = 
                        LocalDrivingLicenseApplicationData.GetActiveApplicationID_ForPersonAndLicenseClass(
                        PersonID, LicenseClass.LicenseClassID);
                    if(ActiveApplicationID != -1)
                        return Result.Failure($"ERROR: Person already has an active application with ID:{ActiveApplicationID}");

                    if(!OriginalApplicationInfo.Save())
                        return Result.Failure("ERROR: Could not save application info to Database table: [Applications]");

                    LocalDrivingLicenseApplicationID =
                        LocalDrivingLicenseApplicationData.AddNewApplication(OriginalApplicationInfo.ApplicationID,
                        LicenseClass.LicenseClassID);
                    if (LocalDrivingLicenseApplicationID == -1)
                        return Result.Failure("ERROR: Could not save application info to Database table: [LocalDrivingLicenseApplications]");

                    Mode = enMode.Update;
                    return Result.Success();
                    

                case enMode.Update:
                    if (!OriginalApplicationInfo.Save())
                        return Result.Failure("ERROR: Could not save application info to Database table: [Applications]");
                    return Result.Success();
                default:
                    return Result.Failure("ERROR: Cannot save application info since it has been deleted");

            }
            
            
        }



    }
}
