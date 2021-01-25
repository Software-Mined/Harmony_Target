namespace Mineware.Systems.Production.LineActionManager
{
    partial class ucLineActionManager_ClosureRequest
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
            this.rcLineAction = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barBtnBack = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.pcReportGen = new FastReport.Preview.PreviewControl();
            this.bkwrMain = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.rcLineAction)).BeginInit();
            this.SuspendLayout();
            // 
            // rcLineAction
            // 
            this.rcLineAction.AllowKeyTips = false;
            this.rcLineAction.AllowMdiChildButtons = false;
            this.rcLineAction.AllowMinimizeRibbon = false;
            this.rcLineAction.AllowTrimPageText = false;
            this.rcLineAction.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.DarkBlue;
            this.rcLineAction.DrawGroupCaptions = DevExpress.Utils.DefaultBoolean.False;
            this.rcLineAction.ExpandCollapseItem.Id = 0;
            this.rcLineAction.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcLineAction.ExpandCollapseItem,
            this.rcLineAction.SearchEditItem,
            this.barBtnBack});
            this.rcLineAction.Location = new System.Drawing.Point(0, 0);
            this.rcLineAction.MaxItemId = 18;
            this.rcLineAction.Name = "rcLineAction";
            this.rcLineAction.OptionsPageCategories.ShowCaptions = false;
            this.rcLineAction.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.rcLineAction.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcLineAction.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcLineAction.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcLineAction.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.rcLineAction.ShowToolbarCustomizeItem = false;
            this.rcLineAction.Size = new System.Drawing.Size(723, 89);
            this.rcLineAction.Toolbar.ShowCustomizeItem = false;
            this.rcLineAction.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // barBtnBack
            // 
            this.barBtnBack.Caption = "Back";
            this.barBtnBack.Id = 6;
            this.barBtnBack.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.BackBlue1;
            this.barBtnBack.Name = "barBtnBack";
            this.barBtnBack.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnBack_ItemClick);
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
            this.ribbonPageGroup2.ItemLinks.Add(this.barBtnBack);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Actions";
            // 
            // pcReportGen
            // 
            this.pcReportGen.BackColor = System.Drawing.Color.Transparent;
            this.pcReportGen.Buttons = ((FastReport.PreviewButtons)(((((((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save) 
            | FastReport.PreviewButtons.Email) 
            | FastReport.PreviewButtons.Find) 
            | FastReport.PreviewButtons.Zoom) 
            | FastReport.PreviewButtons.Outline) 
            | FastReport.PreviewButtons.Navigator)));
            this.pcReportGen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcReportGen.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pcReportGen.Location = new System.Drawing.Point(0, 89);
            this.pcReportGen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pcReportGen.Name = "pcReportGen";
            this.pcReportGen.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReportGen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReportGen.SaveInitialDirectory = null;
            this.pcReportGen.Size = new System.Drawing.Size(723, 381);
            this.pcReportGen.TabIndex = 3;
            this.pcReportGen.UIStyle = FastReport.Utils.UIStyle.Office2007Black;
            // 
            // bkwrMain
            // 
            this.bkwrMain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkwrMain_DoWork);
            this.bkwrMain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkwrMain_RunWorkerCompleted);
            // 
            // ucLineActionManager_ClosureRequest
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pcReportGen);
            this.Controls.Add(this.rcLineAction);
            this.Name = "ucLineActionManager_ClosureRequest";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(723, 470);
            this.Load += new System.EventHandler(this.ucLineActionManager_ClosureRequest_Load);
            this.Controls.SetChildIndex(this.rcLineAction, 0);
            this.Controls.SetChildIndex(this.pcReportGen, 0);
            ((System.ComponentModel.ISupportInitialize)(this.rcLineAction)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl rcLineAction;
        private DevExpress.XtraBars.BarButtonItem barBtnBack;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private FastReport.Preview.PreviewControl pcReportGen;
        private System.ComponentModel.BackgroundWorker bkwrMain;
    }
}
