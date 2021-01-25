using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class frmHR : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Global parameters
        /// </summary>
        public string UserCurrentInfo;
        public string EditID;
        Procedures procs = new Procedures();

        public frmHR()
        {
            InitializeComponent();
        }

        void LoadMethodGroups()
        {
            
            //populate EnquirerID combo
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

            _dbMan.SqlStatement = " Select * from tbl_Code_Activity ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            DataTable dt = _dbMan.ResultsDataTable;

            foreach (DataRow dr in dt.Rows)
            {
                txtActivity.Properties.Items.Add(dr["Activity"].ToString() + ":" + dr["Description"].ToString());
            }      

            //MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            //_dbMan2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo);

            //_dbMan2.SqlStatement = " select * from dbo.tbl_Code_ProblemGroup ";
            //_dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbMan2.ExecuteInstruction();
            //DataTable dt2 = _dbMan2.ResultsDataTable;

            //foreach (DataRow dr2 in dt2.Rows)
            //{
            //    txtPropCode.Properties.Items.Add(dr2["ProblemGroupCode"].ToString());
            //}

          
        }

        private void frmProblemProperties_Load(object sender, EventArgs e)
        {
            LoadMethodGroups();
            

        }

        void SaveProblems()
        {
            string IsValid = string.Empty;
            //do checks           
            if (txtActivity.Text == string.Empty)
            {
                MessageBox.Show("Please select the Activity", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtGroupName.Text == string.Empty)
            {
                MessageBox.Show("Please provide a group name", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           

        
            if (EditID == null)
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                _dbMan.SqlStatement = " insert into tbl_HR_MethodGroups  " +
                                      " VALUES ( '" + procs.ExtractBeforeColon(txtActivity.Text.ToString()) + "', '" + txtGroupName.Text.ToString() + "')";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
            }
            else
            {
                //MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                //_dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo);

                //_dbMan.SqlStatement = " update tbl_Problem set ProblemGroupCode = '" + txtPropCode.Text.ToString() + "', EnquirerID = '" + procs.ExtractBeforeColon(txtActivity.Text.ToString()) + "', " +
                //                      " Description =  '" + txtProbDesc.Text.ToString() + "', ExtraInfo =   '" + txtExtraInfo.SelectedIndex + "', " +
                //                      " Valid  = '" + IsValid.ToString() + "', HQCat = '" + txtHQCat.Text.ToString() + "', emailgroup = '" + txtEmailGroup.Text.ToString() + "' " +
                //                      " where ProblemID = '" + txtProbID.Text.ToString() + "' ";
                //_dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbMan.ExecuteInstruction();

            }           

            IsValid = "Y";
            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved.", Color.CornflowerBlue);
        }


        private void btnSaveProb_Click_1(object sender, EventArgs e)
        {
            SaveProblems();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}