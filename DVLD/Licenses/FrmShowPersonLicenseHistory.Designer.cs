namespace DVLD.Licenses
{
    partial class FrmShowPersonLicenseHistory
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
            this.ctrFindPerson1 = new DVLD.ctrFindPerson();
            this.ctrShowPersonLicenseHistory1 = new DVLD.Licenses.UserControls.ctrShowPersonLicenseHistory();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.Location = new System.Drawing.Point(328, 37);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(152, 30);
            this.lblMode.TabIndex = 10;
            this.lblMode.Text = "License History";
            // 
            // ctrFindPerson1
            // 
            this.ctrFindPerson1.Font = new System.Drawing.Font("Yu Gothic UI", 8.25F);
            this.ctrFindPerson1.Location = new System.Drawing.Point(12, 84);
            this.ctrFindPerson1.Name = "ctrFindPerson1";
            this.ctrFindPerson1.Size = new System.Drawing.Size(805, 378);
            this.ctrFindPerson1.TabIndex = 11;
            // 
            // ctrShowPersonLicenseHistory1
            // 
            this.ctrShowPersonLicenseHistory1.Location = new System.Drawing.Point(12, 468);
            this.ctrShowPersonLicenseHistory1.Name = "ctrShowPersonLicenseHistory1";
            this.ctrShowPersonLicenseHistory1.Size = new System.Drawing.Size(804, 329);
            this.ctrShowPersonLicenseHistory1.TabIndex = 12;
            // 
            // FrmShowPersonLicenseHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 806);
            this.Controls.Add(this.ctrShowPersonLicenseHistory1);
            this.Controls.Add(this.ctrFindPerson1);
            this.Controls.Add(this.lblMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmShowPersonLicenseHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Person License History";
            this.Load += new System.EventHandler(this.FrmShowPersonLicenseHistory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private ctrFindPerson ctrFindPerson1;
        private UserControls.ctrShowPersonLicenseHistory ctrShowPersonLicenseHistory1;
    }
}