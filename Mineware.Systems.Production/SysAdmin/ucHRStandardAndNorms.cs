using DevExpress.XtraEditors;
using Mineware.Systems.Global;
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

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class ucHRStandardAndNorms : BaseUserControl
    {
        string SelectedActivity;
        string SelectedGroup;
        string SelectedDesignation;
        public ucHRStandardAndNorms()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpHR);
            FormActiveRibbonPage = rpHR;
            FormMainRibbonPage = rpHR;
            RibbonControl = rcHR;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmHR frm = new frmHR();
            frm.ShowDialog();
            LoadHRGroups();

        }

        void LoadHRGroups()
        {

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

            _dbMan.SqlStatement = " Select g.*, a.Description from tbl_HR_MethodGroups g inner join tbl_Code_Activity a on g.Activity = a.Activity  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            
            gcMethodGroups.DataSource = _dbMan.ResultsDataTable;

            gcActivity.FieldName = "Description";
            gcGroupName.FieldName = "GroupName";

        }

        void LoadDesignations()
        {

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

            _dbMan.SqlStatement = " select * from tbl_HR_Desigantion where Activity = '" + SelectedActivity + "' and GroupName = '" + SelectedGroup + "'  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            gcDesignation.DataSource = _dbMan.ResultsDataTable;

            colDesignation.FieldName = "Designation";
            colCritSkill.FieldName = "CriticalSkill";

            colInService.FieldName = "InService";
            colDay.FieldName = "DayShift";
            colAfternoon.FieldName = "AfternoonShift";
            colNight.FieldName = "NightShift";
            colRoving.FieldName = "RovingShift";

        }

       

        private void ucHRStandardAndNorms_Load(object sender, EventArgs e)
        {
            LoadHRGroups();
            LoadOccupations();
        }


        void LoadOccupations()
        {

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

            _dbMan.SqlStatement = " select  Distinct WorkGroupCode  from  tbl_EmployeeAll where GangNo in (Select Distinct Substring(ORGUNITDS,0,13) from tbl_Planmonth) ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            foreach (DataRow dr in dt.Rows)
            {
                cbxDesignation.Properties.Items.Add(dr["WorkGroupCode"].ToString());
            }

        }

        private void btnAddDesignation_Click(object sender, EventArgs e)
        {
            if (SelectedActivity != String.Empty)
            {
                SelectedDesignation = cbxDesignation.Text;
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                _dbMan.SqlStatement = " insert into tbl_HR_Desigantion values ('" + SelectedActivity + "','" + SelectedGroup + "','" + SelectedDesignation + "', 'N', 0,0,0,0,0 )  ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                
                LoadDesignations();
            }

            
        }

        private void gvMethodGroups_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            SelectedActivity = gvMethodGroups.GetRowCellValue(gvMethodGroups.FocusedRowHandle, gvMethodGroups.Columns["Description"].FieldName).ToString();
            SelectedGroup = gvMethodGroups.GetRowCellValue(gvMethodGroups.FocusedRowHandle, gvMethodGroups.Columns["GroupName"].FieldName).ToString();
            
            LoadDesignations();
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
           

        }

        private void repositoryItemCheckEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (gvMethodGroups.GetRowCellValue(gvMethodGroups.FocusedRowHandle, gvMethodGroups.Columns["Description"].FieldName).ToString() == "Y")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                _dbMan.SqlStatement = " update tbl_HR_Desigantion  \r\n" +
                    " set CriticalSkill = 'N'\r\n" +                   
                    "Where Activity = '" + SelectedActivity + "' and GroupName ='" + SelectedGroup + "' and Designation = '" + SelectedDesignation + "'";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
            }
            else
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                _dbMan.SqlStatement = " update tbl_HR_Desigantion  \r\n" +
                    " set CriticalSkill = 'Y'\r\n" +
                    "Where Activity = '" + SelectedActivity + "' and GroupName ='" + SelectedGroup + "' and Designation = '" + SelectedDesignation + "'";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
            }

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved.", Color.CornflowerBlue);
        }

        private void gvDesignation_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            SelectedDesignation = gvDesignation.GetRowCellValue(gvDesignation.FocusedRowHandle, gvDesignation.Columns["Designation"].FieldName).ToString();
           
            
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnDeleteDesignation_Click(object sender, EventArgs e)
        {
            if (SelectedActivity != String.Empty)            {
                
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                _dbMan.SqlStatement = " delete from tbl_HR_Desigantion where activity ='" + SelectedActivity + "' and GroupName = '" + SelectedGroup + "' and Designation ='" + SelectedDesignation + "' ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Record Deleted", "Record Deleted", Color.CornflowerBlue);

                LoadDesignations();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            frmHR frm = new frmHR();
            frm.txtActivity.EditValue = SelectedActivity;
            frm.txtGroupName.EditValue = SelectedGroup;
            frm.ShowDialog();
            LoadHRGroups();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan.SqlStatement = " delete from tbl_HR_MethodGroups where activity ='" + SelectedActivity + "' and GroupName = '" + SelectedGroup + "' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Record Deleted", "Record Deleted", Color.CornflowerBlue);
            LoadHRGroups();
        }

        private void btnSaved_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan.SqlStatement = " ";
            for (int i = 0; i < gvDesignation.RowCount; i++)
            {
                DataRow row = gvDesignation.GetDataRow(i);
                string Designation = row["Designation"].ToString();
                string InService = row["InService"].ToString();
                string DayShift = row["DayShift"].ToString();
                string AfternoonShift = row["AfternoonShift"].ToString();
                string NightShift = row["NightShift"].ToString();
                string RovingShift = row["RovingShift"].ToString();

                _dbMan.SqlStatement = _dbMan.SqlStatement + " update tbl_HR_Desigantion  \r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " set InService = '" + InService + "',\r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " DayShift = '" + DayShift + "',\r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " AfternoonShift = '" + AfternoonShift + "',\r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " NightShift = '" + NightShift + "',\r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " RovingShift = '" + RovingShift + "'\r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " Where Activity = '" + SelectedActivity + "' and GroupName ='" + SelectedGroup + "' and Designation = '" + Designation + "'";
            }
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved.", Color.CornflowerBlue);
        }
    }
}
