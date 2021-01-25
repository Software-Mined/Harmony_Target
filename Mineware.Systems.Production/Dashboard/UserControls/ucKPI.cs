using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Mineware.Systems.GlobalConnect;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.Dashboard.UserControls
{
    public partial class ucKPI : XtraUserControl
    {
        #region
        private DataTable dtGetData = new DataTable("dtGetData");
        private BackgroundWorker bgwDataKPI = new BackgroundWorker();
        private string Mine = string.Empty;
        private string ProdMonth = string.Empty;
        private string PrevProdMonth = string.Empty;
        private string CurrProdMonth = string.Empty;
        private string Date = string.Empty;
        private GridView CurrentView { get { return gvKPI; } }
        private GridControl CurrentGrid { get { return gcKPI; } }
        //string LevelName = Properties.Resources.SM;
        #endregion
        public ucKPI()
        {
            InitializeComponent();
            bgwDataKPI.RunWorkerCompleted += bgwDataKPI_RunWorkerCompleted;
        }

        private void bgwDataKPI_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gcKPI.DataSource = dtGetData;
            gvKPI.ExpandAllGroups();
            //gvKPI..Vis;
            gbStopePrevMonth.Caption = PrevProdMonth;
            gbDevPrevMonth.Caption = PrevProdMonth;
            gbStopeCurrMonth.Caption = CurrProdMonth;
            gbDevCurrMonth.Caption = CurrProdMonth;

            colSectionMine.FieldName = "Mine";
            colSectionUM.FieldName = "UMName";
            colSectionMO.FieldName = "MOSec";
            colShiftNo.FieldName = "ShiftDay";
            colShiftTot.FieldName = "TotalShifts";
            //Stoping
            colStopeDailyPlan.FieldName = "DayPlanSqm";
            colStopeDailyAct.FieldName = "DayBookSqm";
            colStopeDailyVar.FieldName = "DayVarSqm";
            colStopePrevPlan.FieldName = "MonthPlanSqmPrev";
            colStopePrevAct.FieldName = "ProgBookSqmPrev";
            colStopePrevVar.FieldName = "ProgVarSqmPrev";
            colStopeProgPlan.FieldName = "ProgPlanSqm";
            colStopeProgAct.FieldName = "ProgBookSqm";
            colStopeProgVar.FieldName = "ProgVarSqm";
            //Development
            colDevDailyPlan.FieldName = "DayPlanAdv";
            colDevDailyAct.FieldName = "DayBookAdv";
            colDevDailyVar.FieldName = "DayVarAdv";
            colDevPrevPlan.FieldName = "MonthPlanAdvPrev";
            colDevPrevAct.FieldName = "ProgBookAdvPrev";
            colDevPrevVar.FieldName = "ProgVarAdvPrev";
            colDevProgPlan.FieldName = "ProgPlanAdv";
            colDevProgAct.FieldName = "ProgBookAdv";
            colDevProgVar.FieldName = "ProgVarAdv";
            this.gcKPI.Enabled = true;
            //bgwDataKPI.CancelAsync();
        }

        private void LoadKPIData()
        {
            string Section = "";
            string Profile = "";
            ////Get Department
            MWDataManager.clsDataAccess _dbUser = new MWDataManager.clsDataAccess();
            _dbUser.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            if (TUserInfo.Site == "Target")
            {
                _dbUser.SqlStatement = "select p.UserID, p.DepartmentID, Description, l.ProfileID, u.Section \r\n " +
                                    " from Syncromine_new.dbo.tblUsers p \r\n " +
                                    " inner join Syncromine_new.dbo.tblDepartments d on p.DepartmentID = d.DepartmentID \r\n " +
                                    " inner join Syncromine_new.dbo.tblUserProfileLink l on p.USERID = l.UserID \r\n " +
                                    " inner join PAS_TGT_Syncromine_New.dbo.tbl_Users_Synchromine u on p.USERID = u.UserID \r\n " +
                                    " where p.UserID = '" + TUserInfo.UserID + "'";
            }
            else 
            { 
            _dbUser.SqlStatement = "select p.UserID, p.DepartmentID, Description, l.ProfileID, u.Section \r\n " +
                                    " from [Syncromine_New].[dbo].tblUsers p \r\n " +
                                    " inner join [Syncromine_New].[dbo].tblDepartments d on p.DepartmentID = d.DepartmentID \r\n " +
                                    " inner join [Syncromine_New].[dbo].tblUserProfileLink l on p.USERID = l.UserID \r\n " +
                                    " inner join  tbl_Users_Synchromine u on p.USERID = u.UserID \r\n " +
                                    " where p.UserID = '" + TUserInfo.UserID + "'"; 
              }
            _dbUser.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbUser.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbUser.ExecuteInstruction();
            if (_dbUser.ResultsDataTable.Rows.Count > 0)
            {
                Section = _dbUser.ResultsDataTable.Rows[0]["Section"].ToString();
                Profile = _dbUser.ResultsDataTable.Rows[0]["ProfileID"].ToString();
            }

            if ((Profile == "MO" || Profile == "SO") && Section != "Total Mine")
            {
                ////Parameters for Stored procedure
                MWDataManager.clsDataAccess _dbParam = new MWDataManager.clsDataAccess();
                _dbParam.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbParam.SqlStatement = @"select Banner Mine, CurrentProductionMonth-1 PrevProdmonth, CurrentProductionMonth Prodmonth, Convert(Varchar,GetDate(),23) RunDate from [dbo].[tbl_SysSet] a, (select max(prodmonth) PrevProdmonth from tbl_Planning where prodmonth <> (select CurrentProductionMonth  from [dbo].[tbl_SysSet]))b";
                _dbParam.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbParam.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbParam.ExecuteInstruction();

                Mine = _dbParam.ResultsDataTable.Rows[0]["Mine"].ToString();
                ProdMonth = _dbParam.ResultsDataTable.Rows[0]["Prodmonth"].ToString();
                PrevProdMonth = _dbParam.ResultsDataTable.Rows[0]["PrevProdmonth"].ToString();
                CurrProdMonth = _dbParam.ResultsDataTable.Rows[0]["Prodmonth"].ToString();
                Date = _dbParam.ResultsDataTable.Rows[0]["RunDate"].ToString();

                MWDataManager.clsDataAccess _dbKPI = new MWDataManager.clsDataAccess();
                _dbKPI.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbKPI.SqlStatement = @" EXEC [KPI_Dashboard_MO] '" + ProdMonth + "','" + PrevProdMonth + "','" + Convert.ToDateTime(Date) + "','" + Mine + "','" + Section + "'";
                _dbKPI.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbKPI.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbKPI.ExecuteInstruction();


                try { dtGetData = _dbKPI.ResultsDataTable; }
                catch { return; }

            }
            else
            {

                ////Parameters for Stored procedure
                MWDataManager.clsDataAccess _dbParam = new MWDataManager.clsDataAccess();
                _dbParam.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbParam.SqlStatement = @"select Banner Mine, CurrentProductionMonth-1 PrevProdmonth, CurrentProductionMonth Prodmonth, Convert(Varchar,GetDate(),23) RunDate from [dbo].[tbl_SysSet] a, (select max(prodmonth) PrevProdmonth from tbl_Planning where prodmonth <> (select CurrentProductionMonth  from [dbo].[tbl_SysSet]))b";
                _dbParam.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbParam.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbParam.ExecuteInstruction();

                Mine = _dbParam.ResultsDataTable.Rows[0]["Mine"].ToString();
                ProdMonth = _dbParam.ResultsDataTable.Rows[0]["Prodmonth"].ToString();
                PrevProdMonth = _dbParam.ResultsDataTable.Rows[0]["PrevProdmonth"].ToString();
                CurrProdMonth = _dbParam.ResultsDataTable.Rows[0]["Prodmonth"].ToString();
                Date = _dbParam.ResultsDataTable.Rows[0]["RunDate"].ToString();

                MWDataManager.clsDataAccess _dbKPI = new MWDataManager.clsDataAccess();
                _dbKPI.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbKPI.SqlStatement = @" EXEC KPI_Dashboard '" + ProdMonth + "','" + PrevProdMonth + "','" + Convert.ToDateTime(Date) + "','" + Mine + "'";
                _dbKPI.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbKPI.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbKPI.ExecuteInstruction();


                try { dtGetData = _dbKPI.ResultsDataTable; }
                catch { return; }
            }



        }

        private void ucKPI_Load(object sender, EventArgs e)
        {
            bgwDataKPI.RunWorkerAsync();
            this.gcKPI.Enabled = false;
            LoadKPIData();

        }

        private void gvKPI_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridGroupRowInfo row = e.Info as GridGroupRowInfo;

            if (row.Column.FieldName == "UMName")
            {
                string displayText = row.EditValue.ToString();
                row.GroupText = displayText;

            }
        }

        private void gvKPI_GroupLevelStyle(object sender, GroupLevelStyleEventArgs e)
        {

        }
    }
}
