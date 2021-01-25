using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Web.UI.WebControls;


namespace Mineware.Systems.Production.Departmental.Ventillation
{
    public partial class frmDevelopmentInspection : DevExpress.XtraEditors.XtraForm
    {
        #region Data Fields 
        //Public Declarations
        //Connections

        public string _theSystemDBTag;
        public string _UserCurrentInfo;
        
        //Tables
        public DataSet dsGlobal = new DataSet();

        //Strings
        public String dbl_rec_Section;
        public String dbl_rec_Crew;
        public String dbl_rec_ProdMonth;
        public string selectDate;
        public string WorkplaceMain;
        public string Auth;
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
        private String formloaded = "N";
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
        private int Activity;
        private int Days;
        private decimal RiskRating;


        //private String WP1 = string.Empty;
        //private String WP2 = string.Empty;
        //private String WP3 = string.Empty;
        //private String WP4 = string.Empty;
        //private String WP5 = string.Empty;

        //private String WP6 = string.Empty;
        //private String WP7 = string.Empty;
        //private String WP8 = string.Empty;
        //private String WP9 = string.Empty;
        //private String WP10 = string.Empty;

        //private String WP11 = string.Empty;
        //private String WP12 = string.Empty;
        //private String WP13 = string.Empty;
        //private String WP14 = string.Empty;
        //private String WP15 = string.Empty;

        //private String WP16 = string.Empty;
        //private String WP17 = string.Empty;
        //private String WP18 = string.Empty;
        //private String WP19 = string.Empty;
        //private String WP20 = string.Empty;

        //private String WP21 = string.Empty;
        //private String WP22 = string.Empty;
        //private String WP23 = string.Empty;
        //private String WP24 = string.Empty;
        //private String WP25 = string.Empty;

        //private String WP1Desc = string.Empty;
        //private String WP2Desc = string.Empty;
        //private String WP3Desc = string.Empty;
        //private String WP4Desc = string.Empty;
        //private String WP5Desc = string.Empty;

        //private String WP6Desc = string.Empty;
        //private String WP7Desc = string.Empty;
        //private String WP8Desc = string.Empty;
        //private String WP9Desc = string.Empty;
        //private String WP10Desc = string.Empty;

        //private String WP11Desc = string.Empty;
        //private String WP12Desc = string.Empty;
        //private String WP13Desc = string.Empty;
        //private String WP14Desc = string.Empty;
        //private String WP15Desc = string.Empty;

        //private String WP16Desc = string.Empty;
        //private String WP17Desc = string.Empty;
        //private String WP18Desc = string.Empty;
        //private String WP19Desc = string.Empty;
        //private String WP20Desc = string.Empty;

        //private String WP21Desc = string.Empty;
        //private String WP22Desc = string.Empty;
        //private String WP23Desc = string.Empty;
        //private String WP24Desc = string.Empty;
        //private String WP25Desc = string.Empty;


        private String FileName = string.Empty;

        DialogResult result1;

        string repDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\DevelopmentInspections\Documents";    //Path to store files
        string repImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\DevelopmentInspections";  //Path to store Images
        string ActionsImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\DevelopmentInspections\ActionImages";

        //string repDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections\Documents";    //Path to store files
        //string repImgDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections";  //Path to store Images
        //string ActionsImgDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections\ActionsImages";
        DialogResult fileResults;

        //Private data fields
        //private String FileName = "";
        private String sourceFile;
        private String destinationFile;
        string docalcparam = "Y";

        #endregion'

        #region Constructor
        public frmDevelopmentInspection()
        {
            InitializeComponent();
            Days = 45; Activity = 1; RiskRating = 0;
        }

        #endregion

        #region Methods/Functions

        //NOTE: Register all tables for sqlconnector to use
        private void tableRegister()
        {
            dsGlobal.Tables.Add(dtWorkplaceData);
            dsGlobal.Tables.Add(dtActions);
            dsGlobal.Tables.Add(dtData);
            dsGlobal.Tables.Add(dtAuth);
            dsGlobal.Tables.Add(dtDataEdit);
            dsGlobal.Tables.Add(dtDataEdit2);
        }

        //Gets Actions
        private void LoadActions()
        {
            MWDataManager.clsDataAccess _dbManAct = new MWDataManager.clsDataAccess();
            _dbManAct.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbManAct.SqlStatement = "EXEC [sp_Dept_Ventilation_LoadActions] '" + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + "', " + Days + ", '" + dbl_rec_MinerSection.Text + "'";
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
            colNoOfRequests.FieldName = "Proposal";
            colRiskRating.FieldName = "RR";

            CalcComp();
        }

        //Gets all Data
        private void LoadData()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
            
            string sql = string.Empty;
            sql = "Exec sp_Dept_Insection_VentDevelopment_Questions " + Days + ",'" + txtSection.EditValue.ToString() + "','" + SelectedDate + "' ";

            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _sqlConnection.SqlStatement = sql;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;

            gcDevInspWorkplaces.DataSource = null;
            gcDevInspWorkplaces.DataSource = dtReceive;

            colWPID.FieldName = "Workplaceid";
            colWorkingEnd.FieldName = "Description";
            colCH4.FieldName = "CH4Data";
            colCO.FieldName = "COData";
            colTypeOfWork.FieldName = "TypeOfWorkData";
            colFanRecirculation.FieldName = "FanRecirculationData";
            colFaceArea.FieldName = "FaceAreaData";
            colQFA.FieldName = "QFAData";
            colKata.FieldName = "KataData";
            colExhaustIntWB.FieldName = "ExhaustIntWBData";
            colExhaustIntDB.FieldName = "ExhaustIntDBData";
            colExhaustDelWB.FieldName = "ExhaustDelWBData";
            colExhaustDelDB.FieldName = "ExhaustDelDBData";
            colForceIntWB.FieldName = "ForceIntWBData";
            colForceIntDB.FieldName = "ForceIntDBData";

            colForceDelWB.FieldName = "ForceDelWBData";
            colForceDelDB.FieldName = "ForceDelDBData";
            colFaceWB.FieldName = "FaceWBData";
            colFaceDB.FieldName = "FaceDBData";
            colRelHum.FieldName = "RelHumData";
            colDistToFaceForce.FieldName = "DistToFaceForceData";
            colDistToFaceExhaust.FieldName = "DistToFaceExhaustData";
            colForceColumn.FieldName = "ForceColumnData";
            colExhaustColumn.FieldName = "ExhaustColumnData";
            colForceExOLap.FieldName = "ForceExOLapData";
            colQtyForceInt.FieldName = "QtyForceIntData";
            colQtyForceDel.FieldName = "QtyForceDelData";
            colQtyExhaustInt.FieldName = "QtyExhaustIntData";
            colQtyExhaustDel.FieldName = "QtyExhaustDelData";
            colTotLeakageForce.FieldName = "TotLeakageForceData";
            colTotLeakageExhaust.FieldName = "TotLeakageExhaustData";
            colSilencerInstOnFan.FieldName = "SilencerInstOnFanData";
            colFanOnStarterBox.FieldName = "FanOnStarterBoxData";
            colStartBoxInThroughVent.FieldName = "StartBoxInThroughVentData";
            colVentColSuspension.FieldName = "VentColSuspensionData";
            colUnventilatedEnd.FieldName = "UnventilatedEndData";
            colNoise.FieldName = "NoiseData";
            colHPD.FieldName = "HPDData";
            colAvailable.FieldName = "AvailableData";
            colInstrumentNo.FieldName = "InstrumentNoData";
            colCondition.FieldName = "ConditionData";
            colKnowledge.FieldName = "KnowledgeData";
            colVentLayoutCater.FieldName = "VentLayoutCaterData";
            colCurrentVentLayout.FieldName = "CurrentVentLayoutData";
            colFanVentLayoutNo.FieldName = "FanVentLayoutNoData";
            colPotentialHoling.FieldName = "PotentialHolingData";
            colMajorGeoStructure.FieldName = "MajorGeoStructureData";
            colCompliance.FieldName = "ComplianceData";


            sql = "Exec sp_Dept_Insection_Vent_Questions_Development_PerSection '" + SelectedDate + "','" + txtSection.EditValue.ToString() + "' \r\n";

            _sqlConnection.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _sqlConnection.SqlStatement = sql;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();

            DataTable dtPerSec = new DataTable();
            dtPerSec = _sqlConnection.ResultsDataTable;

            gcPerSection.DataSource = null;
            gcPerSection.DataSource = dtPerSec;
            colQuestID.FieldName = "QuestID";
            colQuestion.FieldName = "Question";
            colSubCat.FieldName = "QuestionSubCat";
            colWp.FieldName = "Answer";
            colPerValueType.FieldName = "ValueType";

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

            LoadActions();
            loadImage();
            loadDocs();
        }

        void SaveVentStoping()
        {
            if (dbl_rec_WPID.Visible == false)
            {
                return;
            }


            MWDataManager.clsDataAccess _dbManVampCheck = new MWDataManager.clsDataAccess();
            _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManVampCheck.SqlStatement = "select CONVERT(varchar(1),activity) activity from  WORKPLACE where Description = '" + dbl_rec_WPID.Text + "' ";
            _dbManVampCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManVampCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManVampCheck.ExecuteInstruction();

            DataTable DataVamp = _dbManVampCheck.ResultsDataTable;


            string act = DataVamp.Rows[0]["activity"].ToString();

            if (act != "1")
            {
                _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
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
                _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
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

        void SaveDevAllWorkplaces()
        {
            gvDevInspWorkplaces.PostEditor();

            StringBuilder sbSqlQuery = new StringBuilder();

            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            sbSqlQuery.Clear();
            sbSqlQuery.AppendLine("delete from tbl_Dept_Inspection_VentDevelopment_Capture where Section = '" + txtSection.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' ");

            for (int row = 0; row < gvDevInspWorkplaces.RowCount; row++)
            {
                //Declarations
                string NewWPID = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["Workplaceid"]).ToString();
                //string Description = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["Description"]).ToString();
                string CH4Data = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["CH4Data"]).ToString();
                string COData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["COData"]).ToString();
                string TypeOfWorkData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["TypeOfWorkData"]).ToString();
                string FanRecirculationData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["FanRecirculationData"]).ToString();
                string FaceAreaData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["FaceAreaData"]).ToString();
                string QFAData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["QFAData"]).ToString();
                string KataData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["KataData"]).ToString();
                string ExhaustIntWBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ExhaustIntWBData"]).ToString();
                string ExhaustIntDBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ExhaustIntDBData"]).ToString();
                string ExhaustDelWBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ExhaustDelWBData"]).ToString();
                string ExhaustDelDBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ExhaustDelDBData"]).ToString();
                string ForceIntWBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ForceIntWBData"]).ToString();

                string ForceIntDBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ForceIntDBData"]).ToString();
                string ForceDelWBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ForceDelWBData"]).ToString();
                string ForceDelDBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ForceDelDBData"]).ToString();
                string FaceWBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["FaceWBData"]).ToString();
                string FaceDBData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["FaceDBData"]).ToString();
                string RelHumData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["RelHumData"]).ToString();
                string DistToFaceForceData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["DistToFaceForceData"]).ToString();
                string DistToFaceExhaustData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["DistToFaceExhaustData"]).ToString();
                string ForceColumnData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ForceColumnData"]).ToString();
                string ExhaustColumnData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ExhaustColumnData"]).ToString();
                string ForceExOLapData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ForceExOLapData"]).ToString();

                string QtyForceIntData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["QtyForceIntData"]).ToString();
                string QtyForceDelData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["QtyForceDelData"]).ToString();
                string QtyExhaustIntData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["QtyExhaustIntData"]).ToString();
                string QtyExhaustDelData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["QtyExhaustDelData"]).ToString();
                string TotLeakageForceData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["TotLeakageForceData"]).ToString();
                string TotLeakageExhaustData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["TotLeakageExhaustData"]).ToString();
                string SilencerInstOnFanData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["SilencerInstOnFanData"]).ToString();
                string FanOnStarterBoxData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["FanOnStarterBoxData"]).ToString();
                string StartBoxInThroughVentData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["StartBoxInThroughVentData"]).ToString();
                string VentColSuspensionData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["VentColSuspensionData"]).ToString();
                string UnventilatedEndData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["UnventilatedEndData"]).ToString();

                string NoiseData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["NoiseData"]).ToString();
                string HPDData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["HPDData"]).ToString();
                string AvailableData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["AvailableData"]).ToString();
                string InstrumentNoData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["InstrumentNoData"]).ToString();
                string ConditionData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ConditionData"]).ToString();
                string KnowledgeData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["KnowledgeData"]).ToString();
                string VentLayoutCaterData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["VentLayoutCaterData"]).ToString();
                string CurrentVentLayoutData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["CurrentVentLayoutData"]).ToString();
                string FanVentLayoutNoData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["FanVentLayoutNoData"]).ToString();
                string PotentialHolingData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["PotentialHolingData"]).ToString();
                string MajorGeoStructureData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["MajorGeoStructureData"]).ToString();
                string ComplianceData = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["ComplianceData"]).ToString();

                sbSqlQuery.AppendLine(" insert into [tbl_Dept_Inspection_VentDevelopment_Capture] ");
                sbSqlQuery.AppendLine(" values('" + SelectedDate + "', '" + NewWPID + "', '" + txtSection.EditValue.ToString() + "', " + dbl_rec_ProdMonth + " ");
                sbSqlQuery.AppendLine(", " + CH4Data + ", " + COData + ", '" + TypeOfWorkData + "', '" + FanRecirculationData + "', " + FaceAreaData + " ");
                sbSqlQuery.AppendLine(", " + QFAData + ", " + KataData + ", " + ExhaustIntWBData + ", " + ExhaustIntDBData + ", " + ExhaustDelWBData + " ");
                sbSqlQuery.AppendLine(", " + ExhaustDelDBData + ", " + ForceIntWBData + ", " + ForceIntDBData + ", " + ForceDelWBData + ", " + ForceDelDBData + " ");
                sbSqlQuery.AppendLine(", " + FaceWBData + ", " + FaceDBData + ", " + RelHumData + ", " + DistToFaceForceData + ", " + DistToFaceExhaustData + " ");
                sbSqlQuery.AppendLine(", " + ForceColumnData + ", " + ExhaustColumnData + ", '" + ForceExOLapData + "', " + QtyForceIntData + ", " + QtyForceDelData + " ");
                sbSqlQuery.AppendLine(", " + QtyExhaustIntData + ", " + QtyExhaustDelData + ", " + TotLeakageForceData + ", " + TotLeakageExhaustData + ", '" + SilencerInstOnFanData + "' ");
                sbSqlQuery.AppendLine(", '" + FanOnStarterBoxData + "', '" + StartBoxInThroughVentData + "', '" + VentColSuspensionData + "', '" + UnventilatedEndData + "', " + NoiseData + " ");
                sbSqlQuery.AppendLine(", '" + HPDData + "', '" + AvailableData + "', '" + InstrumentNoData + "', '" + ConditionData + "', '" + KnowledgeData + "' ");
                sbSqlQuery.AppendLine(", '" + VentLayoutCaterData + "', '" + CurrentVentLayoutData + "', '" + FanVentLayoutNoData + "', '" + PotentialHolingData + "', '" + MajorGeoStructureData + "', " + ComplianceData + " )");

            }

            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _ActionSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionSave.SqlStatement = sbSqlQuery.ToString();

            var ActionResult = _ActionSave.ExecuteInstruction();
            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Development Inspection Captured", Color.CornflowerBlue);
            }
        }

        void SaveDevPerSection()
        {

            StringBuilder sbSqlQuery = new StringBuilder();

            gvPerSection.PostEditor();

            //string sql = string.Empty;

            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            sbSqlQuery.Clear();
            sbSqlQuery.AppendLine(" delete from [tbl_Dept_Inspection_VentCapture_DevPerSection] where section = '" + txtSection.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' ");

            for (int row = 0; row < gvPerSection.RowCount; row++)
            {
                //Declarations
                string QuestID = gvPerSection.GetRowCellValue(row, gvPerSection.Columns["QuestID"]).ToString();
                string WP1Answer = gvPerSection.GetRowCellValue(row, gvPerSection.Columns["Answer"]).ToString();

                sbSqlQuery.AppendLine(" insert into [tbl_Dept_Inspection_VentCapture_DevPerSection] (calendardate,month,Section,Crew,Workplace,QuestID,Answer) ");
                sbSqlQuery.AppendLine(" values('" + SelectedDate + "', " + dbl_rec_ProdMonth + ", '" + txtSection.EditValue.ToString() + "', '" + ExtractBeforeColon(dbl_rec_Crew) + "', '" + WorkplaceMain + "',");
                sbSqlQuery.AppendLine(" '" + QuestID + "', '" + WP1Answer + "' )");
            }

            MWDataManager.clsDataAccess _PerSecSave = new MWDataManager.clsDataAccess();
            _PerSecSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _PerSecSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PerSecSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PerSecSave.SqlStatement = sbSqlQuery.ToString();

            var ActionResult = _PerSecSave.ExecuteInstruction();
            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Development Inspection PerWorkplace Captured", Color.CornflowerBlue);
            }



        }

        void SaveAvailableTemp()
        {
            int Activity = 1;
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            string sql = " delete from tbl_Dept_Inspection_VentCapture_StationTemp \r\n " +
                    " where Activity = " + Activity + " and Calendardate = '" + SelectedDate + "' and Section = '" + txtSection.EditValue.ToString() + "' \r\n";
            for (int row = 0; row < gvAvailableTemp.RowCount; row++)
            {
                string IntakeWetBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["IntakeWetBulb"]).ToString();
                string IntakeDryBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["IntakeDryBulb"]).ToString();
                string ReturnWetBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["ReturnWetBulb"]).ToString();
                string ReturnDryBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["ReturnDryBulb"]).ToString();
                string IntakeQty = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["IntakeQty"]).ToString();
                string ReturnQty = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["ReturnQty"]).ToString();

                sql += " insert into [tbl_Dept_Inspection_VentCapture_StationTemp] \r\n "
                    + " values(" + Activity + ", '" + SelectedDate + "', '" + txtSection.EditValue.ToString() + "', " + IntakeWetBulb + ", " + IntakeDryBulb + ", " + ReturnWetBulb + ", " + ReturnDryBulb + ", \r\n" +
                    " " + IntakeQty + "," + ReturnQty + ", 0, 0.0)"; //" + NoOfSides + "," + PreviousIntake + ")";
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
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            string sql = " delete from tbl_Dept_Vent_RefugeBayCapture where section = '" + txtSection.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' \r\n";
            int Activity = 1;

            for (int row = 0; row < gvRefugeBay.RowCount; row++)
            {
                //Declarations
                string RefDistToWorkplaceData = gvRefugeBay.GetRowCellValue(row, gvRefugeBay.Columns["RefDistToWorkplaceData"]).ToString();
                string LifeSustData = gvRefugeBay.GetRowCellValue(row, gvRefugeBay.Columns["LifeSustData"]).ToString();
                string LastWpEmergencyDrillData = String.Format("{0:yyyy-MM-dd}", gvRefugeBay.GetRowCellValue(row, gvRefugeBay.Columns["LastWpEmergencyDrillData"]));
                string NoiseTypeData = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["NoiseTypeData"]).ToString();
                string SerialNoData = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["SerialNoData"]).ToString();

                sql += " insert into [tbl_Dept_Vent_RefugeBayCapture] \r\n" +
                    " values(" + dbl_rec_ProdMonth + ", '" + txtSection.EditValue.ToString() + "', '" + SelectedDate + "', " + Activity + ", '" + RefDistToWorkplaceData + "',   \r\n" +
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
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            string sql = " delete from tbl_Dept_Inspection_VentNoiseMeasurement_Capture where Activity = " + Activity + " and section = '" + txtSection.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' \r\n";

            for (int row = 0; row < gvNoiseMeasure.RowCount; row++)
            {
                string NoiseTypeData = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["NoiseTypeData"]).ToString();
                string SerialNoData = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["SerialNoData"]).ToString();
                string NoiseData = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["NoiseData"]).ToString();
                string WorkplaceID = gvNoiseMeasure.GetRowCellValue(row, gvNoiseMeasure.Columns["Workplaceid"]).ToString();


                sql += " insert into [tbl_Dept_Inspection_VentNoiseMeasurement_Capture] \r\n" +
                    " values( " + Activity + ", " + dbl_rec_ProdMonth + ", '" + SelectedDate + "', '" + txtSection.EditValue.ToString() + "',  '" + WorkplaceID + "',   \r\n" +
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

            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);


            string sqlQuery = "delete from tbl_Dept_Inspection_VentCapture_FeildBook where Calendardate = '" + SelectedDate + "' and Section = '" + txtSection.EditValue.ToString() + "' \r\n "
                + " insert into tbl_Dept_Inspection_VentCapture_FeildBook values('" + SelectedDate + "', '" + txtSection.EditValue.ToString() + "', '" + txtFeilBook.Text + "', '" + txtPageNum.Text + "', '" + txtObserverName.Text + "', '" + txtHygName.Text + "', '', '', '', '', '', '' \r\n)";
            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sqlQuery;
            _GenSave.ExecuteInstruction();
        }
        
        private void LoadFeildBook()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
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

                + " from tbl_Dept_Inspection_VentCapture_FeildBook where Calendardate = '" + SelectedDate + "' and Section = '" + txtSection.EditValue.ToString() + "' ";
            _GenSave.ExecuteInstruction();



            DataTable dtFeild = _GenSave.ResultsDataTable;
            //DataSet dsFB = new DataSet();
            //dsFB.Tables.Add(dtFeild);

            txtFeilBook.Text = string.Empty;
            txtObserverName.Text = string.Empty;
            txtPageNum.Text = string.Empty;
            txtHygName.Text = string.Empty;

            if (dtFeild.Rows.Count > 0)
            {
                txtFeilBook.Text = dtFeild.Rows[0][2].ToString();
                txtPageNum.Text = dtFeild.Rows[0][3].ToString();
                txtObserverName.Text = dtFeild.Rows[0][4].ToString();
                txtHygName.Text = dtFeild.Rows[0][5].ToString();
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

                int indexprodmonth = sourcefilename.IndexOf(String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue));

                string SourcefileCheck = sourcefilename.Substring(0, indexprodmonth + 10);

                int NameLength = sourcefilename.Length - SourcefileCheck.Length;

                string Docsname = sourcefilename.Substring(SourcefileCheck.Length, NameLength);


                if (SourcefileCheck == txtSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue))
                {
                    DocsLB.Items.Add(Docsname.ToString());
                }
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

                if (!txtSection.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + txtSection.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                if (txtDpInspecDate.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
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

                if (!txtSection.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + txtSection.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                if (!txtDpInspecDate.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
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

        public void loadImage()
        {
            txtAttachment.Text = string.Empty;
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);


            string[] files = System.IO.Directory.GetFiles(repImgDir);

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                if (item.ToString() == repImgDir + "\\" + txtSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + ext)
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

        void LoadMainGrid()
        {
            txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString());

            dbl_rec_ProdMonth = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtProdMonth.EditValue.ToString()));

            tableRegister();        
            LoadData();
            LoadFeildBook();
            CheckAUth();

            formloaded = "Y";
        }

        private void CheckAUth()
        {
            MWDataManager.clsDataAccess _CheckAuth = new MWDataManager.clsDataAccess();
            _CheckAuth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _CheckAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _CheckAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _CheckAuth.SqlStatement = "Select * from tbl_Dept_Inspection_VentAuthorise \r\n" +
                                      "where activity = '1' and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + "' and SectionID = '" + txtSection.EditValue.ToString() + "' ";
            _CheckAuth.ExecuteInstruction();

            if (_CheckAuth.ResultsDataTable.Rows.Count > 0)
            {
                btnAuthorise.Caption = "UnAuthorise";
                Auth = "Authorised";

                for (int col = 0; col < gvDevInspWorkplaces.Columns.Count; col++)
                {
                    gvDevInspWorkplaces.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvPerSection.Columns.Count; col++)
                {
                    gvPerSection.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvAction.Columns.Count; col++)
                {
                    gvAction.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvNoiseMeasure.Columns.Count; col++)
                {
                    gvNoiseMeasure.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvRefugeBay.Columns.Count; col++)
                {
                    gvRefugeBay.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvAvailableTemp.Columns.Count; col++)
                {
                    gvAvailableTemp.Columns[col].OptionsColumn.AllowEdit = false;
                }

                
                SaveBtn.Enabled = false;
                AddImBtn.Enabled = false;
                btnAddDevDoc.Enabled = false;
                SaveBtn.Enabled = false;
                AddActBtn.Enabled = false;
                EditActBtn.Enabled = false;
                DelActBtn.Enabled = false;

                txtFeilBook.Enabled = false;
                txtPageNum.Enabled = false;
                txtHygName.Enabled = false;
                txtObserverName.Enabled = false;

            }
            else
            {
                btnAuthorise.Caption = "Authorise";
                Auth = "Not Authorised";

                for (int col = 0; col < gvDevInspWorkplaces.Columns.Count; col++)
                {
                    gvDevInspWorkplaces.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvPerSection.Columns.Count; col++)
                {
                    gvPerSection.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvAction.Columns.Count; col++)
                {
                    gvAction.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvNoiseMeasure.Columns.Count; col++)
                {
                    gvNoiseMeasure.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvRefugeBay.Columns.Count; col++)
                {
                    gvRefugeBay.Columns[col].OptionsColumn.AllowEdit = true;
                }

                for (int col = 0; col < gvAvailableTemp.Columns.Count; col++)
                {
                    gvAvailableTemp.Columns[col].OptionsColumn.AllowEdit = true;
                }

                SaveBtn.Enabled = true;
                AddImBtn.Enabled = true;
                btnAddDevDoc.Enabled = true;
                SaveBtn.Enabled = true;

                AddActBtn.Enabled = true;
                EditActBtn.Enabled = true;
                DelActBtn.Enabled = true;

                txtFeilBook.Enabled = true;
                txtPageNum.Enabled = true;
                txtHygName.Enabled = true;
                txtObserverName.Enabled = true;
            }
        }

        void docalc()
        {
            gvPerSection.PostEditor();
            gvDevInspWorkplaces.PostEditor();

            decimal FaceWB = 0;
            decimal QFA = 0;
            decimal Kata = 0;

            decimal FaceDB = 0;
            decimal RelHum = 0;


            if (docalcparam == "Y")
            {
                docalcparam = "N";

                try
                {

                    if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceWBData").ToString() != string.Empty)
                        FaceWB = Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceWBData"));

                    //Kata                                     
                    if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "QFAData").ToString() != string.Empty)
                        QFA = Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "QFAData"));

                    Kata = (Convert.ToDecimal(36.5) - FaceWB) * (Convert.ToDecimal(0.7) + Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(QFA))));
                    gvDevInspWorkplaces.SetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "KataData", Math.Round(Kata, 0));

                    if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceDBData").ToString() != string.Empty)
                        FaceDB = Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceDBData"));

                    if (FaceDB > 0)
                    {
                        RelHum = (FaceWB / FaceDB) * Convert.ToDecimal(100);
                        gvDevInspWorkplaces.SetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "RelHumData", Math.Round(RelHum, 1));
                    }

                }
                catch { }


            }
            //CalcComp();
            docalcparam = "Y";

        }

        private void CalcComp()
        {
            string ActWP = string.Empty;
            decimal answer = 0;
            for (int row = 0; row < gvDevInspWorkplaces.RowCount; row++)
            {
                string Workplaceid = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["Workplaceid"]).ToString();
                string Workplace = gvDevInspWorkplaces.GetRowCellValue(row, gvDevInspWorkplaces.Columns["Description"]).ToString();
                answer = 0;
                for (int ActRow = 0; ActRow < gvAction.RowCount; ActRow++)
                {
                    ActWP = gvAction.GetRowCellValue(ActRow, gvAction.Columns["Workplace"]).ToString();

                    if (Workplace == ActWP)
                    {
                        if(!string.IsNullOrEmpty(gvAction.GetRowCellValue(ActRow, gvAction.Columns["RR"]).ToString()))
                            RiskRating = Convert.ToDecimal(gvAction.GetRowCellValue(ActRow, gvAction.Columns["RR"]));   
                        answer += RiskRating;
                    }
                }
                gvDevInspWorkplaces.SetRowCellValue(row, gvDevInspWorkplaces.Columns["ComplianceData"], Math.Round(answer, 2));
            }
        }

        private void frmDevelopmentInspection_Load(object sender, EventArgs e)
        {
            txtDpInspecDate.EditValue = String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);

            txtCrew.EditValue = dbl_rec_Crew;
            txtSection.EditValue = dbl_rec_MinerSection.Text;
            selectDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            LoadMainGrid();

            MWDataManager.clsDataAccess _CheckAUthSecurity = new MWDataManager.clsDataAccess();
            _CheckAUthSecurity.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _CheckAUthSecurity.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _CheckAUthSecurity.queryReturnType = MWDataManager.ReturnType.DataTable;
            _CheckAUthSecurity.SqlStatement = "  Select ProfileID from [Syncromine_New].[dbo].tblUsers users  \r\n" +
                                       ", [Syncromine_New].[dbo].[tblUserProfileLink] link \r\n" +
                                       "  where users.USERID = link.UserID and users.USERID = '" + TUserInfo.UserID + "'";
            _CheckAUthSecurity.ExecuteInstruction();

            btnAuthorise.Enabled = false;

            DataTable dtRights = _CheckAUthSecurity.ResultsDataTable;

            foreach (DataRow dr in dtRights.Rows)
            {
                if (dr["ProfileID"].ToString() == "SYSADMIN"
                   || dr["ProfileID"].ToString() == "MRMVentAut")
                {
                    btnAuthorise.Enabled = true;
                }
            }


            lblFieldBook.Visible = true;
            lblPageNum.Visible = true;
            lblObsName.Visible = true;


            txtFeilBook.Visible = true;
            txtPageNum.Visible = true;
            txtObserverName.Visible = true;
        }

        #endregion

        #region Events
        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveDevAllWorkplaces();
            SaveDevPerSection();
            SaveAvailableTemp();
            SaveRefugeBay();
            SaveNoiseMeasurement();
            saveFeildBook();
        }

        private void btnAuthorise_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbAuth = new MWDataManager.clsDataAccess();
            _dbAuth.SqlStatement = "delete from tbl_Dept_Inspection_VentAuthorise where Activity = 1 and \r\n "
                + " CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + "' and SectionID = '" + txtSection.EditValue.ToString() + "' \r\n ";

            _dbAuth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAuth.queryReturnType = MWDataManager.ReturnType.DataTable;

            if (btnAuthorise.Caption == "Authorise")
            {
                _dbAuth.SqlStatement += " insert into tbl_Dept_Inspection_VentAuthorise (Activity, CalendarDate, SectionID) \r\n "
                                + " values(1, '" + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + "', '" + txtSection.EditValue.ToString() + "')";
            }


            var uathResult = _dbAuth.ExecuteInstruction();

            if (uathResult.success)
            {
                CheckAUth();
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace authorised", Color.CornflowerBlue);
            }
            else
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace not authorised", Color.CornflowerBlue);
            }
        }

        private void EditActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");

                return;
            }

            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtProdMonth.EditValue.ToString()));
            ActFrm.lblSection.Text = txtSection.EditValue.ToString();
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            ActFrm.cbxWorkplace.Visible = false;
            ActFrm.lblWorkplace.Visible = true;

            ActFrm._theSystemDBTag = this._theSystemDBTag;
            ActFrm._UserCurrentInfo = this._UserCurrentInfo;

            ActFrm.Item = "Inspection Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";
            ActFrm.lblWorkplace.Text = Workplace;
            ActFrm.Item = Description;
            ActFrm.ActionTxt.Text = Recomendation;
            ActFrm.ReqDate.Text = TargetDate;
            ActFrm.txtNoOfRequest.Text = NoOfRequest;
            ActFrm.cbxLikelyhood.SelectedIndex = ActFrm.cbxLikelyhood.Items.IndexOf(Likelihood);
            ActFrm.cbxConsequence.SelectedIndex = ActFrm.cbxLikelyhood.Items.IndexOf(Consequence);
            ActFrm.RespPersonCmb.EditValue = RespPerson;
            ActFrm.OverseerCmb.EditValue = Overseer;

            ActFrm.ActID = ID;

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();

        }

        private void AddActBtn_Click(object sender, EventArgs e)
        {
            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
            ActFrm.lblSection.Text = txtSection.EditValue.ToString();
            //ActFrm.cbxWorkplace.EditValue = WorkplaceMain;
            //ActFrm.cbxWorkplace.Properties.Items.Add(WP1Desc);

            ActFrm.lblWorkplace.Text = WorkplaceMain;

            ActFrm.cbxWorkplace.Visible = false;
            ActFrm.lblWorkplace.Visible = true;

            ActFrm._theSystemDBTag = _theSystemDBTag;
            ActFrm._UserCurrentInfo = _UserCurrentInfo;

            ActFrm.Item = "Vent Schedule Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVentChecklistReport report = new frmVentChecklistReport();
            report.theSystemDBTag = _theSystemDBTag;
            report.UserCurrentInfo = _UserCurrentInfo;
            report.monthDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
            report.section = txtSection.EditValue.ToString();
            report.Frmtype = "Development";
            report.StartPosition = FormStartPosition.CenterScreen;
            report.PicPath = txtAttachment.Text;
            report.prodMonth = dbl_rec_ProdMonth;
            report.Authorise = Auth;
            report.Show();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void AddImBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ofdOpenImageFile.InitialDirectory = folderBrowserDialog1.SelectedPath;
            ofdOpenImageFile.FileName = null;
            ofdOpenImageFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            result1 = ofdOpenImageFile.ShowDialog();

            GetFile();
        }

        private void btnAddDevDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fileResults = openFileDialog1.ShowDialog();
            GetDoc();
        }

        private void DelActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please click the row you want to delete first.");
                return;
            }

            MWDataManager.clsDataAccess _DeleteAction = new MWDataManager.clsDataAccess();
            _DeleteAction.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
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

        private void gvAction_RowCellClick(object sender, RowCellClickEventArgs e)
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
        }

        private void DocsLB_DoubleClick(object sender, EventArgs e)
        {
            string mianDicrectory = repDir;
            if (DocsLB.SelectedIndex != -1)
            {
                string test = mianDicrectory + "\\" + txtSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + DocsLB.SelectedItem.ToString();

                System.Diagnostics.Process.Start(mianDicrectory + "\\" + txtSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + DocsLB.SelectedItem.ToString());
            }
        }

        private void txtDpInspecDate_EditValueChanged(object sender, EventArgs e)
        {
            if (formloaded == "Y")
            {
                LoadData();
                LoadFeildBook();

            }
        }

        private void gvDevInspWorkplaces_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                if (//e.Column.Name == "colCH4" || e.Column.Name == "colCO"
                    //|| e.Column.Name == "colTypeOfWork"|| e.Column.Name == "colFaceArea" 
                     e.Column.Name == "colFanRecirculation"
                    //|| e.Column.Name == "colQFA"
                    //|| e.Column.Name == "colKata" || e.Column.Name == "colExhaustIntWB"

                    //|| e.Column.Name == "colExhaustDelWB" || e.Column.Name == "colQtyExhaustDel"
                    //|| e.Column.Name == "colExhaustIntDB" || e.Column.Name == "colExhaustDelDB"
                    //|| e.Column.Name == "colForceIntWB" || e.Column.Name == "colForceIntDB"
                    //|| e.Column.Name == "colForceDelWB" || e.Column.Name == "colForceDelDB"
                    //|| e.Column.Name == "colFaceWB" || e.Column.Name == "colFaceDB"
                    //|| e.Column.Name == "colRelHum"  
                    //|| e.Column.Name == "colDistToFaceForce" || e.Column.Name == "colDistToFaceExhaust"
                    || e.Column.Name == "colCondition"
                    || e.Column.Name == "colKnowledge" /*|| e.Column.Name == "colForceColumn"*/
                    //|| e.Column.Name == "colExhaustColumn" || e.Column.Name == "colForceExOLap"
                    //|| e.Column.Name == "colQtyForceInt" || e.Column.Name == "colQtyForceDel"

                    //|| e.Column.Name == "colQtyExhaustInt" || e.Column.Name == "colQtyExhaustDel"
                    //|| e.Column.Name == "colTotLeakageForce" || e.Column.Name == "colTotLeakageExhaust"
                    || e.Column.Name == "colSilencerInstOnFan" /*|| e.Column.Name == "colFanOnStarterBox"*/
                    || e.Column.Name == "colStartBoxInThroughVent" || e.Column.Name == "colVentColSuspension"
                    || e.Column.Name == "colUnventilatedEnd" /*|| e.Column.Name == "colNoise"*/
                    /*|| e.Column.Name == "colHPD"*/ || e.Column.Name == "colAvailable"
                    || e.Column.Name == "colInstrumentNo" || e.Column.Name == "colCondition"
                    || e.Column.Name == "colKnowledge" || e.Column.Name == "colVentLayoutCater"
                    || e.Column.Name == "colCurrentVentLayout" || e.Column.Name == "colFanVentLayoutNo"
                    || e.Column.Name == "colPotentialHoling" || e.Column.Name == "colMajorGeoStructure"
                    || e.Column.Name == "colCompliance")
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    StringFormat sf = new StringFormat();
                    sf.Trimming = StringTrimming.EllipsisCharacter;

                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(sf), 270);
                    e.Handled = true;

                }
            }

        }

        private void gvDevInspWorkplaces_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridCellInfo cellInfo = e.Cell as GridCellInfo;
            TextEditViewInfo info = cellInfo.ViewInfo as TextEditViewInfo;

            if (e.Column.Name == "colKata" || e.Column.Name == "colRelHum" || e.Column.Name == "colCompliance")
            {
                e.Appearance.BackColor = Color.Gainsboro;
            }

            //Wet Bulbs and Dry Bulbs
            if (e.Column.Name == "colExhaustIntWB" || e.Column.Name == "colExhaustIntDB"
                || e.Column.Name == "colExhaustDelWB" || e.Column.Name == "colForceIntWB" || e.Column.Name == "colForceDelWB")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(32.0) && Convert.ToDecimal(e.CellValue) <= Convert.ToDecimal(36.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(37.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }
            }

            if ( e.Column.Name == "colForceIntDB" || e.Column.Name == "colForceDelDB"
                || e.Column.Name == "colFaceDB")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(29.0) && Convert.ToDecimal(e.CellValue) <= Convert.ToDecimal(32.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(33.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }
            }

            if (e.Column.Name == "colExhaustDelDB" || e.Column.Name == "colFaceWB")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(32.0) && Convert.ToDecimal(e.CellValue) <= Convert.ToDecimal(36.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[1];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(37.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }
            }

            //Distance To Face "Force"
            if (e.Column.Name == "colDistToFaceForce")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(31.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }
            }

            //Distance To Face "Exhaust"
            if (e.Column.Name == "colDistToFaceExhaust")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(37.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }
            }

            //Where answer is "No"
            if (e.Column.Name == "colForceExOLap" || e.Column.Name == "colSilencerInstOnFan"
                || e.Column.Name == "colHPD" || e.Column.Name == "colAvailable" || e.Column.Name == "colCurrentVentLayout"
                || e.Column.Name == "colVentLayoutCater" || e.Column.Name == "colFanOnStarterBox")
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

            //Where answer is "Yes"
            if (e.Column.Name == "colMajorGeoStructure" || e.Column.Name == "colPotentialHoling" 
                || e.Column.Name == "colFanRecirculation")
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

            //Noise
            if (e.Column.Name == "colNoise")
            {
                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(83.0) && Convert.ToDecimal(e.CellValue) <= Convert.ToDecimal(90.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[0];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(91.0) && Convert.ToDecimal(e.CellValue) <= Convert.ToDecimal(109.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }

                if (Convert.ToDecimal(e.CellValue) >= Convert.ToDecimal(110.0))
                {
                    if (info != null)
                    {
                        info.ContextImage = imgFlags.Images[2];
                        info.CalcViewInfo();
                    }
                    //e.Appearance.BackColor = Color.MistyRose;
                }
            }

            //Where Condition is "Bad".
            if (e.Column.Name == "colCondition" || e.Column.Name == "colVentColSuspension" 
                || e.Column.Name == "colStartBoxInThroughVent" || e.Column.Name == "colVentColSuspension" )
            {
                if (e.CellValue.ToString() == "Bad")
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

        private void gvDevInspWorkplaces_ShowingEditor(object sender, CancelEventArgs e)
        {
            //e.Cancel = false;


            //if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, gvDevInspWorkplaces.Columns["ValueType"]).ToString() == string.Empty)
            //{
            //    e.Cancel = true;
            //}
        }

        private void gvDevInspWorkplaces_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (docalcparam != "Y")
                return;

            //Fan Recirculation
            if (e.Column.Name == "colFanRecirculation")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FanRecirculationData").ToString() == "Yes")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FanRecirculationData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Exhaust Intake WB
            if (e.Column.Name == "colExhaustIntWB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustIntWBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustIntWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Exhaust Intake DB
            if (e.Column.Name == "colExhaustIntDB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustIntDBData")) >= Convert.ToDecimal(37.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustIntDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Exhaust Delivery WB
            if (e.Column.Name == "colExhaustDelWB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustDelWBData")) >= Convert.ToDecimal(37.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustDelWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Exhaust Delivery DB
            if (e.Column.Name == "colExhaustDelDB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustDelDBData")) >= Convert.ToDecimal(37.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustDelDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Force Intake WB
            if (e.Column.Name == "colForceIntWB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceIntWBData")) >= Convert.ToDecimal(37.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceIntWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Force Intake DB
            if (e.Column.Name == "colForceIntDB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceIntDBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceIntDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Force Delivery WB
            if (e.Column.Name == "colForceDelWB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceDelWBData")) >= Convert.ToDecimal(37.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceDelWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Force Delivery DB
            if (e.Column.Name == "colForceDelDB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceDelDBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceDelDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Face WB
            if (e.Column.Name == "colFaceWB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceWBData")) >= Convert.ToDecimal(37.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Face DB
            if (e.Column.Name == "colFaceDB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceDBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Distance To Face "Force"
            if (e.Column.Name == "colDistToFaceForce")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "DistToFaceForceData")) >= Convert.ToDecimal(31.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "DistToFaceForceData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Distance To Face "Exhaust"
            if (e.Column.Name == "colDistToFaceExhaust")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "DistToFaceExhaustData")) >= Convert.ToDecimal(37.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "DistToFaceExhaustData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Force / Exhaust O/Lap 7-20m
            if (e.Column.Name == "colForceExOLap")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceExOLapData").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString();
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceExOLapData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Silencers Installed on Fan 
            if (e.Column.Name == "colSilencerInstOnFan")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "SilencerInstOnFanData").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "SilencerInstOnFanData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Fan on own Starter box 
            if (e.Column.Name == "colFanOnStarterBox")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FanOnStarterBoxData").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FanOnStarterBoxData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Starter box in through Vent. 
            if (e.Column.Name == "colStartBoxInThroughVent")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "StartBoxInThroughVentData").ToString() == "Bad")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "StartBoxInThroughVentData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Vent. column suspension 
            if (e.Column.Name == "colVentColSuspension")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "VentColSuspensionData").ToString() == "Bad")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "VentColSuspensionData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Noise
            if (e.Column.Name == "colNoise")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "NoiseData")) >= Convert.ToDecimal(110.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "NoiseData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //HPD
            if (e.Column.Name == "colHPD")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "HPDData").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "HPDData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //GDI Available
            if (e.Column.Name == "colAvailable")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "AvailableData").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "AvailableData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //GDI Condition.
            if (e.Column.Name == "colCondition")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ConditionData").ToString() == "Bad")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ConditionData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Does the Vent. Layout Cater for...
            if (e.Column.Name == "colVentLayoutCater")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "VentLayoutCaterData").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "VentLayoutCaterData").ToString();
                    //ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Is the Current Vent. Layout adhered to.
            if (e.Column.Name == "colCurrentVentLayout")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "CurrentVentLayoutData").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "CurrentVentLayoutData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }


            //Is the potential holing...
            if (e.Column.Name == "colPotentialHoling")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "PotentialHolingData").ToString() == "Yes")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "PotentialHolingData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }

            //Will the End mine into...
            if (e.Column.Name == "colMajorGeoStructure")
            {
                if (gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "MajorGeoStructureData").ToString() == "Yes")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "MajorGeoStructureData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                    LoadActions();
                    docalcparam = "N";
                }
            }
            // calc co
            if (docalcparam != "N")
                docalc();
            docalcparam = "Y";

            
        }

        private void gvDevInspWorkplaces_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {


            ////1 decimal Place
            //if (gvDevInspWorkplaces.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Decimal(1)")
            //{
            //    if (e.Column.Caption == WP1Desc || e.Column.Caption == WP2Desc || e.Column.Caption == WP3Desc || e.Column.Caption == WP4Desc || e.Column.Caption == WP5Desc
            //        || e.Column.Caption == WP6Desc || e.Column.Caption == WP7Desc || e.Column.Caption == WP8Desc || e.Column.Caption == WP9Desc || e.Column.Caption == WP10Desc
            //        || e.Column.Caption == WP11Desc || e.Column.Caption == WP12Desc || e.Column.Caption == WP13Desc || e.Column.Caption == WP14Desc || e.Column.Caption == WP15Desc
            //        || e.Column.Caption == WP16Desc || e.Column.Caption == WP17Desc || e.Column.Caption == WP18Desc || e.Column.Caption == WP19Desc || e.Column.Caption == WP20Desc)
            //    {
            //        e.RepositoryItem = repSpinDecimal;
            //    }
            //}

            ////2 decimal Place
            //if (gvDevInspWorkplaces.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Decimal(2)")
            //{
            //    if (e.Column.Caption == WP1Desc || e.Column.Caption == WP2Desc || e.Column.Caption == WP3Desc || e.Column.Caption == WP4Desc || e.Column.Caption == WP5Desc
            //        || e.Column.Caption == WP6Desc || e.Column.Caption == WP7Desc || e.Column.Caption == WP8Desc || e.Column.Caption == WP9Desc || e.Column.Caption == WP10Desc
            //        || e.Column.Caption == WP11Desc || e.Column.Caption == WP12Desc || e.Column.Caption == WP13Desc || e.Column.Caption == WP14Desc || e.Column.Caption == WP15Desc
            //        || e.Column.Caption == WP16Desc || e.Column.Caption == WP17Desc || e.Column.Caption == WP18Desc || e.Column.Caption == WP19Desc || e.Column.Caption == WP20Desc)
            //    {
            //        e.RepositoryItem = repTwoDecimal;
            //    }
            //}

            ////Unit
            //if (gvDevInspWorkplaces.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Unit")
            //{
            //    if (e.Column.Caption == WP1Desc || e.Column.Caption == WP2Desc || e.Column.Caption == WP3Desc || e.Column.Caption == WP4Desc || e.Column.Caption == WP5Desc
            //        || e.Column.Caption == WP6Desc || e.Column.Caption == WP7Desc || e.Column.Caption == WP8Desc || e.Column.Caption == WP9Desc || e.Column.Caption == WP10Desc
            //        || e.Column.Caption == WP11Desc || e.Column.Caption == WP12Desc || e.Column.Caption == WP13Desc || e.Column.Caption == WP14Desc || e.Column.Caption == WP15Desc
            //        || e.Column.Caption == WP16Desc || e.Column.Caption == WP17Desc || e.Column.Caption == WP18Desc || e.Column.Caption == WP19Desc || e.Column.Caption == WP20Desc)
            //    {
            //        e.RepositoryItem = repSpinUnit;
            //    }
            //}

            //if (gvDevInspWorkplaces.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Yes/No")
            //{

            //    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
            //    ritem.Items.Add("Yes");
            //    ritem.Items.Add("No");
            //    e.RepositoryItem = ritem;
            //}
        }

        /// <summary>
        /// Per Secton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void gvPerSection_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridCellInfo cellInfo = e.Cell as GridCellInfo;
            TextEditViewInfo info = cellInfo.ViewInfo as TextEditViewInfo;

            if (gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "1" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "2"
                || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "3" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "5"
                || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "6" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "7"
                || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "8" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "9"
                || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "10" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "11"
                || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "13" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "4"
                || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "18" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "19"
                || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "20" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "21"
                || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "22")
            {
                if (e.Column.FieldName == "Answer")
                {
                    if (gvPerSection.GetRowCellValue(e.RowHandle, "Answer").ToString() == "No")
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[1];
                            info.CalcViewInfo();
                        }
                        //e.Appearance.BackColor = Color.LightYellow;
                    }
                }
            }


            if (gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "17" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "12"
               || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "14" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "15"
               || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "16")
            {
                if (e.Column.FieldName == "Answer")
                {
                    if (gvPerSection.GetRowCellValue(e.RowHandle, "Answer").ToString() == "Yes")
                    {
                        if (info != null)
                        {
                            info.ContextImage = imgFlags.Images[1];
                            info.CalcViewInfo();
                        }
                        //e.Appearance.BackColor = Color.LightYellow;
                    }
                }
            }
        }

        private void ofdOpenDocFile_FileOk(object sender, CancelEventArgs e)
        {
            string Docs = openFileDialog1.FileName;

            int indexa = Docs.LastIndexOf("\\");

            string sourcefilename = Docs.Substring(indexa + 1, (Docs.Length - indexa) - 1);

            DocsLB.Items.Add(sourcefilename);
        }

        private void gvPerSection_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (docalcparam != "Y")
                return;

            //Answer = No
            if (gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "1" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "2"
                || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "3" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "5"
                || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "6" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "7"
                || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "8" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "9"
                || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "10" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "11"
                || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "13" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "4"
                || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "18" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "19"
                || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "20" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "21"
                || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "22")
            {
                if (gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Answer").ToString() == "No")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = WorkplaceMain;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.Item = gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Answer").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    LoadActions();
                }
            }


            //Answer = Yes
            if (gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "17" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "12"
               || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "14" || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "15"
               || gvPerSection.GetRowCellValue(e.RowHandle, "QuestID").ToString() == "16")
            {
                if (gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Answer").ToString() == "Yes")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.lblSection.Text = txtSection.EditValue.ToString();
                    ActFrm.lblWorkplace.Text = WorkplaceMain;
                    ActFrm.lblProdMonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnCancel.Enabled = false;
                    ActFrm.cbxWorkplace.Visible = false;
                    ActFrm.WPID = dbl_rec_WPID.Text;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.Item = gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Answer").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    LoadActions();
                }
            }

            

            if (docalcparam != "N")
                docalc();
            docalcparam = "Y";
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
            #endregion


    }
}