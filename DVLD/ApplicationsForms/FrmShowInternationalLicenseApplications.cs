using DVLD.ApplicationsForms.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.ApplicationsForms
{
    public partial class FrmShowInternationalLicenseApplications : Form
    {
        public FrmShowInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmAddNewInternationalLicenseApplication();
            Frm.ShowDialog();
        }
    }
}
