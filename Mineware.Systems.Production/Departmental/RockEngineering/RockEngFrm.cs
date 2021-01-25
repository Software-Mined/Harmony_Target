using DevExpress.XtraEditors.Repository;
using FastReport;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.RockEngineering
{
    public partial class RockEngFrm : DevExpress.XtraEditors.XtraForm
    {
        private string ReportsFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        public string LTO = "";
        Report theReport3 = new Report();

        DialogResult result1;
        OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

        DialogResult resultDocpic1;
        OpenFileDialog openFileDialogDOcpic1 = new System.Windows.Forms.OpenFileDialog();

        DialogResult resultDocpic2;
        OpenFileDialog openFileDialogDOcpic2 = new System.Windows.Forms.OpenFileDialog();

        string DocAttachment1;
        string DocAttachment2;

        string sourceFile;
        string destinationFile;
        string FileName = string.Empty;

        public string imageDir;
        public string _UserCurrentInfo;

        int cat1Checked = 0;
        int cat2Checked = 0;
        int cat3Checked = 0;
        int cat4Checked = 0;
        int cat5Checked = 0;
        int cat6Checked = 0;
        int cat7Checked = 0;
        int cat8Checked = 0;
        int cat9Checked = 0;
        int cat10Checked = 0;
        int cat11Checked = 0;
        int cat12Checked = 0;
        int cat13Checked = 0;
        int cat14Checked = 0;
        int cat15Checked = 0;
        int cat16Checked = 0;
        int cat17Checked = 0;
        int cat18Checked = 0;
        int cat19Checked = 0;
        int cat20Checked = 0;
        int cat21Checked = 0;
        int cat22Checked = 0;
        int cat23Checked = 0;
        int cat24Checked = 0;
        int cat25Checked = 0;
        int cat26Checked = 0;
        int cat27Checked = 0;
        int cat28Checked = 0;
        int cat29Checked = 0;
        int cat30Checked = 0;
        int cat31Checked = 0;
        int cat32Checked = 0;
        int cat33Checked = 0;
        int cat34Checked = 0;
        int cat35Checked = 0;
        int cat36Checked = 0;
        int cat37Checked = 0;
        int cat38Checked = 0;
        int cat39Checked = 0;
        int cat40Checked = 0;
        int cat41Checked = 0;
        int cat42Checked = 0;
        int cat43Checked = 0;
        int cat44Checked = 0;
        int cat45Checked = 0;
        int cat46Checked = 0;
        int cat47Checked = 0;
        int cat48Checked = 0;
        int cat49Checked = 0;
        int cat50Checked = 0;
        int cat51Checked = 0;
        int cat52Checked = 0;
        int cat53Checked = 0;
        int cat54Checked = 0;
        int cat55Checked = 0;
        int cat56Checked = 0;
        int cat57Checked = 0;
        int cat58Checked = 0;
        int cat59Checked = 0;
        int cat61Checked = 0;

        int SelectedQuestID = 0;
        private string AnswersFound = "N";
        int foundIndex = 0;
        string ValueType = string.Empty;
        string ActionType;
        string ActionDescription;
        string CalcIsBusy;


        public string save;

        private void tableRegister()
        {
            dsGlobal.Tables.Add(dtData);
            dsGlobal.Tables.Add(dtSave);
            dsGlobal.Tables.Add(dtQuestions);
            dsGlobal.Tables.Add(dtSubQuestions);
        }

        private DataTable dtData = new DataTable("dtData");
        private DataTable dtSave = new DataTable("dtSave");
        private DataTable dtQuestions = new DataTable("dtQuestions");
        private DataTable dtSubQuestions = new DataTable("dtSubQuestions");
        private BindingSource bsMainQuestions = new BindingSource();
        private BindingSource bsAnswers = new BindingSource();

        #region DataSet
        public DataSet dsGlobal = new DataSet();
        #endregion

        public RockEngFrm()
        {
            InitializeComponent();
        }

        string s = string.Empty;
        string wp = string.Empty;
        public string ActType = string.Empty;

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]

            s = base64String.Trim().Replace(" ", "+");
            s = base64String.Trim().Replace("_", "/");

            if (s.Length % 4 > 0)
                s = s.PadRight(s.Length + 4 - s.Length % 4, '=');

            decimal aa = s.Length;

            byte[] imageBytes = Convert.FromBase64String(s);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);


            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);


            MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
            _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManImage.SqlStatement = "update [tbl_DPT_RockMechInspection] set picture = '" + s + "' where workplace = '" + WPLbl.EditValue + "' and captweek = '" + WkLbl2.EditValue + "' and captyear = datepart(year,getdate()) ";
            
            _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManImage.ResultsTableName = "Image";
            _dbManImage.ExecuteInstruction();
            
            return image;
        }
        
        void comboBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void RockEngFrm_Load(object sender, EventArgs e)
        {
            

            if (LTO == "Y")
            {
                RCRockEngineering.Visible = false;
                tabCapture.Visible = false;

                LoadReportLTO();
            }
            else
            {
                tabControl.Dock = DockStyle.Fill;
                tabControl.Visible = false;

                pbx2SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx3SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx4SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx5SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx6SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx7SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx8SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx9SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx10SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx11SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx12SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx13SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx14SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx15SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx16SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx17SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx18SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx19SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx20SVG.SvgImage = pbxWhiteSVG.SvgImage;

                pbx21SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx22SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx23SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx24SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx25SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx26SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx27SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx28SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx29SVG.SvgImage = pbxWhiteSVG.SvgImage;

                pbx30SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx31SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx32SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx33SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx34SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx35SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx36SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx37SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx38SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx39SVG.SvgImage = pbxWhiteSVG.SvgImage;

                pbx40SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx41SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx42SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx43SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx44SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx45SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx46SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx47SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx48SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx49SVG.SvgImage = pbxWhiteSVG.SvgImage;

                pbx50SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx51SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx52SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx53SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx54SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx55SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx56SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx57SVG.SvgImage = pbxWhiteSVG.SvgImage;

                Cat2.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat3.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat4.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat5.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat5.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat6.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat7.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat8.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat9.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat10.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat11.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat12.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat13.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat14.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat15.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat16.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat17.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat18.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat19.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat20.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat21.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat22.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat23.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat24.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat25.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat26.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat27.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat28.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat29.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat30.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat31.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat32.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat33.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat34.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat35.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat36.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat37.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat38.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat39.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat40.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat41.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat42.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat43.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat44.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat45.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat46.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat47.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat48.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat49.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat50.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat51.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat52.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat53.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat54.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat55.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat56.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat57.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat58.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat59.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat61.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);

                imageDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir;

                Cat2.SelectedIndex = 0;
                Cat3.SelectedIndex = 0;
                Cat4.SelectedIndex = 0;
                Cat5.SelectedIndex = 0;
                Cat5.SelectedIndex = 0;
                Cat6.SelectedIndex = 0;
                Cat7.SelectedIndex = 0;
                Cat8.SelectedIndex = 0;
                Cat9.SelectedIndex = 0;
                Cat10.SelectedIndex = 0;
                Cat11.SelectedIndex = 0;
                Cat12.SelectedIndex = 0;
                Cat13.SelectedIndex = 0;
                Cat14.SelectedIndex = 0;
                Cat15.SelectedIndex = 0;
                Cat16.SelectedIndex = 0;
                Cat17.SelectedIndex = 0;
                Cat18.SelectedIndex = 0;
                Cat19.SelectedIndex = 0;
                Cat20.SelectedIndex = 0;
                Cat21.SelectedIndex = 0;
                Cat22.SelectedIndex = 0;
                Cat23.SelectedIndex = 0;
                Cat24.SelectedIndex = 0;
                Cat25.SelectedIndex = 0;
                Cat26.SelectedIndex = 0;
                Cat27.SelectedIndex = 0;
                Cat28.SelectedIndex = 0;
                Cat29.SelectedIndex = 0;
                Cat30.SelectedIndex = 0;
                Cat31.SelectedIndex = 0;
                Cat32.SelectedIndex = 0;
                Cat33.SelectedIndex = 0;
                Cat34.SelectedIndex = 0;
                Cat35.SelectedIndex = 0;
                Cat36.SelectedIndex = 0;
                Cat37.SelectedIndex = 0;
                Cat38.SelectedIndex = 0;
                Cat39.SelectedIndex = 0;
                Cat40.SelectedIndex = 0;
                Cat41.SelectedIndex = 0;
                Cat42.SelectedIndex = 0;
                Cat43.SelectedIndex = 0;
                Cat44.SelectedIndex = 0;
                Cat45.SelectedIndex = 0;
                Cat46.SelectedIndex = 0;
                Cat47.SelectedIndex = 0;
                Cat48.SelectedIndex = 0;
                Cat49.SelectedIndex = 0;
                Cat50.SelectedIndex = 0;
                Cat51.SelectedIndex = 0;
                Cat52.SelectedIndex = 0;
                Cat53.SelectedIndex = 0;
                Cat54.SelectedIndex = 0;
                Cat55.SelectedIndex = 0;
                Cat56.SelectedIndex = 0;
                Cat57.SelectedIndex = 0;
                Cat58.SelectedIndex = 0;
                Cat59.SelectedIndex = 0;
                Cat61.SelectedIndex = 0;

                if (EditLbl.Text != "Y")
                {
                    tabControl.TabPages.Remove(tabCapture);
                    RCRockEngineering.Visible = false;
                }

                tabControl.Visible = true;

                gvCapture.OptionsView.ShowGroupPanel = false;
                tabControl.TabPages.Remove(tabCapture);

                tableRegister();

                if (save == "Y")
                {
                    InsertBlanks();
                }
                
                LoadData();
                LoadQuestions();
                LoadResponsible();
                LoadReport();
                tabControl.Visible = true;
            }
        }

        private void InsertBlanks()
        {
            string sql = string.Empty;

            MWDataManager.clsDataAccess _DataSave = new MWDataManager.clsDataAccess();
            _DataSave.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _DataSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _DataSave.queryReturnType = MWDataManager.ReturnType.DataTable;

            sql = sql + "Insert Into tbl_DPT_RockMechInspection (Workplace, CaptYear, CaptWeek, ActWeek, CaptDate, WPASuser, RespSB, RespMO, RiskRating,Cat1Checked, Cat1Note, Cat2Checked, Cat2Note, Cat3Checked, Cat3Note, Cat4Checked, Cat4Note, Cat5Checked, Cat5Note,Cat6Checked, Cat6Note, Cat7Checked,Cat7Note,Cat8Checked,Cat8Note, Cat9Checked, Cat9Note, Cat10Checked,Cat10Note,Cat11Checked, Cat11Note, Cat12Checked, Cat12Note, Cat13Checked, Cat13Note, Cat14Checked, Cat14Note,Cat15Checked, Cat15Note,Cat16Checked, Cat16Note, Cat17Checked, Cat17Note, Cat18Checked, Cat18Note, Cat19Checked, Cat19Note, Cat20Checked, Cat20Note,Cat21Checked, Cat21Note, Cat22Checked, Cat22Note, Cat23Checked, Cat23Note,Cat24Checked, Cat24Note, Cat25Checked, Cat25Note,Cat26Checked, Cat26Note, Cat27Checked, Cat27Note, Cat28Checked, Cat28Note, Cat29Checked, Cat29Note, Cat30Checked, Cat30Note,Cat31Checked, Cat31Note, Cat32Checked, Cat32Note, Cat33Checked, Cat33Note, Cat34Checked, Cat34Note, Cat35Checked, Cat35Note,Cat36Checked, Cat36Note, Cat37Checked, Cat37Note, Cat38Checked, Cat38Note, Cat39Checked, Cat39Note, Cat40Checked, Cat40Note,Cat41Checked, Cat41Note, Cat42Checked, Cat42Note, Cat43Checked, Cat43Note, Cat44Checked, Cat44Note, Cat45Checked, Cat45Note,Cat46Checked, Cat46Note, Cat47Checked, Cat47Note, Cat48Checked, Cat48Note, Cat49Checked, Cat49Note, Cat50Checked, Cat50Note,Cat51Checked, Cat51Note, Cat52Checked, Cat52Note, Cat53Checked, Cat53Note, Cat54Checked, Cat54Note, Cat55Checked, Cat55Note,Cat56Checked, Cat56Note, Cat57Checked, Cat57Note, Cat58Checked, Cat58Note, Cat59Checked, Cat59Note, Cat60Checked, Cat60Note,Cat61Checked, Cat61Note, Cat62Checked, Cat62Note, Cat63Checked, Cat63Note, Picture, GeneralComments, WPStatus, Document1, Document2, Date, Cat64Checked, Cat64Note,Cat1Observation, Cat2Observation, Cat3Observation, Cat4Observation, Cat5Observation,Cat6Observation, Cat7Observation, Cat8Observation, Cat9Observation, Cat10Observation,Cat11Observation, Cat12Observation, Cat13Observation, Cat14Observation, Cat15Observation,Cat16Observation, Cat17Observation, Cat18Observation, Cat19Observation, Cat20Observation,Cat21Observation, Cat22Observation, Cat23Observation, Cat24Observation, Cat25Observation,Cat26Observation, Cat27Observation, Cat28Observation, Cat29Observation, Cat30Observation,Cat31Observation, Cat32Observation, Cat33Observation, Cat34Observation, Cat35Observation,Cat36Observation, Cat37Observation, Cat38Observation, Cat39Observation, Cat40Observation,Cat41Observation, Cat42Observation, Cat43Observation, Cat44Observation, Cat45Observation,Cat46Observation, Cat47Observation, Cat48Observation, Cat49Observation, Cat50Observation,Cat51Observation, Cat52Observation, Cat53Observation, Cat54Observation, Cat55Observation,Cat56Observation, Cat57Observation, Cat58Observation, Cat59Observation, Cat60Observation,Cat61Observation, Cat62Observation, Cat63Observation, Cat64Observation, PegToFace) Values ( \r\n" +
                            " '" + WPLbl.EditValue + "', datepart(YYYY,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "'),  '" + WkLbl2.EditValue + "', '" + WkLbl2.EditValue + "'  , getdate() , '" + TUserInfo.UserID + "' , \r\n" +
                            " '' , '' , '" + RRlabel.EditValue + "', \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '' ,  '',  \r\n" +
                            " '', '','', '', \r\n" +
                            " '', '', \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '', '',  \r\n" +
                            " '', '', '', '', '' \r\n" +
                            " ) ";

            _DataSave.SqlStatement = sql;

            var result = _DataSave.ExecuteInstruction();
        }

        private void LoadData()
        {
            string sql = string.Empty;
            //sql = "EXEC sp_DPT_RockMechInspections_Questions '"+ WPLbl.EditValue.ToString() +"', '"+ WkLbl.Text +"' ";
            sql = "select * from vw_RockEng_Data where Workplace = '" + WPLbl.EditValue.ToString() + "' and CaptWeek = '" + WkLbl.Text + "' order by Convert(Decimal(18,2),OrderBy) ";
            sqlConnector(sql, "dtData");

            gcCapture.DataSource = null;
            gvCapture.OptionsView.ShowGroupPanel = false;

            gcCapture.DataSource = dtData;
            gcQuestID.FieldName = "QuestID";
            gcQuest.FieldName = "Question";
            gcCat.FieldName = "QuestionSubCat";
            gcAnswer.FieldName = "Answer";
            gcInstruction.FieldName = "Note";
            gcObservation.FieldName = "Observation";

            if (dtData.Rows.Count > 0)
            {

                cmbSB.EditValue = dtData.Rows[0]["RespSB"].ToString();
                cmbMO.EditValue = dtData.Rows[0]["RespMO"].ToString();
                txtPeg.EditValue = dtData.Rows[0]["PegToFace"].ToString();
                tbDate.EditValue = dtData.Rows[0]["CaptDate"].ToString();
            }

        }

        private void sqlConnector(string sqlQuery, string sqlTableIdentifier)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _sqlConnection.SqlStatement = sqlQuery;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ResultsTableName = sqlTableIdentifier;
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;


            if (!string.IsNullOrEmpty(sqlTableIdentifier))
            {
                for (int i = 0; i < dsGlobal.Tables.Count; i++)
                {
                    if (dsGlobal.Tables[i].TableName == sqlTableIdentifier)
                    {
                        dsGlobal.Tables[i].Clear();
                        dsGlobal.Tables[i].Merge(dtReceive);
                    }
                }
            }
        }

        //Get Questions
        private void LoadQuestions()
        {
            string sql = string.Empty;
            sql = "select * from tbl_DPT_RockMechInspection_Questions";
            sqlConnector(sql, "dtQuestions");

            bsQuestions.DataSource = dtQuestions;
            bsMainQuestions.DataSource = dtQuestions;

            repEngMainQuest.DataSource = bsAnswers;
            repEngMainQuest.ForceInitialize();
            repEngMainQuest.PopulateColumns();
            repEngMainQuest.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("QuestID", "ID", 20));
            repEngMainQuest.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Question", "Question", 100));
            repEngMainQuest.DisplayMember = "Question";
            repEngMainQuest.ValueMember = "QuestID";

        }

        private void LoadSubQuestions()
        {
            string sql = string.Empty;
            sql = "select * from tbl_DPT_RockMechInspection_SubQuestions";
            sqlConnector(sql, "dtSubQuestions");
            bsSubQuestions.DataSource = dtSubQuestions;
            bsAnswers.DataSource = dtSubQuestions;

            repRockEngSubQuestEdit.DataSource = bsAnswers;
            repEngMainQuest.ForceInitialize();
            repEngMainQuest.PopulateColumns();
            repEngMainQuest.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Question", "Question", 100));
            repRockEngSubQuestEdit.DisplayMember = "Question";
            repRockEngSubQuestEdit.ValueMember = "Question";

        }

        void LoadResponsible()
        {
            //Search Edit
            MWDataManager.clsDataAccess _dbManSE = new MWDataManager.clsDataAccess();
            _dbManSE.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            if (TGlobalItems.Client == "Amplats")
            {
                _dbManSE.SqlStatement = "select UserID,Name,LastName from [Syncromine_New].[dbo].tblUsers order by Name";
            }
            else
            {
                _dbManSE.SqlStatement = "select UserID,Name,LastName from Syncromine_New.dbo.tblUsers order by Name";
            }

            _dbManSE.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSE.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSE.ResultsTableName = "Graph";
            _dbManSE.ExecuteInstruction();

            DataTable dt24 = _dbManSE.ResultsDataTable;
            DataSet ds24 = new DataSet();
            if (ds24.Tables.Count > 0)
                ds24.Tables.Clear();
            ds24.Tables.Add(dt24);

            lookupEditSB.DataSource = ds24.Tables[0];
            lookupEditSB.DisplayMember = "UserID";
            lookupEditSB.ValueMember = "UserID";

            lookupEditMO.DataSource = ds24.Tables[0];
            lookupEditMO.DisplayMember = "UserID";
            lookupEditMO.ValueMember = "UserID";
        }

        void LoadReportLTO()
        {
            Cursor = Cursors.WaitCursor;

            MWDataManager.clsDataAccess _dbManWPST21 = new MWDataManager.clsDataAccess();
            _dbManWPST21.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManWPST21.SqlStatement = "select * from  tbl_LicenceToOperate_Seismic where wpdescription = '" + WPLbl.EditValue + "' order by thedate desc";

            _dbManWPST21.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPST21.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPST21.ResultsTableName = "Graph";
            _dbManWPST21.ExecuteInstruction();

            DataSet dsABS111 = new DataSet();
            dsABS111.Tables.Add(_dbManWPST21.ResultsDataTable);

            if (EditLbl.Text == "Y")
            {
                //if (_dbManWPST21.ResultsDataTable.Rows.Count > 0)
                //Cat24Txt.Text = _dbManWPST21.ResultsDataTable.Rows[0]["risk"].ToString();
            }

            theReport3.RegisterData(dsABS111);

            MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
            _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManWPST2.SqlStatement = "select top (20) * from (   \r\n" +
                                        "Select 'Z' bb,  \r\n" +
                                        "case when targetdate is null then 'Not Accepted'  \r\n" +
                                        "when targetdate is not null then 'Accepted'   \r\n" +
                                        " when CompletionDate is not null then 'Completed'  \r\n" +
                                        " when VerificationDate is not null then 'Verified' else '' end as ActionStatus   \r\n" +
                                        " ,[description] as Action, thedate datesubmitted, datediff(day, thedate, getdate()) ss   \r\n" +
                                        " from [dbo].[tbl_Shec_Incidents]  \r\n" +
                                         "       where workplace = '" + WPLbl.EditValue + "'   \r\n" +

                                        " union all \r\n" +
                                        " select 'a' , '', '', null, '' \r\n" +
                                        " union all \r\n" +
                                        " select 'b ', '', '', null, '' \r\n" +
                                        " union all \r\n" +
                                        " select 'c  ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'd   ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'e    ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'f     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'g     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'h     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'i     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'j     ' , '', '', null, '' \r\n" +
                                        " )a \r\n" +
                                        "  order  by bb  desc,datesubmitted \r\n";
            _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPST2.ResultsTableName = "Table2";
            _dbManWPST2.ExecuteInstruction();

            DataSet dsABS1 = new DataSet();
            dsABS1.Tables.Add(_dbManWPST2.ResultsDataTable);

            theReport3.RegisterData(dsABS1);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, " +
                                " RiskRating Risk, '" + RRlabel.EditValue + "' rr,  * from [tbl_DPT_RockMechInspection] " +
                                 "  where workplace = '" + WPLbl.EditValue + "'\r\n" +
                                "   and captweek = (select max(CaptWeek)\r\n" +
                                 "  from [tbl_DPT_RockMechInspection]\r\n" +
                                 "  where workplace = '" + WPLbl.EditValue + "') \r\n" +
                                 "  and captyear = (select max(CaptYear)\r\n" +
                                "   from[tbl_DPT_RockMechInspection]\r\n" +
                                 "  where workplace = '" + WPLbl.EditValue + "' and captweek = (select max(CaptWeek)\r\n" +
                                 "  from [tbl_DPT_RockMechInspection]\r\n" +
                                "   where workplace = '" + WPLbl.EditValue + "') )  \r\n";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "DevSummary";
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {               

                string BlankImage = Application.StartupPath + "\\" + "Neil.bmp";

                MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
               
                _dbManImage.SqlStatement = "Select RiskRating Risk,'" + BlankImage + "' pp, Picture,document1,document2  \r\n" +
                                    "from [tbl_DPT_RockMechInspection] \r\n" +
                                     "  where workplace = '" + WPLbl.EditValue + "'\r\n" +
                                    "   and captweek = (select max(CaptWeek)\r\n" +
                                     "  from[tbl_DPT_RockMechInspection]\r\n" +
                                     "  where workplace = '" + WPLbl.EditValue + "') \r\n" +
                                     "  and captyear = (select max(CaptYear)\r\n" +
                                    "   from[tbl_DPT_RockMechInspection]\r\n" +
                                     "  where workplace = '" + WPLbl.EditValue + "' and captweek = (select max(CaptWeek)\r\n" +
                                     "  from[tbl_DPT_RockMechInspection]\r\n" +
                                    "   where workplace = '" + WPLbl.EditValue + "') )  \r\n";
                _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManImage.ResultsTableName = "Image";
                _dbManImage.ExecuteInstruction();

                DataSet ReportDatasetReport = new DataSet();
                ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReport);

                DataSet ReportDatasetReportImage = new DataSet();
                ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReportImage);

                theReport3.Load(ReportsFolder + "\\RockEng.frx");

                //theReport3.Design();

                pcReport.Clear();
                theReport3.Prepare();
                theReport3.Preview = pcReport;
                theReport3.ShowPrepared();
                tabControl.Visible = true;
                tabCapture.PageVisible = false;
                Cursor = Cursors.Default;

            }
        }

        void LoadReport()
        {
            Cursor = Cursors.WaitCursor;

            MWDataManager.clsDataAccess _dbManGraph = new MWDataManager.clsDataAccess();
            _dbManGraph.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManGraph.SqlStatement = " Select top(4) * from tbl_DPT_RockMechInspection where Workplace = '" + WPLbl.EditValue + "' order by CaptWeek asc ";

            _dbManGraph.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGraph.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGraph.ResultsTableName = "RRGraph";
            _dbManGraph.ExecuteInstruction();

            DataSet dsGraph = new DataSet();
            dsGraph.Tables.Add(_dbManGraph.ResultsDataTable);

            theReport3.RegisterData(dsGraph);

            MWDataManager.clsDataAccess _dbManWPST21 = new MWDataManager.clsDataAccess();
            _dbManWPST21.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManWPST21.SqlStatement = "select * from  tbl_LicenceToOperate_Seismic where wpdescription = '" + WPLbl.EditValue + "' order by thedate desc";

            _dbManWPST21.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPST21.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPST21.ResultsTableName = "Graph";
            _dbManWPST21.ExecuteInstruction();

            DataSet dsABS111 = new DataSet();
            dsABS111.Tables.Add(_dbManWPST21.ResultsDataTable);

            if (EditLbl.Text == "Y")
            {
                //if (_dbManWPST21.ResultsDataTable.Rows.Count > 0)
                //Cat24Txt.Text = _dbManWPST21.ResultsDataTable.Rows[0]["risk"].ToString();
            }

            theReport3.RegisterData(dsABS111);

            MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
            _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManWPST2.SqlStatement = "select top (20) * from (   \r\n" +
                                        "Select 'Z' bb,  \r\n" +
                                        "case when targetdate is null then 'Not Accepted'  \r\n" +
                                        "when targetdate is not null then 'Accepted'   \r\n" +
                                        " when CompletionDate is not null then 'Completed'  \r\n" +
                                        " when VerificationDate is not null then 'Verified' else '' end as ActionStatus   \r\n" +
                                        " ,[description] as Action, thedate datesubmitted, datediff(day, thedate, getdate()) ss   \r\n" +
                                        " from[dbo].[tbl_Shec_Incidents]  \r\n" +
                                         "       where workplace = '" + WPLbl.EditValue + "'   \r\n" +

                                        " union all \r\n" +
                                        " select 'a' , '', '', null, '' \r\n" +
                                        " union all \r\n" +
                                        " select 'b ', '', '', null, '' \r\n" +
                                        " union all \r\n" +
                                        " select 'c  ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'd   ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'e    ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'f     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'g     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'h     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'i     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'j     ' , '', '', null, '' \r\n" +
                                        " )a \r\n" +
                                        "  order  by bb  desc,datesubmitted \r\n";
            _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPST2.ResultsTableName = "Table2";
            _dbManWPST2.ExecuteInstruction();

            DataSet dsABS1 = new DataSet();
            dsABS1.Tables.Add(_dbManWPST2.ResultsDataTable);

            theReport3.RegisterData(dsABS1);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, RiskRating Risk, '" + RRlabel.EditValue + "' rr,  * from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' and CaptYear = datepart(YYYY,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "') and CaptWeek = '" + WkLbl2.EditValue + "'  ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "DevSummary";
            _dbMan.ExecuteInstruction();


            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {
                //cmbSB.EditValue = _dbMan.ResultsDataTable.Rows[0]["RespSB"].ToString();
                //cmbMO.EditValue = _dbMan.ResultsDataTable.Rows[0]["RespMO"].ToString();

                //if (_dbMan.ResultsDataTable.Rows[0]["Cat1Checked"].ToString().Substring(0, 1) == "A")
                //{
                //    Cat1A.Checked = true;
                //}

                //if (_dbMan.ResultsDataTable.Rows[0]["Cat1Checked"].ToString().Substring(0, 1) == "B")
                //{
                //    Cat1B.Checked = true;
                //}

                //if (_dbMan.ResultsDataTable.Rows[0]["Cat1Checked"].ToString().Substring(0, 1) == "S")
                //{
                //    Cat1S.Checked = true;
                //}

                //if (_dbMan.ResultsDataTable.Rows[0]["Cat1Checked"].ToString().Substring(1, 1) == "Y")
                //{
                //    Cat1P.Checked = true;
                //}
                //else
                //{
                //    Cat1P.Checked = false;
                //}


                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "0")
                {
                    Cat2.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "1")
                {
                    Cat2.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "2")
                {
                    Cat2.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "3")
                {
                    Cat2.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "4")
                {
                    Cat2.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "0")
                {
                    Cat3.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "1")
                {
                    Cat3.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "2")
                {
                    Cat3.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "3")
                {
                    Cat3.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "4")
                {
                    Cat3.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "0")
                {
                    Cat4.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "1")
                {
                    Cat4.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "2")
                {
                    Cat4.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "3")
                {
                    Cat4.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "4")
                {
                    Cat4.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "0")
                {
                    Cat5.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "1")
                {
                    Cat5.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "2")
                {
                    Cat5.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "3")
                {
                    Cat5.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "4")
                {
                    Cat5.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "0")
                {
                    Cat6.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "1")
                {
                    Cat6.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "2")
                {
                    Cat6.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "3")
                {
                    Cat6.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "4")
                {
                    Cat6.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "0")
                {
                    Cat7.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "1")
                {
                    Cat7.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "2")
                {
                    Cat7.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "3")
                {
                    Cat7.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "4")
                {
                    Cat7.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "0")
                {
                    Cat8.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "1")
                {
                    Cat8.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "2")
                {
                    Cat8.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "3")
                {
                    Cat8.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "4")
                {
                    Cat8.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "0")
                {
                    Cat9.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "1")
                {
                    Cat9.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "2")
                {
                    Cat9.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "3")
                {
                    Cat9.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "4")
                {
                    Cat9.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "0")
                {
                    Cat10.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "1")
                {
                    Cat10.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "2")
                {
                    Cat10.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "3")
                {
                    Cat10.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "4")
                {
                    Cat10.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "0")
                {
                    Cat11.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "1")
                {
                    Cat11.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "2")
                {
                    Cat11.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "3")
                {
                    Cat11.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "4")
                {
                    Cat11.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "0")
                {
                    Cat12.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "1")
                {
                    Cat12.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "2")
                {
                    Cat12.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "3")
                {
                    Cat12.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "4")
                {
                    Cat12.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "0")
                {
                    Cat13.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "1")
                {
                    Cat13.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "2")
                {
                    Cat13.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "3")
                {
                    Cat13.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "4")
                {
                    Cat13.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "0")
                {
                    Cat14.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "1")
                {
                    Cat14.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "2")
                {
                    Cat14.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "3")
                {
                    Cat14.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "4")
                {
                    Cat14.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "0")
                {
                    Cat15.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "1")
                {
                    Cat15.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "2")
                {
                    Cat15.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "3")
                {
                    Cat15.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "4")
                {
                    Cat15.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "0")
                {
                    Cat16.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "1")
                {
                    Cat16.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "2")
                {
                    Cat16.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "3")
                {
                    Cat16.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "4")
                {
                    Cat16.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "0")
                {
                    Cat17.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "1")
                {
                    Cat17.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "2")
                {
                    Cat17.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "3")
                {
                    Cat17.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "4")
                {
                    Cat17.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "0")
                {
                    Cat18.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "1")
                {
                    Cat18.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "2")
                {
                    Cat18.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "3")
                {
                    Cat18.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "4")
                {
                    Cat18.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "0")
                {
                    Cat19.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "1")
                {
                    Cat19.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "2")
                {
                    Cat19.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "3")
                {
                    Cat19.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "4")
                {
                    Cat19.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "0")
                {
                    Cat20.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "1")
                {
                    Cat20.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "2")
                {
                    Cat20.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "3")
                {
                    Cat20.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "4")
                {
                    Cat20.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "0")
                {
                    Cat21.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "1")
                {
                    Cat21.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "2")
                {
                    Cat21.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "3")
                {
                    Cat21.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "4")
                {
                    Cat21.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "0")
                {
                    Cat22.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "1")
                {
                    Cat22.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "2")
                {
                    Cat22.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "3")
                {
                    Cat22.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "4")
                {
                    Cat22.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "0")
                {
                    Cat23.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "1")
                {
                    Cat23.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "2")
                {
                    Cat23.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "3")
                {
                    Cat23.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "4")
                {
                    Cat23.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "0")
                {
                    Cat24.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "1")
                {
                    Cat24.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "2")
                {
                    Cat24.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "3")
                {
                    Cat24.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "4")
                {
                    Cat24.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "0")
                {
                    Cat25.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "1")
                {
                    Cat25.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "2")
                {
                    Cat25.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "3")
                {
                    Cat25.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "4")
                {
                    Cat25.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "0")
                {
                    Cat26.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "1")
                {
                    Cat26.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "2")
                {
                    Cat26.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "3")
                {
                    Cat26.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "4")
                {
                    Cat26.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "0")
                {
                    Cat27.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "1")
                {
                    Cat27.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "2")
                {
                    Cat27.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "3")
                {
                    Cat27.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "4")
                {
                    Cat27.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "0")
                {
                    Cat28.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "1")
                {
                    Cat28.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "2")
                {
                    Cat28.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "3")
                {
                    Cat28.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "4")
                {
                    Cat28.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "0")
                {
                    Cat29.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "1")
                {
                    Cat29.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "2")
                {
                    Cat29.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "3")
                {
                    Cat29.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "4")
                {
                    Cat29.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "0")
                {
                    Cat30.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "1")
                {
                    Cat30.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "2")
                {
                    Cat30.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "3")
                {
                    Cat30.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "4")
                {
                    Cat30.SelectedIndex = 4;
                }
               


                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "0")
                {
                    Cat31.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "1")
                {
                    Cat31.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "2")
                {
                    Cat31.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "3")
                {
                    Cat31.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "4")
                {
                    Cat31.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "0")
                {
                    Cat32.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "1")
                {
                    Cat32.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "2")
                {
                    Cat32.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "3")
                {
                    Cat32.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "4")
                {
                    Cat32.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "0")
                {
                    Cat33.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "1")
                {
                    Cat33.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "2")
                {
                    Cat33.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "3")
                {
                    Cat33.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "4")
                {
                    Cat33.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "0")
                {
                    Cat34.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "1")
                {
                    Cat34.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "2")
                {
                    Cat34.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "3")
                {
                    Cat34.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "4")
                {
                    Cat34.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "0")
                {
                    Cat35.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "1")
                {
                    Cat35.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "2")
                {
                    Cat35.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "3")
                {
                    Cat35.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "4")
                {
                    Cat35.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "0")
                {
                    Cat36.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "1")
                {
                    Cat36.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "2")
                {
                    Cat36.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "3")
                {
                    Cat36.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "4")
                {
                    Cat36.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "0")
                {
                    Cat37.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "1")
                {
                    Cat37.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "2")
                {
                    Cat37.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "3")
                {
                    Cat37.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "4")
                {
                    Cat37.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "0")
                {
                    Cat38.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "1")
                {
                    Cat38.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "2")
                {
                    Cat38.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "3")
                {
                    Cat38.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "4")
                {
                    Cat38.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "0")
                {
                    Cat39.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "1")
                {
                    Cat39.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "2")
                {
                    Cat39.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "3")
                {
                    Cat39.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "4")
                {
                    Cat39.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "0")
                {
                    Cat40.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "1")
                {
                    Cat40.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "2")
                {
                    Cat40.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "3")
                {
                    Cat40.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "4")
                {
                    Cat40.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "0")
                {
                    Cat41.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "1")
                {
                    Cat41.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "2")
                {
                    Cat41.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "3")
                {
                    Cat41.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "4")
                {
                    Cat41.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "0")
                {
                    Cat42.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "1")
                {
                    Cat42.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "2")
                {
                    Cat42.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "3")
                {
                    Cat42.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "4")
                {
                    Cat42.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "0")
                {
                    Cat43.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "1")
                {
                    Cat43.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "2")
                {
                    Cat43.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "3")
                {
                    Cat43.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "4")
                {
                    Cat43.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "0")
                {
                    Cat44.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "1")
                {
                    Cat44.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "2")
                {
                    Cat44.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "3")
                {
                    Cat44.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "4")
                {
                    Cat44.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "0")
                {
                    Cat45.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "1")
                {
                    Cat45.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "2")
                {
                    Cat45.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "3")
                {
                    Cat45.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "4")
                {
                    Cat45.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "0")
                {
                    Cat46.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "1")
                {
                    Cat46.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "2")
                {
                    Cat46.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "3")
                {
                    Cat46.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "4")
                {
                    Cat46.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "0")
                {
                    Cat47.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "1")
                {
                    Cat47.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "2")
                {
                    Cat47.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "3")
                {
                    Cat47.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "4")
                {
                    Cat47.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "0")
                {
                    Cat48.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "1")
                {
                    Cat48.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "2")
                {
                    Cat48.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "3")
                {
                    Cat48.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "4")
                {
                    Cat48.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "0")
                {
                    Cat49.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "1")
                {
                    Cat49.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "2")
                {
                    Cat49.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "3")
                {
                    Cat49.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "4")
                {
                    Cat49.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "0")
                {
                    Cat50.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "1")
                {
                    Cat50.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "2")
                {
                    Cat50.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "3")
                {
                    Cat50.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "4")
                {
                    Cat50.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "0")
                {
                    Cat51.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "1")
                {
                    Cat51.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "2")
                {
                    Cat51.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "3")
                {
                    Cat51.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "4")
                {
                    Cat51.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "0")
                {
                    Cat52.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "1")
                {
                    Cat52.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "2")
                {
                    Cat52.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "3")
                {
                    Cat52.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "4")
                {
                    Cat52.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "0")
                {
                    Cat53.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "1")
                {
                    Cat53.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "2")
                {
                    Cat53.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "3")
                {
                    Cat53.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "4")
                {
                    Cat53.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "0")
                {
                    Cat54.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "1")
                {
                    Cat54.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "2")
                {
                    Cat54.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "3")
                {
                    Cat54.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "4")
                {
                    Cat54.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "0")
                {
                    Cat55.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "1")
                {
                    Cat55.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "2")
                {
                    Cat55.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "3")
                {
                    Cat55.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "4")
                {
                    Cat55.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "0")
                {
                    Cat56.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "1")
                {
                    Cat56.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "2")
                {
                    Cat56.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "3")
                {
                    Cat56.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "4")
                {
                    Cat56.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "0")
                {
                    Cat57.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "1")
                {
                    Cat57.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "2")
                {
                    Cat57.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "3")
                {
                    Cat57.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "4")
                {
                    Cat57.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "0")
                {
                    Cat58.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "1")
                {
                    Cat58.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "2")
                {
                    Cat58.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "3")
                {
                    Cat58.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "4")
                {
                    Cat58.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "0")
                {
                    Cat59.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "1")
                {
                    Cat59.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "2")
                {
                    Cat59.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "3")
                {
                    Cat59.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "4")
                {
                    Cat59.SelectedIndex = 4;
                }

                //if (_dbMan.ResultsDataTable.Rows[0]["Cat60Checked"].ToString() == "0")
                //{
                //    Cat60.SelectedIndex = 0;
                //}
                //if (_dbMan.ResultsDataTable.Rows[0]["Cat60Checked"].ToString() == "1")
                //{
                //    Cat60.SelectedIndex = 1;
                //}
                //if (_dbMan.ResultsDataTable.Rows[0]["Cat60Checked"].ToString() == "2")
                //{
                //    Cat60.SelectedIndex = 2;
                //}
                //if (_dbMan.ResultsDataTable.Rows[0]["Cat60Checked"].ToString() == "3")
                //{
                //    Cat60.SelectedIndex = 3;
                //}
                //if (_dbMan.ResultsDataTable.Rows[0]["Cat60Checked"].ToString() == "4")
                //{
                //    Cat60.SelectedIndex = 4;
                //}
                
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "0")
                {
                    Cat61.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "1")
                {
                    Cat61.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "2")
                {
                    Cat61.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "3")
                {
                    Cat61.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "4")
                {
                    Cat61.SelectedIndex = 4;
                }

                for (int i = 0; i < Cat58.Items.Count; i++)
                {
                    if (Cat58.Items[i].ToString() == _dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString())
                    {
                        Cat58.SelectedIndex = i;
                    }
                }

                for (int i = 0; i < Cat59.Items.Count; i++)
                {
                    if (Cat59.Items[i].ToString() == _dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString())
                    {
                        Cat59.SelectedIndex = i;
                    }
                }
                
                //Cat60.Value = Convert.ToInt32(_dbMan.ResultsDataTable.Rows[0]["Cat60Checked"].ToString());
                
                for (int i = 0; i < Cat61.Items.Count; i++)
                {
                    if (Cat61.Items[i].ToString() == _dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString())
                    {
                        Cat61.SelectedIndex = i;
                    }
                }
                
                Cat1Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat1Note"].ToString();
                Cat2Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat2Note"].ToString();
                Cat3Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat3Note"].ToString();
                Cat4Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat4Note"].ToString();
                Cat5Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat5Note"].ToString();
                Cat6Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat6Note"].ToString();
                Cat7Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat7Note"].ToString();
                Cat8Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat8Note"].ToString();
                Cat9Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat9Note"].ToString();
                Cat10Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat10Note"].ToString();
                Cat11Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat11Note"].ToString();
                Cat12Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat12Note"].ToString();
                Cat13Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat13Note"].ToString();
                Cat14Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat14Note"].ToString();
                Cat15Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat15Note"].ToString();
                Cat16Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat16Note"].ToString();
                Cat17Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat17Note"].ToString();
                Cat18Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat18Note"].ToString();
                Cat19Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat19Note"].ToString();
                Cat20Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat20Note"].ToString();
                Cat21Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat21Note"].ToString();
                Cat22Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat22Note"].ToString();
                Cat23Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat23Note"].ToString();
                Cat24Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat24Note"].ToString();
                Cat25Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat25Note"].ToString();
                Cat26Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat26Note"].ToString();
                Cat27Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat27Note"].ToString();
                Cat28Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat28Note"].ToString();
                Cat29Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat29Note"].ToString();
                Cat30Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat30Note"].ToString();
                Cat31Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat31Note"].ToString();
                Cat32Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat32Note"].ToString();
                Cat33Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat33Note"].ToString();
                Cat34Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat34Note"].ToString();
                Cat35Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat35Note"].ToString();
                Cat36Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat36Note"].ToString();
                Cat37Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat37Note"].ToString();
                Cat38Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat38Note"].ToString();
                Cat39Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat39Note"].ToString();
                Cat40Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat40Note"].ToString();
                Cat41Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat41Note"].ToString();
                Cat42Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat42Note"].ToString();
                Cat43Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat43Note"].ToString();
                Cat44Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat44Note"].ToString();
                Cat45Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat45Note"].ToString();
                Cat46Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat46Note"].ToString();
                Cat47Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat47Note"].ToString();
                Cat48Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat48Note"].ToString();
                Cat49Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat49Note"].ToString();
                Cat50Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat50Note"].ToString();
                Cat51Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat51Note"].ToString();
                Cat52Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat52Note"].ToString();
                Cat53Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat53Note"].ToString();
                Cat54Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat54Note"].ToString();
                Cat55Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat55Note"].ToString();
                Cat56Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat56Note"].ToString();
                Cat57Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat57Note"].ToString();
                Cat58Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat58Note"].ToString();
                Cat59Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat59Note"].ToString();
                Cat60Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat60Note"].ToString();
                Cat61Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat61Note"].ToString();
                Cat62Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat62Note"].ToString();
                Cat63Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat63Note"].ToString();

                Cat62.Text = _dbMan.ResultsDataTable.Rows[0]["Cat62Checked"].ToString();
                Cat63.Text = _dbMan.ResultsDataTable.Rows[0]["Cat63Checked"].ToString();

                txtGeneralComments.Text = _dbMan.ResultsDataTable.Rows[0]["GeneralComments"].ToString();

                DocAttachment1 = _dbMan.ResultsDataTable.Rows[0]["Document1"].ToString();
                DocAttachment2 = _dbMan.ResultsDataTable.Rows[0]["Document2"].ToString();

                txtAttachment.Text = _dbMan.ResultsDataTable.Rows[0]["picture"].ToString();

                if (_dbMan.ResultsDataTable.Rows[0]["WPASuser"].ToString() == "Tablet")
                {
                    if (_dbMan.ResultsDataTable.Rows[0]["picture"].ToString() != string.Empty)
                    {
                        PicBox.Image = Base64ToImage(_dbMan.ResultsDataTable.Rows[0]["picture"].ToString());
                    }

                    PicBox.Image.Save(Application.StartupPath + "\\" + "Neil.bmp");
                }
                else
                {
                    PicBox.ImageLocation = _dbMan.ResultsDataTable.Rows[0]["picture"].ToString();
                }

                DocPic1.ImageLocation = _dbMan.ResultsDataTable.Rows[0]["Document1"].ToString();
                DocPic2.ImageLocation = _dbMan.ResultsDataTable.Rows[0]["Document2"].ToString();

                //if (_dbMan.ResultsDataTable.Rows[0]["WPStatus"].ToString() == "Green")
                //{
                //    GreenCheck.Checked = true;
                //}

                //if (_dbMan.ResultsDataTable.Rows[0]["WPStatus"].ToString() == "Orange")
                //{
                //    OrangeCheck.Checked = true;
                //}

                //if (_dbMan.ResultsDataTable.Rows[0]["WPStatus"].ToString() == "Red")
                //{
                //    RedCheck.Checked = true;
                //}

                string BlankImage = Application.StartupPath + "\\" + "Neil.bmp";

                //MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                //_dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);

                //_dbManImage.SqlStatement = "Select RiskRating Risk,'" + BlankImage + "' pp, Picture,document1,document2 from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' and captweek = '" + WkLbl2.EditValue + "' and captyear = (select max(CaptYear) from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' and captweek = '" + WkLbl2.EditValue + "') ";

                //_dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbManImage.ResultsTableName = "Image";
                //_dbManImage.ExecuteInstruction();

                MWDataManager.clsDataAccess _dbManload = new MWDataManager.clsDataAccess();
                _dbManload.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
                _dbManload.SqlStatement = "select * from vw_RockEng_Data where Workplace = '" + WPLbl.EditValue.ToString() + "' and CaptWeek = '" + WkLbl.Text + "' order by OrderBy ";

                _dbManload.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManload.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManload.ResultsTableName = "LoadQuestions";
                _dbManload.ExecuteInstruction();

                DataSet dsloadQuest = new DataSet();
                dsloadQuest.Tables.Add(_dbManload.ResultsDataTable);

                theReport3.RegisterData(dsloadQuest);

                MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);

                _dbManImage.SqlStatement = "Select RiskRating Risk,'" + BlankImage + "' pp, Picture,document1,document2 from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' and  captyear = (select max(CaptYear) from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' ) ";

                _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManImage.ResultsTableName = "Image";
                _dbManImage.ExecuteInstruction();

                DataSet ReportDatasetReport = new DataSet();
                ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReport);

                DataSet ReportDatasetReportImage = new DataSet();
                ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReportImage);

                theReport3.Load(ReportsFolder + "\\RockEng.frx");

                theReport3.Design();

                pcReport.Clear();
                theReport3.Prepare();
                theReport3.Preview = pcReport;
                theReport3.ShowPrepared();
            }
            else
            {
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
                _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, '" + RRlabel.EditValue + "' rr,  * from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "'  order by captyear desc, convert(decimal(18,0),actweek)   desc ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "DevSummary";
                _dbMan.ExecuteInstruction();

                if (_dbMan.ResultsDataTable.Rows.Count > 0)
                {
                    if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "0")
                    {
                        Cat2.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "1")
                    {
                        Cat2.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "2")
                    {
                        Cat2.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "3")
                    {
                        Cat2.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "0")
                    {
                        Cat3.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "1")
                    {
                        Cat3.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "2")
                    {
                        Cat3.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "3")
                    {
                        Cat3.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "0")
                    {
                        Cat4.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "1")
                    {
                        Cat4.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "2")
                    {
                        Cat4.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "3")
                    {
                        Cat4.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "0")
                    {
                        Cat5.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "1")
                    {
                        Cat5.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "2")
                    {
                        Cat5.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "3")
                    {
                        Cat5.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "0")
                    {
                        Cat5.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "1")
                    {
                        Cat5.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "2")
                    {
                        Cat5.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "3")
                    {
                        Cat5.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "0")
                    {
                        Cat6.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "1")
                    {
                        Cat6.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "2")
                    {
                        Cat6.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "3")
                    {
                        Cat6.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "0")
                    {
                        Cat7.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "1")
                    {
                        Cat7.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "2")
                    {
                        Cat7.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "3")
                    {
                        Cat7.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "0")
                    {
                        Cat8.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "1")
                    {
                        Cat8.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "2")
                    {
                        Cat8.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "3")
                    {
                        Cat8.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "0")
                    {
                        Cat9.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "1")
                    {
                        Cat9.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "2")
                    {
                        Cat9.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "3")
                    {
                        Cat9.SelectedIndex = 3;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "0")
                {
                    Cat10.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "1")
                {
                    Cat10.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "2")
                {
                    Cat10.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "3")
                {
                    Cat10.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "4")
                {
                    Cat10.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "0")
                {
                    Cat11.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "1")
                {
                    Cat11.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "2")
                {
                    Cat11.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "3")
                {
                    Cat11.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat11Checked"].ToString() == "4")
                {
                    Cat11.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "0")
                {
                    Cat12.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "1")
                {
                    Cat12.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "2")
                {
                    Cat12.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "3")
                {
                    Cat12.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat12Checked"].ToString() == "4")
                {
                    Cat12.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "0")
                {
                    Cat13.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "1")
                {
                    Cat13.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "2")
                {
                    Cat13.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "3")
                {
                    Cat13.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat13Checked"].ToString() == "4")
                {
                    Cat13.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "0")
                {
                    Cat14.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "1")
                {
                    Cat14.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "2")
                {
                    Cat14.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "3")
                {
                    Cat14.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat14Checked"].ToString() == "4")
                {
                    Cat14.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "0")
                {
                    Cat15.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "1")
                {
                    Cat15.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "2")
                {
                    Cat15.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "3")
                {
                    Cat15.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat15Checked"].ToString() == "4")
                {
                    Cat15.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "0")
                {
                    Cat16.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "1")
                {
                    Cat16.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "2")
                {
                    Cat16.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "3")
                {
                    Cat16.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat16Checked"].ToString() == "4")
                {
                    Cat16.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "0")
                {
                    Cat17.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "1")
                {
                    Cat17.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "2")
                {
                    Cat17.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "3")
                {
                    Cat17.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat17Checked"].ToString() == "4")
                {
                    Cat17.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "0")
                {
                    Cat18.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "1")
                {
                    Cat18.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "2")
                {
                    Cat18.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "3")
                {
                    Cat18.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat18Checked"].ToString() == "4")
                {
                    Cat18.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "0")
                {
                    Cat19.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "1")
                {
                    Cat19.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "2")
                {
                    Cat19.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "3")
                {
                    Cat19.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat19Checked"].ToString() == "4")
                {
                    Cat19.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "0")
                {
                    Cat20.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "1")
                {
                    Cat20.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "2")
                {
                    Cat20.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "3")
                {
                    Cat20.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat20Checked"].ToString() == "4")
                {
                    Cat20.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "0")
                {
                    Cat21.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "1")
                {
                    Cat21.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "2")
                {
                    Cat21.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "3")
                {
                    Cat21.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat21Checked"].ToString() == "4")
                {
                    Cat21.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "0")
                {
                    Cat22.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "1")
                {
                    Cat22.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "2")
                {
                    Cat22.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "3")
                {
                    Cat22.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat22Checked"].ToString() == "4")
                {
                    Cat22.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "0")
                {
                    Cat23.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "1")
                {
                    Cat23.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "2")
                {
                    Cat23.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "3")
                {
                    Cat23.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat23Checked"].ToString() == "4")
                {
                    Cat23.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "0")
                {
                    Cat24.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "1")
                {
                    Cat24.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "2")
                {
                    Cat24.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "3")
                {
                    Cat24.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat24Checked"].ToString() == "4")
                {
                    Cat24.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "0")
                {
                    Cat25.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "1")
                {
                    Cat25.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "2")
                {
                    Cat25.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "3")
                {
                    Cat25.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat25Checked"].ToString() == "4")
                {
                    Cat25.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "0")
                {
                    Cat26.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "1")
                {
                    Cat26.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "2")
                {
                    Cat26.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "3")
                {
                    Cat26.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat26Checked"].ToString() == "4")
                {
                    Cat26.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "0")
                {
                    Cat27.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "1")
                {
                    Cat27.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "2")
                {
                    Cat27.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "3")
                {
                    Cat27.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat27Checked"].ToString() == "4")
                {
                    Cat27.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "0")
                {
                    Cat28.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "1")
                {
                    Cat28.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "2")
                {
                    Cat28.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "3")
                {
                    Cat28.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat28Checked"].ToString() == "4")
                {
                    Cat28.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "0")
                {
                    Cat29.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "1")
                {
                    Cat29.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "2")
                {
                    Cat29.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "3")
                {
                    Cat29.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat29Checked"].ToString() == "4")
                {
                    Cat29.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "0")
                {
                    Cat30.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "1")
                {
                    Cat30.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "2")
                {
                    Cat30.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "3")
                {
                    Cat30.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat30Checked"].ToString() == "4")
                {
                    Cat30.SelectedIndex = 4;
                }
               


                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "0")
                {
                    Cat31.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "1")
                {
                    Cat31.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "2")
                {
                    Cat31.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "3")
                {
                    Cat31.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat31Checked"].ToString() == "4")
                {
                    Cat31.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "0")
                {
                    Cat32.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "1")
                {
                    Cat32.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "2")
                {
                    Cat32.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "3")
                {
                    Cat32.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat32Checked"].ToString() == "4")
                {
                    Cat32.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "0")
                {
                    Cat33.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "1")
                {
                    Cat33.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "2")
                {
                    Cat33.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "3")
                {
                    Cat33.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat33Checked"].ToString() == "4")
                {
                    Cat33.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "0")
                {
                    Cat34.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "1")
                {
                    Cat43.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "2")
                {
                    Cat43.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "3")
                {
                    Cat43.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat34Checked"].ToString() == "4")
                {
                    Cat43.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "0")
                {
                    Cat35.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "1")
                {
                    Cat35.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "2")
                {
                    Cat35.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "3")
                {
                    Cat35.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat35Checked"].ToString() == "4")
                {
                    Cat35.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "0")
                {
                    Cat36.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "1")
                {
                    Cat36.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "2")
                {
                    Cat36.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "3")
                {
                    Cat36.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat36Checked"].ToString() == "4")
                {
                    Cat36.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "0")
                {
                    Cat37.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "1")
                {
                    Cat37.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "2")
                {
                    Cat37.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "3")
                {
                    Cat37.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat37Checked"].ToString() == "4")
                {
                    Cat37.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "0")
                {
                    Cat38.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "1")
                {
                    Cat38.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "2")
                {
                    Cat38.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "3")
                {
                    Cat38.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat38Checked"].ToString() == "4")
                {
                    Cat38.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "0")
                {
                    Cat39.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "1")
                {
                    Cat39.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "2")
                {
                    Cat39.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "3")
                {
                    Cat39.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat39Checked"].ToString() == "4")
                {
                    Cat39.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "0")
                {
                    Cat40.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "1")
                {
                    Cat40.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "2")
                {
                    Cat40.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "3")
                {
                    Cat40.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat40Checked"].ToString() == "4")
                {
                    Cat40.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "0")
                {
                    Cat41.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "1")
                {
                    Cat41.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "2")
                {
                    Cat41.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "3")
                {
                    Cat41.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat41Checked"].ToString() == "4")
                {
                    Cat41.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "0")
                {
                    Cat42.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "1")
                {
                    Cat42.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "2")
                {
                    Cat42.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "3")
                {
                    Cat42.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat42Checked"].ToString() == "4")
                {
                    Cat42.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "0")
                {
                    Cat43.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "1")
                {
                    Cat43.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "2")
                {
                    Cat43.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "3")
                {
                    Cat43.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat43Checked"].ToString() == "4")
                {
                    Cat43.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "0")
                {
                    Cat44.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "1")
                {
                    Cat44.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "2")
                {
                    Cat44.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "3")
                {
                    Cat44.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat44Checked"].ToString() == "4")
                {
                    Cat44.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "0")
                {
                    Cat45.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "1")
                {
                    Cat45.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "2")
                {
                    Cat45.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "3")
                {
                    Cat45.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat45Checked"].ToString() == "4")
                {
                    Cat45.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "0")
                {
                    Cat46.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "1")
                {
                    Cat46.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "2")
                {
                    Cat46.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "3")
                {
                    Cat46.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat46Checked"].ToString() == "4")
                {
                    Cat46.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "0")
                {
                    Cat47.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "1")
                {
                    Cat47.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "2")
                {
                    Cat47.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "3")
                {
                    Cat47.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat47Checked"].ToString() == "4")
                {
                    Cat47.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "0")
                {
                    Cat48.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "1")
                {
                    Cat48.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "2")
                {
                    Cat48.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "3")
                {
                    Cat48.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat48Checked"].ToString() == "4")
                {
                    Cat48.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "0")
                {
                    Cat49.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "1")
                {
                    Cat49.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "2")
                {
                    Cat49.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "3")
                {
                    Cat49.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat49Checked"].ToString() == "4")
                {
                    Cat49.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "0")
                {
                    Cat50.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "1")
                {
                    Cat50.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "2")
                {
                    Cat50.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "3")
                {
                    Cat50.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat50Checked"].ToString() == "4")
                {
                    Cat50.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "0")
                {
                    Cat51.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "1")
                {
                    Cat51.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "2")
                {
                    Cat51.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "3")
                {
                    Cat51.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat51Checked"].ToString() == "4")
                {
                    Cat51.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "0")
                {
                    Cat52.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "1")
                {
                    Cat52.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "2")
                {
                    Cat52.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "3")
                {
                    Cat52.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat52Checked"].ToString() == "4")
                {
                    Cat52.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "0")
                {
                    Cat53.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "1")
                {
                    Cat53.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "2")
                {
                    Cat53.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "3")
                {
                    Cat53.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat53Checked"].ToString() == "4")
                {
                    Cat53.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "0")
                {
                    Cat54.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "1")
                {
                    Cat54.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "2")
                {
                    Cat54.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "3")
                {
                    Cat54.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat54Checked"].ToString() == "4")
                {
                    Cat54.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "0")
                {
                    Cat55.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "1")
                {
                    Cat55.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "2")
                {
                    Cat55.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "3")
                {
                    Cat55.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat55Checked"].ToString() == "4")
                {
                    Cat55.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "0")
                {
                    Cat56.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "1")
                {
                    Cat56.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "2")
                {
                    Cat56.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "3")
                {
                    Cat56.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat56Checked"].ToString() == "4")
                {
                    Cat56.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "0")
                {
                    Cat57.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "1")
                {
                    Cat57.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "2")
                {
                    Cat57.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "3")
                {
                    Cat57.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat57Checked"].ToString() == "4")
                {
                    Cat57.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "0")
                {
                    Cat58.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "1")
                {
                    Cat58.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "2")
                {
                    Cat58.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "3")
                {
                    Cat58.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat58Checked"].ToString() == "4")
                {
                    Cat58.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "0")
                {
                    Cat59.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "1")
                {
                    Cat59.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "2")
                {
                    Cat59.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "3")
                {
                    Cat59.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "4")
                {
                    Cat59.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "0")
                {
                    Cat59.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "1")
                {
                    Cat59.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "2")
                {
                    Cat59.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "3")
                {
                    Cat59.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat59Checked"].ToString() == "4")
                {
                    Cat59.SelectedIndex = 4;
                }
                
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "0")
                {
                    Cat61.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "1")
                {
                    Cat61.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "2")
                {
                    Cat61.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "3")
                {
                    Cat61.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat61Checked"].ToString() == "4")
                {
                    Cat61.SelectedIndex = 4;
                }
                    
                    Cat1Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat1Note"].ToString();
                    Cat2Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat2Note"].ToString();
                    Cat3Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat3Note"].ToString();
                    Cat4Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat4Note"].ToString();
                    Cat5Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat5Note"].ToString();
                    Cat6Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat6Note"].ToString();
                    Cat7Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat7Note"].ToString();
                    Cat8Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat8Note"].ToString();
                    Cat9Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat9Note"].ToString();
                    Cat10Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat10Note"].ToString();
                    Cat11Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat11Note"].ToString();
                    Cat12Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat12Note"].ToString();
                    Cat13Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat13Note"].ToString();
                    Cat14Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat14Note"].ToString();
                    Cat15Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat15Note"].ToString();
                    Cat16Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat16Note"].ToString();
                    Cat17Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat17Note"].ToString();
                    Cat18Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat18Note"].ToString();
                    Cat19Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat19Note"].ToString();
                    Cat20Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat20Note"].ToString();
                    Cat21Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat21Note"].ToString();
                    Cat22Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat22Note"].ToString();
                    Cat23Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat23Note"].ToString();
                    Cat24Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat24Note"].ToString();
                    Cat25Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat25Note"].ToString();
                    Cat26Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat26Note"].ToString();
                    Cat27Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat27Note"].ToString();
                    Cat28Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat28Note"].ToString();
                    Cat29Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat29Note"].ToString();
                    Cat30Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat30Note"].ToString();
                    Cat31Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat31Note"].ToString();
                    Cat32Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat32Note"].ToString();
                    Cat33Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat33Note"].ToString();
                    Cat34Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat34Note"].ToString();
                    Cat35Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat35Note"].ToString();
                    Cat36Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat36Note"].ToString();
                    Cat37Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat37Note"].ToString();
                    Cat38Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat38Note"].ToString();
                    Cat39Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat39Note"].ToString();
                    Cat40Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat40Note"].ToString();
                    Cat41Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat41Note"].ToString();
                    Cat42Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat42Note"].ToString();
                    Cat43Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat43Note"].ToString();
                    Cat44Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat44Note"].ToString();
                    Cat45Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat45Note"].ToString();
                    Cat46Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat46Note"].ToString();
                    Cat47Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat47Note"].ToString();
                    Cat48Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat48Note"].ToString();
                    Cat49Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat49Note"].ToString();
                    Cat50Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat50Note"].ToString();
                    Cat51Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat51Note"].ToString();
                    Cat52Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat52Note"].ToString();
                    Cat53Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat53Note"].ToString();
                    Cat54Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat54Note"].ToString();
                    Cat55Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat55Note"].ToString();
                    Cat56Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat56Note"].ToString();
                    Cat57Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat57Note"].ToString();
                    Cat58Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat58Note"].ToString();
                    Cat59Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat59Note"].ToString();
                    Cat60Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat60Note"].ToString();
                    Cat61Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat61Note"].ToString();
                    Cat62Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat62Note"].ToString();
                    Cat63Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat63Note"].ToString();

                    commentsTxt.Text = _dbMan.ResultsDataTable.Rows[0]["GeneralComments"].ToString();
                }

            }

            Cursor = Cursors.Default;
        }

        void LoadPercentageCompleted()
        {
            decimal Total;
            decimal Act;
            
            Total = 59;
            Act = 0;

            //N/A Exclusions
            if (Cat2.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat3.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat4.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat5.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat6.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat7.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat8.SelectedIndex == 4)
            {
                Total = Total - 1;
            }
            

            if (Cat9.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat10.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat11.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat12.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat13.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat14.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat15.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat16.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat17.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat18.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat19.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat20.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat21.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat22.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat23.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat24.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat25.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat26.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat27.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat28.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat29.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat30.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat31.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat32.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat33.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat34.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat35.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat36.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat37.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat38.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat39.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat40.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat41.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat42.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat43.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat44.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat45.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat46.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat47.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat48.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat49.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat50.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat51.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat52.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat53.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat54.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat54.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat55.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat56.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat57.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat58.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat59.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            //if (Cat60.SelectedIndex == 4)
            //{
            //    Total = Total - 1;
            //}

            if (Cat61.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            //if (Cat62.SelectedIndex == 4)
            //{
            //    Total = Total - 1;
            //}

            //if (Cat63.SelectedIndex == 4)
            //{
            //    Total = Total - 1;
            //}
            
            //Calc
            if (Cat2.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat3.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat4.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat5.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat6.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat7.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat8.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat9.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat10.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat11.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat12.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat13.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat14.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat15.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat16.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat17.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat18.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat19.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat20.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat21.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat22.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat23.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat24.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat25.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat26.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat27.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat28.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat29.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat30.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat31.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat32.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat33.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat34.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat35.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat36.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat37.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat38.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat39.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat40.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat41.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat42.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat43.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat44.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat45.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat46.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat47.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat48.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat49.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat50.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat51.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat52.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat53.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat54.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat55.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat56.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat57.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat58.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat59.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            //if (Cat60.SelectedIndex < 1)
            //{
            //    Act = Act + 1;
            //}

            if (Cat61.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            //if (cmbx62.SelectedIndex < 1)
            //{
            //    Act = Act + 1;
            //}

            //if (Cat63.SelectedIndex < 1)
            //{
            //    Act = Act + 1;
            //}
            
            RRlabel.EditValue = Convert.ToString(Math.Round(Convert.ToDecimal((Act / Total) * 100), 2));
        }

        private bool Forceinstructions()
        {
            bool TestFailed = false;

            if (Cat2.SelectedIndex > 0 && Cat2.SelectedIndex < 4 && string.IsNullOrEmpty(Cat2Note.Text))
            {
                TestFailed = true;
                Cat2Note.Focus();
            }

            if (Cat3.SelectedIndex > 0 && Cat3.SelectedIndex < 4 && string.IsNullOrEmpty(Cat3Note.Text))
            {
                TestFailed = true;
                Cat3Note.Focus();
            }

            if (Cat4.SelectedIndex > 0 && Cat4.SelectedIndex < 4 && string.IsNullOrEmpty(Cat4Note.Text))
            {
                TestFailed = true;
                Cat4Note.Focus();
            }

            if (Cat5.SelectedIndex > 0 && Cat5.SelectedIndex < 4 && string.IsNullOrEmpty(Cat5Note.Text))
            {
                TestFailed = true;
                Cat5Note.Focus();
            }

            if (Cat6.SelectedIndex > 0 && Cat6.SelectedIndex < 4 && string.IsNullOrEmpty(Cat6Note.Text))
            {
                TestFailed = true;
                Cat6Note.Focus();
            }

            if (Cat7.SelectedIndex > 0 && Cat7.SelectedIndex < 4 && string.IsNullOrEmpty(Cat7Note.Text))
            {
                TestFailed = true;
                Cat7Note.Focus();
            }

            if (Cat8.SelectedIndex > 0 && Cat8.SelectedIndex < 4 && string.IsNullOrEmpty(Cat8Note.Text))
            {
                TestFailed = true;
                Cat8Note.Focus();
            }

            if (Cat9.SelectedIndex > 0 && Cat9.SelectedIndex < 4 && string.IsNullOrEmpty(Cat9Note.Text))
            {
                TestFailed = true;
                Cat9Note.Focus();
            }

            if (Cat10.SelectedIndex > 0 && Cat10.SelectedIndex < 4 && string.IsNullOrEmpty(Cat10Note.Text))
            {
                TestFailed = true;
                Cat10Note.Focus();
            }

            if (Cat11.SelectedIndex > 0 && Cat11.SelectedIndex < 4 && string.IsNullOrEmpty(Cat11Note.Text))
            {
                TestFailed = true;
                Cat11Note.Focus();
            }

            if (Cat12.SelectedIndex > 0 && Cat12.SelectedIndex < 4 && string.IsNullOrEmpty(Cat12Note.Text))
            {
                TestFailed = true;
                Cat12Note.Focus();
            }

            if (Cat13.SelectedIndex > 0 && Cat13.SelectedIndex < 4 && string.IsNullOrEmpty(Cat13Note.Text))
            {
                TestFailed = true;
                Cat13Note.Focus();
            }

            if (Cat14.SelectedIndex > 0 && Cat14.SelectedIndex < 4 && string.IsNullOrEmpty(Cat14Note.Text))
            {
                TestFailed = true;
                Cat14Note.Focus();
            }

            if (Cat15.SelectedIndex > 0 && Cat15.SelectedIndex < 4 && string.IsNullOrEmpty(Cat15Note.Text))
            {
                TestFailed = true;
                Cat15Note.Focus();
            }

            if (Cat16.SelectedIndex > 0 && Cat16.SelectedIndex < 4 && string.IsNullOrEmpty(Cat16Note.Text))
            {
                TestFailed = true;
                Cat16Note.Focus();
            }

            if (Cat17.SelectedIndex > 0 && Cat17.SelectedIndex < 4 && string.IsNullOrEmpty(Cat17Note.Text))
            {
                TestFailed = true;
                Cat17Note.Focus();
            }

            if (Cat18.SelectedIndex > 0 && Cat18.SelectedIndex < 4 && string.IsNullOrEmpty(Cat18Note.Text))
            {
                TestFailed = true;
                Cat18Note.Focus();
            }

            if (Cat19.SelectedIndex > 0 && Cat19.SelectedIndex < 4 && string.IsNullOrEmpty(Cat19Note.Text))
            {
                TestFailed = true;
                Cat19Note.Focus();
            }

            if (Cat20.SelectedIndex > 0 && Cat19.SelectedIndex < 4 && string.IsNullOrEmpty(Cat20Note.Text))
            {
                TestFailed = true;
                Cat20Note.Focus();
            }

            if (Cat21.SelectedIndex > 0 && Cat21.SelectedIndex < 4 && string.IsNullOrEmpty(Cat21Note.Text))
            {
                TestFailed = true;
                Cat21Note.Focus();
            }

            if (Cat22.SelectedIndex > 0 && Cat22.SelectedIndex < 4 && string.IsNullOrEmpty(Cat22Note.Text))
            {
                TestFailed = true;
                Cat22Note.Focus();
            }

            if (Cat23.SelectedIndex > 0 && Cat23.SelectedIndex < 4 && string.IsNullOrEmpty(Cat23Note.Text))
            {
                TestFailed = true;
                Cat23Note.Focus();
            }

            if (Cat24.SelectedIndex > 0 && Cat24.SelectedIndex < 4 && string.IsNullOrEmpty(Cat24Note.Text))
            {
                TestFailed = true;
                Cat24Note.Focus();
            }

            if (Cat25.SelectedIndex > 0 && Cat25.SelectedIndex < 4 && string.IsNullOrEmpty(Cat25Note.Text))
            {
                TestFailed = true;
                Cat25Note.Focus();
            }

            if (Cat26.SelectedIndex > 0 && Cat26.SelectedIndex < 4 && string.IsNullOrEmpty(Cat26Note.Text))
            {
                TestFailed = true;
                Cat26Note.Focus();
            }

            if (Cat27.SelectedIndex > 0 && Cat27.SelectedIndex < 4 && string.IsNullOrEmpty(Cat27Note.Text))
            {
                TestFailed = true;
                Cat27Note.Focus();
            }

            if (Cat28.SelectedIndex > 0 && Cat28.SelectedIndex < 4 && string.IsNullOrEmpty(Cat28Note.Text))
            {
                TestFailed = true;
                Cat28Note.Focus();
            }

            if (Cat29.SelectedIndex > 0 && Cat29.SelectedIndex < 4 && string.IsNullOrEmpty(Cat29Note.Text))
            {
                TestFailed = true;
                Cat29Note.Focus();
            }

            if (Cat30.SelectedIndex > 0 && Cat30.SelectedIndex < 4 && string.IsNullOrEmpty(Cat30Note.Text))
            {
                TestFailed = true;
                Cat30Note.Focus();
            }

            if (Cat31.SelectedIndex > 0 && Cat31.SelectedIndex < 4 && string.IsNullOrEmpty(Cat31Note.Text))
            {
                TestFailed = true;
                Cat31Note.Focus();
            }

            if (Cat32.SelectedIndex > 0 && Cat32.SelectedIndex < 4 && string.IsNullOrEmpty(Cat32Note.Text))
            {
                TestFailed = true;
                Cat32Note.Focus();
            }

            if (Cat33.SelectedIndex > 0 && Cat33.SelectedIndex < 4 && string.IsNullOrEmpty(Cat33Note.Text))
            {
                TestFailed = true;
                Cat33Note.Focus();
            }

            if (Cat34.SelectedIndex > 0 && Cat34.SelectedIndex < 4 && string.IsNullOrEmpty(Cat34Note.Text))
            {
                TestFailed = true;
                Cat34Note.Focus();
            }

            if (Cat35.SelectedIndex > 0 && Cat35.SelectedIndex < 4 && string.IsNullOrEmpty(Cat35Note.Text))
            {
                TestFailed = true;
                Cat35Note.Focus();
            }

            if (Cat36.SelectedIndex > 0 && Cat36.SelectedIndex < 4 && string.IsNullOrEmpty(Cat36Note.Text))
            {
                TestFailed = true;
                Cat36Note.Focus();
            }

            if (Cat37.SelectedIndex > 0 && Cat37.SelectedIndex < 4 && string.IsNullOrEmpty(Cat37Note.Text))
            {
                TestFailed = true;
                Cat37Note.Focus();
            }

            if (Cat38.SelectedIndex > 0 && Cat38.SelectedIndex < 4 && string.IsNullOrEmpty(Cat38Note.Text))
            {
                TestFailed = true;
                Cat38Note.Focus();
            }

            if (Cat39.SelectedIndex > 0 && Cat39.SelectedIndex < 4 && string.IsNullOrEmpty(Cat39Note.Text))
            {
                TestFailed = true;
                Cat39Note.Focus();
            }

            if (Cat40.SelectedIndex > 0 && Cat40.SelectedIndex < 4 && string.IsNullOrEmpty(Cat40Note.Text))
            {
                TestFailed = true;
                Cat40Note.Focus();
            }

            if (Cat41.SelectedIndex > 0 && Cat41.SelectedIndex < 4 && string.IsNullOrEmpty(Cat41Note.Text))
            {
                TestFailed = true;
                Cat41Note.Focus();
            }

            if (Cat42.SelectedIndex > 0 && Cat42.SelectedIndex < 4 && string.IsNullOrEmpty(Cat42Note.Text))
            {
                TestFailed = true;
                Cat42Note.Focus();
            }

            if (Cat43.SelectedIndex > 0 && Cat43.SelectedIndex < 4 && string.IsNullOrEmpty(Cat43Note.Text))
            {
                TestFailed = true;
                Cat43Note.Focus();
            }

            if (Cat44.SelectedIndex > 0 && Cat44.SelectedIndex < 4 && string.IsNullOrEmpty(Cat44Note.Text))
            {
                TestFailed = true;
                Cat44Note.Focus();
            }

            if (Cat45.SelectedIndex > 0 && Cat45.SelectedIndex < 4 && string.IsNullOrEmpty(Cat45Note.Text))
            {
                TestFailed = true;
                Cat45Note.Focus();
            }

            if (Cat46.SelectedIndex > 0 && Cat46.SelectedIndex < 4 && string.IsNullOrEmpty(Cat46Note.Text))
            {
                TestFailed = true;
                Cat46Note.Focus();
            }

            if (Cat47.SelectedIndex > 0 && Cat47.SelectedIndex < 4 && string.IsNullOrEmpty(Cat47Note.Text))
            {
                TestFailed = true;
                Cat47Note.Focus();
            }

            if (Cat48.SelectedIndex > 0 && Cat48.SelectedIndex < 4 && string.IsNullOrEmpty(Cat48Note.Text))
            {
                TestFailed = true;
                Cat48Note.Focus();
            }

            if (Cat49.SelectedIndex > 0 && Cat49.SelectedIndex < 4 && string.IsNullOrEmpty(Cat49Note.Text))
            {
                TestFailed = true;
                Cat49Note.Focus();
            }

            if (Cat50.SelectedIndex > 0 && Cat50.SelectedIndex < 4 && string.IsNullOrEmpty(Cat50Note.Text))
            {
                TestFailed = true;
                Cat50Note.Focus();
            }

            if (Cat51.SelectedIndex > 0 && Cat51.SelectedIndex < 4 && string.IsNullOrEmpty(Cat51Note.Text))
            {
                TestFailed = true;
                Cat51Note.Focus();
            }

            if (Cat52.SelectedIndex > 0 && Cat52.SelectedIndex < 4 && string.IsNullOrEmpty(Cat52Note.Text))
            {
                TestFailed = true;
                Cat52Note.Focus();
            }

            if (Cat53.SelectedIndex > 0 && Cat53.SelectedIndex < 4 && string.IsNullOrEmpty(Cat53Note.Text))
            {
                TestFailed = true;
                Cat53Note.Focus();
            }

            if (Cat54.SelectedIndex > 0 && Cat54.SelectedIndex < 4 && string.IsNullOrEmpty(Cat54Note.Text))
            {
                TestFailed = true;
                Cat54Note.Focus();
            }

            if (Cat55.SelectedIndex > 0 && Cat55.SelectedIndex < 4 && string.IsNullOrEmpty(Cat55Note.Text))
            {
                TestFailed = true;
                Cat55Note.Focus();
            }

            if (Cat56.SelectedIndex > 0 && Cat56.SelectedIndex < 4 && string.IsNullOrEmpty(Cat56Note.Text))
            {
                TestFailed = true;
                Cat56Note.Focus();
            }

            if (Cat57.SelectedIndex > 0 && Cat57.SelectedIndex < 4 && string.IsNullOrEmpty(Cat57Note.Text))
            {
                TestFailed = true;
                Cat57Note.Focus();
            }

            //if (Cat58.SelectedIndex > 0 && Cat58.SelectedIndex < 4 && string.IsNullOrEmpty(Cat58Note.Text))
            //{
            //    TestFailed = true;
            //    Cat58Note.Focus();
            //}

            //if (Cat59.SelectedIndex > 0 && Cat59.SelectedIndex < 4 && string.IsNullOrEmpty(Cat59Note.Text))
            //{
            //    TestFailed = true;
            //    Cat59Note.Focus();
            //}

            //if (Cat60.SelectedIndex > 0 && Cat60.SelectedIndex < 4 && string.IsNullOrEmpty(Cat60Note.Text))
            //{
            //    TestFailed = true;
            //    Cat60Note.Focus();
            //}

            //if (Cat61.SelectedIndex > 0 && Cat61.SelectedIndex < 4 && string.IsNullOrEmpty(Cat61Note.Text))
            //{
            //    TestFailed = true;
            //    Cat61Note.Focus();
            //}

            //if (Cat62.SelectedIndex > 0 && Cat62.SelectedIndex < 4 && string.IsNullOrEmpty(Cat62Note.Text))
            //{
            //    TestFailed = true;
            //    Cat62Note.Focus();
            //}

            //if (Cat63.SelectedIndex > 0 && Cat63.SelectedIndex < 4 && string.IsNullOrEmpty(Cat63Note.Text))
            //{
            //    TestFailed = true;
            //    Cat63Note.Focus();
            //}
            
            return TestFailed;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            bool testfailed = Forceinstructions();

            if (testfailed)
            {
                MessageBox.Show("Please fill in all Deviation Notes");
                return;
            }

            //Error Message for SB & MO
            try
            {
                if (cmbSB.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please enter a valid Shift Boss?");
                    //cmbSB.ShowPopup();
                    return;
                }
            }
            catch {
                MessageBox.Show("Please enter a valid Shift Boss?");
                //cmbSB.ShowPopup();
                return;
            }
            try
            {
                if (cmbMO.EditValue.ToString() == "[EditValue is null]")
            {
                MessageBox.Show("Please enter a valid Mine Overseer?");
                //cmbMO.ShowPopup();
                return;
            }
            }
            catch
            {
                MessageBox.Show("Please enter a valid Shift Boss?");
                //cmbSB.ShowPopup();
                return;
            }

            string WPStatus = string.Empty;

            string Cat1Check = string.Empty;

            if (Cat1A.Checked == true)
            {
                Cat1Check = "A";
            }
            if (Cat1B.Checked == true)
            {
                Cat1Check = "B";
            }
            if (Cat1S.Checked == true)
            {
                Cat1Check = "S";
            }
            if (Cat1A.Checked == false && Cat1B.Checked == false && Cat1S.Checked == false)
            {
                Cat1Check = "N";
            }
            if (Cat1P.Checked == true)
            {
                Cat1Check = Cat1Check + "Y";
            }
            if (Cat1P.Checked == false)
            {
                Cat1Check = Cat1Check + "N";
            }

            string SB = string.Empty;
            string MO = string.Empty;

            if (cmbSB.EditValue.ToString() != string.Empty)
            {
                SB = cmbSB.EditValue.ToString();
            }
            else
            {
                MessageBox.Show("No Shift Boss selected.");
                return;
            }

            if (cmbMO.EditValue.ToString() != string.Empty)
            {
                MO = cmbMO.EditValue.ToString();
            }
            else
            {
                MessageBox.Show("No Mine Overseer selected.");
                return;
            }
            
            /////Get Start Date for Save so its one checklist for pmonth
            //MWDataManager.clsDataAccess _dbManStartDate = new MWDataManager.clsDataAccess();
            //_dbManStartDate.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            //_dbManStartDate.SqlStatement = "  select Min(BeginDate)StartDate from vw_Seccal_totalShifts_Calc where prodmonth = '"+ WkLbl2.EditValue + "' and SectionID like '" + lblMOSect.Text + "%' \r\n" +
            //                      "  \r\n" +
            //                      "  \r\n";
            //_dbManStartDate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbManStartDate.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbManStartDate.ExecuteInstruction();

            //DateTime StartDate;
            //if (_dbManStartDate.ResultsDataTable.Rows.Count > 0)
            //{
            //    StartDate = Convert.ToDateTime(_dbManStartDate.ResultsDataTable.Rows[0][0].ToString());
            //}
            //else
            //{
            //    MessageBox.Show("Workplace not planned for the Production Month.");
            //    return;
            //}

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = "Delete From [tbl_DPT_RockMechInspection] Where workplace = '" + WPLbl.EditValue + "' and CaptYear = datepart(YYYY,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "') and CaptWeek = '" + WkLbl2.EditValue + "' \r\n \r\n";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into [tbl_DPT_RockMechInspection] VALUES ( '" + WPLbl.EditValue + "' , datepart(YYYY,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "') , datepart(ISOWK,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "') , datepart(ISOWK,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "')  , getdate() , '" + TUserInfo.UserID + "' , \r\n " +

                                  " '" + cmbSB.EditValue.ToString() + "' , '" + cmbMO.EditValue.ToString() + "' , '" + RRlabel.EditValue + "', \r\n" +

                                  " '" + Cat1Check + "' , '" + Cat1Note.Text + "' , \r\n" +
                                  " '" + cat2Checked + "' , '" + Cat2Note.Text + "' , \r\n" +
                                  " '" + cat3Checked + "' , '" + Cat3Note.Text + "' , \r\n" +
                                  " '" + cat4Checked + "' , '" + Cat4Note.Text + "' , \r\n" +
                                  " '" + cat5Checked + "' , '" + Cat5Note.Text + "' , \r\n" +
                                  " '" + cat6Checked + "' , '" + Cat6Note.Text + "' , \r\n" +
                                  " '" + cat7Checked + "' , '" + Cat7Note.Text + "' , \r\n" +
                                  " '" + cat8Checked + "' , '" + Cat8Note.Text + "' , \r\n" +
                                  " '" + cat9Checked + "' , '" + Cat9Note.Text + "' , \r\n" +
                                  " '" + cat10Checked + "' , '" + Cat10Note.Text + "' , \r\n" +
                                  " '" + cat11Checked + "' , '" + Cat11Note.Text + "' , \r\n" +
                                  " '" + cat12Checked + "' , '" + Cat12Note.Text + "' , \r\n" +
                                  " '" + cat13Checked + "' , '" + Cat13Note.Text + "' , \r\n" +
                                  " '" + cat14Checked + "' , '" + Cat14Note.Text + "' , \r\n" +
                                  " '" + cat15Checked + "' , '" + Cat15Note.Text + "' , \r\n" +
                                  " '" + cat16Checked + "' , '" + Cat16Note.Text + "' , \r\n" +
                                  " '" + cat17Checked + "' , '" + Cat17Note.Text + "' , \r\n" +
                                  " '" + cat18Checked + "' , '" + Cat18Note.Text + "' , \r\n" +
                                  " '" + cat19Checked + "' , '" + Cat19Note.Text + "' , \r\n" +
                                  " '" + cat20Checked + "' , '" + Cat20Note.Text + "' , \r\n" +
                                  " '" + cat21Checked + "' , '" + Cat21Note.Text + "' , \r\n" +
                                  " '" + cat22Checked + "' , '" + Cat22Note.Text + "' , \r\n" +
                                  " '" + cat23Checked + "' , '" + Cat23Note.Text + "' , \r\n" +
                                  " '" + cat24Checked + "' , '" + Cat24Note.Text + "' , \r\n" +
                                  " '" + cat25Checked + "' , '" + Cat25Note.Text + "' , \r\n" +
                                  " '" + cat26Checked + "' , '" + Cat26Note.Text + "' , \r\n" +
                                  " '" + cat27Checked + "' , '" + Cat27Note.Text + "' , \r\n" +
                                  " '" + cat28Checked + "' , '" + Cat28Note.Text + "' , \r\n" +
                                  " '" + cat29Checked + "' , '" + Cat29Note.Text + "' , \r\n" +
                                  " '" + cat30Checked + "' , '" + Cat30Note.Text + "' , \r\n" +

                                  " '" + cat31Checked + "' , '" + Cat31Note.Text + "' , \r\n" +
                                  " '" + cat32Checked + "' , '" + Cat32Note.Text + "' , \r\n" +
                                  " '" + cat33Checked + "' , '" + Cat33Note.Text + "' , \r\n" +
                                  " '" + cat34Checked + "' , '" + Cat34Note.Text + "' , \r\n" +
                                  " '" + cat35Checked + "' , '" + Cat35Note.Text + "' , \r\n" +
                                  " '" + cat36Checked + "' , '" + Cat36Note.Text + "' , \r\n" +
                                  " '" + cat37Checked + "' , '" + Cat37Note.Text + "' , \r\n" +
                                  " '" + cat38Checked + "' , '" + Cat38Note.Text + "' , \r\n" +
                                  " '" + cat39Checked + "' , '" + Cat39Note.Text + "' , \r\n" +
                                  " '" + cat40Checked + "' , '" + Cat40Note.Text + "' , \r\n" +
                                  " '" + cat41Checked + "' , '" + Cat41Note.Text + "' , \r\n" +
                                  " '" + cat42Checked + "' , '" + Cat42Note.Text + "' , \r\n" +
                                  " '" + cat43Checked + "' , '" + Cat43Note.Text + "' , \r\n" +
                                  " '" + cat44Checked + "' , '" + Cat44Note.Text + "' , \r\n" +
                                  " '" + cat45Checked + "' , '" + Cat45Note.Text + "' , \r\n" +
                                  " '" + cat46Checked + "' , '" + Cat46Note.Text + "' , \r\n" +
                                  " '" + cat47Checked + "' , '" + Cat47Note.Text + "' , \r\n" +
                                  " '" + cat48Checked + "' , '" + Cat48Note.Text + "' , \r\n" +
                                  " '" + cat49Checked + "' , '" + Cat49Note.Text + "' , \r\n" +
                                  " '" + cat50Checked + "' , '" + Cat50Note.Text + "' , \r\n" +

                                  " '" + cat51Checked + "' , '" + Cat51Note.Text + "' , \r\n" +
                                  " '" + cat52Checked + "' , '" + Cat52Note.Text + "' , \r\n" +
                                  " '" + cat53Checked + "' , '" + Cat53Note.Text + "' , \r\n" +
                                  " '" + cat54Checked + "' , '" + Cat54Note.Text + "' , \r\n" +
                                  " '" + cat55Checked + "' , '" + Cat55Note.Text + "' , \r\n" +
                                  " '" + cat56Checked + "' , '" + Cat56Note.Text + "' , \r\n" +
                                  " '" + cat57Checked + "' , '" + Cat57Note.Text + "' , \r\n" +
                                  " '" + cat58Checked + "' , '" + Cat58Note.Text + "' , \r\n" +
                                  " '" + cat59Checked + "' , '" + Cat59Note.Text + "' , \r\n" +
                                  " '" + Cat60.Text + "' , '" + Cat60Note.Text + "' , \r\n" +
                                  " '" + cat61Checked + "' , '" + Cat61Note.Text + "' , \r\n" +
                                  " '" + Cat62.Text + "' , '" + Cat62Note.Text + "' , \r\n" +
                                  " '" + Cat63.Text + "' , '" + Cat63Note.Text + "' , \r\n" +
                                  "  '" + txtAttachment.Text + "' ,  '" + commentsTxt.Text + "' ,  '" + WPStatus + "', \r\n" +
                                  
                                  " '" + DocAttachment1 + "','" + DocAttachment2 + "','') ";

            _dbMan.SqlStatement = _dbMan.SqlStatement + "\r\n \r\n exec sp_Incidents_Transfer  ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            LoadReport();
        }

        private void AddTypeBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialog1.FileName = null;
            result1 = openFileDialog1.ShowDialog();

            GetFile();
        }

        string BusUnit = string.Empty;

        void GetFile()
        {
            destinationFile = string.Empty;
            
            BusUnit = ProductionGlobal.ProductionGlobalTSysSettings._Banner;

            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(imageDir + @"\RockEngineering");

            if (result1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialog1.FileName;

                index = openFileDialog1.SafeFileName.IndexOf(".");

                if (WPLbl.EditValue.ToString() != string.Empty)
                {
                    FileName = WPLbl.EditValue + "_Doc1";
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog1.SafeFileName.Substring(index);
                
                destinationFile = imageDir + @"\RockEngineering" + "\\" + FileName + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            
                            destinationFile = imageDir + @"\RockEngineering" + "\\" + FileName + Ext;
                        }

                    }
                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    System.IO.File.Copy(sourceFile, imageDir + @"\RockEngineering" + "\\" + FileName + Ext, true);
                    dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering");
                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtAttachment.Text = destinationFile;
                PicBox.Image = Image.FromFile(txtAttachment.Text);
            }
        }

        void GetFileDoc()
        {
            destinationFile = string.Empty;

            BusUnit = ProductionGlobal.ProductionGlobalTSysSettings._Banner;

            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering\Documents");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(imageDir + @"\RockEngineering\Documents");

            if (resultDocpic1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialogDOcpic1.FileName;

                index = openFileDialogDOcpic1.SafeFileName.IndexOf(".");

                if (WPLbl.EditValue.ToString() != string.Empty)
                {
                    FileName = WPLbl.EditValue + "_Doc1";
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialogDOcpic1.SafeFileName.Substring(index);

                destinationFile = imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialogDOcpic1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext;//+ FileName + Name.ToString() + Ext
                        }
                    }

                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    System.IO.File.Copy(sourceFile, imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering\Documents");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                    DocPic1.Image = Image.FromFile(openFileDialogDOcpic1.FileName);
                }

                DocAttachment1 = destinationFile;
                DocPic1.Image = Image.FromFile(DocAttachment1);
            }
        }

        void GetFileDoc2()
        {
            destinationFile = string.Empty;
            BusUnit = ProductionGlobal.ProductionGlobalTSysSettings._Banner;

            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering\Documents");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(imageDir + @"\RockEngineering\Documents");

            if (resultDocpic2 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialogDOcpic2.FileName;

                index = openFileDialogDOcpic2.SafeFileName.IndexOf(".");

                if (WPLbl.EditValue.ToString() != string.Empty)
                {
                    FileName = WPLbl.EditValue + "_Doc2";
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialogDOcpic2.SafeFileName.Substring(index);

                destinationFile = imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialogDOcpic2.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext;
                        }
                    }

                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    System.IO.File.Copy(sourceFile, imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering\Documents");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                    DocPic2.Image = Image.FromFile(openFileDialogDOcpic2.FileName);
                }

                DocAttachment2 = destinationFile;
                DocPic2.Image = Image.FromFile(DocAttachment2);

            }
        }

        private void RockEnginSavebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Cat1A.Checked == false && Cat1B.Checked == false && Cat1S.Checked == false)
            //{
            //    MessageBox.Show("Please do the ABS-P declaration ");
            //    return;
            //}

            NewSave();

            //Old Save
            //SaveBtn_Click(null, null);
        }

        private void RockEnginAddImagebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddTypeBtn_Click(null, null);
        }
        
        private void Cat1ACheck_CheckedChanged(object sender, EventArgs e)
        {
            Cat1B.Checked = false;
            Cat1S.Checked = false;
        }

        private void Cat1BCheck_CheckedChanged(object sender, EventArgs e)
        {
            Cat1A.Checked = false;
            Cat1S.Checked = false;
        }

        private void Cat1SCheck_CheckedChanged(object sender, EventArgs e)
        {
            Cat1A.Checked = false;
            Cat1B.Checked = false;
        }
        
        private void AdjustBookGB_Enter(object sender, EventArgs e)
        {

        }
        
        private void btnaddImage2_Click(object sender, EventArgs e)
        {
            openFileDialogDOcpic1.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialogDOcpic1.FileName = null;
            resultDocpic1 = openFileDialogDOcpic1.ShowDialog();

            GetFileDoc();
        }

        private void btnaddImage3_Click(object sender, EventArgs e)
        {
            openFileDialogDOcpic2.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialogDOcpic2.FileName = null;
            resultDocpic2 = openFileDialogDOcpic2.ShowDialog();

            GetFileDoc2();
        }

        private void date1_ValueChanged(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManWPSTDetail.SqlStatement = "Select DATEPART(ISOWK,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "')";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            WkLbl2.EditValue = _dbManWPSTDetail.ResultsDataTable.Rows[0][0].ToString();
            WkLbl.Text = WkLbl2.EditValue.ToString();
        }

        private void cmbx1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void label79_Click(object sender, EventArgs e)
        {

        }

        private void Cat1NotesTxt_TextChanged(object sender, EventArgs e)
        {

        }

        //Callie
        private void Cat2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat2.SelectedIndex == 0)
            {
                pbx2SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat2Checked = 0;
            }

            if (Cat2.SelectedIndex == 1)
            {

                pbx2SVG.SvgImage = pbxRedSVG.SvgImage;
                cat2Checked = 1;
            }

            if (Cat2.SelectedIndex == 2)
            {

                pbx2SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat2Checked = 2;
            }

            if (Cat2.SelectedIndex == 3)
            {

                pbx2SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat2Checked = 3;
            }
            if (Cat2.SelectedIndex == 4)
            {

                pbx2SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat2Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat3.SelectedIndex == 0)
            {
                pbx3SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat3Checked = 0;
            }
            if (Cat3.SelectedIndex == 1)
            {
                pbx3SVG.SvgImage = pbxRedSVG.SvgImage;
                cat3Checked = 1;
            }
            if (Cat3.SelectedIndex == 2)
            {
                pbx3SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat3Checked = 2;
            }
            if (Cat3.SelectedIndex == 3)
            {
                pbx3SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat3Checked = 3;
            }
            if (Cat3.SelectedIndex == 4)
            {
                pbx3SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat3Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat4.SelectedIndex == 0)
            {
                pbx4SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat4Checked = 0;
            }
            if (Cat4.SelectedIndex == 1)
            {
                pbx4SVG.SvgImage = pbxRedSVG.SvgImage;
                cat4Checked = 1;
            }
            if (Cat4.SelectedIndex == 2)
            {
                pbx4SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat4Checked = 2;
            }
            if (Cat4.SelectedIndex == 3)
            {
                pbx4SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat4Checked = 3;
            }
            if (Cat4.SelectedIndex == 4)
            {
                pbx4SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat4Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat5.SelectedIndex == 0)
            {
                pbx5SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat5Checked = 0;
            }
            if (Cat5.SelectedIndex == 1)
            {
                pbx5SVG.SvgImage = pbxRedSVG.SvgImage;
                cat5Checked = 1;
            }
            if (Cat5.SelectedIndex == 2)
            {
                pbx5SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat5Checked = 2;
            }
            if (Cat5.SelectedIndex == 3)
            {
                pbx5SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat5Checked = 3;
            }
            if (Cat5.SelectedIndex == 4)
            {
                pbx5SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat5Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat6.SelectedIndex == 0)
            {
                pbx6SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat6Checked = 0;
            }
            if (Cat6.SelectedIndex == 1)
            {
                pbx6SVG.SvgImage = pbxRedSVG.SvgImage;
                cat6Checked = 1;
            }
            if (Cat6.SelectedIndex == 2)
            {
                pbx6SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat6Checked = 2;
            }
            if (Cat6.SelectedIndex == 3)
            {
                pbx6SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat6Checked = 3;
            }
            if (Cat6.SelectedIndex == 4)
            {
                pbx6SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat6Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat7.SelectedIndex == 0)
            {
                pbx7SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat7Checked = 0;
            }
            if (Cat7.SelectedIndex == 1)
            {
                pbx7SVG.SvgImage = pbxRedSVG.SvgImage;
                cat7Checked = 1;
            }
            if (Cat7.SelectedIndex == 2)
            {
                pbx7SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat7Checked = 2;
            }
            if (Cat7.SelectedIndex == 3)
            {
                pbx7SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat7Checked = 3;
            }
            if (Cat7.SelectedIndex == 4)
            {
                pbx7SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat7Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat8.SelectedIndex == 0)
            {
                pbx8SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat8Checked = 0;
            }
            if (Cat8.SelectedIndex == 1)
            {
                pbx8SVG.SvgImage = pbxRedSVG.SvgImage;
                cat8Checked = 1;
            }
            if (Cat8.SelectedIndex == 2)
            {
                pbx8SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat8Checked = 2;
            }

            if (Cat8.SelectedIndex == 3)
            {
                pbx8SVG.BringToFront();
                pbx8SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat8Checked = 3;
            }
            if (Cat8.SelectedIndex == 4)
            {
                pbx8SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat8Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat9.SelectedIndex == 0)
            {
                pbx9SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat9Checked = 0;
            }
            if (Cat9.SelectedIndex == 1)
            {
                pbx9SVG.BringToFront();
                pbx9SVG.SvgImage = pbxRedSVG.SvgImage;
                cat9Checked = 1;
            }
            if (Cat9.SelectedIndex == 2)
            {
                pbx9SVG.BringToFront();
                pbx9SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat9Checked = 2;
            }
            if (Cat9.SelectedIndex == 3)
            {
                pbx9SVG.BringToFront();
                pbx9SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat9Checked = 3;
            }
            if (Cat9.SelectedIndex == 4)
            {
                pbx9SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat9Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat10.SelectedIndex == 0)
            {
                pbx10SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat10Checked = 0;
            }
            if (Cat10.SelectedIndex == 1)
            {
                pbx10SVG.BringToFront();
                pbx10SVG.SvgImage = pbxRedSVG.SvgImage;
                cat10Checked = 1;
            }
            if (Cat10.SelectedIndex == 2)
            {
                pbx10SVG.BringToFront();
                pbx10SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat10Checked = 2;
            }
            if (Cat10.SelectedIndex == 3)
            {
                pbx10SVG.BringToFront();
                pbx10SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat10Checked = 3;
            }
            if (Cat10.SelectedIndex == 4)
            {
                pbx10SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat10Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat11.SelectedIndex == 0)
            {
                pbx11SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat1Checked = 0;
            }
            if (Cat11.SelectedIndex == 1)
            {
                pbx11SVG.BringToFront();
                pbx11SVG.SvgImage = pbxRedSVG.SvgImage;
                cat11Checked = 1;
            }
            if (Cat11.SelectedIndex == 2)
            {
                pbx11SVG.BringToFront();
                pbx11SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat11Checked = 2;
            }
            if (Cat11.SelectedIndex == 3)
            {
                pbx11SVG.BringToFront();
                pbx11SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat11Checked = 3;
            }
            if (Cat11.SelectedIndex == 4)
            {
                pbx11SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat11Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat12.SelectedIndex == 0)
            {
                pbx12SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat12Checked = 0;
            }
            if (Cat12.SelectedIndex == 1)
            {
                pbx12SVG.BringToFront();
                pbx12SVG.SvgImage = pbxRedSVG.SvgImage;
                cat12Checked = 1;
            }
            if (Cat12.SelectedIndex == 2)
            {
                pbx12SVG.BringToFront();
                pbx12SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat12Checked = 2;
            }
            if (Cat12.SelectedIndex == 3)
            {
                pbx12SVG.BringToFront();
                pbx12SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat12Checked = 3;
            }
            if (Cat12.SelectedIndex == 4)
            {
                pbx12SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat12Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat13.SelectedIndex == 0)
            {
                pbx13SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat13Checked = 0;
            }
            if (Cat13.SelectedIndex == 1)
            {
                pbx13SVG.BringToFront();
                pbx13SVG.SvgImage = pbxRedSVG.SvgImage;
                cat13Checked = 1;
            }
            if (Cat13.SelectedIndex == 2)
            {
                pbx13SVG.BringToFront();
                pbx13SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat13Checked = 2;
            }
            if (Cat13.SelectedIndex == 3)
            {
                pbx13SVG.BringToFront();
                pbx13SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat13Checked = 3;
            }
            if (Cat13.SelectedIndex == 4)
            {
                pbx13SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat13Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat14_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat14.SelectedIndex == 0)
            {
                pbx14SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat14Checked = 0;
            }
            if (Cat14.SelectedIndex == 1)
            {
                pbx14SVG.BringToFront();
                pbx14SVG.SvgImage = pbxRedSVG.SvgImage;
                cat14Checked = 1;
            }
            if (Cat14.SelectedIndex == 2)
            {
                pbx14SVG.BringToFront();
                pbx14SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat14Checked = 2;
            }
            if (Cat14.SelectedIndex == 3)
            {
                pbx14SVG.BringToFront();
                pbx14SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat14Checked = 3;
            }
            if (Cat14.SelectedIndex == 4)
            {
                pbx14SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat14Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat15_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat15.SelectedIndex == 0)
            {
                pbx15SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat15Checked = 0;
            }
            if (Cat15.SelectedIndex == 1)
            {
                pbx15SVG.BringToFront();
                pbx15SVG.SvgImage = pbxRedSVG.SvgImage;
                cat15Checked = 1;
            }
            if (Cat15.SelectedIndex == 2)
            {
                pbx15SVG.BringToFront();
                pbx15SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat15Checked = 2;
            }
            if (Cat15.SelectedIndex == 3)
            {
                pbx15SVG.BringToFront();
                pbx15SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat15Checked = 3;
            }
            if (Cat15.SelectedIndex == 4)
            {
                pbx15SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat15Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat16_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat16.SelectedIndex == 0)
            {
                pbx16SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat16Checked = 0;
            }
            if (Cat16.SelectedIndex == 1)
            {
                pbx16SVG.BringToFront();
                pbx16SVG.SvgImage = pbxRedSVG.SvgImage;
                cat16Checked = 1;
            }
            if (Cat16.SelectedIndex == 2)
            {
                pbx16SVG.BringToFront();
                pbx16SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat16Checked = 2;
            }
            if (Cat16.SelectedIndex == 3)
            {
                pbx16SVG.BringToFront();
                pbx16SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat16Checked = 3;
            }
            if (Cat16.SelectedIndex == 4)
            {
                pbx16SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat16Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat17_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat17.SelectedIndex == 0)
            {
                pbx17SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat17Checked = 0;
            }
            if (Cat17.SelectedIndex == 1)
            {
                pbx17SVG.BringToFront();
                pbx17SVG.SvgImage = pbxRedSVG.SvgImage;
                cat17Checked = 1;
            }
            if (Cat17.SelectedIndex == 2)
            {
                pbx17SVG.BringToFront();
                pbx17SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat17Checked = 2;
            }
            if (Cat17.SelectedIndex == 3)
            {
                pbx17SVG.BringToFront();
                pbx17SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat17Checked = 3;
            }
            if (Cat17.SelectedIndex == 4)
            {
                pbx17SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat17Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat18_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat18.SelectedIndex == 0)
            {
                pbx18SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat18Checked = 0;
            }

            if (Cat18.SelectedIndex == 1)
            {
                pbx18SVG.BringToFront();
                pbx18SVG.SvgImage = pbxRedSVG.SvgImage;
                cat18Checked = 1;
            }

            if (Cat18.SelectedIndex == 2)
            {
                pbx18SVG.BringToFront();
                pbx18SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat18Checked = 2;
            }

            if (Cat18.SelectedIndex == 3)
            {
                pbx18SVG.BringToFront();
                pbx18SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat18Checked = 3;
            }
            if (Cat18.SelectedIndex == 4)
            {
                pbx18SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat18Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat19_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat19.SelectedIndex == 0)
            {
                pbx19SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat19Checked = 0;
            }

            if (Cat19.SelectedIndex == 1)
            {
                pbx19SVG.BringToFront();
                pbx19SVG.SvgImage = pbxRedSVG.SvgImage;
                cat19Checked = 1;
            }

            if (Cat19.SelectedIndex == 2)
            {
                pbx19SVG.BringToFront();
                pbx19SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat19Checked = 2;
            }

            if (Cat19.SelectedIndex == 3)
            {
                pbx19SVG.BringToFront();
                pbx19SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat19Checked = 3;
            }
            if (Cat19.SelectedIndex == 4)
            {
                pbx19SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat19Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat20_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat20.SelectedIndex == 0)
            {
                pbx20SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat20Checked = 0;
            }

            if (Cat20.SelectedIndex == 1)
            {
                pbx20SVG.BringToFront();
                pbx20SVG.SvgImage = pbxRedSVG.SvgImage;
                cat20Checked = 1;
            }

            if (Cat20.SelectedIndex == 2)
            {
                pbx20SVG.BringToFront();
                pbx20SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat20Checked = 2;
            }

            if (Cat20.SelectedIndex == 3)
            {
                pbx20SVG.BringToFront();
                pbx20SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat20Checked = 3;
            }
            if (Cat20.SelectedIndex == 4)
            {
                pbx20SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat20Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat21_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat21.SelectedIndex == 0)
            {
                pbx21SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat21Checked = 0;
            }

            if (Cat21.SelectedIndex == 1)
            {
                pbx21SVG.BringToFront();
                pbx21SVG.SvgImage = pbxRedSVG.SvgImage;
                cat21Checked = 1;
            }

            if (Cat21.SelectedIndex == 2)
            {
                pbx21SVG.BringToFront();
                pbx21SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat21Checked = 2;
            }

            if (Cat21.SelectedIndex == 3)
            {
                pbx21SVG.BringToFront();
                pbx21SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat21Checked = 3;
            }
            if (Cat21.SelectedIndex == 4)
            {
                pbx21SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat21Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat22_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat22.SelectedIndex == 0)
            {
                pbx22SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat22Checked = 0;
            }

            if (Cat22.SelectedIndex == 1)
            {
                pbx22SVG.BringToFront();
                pbx22SVG.SvgImage = pbxRedSVG.SvgImage;
                cat22Checked = 1;
            }

            if (Cat22.SelectedIndex == 2)
            {
                pbx22SVG.BringToFront();
                pbx22SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat22Checked = 2;
            }

            if (Cat22.SelectedIndex == 3)
            {
                pbx22SVG.BringToFront();
                pbx22SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat22Checked = 3;
            }
            if (Cat22.SelectedIndex == 4)
            {
                pbx22SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat22Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat23_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat23.SelectedIndex == 0)
            {
                pbx23SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat23Checked = 0;
            }

            if (Cat23.SelectedIndex == 1)
            {
                pbx23SVG.BringToFront();
                pbx23SVG.SvgImage = pbxRedSVG.SvgImage;
                cat23Checked = 1;
            }

            if (Cat23.SelectedIndex == 2)
            {
                pbx23SVG.BringToFront();
                pbx23SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat23Checked = 2;
            }

            if (Cat23.SelectedIndex == 3)
            {
                pbx23SVG.BringToFront();
                pbx23SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat23Checked = 3;
            }
            if (Cat23.SelectedIndex == 4)
            {
                pbx23SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat23Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat24_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat24.SelectedIndex == 0)
            {
                pbx24SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat24Checked = 0;
            }

            if (Cat24.SelectedIndex == 1)
            {
                pbx24SVG.BringToFront();
                pbx24SVG.SvgImage = pbxRedSVG.SvgImage;
                cat24Checked = 1;
            }

            if (Cat24.SelectedIndex == 2)
            {
                pbx24SVG.BringToFront();
                pbx24SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat24Checked = 2;
            }

            if (Cat24.SelectedIndex == 3)
            {
                pbx24SVG.BringToFront();
                pbx24SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat24Checked = 3;
            }
            if (Cat24.SelectedIndex == 4)
            {
                pbx24SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat24Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat25_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat25.SelectedIndex == 0)
            {
                pbx25SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat25Checked = 0;
            }

            if (Cat25.SelectedIndex == 1)
            {
                pbx25SVG.BringToFront();
                pbx25SVG.SvgImage = pbxRedSVG.SvgImage;
                cat25Checked = 1;
            }

            if (Cat25.SelectedIndex == 2)
            {
                pbx25SVG.BringToFront();
                pbx25SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat25Checked = 2;
            }

            if (Cat25.SelectedIndex == 3)
            {
                pbx25SVG.BringToFront();
                pbx25SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat25Checked = 3;
            }
            if (Cat25.SelectedIndex == 4)
            {
                pbx25SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat25Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat26_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat26.SelectedIndex == 0)
            {
                pbx26SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat26Checked = 0;
            }

            if (Cat26.SelectedIndex == 1)
            {
                pbx26SVG.BringToFront();
                pbx26SVG.SvgImage = pbxRedSVG.SvgImage;
                cat26Checked = 1;
            }

            if (Cat26.SelectedIndex == 2)
            {
                pbx26SVG.BringToFront();
                pbx26SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat26Checked = 2;
            }

            if (Cat26.SelectedIndex == 3)
            {
                pbx26SVG.BringToFront();
                pbx26SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat26Checked = 3;
            }
            if (Cat26.SelectedIndex == 4)
            {
                pbx26SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat26Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat27_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat27.SelectedIndex == 0)
            {
                pbx27SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat27Checked = 0;
            }

            if (Cat27.SelectedIndex == 1)
            {
                pbx27SVG.BringToFront();
                pbx27SVG.SvgImage = pbxRedSVG.SvgImage;
                cat27Checked = 1;
            }

            if (Cat27.SelectedIndex == 2)
            {
                pbx27SVG.BringToFront();
                pbx27SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat27Checked = 2;
            }

            if (Cat27.SelectedIndex == 3)
            {
                pbx27SVG.BringToFront();
                pbx27SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat27Checked = 3;
            }
            if (Cat27.SelectedIndex == 4)
            {
                pbx27SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat27Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat28_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat28.SelectedIndex == 0)
            {
                pbx28SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat28Checked = 0;
            }

            if (Cat28.SelectedIndex == 1)
            {
                pbx28SVG.BringToFront();
                pbx28SVG.SvgImage = pbxRedSVG.SvgImage;
                cat28Checked = 1;
            }

            if (Cat28.SelectedIndex == 2)
            {
                pbx28SVG.BringToFront();
                pbx28SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat28Checked = 2;
            }

            if (Cat28.SelectedIndex == 3)
            {
                pbx28SVG.BringToFront();
                pbx28SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat28Checked = 3;
            }
            if (Cat28.SelectedIndex == 4)
            {
                pbx28SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat28Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat29_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat29.SelectedIndex == 0)
            {
                pbx29SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat29Checked = 0;
            }

            if (Cat29.SelectedIndex == 1)
            {
                pbx29SVG.BringToFront();
                pbx29SVG.SvgImage = pbxRedSVG.SvgImage;
                cat29Checked = 1;
            }

            if (Cat29.SelectedIndex == 2)
            {
                pbx29SVG.BringToFront();
                pbx29SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat29Checked = 2;
            }

            if (Cat29.SelectedIndex == 3)
            {
                pbx29SVG.BringToFront();
                pbx29SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat29Checked = 3;
            }
            if (Cat29.SelectedIndex == 4)
            {
                pbx29SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat29Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat30_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat30.SelectedIndex == 0)
            {
                pbx30SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat30Checked = 0;
            }

            if (Cat30.SelectedIndex == 1)
            {
                pbx30SVG.BringToFront();
                pbx30SVG.SvgImage = pbxRedSVG.SvgImage;
                cat30Checked = 1;
            }

            if (Cat30.SelectedIndex == 2)
            {
                pbx30SVG.BringToFront();
                pbx30SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat30Checked = 2;
            }

            if (Cat30.SelectedIndex == 3)
            {
                pbx30SVG.BringToFront();
                pbx30SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat30Checked = 3;
            }
            if (Cat30.SelectedIndex == 4)
            {
                pbx30SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat30Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat31_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat31.SelectedIndex == 0)
            {
                pbx31SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat31Checked = 0;
            }

            if (Cat31.SelectedIndex == 1)
            {
                pbx31SVG.BringToFront();
                pbx31SVG.SvgImage = pbxRedSVG.SvgImage;
                cat31Checked = 1;
            }

            if (Cat31.SelectedIndex == 2)
            {
                pbx13SVG.BringToFront();
                pbx31SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat31Checked = 2;
            }

            if (Cat31.SelectedIndex == 3)
            {
                pbx31SVG.BringToFront();
                pbx31SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat31Checked = 3;
            }
            if (Cat31.SelectedIndex == 4)
            {
                pbx31SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat31Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat32_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat32.SelectedIndex == 0)
            {
                pbx32SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat32Checked = 0;
            }

            if (Cat32.SelectedIndex == 1)
            {
                pbx32SVG.BringToFront();
                pbx32SVG.SvgImage = pbxRedSVG.SvgImage;
                cat32Checked = 1;
            }

            if (Cat32.SelectedIndex == 2)
            {
                pbx32SVG.BringToFront();
                pbx32SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat32Checked = 2;
            }

            if (Cat32.SelectedIndex == 3)
            {
                pbx32SVG.BringToFront();
                pbx32SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat32Checked = 3;
            }
            if (Cat32.SelectedIndex == 4)
            {
                pbx32SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat32Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat33_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat33.SelectedIndex == 0)
            {
                pbx33SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat3Checked = 0;
            }

            if (Cat3.SelectedIndex == 1)
            {
                pbx33SVG.BringToFront();
                pbx33SVG.SvgImage = pbxRedSVG.SvgImage;
                cat33Checked = 1;
            }

            if (Cat33.SelectedIndex == 2)
            {
                pbx33SVG.BringToFront();
                pbx33SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat33Checked = 2;
            }

            if (Cat33.SelectedIndex == 3)
            {
                pbx33SVG.BringToFront();
                pbx33SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat33Checked = 3;
            }
            if (Cat33.SelectedIndex == 4)
            {
                pbx33SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat33Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat34_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat34.SelectedIndex == 0)
            {
                pbx34SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat34Checked = 0;
            }

            if (Cat34.SelectedIndex == 1)
            {
                pbx34SVG.BringToFront();
                pbx34SVG.SvgImage = pbxRedSVG.SvgImage;
                cat34Checked = 1;
            }

            if (Cat34.SelectedIndex == 2)
            {
                pbx34SVG.BringToFront();
                pbx34SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat34Checked = 2;
            }

            if (Cat34.SelectedIndex == 3)
            {
                pbx34SVG.BringToFront();
                pbx34SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat34Checked = 3;
            }
            if (Cat34.SelectedIndex == 4)
            {
                pbx34SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat34Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat35_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat35.SelectedIndex == 0)
            {
                pbx35SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat35Checked = 0;
            }

            if (Cat35.SelectedIndex == 1)
            {
                pbx35SVG.BringToFront();
                pbx35SVG.SvgImage = pbxRedSVG.SvgImage;
                cat35Checked = 1;
            }

            if (Cat35.SelectedIndex == 2)
            {
                pbx35SVG.BringToFront();
                pbx35SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat35Checked = 2;
            }

            if (Cat35.SelectedIndex == 3)
            {
                pbx35SVG.BringToFront();
                pbx35SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat35Checked = 3;
            }

            if (Cat35.SelectedIndex == 4)
            {
                pbx35SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat35Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat36_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat36.SelectedIndex == 0)
            {
                pbx36SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat36Checked = 0;
            }

            if (Cat36.SelectedIndex == 1)
            {
                pbx36SVG.BringToFront();
                pbx36SVG.SvgImage = pbxRedSVG.SvgImage;
                cat36Checked = 1;
            }

            if (Cat36.SelectedIndex == 2)
            {
                pbx36SVG.BringToFront();
                pbx36SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat36Checked = 2;
            }

            if (Cat36.SelectedIndex == 3)
            {
                pbx36SVG.BringToFront();
                pbx36SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat36Checked = 3;
            }

            if (Cat36.SelectedIndex == 4)
            {
                pbx36SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat36Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat37_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat37.SelectedIndex == 0)
            {
                pbx37SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat37Checked = 0;
            }

            if (Cat37.SelectedIndex == 1)
            {
                pbx37SVG.BringToFront();
                pbx37SVG.SvgImage = pbxRedSVG.SvgImage;
                cat37Checked = 1;
            }

            if (Cat37.SelectedIndex == 2)
            {
                pbx37SVG.BringToFront();
                pbx37SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat37Checked = 2;
            }

            if (Cat37.SelectedIndex == 3)
            {
                pbx37SVG.BringToFront();
                pbx37SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat37Checked = 3;
            }
            if (Cat37.SelectedIndex == 4)
            {
                pbx37SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat37Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat38_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat38.SelectedIndex == 0)
            {
                pbx38SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat38Checked = 0;
            }

            if (Cat38.SelectedIndex == 1)
            {
                pbx38SVG.BringToFront();
                pbx38SVG.SvgImage = pbxRedSVG.SvgImage;
                cat38Checked = 1;
            }

            if (Cat38.SelectedIndex == 2)
            {
                pbx38SVG.BringToFront();
                pbx38SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat38Checked = 2;
            }

            if (Cat38.SelectedIndex == 3)
            {
                pbx38SVG.BringToFront();
                pbx38SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat38Checked = 3;
            }
            if (Cat38.SelectedIndex == 4)
            {
                pbx38SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat38Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat39_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat39.SelectedIndex == 0)
            {
                pbx39SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat39Checked = 0;
            }
            if (Cat39.SelectedIndex == 1)
            {
                pbx39SVG.BringToFront();
                pbx39SVG.SvgImage = pbxRedSVG.SvgImage;
                cat39Checked = 1;
            }
            if (Cat39.SelectedIndex == 2)
            {
                pbx39SVG.BringToFront();
                pbx39SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat39Checked = 2;
            }
            if (Cat39.SelectedIndex == 3)
            {
                pbx39SVG.BringToFront();
                pbx39SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat39Checked = 3;
            }
            if (Cat39.SelectedIndex == 4)
            {
                pbx39SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat39Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat40_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat40.SelectedIndex == 0)
            {
                pbx40SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat40Checked = 0;
            }
            if (Cat40.SelectedIndex == 1)
            {
                pbx40SVG.BringToFront();
                pbx40SVG.SvgImage = pbxRedSVG.SvgImage;
                cat40Checked = 1;
            }
            if (Cat40.SelectedIndex == 2)
            {
                pbx40SVG.BringToFront();
                pbx40SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat40Checked = 2;
            }
            if (Cat40.SelectedIndex == 3)
            {
                pbx40SVG.BringToFront();
                pbx40SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat40Checked = 3;
            }
            if (Cat40.SelectedIndex == 4)
            {
                pbx40SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat40Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat41_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat41.SelectedIndex == 0)
            {
                pbx41SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat41Checked = 0;
            }
            if (Cat41.SelectedIndex == 1)
            {
                pbx41SVG.BringToFront();
                pbx41SVG.SvgImage = pbxRedSVG.SvgImage;
                cat41Checked = 1;
            }
            if (Cat41.SelectedIndex == 2)
            {
                pbx41SVG.BringToFront();
                pbx41SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat41Checked = 2;
            }
            if (Cat41.SelectedIndex == 3)
            {
                pbx41SVG.BringToFront();
                pbx41SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat41Checked = 3;
            }
            if (Cat41.SelectedIndex == 4)
            {
                pbx41SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat41Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat42_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat42.SelectedIndex == 0)
            {
                pbx42SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat42Checked = 0;
            }
            if (Cat42.SelectedIndex == 1)
            {
                pbx42SVG.BringToFront();
                pbx42SVG.SvgImage = pbxRedSVG.SvgImage;
                cat42Checked = 1;
            }
            if (Cat42.SelectedIndex == 2)
            {
                pbx42SVG.BringToFront();
                pbx42SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat42Checked = 2;
            }
            if (Cat42.SelectedIndex == 3)
            {
                pbx42SVG.BringToFront();
                pbx42SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat42Checked = 3;
            }
            if (Cat42.SelectedIndex == 4)
            {
                pbx42SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat42Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat43_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat43.SelectedIndex == 0)
            {
                pbx43SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat43Checked = 0;
            }
            if (Cat43.SelectedIndex == 1)
            {
                pbx43SVG.BringToFront();
                pbx43SVG.SvgImage = pbxRedSVG.SvgImage;
                cat43Checked = 1;
            }
            if (Cat43.SelectedIndex == 2)
            {
                pbx43SVG.BringToFront();
                pbx43SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat43Checked = 2;
            }
            if (Cat43.SelectedIndex == 3)
            {
                pbx43SVG.BringToFront();
                pbx43SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat43Checked = 3;
            }
            if (Cat43.SelectedIndex == 4)
            {
                pbx43SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat43Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat44_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat44.SelectedIndex == 0)
            {
                pbx44SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat44Checked = 0;
            }
            if (Cat44.SelectedIndex == 1)
            {
                pbx44SVG.BringToFront();
                pbx44SVG.SvgImage = pbxRedSVG.SvgImage;
                cat44Checked = 1;
            }
            if (Cat44.SelectedIndex == 2)
            {
                pbx44SVG.BringToFront();
                pbx44SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat44Checked = 2;
            }
            if (Cat44.SelectedIndex == 3)
            {
                pbx44SVG.BringToFront();
                pbx44SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat44Checked = 3;
            }
            if (Cat44.SelectedIndex == 4)
            {
                pbx44SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat44Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat45_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat45.SelectedIndex == 0)
            {
                pbx45SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat45Checked = 0;
            }
            if (Cat45.SelectedIndex == 1)
            {
                pbx45SVG.BringToFront();
                pbx45SVG.SvgImage = pbxRedSVG.SvgImage;
                cat45Checked = 1;
            }
            if (Cat45.SelectedIndex == 2)
            {
                pbx45SVG.BringToFront();
                pbx45SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat45Checked = 2;
            }
            if (Cat45.SelectedIndex == 3)
            {
                pbx45SVG.BringToFront();
                pbx45SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat45Checked = 3;
            }
            if (Cat45.SelectedIndex == 4)
            {
                pbx45SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat45Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat46_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat46.SelectedIndex == 0)
            {
                pbx46SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat46Checked = 0;
            }
            if (Cat46.SelectedIndex == 1)
            {
                pbx46SVG.BringToFront();
                pbx46SVG.SvgImage = pbxRedSVG.SvgImage;
                cat46Checked = 1;
            }
            if (Cat46.SelectedIndex == 2)
            {
                pbx46SVG.BringToFront();
                pbx46SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat46Checked = 2;
            }
            if (Cat46.SelectedIndex == 3)
            {
                pbx46SVG.BringToFront();
                pbx46SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat46Checked = 3;
            }
            if (Cat46.SelectedIndex == 4)
            {
                pbx46SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat46Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat47_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat47.SelectedIndex == 0)
            {
                pbx47SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat47Checked = 0;
            }
            if (Cat47.SelectedIndex == 1)
            {
                pbx47SVG.BringToFront();
                pbx47SVG.SvgImage = pbxRedSVG.SvgImage;
                cat47Checked = 1;
            }
            if (Cat47.SelectedIndex == 2)
            {
                pbx47SVG.BringToFront();
                pbx47SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat47Checked = 2;
            }
            if (Cat47.SelectedIndex == 3)
            {
                pbx47SVG.BringToFront();
                pbx47SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat47Checked = 3;
            }
            if (Cat47.SelectedIndex == 4)
            {
                pbx47SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat47Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat48_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat48.SelectedIndex == 0)
            {
                pbx48SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat48Checked = 0;
            }
            if (Cat48.SelectedIndex == 1)
            {
                pbx48SVG.BringToFront();
                pbx48SVG.SvgImage = pbxRedSVG.SvgImage;
                cat48Checked = 1;
            }
            if (Cat48.SelectedIndex == 2)
            {
                pbx48SVG.BringToFront();
                pbx48SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat48Checked = 2;
            }
            if (Cat48.SelectedIndex == 3)
            {
                pbx48SVG.BringToFront();
                pbx48SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat48Checked = 3;
            }
            if (Cat48.SelectedIndex == 4)
            {
                pbx48SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat48Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat49_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat49.SelectedIndex == 0)
            {
                pbx49SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat49Checked = 0;
            }
            if (Cat49.SelectedIndex == 1)
            {
                pbx49SVG.BringToFront();
                pbx49SVG.SvgImage = pbxRedSVG.SvgImage;
                cat49Checked = 1;
            }
            if (Cat49.SelectedIndex == 2)
            {
                pbx49SVG.BringToFront();
                pbx49SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat49Checked = 2;
            }
            if (Cat49.SelectedIndex == 3)
            {
                pbx49SVG.BringToFront();
                pbx49SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat49Checked = 3;
            }
            if (Cat49.SelectedIndex == 4)
            {
                pbx49SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat49Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat50_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat50.SelectedIndex == 0)
            {
                pbx50SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat50Checked = 0;
            }
            if (Cat50.SelectedIndex == 1)
            {
                pbx50SVG.BringToFront();
                pbx50SVG.SvgImage = pbxRedSVG.SvgImage;
                cat50Checked = 1;
            }
            if (Cat50.SelectedIndex == 2)
            {
                pbx50SVG.BringToFront();
                pbx50SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat50Checked = 2;
            }
            if (Cat50.SelectedIndex == 3)
            {
                pbx50SVG.BringToFront();
                pbx50SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat50Checked = 3;
            }
            if (Cat50.SelectedIndex == 4)
            {
                pbx50SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat50Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat51_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat51.SelectedIndex == 0)
            {
                pbx51SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat51Checked = 0;
            }
            if (Cat51.SelectedIndex == 1)
            {
                pbx51SVG.BringToFront();
                pbx51SVG.SvgImage = pbxRedSVG.SvgImage;
                cat51Checked = 1;
            }
            if (Cat51.SelectedIndex == 2)
            {
                pbx51SVG.BringToFront();
                pbx51SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat51Checked = 2;
            }
            if (Cat51.SelectedIndex == 3)
            {
                pbx51SVG.BringToFront();
                pbx51SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat51Checked = 3;
            }
            if (Cat51.SelectedIndex == 4)
            {
                pbx51SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat51Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat52_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat52.SelectedIndex == 0)
            {
                pbx52SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat52Checked = 0;
            }
            if (Cat52.SelectedIndex == 1)
            {
                pbx52SVG.BringToFront();
                pbx52SVG.SvgImage = pbxRedSVG.SvgImage;
                cat52Checked = 1;
            }
            if (Cat52.SelectedIndex == 2)
            {
                pbx52SVG.BringToFront();
                pbx52SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat52Checked = 2;
            }
            if (Cat52.SelectedIndex == 3)
            {
                pbx52SVG.BringToFront();
                pbx52SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat52Checked = 3;
            }
            if (Cat52.SelectedIndex == 4)
            {
                pbx52SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat52Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat53_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat53.SelectedIndex == 0)
            {
                pbx53SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat53Checked = 0;
            }
            if (Cat53.SelectedIndex == 1)
            {
                pbx53SVG.BringToFront();
                pbx53SVG.SvgImage = pbxRedSVG.SvgImage;
                cat53Checked = 1;
            }
            if (Cat53.SelectedIndex == 2)
            {
                pbx53SVG.BringToFront();
                pbx53SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat53Checked = 2;
            }
            if (Cat53.SelectedIndex == 3)
            {
                pbx53SVG.BringToFront();
                pbx53SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat53Checked = 3;
            }
            if (Cat53.SelectedIndex == 4)
            {
                pbx53SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat53Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat54_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat54.SelectedIndex == 0)
            {
                pbx54SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat54Checked = 0;
            }
            if (Cat54.SelectedIndex == 1)
            {
                pbx54SVG.BringToFront();
                pbx54SVG.SvgImage = pbxRedSVG.SvgImage;
                cat54Checked = 1;
            }
            if (Cat54.SelectedIndex == 2)
            {
                pbx54SVG.BringToFront();
                pbx54SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat54Checked = 2;
            }
            if (Cat54.SelectedIndex == 3)
            {
                pbx54SVG.BringToFront();
                pbx54SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat54Checked = 3;
            }
            if (Cat54.SelectedIndex == 4)
            {
                pbx54SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat54Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat55_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat55.SelectedIndex == 0)
            {
                pbx55SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat55Checked = 0;
            }
            if (Cat55.SelectedIndex == 1)
            {
                pbx55SVG.BringToFront();
                pbx55SVG.SvgImage = pbxRedSVG.SvgImage;
                cat55Checked = 1;
            }
            if (Cat55.SelectedIndex == 2)
            {
                pbx55SVG.BringToFront();
                pbx55SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat55Checked = 2;
            }
            if (Cat55.SelectedIndex == 3)
            {
                pbx55SVG.BringToFront();
                pbx55SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat55Checked = 3;
            }
            if (Cat55.SelectedIndex == 4)
            {
                pbx55SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat55Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat56_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat56.SelectedIndex == 0)
            {
                pbx56SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat56Checked = 0;
            }
            if (Cat56.SelectedIndex == 1)
            {
                pbx56SVG.BringToFront();
                pbx56SVG.SvgImage = pbxRedSVG.SvgImage;
                cat56Checked = 1;
            }
            if (Cat56.SelectedIndex == 2)
            {
                pbx56SVG.BringToFront();
                pbx56SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat56Checked = 2;
            }
            if (Cat56.SelectedIndex == 3)
            {
                pbx56SVG.BringToFront();
                pbx56SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat56Checked = 3;
            }
            if (Cat56.SelectedIndex == 4)
            {
                pbx56SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat56Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat57_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat57.SelectedIndex == 0)
            {
                pbx57SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat5Checked = 0;
            }
            if (Cat57.SelectedIndex == 1)
            {
                pbx57SVG.BringToFront();
                pbx57SVG.SvgImage = pbxRedSVG.SvgImage;
                cat57Checked = 1;
            }
            if (Cat57.SelectedIndex == 2)
            {
                pbx57SVG.BringToFront();
                pbx57SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat57Checked = 2;
            }
            if (Cat57.SelectedIndex == 3)
            {
                pbx57SVG.BringToFront();
                pbx57SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat57Checked = 3;
            }
            if (Cat57.SelectedIndex == 4)
            {
                pbx57SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat57Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat58_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat58.SelectedIndex == 0)
            {
                cat58Checked = 0;
            }
            if (Cat58.SelectedIndex == 1)
            {
                cat58Checked = 1;
            }
            if (Cat58.SelectedIndex == 2)
            {
                cat58Checked = 2;
            }
            if (Cat58.SelectedIndex == 3)
            {
                cat58Checked = 3;
            }
            if (Cat58.SelectedIndex == 4)
            {
                cat58Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat59_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat59.SelectedIndex == 0)
            {
                cat59Checked = 0;
            }
            if (Cat59.SelectedIndex == 1)
            {
                cat59Checked = 1;
            }
            if (Cat59.SelectedIndex == 2)
            {
                cat59Checked = 2;
            }
            if (Cat59.SelectedIndex == 3)
            {
                cat59Checked = 3;
            }
            if (Cat59.SelectedIndex == 4)
            {
                cat59Checked = 4;
            }
            if (Cat59.SelectedIndex == 5)
            {
                cat59Checked = 5;
            }
            LoadPercentageCompleted();
        }

        private void Cat61_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat61.SelectedIndex == 0)
            {
                cat61Checked = 0;
            }
            if (Cat61.SelectedIndex == 1)
            {
                cat61Checked = 1;
            }
            if (Cat61.SelectedIndex == 2)
            {
                cat61Checked = 2;
            }
            if (Cat61.SelectedIndex == 3)
            {
                cat61Checked = 3;
            }
            if (Cat61.SelectedIndex == 4)
            {
                cat61Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void svgImageBox57_Click(object sender, EventArgs e)
        {

        }

        private void Cat50Note_TextChanged(object sender, EventArgs e)
        {

        }

        private void pcReport_Load(object sender, EventArgs e)
        {

        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            AddTypeBtn_Click(null, null);
        }
        private DrawItemEventArgs lastDrawn;
        private void Cat2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat3_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat4_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat5_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat6_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat7_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void gvCapture_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                SelectedQuestID = Convert.ToInt32(gvCapture.GetRowCellValue(e.RowHandle, "QuestID"));
                ValueType = gvCapture.GetRowCellValue(e.RowHandle, "ValueType").ToString();

                if (e.Column.FieldName == "Answer")
                {
                    if (SelectedQuestID != 0 && ValueType == "DropDownList")
                    {
                        e.RepositoryItem = repImage;
                    }

                    if (SelectedQuestID == 1)
                    {
                        e.RepositoryItem = repABS;
                    }

                    if (SelectedQuestID == 59)
                    {
                        e.RepositoryItem = repSurfaceRockMass;
                    }

                    if (SelectedQuestID == 60)
                    {
                        e.RepositoryItem = repStructure;
                    }

                    if (SelectedQuestID == 61)
                    {
                        e.RepositoryItem = repTextEdit;
                    }

                    if (SelectedQuestID == 62)
                    {
                        e.RepositoryItem = repSurfaceRockMass;
                    }

                    if (SelectedQuestID == 63)
                    {
                        e.RepositoryItem = repTextEdit;
                    }

                    if (SelectedQuestID == 64)
                    {
                        e.RepositoryItem = repTextEdit;
                    }
                }
            }
        }

        

        private void gvCapture_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            decimal Total;
            decimal Act;
             
            Total = 56; 
            Act = 0;   

            for (int i = 0; i < gvCapture.DataRowCount; i++)  
            {

                string aa = gvCapture.GetRowCellValue(i, "Answer").ToString();
                //foreach (DataRow r in dtData.Rows)
                //{
                    string quest = gvCapture.GetRowCellValue(i, "Question").ToString();
                    string answer = gvCapture.GetRowCellValue(e.RowHandle, "Answer").ToString();

                    if (gvCapture.GetRowCellValue(i, "QuestID").ToString() != "1" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "54" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "59" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "60" &&
                        gvCapture.GetRowCellValue(i, "QuestID").ToString() != "61" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "62" &&
                        gvCapture.GetRowCellValue(i, "QuestID").ToString() != "63" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "64")
                    {
                        if (gvCapture.GetRowCellValue(i, "Answer").ToString() == "0" && gvCapture.GetRowCellValue(i, "Answer").ToString() != "")
                        {
                            Act = Act + 1;
                        }
                        else if (gvCapture.GetRowCellValue(i, "Answer").ToString() != "0" && gvCapture.GetRowCellValue(i, "Answer").ToString() != "")
                        {
                            Act = Act - 1;
                        }
                    }
                //}
            }

            for (int i = 0; i < gvCapture.DataRowCount; i++)
            {
                //foreach (DataRow r in dtData.Rows)
                //{
                    string quest = gvCapture.GetRowCellValue(i, "Question").ToString();
                    string answer = gvCapture.GetRowCellValue(i, "Answer").ToString();

                    if (gvCapture.GetRowCellValue(i, "QuestID").ToString() != "1" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "54" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "59" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "60" &&
                        gvCapture.GetRowCellValue(i, "QuestID").ToString() != "61" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "62" &&
                        gvCapture.GetRowCellValue(i, "QuestID").ToString() != "63" && gvCapture.GetRowCellValue(i, "QuestID").ToString() != "64")
                    {
                        if (gvCapture.GetRowCellValue(i, "Answer").ToString() == "4")
                        {
                            Total = Total - 1;
                        }
                    }
                //}
            }

            RRlabel.EditValue = Convert.ToString(Math.Round(Convert.ToDecimal((Act / Total) * 100), 2));
        }

        private void NewSave()
        {
            bool testfailed = Forceinstructions();

            //if (testfailed)
            //{
            //    MessageBox.Show("Please fill in all Deviation Notes");
            //    return;
            //}

            ////Error Message for SB & MO
            //try
            //{
            //    if (cmbSB.EditValue.ToString() == "")
            //    {
            //        MessageBox.Show("Please enter a valid Shift Boss?");
            //        return;
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("Please enter a valid Shift Boss?");
            //    return;
            //}
            //try
            //{
            //    if (cmbMO.EditValue.ToString() == "[EditValue is null]")
            //    {
            //        MessageBox.Show("Please enter a valid Mine Overseer?");
            //        return;
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("Please enter a valid Shift Boss?");
            //    return;
            //}

            string WPStatus = string.Empty;
            string SB = string.Empty;
            string MO = string.Empty;

            //if (cmbSB.EditValue.ToString() != string.Empty)
            //{
            //    SB = cmbSB.EditValue.ToString();
            //}
            //else
            //{
            //    MessageBox.Show("No Shift Boss selected.");
            //    return;
            //}

            //if (cmbMO.EditValue.ToString() != string.Empty)
            //{
            //    MO = cmbMO.EditValue.ToString();
            //}
            //else
            //{
            //    MessageBox.Show("No Mine Overseer selected.");
            //    return;
            //}

            string sql = string.Empty;

            //Delete and Insert new Routine Visit
            MWDataManager.clsDataAccess _Delete = new MWDataManager.clsDataAccess();
            _Delete.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _Delete.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _Delete.queryReturnType = MWDataManager.ReturnType.DataTable;
            sql = sql + " Delete From [tbl_DPT_RockMechInspection] Where workplace = '" + WPLbl.EditValue + "' and CaptYear = datepart(YYYY,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "') and CaptWeek = '" + WkLbl2.EditValue + "' ";

            _Delete.SqlStatement = sql;

            string Answer1 = gvCapture.GetRowCellValue(0, "Answer").ToString();
            string Note1 = gvCapture.GetRowCellValue(0, "Note").ToString();
            string Observation1 = gvCapture.GetRowCellValue(0, "Observation").ToString();
            string Answer2 = gvCapture.GetRowCellValue(1, "Answer").ToString();
            string Note2 = gvCapture.GetRowCellValue(1, "Note").ToString();
            string Observation2 = gvCapture.GetRowCellValue(1, "Observation").ToString();
            string Answer3 = gvCapture.GetRowCellValue(2, "Answer").ToString();
            string Note3 = gvCapture.GetRowCellValue(2, "Note").ToString();
            string Observation3 = gvCapture.GetRowCellValue(2, "Observation").ToString();
            string Answer4 = gvCapture.GetRowCellValue(3, "Answer").ToString();
            string Note4 = gvCapture.GetRowCellValue(3, "Note").ToString();
            string Observation4 = gvCapture.GetRowCellValue(3, "Observation").ToString();
            string Answer5 = gvCapture.GetRowCellValue(4, "Answer").ToString();
            string Note5 = gvCapture.GetRowCellValue(4, "Note").ToString();
            string Observation5 = gvCapture.GetRowCellValue(4, "Observation").ToString();
            string Answer6 = gvCapture.GetRowCellValue(5, "Answer").ToString();
            string Note6 = gvCapture.GetRowCellValue(5, "Note").ToString();
            string Observation6 = gvCapture.GetRowCellValue(5, "Observation").ToString();
            string Answer7 = gvCapture.GetRowCellValue(6, "Answer").ToString();
            string Note7 = gvCapture.GetRowCellValue(6, "Note").ToString();
            string Observation7 = gvCapture.GetRowCellValue(6, "Observation").ToString();
            string Answer8 = gvCapture.GetRowCellValue(7, "Answer").ToString();
            string Note8 = gvCapture.GetRowCellValue(7, "Note").ToString();
            string Observation8 = gvCapture.GetRowCellValue(7, "Observation").ToString();
            string Answer9 = gvCapture.GetRowCellValue(8, "Answer").ToString();
            string Note9 = gvCapture.GetRowCellValue(8, "Note").ToString();
            string Observation9 = gvCapture.GetRowCellValue(8, "Observation").ToString();
            string Answer10 = gvCapture.GetRowCellValue(9, "Answer").ToString();
            string Note10 = gvCapture.GetRowCellValue(9, "Note").ToString();
            string Observation10 = gvCapture.GetRowCellValue(9, "Observation").ToString();

            string Answer11 = gvCapture.GetRowCellValue(10, "Answer").ToString();
            string Note11 = gvCapture.GetRowCellValue(10, "Note").ToString();
            string Observation11 = gvCapture.GetRowCellValue(10, "Observation").ToString();
            string Answer12 = gvCapture.GetRowCellValue(11, "Answer").ToString();
            string Note12 = gvCapture.GetRowCellValue(11, "Note").ToString();
            string Observation12 = gvCapture.GetRowCellValue(11, "Observation").ToString();
            string Answer13 = gvCapture.GetRowCellValue(12, "Answer").ToString();
            string Note13 = gvCapture.GetRowCellValue(12, "Note").ToString();
            string Observation13 = gvCapture.GetRowCellValue(12, "Observation").ToString();
            string Answer14 = gvCapture.GetRowCellValue(13, "Answer").ToString();
            string Note14 = gvCapture.GetRowCellValue(13, "Note").ToString();
            string Observation14 = gvCapture.GetRowCellValue(13, "Observation").ToString();
            string Answer15 = gvCapture.GetRowCellValue(14, "Answer").ToString();
            string Note15 = gvCapture.GetRowCellValue(14, "Note").ToString();
            string Observation15 = gvCapture.GetRowCellValue(14, "Observation").ToString();
            string Answer16 = gvCapture.GetRowCellValue(15, "Answer").ToString();
            string Note16 = gvCapture.GetRowCellValue(15, "Note").ToString();
            string Observation16 = gvCapture.GetRowCellValue(15, "Observation").ToString();
            string Answer17 = gvCapture.GetRowCellValue(16, "Answer").ToString();
            string Note17 = gvCapture.GetRowCellValue(16, "Note").ToString();
            string Observation17 = gvCapture.GetRowCellValue(16, "Observation").ToString();
            string Answer18 = gvCapture.GetRowCellValue(17, "Answer").ToString();
            string Note18 = gvCapture.GetRowCellValue(17, "Note").ToString();
            string Observation18 = gvCapture.GetRowCellValue(17, "Observation").ToString();
            string Answer19 = gvCapture.GetRowCellValue(18, "Answer").ToString();
            string Note19 = gvCapture.GetRowCellValue(18, "Note").ToString();
            string Observation19 = gvCapture.GetRowCellValue(18, "Observation").ToString();
            string Answer20 = gvCapture.GetRowCellValue(19, "Answer").ToString();
            string Note20 = gvCapture.GetRowCellValue(19, "Note").ToString();
            string Observation20 = gvCapture.GetRowCellValue(19, "Observation").ToString();

            string Answer21 = gvCapture.GetRowCellValue(20, "Answer").ToString();
            string Note21 = gvCapture.GetRowCellValue(20, "Note").ToString();
            string Observation21 = gvCapture.GetRowCellValue(20, "Observation").ToString();
            string Answer22 = gvCapture.GetRowCellValue(21, "Answer").ToString();
            string Note22 = gvCapture.GetRowCellValue(21, "Note").ToString();
            string Observation22 = gvCapture.GetRowCellValue(21, "Observation").ToString();
            string Answer23 = gvCapture.GetRowCellValue(22, "Answer").ToString();
            string Note23 = gvCapture.GetRowCellValue(22, "Note").ToString();
            string Observation23 = gvCapture.GetRowCellValue(22, "Observation").ToString();
            string Answer24 = gvCapture.GetRowCellValue(23, "Answer").ToString();
            string Note24 = gvCapture.GetRowCellValue(23, "Note").ToString();
            string Observation24 = gvCapture.GetRowCellValue(23, "Observation").ToString();
            string Answer25 = gvCapture.GetRowCellValue(24, "Answer").ToString();
            string Note25 = gvCapture.GetRowCellValue(24, "Note").ToString();
            string Observation25 = gvCapture.GetRowCellValue(24, "Observation").ToString();
            string Answer26 = gvCapture.GetRowCellValue(25, "Answer").ToString();
            string Note26 = gvCapture.GetRowCellValue(25, "Note").ToString();
            string Observation26 = gvCapture.GetRowCellValue(25, "Observation").ToString();
            string Answer27 = gvCapture.GetRowCellValue(26, "Answer").ToString();
            string Note27 = gvCapture.GetRowCellValue(26, "Note").ToString();
            string Observation27 = gvCapture.GetRowCellValue(26, "Observation").ToString();
            string Answer28 = gvCapture.GetRowCellValue(27, "Answer").ToString();
            string Note28 = gvCapture.GetRowCellValue(27, "Note").ToString();
            string Observation28 = gvCapture.GetRowCellValue(27, "Observation").ToString();
            string Answer29 = gvCapture.GetRowCellValue(28, "Answer").ToString();
            string Note29 = gvCapture.GetRowCellValue(28, "Note").ToString();
            string Observation29 = gvCapture.GetRowCellValue(28, "Observation").ToString();
            string Answer30 = gvCapture.GetRowCellValue(29, "Answer").ToString();
            string Note30 = gvCapture.GetRowCellValue(29, "Note").ToString();
            string Observation30 = gvCapture.GetRowCellValue(29, "Observation").ToString();

            string Answer31 = gvCapture.GetRowCellValue(30, "Answer").ToString();
            string Note31 = gvCapture.GetRowCellValue(30, "Note").ToString();
            string Observation31 = gvCapture.GetRowCellValue(30, "Observation").ToString();
            string Answer32 = gvCapture.GetRowCellValue(31, "Answer").ToString();
            string Note32 = gvCapture.GetRowCellValue(31, "Note").ToString();
            string Observation32 = gvCapture.GetRowCellValue(31, "Observation").ToString();
            string Answer33 = gvCapture.GetRowCellValue(32, "Answer").ToString();
            string Note33 = gvCapture.GetRowCellValue(32, "Note").ToString();
            string Observation33 = gvCapture.GetRowCellValue(32, "Observation").ToString();
            string Answer34 = gvCapture.GetRowCellValue(33, "Answer").ToString();
            string Note34 = gvCapture.GetRowCellValue(33, "Note").ToString();
            string Observation34 = gvCapture.GetRowCellValue(33, "Observation").ToString();
            string Answer35 = gvCapture.GetRowCellValue(34, "Answer").ToString();
            string Note35 = gvCapture.GetRowCellValue(34, "Note").ToString();
            string Observation35 = gvCapture.GetRowCellValue(34, "Observation").ToString();
            string Answer36 = gvCapture.GetRowCellValue(35, "Answer").ToString();
            string Note36 = gvCapture.GetRowCellValue(35, "Note").ToString();
            string Observation36 = gvCapture.GetRowCellValue(35, "Observation").ToString();
            string Answer37 = gvCapture.GetRowCellValue(36, "Answer").ToString();
            string Note37 = gvCapture.GetRowCellValue(36, "Note").ToString();
            string Observation37 = gvCapture.GetRowCellValue(36, "Observation").ToString();
            string Answer38 = gvCapture.GetRowCellValue(37, "Answer").ToString();
            string Note38 = gvCapture.GetRowCellValue(37, "Note").ToString();
            string Observation38 = gvCapture.GetRowCellValue(37, "Observation").ToString();
            string Answer39 = gvCapture.GetRowCellValue(38, "Answer").ToString();
            string Note39 = gvCapture.GetRowCellValue(38, "Note").ToString();
            string Observation39 = gvCapture.GetRowCellValue(38, "Observation").ToString();
            string Answer40 = gvCapture.GetRowCellValue(39, "Answer").ToString();
            string Note40 = gvCapture.GetRowCellValue(39, "Note").ToString();
            string Observation40 = gvCapture.GetRowCellValue(39, "Observation").ToString();

            string Answer41 = gvCapture.GetRowCellValue(40, "Answer").ToString();
            string Note41 = gvCapture.GetRowCellValue(40, "Note").ToString();
            string Observation41 = gvCapture.GetRowCellValue(40, "Observation").ToString();
            string Answer42 = gvCapture.GetRowCellValue(41, "Answer").ToString();
            string Note42 = gvCapture.GetRowCellValue(41, "Note").ToString();
            string Observation42 = gvCapture.GetRowCellValue(41, "Observation").ToString();
            string Answer43 = gvCapture.GetRowCellValue(42, "Answer").ToString();
            string Note43 = gvCapture.GetRowCellValue(42, "Note").ToString();
            string Observation43 = gvCapture.GetRowCellValue(42, "Observation").ToString();
            string Answer44 = gvCapture.GetRowCellValue(43, "Answer").ToString();
            string Note44 = gvCapture.GetRowCellValue(43, "Note").ToString();
            string Observation44 = gvCapture.GetRowCellValue(43, "Observation").ToString();
            string Answer45 = gvCapture.GetRowCellValue(44, "Answer").ToString();
            string Note45 = gvCapture.GetRowCellValue(44, "Note").ToString();
            string Observation45 = gvCapture.GetRowCellValue(44, "Observation").ToString();
            string Answer46 = gvCapture.GetRowCellValue(45, "Answer").ToString();
            string Note46 = gvCapture.GetRowCellValue(45, "Note").ToString();
            string Observation46 = gvCapture.GetRowCellValue(45, "Observation").ToString();
            string Answer47 = gvCapture.GetRowCellValue(46, "Answer").ToString();
            string Note47 = gvCapture.GetRowCellValue(46, "Note").ToString();
            string Observation47 = gvCapture.GetRowCellValue(46, "Observation").ToString();
            string Answer48 = gvCapture.GetRowCellValue(47, "Answer").ToString();
            string Note48 = gvCapture.GetRowCellValue(47, "Note").ToString();
            string Observation48 = gvCapture.GetRowCellValue(47, "Observation").ToString();
            string Answer49 = gvCapture.GetRowCellValue(48, "Answer").ToString();
            string Note49 = gvCapture.GetRowCellValue(48, "Note").ToString();
            string Observation49 = gvCapture.GetRowCellValue(48, "Observation").ToString();
            string Answer50 = gvCapture.GetRowCellValue(49, "Answer").ToString();
            string Note50 = gvCapture.GetRowCellValue(49, "Note").ToString();
            string Observation50 = gvCapture.GetRowCellValue(49, "Observation").ToString();

            string Answer51 = gvCapture.GetRowCellValue(50, "Answer").ToString();
            string Note51 = gvCapture.GetRowCellValue(50, "Note").ToString();
            string Observation51 = gvCapture.GetRowCellValue(50, "Observation").ToString();
            string Answer52 = gvCapture.GetRowCellValue(51, "Answer").ToString();
            string Note52 = gvCapture.GetRowCellValue(51, "Note").ToString();
            string Observation52 = gvCapture.GetRowCellValue(51, "Observation").ToString();
            string Answer53 = gvCapture.GetRowCellValue(52, "Answer").ToString();
            string Note53 = gvCapture.GetRowCellValue(52, "Note").ToString();
            string Observation53 = gvCapture.GetRowCellValue(52, "Observation").ToString();
            string Answer54 = gvCapture.GetRowCellValue(53, "Answer").ToString();
            string Note54 = gvCapture.GetRowCellValue(53, "Note").ToString();
            string Observation54 = gvCapture.GetRowCellValue(53, "Observation").ToString();
            string Answer55 = gvCapture.GetRowCellValue(54, "Answer").ToString();
            string Note55 = gvCapture.GetRowCellValue(54, "Note").ToString();
            string Observation55 = gvCapture.GetRowCellValue(54, "Observation").ToString();
            string Answer56 = gvCapture.GetRowCellValue(55, "Answer").ToString();
            string Note56 = gvCapture.GetRowCellValue(55, "Note").ToString();
            string Observation56 = gvCapture.GetRowCellValue(55, "Observation").ToString();
            string Answer57 = gvCapture.GetRowCellValue(56, "Answer").ToString();
            string Note57 = gvCapture.GetRowCellValue(56, "Note").ToString();
            string Observation57 = gvCapture.GetRowCellValue(56, "Observation").ToString();
            string Answer58 = gvCapture.GetRowCellValue(57, "Answer").ToString();
            string Note58 = gvCapture.GetRowCellValue(57, "Note").ToString();
            string Observation58 = gvCapture.GetRowCellValue(57, "Observation").ToString();
            string Answer59 = gvCapture.GetRowCellValue(58, "Answer").ToString();
            string Note59 = gvCapture.GetRowCellValue(58, "Note").ToString();
            string Observation59 = gvCapture.GetRowCellValue(58, "Observation").ToString();
            string Answer60 = gvCapture.GetRowCellValue(59, "Answer").ToString();
            string Note60 = gvCapture.GetRowCellValue(59, "Note").ToString();
            string Observation60 = gvCapture.GetRowCellValue(59, "Observation").ToString();

            string Answer61 = gvCapture.GetRowCellValue(60, "Answer").ToString();
            string Note61 = gvCapture.GetRowCellValue(60, "Note").ToString();
            string Observation61 = gvCapture.GetRowCellValue(60, "Observation").ToString();
            string Answer62 = gvCapture.GetRowCellValue(61, "Answer").ToString();
            string Note62 = gvCapture.GetRowCellValue(61, "Note").ToString();
            string Observation62 = gvCapture.GetRowCellValue(61, "Observation").ToString();
            string Answer63 = gvCapture.GetRowCellValue(62, "Answer").ToString();
            string Note63 = gvCapture.GetRowCellValue(62, "Note").ToString();
            string Observation63 = gvCapture.GetRowCellValue(62, "Observation").ToString();
            string Answer64 = gvCapture.GetRowCellValue(63, "Answer").ToString();
            string Note64 = gvCapture.GetRowCellValue(63, "Note").ToString();
            string Observation64 = gvCapture.GetRowCellValue(63, "Observation").ToString();

            MWDataManager.clsDataAccess _DataSave = new MWDataManager.clsDataAccess();
            _DataSave.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _DataSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _DataSave.queryReturnType = MWDataManager.ReturnType.DataTable;

            sql = sql + " \r\n \r\n Insert Into tbl_DPT_RockMechInspection (Workplace, CaptYear, CaptWeek, ActWeek, CaptDate, WPASuser, RespSB, RespMO, RiskRating,Cat1Checked, Cat1Note, Cat2Checked, Cat2Note, Cat3Checked, Cat3Note, Cat4Checked, Cat4Note, Cat5Checked, Cat5Note,Cat6Checked, Cat6Note, Cat7Checked,Cat7Note,Cat8Checked,Cat8Note, Cat9Checked, Cat9Note, Cat10Checked,Cat10Note,Cat11Checked, Cat11Note, Cat12Checked, Cat12Note, Cat13Checked, Cat13Note, Cat14Checked, Cat14Note,Cat15Checked, Cat15Note,Cat16Checked, Cat16Note, Cat17Checked, Cat17Note, Cat18Checked, Cat18Note, Cat19Checked, Cat19Note, Cat20Checked, Cat20Note,Cat21Checked, Cat21Note, Cat22Checked, Cat22Note, Cat23Checked, Cat23Note,Cat24Checked, Cat24Note, Cat25Checked, Cat25Note,Cat26Checked, Cat26Note, Cat27Checked, Cat27Note, Cat28Checked, Cat28Note, Cat29Checked, Cat29Note, Cat30Checked, Cat30Note,Cat31Checked, Cat31Note, Cat32Checked, Cat32Note, Cat33Checked, Cat33Note, Cat34Checked, Cat34Note, Cat35Checked, Cat35Note,Cat36Checked, Cat36Note, Cat37Checked, Cat37Note, Cat38Checked, Cat38Note, Cat39Checked, Cat39Note, Cat40Checked, Cat40Note,Cat41Checked, Cat41Note, Cat42Checked, Cat42Note, Cat43Checked, Cat43Note, Cat44Checked, Cat44Note, Cat45Checked, Cat45Note,Cat46Checked, Cat46Note, Cat47Checked, Cat47Note, Cat48Checked, Cat48Note, Cat49Checked, Cat49Note, Cat50Checked, Cat50Note,Cat51Checked, Cat51Note, Cat52Checked, Cat52Note, Cat53Checked, Cat53Note, Cat54Checked, Cat54Note, Cat55Checked, Cat55Note,Cat56Checked, Cat56Note, Cat57Checked, Cat57Note, Cat58Checked, Cat58Note, Cat59Checked, Cat59Note, Cat60Checked, Cat60Note,Cat61Checked, Cat61Note, Cat62Checked, Cat62Note, Cat63Checked, Cat63Note, Picture, GeneralComments, WPStatus, Document1, Document2, Date, Cat64Checked, Cat64Note,Cat1Observation, Cat2Observation, Cat3Observation, Cat4Observation, Cat5Observation,Cat6Observation, Cat7Observation, Cat8Observation, Cat9Observation, Cat10Observation,Cat11Observation, Cat12Observation, Cat13Observation, Cat14Observation, Cat15Observation,Cat16Observation, Cat17Observation, Cat18Observation, Cat19Observation, Cat20Observation,Cat21Observation, Cat22Observation, Cat23Observation, Cat24Observation, Cat25Observation,Cat26Observation, Cat27Observation, Cat28Observation, Cat29Observation, Cat30Observation,Cat31Observation, Cat32Observation, Cat33Observation, Cat34Observation, Cat35Observation,Cat36Observation, Cat37Observation, Cat38Observation, Cat39Observation, Cat40Observation,Cat41Observation, Cat42Observation, Cat43Observation, Cat44Observation, Cat45Observation,Cat46Observation, Cat47Observation, Cat48Observation, Cat49Observation, Cat50Observation,Cat51Observation, Cat52Observation, Cat53Observation, Cat54Observation, Cat55Observation,Cat56Observation, Cat57Observation, Cat58Observation, Cat59Observation, Cat60Observation,Cat61Observation, Cat62Observation, Cat63Observation, Cat64Observation, PegToFace) Values ( \r\n" +
                            " '" + WPLbl.EditValue + "', datepart(YYYY,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "'),  '" + WkLbl2.EditValue + "' , '" + WkLbl2.EditValue + "'  , getdate() , '" + TUserInfo.UserID + "' , \r\n" +
                            " '" + cmbSB.EditValue.ToString() + "' , '" + cmbMO.EditValue.ToString() + "' , '" + RRlabel.EditValue + "', \r\n" +
                            " '" + Answer1 + "', '" + Note1 + "', '" + Answer2 + "', '" + Note2 + "', '" + Answer3 + "', '" + Note3 + "',  \r\n" +
                            " '" + Answer4 + "', '" + Note4 + "', '" + Answer5 + "', '" + Note5 + "', '" + Answer6 + "', '" + Note6 + "',  \r\n" +
                            " '" + Answer7 + "', '" + Note7 + "', '" + Answer8 + "', '" + Note8 + "', '" + Answer9 + "', '" + Note9 + "',  \r\n" +
                            " '" + Answer10 + "', '" + Note10 + "', '" + Answer11 + "', '" + Note11 + "', '" + Answer12 + "', '" + Note12 + "',  \r\n" +
                            " '" + Answer13 + "', '" + Note13 + "', '" + Answer14 + "', '" + Note14 + "', '" + Answer15 + "', '" + Note15 + "',  \r\n" +
                            " '" + Answer16 + "', '" + Note16 + "', '" + Answer17 + "', '" + Note17 + "', '" + Answer18 + "', '" + Note18 + "',  \r\n" +
                            " '" + Answer19 + "', '" + Note19 + "', '" + Answer20 + "', '" + Note20 + "', '" + Answer21 + "', '" + Note21 + "',  \r\n" +
                            " '" + Answer22 + "', '" + Note22 + "', '" + Answer23 + "', '" + Note23 + "', '" + Answer24 + "', '" + Note24 + "',  \r\n" +
                            " '" + Answer25 + "', '" + Note25 + "', '" + Answer26 + "', '" + Note26 + "', '" + Answer27 + "', '" + Note27 + "',  \r\n" +
                            " '" + Answer28 + "', '" + Note28 + "', '" + Answer29 + "', '" + Note29 + "', '" + Answer30 + "', '" + Note30 + "',  \r\n" +
                            " '" + Answer31 + "', '" + Note31 + "', '" + Answer32 + "', '" + Note32 + "', '" + Answer33 + "', '" + Note33 + "',  \r\n" +
                            " '" + Answer34 + "', '" + Note34 + "', '" + Answer35 + "', '" + Note35 + "', '" + Answer36 + "', '" + Note36 + "',  \r\n" +
                            " '" + Answer37 + "', '" + Note37 + "', '" + Answer38 + "', '" + Note38 + "', '" + Answer39 + "', '" + Note39 + "',  \r\n" +
                            " '" + Answer40 + "', '" + Note40 + "', '" + Answer41 + "', '" + Note41 + "', '" + Answer42 + "', '" + Note42 + "',  \r\n" +
                            " '" + Answer43 + "', '" + Note43 + "', '" + Answer44 + "', '" + Note44 + "', '" + Answer45 + "', '" + Note45 + "',  \r\n" +
                            " '" + Answer46 + "', '" + Note46 + "',  \r\n" +
                            " '" + Answer47 + "', '" + Note47 + "', '" + Answer48 + "', '" + Note48 + "', '" + Answer49 + "', '" + Note49 + "',  \r\n" +
                            " '" + Answer50 + "', '" + Note50 + "', '" + Answer51 + "', '" + Note51 + "', '" + Answer52 + "', '" + Note52 + "',  \r\n" +
                            " '" + Answer53 + "', '" + Note53 + "', '" + Answer54 + "', '" + Note54 + "', '" + Answer55 + "', '" + Note55 + "',  \r\n" +
                            " '" + Answer56 + "', '" + Note56 + "', '" + Answer57 + "', '" + Note57 + "', '" + Answer58 + "', '" + Note58 + "',  \r\n" +
                            " '" + Answer59 + "', '" + Note59 + "', '" + Answer60 + "', '" + Note60 + "', '" + Answer61 + "', '" + Note61 + "',  \r\n" +
                            " '" + Answer62 + "', '" + Note62 + "', '" + Answer63 + "', '" + Note63 + "', '" + txtAttachment.Text + "' ,  '" + txtGeneralComments.Text + "',  \r\n" +
                            " '" + WPStatus + "', '" + DocAttachment1 + "','" + DocAttachment2 + "', '" + tbDate.EditValue.ToString() + "', \r\n" +
                            " '" + Answer64 + "', '" + Note64 + "', \r\n" +
                            " '" + Observation1 + "', '" + Observation2 + "', '" + Observation3 + "', '" + Observation4 + "', '" + Observation5 + "', '" + Observation6 + "',  \r\n" +
                            " '" + Observation7 + "', '" + Observation8 + "', '" + Observation9 + "', '" + Observation10 + "', '" + Observation11 + "', '" + Observation12 + "',  \r\n" +
                            " '" + Observation13 + "', '" + Observation14 + "', '" + Observation15 + "', '" + Observation16 + "', '" + Observation17 + "', '" + Observation18 + "',  \r\n" +
                            " '" + Observation19 + "', '" + Observation20 + "', '" + Observation21 + "', '" + Observation22 + "', '" + Observation23 + "', '" + Observation24 + "',  \r\n" +
                            " '" + Observation25 + "', '" + Observation26 + "', '" + Observation27 + "', '" + Observation28 + "', '" + Observation29 + "', '" + Observation30 + "',  \r\n" +
                            " '" + Observation31 + "', '" + Observation32 + "', '" + Observation33 + "', '" + Observation34 + "', '" + Observation35 + "', '" + Observation36 + "',  \r\n" +
                            " '" + Observation37 + "', '" + Observation38 + "', '" + Observation39 + "', '" + Observation40 + "', '" + Observation41 + "', '" + Observation42 + "',  \r\n" +
                            " '" + Observation43 + "', '" + Observation44 + "', '" + Observation45 + "', '" + Observation46 + "', '" + Observation47 + "', '" + Observation48 + "',  \r\n" +
                            " '" + Observation49 + "', '" + Observation50 + "', '" + Observation51 + "', '" + Observation52 + "', '" + Observation53 + "', '" + Observation54 + "',  \r\n" +
                            " '" + Observation55 + "', '" + Observation56 + "', '" + Observation57 + "', '" + Observation58 + "', '" + Observation59 + "', '" + Observation60 + "',  \r\n" +
                            " '" + Observation61 + "', '" + Observation62 + "', '" + Observation63 + "', '" + Observation64 + "', '" + txtPeg.EditValue + "' \r\n" +
                            " ) ";

            _DataSave.SqlStatement = sql;

            MWDataManager.clsDataAccess _Transfer = new MWDataManager.clsDataAccess();
            _Transfer.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _Transfer.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _Transfer.queryReturnType = MWDataManager.ReturnType.DataTable;
            sql = sql + " \r\n \r\n exec sp_Incidents_Transfer ";

            _Transfer.SqlStatement = sql;

            var result = _DataSave.ExecuteInstruction();
            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                LoadReport();
                tabControl.SelectedTabPage = tabReport;

            }
        }

        private void groupControl1_Click(object sender, EventArgs e)
        {
            AddTypeBtn_Click(null, null);
        }

        private void DocPic1_Click(object sender, EventArgs e)
        {

        }

        private void groupControl3_Click(object sender, EventArgs e)
        {
            openFileDialogDOcpic1.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialogDOcpic1.FileName = null;
            resultDocpic1 = openFileDialogDOcpic1.ShowDialog();

            GetFileDoc();
        }

        private void groupControl4_Click(object sender, EventArgs e)
        {
            openFileDialogDOcpic2.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialogDOcpic2.FileName = null;
            resultDocpic2 = openFileDialogDOcpic2.ShowDialog();

            GetFileDoc2();
        }
    }
}
