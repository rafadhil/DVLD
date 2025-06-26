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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD
{
    public partial class FrmAddNewLocalDrivingLicenseApplication : Form
    {
        private enum enMode { AddNew, Update}
        private enMode Mode;
        private bool IsApplicationInfoTabAllowed = false;
        private LocalDrivingLicenseApplication ActiveApplication;
        private DataTable LicenseClasses;
        public FrmAddNewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            Mode = enMode.AddNew;
        }
        public FrmAddNewLocalDrivingLicenseApplication(int ApplicationID)
        {
            InitializeComponent();
            ActiveApplication = LocalDrivingLicenseApplication.GetApplicationByID(ApplicationID);
            if(ActiveApplication == null)
            {
                MessageBox.Show($"ERROR: Could not find application with ID {ApplicationID}");
                this.Close();
            }

            Mode = enMode.Update;
        }


        private void FrmAddNewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            InitializeActiveApplicationObject();
            SetLabelApplicationDate();
            LoadLicenseClasses();
            Set_cbLicenseClassesItems();
            cbLicenseClasses.SelectedValue = "Class 3 - Ordinary driving license";
            SetLabelApplicationFees();
            SetLabelCreatedBy();
        }

        private void SetFormToUpdateMode()
        {
            lblMode.Text = "Update Local Driving License Application";
        }

        private void InitializeActiveApplicationObject()
        {
            ActiveApplication = new LocalDrivingLicenseApplication();
            ActiveApplication.OriginalApplicationInfo.CreatedByUser = UserSettings.LoggedInUser;
            ActiveApplication.OriginalApplicationInfo.ApplicationType = ApplicationType.GetApplicationTypeByID(1);
        }

        private void SetLabelApplicationDate()
        {
            DateTime Now = DateTime.Now;

            int Day = Now.Day;
            int Month = Now.Month;
            int Year = Now.Year;
            lblApplicationDate.Text = $"{Day}/{Month}/{Year}";
        }

        private void SetLabelApplicationFees()
        {
            lblApplicationFees.Text = ApplicationType.GetApplicationTypeByID(1).TypeFees.ToString();
        }

        private void LoadLicenseClasses()
        {
            LicenseClasses = LicenseClass.GetLicenseClassses();
        }

        private void Set_cbLicenseClassesItems()
        {
            cbLicenseClasses.DataSource = LicenseClasses;
            cbLicenseClasses.DisplayMember = "Name";
            cbLicenseClasses.ValueMember = "Name";
        }

        private void SetLabelCreatedBy()
        {
            lblCreatedBy.Text = UserSettings.LoggedInUser.Username;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(errorProvider1.GetError(btnNext)))
                tabControl1.SelectedTab = tabControl1.TabPages[1];
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabControl1.TabPages[1] && !IsApplicationInfoTabAllowed)
            {
                e.Cancel = true;
                MessageBox.Show("Login info tab is disabled, you must complete the current tab requirements first.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ctrFindPerson1_OnPersonSelected(int obj)
        {
            ActiveApplication.OriginalApplicationInfo.ApplicantInfo = Person.GetPersonByID(obj);
            IsApplicationInfoTabAllowed = true;
            btnSave.Enabled = true;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ActiveApplication.OriginalApplicationInfo.ApplicationDate = DateTime.Now;
            ActiveApplication.OriginalApplicationInfo.PaidFees = Convert.ToDecimal(lblApplicationFees.Text);
            ActiveApplication.LicenseClass = LicenseClass.GetClassByID(Convert.ToInt32(cbLicenseClasses.SelectedIndex + 1));

            Result Result = ActiveApplication.Save();
            if (Result.IsSuccess)
            {
                MessageBox.Show("Application info has been saved successfully", "Success");
                lblApplicationID.Text = ActiveApplication.LocalDrivingLicenseApplicationID.ToString();
                //SwithToUpdateMode();
            }
            else
            {
                MessageBox.Show(Result.ErrorMessage, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lblMode_Click(object sender, EventArgs e)
        {

        }
    }
}
