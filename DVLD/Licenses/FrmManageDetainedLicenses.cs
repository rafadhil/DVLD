using BusinessLayer;
using BusinessLayer.Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class FrmManageDetainedLicenses : Form
    {
        public FrmManageDetainedLicenses()
        {
            InitializeComponent();
        }

       

        private void FrmManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            ResetDGV();
            cbIsReleased.SelectedItem = "All";
        }

        public void ResetDGV()
        {
            cbFilterBy.SelectedItem = "None";
            txtSearch.Visible = false;
            cbIsReleased.Visible = false;
            dgvLicenses.DataSource = DetainedLicense.GetAllRecords();
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if(IsSelectedLicenseReleased())
            {
                releaseDetainedLicenseToolStripMenuItem.Enabled = false;
            }
            else
            {
                releaseDetainedLicenseToolStripMenuItem.Enabled = true;

            }
        }

        private void SetNumberOfRecordsLabel()
        {
            lblNumberOfRecords.Text = Convert.ToString(dgvLicenses.AllowUserToAddRows ? dgvLicenses.Rows.Count - 1 : dgvLicenses.Rows.Count);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
            {
                dgvLicenses.DataSource = DetainedLicense.GetAllRecords();
                SetNumberOfRecordsLabel();
                return;
            }

            String SelectedItem = cbFilterBy.SelectedItem.ToString();
            if (SelectedItem.Equals("Detention ID"))
            {
                dgvLicenses.DataSource = DetainedLicense.GetAllRecordsByDetainIDLike(txtSearch.Text);
                SetNumberOfRecordsLabel();
                return;
            }


            if (SelectedItem == "National No.")
            {
                dgvLicenses.DataSource = DetainedLicense.GetAllRecordsByNationalNumberLike(txtSearch.Text);
                SetNumberOfRecordsLabel();
                return;
            }

            if (SelectedItem == "Full Name")
            {
                dgvLicenses.DataSource = DetainedLicense.GetAllRecordsByFullNameLike(txtSearch.Text);
                SetNumberOfRecordsLabel();
                return;
            }

            if (SelectedItem == "Release Application ID")
            {
                dgvLicenses.DataSource = DetainedLicense.GetAllReleasedRecordsByReleaseApplicationIDLike(txtSearch.Text);
                SetNumberOfRecordsLabel();
                return;
            }
        }

        Person GetPersonSelectedFromDGV()
        {
            DataGridViewRow SelectedRow = dgvLicenses.SelectedRows[0];
            String PersonNationalNumber =  SelectedRow.Cells["Nat.No."].Value.ToString();
            return Person.GetPersonByNationalNumber(PersonNationalNumber);
        }

       BusinessLayer.License GetLicenseSelectedFromDGV()
        {
            DataGridViewRow SelectedRow = dgvLicenses.SelectedRows[0];
            int LicenseID = Convert.ToInt32(SelectedRow.Cells["L.ID"].Value);
            return BusinessLayer.License.GetLicenseByLicenseID(LicenseID);
        }

        bool IsSelectedLicenseReleased()
        {
            DataGridViewRow SelectedRow = dgvLicenses.SelectedRows[0];
            return Convert.ToBoolean(SelectedRow.Cells["Is Released"].Value);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            String SelectedItem = cbFilterBy.SelectedItem.ToString();

            if ((SelectedItem.Equals("Detention ID") || SelectedItem == ("Release Application ID")) &&
                (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)))
            {
                e.Handled = true; // Stop the character from being entered
                return;
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            dgvLicenses.DataSource = dgvLicenses.DataSource = DetainedLicense.GetAllRecords();
            SetNumberOfRecordsLabel();
            switch (cbFilterBy.SelectedItem)
            {
                case "None":
                    ResetDGV();
                    return;

                case "Detention ID":
                    cbIsReleased.Visible = false;
                    txtSearch.Visible = true;
                    break;
                case "Is Released":
                    cbIsReleased.Visible = true;
                    txtSearch.Visible = false;
                    cbIsReleased.SelectedItem = "All";
                    break;
                case "National No.":
                    cbIsReleased.Visible = false;
                    txtSearch.Visible = true;
                    break;                
                case "Full Name":
                    cbIsReleased.Visible = false;
                    txtSearch.Visible = true;
                    break;
                case "Release Application ID":
                    cbIsReleased.Visible = false;
                    txtSearch.Visible = true;
                    break;

            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsReleased.SelectedItem)
            {
                case "All":
                    dgvLicenses.DataSource = DetainedLicense.GetAllRecords();
                    SetNumberOfRecordsLabel();
                    return;

                case "Released":
                    dgvLicenses.DataSource = DetainedLicense.GetAllReleasedRecords();
                    SetNumberOfRecordsLabel();
                    return;

                default:
                    dgvLicenses.DataSource = DetainedLicense.GetAllUnreleasedRecords();
                    SetNumberOfRecordsLabel();
                    return;
            }
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmDetainLicense();
            Frm.ShowDialog();
            ResetDGV();
        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmReleaseDetainedLicense();
            Frm.ShowDialog();
            ResetDGV();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmShowPersonDetails(GetPersonSelectedFromDGV().PersonID);
            Frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmShowLicenseInfo(GetLicenseSelectedFromDGV());
            Frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmShowPersonLicenseHistory(GetPersonSelectedFromDGV());
            Frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmReleaseDetainedLicense(GetLicenseSelectedFromDGV());
            Frm.ShowDialog();
            ResetDGV();
        }
    }
}
