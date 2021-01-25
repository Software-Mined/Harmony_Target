namespace Mineware.Systems.Production.SysAdmin
{
    partial class ucEquipmentType
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
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleIconSet formatConditionRuleIconSet1 = new DevExpress.XtraEditors.FormatConditionRuleIconSet();
            DevExpress.XtraEditors.FormatConditionIconSet formatConditionIconSet1 = new DevExpress.XtraEditors.FormatConditionIconSet();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon1 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon2 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon3 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucEquipmentType));
            this.colEquipValid = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gvDetail = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colWPIDDetail = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWPDetail = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colStoppageDate = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDist = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colStoppageType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDetailComments = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcEquipmentType = new DevExpress.XtraGrid.GridControl();
            this.gvEquipmentType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colEquipID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colEquipName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colEquipType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colEquipMake = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rcWorkplace = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.CancelBtn = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.btnWPAdd = new DevExpress.XtraBars.BarButtonItem();
            this.PrevRepbtn = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.FLEdit = new DevExpress.XtraBars.BarEditItem();
            this.AdvEdit = new DevExpress.XtraBars.BarEditItem();
            this.AuthBtn = new DevExpress.XtraBars.BarButtonItem();
            this.BtnAddDoc = new DevExpress.XtraBars.BarButtonItem();
            this.btnEngInventory = new DevExpress.XtraBars.BarButtonItem();
            this.tbProdMonth = new DevExpress.XtraBars.BarEditItem();
            this.tbSection = new DevExpress.XtraBars.BarEditItem();
            this.tbCrew = new DevExpress.XtraBars.BarEditItem();
            this.tbDpInspecDate = new DevExpress.XtraBars.BarEditItem();
            this.btnWPDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnClose = new DevExpress.XtraBars.BarButtonItem();
            this.barActivity = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemRadioGroup1 = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.btnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.EditBtn = new DevExpress.XtraBars.BarButtonItem();
            this.rpEquipment = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.repositoryItemComboStoppages = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcEquipmentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEquipmentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcWorkplace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboStoppages)).BeginInit();
            this.SuspendLayout();
            // 
            // colEquipValid
            // 
            this.colEquipValid.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colEquipValid.AppearanceHeader.Options.UseForeColor = true;
            this.colEquipValid.AppearanceHeader.Options.UseTextOptions = true;
            this.colEquipValid.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colEquipValid.Caption = "Valid";
            this.colEquipValid.Name = "colEquipValid";
            this.colEquipValid.OptionsColumn.AllowEdit = false;
            this.colEquipValid.Visible = true;
            this.colEquipValid.Width = 35;
            // 
            // gvDetail
            // 
            this.gvDetail.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand2});
            this.gvDetail.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colWPIDDetail,
            this.colWPDetail,
            this.colDist,
            this.colStoppageType,
            this.colDetailComments,
            this.colStoppageDate});
            this.gvDetail.GridControl = this.gcEquipmentType;
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.OptionsView.ShowBands = false;
            this.gvDetail.OptionsView.ShowGroupPanel = false;
            // 
            // gridBand2
            // 
            this.gridBand2.Caption = "gridBand2";
            this.gridBand2.Columns.Add(this.colWPIDDetail);
            this.gridBand2.Columns.Add(this.colWPDetail);
            this.gridBand2.Columns.Add(this.colStoppageDate);
            this.gridBand2.Columns.Add(this.colDist);
            this.gridBand2.Columns.Add(this.colStoppageType);
            this.gridBand2.Columns.Add(this.colDetailComments);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 0;
            this.gridBand2.Width = 1039;
            // 
            // colWPIDDetail
            // 
            this.colWPIDDetail.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colWPIDDetail.AppearanceHeader.Options.UseForeColor = true;
            this.colWPIDDetail.Caption = "WPID";
            this.colWPIDDetail.Name = "colWPIDDetail";
            this.colWPIDDetail.OptionsColumn.AllowEdit = false;
            this.colWPIDDetail.Visible = true;
            this.colWPIDDetail.Width = 64;
            // 
            // colWPDetail
            // 
            this.colWPDetail.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colWPDetail.AppearanceHeader.Options.UseForeColor = true;
            this.colWPDetail.Caption = "Workplace";
            this.colWPDetail.Name = "colWPDetail";
            this.colWPDetail.OptionsColumn.AllowEdit = false;
            this.colWPDetail.Visible = true;
            this.colWPDetail.Width = 148;
            // 
            // colStoppageDate
            // 
            this.colStoppageDate.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colStoppageDate.AppearanceHeader.Options.UseForeColor = true;
            this.colStoppageDate.Caption = "Stoppage Date";
            this.colStoppageDate.Name = "colStoppageDate";
            this.colStoppageDate.OptionsColumn.AllowEdit = false;
            this.colStoppageDate.Visible = true;
            this.colStoppageDate.Width = 86;
            // 
            // colDist
            // 
            this.colDist.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDist.AppearanceHeader.Options.UseForeColor = true;
            this.colDist.Caption = "Distance";
            this.colDist.Name = "colDist";
            this.colDist.OptionsColumn.AllowEdit = false;
            this.colDist.Visible = true;
            this.colDist.Width = 55;
            // 
            // colStoppageType
            // 
            this.colStoppageType.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colStoppageType.AppearanceHeader.Options.UseForeColor = true;
            this.colStoppageType.Caption = "Stoppage Type";
            this.colStoppageType.Name = "colStoppageType";
            this.colStoppageType.OptionsColumn.AllowEdit = false;
            this.colStoppageType.Visible = true;
            this.colStoppageType.Width = 232;
            // 
            // colDetailComments
            // 
            this.colDetailComments.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDetailComments.AppearanceHeader.Options.UseForeColor = true;
            this.colDetailComments.Caption = "Comments";
            this.colDetailComments.Name = "colDetailComments";
            this.colDetailComments.OptionsColumn.AllowEdit = false;
            this.colDetailComments.Visible = true;
            this.colDetailComments.Width = 454;
            // 
            // gcEquipmentType
            // 
            this.gcEquipmentType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcEquipmentType.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gcEquipmentType.Location = new System.Drawing.Point(0, 111);
            this.gcEquipmentType.MainView = this.gvEquipmentType;
            this.gcEquipmentType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcEquipmentType.MenuManager = this.rcWorkplace;
            this.gcEquipmentType.Name = "gcEquipmentType";
            this.gcEquipmentType.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboStoppages});
            this.gcEquipmentType.Size = new System.Drawing.Size(1057, 575);
            this.gcEquipmentType.TabIndex = 2;
            this.gcEquipmentType.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvEquipmentType,
            this.gvDetail});
            // 
            // gvEquipmentType
            // 
            this.gvEquipmentType.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.gvEquipmentType.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colEquipID,
            this.colEquipName,
            this.colEquipType,
            this.colEquipMake,
            this.colEquipValid});
            this.gvEquipmentType.DetailHeight = 431;
            gridFormatRule1.Column = this.colEquipValid;
            gridFormatRule1.Name = "TickYesNo";
            formatConditionIconSet1.CategoryName = "Symbols";
            formatConditionIconSetIcon1.PredefinedName = "Symbols23_1.png";
            formatConditionIconSetIcon1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            formatConditionIconSetIcon1.ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSetIcon2.PredefinedName = "Symbols23_2.png";
            formatConditionIconSetIcon2.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            formatConditionIconSetIcon2.ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSetIcon3.PredefinedName = "Symbols23_3.png";
            formatConditionIconSetIcon3.ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon1);
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon2);
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon3);
            formatConditionIconSet1.Name = "Symbols3Circled";
            formatConditionIconSet1.ValueType = DevExpress.XtraEditors.FormatConditionValueType.Number;
            formatConditionRuleIconSet1.IconSet = formatConditionIconSet1;
            gridFormatRule1.Rule = formatConditionRuleIconSet1;
            this.gvEquipmentType.FormatRules.Add(gridFormatRule1);
            this.gvEquipmentType.GridControl = this.gcEquipmentType;
            this.gvEquipmentType.Name = "gvEquipmentType";
            this.gvEquipmentType.OptionsView.ColumnAutoWidth = false;
            this.gvEquipmentType.OptionsView.ShowAutoFilterRow = true;
            this.gvEquipmentType.OptionsView.ShowBands = false;
            this.gvEquipmentType.OptionsView.ShowGroupPanel = false;
            this.gvEquipmentType.OptionsView.ShowIndicator = false;
            this.gvEquipmentType.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gvWorkplaces_RowCellClick);
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Columns.Add(this.colEquipID);
            this.gridBand1.Columns.Add(this.colEquipName);
            this.gridBand1.Columns.Add(this.colEquipType);
            this.gridBand1.Columns.Add(this.colEquipMake);
            this.gridBand1.Columns.Add(this.colEquipValid);
            this.gridBand1.MinWidth = 14;
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 778;
            // 
            // colEquipID
            // 
            this.colEquipID.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colEquipID.AppearanceHeader.Options.UseFont = true;
            this.colEquipID.AppearanceHeader.Options.UseForeColor = true;
            this.colEquipID.AppearanceHeader.Options.UseTextOptions = true;
            this.colEquipID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colEquipID.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colEquipID.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colEquipID.Caption = "EquipID";
            this.colEquipID.MinWidth = 27;
            this.colEquipID.Name = "colEquipID";
            this.colEquipID.OptionsColumn.AllowEdit = false;
            this.colEquipID.OptionsColumn.AllowFocus = false;
            this.colEquipID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colEquipID.OptionsColumn.AllowMove = false;
            this.colEquipID.OptionsColumn.AllowSize = false;
            this.colEquipID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colEquipID.OptionsColumn.FixedWidth = true;
            this.colEquipID.Visible = true;
            this.colEquipID.Width = 113;
            // 
            // colEquipName
            // 
            this.colEquipName.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colEquipName.AppearanceHeader.Options.UseFont = true;
            this.colEquipName.AppearanceHeader.Options.UseForeColor = true;
            this.colEquipName.AppearanceHeader.Options.UseTextOptions = true;
            this.colEquipName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colEquipName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colEquipName.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colEquipName.Caption = "Equipment Name";
            this.colEquipName.MinWidth = 27;
            this.colEquipName.Name = "colEquipName";
            this.colEquipName.OptionsColumn.AllowEdit = false;
            this.colEquipName.OptionsColumn.AllowFocus = false;
            this.colEquipName.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colEquipName.OptionsColumn.AllowMove = false;
            this.colEquipName.OptionsColumn.AllowSize = false;
            this.colEquipName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colEquipName.OptionsColumn.FixedWidth = true;
            this.colEquipName.Visible = true;
            this.colEquipName.Width = 206;
            // 
            // colEquipType
            // 
            this.colEquipType.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colEquipType.AppearanceHeader.Options.UseForeColor = true;
            this.colEquipType.AppearanceHeader.Options.UseTextOptions = true;
            this.colEquipType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colEquipType.Caption = "Equipment Type";
            this.colEquipType.MinWidth = 25;
            this.colEquipType.Name = "colEquipType";
            this.colEquipType.OptionsColumn.AllowEdit = false;
            this.colEquipType.Visible = true;
            this.colEquipType.Width = 198;
            // 
            // colEquipMake
            // 
            this.colEquipMake.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colEquipMake.AppearanceHeader.Options.UseForeColor = true;
            this.colEquipMake.AppearanceHeader.Options.UseTextOptions = true;
            this.colEquipMake.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colEquipMake.Caption = "Equipment Make";
            this.colEquipMake.MinWidth = 25;
            this.colEquipMake.Name = "colEquipMake";
            this.colEquipMake.OptionsColumn.AllowEdit = false;
            this.colEquipMake.Visible = true;
            this.colEquipMake.Width = 226;
            // 
            // rcWorkplace
            // 
            this.rcWorkplace.AllowKeyTips = false;
            this.rcWorkplace.AllowMdiChildButtons = false;
            this.rcWorkplace.AllowMinimizeRibbon = false;
            this.rcWorkplace.AllowTrimPageText = false;
            this.rcWorkplace.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.rcWorkplace.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.DarkBlue;
            this.rcWorkplace.ExpandCollapseItem.Id = 0;
            this.rcWorkplace.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcWorkplace.ExpandCollapseItem,
            this.rcWorkplace.SearchEditItem,
            this.barButtonItem1,
            this.CancelBtn,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.btnWPAdd,
            this.PrevRepbtn,
            this.barEditItem1,
            this.FLEdit,
            this.AdvEdit,
            this.AuthBtn,
            this.BtnAddDoc,
            this.btnEngInventory,
            this.tbProdMonth,
            this.tbSection,
            this.tbCrew,
            this.tbDpInspecDate,
            this.btnWPDelete,
            this.btnSave,
            this.barbtnClose,
            this.barActivity,
            this.btnAdd,
            this.EditBtn});
            this.rcWorkplace.Location = new System.Drawing.Point(0, 0);
            this.rcWorkplace.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rcWorkplace.MaxItemId = 31;
            this.rcWorkplace.Name = "rcWorkplace";
            this.rcWorkplace.OptionsPageCategories.ShowCaptions = false;
            this.rcWorkplace.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpEquipment});
            this.rcWorkplace.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemRadioGroup1});
            this.rcWorkplace.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcWorkplace.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcWorkplace.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcWorkplace.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.rcWorkplace.ShowToolbarCustomizeItem = false;
            this.rcWorkplace.Size = new System.Drawing.Size(1057, 111);
            this.rcWorkplace.Toolbar.ShowCustomizeItem = false;
            this.rcWorkplace.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Save";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Caption = "Cancel";
            this.CancelBtn.Id = 2;
            this.CancelBtn.Name = "CancelBtn";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "                                                                                 " +
    "        ";
            this.barButtonItem2.Id = 3;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "                               ";
            this.barButtonItem3.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barButtonItem3.Id = 4;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Show";
            this.barButtonItem4.Id = 5;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // btnWPAdd
            // 
            this.btnWPAdd.Caption = "Add";
            this.btnWPAdd.Enabled = false;
            this.btnWPAdd.Id = 7;
            this.btnWPAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnWPAdd.ImageOptions.Image")));
            this.btnWPAdd.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnWPAdd.ImageOptions.LargeImage")));
            this.btnWPAdd.LargeWidth = 50;
            this.btnWPAdd.Name = "btnWPAdd";
            // 
            // PrevRepbtn
            // 
            this.PrevRepbtn.Caption = "Preview Report";
            this.PrevRepbtn.Id = 8;
            this.PrevRepbtn.Name = "PrevRepbtn";
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "barEditItem1";
            this.barEditItem1.Edit = null;
            this.barEditItem1.Id = 9;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // FLEdit
            // 
            this.FLEdit.Caption = "FL  ";
            this.FLEdit.Edit = null;
            this.FLEdit.Enabled = false;
            this.FLEdit.Id = 13;
            this.FLEdit.Name = "FLEdit";
            // 
            // AdvEdit
            // 
            this.AdvEdit.Caption = "Adv";
            this.AdvEdit.Edit = null;
            this.AdvEdit.Enabled = false;
            this.AdvEdit.Id = 14;
            this.AdvEdit.Name = "AdvEdit";
            // 
            // AuthBtn
            // 
            this.AuthBtn.Caption = "Authorize";
            this.AuthBtn.Id = 15;
            this.AuthBtn.Name = "AuthBtn";
            // 
            // BtnAddDoc
            // 
            this.BtnAddDoc.Caption = "Add   Documents";
            this.BtnAddDoc.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.BtnAddDoc.Id = 16;
            this.BtnAddDoc.Name = "BtnAddDoc";
            // 
            // btnEngInventory
            // 
            this.btnEngInventory.Caption = "Engineering Inventory";
            this.btnEngInventory.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.btnEngInventory.Id = 17;
            this.btnEngInventory.Name = "btnEngInventory";
            // 
            // tbProdMonth
            // 
            this.tbProdMonth.Caption = "Month  ";
            this.tbProdMonth.Edit = null;
            this.tbProdMonth.EditWidth = 100;
            this.tbProdMonth.Enabled = false;
            this.tbProdMonth.Id = 18;
            this.tbProdMonth.Name = "tbProdMonth";
            // 
            // tbSection
            // 
            this.tbSection.Caption = "Section";
            this.tbSection.Edit = null;
            this.tbSection.EditWidth = 100;
            this.tbSection.Enabled = false;
            this.tbSection.Id = 19;
            this.tbSection.Name = "tbSection";
            // 
            // tbCrew
            // 
            this.tbCrew.Caption = "Crew    ";
            this.tbCrew.Edit = null;
            this.tbCrew.EditWidth = 190;
            this.tbCrew.Enabled = false;
            this.tbCrew.Id = 20;
            this.tbCrew.Name = "tbCrew";
            // 
            // tbDpInspecDate
            // 
            this.tbDpInspecDate.Caption = "Date     ";
            this.tbDpInspecDate.Edit = null;
            this.tbDpInspecDate.EditWidth = 120;
            this.tbDpInspecDate.Id = 21;
            this.tbDpInspecDate.Name = "tbDpInspecDate";
            // 
            // btnWPDelete
            // 
            this.btnWPDelete.Caption = "Delete";
            this.btnWPDelete.Enabled = false;
            this.btnWPDelete.Id = 22;
            this.btnWPDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnWPDelete.ImageOptions.SvgImage")));
            this.btnWPDelete.LargeWidth = 50;
            this.btnWPDelete.Name = "btnWPDelete";
            // 
            // btnSave
            // 
            this.btnSave.Caption = "Save";
            this.btnSave.Id = 23;
            this.btnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSave.ImageOptions.SvgImage")));
            this.btnSave.LargeWidth = 50;
            this.btnSave.Name = "btnSave";
            // 
            // barbtnClose
            // 
            this.barbtnClose.Caption = "Close";
            this.barbtnClose.Id = 27;
            this.barbtnClose.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.CloseRed;
            this.barbtnClose.Name = "barbtnClose";
            this.barbtnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnClose_ItemClick);
            // 
            // barActivity
            // 
            this.barActivity.Caption = "Activity";
            this.barActivity.Edit = this.repositoryItemRadioGroup1;
            this.barActivity.EditHeight = 45;
            this.barActivity.EditValue = ((short)(0));
            this.barActivity.EditWidth = 120;
            this.barActivity.Id = 28;
            this.barActivity.Name = "barActivity";
            this.barActivity.EditValueChanged += new System.EventHandler(this.barActivity_EditValueChanged);
            // 
            // repositoryItemRadioGroup1
            // 
            this.repositoryItemRadioGroup1.Columns = 1;
            this.repositoryItemRadioGroup1.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(0)), "Stoping"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(1)), "Development")});
            this.repositoryItemRadioGroup1.Name = "repositoryItemRadioGroup1";
            // 
            // btnAdd
            // 
            this.btnAdd.Caption = "Add";
            this.btnAdd.Id = 29;
            this.btnAdd.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.AddBlue1;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAdd_ItemClick);
            // 
            // EditBtn
            // 
            this.EditBtn.Caption = "Edit";
            this.EditBtn.Enabled = false;
            this.EditBtn.Id = 30;
            this.EditBtn.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.EditBlue;
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBtn_ItemClick);
            // 
            // rpEquipment
            // 
            this.rpEquipment.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            this.rpEquipment.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.TruckBlue1;
            this.rpEquipment.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            this.rpEquipment.Name = "rpEquipment";
            this.rpEquipment.Text = " Equipment Setup";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnAdd);
            this.ribbonPageGroup2.ItemLinks.Add(this.EditBtn);
            this.ribbonPageGroup2.ItemLinks.Add(this.barbtnClose);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Options";
            // 
            // repositoryItemComboStoppages
            // 
            this.repositoryItemComboStoppages.AutoHeight = false;
            this.repositoryItemComboStoppages.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboStoppages.Items.AddRange(new object[] {
            "Temporary Stoppage",
            "Permenant Stoppage"});
            this.repositoryItemComboStoppages.Name = "repositoryItemComboStoppages";
            // 
            // ucEquipmentType
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcEquipmentType);
            this.Controls.Add(this.rcWorkplace);
            this.Name = "ucEquipmentType";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(1057, 686);
            this.Load += new System.EventHandler(this.ucStopWorkplace_Load);
            this.Controls.SetChildIndex(this.rcWorkplace, 0);
            this.Controls.SetChildIndex(this.gcEquipmentType, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcEquipmentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEquipmentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcWorkplace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboStoppages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl rcWorkplace;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem CancelBtn;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        public DevExpress.XtraBars.BarButtonItem btnWPAdd;
        private DevExpress.XtraBars.BarButtonItem PrevRepbtn;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraBars.BarEditItem FLEdit;
        private DevExpress.XtraBars.BarEditItem AdvEdit;
        public DevExpress.XtraBars.BarButtonItem AuthBtn;
        public DevExpress.XtraBars.BarButtonItem BtnAddDoc;
        public DevExpress.XtraBars.BarButtonItem btnEngInventory;
        private DevExpress.XtraBars.BarEditItem tbProdMonth;
        private DevExpress.XtraBars.BarEditItem tbSection;
        public DevExpress.XtraBars.BarEditItem tbCrew;
        private DevExpress.XtraBars.BarEditItem tbDpInspecDate;
        private DevExpress.XtraBars.BarButtonItem btnWPDelete;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.BarButtonItem barbtnClose;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpEquipment;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraGrid.GridControl gcEquipmentType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvEquipmentType;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEquipID;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEquipName;
        private DevExpress.XtraBars.BarEditItem barActivity;
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup repositoryItemRadioGroup1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEquipType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboStoppages;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvDetail;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWPIDDetail;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWPDetail;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDist;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colStoppageType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDetailComments;
        private DevExpress.XtraBars.BarButtonItem btnAdd;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colStoppageDate;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEquipMake;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEquipValid;
        private DevExpress.XtraBars.BarButtonItem EditBtn;
    }
}
