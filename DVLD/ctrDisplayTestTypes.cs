using BusinessLayer;
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
    public partial class ctrDisplayTestTypes : UserControl
    {
        public ctrDisplayTestTypes()
        {
            InitializeComponent();
        }

        private void ctrDisplayTestTypes_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            RefreshDGV();
            dgvTestTypes.Columns["ID"].Width = 50;
            dgvTestTypes.Columns["Title"].Width = 150;
            dgvTestTypes.Columns["Description"].Width = 330;

            

        }

        public void RefreshDGV()
        {
            dgvTestTypes.DataSource = TestType.GetTestTypes();
            lblNumberOfRecords.Text = Convert.ToString(dgvTestTypes.AllowUserToAddRows ?
                dgvTestTypes.Rows.Count - 1 : dgvTestTypes.Rows.Count);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DataGridViewRow SelectedRow = dgvTestTypes.SelectedRows[0];
            int TestID = Convert.ToInt32(SelectedRow.Cells["ID"].Value);
            Form Frm = new FrmUpdateTestType(TestID);
            Frm.ShowDialog();
            RefreshDGV();
        }
    }
}
