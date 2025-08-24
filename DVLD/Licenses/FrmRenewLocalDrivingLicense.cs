using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class FrmRenewLocalDrivingLicense : Form
    {
        private BusinessLayer.License ActiveOldLicense;
        private BusinessLayer.License NewLicense;
        private BusinessLayer.Application RenewLicenseApplication;
        public FrmRenewLocalDrivingLicense()
        {
            InitializeComponent();
        }

        private void ctrFindLicense1_OnLicenseSelected(BusinessLayer.License obj)
        {
            DisableControls();

            if (!obj.IsExpired())
            {
                MessageBox.Show(
                    $"ERROR: License is not expired yet, it expires on {obj.ExpirationDate.ToString("dd/MMM/yyyy")}",
                    "Failure",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ctrFindLicense1.ClearSelection();
                btnIssue.Enabled = false;
                return;
            }

            if(!obj.IsActive)
            {
                MessageBox.Show($"ERROR: License is not active", 
                    "Failure",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ctrFindLicense1.ClearSelection();
                btnIssue.Enabled = false;
                return;
            }

            lblShowLicensesHistory.Enabled = true;
            ActiveOldLicense = obj;
            lblOldLicenseID.Text = ActiveOldLicense.LicenseID.ToString();
            SetLicenseFeesLabel();
            SetTotalFeesLabel();
            btnIssue.Enabled = true;
        }

        private void DisableControls()
        {
            btnIssue.Enabled = false;
            lblShowLicensesHistory.Enabled = false;
            lblShowLicensesHistory.Enabled = false;
        }

        private void FrmRenewLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            SetApplicationFeesLabel();
        }

        private void SetApplicationFeesLabel()
        {
            int RenewApplicationTypeID = (int)Common.enApplicationType.RenewDrivingLicense;
            lblApplicationFees.Text = ApplicationType.GetApplicationTypeByID(RenewApplicationTypeID).TypeFees.ToString();
        }

        private void SetTotalFeesLabel()
        {
            lblTotalFees.Text = (Convert.ToDecimal(lblLicenseFees.Text) + Convert.ToDecimal(lblApplicationFees.Text)).ToString();
        }

        private void SetLicenseFeesLabel()
        { 
            lblLicenseFees.Text = ActiveOldLicense.LicenseClass.ClassFees.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            RenewLicenseApplication = CreateNewApplication();
            if (RenewLicenseApplication == null)
            {
                MessageBox.Show("An error has occured while trying to create application, issuing failed", "ERROR");
                return;
            }

           Result result = BusinessLayer.License.RenewLicense(
                RenewLicenseApplication,
                ActiveOldLicense, 
                txtNotes.Text, 
                Convert.ToDecimal(lblTotalFees.Text));

            if(result.IsSuccess)
            {
                MessageBox.Show(result.Message);
                NewLicense = BusinessLayer.License.GetLicenseByApplicationID(RenewLicenseApplication.ApplicationID);
                lblShowNewLicenseInfo.Enabled = true;
                LoadNewLicenseInfo();
            }
            else
            {
                MessageBox.Show(result.Message);
            }

            btnIssue.Enabled = false;
        }

        private void LoadNewLicenseInfo()
        {
            lblRenewingLicenseApplicationID.Text = RenewLicenseApplication.ApplicationID.ToString();
            lblApplicationDate.Text = RenewLicenseApplication.ApplicationDate.ToString("dd/MMM/yyyy");
            lblRenewedLicenseID.Text = NewLicense.LicenseID.ToString();
            lblIssueDate.Text = NewLicense.IssueDate.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = NewLicense.CreatedByUser.Username;
        }


        private BusinessLayer.Application CreateNewApplication()
        {
            BusinessLayer.Application application = new BusinessLayer.Application();
            application.ApplicationDate = DateTime.Now;
            application.PaidFees = Convert.ToDecimal(lblApplicationFees.Text);
            application.ApplicantInfo = ActiveOldLicense.Application.ApplicantInfo;
            application.ApplicationType = ApplicationType.GetApplicationTypeByID((int)Common.enApplicationType.RenewDrivingLicense);
            application.CreatedByUser = UserSettings.LoggedInUser;
            if (application.Save())
            {
                return application;
            }
            else
                return null;
        }

        private void lblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowPersonLicenseHistory(ActiveOldLicense.Application.ApplicantInfo);
            Frm.ShowDialog();
        }

        private void lblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowLicenseInfo(NewLicense);
            Frm.ShowDialog();
        }
    }
}
