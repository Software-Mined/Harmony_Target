using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using DevExpress.XtraGrid.Views.Grid;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class ucEmailSetup : BaseUserControl
    {
        public ucEmailSetup()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpEmailSetup);
            FormActiveRibbonPage = rpEmailSetup;
            FormMainRibbonPage = rpEmailSetup;
            RibbonControl = rcEmailSetup;
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void ucEmailSetup_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManGetISAfterStart = new MWDataManager.clsDataAccess();
            _dbManGetISAfterStart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetISAfterStart.SqlStatement = "SELECT EmailGroup FROM [tbl_EmailGroups] ";

            _dbManGetISAfterStart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetISAfterStart.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetISAfterStart.ExecuteInstruction();

            LookUpEditEmail.DataSource = _dbManGetISAfterStart.ResultsDataTable;
            LookUpEditEmail.DisplayMember = "EmailGroup";
            LookUpEditEmail.ValueMember = "EmailGroup";
        }

        private void editEmail_EditValueChanged(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " \r\n" +
                              " select a.*, \r\n" +
                                " case when b.EmailUser is null then 'N' else 'Y' end as RecieveEmail  from( \r\n" +
                                " select UserID, EMail, Name + ' ' + LastName FullName, d.DepartmentID, d.Description Department \r\n" +
                                " from [Syncromine_New].[dbo].tblUsers u \r\n" +
                                " inner  join [Syncromine_New].[dbo].tblDepartments d on u.DepartmentID = d.DepartmentID)a \r\n" +
                                " left outer join(select e.EmailGroup, e.EmailUser from [tbl_EmailLink] e \r\n" +
                                " left outer join [tbl_EmailGroups] eg on e.EmailGroup = eg.EmailGroup \r\n" +
                                " where e.EmailGroup = '"+ editEmail.EditValue + "') b \r\n" +
                                " on a.USERID = b.EmailUser ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            gcEmailSetup.DataSource = _dbMan.ResultsDataTable;

            colUserID.FieldName = "UserID";
            colName.FieldName = "FullName";
            colEmail.FieldName = "EMail";
            colDepartment.FieldName = "Department";
            colChecked.FieldName = "RecieveEmail";
            
        }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            var gv = gcEmailSetup.MainView as GridView;
            var index = gv.FocusedRowHandle;
            if (index > 0)
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " begin try  \r\n" +
                                  " insert into [tbl_EmailLink] values ( \r\n" +
                                  " '" + editEmail.EditValue + "', \r\n" +
                                  " '" + gvEmailSetup.GetRowCellValue(gvEmailSetup.FocusedRowHandle, gvEmailSetup.Columns["UserID"].FieldName).ToString() + "', \r\n" +
                                  " '" + gvEmailSetup.GetRowCellValue(gvEmailSetup.FocusedRowHandle, gvEmailSetup.Columns["Department"].FieldName).ToString() + "') \r\n" +
                                  " end try \r\n" +
                                  " begin catch  \r\n" +
                                  " delete from  [tbl_EmailLink] \r\n" +
                                  " where EmailUser = '" + gvEmailSetup.GetRowCellValue(gvEmailSetup.FocusedRowHandle, gvEmailSetup.Columns["UserID"].FieldName).ToString() + "' \r\n" +
                                  " and emailgroup = '" + editEmail.EditValue + "' \r\n" +
                                  " end catch ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
            }
        }
    }
}
