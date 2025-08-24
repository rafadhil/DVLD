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
    public partial class FrmManageDetainedLicenses : Form
    {
        public FrmManageDetainedLicenses()
        {
            InitializeComponent();
        }

       

        private void FrmManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            dgvLicenses.DataSource = DetainedLicense.GetAllRecords();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
