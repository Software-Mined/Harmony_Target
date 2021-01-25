namespace Mineware.Systems.Production.OreflowDiagram
{
    partial class frmAddDamRange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddDamRange));
            this.separatorButtons = new DevExpress.XtraEditors.SeparatorControl();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.speDistance = new DevExpress.XtraEditors.SpinEdit();
            this.cbxDam = new System.Windows.Forms.ComboBox();
            this.txtRangeName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.gcOreflowentities = new DevExpress.XtraGrid.GridControl();
            this.gvOreflowentities = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDistance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDam = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).BeginInit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speDistance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcOreflowentities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOreflowentities)).BeginInit();
            this.SuspendLayout();
            // 
            // separatorButtons
            // 
            this.separatorButtons.Location = new System.Drawing.Point(0, 365);
            this.separatorButtons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.separatorButtons.Name = "separatorButtons";
            this.separatorButtons.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.separatorButtons.Size = new System.Drawing.Size(870, 26);
            this.separatorButtons.TabIndex = 10;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Location = new System.Drawing.Point(278, 394);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(285, 35);
            this.pnlButtons.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCancel.ImageOptions.SvgImage")));
            this.btnCancel.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnCancel.Location = new System.Drawing.Point(152, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 26);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.btnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSave.Location = new System.Drawing.Point(31, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 26);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // speDistance
            // 
            this.speDistance.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.speDistance.Location = new System.Drawing.Point(652, 140);
            this.speDistance.Name = "speDistance";
            this.speDistance.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.speDistance.Properties.Mask.EditMask = "n2";
            this.speDistance.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.speDistance.Size = new System.Drawing.Size(96, 24);
            this.speDistance.TabIndex = 6;
            // 
            // cbxDam
            // 
            this.cbxDam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDam.FormattingEnabled = true;
            this.cbxDam.Location = new System.Drawing.Point(652, 22);
            this.cbxDam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbxDam.Name = "cbxDam";
            this.cbxDam.Size = new System.Drawing.Size(196, 25);
            this.cbxDam.TabIndex = 4;
            // 
            // txtRangeName
            // 
            this.txtRangeName.Location = new System.Drawing.Point(652, 82);
            this.txtRangeName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRangeName.MaxLength = 50;
            this.txtRangeName.Name = "txtRangeName";
            this.txtRangeName.Size = new System.Drawing.Size(196, 25);
            this.txtRangeName.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(649, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 19);
            this.label5.TabIndex = 3;
            this.label5.Text = "Distance :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(649, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "Range Name :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(649, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "Dam :";
            // 
            // btnDelete
            // 
            this.btnDelete.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.DeleteRed;
            this.btnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnDelete.Location = new System.Drawing.Point(754, 207);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(94, 32);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete  ";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.AddBlue1;
            this.btnAdd.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnAdd.Location = new System.Drawing.Point(652, 207);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(94, 32);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add  ";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gcOreflowentities
            // 
            this.gcOreflowentities.Location = new System.Drawing.Point(12, 13);
            this.gcOreflowentities.MainView = this.gvOreflowentities;
            this.gcOreflowentities.Name = "gcOreflowentities";
            this.gcOreflowentities.Size = new System.Drawing.Size(605, 344);
            this.gcOreflowentities.TabIndex = 13;
            this.gcOreflowentities.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOreflowentities});
            // 
            // gvOreflowentities
            // 
            this.gvOreflowentities.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDescription,
            this.colID,
            this.colDistance,
            this.colDam});
            this.gvOreflowentities.DetailHeight = 458;
            this.gvOreflowentities.FixedLineWidth = 3;
            this.gvOreflowentities.GridControl = this.gcOreflowentities;
            this.gvOreflowentities.Name = "gvOreflowentities";
            this.gvOreflowentities.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvOreflowentities.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvOreflowentities.OptionsBehavior.Editable = false;
            this.gvOreflowentities.OptionsBehavior.ReadOnly = true;
            this.gvOreflowentities.OptionsView.ShowGroupPanel = false;
            this.gvOreflowentities.OptionsView.ShowIndicator = false;
            // 
            // colDescription
            // 
            this.colDescription.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDescription.AppearanceHeader.Options.UseForeColor = true;
            this.colDescription.Caption = "Range";
            this.colDescription.MinWidth = 24;
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.AllowEdit = false;
            this.colDescription.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDescription.OptionsColumn.AllowMove = false;
            this.colDescription.OptionsColumn.AllowSize = false;
            this.colDescription.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colDescription.OptionsColumn.FixedWidth = true;
            this.colDescription.OptionsFilter.AllowAutoFilter = false;
            this.colDescription.OptionsFilter.AllowFilter = false;
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 0;
            this.colDescription.Width = 100;
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.MinWidth = 24;
            this.colID.Name = "colID";
            this.colID.OptionsColumn.AllowEdit = false;
            this.colID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colID.OptionsColumn.AllowMove = false;
            this.colID.OptionsColumn.AllowSize = false;
            this.colID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colID.OptionsColumn.FixedWidth = true;
            this.colID.OptionsFilter.AllowAutoFilter = false;
            this.colID.OptionsFilter.AllowFilter = false;
            this.colID.Width = 100;
            // 
            // colDistance
            // 
            this.colDistance.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDistance.AppearanceHeader.Options.UseForeColor = true;
            this.colDistance.Caption = "Distance";
            this.colDistance.MinWidth = 24;
            this.colDistance.Name = "colDistance";
            this.colDistance.OptionsColumn.AllowEdit = false;
            this.colDistance.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDistance.OptionsColumn.AllowMove = false;
            this.colDistance.OptionsColumn.AllowSize = false;
            this.colDistance.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colDistance.OptionsColumn.FixedWidth = true;
            this.colDistance.OptionsFilter.AllowAutoFilter = false;
            this.colDistance.OptionsFilter.AllowFilter = false;
            this.colDistance.Visible = true;
            this.colDistance.VisibleIndex = 2;
            this.colDistance.Width = 40;
            // 
            // colDam
            // 
            this.colDam.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDam.AppearanceHeader.Options.UseForeColor = true;
            this.colDam.Caption = "Dam";
            this.colDam.MinWidth = 24;
            this.colDam.Name = "colDam";
            this.colDam.OptionsColumn.AllowEdit = false;
            this.colDam.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDam.OptionsColumn.AllowMove = false;
            this.colDam.OptionsColumn.AllowSize = false;
            this.colDam.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colDam.OptionsColumn.FixedWidth = true;
            this.colDam.OptionsFilter.AllowAutoFilter = false;
            this.colDam.OptionsFilter.AllowFilter = false;
            this.colDam.Visible = true;
            this.colDam.VisibleIndex = 1;
            this.colDam.Width = 94;
            // 
            // frmAddDamRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 443);
            this.Controls.Add(this.gcOreflowentities);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.speDistance);
            this.Controls.Add(this.cbxDam);
            this.Controls.Add(this.txtRangeName);
            this.Controls.Add(this.separatorButtons);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmAddDamRange";
            this.Text = "Add Range";
            this.Load += new System.EventHandler(this.frmAddDamRange_Load);
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.speDistance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcOreflowentities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOreflowentities)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SeparatorControl separatorButtons;
        private System.Windows.Forms.Panel pnlButtons;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SpinEdit speDistance;
        public System.Windows.Forms.ComboBox cbxDam;
        public System.Windows.Forms.TextBox txtRangeName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraGrid.GridControl gcOreflowentities;
        private DevExpress.XtraGrid.Views.Grid.GridView gvOreflowentities;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colDistance;
        private DevExpress.XtraGrid.Columns.GridColumn colDam;
    }
}