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
    public partial class ucWorstPerformer : BaseUserControl
    {
        #region Fields and Properties 
        DataTable dtOrgUnit;
        DataTable dtMO;
        DataTable dtStoping;
        DataTable dtDev;
        string Section;
        string SectionName;
        string Plan;
        string Book;
        string Crew;
        string _FirstLoad = "Y";

        private Report _theReport = new Report();
        /// <summary>
        /// The report to show
        /// </summary>
        public Report theReport
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
        public ucWorstPerformer()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpWorstPerformer);
            FormActiveRibbonPage = rpWorstPerformer;
            FormMainRibbonPage = rpWorstPerformer;
            RibbonControl = rcWorstPerformer;
        }
        #endregion Constructor

        #region Events

        private void ucWorstPerformer_Load(object sender, EventArgs e)
        {
            tabPage1.Text = "MO Summary" + "                                        ".Substring(0, 30);
            tabPage2.Text = "Crew Analysis" + "                                        ".Substring(0, 30);
            //pcMoReport.Visible = false;
            //PlanCkb.Checked = true;
            //BookCbk.Checked = true;
            Plan = "Y";
            Book = "Y";

            StartDate.Value = EndDate.Value.AddDays(-7);

            LoadWorstPerformers();
            MO1btn_Click(null, null);
            tabControl1.Visible = true;
        }

        private void OrgUnit1btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit1btn.Text;
            LoadOrgUnit();
        }

        private void OrgUnit2btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit2btn.Text;
            LoadOrgUnit();
        }

        private void OrgUnit3btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit3btn.Text;
            LoadOrgUnit();
        }

        private void OrgUnit4btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit4btn.Text;
            LoadOrgUnit();
        }

        private void OrgUnit5btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit5btn.Text;
            LoadOrgUnit();
        }

        private void OrgUnit6btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit6btn.Text;
            LoadOrgUnit();
        }

        private void OrgUnit7btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit7btn.Text;
            LoadOrgUnit();
        }

        private void OrgUnit8btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit8btn.Text;
            LoadOrgUnit();
        }

        private void OrgUnit9btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit9btn.Text;
            LoadOrgUnit();
        }

        private void OrgUnit10btn_Click(object sender, EventArgs e)
        {
            Crew = OrgUnit10btn.Text;
            LoadOrgUnit();
        }

        private void TotalMinebtn_Click(object sender, EventArgs e)
        {
            Plan = "Y";
            Book = "Y";
            LoadWorstPerformers();
        }

        private void PlanCkb_CheckedChanged(object sender, EventArgs e)
        {
            if (PlanCkb.Checked == true)
            {
                Plan = "Y";
                LoadWorstPerformers();
            }
            else
            {
                Plan = "N";
                LoadWorstPerformers();
            }
        }

        private void BookCkb_CheckedChanged(object sender, EventArgs e)
        {
            if (PlanCkb.Checked == true)
            {
                Book = "Y";
                LoadWorstPerformers();
            }
            else
            {
                Book = "N";
                LoadWorstPerformers();
            }
        }

        private void Activityrgb_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadWorstPerformers();
            MO1btn_Click(null, null);
        }

        private void Timeframergb_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadWorstPerformers();
            MO1btn_Click(null, null);
        }

        private void MO1btn_Click(object sender, EventArgs e)
        {
            Section = Mo1lbl.Text;
            SectionName = SectionName1.Text;

            LoadGraphs();
        }

        private void MO2btn_Click(object sender, EventArgs e)
        {
            Section = Mo2lbl.Text;
            SectionName = SectionName2.Text;

            LoadGraphs();
        }

        private void MO3btn_Click(object sender, EventArgs e)
        {
            Section = Mo3lbl.Text;
            SectionName = SectionName3.Text;

            LoadGraphs();
        }

        private void MO4btn_Click(object sender, EventArgs e)
        {
            Section = Mo4lbl.Text;
            SectionName = SectionName4.Text;

            LoadGraphs();
        }

        private void MO5btn_Click(object sender, EventArgs e)
        {
            Section = Mo5lbl.Text;
            SectionName = SectionName5.Text;

            LoadGraphs();
        }

        private void MO6btn_Click(object sender, EventArgs e)
        {
            Section = Mo5lbl.Text;
            SectionName = SectionName5.Text;

            LoadGraphs();
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
            _theReport.Preview = pcMoReport;

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
            DataSet ds = new DataSet();
            MWDataManager.clsDataAccess _databaseConnection = new MWDataManager.clsDataAccess();
            _databaseConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _databaseConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _databaseConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _databaseConnection.SqlStatement = "PUT SQL HERE";

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


        void LoadWorstPerformers()
        {
            this.Cursor = Cursors.WaitCursor;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (Activityrgb.SelectedIndex == 0) //Stoping
            {
                _dbMan.SqlStatement = "select Top 20 '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, '" + Plan + "' planchk, '" + Book + "' Bookchk, convert(varchar(50), orgunitds+ '  ') org, a.*, case when a.booksqm is null then 0-a.plansqm else a.booksqm-a.plansqm end as var from " +
                                    "(select substring(orgunitds,1,12) orgunitds, " +
                                    "sum(sqm) plansqm, ";
                if (ProductionGlobalTSysSettings._AdjBook == "Y")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " sum(p.booksqm + p.AdjSqm) booksqm ";
                }
                else
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " sum(p.booksqm) booksqm ";
                }
                _dbMan.SqlStatement = _dbMan.SqlStatement + " from tbl_Planning p  " +
                                   " " +
                                   "where p.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' " +
                                   "and p.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' " +
                                   "and p.activity in(0,9) " +
                                   "group by orgunitds) a " +
                                   "order by var ";
            }

            if (Activityrgb.SelectedIndex == 1) //Development
            {
                _dbMan.SqlStatement = "select Top 20  '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, '" + Plan + "' planchk,'" + Book + "' Bookchk, convert(varchar(50), orgunitds+ '  ') org, a.*, case when a.booksqm is null then 0-a.plansqm else a.booksqm-a.plansqm end as var from " +
                                    " (select  substring(orgunitds,1,12) orgunitds, " +
                                    "sum(adv) plansqm, ";
                if (ProductionGlobalTSysSettings._AdjBook == "Y")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " sum(p.bookadv) booksqm ";
                }
                else
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " sum(p.bookadv) booksqm ";
                }
                _dbMan.SqlStatement = _dbMan.SqlStatement + " from tbl_Planning p  " +
                                    " " +
                                    "where p.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' " +
                                    "and p.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' " +
                                    "and p.activity in(1) " +
                                    "group by orgunitds) a " +
                                    "order by var ";
            }
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "OrgUnit";
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count == 0)
            {
                MessageBox.Show("No Bookings found for " + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + " - " + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "  ");
            }

            DataSet dsOrg = new DataSet();
            dsOrg.Tables.Add(_dbMan.ResultsDataTable);

            _theReport.RegisterData(dsOrg);

            _theReport.Load(_reportFolder + "WorstPerformersOrg.frx");
            _theReport.Design();

            pcCrewReport.Clear();
            _theReport.Prepare();
            _theReport.Preview = pcCrewReport;
            _theReport.ShowPrepared();

            tabControl1.Visible = true;
            pcCrewReport.Visible = true;



            int x = 1;


            dtOrgUnit = _dbMan.ResultsDataTable;

            foreach (DataRow dr in dtOrgUnit.Rows)
            {
                if (x == 1)
                {
                    OrgUnit1btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit1btn.Visible = true;
                }

                if (x == 2)
                {
                    OrgUnit2btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit2btn.Visible = true;
                }

                if (x == 3)
                {
                    OrgUnit3btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit3btn.Visible = true;
                }

                if (x == 4)
                {
                    OrgUnit4btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit4btn.Visible = true;
                }

                if (x == 5)
                {
                    OrgUnit5btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit5btn.Visible = true;
                }

                if (x == 6)
                {
                    OrgUnit6btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit6btn.Visible = true;
                }

                if (x == 7)
                {
                    OrgUnit7btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit7btn.Visible = true;
                }

                if (x == 8)
                {
                    OrgUnit8btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit8btn.Visible = true;
                }

                if (x == 9)
                {
                    OrgUnit9btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit9btn.Visible = true;
                }

                if (x == 10)
                {
                    OrgUnit10btn.Text = dr["Orgunitds"].ToString();
                    OrgUnit10btn.Visible = true;
                }

                x = x + 1;
            }


            ////////////////////////////////////////MO Chart///////////////////////////
            MWDataManager.clsDataAccess _dbManMO = new MWDataManager.clsDataAccess();
            _dbManMO.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (Activityrgb.SelectedIndex == 0)
            {

                _dbManMO.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, a.*, a.booksqm-a.plansqm var from (select  s2.sectionid sec, s2.name mo, " +
                                        " sum(sqm) plansqm, ";
                if (ProductionGlobalTSysSettings._AdjBook == "Y")
                {
                    _dbManMO.SqlStatement = _dbManMO.SqlStatement + " sum(p.booksqm + p.AdjSqm) booksqm ";
                }
                else
                {
                    _dbManMO.SqlStatement = _dbManMO.SqlStatement + " sum(p.booksqm) booksqm ";
                }
                _dbManMO.SqlStatement = _dbManMO.SqlStatement + " from tbl_Planning p  " +
                ", tbl_Section s, tbl_Section s1, tbl_Section s2 where p.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", "2019-11-27") + "' " +
                                            "and p.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", "2019-05-20") + "' and p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and s.reporttosectionid = s1.sectionid " +
                                            "and s.prodmonth = s1.prodmonth and s1.reporttosectionid = s2.sectionid and s1.prodmonth = s2.prodmonth " +
                                            "and activity in (0,9) group by s2.sectionid, s2.name) a  " +

                                            //  "(select  s2.name mo, sum(squaremetres) booksqm from booking p, tbl_Section s, tbl_Section s1, tbl_Section s2 " +
                                            //    "where p.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' and p.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' and p.sectionid = s.sectionid and p.prodmonth = s.prodmonth " +
                                            //   "and s.reporttosectionid = s1.sectionid and s.prodmonth = s1.prodmonth and s1.reporttosectionid = s2.sectionid " +
                                            //    "and s1.prodmonth = s2.prodmonth and activity = 0 group by s2.name) b on a.mo = b.mo " +
                                            "order by var ";

            }
            else
            {
                _dbManMO.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, a.*, a.booksqm-a.plansqm var from (select  s2.sectionid sec, s2.name mo, " +
                                        " sum(adv) plansqm, ";
                if (ProductionGlobalTSysSettings._AdjBook == "Y")
                {
                    _dbManMO.SqlStatement = _dbManMO.SqlStatement + " sum(p.bookadv) booksqm ";
                }
                else
                {
                    _dbManMO.SqlStatement = _dbManMO.SqlStatement + " sum(p.booksqm) booksqm ";
                }
                _dbManMO.SqlStatement = _dbManMO.SqlStatement + " from tbl_Planning p , tbl_Section s, tbl_Section s1, tbl_Section s2 where p.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' " +
                                            "and p.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' and p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and s.reporttosectionid = s1.sectionid " +
                                            "and s.prodmonth = s1.prodmonth and s1.reporttosectionid = s2.sectionid and s1.prodmonth = s2.prodmonth " +
                                            "and activity in (1) group by s2.sectionid, s2.name) a  " +

                                            "order by var ";
            }
            _dbManMO.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMO.queryReturnType = MWDataManager.ReturnType.DataTable;

            _dbManMO.ResultsTableName = "MO";

            _dbManMO.ExecuteInstruction();

            DataSet dsMO = new DataSet();
            dsMO.Tables.Add(_dbManMO.ResultsDataTable);

            _theReport.RegisterData(dsMO);

            dtMO = _dbManMO.ResultsDataTable;
            int y = 1;

            foreach (DataRow drMO in dtMO.Rows)
            {
                if (y == 1)
                {
                    MO1btn.Text = drMO["sec"].ToString() + ":" + drMO["mo"].ToString();
                    MO1btn.Visible = true;
                    Mo1lbl.Text = drMO["sec"].ToString();
                    SectionName1.Text = drMO["mo"].ToString();
                }

                if (y == 2)
                {
                    MO2btn.Text = drMO["sec"].ToString() + ":" + drMO["mo"].ToString();
                    MO2btn.Visible = true;
                    Mo2lbl.Text = drMO["sec"].ToString();
                    SectionName2.Text = drMO["mo"].ToString();
                }

                if (y == 3)
                {
                    MO3btn.Text = drMO["sec"].ToString() + ":" + drMO["mo"].ToString();
                    MO3btn.Visible = true;
                    Mo3lbl.Text = drMO["sec"].ToString();
                    SectionName3.Text = drMO["mo"].ToString();
                }

                if (y == 4)
                {
                    MO4btn.Text = drMO["sec"].ToString() + ":" + drMO["mo"].ToString();
                    MO4btn.Visible = true;
                    Mo4lbl.Text = drMO["sec"].ToString();
                    SectionName4.Text = drMO["mo"].ToString();
                }
                if (y == 5)
                {
                    MO5btn.Text = drMO["sec"].ToString() + ":" + drMO["mo"].ToString();
                    MO5btn.Visible = true;
                    Mo5lbl.Text = drMO["sec"].ToString();
                    SectionName5.Text = drMO["mo"].ToString();
                }
                if (y == 6)
                {
                    MO6btn.Text = drMO["sec"].ToString() + ":" + drMO["mo"].ToString();
                    MO6btn.Visible = true;
                    Mo6lbl.Text = drMO["sec"].ToString();
                    SectionName6.Text = drMO["mo"].ToString();
                }
                y = y + 1;

            }

            this.Cursor = Cursors.Default;
        }

        void LoadOrgUnit()
        {
            MWDataManager.clsDataAccess _dbManCrew = new MWDataManager.clsDataAccess();
            _dbManCrew.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            if (Activityrgb.SelectedIndex == 0)
            {

                _dbManCrew.SqlStatement = "Select 'aa' aa,'" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner ,p.WorkplaceID, SUM(Sqm) Sqm, ";
                if (ProductionGlobalTSysSettings._AdjBook == "Y")
                {
                    _dbManCrew.SqlStatement = _dbManCrew.SqlStatement + "SUM(BookSqm)+SUM(AdjSqm)-SUM(Sqm) BookVar ";
                }
                else
                {

                    _dbManCrew.SqlStatement = _dbManCrew.SqlStatement + "SUM(BookSqm-Sqm) BookVar ";
                }

                _dbManCrew.SqlStatement = _dbManCrew.SqlStatement + " ,OrgUnitds, " +
                                        "w.Description from tbl_Planning p, tbl_Workplace w where p.OrgUnitDS = '" + Crew + "' and p.CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' and p.CalendarDate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' and p.Activity in (0,9) " +
                                        "and p.WorkplaceID = w.WorkplaceID " +
                                        "group by p.WorkplaceID,p.Orgunitds,w.Description ";

            }
            else
            {
                _dbManCrew.SqlStatement = "Select 'aa' aa,'" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner ,p.WorkplaceID,SUM(adv) Sqm";
                if (ProductionGlobalTSysSettings._AdjBook == "Y")
                {
                    _dbManCrew.SqlStatement = _dbManCrew.SqlStatement + "SUM((Bookadv)-adv) BookVar ";
                }
                else
                {

                    _dbManCrew.SqlStatement = _dbManCrew.SqlStatement + "SUM(Bookadv-adv) BookVar ";
                }

                _dbManCrew.SqlStatement = _dbManCrew.SqlStatement + ",OrgUnitds,w.Description from tbl_Planning p, tbl_Workplace w where p.OrgUnitDS = '" + Crew + "' and p.CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' and p.CalendarDate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' and p.Activity in (1) " +
                                        "and p.WorkplaceID = w.WorkplaceID " +
                                        "group by p.WorkplaceID,p.Orgunitds,w.Description ";
            }


            _dbManCrew.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCrew.queryReturnType = MWDataManager.ReturnType.DataTable;

            _dbManCrew.ResultsTableName = "Crew";

            _dbManCrew.ExecuteInstruction();

            DataSet dsCrew = new DataSet();
            dsCrew.Tables.Add(_dbManCrew.ResultsDataTable);

            _theReport.RegisterData(dsCrew);


            MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();
            _dbManProb.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            if (Activityrgb.SelectedIndex == 0)
            {

                _dbManProb.SqlStatement = "select a.*, p.sbossnotes from (select 'a' a, '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, p.CalendarDate, p.BookProb, pb.Description, p.workplaceid from tbl_Planning p, " +
                                        "tbl_Problem pb where p.BookProb = pb.ProblemID  " +
                                        "and p.OrgUnitDS = '" + Crew + "' and p.CalendarDate <=  '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' and p.CalendarDate >=  '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' and p.Activity in (0,9)) a " +
                "left outer join tbl_ProblemBook p on a.calendardate = p.calendardate and a.workplaceid = p.workplaceid and a.bookprob = p.problemid ";
            }
            else
            {
                _dbManProb.SqlStatement = "select a.*, p.sbossnotes from (select 'a' a, '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, p.CalendarDate,p.BookProb,pb.Description, p.workplaceid from tbl_Planning p, " +
                                        "tbl_Problem pb where p.BookProb = pb.ProblemID  " +
                                        "and p.OrgUnitDS = '" + Crew + "' and p.CalendarDate <=  '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' and p.CalendarDate >=  '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' and p.Activity in (1)) a " +
                                        "left outer join tbl_ProblemBook p on a.calendardate = p.calendardate and a.workplaceid = p.workplaceid and a.bookprob = p.problemid ";
            }


            _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;

            _dbManProb.ResultsTableName = "Prob";
            textBox1.Text = _dbManProb.SqlStatement;
            _dbManProb.ExecuteInstruction();

            DataSet dsProb = new DataSet();
            dsProb.Tables.Add(_dbManProb.ResultsDataTable);

            _theReport.RegisterData(dsProb);

            MWDataManager.clsDataAccess _dbManChart = new MWDataManager.clsDataAccess();
            _dbManChart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            if (Activityrgb.SelectedIndex == 0)
            {

                _dbManChart.SqlStatement = "Select 'a'a, PROBLEMID, COUNT(problemid) TotalCount " +
                                        "from tbl_Planning p,tbl_Problem pb where p.OrgUnitDS = '" + Crew + "'  " +
                                        "and p.CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' and p.CalendarDate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' " +
                                        "and p.Activity in (0,9) and p.BookProb = pb.ProblemID " +
                                        "group by ProblemID having COUNT(problemid) > 0 order by ProblemID desc";
            }
            else
            {
                _dbManChart.SqlStatement = "Select 'a'a, PROBLEMID, COUNT(problemid) TotalCount " +
                                         "from tbl_Planning p,tbl_Problem pb where p.OrgUnitDS = '" + Crew + "' and p.CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' and p.CalendarDate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' and p.Activity in (1) " +
                                         "and p.BookProb = pb.ProblemID " +
                                        "and p.CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "' and p.CalendarDate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' " +
                                         "group by ProblemID " +
                                         "having COUNT(problemid) > 0 " +
                                         "order by ProblemID desc";
            }



            _dbManChart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManChart.queryReturnType = MWDataManager.ReturnType.DataTable;

            _dbManChart.ResultsTableName = "Chart";

            _dbManChart.ExecuteInstruction();

            DataSet dsChart = new DataSet();
            dsChart.Tables.Add(_dbManChart.ResultsDataTable);

            _theReport.RegisterData(dsChart);

            _theReport.Load(_reportFolder + "WorstPerformersCrew.frx");
            //_theReport.Design();

            pcCrewReport.Clear();
            _theReport.Prepare();
            _theReport.Preview = pcCrewReport;
            _theReport.ShowPrepared();

            pcCrewReport.Visible = true;
        }

        void LoadGraphs()
        {
            ///////////////////////Stoping and Development/////////////////////

            //if (dtMO.Rows.Count != 0)
            //{

            MWDataManager.clsDataAccess _dbManSD = new MWDataManager.clsDataAccess();
            _dbManSD.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbManSD.SqlStatement = "select Top 12 convert(varchar(50),a.prodmonth) pm, a.*, case when b.booksqm is null then 0 else b.booksqm end as booksqm " +
                                   ",case when c.meassqm is null then 0 else c.meassqm end as meassqm, " +
                                  " case when c.meassqm is null then 0 else c.meassqm-a.plansqm end as var " +
                                     "from ( " +
                                  " select p.prodmonth, sum(p.sqmtotal) plansqm from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2 where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and s.reporttosectionid = s1.sectionid " +
                                   "and s.prodmonth = s1.prodmonth and s1.reporttosectionid = s2.sectionid and s1.prodmonth = s2.prodmonth " +
                                   "and activity in(0,9) and s2.sectionid = '" + Section + "' " +
                                   "group by p.prodmonth) a " +
                                   "left outer join " +
                                   "(select p.prodmonth, ";
            if (ProductionGlobalTSysSettings._AdjBook == "Y")
            {
                _dbManSD.SqlStatement = _dbManSD.SqlStatement + "sum(p.booksqm + AdjSqm) booksqm";
            }
            else
            {
                _dbManSD.SqlStatement = _dbManSD.SqlStatement + " sum(p.booksqm) booksqm ";
            }

            _dbManSD.SqlStatement = _dbManSD.SqlStatement + " from tbl_Planning p, tbl_Section s, tbl_Section s1, tbl_Section s2 where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and s.reporttosectionid = s1.sectionid " +
            "and s.prodmonth = s1.prodmonth and s1.reporttosectionid = s2.sectionid and s1.prodmonth = s2.prodmonth " +
            "and activity in(0,9) and s2.sectionid = '" + Section + "' " +
            "group by p.prodmonth) b on " +
            "a.prodmonth = b.prodmonth " +
            "left outer join " +
            "(select p.prodmonth, sum(p.sqmtotal) meassqm from survey p, tbl_Section s, tbl_Section s1, tbl_Section s2 where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and s.reporttosectionid = s1.sectionid " +
            "and s.prodmonth = s1.prodmonth and s1.reporttosectionid = s2.sectionid and s1.prodmonth = s2.prodmonth " +
            "and activitycode in (0,9) and s2.sectionid = '" + Section + "' " +
           " group by p.prodmonth) c on " +
            "a.prodmonth = c.prodmonth " +
            "order by a.prodmonth desc ";


            _dbManSD.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSD.queryReturnType = MWDataManager.ReturnType.DataTable;

            _dbManSD.ResultsTableName = "Stoping";
            _dbManSD.ExecuteInstruction();

            DataSet dsStoping = new DataSet();
            dsStoping.Tables.Add(_dbManSD.ResultsDataTable);

            _theReport.RegisterData(dsStoping);
            dtStoping = _dbManSD.ResultsDataTable;



            ////////////////Development//////////////////
            MWDataManager.clsDataAccess _dbManDev = new MWDataManager.clsDataAccess();
            _dbManDev.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbManDev.SqlStatement = "select Top 12 a.*, convert(varchar(50),a.prodmonth) pp,'" + Section + "' + ':' +'" + SectionName + "' Section, case when b.booksqm is null then 0 else b.booksqm end as booksqm " +
                                    ",case when c.meassqm is null then 0 else c.meassqm end as meassqm, " +
                                    "case when c.meassqm is null then 0 else c.meassqm-a.plansqm end as var " +
                                    "    from ( " +
                                    "select p.prodmonth, sum(p.adv) plansqm from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2 where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and s.reporttosectionid = s1.sectionid " +
                                    "and s.prodmonth = s1.prodmonth and s1.reporttosectionid = s2.sectionid and s1.prodmonth = s2.prodmonth " +
                                    "and activity = 1 and s2.sectionid = '" + Section + "' " +
                                    "group by p.prodmonth) a " +
                                    "left outer join " +
                                    "(select p.prodmonth, sum(p.bookadv) booksqm from tbl_Planning p, tbl_Section s, tbl_Section s1, tbl_Section s2 where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and s.reporttosectionid = s1.sectionid " +
                                    "and s.prodmonth = s1.prodmonth and s1.reporttosectionid = s2.sectionid and s1.prodmonth = s2.prodmonth " +
                                    "and activity = 1 and s2.sectionid ='" + Section + "' " +
                                    "group by p.prodmonth) b on " +
                                    "a.prodmonth = b.prodmonth " +
                                    "left outer join " +
                                    "(select p.prodmonth, sum(p.adv) meassqm from survey p, tbl_Section s, tbl_Section s1, tbl_Section s2 where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and s.reporttosectionid = s1.sectionid " +
                                    "and s.prodmonth = s1.prodmonth and s1.reporttosectionid = s2.sectionid and s1.prodmonth = s2.prodmonth " +
                                    "and activitycode = 1 and s2.sectionid = '" + Section + "' " +
                                    "group by p.prodmonth) c on " +
                                    "a.prodmonth = c.prodmonth " +
                                    "order by a.prodmonth desc";

            _dbManDev.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDev.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDev.ResultsTableName = "Development";

            _dbManDev.ExecuteInstruction();

            DataSet dsDev = new DataSet();
            dsDev.Tables.Add(_dbManDev.ResultsDataTable);

            _theReport.RegisterData(dsDev);

            _dbManDev.ExecuteInstruction();
            dtDev = _dbManDev.ResultsDataTable;

            _theReport.Load(_reportFolder + "WorstPerformers.frx");
            //_theReport.Design();

            pcMoReport.Clear();
            _theReport.Prepare();

            _theReport.Preview = pcMoReport;

            _theReport.ShowPrepared();

            pcMoReport.Visible = true;
        }



        #endregion Methods 

        private void StartDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void EndDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void StartDate_CloseUp(object sender, EventArgs e)
        {
            LoadWorstPerformers();
        }

        private void EndDate_CloseUp(object sender, EventArgs e)
        {
            LoadWorstPerformers();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadWorstPerformers();
        }

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadWorstPerformers();
        }
    }
}
