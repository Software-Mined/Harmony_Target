
namespace Mineware.Systems.Production.Reporting.WorkplaceSummary
{
    partial class ucWorkplaceSummary
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer2 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer3 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucWorkplaceSummary));
            this.documentData = new DevExpress.XtraBars.Docking2010.Views.Widget.Document(this.components);
            this.documentReport = new DevExpress.XtraBars.Docking2010.Views.Widget.Document(this.components);
            this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.widgetView1 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetView(this.components);
            this.rcPreplanning = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.barProdMonth = new DevExpress.XtraBars.BarEditItem();
            this.MOSection = new DevExpress.XtraBars.BarEditItem();
            this.barbtnClose = new DevExpress.XtraBars.BarButtonItem();
            this.btnPPRE = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.btnHelp = new DevExpress.XtraBars.BarButtonItem();
            this.btnPPGeo = new DevExpress.XtraBars.BarButtonItem();
            this.btnPPVent = new DevExpress.XtraBars.BarButtonItem();
            this.btnShow = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.btnPPSurvey = new DevExpress.XtraBars.BarButtonItem();
            this.btnPPSafety = new DevExpress.XtraBars.BarButtonItem();
            this.btnPPEng = new DevExpress.XtraBars.BarButtonItem();
            this.btnDepRE = new DevExpress.XtraBars.BarButtonItem();
            this.btnDepGeo = new DevExpress.XtraBars.BarButtonItem();
            this.btnDepVent = new DevExpress.XtraBars.BarButtonItem();
            this.rpPreplanning = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.documentData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widgetView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcPreplanning)).BeginInit();
            this.SuspendLayout();
            // 
            // documentData
            // 
            this.documentData.Caption = "Workplace Data";
            this.documentData.ControlName = "document1";
            this.documentData.FreeLayoutWidth.UnitValue = 0.78445599768096119D;
            this.documentData.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            this.documentData.Properties.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            // 
            // documentReport
            // 
            this.documentReport.Caption = "Report";
            this.documentReport.ControlName = "document2";
            this.documentReport.FreeLayoutWidth.UnitValue = 1.2155440023190389D;
            this.documentReport.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            this.documentReport.Properties.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            // 
            // documentManager
            // 
            this.documentManager.ContainerControl = this;
            this.documentManager.View = this.widgetView1;
            this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.widgetView1});
            // 
            // widgetView1
            // 
            this.widgetView1.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.documentData,
            this.documentReport});
            this.widgetView1.FreeLayoutProperties.FreeLayoutItems.AddRange(new DevExpress.XtraBars.Docking2010.Views.Widget.Document[] {
            this.documentData,
            this.documentReport});
            this.widgetView1.LayoutMode = DevExpress.XtraBars.Docking2010.Views.Widget.LayoutMode.FreeLayout;
            widgetDockingContainer2.Element = this.documentData;
            widgetDockingContainer3.Element = this.documentReport;
            widgetDockingContainer1.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            widgetDockingContainer2,
            widgetDockingContainer3});
            this.widgetView1.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            widgetDockingContainer1});
            this.widgetView1.RootContainer.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // rcPreplanning
            // 
            this.rcPreplanning.AllowKeyTips = false;
            this.rcPreplanning.AllowMdiChildButtons = false;
            this.rcPreplanning.AllowMinimizeRibbon = false;
            this.rcPreplanning.AllowTrimPageText = false;
            this.rcPreplanning.AutoSizeItems = true;
            this.rcPreplanning.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.DarkBlue;
            this.rcPreplanning.ExpandCollapseItem.Id = 0;
            this.rcPreplanning.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcPreplanning.ExpandCollapseItem,
            this.rcPreplanning.SearchEditItem,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barHeaderItem1,
            this.barProdMonth,
            this.MOSection,
            this.barbtnClose,
            this.btnPPRE,
            this.barCheckItem1,
            this.btnHelp,
            this.btnPPGeo,
            this.btnPPVent,
            this.btnShow,
            this.barStaticItem1,
            this.barStaticItem2,
            this.btnPPSurvey,
            this.btnPPSafety,
            this.btnPPEng,
            this.btnDepRE,
            this.btnDepGeo,
            this.btnDepVent});
            this.rcPreplanning.Location = new System.Drawing.Point(0, 0);
            this.rcPreplanning.MaxItemId = 45;
            this.rcPreplanning.Name = "rcPreplanning";
            this.rcPreplanning.OptionsPageCategories.ShowCaptions = false;
            this.rcPreplanning.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpPreplanning});
            this.rcPreplanning.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcPreplanning.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcPreplanning.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcPreplanning.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.rcPreplanning.ShowToolbarCustomizeItem = false;
            this.rcPreplanning.Size = new System.Drawing.Size(1394, 110);
            this.rcPreplanning.Toolbar.ShowCustomizeItem = false;
            this.rcPreplanning.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
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
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "This is a Hint";
            this.barHeaderItem1.Id = 24;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // barProdMonth
            // 
            this.barProdMonth.Caption = "Production Month";
            this.barProdMonth.Edit = null;
            this.barProdMonth.EditWidth = 100;
            this.barProdMonth.Id = 28;
            this.barProdMonth.Name = "barProdMonth";
            // 
            // MOSection
            // 
            this.MOSection.Caption = "Section ";
            this.MOSection.Edit = null;
            this.MOSection.EditWidth = 160;
            this.MOSection.Id = 29;
            this.MOSection.Name = "MOSection";
            // 
            // barbtnClose
            // 
            this.barbtnClose.Caption = "Close";
            this.barbtnClose.Id = 30;
            this.barbtnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barbtnClose.ImageOptions.SvgImage")));
            this.barbtnClose.Name = "barbtnClose";
            this.barbtnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnClose_ItemClick);
            // 
            // btnPPRE
            // 
            this.btnPPRE.Caption = "Rock  Engineering";
            this.btnPPRE.Id = 31;
            this.btnPPRE.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.ConfigBlue;
            this.btnPPRE.Name = "btnPPRE";
            this.btnPPRE.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPPRE_ItemClick);
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "barCheckItem1";
            this.barCheckItem1.Id = 32;
            this.barCheckItem1.Name = "barCheckItem1";
            // 
            // btnHelp
            // 
            this.btnHelp.Caption = "Help";
            this.btnHelp.Id = 33;
            this.btnHelp.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.HelpGreen;
            this.btnHelp.Name = "btnHelp";
            // 
            // btnPPGeo
            // 
            this.btnPPGeo.Caption = "Geology";
            this.btnPPGeo.Id = 34;
            this.btnPPGeo.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.ChainBlue1;
            this.btnPPGeo.Name = "btnPPGeo";
            this.btnPPGeo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPPGeo_ItemClick);
            // 
            // btnPPVent
            // 
            this.btnPPVent.Caption = "Ventillation";
            this.btnPPVent.Id = 35;
            this.btnPPVent.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.WindBlue;
            this.btnPPVent.Name = "btnPPVent";
            this.btnPPVent.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPPVent_ItemClick);
            // 
            // btnShow
            // 
            this.btnShow.Caption = "Show";
            this.btnShow.Id = 36;
            this.btnShow.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.ZoomBlue22;
            this.btnShow.Name = "btnShow";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "Double-Click the Workplace to view Report";
            this.barStaticItem1.Id = 37;
            this.barStaticItem1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.InformationBlue;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "Double-Click the Department to Capture the Scrutiny information.";
            this.barStaticItem2.Id = 38;
            this.barStaticItem2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.InformationBlue;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // btnPPSurvey
            // 
            this.btnPPSurvey.Caption = "Survey";
            this.btnPPSurvey.Id = 39;
            this.btnPPSurvey.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.ZoomBlue2;
            this.btnPPSurvey.Name = "btnPPSurvey";
            this.btnPPSurvey.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPPSurvey_ItemClick);
            // 
            // btnPPSafety
            // 
            this.btnPPSafety.Caption = "Safety";
            this.btnPPSafety.Id = 40;
            this.btnPPSafety.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.actions_removecircled;
            this.btnPPSafety.Name = "btnPPSafety";
            this.btnPPSafety.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPPSafety_ItemClick);
            // 
            // btnPPEng
            // 
            this.btnPPEng.Caption = "Engineering";
            this.btnPPEng.Id = 41;
            this.btnPPEng.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.TruckBlue1;
            this.btnPPEng.Name = "btnPPEng";
            this.btnPPEng.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPPEng_ItemClick);
            // 
            // btnDepRE
            // 
            this.btnDepRE.Caption = "Rock  Engineering";
            this.btnDepRE.Id = 42;
            this.btnDepRE.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.ConfigBlue;
            this.btnDepRE.Name = "btnDepRE";
            this.btnDepRE.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDepRE_ItemClick);
            // 
            // btnDepGeo
            // 
            this.btnDepGeo.Caption = "Geology";
            this.btnDepGeo.Id = 43;
            this.btnDepGeo.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.ChainBlue1;
            this.btnDepGeo.Name = "btnDepGeo";
            this.btnDepGeo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDepGeo_ItemClick);
            // 
            // btnDepVent
            // 
            this.btnDepVent.Caption = "Ventillation";
            this.btnDepVent.Id = 44;
            this.btnDepVent.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.WindBlue;
            this.btnDepVent.Name = "btnDepVent";
            this.btnDepVent.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDepVent_ItemClick);
            // 
            // rpPreplanning
            // 
            this.rpPreplanning.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3,
            this.ribbonPageGroup5,
            this.ribbonPageGroup2,
            this.ribbonPageGroup4});
            this.rpPreplanning.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.Report;
            this.rpPreplanning.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.rpPreplanning.Name = "rpPreplanning";
            this.rpPreplanning.Text = " Workplace Summary";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.btnPPRE);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnPPGeo);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnPPVent);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnPPSurvey);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnPPSafety);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnPPEng);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Pre-Planning";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.btnDepRE);
            this.ribbonPageGroup5.ItemLinks.Add(this.btnDepGeo);
            this.ribbonPageGroup5.ItemLinks.Add(this.btnDepVent);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.Text = "Departmental Visits";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnHelp);
            this.ribbonPageGroup2.ItemLinks.Add(this.barbtnClose);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Action";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.barStaticItem1);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "Information";
            // 
            // ucWorkplaceSummary
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rcPreplanning);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ucWorkplaceSummary";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(1394, 785);
            this.Load += new System.EventHandler(this.ucWorkplaceSummary_Load);
            this.Controls.SetChildIndex(this.rcPreplanning, 0);
            ((System.ComponentModel.ISupportInitialize)(this.documentData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widgetView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcPreplanning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
        private DevExpress.XtraBars.Docking2010.Views.Widget.WidgetView widgetView1;
        private DevExpress.XtraBars.Docking2010.Views.Widget.Document documentData;
        private DevExpress.XtraBars.Docking2010.Views.Widget.Document documentReport;
        public DevExpress.XtraBars.Ribbon.RibbonControl rcPreplanning;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private DevExpress.XtraBars.BarEditItem barProdMonth;
        private DevExpress.XtraBars.BarEditItem MOSection;
        private DevExpress.XtraBars.BarButtonItem barbtnClose;
        private DevExpress.XtraBars.BarButtonItem btnPPRE;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarButtonItem btnHelp;
        private DevExpress.XtraBars.BarButtonItem btnPPGeo;
        private DevExpress.XtraBars.BarButtonItem btnPPVent;
        private DevExpress.XtraBars.BarButtonItem btnShow;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpPreplanning;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarButtonItem btnPPSurvey;
        private DevExpress.XtraBars.BarButtonItem btnPPSafety;
        private DevExpress.XtraBars.BarButtonItem btnPPEng;
        private DevExpress.XtraBars.BarButtonItem btnDepRE;
        private DevExpress.XtraBars.BarButtonItem btnDepGeo;
        private DevExpress.XtraBars.BarButtonItem btnDepVent;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
    }
}
