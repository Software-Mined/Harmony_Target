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
    public partial class ucMonthEndRecon : BaseUserControl
    {
        #region Fields and Properties 
        private string _FirstLoad = "Y";
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
        public ucMonthEndRecon()
        {
            InitializeComponent();
           
            FormRibbonPages.Add(rpMeasList);
            FormActiveRibbonPage = rpMeasList;
            FormMainRibbonPage = rpMeasList;
            RibbonControl = rcMeaslist;
        }
        #endregion Constructor

        #region Events
        

        private void ucMeasList_Load(object sender, EventArgs e)
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

            LoadMonthEndRecon();
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

            if (_designReport)
                _theReport.Design();
            else
                _theReport.Show();
        }

        /// <summary>
        /// Used to load data on the report
        /// </summary>


        public void LoadMonthEndRecon()
        {
            _section = cbxBookingMO.EditValue.ToString();

            _month = ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)).ToString();

            if (radioType.EditValue.ToString() == "Stoping")
            {

                MonthEndReconReport_Stoping();

            }

            if (radioType.EditValue.ToString() == "Development")
            {
                MonthEndReconReport_Dev();
            }

            if (radioType.EditValue.ToString() == "Vamping")
            {
                MonthEndReconReport_Vamp();

            }

        }

        public void MonthEndReconReport_Vamp()
        {
            //Summary
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select name_3 , name_2 GroupSec " +
                                    ",SUM(plansqm) plansqm, SUM(booksqm) booksqm, SUM(meassqm) meassqm, 0 measfl, 0 bb " +
                                     "from Vamping_MonthEndRecon where prodmonth =  '" + _month + "'  " +
                                    " group by name_3 , name_2  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "StopingSummary";
            _dbMan.ExecuteInstruction();

            //Detail
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = "select * from Vamping_MonthEndRecon where prodmonth = '" + _month + "' ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ResultsTableName = "StopingDetail";
            _dbMan1.ExecuteInstruction();

            //Report
            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan2.SqlStatement = "select '" + _month + "' ProdMonth, " +
                                   " '" + _section + "' SectionID, " +
                                   "'+ExtractAfterColon(SectionsBox.Text)' Name, " +
                                   "Banner from tbl_SysSet";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ResultsTableName = "Report";
            _dbMan2.ExecuteInstruction();

            DataSet ReportDatasetReport = new DataSet();
            ReportDatasetReport.Tables.Add(_dbMan2.ResultsDataTable);

            DataSet ReportDatasetDetail = new DataSet();
            ReportDatasetDetail.Tables.Add(_dbMan1.ResultsDataTable);

            DataSet ReportDatasetSummary = new DataSet();
            ReportDatasetSummary.Tables.Add(_dbMan.ResultsDataTable);

            if (_dbMan.ResultsDataTable.Rows.Count < 1)
            {
                MessageBox.Show("No Data for selected criteria");
                return;
            }

            _theReport.RegisterData(ReportDatasetSummary);
            _theReport.RegisterData(ReportDatasetDetail);
            _theReport.RegisterData(ReportDatasetReport);


            _theReport.Load(_reportFolder + "MonthEndReconVamps.frx");

            //_theReport.Design();

            pcReport2.Clear();
            _theReport.Prepare();
            _theReport.Preview = pcReport2;
            _theReport.ShowPrepared();
            pcReport2.Visible = true;


        }



        public void MonthEndReconReport_Stoping()
        {
            //Summary
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "exec Report_MonthEndRecon_StopingSummary '" + _month + "' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "StopingSummary";
            _dbMan.ExecuteInstruction();

            //Detail
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = "exec Report_MonthEndRecon_StopingDetail '" + _month + "' ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ResultsTableName = "StopingDetail";
            _dbMan1.ExecuteInstruction();

            //Report
            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan2.SqlStatement = "select '" + _month + "' ProdMonth, " +
                                   " '" + _section + "' SectionID, " +
                                   "'+ExtractAfterColon(SectionsBox.Text)' Name, " +
                                   "Banner from tbl_SysSet";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ResultsTableName = "Report";
            _dbMan2.ExecuteInstruction();

            DataSet ReportDatasetReport = new DataSet();
            ReportDatasetReport.Tables.Add(_dbMan2.ResultsDataTable);

            DataSet ReportDatasetDetail = new DataSet();
            ReportDatasetDetail.Tables.Add(_dbMan1.ResultsDataTable);

            DataSet ReportDatasetSummary = new DataSet();
            ReportDatasetSummary.Tables.Add(_dbMan.ResultsDataTable);

            if (_dbMan.ResultsDataTable.Rows.Count < 1)
            {
                MessageBox.Show("No Data for selected criteria");
                return;
            }

            _theReport2.RegisterData(ReportDatasetSummary);
            _theReport2.RegisterData(ReportDatasetDetail);
            _theReport2.RegisterData(ReportDatasetReport);

            _theReport2.Load(_reportFolder + "MonthEndReconReport.frx");


            //_theReport2.Design();

            pcReport2.Clear();
            _theReport2.Prepare();
            _theReport2.Preview = pcReport2;
            _theReport2.ShowPrepared();
            pcReport2.Visible = true;

            Application.Idle += new System.EventHandler(this.Application_Idle2);

        }




        public void MonthEndReconReport_Dev()
        {
            //if (radioGroupDevCat.SelectedIndex == 0)
            //{
            //    Cat = "Total";
            //}
            //else if (radioGroupDevCat.SelectedIndex == 1)
            //{
            //    Cat = "Prim";
            //}
            //else
            //{
            //    Cat = "Sec";
            //}

            Cat = "Total";

            //Summary
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "exec Report_MonthEndRecon_DevSummary '" + _month + "', '" + Cat.ToString() + "' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "DevSummary";
            _dbMan.ExecuteInstruction();

            //Detail
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = "exec Report_MonthEndRecon_DevDetail '" + _month + "', '" + Cat.ToString() + "' ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ResultsTableName = "DevDetail";
            _dbMan1.ExecuteInstruction();

            //Report
            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan2.SqlStatement = "select '" + _month + "' ProdMonth, " +
                                   " '" + _section + "' SectionID, " +
                                   "'+ExtractAfterColon(SectionsBox.Text)' Name, " +
                                   "Banner from tbl_SysSet";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ResultsTableName = "Report";
            _dbMan2.ExecuteInstruction();

            DataSet ReportDatasetReport = new DataSet();
            ReportDatasetReport.Tables.Add(_dbMan2.ResultsDataTable);

            DataSet ReportDatasetDetail = new DataSet();
            ReportDatasetDetail.Tables.Add(_dbMan1.ResultsDataTable);

            DataSet ReportDatasetSummary = new DataSet();
            ReportDatasetSummary.Tables.Add(_dbMan.ResultsDataTable);

            _theReport3.RegisterData(ReportDatasetSummary);
            _theReport3.RegisterData(ReportDatasetDetail);
            _theReport3.RegisterData(ReportDatasetReport);

            _theReport3.Load(_reportFolder + "MonthEndReconReportDev.frx");

           // _theReport3.Design();

            pcReport2.Clear();
            _theReport3.Prepare();
            _theReport3.Preview = pcReport2;
            _theReport3.ShowPrepared();
            pcReport2.Visible = true;

            Application.Idle += new System.EventHandler(this.Application_Idle2);
        }


        private void Application_Idle2(object sender, EventArgs e)
        {
            ///MonthEndReconNotesSave
            /////Stoping
            if (_theReport2.GetParameterValue("MonthEndReconWPID") != null)
            {
                NotesLbl = _theReport2.GetParameterValue("MonthEndReconWPID").ToString();
            }
            //////Development
            if (_theReport3.GetParameterValue("MonthEndReconWPID") != null)
            {
                NotesLbl = _theReport3.GetParameterValue("MonthEndReconWPID").ToString();
            }
        }



        private void LoadSections()
        {
            MWDataManager.clsDataAccess _PrePlanningLoadSections = new MWDataManager.clsDataAccess();
            _PrePlanningLoadSections.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _PrePlanningLoadSections.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningLoadSections.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningLoadSections.SqlStatement = " Select moid Sectionid_2,moname Name_2 \r\n" +
                                                    "from [dbo].[tbl_sectioncomplete] \r\n" +
                                                    "where prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue.ToString())) + "' \r\n " +
                                                    " Group By moid,moname \r\n " +
                                                    " Order By moid,moname";
            _PrePlanningLoadSections.ExecuteInstruction();

            DataTable tbl_Sections = _PrePlanningLoadSections.ResultsDataTable;


            LookUpEditSection.DataSource = tbl_Sections;
            LookUpEditSection.DisplayMember = "Name_2";
            LookUpEditSection.ValueMember = "Sectionid_2";

            cbxBookingMO.EditValue = tbl_Sections.Rows[0]["Sectionid_2"].ToString();
        }
        #endregion Methods 

        private void pnlSideBar_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadMonthEndRecon();
            }
        }

        private void cbxBookingMO_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadMonthEndRecon();
            }
        }

        private void radioType_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadMonthEndRecon();
            }
        }
    }
}
