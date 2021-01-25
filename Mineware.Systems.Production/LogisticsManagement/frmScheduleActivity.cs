using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class frmScheduleActivity : DevExpress.XtraEditors.XtraForm
    {
        public frmScheduleActivity()
        {
            InitializeComponent();
        }

        #region public variables
        public string _UserCurrentInfoConnection;
        public string lblMainActID;
        public string lblDay;
        public string lblProceedson;
        public string lblSubAct;
        public string lblFrmType;
        public string lblMainAct;
        public string OrigSSlabel;
        public string lblPrevStarton;
        #endregion

        private void Filtertxt_TextChanged(object sender, EventArgs e)
        {
            string filter = "%" + Filtertxt.Text + "%";

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " Select * from tbl_SDB_SubActivity where Description like '" + filter + "' ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            lbxActivity.Items.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                lbxActivity.Items.Add(dr["Description"].ToString());
            }
        }

        private void frmScheduleActivity_Load(object sender, EventArgs e)
        {
            LoadActivities();
            LoadProcedingSubAct();

            if (lblFrmType == "Edit")
            {
                for (int i = 0; i < lbxActivity.Items.Count; i++)
                {
                    string test = lbxActivity.Items[i].ToString();

                    if (lblSubAct == lbxActivity.Items[i].ToString())
                    {
                        lbxActivity.SelectedIndex = i;
                    }
                }
            }

            if (lblFrmType == "Edit")
            {
                for (int i = 0; i < cmbProceedsSubAct.Items.Count; i++)
                {
                    string test = cmbProceedsSubAct.Items[i].ToString();

                    if (lblProceedson == cmbProceedsSubAct.Items[i].ToString())
                    {
                        cmbProceedsSubAct.SelectedIndex = i;
                    }
                }

                cmbProceedsSubAct.Enabled = false;

                MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                _dbManMain.SqlStatement = " Select Starton + Duration endon,* from tbl_SDB_Activity_Schedule where MainActID = '" + lblMainActID + "' and SubAct = '" + lbxActivity.SelectedItem + "'   ";

                _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMain.ExecuteInstruction();

                DataTable dt = _dbManMain.ResultsDataTable;

                foreach (DataRow dr in dt.Rows)
                {
                    spinEditOrder.Value = Convert.ToInt32(dr["order1"].ToString());
                }
            }
        }

        private void LoadProcedingSubAct()
        {
            MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
            _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
            _dbManMain.SqlStatement = "  Select SubActID, Subact from tbl_SDB_Activity_Schedule Where MainactID = '" + lblMainActID + "' order by starton desc  ";

            _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMain.ExecuteInstruction();

            DataTable dtMain = _dbManMain.ResultsDataTable;

            foreach (DataRow dr in dtMain.Rows)
            {
                cmbProceedsSubAct.Items.Add(dr["Subact"].ToString());
            }

            cmbProceedsSubAct.Items.Add("None");

            cmbProceedsSubAct.SelectedIndex = 0;
        }

        private void LoadActivities()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);

            if (lblFrmType == "Add")
                _dbMan.SqlStatement = " Select * from tbl_SDB_SubActivity ";

            if (lblFrmType == "Edit")
                _dbMan.SqlStatement = " Select * from tbl_SDB_SubActivity  where Description = '" + lblSubAct + "'   ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtSub = _dbMan.ResultsDataTable;

            foreach (DataRow dr in dtSub.Rows)
            {
                lbxActivity.Items.Add(dr["Description"].ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbProceedsSubAct.Text))
            {
                MessageBox.Show("Please Select a Proceding Sub Activity");
                return;
            }

            if (string.IsNullOrEmpty(lbxActivity.Text))
            {
                MessageBox.Show("Please Select a Sub Activity");
                return;
            }

            string Proceeds = string.Empty;

            if (lblFrmType == "Add")
            {
                MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                _dbManMain.SqlStatement = "Insert into [tbl_SDB_Activity_Schedule] (MainActID,MainAct,SubActID,SubAct,Starton,Duration,ProceedsOn, order1)  \r\n" +
                                          " values ('" + lblMainActID + "','" + lblMainAct + "',(Select SubActID from tbl_SDB_SubActivity where Description = '" + lbxActivity.SelectedItem + "' ),'" + lbxActivity.SelectedItem + "','" + spinEditStartDay.Value.ToString() + "','" + spinEditDuration.Value.ToString() + "',  (Select SubActID from tbl_SDB_SubActivity where Description = '" + cmbProceedsSubAct.Text + "' ), '" + spinEditOrder.Value.ToString() + "' ) ";

                _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMain.ExecuteInstruction();
            }

            if (lblFrmType == "Edit")
            {
                MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                _dbManMain.SqlStatement = "update [tbl_SDB_Activity_Schedule] set proceedson = (Select max(SubActID) from tbl_SDB_SubActivity where Description = '" + cmbProceedsSubAct.Text + "' ) , Starton = '" + spinEditStartDay.Value.ToString() + "' , Duration = '" + spinEditDuration.Value.ToString() + "', order1 = '" + spinEditOrder.Value.ToString() + "' \r\n" +
                    "where mainactid = '" + lblMainActID + "' and SubAct = '" + lblSubAct + "' and Starton = '" + OrigSSlabel + "'  ";

                _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMain.ExecuteInstruction();
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbxActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
            _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
            _dbManMain.SqlStatement = " Select * from tbl_SDB_SubActivity where Description = '" + lbxActivity.SelectedItem + "'  ";

            _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMain.ExecuteInstruction();

            DataTable dt = _dbManMain.ResultsDataTable;

            foreach (DataRow dr in dt.Rows)
            {
                spinEditDuration.Value = Convert.ToInt32(dr["DefaultDuration"].ToString());
            }
        }

        private void cmbProceedsSubAct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProceedsSubAct.Text != "None")
            {
                MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                _dbManMain.SqlStatement = " Select Starton + Duration endon,* from tbl_SDB_Activity_Schedule where MainActID = '" + lblMainActID + "' and SubAct = '" + cmbProceedsSubAct.Text + "'  ";

                _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMain.ExecuteInstruction();

                DataTable dt = _dbManMain.ResultsDataTable;

                foreach (DataRow dr in dt.Rows)
                {
                    spinEditStartDay.Value = Convert.ToInt32(dr["endon"].ToString());
                }
                spinEditStartDay.Enabled = false;
            }
            else
            {
                spinEditStartDay.Enabled = true;
                spinEditStartDay.Value = 1;
            }
        }
    }
}