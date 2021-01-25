using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using MWDataManager;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Bookings
{
    public partial class ucBookingProduction_MainNS : BaseUserControl
    {
        private string connectionStr;
        private string _prodMonth = ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString();
        private string _sectionID = string.Empty;
        private string _numDay = "2";
        private int count = 0;

        private DataTable dtProblems;
        private DataTable dtStoppages;

        public ucBookingProduction_MainNS()
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

            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = connectionStr;
            theData.SqlStatement = "Select ProblemID from  [tbl_Code_Problem_Main]";
            theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = ReturnType.DataTable;
            theData.ExecuteInstruction();

            dtProblems = theData.ResultsDataTable;

            clsDataAccess theDataPS = new clsDataAccess();
            theDataPS.ConnectionString = connectionStr;
            theDataPS.SqlStatement = "SELECT [Code] FROM tbl_Code_Cycle WHERE Code <> ''";
            theDataPS.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theDataPS.queryReturnType = ReturnType.DataTable;
            theDataPS.ExecuteInstruction();

            dtStoppages = theDataPS.ResultsDataTable;

            //Set to current production month           
            barProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(_prodMonth);
        }

        private void gvStopingBooking_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            char[] Alph = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();

            if (!string.IsNullOrEmpty(e.CellValue.ToString()))
            {
                foreach (var v in Alph)
                {
                    if (e.CellValue.ToString().ToUpper().Contains(v))
                    {
                        e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                        //e.Appearance.ForeColor = Color.Gainsboro;
                    }
                }
                ++count;

                if (e.CellValue.ToString() == "OFF")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                    e.Appearance.ForeColor = Color.Gainsboro;
                }

                foreach (DataRow dr in dtProblems.Rows)
                {
                    if (e.CellValue.ToString() == dr["ProblemID"].ToString())
                    {
                        e.Appearance.ForeColor = Color.Tomato;
                    }
                }

                foreach (DataRow dr in dtStoppages.Rows)
                {
                    if (e.CellValue.ToString() == dr["Code"].ToString())
                    {
                        e.Appearance.ForeColor = Color.SteelBlue;
                    }
                }
            }

        }

        private void gvDevBooking_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            char[] Alph = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();

            if (!string.IsNullOrEmpty(e.CellValue.ToString()))
            {
                foreach (var v in Alph)
                {
                    if (e.CellValue.ToString().Contains(v))
                    {
                        e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                        //e.Appearance.ForeColor = Color.Gainsboro;
                    }
                }
                ++count;

                if (e.CellValue.ToString() == "OFF")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                    e.Appearance.ForeColor = Color.Gainsboro;
                }

                foreach (DataRow dr in dtProblems.Rows)
                {
                    if (e.CellValue.ToString() == dr["ProblemID"].ToString())
                    {
                        e.Appearance.ForeColor = Color.Tomato;
                    }
                }

                foreach (DataRow dr in dtStoppages.Rows)
                {
                    if (e.CellValue.ToString() == dr["Code"].ToString())
                    {
                        e.Appearance.ForeColor = Color.SteelBlue;
                    }
                }

            }
        }

        private void gvStopingBooking_DoubleClick(object sender, EventArgs e)
        {
            string colCaption = gvStopingBooking.FocusedColumn.Caption;

            if (colCaption == "Workplace")
            {
                string PlanFL = gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["pfl"]).ToString();

                var tmp = new frmBookingProductionNS();
                //tmp.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                tmp.WorkplaceName = gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["description"]).ToString();
                var wpDetails = getWorkplaceDetails(tmp.WorkplaceName);
                tmp.WpID = wpDetails[0];
                tmp.SectionID = Mineware.Systems.ProductionGlobal.ProductionGlobal.ExtractBeforeColon(gvStopingBooking.GetFocusedRowCellValue(gvStopingBooking.Columns["sb"]).ToString());
                tmp.Activity = string.IsNullOrWhiteSpace(wpDetails[1]) == true ? 0 : Convert.ToInt32(wpDetails[1]);
                tmp.CanChangeDate = true;
                tmp.TheDate = getADateForWPSection(tmp.WpID, tmp.SectionID);
                tmp.PlanFL = PlanFL;
                //tmp.IsNightShiftBooking = true;
                tmp.connection = UserCurrentInfo.Connection;
                tmp.theSystemDBTag = theSystemDBTag;
                tmp.WindowState = FormWindowState.Maximized;
                tmp.ShowDialog();

                loadData();
            }
        }

        private void gvDevBooking_DoubleClick(object sender, EventArgs e)
        {
            string colCaption = gvDevBooking.FocusedColumn.Caption;

            if (colCaption == "Workplace")
            {
                string PlanFL = gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["pfl"]).ToString();

                var tmp = new frmBookingProductionNS();
                // tmp.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                tmp.WorkplaceName = gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["description"]).ToString();
                var wpDetails = getWorkplaceDetails(tmp.WorkplaceName);
                tmp.WpID = wpDetails[0];
                tmp.SectionID = Mineware.Systems.ProductionGlobal.ProductionGlobal.ExtractBeforeColon(gvDevBooking.GetFocusedRowCellValue(gvDevBooking.Columns["sb"]).ToString());
                tmp.Activity = string.IsNullOrWhiteSpace(wpDetails[1]) == true ? 0 : Convert.ToInt32(wpDetails[1]);
                tmp.CanChangeDate = true;
                tmp.TheDate = getADateForWPSection(tmp.WpID, tmp.SectionID);
                tmp.PlanFL = PlanFL;
                //tmp.IsNightShiftBooking = true;
                tmp.connection = UserCurrentInfo.Connection;
                tmp.theSystemDBTag = theSystemDBTag;
                tmp.WindowState = FormWindowState.Maximized;
                tmp.ShowDialog();

                loadData();
            }
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

            if (theData.ResultsDataTable != null && theData.ResultsDataTable?.Rows.Count > 0)
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
            theData.SqlStatement = "select max(CalendarDate) from tbl_Planning where CalendarDate < Getdate() and WorkingDay = 'Y' and workplaceid = '" + WpID + "' and sectionid = '" + SectionID + "'";
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
            MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
            minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            minerData.SqlStatement = "Select SectionID,Name from tbl_Section where prodmonth = '" + _prodMonth + "' and Hierarchicalid = '4'  order by SectionID";

            var theResult = minerData.ExecuteInstruction();

            DataTable dtSections = new DataTable();
            dtSections = minerData.ResultsDataTable;

            if (dtSections.Rows.Count > 0)
            {
                repSleSection.DataSource = null;
                repSleSection.DataSource = dtSections;
                repSleSection.DisplayMember = "Name";
                repSleSection.ValueMember = "SectionID";
                repSleSection.PopulateViewColumns();
                repSleSection.View.Columns[0].Width = 80;
                barSection.EditValue = dtSections.Rows[0][0];
            }


            //if (dtAllSections.Rows.Count == 0)
            //    theMessage.viewMessage(MessageType.Info, "NO SECTIONS", "There are no sections avaliable for production month " + THarmonyPASGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue)), ButtonTypes.OK, MessageDisplayType.FullScreen);
        }

        private void barProdMonth_EditValueChanged(object sender, EventArgs e)
        {
            _prodMonth = ProductionGlobal.ProductionGlobal.ProdMonthAsString((DateTime)barProdMonth.EditValue);
            LoadSections();
            loadData();
        }

        private void barSection_EditValueChanged(object sender, EventArgs e)
        {
            _sectionID = barSection.EditValue.ToString();
            loadData();
        }

        private void loadData()
        {
            clsDataAccess _dbCal = new clsDataAccess();
            _dbCal.ConnectionString = connectionStr;
            _dbCal.SqlStatement = "Select max(BeginDate) BeginDate , max(EndDate) EndDate from tbl_Code_Calendar_Section \r\n" +
                                  "where Prodmonth = '" + _prodMonth + "' and CalendarCode = '" + _sectionID + "' ";
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
            _dbStp.SqlStatement = "exec sp_Bookings_StopingNS " + _prodMonth + "," + _sectionID + "," + _numDay;
            _dbStp.queryExecutionType = ExecutionType.GeneralSQLStatement;
            _dbStp.queryReturnType = ReturnType.DataTable;
            _dbStp.ExecuteInstruction();

            DataTable dtStp = _dbStp.ResultsDataTable;

            DataSet dsStp = new DataSet();
            dsStp.Tables.Add(dtStp);

            colStpPlan.FieldName = "pfl";
            colStpSB.FieldName = "sb";
            colStpGang.FieldName = "orgunitds";
            colStpWP.FieldName = "description";
            colStpShift.FieldName = "SHiftName";
            colStpD1.FieldName = "D1";
            colStpD2.FieldName = "D2";
            colStpD3.FieldName = "D3";
            colStpD4.FieldName = "D4";
            colStpD5.FieldName = "D5";
            colStpD6.FieldName = "D6";
            colStpD7.FieldName = "D7";
            colStpD8.FieldName = "D8";
            colStpD9.FieldName = "D9";
            colStpD10.FieldName = "D10";
            colStpD11.FieldName = "D11";
            colStpD12.FieldName = "D12";
            colStpD13.FieldName = "D13";
            colStpD14.FieldName = "D14";
            colStpD15.FieldName = "D15";
            colStpD16.FieldName = "D16";
            colStpD17.FieldName = "D17";
            colStpD18.FieldName = "D18";
            colStpD19.FieldName = "D19";
            colStpD20.FieldName = "D20";
            colStpD21.FieldName = "D21";
            colStpD22.FieldName = "D22";
            colStpD23.FieldName = "D23";
            colStpD24.FieldName = "D24";
            colStpD25.FieldName = "D25";
            colStpD26.FieldName = "D26";
            colStpD27.FieldName = "D27";
            colStpD28.FieldName = "D28";
            colStpD29.FieldName = "D29";
            colStpD30.FieldName = "D30";
            colStpD31.FieldName = "D31";
            colStpD32.FieldName = "D32";
            colStpD33.FieldName = "D33";
            colStpD34.FieldName = "D34";
            colStpD35.FieldName = "D35";
            colStpD36.FieldName = "D36";
            colStpD37.FieldName = "D37";
            colStpD38.FieldName = "D38";
            colStpD39.FieldName = "D39";
            colStpD40.FieldName = "D40";
            colStpD41.FieldName = "D41";
            colStpD42.FieldName = "D42";
            colStpD43.FieldName = "D43";
            colStpD44.FieldName = "D44";
            colStpD45.FieldName = "D45";
            colStpD46.FieldName = "D46";
            colStpD47.FieldName = "D47";
            colStpD48.FieldName = "D48";
            colStpD49.FieldName = "D49";
            colStpD50.FieldName = "D50";
            colStpD51.FieldName = "D51";
            colStpD52.FieldName = "D52";
            colStpD53.FieldName = "D53";
            colStpD54.FieldName = "D54";
            colStpD55.FieldName = "D55";
            colStpD56.FieldName = "D56";
            colStpD57.FieldName = "D57";
            colStpD58.FieldName = "D58";
            colStpD59.FieldName = "D59";
            colStpD60.FieldName = "D60";
            colStpD61.FieldName = "D61";
            colStpD62.FieldName = "D62";
            colStpD63.FieldName = "D63";
            colStpD64.FieldName = "D64";
            colStpD65.FieldName = "D65";

            //fix totals
            for (int x = 0; x < dsStp.Tables[0].Rows.Count; x++)
            {
                if (dsStp.Tables[0].Rows[x][4].ToString() == "z")
                    dsStp.Tables[0].Rows[x][4] = "SP Total";
                else if (dsStp.Tables[0].Rows[x][4].ToString() == "zz")
                    dsStp.Tables[0].Rows[x][4] = string.Empty;
            }
            gcStopingBooking.DataSource = dsStp.Tables[0];

            if (dtStp.Rows.Count > 0)
            {
                DateTime startdate = Convert.ToDateTime(dtStp.Rows[0]["BeginDate"].ToString());
                int columnIndex = 5;

                //Headers Date
                for (int i = 0; i < daysDiff + 1; i++)
                {
                    gvStopingBooking.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM ddd");
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

            #region Develpment

            ///Load Development data
            ///
            clsDataAccess _dbDev = new clsDataAccess();
            _dbDev.ConnectionString = connectionStr;
            _dbDev.SqlStatement = "exec sp_Bookings_DevelopmentNS " + _prodMonth + "," + _sectionID + "," + _numDay;
            _dbDev.queryExecutionType = ExecutionType.GeneralSQLStatement;
            _dbDev.queryReturnType = ReturnType.DataTable;
            _dbDev.ExecuteInstruction();

            DataTable dtDev = _dbDev.ResultsDataTable;

            DataSet dsDev = new DataSet();
            dsDev.Tables.Add(dtDev);

            colDevPlan.FieldName = "pfl";
            colDevSB.FieldName = "sb";
            colDevGang.FieldName = "orgunitds";
            colDevWP.FieldName = "description";
            colDevShift.FieldName = "SHiftName";
            colDevD1.FieldName = "D1";
            colDevD2.FieldName = "D2";
            colDevD3.FieldName = "D3";
            colDevD4.FieldName = "D4";
            colDevD5.FieldName = "D5";
            colDevD6.FieldName = "D6";
            colDevD7.FieldName = "D7";
            colDevD8.FieldName = "D8";
            colDevD9.FieldName = "D9";
            colDevD10.FieldName = "D10";
            colDevD11.FieldName = "D11";
            colDevD12.FieldName = "D12";
            colDevD13.FieldName = "D13";
            colDevD14.FieldName = "D14";
            colDevD15.FieldName = "D15";
            colDevD16.FieldName = "D16";
            colDevD17.FieldName = "D17";
            colDevD18.FieldName = "D18";
            colDevD19.FieldName = "D19";
            colDevD20.FieldName = "D20";
            colDevD21.FieldName = "D21";
            colDevD22.FieldName = "D22";
            colDevD23.FieldName = "D23";
            colDevD24.FieldName = "D24";
            colDevD25.FieldName = "D25";
            colDevD26.FieldName = "D26";
            colDevD27.FieldName = "D27";
            colDevD28.FieldName = "D28";
            colDevD29.FieldName = "D29";
            colDevD30.FieldName = "D30";
            colDevD31.FieldName = "D31";
            colDevD32.FieldName = "D32";
            colDevD33.FieldName = "D33";
            colDevD34.FieldName = "D34";
            colDevD35.FieldName = "D35";
            colDevD36.FieldName = "D36";
            colDevD37.FieldName = "D37";
            colDevD38.FieldName = "D38";
            colDevD39.FieldName = "D39";
            colDevD40.FieldName = "D40";
            colDevD41.FieldName = "D41";
            colDevD42.FieldName = "D42";
            colDevD43.FieldName = "D43";
            colDevD44.FieldName = "D44";
            colDevD45.FieldName = "D45";
            colDevD46.FieldName = "D46";
            colDevD47.FieldName = "D47";
            colDevD48.FieldName = "D48";
            colDevD49.FieldName = "D49";
            colDevD50.FieldName = "D50";
            colDevD51.FieldName = "D51";
            colDevD52.FieldName = "D52";
            colDevD53.FieldName = "D53";
            colDevD54.FieldName = "D54";
            colDevD55.FieldName = "D55";
            colDevD56.FieldName = "D56";
            colDevD57.FieldName = "D57";
            colDevD58.FieldName = "D58";
            colDevD59.FieldName = "D59";
            colDevD60.FieldName = "D60";
            colDevD61.FieldName = "D61";
            colDevD62.FieldName = "D62";
            colDevD63.FieldName = "D63";
            colDevD64.FieldName = "D64";
            colDevD65.FieldName = "D65";

            gcDevBooking.DataSource = dsDev.Tables[0];

            if (dtStp.Rows.Count > 0)
            {
                DateTime startdate = Convert.ToDateTime(dtStp.Rows[0]["BeginDate"].ToString());
                int columnIndex = 5;

                //Headers Date
                for (int i = 0; i < daysDiff + 1; i++)
                {
                    gvDevBooking.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM ddd");
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
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
