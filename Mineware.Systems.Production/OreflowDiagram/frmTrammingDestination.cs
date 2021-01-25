using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class frmTrammingDestination : DevExpress.XtraEditors.XtraForm
    {
        #region Public variables
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        public string _BHID;
        #endregion

        public frmTrammingDestination()
        {
            InitializeComponent();
        }

        private void frmTrammingDestination_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan1.SqlStatement = " select * from dbo.tbl_OREFLOWENTITIES " +
                                    "where OreFlowcode = 'OPass' and name not in (select destination from  tbl_Oreflow_BoxholeDestination where oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(_BHID) + "') and name <> '' order by name ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();
            DataTable dtOPass = _dbMan1.ResultsDataTable;
            lbxDest.Items.Clear();
            foreach (DataRow dr1 in dtOPass.Rows)
            {
                lbxDest.Items.Add(dr1["name"].ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lbxDest.SelectedIndex > -1)
            {
                MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
                _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan1.SqlStatement = " update  tbl_Oreflow_BoxholeDestination set def = 'N' where oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(_BHID) + "' " +
                                        " insert into  tbl_Oreflow_BoxholeDestination values ('" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(_BHID) + "', " +
                                        "'" + lbxDest.SelectedItem.ToString() + "', 'Y', '', '') ";
                _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan1.queryReturnType = MWDataManager.ReturnType.longNumber;

                var result = _dbMan1.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Destination Inserted", Color.CornflowerBlue);
                }

                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}