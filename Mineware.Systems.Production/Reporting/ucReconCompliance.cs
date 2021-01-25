using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting
{
    public partial class ucReconCompliance : BaseUserControl
    {
        #region Fields and Properties           
        private Report _theReport = new Report();
        /// <summary>
        /// The report to show
        /// </summary>
        private string _FirstLoad = "Y";
        private string _numDay = "2";

        /// <summary>
        /// The folder name where the reports are located
        /// </summary>
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

        private string _connectionString = string.Empty;

#if DEBUG
        private bool _designReport = true;
#else
        private bool _designReport = false;
#endif
        #endregion Fields and Properties  

        #region Constructor
        public ucReconCompliance()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpMODaily);
            FormActiveRibbonPage = rpMODaily;
            FormMainRibbonPage = rpMODaily;            
            RibbonControl = rcMODaily;
            //RibbonControl.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.True;
            //rcMODaily.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.True;
        }
        #endregion Constructor

        #region Events
        private void lueReport_EditValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void mwRepositoryItemProdMonth1_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void lueMOSection_EditValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ucMODailyReport_Load(object sender, EventArgs e)
        {
            //Load report types 
            LookUpEditType.DataSource = new string[] { "Stoping", "Development", "Night Shift Stoping", "Night Shift Development", "Stoping Cycle", "Development Cycle", "Vamping", "Stoping Compliance", "Development Compliance" };
            cbxType.EditValue = "Stoping";
            cbxBookingProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobalTSysSettings._currentProductionMonth.ToString());
        }


        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        #endregion Events 

        #region Methods  
        /// <summary>
        /// Loads the report into the viewer
        /// </summary>
        /// <param name="report"></param>
        public void LoadReport(Report report)
        {
            _theReport = report;
            _theReport.Preview = pcReport2;

            //if (_designReport)
            //_theReport.Design();

            _theReport.Show();
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Used to load data on the report
        /// </summary>
        public void LoadData()
        {
            this.Cursor = Cursors.WaitCursor;
            if (cbxBookingProdMonth.EditValue == null || cbxBookingMO.EditValue == null)
            {
                this.Cursor = Cursors.Default;
                return;
            }

            //TODO: make sure calling correct report and name the table correctly
            string reportFileName = string.Empty;
            string sql = string.Empty;
            string tableName = string.Empty;
            
            string Banner = ProductionGlobalTSysSettings._Banner.ToString();

            //Add Section and Prodmonth
            string Prodaa = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString()));

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(Prodaa));
            string ProdMonthDesc = ProductionGlobal.ProductionGlobal.Prod2;
            string SectDesc = lookupEditSection.GetDisplayText(cbxBookingMO.EditValue);

            DataSet ds = new DataSet();
            MWDataManager.clsDataAccess _databaseConnection = new MWDataManager.clsDataAccess();
            _databaseConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _databaseConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _databaseConnection.queryReturnType = MWDataManager.ReturnType.DataTable;

            reportFileName = "ReconCompliance.frx";
            tableName = "6Shift";
            sql = @"EXEC [dbo].[sp_Report_ReconCompliance] '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString())) + @"', '" + cbxBookingMO.EditValue.ToString() + @"','Mon', '" + cbxBookingMO.EditValue.ToString() + @"'";

            _databaseConnection.SqlStatement = sql;

            var ActionResult = _databaseConnection.ExecuteInstruction();

            if (ActionResult.success)
            {
                _databaseConnection.ResultsDataTable.TableName = tableName;
                ds.Tables.Add(_databaseConnection.ResultsDataTable);
            }
            else
            {
                this.Cursor = Cursors.Default;
                return;
            }

            _theReport.Load(_reportFolder + reportFileName);
            _theReport.RegisterData(ds);
           
            _theReport.SetParameterValue("ProdMonth", ProdMonthDesc);
            _theReport.SetParameterValue("MOSection", SectDesc);
            _theReport.SetParameterValue("Banner", Banner);

           // _theReport.Design();

            //Show the report
            LoadReport(_theReport);
        }

        /// <summary>
        /// Load the sections from DB
        /// </summary>
        private void LoadSections()
        {
            MWDataManager.clsDataAccess _PrePlanningLoadSections = new MWDataManager.clsDataAccess();
            _PrePlanningLoadSections.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _PrePlanningLoadSections.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningLoadSections.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningLoadSections.SqlStatement = " Select moid Sectionid_2,moname Name_2 \r\n" +
                                                    "from [dbo].[tbl_sectioncomplete] \r\n" +
                                                    "where prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString())) + "' \r\n " +
                                                    " Group By moid,moname \r\n " +
                                                    " Order By moid,moname";
            _PrePlanningLoadSections.ExecuteInstruction();

            DataTable tbl_Sections = _PrePlanningLoadSections.ResultsDataTable;

            lookupEditSection.DataSource = tbl_Sections;
            lookupEditSection.DisplayMember = "Name_2";
            lookupEditSection.ValueMember = "Sectionid_2";

            if (tbl_Sections != null && tbl_Sections?.Rows.Count > 0)
                cbxBookingMO.EditValue = tbl_Sections.Rows[0]["Sectionid_2"].ToString();
        }


        #endregion Methods

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
            rcMODaily.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.True;
        }


        private void cbxBookingProdMonth_EditValueChanged(object sender, EventArgs e)
        {
            LoadSections();
            LoadData();
            _FirstLoad = "N";
        }

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_FirstLoad == "N")
                LoadData();
        }

        private void cbxType_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
                LoadData();
        }

        private void cbxBookingMO_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
                LoadData();
        }

        private void rcMODaily_Click(object sender, EventArgs e)
        {

        }
    }
}
