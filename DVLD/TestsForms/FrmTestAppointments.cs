using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using DVLD.Common;


namespace DVLD
{
    public partial class FrmTestAppointments : Form
    {

        private enTestType TestType;
        private LocalDrivingLicenseApplication ActiveApplication;


        public FrmTestAppointments(LocalDrivingLicenseApplication LDL_Application, enTestType testType)
        {
            InitializeComponent();
            if(LDL_Application == null)
            {
                MessageBox.Show("ERROR: Could not find Local Driving License Application");
                this.Close();
                return;
            }

            ActiveApplication = LDL_Application;
            TestType = testType;
        }

        private void FrmTestAppointments_Load(object sender, EventArgs e)
        {
            ctrShowLocalDrivingLicenseApplicationInfo1.LoadInfo(ActiveApplication);
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

        private void SetFormToVisionTest()
        {
            lblTitle.Text = "Vision Test Appointments";
            this.Text = "Vision Test Appointments";
            pbTestPicture.Image = Properties.Resources.visionTest;
            LoadDGVWithVisionTestAppointments();
        }

        private void SetFormToWrittenTest()
        {
            lblTitle.Text = "Written Test Appointments";
            this.Text = "Written Test Appointments";
            pbTestPicture.Image = Properties.Resources.writtenText;
            LoadDGVWithWrittenTestAppointments();
        }

        private void SetFormToStreetTest()
        {
            lblTitle.Text = "Street Test Appointments";
            this.Text = "Street Test Appointments";
            pbTestPicture.Image = Properties.Resources.streetTest;
            LoadDGVWithStreetTestAppointments();
        }

        public void RefreshDGV()
        {
            switch (TestType)
            {
                case enTestType.Vision:
                    LoadDGVWithVisionTestAppointments();
                    return;

                case enTestType.Written:
                    LoadDGVWithWrittenTestAppointments();
                    return;

                case enTestType.Street:
                    LoadDGVWithStreetTestAppointments();
                    return;
            }

        }

        private void LoadDGVWithVisionTestAppointments()
        {
           dgvAppointments.DataSource = TestAppointment.GetAppointmentsForApplicationIDAndTestTypeID(
               ActiveApplication.LocalDrivingLicenseApplicationID, 1);
        }

        private void LoadDGVWithWrittenTestAppointments()
        {
            dgvAppointments.DataSource = TestAppointment.GetAppointmentsForApplicationIDAndTestTypeID(
               ActiveApplication.LocalDrivingLicenseApplicationID, 2);
        }

        private void LoadDGVWithStreetTestAppointments()
        {
            dgvAppointments.DataSource = TestAppointment.GetAppointmentsForApplicationIDAndTestTypeID(
                ActiveApplication.LocalDrivingLicenseApplicationID, 3);
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            if (TestAppointment.DoesActiveAppointmentExist(ActiveApplication.LocalDrivingLicenseApplicationID, (int) TestType))
            {
                MessageBox.Show("Cannot book a new appointment, an active appointment already exists",
                    "Failed", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if(Test.GetNumberOfPassedTests(ActiveApplication.LocalDrivingLicenseApplicationID, (int) TestType) > 0)
            {
                MessageBox.Show("Person has already passed this test, cannot book a new appointment", "Failed", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Form Frm = new FrmScheduleTests(ActiveApplication, TestType);
            Frm.ShowDialog();
            RefreshDGV();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmScheduleTests(GetTestAppointmentOfSelectedDGVrow());
            Frm.ShowDialog();
            RefreshDGV();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count < 1 || Convert.ToBoolean(dgvAppointments.CurrentRow.Cells["Is Locked"].Value))
            {
                e.Cancel = true;
            }
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAppointment Appointment = GetTestAppointmentOfSelectedDGVrow();
            if(Appointment == null)
            {
                MessageBox.Show("ERROR: Could not find test appointment");
                return;
            }
            
            Form Frm = new FrmTakeTest(GetTestAppointmentOfSelectedDGVrow());
            Frm.ShowDialog();
            RefreshDGV();
        }

        private TestAppointment GetTestAppointmentOfSelectedDGVrow()
        {
            DataGridViewRow SelectedRow = dgvAppointments.SelectedRows[0];
            int TestAptID = Convert.ToInt32(SelectedRow.Cells["Appointment ID"].Value);
            return TestAppointment.GetAppointmentByID(TestAptID);
        }

    }

}