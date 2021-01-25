using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Planning
{
    public partial class frmGraphicsPrePlanningEngInventory : DevExpress.XtraEditors.XtraForm
    {
        public string _theSystemDBTag;
        public string _UserCurrentInfo;

        public string IsEdit;
        public string FormStatus;
        public string EditID;

        public frmGraphicsPrePlanningEngInventory()
        {
            InitializeComponent();
        }

        private void frmGraphicsPrePlanningEngInventory_Load(object sender, EventArgs e)
        {
            cmbManufacturer.SelectedIndex = 0;

            if (IsEdit == "Y")
            {
                EngInvAddPanel.Visible = false;
            }

            LoadEngEquipment();
            PrePlanningEngInvAdd();
        }

        private void LoadEngEquipment()
        {
            MWDataManager.clsDataAccess _dbManEngEquipment = new MWDataManager.clsDataAccess();
            _dbManEngEquipment.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbManEngEquipment.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEngEquipment.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEngEquipment.SqlStatement = " select * from [dbo].[tbl_Preplanning_Eng_Inventory] order by EquiptmentType";

            _dbManEngEquipment.ExecuteInstruction();

            DataTable dtEngEquipt = _dbManEngEquipment.ResultsDataTable;

            DataSet ds = new DataSet();
            ds.Tables.Add(dtEngEquipt);

            gcEngInv.DataSource = ds.Tables[0];

            colID.FieldName = "ID";
            colEquipmentType.FieldName = "EquiptmentType";
            colEquipmentNumber.FieldName = "MotorNumber";
            colSize.FieldName = "Size";
            colManufacturer.FieldName = "Manufacturer";
        }

        private void PrePlanningEngInvAdd()
        {
            ///Load Equiptment Type
            MWDataManager.clsDataAccess _dbManEngEquipment = new MWDataManager.clsDataAccess();
            _dbManEngEquipment.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbManEngEquipment.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEngEquipment.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEngEquipment.SqlStatement = " select 'Face Winch Number and Size' EquipmentType \r\n " +
                                               " Union select 'Face Winch Motor Number and Size' EquipmentType \r\n " +
                                               " Union select 'Gully Winch Number and Size' EquipmentType \r\n " +
                                               " Union select 'Gully Winch Motor Number and Size' EquipmentType \r\n " +
                                               " union select 'Dip Gully Winch Number and Size' EquipmentType \r\n " +
                                               " union select 'Dip Gully Winch Motor Number and Size' EquipmentType \r\n " +
                                               " union select 'Centre Gully Winch Number and Size' EquipmentType \r\n " +
                                               " union select 'Centre Gully Winch Motor Number and Size' EquipmentType \r\n " +
                                               " union select 'Escape Gully Winch Number and Size' EquipmentType \r\n " +
                                               " union select 'Escape Gully Winch Motor Number and Size' EquipmentType \r\n " +
                                               " union select 'Waterjet Number and Size' EquipmentType \r\n " +
                                               " union select 'Waterjet Motor Number and Size' EquipmentType \r\n " +
                                               " union select 'MonoWinch Number and Size' EquipmentType \r\n " +
                                               " union select 'MonoWinch Motor Number and Size' EquipmentType \r\n " +
                                               " union select 'Boxfront Number and Size' EquipmentType ";

            _dbManEngEquipment.ExecuteInstruction();

            DataTable dt = _dbManEngEquipment.ResultsDataTable;

            foreach (DataRow dr in dt.Rows)
            {
                cmbEquipmentType.Items.Add(dr["EquipmentType"].ToString());
            }

            if (IsEdit == "Y")
            {
                MWDataManager.clsDataAccess _dbManEngEquipmentEdit = new MWDataManager.clsDataAccess();
                _dbManEngEquipmentEdit.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
                _dbManEngEquipmentEdit.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManEngEquipmentEdit.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManEngEquipmentEdit.SqlStatement = "select * from tbl_Preplanning_Eng_Inventory where ID = '" + EditID + "' ";

                _dbManEngEquipmentEdit.ExecuteInstruction();



                DataTable dtedit = _dbManEngEquipmentEdit.ResultsDataTable;

                foreach (DataRow dr in dtedit.Rows)
                {
                    for (int i = 0; i < cmbEquipmentType.Items.Count; i++)
                    {
                        if (dr["EquiptmentType"].ToString() == cmbEquipmentType.Items[i].ToString())
                        {
                            cmbEquipmentType.SelectedIndex = i;
                        }

                        txtMotorNumber.Text = dr["MotorNumber"].ToString();
                        txtSize.Text = dr["Size"].ToString();
                        //txtManufacturer.Text = dr["Manufacturer"].ToString();
                    }
                }

                foreach (DataRow dr1 in dtedit.Rows)
                {
                    for (int ii = 0; ii < cmbManufacturer.Items.Count; ii++)
                    {
                        if (dr1["Manufacturer"].ToString() == cmbManufacturer.Items[ii].ToString())
                        {
                            cmbManufacturer.SelectedIndex = ii;
                        }

                        txtMotorNumber.Text = dr1["Manufacturer"].ToString();
                        txtSize.Text = dr1["Size"].ToString();
                        //txtManufacturer.Text = dr1["Manufacturer"].ToString();
                    }
                }

            }

        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cmbEquipmentType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Equipment type");
                return;
            }

            if (txtMotorNumber.Text == string.Empty)
            {
                MessageBox.Show("Please fill in a Equipment Number");
                return;
            }

            if (txtSize.Text == string.Empty)
            {
                MessageBox.Show("Please fill in a Size");
                return;
            }

            if (cmbManufacturer.SelectedIndex == 0)
            {
                MessageBox.Show("Please fill in a Manufacturer");
                return;
            }

            MWDataManager.clsDataAccess _dbManEngEquipmentEdit = new MWDataManager.clsDataAccess();
            _dbManEngEquipmentEdit.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbManEngEquipmentEdit.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEngEquipmentEdit.queryReturnType = MWDataManager.ReturnType.DataTable;

            if (IsEdit == "N")
            {
                _dbManEngEquipmentEdit.SqlStatement = " declare @Wp varchar(100) \r\n" +
                                                   " set @Wp = ( select max(WorkplaceID) from [dbo].[tbl_Preplanning_Eng_Inventory] \r\n" +
                                                   " where MotorNumber = '" + txtMotorNumber.Text + "' ) \r\n" +

                                                   " Delete from [dbo].[tbl_Preplanning_Eng_Inventory] \r\n" +
                                                   " where MotorNumber = '" + txtMotorNumber.Text + "' \r\n" +

                                                   "insert into [dbo].[tbl_Preplanning_Eng_Inventory] \r\n" +
                                                   "(EquiptmentType,MotorNumber,Size,Manufacturer,WorkplaceID) \r\n" +
                                                   " values ('" + cmbEquipmentType.SelectedItem.ToString() + "','" + txtMotorNumber.Text + "','" + txtSize.Text + "','" + cmbManufacturer.SelectedItem.ToString() + "', @Wp)  ";
            }

            if (IsEdit == "Y")
            {
                _dbManEngEquipmentEdit.SqlStatement = "Update [dbo].[tbl_Preplanning_Eng_Inventory] \r\n" +
                                               "set EquiptmentType = '" + cmbEquipmentType.SelectedItem.ToString() + "',MotorNumber = '" + txtMotorNumber.Text + "',Size = '" + txtSize.Text + "',Manufacturer = '" + cmbManufacturer.SelectedItem.ToString() + "' \r\n" +
                                               "where ID = '" + EditID + "' ";
            }

            _dbManEngEquipmentEdit.ExecuteInstruction();


            cmbEquipmentType.SelectedIndex = -1;
            txtMotorNumber.Text = string.Empty;
            txtSize.Text = string.Empty;
            cmbManufacturer.SelectedIndex = 0;

            LoadEngEquipment();

            //Close();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gvEngInv_DoubleClick(object sender, EventArgs e)
        {
            string ID = gvEngInv.GetRowCellValue(gvEngInv.FocusedRowHandle, gvEngInv.Columns["ID"]).ToString();

            string EquipmentType = gvEngInv.GetRowCellValue(gvEngInv.FocusedRowHandle, gvEngInv.Columns["EquiptmentType"]).ToString();
            string EquipmentNum = gvEngInv.GetRowCellValue(gvEngInv.FocusedRowHandle, gvEngInv.Columns["MotorNumber"]).ToString();

            if (MessageBox.Show("Are you sure you want to delete the '" + EquipmentType + "' with MotorNumber " + EquipmentNum + "  ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MWDataManager.clsDataAccess _dbManEngEquipmentEdit = new MWDataManager.clsDataAccess();
                _dbManEngEquipmentEdit.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
                _dbManEngEquipmentEdit.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManEngEquipmentEdit.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManEngEquipmentEdit.SqlStatement = " delete from [dbo].[tbl_Preplanning_Eng_Inventory] where ID = '" + ID + "' ";

                _dbManEngEquipmentEdit.ExecuteInstruction();
            }

            LoadEngEquipment();
        }
    }
}