namespace Mineware.Systems.Production.Departmental.Vamping
{
    partial class frmAddOldWp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddOldWp));
            this.WPAddPnl = new System.Windows.Forms.Panel();
            this.gcOldWorkplaces = new DevExpress.XtraGrid.GridControl();
            this.gvOldWorkplaces = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWPID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RCRockEngineering = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.RockEnginSavebtn = new DevExpress.XtraBars.BarButtonItem();
            this.RockEnginAddImagebtn = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.VampwplistBox = new DevExpress.XtraEditors.ListBoxControl();
            this.searchControl1 = new DevExpress.XtraEditors.SearchControl();
            this.label132 = new System.Windows.Forms.Label();
            this.WPlinetxt = new System.Windows.Forms.TextBox();
            this.label134 = new System.Windows.Forms.Label();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.WPDescNewTxt = new System.Windows.Forms.TextBox();
            this.label138 = new System.Windows.Forms.Label();
            this.WPLevelcmb = new System.Windows.Forms.ComboBox();
            this.label139 = new System.Windows.Forms.Label();
            this.NewWpLbl = new System.Windows.Forms.Label();
            this.WPAddPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcOldWorkplaces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOldWorkplaces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RCRockEngineering)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VampwplistBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // WPAddPnl
            // 
            this.WPAddPnl.BackColor = System.Drawing.Color.White;
            this.WPAddPnl.Controls.Add(this.gcOldWorkplaces);
            this.WPAddPnl.Controls.Add(this.VampwplistBox);
            this.WPAddPnl.Controls.Add(this.searchControl1);
            this.WPAddPnl.Controls.Add(this.label132);
            this.WPAddPnl.Controls.Add(this.WPlinetxt);
            this.WPAddPnl.Controls.Add(this.label134);
            this.WPAddPnl.Controls.Add(this.textEdit1);
            this.WPAddPnl.Controls.Add(this.WPDescNewTxt);
            this.WPAddPnl.Controls.Add(this.label138);
            this.WPAddPnl.Controls.Add(this.WPLevelcmb);
            this.WPAddPnl.Controls.Add(this.label139);
            this.WPAddPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WPAddPnl.Location = new System.Drawing.Point(0, 87);
            this.WPAddPnl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.WPAddPnl.Name = "WPAddPnl";
            this.WPAddPnl.Size = new System.Drawing.Size(743, 552);
            this.WPAddPnl.TabIndex = 62;
            // 
            // gcOldWorkplaces
            // 
            this.gcOldWorkplaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcOldWorkplaces.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcOldWorkplaces.Location = new System.Drawing.Point(0, 0);
            this.gcOldWorkplaces.MainView = this.gvOldWorkplaces;
            this.gcOldWorkplaces.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcOldWorkplaces.MenuManager = this.RCRockEngineering;
            this.gcOldWorkplaces.Name = "gcOldWorkplaces";
            this.gcOldWorkplaces.Size = new System.Drawing.Size(743, 552);
            this.gcOldWorkplaces.TabIndex = 66;
            this.gcOldWorkplaces.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOldWorkplaces});
            // 
            // gvOldWorkplaces
            // 
            this.gvOldWorkplaces.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDescription,
            this.colActivity,
            this.colWPID});
            this.gvOldWorkplaces.DetailHeight = 458;
            this.gvOldWorkplaces.FixedLineWidth = 3;
            this.gvOldWorkplaces.GridControl = this.gcOldWorkplaces;
            this.gvOldWorkplaces.Name = "gvOldWorkplaces";
            this.gvOldWorkplaces.OptionsFind.AlwaysVisible = true;
            this.gvOldWorkplaces.OptionsSelection.MultiSelect = true;
            this.gvOldWorkplaces.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gvOldWorkplaces.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.False;
            this.gvOldWorkplaces.OptionsView.ShowGroupPanel = false;
            // 
            // colDescription
            // 
            this.colDescription.Caption = "Workplace Description";
            this.colDescription.FieldName = "Description";
            this.colDescription.MinWidth = 23;
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.AllowEdit = false;
            this.colDescription.OptionsColumn.ReadOnly = true;
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 2;
            this.colDescription.Width = 87;
            // 
            // colActivity
            // 
            this.colActivity.Caption = "Activity";
            this.colActivity.FieldName = "Activity";
            this.colActivity.MaxWidth = 64;
            this.colActivity.MinWidth = 64;
            this.colActivity.Name = "colActivity";
            this.colActivity.OptionsColumn.AllowEdit = false;
            this.colActivity.OptionsColumn.ReadOnly = true;
            this.colActivity.Width = 64;
            // 
            // colWPID
            // 
            this.colWPID.Caption = "Workplace ID";
            this.colWPID.FieldName = "WorkplaceID";
            this.colWPID.MaxWidth = 99;
            this.colWPID.MinWidth = 99;
            this.colWPID.Name = "colWPID";
            this.colWPID.OptionsColumn.AllowEdit = false;
            this.colWPID.OptionsColumn.ReadOnly = true;
            this.colWPID.Visible = true;
            this.colWPID.VisibleIndex = 1;
            this.colWPID.Width = 99;
            // 
            // RCRockEngineering
            // 
            this.RCRockEngineering.AllowKeyTips = false;
            this.RCRockEngineering.AllowMdiChildButtons = false;
            this.RCRockEngineering.AllowMinimizeRibbon = false;
            this.RCRockEngineering.AllowTrimPageText = false;
            this.RCRockEngineering.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.DarkBlue;
            this.RCRockEngineering.DrawGroupsBorderMode = DevExpress.Utils.DefaultBoolean.False;
            this.RCRockEngineering.ExpandCollapseItem.Id = 0;
            this.RCRockEngineering.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.RCRockEngineering.ExpandCollapseItem,
            this.barButtonItem1,
            this.RockEnginSavebtn,
            this.RockEnginAddImagebtn,
            this.RCRockEngineering.SearchEditItem});
            this.RCRockEngineering.Location = new System.Drawing.Point(0, 0);
            this.RCRockEngineering.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RCRockEngineering.MaxItemId = 8;
            this.RCRockEngineering.Name = "RCRockEngineering";
            this.RCRockEngineering.OptionsPageCategories.ShowCaptions = false;
            this.RCRockEngineering.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.RCRockEngineering.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.RCRockEngineering.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.RCRockEngineering.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.RCRockEngineering.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.RCRockEngineering.ShowToolbarCustomizeItem = false;
            this.RCRockEngineering.Size = new System.Drawing.Size(743, 87);
            this.RCRockEngineering.Toolbar.ShowCustomizeItem = false;
            this.RCRockEngineering.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            this.RCRockEngineering.TransparentEditorsMode = DevExpress.Utils.DefaultBoolean.False;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "                                         ";
            this.barButtonItem1.Id = 3;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // RockEnginSavebtn
            // 
            this.RockEnginSavebtn.Caption = "Save";
            this.RockEnginSavebtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.RockEnginSavebtn.Id = 5;
            this.RockEnginSavebtn.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.RockEnginSavebtn.Name = "RockEnginSavebtn";
            this.RockEnginSavebtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RockEnginSavebtn_ItemClick);
            // 
            // RockEnginAddImagebtn
            // 
            this.RockEnginAddImagebtn.Caption = "Close";
            this.RockEnginAddImagebtn.Id = 7;
            this.RockEnginAddImagebtn.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.close;
            this.RockEnginAddImagebtn.Name = "RockEnginAddImagebtn";
            this.RockEnginAddImagebtn.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.RockEnginAddImagebtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RockEnginAddImagebtn_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.RockEnginSavebtn);
            this.ribbonPageGroup1.ItemLinks.Add(this.RockEnginAddImagebtn);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // VampwplistBox
            // 
            this.VampwplistBox.Location = new System.Drawing.Point(3, 46);
            this.VampwplistBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VampwplistBox.Name = "VampwplistBox";
            this.VampwplistBox.Size = new System.Drawing.Size(301, 352);
            this.VampwplistBox.TabIndex = 65;
            this.VampwplistBox.SelectedIndexChanged += new System.EventHandler(this.VampwplistBox_SelectedIndexChanged);
            // 
            // searchControl1
            // 
            this.searchControl1.Client = this.VampwplistBox;
            this.searchControl1.Location = new System.Drawing.Point(68, 12);
            this.searchControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.searchControl1.MenuManager = this.RCRockEngineering;
            this.searchControl1.Name = "searchControl1";
            this.searchControl1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.searchControl1.Properties.Client = this.VampwplistBox;
            this.searchControl1.Properties.ShowDefaultButtonsMode = DevExpress.XtraEditors.Repository.ShowDefaultButtonsMode.AutoShowClear;
            this.searchControl1.Size = new System.Drawing.Size(237, 24);
            this.searchControl1.TabIndex = 64;
            this.searchControl1.TextChanged += new System.EventHandler(this.SearchControl1_TextChanged);
            // 
            // label132
            // 
            this.label132.AutoSize = true;
            this.label132.Location = new System.Drawing.Point(168, 341);
            this.label132.Name = "label132";
            this.label132.Size = new System.Drawing.Size(34, 19);
            this.label132.TabIndex = 42;
            this.label132.Text = "Line";
            this.label132.Visible = false;
            // 
            // WPlinetxt
            // 
            this.WPlinetxt.BackColor = System.Drawing.Color.White;
            this.WPlinetxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.WPlinetxt.Location = new System.Drawing.Point(311, 337);
            this.WPlinetxt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.WPlinetxt.MaxLength = 50;
            this.WPlinetxt.Name = "WPlinetxt";
            this.WPlinetxt.Size = new System.Drawing.Size(303, 25);
            this.WPlinetxt.TabIndex = 41;
            this.WPlinetxt.Visible = false;
            // 
            // label134
            // 
            this.label134.AutoSize = true;
            this.label134.Location = new System.Drawing.Point(59, 544);
            this.label134.Name = "label134";
            this.label134.Size = new System.Drawing.Size(64, 19);
            this.label134.TabIndex = 37;
            this.label134.Text = "Distance:";
            this.label134.Visible = false;
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "0.0";
            this.textEdit1.Location = new System.Drawing.Point(59, 569);
            this.textEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.Appearance.Options.UseTextOptions = true;
            this.textEdit1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.textEdit1.Properties.Mask.EditMask = "n1";
            this.textEdit1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.textEdit1.Size = new System.Drawing.Size(85, 24);
            this.textEdit1.TabIndex = 36;
            this.textEdit1.Visible = false;
            // 
            // WPDescNewTxt
            // 
            this.WPDescNewTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.WPDescNewTxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.WPDescNewTxt.Location = new System.Drawing.Point(311, 284);
            this.WPDescNewTxt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.WPDescNewTxt.Name = "WPDescNewTxt";
            this.WPDescNewTxt.ReadOnly = true;
            this.WPDescNewTxt.Size = new System.Drawing.Size(303, 25);
            this.WPDescNewTxt.TabIndex = 6;
            this.WPDescNewTxt.Visible = false;
            // 
            // label138
            // 
            this.label138.AutoSize = true;
            this.label138.Location = new System.Drawing.Point(194, 245);
            this.label138.Name = "label138";
            this.label138.Size = new System.Drawing.Size(40, 19);
            this.label138.TabIndex = 5;
            this.label138.Text = "Level";
            this.label138.Visible = false;
            // 
            // WPLevelcmb
            // 
            this.WPLevelcmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WPLevelcmb.FormattingEnabled = true;
            this.WPLevelcmb.Location = new System.Drawing.Point(311, 234);
            this.WPLevelcmb.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.WPLevelcmb.Name = "WPLevelcmb";
            this.WPLevelcmb.Size = new System.Drawing.Size(287, 25);
            this.WPLevelcmb.TabIndex = 4;
            this.WPLevelcmb.Visible = false;
            // 
            // label139
            // 
            this.label139.AutoSize = true;
            this.label139.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label139.Location = new System.Drawing.Point(14, 16);
            this.label139.Name = "label139";
            this.label139.Size = new System.Drawing.Size(51, 19);
            this.label139.TabIndex = 3;
            this.label139.Text = "Search";
            // 
            // NewWpLbl
            // 
            this.NewWpLbl.AutoSize = true;
            this.NewWpLbl.Location = new System.Drawing.Point(194, 44);
            this.NewWpLbl.Name = "NewWpLbl";
            this.NewWpLbl.Size = new System.Drawing.Size(145, 19);
            this.NewWpLbl.TabIndex = 7;
            this.NewWpLbl.Text = "Workplace Description";
            this.NewWpLbl.Visible = false;
            // 
            // frmAddOldWp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 639);
            this.Controls.Add(this.NewWpLbl);
            this.Controls.Add(this.WPAddPnl);
            this.Controls.Add(this.RCRockEngineering);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("frmAddOldWp.IconOptions.Icon")));
            this.Name = "frmAddOldWp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Old Workplaces";
            this.Load += new System.EventHandler(this.frmAddOldWp_Load);
            this.WPAddPnl.ResumeLayout(false);
            this.WPAddPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcOldWorkplaces)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOldWorkplaces)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RCRockEngineering)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VampwplistBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel WPAddPnl;
        private System.Windows.Forms.Label label132;
        private System.Windows.Forms.TextBox WPlinetxt;
        private System.Windows.Forms.Label label134;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private System.Windows.Forms.Label NewWpLbl;
        private System.Windows.Forms.TextBox WPDescNewTxt;
        private System.Windows.Forms.Label label138;
        public System.Windows.Forms.ComboBox WPLevelcmb;
        private System.Windows.Forms.Label label139;
        private DevExpress.XtraBars.Ribbon.RibbonControl RCRockEngineering;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem RockEnginSavebtn;
        private DevExpress.XtraBars.BarButtonItem RockEnginAddImagebtn;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraEditors.ListBoxControl VampwplistBox;
        private DevExpress.XtraEditors.SearchControl searchControl1;
        private DevExpress.XtraGrid.GridControl gcOldWorkplaces;
        private DevExpress.XtraGrid.Views.Grid.GridView gvOldWorkplaces;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colActivity;
        private DevExpress.XtraGrid.Columns.GridColumn colWPID;
    }
}