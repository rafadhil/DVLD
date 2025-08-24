using BusinessLayer;
using BusinessLayer.Licenses;
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
    public partial class FrmReleaseDetainedLicense : Form
    {
        BusinessLayer.License ActiveLicense;
        DetainedLicense DetentionDetails;
        public FrmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        private void ctrFindLicense1_OnLicenseSelected(BusinessLayer.License obj)
        {
            lblShowLicensesHistory.Enabled = false;
            lblShowLicenseInfo.Enabled = false;

            if (!obj.IsDetained())
            {
                MessageBox.Show("License isn't detained, cannot proceed.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrFindLicense1.ClearSelection();
                return;
            }


            lblShowLicensesHistory.Enabled = true;
            lblShowLicenseInfo.Enabled = true;
            ActiveLicense = obj;

            DetentionDetails = DetainedLicense.GetDetentionInfoForDetainedLicense(ActiveLicense);

            lblCreatedBy.Text = DetentionDetails.CreatedBy.Username;
            lblDetentionDate.Text = DetentionDetails.DetainTime.ToString("dd/MMM/yyyy");
            lblDetentionID.Text = DetentionDetails.DetainID.ToString();
            lblFineFees.Text = DetentionDetails.FineFees.ToString();
            lblTotalFees.Text = GetTotalFees().ToString();

            lblLicenseID.Text = ActiveLicense.LicenseID.ToString();
            btnRelease.Enabled = true;
        }

        private decimal GetTotalFees()
        {
            return Convert.ToDecimal(lblFineFees.Text) + Convert.ToDecimal(lblApplicationFees.Text);
        }
        private void lblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowPersonLicenseHistory(ActiveLicense.DriverInfo.PersonalInfo);
            Frm.ShowDialog();
        }

        private void lblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowLicenseInfo(ActiveLicense);
            Frm.ShowDialog();
        }

        private void FrmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            lblApplicationFees.Text = ApplicationType.GetApplicationTypeByID(
               (int)Common.enApplicationType.ReleaseDetainedLicense).TypeFees.ToString();


        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
           BusinessLayer.Application ReleaseApplication = CreateNewReleaseDetainedLicenseApplication();
            if (ReleaseApplication == null)
            {
                MessageBox.Show("ERROR: Failed to create application");
                return;
            }

            Result ReleaseResult = DetainedLicense.ReleaseLicense(ActiveLicense, ReleaseApplication);
            if (!ReleaseResult.IsSuccess)
            {
                MessageBox.Show(ReleaseResult.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationID.Text = ReleaseApplication.ApplicationID.ToString();
            MessageBox.Show("License has been successfully released", "Success");
            btnRelease.Enabled = false;
            ctrFindLicense1.RefreshSelectedLicenseData();
            ctrFindLicense1.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private BusinessLayer.Application CreateNewReleaseDetainedLicenseApplication()
        {
            BusinessLayer.Application application = new BusinessLayer.Application();
            application.ApplicationDate = DateTime.Now;
            application.PaidFees = Convert.ToDecimal(lblApplicationFees.Text);
            application.ApplicantInfo = ActiveLicense.Application.ApplicantInfo;
            application.ApplicationType = ApplicationType.GetApplicationTypeByID((int)Common.enApplicationType.ReleaseDetainedLicense);
            application.CreatedByUser = UserSettings.LoggedInUser;
            if (application.Save())
            {
                return application;
            }
            else
                return null;
        }
    }
}
