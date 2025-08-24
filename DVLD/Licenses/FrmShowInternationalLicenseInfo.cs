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
    public partial class FrmShowInternationalLicenseInfo : Form
    {
        InternationalLicense ActiveLicense;
        public FrmShowInternationalLicenseInfo(InternationalLicense License)
        {
            InitializeComponent();
            if(License == null)
            {
                MessageBox.Show("ERROR: Cannot find license, closing form..");
                this.Close();
                return;
            }

            ActiveLicense = License;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrShowInternationalLicenseInfo1.LoadInfo(ActiveLicense);
        }
    }
}
