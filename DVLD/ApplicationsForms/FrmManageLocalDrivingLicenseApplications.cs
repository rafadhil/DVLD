using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class FrmManageLocalDrivingLicenseApplications : Form
    {
        public FrmManageLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void FrmManageLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {

        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Form Frm = new FrmAddNewLocalDrivingLicenseApplication();
            Frm.ShowDialog();
            ctrShowLocalDLApplications1.RefreshDGV();
        }
    }
}
