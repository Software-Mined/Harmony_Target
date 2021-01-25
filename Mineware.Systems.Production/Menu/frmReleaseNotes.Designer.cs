namespace Mineware.Systems.Production.Menu
{
    partial class frmReleaseNotes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReleaseNotes));
            this.label11 = new System.Windows.Forms.Label();
            this.VersionTxt = new DevExpress.XtraEditors.TextEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.memoComments = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.VersionTxt.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoComments.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label11.Location = new System.Drawing.Point(495, 469);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 19);
            this.label11.TabIndex = 42;
            this.label11.Text = "Version";
            // 
            // VersionTxt
            // 
            this.VersionTxt.Enabled = false;
            this.VersionTxt.Location = new System.Drawing.Point(554, 466);
            this.VersionTxt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VersionTxt.Name = "VersionTxt";
            this.VersionTxt.Size = new System.Drawing.Size(87, 24);
            this.VersionTxt.TabIndex = 41;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.memoComments);
            this.panel2.Location = new System.Drawing.Point(14, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(640, 445);
            this.panel2.TabIndex = 141;
            // 
            // memoComments
            // 
            this.memoComments.EditValue = resources.GetString("memoComments.EditValue");
            this.memoComments.Enabled = false;
            this.memoComments.Location = new System.Drawing.Point(3, 4);
            this.memoComments.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.memoComments.Name = "memoComments";
            this.memoComments.Size = new System.Drawing.Size(634, 437);
            this.memoComments.TabIndex = 130;
            this.memoComments.EditValueChanged += new System.EventHandler(this.memoComments_EditValueChanged);
            // 
            // frmReleaseNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 507);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.VersionTxt);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmReleaseNotes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Release Notes";
            this.Load += new System.EventHandler(this.frmReleaseNotes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.VersionTxt.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoComments.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.TextEdit VersionTxt;
        public System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.MemoEdit memoComments;
    }
}