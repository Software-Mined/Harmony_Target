using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Mineware.Systems.GlobalConnect;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class FrmWorkplaces : DevExpress.XtraEditors.XtraForm
    {
        public FrmWorkplaces()
        {
            InitializeComponent();
        }

        public string theSystemDBTag = "";
        public string UserCurrentInfo = "";

        private void LoadLatestWP()
        {
            MWDataManager.clsDataAccess _LatestWP = new MWDataManager.clsDataAccess();
            _LatestWP.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo);
            _LatestWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LatestWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LatestWP.SqlStatement = " select isnull(max(convert(int,substring(workplaceid,2,5))),0) LatestID from [tbl_Workplace] where Activity <> 1";

            _LatestWP.ExecuteInstruction();

            DataTable dtWPID = new DataTable();
            dtWPID = _LatestWP.ResultsDataTable;
            
            txtWPID.Text = _LatestWP.ResultsDataTable.Rows[0][0].ToString();
        }

        private void FrmWorkplaces_Load(object sender, EventArgs e)
        {
            LoadLatestWP();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}