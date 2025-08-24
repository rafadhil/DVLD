using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Test
    {
        public int TestID { get; private set; }
        public TestAppointment TestAppointment { set;  get; }
        public bool TestResult { set; get; }
        public String Notes { set; get; }
        public int CreatedByUserID { set; get; }

        public Test(TestAppointment testAppointment, bool testResult, string notes, int createdByUserID)
        {
            TestID = -1;
            TestAppointment = testAppointment;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUserID;
        }

        public static int GetNumberOfPassedTests(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestData.GetNumberOfPassedTests(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static int GetNumberOfFailedTests(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestData.GetNumberOfFailedTests(LocalDrivingLicenseApplicationID, TestTypeID);

        }
        public bool Save()
        {
            if(TestAppointment == null)
            {
                return false;
            }

            TestID =  TestData.AddNewTest(TestAppointment.AppointmentID, TestResult, Notes, CreatedByUserID);

            TestAppointment.IsLocked = true;
            TestAppointment.Save();

            TestData.SetRetakeTestApplicationToCompleted(TestAppointment.AppointmentID);
            return TestID != -1;
        }
    }
}
