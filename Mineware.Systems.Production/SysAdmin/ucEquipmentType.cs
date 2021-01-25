using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class ucEquipmentType : BaseUserControl
    {
        private string EquipmentName;
        private string EquipmentID;
        public ucEquipmentType()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpEquipment);
            FormActiveRibbonPage = rpEquipment;
            FormMainRibbonPage = rpEquipment;
            RibbonControl = rcWorkplace;
        }

        private void ucStopWorkplace_Load(object sender, EventArgs e)
        {
            LoadEquipment();
        }

        void LoadEquipment()
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan1.SqlStatement = " select * from tbl_Equipement";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable dtWp = _dbMan1.ResultsDataTable;
            gcEquipmentType.DataSource = dtWp;

            colEquipID.FieldName = "EquipmentID";
            colEquipName.FieldName = "EquipmentName";
            colEquipType.FieldName = "EquipmentType";
            colEquipMake.FieldName = "EquipmentMake";
            colEquipValid.FieldName = "EquipmentValid";         


        }

        private void barActivity_EditValueChanged(object sender, EventArgs e)
        {
          
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmEquipmentProperties frm = new frmEquipmentProperties();
            frm._ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            frm.Edit = "N";
            frm.ShowDialog();

            LoadEquipment();
        }

        private void gvWorkplaces_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                EquipmentName = gvEquipmentType.GetRowCellValue(gvEquipmentType.FocusedRowHandle, gvEquipmentType.Columns["EquipmentName"]).ToString();
                EquipmentID = gvEquipmentType.GetRowCellValue(gvEquipmentType.FocusedRowHandle, gvEquipmentType.Columns["EquipmentID"]).ToString();
                EditBtn.Enabled = true;
            }
            catch { return; }
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void EditBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmEquipmentProperties frm = new frmEquipmentProperties();
            frm._ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            
            frm.EquipmentID = EquipmentID;
            frm.Edit = "Y";
            frm.ShowDialog();

            LoadEquipment();
        }
    }
}
