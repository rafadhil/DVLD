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
    public partial class FrmIssueDrivingLicenseFirstTime : Form
    {
        LocalDrivingLicenseApplication LDL_Application;
        public FrmIssueDrivingLicenseFirstTime(LocalDrivingLicenseApplication LocalDrivingLicenseApplication)
        {
            InitializeComponent();
            if(LocalDrivingLicenseApplication ==  null)
            {
                MessageBox.Show("ERROR: Could not find local driving license application", "Failed");
                this.Close();
                return;
            }

            LDL_Application = LocalDrivingLicenseApplication;

        }

        private void FrmIssueDrivingLicenseFirstTime_Load(object sender, EventArgs e)
        {
            
            ctrShowLocalDrivingLicenseApplicationInfo1.LoadInfo(LDL_Application);
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
       
            Result result = BusinessLayer.License.IssueNewLicense(
                LDL_Application.OriginalApplicationInfo.ApplicationID, 
                LDL_Application.LicenseClass.LicenseClassID, 
                txtNotes.Text, 
                LDL_Application.OriginalApplicationInfo.PaidFees, 
                (byte) Common.enLicenseIssueReason.FirstTime, 
                UserSettings.LoggedInUser.UserID
                );

            if(result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Success");
                this.Close();
                return;
            }

            MessageBox.Show(result.Message, "Failure");
            this.Close();
            return;

        }
    }
}
