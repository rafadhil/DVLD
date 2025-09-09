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
        enum enFormMode {AddNew, Update}

        LocalDrivingLicenseApplication ActiveApplication;
        TestAppointment ActiveAppointment;
        enFormMode FormMode;
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
            ActiveAppointment = new TestAppointment();
            
            ActiveApplication = LDL_Application;
            TestType = testType;
            TrialNumber = Test.GetNumberOfFailedTests(LDL_Application.LocalDrivingLicenseApplicationID, Convert.ToInt32(testType)) + 1;
            FormMode = enFormMode.AddNew;
        }

        public FrmScheduleTests(TestAppointment appointment)
        {
            InitializeComponent();

            if (appointment == null)
            {
                MessageBox.Show("ERROR: Could not find appointment");
                this.Close();
                return;
            }

            if(appointment.IsLocked)
            {
                MessageBox.Show("ERROR: person has already taken this test appointment, cannot edit appointment info");
                this.Close();
                return;
            }

            ActiveAppointment = appointment;
            ActiveApplication = appointment.LDL_Application;
            TestType = (enTestType) appointment.TestType.TypeID;
            TrialNumber = Test.GetNumberOfFailedTests(
                ActiveApplication.LocalDrivingLicenseApplicationID,
                Convert.ToInt32(TestType)) + 1;
            FormMode = enFormMode.Update;
        }
        private void FrmScheduleTests_Load(object sender, EventArgs e)
        {
            dtpDate.MinDate = DateTime.Now;
            FillFormData();

            if (FormMode == enFormMode.AddNew)
            {
                lblTitle.Text = "Schedule Test";
            }
            else
            {
                lblTitle.Text = "Update Appointment";
            }


            if (Test.GetNumberOfPassedTests(ActiveApplication.LocalDrivingLicenseApplicationID,
                Convert.ToInt32(TestType)) > 0)
            {
                MessageBox.Show("ERROR: Person has already passed this test, cannot update or book a test appointment", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }

            if(TrialNumber > 1)
            {
                ActivateRetakeTestApplication();
            }

            SetImage();

        }

        private void SetImage()
        {
            switch (TestType)
            {
                case enTestType.Vision:
                    pbTestPicture.Image = Properties.Resources.visionTest;
                    return;

                case enTestType.Written:
                    pbTestPicture.Image = Properties.Resources.writtenText;
                    return;

                case enTestType.Street:
                    pbTestPicture.Image = Properties.Resources.streetTest;
                    return;

            }

        }

        private void ActivateRetakeTestApplication()
        {
            gbRetakeTest.Enabled = true;

            if(FormMode == enFormMode.AddNew)
                lblTitle.Text = "Schedule Test Retake";

            lblRetakeTestFees.Text = ApplicationType.GetApplicationTypeByID(7).TypeFees.ToString("0");
            lblTotalFees.Text = Convert.ToString(Convert.ToDecimal(lblRetakeTestFees.Text)
                + Convert.ToDecimal(lblFees.Text));
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


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // If FormMode is Update, only update the appointment date and save the info to the database
            if(FormMode == enFormMode.Update)
            {
                ActiveAppointment.AppointmentDate = dtpDate.Value;
                Result Output = ActiveAppointment.Save();
                if(Output.IsSuccess)
                {
                    MessageBox.Show("Update Successful", "Success");
                }
                else
                {
                    MessageBox.Show(Output.Message);
                }
                return;
            }

            // If FormMode is addNew:

            /* 1. check if this is the first trial, if it is , go to step number 3, if not, go to step number 2
             * 2. make a new application of a type of retake test and go to step number 3
             * 3. make a new test appointment
            */

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

   
            if(AddNewTestAppointment(RetakeTestApplicationID))
            {
                MessageBox.Show("Test appointment has been scheduled successfully", "Success");
                FormMode = enFormMode.Update;

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

        private bool AddNewTestAppointment(int RetakeTestApplicationID)
        {
            ActiveAppointment.AppointmentDate = DateTime.Now;
            ActiveAppointment.CreatedByUser = UserSettings.LoggedInUser;
            ActiveAppointment.TestType = BusinessLayer.TestType.GetTestTypeByID((int) TestType);
            ActiveAppointment.LDL_Application = ActiveApplication;
            ActiveAppointment.PaidFees = Convert.ToDecimal(lblFees.Text);
            ActiveAppointment.IsLocked = false;
            ActiveAppointment.RetakeTestApplicationID = RetakeTestApplicationID;

            Result SaveResult = ActiveAppointment.Save();

            return ActiveAppointment.Save().IsSuccess;

        }


    }
}
