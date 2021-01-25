namespace Mineware.Systems.Production.Logistics_Management
{
    partial class frmAddActivity
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
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.separatorButtons = new DevExpress.XtraEditors.SeparatorControl();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHide1 = new System.Windows.Forms.Label();
            this.lblHide2 = new System.Windows.Forms.Label();
            this.lblHide3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Descriptiontxt = new System.Windows.Forms.TextBox();
            this.spinEditDuration = new DevExpress.XtraEditors.SpinEdit();
            this.cmbMeasureType = new System.Windows.Forms.ComboBox();
            this.cmbImpact = new System.Windows.Forms.ComboBox();
            this.Amounttxt = new DevExpress.XtraEditors.TextEdit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditDuration.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amounttxt.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.simpleButton1);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.simpleButton2);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Location = new System.Drawing.Point(99, 371);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(301, 35);
            this.pnlButtons.TabIndex = 20;
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.CloseRed;
            this.simpleButton1.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.simpleButton1.Location = new System.Drawing.Point(168, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(94, 26);
            this.simpleButton1.TabIndex = 36;
            this.simpleButton1.Text = "Cancel";
            this.simpleButton1.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 2);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(16, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.simpleButton2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.simpleButton2.Location = new System.Drawing.Point(50, 3);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(94, 26);
            this.simpleButton2.TabIndex = 35;
            this.simpleButton2.Text = "Save";
            this.simpleButton2.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(180, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(13, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // separatorButtons
            // 
            this.separatorButtons.Location = new System.Drawing.Point(50, 335);
            this.separatorButtons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.separatorButtons.Name = "separatorButtons";
            this.separatorButtons.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.separatorButtons.Size = new System.Drawing.Size(435, 31);
            this.separatorButtons.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 19);
            this.label1.TabIndex = 22;
            this.label1.Text = "Description :";
            // 
            // lblHide1
            // 
            this.lblHide1.AutoSize = true;
            this.lblHide1.Location = new System.Drawing.Point(93, 101);
            this.lblHide1.Name = "lblHide1";
            this.lblHide1.Size = new System.Drawing.Size(70, 19);
            this.lblHide1.TabIndex = 23;
            this.lblHide1.Text = "Duration :";
            // 
            // lblHide2
            // 
            this.lblHide2.AutoSize = true;
            this.lblHide2.Location = new System.Drawing.Point(25, 159);
            this.lblHide2.Name = "lblHide2";
            this.lblHide2.Size = new System.Drawing.Size(137, 19);
            this.lblHide2.TabIndex = 24;
            this.lblHide2.Text = "Measurement Type  :";
            // 
            // lblHide3
            // 
            this.lblHide3.AutoSize = true;
            this.lblHide3.Location = new System.Drawing.Point(97, 219);
            this.lblHide3.Name = "lblHide3";
            this.lblHide3.Size = new System.Drawing.Size(66, 19);
            this.lblHide3.TabIndex = 25;
            this.lblHide3.Text = "Amount :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(99, 272);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 19);
            this.label6.TabIndex = 27;
            this.label6.Text = "Impact  :";
            this.label6.Visible = false;
            // 
            // Descriptiontxt
            // 
            this.Descriptiontxt.Location = new System.Drawing.Point(171, 42);
            this.Descriptiontxt.Name = "Descriptiontxt";
            this.Descriptiontxt.Size = new System.Drawing.Size(285, 25);
            this.Descriptiontxt.TabIndex = 28;
            // 
            // spinEditDuration
            // 
            this.spinEditDuration.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditDuration.Location = new System.Drawing.Point(171, 97);
            this.spinEditDuration.Name = "spinEditDuration";
            this.spinEditDuration.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditDuration.Properties.MaxValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.spinEditDuration.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditDuration.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.spinEditDuration.Size = new System.Drawing.Size(44, 24);
            this.spinEditDuration.TabIndex = 29;
            // 
            // cmbMeasureType
            // 
            this.cmbMeasureType.FormattingEnabled = true;
            this.cmbMeasureType.Location = new System.Drawing.Point(171, 156);
            this.cmbMeasureType.Name = "cmbMeasureType";
            this.cmbMeasureType.Size = new System.Drawing.Size(166, 25);
            this.cmbMeasureType.TabIndex = 30;
            // 
            // cmbImpact
            // 
            this.cmbImpact.FormattingEnabled = true;
            this.cmbImpact.Location = new System.Drawing.Point(171, 269);
            this.cmbImpact.Name = "cmbImpact";
            this.cmbImpact.Size = new System.Drawing.Size(121, 25);
            this.cmbImpact.TabIndex = 33;
            this.cmbImpact.Visible = false;
            // 
            // Amounttxt
            // 
            this.Amounttxt.EditValue = "0.00";
            this.Amounttxt.Location = new System.Drawing.Point(171, 216);
            this.Amounttxt.Name = "Amounttxt";
            this.Amounttxt.Properties.Mask.EditMask = "N2";
            this.Amounttxt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.Amounttxt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.Amounttxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Amounttxt.Size = new System.Drawing.Size(50, 24);
            this.Amounttxt.TabIndex = 34;
            // 
            // frmAddActivity
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 416);
            this.Controls.Add(this.Amounttxt);
            this.Controls.Add(this.cmbImpact);
            this.Controls.Add(this.cmbMeasureType);
            this.Controls.Add(this.spinEditDuration);
            this.Controls.Add(this.Descriptiontxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblHide3);
            this.Controls.Add(this.lblHide2);
            this.Controls.Add(this.lblHide1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.separatorButtons);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmAddActivity";
            this.Text = "Activity";
            this.Load += new System.EventHandler(this.frmAddActivity_Load);
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditDuration.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Amounttxt.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHide1;
        private System.Windows.Forms.Label lblHide2;
        private System.Windows.Forms.Label lblHide3;
        private System.Windows.Forms.Label label6;
        public DevExpress.XtraEditors.TextEdit Amounttxt;
        public System.Windows.Forms.TextBox Descriptiontxt;
        public DevExpress.XtraEditors.SpinEdit spinEditDuration;
        public System.Windows.Forms.ComboBox cmbImpact;
        public System.Windows.Forms.ComboBox cmbMeasureType;
        public System.Windows.Forms.Panel pnlButtons;
        public DevExpress.XtraEditors.SeparatorControl separatorButtons;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}