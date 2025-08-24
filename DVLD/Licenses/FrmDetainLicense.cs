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
    public partial class FrmDetainLicense : Form
    {
        BusinessLayer.License ActiveLicense;
        public FrmDetainLicense()
        {
            InitializeComponent();
        }

        private void ctrFindLicense1_OnLicenseSelected(BusinessLayer.License obj)
        {
            lblShowLicensesHistory.Enabled = false;
            lblShowLicenseInfo.Enabled = false;

            if (obj.IsDetained())
            {
                MessageBox.Show("License is already detained, cannot proceed.");
                ctrFindLicense1.ClearSelection();
                return;
            }

            lblShowLicensesHistory.Enabled = true;
            lblShowLicenseInfo.Enabled = true;
            ActiveLicense = obj;

            lblLicenseID.Text = ActiveLicense.LicenseID.ToString();
            lblDriverID.Text = ActiveLicense.DriverInfo.DriverID.ToString();

            btnDetain.Enabled = true;
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtFineFees.Text))
            {
                MessageBox.Show("Field Fine Fees cannot be empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int DetentionID = ActiveLicense.Detain(Convert.ToDecimal(txtFineFees.Text));
            if (DetentionID == -1)
            {
                MessageBox.Show("Failed to detain license", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("License has been detained successfully");
            //LoadDetentionInfo();
            lblDetentionID.Text = DetentionID.ToString();
            btnDetain.Enabled = false;
            ctrFindLicense1.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadDetentionInfo()
        {

        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Stop the character from being entered
            }
        }

        private void FrmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetentionDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = UserSettings.LoggedInUser.Username.ToString();

        }

        private void lblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowLicenseInfo(ActiveLicense);
            Frm.ShowDialog();
        }

        private void lblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form Frm = new FrmShowPersonLicenseHistory(ActiveLicense.DriverInfo.PersonalInfo);
            Frm.ShowDialog();
        }
    }
}
