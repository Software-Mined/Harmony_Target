using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mineware.Systems.GlobalConnect;
using DevExpress.XtraCharts;

namespace Mineware.Systems.HarmonyPAS.SysAdminScreens.Labour
{
    public partial class ucLabourDashboard : Mineware.Systems.Global.ucBaseUserControl
    {

        private DataSet dsTableRegister = new DataSet();
        private DataTable dtBarChartDataTL = new DataTable("dtBarChartDataTL");
        private DataTable dtBarChartDataRDO = new DataTable("dtBarChartDataRDO");
        private DataTable dtBarChartDataTCS = new DataTable("dtBarChartDataTCS");
        private DataTable dtBarChartDataWO = new DataTable("dtBarChartDataWO");
        private DataTable dtBarChartDataCM = new DataTable("dtBarChartDataCM");
        private DataTable dtGaugesDataTeamLeader = new DataTable("dtGaugesDataTeamLeader");
        private DataTable dtGaugesDataRockDrillOperator = new DataTable("dtGaugesDataRockDrillOperator");
        private DataTable dtGaugesDataCrewMember = new DataTable("dtGaugesDataCrewMember");
        private DataTable dtGaugesDataWinchOperator = new DataTable("dtGaugesDataWinchOperator");
        private DataTable dtSectionsData = new DataTable("dtSectionsData");
        private DataTable dtOccupationData = new DataTable("dtOccupationData");
        private String Section;
        private String TlStats = "";
        private String TlStatsProg = "";
        private String RDOStats = "";
        private String RDOStatsProg = "";
        private String WOStats = "";
        private String WOStatsProg = "";
        private String CMStats = "";
        private String CMStatsProg = "";
        private String[] strList = new String[8]
                {"TlStats"
                ,"TlStatsProg"
                ,"RDOStats"
                ,"RDOStatsProg"
                ,"WOStats"
                ,"WOStatsProg"
                ,"CMStats"
                ,"CMStatsProg"};

        private void loadBarChars()
        {
            //Get Data TL

            MWDataManager.clsDataAccess _barChartTeamLeader = new MWDataManager.clsDataAccess();
            _barChartTeamLeader.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _barChartTeamLeader.SqlStatement = " SELECT MO as Section, convert(int,value) value FROM [dbo].tbl_Labour_DashBoardMain where maintype = 'Team Leader' and mo <> 'Total' order by value ";
            _barChartTeamLeader.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _barChartTeamLeader.queryReturnType = MWDataManager.ReturnType.DataTable;
            _barChartTeamLeader.ExecuteInstruction();
            dtBarChartDataTL = _barChartTeamLeader.ResultsDataTable;

            //Get Data RDO

            MWDataManager.clsDataAccess _barChartRDO = new MWDataManager.clsDataAccess();
            _barChartRDO.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _barChartRDO.SqlStatement = " SELECT MO as Section, convert(int,value) value FROM [dbo].tbl_Labour_DashBoardMain where maintype = 'Rock Drill Operator' and mo <> 'Total' order by value ";
            _barChartRDO.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _barChartRDO.queryReturnType = MWDataManager.ReturnType.DataTable;
            _barChartRDO.ExecuteInstruction();
            dtBarChartDataRDO = _barChartRDO.ResultsDataTable;

            //Get Data TCS

            MWDataManager.clsDataAccess _barChartTCS = new MWDataManager.clsDataAccess();
            _barChartTCS.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _barChartTCS.SqlStatement = "SELECT MO AS Section , case when sum(Total) > 0 then CONVERT(int, SUM(Num)/ SUM(Total) *100) else 0 end [value] " +
               " FROM " +
               " ( " +
               " SELECT * FROM[dbo].tbl_Labour_DashBoardMain " +
               " WHERE MO <> 'Total' " +
               " ) Main " +
               " GROUP BY MO " +
               "ORDER BY value";
            _barChartTCS.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _barChartTCS.queryReturnType = MWDataManager.ReturnType.DataTable;
            _barChartTCS.ExecuteInstruction();
            dtBarChartDataTCS = _barChartTCS.ResultsDataTable;

            //Get Data WO

            MWDataManager.clsDataAccess _barChartWO = new MWDataManager.clsDataAccess();
            _barChartWO.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _barChartWO.SqlStatement = " SELECT MO as Section, convert(int,value) value FROM [dbo].tbl_Labour_DashBoardMain where maintype = 'Winch Operator' and mo <> 'Total' order by value ";
            _barChartWO.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _barChartWO.queryReturnType = MWDataManager.ReturnType.DataTable;
            _barChartWO.ExecuteInstruction();
            dtBarChartDataWO = _barChartWO.ResultsDataTable;

            //Get Data CM

            MWDataManager.clsDataAccess _barChartCM = new MWDataManager.clsDataAccess();
            _barChartCM.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _barChartCM.SqlStatement = " SELECT MO as Section, convert(int,value) value FROM [dbo].tbl_Labour_DashBoardMain where maintype = 'Crew Member' and mo <> 'Total' order by value ";
            _barChartCM.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _barChartCM.queryReturnType = MWDataManager.ReturnType.DataTable;
            _barChartCM.ExecuteInstruction();
            dtBarChartDataCM = _barChartCM.ResultsDataTable;


            //Set BarChart Values

            //Team Leader

            ccLeftTop.DataSource = dtBarChartDataTL;
            ccLeftTop.Series[0].ArgumentScaleType = ScaleType.Auto;
            ccLeftTop.Series[0].ArgumentDataMember = "Section";
            ccLeftTop.Series[0].ValueScaleType = ScaleType.Numerical;
            ccLeftTop.Series[0].ValueDataMembers.AddRange(new string[] { "value" });

            //RDO
            ccLeftBottom.DataSource = dtBarChartDataRDO;
            ccLeftBottom.Series[0].ArgumentScaleType = ScaleType.Auto;
            ccLeftBottom.Series[0].ArgumentDataMember = "Section";
            ccLeftBottom.Series[0].ValueScaleType = ScaleType.Numerical;
            ccLeftBottom.Series[0].ValueDataMembers.AddRange(new string[] { "value" });

            //Total Critical Skills
            ccMiddle.DataSource = dtBarChartDataTCS;
            ccMiddle.Series[0].ArgumentScaleType = ScaleType.Auto;
            ccMiddle.Series[0].ArgumentDataMember = "Section";
            ccMiddle.Series[0].ValueScaleType = ScaleType.Numerical;
            ccMiddle.Series[0].ValueDataMembers.AddRange(new string[] { "value" });

            //Winch Operator
            ccRightTop.DataSource = dtBarChartDataWO;
            ccRightTop.Series[0].ArgumentScaleType = ScaleType.Auto;
            ccRightTop.Series[0].ArgumentDataMember = "Section";
            ccRightTop.Series[0].ValueScaleType = ScaleType.Numerical;
            ccRightTop.Series[0].ValueDataMembers.AddRange(new string[] { "value" });

            //Crew Member
            ccRightBottom.DataSource = dtBarChartDataCM;
            ccRightBottom.Series[0].ArgumentScaleType = ScaleType.Auto;
            ccRightBottom.Series[0].ArgumentDataMember = "Section";
            ccRightBottom.Series[0].ValueScaleType = ScaleType.Numerical;
            ccRightBottom.Series[0].ValueDataMembers.AddRange(new string[] { "value" });

        }

        private void registerDeclarations()
        {
            dsTableRegister.Tables.Add(dtBarChartDataTL);
            dsTableRegister.Tables.Add(dtBarChartDataRDO);
            dsTableRegister.Tables.Add(dtBarChartDataTCS);
            dsTableRegister.Tables.Add(dtBarChartDataWO);
            dsTableRegister.Tables.Add(dtBarChartDataCM);
            dsTableRegister.Tables.Add(dtGaugesDataTeamLeader);
            dsTableRegister.Tables.Add(dtGaugesDataRockDrillOperator);
            dsTableRegister.Tables.Add(dtGaugesDataCrewMember);
            dsTableRegister.Tables.Add(dtGaugesDataWinchOperator);
            dsTableRegister.Tables.Add(dtSectionsData);
            dsTableRegister.Tables.Add(dtOccupationData);
        }

        private void fillBlankData(string TableName)
        {
            DataTable dtClear = new DataTable();
            dtClear.Columns.Add("Section");
            dtClear.Columns.Add("value");
            dtClear.Columns.Add("Num");
            dtClear.Columns.Add("Total");
            dtClear.Columns.Add("ProgValue");
            dtClear.Columns.Add("ProgNum");
            dtClear.Columns.Add("ProgTotal");
            dtClear.Rows.Add("No Data", "0", "0", "0", "0", "0", "0");

            for (int i = 0; i < dsTableRegister.Tables.Count; i++)
            {
                if(dsTableRegister.Tables[i].TableName == TableName)
                {
                    dsTableRegister.Tables[i].Clear();
                    dsTableRegister.Merge(dtClear);
                    return;
                }
            }
        }
        private void loadGauges()
        {
            //If Data is empty
            DataTable dtClear = new DataTable();
            dtClear.Columns.Add("Section");
            dtClear.Columns.Add("value");
            dtClear.Columns.Add("Num");
            dtClear.Columns.Add("Total");
            dtClear.Columns.Add("ProgValue");
            dtClear.Columns.Add("ProgNum");
            dtClear.Columns.Add("ProgTotal");
            dtClear.Rows.Add("No Data", "0", "0", "0", "0", "0", "0");

            //Get Data Team Leader

            MWDataManager.clsDataAccess _guagesCritSkillsTL = new MWDataManager.clsDataAccess();
            _guagesCritSkillsTL.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _guagesCritSkillsTL.SqlStatement = " SELECT MO as Section, convert(int,value) value, Num, Total, ProgValue, ProgNum, ProgTotal FROM [dbo].tbl_Labour_DashBoardMain where maintype = 'Team Leader' and mo = '" + Section + "' order by value ";
            _guagesCritSkillsTL.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _guagesCritSkillsTL.queryReturnType = MWDataManager.ReturnType.DataTable;
            _guagesCritSkillsTL.ExecuteInstruction();
            if (_guagesCritSkillsTL.ResultsDataTable.Rows.Count != 0)
            {
                dtGaugesDataTeamLeader = _guagesCritSkillsTL.ResultsDataTable;
                 TlStats = "Daily " + dtGaugesDataTeamLeader.Rows[0]["value"].ToString() + "% \r \n" + dtGaugesDataTeamLeader.Rows[0]["Num"].ToString() + " of " + dtGaugesDataTeamLeader.Rows[0]["Total"].ToString();
                 TlStatsProg = "Prog " + dtGaugesDataTeamLeader.Rows[0]["Progvalue"].ToString() + "% \r \n" + dtGaugesDataTeamLeader.Rows[0]["ProgNum"].ToString() + " of " + dtGaugesDataTeamLeader.Rows[0]["ProgTotal"].ToString();
            }
            else
            {
                //fillBlankData("dtGaugesDataTeamLeader");
                dtGaugesDataTeamLeader = dtClear;
                TlStats = "Daily " + dtGaugesDataTeamLeader.Rows[0]["value"].ToString() + "% \r \n" + dtGaugesDataTeamLeader.Rows[0]["Num"].ToString() + " of " + dtGaugesDataTeamLeader.Rows[0]["Total"].ToString();
                TlStatsProg = "Prog " + dtGaugesDataTeamLeader.Rows[0]["Progvalue"].ToString() + "% \r \n" + dtGaugesDataTeamLeader.Rows[0]["ProgNum"].ToString() + " of " + dtGaugesDataTeamLeader.Rows[0]["ProgTotal"].ToString();
            }
            //Get Data RDO

            MWDataManager.clsDataAccess _guagesCritSkillsRDO = new MWDataManager.clsDataAccess();
            _guagesCritSkillsRDO.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _guagesCritSkillsRDO.SqlStatement = " SELECT MO as Section, convert(int,value) value, Num, Total, ProgValue, ProgNum, ProgTotal  FROM [dbo].tbl_Labour_DashBoardMain where maintype = 'Rock Drill Operator' and mo = '" + Section + "' order by value ";
            _guagesCritSkillsRDO.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _guagesCritSkillsRDO.queryReturnType = MWDataManager.ReturnType.DataTable;
            _guagesCritSkillsRDO.ExecuteInstruction();
            if (_guagesCritSkillsRDO.ResultsDataTable.Rows.Count != 0)
            {
                dtGaugesDataRockDrillOperator = _guagesCritSkillsRDO.ResultsDataTable;
                 RDOStats = "Daily " + dtGaugesDataRockDrillOperator.Rows[0]["value"].ToString() + "% \r \n" + dtGaugesDataRockDrillOperator.Rows[0]["Num"].ToString() + " of " + dtGaugesDataRockDrillOperator.Rows[0]["Total"].ToString();
                 RDOStatsProg = "Prog " + dtGaugesDataRockDrillOperator.Rows[0]["Progvalue"].ToString() + "% \r \n" + dtGaugesDataRockDrillOperator.Rows[0]["ProgNum"].ToString() + " of " + dtGaugesDataRockDrillOperator.Rows[0]["ProgTotal"].ToString();
            }
            else
            {
                //fillBlankData("dtGaugesDataRockDrillOperator");
                dtGaugesDataRockDrillOperator = dtClear;
                RDOStats = "Daily " + dtGaugesDataRockDrillOperator.Rows[0]["value"].ToString() + "% \r \n" + dtGaugesDataRockDrillOperator.Rows[0]["Num"].ToString() + " of " + dtGaugesDataRockDrillOperator.Rows[0]["Total"].ToString();
                RDOStatsProg = "Prog " + dtGaugesDataRockDrillOperator.Rows[0]["Progvalue"].ToString() + "% \r \n" + dtGaugesDataRockDrillOperator.Rows[0]["ProgNum"].ToString() + " of " + dtGaugesDataRockDrillOperator.Rows[0]["ProgTotal"].ToString();
            }
            //Get Data WO

            MWDataManager.clsDataAccess _guagesCritSkillsWO = new MWDataManager.clsDataAccess();
            _guagesCritSkillsWO.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _guagesCritSkillsWO.SqlStatement = " SELECT MO as Section, convert(int,value) value, Num, Total, ProgValue, ProgNum, ProgTotal  FROM [dbo].tbl_Labour_DashBoardMain where maintype = 'Winch Operator' and mo = '" + Section + "' order by value ";
            _guagesCritSkillsWO.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _guagesCritSkillsWO.queryReturnType = MWDataManager.ReturnType.DataTable;
            _guagesCritSkillsWO.ExecuteInstruction();
            if (_guagesCritSkillsWO.ResultsDataTable.Rows.Count != 0)
            {
                dtGaugesDataWinchOperator = _guagesCritSkillsWO.ResultsDataTable;
                 WOStats = "Daily " + dtGaugesDataWinchOperator.Rows[0]["value"].ToString() + "% \r \n" + dtGaugesDataWinchOperator.Rows[0]["Num"].ToString() + " of " + dtGaugesDataWinchOperator.Rows[0]["Total"].ToString();
                 WOStatsProg = "Prog " + dtGaugesDataWinchOperator.Rows[0]["Progvalue"].ToString() + "% \r \n" + dtGaugesDataWinchOperator.Rows[0]["ProgNum"].ToString() + " of " + dtGaugesDataWinchOperator.Rows[0]["ProgTotal"].ToString();
            }
            else
            {
                //fillBlankData("dtGaugesDataWinchOperator");
                dtGaugesDataWinchOperator = dtClear;
                WOStats = "Daily " + dtGaugesDataWinchOperator.Rows[0]["value"].ToString() + "% \r \n" + dtGaugesDataWinchOperator.Rows[0]["Num"].ToString() + " of " + dtGaugesDataWinchOperator.Rows[0]["Total"].ToString();
                WOStatsProg = "Prog " + dtGaugesDataWinchOperator.Rows[0]["Progvalue"].ToString() + "% \r \n" + dtGaugesDataWinchOperator.Rows[0]["ProgNum"].ToString() + " of " + dtGaugesDataWinchOperator.Rows[0]["ProgTotal"].ToString();

            }
            //Get Data CM

            MWDataManager.clsDataAccess _guagesCritSkillsCM = new MWDataManager.clsDataAccess();
            _guagesCritSkillsCM.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _guagesCritSkillsCM.SqlStatement = " SELECT MO as Section, convert(int,value) value, Num, Total, ProgValue, ProgNum, ProgTotal  FROM [dbo].tbl_Labour_DashBoardMain where maintype = 'Crew Member' and mo = '" + Section + "' order by value ";
            _guagesCritSkillsCM.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _guagesCritSkillsCM.queryReturnType = MWDataManager.ReturnType.DataTable;
            _guagesCritSkillsCM.ExecuteInstruction();
            if (_guagesCritSkillsCM.ResultsDataTable.Rows.Count != 0)
            {
                dtGaugesDataCrewMember = _guagesCritSkillsCM.ResultsDataTable;
                 CMStats = "Daily " + dtGaugesDataCrewMember.Rows[0]["value"].ToString() + "% \r \n" + dtGaugesDataCrewMember.Rows[0]["Num"].ToString() + " of " + dtGaugesDataCrewMember.Rows[0]["Total"].ToString();
                 CMStatsProg = "Prog " + dtGaugesDataCrewMember.Rows[0]["ProgValue"].ToString() + "% \r \n" + dtGaugesDataCrewMember.Rows[0]["ProgNum"].ToString() + " of " + dtGaugesDataCrewMember.Rows[0]["ProgTotal"].ToString();
            }
            else
            {
                //fillBlankData("dtGaugesDataCrewMember");
                dtGaugesDataCrewMember = dtClear;
                CMStats = "Daily " + dtGaugesDataCrewMember.Rows[0]["value"].ToString() + "% \r \n" + dtGaugesDataCrewMember.Rows[0]["Num"].ToString() + " of " + dtGaugesDataCrewMember.Rows[0]["Total"].ToString();
                CMStatsProg = "Prog " + dtGaugesDataCrewMember.Rows[0]["ProgValue"].ToString() + "% \r \n" + dtGaugesDataCrewMember.Rows[0]["ProgNum"].ToString() + " of " + dtGaugesDataCrewMember.Rows[0]["ProgTotal"].ToString();
            }

            //Set Gauges

            arcScaleNeedleComponent1.Value = Convert.ToInt16(dtGaugesDataTeamLeader.Rows[0]["value"]);
            arcScaleNeedleComponent2.Value = Convert.ToInt16(dtGaugesDataRockDrillOperator.Rows[0]["value"]);
            arcScaleNeedleComponent4.Value = Convert.ToInt16(dtGaugesDataCrewMember.Rows[0]["value"]);
            arcScaleNeedleComponent3.Value = Convert.ToInt16(dtGaugesDataWinchOperator.Rows[0]["value"]);



            //Set Display Parameters and Labels

            lblGaugeONEDaily.Text = TlStats;
            lblGaugeTWODaily.Text = RDOStats;
            lblGaugeTHREEDaily.Text = WOStats;
            lblGaugeFOURDaily.Text = CMStats;

            lblGaugeONEProg.Text = TlStatsProg;
            lblGaugeTWOProg.Text = RDOStatsProg;
            lblGaugeTHREEProg.Text = WOStatsProg;
            lblGaugeFOURProg.Text = CMStatsProg;
            //lblGaugeONEDaily.Text = Convert.ToInt16(dtGaugesData.Rows[0]["TLDaily"]).ToString() + "%";
            //lblGaugeTWODaily.Text = Convert.ToInt16(dtGaugesData.Rows[0]["RDODaily"]).ToString() + "%";
            //lblGaugeTHREEDaily.Text = Convert.ToInt16(dtGaugesData.Rows[0]["WODaily"]).ToString() + "%";
            //lblGaugeFOURDaily.Text = Convert.ToInt16(dtGaugesData.Rows[0]["CMDaily"]).ToString() + "%";

            lblGaugeONE.Text = "Team Leader";
            lblGaugeTWO.Text = "RDO";
            lblGaugeTHREE.Text = "Winch Operator";
            lblGuageFOUR.Text = "Crew Member";

        }

        private void loadCombos()
        {

            //Get Data

            MWDataManager.clsDataAccess _cbSections = new MWDataManager.clsDataAccess();
            _cbSections.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _cbSections.SqlStatement = "SELECT DISTINCT(Sectionid) sec FROM dbo.PLANNING WHERE ProdMonth = (SELECT currentproductionmonth FROM Sysset) ORDER BY SectionID";

            _cbSections.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _cbSections.queryReturnType = MWDataManager.ReturnType.DataTable;
            _cbSections.ExecuteInstruction();
            dtSectionsData = _cbSections.ResultsDataTable;

            //Set ComboBox

            cbSection.Properties.Items.Add("Total");

            for(int i = 0; i < dtSectionsData.Rows.Count; i++)
            {
                cbSection.Properties.Items.Add(dtSectionsData.Rows[i][0]);
            }
            cbSection.SelectedIndex = 0;


            //Get Data Occupation

            MWDataManager.clsDataAccess _cbOcc = new MWDataManager.clsDataAccess();
            _cbOcc.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _cbOcc.SqlStatement = "SELECT DISTINCT (name) name FROM dbo.Personal_CriticalSkills_DefaultLink ORDER BY name";

            _cbOcc.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _cbOcc.queryReturnType = MWDataManager.ReturnType.DataTable;
            _cbOcc.ExecuteInstruction();
            dtOccupationData = _cbOcc.ResultsDataTable;

            //Set ComboBox

            for (int i = 0; i < dtOccupationData.Rows.Count; i++)
            {
                cbOccupation.Properties.Items.Add(dtOccupationData.Rows[i][0]);
            }
            cbOccupation.SelectedIndex = 0;

        }

        public ucLabourDashboard()
        {
            InitializeComponent();
        }

        
        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void navBarFilters_SizeChanged(object sender, EventArgs e)
        {
            //visual changes to panel on open and close of filters tab on screen
            pnlLabour_Left.Width = navBarFilters.Width;
            if (navBarFilters.Width == 32)
            {
                pnlLabour_Left_Top.Dock = DockStyle.Fill;
                pnlLabour_Left_Top.BringToFront();
            }
            if(navBarFilters.Width == 200)
            {
                pnlLabour_Left_Top.Dock = DockStyle.Top;
                pnlLabour_Left_Fill.BringToFront();
            }
        }


        private void ucLabourDashboard_Load(object sender, EventArgs e)
        {
            Section = "Total";

            //Adds tables to dataset
            registerDeclarations();

            //Load data for barcharts
            loadBarChars();

            //Load data for gauges
            loadGauges();

            //Hide Filter Panel and set variants of labels
            pnlLabour_Left.Visible = false;

            var combinesec = cbSection.Location.X - lblSec.Width;
            lblSec.Left = combinesec;

            var combineocc = cbOccupation.Location.X - lblOcc.Width;
            lblOcc.Left = combineocc;

            //Load Combo boxes
            loadCombos();

            

        }

        private void ucLabourDashboard_SizeChanged(object sender, EventArgs e)
        {

            //change panel effect on resize on main form
            pnlLabourCritOne.Width = pnlLabour_Main.Width / 3;
            pnlLabourCritTwo.Width = pnlLabour_Main.Width / 3;
            pnlLabourCritThree.Width = pnlLabour_Main.Width / 3;

            pnlGaugeONE.Width = pnlLabour_Main.Width / 4;
            pnlGaugeTWO.Width = pnlLabour_Main.Width / 4;
            pnlGaugeTHREE.Width = pnlLabour_Main.Width / 4;
            pnlGaugeFOUR.Width = pnlLabour_Main.Width / 4;

            pnlCritLeft1.Height = ccMiddle.Height / 2 + 25 ;
            pnlCritRight.Height = ccMiddle.Height / 2 + 25;

            pnlLabour_Main_Top.Height = pnlLabour_Main.Height / 2 + 75;
        }

        private void cbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(cbSection.Text))
            {
                Section = cbSection.Text;
                loadGauges();
            }
        }

        private void ccLeftTop_CustomDrawSeries(object sender, CustomDrawSeriesEventArgs e)
        {
        }
    }
}
