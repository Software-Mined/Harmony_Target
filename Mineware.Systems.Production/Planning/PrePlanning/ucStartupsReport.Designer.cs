namespace Mineware.Systems.Production.Planning
{
    partial class ucStartupsReport
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
            this.pnlStrtReport = new System.Windows.Forms.Panel();
            this.pcReport = new FastReport.Preview.PreviewControl();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.tbProdMonth = new DevExpress.XtraBars.BarEditItem();
            this.tbSection = new DevExpress.XtraBars.BarEditItem();
            this.tbCrew = new DevExpress.XtraBars.BarEditItem();
            this.barDepartment = new DevExpress.XtraBars.BarEditItem();
            this.txtWorkplace = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.txtDepartment = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.pnlStrtReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlStrtReport
            // 
            this.pnlStrtReport.Controls.Add(this.pcReport);
            this.pnlStrtReport.Controls.Add(this.ribbonControl1);
            this.pnlStrtReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStrtReport.Location = new System.Drawing.Point(0, 0);
            this.pnlStrtReport.Name = "pnlStrtReport";
            this.pnlStrtReport.Size = new System.Drawing.Size(1183, 666);
            this.pnlStrtReport.TabIndex = 0;
            // 
            // pcReport
            // 
            this.pcReport.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pcReport.Buttons = ((FastReport.PreviewButtons)((((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save) 
            | FastReport.PreviewButtons.Zoom) 
            | FastReport.PreviewButtons.Navigator)));
            this.pcReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcReport.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pcReport.Location = new System.Drawing.Point(0, 111);
            this.pcReport.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.pcReport.Name = "pcReport";
            this.pcReport.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReport.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReport.SaveInitialDirectory = null;
            this.pcReport.Size = new System.Drawing.Size(1183, 555);
            this.pcReport.TabIndex = 85;
            this.pcReport.UIStyle = FastReport.Utils.UIStyle.VistaGlass;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.AllowKeyTips = false;
            this.ribbonControl1.AllowMdiChildButtons = false;
            this.ribbonControl1.AllowMinimizeRibbon = false;
            this.ribbonControl1.AllowTrimPageText = false;
            this.ribbonControl1.AutoSizeItems = true;
            this.ribbonControl1.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.DarkBlue;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.tbProdMonth,
            this.tbSection,
            this.tbCrew,
            this.barDepartment,
            this.txtWorkplace,
            this.txtDepartment,
            this.barButtonItem1});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 39;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.OptionsPageCategories.ShowCaptions = false;
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemComboBox1});
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(1183, 111);
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            this.ribbonControl1.Visible = false;
            // 
            // tbProdMonth
            // 
            this.tbProdMonth.Caption = "ProdMonth ";
            this.tbProdMonth.Edit = null;
            this.tbProdMonth.EditWidth = 100;
            this.tbProdMonth.Id = 25;
            this.tbProdMonth.Name = "tbProdMonth";
            // 
            // tbSection
            // 
            this.tbSection.Caption = "Section       ";
            this.tbSection.Edit = null;
            this.tbSection.EditWidth = 100;
            this.tbSection.Id = 26;
            this.tbSection.Name = "tbSection";
            // 
            // tbCrew
            // 
            this.tbCrew.Caption = "Crew          ";
            this.tbCrew.Edit = null;
            this.tbCrew.EditWidth = 200;
            this.tbCrew.Id = 27;
            this.tbCrew.Name = "tbCrew";
            // 
            // barDepartment
            // 
            this.barDepartment.Caption = "Department";
            this.barDepartment.Edit = null;
            this.barDepartment.EditWidth = 150;
            this.barDepartment.Id = 35;
            this.barDepartment.Name = "barDepartment";
            // 
            // txtWorkplace
            // 
            this.txtWorkplace.Caption = "Workplace  ";
            this.txtWorkplace.Edit = this.repositoryItemTextEdit1;
            this.txtWorkplace.EditWidth = 160;
            this.txtWorkplace.Enabled = false;
            this.txtWorkplace.Id = 36;
            this.txtWorkplace.Name = "txtWorkplace";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // txtDepartment
            // 
            this.txtDepartment.Caption = "Department";
            this.txtDepartment.Edit = this.repositoryItemComboBox1;
            this.txtDepartment.EditWidth = 160;
            this.txtDepartment.Id = 37;
            this.txtDepartment.Name = "txtDepartment";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Close";
            this.barButtonItem1.Id = 38;
            this.barButtonItem1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.CloseRed;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ribbonPage1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.business_report;
            this.ribbonPage1.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Startups Report";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.AllowTextClipping = false;
            this.ribbonPageGroup1.ItemLinks.Add(this.tbProdMonth);
            this.ribbonPageGroup1.ItemLinks.Add(this.tbSection);
            this.ribbonPageGroup1.ItemLinks.Add(this.tbCrew);
            this.ribbonPageGroup1.ItemLinks.Add(this.txtWorkplace);
            this.ribbonPageGroup1.ItemLinks.Add(this.txtDepartment);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Filter";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Options";
            // 
            // ucStartupsReport
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlStrtReport);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ucStartupsReport";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(1183, 666);
            this.Load += new System.EventHandler(this.ucStartupsReport_Load);
            this.Controls.SetChildIndex(this.pnlStrtReport, 0);
            this.pnlStrtReport.ResumeLayout(false);
            this.pnlStrtReport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlStrtReport;
        public DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        public DevExpress.XtraBars.BarEditItem tbProdMonth;
        public DevExpress.XtraBars.BarEditItem tbSection;
        public DevExpress.XtraBars.BarEditItem tbCrew;
        private DevExpress.XtraBars.BarEditItem barDepartment;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarEditItem txtWorkplace;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem txtDepartment;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private FastReport.Preview.PreviewControl pcReport;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
    }
}
