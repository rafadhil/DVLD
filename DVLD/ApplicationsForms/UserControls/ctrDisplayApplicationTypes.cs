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
    public partial class ctrDisplayApplicationTypes : UserControl
    {
        public ctrDisplayApplicationTypes()
        {
            InitializeComponent();
        }


        private void ctrDisplayApplicationTypes_Load(object sender, EventArgs e)
        {
            RefreshDGV();
            dgvApplicationTypes.Columns["Title"].Width = 430;
        }

        private void RefreshDGV()
        {
            dgvApplicationTypes.DataSource = ApplicationType.GetApplicationTypes();
            lblNumberOfRecords.Text = Convert.ToString(dgvApplicationTypes.AllowUserToAddRows ? 
                dgvApplicationTypes.Rows.Count - 1 : dgvApplicationTypes.Rows.Count);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DataGridViewRow SelectedRow = dgvApplicationTypes.SelectedRows[0];
            int ApplicationID = Convert.ToInt32(SelectedRow.Cells["ID"].Value);

            Form Frm = new FrmUpdateApplicationType(ApplicationID);
            Frm.ShowDialog();
            RefreshDGV();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvApplicationTypes.SelectedRows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

        }
    }
}
