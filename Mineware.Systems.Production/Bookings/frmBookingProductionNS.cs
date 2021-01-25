using DevExpress.XtraEditors;
using Mineware.Systems.GlobalConnect;
using MWDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Mineware.Systems.Production.Bookings
{
    public partial class frmBookingProductionNS : DevExpress.XtraEditors.XtraForm
    {
        public frmBookingProductionNS()
        {
            InitializeComponent();
        }

        #region Public Variables
        public string theSystemDBTag = string.Empty;
        public string connection = string.Empty;
        public string WorkplaceName;
        public string WpID;
        public string SectionID;
        public int Activity;
        public DateTime TheDate;
        public string PlanFL;

        public bool CanChangeDate { set { dtTheDate.Enabled = value; } }
        #endregion

        #region Private Variables
        private string _userCurrentInfoConnection;
        private Dictionary<string, string> _problems = new Dictionary<string, string>();
        //private DateTime _theDate;
        #endregion

        private void frmBookingProductionNS_Load(object sender, EventArgs e)
        {
            pnlS.Visible = false;
            pnlB.Visible = false;
            pnlA.Visible = true;

            txtPlanned.EditValue = PlanFL;

            //Ad WP Name to title
            this.Text += " (" + WorkplaceName + ")";

            _userCurrentInfoConnection = TConnections.GetConnectionString(theSystemDBTag, connection);

            dtTheDate.EditValue = TheDate;

            LoadIncidentsData();
            LoadBookedData();
            SetDSBookValue();
            LoadABSData();
        }

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
                                     ,CASE WHEN[Application_Origin] LIKE 'Pivot%' THEN 'Pivot Action' ELSE 'Normal Action' END AS[App_Origin] FROM[dbo].[tbl_Incidents]
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

        private void btnTeamA_Click(object sender, EventArgs e)
        {
            //Clear all selections
            ClearBAndSPanels();

            var btn = sender as SimpleButton;
            bool isSelected = Convert.ToBoolean(btn.Tag);
            //update the buttons tag to know if it is selected or not
            btn.Tag = !isSelected;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBookingProductionNS));

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

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
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
                absCode = "B" + absCode;

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
                absCode = "S" + absCode;

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

            string Completed = string.Empty;

            if (cbxCompleted.Checked == true)
            {
                Completed = "Y";
            }

            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _userCurrentInfoConnection;
            theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = ReturnType.DataTable;

            theData.SqlStatement = theData.SqlStatement + " delete from tbl_Booking_NightShift where workplaceid = '" + WpID + "' and calendardate = '" + String.Format("{0:yyyy-MM-dd}", dtTheDate.EditValue) + "' \r\n\r\n";

            theData.SqlStatement = theData.SqlStatement + " insert into tbl_Booking_NightShift Values(  \r\n" +
            " '" + WpID + "',  " +
            " '" + String.Format("{0:yyyy-MM-dd}", dtTheDate.EditValue) + "',  " +
            "  '',  " +
            " '" + txtCleaned.EditValue + "',  \r\n" +
            "  '" + absCode + "',  " +
            " '" + _problems.Keys.FirstOrDefault() + "',  \r\n" +
            "  '" + Completed + "', '" + String.Format("{0:yyyy-MM-dd}", dtBackDate.EditValue) + "', '" + txtVerification.Text + "', '" + txtAdditionalNotes.Text + "')  \r\n";
            //theData.SqlStatement = @"insert into tbl_Booking_NightShift SET [ABSCode] = '" + absCode + @"', [Range] = '" + absRange + @"' WHERE SectionID = '" + SectionID + @"' AND WorkplaceID = '" + WpID + @"' AND CalendarDate = '" + TheDate.ToString("yyyy-MM-dd") + @"'";
            var result = theData.ExecuteInstruction();

            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Saved", Color.CornflowerBlue);
                this.Close();
            }
        }

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

        private void btnAddProblems_Click(object sender, EventArgs e)
        {
            frmBookingsABSProblems ucProbs = new frmBookingsABSProblems();
            //Setup the labels
            ucProbs.TheWorkplace = WorkplaceName;
            ucProbs.TheSection = SectionID;
            ucProbs.TheDate = DateTime.Now;
            ucProbs.TheActivity = Activity;
            ucProbs.NoteID = "NoteId";
            ucProbs._theSystemDBTag = theSystemDBTag;
            ucProbs._UserCurrentInfoConnection = connection;
            ucProbs.Problems = _problems;

            if (!string.IsNullOrEmpty(lblProbNote.Text))
                ucProbs.ProblemDesc = lblProbNote.Text;

            ucProbs.ShowDialog(this);

            lblProbNote.Text = ProductionGlobal.ProductionGlobal.ExtractAfterColon(ucProbs.lbNoteID.Text);
            lblProbNote.Visible = true;
        }

        private void dtTheDate_EditValueChanged(object sender, EventArgs e)
        {
            clsDataAccess theDataa = new clsDataAccess();
            theDataa.ConnectionString = _userCurrentInfoConnection;
            theDataa.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theDataa.queryReturnType = ReturnType.DataTable;
            theDataa.SqlStatement = "select max(CalendarDate) PrevWorkingDay from tbl_Planning \r\n" +
                                   "where CalendarDate < '" + String.Format("{0:yyyy-MM-dd}", dtTheDate.EditValue) + "' and WorkingDay = 'Y' and workplaceid = '" + WpID + "'  ";
            var result = theDataa.ExecuteInstruction();

            DataTable dt = theDataa.ResultsDataTable;

            if (result.success)
            {
                dtBackDate.EditValue = dt.Rows[0]["PrevWorkingDay"];
            }

            LoadBookedData();
            LoadABSData();
            SetDSBookValue();
        }

        private void SetDSBookValue()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _userCurrentInfoConnection;
            theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = ReturnType.DataTable;
            theData.SqlStatement = "Select case when Activity <> '1' then BookFL  \r\n" +
                                   "else BookAdv end as Booked  \r\n" +
                                   "from tbl_Planning where CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", dtBackDate.EditValue) + "' and WorkplaceID = '" + WpID + "'";
            var result = theData.ExecuteInstruction();

            DataTable dt = theData.ResultsDataTable;

            if (result.success)
            {
                txtDSBooked.EditValue = dt.Rows[0]["Booked"].ToString();
            }
        }

        private void LoadBookedData()
        {
            clsDataAccess theDataa = new clsDataAccess();
            theDataa.ConnectionString = _userCurrentInfoConnection;
            theDataa.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theDataa.queryReturnType = ReturnType.DataTable;
            theDataa.SqlStatement = "  Select book.*,pr.ProblemDesc  \r\n" +
                                    "from tbl_Booking_NightShift book  \r\n" +
                                    "left outer join  \r\n" +
                                    "[tbl_Code_Problem_Main] PR on book.NProb = pr.ProblemID   \r\n" +
                                    "where workplaceid = '" + WpID + "' and calendardate = '" + String.Format("{0:yyyy-MM-dd}", dtTheDate.EditValue) + "' ";
            var result = theDataa.ExecuteInstruction();

            DataTable dt = theDataa.ResultsDataTable;

            if (result.success)
            {
                if (dt.Rows.Count > 0)
                {
                    txtCleaned.EditValue = dt.Rows[0]["Fl"];
                    txtVerification.EditValue = dt.Rows[0]["PrevBook"];

                    if (dt.Rows[0]["PrevBook"].ToString() == "Y")
                    {
                        cbxCompleted.Checked = true;
                    }

                    string prob = dt.Rows[0]["NProb"].ToString();

                    _problems.Clear();

                    if (!string.IsNullOrWhiteSpace(prob))
                    {
                        string probDesc = dt.Rows[0]["ProblemDesc"].ToString();
                        string SBNotes = " ";
                        _problems.Add(prob, "PR" + ";" + SBNotes);

                        lblProbNote.Text = probDesc;
                        lblProbNote.Visible = true;
                    }
                    else
                    {
                        lblProbNote.Visible = false;
                    }
                }
            }
        }

        private void LoadABSData()
        {
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _userCurrentInfoConnection;
            _dbAction.SqlStatement = "SELECT [NCode] ABSCode,'' [Range],isnull(ABSNotes,'') ABSNotes FROM tbl_Booking_NightShift \r\n" +
                                    "WHERE  WorkplaceID = '" + WpID + "' AND CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", dtTheDate.EditValue) + "' ";

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

                btnTeamA_Click(btn, null);
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
                    btnTeamA_Click(btn, null);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}