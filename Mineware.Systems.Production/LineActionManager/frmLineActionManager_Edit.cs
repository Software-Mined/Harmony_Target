using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.LineActionManager
{
    public partial class frmLineActionManager_Edit : DevExpress.XtraEditors.XtraForm
    {
        clsActionManager _clsActionManager = new clsActionManager();
        public frmLineActionManager_Edit()
        {
            InitializeComponent();
        }

        public string currHaz;
        public string UserCurrentInfoConnection;
        public string CloseDate;


        private void frmLineActionManager_Edit_Load(object sender, EventArgs e)
        {
            lblIncidentNumber.Visible = false;
            label1.Visible = false;

            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfoConnection);
            _sqlConnection.SqlStatement = "Select 'High' Code \r\n" +
                                          "union \r\n" +
                                          "Select 'Medium' Code \r\n" +
                                          "union \r\n" +
                                          "Select 'Low' Code \r\n" +
            string.Empty;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ResultsTableName = "Receive";
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = _sqlConnection.ResultsDataTable;

            DataSet ds = new DataSet();
            ds.Tables.Add(dtReceive);

            leHazard.Properties.DataSource = dtReceive;
            leHazard.Properties.PopulateColumns();
            leHazard.Properties.ValueMember = "Code";
            leHazard.Properties.DisplayMember = "Code";

            leHazard.EditValue = currHaz;
            if (currHaz == "H")
            {
                currHaz = "High";
                leHazard.EditValue = currHaz;
            }

            if (currHaz == "M")
            {
                currHaz = "Medium";
                leHazard.EditValue = currHaz;
            }

            if (currHaz == "L")
            {
                currHaz = "Low";
                leHazard.EditValue = currHaz;
            }

            ///Load Action
            MWDataManager.clsDataAccess _sqlConnection2 = new MWDataManager.clsDataAccess();
            _sqlConnection2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfoConnection);
            _sqlConnection2.SqlStatement = "select * from  [dbo].[tbl_Incidents] where workplace = '" + txtWorkplace.Text + "'";            
            _sqlConnection2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection2.ExecuteInstruction();

            if (_sqlConnection2.ResultsDataTable.Rows.Count > 0)
            {
                txtRespFeedback.EditValue = _sqlConnection2.ResultsDataTable.Rows[0]["Responsible_Person_Feedback"].ToString();               

                if (_sqlConnection2.ResultsDataTable.Rows[0]["Hazard"].ToString() == "H")
                {
                    currHaz = "High";
                    leHazard.EditValue = currHaz;
                }

                if (_sqlConnection2.ResultsDataTable.Rows[0]["Hazard"].ToString() == "M")
                {
                    currHaz = "Medium";
                    leHazard.EditValue = currHaz;
                }

                if (_sqlConnection2.ResultsDataTable.Rows[0]["Hazard"].ToString() == "L")
                {
                    currHaz = "Low";
                    leHazard.EditValue = currHaz;
                }

            }

        }


        private void cbxClose_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void barBtnSave_Click(object sender, EventArgs e)
        {
            string SelectHaz = leHazard.EditValue.ToString();

            if (leHazard.EditValue.ToString() == "High")
            {
                SelectHaz = "H";
            }

            if (leHazard.EditValue.ToString() == "Medium")
            {
                SelectHaz = "M";
            }

            if (leHazard.EditValue.ToString() == "Low")
            {
                SelectHaz = "L";
            }

            var EditedRow = clsActionManager.dtActions.Select(
                                           " IncidentNumber = '" + lblIncidentNumber.Text + "' ");
            foreach (var dr in EditedRow)
            {
                dr["workplace"] = txtWorkplace.Text;
                dr["Section"] = txtMineoverseer.EditValue;
                dr["Hazard"] = SelectHaz;
                dr["RespFeedback"] = txtRespFeedback.EditValue.ToString();
                if (cbxClose.Checked == true)
                {
                    dr["Action_Status"] = "Closed";
                }
                else
                {
                    dr["Action_Status"] = "Open";
                }
                dr["Action_Close_Date"] = String.Format("{0:yyyy-MM-dd}", CloseDate);
            }
            clsActionManager.dtActions.AcceptChanges();

            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfoConnection);
            _sqlConnection.SqlStatement = "update [dbo].[tbl_Incidents] set workplace = '" + txtWorkplace.Text + "', \r\n ";
            _sqlConnection.SqlStatement = _sqlConnection.SqlStatement + " Section = '" + txtMineoverseer.EditValue + "' , Hazard = '" + SelectHaz + "', \r\n ";
            _sqlConnection.SqlStatement = _sqlConnection.SqlStatement + " Responsible_Person_Feedback  = '" + txtRespFeedback.EditValue.ToString() + "', \r\n ";
            _sqlConnection.SqlStatement = _sqlConnection.SqlStatement + " Action_Title = '" + txtAction.EditValue.ToString() + "' \r\n ";

            if (cbxClose.Checked == true)
            {
                _sqlConnection.SqlStatement += " ,Action_Status = 'Closed', \r\n "
                    + " Action_Close_Date = '" + String.Format("{0:yyyy-MM-dd}", CloseDate) + "' \r\n";
            }

            _sqlConnection.SqlStatement = _sqlConnection.SqlStatement + " where Mineware_Action_ID = '" + lblIncidentNumber.Text + "'";
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();

            //frmLineActionManager_Edit Mainfrm;
            //Mainfrm = (frmLineActionManager_Edit)this.FindForm();
            //alertControl1.Show(Mainfrm, "Information", "Record Updated Successfuly.");       

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtWorkplace.EditValue = string.Empty;
            txtAction.EditValue = string.Empty;
            txtRespFeedback.EditValue = string.Empty;
            txtMineoverseer.EditValue = string.Empty;
            this.Close();
        }

    }
}
