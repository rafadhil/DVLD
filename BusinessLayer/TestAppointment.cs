using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLayer
{
    public class TestAppointment
    {
        enum enMode { AddNew, Update, Delete }

        private enMode Mode;
        public int AppointmentID { private set; get; }
        public TestType TestType { set; get; }
        public LocalDrivingLicenseApplication LDL_Application { set; get; }
        public DateTime AppointmentDate { set; get; }
        public decimal PaidFees { set; get; }
        public User CreatedByUser { set; get; }
        public bool IsLocked { set; get; }
        public int RetakeTestApplicationID { set; get; }

        public TestAppointment()
        {
            Mode = enMode.AddNew;
            AppointmentID = -1;
            TestType = TestType.GetTestTypeByID(1);
            LDL_Application = new LocalDrivingLicenseApplication();
            AppointmentDate = DateTime.MinValue;
            PaidFees = 0.0M;
            CreatedByUser = new User();
            IsLocked = false;
            RetakeTestApplicationID = -1;
        }

        private TestAppointment(int appointmentID, TestType testType, LocalDrivingLicenseApplication lDL_Application, 
            DateTime appointmentDate, decimal paidFees, User createdByUser, bool isLocked, int retakeTestApplicationID = -1)
        {
            Mode = enMode.Update;
            AppointmentID = appointmentID;
            TestType = testType;
            LDL_Application = lDL_Application;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUser = createdByUser;
            IsLocked = isLocked;
            RetakeTestApplicationID = retakeTestApplicationID;
        }

        public static DataTable GetAppointmentsForApplicationIDAndTestTypeID(int LDL_ApplicationID, int TestTypeID)
        {
            return TestAppointmentData.GetAppointmentsForApplicationIDAndTestTypeID(LDL_ApplicationID, TestTypeID);
        }

        public static bool DoesActiveAppointmentExist(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestAppointmentData.DoesActiveAppointmentExist(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static TestAppointment GetAppointmentByID(int AppointmentID)
        {
            int TestTypeID = -1, LDL_ApplicationID = -1, CreatedByUserID = -1, RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.MinValue;
            decimal PaidFees = 0.0M;
            bool IsLocked = true;

            if (TestAppointmentData.GetAppointmentByAppointmentID(AppointmentID, ref TestTypeID,
                ref LDL_ApplicationID, ref AppointmentDate, ref PaidFees,
                 ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new TestAppointment(AppointmentID, TestType.GetTestTypeByID(TestTypeID),
                    LocalDrivingLicenseApplication.GetApplicationByID(LDL_ApplicationID),
                    AppointmentDate, PaidFees, User.GetUserByUserID(CreatedByUserID), IsLocked, RetakeTestApplicationID);
            }
            else
                return null ;
        }

        public Result Save()
        {

            if (TestData.GetNumberOfPassedTests(LDL_Application.LocalDrivingLicenseApplicationID,
                TestType.TypeID) > 0)
            {
                return Result.Failure("Person has already passed this test, cannot book a new appointment");
            }

            if (Mode == enMode.AddNew)
            {
                AppointmentID = TestAppointmentData.AddNewAppointment(TestType.TypeID,
                    LDL_Application.LocalDrivingLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUser.UserID, false, RetakeTestApplicationID);
                if (AppointmentID != -1)
                {
                    Mode = enMode.Update;
                    return Result.Success();
                }
                else
                    return Result.Failure("ERROR: Failed to book new appointment");
            }

            else if (Mode == enMode.Update)
            {
                return TestAppointmentData.UpdateAppointment(AppointmentID,
                    AppointmentDate, IsLocked) ? Result.Success() : Result.Failure("ERROR: Failed to update appointment");
            }
            else
                return Result.Failure("ERROR: Failed to save appointment details");
        }
    }
}
