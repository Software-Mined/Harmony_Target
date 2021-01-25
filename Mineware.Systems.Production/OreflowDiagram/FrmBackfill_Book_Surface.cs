using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class FrmBackfill_Book_Surface : DevExpress.XtraEditors.XtraForm
    {
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;

        public FrmBackfill_Book_Surface()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmBackfill_Book_Surface_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "  select Name from [dbo].[tbl_OreFlowEntities] where OreFlowID = '" + Tanktxt.Text + "'  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            foreach (DataRow row in dt.Rows)
            {
                Tanktxt.Text = row["Name"].ToString();
            }

            MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();
            _dbManProb.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManProb.SqlStatement = " select * from [tbl_Code_Backfill_Problems] ";
            _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManProb.ExecuteInstruction();

            DataTable dtProb = _dbManProb.ResultsDataTable;
            DataSet dsProb = new DataSet();
            dsProb.Tables.Add(dtProb);

            foreach (DataRow row in dtProb.Rows)
            {
                cmbProblems.Items.Add(row["ProblemID"] + ":" + row["Description"]);
            }

            loadGrid();
            LoadProblemsGrid();
        }

        private void loadGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " select * from tbl_Backfill_Book_Surface where CalenderDate = '" + String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date.ToString()) + "'  order by BookTime  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            if (dt.Rows.Count > 0)
            {
                colDate.FieldName = "CalenderDate";
                colTank.FieldName = "Tank";
                colTime.FieldName = "BookTime";
                colTonnage.FieldName = "Tonnage";
                colLevel.FieldName = "Level";
                colRD.FieldName = "RD";
                colRetention.FieldName = "Retention";
            }

            dtBookSurface.DataSource = ds.Tables[0];
        }

        public void LoadProblemsGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " Select * from [dbo].[tbl_Backfill_ProblemsSurface] Where  CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date.ToString()) + "'     ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            if (dt.Rows.Count > 0)
            {
                colProbID.FieldName = "ProblemID";
                colProbDescription.FieldName = "Description";
                colProbFrom.FieldName = "TimeFrom";
                colProbTo.FieldName = "TimeTo";
                colProbNote.FieldName = "Notes";
            }

            ProblemsGrid.DataSource = ds.Tables[0];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string time = String.Format("{0:t}", BookTimePicker.Value.TimeOfDay.ToString());
            time = time.Substring(0, 5);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " Delete from tbl_Backfill_Book_Surface where Tank = '" + Tanktxt.Text + "' and CalenderDate = '" + String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date.ToString()) + "' and BookTime = '" + time + "'  \r\n";

            _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_Backfill_Book_Surface (Tank,CalenderDate,Level,Tonnage,RD,Retention,BookTime)  \r\n" +
                                  " values ( '" + Tanktxt.Text + "','" + String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date.ToString()) + "', '" + Leveltxt.Text + "'," + Tonnagetxt.Text + ",'" + RDtxt.Text + "', '" + Retentiontxt.Text + "' , '" + time + "' )   ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);

            this.Close();
        }

        private void AddProbbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbProblems.Text))
            {
                MessageBox.Show("Please choose a Problem Description before you try and add an problem");
                return;
            }

            string timeProbStart = String.Format("{0:t}", ProblemStartTime.Value.TimeOfDay.ToString());
            timeProbStart = timeProbStart.Substring(0, 5);

            string timeProbStop = String.Format("{0:t}", ProblemStopTime.Value.TimeOfDay.ToString());
            timeProbStop = timeProbStop.Substring(0, 5);

            frmBackfillNotes Backfillfrm = new frmBackfillNotes();
            Backfillfrm.StartPosition = FormStartPosition.CenterScreen;
            Backfillfrm._Description = ProductionGlobal.ProductionGlobal.ExtractAfterColon(cmbProblems.Text);
            Backfillfrm._From = timeProbStart;
            Backfillfrm._To = timeProbStop;
            Backfillfrm._Date = String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date.ToString());
            Backfillfrm._FormType = "BookSurface";
            Backfillfrm._theSystemDBTag = _theSystemDBTag;
            Backfillfrm._UserCurrentInfoConnection = _UserCurrentInfoConnection;
            Backfillfrm.ShowDialog();

            LoadProblemsGrid();
        }

        private void addSurfBookingbtn_Click(object sender, EventArgs e)
        {
            string time = String.Format("{0:t}", BookTimePicker.Value.TimeOfDay.ToString());
            time = time.Substring(0, 5);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " Delete from tbl_Backfill_Book_Surface where Tank = '" + Tanktxt.Text + "' and CalenderDate = '" + String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date.ToString()) + "' and BookTime = '" + time + "'  \r\n";

            _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_Backfill_Book_Surface (Tank,CalenderDate,Level,Tonnage,RD,Retention,BookTime)  \r\n" +
                                  " values ( '" + Tanktxt.Text + "','" + String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date.ToString()) + "', '" + Leveltxt.Text + "'," + Tonnagetxt.Text + ",'" + RDtxt.Text + "', '" + Retentiontxt.Text + "' , '" + time + "' )   ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);

            loadGrid();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            string lblTank = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            string lblTime = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            string lblCalendar = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();

            if (MessageBox.Show("Are you sure you want to delete record where the tank is " + lblTank + "?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = " Delete from tbl_Backfill_Book_Surface where Tank = '" + lblTank + "' and CalenderDate = '" + lblCalendar + "' and BookTime = '" + lblTime + "'  \r\n";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                loadGrid();
            }
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.RowCount > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete record where the Problem Description is '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["Description"]) + "' and time from is '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["TimeFrom"]) + "' ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                    _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbMan.SqlStatement = " Delete from tbl_Backfill_ProblemsSurface where ProblemID = '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["ProblemID"]) + "' \r\n " +
                                          " and TimeFrom = '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["TimeFrom"]) + "' and TimeTo = '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["TimeTo"]) + "'  \r\n";
                    _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan.ExecuteInstruction();

                    LoadProblemsGrid();
                }
                else
                {
                }
            }
        }

        private void DPdate_ValueChanged(object sender, EventArgs e)
        {
            loadGrid();
        }

        private void cmbProblems_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProblemsGrid();
        }
    }
}