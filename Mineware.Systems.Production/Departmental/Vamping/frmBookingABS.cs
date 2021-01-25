using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
///using MWDataManager;

namespace Mineware.Systems.Production.Departmental.Vamping
{
    public partial class frmBookingABS : DevExpress.XtraEditors.XtraForm
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
        public string absCode = string.Empty;
        public string ABSCode;
        public string ABSRange;
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
       
        /// <summary>
        /// Is this the night shift booking
        /// </summary>
        public bool IsNightShiftBooking = false;

        public string Selectedprodmonth;

        public string theSystemDBTag;

        public string connection;

     
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
        public frmBookingABS()
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
           

            _userCurrentInfoConnection = TConnections.GetConnectionString(theSystemDBTag, connection);
           

           

            //Ad WP Name to title
            this.Text += " (" + WorkplaceName + ")";

         

            dtTheDate_EditValueChanged(null, null);

           
            LoadABSData();
          
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

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBookingABS));

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
                        ABSCode = "A";
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
                        ABSCode = "B";
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
                        ABSCode = "S";
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBookingABS));

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
       

        /// <summary>
        /// Set the recon visible depending on day of week
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtTheDate_EditValueChanged(object sender, EventArgs e)
        {
            string thedate = dtTheDate.EditValue == null ? DateTime.Now.ToString("dd-MMM-yyyy") : ((DateTime)dtTheDate.EditValue).ToString("dd-MMM-yyyy");

            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = _userCurrentInfoConnection;
            _dbAction.SqlStatement = @"Select workingday,isnull(pumahola,'N') pumahola from tbl_planning where Sectionid = '" + MinerID + "' and calendardate = '" + thedate + "' and workplaceid = '" + WpID + "' ";

            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            var result = _dbAction.ExecuteInstruction();

            

            TheDate = (DateTime)dtTheDate.EditValue;

            //Load the data
          
            LoadABSData();
          
        }

        /// <summary>
        /// Open the action form to add responcible person feedback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

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
       

        /// <summary>
        /// Save the data to the DB
        /// </summary>
        private void SaveData()
        {
            

            string tablename = "[dbo].[tbl_PLANNING_Vamping]";

            

            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = _userCurrentInfoConnection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;

           

            //Save ABS ==============================================
           
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

            theData.SqlStatement = @"UPDATE " + tablename + " SET [ABSCode] = '" + absCode + @"', [ABSRange] = '" + absRange + @"' , absnotes = '" + txtAdditionalNotes.Text + "' WHERE  WorkplaceID = '" + WpID + @"' AND SectionID = '" + MinerID + @"' AND activity = '" + WPAct + "' AND CalendarDate = '" + TheDate.ToString("yyyy-MM-dd") + @"'";
            theData.ExecuteInstruction();

            this.Close();
        }

        /// <summary>
        /// Get the workplaces for the dropdown menu
        /// </summary>
        /// <remarks>TODO: Still need to add the sectionID in query</remarks>
        

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
        
        #endregion Methods   

        
        private void pnlA_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

       

        string _Date = string.Empty;
        string _Cycle = string.Empty;
        string _Sqm = string.Empty;
        string _Problem = string.Empty;
        string _LostBlastCheck = string.Empty;

        



       

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