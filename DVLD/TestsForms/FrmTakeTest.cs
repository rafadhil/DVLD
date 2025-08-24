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

namespace DVLD
{
    public partial class FrmTakeTest : Form
    {
        TestAppointment Test_Appointment;
        enTestType testType;
        public FrmTakeTest(TestAppointment test_Appointment)
        {
            InitializeComponent();
            if(test_Appointment == null)
            {
                MessageBox.Show("ERROR: Could not find test appointment");
                this.Close();
                return;
            }

            Test_Appointment = test_Appointment;
            testType = (enTestType)test_Appointment.TestType.TypeID;
        }

        private void FrmTakeTest_Load(object sender, EventArgs e)
        {
            SetFormData();

        }

        private void SetFormData()
        {
            lblDL_ApplicationID.Text = Test_Appointment.LDL_Application.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = Test_Appointment.LDL_Application.LicenseClass.ClassName;
            lblPersonName.Text = Test_Appointment.LDL_Application.OriginalApplicationInfo.ApplicantInfo.GetFullName();
            lblTrialNumber.Text = (Test.GetNumberOfFailedTests(
                Test_Appointment.LDL_Application.LocalDrivingLicenseApplicationID,
                (int)testType) + 1).ToString();
            lblDate.Text = Test_Appointment.AppointmentDate.ToString("dd/MM/yyyy");
            lblFees.Text = Test_Appointment.PaidFees.ToString("0");

            if (testType == enTestType.Vision)
            {
                pbTestPicture.Image = Properties.Resources.visionTest;
                return;
            }

            if (testType == enTestType.Written)
            {
                pbTestPicture.Image = Properties.Resources.writtenText;
                return;
            }

            if (testType == enTestType.Street)
            {
                pbTestPicture.Image = Properties.Resources.streetTest;
                return;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!rbPass.Checked && !rbFail.Checked)
            {
                MessageBox.Show("Choose a result first", "Failed");
                return;
            }

            Test Test = new Test(
                Test_Appointment, 
                rbPass.Checked,
               String.IsNullOrEmpty(txtNotes.Text)? null: txtNotes.Text,
               UserSettings.LoggedInUser.UserID
                );

            if(Test.Save())
            {
                MessageBox.Show("Test Result has been successfully saved", "Success");
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show("ERROR: Could not save result");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
