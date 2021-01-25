using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class frmAddActivity : DevExpress.XtraEditors.XtraForm
    {
        public frmAddActivity()
        {
            InitializeComponent();
        }

        #region private variables

        #endregion

        #region public variables
        public string lblFrmtype;
        public string lblActType;
        public string lblID;
        public string lblDestination;
        public string _UserCurrentInfoConnection;
        public DataTable _dtTaskCheck = new DataTable();
        #endregion

        private void frmAddActivity_Load(object sender, EventArgs e)
        {
            if (lblActType == "Main")
            {
                cmbMeasureType.Visible = false;
                spinEditDuration.Visible = false;
                Amounttxt.Visible = false;
                lblHide1.Visible = false;
                lblHide2.Visible = false;
                lblHide3.Visible = false;

                lblHide2.Visible = false;
                cmbImpact.Visible = false;
                lblHide3.Visible = false;
            }

            cmbMeasureType.Items.Add("Time");
            cmbMeasureType.Items.Add("Quantity");
            cmbMeasureType.Items.Add("SQM");
            cmbMeasureType.Items.Add("Meters");
            cmbMeasureType.Items.Add("% Complete");
            cmbMeasureType.Items.Add("Yes/No");

            if (string.IsNullOrEmpty(lblDestination))
            {
                //picCheck.Visible = false;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string WorkingAboveBelow = "N";


            if (lblActType == "Main")
            {
                if (lblFrmtype == "Add")
                {
                    MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                    _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                    _dbManMain.SqlStatement = " Insert into tbl_SDB_Activity (MainActID,Description,DocShortCut) values( \r\n" +
                                              " (select  'A' + substring(convert(varchar(10),convert(decimal(18,0),substring(max([MainActID]),2,5)) +1 +100000),3,6) num from [dbo].[tbl_SDB_Activity]) \r\n" +
                                              ",'" + Descriptiontxt.Text + "','')   \r\n\r\n";

                    _dbManMain.SqlStatement = _dbManMain.SqlStatement + " Insert into tbl_SDBPM (PMID,PMDescription) values( \r\n" +
                                              " (select  'PM' + substring(convert(varchar(10),convert(decimal(18,0),substring(max([PMID]),3,5)) +1 +100000),3,6) num from [dbo].[tbl_SDBPM]) \r\n" +
                                              ",'" + Descriptiontxt.Text + "')   ";

                    _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMain.ExecuteInstruction();
                }

                if (lblFrmtype == "Edit")
                {
                    MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                    _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                    _dbManMain.SqlStatement = " Update tbl_SDB_Activity set Description = '" + Descriptiontxt.Text + "' , DocShortCut = '' where MainActID = '" + lblID + "'  ";

                    _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMain.ExecuteInstruction();
                }
            }

            if (lblActType == "Sub")
            {
                if (lblFrmtype == "Add")
                {
                    DataView dv = new DataView(_dtTaskCheck);
                    string SearchExpression = null;

                    if (!String.IsNullOrEmpty(Descriptiontxt.Text))//(Filtertxt.Text))
                    {
                        SearchExpression = string.Format("'{0}'", Descriptiontxt.Text);//Filtertxt.Text);
                        dv.RowFilter = "Description = " + SearchExpression;
                    }

                    if (dv.Count > 0)
                    {
                        MessageBox.Show("Can't add Same Task twice");
                        return;
                    }

                    MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                    _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                    _dbManMain.SqlStatement = " Insert into tbl_SDB_SubActivity (Description,DefaultDuration,MeasureType,Amount,DocShortCut) \r\n" +
                                              "values('" + Descriptiontxt.Text + "','" + spinEditDuration.Value.ToString() + "','" + cmbMeasureType.Text + "','" + Amounttxt.Text + "','')  ";

                    _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMain.ExecuteInstruction();
                }

                if (lblFrmtype == "Edit")
                {
                    MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                    _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                    _dbManMain.SqlStatement = " Update tbl_SDB_SubActivity set Description = '" + Descriptiontxt.Text + "' ,DefaultDuration = '" + spinEditDuration.Value.ToString() + "' ,MeasureType = '" + cmbMeasureType.Text + "',Amount = '" + Amounttxt.Text + "' , DocShortCut = ''   where SubActID = '" + lblID + "'  ";

                    _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManMain.ExecuteInstruction();
                }
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}