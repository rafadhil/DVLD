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

namespace DVLD
{
    public partial class FrmManagePeople : Form
    {
        public FrmManagePeople()
        {
            InitializeComponent();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            
        }

        private void FrmManagePeople_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Form frm = new FrmAddNewUpdate();
            frm.ShowDialog();
            ctrShowPeople1.RefreshDGV();
        }
    }
}
