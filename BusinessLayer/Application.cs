using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Application
    {
        enum enMode { AddNew, Update, Delete}
        enMode Mode;
        public int ApplicationID { private set; get; }
        public Person ApplicantInfo { set; get; }
        public DateTime ApplicationDate { set; get; }
        public ApplicationType ApplicationType { set; get; }
        public byte ApplicationStatus { private set; get; }
        public DateTime LastStatusDate { private set; get; }
        public decimal PaidFees { set; get; }
        public User CreatedByUser { set; get; }


        public Application()
        {
            Mode = enMode.AddNew;
            ApplicationID = -1;
            ApplicantInfo = new Person();
            ApplicationDate = DateTime.MinValue;
            ApplicationType = ApplicationType.GetApplicationTypeByID(1);
            ApplicationStatus = 1;
            LastStatusDate = DateTime.MinValue;
            PaidFees = -1;
            CreatedByUser = new User();
        }

        private Application(int applicationID, Person applicantInfo, DateTime applicationDate,
            ApplicationType applicationType, byte applicationStatus, DateTime lastStatusDate, 
            decimal paidFees, User createdByUser)
        {
            Mode = enMode.Update;
            ApplicationID = applicationID;
            ApplicantInfo = applicantInfo;
            ApplicationDate = applicationDate;
            ApplicationType = applicationType;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            CreatedByUser = createdByUser;
        }

        public static Application GetApplicationByID(int ApplicationID)
        {
            int ApplicantID = -1, CreatedByUserID = -1, ApplicationTypeID = -1;
            DateTime ApplicationDate = DateTime.MinValue;
            byte ApplicationStatus = 0;
            DateTime LastStatusDate = DateTime.MinValue;
            decimal PaidFees = 0.0M;
            
                    
           if (ApplicationData.GetApplicationByID(ApplicationID, ref ApplicantID, ref ApplicationDate,
                ref ApplicationTypeID, ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
                {
                return new Application(ApplicationID, Person.GetPersonByID(ApplicantID),
                    ApplicationDate, ApplicationType.GetApplicationTypeByID(ApplicationTypeID),
                    ApplicationStatus, LastStatusDate, PaidFees, User.GetUserByUserID(CreatedByUserID));
                }

           else
                return null;
        }

        public String GetStatus()
        {
            switch (ApplicationStatus)
            {
                case 1:
                    return "New";

                case 2:
                    return "Cancelled";

                case 3:
                    return "Completed";

                default:
                    return "?";
            }

        }
        public bool DeleteApplication()
        {
            if (DeleteApplication(ApplicationID))
            {
                Mode = enMode.Delete;
                return true;
            }
            else
                return false;
        }

        public bool CancelApplication()
        {
            return ApplicationData.UpdateApplicationStatus(this.ApplicationID, 2);
        }
        

        public static bool DeleteApplication(int ApplicationID)
        {
            return ApplicationData.DeleteApplication(ApplicationID);
        }

        //public Application GetApplicationByID(int ApplicationID)
        //{ 

        //}

        //public static DataTable GetApplications()
        //{

        //}


        public bool Save()
        {
            if(Mode == enMode.AddNew)
            {
                 ApplicationID = ApplicationData.AddNewApplication(ApplicantInfo.PersonID, ApplicationDate,
                    ApplicationType.TypeID, ApplicationStatus, ApplicationDate, PaidFees,
                    UserSettings.LoggedInUser.UserID);
                if (ApplicationID != -1)
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                    return false;
            }
            else if(Mode == enMode.Update)
            {
                return ApplicationData.UpdateApplicationStatus(ApplicationID, ApplicationStatus);
            }
            else
            {
                return false;
            }
        }




    }
}
