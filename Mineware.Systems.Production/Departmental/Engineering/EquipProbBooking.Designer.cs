namespace Mineware.Systems.ProductionAmplats.Departmental.Engineering
{
    partial class EquipProbBooking
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
            this.ShiftDatelbl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbReportto = new System.Windows.Forms.ComboBox();
            this.cmbReportby = new System.Windows.Forms.ComboBox();
            this.BRTimetxt = new DevExpress.XtraEditors.TextEdit();
            this.BRDate = new System.Windows.Forms.DateTimePicker();
            this.EquipLbl = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbLocation2 = new System.Windows.Forms.ComboBox();
            this.cmbLocation1 = new System.Windows.Forms.ComboBox();
            this.Faulttxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CloseBtn = new DevExpress.XtraEditors.SimpleButton();
            this.SaveBtn = new DevExpress.XtraEditors.SimpleButton();
            this.ShiftLbl = new System.Windows.Forms.Label();
            this.cbxDamaged = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Durtxt = new System.Windows.Forms.TextBox();
            this.Numerictxt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.EndTimetxt = new DevExpress.XtraEditors.TextEdit();
            this.EndDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.StartTimetxt = new DevExpress.XtraEditors.TextEdit();
            this.StartDate = new System.Windows.Forms.DateTimePicker();
            this.Maximotxt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TMRemarkstxt = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BRTimetxt.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EndTimetxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTimetxt.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ShiftDatelbl
            // 
            this.ShiftDatelbl.AutoSize = true;
            this.ShiftDatelbl.Location = new System.Drawing.Point(522, 9);
            this.ShiftDatelbl.Name = "ShiftDatelbl";
            this.ShiftDatelbl.Size = new System.Drawing.Size(35, 13);
            this.ShiftDatelbl.TabIndex = 0;
            this.ShiftDatelbl.Text = "label1";
            this.ShiftDatelbl.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbReportto);
            this.groupBox1.Controls.Add(this.cmbReportby);
            this.groupBox1.Controls.Add(this.BRTimetxt);
            this.groupBox1.Controls.Add(this.BRDate);
            this.groupBox1.Location = new System.Drawing.Point(494, 282);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(29, 10);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Breakdown Report";
            this.groupBox1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 87;
            this.label2.Text = "Reported to:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Reported by:";
            // 
            // cmbReportto
            // 
            this.cmbReportto.FormattingEnabled = true;
            this.cmbReportto.Location = new System.Drawing.Point(6, 107);
            this.cmbReportto.Name = "cmbReportto";
            this.cmbReportto.Size = new System.Drawing.Size(174, 21);
            this.cmbReportto.TabIndex = 86;
            // 
            // cmbReportby
            // 
            this.cmbReportby.FormattingEnabled = true;
            this.cmbReportby.Location = new System.Drawing.Point(6, 69);
            this.cmbReportby.Name = "cmbReportby";
            this.cmbReportby.Size = new System.Drawing.Size(174, 21);
            this.cmbReportby.TabIndex = 2;
            // 
            // BRTimetxt
            // 
            this.BRTimetxt.EditValue = "00:00";
            this.BRTimetxt.Location = new System.Drawing.Point(115, 19);
            this.BRTimetxt.Name = "BRTimetxt";
            this.BRTimetxt.Properties.Mask.EditMask = "t";
            this.BRTimetxt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            this.BRTimetxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.BRTimetxt.Size = new System.Drawing.Size(36, 20);
            this.BRTimetxt.TabIndex = 85;
            // 
            // BRDate
            // 
            this.BRDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.BRDate.Location = new System.Drawing.Point(9, 19);
            this.BRDate.Name = "BRDate";
            this.BRDate.Size = new System.Drawing.Size(87, 20);
            this.BRDate.TabIndex = 2;
            // 
            // EquipLbl
            // 
            this.EquipLbl.AutoSize = true;
            this.EquipLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EquipLbl.Location = new System.Drawing.Point(12, 9);
            this.EquipLbl.Name = "EquipLbl";
            this.EquipLbl.Size = new System.Drawing.Size(52, 18);
            this.EquipLbl.TabIndex = 2;
            this.EquipLbl.Text = "label3";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbLocation2);
            this.groupBox2.Controls.Add(this.cmbLocation1);
            this.groupBox2.Controls.Add(this.Faulttxt);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cmbType);
            this.groupBox2.Location = new System.Drawing.Point(12, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 171);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 94;
            this.label5.Text = "Location 2:";
            this.label5.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 92;
            this.label6.Text = "Location 1:";
            this.label6.Visible = false;
            // 
            // cmbLocation2
            // 
            this.cmbLocation2.FormattingEnabled = true;
            this.cmbLocation2.Location = new System.Drawing.Point(13, 226);
            this.cmbLocation2.Name = "cmbLocation2";
            this.cmbLocation2.Size = new System.Drawing.Size(174, 21);
            this.cmbLocation2.TabIndex = 93;
            this.cmbLocation2.Visible = false;
            // 
            // cmbLocation1
            // 
            this.cmbLocation1.FormattingEnabled = true;
            this.cmbLocation1.Location = new System.Drawing.Point(13, 188);
            this.cmbLocation1.Name = "cmbLocation1";
            this.cmbLocation1.Size = new System.Drawing.Size(174, 21);
            this.cmbLocation1.TabIndex = 91;
            this.cmbLocation1.Visible = false;
            // 
            // Faulttxt
            // 
            this.Faulttxt.Location = new System.Drawing.Point(12, 93);
            this.Faulttxt.Multiline = true;
            this.Faulttxt.Name = "Faulttxt";
            this.Faulttxt.Size = new System.Drawing.Size(191, 63);
            this.Faulttxt.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 90;
            this.label4.Text = "Fault Reported:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 89;
            this.label3.Text = "Type:";
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(9, 38);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(194, 21);
            this.cmbType.TabIndex = 88;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.CloseBtn);
            this.panel1.Controls.Add(this.SaveBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 403);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(561, 88);
            this.panel1.TabIndex = 4;
            // 
            // CloseBtn
            // 
            this.CloseBtn.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CloseBtn.Appearance.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.CloseBtn.Appearance.Options.UseBackColor = true;
            this.CloseBtn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            //this.CloseBtn.Image = global::PAS.Properties.Resources.Close;
            this.CloseBtn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.CloseBtn.Location = new System.Drawing.Point(329, 13);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(136, 48);
            this.CloseBtn.TabIndex = 36;
            this.CloseBtn.Text = "&Close                 ";
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.SaveBtn.Appearance.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.SaveBtn.Appearance.Options.UseBackColor = true;
            this.SaveBtn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            //this.SaveBtn.Image = global::PAS.Properties.Resources.Save;
            this.SaveBtn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.SaveBtn.Location = new System.Drawing.Point(106, 13);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(136, 48);
            this.SaveBtn.TabIndex = 35;
            this.SaveBtn.Text = "&Save                 ";
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // ShiftLbl
            // 
            this.ShiftLbl.AutoSize = true;
            this.ShiftLbl.Location = new System.Drawing.Point(462, 9);
            this.ShiftLbl.Name = "ShiftLbl";
            this.ShiftLbl.Size = new System.Drawing.Size(35, 13);
            this.ShiftLbl.TabIndex = 5;
            this.ShiftLbl.Text = "label1";
            this.ShiftLbl.Visible = false;
            // 
            // cbxDamaged
            // 
            this.cbxDamaged.AutoSize = true;
            this.cbxDamaged.Location = new System.Drawing.Point(300, 31);
            this.cbxDamaged.Name = "cbxDamaged";
            this.cbxDamaged.Size = new System.Drawing.Size(72, 17);
            this.cbxDamaged.TabIndex = 6;
            this.cbxDamaged.Text = "Damaged";
            this.cbxDamaged.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(300, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 89;
            this.label7.Text = "Status";
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(300, 81);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(174, 21);
            this.cmbStatus.TabIndex = 88;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Durtxt);
            this.groupBox3.Controls.Add(this.Numerictxt);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.EndTimetxt);
            this.groupBox3.Controls.Add(this.EndDate);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.StartTimetxt);
            this.groupBox3.Controls.Add(this.StartDate);
            this.groupBox3.Location = new System.Drawing.Point(300, 119);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(257, 142);
            this.groupBox3.TabIndex = 88;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Delay";
            // 
            // Durtxt
            // 
            this.Durtxt.Location = new System.Drawing.Point(101, 116);
            this.Durtxt.Name = "Durtxt";
            this.Durtxt.Size = new System.Drawing.Size(73, 20);
            this.Durtxt.TabIndex = 98;
            this.Durtxt.Visible = false;
            // 
            // Numerictxt
            // 
            this.Numerictxt.Location = new System.Drawing.Point(3, 116);
            this.Numerictxt.Name = "Numerictxt";
            this.Numerictxt.Size = new System.Drawing.Size(73, 20);
            this.Numerictxt.TabIndex = 97;
            this.Numerictxt.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 93;
            this.label9.Text = "End Date:";
            // 
            // EndTimetxt
            // 
            this.EndTimetxt.EditValue = "00:00";
            this.EndTimetxt.Location = new System.Drawing.Point(144, 85);
            this.EndTimetxt.Name = "EndTimetxt";
            this.EndTimetxt.Properties.Mask.EditMask = "t";
            this.EndTimetxt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            this.EndTimetxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.EndTimetxt.Size = new System.Drawing.Size(36, 20);
            this.EndTimetxt.TabIndex = 92;
            // 
            // EndDate
            // 
            this.EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.EndDate.Location = new System.Drawing.Point(6, 85);
            this.EndDate.Name = "EndDate";
            this.EndDate.Size = new System.Drawing.Size(121, 20);
            this.EndDate.TabIndex = 91;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 90;
            this.label8.Text = "Start Date:";
            // 
            // StartTimetxt
            // 
            this.StartTimetxt.EditValue = "00:00";
            this.StartTimetxt.Location = new System.Drawing.Point(144, 37);
            this.StartTimetxt.Name = "StartTimetxt";
            this.StartTimetxt.Properties.Mask.EditMask = "t";
            this.StartTimetxt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            this.StartTimetxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartTimetxt.Size = new System.Drawing.Size(36, 20);
            this.StartTimetxt.TabIndex = 85;
            // 
            // StartDate
            // 
            this.StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.StartDate.Location = new System.Drawing.Point(6, 37);
            this.StartDate.Name = "StartDate";
            this.StartDate.Size = new System.Drawing.Size(121, 20);
            this.StartDate.TabIndex = 2;
            // 
            // Maximotxt
            // 
            this.Maximotxt.Location = new System.Drawing.Point(21, 242);
            this.Maximotxt.Name = "Maximotxt";
            this.Maximotxt.Size = new System.Drawing.Size(111, 20);
            this.Maximotxt.TabIndex = 98;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 226);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 13);
            this.label10.TabIndex = 97;
            this.label10.Text = "Maximo No:";
            // 
            // TMRemarkstxt
            // 
            this.TMRemarkstxt.Location = new System.Drawing.Point(18, 295);
            this.TMRemarkstxt.Multiline = true;
            this.TMRemarkstxt.Name = "TMRemarkstxt";
            this.TMRemarkstxt.Size = new System.Drawing.Size(220, 63);
            this.TMRemarkstxt.TabIndex = 95;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 279);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 13);
            this.label11.TabIndex = 96;
            this.label11.Text = "TM3 Remarks:";
            // 
            // EquipProbBooking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 491);
            this.Controls.Add(this.TMRemarkstxt);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Maximotxt);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbxDamaged);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.ShiftLbl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.EquipLbl);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ShiftDatelbl);
            this.Name = "EquipProbBooking";
            this.Text = "EquipProbBooking";
            this.Load += new System.EventHandler(this.EquipProbBooking_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BRTimetxt.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EndTimetxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTimetxt.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker BRDate;
        public DevExpress.XtraEditors.TextEdit BRTimetxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbLocation2;
        private System.Windows.Forms.ComboBox cmbLocation1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton CloseBtn;
        private DevExpress.XtraEditors.SimpleButton SaveBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        public DevExpress.XtraEditors.TextEdit EndTimetxt;
        private System.Windows.Forms.Label label8;
        public DevExpress.XtraEditors.TextEdit StartTimetxt;
        private System.Windows.Forms.TextBox Numerictxt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.ComboBox cmbReportto;
        public System.Windows.Forms.ComboBox cmbReportby;
        public System.Windows.Forms.ComboBox cmbType;
        public System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox Durtxt;
        public System.Windows.Forms.DateTimePicker EndDate;
        public System.Windows.Forms.DateTimePicker StartDate;
        public System.Windows.Forms.Label EquipLbl;
        public System.Windows.Forms.Label ShiftLbl;
        public System.Windows.Forms.TextBox Faulttxt;
        public System.Windows.Forms.CheckBox cbxDamaged;
        public System.Windows.Forms.ComboBox cmbStatus;
        public System.Windows.Forms.TextBox Maximotxt;
        public System.Windows.Forms.TextBox TMRemarkstxt;
        public System.Windows.Forms.Label ShiftDatelbl;
    }
}