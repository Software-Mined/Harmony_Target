using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting.Analysis
{
    public partial class ucLostBlastAnalysis : BaseUserControl
    {
        public ucLostBlastAnalysis()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpLostBlastAnalysis);
            FormActiveRibbonPage = rpLostBlastAnalysis;
            FormMainRibbonPage = rpLostBlastAnalysis;
            RibbonControl = rcSDB_Activities;
        }

        #region private variables
        DataTable dtSections = new DataTable();
        string Hier;
        string sec;
        
        string GraphType;
        string FirtsLoad = "Y";
        Report theReportprob = new Report();
        Report theReport2prob = new Report();
        Report theReport3prob = new Report();
        Report theReportWPStop = new Report();
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        #endregion

        private void ucLostBlastAnalysis_Load(object sender, EventArgs e)
        {
            string repdir = ProductionGlobalTSysSettings._RepDir;

            FromDate.EditValue = DateTime.Now;
            ToDate.EditValue = DateTime.Now;

            DataTab.Visible = false;
            DataTab.Text = "Data" + "                                        ".Substring(0, 30);
            DatatabP.Text = "Problems" + "                                        ".Substring(0, 30);
            GraphTabP.Text = "Problem History Graph" + "                                        ".Substring(0, 30);
            ParitoChart.Text = "Pareto Graph" + "                                        ".Substring(0, 30);
            tabPage1.Text = "Workplace Stoppage" + "                                        ".Substring(0, 30);

            radioGroup1.EditValue = "Prodmonth";
            TypeRgb.EditValue = "Stoping";
            ProblemsRgb.EditValue = "Plan To Be Blasted Lost Blast";
            GraphViewTypergb.EditValue = "Itemised";

            barProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobalTSysSettings._currentProductionMonth.ToString());

            //LoadSection();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan.SqlStatement = "Select 'All' HQCat,0 ord \r\n" +
                                  "union  \r\n" +
                                  "select Distinct(HQCat),1 ord from tbl_PROBLEM";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtResp = _dbMan.ResultsDataTable;

            repleHOCat.DataSource = dtResp;
            repleHOCat.DisplayMember = "HQCat";
            repleHOCat.ValueMember = "HQCat";
            repleHOCat.PopulateColumns();
            repleHOCat.Columns[1].Visible = false;

            HQCatCmb.EditValue = _dbMan.ResultsDataTable.Rows[0][0].ToString();

            MWDataManager.clsDataAccess _dbManProbGroup = new MWDataManager.clsDataAccess();
            _dbManProbGroup.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbManProbGroup.SqlStatement = "Select 'All' ProblemGroup,0 ord \r\n" +
                                           "union \r\n" +
                                           "Select ProblemGroupCode,1 ord from tbl_CODE_PROBLEMGROUP where valid = 'Y' " +
                                           "Order by ord,ProblemGroup";

            _dbManProbGroup.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManProbGroup.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManProbGroup.ExecuteInstruction();

            DataTable dtProbGroup = _dbManProbGroup.ResultsDataTable;

            repleProbGroup.DataSource = dtProbGroup;
            repleProbGroup.DisplayMember = "ProblemGroup";
            repleProbGroup.ValueMember = "ProblemGroup";
            repleProbGroup.PopulateColumns();
            repleProbGroup.Columns[1].Visible = false;

            ProbGroupCmb.EditValue = _dbManProbGroup.ResultsDataTable.Rows[0][0].ToString();

            DataTab.SelectedTab = GraphTabP;

            MainLoad();
        }

        void LoadSection()
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = "select Sectionid, case when Name = '' then Sectionid else Name end as Name,Hierarchicalid  from tbl_Section where Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and HierarchicalType = 'Pro' and Hierarchicalid < 5 " +
                                   "order by Hierarchicalid, sectionid ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            dtSections = _dbMan1.ResultsDataTable;

            DataTable dtSect = _dbMan1.ResultsDataTable;
            repsleSection.DataSource = dtSect;
            repsleSection.DisplayMember = "Name";
            repsleSection.ValueMember = "Sectionid";
            repsleSection.PopulateViewColumns();
            repsleSection.View.Columns[2].Visible = false;

            UnderSectionCmb.EditValue = _dbMan1.ResultsDataTable.Rows[0][0].ToString();
        }

        private void MainLoad()
        {
            pnlGraphTop.Visible = true;
            DataTab.TabPages.Remove(ParitoChart);

            DataTab.TabPages.Remove(tabPage1);

            if (UnderSectionCmb.EditValue.ToString() == "[EditValue is null]")
            {
                MessageBox.Show("Please select a section");
                return;
            }
            else
            {
                if (TypeRgb.EditValue.ToString() == "Stoping" || TypeRgb.EditValue.ToString() == "Development")
                {
                    GraphType = "Bar";
                    loadData();
                    NumberBtn_Click(null, null);
                    LoadComboGraph();
                    DataTab.Visible = true;
                    DataTab.TabPages.Add(ParitoChart);
                    DataTab.TabPages.Add(tabPage1);
                }
                if (TypeRgb.EditValue.ToString() == "Vamping")
                {
                    GraphType = "Bar";
                    pnlGraphTop.Visible = false;
                    loadDataVamps();
                }

                if (TypeRgb.EditValue.ToString() == "N/S")
                {
                    pnlGraphTop.Visible = false;
                    LoadNSData();
                }
            }

            string test = UnderSectionCmb.EditValue.ToString();

            FirtsLoad = "N";
        }

        private void UnderSectionCmb_EditValueChanged(object sender, EventArgs e)
        {
            sec = UnderSectionCmb.EditValue.ToString();

            for (int x = 0; x <= dtSections.Rows.Count - 1; x++)
            {
                if (sec == dtSections.Rows[x][0].ToString())
                {
                    Hier = dtSections.Rows[x]["Hierarchicalid"].ToString();
                    if (FirtsLoad == "N")
                        MainLoad();

                    break;
                }
            }
        }

        private void radioGroup1_EditValueChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue.ToString() == "Prodmonth")
            {
                FromDate.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                ToDate.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barProdmonth.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                FromDate.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                ToDate.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                barProdmonth.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                if (radioGroup1.EditValue.ToString() == "Last 7 Days")
                {
                    FromDate.EditValue = DateTime.Now.AddDays(-7);
                    ToDate.EditValue = DateTime.Now.AddDays(0);
                }

                if (radioGroup1.EditValue.ToString() == "Last 30 Days")
                {
                    FromDate.EditValue = DateTime.Now.AddDays(-30);
                    ToDate.EditValue = DateTime.Now.AddDays(0);
                }
            }
            if (FirtsLoad == "N")
                MainLoad();
        }

        private void TypeRgb_EditValueChanged(object sender, EventArgs e)
        {
            if (TypeRgb.EditValue.ToString() == "Stoping")
                SqmAdvBtn.Text = "Sqm";
            if (TypeRgb.EditValue.ToString() == "Development")
                SqmAdvBtn.Text = "Adv";

            if (FirtsLoad == "N")
                MainLoad();
        }

        private void barProdmonth_EditValueChanged(object sender, EventArgs e)
        {
            LoadSection();
            if (FirtsLoad == "N")
                MainLoad();
        }

        void loadData()
        {
            String Lbl2 = string.Empty;

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
                Lbl2 = "Prodmonth- " + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
            else
                Lbl2 = "From- " + String.Format("{0:dd-MMM-yyyy}", FromDate.EditValue) + "   To- " + String.Format("{0:dd-MMM-yyyy}", ToDate.EditValue);

            int NoLostBlasts = 0;

            // do no blasts
            MWDataManager.clsDataAccess _dbMan7 = new MWDataManager.clsDataAccess();
            _dbMan7.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan7.SqlStatement = "select sum(dp) dp, sum(bb) bb \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "from (select case when bookcode = 'DP' then 1 else 0 end as dp, \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "case when booktons > 0 then 1 else 0 end as bb  from tbl_Planning p,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4 \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "where p.prodmonth <> '200000' \r\n ";

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' \r\n ";
            }
            if (radioGroup1.EditValue.ToString() != "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' \r\n ";
            }

            if (TypeRgb.EditValue.ToString() == "Stoping")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and activity <> 1 \r\n ";
            }
            if (TypeRgb.EditValue.ToString() != "Stoping")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and activity = 1 \r\n ";
            }

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s1.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s2.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s3.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s4.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.SectionID = s.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

            if (Hier == "1")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "2")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "3")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "4")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "5")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "6")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
            }
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ") a  ";

            _dbMan7.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan7.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan7.ResultsTableName = "Data";
            //textBox1.Text = _dbMan7.SqlStatement;
            _dbMan7.ExecuteInstruction();
            DataTable Neil = _dbMan7.ResultsDataTable;

            NoBlastsLbl.Text = Neil.Rows[0]["bb"].ToString();
            NoDualBlastsLbl.Text = Neil.Rows[0]["dp"].ToString();


            if (TypeRgb.EditValue.ToString() == "Stoping")
            {

                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

                _dbMan.SqlStatement = " select *, sqm1*advblast sqm, tons1 *advblast  tons, oz1 *advblast  oz, BookSQM  from (select '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lb2, a.*, b.ProblemDesc as ProbDescription, s1.ReportToSectionid mo, \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "c.Description as GroupDesc, d.Description as WPDescription,''EnquirerID, ''hqcat, pm.fl sqm1, pm.fl*sw/100*dens tons1, convert(decimal(18,0),((pm.fl/100*dens) * cmgt)/1000)  oz1, pp.booksqm BookSQM, pp.BookTons, pp.BookGrams/1000 BookKGs, wt.WpSublocation Mprass, wpexternalid \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "from tbl_ProblemBook a, tbl_Code_Problem_Main b, tbl_Code_Problemgroup c, tbl_Workplace d,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4, tbl_PlanMonth pm, tbl_Planning pp, tbl_WORKPLACE_Total wt \r\n ";

                if (radioGroup1.EditValue.ToString() == "Prodmonth")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + "where a.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and a.ProblemID = b.ProblemID \r\n ";
                }
                if (radioGroup1.EditValue.ToString() != "Prodmonth")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + "where a.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and a.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' and a.ProblemID = b.ProblemID \r\n ";
                }

                _dbMan.SqlStatement = _dbMan.SqlStatement + "and d.GMSIWPID = wt.GMSIWPID  \r\n ";

                _dbMan.SqlStatement = _dbMan.SqlStatement + "and b.ProblemGroup = c.ProblemGroupCode \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.WorkplaceID = d.WorkplaceID \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.Activity in (0,9) \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.ProblemID = b.ProblemID \r\n ";

                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.WorkplaceID = pp.WorkplaceID \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.calendardate = pp.calendardate \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.sectionid = pp.sectionid \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.prodmonth = pp.prodmonth \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.Activity = pp.Activity \r\n ";

                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.workplaceid = pm.workplaceid and a.sectionid = pm.sectionid and a.activity = pm.activity and a.prodmonth = pm.prodmonth \r\n ";

                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.Prodmonth = s.Prodmonth \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.Prodmonth = s1.Prodmonth \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.Prodmonth = s2.Prodmonth \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.Prodmonth = s3.Prodmonth \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.Prodmonth = s4.Prodmonth \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and a.SectionID = s.SectionID \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

                if (Hier == "1")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "2")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "3")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "4")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "5")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "6")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
                }

                if (ProbGroupCmb.EditValue.ToString() != "All")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " and c.Description = '" + ProbGroupCmb.EditValue + "' \r\n ";
                }

                if (HQCatCmb.EditValue.ToString() != "All")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " and b.HQCat = '" + HQCatCmb.EditValue + "' \r\n ";
                }

                if (ProblemsRgb.EditValue.ToString() == "Lost Blasts Optimal")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " and CausedLostBlast = 'Y' \r\n ";
                }
                else
                {
                    if (ProblemsRgb.EditValue.ToString() == "No Lost Blasts")
                    {
                        _dbMan.SqlStatement = _dbMan.SqlStatement + " and CausedLostBlast = 'N' \r\n ";
                    }

                    if (ProblemsRgb.EditValue.ToString() == "Plan To Be Blasted Lost Blast")
                    {
                        _dbMan.SqlStatement = _dbMan.SqlStatement + " and booktons = 0 and pp.MOCycle in ('BL','SUBL') \r\n ";
                    }
                }

                _dbMan.SqlStatement = _dbMan.SqlStatement + " ) a left outer join (select sectionid, stopingcycle, DevCycle from tbl_Cycle_MOCycleConfig ) bb  on a.mo = bb.sectionid left outer join tbl_WorkPlace_PlanningDefaults def on a.workplaceid = def.workplaceid \r\n ";

                _dbMan.SqlStatement = _dbMan.SqlStatement + "left outer join \r\n ";

                _dbMan.SqlStatement = _dbMan.SqlStatement + "(select Name Name1, avg(advBlast) advBlast from tbl_Cycle_RawData \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " group by Name) zzz on bb.stopingcycle = zzz.Name1 \r\n ";

                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "Data";
                //textBox1.Text = _dbMan.SqlStatement;
                _dbMan.ExecuteInstruction();

                foreach (DataRow dr in _dbMan.ResultsDataTable.Rows)
                {
                    if (dr["CausedLostBlast"].ToString() == "Y")
                    {
                        NoLostBlasts = NoLostBlasts + 1;
                    }
                }

                DataSet dsData = new DataSet();
                dsData.Tables.Add(_dbMan.ResultsDataTable);

                theReportprob.RegisterData(dsData);

                theReportprob.Load(_reportFolder + "ProblemHistoryData.frx");
                // theReportprob.Design();

                NoProblemsLbl.Text = _dbMan.ResultsDataTable.Rows.Count.ToString();
                LostBlastLbl.Text = NoLostBlasts.ToString();

                pcReportprob.Clear();
                theReportprob.Prepare();
                theReportprob.Preview = pcReportprob;

                theReportprob.ShowPrepared();
            }
            else
            {
                MWDataManager.clsDataAccess _dbManDev = new MWDataManager.clsDataAccess();
                _dbManDev.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

                _dbManDev.SqlStatement = "select *, sqm1*advblast sqm, tons1 *advblast  tons, Convert(decimal(10,3),oz1 *advblast)  oz  from (select '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lb2, a.*, b.ProblemDesc as ProbDescription, d.EndTypeID, s1.ReportToSectionid mo, \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "c.Description as GroupDesc, d.Description as WPDescription,''EnquirerID, ''hqcat, 1 sqm1,  Convert(decimal(10,3),Convert(decimal(10,3),1) * pm.DHeight * pm.DWidth * pm.Dens) tons1, Convert(decimal(10,3),((Convert(decimal(10,3),1) * pm.cmgt/100 * pm.DWidth * pm.Dens) )/1000) oz1, pp.bookAdv BookSQM, pp.BookTons, pp.BookGrams/1000 BookKGs, wt.WpSublocation Mprass, wpexternalid \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "from tbl_ProblemBook a, tbl_Code_Problem_Main b, tbl_Code_Problemgroup c, tbl_Workplace d,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4, tbl_PlanMonth pm, tbl_Planning pp, tbl_WORKPLACE_Total wt \r\n ";
                if (radioGroup1.EditValue.ToString() == "Prodmonth")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + "where a.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and a.ProblemID = b.ProblemID \r\n ";
                }
                if (radioGroup1.EditValue.ToString() != "Prodmonth")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + "where a.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and a.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' and a.ProblemID = b.ProblemID \r\n ";
                }

                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and d.GMSIWPID = wt.GMSIWPID  \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and b.ProblemGroup = c.ProblemGroupCode \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.WorkplaceID = d.WorkplaceID \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.Activity in (1) \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.ProblemID = b.ProblemID \r\n ";

                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.WorkplaceID = pp.WorkplaceID \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.calendardate = pp.calendardate \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.sectionid = pp.sectionid \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.prodmonth = pp.prodmonth \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.Activity = pp.Activity \r\n ";

                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.workplaceid = pm.workplaceid and a.sectionid = pm.sectionid and a.activity = pm.activity and a.prodmonth = pm.prodmonth \r\n ";

                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.Prodmonth = s.Prodmonth \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.Prodmonth = s1.Prodmonth \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.Prodmonth = s2.Prodmonth \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.Prodmonth = s3.Prodmonth \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.Prodmonth = s4.Prodmonth \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and a.SectionID = s.SectionID \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

                if (Hier == "1")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "2")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "3")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "4")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "5")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "6")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
                }

                if (ProbGroupCmb.EditValue.ToString() != "All")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and c.Description = '" + ProbGroupCmb.EditValue + "' \r\n ";
                }

                if (HQCatCmb.EditValue.ToString() != "All")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and b.HQCat = '" + HQCatCmb.EditValue + "' \r\n ";
                }

                if (ProblemsRgb.EditValue.ToString() == "Lost Blasts Optimal")
                {
                    _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and CausedLostBlast = 'Y' \r\n ";
                }
                else
                {
                    if (ProblemsRgb.EditValue.ToString() == "No Lost Blasts")
                    {
                        _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and CausedLostBlast = 'N' \r\n ";
                    }

                    if (ProblemsRgb.EditValue.ToString() == "Plan To Be Blasted Lost Blast")
                    {
                        _dbManDev.SqlStatement = _dbManDev.SqlStatement + " and booktons = 0 and pp.MOCycle in ('BL','SUBL') \r\n ";
                    }
                }



                _dbManDev.SqlStatement = _dbManDev.SqlStatement + " ) a left outer join (select sectionid, stopingcycle, DevCycle from tbl_Cycle_MOCycleConfig ) bb  on a.mo = bb.sectionid left outer join tbl_WorkPlace_PlanningDefaults def on a.workplaceid = def.workplaceid \r\n ";

                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "left outer join \r\n ";

                _dbManDev.SqlStatement = _dbManDev.SqlStatement + "(select Name Name1, fl Endtype1, avg(advBlast) advBlast from tbl_Cycle_RawData \r\n ";
                _dbManDev.SqlStatement = _dbManDev.SqlStatement + " group by Name, fl) zzz on bb.devcycle = zzz.Name1 and a.endtypeid = zzz.endtype1 \r\n  ";


                _dbManDev.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDev.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDev.ResultsTableName = "Data";
                //textBox1.Text = _dbMan.SqlStatement;
                _dbManDev.ExecuteInstruction();

                foreach (DataRow dr in _dbManDev.ResultsDataTable.Rows)
                {
                    if (dr["CausedLostBlast"].ToString() == "Y")
                    {
                        NoLostBlasts = NoLostBlasts + 1;
                    }
                }
                NoProblemsLbl.Text = _dbManDev.ResultsDataTable.Rows.Count.ToString();
                LostBlastLbl.Text = NoLostBlasts.ToString();

                DataSet dsData = new DataSet();
                dsData.Tables.Add(_dbManDev.ResultsDataTable);

                theReportprob.RegisterData(dsData);

                theReportprob.Load(_reportFolder + "ProblemHistoryData.frx");
                //theReportprob.Design();

                NoProblemsLbl.Text = _dbManDev.ResultsDataTable.Rows.Count.ToString();

                pcReportprob.Clear();
                theReportprob.Prepare();
                theReportprob.Preview = pcReportprob;
                theReportprob.ShowPrepared();
            }
        }

        private void NumberBtn_Click(object sender, EventArgs e)
        {
            lblGraph.Text = "selected for the Number";
            loadGraph();
        }

        void loadGraph()
        {
            String Lbl2 = string.Empty;

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
                Lbl2 = "Prodmonth- " + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
            else
                Lbl2 = "From- " + String.Format("{0:dd-MMM-yyyy}", FromDate.EditValue) + "   To- " + String.Format("{0:dd-MMM-yyyy}", ToDate.EditValue);

            if (TypeRgb.EditValue.ToString() == "Stoping")
            {
                MWDataManager.clsDataAccess _dbManGraph = new MWDataManager.clsDataAccess();
                _dbManGraph.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

                if (GraphViewTypergb.EditValue.ToString() == "Itemised")
                {
                    if (lblGraph.Text == "selected for the Number")
                    {
                        _dbManGraph.SqlStatement = "select Top 10 description, sum(TheCount) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1 TheCount  from (select \r\n " +
                             " sum(count11) [TheCount1], SUM(BookSqm) BookSQM, SUM(BookTons) BookTons, SUM(BookKgs) BookKgs, wwwwp, Description [Description], max(mo) mo \r\n " +
                                              "from ";
                    }
                    else
                    {
                        //_dbManGraph.SqlStatement = "select Top 10 description, sum(TheCount) TheCount, '" + ProductionAmplatsGlobalTSysSettings.Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(UnderSectionCmb.Text) + "' TheSection, '" + HQCatCmb.Text + "' theHqCat, '" + ProbGroupCmb.Text + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n " +
                        if (lblGraph.Text == "selected for the Sqm")
                            _dbManGraph.SqlStatement = "select Top 10 description, sum(TheCount)-SUM(BookSQM) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n ";
                        if (lblGraph.Text == "selected for the Stope Tons")
                            _dbManGraph.SqlStatement = "select Top 10 description, sum(TheCount)-SUM(BookTons) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n ";
                        if (lblGraph.Text == "selected for the Stope Ounces")
                            _dbManGraph.SqlStatement = "select Top 10 description, sum((TheCount*35.274))-SUM((BookKgs*35.274)) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n ";

                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " sum(count11) [TheCount1], SUM(BookSqm) BookSQM, SUM(BookTons) BookTons, SUM(BookKgs) BookKgs, wwwwp, Description [Description], max(mo) mo \r\n " +
                                             "from ";
                    }

                    if (lblGraph.Text == "selected for the Number")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select 1 count11, BookSQM, BookTons, BookGrams/1000 BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Sqm")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select Convert(decimal(10,0),(pm.FL * 1)) count11, BookSQM, BookTons, BookGrams/1000 BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Stope Tons")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select Convert(decimal(10,0),pm.FL * 1*pm.Dens*pm.SW/100) count11, BookSQM, BookTons, BookGrams/1000 BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Stope Ounces")
                        //_dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select Convert(decimal(10,0),((pm.FL * 1*pm.Dens) * pm.cmGT/100 )/31.10348) count11, \r\n ";
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select Convert(decimal(10,3),((pm.FL * 1*pm.Dens) * pm.cmGT/100 )/1000) count11, BookSQM, BookTons, BookGrams/1000 BookKgs, \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " d.workplaceid wwwwp, b.ProblemDesc Description, s1.ReportToSectionid mo from tbl_ProblemBook a, tbl_Code_Problem_Main b, tbl_Code_Problemgroup c, tbl_Workplace d,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4, tbl_PlanMonth pm, tbl_Planning pp \r\n ";

                    if (radioGroup1.EditValue.ToString() == "Prodmonth")
                    {
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "where a.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }
                    if (radioGroup1.EditValue.ToString() != "Prodmonth")
                    {
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "where a.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and a.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.prodmonth = pm.prodmonth and a.sectionid = pm.sectionid and a.workplaceid = pm.workplaceid and a.activity = pm.activity \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and b.ProblemGroup = c.ProblemGroupCode \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.WorkplaceID = d.WorkplaceID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Activity in (0,9) \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.ProblemID = b.ProblemID \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s1.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s2.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s3.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s4.Prodmonth \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.WorkplaceID = pp.WorkplaceID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.calendardate = pp.calendardate \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.sectionid = pp.sectionid \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.prodmonth = pp.prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Activity = pp.Activity \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.SectionID = s.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";
                }

                if (GraphViewTypergb.EditValue.ToString() == "Prob Group")
                {
                    if (lblGraph.Text == "selected for the Number")
                    {
                        _dbManGraph.SqlStatement = "select Top 10 description, sum(TheCount) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1 TheCount  from (select \r\n " +
                             " sum(count11) [TheCount1], SUM(BookSqm) BookSQM, SUM(BookTons) BookTons, SUM(BookKgs) BookKgs, wwwwp, Description [Description], max(mo) mo \r\n " +
                                              "from ";
                    }
                    else
                    {
                        if (lblGraph.Text == "selected for the Sqm")
                            _dbManGraph.SqlStatement = "select Top 10 description, sum(TheCount)-SUM(BookSQM) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n ";
                        if (lblGraph.Text == "selected for the Stope Tons")
                            _dbManGraph.SqlStatement = "select Top 10 description, sum(TheCount)-SUM(BookTons) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n ";
                        if (lblGraph.Text == "selected for the Stope Ounces")
                            _dbManGraph.SqlStatement = "select Top 10 description, sum((TheCount*35.274))-SUM((BookKgs*35.274)) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n ";

                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " sum(count11) [TheCount1], SUM(BookSqm) BookSQM, SUM(BookTons) BookTons, SUM(BookKgs) BookKgs, wwwwp, Description [Description], max(mo) mo \r\n " +
                                         "from ";
                    }
                    if (lblGraph.Text == "selected for the Number")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select 1 count11, BookSQM, BookTons, (BookGrams/1000*35.274) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Sqm")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select Convert(decimal(10,0),(pm.FL * 1)) count11, BookSQM, BookTons, (BookGrams/1000*35.274) BookKgs,  \r\n ";
                    if (lblGraph.Text == "selected for the Stope Tons")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select Convert(decimal(10,0),pm.FL *1*pm.Dens*pm.SW/100) count11, BookSQM, BookTons, (BookGrams/1000*35.274) BookKgs,  \r\n ";
                    if (lblGraph.Text == "selected for the Stope Ounces")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select Convert(decimal(10,3),((pm.FL * 1*pm.Dens*pm.cmgt/100) )/1000) count11, BookSQM, BookTons, BookGrams/1000 BookKgs, \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " d.workplaceid wwwwp, s1.ReportToSectionid mo, b.ProblemGroup Description from tbl_ProblemBook a, tbl_Code_Problem_Main b, tbl_Code_Problemgroup c, tbl_Workplace d,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4, tbl_PlanMonth pm, tbl_Planning pp \r\n ";
                    if (radioGroup1.EditValue.ToString() == "Prodmonth")
                    {
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "where a.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }
                    if (radioGroup1.EditValue.ToString() != "Prodmonth")
                    {
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "where a.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and a.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.prodmonth = pm.prodmonth and a.sectionid = pm.sectionid and a.workplaceid = pm.workplaceid and a.activity = pm.activity \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and b.ProblemGroup = c.ProblemGroupCode \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.WorkplaceID = d.WorkplaceID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Activity in (0,9) \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.ProblemID = b.ProblemID \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s1.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s2.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s3.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s4.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.SectionID = s.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.WorkplaceID = pp.WorkplaceID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.calendardate = pp.calendardate \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.sectionid = pp.sectionid \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.prodmonth = pp.prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Activity = pp.Activity \r\n ";
                }

                if (GraphViewTypergb.EditValue.ToString() == "HO Cat")
                {
                    if (lblGraph.Text == "selected for the Number")
                    {
                        _dbManGraph.SqlStatement = "select Top 10 description, sum(TheCount) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1 TheCount  from (select \r\n " +
                             " sum(count11) [TheCount1], SUM(BookSqm) BookSQM, SUM(BookTons) BookTons, SUM(BookKgs) BookKgs, wwwwp, ProblemDesc [Description], max(mo) mo \r\n " +
                                              "from \r\n ";
                    }
                    else
                    {
                        if (lblGraph.Text == "selected for the Sqm")
                            _dbManGraph.SqlStatement = "select Top 10 ProblemDesc, sum(TheCount)-SUM(BookSQM) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n ";
                        if (lblGraph.Text == "selected for the Stope Tons")
                            _dbManGraph.SqlStatement = "select Top 10 ProblemDesc, sum(TheCount)-SUM(BookTons) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n ";
                        if (lblGraph.Text == "selected for the Stope Ounces")
                            _dbManGraph.SqlStatement = "select Top 10 ProblemDesc, sum((TheCount*35.274))-SUM((BookKgs*35.274)) TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from (select *, TheCount1*advblast TheCount from (select \r\n ";

                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " sum(count11) [TheCount1], SUM(BookSqm) BookSQM, SUM(BookTons) BookTons, SUM(BookKgs) BookKgs, wwwwp, Description [Description], max(mo) mo \r\n " +
                                              "from \r\n ";
                    }


                    if (lblGraph.Text == "selected for the Number")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select 1 count11, BookSQM, BookTons, BookGrams/1000 BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Sqm")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select Convert(decimal(10,0),(pm.FL * 1)) count11, BookSQM, BookTons, BookGrams/1000 BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Stope Tons")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "( select Convert(decimal(10,0),pm.FL *1*pm.Dens*pm.SW/100) count11, BookSQM, BookTons, BookGrams/1000 BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Stope Ounces")
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "(select Convert(decimal(10,3),((pm.FL * 1*pm.Dens*pm.cmgt/100))/1000) count11, BookSQM, BookTons, (BookGrams/1000) BookKgs, \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "d.workplaceid wwwwp, s1.ReportToSectionid mo, b.HQCat Description from tbl_ProblemBook a, tbl_Code_Problem_Main b, tbl_Code_Problemgroup c, tbl_Workplace d,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4, tbl_PlanMonth pm, tbl_Planning pp \r\n ";

                    if (radioGroup1.EditValue.ToString() == "Prodmonth")
                    {
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "where a.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }
                    if (radioGroup1.EditValue.ToString() != "Prodmonth")
                    {
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "where a.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and a.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.prodmonth = pm.prodmonth and a.sectionid = pm.sectionid and a.workplaceid = pm.workplaceid and a.activity = pm.activity \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and b.ProblemGroup = c.ProblemGroupCode \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.WorkplaceID = d.WorkplaceID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Activity in (0,9) \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.ProblemID = b.ProblemID \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s1.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s2.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s3.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Prodmonth = s4.Prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.SectionID = s.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.WorkplaceID = pp.WorkplaceID \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.calendardate = pp.calendardate \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.sectionid = pp.sectionid \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.prodmonth = pp.prodmonth \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "and a.Activity = pp.Activity \r\n ";
                }

                if (Hier == "1")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "2")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "3")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "4")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "5")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "6")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
                }

                if (ProbGroupCmb.EditValue.ToString() != "All")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and c.Description = '" + ProbGroupCmb.EditValue.ToString() + "' \r\n ";
                }

                if (HQCatCmb.EditValue.ToString() != "All")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and b.HQCat = '" + HQCatCmb.EditValue + "' \r\n ";
                }

                if (ProblemsRgb.EditValue.ToString() == "Lost Blasts Optimal")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and CausedLostBlast = 'Y' \r\n ";
                }
                else
                {
                    if (ProblemsRgb.EditValue.ToString() == "No Lost Blasts")
                    {
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and CausedLostBlast = 'N' \r\n ";
                    }

                    if (ProblemsRgb.EditValue.ToString() == "Plan To Be Blasted Lost Blast")
                    {
                        _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and booktons = 0 and pp.MOCycle in ('BL','SUBL') \r\n ";
                    }
                }

                if (GraphViewTypergb.EditValue.ToString() == "Itemised")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + ") a group by wwwwp, Description) a \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "left outer join (select sectionid   sss, stopingcycle, DevCycle from tbl_Cycle_MOCycleConfig ) bb  on a.mo = bb.sss left outer join tbl_WorkPlace_PlanningDefaults def on a.wwwwp = def.workplaceid \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "left outer join \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "(select Name Name1, avg(advBlast) advBlast from tbl_Cycle_RawData \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " group by Name) zzz on bb.stopingcycle = zzz.Name1) a group by  Description order by theCount Desc \r\n ";
                }

                if (GraphViewTypergb.EditValue.ToString() == "Prob Group")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + ") a group by wwwwp, Description) a \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "left outer join (select sectionid   sss, stopingcycle, DevCycle from tbl_Cycle_MOCycleConfig ) bb  on a.mo = bb.sss left outer join tbl_WorkPlace_PlanningDefaults def on a.wwwwp = def.workplaceid \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "left outer join \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "(select Name Name1, avg(advBlast) advBlast from tbl_Cycle_RawData \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " group by Name) zzz on bb.stopingcycle = zzz.Name1) a group by  Description order by theCount Desc \r\n ";
                }

                if (GraphViewTypergb.EditValue.ToString() == "HO Cat")
                {
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + ") a group by wwwwp, Description) a \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "left outer join (select sectionid   sss, stopingcycle, DevCycle from tbl_Cycle_MOCycleConfig ) bb  on a.mo = bb.sss left outer join tbl_WorkPlace_PlanningDefaults def on a.wwwwp = def.workplaceid \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "left outer join \r\n ";

                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + "(select Name Name1, avg(advBlast) advBlast from tbl_Cycle_RawData \r\n ";
                    _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " group by Name) zzz on bb.stopingcycle = zzz.Name1) a group by  Description order by theCount Desc \r\n ";
                }
                _dbManGraph.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGraph.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGraph.ResultsTableName = "Graph";
                //textBox1.Text = _dbManGraph.SqlStatement;
                _dbManGraph.ExecuteInstruction();

                DataSet dsGraph = new DataSet();

                dsGraph.Tables.Add(_dbManGraph.ResultsDataTable);
                theReport2prob.RegisterData(dsGraph);
            }
            else
            {
                MWDataManager.clsDataAccess _dbManGraphDev = new MWDataManager.clsDataAccess();
                _dbManGraphDev.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                if (GraphViewTypergb.EditValue.ToString() == "Itemised")
                {
                    _dbManGraphDev.SqlStatement = "select Top 10 '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup, \r\n ";

                    if (lblGraph.Text == "selected for the Number")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " sum(count1) [TheCount], Description [Description] \r\n ";
                    if (lblGraph.Text == "selected for the Development meters")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " sum(count1)-SUM(BookAdv) [TheCount1], Description [Description], SUM(advBlast)-SUM(BookAdv) TheCount \r\n ";
                    if (lblGraph.Text == "selected for the Development Tons")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " sum(count1)-SUM(BookTons) [TheCount1], Description [Description], sum(advBlast * count1)-SUM(BookTons) TheCount \r\n ";
                    if (lblGraph.Text == "selected for the Development Ounces")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " (sum((count1*35.274))-SUM((BookKgs*35.274)) [TheCount1], Description [Description], (sum(advBlast * count1)-SUM(BookKgs)*35.274) [TheCount] \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "from ";

                    if (lblGraph.Text == "selected for the Number")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select 1 count1, BookAdv, BookTons, ((BookGrams/1000)*35.274) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Development meters")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select 1.5 count1, BookAdv, BookTons, ((BookGrams/1000)*35.274) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Development Tons")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select Convert(decimal(10,0),pm.DHeight * pm.DWidth * pm.Dens) count1, BookAdv, BookTons, ((BookGrams/1000)*35.274) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Development Ounces")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select Convert(decimal(10,3),((pm.cmgt/100 * pm.DWidth * pm.Dens) )/1000)  count1, BookAdv, BookTons, (BookGrams/1000) BookKgs, \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " b.ProblemDesc Description, b.ProblemDesc wp1, d.endtypeid, s1.ReportToSectionid mo from tbl_ProblemBook a, tbl_Code_Problem_Main b, tbl_Code_Problemgroup c, tbl_Workplace d,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4, tbl_PlanMonth pm, tbl_Planning pp \r\n ";

                    if (radioGroup1.EditValue.ToString() == "Prodmonth")
                    {
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "where a.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and a.ProblemID = b.ProblemID  \r\n";
                    }
                    if (radioGroup1.EditValue.ToString() != "Prodmonth")
                    {
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "where a.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and a.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.prodmonth = pm.prodmonth and a.sectionid = pm.sectionid and a.workplaceid = pm.workplaceid and a.activity = pm.activity \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and b.ProblemGroup = c.ProblemGroupCode \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.WorkplaceID = d.WorkplaceID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Activity in (1) \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.ProblemID = b.ProblemID \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s1.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s2.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s3.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s4.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.SectionID = s.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.WorkplaceID = pp.WorkplaceID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.calendardate = pp.calendardate \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.sectionid = pp.sectionid \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.prodmonth = pp.prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Activity = pp.Activity \r\n ";
                }

                if (GraphViewTypergb.EditValue.ToString() == "Prob Group")
                {
                    _dbManGraphDev.SqlStatement = "select Top 10 '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup, \r\n ";

                    if (lblGraph.Text == "selected for the Number")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "sum(count1) [TheCount], Description [Description] \r\n ";
                    if (lblGraph.Text == "selected for the Development meters")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "sum(count1)-sum(BookAdv) [TheCount1], Description [Description], SUM(advBlast)-SUM(BookAdv) TheCount \r\n ";
                    if (lblGraph.Text == "selected for the Development Tons")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "sum(count1)-sum(BookTons) [TheCount1], Description [Description], sum(advBlast * count1)-SUM(BookTons) TheCount \r\n ";
                    if (lblGraph.Text == "selected for the Development Ounces")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "sum((count1*35.274))-sum((BookKgs*35.274)) [TheCount1], Description [Description], (sum(advBlast * count1)*35.274)-(SUM(BookKgs)*35.274) [TheCount] \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "from \r\n ";

                    if (lblGraph.Text == "selected for the Number")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select 1 count1, BookAdv, BookTons, (BookGrams/1000) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Development meters")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select 1.5 count1, BookAdv, BookTons, (BookGrams/1000) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Development Tons")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select Convert(decimal(10,0),pm.DHeight * pm.DWidth * pm.Dens) count1, BookAdv, BookTons, ((BookGrams/1000)*35.274) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Development Ounces")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select Convert(decimal(10,3),((pm.cmgt/100 * pm.DWidth * pm.Dens) )/1000)  count1, BookAdv, BookTons, (BookGrams/1000) BookKgs, \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "b.ProblemGroup Description, d.endtypeid, s1.ReportToSectionid mo, b.Description wp1 from tbl_ProblemBook a, tbl_Code_Problem_Main b, tbl_Code_Problemgroup c, tbl_Workplace d,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4, tbl_PlanMonth pm , tbl_Planning pp \r\n ";

                    if (radioGroup1.EditValue.ToString() == "Prodmonth")
                    {
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "where a.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }
                    if (radioGroup1.EditValue.ToString() != "Prodmonth")
                    {
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "where a.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and a.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' and a.ProblemID = b.ProblemID  \r\n ";
                    }
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.prodmonth = pm.prodmonth and a.sectionid = pm.sectionid and a.workplaceid = pm.workplaceid \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and b.ProblemGroup = c.ProblemGroupCode \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.WorkplaceID = d.WorkplaceID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Activity in (1) \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.ProblemID = b.ProblemID \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s1.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s2.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s3.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s4.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.SectionID = s.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.WorkplaceID = pp.WorkplaceID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.calendardate = pp.calendardate \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.sectionid = pp.sectionid \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.prodmonth = pp.prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Activity = pp.Activity \r\n ";
                }

                if (GraphViewTypergb.EditValue.ToString() == "HO Cat")
                {
                    _dbManGraphDev.SqlStatement = "select Top 10 '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup, \r\n ";

                    if (lblGraph.Text == "selected for the Number")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "sum(count1) [TheCount], Description [Description] \r\n ";
                    if (lblGraph.Text == "selected for the Development meters")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "sum(count1)-sum(BookAdv) [TheCount1], Description [Description], SUM(advBlast)-SUM(BookAdv) TheCount \r\n ";
                    if (lblGraph.Text == "selected for the Development Tons")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "sum(count1)-sum(BookTons) [TheCount1], Description [Description], sum(advBlast * count1)-SUM(BookTons) TheCount \r\n ";
                    if (lblGraph.Text == "selected for the Development Ounces")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "sum((count1*35.274))-sum((BookKgs*35.274)) [TheCount1], Description [Description], sum((advBlast * count1*35.274))-SUM((BookKgs*35.274)) [TheCount] \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "from  \r\n ";

                    if (lblGraph.Text == "selected for the Number")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select *, count count1 from (select 1 count, BookAdv, BookTons, (BookGrams/1000) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Development meters")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select *, count * advBlast count1 from (select 1 count, BookAdv, BookTons, (BookGrams/1000) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Development Tons")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select *, count * advBlast count1 from (select Convert(decimal(10,0),pm.DHeight * pm.DWidth * pm.Dens) count, BookAdv, BookTons, ((BookGrams/1000)*35.274) BookKgs, \r\n ";
                    if (lblGraph.Text == "selected for the Development Ounces")
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "( select *, count * advBlast count1 from select (Convert(decimal(10,3),((pm.cmgt/100 * pm.DWidth * pm.Dens) )/1000)  count, BookAdv, BookTons, (BookGrams/1000) BookKgs, \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "b.HQCat Description, d.endtypeid, s1.ReportToSectionid mo, d.workplaceid wp1 from tbl_ProblemBook a, tbl_Code_Problem_Main b, tbl_Code_Problemgroup c, tbl_Workplace d,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4, tbl_PlanMonth pm , tbl_Planning pp \r\n ";

                    if (radioGroup1.EditValue.ToString() == "Prodmonth")
                    {
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "where a.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }
                    if (radioGroup1.EditValue.ToString() != "Prodmonth")
                    {
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "where a.calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and a.calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' and a.ProblemID = b.ProblemID \r\n ";
                    }

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.prodmonth = pm.prodmonth and a.sectionid = pm.sectionid and a.workplaceid = pm.workplaceid and a.activity = pm.activity \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and b.ProblemGroup = c.ProblemGroupCode \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.WorkplaceID = d.WorkplaceID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Activity in (1) \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.ProblemID = b.ProblemID \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s1.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s2.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s3.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Prodmonth = s4.Prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.SectionID = s.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.WorkplaceID = pp.WorkplaceID \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.calendardate = pp.calendardate \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.sectionid = pp.sectionid \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.prodmonth = pp.prodmonth \r\n ";
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "and a.Activity = pp.Activity \r\n ";
                }

                if (Hier == "1")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "2")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "3")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "4")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "5")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
                }
                if (Hier == "6")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
                }

                if (ProbGroupCmb.EditValue.ToString() != "All")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and c.Description = '" + ProbGroupCmb.EditValue.ToString() + "' \r\n ";
                }

                if (HQCatCmb.EditValue.ToString() != "All")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and b.HQCat = '" + HQCatCmb.EditValue.ToString() + "' \r\n ";
                }

                if (ProblemsRgb.EditValue.ToString() == "Lost Blasts Optimal")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and CausedLostBlast = 'Y' \r\n ";
                }
                else
                {
                    if (ProblemsRgb.EditValue.ToString() == "No Lost Blasts")
                    {
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and CausedLostBlast = 'N' \r\n ";
                    }

                    if (ProblemsRgb.EditValue.ToString() == "Plan To Be Blasted Lost Blast")
                    {
                        _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " and booktons = 0 and pp.MOCycle in ('BL','SUBL') \r\n ";
                    }
                }
                _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " ) a left outer join (select sectionid sec1111, stopingcycle, DevCycle from tbl_Cycle_MOCycleConfig ) bb  on a.mo = bb.sec1111 left outer join tbl_WorkPlace_PlanningDefaults def on a.wp1 = def.workplaceid \r\n ";

                _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "left outer join \r\n ";

                _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + "(select Name Name1, fl Endtype1, avg(advBlast) advBlast from tbl_Cycle_RawData \r\n ";
                _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " group by Name, fl) zzz on bb.devcycle = zzz.Name1 and a.endtypeid = zzz.endtype1  \r\n ";

                if (GraphViewTypergb.EditValue.ToString() == "Itemised")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " group by Description order  by theCount Desc ";
                }

                if (GraphViewTypergb.EditValue.ToString() == "Prob Group")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + " group by Description order by theCount Desc";//group by b.ProblemGroupCode order by theCount Desc
                }

                if (GraphViewTypergb.EditValue.ToString() == "HO Cat")
                {
                    _dbManGraphDev.SqlStatement = _dbManGraphDev.SqlStatement + ")a group by Description order by theCount Desc";//group by b.HQCat order by theCount Desc
                }

                _dbManGraphDev.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGraphDev.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGraphDev.ResultsTableName = "Graph";
                //textBox1.Text = _dbManGraphDev.SqlStatement;
                _dbManGraphDev.ExecuteInstruction();
                DataSet dsGraph = new DataSet();
                dsGraph.Tables.Add(_dbManGraphDev.ResultsDataTable);

                theReport2prob.RegisterData(dsGraph);
            }
            theReport2prob.Load(_reportFolder + "ProblemHistoryGraph.frx");

            //theReport2prob.Design();

            pcReport2prob.Clear();
            theReport2prob.Prepare();
            theReport2prob.Preview = pcReport2prob;
            theReport2prob.ShowPrepared();
        }

        private void SqmAdvBtn_Click(object sender, EventArgs e)
        {
            if (TypeRgb.EditValue.ToString() == "Stoping")
                lblGraph.Text = "selected for the Sqm";
            if (TypeRgb.EditValue.ToString() == "Development")
                lblGraph.Text = "selected for the Development meters";

            loadGraph();
        }

        private void TonsBtn_Click(object sender, EventArgs e)
        {
            if (TypeRgb.EditValue.ToString() == "Stoping")
                lblGraph.Text = "selected for the Stope Tons";
            if (TypeRgb.EditValue.ToString() == "Development")
                lblGraph.Text = "selected for the Development Tons";

            loadGraph();
        }

        private void OuncesBtn_Click(object sender, EventArgs e)
        {
            if (TypeRgb.EditValue.ToString() == "Stoping")
                lblGraph.Text = "selected for the Stope Ounces";
            if (TypeRgb.EditValue.ToString() == "Development")
                lblGraph.Text = "selected for the Development Ounces";

            loadGraph();
        }

        private void Barbtn_Click(object sender, EventArgs e)
        {
            GraphType = "Bar";
            loadGraph();
            LoadComboGraph();
        }

        void LoadComboGraph()
        {
            MWDataManager.clsDataAccess _dbManGraph = new MWDataManager.clsDataAccess();
            _dbManGraph.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (ProblemsRgb.EditValue.ToString() == "Plan To Be Blasted Lost Blast")
                _dbManGraph.SqlStatement = "exec [Proc_LostBlastPlanCycle] @ProdMonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "'";
            else
                _dbManGraph.SqlStatement = "exec Proc_LostBlast @ProdMonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "'";

            _dbManGraph.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGraph.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGraph.ResultsTableName = "GraphCombo";
            _dbManGraph.ExecuteInstruction();
            DataSet dsGraphCombo = new DataSet();

            dsGraphCombo.Tables.Add(_dbManGraph.ResultsDataTable);

            theReport3prob.RegisterData(dsGraphCombo);

            MWDataManager.clsDataAccess _dbManMO = new MWDataManager.clsDataAccess();
            _dbManMO.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManMO.SqlStatement = " select '" + ProductionGlobalTSysSettings._Banner + "' banner, moid,MOName ,convert(integer,Sum(Tons)) Tons,COUNT(problemid) Problem \r\n" +
                                    " from lostblast  where Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' \r\n" +
                                    " group by MOID,MOName \r\n" +
                                    " order by MOID ";

            _dbManMO.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMO.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMO.ResultsTableName = "MO";
            _dbManMO.ExecuteInstruction();
            DataSet dsMO = new DataSet();

            dsMO.Tables.Add(_dbManMO.ResultsDataTable);

            theReport3prob.RegisterData(dsMO);

            theReport3prob.Load(_reportFolder + "ProblemHistoryParito.frx");
            //theReport3prob.Design();

            pcControl3prob.Clear();
            theReport3prob.Prepare();
            theReport3prob.Preview = pcControl3prob;
            theReport3prob.ShowPrepared();

            // Do Workplace Stoppage
            MWDataManager.clsDataAccess _dbManWPStopa1 = new MWDataManager.clsDataAccess();
            _dbManWPStopa1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPStopa1.SqlStatement = " select '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' MyProdmonth ";
            _dbManWPStopa1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPStopa1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPStopa1.ResultsTableName = "aaaaa";
            _dbManWPStopa1.ExecuteInstruction();
            DataSet WPStopa1 = new DataSet();
            WPStopa1.Tables.Add(_dbManWPStopa1.ResultsDataTable);

            theReportWPStop.RegisterData(WPStopa1);

            //First Do Stoping
            MWDataManager.clsDataAccess _dbManWPStop1 = new MWDataManager.clsDataAccess();
            _dbManWPStop1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPStop1.SqlStatement = "select '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' MyProdmonth, moid+':'+moname moid1, SUM(Sqm1) Sqm1, SUM(kgs1) kgs1 \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + ", SUM(Sqm2) Sqm2, SUM(kgs2) kgs2 \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + ", SUM(Sqm3) Sqm3, SUM(kgs3) kgs3 \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + ", SUM(Sqm4) Sqm4, SUM(kgs4) kgs4 \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + ", SUM(Sqm5) Sqm5, SUM(kgs5) kgs5 \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + ",SUM(sqm) sqmtot, SUM(kgs)  kgstot from ( \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "select 'Amandelbult' banner, \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Safety stoppage: Crew' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(fl*aa)-BookSqm else 0 end as Sqm1, \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Safety stoppage: Crew' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs else 0 end as kgs1, \r\n ";

            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Safety stoppage: Line Mngmt' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(fl*aa)-BookSqm else 0 end as Sqm2, \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Safety stoppage: Line Mngmt' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs else 0 end as kgs2, \r\n ";

            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Safety stoppage: Other' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(fl*aa)-BookSqm else 0 end as Sqm3, \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Safety stoppage: Other' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs else 0 end as kgs3, \r\n ";

            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Section 54' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(fl*aa)-BookSqm else 0 end as Sqm4, \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Section 54' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs else 0 end as kgs4, \r\n ";

            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Section 55' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(fl*aa)-BookSqm else 0 end as Sqm5, \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "case when prbdesc = 'Section 55' then \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs else 0 end as kgs5, \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "*, (fl*aa)-BookSqm sqm, (((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs kgs from (select s.MOID, MOName , w.Description wpdesc, \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "pb.ProblemID, p.ProblemDesc prbdesc, pm.FL, pm.SW, pm.CMGT, pm.Dens, pp.BookSqm, pp.BookGrams/1000 BookKgs \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "from tbl_ProblemBook pb, tbl_SectionComplete s, tbl_Workplace w, tbl_Code_Problem_Main p, tbl_PlanMonth pm, tbl_Planning pp \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "where pb.Prodmonth = s.prodmonth and pb.SectionID = s.SectionID and pb.WorkplaceID = w.WorkplaceID and \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "pb.ProblemID = p.ProblemID and pb.Prodmonth = pm.Prodmonth and pb.WorkplaceID = pm.Workplaceid and pb.SectionID = pm.Sectionid \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "and pb.Activity = pm.Activity and \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "pb.prodmonth = pp.Prodmonth and pb.CalendarDate = pp.CalendarDate and pb.sectionid = pp.SectionID and pb.WorkplaceID = pp.WorkplaceID and pb.Activity = pp.Activity and \r\n ";
            if (ProblemsRgb.EditValue.ToString() == "Plan To Be Blasted Lost Blast")
                _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "pb.problemid in ('E6','E9','E11','E12','E13','E14') and pb.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and pb.Activity <> 1 and booktons = 0 and pp.MOCycle in ('BL','SUBL') ) a \r\n ";
            else
                _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "pb.problemid in ('E6','E9','E11','E12','E13','E14') and pb.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and pb.Activity <> 1  ) a \r\n ";

            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "left outer join \r\n ";

            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(select * from \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(select Sectionid, StopingCycle from tbl_Cycle_MOCycleConfig \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "where StopingCycle <> '') a left outer join \r\n ";

            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "(select Name, max(advblast) aa from tbl_Cycle_RawData where [Type] = 'S' group by Name) b \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "on a.StopingCycle = b.Name) b \r\n ";
            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + "on a.moid = b.sectionid ) a group by moid,moname order by moid ";

            _dbManWPStop1.SqlStatement = _dbManWPStop1.SqlStatement + string.Empty;
            _dbManWPStop1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPStop1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPStop1.ResultsTableName = "Deatal11";

            _dbManWPStop1.ExecuteInstruction();
            DataSet WPStop1 = new DataSet();

            WPStop1.Tables.Add(_dbManWPStop1.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManWPStop = new MWDataManager.clsDataAccess();
            _dbManWPStop.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPStop.SqlStatement = "select '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' MyProdmonth, '" + ProductionGlobalTSysSettings._Banner + "' banner, \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "*, (fl*aa)-BookSqm sqm, (((FL*aa) *CMGT/100*Dens)/1000)-BookKgs kgs from (select s.MOID, MOName , w.Description wpdesc, \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "pb.ProblemID, p.ProblemDesc prbdesc, pm.FL, pm.SW, pm.CMGT, pm.Dens, BookSqm, BookGrams/1000 BookKgs \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "from tbl_ProblemBook pb, tbl_SectionComplete s, tbl_Workplace w, tbl_Code_Problem_Main p, tbl_PlanMonth pm, tbl_Planning pp  \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "where pb.Prodmonth = s.prodmonth and pb.SectionID = s.SectionID and pb.WorkplaceID = w.WorkplaceID and \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "pb.ProblemID = p.ProblemID and pb.Prodmonth = pm.Prodmonth and pb.WorkplaceID = pm.Workplaceid and pb.SectionID = pm.Sectionid \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "and pb.Activity = pm.Activity and  \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "pb.prodmonth = pp.Prodmonth and pb.CalendarDate = pp.CalendarDate and pb.sectionid = pp.SectionID and pb.WorkplaceID = pp.WorkplaceID and pb.Activity = pp.Activity and ";

            if (ProblemsRgb.EditValue.ToString() == "Plan To Be Blasted Lost Blast")
                _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "pb.problemid in ('E6','E9','E11','E12','E13','E14') and pb.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and pb.Activity <> 1  and booktons = 0 and pp.MOCycle in ('BL','SUBL')) a \r\n ";
            else
                _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "pb.problemid in ('E6','E9','E11','E12','E13','E14') and pb.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and pb.Activity <> 1 ) a \r\n ";

            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "left outer join \r\n ";

            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "(select * from \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "(select Sectionid, StopingCycle from tbl_Cycle_MOCycleConfig \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "where StopingCycle <> '') a left outer join \r\n ";

            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "(select Name, max(advblast) aa from tbl_Cycle_RawData where [Type] = 'S' group by Name) b \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "on a.StopingCycle = b.Name) b \r\n ";
            _dbManWPStop.SqlStatement = _dbManWPStop.SqlStatement + "on a.moid = b.sectionid ";
            _dbManWPStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPStop.ResultsTableName = "Deatal";
            //textBox1.Text = _dbManGraph.SqlStatement;
            _dbManWPStop.ExecuteInstruction();
            DataSet WPStop = new DataSet();

            WPStop.Tables.Add(_dbManWPStop.ResultsDataTable);

            theReportWPStop.RegisterData(WPStop);
            theReportWPStop.RegisterData(WPStop1);

            // Now do Development
            MWDataManager.clsDataAccess _dbManWPStop11 = new MWDataManager.clsDataAccess();
            _dbManWPStop11.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPStop11.SqlStatement = "select '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' MyProdmonth, moid+':'+moname moid1, SUM(Sqm1) Sqm1, SUM(kgs1) kgs1 \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + ", SUM(Sqm2) Sqm2, SUM(kgs2) kgs2 \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + ", SUM(Sqm3) Sqm3, SUM(kgs3) kgs3 \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + ", SUM(Sqm4) Sqm4, SUM(kgs4) kgs4 \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + ", SUM(Sqm5) Sqm5, SUM(kgs5) kgs5 \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + ",SUM(sqm) sqmtot, SUM(kgs)  kgstot from ( \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "select 'Moab Khotsong' banner, \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Safety stoppage: Crew' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(fl*aa)-BookAdv1 else 0 end as Sqm1, \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Safety stoppage: Crew' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs1 else 0 end as kgs1, \r\n ";

            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Safety stoppage: Line Mngmt' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(fl*aa)-BookAdv1 else 0 end as Sqm2, \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Safety stoppage: Line Mngmt' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs1 else 0 end as kgs2, \r\n ";

            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Safety stoppage: Other' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(fl*aa)-BookAdv1 else 0 end as Sqm3, \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Safety stoppage: Other' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs1 else 0 end as kgs3, \r\n ";

            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Section 54' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(fl*aa)-BookAdv1 else 0 end as Sqm4, \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Section 54' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs1 else 0 end as kgs4, \r\n ";

            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Section 55' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(fl*aa)-BookAdv1 else 0 end as Sqm5, \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "case when prbdesc = 'Section 55' then \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(((FL*aa) *CMGT/100*Dens)/1000 * 1)-BookKgs1 else 0 end as kgs5, \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "*, (fl*aa)-BookAdv1 sqm, (((FL*aa) *CMGT/100*Dens)/1000)-BookKgs1 kgs from ( \r\n ";

            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "select *, case when BookAdv is null then 0 else BookAdv end as BookAdv1, case when BookKgs is null then 0 else BookKgs end as BookKgs1  from (  \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "select s.MOID, MOName , w.Description wpdesc, w.workplaceid wp1, w.EndTypeID, \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "pb.ProblemID, p.ProblemDesc prbdesc, '1' FL, pm.SW, pm.CMGT, pm.Dens, pp.BookAdv, pp.BookGrams/1000 BookKgs  \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "from tbl_ProblemBook pb, tbl_SectionComplete s, tbl_Workplace w, tbl_Code_Problem_Main p, tbl_PlanMonth pm, tbl_Planning pp \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "where pb.Prodmonth = s.prodmonth and pb.SectionID = s.SectionID and pb.WorkplaceID = w.WorkplaceID and \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "pb.ProblemID = p.ProblemID and pb.Prodmonth = pm.Prodmonth and pb.WorkplaceID = pm.Workplaceid and pb.SectionID = pm.Sectionid \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "and pb.prodmonth = pp.Prodmonth and pb.CalendarDate = pp.CalendarDate and pb.sectionid = pp.SectionID and pb.WorkplaceID = pp.WorkplaceID and pb.Activity = pp.Activity \r\n";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "and pb.Activity = pm.Activity and \r\n ";

            if (ProblemsRgb.EditValue.ToString() == "Plan To Be Blasted Lost Blast")
                _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "pb.problemid in ('E6','E9','E11','E12','E13','E14') and pb.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and pb.Activity = 1 and booktons = 0 and pp.MOCycle in ('BL','SUBL')) q \r\n ";
            else
                _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "pb.problemid in ('E6','E9','E11','E12','E13','E14') and pb.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and pb.Activity = 1) q \r\n ";

            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "left outer join \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(select sectionid sec1111, stopingcycle, DevCycle from tbl_Cycle_MOCycleConfig ) bb  on q.moid = bb.sec1111 left outer join tbl_WorkPlace_PlanningDefaults def on q.wp1 = def.workplaceid  \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "left outer join \r\n ";

            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "(select Name Name1, fl Endtype1, avg(advBlast) aa from tbl_Cycle_RawData \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "group by Name, fl) zzz on bb.devcycle = zzz.Name1 and q.endtypeid = zzz.endtype1 )a )b \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "group by moid,moname  \r\n ";
            _dbManWPStop11.SqlStatement = _dbManWPStop11.SqlStatement + "order by moid   ";

            _dbManWPStop11.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPStop11.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPStop11.ResultsTableName = "DeatalDev";
            _dbManWPStop11.ExecuteInstruction();
            DataSet WPStop11 = new DataSet();

            WPStop11.Tables.Add(_dbManWPStop11.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManWPStop22 = new MWDataManager.clsDataAccess();
            _dbManWPStop22.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPStop22.SqlStatement = "select '" + barProdmonth.Edit.ToString() + "' MyProdmonth, '" + ProductionGlobalTSysSettings._Banner + "' banner, ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "*, fl*aa sqm, ((FL*aa) *CMGT/100*Dens)/1000 kgs from (select s.MOID, MOName , w.Description wpdesc, ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "pb.ProblemID, p.ProblemDesc prbdesc, '1' FL, pm.SW, pm.CMGT, pm.Dens ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "from tbl_ProblemBook pb, tbl_SectionComplete s, tbl_Workplace w, tbl_Code_Problem_Main p, tbl_PlanMonth pm, tbl_Planning pp ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "where pb.Prodmonth = s.prodmonth and pb.SectionID = s.SectionID and pb.WorkplaceID = w.WorkplaceID and ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "pb.ProblemID = p.ProblemID and pb.Prodmonth = pm.Prodmonth and pb.WorkplaceID = pm.Workplaceid and pb.SectionID = pm.Sectionid ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "and pb.Activity = pm.Activity and ";

            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + " pb.prodmonth = pp.Prodmonth and pb.CalendarDate = pp.CalendarDate and pb.sectionid = pp.SectionID and pb.WorkplaceID = pp.WorkplaceID and pb.Activity = pp.Activity and \r\n";

            if (ProblemsRgb.EditValue.ToString() == "Plan To Be Blasted Lost Blast")
                _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "pb.problemid in ('E6','E9','E11','E12','E13','E14') and pb.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and pb.Activity = 1 and booktons = 0 and pp.MOCycle in ('BL','SUBL')) a ";
            else
                _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "pb.problemid in ('E6','E9','E11','E12','E13','E14') and pb.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and pb.Activity = 1) a ";

            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "left outer join ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "(select * from  ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "(select Sectionid, DevCycle StopingCycle from tbl_Cycle_MOCycleConfig ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "where DevCycle <> '') a left outer join ";

            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "(select Name, max(advblast) aa from tbl_Cycle_RawData where [Type] = 'D' group by Name) b ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "on a.StopingCycle = b.Name) b ";
            _dbManWPStop22.SqlStatement = _dbManWPStop22.SqlStatement + "on a.moid = b.sectionid ";
            _dbManWPStop22.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPStop22.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPStop22.ResultsTableName = "Deatal2Dev";
            _dbManWPStop22.ExecuteInstruction();
            DataSet WPStop22 = new DataSet();

            WPStop22.Tables.Add(_dbManWPStop22.ResultsDataTable);

            theReportWPStop.RegisterData(WPStop11);
            theReportWPStop.RegisterData(WPStop22);

            theReportWPStop.Load(_reportFolder + "ProblemHistoryWPStoppage.frx");
            // theReportWPStop.Design();

            previewControl1.Clear();
            theReportWPStop.Prepare();
            theReportWPStop.Preview = previewControl1;
            theReportWPStop.ShowPrepared();
        }

        void loadDataVamps()
        {
            String Lbl2 = string.Empty;

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
                Lbl2 = "Prodmonth- " + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
            else
                Lbl2 = "From- " + String.Format("{0:dd-MMM-yyyy}", FromDate.EditValue) + "   To- " + String.Format("{0:dd-MMM-yyyy}", ToDate.EditValue);

            //int NoLostBlasts = 0;

            // do no blasts
            MWDataManager.clsDataAccess _dbMan7 = new MWDataManager.clsDataAccess();
            _dbMan7.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan7.SqlStatement = "select * \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "from (select  case when bookprob <> '' then 1 else 0 end as pr, ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "case when booktons <> 0 then 1 else 0 end as bb, case when PlanSqm <> 0 and  bookprob <> '' then 1 else 0 end as lb  from tbl_PLANNING_Vamping p,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4 \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "where p.prodmonth <> '200000' \r\n ";

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' \r\n ";
            }
            if (radioGroup1.EditValue.ToString() != "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' \r\n ";
            }

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s1.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s2.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s3.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s4.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.SectionID = s.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

            if (Hier == "1")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "2")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "3")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "4")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "5")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "6")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
            }
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ") a  ";

            _dbMan7.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan7.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan7.ResultsTableName = "Data";
            //textBox1.Text = _dbMan7.SqlStatement;
            _dbMan7.ExecuteInstruction();
            DataTable Neil = _dbMan7.ResultsDataTable;

            NoBlastsLbl.Text = "0";
            NoDualBlastsLbl.Text = "0";
            NoProblemsLbl.Text = "0";
            LostBlastLbl.Text = "0";

            for (int i = 0; i < Neil.Rows.Count; i++)
            {
                NoBlastsLbl.Text = (Convert.ToInt32(NoBlastsLbl.Text) + Convert.ToInt32(Neil.Rows[i]["bb"].ToString())).ToString();
                NoProblemsLbl.Text = (Convert.ToInt32(NoProblemsLbl.Text) + Convert.ToInt32(Neil.Rows[i]["pr"].ToString())).ToString();
                LostBlastLbl.Text = (Convert.ToInt32(LostBlastLbl.Text) + Convert.ToInt32(Neil.Rows[i]["lb"].ToString())).ToString();
            }

            _dbMan7.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan7.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lb2, workplaceid, prodmonth, sectionid, convert(decimal(18,1),9.1) Activity, calendardate \r\n ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " ,problemid, 'D' Shift, 'N' ShowIndicator, '' SBossNotes,  convert(decimal(18,0),BookSQM) BookSQM, ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " convert(decimal(18,0),BookTons) BookTons, convert(decimal(18,3),isnull(0,BookKGs)) BookKGs, convert(decimal(18,3),sqm) sqm, convert(decimal(18,3),0) tons, convert(decimal(18,3),0) oz  ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " , WPDescription, Mprass, wpexternalid, calendardate TimeBreakDown, GroupDesc, case when lb = 1 then 'Y' else 'N' end as CausedLostBlast ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " , ProbDescription ";


            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "from (select  case when bookprob <> '' then 1 else 0 end as pr, ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "case when booktons <> 0 then 1 else 0 end as bb, case when PlanSqm <> 0 and  bookprob <> '' then 1 else 0 end as lb, bookprob  ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ", p.WorkplaceID, p.Prodmonth, p.SectionID, p.CalendarDate, pr.ProblemID, ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "  BookSQM,  ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " BookTons, BookContent/1000 BookKGs, plansqm sqm ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ", w.description WPDescription,  wt.WpSublocation Mprass, wt.wpexternalid, problemgroupcode GroupDesc, pr.description ProbDescription ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "from tbl_PLANNING_Vamping p, tbl_WORKPLACE_Total wt, tbl_Workplace w ,tbl_Code_Problem_Main pr,  tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Section s3, tbl_Section s4 \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " ,(SELECT [Workplaceid] dd ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ",[VampTons]/([VampSqm]+0.0001) tsqm ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ", [VampContent]/([VampSqm]+0.0001) csqm  ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "FROM tbl_Vamping_PreInspectionSheet ) zz ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "where w.description = zz.dd and w.gmsiwpid = wt.gmsiwpid and p.workplaceid = w.workplaceid and p.prodmonth <> '200000' and  p.bookProb = pr.problemid+':'+pr.description \r\n ";

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' \r\n ";
            }
            if (radioGroup1.EditValue.ToString() != "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' \r\n ";
            }

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s1.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s2.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s3.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s4.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.SectionID = s.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

            if (Hier == "1")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "2")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "3")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "4")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "5")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "6")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
            }
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ") a where bookprob <> '' and Sqm <> 0 ";
            _dbMan7.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan7.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan7.ResultsTableName = "Data";
            //textBox1.Text = _dbMan7.SqlStatement;
            _dbMan7.ExecuteInstruction();
            Neil = _dbMan7.ResultsDataTable;

            DataSet dsData = new DataSet();
            dsData.Tables.Add(_dbMan7.ResultsDataTable);

            theReportprob.RegisterData(dsData);

            theReportprob.Load(_reportFolder + "ProblemHistoryData.frx");

            pcReportprob.Clear();
            //  theReportprob.Design();
            theReportprob.Prepare();
            theReportprob.Preview = pcReportprob;
            theReportprob.ShowPrepared();

            MWDataManager.clsDataAccess _dbMan71 = new MWDataManager.clsDataAccess();

            _dbMan71.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan71.SqlStatement = "select Top 10 description,TheCount TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from ( ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " select ProbDescription description, COUNT(workplaceid) TheCount, 'Mponeng' banner, 'Prodmonth- Nov-2014' Lb2 ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ", 'Bar' Graph, '' label, '' TheSection, '' TheHqCat, '' theProbGroup ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " from ( ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "select '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lb2, workplaceid, prodmonth, sectionid, convert(decimal(18,1),9.1) Activity, calendardate \r\n ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " ,problemid, 'D' Shift, 'N' ShowIndicator, '' SBossNotes,  convert(decimal(18,0),BookSQM) BookSQM, ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " convert(decimal(18,0),BookTons) BookTons, convert(decimal(18,3),isnull(0,BookKGs)) BookKGs, convert(decimal(18,3),sqm) sqm, convert(decimal(18,3),0) tons, convert(decimal(18,3),0) oz  ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " , WPDescription, Mprass, wpexternalid, calendardate TimeBreakDown, GroupDesc, case when lb = 1 then 'Y' else 'N' end as CausedLostBlast ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " , ProbDescription ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "from (select  case when bookprob <> '' then 1 else 0 end as pr, ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "case when booktons <> 0 then 1 else 0 end as bb, case when PlanSqm <> 0 and  bookprob <> '' then 1 else 0 end as lb, bookprob  ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ", p.WorkplaceID, p.Prodmonth, p.SectionID, p.CalendarDate, pr.ProblemID, ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "  BookSQM,  ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " BookTons, BookContent/1000 BookKGs, plansqm sqm ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ", w.description WPDescription,  wt.WpSublocation Mprass, wt.wpexternalid, problemgroupcode GroupDesc, pr.description ProbDescription ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "from tbl_PLANNING_Vamping p, tbl_WORKPLACE_Total wt, tbl_Workplace w ,tbl_Code_Problem_Main pr,  tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Section s3, tbl_Section s4 \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " ,(SELECT [Workplaceid] dd ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ",[VampTons]/([VampSqm]+0.0001) tsqm ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ", [VampContent]/([VampSqm]+0.0001) csqm  ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "FROM tbl_Vamping_PreInspectionSheet ) zz ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "where w.description = zz.dd and w.gmsiwpid = wt.gmsiwpid and p.workplaceid = w.workplaceid and p.prodmonth <> '200000' and  p.bookProb = pr.problemid+':'+pr.description \r\n ";

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' \r\n ";
            }
            if (radioGroup1.EditValue.ToString() != "Prodmonth")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' \r\n ";
            }

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s1.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s2.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s3.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s4.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.SectionID = s.SectionID \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

            if (Hier == "1")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "2")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "3")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "4")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "5")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "6")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
            }

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ") a where bookprob <> '' and Sqm <> 0 ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ") a  group by ProbDescription) a order by Description ";
            _dbMan71.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan71.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan71.ResultsTableName = "Graph";
            //textBox1.Text = _dbMan71.SqlStatement;
            _dbMan71.ExecuteInstruction();

            DataSet dsGraph = new DataSet();

            dsGraph.Tables.Add(_dbMan71.ResultsDataTable);
            theReport2prob.RegisterData(dsGraph);

            theReport2prob.Load(_reportFolder + "ProblemHistoryGraph.frx");

            //theReport2prob.Design();

            pcReport2prob.Clear();
            theReport2prob.Prepare();
            theReport2prob.Preview = pcReport2prob;
            theReport2prob.ShowPrepared();
        }

        void LoadNSData()
        {
            String Lbl2 = string.Empty;

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
                Lbl2 = "Prodmonth- " + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
            else
                Lbl2 = "From- " + String.Format("{0:dd-MMM-yyyy}", FromDate.EditValue) + "   To- " + String.Format("{0:dd-MMM-yyyy}", ToDate.EditValue);

            //int NoLostBlasts = 0;

            // do no blasts
            MWDataManager.clsDataAccess _dbMan7 = new MWDataManager.clsDataAccess();
            _dbMan7.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan7.SqlStatement = "select * \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "from (select  case when bookprob <> '' then 1 else 0 end as pr, ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "case when booktons <> 0 then 1 else 0 end as bb, case when PlanSqm <> 0 and  bookprob <> '' then 1 else 0 end as lb  from tbl_PLANNING_Vamping p,tbl_SECTION s,tbl_SECTION s1,tbl_SECTION s2,tbl_SECTION s3,tbl_SECTION s4 \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "where p.prodmonth <> '200000' \r\n ";

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' \r\n ";
            }
            if (radioGroup1.EditValue.ToString() != "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' \r\n ";
            }

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s1.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s2.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s3.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s4.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.SectionID = s.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

            if (Hier == "1")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "2")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "3")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "4")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "5")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "6")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
            }
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ") a  ";

            _dbMan7.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan7.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan7.ResultsTableName = "Data";
            //textBox1.Text = _dbMan7.SqlStatement;
            _dbMan7.ExecuteInstruction();
            DataTable Neil = _dbMan7.ResultsDataTable;

            NoBlastsLbl.Text = "0";
            NoDualBlastsLbl.Text = "0";
            NoProblemsLbl.Text = "0";
            LostBlastLbl.Text = "0";

            for (int i = 0; i < Neil.Rows.Count; i++)
            {
                NoBlastsLbl.Text = (Convert.ToInt32(NoBlastsLbl.Text) + Convert.ToInt32(Neil.Rows[i]["bb"].ToString())).ToString();
                NoProblemsLbl.Text = (Convert.ToInt32(NoProblemsLbl.Text) + Convert.ToInt32(Neil.Rows[i]["pr"].ToString())).ToString();
                LostBlastLbl.Text = (Convert.ToInt32(LostBlastLbl.Text) + Convert.ToInt32(Neil.Rows[i]["lb"].ToString())).ToString();
            }

            _dbMan7.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan7.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lb2, workplaceid, prodmonth, sectionid, convert(decimal(18,1),9.1) Activity, calendardate \r\n ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " ,problemid, 'D' Shift, 'N' ShowIndicator, '' SBossNotes,  convert(decimal(18,0),BookSQM) BookSQM, ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " convert(decimal(18,0),BookTons) BookTons, convert(decimal(18,3),isnull(0,BookKGs)) BookKGs, convert(decimal(18,3),sqm) sqm, convert(decimal(18,3),0) tons, convert(decimal(18,3),0) oz  ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " , WPDescription, Mprass, wpexternalid, calendardate TimeBreakDown, GroupDesc, case when lb = 1 then 'Y' else 'N' end as CausedLostBlast ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " , ProbDescription ";


            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "from (select  case when bookprob <> '' then 1 else 0 end as pr, ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "case when booktons <> 0 then 1 else 0 end as bb, case when Sqm <> 0 and  bookprob <> '' then 1 else 0 end as lb, bookprob  ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ", p.WorkplaceID, p.Prodmonth, p.SectionID, p.CalendarDate, pr.ProblemID, ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "  BookSQM,  ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " BookTons, 0 BookKGs, sqm sqm ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ", w.description WPDescription,  wt.WpSublocation Mprass, wt.wpexternalid, problemgroupcode GroupDesc, pr.description ProbDescription ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "from tbl_PLANNING p, tbl_WORKPLACE_Total wt, tbl_Workplace w ,tbl_Code_Problem_Main pr,  tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Section s3, tbl_Section s4 \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + " ,(SELECT [Workplaceid] dd ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ",0 tsqm ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ", 0 csqm , Calendardate ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "FROM [dbo].[tbl_Booking_NightShift] ) zz ";

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "where w.workplaceid = zz.dd and w.gmsiwpid = wt.gmsiwpid and p.workplaceid = w.workplaceid and p.prodmonth <> '200000' and  p.bookProb = pr.problemid and zz.Calendardate = p.Calendardate \r\n ";

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' \r\n ";
            }
            if (radioGroup1.EditValue.ToString() != "Prodmonth")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' \r\n ";
            }

            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s1.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s2.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s3.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.Prodmonth = s4.Prodmonth \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and p.SectionID = s.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

            if (Hier == "1")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "2")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "3")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "4")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "5")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "6")
            {
                _dbMan7.SqlStatement = _dbMan7.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
            }
            _dbMan7.SqlStatement = _dbMan7.SqlStatement + ") a where bookprob <> '' and Sqm <> 0 ";

            _dbMan7.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan7.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan7.ResultsTableName = "Data";
            //textBox1.Text = _dbMan7.SqlStatement;
            _dbMan7.ExecuteInstruction();
            Neil = _dbMan7.ResultsDataTable;

            DataSet dsData = new DataSet();
            dsData.Tables.Add(_dbMan7.ResultsDataTable);

            theReportprob.RegisterData(dsData);

            theReportprob.Load(_reportFolder + "ProblemHistoryData.frx");

            pcReportprob.Clear();
            //  theReportprob.Design();
            theReportprob.Prepare();
            theReportprob.Preview = pcReportprob;
            theReportprob.ShowPrepared();

            MWDataManager.clsDataAccess _dbMan71 = new MWDataManager.clsDataAccess();
            _dbMan71.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan71.SqlStatement = "select Top 10 description,TheCount TheCount, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lbl2, '" + GraphType + "' Graph, '" + lblGraph.Text + "' label, '" + UnderSectionCmb.EditValue.ToString() + "' TheSection, '" + HQCatCmb.EditValue.ToString() + "' theHqCat, '" + ProbGroupCmb.EditValue.ToString() + "' theProbGroup from ( ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " select ProbDescription description, COUNT(workplaceid) TheCount, 'Mponeng' banner, 'Prodmonth- Nov-2014' Lb2 ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ", 'Bar' Graph, '' label, '' TheSection, '' TheHqCat, '' theProbGroup ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " from ( ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "select '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + Lbl2 + "' Lb2, workplaceid, prodmonth, sectionid, convert(decimal(18,1),9.1) Activity, calendardate \r\n ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " ,problemid, 'D' Shift, 'N' ShowIndicator, '' SBossNotes,  convert(decimal(18,0),BookSQM) BookSQM, ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " convert(decimal(18,0),BookTons) BookTons, convert(decimal(18,3),isnull(0,BookKGs)) BookKGs, convert(decimal(18,3),sqm) sqm, convert(decimal(18,3),0) tons, convert(decimal(18,3),0) oz  ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " , WPDescription, Mprass, wpexternalid, calendardate TimeBreakDown, GroupDesc, case when lb = 1 then 'Y' else 'N' end as CausedLostBlast ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " , ProbDescription ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "from (select  case when bookprob <> '' then 1 else 0 end as pr, ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "case when booktons <> 0 then 1 else 0 end as bb, case when Sqm <> 0 and  bookprob <> '' then 1 else 0 end as lb, bookprob  ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ", p.WorkplaceID, p.Prodmonth, p.SectionID, p.CalendarDate, pr.ProblemID, ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "  BookSQM,  ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " BookTons, 0 BookKGs, sqm sqm ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ", w.description WPDescription,  wt.WpSublocation Mprass, wt.wpexternalid, problemgroupcode GroupDesc, pr.description ProbDescription ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "from tbl_PLANNING p, tbl_WORKPLACE_Total wt, tbl_Workplace w ,tbl_Code_Problem_Main pr,  tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Section s3, tbl_Section s4 \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + " ,(SELECT [Workplaceid] dd ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ",0 tsqm ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ", 0 csqm, Calendardate  ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "FROM tbl_Booking_NightShift ) zz ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "where w.workplaceid = zz.dd and w.gmsiwpid = wt.gmsiwpid and p.workplaceid = w.workplaceid and p.prodmonth <> '200000' and  p.bookProb = pr.problemid and zz.Calendardate = p.Calendardate \r\n ";

            if (radioGroup1.EditValue.ToString() == "Prodmonth")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' \r\n ";
            }
            if (radioGroup1.EditValue.ToString() != "Prodmonth")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", FromDate.EditValue) + "' and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", ToDate.EditValue) + "' \r\n ";
            }

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s1.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s2.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s3.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.Prodmonth = s4.Prodmonth \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and p.SectionID = s.SectionID \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and s.ReportToSectionid = s1.SectionID \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and s1.ReportToSectionid = s2.SectionID \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and s2.ReportToSectionid = s3.SectionID \r\n ";
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + "and s3.ReportToSectionid = s4.SectionID \r\n ";

            if (Hier == "1")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s4.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "2")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s3.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "3")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s2.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "4")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s1.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "5")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s.ReportToSectionid = '" + sec + "' \r\n ";
            }
            if (Hier == "6")
            {
                _dbMan71.SqlStatement = _dbMan71.SqlStatement + " and s.SectionID = '" + sec + "' \r\n ";
            }
            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ") a where bookprob <> '' and Sqm <> 0 ";

            _dbMan71.SqlStatement = _dbMan71.SqlStatement + ") a  group by ProbDescription) a order by Description ";
            _dbMan71.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan71.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan71.ResultsTableName = "Graph";
            //textBox1.Text = _dbMan71.SqlStatement;
            _dbMan71.ExecuteInstruction();

            DataSet dsGraph = new DataSet();

            dsGraph.Tables.Add(_dbMan71.ResultsDataTable);
            theReport2prob.RegisterData(dsGraph);

            theReport2prob.Load(_reportFolder + "ProblemHistoryGraph.frx");

            //theReport2prob.Design();

            pcReport2prob.Clear();
            theReport2prob.Prepare();
            theReport2prob.Preview = pcReport2prob;
            theReport2prob.ShowPrepared();
        }

        private void FromDate_EditValueChanged(object sender, EventArgs e)
        {
            if (FirtsLoad == "N")
                MainLoad();
        }

        private void ToDate_EditValueChanged(object sender, EventArgs e)
        {
            if (FirtsLoad == "N")
                MainLoad();
        }

        private void ProblemsRgb_EditValueChanged(object sender, EventArgs e)
        {
            if (FirtsLoad == "N")
                MainLoad();
        }

        private void GraphViewTypergb_EditValueChanged(object sender, EventArgs e)
        {
            if (FirtsLoad == "N")
                MainLoad();
        }

        private void HQCatCmb_EditValueChanged(object sender, EventArgs e)
        {
            if (FirtsLoad == "N")
                MainLoad();
        }

        private void ProbGroupCmb_EditValueChanged(object sender, EventArgs e)
        {
            if (FirtsLoad == "N")
                MainLoad();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FirtsLoad == "N")
                MainLoad();
        }
    }
}
