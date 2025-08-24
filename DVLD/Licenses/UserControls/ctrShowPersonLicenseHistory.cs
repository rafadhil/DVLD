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

namespace DVLD.Licenses.UserControls
{
    public partial class ctrShowPersonLicenseHistory : UserControl
    {
        Person ActivePerson;
        public ctrShowPersonLicenseHistory()
        {
            InitializeComponent();
        }

        public void LoadInfo(Person person)
        {
            if(person == null)
            {
                MessageBox.Show("ERROR: Could not find person", "Failure");
                return;
            }
            ActivePerson = person;
            LoadDGV();
            SetRecordsCount();
        }


        private void LoadDGV()
        {
            dgvLocalLicenses.DataSource = BusinessLayer.License.GetAllLicensesForPerson(ActivePerson.PersonID);
            dgvInternationalLicenses.DataSource = InternationalLicense.GetAllLicensesForDriver(Driver.GetDriverByPersonID(ActivePerson.PersonID));
        }

        private void SetRecordsCount()
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                lblNumberOfRecords.Text = dgvLocalLicenses.Rows.Count.ToString();
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                lblNumberOfRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
            }
        }
    
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRecordsCount();
        }


        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmShowLicenseInfo(GetLocalLicenseSelectedFromDGV());
            Frm.ShowDialog();
        }

        private BusinessLayer.License GetLocalLicenseSelectedFromDGV()
        {
            DataGridViewRow SelectedRow = dgvLocalLicenses.SelectedRows[0];
            int LicenseID = Convert.ToInt32(SelectedRow.Cells["License ID"].Value);
            return BusinessLayer.License.GetLicenseByLicenseID(LicenseID);
        }
        private InternationalLicense GetInternationalLicenseSelectedFromDGV()
        {
            DataGridViewRow SelectedRow = dgvInternationalLicenses.SelectedRows[0];
            int LicenseID = Convert.ToInt32(SelectedRow.Cells["License ID"].Value);
            return InternationalLicense.GetLicenseByLicenseID(LicenseID);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form form = new FrmShowInternationalLicenseInfo(GetInternationalLicenseSelectedFromDGV());
            form.ShowDialog();
        }


    }
}
