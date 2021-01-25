using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Engineering
{
    public partial class frmAddBreakCat : DevExpress.XtraEditors.XtraForm
    {
        public frmAddBreakCat()
        {
            InitializeComponent();
        }

        public string theSystemDBTag;
        public string UserCurrentInfo;

        private void frmAddBreakCat_Load(object sender, EventArgs e)
        {
            LoadEquipment();
        }

        private void LoadEquipment()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbMan.SqlStatement = " Select * from tbl_Eng_Equip_BreakCat ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            BreakdownInfoGrid.DataSource = ds.Tables[0];

            ColID.FieldName = "UNCatID";
            ColEquipType.FieldName = "EquipType";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbID = new MWDataManager.clsDataAccess();
            _dbID.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbID.SqlStatement = " select isnull(max(UNCatID)+1,1) ID from tbl_Eng_Equip_BreakCat ";

            _dbID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbID.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbID.ExecuteInstruction();

            String ItemSaveID = _dbID.ResultsDataTable.Rows[0][0].ToString();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbMan.SqlStatement = " Insert Into tbl_Eng_Equip_BreakCat Values ( '"+ ItemSaveID + "','" + txtType.Text + "')";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            LoadEquipment();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
