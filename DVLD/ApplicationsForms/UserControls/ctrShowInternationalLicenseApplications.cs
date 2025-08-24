using BusinessLayer;
using BusinessLayer.Licenses;
using DVLD.Licenses;
using DVLD.Licenses.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.ApplicationsForms.UserControls
{
    public partial class ctrShowInternationalLicenseApplications : UserControl
    {
        public ctrShowInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void ctrShowInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            RefreshDGV();
        }

        public void RefreshDGV()
        {
            cbFilterBy.SelectedItem = "None";
            dgvApplications.DataSource = InternationalLicense.GetAllLicenses();
            SetNumberOfRecordsLabel();
        }

        private void SetNumberOfRecordsLabel()
        {
            lblNumberOfRecords.Text = Convert.ToString(dgvApplications.AllowUserToAddRows ? dgvApplications.Rows.Count - 1 : dgvApplications.Rows.Count);
        }

        private Person GetPersonSelectedFromDGV()
        {
            DataGridViewRow SelectedRow = dgvApplications.SelectedRows[0];
            int DriverID = Convert.ToInt32(SelectedRow.Cells["Driver ID"].Value);
            return Driver.GetDriverByDriverID(DriverID).PersonalInfo;
        }

        private InternationalLicense GetLicenseSelectedFromDGV()
        {
            DataGridViewRow SelectedRow = dgvApplications.SelectedRows[0];
            int LicenseID = Convert.ToInt32(SelectedRow.Cells["License ID"].Value);
            return InternationalLicense.GetLicenseByLicenseID(LicenseID);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtSearch.Text))
            {
                return;
            }


            switch (cbFilterBy.SelectedItem)
            {

                case "Int. LicenseID":
                    dgvApplications.DataSource = InternationalLicense.GetAllLicensesByLicenseIDLike(txtSearch.Text);
                    break;

                case "Loc. LicenseID":
                    dgvApplications.DataSource = InternationalLicense.GetAllLicensesByLocalLicenseIDLike(txtSearch.Text);
                    break;

                case "DriverID":
                    dgvApplications.DataSource = InternationalLicense.GetAllLicensesByDriverIDLike(txtSearch.Text);
                    break;

                default:
                    RefreshDGV();
                    break;
            }

            SetNumberOfRecordsLabel();

        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmShowPersonDetails(GetPersonSelectedFromDGV().PersonID);
            Frm.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmShowInternationalLicenseInfo(GetLicenseSelectedFromDGV());
            Frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmShowPersonLicenseHistory(GetPersonSelectedFromDGV());
            Frm.ShowDialog();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            if(cbFilterBy.SelectedItem.Equals("None"))
            {
                RefreshDGV();
                txtSearch.Visible = false;
                return;
            }

            txtSearch.Visible = true;

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Stop the character from being entered
                return;
            }
        }
    }
}
