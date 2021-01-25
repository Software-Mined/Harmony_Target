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
    public partial class ucPlanningReport : BaseUserControl
    {
        #region Fields and Properties           
        private Report _theReport = new Report();
        private string _FirstLoad = "Y";
        DataTable dtReport = new DataTable();
        DataTable dtOrgUnit = new DataTable();
        string Plan;
        string Book;

        /// <summary>
        /// The report to show
        /// </summary>
        public Report TheReport
        {
            get
            {
                return _theReport;
            }
            set
            {
                LoadReport(value);
            }
        }

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
        public ucPlanningReport()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpPlanningReport);
            FormActiveRibbonPage = rpPlanningReport;
            FormMainRibbonPage = rpPlanningReport;
            RibbonControl = rcPlanningReport;
        }
        #endregion Constructor

        #region Events
        private void ucPlanningReport_Load(object sender, EventArgs e)
        {
            cbxBookingProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobalTSysSettings._currentProductionMonth.ToString());
            //mwPlanPM.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionAmplatsGlobalTSysSettings._currentProductionMonth.ToString());
            LoadSections();

            //PlanTyperadioGroup.SelectedIndex = 0;
            LoadPlanningAnglo();
            _FirstLoad = "N";
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainLoad();
        }

        private void mwPlanPM_EditValueChanged(object sender, EventArgs e)
        {
            LoadSections();
            MainLoad();
        }

        private void MainLoad()
        {
            if (radioType.EditValue.ToString() == "0")
            {
                LoadPlanningAnglo();
            }

            if (radioType.EditValue.ToString() == "2")
            {
                WarSheetReport();
            }
        }

        private void luePlanMOSection_EditValueChanged(object sender, EventArgs e)
        {
            MainLoad();
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
        public void LoadData()
        {
            string reportFileName = string.Empty;
            string sql = string.Empty;
            DataSet ds = new DataSet();
            MWDataManager.clsDataAccess _databaseConnection = new MWDataManager.clsDataAccess();
            _databaseConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _databaseConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _databaseConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _databaseConnection.SqlStatement = "PUT SQL HERE";

            //switch (luePlanReport.EditValue.ToString())
            //{
            //    case "Planning":
            //        reportFileName = "Planning.frx";
            //        //sql = @"EXEC [dbo].[sp_MODaily_Developement] '" + mwPlanPM.EditValue.ToString() + @"', '" + luePlanMOSection.EditValue.ToString() + @"'";
            //        break;
            //}



            var ActionResult = _databaseConnection.ExecuteInstruction();

            if (ActionResult.success)
            {
                _databaseConnection.ResultsDataTable.TableName = "TABLENAME";
                ds.Tables.Add(_databaseConnection.ResultsDataTable);
            }


            _theReport.RegisterData(ds);

            //Show the report
            LoadReport(_theReport);
        }

        void LoadPlanningAnglo()
        {

            MWDataManager.clsDataAccess _dbManBanner = new MWDataManager.clsDataAccess();
            _dbManBanner.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManBanner.SqlStatement = " select '" + TUserInfo.Site + "'+' Mine', '" + cbxBookingMO.EditValue.ToString() + "', '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "', '" + cbxBookingMO.EditValue + "'  ";

            _dbManBanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManBanner.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManBanner.ResultsTableName = "Banner";
            _dbManBanner.ExecuteInstruction();


            DataSet DsBanner = new DataSet();
            DsBanner.Tables.Add(_dbManBanner.ResultsDataTable);

            _theReport.RegisterData(DsBanner);

            //_theReport.Load("Planning.frx");


            MWDataManager.clsDataAccess _dbManDate = new MWDataManager.clsDataAccess();
            _dbManDate.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + " select *, DATEPART(ISOWK,BeginDate) ww  ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + " , DATEPART(ISOWK,BeginDate+7) ww1   ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+14) ww2  ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+21) ww3  ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+28) ww4  ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+35) ww5  ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+42) ww6  ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+49) ww7  ";

            _dbManDate.SqlStatement = _dbManDate.SqlStatement + " from (select Min(s.BeginDate) BeginDate,MAX(s.EndDate) EndDate from tbl_Code_Calendar_Section s  ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + " left outer join tbl_Section sc ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + " on s.Sectionid = sc.SectionID ";
            _dbManDate.SqlStatement = _dbManDate.SqlStatement + " where s.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and sc.ReportToSectionid = '" + cbxBookingMO.EditValue + "') a ";
            _dbManDate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDate.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDate.ResultsTableName = "sysset";
            _dbManDate.ExecuteInstruction();

            TimeSpan Span;
            DateTime BeginDate;
            DateTime EndDate;
            DataTable CalDate = new DataTable();
            int Day = 0;
            int Week = 0;
            int Weekno = 0;

            BeginDate = Convert.ToDateTime(_dbManDate.ResultsDataTable.Rows[0][0].ToString());
            EndDate = Convert.ToDateTime(_dbManDate.ResultsDataTable.Rows[0][1].ToString());
            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][2].ToString());

            CalDate.Rows.Add();

            for (int x = 0; x <= 45; x++)
            {
                if (BeginDate.AddDays(Day) <= EndDate)
                {

                    CalDate.Columns.Add();
                    CalDate.Rows[0][x] = BeginDate.AddDays(Day).ToString("dd MMM ddd");
                    if (BeginDate.AddDays(Day).ToString("ddd") == "Mon")
                    {
                        CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                W";
                    }
                    if (BeginDate.AddDays(Day).ToString("ddd") == "Tue")
                    {
                        CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                E";
                    }
                    if (BeginDate.AddDays(Day).ToString("ddd") == "Wed")
                    {
                        CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                E";
                    }
                    if (BeginDate.AddDays(Day).ToString("ddd") == "Thu")
                    {
                        CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                K";
                    }
                    if (BeginDate.AddDays(Day).ToString("ddd") == "Fri")
                    {
                        CalDate.Rows[0][x] = CalDate.Rows[0][x] + "               -";
                    }
                    if (BeginDate.AddDays(Day).ToString("ddd") == "Sat")
                    {
                        // do first wwk
                        if (Weekno == 0)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][2].ToString());

                        if (Weekno == 1)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][3].ToString());

                        if (Weekno == 2)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][4].ToString());

                        if (Weekno == 3)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][5].ToString());

                        if (Weekno == 4)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][6].ToString());
                        if (Weekno == 5)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][7].ToString());
                        if (Weekno == 6)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][8].ToString());


                        // CalDate.Rows[0][x] = CalDate.Rows[0][x] + "        ";
                        if (Week >= 5000)
                        {
                            //Week = 1;

                            if (Week > 9)
                            {
                                CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(0, 1);

                            }
                            else
                            {
                                CalDate.Rows[0][x] = CalDate.Rows[0][x] + " " + "         0";

                            }
                        }
                        else
                        {
                            if (Week > 9)
                            {
                                CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(0, 1);

                            }
                            else
                            {
                                CalDate.Rows[0][x] = CalDate.Rows[0][x] + " " + "         0";

                            }
                        }



                    }

                    if (BeginDate.AddDays(Day).ToString("ddd") == "Sun")
                    {
                        if (Weekno == 0)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][2].ToString());

                        if (Weekno == 1)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][3].ToString());

                        if (Weekno == 2)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][4].ToString());

                        if (Weekno == 3)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][5].ToString());

                        if (Weekno == 4)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][6].ToString());
                        if (Weekno == 5)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][7].ToString());
                        if (Weekno == 6)
                            Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][8].ToString());

                        // CalDate.Rows[0][x] = CalDate.Rows[0][x] + "        ";
                        if (Week >= 54000)
                        {
                            // Week = 1;

                            if (Week > 9)
                            {
                                CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(1, 1);
                                //Week = Week + 1;

                            }
                            else
                            {
                                CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString();
                                //Week = Week + 1;

                            }
                        }
                        else
                        {
                            if (Week > 9)
                            {
                                CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(1, 1);
                                // Week = Week + 1;

                            }
                            else
                            {
                                CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString();
                                //Week = Week + 1;

                            }
                        }

                        Weekno = Weekno + 1;

                    }


                    // Weekno = Weekno + 1;

                    Day = Day + 1;


                }
                else
                {
                    CalDate.Columns.Add();
                    CalDate.Rows[0][x] = string.Empty;
                }

            }

            CalDate.Columns.Add();
            CalDate.Rows[0][CalDate.Columns.Count - 1] = Day.ToString();

            CalDate.TableName = "CalDates";
            DataSet DsCalDate = new DataSet();
            DsCalDate.Tables.Add(CalDate);

            _theReport.RegisterData(DsCalDate);


            MWDataManager.clsDataAccess _dbManPlan = new MWDataManager.clsDataAccess();
            _dbManPlan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "declare @Start datetime ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "set @Start = ( ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "select  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "min(calendardate) ss from tbl_Planning_Vamping v, tbl_section s , tbl_section s1, tbl_section s2 ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "where v.Sectionid = s.SectionID and v.Prodmonth = s.Prodmonth  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth   ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " and v.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and s2.SectionID = '" + cbxBookingMO.EditValue.ToString() + "')  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  declare @prev integer ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " set @prev = ( select max(prodmonth) aaaa from tbl_planmonth where prodmonth < '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "') ";



            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " select  case when newptt is null then 'Red' when newptt is not null and newptt < @prev then 'orange' else '' end as newwpflag, * from ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  (select '1' Line,s1.Sectionid sbid, s1.Name sbname, s.Sectionid minid, s.Name minname, pm.workplaceid, case when pm.activity = 9 then w.description+' Ledge' else w.description end as description , \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    case when pm.Activity = 9 then 'Ledge' \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "   when pm.Activity = 0 then 'Stope' \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "   when pm.Activity = 1 then 'Dev' End as Act, \r\n ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity = 1 then Adv else 0 end as DevTotAdv,  \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity = 1 and w.ReefWaste = 'W' then Adv else 0 end as DevWasteAdv, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity = 1 then Tons else 0 end as DevTons, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when  pm.Activity = 1 then Content else 0 end as DevCont,  \r\n ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity <> 1 then Tons else 0 end as StpTons, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity <> 1 then Content else 0 end as StpCont,  \r\n ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    case when pm.Activity <> 1 then 'a.Stoping' else 'b.Development' end as MainGroup, \r\n ";


            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    pm.FL, pm.SqmTotal plansqm, cyclesqm, orgunitds, 'Default Cycle' aa, pm.adv, tons, content, offreefsqm, Budget, pm.activity,   \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  case when pm.activity <> 1 then pm.extrabudget else 0 end as extrabudget, case when pm.activity = 1 then pm.extrabudget else 0 end as extrabudget1,  SUBSTRING(DefaultCycle,1,4) aa1, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,5,4) aa2, SUBSTRING(DefaultCycle,9,4) aa3, SUBSTRING(DefaultCycle,13,4) aa4, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,17,4) aa5, SUBSTRING(DefaultCycle,21,4) aa6, SUBSTRING(DefaultCycle,25,4) aa7, SUBSTRING(DefaultCycle,29,4) aa8, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,33,4) aa9, SUBSTRING(DefaultCycle,37,4) aa10, SUBSTRING(DefaultCycle,41,4) aa11, SUBSTRING(DefaultCycle,45,4) aa12, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,49,4) aa13, SUBSTRING(DefaultCycle,53,4) aa14, SUBSTRING(DefaultCycle,57,4) aa15, SUBSTRING(DefaultCycle,61,4) aa16, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,65,4) aa17, SUBSTRING(DefaultCycle,69,4) aa18, SUBSTRING(DefaultCycle,73,4) aa19, SUBSTRING(DefaultCycle,77,4) aa20, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,81,4) aa21, SUBSTRING(DefaultCycle,85,4) aa22, SUBSTRING(DefaultCycle,89,4) aa23, SUBSTRING(DefaultCycle,93,4) aa24, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,97,4) aa25, SUBSTRING(DefaultCycle,101,4) aa26, SUBSTRING(DefaultCycle,105,4) aa27, SUBSTRING(DefaultCycle,109,4) aa28, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,113,4) aa29, SUBSTRING(DefaultCycle,117,4) aa30, SUBSTRING(DefaultCycle,121,4) aa31, SUBSTRING(DefaultCycle,125,4) aa32, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,129,4) aa33, SUBSTRING(DefaultCycle,133,4) aa34, SUBSTRING(DefaultCycle,137,4) aa35, SUBSTRING(DefaultCycle,141,4) aa36, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,145,4) aa37, SUBSTRING(DefaultCycle,149,4) aa38, SUBSTRING(DefaultCycle,153,4) aa39, SUBSTRING(DefaultCycle,157,4) aa40, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,161,4) aa41, SUBSTRING(DefaultCycle,165,4) aa42, SUBSTRING(DefaultCycle,169,4) aa43, SUBSTRING(DefaultCycle,173,4) aa44, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,177,4) aa45, s1.ReportToSectionid, wt.WpSublocation Mprass, wpexternalid, case when substring(comments,1,3) = 'Sur' then 'Survey Zero Plan' else '' end as Surv ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , pmold1 newptt ";



            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      from tbl_PlanMonth pm, (select w.*, pmold1 from tbl_Workplace w left outer join ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     (select workplaceid wz, max(prodmonth) pmold1 from tbl_PlanMonth ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     where prodmonth < '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' group by workplaceid) newwp on  w.workplaceid = newwp.wz) w, tbl_Section s, tbl_Section s1, tbl_Workplace_Total wt where pm.workplaceid = w.workplaceid and w.GMSIWPID = wt.GMSIWPID   \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.Sectionid = s.SectionID and pm.Prodmonth = s.Prodmonth \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s.reporttoSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "'";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s1.ReportToSectionid = '" + cbxBookingMO.EditValue + "') a \r\n ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      left outer join \r\n ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      (select '2' Line,s1.Sectionid sbid, s1.Name sbname, s.Sectionid minid, s.Name minname, pm.workplaceid, w.description, pm.activity, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     case when pm.Activity = 9 then 'Ledge' \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 0 then 'Stope' \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 1 then 'Dev' End as Act, \r\n ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     case when pm.Activity <> 1 then 'a.Stoping' else 'b.Development' end as MainGroup, \r\n ";


            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     pm.FL, pm.SqmTotal plansqm, cyclesqm, orgunitds, 'MO Cycle' aa, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,1,4) a1, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,5,4) a2, SUBSTRING(MOCycle,9,4) a3, SUBSTRING(MOCycle,13,4) a4, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,17,4) a5, SUBSTRING(MOCycle,21,4) a6, SUBSTRING(MOCycle,25,4) a7, SUBSTRING(MOCycle,29,4) a8, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,33,4) a9, SUBSTRING(MOCycle,37,4) a10, SUBSTRING(MOCycle,41,4) a11, SUBSTRING(MOCycle,45,4) a12, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,49,4) a13, SUBSTRING(MOCycle,53,4) a14, SUBSTRING(MOCycle,57,4) a15, SUBSTRING(MOCycle,61,4) a16, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,65,4) a17, SUBSTRING(MOCycle,69,4) a18, SUBSTRING(MOCycle,73,4) a19, SUBSTRING(MOCycle,77,4) a20, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,81,4) a21, SUBSTRING(MOCycle,85,4) a22, SUBSTRING(MOCycle,89,4) a23, SUBSTRING(MOCycle,93,4) a24, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,97,4) a25, SUBSTRING(MOCycle,101,4) a26, SUBSTRING(MOCycle,105,4) a27, SUBSTRING(MOCycle,109,4) a28, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,113,4) a29, SUBSTRING(MOCycle,117,4) a30, SUBSTRING(MOCycle,121,4) a31, SUBSTRING(MOCycle,125,4) a32, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,129,4) a33, SUBSTRING(MOCycle,133,4) a34, SUBSTRING(MOCycle,137,4) a35, SUBSTRING(MOCycle,141,4) a36, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,145,4) a37, SUBSTRING(MOCycle,149,4) a38, SUBSTRING(MOCycle,153,4) a39, SUBSTRING(MOCycle,157,4) a40, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,161,4) a41, SUBSTRING(MOCycle,165,4) a42, SUBSTRING(MOCycle,169,4) a43, SUBSTRING(MOCycle,173,4) a44, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,177,4) a45, s1.ReportToSectionid";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      from tbl_PlanMonth pm, tbl_Workplace w, tbl_section s, tbl_section s1 where pm.workplaceid = w.workplaceid \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.Sectionid = s.SectionID and pm.Prodmonth = s.Prodmonth \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s.reporttoSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s1.ReportToSectionid = '" + cbxBookingMO.EditValue + "') b on a.workplaceid = b.workplaceid and a.activity = b.activity and a.minid = b.minid \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      left outer join \r\n ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      (select '3' Line,s1.Sectionid sbid, s1.Name sbname, s.Sectionid minid, s.Name minname, pm.workplaceid, w.description, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     case when pm.Activity = 9 then 'Ledge' \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 0 then 'Stope' \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 1 then 'Dev' End as Act, \r\n ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      case when pm.Activity <> 1 then 'a.Stoping' else 'b.Development' end as MainGroup, \r\n ";


            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      pm.FL, pm.SqmTotal plansqm, cyclesqm, orgunitds, case when pm.Activity in (0,9) then 'Day Sqm' else 'Day M' end as aa,  \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,1,4) b1, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,5,4) b2, SUBSTRING(MOCycleNum,9,4) b3, SUBSTRING(MOCycleNum,13,4) b4, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,17,4) b5, SUBSTRING(MOCycleNum,21,4) b6, SUBSTRING(MOCycleNum,25,4) b7, SUBSTRING(MOCycleNum,29,4) b8, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,33,4) b9, SUBSTRING(MOCycleNum,37,4) b10, SUBSTRING(MOCycleNum,41,4) b11, SUBSTRING(MOCycleNum,45,4) b12, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,49,4) b13, SUBSTRING(MOCycleNum,53,4) b14, SUBSTRING(MOCycleNum,57,4) b15, SUBSTRING(MOCycleNum,61,4) b16, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,65,4) b17, SUBSTRING(MOCycleNum,69,4) b18, SUBSTRING(MOCycleNum,73,4) b19, SUBSTRING(MOCycleNum,77,4) b20, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,81,4) b21, SUBSTRING(MOCycleNum,85,4) b22, SUBSTRING(MOCycleNum,89,4) b23, SUBSTRING(MOCycleNum,93,4) b24, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,97,4) b25, SUBSTRING(MOCycleNum,101,4) b26, SUBSTRING(MOCycleNum,105,4) b27, SUBSTRING(MOCycleNum,109,4) b28, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,113,4) b29, SUBSTRING(MOCycleNum,117,4) b30, SUBSTRING(MOCycleNum,121,4) b31, SUBSTRING(MOCycleNum,125,4) b32, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,129,4) b33, SUBSTRING(MOCycleNum,133,4) b34, SUBSTRING(MOCycleNum,137,4) b35, SUBSTRING(MOCycleNum,141,4) b36, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,145,4) b37, SUBSTRING(MOCycleNum,149,4) b38, SUBSTRING(MOCycleNum,153,4) b39, SUBSTRING(MOCycleNum,157,4) b40, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,161,4) b41, SUBSTRING(MOCycleNum,165,4) b42, SUBSTRING(MOCycleNum,169,4) b43, SUBSTRING(MOCycleNum,173,4) b44, \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,177,4) b45 , s1.ReportToSectionid, pm.activity  \r\n ";


            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      from tbl_PlanMonth pm, tbl_Workplace w, tbl_section s, tbl_section s1 where pm.workplaceid = w.workplaceid  \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.Sectionid = s.SectionID and pm.Prodmonth = s.Prodmonth \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     and s.reporttoSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     and pm.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     and s1.ReportToSectionid = '" + cbxBookingMO.EditValue.ToString() + "') c on a.Workplaceid = c.workplaceid  and a.activity = c.activity  and a.minid = c.minid \r\n ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , (select 0 act1, max(rand) rr FROM [dbo].[tbl_Rates_Stoping]) d  \r\n  ";



            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " union  \r\n  ";




            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " select   case when MAX(pmold1) is null then 'Red' when MAX(pmold1) is not null and MAX(pmold1) < @prev then 'orange' else '' end as newwpflag, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " Line, sbid, sbname, minid, minname, WorkplaceID,  Description, Act, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " sum(DevTotAdv) DevTotAdv, sum(DevWasteAdv) DevWasteAdv, sum(DevTons) DevTons, sum(DevCont) DevCont, sum(StpTons) StpTons \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , sum(StpCont) StpCont, max(MainGroup) MainGroup, max(FL) FL, convert(int,sum(plansqm)) plansqm \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , convert(int,sum(cyclesqm)) cyclesqm, MAX(orgunitds) orgunitds, MAX(aa)  aa \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(adv) adv, convert(int,sum(tons)) tons, convert(int,sum(content)) content, MAX(offreefsqm) offreefsqm, MAX(Budget) Budget, MAX(activity) activity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(extrabudget) extrabudget, MAX(extrabudget1) extrabudget1 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa1) aa1,MAX(aa2) aa2,MAX(aa3) aa3,MAX(aa4) aa4,MAX(aa5) aa5,MAX(aa6) aa6,MAX(aa7) aa7,MAX(aa8) aa8,MAX(aa9) aa9,MAX(aa10) aa10 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa11) aa11,MAX(aa12) aa12,MAX(aa13) aa13,MAX(aa14) aa14,MAX(aa15) aa15,MAX(aa16) aa16,MAX(aa17) aa17,MAX(aa18) aa18,MAX(aa19) aa19,MAX(aa20) aa20 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa21) aa21,MAX(aa22) aa22,MAX(aa23) aa23,MAX(aa24) aa24,MAX(aa25) aa25,MAX(aa26) aa26,MAX(aa27) aa27,MAX(aa28) aa28,MAX(aa29) aa29,MAX(aa30) aa30 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa31) aa31,MAX(aa32) aa32,MAX(aa33) aa33,MAX(aa34) aa34,MAX(aa35) aa35,MAX(aa36) aa36,MAX(aa37) aa37,MAX(aa38) aa38,MAX(aa39) aa39,MAX(aa40) aa40 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa41) aa41,MAX(aa42) aa42,MAX(aa43) aa43,MAX(aa44) aa44,MAX(aa45) aa45 \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(SectionID) SectionID, MAX(Mprass) Mprass, MAX(wpexternalid) wpexternalid, MAX(Surv) Surv \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(pmold1) newptt, ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 2 Line2, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(sbid1) sbid1, MAX(sbname1) sbname1, MAX(minid1) minid1, MAX(minname1) minname1, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(wp1) wp1,MAX(wd1) wd1, 9 Act1a, MAX(Act1) Act1, \r\n  ";


            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(MainGroup1) MainGroup1,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " sum(FL1) FL1, convert(int,sum(plansqm1)) plansqm1, convert(int,sum(cyclesqma)) cyclesqma , MAX(da) da, MAX(aaa) aaa \r\n  ";


            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a1) a1,MAX(a2) a2,MAX(a3) a3,MAX(a4) a4,MAX(a5) a5,MAX(a6) a6,MAX(a7) a7,MAX(a8) a8,MAX(a9) a9,MAX(a10) a10 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a11) a11,MAX(a12) a12,MAX(a13) a13,MAX(a14) a14,MAX(a15) a15,MAX(a16) a16,MAX(a17) a17,MAX(a18) a18,MAX(a19) a19,MAX(a20) a20 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a21) a21,MAX(a22) a22,MAX(a23) a23,MAX(a24) a24,MAX(a25) a25,MAX(a26) a26,MAX(a27) a27,MAX(a28) a28,MAX(a29) a29,MAX(a30) a30 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a31) a31,MAX(a32) a32,MAX(a33) a33,MAX(a34) a34,MAX(a35) a35,MAX(a36) a36,MAX(a37) a37,MAX(a38) a38,MAX(a39) a39,MAX(a40) a40 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a41) a41,MAX(a42) a42,MAX(a43) a43,MAX(a44) a44,MAX(a45) a45 \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(SectionID) SectionID2, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(Line3) Line3, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(sbid1) sbid2, MAX(sbname1) sbname2, MAX(minid1) minid2, MAX(minname1) minname2, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(wp1) wp2,MAX(wd1) wd2, MAX(Act1) Act2, \r\n  ";


            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(MainGroup1) MainGroup2,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " sum(FL1) FL2, convert(int,sum(plansqm1)) plansqm2, convert(int,sum(cyclesqma)) cyclesqma2 , MAX(da) da2, MAX(aaa) aaa2 \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b1) b1,MAX(b2) b2,MAX(b3) b3,MAX(b4) b4,MAX(b5) b5,MAX(b6) b6,MAX(b7) b7,MAX(b8) b8,MAX(b9) b9,MAX(b10) b10 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b11) b11,MAX(b12) b12,MAX(b13) b13,MAX(b14) b14,MAX(b15) b15,MAX(b16) b16,MAX(b17) b17,MAX(b18) b18,MAX(b19) b19,MAX(b20) b20 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b21) b21,MAX(b22) b22,MAX(b23) b23,MAX(b24) b24,MAX(b25) b25,MAX(b26) b26,MAX(b27) b27,MAX(b28) b28,MAX(b29) b29,MAX(b30) b30 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b31) b31,MAX(b32) b32,MAX(b33) b33,MAX(b34) b34,MAX(b35) b35,MAX(b36) b36,MAX(b37) b37,MAX(b38) b38,MAX(b39) b39,MAX(b40) b40 \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b41) b41,MAX(b42) b42,MAX(b43) b43,MAX(b44) b44,MAX(b45) b45 \r\n  ";




            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,max(mo) mo , 9 ActF, pm, sum(rr) rr \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  from ( \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " select 1 Line, s1.SectionID sbid, s1.Name sbname, s.SectionID minid, s.Name minname, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " v.WorkplaceID, w.Description, 'Vamping' Act, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevTotAdv,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevWasteAdv, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevTons,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevCont,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " plantons StpTons,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " PlanContent StpCont,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'C.Vamping'  MainGroup,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 FL, PlanSqm plansqm, PlanSqm cyclesqm, orgunitds,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'Default Cycle' aa, 0 adv, plantons tons, PlanContent content, 0 offreefsqm, 0 Budget, 9 activity,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 extrabudget, 0 extrabudget1, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa1,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa2, '' aa3, '' aa4,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa5, '' aa6, '' aa7, '' aa8,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa9, '' aa10, '' aa11, '' aa12,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa13, '' aa14,'' aa15, '' aa16,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa17, '' aa18, '' aa19, '' aa20, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa21, '' aa22, '' aa23, '' aa24, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa25, '' aa26, '' aa27, '' aa28, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa29, '' aa30, '' aa31, '' aa32, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa33, '' aa34, '' aa35, '' aa36,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa37, '' aa38, '' aa39, '' aa40,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa41, '' aa42, '' aa43, '' aa44,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa45, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " s2.SectionID, wt.WpSublocation Mprass, wpexternalid, '' Surv, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 2 Line3, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  s1.SectionID sbid1, s1.Name sbname1, s.SectionID minid1, s.Name minname1, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " v.WorkplaceID wp1, w.Description wd1, 'Vamping' Act1, \r\n  ";


            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'C.Vamping'  MainGroup1, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 FL1, PlanSqm plansqm1, PlanSqm cyclesqma, orgunitds da, 'MO Cycle' aaa, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a1, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+1 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+1 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a2, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+2 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+2 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a3, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+3 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+3 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a4, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+4 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+4 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a5, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+5 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+5 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a6, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+6 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+6 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a7, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+7 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+7 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a8, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+8 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+8 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a9, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+9 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+9 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a10, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+10 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+10 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a11, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+11 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+11 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a12, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+12 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+12 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a13, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+13 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+13 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a14, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+14 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+14 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a15, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+15 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+15 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a16, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+16 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+16 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a17, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+17 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+17 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a18, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+18 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+18 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a19, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+19 and workingday = 'Y' then PlanActivity \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+19 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a20, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+20 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+20 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a21, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+21 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+21 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a22, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+22 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+22 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a23, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+23 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+23 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a24, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+24 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+24 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a25, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+25 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+25 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a26, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+26 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+26 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a27, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+27 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+27 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a28, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+28 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+28 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a29, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+29 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+29 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a30, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+30 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+30 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a31, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+31 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+31 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a32, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+32 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+32 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a33, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+33 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+33 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a34, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+34 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+34 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a35, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+35 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+35 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a36, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+36 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+36 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a37, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+37 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+37 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  else '' end as a38, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+38 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+38 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a39, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+39 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+39 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a40, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+40 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+40 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a41, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+41 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+41 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a42, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+42 and workingday = 'Y' then PlanActivity \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+42 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a43, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+43 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+43 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a44, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+44 and workingday = 'Y' then PlanActivity  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+44 and workingday = 'N' then 'OFF' \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a45, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " s2.SectionID ReporttoSectioniD, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 3 Line3a, \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  s1.SectionID sbid3, s1.Name sbname3, s.SectionID minid3, s.Name minname3, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " v.WorkplaceID wp2, w.Description wd2, 'Vamping' Act3,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'C.Vamping'  MainGroupaaa,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 FLz, PlanSqm plansqmz, PlanSqm cyclesqm1, orgunitds ds1, 'Day Sqm' aaz, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b1, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+1 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b2, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+2 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b3, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+3 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm)) \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b4, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+4 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b5, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+5 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b6, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+6 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b7, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+7 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b8, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+8 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b9, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+9 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b10, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+10 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b11, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+11 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b12, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+12 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b13, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+13 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b14, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+14 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b15, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+15 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b16, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+16 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b17, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+17 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b18, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+18 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b19, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+19 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b20, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+20 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b21, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+21 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b22, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+22 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b23, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+23 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b24, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+24 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b25, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+25 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b26, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+26 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b27, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+27 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b28, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+28 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b29, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+29 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b30, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+30 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b31, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+31 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b32, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+32 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b33, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+33 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b34, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+34 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b35, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+35 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b36, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+36 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b37, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+37 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b38, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+38 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b39, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+39 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b40, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+40 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b41, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+41 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b42, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+42and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b43, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+43 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b44, \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+44 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b45,  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " s2.SectionID mo, v.Prodmonth pm, 0 rr, pmold1 \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  from tbl_Planning_Vamping v, tbl_Section s , tbl_Section s1, tbl_Section s2, (select w.*, pmold1 from tbl_Workplace w left outer join  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " (select workplaceid wz, max(prodmonth) pmold1 from tbl_Planning_Vamping  \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  where prodmonth < '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' group by workplaceid) newwp on  w.workplaceid = newwp.wz) w, tbl_Workplace_Total wt \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " where v.Sectionid = s.SectionID and v.Prodmonth = s.Prodmonth   \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth   \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and v.WorkplaceID = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n  ";
            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and v.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and s2.SectionID = '" + cbxBookingMO.EditValue.ToString() + "') a \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  group by Line, sbid, sbname, minid, minname, WorkplaceID,  Description, Act, pm \r\n  ";

            _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " order by  a.sbid,a.MainGroup,a.OrgUnitDS,a.Workplaceid,a.aa1 Desc,a.Line \r\n  ";

            _dbManPlan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManPlan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManPlan.ResultsTableName = "Planning";
            _dbManPlan.ExecuteInstruction();

            DataSet DsPlanning = new DataSet();
            DsPlanning.Tables.Add(_dbManPlan.ResultsDataTable);

            _theReport.RegisterData(DsPlanning);

            _theReport.Load(_reportFolder + "Planning.frx");

            //_theReport.Design();

            pcReport2.OutlineVisible = true;

            pcReport2.Clear();
            _theReport.Prepare();
            _theReport.Preview = pcReport2;
            _theReport.ShowPrepared();
        }

        public void WarSheetReport()
        {
            WarGrid.Visible = false;
            WarGrid.ColumnCount = 1;
            WarGrid.RowCount = 200;
            WarGrid.ColumnCount = 51;
            WarGrid.AllowUserToResizeColumns = true;

            WarGrid.Columns[0].Width = 100;
            WarGrid.Columns[0].HeaderText = "Section";
            WarGrid.Columns[0].ReadOnly = true;
            WarGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            WarGrid.Columns[0].Frozen = true;

            WarGrid.Columns[1].Width = 80;
            WarGrid.Columns[1].HeaderText = "Orgunit";
            WarGrid.Columns[1].ReadOnly = true;
            WarGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            WarGrid.Columns[1].Frozen = true;

            WarGrid.Columns[2].Width = 180;
            WarGrid.Columns[2].HeaderText = "Workplace";
            WarGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            WarGrid.Columns[2].ReadOnly = true;
            WarGrid.Columns[2].Frozen = true;

            WarGrid.Columns[2].Width = 180;
            WarGrid.Columns[2].HeaderText = "FL";
            WarGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            WarGrid.Columns[2].ReadOnly = true;
            WarGrid.Columns[2].Frozen = true;

            int count = 1;
            for (int x = 4; x <= 44; x++)
            {
                WarGrid.Columns[x].Width = 180;
                //WarGrid.Columns[x].HeaderText = "col" + count;
                WarGrid.Columns[x].SortMode = DataGridViewColumnSortMode.NotSortable;
                WarGrid.Columns[x].ReadOnly = true;
                count++;
            }

            WarGrid.Visible = false;

            MWDataManager.clsDataAccess _dbManLosses = new MWDataManager.clsDataAccess();
            _dbManLosses.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManLosses.SqlStatement = "select Max(calendardate) calendardate, MAX(ShiftDay) ShiftDay,MAX(WorkingDay) WorkingDay from tbl_Planning p, tbl_Section s, tbl_Section s1, tbl_Section s2 where " +
                                        " p.Prodmonth = s.Prodmonth and p.Sectionid = s.SectionID and " +
                                        "s.Prodmonth = s1.Prodmonth and s.reporttoSectionid = s1.SectionID and " +
                                        "s1.Prodmonth = s2.Prodmonth and s1.reporttoSectionid = s2.SectionID and " +
                                        "p.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and s2.SectionID  = '" + cbxBookingMO.EditValue.ToString() + "' and p.WorkingDay = 'Y' " +
                                        "group by CalendarDate " +
                                        "order by calendardate ";
            _dbManLosses.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLosses.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbManLosses.ResultsTableName = "Days";  //get table name
            _dbManLosses.ExecuteInstruction();

            DataTable dt = _dbManLosses.ResultsDataTable;
            int col = 4;
            int row = 0;
            int countShift = 0;
            foreach (DataRow dr in dt.Rows)
            {

                WarGrid.Columns[col].HeaderText = Convert.ToDateTime(dr["calendardate"].ToString()).ToString("dd MMM ddd yyyy");
                if (WarGrid.Columns[col].HeaderText != string.Empty)
                {
                    countShift++;
                }
                WarGrid.Rows[row].Cells[col].Value = dr["ShiftDay"].ToString();
                WarGrid.Rows[row + 1].Cells[col].Value = dr["WorkingDay"].ToString();

                col++;
                //row++;
            }
            WarGrid.Columns[3].HeaderText = countShift.ToString();

            MWDataManager.clsDataAccess _dbManCheck = new MWDataManager.clsDataAccess();
            _dbManCheck.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCheck.SqlStatement = "select CheckMeas from tbl_SysSet ";
            _dbManCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
            // _dbManLosses.ResultsTableName = "Days";  //get table name
            _dbManCheck.ExecuteInstruction();

            DataTable dtCheck = _dbManCheck.ResultsDataTable;
            WarGrid.Columns[2].HeaderText = dtCheck.Rows[0]["CheckMeas"].ToString();


            //MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            //_dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //_dbMan1.SqlStatement = "delete from Temp_MoDailyHeading where userid = '" + clsUserInfo.UserID.ToString() + "'     insert into Temp_MoDailyHeading values('" + clsUserInfo.UserID.ToString() + "','" + WarGrid.Columns[0].HeaderText.ToString() + "',  '" + WarGrid.Columns[1].HeaderText.ToString() + "' ,";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + WarGrid.Columns[3].HeaderText.ToString() + "','" + WarGrid.Columns[4].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[5].HeaderText.ToString() + "', '" + WarGrid.Columns[6].HeaderText.ToString() + "','" + WarGrid.Columns[7].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[8].HeaderText.ToString() + "', '" + WarGrid.Columns[9].HeaderText.ToString() + "','" + WarGrid.Columns[10].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[11].HeaderText.ToString() + "', '" + WarGrid.Columns[12].HeaderText.ToString() + "','" + WarGrid.Columns[13].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[14].HeaderText.ToString() + "', '" + WarGrid.Columns[15].HeaderText.ToString() + "','" + WarGrid.Columns[16].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[17].HeaderText.ToString() + "', '" + WarGrid.Columns[18].HeaderText.ToString() + "','" + WarGrid.Columns[19].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[20].HeaderText.ToString() + "', '" + WarGrid.Columns[21].HeaderText.ToString() + "','" + WarGrid.Columns[22].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[23].HeaderText.ToString() + "', '" + WarGrid.Columns[24].HeaderText.ToString() + "','" + WarGrid.Columns[25].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[26].HeaderText.ToString() + "', '" + WarGrid.Columns[27].HeaderText.ToString() + "','" + WarGrid.Columns[28].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[29].HeaderText.ToString() + "', '" + WarGrid.Columns[30].HeaderText.ToString() + "','" + WarGrid.Columns[31].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[32].HeaderText.ToString() + "', '" + WarGrid.Columns[33].HeaderText.ToString() + "','" + WarGrid.Columns[34].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[35].HeaderText.ToString() + "', '" + WarGrid.Columns[36].HeaderText.ToString() + "','" + WarGrid.Columns[37].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[38].HeaderText.ToString() + "', '" + WarGrid.Columns[39].HeaderText.ToString() + "','" + WarGrid.Columns[40].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[41].HeaderText.ToString() + "','" + WarGrid.Columns[42].HeaderText.ToString() + "','" + WarGrid.Columns[43].HeaderText.ToString() + "','" + WarGrid.Columns[44].HeaderText.ToString() + "', '" + WarGrid.Columns[45].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[47].HeaderText.ToString() + "', '" + WarGrid.Columns[48].HeaderText.ToString() + "','" + WarGrid.Columns[49].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[46].HeaderText.ToString() + "','" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(mwPlanPM.EditValue)) + "','" + SectionsCombo.Text.ToString() + "','" + WarGrid.Columns[2].HeaderText.ToString() + "')";

            //_dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbMan1.ResultsTableName = "Headings";
            //_dbMan1.ExecuteInstruction();

            //MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            //_dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //_dbMan1.SqlStatement = "datatable.Rows.Add( '" + clsUserInfo.UserID.ToString() + "' ,'" + WarGrid.Columns[0].HeaderText.ToString() + "',  '" + WarGrid.Columns[1].HeaderText.ToString() + "' ,";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + WarGrid.Columns[3].HeaderText.ToString() + "','" + WarGrid.Columns[4].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[5].HeaderText.ToString() + "', '" + WarGrid.Columns[6].HeaderText.ToString() + "','" + WarGrid.Columns[7].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[8].HeaderText.ToString() + "', '" + WarGrid.Columns[9].HeaderText.ToString() + "','" + WarGrid.Columns[10].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[11].HeaderText.ToString() + "', '" + WarGrid.Columns[12].HeaderText.ToString() + "','" + WarGrid.Columns[13].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[14].HeaderText.ToString() + "', '" + WarGrid.Columns[15].HeaderText.ToString() + "','" + WarGrid.Columns[16].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[17].HeaderText.ToString() + "', '" + WarGrid.Columns[18].HeaderText.ToString() + "','" + WarGrid.Columns[19].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[20].HeaderText.ToString() + "', '" + WarGrid.Columns[21].HeaderText.ToString() + "','" + WarGrid.Columns[22].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[23].HeaderText.ToString() + "', '" + WarGrid.Columns[24].HeaderText.ToString() + "','" + WarGrid.Columns[25].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[26].HeaderText.ToString() + "', '" + WarGrid.Columns[27].HeaderText.ToString() + "','" + WarGrid.Columns[28].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[29].HeaderText.ToString() + "', '" + WarGrid.Columns[30].HeaderText.ToString() + "','" + WarGrid.Columns[31].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[32].HeaderText.ToString() + "', '" + WarGrid.Columns[33].HeaderText.ToString() + "','" + WarGrid.Columns[34].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[35].HeaderText.ToString() + "', '" + WarGrid.Columns[36].HeaderText.ToString() + "','" + WarGrid.Columns[37].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[38].HeaderText.ToString() + "', '" + WarGrid.Columns[39].HeaderText.ToString() + "','" + WarGrid.Columns[40].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[41].HeaderText.ToString() + "','" + WarGrid.Columns[42].HeaderText.ToString() + "','" + WarGrid.Columns[43].HeaderText.ToString() + "','" + WarGrid.Columns[44].HeaderText.ToString() + "', '" + WarGrid.Columns[45].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[47].HeaderText.ToString() + "', '" + WarGrid.Columns[48].HeaderText.ToString() + "','" + WarGrid.Columns[49].HeaderText.ToString() + "' , ";
            //_dbMan1.SqlStatement = _dbMan1.SqlStatement + "'" + WarGrid.Columns[46].HeaderText.ToString() + "','" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(mwPlanPM.EditValue)) + "','" + SectionsCombo.Text.ToString() + "','" + WarGrid.Columns[2].HeaderText.ToString() + "' ";

            //_dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbMan1.ResultsTableName = "Headings";
            //_dbMan1.ExecuteInstruction();

            //DataTable ReportDatasetHeadings = new DataTable();

            //ReportDatasetHeadings.Columns.Add("userid", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Section", typeof(string));
            //ReportDatasetHeadings.Columns.Add("orgunit", typeof(string));
            //ReportDatasetHeadings.Columns.Add("fl", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col1", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col2", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col3", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col4", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col5", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col6", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col7", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col8", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col9", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col10", typeof(string));

            //ReportDatasetHeadings.Columns.Add("Col11", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col12", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col13", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col14", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col15", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col16", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col17", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col18", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col19", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col20", typeof(string));

            //ReportDatasetHeadings.Columns.Add("Col21", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col22", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col23", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col24", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col25", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col26", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col27", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col28", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col29", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col30", typeof(string));

            //ReportDatasetHeadings.Columns.Add("Col31", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col32", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col33", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col34", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col35", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col36", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col37", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col38", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col39", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Col40", typeof(string));

            //ReportDatasetHeadings.Columns.Add("ProgPlan", typeof(string));
            //ReportDatasetHeadings.Columns.Add("ProgBook", typeof(string));
            //ReportDatasetHeadings.Columns.Add("MonthCall", typeof(string));
            //ReportDatasetHeadings.Columns.Add("MonthFC", typeof(string));
            //ReportDatasetHeadings.Columns.Add("MOFC", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Spare", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Spare1", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Color", typeof(string));
            //ReportDatasetHeadings.Columns.Add("Workplace", typeof(string));
            //ReportDatasetHeadings.Columns.Add("RiskRating", typeof(string));

            MWDataManager.clsDataAccess _dbManStopingHeading = new MWDataManager.clsDataAccess();
            _dbManStopingHeading.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManStopingHeading.SqlStatement = "select '" + TUserInfo.UserID.ToString() + "' userid,'" + WarGrid.Columns[0].HeaderText.ToString() + "' section,  '" + WarGrid.Columns[1].HeaderText.ToString() + "' orgunit ," +
             " '" + WarGrid.Columns[3].HeaderText.ToString() + "' fl,'" + WarGrid.Columns[4].HeaderText.ToString() + "' col1 , " +
             "'" + WarGrid.Columns[5].HeaderText.ToString() + "' col2, '" + WarGrid.Columns[6].HeaderText.ToString() + "' col3,'" + WarGrid.Columns[7].HeaderText.ToString() + "' col4, " +
             "'" + WarGrid.Columns[8].HeaderText.ToString() + "' col5, '" + WarGrid.Columns[9].HeaderText.ToString() + "' col6,'" + WarGrid.Columns[10].HeaderText.ToString() + "' col7, " +
             "'" + WarGrid.Columns[11].HeaderText.ToString() + "' col8, '" + WarGrid.Columns[12].HeaderText.ToString() + "' col9,'" + WarGrid.Columns[13].HeaderText.ToString() + "' col10 , " +
             "'" + WarGrid.Columns[14].HeaderText.ToString() + "' col11, '" + WarGrid.Columns[15].HeaderText.ToString() + "' col12,'" + WarGrid.Columns[16].HeaderText.ToString() + "' col13, " +
             "'" + WarGrid.Columns[17].HeaderText.ToString() + "' col14, '" + WarGrid.Columns[18].HeaderText.ToString() + "' col15,'" + WarGrid.Columns[19].HeaderText.ToString() + "' col16, " +
             "'" + WarGrid.Columns[20].HeaderText.ToString() + "' col17, '" + WarGrid.Columns[21].HeaderText.ToString() + "' col18,'" + WarGrid.Columns[22].HeaderText.ToString() + "' col19, " +
             "'" + WarGrid.Columns[23].HeaderText.ToString() + "' col20, '" + WarGrid.Columns[24].HeaderText.ToString() + "' col21,'" + WarGrid.Columns[25].HeaderText.ToString() + "' col22, " +
             "'" + WarGrid.Columns[26].HeaderText.ToString() + "' col23, '" + WarGrid.Columns[27].HeaderText.ToString() + "' col24,'" + WarGrid.Columns[28].HeaderText.ToString() + "' col25, " +
             "'" + WarGrid.Columns[29].HeaderText.ToString() + "' col26, '" + WarGrid.Columns[30].HeaderText.ToString() + "' col27,'" + WarGrid.Columns[31].HeaderText.ToString() + "' col28, " +
             "'" + WarGrid.Columns[32].HeaderText.ToString() + "' col29, '" + WarGrid.Columns[33].HeaderText.ToString() + "' col30,'" + WarGrid.Columns[34].HeaderText.ToString() + "' col31, " +
             "'" + WarGrid.Columns[35].HeaderText.ToString() + "' col32, '" + WarGrid.Columns[36].HeaderText.ToString() + "' col33,'" + WarGrid.Columns[37].HeaderText.ToString() + "' col34, " +
             "'" + WarGrid.Columns[38].HeaderText.ToString() + "' col35, '" + WarGrid.Columns[39].HeaderText.ToString() + "' col36,'" + WarGrid.Columns[40].HeaderText.ToString() + "' col37, " +
             "'" + WarGrid.Columns[41].HeaderText.ToString() + "' col38,'" + WarGrid.Columns[42].HeaderText.ToString() + "' col39,'" + WarGrid.Columns[43].HeaderText.ToString() + "' col40,'" + WarGrid.Columns[44].HeaderText.ToString() + "' ProgPlan, '" + WarGrid.Columns[45].HeaderText.ToString() + "' ProgBook, " +
             "'" + WarGrid.Columns[47].HeaderText.ToString() + "' MonthCall, '" + WarGrid.Columns[48].HeaderText.ToString() + "' MonthFC,'" + WarGrid.Columns[49].HeaderText.ToString() + "' Spare, " +
             "'" + WarGrid.Columns[46].HeaderText.ToString() + "' Spare1,'" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' Color,'" + cbxBookingMO.EditValue.ToString() + "' Workplace,'" + WarGrid.Columns[2].HeaderText.ToString() + "' RiskRating ";

            //////////////////////////////////////////////////////////////////////////////////////////
            //MWDataManager.clsDataAccess _dbManStopingHeading = new MWDataManager.clsDataAccess();
            //_dbManStopingHeading.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //_dbManStopingHeading.SqlStatement = "select   " +
            //                    " * from Temp_MoDailyHeading where userid = '" + clsUserInfo.UserID + "' ";
            _dbManStopingHeading.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManStopingHeading.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManStopingHeading.ResultsTableName = "MODaily_Stoping_Headings";
            _dbManStopingHeading.ExecuteInstruction();

            DataSet ReportDatasetHeadings = new DataSet();
            ReportDatasetHeadings.Tables.Add(_dbManStopingHeading.ResultsDataTable);
            _theReport.RegisterData(ReportDatasetHeadings);



            MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
            _dbManData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //_dbManData.SqlStatement =   "Select Userid,a.* from ( \r\n" +

            //                            "select '" + ProductionAmplatsGlobalTSysSettings._Banner.ToString() + "' banner, *, case when Activity <> 1 then fl1 else fl1/(totblast+0.001) end as fl from ( \r\n" +
            //                            "select 0 PlanPrimM,p.SqmTotal,p.Activity, case when p.activity <> 1 then convert(decimal(7,0), p.fl) else convert(decimal(7,1), p.Adv) end as fl1,p.prodmonth,s2.SectionID moid, s2.Name monmae, s1.SectionID sbid, s1.Name sbname,s.SectionID minerID,s.Name MinerName, OrgUnitDS, \r\n" +
            //                            "p.workplaceid, w.Description wpname, \r\n" +

            //                            "case when endtypeid IN(9,15) then 'C1' \r\n" +
            //                            "when endtypeid not IN(9,15) then 'B1' \r\n" +
            //                            "when p.Activity  <> 1 then 'A1' end as order1, replace(p.MOCycle,'OFF ','') + '                                         ' MOCycle, p.MOCycleNum \r\n" +
            //                            "from tbl_PlanMonth p,tbl_Workplace w, tbl_Section s, tbl_Section s1, tbl_Section s2 \r\n" +
            //                            "where p.workplaceid = w.workplaceid and p.Prodmonth = s.Prodmonth and p.Sectionid = s.SectionID and \r\n" +
            //                            "s.Prodmonth = s1.Prodmonth and s.reporttoSectionid = s1.SectionID and \r\n" +
            //                            "s1.Prodmonth = s2.Prodmonth and s1.reporttoSectionid = s2.SectionID \r\n" +

            //                            "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(mwPlanPM.EditValue)) + "' and s2.SectionID = '" + luePlanMOSection.EditValue.ToString() + "' and p.Tons > 0) a \r\n" +

            //                            "left outer join  \r\n" +
            //                            "(select wp, sec, SUM(totblast) totblast from ( \r\n" +
            //                            "select WorkplaceID wp, p.sectionid sec,  case when MOCycle in ('BL', 'BRL', 'SUBL') then 1 else 0 end as totblast from tbl_Planning p, tbl_Section s, tbl_Section s1, tbl_Section s2 \r\n" +
            //                            "where p.Prodmonth = s.Prodmonth and p.Sectionid = s.SectionID \r\n" +
            //                            "and s.Prodmonth = s1.Prodmonth and s.reporttoSectionid = s1.SectionID  \r\n" +
            //                            "and s1.Prodmonth = s2.Prodmonth and s1.reporttoSectionid = s2.SectionID  \r\n" +
            //                            "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(mwPlanPM.EditValue)) + "' and s2.SectionID = '" + luePlanMOSection.EditValue.ToString() + "' and p.Tons > 0) a group by wp, sec) b  \r\n" +
            //                            "on a.Workplaceid = b.wp and a.minerID = b.sec \r\n" +

            //                            " ) a \r\n" +

            //                            "left outer join( \r\n" +
            //                            "Select * from[dbo].[tbl_Booking_Audit] curr, \r\n" +
            //                            "(select max(thedate) maxThedate, Workplaceid  wp2, Sectionid sec2 from[tbl_Booking_Audit] group by workplaceid, Sectionid) maxdd \r\n" +
            //                            "where curr.TheDate = maxdd.maxThedate and curr.Workplaceid = maxdd.wp2 and curr.Sectionid = maxdd.sec2 \r\n" +

            //                            ") c \r\n" +

            //                            "on a.Workplaceid = c.Workplaceid and a.minerID = c.Sectionid \r\n" +
            //                            "order by order1, moid, sbid, OrgUnitDS, wpname ";

            _dbManData.SqlStatement = "Select case when Userid is NULL then '' else Userid end as BookUserID ,a.* From (   \r\n" +

                                        "select '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, *, case when Activity <> 1 then fl1 else fl1/(totblast+0.001) end as fl from ( \r\n" +
                                        "select 0 PlanPrimM,p.SqmTotal,p.Activity, case when p.activity <> 1 then convert(decimal(7,0), p.fl) else convert(decimal(7,1), p.Adv) end as fl1,p.prodmonth,s2.SectionID moid, s2.Name monmae, s1.SectionID sbid, s1.Name sbname,s.SectionID minerID,s.Name MinerName, OrgUnitDS, \r\n" +
                                        "p.workplaceid, w.Description wpname, \r\n" +

                                        "case when endtypeid IN(9,15) then 'C1' \r\n" +
                                        "when endtypeid not IN(9,15) then 'B1' \r\n" +
                                        "when p.Activity  <> 1 then 'A1' end as order1, replace(p.MOCycle,'OFF ','') + '                                         ' MOCycle, p.MOCycleNum \r\n" +
                                        "from tbl_PlanMonth p,tbl_Workplace w, tbl_Section s, tbl_Section s1, tbl_Section s2 \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.Prodmonth = s.Prodmonth and p.Sectionid = s.SectionID and \r\n" +
                                        "s.Prodmonth = s1.Prodmonth and s.reporttoSectionid = s1.SectionID and \r\n" +
                                        "s1.Prodmonth = s2.Prodmonth and s1.reporttoSectionid = s2.SectionID \r\n" +

                                        "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and s2.SectionID = '" + cbxBookingMO.EditValue.ToString() + "' and p.Tons > 0) a \r\n" +

                                        "left outer join  \r\n" +
                                        "(select wp, sec, SUM(totblast) totblast from ( \r\n" +
                                        "select WorkplaceID wp, p.sectionid sec,  case when MOCycle in ('BL', 'BRL', 'SUBL') then 1 else 0 end as totblast from tbl_Planning p, tbl_Section s, tbl_Section s1, tbl_Section s2 \r\n" +
                                        "where p.Prodmonth = s.Prodmonth and p.Sectionid = s.SectionID \r\n" +
                                        "and s.Prodmonth = s1.Prodmonth and s.reporttoSectionid = s1.SectionID  \r\n" +
                                        "and s1.Prodmonth = s2.Prodmonth and s1.reporttoSectionid = s2.SectionID  \r\n" +
                                        "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and s2.SectionID = '" + cbxBookingMO.EditValue.ToString() + "' and p.Tons > 0) a group by wp, sec) b  \r\n" +
                                        "on a.Workplaceid = b.wp and a.minerID = b.sec \r\n" +

                                        ") a \r\n" +
                                        "left outer join( \r\n" +
                                        "Select *, Squaremetres sqm from[dbo].[tbl_Booking_Audit] curr, ( \r\n" +
                                        "Select min(b.TheDate) maxThedate, a.Workplaceid wp2, b.SectionID sec2 from( \r\n" +
                                        //"Select * from tbl_planning p \r\n" +
                                        //"where p.calendardate = convert(varchar, getdate()-1, 23) )a, ( \r\n" +
                                        //"Select * from[tbl_Booking_Audit] where BookDate = convert(varchar, getdate()-1, 23) )b \r\n" +
                                        //"where a.CalendarDate = b.BookDate and a.BookSqm = b.Squaremetres and a.WorkplaceID = b.Workplaceid \r\n" +
                                        //"group by  a.Workplaceid,b.Sectionid ) maxdd where curr.TheDate = maxdd.maxThedate and curr.Workplaceid = maxdd.wp2 and curr.Sectionid = maxdd.sec2 \r\n" +
                                        //") c \r\n" +

                                        //"on a.Workplaceid = c.Workplaceid and a.minerID = c.Sectionid \r\n" +
                                        //"order by order1, moid, sbid, OrgUnitDS, wpname ";
                                        "Select * from tbl_planning p \r\n" +
                                        "where \r\n" +
                                        "--p.calendardate = convert(varchar, getdate() - 1, 23) \r\n" +
                                        "p.calendardate = DATEADD(DAY, CASE DATENAME(WEEKDAY, GETDATE()) WHEN 'Sunday' THEN - 2 WHEN 'Monday' THEN - 3 ELSE - 1 END, DATEDIFF(DAY, 0, GETDATE())) \r\n" +
                                        ")a, ( \r\n" +
                                        "Select * from[tbl_Booking_Audit] where BookDate = DATEADD(DAY, CASE DATENAME(WEEKDAY, GETDATE()) WHEN 'Sunday' THEN - 2 WHEN 'Monday' THEN - 3 ELSE - 1 END, DATEDIFF(DAY, 0, GETDATE())) \r\n" +
                                        "--BookDate = convert(varchar, getdate() - 1, 23) \r\n" +
                                        ")b \r\n" +
                                        //"where a.CalendarDate = b.BookDate and a.BookSqm = b.Squaremetres and a.WorkplaceID = b.Workplaceid \r\n" +
                                        "where a.CalendarDate = b.BookDate and (a.BookSqm = b.Squaremetres or a.Bookadv = b.MetresAdvance) and a.WorkplaceID = b.Workplaceid \r\n" +
                                        "group by  a.Workplaceid,b.Sectionid ) maxdd where curr.TheDate = maxdd.maxThedate and curr.Workplaceid = maxdd.wp2 and curr.Sectionid = maxdd.sec2 \r\n" +
                                        ") c \r\n" +
                                        "on a.Workplaceid = c.Workplaceid and a.minerID = c.Sectionid \r\n" +
                                        "order by order1, moid, sbid, OrgUnitDS, wpname ";

            _dbManData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManData.ResultsTableName = "WallChart";  //get table name
            _dbManData.ExecuteInstruction();



            DataSet ReportDatasetLosses = new DataSet();
            ReportDatasetLosses.Tables.Add(_dbManData.ResultsDataTable);
            _theReport.RegisterData(ReportDatasetLosses);

            _theReport.Load(_reportFolder + "WallRoom.frx");


            //_theReport.Design();

            pcReport2.Clear();
            _theReport.Prepare();
            _theReport.Preview = pcReport2;
            _theReport.ShowPrepared();
            pcReport2.Visible = true;


        }

        //public void LoadKPI()
        //{
        //    this.Cursor = Cursors.WaitCursor;

        //    Label OldLbl = new Label();
        //    Label NewLbl = new Label();
        //    Label newstplbl = new Label();



        //    //tabPage1.Text = "Graphs";

        //    //CycleTab.Visible = false;

        //    MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
        //    _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbMan.SqlStatement = " select * from (select MAX(prodmonth) zz from tbl_Planning where CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "') a ";
        //    _dbMan.SqlStatement = _dbMan.SqlStatement + " , ";
        //    _dbMan.SqlStatement = _dbMan.SqlStatement + " (select MAX(prodmonth) zzold from tbl_Planning where CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "' ";
        //    _dbMan.SqlStatement = _dbMan.SqlStatement + " and Prodmonth <> (select MAX(prodmonth) zz from tbl_Planning where CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "')) b ";
        //    _dbMan.SqlStatement = _dbMan.SqlStatement + "    ";
        //    _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbMan.ExecuteInstruction();


        //    DataTable SubA = _dbMan.ResultsDataTable;

        //    OldLbl.Text = SubA.Rows[0]["zzold"].ToString();
        //    NewLbl.Text = SubA.Rows[0]["zz"].ToString();


        //    MWDataManager.clsDataAccess _dbMana1 = new MWDataManager.clsDataAccess();
        //    _dbMana1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbMana1.SqlStatement = " select * from (select MAX(prodmonth) zz from tbl_Planning where activity <> 1 and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "') a ";
        //    _dbMana1.SqlStatement = _dbMana1.SqlStatement + " , ";
        //    _dbMana1.SqlStatement = _dbMana1.SqlStatement + " (select MAX(prodmonth) zzold from tbl_Planning where activity <> 1 and  CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "' ";
        //    _dbMana1.SqlStatement = _dbMana1.SqlStatement + " and Prodmonth <> (select MAX(prodmonth) zz from tbl_Planning where activity <> 1 and  CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "')) b ";
        //    _dbMana1.SqlStatement = _dbMana1.SqlStatement + "    ";
        //    _dbMana1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbMana1.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbMana1.ExecuteInstruction();


        //    DataTable SubA1 = _dbMana1.ResultsDataTable;


        //    newstplbl.Text = SubA1.Rows[0]["zz"].ToString();

        //    //Procedures procs = new Procedures();
        //    //procs.ProdMonthCalc(Convert.ToInt32(OldLbl.Text));
        //    ProductionGlobal.ProductionGlobal.ProdMonthCalc(Convert.ToInt32(OldLbl.Text));
        //    //OldLbl.Text = Procedures.Prod.ToString();
        //    //procs.ProdMonthVis(Convert.ToInt32(OldLbl.Text));
        //    ProductionGlobal.ProductionGlobal.ProdMonthCalc(Convert.ToInt32(OldLbl.Text));

        //    tabPage6.Text = "Previous Production Month  " + Procedures.Prod2;
        //    lab1.Text = Procedures.Prod2;


        //    //procs.ProdMonthCalc(Convert.ToInt32(NewLbl.Text));
        //    ProductionGlobal.ProductionGlobal.ProdMonthCalc(Convert.ToInt32(NewLbl.Text));
        //    //OldLbl.Text = Procedures.Prod.ToString();
        //    //procs.ProdMonthVis(Convert.ToInt32(NewLbl.Text));
        //    ProductionGlobal.ProductionGlobal.ProdMonthCalc(Convert.ToInt32(NewLbl.Text));

        //    tabPage7.Text = "Current Production Month  " + Procedures.Prod2;
        //    lab2.Text = Procedures.Prod2;

        //    tabPage8.Text = "Summary";

        //    //get old

        //    MWDataManager.clsDataAccess _dbManold = new MWDataManager.clsDataAccess();
        //    _dbManold.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManold.SqlStatement = " exec dbo.KPI_Stoping '" + OldLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "', '" + ProductionAmplatsGlobalTSysSettings._Banner.ToString() + "' ";
        //    _dbManold.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManold.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManold.ResultsTableName = "Previous";  //get table name
        //    _dbManold.ExecuteInstruction();

        //    DataSet ReportDatasetold = new DataSet();
        //    ReportDatasetold.Tables.Add(_dbManold.ResultsDataTable);
        //    _theReport.RegisterData(ReportDatasetold);

        //    MWDataManager.clsDataAccess _dbManoldDev = new MWDataManager.clsDataAccess();
        //    _dbManoldDev.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManoldDev.SqlStatement = " exec dbo.KPI_Dev '" + OldLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "' ";
        //    _dbManoldDev.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManoldDev.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManoldDev.ResultsTableName = "PreviousDev";  //get table name
        //    _dbManoldDev.ExecuteInstruction();

        //    DataSet ReportDatasetoldDev = new DataSet();
        //    ReportDatasetoldDev.Tables.Add(_dbManoldDev.ResultsDataTable);
        //    _theReport.RegisterData(ReportDatasetoldDev);

        //    _theReport.Load("SICold.frx");

        //    //theReport.Design();

        //    PrevPrvCntrl.Clear();
        //    _theReport.Prepare();
        //    _theReport.Preview = PrevPrvCntrl;
        //    _theReport.ShowPrepared();


        //    MWDataManager.clsDataAccess _dbManold1 = new MWDataManager.clsDataAccess();
        //    _dbManold1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManold1.SqlStatement = " exec dbo.KPI_Stoping '" + NewLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "', '" + ProductionAmplatsGlobalTSysSettings._Banner.ToString() + "'  ";
        //    _dbManold1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManold1.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManold1.ResultsTableName = "Previous";  //get table name
        //    _dbManold1.ExecuteInstruction();

        //    DataSet ReportDatasetold1 = new DataSet();
        //    ReportDatasetold1.Tables.Add(_dbManold1.ResultsDataTable);
        //    _theReport.RegisterData(ReportDatasetold1);


        //    MWDataManager.clsDataAccess _dbManoldDev1 = new MWDataManager.clsDataAccess();
        //    _dbManoldDev1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManoldDev1.SqlStatement = " exec dbo.KPI_Dev '" + NewLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "' ";
        //    _dbManoldDev1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManoldDev1.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManoldDev1.ResultsTableName = "PreviousDev";  //get table name
        //    _dbManoldDev1.ExecuteInstruction();

        //    DataSet ReportDatasetoldDev1 = new DataSet();
        //    ReportDatasetoldDev1.Tables.Add(_dbManoldDev1.ResultsDataTable);
        //    _theReport.RegisterData(ReportDatasetoldDev1);

        //    _theReport.Load("SICnew.frx");

        //    //_theReport.Design();

        //    CurrentPrevCntrl.Clear();
        //    _theReport.Prepare();
        //    _theReport.Preview = CurrentPrevCntrl;
        //    _theReport.ShowPrepared();




        //    MWDataManager.clsDataAccess _dbManoldSum = new MWDataManager.clsDataAccess();
        //    _dbManoldSum.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManoldSum.SqlStatement = " exec dbo.KPI_Sum  '" + NewLbl.Text + "', '" + OldLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "', '" + ProductionAmplatsGlobalTSysSettings._Banner.ToString() + "' ";
        //    _dbManoldSum.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManoldSum.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManoldSum.ResultsTableName = "Sum";  //get table name
        //    _dbManoldSum.ExecuteInstruction();

        //    DataSet ReportDatasetoldSum = new DataSet();
        //    ReportDatasetoldSum.Tables.Add(_dbManoldSum.ResultsDataTable);
        //    _theReport.RegisterData(ReportDatasetoldSum);


        //    MWDataManager.clsDataAccess _dbManSafetyold = new MWDataManager.clsDataAccess();
        //    _dbManSafetyold.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    if (ProductionAmplatsGlobalTSysSettings._Banner.ToString() != "Mponeng")
        //    {
        //        _dbManSafetyold.SqlStatement = " select '" + lab1.Text + "' lbl, [Inj], count([Inj]) num from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from tbl_sysset) group by [Inj] ";
        //    }
        //    else
        //    {
        //        _dbManSafetyold.SqlStatement = " select '" + lab1.Text + "' lbl,  'All Injuries'  Inj,  count([Inj]) num  from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] " +
        //                                       "where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from tbl_sysset) and inj not in ('Fatality') " +
        //                                       "union " +
        //                                       "select '" + lab1.Text + "' lbl,  'Lost Time Injury'  Inj,  count([Inj]) num  from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main]  " +
        //                                       "where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from tbl_sysset) and inj not in ('Fatality', 'Dressing Case') " +
        //                                       "union " +
        //                                       "select '" + lab1.Text + "' lbl,  'Serious Injury'  Inj,  count([Inj]) num  from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] " +
        //                                       "where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from tbl_sysset) and inj not in ('Fatality', 'Dressing Case','LTI (Lost time injury)') ";
        //        //  "union " +
        //        //  "select '" + lab1.Text + "' lbl,  'Fatality'  Inj,  count([Inj]) num  from [MW_PassStageDB].dbo.[SafetyFigures_Main]  " +
        //        //  "where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from sysset) and inj in ('Fatality') ";


        //    }
        //    _dbManSafetyold.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManSafetyold.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManSafetyold.ResultsTableName = "SumSafetyOld";  //get table name
        //    _dbManSafetyold.ExecuteInstruction();

        //    DataSet ReportDatasetSafetyOld = new DataSet();
        //    ReportDatasetSafetyOld.Tables.Add(_dbManSafetyold.ResultsDataTable);
        //    _theReport.RegisterData(ReportDatasetSafetyOld);


        //    MWDataManager.clsDataAccess _dbManSafetyold1 = new MWDataManager.clsDataAccess();
        //    _dbManSafetyold1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManSafetyold1.SqlStatement = " select * from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS  from tbl_sysset) and [Inj] = 'LTI (Lost time injury)'  ";
        //    _dbManSafetyold1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManSafetyold1.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManSafetyold1.ResultsTableName = "SumSafetyOld1";  //get table name
        //    _dbManSafetyold1.ExecuteInstruction();

        //    DataSet ReportDatasetSafetyOld1 = new DataSet();
        //    ReportDatasetSafetyOld1.Tables.Add(_dbManSafetyold1.ResultsDataTable);
        //    _theReport.RegisterData(ReportDatasetSafetyOld1);

        //    MWDataManager.clsDataAccess _dbManSafetyold2 = new MWDataManager.clsDataAccess();
        //    _dbManSafetyold2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManSafetyold2.SqlStatement = " select '" + lab2.Text + "' lbl, [Inj], count([Inj]) num from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] where themonth = '" + NewLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS  from tbl_sysset) group by [Inj]  ";
        //    _dbManSafetyold2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManSafetyold2.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManSafetyold2.ResultsTableName = "SumSafetyOld2";  //get table name
        //    _dbManSafetyold2.ExecuteInstruction();

        //    DataSet ReportDatasetSafetyOld2 = new DataSet();
        //    ReportDatasetSafetyOld2.Tables.Add(_dbManSafetyold2.ResultsDataTable);
        //    _theReport.RegisterData(ReportDatasetSafetyOld2);



        //    _theReport.Load(_reportFolder + "SICSum.frx");

        //    //_theReport.Design();

        //    SumPrevCntrl.Clear();
        //    _theReport.Prepare();
        //    _theReport.Preview = SumPrevCntrl;
        //    _theReport.ShowPrepared();

        //    // get millmonth
        //    MWDataManager.clsDataAccess _dbManSqm1 = new MWDataManager.clsDataAccess();
        //    _dbManSqm1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManSqm1.SqlStatement = " select * from tbl_CalendarMill where enddate >= '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "' and startdate <= '" + String.Format("{0:yyyy-MM-dd}", TheDate.Value) + "'  ";
        //    _dbManSqm1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManSqm1.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManSqm1.ResultsTableName = "SumSqm11111";  //get table name
        //    _dbManSqm1.ExecuteInstruction();

        //    if (_dbManSqm1.ResultsDataTable.Rows.Count == 0)
        //    {
        //        MessageBox.Show("No Mill month created please contact the system admin");
        //        return;

        //    }

        //    millmonth = _dbManSqm1.ResultsDataTable.Rows[0]["millmonth"].ToString();
        //    startdate = Convert.ToDateTime(_dbManSqm1.ResultsDataTable.Rows[0][2].ToString());
        //    enddate = Convert.ToDateTime(_dbManSqm1.ResultsDataTable.Rows[0][3].ToString());

        //    MWDataManager.clsDataAccess _dbManSqm = new MWDataManager.clsDataAccess();
        //    _dbManSqm.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManSqm.SqlStatement = " select '" + millmonth + "' mmill, '" + ProductionAmplatsGlobalTSysSettings._Banner.ToString() + "' banner1, a.*, case when plansqm > 0 then plansqmcmgt/plansqm else 0 end as plancmgt, " +
        //                               "case when onreefsqm > 0 then Bookcmgt/onreefsqm else onreefsqm end as bookcmgt1  " +
        //                               " from (select SUM(sqm) plansqm, SUM(booksqm+AdjSqm) booksqm,  SUM(sqm*Pm.cmgt) plansqmcmgt, sum((convert(decimal(18,0),(OnReefFL*bookadv))+AdjSqm))  onreefsqm, " +
        //                               " sum((convert(decimal(18,0),(OnReefFL*bookadv))+AdjSqm)*pm.CMGT)  Bookcmgt," +
        //                               "CalendarDate from tbl_Planning p   " +
        //                               ", tbl_PlanMonth pm  " +
        //                               "where p.prodmonth = pm.prodmonth and p.workplaceid = pm.workplaceid and p.sectionid = pm.sectionid and p.activity = pm.activity and CalendarDate >= (  " +
        //                               "select StartDate  from tbl_CalendarMill where MillMonth = '" + millmonth + "') and CalendarDate <= ( " +


        //                               "select EndDate from tbl_CalendarMill where MillMonth = '" + millmonth + "') group by CalendarDate) a " +
        //                               //"left outer join MILLING m on a.CalendarDate = m.CalendarDate "+



        //                               "order by a.CalendarDate  ";
        //    _dbManSqm.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManSqm.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManSqm.ResultsTableName = "SumSqm";  //get table name
        //    _dbManSqm.ExecuteInstruction();

        //    DataSet ReportDatasetSqm = new DataSet();
        //    ReportDatasetSqm.Tables.Add(_dbManSqm.ResultsDataTable);
        //    theReportSum1.RegisterData(ReportDatasetSqm);


        //    // get factors
        //    MWDataManager.clsDataAccess _dbManSqm2 = new MWDataManager.clsDataAccess();
        //    _dbManSqm2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManSqm2.SqlStatement = " select a.*, isnull(m.actualtons,0) actualtons, isnull(m.concdesp,0) concdesp  from ( " +
        //                                "select *, isnull(onreefsqm * tomill,0) tons, " +
        //                             " isnull(BookGrams*dens*OffReefFact/100*MCF/100* REC/100,0) cont,  " +
        //                              " case when  onreefsqm * tomill > 0 then  (BookGrams*dens*OffReefFact/100*MCF/100* REC/100)/ " +
        //                              " (onreefsqm * tomill)*1000 else 0 end as calpulpval " +
        //                              "  from  " +
        //                              " (select CalendarDate DD , isnull(sum((convert(decimal(18,0),(OnReefFL*bookadv))+AdjSqm)),0) onreefsqm, " +
        //                              " isnull(sum((convert(decimal(18,0),(OnReefFL*bookadv))+AdjSqm)*pm.CMGT/100/1000),0) BookGrams " +
        //                              " from tbl_Planning p, tbl_PlanMonth pm " +
        //                              " where  " +
        //                              " p.Prodmonth = pm.Prodmonth and p.SectionID = pm.SectionID and p.WorkplaceID = pm.Workplaceid and p.Activity = pm.Activity and " +

        //                              " CalendarDate+10 >= ( " +

        //                              " select StartDate  from tbl_CalendarMill " +
        //                              " where MillMonth = '" + millmonth + "') and CalendarDate <=  " +
        //                              " ( " +

        //                              " select EndDate+10 from tbl_CalendarMill " +
        //                              " where MillMonth = '" + millmonth + "') group by CalendarDate) a " +
        //                             " , " +
        //                             " ( select * from tbl_SYSSET_MonthlyFactors   where prodmonth = '" + millmonth + "') b " +
        //                              " , (select max(dens) dens from tbl_PlanMonth where Prodmonth = '" + millmonth + "') c " +
        //                              ") a left outer join tbl_MILLING m on a.DD = m.CalendarDate " +

        //                              " order by a.DD ";
        //    _dbManSqm2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManSqm2.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManSqm2.ResultsTableName = "SumSqm11111";  //get table name
        //    _dbManSqm2.ExecuteInstruction();

        //    //SqmToMill.Text = _dbManSqm2.ResultsDataTable.Rows[0]["tomill"].ToString(); ;


        //    Emp = _dbManSqm2.ResultsDataTable;
        //    int xx = Emp.Rows.Count;

        //    bs.DataSource = Emp;


        //    LoadGraph();


        //    // do oreflow
        //    MWDataManager.clsDataAccess _dbManGetISAfterStart = new MWDataManager.clsDataAccess();
        //    _dbManGetISAfterStart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManGetISAfterStart.SqlStatement = " select * from tbl_SYSSET_MonthlyFactors where Prodmonth = '" + millmonth + "'  ";
        //    _dbManGetISAfterStart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManGetISAfterStart.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManGetISAfterStart.ExecuteInstruction();

        //    DataTable dtSections = _dbManGetISAfterStart.ResultsDataTable;

        //    //cmbSections.Items.Clear();

        //    foreach (DataRow dr in dtSections.Rows)
        //    {
        //        TramFactlbl.Text = Math.Round(Convert.ToDecimal(dr["tram"].ToString()), 0).ToString(); //  cmbSections.Items.Add(dr["sectionid"].ToString() + ":" + dr["name"].ToString());
        //    }



        //    LoadGrid("R");
        //    LoadGrid_Graph();



        //    Stoping.Visible = false;

        //    Report _theReport = new Report();

        //    MWDataManager.clsDataAccess _dbManMill = new MWDataManager.clsDataAccess();
        //    _dbManMill.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbManMill.SqlStatement = "select case when a.Heading = 'Plan Tons' then 1 " +
        //                          "when a.Heading = 'Book Tons' then 2 " +
        //                          "when a.Heading = 'Tram Tons' then 3   " +
        //                           "when a.Heading = 'Smart Rail' then 4   " +
        //                          "when a.Heading = 'Hoist Tons' then 5  " +
        //                          "else 6 end as q, * from  " +
        //                          "(select * from tbl_Temp_OreFlow_Reef_Data where userid = '" + clsUserInfo.UserID + "') a ";
        //    _dbManMill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbManMill.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbManMill.ResultsTableName = "Reef_Data";
        //    _dbManMill.ExecuteInstruction();


        //    string lbl1 = "";
        //    string lbl2 = "Progressive";

        //    //if (AdjRadio.Checked == true)
        //    lbl1 = "Using Tramming Width " + TramFactlbl.Text + "cm";


        //    MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
        //    _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbMan1.SqlStatement = "select '" + lbl1 + "' lbl, * from tbl_Temp_OreFlow_Reef_Heading where userid = '" + clsUserInfo.UserID + "'";
        //    _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbMan1.ResultsTableName = "Reef_Heading";
        //    _dbMan1.ExecuteInstruction();

        //    MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
        //    _dbMan2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbMan2.SqlStatement =
        //    " select '" + lbl2 + "' llbl, '" + 'R' + "' Type, '" + millmonth + "' MillMonth, *, " +
        //    "case when progbook = '' then null else progbook end as progbook1, " +
        //    "case when progtram = '' then null else progtram end as progtram1, " +
        //    "case when proghoist = '' then null else proghoist end as proghoist1, " +
        //    "case when progmill = '' then null else progmill end as progmill1, " +
        //    "case when progmill = '' then null else progplan end as progplan1, " +

        //    "case when progsrtons = '' then null else progsrtons end as progsrtons1, " +

        //    " case when substring(calendardate,1,3) in ('01', '15') then substring(calendardate,1,3) + ' ' +  " +
        //    " substring(calendardate,3,4) " +
        //    " else substring(calendardate,1,3) end as lbl " +
        //    " from tbl_Temp_OreFlow_Data_Graph where userid = '" + clsUserInfo.UserID + "' order by xxxx";
        //    _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbMan2.ResultsTableName = "Graph_Data";
        //    _dbMan2.ExecuteInstruction();


        //    MWDataManager.clsDataAccess _dbMan3 = new MWDataManager.clsDataAccess();
        //    _dbMan3.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
        //    _dbMan3.SqlStatement = "";

        //    //  if (GraphradioGroup.SelectedIndex == 0)
        //    //  {

        //    _dbMan3.SqlStatement = _dbMan3.SqlStatement + " select max(progplan1) progplan1, max(progbook1) progbook1, max(progtram1) progtram1 ";
        //    _dbMan3.SqlStatement = _dbMan3.SqlStatement + ", max(proghoist1) proghoist1, max(progmill1) progmill1 ";
        //    // }
        //    // else
        //    // {
        //    //     _dbMan3.SqlStatement = _dbMan3.SqlStatement + " select sum(progplan1) progplan1, sum(progbook1) progbook1, sum(progtram1) progtram1 ";
        //    //     _dbMan3.SqlStatement = _dbMan3.SqlStatement + ", sum(proghoist1) proghoist1, sum(progmill1) progmill1 ";


        //    //  }

        //    _dbMan3.SqlStatement = _dbMan3.SqlStatement + "from (select '" + 'R' + "' Type, '" + millmonth + "' MillMonth, *, " +
        //    "case when progbook = '' then 0.0 else convert(numeric,progbook) end as progbook1, " +
        //    "case when progtram = '' then 0.0 else convert(numeric,progtram) end as progtram1, " +
        //    "case when proghoist = '' then 0.0 else convert(numeric,proghoist) end as proghoist1, " +
        //    "case when progmill = '' then 0.0 else convert(numeric,progmill) end as progmill1, " +
        //    "case when progmill = '' then 0.0 else convert(numeric,progplan) end as progplan1, " +

        //    " case when substring(calendardate,1,3) in ('01', '15') then substring(calendardate,1,3) + ' ' +  " +
        //    " substring(calendardate,3,4) " +
        //    " else substring(calendardate,1,3) end as lbl " +
        //    " from tbl_Temp_OreFlow_Data_Graph where userid = '" + clsUserInfo.UserID + "') a";
        //    _dbMan3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbMan3.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbMan3.ResultsTableName = "Graph_Data1";
        //    _dbMan3.ExecuteInstruction();



        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(_dbManMill.ResultsDataTable);
        //    DataSet ds1 = new DataSet();
        //    ds1.Tables.Add(_dbMan1.ResultsDataTable);

        //    DataSet ds2 = new DataSet();
        //    ds2.Tables.Add(_dbMan2.ResultsDataTable);
        //    DataSet ds3 = new DataSet();

        //    DataSet ds4 = new DataSet();
        //    ds4.Tables.Add(_dbMan3.ResultsDataTable);


        //    if (_dbMan.ResultsDataTable.Rows.Count < 1)
        //    {
        //        MessageBox.Show("There is no data for selected criteria");
        //        return;
        //    }

        //    _theReport.RegisterData(ds);
        //    _theReport.RegisterData(ds1);
        //    _theReport.RegisterData(ds2);
        //    _theReport.RegisterData(ds4);

        //    _theReport.Load("OreFlow.frx");

        //    //_theReport.Design();


        //    _theReport.Prepare();
        //    _theReport.Preview = Oreflowpc;
        //    _theReport.ShowPrepared();



        //    // top20
        //    LoadTop20();



        //    this.Cursor = Cursors.Default;
        //    CycleTab.Visible = true;

        //}

        /// <summary>
        /// Load the sections from DB
        /// </summary>
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

            

            lookupEditSection.DataSource = tbl_Sections;
            lookupEditSection.DisplayMember = "Name_2";
            lookupEditSection.ValueMember = "Sectionid_2";

            cbxBookingMO.EditValue = tbl_Sections.Rows[0]["Sectionid_2"].ToString();
        }


        #endregion Methods 

        private void btnLoad_Click(object sender, EventArgs e)
        {
            MainLoad();
        }

        private void ucPlanningReport_Leave(object sender, EventArgs e)
        {

        }

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_FirstLoad == "N")
                MainLoad();
        }

        private void cbxBookingMO_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void cbxBookingMO_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
                MainLoad();
            
        }

        private void cbxBookingProdMonth_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void radioActivity_EditValueChanged(object sender, EventArgs e)
        {
            if (radioActivity.EditValue.ToString() == "3")
            {
                if (_FirstLoad == "N")
                    MainLoad();
            }
            if (radioActivity.EditValue.ToString() == "0")
            {
                MWDataManager.clsDataAccess _dbManBanner = new MWDataManager.clsDataAccess();
                _dbManBanner.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManBanner.SqlStatement = " select 'Amandelbult', '" + cbxBookingMO.EditValue.ToString() + "', '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "', '" + cbxBookingMO.EditValue + "'  ";

                _dbManBanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManBanner.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManBanner.ResultsTableName = "Banner";
                _dbManBanner.ExecuteInstruction();


                DataSet DsBanner = new DataSet();
                DsBanner.Tables.Add(_dbManBanner.ResultsDataTable);

                _theReport.RegisterData(DsBanner);

                //_theReport.Load("Planning.frx");


                MWDataManager.clsDataAccess _dbManDate = new MWDataManager.clsDataAccess();
                _dbManDate.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " select *, DATEPART(ISOWK,BeginDate) ww  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " , DATEPART(ISOWK,BeginDate+7) ww1   ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+14) ww2  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+21) ww3  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+28) ww4  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+35) ww5  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+42) ww6  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+49) ww7  ";

                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " from (select Min(s.BeginDate) BeginDate,MAX(s.EndDate) EndDate from tbl_Code_Calendar_Section s  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " left outer join tbl_Section sc ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " on s.Sectionid = sc.SectionID ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " where s.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and sc.ReportToSectionid = '" + cbxBookingMO.EditValue + "') a ";
                _dbManDate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDate.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDate.ResultsTableName = "sysset";
                _dbManDate.ExecuteInstruction();

                //TimeSpan Span;
                DateTime BeginDate;
                DateTime EndDate;
                DataTable CalDate = new DataTable();
                int Day = 0;
                int Week = 0;
                int Weekno = 0;

                BeginDate = Convert.ToDateTime(_dbManDate.ResultsDataTable.Rows[0][0].ToString());
                EndDate = Convert.ToDateTime(_dbManDate.ResultsDataTable.Rows[0][1].ToString());
                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][2].ToString());

                CalDate.Rows.Add();

                for (int x = 0; x <= 45; x++)
                {
                    if (BeginDate.AddDays(Day) <= EndDate)
                    {

                        CalDate.Columns.Add();
                        CalDate.Rows[0][x] = BeginDate.AddDays(Day).ToString("dd MMM ddd");
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Mon")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                W";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Tue")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                E";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Wed")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                E";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Thu")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                K";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Fri")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "               -";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Sat")
                        {
                            // do first wwk
                            if (Weekno == 0)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][2].ToString());

                            if (Weekno == 1)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][3].ToString());

                            if (Weekno == 2)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][4].ToString());

                            if (Weekno == 3)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][5].ToString());

                            if (Weekno == 4)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][6].ToString());
                            if (Weekno == 5)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][7].ToString());
                            if (Weekno == 6)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][8].ToString());


                            // CalDate.Rows[0][x] = CalDate.Rows[0][x] + "        ";
                            if (Week >= 5000)
                            {
                                //Week = 1;

                                if (Week > 9)
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(0, 1);

                                }
                                else
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + " " + "         0";

                                }
                            }
                            else
                            {
                                if (Week > 9)
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(0, 1);

                                }
                                else
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + " " + "         0";

                                }
                            }



                        }

                        if (BeginDate.AddDays(Day).ToString("ddd") == "Sun")
                        {
                            if (Weekno == 0)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][2].ToString());

                            if (Weekno == 1)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][3].ToString());

                            if (Weekno == 2)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][4].ToString());

                            if (Weekno == 3)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][5].ToString());

                            if (Weekno == 4)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][6].ToString());
                            if (Weekno == 5)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][7].ToString());
                            if (Weekno == 6)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][8].ToString());

                            // CalDate.Rows[0][x] = CalDate.Rows[0][x] + "        ";
                            if (Week >= 54000)
                            {
                                // Week = 1;

                                if (Week > 9)
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(1, 1);
                                    //Week = Week + 1;

                                }
                                else
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString();
                                    //Week = Week + 1;

                                }
                            }
                            else
                            {
                                if (Week > 9)
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(1, 1);
                                    // Week = Week + 1;

                                }
                                else
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString();
                                    //Week = Week + 1;

                                }
                            }

                            Weekno = Weekno + 1;

                        }


                        // Weekno = Weekno + 1;

                        Day = Day + 1;


                    }
                    else
                    {
                        CalDate.Columns.Add();
                        CalDate.Rows[0][x] = string.Empty;
                    }

                }

                CalDate.Columns.Add();
                CalDate.Rows[0][CalDate.Columns.Count - 1] = Day.ToString();

                CalDate.TableName = "CalDates";
                DataSet DsCalDate = new DataSet();
                DsCalDate.Tables.Add(CalDate);

                _theReport.RegisterData(DsCalDate);


                MWDataManager.clsDataAccess _dbManPlan = new MWDataManager.clsDataAccess();
                _dbManPlan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "declare @Start datetime ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "set @Start = ( ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "select  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "min(calendardate) ss from tbl_Planning_Vamping v, tbl_section s , tbl_section s1, tbl_section s2 ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "where v.Sectionid = s.SectionID and v.Prodmonth = s.Prodmonth  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth   ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " and v.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and s2.SectionID = '" + cbxBookingMO.EditValue.ToString() + "')  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  declare @prev integer ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " set @prev = ( select max(prodmonth) aaaa from tbl_planmonth where prodmonth < '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "') ";



                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " select  case when newptt is null then 'Red' when newptt is not null and newptt < @prev then 'orange' else '' end as newwpflag, * from ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  (select '1' Line,s1.Sectionid sbid, s1.Name sbname, s.Sectionid minid, s.Name minname, pm.workplaceid, case when pm.activity = 9 then w.description+' Ledge' else w.description end as description , \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    case when pm.Activity = 9 then 'Ledge' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "   when pm.Activity = 0 then 'Stope' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "   when pm.Activity = 1 then 'Dev' End as Act, \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity = 1 then Adv else 0 end as DevTotAdv,  \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity = 1 and w.ReefWaste = 'W' then Adv else 0 end as DevWasteAdv, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity = 1 then Tons else 0 end as DevTons, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when  pm.Activity = 1 then Content else 0 end as DevCont,  \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity <> 1 then Tons else 0 end as StpTons, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity <> 1 then Content else 0 end as StpCont,  \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    case when pm.Activity <> 1 then 'a.Stoping' else 'b.Development' end as MainGroup, \r\n ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    pm.FL, pm.SqmTotal plansqm, cyclesqm, orgunitds, 'Default Cycle' aa, pm.adv, tons, content, offreefsqm, Budget, pm.activity,   \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  case when pm.activity <> 1 then pm.extrabudget else 0 end as extrabudget, case when pm.activity = 1 then pm.extrabudget else 0 end as extrabudget1,  SUBSTRING(DefaultCycle,1,4) aa1, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,5,4) aa2, SUBSTRING(DefaultCycle,9,4) aa3, SUBSTRING(DefaultCycle,13,4) aa4, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,17,4) aa5, SUBSTRING(DefaultCycle,21,4) aa6, SUBSTRING(DefaultCycle,25,4) aa7, SUBSTRING(DefaultCycle,29,4) aa8, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,33,4) aa9, SUBSTRING(DefaultCycle,37,4) aa10, SUBSTRING(DefaultCycle,41,4) aa11, SUBSTRING(DefaultCycle,45,4) aa12, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,49,4) aa13, SUBSTRING(DefaultCycle,53,4) aa14, SUBSTRING(DefaultCycle,57,4) aa15, SUBSTRING(DefaultCycle,61,4) aa16, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,65,4) aa17, SUBSTRING(DefaultCycle,69,4) aa18, SUBSTRING(DefaultCycle,73,4) aa19, SUBSTRING(DefaultCycle,77,4) aa20, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,81,4) aa21, SUBSTRING(DefaultCycle,85,4) aa22, SUBSTRING(DefaultCycle,89,4) aa23, SUBSTRING(DefaultCycle,93,4) aa24, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,97,4) aa25, SUBSTRING(DefaultCycle,101,4) aa26, SUBSTRING(DefaultCycle,105,4) aa27, SUBSTRING(DefaultCycle,109,4) aa28, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,113,4) aa29, SUBSTRING(DefaultCycle,117,4) aa30, SUBSTRING(DefaultCycle,121,4) aa31, SUBSTRING(DefaultCycle,125,4) aa32, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,129,4) aa33, SUBSTRING(DefaultCycle,133,4) aa34, SUBSTRING(DefaultCycle,137,4) aa35, SUBSTRING(DefaultCycle,141,4) aa36, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,145,4) aa37, SUBSTRING(DefaultCycle,149,4) aa38, SUBSTRING(DefaultCycle,153,4) aa39, SUBSTRING(DefaultCycle,157,4) aa40, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,161,4) aa41, SUBSTRING(DefaultCycle,165,4) aa42, SUBSTRING(DefaultCycle,169,4) aa43, SUBSTRING(DefaultCycle,173,4) aa44, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,177,4) aa45, s1.ReportToSectionid, wt.WpSublocation Mprass, wpexternalid, case when substring(comments,1,3) = 'Sur' then 'Survey Zero Plan' else '' end as Surv ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , pmold1 newptt ";



                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      from tbl_PlanMonth pm, (select w.*, pmold1 from tbl_Workplace w left outer join ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     (select workplaceid wz, max(prodmonth) pmold1 from tbl_PlanMonth ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     where prodmonth < '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' group by workplaceid) newwp on  w.workplaceid = newwp.wz) w, tbl_Section s, tbl_Section s1, tbl_Workplace_Total wt where pm.workplaceid = w.workplaceid and w.GMSIWPID = wt.GMSIWPID   \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.Sectionid = s.SectionID and pm.Prodmonth = s.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s.reporttoSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "'";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s1.ReportToSectionid = '" + cbxBookingMO.EditValue + "') a \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      left outer join \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      (select '2' Line,s1.Sectionid sbid, s1.Name sbname, s.Sectionid minid, s.Name minname, pm.workplaceid, w.description, pm.activity, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     case when pm.Activity = 9 then 'Ledge' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 0 then 'Stope' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 1 then 'Dev' End as Act, \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     case when pm.Activity <> 1 then 'a.Stoping' else 'b.Development' end as MainGroup, \r\n ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     pm.FL, pm.SqmTotal plansqm, cyclesqm, orgunitds, 'MO Cycle' aa, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,1,4) a1, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,5,4) a2, SUBSTRING(MOCycle,9,4) a3, SUBSTRING(MOCycle,13,4) a4, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,17,4) a5, SUBSTRING(MOCycle,21,4) a6, SUBSTRING(MOCycle,25,4) a7, SUBSTRING(MOCycle,29,4) a8, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,33,4) a9, SUBSTRING(MOCycle,37,4) a10, SUBSTRING(MOCycle,41,4) a11, SUBSTRING(MOCycle,45,4) a12, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,49,4) a13, SUBSTRING(MOCycle,53,4) a14, SUBSTRING(MOCycle,57,4) a15, SUBSTRING(MOCycle,61,4) a16, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,65,4) a17, SUBSTRING(MOCycle,69,4) a18, SUBSTRING(MOCycle,73,4) a19, SUBSTRING(MOCycle,77,4) a20, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,81,4) a21, SUBSTRING(MOCycle,85,4) a22, SUBSTRING(MOCycle,89,4) a23, SUBSTRING(MOCycle,93,4) a24, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,97,4) a25, SUBSTRING(MOCycle,101,4) a26, SUBSTRING(MOCycle,105,4) a27, SUBSTRING(MOCycle,109,4) a28, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,113,4) a29, SUBSTRING(MOCycle,117,4) a30, SUBSTRING(MOCycle,121,4) a31, SUBSTRING(MOCycle,125,4) a32, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,129,4) a33, SUBSTRING(MOCycle,133,4) a34, SUBSTRING(MOCycle,137,4) a35, SUBSTRING(MOCycle,141,4) a36, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,145,4) a37, SUBSTRING(MOCycle,149,4) a38, SUBSTRING(MOCycle,153,4) a39, SUBSTRING(MOCycle,157,4) a40, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,161,4) a41, SUBSTRING(MOCycle,165,4) a42, SUBSTRING(MOCycle,169,4) a43, SUBSTRING(MOCycle,173,4) a44, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,177,4) a45, s1.ReportToSectionid";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      from tbl_PlanMonth pm, tbl_Workplace w, tbl_section s, tbl_section s1 where pm.workplaceid = w.workplaceid \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.Sectionid = s.SectionID and pm.Prodmonth = s.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s.reporttoSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s1.ReportToSectionid = '" + cbxBookingMO.EditValue + "') b on a.workplaceid = b.workplaceid and a.activity = b.activity and a.minid = b.minid \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      left outer join \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      (select '3' Line,s1.Sectionid sbid, s1.Name sbname, s.Sectionid minid, s.Name minname, pm.workplaceid, w.description, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     case when pm.Activity = 9 then 'Ledge' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 0 then 'Stope' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 1 then 'Dev' End as Act, \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      case when pm.Activity <> 1 then 'a.Stoping' else 'b.Development' end as MainGroup, \r\n ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      pm.FL, pm.SqmTotal plansqm, cyclesqm, orgunitds, case when pm.Activity in (0,9) then 'Day Sqm' else 'Day M' end as aa,  \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,1,4) b1, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,5,4) b2, SUBSTRING(MOCycleNum,9,4) b3, SUBSTRING(MOCycleNum,13,4) b4, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,17,4) b5, SUBSTRING(MOCycleNum,21,4) b6, SUBSTRING(MOCycleNum,25,4) b7, SUBSTRING(MOCycleNum,29,4) b8, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,33,4) b9, SUBSTRING(MOCycleNum,37,4) b10, SUBSTRING(MOCycleNum,41,4) b11, SUBSTRING(MOCycleNum,45,4) b12, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,49,4) b13, SUBSTRING(MOCycleNum,53,4) b14, SUBSTRING(MOCycleNum,57,4) b15, SUBSTRING(MOCycleNum,61,4) b16, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,65,4) b17, SUBSTRING(MOCycleNum,69,4) b18, SUBSTRING(MOCycleNum,73,4) b19, SUBSTRING(MOCycleNum,77,4) b20, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,81,4) b21, SUBSTRING(MOCycleNum,85,4) b22, SUBSTRING(MOCycleNum,89,4) b23, SUBSTRING(MOCycleNum,93,4) b24, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,97,4) b25, SUBSTRING(MOCycleNum,101,4) b26, SUBSTRING(MOCycleNum,105,4) b27, SUBSTRING(MOCycleNum,109,4) b28, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,113,4) b29, SUBSTRING(MOCycleNum,117,4) b30, SUBSTRING(MOCycleNum,121,4) b31, SUBSTRING(MOCycleNum,125,4) b32, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,129,4) b33, SUBSTRING(MOCycleNum,133,4) b34, SUBSTRING(MOCycleNum,137,4) b35, SUBSTRING(MOCycleNum,141,4) b36, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,145,4) b37, SUBSTRING(MOCycleNum,149,4) b38, SUBSTRING(MOCycleNum,153,4) b39, SUBSTRING(MOCycleNum,157,4) b40, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,161,4) b41, SUBSTRING(MOCycleNum,165,4) b42, SUBSTRING(MOCycleNum,169,4) b43, SUBSTRING(MOCycleNum,173,4) b44, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,177,4) b45 , s1.ReportToSectionid, pm.activity  \r\n ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      from tbl_PlanMonth pm, tbl_Workplace w, tbl_section s, tbl_section s1 where pm.workplaceid = w.workplaceid  \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.Sectionid = s.SectionID and pm.Prodmonth = s.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     and s.reporttoSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     and pm.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     and s1.ReportToSectionid = '" + cbxBookingMO.EditValue.ToString() + "') c on a.Workplaceid = c.workplaceid  and a.activity = c.activity  and a.minid = c.minid \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , (select 0 act1, max(rand) rr FROM [dbo].[tbl_Rates_Stoping]) d  \r\n  ";



                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " union  \r\n  ";




                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " select   case when MAX(pmold1) is null then 'Red' when MAX(pmold1) is not null and MAX(pmold1) < @prev then 'orange' else '' end as newwpflag, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " Line, sbid, sbname, minid, minname, WorkplaceID,  Description, Act, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " sum(DevTotAdv) DevTotAdv, sum(DevWasteAdv) DevWasteAdv, sum(DevTons) DevTons, sum(DevCont) DevCont, sum(StpTons) StpTons \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , sum(StpCont) StpCont, max(MainGroup) MainGroup, max(FL) FL, convert(int,sum(plansqm)) plansqm \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , convert(int,sum(cyclesqm)) cyclesqm, MAX(orgunitds) orgunitds, MAX(aa)  aa \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(adv) adv, convert(int,sum(tons)) tons, convert(int,sum(content)) content, MAX(offreefsqm) offreefsqm, MAX(Budget) Budget, MAX(activity) activity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(extrabudget) extrabudget, MAX(extrabudget1) extrabudget1 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa1) aa1,MAX(aa2) aa2,MAX(aa3) aa3,MAX(aa4) aa4,MAX(aa5) aa5,MAX(aa6) aa6,MAX(aa7) aa7,MAX(aa8) aa8,MAX(aa9) aa9,MAX(aa10) aa10 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa11) aa11,MAX(aa12) aa12,MAX(aa13) aa13,MAX(aa14) aa14,MAX(aa15) aa15,MAX(aa16) aa16,MAX(aa17) aa17,MAX(aa18) aa18,MAX(aa19) aa19,MAX(aa20) aa20 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa21) aa21,MAX(aa22) aa22,MAX(aa23) aa23,MAX(aa24) aa24,MAX(aa25) aa25,MAX(aa26) aa26,MAX(aa27) aa27,MAX(aa28) aa28,MAX(aa29) aa29,MAX(aa30) aa30 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa31) aa31,MAX(aa32) aa32,MAX(aa33) aa33,MAX(aa34) aa34,MAX(aa35) aa35,MAX(aa36) aa36,MAX(aa37) aa37,MAX(aa38) aa38,MAX(aa39) aa39,MAX(aa40) aa40 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa41) aa41,MAX(aa42) aa42,MAX(aa43) aa43,MAX(aa44) aa44,MAX(aa45) aa45 \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(SectionID) SectionID, MAX(Mprass) Mprass, MAX(wpexternalid) wpexternalid, MAX(Surv) Surv \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(pmold1) newptt, ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 2 Line2, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(sbid1) sbid1, MAX(sbname1) sbname1, MAX(minid1) minid1, MAX(minname1) minname1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(wp1) wp1,MAX(wd1) wd1, 9 Act1a, MAX(Act1) Act1, \r\n  ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(MainGroup1) MainGroup1,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " sum(FL1) FL1, convert(int,sum(plansqm1)) plansqm1, convert(int,sum(cyclesqma)) cyclesqma , MAX(da) da, MAX(aaa) aaa \r\n  ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a1) a1,MAX(a2) a2,MAX(a3) a3,MAX(a4) a4,MAX(a5) a5,MAX(a6) a6,MAX(a7) a7,MAX(a8) a8,MAX(a9) a9,MAX(a10) a10 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a11) a11,MAX(a12) a12,MAX(a13) a13,MAX(a14) a14,MAX(a15) a15,MAX(a16) a16,MAX(a17) a17,MAX(a18) a18,MAX(a19) a19,MAX(a20) a20 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a21) a21,MAX(a22) a22,MAX(a23) a23,MAX(a24) a24,MAX(a25) a25,MAX(a26) a26,MAX(a27) a27,MAX(a28) a28,MAX(a29) a29,MAX(a30) a30 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a31) a31,MAX(a32) a32,MAX(a33) a33,MAX(a34) a34,MAX(a35) a35,MAX(a36) a36,MAX(a37) a37,MAX(a38) a38,MAX(a39) a39,MAX(a40) a40 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a41) a41,MAX(a42) a42,MAX(a43) a43,MAX(a44) a44,MAX(a45) a45 \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(SectionID) SectionID2, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(Line3) Line3, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(sbid1) sbid2, MAX(sbname1) sbname2, MAX(minid1) minid2, MAX(minname1) minname2, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(wp1) wp2,MAX(wd1) wd2, MAX(Act1) Act2, \r\n  ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(MainGroup1) MainGroup2,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " sum(FL1) FL2, convert(int,sum(plansqm1)) plansqm2, convert(int,sum(cyclesqma)) cyclesqma2 , MAX(da) da2, MAX(aaa) aaa2 \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b1) b1,MAX(b2) b2,MAX(b3) b3,MAX(b4) b4,MAX(b5) b5,MAX(b6) b6,MAX(b7) b7,MAX(b8) b8,MAX(b9) b9,MAX(b10) b10 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b11) b11,MAX(b12) b12,MAX(b13) b13,MAX(b14) b14,MAX(b15) b15,MAX(b16) b16,MAX(b17) b17,MAX(b18) b18,MAX(b19) b19,MAX(b20) b20 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b21) b21,MAX(b22) b22,MAX(b23) b23,MAX(b24) b24,MAX(b25) b25,MAX(b26) b26,MAX(b27) b27,MAX(b28) b28,MAX(b29) b29,MAX(b30) b30 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b31) b31,MAX(b32) b32,MAX(b33) b33,MAX(b34) b34,MAX(b35) b35,MAX(b36) b36,MAX(b37) b37,MAX(b38) b38,MAX(b39) b39,MAX(b40) b40 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b41) b41,MAX(b42) b42,MAX(b43) b43,MAX(b44) b44,MAX(b45) b45 \r\n  ";




                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,max(mo) mo , 9 ActF, pm, sum(rr) rr \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  from ( \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " select 1 Line, s1.SectionID sbid, s1.Name sbname, s.SectionID minid, s.Name minname, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " v.WorkplaceID, w.Description, 'Vamping' Act, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevTotAdv,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevWasteAdv, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevTons,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevCont,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " plantons StpTons,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " PlanContent StpCont,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'C.Vamping'  MainGroup,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 FL, PlanSqm plansqm, PlanSqm cyclesqm, orgunitds,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'Default Cycle' aa, 0 adv, plantons tons, PlanContent content, 0 offreefsqm, 0 Budget, 9 activity,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 extrabudget, 0 extrabudget1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa1,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa2, '' aa3, '' aa4,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa5, '' aa6, '' aa7, '' aa8,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa9, '' aa10, '' aa11, '' aa12,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa13, '' aa14,'' aa15, '' aa16,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa17, '' aa18, '' aa19, '' aa20, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa21, '' aa22, '' aa23, '' aa24, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa25, '' aa26, '' aa27, '' aa28, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa29, '' aa30, '' aa31, '' aa32, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa33, '' aa34, '' aa35, '' aa36,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa37, '' aa38, '' aa39, '' aa40,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa41, '' aa42, '' aa43, '' aa44,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa45, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " s2.SectionID, wt.WpSublocation Mprass, wpexternalid, '' Surv, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 2 Line3, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  s1.SectionID sbid1, s1.Name sbname1, s.SectionID minid1, s.Name minname1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " v.WorkplaceID wp1, w.Description wd1, 'Vamping' Act1, \r\n  ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'C.Vamping'  MainGroup1, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 FL1, PlanSqm plansqm1, PlanSqm cyclesqma, orgunitds da, 'MO Cycle' aaa, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+1 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+1 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a2, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+2 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+2 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a3, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+3 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+3 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a4, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+4 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+4 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a5, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+5 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+5 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a6, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+6 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+6 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a7, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+7 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+7 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a8, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+8 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+8 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a9, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+9 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+9 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a10, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+10 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+10 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a11, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+11 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+11 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a12, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+12 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+12 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a13, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+13 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+13 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a14, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+14 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+14 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a15, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+15 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+15 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a16, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+16 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+16 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a17, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+17 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+17 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a18, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+18 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+18 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a19, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+19 and workingday = 'Y' then PlanActivity \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+19 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a20, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+20 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+20 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a21, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+21 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+21 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a22, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+22 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+22 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a23, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+23 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+23 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a24, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+24 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+24 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a25, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+25 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+25 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a26, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+26 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+26 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a27, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+27 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+27 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a28, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+28 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+28 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a29, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+29 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+29 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a30, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+30 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+30 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a31, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+31 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+31 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a32, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+32 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+32 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a33, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+33 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+33 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a34, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+34 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+34 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a35, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+35 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+35 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a36, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+36 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+36 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a37, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+37 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+37 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  else '' end as a38, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+38 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+38 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a39, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+39 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+39 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a40, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+40 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+40 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a41, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+41 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+41 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a42, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+42 and workingday = 'Y' then PlanActivity \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+42 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a43, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+43 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+43 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a44, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+44 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+44 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a45, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " s2.SectionID ReporttoSectioniD, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 3 Line3a, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  s1.SectionID sbid3, s1.Name sbname3, s.SectionID minid3, s.Name minname3, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " v.WorkplaceID wp2, w.Description wd2, 'Vamping' Act3,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'C.Vamping'  MainGroupaaa,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 FLz, PlanSqm plansqmz, PlanSqm cyclesqm1, orgunitds ds1, 'Day Sqm' aaz, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+1 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b2, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+2 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b3, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+3 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm)) \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b4, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+4 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b5, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+5 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b6, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+6 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b7, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+7 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b8, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+8 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b9, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+9 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b10, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+10 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b11, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+11 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b12, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+12 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b13, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+13 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b14, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+14 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b15, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+15 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b16, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+16 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b17, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+17 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b18, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+18 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b19, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+19 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b20, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+20 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b21, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+21 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b22, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+22 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b23, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+23 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b24, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+24 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b25, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+25 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b26, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+26 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b27, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+27 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b28, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+28 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b29, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+29 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b30, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+30 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b31, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+31 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b32, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+32 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b33, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+33 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b34, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+34 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b35, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+35 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b36, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+36 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b37, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+37 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b38, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+38 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b39, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+39 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b40, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+40 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b41, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+41 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b42, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+42and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b43, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+43 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b44, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+44 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b45,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " s2.SectionID mo, v.Prodmonth pm, 0 rr, pmold1 \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  from tbl_Planning_Vamping v, tbl_Section s , tbl_Section s1, tbl_Section s2, (select w.*, pmold1 from tbl_Workplace w left outer join  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " (select workplaceid wz, max(prodmonth) pmold1 from tbl_Planning_Vamping  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  where prodmonth < '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' group by workplaceid) newwp on  w.workplaceid = newwp.wz) w, tbl_Workplace_Total wt \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " where v.Sectionid = s.SectionID and v.Prodmonth = s.Prodmonth   \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth   \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and v.WorkplaceID = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and v.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and s2.SectionID = '" + cbxBookingMO.EditValue.ToString() + "') a \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  group by Line, sbid, sbname, minid, minname, WorkplaceID,  Description, Act, pm \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " order by  a.sbid,a.MainGroup,a.OrgUnitDS,a.Workplaceid,a.aa1 Desc,a.Line \r\n  ";

                _dbManPlan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPlan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPlan.ResultsTableName = "Planning";
                _dbManPlan.ExecuteInstruction();

                DataSet DsPlanning = new DataSet();
                DsPlanning.Tables.Add(_dbManPlan.ResultsDataTable);

                _theReport.RegisterData(DsPlanning);

                _theReport.Load(_reportFolder + "PlanningStoping.frx");

                //_theReport.Design();

                pcReport2.OutlineVisible = true;

                pcReport2.Clear();
                _theReport.Prepare();
                _theReport.Preview = pcReport2;
                _theReport.ShowPrepared();
            }
            if (radioActivity.EditValue.ToString() == "1")
            {
                MWDataManager.clsDataAccess _dbManBanner = new MWDataManager.clsDataAccess();
                _dbManBanner.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManBanner.SqlStatement = " select 'Amandelbult', '" + cbxBookingMO.EditValue.ToString() + "', '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "', '" + cbxBookingMO.EditValue + "'  ";

                _dbManBanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManBanner.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManBanner.ResultsTableName = "Banner";
                _dbManBanner.ExecuteInstruction();


                DataSet DsBanner = new DataSet();
                DsBanner.Tables.Add(_dbManBanner.ResultsDataTable);

                _theReport.RegisterData(DsBanner);

                //_theReport.Load("Planning.frx");


                MWDataManager.clsDataAccess _dbManDate = new MWDataManager.clsDataAccess();
                _dbManDate.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " select *, DATEPART(ISOWK,BeginDate) ww  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " , DATEPART(ISOWK,BeginDate+7) ww1   ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+14) ww2  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+21) ww3  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+28) ww4  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+35) ww5  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+42) ww6  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + "  , DATEPART(ISOWK,BeginDate+49) ww7  ";

                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " from (select Min(s.BeginDate) BeginDate,MAX(s.EndDate) EndDate from tbl_Code_Calendar_Section s  ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " left outer join tbl_Section sc ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " on s.Sectionid = sc.SectionID ";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " where s.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and sc.ReportToSectionid = '" + cbxBookingMO.EditValue + "') a ";
                _dbManDate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDate.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDate.ResultsTableName = "sysset";
                _dbManDate.ExecuteInstruction();

                TimeSpan Span;
                DateTime BeginDate;
                DateTime EndDate;
                DataTable CalDate = new DataTable();
                int Day = 0;
                int Week = 0;
                int Weekno = 0;

                BeginDate = Convert.ToDateTime(_dbManDate.ResultsDataTable.Rows[0][0].ToString());
                EndDate = Convert.ToDateTime(_dbManDate.ResultsDataTable.Rows[0][1].ToString());
                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][2].ToString());

                CalDate.Rows.Add();

                for (int x = 0; x <= 45; x++)
                {
                    if (BeginDate.AddDays(Day) <= EndDate)
                    {

                        CalDate.Columns.Add();
                        CalDate.Rows[0][x] = BeginDate.AddDays(Day).ToString("dd MMM ddd");
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Mon")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                W";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Tue")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                E";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Wed")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                E";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Thu")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "                K";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Fri")
                        {
                            CalDate.Rows[0][x] = CalDate.Rows[0][x] + "               -";
                        }
                        if (BeginDate.AddDays(Day).ToString("ddd") == "Sat")
                        {
                            // do first wwk
                            if (Weekno == 0)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][2].ToString());

                            if (Weekno == 1)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][3].ToString());

                            if (Weekno == 2)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][4].ToString());

                            if (Weekno == 3)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][5].ToString());

                            if (Weekno == 4)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][6].ToString());
                            if (Weekno == 5)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][7].ToString());
                            if (Weekno == 6)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][8].ToString());


                            // CalDate.Rows[0][x] = CalDate.Rows[0][x] + "        ";
                            if (Week >= 5000)
                            {
                                //Week = 1;

                                if (Week > 9)
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(0, 1);

                                }
                                else
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + " " + "         0";

                                }
                            }
                            else
                            {
                                if (Week > 9)
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(0, 1);

                                }
                                else
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + " " + "         0";

                                }
                            }



                        }

                        if (BeginDate.AddDays(Day).ToString("ddd") == "Sun")
                        {
                            if (Weekno == 0)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][2].ToString());

                            if (Weekno == 1)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][3].ToString());

                            if (Weekno == 2)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][4].ToString());

                            if (Weekno == 3)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][5].ToString());

                            if (Weekno == 4)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][6].ToString());
                            if (Weekno == 5)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][7].ToString());
                            if (Weekno == 6)
                                Week = Convert.ToInt32(_dbManDate.ResultsDataTable.Rows[0][8].ToString());

                            // CalDate.Rows[0][x] = CalDate.Rows[0][x] + "        ";
                            if (Week >= 54000)
                            {
                                // Week = 1;

                                if (Week > 9)
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(1, 1);
                                    //Week = Week + 1;

                                }
                                else
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString();
                                    //Week = Week + 1;

                                }
                            }
                            else
                            {
                                if (Week > 9)
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString().Substring(1, 1);
                                    // Week = Week + 1;

                                }
                                else
                                {
                                    CalDate.Rows[0][x] = CalDate.Rows[0][x] + "              " + Week.ToString();
                                    //Week = Week + 1;

                                }
                            }

                            Weekno = Weekno + 1;

                        }


                        // Weekno = Weekno + 1;

                        Day = Day + 1;


                    }
                    else
                    {
                        CalDate.Columns.Add();
                        CalDate.Rows[0][x] = string.Empty;
                    }

                }

                CalDate.Columns.Add();
                CalDate.Rows[0][CalDate.Columns.Count - 1] = Day.ToString();

                CalDate.TableName = "CalDates";
                DataSet DsCalDate = new DataSet();
                DsCalDate.Tables.Add(CalDate);

                _theReport.RegisterData(DsCalDate);


                MWDataManager.clsDataAccess _dbManPlan = new MWDataManager.clsDataAccess();
                _dbManPlan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "declare @Start datetime ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "set @Start = ( ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "select  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "min(calendardate) ss from tbl_Planning_Vamping v, tbl_section s , tbl_section s1, tbl_section s2 ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "where v.Sectionid = s.SectionID and v.Prodmonth = s.Prodmonth  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth   ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " and v.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and s2.SectionID = '" + cbxBookingMO.EditValue.ToString() + "')  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  declare @prev integer ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " set @prev = ( select max(prodmonth) aaaa from tbl_planmonth where prodmonth < '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "') ";



                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " select  case when newptt is null then 'Red' when newptt is not null and newptt < @prev then 'orange' else '' end as newwpflag, * from ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  (select '1' Line,s1.Sectionid sbid, s1.Name sbname, s.Sectionid minid, s.Name minname, pm.workplaceid, case when pm.activity = 9 then w.description+' Ledge' else w.description end as description , \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    case when pm.Activity = 9 then 'Ledge' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "   when pm.Activity = 0 then 'Stope' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "   when pm.Activity = 1 then 'Dev' End as Act, \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity = 1 then Adv else 0 end as DevTotAdv,  \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity = 1 and w.ReefWaste = 'W' then Adv else 0 end as DevWasteAdv, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity = 1 then Tons else 0 end as DevTons, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when  pm.Activity = 1 then Content else 0 end as DevCont,  \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity <> 1 then Tons else 0 end as StpTons, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "case when pm.activity <> 1 then Content else 0 end as StpCont,  \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    case when pm.Activity <> 1 then 'a.Stoping' else 'b.Development' end as MainGroup, \r\n ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    pm.FL, pm.SqmTotal plansqm, cyclesqm, orgunitds, 'Default Cycle' aa, pm.adv, tons, content, offreefsqm, Budget, pm.activity,   \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  case when pm.activity <> 1 then pm.extrabudget else 0 end as extrabudget, case when pm.activity = 1 then pm.extrabudget else 0 end as extrabudget1,  SUBSTRING(DefaultCycle,1,4) aa1, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,5,4) aa2, SUBSTRING(DefaultCycle,9,4) aa3, SUBSTRING(DefaultCycle,13,4) aa4, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,17,4) aa5, SUBSTRING(DefaultCycle,21,4) aa6, SUBSTRING(DefaultCycle,25,4) aa7, SUBSTRING(DefaultCycle,29,4) aa8, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,33,4) aa9, SUBSTRING(DefaultCycle,37,4) aa10, SUBSTRING(DefaultCycle,41,4) aa11, SUBSTRING(DefaultCycle,45,4) aa12, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,49,4) aa13, SUBSTRING(DefaultCycle,53,4) aa14, SUBSTRING(DefaultCycle,57,4) aa15, SUBSTRING(DefaultCycle,61,4) aa16, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     SUBSTRING(DefaultCycle,65,4) aa17, SUBSTRING(DefaultCycle,69,4) aa18, SUBSTRING(DefaultCycle,73,4) aa19, SUBSTRING(DefaultCycle,77,4) aa20, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,81,4) aa21, SUBSTRING(DefaultCycle,85,4) aa22, SUBSTRING(DefaultCycle,89,4) aa23, SUBSTRING(DefaultCycle,93,4) aa24, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,97,4) aa25, SUBSTRING(DefaultCycle,101,4) aa26, SUBSTRING(DefaultCycle,105,4) aa27, SUBSTRING(DefaultCycle,109,4) aa28, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,113,4) aa29, SUBSTRING(DefaultCycle,117,4) aa30, SUBSTRING(DefaultCycle,121,4) aa31, SUBSTRING(DefaultCycle,125,4) aa32, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,129,4) aa33, SUBSTRING(DefaultCycle,133,4) aa34, SUBSTRING(DefaultCycle,137,4) aa35, SUBSTRING(DefaultCycle,141,4) aa36, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,145,4) aa37, SUBSTRING(DefaultCycle,149,4) aa38, SUBSTRING(DefaultCycle,153,4) aa39, SUBSTRING(DefaultCycle,157,4) aa40, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,161,4) aa41, SUBSTRING(DefaultCycle,165,4) aa42, SUBSTRING(DefaultCycle,169,4) aa43, SUBSTRING(DefaultCycle,173,4) aa44, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(DefaultCycle,177,4) aa45, s1.ReportToSectionid, wt.WpSublocation Mprass, wpexternalid, case when substring(comments,1,3) = 'Sur' then 'Survey Zero Plan' else '' end as Surv ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , pmold1 newptt ";



                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      from tbl_PlanMonth pm, (select w.*, pmold1 from tbl_Workplace w left outer join ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     (select workplaceid wz, max(prodmonth) pmold1 from tbl_PlanMonth ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     where prodmonth < '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' group by workplaceid) newwp on  w.workplaceid = newwp.wz) w, tbl_Section s, tbl_Section s1, tbl_Workplace_Total wt where pm.workplaceid = w.workplaceid and w.GMSIWPID = wt.GMSIWPID   \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.Sectionid = s.SectionID and pm.Prodmonth = s.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s.reporttoSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "'";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s1.ReportToSectionid = '" + cbxBookingMO.EditValue + "') a \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      left outer join \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      (select '2' Line,s1.Sectionid sbid, s1.Name sbname, s.Sectionid minid, s.Name minname, pm.workplaceid, w.description, pm.activity, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     case when pm.Activity = 9 then 'Ledge' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 0 then 'Stope' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 1 then 'Dev' End as Act, \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     case when pm.Activity <> 1 then 'a.Stoping' else 'b.Development' end as MainGroup, \r\n ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     pm.FL, pm.SqmTotal plansqm, cyclesqm, orgunitds, 'MO Cycle' aa, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,1,4) a1, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,5,4) a2, SUBSTRING(MOCycle,9,4) a3, SUBSTRING(MOCycle,13,4) a4, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,17,4) a5, SUBSTRING(MOCycle,21,4) a6, SUBSTRING(MOCycle,25,4) a7, SUBSTRING(MOCycle,29,4) a8, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,33,4) a9, SUBSTRING(MOCycle,37,4) a10, SUBSTRING(MOCycle,41,4) a11, SUBSTRING(MOCycle,45,4) a12, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,49,4) a13, SUBSTRING(MOCycle,53,4) a14, SUBSTRING(MOCycle,57,4) a15, SUBSTRING(MOCycle,61,4) a16, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,65,4) a17, SUBSTRING(MOCycle,69,4) a18, SUBSTRING(MOCycle,73,4) a19, SUBSTRING(MOCycle,77,4) a20, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,81,4) a21, SUBSTRING(MOCycle,85,4) a22, SUBSTRING(MOCycle,89,4) a23, SUBSTRING(MOCycle,93,4) a24, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,97,4) a25, SUBSTRING(MOCycle,101,4) a26, SUBSTRING(MOCycle,105,4) a27, SUBSTRING(MOCycle,109,4) a28, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,113,4) a29, SUBSTRING(MOCycle,117,4) a30, SUBSTRING(MOCycle,121,4) a31, SUBSTRING(MOCycle,125,4) a32, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,129,4) a33, SUBSTRING(MOCycle,133,4) a34, SUBSTRING(MOCycle,137,4) a35, SUBSTRING(MOCycle,141,4) a36, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,145,4) a37, SUBSTRING(MOCycle,149,4) a38, SUBSTRING(MOCycle,153,4) a39, SUBSTRING(MOCycle,157,4) a40, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,161,4) a41, SUBSTRING(MOCycle,165,4) a42, SUBSTRING(MOCycle,169,4) a43, SUBSTRING(MOCycle,173,4) a44, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycle,177,4) a45, s1.ReportToSectionid";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      from tbl_PlanMonth pm, tbl_Workplace w, tbl_section s, tbl_section s1 where pm.workplaceid = w.workplaceid \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.Sectionid = s.SectionID and pm.Prodmonth = s.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s.reporttoSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and s1.ReportToSectionid = '" + cbxBookingMO.EditValue + "') b on a.workplaceid = b.workplaceid and a.activity = b.activity and a.minid = b.minid \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      left outer join \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      (select '3' Line,s1.Sectionid sbid, s1.Name sbname, s.Sectionid minid, s.Name minname, pm.workplaceid, w.description, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     case when pm.Activity = 9 then 'Ledge' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 0 then 'Stope' \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "    when pm.Activity = 1 then 'Dev' End as Act, \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      case when pm.Activity <> 1 then 'a.Stoping' else 'b.Development' end as MainGroup, \r\n ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      pm.FL, pm.SqmTotal plansqm, cyclesqm, orgunitds, case when pm.Activity in (0,9) then 'Day Sqm' else 'Day M' end as aa,  \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,1,4) b1, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,5,4) b2, SUBSTRING(MOCycleNum,9,4) b3, SUBSTRING(MOCycleNum,13,4) b4, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,17,4) b5, SUBSTRING(MOCycleNum,21,4) b6, SUBSTRING(MOCycleNum,25,4) b7, SUBSTRING(MOCycleNum,29,4) b8, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,33,4) b9, SUBSTRING(MOCycleNum,37,4) b10, SUBSTRING(MOCycleNum,41,4) b11, SUBSTRING(MOCycleNum,45,4) b12, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,49,4) b13, SUBSTRING(MOCycleNum,53,4) b14, SUBSTRING(MOCycleNum,57,4) b15, SUBSTRING(MOCycleNum,61,4) b16, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,65,4) b17, SUBSTRING(MOCycleNum,69,4) b18, SUBSTRING(MOCycleNum,73,4) b19, SUBSTRING(MOCycleNum,77,4) b20, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,81,4) b21, SUBSTRING(MOCycleNum,85,4) b22, SUBSTRING(MOCycleNum,89,4) b23, SUBSTRING(MOCycleNum,93,4) b24, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,97,4) b25, SUBSTRING(MOCycleNum,101,4) b26, SUBSTRING(MOCycleNum,105,4) b27, SUBSTRING(MOCycleNum,109,4) b28, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,113,4) b29, SUBSTRING(MOCycleNum,117,4) b30, SUBSTRING(MOCycleNum,121,4) b31, SUBSTRING(MOCycleNum,125,4) b32, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,129,4) b33, SUBSTRING(MOCycleNum,133,4) b34, SUBSTRING(MOCycleNum,137,4) b35, SUBSTRING(MOCycleNum,141,4) b36, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,145,4) b37, SUBSTRING(MOCycleNum,149,4) b38, SUBSTRING(MOCycleNum,153,4) b39, SUBSTRING(MOCycleNum,157,4) b40, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,161,4) b41, SUBSTRING(MOCycleNum,165,4) b42, SUBSTRING(MOCycleNum,169,4) b43, SUBSTRING(MOCycleNum,173,4) b44, \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      SUBSTRING(MOCycleNum,177,4) b45 , s1.ReportToSectionid, pm.activity  \r\n ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      from tbl_PlanMonth pm, tbl_Workplace w, tbl_section s, tbl_section s1 where pm.workplaceid = w.workplaceid  \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "      and pm.Sectionid = s.SectionID and pm.Prodmonth = s.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     and s.reporttoSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     and pm.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "     and s1.ReportToSectionid = '" + cbxBookingMO.EditValue.ToString() + "') c on a.Workplaceid = c.workplaceid  and a.activity = c.activity  and a.minid = c.minid \r\n ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , (select 0 act1, max(rand) rr FROM [dbo].[tbl_Rates_Stoping]) d  \r\n  ";



                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " union  \r\n  ";




                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " select   case when MAX(pmold1) is null then 'Red' when MAX(pmold1) is not null and MAX(pmold1) < @prev then 'orange' else '' end as newwpflag, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " Line, sbid, sbname, minid, minname, WorkplaceID,  Description, Act, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " sum(DevTotAdv) DevTotAdv, sum(DevWasteAdv) DevWasteAdv, sum(DevTons) DevTons, sum(DevCont) DevCont, sum(StpTons) StpTons \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , sum(StpCont) StpCont, max(MainGroup) MainGroup, max(FL) FL, convert(int,sum(plansqm)) plansqm \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , convert(int,sum(cyclesqm)) cyclesqm, MAX(orgunitds) orgunitds, MAX(aa)  aa \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(adv) adv, convert(int,sum(tons)) tons, convert(int,sum(content)) content, MAX(offreefsqm) offreefsqm, MAX(Budget) Budget, MAX(activity) activity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(extrabudget) extrabudget, MAX(extrabudget1) extrabudget1 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa1) aa1,MAX(aa2) aa2,MAX(aa3) aa3,MAX(aa4) aa4,MAX(aa5) aa5,MAX(aa6) aa6,MAX(aa7) aa7,MAX(aa8) aa8,MAX(aa9) aa9,MAX(aa10) aa10 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa11) aa11,MAX(aa12) aa12,MAX(aa13) aa13,MAX(aa14) aa14,MAX(aa15) aa15,MAX(aa16) aa16,MAX(aa17) aa17,MAX(aa18) aa18,MAX(aa19) aa19,MAX(aa20) aa20 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa21) aa21,MAX(aa22) aa22,MAX(aa23) aa23,MAX(aa24) aa24,MAX(aa25) aa25,MAX(aa26) aa26,MAX(aa27) aa27,MAX(aa28) aa28,MAX(aa29) aa29,MAX(aa30) aa30 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa31) aa31,MAX(aa32) aa32,MAX(aa33) aa33,MAX(aa34) aa34,MAX(aa35) aa35,MAX(aa36) aa36,MAX(aa37) aa37,MAX(aa38) aa38,MAX(aa39) aa39,MAX(aa40) aa40 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(aa41) aa41,MAX(aa42) aa42,MAX(aa43) aa43,MAX(aa44) aa44,MAX(aa45) aa45 \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(SectionID) SectionID, MAX(Mprass) Mprass, MAX(wpexternalid) wpexternalid, MAX(Surv) Surv \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(pmold1) newptt, ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 2 Line2, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(sbid1) sbid1, MAX(sbname1) sbname1, MAX(minid1) minid1, MAX(minname1) minname1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(wp1) wp1,MAX(wd1) wd1, 9 Act1a, MAX(Act1) Act1, \r\n  ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(MainGroup1) MainGroup1,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " sum(FL1) FL1, convert(int,sum(plansqm1)) plansqm1, convert(int,sum(cyclesqma)) cyclesqma , MAX(da) da, MAX(aaa) aaa \r\n  ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a1) a1,MAX(a2) a2,MAX(a3) a3,MAX(a4) a4,MAX(a5) a5,MAX(a6) a6,MAX(a7) a7,MAX(a8) a8,MAX(a9) a9,MAX(a10) a10 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a11) a11,MAX(a12) a12,MAX(a13) a13,MAX(a14) a14,MAX(a15) a15,MAX(a16) a16,MAX(a17) a17,MAX(a18) a18,MAX(a19) a19,MAX(a20) a20 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a21) a21,MAX(a22) a22,MAX(a23) a23,MAX(a24) a24,MAX(a25) a25,MAX(a26) a26,MAX(a27) a27,MAX(a28) a28,MAX(a29) a29,MAX(a30) a30 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a31) a31,MAX(a32) a32,MAX(a33) a33,MAX(a34) a34,MAX(a35) a35,MAX(a36) a36,MAX(a37) a37,MAX(a38) a38,MAX(a39) a39,MAX(a40) a40 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(a41) a41,MAX(a42) a42,MAX(a43) a43,MAX(a44) a44,MAX(a45) a45 \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " , MAX(SectionID) SectionID2, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(Line3) Line3, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(sbid1) sbid2, MAX(sbname1) sbname2, MAX(minid1) minid2, MAX(minname1) minname2, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(wp1) wp2,MAX(wd1) wd2, MAX(Act1) Act2, \r\n  ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " MAX(MainGroup1) MainGroup2,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " sum(FL1) FL2, convert(int,sum(plansqm1)) plansqm2, convert(int,sum(cyclesqma)) cyclesqma2 , MAX(da) da2, MAX(aaa) aaa2 \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b1) b1,MAX(b2) b2,MAX(b3) b3,MAX(b4) b4,MAX(b5) b5,MAX(b6) b6,MAX(b7) b7,MAX(b8) b8,MAX(b9) b9,MAX(b10) b10 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b11) b11,MAX(b12) b12,MAX(b13) b13,MAX(b14) b14,MAX(b15) b15,MAX(b16) b16,MAX(b17) b17,MAX(b18) b18,MAX(b19) b19,MAX(b20) b20 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b21) b21,MAX(b22) b22,MAX(b23) b23,MAX(b24) b24,MAX(b25) b25,MAX(b26) b26,MAX(b27) b27,MAX(b28) b28,MAX(b29) b29,MAX(b30) b30 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b31) b31,MAX(b32) b32,MAX(b33) b33,MAX(b34) b34,MAX(b35) b35,MAX(b36) b36,MAX(b37) b37,MAX(b38) b38,MAX(b39) b39,MAX(b40) b40 \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,MAX(b41) b41,MAX(b42) b42,MAX(b43) b43,MAX(b44) b44,MAX(b45) b45 \r\n  ";




                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " ,max(mo) mo , 9 ActF, pm, sum(rr) rr \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  from ( \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " select 1 Line, s1.SectionID sbid, s1.Name sbname, s.SectionID minid, s.Name minname, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " v.WorkplaceID, w.Description, 'Vamping' Act, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevTotAdv,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevWasteAdv, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevTons,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 DevCont,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " plantons StpTons,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " PlanContent StpCont,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'C.Vamping'  MainGroup,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 FL, PlanSqm plansqm, PlanSqm cyclesqm, orgunitds,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'Default Cycle' aa, 0 adv, plantons tons, PlanContent content, 0 offreefsqm, 0 Budget, 9 activity,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 extrabudget, 0 extrabudget1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa1,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa2, '' aa3, '' aa4,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa5, '' aa6, '' aa7, '' aa8,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa9, '' aa10, '' aa11, '' aa12,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa13, '' aa14,'' aa15, '' aa16,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa17, '' aa18, '' aa19, '' aa20, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa21, '' aa22, '' aa23, '' aa24, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa25, '' aa26, '' aa27, '' aa28, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa29, '' aa30, '' aa31, '' aa32, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa33, '' aa34, '' aa35, '' aa36,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa37, '' aa38, '' aa39, '' aa40,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa41, '' aa42, '' aa43, '' aa44,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " '' aa45, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " s2.SectionID, wt.WpSublocation Mprass, wpexternalid, '' Surv, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 2 Line3, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  s1.SectionID sbid1, s1.Name sbname1, s.SectionID minid1, s.Name minname1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " v.WorkplaceID wp1, w.Description wd1, 'Vamping' Act1, \r\n  ";


                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'C.Vamping'  MainGroup1, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 FL1, PlanSqm plansqm1, PlanSqm cyclesqma, orgunitds da, 'MO Cycle' aaa, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+1 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+1 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a2, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+2 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+2 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a3, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+3 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+3 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a4, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+4 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+4 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a5, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+5 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+5 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a6, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+6 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+6 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a7, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+7 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+7 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a8, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+8 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+8 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a9, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+9 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+9 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a10, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+10 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+10 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a11, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+11 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+11 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a12, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+12 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+12 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a13, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+13 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+13 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a14, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+14 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+14 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a15, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+15 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+15 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a16, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+16 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+16 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a17, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+17 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+17 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a18, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+18 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+18 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a19, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+19 and workingday = 'Y' then PlanActivity \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+19 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a20, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+20 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+20 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a21, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+21 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+21 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a22, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+22 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+22 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a23, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+23 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+23 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a24, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+24 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+24 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a25, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+25 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+25 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a26, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+26 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+26 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a27, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+27 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+27 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a28, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+28 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+28 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a29, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+29 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+29 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a30, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+30 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+30 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a31, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+31 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+31 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a32, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+32 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+32 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a33, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+33 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+33 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a34, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+34 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+34 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a35, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+35 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+35 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a36, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+36 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+36 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a37, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+37 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+37 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  else '' end as a38, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+38 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+38 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a39, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+39 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+39 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a40, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+40 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+40 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a41, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+41 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+41 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a42, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+42 and workingday = 'Y' then PlanActivity \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+42 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a43, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+43 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+43 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a44, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+44 and workingday = 'Y' then PlanActivity  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " when calendardate = @Start+44 and workingday = 'N' then 'OFF' \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as a45, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " s2.SectionID ReporttoSectioniD, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 3 Line3a, \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  s1.SectionID sbid3, s1.Name sbname3, s.SectionID minid3, s.Name minname3, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " v.WorkplaceID wp2, w.Description wd2, 'Vamping' Act3,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 'C.Vamping'  MainGroupaaa,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " 0 FLz, PlanSqm plansqmz, PlanSqm cyclesqm1, orgunitds ds1, 'Day Sqm' aaz, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b1, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+1 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b2, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+2 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b3, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+3 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm)) \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b4, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+4 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b5, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+5 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b6, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+6 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b7, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+7 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b8, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+8 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b9, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+9 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b10, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+10 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b11, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+11 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b12, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+12 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b13, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+13 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b14, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+14 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b15, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+15 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b16, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+16 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b17, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+17 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b18, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+18 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b19, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+19 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b20, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+20 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b21, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+21 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b22, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+22 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b23, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+23 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b24, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+24 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b25, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+25 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b26, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+26 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b27, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+27 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b28, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+28 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b29, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+29 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b30, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+30 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b31, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+31 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b32, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+32 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b33, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+33 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b34, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+34 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b35, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+35 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b36, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+36 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b37, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+37 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b38, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+38 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b39, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+39 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b40, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+40 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b41, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+41 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b42, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+42and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b43, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+43 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b44, \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " case when calendardate = @Start+44 and  plansqm > 0 then convert(varchar(10),convert(int,PlanSqm))  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " else '' end as b45,  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " s2.SectionID mo, v.Prodmonth pm, 0 rr, pmold1 \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  from tbl_Planning_Vamping v, tbl_Section s , tbl_Section s1, tbl_Section s2, (select w.*, pmold1 from tbl_Workplace w left outer join  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " (select workplaceid wz, max(prodmonth) pmold1 from tbl_Planning_Vamping  \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  where prodmonth < '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' group by workplaceid) newwp on  w.workplaceid = newwp.wz) w, tbl_Workplace_Total wt \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " where v.Sectionid = s.SectionID and v.Prodmonth = s.Prodmonth   \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth   \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and v.WorkplaceID = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n  ";
                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  and v.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' and s2.SectionID = '" + cbxBookingMO.EditValue.ToString() + "') a \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + "  group by Line, sbid, sbname, minid, minname, WorkplaceID,  Description, Act, pm \r\n  ";

                _dbManPlan.SqlStatement = _dbManPlan.SqlStatement + " order by  a.sbid,a.MainGroup,a.OrgUnitDS,a.Workplaceid,a.aa1 Desc,a.Line \r\n  ";

                _dbManPlan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPlan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPlan.ResultsTableName = "Planning";
                _dbManPlan.ExecuteInstruction();

                DataSet DsPlanning = new DataSet();
                DsPlanning.Tables.Add(_dbManPlan.ResultsDataTable);

                _theReport.RegisterData(DsPlanning);

                _theReport.Load(_reportFolder + "PlanningDevelopment.frx");

                //_theReport.Design();

                pcReport2.OutlineVisible = true;

                pcReport2.Clear();
                _theReport.Prepare();
                _theReport.Preview = pcReport2;
                _theReport.ShowPrepared();
            }
        }
    }
}
