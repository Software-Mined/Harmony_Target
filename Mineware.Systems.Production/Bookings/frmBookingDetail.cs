using System;
using System.Collections.Generic;
using System.Linq;

namespace Mineware.Systems.Production.Bookings
{
    public partial class frmBookingDetail : DevExpress.XtraEditors.XtraForm
    {
        Procedures procs = new Procedures();
        public string _connectionString;
        public string Selectedprodmonth;
        public string WorkplaceName;
        public string WpID;
        public string SectionID;
        public bool IsNightShiftBooking = false;
        public int Activity;
        private DateTime _theDate;
        public DateTime TheDate
        {
            get { return _theDate; }
            set { _theDate = value; }
        }
        public string DayOfWeekForRecon = ProductionGlobal.ProductionGlobalTSysSettings._CheckMeas;
        public int WPAct;       
        private decimal PegFrom;
        private decimal PlanHeight;
        private decimal planwidth;
        private decimal planCMGT;
        private decimal planDens;
        private decimal planSW;
        private decimal planAdv;
        private decimal DefAdv;        
        public string MinerID;
        public string MOID;
        private Dictionary<string, string> _problems = new Dictionary<string, string>();
        public frmBookingDetail()
        {
            InitializeComponent();
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmBookingDetail_Load(object sender, EventArgs e)
        {
            dtTheDate.EditValue = TheDate;
            tbSection2.EditValue = SectionID;
            barEditWorkplace.EditValue = WorkplaceName;

            LoadReconDay();
            LoadBookingData();
            LoadLostBlastData();
        }

        private void LoadBookingData()
        {
            //string thedate = dtTheDate.EditValue == null ? DateTime.Now.ToString("dd-MMM-yyyy") : ((DateTime)dtTheDate.EditValue).ToString("dd-MMM-yyyy");
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _connectionString;

            string nightshift = IsNightShiftBooking == true ? "1" : "0";

            if (WPAct != 1)
            {
                _dbAction.SqlStatement = @"EXEC	[dbo].[sp_Load_BookingStoping]  '" + TheDate.ToShortDateString() + "','" + WpID + "','" + nightshift + "' , '" + WPAct + "' ";

                _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbAction.ExecuteInstruction();

                //MWDataManager.clsDataAccess _dbPlanRisk = new MWDataManager.clsDataAccess();
                //_dbPlanRisk.ConnectionString = _connectionString;
                //_dbPlanRisk.SqlStatement = "select* from vw_Preplanning_Rating_Workplace where workplace = '" + WpID + "'";

                //_dbPlanRisk.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbPlanRisk.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbPlanRisk.ExecuteInstruction();
                //int rr = 0;
                //if (_dbPlanRisk.ResultsDataTable.Rows.Count > 0)
                //{
                //    lblRating.Text = _dbPlanRisk.ResultsDataTable.Rows[0]["Answer"].ToString();

                //    rr = Convert.ToInt32(_dbPlanRisk.ResultsDataTable.Rows[0]["Answer"].ToString());
                //}


                //if (rr > 0)
                //{
                //    pbPlanRR.Image = imageCollection2.Images[3];
                //}

                //if (rr > 9)
                //{
                //    pbPlanRR.Image = imageCollection2.Images[4];
                //}

                //if (rr > 16)
                //{
                //    pbPlanRR.Image = imageCollection2.Images[5];
                //}

                if (_dbAction.ResultsDataTable != null || _dbAction.ResultsDataTable?.Rows.Count > 0)
                {
                    if (_dbAction.ResultsDataTable.Rows[0]["PAdv"].ToString() == string.Empty)
                    {
                        return;
                    }

                    planAdv = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["PAdv"].ToString());
                    planSW = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["SW"].ToString());
                    planCMGT = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["CMGT"].ToString());
                    planDens = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["Dens"].ToString());
                    DefAdv = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["DevAdv"].ToString());

                    //If stoping
                    if (WPAct != 1)
                    {
                        txtPlannedCode.Text = _dbAction.ResultsDataTable.Rows[0][0].ToString();
                        txtPlannedFL.Text = _dbAction.ResultsDataTable.Rows[0][1].ToString();
                        txtBookedReefFL.Text = _dbAction.ResultsDataTable.Rows[0][2].ToString();
                        txtBookedWasteFL.Text = _dbAction.ResultsDataTable.Rows[0][3].ToString();
                        txtReconProgBook.Text = _dbAction.ResultsDataTable.Rows[0][4].ToString();
                        txtReconForecast.Text = _dbAction.ResultsDataTable.Rows[0][5].ToString();
                        txtReconRecon.Text = _dbAction.ResultsDataTable.Rows[0][7].ToString();
                    }
                    else //It is nightshift
                    {
                        txtCleaned.Text = _dbAction.ResultsDataTable.Rows[0][7].ToString();
                        txtPlanned.Text = _dbAction.ResultsDataTable.Rows[0][3].ToString();
                        txtBlasted.Text = _dbAction.ResultsDataTable.Rows[0][1].ToString();
                    }

                    //Add problem
                    string prob = _dbAction.ResultsDataTable.Rows[0]["BookProb"].ToString();

                    _problems.Clear();

                    if (!string.IsNullOrWhiteSpace(prob))
                    {
                        string probDesc = _dbAction.ResultsDataTable.Rows[0]["ProblemDesc"].ToString();
                        string SBNotes = _dbAction.ResultsDataTable.Rows[0]["SBossNotes"].ToString();
                        _problems.Add(prob, "PR" + ";" + SBNotes);

                        lblProbNote.Text = probDesc;
                        lblProbNote.Visible = true;
                    }
                    else
                    {
                        lblProbNote.Visible = false;
                    }
                }
                else
                {
                    txtPlannedCode.Text = string.Empty;
                    txtPlannedFL.Text = "0";
                    txtBookedReefFL.Text = "0";
                    txtBookedWasteFL.Text = "0";
                    txtReconProgBook.Text = string.Empty;
                    txtReconForecast.Text = "0";
                    txtReconRecon.Text = "0";
                    txtDevCode.Text = string.Empty;
                    txtDevPegToFace.Text = "0";
                    txtPlanned.Text = "0";
                    txtCleaned.Text = "0";
                    txtBlasted.Text = "0";

                    //remove problem
                    _problems.Clear();
                }
            }


            if (WPAct == 1)
            {
                _dbAction.SqlStatement = @"exec sp_Booking_Develpoment_Detial '" + TheDate.ToShortDateString() + "','" + MinerID + "', '" + TheDate.ToShortDateString() + "'  ";

                _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbAction.ExecuteInstruction();

                if (_dbAction.ResultsDataTable != null && _dbAction.ResultsDataTable?.Rows.Count > 0)
                {
                    txtDevCode.Text = _dbAction.ResultsDataTable.Rows[0]["MOCycle"].ToString();

                    PegFrom = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["pegFrom"].ToString());
                    PlanHeight = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["DHeight"].ToString());
                    planwidth = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["DWidth"].ToString());
                    planCMGT = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["CMGT"].ToString());
                    planDens = Convert.ToDecimal(_dbAction.ResultsDataTable.Rows[0]["Dens"].ToString());

                    txtDevPegToFace.Text = _dbAction.ResultsDataTable.Rows[0]["PegToFace"].ToString();
                    txtDevAdv.Text = _dbAction.ResultsDataTable.Rows[0]["BookAdv"].ToString();
                    txtDevPlanAdv.Text = _dbAction.ResultsDataTable.Rows[0]["PlanAdv"].ToString();

                    //if no booking
                    if (_dbAction.ResultsDataTable.Rows[0]["PegToFace"] == DBNull.Value)
                    {
                        txtDevPegToFace.Text = _dbAction.ResultsDataTable.Rows[0]["PrevPegToFace"].ToString();
                    }

                    //Add problem
                    string prob = _dbAction.ResultsDataTable.Rows[0]["BookProb"].ToString();

                    _problems.Clear();

                    if (!string.IsNullOrWhiteSpace(prob))
                    {
                        string probDesc = _dbAction.ResultsDataTable.Rows[0]["ProblemDesc"].ToString();
                        string SBNotes = _dbAction.ResultsDataTable.Rows[0]["SBossNotes"].ToString();
                        _problems.Add(prob, "PR" + ";" + SBNotes);

                        lblProbNote.Text = probDesc;
                        lblProbNote.Visible = true;
                    }
                    else
                    {
                        lblProbNote.Visible = false;
                    }
                }
                else
                {
                    txtDevCode.Text = string.Empty;
                    txtDevPegToFace.Text = "0";
                    txtDevAdv.Text = "0";
                    txtDevPlanAdv.Text = "0";

                    //remove problem
                    _problems.Clear();
                }
            }

        }

        private void LoadReconDay()
        {
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _connectionString;
            _dbAction.SqlStatement = @"SELECT [CheckMeas] FROM [dbo].[tbl_SysSet]";
            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            DayOfWeekForRecon = _dbAction.ResultsDataTable.Rows[0][0].ToString();
        }

        private void LoadLostBlastData()
        {
            MWDataManager.clsDataAccess _dbLostBlast = new MWDataManager.clsDataAccess();
            _dbLostBlast.ConnectionString = _connectionString;
            _dbLostBlast.SqlStatement = "select Main.Calendardate,mocycle, booksqm, isnull(Problem.ProblemID + ' ' + Problem.Description,'          ') Problem, isnull(ProblemBook.CausedLostBlast,'          ') CausedLostBlast,isnull(SBossNotes,'')SBossNotes from (  \r\n" +
                                        "select Prodmonth,WorkplaceID,SectionID,Calendardate,BookCode,BookProb,MOCycle, booksqm from tbl_planning \r\n" +
                                        "where prodmonth = '" + Selectedprodmonth + "' and workingday = 'Y' and Calendardate >= Getdate()-7 and calendardate <= Getdate() and WorkplaceID = '" + WpID + "' \r\n" +
                                        ") Main \r\n" +
                                        "left outer join \r\n" +
                                        "tbl_Problem Problem \r\n" +
                                        "on Main.BookProb = Problem.ProblemID \r\n" +
                                        "left outer join \r\n" +
                                        "tbl_ProblemBook ProblemBook \r\n" +
                                        "on main.WorkplaceID = problembook.WorkplaceID and main.Prodmonth = ProblemBook.Prodmonth and main.CalendarDate = ProblemBook.CalendarDate \r\n" +
                                        "order by Calendardate Asc";

            _dbLostBlast.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbLostBlast.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbLostBlast.ExecuteInstruction();

            gcLostBlast.DataSource = _dbLostBlast.ResultsDataTable;

            colDate.FieldName = "Calendardate";
            colProblem.FieldName = "Problem";
            colLostBlast.FieldName = "CausedLostBlast";
            colMoCycle.FieldName = "mocycle";
            colSqm.FieldName = "booksqm";
            colShiftbossNotes.FieldName = "SBossNotes";

            CalcGrid();
        }


        public void CalcGrid()
        {
            int PlannedBlast = 0;
            int ActualBlast = 0;
            int LostBlastCheck = 0;

            PlannedBlast = 0;
            for (int row = 0; row < gvLostBlast.RowCount; row++)
            {
                string PBvalue = gvLostBlast.GetRowCellValue(row, gvLostBlast.Columns[1]).ToString();
                string ABvalue = gvLostBlast.GetRowCellValue(row, gvLostBlast.Columns[2]).ToString();
                string LBvalue = gvLostBlast.GetRowCellValue(row, gvLostBlast.Columns[4]).ToString();

                if (!string.IsNullOrEmpty(PBvalue))
                {
                    if (PBvalue.TrimEnd() == "BL")
                    {
                        PlannedBlast = PlannedBlast + 1;
                    }
                }

                if (!string.IsNullOrEmpty(ABvalue))
                {
                    if (Convert.ToDecimal(ABvalue) > 0)
                    {
                        ActualBlast = ActualBlast + 1;
                    }
                }

                if (!string.IsNullOrEmpty(LBvalue))
                {
                    if (LBvalue == "Y")
                    {
                        LostBlastCheck = LostBlastCheck + 1;
                    }
                }
            }


        }
    }
}