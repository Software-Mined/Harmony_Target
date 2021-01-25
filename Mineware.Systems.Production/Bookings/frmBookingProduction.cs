using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
///using MWDataManager;

namespace Mineware.Systems.Production.Bookings
{
    public partial class frmBookingProduction : DevExpress.XtraEditors.XtraForm
    {
        #region Fields and Properties
        /// <summary>
        /// The SystemDB tag  for the connection
        /// </summary>
        public string SystemDBTag { get; set; }
        private string _userCurrentInfoConnection = string.Empty;
        /// <summary>
        /// The current connection
        /// </summary>
        public string UserCurrentInfoConnection { get { return _userCurrentInfoConnection; } set { _userCurrentInfoConnection = value; } }
        /// <summary>
        /// The workplace name
        /// </summary>
        public string WorkplaceName;
        /// <summary>
        /// The workplace ID
        /// </summary>
        public string WpID;
        private Dictionary<string, string> _problems = new Dictionary<string, string>();
        /// <summary>
        /// The id of the section
        /// </summary>
        public string SectionID;
        /// <summary>
        /// The id of the activity
        /// </summary>
        public int Activity;
        private DateTime _theDate;
        /// <summary>
        /// The date 
        /// </summary>
        public DateTime TheDate
        {
            get { return _theDate; }
            set { _theDate = value; }
        }
        /// <summary>
        /// The day of the week we can book recon
        /// </summary>
        public string DayOfWeekForRecon = ProductionGlobal.ProductionGlobalTSysSettings._CheckMeas;
        /// <summary>
        /// Can the user change the date
        /// </summary>
        public bool CanChangeDate { set { dtTheDate.Enabled = value; } }
        /// <summary>
        /// Is this the night shift booking
        /// </summary>
        public bool IsNightShiftBooking = false;

        public string Selectedprodmonth;

        public string theSystemDBTag;

        public string connection;

        private string LBlast = "N";
        private decimal PegFrom;
        private decimal PlanHeight;
        private decimal planwidth;
        private decimal planCMGT;
        private decimal planDens;
        private decimal planSW;
        private decimal planAdv;
        private decimal DefAdv;
        private string CanBookABS = "Y";
        #pragma warning disable IDE0052
        private string LostBlastRecon = "N";
        #pragma warning restore IDE0052

        public int WPAct;
        public string MinerID;
        public string MOID;
        #endregion Fields and Properties

        #region Constructor 
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="workplaceName">The name of the workplace to update</param>
        /// <param name="wpID">The workplace Id to update</param>
        public frmBookingProduction()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Events
        /// <summary>
        /// Set the panels to show correctly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBookingProduction_Load(object sender, EventArgs e)
        {
            pnlS.Visible = false;
            pnlB.Visible = false;
            pnlA.Visible = true;

            ///ReefDrilling
            string MOSection = SectionID.Substring(0,4);
            if (MOSection == "A119")
            {
                lblDrillMeters.Visible = true;
                txtDrillMetres.Visible = true;
            }
            else 
            {
                lblDrillMeters.Visible = false;
                txtDrillMetres.Visible = false;
            }

            _userCurrentInfoConnection = TConnections.GetConnectionString(theSystemDBTag, connection);
            #region Stoppage Warning
            //New Load Stoppage Warning
            MWDataManager.clsDataAccess theDataWarning = new MWDataManager.clsDataAccess();
            theDataWarning.ConnectionString = _userCurrentInfoConnection;
            theDataWarning.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theDataWarning.queryReturnType = MWDataManager.ReturnType.DataTable;
            theDataWarning.SqlStatement = "SELECT  WorkplaceID, max(StoppageDate)StoppageDate ,max(StoppageType)StoppageType,max(Comments)Comments, max(Distance) Distance \r\n" +
                " FROM tbl_Workplace_Stoppages_Detail  where workplaceid = '" + WpID + "' group by WorkplaceID";
            var result2 = theDataWarning.ExecuteInstruction();

            if (result2.success && theDataWarning.ResultsDataTable.Rows.Count > 0)
            {
                lblWarning.Visible = true;
                picEditWarning.Visible = true;
                lblWarning.Text = " Warning - Stoppage type : "+ theDataWarning.ResultsDataTable.Rows[0]["StoppageType"].ToString() + " in :" + theDataWarning.ResultsDataTable.Rows[0]["Distance"].ToString()+ " m";
            }
            else
            {
                lblWarning.Visible = false;
                picEditWarning.Visible = false;
            }
            /////////End Warning
            #endregion

            // change to one decimal if development 
            if (txtpegValue.Visible == true)
            {
                //txtReconForecast.Properties.Mask.EditMask = "d1";
                txtReconForecast.Properties.Mask.EditMask = "f1";
                txtReconForecast.Properties.Mask.UseMaskAsDisplayFormat = true;
            }

            

            dtTheDate.Properties.MaxValue = DateTime.Today.Date;

            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = _userCurrentInfoConnection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.SqlStatement = "Select BK_BackDate FROM [tbl_Users_Synchromine] where userid = '" + TUserInfo.UserID + "' ";
            var result = theData.ExecuteInstruction();

            if (result.success)
            {
                if (theData.ResultsDataTable.Rows[0][0].ToString() == "1")
                {
                    dtTheDate.Enabled = true;
                }
                else
                {
                    dtTheDate.Enabled = false;
                }
            }

            //Ad WP Name to title
            this.Text += " (" + WorkplaceName + ")";

            dtTheDate.EditValue = _theDate;

            dtTheDate_EditValueChanged(null, null);

            LoadReconDay();
            LoadIncidentsData();
            LoadABSData();
            LoadBookingData();
            //LoadReconDay();
            //LoadIncidentsData();
            //LoadABSData();
            //LoadBookingData();
            LoadLostBlastData();
        }

        /// <summary>
        /// Set the image to a tick when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPanelBAndS_Click(object sender, EventArgs e)
        {
            //Clear all selections
            ClearBAndSPanels();

            var btn = sender as SimpleButton;
            if (btn.ImageOptions.Image == null)
            {
                btn.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamA;
                btn.Tag = true;
            }
            else
            {
                btn.ImageOptions.Image = null;
                btn.Tag = false;
            }

        }

        /// <summary>
        /// Show and hide the correct panels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTeamABS_Click(object sender, EventArgs e)
        {
            if (CanBookABS == "N")
            {
                return;
            }

            //Clear all selections
            ClearBAndSPanels();

            var btn = sender as SimpleButton;
            bool isSelected = Convert.ToBoolean(btn.Tag);
            //update the buttons tag to know if it is selected or not
            btn.Tag = !isSelected;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBookingProduction));

            switch (btn.Name)
            {
                case "btnTeamA":
                    //it was selected so unselect it
                    if (isSelected)
                    {
                        btn.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        btnTeamB.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        btnTeamS.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB; ;
                        pnlA.Visible = false;
                        pnlB.Visible = false;
                        pnlS.Visible = false;
                    }
                    else
                    {
                        btn.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamA;
                        btnTeamB.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        btnTeamS.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        pnlA.Visible = true;
                        pnlB.Visible = false;
                        pnlS.Visible = false;
                        pnlAdditionalNotes.Visible = false;
                    }
                    btnTeamB.Tag = false;
                    btnTeamS.Tag = false;
                    break;
                case "btnTeamB":
                    //it was selected so unselect it
                    if (isSelected)
                    {
                        btn.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        btnTeamA.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        btnTeamS.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        pnlA.Visible = false;
                        pnlB.Visible = false;
                        pnlS.Visible = false;
                    }
                    else
                    {
                        btn.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamA;
                        btnTeamA.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        btnTeamS.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        pnlA.Visible = false;
                        pnlB.Visible = true;
                        pnlS.Visible = false;
                        pnlAdditionalNotes.Visible = true;
                    }
                    btnTeamA.Tag = false;
                    btnTeamS.Tag = false;
                    break;
                case "btnTeamS":
                    //it was selected so unselect it
                    if (isSelected)
                    {
                        btn.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        btnTeamB.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        btnTeamA.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        pnlA.Visible = false;
                        pnlB.Visible = false;
                        pnlS.Visible = false;
                    }
                    else
                    {
                        btn.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamA;
                        btnTeamB.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        btnTeamA.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                        pnlA.Visible = false;
                        pnlB.Visible = false;
                        pnlS.Visible = true;
                        pnlAdditionalNotes.Visible = true;
                    }
                    btnTeamB.Tag = false;
                    btnTeamA.Tag = false;
                    break;
            }
        }

        /// <summary>
        /// Change the on precautions state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOnPrecautions_Click(object sender, EventArgs e)
        {
            btnOnPrecautions.Tag = !Convert.ToBoolean(btnOnPrecautions.Tag);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBookingProduction));

            if (Convert.ToBoolean(btnOnPrecautions.Tag))
            {
                btnOnPrecautions.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamA;
            }
            else
            {
                btnOnPrecautions.ImageOptions.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
            }
        }

        /// <summary>
        /// Add a problem to bookings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProblems_Click(object sender, EventArgs e)
        {
            frmBookingsABSProblems ucProbs = new frmBookingsABSProblems();
            //Setup the labels
            ucProbs.TheWorkplace = WorkplaceName;
            ucProbs.TheSection = SectionID;
            ucProbs.TheDate = DateTime.Now;
            ucProbs.TheActivity = Activity;
            ucProbs.NoteID = "NoteId";
            //ucProbs.TheConnection = UserCurrentInfoConnection;
            ucProbs._theSystemDBTag = theSystemDBTag;
            ucProbs._UserCurrentInfoConnection = connection;
            ucProbs.Problems = _problems;

            if (!string.IsNullOrEmpty(lblProbNote.Text))
                ucProbs.ProblemDesc = lblProbNote.Text;

            ucProbs.ShowDialog(this);

            lblProbNote.Text = ProductionGlobal.ProductionGlobal.ExtractAfterColon(ucProbs.lbNoteID.Text);
            lblProbNote.Visible = true;

            //string prob = _problems.Values.FirstOrDefault().ToString();

            //if (!string.IsNullOrWhiteSpace(prob))
            //{
            //    lblProbNote.Text = prob.Split(';')[0];
            //    lblProbNote.Visible = true;
            //}
            //else
            //{
            //    lblProbNote.Visible = false;
            //}
        }

        /// <summary>
        /// Save the information to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        /// <summary>
        /// Close the form without saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Calculate square meters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditBooking_TextChanged(object sender, EventArgs e)
        {

            double reefFL = 0;
            double wasteFL = 0;
            double advance = 0;

            if (double.TryParse(txtBookedReefFL.Text, out reefFL) && double.TryParse(txtBookedWasteFL.Text, out wasteFL)
                && double.TryParse(txtBookedAdvance.Text, out advance))
            {
                txtBookedSqm.Text = ((reefFL + wasteFL) * advance).ToString();
            }
        }

        /// <summary>
        /// Set the recon visible depending on day of week
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtTheDate_EditValueChanged(object sender, EventArgs e)
        {
            string thedate = dtTheDate.EditValue == null ? DateTime.Now.ToString("yyyy-MM-dd") : ((DateTime)dtTheDate.EditValue).ToString("yyyy-MM-dd");

            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _userCurrentInfoConnection;
            _dbAction.SqlStatement = @"Select workingday,isnull(pumahola,'N') pumahola from tbl_planning where Sectionid = '" + MinerID + "' and calendardate = '" + thedate + "' and workplaceid = '" + WpID + "' ";

            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            
            var result = _dbAction.ExecuteInstruction();

            //try
            //{
                if (result.success == true)
                {
                    if (_dbAction.ResultsDataTable.Rows[0]["workingday"].ToString() == "N" && _dbAction.ResultsDataTable.Rows[0]["pumahola"].ToString() != "Y")
                    {
                        MessageBox.Show("Bookings not Allowed on Non-Workingdays");
                        pnlDevPlanned.Enabled = false;
                        pnlDevBooking.Enabled = false;

                        pnlStopingRecon.Enabled = false;
                        pnlStopingPlanned.Enabled = false;
                        pnlStopingBooked.Enabled = false;

                        CanBookABS = "N";

                        return;
                    }
                    else
                    {
                        pnlDevPlanned.Enabled = true;
                        pnlDevBooking.Enabled = true;

                        pnlStopingRecon.Enabled = true;
                        pnlStopingPlanned.Enabled = true;
                        pnlStopingBooked.Enabled = true;

                        CanBookABS = "Y";
                    }
                }
            //}
            //catch { }

            //Only show if is is the correct day and it is stoping
            pnlStopingRecon.Visible = dtTheDate.Text.Contains(DayOfWeekForRecon) && Activity == 0 && !IsNightShiftBooking;
            grpLostBlastRecon.Visible = dtTheDate.Text.Contains(DayOfWeekForRecon) && Activity == 0 && !IsNightShiftBooking;
            //grpLostBlastRecon.Visible = false;

            if (dtTheDate.Text.Contains(DayOfWeekForRecon) && Activity == 1)
            {
                pnlStopingRecon.Visible = true;
                txtReconProgBook.Enabled = false;
                txtReconRecon.Enabled = false;
            }

            //Show the correct panels for stoping
            pnlStopingBooked.Visible = Activity == 0 && !IsNightShiftBooking;
            pnlStopingPlanned.Visible = Activity == 0 && !IsNightShiftBooking;
            //Show Correct panel for development
            pnlDevPlanned.Visible = Activity == 1 && !IsNightShiftBooking;
            //Show correct panel for night shift bookings
            pnlNightShift.Visible = IsNightShiftBooking;

            TheDate = (DateTime)dtTheDate.EditValue;

            //Load the data
            LoadIncidentsData();
            LoadBookingData();
            LoadABSData();
            GetWorkplaces();
            LoadLostBlastData();
        }

        /// <summary>
        /// Open the action form to add responcible person feedback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvAction_DoubleClick(object sender, EventArgs e)
        {
            var tmp = sender as DevExpress.XtraGrid.Views.BandedGrid.BandedGridView;
            var row = tmp.GetFocusedRow() as System.Data.DataRowView;
            frmResponciblePersonFeedback frm = new frmResponciblePersonFeedback();
            frm.ActionDesc = "Action: " + gvAction.GetRowCellValue(gvAction.FocusedRowHandle, gvAction.Columns["ActionID"]).ToString();    //row.Row[0].ToString();
            frm.ActionId = row.Row[3].ToString();
            frm.Workplace = WorkplaceName;
            frm.ConnectionString = _userCurrentInfoConnection;
            frm.ShowDialog(this);

            //Reload the incidents
            LoadIncidentsData();
        }

        /// <summary>
        /// Workplace item click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>TODO: Get the correct date and activity for the workplace and the SectionID</remarks>
        private void Item_Click(object sender, EventArgs e)
        {
            var tmp = sender as System.Windows.Forms.ToolStripMenuItem;
            var details = tmp.Tag.ToString().Split(';');
            WorkplaceName = tmp.Text;
            WpID = details[0];
            SectionID = details[1];
            Activity = Convert.ToInt32(details[2]);
            this.Text = this.Text.Substring(0, 18) + " (" + WorkplaceName + ")";

            //Set the date(this reloads the data) 
            //DateTime dt = (DateTime)dtTheDate.EditValue;
            //dtTheDate.EditValue = DateTime.Now;
            //dtTheDate.EditValue = dt;

            LoadABSData();
            LoadReconDay();
            LoadIncidentsData();
            LoadBookingData();
            LoadLostBlastData();
        }
        #endregion Events

        #region Methods
        /// <summary>
        /// clears all the selections for both  panels
        /// </summary>
        private void ClearBAndSPanels()
        {
            foreach (var ctrl in pnlB.Controls)
            {
                if (ctrl is SimpleButton)
                {
                    var btn = ctrl as SimpleButton;
                    btn.ImageOptions.Image = null;
                    btn.Tag = false;
                }
            }

            foreach (var ctrl in pnlS.Controls)
            {
                if (ctrl is SimpleButton)
                {
                    var btn = ctrl as SimpleButton;
                    btn.ImageOptions.Image = null;
                    btn.Tag = false;
                }
            }
        }

        /// <summary>
        /// Loads the Incidents from the DB
        /// </summary>
        private void LoadIncidentsData()
        {
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _userCurrentInfoConnection;
            _dbAction.SqlStatement = @"SELECT a.[ActionDate]		 
		                                    ,a.[Action_Title]
		                                    ,a.[Responsible_Person]
		                                    ,a.[ActionID]
		                                    ,a.[Hazard]
		                                    ,a.[DaysOpen]
		                                    ,a.[App_Origin]
		                                    ,a.[Enablon_Action_ID]
                                            ,a.[Responsible_Person_Feedback]
		                                    ,CASE WHEN a.[App_Origin] = 'Pivot Action' THEN 'Requested'
                                            WHEN a.[App_Origin] = 'Normal Action'  THEN 'NA' ELSE 'Open' END AS [RequestStatus]
                                     FROM
                                     (SELECT*, DATEDIFF(DAY, [Start_Date], GETDATE()) AS[DaysOpen], ISNULL([Mineware_Action_ID], '')  AS [ActionID]
                                     ,CASE WHEN[Application_Origin] LIKE 'Pivot%' THEN 'Pivot Action' WHEN[Application_Origin] LIKE 'WED%' THEN 'WED Action'  ELSE 'Normal Action' END AS[App_Origin] FROM[dbo].[tbl_Incidents]
                                            WHERE[workplace] = '" + WorkplaceName + @"' AND([Action_Status] = 'Open' OR[Action_Status] = 'New Action')) a";


            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            gcAction.DataSource = _dbAction.ResultsDataTable;

            colActionDate.FieldName = "ActionDate";
            colActionTitle.FieldName = "Action_Title";
            colResPerson.FieldName = "Responsible_Person";
            colActionID.FieldName = "ActionID";
            colHazard.FieldName = "Hazard";
            colDaysOpen.FieldName = "DaysOpen";
            colActionType.FieldName = "App_Origin";
            colPivotID.FieldName = "Enablon_Action_ID";
            colRequestedForClosed.FieldName = "RequestStatus";
            colFeedBack.FieldName = "Responsible_Person_Feedback";
        }

        private void LoadLostBlastData()
        {
            MWDataManager.clsDataAccess _dbLostBlast = new MWDataManager.clsDataAccess();
            _dbLostBlast.ConnectionString = _userCurrentInfoConnection;
            _dbLostBlast.SqlStatement = "select Main.Calendardate,mocycle, booksqm, isnull(Problem.ProblemID + ' ' + Problem.Description,'          ') Problem, isnull(ProblemBook.CausedLostBlast,'          ') CausedLostBlast,isnull(SBossNotes,'')SBossNotes from (  \r\n" +
                                        "select Prodmonth,WorkplaceID,SectionID,Calendardate,BookCode,BookProb,MOCycle, booksqm from tbl_planning \r\n" +
                                        "where prodmonth = '" + Selectedprodmonth + "' and workingday = 'Y' and Calendardate >= DATEADD(wk, DATEDIFF(wk, 6, '" + ((DateTime)dtTheDate.EditValue).ToString("dd-MMM-yyyy") + "'), 0) and calendardate <= DATEADD(wk, DATEDIFF(wk,0,'" + ((DateTime)dtTheDate.EditValue).ToString("dd-MMM-yyyy") + "'), -1) and WorkplaceID = '" + WpID + "' \r\n" +
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

            lblPlannedBlast.Text = PlannedBlast.ToString();
            lblBookedBlast.Text = ActualBlast.ToString();

            int LostBlast = 0;
            LostBlast = PlannedBlast - ActualBlast;
            lblLostBlast.Text = Convert.ToString(PlannedBlast - ActualBlast - LostBlastCheck);

            if (LostBlastCheck == LostBlast || LostBlast < 0)
            {
                pbLostBlast.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamA;
                LostBlastRecon = "Y";
            }
            else
            {
                pbLostBlast.Image = global::Mineware.Systems.Production.Properties.Resources.btnTeamB;
                LostBlastRecon = "N";
            }
        }


        /// <summary>
        /// Load the booking data
        /// </summary>
        /// <remarks>TODO: Still need to get the Advance for stoping</remarks>
        private void LoadBookingData()
        {
            string thedate = dtTheDate.EditValue == null ? DateTime.Now.ToString("dd-MMM-yyyy") : ((DateTime)dtTheDate.EditValue).ToString("dd-MMM-yyyy");
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _userCurrentInfoConnection;

            string nightshift = IsNightShiftBooking == true ? "1" : "0";

            if (WPAct != 1)
            {
                _dbAction.SqlStatement = @"EXEC	[dbo].[sp_Load_BookingStoping]  '" + thedate + "','" + WpID + "','" + nightshift + "' , '" + WPAct + "' ";

                _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbAction.ExecuteInstruction();

                MWDataManager.clsDataAccess _dbPlanRisk = new MWDataManager.clsDataAccess();
                _dbPlanRisk.ConnectionString = _userCurrentInfoConnection;
                _dbPlanRisk.SqlStatement = "select* from vw_Preplanning_Rating_Workplace where workplace = '" + WpID + "'";

                _dbPlanRisk.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbPlanRisk.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbPlanRisk.ExecuteInstruction();
                int rr = 0;
                if (_dbPlanRisk.ResultsDataTable.Rows.Count > 0)
                {
                    lblRating.Text = _dbPlanRisk.ResultsDataTable.Rows[0]["Answer"].ToString();

                    rr = Convert.ToInt32(_dbPlanRisk.ResultsDataTable.Rows[0]["Answer"].ToString());
                }


                if (rr > 0)
                {
                    pbPlanRRSVG.SvgImage = svgImageCollection1[2];
                    pbPlanRRSVG.Visible = true;
                }

                if (rr > 9)
                {
                    pbPlanRRSVG.SvgImage = svgImageCollection1[3];
                    pbPlanRRSVG.Visible = true;
                }

                if (rr > 16)
                {
                    pbPlanRRSVG.SvgImage = svgImageCollection1[4];
                    pbPlanRRSVG.Visible = true;
                }

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
                    if (Activity != 1)
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
                _dbAction.SqlStatement = @"exec sp_Booking_Develpoment_Detial '" + WpID + "','" + MinerID + "', '" + thedate + "'  ";

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
                    txtDrillMetres.Text = _dbAction.ResultsDataTable.Rows[0]["BookCubics"].ToString();

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

        /// <summary>
        /// Loads the ABS Data from DB
        /// </summary>
        private void LoadABSData()
        {
            string tableName = IsNightShiftBooking == true ? "dbo.tbl_Planning_NS" : "dbo.tbl_Planning";
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _userCurrentInfoConnection;
            _dbAction.SqlStatement = @"SELECT [ABSCode], [Range],isnull(ABSNotes,'') ABSNotes FROM " + tableName +
                                        " WHERE SectionID = '" + MinerID + @"' AND Activity = '" + WPAct + "' AND WorkplaceID = '" + WpID + @"' AND CalendarDate = '" + TheDate.ToString("yyyy-MM-dd") + @"' ";

            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            SimpleButton btn = null;
            SimpleButton btn2 = null;

            if (_dbAction.ResultsDataTable != null && _dbAction.ResultsDataTable?.Rows.Count != 0)
            {
                txtAdditionalNotes.Text = _dbAction.ResultsDataTable.Rows[0]["ABSNotes"].ToString();

                switch (_dbAction.ResultsDataTable.Rows[0][0].ToString())
                {
                    case "A":
                        btn = btnTeamA;
                        break;
                    case "B":
                    case "PB":
                        btn = btnTeamB;
                        switch (_dbAction.ResultsDataTable.Rows[0][1].ToString())
                        {
                            case "1":
                                btn2 = btnB1;
                                break;
                            case "2":
                                btn2 = btnB2;
                                break;
                            case "3":
                                btn2 = btnB3;
                                break;
                            case "4":
                                btn2 = btnB4;
                                break;
                            case "5":
                                btn2 = btnB5;
                                break;
                            case "6":
                                btn2 = btnB6;
                                break;
                            case "7":
                                btn2 = btnB7;
                                break;
                        }
                        break;
                    case "S":
                    case "PS":
                        btn = btnTeamS;
                        switch (_dbAction.ResultsDataTable.Rows[0][1].ToString())
                        {
                            case "1":
                                btn2 = btnS1;
                                break;
                            case "2":
                                btn2 = btnS2;
                                break;
                            case "3":
                                btn2 = btnS3;
                                break;
                            case "4":
                                btn2 = btnS4;
                                break;
                            case "5":
                                btn2 = btnS5;
                                break;
                            case "6":
                                btn2 = btnS6;
                                break;
                            case "7":
                                btn2 = btnS7;
                                break;
                            case "8":
                                btn2 = btnS8;
                                break;
                            case "9":
                                btn2 = btnS9;
                                break;
                            case "10":
                                btn2 = btnS10;
                                break;
                            case "11":
                                btn2 = btnS11;
                                break;
                        }
                        break;
                    default:
                        btn = btnTeamA;
                        break;
                }

                //Turn on precaution
                if (_dbAction.ResultsDataTable.Rows[0][0].ToString().Contains("P"))
                {
                    btnOnPrecautions_Click(null, null);
                }

                btnTeamABS_Click(btn, null);
                if (btn2 != null)
                {
                    btnPanelBAndS_Click(btn2, null);
                }
            }
            else
            {
                //Set A to be selected
                if (!Convert.ToBoolean(btnTeamA.Tag))
                {
                    btn = btnTeamA;
                    btnTeamABS_Click(btn, null);
                }

                //Clear all selections
                ClearBAndSPanels();

                //Turn off precaution
                if (Convert.ToBoolean(btnOnPrecautions.Tag))
                {
                    btnOnPrecautions_Click(null, null);
                }
            }
        }

        /// <summary>
        /// Loads the recon day
        /// </summary>
        private void LoadReconDay()
        {
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _userCurrentInfoConnection;
            _dbAction.SqlStatement = @"SELECT [CheckMeas] FROM [dbo].[tbl_SysSet]";
            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            DayOfWeekForRecon = _dbAction.ResultsDataTable.Rows[0][0].ToString();
        }

        /// <summary>
        /// Save the data to the DB
        /// </summary>
        private void SaveData()
        {
            //Lost BlastRecon Check on Save
            MWDataManager.clsDataAccess _dbProbRec = new MWDataManager.clsDataAccess();
            _dbProbRec.ConnectionString = _userCurrentInfoConnection;
            _dbProbRec.SqlStatement = @"SELECT [AdjBook] FROM [dbo].[tbl_SysSet]";
            _dbProbRec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbProbRec.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbProbRec.ExecuteInstruction();

            string ProblemReconForced = _dbProbRec.ResultsDataTable.Rows[0][0].ToString();

            if (ProblemReconForced == "Y")
            {
                if (pnlStopingRecon.Visible == true)
                {
                    if (grpLostBlastRecon.Visible == true)
                    {
                        if (LostBlastRecon == "N")
                        {
                            MessageBox.Show("Please finnish Lost Blast Recon.");
                            return;
                        }
                    }
                }
            }

            string tablename = IsNightShiftBooking == true ? "[dbo].[tbl_Planning_NS]" : "[dbo].[tbl_Planning]";

            string probSaved = "N";

            //Save problem =========================================
            if (_problems.Count > 0)
            {
                var val = _problems.Values.FirstOrDefault().Split(';');

                if (val[0] == "PS")
                {
                    SavePlanStoppage(MinerID, WpID, Convert.ToDateTime(TheDate.ToString("yyyy-MM-dd")), _problems.Keys.FirstOrDefault(), tablename);
                    probSaved = "Y";
                }
                else if (val[0] == "PR")
                {
                    SaveProblem(MinerID, WpID, Convert.ToDateTime(TheDate.ToString("yyyy-MM-dd")), _problems.Keys.FirstOrDefault(), val[1], tablename);
                    probSaved = "Y";
                }
            }

            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = _userCurrentInfoConnection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;

            //Save bookings =========================================
            if (WPAct == 1)
            {
                decimal NewPegtoface = Convert.ToDecimal(txtDevPegToFace.Text);

                decimal NewAdv = NewPegtoface - PegFrom;

                decimal BookTons = NewAdv * planwidth * PlanHeight * planDens;
                decimal BookContent = NewAdv * planwidth * planCMGT * planDens / 100;

                if (Convert.ToDecimal(txtDevAdv.Text) < 0)
                {
                    MessageBox.Show("Negative Adv Booked not Allowed "
                                     , "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //if (NewAdv == 0 && probSaved == "N")
                //{
                //    btnAddProblems_Click(null,null);
                //    return;
                //}

                if (NewAdv == 0 && Convert.ToDecimal(txtDrillMetres.EditValue) == 0 && probSaved == "N")
                {
                    MessageBox.Show("Please add a Problem or an Adv greater than " + ProductionGlobal.ProductionGlobalTSysSettings._BlastQual + "% of the Plan Adv "
                                      , "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (NewAdv > Convert.ToDecimal(40))
                {
                    DialogResult dialogResult = MessageBox.Show("More than 40m has been booked , Do you want to continue saving?", "Invalid Data", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }

                if (pnlStopingRecon.Visible == true)
                {
                    if (txtReconForecast.Text == "0.0")
                    {
                        MessageBox.Show("Please add a MO Forecast before Saving", "Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    txtReconForecast.Text = "0";
                }

                theData.SqlStatement = "UPDATE " + tablename + " SET  [BookAdv] = '" + NewAdv + "', PegToFace = '" + NewPegtoface + "',PegDist = '" + NewPegtoface + "'  \r\n" +
                                       ",mofc = '" + txtReconForecast.Text + "'  ,bookHeight = '" + PlanHeight + "',bookWidth = '" + planwidth + "',pegid = '" + txtpegValue.Text + "' \r\n" +
                                       ",BookTons = '" + BookTons + "' ,bookgrams = '" + BookContent + "' , bookcmgt = '" + planCMGT + "' \r\n" +
                                       ",adjtons = 0, adjcont = 0,CheckMeasprob = 'NNNNNNN', BookCubics =  '" + txtDrillMetres.Text + "' \r\n";
                if (probSaved == "N")
                {
                    theData.SqlStatement = theData.SqlStatement + ", bookcode = 'BL'";
                }

                theData.SqlStatement = theData.SqlStatement + "WHERE SectionID = '" + MinerID + @"' AND WorkplaceID = '" + WpID + @"' AND CalendarDate = '" + TheDate.ToString("yyyy-MM-dd") + @"' AND activity = '" + WPAct + "'";
            }
            else
            {
                if (Convert.ToDecimal(txtBookedSqm.Text) < 0)
                {
                    MessageBox.Show("Negative Sqm Booked not Allowed "
                                     , "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }               

                string peg2Face = string.IsNullOrWhiteSpace(txtDevPegToFace.Text) == true ? "0" : txtDevPegToFace.Text;

                int adjsqm = 0;
                decimal AdjTons = 0;
                decimal AdjContent = 0;

                if (dtTheDate.Text.Contains(DayOfWeekForRecon))
                {
                    adjsqm = Convert.ToInt32(txtReconRecon.Text) - Convert.ToInt32(txtReconProgBook.Text);
                    AdjTons = adjsqm * planSW / 100 * planDens;
                    AdjContent = adjsqm * planCMGT * planDens / 100;
                }

                decimal NewSqm = Convert.ToDecimal(txtBookedSqm.Text);

                if (NewSqm == 0 && probSaved == "N")
                {
                    MessageBox.Show("Please add a Problem or a FL greater than " + ProductionGlobal.ProductionGlobalTSysSettings._BlastQual + "% of the Plan FL "
                                      , "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                    //Callie
                    //btnAddProblems_Click(null, null);
                }

                decimal BookTons = NewSqm * planSW / 100 * planDens;
                decimal BookContent = NewSqm * planCMGT * planDens / 100;
                decimal BookFL = Convert.ToDecimal(txtBookedWasteFL.Text) + Convert.ToDecimal(txtBookedReefFL.Text);

                if ((Convert.ToDecimal(NewSqm) / (Convert.ToDecimal(txtPlannedFL.Text) * Convert.ToDecimal(DefAdv))) < (Convert.ToDecimal(ProductionGlobal.ProductionGlobalTSysSettings._BlastQual) / 100))
                {
                    if (NewSqm > 0 && probSaved == "N")
                    {
                        MessageBox.Show("Please add a Problem or a FL greater than " + ProductionGlobal.ProductionGlobalTSysSettings._BlastQual + "% of the Plan FL "
                                      , "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (pnlStopingRecon.Visible == true)
                {
                    if (txtReconRecon.EditValue == null)
                    {
                        MessageBox.Show("Please add Check measurement before Saving", "Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (txtReconForecast.Text == "0")
                    {
                        MessageBox.Show("Please add a MO Forecast before Saving", "Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    txtReconForecast.Text = "0";
                }

                theData.SqlStatement = "UPDATE " + tablename + " SET [CheckSqm] = " + txtReconRecon.Text + ", [MoFc] = " + txtReconForecast.Text + ",[BookSqm] = " + txtBookedSqm.Text + ", \r\n" +
                                       "BookSW = '" + planSW + "', bookcmgt = '" + planCMGT + "',BookTons = '" + BookTons + "' ,bookgrams = '" + BookContent + "'   \r\n" +
                                       ",BookFL = '" + BookFL + "' ,bookadv = '" + txtBookedAdvance.Text + "'  \r\n" +
                                       ",AdjCont = '" + AdjContent + "' , adjtons = '" + AdjTons + "', AdjSqm = '" + adjsqm + "'  \r\n" +
                                       ",BookCubics = 0,bookSweeps = 0,BookReSweeps = 0,BookVamps = 0, \r\n" +
                                       "[OffReefFL] = " + txtBookedWasteFL.Text + ", [OnReefFL] = " + txtBookedReefFL.Text + " ";
                if (probSaved == "N")
                {
                    theData.SqlStatement = theData.SqlStatement + ", bookcode = 'BL'";
                }
                theData.SqlStatement = theData.SqlStatement + "WHERE  WorkplaceID = '" + WpID + @"' AND SectionID = '" + MinerID + @"' AND CalendarDate = '" + TheDate.ToString("yyyy-MM-dd") + @"' AND activity = '" + WPAct + "' ";
            }

            theData.ExecuteInstruction();

            //Save ABS ==============================================
            string absCode = string.Empty;
            string absRange = string.Empty;

            //Turn on precaution
            if (Convert.ToBoolean(btnOnPrecautions.Tag))
            {
                absCode = "P";
            }

            if (Convert.ToBoolean(btnTeamB.Tag))
            {
                absCode = absCode + "B";

                foreach (var ctrl in pnlB.Controls)
                {
                    if (ctrl is SimpleButton)
                    {
                        var btn = ctrl as SimpleButton;
                        if (Convert.ToBoolean(btn.Tag))
                        {
                            absRange = btn.ToolTip;
                            break;
                        }
                    }
                }
            }
            else if (Convert.ToBoolean(btnTeamS.Tag))
            {
                absCode = absCode + "S";

                foreach (var ctrl in pnlS.Controls)
                {
                    if (ctrl is SimpleButton)
                    {
                        var btn = ctrl as SimpleButton;
                        if (Convert.ToBoolean(btn.Tag))
                        {
                            absRange = btn.ToolTip;
                            break;
                        }
                    }
                }
            }
            else
            {
                absCode = "A";
            }

            if (absCode == "B"|| absCode == "S" || absCode == "PB")
            {
                if (absRange == string.Empty )
                {
                    MessageBox.Show("Please add an ABS Classification.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (absRange == "")
                {
                    MessageBox.Show("Please add an ABS Classification.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtAdditionalNotes.Text == "")
                {
                    MessageBox.Show("Please add an ABS note.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }

            theData.SqlStatement = @"UPDATE " + tablename + " SET [ABSCode] = '" + absCode + @"', [Range] = '" + absRange + @"' , absnotes = '" + txtAdditionalNotes.Text + "' WHERE  WorkplaceID = '" + WpID + @"' AND SectionID = '" + MinerID + @"' AND activity = '" + WPAct + "' AND CalendarDate = '" + TheDate.ToString("yyyy-MM-dd") + @"'";
            theData.ExecuteInstruction();

            this.Close();
        }

        /// <summary>
        /// Get the workplaces for the dropdown menu
        /// </summary>
        /// <remarks>TODO: Still need to add the sectionID in query</remarks>
        private void GetWorkplaces()
        {
            string tablename = IsNightShiftBooking == true ? "[dbo].[tbl_Planning_NS]" : "[dbo].[tbl_Planning]";

            //Get workplaces
            string date = ((DateTime)dtTheDate.EditValue).ToString("dd-MMM-yyyy");
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _userCurrentInfoConnection;
            _dbAction.SqlStatement = @"SELECT DISTINCT wp.[WorkplaceID], wp.[Description], plannin.SectionID, plannin.Activity        
                                          FROM " + tablename + @" AS plannin
                                          , [dbo].[tbl_workplace] AS wp where plannin.WorkplaceID = wp.WorkplaceID
                                        and plannin.Activity = wp.Activity
                                        AND CalendarDate = '" + date + "' AND SectionID Like '" + MOID + "%'";
            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            //Add workplaces to menu
            tsmiWP.DropDownItems.Clear();
            for (int i = 0; i < _dbAction.ResultsDataTable.Rows.Count; i++)
            {
                var item = new System.Windows.Forms.ToolStripMenuItem(_dbAction.ResultsDataTable.Rows[i][1].ToString());
                item.Tag = _dbAction.ResultsDataTable.Rows[i][0].ToString() + ";" + _dbAction.ResultsDataTable.Rows[i][2].ToString() + ";" + _dbAction.ResultsDataTable.Rows[i][3].ToString();
                item.Click += Item_Click;
                item.Font = new System.Drawing.Font("Tahoma", 7.8f);
                tsmiWP.DropDownItems.Add(item);
            }

        }

        /// <summary>
        /// Save the problem to the database
        /// </summary>
        /// <param name="_section"></param>
        /// <param name="_workplace"></param>
        /// <param name="_date"></param>
        /// <param name="_problemID"></param>
        /// <param name="_sbossNotes"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool SaveProblem(string _section, string _workplace, DateTime _date, string _problemID, string _sbossNotes, string tableName)
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = _userCurrentInfoConnection;

            theData.SqlStatement = " update " + tableName + " \r\n" +
            "set BookCode = 'PR', \r\n" +
            " BookProb = '" + _problemID + "' \r\n" +
            //" SbossNotes = '" + _sbossNotes + "'  \r\n" +
            " where workplaceid = '" + _workplace + "' \r\n" +
            " and Calendardate = '" + _date + "' \r\n" +
            " and SectionID = '" + MinerID + "' \r\n" +
            " and activity = '" + WPAct + "' \r\n\r\n"; // and plancode = 'MP' ";

            if (txtBookedAdvance.Text == "0.00")
            {
                LBlast = "Y";
            }

            theData.SqlStatement = theData.SqlStatement + " delete from tbl_ProblemBook \r\n";
            theData.SqlStatement = theData.SqlStatement + "where workplaceid =  '" + _workplace + "' \r\n";
            theData.SqlStatement = theData.SqlStatement + "and sectionid = '" + _section + "' and activity = '" + WPAct + "' and calendardate = '" + String.Format("{0:yyyy-MM-dd}", _date) + "' \r\n\r\n";

            theData.SqlStatement = theData.SqlStatement + " insert into tbl_ProblemBook \r\n";
            theData.SqlStatement = theData.SqlStatement + "values ( '" + _workplace + "',  '" + Selectedprodmonth + "', \r\n";
            theData.SqlStatement = theData.SqlStatement + " '" + _section + "', '" + WPAct + "' , '" + String.Format("{0:yyyy-MM-dd}", _date) + "', \r\n";
            theData.SqlStatement = theData.SqlStatement + " '" + _problemID + "', 'D', 'N', '" + _sbossNotes + "', \r\n";
            theData.SqlStatement = theData.SqlStatement + " null, null, '" + String.Format("{0:yyyy-MM-dd}", _date) + "', '" + LBlast + "', \r\n";
            theData.SqlStatement = theData.SqlStatement + " null, '', '" + String.Format("{0:yyyy-MM-dd}", _date) + "', '', null, null )  \r\n\r\n";

            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            var errorMsg = theData.ExecuteInstruction();

            return true;
        }

        /// <summary>
        /// save the planned stoppage to the database
        /// </summary>
        /// <param name="_section"></param>
        /// <param name="_workplace"></param>
        /// <param name="_date"></param>
        /// <param name="_problemID"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool SavePlanStoppage(string _section, string _workplace, DateTime _date, string _problemID, string tableName)
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = _userCurrentInfoConnection;

            theData.SqlStatement = " UPDATE " + tableName + " SET BookCode = 'PS' ,BookProb = '" + _problemID + "' " +
            " WHERE workplaceid = '" + _workplace + "' \r\n" +
            " AND Calendardate = '" + _date + "' \r\n" +
            " AND SectionID = '" + MinerID + "'  \r\n" +
            " AND activity = '" + WPAct + "' ";

            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            var errorMsg = theData.ExecuteInstruction();
            return true;
        }
        #endregion Methods   

        private void txtDevPegToFace_EditValueChanged(object sender, EventArgs e)
        {
            decimal NewPegtoface = Convert.ToDecimal(txtDevPegToFace.Text);
            decimal NewAdv = NewPegtoface - PegFrom;

            txtDevAdv.Text = NewAdv.ToString();
        }

        private void pnlA_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void gvLostBlast_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                if ((e.Column.Name == "colMoCycle") || (e.Column.Name == "colSqm"))
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);
                    e.Handled = true;
                }
            }
        }

        string _Date = string.Empty;
        string _Cycle = string.Empty;
        string _Sqm = string.Empty;
        string _Problem = string.Empty;
        string _LostBlastCheck = string.Empty;

        private void gvLostBlast_DoubleClick(object sender, EventArgs e)
        {
            frmBookingsABSProblems ucProbs = new frmBookingsABSProblems();
            ucProbs.TheWorkplace = WorkplaceName;
            ucProbs.TheSection = SectionID;
            ucProbs.TheDate = DateTime.Now;
            ucProbs.TheActivity = Activity;
            ucProbs.NoteID = "NoteId";

            string sbossnotes = gvLostBlast.GetRowCellValue(gvLostBlast.FocusedRowHandle, gvLostBlast.Columns["SBossNotes"]).ToString();

            if (_Problem == string.Empty)
            {
                ucProbs._LostBlastRecon = "Y";
                ucProbs._AddProblem = "Y";
                ucProbs._EditProblem = "N";

                ucProbs._MinerID = MinerID;
                ucProbs.TheWorkplace = WorkplaceName;
                ucProbs._workplaceID = WpID;
                ucProbs.TheSection = SectionID;
                ucProbs.TheDate = Convert.ToDateTime(_Date);
                ucProbs._Date = _Date;
                ucProbs.TheActivity = Activity;
                ucProbs._prodmonth = Selectedprodmonth;
            }
            else
            {
                ucProbs._LostBlastRecon = "Y";
                ucProbs._AddProblem = "N";
                ucProbs._EditProblem = "Y";

                ucProbs.TheWorkplace = WorkplaceName;
                ucProbs._workplaceID = WpID;
                ucProbs._MinerID = MinerID;
                ucProbs._Date = _Date;
                ucProbs._prodmonth = Selectedprodmonth;
                ucProbs.TheActivity = Activity;

                int indexEmpty = _Problem.IndexOf(" ");

                string prob = _Problem.Substring(0, indexEmpty);
                _problems.Add(prob, "PR" + ";" + sbossnotes);

                ucProbs.Problems = _problems;

                if (!string.IsNullOrEmpty(lblProbNote.Text))
                    ucProbs.ProblemDesc = lblProbNote.Text;

                ucProbs._Problem = prob;

                if (_LostBlastCheck == "Y")
                {
                    ucProbs.cbxLostBlast.Checked = true;
                }
                else
                {
                    ucProbs.cbxLostBlast.Checked = false;
                }

                ucProbs._ShiftbossNotes = sbossnotes;
            }

            ucProbs._theSystemDBTag = theSystemDBTag;
            ucProbs._UserCurrentInfoConnection = connection;
            ucProbs.Problems = _problems;

            if (!string.IsNullOrEmpty(lblProbNote.Text))
                ucProbs.ProblemDesc = lblProbNote.Text;

            ucProbs.ShowDialog(this);
            _problems.Clear();

            LoadLostBlastData();
        }



        private void gvLostBlast_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            _Date = gvLostBlast.GetRowCellValue(e.RowHandle, gvLostBlast.Columns["Calendardate"]).ToString();
            _Cycle = gvLostBlast.GetRowCellValue(e.RowHandle, gvLostBlast.Columns[1]).ToString();
            _Sqm = gvLostBlast.GetRowCellValue(e.RowHandle, gvLostBlast.Columns[2]).ToString();
            _Problem = gvLostBlast.GetRowCellValue(e.RowHandle, gvLostBlast.Columns[3]).ToString().Trim();
        }

        private void gvLostBlast_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

        }

        private void repLostBlast_CheckedChanged(object sender, EventArgs e)
        {
            gvLostBlast.PostEditor();

            string CausedLostBlast = gvLostBlast.GetRowCellValue(gvLostBlast.FocusedRowHandle, gvLostBlast.Columns["CausedLostBlast"]).ToString();
            string CalendarDate = gvLostBlast.GetRowCellValue(gvLostBlast.FocusedRowHandle, gvLostBlast.Columns["Calendardate"]).ToString();
            string ProblemID = gvLostBlast.GetRowCellValue(gvLostBlast.FocusedRowHandle, gvLostBlast.Columns["Problem"]).ToString();

            MWDataManager.clsDataAccess _dbLostBlast = new MWDataManager.clsDataAccess();
            _dbLostBlast.ConnectionString = _userCurrentInfoConnection;
            _dbLostBlast.SqlStatement = "update tbl_ProblemBook set CausedLostBlast = '" + CausedLostBlast + "' where prodmonth = '" + Selectedprodmonth + "' and workplaceID = '" + WpID + "' and problemid = SUBSTRING('" + ProblemID + "',1,3) and calendardate = '" + CalendarDate + "' ";

            _dbLostBlast.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbLostBlast.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbLostBlast.ExecuteInstruction();

            CalcGrid();
        }

        private void repLostBlast_CheckStateChanged(object sender, EventArgs e)
        {

        }

        private void gvLostBlast_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridView view;
            view = sender as GridView;

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["Problem"]).ToString().Trim() == string.Empty)
            {
                e.Cancel = true;
            }
        }

        private void labelControl6_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void txtpegValue_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void grpLostBlastRecon_Enter(object sender, EventArgs e)
        {

        }

        private void tsmiWP_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}