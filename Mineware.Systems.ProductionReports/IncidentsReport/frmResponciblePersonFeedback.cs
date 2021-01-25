using System;
using System.Data;
using System.Linq;

namespace Mineware.Systems.ProductionReports.IncidentsReport
{
    public partial class frmResponciblePersonFeedback : DevExpress.XtraEditors.XtraForm
    {
        #region Fields and Properties
        /// <summary>
        /// The workplace name
        /// </summary>
        public string Workplace { get; set; }

        /// <summary>
        /// The action ID of the incident
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// The action description
        /// </summary>
        public string ActionDesc { get { return this.Text; } set { this.Text = value; } }
        IncidentsDashboard Report = new IncidentsDashboard();
        Procedures procs = new Procedures();
        public string ConnectionString { get; set; }
        #endregion Fields and Properties

        #region Constructor
        public frmResponciblePersonFeedback()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Events
        /// <summary>
        /// Load the incident
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmResponciblePersonFeedback_Load(object sender, EventArgs e)
        {
            GetFeedback();
        }

        /// <summary>
        /// Save and close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFeedback();
            this.Close();
        }

        /// <summary>
        /// Just close form without saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion Events

        #region Methods
        /// <summary>
        /// Get the feedback text
        /// </summary>
        private void GetFeedback()
        {
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = ConnectionString;
            _dbAction.SqlStatement = @"SELECT [Responsible_Person_Feedback] FROM [tbl_Incidents] WHERE[Mineware_Action_ID] = '" + ActionId + @"'";
            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            if (_dbAction.ResultsDataTable?.Rows.Count > 0)
                txtFeedback.Text = _dbAction.ResultsDataTable.Rows[0][0].ToString();
        }

        /// <summary>
        /// Save the feedback
        /// </summary>
        private void SaveFeedback()
        {
            string query = string.Empty;

            if (chkClose.Checked)
            {
                query = @" UPDATE  [tbl_Incidents] SET [Responsible_Person_Feedback] = '" + txtFeedback.Text + @"', [Action_Status] = 'Closed', [Action_Close_Date] = GETDATE(), [Action_Progress] = 100 WHERE [Mineware_Action_ID] = '" + ActionId + @"'";
            }
            else
            {
                query = @" UPDATE  [tbl_Incidents] SET [Responsible_Person_Feedback] = '" + txtFeedback.Text + @"' WHERE [Mineware_Action_ID] = '" + ActionId + @"'";
            }

            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = ConnectionString;
            _dbAction.SqlStatement = query;
            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            
        }

        #endregion Methods
    }
}
