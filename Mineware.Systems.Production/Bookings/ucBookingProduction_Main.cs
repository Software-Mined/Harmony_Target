using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionHelp;
using MWDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Bookings
{
    public partial class ucBookingProduction_Main : BaseUserControl
    {
        private readonly clsBookings _clsBookings = new clsBookings();
        private string connectionStr = "server = DESKTOP-SN57B1D; DataBase = Mineware; user id = mineware; password = corialanus;";
        private string _prodMonth = ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString();
        private string _sectionID = string.Empty;
        private string _numDay = "2";
        private int count = 0;
        private int count2 = 0;
        private Color _colourA;
        private Color _colourB;
        private Color _colourS;        

        private string IsFirsload = "Y";

        public ucBookingProduction_Main()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpBooking);
            FormActiveRibbonPage = rpBooking;
            FormMainRibbonPage = rpBooking;
            RibbonControl = rcBookings;
        }

        private void ucBookingProduction_Main_Load(object sender, EventArgs e)
        {
            //Get connection string 
            connectionStr = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            //get ABS colours
            getColoursForABS();

            //Set to current production month            
            barProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(_prodMonth);
        }

        private void gvStopingBooking_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;           

            char[] Alph = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();
            char[] Alph1 = Enumerable.Range('1', '8' - '1' + 1).Select(i => (char)i).ToArray();
                   

            if (!string.IsNullOrEmpty(e.CellValue?.ToString()))
            {
                foreach (var v in Alph)
                {
                    if (e.CellValue.ToString().ToUpper().Contains(v))
                    {
                        e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                        foreach (var s in Alph1)
                        {
                            if (e.CellValue.ToString().ToUpper().Contains(s) && e.Column.Caption != "Workplace" && e.Column.Caption != "Shift Boss" && e.Column.Caption != "Gang")
                            {
                                e.Appearance.ForeColor = Color.Red;
                            }
                        }
                        ++count2;

                    }

                    

                }
                ++count;

                
                //Add coloured rows
                var tmpCell = e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo;
                if (tmpCell.Column.Name.Contains("StpD"))
                {
                    var dtRow = tmpCell.RowInfo.RowKey as DataRow;
                    string dayNum = "ABS" + tmpCell.Column.Name.Split('D')[1];
                    if (string.IsNullOrWhiteSpace(dtRow[dayNum].ToString()))
                    {
                        //do nothing
                    }
                    else if (dtRow[dayNum].ToString() == "A")
                    {
                        e.Appearance.BackColor = _colourA;
                    }
                    else if (dtRow[dayNum].ToString() == "B" || dtRow[dayNum].ToString() == "PB")
                    {
                        e.Appearance.BackColor = _colourB;
                    }
                    else if (dtRow[dayNum].ToString() == "S" || dtRow[dayNum].ToString() == "PS")
                    {
                        e.Appearance.BackColor = _colourS;
                    }

                }

                //Set colour to be "disabled"
                if (e.CellValue.ToString() == "OFF")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                    e.Appearance.ForeColor = Color.Gainsboro;
                }

                if (e.CellValue.ToString() == "0" && e.CellValue.ToString() != "OFF")
                {
                    //e.Appearance.BackColor = Color.Gainsboro;
                    e.Appearance.ForeColor = Color.White;
                }


            }

        }

        private void gvDevBooking_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            char[] Alph = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();
            char[] Alph1 = Enumerable.Range('1', '8' - '1' + 1).Select(i => (char)i).ToArray();


            if (!string.IsNullOrEmpty(e.CellValue?.ToString()))
            {
                foreach (var v in Alph)
                {
                    if (e.CellValue.ToString().ToUpper().Contains(v))
                    {
                        e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                        foreach (var s in Alph1)
                        {
                            if (e.CellValue.ToString().ToUpper().Contains(s) && e.Column.Caption != "Workplace" && e.Column.Caption != "Shift Boss" && e.Column.Caption != "Gang")
                            {
                                e.Appearance.ForeColor = Color.Red;
                            }
                        }
                        ++count2;

                    }



                }
                ++count;

                //Add coloured rows
                var tmpCell = e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo;
                if (tmpCell.Column.Name.Contains("DevD"))
                {
                    var dtRow = tmpCell.RowInfo.RowKey as DataRow;
                    string dayNum = "ABS" + tmpCell.Column.Name.Split('D')[2];
                    //if (string.IsNullOrWhiteSpace(dtRow[dayNum].ToString()))
                    //{
                    //    //do nothing
                    //}
                    if (dtRow[dayNum].ToString() == "A")
                    {
                        e.Appearance.BackColor = _colourA;
                    }
                    else if (dtRow[dayNum].ToString() == "B" || dtRow[dayNum].ToString() == "PB")
                    {
                        e.Appearance.BackColor = _colourB;
                    }
                    else if (dtRow[dayNum].ToString() == "S" || dtRow[dayNum].ToString() == "PS")
                    {
                        e.Appearance.BackColor = _colourS;
                    }
                }

                //Set colour to be "disabled"
                if (e.CellValue.ToString() == "OFF")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                    e.Appearance.ForeColor = Color.Gainsboro;
                }

            }
        }

        private void gvStopingBooking_DoubleClick(object sender, EventArgs e)
        {
            string colCaption = gvStopingBooking.FocusedColumn.Caption;

            if (colCaption == "Workplace")
            {
                var tmp = new frmBookingProduction();
                //tmp.Icon = new System.Drawing.Icon(@"\\10.148.225.119\Mineware\Syncromine\Latest\Current\Syncromine\SM.ico");
                //tmp.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                tmp.WorkplaceName = gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["description"]).ToString();
                var wpDetails = getWorkplaceDetails(tmp.WorkplaceName);
                tmp.WpID = wpDetails[0];
                tmp.SectionID = Mineware.Systems.ProductionGlobal.ProductionGlobal.ExtractBeforeColon(gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["sb"]).ToString());
                tmp.Activity = string.IsNullOrWhiteSpace(wpDetails[1]) == true ? 0 : Convert.ToInt32(wpDetails[1]);
                tmp.CanChangeDate = true;
                tmp.TheDate = getADateForWPSection(tmp.WpID, tmp.SectionID);
                tmp.IsNightShiftBooking = false;
                tmp.connection = UserCurrentInfo.Connection;
                tmp.theSystemDBTag = theSystemDBTag;
                tmp.WPAct = Convert.ToInt32(Math.Round(Convert.ToDecimal(gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["Activity"]).ToString()), 0));
                tmp.MinerID = gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["SectionID"]).ToString();
                tmp.MOID = barSection.EditValue.ToString();
                tmp.Selectedprodmonth = Mineware.Systems.ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdMonth.EditValue));
                tmp.pnlStopingBooked.Visible = true;
                tmp.WindowState = FormWindowState.Maximized;
                tmp.ShowDialog();

                loadData();
            }

            string CellVal = gvStopingBooking.GetRowCellValue(gvStopingBooking.FocusedRowHandle, gvStopingBooking.FocusedColumn).ToString();
            if (CellVal == "OFF")
            {
                string currDate = gvStopingBooking.FocusedColumn.Caption;
                string currDate2 = currDate.Substring(1, 5);
                DateTime DtDate;

                try
                {
                    DtDate = Convert.ToDateTime(currDate);
                }
                catch (Exception)
                {
                    DtDate = Convert.ToDateTime(currDate2 + " " + _prodMonth.Substring(0, 4));
                }

                string WP = gvStopingBooking.GetRowCellValue(gvStopingBooking.FocusedRowHandle, gvStopingBooking.Columns["description"]).ToString();
                int WpAct = Convert.ToInt32(Math.Round(Convert.ToDecimal(gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["Activity"]).ToString()), 0));
                var wpDetails = getWorkplaceDetails(WP);
                string WpID = wpDetails[0];

                DialogResult result;
                result = MessageBox.Show("Do you want to make this shift " + currDate + " a Puma Hola Shift", "Shift Change", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
                    _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbMan1.SqlStatement = " update tbl_Planning set  Pumahola = 'Y'  " +
                    " where workplaceid = '" + WpID + "'  " +
                    " and calendardate = '" + Convert.ToDateTime(DtDate).ToString("yyyy-MM-dd") + "' \r\n" +
                    " and activity = '" + WpAct + "'  ";

                    _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan1.ExecuteInstruction();
                }
            }

            //if (colCaption != "Workplace" && colCaption != "Shift Boss" && colCaption != "Gang" && colCaption != "Plan" && colCaption != "Book" && colCaption != "F/C" && colCaption != "Var.")
            //{
            //    var tmp = new frmBookingDetail();
            //    //tmp.Icon = new System.Drawing.Icon(@"\\10.148.225.119\Mineware\Syncromine\Latest\Current\Syncromine\SM.ico");
            //    //tmp.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            //    tmp.WorkplaceName = gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["description"]).ToString();
            //    var wpDetails = getWorkplaceDetails(tmp.WorkplaceName);
            //    tmp.WpID = wpDetails[0];
            //    tmp.SectionID = Mineware.Systems.ProductionGlobal.ProductionGlobal.ExtractBeforeColon(gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["sb"]).ToString());
            //    tmp.Activity = string.IsNullOrWhiteSpace(wpDetails[1]) == true ? 0 : Convert.ToInt32(wpDetails[1]);
            //    //tmp.CanChangeDate = true;
            //    tmp.TheDate = Convert.ToDateTime(colCaption);
            //    tmp.IsNightShiftBooking = false;
            //    tmp._connectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection); ;
            //    tmp.WPAct = Convert.ToInt32(Math.Round(Convert.ToDecimal(gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["Activity"]).ToString()), 0));
            //    tmp.MinerID = gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["SectionID"]).ToString();
            //    tmp.MOID = barSection.EditValue.ToString();
            //    tmp.Selectedprodmonth = Mineware.Systems.ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdMonth.EditValue));
            //    tmp.pnlStopingBooked.Visible = true;
            //    //tmp.WindowState = FormWindowState.Maximized;
            //    tmp.ShowDialog();

            //}

        }

        private void gvDevBooking_DoubleClick(object sender, EventArgs e)
        {
            string colCaption = gvDevBooking.FocusedColumn.Caption;

            if (colCaption == "Workplace")
            {
                var tmp = new frmBookingProduction();
                //tmp.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                tmp.WorkplaceName = gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["description"]).ToString();
                var wpDetails = getWorkplaceDetails(tmp.WorkplaceName);
                tmp.WpID = wpDetails[0];
                tmp.SectionID = Mineware.Systems.ProductionGlobal.ProductionGlobal.ExtractBeforeColon(gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["sb"]).ToString());
                tmp.Activity = string.IsNullOrWhiteSpace(wpDetails[1]) == true ? 0 : Convert.ToInt32(wpDetails[1]);
                tmp.CanChangeDate = true;
                tmp.TheDate = getADateForWPSection(tmp.WpID, tmp.SectionID);
                tmp.IsNightShiftBooking = false;
                tmp.connection = UserCurrentInfo.Connection;
                tmp.theSystemDBTag = theSystemDBTag;
                tmp.WPAct = Convert.ToInt32(Math.Round(Convert.ToDecimal(gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["Activity"]).ToString()), 0));
                tmp.MinerID = gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["SectionID"]).ToString();
                tmp.MOID = barSection.EditValue.ToString();
                tmp.Selectedprodmonth = Mineware.Systems.ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdMonth.EditValue));
                tmp.pnlDevBooking.Visible = true;
                //tmp.WindowState = FormWindowState.Maximized;
                tmp.ShowDialog();

                loadData();
            }

            string CellVal = gvDevBooking.GetRowCellValue(gvDevBooking.FocusedRowHandle, gvDevBooking.FocusedColumn).ToString();

            if (CellVal == "OFF")
            {
                string currDate = gvDevBooking.FocusedColumn.Caption;
                DateTime DtDate;

                try
                {
                    DtDate = Convert.ToDateTime(currDate);
                }
                catch (Exception)
                {
                    DtDate = Convert.ToDateTime(currDate + " " + _prodMonth.Substring(0, 4));
                }

                string WP = gvDevBooking.GetRowCellValue(gvDevBooking.FocusedRowHandle, gvDevBooking.Columns["description"]).ToString();
                int WpAct = Convert.ToInt32(Math.Round(Convert.ToDecimal(gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["Activity"]).ToString()), 0));
                var wpDetails = getWorkplaceDetails(WP);
                string WpID = wpDetails[0];

                DialogResult result;
                result = MessageBox.Show("Do you want to make this shift " + currDate + " a Puma Hola Shift", "Shift Change", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
                    _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbMan1.SqlStatement = " update tbl_Planning set  Pumahola = 'Y'  " +
                    " where workplaceid = '" + WpID + "'  " +
                    " and calendardate = '" + Convert.ToDateTime(DtDate).ToString("yyyy-MM-dd") + "' \r\n" +
                    " and activity = '" + WpAct + "'  ";

                    _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan1.ExecuteInstruction();
                }
            }

            //if (colCaption != "Workplace" && colCaption != "Shift Boss" && colCaption != "Gang" && colCaption != "Plan" && colCaption != "Book" && colCaption != "F/C" && colCaption != "Var.")
            //{
            //    var tmp = new frmBookingDetail();
            //    //tmp.Icon = new System.Drawing.Icon(@"\\10.148.225.119\Mineware\Syncromine\Latest\Current\Syncromine\SM.ico");
            //    //tmp.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            //    tmp.WorkplaceName = gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["description"]).ToString();
            //    var wpDetails = getWorkplaceDetails(tmp.WorkplaceName);
            //    tmp.WpID = wpDetails[0];
            //    tmp.SectionID = Mineware.Systems.ProductionGlobal.ProductionGlobal.ExtractBeforeColon(gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["sb"]).ToString());
            //    tmp.Activity = string.IsNullOrWhiteSpace(wpDetails[1]) == true ? 0 : Convert.ToInt32(wpDetails[1]);
            //    //tmp.CanChangeDate = true;
            //    tmp.TheDate = Convert.ToDateTime(colCaption);
            //    tmp.IsNightShiftBooking = false;
            //    tmp._connectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection); ;
            //    tmp.WPAct = Convert.ToInt32(Math.Round(Convert.ToDecimal(gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["Activity"]).ToString()), 0));
            //    tmp.MinerID = gvStopingBooking.GetFocusedRowCellValue(gvDevBooking.Columns["SectionID"]).ToString();
            //    tmp.MOID = barSection.EditValue.ToString();
            //    tmp.Selectedprodmonth = Mineware.Systems.ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdMonth.EditValue));
            //    tmp.pnlStopingBooked.Visible = true;
            //    tmp.WindowState = FormWindowState.Maximized;
            //    tmp.ShowDialog();

            //}
        }

        /// <summary>
        /// Gets workplaceID and Activity
        /// </summary>
        /// <param name="workplaceName"></param>
        /// <returns></returns>
        private string[] getWorkplaceDetails(string workplaceName)
        {
            string[] str = new string[2];

            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = connectionStr;
            theData.SqlStatement = @"SELECT [workplaceid], [Activity] FROM [dbo].[tbl_Workplace] WHERE [Description] = '" + workplaceName + @"'";
            theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = ReturnType.DataTable;
            theData.ExecuteInstruction();

            if (theData.ResultsDataTable != null || theData.ResultsDataTable?.Rows.Count > 0)
            {
                str[0] = theData.ResultsDataTable.Rows[0][0].ToString();
                str[1] = theData.ResultsDataTable.Rows[0][1].ToString();
            }

            return str;
        }

        private DateTime getADateForWPSection(string WpID, string SectionID)
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = connectionStr;
            theData.SqlStatement = "SELECT TOP 1 [CalendarDate] FROM dbo.tbl_Planning WHERE SectionID = '" + SectionID + @"' AND WorkplaceID = '" + WpID + @"'";
            theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = ReturnType.DataTable;
            theData.ExecuteInstruction();

            if (theData.ResultsDataTable != null && theData.ResultsDataTable?.Rows.Count != 0)
            {
                if (string.IsNullOrWhiteSpace(theData.ResultsDataTable.Rows[0][0].ToString()))
                {
                    return DateTime.Now;
                }
                else
                {
                    return Convert.ToDateTime(theData.ResultsDataTable.Rows[0][0].ToString());
                }
            }
            else
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Loads the sections into the combobox
        /// </summary>
        private void LoadSections()
        {
            //MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
            //minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            //minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            //minerData.SqlStatement = "Select SectionID,Name from tbl_Section where prodmonth = '" + _prodMonth + "' and Hierarchicalid = '4'  order by SectionID";

            //var theResult = minerData.ExecuteInstruction();

            //DataTable dtSections = new DataTable();
            //dtSections = minerData.ResultsDataTable;

            //if (dtSections.Rows.Count > 0)
            //{
            //    repSleSection.DataSource = null;
            //    repSleSection.DataSource = dtSections;
            //    repSleSection.DisplayMember = "Name";
            //    repSleSection.ValueMember = "SectionID";
            //    repSleSection.PopulateViewColumns();
            //    repSleSection.View.Columns[0].Width = 80;
            //    barSection.EditValue = dtSections.Rows[0][0];
            //}


            //if (dtAllSections.Rows.Count == 0)
            //    theMessage.viewMessage(MessageType.Info, "NO SECTIONS", "There are no sections avaliable for production month " + THarmonyPASGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue)), ButtonTypes.OK, MessageDisplayType.FullScreen);

            ///New Kelvin (Load Sections for Current Users)
            DataTable dtBookSection = new DataTable();
            _clsBookings._theConnection = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            dtBookSection = _clsBookings.get_UserBookSection(_prodMonth, UserCurrentInfo.UserID);
            if (dtBookSection.Rows.Count > 0)
            {
                repSleSection.DataSource = dtBookSection;
                repSleSection.ValueMember = "SectionID";
                repSleSection.DisplayMember = "Name";
                barSection.EditValue = dtBookSection.Rows[0][0];
            }
        }

        private void barProdMonth_EditValueChanged(object sender, EventArgs e)
        {
            _prodMonth = ProductionGlobal.ProductionGlobal.ProdMonthAsString((DateTime)barProdMonth.EditValue);
            LoadSections();

            if (IsFirsload != "Y")
            {
                loadData();
            }
        }

        private void barSection_EditValueChanged(object sender, EventArgs e)
        {
            _sectionID = barSection.EditValue.ToString();
            if (_sectionID != "A119")
            {
                lblReefDrill.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            loadData();
        }

        private void loadData()
        {
            clsDataAccess _dbCal = new clsDataAccess();
            _dbCal.ConnectionString = connectionStr;
            _dbCal.SqlStatement = "Select max(BeginDate) BeginDate , max(EndDate) EndDate from tbl_Code_Calendar_Section \r\n" +
                                  "where Prodmonth = '" + _prodMonth + "' and SectionID like '" + _sectionID + "%' ";
            _dbCal.queryExecutionType = ExecutionType.GeneralSQLStatement;
            _dbCal.queryReturnType = ReturnType.DataTable;
            _dbCal.ExecuteInstruction();

            DataTable dtCal = _dbCal.ResultsDataTable;

            DateTime StartDate = Convert.ToDateTime(dtCal.Rows[0]["BeginDate"]);
            DateTime EndDate = Convert.ToDateTime(dtCal.Rows[0]["EndDate"]);

            int daysDiff = (EndDate - StartDate).Days;

            #region Stoping

            //Load Stoping data            
            clsDataAccess _dbStp = new clsDataAccess();
            _dbStp.ConnectionString = connectionStr;
            _dbStp.SqlStatement = "exec sp_Bookings_Stoping " + _prodMonth + "," + _sectionID + "," + _numDay;
            _dbStp.queryExecutionType = ExecutionType.GeneralSQLStatement;
            _dbStp.queryReturnType = ReturnType.DataTable;
            _dbStp.ExecuteInstruction();

            DataTable dtStp = _dbStp.ResultsDataTable;

            DataSet dsStp = new DataSet();
            dsStp.Tables.Add(dtStp);

            colStpSB.FieldName = "sb";
            colStpGang.FieldName = "OrgUnitDS";
            colStpWP.FieldName = "description";
            colProgPlan.FieldName = "progplansqm";
            colProgBook.FieldName = "progbooksqm";
            colMonthlyPlan.FieldName = "monthplansqm";
            colForeCast.FieldName = "fc";
            colPlanVariance.FieldName = "variance";
            colStpD1.FieldName = "day1";
            colStpD2.FieldName = "day2";
            colStpD3.FieldName = "day3";
            colStpD4.FieldName = "day4";
            colStpD5.FieldName = "day5";
            colStpD6.FieldName = "day6";
            colStpD7.FieldName = "day7";
            colStpD8.FieldName = "day8";
            colStpD9.FieldName = "day9";
            colStpD10.FieldName = "day10";
            colStpD11.FieldName = "day11";
            colStpD12.FieldName = "day12";
            colStpD13.FieldName = "day13";
            colStpD14.FieldName = "day14";
            colStpD15.FieldName = "day15";
            colStpD16.FieldName = "day16";
            colStpD17.FieldName = "day17";
            colStpD18.FieldName = "day18";
            colStpD19.FieldName = "day19";
            colStpD20.FieldName = "day20";
            colStpD21.FieldName = "day21";
            colStpD22.FieldName = "day22";
            colStpD23.FieldName = "day23";
            colStpD24.FieldName = "day24";
            colStpD25.FieldName = "day25";
            colStpD26.FieldName = "day26";
            colStpD27.FieldName = "day27";
            colStpD28.FieldName = "day28";
            colStpD29.FieldName = "day29";
            colStpD30.FieldName = "day30";
            colStpD31.FieldName = "day31";
            colStpD32.FieldName = "day32";
            colStpD33.FieldName = "day33";
            colStpD34.FieldName = "day34";
            colStpD35.FieldName = "day35";
            colStpD36.FieldName = "day36";
            colStpD37.FieldName = "day37";
            colStpD38.FieldName = "day38";
            colStpD39.FieldName = "day39";
            colStpD40.FieldName = "day40";
            colStpD41.FieldName = "day41";
            colStpD42.FieldName = "day42";
            colStpD43.FieldName = "day43";
            colStpD44.FieldName = "day44";
            colStpD45.FieldName = "day45";
            colStpD46.FieldName = "day46";
            colStpD47.FieldName = "day47";
            colStpD48.FieldName = "day48";
            colStpD49.FieldName = "day49";
            colStpD50.FieldName = "day50";
            colStpD51.FieldName = "day51";
            colStpD52.FieldName = "day52";
            colStpD53.FieldName = "day53";
            colStpD54.FieldName = "day54";
            colStpD55.FieldName = "day55";
            colStpD56.FieldName = "day56";
            colStpD57.FieldName = "day57";
            colStpD58.FieldName = "day58";
            colStpD59.FieldName = "day59";
            colStpD60.FieldName = "day60";
            colStpD61.FieldName = "day61";
            colStpD62.FieldName = "day62";
            colStpD63.FieldName = "day63";
            colStpD64.FieldName = "day64";
            colStpD65.FieldName = "day65";
            colStpActivity.FieldName = "Activity";
            colStpMinerID.FieldName = "SectionID";

            //fix totals
            for (int x = 0; x < dsStp.Tables[0].Rows.Count; x++)
            {
                if (dsStp.Tables[0].Rows[x][4].ToString() == "z")
                    dsStp.Tables[0].Rows[x][4] = "SP Total";
                else if (dsStp.Tables[0].Rows[x][4].ToString() == "zz")
                    dsStp.Tables[0].Rows[x][4] = string.Empty;

                //Disable colmn if it is < -1000
                //for(int y = 10; y < 70; y++)
                //if(Convert.ToInt32(dsStp.Tables[0].Rows[x][y]) < -1000)
                //{
                //    dsStp.Tables[0].Rows[x][y]
                //}
            }

            gcStopingBooking.DataSource = dsStp.Tables[0];

            if (dtStp.Rows.Count > 0)
            {
                DateTime startdate = Convert.ToDateTime(dtStp.Rows[0]["BeginDate"].ToString());
                int columnIndex = 10;
                int ShiftNo = 0;
                string ShiftOff = "";

                //Headers Date
                for (int i = 0; i < daysDiff + 1; i++)
                {
                    string OffDay = gvStopingBooking.GetRowCellValue(1, gvStopingBooking.Columns[columnIndex].FieldName).ToString();
                    if (OffDay != "OFF")
                    {
                        ShiftNo = ShiftNo + 1;
                        ShiftOff = ShiftNo.ToString();
                    }
                    else
                    {
                        ShiftOff = "OFF";
                    }
                    
                    gvStopingBooking.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM ddd")+" ("+ ShiftOff + ")";
                    gvStopingBooking.Columns[columnIndex].Visible = true;
                    startdate = startdate.AddDays(1);
                    columnIndex++;
                }

                for (int col = columnIndex; col < gvStopingBooking.Columns.Count; col++)
                {
                    gvStopingBooking.Columns[col].Visible = false;
                }
            }

            #endregion

            #region Development

            ///Load Development data
            ///
            clsDataAccess _dbDev = new clsDataAccess();
            _dbDev.ConnectionString = connectionStr;
            _dbDev.SqlStatement = "exec sp_Bookings_Development " + _prodMonth + "," + _sectionID + "," + _numDay;
            _dbDev.queryExecutionType = ExecutionType.GeneralSQLStatement;
            _dbDev.queryReturnType = ReturnType.DataTable;
            _dbDev.ExecuteInstruction();


            DataTable dtDev = _dbDev.ResultsDataTable;

            DataSet dsDev = new DataSet();
            dsDev.Tables.Add(dtDev);

            colDevSB.FieldName = "sb";
            colDevGang.FieldName = "OrgUnitDS";
            colDevWP.FieldName = "description";

            colDevProgPlan.FieldName = "progplansqm";
            colDevProgBook.FieldName = "progbooksqm";
            colDevMonthlyplan.FieldName = "monthplansqm";
            colDevForeCast.FieldName = "fc";
            colDevPlanVar.FieldName = "variance";

            colDevD1.FieldName = "day1";
            colDevD2.FieldName = "day2";
            colDevD3.FieldName = "day3";
            colDevD4.FieldName = "day4";
            colDevD5.FieldName = "day5";
            colDevD6.FieldName = "day6";
            colDevD7.FieldName = "day7";
            colDevD8.FieldName = "day8";
            colDevD9.FieldName = "day9";
            colDevD10.FieldName = "day10";
            colDevD11.FieldName = "day11";
            colDevD12.FieldName = "day12";
            colDevD13.FieldName = "day13";
            colDevD14.FieldName = "day14";
            colDevD15.FieldName = "day15";
            colDevD16.FieldName = "day16";
            colDevD17.FieldName = "day17";
            colDevD18.FieldName = "day18";
            colDevD19.FieldName = "day19";
            colDevD20.FieldName = "day20";
            colDevD21.FieldName = "day21";
            colDevD22.FieldName = "day22";
            colDevD23.FieldName = "day23";
            colDevD24.FieldName = "day24";
            colDevD25.FieldName = "day25";
            colDevD26.FieldName = "day26";
            colDevD27.FieldName = "day27";
            colDevD28.FieldName = "day28";
            colDevD29.FieldName = "day29";
            colDevD30.FieldName = "day30";
            colDevD31.FieldName = "day31";
            colDevD32.FieldName = "day32";
            colDevD33.FieldName = "day33";
            colDevD34.FieldName = "day34";
            colDevD35.FieldName = "day35";
            colDevD36.FieldName = "day36";
            colDevD37.FieldName = "day37";
            colDevD38.FieldName = "day38";
            colDevD39.FieldName = "day39";
            colDevD40.FieldName = "day40";
            colDevD41.FieldName = "day41";
            colDevD42.FieldName = "day42";
            colDevD43.FieldName = "day43";
            colDevD44.FieldName = "day44";
            colDevD45.FieldName = "day45";
            colDevD46.FieldName = "day46";
            colDevD47.FieldName = "day47";
            colDevD48.FieldName = "day48";
            colDevD49.FieldName = "day49";
            colDevD50.FieldName = "day50";
            colDevD51.FieldName = "day51";
            colDevD52.FieldName = "day52";
            colDevD53.FieldName = "day53";
            colDevD54.FieldName = "day54";
            colDevD55.FieldName = "day55";
            colDevD56.FieldName = "day56";
            colDevD57.FieldName = "day57";
            colDevD58.FieldName = "day58";
            colDevD59.FieldName = "day59";
            colDevD60.FieldName = "day60";
            colDevD61.FieldName = "day61";
            colDevD62.FieldName = "day62";
            colDevD63.FieldName = "day63";
            colDevD64.FieldName = "day64";
            colDevD65.FieldName = "day65";

            colDevActivity.FieldName = "Activity";
            colDevMinerID.FieldName = "SectionID";

            gcDevBooking.DataSource = dsDev.Tables[0];

            //fix totals
            for (int x = 0; x < dsDev.Tables[0].Rows.Count; x++)
            {
                if (dsDev.Tables[0].Rows[x][4].ToString() == "z")
                    dsDev.Tables[0].Rows[x][4] = "SP Total";
                else if (dsDev.Tables[0].Rows[x][4].ToString() == "zz")
                    dsDev.Tables[0].Rows[x][4] = string.Empty;
            }

            if (dtDev.Rows.Count > 0)
            {
                DateTime startdate = Convert.ToDateTime(dtDev.Rows[0]["BeginDate"].ToString());
                int columnIndex = 10;
                int ShiftNo = 0;
                string ShiftOff = "";

                //Headers Date
                for (int i = 0; i < daysDiff + 1; i++)
                {
                    string OffDay = gvDevBooking.GetRowCellValue(1, gvDevBooking.Columns[columnIndex].FieldName).ToString();
                    if (OffDay != "OFF")
                    {
                        ShiftNo = ShiftNo + 1;
                        ShiftOff = ShiftNo.ToString();
                    }
                    else
                    {
                        ShiftOff = "OFF";
                    }

                    gvDevBooking.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM ddd") + " (" + ShiftOff + ")";
                    gvDevBooking.Columns[columnIndex].Visible = true;
                    startdate = startdate.AddDays(1);
                    columnIndex++;
                }
                for (int col = columnIndex; col < gvDevBooking.Columns.Count; col++)
                {
                    gvDevBooking.Columns[col].Visible = false;
                }


            }



            #endregion

            IsFirsload = "N";
        }

        /// <summary>
        /// Get the colours to colour the column rows with
        /// </summary>
        private void getColoursForABS()
        {
            //Load Colours       
            clsDataAccess _dbColours = new clsDataAccess();
            _dbColours.ConnectionString = connectionStr;
            _dbColours.SqlStatement = "SELECT [A_Color], [B_Color], [S_Color] FROM [dbo].[tbl_sysSet]";
            _dbColours.queryExecutionType = ExecutionType.GeneralSQLStatement;
            _dbColours.queryReturnType = ReturnType.DataTable;
            _dbColours.ExecuteInstruction();

            _colourA = Color.FromArgb(Convert.ToInt32(_dbColours.ResultsDataTable.Rows[0][0]));
            _colourB = Color.FromArgb(Convert.ToInt32(_dbColours.ResultsDataTable.Rows[0][1]));
            _colourS = Color.FromArgb(Convert.ToInt32(_dbColours.ResultsDataTable.Rows[0][2]));

            aEdit.EditValue = _colourA;
            bEdit.EditValue = _colourB;
            sEdit.EditValue = _colourS;
        }

        private void gvStopingBooking_RowStyle(object sender, RowStyleEventArgs e)
        {
            DevExpress.XtraGrid.Views.BandedGrid.BandedGridView view = (DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)sender;
            if (view.IsValidRowHandle(e.RowHandle))
            {
                if (view.GetRowCellValue(e.RowHandle, "description")?.ToString() == "SP Total")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                }

                if (view.GetRowCellValue(e.RowHandle, "description")?.ToString() == "")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                }

            }
        }

        private void gvDevBooking_RowStyle(object sender, RowStyleEventArgs e)
        {
            DevExpress.XtraGrid.Views.BandedGrid.BandedGridView view = (DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)sender;
            if (view.IsValidRowHandle(e.RowHandle))
            {
                if (view.GetRowCellValue(e.RowHandle, "description")?.ToString() == "SP Total")
                {
                    e.Appearance.BackColor = Color.Gainsboro;

                }
                if (view.GetRowCellValue(e.RowHandle, "description")?.ToString() == "")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                }
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWordEditor helpFrm = new frmWordEditor();
            helpFrm.ViewType = "View";
            helpFrm.MainCat = "Bookings";
            if (tabBookings.SelectedPage.Caption == "Stoping")
            {
                helpFrm.SubCat = "Stoping";
            }
            else
            {
                helpFrm.SubCat = "Development";
            }
            helpFrm.Show();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (tabBookings.SelectedPage == tabStoping)
            {
                string path = "DailyBookings(Stoping).xlsx";
                gvStopingBooking.ExportToXlsx(path);
                // Open the created XLSX file with the default application.
                Process.Start(path);
            }
            else
            {
                string path = "DailyBookings(Development).xlsx";
                gvStopingBooking.ExportToXlsx(path);
                // Open the created XLSX file with the default application.
                Process.Start(path);
            }
        }
        

        private void btnPDF_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            PdfExportOptions options = new PdfExportOptions();
            if (tabBookings.SelectedPage == tabStoping)
            {
                string path = "DailyBookings(Stoping).pdf";                
                options.DocumentOptions.Author = "Mineware";
               // options.ConvertImagesToJpeg = true;
                //options.ExportEditingFieldsToAcroForms = true;
                //options.ShowPrintDialogOnOpen = true;
                //options.PdfACompatibility = PdfACompatibility.PdfA2b;
                //options.RasterizationResolution = 1240;
                options.PageRange = "3";
                //options.DocumentOptions.Author = "Mineware";
                //options.Compressed = false;
                //options.ImageQuality = PdfJpegImageQuality.Highest;
                //gvStopingBooking.Se.
                gvStopingBooking.ExportToPdf(path);               
                System.Diagnostics.Process.Start(path);

                

            }
            else
            {
                string path = "DailyBookings(Development).pdf";
                options.DocumentOptions.Author = "Mineware";
                options.ConvertImagesToJpeg = true;
                options.ExportEditingFieldsToAcroForms = true;
                options.PdfACompatibility = PdfACompatibility.PdfA2b;
                options.RasterizationResolution = 1240;
                options.ShowPrintDialogOnOpen = true;
                options.PageRange = "3";
                options.DocumentOptions.Author = "Mineware";
                options.Compressed = true;
                options.ImageQuality = PdfJpegImageQuality.Highest;
                gvDevBooking.ExportToPdf(path, options);
                System.Diagnostics.Process.Start(path);
            }
        }
    }
}
