namespace DVLD.ApplicationsForms
{
    partial class FrmShowInternationalLicenseApplications
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
            this.ctrShowInternationalLicenseApplications1 = new DVLD.ApplicationsForms.UserControls.ctrShowInternationalLicenseApplications();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.Location = new System.Drawing.Point(271, 65);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(397, 30);
            this.lblMode.TabIndex = 11;
            this.lblMode.Text = "International Driving License Applications";
            // 
            // ctrShowInternationalLicenseApplications1
            // 
            this.ctrShowInternationalLicenseApplications1.Location = new System.Drawing.Point(12, 113);
            this.ctrShowInternationalLicenseApplications1.Name = "ctrShowInternationalLicenseApplications1";
            this.ctrShowInternationalLicenseApplications1.Size = new System.Drawing.Size(952, 549);
            this.ctrShowInternationalLicenseApplications1.TabIndex = 12;
            // 
            // btnAddNew
            // 
            this.btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNew.Location = new System.Drawing.Point(881, 113);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(57, 42);
            this.btnAddNew.TabIndex = 13;
            this.btnAddNew.Text = "+";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // FrmShowInternationalLicenseApplications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 675);
            this.Controls.Add(this.btnAddNew);
            this.Controls.Add(this.ctrShowInternationalLicenseApplications1);
            this.Controls.Add(this.lblMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmShowInternationalLicenseApplications";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "International Driving License Applications";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private UserControls.ctrShowInternationalLicenseApplications ctrShowInternationalLicenseApplications1;
        private System.Windows.Forms.Button btnAddNew;
    }
}