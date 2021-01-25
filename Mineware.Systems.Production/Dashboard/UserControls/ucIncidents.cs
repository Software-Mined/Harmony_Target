using DevExpress.XtraEditors;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.Production.Bookings;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace Mineware.Systems.Production.Dashboard.UserControls
{
    public partial class ucIncidents : XtraUserControl
    {
        private DataTable dtGetData = new DataTable("dtGetData");
        private BackgroundWorker bgwData = new BackgroundWorker();

        public ucIncidents()
        {
            InitializeComponent();
            bgwData.RunWorkerCompleted += bgwIncidents_RunWorkerCompleted;
        }


        private void bgwIncidents_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gcActions.DataSource = null;
            gcActions.DataSource = dtGetData;
            colSection.FieldName = "Section";
            colActionDate.FieldName = "ActionDate";
            colActionTitle.FieldName = "Action_Title";
            colRespPerson.FieldName = "Responsible_Person";
            colActionID.FieldName = "ActionID";
            colHazard.FieldName = "Hazard";
            colDaysOpen.FieldName = "DaysOpen";
            colActionType.FieldName = "Type";
            colPivotID.FieldName = "Enablon_Action_ID";
            colRequestedForClosed.FieldName = "RequestStatus";
            colFeedBack.FieldName = "Responsible_Person_Feedback";
            colWorkplace.FieldName = "Workplace";
            this.gcActions.Enabled = true;
        }


        private void LoadIncidentsData()
        {
            bgwData.RunWorkerAsync();
            this.gcActions.Enabled = false;
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbAction.SqlStatement = @"SELECT i.*, DATEDIFF(DAY, [ActionDate], GETDATE()) AS[DaysOpen], ISNULL([Mineware_Action_ID], '')  AS [ActionID]
                                        ,CASE WHEN[Application_Origin] LIKE 'Pivot%' THEN 'Pivot Action' ELSE 'Normal Action' END AS[App_Origin] , 
                                        case when s.Type = 'PPS' then 'Safety Department'                                       
                                        when s.Type = 'PPRE' then 'Rock Engineering Department'
                                        when s.Type = 'PPVT' then 'Ventillation Department'
                                        when s.Type = 'PPSR' then 'Survey Department'
                                        when s.Type = 'PPGL' then 'Geology Department'
                                        when s.Type = 'PPEG' then 'Engineering Department'
                                        when s.Type = 'SUARE' then 'Rock Engineering Department'
                                        when s.Type = 'SUAS' then 'Survey Department'
                                        when s.Type = 'SUAG' then 'Geology Department'
                                        when s.Type is null then 'OCR Scanning Solution'
                                        else Type end as Type
                                        FROM[dbo].[tbl_Incidents] i
                                        left outer join tbl_Shec_Incidents s on i.Mineware_Action_ID = Convert(varchar(20),s.ID)
                                        where([Action_Status] = 'Open' OR[Action_Status] = 'New Action')";                                          
            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();
            try { dtGetData = _dbAction.ResultsDataTable; }
            catch { return; }
        }

        private void ucIncidents_Load(object sender, EventArgs e)
        {
            LoadIncidentsData();
        }



        private void gvActions_DoubleClick(object sender, EventArgs e)
        {
            string RespPerson = gvActions.GetRowCellValue(gvActions.FocusedRowHandle, gvActions.Columns["Responsible_Person"]).ToString().TrimStart();
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbAction.SqlStatement = @"select Name + ' ' + LastName UserName from [Syncromine_New].[dbo].tblUsers Where UserID = '"+TUserInfo.UserID+"'";
            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();
            string UserName = _dbAction.ResultsDataTable.Rows[0][0].ToString();
            if (RespPerson != UserName)
            {
                MessageBox.Show("Cant close incidents that are not assigned to you.");
                return;
            }
            else
            {

                string workplace = gvActions.GetRowCellValue(gvActions.FocusedRowHandle, gvActions.Columns["Workplace"]).ToString().TrimStart();

                var tmp = sender as DevExpress.XtraGrid.Views.BandedGrid.BandedGridView;
                var row = tmp.GetFocusedRow() as System.Data.DataRowView;
                frmResponciblePersonFeedback frm = new frmResponciblePersonFeedback();
                frm.ActionDesc = "Action: " + gvActions.GetRowCellValue(gvActions.FocusedRowHandle, gvActions.Columns["ActionID"]).ToString();    //row.Row[0].ToString();
                frm.ActionId = row.Row[3].ToString();
                frm.Workplace = workplace;
                frm.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                frm.ShowDialog(this);

                //Reload the incidents
                LoadIncidentsData();
            }
        }


    }
}
