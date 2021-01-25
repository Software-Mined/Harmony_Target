namespace Mineware.Systems.Production.SysAdmin
{
    partial class ucStopWorkplace
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucStopWorkplace));
            this.gvDetail = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colWPIDDetail = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWPDetail = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colStoppageDate = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDist = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colStoppageType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDetailComments = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcWorkplaces = new DevExpress.XtraGrid.GridControl();
            this.gvWorkplaces = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colWpID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWpName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colStopType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colComments = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
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
            this.rpWorkplace = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.repositoryItemComboStoppages = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.btnHelp = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcWorkplaces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWorkplaces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcWorkplace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboStoppages)).BeginInit();
            this.SuspendLayout();
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
            this.gvDetail.GridControl = this.gcWorkplaces;
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
            // gcWorkplaces
            // 
            this.gcWorkplaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcWorkplaces.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            gridLevelNode1.LevelTemplate = this.gvDetail;
            gridLevelNode1.RelationName = "Level1";
            this.gcWorkplaces.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcWorkplaces.Location = new System.Drawing.Point(0, 111);
            this.gcWorkplaces.MainView = this.gvWorkplaces;
            this.gcWorkplaces.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcWorkplaces.MenuManager = this.rcWorkplace;
            this.gcWorkplaces.Name = "gcWorkplaces";
            this.gcWorkplaces.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboStoppages});
            this.gcWorkplaces.Size = new System.Drawing.Size(1057, 575);
            this.gcWorkplaces.TabIndex = 2;
            this.gcWorkplaces.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvWorkplaces,
            this.gvDetail});
            // 
            // gvWorkplaces
            // 
            this.gvWorkplaces.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.gvWorkplaces.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colWpID,
            this.colWpName,
            this.colStopType,
            this.colComments});
            this.gvWorkplaces.DetailHeight = 431;
            this.gvWorkplaces.GridControl = this.gcWorkplaces;
            this.gvWorkplaces.Name = "gvWorkplaces";
            this.gvWorkplaces.OptionsView.ShowAutoFilterRow = true;
            this.gvWorkplaces.OptionsView.ShowBands = false;
            this.gvWorkplaces.OptionsView.ShowGroupPanel = false;
            this.gvWorkplaces.OptionsView.ShowIndicator = false;
            this.gvWorkplaces.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gvWorkplaces_RowCellClick);
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Columns.Add(this.colWpID);
            this.gridBand1.Columns.Add(this.colWpName);
            this.gridBand1.Columns.Add(this.colStopType);
            this.gridBand1.Columns.Add(this.colComments);
            this.gridBand1.MinWidth = 14;
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 1055;
            // 
            // colWpID
            // 
            this.colWpID.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colWpID.AppearanceCell.Options.UseFont = true;
            this.colWpID.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colWpID.AppearanceHeader.Options.UseFont = true;
            this.colWpID.AppearanceHeader.Options.UseForeColor = true;
            this.colWpID.AppearanceHeader.Options.UseTextOptions = true;
            this.colWpID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWpID.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWpID.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWpID.Caption = "WorkplaceID";
            this.colWpID.MinWidth = 27;
            this.colWpID.Name = "colWpID";
            this.colWpID.OptionsColumn.AllowEdit = false;
            this.colWpID.OptionsColumn.AllowFocus = false;
            this.colWpID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colWpID.OptionsColumn.AllowMove = false;
            this.colWpID.OptionsColumn.AllowSize = false;
            this.colWpID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colWpID.OptionsColumn.FixedWidth = true;
            this.colWpID.Visible = true;
            this.colWpID.Width = 83;
            // 
            // colWpName
            // 
            this.colWpName.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colWpName.AppearanceCell.Options.UseFont = true;
            this.colWpName.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colWpName.AppearanceHeader.Options.UseFont = true;
            this.colWpName.AppearanceHeader.Options.UseForeColor = true;
            this.colWpName.AppearanceHeader.Options.UseTextOptions = true;
            this.colWpName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWpName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWpName.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWpName.Caption = "Workplace Name";
            this.colWpName.MinWidth = 27;
            this.colWpName.Name = "colWpName";
            this.colWpName.OptionsColumn.AllowEdit = false;
            this.colWpName.OptionsColumn.AllowFocus = false;
            this.colWpName.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colWpName.OptionsColumn.AllowMove = false;
            this.colWpName.OptionsColumn.AllowSize = false;
            this.colWpName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colWpName.OptionsColumn.FixedWidth = true;
            this.colWpName.Visible = true;
            this.colWpName.Width = 196;
            // 
            // colStopType
            // 
            this.colStopType.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colStopType.AppearanceHeader.Options.UseForeColor = true;
            this.colStopType.Caption = "Last Stoppage Type";
            this.colStopType.MinWidth = 25;
            this.colStopType.Name = "colStopType";
            this.colStopType.OptionsColumn.AllowEdit = false;
            this.colStopType.Visible = true;
            this.colStopType.Width = 210;
            // 
            // colComments
            // 
            this.colComments.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colComments.AppearanceHeader.Options.UseForeColor = true;
            this.colComments.Caption = "Comments";
            this.colComments.MinWidth = 25;
            this.colComments.Name = "colComments";
            this.colComments.OptionsColumn.AllowEdit = false;
            this.colComments.OptionsFilter.AllowAutoFilter = false;
            this.colComments.OptionsFilter.AllowFilter = false;
            this.colComments.Visible = true;
            this.colComments.Width = 566;
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
            this.btnHelp});
            this.rcWorkplace.Location = new System.Drawing.Point(0, 0);
            this.rcWorkplace.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rcWorkplace.MaxItemId = 31;
            this.rcWorkplace.Name = "rcWorkplace";
            this.rcWorkplace.OptionsPageCategories.ShowCaptions = false;
            this.rcWorkplace.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpWorkplace});
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
            // rpWorkplace
            // 
            this.rpWorkplace.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.rpWorkplace.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("rpWorkplace.ImageOptions.SvgImage")));
            this.rpWorkplace.ImageOptions.SvgImageSize = new System.Drawing.Size(22, 22);
            this.rpWorkplace.Name = "rpWorkplace";
            this.rpWorkplace.Text = " Stop Workplaces";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barActivity);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Filter";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnAdd);
            this.ribbonPageGroup2.ItemLinks.Add(this.barbtnClose);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnHelp);
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
            // btnHelp
            // 
            this.btnHelp.Caption = "Help";
            this.btnHelp.Id = 30;
            this.btnHelp.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem5.ImageOptions.SvgImage")));
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHelp_ItemClick);
            // 
            // ucStopWorkplace
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcWorkplaces);
            this.Controls.Add(this.rcWorkplace);
            this.Name = "ucStopWorkplace";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(1057, 686);
            this.Load += new System.EventHandler(this.ucStopWorkplace_Load);
            this.Controls.SetChildIndex(this.rcWorkplace, 0);
            this.Controls.SetChildIndex(this.gcWorkplaces, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcWorkplaces)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWorkplaces)).EndInit();
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
        private DevExpress.XtraBars.Ribbon.RibbonPage rpWorkplace;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraGrid.GridControl gcWorkplaces;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvWorkplaces;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWpID;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWpName;
        private DevExpress.XtraBars.BarEditItem barActivity;
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup repositoryItemRadioGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colStopType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colComments;
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
        private DevExpress.XtraBars.BarButtonItem btnHelp;
    }
}
