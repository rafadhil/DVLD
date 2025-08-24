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
using BusinessLayer.Licenses;
using DVLD.Licenses;
using DVLD.Licenses.UserControls;
namespace DVLD.ApplicationsForms.UserControls
{
    public partial class FrmAddNewInternationalLicenseApplication : Form
    {
        BusinessLayer.License ActiveLocalLicense;
        DateTime ApplicationDate;
        public FrmAddNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void ctrFindLicense1_OnLicenseSelected(BusinessLayer.License obj)
        {
            btnIssue.Enabled = false;
            int OrdinaryDrivingLicenseClassID = 3;
            if(obj.LicenseClass.LicenseClassID != OrdinaryDrivingLicenseClassID)
            {
                MessageBox.Show("Not a license of Class 3 - Ordinary driving license, cannot proceed");
                ctrFindLicense1.ClearSelection();
                return;
            }
            if (!obj.IsActive)
            {
                MessageBox.Show("License is not active, cannot proceed");
                ctrFindLicense1.ClearSelection();
                return;
            }

            if (obj.IsExpired())
            {
                MessageBox.Show("License is expired, cannot proceed");
                ctrFindLicense1.ClearSelection();
                return;
            }

            if(obj.IsDetained())
            {
                MessageBox.Show("License is under detention, cannot proceed");
                ctrFindLicense1.ClearSelection();
                return;
            }

            if(InternationalLicense.HasActiveInternationalLicense(obj.DriverInfo))
            {
                MessageBox.Show("Driver already has an active international license, cannot proceed");
                lblShowLicenseInfo.Enabled = true;
                lblShowLicensesHistory.Enabled = true;
                ActiveLocalLicense = obj;
                return;
            }

            lblShowLicensesHistory.Enabled = true;
            btnIssue.Enabled = true;
            ActiveLocalLicense = obj;

        }

        private void FrmAddNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            ApplicationDate = DateTime.Now;
            lblFees.Text = ApplicationType.GetApplicationTypeByID(6).TypeFees.ToString();
            lblIssueDate.Text = ApplicationDate.ToString("dd/MMM/yyyy");
            lblExpirationDate.Text = ApplicationDate.AddYears(1).ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = UserSettings.LoggedInUser.Username;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            BusinessLayer.Application NewApplication = MakeNewApplication();
            if(NewApplication == null)
            {
                MessageBox.Show("ERROR: Could not make a new application");
                return;
            }

            lblApplicationID.Text = NewApplication.ApplicationID.ToString();

            Result result = InternationalLicense.IssueNewLicense(NewApplication, ActiveLocalLicense.DriverInfo, ActiveLocalLicense);

            if(result.IsSuccess)
            {
                lblShowLicenseInfo.Enabled = true;
            }

            MessageBox.Show(result.Message);
            btnIssue.Enabled = false;
        }

        private BusinessLayer.Application MakeNewApplication()
        {
            int NewInternationalLicenseTypeID = 6;

            BusinessLayer.Application application = new BusinessLayer.Application();
            application.ApplicationType = ApplicationType.GetApplicationTypeByID(NewInternationalLicenseTypeID);
            application.ApplicantInfo = ActiveLocalLicense.DriverInfo.PersonalInfo;
            application.ApplicationDate = ApplicationDate;
            application.PaidFees = Convert.ToDecimal(lblFees.Text);
            application.CreatedByUser = UserSettings.LoggedInUser;

            if (application.Save())
                return application;
            else
                return null;
        }

        private void gb1_Enter(object sender, EventArgs e)
        {

        }

        private void lblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowPersonLicenseHistory(ActiveLocalLicense.Application.ApplicantInfo);
            Frm.ShowDialog();
        }

        private void lblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowInternationalLicenseInfo(InternationalLicense.GetLicenseByLocalLicenseID(ActiveLocalLicense.LicenseID));
            Frm.ShowDialog();
        }
    }
}
