using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class frmAddBackfillBooking : DevExpress.XtraEditors.XtraForm
    {
        #region Public variables
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        #endregion

        public frmAddBackfillBooking()
        {
            InitializeComponent();
        }

        private void frmAddBackfillBooking_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();
            _dbManProb.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManProb.SqlStatement = " select * from [dbo].[tbl_Code_Backfill_Problems] ";
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

            LoadWorkplaces();
            LoadProblemsGrid();
        }

        public void LoadWorkplaces()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "  select * from tbl_Backfill_Booking_RangeWorkplaces where Range = '" + lblRange.Text + "'  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            lbxPermaWorkplaces.Items.Clear();
            lbxTempWorkplaces.Items.Clear();

            foreach (DataRow row in dt.Rows)
            {
                if (row["TempPerm"].ToString() == "T")
                {
                    lbxTempWorkplaces.Items.Add(row["Workplace"].ToString());
                }

                if (row["TempPerm"].ToString() == "P")
                {
                    lbxPermaWorkplaces.Items.Add(row["Workplace"].ToString());
                }
            }
        }

        public void LoadProblemsGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " Select * from [dbo].[tbl_Backfill_Booking_Problems] Where Range = '" + lblRange.Text + "' and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString()) + "' and workplace = '" + lblClickedWp.Text + "'    ";
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
                colProbWorkplace.FieldName = "Workplace";
            }

            ProblemsGrid.DataSource = ds.Tables[0];
        }

        public void loadBookingGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "  select * from tbl_Backfill_Workplace_Bookings where Range = '" + lblRange.Text + "' and DateTime = '" + String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString()) + "'  and workplace = '" + lblClickedWp.Text + "'  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            if (dt.Rows.Count > 0)
            {
                colRange.FieldName = "Range";
                colWorkplace.FieldName = "Workplace";
                colCrewName.FieldName = "CrewName";
                colDateTime.FieldName = "DateTime";
                colPhoneNum.FieldName = "PhoneNum";
                colCompNum.FieldName = "CompanyNum";
                colPersonCalled.FieldName = "PersonCalled";
                colFlushStart.FieldName = "FlushStart";
                colFlushStop.FieldName = "FlushStop";
                colFlushStart2.FieldName = "FlushStart2";
                colFlushStop2.FieldName = "FlushStop2";
                colBackfillStart.FieldName = "BackfillStart";
                colBackfillStop.FieldName = "BackfillStop";
                colFlowrate.FieldName = "Flowrate";
            }

            BookingsGrid.DataSource = ds.Tables[0];
        }

        private void btnAddPermaWP_Click(object sender, EventArgs e)
        {
            frmAddBackfill_Workplaces Backfillfrm = new frmAddBackfill_Workplaces();
            Backfillfrm.StartPosition = FormStartPosition.CenterScreen;
            Backfillfrm._FormType = "Permanent";
            Backfillfrm._Range = lblRange.Text;
            Backfillfrm.StartPosition = FormStartPosition.CenterScreen;
            Backfillfrm._theSystemDBTag = _theSystemDBTag;
            Backfillfrm._UserCurrentInfoConnection = _UserCurrentInfoConnection;
            Backfillfrm.ShowDialog();

            LoadWorkplaces();
        }

        private void btnRemovePermaWP_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the workplace ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MWDataManager.clsDataAccess _dbManDtsub1 = new MWDataManager.clsDataAccess();
                _dbManDtsub1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManDtsub1.SqlStatement = "Delete from tbl_Backfill_Booking_RangeWorkplaces where Range = '" + lblRange.Text + "'  and Workplace = '" + lbxPermaWorkplaces.SelectedItem.ToString() + "'  and TempPerm = 'P' ";
                _dbManDtsub1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDtsub1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDtsub1.ExecuteInstruction();

                LoadWorkplaces();
            }
        }

        private void btnAddTempWP_Click(object sender, EventArgs e)
        {
            frmAddBackfill_Workplaces Backfillfrm = new frmAddBackfill_Workplaces();
            Backfillfrm.StartPosition = FormStartPosition.CenterScreen;
            Backfillfrm._FormType = "Temporary";
            Backfillfrm._Range = lblRange.Text;
            Backfillfrm.StartPosition = FormStartPosition.CenterScreen;
            Backfillfrm._theSystemDBTag = _theSystemDBTag;
            Backfillfrm._UserCurrentInfoConnection = _UserCurrentInfoConnection;
            Backfillfrm.ShowDialog();

            LoadWorkplaces();
        }

        private void RemoveTempbtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the workplace ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                MWDataManager.clsDataAccess _dbManDtsub1 = new MWDataManager.clsDataAccess();
                _dbManDtsub1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManDtsub1.SqlStatement = "Delete from tbl_Backfill_Booking_RangeWorkplaces where Range = '" + lblRange.Text + "'  and Workplace = '" + lbxTempWorkplaces.SelectedItem.ToString() + "'  and TempPerm = 'T' ";
                _dbManDtsub1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDtsub1.queryReturnType = MWDataManager.ReturnType.DataTable;
                var result = _dbManDtsub1.ExecuteInstruction();

                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace Remove", Color.CornflowerBlue);
                    LoadWorkplaces();
                }
            }
        }

        private void btnAddBooking_Click(object sender, EventArgs e)
        {
            string flowrate = string.Empty;

            string lblPersonCalled = string.Empty;
            string lblCrewName = string.Empty;

            if (PhoneNumtxt.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a Phone Number");
                return;
            }

            if (CompNumtxt.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a Company Number");
                return;
            }

            if (lbxPersonCalled.SelectedIndex != -1)
            {
                lblPersonCalled = lbxPersonCalled.SelectedItem.ToString();
            }

            if (lbxCrewName.SelectedIndex != -1)
            {
                lblCrewName = lbxCrewName.SelectedItem.ToString();
            }

            if (Flowratetxt.Text == "0.00")
            {
                MWDataManager.clsDataAccess _dbManaa = new MWDataManager.clsDataAccess();
                _dbManaa.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManaa.SqlStatement = " Select top (1) Flowrate From tbl_Backfill_Workplace_Bookings \r\n " +
                                        " where Range = '" + lblRange.Text + "' and workplace = '" + lblClickedWp.Text + "' and [DateTime] < '" + String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString()) + "' order by [DateTime] desc  ";
                _dbManaa.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManaa.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManaa.ExecuteInstruction();

                DataTable dt = _dbManaa.ResultsDataTable;

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        flowrate = dr["Flowrate"].ToString();
                    }
                }
                else
                {
                    flowrate = Flowratetxt.Text;
                }
            }
            else
            {
                flowrate = Flowratetxt.Text;
            }

            string timeFlushStart = String.Format("{0:t}", FlushingStartTime.Value.TimeOfDay.ToString());
            timeFlushStart = timeFlushStart.Substring(0, 5);

            string timeFlushStop = String.Format("{0:t}", FlushingStopTime.Value.TimeOfDay.ToString());
            timeFlushStop = timeFlushStop.Substring(0, 5);

            string timeBackfillStart = String.Format("{0:t}", BackfillStartTime.Value.TimeOfDay.ToString());
            timeBackfillStart = timeBackfillStart.Substring(0, 5);

            string timeBackfillStop = String.Format("{0:t}", BackfillStopTime.Value.TimeOfDay.ToString());
            timeBackfillStop = timeBackfillStop.Substring(0, 5);

            string timeFlushStart2 = String.Format("{0:t}", FlushingStart2Time.Value.TimeOfDay.ToString());
            timeFlushStart2 = timeFlushStart2.Substring(0, 5);

            string timeFlushStop2 = String.Format("{0:t}", FlushingStop2Time.Value.TimeOfDay.ToString());
            timeFlushStop2 = timeFlushStop2.Substring(0, 5);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_Backfill_Workplace_Bookings (DateTime,Range,Workplace,PersonCalled \r\n " +
                                  ",CompanyNum,PhoneNum,CrewName,FlushStart,FlushStop,BackfillStart,BackfillStop,FlushStart2,FlushStop2 , Flowrate)  values \r\n " +
                                  "('" + String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString()) + "' ,'" + lblRange.Text + "' , '" + lblClickedWp.Text + "' ,  \r\n" +
                                  "  '" + lblPersonCalled + "' , '" + CompNumtxt.Text + "'  , '" + PhoneNumtxt.Text + "' , '" + lblCrewName + "' ,  \r\n" +
                                  "  '" + timeFlushStart + "' , '" + timeFlushStop + "' , '" + timeBackfillStart + "' , '" + timeBackfillStop + "' , '" + timeFlushStart2 + "'  , '" + timeFlushStop2 + "' ,  " +
                                  "  '" + flowrate + "'      ) ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            var result = _dbMan.ExecuteInstruction();

            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Problem Added", Color.CornflowerBlue);
                loadBookingGrid();
            }
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
            Backfillfrm._Range = lblRange.Text;
            Backfillfrm._Workplace = lblClickedWp.Text;
            Backfillfrm._Description = ProductionGlobal.ProductionGlobal.ExtractAfterColon(cmbProblems.Text);
            Backfillfrm._From = timeProbStart;
            Backfillfrm._To = timeProbStop;
            Backfillfrm._Date = String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString());
            Backfillfrm._FormType = "AddBooking";
            Backfillfrm._theSystemDBTag = _theSystemDBTag;
            Backfillfrm._UserCurrentInfoConnection = _UserCurrentInfoConnection;
            Backfillfrm.ShowDialog();

            LoadProblemsGrid();
        }

        private void lbxPermaWorkplaces_DoubleClick(object sender, EventArgs e)
        {
            if (lbxPermaWorkplaces.Items.Count > 0)
            {
                lblClickedWp.Text = lbxPermaWorkplaces.SelectedItem.ToString();
                BookingsPanel.Enabled = true;
                CompNumtxt.Text = string.Empty;
                PhoneNumtxt.Text = string.Empty;
                loadBookingGrid();
                LoadProblemsGrid();
            }
        }

        private void lbxTempWorkplaces_DoubleClick(object sender, EventArgs e)
        {
            if (lbxTempWorkplaces.Items.Count > 0)
            {
                lblClickedWp.Text = lbxTempWorkplaces.SelectedItem.ToString();
                BookingsPanel.Enabled = true;
                CompNumtxt.Text = string.Empty;
                PhoneNumtxt.Text = string.Empty;
                loadBookingGrid();
                LoadProblemsGrid();
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete record ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                    _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbMan.SqlStatement = " Delete from tbl_Backfill_Workplace_Bookings where [DateTime] = '" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DateTime"]) + "'  \r\n" +
                                          " and Range = '" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Range"]) + "' \r\n " +
                                          " and Workplace = '" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Workplace"]) + "'  ";

                    _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan.ExecuteInstruction();

                    loadBookingGrid();
                }
                else
                {
                }
            }
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.RowCount > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete record ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                    _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbMan.SqlStatement = " Delete from tbl_Backfill_Booking_Problems where  TimeFrom = '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["TimeFrom"]) + "' \r\n" +
                                          " and Range = '" + lblRange.Text + "' \r\n " +
                                          " and TimeTo = '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["TimeTo"]) + "'    \r\n " +
                                          " and Workplace = '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["Workplace"]) + "'  ";

                    _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan.ExecuteInstruction();

                    LoadProblemsGrid();
                }
            }
        }

        private void cmbProblems_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProblemsGrid();
        }

        private void ProblemsGrid_Click(object sender, EventArgs e)
        {

        }
    }
}