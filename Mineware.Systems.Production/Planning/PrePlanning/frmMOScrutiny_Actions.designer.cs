namespace Mineware.Systems.Production.Planning
{
	partial class frmMOScrutiny_Actions
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
            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.lcActImage = new DevExpress.XtraLayout.LayoutControl();
            this.btnAddImage = new DevExpress.XtraEditors.SimpleButton();
            this.lcgActImage = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgSubActImage = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ofdOpenImageFile = new System.Windows.Forms.OpenFileDialog();
            this.txtPathAttachment = new DevExpress.XtraEditors.TextEdit();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbxActions = new System.Windows.Forms.GroupBox();
            this.gbxPerson = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RespPersonCmb = new DevExpress.XtraEditors.LookUpEdit();
            this.OverseerCmb = new DevExpress.XtraEditors.LookUpEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.PriorityCmb = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReqDate = new DevExpress.XtraEditors.DateEdit();
            this.lblWP = new System.Windows.Forms.Label();
            this.gbxTop = new System.Windows.Forms.GroupBox();
            this.lblProdmonth = new DevExpress.XtraEditors.LabelControl();
            this.lblSection = new DevExpress.XtraEditors.LabelControl();
            this.lblWPDesc = new DevExpress.XtraEditors.LookUpEdit();
            this.gbxBottom = new System.Windows.Forms.GroupBox();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveAct = new DevExpress.XtraEditors.SimpleButton();
            this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcActImage)).BeginInit();
            this.lcActImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcgActImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgSubActImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPathAttachment.Properties)).BeginInit();
            this.gbxActions.SuspendLayout();
            this.gbxPerson.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RespPersonCmb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverseerCmb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityCmb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqDate.Properties)).BeginInit();
            this.gbxTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblWPDesc.Properties)).BeginInit();
            this.gbxBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // repositoryItemTextEdit3
            // 
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // lcActImage
            // 
            this.lcActImage.Controls.Add(this.btnAddImage);
            this.lcActImage.Controls.Add(this.PicBox);
            this.lcActImage.Dock = System.Windows.Forms.DockStyle.Right;
            this.lcActImage.Location = new System.Drawing.Point(623, 58);
            this.lcActImage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcActImage.Name = "lcActImage";
            this.lcActImage.Root = this.lcgActImage;
            this.lcActImage.Size = new System.Drawing.Size(320, 402);
            this.lcActImage.TabIndex = 4;
            this.lcActImage.Text = "layoutControl1";
            // 
            // btnAddImage
            // 
            this.btnAddImage.Appearance.BackColor = System.Drawing.Color.White;
            this.btnAddImage.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddImage.Appearance.Options.UseBackColor = true;
            this.btnAddImage.Appearance.Options.UseForeColor = true;
            this.btnAddImage.Appearance.Options.UseTextOptions = true;
            this.btnAddImage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.btnAddImage.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.btnAddImage.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.AddBlue2;
            this.btnAddImage.ImageOptions.SvgImageSize = new System.Drawing.Size(12, 12);
            this.btnAddImage.Location = new System.Drawing.Point(264, 1);
            this.btnAddImage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(53, 24);
            this.btnAddImage.StyleController = this.lcActImage;
            this.btnAddImage.TabIndex = 47;
            this.btnAddImage.Text = "Add";
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // lcgActImage
            // 
            this.lcgActImage.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgActImage.GroupBordersVisible = false;
            this.lcgActImage.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgSubActImage});
            this.lcgActImage.Name = "lcgActImage";
            this.lcgActImage.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgActImage.Size = new System.Drawing.Size(320, 402);
            this.lcgActImage.TextVisible = false;
            // 
            // lcgSubActImage
            // 
            this.lcgSubActImage.AppearanceGroup.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lcgSubActImage.AppearanceGroup.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lcgSubActImage.AppearanceGroup.Options.UseFont = true;
            this.lcgSubActImage.AppearanceGroup.Options.UseForeColor = true;
            this.lcgSubActImage.CaptionImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.CameraBlue;
            this.lcgSubActImage.CaptionImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.lcgSubActImage.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.lcgSubActImage.Location = new System.Drawing.Point(0, 0);
            this.lcgSubActImage.Name = "lcgSubActImage";
            this.lcgSubActImage.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgSubActImage.Size = new System.Drawing.Size(320, 402);
            this.lcgSubActImage.Text = "Image";
            // 
            // ofdOpenImageFile
            // 
            this.ofdOpenImageFile.FileName = "openFileDialog1";
            // 
            // txtPathAttachment
            // 
            this.txtPathAttachment.Location = new System.Drawing.Point(812, 25);
            this.txtPathAttachment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPathAttachment.Name = "txtPathAttachment";
            this.txtPathAttachment.Size = new System.Drawing.Size(117, 24);
            this.txtPathAttachment.TabIndex = 43;
            this.txtPathAttachment.Visible = false;
            // 
            // gbxActions
            // 
            this.gbxActions.Controls.Add(this.gbxPerson);
            this.gbxActions.Controls.Add(this.PriorityCmb);
            this.gbxActions.Controls.Add(this.label5);
            this.gbxActions.Controls.Add(this.txtRemarks);
            this.gbxActions.Controls.Add(this.label6);
            this.gbxActions.Controls.Add(this.label4);
            this.gbxActions.Controls.Add(this.txtReqDate);
            this.gbxActions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbxActions.Location = new System.Drawing.Point(10, 61);
            this.gbxActions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxActions.Name = "gbxActions";
            this.gbxActions.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxActions.Size = new System.Drawing.Size(595, 399);
            this.gbxActions.TabIndex = 44;
            this.gbxActions.TabStop = false;
            this.gbxActions.Text = "Problems";
            // 
            // gbxPerson
            // 
            this.gbxPerson.Controls.Add(this.label2);
            this.gbxPerson.Controls.Add(this.RespPersonCmb);
            this.gbxPerson.Controls.Add(this.OverseerCmb);
            this.gbxPerson.Controls.Add(this.label9);
            this.gbxPerson.Location = new System.Drawing.Point(11, 229);
            this.gbxPerson.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxPerson.Name = "gbxPerson";
            this.gbxPerson.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxPerson.Size = new System.Drawing.Size(565, 63);
            this.gbxPerson.TabIndex = 50;
            this.gbxPerson.TabStop = false;
            this.gbxPerson.Text = "Person";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 19);
            this.label2.TabIndex = 9;
            this.label2.Text = "Responsible Person";
            // 
            // RespPersonCmb
            // 
            this.RespPersonCmb.Location = new System.Drawing.Point(173, 27);
            this.RespPersonCmb.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RespPersonCmb.Name = "RespPersonCmb";
            this.RespPersonCmb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RespPersonCmb.Size = new System.Drawing.Size(155, 24);
            this.RespPersonCmb.TabIndex = 14;
            // 
            // OverseerCmb
            // 
            this.OverseerCmb.Location = new System.Drawing.Point(402, 27);
            this.OverseerCmb.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OverseerCmb.Name = "OverseerCmb";
            this.OverseerCmb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.OverseerCmb.Size = new System.Drawing.Size(155, 24);
            this.OverseerCmb.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(335, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 19);
            this.label9.TabIndex = 18;
            this.label9.Text = "Overseer";
            // 
            // PriorityCmb
            // 
            this.PriorityCmb.Location = new System.Drawing.Point(181, 316);
            this.PriorityCmb.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PriorityCmb.Name = "PriorityCmb";
            this.PriorityCmb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PriorityCmb.Size = new System.Drawing.Size(48, 24);
            this.PriorityCmb.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 319);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 19);
            this.label5.TabIndex = 10;
            this.label5.Text = "Priority";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(182, 37);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(384, 128);
            this.txtRemarks.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 19);
            this.label6.TabIndex = 5;
            this.label6.Text = "Required Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "Remarks";
            // 
            // txtReqDate
            // 
            this.txtReqDate.EditValue = null;
            this.txtReqDate.Location = new System.Drawing.Point(182, 184);
            this.txtReqDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtReqDate.Name = "txtReqDate";
            this.txtReqDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtReqDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtReqDate.Properties.Mask.EditMask = "ddd dd MMM yyyy";
            this.txtReqDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtReqDate.Size = new System.Drawing.Size(155, 24);
            this.txtReqDate.TabIndex = 49;
            // 
            // lblWP
            // 
            this.lblWP.AutoSize = true;
            this.lblWP.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWP.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblWP.Location = new System.Drawing.Point(12, 24);
            this.lblWP.Name = "lblWP";
            this.lblWP.Size = new System.Drawing.Size(76, 19);
            this.lblWP.TabIndex = 46;
            this.lblWP.Text = "Workplace";
            // 
            // gbxTop
            // 
            this.gbxTop.Controls.Add(this.lblProdmonth);
            this.gbxTop.Controls.Add(this.lblSection);
            this.gbxTop.Controls.Add(this.lblWPDesc);
            this.gbxTop.Controls.Add(this.lblWP);
            this.gbxTop.Controls.Add(this.txtPathAttachment);
            this.gbxTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxTop.Location = new System.Drawing.Point(0, 0);
            this.gbxTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxTop.Name = "gbxTop";
            this.gbxTop.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxTop.Size = new System.Drawing.Size(943, 58);
            this.gbxTop.TabIndex = 45;
            this.gbxTop.TabStop = false;
            // 
            // lblProdmonth
            // 
            this.lblProdmonth.Location = new System.Drawing.Point(690, 12);
            this.lblProdmonth.Name = "lblProdmonth";
            this.lblProdmonth.Size = new System.Drawing.Size(78, 17);
            this.lblProdmonth.TabIndex = 49;
            this.lblProdmonth.Text = "labelControl2";
            this.lblProdmonth.Visible = false;
            // 
            // lblSection
            // 
            this.lblSection.Location = new System.Drawing.Point(690, 26);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(78, 17);
            this.lblSection.TabIndex = 48;
            this.lblSection.Text = "labelControl1";
            this.lblSection.Visible = false;
            // 
            // lblWPDesc
            // 
            this.lblWPDesc.Location = new System.Drawing.Point(192, 21);
            this.lblWPDesc.Name = "lblWPDesc";
            this.lblWPDesc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lblWPDesc.Properties.NullText = "";
            this.lblWPDesc.Size = new System.Drawing.Size(226, 24);
            this.lblWPDesc.TabIndex = 47;
            // 
            // gbxBottom
            // 
            this.gbxBottom.Controls.Add(this.btnCancel);
            this.gbxBottom.Controls.Add(this.btnSaveAct);
            this.gbxBottom.Controls.Add(this.separatorControl1);
            this.gbxBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbxBottom.Location = new System.Drawing.Point(0, 460);
            this.gbxBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxBottom.Name = "gbxBottom";
            this.gbxBottom.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxBottom.Size = new System.Drawing.Size(943, 67);
            this.gbxBottom.TabIndex = 47;
            this.gbxBottom.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.close;
            this.btnCancel.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnCancel.Location = new System.Drawing.Point(469, 38);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 22);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSaveAct
            // 
            this.btnSaveAct.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.btnSaveAct.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSaveAct.Location = new System.Drawing.Point(353, 38);
            this.btnSaveAct.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveAct.Name = "btnSaveAct";
            this.btnSaveAct.Size = new System.Drawing.Size(87, 22);
            this.btnSaveAct.TabIndex = 1;
            this.btnSaveAct.Text = "Save";
            this.btnSaveAct.Click += new System.EventHandler(this.btnSaveAct_Click);
            // 
            // separatorControl1
            // 
            this.separatorControl1.Location = new System.Drawing.Point(231, 13);
            this.separatorControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.separatorControl1.Name = "separatorControl1";
            this.separatorControl1.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.separatorControl1.Size = new System.Drawing.Size(455, 26);
            this.separatorControl1.TabIndex = 0;
            // 
            // PicBox
            // 
            this.PicBox.Location = new System.Drawing.Point(3, 25);
            this.PicBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(314, 374);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBox.TabIndex = 4;
            this.PicBox.TabStop = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.PicBox;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(316, 376);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // frmMOScrutiny_Actions
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 527);
            this.Controls.Add(this.lcActImage);
            this.Controls.Add(this.gbxBottom);
            this.Controls.Add(this.gbxTop);
            this.Controls.Add(this.gbxActions);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMOScrutiny_Actions";
            this.Text = "Actions";
            this.Load += new System.EventHandler(this.frmDept_Actions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcActImage)).EndInit();
            this.lcActImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lcgActImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgSubActImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPathAttachment.Properties)).EndInit();
            this.gbxActions.ResumeLayout(false);
            this.gbxActions.PerformLayout();
            this.gbxPerson.ResumeLayout(false);
            this.gbxPerson.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RespPersonCmb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverseerCmb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityCmb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReqDate.Properties)).EndInit();
            this.gbxTop.ResumeLayout(false);
            this.gbxTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblWPDesc.Properties)).EndInit();
            this.gbxBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
		private DevExpress.XtraLayout.LayoutControl lcActImage;
		private System.Windows.Forms.PictureBox PicBox;
		private DevExpress.XtraLayout.LayoutControlGroup lcgActImage;
		private DevExpress.XtraLayout.LayoutControlGroup lcgSubActImage;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private System.Windows.Forms.OpenFileDialog ofdOpenImageFile;
		private DevExpress.XtraEditors.TextEdit txtPathAttachment;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
		private System.Windows.Forms.GroupBox gbxActions;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblWP;
		private System.Windows.Forms.GroupBox gbxTop;
		private DevExpress.XtraEditors.SimpleButton btnAddImage;
		private System.Windows.Forms.GroupBox gbxBottom;
		private DevExpress.XtraEditors.SimpleButton btnCancel;
		private DevExpress.XtraEditors.SimpleButton btnSaveAct;
		private DevExpress.XtraEditors.SeparatorControl separatorControl1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.GroupBox gbxPerson;
		public System.Windows.Forms.TextBox txtRemarks;
		public DevExpress.XtraEditors.LookUpEdit RespPersonCmb;
		public DevExpress.XtraEditors.ComboBoxEdit PriorityCmb;
		public DevExpress.XtraEditors.LookUpEdit OverseerCmb;
		public DevExpress.XtraEditors.DateEdit txtReqDate;
        public DevExpress.XtraEditors.LabelControl lblProdmonth;
        public DevExpress.XtraEditors.LabelControl lblSection;
        public DevExpress.XtraEditors.LookUpEdit lblWPDesc;
    }
}