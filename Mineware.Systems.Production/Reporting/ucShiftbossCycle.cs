using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
//using Mineware.Systems.HarmonyPASGlobal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting
{
    public partial class ucShiftbossCycle : BaseUserControl
    {
        #region Fields and Properties 
        private string _FirstLoad = "Y";
        private string _numDay = "2";
        private string NotesLbl = string.Empty;
        private string _section = string.Empty;
        private string _month = string.Empty;
        private string Cat = string.Empty;
        private Report _theReport = new Report();
        private Report _theReport2 = new Report();
        private Report _theReport3 = new Report();
        private Report _theReportError = new Report();
        DataTable dtReport = new DataTable();
        DataTable dtGetDate = new DataTable();
        /// <summary>
        /// The report to show
        /// </summary>


        /// <summary>
        /// The folder name where the reports are located
        /// </summary>
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

#if DEBUG
        private bool _designReport = true;
#else
        private bool _designReport = false;
#endif
        #endregion Fields and Properties  

        #region Constructor
        public ucShiftbossCycle()
        {
            InitializeComponent();
           
            FormRibbonPages.Add(rpMeasList);
            FormActiveRibbonPage = rpMeasList;
            FormMainRibbonPage = rpMeasList;
            RibbonControl = rcMeaslist;
        }
        #endregion Constructor

        #region Events
        

        private void ucShiftbossCycle_Load(object sender, EventArgs e)
        {
            cbxBookingProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobalTSysSettings._currentProductionMonth.ToString());
            //ProdMonth1Txt.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionAmplatsGlobalTSysSettings._currentProductionMonth.ToString());

            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
            _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManMonth.SqlStatement = " select getdate() aa ";
            _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMonth.ResultsTableName = "aa";  //get table name
            _dbManMonth.ExecuteInstruction();

            dtGetDate = _dbManMonth.ResultsDataTable;
            //dateTime.Value = Convert.ToDateTime(dtGetDate.Rows[0]["aa"].ToString());           

            LoadSections();
            //cmbSections.SelectedText = "Stoping";
            _FirstLoad = "N";

            LoadSBCycle();
        }     


        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        #endregion Events 

        #region Methods  
        /// <summary>
        /// Load the report after adding data
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


        public void LoadSBCycle()
        {
            _section = cbxBookingMO.EditValue.ToString();

            _month = ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)).ToString();

            if (radioType.EditValue.ToString() == "Stoping")
            {

                SBCycle_Stoping();

            }

            if (radioType.EditValue.ToString() == "Development")
            {
                SBCycle_Dev();
            }

           

        }       


        public void SBCycle_Stoping()
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
            string sqlGraph = string.Empty;
            string sqlTotals = string.Empty;
            string sqlTotals2 = string.Empty;
            string tableName = string.Empty;

            //The ABS Colors from Sysset
            string AColor = ProductionGlobal.ProductionGlobalTSysSettings._AColor.ToString();
            string BColor = ProductionGlobal.ProductionGlobalTSysSettings._BColor.ToString();
            string SColor = ProductionGlobal.ProductionGlobalTSysSettings._SColor.ToString();
            string Banner = ProductionGlobalTSysSettings._Banner.ToString();

            //Add Section and Prodmonth
            string Prodaa = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString()));

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(Prodaa));
            string ProdMonthDesc = ProductionGlobal.ProductionGlobal.Prod2;
            string SectDesc = LookUpEditSection2.GetDisplayText(cbxBookingMO.EditValue);

            DataSet ds = new DataSet();
            MWDataManager.clsDataAccess _databaseConnection = new MWDataManager.clsDataAccess();
            _databaseConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _databaseConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _databaseConnection.queryReturnType = MWDataManager.ReturnType.DataTable;

            reportFileName = "SBCycleStope.frx";
            tableName = "Table1";
            sql = @"EXEC [dbo].sp_Shiftboss_Stoping_Cycle '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString())) + @"', '" + cbxBookingMO.EditValue.ToString() + @"', '" + _numDay + @"'";

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

            ////Graph            
            MWDataManager.clsDataAccess _databaseConnectionGraph = new MWDataManager.clsDataAccess();
            _databaseConnectionGraph.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _databaseConnectionGraph.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _databaseConnectionGraph.queryReturnType = MWDataManager.ReturnType.DataTable;

            
            tableName = "Graph";
            sqlGraph = @"EXEC [dbo].sp_Shiftboss_Stoping_Cycle_Graph '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString())) + @"', '" + cbxBookingMO.EditValue.ToString() + @"'";

            _databaseConnectionGraph.SqlStatement = sqlGraph;

            var ActionResult2 = _databaseConnectionGraph.ExecuteInstruction();
            if (ActionResult2.success)
            {
                _databaseConnectionGraph.ResultsDataTable.TableName = tableName;
                ds.Tables.Add(_databaseConnectionGraph.ResultsDataTable);
            }
            else
            {
                this.Cursor = Cursors.Default;
                return;
            }


            ////Totals           
            MWDataManager.clsDataAccess _databaseConnectionTotals = new MWDataManager.clsDataAccess();
            _databaseConnectionTotals.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _databaseConnectionTotals.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _databaseConnectionTotals.queryReturnType = MWDataManager.ReturnType.DataTable;

            tableName = "Totals";
            sqlTotals = @"EXEC [dbo].sp_Shiftboss_Stoping_Cycle_Totals '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString())) + @"', '" + cbxBookingMO.EditValue.ToString() + @"'";

            _databaseConnectionTotals.SqlStatement = sqlTotals;

            var ActionResult3 = _databaseConnectionTotals.ExecuteInstruction();
            if (ActionResult3.success)
            {
                _databaseConnectionTotals.ResultsDataTable.TableName = tableName;
                ds.Tables.Add(_databaseConnectionTotals.ResultsDataTable);
            }
            else
            {
                this.Cursor = Cursors.Default;
                return;
            }


            ////Totals2           
            MWDataManager.clsDataAccess _databaseConnectionTotals2 = new MWDataManager.clsDataAccess();
            _databaseConnectionTotals2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _databaseConnectionTotals2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _databaseConnectionTotals2.queryReturnType = MWDataManager.ReturnType.DataTable;

            tableName = "Totals2";
            sqlTotals2 = @"EXEC [dbo].sp_Shiftboss_Stoping_Totals2 '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString())) + @"', '" + cbxBookingMO.EditValue.ToString() + @"'";

            _databaseConnectionTotals2.SqlStatement = sqlTotals2;

            var ActionResult4 = _databaseConnectionTotals2.ExecuteInstruction();
            if (ActionResult4.success)
            {
                _databaseConnectionTotals2.ResultsDataTable.TableName = tableName;
                ds.Tables.Add(_databaseConnectionTotals2.ResultsDataTable);
            }
            else
            {
                this.Cursor = Cursors.Default;
                return;
            }


            _theReport.Load(_reportFolder + reportFileName);
            _theReport.RegisterData(ds);

            _theReport.SetParameterValue("AColor", AColor);
            _theReport.SetParameterValue("BColor", BColor);
            _theReport.SetParameterValue("SColor", SColor);
            _theReport.SetParameterValue("ProdMonth", ProdMonthDesc);
            _theReport.SetParameterValue("MOSection", SectDesc);
            _theReport.SetParameterValue("Banner", Banner);            

            //Show the report
            LoadReport(_theReport);



        }




        public void SBCycle_Dev()
        {
            

            
        }


        



        private void LoadSections()
        {
            MWDataManager.clsDataAccess _PrePlanningLoadSections = new MWDataManager.clsDataAccess();
            _PrePlanningLoadSections.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _PrePlanningLoadSections.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningLoadSections.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningLoadSections.SqlStatement = " Select SBid Sectionid_1,SBName Name_1 \r\n" +
                                                    "from [dbo].[tbl_sectioncomplete] \r\n" +
                                                    "where prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString())) + "' \r\n " +
                                                    " and SBid in (select SECTIONID_1 from tbl_PlanMonth pm, Sections_Complete sc where pm.Sectionid = sc.SECTIONID and pm.Prodmonth = sc.Prodmonth) \r\n" +
                                                    " Group By SBid,SBName \r\n " +
                                                    " Order By SBid,SBName";
            _PrePlanningLoadSections.ExecuteInstruction();

            DataTable tbl_Sections = _PrePlanningLoadSections.ResultsDataTable;


            LookUpEditSection2.DataSource = tbl_Sections;
            LookUpEditSection2.DisplayMember = "Name_1";
            LookUpEditSection2.ValueMember = "Sectionid_1";

            cbxBookingMO.EditValue = tbl_Sections.Rows[0]["Sectionid_1"].ToString();
        }
        #endregion Methods 


        

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadSBCycle();
            }
        }

        private void cbxBookingMO_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadSBCycle();
            }
        }

        private void radioType_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadSBCycle();
            }
        }
    }
}
