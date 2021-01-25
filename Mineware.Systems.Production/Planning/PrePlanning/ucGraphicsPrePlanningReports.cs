using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Planning
{

    public partial class ucGraphicsPrePlanningReports : BaseUserControl
    {
        private string ReportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        private string ImageFolder =  ProductionGlobal.ProductionGlobalTSysSettings._RepDir;

        public String _Section;
        public String _Crew;
        public String _ProdMonth;
        public String _Attachment;
        public String _AttachmentReturned;
        public String _Activity;

        private string Authorise = string.Empty;

        string WP1 = string.Empty;
        string WP2 = string.Empty;
        string WP3 = string.Empty;
        string WP4 = string.Empty;
        string WP5 = string.Empty;

        string WP1Desc = string.Empty;
        string WP2Desc = string.Empty;
        string WP3Desc = string.Empty;
        string WP4Desc = string.Empty;
        string WP5Desc = string.Empty;

        Report theReport = new Report();

        string RepDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDir;

        public ucGraphicsPrePlanningReports()
        {
            InitializeComponent();
        }

        private void ucGraphicsPrePlanningReports_Load(object sender, EventArgs e)
        {
            tbCrew.EditValue = _Crew;
            tbSection.EditValue = _Section;
            tbProdMonth.EditValue = _ProdMonth;

            LoadWP();
        }

        private void LoadWP()
        {
            MWDataManager.clsDataAccess _LoadWorkplaces = new MWDataManager.clsDataAccess();
            _LoadWorkplaces.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadWorkplaces.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadWorkplaces.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadWorkplaces.SqlStatement = " Select pm.Workplaceid, w.Description, ''LastInspection " +
                                           " From tbl_PlanMonth AS pm, " +
                                           " tbl_Workplace w " +
                                           " Where w.WORKPLACEID = pm.Workplaceid " +
                                           " And pm.Prodmonth = " + _ProdMonth + " " +
                                           " And pm.OrgUnitDS like '" + _Crew + "%' " +
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

        //private void loadSafetytobedeletd()
        //{
        //    MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
        //    _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _LoadSafetyActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _LoadSafetyActions.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _LoadSafetyActions.SqlStatement = " SELECT *, LEFT(HOD, CHARINDEX (':', HOD ) - 1) as MO, LEFT(RespPerson, CHARINDEX (':', RespPerson ) - 1) as RespPerson, isnull(compnotes, '') Compnotes1, \r\n" +
        //                                      " case when CompNotes <> '' then '\\\\10.148.225.119\\Mineware\\Images\\Preplanning\\Actions\\' + CompNotes + '.png' \r\n" +
        //                                      " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
        //                                      " where Type = 'PPS' and Completiondate is null and workplace in ('" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "','" + WP4Desc + "','" + WP5Desc + "')";

        //    _LoadSafetyActions.ResultsTableName = "Actions";
        //    _LoadSafetyActions.ExecuteInstruction();

        //    DataSet dsActions = new DataSet();
        //    dsActions.Tables.Clear();
        //    dsActions.Tables.Add(_LoadSafetyActions.ResultsDataTable);


        //    theReport.RegisterData(dsActions);

        //    theReport.Load(TGlobalItems.ReportsFolder + "\\GraphicsPrePlanningSafety.frx");

        //    theReport.SetParameterValue("Prodmonth", _ProdMonth);
        //    theReport.SetParameterValue("Section", _Section);
        //    theReport.SetParameterValue("Crew", _Crew);
        //    theReport.SetParameterValue("Activity", _Activity);

        //    //theReport.Design();

        //    pcReport.Clear();
        //    theReport.Prepare();
        //    theReport.Preview = pcReport;
        //    theReport.ShowPrepared();
        //}


        void loadImage(string _Department)
        {
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Images\\Preplanning" + "\\" + _Department + "   ");
            if (dir2.Exists)
            {
                IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                var files = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                foreach (var item in files)
                {
                    string ext = item.Name.Replace(System.IO.Path.GetFileNameWithoutExtension(item.Name), string.Empty);

                    if (item.ToString() == _Crew + _ProdMonth + ext)
                    {
                        _Attachment = RepDir + "\\Images\\Preplanning" + "\\" + _Department + "\\" + item.ToString();
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


        private void loadSafety()
        {

            //Safety Actions
            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsActions);

            //Cycles
            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
            _dbManCycle.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + _Crew + "' and pm = '" + _ProdMonth + "' ";

            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCycle.ResultsTableName = "Cycles";
            _dbManCycle.ExecuteInstruction();

            DataSet dsCycle = new DataSet();
            dsCycle.Tables.Clear();
            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

            theReport.RegisterData(dsCycle);

            //open actions
            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsOpenActions);

            //Auth Watermark
            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManParaAuth.SqlStatement = "select top (1) case when SafetyDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                     "where prodmonth = '" + _ProdMonth + "' and crew = '" + _Crew + "' ";

            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManParaAuth.ResultsTableName = "ParaAuth";
            _dbManParaAuth.ExecuteInstruction();

            Authorise = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

            DataSet dsParaAuth = new DataSet();
            dsParaAuth.Tables.Clear();
            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

            theReport.RegisterData(dsParaAuth);

            //LoadNotes
            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_SafetyCapt_Notes] where prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "' and crew = '" + _Crew + "' and notes is not null  ";
            _LoadSafetyNotes.ResultsTableName = "Notes";
            _LoadSafetyNotes.ExecuteInstruction();

            DataSet dsSafteyNotes = new DataSet();
            dsSafteyNotes.Tables.Clear();
            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

            theReport.RegisterData(dsSafteyNotes);

            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
            _dbAns.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbAns.SqlStatement = " select* from(  \r\n" +
                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy  \r\n" +
                                    " from tbl_PrePlanning_SafetyQuest q, tbl_PrePlanning_safety c, tbl_Workplace w \r\n" +
                                    " where q.QuestID = c.QuestID \r\n" +
                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                    " and c.prodmonth = '" + _ProdMonth + "' \r\n" +
                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                    " )a order by Workplace, Orderby \r\n";

            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAns.ResultsTableName = "Answers";
            _dbAns.ExecuteInstruction();

            DataSet dsAnss = new DataSet();
            dsAnss.Tables.Clear();
            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

            theReport.RegisterData(dsAnss);

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(_ProdMonth));
            string Date = ProductionGlobal.ProductionGlobal.Prod2;

            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
            {
                theReport.SetParameterValue("OpenCheck", "Y");
            }
            else
            {
                theReport.SetParameterValue("OpenCheck", "N");
            }

            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
            {
                if (_LoadSafetyActions.ResultsDataTable.Rows[0]["TheDate"].ToString() == "1/1/1900 12:00:00 AM")
                {
                    theReport.SetParameterValue("ActionsCheck", "N");
                }
                else
                {
                    theReport.SetParameterValue("ActionsCheck", "Y");
                }
            }
            else
            {
                theReport.SetParameterValue("ActionsCheck", "N");
            }

            //theReport.Load(TGlobalItems.ReportsFolder + "\\GraphicsPrePlanningSafety.frx");
            theReport.Load(ReportFolder + "GraphicsPrePlanningSafety.frx");

            theReport.SetParameterValue("Prodmonth", Date);
            theReport.SetParameterValue("Crew", _Crew);

            loadImage("Safety");
            theReport.SetParameterValue("Image", _Attachment);

            theReport.SetParameterValue("Auth", Authorise);
            theReport.SetParameterValue("Activity", _Activity);

            // theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void loadRockEng()
        {
            MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
            _dbManData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManData.SqlStatement = " select RockEngDepCapt,RockEngDepAuth,isnull(CalendarDate,' ') CalendarDate from tbl_PrePlanning_MonthPlan \r\n" +
                                      " where prodmonth = '" + _ProdMonth + "' \r\n" +
                                      " and SectionID = '" + _Section + "' \r\n" +
                                      " and Crew = '" + _Crew + "'";

            _dbManData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManData.ResultsTableName = "Captured";
            _dbManData.ExecuteInstruction();

            DataSet dsCaptured = new DataSet();
            dsCaptured.Tables.Clear();
            dsCaptured.Tables.Add(_dbManData.ResultsDataTable);

            //open actions
            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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
            _dbManSeismic.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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
            _LoadREActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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
            _dbManCycle.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + _Crew + "' and pm = '" + _ProdMonth + "' ";

            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCycle.ResultsTableName = "Cycles";
            _dbManCycle.ExecuteInstruction();

            DataSet dsCycle = new DataSet();
            dsCycle.Tables.Clear();
            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

            //RE Hazard Rating (6 month)
            MWDataManager.clsDataAccess _dbManREHR = new MWDataManager.clsDataAccess();
            _dbManREHR.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            if (_Activity == "Development")
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
            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManParaAuth.SqlStatement = "select top (1) case when RockEngDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                     "where prodmonth = '" + _ProdMonth + "' and crew = '" + _Crew + "' ";

            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManParaAuth.ResultsTableName = "ParaAuth";
            _dbManParaAuth.ExecuteInstruction();

            Authorise = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

            DataSet dsParaAuth = new DataSet();
            dsParaAuth.Tables.Clear();
            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

            //LoadNotes
            MWDataManager.clsDataAccess _LoadRENotes = new MWDataManager.clsDataAccess();
            _LoadRENotes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadRENotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadRENotes.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadRENotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_RockEngCapt_Notes] where prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "' and crew = '" + _Crew + "' and notes is not null  ";
            _LoadRENotes.ResultsTableName = "Notes";
            _LoadRENotes.ExecuteInstruction();

            DataSet dsRENotes = new DataSet();
            dsRENotes.Tables.Clear();
            dsRENotes.Tables.Add(_LoadRENotes.ResultsDataTable);

            //Underground Re Compliance 
            MWDataManager.clsDataAccess _dbManSR = new MWDataManager.clsDataAccess();
            _dbManSR.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            if (_Activity == "Development")
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
            _dbAns.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (_Activity == "Development")
            {
                _dbAns.SqlStatement = " select* from(  \r\n" +
                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, q.QuestionSubCat, \r\n" +
                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                    " from tbl_PrePlanning_RockEngQuest_Dev q, tbl_PrePlanning_RockEng c, tbl_Workplace w \r\n" +
                                    " where q.QuestID = c.QuestID \r\n" +
                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                    " and c.prodmonth = '" + _ProdMonth + "' \r\n" +
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
                                    " and c.prodmonth = '" + _ProdMonth + "' \r\n" +
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

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(_ProdMonth));
            string Date = ProductionGlobal.ProductionGlobal.Prod2;

            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
            {
                theReport.SetParameterValue("OpenCheck", "Y");
            }
            else
            {
                theReport.SetParameterValue("OpenCheck", "N");
            }

            if (_LoadREActions.ResultsDataTable.Rows.Count > 0)
            {
                if (_LoadREActions.ResultsDataTable.Rows.Count > 1)
                {
                    theReport.SetParameterValue("ActionsCheck", "Y");
                }
                else
                {
                    theReport.SetParameterValue("ActionsCheck", "N");
                }
            }
            else
            {
                theReport.SetParameterValue("ActionsCheck", "N");
            }

            theReport.RegisterData(dsOpenActions);
            theReport.RegisterData(dsCycle);
            theReport.RegisterData(dsParaAuth);
            theReport.RegisterData(dsActions);
            theReport.RegisterData(dsSeismic);
            theReport.RegisterData(dsREHR);
            theReport.RegisterData(dsRENotes);
            theReport.RegisterData(dsSR);
            theReport.RegisterData(dsAnss);
            theReport.RegisterData(dsCaptured);

            if (_Activity == "Development")
            {
                //theReport.Load(TGlobalItems.ReportsFolder + "\\PrePlanExeNoteRockEngDev.frx");
                theReport.Load(ReportFolder + "PrePlanExeNoteRockEngStoping.frx");
            }
            else
            {
                //theReport.Load(TGlobalItems.ReportsFolder + "\\PrePlanExeNoteRockEngStoping.frx");
                theReport.Load(ReportFolder + "PrePlanExeNoteRockEngStoping.frx");
            }
            theReport.SetParameterValue("Prodmonth", Date);
            theReport.SetParameterValue("Crew", _Crew);

            loadImage("RockEngineering");

            theReport.SetParameterValue("Auth", Authorise);

            theReport.SetParameterValue("Banner", ProductionGlobal.ProductionGlobal.Banner);
            theReport.SetParameterValue("Activity", _Activity);
            theReport.SetParameterValue("Image", _Attachment);
            theReport.SetParameterValue("WP1Desc", WP1Desc);
            theReport.SetParameterValue("WP2Desc", WP2Desc);
            theReport.SetParameterValue("WP3Desc", WP3Desc);
            theReport.SetParameterValue("WP4Desc", WP4Desc);
            theReport.SetParameterValue("WP5Desc", WP5Desc);

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void loadVent()
        {
            //Safety Actions
            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsActions);

            //Cycles
            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
            _dbManCycle.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + _Crew + "' and pm = '" + _ProdMonth + "' ";

            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCycle.ResultsTableName = "Cycles";
            _dbManCycle.ExecuteInstruction();

            DataSet dsCycle = new DataSet();
            dsCycle.Tables.Clear();
            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

            theReport.RegisterData(dsCycle);

            //open actions
            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsOpenActions);

            //Auth Watermark
            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManParaAuth.SqlStatement = "select top (1) case when VentDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                     "where prodmonth = '" + _ProdMonth + "' and crew = '" + _Crew + "' ";

            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManParaAuth.ResultsTableName = "ParaAuth";
            _dbManParaAuth.ExecuteInstruction();

            Authorise = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

            DataSet dsParaAuth = new DataSet();
            dsParaAuth.Tables.Clear();
            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

            theReport.RegisterData(dsParaAuth);

            //LoadNotes
            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_VentCapture_Notes] where prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "' and crew = '" + _Crew + "' and notes is not null  ";
            _LoadSafetyNotes.ResultsTableName = "Notes";
            _LoadSafetyNotes.ExecuteInstruction();

            DataSet dsSafteyNotes = new DataSet();
            dsSafteyNotes.Tables.Clear();
            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

            theReport.RegisterData(dsSafteyNotes);

            ///Graphs
            MWDataManager.clsDataAccess _dbMandataGraphWP1Temp = new MWDataManager.clsDataAccess();
            _dbMandataGraphWP1Temp.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsGraphWp1Temp);

            MWDataManager.clsDataAccess _dbMandataGraphWP1Vel = new MWDataManager.clsDataAccess();
            _dbMandataGraphWP1Vel.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsGraphWp1Vel);

            MWDataManager.clsDataAccess _dbMandataGraphWP1AU = new MWDataManager.clsDataAccess();
            _dbMandataGraphWP1AU.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsGraphWp1AU);

            MWDataManager.clsDataAccess _dbMandataGraphWP1SCP = new MWDataManager.clsDataAccess();
            _dbMandataGraphWP1SCP.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsGraphWp1SCP);

            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
            _dbAns.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbAns.SqlStatement = " select* from(  \r\n" +
                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                    " from tbl_PrePlanning_VentQuest q, tbl_PrePlanning_Vent c, tbl_Workplace w \r\n" +
                                    " where q.QuestID = c.QuestID \r\n" +
                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                    " and c.prodmonth = '" + _ProdMonth + "' \r\n" +
                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                    " )a order by Workplace, OrderBy \r\n";

            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAns.ResultsTableName = "Answers";
            _dbAns.ExecuteInstruction();

            DataSet dsAnss = new DataSet();
            dsAnss.Tables.Clear();
            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

            theReport.RegisterData(dsAnss);

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(_ProdMonth));
            string Date = ProductionGlobal.ProductionGlobal.Prod2;


            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
            {
                if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 1)
                {
                    theReport.SetParameterValue("ActionsCheck", "Y");
                }
                else
                {
                    theReport.SetParameterValue("ActionsCheck", "N");
                }
            }
            else
            {
                theReport.SetParameterValue("ActionsCheck", "N");
            }

            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
            {
                theReport.SetParameterValue("OpenCheck", "Y");
            }
            else
            {
                theReport.SetParameterValue("OpenCheck", "N");
            }

            //theReport.Load(TGlobalItems.ReportsFolder + "\\PrePlanExeNoteVent.frx");
            theReport.Load(ReportFolder + "PrePlanExeNoteVent.frx");

            theReport.SetParameterValue("Banner", ProductionGlobal.ProductionGlobal.Banner);
            theReport.SetParameterValue("Activity", _Activity);
            theReport.SetParameterValue("Prodmonth", Date);
            theReport.SetParameterValue("Crew", _Crew);
            theReport.SetParameterValue("WP1Desc", WP1Desc);
            theReport.SetParameterValue("WP2Desc", WP2Desc);
            theReport.SetParameterValue("WP3Desc", WP3Desc);
            theReport.SetParameterValue("WP4Desc", WP4Desc);
            theReport.SetParameterValue("WP5Desc", WP5Desc);


            loadImage("Vent");
            theReport.SetParameterValue("Image", _Attachment);
            theReport.SetParameterValue("Auth", Authorise);

            //theReport.Design(); 

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void loadHR()
        {
            //Safety Actions
            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadSafetyActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadSafetyActions.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadSafetyActions.SqlStatement = " SELECT *, LEFT(HOD, CHARINDEX (':', HOD ) - 1) as MO, LEFT(RespPerson, CHARINDEX (':', RespPerson ) - 1) as RespPerson, isnull(compnotes, '') Compnotes1, \r\n" +
                                              " case when CompNotes <> '' then '" + ImageFolder + "\\Preplanning\\HR\\' + CompNotes + '.png' \r\n" +
                                              " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
                                              " where Type = 'PPHR' and Completiondate is null and workplace in ('" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "','" + WP4Desc + "','" + WP5Desc + "')";
            _LoadSafetyActions.ResultsTableName = "Actions";
            _LoadSafetyActions.ExecuteInstruction();

            DataSet dsActions = new DataSet();
            dsActions.Tables.Clear();
            dsActions.Tables.Add(_LoadSafetyActions.ResultsDataTable);

            theReport.RegisterData(dsActions);

            //Cycles
            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
            _dbManCycle.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + _Crew + "' and Prodmonth = '" + _ProdMonth + "' ";

            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCycle.ResultsTableName = "Cycles";
            _dbManCycle.ExecuteInstruction();

            DataSet dsCycle = new DataSet();
            dsCycle.Tables.Clear();
            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

            theReport.RegisterData(dsCycle);

            //open actions
            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsOpenActions);

            //Auth Watermark
            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManParaAuth.SqlStatement = "select top (1) case when HRDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                     "where prodmonth = '" + _ProdMonth + "' and crew = '" + _Crew + "' ";

            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManParaAuth.ResultsTableName = "ParaAuth";
            _dbManParaAuth.ExecuteInstruction();

            Authorise = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

            DataSet dsParaAuth = new DataSet();
            dsParaAuth.Tables.Clear();
            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

            theReport.RegisterData(dsParaAuth);

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(_ProdMonth));
            string Date = ProductionGlobal.ProductionGlobal.Prod2;

            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
            {
                theReport.SetParameterValue("OpenCheck", "Y");
            }
            else
            {
                theReport.SetParameterValue("OpenCheck", "N");
            }

            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
            {
                if (_LoadSafetyActions.ResultsDataTable.Rows[0]["TheDate"].ToString() == "1/1/1900 12:00:00 AM")
                {
                    theReport.SetParameterValue("ActionsCheck", "N");
                }
                else
                {
                    theReport.SetParameterValue("ActionsCheck", "Y");
                }
            }
            else
            {
                theReport.SetParameterValue("ActionsCheck", "N");
            }

            //theReport.Load("PrePlanExeNoteHR.frx");
            //theReport.Load(TGlobalItems.ReportsFolder + "\\GraphicsPrePlanningHR.frx");
            theReport.Load(ReportFolder + "GraphicsPrePlanningHR.frx");

            theReport.SetParameterValue("Prodmonth", Date);
            theReport.SetParameterValue("Crew", _Crew);

            loadImage("HR");
            theReport.SetParameterValue("Image", _Attachment);

            theReport.SetParameterValue("Auth", Authorise);

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        private void loadSurvey()
        {
            //Safety Actions
            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsActions);

            //Cycles
            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
            _dbManCycle.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + _Crew + "' and pm = '" + _ProdMonth + "' ";

            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCycle.ResultsTableName = "Cycles";
            _dbManCycle.ExecuteInstruction();

            DataSet dsCycle = new DataSet();
            dsCycle.Tables.Clear();
            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

            theReport.RegisterData(dsCycle);

            //open actions
            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsOpenActions);

            //Auth Watermark
            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManParaAuth.SqlStatement = "select top (1) case when VentDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                     "where prodmonth = '" + _ProdMonth + "' and crew = '" + _Crew + "' ";

            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManParaAuth.ResultsTableName = "ParaAuth";
            _dbManParaAuth.ExecuteInstruction();

            Authorise = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

            DataSet dsParaAuth = new DataSet();
            dsParaAuth.Tables.Clear();
            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

            theReport.RegisterData(dsParaAuth);

            //LoadNotes
            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_SurveyCapt_Notes] where prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "' and crew = '" + _Crew + "' and notes is not null  ";
            _LoadSafetyNotes.ResultsTableName = "Notes";
            _LoadSafetyNotes.ExecuteInstruction();

            DataSet dsSafteyNotes = new DataSet();
            dsSafteyNotes.Tables.Clear();
            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

            theReport.RegisterData(dsSafteyNotes);

            ///Questions and Answers////
            ///////2020-04-07///////////      

            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
            _dbAns.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (_Activity == "Development")
            {
                _dbAns.SqlStatement = " select* from(  \r\n" +
                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                    " from tbl_PrePlanning_SurveyQuest_Dev q, tbl_PrePlanning_Survey c, tbl_Workplace w \r\n" +
                                    " where q.QuestID = c.QuestID \r\n" +
                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                    " and c.prodmonth = '" + _ProdMonth + "' \r\n" +
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
                                    " and c.prodmonth = '" + _ProdMonth + "' \r\n" +
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

            theReport.RegisterData(dsAnss);

            //CaptureInfo
            MWDataManager.clsDataAccess _dbManCapt = new MWDataManager.clsDataAccess();
            _dbManCapt.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            if (_Activity == "Development")
            {
                _dbManCapt.SqlStatement = " Select qst.question,WP1,WP2,WP3,WP4,WP5 from tbl_PrePlanning_SurveyQuest_Dev Qst \r\n" +
                                        "left outer join ( \r\n" +
                                        "Select questid,answer WP1 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
                                        "and workplace = '" + WP1 + "' \r\n" +
                                        " ) fstWp on qst.questid = fstWp.questid \r\n" +

                                        " left outer join ( \r\n" +
                                        "Select questid,answer WP2 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
                                        "and workplace = '" + WP2 + "' \r\n" +
                                        " ) secWp on qst.questid = secWp.questid \r\n" +


                                        "  left outer join ( \r\n" +
                                        "Select questid,answer WP3 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
                                        "and workplace = '" + WP3 + "' \r\n" +
                                        " ) thrdWp on qst.questid = thrdWp.questid \r\n" +

                                        " left outer join ( \r\n" +
                                        "Select questid,answer WP4 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
                                        "and workplace = '" + WP4 + "' \r\n" +
                                        " ) frthWp on qst.questid = frthWp.questid \r\n" +

                                        "  left outer join ( \r\n" +
                                        "Select questid,answer WP5 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
                                        "and workplace = '" + WP5 + "' \r\n" +
                                        " ) ftfhWp on qst.questid = ftfhWp.questid \r\n" +

                                        " ";
            }
            else
            {
                _dbManCapt.SqlStatement = " Select qst.question,WP1,WP2,WP3,WP4,WP5 from tbl_PrePlanning_SurveyQuest Qst \r\n" +

                                        "left outer join ( \r\n" +
                                        "Select questid,answer WP1 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
                                        "and workplace = '" + WP1 + "' \r\n" +
                                        " ) fstWp on qst.questid = fstWp.questid \r\n" +

                                        " left outer join ( \r\n" +
                                        "Select questid,answer WP2 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
                                        "and workplace = '" + WP2 + "' \r\n" +
                                        " ) secWp on qst.questid = secWp.questid \r\n" +


                                        "  left outer join ( \r\n" +
                                        "Select questid,answer WP3 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
                                        "and workplace = '" + WP3 + "' \r\n" +
                                        " ) thrdWp on qst.questid = thrdWp.questid \r\n" +

                                        " left outer join ( \r\n" +
                                        "Select questid,answer WP4 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
                                        "and workplace = '" + WP4 + "' \r\n" +
                                        " ) frthWp on qst.questid = frthWp.questid \r\n" +

                                        "  left outer join ( \r\n" +
                                        "Select questid,answer WP5 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "'  \r\n" +
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

            theReport.RegisterData(dsCapt);

            //Complaince Check this query
            MWDataManager.clsDataAccess _dbManComp = new MWDataManager.clsDataAccess();
            _dbManComp.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            if (_Activity == "Development")
            {
                _dbManComp.SqlStatement = "Select substring(convert(varchar(11),convert(date,convert(varchar(10),a.Prodmonth)+'01'),106),4,88) Prodlbl,* from ( \r\n" +
                                        "Select a.prodmonth, WP1, case when a.bsqm is null then 0 when a.PSqm = 0 then 0  else convert(decimal(8,0), a.Bsqm/a.PSqm * 100) end as Compliance1 \r\n" +
                                        ",'a' a \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.Description WP1,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "' \r\n" +
                                        "and p.workplaceid = '" + WP1 + "' \r\n" +
                                        "group by  prodmonth,w.Description ) a  )a \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP2, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance2  \r\n" +
                                        ",'a' b \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.description WP2,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "'\r\n" +
                                        "and p.workplaceid = '" + WP2 + "' \r\n" +
                                        "group by  prodmonth,w.Description ) a ) b on a.a = b.b and a.prodmonth = b.prodmonth \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP3, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance3  \r\n" +
                                        ",'a' c \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.description WP3,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "'  \r\n" +
                                        "and p.workplaceid = '" + WP3 + "' \r\n" +
                                        "group by  prodmonth,w.Description )a ) c on a.a = c.c and a.prodmonth = c.prodmonth \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP4, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance4  \r\n" +
                                        ",'a' d \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.Description WP4,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "' \r\n" +
                                        "and p.workplaceid = '" + WP4 + "' \r\n" +
                                        "group by  prodmonth,w.Description )a ) d on a.a = d.d and a.prodmonth = d.prodmonth \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP5, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance5  \r\n" +
                                        ",'a' e \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.description WP5,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "'  \r\n" +
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
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "' \r\n" +
                                        "and p.workplaceid = '" + WP1 + "' \r\n" +
                                        "group by  prodmonth,w.Description ) a  )a \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP2, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance2  \r\n" +
                                        ",'a' b \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.description WP2,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "'\r\n" +
                                        "and p.workplaceid = '" + WP2 + "' \r\n" +
                                        "group by  prodmonth,w.Description ) a ) b on a.a = b.b and a.prodmonth = b.prodmonth \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP3, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance3  \r\n" +
                                        ",'a' c \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.description WP3,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "'  \r\n" +
                                        "and p.workplaceid = '" + WP3 + "' \r\n" +
                                        "group by  prodmonth,w.Description )a ) c on a.a = c.c and a.prodmonth = c.prodmonth \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP4, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance4  \r\n" +
                                        ",'a' d \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.Description WP4,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "' \r\n" +
                                        "and p.workplaceid = '" + WP4 + "' \r\n" +
                                        "group by  prodmonth,w.Description )a ) d on a.a = d.d and a.prodmonth = d.prodmonth \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP5, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance5  \r\n" +
                                        ",'a' e \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.description WP5,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_Planning p,tbl_Workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_ProdMonth) - 3) + "'  \r\n" +
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

            theReport.RegisterData(dsComp);


            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(_ProdMonth));
            string Date = ProductionGlobal.ProductionGlobal.Prod2;

            if (_Activity == "Development")
            {
                //theReport.Load(TGlobalItems.ReportsFolder + "\\PrePlanExeNoteSurvey.frx");
                theReport.Load(ReportFolder + "PrePlanExeNoteSurvey.frx");

            }
            else
            {
                //theReport.Load(TGlobalItems.ReportsFolder + "\\PrePlanExeNoteSurvey.frx");
                theReport.Load(ReportFolder + "PrePlanExeNoteSurvey.frx");

            }

            theReport.SetParameterValue("Prodmonth", Date);
            theReport.SetParameterValue("Crew", _Crew);

            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
            {
                theReport.SetParameterValue("OpenCheck", "Y");
            }
            else
            {
                theReport.SetParameterValue("OpenCheck", "N");
            }

            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
            {
                if (_LoadSafetyActions.ResultsDataTable.Rows[0]["TheDate"].ToString() == "1/1/1900 12:00:00 AM")
                {
                    theReport.SetParameterValue("ActionsCheck", "N");
                }
                else
                {
                    theReport.SetParameterValue("ActionsCheck", "Y");
                }
            }
            else
            {
                theReport.SetParameterValue("ActionsCheck", "N");
            }

            loadImage("Survey");

            theReport.SetParameterValue("Auth", Authorise);

            theReport.SetParameterValue("Banner", ProductionGlobal.ProductionGlobal.Banner);
            theReport.SetParameterValue("Activity", _Activity);
            theReport.SetParameterValue("Image", _Attachment);
            theReport.SetParameterValue("WP1Desc", WP1Desc);
            theReport.SetParameterValue("WP2Desc", WP2Desc);
            theReport.SetParameterValue("WP3Desc", WP3Desc);
            theReport.SetParameterValue("WP4Desc", WP4Desc);
            theReport.SetParameterValue("WP5Desc", WP5Desc);


            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void loadGeology()
        {


            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsActions);

            //Cycles
            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
            _dbManCycle.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + _Crew + "' and Prodmonth = '" + _ProdMonth + "' ";

            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCycle.ResultsTableName = "Cycles";
            _dbManCycle.ExecuteInstruction();

            DataSet dsCycle = new DataSet();
            dsCycle.Tables.Clear();
            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

            theReport.RegisterData(dsCycle);

            //open actions
            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsOpenActions);

            //Auth Watermark
            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManParaAuth.SqlStatement = "select top (1) case when GeologyDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                     "where prodmonth = '" + _ProdMonth + "' and crew = '" + _Crew + "' ";

            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManParaAuth.ResultsTableName = "ParaAuth";
            _dbManParaAuth.ExecuteInstruction();

            Authorise = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

            DataSet dsParaAuth = new DataSet();
            dsParaAuth.Tables.Clear();
            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

            theReport.RegisterData(dsParaAuth);

            //LoadNotes
            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_GeologyCapture_Notes] where prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "' and crew = '" + _Crew + "' and notes is not null  ";
            _LoadSafetyNotes.ResultsTableName = "Notes";
            _LoadSafetyNotes.ExecuteInstruction();

            DataSet dsSafteyNotes = new DataSet();
            dsSafteyNotes.Tables.Clear();
            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

            theReport.RegisterData(dsSafteyNotes);

            MWDataManager.clsDataAccess _dbMansw1 = new MWDataManager.clsDataAccess();
            _dbMansw1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMansw1.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP1 + "' order by Prodmonth asc";

            _dbMansw1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMansw1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMansw1.ResultsTableName = "sw1";
            _dbMansw1.ExecuteInstruction();

            DataSet dssw1 = new DataSet();
            dssw1.Tables.Clear();
            dssw1.Tables.Add(_dbMansw1.ResultsDataTable);

            theReport.RegisterData(dssw1);

            MWDataManager.clsDataAccess _dbMancmgt1 = new MWDataManager.clsDataAccess();
            _dbMancmgt1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMancmgt1.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP1 + "' order by Prodmonth asc";

            _dbMancmgt1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMancmgt1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMancmgt1.ResultsTableName = "cmgt1";
            _dbMancmgt1.ExecuteInstruction();

            DataSet dscmgt1 = new DataSet();
            dscmgt1.Tables.Clear();
            dscmgt1.Tables.Add(_dbMancmgt1.ResultsDataTable);

            theReport.RegisterData(dscmgt1);

            MWDataManager.clsDataAccess _dbMansw2 = new MWDataManager.clsDataAccess();
            _dbMansw2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMansw2.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP2 + "' order by Prodmonth asc";

            _dbMansw2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMansw2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMansw2.ResultsTableName = "sw2";
            _dbMansw2.ExecuteInstruction();

            DataSet dssw2 = new DataSet();
            dssw2.Tables.Clear();
            dssw2.Tables.Add(_dbMansw2.ResultsDataTable);

            theReport.RegisterData(dssw2);

            MWDataManager.clsDataAccess _dbMancmgt2 = new MWDataManager.clsDataAccess();
            _dbMancmgt2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMancmgt2.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP2 + "' order by Prodmonth asc";

            _dbMancmgt2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMancmgt2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMancmgt2.ResultsTableName = "cmgt2";
            _dbMancmgt2.ExecuteInstruction();

            DataSet dscmgt2 = new DataSet();
            dscmgt2.Tables.Clear();
            dscmgt2.Tables.Add(_dbMancmgt2.ResultsDataTable);

            theReport.RegisterData(dscmgt2);

            MWDataManager.clsDataAccess _dbMansw3 = new MWDataManager.clsDataAccess();
            _dbMansw3.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMansw3.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP3 + "' order by Prodmonth asc";

            _dbMansw3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMansw3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMansw3.ResultsTableName = "sw3";
            _dbMansw3.ExecuteInstruction();

            DataSet dssw3 = new DataSet();
            dssw3.Tables.Clear();
            dssw3.Tables.Add(_dbMansw3.ResultsDataTable);

            theReport.RegisterData(dssw3);

            MWDataManager.clsDataAccess _dbMancmgt3 = new MWDataManager.clsDataAccess();
            _dbMancmgt3.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMancmgt3.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP3 + "' order by Prodmonth asc";

            _dbMancmgt3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMancmgt3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMancmgt3.ResultsTableName = "cmgt3";
            _dbMancmgt3.ExecuteInstruction();

            DataSet dscmgt3 = new DataSet();
            dscmgt3.Tables.Clear();
            dscmgt3.Tables.Add(_dbMancmgt3.ResultsDataTable);

            theReport.RegisterData(dscmgt3);

            MWDataManager.clsDataAccess _dbMansw4 = new MWDataManager.clsDataAccess();
            _dbMansw4.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMansw4.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP4 + "' order by Prodmonth asc";

            _dbMansw4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMansw4.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMansw4.ResultsTableName = "sw4";
            _dbMansw4.ExecuteInstruction();

            DataSet dssw4 = new DataSet();
            dssw4.Tables.Clear();
            dssw4.Tables.Add(_dbMansw4.ResultsDataTable);

            theReport.RegisterData(dssw4);

            MWDataManager.clsDataAccess _dbMancmgt4 = new MWDataManager.clsDataAccess();
            _dbMancmgt4.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMancmgt4.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP4 + "' order by Prodmonth asc";

            _dbMancmgt4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMancmgt4.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMancmgt4.ResultsTableName = "cmgt4";
            _dbMancmgt4.ExecuteInstruction();

            DataSet dscmgt4 = new DataSet();
            dscmgt4.Tables.Clear();
            dscmgt4.Tables.Add(_dbMancmgt4.ResultsDataTable);

            theReport.RegisterData(dscmgt4);

            MWDataManager.clsDataAccess _dbMansw5 = new MWDataManager.clsDataAccess();
            _dbMansw5.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMansw5.SqlStatement = " select prodmonth,SW from tbl_planmonth where workplaceid = '" + WP5 + "' order by Prodmonth asc";

            _dbMansw5.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMansw5.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMansw5.ResultsTableName = "sw5";
            _dbMansw5.ExecuteInstruction();

            DataSet dssw5 = new DataSet();
            dssw5.Tables.Clear();
            dssw5.Tables.Add(_dbMansw5.ResultsDataTable);

            theReport.RegisterData(dssw5);

            MWDataManager.clsDataAccess _dbMancmgt5 = new MWDataManager.clsDataAccess();
            _dbMancmgt5.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMancmgt5.SqlStatement = " select prodmonth,cmgt from tbl_planmonth where workplaceid = '" + WP5 + "' order by Prodmonth asc";

            _dbMancmgt5.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMancmgt5.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMancmgt5.ResultsTableName = "cmgt5";
            _dbMancmgt5.ExecuteInstruction();

            DataSet dscmgt5 = new DataSet();
            dscmgt5.Tables.Clear();
            dscmgt5.Tables.Add(_dbMancmgt5.ResultsDataTable);

            theReport.RegisterData(dscmgt5);

            //CMGT
            MWDataManager.clsDataAccess _dbManCMGT = new MWDataManager.clsDataAccess();
            _dbManCMGT.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCMGT.SqlStatement = "exec sp_PrePlanning_GeoGraphs '" + _ProdMonth + "','" + _Section + "','" + WP1 + "','" + WP2 + "','" + WP3 + "','" + WP4 + "','" + WP5 + "' ";

            _dbManCMGT.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCMGT.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCMGT.ResultsTableName = "Graphs";
            _dbManCMGT.ExecuteInstruction();

            DataSet dsCMGTNEW = new DataSet();
            dsCMGTNEW.Tables.Clear();
            dsCMGTNEW.Tables.Add(_dbManCMGT.ResultsDataTable);

            theReport.RegisterData(dsCMGTNEW);

            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
            _dbAns.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbAns.SqlStatement = " select* from(  \r\n" +
                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                    " from tbl_PrePlanning_GeologyQuest q, tbl_PrePlanning_Geology c, tbl_Workplace w \r\n" +
                                    " where q.QuestID = c.QuestID \r\n" +
                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                    " and c.prodmonth = '" + _ProdMonth + "' \r\n" +
                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                    " )a order by Workplace, OrderBy \r\n";

            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAns.ResultsTableName = "Answers";
            _dbAns.ExecuteInstruction();

            DataSet dsAnss = new DataSet();
            dsAnss.Tables.Clear();
            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

            theReport.RegisterData(dsAnss);

            //theReport.Load(TGlobalItems.ReportsFolder + "\\PrePlanGeoInsp.frx");
            theReport.Load(ReportFolder + "PrePlanGeoInsp.frx");

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(_ProdMonth));
            string Date = ProductionGlobal.ProductionGlobal.Prod2;


            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
            {
                theReport.SetParameterValue("OpenCheck", "Y");
            }
            else
            {
                theReport.SetParameterValue("OpenCheck", "N");
            }

            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
            {
                if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 1)
                {
                    theReport.SetParameterValue("ActionsCheck", "Y");
                }
                else
                {
                    theReport.SetParameterValue("ActionsCheck", "N");
                }
            }
            else
            {
                theReport.SetParameterValue("ActionsCheck", "N");
            }


            theReport.SetParameterValue("Prodmonth", Date);
            theReport.SetParameterValue("Crew", _Crew);

            loadImage("Geology");
            theReport.SetParameterValue("Image", _Attachment);

            theReport.SetParameterValue("Auth", Authorise);

            theReport.SetParameterValue("WP1Desc", WP1Desc);
            theReport.SetParameterValue("WP2Desc", WP2Desc);
            theReport.SetParameterValue("WP3Desc", WP3Desc);
            theReport.SetParameterValue("WP4Desc", WP4Desc);
            theReport.SetParameterValue("WP5Desc", WP5Desc);
            theReport.SetParameterValue("Activity", _Activity);
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void loadEngineering()
        {
            //Safety Actions
            MWDataManager.clsDataAccess _LoadSafetyActions = new MWDataManager.clsDataAccess();
            _LoadSafetyActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsActions);

            //Cycles
            MWDataManager.clsDataAccess _dbManCycle = new MWDataManager.clsDataAccess();
            _dbManCycle.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCycle.SqlStatement = " select * from vw_Preplanning_Cycle where orgunit = '" + _Crew + "' and Prodmonth = '" + _ProdMonth + "' ";

            _dbManCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCycle.ResultsTableName = "Cycles";
            _dbManCycle.ExecuteInstruction();

            DataSet dsCycle = new DataSet();
            dsCycle.Tables.Clear();
            dsCycle.Tables.Add(_dbManCycle.ResultsDataTable);

            theReport.RegisterData(dsCycle);

            //open actions
            MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
            _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

            theReport.RegisterData(dsOpenActions);

            //Auth Watermark
            MWDataManager.clsDataAccess _dbManParaAuth = new MWDataManager.clsDataAccess();
            _dbManParaAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManParaAuth.SqlStatement = "select top (1) case when EngineeringDepAuth = '' then 'Unauthorise' else 'Authorise' end as Auth from tbl_PrePlanning_MonthPlan \r\n" +
                                     "where prodmonth = '" + _ProdMonth + "' and crew = '" + _Crew + "' ";

            _dbManParaAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManParaAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManParaAuth.ResultsTableName = "ParaAuth";
            _dbManParaAuth.ExecuteInstruction();

            Authorise = _dbManParaAuth.ResultsDataTable.Rows[0][0].ToString();

            DataSet dsParaAuth = new DataSet();
            dsParaAuth.Tables.Clear();
            dsParaAuth.Tables.Add(_dbManParaAuth.ResultsDataTable);

            theReport.RegisterData(dsParaAuth);

            //LoadNotes
            MWDataManager.clsDataAccess _LoadSafetyNotes = new MWDataManager.clsDataAccess();
            _LoadSafetyNotes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadSafetyNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadSafetyNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadSafetyNotes.SqlStatement = " select top(1) Notes from [dbo].[tbl_PrePlanning_EngineeringCapture_Notes] where prodmonth = '" + _ProdMonth + "' and section = '" + _Section + "' and crew = '" + _Crew + "' and notes is not null  ";
            _LoadSafetyNotes.ResultsTableName = "Notes";
            _LoadSafetyNotes.ExecuteInstruction();

            DataSet dsSafteyNotes = new DataSet();
            dsSafteyNotes.Tables.Clear();
            dsSafteyNotes.Tables.Add(_LoadSafetyNotes.ResultsDataTable);

            theReport.RegisterData(dsSafteyNotes);

            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
            _dbAns.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbAns.SqlStatement = " select* from(  \r\n" +
                                    " select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                    " Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                    " from tbl_PrePlanning_EngineeringQuest q, tbl_PrePlanning_Engineering c, tbl_Workplace w  \r\n" +
                                    " where q.QuestID = c.QuestID \r\n" +
                                    " and w.WorkplaceID = c.Workplace \r\n" +
                                    " and c.prodmonth = '" + _ProdMonth + "' \r\n" +
                                    " and w.Description in ('" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "') \r\n" +
                                    " )a order by Workplace, OrderBy \r\n";

            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAns.ResultsTableName = "Answers";
            _dbAns.ExecuteInstruction();

            DataSet dsAnss = new DataSet();
            dsAnss.Tables.Clear();
            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

            theReport.RegisterData(dsAnss);


            //theReport.Load(TGlobalItems.ReportsFolder + "\\PrePlanExeNoteEng.frx");
            theReport.Load(ReportFolder + "PrePlanExeNoteEng.frx");

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(_ProdMonth));
            string Date = ProductionGlobal.ProductionGlobal.Prod2;

            if (_dbManOpenActions.ResultsDataTable.Rows.Count > 1)
            {
                theReport.SetParameterValue("OpenCheck", "Y");
            }
            else
            {
                theReport.SetParameterValue("OpenCheck", "N");
            }

            if (_LoadSafetyActions.ResultsDataTable.Rows.Count > 0)
            {
                if (_LoadSafetyActions.ResultsDataTable.Rows[0]["TheDate"].ToString() == "1/1/1900 12:00:00 AM")
                {
                    theReport.SetParameterValue("ActionsCheck", "N");
                }
                else
                {
                    theReport.SetParameterValue("ActionsCheck", "Y");
                }
            }
            else
            {
                theReport.SetParameterValue("ActionsCheck", "N");
            }

            theReport.SetParameterValue("Prodmonth", Date);
            theReport.SetParameterValue("Crew", _Crew);

            loadImage("Engineering");
            theReport.SetParameterValue("Image", _Attachment);

            theReport.SetParameterValue("Activity", _Activity);
            theReport.SetParameterValue("Auth", Authorise);

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }


        private void barDepartment_EditValueChanged(object sender, EventArgs e)
        {
            if (barDepartment.EditValue.ToString() == "Safety")
            {
                loadSafety();
            }

            if (barDepartment.EditValue.ToString() == "Rock Engineering")
            {
                loadRockEng();
            }
            if (barDepartment.EditValue.ToString() == "Ventilation")
            {
                loadVent();
            }
            if (barDepartment.EditValue.ToString() == "HR")
            {
                loadHR();
            }
            if (barDepartment.EditValue.ToString() == "Survey")
            {
                loadSurvey();
            }
            if (barDepartment.EditValue.ToString() == "Geology")
            {
                loadGeology();
            }
            if (barDepartment.EditValue.ToString() == "Engineering")
            {
                loadEngineering();
            }
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

       

        private void tileBarItem4_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadRockEng();
        }

        private void tileBarItem1_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadSafety();
        }

        private void pcReport_Load(object sender, EventArgs e)
        {

        }

        private void tileBarItem2_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadGeology();
        }

        private void tileBarItem3_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadHR();
        }

        private void tileBarItem5_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadVent();
        }

        private void tileBarItem6_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadSurvey();
        }

        private void tileBarItem9_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadSafety();
        }

        private void tileBarItem10_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadGeology();
        }

        private void tileBarItem11_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadHR();
        }

        private void tileBarItem12_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadRockEng();
        }

        private void tileBarItem13_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadVent();
        }

        private void tileBarItem14_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadSurvey();
        }

        private void tileBarItem15_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            loadEngineering();
        }

        private void tileBar2_Click(object sender, EventArgs e)
        {

        }

        private void tileSummary_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            LoadSummary();
        }

        private void LoadSummary()
        {
            MWDataManager.clsDataAccess _dbAns = new MWDataManager.clsDataAccess();
            _dbAns.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbAns.SqlStatement = " exec sp_PrePlanning_SummaryReport '" + _ProdMonth + "', '" + WP1Desc + "' , '" + WP2Desc + "' , '" + WP3Desc + "' , '" + WP4Desc + "' ,'" + WP5Desc + "' \r\n";
             
            _dbAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAns.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAns.ResultsTableName = "SummaryData";
            _dbAns.ExecuteInstruction();

            DataSet dsAnss = new DataSet();
            dsAnss.Tables.Clear();
            dsAnss.Tables.Add(_dbAns.ResultsDataTable);

            theReport.RegisterData(dsAnss);
            
            theReport.Load(ReportFolder + "PrePlanExeNoteSummary.frx");

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(_ProdMonth));
            string Date = ProductionGlobal.ProductionGlobal.Prod2;
            

            theReport.SetParameterValue("Prodmonth", _ProdMonth);
            theReport.SetParameterValue("Crew", _Crew);            

            theReport.SetParameterValue("Activity", _Activity);
            

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

    }
}
