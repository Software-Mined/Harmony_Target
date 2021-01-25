namespace Mineware.Systems.Production.OreflowDiagram
{
    partial class ucHoistingBooking
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucHoistingBooking));
            this.gcHoistBooking = new DevExpress.XtraGrid.GridControl();
            this.gvHoistBooking = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colDate = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colReefTons = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repSpinEditTons = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colWasteTons = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colSkip = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colFactor = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colSkip1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colFactor1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colTons = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colHoppers = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCheck = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lbxShaft = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barProdmonth = new DevExpress.XtraBars.BarEditItem();
            this.mwRepositoryItemProdMonth1 = new Mineware.Systems.Global.CustomControls.MWRepositoryItemProdMonth();
            this.barbtnSave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.SelectedPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gcHoistBooking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHoistBooking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSpinEditTons)).BeginInit();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mwRepositoryItemProdMonth1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcHoistBooking
            // 
            this.gcHoistBooking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcHoistBooking.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gcHoistBooking.Location = new System.Drawing.Point(183, 110);
            this.gcHoistBooking.MainView = this.gvHoistBooking;
            this.gcHoistBooking.Margin = new System.Windows.Forms.Padding(2);
            this.gcHoistBooking.Name = "gcHoistBooking";
            this.gcHoistBooking.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repSpinEditTons});
            this.gcHoistBooking.Size = new System.Drawing.Size(743, 351);
            this.gcHoistBooking.TabIndex = 10;
            this.gcHoistBooking.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHoistBooking});
            // 
            // gvHoistBooking
            // 
            this.gvHoistBooking.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.gvHoistBooking.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colDate,
            this.colSkip,
            this.colFactor,
            this.colReefTons,
            this.colSkip1,
            this.colFactor1,
            this.colWasteTons,
            this.colTons,
            this.colHoppers,
            this.colCheck});
            this.gvHoistBooking.GridControl = this.gcHoistBooking;
            this.gvHoistBooking.Name = "gvHoistBooking";
            this.gvHoistBooking.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvHoistBooking.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvHoistBooking.OptionsView.ColumnAutoWidth = false;
            this.gvHoistBooking.OptionsView.ShowGroupPanel = false;
            this.gvHoistBooking.OptionsView.ShowIndicator = false;
            this.gvHoistBooking.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvHoistBooking_CellValueChanged);
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = " ";
            this.gridBand1.Columns.Add(this.colDate);
            this.gridBand1.Columns.Add(this.colReefTons);
            this.gridBand1.Columns.Add(this.colWasteTons);
            this.gridBand1.Columns.Add(this.colSkip);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 270;
            // 
            // colDate
            // 
            this.colDate.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDate.AppearanceHeader.Options.UseForeColor = true;
            this.colDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDate.Caption = "Date";
            this.colDate.DisplayFormat.FormatString = "dd-MMM-yyyy";
            this.colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colDate.MinWidth = 19;
            this.colDate.Name = "colDate";
            this.colDate.OptionsColumn.AllowEdit = false;
            this.colDate.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDate.OptionsColumn.AllowMove = false;
            this.colDate.OptionsColumn.AllowSize = false;
            this.colDate.OptionsColumn.FixedWidth = true;
            this.colDate.OptionsFilter.AllowAutoFilter = false;
            this.colDate.OptionsFilter.AllowFilter = false;
            this.colDate.Visible = true;
            this.colDate.Width = 90;
            // 
            // colReefTons
            // 
            this.colReefTons.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colReefTons.AppearanceHeader.Options.UseForeColor = true;
            this.colReefTons.AppearanceHeader.Options.UseTextOptions = true;
            this.colReefTons.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colReefTons.Caption = "Reef Tons";
            this.colReefTons.ColumnEdit = this.repSpinEditTons;
            this.colReefTons.MinWidth = 19;
            this.colReefTons.Name = "colReefTons";
            this.colReefTons.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colReefTons.OptionsColumn.AllowMove = false;
            this.colReefTons.OptionsColumn.AllowSize = false;
            this.colReefTons.OptionsColumn.FixedWidth = true;
            this.colReefTons.OptionsFilter.AllowAutoFilter = false;
            this.colReefTons.OptionsFilter.AllowFilter = false;
            this.colReefTons.Visible = true;
            this.colReefTons.Width = 100;
            // 
            // repSpinEditTons
            // 
            this.repSpinEditTons.AutoHeight = false;
            this.repSpinEditTons.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repSpinEditTons.Mask.EditMask = "n0";
            this.repSpinEditTons.Mask.UseMaskAsDisplayFormat = true;
            this.repSpinEditTons.Name = "repSpinEditTons";
            this.repSpinEditTons.EditValueChanged += new System.EventHandler(this.repSpinEditTons_EditValueChanged);
            // 
            // colWasteTons
            // 
            this.colWasteTons.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colWasteTons.AppearanceHeader.Options.UseForeColor = true;
            this.colWasteTons.AppearanceHeader.Options.UseTextOptions = true;
            this.colWasteTons.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWasteTons.Caption = "Waste tons";
            this.colWasteTons.ColumnEdit = this.repSpinEditTons;
            this.colWasteTons.MinWidth = 19;
            this.colWasteTons.Name = "colWasteTons";
            this.colWasteTons.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colWasteTons.OptionsColumn.AllowMove = false;
            this.colWasteTons.OptionsColumn.AllowSize = false;
            this.colWasteTons.OptionsColumn.FixedWidth = true;
            this.colWasteTons.OptionsFilter.AllowAutoFilter = false;
            this.colWasteTons.OptionsFilter.AllowFilter = false;
            this.colWasteTons.Visible = true;
            this.colWasteTons.Width = 80;
            // 
            // colSkip
            // 
            this.colSkip.AppearanceHeader.Options.UseTextOptions = true;
            this.colSkip.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSkip.Caption = "Skip";
            this.colSkip.MinWidth = 19;
            this.colSkip.Name = "colSkip";
            this.colSkip.OptionsColumn.AllowEdit = false;
            this.colSkip.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colSkip.OptionsColumn.AllowMove = false;
            this.colSkip.OptionsColumn.AllowSize = false;
            this.colSkip.OptionsColumn.FixedWidth = true;
            this.colSkip.OptionsFilter.AllowAutoFilter = false;
            this.colSkip.OptionsFilter.AllowFilter = false;
            this.colSkip.Width = 150;
            // 
            // colFactor
            // 
            this.colFactor.AppearanceHeader.Options.UseTextOptions = true;
            this.colFactor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFactor.Caption = "Factor";
            this.colFactor.MinWidth = 19;
            this.colFactor.Name = "colFactor";
            this.colFactor.OptionsColumn.AllowEdit = false;
            this.colFactor.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colFactor.OptionsColumn.AllowMove = false;
            this.colFactor.OptionsColumn.AllowSize = false;
            this.colFactor.OptionsColumn.FixedWidth = true;
            this.colFactor.OptionsFilter.AllowAutoFilter = false;
            this.colFactor.OptionsFilter.AllowFilter = false;
            this.colFactor.Width = 80;
            // 
            // colSkip1
            // 
            this.colSkip1.AppearanceHeader.Options.UseTextOptions = true;
            this.colSkip1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSkip1.Caption = "Skip";
            this.colSkip1.MinWidth = 19;
            this.colSkip1.Name = "colSkip1";
            this.colSkip1.OptionsColumn.AllowEdit = false;
            this.colSkip1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colSkip1.OptionsColumn.AllowMove = false;
            this.colSkip1.OptionsColumn.AllowSize = false;
            this.colSkip1.OptionsColumn.FixedWidth = true;
            this.colSkip1.OptionsFilter.AllowAutoFilter = false;
            this.colSkip1.OptionsFilter.AllowFilter = false;
            this.colSkip1.Width = 90;
            // 
            // colFactor1
            // 
            this.colFactor1.AppearanceHeader.Options.UseTextOptions = true;
            this.colFactor1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFactor1.Caption = "Factor1";
            this.colFactor1.MinWidth = 19;
            this.colFactor1.Name = "colFactor1";
            this.colFactor1.OptionsColumn.AllowEdit = false;
            this.colFactor1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colFactor1.OptionsColumn.AllowMove = false;
            this.colFactor1.OptionsColumn.AllowSize = false;
            this.colFactor1.OptionsColumn.FixedWidth = true;
            this.colFactor1.OptionsFilter.AllowAutoFilter = false;
            this.colFactor1.OptionsFilter.AllowFilter = false;
            // 
            // colTons
            // 
            this.colTons.AppearanceHeader.Options.UseTextOptions = true;
            this.colTons.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTons.Caption = "Tons";
            this.colTons.MinWidth = 19;
            this.colTons.Name = "colTons";
            this.colTons.OptionsColumn.AllowEdit = false;
            this.colTons.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colTons.OptionsColumn.AllowMove = false;
            this.colTons.OptionsColumn.AllowSize = false;
            this.colTons.OptionsColumn.FixedWidth = true;
            this.colTons.OptionsFilter.AllowAutoFilter = false;
            this.colTons.OptionsFilter.AllowFilter = false;
            // 
            // colHoppers
            // 
            this.colHoppers.AppearanceHeader.Options.UseTextOptions = true;
            this.colHoppers.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colHoppers.Caption = "Hoppers";
            this.colHoppers.MinWidth = 19;
            this.colHoppers.Name = "colHoppers";
            this.colHoppers.Width = 70;
            // 
            // colCheck
            // 
            this.colCheck.AppearanceHeader.Options.UseTextOptions = true;
            this.colCheck.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCheck.Caption = "Check";
            this.colCheck.MinWidth = 19;
            this.colCheck.Name = "colCheck";
            this.colCheck.Width = 70;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.lbxShaft);
            this.pnlFilter.Controls.Add(this.label1);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFilter.Location = new System.Drawing.Point(0, 110);
            this.pnlFilter.Margin = new System.Windows.Forms.Padding(2);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(183, 351);
            this.pnlFilter.TabIndex = 11;
            // 
            // lbxShaft
            // 
            this.lbxShaft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxShaft.FormattingEnabled = true;
            this.lbxShaft.ItemHeight = 17;
            this.lbxShaft.Location = new System.Drawing.Point(0, 23);
            this.lbxShaft.Margin = new System.Windows.Forms.Padding(2);
            this.lbxShaft.Name = "lbxShaft";
            this.lbxShaft.Size = new System.Drawing.Size(183, 328);
            this.lbxShaft.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(44, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Shaft";
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.AllowKeyTips = false;
            this.ribbonControl1.AllowMdiChildButtons = false;
            this.ribbonControl1.AllowMinimizeRibbon = false;
            this.ribbonControl1.AllowTrimPageText = false;
            this.ribbonControl1.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.DarkBlue;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.barProdmonth,
            this.barbtnSave,
            this.barButtonItem1});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ribbonControl1.MaxItemId = 36;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.OptionsPageCategories.ShowCaptions = false;
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.mwRepositoryItemProdMonth1});
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.ShowOnMultiplePages;
            this.ribbonControl1.ShowQatLocationSelector = false;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(926, 110);
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // barProdmonth
            // 
            this.barProdmonth.Caption = "Prodmonth ";
            this.barProdmonth.Edit = this.mwRepositoryItemProdMonth1;
            this.barProdmonth.EditWidth = 100;
            this.barProdmonth.Id = 33;
            this.barProdmonth.Name = "barProdmonth";
            // 
            // mwRepositoryItemProdMonth1
            // 
            this.mwRepositoryItemProdMonth1.AutoHeight = false;
            this.mwRepositoryItemProdMonth1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.SpinUp, "1", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.SpinDown, "2", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.mwRepositoryItemProdMonth1.Mask.EditMask = "yyyyMM";
            this.mwRepositoryItemProdMonth1.Mask.IgnoreMaskBlank = false;
            this.mwRepositoryItemProdMonth1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            this.mwRepositoryItemProdMonth1.Mask.UseMaskAsDisplayFormat = true;
            this.mwRepositoryItemProdMonth1.Name = "mwRepositoryItemProdMonth1";
            // 
            // barbtnSave
            // 
            this.barbtnSave.Caption = "Save";
            this.barbtnSave.Id = 34;
            this.barbtnSave.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.barbtnSave.Name = "barbtnSave";
            this.barbtnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnSave_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Close";
            this.barButtonItem1.Id = 35;
            this.barButtonItem1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.close;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.SelectedPageGroup,
            this.ribbonPageGroup1});
            this.ribbonPage1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ribbonPage1.ImageOptions.SvgImage")));
            this.ribbonPage1.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Hoisting Bookings";
            // 
            // SelectedPageGroup
            // 
            this.SelectedPageGroup.ItemLinks.Add(this.barProdmonth);
            this.SelectedPageGroup.Name = "SelectedPageGroup";
            this.SelectedPageGroup.Text = "Filter";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barbtnSave);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Options";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(395, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 19);
            this.label2.TabIndex = 13;
            this.label2.Text = "label2";
            this.label2.Visible = false;
            // 
            // ucHoistingBooking
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gcHoistBooking);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.ribbonControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucHoistingBooking";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(926, 461);
            this.Load += new System.EventHandler(this.ucHoistingBooking_Load);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.pnlFilter, 0);
            this.Controls.SetChildIndex(this.gcHoistBooking, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcHoistBooking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHoistBooking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSpinEditTons)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mwRepositoryItemProdMonth1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcHoistBooking;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvHoistBooking;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDate;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colSkip;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colFactor;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colReefTons;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colSkip1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colFactor1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWasteTons;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTons;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbxShaft;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarEditItem barProdmonth;
        private Global.CustomControls.MWRepositoryItemProdMonth mwRepositoryItemProdMonth1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup SelectedPageGroup;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colHoppers;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCheck;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repSpinEditTons;
        private DevExpress.XtraBars.BarButtonItem barbtnSave;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
    }
}
