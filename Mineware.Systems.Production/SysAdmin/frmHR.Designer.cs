namespace Mineware.Systems.Production.SysAdmin
{
    partial class frmHR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHR));
            this.pnlProp = new System.Windows.Forms.Panel();
            this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGroupName = new DevExpress.XtraEditors.TextEdit();
            this.lblEnquirerID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtActivity = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnSaveProb = new DevExpress.XtraEditors.SimpleButton();
            this.pnlProp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtActivity.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlProp
            // 
            this.pnlProp.Controls.Add(this.separatorControl1);
            this.pnlProp.Controls.Add(this.btnClose);
            this.pnlProp.Controls.Add(this.groupBox1);
            this.pnlProp.Controls.Add(this.btnSaveProb);
            this.pnlProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProp.Location = new System.Drawing.Point(0, 0);
            this.pnlProp.Name = "pnlProp";
            this.pnlProp.Size = new System.Drawing.Size(455, 280);
            this.pnlProp.TabIndex = 2;
            // 
            // separatorControl1
            // 
            this.separatorControl1.Location = new System.Drawing.Point(12, 189);
            this.separatorControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.separatorControl1.Name = "separatorControl1";
            this.separatorControl1.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.separatorControl1.Size = new System.Drawing.Size(433, 30);
            this.separatorControl1.TabIndex = 29;
            // 
            // btnClose
            // 
            this.btnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnClose.ImageOptions.SvgImage")));
            this.btnClose.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnClose.Location = new System.Drawing.Point(228, 219);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 22);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtGroupName);
            this.groupBox1.Controls.Add(this.lblEnquirerID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtActivity);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 153);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HR Method Groups";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(148, 86);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(251, 24);
            this.txtGroupName.TabIndex = 42;
            // 
            // lblEnquirerID
            // 
            this.lblEnquirerID.AutoSize = true;
            this.lblEnquirerID.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.lblEnquirerID.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblEnquirerID.Location = new System.Drawing.Point(38, 54);
            this.lblEnquirerID.Name = "lblEnquirerID";
            this.lblEnquirerID.Size = new System.Drawing.Size(53, 17);
            this.lblEnquirerID.TabIndex = 41;
            this.lblEnquirerID.Text = "Activity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(38, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Group Name";
            // 
            // txtActivity
            // 
            this.txtActivity.Location = new System.Drawing.Point(148, 49);
            this.txtActivity.Name = "txtActivity";
            this.txtActivity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtActivity.Size = new System.Drawing.Size(251, 24);
            this.txtActivity.TabIndex = 37;
            // 
            // btnSaveProb
            // 
            this.btnSaveProb.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.btnSaveProb.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSaveProb.Location = new System.Drawing.Point(111, 219);
            this.btnSaveProb.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveProb.Name = "btnSaveProb";
            this.btnSaveProb.Size = new System.Drawing.Size(87, 22);
            this.btnSaveProb.TabIndex = 27;
            this.btnSaveProb.Text = "Save";
            this.btnSaveProb.Click += new System.EventHandler(this.btnSaveProb_Click_1);
            // 
            // frmHR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 280);
            this.Controls.Add(this.pnlProp);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmHR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Problem Properties";
            this.Load += new System.EventHandler(this.frmProblemProperties_Load);
            this.pnlProp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtActivity.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlProp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SeparatorControl separatorControl1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnSaveProb;
        private System.Windows.Forms.Label lblEnquirerID;
        public DevExpress.XtraEditors.ComboBoxEdit txtActivity;
        public DevExpress.XtraEditors.TextEdit txtGroupName;
    }
}