using FastReport;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Planning
{
    public partial class frmPreplanningSummary : DevExpress.XtraEditors.XtraForm
    {
        private string ReportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

        public string _Prodmonth;
        public string _Mosect;
        public string _systemDBTag;
        public string _UserCurrentInfoConnection;
        public string _Report;
        public string _WP1Desc;
        public string _WP2Desc;
        public string _WP3Desc;
        public string _WP4Desc;
        public string _WP5Desc;

        public string _WP1;
        public string _WP2;
        public string _WP3;
        public string _WP4;
        public string _WP5;

        public string _Section;

        Report theReport = new Report();
        //Procedures procs = new Procedures();

        public frmPreplanningSummary()
        {
            InitializeComponent();
        }

        private void frmPreplanningSummary_Load(object sender, EventArgs e)
        {
            
            if (_Report == "Summary")
            {

                MWDataManager.clsDataAccess _PrePlanningData = new MWDataManager.clsDataAccess();
                _PrePlanningData.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _PrePlanningData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _PrePlanningData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _PrePlanningData.SqlStatement = "exec sp_PrePlanning_Summary_Report '" + _Prodmonth + "','" + _Mosect + "' ";
                _PrePlanningData.ResultsTableName = "ProdDataSummary";
                var result = _PrePlanningData.ExecuteInstruction();
                DataTable tblprodDataSummary = _PrePlanningData.ResultsDataTable;

                DataSet dsProdDataSummary = new DataSet();
                dsProdDataSummary.Tables.Clear();
                dsProdDataSummary.Tables.Add(tblprodDataSummary);

                theReport.RegisterData(dsProdDataSummary);

                //Survey Stp
                MWDataManager.clsDataAccess _LoadSurStp = new MWDataManager.clsDataAccess();
                _LoadSurStp.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _LoadSurStp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _LoadSurStp.queryReturnType = MWDataManager.ReturnType.DataTable;
                _LoadSurStp.SqlStatement = "select * \r\n" +
                                           ",Actsqm - plansqm var \r\n" +
                                           "from( \r\n" +
                                           "select p.prodmonth \r\n" +
                                           ",convert(decimal(18, 1), sum(SqmTotal)) Plansqm \r\n" +
                                           "from tbl_PlanMonth p \r\n" +
                                           "left outer join \r\n" +
                                           "tbl_SectionComplete sc \r\n" +
                                           "on p.Sectionid = sc.SectionID \r\n" +
                                           "and p.Prodmonth = sc.Prodmonth \r\n" +
                                           "where p.prodmonth = " + _Prodmonth + " \r\n" +
                                           "and p.auth = 'Y' \r\n" +
                                           "--and p.plancode = 'MP' \r\n" +
                                           "and activity <> 1 \r\n" +
                                           "--and MOID = '" + _Mosect + "' \r\n" +
                                           "group by p.prodmonth \r\n" +
                                           ") a \r\n" +
                                           "left outer join \r\n" +
                                           "( \r\n" +
                                           "select p.prodmonth pm \r\n" +
                                           ",convert(decimal(18, 1), sum(SqmTotal)) Actsqm \r\n" +
                                           "from SURVEY p \r\n" +
                                           "left outer join \r\n" +
                                           "tbl_SectionComplete sc \r\n" +
                                           "on p.Sectionid = sc.SectionID \r\n" +
                                           "and p.Prodmonth = sc.Prodmonth \r\n" +
                                           "where p.prodmonth = " + _Prodmonth + " \r\n" +
                                           "and ActivityCode <> 1 \r\n" +
                                           "--and MOID = '" + _Mosect + "' \r\n" +
                                           "group by p.prodmonth \r\n" +
                                           ")b \r\n" +
                                           "on a.Prodmonth = b.pm";

                _LoadSurStp.ResultsTableName = "SurveyStp";
                _LoadSurStp.ExecuteInstruction();

                DataTable tbl_SurStp = _LoadSurStp.ResultsDataTable;

                DataSet dsSurStp = new DataSet();
                dsSurStp.Tables.Clear();
                dsSurStp.Tables.Add(tbl_SurStp);

                theReport.RegisterData(dsSurStp);

                //Survey Dev
                MWDataManager.clsDataAccess _SurDev = new MWDataManager.clsDataAccess();
                _SurDev.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _SurDev.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _SurDev.queryReturnType = MWDataManager.ReturnType.DataTable;
                _SurDev.SqlStatement = "select * \r\n" +
                                       ",Actadv - planadv var \r\n" +
                                       "from( \r\n" +
                                       "select p.prodmonth \r\n" +
                                       ",convert(decimal(18, 1) \r\n" +
                                       ",sum(Adv)) Planadv \r\n" +
                                       "from tbl_PlanMonth p \r\n" +
                                       "left outer join \r\n" +
                                       "tbl_SectionComplete sc \r\n" +
                                       "on p.Sectionid = sc.SectionID \r\n" +
                                       "and p.Prodmonth = sc.Prodmonth \r\n" +
                                       "where p.prodmonth = " + _Prodmonth + " \r\n" +
                                       "and p.auth = 'Y' \r\n" +
                                       "--and p.plancode = 'MP' \r\n" +
                                       "and activity = 1 \r\n" +
                                       "--and MOID = '" + _Mosect + "' \r\n" +
                                       "group by p.prodmonth \r\n" +
                                       ")a \r\n" +
                                       "left outer join \r\n" +
                                       "( \r\n" +
                                       "select p.prodmonth pm \r\n" +
                                       ",convert(decimal(18, 1) \r\n" +
                                       ",sum(Adv)) Actadv \r\n" +
                                       "from [tbl_Survey_ImportedDev] p \r\n" +
                                       "left outer join \r\n" +
                                       "tbl_SectionComplete sc \r\n" +
                                       "on p.Sectionid = sc.SectionID \r\n" +
                                       "and p.Prodmonth = sc.Prodmonth \r\n" +
                                       "where p.prodmonth = " + _Prodmonth + " \r\n" +
                                       "--and ActivityCode = 1 \r\n" +
                                       "--and MOID = '" + _Mosect + "' \r\n" +
                                       "group by p.prodmonth \r\n" +
                                       ")b \r\n" +
                                       "on a.Prodmonth = b.pm";

                _SurDev.ResultsTableName = "SurveyDev";
                _SurDev.ExecuteInstruction();

                DataTable tbl_SurDev = _SurDev.ResultsDataTable;

                DataSet dsSurDev = new DataSet();
                dsSurDev.Tables.Clear();
                dsSurDev.Tables.Add(tbl_SurDev);

                theReport.RegisterData(dsSurDev);



                #region All Open actions
                //open actions
                MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
                _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManOpenActions.SqlStatement = "Select '0' ID, convert(varchar(50) , ActionDate, (111)) datesubmitted,workplace,Action_Title, Action_Status,hazard from tbl_Incidents \r\n" +
                                                    " where Action_Status <> 'Closed' \r\n" +
                                                    " and workplace<> '' and Section = '" + _Mosect + "' \r\n" +
                                                    " union \r\n" +
                                                    " Select '1' ID , '' , '', '' , '' , '' \r\n" +
                                                    " order by ID , datesubmitted,workplace";

                _dbManOpenActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManOpenActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManOpenActions.ResultsTableName = "OpenManActions";
                _dbManOpenActions.ExecuteInstruction();

                DataSet dsOpenActions = new DataSet();
                dsOpenActions.Tables.Clear();
                dsOpenActions.Tables.Add(_dbManOpenActions.ResultsDataTable);

                //Actions
                MWDataManager.clsDataAccess _dbMandata = new MWDataManager.clsDataAccess();
                _dbMandata.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbMandata.SqlStatement = "  SELECT *, substring(HOD, CHARINDEX (':', HOD)+1, LEN(HOD)) as MO,  substring(RespPerson, CHARINDEX (':', RespPerson)+1, len(RespPerson)) as RespPerson, isnull(compnotes, '') Compnotes1, \r\n" +
                                          " case when CompNotes <> '' then '\\\\10.10.101.138\\MinewarePics\\Moabkhotsong\\Actions\\' + CompNotes + '.png' \r\n" +
                                          " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
                                         "  where Type in( 'PPRE','PPVT', 'PPSR', 'PPEG', 'PPGL', 'PPS', 'PPSM') and Completiondate is null and TheDate > '2020-05-01 00:00:00.000' \r\n";
               
                _dbMandata.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMandata.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMandata.ResultsTableName = "Actions";
                _dbMandata.ExecuteInstruction();

                DataSet dsActions = new DataSet();
                dsActions.Tables.Clear();
                dsActions.Tables.Add(_dbMandata.ResultsDataTable);

                //Register Data
                theReport.RegisterData(dsOpenActions);
                theReport.RegisterData(dsActions);
                #endregion

                #region HighRiskWP

                ////Risk
                //MWDataManager.clsDataAccess _LoadRisk = new MWDataManager.clsDataAccess();
                //_LoadRisk.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                //_LoadRisk.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_LoadRisk.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_LoadRisk.SqlStatement = " select top(10) * from (select w.Description, MAX(PlanRiskRating) PlanRiskRating \r\n" +
                //                         " , max(Safety) Safety, max(RE) RE, max(Vent) Vent, max(Geology) Geology, max(Eng) Eng \r\n" +
                //                         " from(select Prodmonth, Workplaceid, SECTION, \r\n" +
                //                         " case when dep = 'SF' then PlanRiskRating else 0 end as Safety, \r\n" +
                //                         " case when dep = 'RE' then PlanRiskRating else 0 end as RE, \r\n" +
                //                         " case when dep = 'Vent' then PlanRiskRating else 0 end as Vent, \r\n" +
                //                         " case when dep = 'GE' then PlanRiskRating else 0 end as Geology, \r\n" +
                //                         " case when dep = 'Eng' then PlanRiskRating else 0 end as Eng, PlanRiskRating \r\n" +
                //                         " from vw_Preplanning_RiskRating_Detail \r\n" +
                //                         " where prodmonth = '" + _Prodmonth + "' and section = '" + _Mosect + "') a \r\n" +
                //                         " inner join workplace w on a.workplaceid = w.WorkplaceID \r\n" +
                //                         " group by w.Description) a order by PlanRiskRating desc \r\n" +
                //                         " , Safety + RE + Vent + Geology + Eng desc";

                //_LoadRisk.ResultsTableName = "Risk";
                //_LoadRisk.ExecuteInstruction();

                //DataSet dsRisk = new DataSet();
                //dsRisk.Tables.Clear();
                //dsRisk.Tables.Add(_LoadRisk.ResultsDataTable);

                //theReport.RegisterData(dsRisk);

                ////Reasons
                //MWDataManager.clsDataAccess _LoadReasons = new MWDataManager.clsDataAccess();
                //_LoadReasons.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                //_LoadReasons.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_LoadReasons.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_LoadReasons.SqlStatement = " select * from ( \r\n" +
                //                            " select Dep, Crew, w.Description, planriskrating, RR.WorkplaceID, RR.Questid from vw_Preplanning_RiskRating_Detail RR \r\n" +
                //                            " inner join workplace w on rr.workplaceid = w.workplaceid \r\n" +
                //                            " where PRODMONTH = '" + _Prodmonth + "' \r\n" +
                //                            " and SECTION = '" + _Mosect + "' and planriskrating = '10') Tbl1 \r\n" +
                //                            " inner join \r\n" +
                //                            " ( \r\n" +
                //                            " select 'ST' Lbl, Workplace, REQuest.QuestID, Question, Answer from tbl_PrePlanning_SafetyCapt REANswer \r\n" +
                //                            " inner \r\n" +
                //                            " join tbl_PrePlanning_SafetyQuest REQuest on REANswer.QuestID = REQuest.QuestID \r\n" +
                //                            " where PRODMONTH = '" + _Prodmonth + "' and SECTION = '" + _Mosect + "' \r\n" +
                //                            " union \r\n" +
                //                            " select 'RE' Lbl, Workplace, REQuest.QuestID, Question, Answer from tbl_PrePlanning_RockEngCapt REANswer \r\n" +
                //                            " inner \r\n" +
                //                            " join tbl_PrePlanning_RockEngQuest REQuest on REANswer.QuestID = REQuest.QuestID \r\n" +
                //                            " where PRODMONTH = '" + _Prodmonth + "' and SECTION = '" + _Mosect + "' \r\n" +
                //                            " union \r\n" +
                //                            " select 'Vent' Lbl, Workplace, REQuest.QuestID, Question, Answer from tbl_PrePlanning_VentCapture REANswer \r\n" +
                //                            " inner \r\n" +
                //                            " join tbl_PrePlanning_VentQuest REQuest on REANswer.QuestID = REQuest.QuestID \r\n" +
                //                            " where PRODMONTH = '" + _Prodmonth + "' and SECTION = '" + _Mosect + "' \r\n" +
                //                            " union \r\n" +
                //                            " select 'GE' Lbl, Workplace, REQuest.QuestID, Question, Answer from tbl_PrePlanning_GeologyCapture REANswer \r\n" +
                //                            " inner \r\n" +
                //                            " join tbl_PrePlanning_GeologyQuest REQuest on REANswer.QuestID = REQuest.QuestID \r\n" +
                //                            " where PRODMONTH = '" + _Prodmonth + "' and SECTION = '" + _Mosect + "' \r\n" +
                //                            " union \r\n" +
                //                            " select 'Eng' Lbl, Workplace, REQuest.QuestID, Question, Answer from tbl_PrePlanning_EngineeringCapture REANswer \r\n" +
                //                            " inner \r\n" +
                //                            " join tbl_PrePlanning_EngineeringQuest REQuest on REANswer.QuestID = REQuest.QuestID \r\n" +
                //                            " where PRODMONTH = '" + _Prodmonth + "' and SECTION = '" + _Mosect + "') tbl2 \r\n" +
                //                            " on Tbl1.workplaceid = tbl2.Workplace and Tbl1.Dep = tbl2.Lbl and Tbl1.Questid = tbl2.Questid";

                //_LoadReasons.ResultsTableName = "Reasons";
                //_LoadReasons.ExecuteInstruction();

                //DataSet dsReasons = new DataSet();
                //dsReasons.Tables.Clear();
                //dsReasons.Tables.Add(_LoadReasons.ResultsDataTable);

                //theReport.RegisterData(dsReasons);

                #endregion


                #region Safety
                //string Date = Procedures.Prod2;

                MWDataManager.clsDataAccess _dbManAns = new MWDataManager.clsDataAccess();
                _dbManAns.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManAns.SqlStatement = "select* from( \r\n" +
                                         "select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID,  \r\n" +
                                         "q.Question,  \r\n" +
                                         "case when c.QuestID = 2 then Convert(Varchar(10),Answer,110)  else Answer end as Answer  \r\n" +
                                         ", '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy  \r\n" +
                                         "from tbl_PrePlanning_SafetyQuest q, tbl_PrePlanning_SafetyCapt c, tbl_Workplace w  \r\n" +
                                         "where q.QuestID = c.QuestID  \r\n" +
                                         "and w.WorkplaceID = c.Workplace  \r\n" +
                                         "and c.section like '" + _Mosect + "%' \r\n" +
                                         ")a  order by Workplace, OrderBy \r\n";

                _dbManAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManAns.ResultsTableName = "SafetyAnswers";
                _dbManAns.ExecuteInstruction();

                DataSet dsSafetyAns = new DataSet();
                dsSafetyAns.Tables.Clear();
                dsSafetyAns.Tables.Add(_dbManAns.ResultsDataTable);

                theReport.RegisterData(dsSafetyAns);
                #endregion

                #region RockEng
                //RE Hazard
                MWDataManager.clsDataAccess _dbManREHazard = new MWDataManager.clsDataAccess();
                _dbManREHazard.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManREHazard.SqlStatement = "Select isnull(WP1Riska,-100) WP1Risk, isnull(Wp1ResRiska,-100) Wp1ResRisk, \r\n" +
                                                "isnull(WP2Riska,-100) WP2Risk, isnull(Wp2ResRiska,-100) Wp2ResRisk, \r\n" +
                                                "isnull(WP3Riska,-100) WP3Risk, isnull(Wp3ResRiska,-100) Wp3ResRisk, \r\n" +
                                                "isnull(WP4Riska,-100) WP4Risk, isnull(Wp4ResRiska,-100) Wp4ResRisk, \r\n" +
                                                "isnull(WP5Riska,-100) WP5Risk, isnull(Wp5ResRiska,-100) Wp5ResRisk,* from ( \r\n" +
                                                "Select * , count(*) OVER() AS 'Countaa',ROW_NUMBER() OVER (ORDER BY a) AS Rownum  from ( \r\n" +
                                                "select 'a' a, Convert(Varchar(15),TheDate,106)WP1TheDate2,TheDate,\r\n" +
                                                "wpdescription WP1, \r\n" +
                                                "case when Risk = '0.00' then -100 when Risk is null then -100 else Risk end as WP1Riska, \r\n" +
                                                "case when ResRisk = '0.00' then -100 when ResRisk is null then -100 else ResRisk end as Wp1ResRiska\r\n" +
                                                "from  tbl_LicenceToOperate_Risk \r\n" +
                                                "where wpdescription = '" + _WP1Desc + "' \r\n" +

                                                ")a left outer join ( \r\n" +
                                                "select 'a' b, Convert(Varchar(15),TheDate,106)WP2TheDate2,\r\n" +
                                                "wpdescription WP2, \r\n" +
                                                "case when Risk = '0.00' then -100 when Risk is null then -100 else Risk end as WP2Riska, \r\n" +
                                                "case when ResRisk = '0.00' then -100 when ResRisk is null then -100 else ResRisk end as Wp2ResRiska \r\n" +
                                                "from  tbl_LicenceToOperate_Risk \r\n" +
                                                "where wpdescription = '" + _WP2Desc + "' \r\n" +
                                                ")b on a.a = b.b  and a.WP1TheDate2 = b.WP2TheDate2 \r\n" +

                                                "left outer join ( \r\n" +
                                                "select 'a' c,Convert(Varchar(15),TheDate,106)WP3TheDate2,\r\n" +
                                                "wpdescription WP3,\r\n" +
                                                "case when Risk = '0.00' then -100 when Risk is null then -100 else Risk end as WP3Riska, \r\n" +
                                                "case when ResRisk = '0.00' then -100 when ResRisk is null then -100 else ResRisk end as Wp3ResRiska \r\n" +
                                                "from  tbl_LicenceToOperate_Risk \r\n" +
                                                "where wpdescription = '" + _WP3Desc + "' \r\n" +
                                                ")c on a.a = c.c and a.WP1TheDate2 = c.WP3TheDate2 \r\n" +

                                                "left outer join ( \r\n" +
                                                "select 'a' d, Convert(Varchar(15),TheDate,106)WP4TheDate2,wpdescription WP4, \r\n" +
                                                "case when Risk = '0.00' then -100 when Risk is null then -100 else Risk end as WP4Riska, \r\n" +
                                                "case when ResRisk = '0.00' then -100 when ResRisk is null then -100 else ResRisk end as Wp4ResRiska \r\n" +
                                                "from  tbl_LicenceToOperate_Risk \r\n" +
                                                "where wpdescription = '" + _WP4Desc + "' \r\n" +
                                                ") d \r\n" +
                                                "on a.a = d.d and a.WP1TheDate2 = d.WP4TheDate2 \r\n" +

                                                "left outer join ( \r\n" +
                                                "select 'a' e,Convert(Varchar(15),TheDate,106)WP5TheDate2,wpdescription WP5 , \r\n" +
                                                "case when Risk = '0.00' then -100 when Risk is null then -100 else Risk end as WP5Riska, \r\n" +
                                                "case when ResRisk = '0.00' then -100 when ResRisk is null then -100 else ResRisk end as Wp5ResRiska \r\n" +
                                                "from  tbl_LicenceToOperate_Risk  \r\n" +
                                                "where wpdescription = '" + _WP5Desc + "' \r\n" +
                                                ")e \r\n" +
                                                "on a.a = e.e and a.WP1TheDate2 = e.WP5TheDate2 ) a \r\n" +

                                                "where  Rownum = (a.Countaa/5) \r\n" +
                                                "or Rownum = ((a.Countaa/5)*2) \r\n" +
                                                "or Rownum = (a.Countaa/5)*3 or Rownum = (a.Countaa/5)*4 or Rownum = (a.Countaa/5)*5   \r\n" +
                                                "order by  TheDate ";

                _dbManREHazard.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManREHazard.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManREHazard.ResultsTableName = "REHazard";
                _dbManREHazard.ExecuteInstruction();

                DataSet REHazard = new DataSet();
                REHazard.Tables.Clear();
                REHazard.Tables.Add(_dbManREHazard.ResultsDataTable);

                theReport.RegisterData(REHazard);


                MWDataManager.clsDataAccess _dbManSeismic = new MWDataManager.clsDataAccess();
                _dbManSeismic.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManSeismic.SqlStatement = " select isnull(risk1a, -100) risk1, \r\n" +
                                            "isnull(risk2a, -100) risk2, \r\n" +
                                            "isnull(risk3a, -100) risk3, \r\n" +
                                            "isnull(risk4a, -100) risk4, \r\n" +
                                            "isnull(risk5a, -100) risk5, \r\n" +
                                            " * from (select top(10) Convert(Varchar(15),TheDate,106) TheDate2, max(TheDate) dd from  \r\n" +
                                            "tbl_LicenceToOperate_Seismic where thedate >= getdate()-30 group by TheDate order by TheDate desc) a \r\n" +
                                            " left outer join \r\n" +
                                            "( \r\n" +
                                            "select Convert(Varchar(15),TheDate,106)TheDatewp1, \r\n" +
                                            "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk1a, \r\n" +
                                            "wpdescription Workplace1 from   \r\n" +
                                            "tbl_LicenceToOperate_Seismic where wpdescription in ('" + _WP1Desc + "')) b on a.thedate2 = b.TheDatewp1 \r\n" +
                                            "left outer join \r\n" +
                                            "( \r\n" +
                                            "select Convert(Varchar(15),TheDate,106)TheDatewp2, \r\n" +
                                            "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk2a, \r\n" +
                                            "wpdescription Workplace2 from   \r\n" +
                                            "tbl_LicenceToOperate_Seismic where wpdescription in ('" + _WP2Desc + "')) c on a.thedate2 = c.TheDatewp2 \r\n" +
                                            "left outer join \r\n" +
                                            "( \r\n" +
                                            "select Convert(Varchar(15),TheDate,106)TheDatewp3, \r\n" +
                                            "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk3a, \r\n" +
                                            "wpdescription Workplace3 from   \r\n" +
                                            "tbl_LicenceToOperate_Seismic where wpdescription in ('" + _WP3Desc + "')) d on a.thedate2 = d.TheDatewp3 \r\n" +
                                            "left outer join \r\n" +
                                            "( \r\n" +
                                            "select Convert(Varchar(15),TheDate,106)TheDatewp4, \r\n" +
                                            "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk4a, \r\n" +
                                            "wpdescription Workplace4 from   \r\n" +
                                            "tbl_LicenceToOperate_Seismic where wpdescription in ('" + _WP4Desc + "')) e on a.thedate2 = e.TheDatewp4 \r\n" +
                                            "left outer join \r\n" +
                                            "( \r\n" +
                                            "select Convert(Varchar(15),TheDate,106)TheDatewp5, \r\n" +
                                            "case when risk = '0.00' then -100 when risk is null then -100 else risk end as risk5a, \r\n" +
                                            "wpdescription Workplace5 from   \r\n" +
                                            "tbl_LicenceToOperate_Seismic where wpdescription in ('" + _WP5Desc + "')) f on a.thedate2 = f.TheDatewp5  order by TheDate2";

                _dbManSeismic.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSeismic.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSeismic.ResultsTableName = "Seismic";
                _dbManSeismic.ExecuteInstruction();

                DataSet dsSeismic = new DataSet();
                dsSeismic.Tables.Add(_dbManSeismic.ResultsDataTable);

                //Last backfill
                MWDataManager.clsDataAccess _dbManLB = new MWDataManager.clsDataAccess();
                _dbManLB.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManLB.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + _WP1Desc + "' wp1new,'" + _WP2Desc + "' wp2new,'" + _WP3Desc + "' wp3new,'" + _WP4Desc + "' wp4new,'" + _WP5Desc + "' wp5new, \r\n" +
                                                    " case when wp1answer is null then '-100' when wp1answer = '0' then '-100' when wp1answer = '0' then '-100' when wp1answer = '' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                    " case when wp2answer is null then '-100' when wp2answer = '0' then '-100' when wp2answer = '0' then '-100' when wp2answer = '' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                    " case when wp3answer is null then '-100' when wp3answer = '0' then '-100' when wp3answer = '0' then '-100' when wp3answer = '' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                    " case when wp4answer is null then '-100' when wp4answer = '0' then '-100' when wp4answer = '0' then '-100' when wp4answer = '' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                    " case when wp5answer is null then '-100' when wp5answer = '0' then '-100' when wp5answer = '0' then '-100' when wp5answer = '' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                    " from (     \r\n" +

                                                    " select top(6) prodmonth from tbl_planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm1zz,answer wp1answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP1Desc + "' and questid = 29     \r\n" +
                                                    " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm2zz,answer wp2answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP2Desc + "' and questid = 29     \r\n" +
                                                    " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm3zz,answer wp3answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP3Desc + "' and questid = 29     \r\n" +
                                                    " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm4zz,answer wp4answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP4Desc + "' and questid = 29      \r\n" +
                                                    " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm5zz, answer wp5answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP5Desc + "' and questid = 29     \r\n" +
                                                    " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                    " order by prodmonth";

                _dbManLB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManLB.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManLB.ResultsTableName = "Last backfill Quality check";
                _dbManLB.ExecuteInstruction();

                DataSet dsLB = new DataSet();
                dsLB.Tables.Clear();
                dsLB.Tables.Add(_dbManLB.ResultsDataTable);


                //RE Hazard Rating (6 month)
                MWDataManager.clsDataAccess _dbManREHR = new MWDataManager.clsDataAccess();
                _dbManREHR.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManREHR.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + _WP1Desc + "' wp1new,'" + _WP2Desc + "' wp2new,'" + _WP3Desc + "' wp3new,'" + _WP4Desc + "' wp4new,'" + _WP5Desc + "' wp5new, \r\n" +
                                                " case when wp1answer is null then '-100' when wp1answer = '0' then '-100' when wp1answer = '0' then '-100' when wp1answer = '' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                " case when wp2answer is null then '-100' when wp2answer = '0' then '-100' when wp2answer = '0' then '-100' when wp2answer = '' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                " case when wp3answer is null then '-100' when wp3answer = '0' then '-100' when wp3answer = '0' then '-100' when wp3answer = '' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                " case when wp4answer is null then '-100' when wp4answer = '0' then '-100' when wp4answer = '0' then '-100' when wp4answer = '' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                " case when wp5answer is null then '-100' when wp5answer = '0' then '-100' when wp5answer = '0' then '-100' when wp5answer = '' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                " from (     \r\n" +

                                                " select top(6) prodmonth from tbl_planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                " left outer join(     \r\n" +
                                                " select prodmonth pm1zz,answer wp1answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP1Desc + "' and questid = 29     \r\n" +
                                                " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                " left outer join(     \r\n" +
                                                " select prodmonth pm2zz,answer wp2answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP2Desc + "' and questid = 29     \r\n" +
                                                " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                " left outer join(     \r\n" +
                                                " select prodmonth pm3zz,answer wp3answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP3Desc + "' and questid = 29     \r\n" +
                                                " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                " left outer join(     \r\n" +
                                                " select prodmonth pm4zz,answer wp4answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP4Desc + "' and questid = 29      \r\n" +
                                                " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                " left outer join(     \r\n" +
                                                " select prodmonth pm5zz, answer wp5answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP5Desc + "' and questid = 29     \r\n" +
                                                " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                " order by prodmonth";

                _dbManREHR.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManREHR.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManREHR.ResultsTableName = "RE Hazard Rating";
                _dbManREHR.ExecuteInstruction();

                DataSet dsREHR = new DataSet();
                dsREHR.Tables.Clear();
                dsREHR.Tables.Add(_dbManREHR.ResultsDataTable);


                //Seismic (6 month)
                MWDataManager.clsDataAccess _dbManseis = new MWDataManager.clsDataAccess();
                _dbManseis.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManseis.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + _WP1Desc + "' wp1new,'" + _WP2Desc + "' wp2new,'" + _WP3Desc + "' wp3new,'" + _WP4Desc + "' wp4new,'" + _WP5Desc + "' wp5new, \r\n" +
                                        " case when wp1answer is null then '-100' when wp1answer = '0' then '-100' when wp1answer = '0' then '-100' when wp1answer = '' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                        " case when wp2answer is null then '-100' when wp2answer = '0' then '-100' when wp2answer = '0' then '-100' when wp2answer = '' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                        " case when wp3answer is null then '-100' when wp3answer = '0' then '-100' when wp3answer = '0' then '-100' when wp3answer = '' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                        " case when wp4answer is null then '-100' when wp4answer = '0' then '-100' when wp4answer = '0' then '-100' when wp4answer = '' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                        " case when wp5answer is null then '-100' when wp5answer = '0' then '-100' when wp5answer = '0' then '-100' when wp5answer = '' then '-100' else wp5answer end as wp5answer    \r\n" +

                                        " from (     \r\n" +

                                        " select top(6) prodmonth from tbl_planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                        " left outer join(     \r\n" +
                                        " select prodmonth pm1zz,answer wp1answer     \r\n" +
                                        " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                        " where a.workplace = b.workplaceid and description = '" + _WP1Desc + "' and questid = 17     \r\n" +
                                        " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                        " left outer join(     \r\n" +
                                        " select prodmonth pm2zz,answer wp2answer     \r\n" +
                                        " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                        " where a.workplace = b.workplaceid and description = '" + _WP2Desc + "' and questid = 17     \r\n" +
                                        " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                        " left outer join(     \r\n" +
                                        " select prodmonth pm3zz,answer wp3answer     \r\n" +
                                        " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                        " where a.workplace = b.workplaceid and description = '" + _WP3Desc + "' and questid = 17     \r\n" +
                                        " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                        " left outer join(     \r\n" +
                                        " select prodmonth pm4zz,answer wp4answer     \r\n" +
                                        " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                        " where a.workplace = b.workplaceid and description = '" + _WP4Desc + "' and questid = 17      \r\n" +
                                        " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                        " left outer join(     \r\n" +
                                        " select prodmonth pm5zz, answer wp5answer     \r\n" +
                                        " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                        " where a.workplace = b.workplaceid and description = '" + _WP5Desc + "' and questid = 17     \r\n" +
                                        " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                        " order by prodmonth";

                _dbManseis.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManseis.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManseis.ResultsTableName = "Seismic 6 month";
                _dbManseis.ExecuteInstruction();

                DataSet dsseis = new DataSet();
                dsseis.Tables.Clear();
                dsseis.Tables.Add(_dbManseis.ResultsDataTable);


                //Underground Re Compliance 
                MWDataManager.clsDataAccess _dbManSR = new MWDataManager.clsDataAccess();
                _dbManSR.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);

                _dbManSR.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + _WP1Desc + "' wp1new,'" + _WP2Desc + "' wp2new,'" + _WP3Desc + "' wp3new,'" + _WP4Desc + "' wp4new,'" + _WP5Desc + "' wp5new, \r\n" +
                                                " case when wp1answer is null then '-100' when wp1answer = '0' then '-100' when wp1answer = '0' then '-100' when wp1answer = '' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                " case when wp2answer is null then '-100' when wp2answer = '0' then '-100' when wp2answer = '0' then '-100' when wp2answer = '' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                " case when wp3answer is null then '-100' when wp3answer = '0' then '-100' when wp3answer = '0' then '-100' when wp3answer = '' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                " case when wp4answer is null then '-100' when wp4answer = '0' then '-100' when wp4answer = '0' then '-100' when wp4answer = '' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                " case when wp5answer is null then '-100' when wp5answer = '0' then '-100' when wp5answer = '0' then '-100' when wp5answer = '' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                " from (     \r\n" +

                                                " select top(6) prodmonth from tbl_planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                " left outer join(     \r\n" +
                                                " select prodmonth pm1zz,answer wp1answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP1Desc + "' and questid = 36     \r\n" +
                                                " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                " left outer join(     \r\n" +
                                                " select prodmonth pm2zz,answer wp2answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP2Desc + "' and questid = 36     \r\n" +
                                                " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                " left outer join(     \r\n" +
                                                " select prodmonth pm3zz,answer wp3answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP3Desc + "' and questid = 36     \r\n" +
                                                " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                " left outer join(     \r\n" +
                                                " select prodmonth pm4zz,answer wp4answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP4Desc + "' and questid = 36      \r\n" +
                                                " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                " left outer join(     \r\n" +
                                                " select prodmonth pm5zz, answer wp5answer     \r\n" +
                                                " from [dbo].[tbl_PrePlanning_RockEngCapt] a, tbl_workplace b      \r\n" +
                                                " where a.workplace = b.workplaceid and description = '" + _WP5Desc + "' and questid = 36     \r\n" +
                                                " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                " order by prodmonth";

                _dbManSR.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSR.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSR.ResultsTableName = "Underground RE compliance";
                _dbManSR.ExecuteInstruction();

                DataSet dsSR = new DataSet();
                dsSR.Tables.Clear();
                dsSR.Tables.Add(_dbManSR.ResultsDataTable);


                MWDataManager.clsDataAccess _dbManREAns = new MWDataManager.clsDataAccess();
                _dbManREAns.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManREAns.SqlStatement = "select* from(  \r\n" +
                                           "select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                           "Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                           "from tbl_PrePlanning_RockEngQuest q, tbl_PrePlanning_RockEngCapt c, tbl_workplace w \r\n" +
                                           "where q.QuestID = c.QuestID \r\n" +
                                           "and w.WorkplaceID = c.Workplace \r\n" +
                                           "and c.section like '" + _Mosect + "' \r\n" +
                                           ")a order by Workplace, OrderBy \r\n";

                _dbManREAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManREAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManREAns.ResultsTableName = "REAnswers";
                _dbManREAns.ExecuteInstruction();

                DataSet dsreAns = new DataSet();
                dsreAns.Tables.Clear();
                dsreAns.Tables.Add(_dbManREAns.ResultsDataTable);

                theReport.RegisterData(dsreAns);
                #endregion

                #region Ventilation
                MWDataManager.clsDataAccess _dbMandataGraphWP1Temp = new MWDataManager.clsDataAccess();
                _dbMandataGraphWP1Temp.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbMandataGraphWP1Temp.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + _WP1Desc + "' wp1new,'" + _WP2Desc + "' wp2new,'" + _WP3Desc + "' wp3new,'" + _WP4Desc + "' wp4new,'" + _WP5Desc + "' wp5new, \r\n" +
                                                    " case when wp1answer is null then '-100' when wp1answer = '0.0' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                    " case when wp2answer is null then '-100' when wp2answer = '0.0' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                    " case when wp3answer is null then '-100' when wp3answer = '0.0' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                    " case when wp4answer is null then '-100' when wp4answer = '0.0' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                    " case when wp5answer is null then '-100' when wp5answer = '0.0' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                    " from (     \r\n" +

                                                    " select top(6) prodmonth from tbl_planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm1zz,Workplace wp1,answer wp1answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_Workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP1Desc + "' and questid = 1     \r\n" +
                                                    " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm2zz,Workplace wp2,answer wp2answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_Workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP2Desc + "' and questid = 1     \r\n" +
                                                    " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm3zz,Workplace wp3,answer wp3answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_Workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP3Desc + "' and questid = 1     \r\n" +
                                                    " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm4zz,Workplace wp4,answer wp4answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_Workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP4Desc + "' and questid = 1     \r\n" +
                                                    " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm5zz,Workplace wp5,answer wp5answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_Workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP5Desc + "' and questid = 1     \r\n" +
                                                    " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                    " order by prodmonth";

                _dbMandataGraphWP1Temp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMandataGraphWP1Temp.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMandataGraphWP1Temp.ResultsTableName = "Actions1GraphTemp";
                _dbMandataGraphWP1Temp.ExecuteInstruction();

                DataSet dsGraphWp1Temp = new DataSet();
                dsGraphWp1Temp.Tables.Clear();
                dsGraphWp1Temp.Tables.Add(_dbMandataGraphWP1Temp.ResultsDataTable);

                MWDataManager.clsDataAccess _dbMandataGraphWP1Vel = new MWDataManager.clsDataAccess();
                _dbMandataGraphWP1Vel.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbMandataGraphWP1Vel.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + _WP1Desc + "' wp1new,'" + _WP2Desc + "' wp2new,'" + _WP3Desc + "' wp3new,'" + _WP4Desc + "' wp4new,'" + _WP5Desc + "' wp5new, \r\n" +
                                                    " case when wp1answer is null then '-100' when wp1answer = '0.0' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                    " case when wp2answer is null then '-100' when wp2answer = '0.0' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                    " case when wp3answer is null then '-100' when wp3answer = '0.0' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                    " case when wp4answer is null then '-100' when wp4answer = '0.0' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                    " case when wp5answer is null then '-100' when wp5answer = '0.0' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                    " from (     \r\n" +

                                                    " select top(6) prodmonth from tbl_planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm1zz,Workplace wp1,answer wp1answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP1Desc + "' and questid = 2     \r\n" +
                                                    " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm2zz,Workplace wp2,answer wp2answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP2Desc + "' and questid = 2    \r\n" +
                                                    " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm3zz,Workplace wp3,answer wp3answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP3Desc + "' and questid = 2     \r\n" +
                                                    " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm4zz,Workplace wp4,answer wp4answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP4Desc + "' and questid = 2     \r\n" +
                                                    " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm5zz,Workplace wp5,answer wp5answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP5Desc + "' and questid = 2     \r\n" +
                                                    " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                    " order by prodmonth";

                _dbMandataGraphWP1Vel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMandataGraphWP1Vel.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMandataGraphWP1Vel.ResultsTableName = "Actions1GraphVel";
                _dbMandataGraphWP1Vel.ExecuteInstruction();

                DataSet dsGraphWp1Vel = new DataSet();
                dsGraphWp1Vel.Tables.Clear();
                dsGraphWp1Vel.Tables.Add(_dbMandataGraphWP1Vel.ResultsDataTable);

                MWDataManager.clsDataAccess _dbMandataGraphWP1AU = new MWDataManager.clsDataAccess();
                _dbMandataGraphWP1AU.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbMandataGraphWP1AU.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + _WP1Desc + "' wp1new,'" + _WP2Desc + "' wp2new,'" + _WP3Desc + "' wp3new,'" + _WP4Desc + "' wp4new,'" + _WP5Desc + "' wp5new, \r\n" +
                                                    " case when wp1answer is null then '-100' when wp1answer = '0.0' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                    " case when wp2answer is null then '-100' when wp2answer = '0.0' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                    " case when wp3answer is null then '-100' when wp3answer = '0.0' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                    " case when wp4answer is null then '-100' when wp4answer = '0.0' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                    " case when wp5answer is null then '-100' when wp5answer = '0.0' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                     " from (     \r\n" +

                                                     " select top(6) prodmonth from tbl_planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                     " left outer join(     \r\n" +
                                                     " select prodmonth pm1zz,Workplace wp1,answer wp1answer     \r\n" +
                                                     " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                     " where a.workplace = b.workplaceid and description = '" + _WP1Desc + "' and questid = 4     \r\n" +
                                                     " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                     " left outer join(     \r\n" +
                                                     " select prodmonth pm2zz,Workplace wp2,answer wp2answer     \r\n" +
                                                     " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                     " where a.workplace = b.workplaceid and description = '" + _WP2Desc + "' and questid = 4     \r\n" +
                                                     " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                     " left outer join(     \r\n" +
                                                     " select prodmonth pm3zz,Workplace wp3,answer wp3answer     \r\n" +
                                                     " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                     " where a.workplace = b.workplaceid and description = '" + _WP3Desc + "' and questid = 4     \r\n" +
                                                     " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                     " left outer join(     \r\n" +
                                                     " select prodmonth pm4zz,Workplace wp4,answer wp4answer     \r\n" +
                                                     " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                     " where a.workplace = b.workplaceid and description = '" + _WP4Desc + "' and questid = 4     \r\n" +
                                                     " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                     " left outer join(     \r\n" +
                                                     " select prodmonth pm5zz,Workplace wp5,answer wp5answer     \r\n" +
                                                     " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                     " where a.workplace = b.workplaceid and description = '" + _WP5Desc + "' and questid = 4     \r\n" +
                                                     " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                     " order by prodmonth";

                _dbMandataGraphWP1AU.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMandataGraphWP1AU.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMandataGraphWP1AU.ResultsTableName = "Actions1GraphAu";
                _dbMandataGraphWP1AU.ExecuteInstruction();

                DataSet dsGraphWp1AU = new DataSet();
                dsGraphWp1AU.Tables.Clear();
                dsGraphWp1AU.Tables.Add(_dbMandataGraphWP1AU.ResultsDataTable);

                MWDataManager.clsDataAccess _dbMandataGraphWP1SCP = new MWDataManager.clsDataAccess();
                _dbMandataGraphWP1SCP.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbMandataGraphWP1SCP.SqlStatement = " select substring(convert(varchar(11),convert(date,convert(varchar(10),Prodmonth)+'01'),106),4,88) pm1,'" + _WP1Desc + "' wp1new,'" + _WP2Desc + "' wp2new,'" + _WP3Desc + "' wp3new,'" + _WP4Desc + "' wp4new,'" + _WP5Desc + "' wp5new, \r\n" +
                                                    " case when wp1answer is null then '-100' when wp1answer = '0.0' then '-100' else wp1answer end as wp1answer,    \r\n" +
                                                    " case when wp2answer is null then '-100' when wp2answer = '0.0' then '-100' else wp2answer end as wp2answer,    \r\n" +
                                                    " case when wp3answer is null then '-100' when wp3answer = '0.0' then '-100' else wp3answer end as wp3answer,    \r\n" +
                                                    " case when wp4answer is null then '-100' when wp4answer = '0.0' then '-100' else wp4answer end as wp4answer,    \r\n" +
                                                    " case when wp5answer is null then '-100' when wp5answer = '0.0' then '-100' else wp5answer end as wp5answer    \r\n" +

                                                    " from (     \r\n" +

                                                    " select top(6) prodmonth from tbl_planning where calendardate < getdate() group by prodmonth order by prodmonth desc) a   \r\n" +
                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm1zz,Workplace wp1,answer wp1answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP1Desc + "' and questid = 3     \r\n" +
                                                    " ) b on a.prodmonth = b.pm1zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm2zz,Workplace wp2,answer wp2answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP2Desc + "' and questid = 3     \r\n" +
                                                    " ) c on a.prodmonth = c.pm2zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm3zz,Workplace wp3,answer wp3answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP3Desc + "' and questid = 3     \r\n" +
                                                    " ) d on a.prodmonth = d.pm3zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm4zz,Workplace wp4,answer wp4answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP4Desc + "' and questid = 3     \r\n" +
                                                    " ) e on a.prodmonth = e.pm4zz     \r\n" +

                                                    " left outer join(     \r\n" +
                                                    " select prodmonth pm5zz,Workplace wp5,answer wp5answer     \r\n" +
                                                    " from [dbo].[tbl_PrePlanning_VentCapture] a, tbl_workplace b      \r\n" +
                                                    " where a.workplace = b.workplaceid and description = '" + _WP5Desc + "' and questid = 3     \r\n" +
                                                    " ) f on a.prodmonth = f.pm5zz     \r\n" +

                                                    " order by prodmonth";

                _dbMandataGraphWP1SCP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMandataGraphWP1SCP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMandataGraphWP1SCP.ResultsTableName = "Actions1GraphSCP";
                _dbMandataGraphWP1SCP.ExecuteInstruction();

                DataSet dsGraphWp1SCP = new DataSet();
                dsGraphWp1SCP.Tables.Clear();
                dsGraphWp1SCP.Tables.Add(_dbMandataGraphWP1SCP.ResultsDataTable);


                MWDataManager.clsDataAccess VentTopic = new MWDataManager.clsDataAccess();
                VentTopic.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                VentTopic.SqlStatement = " Select '" + _Prodmonth + "' Prodmonth ,'" + _Mosect + "' MO,'' VentTopic  ";

                VentTopic.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                VentTopic.queryReturnType = MWDataManager.ReturnType.DataTable;
                VentTopic.ResultsTableName = "VentTopic";
                VentTopic.ExecuteInstruction();

                DataSet dsVentTopic = new DataSet();
                dsVentTopic.Tables.Clear();
                dsVentTopic.Tables.Add(VentTopic.ResultsDataTable);


                MWDataManager.clsDataAccess _dbventManAns = new MWDataManager.clsDataAccess();
                _dbventManAns.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbventManAns.SqlStatement = "select* from(  \r\n" +
                                             "select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                             "case when c.QuestID = 1 then Convert(Varchar(5),Answer)  \r\n" +
                                             "when c.QuestID = 2 and answer<> '' then CAST(ROUND(answer,2) as varchar)  \r\n" +
                                             "when c.QuestID = 3 and answer<> '' then CAST(ROUND(answer,2) as varchar)   \r\n" +
                                             "when c.QuestID = 4 and answer<> '' then CAST(ROUND(answer,2) as varchar)   \r\n" +
                                             "when c.QuestID = 10 then Convert(Varchar(10),Answer,110)   \r\n" +
                                             "when c.QuestID = 16 and answer<> '' then Convert(Varchar(10),Answer,110)   \r\n" +
                                             "when c.QuestID = 17 and answer<> '' then CAST(ROUND(answer,2) as varchar)  \r\n" +
                                             "else Answer end as Answer \r\n" +
                                             ", '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                             "from tbl_PrePlanning_VentQuest q, tbl_PrePlanning_VentCapture c, tbl_Workplace w \r\n" +
                                             "where q.QuestID = c.QuestID \r\n" +
                                             "and w.WorkplaceID = c.Workplace \r\n" +
                                             "and c.section like '" + _Mosect + "%' \r\n" +
                                             ")a order by Workplace, OrderBy \r\n";

                _dbventManAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbventManAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbventManAns.ResultsTableName = "VentAnswers";
                _dbventManAns.ExecuteInstruction();

                DataSet dsventAns = new DataSet();
                dsventAns.Tables.Clear();
                dsventAns.Tables.Add(_dbventManAns.ResultsDataTable);

                theReport.RegisterData(dsventAns);
                theReport.RegisterData(dsSafetyAns);
                ////////////////////////////////

                theReport.RegisterData(dsVentTopic);
                theReport.RegisterData(dsGraphWp1Temp);
                theReport.RegisterData(dsGraphWp1Vel);
                theReport.RegisterData(dsGraphWp1AU);
                theReport.RegisterData(dsGraphWp1SCP);

                #endregion

                theReport.RegisterData(dsSeismic);
                theReport.RegisterData(dsLB);
                theReport.RegisterData(dsREHR);
                theReport.RegisterData(dsseis);
                theReport.RegisterData(dsSR);


                #region Survey
                MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
                _dbMan2.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbMan2.SqlStatement = "  ";
                _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan2.ResultsTableName = "People";
                _dbMan2.ExecuteInstruction();

                DataSet ds = new DataSet();
                ds.Tables.Clear();
                ds.Tables.Add(_dbMan2.ResultsDataTable);

                //CaptureInfo
                MWDataManager.clsDataAccess _dbManCapt = new MWDataManager.clsDataAccess();
                _dbManCapt.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);

                _dbManCapt.SqlStatement = " Select qst.question,WP1,WP2,WP3,WP4,WP5 from tbl_PrePlanning_SurveyQuest Qst \r\n" +

                                        "left outer join ( \r\n" +
                                        "Select questid,answer WP1 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _Prodmonth + "' and section = '" + _Mosect + "%'  \r\n" +
                                        ") fstWp on qst.questid = fstWp.questid \r\n" +
                                        "left outer join ( \r\n" +
                                        "Select questid,answer WP2 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _Prodmonth + "' and section = '" + _Mosect + "%'  \r\n" +
                                        ") secWp on qst.questid = secWp.questid \r\n" +
                                        "left outer join ( \r\n" +
                                        "Select questid,answer WP3 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _Prodmonth + "' and section = '" + _Mosect + "%'  \r\n" +
                                        ") thrdWp on qst.questid = thrdWp.questid \r\n" +
                                        "left outer join ( \r\n" +
                                        "Select questid,answer WP4 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _Prodmonth + "' and section = '" + _Mosect + "%'  \r\n" +
                                        ") frthWp on qst.questid = frthWp.questid \r\n" +
                                        "left outer join ( \r\n" +
                                        "Select questid,answer WP5 from tbl_PrePlanning_SurveyCapt  \r\n" +
                                        "where  prodmonth = '" + _Prodmonth + "' and section like '" + _Mosect + "%'  \r\n" +
                                        ") ftfhWp on qst.questid = ftfhWp.questid \r\n" +
                                        "where qst.questid in (16,17,21,20,22)  \r\n" +
                                        " ";

                _dbManCapt.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCapt.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCapt.ResultsTableName = "SurvCaptinfo";
                _dbManCapt.ExecuteInstruction();

                DataSet dsCapt = new DataSet();
                dsCapt.Tables.Clear();
                dsCapt.Tables.Add(_dbManCapt.ResultsDataTable);

                //Complaince
                MWDataManager.clsDataAccess _dbManComp = new MWDataManager.clsDataAccess();
                _dbManComp.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);

                _dbManComp.SqlStatement = "Select substring(convert(varchar(11),convert(date,convert(varchar(10),a.Prodmonth)+'01'),106),4,88) Prodlbl,* from ( \r\n" +
                                        "Select a.prodmonth, WP1, case when a.bsqm is null then 0 when a.PSqm = 0 then 0  else convert(decimal(8,0), a.Bsqm/a.PSqm * 100) end as Compliance1 \r\n" +
                                        ",'a' a \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.Description WP1,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_planning p,tbl_workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_Prodmonth) - 3) + "'   \r\n" +
                                        "and p.sectionId like '" + _Mosect + "' \r\n" +
                                        "group by  prodmonth,w.Description ) a  )a \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP2, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance2  \r\n" +
                                        ",'a' b \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.description WP2,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_planning p,tbl_workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_Prodmonth) - 3) + "'  \r\n" +
                                        "and p.sectionId like '" + _Mosect + "' \r\n" +
                                        "group by  prodmonth,w.Description ) a ) b on a.a = b.b and a.prodmonth = b.prodmonth \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP3, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance3  \r\n" +
                                        ",'a' c \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.description WP3,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_planning p,tbl_workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_Prodmonth) - 3) + "' \r\n" +
                                        "and p.sectionId like '" + _Mosect + "' \r\n" +
                                        "group by  prodmonth,w.Description )a ) c on a.a = c.c and a.prodmonth = c.prodmonth \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP4, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance4  \r\n" +
                                        ",'a' d \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.Description WP4,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_planning p,tbl_workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_Prodmonth) - 3) + "'   \r\n" +
                                        "and p.sectionId like '" + _Mosect + "' \r\n" +
                                        "group by  prodmonth,w.Description )a ) d on a.a = d.d and a.prodmonth = d.prodmonth \r\n" +

                                        "left outer join ( \r\n" +

                                        "Select prodmonth,WP5, case when bsqm is null then 0  when a.PSqm = 0 then 0 else convert(decimal(8,0), Bsqm/PSqm * 100) end as Compliance5  \r\n" +
                                        ",'a' e \r\n" +
                                        "from (   \r\n" +
                                        "Select prodmonth,w.description WP5,sum(sqm) PSqm,sum(booksqm) Bsqm from tbl_planning p,tbl_workplace w  \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.activity =w.activity and \r\n" +
                                        "prodmonth >= '" + Convert.ToString(Convert.ToInt32(_Prodmonth) - 3) + "'  \r\n" +
                                        "and p.sectionId like '" + _Mosect + "' \r\n" +
                                        "group by  prodmonth,w.Description ) a ) e  on a.a = e.e and a.prodmonth = e.prodmonth ";

                _dbManComp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManComp.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManComp.ResultsTableName = "Comp";
                _dbManComp.ExecuteInstruction();

                DataSet dsComp = new DataSet();
                dsComp.Tables.Clear();
                dsComp.Tables.Add(_dbManComp.ResultsDataTable);


                MWDataManager.clsDataAccess _dbManSurvAns = new MWDataManager.clsDataAccess();
                _dbManSurvAns.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManSurvAns.SqlStatement = "select* from(  \r\n" +
                                             "select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                             "Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                             "from tbl_PrePlanning_SurveyQuest q, tbl_PrePlanning_SurveyCapt c, tbl_Workplace w \r\n" +
                                             "where q.QuestID = c.QuestID \r\n" +
                                             "and w.WorkplaceID = c.Workplace \r\n" +
                                             "and c.prodmonth = '" + _Prodmonth + "' \r\n" +
                                             "and c.section like '" + _Mosect + "%' \r\n" +
                                             ")a order by Workplace, OrderBy \r\n";

                _dbManSurvAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSurvAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSurvAns.ResultsTableName = "SurvAnswers";
                _dbManSurvAns.ExecuteInstruction();

                DataSet dsSurvAns = new DataSet();
                dsSurvAns.Tables.Clear();
                dsSurvAns.Tables.Add(_dbManSurvAns.ResultsDataTable);

                theReport.RegisterData(dsSurvAns);
                ////////////////////////////////

                theReport.RegisterData(ds);
                theReport.RegisterData(dsCapt);
                theReport.RegisterData(dsComp);
                #endregion


                #region Geology          
                //CMGT
                MWDataManager.clsDataAccess _dbManCMGT = new MWDataManager.clsDataAccess();
                _dbManCMGT.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManCMGT.SqlStatement = "exec sp_PrePlanning_GeoGraphs '" + _Prodmonth + "','" + _Section + "','" + _WP1 + "','" + _WP2 + "','" + _WP3 + "','" + _WP4 + "','" + _WP5 + "' ";

                _dbManCMGT.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCMGT.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCMGT.ResultsTableName = "Graphs";
                _dbManCMGT.ExecuteInstruction();

                DataSet dsCMGTNEW = new DataSet();
                dsCMGTNEW.Tables.Clear();
                dsCMGTNEW.Tables.Add(_dbManCMGT.ResultsDataTable);


                MWDataManager.clsDataAccess _dbManGeoAns = new MWDataManager.clsDataAccess();
                _dbManGeoAns.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManGeoAns.SqlStatement = "select* from(  \r\n" +
                                            "select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                            "Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                            "from tbl_PrePlanning_GeologyQuest q, tbl_PrePlanning_GeologyCapture c, tbl_workplace w \r\n" +
                                            "where q.QuestID = c.QuestID \r\n" +
                                            "and w.WorkplaceID = c.Workplace \r\n" +
                                            "and c.Section ='" + _Mosect + "'  \r\n" +
                                            ")a order by Workplace, OrderBy \r\n";

                _dbManGeoAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGeoAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGeoAns.ResultsTableName = "GeologyAnswers";
                _dbManGeoAns.ExecuteInstruction();

                DataSet dsGeoAns = new DataSet();
                dsGeoAns.Tables.Clear();
                dsGeoAns.Tables.Add(_dbManGeoAns.ResultsDataTable);

                theReport.RegisterData(dsGeoAns);
                theReport.RegisterData(dsCMGTNEW);
                #endregion

                ///////////////////////////////////
                //No Sample Preplan @ Amandelbult//
                ///////////////////////////////////
                //#region Sampling
                //MWDataManager.clsDataAccess _dbMansw1 = new MWDataManager.clsDataAccess();
                //_dbMansw1.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                //_dbMansw1.SqlStatement = "declare @Data int "+
                //                         "set @Data = (select count(GMSIWPIS) Records from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w " +
                //                         "where a.GMSIWPIS = w.Description and w.workplaceid = '" + _WP1 + "') "+
                //                         "select*, case when @Data > 0 then 'Y' else 'N' end as IsSampling "+
                //                         "from(select convert(varchar(11), calendardate,106) dd, SWidth, GT,StdSWidth AllocatedWidth, SWidth-Footwall -Hangwall chn, 1 orderby "+
                //                         "from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w  where a.GMSIWPIS = w.Description " +
                //                         "and w.workplaceid = '" + _WP1 + "'  "+
                //                         "union select convert(varchar(11),GETDATE(),106) dd, -100 SWidth,-100 GT, -100 AllocatedWidth, 0 chn, 2 orderby )a "+
                //                         "order by orderby ";

                //_dbMansw1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbMansw1.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbMansw1.ResultsTableName = "sw1";
                //_dbMansw1.ExecuteInstruction();

                //DataSet dssw1 = new DataSet();
                //dssw1.Tables.Clear();
                //dssw1.Tables.Add(_dbMansw1.ResultsDataTable);

                //MWDataManager.clsDataAccess _dbMansw2 = new MWDataManager.clsDataAccess();
                //_dbMansw2.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                //_dbMansw2.SqlStatement = "declare @Data int " +
                //                         "set @Data = (select count(GMSIWPIS) Records from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w " +
                //                         "where a.GMSIWPIS = w.Description and w.workplaceid = '" + _WP1 + "') " +
                //                         "select*, case when @Data > 0 then 'Y' else 'N' end as IsSampling " +
                //                         "from(select convert(varchar(11), calendardate,106) dd, SWidth, GT,StdSWidth AllocatedWidth, SWidth-Footwall -Hangwall chn, 1 orderby " +
                //                         "from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w  where a.GMSIWPIS = w.Description " +
                //                         "and w.workplaceid = '" + _WP2 + "'  " +
                //                         "union select convert(varchar(11),GETDATE(),106) dd, -100 SWidth,-100 GT, -100 AllocatedWidth, 0 chn, 2 orderby )a " +
                //                         "order by orderby ";

                //_dbMansw2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbMansw2.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbMansw2.ResultsTableName = "sw2";
                //_dbMansw2.ExecuteInstruction();

                //DataSet dssw2 = new DataSet();
                //dssw2.Tables.Clear();
                //dssw2.Tables.Add(_dbMansw2.ResultsDataTable);

                //MWDataManager.clsDataAccess _dbMansw3 = new MWDataManager.clsDataAccess();
                //_dbMansw3.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                //_dbMansw3.SqlStatement = "declare @Data int " +
                //                         "set @Data = (select count(GMSIWPIS) Records from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w " +
                //                         "where a.GMSIWPIS = w.Description and w.workplaceid = '" + _WP1 + "') " +
                //                         "select*, case when @Data > 0 then 'Y' else 'N' end as IsSampling " +
                //                         "from(select convert(varchar(11), calendardate,106) dd, SWidth, GT,StdSWidth AllocatedWidth, SWidth-Footwall -Hangwall chn, 1 orderby " +
                //                         "from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w  where a.GMSIWPIS = w.Description " +
                //                         "and w.workplaceid = '" + _WP3 + "'  " +
                //                         "union select convert(varchar(11),GETDATE(),106) dd, -100 SWidth,-100 GT, -100 AllocatedWidth, 0 chn, 2 orderby )a " +
                //                         "order by orderby ";

                //_dbMansw3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbMansw3.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbMansw3.ResultsTableName = "sw3";
                //_dbMansw3.ExecuteInstruction();

                //DataSet dssw3 = new DataSet();
                //dssw3.Tables.Clear();
                //dssw3.Tables.Add(_dbMansw3.ResultsDataTable);


                //MWDataManager.clsDataAccess _dbMansw4 = new MWDataManager.clsDataAccess();
                //_dbMansw4.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                //_dbMansw4.SqlStatement = "declare @Data int " +
                //                         "set @Data = (select count(GMSIWPIS) Records from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w " +
                //                         "where a.GMSIWPIS = w.Description and w.workplaceid = '" + _WP1 + "') " +
                //                         "select*, case when @Data > 0 then 'Y' else 'N' end as IsSampling " +
                //                         "from(select convert(varchar(11), calendardate,106) dd, SWidth, GT,StdSWidth AllocatedWidth, SWidth-Footwall -Hangwall chn, 1 orderby " +
                //                         "from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w  where a.GMSIWPIS = w.Description " +
                //                         "and w.workplaceid = '" + _WP4 + "'  " +
                //                         "union select convert(varchar(11),GETDATE(),106) dd, -100 SWidth,-100 GT, -100 AllocatedWidth, 0 chn, 2 orderby )a " +
                //                         "order by orderby ";

                //_dbMansw4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbMansw4.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbMansw4.ResultsTableName = "sw4";
                //_dbMansw4.ExecuteInstruction();

                //DataSet dssw4 = new DataSet();
                //dssw4.Tables.Clear();
                //dssw4.Tables.Add(_dbMansw4.ResultsDataTable);

                //MWDataManager.clsDataAccess _dbMansw5 = new MWDataManager.clsDataAccess();
                //_dbMansw5.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                //_dbMansw5.SqlStatement = "declare @Data int " +
                //                         "set @Data = (select count(GMSIWPIS) Records from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w " +
                //                         "where a.GMSIWPIS = w.Description and w.workplaceid = '" + _WP1 + "') " +
                //                         "select*, case when @Data > 0 then 'Y' else 'N' end as IsSampling " +
                //                         "from(select convert(varchar(11), calendardate,106) dd, SWidth, GT,StdSWidth AllocatedWidth, SWidth-Footwall -Hangwall chn, 1 orderby " +
                //                         "from[dbo].[tbl_Sampling_Imported] a, tbl_Workplace w  where a.GMSIWPIS = w.Description " +
                //                         "and w.workplaceid = '" + _WP5 + "'  " +
                //                         "union select convert(varchar(11),GETDATE(),106) dd, -100 SWidth,-100 GT, -100 AllocatedWidth, 0 chn, 2 orderby )a " +
                //                         "order by orderby ";

                //_dbMansw5.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbMansw5.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbMansw5.ResultsTableName = "sw5";
                //_dbMansw5.ExecuteInstruction();

                //DataSet dssw5 = new DataSet();
                //dssw5.Tables.Clear();
                //dssw5.Tables.Add(_dbMansw5.ResultsDataTable);

                //theReport.RegisterData(dssw1);
                //theReport.RegisterData(dssw2);
                //theReport.RegisterData(dssw3);
                //theReport.RegisterData(dssw4);
                //theReport.RegisterData(dssw5);

                ////MWDataManager.clsDataAccess _dbManSampAns = new MWDataManager.clsDataAccess();
                ////_dbManSampAns.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                ////_dbManSampAns.SqlStatement = "select* from(  \r\n"+
                ////                             "select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n"+
                ////                             "case  when c.QuestID = 9 then Convert(Varchar(10),Answer,110)   \r\n"+
                ////                             "when c.QuestID = 10 and answer <> '' then CAST(ROUND(answer,0) as varchar)   \r\n"+
                ////                             "else Answer end as Answer \r\n"+
                ////                             ", '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n"+
                ////                             "from tbl_PrePlanning_SamplingQuest q, tbl_PrePlanning_SamplingCapture c, tbl_workplace w \r\n"+
                ////                             "where q.QuestID = c.QuestID \r\n"+
                ////                             "and w.WorkplaceID = c.Workplace \r\n"+
                ////                             "and c.Section like '" + _Mosect + "%' \r\n"+
                ////                             ")a order by Workplace, OrderBy \r\n";

                ////_dbManSampAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                ////_dbManSampAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                ////_dbManSampAns.ResultsTableName = "SamplingAnswers";
                ////_dbManSampAns.ExecuteInstruction();

                ////DataSet dsSampAns = new DataSet();
                ////dsSampAns.Tables.Clear();
                ////dsSampAns.Tables.Add(_dbManSampAns.ResultsDataTable);

                ////theReport.RegisterData(dsSampAns);
                //#endregion
                ///////////////////////////////////

                #region Engineering
                //Face Winch
                MWDataManager.clsDataAccess _dbManFW = new MWDataManager.clsDataAccess();
                _dbManFW.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManFW.SqlStatement = " select workplace, FWanswer, FWdist, fw1,fw2 from (     \r\n" +
                                        " select workplace, case when answer = '' then ' : ' when answer is null then ' : ' else answer end as FWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as fw1, Right(answer, CHARINDEX (':', answer ) -4) as fw2 from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "'  and workplace = '" + _WP1 + "'    \r\n" +
                                        " and questid in ('1')) a,     \r\n" +

                                        " (select answer FWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "'  and workplace = '" + _WP1 + "'     \r\n" +
                                        " and questid in ('2')) b  ";

                _dbManFW.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManFW.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManFW.ResultsTableName = "FaceWinch";
                _dbManFW.ExecuteInstruction();

                DataSet dsFW = new DataSet();
                dsFW.Tables.Clear();
                dsFW.Tables.Add(_dbManFW.ResultsDataTable);

                //Face Winch2
                MWDataManager.clsDataAccess _dbManFW2 = new MWDataManager.clsDataAccess();
                _dbManFW2.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManFW2.SqlStatement = " select workplace, FWanswer, FWdist, fw1,fw2 from (     \r\n" +
                                        " select workplace, case when answer = '' then ' : ' when answer is null then ' : ' else answer end as FWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as fw1, Right(answer, CHARINDEX (':', answer ) -4) as fw2 from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "'  and workplace = '" + _WP2 + "'    \r\n" +
                                        " and questid in ('1')) a,     \r\n" +

                                        " (select answer FWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "'  and workplace = '" + _WP2 + "'     \r\n" +
                                        " and questid in ('2')) b  ";

                _dbManFW2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManFW2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManFW2.ResultsTableName = "FaceWinch2";
                _dbManFW2.ExecuteInstruction();

                DataSet dsFW2 = new DataSet();
                dsFW2.Tables.Clear();
                dsFW2.Tables.Add(_dbManFW2.ResultsDataTable);

                //Face Winch3
                MWDataManager.clsDataAccess _dbManFW3 = new MWDataManager.clsDataAccess();
                _dbManFW3.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManFW3.SqlStatement = " select workplace, FWanswer, FWdist, fw1,fw2 from (     \r\n" +
                                        " select workplace, case when answer = '' then ' : ' when answer is null then ' : ' else answer end as FWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as fw1, Right(answer, CHARINDEX (':', answer ) -4) as fw2 from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP3 + "'    \r\n" +
                                        " and questid in ('1')) a,     \r\n" +

                                        " (select answer FWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP3 + "'     \r\n" +
                                        " and questid in ('2')) b  ";

                _dbManFW3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManFW3.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManFW3.ResultsTableName = "FaceWinch3";
                _dbManFW3.ExecuteInstruction();

                DataSet dsFW3 = new DataSet();
                dsFW3.Tables.Clear();
                dsFW3.Tables.Add(_dbManFW3.ResultsDataTable);

                //Face Winch4
                MWDataManager.clsDataAccess _dbManFW4 = new MWDataManager.clsDataAccess();
                _dbManFW4.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManFW4.SqlStatement = " select workplace, FWanswer, FWdist, fw1,fw2 from (     \r\n" +
                                        " select workplace, case when answer = '' then ' : ' when answer is null then ' : ' else answer end as FWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as fw1, Right(answer, CHARINDEX (':', answer ) -4) as fw2 from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP4 + "'    \r\n" +
                                        " and questid in ('1')) a,     \r\n" +

                                        " (select answer FWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP4 + "'     \r\n" +
                                        " and questid in ('2')) b  ";

                _dbManFW4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManFW4.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManFW4.ResultsTableName = "FaceWinch4";
                _dbManFW4.ExecuteInstruction();

                DataSet dsFW4 = new DataSet();
                dsFW4.Tables.Clear();
                dsFW4.Tables.Add(_dbManFW4.ResultsDataTable);

                //Face Winch5
                MWDataManager.clsDataAccess _dbManFW5 = new MWDataManager.clsDataAccess();
                _dbManFW5.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManFW5.SqlStatement = " select workplace, FWanswer, FWdist, fw1,fw2 from (     \r\n" +
                                        " select workplace, case when answer = '' then ' : ' when answer is null then ' : ' else answer end as FWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as fw1, Right(answer, CHARINDEX (':', answer ) -4) as fw2 from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP5 + "'    \r\n" +
                                        " and questid in ('1')) a,     \r\n" +

                                        " (select answer FWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP5 + "'     \r\n" +
                                        " and questid in ('2')) b  ";

                _dbManFW5.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManFW5.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManFW5.ResultsTableName = "FaceWinch5";
                _dbManFW5.ExecuteInstruction();

                DataSet dsFW5 = new DataSet();
                dsFW5.Tables.Clear();
                dsFW5.Tables.Add(_dbManFW5.ResultsDataTable);


                //Gully Winch
                MWDataManager.clsDataAccess _dbManGW = new MWDataManager.clsDataAccess();
                _dbManGW.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManGW.SqlStatement = " select workplace, GWanswer, GWdist, LEFT(GWanswer, CHARINDEX (':', GWanswer ) - 1) as gw1,Right(GWanswer, CHARINDEX (':', GWanswer ) -4) as gw2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '   :   ' when answer is null then '   :   ' else answer end as GWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as gw1, Right(answer, CHARINDEX (':', answer ) -4) as gw2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP1 + "'     \r\n" +
                                        " and questid in ('23')) a,     \r\n" +

                                        " (select answer GWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP1 + "'     \r\n" +
                                        " and questid in ('3')) b   ";

                _dbManGW.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGW.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGW.ResultsTableName = "GullyWinch";
                _dbManGW.ExecuteInstruction();

                DataSet dsGW = new DataSet();
                dsGW.Tables.Clear();
                dsGW.Tables.Add(_dbManGW.ResultsDataTable);

                //Gully Winch 2
                MWDataManager.clsDataAccess _dbManGW2 = new MWDataManager.clsDataAccess();
                _dbManGW2.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManGW2.SqlStatement = " select workplace, GWanswer, GWdist, LEFT(GWanswer, CHARINDEX (':', GWanswer ) - 1) as gw1,Right(GWanswer, CHARINDEX (':', GWanswer ) -4) as gw2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '   :   ' when answer is null then '   :   ' else answer end as GWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as gw1, Right(answer, CHARINDEX (':', answer ) -4) as gw2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP2 + "'     \r\n" +
                                        " and questid in ('23')) a,     \r\n" +

                                        " (select answer GWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP2 + "'     \r\n" +
                                        " and questid in ('3')) b   ";

                _dbManGW2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGW2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGW2.ResultsTableName = "GullyWinch2";
                _dbManGW2.ExecuteInstruction();

                DataSet dsGW2 = new DataSet();
                dsGW2.Tables.Clear();
                dsGW2.Tables.Add(_dbManGW2.ResultsDataTable);

                //Gully Winch 3
                MWDataManager.clsDataAccess _dbManGW3 = new MWDataManager.clsDataAccess();
                _dbManGW3.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManGW3.SqlStatement = " select workplace, GWanswer, GWdist, LEFT(GWanswer, CHARINDEX (':', GWanswer ) - 1) as gw1,Right(GWanswer, CHARINDEX (':', GWanswer ) -4) as gw2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '   :   ' when answer is null then '   :   ' else answer end as GWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as gw1, Right(answer, CHARINDEX (':', answer ) -4) as gw2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP3 + "'     \r\n" +
                                        " and questid in ('23')) a,     \r\n" +

                                        " (select answer GWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP3 + "'     \r\n" +
                                        " and questid in ('3')) b   ";

                _dbManGW3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGW3.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGW3.ResultsTableName = "GullyWinch3";
                _dbManGW3.ExecuteInstruction();

                DataSet dsGW3 = new DataSet();
                dsGW3.Tables.Clear();
                dsGW3.Tables.Add(_dbManGW3.ResultsDataTable);

                //Gully Winch 4
                MWDataManager.clsDataAccess _dbManGW4 = new MWDataManager.clsDataAccess();
                _dbManGW4.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManGW4.SqlStatement = " select workplace, GWanswer, GWdist, LEFT(GWanswer, CHARINDEX (':', GWanswer ) - 1) as gw1,Right(GWanswer, CHARINDEX (':', GWanswer ) -4) as gw2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '    :   ' when answer is null then '   :   ' else answer end as GWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as gw1, Right(answer, CHARINDEX (':', answer ) -4) as gw2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP4 + "'     \r\n" +
                                        " and questid in ('23')) a,     \r\n" +

                                        " (select answer GWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP4 + "'     \r\n" +
                                        " and questid in ('3')) b   ";

                _dbManGW4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGW4.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGW4.ResultsTableName = "GullyWinch4";
                _dbManGW4.ExecuteInstruction();

                DataSet dsGW4 = new DataSet();
                dsGW4.Tables.Clear();
                dsGW4.Tables.Add(_dbManGW4.ResultsDataTable);

                //Gully Winch 5
                MWDataManager.clsDataAccess _dbManGW5 = new MWDataManager.clsDataAccess();
                _dbManGW5.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManGW5.SqlStatement = " select workplace, GWanswer, GWdist, LEFT(GWanswer, CHARINDEX (':', GWanswer ) - 1) as gw1,Right(GWanswer, CHARINDEX (':', GWanswer ) -4) as gw2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '   :   ' when answer is null then '   :   ' else answer end as GWanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as gw1, Right(answer, CHARINDEX (':', answer ) -4) as gw2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP5 + "'     \r\n" +
                                        " and questid in ('23')) a,     \r\n" +

                                        " (select answer GWdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP5 + "'     \r\n" +
                                        " and questid in ('3')) b   ";

                _dbManGW5.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGW5.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGW5.ResultsTableName = "GullyWinch5";
                _dbManGW5.ExecuteInstruction();

                DataSet dsGW5 = new DataSet();
                dsGW5.Tables.Clear();

                //Waterjet
                MWDataManager.clsDataAccess _dbManWJ = new MWDataManager.clsDataAccess();
                _dbManWJ.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManWJ.SqlStatement = " select workplace, WJanswer, WJdist, LEFT(WJanswer, CHARINDEX (':', WJanswer ) - 1) as wj1, Right(WJanswer, CHARINDEX (':', WJanswer ) -4) as wj2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '   :   ' when answer is null then '   :   ' else answer end as WJanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as wj1, Right(answer, CHARINDEX (':', answer ) -4) as wj2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP1 + "'     \r\n" +
                                        " and questid in ('6')) a,     \r\n" +

                                        " (select answer WJdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP1 + "'     \r\n" +
                                        " and questid in ('7')) b     ";

                _dbManWJ.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWJ.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWJ.ResultsTableName = "Waterjet";
                _dbManWJ.ExecuteInstruction();

                DataSet dsWJ = new DataSet();
                dsWJ.Tables.Clear();
                dsWJ.Tables.Add(_dbManWJ.ResultsDataTable);

                //Waterjet 2
                MWDataManager.clsDataAccess _dbManWJ2 = new MWDataManager.clsDataAccess();
                _dbManWJ2.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManWJ2.SqlStatement = " select workplace, WJanswer, WJdist, LEFT(WJanswer, CHARINDEX (':', WJanswer ) - 1) as wj1,Right(WJanswer, CHARINDEX (':', WJanswer ) -4) as wj2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '   :   ' when answer is null then '   :   ' else answer end as WJanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as wj1, Right(answer, CHARINDEX (':', answer ) -4) as wj2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP2 + "'     \r\n" +
                                        " and questid in ('6')) a,     \r\n" +

                                        " (select answer WJdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP2 + "'     \r\n" +
                                        " and questid in ('7')) b     ";

                _dbManWJ2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWJ2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWJ2.ResultsTableName = "Waterjet2";
                _dbManWJ2.ExecuteInstruction();

                DataSet dsWJ2 = new DataSet();
                dsWJ2.Tables.Clear();
                dsWJ2.Tables.Add(_dbManWJ2.ResultsDataTable);

                //Waterjet 3
                MWDataManager.clsDataAccess _dbManWJ3 = new MWDataManager.clsDataAccess();
                _dbManWJ3.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManWJ3.SqlStatement = " select workplace, WJanswer, WJdist, LEFT(WJanswer, CHARINDEX (':', WJanswer ) - 1) as wj1,Right(WJanswer, CHARINDEX (':', WJanswer ) -4) as wj2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '   :   ' when answer is null then '   :   ' else answer end as WJanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as wj1, Right(answer, CHARINDEX (':', answer ) -4) as wj2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP3 + "'     \r\n" +
                                        " and questid in ('6')) a,     \r\n" +

                                        " (select answer WJdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP3 + "'     \r\n" +
                                        " and questid in ('7')) b     ";

                _dbManWJ3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWJ3.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWJ3.ResultsTableName = "Waterjet3";
                _dbManWJ3.ExecuteInstruction();

                DataSet dsWJ3 = new DataSet();
                dsWJ3.Tables.Clear();
                dsWJ3.Tables.Add(_dbManWJ3.ResultsDataTable);

                //Waterjet 3
                MWDataManager.clsDataAccess _dbManWJ4 = new MWDataManager.clsDataAccess();
                _dbManWJ4.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManWJ4.SqlStatement = " select workplace, WJanswer, WJdist, LEFT(WJanswer, CHARINDEX (':', WJanswer ) - 1) as wj1,Right(WJanswer, CHARINDEX (':', WJanswer ) -4) as wj2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '   :   ' when answer is null then '   :   ' else answer end as WJanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as wj1, Right(answer, CHARINDEX (':', answer ) -4) as wj2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP4 + "'     \r\n" +
                                        " and questid in ('6')) a,     \r\n" +

                                        " (select answer WJdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP4 + "'     \r\n" +
                                        " and questid in ('7')) b     ";

                _dbManWJ4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWJ4.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWJ4.ResultsTableName = "Waterjet4";
                _dbManWJ4.ExecuteInstruction();

                DataSet dsWJ4 = new DataSet();
                dsWJ4.Tables.Clear();
                dsWJ4.Tables.Add(_dbManWJ4.ResultsDataTable);

                //Waterjet 5
                MWDataManager.clsDataAccess _dbManWJ5 = new MWDataManager.clsDataAccess();
                _dbManWJ5.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManWJ5.SqlStatement = " select workplace, WJanswer, WJdist, LEFT(WJanswer, CHARINDEX (':', WJanswer ) - 1) as wj1,Right(WJanswer, CHARINDEX (':', WJanswer ) -4) as wj2  from (     \r\n" +
                                        " select workplace, case when answer = '' then '   :   ' when answer is null then '   :   ' else answer end as WJanswer, LEFT(answer, CHARINDEX (':', answer ) - 1) as wj1, Right(answer, CHARINDEX (':', answer ) -4) as wj2  from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP5 + "'     \r\n" +
                                        " and questid in ('6')) a,     \r\n" +

                                        " (select answer WJdist from [dbo].[tbl_PrePlanning_EngineeringCapture]     \r\n" +
                                        " where prodmonth = '" + _Prodmonth + "' and workplace = '" + _WP5 + "'     \r\n" +
                                        " and questid in ('7')) b     ";

                _dbManWJ5.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWJ5.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWJ5.ResultsTableName = "Waterjet5";
                _dbManWJ5.ExecuteInstruction();

                DataSet dsWJ5 = new DataSet();
                dsWJ5.Tables.Clear();
                dsWJ5.Tables.Add(_dbManWJ5.ResultsDataTable);


                MWDataManager.clsDataAccess _dbManEngAns = new MWDataManager.clsDataAccess();
                _dbManEngAns.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManEngAns.SqlStatement = "select* from(  \r\n" +
                                            "select c.Prodmonth, c.Section, c.Workplace, w.Description, c.QuestID, q.Question, \r\n" +
                                            "Answer, '' Notes, '' RiskRating , convert(int, (OrderBy))OrderBy \r\n" +
                                            "from tbl_PrePlanning_EngineeringQuest q, tbl_PrePlanning_EngineeringCapture c, tbl_workplace w \r\n" +
                                            "where q.QuestID = c.QuestID \r\n" +
                                            "and w.WorkplaceID = c.Workplace \r\n" +
                                            "and c.section like '" + _Mosect + "' \r\n" +
                                            ")a order by Workplace, OrderBy \r\n";

                _dbManEngAns.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManEngAns.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManEngAns.ResultsTableName = "EngAnswers";
                _dbManEngAns.ExecuteInstruction();


                DataSet dsEngAns = new DataSet();
                dsEngAns.Tables.Clear();
                dsEngAns.Tables.Add(_dbManEngAns.ResultsDataTable);

                theReport.RegisterData(dsEngAns);

                theReport.RegisterData(dsFW);
                theReport.RegisterData(dsFW2);
                theReport.RegisterData(dsFW3);
                theReport.RegisterData(dsFW4);
                theReport.RegisterData(dsFW5);

                theReport.RegisterData(dsGW);
                theReport.RegisterData(dsGW2);
                theReport.RegisterData(dsGW3);
                theReport.RegisterData(dsGW4);
                theReport.RegisterData(dsGW5);

                theReport.RegisterData(dsWJ);
                theReport.RegisterData(dsWJ2);
                theReport.RegisterData(dsWJ3);
                theReport.RegisterData(dsWJ4);
                theReport.RegisterData(dsWJ5);
                #endregion

                //#region HR

                ////HR
                //MWDataManager.clsDataAccess _LoadHR = new MWDataManager.clsDataAccess();
                //_LoadHR.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                //_LoadHR.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_LoadHR.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_LoadHR.SqlStatement = " select employee_number, surname_and_Initials, rtrim(job_title) job_title, age_category, service_category,  leave_due_Date, medical_certificate_exp_date  \r\n" +
                //                       " MedicalDate \r\n" +
                //                       " from [HGSQL005].[PWA].[PWA_MASTER].[employee] \r\n" +
                //                       " where org6_reference = 'MAAACANURA'";
                //_LoadHR.ResultsTableName = "HR";
                //_LoadHR.ExecuteInstruction();

                //DataSet dsHR = new DataSet();
                //dsHR.Tables.Clear();
                //dsHR.Tables.Add(_LoadHR.ResultsDataTable);


                //MWDataManager.clsDataAccess _data = new MWDataManager.clsDataAccess();
                //_data.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                //_data.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_data.queryReturnType = MWDataManager.ReturnType.DataTable;

                //_data.SqlStatement = "  Select nn,job_title,personal_grade,Plan1,Act, convert( varchar(20),Var1) Var1 from ( \r\n" +
                //                     " select '1' nn, job_title, personal_grade,isnull(max(day), 0) Plan1, isnull(sum(num), 0) Act,convert(varchar(20), isnull(sum(num), 0) - isnull(max(day), 0))  Var1 from ( \r\n" +
                //                     " select * from( \r\n" +
                //                     " select 1 num, employee_number, surname_and_Initials, \r\n" +
                //                     " rtrim(job_title) job_title, age_category, service_category, \r\n" +
                //                     " leave_due_Date, medical_certificate_exp_date \r\n" +
                //                     " MedicalDate, personal_grade \r\n" +
                //                     " from[HGSQL005].[PWA].[PWA_MASTER].[employee] \r\n" +
                //                     " where org6_reference = 'MAAACANURA') a \r\n" +
                //                     " left outer join \r\n" +
                //                     " (SELECT * \r\n" +
                //                     " FROM[PAS_MK_Syncromine].[dbo].[HRStandardsAndNormsDesignations] \r\n" +
                //                     " where HRStandardsAndNormsDesignationsID in ('2', '23', '25')) b on a.job_title = b.Designation \r\n" +
                //                     " ) a \r\n" +
                //                     " group by job_title, personal_grade \r\n" +
                //                     " union \r\n" +
                //                     " select '2' nn, 'Total' job_title, '' personal_grade, sum(Plan1) Plan1, sum(Act) Act, sum(var1) var1 from ( \r\n" +
                //                     " select job_title, personal_grade, isnull(max(day), 0) Plan1, isnull(sum(num), 0) Act,isnull(sum(num), 0) - isnull(max(day), 0) var1 from ( \r\n" +
                //                     " select * from( \r\n" +
                //                     " select 1 num, employee_number, surname_and_Initials, \r\n" +
                //                     " rtrim(job_title) job_title, age_category, service_category, \r\n" +
                //                     " leave_due_Date, medical_certificate_exp_date \r\n" +
                //                     "  MedicalDate, personal_grade \r\n" +
                //                     " from[HGSQL005].[PWA].[PWA_MASTER].[employee] \r\n" +
                //                     " where org6_reference = 'MAAACANURA') a \r\n" +
                //                     " left outer join \r\n" +
                //                     " (SELECT * \r\n" +
                //                     " FROM[PAS_MK_Syncromine].[dbo].[HRStandardsAndNormsDesignations] \r\n" +
                //                     " where HRStandardsAndNormsDesignationsID in ('2', '23', '25')) b on a.job_title = b.Designation \r\n" +
                //                     " ) a \r\n" +
                //                     " group by job_title, personal_grade) a \r\n" +
                //                     " ) a order by nn, personal_grade desc";
                //_data.ResultsTableName = "JobTitle";
                //_data.ExecuteInstruction();

                //DataSet dsjobtitle = new DataSet();
                //dsjobtitle.Tables.Clear();
                //dsjobtitle.Tables.Add(_data.ResultsDataTable);


                //theReport.RegisterData(dsHR);
                //theReport.RegisterData(dsjobtitle);

                //#endregion


                theReport.Load(ReportFolder + "PreplanningSummary.frx");
                //theReport.Load(TGlobalItems.ReportsFolder + "\\PreplanningSummary.frx");

                theReport.SetParameterValue("WP1Desc", _WP1Desc);
                theReport.SetParameterValue("WP2Desc", _WP2Desc);
                theReport.SetParameterValue("WP3Desc", _WP3Desc);
                theReport.SetParameterValue("WP4Desc", _WP4Desc);
                theReport.SetParameterValue("WP5Desc", _WP5Desc);

                theReport.SetParameterValue("WP1", _WP1);
                theReport.SetParameterValue("WP2", _WP2);
                theReport.SetParameterValue("WP3", _WP3);
                theReport.SetParameterValue("WP4", _WP4);
                theReport.SetParameterValue("WP5", _WP5);

                theReport.SetParameterValue("PM", _Prodmonth);
                theReport.SetParameterValue("MO", _Mosect);

                //theReport.Design();

                pcReport.Clear();
                theReport.Prepare();
                theReport.Preview = pcReport;
                theReport.ShowPrepared();
            }
            if (_Report == "RE Comp")
            {
                MWDataManager.clsDataAccess _PrePlanningData = new MWDataManager.clsDataAccess();
                _PrePlanningData.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _PrePlanningData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _PrePlanningData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _PrePlanningData.SqlStatement = "exec sp_Report_Pillar_Compliance '" + _Prodmonth + "','" + _Mosect + "' ";
                _PrePlanningData.ResultsTableName = "ProdDataSummary";
                var result = _PrePlanningData.ExecuteInstruction();
                DataTable tblprodDataSummary = _PrePlanningData.ResultsDataTable;

                DataSet dsProdDataSummary = new DataSet();
                dsProdDataSummary.Tables.Clear();
                dsProdDataSummary.Tables.Add(tblprodDataSummary);

                theReport.RegisterData(dsProdDataSummary);

                #region All Open actions
                //open actions
                MWDataManager.clsDataAccess _dbManOpenActions = new MWDataManager.clsDataAccess();
                _dbManOpenActions.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbManOpenActions.SqlStatement = "Select '0' ID, convert(varchar(50) , ActionDate, (111)) datesubmitted,workplace,Action_Title, Action_Status,hazard from tbl_Incidents \r\n" +
                                                    " where Action_Status <> 'Closed' \r\n" +
                                                    " and workplace<> '' and Section = '" + _Mosect + "' \r\n" +
                                                    " union \r\n" +
                                                    " Select '1' ID , '' , '', '' , '' , '' \r\n" +
                                                    " order by ID , datesubmitted,workplace";

                _dbManOpenActions.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManOpenActions.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManOpenActions.ResultsTableName = "OpenManActions";
                _dbManOpenActions.ExecuteInstruction();

                DataSet dsOpenActions = new DataSet();
                dsOpenActions.Tables.Clear();
                dsOpenActions.Tables.Add(_dbManOpenActions.ResultsDataTable);

                //Actions
                MWDataManager.clsDataAccess _dbMandata = new MWDataManager.clsDataAccess();
                _dbMandata.ConnectionString = TConnections.GetConnectionString(_systemDBTag, _UserCurrentInfoConnection);
                _dbMandata.SqlStatement = "  SELECT *, substring(HOD, CHARINDEX (':', HOD)+1, LEN(HOD)) as MO,  substring(RespPerson, CHARINDEX (':', RespPerson)+1, len(RespPerson)) as RespPerson, isnull(compnotes, '') Compnotes1, \r\n" +
                                          " case when CompNotes <> '' then '\\\\10.10.101.138\\MinewarePics\\Moabkhotsong\\Actions\\' + CompNotes + '.png' \r\n" +
                                          " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
                                         "  where Type in( 'PPRE') and Completiondate is null and TheDate > '2020-05-01 00:00:00.000' \r\n";

                _dbMandata.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMandata.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMandata.ResultsTableName = "Actions";
                _dbMandata.ExecuteInstruction();

                DataSet dsActions = new DataSet();
                dsActions.Tables.Clear();
                dsActions.Tables.Add(_dbMandata.ResultsDataTable);

                //Register Data
                theReport.RegisterData(dsOpenActions);
                theReport.RegisterData(dsActions);
                #endregion

                theReport.Load(ReportFolder + "PreplanningREComp.frx");

                theReport.SetParameterValue("PM", _Prodmonth);
                theReport.SetParameterValue("MO", _Mosect);

                //theReport.Design();

                pcReport.Clear();
                theReport.Prepare();
                theReport.Preview = pcReport;
                theReport.ShowPrepared();
            }
            if (_Report == "High Risk")
            {
                this.Cursor = Cursors.WaitCursor;
                

                //TODO: make sure calling correct report and name the table correctly
                string reportFileName = string.Empty;
                string sql = string.Empty;
                string tableName = string.Empty;

                string Banner = ProductionGlobalTSysSettings._Banner.ToString();

                //Add Section and Prodmonth
                string Prodaa = _Prodmonth;

                ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(Prodaa));
                string ProdMonthDesc = ProductionGlobal.ProductionGlobal.Prod2;
                string SectDesc = _Mosect;

                DataSet ds = new DataSet();
                MWDataManager.clsDataAccess _databaseConnection = new MWDataManager.clsDataAccess();
                _databaseConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _databaseConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _databaseConnection.queryReturnType = MWDataManager.ReturnType.DataTable;

                reportFileName = "HighRiskDepartment.frx";
                tableName = "6Shift";
                sql = @"EXEC [dbo].[sp_Report_HighRisk] '" + _Prodmonth + @"', '" + _Mosect + @"' ";

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

                theReport.Load(ReportFolder + reportFileName);
                theReport.RegisterData(ds);

                theReport.SetParameterValue("ProdMonth", ProdMonthDesc);
                theReport.SetParameterValue("MOSection", SectDesc);
                theReport.SetParameterValue("Banner", Banner);

                // _theReport.Design();

                //Show the report
                //theReport.Design();

                pcReport.Clear();
                theReport.Prepare();
                theReport.Preview = pcReport;
                theReport.ShowPrepared();
            }
        }
    }
}