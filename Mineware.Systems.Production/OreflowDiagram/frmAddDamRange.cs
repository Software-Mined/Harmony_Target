using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class frmAddDamRange : DevExpress.XtraEditors.XtraForm
    {
        public string _OreflowType;
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;

        public bool _IsEdit = false;
        public string _RangeNamecont = string.Empty;
        public string _CbxValue = string.Empty;
        bool DamExist = false;

        public frmAddDamRange()
        {
            InitializeComponent();
        }

        private void frmAddDamRange_Load(object sender, EventArgs e)
        {
            loadDamID();
            loaddam();
            cbxDam.Text = _CbxValue;
        }

        private void loadDamID()
        {
            MWDataManager.clsDataAccess _dbLoadDamCombo = new MWDataManager.clsDataAccess();
            _dbLoadDamCombo.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbLoadDamCombo.SqlStatement = "select * from tbl_Backfill_Dam  ";
            _dbLoadDamCombo.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbLoadDamCombo.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbLoadDamCombo.ExecuteInstruction();

            DataTable DamIDData = _dbLoadDamCombo.ResultsDataTable;

            foreach (DataRow dr1 in DamIDData.Rows)
            {
                cbxDam.Items.Add(dr1["DamID"] + ":" + dr1["DamName"]);
            }

            MWDataManager.clsDataAccess _dbLoadPacCombo = new MWDataManager.clsDataAccess();
            _dbLoadPacCombo.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbLoadPacCombo.SqlStatement = "select * from tbl_Backfill_Pachuca  ";
            _dbLoadPacCombo.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbLoadPacCombo.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbLoadPacCombo.ExecuteInstruction();

            DataTable PacIDData = _dbLoadPacCombo.ResultsDataTable;

            foreach (DataRow dr1 in PacIDData.Rows)
            {
                cbxDam.Items.Add(dr1["PacID"] + ":" + dr1["PacName"]);
            }
        }

        private void loaddam()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "select rng.*,dm.DamName from tbl_Backfill_Range rng, \r\n" +
                                  "(Select * from tbl_Backfill_Dam \r\n" +
                                  "union \r\n" +
                                  "Select pacid DamID, PacName DamName from tbl_Backfill_Pachuca) dm \r\n" +
                                  " where rng.Damid = dm.Damid  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            gcOreflowentities.DataSource = null;
            gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
            colDescription.FieldName = "RangeName";
            colID.FieldName = "RangeID";
            colDam.FieldName = "DamName";
            colDistance.FieldName = "Distance";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
            _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)  \r\n" +
                                      "select RangeID, RangeName, 'Range', '1' from  \r\n " +
                                      "[tbl_Backfill_Range] where RangeID not in (select OreFlowID from [tbl_OreFlowEntities])";
            _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSave.ExecuteInstruction();

            var result = _dbManSave.ExecuteInstruction();
            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
            }

            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbxDam.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Dam");
                return;
            }

            if (string.IsNullOrEmpty(txtRangeName.Text))
            {
                MessageBox.Show("Please fill in a Range Name");
                return;
            }

            string DamNameID = cbxDam.Text;

            MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
            _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbBackfill.SqlStatement = "Insert Into [tbl_Backfill_Range] Values \r\n " +
                                        "((select top 1( 'R' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(RangeID,2,5))+1+10000),2,5))  RangeID from (select * from dbo.tbl_Backfill_Range  \r\n" +
                                        " union  \r\n" +
                                        " select 'R0001', '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(DamNameID) + "' , '" + txtRangeName.Text + "'  ," + Convert.ToDecimal(speDistance.EditValue) + " ) a Order By RangeID desc ), '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(DamNameID) + "'    , '" + txtRangeName.Text + "' , " + Convert.ToDecimal(speDistance.EditValue) + ")";
            _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            var result = _dbBackfill.ExecuteInstruction();
            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
            }

            loaddam();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string RangeID = gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, "RangeID").ToString();

            MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
            _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + RangeID + "'  \r\n";

            _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Delete from tbl_Backfill_Range where RangeID = '" + RangeID + "' ";
            _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

            var result = _dbBackfill.ExecuteInstruction();
            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
            }

            loaddam();
        }
    }
}