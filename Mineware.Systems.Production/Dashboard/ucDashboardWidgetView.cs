using DevExpress.XtraBars.Docking2010.Views.Widget;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.Production.Dashboard.UserControls;
using Mineware.Systems.Production.Menu;
using Mineware.Systems.ProductionGlobal;
using Mineware.Systems.ProductionReports.IncidentsReport;
using Mineware.Systems.ProductionReports.KPIReport;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Mineware.Systems.Production.Dashboard
{
    public partial class ucDashboardWidgetView : BaseUserControl
    {

       
        public DataTable dtIncidents;
        public DataTable dtLTO;        
        int ControlsLoaded = 0;
        clsMenuStructure _clsMenuStructure = new clsMenuStructure();
        Procedures procs = new Procedures();

        public ucDashboardWidgetView()
        {
            //ShowProgressPage(true);
            InitializeComponent();
            //ShowProgressPage(true);
            //this.widgetViewDashboard.QueryControl += widgetView1_QueryControl;
            //CloseProgressPanel(handle);

        }

        // Assigning a required content for each auto generated Document
        void widgetView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {

            try
            {
                
                //SplashScreenManager.Default.Properties.AllowGlowEffect = true;
                // Launch a long-running operation while
                // the Overlay Form overlaps the current form.
                //splashScreenManager.ShowWaitForm();
                if (e.Document == docIncidents)
                {
                    //myForm.labelStatus.Text = "Dashboard - Loading Incidents..";
                    ControlsLoaded = ControlsLoaded + 1;
                    e.Control = new ucIncidents();
                }
                if (e.Document == docKPI)
                {
                    //myForm.labelStatus.Text = "Dashboard - Loading KPI..";
                    ControlsLoaded = ControlsLoaded + 1;
                    e.Control = new ucKPI();
                }
                if (e.Document == docLabour)
                {
                    //myForm.labelStatus.Text = "Dashboard - Loading Labour..";
                    ControlsLoaded = ControlsLoaded + 1;
                    e.Control = new ucLabour();
                }
                if (e.Document == docLTO)
                {
                    //myForm.labelStatus.Text = "Dashboard - Loading LTO..";
                    ControlsLoaded = ControlsLoaded + 1;
                    e.Control = new ucLTO();
                }
                if (e.Document == docSummary)
                {
                    //myForm.labelStatus.Text = "Dashboard - Loading Summary..";
                    ControlsLoaded = ControlsLoaded + 1;
                    e.Control = new ucCards();
                }
                if (e.Control == null)
                {

                    e.Control = new System.Windows.Forms.Control();
                }
            }

            finally
            {

                

            }



        }

        private void ucDashboardWidgetView_Load(object sender, EventArgs e)
        { 
                ////Get Menu Items for System
                //SetProgressPanelDescription = "Loading Syncromine....";
                //SetProgressPanelDescription = "Loading Menu Items....";
                
                SetProgressPanelDescription = "Loading Dashboard....";
                _clsMenuStructure.getAllUserModules();
                _clsMenuStructure.getAllProductionAndSystemMenuItems();
                _clsMenuStructure.getAllParentMenuItems();
                _clsMenuStructure.getOMSParentMenuItems();
                _clsMenuStructure.getAllOMSItems();
                _clsMenuStructure.getAllBCSItems();
                _clsMenuStructure.getBCSParentMenuItems();
                _clsMenuStructure.getOCRParentMenuItems();
                _clsMenuStructure.getAllOCRItems();
                _clsMenuStructure.getAllCCItems();
                _clsMenuStructure.getCCParentMenuItems();

                this.widgetViewDashboard.QueryControl += widgetView1_QueryControl;
           


        }

       
       
        
        private void widgetViewDashboard_ControlShown(object sender, DevExpress.XtraBars.Docking2010.Views.DeferredControlLoadEventArgs e)
        {
            WidgetView view = sender as WidgetView;
            if (e.Document == view.Documents.First())
            {
                //ShowProgressPage(true);
                //SetProgressPanelDescription = "Loading Dashboard....";
            }
            if (e.Document == view.Documents.Last())
            {
                if (!Debugger.IsAttached)
                {
                   ShowProgressPage(false);
                }
            }
        }

        private void docIncidents_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            IncidentsDashboard Report = new IncidentsDashboard();
            Report._Connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            Report._Prodmonth = ProductionGlobalTSysSettings._currentProductionMonth.ToString();
            Report._Mosect = "0";
            Report._MosectName = "Total Mine";
            ReportPrintTool tool = new ReportPrintTool(Report);
            tool.ShowPreview();
        }

        private void docKPI_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            KPIReport Report = new KPIReport();
            Report._Connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            Report._Prodmonth = ProductionGlobalTSysSettings._currentProductionMonth.ToString();
            //Report._Mosect = "0";
            //Report._MosectName = "Total Mine";
            ReportPrintTool tool = new ReportPrintTool(Report);
            tool.ShowPreview();
        }
    }
}
