using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class ucSDB_Activities : BaseUserControl
    {
        public ucSDB_Activities()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpSDB_Activities);
            FormActiveRibbonPage = rpSDB_Activities;
            FormMainRibbonPage = rpSDB_Activities;
            RibbonControl = rcSDB_Activities;
        }

        #region private variables
        String SubEditFlag = "False";
        int SubRowHandle = -1;
        object bandview1;
        #endregion

        private void ucSDB_Activities_Load(object sender, EventArgs e)
        {
            LoadSchedule();
        }

        private void LoadSchedule()
        {
            MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
            _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);

            _dbManMain.SqlStatement = _dbManMain.SqlStatement + " Select mainactid mainactidaa,Description from tbl_SDB_Activity order by Description ";

            _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMain.ExecuteInstruction();

            DataTable dt = _dbManMain.ResultsDataTable;

            MWDataManager.clsDataAccess _dbManMainAct = new MWDataManager.clsDataAccess();
            _dbManMainAct.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbManMainAct.SqlStatement = _dbManMainAct.SqlStatement + " Select *,isnull(ProceedsID,'') ProceedsID2beused from vw_SDB_Mapping order by MainActDescription,starton, order1";
            _dbManMainAct.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMainAct.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMainAct.ExecuteInstruction();

            DataTable dtMain = _dbManMainAct.ResultsDataTable;
            DataSet ds = new DataSet();

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.ColumnCount = 7;

            dataGridView1.Columns[0].HeaderText = "MainActid";
            dataGridView1.Columns[1].HeaderText = "SubActid";
            dataGridView1.Columns[2].HeaderText = "Starton";
            dataGridView1.Columns[3].HeaderText = "duration";
            dataGridView1.Columns[4].HeaderText = "endon";
            dataGridView1.Columns[5].HeaderText = "proceedson";
            dataGridView1.Columns[6].HeaderText = "proceedsonID";

            if (dtMain.Rows.Count > 0)
                dataGridView1.RowCount = dtMain.Rows.Count;

            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                //MainAid
                dataGridView1.Rows[i].Cells[0].Value = dtMain.Rows[i][0].ToString();
                //SubID
                dataGridView1.Rows[i].Cells[1].Value = dtMain.Rows[i][2].ToString();
                //Starton
                dataGridView1.Rows[i].Cells[2].Value = dtMain.Rows[i][4].ToString();
                //Duration
                dataGridView1.Rows[i].Cells[3].Value = dtMain.Rows[i][5].ToString();
                //Endon
                dataGridView1.Rows[i].Cells[4].Value = dtMain.Rows[i][6].ToString();
                //Proceedson
                dataGridView1.Rows[i].Cells[5].Value = dtMain.Rows[i][7].ToString();
                //Proceedson
                dataGridView1.Rows[i].Cells[6].Value = dtMain.Rows[i][39].ToString();
            }

            ds.Tables.Add(dt);
            ds.Tables.Add(dtMain);

            ds.Relations.Clear();

            DataColumn keyColumn1 = ds.Tables[0].Columns[0];
            DataColumn foreignKeyColumn1 = ds.Tables[1].Columns[0];
            ds.Relations.Add("ActivityDetail", keyColumn1, foreignKeyColumn1);

            gcActivity.DataSource = ds.Tables[0];
            gcActivity.LevelTree.Nodes.Add("ActivityDetail", bandedGridView2);

            gcActivity.DataSource = ds.Tables[0];

            colMainActivity.FieldName = "Description";
            colMainID.FieldName = "mainactidaa";

            colOrder.FieldName = "order1";

            colStart.FieldName = "starton";
            colDuration.FieldName = "duration";
            colProceeds.FieldName = "proceedson";
            colDay1.FieldName = "day1";
            colDay2.FieldName = "day2";
            colDay3.FieldName = "day3";
            colDay4.FieldName = "day4";
            colDay5.FieldName = "day5";
            colDay6.FieldName = "day6";
            colDay7.FieldName = "day7";
            colDay8.FieldName = "day8";
            colDay9.FieldName = "day9";
            colDay10.FieldName = "day10";
            colDay11.FieldName = "day11";
            colDay12.FieldName = "day12";
            colDay13.FieldName = "day13";
            colDay14.FieldName = "day14";
            colDay15.FieldName = "day15";
            colDay16.FieldName = "day16";
            colDay17.FieldName = "day17";
            colDay18.FieldName = "day18";
            colDay19.FieldName = "day19";
            colDay20.FieldName = "day20";
            colDay21.FieldName = "day21";
            colDay22.FieldName = "day22";
            colDay23.FieldName = "day23";
            colDay24.FieldName = "day24";
            colDay25.FieldName = "day25";
            colDay26.FieldName = "day26";
            colDay27.FieldName = "day27";
            colDay28.FieldName = "day28";
            colDay29.FieldName = "day29";
            colDay20.FieldName = "day30";

            colSubActDescription.FieldName = "SubActDescription";
            colSubID.FieldName = "subactid";

            gvActivity.ExpandAllGroups();
        }

        private void barbtnMapTask_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Check if User Has rights for SDB Bookings
            MWDataManager.clsDataAccess _dbManRights = new MWDataManager.clsDataAccess();
            _dbManRights.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManRights.SqlStatement = " select isnull(SDB_MapBlueprint,0) SDB_MapBlueprint from tbl_Users_Synchromine where UserID =  '" + TUserInfo.UserID + "' ";
            _dbManRights.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRights.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRights.ExecuteInstruction();
            if (_dbManRights.ResultsDataTable.Rows.Count > 0)
            {
                if (_dbManRights.ResultsDataTable.Rows[0][0].ToString() == "0")
                {
                    MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                    return;
                }
            }

            frmScheduleActivity Serv1Frm = new frmScheduleActivity();
            Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            Serv1Frm.lblMainAct = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["Description"]).ToString();
            Serv1Frm.lblMainActID = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["mainactidaa"]).ToString();
            Serv1Frm.lblFrmType = "Add";
            Serv1Frm.lblDay = gvActivity.FocusedColumn.FieldName.ToString();
            Serv1Frm.ShowDialog();

            int focusedRow = gvActivity.FocusedRowHandle;
            int focusedRow2 = bandedGridView2.FocusedRowHandle;

            gvActivity.BeginDataUpdate();
            bandedGridView2.BeginDataUpdate();
            LoadSchedule();
            gvActivity.EndDataUpdate();
            bandedGridView2.EndDataUpdate();

            gvActivity.FocusedRowHandle = focusedRow;
            bandedGridView2.FocusedRowHandle = focusedRow2;
            gvActivity.SetMasterRowExpanded(focusedRow, !gvActivity.GetMasterRowExpanded(focusedRow));
            bandedGridView2.SetMasterRowExpanded(focusedRow2, !bandedGridView2.GetMasterRowExpanded(focusedRow2));

            Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);
        }

        private void barbtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbManRights = new MWDataManager.clsDataAccess();
            _dbManRights.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManRights.SqlStatement = " select isnull(SDB_MapBlueprint,0) SDB_MapBlueprint from tbl_Users_Synchromine where UserID =  '" + TUserInfo.UserID + "' ";
            _dbManRights.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRights.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRights.ExecuteInstruction();
            if (_dbManRights.ResultsDataTable.Rows.Count > 0)
            {
                if (_dbManRights.ResultsDataTable.Rows[0][0].ToString() == "0")
                {
                    MessageBox.Show("You don't Have rights to Map Blue Print,Please Contact you Administrator");
                    return;
                }
            }

            if (SubRowHandle < 0)
            {
                MessageBox.Show("Please select a task to edit");
                return;
            }

            string mainactid = string.Empty;

            GridView View1 = bandview1 as GridView;

            if (View1.GetRowCellValue(View1.FocusedRowHandle, View1.FocusedColumn) == null)
            {
                MessageBox.Show("No Record Found");
                return;
            }

            frmScheduleActivity Serv1Frm = new frmScheduleActivity();
            Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            Serv1Frm.lblMainAct = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["Description"]).ToString();
            Serv1Frm.lblMainActID = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["mainactidaa"]).ToString();
            mainactid = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["mainactidaa"]).ToString();
            Serv1Frm.OrigSSlabel = View1.GetRowCellValue(View1.FocusedRowHandle, View1.Columns["starton"]).ToString();
            Serv1Frm.lblFrmType = "Edit";
            Serv1Frm.lblProceedson = View1.GetRowCellValue(View1.FocusedRowHandle, View1.Columns["proceedson"]).ToString();
            Serv1Frm.lblSubAct = View1.GetRowCellValue(View1.FocusedRowHandle, View1.Columns["SubActDescription"]).ToString();
            Serv1Frm.ShowDialog();

            int focusedRow = gvActivity.FocusedRowHandle;
            int focusedRow2 = bandedGridView2.FocusedRowHandle;

            gvActivity.BeginDataUpdate();
            bandedGridView2.BeginDataUpdate();
            LoadSchedule();
            gvActivity.EndDataUpdate();
            bandedGridView2.EndDataUpdate();

            gvActivity.FocusedRowHandle = focusedRow;
            bandedGridView2.FocusedRowHandle = focusedRow2;
            gvActivity.SetMasterRowExpanded(focusedRow, !gvActivity.GetMasterRowExpanded(focusedRow));
            bandedGridView2.SetMasterRowExpanded(focusedRow2, !bandedGridView2.GetMasterRowExpanded(focusedRow2));

            Global.sysNotification.TsysNotification.showNotification("Data Edited", "Record Edited Succesfully", Color.CornflowerBlue);

            ReOrderSubact(mainactid);
        }

        private void ReOrderSubact(String _mainactid)
        {
            LoadSchedule();
        }

        private void gvActivity_DoubleClick(object sender, EventArgs e)
        {
            if (!Security.SecurityPAS.HasPermissionSDB(Security.SDBPermissions.MapBlueprint))
            {
                MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                return;
            }

            if (SubEditFlag == "True")
            {
                SubEditFlag = "False";
                return;
            }

            if (gvActivity.FocusedRowHandle < 0)
                return;

            frmScheduleActivity Serv1Frm = new frmScheduleActivity();
            Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            Serv1Frm.lblMainAct = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["Description"]).ToString();
            Serv1Frm.lblMainActID = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["mainactidaa"]).ToString();

            Serv1Frm.lblFrmType = "Add";
            Serv1Frm.lblDay = gvActivity.FocusedColumn.FieldName.ToString();
            Serv1Frm.ShowDialog();

            int focusedRow = gvActivity.FocusedRowHandle;
            int focusedRow2 = bandedGridView2.FocusedRowHandle;

            gvActivity.BeginDataUpdate();
            bandedGridView2.BeginDataUpdate();
            LoadSchedule();
            gvActivity.EndDataUpdate();
            bandedGridView2.EndDataUpdate();

            gvActivity.FocusedRowHandle = focusedRow;
            bandedGridView2.FocusedRowHandle = focusedRow2;
            gvActivity.SetMasterRowExpanded(focusedRow, !gvActivity.GetMasterRowExpanded(focusedRow));
            bandedGridView2.SetMasterRowExpanded(focusedRow2, !bandedGridView2.GetMasterRowExpanded(focusedRow2));

            Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);
        }

        private void bandedGridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView View1 = sender as GridView;

            string ss = string.Empty;

            if (View1.GetRowCellValue(e.RowHandle, e.Column) != null)
            {
                ss = View1.GetRowCellValue(e.RowHandle, e.Column).ToString();
            }

            if (ss == "Y")
            {
                e.Appearance.BackColor = Color.Tomato;
                e.Appearance.ForeColor = Color.Tomato;
            }
        }

        private void bandedGridView2_DoubleClick(object sender, EventArgs e)
        {
            if (!Security.SecurityPAS.HasPermissionSDB(Security.SDBPermissions.MapBlueprint))
            {
                MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                return;
            }

            SubEditFlag = "True";

            string mainactid = string.Empty;

            GridView View1 = sender as GridView;


            if (View1.GetRowCellValue(View1.FocusedRowHandle, View1.FocusedColumn) == null)
            {
                MessageBox.Show("No Record Found");
                return;
            }

            frmScheduleActivity Serv1Frm = new frmScheduleActivity();
            Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            Serv1Frm.lblMainAct = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["Description"]).ToString();
            Serv1Frm.lblMainActID = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["mainactidaa"]).ToString();
            mainactid = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["mainactidaa"]).ToString();

            Serv1Frm.lblFrmType = "Edit";
            Serv1Frm.lblProceedson = View1.GetRowCellValue(View1.FocusedRowHandle, View1.Columns["proceedson"]).ToString();
            Serv1Frm.lblSubAct = View1.GetRowCellValue(View1.FocusedRowHandle, View1.Columns["SubActDescription"]).ToString();
            Serv1Frm.lblPrevStarton = View1.GetRowCellValue(View1.FocusedRowHandle, View1.Columns["starton"]).ToString();
            Serv1Frm.OrigSSlabel = View1.GetRowCellValue(View1.FocusedRowHandle, View1.Columns["starton"]).ToString();
            Serv1Frm.ShowDialog();

            int focusedRow = gvActivity.FocusedRowHandle;
            int focusedRow2 = bandedGridView2.FocusedRowHandle;

            gvActivity.BeginDataUpdate();
            bandedGridView2.BeginDataUpdate();
            LoadSchedule();
            gvActivity.EndDataUpdate();
            bandedGridView2.EndDataUpdate();

            gvActivity.FocusedRowHandle = focusedRow;
            bandedGridView2.FocusedRowHandle = focusedRow2;
            gvActivity.SetMasterRowExpanded(focusedRow, !gvActivity.GetMasterRowExpanded(focusedRow));
            bandedGridView2.SetMasterRowExpanded(focusedRow2, !bandedGridView2.GetMasterRowExpanded(focusedRow2));

            Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);

            ReOrderSubact(mainactid);
            LoadSchedule();
        }

        private void bandedGridView2_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            SubRowHandle = e.RowHandle;
            bandview1 = sender;
        }

        private void barbtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barHeaderItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
