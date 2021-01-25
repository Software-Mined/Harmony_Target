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
    public partial class frmAddEquipment : DevExpress.XtraEditors.XtraForm
    {
        public frmAddEquipment()
        {
            InitializeComponent();
        }

        public string theSystemDBTag;
        public string UserCurrentInfo;

        private void frmAddEquipment_Load(object sender, EventArgs e)
        {
            LoadEquipment();
        }

        private void LoadEquipment()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbMan.SqlStatement = " Select * from [dbo].[tbl_Eng_Equip_Inventory] order by EquipType, Equipno ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            EquipmentInfoGrid.DataSource = ds.Tables[0];

            colEquipNo.FieldName = "EquipNo";
            colEquipType.FieldName = "EquipType";
            colEquipMake.FieldName = "EquipMake";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbMan.SqlStatement = " Insert Into tbl_Eng_Equip_Inventory Values ('"+ txtNo.Text + "', '" + txtType.Text + "', '" + txtMake.Text + "', '')";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            LoadEquipment();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
