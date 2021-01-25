namespace Mineware.Systems.Production.Logistics_Management
{
    partial class frmSDBAddAuthorise
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
            this.lbxMainAct = new System.Windows.Forms.ListBox();
            this.lbxWP = new System.Windows.Forms.ListBox();
            this.lbxCrew = new System.Windows.Forms.ListBox();
            this.lbxMiner = new System.Windows.Forms.ListBox();
            this.DPStartdate = new System.Windows.Forms.DateTimePicker();
            this.DPEnddate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FilterActtxt = new System.Windows.Forms.TextBox();
            this.FilterTasktxt = new System.Windows.Forms.TextBox();
            this.FilterCrewtxt = new System.Windows.Forms.TextBox();
            this.FilterMinertxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave2 = new DevExpress.XtraEditors.SimpleButton();
            this.separatorButtons = new DevExpress.XtraEditors.SeparatorControl();
            this.DPOrigdate = new System.Windows.Forms.DateTimePicker();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // lbxMainAct
            // 
            this.lbxMainAct.FormattingEnabled = true;
            this.lbxMainAct.ItemHeight = 17;
            this.lbxMainAct.Location = new System.Drawing.Point(12, 47);
            this.lbxMainAct.Name = "lbxMainAct";
            this.lbxMainAct.Size = new System.Drawing.Size(338, 259);
            this.lbxMainAct.TabIndex = 0;
            this.lbxMainAct.SelectedIndexChanged += new System.EventHandler(this.lbxMainAct_SelectedIndexChanged);
            // 
            // lbxWP
            // 
            this.lbxWP.FormattingEnabled = true;
            this.lbxWP.ItemHeight = 17;
            this.lbxWP.Location = new System.Drawing.Point(374, 47);
            this.lbxWP.Name = "lbxWP";
            this.lbxWP.Size = new System.Drawing.Size(278, 259);
            this.lbxWP.TabIndex = 1;
            // 
            // lbxCrew
            // 
            this.lbxCrew.FormattingEnabled = true;
            this.lbxCrew.ItemHeight = 17;
            this.lbxCrew.Location = new System.Drawing.Point(671, 47);
            this.lbxCrew.Name = "lbxCrew";
            this.lbxCrew.Size = new System.Drawing.Size(367, 259);
            this.lbxCrew.TabIndex = 2;
            // 
            // lbxMiner
            // 
            this.lbxMiner.FormattingEnabled = true;
            this.lbxMiner.ItemHeight = 17;
            this.lbxMiner.Location = new System.Drawing.Point(1280, 215);
            this.lbxMiner.Name = "lbxMiner";
            this.lbxMiner.Size = new System.Drawing.Size(99, 72);
            this.lbxMiner.TabIndex = 3;
            // 
            // DPStartdate
            // 
            this.DPStartdate.CustomFormat = "dd MMM yyyy";
            this.DPStartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DPStartdate.Location = new System.Drawing.Point(1057, 36);
            this.DPStartdate.Name = "DPStartdate";
            this.DPStartdate.Size = new System.Drawing.Size(127, 25);
            this.DPStartdate.TabIndex = 4;
            this.DPStartdate.CloseUp += new System.EventHandler(this.DPStartdate_CloseUp);
            this.DPStartdate.ValueChanged += new System.EventHandler(this.DPStartdate_ValueChanged);
            // 
            // DPEnddate
            // 
            this.DPEnddate.CustomFormat = "dd MMM yyyy";
            this.DPEnddate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DPEnddate.Location = new System.Drawing.Point(1057, 87);
            this.DPEnddate.Name = "DPEnddate";
            this.DPEnddate.Size = new System.Drawing.Size(127, 25);
            this.DPEnddate.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "Activties Filter :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(371, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "WP Filter :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(668, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Crew Filter :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1277, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 19);
            this.label4.TabIndex = 9;
            this.label4.Text = "Miner Filter :";
            // 
            // FilterActtxt
            // 
            this.FilterActtxt.Location = new System.Drawing.Point(131, 13);
            this.FilterActtxt.Name = "FilterActtxt";
            this.FilterActtxt.Size = new System.Drawing.Size(219, 25);
            this.FilterActtxt.TabIndex = 10;
            this.FilterActtxt.TextChanged += new System.EventHandler(this.FilterActtxt_TextChanged);
            // 
            // FilterTasktxt
            // 
            this.FilterTasktxt.Location = new System.Drawing.Point(463, 13);
            this.FilterTasktxt.Name = "FilterTasktxt";
            this.FilterTasktxt.Size = new System.Drawing.Size(186, 25);
            this.FilterTasktxt.TabIndex = 11;
            this.FilterTasktxt.TextChanged += new System.EventHandler(this.FilterTasktxt_TextChanged);
            // 
            // FilterCrewtxt
            // 
            this.FilterCrewtxt.Location = new System.Drawing.Point(769, 13);
            this.FilterCrewtxt.Name = "FilterCrewtxt";
            this.FilterCrewtxt.Size = new System.Drawing.Size(266, 25);
            this.FilterCrewtxt.TabIndex = 12;
            this.FilterCrewtxt.TextChanged += new System.EventHandler(this.FilterCrewtxt_TextChanged);
            // 
            // FilterMinertxt
            // 
            this.FilterMinertxt.Location = new System.Drawing.Point(1298, 244);
            this.FilterMinertxt.Name = "FilterMinertxt";
            this.FilterMinertxt.Size = new System.Drawing.Size(41, 25);
            this.FilterMinertxt.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1054, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 19);
            this.label5.TabIndex = 14;
            this.label5.Text = "Start Date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1054, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 19);
            this.label6.TabIndex = 15;
            this.label6.Text = "End Date";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel2);
            this.pnlButtons.Controls.Add(this.btnSave2);
            this.pnlButtons.Location = new System.Drawing.Point(463, 354);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(301, 35);
            this.pnlButtons.TabIndex = 16;
            // 
            // btnCancel2
            // 
            this.btnCancel2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.close;
            this.btnCancel2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnCancel2.Location = new System.Drawing.Point(161, 5);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(94, 26);
            this.btnCancel2.TabIndex = 40;
            this.btnCancel2.Text = "Cancel";
            this.btnCancel2.Click += new System.EventHandler(this.btnCancel2_Click);
            // 
            // btnSave2
            // 
            this.btnSave2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.btnSave2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSave2.Location = new System.Drawing.Point(44, 4);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(94, 26);
            this.btnSave2.TabIndex = 39;
            this.btnSave2.Text = "Save";
            this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
            // 
            // separatorButtons
            // 
            this.separatorButtons.Location = new System.Drawing.Point(25, 316);
            this.separatorButtons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.separatorButtons.Name = "separatorButtons";
            this.separatorButtons.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.separatorButtons.Size = new System.Drawing.Size(1035, 31);
            this.separatorButtons.TabIndex = 17;
            // 
            // DPOrigdate
            // 
            this.DPOrigdate.CustomFormat = "dd MMM yyyy";
            this.DPOrigdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DPOrigdate.Location = new System.Drawing.Point(1057, 269);
            this.DPOrigdate.Name = "DPOrigdate";
            this.DPOrigdate.Size = new System.Drawing.Size(127, 25);
            this.DPOrigdate.TabIndex = 18;
            this.DPOrigdate.Visible = false;
            // 
            // frmSDBAddAuthorise
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 401);
            this.Controls.Add(this.DPOrigdate);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.separatorButtons);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FilterMinertxt);
            this.Controls.Add(this.FilterCrewtxt);
            this.Controls.Add(this.FilterTasktxt);
            this.Controls.Add(this.FilterActtxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DPEnddate);
            this.Controls.Add(this.DPStartdate);
            this.Controls.Add(this.lbxMiner);
            this.Controls.Add(this.lbxCrew);
            this.Controls.Add(this.lbxWP);
            this.Controls.Add(this.lbxMainAct);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmSDBAddAuthorise";
            this.Text = "Add";
            this.Load += new System.EventHandler(this.frmSDBAddAuthorise_Load);
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbxMainAct;
        private System.Windows.Forms.ListBox lbxWP;
        private System.Windows.Forms.ListBox lbxCrew;
        private System.Windows.Forms.ListBox lbxMiner;
        public System.Windows.Forms.DateTimePicker DPStartdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox FilterActtxt;
        private System.Windows.Forms.TextBox FilterTasktxt;
        private System.Windows.Forms.TextBox FilterCrewtxt;
        private System.Windows.Forms.TextBox FilterMinertxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnlButtons;
        private DevExpress.XtraEditors.SeparatorControl separatorButtons;
        public System.Windows.Forms.DateTimePicker DPOrigdate;
        public System.Windows.Forms.DateTimePicker DPEnddate;
        private DevExpress.XtraEditors.SimpleButton btnCancel2;
        private DevExpress.XtraEditors.SimpleButton btnSave2;
    }
}