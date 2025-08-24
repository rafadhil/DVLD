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
    public partial class FrmReplaceLocalDrivingLicense : Form
    {
        private BusinessLayer.License ActiveOldLicense;
        private BusinessLayer.License ReplacementLicense;
        private BusinessLayer.Application NewLicenseApplication;
        private decimal ReplacementForDamagedLicenseApplicationFees;
        private decimal ReplacementForLostLicenseApplicationFees;
        public FrmReplaceLocalDrivingLicense()
        {
            InitializeComponent();
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDamagedLicense.Checked)
            {
                lblMode.Text = "Replacement For Damaged License";
                lblApplicationFees.Text = ReplacementForDamagedLicenseApplicationFees.ToString();
            }
            else
            {
                lblMode.Text = "Replacement For Lost License";
                lblApplicationFees.Text = ReplacementForLostLicenseApplicationFees.ToString();
            }
        }

    
        private void ctrFindLicense1_OnLicenseSelected(BusinessLayer.License obj)
        {
            lblShowLicensesHistory.Enabled = true;
            if(!ctrFindLicense1.IsSelectedLicenseActive())
            {
                MessageBox.Show("License is not active, cannot proceed.");
                return;
            }

            ActiveOldLicense = obj;
            lblOldLicenseID.Text = ActiveOldLicense.LicenseID.ToString();
            btnIssue.Enabled = true;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            NewLicenseApplication = _CreateNewApplication();
            if(NewLicenseApplication == null)
            {
                MessageBox.Show("And error has occured while trying to create a new appliction, issuing failed");
                return;
            }

            Result result;
            if(rbDamagedLicense.Checked)
            {
                result = BusinessLayer.License.ReplaceDamagedLicense(NewLicenseApplication, ActiveOldLicense, GetPaidFees());
            }
            else
            {
                result = BusinessLayer.License.ReplaceLostLicense(NewLicenseApplication, ActiveOldLicense, GetPaidFees());
            }

            if(!result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Failure");
                return;
            }

            ReplacementLicense = BusinessLayer.License.GetLicenseByApplicationID(NewLicenseApplication.ApplicationID);
            LoadNewInfo();
            MessageBox.Show(result.Message);
            lblShowNewLicenseInfo.Enabled = true;
            btnIssue.Enabled = false;
            groupBox1.Enabled = false;
            ctrFindLicense1.DisableSearch();
        }

        private void LoadNewInfo()
        {
            lblReplacedLicenseID.Text = ReplacementLicense.LicenseID.ToString();
            lblLicenseReplacementApplicationID.Text = NewLicenseApplication.ApplicationID.ToString();
            lblApplicationDate.Text = NewLicenseApplication.ApplicationDate.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = ReplacementLicense.CreatedByUser.Username;
        }

        private decimal GetPaidFees()
        {
            return Convert.ToDecimal(lblApplicationFees.Text);
        }

        private BusinessLayer.Application _CreateNewApplication()
        {
            BusinessLayer.Application application = new BusinessLayer.Application();
            application.ApplicationDate = DateTime.Now;
            application.PaidFees = Convert.ToDecimal(lblApplicationFees.Text);
            application.ApplicantInfo = ActiveOldLicense.Application.ApplicantInfo;
            int ApplicationTypeID = _GetApplicationTypeID();
            application.ApplicationType = ApplicationType.GetApplicationTypeByID(ApplicationTypeID);
            application.CreatedByUser = UserSettings.LoggedInUser;
            if (application.Save())
            {
                return application;
            }
            else
                return null;
        }

        private int _GetApplicationTypeID()
        {
            return rbDamagedLicense.Checked ?
                (int)Common.enApplicationType.ReplacementForDamagedLicense :
                (int)Common.enApplicationType.ReplacementForLostLicense;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmReplaceLocalDrivingLicense_Load(object sender, EventArgs e)
        {
           ReplacementForDamagedLicenseApplicationFees = 
                ApplicationType.GetApplicationTypeByID((int)Common.enApplicationType.ReplacementForDamagedLicense).TypeFees;

            ReplacementForLostLicenseApplicationFees =
                 ApplicationType.GetApplicationTypeByID((int)Common.enApplicationType.ReplacementForLostLicense).TypeFees;

            lblApplicationFees.Text = ReplacementForDamagedLicenseApplicationFees.ToString();
        }

        private void lblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowLicenseInfo(ReplacementLicense);
            Frm.ShowDialog();
        }

        private void lblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowPersonLicenseHistory(ActiveOldLicense.Application.ApplicantInfo);
            Frm.ShowDialog();
        }
    }
}
