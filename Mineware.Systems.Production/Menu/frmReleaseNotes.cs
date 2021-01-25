using System;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.Menu
{
    public partial class frmReleaseNotes : DevExpress.XtraEditors.XtraForm
    {
        public frmReleaseNotes()
        {
            InitializeComponent();
        }

        private void frmReleaseNotes_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbMan.SqlStatement = " select Version \r\n" +
                                    " from tbl_SysSet \r\n" +
                                    "  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable SubB = _dbMan.ResultsDataTable;

            if (SubB.Rows.Count > 0)
            {

                VersionTxt.EditValue = SubB.Rows[0]["Version"].ToString();

            }
        }

        private void memoComments_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}