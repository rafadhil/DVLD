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
using DVLD.Licenses;

namespace DVLD.Drivers
{
    public partial class ctrShowDrivers : UserControl
    {
        public ctrShowDrivers()
        {
            InitializeComponent();
        }

        private void ctrShowDrivers_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            RefreshDGV();
        }

        public void RefreshDGV()
        {
            dgvDrivers.DataSource = Driver.GetAllDrivers();
            cbFilterBy.SelectedIndex = 0;
            SetNumberOfRecordsInDGV();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";

            if(cbFilterBy.SelectedItem.Equals("None"))
            {
                txtSearch.Visible = false;
                RefreshDGV();
            }
            else
            {
                txtSearch.Visible = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.SelectedItem)
            {

                case "Driver ID":
                    dgvDrivers.DataSource = Driver.GetAllDriversByDriverIDLike(txtSearch.Text);
                    break;

                case "Person ID":
                    dgvDrivers.DataSource = Driver.GetAllDriversByPersonIDLike(txtSearch.Text);
                    break;

                case "National Number":
                    dgvDrivers.DataSource = Driver.GetAllDriversByNationalNumberLike(txtSearch.Text);
                    break;

                case "Full Name":
                    dgvDrivers.DataSource = Driver.GetAllDriversByFullNameLike(txtSearch.Text);
                    break;
            }

            SetNumberOfRecordsInDGV();
        }



        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((cbFilterBy.SelectedItem.Equals("Driver ID") || cbFilterBy.SelectedItem.Equals("Person ID")) &&
                !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SetNumberOfRecordsInDGV()
        {
            lblNumberOfRecords.Text = Convert.ToString(dgvDrivers.Rows.Count);

        }

        private void showLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmShowPersonLicenseHistory(GetPersonSelectedFromDGV());
            Frm.ShowDialog();
        }

        Person GetPersonSelectedFromDGV()
        {
            DataGridViewRow SelectedRow = dgvDrivers.SelectedRows[0];
            int PersonID = Convert.ToInt32(SelectedRow.Cells["Person ID"].Value);
            return Person.GetPersonByID(PersonID);
        }
    }
}
