namespace Mineware.Systems.Production.Departmental.Survey
{
	partial class PegValuesCaptureForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PegValuesCaptureForm));
            this.pnlPegMain = new System.Windows.Forms.Panel();
            this.lcMainPegCapt = new DevExpress.XtraLayout.LayoutControl();
            this.gcPegValues = new DevExpress.XtraGrid.GridControl();
            this.gvPegValues = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colWorkplaceID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDescripion = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colPegID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colValue = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colLetter1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colLetter2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colLetter3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repYesNo = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.repCheckYesNo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcRootPegCapt = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.gbxPegCapt = new System.Windows.Forms.GroupBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtPegNum = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtPegValue = new DevExpress.XtraEditors.TextEdit();
            this.txtLetter1 = new DevExpress.XtraEditors.TextEdit();
            this.lblLetter3 = new DevExpress.XtraEditors.LabelControl();
            this.lblLetter1 = new DevExpress.XtraEditors.LabelControl();
            this.txtLetter3 = new DevExpress.XtraEditors.TextEdit();
            this.txtLetter2 = new DevExpress.XtraEditors.TextEdit();
            this.lblLetter2 = new DevExpress.XtraEditors.LabelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.gbxBottom = new System.Windows.Forms.GroupBox();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblWpDescription = new DevExpress.XtraEditors.LabelControl();
            this.lblWpID = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.pnlPegMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcMainPegCapt)).BeginInit();
            this.lcMainPegCapt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPegValues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPegValues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repYesNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCheckYesNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcRootPegCapt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.gbxPegCapt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPegNum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPegValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetter1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetter3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetter2.Properties)).BeginInit();
            this.gbxBottom.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPegMain
            // 
            this.pnlPegMain.Controls.Add(this.lcMainPegCapt);
            this.pnlPegMain.Controls.Add(this.pnlRight);
            this.pnlPegMain.Controls.Add(this.gbxPegCapt);
            this.pnlPegMain.Location = new System.Drawing.Point(0, 41);
            this.pnlPegMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlPegMain.Name = "pnlPegMain";
            this.pnlPegMain.Size = new System.Drawing.Size(745, 412);
            this.pnlPegMain.TabIndex = 2;
            // 
            // lcMainPegCapt
            // 
            this.lcMainPegCapt.Controls.Add(this.gcPegValues);
            this.lcMainPegCapt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcMainPegCapt.Location = new System.Drawing.Point(220, 0);
            this.lcMainPegCapt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcMainPegCapt.Name = "lcMainPegCapt";
            this.lcMainPegCapt.Root = this.Root;
            this.lcMainPegCapt.Size = new System.Drawing.Size(415, 412);
            this.lcMainPegCapt.TabIndex = 4;
            this.lcMainPegCapt.Text = "lcPegCapture";
            // 
            // gcPegValues
            // 
            this.gcPegValues.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gcPegValues.Location = new System.Drawing.Point(3, 25);
            this.gcPegValues.MainView = this.gvPegValues;
            this.gcPegValues.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcPegValues.Name = "gcPegValues";
            this.gcPegValues.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repYesNo,
            this.repDate,
            this.repCheckYesNo});
            this.gcPegValues.Size = new System.Drawing.Size(409, 384);
            this.gcPegValues.TabIndex = 6;
            this.gcPegValues.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPegValues});
            // 
            // gvPegValues
            // 
            this.gvPegValues.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.gvPegValues.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colWorkplaceID,
            this.colDescripion,
            this.colPegID,
            this.colValue,
            this.colLetter1,
            this.colLetter2,
            this.colLetter3});
            this.gvPegValues.DetailHeight = 458;
            this.gvPegValues.FixedLineWidth = 3;
            this.gvPegValues.GridControl = this.gcPegValues;
            this.gvPegValues.Name = "gvPegValues";
            this.gvPegValues.OptionsView.AllowCellMerge = true;
            this.gvPegValues.OptionsView.ColumnAutoWidth = false;
            this.gvPegValues.OptionsView.ShowBands = false;
            this.gvPegValues.OptionsView.ShowGroupPanel = false;
            this.gvPegValues.OptionsView.ShowIndicator = false;
            this.gvPegValues.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gvPegValues_RowCellClick);
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Columns.Add(this.colWorkplaceID);
            this.gridBand1.Columns.Add(this.colDescripion);
            this.gridBand1.Columns.Add(this.colPegID);
            this.gridBand1.Columns.Add(this.colValue);
            this.gridBand1.Columns.Add(this.colLetter1);
            this.gridBand1.Columns.Add(this.colLetter2);
            this.gridBand1.Columns.Add(this.colLetter3);
            this.gridBand1.MinWidth = 12;
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 192;
            // 
            // colWorkplaceID
            // 
            this.colWorkplaceID.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colWorkplaceID.AppearanceCell.Options.UseFont = true;
            this.colWorkplaceID.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.colWorkplaceID.AppearanceHeader.Options.UseFont = true;
            this.colWorkplaceID.AppearanceHeader.Options.UseTextOptions = true;
            this.colWorkplaceID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWorkplaceID.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWorkplaceID.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWorkplaceID.Caption = "WP ID";
            this.colWorkplaceID.MinWidth = 23;
            this.colWorkplaceID.Name = "colWorkplaceID";
            this.colWorkplaceID.OptionsColumn.AllowEdit = false;
            this.colWorkplaceID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colWorkplaceID.OptionsColumn.AllowMove = false;
            this.colWorkplaceID.OptionsColumn.AllowSize = false;
            this.colWorkplaceID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colWorkplaceID.OptionsColumn.FixedWidth = true;
            this.colWorkplaceID.OptionsFilter.AllowAutoFilter = false;
            this.colWorkplaceID.OptionsFilter.AllowFilter = false;
            this.colWorkplaceID.Width = 87;
            // 
            // colDescripion
            // 
            this.colDescripion.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colDescripion.AppearanceCell.Options.UseFont = true;
            this.colDescripion.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.colDescripion.AppearanceHeader.Options.UseFont = true;
            this.colDescripion.AppearanceHeader.Options.UseTextOptions = true;
            this.colDescripion.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDescripion.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colDescripion.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colDescripion.Caption = "Description";
            this.colDescripion.MinWidth = 23;
            this.colDescripion.Name = "colDescripion";
            this.colDescripion.OptionsColumn.AllowEdit = false;
            this.colDescripion.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDescripion.OptionsColumn.AllowMove = false;
            this.colDescripion.OptionsColumn.AllowSize = false;
            this.colDescripion.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colDescripion.OptionsColumn.FixedWidth = true;
            this.colDescripion.OptionsFilter.AllowAutoFilter = false;
            this.colDescripion.OptionsFilter.AllowFilter = false;
            this.colDescripion.Width = 194;
            // 
            // colPegID
            // 
            this.colPegID.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colPegID.AppearanceCell.Options.UseFont = true;
            this.colPegID.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.colPegID.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colPegID.AppearanceHeader.Options.UseFont = true;
            this.colPegID.AppearanceHeader.Options.UseForeColor = true;
            this.colPegID.AppearanceHeader.Options.UseTextOptions = true;
            this.colPegID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPegID.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPegID.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colPegID.Caption = "Peg ID";
            this.colPegID.MinWidth = 23;
            this.colPegID.Name = "colPegID";
            this.colPegID.OptionsColumn.AllowEdit = false;
            this.colPegID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colPegID.OptionsColumn.AllowMove = false;
            this.colPegID.OptionsColumn.AllowSize = false;
            this.colPegID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colPegID.OptionsColumn.FixedWidth = true;
            this.colPegID.OptionsFilter.AllowAutoFilter = false;
            this.colPegID.OptionsFilter.AllowFilter = false;
            this.colPegID.Visible = true;
            this.colPegID.Width = 105;
            // 
            // colValue
            // 
            this.colValue.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colValue.AppearanceCell.Options.UseFont = true;
            this.colValue.AppearanceCell.Options.UseTextOptions = true;
            this.colValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colValue.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.colValue.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colValue.AppearanceHeader.Options.UseFont = true;
            this.colValue.AppearanceHeader.Options.UseForeColor = true;
            this.colValue.AppearanceHeader.Options.UseTextOptions = true;
            this.colValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colValue.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colValue.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colValue.Caption = "Value";
            this.colValue.MinWidth = 23;
            this.colValue.Name = "colValue";
            this.colValue.OptionsColumn.AllowEdit = false;
            this.colValue.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colValue.OptionsColumn.AllowMove = false;
            this.colValue.OptionsColumn.AllowSize = false;
            this.colValue.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colValue.OptionsColumn.FixedWidth = true;
            this.colValue.OptionsFilter.AllowAutoFilter = false;
            this.colValue.OptionsFilter.AllowFilter = false;
            this.colValue.Visible = true;
            this.colValue.Width = 87;
            // 
            // colLetter1
            // 
            this.colLetter1.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colLetter1.AppearanceCell.Options.UseFont = true;
            this.colLetter1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.colLetter1.AppearanceHeader.Options.UseFont = true;
            this.colLetter1.AppearanceHeader.Options.UseTextOptions = true;
            this.colLetter1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLetter1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLetter1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colLetter1.Caption = "Letter 1";
            this.colLetter1.MinWidth = 23;
            this.colLetter1.Name = "colLetter1";
            this.colLetter1.OptionsColumn.AllowEdit = false;
            this.colLetter1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colLetter1.OptionsColumn.AllowMove = false;
            this.colLetter1.OptionsColumn.AllowSize = false;
            this.colLetter1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colLetter1.OptionsColumn.FixedWidth = true;
            this.colLetter1.OptionsFilter.AllowAutoFilter = false;
            this.colLetter1.OptionsFilter.AllowFilter = false;
            this.colLetter1.Width = 87;
            // 
            // colLetter2
            // 
            this.colLetter2.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colLetter2.AppearanceCell.Options.UseFont = true;
            this.colLetter2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.colLetter2.AppearanceHeader.Options.UseFont = true;
            this.colLetter2.AppearanceHeader.Options.UseTextOptions = true;
            this.colLetter2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLetter2.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLetter2.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colLetter2.Caption = "Letter 2";
            this.colLetter2.MinWidth = 23;
            this.colLetter2.Name = "colLetter2";
            this.colLetter2.OptionsColumn.AllowEdit = false;
            this.colLetter2.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colLetter2.OptionsColumn.AllowMove = false;
            this.colLetter2.OptionsColumn.AllowSize = false;
            this.colLetter2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colLetter2.OptionsColumn.FixedWidth = true;
            this.colLetter2.OptionsFilter.AllowAutoFilter = false;
            this.colLetter2.OptionsFilter.AllowFilter = false;
            this.colLetter2.Width = 87;
            // 
            // colLetter3
            // 
            this.colLetter3.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colLetter3.AppearanceCell.Options.UseFont = true;
            this.colLetter3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.colLetter3.AppearanceHeader.Options.UseFont = true;
            this.colLetter3.AppearanceHeader.Options.UseTextOptions = true;
            this.colLetter3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLetter3.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLetter3.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colLetter3.Caption = "Letter 3";
            this.colLetter3.MinWidth = 23;
            this.colLetter3.Name = "colLetter3";
            this.colLetter3.OptionsColumn.AllowEdit = false;
            this.colLetter3.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colLetter3.OptionsColumn.AllowMove = false;
            this.colLetter3.OptionsColumn.AllowSize = false;
            this.colLetter3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colLetter3.OptionsColumn.FixedWidth = true;
            this.colLetter3.OptionsFilter.AllowAutoFilter = false;
            this.colLetter3.OptionsFilter.AllowFilter = false;
            this.colLetter3.Width = 87;
            // 
            // repYesNo
            // 
            this.repYesNo.AutoHeight = false;
            this.repYesNo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repYesNo.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.repYesNo.Name = "repYesNo";
            // 
            // repDate
            // 
            this.repDate.AutoHeight = false;
            this.repDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repDate.Mask.EditMask = "dd MMM yyyy";
            this.repDate.Mask.UseMaskAsDisplayFormat = true;
            this.repDate.Name = "repDate";
            // 
            // repCheckYesNo
            // 
            this.repCheckYesNo.AutoHeight = false;
            this.repCheckYesNo.Name = "repCheckYesNo";
            this.repCheckYesNo.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repCheckYesNo.ValueChecked = "Y";
            this.repCheckYesNo.ValueUnchecked = "N";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcRootPegCapt});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(415, 412);
            this.Root.TextVisible = false;
            // 
            // lcRootPegCapt
            // 
            this.lcRootPegCapt.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("lcRootPegCapt.CaptionImageOptions.Image")));
            this.lcRootPegCapt.CaptionImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.ZoomBlue2;
            this.lcRootPegCapt.CaptionImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.lcRootPegCapt.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.lcRootPegCapt.Location = new System.Drawing.Point(0, 0);
            this.lcRootPegCapt.Name = "lcRootPegCapt";
            this.lcRootPegCapt.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcRootPegCapt.Size = new System.Drawing.Size(415, 412);
            this.lcRootPegCapt.Text = "Peg Values";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gcPegValues;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(411, 386);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.btnDelete);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(635, 0);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(110, 412);
            this.pnlRight.TabIndex = 5;
            // 
            // btnDelete
            // 
            this.btnDelete.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.DeleteRed;
            this.btnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnDelete.Location = new System.Drawing.Point(9, 184);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(87, 22);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // gbxPegCapt
            // 
            this.gbxPegCapt.Controls.Add(this.labelControl2);
            this.gbxPegCapt.Controls.Add(this.txtPegNum);
            this.gbxPegCapt.Controls.Add(this.labelControl1);
            this.gbxPegCapt.Controls.Add(this.txtPegValue);
            this.gbxPegCapt.Controls.Add(this.txtLetter1);
            this.gbxPegCapt.Controls.Add(this.lblLetter3);
            this.gbxPegCapt.Controls.Add(this.lblLetter1);
            this.gbxPegCapt.Controls.Add(this.txtLetter3);
            this.gbxPegCapt.Controls.Add(this.txtLetter2);
            this.gbxPegCapt.Controls.Add(this.lblLetter2);
            this.gbxPegCapt.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbxPegCapt.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxPegCapt.Location = new System.Drawing.Point(0, 0);
            this.gbxPegCapt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxPegCapt.Name = "gbxPegCapt";
            this.gbxPegCapt.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxPegCapt.Size = new System.Drawing.Size(220, 412);
            this.gbxPegCapt.TabIndex = 4;
            this.gbxPegCapt.TabStop = false;
            this.gbxPegCapt.Text = "Peg Capture";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(10, 39);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(47, 17);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "Peg No.";
            // 
            // txtPegNum
            // 
            this.txtPegNum.EditValue = "";
            this.txtPegNum.Location = new System.Drawing.Point(63, 35);
            this.txtPegNum.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPegNum.Name = "txtPegNum";
            this.txtPegNum.Size = new System.Drawing.Size(148, 24);
            this.txtPegNum.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 72);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 17);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "Value";
            // 
            // txtPegValue
            // 
            this.txtPegValue.EditValue = "0.00";
            this.txtPegValue.Location = new System.Drawing.Point(143, 68);
            this.txtPegValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPegValue.Name = "txtPegValue";
            this.txtPegValue.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPegValue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPegValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPegValue.Properties.Mask.EditMask = "n2";
            this.txtPegValue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPegValue.Size = new System.Drawing.Size(68, 24);
            this.txtPegValue.TabIndex = 2;
            // 
            // txtLetter1
            // 
            this.txtLetter1.Location = new System.Drawing.Point(62, 101);
            this.txtLetter1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLetter1.Name = "txtLetter1";
            this.txtLetter1.Size = new System.Drawing.Size(149, 24);
            this.txtLetter1.TabIndex = 4;
            // 
            // lblLetter3
            // 
            this.lblLetter3.Location = new System.Drawing.Point(10, 170);
            this.lblLetter3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblLetter3.Name = "lblLetter3";
            this.lblLetter3.Size = new System.Drawing.Size(44, 17);
            this.lblLetter3.TabIndex = 9;
            this.lblLetter3.Text = "Letter 3";
            // 
            // lblLetter1
            // 
            this.lblLetter1.Location = new System.Drawing.Point(10, 105);
            this.lblLetter1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblLetter1.Name = "lblLetter1";
            this.lblLetter1.Size = new System.Drawing.Size(44, 17);
            this.lblLetter1.TabIndex = 5;
            this.lblLetter1.Text = "Letter 1";
            // 
            // txtLetter3
            // 
            this.txtLetter3.Location = new System.Drawing.Point(62, 166);
            this.txtLetter3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLetter3.Name = "txtLetter3";
            this.txtLetter3.Size = new System.Drawing.Size(149, 24);
            this.txtLetter3.TabIndex = 8;
            // 
            // txtLetter2
            // 
            this.txtLetter2.Location = new System.Drawing.Point(62, 133);
            this.txtLetter2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLetter2.Name = "txtLetter2";
            this.txtLetter2.Size = new System.Drawing.Size(149, 24);
            this.txtLetter2.TabIndex = 6;
            // 
            // lblLetter2
            // 
            this.lblLetter2.Location = new System.Drawing.Point(10, 137);
            this.lblLetter2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblLetter2.Name = "lblLetter2";
            this.lblLetter2.Size = new System.Drawing.Size(44, 17);
            this.lblLetter2.TabIndex = 7;
            this.lblLetter2.Text = "Letter 2";
            // 
            // btnSave
            // 
            this.btnSave.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.btnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSave.Location = new System.Drawing.Point(258, 20);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 22);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbxBottom
            // 
            this.gbxBottom.Controls.Add(this.btnCancel);
            this.gbxBottom.Controls.Add(this.btnSave);
            this.gbxBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbxBottom.Location = new System.Drawing.Point(0, 451);
            this.gbxBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxBottom.Name = "gbxBottom";
            this.gbxBottom.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxBottom.Size = new System.Drawing.Size(743, 63);
            this.gbxBottom.TabIndex = 49;
            this.gbxBottom.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCancel.ImageOptions.SvgImage")));
            this.btnCancel.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnCancel.Location = new System.Drawing.Point(362, 20);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 22);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblWpDescription);
            this.panel1.Controls.Add(this.lblWpID);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(743, 39);
            this.panel1.TabIndex = 50;
            // 
            // lblWpDescription
            // 
            this.lblWpDescription.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWpDescription.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblWpDescription.Appearance.Options.UseFont = true;
            this.lblWpDescription.Appearance.Options.UseForeColor = true;
            this.lblWpDescription.Location = new System.Drawing.Point(85, 10);
            this.lblWpDescription.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblWpDescription.Name = "lblWpDescription";
            this.lblWpDescription.Size = new System.Drawing.Size(93, 17);
            this.lblWpDescription.TabIndex = 3;
            this.lblWpDescription.Text = "WP Description";
            // 
            // lblWpID
            // 
            this.lblWpID.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWpID.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblWpID.Appearance.Options.UseFont = true;
            this.lblWpID.Appearance.Options.UseForeColor = true;
            this.lblWpID.Location = new System.Drawing.Point(304, 10);
            this.lblWpID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblWpID.Name = "lblWpID";
            this.lblWpID.Size = new System.Drawing.Size(13, 17);
            this.lblWpID.TabIndex = 2;
            this.lblWpID.Text = "ID";
            this.lblWpID.Visible = false;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Appearance.Options.UseForeColor = true;
            this.labelControl4.Location = new System.Drawing.Point(13, 10);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(71, 17);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "Workplace: ";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(218, 10);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(81, 17);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "Workplace ID";
            this.labelControl3.Visible = false;
            // 
            // PegValuesCaptureForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 514);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbxBottom);
            this.Controls.Add(this.pnlPegMain);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PegValuesCaptureForm";
            this.Text = "Peg Values Capture";
            this.Load += new System.EventHandler(this.PegValuesCaptureForm_Load);
            this.pnlPegMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lcMainPegCapt)).EndInit();
            this.lcMainPegCapt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcPegValues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPegValues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repYesNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCheckYesNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcRootPegCapt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.gbxPegCapt.ResumeLayout(false);
            this.gbxPegCapt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPegNum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPegValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetter1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetter3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetter2.Properties)).EndInit();
            this.gbxBottom.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel pnlPegMain;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.LabelControl lblLetter3;
		private DevExpress.XtraEditors.LabelControl lblLetter2;
		private DevExpress.XtraEditors.LabelControl lblLetter1;
		private DevExpress.XtraGrid.GridControl gcPegValues;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvPegValues;
		private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWorkplaceID;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDescripion;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPegID;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colValue;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLetter1;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLetter2;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLetter3;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repYesNo;
		private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repDate;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repCheckYesNo;
		private System.Windows.Forms.GroupBox gbxBottom;
		private DevExpress.XtraEditors.SimpleButton btnDelete;
		private DevExpress.XtraEditors.SimpleButton btnCancel;
		private DevExpress.XtraEditors.SimpleButton btnSave;
		private System.Windows.Forms.Panel panel1;
		private DevExpress.XtraEditors.LabelControl labelControl4;
		private DevExpress.XtraEditors.LabelControl labelControl3;
		public DevExpress.XtraEditors.LabelControl lblWpDescription;
		public DevExpress.XtraEditors.LabelControl lblWpID;
		public DevExpress.XtraEditors.TextEdit txtLetter2;
		public DevExpress.XtraEditors.TextEdit txtLetter1;
		public DevExpress.XtraEditors.TextEdit txtPegValue;
		public DevExpress.XtraEditors.TextEdit txtPegNum;
		public DevExpress.XtraEditors.TextEdit txtLetter3;
		private DevExpress.XtraLayout.LayoutControl lcMainPegCapt;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlGroup lcRootPegCapt;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private System.Windows.Forms.GroupBox gbxPegCapt;
		private System.Windows.Forms.Panel pnlRight;
	}
}