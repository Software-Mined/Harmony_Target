using DevExpress.XtraEditors;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting.WorkplaceSummary
{
    public partial class ucReport : BaseUserControl
    {
        clsWorkplaceSummary _clsWorkplaceSummary = new clsWorkplaceSummary();
        private string ReportsFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        private string ImageFolder = ProductionGlobal.ProductionGlobalTSysSettings._RepDir;
        Report theReportRERoutineVisit = new Report();
        Report theReportGeoRoutineVisit = new Report();
        Report theReportVentRoutineVisit = new Report();

        Report theReportREPrePlan = new Report();
        Report theReportGeoPrePlan = new Report();
        Report theReportSafetyPrePlan = new Report();
        Report theReportVentPrePlan = new Report();
        Report theReportEngPrePlan = new Report();
        Report theReportSurveyPrePlan = new Report();
        Procedures procs = new Procedures();
        string Department = "";
        string Type = "";
        string Workplace = "";
        string CaptDate = "";


        //Ventillation
        public string _picPath;
        public string ImageForReport;
        public string _Auth;

        //PrePlan
        public String _Attachment;
        public String _AttachmentReturned;
        string WP1Desc = string.Empty;
        string WP2Desc = string.Empty;
        string WP3Desc = string.Empty;
        string WP4Desc = string.Empty;
        string WP5Desc = string.Empty;
        string WP1 = string.Empty;
        string WP2 = string.Empty;
        string WP3 = string.Empty;
        string WP4 = string.Empty;
        string WP5 = string.Empty;

        public ucReport()
        {
            InitializeComponent();
        }

        private void ucReport_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        void LoadReport()
        {
            //Walkabouts
            if (clsWorkplaceSummary.SelectedWorkplace != "")
            {
                if (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Department)
                {
                    Department = clsWorkplaceSummary.Department;
                    Type = clsWorkplaceSummary.Type;

                    if (clsWorkplaceSummary.SelectedWorkplace != Workplace || CaptDate != clsWorkplaceSummary.SelectedCaptDate.ToString())
                    {
                        Workplace = clsWorkplaceSummary.SelectedWorkplace;
                        CaptDate = clsWorkplaceSummary.SelectedCaptDate.ToString();

                        if (clsWorkplaceSummary.Type == "Walkabout" && clsWorkplaceSummary.Department == "Rock Engineering")
                        {
                            if (clsWorkplaceSummary.SelectedActivity == "Stp" || clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                MWDataManager.clsDataAccess _dbManWPST21 = new MWDataManager.clsDataAccess();
                                _dbManWPST21.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManWPST21.SqlStatement = "select * from  tbl_LicenceToOperate_Seismic where wpdescription = '" + clsWorkplaceSummary.SelectedWorkplace + "' order by thedate desc";

                                _dbManWPST21.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManWPST21.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManWPST21.ResultsTableName = "Graph";
                                _dbManWPST21.ExecuteInstruction();

                                DataSet dsABS111 = new DataSet();
                                dsABS111.Tables.Add(_dbManWPST21.ResultsDataTable);

                                theReportRERoutineVisit.RegisterData(dsABS111);

                                MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
                                _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManWPST2.SqlStatement = "select top (20) * from (   \r\n" +
                                                            "Select 'Z' bb,  \r\n" +
                                                            "case when targetdate is null then 'Not Accepted'  \r\n" +
                                                            "when targetdate is not null then 'Accepted'   \r\n" +
                                                            " when CompletionDate is not null then 'Completed'  \r\n" +
                                                            " when VerificationDate is not null then 'Verified' else '' end as ActionStatus   \r\n" +
                                                            " ,[description] as Action, thedate datesubmitted, datediff(day, thedate, getdate()) ss   \r\n" +
                                                            " from[dbo].[tbl_Shec_Incidents]  \r\n" +
                                                            " where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'   \r\n" +
                                                            " union all \r\n" +
                                                            " select 'a' , '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'b ', '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'c  ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'd   ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'e    ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'f     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'g     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'h     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'i     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'j     ' , '', '', null, '' \r\n" +
                                                            " )a \r\n" +
                                                            "  order  by bb  desc,datesubmitted \r\n";
                                _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManWPST2.ResultsTableName = "Table2";
                                _dbManWPST2.ExecuteInstruction();

                                DataSet dsABS1 = new DataSet();
                                dsABS1.Tables.Add(_dbManWPST2.ResultsDataTable);

                                theReportRERoutineVisit.RegisterData(dsABS1);

                                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, " +
                                                    " RiskRating Risk, '" + clsWorkplaceSummary.SelectedRiskRating + "' rr,  * from [tbl_DPT_RockMechInspection] " +
                                                     "  where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'\r\n" +
                                                    "   and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) \r\n" +
                                                     "  and captyear = (select DATEPART(YYYY, '" + clsWorkplaceSummary.SelectedCaptDate + "'))\r\n";
                                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbMan.ResultsTableName = "DevSummary";
                                _dbMan.ExecuteInstruction();

                                if (_dbMan.ResultsDataTable.Rows.Count > 0)
                                {

                                    string BlankImage = Application.StartupPath + "\\" + "Neil.bmp";

                                    MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                                    _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                                    _dbManImage.SqlStatement = "Select RiskRating Risk,'" + BlankImage + "' pp, Picture,document1,document2  \r\n" +
                                                        "from [tbl_DPT_RockMechInspection] \r\n" +
                                                         "  where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'\r\n" +
                                                    "   and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) \r\n" +
                                                     "  and captyear = (select DATEPART(YYYY, '" + clsWorkplaceSummary.SelectedCaptDate + "'))\r\n";
                                    _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                    _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                                    _dbManImage.ResultsTableName = "Image";
                                    _dbManImage.ExecuteInstruction();

                                    DataSet ReportDatasetReport = new DataSet();
                                    ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                                    theReportRERoutineVisit.RegisterData(ReportDatasetReport);

                                    DataSet ReportDatasetReportImage = new DataSet();
                                    ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                                    theReportRERoutineVisit.RegisterData(ReportDatasetReportImage);

                                    theReportRERoutineVisit.Load(ReportsFolder + "\\RockEng.frx");

                                    //theReport3.Design();

                                    pcReport.Clear();
                                    theReportRERoutineVisit.Prepare();
                                    theReportRERoutineVisit.Preview = pcReport;
                                    theReportRERoutineVisit.ShowPrepared();

                                }
                            }
                            if (clsWorkplaceSummary.SelectedActivity == "Ledge")
                            {
                                Department = clsWorkplaceSummary.Department;
                                Type = clsWorkplaceSummary.Type;
                                Workplace = clsWorkplaceSummary.SelectedWorkplace;
                                CaptDate = clsWorkplaceSummary.SelectedCaptDate.ToString();
                                MWDataManager.clsDataAccess _dbManWPST21 = new MWDataManager.clsDataAccess();
                                _dbManWPST21.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManWPST21.SqlStatement = "select * from  tbl_LicenceToOperate_Seismic where wpdescription = '" + clsWorkplaceSummary.SelectedWorkplace + "' order by thedate desc";

                                _dbManWPST21.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManWPST21.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManWPST21.ResultsTableName = "Graph";
                                _dbManWPST21.ExecuteInstruction();

                                DataSet dsABS111 = new DataSet();
                                dsABS111.Tables.Add(_dbManWPST21.ResultsDataTable);

                                theReportRERoutineVisit.RegisterData(dsABS111);

                                MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
                                _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManWPST2.SqlStatement = "select top (20) * from (   \r\n" +
                                                            "Select 'Z' bb,  \r\n" +
                                                            "case when targetdate is null then 'Not Accepted'  \r\n" +
                                                            "when targetdate is not null then 'Accepted'   \r\n" +
                                                            " when CompletionDate is not null then 'Completed'  \r\n" +
                                                            " when VerificationDate is not null then 'Verified' else '' end as ActionStatus   \r\n" +
                                                            " ,[description] as Action, thedate datesubmitted, datediff(day, thedate, getdate()) ss   \r\n" +
                                                            " from[dbo].[tbl_Shec_Incidents]  \r\n" +
                                                            " where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'   \r\n" +
                                                            " union all \r\n" +
                                                            " select 'a' , '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'b ', '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'c  ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'd   ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'e    ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'f     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'g     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'h     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'i     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'j     ' , '', '', null, '' \r\n" +
                                                            " )a \r\n" +
                                                            "  order  by bb  desc,datesubmitted \r\n";
                                _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManWPST2.ResultsTableName = "Table2";
                                _dbManWPST2.ExecuteInstruction();

                                DataSet dsABS1 = new DataSet();
                                dsABS1.Tables.Add(_dbManWPST2.ResultsDataTable);

                                theReportRERoutineVisit.RegisterData(dsABS1);

                                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, " +
                                                    " RiskRating Risk, '" + clsWorkplaceSummary.SelectedRiskRating + "' rr,  * from [tbl_DPT_RockMechInspection] " +
                                                     "  where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'\r\n" +
                                                    "   and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) \r\n" +
                                                     "  and captyear = (select DATEPART(YYYY, '" + clsWorkplaceSummary.SelectedCaptDate + "'))\r\n";
                                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbMan.ResultsTableName = "DevSummary";
                                _dbMan.ExecuteInstruction();

                                if (_dbMan.ResultsDataTable.Rows.Count > 0)
                                {

                                    string BlankImage = Application.StartupPath + "\\" + "Neil.bmp";

                                    MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                                    _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                                    _dbManImage.SqlStatement = "Select RiskRating Risk,'" + BlankImage + "' pp, Picture,document1,document2  \r\n" +
                                                        "from [tbl_DPT_RockMechInspection] \r\n" +
                                                         "  where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'\r\n" +
                                                    "   and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) \r\n" +
                                                     "  and captyear = (select DATEPART(YYYY, '" + clsWorkplaceSummary.SelectedCaptDate + "'))\r\n";
                                    _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                    _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                                    _dbManImage.ResultsTableName = "Image";
                                    _dbManImage.ExecuteInstruction();

                                    DataSet ReportDatasetReport = new DataSet();
                                    ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                                    theReportRERoutineVisit.RegisterData(ReportDatasetReport);

                                    DataSet ReportDatasetReportImage = new DataSet();
                                    ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                                    theReportRERoutineVisit.RegisterData(ReportDatasetReportImage);

                                    theReportRERoutineVisit.Load(ReportsFolder + "\\RockEngLedge.frx");

                                    //theReport3.Design();

                                    pcReport.Clear();
                                    theReportRERoutineVisit.Prepare();
                                    theReportRERoutineVisit.Preview = pcReport;
                                    theReportRERoutineVisit.ShowPrepared();

                                }
                            }
                            if (clsWorkplaceSummary.SelectedActivity == "Vamps")
                            {
                                Department = clsWorkplaceSummary.Department;
                                Type = clsWorkplaceSummary.Type;
                                Workplace = clsWorkplaceSummary.SelectedWorkplace;
                                CaptDate = clsWorkplaceSummary.SelectedCaptDate.ToString();
                                MWDataManager.clsDataAccess _dbManWPST21 = new MWDataManager.clsDataAccess();
                                _dbManWPST21.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManWPST21.SqlStatement = "select * from  tbl_LicenceToOperate_Seismic where wpdescription = '" + clsWorkplaceSummary.SelectedWorkplace + "' order by thedate desc";

                                _dbManWPST21.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManWPST21.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManWPST21.ResultsTableName = "Graph";
                                _dbManWPST21.ExecuteInstruction();

                                DataSet dsABS111 = new DataSet();
                                dsABS111.Tables.Add(_dbManWPST21.ResultsDataTable);

                                theReportRERoutineVisit.RegisterData(dsABS111);

                                MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
                                _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManWPST2.SqlStatement = "select top (20) * from (   \r\n" +
                                                            "Select 'Z' bb,  \r\n" +
                                                            "case when targetdate is null then 'Not Accepted'  \r\n" +
                                                            "when targetdate is not null then 'Accepted'   \r\n" +
                                                            " when CompletionDate is not null then 'Completed'  \r\n" +
                                                            " when VerificationDate is not null then 'Verified' else '' end as ActionStatus   \r\n" +
                                                            " ,[description] as Action, thedate datesubmitted, datediff(day, thedate, getdate()) ss   \r\n" +
                                                            " from[dbo].[tbl_Shec_Incidents]  \r\n" +
                                                            " where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'   \r\n" +
                                                            " union all \r\n" +
                                                            " select 'a' , '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'b ', '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'c  ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'd   ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'e    ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'f     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'g     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'h     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'i     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'j     ' , '', '', null, '' \r\n" +
                                                            " )a \r\n" +
                                                            "  order  by bb  desc,datesubmitted \r\n";
                                _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManWPST2.ResultsTableName = "Table2";
                                _dbManWPST2.ExecuteInstruction();

                                DataSet dsABS1 = new DataSet();
                                dsABS1.Tables.Add(_dbManWPST2.ResultsDataTable);

                                theReportRERoutineVisit.RegisterData(dsABS1);

                                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, " +
                                                    " RiskRating Risk, '" + clsWorkplaceSummary.SelectedRiskRating + "' rr,  * from [tbl_DPT_RockMechInspection_Vamping] " +
                                                     "  where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'\r\n" +
                                                    "   and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) \r\n" +
                                                     "  and captyear = (select DATEPART(YYYY, '" + clsWorkplaceSummary.SelectedCaptDate + "'))\r\n";
                                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbMan.ResultsTableName = "DevSummary";
                                _dbMan.ExecuteInstruction();

                                if (_dbMan.ResultsDataTable.Rows.Count > 0)
                                {

                                    string BlankImage = Application.StartupPath + "\\" + "Neil.bmp";

                                    MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                                    _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                                    _dbManImage.SqlStatement = "Select RiskRating Risk,'" + BlankImage + "' pp, Picture,document1,document2  \r\n" +
                                                        "from [tbl_DPT_RockMechInspection_Vamping] \r\n" +
                                                         "  where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'\r\n" +
                                                    "   and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) \r\n" +
                                                     "  and captyear = (select DATEPART(YYYY, '" + clsWorkplaceSummary.SelectedCaptDate + "'))\r\n";
                                    _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                    _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                                    _dbManImage.ResultsTableName = "Image";
                                    _dbManImage.ExecuteInstruction();

                                    DataSet ReportDatasetReport = new DataSet();
                                    ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                                    theReportRERoutineVisit.RegisterData(ReportDatasetReport);

                                    DataSet ReportDatasetReportImage = new DataSet();
                                    ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                                    theReportRERoutineVisit.RegisterData(ReportDatasetReportImage);

                                    theReportRERoutineVisit.Load(ReportsFolder + "\\RockEngVamp.frx");

                                    //theReport3.Design();

                                    pcReport.Clear();
                                    theReportRERoutineVisit.Prepare();
                                    theReportRERoutineVisit.Preview = pcReport;
                                    theReportRERoutineVisit.ShowPrepared();

                                }
                            }
                        }

                        else if (clsWorkplaceSummary.Type == "Walkabout" && clsWorkplaceSummary.Department == "Ventillation")
                        {
                            int Days = 45;
                            int Activity = 0;
                            if (clsWorkplaceSummary.SelectedActivity == "Stp")
                            {
                                MWDataManager.clsDataAccess _dbManField = new MWDataManager.clsDataAccess();
                                _dbManField.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManField.SqlStatement = "select  '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' mine, * from tbl_Dept_Inspection_VentCapture_FeildBook where calendardate = '" + clsWorkplaceSummary.SelectedCaptDate + "' and Section = '" + clsWorkplaceSummary.SelectedMinerSection + "' ";
                                _dbManField.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManField.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManField.ResultsTableName = "FieldBook";  //get table name
                                _dbManField.ExecuteInstruction();

                                DataSet ReportDataTable1 = new DataSet();
                                ReportDataTable1.Tables.Add(_dbManField.ResultsDataTable);

                                //Per Workplace Data
                                MWDataManager.clsDataAccess _dbManbanner = new MWDataManager.clsDataAccess();
                                _dbManbanner.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManbanner.SqlStatement = "exec sp_Dept_Insection_VentStoping_Questions " + Days + ", '" + clsWorkplaceSummary.SelectedMinerSection + "', '" + clsWorkplaceSummary.SelectedCaptDate + "' ";
                                _dbManbanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManbanner.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManbanner.ResultsTableName = "StopingData";  //get table name
                                _dbManbanner.ExecuteInstruction();

                                DataSet ReportDataTable = new DataSet();
                                ReportDataTable.Tables.Add(_dbManbanner.ResultsDataTable);


                                //General
                                MWDataManager.clsDataAccess _dbGenData = new MWDataManager.clsDataAccess();
                                _dbGenData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbGenData.SqlStatement = "exec sp_Dept_Insection_Vent_Questions_Stoping_PerSection '" + clsWorkplaceSummary.SelectedCaptDate + "'," +
                                " '" + clsWorkplaceSummary.SelectedMinerSection + "' ";
                                _dbGenData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbGenData.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbGenData.ResultsTableName = "StopingGeneralData";  //get table name
                                _dbGenData.ExecuteInstruction();

                                DataSet genDt = new DataSet();
                                genDt.Tables.Add(_dbGenData.ResultsDataTable);

                                ///Refuge Bay
                                ///
                                MWDataManager.clsDataAccess _dbRefBayData = new MWDataManager.clsDataAccess();
                                _dbRefBayData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbRefBayData.SqlStatement = "exec sp_Dept_Insection_Vent_RefugeBay '" + clsWorkplaceSummary.SelectedProdmonth + "', '" + clsWorkplaceSummary.SelectedCaptDate + "' " +
                                " ,'" + clsWorkplaceSummary.SelectedMinerSection + "', " + Activity + " ";
                                _dbRefBayData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbRefBayData.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbRefBayData.ResultsTableName = "RefugeBayData";  //get table name
                                _dbRefBayData.ExecuteInstruction();

                                DataSet dtRef = new DataSet();
                                dtRef.Tables.Add(_dbRefBayData.ResultsDataTable);

                                //Available Temperature
                                MWDataManager.clsDataAccess _AvlTemp = new MWDataManager.clsDataAccess();
                                _AvlTemp.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _AvlTemp.SqlStatement = "exec sp_Dept_Insection_Vent_Questions_Stoping_AvailableTemp '" + clsWorkplaceSummary.SelectedCaptDate + "','" + clsWorkplaceSummary.SelectedMinerSection + "', " + Activity + " ";
                                _AvlTemp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _AvlTemp.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _AvlTemp.ResultsTableName = "AvailableTemp";  //get table name
                                _AvlTemp.ExecuteInstruction();

                                DataSet dsAvlTemp = new DataSet();
                                dsAvlTemp.Tables.Add(_AvlTemp.ResultsDataTable);

                                //Noise Measurements
                                MWDataManager.clsDataAccess _NoiseData = new MWDataManager.clsDataAccess();
                                _NoiseData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _NoiseData.SqlStatement = "exec sp_Dept_Insection_VentNoiseMeasurement  " + Days + ",  " + Activity + ", '" + clsWorkplaceSummary.SelectedMinerSection + "','" + clsWorkplaceSummary.SelectedCaptDate + "' ";
                                _NoiseData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _NoiseData.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _NoiseData.ResultsTableName = "NoiseMeasurements";  //get table name
                                _NoiseData.ExecuteInstruction();

                                DataSet dsNoise = new DataSet();
                                dsNoise.Tables.Add(_NoiseData.ResultsDataTable);



                                ///HeaderData
                                ///
                                MWDataManager.clsDataAccess _dbHeaderData = new MWDataManager.clsDataAccess();
                                _dbHeaderData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbHeaderData.SqlStatement = "Select * from tblclsWorkplaceSummary.SelectedMOSectionComplete where sectionid = '" + clsWorkplaceSummary.SelectedMinerSection + "' \r\n" +
                                                            "and prodmonth = (Select max(prodmonth) from tbl_Planning where calendardate = '" + clsWorkplaceSummary.SelectedCaptDate + "' ) ";
                                _dbHeaderData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbHeaderData.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbHeaderData.ResultsTableName = "HeaderData";  //get table name
                                _dbHeaderData.ExecuteInstruction();

                                DataSet dsHeader = new DataSet();
                                dsHeader.Tables.Add(_dbHeaderData.ResultsDataTable);

                                //Actions
                                MWDataManager.clsDataAccess _dbManInc = new MWDataManager.clsDataAccess();
                                _dbManInc.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManInc.SqlStatement = "select * from tbl_Shec_IncidentsVent where thedate = '" + clsWorkplaceSummary.SelectedCaptDate + "' and WPType = '" + clsWorkplaceSummary.SelectedMinerSection + "' and Type = 'VSA' ";
                                _dbManInc.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManInc.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManInc.ResultsTableName = "Incident";  //get table name
                                _dbManInc.ExecuteInstruction();

                                DataSet DSIncident = new DataSet();
                                DSIncident.Tables.Add(_dbManInc.ResultsDataTable);

                                theReportVentRoutineVisit.RegisterData(ReportDataTable1);
                                theReportVentRoutineVisit.RegisterData(ReportDataTable);
                                theReportVentRoutineVisit.RegisterData(genDt);
                                theReportVentRoutineVisit.RegisterData(dsAvlTemp);
                                theReportVentRoutineVisit.RegisterData(dsNoise);
                                theReportVentRoutineVisit.RegisterData(dtRef);
                                theReportVentRoutineVisit.RegisterData(dsHeader);
                                theReportVentRoutineVisit.RegisterData(DSIncident);

                                theReportVentRoutineVisit.Load(ReportsFolder + "MasterStopeReport.frx");

                                theReportVentRoutineVisit.SetParameterValue("VetnImage", ImageForReport);
                                theReportVentRoutineVisit.SetParameterValue("PicPath", _picPath);
                                theReportVentRoutineVisit.SetParameterValue("Auth", _Auth);

                                //theReportVentRoutineVisit.Design();

                                pcReport.Clear();
                                theReportVentRoutineVisit.Prepare();
                                theReportVentRoutineVisit.Preview = pcReport;
                                theReportVentRoutineVisit.ShowPrepared();
                            }

                            if (clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                Activity = 1;

                                MWDataManager.clsDataAccess _dbManField = new MWDataManager.clsDataAccess();
                                _dbManField.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManField.SqlStatement = "select  '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' mine, * from tbl_Dept_Inspection_VentCapture_FeildBook where calendardate = '" + clsWorkplaceSummary.SelectedCaptDate + "' and Section = '" + clsWorkplaceSummary.SelectedMinerSection + "'  ";
                                _dbManField.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManField.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManField.ResultsTableName = "FieldBook";  //get table name
                                _dbManField.ExecuteInstruction();

                                DataSet ReportDataTableFB = new DataSet();
                                ReportDataTableFB.Tables.Add(_dbManField.ResultsDataTable);


                                MWDataManager.clsDataAccess _dbManbanner = new MWDataManager.clsDataAccess();
                                _dbManbanner.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManbanner.SqlStatement = "Exec sp_Dept_Insection_VentDevelopment_Questions " + Days + ",'" + clsWorkplaceSummary.SelectedMinerSection + "','" + clsWorkplaceSummary.SelectedCaptDate + "' ";
                                _dbManbanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManbanner.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManbanner.ResultsTableName = "DevMainData";  //get table name
                                _dbManbanner.ExecuteInstruction();

                                DataSet ReportDatasetbanner = new DataSet();
                                ReportDatasetbanner.Tables.Add(_dbManbanner.ResultsDataTable);


                                //Per Section
                                MWDataManager.clsDataAccess _dbManbannersec = new MWDataManager.clsDataAccess();
                                _dbManbannersec.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManbannersec.SqlStatement = "exec sp_Dept_Insection_Vent_Questions_Stoping_PerSection_Report '" + clsWorkplaceSummary.SelectedCaptDate + "', '" + clsWorkplaceSummary.SelectedMinerSection + "' ";
                                _dbManbannersec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManbannersec.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManbannersec.ResultsTableName = "DevPerSection";  //get table name
                                _dbManbannersec.ExecuteInstruction();

                                DataSet ReportDataTableSect = new DataSet();
                                ReportDataTableSect.Tables.Add(_dbManbannersec.ResultsDataTable);

                                ///Refuge Bay
                                ///
                                MWDataManager.clsDataAccess _dbRefBayData = new MWDataManager.clsDataAccess();
                                _dbRefBayData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbRefBayData.SqlStatement = "exec sp_Dept_Insection_Vent_RefugeBay '" + clsWorkplaceSummary.SelectedProdmonth + "', '" + clsWorkplaceSummary.SelectedCaptDate + "' " +
                                " ,'" + clsWorkplaceSummary.SelectedMinerSection + "', " + Activity + " ";
                                _dbRefBayData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbRefBayData.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbRefBayData.ResultsTableName = "RefugeBayData";  //get table name
                                _dbRefBayData.ExecuteInstruction();

                                DataSet dtRef = new DataSet();
                                dtRef.Tables.Add(_dbRefBayData.ResultsDataTable);

                                //Available Temperature
                                MWDataManager.clsDataAccess _AvlTemp = new MWDataManager.clsDataAccess();
                                _AvlTemp.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _AvlTemp.SqlStatement = "exec sp_Dept_Insection_Vent_Questions_Stoping_AvailableTemp '" + clsWorkplaceSummary.SelectedCaptDate + "','" + clsWorkplaceSummary.SelectedMinerSection + "', " + Activity + " ";
                                _AvlTemp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _AvlTemp.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _AvlTemp.ResultsTableName = "AvailableTemp";  //get table name
                                _AvlTemp.ExecuteInstruction();

                                DataSet dsAvlTemp = new DataSet();
                                dsAvlTemp.Tables.Add(_AvlTemp.ResultsDataTable);

                                //Noise Measurements
                                MWDataManager.clsDataAccess _NoiseData = new MWDataManager.clsDataAccess();
                                _NoiseData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _NoiseData.SqlStatement = "exec sp_Dept_Insection_VentNoiseMeasurement  " + Days + ",  " + Activity + ", '" + clsWorkplaceSummary.SelectedMinerSection + "','" + clsWorkplaceSummary.SelectedCaptDate + "' ";
                                _NoiseData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _NoiseData.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _NoiseData.ResultsTableName = "NoiseMeasurements";  //get table name
                                _NoiseData.ExecuteInstruction();

                                DataSet dsNoise = new DataSet();
                                dsNoise.Tables.Add(_NoiseData.ResultsDataTable);

                                ///HeaderData
                                ///
                                MWDataManager.clsDataAccess _dbManbanner1 = new MWDataManager.clsDataAccess();
                                _dbManbanner1.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManbanner1.SqlStatement = "Select *, '" + UserID + "' insp,  '" + String.Format("{0:yyyy-MM-dd}", clsWorkplaceSummary.SelectedCaptDate) + "'  dd from tblclsWorkplaceSummary.SelectedMOSectionComplete where sectionid = '" + clsWorkplaceSummary.SelectedMinerSection + "' \r\n" +
                                                            "and prodmonth = (Select max(prodmonth) from tbl_Planning where calendardate = '" + clsWorkplaceSummary.SelectedCaptDate + "' ) ";
                                _dbManbanner1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManbanner1.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManbanner1.ResultsTableName = "HeaderData";  //get table name
                                _dbManbanner1.ExecuteInstruction();

                                MWDataManager.clsDataAccess _dbManInc = new MWDataManager.clsDataAccess();
                                _dbManInc.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManInc.SqlStatement = "select * from tbl_Shec_IncidentsVent where thedate = '" + clsWorkplaceSummary.SelectedCaptDate + "' and WPType = '" + clsWorkplaceSummary.SelectedMinerSection + "' and Type = 'VSA' ";
                                _dbManInc.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManInc.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManInc.ResultsTableName = "Incident";  //get table name
                                _dbManInc.ExecuteInstruction();

                                DataSet DSIncident = new DataSet();
                                DSIncident.Tables.Add(_dbManInc.ResultsDataTable);

                                DataSet dsHeader = new DataSet();
                                dsHeader.Tables.Add(_dbManbanner1.ResultsDataTable);

                                theReportVentRoutineVisit.RegisterData(ReportDataTableFB);
                                theReportVentRoutineVisit.RegisterData(ReportDatasetbanner);
                                theReportVentRoutineVisit.RegisterData(ReportDataTableSect);
                                theReportVentRoutineVisit.RegisterData(dsHeader);
                                theReportVentRoutineVisit.RegisterData(DSIncident);
                                theReportVentRoutineVisit.RegisterData(dtRef);
                                theReportVentRoutineVisit.RegisterData(dsAvlTemp);
                                theReportVentRoutineVisit.RegisterData(dsNoise);

                                theReportVentRoutineVisit.Load(ReportsFolder + "MasterStopeDevReport.frx");

                                theReportVentRoutineVisit.SetParameterValue("VetnImage", ImageForReport);
                                theReportVentRoutineVisit.SetParameterValue("PicPath", _picPath);
                                theReportVentRoutineVisit.SetParameterValue("Auth", _Auth);

                                theReportVentRoutineVisit.Design();

                                pcReport.Clear();
                                theReportVentRoutineVisit.Prepare();
                                theReportVentRoutineVisit.Preview = pcReport;
                                theReportVentRoutineVisit.ShowPrepared();
                            }

                            //if (_FrmType == "Other")
                            //{

                            //    MWDataManager.clsDataAccess _batDb = new MWDataManager.clsDataAccess();
                            //    _batDb.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            //    _batDb.SqlStatement = " exec sp_Dept_Insection_Vent_OtherQuestions '" + String.Format("{0:yyyy-MM-dd}", clsWorkplaceSummary.SelectedCaptDate) + "', \r\n "
                            //    + " '" + clsWorkplaceSummary.SelectedMinerSection + "', '" + clsWorkplaceSummary.SelectedWorkplace + "', '" + _ucCheckListID + "' \r\n ";
                            //    _batDb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            //    _batDb.queryReturnType = MWDataManager.ReturnType.DataTable;


                            //    //Actions                
                            //    MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                            //    _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            //    _dbMan.SqlStatement = "select * from  tbl_Shec_IncidentsVent where Workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "' and TheDate = '" + String.Format("{0:yyyy-MM-dd}", clsWorkplaceSummary.SelectedCaptDate) + "' ";
                            //    _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            //    _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                            //    _dbMan.ResultsTableName = "Actions";
                            //    _dbMan.ExecuteInstruction();

                            //    DataSet ds1 = new DataSet();
                            //    ds1.Tables.Add(_dbMan.ResultsDataTable);

                            //    theReportVentRoutineVisit.RegisterData(ds1);

                            //    if (_ucCheckListID == "Battery Bay")
                            //    {
                            //        _batDb.ResultsTableName = "BatteryBayData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);

                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\BatteryBay.frx");

                            //    }


                            //    if (_ucCheckListID == "Booster Fans")
                            //    {
                            //        _batDb.ResultsTableName = "BoosterFansData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\BoosterFans.frx");

                            //    }


                            //    if (_ucCheckListID == "UG Conveyor Belt")
                            //    {
                            //        _batDb.ResultsTableName = "UGConveyorBeltData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\UGConveyorBelt.frx");

                            //    }


                            //    if (_ucCheckListID == "Mini Subs")
                            //    {
                            //        _batDb.ResultsTableName = "MiniSubsData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\MiniSubs.frx");

                            //    }


                            //    if (_ucCheckListID == "Sub Station")
                            //    {
                            //        _batDb.ResultsTableName = "SubStationData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\SubStation.frx");

                            //    }


                            //    if (_ucCheckListID == "UG Store")
                            //    {
                            //        _batDb.ResultsTableName = "UGStoreData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\UGStore.frx");

                            //    }


                            //    if (_ucCheckListID == "Toilets UI")
                            //    {
                            //        _batDb.ResultsTableName = "ToiletsUIData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\ToiletsUI.frx");

                            //    }


                            //    if (_ucCheckListID == "Loco Survey")
                            //    {
                            //        _batDb.ResultsTableName = "LocoSurveyData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\LocoSurvey.frx");

                            //    }


                            //    if (_ucCheckListID == "Workshop - Completed")
                            //    {
                            //        _batDb.ResultsTableName = "WorkshopCompletedData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\WorkshopCompleted.frx");

                            //    }


                            //    if (_ucCheckListID == "Diamond Drill Inspection")
                            //    {
                            //        _batDb.ResultsTableName = "DiamondDrillData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\DiamondDrillInspection.frx");
                            //    }



                            //    if (_ucCheckListID == "Volume")
                            //    {
                            //        _batDb.ResultsTableName = "VolumeData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\Volume.frx");

                            //    }

                            //    if (_ucCheckListID == "Change House Laundry")
                            //    {
                            //        _batDb.ResultsTableName = "ChangeHouseLaundryData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\ChangeHouseLaundry.frx");

                            //    }

                            //    if (_ucCheckListID == "Trackless Equipment Inspection")
                            //    {
                            //        _batDb.ResultsTableName = "TracklessEquipmentData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);


                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\TracklessEquipment.frx");

                            //    }

                            //    if (_ucCheckListID == "Refuge Bay")
                            //    {
                            //        _batDb.ResultsTableName = "RefugeBayData";  //get table name
                            //        _batDb.ExecuteInstruction();

                            //        DataSet batDs = new DataSet();
                            //        batDs.Tables.Add(_batDb.ResultsDataTable);

                            //        theReportVentRoutineVisit.RegisterData(batDs);

                            //        theReportVentRoutineVisit.Load(TGlobalItems.ReportsFolder + "\\RefugeBay.frx");

                            //    }

                            //    //Parameters
                            //    theReportVentRoutineVisit.SetParameterValue("Banner", ProductionGlobal.ProductionAmplatsGlobalTSysSettings._Banner);
                            //    theReportVentRoutineVisit.SetParameterValue("Image", _picPath);
                            //    theReportVentRoutineVisit.SetParameterValue("Workplace", clsWorkplaceSummary.SelectedWorkplace);
                            //    theReportVentRoutineVisit.SetParameterValue("Section", clsWorkplaceSummary.SelectedMinerSection);
                            //    theReportVentRoutineVisit.SetParameterValue("CaptureDate", String.Format("{0:yyyy-MM-dd}", clsWorkplaceSummary.SelectedCaptDate));
                            //    theReportVentRoutineVisit.SetParameterValue("Auth", _Auth);

                            //    //theReportVentRoutineVisit.Design();

                            //    pcReport.Clear();
                            //    theReportVentRoutineVisit.Prepare();
                            //    theReportVentRoutineVisit.Preview = pcReport;
                            //    theReportVentRoutineVisit.ShowPrepared();
                            //}

                        }
                        else if (clsWorkplaceSummary.Type == "Walkabout" && clsWorkplaceSummary.Department == "Geology")
                        {
                            if (clsWorkplaceSummary.SelectedActivity == "Stp")
                            {
                                MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
                                _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManWPST2.SqlStatement = "select top (20) * from (   \r\n" +
                                                            "Select 'Z' bb,  \r\n" +
                                                            "case when targetdate is null then 'Not Accepted'  \r\n" +
                                                            "when targetdate is not null then 'Accepted'   \r\n" +
                                                            " when CompletionDate is not null then 'Completed'  \r\n" +
                                                            " when VerificationDate is not null then 'Verified' else '' end as ActionStatus   \r\n" +
                                                            " ,[description] as Action, thedate datesubmitted, datediff(day, thedate, getdate()) ss   \r\n" +
                                                            " from[dbo].[tbl_Shec_Incidents]  \r\n" +
                                                            " where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'   \r\n" +


                                                            " union all \r\n" +
                                                            " select 'a' , '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'b ', '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'c  ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'd   ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'e    ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'f     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'g     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'h     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'i     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'j     ' , '', '', null, '' \r\n" +
                                                            " )a \r\n" +
                                                            "  order  by bb  desc,datesubmitted \r\n";
                                _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManWPST2.ResultsTableName = "Table2";
                                _dbManWPST2.ExecuteInstruction();

                                DataSet dsABS1 = new DataSet();
                                dsABS1.Tables.Add(_dbManWPST2.ResultsDataTable);

                                theReportGeoRoutineVisit.RegisterData(dsABS1);

                                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, '" + clsWorkplaceSummary.SelectedRiskRating + "' rr,  * from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "' and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) and captyear =  (select DATEPART(YYYY, '" + clsWorkplaceSummary.SelectedCaptDate + "')) ";
                                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbMan.ResultsTableName = "DevSummary";
                                _dbMan.ExecuteInstruction();

                                MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                                _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManImage.SqlStatement = "Select picture, Picture1 from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "' and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) ";
                                _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManImage.ResultsTableName = "Image";
                                _dbManImage.ExecuteInstruction();

                                MWDataManager.clsDataAccess _dbManChart = new MWDataManager.clsDataAccess();
                                _dbManChart.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManChart.SqlStatement = " select top(10) * from (select description, a.CalendarDate aa ,substring(convert(varchar(11),a.CalendarDate),0,12) Calendardate,a.SWidth,a.CorrCut,a.Hangwall,a.Footwall, " +
                                                           " case when a.allocatedWidth = 0 then null else a.allocatedWidth end as allocatedWidth ,a.Notes from [dbo].[tbl_SAMPLING_Imported_Notes] a  \r\n" +
                                                  " left outer  join tbl_Workplace_Total w on convert(varchar(50),a.gmsiwpis) = w.gmsiwpid  \r\n" +
                                                  " and calendardate > getdate()-2000 ) a where description = '" + clsWorkplaceSummary.SelectedWorkplace + "'    \r\n" +
                                                  "  order by aa desc  ";
                                _dbManChart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManChart.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManChart.ResultsTableName = "Chart";
                                _dbManChart.ExecuteInstruction();

                                DataSet ReportDatasetReport = new DataSet();
                                ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                                theReportGeoRoutineVisit.RegisterData(ReportDatasetReport);

                                DataSet ReportDatasetReportImage = new DataSet();
                                ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                                theReportGeoRoutineVisit.RegisterData(ReportDatasetReportImage);

                                DataSet ReportDatasetChart = new DataSet();
                                ReportDatasetChart.Tables.Add(_dbManChart.ResultsDataTable);

                                theReportGeoRoutineVisit.RegisterData(ReportDatasetChart);

                                theReportGeoRoutineVisit.Load(ReportsFolder + "\\GeoInsp.frx");

                                //theReport3.Design();

                                pcReport.Clear();
                                theReportGeoRoutineVisit.Prepare();
                                theReportGeoRoutineVisit.Preview = pcReport;
                                theReportGeoRoutineVisit.ShowPrepared();                              

                            }
                            if (clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
                                _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManWPST2.SqlStatement = "select top (20) * from (   \r\n" +
                                                            "Select 'Z' bb,  \r\n" +
                                                            "case when targetdate is null then 'Not Accepted'  \r\n" +
                                                            "when targetdate is not null then 'Accepted'   \r\n" +
                                                            " when CompletionDate is not null then 'Completed'  \r\n" +
                                                            " when VerificationDate is not null then 'Verified' else '' end as ActionStatus   \r\n" +
                                                            " ,[description] as Action, thedate datesubmitted, datediff(day, thedate, getdate()) ss   \r\n" +
                                                            " from[dbo].[tbl_Shec_Incidents]  \r\n" +
                                                            " where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "'   \r\n" +


                                                            " union all \r\n" +
                                                            " select 'a' , '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'b ', '', '', null, '' \r\n" +
                                                            " union all \r\n" +
                                                            " select 'c  ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'd   ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'e    ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'f     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'g     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'h     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'i     ' , '', '', null, '' \r\n" +
                                                            " union \r\n" +
                                                            " select 'j     ' , '', '', null, '' \r\n" +
                                                            " )a \r\n" +
                                                            "  order  by bb  desc,datesubmitted \r\n";
                                _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManWPST2.ResultsTableName = "Table2";
                                _dbManWPST2.ExecuteInstruction();

                                DataSet dsABS1 = new DataSet();
                                dsABS1.Tables.Add(_dbManWPST2.ResultsDataTable);

                                theReportGeoRoutineVisit.RegisterData(dsABS1);

                                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, '" + clsWorkplaceSummary.SelectedRiskRating + "' rr,  * from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "' and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) and captyear =  (select DATEPART(YYYY, '" + clsWorkplaceSummary.SelectedCaptDate + "')) ";
                                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbMan.ResultsTableName = "DevSummary";
                                _dbMan.ExecuteInstruction();

                                MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                                _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManImage.SqlStatement = "Select picture, Picture1 from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + clsWorkplaceSummary.SelectedWorkplace + "' and captweek = (select DATEPART(wk, '" + clsWorkplaceSummary.SelectedCaptDate + "')) ";
                                _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManImage.ResultsTableName = "Image";
                                _dbManImage.ExecuteInstruction();

                                MWDataManager.clsDataAccess _dbManChart = new MWDataManager.clsDataAccess();
                                _dbManChart.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                                _dbManChart.SqlStatement = " select top(10) * from (select description, a.CalendarDate aa ,substring(convert(varchar(11),a.CalendarDate),0,12) Calendardate,a.SWidth,a.CorrCut,a.Hangwall,a.Footwall, " +
                                                           " case when a.allocatedWidth = 0 then null else a.allocatedWidth end as allocatedWidth ,a.Notes from [dbo].[tbl_SAMPLING_Imported_Notes] a  \r\n" +
                                                  " left outer  join tbl_Workplace_Total w on convert(varchar(50),a.gmsiwpis) = w.gmsiwpid  \r\n" +
                                                  " and calendardate > getdate()-2000 ) a where description = '" + clsWorkplaceSummary.SelectedWorkplace + "'    \r\n" +
                                                  "  order by aa desc  ";
                                _dbManChart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                _dbManChart.queryReturnType = MWDataManager.ReturnType.DataTable;
                                _dbManChart.ResultsTableName = "Chart";
                                _dbManChart.ExecuteInstruction();

                                DataSet ReportDatasetReport = new DataSet();
                                ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                                theReportGeoRoutineVisit.RegisterData(ReportDatasetReport);

                                DataSet ReportDatasetReportImage = new DataSet();
                                ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                                theReportGeoRoutineVisit.RegisterData(ReportDatasetReportImage);

                                DataSet ReportDatasetChart = new DataSet();
                                ReportDatasetChart.Tables.Add(_dbManChart.ResultsDataTable);

                                theReportGeoRoutineVisit.RegisterData(ReportDatasetChart);

                                theReportGeoRoutineVisit.Load(ReportsFolder + "\\GeoInspDev.frx");

                                //theReport3.Design();

                                pcReport.Clear();
                                theReportGeoRoutineVisit.Prepare();
                                theReportGeoRoutineVisit.Preview = pcReport;
                                theReportGeoRoutineVisit.ShowPrepared();                                

                            }

                        }

                        //Pre-Planning
                        else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Geology")
                        {
                            LoadWP();
                            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
                            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyActions.SqlStatement = " SELECT 1 num,TheDate,CompletionDate,Workplace, Action, HOD, RespPerson, isnull(compnotes, '') Compnotes1,  \r\n" +
                                                              " case when CompNotes <> '' then '" + ImageFolder + "\\Preplanning\\Geology\\' + CompNotes + '.png' \r\n" +
                                                              " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
                                                              " where Type = 'PPGL' and Completiondate is null and workplace in ('" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "','" + WP4Desc + "','" + WP5Desc + "') union select 2 num, null,null,'','','','','','' order by num ";
                            _LoadSafetyActions.ResultsTableName = "Actions";
                            _LoadSafetyActions.ExecuteInstruction();

                            DataSet dsActions = new DataSet();
                            dsActions.Tables.Clear();
                            dsActions.Tables.Add(_LoadSafetyActions.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dsActions);

                            //Cycles
                            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
                            _dbManCycle.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + clsWorkplaceSummary.SelectedCrew + "' and Prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' ";

                            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManCycle.ResultsTableName = "Cycles";
                            _dbManCycle.ExecuteInstruction();

                            DataSet dsCycle = new DataSet();
                            dsCycle.Tables.Clear();
                            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dsCycle);

                            //open actions
                            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
                            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManOpenActions.SqlStatement = "select 1 num, convert(varchar(50) , actiondate, (111)) datesubmitted,workplace, action_title [action],hazard from  [dbo].[tbl_Incidents] \r\n" +
                                                             "where workplace in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') and Action_Status <> 'Closed'\r\n" +
                                                             " union select 2 num, null,'','','' order by num ";

                            _dbManOpenActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManOpenActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManOpenActions.ResultsTableName = "OpenManActions";
                            _dbManOpenActions.ExecuteInstruction();

                            DataSet dsOpenActions = new DataSet();
                            dsOpenActions.Tables.Clear();
                            dsOpenActions.Tables.Add(_dbManOpenActions.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dsOpenActions);

                            //Auth Watermark
                            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
                            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManParaAuth.SqlStatement = "select top (1) case when GeologyDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                                     "where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' ";

                            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManParaAuth.ResultsTableName = "ParaAuth";
                            _dbManParaAuth.ExecuteInstruction();

                            _Auth = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

                            DataSet dsParaAuth = new DataSet();
                            dsParaAuth.Tables.Clear();
                            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dsParaAuth);

                            //LoadNotes
                            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
                            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_GeologyCapture_Notes] where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' and notes is not null  ";
                            _LoadSafetyNotes.ResultsTableName = "Notes";
                            _LoadSafetyNotes.ExecuteInstruction();

                            DataSet dsSafteyNotes = new DataSet();
                            dsSafteyNotes.Tables.Clear();
                            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dsSafteyNotes);

                            MWDataManager.clsDataAccess _dbMansw1 = new MWDataManager.clsDataAccess();
                            _dbMansw1.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMansw1.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP1 + "' order by Prodmonth asc";

                            _dbMansw1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMansw1.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMansw1.ResultsTableName = "sw1";
                            _dbMansw1.ExecuteInstruction();

                            DataSet dssw1 = new DataSet();
                            dssw1.Tables.Clear();
                            dssw1.Tables.Add(_dbMansw1.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dssw1);

                            MWDataManager.clsDataAccess _dbMancmgt1 = new MWDataManager.clsDataAccess();
                            _dbMancmgt1.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMancmgt1.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP1 + "' order by Prodmonth asc";

                            _dbMancmgt1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMancmgt1.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMancmgt1.ResultsTableName = "cmgt1";
                            _dbMancmgt1.ExecuteInstruction();

                            DataSet dscmgt1 = new DataSet();
                            dscmgt1.Tables.Clear();
                            dscmgt1.Tables.Add(_dbMancmgt1.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dscmgt1);

                            MWDataManager.clsDataAccess _dbMansw2 = new MWDataManager.clsDataAccess();
                            _dbMansw2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMansw2.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP2 + "' order by Prodmonth asc";

                            _dbMansw2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMansw2.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMansw2.ResultsTableName = "sw2";
                            _dbMansw2.ExecuteInstruction();

                            DataSet dssw2 = new DataSet();
                            dssw2.Tables.Clear();
                            dssw2.Tables.Add(_dbMansw2.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dssw2);

                            MWDataManager.clsDataAccess _dbMancmgt2 = new MWDataManager.clsDataAccess();
                            _dbMancmgt2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMancmgt2.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP2 + "' order by Prodmonth asc";

                            _dbMancmgt2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMancmgt2.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMancmgt2.ResultsTableName = "cmgt2";
                            _dbMancmgt2.ExecuteInstruction();

                            DataSet dscmgt2 = new DataSet();
                            dscmgt2.Tables.Clear();
                            dscmgt2.Tables.Add(_dbMancmgt2.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dscmgt2);

                            MWDataManager.clsDataAccess _dbMansw3 = new MWDataManager.clsDataAccess();
                            _dbMansw3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMansw3.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP3 + "' order by Prodmonth asc";

                            _dbMansw3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMansw3.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMansw3.ResultsTableName = "sw3";
                            _dbMansw3.ExecuteInstruction();

                            DataSet dssw3 = new DataSet();
                            dssw3.Tables.Clear();
                            dssw3.Tables.Add(_dbMansw3.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dssw3);

                            MWDataManager.clsDataAccess _dbMancmgt3 = new MWDataManager.clsDataAccess();
                            _dbMancmgt3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMancmgt3.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP3 + "' order by Prodmonth asc";

                            _dbMancmgt3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMancmgt3.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMancmgt3.ResultsTableName = "cmgt3";
                            _dbMancmgt3.ExecuteInstruction();

                            DataSet dscmgt3 = new DataSet();
                            dscmgt3.Tables.Clear();
                            dscmgt3.Tables.Add(_dbMancmgt3.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dscmgt3);

                            MWDataManager.clsDataAccess _dbMansw4 = new MWDataManager.clsDataAccess();
                            _dbMansw4.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMansw4.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP4 + "' order by Prodmonth asc";

                            _dbMansw4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMansw4.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMansw4.ResultsTableName = "sw4";
                            _dbMansw4.ExecuteInstruction();

                            DataSet dssw4 = new DataSet();
                            dssw4.Tables.Clear();
                            dssw4.Tables.Add(_dbMansw4.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dssw4);

                            MWDataManager.clsDataAccess _dbMancmgt4 = new MWDataManager.clsDataAccess();
                            _dbMancmgt4.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMancmgt4.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP4 + "' order by Prodmonth asc";

                            _dbMancmgt4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMancmgt4.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMancmgt4.ResultsTableName = "cmgt4";
                            _dbMancmgt4.ExecuteInstruction();

                            DataSet dscmgt4 = new DataSet();
                            dscmgt4.Tables.Clear();
                            dscmgt4.Tables.Add(_dbMancmgt4.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dscmgt4);

                            MWDataManager.clsDataAccess _dbMansw5 = new MWDataManager.clsDataAccess();
                            _dbMansw5.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMansw5.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP5 + "' order by Prodmonth asc";

                            _dbMansw5.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMansw5.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMansw5.ResultsTableName = "sw5";
                            _dbMansw5.ExecuteInstruction();

                            DataSet dssw5 = new DataSet();
                            dssw5.Tables.Clear();
                            dssw5.Tables.Add(_dbMansw5.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dssw5);

                            MWDataManager.clsDataAccess _dbMancmgt5 = new MWDataManager.clsDataAccess();
                            _dbMancmgt5.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMancmgt5.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP5 + "' order by Prodmonth asc";

                            _dbMancmgt5.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMancmgt5.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMancmgt5.ResultsTableName = "cmgt5";
                            _dbMancmgt5.ExecuteInstruction();

                            DataSet dscmgt5 = new DataSet();
                            dscmgt5.Tables.Clear();
                            dscmgt5.Tables.Add(_dbMancmgt5.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dscmgt5);

                            //CMGT
                            MWDataManager.clsDataAccess _dbManCMGT = new MWDataManager.clsDataAccess();
                            _dbManCMGT.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManCMGT.SqlStatement = "exec sp_PrePlanning_GeoGraphs '" + clsWorkplaceSummary.SelectedProdmonth + "','" + clsWorkplaceSummary.SelectedMOSection + "','" + WP1 + "','" + WP2 + "','" + WP3 + "','" + WP4 + "','" + WP5 + "' ";

                            _dbManCMGT.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManCMGT.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManCMGT.ResultsTableName = "Graphs";
                            _dbManCMGT.ExecuteInstruction();

                            DataSet dsCMGTNEW = new DataSet();
                            dsCMGTNEW.Tables.Clear();
                            dsCMGTNEW.Tables.Add(_dbManCMGT.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dsCMGTNEW);

                            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
                            _dbAns.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbAns.SqlStatement = " select* from(  \r\n" +
                                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                                    " from tbl_PrePlanning_GeologyQuest q, tbl_PrePlanning_Geology c, tbl_Workplace w \r\n" +
                                                    " where q.QuestID = c.QuestID \r\n" +
                                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                                    " and c.prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' \r\n" +
                                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                                    " )a order by Workplace, OrderBy \r\n";

                            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbAns.ResultsTableName = "Answers";
                            _dbAns.ExecuteInstruction();

                            DataSet dsAnss = new DataSet();
                            dsAnss.Tables.Clear();
                            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

                            theReportGeoPrePlan.RegisterData(dsAnss);
                            
                            theReportGeoPrePlan.Load(ReportsFolder + "PrePlanGeoInsp.frx");

                            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth));
                            string Date = ProductionGlobal.ProductionGlobal.Prod2;


                            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
                            {
                                theReportGeoPrePlan.SetParameterValue("OpenCheck", "Y");
                            }
                            else
                            {
                                theReportGeoPrePlan.SetParameterValue("OpenCheck", "N");
                            }

                            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
                            {
                                if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 1)
                                {
                                    theReportGeoPrePlan.SetParameterValue("ActionsCheck", "Y");
                                }
                                else
                                {
                                    theReportGeoPrePlan.SetParameterValue("ActionsCheck", "N");
                                }
                            }
                            else
                            {
                                theReportGeoPrePlan.SetParameterValue("ActionsCheck", "N");
                            }


                            theReportGeoPrePlan.SetParameterValue("Prodmonth", Date);
                            theReportGeoPrePlan.SetParameterValue("Crew", clsWorkplaceSummary.SelectedCrew);

                            loadImage("Geology");
                            theReportGeoPrePlan.SetParameterValue("Image", _Attachment);

                            theReportGeoPrePlan.SetParameterValue("Auth", _Auth);

                            theReportGeoPrePlan.SetParameterValue("WP1Desc", WP1Desc);
                            theReportGeoPrePlan.SetParameterValue("WP2Desc", WP2Desc);
                            theReportGeoPrePlan.SetParameterValue("WP3Desc", WP3Desc);
                            theReportGeoPrePlan.SetParameterValue("WP4Desc", WP4Desc);
                            theReportGeoPrePlan.SetParameterValue("WP5Desc", WP5Desc);
                            theReportGeoPrePlan.SetParameterValue("Activity", clsWorkplaceSummary.SelectedActivity);
                            //theReportGeoPrePlan.Design();

                            pcReport.Clear();
                            theReportGeoPrePlan.Prepare();
                            theReportGeoPrePlan.Preview = pcReport;
                            theReportGeoPrePlan.ShowPrepared();
                           

                        }

                        else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Rock Engineering")
                        {
                            LoadWP();
                            MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
                            _dbManData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManData.SqlStatement = " select RockEngDepCapt,RockEngDepAuth,isnull(CalendarDate,' ') CalendarDate from tbl_PrePlanning_MonthPlan \r\n" +
                                                      " where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' \r\n" +
                                                      " and SectionID = '" + clsWorkplaceSummary.SelectedMOSection + "' \r\n" +
                                                      " and Crew = '" + clsWorkplaceSummary.SelectedCrew + "'";
                            _dbManData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManData.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManData.ResultsTableName = "Captured";
                            _dbManData.ExecuteInstruction();

                            DataSet dsCaptured = new DataSet();
                            dsCaptured.Tables.Clear();
                            dsCaptured.Tables.Add(_dbManData.ResultsDataTable);

                            //open actions
                            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
                            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManOpenActions.SqlStatement = "select 1 num, convert(varchar(50) , actiondate, (111)) datesubmitted,workplace, action_title [action],hazard from  [dbo].[tbl_Incidents] \r\n" +
                                                             "where workplace in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') and Action_Status <> 'Closed'\r\n" +
                                                             " union select 2 num, null,'','','' order by num ";

                            _dbManOpenActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManOpenActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManOpenActions.ResultsTableName = "OpenManActions";
                            _dbManOpenActions.ExecuteInstruction();

                            DataSet dsOpenActions = new DataSet();
                            dsOpenActions.Tables.Clear();
                            dsOpenActions.Tables.Add(_dbManOpenActions.ResultsDataTable);

                            MWDataManager.clsDataAccess _dbManSeismic = new MWDataManager.clsDataAccess();
                            _dbManSeismic.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManSeismic.SqlStatement = " select isnull(risk1a, -100) risk1, \r\n" +
                                                        "isnull(risk2a, -100) risk2, \r\n" +
                                                        "isnull(risk3a, -100) risk3, \r\n" +
                                                        "isnull(risk4a, -100) risk4, \r\n" +
                                                        "isnull(risk5a, -100) risk5, \r\n" +
                                                        " * from (select Convert(Varchar(15),TheDate,106) TheDate2, max(TheDate) dd from  \r\n" +
                                                        "tbl_LicenceToOperate_Seismic where thedate >= getdate()-30 group by TheDate) a \r\n" +
                                                        " left outer join \r\n" +
                                                        "( \r\n" +
                                                        "select Convert(Varchar(15),TheDate,106)TheDatewp1, \r\n" +
                                                        "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk1a, \r\n" +
                                                        "wpdescription Workplace1 from   \r\n" +
                                                        "tbl_LicenceToOperate_Seismic where wpdescription in ('" + WP1Desc + "')) b on a.thedate2 = b.TheDatewp1 \r\n" +
                                                        "left outer join \r\n" +
                                                        "( \r\n" +
                                                        "select Convert(Varchar(15),TheDate,106)TheDatewp2, \r\n" +
                                                        "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk2a, \r\n" +
                                                        "wpdescription Workplace2 from   \r\n" +
                                                        "tbl_LicenceToOperate_Seismic where wpdescription in ('" + WP2Desc + "')) c on a.thedate2 = c.TheDatewp2 \r\n" +
                                                        "left outer join \r\n" +
                                                        "( \r\n" +
                                                        "select Convert(Varchar(15),TheDate,106)TheDatewp3, \r\n" +
                                                        "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk3a, \r\n" +
                                                        "wpdescription Workplace3 from   \r\n" +
                                                        "tbl_LicenceToOperate_Seismic where wpdescription in ('" + WP3Desc + "')) d on a.thedate2 = d.TheDatewp3 \r\n" +
                                                        "left outer join \r\n" +
                                                        "( \r\n" +
                                                        "select Convert(Varchar(15),TheDate,106)TheDatewp4, \r\n" +
                                                        "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk4a, \r\n" +
                                                        "wpdescription Workplace4 from   \r\n" +
                                                        "tbl_LicenceToOperate_Seismic where wpdescription in ('" + WP4Desc + "')) e on a.thedate2 = e.TheDatewp4 \r\n" +
                                                        "left outer join \r\n" +
                                                        "( \r\n" +
                                                        "select Convert(Varchar(15),TheDate,106)TheDatewp5, \r\n" +
                                                        "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk5a, \r\n" +
                                                        "wpdescription Workplace5 from   \r\n" +
                                                        "tbl_LicenceToOperate_Seismic where wpdescription in ('" + WP5Desc + "')) f on a.thedate2 = f.TheDatewp5  order by TheDate2";

                            _dbManSeismic.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManSeismic.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManSeismic.ResultsTableName = "Seismic";
                            _dbManSeismic.ExecuteInstruction();

                            DataSet dsSeismic = new DataSet();
                            dsSeismic.Tables.Add(_dbManSeismic.ResultsDataTable);


                            MWDataManager.clsDataAccess _LoadREActions = new MWDataManager.clsDataAccess();
                            _LoadREActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadREActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadREActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadREActions.SqlStatement = " SELECT 1 num,TheDate,CompletionDate,Workplace, Action, HOD, RespPerson, isnull(compnotes, '') Compnotes1,  \r\n" +
                                                              " case when CompNotes <> '' then '" + ImageFolder + "\\Preplanning\\Geology\\' + CompNotes + '.png' \r\n" +
                                                              " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
                                                              " where Type = 'PPRE' and Completiondate is null and workplace in ('" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "','" + WP4Desc + "','" + WP5Desc + "') union select 2 num, null,null,'','','','','','' order by num ";
                            _LoadREActions.ResultsTableName = "Actions";
                            _LoadREActions.ExecuteInstruction();

                            DataSet dsActions = new DataSet();
                            dsActions.Tables.Clear();
                            dsActions.Tables.Add(_LoadREActions.ResultsDataTable);


                            //Cycles
                            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
                            _dbManCycle.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + clsWorkplaceSummary.SelectedCrew + "' and pm = '" + clsWorkplaceSummary.SelectedProdmonth + "' ";

                            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManCycle.ResultsTableName = "Cycles";
                            _dbManCycle.ExecuteInstruction();

                            DataSet dsCycle = new DataSet();
                            dsCycle.Tables.Clear();
                            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

                            //RE Hazard Rating (6 month)
                            MWDataManager.clsDataAccess _dbManREHR = new MWDataManager.clsDataAccess();
                            _dbManREHR.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                            if (clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                _dbManREHR.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + WP1Desc + "' wp1new,'" + WP2Desc + "' wp2new,'" + WP3Desc + "' wp3new,'" + WP4Desc + "' wp4new,'" + WP5Desc + "' wp5new, \r\n" +
                                                                " case when wp1answer is null then '-100' when wp1answer = '0' then '-100' when wp1answer = '0' then '-100' when wp1answer = '' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                                " case when wp2answer is null then '-100' when wp2answer = '0' then '-100' when wp2answer = '0' then '-100' when wp2answer = '' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                                " case when wp3answer is null then '-100' when wp3answer = '0' then '-100' when wp3answer = '0' then '-100' when wp3answer = '' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                                " case when wp4answer is null then '-100' when wp4answer = '0' then '-100' when wp4answer = '0' then '-100' when wp4answer = '' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                                " case when wp5answer is null then '-100' when wp5answer = '0' then '-100' when wp5answer = '0' then '-100' when wp5answer = '' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                                " from (     \r\n" +

                                                                " select top(6) prodmonth from tbl_Planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm1zz,answer wp1answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP1Desc + "' and questid = 15     \r\n" +
                                                                " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm2zz,answer wp2answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP2Desc + "' and questid = 15     \r\n" +
                                                                " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm3zz,answer wp3answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP3Desc + "' and questid = 15     \r\n" +
                                                                " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm4zz,answer wp4answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP4Desc + "' and questid = 15      \r\n" +
                                                                " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm5zz, answer wp5answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP5Desc + "' and questid = 15     \r\n" +
                                                                " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                                " order by prodmonth";
                            }
                            else
                            {
                                _dbManREHR.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + WP1Desc + "' wp1new,'" + WP2Desc + "' wp2new,'" + WP3Desc + "' wp3new,'" + WP4Desc + "' wp4new,'" + WP5Desc + "' wp5new, \r\n" +
                                                                    " case when wp1answer is null then '-100' when wp1answer = '0' then '-100' when wp1answer = '0' then '-100' when wp1answer = '' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                                    " case when wp2answer is null then '-100' when wp2answer = '0' then '-100' when wp2answer = '0' then '-100' when wp2answer = '' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                                    " case when wp3answer is null then '-100' when wp3answer = '0' then '-100' when wp3answer = '0' then '-100' when wp3answer = '' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                                    " case when wp4answer is null then '-100' when wp4answer = '0' then '-100' when wp4answer = '0' then '-100' when wp4answer = '' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                                    " case when wp5answer is null then '-100' when wp5answer = '0' then '-100' when wp5answer = '0' then '-100' when wp5answer = '' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                                    " from (     \r\n" +

                                                                    " select top(6) prodmonth from tbl_Planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                                    " left outer join(     \r\n" +
                                                                    " select prodmonth pm1zz,answer wp1answer     \r\n" +
                                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                    " where a.Workplace = b.workplaceid and description = '" + WP1Desc + "' and questid = 34     \r\n" +
                                                                    " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                                    " left outer join(     \r\n" +
                                                                    " select prodmonth pm2zz,answer wp2answer     \r\n" +
                                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                    " where a.Workplace = b.workplaceid and description = '" + WP2Desc + "' and questid = 34     \r\n" +
                                                                    " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                                    " left outer join(     \r\n" +
                                                                    " select prodmonth pm3zz,answer wp3answer     \r\n" +
                                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                    " where a.Workplace = b.workplaceid and description = '" + WP3Desc + "' and questid = 34     \r\n" +
                                                                    " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                                    " left outer join(     \r\n" +
                                                                    " select prodmonth pm4zz,answer wp4answer     \r\n" +
                                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                    " where a.Workplace = b.workplaceid and description = '" + WP4Desc + "' and questid = 34      \r\n" +
                                                                    " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                                    " left outer join(     \r\n" +
                                                                    " select prodmonth pm5zz, answer wp5answer     \r\n" +
                                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                    " where a.Workplace = b.workplaceid and description = '" + WP5Desc + "' and questid = 34     \r\n" +
                                                                    " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                                    " order by prodmonth";
                            }

                            _dbManREHR.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManREHR.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManREHR.ResultsTableName = "RE Hazard Rating";
                            _dbManREHR.ExecuteInstruction();

                            DataSet dsREHR = new DataSet();
                            dsREHR.Tables.Clear();
                            dsREHR.Tables.Add(_dbManREHR.ResultsDataTable);

                            //Auth Watermark
                            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
                            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManParaAuth.SqlStatement = "select top (1) case when RockEngDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                                     "where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' ";

                            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManParaAuth.ResultsTableName = "ParaAuth";
                            _dbManParaAuth.ExecuteInstruction();

                            if (_dbManParaAuth.ResultsDataTable.Rows.Count > 0)
                            {
                                _Auth = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();
                            }
                            DataSet dsParaAuth = new DataSet();
                            dsParaAuth.Tables.Clear();
                            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

                            //LoadNotes
                            MWDataManager.clsDataAccess _LoadRENotes = new MWDataManager.clsDataAccess();
                            _LoadRENotes.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadRENotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadRENotes.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadRENotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_RockEngCapt_Notes] where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' and notes is not null  ";
                            _LoadRENotes.ResultsTableName = "Notes";
                            _LoadRENotes.ExecuteInstruction();

                            DataSet dsRENotes = new DataSet();
                            dsRENotes.Tables.Clear();
                            dsRENotes.Tables.Add(_LoadRENotes.ResultsDataTable);

                            //Underground Re Compliance 
                            MWDataManager.clsDataAccess _dbManSR = new MWDataManager.clsDataAccess();
                            _dbManSR.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                            if (clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                _dbManSR.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + WP1Desc + "' wp1new,'" + WP2Desc + "' wp2new,'" + WP3Desc + "' wp3new,'" + WP4Desc + "' wp4new,'" + WP5Desc + "' wp5new, \r\n" +
                                                                " case when wp1answer is null then '-100' when wp1answer = '0' then '-100' when wp1answer = '0' then '-100' when wp1answer = '' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                                " case when wp2answer is null then '-100' when wp2answer = '0' then '-100' when wp2answer = '0' then '-100' when wp2answer = '' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                                " case when wp3answer is null then '-100' when wp3answer = '0' then '-100' when wp3answer = '0' then '-100' when wp3answer = '' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                                " case when wp4answer is null then '-100' when wp4answer = '0' then '-100' when wp4answer = '0' then '-100' when wp4answer = '' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                                " case when wp5answer is null then '-100' when wp5answer = '0' then '-100' when wp5answer = '0' then '-100' when wp5answer = '' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                                " from (     \r\n" +

                                                                " select top(6) prodmonth from tbl_Planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm1zz,answer wp1answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP1Desc + "' and questid = 36     \r\n" +
                                                                " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm2zz,answer wp2answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP2Desc + "' and questid = 36     \r\n" +
                                                                " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm3zz,answer wp3answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP3Desc + "' and questid = 36     \r\n" +
                                                                " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm4zz,answer wp4answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP4Desc + "' and questid = 36      \r\n" +
                                                                " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm5zz, answer wp5answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP5Desc + "' and questid = 36     \r\n" +
                                                                " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                                " order by prodmonth";
                            }
                            else
                            {
                                _dbManSR.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + WP1Desc + "' wp1new,'" + WP2Desc + "' wp2new,'" + WP3Desc + "' wp3new,'" + WP4Desc + "' wp4new,'" + WP5Desc + "' wp5new, \r\n" +
                                                                " case when wp1answer is null then '-100' when wp1answer = '0' then '-100' when wp1answer = '0' then '-100' when wp1answer = '' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                                " case when wp2answer is null then '-100' when wp2answer = '0' then '-100' when wp2answer = '0' then '-100' when wp2answer = '' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                                " case when wp3answer is null then '-100' when wp3answer = '0' then '-100' when wp3answer = '0' then '-100' when wp3answer = '' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                                " case when wp4answer is null then '-100' when wp4answer = '0' then '-100' when wp4answer = '0' then '-100' when wp4answer = '' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                                " case when wp5answer is null then '-100' when wp5answer = '0' then '-100' when wp5answer = '0' then '-100' when wp5answer = '' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                                " from (     \r\n" +

                                                                " select top(6) prodmonth from tbl_planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm1zz,answer wp1answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP1Desc + "' and questid = 36     \r\n" +
                                                                " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm2zz,answer wp2answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP2Desc + "' and questid = 36     \r\n" +
                                                                " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm3zz,answer wp3answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP3Desc + "' and questid = 36     \r\n" +
                                                                " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm4zz,answer wp4answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP4Desc + "' and questid = 36      \r\n" +
                                                                " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm5zz, answer wp5answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_Workplace b      \r\n" +
                                                                " where a.Workplace = b.workplaceid and description = '" + WP5Desc + "' and questid = 36     \r\n" +
                                                                " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                                " order by prodmonth";
                            }


                            _dbManSR.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManSR.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManSR.ResultsTableName = "Underground RE compliance";
                            _dbManSR.ExecuteInstruction();

                            DataSet dsSR = new DataSet();
                            dsSR.Tables.Clear();
                            dsSR.Tables.Add(_dbManSR.ResultsDataTable);

                            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
                            _dbAns.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            if (clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                _dbAns.SqlStatement = " select* from(  \r\n" +
                                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, q.QuestionSubCat, \r\n" +
                                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                                    " from tbl_PrePlanning_RockEngQuest_Dev q, tbl_PrePlanning_RockEng c, tbl_Workplace w \r\n" +
                                                    " where q.QuestID = c.QuestID \r\n" +
                                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                                    " and c.prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' \r\n" +
                                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                                    " )a order by Workplace, OrderBy \r\n";
                            }
                            else
                            {
                                _dbAns.SqlStatement = " select* from(  \r\n" +
                                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, q.QuestionSubCat, \r\n" +
                                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                                    " from tbl_PrePlanning_RockEngQuest q, tbl_PrePlanning_RockEng c, tbl_Workplace w \r\n" +
                                                    " where q.QuestID = c.QuestID \r\n" +
                                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                                    " and c.prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' \r\n" +
                                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                                    " )a order by Workplace, OrderBy \r\n";
                            }
                            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbAns.ResultsTableName = "Answers";
                            _dbAns.ExecuteInstruction();

                            DataSet dsAnss = new DataSet();
                            dsAnss.Tables.Clear();
                            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

                            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth));
                            string Date = ProductionGlobal.ProductionGlobal.Prod2;

                            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
                            {
                                theReportREPrePlan.SetParameterValue("OpenCheck", "Y");
                            }
                            else
                            {
                                theReportREPrePlan.SetParameterValue("OpenCheck", "N");
                            }

                            if (_LoadREActions.ResultsDataTable.Rows.Count > 0)
                            {
                                if (_LoadREActions.ResultsDataTable.Rows.Count > 1)
                                {
                                    theReportREPrePlan.SetParameterValue("ActionsCheck", "Y");
                                }
                                else
                                {
                                    theReportREPrePlan.SetParameterValue("ActionsCheck", "N");
                                }
                            }
                            else
                            {
                                theReportREPrePlan.SetParameterValue("ActionsCheck", "N");
                            }

                            theReportREPrePlan.RegisterData(dsOpenActions);
                            theReportREPrePlan.RegisterData(dsCycle);
                            theReportREPrePlan.RegisterData(dsParaAuth);
                            theReportREPrePlan.RegisterData(dsActions);
                            theReportREPrePlan.RegisterData(dsSeismic);
                            theReportREPrePlan.RegisterData(dsREHR);
                            theReportREPrePlan.RegisterData(dsRENotes);
                            theReportREPrePlan.RegisterData(dsSR);
                            theReportREPrePlan.RegisterData(dsAnss);
                            theReportREPrePlan.RegisterData(dsCaptured);

                            if (clsWorkplaceSummary.SelectedActivity == "Stp")
                            {
                               
                                theReportREPrePlan.Load(ReportsFolder + "PrePlanExeNoteRockEngStoping.frx");
                            }
                            else
                            {
                                
                                theReportREPrePlan.Load(ReportsFolder + "PrePlanExeNoteRockEngStoping.frx");
                            }
                            theReportREPrePlan.SetParameterValue("Prodmonth", Date);
                            theReportREPrePlan.SetParameterValue("Crew", clsWorkplaceSummary.SelectedCrew);

                            loadImage("RockEngineering");

                            theReportREPrePlan.SetParameterValue("Auth", _Auth);

                            theReportREPrePlan.SetParameterValue("Banner", ProductionGlobal.ProductionGlobal.Banner);
                            theReportREPrePlan.SetParameterValue("Activity", clsWorkplaceSummary.SelectedActivity);
                            theReportREPrePlan.SetParameterValue("Image", _Attachment);
                            theReportREPrePlan.SetParameterValue("WP1Desc", WP1Desc);
                            theReportREPrePlan.SetParameterValue("WP2Desc", WP2Desc);
                            theReportREPrePlan.SetParameterValue("WP3Desc", WP3Desc);
                            theReportREPrePlan.SetParameterValue("WP4Desc", WP4Desc);
                            theReportREPrePlan.SetParameterValue("WP5Desc", WP5Desc);

                            //theReportREPrePlan.Design();

                            pcReport.Clear();
                            theReportREPrePlan.Prepare();
                            theReportREPrePlan.Preview = pcReport;
                            theReportREPrePlan.ShowPrepared();

                           

                        }

                        else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Safety")
                        {
                            LoadWP();
                            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
                            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyActions.SqlStatement = " SELECT *, HOD, RespPerson, isnull(compnotes, '') Compnotes1, \r\n" +
                                                              " case when CompNotes <> '' then '" + ImageFolder + "\\Preplanning\\Safety\\' + CompNotes + '.png' \r\n" +
                                                              " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
                                                              " where Type = 'PPS' and Completiondate is null and workplace in ('" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "','" + WP4Desc + "','" + WP5Desc + "')";
                            _LoadSafetyActions.ResultsTableName = "Actions";
                            _LoadSafetyActions.ExecuteInstruction();

                            DataSet dsActions = new DataSet();
                            dsActions.Tables.Clear();
                            dsActions.Tables.Add(_LoadSafetyActions.ResultsDataTable);

                            theReportSafetyPrePlan.RegisterData(dsActions);

                            //Cycles
                            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
                            _dbManCycle.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + clsWorkplaceSummary.SelectedCrew + "' and pm = '" + clsWorkplaceSummary.SelectedProdmonth + "' ";

                            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManCycle.ResultsTableName = "Cycles";
                            _dbManCycle.ExecuteInstruction();

                            DataSet dsCycle = new DataSet();
                            dsCycle.Tables.Clear();
                            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

                            theReportSafetyPrePlan.RegisterData(dsCycle);

                            //open actions
                            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
                            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManOpenActions.SqlStatement = "select 1 num, convert(varchar(50) , actiondate, (111)) datesubmitted,workplace, action_title [action],hazard from  [dbo].[tbl_Incidents] \r\n" +
                                                             "where workplace in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') and Action_Status <> 'Closed'\r\n" +
                                                             " union select 2 num, null,'','','' order by num ";

                            _dbManOpenActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManOpenActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManOpenActions.ResultsTableName = "OpenManActions";
                            _dbManOpenActions.ExecuteInstruction();

                            DataSet dsOpenActions = new DataSet();
                            dsOpenActions.Tables.Clear();
                            dsOpenActions.Tables.Add(_dbManOpenActions.ResultsDataTable);

                            theReportSafetyPrePlan.RegisterData(dsOpenActions);

                            //Auth Watermark
                            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
                            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManParaAuth.SqlStatement = "select top (1) case when SafetyDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                                     "where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' ";

                            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManParaAuth.ResultsTableName = "ParaAuth";
                            _dbManParaAuth.ExecuteInstruction();

                            _Auth = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

                            DataSet dsParaAuth = new DataSet();
                            dsParaAuth.Tables.Clear();
                            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

                            theReportSafetyPrePlan.RegisterData(dsParaAuth);

                            //LoadNotes
                            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
                            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_SafetyCapt_Notes] where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' and notes is not null  ";
                            _LoadSafetyNotes.ResultsTableName = "Notes";
                            _LoadSafetyNotes.ExecuteInstruction();

                            DataSet dsSafteyNotes = new DataSet();
                            dsSafteyNotes.Tables.Clear();
                            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

                            theReportSafetyPrePlan.RegisterData(dsSafteyNotes);

                            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
                            _dbAns.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbAns.SqlStatement = " select* from(  \r\n" +
                                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy  \r\n" +
                                                    " from tbl_PrePlanning_SafetyQuest q, tbl_PrePlanning_safety c, tbl_Workplace w \r\n" +
                                                    " where q.QuestID = c.QuestID \r\n" +
                                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                                    " and c.prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' \r\n" +
                                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                                    " )a order by Workplace, Orderby \r\n";

                            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbAns.ResultsTableName = "Answers";
                            _dbAns.ExecuteInstruction();

                            DataSet dsAnss = new DataSet();
                            dsAnss.Tables.Clear();
                            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

                            theReportSafetyPrePlan.RegisterData(dsAnss);

                            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth));
                            string Date = ProductionGlobal.ProductionGlobal.Prod2;

                            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
                            {
                                theReportSafetyPrePlan.SetParameterValue("OpenCheck", "Y");
                            }
                            else
                            {
                                theReportSafetyPrePlan.SetParameterValue("OpenCheck", "N");
                            }

                            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
                            {
                                if (_LoadSafetyActions.ResultsDataTable.Rows[0]["TheDate"].ToString() == "1/1/1900 12:00:00 AM")
                                {
                                    theReportSafetyPrePlan.SetParameterValue("ActionsCheck", "N");
                                }
                                else
                                {
                                    theReportSafetyPrePlan.SetParameterValue("ActionsCheck", "Y");
                                }
                            }
                            else
                            {
                                theReportSafetyPrePlan.SetParameterValue("ActionsCheck", "N");
                            }

                           
                            theReportSafetyPrePlan.Load(ReportsFolder + "GraphicsPrePlanningSafety.frx");

                            theReportSafetyPrePlan.SetParameterValue("Prodmonth", Date);
                            theReportSafetyPrePlan.SetParameterValue("Crew", clsWorkplaceSummary.SelectedCrew);

                            loadImage("Safety");
                            theReportSafetyPrePlan.SetParameterValue("Image", _Attachment);

                            theReportSafetyPrePlan.SetParameterValue("Auth", _Auth);
                            theReportSafetyPrePlan.SetParameterValue("Activity", clsWorkplaceSummary.SelectedActivity);

                            // theReportSafetyPrePlan.Design();

                            pcReport.Clear();
                            theReportSafetyPrePlan.Prepare();
                            theReportSafetyPrePlan.Preview = pcReport;
                            theReportSafetyPrePlan.ShowPrepared();
                            

                        }

                        else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Ventillation")
                        {
                            LoadWP();
                            //Safety Actions
                            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
                            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyActions.SqlStatement = " SELECT *, SUBSTRING(HOD, CHARINDEX (':', HOD )+1, DATALENGTH(HOD) ) as MO, SUBSTRING(RespPerson, CHARINDEX (':', RespPerson )+1, DATALENGTH(RespPerson) ) as RespPerson \r\n " +
                                                             ",case when CompNotes<> '' then '" + ImageFolder + "\\Preplanning\\Ventilation\\' + CompNotes + '.png' \r\n " +
                                                             " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n " +
                                                             " where Type = 'PPVT' and Completiondate is null and workplace in ('" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "', '" + WP4Desc + "', '" + WP5Desc + "')";

                            _LoadSafetyActions.ResultsTableName = "Actions";
                            _LoadSafetyActions.ExecuteInstruction();

                            DataSet dsActions = new DataSet();
                            dsActions.Tables.Clear();
                            dsActions.Tables.Add(_LoadSafetyActions.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsActions);

                            //Cycles
                            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
                            _dbManCycle.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + clsWorkplaceSummary.SelectedCrew + "' and pm = '" + clsWorkplaceSummary.SelectedProdmonth + "' ";

                            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManCycle.ResultsTableName = "Cycles";
                            _dbManCycle.ExecuteInstruction();

                            DataSet dsCycle = new DataSet();
                            dsCycle.Tables.Clear();
                            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsCycle);

                            //open actions
                            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
                            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManOpenActions.SqlStatement = "select 1 num, convert(varchar(50) , actiondate, (111)) datesubmitted,workplace, action_title [action],hazard from  [dbo].[tbl_Incidents] \r\n" +
                                                             "where workplace in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') and Action_Status <> 'Closed'\r\n" +
                                                             " union select 2 num, null,'','','' order by num ";

                            _dbManOpenActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManOpenActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManOpenActions.ResultsTableName = "OpenManActions";
                            _dbManOpenActions.ExecuteInstruction();

                            DataSet dsOpenActions = new DataSet();
                            dsOpenActions.Tables.Clear();
                            dsOpenActions.Tables.Add(_dbManOpenActions.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsOpenActions);

                            //Auth Watermark
                            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
                            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManParaAuth.SqlStatement = "select top (1) case when VentDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                                     "where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' ";

                            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManParaAuth.ResultsTableName = "ParaAuth";
                            _dbManParaAuth.ExecuteInstruction();

                            _Auth = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

                            DataSet dsParaAuth = new DataSet();
                            dsParaAuth.Tables.Clear();
                            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsParaAuth);

                            //LoadNotes
                            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
                            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_VentCapture_Notes] where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' and notes is not null  ";
                            _LoadSafetyNotes.ResultsTableName = "Notes";
                            _LoadSafetyNotes.ExecuteInstruction();

                            DataSet dsSafteyNotes = new DataSet();
                            dsSafteyNotes.Tables.Clear();
                            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsSafteyNotes);

                            ///Graphs
                            MWDataManager.clsDataAccess _dbMandataGraphWP1Temp = new MWDataManager.clsDataAccess();
                            _dbMandataGraphWP1Temp.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMandataGraphWP1Temp.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + WP1Desc + "' wp1new,'" + WP2Desc + "' wp2new,'" + WP3Desc + "' wp3new,'" + WP4Desc + "' wp4new,'" + WP5Desc + "' wp5new, \r\n" +
                                                                " case when wp1answer is null then '-100' when wp1answer = '0.0' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                                " case when wp2answer is null then '-100' when wp2answer = '0.0' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                                " case when wp3answer is null then '-100' when wp3answer = '0.0' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                                " case when wp4answer is null then '-100' when wp4answer = '0.0' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                                " case when wp5answer is null then '-100' when wp5answer = '0.0' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                                " from (     \r\n" +

                                                                " select top(6) prodmonth from tbl_Planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm1zz,Workplace wp1,answer wp1answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP1Desc + "' and questid = 1     \r\n" +
                                                                " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm2zz,Workplace wp2,answer wp2answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP2Desc + "' and questid = 1     \r\n" +
                                                                " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm3zz,Workplace wp3,answer wp3answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP3Desc + "' and questid = 1     \r\n" +
                                                                " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm4zz,Workplace wp4,answer wp4answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP4Desc + "' and questid = 1     \r\n" +
                                                                " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm5zz,Workplace wp5,answer wp5answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP5Desc + "' and questid = 1     \r\n" +
                                                                " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                                " order by prodmonth";

                            _dbMandataGraphWP1Temp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMandataGraphWP1Temp.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMandataGraphWP1Temp.ResultsTableName = "Actions1GraphTemp";
                            _dbMandataGraphWP1Temp.ExecuteInstruction();

                            DataSet dsGraphWp1Temp = new DataSet();
                            dsGraphWp1Temp.Tables.Clear();
                            dsGraphWp1Temp.Tables.Add(_dbMandataGraphWP1Temp.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsGraphWp1Temp);

                            MWDataManager.clsDataAccess _dbMandataGraphWP1Vel = new MWDataManager.clsDataAccess();
                            _dbMandataGraphWP1Vel.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMandataGraphWP1Vel.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + WP1Desc + "' wp1new,'" + WP2Desc + "' wp2new,'" + WP3Desc + "' wp3new,'" + WP4Desc + "' wp4new,'" + WP5Desc + "' wp5new, \r\n" +
                                                                " case when wp1answer is null then '-100' when wp1answer = '0.0' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                                " case when wp2answer is null then '-100' when wp2answer = '0.0' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                                " case when wp3answer is null then '-100' when wp3answer = '0.0' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                                " case when wp4answer is null then '-100' when wp4answer = '0.0' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                                " case when wp5answer is null then '-100' when wp5answer = '0.0' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                                " from (     \r\n" +

                                                                " select top(6) prodmonth from tbl_Planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm1zz,Workplace wp1,answer wp1answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP1Desc + "' and questid = 2     \r\n" +
                                                                " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm2zz,Workplace wp2,answer wp2answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP2Desc + "' and questid = 2    \r\n" +
                                                                " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm3zz,Workplace wp3,answer wp3answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP3Desc + "' and questid = 2     \r\n" +
                                                                " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm4zz,Workplace wp4,answer wp4answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP4Desc + "' and questid = 2     \r\n" +
                                                                " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm5zz,Workplace wp5,answer wp5answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP5Desc + "' and questid = 2     \r\n" +
                                                                " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                                " order by prodmonth";

                            _dbMandataGraphWP1Vel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMandataGraphWP1Vel.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMandataGraphWP1Vel.ResultsTableName = "Actions1GraphVel";
                            _dbMandataGraphWP1Vel.ExecuteInstruction();

                            DataSet dsGraphWp1Vel = new DataSet();
                            dsGraphWp1Vel.Tables.Clear();
                            dsGraphWp1Vel.Tables.Add(_dbMandataGraphWP1Vel.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsGraphWp1Vel);

                            MWDataManager.clsDataAccess _dbMandataGraphWP1AU = new MWDataManager.clsDataAccess();
                            _dbMandataGraphWP1AU.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMandataGraphWP1AU.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + WP1Desc + "' wp1new,'" + WP2Desc + "' wp2new,'" + WP3Desc + "' wp3new,'" + WP4Desc + "' wp4new,'" + WP5Desc + "' wp5new, \r\n" +
                                                                " case when wp1answer is null then '-100' when wp1answer = '0.0' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                                " case when wp2answer is null then '-100' when wp2answer = '0.0' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                                " case when wp3answer is null then '-100' when wp3answer = '0.0' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                                " case when wp4answer is null then '-100' when wp4answer = '0.0' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                                " case when wp5answer is null then '-100' when wp5answer = '0.0' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                                 " from (     \r\n" +

                                                                 " select top(6) prodmonth from tbl_Planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                                 " left outer join(     \r\n" +
                                                                 " select prodmonth pm1zz,Workplace wp1,answer wp1answer     \r\n" +
                                                                 " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                 " where a.workplace = b.workplaceid and description = '" + WP1Desc + "' and questid = 4     \r\n" +
                                                                 " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                                 " left outer join(     \r\n" +
                                                                 " select prodmonth pm2zz,Workplace wp2,answer wp2answer     \r\n" +
                                                                 " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                 " where a.workplace = b.workplaceid and description = '" + WP2Desc + "' and questid = 4     \r\n" +
                                                                 " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                                 " left outer join(     \r\n" +
                                                                 " select prodmonth pm3zz,Workplace wp3,answer wp3answer     \r\n" +
                                                                 " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                 " where a.workplace = b.workplaceid and description = '" + WP3Desc + "' and questid = 4     \r\n" +
                                                                 " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                                 " left outer join(     \r\n" +
                                                                 " select prodmonth pm4zz,Workplace wp4,answer wp4answer     \r\n" +
                                                                 " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                 " where a.workplace = b.workplaceid and description = '" + WP4Desc + "' and questid = 4     \r\n" +
                                                                 " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                                 " left outer join(     \r\n" +
                                                                 " select prodmonth pm5zz,Workplace wp5,answer wp5answer     \r\n" +
                                                                 " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                 " where a.workplace = b.workplaceid and description = '" + WP5Desc + "' and questid = 4     \r\n" +
                                                                 " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                                 " order by prodmonth";

                            _dbMandataGraphWP1AU.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMandataGraphWP1AU.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMandataGraphWP1AU.ResultsTableName = "Actions1GraphAu";
                            _dbMandataGraphWP1AU.ExecuteInstruction();

                            DataSet dsGraphWp1AU = new DataSet();
                            dsGraphWp1AU.Tables.Clear();
                            dsGraphWp1AU.Tables.Add(_dbMandataGraphWP1AU.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsGraphWp1AU);

                            MWDataManager.clsDataAccess _dbMandataGraphWP1SCP = new MWDataManager.clsDataAccess();
                            _dbMandataGraphWP1SCP.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbMandataGraphWP1SCP.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + WP1Desc + "' wp1new,'" + WP2Desc + "' wp2new,'" + WP3Desc + "' wp3new,'" + WP4Desc + "' wp4new,'" + WP5Desc + "' wp5new, \r\n" +
                                                                " case when wp1answer is null then '-100' when wp1answer = '0.0' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                                " case when wp2answer is null then '-100' when wp2answer = '0.0' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                                " case when wp3answer is null then '-100' when wp3answer = '0.0' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                                " case when wp4answer is null then '-100' when wp4answer = '0.0' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                                " case when wp5answer is null then '-100' when wp5answer = '0.0' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                                " from (     \r\n" +

                                                                " select top(6) prodmonth from tbl_Planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm1zz,Workplace wp1,answer wp1answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP1Desc + "' and questid = 3     \r\n" +
                                                                " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm2zz,Workplace wp2,answer wp2answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP2Desc + "' and questid = 3     \r\n" +
                                                                " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm3zz,Workplace wp3,answer wp3answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP3Desc + "' and questid = 3     \r\n" +
                                                                " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm4zz,Workplace wp4,answer wp4answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP4Desc + "' and questid = 3     \r\n" +
                                                                " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                                " left outer join(     \r\n" +
                                                                " select prodmonth pm5zz,Workplace wp5,answer wp5answer     \r\n" +
                                                                " from [dbo].[tbl_PrePlanning_Vent] a, tbl_Workplace b      \r\n" +
                                                                " where a.workplace = b.workplaceid and description = '" + WP5Desc + "' and questid = 3     \r\n" +
                                                                " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                                " order by prodmonth";

                            _dbMandataGraphWP1SCP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbMandataGraphWP1SCP.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbMandataGraphWP1SCP.ResultsTableName = "Actions1GraphSCP";
                            _dbMandataGraphWP1SCP.ExecuteInstruction();

                            DataSet dsGraphWp1SCP = new DataSet();
                            dsGraphWp1SCP.Tables.Clear();
                            dsGraphWp1SCP.Tables.Add(_dbMandataGraphWP1SCP.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsGraphWp1SCP);

                            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
                            _dbAns.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbAns.SqlStatement = " select* from(  \r\n" +
                                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                                    " from tbl_PrePlanning_VentQuest q, tbl_PrePlanning_Vent c, tbl_Workplace w \r\n" +
                                                    " where q.QuestID = c.QuestID \r\n" +
                                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                                    " and c.prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' \r\n" +
                                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                                    " )a order by Workplace, OrderBy \r\n";

                            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbAns.ResultsTableName = "Answers";
                            _dbAns.ExecuteInstruction();

                            DataSet dsAnss = new DataSet();
                            dsAnss.Tables.Clear();
                            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

                            theReportVentPrePlan.RegisterData(dsAnss);

                            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth));
                            string Date = ProductionGlobal.ProductionGlobal.Prod2;


                            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
                            {
                                if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 1)
                                {
                                    theReportVentPrePlan.SetParameterValue("ActionsCheck", "Y");
                                }
                                else
                                {
                                    theReportVentPrePlan.SetParameterValue("ActionsCheck", "N");
                                }
                            }
                            else
                            {
                                theReportVentPrePlan.SetParameterValue("ActionsCheck", "N");
                            }

                            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
                            {
                                theReportVentPrePlan.SetParameterValue("OpenCheck", "Y");
                            }
                            else
                            {
                                theReportVentPrePlan.SetParameterValue("OpenCheck", "N");
                            }

                           
                            theReportVentPrePlan.Load(ReportsFolder + "PrePlanExeNoteVent.frx");

                            theReportVentPrePlan.SetParameterValue("Banner", ProductionGlobal.ProductionGlobal.Banner);
                            theReportVentPrePlan.SetParameterValue("Activity", clsWorkplaceSummary.SelectedActivity);
                            theReportVentPrePlan.SetParameterValue("Prodmonth", Date);
                            theReportVentPrePlan.SetParameterValue("Crew", clsWorkplaceSummary.SelectedCrew);
                            theReportVentPrePlan.SetParameterValue("WP1Desc", WP1Desc);
                            theReportVentPrePlan.SetParameterValue("WP2Desc", WP2Desc);
                            theReportVentPrePlan.SetParameterValue("WP3Desc", WP3Desc);
                            theReportVentPrePlan.SetParameterValue("WP4Desc", WP4Desc);
                            theReportVentPrePlan.SetParameterValue("WP5Desc", WP5Desc);


                            loadImage("Vent");
                            theReportVentPrePlan.SetParameterValue("Image", _Attachment);
                            theReportVentPrePlan.SetParameterValue("Auth", _Auth);

                            //theReportVentPrePlan.Design(); 

                            pcReport.Clear();
                            theReportVentPrePlan.Prepare();
                            theReportVentPrePlan.Preview = pcReport;
                            theReportVentPrePlan.ShowPrepared();
                            

                        }

                        else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Engineering")
                        {
                            LoadWP();
                            //Safety Actions
                            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
                            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyActions.SqlStatement = " SELECT 1 num,TheDate,CompletionDate,Workplace, Action, HOD, RespPerson, isnull(compnotes, '') Compnotes1,  \r\n" +
                                                              " case when CompNotes <> '' then '" + ImageFolder + "\\Preplanning\\Geology\\' + CompNotes + '.png' \r\n" +
                                                              " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
                                                              " where Type = 'PPEG' and Completiondate is null and workplace in ('" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "','" + WP4Desc + "','" + WP5Desc + "') union select 2 num, null,null,'','','','','','' order by num ";
                            _LoadSafetyActions.ResultsTableName = "Actions";
                            _LoadSafetyActions.ExecuteInstruction();

                            DataSet dsActions = new DataSet();
                            dsActions.Tables.Clear();
                            dsActions.Tables.Add(_LoadSafetyActions.ResultsDataTable);

                            theReportEngPrePlan.RegisterData(dsActions);

                            //Cycles
                            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
                            _dbManCycle.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + clsWorkplaceSummary.SelectedCrew + "' and Prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' ";

                            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManCycle.ResultsTableName = "Cycles";
                            _dbManCycle.ExecuteInstruction();

                            DataSet dsCycle = new DataSet();
                            dsCycle.Tables.Clear();
                            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

                            theReportEngPrePlan.RegisterData(dsCycle);

                            //open actions
                            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
                            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManOpenActions.SqlStatement = "select 1 num, convert(varchar(50) , actiondate, (111)) datesubmitted,workplace, action_title [action],hazard from  [dbo].[tbl_Incidents] \r\n" +
                                                             "where workplace in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') and Action_Status <> 'Closed'\r\n" +
                                                             " union select 2 num, null,'','','' order by num ";

                            _dbManOpenActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManOpenActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManOpenActions.ResultsTableName = "OpenManActions";
                            _dbManOpenActions.ExecuteInstruction();

                            DataSet dsOpenActions = new DataSet();
                            dsOpenActions.Tables.Clear();
                            dsOpenActions.Tables.Add(_dbManOpenActions.ResultsDataTable);

                            theReportEngPrePlan.RegisterData(dsOpenActions);

                            //Auth Watermark
                            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
                            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManParaAuth.SqlStatement = "select top (1) case when EngineeringDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                                     "where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' ";

                            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManParaAuth.ResultsTableName = "ParaAuth";
                            _dbManParaAuth.ExecuteInstruction();

                            _Auth = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

                            DataSet dsParaAuth = new DataSet();
                            dsParaAuth.Tables.Clear();
                            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

                            theReportEngPrePlan.RegisterData(dsParaAuth);

                            //LoadNotes
                            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
                            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_EngineeringCapture_Notes] where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' and notes is not null  ";
                            _LoadSafetyNotes.ResultsTableName = "Notes";
                            _LoadSafetyNotes.ExecuteInstruction();

                            DataSet dsSafteyNotes = new DataSet();
                            dsSafteyNotes.Tables.Clear();
                            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

                            theReportEngPrePlan.RegisterData(dsSafteyNotes);

                            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
                            _dbAns.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbAns.SqlStatement = " select* from(  \r\n" +
                                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                                    " from tbl_PrePlanning_EngineeringQuest q, tbl_PrePlanning_Engineering c, tbl_Workplace w  \r\n" +
                                                    " where q.QuestID = c.QuestID \r\n" +
                                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                                    " and c.prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' \r\n" +
                                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                                    " )a order by Workplace, OrderBy \r\n";

                            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbAns.ResultsTableName = "Answers";
                            _dbAns.ExecuteInstruction();

                            DataSet dsAnss = new DataSet();
                            dsAnss.Tables.Clear();
                            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

                            theReportEngPrePlan.RegisterData(dsAnss);

                            theReportEngPrePlan.Load(ReportsFolder + "PrePlanExeNoteEng.frx");

                            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth));
                            string Date = ProductionGlobal.ProductionGlobal.Prod2;

                            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
                            {
                                theReportEngPrePlan.SetParameterValue("OpenCheck", "Y");
                            }
                            else
                            {
                                theReportEngPrePlan.SetParameterValue("OpenCheck", "N");
                            }

                            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
                            {
                                if (_LoadSafetyActions.ResultsDataTable.Rows[0]["TheDate"].ToString() == "1/1/1900 12:00:00 AM")
                                {
                                    theReportEngPrePlan.SetParameterValue("ActionsCheck", "N");
                                }
                                else
                                {
                                    theReportEngPrePlan.SetParameterValue("ActionsCheck", "Y");
                                }
                            }
                            else
                            {
                                theReportEngPrePlan.SetParameterValue("ActionsCheck", "N");
                            }

                            theReportEngPrePlan.SetParameterValue("Prodmonth", Date);
                            theReportEngPrePlan.SetParameterValue("Crew", clsWorkplaceSummary.SelectedCrew);

                            loadImage("Engineering");
                            theReportEngPrePlan.SetParameterValue("Image", _Attachment);

                            theReportEngPrePlan.SetParameterValue("Activity", clsWorkplaceSummary.SelectedActivity);
                            theReportEngPrePlan.SetParameterValue("Auth", _Auth);

                            //theReportEngPrePlan.Design();

                            pcReport.Clear();
                            theReportEngPrePlan.Prepare();
                            theReportEngPrePlan.Preview = pcReport;
                            theReportEngPrePlan.ShowPrepared();
                       

                        }

                        else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Survey")
                        {
                            LoadWP();
                            //Safety Actions
                            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
                            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyActions.SqlStatement = " SELECT *, SUBSTRING(HOD, CHARINDEX(':', HOD) + 1, DATALENGTH(HOD)) as MO, SUBSTRING(RespPerson, CHARINDEX(':', RespPerson) + 1, DATALENGTH(RespPerson)) as RespPerson \r\n " +
                                                             ",case when CompNotes<> '' then '" + ImageFolder + "\\Preplanning\\Survey\\' + CompNotes + '.png' \r\n " +
                                                             " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n " +
                                                             " where Type = 'PPSR' and Completiondate is null and workplace in ('" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "', '" + WP4Desc + "', '" + WP5Desc + "')";

                            _LoadSafetyActions.ResultsTableName = "Actions";
                            _LoadSafetyActions.ExecuteInstruction();

                            DataSet dsActions = new DataSet();
                            dsActions.Tables.Clear();
                            dsActions.Tables.Add(_LoadSafetyActions.ResultsDataTable);

                            theReportSurveyPrePlan.RegisterData(dsActions);

                            //Cycles
                            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
                            _dbManCycle.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + clsWorkplaceSummary.SelectedCrew + "' and pm = '" + clsWorkplaceSummary.SelectedProdmonth + "' ";

                            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManCycle.ResultsTableName = "Cycles";
                            _dbManCycle.ExecuteInstruction();

                            DataSet dsCycle = new DataSet();
                            dsCycle.Tables.Clear();
                            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

                            theReportSurveyPrePlan.RegisterData(dsCycle);

                            //open actions
                            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
                            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManOpenActions.SqlStatement = "select 1 num, convert(varchar(50) , actiondate, (111)) datesubmitted,workplace, action_title [action],hazard from  [dbo].[tbl_Incidents] \r\n" +
                                                             "where workplace in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') and Action_Status <> 'Closed'\r\n" +
                                                             " union select 2 num, null,'','','' order by num ";

                            _dbManOpenActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManOpenActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManOpenActions.ResultsTableName = "OpenManActions";
                            _dbManOpenActions.ExecuteInstruction();

                            DataSet dsOpenActions = new DataSet();
                            dsOpenActions.Tables.Clear();
                            dsOpenActions.Tables.Add(_dbManOpenActions.ResultsDataTable);

                            theReportSurveyPrePlan.RegisterData(dsOpenActions);

                            //Auth Watermark
                            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
                            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _dbManParaAuth.SqlStatement = "select top (1) case when VentDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                                     "where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' ";

                            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManParaAuth.ResultsTableName = "ParaAuth";
                            _dbManParaAuth.ExecuteInstruction();

                            _Auth = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

                            DataSet dsParaAuth = new DataSet();
                            dsParaAuth.Tables.Clear();
                            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

                            theReportSurveyPrePlan.RegisterData(dsParaAuth);

                            //LoadNotes
                            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
                            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_SurveyCapt_Notes] where prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "' and crew = '" + clsWorkplaceSummary.SelectedCrew + "' and notes is not null  ";
                            _LoadSafetyNotes.ResultsTableName = "Notes";
                            _LoadSafetyNotes.ExecuteInstruction();

                            DataSet dsSafteyNotes = new DataSet();
                            dsSafteyNotes.Tables.Clear();
                            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

                            theReportSurveyPrePlan.RegisterData(dsSafteyNotes);

                            ///Questions and Answers////
                            ///////2020-04-07///////////      

                            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
                            _dbAns.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                            if (clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                _dbAns.SqlStatement = " select* from(  \r\n" +
                                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                                    " from tbl_PrePlanning_SurveyQuest_Dev q, tbl_PrePlanning_Survey c, tbl_Workplace w \r\n" +
                                                    " where q.QuestID = c.QuestID \r\n" +
                                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                                    " and c.prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' \r\n" +
                                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                                    " )a order by Workplace, OrderBy \r\n";
                            }
                            else
                            {
                                _dbAns.SqlStatement = " select* from(  \r\n" +
                                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                                    " from tbl_PrePlanning_SurveyQuest q, tbl_PrePlanning_Survey c, tbl_Workplace w \r\n" +
                                                    " where q.QuestID = c.QuestID \r\n" +
                                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                                    " and c.prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' \r\n" +
                                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                                    " )a order by Workplace, OrderBy \r\n";
                            }
                            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbAns.ResultsTableName = "Answers";
                            _dbAns.ExecuteInstruction();

                            DataSet dsAnss = new DataSet();
                            dsAnss.Tables.Clear();
                            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

                            theReportSurveyPrePlan.RegisterData(dsAnss);

                            //CaptureInfo
                            MWDataManager.clsDataAccess _dbManCapt = new MWDataManager.clsDataAccess();
                            _dbManCapt.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                            if (clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                _dbManCapt.SqlStatement = " Select qst.question,WP1,WP2,WP3,WP4,WP5 from tbl_PrePlanning_SurveyQuest_Dev Qst \r\n" +
                                                        "left outer join ( \r\n" +
                                                        "Select questid,answer WP1 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP1 + "' \r\n" +
                                                        " ) fstWp on qst.questid = fstWp.questid \r\n" +

                                                        " left outer join ( \r\n" +
                                                        "Select questid,answer WP2 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP2 + "' \r\n" +
                                                        " ) secWp on qst.questid = secWp.questid \r\n" +


                                                        "  left outer join ( \r\n" +
                                                        "Select questid,answer WP3 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP3 + "' \r\n" +
                                                        " ) thrdWp on qst.questid = thrdWp.questid \r\n" +

                                                        " left outer join ( \r\n" +
                                                        "Select questid,answer WP4 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP4 + "' \r\n" +
                                                        " ) frthWp on qst.questid = frthWp.questid \r\n" +

                                                        "  left outer join ( \r\n" +
                                                        "Select questid,answer WP5 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP5 + "' \r\n" +
                                                        " ) ftfhWp on qst.questid = ftfhWp.questid \r\n" +

                                                        " ";
                            }
                            else
                            {
                                _dbManCapt.SqlStatement = " Select qst.question,WP1,WP2,WP3,WP4,WP5 from tbl_PrePlanning_SurveyQuest Qst \r\n" +

                                                        "left outer join ( \r\n" +
                                                        "Select questid,answer WP1 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP1 + "' \r\n" +
                                                        " ) fstWp on qst.questid = fstWp.questid \r\n" +

                                                        " left outer join ( \r\n" +
                                                        "Select questid,answer WP2 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP2 + "' \r\n" +
                                                        " ) secWp on qst.questid = secWp.questid \r\n" +


                                                        "  left outer join ( \r\n" +
                                                        "Select questid,answer WP3 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP3 + "' \r\n" +
                                                        " ) thrdWp on qst.questid = thrdWp.questid \r\n" +

                                                        " left outer join ( \r\n" +
                                                        "Select questid,answer WP4 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP4 + "' \r\n" +
                                                        " ) frthWp on qst.questid = frthWp.questid \r\n" +

                                                        "  left outer join ( \r\n" +
                                                        "Select questid,answer WP5 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                                        "where  prodmonth = '" + clsWorkplaceSummary.SelectedProdmonth + "' and section = '" + clsWorkplaceSummary.SelectedMOSection + "'  \r\n" +
                                                        "and workplace = '" + WP5 + "' \r\n" +
                                                        " ) ftfhWp on qst.questid = ftfhWp.questid \r\n" +

                                                        "where qst.questid in (16,17,21,20,22)  \r\n" +
                                                        " ";
                            }
                            _dbManCapt.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManCapt.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManCapt.ResultsTableName = "Captinfo";
                            _dbManCapt.ExecuteInstruction();

                            DataSet dsCapt = new DataSet();
                            dsCapt.Tables.Clear();
                            dsCapt.Tables.Add(_dbManCapt.ResultsDataTable);

                            theReportSurveyPrePlan.RegisterData(dsCapt);

                            //Complaince Check this query
                            MWDataManager.clsDataAccess _dbManComp = new MWDataManager.clsDataAccess();
                            _dbManComp.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                            if (clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                _dbManComp.SqlStatement = "Select substring(convert(varchar(11),convert(date,convert(varchar(10),a.Prodmonth)+'01'),106),4,88) Prodlbl,* from ( \r\n" +
                                                        "Select a.prodmonth, WP1, case when a.bsqm is null then 0 when a.PSqm = 0 then 0  else convert(decimal(8,0), a.Bsqm/a.PSqm * 100) end as Compliance1 \r\n" +
                                                        ",'a' a \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.Description WP1,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "' \r\n" +
                                                        "and p.workplaceid = '" + WP1 + "' \r\n" +
                                                        "group by  prodmonth,w.Description ) a  )a \r\n" +

                                                        "left outer join ( \r\n" +

                                                        "Select prodmonth,WP2, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance2  \r\n" +
                                                        ",'a' b \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.description WP2,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "'\r\n" +
                                                        "and p.workplaceid = '" + WP2 + "' \r\n" +
                                                        "group by  prodmonth,w.Description ) a ) b on a.a = b.b and a.prodmonth = b.prodmonth \r\n" +

                                                        "left outer join ( \r\n" +

                                                        "Select prodmonth,WP3, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance3  \r\n" +
                                                        ",'a' c \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.description WP3,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "'  \r\n" +
                                                        "and p.workplaceid = '" + WP3 + "' \r\n" +
                                                        "group by  prodmonth,w.Description )a ) c on a.a = c.c and a.prodmonth = c.prodmonth \r\n" +

                                                        "left outer join ( \r\n" +

                                                        "Select prodmonth,WP4, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance4  \r\n" +
                                                        ",'a' d \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.Description WP4,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "' \r\n" +
                                                        "and p.workplaceid = '" + WP4 + "' \r\n" +
                                                        "group by  prodmonth,w.Description )a ) d on a.a = d.d and a.prodmonth = d.prodmonth \r\n" +

                                                        "left outer join ( \r\n" +

                                                        "Select prodmonth,WP5, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance5  \r\n" +
                                                        ",'a' e \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.description WP5,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "'  \r\n" +
                                                        "and p.workplaceid = '" + WP5 + "' \r\n" +
                                                        "group by  prodmonth,w.Description ) a ) e  on a.a = e.e and a.prodmonth = e.prodmonth ";
                            }
                            else
                            {
                                _dbManComp.SqlStatement = "Select substring(convert(varchar(11),convert(date,convert(varchar(10),a.Prodmonth)+'01'),106),4,88) Prodlbl,* from ( \r\n" +
                                                        "Select a.prodmonth, WP1, case when a.bsqm is null then 0 when a.PSqm = 0 then 0  else convert(decimal(8,0), a.Bsqm/a.PSqm * 100) end as Compliance1 \r\n" +
                                                        ",'a' a \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.Description WP1,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "' \r\n" +
                                                        "and p.workplaceid = '" + WP1 + "' \r\n" +
                                                        "group by  prodmonth,w.Description ) a  )a \r\n" +

                                                        "left outer join ( \r\n" +

                                                        "Select prodmonth,WP2, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance2  \r\n" +
                                                        ",'a' b \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.description WP2,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "'\r\n" +
                                                        "and p.workplaceid = '" + WP2 + "' \r\n" +
                                                        "group by  prodmonth,w.Description ) a ) b on a.a = b.b and a.prodmonth = b.prodmonth \r\n" +

                                                        "left outer join ( \r\n" +

                                                        "Select prodmonth,WP3, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance3  \r\n" +
                                                        ",'a' c \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.description WP3,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "'  \r\n" +
                                                        "and p.workplaceid = '" + WP3 + "' \r\n" +
                                                        "group by  prodmonth,w.Description )a ) c on a.a = c.c and a.prodmonth = c.prodmonth \r\n" +

                                                        "left outer join ( \r\n" +

                                                        "Select prodmonth,WP4, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance4  \r\n" +
                                                        ",'a' d \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.Description WP4,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "' \r\n" +
                                                        "and p.workplaceid = '" + WP4 + "' \r\n" +
                                                        "group by  prodmonth,w.Description )a ) d on a.a = d.d and a.prodmonth = d.prodmonth \r\n" +

                                                        "left outer join ( \r\n" +

                                                        "Select prodmonth,WP5, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance5  \r\n" +
                                                        ",'a' e \r\n" +
                                                        "from (   \r\n" +
                                                        "Select prodmonth,w.description WP5,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth) - 3) + "'  \r\n" +
                                                        "and p.workplaceid = '" + WP5 + "' \r\n" +
                                                        "group by  prodmonth,w.Description ) a ) e  on a.a = e.e and a.prodmonth = e.prodmonth ";
                            }

                            _dbManComp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManComp.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManComp.ResultsTableName = "Comp";
                            _dbManComp.ExecuteInstruction();

                            DataSet dsComp = new DataSet();
                            dsComp.Tables.Clear();
                            dsComp.Tables.Add(_dbManComp.ResultsDataTable);

                            theReportSurveyPrePlan.RegisterData(dsComp);


                            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(clsWorkplaceSummary.SelectedProdmonth));
                            string Date = ProductionGlobal.ProductionGlobal.Prod2;

                            if (clsWorkplaceSummary.SelectedActivity == "Dev")
                            {
                                
                                theReportSurveyPrePlan.Load(ReportsFolder + "PrePlanExeNoteSurvey.frx");

                            }
                            else
                            {
                                
                                theReportSurveyPrePlan.Load(ReportsFolder + "PrePlanExeNoteSurvey.frx");

                            }

                            theReportSurveyPrePlan.SetParameterValue("Prodmonth", Date);
                            theReportSurveyPrePlan.SetParameterValue("Crew", clsWorkplaceSummary.SelectedCrew);

                            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
                            {
                                theReportSurveyPrePlan.SetParameterValue("OpenCheck", "Y");
                            }
                            else
                            {
                                theReportSurveyPrePlan.SetParameterValue("OpenCheck", "N");
                            }

                            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
                            {
                                if (_LoadSafetyActions.ResultsDataTable.Rows[0]["TheDate"].ToString() == "1/1/1900 12:00:00 AM")
                                {
                                    theReportSurveyPrePlan.SetParameterValue("ActionsCheck", "N");
                                }
                                else
                                {
                                    theReportSurveyPrePlan.SetParameterValue("ActionsCheck", "Y");
                                }
                            }
                            else
                            {
                                theReportSurveyPrePlan.SetParameterValue("ActionsCheck", "N");
                            }

                            loadImage("Survey");

                            theReportSurveyPrePlan.SetParameterValue("Auth", _Auth);

                            theReportSurveyPrePlan.SetParameterValue("Banner", ProductionGlobal.ProductionGlobal.Banner);
                            theReportSurveyPrePlan.SetParameterValue("Activity", clsWorkplaceSummary.SelectedActivity);
                            theReportSurveyPrePlan.SetParameterValue("Image", _Attachment);
                            theReportSurveyPrePlan.SetParameterValue("WP1Desc", WP1Desc);
                            theReportSurveyPrePlan.SetParameterValue("WP2Desc", WP2Desc);
                            theReportSurveyPrePlan.SetParameterValue("WP3Desc", WP3Desc);
                            theReportSurveyPrePlan.SetParameterValue("WP4Desc", WP4Desc);
                            theReportSurveyPrePlan.SetParameterValue("WP5Desc", WP5Desc);


                            //theReportSurveyPrePlan.Design();

                            pcReport.Clear();
                            theReportSurveyPrePlan.Prepare();
                            theReportSurveyPrePlan.Preview = pcReport;
                            theReportSurveyPrePlan.ShowPrepared();
                            

                        }

                    }
                }

            }
            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            LoadReport();
        }

        void loadImage(string _Department)
        {
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(ReportsFolder + "\\Images\\Preplanning" + "\\" + _Department + "   ");
            if (dir2.Exists)
            {
                IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                var files = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                foreach (var item in files)
                {
                    string ext = item.Name.Replace(System.IO.Path.GetFileNameWithoutExtension(item.Name), string.Empty);

                    if (item.ToString() == clsWorkplaceSummary.SelectedCrew + clsWorkplaceSummary.SelectedProdmonth + ext)
                    {
                        _Attachment = ReportsFolder + "\\Images\\Preplanning" + "\\" + _Department + "\\" + item.ToString();
                    }
                }
                if (_Attachment != string.Empty)
                {
                    _AttachmentReturned = _Attachment;
                }
                else
                {
                    _AttachmentReturned = "NoImage";
                }
            }

        }

        private void LoadWP()
        {
            MWDataManager.clsDataAccess _LoadWorkplaces = new MWDataManager.clsDataAccess();
            _LoadWorkplaces.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _LoadWorkplaces.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadWorkplaces.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadWorkplaces.SqlStatement = " Select pm.Workplaceid, w.Description, ''LastInspection " +
                                           " From tbl_PlanMonth AS pm, " +
                                           " tbl_Workplace w " +
                                           " Where w.WORKPLACEID = pm.Workplaceid " +
                                           " And pm.Prodmonth = " + clsWorkplaceSummary.SelectedProdmonth + " " +
                                           " And pm.OrgUnitDS like '" + clsWorkplaceSummary.SelectedCrew + "%' " +
                                           " Order By w.DESCRIPTION ";

            _LoadWorkplaces.ExecuteInstruction();

            DataTable tbl_Workplace = _LoadWorkplaces.ResultsDataTable;
            // do 
            int x = 0;

            foreach (DataRow r in tbl_Workplace.Rows)
            {
                if (x == 0)
                {
                    WP1 = r["Workplaceid"].ToString();
                    WP1Desc = r["Description"].ToString();

                }
                if (x == 1)
                {
                    WP2 = r["Workplaceid"].ToString();
                    WP2Desc = r["Description"].ToString();
                }
                if (x == 2)
                {
                    WP3 = r["Workplaceid"].ToString();
                    WP3Desc = r["Description"].ToString();
                }
                if (x == 3)
                {
                    WP4 = r["Workplaceid"].ToString();
                    WP4Desc = r["Description"].ToString();
                }
                if (x == 4)
                {
                    WP5 = r["Workplaceid"].ToString();
                    WP5Desc = r["Description"].ToString();
                }
                x = x + 1;
            }
        }

    }

}
