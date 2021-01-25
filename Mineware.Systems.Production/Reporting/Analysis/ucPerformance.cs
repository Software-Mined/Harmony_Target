using DevExpress.XtraCharts.Native;
using DevExpress.XtraEditors.Controls;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace Mineware.Systems.Production.Reporting.Analysis
{
    public partial class ucPerformance : BaseUserControl
    {
        public ucPerformance()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpCrewAnalysis);
            FormActiveRibbonPage = rpCrewAnalysis;
            FormMainRibbonPage = rpCrewAnalysis;
            RibbonControl = rcCrewAnalysis;
        }


        #region Private Variables
        OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
        DataTable dtRawDataBackground = new DataTable();
        DataTable TableGraph = new DataTable();
        Report theReport = new Report();

        int avga = 0;
        int avg1 = 0;
        int avg1aa = 0;
        int avg1bb = 0;
        int avg1cc = 0;

        string FirstLoadDone = "N";
        #endregion

        #region methods
        public string ExtractBeforeColon(string TheString)
        {
            if (TheString != string.Empty)
            {
                string BeforeColon;

                int index = TheString.IndexOf(":");

                BeforeColon = TheString.Substring(0, index);

                return BeforeColon;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        private void ucPerformance_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages[0].Text = "   Graph    ";
            tabControl1.TabPages[1].Text = "  Raw Data  ";

            tabControl2.TabPages[0].Text = "   Summary   ";
            tabControl2.TabPages[1].Text = "   Detail    ";


            tabControl1.TabPages.Remove(tabControl1.TabPages[1]);

            //navBarControl1.OptionsNavPane.NavPaneState = NavPaneState.Collapsed;

            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
            _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManMonth.SqlStatement = " select Pm1, prodmonth from  [tbl_PerformanceCrew]  group by Pm1, prodmonth order by prodmonth desc ";
            _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMonth.ResultsTableName = "Months";  //get table name
            _dbManMonth.ExecuteInstruction();

            DataTable Neil = _dbManMonth.ResultsDataTable;

            foreach (DataRow dr in Neil.Rows)
            {
                cbxlistboxPM.Items.Add(dr["Pm1"].ToString());
            }


            

            cbxlistboxPM.Items[0].CheckState = CheckState.Checked;
            label1.Text = cbxlistboxPM.Items[0].Value.ToString();
            lblStringDate.Text = cbxlistboxPM.Items[0].Value.ToString();

            RgbStopeOrDev.SelectedIndex = 0; 
            RgbSumOrAvg.SelectedIndex = 0;
            rgbXaxisFilter.SelectedIndex = 0;
            rgbXaxisFilter.EditValue = "Crew";
            RgbStopeOrDev.EditValue = "Stoping"; 
            loadGridFirstOff();
            LoadPerformance();


        }


        private void loadGridFirstOff()
        {
            string order = string.Empty;
            string DeScription = string.Empty;
            string Axis = string.Empty;
            string color = string.Empty;
            string Show = string.Empty;
            string Style = string.Empty;
            string ShowLbl = string.Empty;


            MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
            _dbManData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManData.SqlStatement = " select * from [dbo].[tbl_PerformanceCrewColumns_Default] where useyn = 'Y' " +
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



            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
            _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManMonth.SqlStatement = "  ";
            for (int i = 0; i < gridView7.RowCount; i++)
            {

                order = gridView7.GetRowCellValue(i, gridView7.Columns["Order"]).ToString();
                DeScription = gridView7.GetRowCellValue(i, gridView7.Columns["ColDescription"]).ToString();
                Axis = gridView7.GetRowCellValue(i, gridView7.Columns["YAxis"]).ToString();
                color = gridView7.GetRowCellValue(i, gridView7.Columns["ColColour"]).ToString();
                Show = gridView7.GetRowCellValue(i, gridView7.Columns["Show"]).ToString();
                Style = gridView7.GetRowCellValue(i, gridView7.Columns["ChartStyle"]).ToString();
                ShowLbl = gridView7.GetRowCellValue(i, gridView7.Columns["ShowLbl"]).ToString();

                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "update [dbo].[tbl_PerformanceCrewColumns]  \r\n" +
                                        " set ColColour = '" + color + "' ,  Show = '" + Show + "' , YAxis = '" + Axis + "', ChartStyle = '" + Style + "' ,ShowLbl = '" + ShowLbl + "',[Order] = '" + order + "'  \r\n" +
                                        "where ColDescription = '" + DeScription + "'  \r\n\r\n";
            }

            _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMonth.ExecuteInstruction();



        }
        private void LoadPerformance()
        {
            TableGraph.Clear();

            var ids = (from CheckedListBoxItem item in cbxlistboxPM.Items
                       where item.CheckState == CheckState.Checked
                       select (string)item.Value).ToArray();


            string xAxisFilter = string.Empty;

            if (rgbXaxisFilter.SelectedIndex == 0)
                xAxisFilter = "crew";
            if (rgbXaxisFilter.SelectedIndex == 1)
                xAxisFilter = "minname";
            if (rgbXaxisFilter.SelectedIndex == 2)
                xAxisFilter = "sbname";
            if (rgbXaxisFilter.SelectedIndex == 3)
                xAxisFilter = "moname";

            if (FirstLoadDone == "N")
            {
                return;
            }


            if (RgbStopeOrDev.SelectedIndex == 0)
            {
                //Sum
                if (RgbSumOrAvg.SelectedIndex == 0)
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManMonth.SqlStatement = " select b." + xAxisFilter + " crew , case when sum(PlanFL) = 0.0 then 0.0 else convert(decimal(10,1) ,(sum([PlanSqm])/sum(PlanFL))) end as PlanAdv     \r\n" +

                                              ",sum([PlanFL])[PlanFL],sum([PlanSqm]) PlanSqm,sum([BookSqm])[BookSqm]  \r\n" +
                                              ",sum([MeasFL])[MeasFL],sum([MeasSqm])[MeasSqm],sum([MeasAdv])[MeasAdv]  \r\n" +
                                              ",sum([MeasGrams])[MeasGrams],sum([PlanBlast])[PlanBlast],sum([ActBlast])[ActBlast]  \r\n" +
                                              ",sum([BlastComp])[BlastComp],sum([BookComp])[BookComp],avg(convert(decimal(8,0), [OutputComp]) ) [OutputComp]  \r\n" +
                                              ",avg(convert(decimal(8,0), [RiskRating] )) [RiskRating],sum([PTO])  [PTO],sum([PInsp])  [PInsp]  \r\n" +
                                              ",sum([OActions]) [OActions],sum([PlanLabour]) [PlanLabour],sum([ActLabour]) [ActLabour]  \r\n" +
                                              ",sum([LostBlasts]) [LostBlasts],'' [GangNo] from [dbo].[tbl_PerformanceCrew] a  \r\n" +
                                              "left outer join \r\n" +
                                              "vw_PerformanceCrew_Section b on a.crew = b.crew and a.prodmonth = b.prodmonth \r\n" +
                                              "where a.pm1 in( \r\n";
                    foreach (string i in ids)
                    {
                        _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "'" + i + "', ";
                    }
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " '' )  ";

                    if (MOlabel.Text != "<< Total Mine >>")
                        _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " and monid = '" + ExtractBeforeColon(MOlabel.Text) + "' \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "group by b." + xAxisFilter + "  ";
                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ResultsTableName = "aa";  //get table name
                    _dbManMonth.ExecuteInstruction();

                    TableGraph = _dbManMonth.ResultsDataTable;
                }

                //Avg
                if (RgbSumOrAvg.SelectedIndex == 1)
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManMonth.SqlStatement = "Select distinct  crew,convert(decimal(10,1),sum(PlanAdv)) PlanAdv,convert(decimal(10,0),avg(PlanFL)) PlanFL,convert(decimal(10,0),avg(PlanSqm)) PlanSqm,convert(decimal(10,0),avg(BookSqm)) BookSqm,convert(decimal(10,0),avg(MeasFL)) MeasFL,convert(decimal(10,0),avg(MeasSqm)) MeasSqm \r\n" +
                                               " ,convert(decimal(10,1),avg(MeasAdv)) MeasAdv,convert(decimal(10,0),avg(MeasGrams)) MeasGrams,convert(decimal(10,0),avg(PlanBlast)) PlanBlast,convert(decimal(10,0),avg(ActBlast)) ActBlast,convert(decimal(10,0),avg(BlastComp)) BlastComp,convert(decimal(10,0),avg(BookComp)) BookComp  \r\n" +
                                               " ,convert(decimal(10,0),avg(OutputComp)) OutputComp,convert(decimal(10,0),avg(RiskRating)) RiskRating,convert(decimal(10,0),avg(PTO)) PTO,convert(decimal(10,0),avg(PInsp)) PInsp,convert(decimal(10,0),avg(OActions)) OActions,convert(decimal(10,0),avg(PlanLabour)) PlanLabour    \r\n" +
                                               " ,convert(decimal(10,0),avg(ActLabour)) ActLabour,convert(decimal(10,0),avg(LostBlasts)) LostBlasts  " +
                                               "   from ( \r\n  ";
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " Select distinct b." + xAxisFilter + " crew, case when sum(PlanFL) = 0.0 then 0.0 else convert(decimal(10,1) ,(sum([PlanSqm])/sum(PlanFL))) end as  PlanAdv ,convert(decimal(10,0),sum(PlanFL)) PlanFL,convert(decimal(10,0),sum(PlanSqm)) PlanSqm,convert(decimal(10,0),sum(BookSqm)) BookSqm,convert(decimal(10,0),sum(MeasFL)) MeasFL,convert(decimal(10,0),sum(MeasSqm)) MeasSqm \r\n" +
                                               "  ,convert(decimal(10,1),sum(MeasAdv)) MeasAdv,convert(decimal(10,0),sum(MeasGrams)) MeasGrams,convert(decimal(10,0),sum(PlanBlast)) PlanBlast,convert(decimal(10,0),sum(ActBlast)) ActBlast,convert(decimal(10,0),avg(BlastComp)) BlastComp,convert(decimal(10,0),sum(BookComp)) BookComp  \r\n" +
                                               "  ,convert(decimal(10,0),avg(OutputComp)) OutputComp,convert(decimal(10,0),avg(convert(decimal(10,0),RiskRating) )) RiskRating,convert(decimal(10,0),sum(PTO)) PTO,convert(decimal(10,0),sum(PInsp)) PInsp,convert(decimal(10,0),sum(OActions)) OActions,convert(decimal(10,0),sum(PlanLabour)) PlanLabour    \r\n" +
                                               "  ,convert(decimal(10,0),sum(ActLabour)) ActLabour,convert(decimal(10,0),sum(LostBlasts)) LostBlasts   \r\n" +
                                               "  from  [tbl_PerformanceCrew] a   \r\n" +
                                               "  left outer join    \r\n" +
                                               " vw_PerformanceCrew_Section b on a.crew = b.crew and a.prodmonth = b.prodmonth   where a.Pm1 in (  \r\n";
                    foreach (string i in ids)
                    {
                        _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "'" + i + "', ";
                    }
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " '' )  \r\n";

                    if (MOlabel.Text != "<< Total Mine >>")
                        _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " and monid = '" + ExtractBeforeColon(MOlabel.Text) + "' \r\n";


                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "group by a.prodmonth, b." + xAxisFilter + " ) a group by crew ";

                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ResultsTableName = "aa";  //get table name
                    _dbManMonth.ExecuteInstruction();

                    TableGraph = _dbManMonth.ResultsDataTable;
                }


                LoadGraph();
            }

            //dev
            if (RgbStopeOrDev.SelectedIndex == 1)
            {
                //Sum
                if (RgbSumOrAvg.SelectedIndex == 0)
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManMonth.SqlStatement = " Select distinct b." + xAxisFilter + " crew,sum(PlanAdv) PlanAdv,sum(BookAdv) BookAdv  \r\n" +
                                               ",sum(MeasAdv) MeasAdv,sum(PlanBlast) PlanBlast,sum(ActBlast) ActBlast,sum(BlastComp) BlastComp,sum(BookComp) BookComp  \r\n" +
                                               ",sum(OutputComp) OutputComp,sum(convert(decimal(8,0),RiskRating) ) RiskRating,sum(PTO) PTO,sum(PInsp) PInsp,sum(OActions) OActions,sum(PlanLabour) PlanLabour   \r\n" +
                                               ",sum(ActLabour) ActLabour,sum(LostBlasts) LostBlasts   \r\n" +
                                               "from  [tbl_PerformanceCrewDev] a   \r\n" +
                                               "left outer join  \r\n" +
                                               "vw_PerformanceCrew_Section b on a.crew = b.crew and a.prodmonth = b.prodmonth   where a.Pm1 in (\r\n";
                    foreach (string i in ids)
                    {
                        _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "'" + i + "', ";
                    }
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " '' ) \r\n";

                    if (MOlabel.Text != "<< Total Mine >>")
                        _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " and monid = '" + ExtractBeforeColon(MOlabel.Text) + "' \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " group by b." + xAxisFilter + " ";
                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ResultsTableName = "Dev";  //get table name
                    _dbManMonth.ExecuteInstruction();

                    TableGraph = _dbManMonth.ResultsDataTable;
                }

                //Avg
                if (RgbSumOrAvg.SelectedIndex == 1)
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManMonth.SqlStatement = "  Select distinct crew,convert(decimal(10,0),avg(BookAdv)) BookAdv,convert(decimal(10,0),avg(PlanAdv)) PlanAdv   \r\n" +
                                            ",convert(decimal(10,0),avg(MeasAdv)) MeasAdv,convert(decimal(10,0),avg(PlanBlast)) PlanBlast,convert(decimal(10,0),avg(ActBlast)) ActBlast,convert(decimal(10,0),avg(BlastComp)) BlastComp,convert(decimal(10,0),avg(BookComp)) BookComp  \r\n" +
                                            ",convert(decimal(10,0),avg(OutputComp)) OutputComp,convert(decimal(10,0),avg(RiskRating)) RiskRating,convert(decimal(10,0),avg(PTO)) PTO,convert(decimal(10,0),avg(PInsp)) PInsp,convert(decimal(10,0),avg(OActions)) OActions,convert(decimal(10,0),avg(PlanLabour)) PlanLabour  \r\n" +
                                            ",convert(decimal(10,0),avg(ActLabour)) ActLabour,convert(decimal(10,0),avg(LostBlasts)) LostBlasts  from ( \r\n";
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "  Select distinct b." + xAxisFilter + " crew,convert(decimal(10,0),sum(PlanAdv)) PlanAdv,convert(decimal(10,0),sum(BookAdv)) BookAdv  \r\n" +
                                                ",convert(decimal(10,0),sum(MeasAdv)) MeasAdv,convert(decimal(10,0),sum(PlanBlast)) PlanBlast,convert(decimal(10,0),sum(ActBlast)) ActBlast,convert(decimal(10,0),sum(BlastComp)) BlastComp,convert(decimal(10,0),sum(BookComp)) BookComp  \r\n" +
                                                ",convert(decimal(10,0),avg(OutputComp)) OutputComp,convert(decimal(10,0),avg(convert(decimal(10,0),RiskRating) )) RiskRating,convert(decimal(10,0),sum(PTO)) PTO,convert(decimal(10,0),sum(PInsp)) PInsp,convert(decimal(10,0),sum(OActions)) OActions,convert(decimal(10,0),sum(PlanLabour)) PlanLabour   \r\n" +
                                                ",convert(decimal(10,0),sum(ActLabour)) ActLabour,convert(decimal(10,0),sum(LostBlasts)) LostBlasts    \r\n" +
                                                " from  [tbl_PerformanceCrewDev] a  left outer join   \r\n" +
                                                " vw_PerformanceCrew_Section b on a.crew = b.crew and a.prodmonth = b.prodmonth   where a.Pm1 in (   \r\n";
                    foreach (string i in ids)
                    {
                        _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "'" + i + "', ";
                    }
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " '' )  \r\n";

                    if (MOlabel.Text != "<< Total Mine >>")
                        _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " and monid = '" + ExtractBeforeColon(MOlabel.Text) + "' \r\n";


                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " group by a.prodmonth,b." + xAxisFilter + " ) a  group by crew  ";
                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ResultsTableName = "Dev";  //get table name
                    _dbManMonth.ExecuteInstruction();

                    TableGraph = _dbManMonth.ResultsDataTable;
                }

                LoadGraph();
            }

            lblMaxlvl.Text = TableGraph.Rows.Count.ToString();

            lblStringDate.Text = cbxlistboxPM.Items[0].Value.ToString();

            int count = 0;

            foreach (string i in ids)
            {
                count = count + 1;

                if (count != 1)
                {
                    lblStringDate.Text = lblStringDate.Text + ", " + i;
                }
            }

            int Miltiplier = Convert.ToInt32(TableGraph.Rows.Count / 5);

            spinEditHigh2.Value = Miltiplier * 4;
            spinEditLow2.Value = Miltiplier * 3;
            spinEditHigh1.Value = Miltiplier * 2;
            spinEditLow1.Value = Miltiplier;


            //gcRawData.DataSource = "";
            //gcRawData.DataSource = TableGraph;

            //colMeasSqm.FieldName = "MeasSqm";
            //colMeasFL.FieldName = "MeasFL";
            //colMeasAdv.FieldName = "MeasAdv";
            //colMeasGrams.FieldName = "MeasGrams";
            //colPlanFL.FieldName = "PlanFL";
            //colPlanSqm.FieldName = "PlanSqm";
            //colPlanBlast.FieldName = "PlanBlast";
            //colBookSqm.FieldName = "BookSqm";
            //colActBlast.FieldName = "ActBlast";
            //colBlastComp.FieldName = "BlastComp";
            //colBookComp.FieldName = "BookComp";
            //colOutputComp.FieldName = "OutputComp";
            //colRiskRating.FieldName = "RiskRating";
            //colBookAdv.FieldName = "BookAdv";
            //colPInsp.FieldName = "PInsp";
            //colBookFL.FieldName = "BookFL";
            //colPlanLabour.FieldName = "PlanLabour";
            //colActLabour.FieldName = "ActLabour";
            //colLostBlasts.FieldName = "LostBlasts";
            //colPlanAdv.FieldName = "PlanAdv";

            //colFilter.FieldName = "crew";


            //if (RgbStopeOrDev.SelectedIndex == 1)
            //{
            //    colMeasSqm.Visible = true;
            //    colMeasFL.Visible = true;
            //    colMeasAdv.Visible = true;
            //    colPlanFL.Visible = true;
            //    colPlanSqm.Visible = true;
            //    colPlanBlast.Visible = true;
            //    colBookSqm.Visible = true;
            //    colActBlast.Visible = true;
            //    colBookAdv.Visible = true;
            //    colBookFL.Visible = true;
            //    colLostBlasts.Visible = true;
            //    colPlanAdv.Visible = true;

            //    colMeasSqm.Visible = false;
            //    colMeasFL.Visible = false;
            //    colMeasGrams.Visible = false;
            //    colPlanFL.Visible = false;
            //    colPlanSqm.Visible = false;
            //    colBookSqm.Visible = false;
            //    colPInsp.Visible = false;
            //    colBookFL.Visible = false;
            //}
            //else
            //{
            //    colMeasSqm.Visible = true;
            //    colMeasFL.Visible = true;
            //    colMeasAdv.Visible = true;
            //    colPlanFL.Visible = true;
            //    colPlanSqm.Visible = true;
            //    colPlanBlast.Visible = true;
            //    colBookSqm.Visible = true;
            //    colActBlast.Visible = true;
            //    colBookAdv.Visible = true;
            //    colPInsp.Visible = false;
            //    colBookFL.Visible = true;
            //    colLostBlasts.Visible = true;
            //    colPlanAdv.Visible = true;
            //}

            //colPlanLabour.Visible = false;
            //colActLabour.Visible = false;
            //colBlastComp.Visible = false;
            //colBookComp.Visible = false;
            //colOutputComp.Visible = false;
            //colRiskRating.Visible = false;
            //colMeasGrams.Visible = false;

            //if (!bwkrRawData.IsBusy)
            //{
            //    bwkrRawData.RunWorkerAsync();
            //}
        }
        private void LoadGraph()
        {
            // STEP 1 Sort data           

            TableGraph.DefaultView.Sort = gridView7.GetRowCellValue(0, gridView7.Columns["ColDescription"]).ToString();
            TableGraph = TableGraph.DefaultView.ToTable();

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

            if (RgbStopeOrDev.SelectedIndex == 0)
            {
                //Chart.Series[12].IsVisibleInLegend = false;
                //Chart.Series[13].IsVisibleInLegend = false;
                //Chart.Series[14].IsVisibleInLegend = false;
                Chart.Series[15].IsVisibleInLegend = false;
                Chart.Series[16].IsVisibleInLegend = false;
                Chart.Series[17].IsVisibleInLegend = false;
                Chart.Series[18].IsVisibleInLegend = false;
                Chart.Series[19].IsVisibleInLegend = false;
            }

            if (RgbStopeOrDev.SelectedIndex == 1)
            {
                //Chart.Series[11].IsVisibleInLegend = false;
                //Chart.Series[12].IsVisibleInLegend = false;
                //Chart.Series[13].IsVisibleInLegend = false;
                Chart.Series[14].IsVisibleInLegend = false;
            }




            for (int i = 0; i < gridView7.RowCount; i++)
            {
                Chart.Series[i].Name = gridView7.GetRowCellValue(i, gridView7.Columns["ColDescription"]).ToString();
            }

            int count = 0;

            avga = 0;
            avg1 = 0;
            avg1aa = 0;
            avg1bb = 0;
            avg1cc = 0;


            if (RgbStopeOrDev.SelectedIndex == 0)
            {
                foreach (DataRow dr in TableGraph.Rows)
                {
                    Chart.Series[0].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[0].Name].ToString());
                    Chart.Series[1].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[1].Name].ToString());
                    Chart.Series[2].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[2].Name].ToString());
                    Chart.Series[3].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[3].Name].ToString());
                    Chart.Series[4].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[4].Name].ToString());
                    Chart.Series[5].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[5].Name].ToString());
                    Chart.Series[6].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[6].Name].ToString());
                    Chart.Series[7].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[7].Name].ToString());
                    Chart.Series[8].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[8].Name].ToString());
                    Chart.Series[9].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[9].Name].ToString());
                    Chart.Series[10].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[10].Name].ToString());
                    Chart.Series[11].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[11].Name].ToString());
                    Chart.Series[12].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[12].Name].ToString());
                    Chart.Series[13].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[13].Name].ToString());
                    Chart.Series[14].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[14].Name].ToString());
                    Chart.Series[15].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[15].Name].ToString());
                    Chart.Series[16].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[16].Name].ToString());
                    Chart.Series[17].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[17].Name].ToString());
                    Chart.Series[18].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[18].Name].ToString());
                    //Chart.Series[19].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[19].Name].ToString());

                    if (count >= 0 && count < Convert.ToInt32(Lowlevel1txt.Text))
                    {
                        avga = avga + Convert.ToInt32(dr[Chart.Series[0].Name]);
                    }

                    if (count >= Convert.ToInt32(Lowlevel1txt.Text) && count < Convert.ToInt32(HighLevel1txt.Text))
                    {
                        avg1 = avg1 + Convert.ToInt32(dr[Chart.Series[0].Name]);
                    }

                    if (count >= Convert.ToInt32(HighLevel1txt.Text) && count < Convert.ToInt32(LowLevel2txt.Text))
                    {
                        avg1aa = avg1aa + Convert.ToInt32(dr[Chart.Series[0].Name]);
                    }

                    if (count >= Convert.ToInt32(LowLevel2txt.Text) && count < Convert.ToInt32(HighLevel2txt.Text))
                    {
                        avg1bb = avg1bb + Convert.ToInt32(dr[Chart.Series[0].Name]);
                    }

                    if (count >= Convert.ToInt32(HighLevel2txt.Text))
                    {
                        avg1cc = avg1cc + Convert.ToInt32(dr[Chart.Series[0].Name]);
                    }


                    count = count + 1;
                }
            }


            if (RgbStopeOrDev.SelectedIndex == 1)
            {
                foreach (DataRow dr in TableGraph.Rows)
                {
                    Chart.Series[0].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[0].Name].ToString());
                    Chart.Series[1].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[1].Name].ToString());
                    Chart.Series[2].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[2].Name].ToString());
                    Chart.Series[3].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[3].Name].ToString());
                    Chart.Series[4].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[4].Name].ToString());
                    Chart.Series[5].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[5].Name].ToString());
                    Chart.Series[6].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[6].Name].ToString());
                    Chart.Series[7].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[7].Name].ToString());
                    Chart.Series[8].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[8].Name].ToString());
                    Chart.Series[9].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[9].Name].ToString());
                    Chart.Series[10].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[10].Name].ToString());
                    Chart.Series[11].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[11].Name].ToString());
                    Chart.Series[12].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[12].Name].ToString());
                    Chart.Series[13].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[13].Name].ToString());
                    //Chart.Series[14].Points.AddXY(dr["Crew"].ToString(), dr[Chart.Series[14].Name].ToString());

                    if (count >= 0 && count < Convert.ToInt32(Lowlevel1txt.Text))
                    {
                        avga = avga + Convert.ToInt32(dr[Chart.Series[0].Name]);
                    }

                    if (count >= Convert.ToInt32(Lowlevel1txt.Text) && count < Convert.ToInt32(HighLevel1txt.Text))
                    {
                        avg1 = avg1 + Convert.ToInt32(dr[Chart.Series[0].Name]);
                    }

                    if (count >= Convert.ToInt32(HighLevel1txt.Text) && count < Convert.ToInt32(LowLevel2txt.Text))
                    {
                        avg1aa = avg1aa + Convert.ToInt32(dr[Chart.Series[0].Name]);
                    }

                    if (count >= Convert.ToInt32(LowLevel2txt.Text) && count < Convert.ToInt32(HighLevel2txt.Text))
                    {
                        avg1bb = avg1bb + Convert.ToInt32(dr[Chart.Series[0].Name]);
                    }

                    if (count >= Convert.ToInt32(HighLevel2txt.Text))
                    {
                        avg1cc = avg1cc + Convert.ToInt32(dr[Chart.Series[0].Name]);

                    }


                    count = count + 1;
                }
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


            cbxShowAvg.Text = "Avg " + Chart.Series[0].Name.ToString();

            loadAvgLines();
        }

        private void LoadSections()
        {

            var ids = (from CheckedListBoxItem item in cbxlistboxPM.Items
                       where item.CheckState == CheckState.Checked
                       select (string)item.Value).ToArray();


            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
            _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);


            _dbManMonth.SqlStatement = "select mo+':'+mname mo from (select distinct(monid) mo \r\n";
            _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " from [dbo].[vw_PerformanceCrew_Section] \r\n";
            _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " where pm1 in ( \r\n";
            foreach (string i in ids)
            {
                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "'" + i + "', ";
            }

            _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " '' )) a ,  \r\n";
            _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "( select a.monid mm, a.moname mname from [dbo].[vw_PerformanceCrew_Section] a, \r\n";
            _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "(select monid mm, max(prodmonth) pmm from [dbo].[vw_PerformanceCrew_Section] \r\n";
            _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "group by monid) b where a.monid = b.mm and a.prodmonth = b.pmm group by a.monid, a.moname) b \r\n";
            _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "where a.mo = b.mm \r\n";
            _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "order by mo    ";


            _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMonth.ExecuteInstruction();


            DataTable dtSections = _dbManMonth.ResultsDataTable;

            MOcomboBox.Items.Clear();

            MOcomboBox.Items.Add("<< Total Mine >>");

            foreach (DataRow dr in dtSections.Rows)
            {
                MOcomboBox.Items.Add(dr["mo"].ToString());
            }

            //MOcomboBox.SelectedIndex = 0;

            MOcomboBox.Text = MOlabel.Text;


            if (MOcomboBox.SelectedIndex == -1)
                MOcomboBox.SelectedIndex = 0;



        }

        private void loadAvgLines()
        {
            int count1 = 0;

            int Range0 = 0;
            int Range1 = 0;
            int Range2 = 0;
            int Range3 = 0;
            int Range4 = 0;

            Chart.Series[20].Points.Clear();
            Chart.Series[21].Points.Clear();
            Chart.Series[22].Points.Clear();
            Chart.Series[23].Points.Clear();
            Chart.Series[24].Points.Clear();



            Range0 = Convert.ToInt32(Lowlevel1txt.Text) - 0;
            Range1 = Convert.ToInt32(HighLevel1txt.Text) - Convert.ToInt32(Lowlevel1txt.Text);
            Range2 = Convert.ToInt32(LowLevel2txt.Text) - Convert.ToInt32(HighLevel1txt.Text);
            Range3 = Convert.ToInt32(HighLevel2txt.Text) - Convert.ToInt32(LowLevel2txt.Text);
            Range4 = TableGraph.Rows.Count - Convert.ToInt32(HighLevel2txt.Text);



            foreach (DataRow dr in TableGraph.Rows)
            {
                //string aa = dr["Crew"].ToString();

                if (count1 > 0 && count1 <= Convert.ToInt32(Lowlevel1txt.Text))
                {
                    Chart.Series[20].Points.AddXY(dr["Crew"].ToString(), avga / Range0);
                    Chart.Series[21].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                    Chart.Series[22].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                    Chart.Series[23].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                    Chart.Series[24].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                }

                if (count1 > Convert.ToInt32(Lowlevel1txt.Text) && count1 <= Convert.ToInt32(HighLevel1txt.Text))
                {
                    Chart.Series[21].Points.AddXY(dr["Crew"].ToString(), avg1 / Range1);
                    Chart.Series[22].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                    Chart.Series[23].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                    Chart.Series[24].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                }

                if (count1 > Convert.ToInt32(HighLevel1txt.Text) && count1 <= Convert.ToInt32(LowLevel2txt.Text))
                {
                    Chart.Series[22].Points.AddXY(dr["Crew"].ToString(), avg1aa / Range2);
                    Chart.Series[23].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                    Chart.Series[24].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                }

                if (count1 > Convert.ToInt32(LowLevel2txt.Text) && count1 <= Convert.ToInt32(HighLevel2txt.Text))
                {
                    Chart.Series[23].Points.AddXY(dr["Crew"].ToString(), avg1bb / Range3);
                    Chart.Series[24].Points.AddXY(dr["Crew"].ToString(), DBNull.Value);
                }


                if (count1 >= Convert.ToInt32(HighLevel2txt.Text))
                {
                    Chart.Series[24].Points.AddXY(dr["Crew"].ToString(), avg1cc / Range4);
                }


                count1 = count1 + 1;
            }

            if (gridView7.GetRowCellValue(0, gridView7.Columns["YAxis"]).ToString() == "Prim")
            {
                Chart.Series[20].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
                Chart.Series[21].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
                Chart.Series[22].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
                Chart.Series[23].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
                Chart.Series[24].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
            }

            if (gridView7.GetRowCellValue(0, gridView7.Columns["YAxis"]).ToString() == "Sec")
            {
                Chart.Series[20].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                Chart.Series[21].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                Chart.Series[22].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                Chart.Series[23].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                Chart.Series[24].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            }



            if (cbxShowAvg.Checked == false)
            {
                Chart.Series[20].Points.Clear();
                Chart.Series[21].Points.Clear();
                Chart.Series[22].Points.Clear();
                Chart.Series[23].Points.Clear();
                Chart.Series[24].Points.Clear();
                Chart.Series[20].IsVisibleInLegend = false;
            }
            else
            {
                Chart.Series[20].Name = cbxShowAvg.Text;
                Chart.Series[20].IsVisibleInLegend = true;
            }


        }

        private void bwkrRawData_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadRawdataBackground();
        }

        private void bwkrRawData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadRawdata();
        }

        private void LoadRawdataBackground()
        {
            MWDataManager.clsDataAccess _dbManRawDetial = new MWDataManager.clsDataAccess();
            _dbManRawDetial.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManRawDetial.SqlStatement = " select * from vw_PerformanceCrew_Rawdata  \r\n" +
                                           " order by  Typeaa ,pm1, minname, sbname, nn \r\n";

            _dbManRawDetial.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRawDetial.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRawDetial.ExecuteInstruction();

            dtRawDataBackground = _dbManRawDetial.ResultsDataTable;
        }

        private void LoadRawdata()
        {
            gcDetialRawData.DataSource = string.Empty;
            gcDetialRawData.DataSource = dtRawDataBackground;

            colDetType.FieldName = "typeaa";
            colDetProdmonth.FieldName = "pm1";
            colDetMoName.FieldName = "moname";
            colDetSbName.FieldName = "sbname";
            colDetMinName.FieldName = "minname";
            colDetCrew.FieldName = "nn";

            colDetMeasSqm.FieldName = "MeasSqm";
            colDetMeasFL.FieldName = "MeasFL";
            colDetMeasAdv.FieldName = "MeasAdv";
            colDetMeasGrams.FieldName = "MeasGrams";
            colDetPlanFL.FieldName = "planfl";
            colDetPlanSqm.FieldName = "plansqm";
            colDetPlanBlast.FieldName = "planblast";
            colDetBookSqm.FieldName = "BookSqm";
            colDetActBlast.FieldName = "blast";
            colDetBlastComp.FieldName = "blastcomp";
            colDetBookComp.FieldName = "bookcomp";
            colDetOutputComp.FieldName = "OutputCompl";
            colDetRiskRating.FieldName = "RiskRating";
            colDetBookAdv.FieldName = "BookADV";
            colDetPInsp.FieldName = "PInsp";
            colDetBookFL.FieldName = "BookFL";
            colDetPlanLabour.FieldName = "PlanLabour";
            colDetActLabour.FieldName = "ActLabour";
            colDetLostBlasts.FieldName = "Lostblasts";
            colDetPlanAdv.FieldName = "PlanAdv";

            colDetRiskRating.Visible = false;

            tabControl2.Enabled = true;
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



                if (RgbStopeOrDev.SelectedIndex == 0)
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumns]  \r\n" +
                                              " set [Order] = '" + orderNext + "' \r\n" +
                                               "where [Order] = '" + orderCurr + "' and ColDescription = '" + DescripCurr + "'  \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement +
                                              "  update [dbo].[tbl_PerformanceCrewColumns]  \r\n" +
                                              " set [Order] = '" + orderCurr + "' \r\n" +
                                               "where [Order] = '" + orderNext + "' and ColDescription = '" + DescripNext + "' ";
                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ExecuteInstruction();
                }



                if (RgbStopeOrDev.SelectedIndex == 1)
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumnsDev]  \r\n" +
                                              " set [Order] = '" + orderNext + "' \r\n" +
                                               "where [Order] = '" + orderCurr + "' and ColDescription = '" + DescripCurr + "'  \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement +
                                              "  update [dbo].[tbl_PerformanceCrewColumnsDev]  \r\n" +
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

        public void loadGrid()
        {
            if (RgbStopeOrDev.SelectedIndex == 0 || RgbStopeOrDev.SelectedIndex == -1)
            {

                MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
                _dbManData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManData.SqlStatement = " select * from [dbo].[tbl_PerformanceCrewColumns] where useyn = 'Y' " +
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

                colDescription.Width = 84;
            }


            if (RgbStopeOrDev.SelectedIndex == 1)
            {
                MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
                _dbManData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManData.SqlStatement = " select * from [dbo].[tbl_PerformanceCrewColumnsDev] where useyn = 'Y' " +
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

                colDescription.Width = 100;
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


                if (RgbStopeOrDev.SelectedIndex == 0)
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumns]  \r\n" +
                                              " set [Order] = '" + orderNext + "' \r\n" +
                                               "where [Order] = '" + orderCurr + "' and ColDescription = '" + DescripCurr + "'  \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement +
                                              "  update [dbo].[tbl_PerformanceCrewColumns]  \r\n" +
                                              " set [Order] = '" + orderCurr + "' \r\n" +
                                               "where [Order] = '" + orderNext + "' and ColDescription = '" + DescripNext + "' ";
                    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMonth.ExecuteInstruction();
                }


                if (RgbStopeOrDev.SelectedIndex == 1)
                {
                    MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                    _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumnsDev]  \r\n" +
                                              " set [Order] = '" + orderNext + "' \r\n" +
                                               "where [Order] = '" + orderCurr + "' and ColDescription = '" + DescripCurr + "'  \r\n";

                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement +
                                              "  update [dbo].[tbl_PerformanceCrewColumnsDev]  \r\n" +
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

        private void rgbXaxisFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FirstLoadDone = "Y";
            LoadPerformance();
        }

        private void RgbStopeOrDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadGrid();
            LoadPerformance();
        }

        private void RgbSumOrAvg_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPerformance();
        }

        private void cbxShowAvg_CheckedChanged(object sender, EventArgs e)
        {
            LoadGraph();
        }

        private void sbtnSave_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
            _dbManData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManData.SqlStatement = string.Empty;

            if (RgbStopeOrDev.SelectedIndex == 0)
            {
                for (int i = 0; i < gridView7.RowCount; i++)
                {

                    _dbManData.SqlStatement = _dbManData.SqlStatement + "update [dbo].[tbl_PerformanceCrewColumns_Default]  " +
                                          " Set [Order] = '" + gridView7.GetRowCellValue(i, gridView7.Columns["Order"]).ToString() + "' \r\n" +
                                          " , Show = '" + gridView7.GetRowCellValue(i, gridView7.Columns["Show"]).ToString() + "' \r\n" +
                                          " , YAxis = '" + gridView7.GetRowCellValue(i, gridView7.Columns["YAxis"]).ToString() + "' \r\n" +
                                          " , ColColour = '" + gridView7.GetRowCellValue(i, gridView7.Columns["ColColour"]).ToString() + "' \r\n" +

                                          " , ChartStyle = '" + gridView7.GetRowCellValue(i, gridView7.Columns["ChartStyle"]).ToString() + "' \r\n" +
                                          " , ShowLbl = '" + gridView7.GetRowCellValue(i, gridView7.Columns["ShowLbl"]).ToString() + "' \r\n" +
                                          " where ColDescription = '" + gridView7.GetRowCellValue(i, gridView7.Columns["ColDescription"]).ToString() + "' \r\n\r\n ";
                }

                _dbManData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManData.ResultsTableName = "tbl_Result";  //get table name
                _dbManData.ExecuteInstruction();
            }


            if (RgbStopeOrDev.SelectedIndex == 1)
            {
                for (int i = 0; i < gridView7.RowCount; i++)
                {
                    _dbManData.SqlStatement = _dbManData.SqlStatement + "update [dbo].[tbl_PerformanceCrewColumnsDev_Default]  " +
                                          " Set [Order] = '" + gridView7.GetRowCellValue(i, gridView7.Columns["Order"]).ToString() + "' \r\n" +
                                          " , Show = '" + gridView7.GetRowCellValue(i, gridView7.Columns["Show"]).ToString() + "' \r\n" +
                                          " , YAxis = '" + gridView7.GetRowCellValue(i, gridView7.Columns["YAxis"]).ToString() + "' \r\n" +
                                          " , ColColour = '" + gridView7.GetRowCellValue(i, gridView7.Columns["ColColour"]).ToString() + "' \r\n" +

                                          " , ChartStyle = '" + gridView7.GetRowCellValue(i, gridView7.Columns["ChartStyle"]).ToString() + "' \r\n" +
                                          " , ShowLbl = '" + gridView7.GetRowCellValue(i, gridView7.Columns["ShowLbl"]).ToString() + "' \r\n" +
                                          " where ColDescription = '" + gridView7.GetRowCellValue(i, gridView7.Columns["ColDescription"]).ToString() + "' \r\n\r\n ";
                }

                _dbManData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManData.ResultsTableName = "tbl_Result";  //get table name
                _dbManData.ExecuteInstruction();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (RgbStopeOrDev.SelectedIndex == 0)
            {

                var ids = (from CheckedListBoxItem item in cbxlistboxPM.Items
                           where item.CheckState == CheckState.Checked
                           select (string)item.Value).ToArray();


                string xAxisFilter = string.Empty;

                if (rgbXaxisFilter.SelectedIndex == 0)
                    xAxisFilter = "crew";
                if (rgbXaxisFilter.SelectedIndex == 1)
                    xAxisFilter = "minname";
                if (rgbXaxisFilter.SelectedIndex == 2)
                    xAxisFilter = "sbname";
                if (rgbXaxisFilter.SelectedIndex == 3)
                    xAxisFilter = "moname";



                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManMonth.SqlStatement = " ";
                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " select b.monid MO, b." + xAxisFilter + " crew , case when sum(PlanFL) = 0.0 then 0.0 else convert(decimal(10,1) ,(sum([PlanSqm])/sum(PlanFL))) end as PlanAdv     \r\n" +

                                          ",sum([PlanFL])[PlanFL],sum([PlanSqm]) PlanSqm,sum([BookSqm])[BookSqm]  \r\n" +
                                          ",sum([MeasFL])[MeasFL],sum([MeasSqm])[MeasSqm],sum([MeasAdv])[MeasAdv]  \r\n" +
                                          ",sum([MeasGrams])[MeasGrams],sum([PlanBlast])[PlanBlast],sum([ActBlast])[ActBlast]  \r\n" +
                                          ",sum([BlastComp])[BlastComp],sum([BookComp])[BookComp],avg([OutputComp]) [OutputComp]  \r\n" +
                                          ",max([RiskRating]) [RiskRating],sum([PTO])  BookAdv,sum([PInsp])  PInsp  \r\n" +
                                          ",sum([OActions])  BookFL,sum([PlanLabour]) [PlanLabour],sum([ActLabour]) [ActLabour]  \r\n" +
                                          ",sum([LostBlasts]) [LostBlasts],'' [GangNo] from [dbo].[tbl_PerformanceCrew] a  \r\n" +
                                          "left outer join \r\n" +
                                          "vw_PerformanceCrew_Section b on a.crew = b.crew and a.prodmonth = b.prodmonth \r\n" +
                                          "where a.pm1 in( \r\n";
                foreach (string i in ids)
                {
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "'" + i + "', ";
                }


                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " '' )  ";


                if (MOlabel.Text != "<< Total Mine >>")
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " and (monid = '" + ExtractBeforeColon(MOlabel.Text) + "' or sbid = '" + ExtractBeforeColon(MOlabel.Text) + "') \r\n";

                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "group by b." + xAxisFilter + ",b.monid  ";

                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "order by " + gridView7.GetRowCellValue(0, gridView7.Columns["ColDescription"]).ToString() + "   ";


                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ResultsTableName = "aa";  //get table name
                _dbManMonth.ExecuteInstruction();



                DataTable dt = _dbManMonth.ResultsDataTable;

                DataSet ds = new DataSet();

                ds.Tables.Add(dt);

                int dashIndex = label1.Text.IndexOf("-");

                string currentYear = label1.Text.Substring(dashIndex + 1, label1.Text.Length - (dashIndex + 1));

                MWDataManager.clsDataAccess _dbManColData = new MWDataManager.clsDataAccess();
                _dbManColData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManColData.SqlStatement = " Select *, '" + label1.Text + "' ll, '" + UserCurrentInfo.Connection + "' Mine   from [dbo].[tbl_PerformanceCrewColumns] ";

                _dbManColData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManColData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManColData.ResultsTableName = "ColumnData";  //get table name
                _dbManColData.ExecuteInstruction();

                DataTable dtColData = _dbManColData.ResultsDataTable;

                DataSet dscolData = new DataSet();

                dscolData.Tables.Add(dtColData);



                MWDataManager.clsDataAccess _dbManColCrew18 = new MWDataManager.clsDataAccess();
                _dbManColCrew18.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

                _dbManColCrew18.SqlStatement = " exec sp_PeformanceDetialPP '" + currentYear + "'     ";

                _dbManColCrew18.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManColCrew18.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManColCrew18.ResultsTableName = "Crew18";
                _dbManColCrew18.ExecuteInstruction();

                DataTable dtColCrew18 = _dbManColCrew18.ResultsDataTable;

                DataSet dscolCrew18 = new DataSet();

                dscolCrew18.Tables.Add(dtColCrew18);

                theReport.RegisterData(dscolCrew18);
                theReport.RegisterData(dscolData);
                theReport.RegisterData(ds);

                theReport.Load(TGlobalItems.ReportsFolder + "\\PerformanceCrewStp.frx");

                //theReport.Design();

                theReport.Prepare();
                theReport.ShowPrepared();
            }

            if (RgbStopeOrDev.SelectedIndex == 1)
            {
                var ids = (from CheckedListBoxItem item in cbxlistboxPM.Items
                           where item.CheckState == CheckState.Checked
                           select (string)item.Value).ToArray();


                string xAxisFilter = string.Empty;

                if (rgbXaxisFilter.SelectedIndex == 0)
                    xAxisFilter = "crew";
                if (rgbXaxisFilter.SelectedIndex == 1)
                    xAxisFilter = "minname";
                if (rgbXaxisFilter.SelectedIndex == 2)
                    xAxisFilter = "sbname";
                if (rgbXaxisFilter.SelectedIndex == 3)
                    xAxisFilter = "moname";

                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManMonth.SqlStatement = " ";
                _dbManMonth.SqlStatement = " Select distinct b." + xAxisFilter + " crew, b.monid MO,sum(PlanAdv) PlanAdv,sum(BookAdv) BookAdv  \r\n" +
                                               ",sum(MeasAdv) MeasAdv,sum(PlanBlast) PlanBlast,sum(ActBlast) ActBlast,sum(BlastComp) BlastComp,sum(BookComp) BookComp  \r\n" +
                                               ",sum(OutputComp) OutputComp,sum(convert(decimal(8,0), RiskRating) ) RiskRating,sum(PTO) PTO,sum(PInsp) PInsp,sum(OActions) OActions,sum(PlanLabour) PlanLabour   \r\n" +
                                               ",sum(ActLabour) ActLabour,sum(LostBlasts) LostBlasts   \r\n" +
                                               "from  [tbl_PerformanceCrewDev] a   \r\n" +
                                               "left outer join  \r\n" +
                                               "vw_PerformanceCrew_Section b on a.crew = b.crew and a.prodmonth = b.prodmonth   where a.Pm1 in (\r\n";
                foreach (string i in ids)
                {
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "'" + i + "', ";
                }


                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " '' )  ";


                if (MOlabel.Text != "<< Total Mine >>")
                    _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + " and (monid = '" + ExtractBeforeColon(MOlabel.Text) + "' or sbid = '" + ExtractBeforeColon(MOlabel.Text) + "') \r\n";



                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "group by b." + xAxisFilter + ",b.monid  ";
                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "order by " + gridView7.GetRowCellValue(0, gridView7.Columns["ColDescription"]).ToString() + "   ";




                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ResultsTableName = "aa";  //get table name
                _dbManMonth.ExecuteInstruction();

                DataTable dt = _dbManMonth.ResultsDataTable;

                DataSet ds = new DataSet();

                ds.Tables.Add(dt);


                MWDataManager.clsDataAccess _dbManColData = new MWDataManager.clsDataAccess();
                _dbManColData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManColData.SqlStatement = " Select *, '" + label1.Text + "' ll, '" + UserCurrentInfo.Connection + "' Mine from [dbo].[tbl_PerformanceCrewColumnsDev] ";

                _dbManColData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManColData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManColData.ResultsTableName = "ColumnData";  //get table name
                _dbManColData.ExecuteInstruction();

                DataTable dtColData = _dbManColData.ResultsDataTable;

                DataSet dscolData = new DataSet();

                dscolData.Tables.Add(dtColData);

                int aa = label1.Text.IndexOf("-");

                string currentYear = label1.Text.Substring(aa + 1, label1.Text.Length - (aa + 1));

                MWDataManager.clsDataAccess _dbManColCrew18 = new MWDataManager.clsDataAccess();
                _dbManColCrew18.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                //if (rgbXaxisFilter.SelectedIndex == 0)
                _dbManColCrew18.SqlStatement = " exec sp_PeformanceDetialDevPP '" + currentYear + "'     ";
                //if (rgbXaxisFilter.SelectedIndex == 1)
                //    _dbManColCrew18.SqlStatement = " exec sp_PeformanceDetialDevMinPP '" + currentYear + "'    ";
                //if (rgbXaxisFilter.SelectedIndex == 2)
                //    _dbManColCrew18.SqlStatement = " exec sp_PeformanceDetialDevSBPP '" + currentYear + "'    ";
                //if (rgbXaxisFilter.SelectedIndex == 3)
                //    _dbManColCrew18.SqlStatement = " exec sp_PeformanceDetialDevMOPP '" + currentYear + "'    ";

                _dbManColCrew18.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManColCrew18.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManColCrew18.ResultsTableName = "Crew18";  //get table name
                _dbManColCrew18.ExecuteInstruction();

                DataTable dtColCrew18 = _dbManColCrew18.ResultsDataTable;

                DataSet dscolCrew18 = new DataSet();

                dscolCrew18.Tables.Add(dtColCrew18);

                theReport.RegisterData(dscolCrew18);
                theReport.RegisterData(dscolData);
                theReport.RegisterData(ds);

                theReport.Load(TGlobalItems.ReportsFolder + "\\PerformanceCrewDev.frx");

                //theReport.Design();

                theReport.Prepare();
                theReport.ShowPrepared();
            }
        }

        private void cbxlistboxPM_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            LoadSections();
            LoadPerformance();
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

        private void GridOrig_DoubleClick(object sender, EventArgs e)
        {
            int focusedColumn = gridView7.FocusedRowHandle;

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
                //if (gridView7.GetRowCellValue(gridView7.FocusedRowHandle, gridView7.Columns["Show"]).ToString() == "N")
                //{
                //    gridView7.SetFocusedRowCellValue(gridView7.FocusedColumn, "Y");
                //}
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
                OreP.FormType = "Performance";
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



            if (RgbStopeOrDev.SelectedIndex == 0)
            {
                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumns]  \r\n" +
                                          " set ColColour = '" + color + "' ,  Show = '" + Show + "' , YAxis = '" + Axis + "', ChartStyle = '" + Style + "' ,ShowLbl = '" + ShowLbl + "'  \r\n" +
                                           "where [Order] = '" + order + "' and ColDescription = '" + DeScription + "'  ";
                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ExecuteInstruction();



                loadGrid();
                LoadGraph();
            }


            if (RgbStopeOrDev.SelectedIndex == 1)
            {
                MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManMonth.SqlStatement = "update [dbo].[tbl_PerformanceCrewColumnsDev]  \r\n" +
                                          " set ColColour = '" + color + "' ,  Show = '" + Show + "' , YAxis = '" + Axis + "', ChartStyle = '" + Style + "' ,ShowLbl = '" + ShowLbl + "'  \r\n" +
                                           "where [Order] = '" + order + "' and ColDescription = '" + DeScription + "'  ";
                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ExecuteInstruction();



                loadGrid();
                LoadGraph();
            }


            gridView7.Focus();
            gridView7.FocusedRowHandle = focusedColumn;
        }

        private void spinEditLow1_EditValueChanged(object sender, EventArgs e)
        {
            if (spinEditLow1.Value >= spinEditHigh1.Value)
            {
                spinEditLow1.Value = spinEditHigh1.Value - 1;
            }

            Lowlevel1txt.Text = spinEditLow1.Value.ToString();
        }

        private void spinEditHigh1_EditValueChanged(object sender, EventArgs e)
        {
            if (spinEditHigh1.Value >= spinEditLow2.Value)
            {
                spinEditHigh1.Value = spinEditLow2.Value - 1;
            }

            HighLevel1txt.Text = spinEditHigh1.Value.ToString();
        }

        private void spinEditLow2_EditValueChanged(object sender, EventArgs e)
        {
            if (spinEditLow2.Value >= spinEditHigh2.Value)
            {
                spinEditLow2.Value = spinEditHigh2.Value - 1;
            }

            LowLevel2txt.Text = spinEditLow2.Value.ToString();
        }

        private void spinEditHigh2_EditValueChanged(object sender, EventArgs e)
        {
            HighLevel2txt.Text = spinEditHigh2.Value.ToString();
        }

        private void Chart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            var results = Chart.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);

            string order = string.Empty;
            string DeScription = string.Empty;
            string Axis = string.Empty;
            string color = string.Empty;
            string Show = string.Empty;
            string Style = string.Empty;
            string ShowLbl = string.Empty;

            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    if (result.Series.Name != "avg1" || result.Series.Name != "avg2" || result.Series.Name != "avg3" || result.Series.Name != "avg4" || result.Series.Name != "avg5")
                    {
                        frmPerformanceDetial RepFrm = new frmPerformanceDetial();
                        RepFrm.Text = "Crew Ranking";
                        RepFrm.lblCrew.Text = result.Series.Points[result.PointIndex].AxisLabel.ToString().TrimEnd();
                        RepFrm.currMonth = label1.Text;
                        RepFrm.StopeOrDev = RgbStopeOrDev.SelectedIndex.ToString();
                        RepFrm._strFilter = rgbXaxisFilter.SelectedIndex.ToString();
                        RepFrm._theSystemDBTag = theSystemDBTag;
                        RepFrm._UserCurrentInfoConnection = UserCurrentInfo.Connection;

                        if (RgbStopeOrDev.SelectedIndex == 0)
                        {
                            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                            _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                            _dbManMonth.SqlStatement = "  ";
                            for (int i = 0; i < gridView7.RowCount; i++)
                            {

                                order = gridView7.GetRowCellValue(i, gridView7.Columns["Order"]).ToString();
                                DeScription = gridView7.GetRowCellValue(i, gridView7.Columns["ColDescription"]).ToString();
                                Axis = gridView7.GetRowCellValue(i, gridView7.Columns["YAxis"]).ToString();
                                color = gridView7.GetRowCellValue(i, gridView7.Columns["ColColour"]).ToString();
                                Show = gridView7.GetRowCellValue(i, gridView7.Columns["Show"]).ToString();
                                Style = gridView7.GetRowCellValue(i, gridView7.Columns["ChartStyle"]).ToString();
                                ShowLbl = gridView7.GetRowCellValue(i, gridView7.Columns["ShowLbl"]).ToString();

                                _dbManMonth.SqlStatement = _dbManMonth.SqlStatement + "update [dbo].[tbl_PerformanceCrewColumnsDetial]  \r\n" +
                                                        " set ColColour = '" + color + "' ,  Show = '" + Show + "' , YAxis = '" + Axis + "', ChartStyle = '" + Style + "' ,ShowLbl = '" + ShowLbl + "',[Order] = '" + order + "'  \r\n" +
                                                        "where ColDescription = '" + DeScription + "'  \r\n\r\n";
                            }

                            _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManMonth.ExecuteInstruction();
                        }

                        if (RgbStopeOrDev.SelectedIndex == 1)
                        {
                            MWDataManager.clsDataAccess _dbManMonthaa = new MWDataManager.clsDataAccess();
                            _dbManMonthaa.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                            _dbManMonthaa.SqlStatement = "  ";
                            for (int i = 0; i < gridView7.RowCount; i++)
                            {

                                order = gridView7.GetRowCellValue(i, gridView7.Columns["Order"]).ToString();
                                DeScription = gridView7.GetRowCellValue(i, gridView7.Columns["ColDescription"]).ToString();
                                Axis = gridView7.GetRowCellValue(i, gridView7.Columns["YAxis"]).ToString();
                                color = gridView7.GetRowCellValue(i, gridView7.Columns["ColColour"]).ToString();
                                Show = gridView7.GetRowCellValue(i, gridView7.Columns["Show"]).ToString();
                                Style = gridView7.GetRowCellValue(i, gridView7.Columns["ChartStyle"]).ToString();
                                ShowLbl = gridView7.GetRowCellValue(i, gridView7.Columns["ShowLbl"]).ToString();

                                _dbManMonthaa.SqlStatement = _dbManMonthaa.SqlStatement + "update [dbo].[tbl_PerformanceCrewColumnsDevDetial]  \r\n" +
                                                        " set ColColour = '" + color + "' ,  Show = '" + Show + "' , YAxis = '" + Axis + "', ChartStyle = '" + Style + "' ,ShowLbl = '" + ShowLbl + "',[Order] = '" + order + "'  \r\n" +
                                                        "where ColDescription = '" + DeScription + "'  \r\n\r\n";
                            }

                            _dbManMonthaa.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManMonthaa.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManMonthaa.ExecuteInstruction();
                        }

                        RepFrm.WindowState = FormWindowState.Maximized;
                        RepFrm.ShowDialog();

                    }
                }
            }
        }

        private void MOcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MOlabel.Text = MOcomboBox.Text;
            LoadPerformance();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void rcCrewAnalysis_Click(object sender, EventArgs e)
        {

        }
    }
}
