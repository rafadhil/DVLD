namespace DVLD
{
    partial class FrmManageLocalDrivingLicenseApplications
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMode = new System.Windows.Forms.Label();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.ctrShowLocalDLApplications1 = new DVLD.ctrShowLocalDLApplications();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.Location = new System.Drawing.Point(288, 77);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(409, 30);
            this.lblMode.TabIndex = 8;
            this.lblMode.Text = "Manage Local Driving License Applications";
            // 
            // btnAddNew
            // 
            this.btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNew.Location = new System.Drawing.Point(895, 136);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(57, 42);
            this.btnAddNew.TabIndex = 9;
            this.btnAddNew.Text = "+";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // ctrShowLocalDLApplications1
            // 
            this.ctrShowLocalDLApplications1.Location = new System.Drawing.Point(12, 136);
            this.ctrShowLocalDLApplications1.Name = "ctrShowLocalDLApplications1";
            this.ctrShowLocalDLApplications1.Size = new System.Drawing.Size(962, 546);
            this.ctrShowLocalDLApplications1.TabIndex = 0;
            // 
            // FrmManageLocalDrivingLicenseApplications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 694);
            this.Controls.Add(this.btnAddNew);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.ctrShowLocalDLApplications1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmManageLocalDrivingLicenseApplications";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Local Driving License Applications";
            this.Load += new System.EventHandler(this.FrmManageLocalDrivingLicenseApplications_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ctrShowLocalDLApplications ctrShowLocalDLApplications1;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Button btnAddNew;
    }
}