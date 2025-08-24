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
    public partial class FrmShowLicenseInfo : Form
    {
        BusinessLayer.License ActiveDriverLicense;
        public FrmShowLicenseInfo(BusinessLayer.License DriverLicense)
        {
            InitializeComponent();

            if (DriverLicense == null)
            {
                MessageBox.Show("ERROR: Could not find driving license", "Failure");
                this.Close();
                return;
            }

            ActiveDriverLicense = DriverLicense;

        }

        private void FrmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrShowLicense1.LoadInfo(ActiveDriverLicense);
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
