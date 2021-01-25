using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Web.UI.WebControls;


namespace Mineware.Systems.DocumentManager.Ventilation
{
    public partial class frmDevelopmentInspection : DevExpress.XtraEditors.XtraForm
    {
        public frmDevelopmentInspection()
        {
            InitializeComponent();
        }

        //Public Declarations
        //Connections
        Procedures procs = new Procedures();
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
        private DataTable dtEngEquipt = new DataTable("dtEngEquipt");


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
        private String WP1 = string.Empty;
        private String WP2 = string.Empty;
        private String WP3 = string.Empty;
        private String WP4 = string.Empty;
        private String WP5 = string.Empty;

        private String WP6 = string.Empty;
        private String WP7 = string.Empty;
        private String WP8 = string.Empty;
        private String WP9 = string.Empty;
        private String WP10 = string.Empty;

        private String WP11 = string.Empty;
        private String WP12 = string.Empty;
        private String WP13 = string.Empty;
        private String WP14 = string.Empty;
        private String WP15 = string.Empty;

        private String WP16 = string.Empty;
        private String WP17 = string.Empty;
        private String WP18 = string.Empty;
        private String WP19 = string.Empty;
        private String WP20 = string.Empty;

        private String WP21 = string.Empty;
        private String WP22 = string.Empty;
        private String WP23 = string.Empty;
        private String WP24 = string.Empty;
        private String WP25 = string.Empty;

        private String WP1Desc = string.Empty;
        private String WP2Desc = string.Empty;
        private String WP3Desc = string.Empty;
        private String WP4Desc = string.Empty;
        private String WP5Desc = string.Empty;

        private String WP6Desc = string.Empty;
        private String WP7Desc = string.Empty;
        private String WP8Desc = string.Empty;
        private String WP9Desc = string.Empty;
        private String WP10Desc = string.Empty;

        private String WP11Desc = string.Empty;
        private String WP12Desc = string.Empty;
        private String WP13Desc = string.Empty;
        private String WP14Desc = string.Empty;
        private String WP15Desc = string.Empty;

        private String WP16Desc = string.Empty;
        private String WP17Desc = string.Empty;
        private String WP18Desc = string.Empty;
        private String WP19Desc = string.Empty;
        private String WP20Desc = string.Empty;

        private String WP21Desc = string.Empty;
        private String WP22Desc = string.Empty;
        private String WP23Desc = string.Empty;
        private String WP24Desc = string.Empty;
        private String WP25Desc = string.Empty;


        private String FileName = string.Empty;

        private int Days = 45;

        DialogResult result1;
        
        string repDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\DevelopmentInspections\Documents";    //Path to store files
        string repImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\DevelopmentInspections";  //Path to store Images
        string ActionsImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\DevelopmentInspections\ActionImages";

        //string repDir = @"\\10.148.225.119\Mineware\Images\VentilationInspections\DevelopmentInspections\Documents";    //Path to store files
        //string repImgDir = @"\\10.148.225.119\Mineware\Images\VentilationInspections\DevelopmentInspections";  //Path to store Images
        //string ActionsImgDir = @"\\10.148.225.119\Mineware\Images\VentilationInspections\DevelopmentInspections\ActionsImages";

        //string repDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections\Documents";    //Path to store files
        //string repImgDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections";  //Path to store Images
        //string ActionsImgDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections\ActionsImages";
        DialogResult fileResults;

        //Private data fields
        //private String FileName = "";
        private String sourceFile;
        private String destinationFile;



        //NOTE: Register all tables for sqlconnector to use
        private void tableRegister()
        {
            dsGlobal.Tables.Add(dtWorkplaceData);
            dsGlobal.Tables.Add(dtActions);
            dsGlobal.Tables.Add(dtData);
            dsGlobal.Tables.Add(dtAuth);
            dsGlobal.Tables.Add(dtDataEdit);
            dsGlobal.Tables.Add(dtDataEdit2);

            //Engineering Questions Tables
            dsGlobal.Tables.Add(dtEngEquipt);
        }

        //Use this for Syncromine
        private void sqlConnector(string sqlQuery, string sqlTableIdentifier)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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

        //Gets Workplaces
        private void LoadWorkplaces()
        {
            string sql = string.Empty;

            sql = "Exec sp_Dept_Insection_VentDevelopment_Questions " + Days + ",'" + txtSection.EditValue.ToString() + "','" + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + "' ";
            sqlConnector(sql, "dtWorkplaceData");


            // do 
            int x = 0;

            foreach (DataRow r in dtWorkplaceData.Rows)
            {
                if (x == 0)
                {
                    WP1 = r["Workplaceid"].ToString();
                    WP1Desc = r["Description"].ToString();
                }

                if (x == 1)
                {
                    //colQFA.Caption = r["Description"].ToString();
                    //colQFA.Visible = true;
                    WP2 = r["Workplaceid"].ToString();
                    WP2Desc = r["Description"].ToString();
                }

                if (x == 2)
                {
                    WP3 = r["Workplaceid"].ToString();
                    WP3Desc = r["Description"].ToString();
                }

                if (x == 3)
                {
                    WP4 = r["Workplaceid"].ToString();
                    WP4Desc = r["Description"].ToString();
                }

                if (x == 4)
                {
                    WP5 = r["Workplaceid"].ToString();
                    WP5Desc = r["Description"].ToString();
                }

                if (x == 5)
                {
                    WP6 = r["Workplaceid"].ToString();
                    WP6Desc = r["Description"].ToString();
                }

                if (x == 6)
                {
                    WP7 = r["Workplaceid"].ToString();
                    WP7Desc = r["Description"].ToString();
                }

                if (x == 7)
                {
                    WP8 = r["Workplaceid"].ToString();
                    WP8Desc = r["Description"].ToString();
                }

                if (x == 8)
                {
                    WP9 = r["Workplaceid"].ToString();
                    WP9Desc = r["Description"].ToString();
                }

                if (x == 9)
                {
                    WP10 = r["Workplaceid"].ToString();
                    WP10Desc = r["Description"].ToString();
                }

                x++;
            }
        }

        //Gets Actions
        private void LoadActions()
        {
            MWDataManager.clsDataAccess _dbManAct = new MWDataManager.clsDataAccess();
            _dbManAct.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManAct.SqlStatement = "EXEC [sp_Dept_Ventilation_LoadActions] '" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "', '" + WP4Desc + "', '" + WP5Desc + "', '" + WP6Desc + "' " +
                ", '" + WP7Desc + "', '" + WP8Desc + "', '" + WP9Desc + "', '" + WP10Desc + "'" +
                  " ,'" + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + "' ";
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

        }

        //Gets all Data
        private void LoadData()
        {
            
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
            
            string sql = string.Empty;
            sql = "Exec sp_Dept_Insection_VentDevelopment_Questions " + Days + ",'" + txtSection.EditValue.ToString() + "','" + SelectedDate + "' ";



            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _sqlConnection.SqlStatement = sql;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;

            gcDevInspWorkplaces.DataSource = null;
            gcDevInspWorkplaces.DataSource = dtReceive;

            //colWPID.FieldName = "NewWP";
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


            sql = "Exec sp_Dept_Insection_Vent_Questions_Development_PerSection '" + SelectedDate + "','" + txtSection.EditValue.ToString() + "' \r\n";

            _sqlConnection.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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
            _dbManVampCheck.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManVampCheck.SqlStatement = "select CONVERT(varchar(1),activity) activity from  WORKPLACE where Description = '" + dbl_rec_WPID.Text + "' ";
            _dbManVampCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManVampCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManVampCheck.ExecuteInstruction();

            DataTable DataVamp = _dbManVampCheck.ResultsDataTable;


            string act = DataVamp.Rows[0]["activity"].ToString();

            if (act != "1")
            {
                _dbManVampCheck.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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
                _dbManVampCheck.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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
                sbSqlQuery.AppendLine(", '" + VentLayoutCaterData + "', '" + CurrentVentLayoutData + "', '" + FanVentLayoutNoData + "', '" + PotentialHolingData + "', '" + MajorGeoStructureData + "' )");

            }

            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _ActionSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionSave.SqlStatement = sbSqlQuery.ToString();

            var ActionResult = _ActionSave.ExecuteInstruction();
            Global.sysNotification.TsysNotification.showNotification("Data Saved", "Development Inspection Captured", Color.CornflowerBlue);
           
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
            _PerSecSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _PerSecSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PerSecSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PerSecSave.SqlStatement = sbSqlQuery.ToString();

            var ActionResult = _PerSecSave.ExecuteInstruction();
            Global.sysNotification.TsysNotification.showNotification("Data Saved", "Development Inspection PerWorkplace Captured", Color.CornflowerBlue);
          

        }



        void SaveBtn_ItemClick()
        {
            //SaveVentStoping();

            /*NOTE: use clsUserInfo.UserID*/
            //sql = " Update tbl_PrePlanning_MonthPlan Set EngDep = 'Y', EngDepCapt = '" + Environment.UserName + "'  where prodmonth = '" + dbl_rec_ProdMonth + "' and Crew = '" + dbl_rec_Crew + "'";
            //sqlConnector(sql, null);

            //sql = "delete from [tbl_Dept_Inspection_VentCapture_Stoping] where Workplace = ''  and Prodmonth = '" + dbl_rec_ProdMonth + "'";
            //sqlConnector(sql, null);

            //MWDataManager.clsDataAccess _Auth = new MWDataManager.clsDataAccess();
            //_Auth.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            //_Auth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_Auth.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_Auth.SqlStatement = " Update tbl_OCR_MonthPlan Set EngDep = 'Y', EngDepCapt = '" + TUserInfo.UserID + "'  where prodmonth = '" + dbl_rec_ProdMonth + "' and Crew = '" + ExtractBeforeColon(dbl_rec_Crew.ToString()) + "' \r\n" +
            //    "";

            //_Auth.ExecuteInstruction();
        }

        void saveFeildBook()
        {

            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);


            string sqlQuery = "delete from tbl_Dept_Inspection_VentCapture_FeildBook where Calendardate = '" + SelectedDate + "' and Section = '" + txtSection.EditValue.ToString() + "' \r\n "
                + " insert into tbl_Dept_Inspection_VentCapture_FeildBook values('" + SelectedDate + "', '" + txtSection.EditValue.ToString() + "', '" + txtFeilBook.Text + "', '" + txtPageNum.Text + "', '" + txtObserverName.Text + "', '" + txtHygName.Text + "', '', '', '', '', '', '' \r\n)";
            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sqlQuery;
            _GenSave.ExecuteInstruction();
        }


        private void LoadFeildBook()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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

        void LoadMainGrid()
        {
            txtProdMonth.EditValue = procs.ProdMonthAsDate(clsUserInfo.ProdMonth.ToString());

            dbl_rec_ProdMonth = procs.ProdMonthAsString(Convert.ToDateTime(txtProdMonth.EditValue.ToString()));

            //Load Data for parameters
            tableRegister();
            LoadWorkplaces();
          
            //loadImage();
            LoadData();
            LoadActions();
            LoadFeildBook();

            //loadDocs();

            CheckAUth();

            formloaded = "Y";
        }

        private void CheckAUth()
        {
            MWDataManager.clsDataAccess _CheckAuth = new MWDataManager.clsDataAccess();
            _CheckAuth.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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

        private void frmDevelopmentInspection_Load(object sender, EventArgs e)
        {
            txtDpInspecDate.EditValue = String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);

            txtCrew.EditValue = dbl_rec_Crew;
            txtSection.EditValue = dbl_rec_MinerSection.Text;
            selectDate = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            LoadMainGrid();

            MWDataManager.clsDataAccess _CheckAUthSecurity = new MWDataManager.clsDataAccess();
            _CheckAUthSecurity.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _CheckAUthSecurity.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _CheckAUthSecurity.queryReturnType = MWDataManager.ReturnType.DataTable;
            _CheckAUthSecurity.SqlStatement = "  Select ProfileID from [Syncromine_New].[dbo].tblUsers users  \r\n" +
                                       ", [Syncromine_New].[dbo].[tblUserProfileLink] link \r\n" +
                                       "  where users.USERID = link.UserID and users.USERID = '" + clsUserInfo.UserID + "'";
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

        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveBtn_ItemClick();
            SaveDevAllWorkplaces();
            SaveDevPerSection();

            saveFeildBook();

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
                    || e.Column.Name == "colPotentialHoling" || e.Column.Name == "colMajorGeoStructure")
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    StringFormat sf = new StringFormat();
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    //sf.FormatFlags |= StringFormatFlags.NoWrap;
                    //sf.Alignment = StringAlignment.Near;

                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(sf), 270);
                    e.Handled = true;


                    //Rectangle r = e.Info.CaptionRect;
                    //r.Inflate(50, 0);
                    //e.Info.Caption = e.Info.Caption;
                    //e.Painter.DrawObject(e.Info);
                    //System.Drawing.Drawing2D.GraphicsState state = e.Graphics.Save();
                    //StringFormat sf = new StringFormat();
                    //sf.Trimming = StringTrimming.EllipsisCharacter;
                    //sf.FormatFlags |= StringFormatFlags.NoWrap;
                    //sf.Alignment = StringAlignment.Near;
                    //r = Transform(e.Graphics, 270, r);
                    //e.Graphics.DrawString(e.Column.Caption, e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), r, sf);
                    //e.Graphics.Restore(state);
                    //e.Handled = true;
                }
            }

        }

        private void gvDevInspWorkplaces_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridCellInfo cellInfo = e.Cell as GridCellInfo;
            TextEditViewInfo info = cellInfo.ViewInfo as TextEditViewInfo;

            if (e.Column.Name == "colKata" || e.Column.Name == "colRelHum")
            {
                e.Appearance.BackColor = Color.Gainsboro;
            }

            //Wet Bulbs and Dry Bulbs
            if (e.Column.Name == "colExhaustIntWB" || e.Column.Name == "colExhaustIntDB"
                || e.Column.Name == "colExhaustDelWB" || e.Column.Name == "colExhaustDelDB"
                || e.Column.Name == "colForceIntWB" || e.Column.Name == "colForceIntDB"
                || e.Column.Name == "colForceDelWB" || e.Column.Name == "colForceDelDB"
                || e.Column.Name == "colFaceWB" || e.Column.Name == "colFaceDB")
            {
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
            if (e.Column.Name == "colMajorGeoStructure" || e.Column.Name == "colPotentialHoling")
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

        string docalcparam = "Y";

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FanRecirculationData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustIntWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            //Exhaust Intake DB
            if (e.Column.Name == "colExhaustIntDB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustIntDBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustIntDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            //Exhaust Delivery WB
            if (e.Column.Name == "colExhaustDelWB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustDelWBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustDelWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            //Exhaust Delivery DB
            if (e.Column.Name == "colExhaustDelDB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustDelDBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ExhaustDelDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            //Force Intake WB
            if (e.Column.Name == "colForceIntWB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceIntWBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceIntWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceIntDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            //Force Delivery WB
            if (e.Column.Name == "colForceDelWB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceDelWBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceDelWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceDelDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            //Face WB
            if (e.Column.Name == "colFaceWB")
            {
                if (Convert.ToDecimal(gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceWBData")) >= Convert.ToDecimal(33.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceWBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FaceDBData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "DistToFaceForceData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "DistToFaceExhaustData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString();
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ForceExOLapData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "SilencerInstOnFanData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "FanOnStarterBoxData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "StartBoxInThroughVentData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "VentColSuspensionData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "NoiseData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "HPDData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "AvailableData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "ConditionData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "VentLayoutCaterData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "CurrentVentLayoutData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "PotentialHolingData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Description").ToString(); ;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "Workplaceid").ToString();
                    ActFrm.Item = gvDevInspWorkplaces.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvDevInspWorkplaces.GetRowCellValue(gvDevInspWorkplaces.FocusedRowHandle, "MajorGeoStructureData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }
            // calc co
            if (docalcparam != "N")
                docalc();
            docalcparam = "Y";

            LoadActions();
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

                // wp1
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
            CalcComp();
            docalcparam = "Y";

        }

        private void CalcComp()
        {
            decimal CountActionsWP1 = 0;
            decimal CountActionsWP2 = 0;
            decimal CountActionsWP3 = 0;
            decimal CountActionsWP4 = 0;
            decimal CountActionsWP5 = 0;
            decimal CountActionsWP6 = 0;
            decimal CountActionsWP7 = 0;
            decimal CountActionsWP8 = 0;
            decimal CountActionsWP9 = 0;
            decimal CountActionsWP10 = 0;

            decimal CountActionsWP11 = 0;
            decimal CountActionsWP12 = 0;
            decimal CountActionsWP13 = 0;
            decimal CountActionsWP14 = 0;
            decimal CountActionsWP15 = 0;
            decimal CountActionsWP16 = 0;
            decimal CountActionsWP17 = 0;
            decimal CountActionsWP18 = 0;
            decimal CountActionsWP19 = 0;
            decimal CountActionsWP20 = 0;

            for (int rows = 0; rows < gvDevInspWorkplaces.RowCount; rows++)
            {
                if (rows == 6 || rows == 8 || rows == 10 || rows == 15 || rows == 18 || rows == 20
                    || rows == 23 || rows == 26 || rows == 28 || rows == 30 || rows == 35 || rows == 37 || rows == 39
                    || rows == 41 || rows == 44 || rows == 49 || rows == 51 || rows == 53 || rows == 55 || rows == 57
                    || rows == 60 || rows == 65 || rows == 69 || rows == 71 || rows == 73 || rows == 83
                    || rows == 91 || rows == 93)
                {
                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP1"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP1"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP1"]).ToString() == "C")
                    {
                        CountActionsWP1 = CountActionsWP1 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP2"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP2"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP2"]).ToString() == "C")
                    {
                        CountActionsWP2 = CountActionsWP2 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP3"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP3"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP3"]).ToString() == "C")
                    {
                        CountActionsWP3 = CountActionsWP3 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP4"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP4"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP4"]).ToString() == "C")
                    {
                        CountActionsWP4 = CountActionsWP4 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP5"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP5"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP5"]).ToString() == "C")
                    {
                        CountActionsWP5 = CountActionsWP5 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP6"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP6"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP6"]).ToString() == "C")
                    {
                        CountActionsWP6 = CountActionsWP6 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP7"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP7"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP7"]).ToString() == "C")
                    {
                        CountActionsWP7 = CountActionsWP7 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP8"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP8"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP8"]).ToString() == "C")
                    {
                        CountActionsWP8 = CountActionsWP8 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP9"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP9"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP9"]).ToString() == "C")
                    {
                        CountActionsWP9 = CountActionsWP9 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP10"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP10"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP10"]).ToString() == "C")
                    {
                        CountActionsWP10 = CountActionsWP10 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP11"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP11"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP11"]).ToString() == "C")
                    {
                        CountActionsWP11 = CountActionsWP11 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP12"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP12"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP12"]).ToString() == "C")
                    {
                        CountActionsWP12 = CountActionsWP12 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP13"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP13"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP13"]).ToString() == "C")
                    {
                        CountActionsWP13 = CountActionsWP13 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP14"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP14"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP14"]).ToString() == "C")
                    {
                        CountActionsWP14 = CountActionsWP14 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP15"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP15"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP15"]).ToString() == "C")
                    {
                        CountActionsWP15 = CountActionsWP15 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP16"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP16"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP16"]).ToString() == "C")
                    {
                        CountActionsWP16 = CountActionsWP16 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP17"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP17"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP17"]).ToString() == "C")
                    {
                        CountActionsWP17 = CountActionsWP17 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP18"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP18"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP18"]).ToString() == "C")
                    {
                        CountActionsWP18 = CountActionsWP18 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP19"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP19"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP19"]).ToString() == "C")
                    {
                        CountActionsWP19 = CountActionsWP19 + 1;
                    }

                    if (gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP20"]).ToString() == "A" || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP20"]).ToString() == "B"
                        || gvDevInspWorkplaces.GetRowCellValue(rows, gvDevInspWorkplaces.Columns["WP20"]).ToString() == "C")
                    {
                        CountActionsWP20 = CountActionsWP20 + 1;
                    }
                }
            }

            decimal Wp1Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP1 / 27) * 100))), 1);
            if (Wp1Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP1"], Wp1Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP1"], 100);
            }

            decimal Wp2Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP2 / 27) * 100)), 1);
            if (Wp2Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP2"], Wp2Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP2"], 100);
            }

            decimal Wp3Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP3 / 27) * 100)), 1);
            if (Wp3Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP3"], Wp3Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP3"], 100);
            }

            decimal Wp4Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP4 / 27) * 100)), 1);
            if (Wp4Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP4"], Wp4Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP4"], 100);
            }

            decimal Wp5Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP5 / 27) * 100)), 1);
            if (Wp5Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP5"], Wp5Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP5"], 100);
            }

            decimal Wp6Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP6 / 27) * 100)), 1);
            if (Wp6Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP6"], Wp6Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP6"], 100);
            }

            decimal Wp7Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP7 / 27) * 100)), 1);
            if (Wp7Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP7"], Wp7Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP7"], 100);
            }

            decimal Wp8Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP8 / 27) * 100)), 1);
            if (Wp8Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP8"], Wp8Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP8"], 100);
            }

            decimal Wp9Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP9 / 27) * 100)), 1);
            if (Wp9Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP9"], Wp9Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP9"], 100);
            }

            decimal Wp10Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP10 / 27) * 100)), 1);
            if (Wp10Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP10"], Wp10Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP10"], 100);
            }

            decimal Wp11Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP11 / 27) * 100))), 1);
            if (Wp11Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP11"], Wp11Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP11"], 100);
            }

            decimal Wp12Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP12 / 27) * 100))), 1);
            if (Wp12Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP12"], Wp12Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP12"], 100);
            }

            decimal Wp13Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP13 / 27) * 100))), 1);
            if (Wp13Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP13"], Wp13Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP13"], 100);
            }

            decimal Wp14Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP14 / 27) * 100))), 1);
            if (Wp14Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP14"], Wp14Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP14"], 100);
            }

            decimal Wp15Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP15 / 27) * 100))), 1);
            if (Wp15Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP15"], Wp15Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP15"], 100);
            }

            decimal Wp16Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP16 / 27) * 100))), 1);
            if (Wp16Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP16"], Wp16Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP16"], 100);
            }

            decimal Wp17Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP17 / 27) * 100))), 1);
            if (Wp17Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP17"], Wp17Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP17"], 100);
            }

            decimal Wp18Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP18 / 27) * 100))), 1);
            if (Wp18Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP18"], Wp18Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP18"], 100);
            }

            decimal Wp19Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP19 / 27) * 100))), 1);
            if (Wp19Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP19"], Wp19Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP19"], 100);
            }

            decimal Wp20Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP20 / 27) * 100))), 1);
            if (Wp20Comp != 1)
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP20"], Wp20Comp);
            }
            else
            {
                gvDevInspWorkplaces.SetRowCellValue(0, gvDevInspWorkplaces.Columns["WP20"], 100);
            }
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

        private void AddActBtn_Click(object sender, EventArgs e)
        {
            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
            ActFrm.txtSection.EditValue = txtSection.EditValue;
            ActFrm.cbxWorkplace.EditValue = WP1Desc;
            ActFrm.WPComboEdit.Items.Add(WP1Desc);
            if (WP2Desc != string.Empty)
            {
                ActFrm.WPComboEdit.Items.Add(WP2Desc);
            }
            if (WP3Desc != string.Empty)
            {
                ActFrm.WPComboEdit.Items.Add(WP3Desc);
            }
            if (WP4Desc != string.Empty)
            {
                ActFrm.WPComboEdit.Items.Add(WP4Desc);
            }
            if (WP5Desc != string.Empty)
            {
                ActFrm.WPComboEdit.Items.Add(WP5Desc);
            }
            if (WP6Desc != string.Empty)
            {
                ActFrm.WPComboEdit.Items.Add(WP6Desc);
            }
            if (WP7Desc != string.Empty)
            {
                ActFrm.WPComboEdit.Items.Add(WP7Desc);
            }
            if (WP8Desc != string.Empty)
            {
                ActFrm.WPComboEdit.Items.Add(WP8Desc);
            }
            if (WP9Desc != string.Empty)
            {
                ActFrm.WPComboEdit.Items.Add(WP9Desc);
            }
            if (WP10Desc != string.Empty)
            {
                ActFrm.WPComboEdit.Items.Add(WP10Desc);
            }
            //if (WP11Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP11Desc);
            //}
            //if (WP12Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP12Desc);
            //}
            //if (WP13Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP13Desc);
            //}
            //if (WP14Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP14Desc);
            //}
            //if (WP15Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP15Desc);
            //}
            //if (WP16Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP16Desc);
            //}
            //if (WP17Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP17Desc);
            //}
            //if (WP18Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP18Desc);
            //}
            //if (WP19Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP19Desc);
            //}
            //if (WP20Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP20Desc);
            //}

            ActFrm.txtWorkplace.EditValue = WP1Desc;

            ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ActFrm.txtWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            ActFrm._theSystemDBTag = _theSystemDBTag;
            ActFrm._UserCurrentInfo = _UserCurrentInfo;

            ActFrm.Item = "Vent Schedule Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";


            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        /// <summary>
        /// Per Secton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void gvPerSection_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
        //    //Yes/No
        //    if (gvPerSection.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Yes/No")
        //    {
        //        if (e.Column.FieldName == "Answer")
        //        {
        //            RepositoryItemComboBox ritem = new RepositoryItemComboBox();
        //            ritem.Items.Add("Yes");
        //            ritem.Items.Add("No");
        //            e.RepositoryItem = ritem;
        //        }
        //    }


        //    //Good/Poor
        //    if (gvPerSection.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Poor")
        //    {
        //        if (e.Column.FieldName == "Answer")
        //        {
        //            RepositoryItemComboBox ritem = new RepositoryItemComboBox();
        //            ritem.Items.Add("Good");
        //            ritem.Items.Add("Average");
        //            ritem.Items.Add("Poor");
        //            e.RepositoryItem = ritem;
        //        }
        //    }

        //    //1 decimal Place
        //    if (gvPerSection.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Decimal(1)")
        //    {
        //        if (e.Column.FieldName == "Answer")
        //        {
        //            e.RepositoryItem = repPerDecimal;
        //        }
        //    }

        //    //Unit
        //    if (gvPerSection.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Unit")
        //    {
        //        if (e.Column.FieldName == "Answer")
        //        {
        //            e.RepositoryItem = repPerUnit;
        //        }
        //    }

        }

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

        private void gvPerSection_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = false;


            if (gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, gvPerSection.Columns["ValueType"]).ToString() == string.Empty)
            {
                e.Cancel = true;
            }
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

                            //destinationFile = @"C:\Images" + "\\" + FileName + Ext;//+ FileName + Name.ToString() + Ext
                            destinationFile = repImgDir + "\\" + FileName + Ext;//+ FileName + Name.ToString() + Ext
                                                                                //PicBox.Image = Image.FromFile(openFileDialog1.FileName); 
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

        private void btnAddDevDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fileResults = openFileDialog1.ShowDialog();
            GetDoc();
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

                //if (!txtCrew.EditValue.Equals(""))
                //{
                //	FileName = procs.ExtractAfterColon(txtCrew.EditValue.ToString());
                //}
                //else
                //{
                //	XtraMessageBox.Show("Please select a workplace");
                //	return;
                //}

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
                        System.IO.File.Copy(sourceFile, destinationFile, true);
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
                LoadActions();
                LoadFeildBook();

            }
        }

        private void gvDevInspWorkplaces_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            //string Rowhandle = gvDevInspWorkplaces.GetRowCellValue(e.RowHandle, gvDevInspWorkplaces.Columns["Question"]).ToString();

            //string Questid = gvDevInspWorkplaces.GetRowCellValue(e.RowHandle, gvDevInspWorkplaces.Columns["QuestID"]).ToString();

            //string Test = string.Empty;
        }

        private void DelActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please click the row you want to delete first.");
                return;
            }

            MWDataManager.clsDataAccess _DeleteAction = new MWDataManager.clsDataAccess();
            _DeleteAction.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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
        }

        private void ofdOpenDocFile_FileOk(object sender, CancelEventArgs e)
        {
            string Docs = openFileDialog1.FileName;

            int indexa = Docs.LastIndexOf("\\");

            string sourcefilename = Docs.Substring(indexa + 1, (Docs.Length - indexa) - 1);

            DocsLB.Items.Add(sourcefilename);
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
                || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "13" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "17"
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
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = WorkplaceMain;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = WP1;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.Item = gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Answer").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                }
            }


            //Answer = Yes
            if (gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "4" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "12"
               || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "14" || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "15"
               || gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "QuestID").ToString() == "16")
            {
                if (gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Answer").ToString() == "Yes")
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.txtSection.EditValue = txtSection.EditValue;
                    ActFrm.txtWorkplace.EditValue = WorkplaceMain;
                    ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtDpInspecDate.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(txtDpInspecDate.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = dbl_rec_WPID.Text;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);
                    ActFrm.Item = gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvPerSection.GetRowCellValue(gvPerSection.FocusedRowHandle, "Answer").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);
                }
            }

            LoadActions();

            if (docalcparam != "N")
                docalc();
            docalcparam = "Y";
        }

        private void btnAuthorise_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbAuth = new MWDataManager.clsDataAccess();
            _dbAuth.SqlStatement = "delete from tbl_Dept_Inspection_VentAuthorise where Activity = 1 and \r\n "
                + " CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + "' and SectionID = '" + txtSection.EditValue.ToString() + "' \r\n ";

            _dbAuth.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAuth.queryReturnType = MWDataManager.ReturnType.DataTable;

            if (btnAuthorise.Caption == "Authorise")
            {
                _dbAuth.SqlStatement += " insert into tbl_Dept_Inspection_VentAuthorise (Activity, CalendarDate, SectionID) \r\n "
                                + " values(1, '" + String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue) + "', '" + txtSection.EditValue.ToString() + "')";
            }


            var uathResult = _dbAuth.ExecuteInstruction();
            CheckAUth();
            Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace authorised", Color.CornflowerBlue);

            
        }

        private void EditActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");

                return;
            }

            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            ActFrm.txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtProdMonth.EditValue.ToString())); //String.Format("{0:yyyy-MM-dd}", tbProdMonth.EditValue.ToString());
            ActFrm.txtSection.EditValue = txtSection.EditValue;
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtDpInspecDate.EditValue);

            ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            ActFrm.txtWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;


            ActFrm._theSystemDBTag = this._theSystemDBTag;
            ActFrm._UserCurrentInfo = this._UserCurrentInfo;

            ActFrm.Item = "Inspection Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";

            ActFrm.txtWorkplace.EditValue = Workplace;
            ActFrm.Item = Description;
            ActFrm.ActionTxt.Text = Recomendation;
            ActFrm.ReqDate.Text = TargetDate;
            ActFrm.PriorityCmb.Text = Priority;
            ActFrm.RespPersonCmb.EditValue = RespPerson;
            ActFrm.OverseerCmb.EditValue = Overseer;

            ActFrm.ActID = ID;

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();

        }

        private void gvDevInspWorkplaces_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {
            //Hide row "Pressure Air"   
            if (e.ListSourceRow == 59)
            {
                e.Visible = false;
                e.Handled = true;
            }
        }

        private void dbl_rec_MinerSection_Click(object sender, EventArgs e)
        {

        }

        private void dbl_rec_Date_Click(object sender, EventArgs e)
        {

        }

        private void lblCon_Click(object sender, EventArgs e)
        {

        }

        private void dbl_rec_WPID_Click(object sender, EventArgs e)
        {

        }


    }
}