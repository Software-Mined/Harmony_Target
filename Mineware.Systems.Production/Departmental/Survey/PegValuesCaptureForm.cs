using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Survey
{
    public partial class PegValuesCaptureForm : DevExpress.XtraEditors.XtraForm
    {
        #region Data Feilds
        string WPid;
        string PegID;
        string PegValue;
        string Letter1;
        string Letter2;
        string Letter3;

        StringBuilder sbSqlQuery = new StringBuilder();


        public string _ConnectionString;
        #endregion

        #region Constructors

        public PegValuesCaptureForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods/Functions

        private void PegValuesCaptureForm_Load(object sender, EventArgs e)
        {
            txtPegNum.Text.ToUpper();
            LoadPegsValues();
        }

        public void LoadPegsValues()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = _ConnectionString;
            _dbMan.SqlStatement = "select * from tbl_Peg where WorkplaceID = '" + lblWpID.Text + "' \r\n order by Value desc";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtPegData = _dbMan.ResultsDataTable;
            gcPegValues.DataSource = dtPegData;

            colWorkplaceID.FieldName = "WorkplaceID";
            colPegID.FieldName = "PegID";
            colValue.FieldName = "Value";
            colLetter1.FieldName = "Letter1";
            colLetter2.FieldName = "Letter2";
            colLetter3.FieldName = "Letter3";
        }

        #endregion

        #region Events

        private void gvPegValues_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            PegID = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["PegID"]).ToString();
            WPid = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["WorkplaceID"]).ToString();
            PegValue = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["Value"]).ToString();
            Letter1 = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["Letter1"]).ToString();
            Letter2 = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["Letter2"]).ToString();
            Letter3 = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["Letter3"]).ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = _ConnectionString;
            _dbMan.SqlStatement = "Select * from tbl_Peg where WorkplaceID = '" + lblWpID.Text + "' and PegID = '" + txtPegNum.EditValue.ToString() + "'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            DataTable dt = _dbMan.ResultsDataTable;
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("This Peg (" + txtPegNum.EditValue.ToString().ToUpper() + ") already exists for this workplace", "Already exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                sbSqlQuery.Clear();
                sbSqlQuery.AppendLine("insert into tbl_Peg ");
                sbSqlQuery.AppendLine(" values('" + txtPegNum.EditValue.ToString().ToUpper() + "', '" + lblWpID.Text + "', ");
                sbSqlQuery.AppendLine(" '" + txtPegValue.EditValue + "', '" + txtLetter1.EditValue + "', '" + txtLetter2.EditValue + "', ");
                sbSqlQuery.AppendLine(" '" + txtLetter3.EditValue + "') ");

                MWDataManager.clsDataAccess _PegSave = new MWDataManager.clsDataAccess();
                _PegSave.ConnectionString = _ConnectionString;
                _PegSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _PegSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _PegSave.SqlStatement = sbSqlQuery.ToString();

                var ActionResult = _PegSave.ExecuteInstruction();

                if (ActionResult.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Peg Values Captured", Color.CornflowerBlue);
                }
            }

            //Clear Data in Text boxes
            txtPegNum.EditValue = string.Empty;
            txtPegValue.EditValue = "0.00";
            txtLetter1.EditValue = string.Empty;
            txtLetter2.EditValue = string.Empty;
            txtLetter3.EditValue = string.Empty;

            LoadPegsValues();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (PegID != string.Empty && PegID != null)
            {
                sbSqlQuery.Clear();
                sbSqlQuery.AppendLine(" Select * from tbl_Planning where Activity = 1 and ");
                sbSqlQuery.AppendLine(" WorkplaceID = '" + WPid + "' and PegID = '" + PegID + "' ");

                MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
                _dbMan1.ConnectionString = _ConnectionString;
                _dbMan1.SqlStatement = sbSqlQuery.ToString();
                _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan1.ExecuteInstruction();
                DataTable dt = _dbMan1.ResultsDataTable;

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("You cannot delete this Peg because there is a booking attched to it", "Invalid Option", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to Delete this Peg?", "Confirmation Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        sbSqlQuery.Clear();
                        sbSqlQuery.AppendLine("delete from tbl_Peg where WorkplaceID = '" + WPid + "' ");
                        sbSqlQuery.AppendLine(" and PegID = '" + PegID + "' ");

                        MWDataManager.clsDataAccess _PegSave = new MWDataManager.clsDataAccess();
                        _PegSave.ConnectionString = _ConnectionString;
                        _PegSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _PegSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PegSave.SqlStatement = sbSqlQuery.ToString();

                        var ActionResult = _PegSave.ExecuteInstruction();

                        if (ActionResult.success)
                        {
                            Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Peg Value deleted", Color.Red);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a record you want to delete", "Delete Peg Values", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //Clear Data in Text boxes
            txtPegNum.EditValue = string.Empty;
            txtPegValue.EditValue = "0.00";
            txtLetter1.EditValue = string.Empty;
            txtLetter2.EditValue = string.Empty;
            txtLetter3.EditValue = string.Empty;

            LoadPegsValues();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}