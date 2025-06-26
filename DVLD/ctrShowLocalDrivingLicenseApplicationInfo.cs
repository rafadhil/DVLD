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

namespace DVLD
{
    public partial class ctrShowLocalDrivingLicenseApplicationInfo : UserControl
    {
        private LocalDrivingLicenseApplication ActiveApplication;
        public ctrShowLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }
        public void LoadInfo(LocalDrivingLicenseApplication Application)
        {

            if (Application == null)
            {
                MessageBox.Show($"ERROR: Could not find local driving license application, loading failed");
                return;
            }
            ActiveApplication = Application;

            lbl_DL_ApplicationID.Text = ActiveApplication.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = ActiveApplication.LicenseClass.ClassName;
            lblPassedTests.Text = ActiveApplication.GetNumberOfPassedTests().ToString() + "/3";

            lblApplicationID.Text = ActiveApplication.OriginalApplicationInfo.ApplicationID.ToString();
            lblStatus.Text = ActiveApplication.OriginalApplicationInfo.GetStatus();
            lblFees.Text = ActiveApplication.OriginalApplicationInfo.PaidFees.ToString();
            lblType.Text = ActiveApplication.OriginalApplicationInfo.ApplicationType.TypeTitle;
            lblApplicantFullName.Text = ActiveApplication.OriginalApplicationInfo.ApplicantInfo.GetFullName();
            lblApplicationDate.Text = ActiveApplication.OriginalApplicationInfo.ApplicationDate.ToString("d");
            lblApplicationStatusDate.Text = ActiveApplication.OriginalApplicationInfo.LastStatusDate.ToString("d");

            if (ActiveApplication.OriginalApplicationInfo.CreatedByUser == null)
            {
                MessageBox.Show("Could not find the info for the user who created this record");
                
            }
            else
                lblCreatedBy.Text = ActiveApplication.OriginalApplicationInfo.CreatedByUser.Username;
        }


        private void ctrShowLocalDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
        }

        private void lblViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lblViewPersonInfo_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowPersonDetails(ActiveApplication.OriginalApplicationInfo.ApplicantInfo.PersonID);
            Frm.ShowDialog();
        }
    }
}
