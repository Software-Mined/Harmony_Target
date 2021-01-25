namespace Mineware.Systems.ProductionReports.IncidentsReport
{
    partial class frmResponciblePersonFeedback
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResponciblePersonFeedback));
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.separatorButtons = new DevExpress.XtraEditors.SeparatorControl();
            this.txtFeedback = new DevExpress.XtraEditors.MemoEdit();
            this.chkClose = new DevExpress.XtraEditors.CheckEdit();
            this.pnlBottom.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFeedback.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkClose.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.pnlButtons);
            this.pnlBottom.Controls.Add(this.separatorButtons);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 195);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(597, 82);
            this.pnlBottom.TabIndex = 154;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Location = new System.Drawing.Point(159, 34);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(301, 35);
            this.pnlButtons.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCancel.ImageOptions.SvgImage")));
            this.btnCancel.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnCancel.Location = new System.Drawing.Point(155, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 26);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.ImageOptions.SvgImage = global::Mineware.Systems.ProductionReports.Properties.Resources.SaveBlue;
            this.btnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSave.Location = new System.Drawing.Point(56, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 26);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // separatorButtons
            // 
            this.separatorButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separatorButtons.Location = new System.Drawing.Point(12, 4);
            this.separatorButtons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.separatorButtons.Name = "separatorButtons";
            this.separatorButtons.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.separatorButtons.Size = new System.Drawing.Size(573, 37);
            this.separatorButtons.TabIndex = 3;
            // 
            // txtFeedback
            // 
            this.txtFeedback.Location = new System.Drawing.Point(12, 13);
            this.txtFeedback.Name = "txtFeedback";
            this.txtFeedback.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFeedback.Size = new System.Drawing.Size(575, 147);
            this.txtFeedback.TabIndex = 155;
            // 
            // chkClose
            // 
            this.chkClose.Location = new System.Drawing.Point(522, 166);
            this.chkClose.Name = "chkClose";
            this.chkClose.Properties.Caption = "Close";
            this.chkClose.Size = new System.Drawing.Size(65, 24);
            this.chkClose.TabIndex = 156;
            // 
            // frmResponciblePersonFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 277);
            this.Controls.Add(this.chkClose);
            this.Controls.Add(this.txtFeedback);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmResponciblePersonFeedback";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Feedback";
            this.Load += new System.EventHandler(this.frmResponciblePersonFeedback_Load);
            this.pnlBottom.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFeedback.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkClose.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlButtons;
        private DevExpress.XtraEditors.SeparatorControl separatorButtons;
        private DevExpress.XtraEditors.MemoEdit txtFeedback;
        private DevExpress.XtraEditors.CheckEdit chkClose;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
    }
}