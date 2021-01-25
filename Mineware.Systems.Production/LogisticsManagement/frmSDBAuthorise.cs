using Mineware.Systems.GlobalConnect;
using System;
using System.Linq;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class frmSDBAuthorise : DevExpress.XtraEditors.XtraForm
    {
        public frmSDBAuthorise()
        {
            InitializeComponent();
        }

        #region public variables
        public string _UserCurrentInfoConnection;
        public string lblAuthID;
        public string lblWP;
        public string lblMainAct;
        public string lblUserID;
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            DateTime test = System.DateTime.Today;

            MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
            _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
            _dbManMain.SqlStatement = " insert into tbl_SDB_AuthoriseNotes (AuthID,MainActID,Workpalce,AuthNote,UserID) values ('" + lblAuthID + "', (Select MainActID from tbl_SDB_Activity where Description = '" + lblMainAct + "' ),'" + lblWP + "' , '" + Notetxt.Text + "','" + lblUserID + "')  ";
            _dbManMain.SqlStatement = _dbManMain.SqlStatement + "\r\n\r\n update tbl_SDB_Activity_Authorisation set Authorise = 'Y' , AuthDate = '" + test + "' where AuthID = '" + lblAuthID + "' and MainActID = (Select MainActID from tbl_SDB_Activity where Description = '" + lblMainAct + "' ) and Workplace = '" + lblWP + "'  ";

            _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMain.ExecuteInstruction();

            this.Close();
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSDBAuthorise_Load(object sender, EventArgs e)
        {

        }
    }
}