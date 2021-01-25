using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class frmProblemProperties : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Global parameters
        /// </summary>
        //public string UserCurrentInfo;
        public string EditID;
        Procedures procs = new Procedures();

        public frmProblemProperties()
        {
            InitializeComponent();
        }

        void LoadProblems()
        {
            //ProbPnl.Visible = true;
            //ProbPnl.Dock = DockStyle.Fill;
            //ProbPnl.BringToFront();
            //Text = "Problem Bookings";

            //populate EnquirerID combo
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

            _dbMan.SqlStatement = " select * from dbo.tbl_Enquirer ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            DataTable dt = _dbMan.ResultsDataTable;




            foreach (DataRow dr in dt.Rows)
            {
                txtEnquirerID.Properties.Items.Add(dr["EnquirerID"].ToString() + ":" + dr["Description"].ToString());
            }
            // ProblemsEnqIDCombo.Enabled = true;

            //populate code combo box
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

            _dbMan1.SqlStatement = " select * from dbo.tbl_Codes_HQProblemGroups ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();
            DataTable dt1 = _dbMan1.ResultsDataTable;

            foreach (DataRow dr1 in dt1.Rows)
            {
                txtHQCat.Properties.Items.Add(dr1["hqcat"].ToString());
            }

            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

            _dbMan2.SqlStatement = " select * from dbo.tbl_Code_ProblemGroup ";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ExecuteInstruction();
            DataTable dt2 = _dbMan2.ResultsDataTable;

            foreach (DataRow dr2 in dt2.Rows)
            {
                txtPropCode.Properties.Items.Add(dr2["ProblemGroupCode"].ToString());
            }

            txtExtraInfo.Properties.Items.Add("No Notes");
            txtExtraInfo.Properties.Items.Add("Notes Not Forced");
            txtExtraInfo.Properties.Items.Add("Notes Forced");

            MWDataManager.clsDataAccess _dbMan3 = new MWDataManager.clsDataAccess();
            _dbMan3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

            _dbMan3.SqlStatement = " select * from dbo.tbl_EmailGroups order by EmailGroup";
            _dbMan3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3.ExecuteInstruction();
            DataTable dt3 = _dbMan3.ResultsDataTable;

            txtEmailGroup.Properties.Items.Add(string.Empty);

            foreach (DataRow dr3 in dt3.Rows)
            {
                txtEmailGroup.Properties.Items.Add(dr3["EmailGroup"].ToString());
            }



            //if (EditID == "")
            //{
            //    ProblemsIDTxt.Enabled = true;
            //    //MessageBox.Show("ADD   "+ EditID.ToString());
            //}
            //else
            //{
            //    ProblemsIDTxt.Text = EditID.ToString();
            //    ProblemsDescTxt.Text = SysAdminFrm.ProbDesc.ToString();
            //    //need to populate combo boxes
            //    ProblemsEnqIDCombo.Text = SysAdminFrm.ProbEnqId.ToString();
            //    ProblemsInfoCombo.Text = SysAdminFrm.ProbInfo.ToString();
            //    EmailCombo.Text = SysAdminFrm.ProbEmail.ToString();

            //    ProblemsCodeCombo.Text = SysAdminFrm.ProbCode.ToString();
            //    ProblemsHQCatCombo.Text = SysAdminFrm.ProbHOCat.ToString();

            //    ProblemsIDTxt.Enabled = false;
            //    ProblemsIDTxt.BackColor = Color.White;
            //}

            //btnAdd.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void frmProblemProperties_Load(object sender, EventArgs e)
        {
            LoadProblems();
            txtEnquirerID.SelectedIndex = -1;
            txtHQCat.SelectedIndex = -1;
            txtPropCode.SelectedIndex = -1;

        }

        void SaveProblems()
        {
            string IsValid = string.Empty;
            //do checks
            if (txtProbID.Text == string.Empty)
            {
                MessageBox.Show("Please enter the Problem Id", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtProbDesc.Text == string.Empty)
            {
                MessageBox.Show("Please enter the Problem Description", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtEnquirerID.Text == string.Empty)
            {
                MessageBox.Show("Please select the Enquirer Id", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtExtraInfo.Text == string.Empty)
            {
                MessageBox.Show("Please select Extra Info for entry", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtProbDesc.Text == string.Empty)
            {
                MessageBox.Show("Please select a Problem Code", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //if (ProblemsHQCatCombo.Text == "")
            //{
            //    MessageBox.Show("Please select HQ Category for problem", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            if (radYes.Checked == true)
            {
                IsValid = "Y";
            }
            else
            {
                IsValid = "N";
            }

            //string EnqID = procs.ExtractBeforeColon(txtEnquirerID.Text.ToString());




            if (EditID == null)
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                _dbMan.SqlStatement = " insert into tbl_Problem (ProblemID, ProblemGroupCode, EnquirerID, Description, ExtraInfo, Valid, HQCat, emailgroup) " +
                                      " VALUES ( '" + txtProbID.Text.ToString() + "', '" + txtPropCode.Text.ToString() + "', " +
                                      " '" + procs.ExtractBeforeColon(txtEnquirerID.Text.ToString()) + "', '" + txtProbDesc.Text.ToString() + "', '" + txtExtraInfo.SelectedIndex + "',  " +
                                      " '" + IsValid.ToString() + "', '" + txtHQCat.Text.ToString() + "', '" + txtExtraInfo.Text.ToString() + "' )";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
            }
            else
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);

                _dbMan.SqlStatement = " update tbl_Problem set ProblemGroupCode = '" + txtPropCode.Text.ToString() + "', EnquirerID = '" + procs.ExtractBeforeColon(txtEnquirerID.Text.ToString()) + "', " +
                                      " Description =  '" + txtProbDesc.Text.ToString() + "', ExtraInfo =   '" + txtExtraInfo.SelectedIndex + "', " +
                                      " Valid  = '" + IsValid.ToString() + "', HQCat = '" + txtHQCat.Text.ToString() + "', emailgroup = '" + txtEmailGroup.Text.ToString() + "' " +
                                      " where ProblemID = '" + txtProbID.Text.ToString() + "' ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

            }

            //if (EditID == "")
            //{
            //    MessageFrm MsgFrm = new MessageFrm();
            //    MsgFrm.Text = "Record Inserted";
            //    Procedures.MsgText = "Problem Added";
            //    MsgFrm.Show();
            //}
            //else
            //{
            //    MessageFrm MsgFrm = new MessageFrm();
            //    MsgFrm.Text = "Record Updated";
            //    Procedures.MsgText = "Problem Updated";
            //    MsgFrm.Show();
            //}


            IsValid = "Y";
            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Default Cycle Saved.", Color.CornflowerBlue);
        }


        private void btnSaveProb_Click_1(object sender, EventArgs e)
        {
            SaveProblems();
        }
    }
}