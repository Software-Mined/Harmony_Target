using DevExpress.XtraEditors;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Ventillation
{
    public partial class VentilationActionForm : DevExpress.XtraEditors.XtraForm
    {
        #region Data Feilds

        string repImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\OtherInspections\ActionsImages";  //Path to store Images
        DialogResult result1;

        //Private data fields
        private String sourceFile;
        private String destinationFile;
        private String FileName = string.Empty;

        //Public variables
        public string WPID = string.Empty;
        public string FlagEdit = "N";
        public string ActID = string.Empty;
        public string ItemSaveID = string.Empty;
        public string Section;

        public string Item;
        public string Type;
        public string AllowExit;

        public string CurrentDate;

        string ImageDir = string.Empty;
        string TempDir = string.Empty;

        string ImageSaved = "N";
        public string hasSaved = string.Empty;

        public string _theSystemDBTag;
        public string _UserCurrentInfo;

        #endregion

        #region Constructor

        public VentilationActionForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods/Functions
        private void VentilationActionForm_Load(object sender, EventArgs e)
        {
            txtReqDate.EditValue = DateTime.Now.Date;

            //LoadSBandMo
            MWDataManager.clsDataAccess _LoadUser = new MWDataManager.clsDataAccess();
            _LoadUser.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _LoadUser.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadUser.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadUser.SqlStatement = " Select USERID +':' + UserName Person, '" + lblSection.Text + "' Section from tbl_Users";
            _LoadUser.ExecuteInstruction();

            DataTable tbl_User = _LoadUser.ResultsDataTable;

            RespPersonCmb.Properties.DataSource = tbl_User;
            RespPersonCmb.Properties.DisplayMember = "Person";
            RespPersonCmb.Properties.ValueMember = "Person";
            RespPersonCmb.Properties.PopulateColumns();
            RespPersonCmb.Properties.Columns[0].Width = 60;
            RespPersonCmb.Properties.Columns[1].Width = 40;

            //LoadSBandMo
            MWDataManager.clsDataAccess _LoadSB = new MWDataManager.clsDataAccess();
            _LoadSB.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo); ;
            _LoadSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadSB.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadSB.SqlStatement = " Select USERID +':' + UserName Person, '" + lblSection.Text + "' Section from tbl_Users";
            _LoadSB.ExecuteInstruction();

            DataTable tbl_UserSB = _LoadSB.ResultsDataTable;

            OverseerCmb.Properties.DataSource = tbl_UserSB;
            OverseerCmb.Properties.DisplayMember = "Person";
            OverseerCmb.Properties.ValueMember = "Person";
            OverseerCmb.Properties.PopulateColumns();
            OverseerCmb.Properties.Columns[0].Width = 60;
            OverseerCmb.Properties.Columns[1].Width = 40;

            PriorityCmb.Properties.Items.Clear();
            PriorityCmb.Properties.Items.Add("H");
            PriorityCmb.Properties.Items.Add("M");
            PriorityCmb.Properties.Items.Add("L");
        }

        #endregion

        #region Events



        #endregion

        private void btnSaveAct_Click(object sender, EventArgs e)
        {
            if (lblWPDesc.Text == string.Empty)
            {
                XtraMessageBox.Show("Please select a workplace");
                return;
            }

            if (PriorityCmb.SelectedIndex == -1)
            {
                XtraMessageBox.Show("Please select a Priority for the Action");
                return;
            }

            if (RespPersonCmb.Text == "[EditValue is null]")
            {
                XtraMessageBox.Show("Please select a Responsible Person");
                return;
            }

            if (OverseerCmb.Text == "[EditValue is null]")
            {
                XtraMessageBox.Show("Please select a Overseer");
                return;
            }

            //if (FlagEdit == "Edit")
            //{
            //	LoadEdit();
            //	SaveImage();               

            //	Close();

            //	return;
            //}

            MWDataManager.clsDataAccess _GetID = new MWDataManager.clsDataAccess();
            _GetID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo); ;
            _GetID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GetID.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GetID.SqlStatement = " select max(id)+1 a from  [tbl_Shec_Incidents] ";

            _GetID.ExecuteInstruction();

            ItemSaveID = _GetID.ResultsDataTable.Rows[0][0].ToString();

            //do checks
            hasSaved = "N";

            //string WP = "";

            //if (AnswerLbl.Text == "N/A")
            //             WP = cbxWorkplace.EditValue.ToString();
            //         if (AnswerLbl.Text != "N/A")
            //	WP = txtWorkplace.EditValue.ToString();

            string image = string.Empty;

            if (ImageSaved == "Y")
            {
                image = ItemSaveID;
            }

            string sqlQuery = string.Empty;
            ///Edit
            if (ActID != string.Empty)
            {
                sqlQuery = "Update tbl_Shec_IncidentsVent \r\n" +
                    " set  Action = '" + txtRemarks.Text + "',  Priority = '" + PriorityCmb.Text + "', CompNotes = '" + ActID + "', \r\n " +
                    " RespPerson = '" + RespPersonCmb.EditValue + "', HOD = '" + OverseerCmb.EditValue + "'\r\n" +
                    " where ID = '" + ActID + "' \r\n ";
                //SaveImage();
            }

            if (ActID == string.Empty)
            {
                sqlQuery = " INSERT INTO tbl_Shec_IncidentsVent \r\n" +
                    " VALUES \r\n" +
                    "((select isnull( max(id)+1, '1')a from  [tbl_Shec_Incidents] ), '" + Type + "', null " +
                    ", '" + lblWPDesc.Text + "','" + Item + "','" + txtRemarks.Text + "',null,null " +
                    ",'" + String.Format("{0:yyyy-MM-dd}", CurrentDate) + "','','','" + PriorityCmb.Text + "','" + RespPersonCmb.Text.ToString() + "',null,'" + OverseerCmb.Text.ToString() + "',null,'' " +
                    ",'','','','','','','','','','','','','','','' " +
                    ",'','','','','','','','','' ,'' ,'" + String.Format("{0:yyyy-MM-dd}", txtReqDate.EditValue) + "',null,null,null,'" + TUserInfo.UserID + "','" + image + "','' " +
                    ",null,null,'','','','',null,null,'','') ";
            }

            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _ActionSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionSave.SqlStatement = sqlQuery;

            var ActionResult = _ActionSave.ExecuteInstruction();
            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Action saved", Color.CornflowerBlue);
                hasSaved = "Y";
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}