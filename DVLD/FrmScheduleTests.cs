using BusinessLayer;
using DVLD.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices.ActiveDirectory;

namespace DVLD
{
    public partial class FrmScheduleTests : Form
    {
        LocalDrivingLicenseApplication ActiveApplication;
        enTestType TestType;
        int TrialNumber;
        public FrmScheduleTests(LocalDrivingLicenseApplication LDL_Application, enTestType testType)
        {
            InitializeComponent();

            if(LDL_Application == null)
            {
                MessageBox.Show("ERROR: Could not find Local Driving License Application");
                this.Close();
                return;
            }

            
            TrialNumber = Test.GetNumberOfFailedTests(LDL_Application.LocalDrivingLicenseApplicationID, Convert.ToInt32(testType)) + 1;

            ActiveApplication = LDL_Application;
            TestType = testType;
        }

        private void FrmScheduleTests_Load(object sender, EventArgs e)
        {
            dtpDate.MinDate = DateTime.Now;
            FillFormData();


            if(Test.GetNumberOfPassedTests(ActiveApplication.LocalDrivingLicenseApplicationID,
                Convert.ToInt32(TestType)) > 0)
            {
                MessageBox.Show("Person has already passed this test, cannot book a new test appointment", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }

            if(TrialNumber > 1)
            {
                ActivateRetakeTestApplication();
            }

            if (TestType == enTestType.Vision)
            {
                SetFormToVisionTest();
                return;
            }


            if (TestType == enTestType.Written)
            {
                SetFormToWrittenTest();
                return;
            }


            if (TestType == enTestType.Street)
            {
                SetFormToStreetTest();
                return;
            }
        }

        private void ActivateRetakeTestApplication()
        {
            gbRetakeTest.Enabled = true;
            lblTitle.Text = "Schedule Retake Test";
            lblRetakeTestFees.Text = ApplicationType.GetApplicationTypeByID(7).TypeFees.ToString("0");
        }

        private void FillFormData()
        {
            gbTest.Text = TestType.ToString() + " Test";
            lblTrialNumber.Text = TrialNumber.ToString();
            lblDL_ApplicationID.Text = ActiveApplication.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = ActiveApplication.LicenseClass.ClassName;
            lblPersonName.Text = ActiveApplication.OriginalApplicationInfo.ApplicantInfo.GetFullName();

            lblFees.Text = BusinessLayer.TestType.GetTestTypeByID((int)TestType).TypeFees.ToString("0");
        }

        private void SetFormToVisionTest()
        {
            pbTestPicture.Image = Properties.Resources.visionTest;
        }

        private void SetFormToWrittenTest()
        {
            pbTestPicture.Image = Properties.Resources.writtenText;
        }

        private void SetFormToStreetTest()
        {
            pbTestPicture.Image = Properties.Resources.streetTest;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int RetakeTestApplicationID = -1;
            if (TrialNumber > 1)
            {
                RetakeTestApplicationID = AddNewRetakeTestApplication();

                if (RetakeTestApplicationID != -1)
                {
                    lblRetakeTestAppID.Text = RetakeTestApplicationID.ToString();
                }
                else
                {
                    MessageBox.Show("ERROR: Could not add a new application of type (Retake Test)");
                    return;
                }

            }

            int NewTestAppointmentID = AddNewTestAppointment(RetakeTestApplicationID);
            if(NewTestAppointmentID == -1)
            {
                return;
            }

            MessageBox.Show("Test has been scheduled successfully", "Success");



        }

        private int AddNewRetakeTestApplication()
        {
           BusinessLayer.Application RetakeTestApplication = new BusinessLayer.Application();
            RetakeTestApplication.ApplicantInfo = ActiveApplication.OriginalApplicationInfo.ApplicantInfo;
            RetakeTestApplication.ApplicationDate = DateTime.Now;
            RetakeTestApplication.ApplicationType = ApplicationType.GetApplicationTypeByID(7);
            RetakeTestApplication.PaidFees = Convert.ToDecimal(lblRetakeTestFees.Text);
            RetakeTestApplication.CreatedByUser = UserSettings.LoggedInUser;

            if(RetakeTestApplication.Save())
            {
                return RetakeTestApplication.ApplicationID;
            }
            else
            {
                return -1;
            }
        }

        private int AddNewTestAppointment(int RetakeTestApplicationID)
        {
            TestAppointment NewAppointment = new TestAppointment();
            NewAppointment.AppointmentDate = DateTime.Now;
            NewAppointment.CreatedByUser = UserSettings.LoggedInUser;
            NewAppointment.TestType = BusinessLayer.TestType.GetTestTypeByID((int) TestType);
            NewAppointment.LDL_Application = ActiveApplication;
            NewAppointment.PaidFees = Convert.ToDecimal(lblFees.Text);
            NewAppointment.IsLocked = false;
            NewAppointment.RetakeTestApplicationID = RetakeTestApplicationID;

            Result SaveResult = NewAppointment.Save();

            if(SaveResult.IsSuccess)
            {
                return NewAppointment.AppointmentID;
            }
            else
            {
                MessageBox.Show(SaveResult.ErrorMessage);
                return -1;
            }
        }
    }
}
