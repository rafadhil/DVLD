using BusinessLayer;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using DVLD.Common;
namespace DVLD

{
    public partial class ctrShowLocalDLApplications : UserControl
    {
        public ctrShowLocalDLApplications()
        {
            InitializeComponent();
        }

        private void ctrShowLocalDLApplications_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            cbApplicationStatus.SelectedItem = "All";
            RefreshDGV();
        }

        public void RefreshDGV()
        {
            cbFilterBy.SelectedItem = "None";
            txtSearch.Visible = false;
            cbApplicationStatus.Visible = false;
            dgvApplications.DataSource = LocalDrivingLicenseApplication.GetAll_LDL_Applications();
            SetNumberOfRecordsLabel();
        }
        private void SetNumberOfRecordsLabel()
        {
            lblNumberOfRecords.Text = Convert.ToString(dgvApplications.AllowUserToAddRows ? dgvApplications.Rows.Count - 1 : dgvApplications.Rows.Count);
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtSearch.Text))
            {
                dgvApplications.DataSource = LocalDrivingLicenseApplication.GetAll_LDL_Applications();
                SetNumberOfRecordsLabel();
                return;
            }

            String SelectedItem = cbFilterBy.SelectedItem.ToString();
            if(SelectedItem == "L.D.L AppID")
            {
                dgvApplications.DataSource = LocalDrivingLicenseApplication.
                    GetAll_LDL_ApplicationsByApplicationIDLike(Convert.ToInt32(txtSearch.Text));
                SetNumberOfRecordsLabel();
                return;
            }

            if(SelectedItem == "National Number")
            {
                dgvApplications.DataSource = LocalDrivingLicenseApplication.
                    GetAll_LDL_ApplicationsByPersonNationalNumberLike(txtSearch.Text);
                SetNumberOfRecordsLabel();
                return;
            }

            if (SelectedItem == "Full Name")
            {
                dgvApplications.DataSource = LocalDrivingLicenseApplication.
                    GetAll_LDL_ApplicationsByPersonFullNameLike(txtSearch.Text);
                SetNumberOfRecordsLabel();
                return;
            }



        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            dgvApplications.DataSource = LocalDrivingLicenseApplication.GetAll_LDL_Applications();
            SetNumberOfRecordsLabel();
            switch (cbFilterBy.SelectedItem)
            {
                case "None":
                    RefreshDGV();
                    break;

                case "Status":
                    cbApplicationStatus.Visible = true;
                    txtSearch.Visible = false;
                    cbApplicationStatus.SelectedItem = "All";
                    break;

                default:
                    cbApplicationStatus.Visible = false;
                    txtSearch.Visible = true;
                    break;
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            String SelectedItem = cbFilterBy.SelectedItem.ToString();

            if (SelectedItem == "L.D.L AppID" &&
                (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)))
            {
                e.Handled = true; // Stop the character from being entered
                return;
            }
        }

        private void cbApplicationStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            String SelectedItem = cbApplicationStatus.SelectedItem.ToString();

            switch (SelectedItem)
            {
                case "All":
                    dgvApplications.DataSource = LocalDrivingLicenseApplication.GetAll_LDL_Applications();
                    SetNumberOfRecordsLabel();
                    break;

                case "New":
                    dgvApplications.DataSource = LocalDrivingLicenseApplication.GetAllNew_LDL_Applications();
                    SetNumberOfRecordsLabel();
                    break;

                case "Cancelled":
                    dgvApplications.DataSource = LocalDrivingLicenseApplication.GetAllCancelled_LDL_Applications();
                    SetNumberOfRecordsLabel();
                    break;

                case "Completed":
                    dgvApplications.DataSource = LocalDrivingLicenseApplication.GetAllCompleted_LDL_Applications();
                    SetNumberOfRecordsLabel();
                    break;

            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show(
                "Are you sure you want to cancel this application?",
                "Confirm Canecllation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
                );

            if (Result == DialogResult.No)
            {
                return;
            }


            LocalDrivingLicenseApplication Application = GetApplicationSelectedFromDGV();

            Result Output = Application.CancelApplication();
            if (Output.IsSuccess)
            {
                MessageBox.Show("Application has been cancelled successfully",
                "Success",
                MessageBoxButtons.OK);
                RefreshDGV();
            }
            else
            {
                MessageBox.Show($"{Output.ErrorMessage}",
                "Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void deleteApplicatoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show(
               "Are you sure you want to cancel this application?",
               "Confirm Canecllation",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning,
               MessageBoxDefaultButton.Button2
               );

            if (Result == DialogResult.No)
            {
                return;
            }

            LocalDrivingLicenseApplication Application = GetApplicationSelectedFromDGV();

            Result Output = Application.DeleteApplication();
            if (Output.IsSuccess)
            {
                MessageBox.Show("Application has been deleted successfully",
                "Success",
                MessageBoxButtons.OK);
                RefreshDGV();
            }
            else
            {
                MessageBox.Show($"{Output.ErrorMessage}",
                "Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        LocalDrivingLicenseApplication GetApplicationSelectedFromDGV()
        {
            DataGridViewRow SelectedRow = dgvApplications.SelectedRows[0];
            int LDL_ApplicationID = Convert.ToInt32(SelectedRow.Cells["L.D.L.AppID"].Value);
            return LocalDrivingLicenseApplication.GetApplicationByID(LDL_ApplicationID);
        }

        private String GetStatusOfSelectedRecord()
        {
            DataGridViewRow SelectedRow = dgvApplications.SelectedRows[0];
            return SelectedRow.Cells["Status"].Value.ToString();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            String SelectedRecordStatus = GetStatusOfSelectedRecord();

            if (SelectedRecordStatus == "New")
            {
                SetContextMenuItemsForNewApplication();
                return;
            }

            if (SelectedRecordStatus == "Completed")
            {
                SetContextMenuItemsForCompletedApplication();
                return;
            }

            if (SelectedRecordStatus == "Cancelled")
            {
                SetContextMenuItemsForCancelledApplication();
                return;
            }


        }

        private void SetContextMenuItemsForCompletedApplication()
        {
            showApplicationDetailsToolStripMenuItem.Enabled = true;
            editApplicationToolStripMenuItem.Enabled = false;
            deleteApplicatoinToolStripMenuItem.Enabled = false;
            cancelApplicationToolStripMenuItem.Enabled = false;
            issueDrvingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = true;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
            scheduleTestsToolStripMenuItem.Enabled = false;
        }

        private void SetContextMenuItemsForCancelledApplication()
        {
            showApplicationDetailsToolStripMenuItem.Enabled = true;
            editApplicationToolStripMenuItem.Enabled = false;
            deleteApplicatoinToolStripMenuItem.Enabled = true;
            cancelApplicationToolStripMenuItem.Enabled = false;
            issueDrvingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
            scheduleTestsToolStripMenuItem.Enabled = false;
        }

        private void SetContextMenuItemsForNewApplication()
        {
            showApplicationDetailsToolStripMenuItem.Enabled = true;
            editApplicationToolStripMenuItem.Enabled = true;
            deleteApplicatoinToolStripMenuItem.Enabled = true;
            cancelApplicationToolStripMenuItem.Enabled = true;
            issueDrvingLicenseFirstTimeToolStripMenuItem.Enabled = GetNumberOfPassedTestsForSelectedRow() == 3;
            showLicenseToolStripMenuItem.Enabled = false;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
            EnableAppropriateTestTypeForNewApplication();
        }

        private void EnableAppropriateTestTypeForNewApplication()
        {
            scheduleTestsToolStripMenuItem.Enabled = true;
            scheduleVisionTestToolStripMenuItem.Enabled = false;
            scheduleWrittenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = false;

            switch (GetNumberOfPassedTestsForSelectedRow())
            {
                case 0:
                    scheduleVisionTestToolStripMenuItem.Enabled = true;
                    return;

                case 1:
                    scheduleWrittenTestToolStripMenuItem.Enabled = true;
                    return;


                case 2:
                    scheduleStreetTestToolStripMenuItem.Enabled = true;
                    return;

                default:
                    scheduleTestsToolStripMenuItem.Enabled = false;
                    return;

            }
        } 

        private byte GetNumberOfPassedTestsForSelectedRow()
        {
            DataGridViewRow SelectedRow = dgvApplications.SelectedRows[0];
            return Convert.ToByte(SelectedRow.Cells["Passed Tests"].Value);
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form Frm = new FrmTestAppointments(GetApplicationSelectedFromDGV(), enTestType.Vision);
            Frm.ShowDialog();
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form Frm = new FrmTestAppointments(GetApplicationSelectedFromDGV(), enTestType.Written);
            Frm.ShowDialog();
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form Frm = new FrmTestAppointments(GetApplicationSelectedFromDGV(), enTestType.Street);
            Frm.ShowDialog();
        }

 
    }
}
