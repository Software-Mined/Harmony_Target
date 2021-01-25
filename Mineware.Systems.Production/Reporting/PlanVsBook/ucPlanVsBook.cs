using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Mineware.Systems.Production.PlanvsBook
{
    public partial class ucPlanVsBook : BaseUserControl
    {
        /// <summary>
        /// Object declaration and inicialisation
        /// </summary>
        DataTable dtSysSet;
        //clsSystemSettings _clsSystemSettings = new clsSystemSettings();
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        string sec;
        string Hier;
        //Procedures procs = new Procedures();
        int PreviousSelection = 0;
        private string _FirstLoad = "Y";
        string month;
        int col = 5;
        int col1 = 5;
        string vm = "Vamping", dev = "Development", stp = "Stoping";
        string sqm = "Sqm", mts = "Metres", ton = "Tons", kg = "Kilograms";
        string SectionName = string.Empty;
        string SelectedRadioName = string.Empty;
        string adjBook = "Y";

        Procedures procs = new Procedures();

        DataTable dtSections = new DataTable();

        Report theReport = new Report();



        public ucPlanVsBook()
        {
            InitializeComponent();

            FormRibbonPages.Add(rpPlanVsBook);
            FormActiveRibbonPage = rpPlanVsBook;
            FormMainRibbonPage = rpPlanVsBook;
            RibbonControl = rcPlanVsBook;
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void dtpStartDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void ucPlanVsBooking_Load(object sender, EventArgs e)
        {

            tabCycle.Visible = false;
            barProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobalTSysSettings._currentProductionMonth.ToString());
            adjBook = "Y";
            Show = "SQM";
            LoadSection();

            Showrgb.EditValue = sqm;
            Typergp.EditValue = stp;

            tabDaily.Text = "     Daily     ";
            ProgressiveTab.Text = "  Progressive  ";
            GraphTab.Text = "     Graph     ";

            SelectedRadioName = "Sqm";
            gridDaily.Rows.Clear();
            gridProgressive.Rows.Clear();
            gridBack.Rows.Clear();
            btnShow.Enabled = true;

            barButtonItem4_ItemClick(null, null);
        }



        void LoadSection()
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = " Select SectionID,case when name = '' then sectionid else isnull(Name,SectionID) end as Name,Hierarchicalid from tbl_section where prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' order by Hierarchicalid,SectionID ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            dtSections = _dbMan1.ResultsDataTable;

            DataTable dtSect = _dbMan1.ResultsDataTable;
            repSection.DataSource = dtSect;
            repSection.DisplayMember = "Name";
            repSection.ValueMember = "SectionID";
            repSection.PopulateViewColumns();
            repSection.View.Columns[2].Visible = false;

            barSection.EditValue = _dbMan1.ResultsDataTable.Rows[0][0].ToString();

        }

        private void mwRepositoryItemProdMonth1_EditValueChanged(object sender, EventArgs e)
        {
            //ribbonControl1.Manager.ActiveEditItemLink.PostEditor();
        }

        private void barProdmonth_EditValueChanged(object sender, EventArgs e)
        {
            LoadSection();
            _FirstLoad = "N";
        }

        private void barSection_EditValueChanged(object sender, EventArgs e)
        {
            btnShow.Enabled = true;
            gridDaily.Rows.Clear();
            gridProgressive.Rows.Clear();
            gridDaily.Visible = false;
            //gridBack.Rows.Clear();
            gridProgressive.Visible = false;
            sec = barSection.EditValue.ToString();
            //SectionName = barSection.;

            for (int x = 0; x <= dtSections.Rows.Count; x++)
            {
                if (sec == dtSections.Rows[x][0].ToString())
                {
                    Hier = dtSections.Rows[x]["Hierarchicalid"].ToString();
                    break;
                }
            }

            if (Hier == "0")
            {


                repCbxShowInfo.Items.Clear();
                repCbxShowInfo.Items.Add("Total Mine");
                repCbxShowInfo.Items.Add("Mine Manager");
                repCbxShowInfo.Items.Add("Mining Manager");
                repCbxShowInfo.Items.Add("Mine Overseer");
                repCbxShowInfo.Items.Add("Shiftboss");
                repCbxShowInfo.Items.Add("Miner");
                repCbxShowInfo.Items.Add("Workplace");

                barShowInfo.EditValue = "Total Mine";
                try
                {
                    gridDaily.Columns[1].Visible = false;
                    gridDaily.Columns[2].Visible = false;
                    gridProgressive.Columns[1].Visible = false;
                    gridProgressive.Columns[2].Visible = false;
                }
                catch { }
            }

            if (Hier == "1")
            {


                repCbxShowInfo.Items.Clear();
                repCbxShowInfo.Items.Add("Total Mine");
                repCbxShowInfo.Items.Add("Mine Manager");
                repCbxShowInfo.Items.Add("Mining Manager");
                repCbxShowInfo.Items.Add("Mine Overseer");
                repCbxShowInfo.Items.Add("Shiftboss");
                repCbxShowInfo.Items.Add("Miner");
                repCbxShowInfo.Items.Add("Workplace");

                barShowInfo.EditValue = "Total Mine";
                try
                {
                    gridDaily.Columns[1].Visible = false;
                    gridDaily.Columns[2].Visible = false;
                    gridProgressive.Columns[1].Visible = false;
                    gridProgressive.Columns[2].Visible = false;
                }
                catch { }
            }

            if (Hier == "2")
            {
                repCbxShowInfo.Items.Clear();
                repCbxShowInfo.Items.Add("Mine Manager");
                repCbxShowInfo.Items.Add("Mining Manager");
                repCbxShowInfo.Items.Add("Mine Overseer");
                repCbxShowInfo.Items.Add("Shiftboss");
                repCbxShowInfo.Items.Add("Miner");
                repCbxShowInfo.Items.Add("Workplace");

                barShowInfo.EditValue = "Mine Manager";

                try
                {
                    gridDaily.Columns[1].Visible = false;
                    gridDaily.Columns[2].Visible = false;
                    gridProgressive.Columns[1].Visible = false;
                    gridProgressive.Columns[2].Visible = false;
                }
                catch { }
            }

            if (Hier == "3")
            {
                repCbxShowInfo.Items.Clear();
                repCbxShowInfo.Items.Add("Mining Manager");
                repCbxShowInfo.Items.Add("Mine Overseer");
                repCbxShowInfo.Items.Add("Shiftboss");
                repCbxShowInfo.Items.Add("Miner");
                repCbxShowInfo.Items.Add("Workplace");

                barShowInfo.EditValue = "Mining Manager";

                try
                {
                    gridDaily.Columns[1].Visible = false;
                    gridDaily.Columns[2].Visible = false;
                    gridProgressive.Columns[1].Visible = false;
                    gridProgressive.Columns[2].Visible = false;
                }
                catch { }
            }

            if (Hier == "4")
            {
                repCbxShowInfo.Items.Clear();
                repCbxShowInfo.Items.Add("Mine Overseer");
                repCbxShowInfo.Items.Add("Shiftboss");
                repCbxShowInfo.Items.Add("Miner");
                repCbxShowInfo.Items.Add("Workplace");

                barShowInfo.EditValue = "Mine Overseer";

                try
                {
                    gridDaily.Columns[1].Visible = false;
                    gridDaily.Columns[2].Visible = false;
                    gridProgressive.Columns[1].Visible = false;
                    gridProgressive.Columns[2].Visible = false;
                }
                catch { }
            }

            if (Hier == "5")
            {
                repCbxShowInfo.Items.Clear();
                repCbxShowInfo.Items.Add("Shiftboss");
                repCbxShowInfo.Items.Add("Miner");
                repCbxShowInfo.Items.Add("Workplace");

                barShowInfo.EditValue = "Shiftboss";

                try
                {
                    gridDaily.Columns[1].Visible = false;
                    gridDaily.Columns[2].Visible = false;
                    gridProgressive.Columns[1].Visible = false;
                    gridProgressive.Columns[2].Visible = false;
                }
                catch { }


            }

            if (Hier == "6")
            {
                repCbxShowInfo.Items.Clear();
                repCbxShowInfo.Items.Add("Miner");
                repCbxShowInfo.Items.Add("Workplace");

                barShowInfo.EditValue = "Miner";

                try
                {
                    gridDaily.Columns[1].Visible = true;
                    gridDaily.Columns[2].Visible = true;
                    gridProgressive.Columns[1].Visible = true;
                    gridProgressive.Columns[2].Visible = true;
                }
                catch { }
            }

            gridDaily.Rows.Clear();
            barButtonItem4_ItemClick(null, null);

        }

        private void LineBtn_Click(object sender, EventArgs e)
        {
            CycleChart.Series[0].ChartType = SeriesChartType.Line;
            CycleChart.Series[1].ChartType = SeriesChartType.Line;
        }

        private void BarBtn_Click(object sender, EventArgs e)
        {
            CycleChart.Series[0].ChartType = SeriesChartType.Column;
            CycleChart.Series[1].ChartType = SeriesChartType.Column;
        }

        private void AreaBtn_Click(object sender, EventArgs e)
        {
            CycleChart.Series[0].ChartType = SeriesChartType.Area;
            CycleChart.Series[1].ChartType = SeriesChartType.Area;
        }

        private void TwoDBtn_Click(object sender, EventArgs e)
        {
            if (CycleChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D == false)
            {
                CycleChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                TwoDBtn.Text = "2D";
            }
            else
            {
                CycleChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
                TwoDBtn.Text = "3D";
            }
        }

       
        #pragma warning disable CS0108
        private string Show = string.Empty;
        #pragma warning restore CS0108
        private void gridDaily_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    if (gridDaily.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.Yellow)
                    {


                        if (SelectedRadioName == "SQM")
                        {
                            Show = "SQM";
                        }
                        if (SelectedRadioName == "Meters")
                        {
                            Show = "Meters";
                        }
                        if (SelectedRadioName == "Tons")
                        {
                            Show = "Tons";
                        }
                        if (SelectedRadioName == "Kilograms")
                        {
                            Show = "Kilograms";
                        }
                        if (SelectedRadioName == "Ounces")
                        {
                            Show = "Ounces";
                        }

                        CycleChart.Series[0].Points.Clear();
                        CycleChart.Series[1].Points.Clear();

                        chart1.Series[0].Points.Clear();
                        chart1.Series[1].Points.Clear();

                        DateTime day1 = dtpStartDate.Value;

                        for (int s = 5; s <= col; s++)
                        {

                            if (gridDaily.CurrentRow.Cells[3].Value.ToString() == "Plan")
                            {

                                int nextRow = Convert.ToInt32(gridDaily.CurrentRow.Index) + 1;
                                if (gridDaily.CurrentRow.Cells[s].Value == null)
                                {
                                    CycleChart.Series[0].Points.AddXY(day1, 0);
                                    chart1.Series[0].Points.AddXY(day1, 0);
                                }
                                else
                                {
                                    try
                                    {
                                        SectionName = gridDaily.Rows[nextRow].Cells[0].Value.ToString();
                                        sec = gridDaily.CurrentRow.Cells[0].Value.ToString();
                                        CycleChart.Series[0].Points.AddXY(day1, gridDaily.CurrentRow.Cells[s].Value.ToString());
                                        chart1.Series[0].Points.AddXY(day1, gridDaily.CurrentRow.Cells[s].Value.ToString());
                                    }
                                    catch { }

                                }

                                if (gridDaily.Rows[nextRow].Cells[s].Value == null)
                                {
                                    CycleChart.Series[1].Points.AddXY(day1, 0);
                                    chart1.Series[1].Points.AddXY(day1, 0);
                                }
                                else
                                {
                                    SectionName = gridDaily.Rows[nextRow].Cells[0].Value.ToString();
                                    CycleChart.Series[1].Points.AddXY(day1, gridDaily.Rows[nextRow].Cells[s].Value.ToString());
                                    chart1.Series[1].Points.AddXY(day1, gridDaily.Rows[nextRow].Cells[s].Value.ToString());
                                }
                            }

                            else
                            {
                                int PrevRow = Convert.ToInt32(gridDaily.CurrentRow.Index) - 1;
                                if (gridDaily.Rows[PrevRow].Cells[s].Value == null)
                                {
                                    CycleChart.Series[0].Points.AddXY(day1, 0);
                                    chart1.Series[0].Points.AddXY(day1, 0);
                                }
                                else
                                {
                                    SectionName = gridDaily.Rows[PrevRow].Cells[0].Value.ToString();
                                    sec = gridDaily.Rows[PrevRow].Cells[0].Value.ToString();
                                    CycleChart.Series[0].Points.AddXY(day1, gridDaily.Rows[PrevRow].Cells[s].Value.ToString());
                                    chart1.Series[0].Points.AddXY(day1, gridDaily.Rows[PrevRow].Cells[s].Value.ToString());
                                }

                                if (gridDaily.CurrentRow.Cells[s].Value == null)
                                {
                                    CycleChart.Series[1].Points.AddXY(day1, 0);
                                    chart1.Series[1].Points.AddXY(day1, 0);
                                }
                                else
                                {
                                    SectionName = gridDaily.CurrentRow.Cells[0].Value.ToString();
                                    CycleChart.Series[1].Points.AddXY(day1, gridDaily.CurrentRow.Cells[s].Value.ToString());
                                    chart1.Series[1].Points.AddXY(day1, gridDaily.CurrentRow.Cells[s].Value.ToString());
                                }
                            }
                            day1 = day1.AddDays(1);
                        }
                        CycleChart.Titles["Title1"].Text = "Cycle Plan vs Actual  Prodmonth " + procs.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
                        CycleChart.Titles["Title2"].Text = "Daily figures for " + SectionName + "            For: " + Show;


                        chart1.Titles["Title1"].Text = "Cycle Plan vs Actual  Prodmonth " + procs.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
                        chart1.Titles["Title2"].Text = "Daily figures for " + SectionName + "            For: " + Show;



                    }
                }
            }
            catch { return; }
        }

        private void Showrgb_EditValueChanged(object sender, EventArgs e)
        {
            SelectedRadioName = Showrgb.EditValue.ToString();
            Show = Showrgb.EditValue.ToString();
            gridDaily.Rows.Clear();
            gridProgressive.Rows.Clear();
            gridBack.Rows.Clear();
            btnShow.Enabled = true;

            //barButtonItem4_ItemClick(null, null);

            //sec = procs.ExtractBeforeColon(barSection.Text);
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (tabCycle.SelectedTabPageIndex == 0)
            {
                DataTable Header = new DataTable();

                DataTable Detail = new DataTable();

                for (int i = 0; i < 60; i++)
                {
                    Header.Columns.Add();
                    Detail.Columns.Add();
                }

                Detail.Columns.Add();

                Header.Rows.Add();

                for (int i = 0; i < 60; i++)
                {
                    if (tabCycle.SelectedTabPageIndex == 0)
                    {
                        if (gridDaily.Columns[i].HeaderText.ToString() != string.Empty)
                            Header.Rows[0][i] = gridDaily.Columns[i].HeaderText.ToString();
                        else
                            if (i > 10)
                            Header.Rows[0][i] = "a";
                        else
                            Header.Rows[0][i] = string.Empty;



                    }
                }

                Header.Columns.Add();
                Header.Rows[0][60] = SysSettings.Banner;

                Header.Columns.Add();
                Header.Rows[0][61] = month;

                Header.Columns.Add();
                if (Showrgb.EditValue.ToString() == "Sqm")
                    Header.Rows[0][62] = barSection.EditValue + "  Squaremetres (Daily) ";

                if (Showrgb.EditValue.ToString() == "Metres")
                    Header.Rows[0][62] = barSection.EditValue + "  Metres (Daily) ";

                if (Showrgb.EditValue.ToString() == "Tons")
                    Header.Rows[0][62] = barSection.EditValue + "  Tons (Daily) ";

                if (Showrgb.EditValue.ToString() == "Kilograms")
                    Header.Rows[0][62] = barSection.EditValue + "  Kilograms (Daily) ";


                if (tabCycle.SelectedTabPageIndex == 0)
                {
                    for (int i = 0; i < gridDaily.RowCount; i++)
                    {
                        Detail.Rows.Add();

                        for (int c = 0; c < 60; c++)
                        {
                            if (gridDaily[c, i].Value != null)
                            {
                                Detail.Rows[i][c] = gridDaily[c, i].Value.ToString();
                                if (gridDaily[c, i].Style.BackColor == Color.Gainsboro)
                                {
                                    Detail.Rows[i][c] = "zz";
                                }
                            }
                            else
                                if (c > 10)
                            {
                                if (gridDaily.Columns[c].HeaderText.ToString() == string.Empty)
                                    Detail.Rows[i][c] = "a";
                            }
                            else
                            {

                                Detail.Rows[i][c] = string.Empty;


                            }

                        }


                        if (i < gridDaily.RowCount - 2)
                            Detail.Rows[i][60] = i;
                        else
                            Detail.Rows[i][60] = i * 10000;

                    }



                }


                Header.TableName = "Headers";
                DataSet DsHeader = new DataSet();
                DsHeader.Tables.Add(Header);

                theReport.RegisterData(DsHeader);


                Detail.TableName = "Details";
                DataSet DsDetail = new DataSet();
                DsDetail.Tables.Add(Detail);

                theReport.RegisterData(DsDetail);

                theReport.Load(_reportFolder + "\\PlanVrsBook.frx");

                //theReport.Design();


                theReport.Prepare();
                theReport.ShowPrepared();
            }


            if (tabCycle.SelectedTabPageIndex == 1)
            {
                DataTable Header = new DataTable();

                DataTable Detail = new DataTable();

                for (int i = 0; i < 60; i++)
                {
                    Header.Columns.Add();
                    Detail.Columns.Add();
                }

                Detail.Columns.Add();

                Header.Rows.Add();

                for (int i = 0; i < 60; i++)
                {
                    if (tabCycle.SelectedTabPageIndex == 1)
                    {
                        if (gridProgressive.Columns[i].HeaderText.ToString() != string.Empty)
                            Header.Rows[0][i] = gridProgressive.Columns[i].HeaderText.ToString();
                        else
                            if (i > 10)
                            Header.Rows[0][i] = "a";
                        else
                            Header.Rows[0][i] = string.Empty;



                    }
                }

                Header.Columns.Add();
                Header.Rows[0][60] = SysSettings.Banner;

                Header.Columns.Add();
                Header.Rows[0][61] = month;

                Header.Columns.Add();

                if (Showrgb.EditValue.ToString() == "Sqm")
                    Header.Rows[0][62] = barSection.EditValue + "  Squaremetres (Progressive) ";

                if (Showrgb.EditValue.ToString() == "Metres")
                    Header.Rows[0][62] = barSection.EditValue + "  Metres (Progressive) ";

                if (Showrgb.EditValue.ToString() == "Tons")
                    Header.Rows[0][62] = barSection.EditValue + "  Tons (Progressive) ";

                if (Showrgb.EditValue.ToString() == "Kilograms")
                    Header.Rows[0][62] = barSection.EditValue + "  Kilograms (Progressive) ";


                if (tabCycle.SelectedTabPageIndex == 1)
                {
                    for (int i = 0; i < gridProgressive.RowCount; i++)
                    {
                        Detail.Rows.Add();

                        for (int c = 0; c < 60; c++)
                        {
                            if (gridProgressive[c, i].Value != null)
                            {
                                Detail.Rows[i][c] = gridProgressive[c, i].Value.ToString();
                                if (gridProgressive[c, i].Style.BackColor == Color.Gainsboro)
                                {
                                    Detail.Rows[i][c] = "zz";
                                }
                            }
                            else
                                if (c > 10)
                            {
                                if (gridProgressive.Columns[c].HeaderText.ToString() == string.Empty)
                                    Detail.Rows[i][c] = "a";
                            }
                            else
                            {

                                Detail.Rows[i][c] = string.Empty;


                            }

                        }


                        if (i < gridProgressive.RowCount - 2)
                            Detail.Rows[i][60] = i;
                        else
                            Detail.Rows[i][60] = i * 10000;

                    }



                }


                Header.TableName = "Headers";
                DataSet DsHeader = new DataSet();
                DsHeader.Tables.Add(Header);

                theReport.RegisterData(DsHeader);


                Detail.TableName = "Details";
                DataSet DsDetail = new DataSet();
                DsDetail.Tables.Add(Detail);

                theReport.RegisterData(DsDetail);

                theReport.Load(TGlobalItems.ReportsFolder + "\\PlanVrsBook.frx");

                //theReport.Design();

                theReport.Prepare();
                theReport.ShowPrepared();
            }

            var printDialog = new PrintDialog();


            if (tabCycle.SelectedTabPageIndex == 2)
            {
                //public PrintingManager Printing { get; }




                //printDialog.Document.PrinterSettings.DefaultPageSettings.Landscape = true;

                // CycleChart.Width = 500;


                chart1.Printing.PageSetup();
                chart1.Printing.PrintPreview();
            }
        }

        private void gridDaily_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                if (gridDaily.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.LightSteelBlue)
                {
                    LoadBackGrid();
                    PreviousSelection = gridBack.Rows.Add();

                    gridBack.Rows[PreviousSelection].Cells[0].Value = barSection.EditValue.ToString();
                    gridBack.Rows[PreviousSelection].Cells[1].Value = barShowInfo.EditValue.ToString();



                    barSection.EditValue = sec;

                    for (int x = 0; x <= dtSections.Rows.Count - 1; x++)
                    {
                        if (sec == dtSections.Rows[x][1].ToString())
                        {
                            Hier = dtSections.Rows[x]["Hierarchicalid"].ToString();
                            break;
                        }
                    }
                    if (Hier == "0")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Total Mine");
                        repCbxShowInfo.Items.Add("Mine Manager");
                        repCbxShowInfo.Items.Add("Mining Manager");
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Total Mine";

                        gridDaily.Columns[1].Visible = false;
                        gridDaily.Columns[2].Visible = false;
                    }

                    if (Hier == "1")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Total Mine");
                        repCbxShowInfo.Items.Add("Mine Manager");
                        repCbxShowInfo.Items.Add("Mining Manager");
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Mine Manager";

                        gridDaily.Columns[1].Visible = false;
                        gridDaily.Columns[2].Visible = false;
                    }

                    if (Hier == "2")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Mine Manager");
                        repCbxShowInfo.Items.Add("Mining Manager");
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Mining Manager";

                        gridDaily.Columns[1].Visible = false;
                        gridDaily.Columns[2].Visible = false;
                    }

                    if (Hier == "3")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Mining Manager");
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Mine Overseer";

                        gridDaily.Columns[1].Visible = false;
                        gridDaily.Columns[2].Visible = false;
                    }

                    if (Hier == "4")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Shiftboss";

                        gridDaily.Columns[1].Visible = false;
                        gridDaily.Columns[2].Visible = false;
                    }

                    if (Hier == "5")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Miner";

                        gridDaily.Columns[1].Visible = false;
                        gridDaily.Columns[2].Visible = false;
                    }

                    if (Hier == "6")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Workplace";

                        gridDaily.Columns[1].Visible = true;
                        gridDaily.Columns[2].Visible = true;
                    }

                    if (_FirstLoad == "N")
                    {
                        if (Typergp.EditValue.ToString() == vm)
                            LoadDailyVamps();
                        else
                            LoadDaily();
                    }
                }
            }
        }

        void LoadBackGrid()
        {
            gridBack.ColumnCount = 2;
            gridBack.Columns[0].Width = 80;
            gridBack.Columns[0].HeaderText = "Section";
            gridBack.Columns[0].ReadOnly = true;
            gridBack.Columns[0].Visible = true;

            gridBack.Columns[1].Width = 50;
            gridBack.Columns[1].HeaderText = "Show";
            gridBack.Columns[1].ReadOnly = true;
            gridBack.Columns[1].Visible = true;
        }

        private void gridProgressive_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                if (gridProgressive.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.LightGreen)
                {
                    LoadBackGrid();
                    PreviousSelection = gridBack.Rows.Add();

                    gridBack.Rows[PreviousSelection].Cells[0].Value = barSection.EditValue.ToString();
                    gridBack.Rows[PreviousSelection].Cells[1].Value = barShowInfo.EditValue.ToString();


                    barSection.EditValue = sec;

                    for (int x = 0; x <= dtSections.Rows.Count - 1; x++)
                    {
                        if (sec == dtSections.Rows[x][1].ToString())
                        {
                            Hier = dtSections.Rows[x]["Hierarchicalid"].ToString();
                            break;
                        }
                    }

                    if (Hier == "0")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Total Mine");
                        repCbxShowInfo.Items.Add("Mine Manager");
                        repCbxShowInfo.Items.Add("Mining Manager");
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Total Mine";

                        gridProgressive.Columns[1].Visible = false;
                        gridProgressive.Columns[2].Visible = false;
                    }

                    if (Hier == "1")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Total Mine");
                        repCbxShowInfo.Items.Add("Mine Manager");
                        repCbxShowInfo.Items.Add("Mining Manager");
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Mine Manager";

                        gridProgressive.Columns[1].Visible = false;
                        gridProgressive.Columns[2].Visible = false;
                    }

                    if (Hier == "2")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Mine Manager");
                        repCbxShowInfo.Items.Add("Mining Manager");
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Mining Manager";

                        gridProgressive.Columns[1].Visible = false;
                        gridProgressive.Columns[2].Visible = false;
                    }

                    if (Hier == "3")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Mining Manager");
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Mine Overseer";

                        gridProgressive.Columns[1].Visible = false;
                        gridProgressive.Columns[2].Visible = false;
                    }

                    if (Hier == "4")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Mine Overseer");
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Shiftboss";

                        gridProgressive.Columns[1].Visible = false;
                        gridProgressive.Columns[2].Visible = false;
                    }

                    if (Hier == "5")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Shiftboss");
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Miner";

                        gridProgressive.Columns[1].Visible = false;
                        gridProgressive.Columns[2].Visible = false;
                    }

                    if (Hier == "6")
                    {
                        repCbxShowInfo.Items.Clear();
                        repCbxShowInfo.Items.Add("Miner");
                        repCbxShowInfo.Items.Add("Workplace");

                        barShowInfo.EditValue = "Workplace";

                        gridProgressive.Columns[1].Visible = true;
                        gridProgressive.Columns[2].Visible = true;
                    }

                    if (_FirstLoad == "N")
                    {
                        if (Typergp.EditValue.ToString() == vm)
                            LoadDailyVamps();
                        else
                            LoadDaily();
                    }
                }
            }
        }

        private void gridDaily_DoubleClick(object sender, EventArgs e)
        {

        }

        private void gridProgressive_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (gridProgressive.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.Yellow)
                {


                    if (SelectedRadioName == "SQM")
                    {
                        Show = "SQM";
                    }
                    if (SelectedRadioName == "Meters")
                    {
                        Show = "Meters";
                    }
                    if (SelectedRadioName == "Tons")
                    {
                        Show = "Tons";
                    }
                    if (SelectedRadioName == "Kilograms")
                    {
                        Show = "Kilograms";
                    }
                    if (SelectedRadioName == "Ounces")
                    {
                        Show = "Ounces";
                    }

                    CycleChart.Series[0].Points.Clear();
                    CycleChart.Series[1].Points.Clear();

                    chart1.Series[0].Points.Clear();
                    chart1.Series[1].Points.Clear();

                    DateTime day1 = dtpStartDate.Value;

                    for (int s = 5; s <= col; s++)
                    {
                        if (gridProgressive.CurrentRow.Cells[3].Value.ToString() == "Plan")
                        {

                            int nextRow = Convert.ToInt32(gridDaily.CurrentRow.Index) + 1;
                            if (gridProgressive.CurrentRow.Cells[s].Value == null)
                            {
                                CycleChart.Series[0].Points.AddXY(day1, 0);
                                chart1.Series[0].Points.AddXY(day1, 0);
                            }
                            else
                            {
                                try
                                {
                                    SectionName = gridProgressive.Rows[nextRow].Cells[0].Value.ToString();
                                    sec = gridProgressive.CurrentRow.Cells[0].Value.ToString();
                                    CycleChart.Series[0].Points.AddXY(day1, gridProgressive.CurrentRow.Cells[s].Value.ToString());
                                    chart1.Series[0].Points.AddXY(day1, gridProgressive.CurrentRow.Cells[s].Value.ToString());
                                }
                                catch { }

                            }

                            if (gridProgressive.Rows[nextRow].Cells[s].Value == null)
                            {
                                CycleChart.Series[1].Points.AddXY(day1, 0);
                                chart1.Series[1].Points.AddXY(day1, 0);
                            }
                            else
                            {
                                SectionName = gridDaily.Rows[nextRow].Cells[0].Value.ToString();
                                CycleChart.Series[1].Points.AddXY(day1, gridProgressive.Rows[nextRow].Cells[s].Value.ToString());
                                chart1.Series[1].Points.AddXY(day1, gridProgressive.Rows[nextRow].Cells[s].Value.ToString());
                            }

                        }
                        else
                        {
                            int PrevRow = Convert.ToInt32(gridProgressive.CurrentRow.Index) - 1;
                            if (gridProgressive.Rows[PrevRow].Cells[s].Value == null)
                            {
                                CycleChart.Series[0].Points.AddXY(day1, 0);
                                chart1.Series[0].Points.AddXY(day1, 0);
                            }
                            else
                            {
                                SectionName = gridProgressive.Rows[PrevRow].Cells[0].Value.ToString();
                                sec = gridProgressive.Rows[PrevRow].Cells[0].Value.ToString();
                                CycleChart.Series[0].Points.AddXY(day1, gridProgressive.Rows[PrevRow].Cells[s].Value.ToString());
                                chart1.Series[0].Points.AddXY(day1, gridProgressive.Rows[PrevRow].Cells[s].Value.ToString());
                            }

                            if (gridDaily.CurrentRow.Cells[s].Value == null)
                            {
                                CycleChart.Series[1].Points.AddXY(day1, 0);
                                chart1.Series[1].Points.AddXY(day1, 0);
                            }
                            else
                            {
                                SectionName = gridDaily.CurrentRow.Cells[0].Value.ToString();
                                CycleChart.Series[1].Points.AddXY(day1, gridProgressive.CurrentRow.Cells[s].Value.ToString());
                                chart1.Series[1].Points.AddXY(day1, gridProgressive.CurrentRow.Cells[s].Value.ToString());
                            }
                        }
                        day1 = day1.AddDays(1);
                    }
                    CycleChart.Titles["Title1"].Text = "Cycle Plan vs Actual  Prodmonth " + procs.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
                    CycleChart.Titles["Title2"].Text = "Daily figures for " + SectionName + "            For: " + Show;


                    chart1.Titles["Title1"].Text = "Cycle Plan vs Actual  Prodmonth " + procs.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
                    chart1.Titles["Title2"].Text = "Daily figures for " + SectionName + "            For: " + Show;



                }
            }
        }

        private void barShowInfo_EditValueChanged(object sender, EventArgs e)
        {
            barButtonItem4_ItemClick(null, null);
        }

        private void Typergp_EditValueChanged(object sender, EventArgs e)
        {
            barButtonItem4_ItemClick(null, null);
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void gridProgressive_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1)
                {
                    if (gridProgressive.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.Yellow)
                    {


                        if (SelectedRadioName == "SQM")
                        {
                            Show = "SQM";
                        }
                        if (SelectedRadioName == "Meters")
                        {
                            Show = "Meters";
                        }
                        if (SelectedRadioName == "Tons")
                        {
                            Show = "Tons";
                        }
                        if (SelectedRadioName == "Kilograms")
                        {
                            Show = "Kilograms";
                        }
                        if (SelectedRadioName == "Ounces")
                        {
                            Show = "Ounces";
                        }

                        CycleChart.Series[0].Points.Clear();
                        CycleChart.Series[1].Points.Clear();

                        chart1.Series[0].Points.Clear();
                        chart1.Series[1].Points.Clear();

                        DateTime day1 = dtpStartDate.Value;

                        for (int s = 5; s <= col; s++)
                        {

                            if (gridProgressive.CurrentRow.Cells[3].Value.ToString() == "Plan")
                            {

                                int nextRow = Convert.ToInt32(gridProgressive.CurrentRow.Index) + 1;
                                if (gridProgressive.CurrentRow.Cells[s].Value == null)
                                {
                                    CycleChart.Series[0].Points.AddXY(day1, 0);
                                    chart1.Series[0].Points.AddXY(day1, 0);
                                }
                                else
                                {
                                    try
                                    {
                                        SectionName = gridProgressive.Rows[nextRow].Cells[0].Value.ToString();
                                        sec = gridProgressive.CurrentRow.Cells[0].Value.ToString();
                                        CycleChart.Series[0].Points.AddXY(day1, gridProgressive.CurrentRow.Cells[s].Value.ToString());
                                        chart1.Series[0].Points.AddXY(day1, gridProgressive.CurrentRow.Cells[s].Value.ToString());
                                    }
                                    catch { }

                                }

                                if (gridProgressive.Rows[nextRow].Cells[s].Value == null)
                                {
                                    CycleChart.Series[1].Points.AddXY(day1, 0);
                                    chart1.Series[1].Points.AddXY(day1, 0);
                                }
                                else
                                {
                                    SectionName = gridProgressive.Rows[nextRow].Cells[0].Value.ToString();
                                    CycleChart.Series[1].Points.AddXY(day1, gridProgressive.Rows[nextRow].Cells[s].Value.ToString());
                                    chart1.Series[1].Points.AddXY(day1, gridProgressive.Rows[nextRow].Cells[s].Value.ToString());
                                }
                            }

                            else
                            {
                                int PrevRow = Convert.ToInt32(gridProgressive.CurrentRow.Index) - 1;
                                if (gridProgressive.Rows[PrevRow].Cells[s].Value == null)
                                {
                                    CycleChart.Series[0].Points.AddXY(day1, 0);
                                    chart1.Series[0].Points.AddXY(day1, 0);
                                }
                                else
                                {
                                    SectionName = gridProgressive.Rows[PrevRow].Cells[0].Value.ToString();
                                    sec = gridProgressive.Rows[PrevRow].Cells[0].Value.ToString();
                                    CycleChart.Series[0].Points.AddXY(day1, gridProgressive.Rows[PrevRow].Cells[s].Value.ToString());
                                    chart1.Series[0].Points.AddXY(day1, gridProgressive.Rows[PrevRow].Cells[s].Value.ToString());
                                }

                                if (gridProgressive.CurrentRow.Cells[s].Value == null)
                                {
                                    CycleChart.Series[1].Points.AddXY(day1, 0);
                                    chart1.Series[1].Points.AddXY(day1, 0);
                                }
                                else
                                {
                                    SectionName = gridProgressive.CurrentRow.Cells[0].Value.ToString();
                                    CycleChart.Series[1].Points.AddXY(day1, gridProgressive.CurrentRow.Cells[s].Value.ToString());
                                    chart1.Series[1].Points.AddXY(day1, gridProgressive.CurrentRow.Cells[s].Value.ToString());
                                }
                            }
                            day1 = day1.AddDays(1);
                        }
                        CycleChart.Titles["Title1"].Text = "Cycle Plan vs Actual  Prodmonth " + procs.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
                        CycleChart.Titles["Title2"].Text = "Progressive figures for " + SectionName + "            For: " + Show;


                        chart1.Titles["Title1"].Text = "Cycle Plan vs Actual  Prodmonth " + procs.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));
                        chart1.Titles["Title2"].Text = "Progressive figures for " + SectionName + "            For: " + Show;



                    }
                }
            }
            catch { return; }
        }

        private void barbtnBack_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridBack.RowCount != 0)
            {
                barSection.EditValue = gridBack.Rows[PreviousSelection].Cells[0].Value.ToString();
                barShowInfo.EditValue = gridBack.Rows[PreviousSelection].Cells[1].Value.ToString();
                sec = procs.ExtractBeforeColon(gridBack.Rows[PreviousSelection].Cells[0].Value.ToString());

                for (int x = 0; x <= dtSections.Rows.Count - 1; x++)
                {
                    if (sec == dtSections.Rows[x][1].ToString())
                    {
                        Hier = dtSections.Rows[x]["Hierarchicalid"].ToString();
                        break;
                    }
                }

                if (_FirstLoad == "N")
                {
                    LoadDaily();
                }
                

                gridDaily.Columns[1].Visible = false;
                gridDaily.Columns[2].Visible = false;
                gridProgressive.Columns[1].Visible = false;
                gridProgressive.Columns[2].Visible = false;

                gridBack.Rows.RemoveAt(PreviousSelection);
                PreviousSelection--;
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridDaily.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSlateGray;
            gridDaily.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            gridDaily.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Pixel);
            gridDaily.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.2F, GraphicsUnit.Pixel);
            gridDaily.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridDaily.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //gridDaily.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            gridDaily.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            gridDaily.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            gridDaily.GridColor = Color.LightSlateGray;
            gridDaily.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gridDaily.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gridDaily.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.OutsetDouble;

            gridProgressive.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSlateGray;
            gridProgressive.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            gridProgressive.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Pixel);
            gridProgressive.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.2F, GraphicsUnit.Pixel);
            gridProgressive.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridProgressive.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //gridDaily.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            gridProgressive.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            gridProgressive.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            gridProgressive.GridColor = Color.LightSlateGray;
            gridProgressive.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gridDaily.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.OutsetDouble;

            DataTable dtCalendar = new DataTable();
            dtCalendar.Clear();
            //gridBack.Rows.Clear();
            gridDaily.Rows.Clear();
            gridDaily.ColumnCount = 0;
            gridProgressive.Rows.Clear();

            sec = barSection.EditValue.ToString();

            PreviousSelection = 0;
            gridDaily.ColumnCount = 70;

            gridDaily.Columns[0].Width = 130;
            gridDaily.Columns[0].HeaderText = "Section";
            gridDaily.Columns[0].ReadOnly = true;
            gridDaily.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridDaily.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridDaily.Columns[0].Visible = true;

            gridDaily.Columns[1].Width = 60;
            gridDaily.Columns[1].HeaderText = string.Empty;
            gridDaily.Columns[1].ReadOnly = true;
            gridDaily.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridDaily.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            if (barShowInfo.EditValue.ToString() == "Workplace")
            {
                gridDaily.Columns[1].Visible = true;
            }
            else
            {
                gridDaily.Columns[1].Visible = false;
            }

            gridDaily.Columns[2].Width = 160;
            gridDaily.Columns[2].HeaderText = "Workplace";
            gridDaily.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridDaily.Columns[2].ReadOnly = true;
            if (barShowInfo.EditValue.ToString() == "Workplace")
            {
                gridDaily.Columns[2].Visible = true;
            }
            else
            {
                gridDaily.Columns[2].Visible = false;
            }

            gridDaily.Columns[3].Width = 40;
            gridDaily.Columns[3].HeaderText = string.Empty;
            gridDaily.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridDaily.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridDaily.Columns[3].ReadOnly = true;
            gridDaily.Columns[3].Visible = true;


            gridDaily.Columns[4].Width = 45;
            gridDaily.Columns[4].HeaderText = "Prog.";
            gridDaily.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridDaily.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridDaily.Columns[4].ReadOnly = true;
            gridDaily.Columns[4].Visible = true;


            ////////////Proggresive Grid//////////////
            gridProgressive.ColumnCount = 70;

            gridProgressive.Columns[0].Width = 130;
            gridProgressive.Columns[0].HeaderText = "Section";
            gridProgressive.Columns[0].ReadOnly = true;
            gridProgressive.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridProgressive.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridProgressive.Columns[0].Visible = true;

            gridProgressive.Columns[1].Width = 60;
            gridProgressive.Columns[1].HeaderText = string.Empty;
            gridProgressive.Columns[1].ReadOnly = true;
            gridProgressive.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridProgressive.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            if (barShowInfo.EditValue.ToString() == "Workplace")
            {
                gridProgressive.Columns[1].Visible = true;
            }
            else
            {
                gridProgressive.Columns[1].Visible = false;
            }

            gridProgressive.Columns[2].Width = 160;
            gridProgressive.Columns[2].HeaderText = "Workplace";
            gridProgressive.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridProgressive.Columns[2].ReadOnly = true;
            if (barShowInfo.EditValue.ToString() == "Workplace")
            {
                gridProgressive.Columns[2].Visible = true;
            }
            else
            {
                gridProgressive.Columns[2].Visible = false;
            }

            gridProgressive.Columns[3].Width = 40;
            gridProgressive.Columns[3].HeaderText = string.Empty;
            gridProgressive.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridProgressive.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridProgressive.Columns[3].ReadOnly = true;
            gridProgressive.Columns[3].Visible = true;


            gridProgressive.Columns[4].Width = 45;
            gridProgressive.Columns[4].HeaderText = "Prog.";
            gridProgressive.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridProgressive.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridProgressive.Columns[4].ReadOnly = true;
            gridProgressive.Columns[4].Visible = true;

            for (int y = 5; y < 70; y++)
            {
                gridProgressive.Columns[y].Width = 40;
                gridProgressive.Columns[y].SortMode = DataGridViewColumnSortMode.NotSortable;
                gridProgressive.Columns[y].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridProgressive.Columns[y].ReadOnly = true;
                gridProgressive.Columns[y].Visible = false;

                gridDaily.Columns[y].Width = 36;
                gridDaily.Columns[y].SortMode = DataGridViewColumnSortMode.NotSortable;
                gridDaily.Columns[y].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                gridDaily.Columns[y].ReadOnly = true;
                gridDaily.Columns[y].Visible = false;
            }

            month = procs.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue));

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (Typergp.EditValue.ToString() == vm)
                _dbMan.SqlStatement = "select distinct(calendardate) TheDate from tbl_PLANNING_Vamping p ";
            else
                _dbMan.SqlStatement = "select distinct(calendardate) TheDate from vw_PlanningFact p ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "left outer join tbl_section s on p.sectionid = s.sectionid and " +
                                   "p.prodmonth = s.prodmonth " +
                                   "left outer join tbl_section s1 on s.reporttosectionid = s1.sectionid and " +
                                   "s.prodmonth = s1.prodmonth " +
                                   "left outer join tbl_section s2 on s1.reporttosectionid = s2.sectionid and " +
                                   "s1.prodmonth = s2.prodmonth " +
                                   "left outer join tbl_section s3 on s2.reporttosectionid = s3.sectionid and " +
                                   "s2.prodmonth = s3.prodmonth  " +
                                   "left outer join tbl_section s4 on s3.reporttosectionid = s4.sectionid and  " +
                                   "s3.prodmonth = s4.prodmonth " +
                                    "left outer join tbl_section s5 on s4.reporttosectionid = s5.sectionid and  " +
                                   "s4.prodmonth = s5.prodmonth ";
            if (Typergp.EditValue.ToString() == stp)
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "where p.prodmonth = '" + month + "' and activity <> 1  ";
            }
            if (Typergp.EditValue.ToString() == dev)
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "where p.prodmonth = '" + month + "' and activity = 1  ";
            }

            if (Typergp.EditValue.ToString() == vm)
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "where p.prodmonth = '" + month + "' ";
            }

            if (Hier == "0")
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s5.reporttosectionid = '" + sec + "'";
            }

            if (Hier == "1")
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s4.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "2")
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s3.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "3")
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s2.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "4")
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s1.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "5")
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "6")
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "and s.sectionid = '" + sec + "'";
            }
            _dbMan.SqlStatement = _dbMan.SqlStatement + "order by calendardate ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dtCalendar = _dbMan.ResultsDataTable;

            if (dtCalendar.Rows.Count > 0)
            {
                int col = 5;

                dtpStartDate.Value = Convert.ToDateTime(dtCalendar.Rows[0]["TheDate"].ToString());

                foreach (DataRow dr in dtCalendar.Rows)
                {

                    gridDaily.Columns[col].HeaderText = Convert.ToDateTime(dr["TheDate"].ToString()).ToString("dd MMM ddd");
                    gridDaily.Columns[col].Visible = true;

                    gridProgressive.Columns[col].HeaderText = Convert.ToDateTime(dr["TheDate"].ToString()).ToString("dd MMM ddd");
                    gridProgressive.Columns[col].Visible = true;

                    col++;
                }

                gridDaily.Rows.Clear();
                gridProgressive.Rows.Clear();

                if (_FirstLoad == "N")
                {
                    if (Typergp.EditValue.ToString() == vm)
                        LoadDailyVamps();
                    else
                        LoadDaily();
                }

                tabCycle.Visible = true;

            }
        }

        private void tabCycle_Click(object sender, EventArgs e)
        {

        }


        public void LoadDailyVamps()
        {
            gridDaily.Visible = false;
            gridProgressive.Visible = false;
            //SectionName = procs.ExtractAfterColon(barSection.Text);


            //sec = procs.ExtractBeforeColon(barSection.Text);

            MWDataManager.clsDataAccess _dbManDaily = new MWDataManager.clsDataAccess();
            _dbManDaily.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            if (barShowInfo.EditValue.Equals("Total Mine"))
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " Select '" + sec + "' [SectionId] , '" + SectionName + "' [SectionName] ,'" + string.Empty + "' workplaceid, '" + string.Empty + "' description,theDate,  " +
                                           "Sum(plansqm) PlanSqm, Sum(planadv) PlanAdv, Sum(planTons) PlanTons, " +
                                           "Sum(plangrams) PlanGrams,  " +
                                           "Sum(booksqm) BookSqm, Sum(bookadv) BookAdv, Sum(booktons) BookTons, " +
                                           "Sum(bookgrams) BookGrams, sum(wd) wd, " +
                                           "Sum(Adjsqm) Adjsqm, isnull(Sum(AdjTons),0) AdjTons, Sum(AdjCont) AdjCont " +
                                           " from( ";
            }

            if (barShowInfo.EditValue.Equals("Mine Manager"))
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select pm sectionid, pmname [SectionName], '" + string.Empty + "' workplaceid, '" + string.Empty + "' description, theDate, " +
                                            "sum(plansqm) PlanSqm,  sum(planadv) PlanAdv, sum(plantons) PlanTons " +
                                            ", sum(plangrams) PlanGrams, " +
                                            "sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons " +
                                            ", sum(bookgrams) BookGrams, sum(wd) wd, " +
                                            "Sum(Adjsqm) Adjsqm, isnull(Sum(AdjTons),0) AdjTons, Sum(AdjCont) AdjCont " +
                                            " from( ";
            }

            if (barShowInfo.EditValue.Equals("Mining Manager"))
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select um sectionid, umname sectionname,'" + string.Empty + "' workplaceid, '" + string.Empty + "' description, thedate, " +
                                            "sum(plansqm) PlanSqm, sum(planadv) PlanAdv, sum(plantons) PlanTons " +
                                            ", sum(plangrams) PlanGrams, " +
                                            "sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons " +
                                            ", sum(bookgrams) BookGrams, sum(wd) wd, " +
                                            "Sum(Adjsqm) Adjsqm, isnull(Sum(AdjTons),0) AdjTons, Sum(AdjCont) AdjCont " +
                                            " from( ";
            }

            if (barShowInfo.EditValue.Equals("Mine Overseer"))
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select mo sectionid, moname sectionname,'" + string.Empty + "' workplaceid, '" + string.Empty + "' description, thedate, " +
                                            "sum(plansqm) PlanSqm, sum(planadv) PlanAdv, sum(plantons) PlanTons " +
                                            ", sum(plangrams) PlanGrams, " +
                                            "sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons " +
                                            ", sum(bookgrams) BookGrams, sum(wd) wd, " +
                                            "Sum(Adjsqm) Adjsqm,  isnull(Sum(AdjTons),0) AdjTons, Sum(AdjCont) AdjCont " +
                                            " from(";
            }


            if (barShowInfo.EditValue.Equals("Shiftboss"))
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select sb sectionid, sbname sectionname, '" + string.Empty + "' workplaceid, '" + string.Empty + "' description, thedate, " +
                                            " sum(plansqm) PlanSqm, sum(planadv) PlanAdv, sum(plantons) PlanTons " +
                                            ", sum(plangrams) PlanGrams, " +
                                            " sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons " +
                                            ", sum(bookgrams) Bookgrams, sum(wd) wd, " +
                                            "Sum(Adjsqm) Adjsqm,  isnull(Sum(AdjTons),0) AdjTons, Sum(AdjCont) AdjCont " +
                                            " from(";
            }


            if (barShowInfo.EditValue.Equals("Miner"))
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select sectionid, conname sectionname,  '" + string.Empty + "' workplaceid, '" + string.Empty + "' description, thedate, " +
                                            "sum(plansqm) PlanSqm, sum(planadv) PlanAdv, sum(plantons) PlanTons " +
                                            ", sum(plangrams) PlanGrams, " +
                                            " sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons " +
                                            ", sum(bookgrams) BookGrams, sum(wd) wd, " +
                                            "Sum(Adjsqm) Adjsqm,  isnull(Sum(AdjTons),0) AdjTons, Sum(AdjCont) AdjCont " +
                                            " from(";

            }

            _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select b.*, w.description, s.reporttosectionid sb, s1.name sbname, s1.reporttosectionid mo " +
                                        ",s2.reporttosectionid um, s3.reporttosectionid pm, " +
                                        " s.name conname, s2.name moname " +
                                        ", s3.name umname, s4.name pmname, s.name sectionname " +
                                        "from (select case when workingday = 'Y' then 1 else 0 end as wd, " +
                                        " '' activity, p.BookAdv BookAdv, convert(decimal(10,0),p.BookSqm) BookSqm , p.BookTons BookTons, p.BookContent BookGrams, " +
                                        "p.sectionid, " +
                                        "p.workplaceid, " +
                                        "p.prodmonth, " +
                                        "p.calendardate TheDate, " +
                                        "convert(decimal(10,0),p.Plansqm) PlanSqm, p.Planadv PlanAdv, " +
                                        "p.Plantons PlanTons, p.Plancontent PlanGrams, 0 AdjSqm, 0 AdjTons, 0 AdjCont from [PLANNING_Vamping] p  " +
                                        " " +
                                        " ";

            if (Typergp.EditValue.ToString() == stp)
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "where p.prodmonth = '" + month + "' and pm.sqmtotal > 0 ) b left outer join ";
            }
            else
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "where p.prodmonth = '" + month + "'  ) b left outer join";
            }

            _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " workplace w on b.workplaceid = w.workplaceid " +
                                       " " +
                                       "left outer join tbl_section s on b.sectionid = s.sectionid and " +
                                       "b.prodmonth = s.prodmonth " +
                                       "left outer join tbl_section s1 on s.reporttosectionid = s1.sectionid and " +
                                       "s.prodmonth = s1.prodmonth " +
                                       "left outer join tbl_section s2 on s1.reporttosectionid = s2.sectionid and " +
                                       "s1.prodmonth = s2.prodmonth " +
                                       "left outer join tbl_section s3 on s2.reporttosectionid = s3.sectionid and " +
                                       "s2.prodmonth = s3.prodmonth " +
                                       "left outer join tbl_section s4 on s3.reporttosectionid = s4.sectionid and " +
                                       "s3.prodmonth = s4.prodmonth" +
                                        "left outer join tbl_section s5 on s4.reporttosectionid = s5.sectionid and " +
                                      "s4.prodmonth = s5.prodmonth";

            if (Typergp.EditValue.ToString() == stp)
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " where ";
            }
            else
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " where ";
            }
            if (Hier == "0")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " s5.reporttosectionid = '" + sec + "'";
            }

            if (Hier == "1")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " s4.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "2")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " s3.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "3")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " s2.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "4")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " s1.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "5")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " s.reporttosectionid = '" + sec + "'";
            }
            if (Hier == "6")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " s.sectionid = '" + sec + "'";
            }

            if (barShowInfo.EditValue.ToString() == "Total Mine")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by thedate order by thedate ";
            }

            if (barShowInfo.EditValue.ToString() == "Mine Manager")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by pm, pmname, thedate order by pm , thedate ";
            }

            if (barShowInfo.EditValue.ToString() == "Mining Manager")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by um, umname, thedate order by um , thedate  ";
            }
            if (barShowInfo.EditValue.ToString() == "Mine Overseer")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by mo, moname, thedate order by mo , thedate ";
            }

            if (barShowInfo.EditValue.ToString() == "Shiftboss")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by sb, sbname, thedate order by sb , thedate";
            }
            if (barShowInfo.EditValue.ToString() == "Miner")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by sectionid, conname, thedate order by sectionid , thedate";
            }
            if (barShowInfo.EditValue.ToString() == "Workplace")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "order by b.sectionid, conname , w.description, b.Activity,  b.thedate";
            }

            _dbManDaily.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDaily.queryReturnType = MWDataManager.ReturnType.DataTable;


            _dbManDaily.ExecuteInstruction();






            DataTable dtDaily = _dbManDaily.ResultsDataTable;
            gridDaily.RowCount = 500;
            gridProgressive.RowCount = 500;


            for (int i = 0; i < gridDaily.ColumnCount; i++)
            {
                for (int j = 0; j < 499; j++)
                {
                    gridDaily.Rows[j].Cells[i].Value = string.Empty;
                    gridProgressive.Rows[j].Cells[i].Value = string.Empty;

                    gridDaily.Rows[494].Cells[i].Value = "0";
                    gridProgressive.Rows[494].Cells[i].Value = "0";
                    gridDaily.Rows[495].Cells[i].Value = "0";
                    gridProgressive.Rows[495].Cells[i].Value = "0";
                    gridDaily.Rows[496].Cells[i].Value = "0";
                    gridProgressive.Rows[496].Cells[i].Value = "0";
                    gridDaily.Rows[497].Cells[i].Value = "0";
                    gridProgressive.Rows[497].Cells[i].Value = "0";
                    gridDaily.Rows[498].Cells[i].Value = "0";
                    gridProgressive.Rows[498].Cells[i].Value = "0";
                    gridDaily.Rows[499].Cells[i].Value = "0";
                    gridProgressive.Rows[499].Cells[i].Value = "0";

                }
            }



            // int col = 5;
            int row = 0;

            int nextRow = row + 1;
            int ProgPlan = 0;
            int ProgBook = 0;
            int TotPlan = 0;
            int TotBook = 0;
            int totRow = nextRow + 1;
            int totRownext = totRow + 1;

            decimal ProgPlanAdv = 0;
            decimal ProgBookAdv = 0;

            decimal ProgPlanTons = 0;
            decimal ProgBookTons = 0;

            decimal ProgPlanGrams = 0;
            decimal ProgBookGrams = 0;

            double ProgPlanOunces = 0;
            double ProgBookOunces = 0;

            string group = string.Empty;

            ////if (dtDaily.Rows.Count > 0)
            ////{

            if (barShowInfo.EditValue.ToString() == "Workplace")
            {
                group = dtDaily.Rows[0]["workplaceid"].ToString() + dtDaily.Rows[0]["activity"].ToString();
                gridDaily.Rows[row].Cells[0].Value = dtDaily.Rows[0]["SectionID"].ToString();
                gridDaily.Rows[nextRow].Cells[0].Value = dtDaily.Rows[0]["SectionName"].ToString();
                gridDaily.Rows[row].Cells[1].Value = dtDaily.Rows[0]["workplaceid"].ToString();
                gridDaily.Rows[row].Cells[2].Value = dtDaily.Rows[0]["description"].ToString();
                gridDaily.Rows[row].Cells[3].Value = "Plan";
                gridDaily.Rows[nextRow].Cells[3].Value = "Book";
                gridDaily.Rows[totRow].Cells[0].Value = "Total";
                gridDaily.Rows[totRow].Cells[3].Value = "Plan";
                gridDaily.Rows[totRownext].Cells[3].Value = "Book";

                ////////////////////////////////Progressive Grid/////////////////////////////
                gridProgressive.Rows[row].Cells[0].Value = dtDaily.Rows[0]["SectionID"].ToString();
                gridProgressive.Rows[nextRow].Cells[0].Value = dtDaily.Rows[0]["SectionName"].ToString();
                gridProgressive.Rows[row].Cells[1].Value = dtDaily.Rows[0]["workplaceid"].ToString();
                gridProgressive.Rows[row].Cells[2].Value = dtDaily.Rows[0]["description"].ToString();
                gridProgressive.Rows[row].Cells[3].Value = "Plan";
                gridProgressive.Rows[nextRow].Cells[3].Value = "Book";
                gridProgressive.Rows[totRow].Cells[0].Value = "Total";
                gridProgressive.Rows[totRow].Cells[3].Value = "Plan";
                gridProgressive.Rows[totRownext].Cells[3].Value = "Book";

            }

            else
            {
                group = dtDaily.Rows[0]["sectionid"].ToString();
                gridDaily.Rows[row].Cells[0].Value = dtDaily.Rows[0]["SectionID"].ToString();
                gridDaily.Rows[nextRow].Cells[0].Value = dtDaily.Rows[0]["SectionName"].ToString();
                gridDaily.Rows[row].Cells[1].Value = dtDaily.Rows[0]["workplaceid"].ToString();
                gridDaily.Rows[row].Cells[2].Value = dtDaily.Rows[0]["description"].ToString();
                gridDaily.Rows[row].Cells[3].Value = "Plan";
                gridDaily.Rows[nextRow].Cells[3].Value = "Book";
                gridDaily.Rows[totRow].Cells[0].Value = "Total";
                gridDaily.Rows[totRow].Cells[3].Value = "Plan";
                gridDaily.Rows[totRownext].Cells[3].Value = "Book";


                /////////////////////Progressive Grid///////////////////////

                gridProgressive.Rows[row].Cells[0].Value = dtDaily.Rows[0]["SectionID"].ToString();
                gridProgressive.Rows[nextRow].Cells[0].Value = dtDaily.Rows[0]["SectionName"].ToString();
                gridProgressive.Rows[row].Cells[1].Value = dtDaily.Rows[0]["workplaceid"].ToString();
                gridProgressive.Rows[row].Cells[2].Value = dtDaily.Rows[0]["description"].ToString();
                gridProgressive.Rows[row].Cells[3].Value = "Plan";
                gridProgressive.Rows[nextRow].Cells[3].Value = "Book";
                gridProgressive.Rows[totRow].Cells[0].Value = "Total";
                gridProgressive.Rows[totRow].Cells[3].Value = "Plan";
                gridProgressive.Rows[totRownext].Cells[3].Value = "Book";
            }


            TimeSpan Span;



            foreach (DataRow drDaily in dtDaily.Rows)
            {
                Span = Convert.ToDateTime(drDaily["thedate"].ToString()).Subtract(dtpStartDate.Value);

                col = Convert.ToInt32(Span.Days) + 5;
                if (col > col1)
                    col1 = col;
                //TotPlan = 0;
                //TotBook = 0;

                if (barShowInfo.EditValue.ToString() == "Workplace")
                {
                    if (drDaily["workplaceid"].ToString() + drDaily["activity"].ToString() != group)
                    {

                        row = row + 2;
                        nextRow = nextRow + 2;
                        totRow = totRow + 2;
                        totRownext = totRownext + 2;
                        gridDaily.Rows[row].Cells[0].Value = drDaily["SectionID"].ToString();
                        gridDaily.Rows[nextRow].Cells[0].Value = drDaily["SectionName"].ToString();

                        gridDaily.Rows[row].Cells[1].Value = drDaily["workplaceid"].ToString();
                        gridDaily.Rows[row].Cells[2].Value = drDaily["description"].ToString();
                        gridDaily.Rows[row].Cells[3].Value = "Plan";
                        gridDaily.Rows[nextRow].Cells[3].Value = "Book";

                        gridProgressive.Rows[row].Cells[0].Value = drDaily["SectionID"].ToString();
                        gridProgressive.Rows[nextRow].Cells[0].Value = drDaily["SectionName"].ToString();

                        gridProgressive.Rows[row].Cells[1].Value = drDaily["workplaceid"].ToString();
                        gridProgressive.Rows[row].Cells[2].Value = drDaily["description"].ToString();
                        gridProgressive.Rows[row].Cells[3].Value = "Plan";
                        gridProgressive.Rows[nextRow].Cells[3].Value = "Book";






                        ProgPlan = 0;
                        ProgBook = 0;
                        ProgPlanAdv = 0;
                        ProgBookAdv = 0;
                        ProgPlanTons = 0;
                        ProgBookTons = 0;
                        ProgPlanGrams = 0;
                        ProgBookGrams = 0;
                        ProgPlanOunces = 0;
                        ProgBookOunces = 0;


                        group = drDaily["workplaceid"].ToString() + drDaily["activity"].ToString();

                    }


                }
                else
                {
                    if (drDaily["SectionID"].ToString() != group)
                    {
                        row = row + 2;
                        nextRow = nextRow + 2;
                        totRow = totRow + 2;
                        totRownext = totRownext + 2;
                        gridDaily.Rows[row].Cells[0].Value = drDaily["SectionID"].ToString();
                        gridDaily.Rows[nextRow].Cells[0].Value = drDaily["SectionName"].ToString();

                        gridDaily.Rows[row].Cells[1].Value = drDaily["workplaceid"].ToString();
                        gridDaily.Rows[row].Cells[2].Value = drDaily["description"].ToString();
                        gridDaily.Rows[row].Cells[3].Value = "Plan";
                        gridDaily.Rows[nextRow].Cells[3].Value = "Book";

                        /////////////////////////////progressive Book////////////////////////
                        gridProgressive.Rows[row].Cells[0].Value = drDaily["SectionID"].ToString();
                        gridProgressive.Rows[nextRow].Cells[0].Value = drDaily["SectionName"].ToString();

                        gridProgressive.Rows[row].Cells[1].Value = drDaily["workplaceid"].ToString();
                        gridProgressive.Rows[row].Cells[2].Value = drDaily["description"].ToString();
                        gridProgressive.Rows[row].Cells[3].Value = "Plan";
                        gridProgressive.Rows[nextRow].Cells[3].Value = "Book";

                        ProgPlan = 0;
                        ProgBook = 0;
                        ProgPlanAdv = 0;
                        ProgBookAdv = 0;
                        ProgPlanTons = 0;
                        ProgBookTons = 0;
                        ProgPlanGrams = 0;
                        ProgBookGrams = 0;
                        ProgPlanOunces = 0;
                        ProgBookOunces = 0;


                        group = drDaily["SectionID"].ToString();
                    }


                }


                if (drDaily["wd"].ToString() == "0")
                {
                    gridDaily[col, row].Style.BackColor = Color.Gainsboro;
                    gridDaily[col, row].Style.ForeColor = Color.Gainsboro;
                    gridDaily[col, nextRow].Style.BackColor = Color.Gainsboro;

                    /////////Progressive Grid//////////////////
                    gridProgressive[col, row].Style.BackColor = Color.Gainsboro;
                    gridProgressive[col, row].Style.ForeColor = Color.Gainsboro;
                    gridProgressive[col, nextRow].Style.BackColor = Color.Gainsboro;
                    gridProgressive[col, nextRow].Style.ForeColor = Color.Gainsboro;
                }

                int prog = 0;
                int progBook1 = 0;

                ////////////SQM////////////////
                if (Showrgb.EditValue.ToString() == sqm)
                {

                    if ((drDaily["PlanSqm"].ToString() == string.Empty) | (drDaily["PlanSqm"].ToString() == null))
                    {
                        gridDaily.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = string.Empty;


                    }
                    else
                    {
                        if (gridProgressive.Rows[row].Cells[col - 1].Value.ToString() == string.Empty)
                            prog = 0;
                        else
                            prog = Convert.ToInt32(gridProgressive.Rows[row].Cells[col - 1].Value);

                        gridDaily.Rows[row].Cells[col].Value = drDaily["PlanSqm"].ToString();
                        gridProgressive.Rows[row].Cells[col].Value = (Convert.ToInt32(drDaily["PlanSqm"].ToString()) + prog).ToString();
                        ProgPlan = ProgPlan + Convert.ToInt32(drDaily["PlanSqm"].ToString());
                        //TotPlan = TotPlan + Convert.ToInt32(drDaily["PlanSqm"].ToString());

                        if (gridDaily.Rows[497].Cells[col].Value.ToString() == string.Empty)
                            gridDaily.Rows[497].Cells[col].Value = "0";

                        if (gridDaily.Rows[494].Cells[4].Value.ToString() == string.Empty)
                            gridDaily.Rows[494].Cells[4].Value = "0";


                        if (gridProgressive.Rows[497].Cells[col - 1].Value.ToString() == string.Empty)
                            gridProgressive.Rows[497].Cells[col - 1].Value = "0";

                        if (gridProgressive.Rows[494].Cells[4].Value.ToString() == string.Empty)
                            gridProgressive.Rows[494].Cells[4].Value = "0";


                        gridDaily.Rows[497].Cells[col].Value = Convert.ToInt32(gridDaily.Rows[497].Cells[col].Value) + Convert.ToInt32(drDaily["PlanSqm"].ToString());
                        gridDaily.Rows[494].Cells[4].Value = Convert.ToInt32(gridDaily.Rows[494].Cells[4].Value) + Convert.ToInt32(drDaily["PlanSqm"].ToString());

                        gridProgressive.Rows[497].Cells[col].Value = Convert.ToInt32(gridProgressive.Rows[497].Cells[col - 1].Value) + Convert.ToInt32(drDaily["PlanSqm"].ToString());
                        gridProgressive.Rows[494].Cells[4].Value = Convert.ToInt32(gridProgressive.Rows[494].Cells[4].Value) + Convert.ToInt32(drDaily["PlanSqm"].ToString());

                    }

                    if ((drDaily["BookSqm"].ToString() == string.Empty) | (drDaily["BookSqm"].ToString() == null))
                    {
                        gridDaily.Rows[nextRow].Cells[col].Value = string.Empty;


                        if (gridProgressive.Rows[row].Cells[col - 1].Value.ToString() == string.Empty)
                            progBook1 = 0;
                        else
                            progBook1 = Convert.ToInt32(gridProgressive.Rows[nextRow].Cells[col - 1].Value);


                        gridProgressive.Rows[nextRow].Cells[col].Value = Convert.ToInt32(0) + progBook1;
                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToInt32(0) + progBook1;
                    }
                    else
                    {
                        //if (adjBook == "Y")
                        //{

                        if (gridProgressive.Rows[nextRow].Cells[col - 1].Value.ToString() == string.Empty)
                            gridProgressive.Rows[nextRow].Cells[col - 1].Value = "0";

                        gridDaily.Rows[nextRow].Cells[col].Value = Convert.ToString(Convert.ToInt32(drDaily["BookSqm"].ToString()));
                        gridProgressive.Rows[nextRow].Cells[col].Value = Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(drDaily["AdjSqm"].ToString()) + Convert.ToInt32(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        ProgBook = ProgBook + Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(drDaily["AdjSqm"].ToString());

                        gridDaily.Rows[498].Cells[col].Value = Convert.ToInt32(gridDaily.Rows[498].Cells[col].Value) + Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(drDaily["AdjSqm"].ToString());
                        gridDaily.Rows[495].Cells[4].Value = Convert.ToInt32(gridDaily.Rows[495].Cells[4].Value) + Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(drDaily["AdjSqm"].ToString());

                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(drDaily["AdjSqm"].ToString()) + Convert.ToInt32(gridProgressive.Rows[498].Cells[col - 1].Value);
                        gridProgressive.Rows[495].Cells[4].Value = Convert.ToInt32(gridProgressive.Rows[495].Cells[4].Value) + Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(drDaily["AdjSqm"].ToString());
                        //}
                        //else
                        //{
                        //    gridDaily.Rows[nextRow].Cells[col].Value = drDaily["BookSqm"].ToString();
                        //    gridProgressive.Rows[nextRow].Cells[col].Value = Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        //    ProgBook = ProgBook + Convert.ToInt32(drDaily["BookSqm"].ToString());
                        //    //TotBook = TotBook + Convert.ToInt32(drDaily["BookSqm"].ToString());
                        //    gridDaily.Rows[498].Cells[col].Value = Convert.ToInt32(gridDaily.Rows[498].Cells[col].Value) + Convert.ToInt32(drDaily["BookSqm"].ToString());
                        //    gridDaily.Rows[495].Cells[4].Value = Convert.ToInt32(gridDaily.Rows[495].Cells[4].Value) + Convert.ToInt32(drDaily["BookSqm"].ToString());

                        //    gridProgressive.Rows[498].Cells[col].Value = Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(gridProgressive.Rows[498].Cells[col - 1].Value);
                        //    gridProgressive.Rows[495].Cells[4].Value = Convert.ToInt32(gridProgressive.Rows[495].Cells[4].Value) + Convert.ToInt32(drDaily["BookSqm"].ToString());
                        //}
                    }

                    gridDaily.Rows[row].Cells[4].Value = ProgPlan.ToString();
                    gridDaily.Rows[nextRow].Cells[4].Value = ProgBook.ToString();

                    gridProgressive.Rows[row].Cells[4].Value = ProgPlan.ToString();
                    gridProgressive.Rows[nextRow].Cells[4].Value = ProgBook.ToString();
                }

                ////////////////Meters//////////////////////////////////////
                if (Showrgb.EditValue.Equals(mts))
                {

                    if ((drDaily["PlanAdv"].ToString() == string.Empty) | (drDaily["PlanAdv"].ToString() == null))
                    {
                        gridDaily.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = Convert.ToDecimal(0) + Convert.ToDecimal(gridProgressive.Rows[row].Cells[col - 1].Value);

                    }
                    else
                    {
                        if (gridProgressive.Rows[row].Cells[col - 1].Value.ToString() == string.Empty)
                            prog = 0;
                        else
                            prog = Convert.ToInt32(gridProgressive.Rows[row].Cells[col - 1].Value);

                        gridDaily.Rows[row].Cells[col].Value = drDaily["PlanAdv"].ToString();
                        gridProgressive.Rows[row].Cells[col].Value = Convert.ToDecimal(drDaily["PlanAdv"].ToString()) + prog;
                        ProgPlanAdv = ProgPlanAdv + Convert.ToDecimal(drDaily["PlanAdv"].ToString());

                        gridDaily.Rows[497].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[497].Cells[col].Value) + Convert.ToDecimal(drDaily["PlanAdv"].ToString());
                        gridDaily.Rows[494].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[494].Cells[4].Value) + Convert.ToDecimal(drDaily["PlanAdv"].ToString());

                        gridProgressive.Rows[497].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[497].Cells[col - 1].Value) + Convert.ToDecimal(drDaily["PlanAdv"].ToString());
                        gridProgressive.Rows[494].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[494].Cells[4].Value) + Convert.ToDecimal(drDaily["PlanAdv"].ToString());

                    }

                    if ((drDaily["BookAdv"].ToString() == string.Empty) | (drDaily["BookAdv"].ToString() == null))
                    {
                        if (gridProgressive.Rows[row].Cells[col - 1].Value.ToString() == string.Empty)
                            progBook1 = 0;
                        else
                            progBook1 = Convert.ToInt32(gridProgressive.Rows[nextRow].Cells[col - 1].Value);

                        gridDaily.Rows[nextRow].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[nextRow].Cells[col].Value = Convert.ToDecimal(0) + progBook1;
                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(0) + progBook1;
                    }
                    else
                    {
                        gridDaily.Rows[nextRow].Cells[col].Value = drDaily["BookAdv"].ToString();
                        gridProgressive.Rows[nextRow].Cells[col].Value = Convert.ToDecimal(drDaily["BookAdv"].ToString()) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        ProgBookAdv = ProgBookAdv + Convert.ToDecimal(drDaily["BookAdv"].ToString());
                        gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Convert.ToDecimal(drDaily["BookAdv"].ToString());
                        gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Convert.ToDecimal(drDaily["BookAdv"].ToString());

                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Convert.ToDecimal(gridProgressive.Rows[498].Cells[col - 1].Value) + Convert.ToDecimal(drDaily["BookAdv"].ToString());
                        gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Convert.ToDecimal(drDaily["BookAdv"].ToString());
                    }

                    gridDaily.Rows[row].Cells[4].Value = ProgPlanAdv.ToString();
                    gridDaily.Rows[nextRow].Cells[4].Value = ProgBookAdv.ToString();

                    gridProgressive.Rows[row].Cells[4].Value = ProgPlanAdv.ToString();
                    gridProgressive.Rows[nextRow].Cells[4].Value = ProgBookAdv.ToString();

                }


                ///////////Tons/////////////////////
                if (Showrgb.EditValue.Equals(ton))
                {

                    if ((drDaily["PlanTons"].ToString() == string.Empty) | (drDaily["PlanTons"].ToString() == null))
                    {
                        gridDaily.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(0), 0) + Convert.ToDecimal(gridProgressive.Rows[row].Cells[col - 1].Value);

                    }
                    else
                    {
                        if (gridProgressive.Rows[row].Cells[col - 1].Value.ToString() == string.Empty)
                            prog = 0;
                        else
                            prog = Convert.ToInt32(gridProgressive.Rows[row].Cells[col - 1].Value);

                        gridDaily.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanTons"]), 0).ToString();
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanTons"].ToString()), 0) + prog;
                        ProgPlanTons = ProgPlanTons + Math.Round(Convert.ToDecimal(drDaily["PlanTons"].ToString()), 0);

                        gridDaily.Rows[497].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[497].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanTons"].ToString()), 0);
                        gridDaily.Rows[494].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanTons"].ToString()), 0);

                        gridProgressive.Rows[497].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[497].Cells[col - 1].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanTons"].ToString()), 0);
                        gridProgressive.Rows[494].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanTons"].ToString()), 0);

                    }

                    if ((drDaily["BookTons"].ToString() == string.Empty) | (drDaily["BookTons"].ToString() == null))
                    {
                        if (gridProgressive.Rows[row].Cells[col - 1].Value.ToString() == string.Empty)
                            progBook1 = 0;
                        else
                            progBook1 = Convert.ToInt32(gridProgressive.Rows[nextRow].Cells[col - 1].Value);

                        gridDaily.Rows[nextRow].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(0), 0) + progBook1;
                        gridProgressive.Rows[498].Cells[col].Value = Math.Round(Convert.ToDecimal(0), 0) + progBook1;
                    }
                    else
                    {
                        //if (adjBook == "Y")
                        //{
                        if (gridProgressive.Rows[nextRow].Cells[col - 1].Value.ToString() == string.Empty)
                            gridProgressive.Rows[nextRow].Cells[col - 1].Value = "0";
                        gridDaily.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookTons"]));
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()) + Convert.ToDecimal(drDaily["AdjTons"].ToString()), 0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        ProgBookTons = ProgBookTons + Math.Round((Convert.ToDecimal(drDaily["BookTons"].ToString()) + Convert.ToDecimal(drDaily["AdjTons"].ToString())), 0);

                        gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()) + Convert.ToDecimal(drDaily["AdjTons"].ToString()), 0);
                        gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()) + Convert.ToDecimal(drDaily["AdjTons"].ToString()), 0);

                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Convert.ToDecimal(gridProgressive.Rows[498].Cells[col - 1].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()) + Convert.ToDecimal(drDaily["AdjTons"].ToString()), 0);
                        gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()) + Convert.ToDecimal(drDaily["AdjTons"].ToString()), 0);
                        //}
                        //else
                        //{
                        //    gridDaily.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookTons"]), 0).ToString();
                        //    gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        //    ProgBookTons = ProgBookTons + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0);

                        //    gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0);
                        //    gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Convert.ToDecimal(drDaily["BookTons"].ToString());

                        //    gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Convert.ToDecimal(gridProgressive.Rows[498].Cells[col - 1].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0);
                        //    gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0);
                        //}
                    }

                    gridDaily.Rows[row].Cells[4].Value = ProgPlanTons.ToString();
                    gridDaily.Rows[nextRow].Cells[4].Value = ProgBookTons.ToString();

                    gridProgressive.Rows[row].Cells[4].Value = ProgPlanTons.ToString();
                    gridProgressive.Rows[nextRow].Cells[4].Value = ProgBookTons.ToString();

                }

                ///////////////Kilograms//////////////////
                if (Showrgb.EditValue.Equals(kg))
                {

                    if ((drDaily["PlanGrams"].ToString() == string.Empty) | (drDaily["PlanGrams"].ToString() == null))
                    {
                        gridDaily.Rows[row].Cells[col].Value = string.Empty;

                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(0), 1) + Convert.ToDecimal(gridProgressive.Rows[row].Cells[col - 1].Value);

                    }
                    else
                    {
                        if (gridProgressive.Rows[row].Cells[col - 1].Value.ToString() == string.Empty)
                            prog = 0;
                        else
                            prog = Convert.ToInt32(gridProgressive.Rows[row].Cells[col - 1].Value);

                        gridDaily.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 1); ;
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 1) + prog;
                        ProgPlanGrams = Math.Round(ProgPlanGrams + Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 3);


                        gridDaily.Rows[497].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[497].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 1);
                        gridDaily.Rows[494].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 1);

                        gridProgressive.Rows[497].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[497].Cells[col - 1].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 1);
                        gridProgressive.Rows[494].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 1);

                    }

                    if ((drDaily["BookGrams"].ToString() == string.Empty) | (drDaily["BookGrams"].ToString() == null))
                    {
                        if (gridProgressive.Rows[row].Cells[col - 1].Value.ToString() == string.Empty)
                            progBook1 = 0;
                        else
                            progBook1 = Convert.ToInt32(gridProgressive.Rows[nextRow].Cells[col - 1].Value);


                        gridDaily.Rows[nextRow].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(0), 1) + progBook1;
                        gridProgressive.Rows[498].Cells[col].Value = Math.Round(Convert.ToDecimal(0), 1) + progBook1;
                    }
                    else
                    {
                        //if (adjBook == "Y")
                        //{
                        if (gridProgressive.Rows[nextRow].Cells[col - 1].Value.ToString() == string.Empty)
                            gridProgressive.Rows[nextRow].Cells[col - 1].Value = "0";
                        gridDaily.Rows[nextRow].Cells[col].Value = Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString())) / 1000, 1);
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString()) + Convert.ToDecimal(drDaily["AdjCont"].ToString())) / 1000, 1) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        ProgBookGrams = ProgBookGrams + Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString()) + Convert.ToDecimal(drDaily["AdjCont"].ToString())) / 1000, 3);


                        gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString()) + Convert.ToDecimal(drDaily["AdjCont"].ToString())) / 1000, 1);
                        gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString()) + Convert.ToDecimal(drDaily["AdjCont"].ToString())) / 1000, 1);

                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Convert.ToDecimal(gridProgressive.Rows[498].Cells[col - 1].Value) + Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString()) + Convert.ToDecimal(drDaily["AdjCont"].ToString())) / 1000, 1);
                        gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString()) + Convert.ToDecimal(drDaily["AdjCont"].ToString())) / 1000, 1);
                        //}
                        //else
                        //{
                        //    gridDaily.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 1);
                        //    gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 1) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        //    ProgBookGrams = ProgBookGrams + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 3);

                        //    gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 1);
                        //    gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 1);

                        //    gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Convert.ToDecimal(gridProgressive.Rows[498].Cells[col - 1].Value) + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 1);
                        //    gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 1);
                        //}
                    }

                    gridDaily.Rows[row].Cells[4].Value = Math.Round(ProgPlanGrams, 1);
                    gridDaily.Rows[nextRow].Cells[4].Value = Math.Round(ProgBookGrams, 1);

                    gridProgressive.Rows[row].Cells[4].Value = Math.Round(ProgPlanGrams, 1);
                    gridProgressive.Rows[nextRow].Cells[4].Value = Math.Round(ProgBookGrams, 1);

                }

            }

            gridDaily.Rows[totRow].DefaultCellStyle.BackColor = Color.LightGray;
            gridDaily.Rows[totRownext].DefaultCellStyle.BackColor = Color.LightGray;

            gridDaily.Rows[totRow].Cells[0].Value = "Total";
            gridDaily.Rows[totRow].Cells[3].Value = "Plan";
            gridDaily.Rows[totRownext].Cells[0].Value = sec + ":" + SectionName;
            gridDaily.Rows[totRownext].Cells[3].Value = "Book";

            gridDaily.Rows[totRow].Cells[4].Value = gridDaily.Rows[494].Cells[4].Value;
            gridDaily.Rows[totRownext].Cells[4].Value = gridDaily.Rows[495].Cells[4].Value;

            Decimal Plan = 0;
            Decimal Book = 0;

            for (int x = 5; x <= col1; x++)
            {
                gridDaily.Rows[totRow].Cells[x].Value = gridDaily.Rows[497].Cells[x].Value;
                gridDaily.Rows[totRownext].Cells[x].Value = gridDaily.Rows[498].Cells[x].Value;
                if (gridDaily.Rows[totRow].Cells[x].Value != null)
                {
                    Plan = Plan + Convert.ToDecimal(gridDaily.Rows[totRow].Cells[x].Value);
                }
                if (gridDaily.Rows[totRownext].Cells[x].Value != null)
                {
                    Book = Book + Convert.ToDecimal(gridDaily.Rows[totRownext].Cells[x].Value);
                }
                if (Showrgb.EditValue.Equals(stp))
                {
                    gridProgressive.Rows[totRow].Cells[x].Value = Math.Round(Plan, 0);
                    gridProgressive.Rows[totRownext].Cells[x].Value = Math.Round(Book, 0);
                }
                if (Showrgb.EditValue.Equals(mts))
                {
                    gridProgressive.Rows[totRow].Cells[x].Value = Math.Round(Plan, 2);
                    gridProgressive.Rows[totRownext].Cells[x].Value = Math.Round(Book, 2);
                }
                if (Showrgb.EditValue.Equals(ton))
                {
                    gridProgressive.Rows[totRow].Cells[x].Value = Math.Round(Plan, 0);
                    gridProgressive.Rows[totRownext].Cells[x].Value = Math.Round(Book, 0);
                }
                if (Showrgb.EditValue.Equals(kg))
                {
                    gridProgressive.Rows[totRow].Cells[x].Value = Math.Round(Plan, 1);
                    gridProgressive.Rows[totRownext].Cells[x].Value = Math.Round(Book, 1);
                }

            }

            //////////Progresive Grid//////////////
            gridProgressive.Rows[totRow].DefaultCellStyle.BackColor = Color.LightGray;
            gridProgressive.Rows[totRownext].DefaultCellStyle.BackColor = Color.LightGray;

            gridProgressive.Rows[totRow].Cells[0].Value = "Total";
            gridProgressive.Rows[totRow].Cells[3].Value = "Plan";

            gridProgressive.Rows[totRownext].Cells[0].Value = sec + ":" + SectionName;
            gridProgressive.Rows[totRownext].Cells[3].Value = "Book";


            gridProgressive.Rows[totRow].Cells[4].Value = gridProgressive.Rows[494].Cells[4].Value;
            gridProgressive.Rows[totRownext].Cells[4].Value = gridProgressive.Rows[495].Cells[4].Value;



            gridDaily.RowCount = nextRow + 3;
            gridProgressive.RowCount = nextRow + 3;
            //gridDaily.RowCount = totRownext +1;
            gridDaily.Visible = true;
            gridProgressive.Visible = true;
            gridDaily.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            gridProgressive.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;



            //////
        }

        void LoadDaily()
        {

            gridDaily.Visible = false;
            gridProgressive.Visible = false;
            SectionName = procs.ExtractAfterColon(barSection.EditValue.ToString());
            //sec = procs.ExtractBeforeColon(barSection.Text);

            MWDataManager.clsDataAccess _dbManDaily = new MWDataManager.clsDataAccess();
            _dbManDaily.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            if (barShowInfo.EditValue.ToString() == "Total Mine")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " Select GM,'" + sec + "' [SectionId] , GM [SectionName] ,'" + string.Empty + "' workplaceid, '" + string.Empty + "' description,theDate,  \r\n" +
                                           "Sum(plansqm) PlanSqm, Sum(planadv) PlanAdv, Sum(planTons) PlanTons, \r\n" +
                                           "Sum(plangrams) PlanGrams,  \r\n" +
                                           "Sum(booksqm) BookSqm, Sum(bookadv) BookAdv, Sum(booktons) BookTons, \r\n" +
                                           "Sum(bookgrams) BookGrams, sum(wd) wd, \r\n" +
                                           "Sum(Adjsqm) Adjsqm, Sum(AdjTons) AdjTons, Sum(AdjCont) AdjCont, \r\n" +
                                           " sum(ProgPlansqm) ProgPlansqm, sum(ProgBooksqm) ProgBooksqm, sum(ProgPlanadv) ProgPlanadv, sum(ProgBookadv) ProgBookadv, sum(ProgPlanTons) ProgPlanTons, sum(ProgBookTons) ProgBookTons, sum(ProgPlanCont) ProgPlanCont, sum(ProgBookCont) ProgBookCont \r\n" +
                                           " from( ";
            }

            if (barShowInfo.EditValue.ToString() == "Mine Manager")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select MineManager, pm sectionid, MineManager [SectionName], '" + string.Empty + "' workplaceid, '" + string.Empty + "' description, theDate, \r\n" +
                                            "sum(plansqm) PlanSqm,  sum(planadv) PlanAdv, sum(plantons) PlanTons \r\n" +
                                            ", sum(plangrams) PlanGrams, \r\n" +
                                            "sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons \r\n" +
                                            ", sum(bookgrams) BookGrams, sum(wd) wd, \r\n" +
                                            "Sum(Adjsqm) Adjsqm, Sum(AdjTons) AdjTons, Sum(AdjCont) AdjCont, \r\n" +
                                            " sum(ProgPlansqm) ProgPlansqm, sum(ProgBooksqm) ProgBooksqm, sum(ProgPlanadv) ProgPlanadv, sum(ProgBookadv) ProgBookadv, sum(ProgPlanTons) ProgPlanTons, sum(ProgBookTons) ProgBookTons, sum(ProgPlanCont) ProgPlanCont, sum(ProgBookCont) ProgBookCont \r\n" +
                                            " from( ";
            }

            if (barShowInfo.EditValue.ToString() == "Mining Manager")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select MiningManager,um sectionid, MiningManager sectionname,'" + string.Empty + "' workplaceid, '" + string.Empty + "' description, thedate, \r\n" +
                                            "sum(plansqm) PlanSqm, sum(planadv) PlanAdv, sum(plantons) PlanTons \r\n" +
                                            ", sum(plangrams) PlanGrams, \r\n" +
                                            "sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons \r\n" +
                                            ", sum(bookgrams) BookGrams, sum(wd) wd, \r\n" +
                                            "Sum(Adjsqm) Adjsqm, Sum(AdjTons) AdjTons, Sum(AdjCont) AdjCont, \r\n" +
                                            " sum(ProgPlansqm) ProgPlansqm, sum(ProgBooksqm) ProgBooksqm, sum(ProgPlanadv) ProgPlanadv, sum(ProgBookadv) ProgBookadv, sum(ProgPlanTons) ProgPlanTons, sum(ProgBookTons) ProgBookTons, sum(ProgPlanCont) ProgPlanCont, sum(ProgBookCont) ProgBookCont \r\n" +
                                            " from( ";
            }

            if (barShowInfo.EditValue.ToString() == "Mine Overseer")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select MineOverseer,mo sectionid, MineOverseer sectionname,'" + string.Empty + "' workplaceid, '" + string.Empty + "' description, thedate, \r\n" +
                                            "sum(plansqm) PlanSqm, sum(planadv) PlanAdv, sum(plantons) PlanTons \r\n" +
                                            ", sum(plangrams) PlanGrams, \r\n" +
                                            "sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons \r\n" +
                                            ", sum(bookgrams) BookGrams, sum(wd) wd, \r\n" +
                                            "Sum(Adjsqm) Adjsqm, Sum(AdjTons) AdjTons, Sum(AdjCont) AdjCont, \r\n" +
                                            " sum(ProgPlansqm) ProgPlansqm, sum(ProgBooksqm) ProgBooksqm, sum(ProgPlanadv) ProgPlanadv, sum(ProgBookadv) ProgBookadv, sum(ProgPlanTons) ProgPlanTons, sum(ProgBookTons) ProgBookTons, sum(ProgPlanCont) ProgPlanCont, sum(ProgBookCont) ProgBookCont \r\n" +
                                            " from(";
            }


            if (barShowInfo.EditValue.ToString() == "Shiftboss")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select Shiftboss,sb sectionid, Shiftboss sectionname, '" + string.Empty + "' workplaceid, '" + string.Empty + "' description, thedate, \r\n" +
                                            " sum(plansqm) PlanSqm, sum(planadv) PlanAdv, sum(plantons) PlanTons \r\n" +
                                            ", sum(plangrams) PlanGrams, \r\n" +
                                            " sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons \r\n" +
                                            ", sum(bookgrams) Bookgrams, sum(wd) wd, \r\n" +
                                            "Sum(Adjsqm) Adjsqm, Sum(AdjTons) AdjTons, Sum(AdjCont) AdjCont, \r\n" +
                                            " sum(ProgPlansqm) ProgPlansqm, sum(ProgBooksqm) ProgBooksqm, sum(ProgPlanadv) ProgPlanadv, sum(ProgBookadv) ProgBookadv, sum(ProgPlanTons) ProgPlanTons, sum(ProgBookTons) ProgBookTons, sum(ProgPlanCont) ProgPlanCont, sum(ProgBookCont) ProgBookCont \r\n" +
                                            " from(";
            }


            if (barShowInfo.EditValue.ToString() == "Miner")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select Miner,sectionid, Miner sectionname,  '" + string.Empty + "' workplaceid, '" + string.Empty + "' description, thedate, \r\n" +
                                            "sum(plansqm) PlanSqm, sum(planadv) PlanAdv, sum(plantons) PlanTons \r\n" +
                                            ", sum(plangrams) PlanGrams, \r\n" +
                                            " sum(booksqm) BookSqm, sum(bookadv) BookAdv, sum(booktons) BookTons \r\n" +
                                            ", sum(bookgrams) BookGrams, sum(wd) wd, \r\n" +
                                            "Sum(Adjsqm) Adjsqm, Sum(AdjTons) AdjTons, Sum(AdjCont) AdjCont,  \r\n" +
                                            " sum(ProgPlansqm) ProgPlansqm, sum(ProgBooksqm) ProgBooksqm, sum(ProgPlanadv) ProgPlanadv, sum(ProgBookadv) ProgBookadv, sum(ProgPlanTons) ProgPlanTons, sum(ProgBookTons) ProgBookTons, sum(ProgPlanCont) ProgPlanCont, sum(ProgBookCont) ProgBookCont \r\n" +
                                            " from(";

            }

            _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " select b.*, w.description, s.reporttosectionid sb, s1.name sbname, s1.reporttosectionid mo \r\n" +
                                        ",s2.reporttosectionid um, s3.reporttosectionid pm, \r\n" +
                                        "s.name conname, s2.name moname, s3.name umname, s4.name pmname, s.name sectionname from(\r\n" +
                                        "select sec.Name_5 GM,sec.Name_4 MineManager, \r\n" +
                                        " sec.Name_3 MiningManager, sec.Name_2 MineOverseer, \r\n" +
                                        "sec.Name_1 Shiftboss,sec.Name Miner, \r\n" +
                                        "case when cal.workingday = 'Y' then 1 else 0 end as wd, \r\n" +
                                        "p.activity, p.BookAdv BookAdv, isnull(p.BookSqm,0) BookSqm , isnull(p.BookTons,0) BookTons, isnull(p.BookGrams,0) BookGrams, \r\n" +
                                        "p.sectionid,p.workplaceid,p.prodmonth,p.calendardate TheDate,p.sqm PlanSqm, p.Adv PlanAdv,  \r\n" +
                                        "p.tons PlanTons, p.Grams PlanGrams,isnull(p.AdjSqm,0) AdjSqm,isnull(p.AdjTons,0) AdjTons,isnull(p.AdjCont,0) AdjCont \r\n" +

                                        ", case when p.calendardate < getdate()+1 then p.sqm else 0 end as ProgPlansqm \r\n" +
                                        ", case when p.calendardate < getdate()+1 then isnull(p.BookSqm,0)+isnull(p.AdjSqm,0) else 0 end as ProgBooksqm \r\n" +

                                        ", case when p.calendardate < getdate()+1 then p.Adv else 0 end as ProgPlanadv \r\n" +
                                        ", case when p.calendardate < getdate()+1 then p.BookAdv else 0 end as ProgBookadv \r\n" +

                                         ", case when p.calendardate < getdate()+1 then p.tons else 0 end as ProgPlanTons \r\n" +
                                        ", case when p.calendardate < getdate()+1 then isnull(p.BookTons,0)+isnull(p.AdjTons,0) else 0 end as ProgBookTons \r\n" +

                                         ", case when p.calendardate < getdate()+1 then  p.Grams else 0 end as ProgPlanCont \r\n" +
                                        ", case when p.calendardate < getdate()+1 then isnull(p.BookGrams,0)+isnull(p.AdjCont,0) else 0 end as ProgBookCont \r\n" +


                                        "from vw_PlanningFact p left outer join \r\n" +
                                        "vw_PlanmonthFact pm on p.prodmonth = pm.prodmonth and p.sectionid = pm.sectionid \r\n" +
                                        "and p.workplaceid = pm.workplaceid and p.activity = pm.activity \r\n" +
                                        "left outer join SECTIONS_COMPLETE sec on pm.Prodmonth = sec.Prodmonth \r\n" +
                                        "and pm.Sectionid = sec.SectionID \r\n" +
                                        "left outer join vw_Seccal_totalShifts_Calc seccal on pm.Prodmonth = seccal.Prodmonth  \r\n" +
                                        "and sec.sectionid_1 = seccal.Sectionid \r\n" +
                                        "left outer join tbl_Code_Calendar_Type cal on seccal.CalendarCode = cal.CalendarCode \r\n" +
                                        "and p.calendardate = cal.CalendarDate \r\n" +
            string.Empty;

            if (Typergp.EditValue.ToString() == stp)
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "where p.prodmonth = '" + month + "' and pm.Auth = 'Y' ) b left outer join \r\n";
            }
            else
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "where p.prodmonth = '" + month + "' and pm.Auth = 'Y'  ) b left outer join \r\n";
            }

            _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " tbl_workplace w on b.workplaceid = w.workplaceid \r\n" +
                                       " " +
                                       "left outer join tbl_section s on b.sectionid = s.sectionid and \r\n" +
                                       "b.prodmonth = s.prodmonth \r\n" +
                                       "left outer join tbl_section s1 on s.reporttosectionid = s1.sectionid and \r\n" +
                                       "s.prodmonth = s1.prodmonth \r\n" +
                                       "left outer join tbl_section s2 on s1.reporttosectionid = s2.sectionid and \r\n" +
                                       "s1.prodmonth = s2.prodmonth \r\n" +
                                       "left outer join tbl_section s3 on s2.reporttosectionid = s3.sectionid and \r\n" +
                                       "s2.prodmonth = s3.prodmonth \r\n" +
                                       "left outer join tbl_section s4 on s3.reporttosectionid = s4.sectionid and \r\n" +
                                       "s3.prodmonth = s4.prodmonth \r\n" +
                                       "left outer join tbl_section s5 on s4.reporttosectionid = s5.sectionid and \r\n" +
                                       "s4.prodmonth = s5.prodmonth \r\n";

            if (Typergp.EditValue.ToString() == stp)
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " where b.activity <> 1 \r\n";
            }
            else
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + " where b.activity = 1 \r\n";
            }
            if (Hier == "0")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "and s5.reporttosectionid = '" + sec + "' \r\n";
            }

            if (Hier == "1")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "and s4.reporttosectionid = '" + sec + "' \r\n";
            }
            if (Hier == "2")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "and s3.reporttosectionid = '" + sec + "' \r\n";
            }
            if (Hier == "3")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "and s2.reporttosectionid = '" + sec + "' \r\n";
            }
            if (Hier == "4")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "and s1.reporttosectionid = '" + sec + "' \r\n";
            }
            if (Hier == "5")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "and s.reporttosectionid = '" + sec + "' \r\n";
            }
            if (Hier == "6")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "and s.sectionid = '" + sec + "' \r\n";
            }

            if (barShowInfo.EditValue.ToString() == "Total Mine")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by GM,thedate order by thedate \r\n";
            }

            if (barShowInfo.EditValue.ToString() == "Mine Manager")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by MineManager,pm, pmname, thedate order by pm , thedate \r\n";
            }

            if (barShowInfo.EditValue.ToString() == "Mining Manager")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by MiningManager,um, umname, thedate order by um , thedate  \r\n";
            }
            if (barShowInfo.EditValue.ToString() == "Mine Overseer")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by MineOverseer,mo, moname, thedate order by mo , thedate \r\n";
            }

            if (barShowInfo.EditValue.ToString() == "Shiftboss")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by Shiftboss,sb, sbname, thedate order by sb , thedate \r\n";
            }
            if (barShowInfo.EditValue.ToString() == "Miner")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + ")z group by Miner,sectionid, conname, thedate order by sectionid , thedate \r\n";
            }
            if (barShowInfo.EditValue.ToString() == "Workplace")
            {
                _dbManDaily.SqlStatement = _dbManDaily.SqlStatement + "order by b.sectionid, conname , w.description, b.Activity,  b.thedate \r\n";
            }

            _dbManDaily.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDaily.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDaily.ExecuteInstruction();


            DataTable dtDaily = _dbManDaily.ResultsDataTable;
            SectionName = dtDaily.Rows[0][0].ToString();
            gridDaily.RowCount = 500;
            gridProgressive.RowCount = 500;
            // int col = 5;
            int row = 0;

            int nextRow = row + 1;
            int ProgPlan = 0;
            decimal ProgBook = 0;
            int TotPlan = 0;
            int TotBook = 0;
            int totRow = nextRow + 1;
            int totRownext = totRow + 1;

            decimal ProgPlanAdv = 0;
            decimal ProgBookAdv = 0;

            decimal ProgPlanTons = 0;
            decimal ProgBookTons = 0;

            decimal ProgPlanGrams = 0;
            decimal ProgBookGrams = 0;

            double ProgPlanOunces = 0;
            double ProgBookOunces = 0;

            string group = string.Empty;

            ////if (dtDaily.Rows.Count > 0)
            ////{

            if (barShowInfo.EditValue.ToString() == "Workplace")
            {
                group = dtDaily.Rows[0]["workplaceid"].ToString() + dtDaily.Rows[0]["activity"].ToString();
                gridDaily.Rows[row].Cells[0].Value = dtDaily.Rows[0]["SectionID"].ToString();
                gridDaily.Rows[nextRow].Cells[0].Value = dtDaily.Rows[0]["SectionName"].ToString();
                gridDaily.Rows[row].Cells[1].Value = dtDaily.Rows[0]["workplaceid"].ToString();
                gridDaily.Rows[row].Cells[2].Value = dtDaily.Rows[0]["description"].ToString();
                gridDaily.Rows[row].Cells[3].Value = "Plan";
                gridDaily.Rows[nextRow].Cells[3].Value = "Book";
                gridDaily.Rows[totRow].Cells[0].Value = "Total";
                gridDaily.Rows[totRow].Cells[3].Value = "Plan";
                gridDaily.Rows[totRownext].Cells[3].Value = "Book";

                ////////////////////////////////Progressive Grid/////////////////////////////
                gridProgressive.Rows[row].Cells[0].Value = dtDaily.Rows[0]["SectionID"].ToString();
                gridProgressive.Rows[nextRow].Cells[0].Value = dtDaily.Rows[0]["SectionName"].ToString();
                gridProgressive.Rows[row].Cells[1].Value = dtDaily.Rows[0]["workplaceid"].ToString();
                gridProgressive.Rows[row].Cells[2].Value = dtDaily.Rows[0]["description"].ToString();
                gridProgressive.Rows[row].Cells[3].Value = "Plan";
                gridProgressive.Rows[nextRow].Cells[3].Value = "Book";
                gridProgressive.Rows[totRow].Cells[0].Value = "Total";
                gridProgressive.Rows[totRow].Cells[3].Value = "Plan";
                gridProgressive.Rows[totRownext].Cells[3].Value = "Book";

            }
            else
            {
                group = dtDaily.Rows[0]["SectionId"].ToString();
                gridDaily.Rows[row].Cells[0].Value = dtDaily.Rows[0]["SectionID"].ToString();
                gridDaily.Rows[nextRow].Cells[0].Value = dtDaily.Rows[0]["SectionName"].ToString();
                gridDaily.Rows[row].Cells[1].Value = dtDaily.Rows[0]["workplaceid"].ToString();
                gridDaily.Rows[row].Cells[2].Value = dtDaily.Rows[0]["description"].ToString();
                gridDaily.Rows[row].Cells[3].Value = "Plan";
                gridDaily.Rows[nextRow].Cells[3].Value = "Book";
                gridDaily.Rows[totRow].Cells[0].Value = "Total";
                gridDaily.Rows[totRow].Cells[3].Value = "Plan";
                gridDaily.Rows[totRownext].Cells[3].Value = "Book";


                /////////////////////Progressive Grid///////////////////////

                gridProgressive.Rows[row].Cells[0].Value = dtDaily.Rows[0]["SectionID"].ToString();
                gridProgressive.Rows[nextRow].Cells[0].Value = dtDaily.Rows[0]["SectionName"].ToString();
                gridProgressive.Rows[row].Cells[1].Value = dtDaily.Rows[0]["workplaceid"].ToString();
                gridProgressive.Rows[row].Cells[2].Value = dtDaily.Rows[0]["description"].ToString();
                gridProgressive.Rows[row].Cells[3].Value = "Plan";
                gridProgressive.Rows[nextRow].Cells[3].Value = "Book";
                gridProgressive.Rows[totRow].Cells[0].Value = "Total";
                gridProgressive.Rows[totRow].Cells[3].Value = "Plan";
                gridProgressive.Rows[totRownext].Cells[3].Value = "Book";
            }


            TimeSpan Span;



            foreach (DataRow drDaily in dtDaily.Rows)
            {
                Span = Convert.ToDateTime(drDaily["thedate"].ToString()).Subtract(dtpStartDate.Value);

                col = Convert.ToInt32(Span.Days) + 5;
                if (col > col1)
                    col1 = col;
                //TotPlan = 0;
                //TotBook = 0;

                if (barShowInfo.EditValue.ToString() == "Workplace")
                {
                    if (drDaily["workplaceid"].ToString() + drDaily["activity"].ToString() != group)
                    {
                        row = row + 2;
                        nextRow = nextRow + 2;
                        totRow = totRow + 2;
                        totRownext = totRownext + 2;
                        gridDaily.Rows[row].Cells[0].Value = drDaily["SectionID"].ToString();
                        gridDaily.Rows[nextRow].Cells[0].Value = drDaily["SectionName"].ToString();

                        gridDaily.Rows[row].Cells[1].Value = drDaily["workplaceid"].ToString();
                        gridDaily.Rows[row].Cells[2].Value = drDaily["description"].ToString();
                        gridDaily.Rows[row].Cells[3].Value = "Plan";
                        gridDaily.Rows[nextRow].Cells[3].Value = "Book";

                        gridProgressive.Rows[row].Cells[0].Value = drDaily["SectionID"].ToString();
                        gridProgressive.Rows[nextRow].Cells[0].Value = drDaily["SectionName"].ToString();

                        gridProgressive.Rows[row].Cells[1].Value = drDaily["workplaceid"].ToString();
                        gridProgressive.Rows[row].Cells[2].Value = drDaily["description"].ToString();
                        gridProgressive.Rows[row].Cells[3].Value = "Plan";
                        gridProgressive.Rows[nextRow].Cells[3].Value = "Book";

                        ProgPlan = 0;
                        ProgBook = 0;
                        ProgPlanAdv = 0;
                        ProgBookAdv = 0;
                        ProgPlanTons = 0;
                        ProgBookTons = 0;
                        ProgPlanGrams = 0;
                        ProgBookGrams = 0;
                        ProgPlanOunces = 0;
                        ProgBookOunces = 0;


                        group = drDaily["workplaceid"].ToString() + drDaily["activity"].ToString();

                    }


                }
                else
                {
                    if (drDaily["SectionID"].ToString() != group)
                    {
                        row = row + 2;
                        nextRow = nextRow + 2;
                        totRow = totRow + 2;
                        totRownext = totRownext + 2;
                        gridDaily.Rows[row].Cells[0].Value = drDaily["SectionID"].ToString();
                        gridDaily.Rows[nextRow].Cells[0].Value = drDaily["SectionName"].ToString();

                        gridDaily.Rows[row].Cells[1].Value = drDaily["workplaceid"].ToString();
                        gridDaily.Rows[row].Cells[2].Value = drDaily["description"].ToString();
                        gridDaily.Rows[row].Cells[3].Value = "Plan";
                        gridDaily.Rows[nextRow].Cells[3].Value = "Book";

                        /////////////////////////////progressive Book////////////////////////
                        gridProgressive.Rows[row].Cells[0].Value = drDaily["SectionID"].ToString();
                        gridProgressive.Rows[nextRow].Cells[0].Value = drDaily["SectionName"].ToString();

                        gridProgressive.Rows[row].Cells[1].Value = drDaily["workplaceid"].ToString();
                        gridProgressive.Rows[row].Cells[2].Value = drDaily["description"].ToString();
                        gridProgressive.Rows[row].Cells[3].Value = "Plan";
                        gridProgressive.Rows[nextRow].Cells[3].Value = "Book";

                        ProgPlan = 0;
                        ProgBook = 0;
                        ProgPlanAdv = 0;
                        ProgBookAdv = 0;
                        ProgPlanTons = 0;
                        ProgBookTons = 0;
                        ProgPlanGrams = 0;
                        ProgBookGrams = 0;
                        ProgPlanOunces = 0;
                        ProgBookOunces = 0;


                        group = drDaily["SectionID"].ToString();//drDaily["workplaceid"].ToString() + drDaily["activity"].ToString();
                    }


                }


                //gridDaily.Rows[row].Cells[col].Value = drDaily["PlanSqm"].ToString();
                //gridDaily.Rows[nextRow].Cells[col].Value = drDaily["BookSqm"].ToString();


                if (drDaily["wd"].ToString() == "0")
                {
                    gridDaily[col, row].Style.BackColor = Color.LightGray;
                    gridDaily[col, row].Style.ForeColor = Color.LightGray;
                    gridDaily[col, nextRow].Style.BackColor = Color.LightGray;

                    /////////Progressive Grid//////////////////
                    gridProgressive[col, row].Style.BackColor = Color.LightGray;
                    gridProgressive[col, row].Style.ForeColor = Color.LightGray;
                    gridProgressive[col, nextRow].Style.BackColor = Color.LightGray;
                    gridProgressive[col, nextRow].Style.ForeColor = Color.LightGray;
                }

                ////////////SQM////////////////
                if (Showrgb.EditValue.ToString() == sqm)
                {

                    if ((drDaily["PlanSqm"].ToString() == string.Empty) | (drDaily["PlanSqm"].ToString() == null))
                    {
                        gridDaily.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = string.Empty;

                    }
                    else
                    {
                        gridDaily.Rows[row].Cells[col].Value = drDaily["PlanSqm"].ToString();
                        gridProgressive.Rows[row].Cells[col].Value = Convert.ToInt32(drDaily["PlanSqm"].ToString()) + Convert.ToInt32(gridProgressive.Rows[row].Cells[col - 1].Value);


                        ProgPlan = ProgPlan + Convert.ToInt32(drDaily["ProgPlansqm"].ToString());


                        gridDaily.Rows[497].Cells[col].Value = Convert.ToInt32(gridDaily.Rows[497].Cells[col].Value) + Convert.ToInt32(drDaily["PlanSqm"].ToString());
                        gridDaily.Rows[494].Cells[4].Value = Convert.ToInt32(gridDaily.Rows[494].Cells[4].Value) + Convert.ToInt32(drDaily["ProgPlansqm"].ToString());

                        gridProgressive.Rows[497].Cells[col].Value = Convert.ToInt32(gridProgressive.Rows[497].Cells[col - 1].Value) + Convert.ToInt32(drDaily["PlanSqm"].ToString());
                        gridProgressive.Rows[494].Cells[4].Value = Convert.ToInt32(gridProgressive.Rows[494].Cells[4].Value) + Convert.ToInt32(drDaily["ProgPlansqm"].ToString());

                    }

                    if ((drDaily["BookSqm"].ToString() == string.Empty) | (drDaily["BookSqm"].ToString() == null))
                    {
                        gridDaily.Rows[nextRow].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[nextRow].Cells[col].Value = Convert.ToInt32(0) + Convert.ToInt32(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToInt32(0) + Convert.ToInt32(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                    }
                    else
                    {
                        //string CheckMeasDay = System.DateTime.Today.ToShortDateString("d");
                        //if (adjBook == "Y")
                        //{

                        gridDaily.Rows[nextRow].Cells[col].Value = Convert.ToString(Convert.ToInt32(drDaily["BookSqm"].ToString()));
                        gridProgressive.Rows[nextRow].Cells[col].Value = Convert.ToDecimal(drDaily["BookSqm"].ToString()) + Convert.ToDecimal(drDaily["AdjSqm"].ToString()) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        ProgBook = ProgBook + Convert.ToDecimal(drDaily["ProgBooksqm"].ToString());
                        gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Convert.ToDecimal(drDaily["BookSqm"].ToString());
                        gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Convert.ToDecimal(drDaily["ProgBooksqm"].ToString());

                        string aa = drDaily["ProgBooksqm"].ToString();

                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(drDaily["BookSqm"].ToString()) + Convert.ToDecimal(drDaily["AdjSqm"].ToString()) + Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value);
                        gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Convert.ToDecimal(drDaily["ProgBooksqm"].ToString());
                        // gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(ProgBook);
                        //}
                        //else
                        //{
                        //    gridDaily.Rows[nextRow].Cells[col].Value = drDaily["BookSqm"].ToString();
                        //    gridProgressive.Rows[nextRow].Cells[col].Value = Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        //    ProgBook = ProgBook + Convert.ToInt32(drDaily["BookSqm"].ToString());
                        //    gridDaily.Rows[498].Cells[col].Value = Convert.ToInt32(gridDaily.Rows[498].Cells[col].Value) + Convert.ToInt32(drDaily["BookSqm"].ToString());
                        //    gridDaily.Rows[495].Cells[4].Value = Convert.ToInt32(gridDaily.Rows[495].Cells[4].Value) + Convert.ToInt32(drDaily["BookSqm"].ToString());

                        //    gridProgressive.Rows[498].Cells[col].Value = Convert.ToInt32(drDaily["BookSqm"].ToString()) + Convert.ToInt32(gridProgressive.Rows[498].Cells[col - 1].Value);
                        //    gridProgressive.Rows[495].Cells[4].Value = Convert.ToInt32(gridProgressive.Rows[495].Cells[4].Value) + Convert.ToInt32(drDaily["BookSqm"].ToString());
                        //}
                    }

                    gridDaily.Rows[row].Cells[4].Value = ProgPlan.ToString();
                    gridDaily.Rows[nextRow].Cells[4].Value = ProgBook.ToString();

                    gridProgressive.Rows[row].Cells[4].Value = ProgPlan.ToString();
                    gridProgressive.Rows[nextRow].Cells[4].Value = ProgBook.ToString();
                }

                ////////////////Meters//////////////////////////////////////
                if (Showrgb.EditValue.ToString() == mts)
                {

                    if ((drDaily["PlanAdv"].ToString() == string.Empty) | (drDaily["PlanAdv"].ToString() == null))
                    {
                        gridDaily.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = Convert.ToDecimal(0) + Convert.ToDecimal(gridProgressive.Rows[row].Cells[col - 1].Value);

                    }
                    else
                    {
                        gridDaily.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanAdv"].ToString()), 1);
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanAdv"].ToString()) + Convert.ToDecimal(gridProgressive.Rows[row].Cells[col - 1].Value), 1);
                        ProgPlanAdv = ProgPlanAdv + Math.Round(Convert.ToDecimal(drDaily["ProgPlanAdv"].ToString()), 1);

                        gridDaily.Rows[497].Cells[col].Value = Math.Round(Convert.ToDecimal(gridDaily.Rows[497].Cells[col].Value) + Convert.ToDecimal(drDaily["PlanAdv"].ToString()), 1);
                        gridDaily.Rows[494].Cells[4].Value = Math.Round(Convert.ToDecimal(gridDaily.Rows[494].Cells[4].Value) + Convert.ToDecimal(drDaily["ProgPlanAdv"].ToString()), 1);

                        gridProgressive.Rows[497].Cells[col].Value = Math.Round(Convert.ToDecimal(gridProgressive.Rows[497].Cells[col - 1].Value) + Convert.ToDecimal(drDaily["PlanAdv"].ToString()), 1);
                        gridProgressive.Rows[494].Cells[4].Value = Math.Round(Convert.ToDecimal(gridProgressive.Rows[494].Cells[4].Value) + Convert.ToDecimal(drDaily["ProgPlanAdv"].ToString()), 1);

                    }

                    if ((drDaily["BookAdv"].ToString() == string.Empty) | (drDaily["BookAdv"].ToString() == null))
                    {
                        gridDaily.Rows[nextRow].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value), 1);
                        gridProgressive.Rows[498].Cells[col].Value = Math.Round(Convert.ToDecimal(0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value), 1);
                    }
                    else
                    {
                        gridDaily.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookAdv"].ToString()), 1);
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookAdv"].ToString()) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value), 1);
                        ProgBookAdv = ProgBookAdv + Math.Round(Convert.ToDecimal(drDaily["ProgBookAdv"].ToString()), 1);
                        gridDaily.Rows[498].Cells[col].Value = Math.Round(Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Convert.ToDecimal(drDaily["BookAdv"].ToString()), 1);
                        gridDaily.Rows[495].Cells[4].Value = Math.Round(Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Convert.ToDecimal(drDaily["ProgBookAdv"].ToString()), 1);

                        gridProgressive.Rows[498].Cells[col].Value = Math.Round(Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Convert.ToDecimal(drDaily["BookAdv"].ToString()), 1);
                        gridProgressive.Rows[495].Cells[4].Value = Math.Round(Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Convert.ToDecimal(drDaily["ProgBookAdv"].ToString()), 1);
                    }

                    gridDaily.Rows[row].Cells[4].Value = ProgPlanAdv.ToString();
                    gridDaily.Rows[nextRow].Cells[4].Value = ProgBookAdv.ToString();

                    gridProgressive.Rows[row].Cells[4].Value = ProgPlanAdv.ToString();
                    gridProgressive.Rows[nextRow].Cells[4].Value = ProgBookAdv.ToString();

                }


                ///////////Tons/////////////////////
                if (Showrgb.EditValue.ToString() == ton)
                {

                    if ((drDaily["PlanTons"].ToString() == string.Empty) | (drDaily["PlanTons"].ToString() == null))
                    {
                        gridDaily.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(0), 0) + Convert.ToDecimal(gridProgressive.Rows[row].Cells[col - 1].Value);

                    }
                    else
                    {
                        gridDaily.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanTons"]), 0).ToString();
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanTons"].ToString()), 0) + Convert.ToDecimal(gridProgressive.Rows[row].Cells[col - 1].Value);
                        ProgPlanTons = ProgPlanTons + Math.Round(Convert.ToDecimal(drDaily["ProgPlanTons"].ToString()), 0);

                        gridDaily.Rows[497].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[497].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanTons"].ToString()), 0);
                        gridDaily.Rows[494].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["ProgPlanTons"].ToString()), 0);

                        gridProgressive.Rows[497].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[497].Cells[col - 1].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanTons"].ToString()), 0);
                        gridProgressive.Rows[494].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["ProgPlanTons"].ToString()), 0);

                    }

                    if ((drDaily["BookTons"].ToString() == string.Empty) | (drDaily["BookTons"].ToString() == null))
                    {
                        gridDaily.Rows[nextRow].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(0), 0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        gridProgressive.Rows[498].Cells[col].Value = Math.Round(Convert.ToDecimal(0), 0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                    }
                    else
                    {
                        //if (adjBook == "Y")
                        //{

                        gridDaily.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookTons"]), 0).ToString();
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()) + Convert.ToDecimal(drDaily["AdjTons"].ToString()), 0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        ProgBookTons = ProgBookTons + Math.Round((Convert.ToDecimal(drDaily["ProgBookTons"].ToString())), 0);
                        gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0);
                        gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["ProgBookTons"].ToString()), 0);

                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()) + Convert.ToDecimal(drDaily["AdjTons"].ToString()), 0);
                        gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["ProgBookTons"].ToString()), 0);
                        //}
                        //else
                        //{
                        //    gridDaily.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookTons"]), 0).ToString();
                        //    gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        //    ProgBookTons = ProgBookTons + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0);
                        //    gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0);
                        //    gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Convert.ToDecimal(drDaily["BookTons"].ToString());

                        //    gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Convert.ToDecimal(gridProgressive.Rows[498].Cells[col - 1].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0);
                        //    gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["BookTons"].ToString()), 0);
                        //}
                    }

                    gridDaily.Rows[row].Cells[4].Value = ProgPlanTons.ToString();
                    gridDaily.Rows[nextRow].Cells[4].Value = ProgBookTons.ToString();

                    gridProgressive.Rows[row].Cells[4].Value = ProgPlanTons.ToString();
                    gridProgressive.Rows[nextRow].Cells[4].Value = ProgBookTons.ToString();

                }

                ///////////////Kilograms//////////////////
                if (Showrgb.EditValue.ToString() == kg)
                {
                    decimal zero = 0;
                    if ((drDaily["PlanGrams"].ToString() == string.Empty) | (drDaily["PlanGrams"].ToString() == null))
                    {
                        gridDaily.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(zero), 0) + Convert.ToDecimal(gridProgressive.Rows[row].Cells[col - 1].Value);

                    }
                    else
                    {
                        gridDaily.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 2); ;
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 2) + Convert.ToDecimal(gridProgressive.Rows[row].Cells[col - 1].Value);
                        ProgPlanGrams = Math.Round(ProgPlanGrams + Convert.ToDecimal(drDaily["ProgPlanCont"].ToString()) / 1000, 2);

                        gridDaily.Rows[497].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[497].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 2);
                        gridDaily.Rows[494].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["ProgPlanCont"].ToString()) / 1000, 2);

                        gridProgressive.Rows[497].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[497].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["PlanGrams"].ToString()) / 1000, 2);
                        gridProgressive.Rows[494].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["ProgPlanCont"].ToString()) / 1000, 2);

                    }

                    if ((drDaily["BookGrams"].ToString() == string.Empty) | (drDaily["BookGrams"].ToString() == null))
                    {
                        gridDaily.Rows[nextRow].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(zero), 0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        gridProgressive.Rows[498].Cells[col].Value = Math.Round(Convert.ToDecimal(zero), 0) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                    }
                    else
                    {
                        //if (adjBook == "Y")
                        //{

                        gridDaily.Rows[nextRow].Cells[col].Value = Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString())) / 1000, 2);
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString()) + Convert.ToDecimal(drDaily["AdjCont"].ToString())) / 1000, 2) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        ProgBookGrams = ProgBookGrams + Math.Round((Convert.ToDecimal(drDaily["ProgBookCont"].ToString())) / 1000, 2);
                        gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString())) / 1000, 2);
                        gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Math.Round((Convert.ToDecimal(drDaily["ProgBookCont"].ToString()) + Convert.ToDecimal(drDaily["AdjCont"].ToString())) / 1000, 2);

                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Math.Round((Convert.ToDecimal(drDaily["BookGrams"].ToString()) + Convert.ToDecimal(drDaily["AdjCont"].ToString())) / 1000, 2);
                        gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Math.Round((Convert.ToDecimal(drDaily["ProgBookCont"].ToString())) / 1000, 2);
                        //}
                        //else
                        //{
                        //    gridDaily.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 2);
                        //    gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 2) + Convert.ToDecimal(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        //    ProgBookGrams = ProgBookGrams + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 2);
                        //    gridDaily.Rows[498].Cells[col].Value = Convert.ToDecimal(gridDaily.Rows[498].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 2);
                        //    gridDaily.Rows[495].Cells[4].Value = Convert.ToDecimal(gridDaily.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 2);

                        //    gridProgressive.Rows[498].Cells[col].Value = Convert.ToDecimal(gridProgressive.Rows[498].Cells[col].Value) + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 2);
                        //    gridProgressive.Rows[495].Cells[4].Value = Convert.ToDecimal(gridProgressive.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDecimal(drDaily["BookGrams"].ToString()) / 1000, 2);
                        //}
                    }

                    gridDaily.Rows[row].Cells[4].Value = Math.Round(ProgPlanGrams, 2);
                    gridDaily.Rows[nextRow].Cells[4].Value = Math.Round(ProgBookGrams, 2);

                    gridProgressive.Rows[row].Cells[4].Value = Math.Round(ProgPlanGrams, 2);
                    gridProgressive.Rows[nextRow].Cells[4].Value = Math.Round(ProgBookGrams, 2);

                }
                /////////////////Ounces////////////////////////
                if (Showrgb.EditValue.ToString() == "Ounes")
                {

                    if ((drDaily["PlanGrams"].ToString() == string.Empty) | (drDaily["PlanGrams"].ToString() == null))
                    {
                        gridDaily.Rows[row].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDouble(0), 0) + Convert.ToDouble(gridProgressive.Rows[row].Cells[col - 1].Value);

                    }
                    else
                    {
                        gridDaily.Rows[row].Cells[col].Value = Math.Round(Convert.ToDouble(drDaily["PlanGrams"].ToString()) / 31.10348, 0); ;
                        gridProgressive.Rows[row].Cells[col].Value = Math.Round(Convert.ToDouble(drDaily["PlanGrams"].ToString()) / 31.10348, 0) + Convert.ToDouble(gridProgressive.Rows[row].Cells[col - 1].Value);
                        ProgPlanOunces = Math.Round(ProgPlanOunces + Convert.ToDouble(drDaily["PlanGrams"].ToString()) / 31.10348, 3);

                        gridDaily.Rows[497].Cells[col].Value = Convert.ToDouble(gridDaily.Rows[497].Cells[col].Value) + Math.Round(Convert.ToDouble(drDaily["PlanGrams"].ToString()) / 31.10348, 0);
                        gridDaily.Rows[494].Cells[4].Value = Convert.ToDouble(gridDaily.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDouble(drDaily["PlanGrams"].ToString()) / 31.10348, 0);

                        gridProgressive.Rows[497].Cells[col].Value = Convert.ToDouble(gridProgressive.Rows[497].Cells[col - 1].Value) + Math.Round(Convert.ToDouble(drDaily["PlanGrams"].ToString()) / 31.10348, 0);
                        gridProgressive.Rows[494].Cells[4].Value = Convert.ToDouble(gridProgressive.Rows[494].Cells[4].Value) + Math.Round(Convert.ToDouble(drDaily["PlanGrams"].ToString()) / 31.10348, 0);
                    }

                    if ((drDaily["BookGrams"].ToString() == string.Empty) | (drDaily["BookGrams"].ToString() == null))
                    {
                        gridDaily.Rows[nextRow].Cells[col].Value = string.Empty;
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDouble(0), 0) + Convert.ToDouble(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        gridProgressive.Rows[498].Cells[col].Value = Math.Round(Convert.ToDouble(0), 0) + Convert.ToDouble(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                    }
                    else
                    {
                        //if (adjBook == "Y")
                        //{
                        gridDaily.Rows[nextRow].Cells[col].Value = Math.Round((Convert.ToDouble(drDaily["BookGrams"].ToString()) + Convert.ToDouble(drDaily["AdjCont"].ToString())) / 31.10348, 0);
                        gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round((Convert.ToDouble(drDaily["BookGrams"].ToString()) + Convert.ToDouble(drDaily["AdjCont"].ToString())) / 31.10348, 0) + Convert.ToDouble(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        ProgBookOunces = ProgBookOunces + Math.Round((Convert.ToDouble(drDaily["BookGrams"].ToString()) + Convert.ToDouble(drDaily["AdjCont"].ToString())) / 31.10348, 3);
                        gridDaily.Rows[498].Cells[col].Value = Convert.ToDouble(gridDaily.Rows[498].Cells[col].Value) + Math.Round((Convert.ToDouble(drDaily["BookGrams"].ToString()) + Convert.ToDouble(drDaily["AdjCont"].ToString())) / 31.10348, 0);
                        gridDaily.Rows[495].Cells[4].Value = Convert.ToDouble(gridDaily.Rows[495].Cells[4].Value) + Math.Round((Convert.ToDouble(drDaily["BookGrams"].ToString()) + Convert.ToDouble(drDaily["AdjCont"].ToString())) / 31.10348, 0);

                        gridProgressive.Rows[498].Cells[col].Value = Convert.ToDouble(gridProgressive.Rows[498].Cells[col].Value) + Convert.ToDouble(gridProgressive.Rows[498].Cells[col - 1].Value) + Math.Round((Convert.ToDouble(drDaily["BookGrams"].ToString()) + Convert.ToDouble(drDaily["AdjCont"].ToString())) / 31.10348, 0);
                        gridProgressive.Rows[495].Cells[4].Value = Convert.ToDouble(gridProgressive.Rows[495].Cells[4].Value) + Math.Round((Convert.ToDouble(drDaily["BookGrams"].ToString()) + Convert.ToDouble(drDaily["AdjCont"].ToString())) / 31.10348, 0);
                        //}
                        //else
                        //{


                        //    gridDaily.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDouble(drDaily["BookGrams"].ToString()) / 31.10348, 0);
                        //    gridProgressive.Rows[nextRow].Cells[col].Value = Math.Round(Convert.ToDouble(drDaily["BookGrams"].ToString()) / 31.10348, 0) + Convert.ToDouble(gridProgressive.Rows[nextRow].Cells[col - 1].Value);
                        //    ProgBookOunces = ProgBookOunces + Math.Round(Convert.ToDouble(drDaily["BookGrams"].ToString()) / 31.10348, 3);
                        //    gridDaily.Rows[498].Cells[col].Value = Convert.ToDouble(gridDaily.Rows[498].Cells[col].Value) + Math.Round(Convert.ToDouble(drDaily["BookGrams"].ToString()) / 31.10348, 0);
                        //    gridDaily.Rows[495].Cells[4].Value = Convert.ToDouble(gridDaily.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDouble(drDaily["BookGrams"].ToString()) / 31.10348, 0);

                        //    gridProgressive.Rows[498].Cells[col].Value = Convert.ToDouble(gridProgressive.Rows[498].Cells[col].Value) + Convert.ToDouble(gridProgressive.Rows[498].Cells[col - 1].Value) + Math.Round(Convert.ToDouble(drDaily["BookGrams"].ToString()) / 31.10348, 0);
                        //    gridProgressive.Rows[495].Cells[4].Value = Convert.ToDouble(gridProgressive.Rows[495].Cells[4].Value) + Math.Round(Convert.ToDouble(drDaily["BookGrams"].ToString()) / 31.10348, 0);
                        //}
                    }

                    gridDaily.Rows[row].Cells[4].Value = Math.Round(ProgPlanOunces, 0);
                    gridDaily.Rows[nextRow].Cells[4].Value = Math.Round(ProgBookOunces, 0);

                    gridProgressive.Rows[row].Cells[4].Value = Math.Round(ProgPlanOunces, 0);
                    gridProgressive.Rows[nextRow].Cells[4].Value = Math.Round(ProgBookOunces, 0);

                }

            }


            gridDaily.Rows[totRow].DefaultCellStyle.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold, GraphicsUnit.Pixel);
            gridDaily.Rows[totRownext].DefaultCellStyle.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold, GraphicsUnit.Pixel);

            gridDaily.Rows[totRow].DefaultCellStyle.BackColor = Color.LightSteelBlue;
            gridDaily.Rows[totRownext].DefaultCellStyle.BackColor = Color.LightSteelBlue;

            gridDaily.Rows[totRow].Cells[0].Value = "Total";
            gridDaily.Rows[totRow].Cells[3].Value = "Plan";
            gridDaily.Rows[totRownext].Cells[0].Value = sec + ":" + SectionName;
            gridDaily.Rows[totRownext].Cells[3].Value = "Book";

            gridDaily.Rows[totRow].Cells[4].Value = gridDaily.Rows[494].Cells[4].Value;
            gridDaily.Rows[totRownext].Cells[4].Value = gridDaily.Rows[495].Cells[4].Value;

            Decimal Plan = 0;
            Decimal Book = 0;

            for (int x = 5; x <= col1; x++)
            {
                gridDaily.Rows[totRow].Cells[x].Value = gridDaily.Rows[497].Cells[x].Value;
                gridDaily.Rows[totRownext].Cells[x].Value = gridDaily.Rows[498].Cells[x].Value;
                if (gridDaily.Rows[totRow].Cells[x].Value != null)
                {
                    Plan = Plan + Convert.ToDecimal(gridDaily.Rows[totRow].Cells[x].Value);
                }
                if (gridDaily.Rows[totRownext].Cells[x].Value != null)
                {
                    Book = Book + Convert.ToDecimal(gridProgressive.Rows[498].Cells[x].Value);
                }
                if (Showrgb.EditValue.ToString() == sqm)
                {
                    gridProgressive.Rows[totRow].Cells[x].Value = Math.Round(Plan, 0);
                    gridProgressive.Rows[totRownext].Cells[x].Value = Math.Round(Book, 0);
                }
                if (Showrgb.EditValue.ToString() == mts)
                {
                    gridProgressive.Rows[totRow].Cells[x].Value = Math.Round(Plan, 1);
                    gridProgressive.Rows[totRownext].Cells[x].Value = Math.Round(Book, 1);
                }
                if (Showrgb.EditValue.ToString() == ton)
                {
                    gridProgressive.Rows[totRow].Cells[x].Value = Math.Round(Plan, 0);
                    gridProgressive.Rows[totRownext].Cells[x].Value = Math.Round(Book, 0);
                }
                if (Showrgb.EditValue.ToString() == kg)
                {
                    gridProgressive.Rows[totRow].Cells[x].Value = Math.Round(Plan, 2);
                    gridProgressive.Rows[totRownext].Cells[x].Value = Math.Round(Book, 2);
                }
                if (Showrgb.EditValue.ToString() == "ouens")
                {
                    gridProgressive.Rows[totRow].Cells[x].Value = Math.Round(Plan, 0);
                    gridProgressive.Rows[totRownext].Cells[x].Value = Math.Round(Book, 0);
                }



            }

            //////////Progresive Grid//////////////            
            gridProgressive.Rows[totRow].DefaultCellStyle.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold, GraphicsUnit.Pixel);
            gridProgressive.Rows[totRownext].DefaultCellStyle.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold, GraphicsUnit.Pixel);

            gridProgressive.Rows[totRow].DefaultCellStyle.BackColor = Color.LightSteelBlue;
            gridProgressive.Rows[totRownext].DefaultCellStyle.BackColor = Color.LightSteelBlue;

            gridProgressive.Rows[totRow].Cells[0].Value = "Total";
            gridProgressive.Rows[totRow].Cells[3].Value = "Plan";

            gridProgressive.Rows[totRownext].Cells[0].Value = sec + ":" + SectionName;
            gridProgressive.Rows[totRownext].Cells[3].Value = "Book";


            gridProgressive.Rows[totRow].Cells[4].Value = gridProgressive.Rows[494].Cells[4].Value;
            gridProgressive.Rows[totRownext].Cells[4].Value = gridProgressive.Rows[495].Cells[4].Value;



            gridDaily.RowCount = nextRow + 4;
            gridProgressive.RowCount = nextRow + 4;
            //gridDaily.RowCount = totRownext +1;
            gridDaily.Visible = true;
            gridProgressive.Visible = true;
            gridDaily.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            gridProgressive.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

        }

    }
}
