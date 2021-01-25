namespace Mineware.Systems.Production.SysAdmin
{
    partial class frmEquipmentProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEquipmentProperties));
            this.pnlProp = new System.Windows.Forms.Panel();
            this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEquipmentMake = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblEnquirerID = new System.Windows.Forms.Label();
            this.txtEquipmentName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radNo = new System.Windows.Forms.RadioButton();
            this.radYes = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEquipmentID = new DevExpress.XtraEditors.TextEdit();
            this.lblPorbID = new System.Windows.Forms.Label();
            this.txtEquipmentType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnSaveProb = new DevExpress.XtraEditors.SimpleButton();
            this.pnlProp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipmentMake.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipmentID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipmentType.Properties)).BeginInit();
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
            this.pnlProp.Size = new System.Drawing.Size(455, 422);
            this.pnlProp.TabIndex = 2;
            // 
            // separatorControl1
            // 
            this.separatorControl1.Location = new System.Drawing.Point(12, 342);
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
            this.btnClose.Location = new System.Drawing.Point(233, 377);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 25);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "Close  ";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtEquipmentMake);
            this.groupBox1.Controls.Add(this.lblEnquirerID);
            this.groupBox1.Controls.Add(this.txtEquipmentName);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtEquipmentID);
            this.groupBox1.Controls.Add(this.lblPorbID);
            this.groupBox1.Controls.Add(this.txtEquipmentType);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 8F);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 315);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Equipment Setup";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(40, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 19);
            this.label1.TabIndex = 43;
            this.label1.Text = "Equipment Make";
            // 
            // txtEquipmentMake
            // 
            this.txtEquipmentMake.Location = new System.Drawing.Point(164, 178);
            this.txtEquipmentMake.Name = "txtEquipmentMake";
            this.txtEquipmentMake.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtEquipmentMake.Properties.Items.AddRange(new object[] {
            "Aard",
            "Catvan",
            "GHH",
            "Sandvik",
            "Toyota"});
            this.txtEquipmentMake.Size = new System.Drawing.Size(234, 24);
            this.txtEquipmentMake.TabIndex = 42;
            // 
            // lblEnquirerID
            // 
            this.lblEnquirerID.AutoSize = true;
            this.lblEnquirerID.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblEnquirerID.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblEnquirerID.Location = new System.Drawing.Point(40, 143);
            this.lblEnquirerID.Name = "lblEnquirerID";
            this.lblEnquirerID.Size = new System.Drawing.Size(107, 19);
            this.lblEnquirerID.TabIndex = 41;
            this.lblEnquirerID.Text = "Equipment Type";
            // 
            // txtEquipmentName
            // 
            this.txtEquipmentName.Location = new System.Drawing.Point(42, 98);
            this.txtEquipmentName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEquipmentName.MaxLength = 250;
            this.txtEquipmentName.Multiline = true;
            this.txtEquipmentName.Name = "txtEquipmentName";
            this.txtEquipmentName.Size = new System.Drawing.Size(357, 25);
            this.txtEquipmentName.TabIndex = 36;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radNo);
            this.groupBox2.Controls.Add(this.radYes);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.groupBox2.Location = new System.Drawing.Point(44, 229);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(358, 58);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Valid";
            // 
            // radNo
            // 
            this.radNo.AutoSize = true;
            this.radNo.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.radNo.Location = new System.Drawing.Point(195, 18);
            this.radNo.Name = "radNo";
            this.radNo.Size = new System.Drawing.Size(48, 23);
            this.radNo.TabIndex = 1;
            this.radNo.TabStop = true;
            this.radNo.Text = "No";
            this.radNo.UseVisualStyleBackColor = true;
            this.radNo.CheckedChanged += new System.EventHandler(this.radNo_CheckedChanged);
            // 
            // radYes
            // 
            this.radYes.AutoSize = true;
            this.radYes.Checked = true;
            this.radYes.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.radYes.Location = new System.Drawing.Point(90, 18);
            this.radYes.Name = "radYes";
            this.radYes.Size = new System.Drawing.Size(50, 23);
            this.radYes.TabIndex = 0;
            this.radYes.TabStop = true;
            this.radYes.Text = "Yes";
            this.radYes.UseVisualStyleBackColor = true;
            this.radYes.CheckedChanged += new System.EventHandler(this.radYes_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(38, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 19);
            this.label6.TabIndex = 7;
            this.label6.Text = "Equipment Name";
            // 
            // txtEquipmentID
            // 
            this.txtEquipmentID.Location = new System.Drawing.Point(164, 44);
            this.txtEquipmentID.Name = "txtEquipmentID";
            this.txtEquipmentID.Size = new System.Drawing.Size(234, 24);
            this.txtEquipmentID.TabIndex = 1;
            // 
            // lblPorbID
            // 
            this.lblPorbID.AutoSize = true;
            this.lblPorbID.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblPorbID.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPorbID.Location = new System.Drawing.Point(38, 47);
            this.lblPorbID.Name = "lblPorbID";
            this.lblPorbID.Size = new System.Drawing.Size(93, 19);
            this.lblPorbID.TabIndex = 0;
            this.lblPorbID.Text = "Equipment ID";
            // 
            // txtEquipmentType
            // 
            this.txtEquipmentType.Location = new System.Drawing.Point(164, 139);
            this.txtEquipmentType.Name = "txtEquipmentType";
            this.txtEquipmentType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtEquipmentType.Properties.Items.AddRange(new object[] {
            "Loco",
            "Drill Rig",
            "Rasiebore",
            "Bob Cat",
            "Bolter",
            "Crane",
            "Grader",
            "LDV",
            "LHD",
            "Manitou",
            "Maubra"});
            this.txtEquipmentType.Size = new System.Drawing.Size(234, 24);
            this.txtEquipmentType.TabIndex = 37;
            // 
            // btnSaveProb
            // 
            this.btnSaveProb.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.btnSaveProb.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSaveProb.Location = new System.Drawing.Point(109, 377);
            this.btnSaveProb.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveProb.Name = "btnSaveProb";
            this.btnSaveProb.Size = new System.Drawing.Size(87, 25);
            this.btnSaveProb.TabIndex = 27;
            this.btnSaveProb.Text = "Save  ";
            this.btnSaveProb.Click += new System.EventHandler(this.btnSaveProb_Click_1);
            // 
            // frmEquipmentProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 422);
            this.Controls.Add(this.pnlProp);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmEquipmentProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Equipment Properties";
            this.Load += new System.EventHandler(this.frmProblemProperties_Load);
            this.pnlProp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipmentMake.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipmentID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEquipmentType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlProp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radNo;
        private System.Windows.Forms.RadioButton radYes;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtEquipmentID;
        private System.Windows.Forms.Label lblPorbID;
        private DevExpress.XtraEditors.SeparatorControl separatorControl1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnSaveProb;
        private System.Windows.Forms.TextBox txtEquipmentName;
        private DevExpress.XtraEditors.ComboBoxEdit txtEquipmentType;
        private System.Windows.Forms.Label lblEnquirerID;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit txtEquipmentMake;
    }
}