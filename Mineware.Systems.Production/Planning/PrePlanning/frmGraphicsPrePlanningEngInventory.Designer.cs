namespace Mineware.Systems.Production.Planning
{
    partial class frmGraphicsPrePlanningEngInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGraphicsPrePlanningEngInventory));
            this.panel1 = new System.Windows.Forms.Panel();
            this.gcEngInv = new DevExpress.XtraGrid.GridControl();
            this.gvEngInv = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand41 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colEquipmentNumber = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colEquipmentType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colSize = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colManufacturer = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand42 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand43 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand44 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand45 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand46 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand47 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand48 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand49 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand50 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.repositoryItemImageComboBox4 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.EngInvAddPanel = new System.Windows.Forms.Panel();
            this.cmbManufacturer = new System.Windows.Forms.ComboBox();
            this.cmbEquipmentType = new System.Windows.Forms.ComboBox();
            this.txtSize = new DevExpress.XtraEditors.TextEdit();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.txtMotorNumber = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcEngInv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEngInv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox4)).BeginInit();
            this.EngInvAddPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMotorNumber.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1080, 29);
            this.panel1.TabIndex = 0;
            // 
            // gcEngInv
            // 
            this.gcEngInv.Cursor = System.Windows.Forms.Cursors.Default;
            this.gcEngInv.Dock = System.Windows.Forms.DockStyle.Left;
            this.gcEngInv.EmbeddedNavigator.Appearance.Options.UseTextOptions = true;
            this.gcEngInv.EmbeddedNavigator.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gcEngInv.Location = new System.Drawing.Point(0, 29);
            this.gcEngInv.MainView = this.gvEngInv;
            this.gcEngInv.Name = "gcEngInv";
            this.gcEngInv.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageComboBox4});
            this.gcEngInv.Size = new System.Drawing.Size(716, 498);
            this.gcEngInv.TabIndex = 217;
            this.gcEngInv.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvEngInv});
            // 
            // gvEngInv
            // 
            this.gvEngInv.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gvEngInv.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvEngInv.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvEngInv.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gvEngInv.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand41,
            this.gridBand42,
            this.gridBand43,
            this.gridBand44,
            this.gridBand45,
            this.gridBand46,
            this.gridBand47,
            this.gridBand48,
            this.gridBand49,
            this.gridBand50});
            this.gvEngInv.ColumnPanelRowHeight = 21;
            this.gvEngInv.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colEquipmentType,
            this.colEquipmentNumber,
            this.colSize,
            this.colManufacturer,
            this.colID});
            this.gvEngInv.DetailHeight = 372;
            this.gvEngInv.GridControl = this.gcEngInv;
            this.gvEngInv.Name = "gvEngInv";
            this.gvEngInv.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvEngInv.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvEngInv.OptionsView.AllowCellMerge = true;
            this.gvEngInv.OptionsView.ColumnAutoWidth = false;
            this.gvEngInv.OptionsView.ShowBands = false;
            this.gvEngInv.OptionsView.ShowGroupPanel = false;
            this.gvEngInv.OptionsView.ShowIndicator = false;
            this.gvEngInv.DoubleClick += new System.EventHandler(this.gvEngInv_DoubleClick);
            // 
            // gridBand41
            // 
            this.gridBand41.Columns.Add(this.colEquipmentNumber);
            this.gridBand41.Columns.Add(this.colEquipmentType);
            this.gridBand41.Columns.Add(this.colSize);
            this.gridBand41.Columns.Add(this.colManufacturer);
            this.gridBand41.Columns.Add(this.colID);
            this.gridBand41.Name = "gridBand41";
            this.gridBand41.VisibleIndex = 0;
            this.gridBand41.Width = 707;
            // 
            // colEquipmentNumber
            // 
            this.colEquipmentNumber.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colEquipmentNumber.AppearanceHeader.Options.UseForeColor = true;
            this.colEquipmentNumber.Caption = "Equipment Number";
            this.colEquipmentNumber.MinWidth = 25;
            this.colEquipmentNumber.Name = "colEquipmentNumber";
            this.colEquipmentNumber.OptionsColumn.AllowEdit = false;
            this.colEquipmentNumber.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colEquipmentNumber.OptionsColumn.AllowMove = false;
            this.colEquipmentNumber.OptionsColumn.AllowSize = false;
            this.colEquipmentNumber.OptionsColumn.FixedWidth = true;
            this.colEquipmentNumber.OptionsFilter.AllowFilter = false;
            this.colEquipmentNumber.Visible = true;
            this.colEquipmentNumber.Width = 133;
            // 
            // colEquipmentType
            // 
            this.colEquipmentType.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colEquipmentType.AppearanceHeader.Options.UseForeColor = true;
            this.colEquipmentType.Caption = "Equipment Type";
            this.colEquipmentType.MinWidth = 25;
            this.colEquipmentType.Name = "colEquipmentType";
            this.colEquipmentType.OptionsColumn.AllowEdit = false;
            this.colEquipmentType.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colEquipmentType.OptionsColumn.AllowMove = false;
            this.colEquipmentType.OptionsColumn.AllowSize = false;
            this.colEquipmentType.OptionsColumn.FixedWidth = true;
            this.colEquipmentType.OptionsFilter.AllowFilter = false;
            this.colEquipmentType.Visible = true;
            this.colEquipmentType.Width = 307;
            // 
            // colSize
            // 
            this.colSize.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colSize.AppearanceHeader.Options.UseForeColor = true;
            this.colSize.Caption = "Size";
            this.colSize.MinWidth = 25;
            this.colSize.Name = "colSize";
            this.colSize.OptionsColumn.AllowEdit = false;
            this.colSize.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colSize.OptionsColumn.AllowMove = false;
            this.colSize.OptionsColumn.AllowSize = false;
            this.colSize.OptionsColumn.FixedWidth = true;
            this.colSize.OptionsFilter.AllowFilter = false;
            this.colSize.Visible = true;
            this.colSize.Width = 67;
            // 
            // colManufacturer
            // 
            this.colManufacturer.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colManufacturer.AppearanceHeader.Options.UseForeColor = true;
            this.colManufacturer.Caption = "Manufacturer";
            this.colManufacturer.MinWidth = 25;
            this.colManufacturer.Name = "colManufacturer";
            this.colManufacturer.OptionsColumn.AllowEdit = false;
            this.colManufacturer.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colManufacturer.OptionsColumn.AllowMove = false;
            this.colManufacturer.OptionsColumn.AllowSize = false;
            this.colManufacturer.OptionsColumn.FixedWidth = true;
            this.colManufacturer.OptionsFilter.AllowFilter = false;
            this.colManufacturer.Visible = true;
            this.colManufacturer.Width = 200;
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.MinWidth = 25;
            this.colID.Name = "colID";
            this.colID.OptionsColumn.AllowEdit = false;
            this.colID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colID.OptionsColumn.AllowMove = false;
            this.colID.OptionsColumn.AllowSize = false;
            this.colID.OptionsColumn.FixedWidth = true;
            this.colID.OptionsFilter.AllowFilter = false;
            this.colID.Width = 107;
            // 
            // gridBand42
            // 
            this.gridBand42.Name = "gridBand42";
            this.gridBand42.Visible = false;
            this.gridBand42.VisibleIndex = -1;
            this.gridBand42.Width = 20;
            // 
            // gridBand43
            // 
            this.gridBand43.Caption = "MICRO SEQUENCE";
            this.gridBand43.Name = "gridBand43";
            this.gridBand43.Visible = false;
            this.gridBand43.VisibleIndex = -1;
            this.gridBand43.Width = 20;
            // 
            // gridBand44
            // 
            this.gridBand44.Caption = "FACE SHAPE";
            this.gridBand44.Name = "gridBand44";
            this.gridBand44.Visible = false;
            this.gridBand44.VisibleIndex = -1;
            this.gridBand44.Width = 20;
            // 
            // gridBand45
            // 
            this.gridBand45.Name = "gridBand45";
            this.gridBand45.Visible = false;
            this.gridBand45.VisibleIndex = -1;
            this.gridBand45.Width = 20;
            // 
            // gridBand46
            // 
            this.gridBand46.Caption = "STRATA CONTROL";
            this.gridBand46.Name = "gridBand46";
            this.gridBand46.Visible = false;
            this.gridBand46.VisibleIndex = -1;
            this.gridBand46.Width = 20;
            // 
            // gridBand47
            // 
            this.gridBand47.Name = "gridBand47";
            this.gridBand47.Visible = false;
            this.gridBand47.VisibleIndex = -1;
            this.gridBand47.Width = 20;
            // 
            // gridBand48
            // 
            this.gridBand48.Caption = "PANEL RATING";
            this.gridBand48.Name = "gridBand48";
            this.gridBand48.Visible = false;
            this.gridBand48.VisibleIndex = -1;
            this.gridBand48.Width = 20;
            // 
            // gridBand49
            // 
            this.gridBand49.Name = "gridBand49";
            this.gridBand49.Visible = false;
            this.gridBand49.VisibleIndex = -1;
            this.gridBand49.Width = 20;
            // 
            // gridBand50
            // 
            this.gridBand50.Name = "gridBand50";
            this.gridBand50.Visible = false;
            this.gridBand50.VisibleIndex = -1;
            this.gridBand50.Width = 509;
            // 
            // repositoryItemImageComboBox4
            // 
            this.repositoryItemImageComboBox4.AutoHeight = false;
            this.repositoryItemImageComboBox4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox4.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", "Y", 0),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", "N", 1)});
            this.repositoryItemImageComboBox4.Name = "repositoryItemImageComboBox4";
            // 
            // EngInvAddPanel
            // 
            this.EngInvAddPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.EngInvAddPanel.Controls.Add(this.cmbManufacturer);
            this.EngInvAddPanel.Controls.Add(this.cmbEquipmentType);
            this.EngInvAddPanel.Controls.Add(this.txtSize);
            this.EngInvAddPanel.Controls.Add(this.txtMotorNumber);
            this.EngInvAddPanel.Controls.Add(this.label5);
            this.EngInvAddPanel.Controls.Add(this.label4);
            this.EngInvAddPanel.Controls.Add(this.label3);
            this.EngInvAddPanel.Controls.Add(this.label2);
            this.EngInvAddPanel.Controls.Add(this.ribbonControl1);
            this.EngInvAddPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EngInvAddPanel.Location = new System.Drawing.Point(716, 29);
            this.EngInvAddPanel.Name = "EngInvAddPanel";
            this.EngInvAddPanel.Size = new System.Drawing.Size(364, 498);
            this.EngInvAddPanel.TabIndex = 218;
            // 
            // cmbManufacturer
            // 
            this.cmbManufacturer.FormattingEnabled = true;
            this.cmbManufacturer.Items.AddRange(new object[] {
            "",
            "Pillman/Joesten",
            "Orotech",
            "Pillman",
            "Trident SA",
            "Carleton Eng.",
            "Actom/Alstom",
            "Udor"});
            this.cmbManufacturer.Location = new System.Drawing.Point(90, 368);
            this.cmbManufacturer.Name = "cmbManufacturer";
            this.cmbManufacturer.Size = new System.Drawing.Size(206, 25);
            this.cmbManufacturer.TabIndex = 11;
            // 
            // cmbEquipmentType
            // 
            this.cmbEquipmentType.FormattingEnabled = true;
            this.cmbEquipmentType.Location = new System.Drawing.Point(90, 178);
            this.cmbEquipmentType.Name = "cmbEquipmentType";
            this.cmbEquipmentType.Size = new System.Drawing.Size(206, 25);
            this.cmbEquipmentType.TabIndex = 10;
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(90, 300);
            this.txtSize.MenuManager = this.ribbonControl1;
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(206, 24);
            this.txtSize.TabIndex = 9;
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
            this.btnAdd,
            this.btnClose});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 26;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.OptionsPageCategories.ShowCaptions = false;
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(364, 110);
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // btnAdd
            // 
            this.btnAdd.Caption = "Add";
            this.btnAdd.Id = 24;
            this.btnAdd.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.AddBlue22;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAdd_ItemClick);
            // 
            // btnClose
            // 
            this.btnClose.Caption = "Close";
            this.btnClose.Id = 25;
            this.btnClose.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.CloseRed;
            this.btnClose.Name = "btnClose";
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnAdd);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnClose);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Actions";
            // 
            // txtMotorNumber
            // 
            this.txtMotorNumber.Location = new System.Drawing.Point(90, 244);
            this.txtMotorNumber.MenuManager = this.ribbonControl1;
            this.txtMotorNumber.Name = "txtMotorNumber";
            this.txtMotorNumber.Size = new System.Drawing.Size(206, 24);
            this.txtMotorNumber.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(87, 346);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 19);
            this.label5.TabIndex = 5;
            this.label5.Text = "Manufacturer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 278);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 19);
            this.label4.TabIndex = 4;
            this.label4.Text = "Size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "Equipent Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Equipment Type";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelControl2.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelControl2.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelControl2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.InformationBlue;
            this.labelControl2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.labelControl2.Location = new System.Drawing.Point(0, 0);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(211, 24);
            this.labelControl2.TabIndex = 191;
            this.labelControl2.Text = "DQlik Equipment Type to Delete";
            // 
            // frmGraphicsPrePlanningEngInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 527);
            this.Controls.Add(this.EngInvAddPanel);
            this.Controls.Add(this.gcEngInv);
            this.Controls.Add(this.panel1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frmGraphicsPrePlanningEngInventory.IconOptions.Image")));
            this.Name = "frmGraphicsPrePlanningEngInventory";
            this.Text = "Engineering Inventory";
            this.Load += new System.EventHandler(this.frmGraphicsPrePlanningEngInventory_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcEngInv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEngInv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox4)).EndInit();
            this.EngInvAddPanel.ResumeLayout(false);
            this.EngInvAddPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMotorNumber.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gcEngInv;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvEngInv;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand41;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEquipmentNumber;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEquipmentType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colSize;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colManufacturer;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colID;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand42;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand43;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand44;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand45;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand46;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand47;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand48;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand49;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand50;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox4;
        private System.Windows.Forms.Panel EngInvAddPanel;
        private DevExpress.XtraEditors.TextEdit txtSize;
        public DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem btnAdd;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraEditors.TextEdit txtMotorNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbManufacturer;
        private System.Windows.Forms.ComboBox cmbEquipmentType;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}