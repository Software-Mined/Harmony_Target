using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
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

namespace Mineware.Systems.Production.Planning
{
    public partial class ucGraphicsPreplanning : BaseUserControl
    {
        StringBuilder SQLQuery = new StringBuilder();
        DialogResult Result;

        //public static List<Models.Questions> questionsData { get; set; }

        public ucGraphicsPreplanning()
        {
            InitializeComponent();
        }

        //Use this for Syncromine
        private void sqlConnector(string sqlQuery, string sqlTableIdentifier)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
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

        //NOTE: Register all tables for sqlconnector to use
        private void tableRegister()
        {
            dsGlobal.Tables.Add(dtWorkplaceData);
            dsGlobal.Tables.Add(dtActions);
            dsGlobal.Tables.Add(dtData);
            dsGlobal.Tables.Add(dtAuth);
            dsGlobal.Tables.Add(dtDataEdit);
            dsGlobal.Tables.Add(dtDataEdit2);
            dsGlobal.Tables.Add(dtCycleData);
            dsGlobal.Tables.Add(dtEngEquipt);
            dsGlobal.Tables.Add(dtQuestions);
            dsGlobal.Tables.Add(dtSubQuestions);
            dsGlobal.Tables.Add(dtSave);
        }

        #region Datatables
        private DataTable dtWorkplaceData = new DataTable("dtWorkplaceData");
        private DataTable dtActions = new DataTable("dtActions");
        private DataTable dtData = new DataTable("dtData");
        private DataTable dtAuth = new DataTable("dtAuth");
        private DataTable dtDataEdit = new DataTable("dtDataEdit");
        private DataTable dtDataEdit2 = new DataTable("dtDataEdit2");
        private DataTable dtCycleData = new DataTable("dtCycleData");
        private DataTable dtSubQuestion = new DataTable("dtSubQuestion");
        private DataTable dtEngEquipt = new DataTable("dtEngEquipt");
        private DataTable dtQuestions = new DataTable("dtQuestions");
        private DataTable dtSubQuestions = new DataTable("dtSubQuestions");
        private DataTable dtSave = new DataTable("dtSave");
        private BindingSource bsMainQuestions = new BindingSource();
        private BindingSource bsAnswers = new BindingSource();
        #endregion

        #region DataSet
        public DataSet dsGlobal = new DataSet();
        #endregion

        #region Variables
        public String _FormType;
        public String _Crew;
        public String _Prodmonth;
        public string _Activity;

        Boolean frmloaded;
        string Likelihood = string.Empty;
        string Consequence = string.Empty;
        string RiskRating = string.Empty;
        int SelectedQuestID = 0;
        private string AnswersFound = "N";
        int foundIndex = 0;
        string ValueType = string.Empty;
        string ActionType;
        string ActionDescription;
        string CalcIsBusy;

        private String WP1 = string.Empty;
        private String WP2 = string.Empty;
        private String WP3 = string.Empty;
        private String WP4 = string.Empty;
        private String WP5 = string.Empty;
        private String WP1Desc = string.Empty;
        private String WP2Desc = string.Empty;
        private String WP3Desc = string.Empty;
        private String WP4Desc = string.Empty;
        private String WP5Desc = string.Empty;

        private String RepDir = string.Empty;
        private String RepDirDoc = string.Empty;

        private String ID = string.Empty;
        private String Workplace = string.Empty;
        private String Description = string.Empty;
        private String Recomendation = string.Empty;
        private String Priority = string.Empty;
        private String TargetDate = string.Empty;
        private String RespPerson = string.Empty;
        private String Overseer = string.Empty;

        private String SourceFile;
        private String DestinationFile;
        private String FileName = string.Empty;
        #endregion

        private void ucGraphicsPreplanning_Load(object sender, EventArgs e)
        {
            RepDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage;
            RepDirDoc = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage + @"\\Preplanning\\" + _FormType + "\\Documents";
            CalcIsBusy = "N";

            if (_FormType == "RockEng")
            {
                ActionType = "PPRE";
                ActionDescription = "Rock Engineering Actions";
            }

            if (_FormType == "Vent")
            {
                ActionType = "PPVT";
                ActionDescription = "Ventilation Actions";
            }

            if (_FormType == "Survey")
            {
                ActionType = "PPSR";
                ActionDescription = "Survey Actions";
            }

            if (_FormType == "Safety")
            {
                ActionType = "PPS";
                ActionDescription = "Safety Actions";
            }

            if (_FormType == "Geology")
            {
                ActionType = "PPGL";
                ActionDescription = "Geology Actions";
            }

            if (_FormType == "Engineering")
            {
                ActionType = "PPEG";
                ActionDescription = "Engineering Actions";
            }

            tbCrew.EditValue = _Crew;
            tbProdMonth.EditValue = _Prodmonth;

            lcCapture.Text = _FormType + " Capture";

            tableRegister();
            LoadWorkplaces();
            LoadActions();
            LoadRockEngineeringData();
            LoadQuestions();
            LoadSubQuestions();
            //LoadRecommendations();
            LoadCycles();
            LoadNotes();
            LoadAuthorised();
            LoadCalc();
            loadImage();
            loadDocs();
            

            frmloaded = true;

            pnlMain.Visible = true;
        }

        private void LoadRockEngineeringData()
        {
            if (_Activity == "0" || _Activity == "Stoping")
            {
                string sql = string.Empty;
                sql = "EXEC sp_PrePlanning_" + _FormType + "_Questions '" + _Prodmonth + "','" + WP1 + "','" + WP2 + "','" + WP3 + "','" + WP4 + "','" + WP5 + "'";
                sqlConnector(sql, "dtData");
            }

            if (_Activity == "1" || _Activity == "Development")
            {
                string sql = string.Empty;
                sql = "EXEC sp_PrePlanning_" + _FormType + "_Questions_Dev '" + _Prodmonth + "','" + WP1 + "','" + WP2 + "','" + WP3 + "','" + WP4 + "','" + WP5 + "'";
                sqlConnector(sql, "dtData");
            }

            gcCapture.DataSource = null;
            gvCapture.OptionsView.ShowGroupPanel = false;

            if (WP1 == string.Empty)
                gbWP1.Visible = false;
            if (WP2 == string.Empty)
                gbWP2.Visible = false;
            if (WP3 == string.Empty)
                gbWP3.Visible = false;
            if (WP4 == string.Empty)
                gbWP4.Visible = false;
            if (WP5 == string.Empty)
                gbWP5.Visible = false;

            gcCapture.DataSource = dtData;
            gcQuestID.FieldName = "QuestID";
            gcQuest.FieldName = "Question";
            gcCat.FieldName = "QuestionSubCat";
            gcWP1.FieldName = "WP1Answer";
            gcWP2.FieldName = "WP2Answer";
            gcWP3.FieldName = "WP3Answer";
            gcWP4.FieldName = "WP4Answer";
            gcWP5.FieldName = "WP5Answer";
            gcWP1Likelihood.FieldName = "WP1Likelihood";
            gcWP2Likelihood.FieldName = "WP2Likelihood";
            gcWP3Likelihood.FieldName = "WP3Likelihood";
            gcWP4Likelihood.FieldName = "WP4Likelihood";
            gcWP5Likelihood.FieldName = "WP5Likelihood";
            gcWP1Consequence.FieldName = "WP1Consequence";
            gcWP2Consequence.FieldName = "WP2Consequence";
            gcWP3Consequence.FieldName = "WP3Consequence";
            gcWP4Consequence.FieldName = "WP4Consequence";
            gcWP5Consequence.FieldName = "WP5Consequence";
            gcWP1RiskRating.FieldName = "WP1RiskRating";
            gcWP2RiskRating.FieldName = "WP2RiskRating";
            gcWP3RiskRating.FieldName = "WP3RiskRating";
            gcWP4RiskRating.FieldName = "WP4RiskRating";
            gcWP5RiskRating.FieldName = "WP5RiskRating";
            gcType.FieldName = "ValueType";
        }

        #region Procedures

        //Get Max RiskRating
        private void LoadCalc()
        {
            int Groupcount = 0;

            for (int Grow = 0; Grow < gvCapture.RowCount; Grow++)
            {
                if (gvCapture.IsDataRow(Grow))
                {
                    Groupcount = Groupcount + 1;
                }
            }

            string WP = string.Empty;
            string WPAnswer = string.Empty;
            int calcTotalMeas = 0;
            int calcMaxMeas = 0;
            int calcTotalPlan = 0;
            int calcMaxPlan = 0;

            for (int col = 3; col < gvCapture.Columns.Count; col++)
            {
                WP = string.Empty;
                if (col == 5)
                {
                    WP = "WP1RiskRating";
                    WPAnswer = "WP1Answer";
                }

                if (col == 9)
                {
                    WP = "WP2RiskRating";
                    WPAnswer = "WP2Answer";
                }

                if (col == 13)
                {
                    WP = "WP3RiskRating";
                    WPAnswer = "WP3Answer";
                }

                if (col == 17)
                {
                    WP = "WP4RiskRating";
                    WPAnswer = "WP4Answer";
                }

                if (col == 21)
                {
                    WP = "WP5RiskRating";
                    WPAnswer = "WP5Answer";
                }

                calcTotalMeas = 0;
                calcTotalPlan = 0;

                if (WP != string.Empty)
                {
                    for (int row = 0; row < gvCapture.RowCount - (gvCapture.RowCount - Groupcount); row++)
                    {
                        string currvalue = gvCapture.GetRowCellValue(row, gvCapture.Columns[col]).ToString();
                        if (!string.IsNullOrEmpty(currvalue))
                        {
                            if (gvCapture.GetRowCellValue(row, gvCapture.Columns["QuestionSubCat"]).ToString() == "MEASURING MONTH")
                            {
                                calcTotalMeas = calcTotalMeas + Convert.ToInt32(gvCapture.GetRowCellValue(row, gvCapture.Columns[WP]).ToString());
                                if (Convert.ToInt32(gvCapture.GetRowCellValue(row, gvCapture.Columns[WP]).ToString()) > calcMaxMeas)
                                {
                                    calcMaxMeas = Convert.ToInt32(gvCapture.GetRowCellValue(row, gvCapture.Columns[WP]).ToString());
                                }
                            }

                            if (gvCapture.GetRowCellValue(row, gvCapture.Columns["QuestionSubCat"]).ToString() == "PLANNING MONTH")
                            {
                                calcTotalPlan = calcTotalPlan + Convert.ToInt32(gvCapture.GetRowCellValue(row, gvCapture.Columns[WP]).ToString());
                                if (Convert.ToInt32(gvCapture.GetRowCellValue(row, gvCapture.Columns[WP]).ToString()) > calcMaxPlan)
                                {
                                    calcMaxPlan = Convert.ToInt32(gvCapture.GetRowCellValue(row, gvCapture.Columns[WP]).ToString());
                                }
                            }
                        }

                        CalcIsBusy = "Y";

                        gvCapture.SetRowCellValue(0, gvCapture.Columns[WP], calcMaxMeas.ToString());
                        gvCapture.SetRowCellValue(1, gvCapture.Columns[WP], calcTotalMeas.ToString());
                        gvCapture.SetRowCellValue(2, gvCapture.Columns[WP], calcMaxPlan.ToString());
                        gvCapture.SetRowCellValue(3, gvCapture.Columns[WP], calcTotalPlan.ToString());

                        gvCapture.SetRowCellValue(0, gvCapture.Columns[WPAnswer], calcMaxMeas.ToString());
                        gvCapture.SetRowCellValue(1, gvCapture.Columns[WPAnswer], calcTotalMeas.ToString());
                        gvCapture.SetRowCellValue(2, gvCapture.Columns[WPAnswer], calcMaxPlan.ToString());
                        gvCapture.SetRowCellValue(3, gvCapture.Columns[WPAnswer], calcTotalPlan.ToString());

                        CalcIsBusy = "N";
                    }
                }
            }
        }

        //Get Questions
        private void LoadQuestions()
        {
            string sql = string.Empty;
            sql = "Select * from tbl_preplanning_" + _FormType + "Quest";
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
            sql = "Select * from tbl_preplanning_subquest_" + _FormType + string.Empty;
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


        private void LoadRecommendations()
        {
            string sql = string.Empty;
            sql = "Select * from tbl_preplanning_subquest_" + _FormType + string.Empty;
            sqlConnector(sql, "dtSubQuestions");
            bsSubQuestions.DataSource = dtSubQuestions;
            bsAnswers.DataSource = dtSubQuestions;

            repRockEngSubQuestEdit.DataSource = bsAnswers;
            repEngMainQuest.ForceInitialize();
            repEngMainQuest.PopulateColumns();
            repEngMainQuest.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Recommendation", "Recommendation", 100));
            repRockEngSubQuestEdit.DisplayMember = "Recommendation";
            repRockEngSubQuestEdit.ValueMember = "Recommendation";
        }

        //Gets Workplaces
        private void LoadWorkplaces()
        {
            string sql = string.Empty;

            sql = "SELECT pm.Workplaceid, w.Description, ''LastInspection " +
                    "FROM tbl_PLANMONTH AS pm, " +
                    "tbl_Workplace w \r\n" +
                    "WHERE w.WORKPLACEID = pm.Workplaceid " +
                    "AND pm.Prodmonth = " + _Prodmonth + " " +
                    "AND substring( pm.OrgUnitDS+'                ',1,12) = '" + _Crew + "' \r\n" +
                    " union " +
                    "SELECT pm.Workplaceid, w.Description, ''LastInspection " +
                    "FROM tbl_PLANMONTH_MOScrutiny AS pm, " +
                    "tbl_Workplace w \r\n" +
                    "WHERE w.WORKPLACEID = pm.Workplaceid " +
                    "AND pm.Prodmonth = " + _Prodmonth + " " +
                    "AND substring( pm.OrgUnitDS+'                ',1,12) = '" + _Crew + "' \r\n" +
                    " and pm.Workplaceid not in ( \r\n" +
                     "SELECT pm.Workplaceid " +
                    "FROM tbl_PLANMONTH AS pm, " +
                    "tbl_Workplace w \r\n" +
                    "WHERE w.WORKPLACEID = pm.Workplaceid " +
                    "AND pm.Prodmonth = " + _Prodmonth + " " +
                    "AND substring( pm.OrgUnitDS+'                ',1,12) = '" + _Crew + "' \r\n" +
                     " ) \r\n" +

                    "ORDER BY w.DESCRIPTION ";

            sqlConnector(sql, "dtWorkplaceData");


            // do 
            int x = 0;

            gcWP1.Visible = false;
            gcWP2.Visible = false;
            gcWP3.Visible = false;
            gcWP4.Visible = false;
            gcWP5.Visible = false;

            gcWP1Likelihood.Visible = false;
            gcWP1Consequence.Visible = false;
            gcWP1RiskRating.Visible = false;
            gcWP2Likelihood.Visible = false;
            gcWP2Consequence.Visible = false;
            gcWP2RiskRating.Visible = false;
            gcWP3Likelihood.Visible = false;
            gcWP3Consequence.Visible = false;
            gcWP3RiskRating.Visible = false;
            gcWP4Likelihood.Visible = false;
            gcWP4Consequence.Visible = false;
            gcWP4RiskRating.Visible = false;
            gcWP5Likelihood.Visible = false;
            gcWP5Consequence.Visible = false;
            gcWP5RiskRating.Visible = false;

            foreach (DataRow r in dtWorkplaceData.Rows)
            {
                if (x == 0)
                {
                    gcWP1.Caption = r["Description"].ToString();
                    gcWP1.Visible = true;
                    gcWP1Likelihood.Visible = true;
                    gcWP1Consequence.Visible = true;
                    gcWP1RiskRating.Visible = true;
                    WP1 = r["Workplaceid"].ToString();
                    WP1Desc = r["Description"].ToString();
                }

                if (x == 1)
                {
                    gcWP2.Caption = r["Description"].ToString();
                    gcWP2.Visible = true;
                    gcWP2Likelihood.Visible = true;
                    gcWP2Consequence.Visible = true;
                    gcWP2RiskRating.Visible = true;
                    WP2 = r["Workplaceid"].ToString();
                    WP2Desc = r["Description"].ToString();
                }

                if (x == 2)
                {
                    gcWP3.Caption = r["Description"].ToString();
                    gcWP3.Visible = true;
                    gcWP3Likelihood.Visible = true;
                    gcWP3Consequence.Visible = true;
                    gcWP3RiskRating.Visible = true;
                    WP3 = r["Workplaceid"].ToString();
                    WP3Desc = r["Description"].ToString();
                }

                if (x == 3)
                {
                    gcWP4.Caption = r["Description"].ToString();
                    gcWP4.Visible = true;
                    gcWP4Likelihood.Visible = true;
                    gcWP4Consequence.Visible = true;
                    gcWP4RiskRating.Visible = true;
                    WP4 = r["Workplaceid"].ToString();
                    WP4Desc = r["Description"].ToString();
                }

                if (x == 4)
                {
                    gcWP5.Caption = r["Description"].ToString();
                    gcWP5.Visible = true;
                    gcWP5Likelihood.Visible = true;
                    gcWP5Consequence.Visible = true;
                    gcWP5RiskRating.Visible = true;
                    WP5 = r["Workplaceid"].ToString();
                    WP5Desc = r["Description"].ToString();
                }
                x = x + 1;
            }
        }

        //Load Cycle
        void LoadCycles()
        {
            //Declarations
            string sql = string.Empty;

            sql = " Select * from vw_Preplanning_Cycle where orgunit = '" + _Crew + "' and pm = " + _Prodmonth + " " +
                                  " order by OrderNum,description ";

            gcCycle.DataSource = null;

            sqlConnector(sql, "dtCycleData");

            gcCycle.DataSource = dtCycleData;

            colWorkplace.FieldName = "description";
            colFL.FieldName = "FL";
            col1.FieldName = "day1";
            col2.FieldName = "day2";
            col3.FieldName = "day3";
            col4.FieldName = "day4";
            col5.FieldName = "day5";
            col6.FieldName = "day6";
            col7.FieldName = "day7";
            col8.FieldName = "day8";
            col9.FieldName = "day9";
            col10.FieldName = "day10";

            col11.FieldName = "day11";
            col12.FieldName = "day12";
            col13.FieldName = "day13";
            col14.FieldName = "day14";
            col15.FieldName = "day15";
            col16.FieldName = "day16";
            col17.FieldName = "day17";
            col18.FieldName = "day18";
            col19.FieldName = "day19";
            col20.FieldName = "day20";

            col21.FieldName = "day21";
            col22.FieldName = "day22";
            col23.FieldName = "day23";
            col24.FieldName = "day24";
            col25.FieldName = "day25";
            col26.FieldName = "day26";
            col27.FieldName = "day27";
            col28.FieldName = "day28";
            col29.FieldName = "day29";
            col30.FieldName = "day30";

            col31.FieldName = "day31";
            col32.FieldName = "day32";
            col33.FieldName = "day33";
            col34.FieldName = "day34";
            col35.FieldName = "day35";
            col36.FieldName = "day36";
            col37.FieldName = "day37";
            col38.FieldName = "day38";
            col39.FieldName = "day39";
            col40.FieldName = "day40";

            if (dtCycleData.Rows.Count > 0)
            {
                DateTime startdate = Convert.ToDateTime(dtCycleData.Rows[0]["BeginDate"].ToString());
                int columnIndex = 2;

                //Headers Date
                for (int i = 0; i < 40; i++)
                {
                    string test = gvCycle.Columns[columnIndex].Caption;


                    gvCycle.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM ddd");

                    startdate = startdate.AddDays(1);
                    columnIndex++;
                }
            }


            for (int i = 0; i < gvCycle.RowCount; i++)
            {
                string val = gvCycle.GetRowCellValue(i, gvCycle.Columns["day20"]).ToString();

                foreach (DataRow item in dtCycleData.Rows)
                {
                    if (val == string.Empty)
                    {
                        col20.Visible = false;
                        col21.Visible = false;
                        col22.Visible = false;
                        col23.Visible = false;
                        col24.Visible = false;
                        col25.Visible = false;
                        col26.Visible = false;
                        col27.Visible = false;
                        col28.Visible = false;
                        col29.Visible = false;

                        col30.Visible = false;
                        col31.Visible = false;
                        col32.Visible = false;
                        col33.Visible = false;
                        col34.Visible = false;
                        col35.Visible = false;
                        col36.Visible = false;
                        col37.Visible = false;
                        col38.Visible = false;
                        col39.Visible = false;
                        col40.Visible = false;
                    }
                }

            }

            foreach (DataRow dr in dtCycleData.Rows)
            {
                if (dr["workplaceid"].ToString() == WP1)
                {

                    gvCapture.SetRowCellValue(24, gvCapture.Columns["WP1"], dr["InspectionDate"].ToString());
                    gvCapture.SetRowCellValue(26, gvCapture.Columns["WP1"], dr["SW"].ToString());
                    gvCapture.SetRowCellValue(27, gvCapture.Columns["WP1"], dr["CW"].ToString());
                }
                if (dr["workplaceid"].ToString() == WP2)
                {
                    gvCapture.SetRowCellValue(26, gvCapture.Columns["WP2"], dr["SW"].ToString());
                    gvCapture.SetRowCellValue(27, gvCapture.Columns["WP2"], dr["CW"].ToString());
                }
                if (dr["workplaceid"].ToString() == WP3)
                {
                    gvCapture.SetRowCellValue(26, gvCapture.Columns["WP3"], dr["SW"].ToString());
                    gvCapture.SetRowCellValue(27, gvCapture.Columns["WP3"], dr["CW"].ToString());
                }
                if (dr["workplaceid"].ToString() == WP4)
                {
                    gvCapture.SetRowCellValue(26, gvCapture.Columns["WP4"], dr["SW"].ToString());
                    gvCapture.SetRowCellValue(27, gvCapture.Columns["WP4"], dr["CW"].ToString());
                }
                if (dr["workplaceid"].ToString() == WP5)
                {
                    gvCapture.SetRowCellValue(26, gvCapture.Columns["WP5"], dr["SW"].ToString());
                    gvCapture.SetRowCellValue(27, gvCapture.Columns["WP5"], dr["CW"].ToString());
                }
            }

            for (int i = 0; i < gvCycle.RowCount; i++)
            {
                int val1 = Convert.ToInt32(dtCycleData.Rows[0]["TotalShifts"].ToString());



                for (int j = val1 + 3; j < 43; j++)
                {
                    gvCycle.Columns[j].Visible = false;
                }
            }

        }

        //Gets Actions
        private void LoadActions()
        {
            //Declarations
            string sql = string.Empty;

            sql = "EXEC sp_PrePlanning_" + _FormType + "_LoadActions '" + WP1Desc + "','" + WP2Desc + "','" + WP3Desc + "','" + WP4Desc + "','" + WP5Desc + "'";
            sqlConnector(sql, "dtActions");

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
        }

        //Load Authorised
        private void LoadAuthorised()
        {
            //Declarations
            string sql = string.Empty;

            sql = "  Select Max(" + _FormType + "DepAuth) DepAuth from tbl_PrePlanning_MonthPlan \r\n" +
                                   "  where Prodmonth = " + _Prodmonth + " \r\n" +
                                   "  and substring(crew,1,15) = '" + _Crew + "'";
            sqlConnector(sql, "dtAuth");

            if (dtAuth.Rows[0][0].ToString() != string.Empty && dtAuth.Rows.Count > 0)
            {
                btnImageRemove.Enabled = false;
                btnDocRemove.Enabled = false;

                btnSave.Enabled = false;
                btnAddImage.Enabled = false;
                btnAddDocument.Enabled = false;

                AddActBtn.Enabled = false;
                EditActBtn.Enabled = false;
                DelActBtn.Enabled = false;

                gcQuest.OptionsColumn.AllowEdit = false;
                gcWP1.OptionsColumn.AllowEdit = false;
                gcWP2.OptionsColumn.AllowEdit = false;
                gcWP3.OptionsColumn.AllowEdit = false;
                gcWP4.OptionsColumn.AllowEdit = false;
                gcWP5.OptionsColumn.AllowEdit = false;
                btnAuth.Caption = "Unauthorise";
            }
        }

        //Gets Notes
        private void LoadNotes()
        {
            StringBuilder SQL = new StringBuilder();

            MWDataManager.clsDataAccess _Note = new MWDataManager.clsDataAccess();
            _Note.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _Note.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _Note.queryReturnType = MWDataManager.ReturnType.DataTable;
            _Note.SqlStatement = "select Notes from [dbo].[tbl_PrePlanning_" + _FormType + "_Notes] where prodmonth = '" + _Prodmonth + "'  and crew = '" + _Crew + "' and notes is not null ";
            _Note.ExecuteInstruction();

            DataTable dtReceive = new DataTable();
            dtReceive = _Note.ResultsDataTable;

            if (dtReceive.Rows.Count > 0)
            {
                txtNotes.Text = _Note.ResultsDataTable.Rows[0][0].ToString();
            }
        }

        //Load Image
        public void loadImage()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\" + _FormType + string.Empty);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(RepDir + "\\Preplanning\\" + _FormType + string.Empty);

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                if (item.ToString() == RepDir + "\\Preplanning\\" + _FormType + string.Empty + "\\" + _Crew.ToString() + _Prodmonth.ToString() + ext)
                {
                    txtAttachment.Text = item.ToString();
                }
            }

            if (txtAttachment.Text != string.Empty)
            {
                using (FileStream stream = new FileStream(txtAttachment.Text, FileMode.Open, FileAccess.Read))
                {
                    PicBox.Image = Image.FromStream(stream);
                    stream.Dispose();
                }
            }
        }

        public void GetFile()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\" + _FormType + string.Empty);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(RepDir + "\\Preplanning\\" + _FormType + string.Empty);

            if (Result == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                SourceFile = openFileDialog3.FileName;

                index = openFileDialog3.SafeFileName.IndexOf(".");

                if (tbCrew.EditValue.ToString() != string.Empty)
                {
                    FileName = tbCrew.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (tbProdMonth.EditValue != string.Empty)
                {
                    FileName = FileName + tbProdMonth.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog3.SafeFileName.Substring(index);

                DestinationFile = RepDir + "\\Preplanning\\" + _FormType + string.Empty + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog3.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            DestinationFile = RepDir + "\\Preplanning\\" + _FormType + string.Empty + "\\" + FileName + Ext;
                        }
                    }

                    try
                    {
                        System.IO.File.Copy(SourceFile, DestinationFile, true);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    System.IO.File.Copy(SourceFile, RepDir + "\\Preplanning\\" + _FormType + string.Empty + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(RepDir);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtAttachment.Text = DestinationFile;
                PicBox.Image = Image.FromFile(openFileDialog3.FileName);
            }
        }

        //Load Documents
        public void loadDocs()
        {

            Random r = new Random();

            string mianDicrectory = RepDirDoc;

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);


            string[] files = System.IO.Directory.GetFiles(mianDicrectory);

            //Do everywhere
            if (DocsLB.InvokeRequired)
                DocsLB.Invoke(new Action(() => DocsLB.Items.Clear()));
            else
                DocsLB.Items.Clear();


            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int Extpos = aa.IndexOf(".");

                string Ext = aa.Substring(Extpos, aa.Length - Extpos);

                int indexa = item.LastIndexOf("\\");

                string SourceFileName = item.Substring(indexa + 1, (item.Length - indexa) - 1);

                int indexprodmonth = SourceFileName.IndexOf(tbProdMonth.EditValue.ToString());

                string SourceFileCheck = SourceFileName.Substring(0, indexprodmonth + 6);

                int NameLength = SourceFileName.Length - SourceFileCheck.Length;

                string Docsname = SourceFileName.Substring(SourceFileCheck.Length, NameLength);


                if (SourceFileCheck == tbCrew.EditValue.ToString() + tbProdMonth.EditValue.ToString())
                {
                    if (DocsLB.InvokeRequired)
                        DocsLB.Invoke(new Action(() => DocsLB.Items.Add(Docsname.ToString())));
                    else
                        DocsLB.Items.Add(Docsname.ToString());

                }
            }

        }

        public void GetDoc()
        {
            string mianDicrectory = RepDirDoc;

            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(mianDicrectory);

            if (Result == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                string SourcefileName = string.Empty;

                int index = 0;

                SourceFile = openFileDialog4.FileName;

                index = openFileDialog4.SafeFileName.IndexOf(".");

                SourcefileName = openFileDialog4.SafeFileName.Substring(0, index);

                if (tbCrew.EditValue.ToString() != string.Empty)
                {
                    FileName = tbCrew.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (tbProdMonth.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + tbProdMonth.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog4.SafeFileName.Substring(index);

                DestinationFile = mianDicrectory + "\\" + FileName + SourcefileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog4.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            DestinationFile = mianDicrectory + "\\" + FileName + Ext;
                        }

                    }

                    try
                    {
                        System.IO.File.Copy(SourceFile, DestinationFile, true);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    System.IO.File.Copy(SourceFile, mianDicrectory + "\\" + FileName + SourcefileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(mianDicrectory);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }
            }
        }

        #endregion

        #region gcCapture
        private void gvCapture_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            frmloaded = true;
            string Recommendation = "";
            if (frmloaded == true && CalcIsBusy == "N")
            {
                if (e.Column.FieldName == "WP1Answer")
                {
                    try
                    {
                        if (Convert.ToDecimal(gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP1RiskRating").ToString()) > Convert.ToDecimal(10))
                        {
                            frmGraphicsPrePlanningActionCapture ActFrm = new frmGraphicsPrePlanningActionCapture();
                            ActFrm._theSystemDBTag = this.theSystemDBTag;
                            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;
                            ActFrm.tbSections.Text = tbSection.EditValue.ToString();
                            ActFrm.tbPM.Text = tbProdMonth.EditValue.ToString();
                            ActFrm.tbWorkplace.Text = WP1Desc;
                            ActFrm.WPID = WP1;                          
                            
                            /////////Recomendation
                            if (dtSubQuestions.Rows.Count > 0)
                            {
                                string Answer = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP1Answer"]).ToString();// gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP1").ToString();
                                
                                foreach (DataRow o in dtSubQuestions.Select("Question = '"+ Answer + "' and QuestID = '" + SelectedQuestID.ToString() + "'"))
                                {
                                    Recommendation = o["Recommendation"].ToString();
                                    ActFrm.ActionTxt.Text = Recommendation;
                                }
                            }
                            /////////////
                               
                            ActFrm.Item = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "Question").ToString();
                            ActFrm.AnswerLbl.Text = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP1RiskRating").ToString();
                            ActFrm.WPEdit2.Visible = false;
                            ActFrm.tbWorkplace.Visible = true;
                            ActFrm.Type = ActionType;
                            ActFrm.StartPosition = FormStartPosition.CenterScreen;
                            ActFrm.ShowDialog(this);

                            LoadActions();
                        }
                    }
                    catch { }
                }

                if (e.Column.FieldName == "WP2Answer")
                {
                    try
                    {
                        if (Convert.ToDecimal(gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP2RiskRating").ToString()) > Convert.ToDecimal(10))
                        {
                            frmGraphicsPrePlanningActionCapture ActFrm = new frmGraphicsPrePlanningActionCapture();
                            ActFrm._theSystemDBTag = this.theSystemDBTag;
                            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;
                            ActFrm.tbSections.Text = tbSection.EditValue.ToString();
                            ActFrm.tbPM.Text = tbProdMonth.EditValue.ToString();
                            ActFrm.tbWorkplace.Text = WP1Desc;
                            ActFrm.WPID = WP1;
                            /////////Recomendation
                            if (dtSubQuestions.Rows.Count > 0)
                            {
                                string Answer = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP2Answer"]).ToString();// gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP1").ToString();

                                foreach (DataRow o in dtSubQuestions.Select("Question = '" + Answer + "' and QuestID = '" + SelectedQuestID.ToString() + "'"))
                                {
                                    Recommendation = o["Recommendation"].ToString();
                                    ActFrm.ActionTxt.Text = Recommendation;
                                }
                            }
                            /////////////
                            ActFrm.Item = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "Question").ToString();
                            ActFrm.AnswerLbl.Text = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP2RiskRating").ToString();
                            ActFrm.WPEdit2.Visible = false;
                            ActFrm.tbWorkplace.Visible = true;
                            ActFrm.Type = ActionType;
                            ActFrm.StartPosition = FormStartPosition.CenterScreen;
                            ActFrm.ShowDialog(this);

                            LoadActions();
                        }
                    }
                    catch { }
                }

                if (e.Column.FieldName == "WP3Answer")
                {
                    try
                    {
                        if (Convert.ToDecimal(gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP3RiskRating").ToString()) > Convert.ToDecimal(10))
                        {
                            frmGraphicsPrePlanningActionCapture ActFrm = new frmGraphicsPrePlanningActionCapture();
                            ActFrm._theSystemDBTag = this.theSystemDBTag;
                            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;
                            ActFrm.tbSections.Text = tbSection.EditValue.ToString();
                            ActFrm.tbPM.Text = tbProdMonth.EditValue.ToString();
                            ActFrm.tbWorkplace.Text = WP1Desc;
                            ActFrm.WPID = WP1;
                            /////////Recomendation
                            if (dtSubQuestions.Rows.Count > 0)
                            {
                                string Answer = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP3Answer"]).ToString();// gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP1").ToString();

                                foreach (DataRow o in dtSubQuestions.Select("Question = '" + Answer + "' and QuestID = '" + SelectedQuestID.ToString() + "'"))
                                {
                                    Recommendation = o["Recommendation"].ToString();
                                    ActFrm.ActionTxt.Text = Recommendation;
                                }
                            }
                            /////////////
                            ActFrm.Item = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "Question").ToString();
                            ActFrm.AnswerLbl.Text = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP3RiskRating").ToString();
                            ActFrm.WPEdit2.Visible = false;
                            ActFrm.tbWorkplace.Visible = true;
                            ActFrm.Type = ActionType;
                            ActFrm.StartPosition = FormStartPosition.CenterScreen;
                            ActFrm.ShowDialog(this);

                            LoadActions();
                        }
                    }
                    catch { }
                }

                if (e.Column.FieldName == "WP4Answer")
                {
                    try
                    {
                        if (Convert.ToDecimal(gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP4RiskRating").ToString()) > Convert.ToDecimal(10))
                        {
                            frmGraphicsPrePlanningActionCapture ActFrm = new frmGraphicsPrePlanningActionCapture();
                            ActFrm._theSystemDBTag = this.theSystemDBTag;
                            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;
                            ActFrm.tbSections.Text = tbSection.EditValue.ToString();
                            ActFrm.tbPM.Text = tbProdMonth.EditValue.ToString();
                            ActFrm.tbWorkplace.Text = WP1Desc;
                            ActFrm.WPID = WP1;
                            /////////Recomendation
                            if (dtSubQuestions.Rows.Count > 0)
                            {
                                string Answer = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP4Answer"]).ToString();// gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP1").ToString();

                                foreach (DataRow o in dtSubQuestions.Select("Question = '" + Answer + "' and QuestID = '" + SelectedQuestID.ToString() + "'"))
                                {
                                    Recommendation = o["Recommendation"].ToString();
                                    ActFrm.ActionTxt.Text = Recommendation;
                                }
                            }
                            /////////////
                            ActFrm.Item = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "Question").ToString();
                            ActFrm.AnswerLbl.Text = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP4RiskRating").ToString();
                            ActFrm.WPEdit2.Visible = false;
                            ActFrm.tbWorkplace.Visible = true;
                            ActFrm.Type = ActionType;
                            ActFrm.StartPosition = FormStartPosition.CenterScreen;
                            ActFrm.ShowDialog(this);

                            LoadActions();
                        }
                    }
                    catch { }
                }

                if (e.Column.FieldName == "WP5Answer")
                {
                    try
                    {
                        if (Convert.ToDecimal(gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP5RiskRating").ToString()) > Convert.ToDecimal(10))
                        {
                            frmGraphicsPrePlanningActionCapture ActFrm = new frmGraphicsPrePlanningActionCapture();
                            ActFrm._theSystemDBTag = this.theSystemDBTag;
                            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;
                            ActFrm.tbSections.Text = tbSection.EditValue.ToString();
                            ActFrm.tbPM.Text = tbProdMonth.EditValue.ToString();
                            ActFrm.tbWorkplace.Text = WP1Desc;
                            ActFrm.WPID = WP1;
                            /////////Recomendation
                            if (dtSubQuestions.Rows.Count > 0)
                            {
                                string Answer = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP5Answer"]).ToString();// gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP1").ToString();

                                foreach (DataRow o in dtSubQuestions.Select("Question = '" + Answer + "' and QuestID = '" + SelectedQuestID.ToString() + "'"))
                                {
                                    Recommendation = o["Recommendation"].ToString();
                                    ActFrm.ActionTxt.Text = Recommendation;
                                }
                            }
                            /////////////
                            ActFrm.Item = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "Question").ToString();
                            ActFrm.AnswerLbl.Text = gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, "WP5RiskRating").ToString();
                            ActFrm.WPEdit2.Visible = false;
                            ActFrm.tbWorkplace.Visible = true;
                            ActFrm.Type = ActionType;
                            ActFrm.StartPosition = FormStartPosition.CenterScreen;
                            ActFrm.ShowDialog(this);

                            LoadActions();
                        }
                    }
                    catch { }
                }
                LoadCalc();
            }
        }

        private void gvCapture_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            #region PIS
            if (e.RowHandle == 0 && e.Column.FieldName != "Question" && e.Column.FieldName != "WP1RiskRating"
            && e.Column.FieldName != "WP2RiskRating" && e.Column.FieldName != "WP3RiskRating" && e.Column.FieldName != "WP4RiskRating"
            && e.Column.FieldName != "WP5RiskRating")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }
            if (e.RowHandle == 1 && e.Column.FieldName != "Question" && e.Column.FieldName != "WP1RiskRating"
                && e.Column.FieldName != "WP2RiskRating" && e.Column.FieldName != "WP3RiskRating" && e.Column.FieldName != "WP4RiskRating"
                && e.Column.FieldName != "WP5RiskRating")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }
            if (e.RowHandle == 2 && e.Column.FieldName != "Question" && e.Column.FieldName != "WP1RiskRating"
                && e.Column.FieldName != "WP2RiskRating" && e.Column.FieldName != "WP3RiskRating" && e.Column.FieldName != "WP4RiskRating"
                && e.Column.FieldName != "WP5RiskRating")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }
            if (e.RowHandle == 3 && e.Column.FieldName != "Question" && e.Column.FieldName != "WP1RiskRating"
                && e.Column.FieldName != "WP2RiskRating" && e.Column.FieldName != "WP3RiskRating" && e.Column.FieldName != "WP4RiskRating"
                && e.Column.FieldName != "WP5RiskRating")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }
            #endregion

            if (e.Column.FieldName == "WP1Likelihood")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP1Likelihood").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP2Likelihood")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP2Likelihood").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP3Likelihood")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP3Likelihood").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP4Likelihood")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP4Likelihood").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP5Likelihood")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP5Likelihood").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP1Consequence")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP1Consequence").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP2Consequence")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP2Consequence").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP3Consequence")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP3Consequence").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP4Consequence")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP4Consequence").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP5Consequence")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP5Consequence").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "WP1RiskRating")
            {
                if (e.RowHandle != 1 && e.RowHandle != 3 && gvCapture.GetRowCellValue(e.RowHandle, "WP1RiskRating").ToString() != string.Empty)
                {
                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP1RiskRating").ToString()) < Convert.ToDecimal(10))
                    {
                        e.Appearance.BackColor = Color.LightGreen;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP1RiskRating").ToString()) > Convert.ToDecimal(10) &&
                        Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP1RiskRating").ToString()) < Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.Orange;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP1RiskRating").ToString()) > Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.MistyRose;
                    }
                }
            }

            if (e.RowHandle != 1 && e.RowHandle != 3 && e.Column.FieldName == "WP2RiskRating")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP2RiskRating").ToString() != string.Empty)
                {
                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP2RiskRating").ToString()) < Convert.ToDecimal(10))
                    {
                        e.Appearance.BackColor = Color.LightGreen;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP2RiskRating").ToString()) > Convert.ToDecimal(10) &&
                        Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP2RiskRating").ToString()) < Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.Orange;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP2RiskRating").ToString()) > Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.MistyRose;
                    }
                }
            }

            if (e.RowHandle != 1 && e.RowHandle != 3 && e.Column.FieldName == "WP3RiskRating")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP3RiskRating").ToString() != string.Empty)
                {
                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP3RiskRating").ToString()) < Convert.ToDecimal(10))
                    {
                        e.Appearance.BackColor = Color.LightGreen;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP3RiskRating").ToString()) > Convert.ToDecimal(10) &&
                        Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP3RiskRating").ToString()) < Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.Orange;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP3RiskRating").ToString()) > Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.MistyRose;
                    }
                }
            }

            if (e.RowHandle != 1 && e.RowHandle != 3 && e.Column.FieldName == "WP4RiskRating")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP4RiskRating").ToString() != string.Empty)
                {
                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP4RiskRating").ToString()) < Convert.ToDecimal(10))
                    {
                        e.Appearance.BackColor = Color.LightGreen;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP4RiskRating").ToString()) > Convert.ToDecimal(10) &&
                        Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP4RiskRating").ToString()) < Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.Orange;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP4RiskRating").ToString()) > Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.MistyRose;
                    }
                }
            }

            if (e.RowHandle != 1 && e.RowHandle != 3 && e.Column.FieldName == "WP5RiskRating")
            {
                if (gvCapture.GetRowCellValue(e.RowHandle, "WP5RiskRating").ToString() != string.Empty)
                {
                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP5RiskRating").ToString()) < Convert.ToDecimal(10))
                    {
                        e.Appearance.BackColor = Color.LightGreen;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP5RiskRating").ToString()) > Convert.ToDecimal(10) &&
                        Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP5RiskRating").ToString()) < Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.Orange;
                    }

                    if (Convert.ToDecimal(gvCapture.GetRowCellValue(e.RowHandle, "WP5RiskRating").ToString()) > Convert.ToDecimal(20))
                    {
                        e.Appearance.BackColor = Color.MistyRose;
                    }
                }
            }

        }

        private void gvCapture_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                if ((e.Column.Name == "gcWP1Likelihood") || (e.Column.Name == "gcWP1Consequence") || (e.Column.Name == "gcWP1RiskRating") ||
                    (e.Column.Name == "gcWP2Likelihood") || (e.Column.Name == "gcWP2Consequence") || (e.Column.Name == "gcWP2RiskRating") ||
                    (e.Column.Name == "gcWP3Likelihood") || (e.Column.Name == "gcWP3Consequence") || (e.Column.Name == "gcWP3RiskRating") ||
                    (e.Column.Name == "gcWP4Likelihood") || (e.Column.Name == "gcWP4Consequence") || (e.Column.Name == "gcWP4RiskRating") ||
                    (e.Column.Name == "gcWP5Likelihood") || (e.Column.Name == "gcWP5Consequence") || (e.Column.Name == "gcWP5RiskRating"))
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);
                    e.Handled = true;
                }
            }
        }

        private void gvCapture_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            SelectedQuestID = Convert.ToInt32(gvCapture.GetFocusedRowCellValue("QuestID"));

            if (e.RowHandle > -1)
            {
                SelectedQuestID = Convert.ToInt32(gvCapture.GetRowCellValue(e.RowHandle, "QuestID"));
                ValueType = gvCapture.GetRowCellValue(e.RowHandle, "ValueType").ToString();

                if (e.Column.FieldName == "WP1Answer" || e.Column.FieldName == "WP2Answer"
                        || e.Column.FieldName == "WP3Answer" || e.Column.FieldName == "WP4Answer"
                        || e.Column.FieldName == "WP5Answer")
                {
                    if (SelectedQuestID != 0 && ValueType == "DropDownList")
                    {
                        RepositoryItemLookUpEdit editor2 = new RepositoryItemLookUpEdit();
                        editor2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                        editor2.DataSource = dtSubQuestions.DefaultView.Cast<DataRowView>().Where(a => a.Row["QuestID"].ToString() == SelectedQuestID.ToString());
                        editor2.DisplayMember = "Question";
                        editor2.ValueMember = "Question";
                        editor2.PopulateColumns();
                        editor2.Columns[0].Visible = false;
                        editor2.Columns[2].Visible = false;
                        editor2.Columns[3].Visible = false;
                        editor2.Columns[4].Visible = false;
                        editor2.EditValueChanged += repSubQuestEdit_EditValueChanged;
                        e.RepositoryItem = editor2;
                    }

                    if (SelectedQuestID != 0 && ValueType == "DateEdit")
                    {
                        e.RepositoryItem = repDateEdit;
                    }

                    if (SelectedQuestID != 0 && ValueType == "WholeNumber")
                    {
                        e.RepositoryItem = repWholeNum;
                    }

                    if (SelectedQuestID != 0 && ValueType == "1 Decimal")
                    {
                        e.RepositoryItem = repWholeNum;
                    }
                }
            }
        }

        private void gvCapture_RowCellClick(object sender, RowCellClickEventArgs e)
        {

        }

        private void gvCapture_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            if (gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["ValueType"]).ToString() == string.Empty ||
                gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["Question"]).ToString() == "Measuring Month Risk Classification" ||
                gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["Question"]).ToString() == "Measuring Month Total Risk" ||
                gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["Question"]).ToString() == "Planning Month Risk Classification" ||
                gvCapture.GetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["Question"]).ToString() == "Planning Month Total Risk")
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region gcCycle
        private void gvCycle_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.CellValue.ToString().Trim() == "BL")
            {
                e.Appearance.BackColor = Color.MistyRose;
            }

            if (e.CellValue.ToString().Trim() == "SR")
            {
                e.Appearance.BackColor = Color.MistyRose;
            }

            if (e.CellValue.ToString().Trim() == "SUBL")
            {
                e.Appearance.BackColor = Color.MistyRose;
            }

            if (e.CellValue.ToString().Trim() == "OFF")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }
        }
        #endregion

        #region gcAction


        private void gvAction_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            ID = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[0]).ToString();
            Workplace = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[1]).ToString();
            Description = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[2]).ToString();
            Recomendation = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[3]).ToString();
            TargetDate = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[4]).ToString();
            Priority = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[5]).ToString();
            FileName = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[6]).ToString();

            RespPerson = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[9]).ToString();
            Overseer = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[10]).ToString();
        }
        #endregion

        #region Events

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sql = string.Empty;

            int Groupcount = 0;

            for (int Grow = 0; Grow < gvCapture.RowCount; Grow++)
            {
                if (gvCapture.IsDataRow(Grow))
                {
                    Groupcount = Groupcount + 1;
                }
            }

            MWDataManager.clsDataAccess _DataSave = new MWDataManager.clsDataAccess();
            _DataSave.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _DataSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _DataSave.queryReturnType = MWDataManager.ReturnType.DataTable;

            //Use String Builder
            for (int row = 0; row < gvCapture.RowCount - (gvCapture.RowCount - Groupcount); row++)
            {
                if (!gvCapture.IsGroupRow(row))
                {
                    //Declarations
                    string QuestID = gvCapture.GetRowCellValue(row, gvCapture.Columns["QuestID"]).ToString();
                    string WP1Answer = gvCapture.GetRowCellValue(row, gvCapture.Columns["WP1Answer"]).ToString();
                    string WP2Answer = gvCapture.GetRowCellValue(row, gvCapture.Columns["WP2Answer"]).ToString();
                    string WP3Answer = gvCapture.GetRowCellValue(row, gvCapture.Columns["WP3Answer"]).ToString();
                    string WP4Answer = gvCapture.GetRowCellValue(row, gvCapture.Columns["WP4Answer"]).ToString();
                    string WP5Answer = gvCapture.GetRowCellValue(row, gvCapture.Columns["WP5Answer"]).ToString();

                    //WP1
                    sql = sql + " begin try \r\n" +
                          " insert into tbl_PrePlanning_" + _FormType + " (Prodmonth,Section,Crew,Workplace,QuestID,Answer) values( \r\n" +
                          " " + _Prodmonth + ", \r\n" +
                          " '',  \r\n" +
                          " '" + _Crew + "',   \r\n" +
                          " '" + WP1 + "',  \r\n" +
                          " '" + QuestID + "',   \r\n" +
                          " '" + WP1Answer + "'   \r\n" +
                          " ) end try \r\n" +
                          " begin catch \r\n" +
                          " update tbl_PrePlanning_" + _FormType + " \r\n" +
                          " set Answer = '" + WP1Answer + "' \r\n" +
                          " where prodmonth =  " + _Prodmonth + " \r\n" +
                          " and substring(crew+'             ',1,12) = '" + _Crew.ToString() + "' and Workplace = '" + WP1 + "' and QuestID = '" + QuestID + "' \r\n" +
                          " end catch \r\n" +

                          //WP2
                          " begin try \r\n" +
                          " insert into tbl_PrePlanning_" + _FormType + " (Prodmonth,Section,Crew,Workplace,QuestID,Answer)  values( \r\n" +
                          " " + _Prodmonth + ", \r\n" +
                          " '',  \r\n" +
                          " '" + _Crew + "',   \r\n" +
                          " '" + WP2 + "',  \r\n" +
                          " '" + QuestID + "',   \r\n" +
                          " '" + WP2Answer + "'   \r\n" +
                          " ) end try \r\n" +
                          " begin catch \r\n" +
                          " update tbl_PrePlanning_" + _FormType + " \r\n" +
                          " set Answer = '" + WP2Answer + "' \r\n" +
                          " where prodmonth =  " + _Prodmonth + " \r\n" +
                          " and substring(crew+'             ',1,12) = '" + _Crew.ToString() + "' and Workplace = '" + WP2 + "' and QuestID = '" + QuestID + "' \r\n" +
                          " end catch \r\n" +

                          //WP3
                          " begin try \r\n" +
                          " insert into tbl_PrePlanning_" + _FormType + " (Prodmonth,Section,Crew,Workplace,QuestID,Answer)  values( \r\n" +
                          " " + _Prodmonth + ", \r\n" +
                          " '',  \r\n" +
                          " '" + _Crew + "',   \r\n" +
                          " '" + WP3 + "',  \r\n" +
                          " '" + QuestID + "',   \r\n" +
                          " '" + WP3Answer + "'   \r\n" +
                          " ) end try \r\n" +
                          " begin catch \r\n" +
                          " update tbl_PrePlanning_" + _FormType + " \r\n" +
                          " set Answer = '" + WP3Answer + "' \r\n" +
                          " where prodmonth =  " + _Prodmonth + " \r\n" +
                          " and substring(crew+'             ',1,12) = '" + _Crew.ToString() + "' and Workplace = '" + WP3 + "' and QuestID = '" + QuestID + "' \r\n" +
                          " end catch \r\n" +

                          //WP4
                          " begin try \r\n" +
                          " insert into tbl_PrePlanning_" + _FormType + " (Prodmonth,Section,Crew,Workplace,QuestID,Answer)  values( \r\n" +
                          " " + _Prodmonth + ", \r\n" +
                          " '',  \r\n" +
                          " '" + _Crew + "',   \r\n" +
                          " '" + WP4 + "',  \r\n" +
                          " '" + QuestID + "',   \r\n" +
                          " '" + WP4Answer + "'   \r\n" +
                          " ) end try \r\n" +
                          " begin catch \r\n" +
                          " update tbl_PrePlanning_" + _FormType + " \r\n" +
                          " set Answer = '" + WP4Answer + "' \r\n" +
                          " where prodmonth =  " + _Prodmonth + " \r\n" +
                          " and substring(crew+'             ',1,12) = '" + _Crew.ToString() + "' and Workplace = '" + WP4 + "' and QuestID = '" + QuestID + "' \r\n" +
                          " end catch \r\n" +

                          //WP5
                          " begin try \r\n" +
                          " insert into tbl_PrePlanning_" + _FormType + " (Prodmonth,Section,Crew,Workplace,QuestID,Answer)  values( \r\n" +
                          " " + _Prodmonth + ", \r\n" +
                          " '',  \r\n" +
                          " '" + _Crew + "',   \r\n" +
                          " '" + WP5 + "',  \r\n" +
                          " '" + QuestID + "',   \r\n" +
                          " '" + WP5Answer + "'   \r\n" +
                          " ) end try \r\n" +
                          " begin catch \r\n" +
                          " update tbl_PrePlanning_" + _FormType + " \r\n" +
                          " set Answer = '" + WP5Answer + "' \r\n" +
                          " where prodmonth =  " + _Prodmonth + " \r\n" +
                          " and substring(crew+'             ',1,12) = '" + _Crew.ToString() + "' and Workplace = '" + WP5 + "' and QuestID = '" + QuestID + "' \r\n" +
                          " end catch \r\n";
                }
            }

            _DataSave.SqlStatement = sql;

            var result = _DataSave.ExecuteInstruction();
            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
            }

            if (WP1 != string.Empty)
            {
                MWDataManager.clsDataAccess _Notes = new MWDataManager.clsDataAccess();
                _Notes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes.SqlStatement = " insert into tbl_PrePlanning_" + _FormType + "_Notes (Prodmonth,Section,Crew,Workplace,Notes)  values( \r\n" +
                 "  " + _Prodmonth + ", \r\n" +
                 " '',  \r\n" +
                 " '" + _Crew + "',   \r\n" +
                 " '" + WP1 + "',  \r\n" +
                 " '" + txtNotes.Text + "') ";
                _Notes.ExecuteInstruction();

                MWDataManager.clsDataAccess _Notes1 = new MWDataManager.clsDataAccess();
                _Notes1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes1.SqlStatement = " update tbl_PrePlanning_" + _FormType + "_Notes  \r\n" +
                 " set Notes = '" + txtNotes.Text + "' \r\n" +
                 " where prodmonth = '" + _Prodmonth + "' \r\n" +
                 " and crew = '" + _Crew + "' \r\n" +
                 " and workplace = '" + WP1 + "' ";
                _Notes1.ExecuteInstruction();
            }


            if (WP2 != string.Empty)
            {
                MWDataManager.clsDataAccess _Notes = new MWDataManager.clsDataAccess();
                _Notes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes.SqlStatement = " insert into tbl_PrePlanning_" + _FormType + "_Notes (Prodmonth,Section,Crew,Workplace,Notes)  values( \r\n" +
                 "  " + _Prodmonth + ", \r\n" +
                 " '',  \r\n" +
                 " '" + _Crew + "',   \r\n" +
                 " '" + WP2 + "',  \r\n" +
                 " '" + txtNotes.Text + "') ";
                _Notes.ExecuteInstruction();

                MWDataManager.clsDataAccess _Notes1 = new MWDataManager.clsDataAccess();
                _Notes1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes1.SqlStatement = " update tbl_PrePlanning_" + _FormType + "_Notes  \r\n" +
                 " set Notes = '" + txtNotes.Text + "' \r\n" +
                 " where prodmonth = '" + _Prodmonth + "' \r\n" +
                 " and crew = '" + _Crew + "' \r\n" +
                 " and workplace = '" + WP2 + "' ";
                _Notes1.ExecuteInstruction();
            }

            if (WP3 != string.Empty)
            {
                MWDataManager.clsDataAccess _Notes = new MWDataManager.clsDataAccess();
                _Notes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes.SqlStatement = " insert into tbl_PrePlanning_" + _FormType + "_Notes (Prodmonth,Section,Crew,Workplace,Notes)  values( \r\n" +
                 "  " + _Prodmonth + ", \r\n" +
                 " '',  \r\n" +
                 " '" + _Crew + "',   \r\n" +
                 " '" + WP3 + "',  \r\n" +
                 " '" + txtNotes.Text + "') ";
                _Notes.ExecuteInstruction();

                MWDataManager.clsDataAccess _Notes1 = new MWDataManager.clsDataAccess();
                _Notes1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes1.SqlStatement = " update tbl_PrePlanning_" + _FormType + "_Notes  \r\n" +
                 " set Notes = '" + txtNotes.Text + "' \r\n" +
                 " where prodmonth = '" + _Prodmonth + "' \r\n" +
                 " and crew = '" + _Crew + "' \r\n" +
                 " and workplace = '" + WP3 + "' ";
                _Notes1.ExecuteInstruction();
            }

            if (WP4 != string.Empty)
            {
                MWDataManager.clsDataAccess _Notes = new MWDataManager.clsDataAccess();
                _Notes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes.SqlStatement = " insert into tbl_PrePlanning_" + _FormType + "_Notes (Prodmonth,Section,Crew,Workplace,Notes)  values( \r\n" +
                 "  " + _Prodmonth + ", \r\n" +
                 " '',  \r\n" +
                 " '" + _Crew + "',   \r\n" +
                 " '" + WP4 + "',  \r\n" +
                 " '" + txtNotes.Text + "') ";
                _Notes.ExecuteInstruction();

                MWDataManager.clsDataAccess _Notes1 = new MWDataManager.clsDataAccess();
                _Notes1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes1.SqlStatement = " update tbl_PrePlanning_" + _FormType + "_Notes  \r\n" +
                 " set Notes = '" + txtNotes.Text + "' \r\n" +
                 " where prodmonth = '" + _Prodmonth + "' \r\n" +
                 " and crew = '" + _Crew + "' \r\n" +
                 " and workplace = '" + WP4 + "' ";
                _Notes1.ExecuteInstruction();
            }

            if (WP5 != string.Empty)
            {
                MWDataManager.clsDataAccess _Notes = new MWDataManager.clsDataAccess();
                _Notes.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes.SqlStatement = " insert into tbl_PrePlanning_" + _FormType + "_Notes (Prodmonth,Section,Crew,Workplace,Notes)  values( \r\n" +
                 "  " + _Prodmonth + ", \r\n" +
                 " '',  \r\n" +
                 " '" + _Crew + "',   \r\n" +
                 " '" + WP5 + "',  \r\n" +
                 " '" + txtNotes.Text + "') ";

                _Notes.ExecuteInstruction();
                MWDataManager.clsDataAccess _Notes1 = new MWDataManager.clsDataAccess();
                _Notes1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _Notes1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _Notes1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _Notes1.SqlStatement = " update tbl_PrePlanning_" + _FormType + "_Notes  \r\n" +
                 " set Notes = '" + txtNotes.Text + "' \r\n" +
                 " where prodmonth = '" + _Prodmonth + "' \r\n" +
                 " and crew = '" + _Crew + "' \r\n" +
                 " and workplace = '" + WP5 + "' ";
                _Notes1.ExecuteInstruction();
            }

            MWDataManager.clsDataAccess _Del = new MWDataManager.clsDataAccess();
            _Del.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _Del.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _Del.queryReturnType = MWDataManager.ReturnType.DataTable;
            _Del.SqlStatement = " delete from tbl_PrePlanning_" + _FormType + " where Workplace = ''  and Prodmonth = '" + _Prodmonth + "' ";
            _Del.ExecuteInstruction();

            MWDataManager.clsDataAccess _Auth = new MWDataManager.clsDataAccess();
            _Auth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _Auth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _Auth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _Auth.SqlStatement = " Update tbl_PrePlanning_MonthPlan Set " + _FormType + "Dep = 'Y', " + _FormType + "DepCapt = '', " + _FormType + "DepAuth = '' where prodmonth = '" + _Prodmonth + "' and Crew = '" + _Crew.ToString() + "' \r\n";
            _Auth.ExecuteInstruction();
        }

        private void btnAddImage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openFileDialog3.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialog3.FileName = null;
            openFileDialog3.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            Result = openFileDialog3.ShowDialog();

            GetFile();
        }

        private void btnAddDocument_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Result = openFileDialog4.ShowDialog();

            GetDoc();
            loadDocs();
        }

        private void btnAuth_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btnAuth.Caption == "Unauthorise")
            {
                btnImageRemove.Enabled = true;
                btnDocRemove.Enabled = true;

                btnAddImage.Enabled = true;
                btnAddDocument.Enabled = true;
                btnSave.Enabled = true;
                txtNotes.Enabled = true;
                AddActBtn.Enabled = true;
                EditActBtn.Enabled = true;
                DelActBtn.Enabled = true;

                gcWP1.OptionsColumn.AllowEdit = true;
                gcWP2.OptionsColumn.AllowEdit = true;
                gcWP3.OptionsColumn.AllowEdit = true;
                gcWP4.OptionsColumn.AllowEdit = true;
                gcWP5.OptionsColumn.AllowEdit = true;

                btnAuth.Caption = "Authorise";

                MWDataManager.clsDataAccess _PrePlanningSafetyAuth = new MWDataManager.clsDataAccess();
                _PrePlanningSafetyAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _PrePlanningSafetyAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _PrePlanningSafetyAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _PrePlanningSafetyAuth.SqlStatement = " Update tbl_PrePlanning_MonthPlan Set " + _FormType + "DepCapt = '" + TUserInfo.UserID + "', " + _FormType + "DepAuth = ''  where prodmonth = '" + _Prodmonth + "' and Crew = '" + _Crew.ToString() + "' ";

                var result = _PrePlanningSafetyAuth.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Unauthorised", Color.CornflowerBlue);
                }

            }
            else
            {
                btnSave_ItemClick(null, null);

                btnImageRemove.Enabled = false;
                btnDocRemove.Enabled = false;

                txtNotes.Enabled = false;

                btnSave.Enabled = false;
                btnAddImage.Enabled = false;
                btnAddDocument.Enabled = false;

                AddActBtn.Enabled = false;
                EditActBtn.Enabled = false;
                DelActBtn.Enabled = false;

                gcWP1.OptionsColumn.AllowEdit = false;
                gcWP2.OptionsColumn.AllowEdit = false;
                gcWP3.OptionsColumn.AllowEdit = false;
                gcWP4.OptionsColumn.AllowEdit = false;
                gcWP5.OptionsColumn.AllowEdit = false;

                btnAuth.Caption = "Unauthorise";

                MWDataManager.clsDataAccess _PrePlanningSafetyAuth = new MWDataManager.clsDataAccess();
                _PrePlanningSafetyAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _PrePlanningSafetyAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _PrePlanningSafetyAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _PrePlanningSafetyAuth.SqlStatement = " Update tbl_PrePlanning_MonthPlan Set " + _FormType + "DepAuth = '" + TUserInfo.UserID + "'  where prodmonth = '" + _Prodmonth + "' and Crew = '" + _Crew.ToString() + "' ";

                var result = _PrePlanningSafetyAuth.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Authorised", Color.CornflowerBlue);
                }

            }

            #region Incidents
            MWDataManager.clsDataAccess _PrePlanningIncidentsAuth = new MWDataManager.clsDataAccess();
            _PrePlanningIncidentsAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _PrePlanningIncidentsAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningIncidentsAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningIncidentsAuth.SqlStatement = " exec sp_Incidents_Update  '" + ActionType + "' ";
            var result2 = _PrePlanningIncidentsAuth.ExecuteInstruction();
            #endregion
        }

        private void AddActBtn_Click(object sender, EventArgs e)
        {
            frmGraphicsPrePlanningActionCapture ActFrm = new frmGraphicsPrePlanningActionCapture();
            ActFrm.tbPM.Text = tbProdMonth.EditValue.ToString();
            ActFrm.tbSections.Text = tbSection.EditValue.ToString();
            ActFrm.WPEdit2.Properties.Items.Add(WP1Desc);
            if (WP2Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP2Desc);
            }
            if (WP3Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP3Desc);
            }
            if (WP4Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP4Desc);
            }
            if (WP5Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP5Desc);
            }
            ActFrm.WPEdit2.Visible = true;
            ActFrm.tbWorkplace.Visible = false;

            ActFrm._theSystemDBTag = this.theSystemDBTag;
            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;

            ActFrm.Item = ActionDescription;
            ActFrm.Type = ActionType;
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

            frmGraphicsPrePlanningActionCapture ActFrm = new frmGraphicsPrePlanningActionCapture();
            ActFrm.tbPM.Text = tbProdMonth.EditValue.ToString();
            ActFrm.tbSections.Text = tbSection.EditValue.ToString();
            ActFrm.WPEdit2.Properties.Items.Add(WP1Desc);

            if (WP2Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP2Desc);
            }

            if (WP3Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP3Desc);
            }

            if (WP4Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP4Desc);
            }

            if (WP5Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP5Desc);
            }
            ActFrm.WPEdit2.Visible = true;
            ActFrm.tbWorkplace.Visible = false;

            ActFrm._theSystemDBTag = this.theSystemDBTag;
            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;

            ActFrm.Item = "Rock Engineering Actions";
            ActFrm.Type = ActionType;
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";

            ActFrm.WPEdit2.EditValue = Workplace;
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

        private void DelActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please click the row you want to delete first.");
                return;
            }



            MWDataManager.clsDataAccess _DeleteAction = new MWDataManager.clsDataAccess();
            _DeleteAction.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _DeleteAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _DeleteAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _DeleteAction.SqlStatement = " Delete from tbl_Shec_Incidents where ID = '" + ID + "'\r\n";

            _DeleteAction.ExecuteInstruction();

            LoadActions();
        }

        private void DocsLB_DoubleClick(object sender, EventArgs e)
        {
            string mianDicrectory = RepDirDoc;
            if (DocsLB.SelectedIndex != -1)
            {
                System.Diagnostics.Process.Start(mianDicrectory + "\\" + tbCrew.EditValue.ToString() + tbProdMonth.EditValue.ToString() + DocsLB.SelectedItem.ToString());
            }
        }

        #endregion

        protected void Question_Changed(object sender, EventArgs e)
        {

        }

        private void gvCapture_ShownEditor(object sender, EventArgs e)
        {
            SelectedQuestID = Convert.ToInt32(gvCapture.GetFocusedRowCellValue("QuestID"));
            ValueType = gvCapture.GetFocusedRowCellValue("ValueType").ToString();
            foundIndex = 0;
            AnswersFound = "N";

            if (gvCapture.FocusedColumn.Caption == WP1Desc || gvCapture.FocusedColumn.Caption == WP2Desc
                    || gvCapture.FocusedColumn.Caption == WP3Desc || gvCapture.FocusedColumn.Caption == WP4Desc
                    || gvCapture.FocusedColumn.Caption == WP5Desc)
            {
                foundIndex = bsAnswers.Find("QuestID", SelectedQuestID);
                if (SelectedQuestID != 0 && foundIndex > -1 && ValueType == "DropDownList")
                {
                    try
                    {
                        AnswersFound = "Y";
                        repRockEngSubQuestEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                        LookUpEdit editor2 = (LookUpEdit)gvCapture.ActiveEditor;
                        editor2.Properties.DataSource = dtSubQuestions.DefaultView.Cast<DataRowView>().Where(a => a.Row["QuestID"].ToString() == SelectedQuestID.ToString());
                        //gcWP1.ColumnEdit = repRockEngSubQuestEdit;
                        //gcWP2.ColumnEdit = repRockEngSubQuestEdit;
                        //gcWP3.ColumnEdit = repRockEngSubQuestEdit;
                        //gcWP4.ColumnEdit = repRockEngSubQuestEdit;
                        //gcWP5.ColumnEdit = repRockEngSubQuestEdit;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void repSubQuestEdit_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit lookUpEdit = sender as LookUpEdit;
            DataRowView selectedDataRow = (DataRowView)lookUpEdit.GetSelectedDataRow();

            Likelihood = selectedDataRow.Row["Likelihood"].ToString();
            Consequence = selectedDataRow.Row["Consequence"].ToString();
            RiskRating = selectedDataRow.Row["RiskRating"].ToString();

            if (gvCapture.FocusedColumn.Caption == WP1Desc)
            {
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP1Likelihood"], Likelihood);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP1Consequence"], Consequence);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP1RiskRating"], RiskRating);
            }
            if (gvCapture.FocusedColumn.Caption == WP2Desc)
            {
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP2Likelihood"], Likelihood);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP2Consequence"], Consequence);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP2RiskRating"], RiskRating);
            }
            if (gvCapture.FocusedColumn.Caption == WP3Desc)
            {
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP3Likelihood"], Likelihood);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP3Consequence"], Consequence);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP3RiskRating"], RiskRating);
            }
            if (gvCapture.FocusedColumn.Caption == WP4Desc)
            {
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP4Likelihood"], Likelihood);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP4Consequence"], Consequence);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP4RiskRating"], RiskRating);
            }
            if (gvCapture.FocusedColumn.Caption == WP5Desc)
            {
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP5Likelihood"], Likelihood);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP5Consequence"], Consequence);
                gvCapture.SetRowCellValue(gvCapture.FocusedRowHandle, gvCapture.Columns["WP5RiskRating"], RiskRating);
            }

            gvCapture.PostEditor();
        }

        private void tblPrePlanningSubQuestRockEngBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void repTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            this.gvCapture.PostEditor();
            this.gvCapture.SetFocusedRowCellValue("CityCode", null);
        }

        private void openFileDialog4_FileOk(object sender, CancelEventArgs e)
        {
            string Docs = openFileDialog4.FileName;

            int indexa = Docs.LastIndexOf("\\");

            string sourcefilename = Docs.Substring(indexa + 1, (Docs.Length - indexa) - 1);

            DocsLB.Items.Add(sourcefilename);
        }

        private void layoutControlGroup4_Click(object sender, EventArgs e)
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\" + _FormType + " ");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(RepDir + "\\Preplanning\\" + _FormType + " ");

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                if (item.ToString() == RepDir + "\\Preplanning\\" + _FormType + " " + "\\" + tbCrew.EditValue.ToString() + tbProdMonth.EditValue.ToString() + ext)
                {
                    txtAttachment.Text = item.ToString();
                }
            }
            if (txtAttachment.Text != string.Empty)
            {
                PicBox.Image = null;
                File.Delete(txtAttachment.Text);
            }
        }

        private void layoutControlGroup7_Click(object sender, EventArgs e)
        {
            //btnDocRemove_Click(null,null);
            string mianDicrectory = RepDirDoc;
            if (DocsLB.SelectedIndex != -1)
            {
                File.Delete(mianDicrectory + "\\" + tbCrew.EditValue.ToString() + tbProdMonth.EditValue.ToString() + DocsLB.SelectedItem.ToString());
            }

            loadDocs();
        }

        private void repDateEdit_EditValueChanged(object sender, EventArgs e)
        {
            gvCapture.PostEditor();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
            //this.Close();
        }
    }
}
