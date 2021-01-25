namespace Mineware.Systems.Production.Logistics_Management
{
    partial class frmScheduleActivity
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
            this.Filtertxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbxActivity = new System.Windows.Forms.ListBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.separatorButtons = new DevExpress.XtraEditors.SeparatorControl();
            this.spinEditOrder = new DevExpress.XtraEditors.SpinEdit();
            this.spinEditStartDay = new DevExpress.XtraEditors.SpinEdit();
            this.spinEditDuration = new DevExpress.XtraEditors.SpinEdit();
            this.cmbProceedsSubAct = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditOrder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditStartDay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditDuration.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Filtertxt
            // 
            this.Filtertxt.Location = new System.Drawing.Point(134, 34);
            this.Filtertxt.Name = "Filtertxt";
            this.Filtertxt.Size = new System.Drawing.Size(174, 25);
            this.Filtertxt.TabIndex = 13;
            this.Filtertxt.TextChanged += new System.EventHandler(this.Filtertxt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(29, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 19);
            this.label1.TabIndex = 12;
            this.label1.Text = "Tasks Filter :";
            // 
            // lbxActivity
            // 
            this.lbxActivity.FormattingEnabled = true;
            this.lbxActivity.ItemHeight = 17;
            this.lbxActivity.Location = new System.Drawing.Point(29, 68);
            this.lbxActivity.Name = "lbxActivity";
            this.lbxActivity.Size = new System.Drawing.Size(279, 259);
            this.lbxActivity.TabIndex = 11;
            this.lbxActivity.SelectedIndexChanged += new System.EventHandler(this.lbxActivity_SelectedIndexChanged);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.simpleButton1);
            this.pnlButtons.Controls.Add(this.simpleButton2);
            this.pnlButtons.Location = new System.Drawing.Point(241, 381);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(301, 35);
            this.pnlButtons.TabIndex = 18;
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.CloseRed;
            this.simpleButton1.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.simpleButton1.Location = new System.Drawing.Point(157, 5);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(94, 26);
            this.simpleButton1.TabIndex = 38;
            this.simpleButton1.Text = "Cancel";
            this.simpleButton1.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.simpleButton2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.simpleButton2.Location = new System.Drawing.Point(41, 5);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(94, 26);
            this.simpleButton2.TabIndex = 37;
            this.simpleButton2.Text = "Save";
            this.simpleButton2.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // separatorButtons
            // 
            this.separatorButtons.Location = new System.Drawing.Point(51, 343);
            this.separatorButtons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.separatorButtons.Name = "separatorButtons";
            this.separatorButtons.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.separatorButtons.Size = new System.Drawing.Size(660, 31);
            this.separatorButtons.TabIndex = 19;
            // 
            // spinEditOrder
            // 
            this.spinEditOrder.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditOrder.Location = new System.Drawing.Point(570, 246);
            this.spinEditOrder.Name = "spinEditOrder";
            this.spinEditOrder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditOrder.Properties.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.spinEditOrder.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditOrder.Size = new System.Drawing.Size(72, 24);
            this.spinEditOrder.TabIndex = 20;
            // 
            // spinEditStartDay
            // 
            this.spinEditStartDay.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditStartDay.Location = new System.Drawing.Point(439, 88);
            this.spinEditStartDay.Name = "spinEditStartDay";
            this.spinEditStartDay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditStartDay.Properties.MaxValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.spinEditStartDay.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditStartDay.Size = new System.Drawing.Size(72, 24);
            this.spinEditStartDay.TabIndex = 21;
            // 
            // spinEditDuration
            // 
            this.spinEditDuration.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEditDuration.Location = new System.Drawing.Point(439, 136);
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
            this.spinEditDuration.Size = new System.Drawing.Size(72, 24);
            this.spinEditDuration.TabIndex = 22;
            // 
            // cmbProceedsSubAct
            // 
            this.cmbProceedsSubAct.FormattingEnabled = true;
            this.cmbProceedsSubAct.Location = new System.Drawing.Point(439, 34);
            this.cmbProceedsSubAct.Name = "cmbProceedsSubAct";
            this.cmbProceedsSubAct.Size = new System.Drawing.Size(272, 25);
            this.cmbProceedsSubAct.TabIndex = 23;
            this.cmbProceedsSubAct.SelectedIndexChanged += new System.EventHandler(this.cmbProceedsSubAct_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(334, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 19);
            this.label2.TabIndex = 24;
            this.label2.Text = "Proceeds On :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(334, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 19);
            this.label3.TabIndex = 25;
            this.label3.Text = "Start Day :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(337, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 19);
            this.label4.TabIndex = 26;
            this.label4.Text = "Duration :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(513, 250);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 19);
            this.label5.TabIndex = 27;
            this.label5.Text = "Order :";
            // 
            // frmScheduleActivity
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 435);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbProceedsSubAct);
            this.Controls.Add(this.spinEditDuration);
            this.Controls.Add(this.spinEditStartDay);
            this.Controls.Add(this.spinEditOrder);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.separatorButtons);
            this.Controls.Add(this.Filtertxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbxActivity);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmScheduleActivity";
            this.Text = "Map Tasks";
            this.Load += new System.EventHandler(this.frmScheduleActivity_Load);
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditOrder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditStartDay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditDuration.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Filtertxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbxActivity;
        private System.Windows.Forms.Panel pnlButtons;
        private DevExpress.XtraEditors.SeparatorControl separatorButtons;
        private DevExpress.XtraEditors.SpinEdit spinEditOrder;
        private DevExpress.XtraEditors.SpinEdit spinEditStartDay;
        private DevExpress.XtraEditors.SpinEdit spinEditDuration;
        private System.Windows.Forms.ComboBox cmbProceedsSubAct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}