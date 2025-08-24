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
    public partial class ctrFindLicense : UserControl
    {
        BusinessLayer.License ActiveLicense;

        public event Action<BusinessLayer.License> OnLicenseSelected;
        protected virtual void LicenseSelected(BusinessLayer.License LicenseID)
        {
            Action<BusinessLayer.License> handler = OnLicenseSelected;
            if (handler != null)
            {
                handler(ActiveLicense); // Raise the event with the parameter
            }
        }
        public ctrFindLicense()
        {
            InitializeComponent();
        }



        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtSearch.Text))
            {
                MessageBox.Show("Field cannot be empty", "Failure");
                return;
            }


            ActiveLicense = BusinessLayer.License.GetLicenseByLicenseID(Convert.ToInt32(txtSearch.Text));
            if (ActiveLicense == null)
                return;

            ctrShowLicense1.LoadInfo(ActiveLicense);

            if (OnLicenseSelected != null)
            {
                OnLicenseSelected(ActiveLicense);
            }

        }

        public void RefreshSelectedLicenseData()
        {
            if(ActiveLicense == null)
            {
                return;
            }

            ctrShowLicense1.LoadInfo(ActiveLicense);
        }

        public void ClearSelection()
        {
            ctrShowLicense1.ClearSelection();
            txtSearch.Text = "";
        }

        private BusinessLayer.License GetActiveLicense()
        {
            return ActiveLicense;
        }

        public bool IsSelectedLicenseActive()
        {
            return ActiveLicense.IsActive;
        }

        public void DisableSearch()
        {
            groupBox1.Enabled = false;
        }
        public void EnableSearch()
        {
            groupBox1.Enabled = true;
        }

        private void ctrShowLicense1_Load(object sender, EventArgs e)
        {

        }
    }
}
