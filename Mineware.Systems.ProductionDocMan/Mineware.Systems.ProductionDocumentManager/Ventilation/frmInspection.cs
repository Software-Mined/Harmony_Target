using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.ViewInfo;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace Mineware.Systems.DocumentManager.Ventilation
{
    public partial class frmInspection : DevExpress.XtraEditors.XtraForm
    {
        public frmInspection()
        {
            InitializeComponent();
        }

        //Public Declarations
        //Connections

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
        string AvgAuiRow16;


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
        private DataTable dtEngEquipt = new DataTable("dtEngEquipt");

        //Strings
        private String frmFirstLoad = "Y";
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

        DialogResult result1;

        string repDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\StandardInspections\Documents";    //Path to store files
        string repImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\StandardInspections";  //Path to store Images
        string ActionsImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\StandardInspections\ActionsImages";


        //string repDir = @"\\10.148.225.119\Mineware\Images\VentilationInspections\StandardInspections\Documents";    //Path to store files
        //string repImgDir = @"\\10.148.225.119\Mineware\Images\VentilationInspections\StandardInspections";  //Path to store Images
        //string ActionsImgDir = @"\\10.148.225.119\Mineware\Images\VentilationInspections\StandardInspections\ActionsImages";

        DialogResult fileResults;

        //Private data fields
        //private String FileName = "";
        private String sourceFile;
        private String destinationFile;


        string IsFormAuth = "N";


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
            sql = "exec sp_Dept_Insection_VentStoping_Questions 45, '" + dbl_rec_MinerSection.Text + "', '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue) + "'   \r\n";
            sqlConnector(sql, "dtWorkplaceData");

            int x = 0;

            foreach (DataRow r in dtWorkplaceData.Rows)
            {
                if (x == 0)
                {
                    WP1 = r["NewWP"].ToString();
                    WP1Desc = r["Description"].ToString();
                }

                if (x == 1)
                {
                    WP2 = r["NewWP"].ToString();
                    WP2Desc = r["Description"].ToString();
                }

                if (x == 2)
                {
                    WP3 = r["NewWP"].ToString();
                    WP3Desc = r["Description"].ToString();
                }

                if (x == 3)
                {
                    WP4 = r["NewWP"].ToString();
                    WP4Desc = r["Description"].ToString();
                }

                if (x == 4)
                {
                    WP5 = r["NewWP"].ToString();
                    WP5Desc = r["Description"].ToString();
                }

                if (x == 5)
                {
                    WP6 = r["NewWP"].ToString();
                    WP6Desc = r["Description"].ToString();
                }

                if (x == 6)
                {
                    WP7 = r["NewWP"].ToString();
                    WP7Desc = r["Description"].ToString();
                }

                if (x == 7)
                {
                    WP8 = r["NewWP"].ToString();
                    WP8Desc = r["Description"].ToString();
                }

                if (x == 8)
                {
                    WP9 = r["NewWP"].ToString();
                    WP9Desc = r["Description"].ToString();
                }

                if (x == 9)
                {
                    WP10 = r["NewWP"].ToString();
                    WP10Desc = r["Description"].ToString();
                }
                x++;
            }

              
            }

        private void LoadFeildBook()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);

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


                + " from tbl_Dept_Inspection_VentCapture_FeildBook where Calendardate = '" + SelectedDate + "' and Section = '" + dbl_rec_MinerSection.Text + "' ";
            _GenSave.ExecuteInstruction();



            DataTable dtFeild = _GenSave.ResultsDataTable;

            txtFeilBook.Text = string.Empty;
            txtObserverName.Text = string.Empty;
            txtPageNum.Text = string.Empty;

            txtHygName.Text = string.Empty;

            Pers1.Text = string.Empty;
            Pers2.Text = string.Empty;
            Pers3.Text = string.Empty;
            Pers4.Text = string.Empty;
            Pers5.Text = string.Empty;
            Pers6.Text = string.Empty;

            if (dtFeild.Rows.Count > 0)
            {
                txtFeilBook.Text = dtFeild.Rows[0][2].ToString();
                txtPageNum.Text = dtFeild.Rows[0][3].ToString();
                txtObserverName.Text = dtFeild.Rows[0][4].ToString();
                txtHygName.Text = dtFeild.Rows[0][5].ToString();

                Pers1.Text = dtFeild.Rows[0][6].ToString();
                Pers2.Text = dtFeild.Rows[0][7].ToString();
                Pers3.Text = dtFeild.Rows[0][8].ToString();
                Pers4.Text = dtFeild.Rows[0][9].ToString();
                Pers5.Text = dtFeild.Rows[0][10].ToString();
                Pers6.Text = dtFeild.Rows[0][11].ToString();
            }


        }

        //Gets Actions
        private void LoadActions()
        {
            MWDataManager.clsDataAccess _dbManAct = new MWDataManager.clsDataAccess();
            _dbManAct.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManAct.SqlStatement = "EXEC [sp_Dept_Ventilation_LoadActions] '" + WP1Desc + "', '" + WP2Desc + "', '" + WP3Desc + "', '" + WP4Desc + "', '" + WP5Desc + "', '" + WP6Desc + "' " +
                ", '" + WP7Desc + "', '" + WP8Desc + "', '" + WP9Desc + "', '" + WP10Desc + "','" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue) + "' ";
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
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);

            string sql = string.Empty;
            sql = "exec sp_Dept_Insection_VentStoping_Questions 45, '" + dbl_rec_MinerSection.Text + "', '" + SelectedDate + "'  \r\n";

            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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


            LoadActions();
            loadImage();
            loadDocs();


            ///General Info
            sql = "exec sp_Dept_Insection_Vent_Questions_Stoping_PerSection '" + SelectedDate + "'," +
                " '" + dbl_rec_MinerSection.Text + "'  ";
            _sqlConnection.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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

            DateTime DateOFReport = Convert.ToDateTime(tbDpInspecDate2.EditValue).Date;
            gvGeneralInfo.SetRowCellValue(22, gvGeneralInfo.Columns["Answer"], Convert.ToDateTime(DateOFReport));



            ///Station Temp
            sql = "exec sp_Dept_Insection_Vent_Questions_Stoping_AvailableTemp '" + SelectedDate + "','" + dbl_rec_MinerSection.Text + "' ";
            _sqlConnection.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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

            ///RefugeBay		

            sql = "exec sp_Dept_Insection_VentStoping_RefugeBay '" + dbl_rec_ProdMonth + "', '" + SelectedDate + "', '" + dbl_rec_MinerSection.Text + "' ";

            _sqlConnection.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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
        }



        void SaveVentStoping()
        {
            if (dbl_rec_WPID.Visible == false)
            {
                //MessageBox.Show("Please Select a workplace to activiate", "Enter Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        
        private void EditActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");
                return;
            }

            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbProdMonth2.EditValue.ToString())); //String.Format("{0:yyyy-MM-dd}", tbProdMonth2.EditValue.ToString());
            ActFrm.txtSection.EditValue = tbSection2.EditValue;

            ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            ActFrm._theSystemDBTag = this._theSystemDBTag;
            ActFrm._UserCurrentInfo = this._UserCurrentInfo;
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);

            ActFrm.Item = "Inspection Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";

            ActFrm.txtWorkplace.EditValue = WP1Desc;
            ActFrm.Item = Description;
            ActFrm.ActionTxt.Text = Recomendation;
            ActFrm.ReqDate.Text = TargetDate;
            ActFrm.PriorityCmb.Text = Priority;
            ActFrm.RespPersonCmb.EditValue = RespPerson;
            ActFrm.OverseerCmb.EditValue = Overseer;
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);

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


        /// <summary>
        /// Capture Input data methods/procedures
        /// </summary>
        void SaveBtn_ItemClick()
        {
            gvRockEng.PostEditor();
            gvRefugeBay.PostEditor();
            gvGeneralInfo.PostEditor();

            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
            sbSqlQuery.Clear();
            sbSqlQuery.AppendLine("delete from tbl_Dept_Inspection_VentStoping_Capture where Section = '" + tbSection2.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' ");

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

                sbSqlQuery.AppendLine(" insert into tbl_Dept_Inspection_VentStoping_Capture ");
                sbSqlQuery.AppendLine("values('" + SelectedDate + "', '" + Workplaceid + "', '" + tbSection2.EditValue.ToString() + "', " + dbl_rec_ProdMonth + ", '" + TypeOfWork + "', ");
                sbSqlQuery.AppendLine("'" + Activity + "', " + WBTop + ", " + WBBot + ", " + DBTop + ", " + DBBot + ", " + VelocityTop + ", " + VelocityBot + "," + Kata + "," + AveStopeWidth + ",");
                sbSqlQuery.AppendLine(" " + RelHum + ", " + AUI + ", " + SCP + ", " + Noise + ", '" + HDP + "', '" + ContToFaceTop + "', '" + ContToFaceBot + "', '" + VentContCondition + "',");
                sbSqlQuery.AppendLine("'" + PillarHolingDistToFace + "', '" + PillarHolingSize + "', '" + PillarHolingsRestricted + "', " + NoOfOpenPillarHoling + ", '" + CGBrattInstalled + "', '" + CGBrattEffective + "', '" + VelTroughCGBratt + "', '" + TempCGBratt + "',");
                sbSqlQuery.AppendLine("'" + GasIntrAvailable + "', '" + IntrumentNumber + "', '" + ConditionGDI + "', '" + Knowledge + "', '" + PanelLeadLeg + "', '" + DesignEncorVentHoling + "', '" + VentLayoutAdheredTo + "', '" + GeologicalStructure + "',");
                sbSqlQuery.AppendLine("'" + Available + "', '" + ConditionVent + "', '" + KnowledgeVentIntrument + "', '" + KnowledgeEscapeRoute + "', '" + GenVentKnowledge + "' )");

            }
            
            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _ActionSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionSave.SqlStatement = sbSqlQuery.ToString();

            var ActionResult = _ActionSave.ExecuteInstruction();
            
        }

        void SaveGeneralInfo()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
            sbSqlQuery.Clear();
            sbSqlQuery.AppendLine(" delete from [tbl_Dept_Inspection_VentCapture_Stoping_PerSection] where section = '" + tbSection2.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' ");

            for (int row = 0; row < gvGeneralInfo.RowCount; row++)
            {
                //Declarations
                string QuestID = gvGeneralInfo.GetRowCellValue(row, gvGeneralInfo.Columns["QuestID"]).ToString();
                string WP1Answer = gvGeneralInfo.GetRowCellValue(row, gvGeneralInfo.Columns["Answer"]).ToString();

                sbSqlQuery.AppendLine(" insert into [tbl_Dept_Inspection_VentCapture_Stoping_PerSection] (calendardate,month,Section,Crew,Workplace,QuestID,Answer) ");
                sbSqlQuery.AppendLine(" values('" + SelectedDate + "', " + dbl_rec_ProdMonth + ", '" + tbSection2.EditValue.ToString() + "', '" + ExtractBeforeColon(dbl_rec_Crew) + "', '" + WP1 + "',");
                sbSqlQuery.AppendLine(" '" + QuestID + "', '" + WP1Answer + "' )");                
            }

            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sbSqlQuery.ToString();

            var ActionResult = _GenSave.ExecuteInstruction();

           
        }

        void SaveAvailableTemp()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);

            string sql = " delete from tbl_Dept_Inspection_VentCapture_StationTemp \r\n " +
                " where Calendardate = '" + SelectedDate + "' and Section = '" + tbSection2.EditValue.ToString() + "' \r\n";

            for (int row = 0; row < gvAvailableTemp.RowCount; row++)
            {
                string IntakeWetBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["IntakeWetBulb"]).ToString();
                string IntakeDryBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["IntakeDryBulb"]).ToString();
                string ReturnWetBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["ReturnWetBulb"]).ToString();
                string ReturnDryBulb = gvAvailableTemp.GetRowCellValue(row, gvAvailableTemp.Columns["ReturnDryBulb"]).ToString();
                
                sql += " insert into [tbl_Dept_Inspection_VentCapture_StationTemp] \r\n "
                    + " values('" + SelectedDate + "', '" + tbSection2.EditValue.ToString() + "', " + IntakeWetBulb + ", " + IntakeDryBulb + ", " + ReturnWetBulb + ", " + ReturnDryBulb + ")";


            }

            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sql;

            var ActionResult = _GenSave.ExecuteInstruction();

            
        }

        void SaveRefugeBay()
        {
            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);

            string sql = " delete from tbl_Dept_Vent_RefugeBayCapture where section = '" + tbSection2.EditValue.ToString() + "' and calendardate = '" + SelectedDate + "' \r\n";

            for (int row = 0; row < gvRefugeBay.RowCount; row++)
            {
                //Declarations
                string RefDistToWorkplaceData = gvRefugeBay.GetRowCellValue(row, gvRefugeBay.Columns["RefDistToWorkplaceData"]).ToString();
                string LifeSustData = gvRefugeBay.GetRowCellValue(row, gvRefugeBay.Columns["LifeSustData"]).ToString();
                string LastWpEmergencyDrillData = String.Format("{0:yyyy-MM-dd}", gvRefugeBay.GetRowCellValue(row, gvRefugeBay.Columns["LastWpEmergencyDrillData"]));

                sql += " insert into [tbl_Dept_Vent_RefugeBayCapture] \r\n" +
                    " values(" + dbl_rec_ProdMonth + ", '" + tbSection2.EditValue.ToString() + "', '" + SelectedDate + "', '" + RefDistToWorkplaceData + "',   \r\n" +
                    " '" + LifeSustData + "', '" + String.Format("{0:yyyy-MM-dd}", LastWpEmergencyDrillData) + "' )";
            }

            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sql;

            var ActionResult = _GenSave.ExecuteInstruction();

            
        }

        void saveFeildBook()
        {

            string SelectedDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);


            string sqlQuery = "delete from tbl_Dept_Inspection_VentCapture_FeildBook where Calendardate = '" + SelectedDate + "' and Section = '" + tbSection2.EditValue.ToString() + "' \r\n "
                + " insert into tbl_Dept_Inspection_VentCapture_FeildBook values('" + SelectedDate + "', '" + tbSection2.EditValue.ToString() + "', '" + txtFeilBook.Text + "', '" + txtPageNum.Text + "', '" + txtObserverName.Text + "', '" + txtHygName.Text + "', '" + Pers1.Text + "', '" + Pers2.Text + "', '" + Pers3.Text + "', '" + Pers4.Text + "', '" + Pers5.Text + "', '" + Pers6.Text + "' \r\n)";
            MWDataManager.clsDataAccess _GenSave = new MWDataManager.clsDataAccess();
            _GenSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _GenSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GenSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GenSave.SqlStatement = sqlQuery;
            _GenSave.ExecuteInstruction();
        }

        private void frmStopeVent_Load(object sender, EventArgs e)
        {
            tbCrew2.EditValue = dbl_rec_Crew;
            tbSection2.EditValue = dbl_rec_MinerSection.Text;
 
            tbDpInspecDate2.EditValue = String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);

            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
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
            txtObserverName.Visible = true;

            frmFirstLoad = "N";

            LoadMainGrid();
        }

        void LoadMainGrid()
        {
            tbProdMonth2.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString());

            dbl_rec_ProdMonth = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbProdMonth2.EditValue.ToString()));

            tableRegister();

            LoadWorkplaces();
            LoadData();
            LoadActions();
            CHeckAuth();

            LoadFeildBook();

            if (dbl_rec_Date.Text == "Y")
            {
                ribbonControl2.Visible = false;

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
            _CheckAuth.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _CheckAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _CheckAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _CheckAuth.SqlStatement = "Select * from tbl_Dept_Inspection_VentAuthorise \r\n" +
                                      "where activity = '0' and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue) + "' and SectionID = '" + tbSection2.EditValue.ToString() + "' ";
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

                for (int col = 0; col < gvStationTemp.Columns.Count; col++)
                {
                    gvStationTemp.Columns[col].OptionsColumn.AllowEdit = false;
                }

                SaveBtn.Enabled = false;
                AddImBtn.Enabled = false;
                btnAddStopDoc.Enabled = false;
                barbtnFireRating.Enabled = false;

                AddActBtn.Enabled = false;
                EditActBtn.Enabled = false;
                DelActBtn.Enabled = false;

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

                for (int col = 0; col < gvStationTemp.Columns.Count; col++)
                {
                    gvStationTemp.Columns[col].OptionsColumn.AllowEdit = true;
                }

                SaveBtn.Enabled = true;
                AddImBtn.Enabled = true;
                btnAddStopDoc.Enabled = true;
                barbtnFireRating.Enabled = true;

                AddActBtn.Enabled = true;
                EditActBtn.Enabled = true;
                DelActBtn.Enabled = true;
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

                if (!tbSection2.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + tbSection2.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a Section");
                    return;
                }

                if (tbProdMonth2.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
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

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(repImgDir);

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                string s1 = @"\\10.148.225.119\Mineware\Images\VentilationInspections\StandardInspections\\A114301100052020-04-16.jpg";
                string s2 = @"\\10.148.225.119\Mineware\Images\VentilationInspections\StandardInspections\\A114301100052020-04-16.jpg";

                if (item.ToString() == repImgDir + "\\" + tbSection2.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue) + ext)
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

                //if (!tbCrew.EditValue.Equals(""))
                //{
                //	FileName = ProductionGlobal.ProductionGlobal.ExtractAfterColon(tbCrew.EditValue.ToString());
                //}
                //else
                //{
                //	XtraMessageBox.Show("Please select a workplace");
                //	return;
                //}

                if (!tbSection2.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + tbSection2.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a Section");
                    return;
                }

                if (!tbProdMonth2.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
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
                //string dd = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                int indexprodmonth = sourcefilename.IndexOf(String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue));

                string SourcefileCheck = sourcefilename.Substring(0, indexprodmonth + 10);

                int NameLength = sourcefilename.Length - SourcefileCheck.Length;

                string Docsname = sourcefilename.Substring(SourcefileCheck.Length, NameLength);

                if (SourcefileCheck == tbSection2.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue))
                {
                    DocsLB.Items.Add(Docsname.ToString());
                }
            }

        }

        private void GcRockEng_Click(object sender, EventArgs e)
        {

        }

        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveBtn_ItemClick();

            SaveGeneralInfo();

            SaveAvailableTemp();

            SaveRefugeBay();

            saveFeildBook();

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
                    //|| e.Column.Name == "colVentContCond" || e.Column.Name == "colPilHolDistFace"
                    //|| e.Column.Name == "colPilHolSize" || e.Column.Name == "colPilHolRest"
                    //|| e.Column.Name == "colNoOpenPill" || e.Column.Name == "colCGBrattInst"
                    //|| e.Column.Name == "colCGBrattEffct" || e.Column.Name == "colVelTrough"
                    //|| e.Column.Name == "colTempCGBratt" || e.Column.Name == "colGasInstrAvl"
                    //e.Column.Name == "colInstrumentNo" || e.Column.Name == "colGDICond"
                    //|| e.Column.Name == "colGDIKnw" || e.Column.Name == "colDesIncorpVent"
                    //|| e.Column.Name == "colVentLayout" || e.Column.Name == "colGeoStructure"
                    //|| e.Column.Name == "colVentAvailable" || e.Column.Name == "colVentCond"
                    //|| e.Column.Name == "colVentKnowInstr" || e.Column.Name == "colKnowEscRoute"
                    //|| e.Column.Name == "colGenVentKnow" || e.Column.Name == "colPnlLeadLeg"
                    //)
                    //|| e.Column.Name == "colBotContFace") // || e.Column.Name == "gcWP25"
                    //|| e.Column.Name == "gcWP25" || e.Column.Name == "gcWP25"
                /*|| e.Column.Name == "gcWP25" ||*/ e.Column.Name == "gcWP25")
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

        private void AddActBtn_Click(object sender, EventArgs e)
        {
            //VentilationActionForm ActFrm = new VentilationActionForm();
            //ActFrm._theSystemDBTag = ProductionRes.systemDBTag;
            //ActFrm._UserCurrentInfo = _UserCurrentInfo;
            //ActFrm.lblWPDesc.Text = WP1Desc;
            //ActFrm.lblSection.Text = tbSection2.EditValue.ToString();
            //ActFrm.CurrentDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);


            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            //ActFrm._theSystemDBTag = ProductionRes.systemDBTag;
            ActFrm._UserCurrentInfo = _UserCurrentInfo;
            ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbProdMonth2.EditValue));
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
            ActFrm.txtSection.EditValue = tbSection2.EditValue;
            ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
            ActFrm.WPComboEdit.Items.Add(WP1Desc);
            ActFrm.txtWorkplace.EditValue = WP1Desc;
            ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            ActFrm.txtWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

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
            //if (WP21Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP21Desc);
            //}
            //if (WP22Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP22Desc);
            //}
            //if (WP23Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP23Desc);
            //}
            //if (WP24Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP24Desc);
            //}
            //if (WP25Desc != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(WP25Desc);
            //}

            ActFrm.Item = "Vent Schedule Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        private void gvRockEng_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            string Activity = gvRockEng.GetRowCellValue(e.RowHandle, "Activity").ToString();

            //Stop Control(Numeric/Text)
            if (e.Column.Name == "colTopContDistFace" || e.Column.Name == "colBotContFace" || e.Column.Name == "colPilHolDistFace" || e.Column.Name == "colPilHolSize"
                    || e.Column.Name == "colNoOpenPill" || e.Column.Name == "colVelTrough" || e.Column.Name == "colTempCGBratt"
                    || e.Column.Name == "colPilHolRest" || e.Column.Name == "NoOfOpenPillarHoling"
                    || e.Column.Name == "colVentContCond")
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
                        info.ContextImage = imgFlags.Images[2];
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
        }

        private void gvRockEng_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {


        }

        private void gvRockEng_ShownEditor(object sender, EventArgs e)
        {

        }

        string docalcparam = "Y";

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "WBBotData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "WBTopData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            //Face Temp Dry Bulb
            if (e.Column.Name == "colDBbott")
            {

                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBBotData")) >= Convert.ToDecimal(35.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBBotData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            //Face Temp Dry Bulb
            if (e.Column.Name == "colDBTop")
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBTopData")) >= Convert.ToDecimal(35.0))
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "DBTopData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                        ActFrm.txtSection.EditValue = tbSection2.EditValue;
                        ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                        ActFrm.btnClose.Enabled = false;
                        ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceBotData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);

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
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                        ActFrm.txtSection.EditValue = tbSection2.EditValue;
                        ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                        ActFrm.btnClose.Enabled = false;
                        ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceBotData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);

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
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                        ActFrm.txtSection.EditValue = tbSection2.EditValue;
                        ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                        ActFrm.btnClose.Enabled = false;
                        ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceTopData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);

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
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                        ActFrm.txtSection.EditValue = tbSection2.EditValue;
                        ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                        ActFrm.btnClose.Enabled = false;
                        ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "ContToFaceTopData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "VentContConditionData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString(); ;
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "AUIData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "NoiseData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "HPDData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                        ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                        ActFrm.txtSection.EditValue = tbSection2.EditValue;
                        ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                        ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                        ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                        ActFrm.btnClose.Enabled = false;
                        ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                        ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                        ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "PillarHolingsRestrictedData").ToString();
                        ActFrm.Type = "VSA";
                        ActFrm.ShowDialog(this);

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "NoOfOpenPillarHolingData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "GasIntrAvailableData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Description"]).ToString();
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, gvRockEng.Columns["Workplaceid"]).ToString();
                    ActFrm.Item = gvRockEng.FocusedColumn.Caption.ToString();
                    ActFrm.AnswerLbl.Text = gvRockEng.GetRowCellValue(gvRockEng.FocusedRowHandle, "PanelLeadLegData").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            //// calc co
            //if (docalcparam != "N")
            //    docalc();
            //docalcparam = "Y";

            LoadActions();
        }

        void docalc()
        {
            gvGeneralInfo.PostEditor();
            gvRockEng.PostEditor();

            double Result, Number1, Number2;

            double wetbulbwp1 = 0;
            double Drybulbwp1 = 0;
            double Velocitywp1 = 0;
            double SWwp1 = 0;
            string isLedge = "No";

            double BarPresswp1 = 0;
            double AvailableQTYwp1 = 0;
            double NoofSidesMinedwp1 = 0;

            decimal HumidityCalc1 = 0;
            decimal HumidityCalc2 = 0;
            decimal HumidityCalc3 = 0;
            decimal HumidityCalc4 = 0;
            decimal HumidityAnswer = 0;

            decimal PotVel = 0;
            decimal AUI = 0;
            decimal WorkPerc = 0;

            if (docalcparam == "Y")
            {
                docalcparam = "N";

                // wp1
                try
                {
                    #region WP1Calc
                    if (gvRockEng.GetRowCellValue(3, "WP1").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP1"));
                    if (gvRockEng.GetRowCellValue(5, "WP1").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP1"));
                    if (gvRockEng.GetRowCellValue(7, "WP1").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP1"));
                    if (gvRockEng.GetRowCellValue(9, "WP1").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP1"));
                    if (gvRockEng.GetRowCellValue(2, "WP1").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP1").ToString();

                    //General


                    string aa = gvGeneralInfo.GetRowCellValue(17, "Answer").ToString();
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP1", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP1", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP1", "A");
                    }

                    string _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    string _sqlOrder = "Col1 desc";

                    string ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP1", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP1", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP1", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP1", HumidityAnswer);
                    }

                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP1", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP1", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP1", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP1", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP1", "A");

                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP2Calc
                    if (gvRockEng.GetRowCellValue(3, "WP2").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP2"));
                    if (gvRockEng.GetRowCellValue(5, "WP2").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP2"));
                    if (gvRockEng.GetRowCellValue(7, "WP2").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP2"));
                    if (gvRockEng.GetRowCellValue(9, "WP2").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP2"));
                    if (gvRockEng.GetRowCellValue(2, "WP2").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP2").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP2", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP2", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP2", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP2", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP2", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP2", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP2", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP2", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round( Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)) ,1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP2", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP2", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP2", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP2", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP3Calc
                    if (gvRockEng.GetRowCellValue(3, "WP3").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP3"));
                    if (gvRockEng.GetRowCellValue(5, "WP3").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP3"));
                    if (gvRockEng.GetRowCellValue(7, "WP3").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP3"));
                    if (gvRockEng.GetRowCellValue(9, "WP3").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP3"));
                    if (gvRockEng.GetRowCellValue(2, "WP3").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP3").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP3", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP3", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP3", "A");
                    }


                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP3", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP3", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP3", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP3", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP3", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round( Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)) ,1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP3", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP3", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP3", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP3", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP4Calc
                    if (gvRockEng.GetRowCellValue(3, "WP4").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP4"));
                    if (gvRockEng.GetRowCellValue(5, "WP4").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP4"));
                    if (gvRockEng.GetRowCellValue(7, "WP4").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP4"));
                    if (gvRockEng.GetRowCellValue(9, "WP4").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP4"));
                    if (gvRockEng.GetRowCellValue(2, "WP4").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP4").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP4", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP4", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP4", "A");
                    }


                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP4", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP4", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP4", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP4", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP4", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round( Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)) ,1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP4", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP4", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP4", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP4", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP5Calc
                    if (gvRockEng.GetRowCellValue(3, "WP5").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP5"));
                    if (gvRockEng.GetRowCellValue(5, "WP5").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP5"));
                    if (gvRockEng.GetRowCellValue(7, "WP5").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP5"));
                    if (gvRockEng.GetRowCellValue(9, "WP5").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP5"));
                    if (gvRockEng.GetRowCellValue(2, "WP5").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP5").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP5", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP5", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP5", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP5", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP5", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP5", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP5", HumidityAnswer);
                    }

                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP5", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round( Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)) ,1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP5", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP5", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP5", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP5", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP6Calc
                    if (gvRockEng.GetRowCellValue(3, "WP6").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP6"));
                    if (gvRockEng.GetRowCellValue(5, "WP6").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP6"));
                    if (gvRockEng.GetRowCellValue(7, "WP6").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP6"));
                    if (gvRockEng.GetRowCellValue(9, "WP6").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP6"));
                    if (gvRockEng.GetRowCellValue(2, "WP6").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP6").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));



                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP6", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP6", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP6", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP6", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP6", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP6", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP6", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP6", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round( Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)) ,1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP6", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP6", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP6", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP6", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP7Calc
                    if (gvRockEng.GetRowCellValue(3, "WP7").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP7"));
                    if (gvRockEng.GetRowCellValue(5, "WP7").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP7"));
                    if (gvRockEng.GetRowCellValue(7, "WP7").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP7"));
                    if (gvRockEng.GetRowCellValue(9, "WP7").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP7"));
                    if (gvRockEng.GetRowCellValue(2, "WP7").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP7").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP7", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP7", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP7", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP7", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP7", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP7", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP7", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP7", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round( Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)) ,1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP7", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP7", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP7", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP7", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP8Calc
                    if (gvRockEng.GetRowCellValue(3, "WP8").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP8"));
                    if (gvRockEng.GetRowCellValue(5, "WP8").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP8"));
                    if (gvRockEng.GetRowCellValue(7, "WP8").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP8"));
                    if (gvRockEng.GetRowCellValue(9, "WP8").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP8"));
                    if (gvRockEng.GetRowCellValue(2, "WP8").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP8").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP8", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP8", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP8", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP8", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP8", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP8", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP8", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP8", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round( Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)) ,1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP8", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP8", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP8", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP8", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP9Calc
                    if (gvRockEng.GetRowCellValue(3, "WP9").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP9"));
                    if (gvRockEng.GetRowCellValue(5, "WP9").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP9"));
                    if (gvRockEng.GetRowCellValue(7, "WP9").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP9"));
                    if (gvRockEng.GetRowCellValue(9, "WP9").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP9"));
                    if (gvRockEng.GetRowCellValue(2, "WP9").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP9").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP9", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP9", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP9", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP9", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP9", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP9", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP9", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP9", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round( Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)) ,1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP9", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP9", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP9", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP9", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP10Calc
                    if (gvRockEng.GetRowCellValue(3, "WP10").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP10"));
                    if (gvRockEng.GetRowCellValue(5, "WP10").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP10"));
                    if (gvRockEng.GetRowCellValue(7, "WP10").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP10"));
                    if (gvRockEng.GetRowCellValue(9, "WP10").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP10"));
                    if (gvRockEng.GetRowCellValue(2, "WP10").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP10").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP10", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP10", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP10", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP10", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP10", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP10", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP10", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP10", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round( Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)) ,1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP10", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP10", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP10", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP10", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP11Calc
                    if (gvRockEng.GetRowCellValue(3, "WP11").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP11"));
                    if (gvRockEng.GetRowCellValue(5, "WP11").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP11"));
                    if (gvRockEng.GetRowCellValue(7, "WP11").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP11"));
                    if (gvRockEng.GetRowCellValue(9, "WP11").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP11"));
                    if (gvRockEng.GetRowCellValue(2, "WP11").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP11").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP11", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP11", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP11", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP11", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP11", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP11", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP11", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP11", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP11", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP11", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP11", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP11", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP12Calc
                    if (gvRockEng.GetRowCellValue(3, "WP12").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP12"));
                    if (gvRockEng.GetRowCellValue(5, "WP12").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP12"));
                    if (gvRockEng.GetRowCellValue(7, "WP12").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP12"));
                    if (gvRockEng.GetRowCellValue(9, "WP12").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP12"));
                    if (gvRockEng.GetRowCellValue(2, "WP12").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP12").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP12", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP12", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP12", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP12", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP12", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP12", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP12", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP12", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP12", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP12", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP12", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP12", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;


                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP13Calc
                    if (gvRockEng.GetRowCellValue(3, "WP13").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP13"));
                    if (gvRockEng.GetRowCellValue(5, "WP13").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP13"));
                    if (gvRockEng.GetRowCellValue(7, "WP13").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP13"));
                    if (gvRockEng.GetRowCellValue(9, "WP13").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP13"));
                    if (gvRockEng.GetRowCellValue(2, "WP13").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP13").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP13", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP13", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP13", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP13", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP13", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP13", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP13", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP13", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP13", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP13", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP13", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP13", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP14Calc
                    if (gvRockEng.GetRowCellValue(3, "WP14").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP14"));
                    if (gvRockEng.GetRowCellValue(5, "WP14").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP14"));
                    if (gvRockEng.GetRowCellValue(7, "WP14").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP14"));
                    if (gvRockEng.GetRowCellValue(9, "WP14").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP14"));
                    if (gvRockEng.GetRowCellValue(2, "WP14").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP14").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP14", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP14", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP14", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP14", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP14", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP14", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP14", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP14", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP14", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP14", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP14", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP14", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP15Calc
                    if (gvRockEng.GetRowCellValue(3, "WP15").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP15"));
                    if (gvRockEng.GetRowCellValue(5, "WP15").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP15"));
                    if (gvRockEng.GetRowCellValue(7, "WP15").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP15"));
                    if (gvRockEng.GetRowCellValue(9, "WP15").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP15"));
                    if (gvRockEng.GetRowCellValue(2, "WP15").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP15").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP15", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP15", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP15", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP15", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP15", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP15", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP15", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP15", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP15", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP15", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP15", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP15", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP16Calc
                    if (gvRockEng.GetRowCellValue(3, "WP16").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP16"));
                    if (gvRockEng.GetRowCellValue(5, "WP16").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP16"));
                    if (gvRockEng.GetRowCellValue(7, "WP16").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP16"));
                    if (gvRockEng.GetRowCellValue(9, "WP16").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP16"));
                    if (gvRockEng.GetRowCellValue(2, "WP16").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP16").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP16", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP16", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP16", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP16", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP16", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP16", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP16", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP16", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP16", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP16", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP16", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP16", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP17Calc
                    if (gvRockEng.GetRowCellValue(3, "WP17").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP17"));
                    if (gvRockEng.GetRowCellValue(5, "WP17").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP17"));
                    if (gvRockEng.GetRowCellValue(7, "WP17").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP17"));
                    if (gvRockEng.GetRowCellValue(9, "WP17").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP17"));
                    if (gvRockEng.GetRowCellValue(2, "WP17").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP17").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP17", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP17", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP17", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP17", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP17", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP17", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP17", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP17", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP17", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP17", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP17", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP17", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP18Calc
                    if (gvRockEng.GetRowCellValue(3, "WP18").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP18"));
                    if (gvRockEng.GetRowCellValue(5, "WP18").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP18"));
                    if (gvRockEng.GetRowCellValue(7, "WP18").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP18"));
                    if (gvRockEng.GetRowCellValue(9, "WP18").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP18"));
                    if (gvRockEng.GetRowCellValue(2, "WP18").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP18").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP18", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP18", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP18", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP18", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP18", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP18", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP18", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP18", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP18", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP18", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP18", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP18", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP19Calc
                    if (gvRockEng.GetRowCellValue(3, "WP19").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP19"));
                    if (gvRockEng.GetRowCellValue(5, "WP19").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP19"));
                    if (gvRockEng.GetRowCellValue(7, "WP19").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP19"));
                    if (gvRockEng.GetRowCellValue(9, "WP19").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP19"));
                    if (gvRockEng.GetRowCellValue(2, "WP19").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP19").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP19", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP19", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP19", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP19", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP19", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP19", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP19", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP19", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP19", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP19", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP19", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP19", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP20Calc
                    if (gvRockEng.GetRowCellValue(3, "WP20").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP20"));
                    if (gvRockEng.GetRowCellValue(5, "WP20").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP20"));
                    if (gvRockEng.GetRowCellValue(7, "WP20").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP20"));
                    if (gvRockEng.GetRowCellValue(9, "WP20").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP20"));
                    if (gvRockEng.GetRowCellValue(2, "WP20").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP20").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP20", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP20", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP20", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP20", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP20", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP20", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP20", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP20", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP20", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP20", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP20", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP20", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP21Calc
                    if (gvRockEng.GetRowCellValue(3, "WP21").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP21"));
                    if (gvRockEng.GetRowCellValue(5, "WP21").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP21"));
                    if (gvRockEng.GetRowCellValue(7, "WP21").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP21"));
                    if (gvRockEng.GetRowCellValue(9, "WP21").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP21"));
                    if (gvRockEng.GetRowCellValue(2, "WP21").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP21").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    //  double Result, Number1, Number2;
                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP21", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP21", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP21", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP21", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP21", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP21", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP21", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP21", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP21", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP21", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP21", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP21", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP22Calc
                    if (gvRockEng.GetRowCellValue(3, "WP22").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP22"));
                    if (gvRockEng.GetRowCellValue(5, "WP22").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP22"));
                    if (gvRockEng.GetRowCellValue(7, "WP22").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP22"));
                    if (gvRockEng.GetRowCellValue(9, "WP22").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP22"));
                    if (gvRockEng.GetRowCellValue(2, "WP22").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP22").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP22", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP22", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP22", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP22", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP22", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP22", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP22", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP22", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP22", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP22", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP22", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP22", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP23Calc
                    if (gvRockEng.GetRowCellValue(3, "WP23").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP23"));
                    if (gvRockEng.GetRowCellValue(5, "WP23").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP23"));
                    if (gvRockEng.GetRowCellValue(7, "WP23").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP23"));
                    if (gvRockEng.GetRowCellValue(9, "WP23").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP23"));
                    if (gvRockEng.GetRowCellValue(2, "WP23").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP23").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP23", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP23", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP23", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP23", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP23", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP23", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP23", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP23", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            // AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP23", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP23", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP23", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP23", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP24Calc
                    if (gvRockEng.GetRowCellValue(3, "WP24").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP24"));
                    if (gvRockEng.GetRowCellValue(5, "WP24").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP24"));
                    if (gvRockEng.GetRowCellValue(7, "WP24").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP24"));
                    if (gvRockEng.GetRowCellValue(9, "WP24").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP24"));
                    if (gvRockEng.GetRowCellValue(2, "WP24").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP24").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP24", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP24", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP24", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP24", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP24", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP24", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP24", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP24", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP24", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP24", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP24", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP24", "A");


                    #endregion

                    wetbulbwp1 = 0;
                    Drybulbwp1 = 0;
                    Velocitywp1 = 0;
                    SWwp1 = 0;
                    isLedge = "No";

                    BarPresswp1 = 0;
                    AvailableQTYwp1 = 0;
                    NoofSidesMinedwp1 = 0;

                    HumidityCalc1 = 0;
                    HumidityCalc2 = 0;
                    HumidityCalc3 = 0;
                    HumidityCalc4 = 0;
                    HumidityAnswer = 0;

                    PotVel = 0;
                    AUI = 0;
                    WorkPerc = 0;

                    #region WP25Calc
                    if (gvRockEng.GetRowCellValue(3, "WP25").ToString() != string.Empty)
                        wetbulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(3, "WP25"));
                    if (gvRockEng.GetRowCellValue(5, "WP25").ToString() != string.Empty)
                        Drybulbwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(5, "WP25"));
                    if (gvRockEng.GetRowCellValue(7, "WP25").ToString() != string.Empty)
                        Velocitywp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(7, "WP25"));
                    if (gvRockEng.GetRowCellValue(9, "WP25").ToString() != string.Empty)
                        SWwp1 = Convert.ToDouble(gvRockEng.GetRowCellValue(9, "WP25"));
                    if (gvRockEng.GetRowCellValue(2, "WP25").ToString() != string.Empty)
                        isLedge = gvRockEng.GetRowCellValue(2, "WP25").ToString();

                    //General
                    if (gvGeneralInfo.GetRowCellValue(17, "Answer").ToString() != string.Empty)
                        BarPresswp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(17, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(0, "Answer").ToString() != string.Empty)
                        AvailableQTYwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(0, "Answer"));
                    if (gvGeneralInfo.GetRowCellValue(18, "Answer").ToString() != string.Empty)
                        NoofSidesMinedwp1 = Convert.ToDouble(gvGeneralInfo.GetRowCellValue(18, "Answer"));

                    Number1 = 0;
                    Number2 = 0;

                    Number1 = Velocitywp1;
                    Number2 = 0.5;
                    Result = Math.Pow(Number1, Number2);

                    Result = (36.5 - wetbulbwp1) * (0.7 + Result);

                    if (wetbulbwp1 > 0)
                    {
                        gvRockEng.SetRowCellValue(12, "WP25", Math.Round(Result, 1));

                        gvRockEng.SetRowCellValue(13, "WP25", string.Empty);
                        if (Result <= 6)
                            gvRockEng.SetRowCellValue(13, "WP25", "A");
                    }

                    _sqlWhere = "Col1 <= '" + wetbulbwp1 + "' ";
                    _sqlOrder = "Col1 desc";

                    ValSCP = string.Empty;

                    if (wetbulbwp1 >= Convert.ToDouble(20.4))
                    {
                        DtSCPFilter = DtSCP.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

                        int colnumber = 2;

                        colnumber = Convert.ToInt32(Math.Round(Velocitywp1 * 10, 0));

                        if (colnumber <= 2)
                            colnumber = 2;

                        if (colnumber >= 21)
                            colnumber = 20;
                        if (colnumber >= 24)
                            colnumber = 22;
                        if (colnumber >= 26)
                            colnumber = 23;
                        if (colnumber >= 28)
                            colnumber = 24;
                        if (colnumber >= 30)
                            colnumber = 25;
                        if (colnumber >= 35)
                            colnumber = 26;
                        if (colnumber >= 40)
                            colnumber = 27;
                        if (colnumber >= 45)
                            colnumber = 28;

                        ValSCP = DtSCPFilter.Rows[0][colnumber].ToString();

                        gvRockEng.SetRowCellValue(10, "WP25", ValSCP);

                        gvRockEng.SetRowCellValue(11, "WP25", string.Empty);
                        if (Convert.ToInt32(ValSCP) <= 180)
                            gvRockEng.SetRowCellValue(11, "WP25", "A");
                    }

                    HumidityCalc1 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * wetbulbwp1) / (Convert.ToDouble(237.3) + wetbulbwp1)));
                    HumidityCalc2 = Convert.ToDecimal(Convert.ToDouble(0.000644) * (BarPresswp1 * (Drybulbwp1 - wetbulbwp1)));
                    HumidityCalc3 = HumidityCalc1 - HumidityCalc2;
                    HumidityCalc4 = Convert.ToDecimal(Convert.ToDouble(0.6105) * Math.Exp((Convert.ToDouble(17.27) * Drybulbwp1) / (Convert.ToDouble(237.3) + Drybulbwp1)));
                    HumidityAnswer = Math.Round((HumidityCalc3 / HumidityCalc4) * 100, 1);

                    if (wetbulbwp1 != 0 && Drybulbwp1 != 0)
                    {
                        gvRockEng.SetRowCellValue(14, "WP25", HumidityAnswer);
                    }


                    PotVel = 0;
                    if (SWwp1 > 0 && NoofSidesMinedwp1 > 0)
                    {
                        PotVel = Math.Round(Convert.ToDecimal(AvailableQTYwp1 / (Convert.ToDouble(0.8) * 9 * SWwp1 * NoofSidesMinedwp1)), 2);
                    }

                    gvRockEng.SetRowCellValue(15, "WP25", PotVel);

                    AUI = 0;
                    if (isLedge == "Yes")
                    {
                        AUI = 80;
                    }
                    else
                    {
                        if (PotVel != 0)
                        {
                            AUI = Math.Round((Convert.ToDecimal(Velocitywp1) / PotVel) * 100, 1);
                            //AUI = Math.Round(Convert.ToDecimal(Velocitywp1 / (Convert.ToDouble(PotVel) * 100)), 1);
                        }
                    }
                    gvRockEng.SetRowCellValue(16, "WP25", AUI);

                    WorkPerc = 0;
                    if (ValSCP != string.Empty)
                    {
                        WorkPerc = Math.Round(Convert.ToDecimal((Convert.ToDecimal(ValSCP) / 240) * 100), 1);
                    }

                    gvRockEng.SetRowCellValue(18, "WP25", WorkPerc);

                    gvRockEng.SetRowCellValue(19, "WP25", string.Empty);
                    if (Convert.ToDecimal(WorkPerc) <= 75 && wetbulbwp1 > 0)
                        gvRockEng.SetRowCellValue(19, "WP25", "A");


                    #endregion


                    //int wpcount = 0;
                    //Decimal ToPotVol = 0;
                    //Decimal ToAui = 0;
                    //Decimal ToWB = 0;

                    //string WP = "";

                    //for (int x = 1; x <= 25; x++)
                    //{
                    //    WP = "WP" + x.ToString();

                    //    if (gvRockEng.GetRowCellValue(3, WP).ToString() != "")
                    //    {
                    //        if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, WP).ToString()) > 0)
                    //        {
                    //            wpcount = wpcount + 1;
                    //        }

                    //    }

                    //    if (gvRockEng.GetRowCellValue(15, WP).ToString() != "")
                    //    {

                    //        ToPotVol = ToPotVol + Convert.ToDecimal(gvRockEng.GetRowCellValue(15, WP).ToString());

                    //    }

                    //    if (gvRockEng.GetRowCellValue(16, WP).ToString() != "")
                    //    {

                    //        ToAui = ToAui + Convert.ToDecimal(gvRockEng.GetRowCellValue(16, WP).ToString());

                    //    }

                    //  //  if (gvRockEng.GetRowCellValue(16, WP).ToString() != "")
                    // //   {

                    ////        ToWB = ToWB + Convert.ToDecimal(gvRockEng.GetRowCellValue(5, WP).ToString());

                    //   // }

                    //}

                    //if (wpcount <= 0)
                    //    wpcount = 1;

                    //string AvgVolRow15 = Math.Round((ToPotVol / wpcount), 2).ToString();

                    //string AvgAuiRow16 = Math.Round((ToAui / wpcount), 2).ToString();


                    //labelPotValAvg.Text = Math.Round((ToPotVol / wpcount), 2).ToString();

                    //gvGeneralInfo.SetRowCellValue(15, gvGeneralInfo.Columns["Answer"], Math.Round((ToPotVol / wpcount), 2).ToString());

                    ///Air Utilization
                    ///
                    //int wpcountAui = 0;
                    //Decimal ToAui = 0;

                    //decimal scsrPerc = 0;

                    //string WPaui = "";

                    //for (int x = 1; x <= 25; x++)
                    //{
                    //    WPaui = "WP" + x.ToString();

                    //    if (gvRockEng.GetRowCellValue(2, WPaui).ToString() != "")
                    //    {
                    //        wpcountAui = wpcountAui + 1;
                    //    }

                    //    if (gvRockEng.GetRowCellValue(16, WPaui).ToString() != "")
                    //    {

                    //        ToAui = ToAui + Convert.ToDecimal(gvRockEng.GetRowCellValue(16, WPaui).ToString());

                    //    }

                    //}

                    //if (wpcountAui <= 0)
                    //    wpcountAui = 1;

                    //AvgAuiRow16 = Math.Round((ToAui / wpcountAui), 2).ToString();
                    // gvGeneralInfo.SetRowCellValue(1, gvGeneralInfo.Columns["Answer"], Math.Round((ToAui / wpcountAui), 2).ToString());

                    //decimal scsrPerc = 0;
                    /////SCSR Compliance
                    /////
                    //if (gvGeneralInfo.GetRowCellValue(5, gvGeneralInfo.Columns["Answer"]).ToString() != "" && gvGeneralInfo.GetRowCellValue(8, gvGeneralInfo.Columns["Answer"]).ToString() != "")
                    //{
                    //    scsrPerc = (Convert.ToDecimal(gvGeneralInfo.GetRowCellValue(5, gvGeneralInfo.Columns["Answer"])) 
                    //        / (Convert.ToDecimal(gvGeneralInfo.GetRowCellValue(8, gvGeneralInfo.Columns["Answer"]))) * 100);

                    //    gvGeneralInfo.SetRowCellValue(21, gvGeneralInfo.Columns["Answer"], Math.Round(scsrPerc, 2).ToString());
                    //}


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

            decimal CountActionsWP21 = 0;
            decimal CountActionsWP22 = 0;
            decimal CountActionsWP23 = 0;
            decimal CountActionsWP24 = 0;
            decimal CountActionsWP25 = 0;

            for (int rows = 0; rows < gvRockEng.RowCount; rows++)
            {
                if (rows == 4 || rows == 6 || rows == 8 || rows == 11 || rows == 13 || rows == 17
                    || rows == 19 || rows == 23 || rows == 26)
                {
                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP1"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP1"]).ToString() == "B")
                    {
                        CountActionsWP1 = CountActionsWP1 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP2"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP2"]).ToString() == "B")
                    {
                        CountActionsWP2 = CountActionsWP2 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP3"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP3"]).ToString() == "B")
                    {
                        CountActionsWP3 = CountActionsWP3 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP4"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP4"]).ToString() == "B")
                    {
                        CountActionsWP4 = CountActionsWP4 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP5"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP5"]).ToString() == "B")
                    {
                        CountActionsWP5 = CountActionsWP5 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP6"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP6"]).ToString() == "B")
                    {
                        CountActionsWP6 = CountActionsWP6 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP7"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP7"]).ToString() == "B")
                    {
                        CountActionsWP7 = CountActionsWP7 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP8"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP8"]).ToString() == "B")
                    {
                        CountActionsWP8 = CountActionsWP8 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP9"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP9"]).ToString() == "B")
                    {
                        CountActionsWP9 = CountActionsWP9 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP10"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP10"]).ToString() == "B")
                    {
                        CountActionsWP10 = CountActionsWP10 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP11"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP11"]).ToString() == "B")
                    {
                        CountActionsWP11 = CountActionsWP11 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP12"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP12"]).ToString() == "B")
                    {
                        CountActionsWP12 = CountActionsWP12 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP13"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP13"]).ToString() == "B")
                    {
                        CountActionsWP13 = CountActionsWP13 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP14"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP14"]).ToString() == "B")
                    {
                        CountActionsWP14 = CountActionsWP14 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP15"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP15"]).ToString() == "B")
                    {
                        CountActionsWP15 = CountActionsWP15 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP16"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP16"]).ToString() == "B")
                    {
                        CountActionsWP16 = CountActionsWP16 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP17"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP17"]).ToString() == "B")
                    {
                        CountActionsWP17 = CountActionsWP17 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP18"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP18"]).ToString() == "B")
                    {
                        CountActionsWP18 = CountActionsWP18 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP19"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP19"]).ToString() == "B")
                    {
                        CountActionsWP19 = CountActionsWP19 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP20"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP20"]).ToString() == "B")
                    {
                        CountActionsWP20 = CountActionsWP20 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP21"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP21"]).ToString() == "B")
                    {
                        CountActionsWP21 = CountActionsWP21 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP22"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP22"]).ToString() == "B")
                    {
                        CountActionsWP22 = CountActionsWP22 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP23"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP23"]).ToString() == "B")
                    {
                        CountActionsWP23 = CountActionsWP23 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP24"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP24"]).ToString() == "B")
                    {
                        CountActionsWP24 = CountActionsWP24 + 1;
                    }

                    if (gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP25"]).ToString() == "A" || gvRockEng.GetRowCellValue(rows, gvRockEng.Columns["WP25"]).ToString() == "B")
                    {
                        CountActionsWP25 = CountActionsWP25 + 1;
                    }
                }
            }

            decimal Wp1Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP1 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP1"], Wp1Comp);

            decimal Wp2Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP2 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP2"], Wp2Comp);

            decimal Wp3Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP3 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP3"], Wp3Comp);

            decimal Wp4Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP4 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP4"], Wp4Comp);

            decimal Wp5Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP5 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP5"], Wp5Comp);

            decimal Wp6Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP6 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP6"], Wp6Comp);

            decimal Wp7Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP7 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP7"], Wp7Comp);

            decimal Wp8Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP8 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP8"], Wp8Comp);

            decimal Wp9Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP9 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP9"], Wp9Comp);

            decimal Wp10Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP10 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP10"], Wp10Comp);

            decimal Wp11Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP11 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP11"], Wp11Comp);

            decimal Wp12Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP12 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP12"], Wp12Comp);

            decimal Wp13Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP13 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP13"], Wp13Comp);

            decimal Wp14Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP14 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP14"], Wp14Comp);

            decimal Wp15Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP15 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP15"], Wp15Comp);

            decimal Wp16Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP16 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP16"], Wp16Comp);

            decimal Wp17Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP17 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP17"], Wp17Comp);

            decimal Wp18Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP18 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP18"], Wp18Comp);

            decimal Wp19Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP19 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP19"], Wp19Comp);

            decimal Wp20Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP20 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP20"], Wp20Comp);

            decimal Wp21Comp = Math.Round(Convert.ToDecimal((100 - ((CountActionsWP21 / 17) * 100))), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP21"], Wp21Comp);

            decimal Wp22Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP22 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP22"], Wp22Comp);

            decimal Wp23Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP23 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP23"], Wp23Comp);

            decimal Wp24Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP24 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP24"], Wp24Comp);

            decimal Wp25Comp = Math.Round(Convert.ToDecimal(100 - ((CountActionsWP25 / 17) * 100)), 1);
            gvRockEng.SetRowCellValue(0, gvRockEng.Columns["WP25"], Wp25Comp);

            CalcAvg();
        }


        private void CalcAvg()
        {
            int countWP = 1;

            if (WP1 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP1"]).ToString()) > 0)
                    countWP = 1;
            }

            if (WP2 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP2"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP3 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP3"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP4 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP4"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP5 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP5"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP6 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP6"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP7 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP7"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP8 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP8"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP9 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP9"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP10 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP10"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP11 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP11"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP12 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP13"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP13 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP13"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP14 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP14"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP15 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP15"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP16 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP16"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP17 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP17"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP18 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP18"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP19 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP19"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP20 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP20"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP21 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP21"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP22 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP22"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP23 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP23"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP24 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP24"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            if (WP25 != string.Empty && gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
            {
                if (Convert.ToDecimal(gvRockEng.GetRowCellValue(3, gvRockEng.Columns["WP25"]).ToString()) > 0)
                    countWP = countWP + 1;
            }

            decimal wbAvg = 0;
            decimal DbAvg = 0;
            decimal VelAvg = 0;
            decimal SWAvg = 0;
            decimal SCPAvg = 0;
            decimal KataAvg = 0;
            decimal HumidityAvg = 0;
            decimal WorkPerAvg = 0;
            decimal AUIAvg = 0;
            decimal PotVelAvg = 0;
            decimal StrikeCtrlAvg = 0;

            for (int row = 0; row < gvRockEng.RowCount; row++)
            {
                string QuestID = gvRockEng.GetRowCellValue(row, gvRockEng.Columns["QuestID"]).ToString();

                if (WP1 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP1"]));




                }

                if (WP2 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP2"]));


                }

                if (WP3 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP3"]));


                }

                if (WP4 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP4"]));


                }

                if (WP5 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP5"]));


                }

                if (WP6 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP6"]));


                }

                if (WP7 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP7"]));


                }

                if (WP8 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP8"]));


                }

                if (WP9 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP9"]));


                }

                if (WP10 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP10"]));


                }

                if (WP11 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP11"]));


                }

                if (WP12 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP12"]));


                }

                if (WP13 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP13"]));


                }

                if (WP14 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP14"]));


                }

                if (WP15 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP15"]));


                }

                if (WP16 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP16"]));


                }

                if (WP17 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP17"]));

                }

                if (WP18 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP18"]));

                }

                if (WP19 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP19"]));


                }

                if (WP20 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP20"]));


                }

                if (WP21 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP21"]));


                }

                if (WP22 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP22"]));

                }

                if (WP23 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP23"]));

                }

                if (WP24 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP24"]));


                }

                if (WP25 != string.Empty)
                {
                    if (QuestID == "70" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        wbAvg = wbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "72" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        DbAvg = DbAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "74" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        VelAvg = VelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "8" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        SWAvg = SWAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "77" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        SCPAvg = SCPAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "10" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        KataAvg = KataAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "81" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        HumidityAvg = HumidityAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "85" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        WorkPerAvg = WorkPerAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "13" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        AUIAvg = AUIAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "12" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        PotVelAvg = PotVelAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));

                    if (QuestID == "88" && gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]).ToString() != string.Empty)
                        StrikeCtrlAvg = StrikeCtrlAvg + Convert.ToDecimal(gvRockEng.GetRowCellValue(row, gvRockEng.Columns["WP25"]));


                }
            }

            wbAvg = wbAvg / countWP;
            DbAvg = DbAvg / countWP;
            VelAvg = VelAvg / countWP;
            SWAvg = SWAvg / countWP;
            SCPAvg = SCPAvg / countWP;
            KataAvg = KataAvg / countWP;
            HumidityAvg = HumidityAvg / countWP;
            WorkPerAvg = WorkPerAvg / countWP;
            AUIAvg = AUIAvg / countWP;
            PotVelAvg = PotVelAvg / countWP;
            StrikeCtrlAvg = StrikeCtrlAvg / countWP;

            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(27, gvGeneralInfo.Columns["Answer"], Math.Round(wbAvg, 1));
            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(28, gvGeneralInfo.Columns["Answer"], Math.Round(DbAvg, 1));
            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(29, gvGeneralInfo.Columns["Answer"], Math.Round(VelAvg, 1));
            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(30, gvGeneralInfo.Columns["Answer"], Math.Round(SWAvg, 1));
            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(31, gvGeneralInfo.Columns["Answer"], Math.Round(SCPAvg, 0));
            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(23, gvGeneralInfo.Columns["Answer"], Math.Round(HumidityAvg, 1));
            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(24, gvGeneralInfo.Columns["Answer"], Math.Round(WorkPerAvg, 1));
            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(25, gvGeneralInfo.Columns["Answer"], Math.Round(AUIAvg, 1));

            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(15, gvGeneralInfo.Columns["Answer"], Math.Round(PotVelAvg, 1));

            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(1, gvGeneralInfo.Columns["Answer"], Math.Round(AUIAvg, 1));

            docalcparam = "N";
            gvGeneralInfo.SetRowCellValue(26, gvGeneralInfo.Columns["Answer"], Math.Round(StrikeCtrlAvg, 1));



            decimal scsrPerc = 0;
            ///SCSR Compliance
            ///
            if (gvGeneralInfo.GetRowCellValue(5, gvGeneralInfo.Columns["Answer"]).ToString() != string.Empty && gvGeneralInfo.GetRowCellValue(8, gvGeneralInfo.Columns["Answer"]).ToString() != string.Empty)
            {
                scsrPerc = (Convert.ToDecimal(gvGeneralInfo.GetRowCellValue(5, gvGeneralInfo.Columns["Answer"]))
                    / (Convert.ToDecimal(gvGeneralInfo.GetRowCellValue(8, gvGeneralInfo.Columns["Answer"]))) * 100);

                gvGeneralInfo.SetRowCellValue(21, gvGeneralInfo.Columns["Answer"], Math.Round(scsrPerc, 2).ToString());
            }

        }

        private void gvGeneralInfo_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
           
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
                        //e.Appearance.BackColor = Color.LightYellow;
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
                        //e.Appearance.BackColor = Color.LightYellow;
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


        private void gvPerSectionWinch1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //Number
            //if (gvRefugeBay.GetRowCellValue(e.RowHandle, "ValueType").ToString() == string.Empty)
            //{
            //    if (e.Column.FieldName == "Answer")
            //    {
            //        e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //        e.Appearance.BackColor = Color.Transparent;
            //    }
            //}
        }

        private void gvPerSectionWinch1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            ////Number
            //if (gvRefugeBay.GetRowCellValue(e.RowHandle, "ValueType").ToString() == string.Empty)
            //{
            //    if (e.Column.FieldName == "Answer")
            //    {
            //        //if (e.Column.Caption == "Answer")
            //        //{
            //        e.RepositoryItem = repRefUnit;
            //        //}
            //    }
            //}
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVentChecklistReport report = new frmVentChecklistReport();
            report.theSystemDBTag = _theSystemDBTag;
            report.UserCurrentInfo = _UserCurrentInfo;
            report.monthDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
            report.section = tbSection2.EditValue.ToString();
            report.prodMonth = dbl_rec_ProdMonth;
            report.workPlace = WP1;
            report.crew = tbCrew2.EditValue.ToString();
            report.Frmtype = "Stoping";
            report.StartPosition = FormStartPosition.CenterScreen;
            report.PicPath = txtAttachment.Text;
            report._totScore = _totScore;
            report._totWeight = _totWeight;
            report._percentage = _percentage;
            report.Authorise = Auth;
            //report.fieldBook = txtFeilBook.Text;
            //report.pageNum = txtPageNum.Text;
            //report.ObsName = txtObserverName.Text;

            report.Show();

        }

        private void gvPerSectionWinch_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

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
                string test = mianDicrectory + "\\" + tbSection2.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue) + DocsLB.SelectedItem.ToString();

                System.Diagnostics.Process.Start(mianDicrectory + "\\" + tbSection2.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue) + DocsLB.SelectedItem.ToString());
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
                LoadActions();
                LoadFeildBook();
                frmFirstLoad = "N";
            }
            
        }

        private void gvRockEng_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //string Rowhandle = gvRockEng.GetRowCellValue(e.RowHandle, gvRockEng.Columns["Question"]).ToString();

            //string Test = string.Empty;
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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = WP1Desc;
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = WP1;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.Item = gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Answer").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

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
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;
                    ActFrm.txtSection.EditValue = tbSection2.EditValue;
                    ActFrm.txtWorkplace.EditValue = WP1Desc;
                    ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString()));
                    ActFrm.ReqDate.Value = Convert.ToDateTime(tbDpInspecDate2.EditValue.ToString());
                    ActFrm.btnClose.Enabled = false;
                    ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ActFrm.WPID = dbl_rec_WPID.Text;
                    ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
                    ActFrm.Item = gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Question").ToString();
                    ActFrm.AnswerLbl.Text = gvGeneralInfo.GetRowCellValue(gvGeneralInfo.FocusedRowHandle, "Answer").ToString();
                    ActFrm.Type = "VSA";
                    ActFrm.ShowDialog(this);

                    docalcparam = "N";
                }
            }

            LoadActions();

            //docalcparam = "Y";


        }

        private void ofdOpenDocFile_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string Docs = openFileDialog1.FileName;

            int indexa = Docs.LastIndexOf("\\");

            string sourcefilename = Docs.Substring(indexa + 1, (Docs.Length - indexa) - 1);

            DocsLB.Items.Add(sourcefilename);
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

        private void labelMainGroupChange_Click(object sender, EventArgs e)
        {
            // docalc();
        }

        private void labelMainGroupChange_TextChanged(object sender, EventArgs e)
        {
            docalc();
        }

        private void PicBox_Click(object sender, EventArgs e)
        {

        }

        private void tbDpInspecDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveBtn_ItemClick();

            SaveGeneralInfo();

            SaveAvailableTemp();

            SaveRefugeBay();

            saveFeildBook();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ofdOpenImageFile.InitialDirectory = folderBrowserDialog1.SelectedPath;
            ofdOpenImageFile.FileName = null;
            ofdOpenImageFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            result1 = ofdOpenImageFile.ShowDialog();

            GetFile();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fileResults = openFileDialog1.ShowDialog();
            GetDoc();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVentChecklistReport report = new frmVentChecklistReport();
            report.theSystemDBTag = _theSystemDBTag;
            report.UserCurrentInfo = _UserCurrentInfo;
            report.monthDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue);
            report.section = tbSection2.EditValue.ToString();
            report.prodMonth = dbl_rec_ProdMonth;
            report.workPlace = WP1;
            report.crew = tbCrew2.EditValue.ToString();
            report.Frmtype = "Stoping";
            report.StartPosition = FormStartPosition.CenterScreen;
            report.PicPath = txtAttachment.Text;
            report._totScore = _totScore;
            report._totWeight = _totWeight;
            report._percentage = _percentage;
            report.Authorise = Auth;
            //report.fieldBook = txtFeilBook.Text;
            //report.pageNum = txtPageNum.Text;
            //report.ObsName = txtObserverName.Text;

            report.Show();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmInspectionFireRating frmFireRating = new frmInspectionFireRating();
            frmFireRating._theSystemDBTag = _theSystemDBTag;
            frmFireRating._UserCurrentInfo = _UserCurrentInfo;
            frmFireRating._prodMonth = tbProdMonth2.EditValue.ToString();
            frmFireRating.tbDpInspecDate.EditValue = tbDpInspecDate2.EditValue;
            frmFireRating._section = tbSection2.EditValue.ToString();
            frmFireRating._crew = tbCrew2.EditValue.ToString();
            frmFireRating._workPlace = WP1Desc;//tbSection2.EditValue.ToString().Substring(0, 4); ;
            frmFireRating.ShowDialog(this);

            _totScore = frmFireRating.lblTotalVal.Text;
            _totWeight = frmFireRating.lblTotWeight.Text;
            _percentage = frmFireRating.lblPercentage.Text;
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbAuth = new MWDataManager.clsDataAccess();
            _dbAuth.SqlStatement = "delete from tbl_Dept_Inspection_VentAuthorise where Activity = 0 and \r\n "
                + " CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue) + "' and SectionID = '" + tbSection2.EditValue.ToString() + "' \r\n ";

            _dbAuth.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAuth.queryReturnType = MWDataManager.ReturnType.DataTable;

            if (btnAuthWP.Caption == "Authorise")
            {
                _dbAuth.SqlStatement += " insert into tbl_Dept_Inspection_VentAuthorise (Activity, CalendarDate, SectionID) \r\n "
                                + " values(0, '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate2.EditValue) + "', '" + tbSection2.EditValue.ToString() + "')";

            }

            var uathResult = _dbAuth.ExecuteInstruction();
            CHeckAuth();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
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

        private void gcRockEng_PaintEx(object sender, DevExpress.XtraGrid.PaintExEventArgs e)
        {
            //using (Font font = new Font("Times New Roman", 50, FontStyle.Bold, GraphicsUnit.Pixel))
            //{
            //	Point point1 = new Point(300, 200);
            //	TextRenderer.DrawText(e.Cache, "Top", font, point1, Color.Black);
            //}
        }
    }
}