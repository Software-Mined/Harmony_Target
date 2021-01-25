namespace Mineware.Systems.Production.Dashboard
{
    partial class ucDashboardWidgetView
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraBars.Docking.CustomHeaderButtonImageOptions customHeaderButtonImageOptions1 = new DevExpress.XtraBars.Docking.CustomHeaderButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraBars.Docking.CustomHeaderButtonImageOptions customHeaderButtonImageOptions2 = new DevExpress.XtraBars.Docking.CustomHeaderButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraBars.Docking.CustomHeaderButtonImageOptions customHeaderButtonImageOptions3 = new DevExpress.XtraBars.Docking.CustomHeaderButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer2 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer3 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer4 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer5 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer6 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer7 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer widgetDockingContainer8 = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetDockingContainer();
            this.docSummary = new DevExpress.XtraBars.Docking2010.Views.Widget.Document(this.components);
            this.docLabour = new DevExpress.XtraBars.Docking2010.Views.Widget.Document(this.components);
            this.docLTO = new DevExpress.XtraBars.Docking2010.Views.Widget.Document(this.components);
            this.docKPI = new DevExpress.XtraBars.Docking2010.Views.Widget.Document(this.components);
            this.docIncidents = new DevExpress.XtraBars.Docking2010.Views.Widget.Document(this.components);
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.widgetViewDashboard = new DevExpress.XtraBars.Docking2010.Views.Widget.WidgetView(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.docSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docLabour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docLTO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docKPI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docIncidents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widgetViewDashboard)).BeginInit();
            this.SuspendLayout();
            // 
            // docSummary
            // 
            this.docSummary.Caption = "Summary";
            this.docSummary.ControlName = "ucCards";
            this.docSummary.ControlTypeName = "Mineware.Systems.Production.Dashboard.DasboardItems.ucCards";
            this.docSummary.FreeLayoutHeight.UnitValue = 0.54059442974079541D;
            this.docSummary.Properties.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.docSummary.Properties.ShowMaximizeButton = DevExpress.Utils.DefaultBoolean.False;
            // 
            // docLabour
            // 
            this.docLabour.Caption = "Labour";
            this.docLabour.ControlName = "ucLabour";
            this.docLabour.ControlTypeName = "Mineware.Systems.Production.Dashboard.DasboardItems.ucLabour";
            customHeaderButtonImageOptions1.SvgImage = global::Mineware.Systems.Production.Properties.Resources.DocumentHybridBlue;
            customHeaderButtonImageOptions1.SvgImageSize = new System.Drawing.Size(16, 16);
            this.docLabour.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] {
            new DevExpress.XtraBars.Docking.CustomHeaderButton("Report", true, customHeaderButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, serializableAppearanceObject1, null, -1)});
            this.docLabour.FreeLayoutHeight.UnitValue = 0.7853952137765956D;
            this.docLabour.FreeLayoutWidth.UnitValue = 0.74756017367580052D;
            this.docLabour.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            // 
            // docLTO
            // 
            this.docLTO.Caption = "License To Operate";
            this.docLTO.ControlName = "ucLTO";
            this.docLTO.ControlTypeName = "Mineware.Systems.Production.Dashboard.DasboardItems.ucLTO";
            this.docLTO.FreeLayoutHeight.UnitValue = 1.5921304090108879D;
            this.docLTO.FreeLayoutWidth.UnitValue = 0.759769993233352D;
            this.docLTO.Properties.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            // 
            // docKPI
            // 
            this.docKPI.Caption = "KPI";
            this.docKPI.ControlName = "ucKPI";
            this.docKPI.ControlTypeName = "Mineware.Systems.Production.Dashboard.DasboardItems.ucKPI";
            customHeaderButtonImageOptions2.SvgImage = global::Mineware.Systems.Production.Properties.Resources.DocumentHybridBlue;
            customHeaderButtonImageOptions2.SvgImageSize = new System.Drawing.Size(16, 16);
            this.docKPI.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] {
            new DevExpress.XtraBars.Docking.CustomHeaderButton("Report", true, customHeaderButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, serializableAppearanceObject2, null, -1)});
            this.docKPI.FreeLayoutHeight.UnitValue = 1.0606605956469604D;
            this.docKPI.FreeLayoutWidth.UnitValue = 1.3129802780879694D;
            this.docKPI.Properties.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.docKPI.CustomButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.docKPI_CustomButtonClick);
            // 
            // docIncidents
            // 
            this.docIncidents.Caption = "Open Incidents (Double-Click your Incidents to close them)";
            this.docIncidents.ControlName = "ucIncidents";
            this.docIncidents.ControlTypeName = "Mineware.Systems.Production.Dashboard.DasboardItems.ucIncidents";
            customHeaderButtonImageOptions3.Location = DevExpress.XtraBars.Docking2010.HorizontalImageLocation.BeforeText;
            customHeaderButtonImageOptions3.SvgImage = global::Mineware.Systems.Production.Properties.Resources.DocumentHybridBlue;
            customHeaderButtonImageOptions3.SvgImageSize = new System.Drawing.Size(16, 16);
            this.docIncidents.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] {
            new DevExpress.XtraBars.Docking.CustomHeaderButton("Report", true, customHeaderButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, serializableAppearanceObject3, null, -1)});
            this.docIncidents.FreeLayoutHeight.UnitValue = 0.44721549019244655D;
            this.docIncidents.Properties.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.docIncidents.CustomButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.docIncidents_CustomButtonClick);
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.RibbonAndBarsMergeStyle = DevExpress.XtraBars.Docking2010.Views.RibbonAndBarsMergeStyle.Always;
            this.documentManager1.SnapMode = DevExpress.Utils.Controls.SnapMode.OwnerControl;
            this.documentManager1.View = this.widgetViewDashboard;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.widgetViewDashboard});
            // 
            // widgetViewDashboard
            // 
            this.widgetViewDashboard.Appearance.BackColor = System.Drawing.Color.White;
            this.widgetViewDashboard.Appearance.Options.UseBackColor = true;
            this.widgetViewDashboard.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.docLTO,
            this.docLabour,
            this.docKPI,
            this.docIncidents,
            this.docSummary});
            this.widgetViewDashboard.FreeLayoutProperties.FreeLayoutItems.AddRange(new DevExpress.XtraBars.Docking2010.Views.Widget.Document[] {
            this.docLTO,
            this.docLabour,
            this.docKPI,
            this.docIncidents,
            this.docSummary});
            this.widgetViewDashboard.LayoutMode = DevExpress.XtraBars.Docking2010.Views.Widget.LayoutMode.FreeLayout;
            widgetDockingContainer3.Element = this.docSummary;
            widgetDockingContainer4.Element = this.docLabour;
            widgetDockingContainer5.Element = this.docLTO;
            widgetDockingContainer2.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            widgetDockingContainer3,
            widgetDockingContainer4,
            widgetDockingContainer5});
            widgetDockingContainer2.Orientation = System.Windows.Forms.Orientation.Vertical;
            widgetDockingContainer2.Size.Width.UnitValue = 0.74762581730120325D;
            widgetDockingContainer7.Element = this.docKPI;
            widgetDockingContainer8.Element = this.docIncidents;
            widgetDockingContainer6.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            widgetDockingContainer7,
            widgetDockingContainer8});
            widgetDockingContainer6.Orientation = System.Windows.Forms.Orientation.Vertical;
            widgetDockingContainer6.Size.Width.UnitValue = 1.2523741826987962D;
            widgetDockingContainer1.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            widgetDockingContainer2,
            widgetDockingContainer6});
            widgetDockingContainer1.Size.Height.UnitValue = 1.584707362808442D;
            this.widgetViewDashboard.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            widgetDockingContainer1});
            this.widgetViewDashboard.RootContainer.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.widgetViewDashboard.ControlShown += new DevExpress.XtraBars.Docking2010.Views.DeferredControlLoadEventHandler(this.widgetViewDashboard_ControlShown);
            // 
            // ucDashboardWidgetView
            // 
            this.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "ucDashboardWidgetView";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(1331, 864);
            this.Load += new System.EventHandler(this.ucDashboardWidgetView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.docSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docLabour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docLTO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docKPI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docIncidents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widgetViewDashboard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Widget.WidgetView widgetViewDashboard;
        private DevExpress.XtraBars.Docking2010.Views.Widget.Document docLTO;
        private DevExpress.XtraBars.Docking2010.Views.Widget.Document docLabour;
        private DevExpress.XtraBars.Docking2010.Views.Widget.Document docKPI;
        private DevExpress.XtraBars.Docking2010.Views.Widget.Document docIncidents;
        private DevExpress.XtraBars.Docking2010.Views.Widget.Document docSummary;
    }
}
