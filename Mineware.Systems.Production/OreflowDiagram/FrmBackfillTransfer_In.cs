using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class FrmBackfillTransfer_In : DevExpress.XtraEditors.XtraForm
    {
        #region public variable
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        #endregion

        public FrmBackfillTransfer_In()
        {
            InitializeComponent();
        }

        private void FrmBackfillTransfer_In_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " select Name, 'Y' aa from [dbo].[tbl_OreFlowEntities] where OreFlowCode in ('Tank','Pachuca','Dam')  \r\n" +
                                  " and OreFlowID <> '" + LocationTotxt.Text + "' \r\n " +
                                  " union  \r\n" +
                                  " select Name, 'b' aa from [dbo].[tbl_OreFlowEntities] where OreFlowCode in ('Tank','Pachuca','Dam') and OreFlowID = '" + LocationTotxt.Text + "'    ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;
            DataSet ds = new DataSet();

            ds.Tables.Add(dt);

            foreach (DataRow row in dt.Rows)
            {
                if (row["aa"].ToString() == "Y")
                {
                    cmbReceiveTank.Items.Add(row["Name"]);
                }
                else
                {
                    LocationTotxt.Text = row["Name"].ToString();
                }
            }

            MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
            _dbMana.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMana.SqlStatement = " select * from tbl_Backfill_TransferRange order by TRangeID ";
            _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMana.ExecuteInstruction();

            DataTable dta = _dbMana.ResultsDataTable;
            DataSet dsa = new DataSet();
            dsa.Tables.Add(dta);

            foreach (DataRow row in dta.Rows)
            {
                cmbRange.Items.Add(row["TRangeName"]);
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

            loadTransferInGrid();
            LoadProblemsGrid();
        }

        private void loadTransferInGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " select * from tbl_Backfill_Transfer_In where CalenderDateSent = '" + String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString()) + "'    ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            if (dt.Rows.Count > 0)
            {
                colDateSent.FieldName = "CalenderDateSent";
                colRecTank.FieldName = "ReceiveTank";
                colFlowRate.FieldName = "Flowrate";
                colRange.FieldName = "ranges";
                colRD.FieldName = "RD";
                colLocTo.FieldName = "LocationTo";
                colBackfillStart.FieldName = "BackfillStart";
                colBackfillStop.FieldName = "BackfillStop";
                colFlushStart.FieldName = "FlushStart";
                colFlushStop.FieldName = "FlushStop";
                colRejected.FieldName = "Rejected";
                colFlushStart2.FieldName = "FlushStart2";
                colFlushStop2.FieldName = "FlushStop2";
                colTonnage.FieldName = "Tonnage";
                colBQT.FieldName = "BQT";
            }

            dtTransferIn.DataSource = ds.Tables[0];
        }

        public void LoadProblemsGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " Select * from [dbo].[tbl_Backfill_Problems] Where Description = '" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(cmbProblems.Text) + "'  and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString()) + "'   ";
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

        private void SentDate_ValueChanged(object sender, EventArgs e)
        {
            loadTransferInGrid();
        }

        private void AddTransferbtn_Click(object sender, EventArgs e)
        {
            if (cmbReceiveTank.Text == string.Empty)
            {
                MessageBox.Show("Please Choose a Receiving Tank");
                return;
            }

            if (cmbRange.Text == string.Empty)
            {
                MessageBox.Show("Please Choose a Range");
                return;
            }

            string rejected = string.Empty;

            if (cbxReject.Checked == true)
            {
                rejected = "Y";
            }
            else
            {
                rejected = "N";
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
            _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_Backfill_Transfer_In (CalenderDateSent,ReceiveTank,ranges,LocationTo \r\n " +
                                  ",Flowrate,RD,Rejected,FlushStart,FlushStop,BackfillStart,BackfillStop,FlushStart2,FlushStop2,Tonnage,BQT)  values \r\n " +
                                  "('" + String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString()) + "' ,'" + cmbReceiveTank.Text.ToString() + "', '" + cmbRange.Text.ToString() + "', '" + LocationTotxt.Text + "',  \r\n" +
                                  " '" + Flowratetxt.Text + "', '" + RDtxt.Text + "', '" + rejected + "','" + timeFlushStart + "', '" + timeFlushStop + "','" + timeBackfillStart + "', '" + timeBackfillStop + "', \r\n" +
                                  " '" + timeFlushStart2 + "' , '" + timeFlushStop2 + "' , '" + Tonnagetxt.Text + "' , '" + BQTtxt.Text + "'     )   ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            loadTransferInGrid();

            Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
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
            //Backfillfrm.lblID.Text = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(cmbProblems.Text);
            Backfillfrm._Description = ProductionGlobal.ProductionGlobal.ExtractAfterColon(cmbProblems.Text);
            Backfillfrm._From = timeProbStart;
            Backfillfrm._To = timeProbStop;
            Backfillfrm._Date = String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString());
            Backfillfrm._FormType = "Transfer";
            Backfillfrm._theSystemDBTag = _theSystemDBTag;
            Backfillfrm._UserCurrentInfoConnection = _UserCurrentInfoConnection;
            Backfillfrm.ShowDialog();

            LoadProblemsGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbReceiveTank.Text == string.Empty)
            {
                MessageBox.Show("Please Choose a Receiving Tank");
                return;
            }

            if (cmbRange.Text == string.Empty)
            {
                MessageBox.Show("Please Choose a Range");
                return;
            }

            string rejected = string.Empty;

            if (cbxReject.Checked == true)
            {
                rejected = "Y";
            }
            else
            {
                rejected = "N";
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
            _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_Backfill_Transfer_In (CalenderDateSent,ReceiveTank,ranges,LocationTo \r\n " +
                                  ",Flowrate,RD,Rejected,FlushStart,FlushStop,BackfillStart,BackfillStop,FlushStart2,FlushStop2,Tonnage,BQT)  values \r\n " +
                                  "('" + String.Format("{0:yyyy-MM-dd}", SentDate.Value.Date.ToString()) + "' ,'" + cmbReceiveTank.Text.ToString() + "', '" + cmbRange.Text.ToString() + "', '" + LocationTotxt.Text + "',  \r\n" +
                                  " '" + Flowratetxt.Text + "', '" + RDtxt.Text + "', '" + rejected + "','" + timeFlushStart + "', '" + timeFlushStop + "','" + timeBackfillStart + "', '" + timeBackfillStop + "', \r\n" +
                                  " '" + timeFlushStart2 + "' , '" + timeFlushStop2 + "' , '" + Tonnagetxt.Text + "' , '" + BQTtxt.Text + "'     )   ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            loadTransferInGrid();

            Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete record ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                    _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbMan.SqlStatement = " Delete from tbl_Backfill_Transfer_In where CalenderDateSent = '" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["CalenderDateSent"]) + "'  \r\n" +
                                          " and ReceiveTank = '" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ReceiveTank"]) + "' \r\n " +
                                          " and LocationTo = '" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["LocationTo"]) + "'  \r\n " +
                                          " and ranges = '" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["ranges"]) + "' \r\n  " +
                                          " and Tonnage = '" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Tonnage"]) + "' \r\n " +
                                          " and Flowrate = '" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Flowrate"]) + "'  ";
                    _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan.ExecuteInstruction();

                    loadTransferInGrid();
                }
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
                    _dbMan.SqlStatement = " Delete from tbl_Backfill_Problems where ProblemID = '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["ProblemID"]) + "' \r\n " +
                                          " and TimeFrom = '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["TimeFrom"]) + "' and TimeTo = '" + gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["TimeTo"]) + "'  \r\n";
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
    }
}