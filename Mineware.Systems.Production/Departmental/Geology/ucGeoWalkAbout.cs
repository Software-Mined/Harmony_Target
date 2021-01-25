using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace Mineware.Systems.Production.Departmental.Geology
{
    public partial class ucGeoWalkAbout : BaseUserControl
    {
        public ucGeoWalkAbout()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpGeo);
            FormActiveRibbonPage = rpGeo;
            FormMainRibbonPage = rpGeo;
            RibbonControl = rcGeology;
        }

        private void ucGeoWalkAbout_Load(object sender, EventArgs e)
        {
            if (editActivity.EditValue.ToString() == "0")
            {
                LoadGeoInspGrid();
            }
            else
            {
                LoadGeoInspGridDev();
            }
        }

        #region Private variables
        FastReport.Report theReportGeoSum = new FastReport.Report();
        string clickcol = string.Empty;
        private string ReportsFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        string clickcolnum = "N";
        string clickRMS = "0";
        string clickAct = "0";
        string WPLbl = string.Empty;
        string label36 = string.Empty;
        string WKLbl = string.Empty;
        private bool expanded = true;
        #endregion


        public void LoadGeoInspGrid()
        {
            MWDataManager.clsDataAccess _dbManWK = new MWDataManager.clsDataAccess();
            _dbManWK.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _dbManWK.SqlStatement = "select distinct(a) a from ( " +
                                           "select convert(varchar(10),captyear) +' Wk' + substring('00'+convert(varchar(10),captweek+1000),5,2) a " +
                                           "from  [tbl_DPT_GeoScience_Inspection]) a order by a desc ";
            _dbManWK.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWK.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWK.ExecuteInstruction();

            DataTable dtwk = _dbManWK.ResultsDataTable;

            listBoxWk.Items.Clear();
            listBoxWk.Items.Add(string.Empty);
            foreach (DataRow dr in dtwk.Rows)
            {
                listBoxWk.Items.Add(dr["a"].ToString());
            }

            listBoxWk.SelectedIndex = 0;

            GeoScienceInspPnl.BringToFront();
            GeoScienceInspPnl.Dock = DockStyle.Fill;
            GeoScienceInspPnl.Visible = true;

            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _dbManWPSTDetail.SqlStatement = "Select * from(select *, isnull(convert(decimal(18, 0), 0 + 0.4), 0) aa from(select * \r\n" +

                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 42))), 2, 2) hhwk6 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 35))), 2, 2) hhwk5 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 28))), 2, 2) hhwk4 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 21))), 2, 2) hhwk3 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 14))), 2, 2) hhwk2 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 7))), 2, 2) hhwk1 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate()))), 2, 2) hhwnow \r\n" +

                                              ",substring( convert(varchar(30), DATEADD( DAY,- DATEPART(dw, getdate()-2), GETDATE()-42 ),106 )  ,1,6) Startdatewk6 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 42)), 106), 1, 6) Endatewk6 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 35), 106), 1, 6) Startdatewk5 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 35)), 106), 1, 6) Endatewk5 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 28), 106), 1, 6) Startdatewk4 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 28)), 106), 1, 6) Endatewk4 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 21), 106), 1, 6) Startdatewk3 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 21)), 106), 1, 6) Endatewk3 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 14), 106), 1, 6) Startdatewk2 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 14)), 106), 1, 6) Endatewk2 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 7), 106), 1, 6) Startdatewk1 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 7)), 106), 1, 6) Endatewk1 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE()), 106), 1, 6) StartdateNow \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE())), 106), 1, 6) EndateNow \r\n" +

                                              " , case when act = '1' then 'Dev' Else 'Stp' end as activityfinal \r\n" +
                                              " from(select nn, a.description, act, rr, case when description like '%-S%' then 'a' else isnull(wp6, '') end as wp6, case when description like '%-S%' then 'a' else isnull(wp5, '') end as wp5, case when description like '%-S%' then 'a' else isnull(wp4, '') end as wp4, case when description like '%-S%' then 'a' else isnull(wp3, '') end as wp3 \r\n" +
                                              " , case when description like '%-S%' then 'a' else isnull(wp2, '') end as wp2, case when description like '%-S%' then 'a' else isnull(wp1, '') end as wp1, case when description like '%-S%' then 'a' else isnull(wpnow, '') end as wpnow, isnull(colnow, '') colnow, isnull(col6, '') col6, isnull(col5, '') col5, isnull(col4, '') col4, isnull(col3, '') col3, isnull(col2, '') col2, isnull(col1, '') col1   from(select substring(sectionid, 1, 4) mo, w.description, p.workplaceid w, max(p.activity) act, max(riskrating) rr from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid \r\n" +
                                              " and calendardate > getdate() - 60 and calendardate < getdate() + 7 \r\n" +
                                              " and p.activity <> 100 group by w.description, substring(sectionid, 1, 4), p.workplaceid \r\n" +
                                              " union \r\n" +
                                              " select[mo],[WPDescription],[WNo],[act],[rr] from vw_Department_Walkabout_Sum \r\n" +
                                              " ) a \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col6 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 42))), 1, 2)) zz6 on a.Description = zz6.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col5 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 35))), 1, 2)) zz5 on a.Description = zz5.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col4 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 28))), 1, 2)) zz4 on a.Description = zz4.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col3 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 21))), 1, 2)) zz3 on a.Description = zz3.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col2 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 14))), 1, 2)) zz2 on a.Description = zz2.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col1 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 7))), 1, 2)) zz1 on a.Description = zz1.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' colnow from [tbl_DPT_GeoScience_Inspection] where  \r\n" +
                                              "CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate()))), 1, 2)  and captyear = datepart(year, getdate())) zz on a.Description = zz.w COLLATE DATABASE_DEFAULT  \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wpnow from tbl_Planning where activity <> 1 and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate()) and year(calendardate) = year(Getdate())) b on a.w = b.wpnow \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp1 from tbl_Planning where activity <> 1  and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 7) and year(calendardate) = year(Getdate())) c on a.w = c.wp1 \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp2 from tbl_Planning where activity <> 1  and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 14) and year(calendardate) = year(Getdate())) d on a.w = d.wp2 \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp3 from tbl_Planning where activity <> 1  and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 21) and year(calendardate) = year(Getdate())) e on a.w = e.wp3 \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp4 from tbl_Planning where activity <> 1 and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 28) and year(calendardate) = year(Getdate())) f on a.w = f.wp4 \r\n" +

                                              "  left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp5 from tbl_Planning where activity <> 1  and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 35) and year(calendardate) = year(Getdate())) g on a.w = g.wp5 \r\n" +

                                              "   left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp6 from tbl_Planning where activity <> 1  and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 42) and year(calendardate) = year(Getdate())) h on a.w = h.wp6 \r\n" +

                                              " left outer join \r\n" +
                                              " (select s.sectionid, s.sectionid + ' ' + name nn from tbl_Section s, (select sectionid, max(prodmonth)pm from tbl_Section where hierarchicalid = 4 \r\n" +
                                              "  group by sectionid) b where s.sectionid = b.sectionid and s.prodmonth = b.pm) i on a.mo = i.sectionid) a \r\n" +
                                              " where(wpnow + wp1 + wp2 + wp3 + wp4 + wp5 + wp6 <> '') or  description like '%-S%' \r\n" +
                                              ") a \r\n" +

                                              ")a \r\n" +
                                              "Left outer join \r\n" +
                                              "(Select DATEDIFF(DAY, LastVisitDate, Getdate()) DaysSince, a.* from( \r\n" +
                                              "Select Max(CaptDate) LastVisitDate, Workplace from  [tbl_DPT_GeoScience_Inspection] \r\n" +
                                              "Group by Workplace) a " +

                                              " ) b on a.Description = b.Workplace  order by nn,Description";
            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            DataTable dtDetail = _dbManWPSTDetail.ResultsDataTable;
            DataSet ds = new DataSet();
            ds.Tables.Add(dtDetail);

            gcGeology.DataSource = ds.Tables[0];

            col1SecIDGS.FieldName = "nn";
            col1SecIDGS.Visible = false;
            col1WpGS.FieldName = "description";
            col1RRGS.FieldName = "rr";
            col1Wk1GS.FieldName = "wp6";
            col1Wk2GS.FieldName = "wp5";
            col1Wk3GS.FieldName = "wp4";
            col1Wk4GS.FieldName = "wp3";
            col1Wk5GS.FieldName = "wp2";
            col1Wk6GS.FieldName = "wp1";
            col1WkNowGS.FieldName = "wpnow";

            Col1Day1GS.FieldName = "col6";
            Col1Day2GS.FieldName = "col5";
            Col1Day3GS.FieldName = "col4";
            Col1Day4GS.FieldName = "col3";
            Col1Day5GS.FieldName = "col2";
            Col1Day6GS.FieldName = "col1";
            Col1Day7GS.FieldName = "colnow";

            if (dtDetail.Rows.Count > 0)
            {
                Col1Day1GS.Caption = dtDetail.Rows[0]["hhwk6"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk6"].ToString() + "-" + dtDetail.Rows[0]["Endatewk6"].ToString() + ")";
                Col1Day2GS.Caption = dtDetail.Rows[0]["hhwk5"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk5"].ToString() + "-" + dtDetail.Rows[0]["Endatewk5"].ToString() + ")";
                Col1Day3GS.Caption = dtDetail.Rows[0]["hhwk4"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk4"].ToString() + "-" + dtDetail.Rows[0]["Endatewk4"].ToString() + ")";
                Col1Day4GS.Caption = dtDetail.Rows[0]["hhwk3"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk3"].ToString() + "-" + dtDetail.Rows[0]["Endatewk3"].ToString() + ")";
                Col1Day5GS.Caption = dtDetail.Rows[0]["hhwk2"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk2"].ToString() + "-" + dtDetail.Rows[0]["Endatewk2"].ToString() + ")";
                Col1Day6GS.Caption = dtDetail.Rows[0]["hhwk1"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk1"].ToString() + "-" + dtDetail.Rows[0]["Endatewk1"].ToString() + ")";
                Col1Day7GS.Caption = dtDetail.Rows[0]["hhwnow"].ToString() + "\r\n(" + dtDetail.Rows[0]["StartdateNow"].ToString() + "-" + dtDetail.Rows[0]["EndateNow"].ToString() + ")";
                //Col1Day1GS.Caption = dtDetail.Rows[0]["hhwk6"].ToString();
                //Col1Day2GS.Caption = dtDetail.Rows[0]["hhwk5"].ToString();
                //Col1Day3GS.Caption = dtDetail.Rows[0]["hhwk4"].ToString();
                //Col1Day4GS.Caption = dtDetail.Rows[0]["hhwk3"].ToString();
                //Col1Day5GS.Caption = dtDetail.Rows[0]["hhwk2"].ToString();
                //Col1Day6GS.Caption = dtDetail.Rows[0]["hhwk1"].ToString();
                //Col1Day7GS.Caption = dtDetail.Rows[0]["hhwnow"].ToString();
            }

            colLastVisitGS.FieldName = "LastVisitDate";
            colDaysSinceLastVisitGS.FieldName = "DaysSince";

            gvGeology.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

            gvGeology.Columns[0].GroupIndex = 0;

            gvGeology.ExpandAllGroups();

            gcGeology.Dock = DockStyle.Fill;
            gcGeology.Visible = true;
        }


        public void LoadGeoInspGridDev()
        {
            MWDataManager.clsDataAccess _dbManWK = new MWDataManager.clsDataAccess();
            _dbManWK.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _dbManWK.SqlStatement = "select distinct(a) a from ( " +
                                           "select convert(varchar(10),captyear) +' Wk' + substring('00'+convert(varchar(10),captweek+1000),5,2) a " +
                                           "from  [tbl_DPT_GeoScience_Inspection]) a order by a desc ";
            _dbManWK.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWK.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWK.ExecuteInstruction();

            DataTable dtwk = _dbManWK.ResultsDataTable;

            listBoxWk.Items.Clear();
            listBoxWk.Items.Add(string.Empty);
            foreach (DataRow dr in dtwk.Rows)
            {
                listBoxWk.Items.Add(dr["a"].ToString());
            }

            listBoxWk.SelectedIndex = 0;

            GeoScienceInspPnl.BringToFront();
            GeoScienceInspPnl.Dock = DockStyle.Fill;
            GeoScienceInspPnl.Visible = true;

            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _dbManWPSTDetail.SqlStatement = "Select * from(select *, isnull(convert(decimal(18, 0), 0 + 0.4), 0) aa from(select * \r\n" +

                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 42))), 2, 2) hhwk6 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 35))), 2, 2) hhwk5 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 28))), 2, 2) hhwk4 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 21))), 2, 2) hhwk3 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 14))), 2, 2) hhwk2 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate() - 7))), 2, 2) hhwk1 \r\n" +
                                              " , 'Wk-' + substring(convert(varchar(5), 100 + (DATEPART(ISOWK, Getdate()))), 2, 2) hhwnow \r\n" +

                                              ",substring( convert(varchar(30), DATEADD( DAY,- DATEPART(dw, getdate()-2), GETDATE()-42 ),106 )  ,1,6) Startdatewk6 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 42)), 106), 1, 6) Endatewk6 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 35), 106), 1, 6) Startdatewk5 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 35)), 106), 1, 6) Endatewk5 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 28), 106), 1, 6) Startdatewk4 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 28)), 106), 1, 6) Endatewk4 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 21), 106), 1, 6) Startdatewk3 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 21)), 106), 1, 6) Endatewk3 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 14), 106), 1, 6) Startdatewk2 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 14)), 106), 1, 6) Endatewk2 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 7), 106), 1, 6) Startdatewk1 \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE() - 7)), 106), 1, 6) Endatewk1 \r\n" +

                                              ",substring(convert(varchar(30), DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE()), 106), 1, 6) StartdateNow \r\n" +
                                              ",substring(convert(varchar(30), DATEADD(DAY, 6, DATEADD(DAY, -DATEPART(dw, getdate() - 2), GETDATE())), 106), 1, 6) EndateNow \r\n" +

                                              " , case when act = '1' then 'Dev' Else 'Stp' end as activityfinal \r\n" +
                                              " from(select nn, a.description, act, rr, case when description like '%-S%' then 'a' else isnull(wp6, '') end as wp6, case when description like '%-S%' then 'a' else isnull(wp5, '') end as wp5, case when description like '%-S%' then 'a' else isnull(wp4, '') end as wp4, case when description like '%-S%' then 'a' else isnull(wp3, '') end as wp3 \r\n" +
                                              " , case when description like '%-S%' then 'a' else isnull(wp2, '') end as wp2, case when description like '%-S%' then 'a' else isnull(wp1, '') end as wp1, case when description like '%-S%' then 'a' else isnull(wpnow, '') end as wpnow, isnull(colnow, '') colnow, isnull(col6, '') col6, isnull(col5, '') col5, isnull(col4, '') col4, isnull(col3, '') col3, isnull(col2, '') col2, isnull(col1, '') col1   from(select substring(sectionid, 1, 4) mo, w.description, p.workplaceid w, max(p.activity) act, max(riskrating) rr from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid \r\n" +
                                              " and calendardate > getdate() - 60 and calendardate < getdate() + 7 \r\n" +
                                              " and p.activity = 1 group by w.description, substring(sectionid, 1, 4), p.workplaceid \r\n" +
                                              " union \r\n" +
                                              " select[mo],[WPDescription],[WNo],[act],[rr] from vw_Department_Walkabout_Sum \r\n" +
                                              " ) a \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col6 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 42))), 1, 2)) zz6 on a.Description = zz6.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col5 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 35))), 1, 2)) zz5 on a.Description = zz5.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col4 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 28))), 1, 2)) zz4 on a.Description = zz4.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col3 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 21))), 1, 2)) zz3 on a.Description = zz3.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col2 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 14))), 1, 2)) zz2 on a.Description = zz2.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' col1 from  [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                              " CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate() - 7))), 1, 2)) zz1 on a.Description = zz1.w COLLATE DATABASE_DEFAULT \r\n" +

                                              " left outer  join \r\n" +
                                              " (select Workplace w, 'Y' colnow from [tbl_DPT_GeoScience_Inspection] where  \r\n" +
                                              "CaptWeek = substring(convert(varchar(5), (DATEPART(ISOWK, Getdate()))), 1, 2)  and captyear = datepart(year, getdate())) zz on a.Description = zz.w COLLATE DATABASE_DEFAULT  \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wpnow from tbl_Planning where activity = 1 and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate()) and year(calendardate) = year(Getdate())) b on a.w = b.wpnow \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp1 from tbl_Planning where activity = 1 and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 7) and year(calendardate) = year(Getdate())) c on a.w = c.wp1 \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp2 from tbl_Planning where activity = 1  and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 14) and year(calendardate) = year(Getdate())) d on a.w = d.wp2 \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp3 from tbl_Planning where activity = 1  and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 21) and year(calendardate) = year(Getdate())) e on a.w = e.wp3 \r\n" +

                                              " left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp4 from tbl_Planning where activity = 1 and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 28) and year(calendardate) = year(Getdate())) f on a.w = f.wp4 \r\n" +

                                              "  left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp5 from tbl_Planning where activity = 1  and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 35) and year(calendardate) = year(Getdate())) g on a.w = g.wp5 \r\n" +

                                              "   left outer  join \r\n" +
                                              " (select distinct(workplaceid) wp6 from tbl_Planning where activity = 1  and \r\n" +
                                              " DATEPART(ISOWK, calendardate) = DATEPART(ISOWK, Getdate() - 42) and year(calendardate) = year(Getdate())) h on a.w = h.wp6 \r\n" +

                                              " left outer join \r\n" +
                                              " (select s.sectionid, s.sectionid + ' ' + name nn from tbl_Section s, (select sectionid, max(prodmonth)pm from tbl_Section where hierarchicalid = 4 \r\n" +
                                              "  group by sectionid) b where s.sectionid = b.sectionid and s.prodmonth = b.pm) i on a.mo = i.sectionid) a \r\n" +
                                              " where(wpnow + wp1 + wp2 + wp3 + wp4 + wp5 + wp6 <> '') or  description like '%-S%' \r\n" +
                                              ") a \r\n" +

                                              ")a \r\n" +
                                              "Left outer join \r\n" +
                                              "(Select DATEDIFF(DAY, LastVisitDate, Getdate()) DaysSince, a.* from( \r\n" +
                                              "Select Max(CaptDate) LastVisitDate, Workplace from  [tbl_DPT_GeoScience_Inspection] \r\n" +
                                              "Group by Workplace) a " +

                                              " ) b on a.Description = b.Workplace  order by nn,Description";
            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            DataTable dtDetail = _dbManWPSTDetail.ResultsDataTable;
            DataSet ds = new DataSet();
            ds.Tables.Add(dtDetail);

            gcGeology.DataSource = ds.Tables[0];

            col1SecIDGS.FieldName = "nn";
            col1SecIDGS.Visible = false;
            col1WpGS.FieldName = "description";
            col1RRGS.FieldName = "rr";
            col1Wk1GS.FieldName = "wp6";
            col1Wk2GS.FieldName = "wp5";
            col1Wk3GS.FieldName = "wp4";
            col1Wk4GS.FieldName = "wp3";
            col1Wk5GS.FieldName = "wp2";
            col1Wk6GS.FieldName = "wp1";
            col1WkNowGS.FieldName = "wpnow";

            Col1Day1GS.FieldName = "col6";
            Col1Day2GS.FieldName = "col5";
            Col1Day3GS.FieldName = "col4";
            Col1Day4GS.FieldName = "col3";
            Col1Day5GS.FieldName = "col2";
            Col1Day6GS.FieldName = "col1";
            Col1Day7GS.FieldName = "colnow";

            if (dtDetail.Rows.Count > 0)
            {
                Col1Day1GS.Caption = dtDetail.Rows[0]["hhwk6"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk6"].ToString() + "-" + dtDetail.Rows[0]["Endatewk6"].ToString() + ")";
                Col1Day2GS.Caption = dtDetail.Rows[0]["hhwk5"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk5"].ToString() + "-" + dtDetail.Rows[0]["Endatewk5"].ToString() + ")";
                Col1Day3GS.Caption = dtDetail.Rows[0]["hhwk4"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk4"].ToString() + "-" + dtDetail.Rows[0]["Endatewk4"].ToString() + ")";
                Col1Day4GS.Caption = dtDetail.Rows[0]["hhwk3"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk3"].ToString() + "-" + dtDetail.Rows[0]["Endatewk3"].ToString() + ")";
                Col1Day5GS.Caption = dtDetail.Rows[0]["hhwk2"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk2"].ToString() + "-" + dtDetail.Rows[0]["Endatewk2"].ToString() + ")";
                Col1Day6GS.Caption = dtDetail.Rows[0]["hhwk1"].ToString() + "\r\n(" + dtDetail.Rows[0]["Startdatewk1"].ToString() + "-" + dtDetail.Rows[0]["Endatewk1"].ToString() + ")";
                Col1Day7GS.Caption = dtDetail.Rows[0]["hhwnow"].ToString() + "\r\n(" + dtDetail.Rows[0]["StartdateNow"].ToString() + "-" + dtDetail.Rows[0]["EndateNow"].ToString() + ")";
                //Col1Day1GS.Caption = dtDetail.Rows[0]["hhwk6"].ToString();
                //Col1Day2GS.Caption = dtDetail.Rows[0]["hhwk5"].ToString();
                //Col1Day3GS.Caption = dtDetail.Rows[0]["hhwk4"].ToString();
                //Col1Day4GS.Caption = dtDetail.Rows[0]["hhwk3"].ToString();
                //Col1Day5GS.Caption = dtDetail.Rows[0]["hhwk2"].ToString();
                //Col1Day6GS.Caption = dtDetail.Rows[0]["hhwk1"].ToString();
                //Col1Day7GS.Caption = dtDetail.Rows[0]["hhwnow"].ToString();
            }

            colLastVisitGS.FieldName = "LastVisitDate";
            colDaysSinceLastVisitGS.FieldName = "DaysSince";

            gvGeology.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

            gvGeology.Columns[0].GroupIndex = 0;

            gvGeology.ExpandAllGroups();

            gcGeology.Dock = DockStyle.Fill;
            gcGeology.Visible = true;
        }

        private void gvGeology_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.AbsoluteIndex == 1)
            {
                if (View.FocusedRowHandle != GridControl.InvalidRowHandle)
                {
                    if (View.GetRowCellValue(e.RowHandle, e.Column).ToString().Contains("Sum") == true)
                    {
                        e.Appearance.BackColor = Color.Salmon;
                    }
                }
            }

            if (View.GetRowCellValue(e.RowHandle, "wp6").ToString() == string.Empty)
            {
                if (e.Column.AbsoluteIndex == 3)
                {
                    e.DisplayText = "No Act.";
                    e.Appearance.ForeColor = Color.DarkGray;
                    e.Appearance.BackColor = Color.Gainsboro;
                }
            }

            if (View.GetRowCellValue(e.RowHandle, "wp5").ToString() == string.Empty)
            {
                if (e.Column.AbsoluteIndex == 4)
                {
                    e.DisplayText = "No Act.";
                    e.Appearance.ForeColor = Color.DarkGray;
                    e.Appearance.BackColor = Color.Gainsboro;
                }
            }

            if (View.GetRowCellValue(e.RowHandle, "wp4").ToString() == string.Empty)
            {
                if (e.Column.AbsoluteIndex == 5)
                {
                    e.DisplayText = "No Act.";
                    e.Appearance.ForeColor = Color.DarkGray;
                    e.Appearance.BackColor = Color.Gainsboro;
                }
            }

            if (View.GetRowCellValue(e.RowHandle, "wp3").ToString() == string.Empty)
            {
                if (e.Column.AbsoluteIndex == 6)
                {
                    e.DisplayText = "No Act.";
                    e.Appearance.ForeColor = Color.DarkGray;
                    e.Appearance.BackColor = Color.Gainsboro;
                }
            }

            if (View.GetRowCellValue(e.RowHandle, "wp2").ToString() == string.Empty)
            {
                if (e.Column.AbsoluteIndex == 7)
                {
                    e.DisplayText = "No Act.";
                    e.Appearance.ForeColor = Color.DarkGray;
                    e.Appearance.BackColor = Color.Gainsboro;
                }
            }

            if (View.GetRowCellValue(e.RowHandle, "wp1").ToString() == string.Empty)
            {
                if (e.Column.AbsoluteIndex == 8)
                {
                    e.DisplayText = "No Act.";
                    e.Appearance.ForeColor = Color.DarkGray;
                    e.Appearance.BackColor = Color.Gainsboro;
                }
            }

            if (View.GetRowCellValue(e.RowHandle, "wpnow").ToString() == string.Empty)
            {
                if (e.Column.AbsoluteIndex == 9)
                {
                    e.DisplayText = "No Act.";
                    e.Appearance.ForeColor = Color.DarkGray;
                    e.Appearance.BackColor = Color.Gainsboro;
                }
            }
        }

        private void gvGeology_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            WPLbl = gvGeology.GetRowCellValue(e.RowHandle, gvGeology.Columns[1]).ToString();
            label36 = gvGeology.GetRowCellValue(e.RowHandle, gvGeology.Columns[2]).ToString();

            clickcol = e.Column.ToString();

            clickcol = "0";

            if (e.Column.Name.ToString() == "Col1Day1GS")
            {
                clickcol = "1";
                WKLbl = gvGeology.Columns[3].Caption.ToString();
            }

            if (e.Column.Name.ToString() == "Col1Day2GS")
            {
                clickcol = "2";
                WKLbl = gvGeology.Columns[4].Caption.ToString();
            }

            if (e.Column.Name.ToString() == "Col1Day3GS")
            {
                clickcol = "3";
                WKLbl = gvGeology.Columns[5].Caption.ToString();
            }

            if (e.Column.Name.ToString() == "Col1Day4GS")
            {
                clickcol = "4";
                WKLbl = gvGeology.Columns[6].Caption.ToString();
            }

            if (e.Column.Name.ToString() == "Col1Day5GS")
            {
                clickcol = "5";
                WKLbl = gvGeology.Columns[7].Caption.ToString();
            }

            if (e.Column.Name.ToString() == "Col1Day6GS")
            {
                clickcol = "6";
                WKLbl = gvGeology.Columns[8].Caption.ToString();
            }

            if (e.Column.Name.ToString() == "Col1Day7GS")
            {
                clickcol = "7";
                WKLbl = gvGeology.Columns[9].Caption.ToString();
            }
        }

        private void gvGeology_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.Name.ToString() == "col1RRGS")
            {
                if ((decimal)e.CellValue < 70)
                {
                    e.Appearance.ForeColor = Color.Green;
                }

                if ((decimal)e.CellValue > 70 && (decimal)e.CellValue < 140)
                {
                    e.Appearance.ForeColor = Color.Orange;
                }

                if ((decimal)e.CellValue > 140)
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }

            //// 0 -- invisible
            if (e.CellValue != null && e.Column.ColumnType == typeof(decimal))
            {
                if ((decimal)e.CellValue == 0)
                {
                    e.Appearance.ForeColor = e.Appearance.BackColor;
                }
            }
            if (e.CellValue != null && e.Column.ColumnType == typeof(int))
            {
                if (e.CellValue.ToString() == "0")
                {
                    e.Appearance.ForeColor = e.Appearance.BackColor;
                }
            }
        }

        private void gvGeology_DoubleClick(object sender, EventArgs e)
        {
            if (gvGeology.FocusedColumn.Name == "col1RRGS" || gvGeology.FocusedColumn.Name == "colLastVisitGS"
                || gvGeology.FocusedColumn.Name == "col1WpGS" || gvGeology.FocusedColumn.Name == "colDaysSinceLastVisitGS"
                || gvGeology.FocusedColumn.Name == "ColActGS")
            {
                return;
            }

            

            if (editActivity.EditValue.ToString() == "0")
            {
                GeoInspFrm FPMessagefrm = new GeoInspFrm();
                FPMessagefrm.WPLbl.EditValue = WPLbl;
                FPMessagefrm.WkLbl.Text = clickcol;
                FPMessagefrm.WkLbl2.EditValue = WKLbl;
                FPMessagefrm.RRLbl.Text = label36;
                FPMessagefrm._UserCurrentInfo = UserCurrentInfo.Connection;
                FPMessagefrm.ShowDialog();
                LoadGeoInspGrid();
            }
            else
            {
                GeoInspFrmDev FPMessagefrm = new GeoInspFrmDev();
                FPMessagefrm.WPLbl.EditValue = WPLbl;
                FPMessagefrm.WkLbl.Text = clickcol;
                FPMessagefrm.WkLbl2.EditValue = WKLbl;
                FPMessagefrm.RRLbl.Text = label36;
                FPMessagefrm._UserCurrentInfo = UserCurrentInfo.Connection;
                FPMessagefrm.ShowDialog();
                LoadGeoInspGridDev();
            }
        }

        private void listBoxWk_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelwwk.Text = listBoxWk.SelectedItem.ToString();
        }

        private void listBoxSum_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelsum1.Text = listBoxSum.SelectedItem.ToString();
        }

        private void labelwwk_TextChanged(object sender, EventArgs e)
        {
            listBoxSum.Items.Clear();
            if (labelwwk.Text != string.Empty)
            {
                MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
                _dbManWP.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                _dbManWP.SqlStatement = " select distinct(s1.reporttosectionid) mo from  " +
                                        "tbl_Planning p, tbl_Section s, tbl_Section s1  where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth  " +
                                        " and s.reporttosectionid = s1.sectionid and s.prodmonth = s1.prodmonth   " +
                                        " and activity <> 1   " +
                                        " and Datepart(isowk, calendardate) = substring('" + labelwwk.Text + "',8,2) and year(calendardate) =  substring('" + labelwwk.Text + "',1,4)  " +
                                        " order by s1.reporttosectionid ";
                _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWP.ExecuteInstruction();

                DataTable dtwp = _dbManWP.ResultsDataTable;

                listBoxSum.Items.Add(string.Empty);
                foreach (DataRow dr in dtwp.Rows)
                {
                    listBoxSum.Items.Add(dr["mo"].ToString());
                }

                listBoxSum.SelectedIndex = 0;
            }
        }

        private void labelsum1_TextChanged(object sender, EventArgs e)
        {
            if (labelsum1.Text == string.Empty)
                return;

            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _dbManWP.SqlStatement = "declare @year1 varchar(10)  \r\n" +
                                    "declare @ww varchar(10)  \r\n" +
                                    "declare @mo varchar(10)  \r\n" +

                                    "set @year1 = substring('" + labelwwk.Text + "',1,4)   \r\n" +
                                    "set @ww = substring('" + labelwwk.Text + "',8,2)  \r\n" +
                                    "set @mo = '" + labelsum1.Text + "'  \r\n" +

                                    "select @mo mo, @year1 +' wk'+@ww www, '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' hh, * from (select '1' ll, 'RIF/RIH' lbl, workplace,   \r\n" +
                                    "case when RIF = 'N' then 'O' else 'R' end as CC,RIFNote cat25note  \r\n" +

                                    " from   [tbl_DPT_GeoScience_Inspection] where  \r\n" +
                                    "captyear = @year1 and captweek = convert(decimal(18,0),@ww) and workplace in (  \r\n" +

                                    "select distinct(description) aa from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and  \r\n" +
                                    " year(calendardate) = @year1 and datepart(week,calendardate) = convert(decimal(18,0),@ww)  \r\n" +
                                    " and substring(sectionid,1,4) = @mo  \r\n" +
                                    " )  \r\n" +
                                    "-- and RIF <> 0 \r\n" +

                                    " union \r\n" +

                                    " select '2' ll, 'Fault' lbl, workplace,  \r\n" +
                                    "case when Fault = 'N' then 'O' else 'R' end as CC,FaultNote cat29note \r\n" +

                                    " from   [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                    "captyear = @year1 and captweek = convert(decimal(18,0),@ww) and workplace in ( \r\n" +

                                    "select distinct(description) aa from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and \r\n" +
                                    " year(calendardate) = @year1 and datepart(week,calendardate) = convert(decimal(18,0),@ww)  \r\n" +
                                    " and substring(sectionid,1,4) = @mo \r\n" +
                                    " ) \r\n" +
                                    "-- and Fault <> 0 \r\n" +

                                    "  union \r\n" +

                                    " select '3' ll, 'Dyke' lbl, workplace, \r\n" +
                                    "case when Dykes = 'N' then 'O' else 'R' end as CC,DykesNote cat30note \r\n" +

                                    "from   [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                    "captyear = @year1 and captweek = convert(decimal(18,0),@ww) and workplace in ( \r\n" +

                                    "select distinct(description) aa from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and \r\n" +
                                    " year(calendardate) = @year1 and datepart(week,calendardate) = convert(decimal(18,0),@ww)  \r\n" +
                                    " and substring(sectionid,1,4) = @mo \r\n" +
                                    " ) \r\n" +
                                    "-- and Dykes <> 0 \r\n" +

                                    "   union \r\n" +

                                    " select '4' ll, 'Sill' lbl, workplace,  \r\n" +
                                    "'' CC,'' cat31note \r\n" +

                                    " from   [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                    "captyear = @year1 and captweek = convert(decimal(18,0),@ww) and workplace in ( \r\n" +

                                    "select distinct(description) aa from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and \r\n" +
                                    " year(calendardate) = @year1 and datepart(week,calendardate) = convert(decimal(18,0),@ww)  \r\n" +
                                    " and substring(sectionid,1,4) = @mo \r\n" +
                                    " ) \r\n" +
                                    "-- and cat31rate <> 0 \r\n" +

                                    "  union \r\n" +

                                    " select '5' ll, 'Reef Position' lbl, workplace, \r\n" +
                                    "case when ReefRoll = 'N' then 'O' else 'R' end as CC,ReefRollNote cat32note \r\n" +
                                    " from   [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                    "captyear = @year1 and captweek = convert(decimal(18,0),@ww) and workplace in ( \r\n" +

                                    "select distinct(description) aa from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and \r\n" +
                                    " year(calendardate) = @year1 and datepart(week,calendardate) = convert(decimal(18,0),@ww)  \r\n" +
                                    " and substring(sectionid,1,4) = @mo \r\n" +
                                    " ) \r\n" +
                                    "-- and ReefRoll <> 0 \r\n" +

                                    "  union \r\n" +

                                    " select '6' ll, 'Stope Width' lbl, workplace,  \r\n" +
                                    "'' CC,'' cat33note \r\n" +

                                    "from   [tbl_DPT_GeoScience_Inspection] where \r\n" +
                                    "captyear = @year1 and captweek = convert(decimal(18,0),@ww) and workplace in ( \r\n" +

                                    "select distinct(description) aa from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and \r\n" +
                                    " year(calendardate) = @year1 and datepart(week,calendardate) = convert(decimal(18,0),@ww) \r\n" +
                                    " and substring(sectionid,1,4) = @mo \r\n" +
                                    " ) \r\n" +
                                    "-- and cat33rate <> 0  \r\n" +
                                    ") a \r\n";
            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ResultsTableName = "REDetail2";
            _dbManWP.ExecuteInstruction();

            DataSet Data1 = new DataSet();
            Data1.Tables.Add(_dbManWP.ResultsDataTable);

            theReportGeoSum.RegisterData(Data1);
            theReportGeoSum.Load(ReportsFolder + "\\GeolSumReport.frx");
            //theReportGeoSum.Design();

            previewControl3.Clear();
            theReportGeoSum.Prepare();
            theReportGeoSum.Preview = previewControl3;
            theReportGeoSum.ShowPrepared();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnExp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (expanded)
            {
                gvGeology.CollapseAllGroups();
                expanded = false;
                return;
            }

            if (!expanded)
            {
                gvGeology.ExpandAllGroups();
                expanded = true;
                return;
            }
        }

        private void editActivity_EditValueChanged(object sender, EventArgs e)
        {
            if (editActivity.EditValue.ToString() == "0")
            {
                LoadGeoInspGrid();
            }
            else
            {
                LoadGeoInspGridDev();
            }
        }
    }
}
