using DevExpress.XtraEditors.Controls;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class frmTramBooking : DevExpress.XtraEditors.XtraForm
    {
        #region Public variables
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        Procedures procs = new Procedures();
        #endregion

        public frmTramBooking()
        {
            InitializeComponent();
        }

        private void frmTramBooking_Load(object sender, EventArgs e)
        {
            loaddest();
            LoadGrid();
            loadTramProblems();

            MWDataManager.clsDataAccess _dbMan17 = new MWDataManager.clsDataAccess();
            _dbMan17.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan17.SqlStatement = " select * from  [tbl_Problems_Tramming] order by problemid ";
            _dbMan17.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan17.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan17.ExecuteInstruction();
            DataTable dtdest = _dbMan17.ResultsDataTable;

            cmbProb.Items.Add(string.Empty);
            foreach (DataRow dr1 in dtdest.Rows)
            {
                cmbProb.Items.Add(dr1["ProblemID"].ToString() + ":" + dr1["Description"].ToString());
            }

            // Load locos
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " select * from tbl_Equipement Where EquipmentType = 'Loco' order by EquipmentID";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            DataTable dtLoco = _dbMan.ResultsDataTable;

            LocoBox.Items.Add(string.Empty);
            foreach (DataRow dr1 in dtLoco.Rows)
            {
                LocoBox.Items.Add(dr1["EquipmentID"].ToString() + ":" + dr1["EquipmentName"].ToString());
            }


            // Load Crews
            MWDataManager.clsDataAccess _dbManCrew = new MWDataManager.clsDataAccess();
            _dbManCrew.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManCrew.SqlStatement = " select * from tbl_Orgunits order by OrgUnit";
            _dbManCrew.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCrew.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCrew.ExecuteInstruction();
            DataTable dtCrew = _dbManCrew.ResultsDataTable;

            CrewBox.Items.Add(string.Empty);
            foreach (DataRow dr1 in dtCrew.Rows)
            {
                CrewBox.Items.Add(dr1["OrgUnit"].ToString());
            }
        }

        private void LoadGrid()
        {
            MWDataManager.clsDataAccess _dbMan17 = new MWDataManager.clsDataAccess();
            _dbMan17.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan17.SqlStatement = " select bo.*, o.name from tbl_Booking_Oreflow bo \r\n" +
                                    "left outer join tbl_oreflowentities o on bo.tooreflowid = o.oreflowid \r\n" +
                                    "where bo.fromoreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' and calendardate =  '" + String.Format("{0:yyyy-MM-dd}", TramDateProp.Value) + "' \r\n" +
                                    "order by shift ";
            _dbMan17.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan17.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan17.ExecuteInstruction();
            DataTable dtdest = _dbMan17.ResultsDataTable;

            gcTramBooking.DataSource = null;
            gcTramBooking.DataSource = dtdest;

            colShift.FieldName = "Shift";
            colDestination.FieldName = "name";
            colHoppers.FieldName = "Hoppers";
            colHopperFactor.FieldName = "Grade";
            colTons.FieldName = "Tons";
            colLoco.FieldName = "Loco";
            colCrew.FieldName = "Crew";
            colDate.FieldName = "DateBooked";
        }

        public void loaddest()
        {
            MWDataManager.clsDataAccess _dbManHopper = new MWDataManager.clsDataAccess();
            _dbManHopper.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManHopper.SqlStatement = " select * from  tbl_CODE_TrammingFactors ";
            _dbManHopper.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManHopper.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManHopper.ExecuteInstruction();

            HFactcmb.Items.Clear();
            foreach (DataRow dr in _dbManHopper.ResultsDataTable.Rows)
            {
                HFactcmb.Items.Add(dr["Description"].ToString() + ":" + dr["Factor"].ToString());
            }

            DestRadio.Properties.Items.Clear();
            MWDataManager.clsDataAccess _dbMan17 = new MWDataManager.clsDataAccess();
            _dbMan17.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan17.SqlStatement = " select * from  tbl_Oreflow_BoxholeDestination where oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' order by destination ";
            _dbMan17.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan17.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan17.ExecuteInstruction();
            DataTable dtdest = _dbMan17.ResultsDataTable;
            int X = 0;
            int y = 0;

            DestRadio.Properties.Items.Add(new RadioGroupItem(0, "Shaft"));

            foreach (DataRow dr1 in dtdest.Rows)
            {
                X = X + 1;
                DestRadio.Properties.Items.Add(new RadioGroupItem(X, dr1["destination"].ToString()));

                if (dr1["def"].ToString() == "Y")
                {
                    y = X;
                }

                if (dr1["HopperDef"].ToString() == "Y")
                {
                    HFactcmb.Text = dr1["HopperFactor"].ToString();
                }
            }
            DestRadio.SelectedIndex = y;
        }

        public void loadTramProblems()
        {
            MWDataManager.clsDataAccess _dbMan17 = new MWDataManager.clsDataAccess();
            _dbMan17.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan17.SqlStatement = " select * from  tbl_BOOKINGOREFLOW_Problem  where oreflowid =  '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' and calendardate = '" + String.Format("{0:yyyy-MM-dd}", TramDateProp.Value) + "' order by shift ";
            _dbMan17.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan17.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan17.ExecuteInstruction();
            DataTable dtdest = _dbMan17.ResultsDataTable;

            gcTramProb.DataSource = dtdest;

            colProbShift.FieldName = "Shift";
            colProbDescription.FieldName = "ProblemDesc";
            colProbHours.FieldName = "Hours";
            colProbNotes.FieldName = "Notes";
        }

        private void btnAddProb_Click(object sender, EventArgs e)
        {
            string Shift = "D";
            if (ShiftProbRadio.SelectedIndex == 1)
                Shift = "A";
            if (ShiftProbRadio.SelectedIndex == 2)
                Shift = "N";

            if (cmbProb.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a valid problem", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan1.SqlStatement = "  insert into  tbl_BOOKINGOREFLOW_Problem values ('" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "', '" + String.Format("{0:yyyy-MM-dd}", TramDateProp.Value) + "', '" + Shift + "', " +
                                    "  '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(cmbProb.Text) + "',    '" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(cmbProb.Text) + "', '" + txtProbNote.Text + "', '" + spinProb.Value + "') ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;

            var result = _dbMan1.ExecuteInstruction();
            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                loadTramProblems();
            }
        }

        private void btnAddBooking_Click(object sender, EventArgs e)
        {
            string Shift = "D";
            if (ShiftRadio.SelectedIndex == 1)
                Shift = "A";
            if (ShiftRadio.SelectedIndex == 2)
                Shift = "N";

            MWDataManager.clsDataAccess _dbManOre = new MWDataManager.clsDataAccess();
            _dbManOre.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManOre.SqlStatement = " select Max(oreflowid) from tbl_oreflowentities where OreFlowcode = 'Mill' ";
            _dbManOre.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManOre.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManOre.ExecuteInstruction();

            string Dest = string.Empty;
            Dest = _dbManOre.ResultsDataTable.Rows[0][0].ToString();

            if (DestRadio.SelectedIndex > 0)
            {
                MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
                _dbMan2.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan2.SqlStatement = " select oreflowid from tbl_oreflowentities where name = '" + DestRadio.Properties.Items[DestRadio.SelectedIndex].Description.ToString() + "' ";
                _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan2.ExecuteInstruction();

                Dest = _dbMan2.ResultsDataTable.Rows[0]["oreflowid"].ToString();
            }

            string millmonth = string.Empty;
            // get millmonth
            MWDataManager.clsDataAccess _dbMan3 = new MWDataManager.clsDataAccess();
            _dbMan3.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan3.SqlStatement = "select max(millmonth) mm from dbo.tbl_CalendarMill where " +
                                    "startdate <= '" + String.Format("{0:yyyy-MM-dd}", TramDateProp.Value) + "' and  enddate >= '" + String.Format("{0:yyyy-MM-dd}", TramDateProp.Value) + "'";
            _dbMan3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3.ExecuteInstruction();

            if (_dbMan3.ResultsDataTable.Rows.Count == 0)
            {
                MessageBox.Show("No Mill Month has been set up", "No Mill Month", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (_dbMan3.ResultsDataTable.Rows[0]["mm"] == DBNull.Value)
                {
                    MessageBox.Show("No Mill Month has been set up", "No Mill Month", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                millmonth = _dbMan3.ResultsDataTable.Rows[0]["mm"].ToString();
            }

            string loco = string.Empty;
            if (LocoBox.SelectedIndex > -1)
                loco = LocoBox.SelectedItem.ToString();

            string Crew = string.Empty;
            if (CrewBox.SelectedIndex > -1)
                Crew = CrewBox.SelectedItem.ToString();

            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan1.SqlStatement = " update  tbl_Oreflow_BoxholeDestination set def = 'N' where oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' " +
                                    " update  tbl_Oreflow_BoxholeDestination set def = 'Y' where oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' and destination = '" + DestRadio.Properties.Items[DestRadio.SelectedIndex].Description.ToString() + "' " +
                                     " update  tbl_Oreflow_BoxholeDestination set HopperFactor = '" + HFactcmb.Text + "' where oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' " +
                                     "update  tbl_Oreflow_BoxholeDestination set HopperDef = 'N' where oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' " +
                                      " update  tbl_Oreflow_BoxholeDestination set HopperDef = 'Y' where oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' and destination = '" + DestRadio.Properties.Items[DestRadio.SelectedIndex].Description.ToString() + "' " +
                                    " insert into tbl_Booking_Oreflow values ('" + String.Format("{0:yyyy-MM-dd}", TramDateProp.Value) + "', '" + Shift + "', " +
                                    "  '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "',   '" + Dest + "', '" + millmonth + "', '" + TonsEdit.Text + "', '" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(HFactcmb.Text) + "', 'R', '', 'R', '" + HoppersEdit.Text + "', '', 0, '',   '" + loco + "',  '" + Crew + "', '" + TramDateProp.Value + "') ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.longNumber;

            var result = _dbMan1.ExecuteInstruction();
            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                LoadGrid();
            }
        }

        private void btnProbDelete_Click(object sender, EventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to delete this problem?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                string probDesc = gvTramProb.GetRowCellValue(gvTramProb.FocusedRowHandle, gvTramProb.Columns["ProblemDesc"]).ToString();
                string probShift = gvTramProb.GetRowCellValue(gvTramProb.FocusedRowHandle, gvTramProb.Columns["Shift"]).ToString();

                MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
                _dbMan2.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan2.SqlStatement = "Delete from  tbl_BOOKINGOREFLOW_Problem \r\n" +
                                       "where calendardate = '" + String.Format("{0:yyyy-MM-dd}", TramDateProp.Value) + "' \r\n" +
                                       "and oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' and ProblemDesc =  '" + probDesc + "' and shift =  '" + probShift + "'   ";
                _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;

                var resultSave = _dbMan2.ExecuteInstruction();
                if (resultSave.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                    loadTramProblems();
                }
            }
        }

        private void gvTramBooking_DoubleClick(object sender, EventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to delete this booking?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                string DateBooked = gvTramBooking.GetRowCellValue(gvTramBooking.FocusedRowHandle, gvTramBooking.Columns["DateBooked"]).ToString();

                MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
                _dbMan2.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan2.SqlStatement = "Delete from tbl_Booking_Oreflow where DateBooked = '" + Convert.ToDateTime(DateBooked) + "' ";
                _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;

                var resultSave = _dbMan2.ExecuteInstruction();
                if (resultSave.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                    LoadGrid();
                }
            }
        }

        private void TramDateProp_ValueChanged(object sender, EventArgs e)
        {
            LoadGrid();
            loadTramProblems();
        }

        private void btnDestAdd_Click(object sender, EventArgs e)
        {
            frmTrammingDestination Propfrm = new frmTrammingDestination();
            Propfrm._BHID = BHLBL.Text;
            Propfrm._theSystemDBTag = _theSystemDBTag;
            Propfrm._UserCurrentInfoConnection = _UserCurrentInfoConnection;
            Propfrm.StartPosition = FormStartPosition.CenterScreen;
            Propfrm.ShowDialog();

            loaddest();
        }

        private void gvTramProb_DoubleClick(object sender, EventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to delete this problem?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                string probDesc = gvTramProb.GetRowCellValue(gvTramProb.FocusedRowHandle, gvTramProb.Columns["ProblemDesc"]).ToString();
                string probShift = gvTramProb.GetRowCellValue(gvTramProb.FocusedRowHandle, gvTramProb.Columns["Shift"]).ToString();

                MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
                _dbMan2.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan2.SqlStatement = "Delete from  tbl_BOOKINGOREFLOW_Problem \r\n" +
                                       "where calendardate = '" + String.Format("{0:yyyy-MM-dd}", TramDateProp.Value) + "' \r\n" +
                                       "and oreflowid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(BHLBL.Text) + "' and ProblemDesc =  '" + probDesc + "' and shift =  '" + probShift + "'   ";
                _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;

                var resultSave = _dbMan2.ExecuteInstruction();
                if (resultSave.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                    loadTramProblems();
                }
            }
        }

        private void btnDestDel_Click(object sender, EventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to delete " + DestRadio.Properties.Items[DestRadio.SelectedIndex].Description.ToString() + "?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
                _dbMan2.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan2.SqlStatement = "Delete from  tbl_Oreflow_BoxholeDestination \r\n" +
                                       "where Destination = '" + DestRadio.Properties.Items[DestRadio.SelectedIndex].Description.ToString() + "'  ";
                _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;

                var resultSave = _dbMan2.ExecuteInstruction();
                if (resultSave.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                    loaddest();
                }
            }
        }

        private void txtProbNote_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void HFactcmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Decimal HFact = Convert.ToDecimal(procs.ExtractAfterColon(HFactcmb.SelectedItem.ToString()));
                TonsEdit.EditValue = Convert.ToDecimal(HoppersEdit.EditValue) * HFact;
            }
            catch { };
        }

        private void HoppersEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                Decimal HFact = Convert.ToDecimal(procs.ExtractAfterColon(HFactcmb.SelectedItem.ToString()));
                TonsEdit.EditValue = Convert.ToDecimal(HoppersEdit.EditValue) * HFact;
            }
            catch { };
        }
    }
}