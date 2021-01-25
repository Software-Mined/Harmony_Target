using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting.BlastingCompliance
{
    public partial class ucBlastingCompliance : BaseUserControl
    {

        private string ReportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

        public ucBlastingCompliance()
        {
            InitializeComponent();
            FormRibbonPages.Add(ribbonPage4);
            FormActiveRibbonPage = ribbonPage4;
            FormMainRibbonPage = ribbonPage4;
            RibbonControl = ribbonControl1;
            //ribbonPage3.Ribbon.Minimized = true;
        }

        #region Private variables
        string month = string.Empty;
        string wk = string.Empty;
        Report theReportComplianceMO = new Report();
        Report theReport = new Report();
        Procedures procs = new Procedures();
        #endregion

        private void ucBlastingCompliance_Load(object sender, EventArgs e)
        {
            cbxBookingProdMonth2.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobalTSysSettings._currentProductionMonth.ToString());
            //rpBlastComp.Ribbon.Minimized = true;
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select distinct(prodmonth) pm from [dbo].[tbl_planning] where calendardate < getdate() order by prodmonth desc ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable Data = _dbMan.ResultsDataTable;

            foreach (DataRow dr in Data.Rows)
            {
                lbxMonth.Items.Add(dr["pm"].ToString());
            }

            lbxMonth.SelectedIndex = 0;

            _dbMan.SqlStatement = "select convert(varchar(10),ProdYear) + ' Wk' + Weeknum NewWeek from ( " +
                                  "select CASE " +
                                  "WHEN Datepart(isowk, calendardate)=1 " +
                                  "AND Month(calendardate)=12 THEN Year(calendardate)+1 " +
                                  "WHEN Datepart(isowk, calendardate)=53 " +
                                  "AND Month(calendardate)=1 THEN Year(calendardate)-1 " +
                                  "WHEN Datepart(isowk, calendardate)=52 " +
                                  "AND Month(calendardate)=1 THEN Year(calendardate)-1 " +
                                  "ELSE Year(calendardate) end as ProdYear , substring(convert(varchar(10),DATEPART(ISOWK,calendardate)+100),2,2) Weeknum " +

                                  "from [dbo].[tbl_planning] where calendardate < getdate()-1 ) Weeks group by ProdYear, Weeknum order by ProdYear desc, Weeknum desc ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();


            DataTable Data1 = _dbMan.ResultsDataTable;


            foreach (DataRow dr in Data1.Rows)
            {
                lbxWeek.Items.Add(dr["NewWeek"].ToString());
            }
            lbxWeek.SelectedIndex = 0;

            LoadProdmonth();
            loadWeek();
            LoadSections();
            //Load
        }

        void LoadSections()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " select Distinct MOID, MOName \r\n " +
                                    "from tbl_planmonth pm,  tbl_SectionComplete sc \r\n " +
                                    "where pm.prodmonth = '" + procs.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth2.EditValue.ToString())) + "' \r\n " +
                                    "and sc.SectionID = pm.SectionID and sc.Prodmonth = pm.Prodmonth order by MOID ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {
                repositoryItemLookUpEdit1.DataSource = _dbMan.ResultsDataTable;
                repositoryItemLookUpEdit1.ValueMember = "MOID";
                repositoryItemLookUpEdit1.DisplayMember = "MOName";
                tbSection2.EditValue = _dbMan.ResultsDataTable.Rows[0]["MOID"].ToString();
            }
            
        }

        private void LoadProdmonth()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select section, prodmonth, sum(planblast) planBlasts, sum(bookblast) bookblasts \r\n " +
                                  ", isnull(sum(plansqm),0) plansqm, isnull(sum(booksqm),0) booksqm, isnull(sum(SurvSQM),0) SurSQM \r\n " +
                                  ", case when sum(bookblast) > 0 then convert(decimal(18,0),isnull(convert(decimal(18,3),sum(bookblast))/ sum(planblast) *100, 0)) else 0 end as BlastComp \r\n " +
                                  ", case when sum(plansqm) > 0 then convert(decimal(18,0),isnull(convert(decimal(18,3),sum(booksqm))/ sum(plansqm) *100, 0)) else 0 end as BookComp \r\n " +
                                  ", case when sum(plansqm) > 0 then convert(decimal(18,0),isnull(convert(decimal(18,3),sum(SurvSQM))/ sum(plansqm) *100, 0)) else 0 end as OutputComp \r\n " +
                                  " from ( \r\n " +
                                  "select section, prodmonth, planblast, bookblast , plansqm, booksqm, SurvSqm \r\n " +
                                  "from [dbo].[tbl_CompRepMonthData]  \r\n " +
                                  "where prodmonth = '" + month + "' \r\n " +
                                  "  ) tblone \r\n " +
                                  "group by section, prodmonth \r\n ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();


            decimal planblast = 0;
            decimal bookblast = 0;

            decimal plansqm = 0;
            decimal booksqm = 0;


            decimal Sursqm = 0;

            DataTable dt = _dbMan.ResultsDataTable;
            foreach (DataRow r in dt.Rows)
            {
                planblast = planblast + Convert.ToInt32(r["planBlasts"].ToString());
                bookblast = bookblast + Convert.ToInt32(r["bookblasts"].ToString());
                plansqm = plansqm + Convert.ToInt32(r["plansqm"].ToString());
                booksqm = booksqm + Convert.ToInt32(r["booksqm"].ToString());
                Sursqm = Sursqm + Convert.ToInt32(r["SurSQM"].ToString());
            }

            BlastComptxt.Text = "0";
            if (planblast > 0)
                BlastComptxt.Text = Math.Round(bookblast / planblast * Convert.ToDecimal(100), 0).ToString();

            BookComptxt.Text = "0";
            if (plansqm > 0)
                BookComptxt.Text = Math.Round(booksqm / plansqm * Convert.ToDecimal(100), 0).ToString();

            OutComptxt.Text = "0";
            if (plansqm > 0)
                OutComptxt.Text = Math.Round(Sursqm / plansqm * Convert.ToDecimal(100), 0).ToString();


            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);

            gcBlastMonth.DataSource = ds1.Tables[0];

            col1SecID.FieldName = "section";
            colBlastPlan.FieldName = "planBlasts";
            colBlastBook.FieldName = "bookblasts";
            ColPlanSqm.FieldName = "plansqm";
            colBookSqm.FieldName = "booksqm";
            ColReconSqm.FieldName = "SurSQM";
            colBlastComp.FieldName = "BlastComp";
            ColBookComp.FieldName = "BookComp";
            ColOutputComp.FieldName = "OutputComp";


            colBlastPlan.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            colBlastPlan.SummaryItem.Tag = "1";


            //BlastComptxt.Top = gcBlastMonth.Location.Y + gcBlastMonth.Size.Height - 25;
            //BookComptxt.Top = gcBlastMonth.Location.Y + gcBlastMonth.Size.Height - 25;
            //OutComptxt.Top = gcBlastMonth.Location.Y + gcBlastMonth.Size.Height - 25;


            colBlastPlan.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
        }



        private void loadWeek()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select section, prodmonth, sum(planblast) planBlasts, sum(bookblast) bookblasts \r\n " +
                                  ", isnull(sum(plansqm),0) plansqm, isnull(sum(booksqm),0) booksqm, isnull(sum(SurvSQM),0) SurSQM \r\n " +
                                  ", case when sum(bookblast) > 0 then convert(decimal(18,0),isnull(convert(decimal(18,3),sum(bookblast))/ sum(planblast) *100, 0)) else 0 end as BlastComp \r\n " +
                                  ", case when sum(plansqm) > 0 then convert(decimal(18,0),isnull(convert(decimal(18,3),sum(booksqm))/ sum(plansqm) *100, 0)) else 0 end as BookComp \r\n " +
                                  ", case when sum(plansqm) > 0 then convert(decimal(18,0),isnull(convert(decimal(18,3),sum(SurvSQM))/ sum(plansqm) *100, 0)) else 0 end as OutputComp \r\n " +
                                  " from ( \r\n " +
                                  "select section, [week] prodmonth, planblast, bookblast , plansqm, booksqm, SurvSqm \r\n " +
                                  "from [dbo].[tbl_CompRepWeekData]  \r\n " +
                                  "where [week] = '" + wk + "' \r\n " +
                                  "  ) tblone \r\n " +
                                  "group by section, prodmonth \r\n ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();


            decimal planblast = 0;
            decimal bookblast = 0;

            decimal plansqm = 0;
            decimal booksqm = 0;


            decimal Sursqm = 0;

            DataTable dt = _dbMan.ResultsDataTable;
            foreach (DataRow r in dt.Rows)
            {
                planblast = planblast + Convert.ToInt32(r["planBlasts"].ToString());
                bookblast = bookblast + Convert.ToInt32(r["bookblasts"].ToString());
                plansqm = plansqm + Convert.ToInt32(r["plansqm"].ToString());
                booksqm = booksqm + Convert.ToInt32(r["booksqm"].ToString());
                Sursqm = Sursqm + Convert.ToInt32(r["SurSQM"].ToString());
            }

            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);

            gcBlastWeek.DataSource = ds1.Tables[0];

            colWkSecID.FieldName = "section";
            colWkBlastPlan.FieldName = "planBlasts";
            colWkBlastBook.FieldName = "bookblasts";
            ColWkPlanSqm.FieldName = "plansqm";
            colWkBookSqm.FieldName = "booksqm";
            ColWkReconSqm.FieldName = "SurSQM";
            colWkBlastComp.FieldName = "BlastComp";
            ColWkBookComp.FieldName = "BookComp";
            ColWkOutputComp.FieldName = "OutputComp";
        }

        private void gvBlastMonth_DoubleClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string sssect = gvBlastMonth.GetRowCellValue(gvBlastMonth.FocusedRowHandle, gvBlastMonth.Columns["section"]).ToString();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select sbname, minername, orgunitds, description,  \r\n" +
                                  " isnull(planblast,0) planblast,  isnull(bookblast,0) bookblast,  isnull(plansqm,0) plansqm,  isnull(booksqm,0) booksqm , isnull(SurSQM,0) SurSQM , case when isnull(plansqm,0) > 0 then isnull(SurSQM,0)/ (isnull(plansqm, 0)) * 100 else 0 end as SurvPer \r\n" +
                                  "from [dbo].BlastCompMonthly  \r\n" +
                                  "where prodmonth = '" + month + "' and section  = '" + sssect + "' order by sectionid ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "WPDetail";
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count < 1)
            {
                Cursor = Cursors.Default;
                return;
            }

            DataSet ReportDatasetWPDetail = new DataSet();
            ReportDatasetWPDetail.Tables.Add(_dbMan.ResultsDataTable);


            MWDataManager.clsDataAccess _dbManHeader = new MWDataManager.clsDataAccess();
            _dbManHeader.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManHeader.SqlStatement = "select '" + UserCurrentInfo.Connection + "' mine, '" + sssect + "' Section, '" + lblDispMonth.Text + "' mm ";
            _dbManHeader.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManHeader.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManHeader.ResultsTableName = "Header";
            _dbManHeader.ExecuteInstruction();

            DataSet ReportDatasetHeader = new DataSet();
            ReportDatasetHeader.Tables.Add(_dbManHeader.ResultsDataTable);


            theReportComplianceMO.RegisterData(ReportDatasetWPDetail);
            theReportComplianceMO.RegisterData(ReportDatasetHeader);

            theReportComplianceMO.Load(ReportFolder + "ComplianceMO.frx");

            //theReportComplianceMO.Design();
            //theReportComplianceMO.Show();

            pcReport.Clear();
            theReportComplianceMO.Prepare();
            theReportComplianceMO.Preview = pcReport;
            theReportComplianceMO.ShowPrepared();

            pcReport.Visible = true;
            pcReport.BringToFront();

            Cursor = Cursors.Default;
        }

        private void gvBlastWeek_DoubleClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string sssect = gvBlastWeek.GetRowCellValue(gvBlastWeek.FocusedRowHandle, gvBlastWeek.Columns["section"]).ToString();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select sbname, minername, orgunitds, description,  \r\n" +
                                  " isnull(planblast, 0) planblast,  isnull(bookblast, 0) bookblast,  isnull(plansqm, 0) plansqm,  isnull(booksqm, 0) booksqm , isnull(SurSQM, 0) SurSQM, case when isnull(plansqm,0) > 0 then isnull(SurSQM,0)/ (isnull(plansqm, 0)) * 100 else 0 end as SurvPer  \r\n" +
                                  "from [dbo].[BlastCompWeekly]  \r\n" +
                                  "where prodmonth = '" + wk + "' and section  = '" + sssect + "' order by sectionid ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "WPDetail";
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count < 1)
            {
                Cursor = Cursors.Default;
                return;
            }

            DataSet ReportDatasetWPDetail = new DataSet();
            ReportDatasetWPDetail.Tables.Add(_dbMan.ResultsDataTable);


            MWDataManager.clsDataAccess _dbManHeader = new MWDataManager.clsDataAccess();
            _dbManHeader.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManHeader.SqlStatement = "select '" + UserCurrentInfo.Connection + "' mine, '" + sssect + "' Section, '" + lblDispWeek.Text + "' mm ";
            _dbManHeader.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManHeader.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManHeader.ResultsTableName = "Header";
            _dbManHeader.ExecuteInstruction();

            DataSet ReportDatasetHeader = new DataSet();
            ReportDatasetHeader.Tables.Add(_dbManHeader.ResultsDataTable);

            theReportComplianceMO.RegisterData(ReportDatasetWPDetail);
            theReportComplianceMO.RegisterData(ReportDatasetHeader);

            theReportComplianceMO.Load(ReportFolder + "ComplianceMO.frx");


            //theReportComplianceMO.Design();

            pcReport.Clear();
            theReportComplianceMO.Prepare();
            theReportComplianceMO.Preview = pcReport;
            theReportComplianceMO.ShowPrepared();

            pcReport.Visible = true;
            pcReport.BringToFront();

            Cursor = Cursors.Default;
        }

        private void lbxWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            wk = lbxWeek.Text.Substring(0, 4);
            wk = wk + lbxWeek.Text.Substring(7, 2);
            lblDispWeek.Text = "Week " + lbxWeek.Text;
            loadWeek();
        }

        private void lbxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            month = lbxMonth.Text;
            lblDispMonth.Text = "Month " + lbxMonth.Text;
            LoadProdmonth();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        void LoadCompliance()
        {

            MWDataManager.clsDataAccess _dbManSB11 = new MWDataManager.clsDataAccess();
            _dbManSB11.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSB11.SqlStatement = _dbManSB11.SqlStatement + " select top(4) prodmonth from tbl_PlanMonth where prodmonth <=  '" + procs.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth2.EditValue.ToString())) + "' group by Prodmonth order by Prodmonth desc ";
            _dbManSB11.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSB11.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSB11.ResultsTableName = "SBoss";
            _dbManSB11.ExecuteInstruction();

            DataTable ReportSB11 = _dbManSB11.ResultsDataTable;
            string pmm = string.Empty;
            try
            {
                pmm = ReportSB11.Rows[3]["prodmonth"].ToString();
            }
            catch { pmm = ReportSB11.Rows[0]["prodmonth"].ToString(); }

            MWDataManager.clsDataAccess _dbManSB = new MWDataManager.clsDataAccess();
            _dbManSB.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSB.SqlStatement = _dbManSB.SqlStatement + " exec sp_Compliance_Weekly '" + procs.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth2.EditValue.ToString())) + "','" + tbSection2.EditValue + "' ";
            _dbManSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSB.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSB.ResultsTableName = "SBoss";
            _dbManSB.ExecuteInstruction();

            DataSet ReportSB = new DataSet();
            ReportSB.Tables.Add(_dbManSB.ResultsDataTable);

            theReport.RegisterData(ReportSB);


            MWDataManager.clsDataAccess _dbManGraph = new MWDataManager.clsDataAccess();
            _dbManGraph.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " select 'Prog' aa, 'Week ' + convert(varchar(2),a.ww) weeka, *, null progplan, null progbook from ( ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " select top(4) * from ( ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " select ww, MAX(calendardate) dd from ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " ( ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " select calendardate, DATEPART( wk, calendardate-1)+1 ww from tbl_Planning where calendardate >= GETDATE()-35 ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " and CalendarDate <= GETDATE()-7) a group by ww) a order by dd desc) a ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " left outer join ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " (select ww1, SUM(planblast) planblast,  SUM(bookblast) bookblast, sum(sqm) sqm, sum(booksqm) booksqm from ( ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " select DATEPART( wk, calendardate-1) ww1, ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " case when tons > 0 then 1 else 0 end as planblast, ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " case when booktons > 0 then 1 else 0 end as bookblast, sqm, booksqm from tbl_Planning p, tbl_Section s, tbl_Section s1 ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " where p.SectionID = s.SectionID and p.Prodmonth = s.Prodmonth and ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " s.reporttoSectionID = s1.SectionID and s.Prodmonth = s1.Prodmonth and ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " calendardate >= CONVERT(VARCHAR(10),GETDATE()-35,111) and s1.ReportToSectionid = '" + tbSection2.EditValue + "' and p.Activity <> 21 and CalendarDate <= GETDATE()) a group by ww1) b ";
            _dbManGraph.SqlStatement = _dbManGraph.SqlStatement + " on a.ww = b.ww1 order by dd ";


            _dbManGraph.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGraph.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGraph.ResultsTableName = "Graph";
            _dbManGraph.ExecuteInstruction();

            DataSet ReportGraph = new DataSet();
            ReportGraph.Tables.Add(_dbManGraph.ResultsDataTable);

            theReport.RegisterData(ReportGraph);


            MWDataManager.clsDataAccess _dbManGraph1 = new MWDataManager.clsDataAccess();
            _dbManGraph1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " select '" + tbSection2.EditValue + "' sec, '" + ProductionGlobalTSysSettings._Banner.ToString() + "' bann, 'Prog ' weeka, 1 ww, '2050-01-01' dd, 1 ww1, null progplan, null planblast, SUM(planblast) progplan,  SUM(bookblast) progbook, sum(sqm) sqm1, sum(booksqm) booksqm1 from ( ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " select top(4) * from ( ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " select ww, MAX(calendardate) dd from ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " ( ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " select calendardate, DATEPART( wk, calendardate-1)+1 ww from tbl_Planning where calendardate >= GETDATE()-35 ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " and CalendarDate <= GETDATE()-7) a group by ww) a order by dd desc) a ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " left outer join ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " (select ww1, SUM(planblast) planblast,  SUM(bookblast) bookblast, sum(sqm) sqm, sum(booksqm) booksqm from ( ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " select DATEPART( wk, calendardate-1) ww1,  ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " case when tons > 0 then 1 else 0 end as planblast, ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " case when BookTons > 0 then 1 else 0 end as bookblast, sqm, booksqm from tbl_Planning p, tbl_Section s, tbl_Section s1 ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " where p.SectionID = s.SectionID and p.Prodmonth = s.Prodmonth and ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " s.reporttoSectionID = s1.SectionID and s.Prodmonth = s1.Prodmonth and  ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " calendardate >= CONVERT(VARCHAR(10),GETDATE()-35,111) and s1.ReportToSectionid = '" + tbSection2.EditValue + "' and p.Activity <> 21 and CalendarDate <= GETDATE()) a group by ww1) b ";
            _dbManGraph1.SqlStatement = _dbManGraph1.SqlStatement + " on a.ww = b.ww1 order by dd ";

            _dbManGraph1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGraph1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGraph1.ResultsTableName = "GraphProg";
            _dbManGraph1.ExecuteInstruction();

            DataSet ReportGraph1 = new DataSet();
            ReportGraph1.Tables.Add(_dbManGraph1.ResultsDataTable);

            theReport.RegisterData(ReportGraph1);


            MWDataManager.clsDataAccess _dbManGraph2 = new MWDataManager.clsDataAccess();
            _dbManGraph2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + " select 'Prog' aa, 'Week ' +  ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + " convert(varchar(2),ww) weeka, CONVERT(decimal(18,0),comp) comp1, * ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "from ( ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "select top(4) * from ( ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "select (Blast + sweep + sup + other) /4 comp, * from (select ww, blast,  ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when sweep > 100 then 100 else Sweep end as sweep, ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when sup > 100 then 100 else sup end as sup, ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when other > 100 then 100 else other end as other, PlanBlast , bookBlast ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "from (select ww, ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "SUM(PlanBlast) PlanBlast, SUM(bookBlast) bookBlast ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + ", SUM(PlanSweep) PlanSweep,Sum(bookSweep)bookSweep ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + ", SUM(PlanSup) PlanSup, sum(bookSup) bookSup ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + ", SUM(PlanOther) PlanOther, SUM(bookOther) bookOther, ";

            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "(SUM(bookBlast)/ (convert(decimal(18,5),SUM(PlanBlast))+0.0000001))*100 Blast ";

            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + ", (SUM(bookSweep+ActSWP1)/(convert(decimal(18,5),SUM(PlanSweep))+0.0000001))*100 Sweep ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + ", SUM(bookSup+ActSU1)/(convert(decimal(18,5),SUM(PlanSup))+0.0000001)*100 Sup ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + ", (SUM(bookOther+ActBRL1+ActW1+ActBF1)/(convert(decimal(18,5),SUM(PlanOther))+0.0000001))*100 Other ";



            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + " from (select  DATEPART( wk, calendardate-1) ww,  ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when tons > 0 then 1 else 0 end as PlanBlast, ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when mocycle = 'SWP' then 1 else 0 end as PlanSweep, ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when mocycle = 'SU' then 1 else 0 end as PlanSup, ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when mocycle IN ( 'SU','SWP','BL') then 0 else 1 end as PlanOther, ";


            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + " case when substring(CheckMeasProb,1,1) = 'Y' then 1 else 0 end as ActSWP1 ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + " ,case when substring(CheckMeasProb,2,1) = 'Y' then 1 else 0 end as ActBRL1 ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + " ,case when substring(CheckMeasProb,3,1) = 'Y'then 1 else 0 end as ActSU1 ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + " ,case when substring(CheckMeasProb,4,1) = 'Y' then 1 else 0 end as ActW1 ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + " ,case when substring(CheckMeasProb,5,1) = 'Y' then 1 else 0 end as ActBF1, ";


            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when booktons > 0 then 1 else 0 end as BookBlast,  ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when booktons = 0 And bookprob = 'SWP' then 1 else 0 end as BookSweep, ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when booktons = 0 And bookprob = 'SU' then 1 else 0 end as BookSup, ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "case when booktons = 0 And bookprob in ('SU','SWP') then 0 else 1 end as BookOther from tbl_Planning p, tbl_Section s, tbl_Section s1 ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "where ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "p.SectionID = s.SectionID and p.Prodmonth = s.Prodmonth and ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth and ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "Activity <> 21 and calendardate >= CONVERT(VARCHAR(10),GETDATE()-35,111)  and CalendarDate <= GETDATE() ";

            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "And s1.ReportToSectionid = '" + tbSection2.EditValue + "' ";
            _dbManGraph2.SqlStatement = _dbManGraph2.SqlStatement + "and CalendarDate <= GETDATE()) a group by ww) a) a ) a order by ww desc) a order by ww ";

            _dbManGraph2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGraph2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGraph2.ResultsTableName = "GraphComp";
            _dbManGraph2.ExecuteInstruction();

            DataSet ReportGraph2 = new DataSet();
            ReportGraph2.Tables.Add(_dbManGraph2.ResultsDataTable);

            theReport.RegisterData(ReportGraph2);




            MWDataManager.clsDataAccess _dbManGraph3 = new MWDataManager.clsDataAccess();
            _dbManGraph3.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + " select 'Prog' aa,   ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + " ' ' + convert(varchar(6),ww) weeka, CONVERT(decimal(18,0),comp) comp1, * ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "from ( ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "select  * from ( ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "select (Blast + sweep + sup + other) /4 comp, * from (select ww, blast,  ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when sweep > 100 then 100 else Sweep end as sweep, ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when sup > 100 then 100 else sup end as sup, ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when other > 100 then 100 else other end as other, PlanBlast , bookBlast ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "from (select ww, ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "SUM(PlanBlast) PlanBlast, SUM(bookBlast) bookBlast ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + ", SUM(PlanSweep) PlanSweep,Sum(bookSweep)bookSweep ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + ", SUM(PlanSup) PlanSup, sum(bookSup) bookSup ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + ", SUM(PlanOther) PlanOther, SUM(bookOther) bookOther, ";

            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "(SUM(bookBlast)/ (convert(decimal(18,5),SUM(PlanBlast))+0.0000001))*100 Blast ";

            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + ", (SUM(bookSweep+ActSWP1)/(convert(decimal(18,5),SUM(PlanSweep))+0.0000001))*100 Sweep ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + ", SUM(bookSup+ActSU1)/(convert(decimal(18,5),SUM(PlanSup))+0.0000001)*100 Sup ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + ", (SUM(bookOther+ActBRL1+ActW1+ActBF1)/(convert(decimal(18,5),SUM(PlanOther))+0.0000001))*100 Other ";



            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + " from (select  p.prodmonth ww,  ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when tons > 0 then 1 else 0 end as PlanBlast, ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when mocycle = 'SWP' then 1 else 0 end as PlanSweep, ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when mocycle = 'SU' then 1 else 0 end as PlanSup, ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when mocycle IN ( 'SU','SWP','BL') then 0 else 1 end as PlanOther, ";


            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + " case when substring(CheckMeasProb,1,1) = 'Y' then 1 else 0 end as ActSWP1 ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + " ,case when substring(CheckMeasProb,2,1) = 'Y' then 1 else 0 end as ActBRL1 ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + " ,case when substring(CheckMeasProb,3,1) = 'Y'then 1 else 0 end as ActSU1 ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + " ,case when substring(CheckMeasProb,4,1) = 'Y' then 1 else 0 end as ActW1 ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + " ,case when substring(CheckMeasProb,5,1) = 'Y' then 1 else 0 end as ActBF1, ";



            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when booktons > 0 then 1 else 0 end as BookBlast,  ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when booktons = 0 And bookprob = 'SWP' then 1 else 0 end as BookSweep, ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when booktons = 0 And bookprob = 'SU' then 1 else 0 end as BookSup, ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "case when booktons = 0 And bookprob in ('SU','SWP') then 0 else 1 end as BookOther from tbl_Planning p, tbl_Section s, tbl_Section s1 ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "where ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "p.SectionID = s.SectionID and p.Prodmonth = s.Prodmonth and ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth and ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "Activity <> 21 and calendardate >= GETDATE()-150  and CalendarDate <= GETDATE() ";

            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "And s1.ReportToSectionid = '" + tbSection2.EditValue + "' ";
            _dbManGraph3.SqlStatement = _dbManGraph3.SqlStatement + "and CalendarDate <= GETDATE() and p.prodmonth > = '" + pmm.ToString() + "' ) a group by ww) a) a ) a ) a order by ww ";

            _dbManGraph3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGraph3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGraph3.ResultsTableName = "GraphCompmbym";
            _dbManGraph3.ExecuteInstruction();

            DataSet ReportGraph3 = new DataSet();
            ReportGraph3.Tables.Add(_dbManGraph3.ResultsDataTable);

            theReport.RegisterData(ReportGraph3);


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " select p.workplaceid + s.sectionid +convert(varchar(1),p.Activity) chhange,s1.SectionID sb, s1.Name sbname, s.SectionID minsec, s.Name minname, SUBSTRING(orgunitds, 1 ,12) org, ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " p.workplaceid, description, CalendarDate, WorkingDay , MOCycle, ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " case when booksqm > 0 then convert(varchar(10),BookSqm) ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " else bookprob end bookcode ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , case when bookcode = 'PR' then 'R' ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " when bookcode = 'PS' then 'B' ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " else 'FA' end as colour1,p.Sqm Sqm,p.BookSqm BookSqm ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " from tbl_Planning p, tbl_Workplace w, tbl_Section s, tbl_Section s1 ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where p.WorkplaceID = w.WorkplaceID and  p.SectionID = s.SectionID and p.Prodmonth = s.Prodmonth and ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " s.reporttoSectionID = s1.SectionID and s.Prodmonth = s1.Prodmonth and ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " calendardate >= CONVERT(VARCHAR(10),GETDATE()-35,111) and s1.ReportToSectionid = '" + tbSection2.EditValue + "' and p.Activity <> 21 and CalendarDate <= GETDATE() ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " order by s1.SectionID, s.SectionID, SUBSTRING(orgunitds, 1 ,12) , Description, p.activity, CalendarDate ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable Date = new DataTable();
            DataTable Data = _dbMan.ResultsDataTable;


            TimeSpan Span;


            //MessageBox.Show(DateTime.Now.ToString());
            DateTime StartDate = DateTime.Now.AddDays(-35);
            // MessageBox.Show(StartDate.ToString());


            string Change = string.Empty;

            int Day = 0;

            Date.Rows.Add();
            for (int x = 0; x <= 34; x++)
            {
                if (StartDate.AddDays(Day) <= DateTime.Now)
                {

                    Date.Columns.Add();
                    Date.Rows[0][x] = StartDate.AddDays(Day).ToString("dd MMM ddd");
                    Day = Day + 1;


                }
                else
                {
                    Date.Columns.Add();
                    Date.Rows[0][x] = string.Empty;
                }

            }

            Date.Columns.Add();
            Date.Rows[0][Date.Columns.Count - 1] = Day.ToString();

            Date.TableName = "CalDates";



            DataSet ReportDate = new DataSet();
            ReportDate.Tables.Add(Date);

            theReport.RegisterData(ReportDate);

            DataTable Plan = new DataTable();
            DataTable Book = new DataTable();
            int col = 8;
            int col2 = 52;
            int ColourCol = 87;
            decimal ProgPlan = 0;
            decimal ProgBook = 0;

            for (int s = 0; s <= 150; s++)
            {
                Plan.Columns.Add();
                Book.Columns.Add();
            }

            int y = -1;
            //int z = -1;


            foreach (DataRow dr in Data.Rows)
            {
                col = 8;
                col2 = 52;

                Span = Convert.ToDateTime(dr["calendardate"].ToString()).Subtract(StartDate.Date);
                col = Convert.ToInt32(Span.Days) + col;
                col2 = Convert.ToInt32(Span.Days) + col2;

                if (Change != dr["chhange"].ToString())
                {
                    ColourCol = 87;
                    Plan.Rows.Add();

                    y = y + 1;
                    // z = z+ 2;

                    Change = dr["chhange"].ToString();
                    //Span = Convert.ToDateTime(dr["calendardate"].ToString()).Subtract(StartDate);
                    //col = Convert.ToInt32(Span.Days) + col;


                    Plan.Rows[y][0] = dr["Org"].ToString();
                    Plan.Rows[y][1] = dr["Workplaceid"].ToString() + ":" + dr["Description"].ToString();
                    Plan.Rows[y][2] = dr["sb"].ToString();
                    Plan.Rows[y][3] = dr["sbname"].ToString();
                    Plan.Rows[y][4] = dr["minsec"].ToString();
                    Plan.Rows[y][5] = dr["minname"].ToString();
                    Plan.Rows[y][6] = dr["Colour1"].ToString();

                    if (Plan.Rows[y][7].ToString() == string.Empty)
                        Plan.Rows[y][7] = 0;

                    if (dr["Sqm"] != DBNull.Value)
                    {
                        ProgPlan = ProgPlan + Convert.ToInt32(dr["Sqm"].ToString());

                        Plan.Rows[y][7] = ProgPlan.ToString();
                    }

                    Plan.Rows[y][col] = dr["MOCycle"].ToString();

                    ///////////////////////////////Bookings Data/////////////////////
                    //Plan.Rows.Add();

                    Plan.Rows[y][44] = dr["Org"].ToString();
                    Plan.Rows[y][45] = dr["Workplaceid"].ToString() + ":" + dr["Description"].ToString();
                    Plan.Rows[y][46] = dr["sb"].ToString();
                    Plan.Rows[y][47] = dr["sbname"].ToString();
                    Plan.Rows[y][48] = dr["minsec"].ToString();
                    Plan.Rows[y][49] = dr["minname"].ToString();
                    Plan.Rows[y][50] = string.Empty;//dr["Colour1"].ToString();

                    if (Plan.Rows[y][51].ToString() == string.Empty)
                        Plan.Rows[y][51] = 0;

                    if (dr["BookSqm"] != DBNull.Value)
                    {
                        ProgBook = ProgBook + Convert.ToInt32(dr["BookSqm"].ToString());

                        Plan.Rows[y][51] = ProgBook.ToString();
                    }

                    Plan.Rows[y][col2] = dr["bookcode"].ToString();
                    Plan.Rows[y][ColourCol] = dr["Colour1"].ToString();


                }
                else
                {
                    Plan.Rows[y][0] = dr["Org"].ToString();
                    Plan.Rows[y][1] = dr["Workplaceid"].ToString() + ":" + dr["Description"].ToString();
                    Plan.Rows[y][2] = dr["sb"].ToString();
                    Plan.Rows[y][3] = dr["sbname"].ToString();
                    Plan.Rows[y][4] = dr["minsec"].ToString();
                    Plan.Rows[y][5] = dr["minname"].ToString();
                    Plan.Rows[y][6] = dr["Colour1"].ToString();

                    if (Plan.Rows[y][7].ToString() == string.Empty)
                        Plan.Rows[y][7] = 0;

                    if (dr["Sqm"] != DBNull.Value)
                    {
                        ProgPlan = ProgPlan + Convert.ToInt32(dr["Sqm"].ToString());

                        Plan.Rows[y][7] = ProgPlan.ToString();
                    }

                    Plan.Rows[y][col] = dr["MOCycle"].ToString();

                    ///////////////////////////////Bookings Data/////////////////////




                    Plan.Rows[y][0] = dr["Org"].ToString();
                    Plan.Rows[y][1] = dr["Workplaceid"].ToString() + ":" + dr["Description"].ToString();
                    Plan.Rows[y][2] = dr["sb"].ToString();
                    Plan.Rows[y][3] = dr["sbname"].ToString();
                    Plan.Rows[y][4] = dr["minsec"].ToString();
                    Plan.Rows[y][5] = dr["minname"].ToString();
                    Plan.Rows[y][6] = dr["Colour1"].ToString();

                    if (Plan.Rows[y][7].ToString() == string.Empty)
                        Plan.Rows[y][7] = 0;

                    if (dr["BookSqm"] != DBNull.Value)
                    {
                        ProgBook = ProgBook + Convert.ToInt32(dr["BookSqm"].ToString());

                        Plan.Rows[y][7] = ProgBook.ToString();
                    }

                    Plan.Rows[y][col2] = dr["bookcode"].ToString();
                    Plan.Rows[y][ColourCol] = dr["Colour1"].ToString();
                }

                ColourCol = ColourCol + 1;

            }

            Plan.TableName = "Plan";

            DataSet ReportPlan = new DataSet();
            ReportPlan.Tables.Add(Plan);

            theReport.RegisterData(ReportPlan);

            Book.TableName = "Book";

            DataSet ReportBook = new DataSet();
            ReportBook.Tables.Add(Book);

            theReport.RegisterData(ReportBook);

            theReport.Load(ReportFolder + "Compliance.frx");

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReportGraph;
            theReport.ShowPrepared();
        }

        private void btnClose2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void rcBlastComp_SelectedPageChanged(object sender, EventArgs e)
        {

        }

        private void tbSection_EditValueChanged(object sender, EventArgs e)
        {
            // LoadCompliance();
        }

        private void cbxBookingProdMonth_EditValueChanged(object sender, EventArgs e)
        {
            //LoadSections();
        }

        private void cbxBookingProdMonth2_EditValueChanged(object sender, EventArgs e)
        {
            LoadSections();
        }

        private void tbSection2_EditValueChanged(object sender, EventArgs e)
        {
            LoadCompliance();
            //LoadCompliance()
        }

        private void barButtonClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void xtraTabControl_CloseButtonClick(object sender, EventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadCompliance();
        }
    }
}
