namespace DVLD
{
    partial class FrmShowPersonDetails
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ctrDisplayPersonDetails1 = new DVLD.ctrDisplayPersonDetails();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ctrDisplayPersonDetails1
            // 
            this.ctrDisplayPersonDetails1.AutoSize = true;
            this.ctrDisplayPersonDetails1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ctrDisplayPersonDetails1.Location = new System.Drawing.Point(19, 122);
            this.ctrDisplayPersonDetails1.Name = "ctrDisplayPersonDetails1";
            this.ctrDisplayPersonDetails1.Size = new System.Drawing.Size(797, 307);
            this.ctrDisplayPersonDetails1.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(331, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 30);
            this.label1.TabIndex = 42;
            this.label1.Text = "Person Details";
            // 
            // FrmShowPersonDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(828, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctrDisplayPersonDetails1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmShowPersonDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Person Info";
            this.Load += new System.EventHandler(this.FrmShowPersonDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ctrDisplayPersonDetails ctrDisplayPersonDetails1;
        private System.Windows.Forms.Label label1;
    }
}