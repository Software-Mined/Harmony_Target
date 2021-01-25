using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.ViewInfo;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace Mineware.Systems.Production.Departmental.Ventillation
{
    public partial class frmInspection : DevExpress.XtraEditors.XtraForm
    {
        #region Data Fields
        public string _theSystemDBTag;
        public string _UserCurrentInfo;
        public string _totScore, _totWeight, _percentage;

        //Tables
        public DataSet dsGlobal = new DataSet();

        public String dbl_rec_Section;
        public String dbl_rec_Crew;
        public String dbl_rec_ProdMonth;
        public string selectDate;
        public String getWorkplace;
        public string Auth;
        public int Activity;
        public int Days;
        
        DataTable DtSCP = new DataTable();
        DataTable DtSCPFilter = new DataTable();
        StringBuilder sbSqlQuery = new StringBuilder();

        public string ExtractBeforeColon(string TheString)
        {
            if (TheString != string.Empty)
            {
                string BeforeColon;
                int index = TheString.IndexOf(":");
                BeforeColon = TheString.Substring(0, index);
                return BeforeColon;
            }
            else
            {
                return string.Empty;
            }
        }

        //Private Declaration
        //Tables
        private DataTable dtWorkplaceData = new DataTable("dtWorkplaceData");
        private DataTable dtActions = new DataTable("dtActions");
        private DataTable dtData = new DataTable("dtData");
        private DataTable dtAuth = new DataTable("dtAuth");
        private DataTable dtDataEdit = new DataTable("dtDataEdit");
        private DataTable dtDataEdit2 = new DataTable("dtDataEdit2");

        //Questions Tables
        private DataTable dtQuestion = new DataTable("dtQuestion");

        //Strings
        private String frmFirstLoad;
        private String ID = string.Empty;
        private String Workplace = string.Empty;
        private String Description = string.Empty;
        private String Recomendation = string.Empty;
        private String Priority = string.Empty;
        private String TargetDate = string.Empty;
        private String RespPerson = string.Empty;
        private String Overseer = string.Empty;
        private String Likelihood = string.Empty;
        private String Consequence = string.Empty;
        private String NoOfRequest = string.Empty;
        private decimal RiskRating;
        private string docalcparam;
        private String FileName = string.Empty;
        DialogResult result1;

        //string repDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections\Documents";    //Path to store files
        //string repImgDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections";  //Path to store Images
        //string ActionsImgDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections\ActionsImages";
        string RepDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage;

        string repDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage + @"\VentilationInspections\StandardInspections\Documents";    //Path to store files
        string repImgDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage + @"\VentilationInspections\StandardInspections";  //Path to store Images
        string ActionsImgDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage + @"\VentilationInspections\StandardInspections\ActionsImages";
        DialogResult fileResults;

        //Private data fields
        //private String FileName = "";
        private String sourceFile;
        private String destinationFile;
        
        #endregion

        #region Constructor
        public frmInspection()
        {
            InitializeComponent();
            Activity = 0; Days = 45; RiskRating = 0; docalcparam = "Y"; frmFirstLoad = "Y";
        }

        #endregion

        #region Methods / Functions
        //NOTE: Register all tables for sqlconnector to use
        private void tableRegister()
        {
            dsGlobal.Tables.Add(dtWorkplaceData);
            dsGlobal.Tables.Add(dtActions);
            dsGlobal.Tables.Add(dtData);
            dsGlobal.Tables.Add(dtAuth);
            dsGlobal.Tables.Add(dtDataEdit);
            dsGlobal.Tables.Add(dtDataEdit2);
            dsGlobal.Tables.Add(dtQuestion);
        }

        //Use this for Syncromine
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

        private void LoadFeildBook()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);

            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = "select Calendardate, Section \r\n "
                + ", case when FeildBookNum is null then '' else FeildBookNum end as FieldBookNum \r\n "
                + " , case when PageNum is null then '' else PageNum end as PageNum \r\n "
                + " , case when ObserverName is null then '' else ObserverName end as ObserverName \r\n "
                + " , case when Hygienist is null then '' else Hygienist end as Hygienist \r\n "
                + " , case when NoneComp1 is null then '' else NoneComp1 end as NoneComp1 \r\n "
                + " , case when NoneComp2 is null then '' else NoneComp2 end as NoneComp2 \r\n "
                + " , case when NoneComp3 is null then '' else NoneComp3 end as NoneComp3 \r\n "
                + " , case when NoneComp4 is null then '' else NoneComp4 end as NoneComp4 \r\n "
                + " , case when NoneComp5 is null then '' else NoneComp5 end as NoneComp5 \r\n "
                + " , case when NoneComp6 is null then '' else NoneComp6 end as NoneComp6 \r\n "

                + " from tbl_Dept_Inspection_VentCapture_FeildBook where Calendardate = '" + SelectedDate + "' and Section = '" + dbl_rec_MinerSection.Text + "' ";
            _GenSave.ExecuteInstruction();

            DataTable dtFeild = _GenSave.ResultsDataTable;

            txtFeilBook.Text = string.Empty;
            txtPageNum.Text = string.Empty;

            if (dtFeild.Rows.Count > 0)
            {
                txtFeilBook.Text = dtFeild.Rows[0][2].ToString();
                txtPageNum.Text = dtFeild.Rows[0][3].ToString();
                cbxObserverName.SelectedText = dtFeild.Rows[0][4].ToString();
                cbxHygName.SelectedText = dtFeild.Rows[0][5].ToString();

                Pers1.SelectedText = dtFeild.Rows[0][6].ToString();
                Pers2.SelectedText = dtFeild.Rows[0][7].ToString();
                Pers3.SelectedText = dtFeild.Rows[0][8].ToString();
                Pers4.SelectedText = dtFeild.Rows[0][9].ToString();
                Pers5.SelectedText = dtFeild.Rows[0][10].ToString();
                Pers6.SelectedText = dtFeild.Rows[0][11].ToString();
            }
        }

        //Gets Actions
        private void LoadActions()
        {
            MWDataManager.clsDataAccess _dbManAct = new MWDataManager.clsDataAccess();
            _dbManAct.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManAct.SqlStatement = "EXEC [sp_Dept_Ventilation_LoadActions] '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + "', " + Days + ", '" + dbl_rec_MinerSection.Text + "' ";
            _dbManAct.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManAct.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManAct.ExecuteInstruction();

            dtActions = _dbManAct.ResultsDataTable;

            gcActions.DataSource = null;
            gcActions.DataSource = dtActions;
            gcActID.FieldName = "ID";
            gcWorkplace.FieldName = "Workplace";
            gcDescription.FieldName = "Description";
            gcRecommendation.FieldName = "Action";
            gcTargetDate.FieldName = "TargetDate";
            gcPriority.FieldName = "Priority";
            gcImage.FieldName = "CompNotes";
            gcViewImage.FieldName = "Hyperlink";
            gcRespPerson.FieldName = "RespPerson";
            gcOverseer.FieldName = "HOD";
            gcSection.FieldName = "WPType";
            colLikelihood.FieldName = "SFact";
            colConsequence.FieldName = "FFact";
            colNoOfRequest.FieldName = "Proposal";
            gcRR.FieldName = "RR";

            CalcComp();
        }

        //Gets all Data
        private void LoadData()
        {
            
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);

            string sql = string.Empty;
            sql = "exec sp_Dept_Insection_VentStoping_Questions " + Days + ", '" + dbl_rec_MinerSection.Text + "', '" + SelectedDate + "'  \r\n";

            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _sqlConnection.SqlStatement = sql;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;

            gcRockEng.DataSource = null;
            gcRockEng.DataSource = dtReceive;

            colWorkplaceID.FieldName = "Workplaceid";
            colWorkPlace.FieldName = "Description";
            colActivity.FieldName = "Activity";
            colTypeOfWk.FieldName = "TypeOfWorkData";            
            colWBTop.FieldName = "WBTopData";
            colWBbott.FieldName = "WBBotData";
            colDBTop.FieldName = "DBTopData";
            colDBbott.FieldName = "DBBotData";
            colVelTop.FieldName = "VelocityTopData";
            colVelBott.FieldName = "VelocityBotData";
            colKata.FieldName = "KataData";
            colAveStpWid.FieldName = "AveStopeWidthData";
            colRelHum.FieldName = "RelHumData";
            colAUI.FieldName = "AUIData";
            colSCP.FieldName = "SCPData";
            colNoise.FieldName = "NoiseData";
            colHPD.FieldName = "HPDData";            
            colTopContDistFace.FieldName = "ContToFaceTopData";
            colBotContFace.FieldName = "ContToFaceBotData";
            colVentContCond.FieldName = "VentContConditionData";
            colPilHolDistFace.FieldName = "PillarHolingDistToFaceData";
            colPilHolSize.FieldName = "PillarHolingSizeData";
            colPilHolRest.FieldName = "PillarHolingsRestrictedData";
            colNoOpenPill.FieldName = "NoOfOpenPillarHolingData";
            colCGBrattInst.FieldName = "CGBrattInstalledData";
            colCGBrattEffct.FieldName = "CGBrattEffectiveData";
            colVelTrough.FieldName = "VelTroughCGBrattData";
            colTempCGBratt.FieldName = "TempCGBrattData";
            colGasInstrAvl.FieldName = "GasIntrAvailableData";
            colInstrumentNo.FieldName = "IntrumentNumberData";
            colGDICond.FieldName = "ConditionGDIData";
            colGDIKnw.FieldName = "KnowledgeData";
            colDesIncorpVent.FieldName = "DesignEncorVentHolingData";
            colVentLayout.FieldName = "VentLayoutAdheredToData";
            colGeoStructure.FieldName = "GeologicalStructureData";
            colVentAvailable.FieldName = "AvailableData";
            colVentCond.FieldName = "ConditionVentData";
            colVentKnowInstr.FieldName = "KnowledgeVentIntrumentData";
            colKnowEscRoute.FieldName = "KnowledgeEscapeRouteData";
            colGenVentKnow.FieldName = "GenVentKnowledgeData";
            colPnlLeadLeg.FieldName = "PanelLeadLegData";
            colCompliance.FieldName = "ComplianceData";

            //Noise Measurements
            sql = "exec sp_Dept_Insection_VentNoiseMeasurement " + Days + "," + Activity + ", '" + dbl_rec_MinerSection.Text + "', '" + SelectedDate + "'  \r\n";

            MWDataManager.clsDataAccess _NoiseData = new MWDataManager.clsDataAccess();
            _NoiseData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _NoiseData.SqlStatement = sql;
            _NoiseData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _NoiseData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _NoiseData.ExecuteInstruction();

            DataTable dtNoise = new DataTable();
            dtNoise = _NoiseData.ResultsDataTable;

            gcNoiseMeasure.DataSource = null;
            gcNoiseMeasure.DataSource = dtNoise;
            colNoiseWPid.FieldName = "Workplaceid";
            colNoiseWP.FieldName = "Description";
            colNoiseType.FieldName = "NoiseTypeData";
            colSerialNo.FieldName = "SerialNoData";
            colNoiseMeasure.FieldName = "NoiseData";


            ///General Info
            sql = "exec sp_Dept_Insection_Vent_Questions_Stoping_PerSection '" + SelectedDate + "'," +
                " '" + dbl_rec_MinerSection.Text + "'  ";
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _sqlConnection.SqlStatement = sql;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();

            DataTable dtGen = new DataTable();
            dtGen = _sqlConnection.ResultsDataTable;

            gcGeneralInfo.DataSource = null;
            gcGeneralInfo.DataSource = dtGen;
            colGenQuestID.FieldName = "QuestID";
            colGenSubCat.FieldName = "QuestionSubCat";
            colGenQuestion.FieldName = "Question";
            colGenValueType.FieldName = "ValueType";
            colGenNow.FieldName = "Answer";
            colGenPrev.FieldName = "PrevAnswer";
            colGenWP.FieldName = "WP1";

            DateTime DateOFReport = Convert.ToDateTime(tbDpInspecDate.EditValue).Date;
            gvGeneralInfo.SetRowCellValue(22, gvGeneralInfo.Columns["Answer"], Convert.ToDateTime(DateOFReport));



            ///Available Temp
            sql = "exec sp_Dept_Insection_Vent_Questions_Stoping_AvailableTemp '" + SelectedDate + "','" + dbl_rec_MinerSection.Text + "', " + Activity + " ";
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _sqlConnection.SqlStatement = sql;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();

            DataTable dtStation = new DataTable();
            dtStation = _sqlConnection.ResultsDataTable;

            gcAvailbleTemp.DataSource = null;
            gcAvailbleTemp.DataSource = dtStation;
            colIntWB.FieldName = "IntakeWetBulb";
            colIntDB.FieldName = "IntakeDryBulb";
            colReturnWB.FieldName = "ReturnWetBulb";
            colReturnDB.FieldName = "ReturnDryBulb";
            colAvlQtyInt.FieldName = "IntakeQty";
            colAvlQtyReturn.FieldName = "ReturnQty";

            gcIntakeSides.DataSource = null;
            gcIntakeSides.DataSource = dtStation;
            colNoOfSides.FieldName = "NoOfSides";
            colPrevIntake.FieldName = "PreviousIntake";

            ///RefugeBay
            sql = "exec sp_Dept_Insection_Vent_RefugeBay '" + dbl_rec_ProdMonth + "', '" + SelectedDate + "', '" + dbl_rec_MinerSection.Text + "', " + Activity + " ";

            _sqlConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _sqlConnection.SqlStatement = sql;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();

            DataTable dtRef = new DataTable();
            dtRef = _sqlConnection.ResultsDataTable;

            gcRefugeBay.DataSource = null;
            gcRefugeBay.DataSource = dtRef;
            
            colLastWpEmergencyDrill.FieldName = "LastWpEmergencyDrillData";
            colRefDistToWorkplace.FieldName = "RefDistToWorkplaceData";
            colLifeSust.FieldName = "LifeSustData";

            LoadFieldBook();

            LoadActions();
            loadImage();
            loadDocs();
        }

        private void LoadFieldBook()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);

            //Get Users
            MWDataManager.clsDataAccess _Users = new MWDataManager.clsDataAccess();
            _Users.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _Users.SqlStatement = " Select UserID Person, '" + tbSection.EditValue.ToString() + "' Section from tbl_Users";
            _Users.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _Users.queryReturnType = MWDataManager.ReturnType.DataTable;
            _Users.ExecuteInstruction();

            DataTable dtUsers = new DataTable();
            dtUsers = _Users.ResultsDataTable;

            //Observer
            cbxObserverName.Properties.DataSource = dtUsers;
            cbxObserverName.Properties.ForceInitialize();
            cbxObserverName.Properties.DisplayMember = "Person";
            cbxObserverName.Properties.ValueMember = "Person";
            cbxObserverName.Properties.PopulateColumns();
            cbxObserverName.ItemIndex = 0;

            //Hyginist
            cbxHygName.Properties.DataSource = dtUsers;
            cbxHygName.Properties.ForceInitialize();
            cbxHygName.Properties.DisplayMember = "Person";
            cbxHygName.Properties.ValueMember = "Person";
            cbxHygName.Properties.PopulateColumns();
            cbxHygName.ItemIndex = 0;

            //Person
            Pers1.Properties.DataSource = dtUsers;
            Pers1.Properties.ForceInitialize();
            Pers1.Properties.DisplayMember = "Person";
            Pers1.Properties.ValueMember = "Person";
            Pers1.Properties.PopulateColumns();
            Pers1.ItemIndex = 0;

            Pers2.Properties.DataSource = dtUsers;
            Pers2.Properties.DisplayMember = "Person";
            Pers2.Properties.ValueMember = "Person";
            Pers2.Properties.PopulateColumns();
            Pers2.ItemIndex = 0;

            Pers3.Properties.DataSource = dtUsers;
            Pers3.Properties.DisplayMember = "Person";
            Pers3.Properties.ValueMember = "Person";
            Pers3.Properties.PopulateColumns();
            Pers3.ItemIndex = 0;

            Pers4.Properties.DataSource = dtUsers;
            Pers4.Properties.DisplayMember = "Person";
            Pers4.Properties.ValueMember = "Person";
            Pers4.Properties.PopulateColumns();
            Pers4.ItemIndex = 0;

            Pers5.Properties.DataSource = dtUsers;
            Pers5.Properties.DisplayMember = "Person";
            Pers5.Properties.ValueMember = "Person";
            Pers5.Properties.PopulateColumns();
            Pers5.ItemIndex = 0;

            Pers6.Properties.DataSource = dtUsers;
            Pers6.Properties.DisplayMember = "Person";
            Pers6.Properties.ValueMember = "Person";
            Pers6.Properties.PopulateColumns();
            Pers6.ItemIndex = 0;

            //Field Book
            MWDataManager.clsDataAccess _FieldBook = new MWDataManager.clsDataAccess();
            _FieldBook.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _FieldBook.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _FieldBook.queryReturnType = MWDataManager.ReturnType.DataTable;
            _FieldBook.SqlStatement = "select Calendardate, Section \r\n "
                + ", case when FeildBookNum is null then '' else FeildBookNum end as FieldBookNum \r\n "
                + " , case when PageNum is null then '' else PageNum end as PageNum \r\n "
                + " , case when ObserverName is null then '' else ObserverName end as ObserverName \r\n "
                + " , case when Hygienist is null then '' else Hygienist end as Hygienist \r\n "
                + " , case when NoneComp1 is null then '' else NoneComp1 end as NoneComp1 \r\n "
                + " , case when NoneComp2 is null then '' else NoneComp2 end as NoneComp2 \r\n "
                + " , case when NoneComp3 is null then '' else NoneComp3 end as NoneComp3 \r\n "
                + " , case when NoneComp4 is null then '' else NoneComp4 end as NoneComp4 \r\n "
                + " , case when NoneComp5 is null then '' else NoneComp5 end as NoneComp5 \r\n "
                + " , case when NoneComp6 is null then '' else NoneComp6 end as NoneComp6 \r\n "

                + " from tbl_Dept_Inspection_VentCapture_FeildBook where Calendardate = '" + SelectedDate + "' and Section = '" + dbl_rec_MinerSection.Text + "' ";
            _FieldBook.ExecuteInstruction();

            DataTable dtFeild = _FieldBook.ResultsDataTable;
            
            if (dtFeild.Rows.Count > 0)
            {
                //dtUsers = _Users.ResultsDataTable;

                ////Observer
                //cbxObserverName.Properties.DataSource = dtUsers;
                //cbxObserverName.Properties.ForceInitialize();
                //cbxObserverName.Properties.DisplayMember = "Person";
                //cbxObserverName.Properties.ValueMember = "Person";
                //cbxObserverName.Properties.PopulateColumns();
                //cbxObserverName.ItemIndex = 0;

                ////Hyginist
                //cbxHygName.Properties.DataSource = dtUsers;
                //cbxHygName.Properties.DisplayMember = "Person";
                //cbxHygName.Properties.ValueMember = "Person";
                //cbxHygName.Properties.PopulateColumns();
                //cbxHygName.ItemIndex = 0;

                ////Person
                //Pers1.Properties.DataSource = dtUsers;
                //Pers1.Properties.DisplayMember = "Person";
                //Pers1.Properties.ValueMember = "Person";
                //Pers1.Properties.PopulateColumns();
                //Pers1.ItemIndex = 0;

                //Pers2.Properties.DataSource = dtUsers;
                //Pers2.Properties.DisplayMember = "Person";
                //Pers2.Properties.ValueMember = "Person";
                //Pers2.Properties.PopulateColumns();
                //Pers2.ItemIndex = 0;

                //Pers3.Properties.DataSource = dtUsers;
                //Pers3.Properties.DisplayMember = "Person";
                //Pers3.Properties.ValueMember = "Person";
                //Pers3.Properties.PopulateColumns();
                //Pers3.ItemIndex = 0;

                //Pers4.Properties.DataSource = dtUsers;
                //Pers4.Properties.DisplayMember = "Person";
                //Pers4.Properties.ValueMember = "Person";
                //Pers4.Properties.PopulateColumns();
                //Pers4.ItemIndex = 0;

                //Pers5.Properties.DataSource = dtUsers;
                //Pers5.Properties.DisplayMember = "Person";
                //Pers5.Properties.ValueMember = "Person";
                //Pers5.Properties.PopulateColumns();
                //Pers5.ItemIndex = 0;

                //Pers6.Properties.DataSource = dtUsers;
                //Pers6.Properties.DisplayMember = "Person";
                //Pers6.Properties.ValueMember = "Person";
                //Pers6.Properties.PopulateColumns();
                //Pers6.ItemIndex = 0;

                txtFeilBook.Text = dtFeild.Rows[0][2].ToString();
                txtPageNum.Text = dtFeild.Rows[0][3].ToString();
                cbxObserverName.Properties.ForceInitialize();
                cbxObserverName.EditValue = dtFeild.Rows[0][4].ToString();
                cbxHygName.Properties.ForceInitialize();
                cbxHygName.EditValue = dtFeild.Rows[0][5].ToString();

                Pers1.Properties.ForceInitialize();
                Pers1.EditValue = dtFeild.Rows[0][6].ToString();
                Pers2.Properties.ForceInitialize();
                Pers2.EditValue = dtFeild.Rows[0][7].ToString();
                Pers3.Properties.ForceInitialize();
                Pers3.EditValue = dtFeild.Rows[0][8].ToString();
                Pers4.Properties.ForceInitialize();
                Pers4.EditValue = dtFeild.Rows[0][9].ToString();
                Pers5.Properties.ForceInitialize();
                Pers5.EditValue = dtFeild.Rows[0][10].ToString();
                Pers6.Properties.ForceInitialize();
                Pers6.EditValue = dtFeild.Rows[0][11].ToString();
            }
        }

        void SaveVentStoping()
        {
            if (dbl_rec_WPID.Visible == false)
            {
                //MessageBox.Show("Please Select a workplace to activiate", "Enter Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                                 
            MWDataManager.clsDataAccess _dbManVampCheck = new MWDataManager.clsDataAccess();
            _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);

            _dbManVampCheck.SqlStatement = "select CONVERT(varchar(1),activity) activity from  WORKPLACE where Description = '" + dbl_rec_WPID.Text + "' ";
            _dbManVampCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManVampCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManVampCheck.ExecuteInstruction();

            DataTable DataVamp = _dbManVampCheck.ResultsDataTable;


            string act = DataVamp.Rows[0]["activity"].ToString();

            if (act != "1")
            {
                _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
                _dbManVampCheck.SqlStatement = "Insert into workplace " +
                                              " select 'S'+substring(convert(varchar(10),1+b  + 100000),2,5) workplaceid, oreflowid, r.reefid, endtypeid, w.description, '0' act, " +
                                              "'R' reefwaste,  case when  gmsiwpid like '%200' then 'GN' else null end as stpcode, null, null, Line, w.direction, null, null, null, 0, 0, GMSIWPID, null  from WORKPLACE w, reef r, " +
                                              "(select max(convert(int,substring(workplaceid,2,5))) b from workplace_total where Activity = 0) b " +
                                              "where w.reefid = r.shortdesc and w.Description = '" + dbl_rec_WPID.Text + "'  and GMSIWPID  = (select max(GMSIWPID) from WORKPLACE where Description = '" + dbl_rec_WPID.Text + "') ";
                _dbManVampCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVampCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVampCheck.ExecuteInstruction();

            }
            else
            {
                _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
                _dbManVampCheck.SqlStatement = "Insert into workplace " +
                                              "select 'D'+substring(convert(varchar(10),1+b  + 100000),2,5)  workplaceid, oreflowid, reefid, endtypeid, description, 1 act, " +
                                              " reefwaste,  case when  wpid like '%200' then 'GN' else null end as stpcode ,EndWidth , EndHeight, Line, direction, null, null, null, 0, 0, wpid, null  " +
                                              "  from ( " +
                                              " select row_number()over (order by w.description) a, " +
                                              "  oreflowid, r.reefid, e.endtypeid, w.description, w.GMSIWPID wpid, w.Line, w.direction, e.EndHeight, e.EndWidth, e.ReefWaste  " +

                                              "  from WORKPLACE w, reef r, ENDTYPE e " +
                                              " where  w.reefid = r.shortdesc and w.EndTypeID = e.ProcessCode and  w.activity = 1  and w.Description = '" + dbl_rec_WPID.Text + "' and GMSIWPID  = (select max(GMSIWPID) from WORKPLACE where Description = '" + dbl_rec_WPID.Text + "')) a, " +

                                              " (select max(convert(int,substring(workplaceid,2,5))) b from workplace where Activity = 1) b ";
                _dbManVampCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVampCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVampCheck.ExecuteInstruction();
            }

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "saved", Color.CornflowerBlue);
            this.Close();
        }

        void SaveBtn_ItemClick()
        {
            gvRockEng.PostEditor();
            gvRefugeBay.PostEditor();
            gvGeneralInfo.PostEditor();

            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
            sbSqlQuery.Clear();
            sbSqlQuery.AppendLine("delete from tbl_Dept_Inspection_VentStoping_Capture where Section = '" + tbSection.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' ");

            for (int row = 0; row < gvRockEng.RowCount; row++)
            {
                //Declarations
                string Workplaceid = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["Workplaceid"]).ToString();
                string Workplace = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["Description"]).ToString();
                string Activity = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["Activity"]).ToString();
                string TypeOfWork = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["TypeOfWorkData"]).ToString();
                string WBTop = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WBTopData"]).ToString();
                string WBBot = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WBBotData"]).ToString();
                string DBBot = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["DBBotData"]).ToString();
                string DBTop = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["DBTopData"]).ToString();
                string VelocityTop = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["VelocityTopData"]).ToString();
                string VelocityBot = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["VelocityBotData"]).ToString();
                string Kata = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["KataData"]).ToString();
                string AveStopeWidth = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["AveStopeWidthData"]).ToString();
                string RelHum = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["RelHumData"]).ToString();

                string AUI = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["AUIData"]).ToString();
                string SCP = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["SCPData"]).ToString();
                string Noise = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["NoiseData"]).ToString();
                string HDP = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["HPDData"]).ToString();
                string ContToFaceTop = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["ContToFaceTopData"]).ToString();
                string ContToFaceBot = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["ContToFaceBotData"]).ToString();
                string VentContCondition = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["VentContConditionData"]).ToString();
                string PillarHolingDistToFace = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["PillarHolingDistToFaceData"]).ToString();
                string PillarHolingSize = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["PillarHolingSizeData"]).ToString();
                string PillarHolingsRestricted = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["PillarHolingsRestrictedData"]).ToString();
                string NoOfOpenPillarHoling = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["NoOfOpenPillarHolingData"]).ToString();
                string CGBrattInstalled = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["CGBrattInstalledData"]).ToString();
                string CGBrattEffective = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["CGBrattEffectiveData"]).ToString();
                string VelTroughCGBratt = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["VelTroughCGBrattData"]).ToString();
                string TempCGBratt = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["TempCGBrattData"]).ToString();
                string GasIntrAvailable = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["GasIntrAvailableData"]).ToString();
                string IntrumentNumber = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["IntrumentNumberData"]).ToString();
                string ConditionGDI = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["ConditionGDIData"]).ToString();
                string Knowledge = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["KnowledgeData"]).ToString();
                string PanelLeadLeg = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["PanelLeadLegData"]).ToString();
                string DesignEncorVentHoling = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["DesignEncorVentHolingData"]).ToString();
                string VentLayoutAdheredTo = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["VentLayoutAdheredToData"]).ToString();
                string GeologicalStructure = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["GeologicalStructureData"]).ToString();
                string Available = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["AvailableData"]).ToString();
                string ConditionVent = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["ConditionVentData"]).ToString();
                string KnowledgeVentIntrument = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["KnowledgeVentIntrumentData"]).ToString();
                string KnowledgeEscapeRoute = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["KnowledgeEscapeRouteData"]).ToString();
                string GenVentKnowledge = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["GenVentKnowledgeData"]).ToString();
                string ComplianceData = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["ComplianceData"]).ToString();

                sbSqlQuery.AppendLine(" insert into tbl_Dept_Inspection_VentStoping_Capture ");
                sbSqlQuery.AppendLine("values('" + SelectedDate + "', '" + Workplaceid + "', '" + tbSection.EditValue.ToString() + "', " + dbl_rec_ProdMonth + ", '" + TypeOfWork + "', ");
                sbSqlQuery.AppendLine("'" + Activity + "', " + WBTop + ", " + WBBot + ", " + DBTop + ", " + DBBot + ", " + VelocityTop + ", " + VelocityBot + "," + Kata + "," + AveStopeWidth + ",");
                sbSqlQuery.AppendLine(" " + RelHum + ", " + AUI + ", " + SCP + ", " + Noise + ", '" + HDP + "', '" + ContToFaceTop + "', '" + ContToFaceBot + "', '" + VentContCondition + "',");
                sbSqlQuery.AppendLine("'" + PillarHolingDistToFace + "', '" + PillarHolingSize + "', '" + PillarHolingsRestricted + "', " + NoOfOpenPillarHoling + ", '" + CGBrattInstalled + "', '" + CGBrattEffective + "', '" + VelTroughCGBratt + "', '" + TempCGBratt + "',");
                sbSqlQuery.AppendLine("'" + GasIntrAvailable + "', '" + IntrumentNumber + "', '" + ConditionGDI + "', '" + Knowledge + "', '" + PanelLeadLeg + "', '" + DesignEncorVentHoling + "', '" + VentLayoutAdheredTo + "', '" + GeologicalStructure + "',");
                sbSqlQuery.AppendLine("'" + Available + "', '" + ConditionVent + "', '" + KnowledgeVentIntrument + "', '" + KnowledgeEscapeRoute + "', '" + GenVentKnowledge + "', " + ComplianceData + " )");

            }

            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _ActionSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionSave.SqlStatement = sbSqlQuery.ToString();

            var ActionResult = _ActionSave.ExecuteInstruction();


            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Inspection Captured", Color.CornflowerBlue);
            }
        }

        void SaveGeneralInfo()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
            sbSqlQuery.Clear();
            sbSqlQuery.AppendLine(" delete from [tbl_Dept_Inspection_VentCapture_Stoping_PerSection] where section = '" + tbSection.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' ");

            for (int row = 0; row < gvGeneralInfo.RowCount; row++)
            {
                //Declarations
                string QuestID = gvGeneralInfo.GetRowCellValue(row, gvGeneralInfo.Columns["QuestID"]).ToString();
                string Answer = gvGeneralInfo.GetRowCellValue(row, gvGeneralInfo.Columns["Answer"]).ToString();

                sbSqlQuery.AppendLine(" insert into [tbl_Dept_Inspection_VentCapture_Stoping_PerSection] (calendardate,month,Section,Crew,Workplace,QuestID,Answer) ");
                sbSqlQuery.AppendLine(" values('" + SelectedDate + "', " + dbl_rec_ProdMonth + ", '" + tbSection.EditValue.ToString() + "', '" + ExtractBeforeColon(dbl_rec_Crew) + "', '" + getWorkplace + "',");
                sbSqlQuery.AppendLine(" '" + QuestID + "', '" + Answer + "' )");
            }

            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sbSqlQuery.ToString();

            var ActionResult = _GenSave.ExecuteInstruction();

            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "General Inspection Captured", Color.CornflowerBlue);
            }
        }

        void SaveAvailableTemp()
        {
            int Activity = 0;

            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);

            string sql = " delete from tbl_Dept_Inspection_VentCapture_StationTemp \r\n " +
                    " where Activity = " + Activity + " and Calendardate = '" + SelectedDate + "' and Section = '" + tbSection.EditValue.ToString() + "' \r\n";
            for (int row = 0; row < gvAvailableTemp.RowCount; row++)
            {
                string IntakeWetBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["IntakeWetBulb"]).ToString();
                string IntakeDryBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["IntakeDryBulb"]).ToString();
                string ReturnWetBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["ReturnWetBulb"]).ToString();
                string ReturnDryBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["ReturnDryBulb"]).ToString();
                string IntakeQty = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["IntakeQty"]).ToString();
                string ReturnQty = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["ReturnQty"]).ToString();
                string NoOfSides = gvIntakeSides.GetRowCellValue(row, gvIntakeSides.Columns["NoOfSides"]).ToString();
                string PreviousIntake = gvIntakeSides.GetRowCellValue(row, gvIntakeSides.Columns["PreviousIntake"]).ToString();

                sql += " insert into [tbl_Dept_Inspection_VentCapture_StationTemp] \r\n "
                    + " values(" + Activity + ",'" + SelectedDate + "', '" + tbSection.EditValue.ToString() + "', " + IntakeWetBulb + ", " + IntakeDryBulb + ", " + ReturnWetBulb + ", " + ReturnDryBulb + ", \r\n" +
                    " " + IntakeQty + "," + ReturnQty + ", " + NoOfSides + "," + PreviousIntake + ")";
            }

            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sql;

            var ActionResult = _GenSave.ExecuteInstruction();

            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Available Temp. Captured", Color.CornflowerBlue);
            }
        }

        void SaveRefugeBay()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);

            string sql = " delete from tbl_Dept_Vent_RefugeBayCapture where section = '" + tbSection.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' \r\n";

            for (int row = 0; row < gvRefugeBay.RowCount; row++)
            {
                string RefDistToWorkplaceData = gvRefugeBay.GetRowCellValue(row, gvRefugeBay.Columns["RefDistToWorkplaceData"]).ToString();
                string LifeSustData = gvRefugeBay.GetRowCellValue(row, gvRefugeBay.Columns["LifeSustData"]).ToString();
                string LastWpEmergencyDrillData = String.Format("{0:yyyy-MM-dd}", gvRefugeBay.GetRowCellValue(row, gvRefugeBay.Columns["LastWpEmergencyDrillData"]));

                sql += " insert into [tbl_Dept_Vent_RefugeBayCapture] \r\n" +
                    " values(" + dbl_rec_ProdMonth + ", '" + tbSection.EditValue.ToString() + "', '" + SelectedDate + "', " + Activity + ", '" + RefDistToWorkplaceData + "',   \r\n" +
                    " '" + LifeSustData + "', '" + String.Format("{0:yyyy-MM-dd}", LastWpEmergencyDrillData) + "')";
            }

            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sql;

            var ActionResult = _GenSave.ExecuteInstruction();

            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Refuge Bay Captured", Color.CornflowerBlue);
            }
        }

        void SaveNoiseMeasurement()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);

            string sql = " delete from tbl_Dept_Inspection_VentNoiseMeasurement_Capture where Activity = " + Activity + " and section = '" + tbSection.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' \r\n";

            for (int row = 0; row < gvNoiseMeasure.RowCount; row++)
            {
                string NoiseTypeData = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["NoiseTypeData"]).ToString();
                string SerialNoData = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["SerialNoData"]).ToString();
                string NoiseData = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["NoiseData"]).ToString();
                string WorkplaceID = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["Workplaceid"]).ToString();


                sql += " insert into [tbl_Dept_Inspection_VentNoiseMeasurement_Capture] \r\n" +
                    " values( " + Activity + ", " + dbl_rec_ProdMonth + ", '" + SelectedDate + "', '" + tbSection.EditValue.ToString() + "',  '" + WorkplaceID + "',   \r\n" +
                    " '" + NoiseTypeData + "', '" + SerialNoData + "', '" + NoiseData + "')";
            }

            MWDataManager.clsDataAccess _NoiseData = new MWDataManager.clsDataAccess();
            _NoiseData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _NoiseData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _NoiseData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _NoiseData.SqlStatement = sql;

            var ActionResult = _NoiseData.ExecuteInstruction();

            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Noise Measurement Captured", Color.CornflowerBlue);
            }
        }

        void saveFeildBook()
        {

            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);


            string sqlQuery = "delete from tbl_Dept_Inspection_VentCapture_FeildBook where Calendardate = '" + SelectedDate + "' and Section = '" + tbSection.EditValue.ToString() + "' \r\n "
                + " insert into tbl_Dept_Inspection_VentCapture_FeildBook values('" + SelectedDate + "', '" + tbSection.EditValue.ToString() + "', '" + txtFeilBook.Text + "', '" + txtPageNum.Text + "'\r\n"
                + ", '" + cbxObserverName.Text + "', '" + cbxHygName.Text + "', '" + Pers1.Text + "', '" + Pers2.Text + "', '" + Pers3.Text + "' \r\n"
                + ", '" + Pers4.Text + "', '" + Pers5.Text + "', '" + Pers6.Text + "' \r\n)";
            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sqlQuery;
            _GenSave.ExecuteInstruction();
        }

        void LoadMainGrid()
        {
            tbProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString());

            dbl_rec_ProdMonth = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbProdMonth.EditValue.ToString()));

            tableRegister();
            LoadData();
            LoadActions();
            CHeckAuth();

            LoadFeildBook();

            if (dbl_rec_Date.Text == "Y")
            {
                ribbonControl1.Visible = false;

                AddActBtn.Enabled = false;
                EditActBtn.Enabled = false;
                DelActBtn.Enabled = false;

                colTypeOfWk.OptionsColumn.AllowEdit = false;
                colActivity.OptionsColumn.AllowEdit = false;
                colWBbott.OptionsColumn.AllowEdit = false;
                colWBTop.OptionsColumn.AllowEdit = false;
                colDBbott.OptionsColumn.AllowEdit = false;
                colDBTop.OptionsColumn.AllowEdit = false;
                colVelTop.OptionsColumn.AllowEdit = false;
                colKata.OptionsColumn.AllowEdit = false;
                colAveStpWid.OptionsColumn.AllowEdit = false;
                colRelHum.OptionsColumn.AllowEdit = false;
            }
        }

        void CHeckAuth()
        {
            MWDataManager.clsDataAccess _CheckAuth = new MWDataManager.clsDataAccess();
            _CheckAuth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _CheckAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _CheckAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _CheckAuth.SqlStatement = "Select * from tbl_Dept_Inspection_VentAuthorise \r\n" +
                                      "where activity = '0' and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + "' and SectionID = '" + tbSection.EditValue.ToString() + "' ";
            _CheckAuth.ExecuteInstruction();

            if (_CheckAuth.ResultsDataTable.Rows.Count > 0)
            {
                btnAuthWP.Caption = "UnAuthorise";
                Auth = "Authorised";

                for (int col = 0; col < gvRockEng.Columns.Count; col++)
                {
                    gvRockEng.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvGeneralInfo.Columns.Count; col++)
                {
                    gvGeneralInfo.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvPerSectionWinch.Columns.Count; col++)
                {
                    gvPerSectionWinch.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvRefugeBay.Columns.Count; col++)
                {
                    gvRefugeBay.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvAction.Columns.Count; col++)
                {
                    gvAction.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvAvailableTemp.Columns.Count; col++)
                {
                    gvAvailableTemp.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvNoiseMeasure.Columns.Count; col++)
                {
                    gvNoiseMeasure.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvIntakeSides.Columns.Count; col++)
                {
                    gvIntakeSides.Columns[col].OptionsColumn.AllowEdit = false;
                }

                SaveBtn.Enabled = false;
                AddImBtn.Enabled = false;
                btnAddStopDoc.Enabled = false;
                barbtnFireRating.Enabled = false;

                AddActBtn.Enabled = false;
                EditActBtn.Enabled = false;
                DelActBtn.Enabled = false;

                txtFeilBook.Enabled = false;
                cbxHygName.Enabled = false;
                cbxObserverName.Enabled = false;
                txtPageNum.Enabled = false;

                Pers1.Enabled = false;
                Pers2.Enabled = false;
                Pers3.Enabled = false;
                Pers4.Enabled = false;
                Pers5.Enabled = false;
                Pers6.Enabled = false;

            }
            else
            {
                btnAuthWP.Caption = "Authorise";
                Auth = "Not Authorised";

                for (int col = 0; col < gvRockEng.Columns.Count; col++)
                {
                    gvRockEng.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvGeneralInfo.Columns.Count; col++)
                {
                    gvGeneralInfo.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvPerSectionWinch.Columns.Count; col++)
                {
                    gvPerSectionWinch.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvRefugeBay.Columns.Count; col++)
                {
                    gvRefugeBay.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvAction.Columns.Count; col++)
                {
                    gvAction.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvAvailableTemp.Columns.Count; col++)
                {
                    gvAvailableTemp.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvNoiseMeasure.Columns.Count; col++)
                {
                    gvNoiseMeasure.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvIntakeSides.Columns.Count; col++)
                {
                    gvIntakeSides.Columns[col].OptionsColumn.AllowEdit = true;
                }

                SaveBtn.Enabled = true;
                AddImBtn.Enabled = true;
                btnAddStopDoc.Enabled = true;
                barbtnFireRating.Enabled = true;

                AddActBtn.Enabled = true;
                EditActBtn.Enabled = true;
                DelActBtn.Enabled = true;

                txtFeilBook.Enabled = true;
                cbxHygName.Enabled = true;
                cbxObserverName.Enabled = true;
                txtPageNum.Enabled = true;

                Pers1.Enabled = true;
                Pers2.Enabled = true;
                Pers3.Enabled = true;
                Pers4.Enabled = true;
                Pers5.Enabled = true;
                Pers6.Enabled = true;
            }

        }

        void GetFile()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir);
            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            string[] files = System.IO.Directory.GetFiles(repImgDir);



            if (result1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = ofdOpenImageFile.FileName;

                index = ofdOpenImageFile.SafeFileName.IndexOf(".");

                if (!tbSection.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + tbSection.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a Section");
                    return;
                }

                if (tbProdMonth.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                }
                else
                {
                    MessageBox.Show("Please select a Production Month");
                    return;
                }

                Ext = ofdOpenImageFile.SafeFileName.Substring(index);

                destinationFile = repImgDir + "\\" + FileName + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == ofdOpenImageFile.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = repImgDir + "\\" + FileName + Ext;
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
                    System.IO.File.Copy(sourceFile, repImgDir + "\\" + FileName + Ext, true);
                    dir2 = new System.IO.DirectoryInfo(repImgDir);
                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtAttachment.EditValue = destinationFile;
                PicBox.Image = System.Drawing.Image.FromFile(ofdOpenImageFile.FileName);

            }
        }

        public void loadImage()
        {
            txtAttachment.Text = string.Empty;
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(repImgDir);

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                if (item.ToString() == repImgDir + "\\" + tbSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + ext)
                {
                    txtAttachment.Text = item.ToString();
                }
            }


            if (txtAttachment.Text != string.Empty)
            {
                using (FileStream stream = new FileStream(txtAttachment.Text, FileMode.Open, FileAccess.Read))
                {
                    PicBox.Image = System.Drawing.Image.FromStream(stream);
                    stream.Dispose();
                }
            }
            else
            {
                PicBox.Image = null;
            }
        }

        void GetDoc()
        {
            string mianDicrectory = repDir;

            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);
            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(mianDicrectory);

            if (fileResults == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                string SourcefileName = string.Empty;

                int index = 0;

                sourceFile = openFileDialog1.FileName;

                index = openFileDialog1.SafeFileName.IndexOf(".");

                SourcefileName = openFileDialog1.SafeFileName.Substring(0, index);


                if (!tbSection.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + tbSection.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a Section");
                    return;
                }

                if (!tbProdMonth.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                }
                else
                {
                    XtraMessageBox.Show("Please select a Production Month");
                    return;
                }

                Ext = openFileDialog1.SafeFileName.Substring(index);

                destinationFile = mianDicrectory + "\\" + FileName + SourcefileName + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            destinationFile = mianDicrectory + "\\" + FileName + Ext;
                        }
                    }

                    try
                    {
                        //System.IO.File.Copy(sourceFile, destinationFile, true);
                        System.IO.File.Copy(sourceFile, mianDicrectory + "\\" + FileName + SourcefileName + Ext, true);

                        dir2 = new System.IO.DirectoryInfo(mianDicrectory);

                        list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                        DocsLB.Items.Add(SourcefileName + Ext);
                    }
                    catch
                    {

                    }

                }
                else
                {
                    System.IO.File.Copy(sourceFile, mianDicrectory + "\\" + FileName + SourcefileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(mianDicrectory);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                    DocsLB.Items.Add(SourcefileName + Ext);
                }

            }
        }

        public void loadDocs()
        {
            Random r = new Random();

            string mianDicrectory = repDir;

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(mianDicrectory);

            //Do everywhere
            DocsLB.Items.Clear();

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                int indexa = item.LastIndexOf("\\");

                string sourcefilename = item.Substring(indexa + 1, (item.Length - indexa) - 1);
                int indexprodmonth = sourcefilename.IndexOf(String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue));

                string SourcefileCheck = sourcefilename.Substring(0, indexprodmonth + 10);

                int NameLength = sourcefilename.Length - SourcefileCheck.Length;

                string Docsname = sourcefilename.Substring(SourcefileCheck.Length, NameLength);

                if (SourcefileCheck == tbSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue))
                {
                    DocsLB.Items.Add(Docsname.ToString());
                }
            }

        }

        private void CalcComp()
        {
            string ActWP = string.Empty;
            decimal answer = 0;
            for (int row = 0; row < gvRockEng.RowCount; row++)
            {
                string Workplaceid = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["Workplaceid"]).ToString();
                string Workplace = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["Description"]).ToString();
                answer = 0;
                for (int ActRow = 0; ActRow  < gvAction.RowCount; ActRow++)
                {
                    ActWP = gvAction.GetRowCellValue(ActRow, gvAction.Columns["Workplace"]).ToString();

                    if (Workplace == ActWP)
                    {
                        RiskRating = Convert.ToDecimal(gvAction.GetRowCellValue(ActRow, gvAction.Columns["RR"]));
                        answer += RiskRating;
                    }
                }
                gvRockEng.SetRowCellValue(row, gvRockEng.Columns["ComplianceData"], Math.Round(answer, 2));
            }
        }

        //private void CalcAvg()
        //{
        //    int countWP = 1;

        //    if (WP1 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP1"]).ToString()) > 0)
        //            countWP = 1;
        //    }

        //    if (WP2 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP2"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP3 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP3"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP4 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP4"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP5 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP5"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP6 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP6"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP7 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP7"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP8 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP8"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP9 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP9"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP10 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP10"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP11 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP11"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP12 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP13"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP13 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP13"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP14 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP14"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP15 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP15"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP16 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP16"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP17 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP17"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP18 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP18"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP19 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP19"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP20 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP20"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP21 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP21"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP22 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP22"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP23 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP23"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP24 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP24"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    if (WP25 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //    {
        //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP25"]).ToString()) > 0)
        //            countWP = countWP + 1;
        //    }

        //    decimal wbAvg = 0;
        //    decimal DbAvg = 0;
        //    decimal VelAvg = 0;
        //    decimal SWAvg = 0;
        //    decimal SCPAvg = 0;
        //    decimal KataAvg = 0;
        //    decimal HumidityAvg = 0;
        //    decimal WorkPerAvg = 0;
        //    decimal AUIAvg = 0;
        //    decimal PotVelAvg = 0;
        //    decimal StrikeCtrlAvg = 0;

        //    for (int row = 0; row < gvRockEng.RowCount; row++)
        //    {
        //        string QuestID = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["QuestID"]).ToString();

        //        if (WP1 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));




        //        }

        //        if (WP2 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));


        //        }

        //        if (WP3 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));


        //        }

        //        if (WP4 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));


        //        }

        //        if (WP5 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));


        //        }

        //        if (WP6 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));


        //        }

        //        if (WP7 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));


        //        }

        //        if (WP8 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));


        //        }

        //        if (WP9 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));


        //        }

        //        if (WP10 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));


        //        }

        //        if (WP11 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));


        //        }

        //        if (WP12 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));


        //        }

        //        if (WP13 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));


        //        }

        //        if (WP14 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));


        //        }

        //        if (WP15 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));


        //        }

        //        if (WP16 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));


        //        }

        //        if (WP17 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

        //        }

        //        if (WP18 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

        //        }

        //        if (WP19 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));


        //        }

        //        if (WP20 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));


        //        }

        //        if (WP21 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));


        //        }

        //        if (WP22 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

        //        }

        //        if (WP23 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

        //        }

        //        if (WP24 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));


        //        }

        //        if (WP25 != string.Empty)
        //        {
        //            if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

        //            if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
        //                StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));


        //        }
        //    }

        //    wbAvg = wbAvg / countWP;
        //    DbAvg = DbAvg / countWP;
        //    VelAvg = VelAvg / countWP;
        //    SWAvg = SWAvg / countWP;
        //    SCPAvg = SCPAvg / countWP;
        //    KataAvg = KataAvg / countWP;
        //    HumidityAvg = HumidityAvg / countWP;
        //    WorkPerAvg = WorkPerAvg / countWP;
        //    AUIAvg = AUIAvg / countWP;
        //    PotVelAvg = PotVelAvg / countWP;
        //    StrikeCtrlAvg = StrikeCtrlAvg / countWP;

        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(27, gvGeneralInfo.Columns["Answer"], Math.Round(wbAvg, 1));
        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(28, gvGeneralInfo.Columns["Answer"], Math.Round(DbAvg, 1));
        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(29, gvGeneralInfo.Columns["Answer"], Math.Round(VelAvg, 1));
        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(30, gvGeneralInfo.Columns["Answer"], Math.Round(SWAvg, 1));
        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(31, gvGeneralInfo.Columns["Answer"], Math.Round(SCPAvg, 0));
        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(23, gvGeneralInfo.Columns["Answer"], Math.Round(HumidityAvg, 1));
        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(24, gvGeneralInfo.Columns["Answer"], Math.Round(WorkPerAvg, 1));
        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(25, gvGeneralInfo.Columns["Answer"], Math.Round(AUIAvg, 1));

        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(15, gvGeneralInfo.Columns["Answer"], Math.Round(PotVelAvg, 1));

        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(1, gvGeneralInfo.Columns["Answer"], Math.Round(AUIAvg, 1));

        //    docalcparam = "N";
        //    gvGeneralInfo.SetRowCellValue(26, gvGeneralInfo.Columns["Answer"], Math.Round(StrikeCtrlAvg, 1));



        //    decimal scsrPerc = 0;
        //    ///SCSR Compliance
        //    ///
        //    if (gvGeneralInfo.GetRowCellValue(5, gvGeneralInfo.Columns["Answer"]).ToString() != string.Empty && gvGeneralInfo.GetRowCellValue(8, gvGeneralInfo.Columns["Answer"]).ToString() != string.Empty)
        //    {
        //        scsrPerc = (Convert.ToDecimal(gvGeneralInfo.GetRowCellValue(5, gvGeneralInfo.Columns["Answer"]))
        //            / (Convert.ToDecimal(gvGeneralInfo.GetRowCellValue(8, gvGeneralInfo.Columns["Answer"]))) * 100);

        //        gvGeneralInfo.SetRowCellValue(21, gvGeneralInfo.Columns["Answer"], Math.Round(scsrPerc, 2).ToString());
        //    }

        //}

        #endregion

        #region Events

        private void frmStopeVent_Load(object sender, EventArgs e)
        {
            tbCrew.EditValue = dbl_rec_Crew;
            tbSection.EditValue = dbl_rec_MinerSection.Text;

            tbDpInspecDate.EditValue = String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);

            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _ActionSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionSave.SqlStatement = "Select ROW_NUMBER() OVER(ORDER BY col1 ASC) rownum, *   FROM [dbo].[tbl_Dept_Inspection_VentData_Scp]";
            _ActionSave.ExecuteInstruction();

            DtSCP = _ActionSave.ResultsDataTable;

            lblFieldBook.Visible = true;
            lblPageNum.Visible = true;
            lblObsName.Visible = true;

            txtFeilBook.Visible = true;
            txtPageNum.Visible = true;
            cbxObserverName.Visible = true;

            frmFirstLoad = "N";

            LoadMainGrid();
        }

        private Rectangle Transform(System.Drawing.Graphics graphics, int degree, Rectangle r)
        {
            graphics.TranslateTransform(r.Width / 2, r.Height / 2);
            graphics.RotateTransform(degree);
            float cos = (float)Math.Round(Math.Cos(degree * (Math.PI / 180)), 2);
            float sin = (float)Math.Round(Math.Sin(degree * (Math.PI / 180)), 2);
            Rectangle r1 = r;
            r1.X = (int)(r.X * cos + r.Y * sin);

            r1.X = -50;
            r1.Y = (int)(r.X * (-sin) + r.Y * cos);
            r1.Y = r1.Y - 8;
            return r1;
            //throw new NotImplementedException();

        }

        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveBtn_ItemClick();

            SaveGeneralInfo();

            SaveAvailableTemp();

            SaveRefugeBay();

            SaveNoiseMeasurement();

            saveFeildBook();

        }

        private void AddActBtn_Click(object sender, EventArgs e)
        {
            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            ActFrm._theSystemDBTag = ProductionRes.systemDBTag;
            ActFrm._UserCurrentInfo = _UserCurrentInfo;
            ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbProdMonth.EditValue));
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
            ActFrm.lblSection.Text = tbSection.EditValue.ToString();
            ActFrm._Days = Days;
            ActFrm.cbxWorkplace.Visible = true;
            ActFrm.lblWorkplace.Visible = false;
            ActFrm.lblWorkplace.Text = string.Empty;
            ActFrm.Item = "Vent Schedule Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        private void EditActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");
                return;
            }

            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbProdMonth.EditValue.ToString()));
            ActFrm.lblSection.Text = tbSection.EditValue.ToString();

            ActFrm.cbxWorkplace.Visible = false;

            ActFrm._theSystemDBTag = this._theSystemDBTag;
            ActFrm._UserCurrentInfo = this._UserCurrentInfo;
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);

            ActFrm.Item = "Inspection Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";
            ActFrm.lblWorkplace.Text = gvAction.GetRowCellValue(gvAction.FocusedRowHandle, gvAction.Columns["Workplace"]).ToString();
            ActFrm.Item = Description;
            ActFrm.ActionTxt.Text = Recomendation;
            ActFrm.ReqDate.Text = TargetDate;
            ActFrm._RespPerson = RespPerson;
            ActFrm._Overseer = Overseer;
            ActFrm.cbxLikelyhood.SelectedIndex = ActFrm.cbxLikelyhood.Items.IndexOf(Likelihood);
            ActFrm.cbxConsequence.SelectedIndex = ActFrm.cbxLikelyhood.Items.IndexOf(Consequence);
            ActFrm.txtNoOfRequest.Text = NoOfRequest;
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);

            ActFrm.ActID = ID;

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog();


            LoadActions();
        }

        private void gvAction_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            ID = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["ID"]).ToString();
            Workplace = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Workplace"]).ToString();
            Description = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Description"]).ToString();
            Recomendation = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Action"]).ToString();
            TargetDate = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["TargetDate"]).ToString();
            Priority = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Priority"]).ToString();
            FileName = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["CompNotes"]).ToString();
            RespPerson = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["RespPerson"]).ToString();
            Overseer = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["HOD"]).ToString();
            Likelihood = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["SFact"]).ToString();
            Consequence = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["FFact"]).ToString();
            NoOfRequest = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Proposal"]).ToString();
            RiskRating = Convert.ToDecimal(gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["RR"]));
        }

        private void DelActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please click the row you want to delete first.");
                return;
            }


            MWDataManager.clsDataAccess _DeleteAction = new MWDataManager.clsDataAccess();
            _DeleteAction.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _DeleteAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _DeleteAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _DeleteAction.SqlStatement = " Delete from tbl_Shec_Incidents where ID = '" + ID + "'   \r\n" +
                                         " Delete from tbl_Shec_IncidentsVent where ID = '" + ID + "'    \r\n";

            _DeleteAction.ExecuteInstruction();

            string mianDicrectory = ActionsImgDir;

            string Image = mianDicrectory + "\\" + ID + ".png  ";

            if (File.Exists(Image))
            {
                File.Delete(Image);
            }

            LoadActions();
        }

        private void GcRockEng_Click(object sender, EventArgs e)
        {

        }

        private void GvRockEng_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            //Top and Bottom
            GridView view = sender as GridView;
            Rectangle r1 = e.Bounds;

            if (e.Column != null)
            {
                if (//e.Column.Name == "colTypeOfWk" || e.Column.Name == "colWBbott"
                    //    || e.Column.Name == "colWBTop" || e.Column.Name == "colDBbott"
                    //    || e.Column.Name == "colDBTop" || e.Column.Name == "colVelBott"
                    //    || e.Column.Name == "colVelTop" || e.Column.Name == "colKata"
                    //e.Column.Name == "colAveStpWid" //|| e.Column.Name == "colRelHum"
                    //|| e.Column.Name == "colAUI" || e.Column.Name == "colSCP"
                    //|| e.Column.Name == "colNoise" 
                    //|| e.Column.Name == "colActivity"
                    //e.Column.Name == "colHPD" || e.Column.Name == "colTopContDistFace"
                     e.Column.Name == "colVentContCond"// || e.Column.Name == "colPilHolDistFace"
                    /*|| e.Column.Name == "colPilHolSize" */ ||e.Column.Name == "colPilHolRest"
                    //|| e.Column.Name == "colNoOpenPill" || e.Column.Name == "colCGBrattInst"
                    //|| e.Column.Name == "colCGBrattEffct" || e.Column.Name == "colVelTrough"
                    //|| e.Column.Name == "colTempCGBratt" || e.Column.Name == "colGasInstrAvl"
                    || e.Column.Name == "colInstrumentNo" || e.Column.Name == "colGDICond"
                    || e.Column.Name == "colGDIKnw" || e.Column.Name == "colDesIncorpVent"
                    || e.Column.Name == "colVentLayout" || e.Column.Name == "colGeoStructure"
                    || e.Column.Name == "colVentAvailable" || e.Column.Name == "colVentCond"
                    || e.Column.Name == "colVentKnowInstr" || e.Column.Name == "colKnowEscRoute"
                    || e.Column.Name == "colGenVentKnow" || e.Column.Name == "colPnlLeadLeg"
                    //|| e.Column.Name == "colBotContFace") // || e.Column.Name == "gcWP25"
                    //|| e.Column.Name == "gcWP25" || e.Column.Name == "gcWP25"
                    || e.Column.Name == "colCompliance" )
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    //StringFormat sf = new StringFormat();
                    //sf.Trimming = StringTrimming.EllipsisCharacter;
                    //sf.FormatFlags |= StringFormatFlags.NoWrap;
                    //sf.Alignment = StringAlignment.Near;

                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);
                    e.Handled = true;

                }
            }
        }
       
        private void gvRockEng_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            string Activity = gvRockEng.GetRowCellValue(e.RowHandle, "Activity").ToString();

            //Stop Control(Numeric/Text)
            if (e.Column.Name == "colTopContDistFace" || e.Column.Name == "colBotContFace" || e.Column.Name == "colPilHolDistFace" || e.Column.Name == "colPilHolSize"
                    || e.Column.Name == "colNoOpenPill" || e.Column.Name == "colVelTrough" || e.Column.Name == "colTempCGBratt"
                    /*|| e.Column.Name == "colPilHolRest"*/ || e.Column.Name == "NoOfOpenPillarHoling"
                    /*|| e.Column.Name == "colVentContCond"*/)
            {
                if (Activity == "Vamping" || Activity == "Stoping")
                {
                    e.RepositoryItem = riNumericOneDecimal;
                }                
            }

            if (Activity == "Ledging")
            {
                if (e.Column.Name == "colTopContDistFace" || e.Column.Name == "colBotContFace" || e.Column.Name == "colPilHolDistFace" || e.Column.Name == "colPilHolSize"
                || e.Column.Name == "colNoOpenPill" || e.Column.Name == "colVelTrough" || e.Column.Name == "colTempCGBratt"
                || e.Column.Name == "colPilHolRest" || e.Column.Name == "NoOfOpenPillarHoling" || e.Column.Name == "colVentContCond"
                || e.Column.Name == "colCGBrattInst" || e.Column.Name == "colCGBrattEffct")
                {
                    e.RepositoryItem = riReadOnly;
                }
            }

            return;
        }

        private void gvPerSection_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            
        }

        private void gvRockEng_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string Activity = gvRockEng.GetRowCellValue(e.RowHandle, "Activity").ToString();

            GridView view = sender as GridView;
            GridCellInfo cellInfo = e.Cell as GridCellInfo;
            TextEditViewInfo info = cellInfo.ViewInfo as TextEditViewInfo;

            if ( e.Column.Name == "colCompliance")
            {
                e.Appearance.BackColor = Color.Gainsboro;
            }

            //Ledging Read only values
            if (e.Column.Name == "colTopContDistFace" || e.Column.Name == "colBotContFace" || e.Column.Name == "colPilHolDistFace" || e.Column.Name == "colPilHolSize"
                    || e.Column.Name == "colPilHolRest" || e.Column.Name == "colNoOpenPill" || e.Column.Name == "colVelTrough" || e.Column.Name == "colTempCGBratt"
                    || e.Column.Name == "colPilHolRest" || e.Column.Name == "colCGBrattInst" || e.Column.Name == "colCGBrattEffct" || e.Column.Name == "NoOfOpenPillarHoling"
                    || e.Column.Name == "colVentContCond")
            {
                if (Activity == "Ledging")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                    e.Appearance.ForeColor = Color.Gainsboro;
                }
            }

            //Wet Bulb Top & Bott.
            if (e.Column.Name == "colWBbott" || e.Column.Name == "colWBTop")
            {
                if (Convert.ToDecimal(e.CellValue) == Convert.ToDecimal(28.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[0];
                        info.CalcViewInfo();
                    }
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(29.0) && Convert.ToDecimal(e.CellValue) <= Convert.ToDecimal(32.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(33.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                }
            }



            //Dry Bulb Top & Bott.
            if (e.Column.Name == "colDBbott" || e.Column.Name == "colDBTop")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(35.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(31.0) && Convert.ToDecimal(e.CellValue) <= Convert.ToDecimal(34.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(41.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[0];
                        info.CalcViewInfo();
                    }
                }
            }

            //AUI
            if (e.Column.Name == "colAUI")
            {
                if (Convert.ToDecimal(e.CellValue) <= Convert.ToDecimal(60.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[0];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.LightYellow;
                }

                if (Convert.ToDecimal(e.CellValue) > Convert.ToDecimal(60.0) && Convert.ToDecimal(e.CellValue) < Convert.ToDecimal(80.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.OrangeRed;
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(80.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.LightGreen;
                }

            }

            //Noise
            if (e.Column.Name == "colNoise")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(85.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[0];
                        info.CalcViewInfo();
                    }

                    //e.Appearance.BackColor = Color.LightYellow;
                }
            }

            //H.P.D
            if (e.Column.Name == "colHPD")
            {
                if (e.CellValue.ToString() == "No")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }
            }

            //Bottom Cont. to Face
            if (e.Column.Name == "colBotContFace")
            {
                if (Activity == "Vamping")
                {
                    if (Convert.ToDecimal(e.CellValue) > Convert.ToDecimal(12.0))
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[2];
                            info.CalcViewInfo();
                        }
                        //e.Appearance.BackColor = Color.MistyRose;
                    }
                }                
            }

            //Top Cont. to Face
            if (e.Column.Name == "colTopContFace")
            {
                if (Activity == "Vamping")
                {
                    if (Convert.ToDecimal(e.CellValue) > Convert.ToDecimal(12.0))
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[2];
                            info.CalcViewInfo();
                        }
                        //e.Appearance.BackColor = Color.MistyRose;
                    }
                }
            }

            //Pillar Holing Dist. To Face
            if (e.Column.Name == "colPilHolDistFace")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(85.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.LightYellow;
                }
            }

            //Pillar Holing Size
            if (e.Column.Name == "colPilHolSize")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(85.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.LightYellow;
                }
            }

            //Pillar Holing Restricted
            if (e.Column.Name == "colPilHolRest")
            {
                if (e.CellValue.ToString() == "Yes")
                {
                    if (Activity == "Vamping")
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[2];
                            info.CalcViewInfo();
                        }
                        //e.Appearance.BackColor = Color.MistyRose;
                    }

                    if (Activity == "Stoping")
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[0];
                            info.CalcViewInfo();
                        }
                        //e.Appearance.BackColor = Color.LightYellow;
                    }
                }                                
            }

            //No Of OPen Pillars
            if (e.Column.Name == "colNoOpenPill")
            {

                if (Activity == "Vamping")
                {
                    if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(3.0) && Convert.ToDecimal(e.CellValue) <= Convert.ToDecimal(20.0))
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[2];
                            info.CalcViewInfo();
                        }
                        //e.Appearance.BackColor = Color.MistyRose;
                    }

                    if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(21.0))
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[0];
                            info.CalcViewInfo();
                        }
                        //e.Appearance.BackColor = Color.LightYellow;
                    }
                }

                if (Activity == "Stoping")
                {
                    if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(85.0))
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[0];
                            info.CalcViewInfo();
                        }
                        //e.Appearance.BackColor = Color.LightYellow;
                    }
                }
            }

            //Gully Bratt Installed
            if (e.Column.Name == "colCGBrattInst" || e.Column.Name == "colCGBrattEffct"
                || e.Column.Name == "colDesIncorpVent" || e.Column.Name == "colVentLayout"
                || e.Column.Name == "colVentAvailable")
            {
                if (e.CellValue.ToString() == "No")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[0];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.LightYellow;
                }
            }

            //Gas Instr. Available
            if (e.Column.Name == "colGasInstrAvl")
            {
                if (e.CellValue.ToString() == "No")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }
            }

            //Knowledge
            if (e.Column.Name == "colGDIKnw")
            {
                if (e.CellValue.ToString() == "Bad")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.Gainsboro;
                }
            }

            //Does the Panel Lead Leg....
            if (e.Column.Name == "colPnlLeadLeg")
            {
                if (e.CellValue.ToString() == "Yes")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }
            }
            
            //Is there any Geo. structure
            if (e.Column.Name == "colGeoStructure")
            {
                if (e.CellValue.ToString() == "Yes")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[0];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.LightYellow;
                }
            }
            
            //Bad
            if (e.Column.Name == "colVentCond" || e.Column.Name == "colVentKnowInstr"
                || e.Column.Name == "colKnowEscRoute" || e.Column.Name == "colGenVentKnow")
            {
                if (e.CellValue.ToString() == "Bad")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[0];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.LightYellow;
                }
            }

            if (e.Column.Name == "colVentContCond")
            {
                if (e.CellValue.ToString() == "Bad")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                }
            }
        }

        private void gvRockEng_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {   
            string Activity = gvRockEng.GetRowCellValue(e.RowHandle, "Activity").ToString();

            //Face Temp Wet Bulb
            if (e.Column.Name == "colWBbott")
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "WBBotData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "WBBotData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Face Temp Wet Bulb
            if (e.Column.Name == "colWBTop")
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "WBTopData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "WBTopData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Face Temp Dry Bulb
            if (e.Column.Name == "colDBbott")
            {

                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBBotData")) >= Convert.ToDecimal(35.0)
                    && Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBBotData")) <= Convert.ToDecimal(40.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBBotData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Face Temp Dry Bulb
            if (e.Column.Name == "colDBTop")
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBTopData")) >= Convert.ToDecimal(35.0)
                    && Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBBotData")) <= Convert.ToDecimal(40.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBTopData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Top Control distance to Face
            if (e.Column.Name == "colBotContFace")
            {
                if (Activity == "Vamping" || Activity == "Stoping")
                {
                    //decimal d = Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceBotData"));

                    if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceBotData")) > Convert.ToDecimal(12.0))
                    {
                        frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                        ActFrm._theSystemDBTag = _theSystemDBTag;
                        ActFrm._UserCurrentInfo = _UserCurrentInfo;
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                        ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                        ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                        ActFrm.btnCancel.Enabled = false;
                        ActFrm.cbxWorkplace.Visible = false;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceBotData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);
                        LoadActions();
                        docalcparam = "N";
                    }
                }

                //Bad
                if (Activity == "Ledging")
                {
                    if (gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceBotData").ToString() == "Bad")
                    {
                        frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                        ActFrm._theSystemDBTag = _theSystemDBTag;
                        ActFrm._UserCurrentInfo = _UserCurrentInfo;
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                        ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                        ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                        ActFrm.btnCancel.Enabled = false;
                        ActFrm.cbxWorkplace.Visible = false;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceBotData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);
                        LoadActions();
                        docalcparam = "N";
                    }
                }
            }

            //Bott Control distance to Face
            if (e.Column.Name == "colTopContDistFace")
            {
                if (Activity == "Vamping" || Activity == "Stoping")
                {
                    decimal dd = Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceTopData"));

                    if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceTopData")) > Convert.ToDecimal(12.0))
                    {
                        frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                        ActFrm._theSystemDBTag = _theSystemDBTag;
                        ActFrm._UserCurrentInfo = _UserCurrentInfo;
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                        ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                        ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                        ActFrm.btnCancel.Enabled = false;
                        ActFrm.cbxWorkplace.Visible = false;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceTopData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);
                        LoadActions();
                        docalcparam = "N";
                    }
                }

                //Bad
                if (Activity == "Ledging")
                {
                    if (gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceTopData").ToString() == "Bad")
                    {
                        frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                        ActFrm._theSystemDBTag = _theSystemDBTag;
                        ActFrm._UserCurrentInfo = _UserCurrentInfo;
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                        ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                        ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                        ActFrm.btnCancel.Enabled = false;
                        ActFrm.cbxWorkplace.Visible = false;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceTopData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);
                        LoadActions();
                        docalcparam = "N";
                    }
                }
            }

            //Vent Control Condition
            if (e.Column.Name == "colVentContCond")
            {
                if (gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "VentContConditionData").ToString() == "Bad")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "VentContConditionData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //AUI
            if (e.Column.Name == "AUI")
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "AUIData")) >= Convert.ToDecimal(80.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "AUIData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Noise
            if (e.Column.Name == "colNoise")
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "NoiseData")) >= Convert.ToDecimal(85.00))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "NoiseData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //H.P.D
            if (e.Column.Name == "colHPD")
            {
                if (gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "HPDData").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "HPDData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Pillar Holings Restricted
            if (e.Column.Name == "colPilHolRest")
            {
                if (Activity == "Vamping")
                {
                    if (gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "PillarHolingsRestrictedData").ToString() == "Yes")
                    {
                        frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                        ActFrm._theSystemDBTag = _theSystemDBTag;
                        ActFrm._UserCurrentInfo = _UserCurrentInfo;
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                        ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                        ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                        ActFrm.btnCancel.Enabled = false;
                        ActFrm.cbxWorkplace.Visible = false;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "PillarHolingsRestrictedData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);
                        LoadActions();
                        docalcparam = "N";
                    }
                }
            }

            //Pillar Holings Restricted
            if (e.Column.Name == "colNoOpenPill")
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "NoOfOpenPillarHolingData")) >= Convert.ToDecimal(3.0)
                    && Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "NoOfOpenPillarHolingData")) <= Convert.ToDecimal(20.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "NoOfOpenPillarHolingData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Gas Intrument Available
            if (e.Column.Name == "colGasInstrAvl")
            {
                if (gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "GasIntrAvailableData").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "GasIntrAvailableData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Panel Lead or lag > 17m
            if (e.Column.Name == "colPnlLeadLeg")
            {
                if (gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "PanelLeadLegData").ToString() == "Yes")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "PanelLeadLegData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            // calc co
            //if (docalcparam != "N")
            //    docalc();
            //docalcparam = "Y";

            //LoadActions();
        }

        private void gvGeneralInfo_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridCellInfo cellInfo = e.Cell as GridCellInfo;
            TextEditViewInfo info = cellInfo.ViewInfo as TextEditViewInfo;

            if (gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "1" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "2"
                || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "3" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "5"
                || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "6" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "7"
                || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "8" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "9"
                || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "10" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "11"
                || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "13" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "17"
                || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "18" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "19"
                || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "20" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "21"
                || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "22")
            {
                if (e.Column.FieldName == "Answer")
                {
                    if (gvGeneralInfo.GetRowCellValue(e.RowHandle, "Answer").ToString() == "No")
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[1];
                            info.CalcViewInfo();
                        }
                    }

                    if (gvGeneralInfo.GetRowCellValue(e.RowHandle, "Answer").ToString() == "Yes")
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[0];
                            info.CalcViewInfo();
                        }
                    }
                }
            }


            if (gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "4" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "12"
               || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "14" || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "15"
               || gvGeneralInfo.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "16")
            {
                if (e.Column.FieldName == "Answer")
                {
                    if (gvGeneralInfo.GetRowCellValue(e.RowHandle, "Answer").ToString() == "Yes")
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[1];
                            info.CalcViewInfo();
                        }
                    }

                    if (gvGeneralInfo.GetRowCellValue(e.RowHandle, "Answer").ToString() == "No")
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[0];
                            info.CalcViewInfo();
                        }
                    }
                }
            }
            //Prev. Column
            if (e.Column.FieldName == "PrevAnswer")
            {
                e.Appearance.BackColor = Color.WhiteSmoke;
            }

            if (e.Column.Caption == "Now")
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                //e.Appearance.BackColor = Color.WhiteSmoke;
            }
        }

        private void gvStationTemp_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //1 decimal Place

            if (e.Column.FieldName == "WetBulb" || e.Column.FieldName == "DryBulb")
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.BackColor = Color.Transparent;
            }
        }

        private void gvStationTemp_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            ////1 decimal Place

            if (e.Column.FieldName == "WetBulb" || e.Column.FieldName == "DryBulb")
            {
                e.RepositoryItem = repStation1Decimal;
            }
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVentChecklistReport report = new frmVentChecklistReport();
            report.theSystemDBTag = _theSystemDBTag;
            report.UserCurrentInfo = _UserCurrentInfo;
            report.monthDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
            report.section = tbSection.EditValue.ToString();
            report.prodMonth = dbl_rec_ProdMonth;
            report.workPlace = getWorkplace;
            report.crew = tbCrew.EditValue.ToString();
            report.Frmtype = "Stoping";
            report.StartPosition = FormStartPosition.CenterScreen;
            report.PicPath = txtAttachment.Text;
            report._totScore = _totScore;
            report._totWeight = _totWeight;
            report._percentage = _percentage;
            report.Authorise = Auth;

            report.Show();

        }
        
        private void AddImBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ofdOpenImageFile.InitialDirectory = folderBrowserDialog1.SelectedPath;
            ofdOpenImageFile.FileName = null;
            ofdOpenImageFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            result1 = ofdOpenImageFile.ShowDialog();

            GetFile();
        }

        
        private void btnAddStopDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fileResults = openFileDialog1.ShowDialog();
            GetDoc();
        }

        
        private void DocsLB_DoubleClick(object sender, EventArgs e)
        {
            string mianDicrectory = repDir;
            if (DocsLB.SelectedIndex != -1)
            {
                string test = mianDicrectory + "\\" + tbSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + DocsLB.SelectedItem.ToString();

                System.Diagnostics.Process.Start(mianDicrectory + "\\" + tbSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + DocsLB.SelectedItem.ToString());
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void tbDpInspecDate_EditValueChanged(object sender, EventArgs e)
        {
            if (frmFirstLoad == "N")
            {
                frmFirstLoad = "Y";
                LoadData();
                LoadFeildBook();
                frmFirstLoad = "N";
            }
            
        }
        
        private void gvGeneralInfo_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            string Rowhandle = gvGeneralInfo.GetRowCellValue(e.RowHandle, gvGeneralInfo.Columns["Question"]).ToString();

            string Test = string.Empty;
        }

        private void gvGeneralInfo_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //// calc co
            //if (docalcparam != "Y")
            //    return;

            if (frmFirstLoad == "Y")
                return;

            //Answer = No
            if (gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "1" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "2"
            || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "3" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "5"
            || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "6" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "7"
            || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "8" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "9"
            || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "10" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "11"
            || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "13" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "17"
            || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "18" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "19"
            || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "20" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "21"
            || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "22")
            {
                string ss = gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Answer").ToString();
                //string ID = gvAction.GetRowCellValue(gvAction.FocusedRowHandle, "ID").ToString();

                if (gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Answer").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = getWorkplace;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = dbl_rec_WPID.Text;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.Item = gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Answer").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }


            //Answer = Yes
            if (gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "4" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "12"
               || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "14" || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "15"
               || gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "QuestID").ToString() == "16")
            {
                if (gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Answer").ToString() == "Yes")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.lblSection.Text = tbSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = getWorkplace;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = dbl_rec_WPID.Text;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
                    ActFrm.Item = gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Answer").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }
        }

        private void ofdOpenDocFile_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string Docs = openFileDialog1.FileName;

            int indexa = Docs.LastIndexOf("\\");

            string sourcefilename = Docs.Substring(indexa + 1, (Docs.Length - indexa) - 1);

            DocsLB.Items.Add(sourcefilename);
        }

        private void barbtnFireRating_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmInspectionFireRating frmFireRating = new frmInspectionFireRating();
            //frmFireRating._theSystemDBTag = _theSystemDBTag;
            //frmFireRating._UserCurrentInfo = _UserCurrentInfo;
            //frmFireRating._prodMonth = tbProdMonth.EditValue.ToString();
            //frmFireRating.tbDpInspecDate.EditValue = tbDpInspecDate.EditValue;
            //frmFireRating._section = tbSection.EditValue.ToString();
            //frmFireRating._crew = tbCrew.EditValue.ToString();
            //frmFireRating._workPlace = getWorkplace;
            //frmFireRating.ShowDialog(this);

            //_totScore = frmFireRating.lblTotalVal.Text;
            //_totWeight = frmFireRating.lblTotWeight.Text;
            //_percentage = frmFireRating.lblPercentage.Text;

        }

        private void btnAuthWP_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbAuth = new MWDataManager.clsDataAccess();
            _dbAuth.SqlStatement = "delete from tbl_Dept_Inspection_VentAuthorise where Activity = 0 and \r\n "
                + " CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + "' and SectionID = '" + tbSection.EditValue.ToString() + "' \r\n ";

            _dbAuth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAuth.queryReturnType = MWDataManager.ReturnType.DataTable;

            if (btnAuthWP.Caption == "Authorise")
            {
                _dbAuth.SqlStatement += " insert into tbl_Dept_Inspection_VentAuthorise (Activity, CalendarDate, SectionID) \r\n "
                                + " values(0, '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + "', '" + tbSection.EditValue.ToString() + "')";

            }

            var uathResult = _dbAuth.ExecuteInstruction();

            if (uathResult.success)
            {
                CHeckAuth();
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace authorised", Color.CornflowerBlue);
            }
            else
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace not authorised", Color.CornflowerBlue);
            }
        }

        private void gvGeneralInfo_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;

            if (gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, gvGeneralInfo.Columns["ValueType"]).ToString() == string.Empty)
            {
                e.Cancel = true;
            }

            if (gvGeneralInfo.FocusedRowHandle == 22)
            {
                e.Cancel = true;
            }
        }

        private void gvRefugeBay_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Answer")
            {
                string refnum = gvRefugeBay.GetRowCellValue(0, gvRefugeBay.Columns["Answer"]).ToString();
                string refRemakrs = gvRefugeBay.GetRowCellValue(1, gvRefugeBay.Columns["Answer"]).ToString();
                string refDist = gvRefugeBay.GetRowCellValue(2, gvRefugeBay.Columns["Answer"]).ToString();

                gvGeneralInfo.SetRowCellValue(32, gvGeneralInfo.Columns["Answer"], refnum);
                gvGeneralInfo.SetRowCellValue(33, gvGeneralInfo.Columns["Answer"], refRemakrs);
                gvGeneralInfo.SetRowCellValue(34, gvGeneralInfo.Columns["Answer"], refDist);
            }
        }

        private void gvIntakeSides_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            //Top and Bottom
            GridView view = sender as GridView;
            Rectangle r1 = e.Bounds;

            if (e.Column != null)
            {
                if (e.Column.Name == "colNoOfSides" || e.Column.Name == "colPrevIntake")
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    StringFormat sf = new StringFormat();
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    //sf.FormatFlags |= StringFormatFlags.NoWrap;
                    sf.Alignment = StringAlignment.Near;

                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(sf), 270);
                    e.Handled = true;

                }
            }
        }

        private void gvRefugeBay_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                if (e.Column.Name == "colLifeSust" || e.Column.Name == "colRefDistToWorkplace")
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    StringFormat sf = new StringFormat();
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    //sf.FormatFlags |= StringFormatFlags.NoWrap;
                    //sf.Alignment = StringAlignment.Near;

                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(sf), 270);
                    e.Handled = true;

                }
            }
        }

        private void gvAction_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            GridView view = sender as GridView;
            GridCellInfo cellInfo = e.Cell as GridCellInfo;
            TextEditViewInfo info = cellInfo.ViewInfo as TextEditViewInfo;

            //Is there any Geo. structure
            if (e.Column.Name == "gcPriority")
            {
                if (e.CellValue.ToString() == "L")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[0];
                        info.CalcViewInfo();
                    }
                }

                if (e.CellValue.ToString() == "M")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                }

                if (e.CellValue.ToString() == "H")
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                }
            }

        }

        private void gvStationTemp_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "WetBulb")
            {
                string wetbulbStart = gvStationTemp.GetRowCellValue(0, gvStationTemp.Columns["WetBulb"]).ToString();
                string wetbulbEnd = gvStationTemp.GetRowCellValue(1, gvStationTemp.Columns["WetBulb"]).ToString();

                gvGeneralInfo.SetRowCellValue(35, gvGeneralInfo.Columns["Answer"], wetbulbStart);
                gvGeneralInfo.SetRowCellValue(36, gvGeneralInfo.Columns["Answer"], wetbulbEnd);
            }

            if (e.Column.FieldName == "DryBulb")
            {
                string drybulbStart = gvStationTemp.GetRowCellValue(0, gvStationTemp.Columns["DryBulb"]).ToString();
                string drybulbEnd = gvStationTemp.GetRowCellValue(1, gvStationTemp.Columns["DryBulb"]).ToString();

                gvGeneralInfo.SetRowCellValue(37, gvGeneralInfo.Columns["Answer"], drybulbStart);
                gvGeneralInfo.SetRowCellValue(38, gvGeneralInfo.Columns["Answer"], drybulbEnd);
            }
        }

        #endregion
    }
}