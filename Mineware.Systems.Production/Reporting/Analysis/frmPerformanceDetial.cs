using DevExpress.XtraNavBar;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Mineware.Systems.Production.Reporting.Analysis
{
    public partial class frmPerformanceDetial : DevExpress.XtraEditors.XtraForm
    {
        #region private Variables
        string Filter = string.Empty;
        DataTable TableGraph = new DataTable();
        private string Probpm1;
        #endregion

        #region Public Variables
        public string _strFilter;
        public string currMonth;
        public string StopeOrDev;
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        #endregion

        public frmPerformanceDetial()
        {
            InitializeComponent();
        }

        private void frmPerformanceDetial_Load(object sender, EventArgs e)
        {
            if (_strFilter == "0")
                Filter = "Crew";
            if (_strFilter == "1")
                Filter = "Miner";
            if (_strFilter == "2")
                Filter = "SB";
            if (_strFilter == "3")
                Filter = "MO";


            navBarControl2.OptionsNavPane.NavPaneState = NavPaneState.Collapsed;

            int aa = currMonth.IndexOf("-");

            string currentYear = currMonth.Substring(aa + 1, currMonth.Length - (aa + 1));

            if (StopeOrDev == "0")
            {
                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                if (Filter == "Crew")
                    _dbManMonth.SqlStatement = " exec sp_PeformanceDetial '" + currentYear + "',  '" + lblCrew.Text + "'     ";
                if (Filter == "Miner")
                    _dbManMonth.SqlStatement = " exec sp_PeformanceDetialMiner '" + currentYear + "',  '" + lblCrew.Text + "'     ";
                if (Filter == "SB")
                    _dbManMonth.SqlStatement = " exec sp_PeformanceDetialSB '" + currentYear + "',  '" + lblCrew.Text + "'     ";
                if (Filter == "MO")
                    _dbManMonth.SqlStatement = " exec sp_PeformanceDetialMO '" + currentYear + "',  '" + lblCrew.Text + "'     ";


                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ResultsTableName = "aa";  //get table name
                _dbManMonth.ExecuteInstruction();

                TableGraph = _dbManMonth.ResultsDataTable;

                loadGrid();
                LoadGraph();
                LoadComments();
            }


            if (StopeOrDev == "1")
            {
                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                if (Filter == "Crew")
                    _dbManMonth.SqlStatement = " exec sp_PeformanceDetialDev '" + currentYear + "',  '" + lblCrew.Text + "'     ";
                if (Filter == "Miner")
                    _dbManMonth.SqlStatement = " exec sp_PeformanceDetialDevMin '" + currentYear + "',  '" + lblCrew.Text + "'     ";
                if (Filter == "SB")
                    _dbManMonth.SqlStatement = " exec sp_PeformanceDetialDevSB '" + currentYear + "',  '" + lblCrew.Text + "'     ";
                if (Filter == "MO")
                    _dbManMonth.SqlStatement = " exec sp_PeformanceDetialDevMO '" + currentYear + "',  '" + lblCrew.Text + "'     ";
                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ResultsTableName = "aa";  //get table name
                _dbManMonth.ExecuteInstruction();

                TableGraph = _dbManMonth.ResultsDataTable;

                loadGrid();
                LoadGraph();
                LoadComments();

            }

            LoadDefaultProblems();
        }

        private void loadGrid()
        {
            if (StopeOrDev == "0")
            {
                MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
                _dbManData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManData.SqlStatement = " select * from [dbo].[tbl_PerformanceCrewColumnsDetial] where useyn = 'Y' " +
                                            " order by [order] ";
                _dbManData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManData.ResultsTableName = "aa";  //get table name
                _dbManData.ExecuteInstruction();

                DataSet ds = new DataSet();
                ds.Tables.Add(_dbManData.ResultsDataTable);

                GridOrig.DataSource = ds.Tables[0];

                colDescription.FieldName = "ColDescription";
                colY.FieldName = "YAxis";
                colOnOff.FieldName = "Show";
                colColor.FieldName = "ColColour";
                colOrder.FieldName = "Order";
                colChartStyle.FieldName = "ChartStyle";
                colShowLbl.FieldName = "ShowLbl";
            }

            if (StopeOrDev == "1")
            {
                MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
                _dbManData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManData.SqlStatement = " select * from [dbo].[tbl_PerformanceCrewColumnsDevDetial] where useyn = 'Y' " +
                                            " order by [order] ";
                _dbManData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManData.ResultsTableName = "aa";  //get table name
                _dbManData.ExecuteInstruction();

                DataSet ds = new DataSet();
                ds.Tables.Add(_dbManData.ResultsDataTable);


                GridOrig.DataSource = ds.Tables[0];

                colDescription.FieldName = "ColDescription";
                colY.FieldName = "YAxis";
                colOnOff.FieldName = "Show";
                colColor.FieldName = "ColColour";
                colOrder.FieldName = "Order";
                colChartStyle.FieldName = "ChartStyle";
                colShowLbl.FieldName = "ShowLbl";
            }
        }
        private void LoadGraph()
        {
            //TableGraph.DefaultView.Sort = gridView7.GetRowCellValue(0, gridView7.Columns["ColDescription"]).ToString();
            //TableGraph = TableGraph.DefaultView.ToTable();

            Chart.Series[0].Name = "a";
            Chart.Series[1].Name = "b";
            Chart.Series[2].Name = "c";
            Chart.Series[3].Name = "d";
            Chart.Series[4].Name = "e";
            Chart.Series[5].Name = "f";
            Chart.Series[6].Name = "g";
            Chart.Series[7].Name = "h";
            Chart.Series[8].Name = "i";
            Chart.Series[9].Name = "j";
            Chart.Series[10].Name = "k";
            Chart.Series[11].Name = "l";
            Chart.Series[12].Name = "m";
            Chart.Series[13].Name = "n";
            Chart.Series[14].Name = "o";
            Chart.Series[15].Name = "p";
            Chart.Series[16].Name = "q";
            Chart.Series[17].Name = "s";
            Chart.Series[18].Name = "r";
            Chart.Series[19].Name = "t";

            Chart.Series[0].Points.Clear();
            Chart.Series[1].Points.Clear();
            Chart.Series[2].Points.Clear();
            Chart.Series[3].Points.Clear();
            Chart.Series[4].Points.Clear();
            Chart.Series[5].Points.Clear();
            Chart.Series[6].Points.Clear();
            Chart.Series[7].Points.Clear();
            Chart.Series[8].Points.Clear();
            Chart.Series[9].Points.Clear();
            Chart.Series[10].Points.Clear();
            Chart.Series[11].Points.Clear();
            Chart.Series[12].Points.Clear();
            Chart.Series[13].Points.Clear();
            Chart.Series[14].Points.Clear();
            Chart.Series[15].Points.Clear();
            Chart.Series[16].Points.Clear();
            Chart.Series[17].Points.Clear();
            Chart.Series[18].Points.Clear();
            Chart.Series[19].Points.Clear();

            if (StopeOrDev == "0")
            {
                Chart.Series[0].IsVisibleInLegend = true;
                Chart.Series[1].IsVisibleInLegend = true;
                Chart.Series[2].IsVisibleInLegend = true;
                Chart.Series[3].IsVisibleInLegend = true;
                Chart.Series[4].IsVisibleInLegend = true;
                Chart.Series[5].IsVisibleInLegend = true;
                Chart.Series[6].IsVisibleInLegend = true;
                Chart.Series[7].IsVisibleInLegend = true;
                Chart.Series[8].IsVisibleInLegend = true;
                Chart.Series[9].IsVisibleInLegend = true;
                Chart.Series[10].IsVisibleInLegend = true;
                Chart.Series[11].IsVisibleInLegend = true;
                Chart.Series[12].IsVisibleInLegend = true;
                Chart.Series[13].IsVisibleInLegend = true;
                Chart.Series[14].IsVisibleInLegend = true;
                Chart.Series[15].IsVisibleInLegend = true;
                Chart.Series[16].IsVisibleInLegend = true;
                Chart.Series[17].IsVisibleInLegend = true;
                Chart.Series[18].IsVisibleInLegend = true;
                Chart.Series[19].IsVisibleInLegend = false;
            }

            if (StopeOrDev == "1")
            {
                Chart.Series[0].IsVisibleInLegend = true;
                Chart.Series[1].IsVisibleInLegend = true;
                Chart.Series[2].IsVisibleInLegend = true;
                Chart.Series[3].IsVisibleInLegend = true;
                Chart.Series[4].IsVisibleInLegend = true;
                Chart.Series[5].IsVisibleInLegend = true;
                Chart.Series[6].IsVisibleInLegend = true;
                Chart.Series[7].IsVisibleInLegend = true;
                Chart.Series[8].IsVisibleInLegend = true;
                Chart.Series[9].IsVisibleInLegend = true;
                Chart.Series[10].IsVisibleInLegend = true;
                Chart.Series[11].IsVisibleInLegend = true;
                Chart.Series[12].IsVisibleInLegend = true;
                Chart.Series[13].IsVisibleInLegend = true;
                Chart.Series[14].IsVisibleInLegend = false;
            }

            for (int i = 0; i < gridView7.RowCount; i++)
            {
                Chart.Series[i].Name = gridView7.GetRowCellValue(i, gridView7.Columns["ColDescription"]).ToString();
            }

            foreach (DataRow dr in TableGraph.Rows)
            {
                if (StopeOrDev == "1")
                {
                    Chart.Series[0].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[0].Name].ToString());
                    Chart.Series[1].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[1].Name].ToString());
                    Chart.Series[2].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[2].Name].ToString());
                    Chart.Series[3].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[3].Name].ToString());
                    Chart.Series[4].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[4].Name].ToString());
                    Chart.Series[5].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[5].Name].ToString());
                    Chart.Series[6].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[6].Name].ToString());
                    Chart.Series[7].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[7].Name].ToString());
                    Chart.Series[8].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[8].Name].ToString());
                    Chart.Series[9].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[9].Name].ToString());
                    Chart.Series[10].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[10].Name].ToString());
                    Chart.Series[11].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[11].Name].ToString());
                    Chart.Series[12].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[12].Name].ToString());
                    Chart.Series[13].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[13].Name].ToString());
                    // Chart.Series[14].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[14].Name].ToString());

                }

                if (StopeOrDev == "0")
                {
                    Chart.Series[0].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[0].Name].ToString());
                    Chart.Series[1].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[1].Name].ToString());
                    Chart.Series[2].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[2].Name].ToString());
                    Chart.Series[3].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[3].Name].ToString());
                    Chart.Series[4].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[4].Name].ToString());
                    Chart.Series[5].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[5].Name].ToString());
                    Chart.Series[6].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[6].Name].ToString());
                    Chart.Series[7].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[7].Name].ToString());
                    Chart.Series[8].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[8].Name].ToString());
                    Chart.Series[9].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[9].Name].ToString());
                    Chart.Series[10].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[10].Name].ToString());
                    Chart.Series[11].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[11].Name].ToString());
                    Chart.Series[12].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[12].Name].ToString());
                    Chart.Series[13].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[13].Name].ToString());
                    Chart.Series[14].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[14].Name].ToString());
                    Chart.Series[15].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[15].Name].ToString());
                    Chart.Series[16].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[16].Name].ToString());
                    Chart.Series[17].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[17].Name].ToString());
                    Chart.Series[18].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[18].Name].ToString());
                    //Chart.Series[19].Points.AddXY(dr["YearA"].ToString(), dr[Chart.Series[19].Name].ToString());
                }

                for (int i = 0; i < gridView7.RowCount; i++)
                {

                    if (gridView7.GetRowCellValue(i, gridView7.Columns["Show"]).ToString() == "N")
                    {
                        Chart.Series[i].Points.Clear();
                        Chart.Series[i].IsVisibleInLegend = false;
                    }

                }

                for (int i = 0; i < gridView7.RowCount; i++)
                {
                    if (gridView7.GetRowCellValue(i, gridView7.Columns["YAxis"]).ToString() == "Prim")
                    {
                        Chart.Series[i].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
                    }

                    if (gridView7.GetRowCellValue(i, gridView7.Columns["YAxis"]).ToString() == "Sec")
                    {
                        Chart.Series[i].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                    }
                }

                for (int i = 0; i < gridView7.RowCount; i++)
                {
                    if (gridView7.GetRowCellValue(i, gridView7.Columns["ChartStyle"]).ToString() == "Bar")
                    {
                        Chart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    }

                    if (gridView7.GetRowCellValue(i, gridView7.Columns["ChartStyle"]).ToString() == "Line")
                    {
                        Chart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    }

                    if (gridView7.GetRowCellValue(i, gridView7.Columns["ChartStyle"]).ToString() == "Dots")
                    {
                        Chart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    }
                }

                for (int i = 0; i < gridView7.RowCount; i++)
                {
                    Chart.Series[i].Color = Color.FromArgb(Convert.ToInt32(gridView7.GetRowCellValue(i, gridView7.Columns["ColColour"])));
                }

                for (int i = 0; i < gridView7.RowCount; i++)
                {
                    if (gridView7.GetRowCellValue(i, gridView7.Columns["ShowLbl"]).ToString() == "Y")
                    {
                        Chart.Series[i].IsValueShownAsLabel = true;
                    }
                    else
                    {
                        Chart.Series[i].IsValueShownAsLabel = false;
                    }
                }

            }
        }
        private void LoadComments()
        {
            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
            _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManMonth.SqlStatement = "Select * from tbl_Performances_Comments where UserID = '" + TUserInfo.UserID + "' and Crew = '" + lblCrew.Text + "'   order by DateEnt  desc     ";

            _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMonth.ExecuteInstruction();

            DataTable dt = _dbManMonth.ResultsDataTable;
            DataSet ds = new DataSet();

            ds.Tables.Add(dt);

            CommentsGrid.ColumnCount = 2;

            CommentsGrid.Columns[0].HeaderText = " ";
            CommentsGrid.Columns[0].Width = CommentsGrid.Width - 20;
            CommentsGrid.Columns[0].ReadOnly = true;
            CommentsGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            CommentsGrid.AllowUserToResizeColumns = false;
            CommentsGrid.AllowUserToResizeRows = false;
            CommentsGrid.RowHeadersVisible = false;
            CommentsGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            CommentsGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            CommentsGrid.Columns[1].Visible = false;

            CommentsGrid.Rows.Clear();

            int x = 0;

            foreach (DataRow dr in dt.Rows)
            {

                if (dr["HeadorText"].ToString() == "1")
                {
                    CommentsGrid.Rows.Add(dr["Comments"].ToString(), dr["HeadorText"].ToString());
                    CommentsGrid.Rows[x].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else
                {
                    CommentsGrid.Rows.Add(dr["Comments"].ToString(), dr["HeadorText"].ToString());
                    CommentsGrid.Rows[x].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                x = x + 1;
            }
        }
        private void LoadDefaultProblems()
        {
            lblProbProdmonth.Text = currMonth;

            string Prodmonth = currMonth;
            string pm1 = string.Empty;

            if (Prodmonth.Substring(0, 3) == "Jan")
                pm1 = Prodmonth.Substring(4, 4) + "01";
            if (Prodmonth.Substring(0, 3) == "Feb")
                pm1 = Prodmonth.Substring(4, 4) + "02";
            if (Prodmonth.Substring(0, 3) == "Mar")
                pm1 = Prodmonth.Substring(4, 4) + "03";
            if (Prodmonth.Substring(0, 3) == "Apr")
                pm1 = Prodmonth.Substring(4, 4) + "04";
            if (Prodmonth.Substring(0, 3) == "May")
                pm1 = Prodmonth.Substring(4, 4) + "05";
            if (Prodmonth.Substring(0, 3) == "Jun")
                pm1 = Prodmonth.Substring(4, 4) + "06";
            if (Prodmonth.Substring(0, 3) == "Jul")
                pm1 = Prodmonth.Substring(4, 4) + "07";
            if (Prodmonth.Substring(0, 3) == "Aug")
                pm1 = Prodmonth.Substring(4, 4) + "08";
            if (Prodmonth.Substring(0, 3) == "Sep")
                pm1 = Prodmonth.Substring(4, 4) + "09";
            if (Prodmonth.Substring(0, 3) == "Oct")
                pm1 = Prodmonth.Substring(4, 4) + "10";
            if (Prodmonth.Substring(0, 3) == "Nov")
                pm1 = Prodmonth.Substring(4, 4) + "11";
            if (Prodmonth.Substring(0, 3) == "Dec")
                pm1 = Prodmonth.Substring(4, 4) + "12";

            Probpm1 = pm1;

            LoadPm1Change();
        }
        private void LoadPm1Change()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            dt.Clear();
            ds.Clear();

            //Stoping
            if (StopeOrDev == "0")
            {
                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManMonth.SqlStatement = "exec sp_PerformanceCrewStoping '" + Probpm1 + "','" + lblCrew.Text + "'  ";
                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ExecuteInstruction();

                dt = _dbManMonth.ResultsDataTable;
            }

            //Developement
            if (StopeOrDev == "1")
            {
                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManMonth.SqlStatement = "exec sp_PerformanceCrewDev '" + Probpm1 + "','" + lblCrew.Text + "'  ";
                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ExecuteInstruction();

                dt = _dbManMonth.ResultsDataTable;
            }

            try
            {

                ds.Tables.Add(dt);

                PerfCommGrid.DataSource = ds.Tables[0];

                colCalendarDate.FieldName = "calendardate";
                colNote.FieldName = "sbossnotes";
                colDescriptProb.FieldName = "description";
            }
            catch { }


            MWDataManager.clsDataAccess _dbManMonthbb = new MWDataManager.clsDataAccess();
            _dbManMonthbb.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManMonthbb.SqlStatement = "Select distinct(w.description) description from planmonth p , workplace w , CREW crew \r\n" +
                                        "where p.workplaceid = w.workplaceid and p.orgunitday = crew.GangNo \r\n" +
                                        "and prodmonth = '" + Probpm1 + "'  \r\n" +
                                        " and Crewname = '" + lblCrew.Text + "' order by description  ";
            _dbManMonthbb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMonthbb.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMonthbb.ExecuteInstruction();

            DataTable dtWP = _dbManMonthbb.ResultsDataTable;
            DataSet dsWP = new DataSet();

            dsWP.Tables.Add(dtWP);

            WPGrid.DataSource = dsWP.Tables[0];

            colWP.FieldName = "description";
        }

        private void gridView7_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.ToString() == "Colour")
            {
                if (gridView7.GetRowCellValue(e.RowHandle, e.Column).ToString() != DBNull.Value.ToString())
                {
                    e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(gridView7.GetRowCellValue(e.RowHandle, e.Column)));
                    e.Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(gridView7.GetRowCellValue(e.RowHandle, e.Column)));
                }
            }
        }

        private void gridView7_DoubleClick(object sender, EventArgs e)
        {
            string col = gridView7.FocusedColumn.Name.ToString();

            string order = string.Empty;
            string DeScription = string.Empty;
            string Axis = string.Empty;
            string color = string.Empty;
            string Show = string.Empty;
            string Style = string.Empty;
            string ShowLbl = string.Empty;

            order = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["Order"]).ToString();
            DeScription = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["ColDescription"]).ToString();
            Axis = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["YAxis"]).ToString();
            color = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["ColColour"]).ToString();
            Show = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["Show"]).ToString();
            Style = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["ChartStyle"]).ToString();
            ShowLbl = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["ShowLbl"]).ToString();


            if (col == "colOnOff")
            {
                if (Show == "N")
                {
                    Show = "Y";
                }
                else
                {
                    Show = "N";
                }
            }

            if (col == "colY")
            {
                if (Axis == "Prim")
                {
                    Axis = "Sec";
                }
                else if (Axis == "Sec")
                {
                    Axis = "Prim";
                }
            }


            if (col == "colColor")
            {
                frmColourPicker OreP = new frmColourPicker(this);
                OreP.StartPosition = FormStartPosition.CenterScreen;
                OreP.RowNum = gridView7.FocusedRowHandle.ToString();
                OreP.FormType = "Detial";
                OreP.ShowDialog();

                color = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["ColColour"]).ToString();
            }


            if (col == "colChartStyle")
            {
                if (Style == "Bar")
                {
                    Style = "Line";
                }
                else if (Style == "Line")
                {
                    Style = "Dots";
                }
                else if (Style == "Dots")
                {
                    Style = "Bar";
                }
            }

            if (col == "colShowLbl")
            {
                if (ShowLbl == "N")
                {
                    ShowLbl = "Y";
                }
                else
                {
                    ShowLbl = "N";
                }
            }

            if (StopeOrDev == "1")
            {
                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumnsDevDetial]  \r\n" +
                                          " set ColColour = '" + color + "' ,  Show = '" + Show + "' , YAxis = '" + Axis + "', ChartStyle = '" + Style + "' ,ShowLbl = '" + ShowLbl + "'  \r\n" +
                                           "where [Order] = '" + order + "' and ColDescription = '" + DeScription + "'  ";
                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ExecuteInstruction();

                loadGrid();
                LoadGraph();
            }

            if (StopeOrDev == "0")
            {
                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumnsDetial]  \r\n" +
                                          " set ColColour = '" + color + "' ,  Show = '" + Show + "' , YAxis = '" + Axis + "', ChartStyle = '" + Style + "' ,ShowLbl = '" + ShowLbl + "'  \r\n" +
                                           "where [Order] = '" + order + "' and ColDescription = '" + DeScription + "'  ";
                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ExecuteInstruction();

                loadGrid();
                LoadGraph();
            }

        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (gridView7.FocusedRowHandle - 1 > -1)
            {
                string orderCurr = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["Order"]).ToString();
                string DescripCurr = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["ColDescription"]).ToString();


                string orderNext = gridView7.GetRowCellValue(gridView7.FocusedRowHandle - 1, gridView7.Columns["Order"]).ToString();
                string DescripNext = gridView7.GetRowCellValue(gridView7.FocusedRowHandle - 1, gridView7.Columns["ColDescription"]).ToString();
                int lastrow = gridView7.FocusedRowHandle - 1;

                if (StopeOrDev == "0")
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumnsDetial]  \r\n" +
                                              " set [Order] = '" + orderNext + "' \r\n" +
                                               "where [Order] = '" + orderCurr + "' and ColDescription = '" + DescripCurr + "'  \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement +
                                              "  update [dbo].[tbl_PerformanceCrewColumnsDetial]  \r\n" +
                                              " set [Order] = '" + orderCurr + "' \r\n" +
                                               "where [Order] = '" + orderNext + "' and ColDescription = '" + DescripNext + "' ";
                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ExecuteInstruction();
                }

                if (StopeOrDev == "1")
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumnsDevDetial]  \r\n" +
                                              " set [Order] = '" + orderNext + "' \r\n" +
                                               "where [Order] = '" + orderCurr + "' and ColDescription = '" + DescripCurr + "'  \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement +
                                              "  update [dbo].[tbl_PerformanceCrewColumnsDevDetial]  \r\n" +
                                              " set [Order] = '" + orderCurr + "' \r\n" +
                                               "where [Order] = '" + orderNext + "' and ColDescription = '" + DescripNext + "' ";
                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ExecuteInstruction();
                }

                loadGrid();
                gridView7.Focus();
                gridView7.FocusedRowHandle = lastrow;
                LoadGraph();
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (gridView7.FocusedRowHandle + 1 < gridView7.RowCount)
            {
                string orderCurr = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["Order"]).ToString();
                string DescripCurr = gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["ColDescription"]).ToString();


                string orderNext = gridView7.GetRowCellValue(gridView7.FocusedRowHandle + 1, gridView7.Columns["Order"]).ToString();
                string DescripNext = gridView7.GetRowCellValue(gridView7.FocusedRowHandle + 1, gridView7.Columns["ColDescription"]).ToString();

                int lastrow = gridView7.FocusedRowHandle + 1;

                if (StopeOrDev == "0")
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumnsDetial]  \r\n" +
                                              " set [Order] = '" + orderNext + "' \r\n" +
                                               "where [Order] = '" + orderCurr + "' and ColDescription = '" + DescripCurr + "'  \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement +
                                              "  update [dbo].[tbl_PerformanceCrewColumnsDetial]  \r\n" +
                                              " set [Order] = '" + orderCurr + "' \r\n" +
                                               "where [Order] = '" + orderNext + "' and ColDescription = '" + DescripNext + "' ";
                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ExecuteInstruction();
                }

                if (StopeOrDev == "1")
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumnsDevDetial]  \r\n" +
                                              " set [Order] = '" + orderNext + "' \r\n" +
                                               "where [Order] = '" + orderCurr + "' and ColDescription = '" + DescripCurr + "'  \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement +
                                              "  update [dbo].[tbl_PerformanceCrewColumnsDevDetial]  \r\n" +
                                              " set [Order] = '" + orderCurr + "' \r\n" +
                                               "where [Order] = '" + orderNext + "' and ColDescription = '" + DescripNext + "' ";
                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ExecuteInstruction();
                }

                loadGrid();
                gridView7.Focus();
                gridView7.FocusedRowHandle = lastrow;
                LoadGraph();
            }
        }

        private void sbtnSave_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "Select * from tbl_Performances_Comments where HeadorText = '0'  and  DateEnt = SUBSTRING(CONVERT(varchar(40) ,getdate()),0,12) ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtTest = _dbMan.ResultsDataTable;



            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
            _dbManMonth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);

            if (dtTest.Rows.Count == 0)
            {
                _dbManMonth.SqlStatement = "insert into tbl_Performances_Comments (UserID,DateEnt,Comments,Crew,HeadorText) \r\n" +
                                           "values ('" + TUserInfo.UserID + "', SUBSTRING(CONVERT(varchar(40) ,getdate()),0,12) , SUBSTRING(CONVERT(varchar(40) ,getdate()),0,12) + '       " + TUserInfo.UserID + "'    ,'" + lblCrew.Text + "','0')  \r\n\r\n ";
            }

            _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "insert into tbl_Performances_Comments (UserID,DateEnt,Comments,Crew,HeadorText) \r\n" +
                                       "values ('" + TUserInfo.UserID + "',SUBSTRING(CONVERT(varchar(40) ,getdate()),0,12),'" + Notetxt.Text + "' ,'" + lblCrew.Text + "','1')  ";

            _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMonth.ExecuteInstruction();

            LoadComments();

            Notetxt.Text = string.Empty;
            Notetxt.Focus();
        }
    }
}