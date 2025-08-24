namespace DVLD
{
    partial class FrmDisplayUserDetails
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
            this.gbLoginInformation = new System.Windows.Forms.GroupBox();
            this.txtIsActive = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ctrDisplayPersonDetails1 = new DVLD.ctrDisplayPersonDetails();
            this.gbLoginInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLoginInformation
            // 
            this.gbLoginInformation.Controls.Add(this.txtIsActive);
            this.gbLoginInformation.Controls.Add(this.txtUsername);
            this.gbLoginInformation.Controls.Add(this.txtUserID);
            this.gbLoginInformation.Controls.Add(this.label1);
            this.gbLoginInformation.Controls.Add(this.label2);
            this.gbLoginInformation.Controls.Add(this.label3);
            this.gbLoginInformation.Location = new System.Drawing.Point(12, 325);
            this.gbLoginInformation.Name = "gbLoginInformation";
            this.gbLoginInformation.Size = new System.Drawing.Size(797, 62);
            this.gbLoginInformation.TabIndex = 3;
            this.gbLoginInformation.TabStop = false;
            this.gbLoginInformation.Text = "Login Information";
            // 
            // txtIsActive
            // 
            this.txtIsActive.AutoSize = true;
            this.txtIsActive.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.txtIsActive.Location = new System.Drawing.Point(707, 25);
            this.txtIsActive.Name = "txtIsActive";
            this.txtIsActive.Size = new System.Drawing.Size(34, 21);
            this.txtIsActive.TabIndex = 34;
            this.txtIsActive.Text = "[??]";
            // 
            // txtUsername
            // 
            this.txtUsername.AutoSize = true;
            this.txtUsername.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.txtUsername.Location = new System.Drawing.Point(277, 25);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(34, 21);
            this.txtUsername.TabIndex = 33;
            this.txtUsername.Text = "[??]";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = true;
            this.txtUserID.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.txtUserID.Location = new System.Drawing.Point(96, 25);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(34, 21);
            this.txtUserID.TabIndex = 32;
            this.txtUserID.Text = "[??]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.label1.Location = new System.Drawing.Point(19, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 21);
            this.label1.TabIndex = 31;
            this.label1.Text = "UserID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.label2.Location = new System.Drawing.Point(623, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 21);
            this.label2.TabIndex = 29;
            this.label2.Text = "IsActive:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.label3.Location = new System.Drawing.Point(175, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 21);
            this.label3.TabIndex = 30;
            this.label3.Text = "Username:";
            // 
            // ctrDisplayPersonDetails1
            // 
            this.ctrDisplayPersonDetails1.AutoSize = true;
            this.ctrDisplayPersonDetails1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ctrDisplayPersonDetails1.Location = new System.Drawing.Point(12, 12);
            this.ctrDisplayPersonDetails1.Name = "ctrDisplayPersonDetails1";
            this.ctrDisplayPersonDetails1.Size = new System.Drawing.Size(797, 307);
            this.ctrDisplayPersonDetails1.TabIndex = 2;
            // 
            // FrmDisplayUserDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 445);
            this.Controls.Add(this.gbLoginInformation);
            this.Controls.Add(this.ctrDisplayPersonDetails1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmDisplayUserDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Info";
            this.Load += new System.EventHandler(this.FrmDisplayUserDetails_Load);
            this.gbLoginInformation.ResumeLayout(false);
            this.gbLoginInformation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLoginInformation;
        private System.Windows.Forms.Label txtIsActive;
        private System.Windows.Forms.Label txtUsername;
        private System.Windows.Forms.Label txtUserID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ctrDisplayPersonDetails ctrDisplayPersonDetails1;
    }
}