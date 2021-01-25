using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionHelp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace Mineware.Systems.Production.Planning
{
    public partial class ucPlanningMain : BaseUserControl
    {
        public ucPlanningMain()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpPreplanning);
            FormActiveRibbonPage = rpPreplanning;
            FormMainRibbonPage = rpPreplanning;
            RibbonControl = rcPlanning;
        }


        #region public varibles



        #endregion


        #region private varibles
        string SelectProdmonth;
        string SelectMOSectionID;
        string SelectActivity;
        string CycleCode;
        string MoCycleNumNew;
        string FirtsLoad;

        DataTable dtAllSections = new DataTable();
        DataTable tblOrgUnitsData = new DataTable();
        DataTable tblMinerListData = new DataTable();
        DataTable tblBoxholeData = new DataTable();
        DataTable tblPlanningData = new DataTable();
        DataTable tblPlanningCycleData = new DataTable();
        DataTable tblPlanningWorkingDays = new DataTable();

        Mineware.Systems.Global.sysMessages.sysMessagesClass theMessage = new Global.sysMessages.sysMessagesClass();
        #endregion


        private void ucAmandelbultPlanningMain_Load(object sender, EventArgs e)
        {
            FirtsLoad = "Y";

            SelectProdmonth = ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString();

            //barProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobal.ProductionGlobal.getSystemSettingsProductionInfo(UserCurrentInfo.Connection)._currentProductionMonth.ToString());

            barProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth);

            LoadBoxholeList();
            LoadOrgUnits();

            LoadSections();
            LoadActivity();

            barSection.EditValue = dtAllSections.Rows[0]["SectionID"].ToString();
            barActivity.EditValue = "0";

            barbtnShow_ItemClick(null, null);

            FirtsLoad = "N";
        }

        private void LoadCyleCodes()
        {
            DataTable DtCycleCodes = new DataTable();

            MWDataManager.clsDataAccess _CycleCodes = new MWDataManager.clsDataAccess();

            _CycleCodes.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _CycleCodes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _CycleCodes.queryReturnType = MWDataManager.ReturnType.DataTable;

            if (barActivity.EditValue.ToString() != "1")
            {
                _CycleCodes.SqlStatement = "SELECT code CycleCode,[Description] FROM  tbl_Central_Code_Cycle where Stope = 'Y' ";
            }

            if (barActivity.EditValue.ToString() == "1")
            {
                _CycleCodes.SqlStatement = "SELECT code CycleCode,[Description] FROM  tbl_Central_Code_Cycle where Dev = 'Y' ";
            }

            var CycleCodesResult = _CycleCodes.ExecuteInstruction();
            //DtCycleCodes = null;
            DtCycleCodes = _CycleCodes.ResultsDataTable;

            lbxCodeCycles.DataSource = null;
            lbxCodeCycles.DataSource = DtCycleCodes;
            lbxCodeCycles.ValueMember = "CycleCode";
            lbxCodeCycles.DisplayMember = "Description";

        }

        private void LoadSections()
        {
            MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
            minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            minerData.SqlStatement = "Select SectionID,Name from tbl_Section where prodmonth = '" + SelectProdmonth + "' and Hierarchicalid = '4'  order by SectionID";

            var theResult = minerData.ExecuteInstruction();

            DataTable dtSections = new DataTable();
            dtSections = minerData.ResultsDataTable;

            dtAllSections = dtSections;

            if (dtAllSections.Rows.Count > 0)
            {
                repSleSection.DataSource = null;
                repSleSection.DataSource = dtAllSections;
                repSleSection.DisplayMember = "Name";
                repSleSection.ValueMember = "SectionID";
                repSleSection.PopulateViewColumns();
                repSleSection.View.Columns[0].Width = 80;
            }


            //if (dtAllSections.Rows.Count == 0)
            //    theMessage.viewMessage(MessageType.Info, "NO SECTIONS", "There are no sections avaliable for production month " + THarmonyPASGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue)), ButtonTypes.OK, MessageDisplayType.FullScreen);
        }

        private void LoadActivity()
        {
            MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
            minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            minerData.SqlStatement = "Select * from tbl_Code_Activity where activity <> '9' ";

            var theResult = minerData.ExecuteInstruction();

            DataTable dtActivity = new DataTable();
            dtActivity = minerData.ResultsDataTable;


            if (dtActivity.Rows.Count > 0)
            {
                repLeActivity.DataSource = null;
                repLeActivity.DataSource = dtActivity;
                repLeActivity.DisplayMember = "Description";
                repLeActivity.ValueMember = "Activity";
                repLeActivity.PopulateColumns();
                repLeActivity.Columns[0].Width = 80;
            }


            //if (dtAllSections.Rows.Count == 0)
            //    theMessage.viewMessage(MessageType.Info, "NO SECTIONS", "There are no sections avaliable for production month " + THarmonyPASGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue)), ButtonTypes.OK, MessageDisplayType.FullScreen);
        }

        private void barbtnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(barSection.EditValue.ToString()))
            {
                MessageBox.Show("Please select a section");
                return;
            }
            if (string.IsNullOrEmpty(barActivity.EditValue.ToString()))
            {
                MessageBox.Show("Please select a Activity");
                return;
            }

            barbtnCyclePlanning.Down = false;

            SelectProdmonth = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdMonth.EditValue.ToString()));
            SelectMOSectionID = barSection.EditValue.ToString();
            SelectActivity = barActivity.EditValue.ToString();

            layoutControlMain.Visible = true;

            MainGrid.Visible = false;
            MainGridStope.Visible = false;
            pcCycle.Visible = false;

            if (tbCycle.SelectedIndex == 1)
            {
                pcCycle.Visible = true;
                pcCycle.Height = 80;
                gcCyclePlan.Visible = false;
                pcInfo.Visible = false;
            }

            this.Cursor = Cursors.WaitCursor;

            LoadMinerList();
            LoadCadsPlan();
            LoadPlanned();
            LoadCyleCodes();
            CalcSum();

            this.Cursor = Cursors.Default;
        }


        private void CalcSum()
        {
            if (barActivity.EditValue.ToString() != "1")
            {
                decimal PlanCall = 0;
                decimal CycleCall = 0;

                foreach (DataRow dr in tblPlanningData.Rows)
                {
                    PlanCall = PlanCall + Convert.ToDecimal(dr["MonthlyTotalSQM"].ToString());
                    CycleCall = CycleCall + Convert.ToDecimal(dr["callValue"].ToString());
                }

                barTxtTotalPlanCall.EditValue = PlanCall;
                barTxtTotalCallCycle.EditValue = CycleCall;
            }

            if (barActivity.EditValue.ToString() == "1")
            {
                decimal PlanCall = 0;
                decimal CycleCall = 0;

                foreach (DataRow dr in tblPlanningData.Rows)
                {
                    PlanCall = PlanCall + Convert.ToDecimal(dr["MonthlyTotalSQM"].ToString());
                    CycleCall = CycleCall + Convert.ToDecimal(dr["Metresadvance"].ToString());
                }

                barTxtTotalPlanCall.EditValue = Math.Round(PlanCall, 1);
                barTxtTotalCallCycle.EditValue = Math.Round(CycleCall, 1);
            }
        }

        public void LoadBoxholeList()
        {
            MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
            minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            minerData.SqlStatement = "select oreflowid BHID, Name BHName from tbl_OREFLOWENTITIES where oreflowcode = 'BH' ";

            var theResult = minerData.ExecuteInstruction();
            if (theResult.success) // check if the query was executed correctly 
            {
                tblBoxholeData = minerData.ResultsDataTable.Copy();
            }


            minerData.Dispose();
            minerData = null;

            reBoxholeStope.DataSource = tblBoxholeData;
            reBoxholeStope.DisplayMember = "BHName";
            reBoxholeStope.ValueMember = "BHID";

            reBoxholeDev.DataSource = tblBoxholeData;
            reBoxholeDev.DisplayMember = "BHName";
            reBoxholeDev.ValueMember = "BHID";
        }

        public void LoadMinerList()
        {
            MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
            minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            minerData.SqlStatement = "select distinct a.SECTIONID, Name from tbl_SectionComplete a inner join tbl_Code_Calendar_Section b on " +
                                     "A.PRODMONTH = B.Prodmonth AND " +
                                     "A.SBID = B.SectionID  " +
                                     "where A.prodmonth = '" + SelectProdmonth + "' and a.MOID = '" + SelectMOSectionID + "' ORDER BY a.Name";

            var theResult = minerData.ExecuteInstruction();
            if (theResult.success) // check if the query was executed correctly 
            {
                tblMinerListData = minerData.ResultsDataTable.Copy();
            }
            minerData.Dispose();
            minerData = null;

            reMinerSelectionStope.DataSource = tblMinerListData;
            reMinerSelectionStope.DisplayMember = "Name";
            reMinerSelectionStope.ValueMember = "SECTIONID";
            reMinerSelectionStope.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            //reMinerSelectionDev.DataSource = tblMinerListData;
            //reMinerSelectionDev.DisplayMember = "Name";
            //reMinerSelectionDev.ValueMember = "SECTIONID";
            //reMinerSelectionDev.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
        }

        public void LoadOrgUnits()
        {
            MWDataManager.clsDataAccess _OrgUnitsData = new MWDataManager.clsDataAccess();
            _OrgUnitsData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _OrgUnitsData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _OrgUnitsData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _OrgUnitsData.SqlStatement = "SELECT 1 thepos,'' CrewOrg, ''GangNo \r\n" +
                                         " UNION \r\n" +
                                         " SELECT 2 thepos,'Contractor'+':'+'0' CrewOrg, '0'GangNo \r\n" +
                                         " UNION \r\n" +
                                         "SELECT 3 thepos, OrgUnit+':'+ GangName CrewOrg,OrgUnit GangNo  FROM tbl_Orgunits  ";
            _OrgUnitsData.ExecuteInstruction();

            tblOrgUnitsData = _OrgUnitsData.ResultsDataTable.Copy();

            #region Stoping
            reOrgDaySelectionStope.DataSource = tblOrgUnitsData;
            reOrgDaySelectionStope.DisplayMember = "Crew_Org";
            reOrgDaySelectionStope.ValueMember = "GangNo";
            reOrgDaySelectionStope.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            reOrgNightSelectionStope.DataSource = tblOrgUnitsData;
            reOrgNightSelectionStope.DisplayMember = "Crew_Org";
            reOrgNightSelectionStope.ValueMember = "GangNo";
            reOrgNightSelectionStope.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            reOrgAfternoonStope.DataSource = tblOrgUnitsData;
            reOrgAfternoonStope.DisplayMember = "Crew_Org";
            reOrgAfternoonStope.ValueMember = "GangNo";
            reOrgAfternoonStope.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            reOrgRovingStope.DataSource = tblOrgUnitsData;
            reOrgRovingStope.DisplayMember = "Crew_Org";
            reOrgRovingStope.ValueMember = "GangNo";
            reOrgRovingStope.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            #endregion

            #region Development
            //reOrgDaySelectionDev.DataSource = tblOrgUnitsData;
            //reOrgDaySelectionDev.DisplayMember = "Crew_Org";
            //reOrgDaySelectionDev.ValueMember = "GangNo";
            //reOrgDaySelectionDev.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            reOrgNightSelectionDev.DataSource = tblOrgUnitsData;
            reOrgNightSelectionDev.DisplayMember = "Crew_Org";
            reOrgNightSelectionDev.ValueMember = "GangNo";
            reOrgNightSelectionDev.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            reOrgAfternoonDev.DataSource = tblOrgUnitsData;
            reOrgAfternoonDev.DisplayMember = "Crew_Org";
            reOrgAfternoonDev.ValueMember = "GangNo";
            reOrgAfternoonDev.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

            reOrgRovingDev.DataSource = tblOrgUnitsData;
            reOrgRovingDev.DisplayMember = "Crew_Org";
            reOrgRovingDev.ValueMember = "GangNo";
            reOrgRovingDev.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            #endregion
        }

        private void LoadCadsPlan()
        {
            MWDataManager.clsDataAccess CadsData = new MWDataManager.clsDataAccess();
            CadsData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            CadsData.SqlStatement = " Select cads.Workplaceid,w.Description workplaceDesc \r\n" +
                                    " ,case when CadCrew.Sectionid = 'NOT ASSIGNED' then 'N' else 'Y' end as CanImport \r\n" +
                                    " from tbl_PlanMonth_Cads cads \r\n" +
                                    " left outer join( \r\n" +
                                    " Select pm.Prodmonth, pm.Workplaceid, pm.Activity, sec.SECTIONID_2 \r\n" +
                                    " from tbl_PlanMonth pm, Sections_Complete sec \r\n" +
                                    " where pm.Prodmonth = sec.Prodmonth and pm.Sectionid = sec.SECTIONID \r\n" +
                                    " ) planned on cads.Prodmonth = planned.Prodmonth \r\n" +
                                    " and cads.Workplaceid = planned.Workplaceid \r\n" +
                                    " and cads.Activity = planned.Activity \r\n" +
                                    " and cads.MOSectionid = planned.SECTIONID_2 \r\n" +
                                    " left outer join tbl_Workplace w on cads.Workplaceid = w.WorkplaceID and cads.Activity = w.Activity \r\n" +
                                    " left outer join tbl_PlanMonth_Cads_Crew CadCrew on cads.Prodmonth = CadCrew.Prodmonth \r\n" +
                                    " and cads.Workplaceid = CadCrew.Workplaceid \r\n" +
                                    " where cads.Prodmonth = '" + SelectProdmonth + "' \r\n" +
                                    " and cads.MOSectionid = '" + SelectMOSectionID + "' and cads.activity = '" + SelectActivity + "' and planned.Prodmonth is null";

            CadsData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            CadsData.queryReturnType = MWDataManager.ReturnType.DataTable;
            var theResult = CadsData.ExecuteInstruction();

            DataTable dtCadsInfo = new DataTable();
            dtCadsInfo = CadsData.ResultsDataTable;

            gcImportedWp.DataSource = null;
            gcImportedWp.DataSource = dtCadsInfo;

            colImportWorkplaceID.FieldName = "Workplaceid";
            colImportWorkplaceDesc.FieldName = "workplaceDesc";
            colCanImport.FieldName = "CanImport";
        }

        private void LoadPlanned()
        {
            MainGridStope.Visible = false;
            MainGrid.Visible = false;
            MainGrid.Dock = DockStyle.None;
            MainGridStope.Dock = DockStyle.None;

            colnewOrgunitDisplay.UnGroup();

            if (barActivity.EditValue.ToString() != "1")
            {
                MainGridStope.Visible = true;
                MainGridStope.Dock = DockStyle.Fill;

                MWDataManager.clsDataAccess CadsData = new MWDataManager.clsDataAccess();
                CadsData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                CadsData.SqlStatement = "exec sp_Planning_Load_Stope '" + SelectProdmonth + "' , '" + SelectMOSectionID + "' ,'" + SelectActivity + "'  ";

                CadsData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                CadsData.queryReturnType = MWDataManager.ReturnType.DataTable;
                var theResult = CadsData.ExecuteInstruction();

                tblPlanningData = CadsData.ResultsDataTable.Copy();

                MainGridStope.DataSource = null;
                MainGridStope.DataSource = tblPlanningData;

                MWDataManager.clsDataAccess CycleData = new MWDataManager.clsDataAccess();
                CycleData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                CycleData.SqlStatement = "exec sp_Planning_Load_Stope_Cycle '" + SelectProdmonth + "' , '" + SelectMOSectionID + "' ,'" + SelectActivity + "'  ";

                CycleData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                CycleData.queryReturnType = MWDataManager.ReturnType.DataTable;
                var theResult1 = CycleData.ExecuteInstruction();

                tblPlanningCycleData = CycleData.ResultsDataTable.Copy();

                gcNewCycle.Visible = false;
                gcNewCycle.DataSource = null;
                gcNewCycle.DataSource = tblPlanningCycleData;
                gcNewCycle.Visible = true;
            }

            if (barActivity.EditValue.ToString() == "1")
            {
                MainGrid.Visible = true;
                MainGrid.Dock = DockStyle.Fill;

                MWDataManager.clsDataAccess CadsData = new MWDataManager.clsDataAccess();
                CadsData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                CadsData.SqlStatement = "exec sp_Planning_Load_Dev '" + SelectProdmonth + "' , '" + SelectMOSectionID + "' ,'" + SelectActivity + "'  ";

                CadsData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                CadsData.queryReturnType = MWDataManager.ReturnType.DataTable;
                var theResult = CadsData.ExecuteInstruction();

                tblPlanningData = CadsData.ResultsDataTable.Copy();

                MainGrid.DataSource = null;
                MainGrid.DataSource = tblPlanningData;

                MWDataManager.clsDataAccess CycleData = new MWDataManager.clsDataAccess();
                CycleData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                CycleData.SqlStatement = "exec sp_Planning_Load_Dev_Cycle '" + SelectProdmonth + "' , '" + SelectMOSectionID + "' ,'" + SelectActivity + "'  ";

                CycleData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                CycleData.queryReturnType = MWDataManager.ReturnType.DataTable;
                var theResult1 = CycleData.ExecuteInstruction();

                tblPlanningCycleData = CycleData.ResultsDataTable.Copy();

                gcNewCycle.Visible = false;
                gcNewCycle.DataSource = null;
                gcNewCycle.DataSource = tblPlanningCycleData;
                gcNewCycle.Visible = true;
            }


            if (SelectActivity != "1")
            {
                colNewCadsSqm.ColumnEdit = reSeSqm;
                colNewCycleSqm.ColumnEdit = reSeSqm;
            }
            else
            {
                colNewCadsSqm.ColumnEdit = reSeMAdv;
                colNewCycleSqm.ColumnEdit = reSeMAdv;
            }

            if (tblPlanningCycleData.Rows.Count == 0)
            {
                return;
            }

            if (tblPlanningCycleData.Rows.Count > 0)
            {
                DateTime startdate = Convert.ToDateTime(tblPlanningCycleData.Rows[0]["BeginDate"].ToString());
                
                int columnIndex = 6;

                //Headers Date
                for (int i = 0; i < 66; i++)
                {
                    gvNewCycle.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM yyyy");

                    startdate = startdate.AddDays(1);
                    
                    columnIndex++;
                }
            }

            WorkingDays();

            string _sqlWhere = "sectionid = '" + tblPlanningCycleData.Rows[0]["sb"].ToString() + "'";
            string _sqlOrder = "ShiftDay asc";

            DataTable dtGridSetup = tblPlanningWorkingDays.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

            SetVisibleIndex();

            if (tblPlanningCycleData.Rows.Count > 0)
            {
                for (int i = 6; i < gvNewCycle.Columns.Count; i++)
                {
                    string val = gvNewCycle.Columns[i].Caption;

                    if (i < 71)
                    {
                        gvNewCycle.Columns[i].Visible = true;
                        if (Convert.ToDateTime(val) > Convert.ToDateTime(tblPlanningCycleData.Rows[0]["EndDate"]))
                        {
                            gvNewCycle.Columns[i].Visible = false;
                        }
                    }
                    else
                    {
                        gvNewCycle.Columns[i].Visible = false;
                    }
                }

                int WDCol = 0;

                for (int Gridrow = 0; Gridrow < gvNewCycle.RowCount; Gridrow++)
                {
                    foreach (DataRow dr in tblPlanningCycleData.Rows)
                    {
                        string currWpID = gvNewCycle.GetRowCellValue(Gridrow, gvNewCycle.Columns["workplaceid"]).ToString();

                        if (dr["workplaceid"].ToString() == currWpID)
                        {
                            if (string.IsNullOrEmpty(dr["mocycle"].ToString()))
                            {
                                for (int Gridcol = 6; Gridcol < gvNewCycle.Columns.Count; Gridcol++)
                                {
                                    if (Gridcol < dtGridSetup.Rows.Count + 6)
                                    {
                                        string IsWd = dtGridSetup.Rows[Gridcol - 6]["workingday"].ToString();

                                        if (IsWd == "N")
                                        {
                                            gvNewCycle.SetRowCellValue(Gridrow, gvNewCycle.Columns[Gridcol], "OFF ");
                                        }
                                        else
                                        {
                                            string DefCycleCode = gvNewCycle.GetRowCellValue(Gridrow, gvNewCycle.Columns[Gridcol + 65]).ToString();
                                            gvNewCycle.SetRowCellValue(Gridrow, gvNewCycle.Columns[Gridcol], DefCycleCode);
                                        }
                                    }
                                }

                                gvNewCycle.FocusedRowHandle = Gridrow;
                                NewCalcSqm();
                            }
                        }
                    }
                }

                for (int Gridrow = 0; Gridrow < gvNewCycle.RowCount; Gridrow++)
                {
                    for (int Gridcol = 6; Gridcol < gvNewCycle.Columns.Count; Gridcol++)
                    {
                        if (Gridcol < dtGridSetup.Rows.Count + 6)
                        {
                            string calDate = dtGridSetup.Rows[Gridcol - 6]["calendardate"].ToString();
                            string IsWd = dtGridSetup.Rows[Gridcol - 6]["workingday"].ToString();

                            if (IsWd == "N")
                            {
                                gvNewCycle.SetRowCellValue(Gridrow, gvNewCycle.Columns[Gridcol], "OFF ");
                            }
                        }
                    }
                }

                colnewOrgunitDisplay.Group();
                gvNewCycle.ExpandAllGroups();
            }
        }

        private void SetVisibleIndex()
        {
            colNewDay29.VisibleIndex = 33;
            colNewDay30.VisibleIndex = 34;
            colNewDay31.VisibleIndex = 35;
            colNewDay32.VisibleIndex = 36;
            colNewDay33.VisibleIndex = 37;
            colNewDay34.VisibleIndex = 38;
            colNewDay35.VisibleIndex = 39;
            colNewDay36.VisibleIndex = 40;
            colNewDay37.VisibleIndex = 41;
            colNewDay38.VisibleIndex = 42;
            colNewDay39.VisibleIndex = 43;
            colNewDay40.VisibleIndex = 45;
            colNewDay41.VisibleIndex = 46;
            colNewDay42.VisibleIndex = 47;
            colNewDay43.VisibleIndex = 48;
            colNewDay44.VisibleIndex = 49;
            colNewDay45.VisibleIndex = 50;
            colNewDay46.VisibleIndex = 51;
            colNewDay47.VisibleIndex = 52;
            colNewDay48.VisibleIndex = 53;
            colNewDay49.VisibleIndex = 54;
            colNewDay50.VisibleIndex = 55;
            colNewDay51.VisibleIndex = 56;
            colNewDay52.VisibleIndex = 57;
            colNewDay53.VisibleIndex = 58;
            colNewDay54.VisibleIndex = 59;
            colNewDay55.VisibleIndex = 60;
            colNewDay56.VisibleIndex = 61;
            colNewDay57.VisibleIndex = 62;
            colNewDay58.VisibleIndex = 63;
            colNewDay59.VisibleIndex = 64;
            colNewDay60.VisibleIndex = 65;
            colNewDay61.VisibleIndex = 66;
            colNewDay62.VisibleIndex = 67;
            colNewDay63.VisibleIndex = 68;
            colNewDay64.VisibleIndex = 69;
            colNewDay65.VisibleIndex = 70;
        }



        private void barbtnCyclePlanning_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SelectActivity == "1")
            {
                ShowDevCycle(barbtnCyclePlanning.Down);
            }

            if (SelectActivity == "0")
            {
                ShowStopeCycle(barbtnCyclePlanning.Down);
            }
        }



        public void ShowStopeCycle(bool show)
        {
            viewPlanningStoping.PostEditor();

            if (tblPlanningData.Rows.Count == 0)
            {
                return;
            }

            if (viewPlanningStoping.FocusedRowHandle == -1)
            {
                MessageBox.Show("Please select a workplace before trying to cycle");

                pcCycle.Visible = false;
                return;
            }

            if (tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["SectionID"].ToString() == "-1"
                || tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["SectionID"].ToString() == SelectMOSectionID)
            {
                MessageBox.Show("Please select a Miner before trying to cycle");
                pcCycle.Visible = false;
                return;
            }

            pcCycle.Visible = show;

            if (show == false)
            {
                return;
            }

            editMonthPlan.Text = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["MonthlyTotalSQM"].ToString())), 2).ToString();

            //Update workingDaystable for new workpalces
            WorkingDays();

            string _sqlWhere = "sectionid = '" + tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["ShiftBossID"].ToString() + "'";
            string _sqlOrder = "ShiftDay asc";

            DataTable dtGridSetup = tblPlanningWorkingDays.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

            //StartdateofDay
            int columnIndex = 2;

            //Apply dates 
            for (int i = 0; i < dtGridSetup.Rows.Count; i++)
            {
                gvCyclePlan.Columns[columnIndex].Caption = Convert.ToDateTime(dtGridSetup.Rows[i]["calendardate"].ToString()).ToString("dd MMM yyyy");
                columnIndex++;
            }

            //Hide coulmuns that aren't needed
            for (int i = 0; i < gvCyclePlan.Columns.Count; i++)
            {
                if (i >= columnIndex)
                {
                    gvCyclePlan.Columns[i].Visible = false;
                }
            }

            string _sqlWhereCyc = "WorkplaceID = '" + tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["WorkplaceID"].ToString() + "'";
            string _sqlOrderCyc = "WorkplaceID asc";

            //Initial Data take on being re-used
            DataTable dtCycles = tblPlanningData.Select(_sqlWhereCyc, _sqlOrderCyc).CopyToDataTable();

            //Get Mo Cycle from MainLoad 
            string MoCycle = dtCycles.Rows[0]["mocycle"].ToString().Replace("OFF ", string.Empty) + "                                                                                                                                                                                                                                                   ";

            string MocycTrimmed = string.Empty;

            if (MoCycle.Trim().Length > 1)
            {
                MocycTrimmed = MoCycle.Trim().Substring(0, MoCycle.Trim().Length);
            }

            if (MoCycle.Trim().Length == 0 || MocycTrimmed == "end")
            {
                MoCycle = dtCycles.Rows[0]["DefaultOrig"].ToString().Replace("OFF ", string.Empty) + "                                                                                                                                                                                                                                                   ";
            }

            //To show Changed Cycle 
            if (tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Changed"].ToString() == "Y")
            {
                MoCycle = tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["mocycle"].ToString().Replace("OFF ", string.Empty);
            }

            //Get Mo Cycle from MainLoad 
            string DefaultCycle = dtCycles.Rows[0]["DefaultOrig"].ToString().Replace("OFF ", string.Empty) + "                                                                                                                                                                                                                                                   ";

            //Data Table to Act as Datasource for CycleGrid
            DataTable dtCyclePlan = new DataTable();


            DataColumn dc = new DataColumn("detail", typeof(String));
            dtCyclePlan.Columns.Add(dc);

            dc = new DataColumn("Total", typeof(String));
            dtCyclePlan.Columns.Add(dc);

            for (int day = 1; day <= 60; day++)
            {
                dc = new DataColumn("Day" + day, typeof(String));
                dtCyclePlan.Columns.Add(dc);
            }

            for (int i = 0; i < 4; i++)
            {
                DataRow dr = dtCyclePlan.NewRow();

                if (i == 0)
                {
                    dr[0] = "Std Cycle";
                }

                if (i == 2)
                {
                    dr[0] = "Crew Cycle";
                }

                if (i == 1 || i == 3)
                {
                    dr[0] = "Adv";
                }

                dr[1] = string.Empty;

                int columnDayIndex = 2;

                int CycleIndex = 0;

                for (int wrDay = 0; wrDay < dtGridSetup.Rows.Count; wrDay++)
                {
                    if (dtGridSetup.Rows[wrDay]["workingday"].ToString() == "N")
                    {
                        dr[columnDayIndex] = "OFF ";
                    }
                    else
                    {
                        if (i == 0)
                        {
                            dr[columnDayIndex] = DefaultCycle.Substring(CycleIndex, 4);
                            CycleIndex = CycleIndex + 4;
                        }

                        if (i == 2)
                        {
                            dr[columnDayIndex] = MoCycle.Substring(CycleIndex, 4);
                            CycleIndex = CycleIndex + 4;
                        }
                    }

                    columnDayIndex++;
                }

                dtCyclePlan.Rows.Add(dr);
            }

            gcCyclePlan.DataSource = dtCyclePlan;

            ColDesc.FieldName = "detail";

            ColDay1.FieldName = "Day1";
            ColDay2.FieldName = "Day2";
            ColDay3.FieldName = "Day3";
            ColDay4.FieldName = "Day4";
            ColDay5.FieldName = "Day5";
            ColDay6.FieldName = "Day6";
            ColDay7.FieldName = "Day7";
            ColDay8.FieldName = "Day8";
            ColDay9.FieldName = "Day9";
            ColDay10.FieldName = "Day10";

            ColDay11.FieldName = "Day11";
            ColDay12.FieldName = "Day12";
            ColDay13.FieldName = "Day13";
            ColDay14.FieldName = "Day14";
            ColDay15.FieldName = "Day15";
            ColDay16.FieldName = "Day16";
            ColDay17.FieldName = "Day17";
            ColDay18.FieldName = "Day18";
            ColDay19.FieldName = "Day19";
            ColDay20.FieldName = "Day20";

            ColDay21.FieldName = "Day21";
            ColDay22.FieldName = "Day22";
            ColDay23.FieldName = "Day23";
            ColDay24.FieldName = "Day24";
            ColDay25.FieldName = "Day25";
            ColDay26.FieldName = "Day26";
            ColDay27.FieldName = "Day27";
            ColDay28.FieldName = "Day28";
            ColDay29.FieldName = "Day29";
            ColDay30.FieldName = "Day30";

            ColDay31.FieldName = "Day31";
            ColDay32.FieldName = "Day32";
            ColDay33.FieldName = "Day33";
            ColDay34.FieldName = "Day34";
            ColDay35.FieldName = "Day35";
            ColDay36.FieldName = "Day36";
            ColDay37.FieldName = "Day37";
            ColDay38.FieldName = "Day38";
            ColDay39.FieldName = "Day39";
            ColDay40.FieldName = "Day40";

            ColDay41.FieldName = "Day41";
            ColDay42.FieldName = "Day42";
            ColDay43.FieldName = "Day43";
            ColDay44.FieldName = "Day44";
            ColDay45.FieldName = "Day45";
            ColDay46.FieldName = "Day46";
            ColDay47.FieldName = "Day47";
            ColDay48.FieldName = "Day48";
            ColDay49.FieldName = "Day49";
            ColDay50.FieldName = "Day50";

            ColDay51.FieldName = "Day51";
            ColDay52.FieldName = "Day52";
            ColDay53.FieldName = "Day53";
            ColDay54.FieldName = "Day54";
            ColDay55.FieldName = "Day55";
            ColDay56.FieldName = "Day56";
            ColDay57.FieldName = "Day57";
            ColDay58.FieldName = "Day58";
            ColDay59.FieldName = "Day59";
            ColDay60.FieldName = "Day60";

            decimal dailycallsqm = 0;

            if (!string.IsNullOrEmpty(tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["AdvBlast"].ToString()))
            {
                decimal CalcFL = Convert.ToDecimal(tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["FL"].ToString());
                dailycallsqm = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["AdvBlast"].ToString())) * CalcFL, 0); ;
            }

            decimal MoProgsqm = 0;

            int blast = 0;

            for (int i = 2; i < 60; i++)
            {
                if (gvCyclePlan.GetRowCellValue(0, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "BL" || gvCyclePlan.GetRowCellValue(0, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "SUBL")
                {
                    if (MoProgsqm < Convert.ToDecimal(editMonthPlan.Text))
                    {
                        blast = blast + 1;

                        gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], dailycallsqm.ToString());
                        MoProgsqm = MoProgsqm + dailycallsqm;
                    }
                    else
                    {
                        gvCyclePlan.SetRowCellValue(0, gvCyclePlan.Columns[i], string.Empty);
                        gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], string.Empty);
                    }
                }
                else if (gvCyclePlan.GetRowCellValue(0, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "SR")
                {
                    if (MoProgsqm < Convert.ToDecimal(editMonthPlan.Text))
                    {
                        blast = blast + 1;

                        gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], Convert.ToString(Math.Floor(dailycallsqm / 2)));
                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm / 2));
                    }
                    else
                    {
                        gvCyclePlan.SetRowCellValue(0, gvCyclePlan.Columns[i], string.Empty);
                        gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], string.Empty);
                    }
                }
                else if (gvCyclePlan.GetRowCellValue(0, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "OFF")
                {
                    gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], "OFF ");
                }
                else
                {
                    gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], string.Empty);
                }
            }

            CalcSqm();
        }

        public void ShowDevCycle(bool show)
        {
            viewPlanningDev.PostEditor();

            if (tblPlanningData.Rows.Count == 0)
            {
                return;
            }

            if (viewPlanningDev.FocusedRowHandle == -1)
            {
                MessageBox.Show("Please select a workplace before trying to cycle");
                pcCycle.Visible = false;
                return;
            }



            if (tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["SectionID"].ToString() == "-1"
                || tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["SectionID"].ToString() == SelectMOSectionID)
            {
                MessageBox.Show("Please select a Miner before trying to cycle");
                pcCycle.Visible = false;

                return;
            }

            pcCycle.Visible = show;

            if (show == false)
            {
                return;
            }

            editMonthPlan.Text = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["MonthlyTotalSQM"].ToString())), 2).ToString();

            //Update workingDaystable for new workpalces
            WorkingDays();

            string _sqlWhere = "sectionid = '" + tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["ShiftBossID"].ToString() + "'";
            string _sqlOrder = "ShiftDay asc";

            DataTable dtGridSetup = tblPlanningWorkingDays.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

            //StartdateofDay
            int columnIndex = 2;

            //Apply dates 
            for (int i = 0; i < dtGridSetup.Rows.Count; i++)
            {
                gvCyclePlan.Columns[columnIndex].Caption = Convert.ToDateTime(dtGridSetup.Rows[i]["calendardate"].ToString()).ToString("dd MMM yyyy");
                columnIndex++;
            }

            //Hide coulmuns that aren't needed
            for (int i = 0; i < gvCyclePlan.Columns.Count; i++)
            {
                if (i >= columnIndex)
                {
                    gvCyclePlan.Columns[i].Visible = false;
                }
            }

            string _sqlWhereCyc = "workplaceid = '" + tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Workplaceid"].ToString() + "'";
            string _sqlOrderCyc = "workplaceid asc";

            //Initial Data take on being re-used
            DataTable dtCycles = tblPlanningData.Select(_sqlWhereCyc, _sqlOrderCyc).CopyToDataTable();

            //Get Mo Cycle from MainLoad 
            string MoCycle = dtCycles.Rows[0]["mocycle"].ToString().Replace("OFF ", string.Empty) + "                                                                                                                                                                                                                                                   ";

            string MocycTrimmed = string.Empty;

            if (MoCycle.Trim().Length > 1)
            {
                MocycTrimmed = MoCycle.Trim().Substring(0, MoCycle.Trim().Length);
            }

            if (MoCycle.Trim().Length == 0 || MocycTrimmed == "end")
            {
                MoCycle = dtCycles.Rows[0]["DefaultOrig"].ToString().Replace("OFF ", string.Empty) + "                                                                                                                                                                                                                                                   ";
            }

            //To show Changed Cycle 
            if (tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Changed"].ToString() == "Y")
            {
                MoCycle = tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["mocycle"].ToString().Replace("OFF ", string.Empty);
            }

            //Get Mo Cycle from MainLoad 
            string DefaultCycle = dtCycles.Rows[0]["DefaultOrig"].ToString().Replace("OFF ", string.Empty) + "                                                                                                                                                                                                                                                   ";

            //Data Table to Act as Datasource for CycleGrid
            DataTable dtCyclePlan = new DataTable();


            DataColumn dc = new DataColumn("detail", typeof(String));
            dtCyclePlan.Columns.Add(dc);

            dc = new DataColumn("Total", typeof(String));
            dtCyclePlan.Columns.Add(dc);

            for (int day = 1; day <= 60; day++)
            {
                dc = new DataColumn("Day" + day, typeof(String));
                dtCyclePlan.Columns.Add(dc);
            }

            for (int i = 0; i < 4; i++)
            {
                DataRow dr = dtCyclePlan.NewRow();

                if (i == 0)
                {
                    dr[0] = "Std Cycle";
                }

                if (i == 2)
                {
                    dr[0] = "Crew Cycle";
                }

                if (i == 1 || i == 3)
                {
                    dr[0] = "Adv";
                }

                dr[1] = string.Empty;

                int columnDayIndex = 2;

                int CycleIndex = 0;

                for (int wrDay = 0; wrDay < dtGridSetup.Rows.Count; wrDay++)
                {
                    if (dtGridSetup.Rows[wrDay]["workingday"].ToString() == "N")
                    {
                        dr[columnDayIndex] = "OFF ";
                    }
                    else
                    {
                        if (i == 0)
                        {
                            dr[columnDayIndex] = DefaultCycle.Substring(CycleIndex, 4);
                            CycleIndex = CycleIndex + 4;
                        }

                        if (i == 2)
                        {
                            dr[columnDayIndex] = MoCycle.Substring(CycleIndex, 4);
                            CycleIndex = CycleIndex + 4;
                        }
                    }

                    columnDayIndex++;
                }

                dtCyclePlan.Rows.Add(dr);
            }

            gcCyclePlan.DataSource = dtCyclePlan;

            ColDesc.FieldName = "detail";

            ColDay1.FieldName = "Day1";
            ColDay2.FieldName = "Day2";
            ColDay3.FieldName = "Day3";
            ColDay4.FieldName = "Day4";
            ColDay5.FieldName = "Day5";
            ColDay6.FieldName = "Day6";
            ColDay7.FieldName = "Day7";
            ColDay8.FieldName = "Day8";
            ColDay9.FieldName = "Day9";
            ColDay10.FieldName = "Day10";

            ColDay11.FieldName = "Day11";
            ColDay12.FieldName = "Day12";
            ColDay13.FieldName = "Day13";
            ColDay14.FieldName = "Day14";
            ColDay15.FieldName = "Day15";
            ColDay16.FieldName = "Day16";
            ColDay17.FieldName = "Day17";
            ColDay18.FieldName = "Day18";
            ColDay19.FieldName = "Day19";
            ColDay20.FieldName = "Day20";

            ColDay21.FieldName = "Day21";
            ColDay22.FieldName = "Day22";
            ColDay23.FieldName = "Day23";
            ColDay24.FieldName = "Day24";
            ColDay25.FieldName = "Day25";
            ColDay26.FieldName = "Day26";
            ColDay27.FieldName = "Day27";
            ColDay28.FieldName = "Day28";
            ColDay29.FieldName = "Day29";
            ColDay30.FieldName = "Day30";

            ColDay31.FieldName = "Day31";
            ColDay32.FieldName = "Day32";
            ColDay33.FieldName = "Day33";
            ColDay34.FieldName = "Day34";
            ColDay35.FieldName = "Day35";
            ColDay36.FieldName = "Day36";
            ColDay37.FieldName = "Day37";
            ColDay38.FieldName = "Day38";
            ColDay39.FieldName = "Day39";
            ColDay40.FieldName = "Day40";

            ColDay41.FieldName = "Day41";
            ColDay42.FieldName = "Day42";
            ColDay43.FieldName = "Day43";
            ColDay44.FieldName = "Day44";
            ColDay45.FieldName = "Day45";
            ColDay46.FieldName = "Day46";
            ColDay47.FieldName = "Day47";
            ColDay48.FieldName = "Day48";
            ColDay49.FieldName = "Day49";
            ColDay50.FieldName = "Day50";

            ColDay51.FieldName = "Day51";
            ColDay52.FieldName = "Day52";
            ColDay53.FieldName = "Day53";
            ColDay54.FieldName = "Day54";
            ColDay55.FieldName = "Day55";
            ColDay56.FieldName = "Day56";
            ColDay57.FieldName = "Day57";
            ColDay58.FieldName = "Day58";
            ColDay59.FieldName = "Day59";
            ColDay60.FieldName = "Day60";

            decimal dailycallsqm = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["AdvBlast"].ToString())), 2);

            decimal MoProgsqm = 0;

            int blast = 0;

            for (int i = 2; i < 60; i++)
            {
                if (gvCyclePlan.GetRowCellValue(0, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "BL" || gvCyclePlan.GetRowCellValue(0, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "SUBL")
                {
                    if (MoProgsqm < Convert.ToDecimal(editMonthPlan.Text))
                    {
                        blast = blast + 1;

                        gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], dailycallsqm.ToString());
                        MoProgsqm = MoProgsqm + dailycallsqm;
                    }
                    else
                    {
                        gvCyclePlan.SetRowCellValue(0, gvCyclePlan.Columns[i], string.Empty);
                        gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], string.Empty);
                    }
                }
                else if (gvCyclePlan.GetRowCellValue(0, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "SR")
                {
                    if (MoProgsqm < Convert.ToDecimal(editMonthPlan.Text))
                    {
                        blast = blast + 1;

                        gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], Convert.ToString(Math.Floor(dailycallsqm / 2)));
                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm / 2));
                    }
                    else
                    {
                        gvCyclePlan.SetRowCellValue(0, gvCyclePlan.Columns[i], string.Empty);
                        gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], string.Empty);
                    }
                }
                else if (gvCyclePlan.GetRowCellValue(0, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "OFF")
                {
                    gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], "OFF ");
                }
                else
                {
                    gvCyclePlan.SetRowCellValue(1, gvCyclePlan.Columns[i], string.Empty);
                }
            }

            CalcSqm();
        }

        public void WorkingDays()
        {
            MWDataManager.clsDataAccess _PlanningDevWorkingDay = new MWDataManager.clsDataAccess();
            _PlanningDevWorkingDay.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _PlanningDevWorkingDay.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
            _PlanningDevWorkingDay.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PlanningDevWorkingDay.SqlStatement = "sp_Planning_WorkingDays";

            SqlParameter[] _paramCollection =
            {
                    _PlanningDevWorkingDay.CreateParameter("@pm",SqlDbType.Int, 0,Convert.ToInt32(SelectProdmonth)),
                    _PlanningDevWorkingDay.CreateParameter("@mo",SqlDbType.VarChar, 20, SelectMOSectionID),
                };


            _PlanningDevWorkingDay.ParamCollection = _paramCollection;
            var result = _PlanningDevWorkingDay.ExecuteInstruction();
            tblPlanningWorkingDays = _PlanningDevWorkingDay.ResultsDataTable.Copy();
            _PlanningDevWorkingDay.Dispose();
            _PlanningDevWorkingDay = null;
        }

        private void NewCalcSqm()
        {
            int PlanningRow = 0;
            string WorkplaceID = gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns["workplaceid"]).ToString();

            for (int row = 0; row < tblPlanningData.Rows.Count; row++)
            {
                if (tblPlanningData.Rows[row]["Workplaceid"].ToString() == WorkplaceID)
                {
                    PlanningRow = row;
                }
            }

            decimal dailycallsqm = 0;

            if (SelectActivity == "1")
            {
                dailycallsqm = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[PlanningRow]["AdvBlast"].ToString())), 2);
            }

            if (SelectActivity != "1")
            {
                decimal CalcFL = Convert.ToDecimal(tblPlanningData.Rows[PlanningRow]["FL"].ToString());
                if (tblPlanningData.Rows[PlanningRow]["AdvBlast"].ToString() != string.Empty)
                {
                    dailycallsqm = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[PlanningRow]["AdvBlast"].ToString())) * CalcFL, 0);
                }
            }

            MoCycleNumNew = string.Empty;

            decimal MoProgsqm = 0;

            int blast = 0;

            for (int i = 6; i < 71; i++)
            {
                if (gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns[i]).ToString().TrimEnd() == "BL" || gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns[i]).ToString().TrimEnd() == "SUBL")
                {
                    blast = blast + 1;
                    MoCycleNumNew = MoCycleNumNew + dailycallsqm.ToString();
                    MoProgsqm = MoProgsqm + dailycallsqm;
                }
                else
                {
                    if (gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns[i]).ToString().TrimEnd() == "OFF")
                    {
                        MoCycleNumNew = MoCycleNumNew + "OFF ";
                    }
                    else
                    {
                        MoCycleNumNew = MoCycleNumNew + "    ";
                    }

                    if (gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns[i]).ToString().TrimEnd() == "SR")
                    {
                        blast = blast + 1;
                        MoCycleNumNew = MoCycleNumNew + Convert.ToString(Math.Floor(dailycallsqm / 2));
                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm / 2));
                    }

                    if (gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns[i]).ToString().TrimEnd() == "LR1")
                    {
                        blast = blast + 1;
                        MoCycleNumNew = MoCycleNumNew + Convert.ToString(Math.Floor(dailycallsqm * 2));
                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm * 2));
                    }

                    if (gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns[i]).ToString().TrimEnd() == "LR2")
                    {
                        blast = blast + 1;
                        MoCycleNumNew = MoCycleNumNew + Convert.ToString(Math.Floor(dailycallsqm * 3));
                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm * 3));
                    }

                    if (gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns[i]).ToString().TrimEnd() == "LR3")
                    {
                        blast = blast + 1;
                        MoCycleNumNew = MoCycleNumNew + Convert.ToString(Math.Floor(dailycallsqm * 4));
                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm * 4));
                    }
                }
            }

            editCycleCall.Text = MoProgsqm.ToString();
            gvNewCycle.SetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns["CycleSqm"], MoProgsqm.ToString());

            BuiltMoStringNew(PlanningRow);
        }

        private void CalcSqm()
        {
            decimal dailycallsqm = 0;

            if (SelectActivity == "1")
            {
                dailycallsqm = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["AdvBlast"].ToString())), 2);
            }

            if (SelectActivity != "1")
            {
                decimal CalcFL = Convert.ToDecimal(tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["FL"].ToString());
                if (tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["AdvBlast"].ToString() != string.Empty)
                {
                    dailycallsqm = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["AdvBlast"].ToString())) * CalcFL, 0);
                }
            }

            decimal MoProgsqm = 0;

            int blast = 0;

            for (int i = 2; i < 60; i++)
            {
                if (gvCyclePlan.GetRowCellValue(2, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "OFF")
                {
                    gvCyclePlan.SetRowCellValue(3, gvCyclePlan.Columns[i], "OFF ");
                }
                else
                {
                    gvCyclePlan.SetRowCellValue(3, gvCyclePlan.Columns[i], string.Empty);
                }


                if (gvCyclePlan.GetRowCellValue(2, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "BL" || gvCyclePlan.GetRowCellValue(2, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "SUBL")
                {
                    blast = blast + 1;

                    gvCyclePlan.SetRowCellValue(3, gvCyclePlan.Columns[i], dailycallsqm.ToString());

                    MoProgsqm = MoProgsqm + dailycallsqm;
                }
                else
                {
                    if (gvCyclePlan.GetRowCellValue(2, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "SR")
                    {
                        blast = blast + 1;

                        gvCyclePlan.SetRowCellValue(3, gvCyclePlan.Columns[i], Convert.ToString(Math.Floor(dailycallsqm / 2)));


                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm / 2));
                    }


                    if (gvCyclePlan.GetRowCellValue(2, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "LR1")
                    {
                        blast = blast + 1;

                        gvCyclePlan.SetRowCellValue(3, gvCyclePlan.Columns[i], Convert.ToString(Math.Floor(dailycallsqm * 2)));


                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm * 2));
                    }

                    if (gvCyclePlan.GetRowCellValue(2, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "LR2")
                    {
                        blast = blast + 1;

                        gvCyclePlan.SetRowCellValue(3, gvCyclePlan.Columns[i], Convert.ToString(Math.Floor(dailycallsqm * 3)));


                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm * 3));
                    }

                    if (gvCyclePlan.GetRowCellValue(2, gvCyclePlan.Columns[i]).ToString().TrimEnd() == "LR3")
                    {
                        blast = blast + 1;

                        gvCyclePlan.SetRowCellValue(3, gvCyclePlan.Columns[i], Convert.ToString(Math.Floor(dailycallsqm * 4)));


                        MoProgsqm = MoProgsqm + Convert.ToDecimal(Math.Floor(dailycallsqm * 4));
                    }



                }
            }

            editCycleCall.Text = MoProgsqm.ToString();

            BuiltMoString();
        }

        private void BuiltMoStringNew(int _mainRowHandle)
        {
            string SaveDefaultCycle = string.Empty;
            string SaveMoCycle = string.Empty;
            string SaveMoCycleNum = string.Empty;

            int Count = 0;

            for (int i = 6; i < 71; i++)
            {
                SaveDefaultCycle = SaveDefaultCycle + (gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns[i + 60].FieldName).ToString() + "       ").Substring(0, 4);
                SaveMoCycle = SaveMoCycle + (gvNewCycle.GetRowCellValue(gvNewCycle.FocusedRowHandle, gvNewCycle.Columns[i].FieldName).ToString() + "       ").Substring(0, 4);
                SaveMoCycleNum = MoCycleNumNew.Replace("OFF ", "    ");

                Count = Count + 1;
            }

            if (!string.IsNullOrEmpty(SaveMoCycle))
            {
                if (SelectActivity == "1")
                {
                    tblPlanningData.Rows[_mainRowHandle]["mocycle"] = SaveMoCycle;
                    tblPlanningData.Rows[_mainRowHandle]["mocyclenum"] = SaveMoCycleNum;
                    tblPlanningData.Rows[_mainRowHandle]["DefaultCycle"] = SaveDefaultCycle;
                    tblPlanningData.Rows[_mainRowHandle]["Changed"] = "Y";
                    tblPlanningData.Rows[_mainRowHandle]["Metresadvance"] = editCycleCall.Text;
                }

                if (SelectActivity != "1")
                {
                    tblPlanningData.Rows[_mainRowHandle]["mocycle"] = SaveMoCycle;
                    tblPlanningData.Rows[_mainRowHandle]["mocyclenum"] = SaveMoCycleNum;
                    tblPlanningData.Rows[_mainRowHandle]["DefaultCycle"] = SaveDefaultCycle;
                    tblPlanningData.Rows[_mainRowHandle]["Changed"] = "Y";
                    tblPlanningData.Rows[_mainRowHandle]["callValue"] = editCycleCall.Text;
                }
            }

            CalcSum();
        }

        private void BuiltMoString()
        {
            string SaveDefaultCycle = string.Empty;
            string SaveMoCycle = string.Empty;
            string SaveMoCycleNum = string.Empty;

            int Count = 0;

            for (int i = 2; i < 60; i++)
            {
                SaveDefaultCycle = SaveDefaultCycle + (gvCyclePlan.GetRowCellValue(0, gvCyclePlan.Columns[i].FieldName).ToString() + "       ").Substring(0, 4);
                SaveMoCycle = SaveMoCycle + (gvCyclePlan.GetRowCellValue(2, gvCyclePlan.Columns[i].FieldName).ToString() + "       ").Substring(0, 4);
                SaveMoCycleNum = SaveMoCycleNum + (gvCyclePlan.GetRowCellValue(3, gvCyclePlan.Columns[i].FieldName).ToString() + "       ").Substring(0, 4).Replace("OFF ", string.Empty);

                Count = Count + 1;
            }

            if (!string.IsNullOrEmpty(SaveMoCycle))
            {
                if (SelectActivity == "1")
                {
                    tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["mocycle"] = SaveMoCycle;
                    tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["MOCycleNum"] = SaveMoCycleNum;
                    tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["DefaultCycle"] = SaveDefaultCycle;
                    tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Changed"] = "Y";
                    tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Metresadvance"] = editCycleCall.Text;
                }

                if (SelectActivity != "1")
                {
                    tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["mocycle"] = SaveMoCycle;
                    tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["MOCycleNum"] = SaveMoCycleNum;
                    tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["DefaultCycle"] = SaveDefaultCycle;
                    tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Changed"] = "Y";
                    tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["callValue"] = editCycleCall.Text;
                }
            }
        }

        private void barbtnSavePlanning_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SelectActivity == "1")
            {
                SavePlanningDev();
            }

            if (SelectActivity != "1")
            {
                SavePlanningStope();
            }
        }

        public void SavePlanningDev()
        {
            string SQLInsertPLANNING_AUDIT = string.Empty;

            string WorkplaceFail = "N";//Used to give error message only once for user

            for (int i = 0; i < viewPlanningDev.RowCount; i++)
            {
                if (tblPlanningData.Rows[i]["Changed"].ToString() == "Y")
                {
                    string ViableforSave = "Y";//Check wheter if workplace can save if not just skips

                    if (tblPlanningData.Rows[i]["SectionID"].ToString() == "-1")
                    {
                        if (WorkplaceFail == "N")
                        {
                            MessageBox.Show("Workplace " + tblPlanningData.Rows[i]["WorkplaceDesc"].ToString() + " Did NOT SAVE : No Miner Selected");
                        }
                        ViableforSave = "N";
                        WorkplaceFail = "Y";
                    }

                    if (ViableforSave == "Y")
                    {
                        string _sqlWhere = "sectionid = '" + tblPlanningData.Rows[i]["ShiftBossID"].ToString() + "'";
                        string _sqlOrder = "ShiftDay asc";

                        DataTable dtGridSetup = tblPlanningWorkingDays.Select(_sqlWhere, _sqlOrder).CopyToDataTable();//Gets calendar

                        string Prodmonth = SelectProdmonth;
                        string SectionID = ExtractBeforeColon(tblPlanningData.Rows[i]["Sectionid"].ToString());
                        string WorkplaceID = tblPlanningData.Rows[i]["Workplaceid"].ToString();
                        string Activity = "1";
                        string Startdate = String.Format("{0:yyyy-MM-dd}", dtGridSetup.Rows[0]["begindate"]);
                        string BoxholeID = tblPlanningData.Rows[i]["BoxholeID"].ToString();

                        string MonthlyTotalSQM = tblPlanningData.Rows[i]["MonthlyTotalSQM"].ToString();

                        string orgunitDay = tblPlanningData.Rows[i]["OrgUnitDay"].ToString();

                        string PlanCodesDefault = tblPlanningData.Rows[i]["DefaultCycle"].ToString();
                        string PlanCodesMO = (tblPlanningData.Rows[i]["mocycle"].ToString() + "                                                                                                                                                                                                                                                                                         ").Substring(0, 260) + "End";
                        string PlanCodesMONum = (tblPlanningData.Rows[i]["mocyclenum"].ToString() + "                                                                                                                                                                                                                                                                                       ").Substring(0, 260);

                        string _sqlWhereOrg = "GangNo = '" + orgunitDay + "'";
                        string _sqlOrderOrg = "GangNo asc";

                        DataTable dtGridOrg = tblOrgUnitsData.Select(_sqlWhereOrg, _sqlOrderOrg).CopyToDataTable();//Gets orgunitName

                        orgunitDay = dtGridOrg.Rows[0]["CrewOrg"].ToString();

                        int blastCount = 0;

                        int substringindex = 0;

                        for (int CycCount = 0; CycCount < 65; CycCount++)
                        {
                            string currString = PlanCodesMO.Substring(substringindex, 4);

                            if (currString.TrimEnd() == "BL")
                            {
                                blastCount = blastCount + 1;
                            }
                            substringindex = substringindex + 4;
                        }

                        //Math.Round((Convert.ToDecimal(PlanningClass.tblPlanningData.Rows[viewPlanning.FocusedRowHandle]["AdvBlast"].ToString())), 2)

                        decimal BuiltAdvperblast = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[i]["AdvBlast"].ToString())), 2);

                        decimal EndWidth = Convert.ToDecimal(tblPlanningData.Rows[i]["ENDWIDTH"].ToString());
                        decimal EndHeight = Convert.ToDecimal(tblPlanningData.Rows[i]["ENDHEIGHT"].ToString());
                        decimal Density = Convert.ToDecimal(tblPlanningData.Rows[i]["Density"].ToString());
                        decimal CMGT = Convert.ToDecimal(tblPlanningData.Rows[i]["CMGT"].ToString());

                        decimal tons = BuiltAdvperblast * EndWidth * EndHeight * Density;
                        decimal content = BuiltAdvperblast * EndWidth * CMGT * Density / 100;

                        decimal wasteFact = 0;

                        //if (Convert.ToDecimal(tblPlanningData.Rows[i]["MonthlyTotalSQM"].ToString()) > 0)
                        //    wasteFact = Convert.ToDecimal(tblPlanningData.Rows[i]["MonthlyWatseSQM"].ToString()) / Convert.ToDecimal(tblPlanningData.Rows[i]["MonthlyTotalSQM"].ToString());

                        decimal wasteadv = Convert.ToDecimal(BuiltAdvperblast) * wasteFact;
                        decimal reefadv = Convert.ToDecimal(BuiltAdvperblast) - wasteadv;

                        decimal wastetons = Convert.ToDecimal(tons) * wasteFact;
                        decimal reeftons = Convert.ToDecimal(tons) - wastetons;

                        string Processed = "N";

                        decimal AdvPerBlastReef = reefadv;
                        decimal AdvPerBlastWaste = wasteadv;
                        decimal TonsPerBlastReef = reeftons;
                        decimal TonsPerBlastWaste = wastetons;
                        decimal NewEndWith = EndHeight;
                        decimal NewCmgt = CMGT;
                        decimal ContentPerBlast = Math.Round(content, 3);
                        string UserID = string.Empty;


                        UserID = UserCurrentInfo.UserID;

                        SQLInsertPLANNING_AUDIT = SQLInsertPLANNING_AUDIT +
                                                  "\r\n\r\n  insert into tbl_Planning_Audit_Dev values (   \r\n" +
                                                  "  '" + Prodmonth + "' , '" + SectionID + "' , '" + WorkplaceID + "', '" + Activity + "', '" + Startdate + "'  \r\n" +
                                                  "  , '" + AdvPerBlastReef + "', '" + AdvPerBlastWaste + "', '" + TonsPerBlastReef + "', '" + TonsPerBlastWaste + "', '" + NewEndWith + "' \r\n" +
                                                  "  , '" + NewCmgt + "', '" + ContentPerBlast + "', '" + PlanCodesDefault + "', '" + PlanCodesMO + "', 'N' \r\n" +
                                                  "  , '" + UserID + "', getdate(), '" + orgunitDay + "','0','0','0','" + MonthlyTotalSQM + "','" + PlanCodesMONum + "','" + BoxholeID + "') ";

                        tblPlanningData.Rows[i]["Changed"] = "N";
                    }

                }
            }


            if (SQLInsertPLANNING_AUDIT != string.Empty)
            {
                MWDataManager.clsDataAccess _PrePlanningCycleData = new MWDataManager.clsDataAccess();
                _PrePlanningCycleData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                _PrePlanningCycleData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _PrePlanningCycleData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _PrePlanningCycleData.SqlStatement = SQLInsertPLANNING_AUDIT + "\r\n\r\n exec sp_Planning_Dev_Changes";

                var result = _PrePlanningCycleData.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Planning saved", Color.CornflowerBlue);
                }
                else
                {

                }
            }
        }

        public void SavePlanningStope()
        {
            string SQLInsertPLANNING_AUDIT = string.Empty;

            string WorkplaceFail = "N";//Used to give error message only once for user

            for (int i = 0; i < viewPlanningStoping.RowCount; i++)
            {
                if (tblPlanningData.Rows[i]["Changed"].ToString() == "Y")
                {
                    string ViableforSave = "Y";//Check wheter if workplace can save if not just skips

                    if (tblPlanningData.Rows[i]["SectionID"].ToString() == "-1")
                    {
                        if (WorkplaceFail == "N")
                        {
                            MessageBox.Show("Workplace " + tblPlanningData.Rows[i]["WorkplaceDesc"].ToString() + " Did NOT SAVE : No Miner Selected");
                        }
                        ViableforSave = "N";
                        WorkplaceFail = "Y";
                    }

                    if (ViableforSave == "Y")
                    {
                        string _sqlWhere = "sectionid = '" + tblPlanningData.Rows[i]["ShiftBossID"].ToString() + "'";
                        string _sqlOrder = "ShiftDay asc";

                        DataTable dtGridSetup = tblPlanningWorkingDays.Select(_sqlWhere, _sqlOrder).CopyToDataTable();//Gets calendar

                        string Prodmonth = SelectProdmonth;
                        string SectionID = tblPlanningData.Rows[i]["Sectionid"].ToString();
                        string WorkplaceID = tblPlanningData.Rows[i]["Workplaceid"].ToString();
                        string Activity = SelectActivity;
                        string BoxholeID = tblPlanningData.Rows[i]["BoxholeID"].ToString();

                        decimal FL = Convert.ToDecimal(tblPlanningData.Rows[i]["FL"].ToString());
                        decimal SW = Convert.ToDecimal(tblPlanningData.Rows[i]["SW"].ToString());
                        decimal CW = 0;

                        string Startdate = String.Format("{0:yyyy-MM-dd}", dtGridSetup.Rows[0]["begindate"]);
                        string MonthlyReefSQM = "0";
                        string MonthlyWatseSQM = "0";
                        string MonthlyTotalSQM = tblPlanningData.Rows[i]["MonthlyTotalSQM"].ToString();

                        string orgunitDay = tblPlanningData.Rows[i]["OrgUnitDay"].ToString();
                        string TargetID = "-1";

                        string PlanCodesDefault = tblPlanningData.Rows[i]["DefaultCycle"].ToString();
                        string PlanCodesMO = (tblPlanningData.Rows[i]["mocycle"].ToString() + "                                                                                                                                                                                                                                                                          ").Substring(0, 260) + "End";
                        string PlanCodesMONum = (tblPlanningData.Rows[i]["mocyclenum"].ToString() + "                                                                                                                                                                                                                                                                       ").Substring(0, 260);


                        string _sqlWhereOrg = "GangNo = '" + orgunitDay + "'";
                        string _sqlOrderOrg = "GangNo asc";

                        DataTable dtGridOrg = tblOrgUnitsData.Select(_sqlWhereOrg, _sqlOrderOrg).CopyToDataTable();//Gets orgunitName

                        orgunitDay = dtGridOrg.Rows[0]["CrewOrg"].ToString();

                        int blastCount = 0;

                        int substringindex = 0;

                        for (int CycCount = 0; CycCount < 65; CycCount++)
                        {
                            string currString = PlanCodesMO.Substring(substringindex, 4);

                            if (currString.TrimEnd() == "BL")
                            {
                                blastCount = blastCount + 1;
                            }
                            substringindex = substringindex + 4;
                        }

                        decimal BuiltAdvperblast = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[i]["AdvBlast"].ToString())), 2);

                        decimal Density = Convert.ToDecimal(tblPlanningData.Rows[i]["Density"].ToString());
                        decimal CMGT = Convert.ToDecimal(tblPlanningData.Rows[i]["CMGT"].ToString());

                        decimal SQMperblast = BuiltAdvperblast * FL;

                        decimal tons = SQMperblast * SW / 100 * Density;
                        decimal content = SQMperblast * CMGT * Density / 100;

                        decimal wasteFact = 0;

                        //if (Convert.ToDecimal(tblPlanningData.Rows[i]["MonthlyWatseSQM"].ToString()) > 0)
                        //    wasteFact = Convert.ToDecimal(tblPlanningData.Rows[i]["MonthlyWatseSQM"].ToString()) / Convert.ToDecimal(tblPlanningData.Rows[i]["MonthlyTotalSQM"].ToString());

                        decimal wasteSqm = SQMperblast * wasteFact;
                        decimal ReefSqm = SQMperblast - wasteSqm;

                        decimal wasteadv = Convert.ToDecimal(BuiltAdvperblast) * wasteFact;
                        decimal reefadv = Convert.ToDecimal(BuiltAdvperblast) - wasteadv;

                        decimal wastetons = Convert.ToDecimal(tons) * wasteFact;
                        decimal reeftons = Convert.ToDecimal(tons) - wastetons;

                        string Processed = "N";

                        decimal AdvPerBlastReef = reefadv;
                        decimal AdvPerBlastWaste = wasteadv;
                        decimal TonsPerBlastReef = reeftons;
                        decimal TonsPerBlastWaste = wastetons;
                        //decimal NewEndWith = EndHeight;
                        decimal NewCmgt = CMGT;
                        decimal ContentPerBlast = content;
                        string UserID = string.Empty;


                        UserID = UserCurrentInfo.UserID;

                        SQLInsertPLANNING_AUDIT = SQLInsertPLANNING_AUDIT +
                                                  "\r\n\r\n  insert into tbl_Planning_Audit_Stope values (   \r\n" +
                                                  "  '" + Prodmonth + "' , '" + SectionID + "' , '" + WorkplaceID + "', '" + Activity + "', '" + Startdate + "'  \r\n" +
                                                  "  ,'" + ReefSqm + "','" + wasteSqm + "', '" + AdvPerBlastReef + "', '" + AdvPerBlastWaste + "', '" + TonsPerBlastReef + "', '" + TonsPerBlastWaste + "','" + FL + "','" + SW + "','" + CW + "' \r\n" +
                                                  "  , '" + NewCmgt + "', '" + ContentPerBlast + "', '" + PlanCodesDefault + "', '" + PlanCodesMO + "', 'N' \r\n" +
                                                  "  , '" + UserID + "', getdate(), '" + orgunitDay + "','" + TargetID + "','" + MonthlyReefSQM + "','" + MonthlyWatseSQM + "','" + MonthlyTotalSQM + "','" + PlanCodesMONum + "','" + BoxholeID + "') ";

                        tblPlanningData.Rows[i]["Changed"] = "N";
                    }
                }
            }


            if (SQLInsertPLANNING_AUDIT != string.Empty)
            {
                MWDataManager.clsDataAccess _PrePlanningCycleData = new MWDataManager.clsDataAccess();
                _PrePlanningCycleData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                _PrePlanningCycleData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _PrePlanningCycleData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _PrePlanningCycleData.SqlStatement = SQLInsertPLANNING_AUDIT + "\r\n\r\n exec sp_Planning_Stope_Changes";

                var result = _PrePlanningCycleData.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Planning saved", Color.CornflowerBlue);
                }
                else
                {

                }
            }
        }


        private void gvImportedWp_DoubleClick(object sender, EventArgs e)
        {
            if (gvImportedWp.FocusedRowHandle == -1)
            {
                return;
            }

            if (gvImportedWp.GetRowCellValue(gvImportedWp.FocusedRowHandle, gvImportedWp.Columns["CanImport"]).ToString() == "N")
            {
                MessageBox.Show("Can not Import this workplace,No Miner assigned from Cads");
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            string SelectedWorkplaceid = gvImportedWp.GetRowCellValue(gvImportedWp.FocusedRowHandle, gvImportedWp.Columns["Workplaceid"]).ToString();

            if (barActivity.EditValue.ToString() != "1")
            {
                MWDataManager.clsDataAccess _NewWorkPlace = new MWDataManager.clsDataAccess();
                _NewWorkPlace.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);

                _NewWorkPlace.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _NewWorkPlace.queryReturnType = MWDataManager.ReturnType.DataTable;
                _NewWorkPlace.SqlStatement = "exec sp_Planning_Add_Workplace_Stope '" + SelectProdmonth + "','" + SelectMOSectionID + "','" + SelectedWorkplaceid + "' ";

                clsDataResult exr = _NewWorkPlace.ExecuteInstruction();

                if (exr.success == false)
                {
                    MessageBox.Show(exr.Message);
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace Added", Color.CornflowerBlue);
                }

                LoadPlanned();
                LoadCadsPlan();
            }

            if (barActivity.EditValue.ToString() == "1")
            {
                MWDataManager.clsDataAccess _NewWorkPlace = new MWDataManager.clsDataAccess();
                _NewWorkPlace.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);

                _NewWorkPlace.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _NewWorkPlace.queryReturnType = MWDataManager.ReturnType.DataTable;
                _NewWorkPlace.SqlStatement = "exec sp_Planning_Add_Workplace_Dev '" + SelectProdmonth + "','" + SelectMOSectionID + "','" + SelectedWorkplaceid + "' ";

                clsDataResult exr = _NewWorkPlace.ExecuteInstruction();

                if (exr.success == false)
                {
                    MessageBox.Show(exr.Message);
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace Added", Color.CornflowerBlue);
                }

                LoadPlanned();
                LoadCadsPlan();
            }

            colnewOrgunitDisplay.UnGroup();

            if (tblPlanningCycleData.Rows.Count == 0)
            {
                return;
            }

            if (tblPlanningCycleData.Rows.Count > 0)
            {
                DateTime startdate = Convert.ToDateTime(tblPlanningCycleData.Rows[0]["BeginDate"].ToString());
                int columnIndex = 6;

                //Headers Date
                for (int i = 0; i < 66; i++)
                {
                    string test = gvNewCycle.Columns[columnIndex].Caption;

                    gvNewCycle.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM ddd");

                    startdate = startdate.AddDays(1);
                    columnIndex++;
                }
            }

            WorkingDays();

            string _sqlWhere = "sectionid = '" + tblPlanningCycleData.Rows[0]["sb"].ToString() + "'";
            string _sqlOrder = "ShiftDay asc";

            DataTable dtGridSetup = tblPlanningWorkingDays.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

            if (tblPlanningCycleData.Rows.Count > 0)
            {
                for (int i = 6; i < gvNewCycle.Columns.Count; i++)
                {
                    string val = gvNewCycle.Columns[i].Caption;

                    if (i < 71)
                    {
                        if (Convert.ToDateTime(val) > Convert.ToDateTime(tblPlanningCycleData.Rows[0]["EndDate"]))
                        {
                            gvNewCycle.Columns[i].Visible = false;
                        }
                    }
                    else
                    {
                        gvNewCycle.Columns[i].Visible = false;
                    }
                }

                int WDCol = 0;

                for (int Gridrow = 0; Gridrow < tblPlanningCycleData.Rows.Count; Gridrow++)
                {
                    foreach (DataRow dr in tblPlanningCycleData.Rows)
                    {
                        string currWpID = gvNewCycle.GetRowCellValue(Gridrow, gvNewCycle.Columns["workplaceid"]).ToString();

                        if (dr["workplaceid"].ToString() == currWpID)
                        {
                            if (string.IsNullOrEmpty(dr["mocycle"].ToString()))
                            {
                                for (int Gridcol = 6; Gridcol < gvNewCycle.Columns.Count; Gridcol++)
                                {
                                    if (Gridcol < dtGridSetup.Rows.Count + 6)
                                    {
                                        string IsWd = dtGridSetup.Rows[Gridcol - 6]["workingday"].ToString();

                                        string test = gvNewCycle.Columns[Gridcol].ToString();

                                        if (IsWd == "N")
                                        {
                                            gvNewCycle.SetRowCellValue(Gridrow, gvNewCycle.Columns[Gridcol], "OFF ");
                                        }
                                        else
                                        {
                                            string DefCycleCode = gvNewCycle.GetRowCellValue(Gridrow, gvNewCycle.Columns[Gridcol + 65]).ToString();
                                            gvNewCycle.SetRowCellValue(Gridrow, gvNewCycle.Columns[Gridcol], DefCycleCode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                for (int Gridrow = 0; Gridrow < gvNewCycle.RowCount; Gridrow++)
                {
                    for (int Gridcol = 6; Gridcol < gvNewCycle.Columns.Count; Gridcol++)
                    {
                        if (Gridcol < dtGridSetup.Rows.Count + 6)
                        {
                            string calDate = dtGridSetup.Rows[Gridcol - 6]["calendardate"].ToString();
                            string IsWd = dtGridSetup.Rows[Gridcol - 6]["workingday"].ToString();

                            if (IsWd == "N")
                            {
                                gvNewCycle.SetRowCellValue(Gridrow, gvNewCycle.Columns[Gridcol], "OFF ");
                            }
                        }
                    }
                }

                colnewOrgunitDisplay.Group();
                gvNewCycle.ExpandAllGroups();

                for (int Gridrow = 0; Gridrow < tblPlanningCycleData.Rows.Count; Gridrow++)
                {
                    string currWpID = gvNewCycle.GetRowCellValue(Gridrow, gvNewCycle.Columns["workplaceid"]).ToString();

                    if (currWpID == SelectedWorkplaceid)
                    {
                        gvNewCycle.FocusedRowHandle = Gridrow;
                        NewCalcSqm();
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void lbxCodeCycles_MouseDown(object sender, MouseEventArgs e)
        {
            if (lbxCodeCycles.Items.Count == 0)
                return;
            int index = lbxCodeCycles.IndexFromPoint(e.X, e.Y);
            if (index > -1)
            {
                if (lbxCodeCycles.SelectedValue.ToString().TrimEnd() == "B")
                {
                    CycleCode = "    ";
                }
                else
                {
                    CycleCode = (lbxCodeCycles.SelectedValue.ToString() + "    ").Substring(0, 4);
                }

                string s = CycleCode;

                DragDropEffects dde1 = DoDragDrop(s,
                    DragDropEffects.All);
            }
        }

        private void gcCyclePlan_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.gcCyclePlan.PointToClient(new Point(e.X, e.Y));
            int row = gvCyclePlan.CalcHitInfo(p.X, p.Y).RowHandle;
            if (row > -1)
            {
                if (gvCyclePlan.GetRowCellValue(row, gvCyclePlan.CalcHitInfo(p.X, p.Y).Column.FieldName).ToString().Trim() == "OFF" || gvCyclePlan.CalcHitInfo(p.X, p.Y).Column.FieldName.ToString() == "detail")
                {
                    return;
                }

                if (gvCyclePlan.CalcHitInfo(p.X, p.Y).Column.FieldName != null && gvCyclePlan.CalcHitInfo(p.X, p.Y).RowHandle == 2)
                {
                    this.gvCyclePlan.SetRowCellValue(row, gvCyclePlan.CalcHitInfo(p.X, p.Y).Column.FieldName, CycleCode);
                    CalcSqm();
                }
            }
        }

        private void gcCyclePlan_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void gcCyclePlan_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void gvCyclePlan_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.CellValue.ToString() == "OFF ")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                    e.Appearance.ForeColor = Color.Gainsboro;
                }

                if (e.CellValue.ToString() == "BL  ")
                {
                    e.Appearance.ForeColor = Color.Tomato;
                }

                if (e.CellValue.ToString() == "BFBL")
                {
                    e.Appearance.ForeColor = Color.Tomato;
                }

                if (e.CellValue.ToString() == "SR  ")
                {
                    e.Appearance.ForeColor = Color.Tomato;
                }

                if (e.CellValue.ToString() == "EX  ")
                {
                    e.Column.Visible = false;
                }
            }
            catch (Exception)
            {


            }
        }

        private void gvCyclePlan_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gvCyclePlan.FocusedColumn.FieldName.ToString() == "detail" || gvCyclePlan.GetRowCellValue(e.RowHandle, gvCyclePlan.FocusedColumn).ToString().Trim() == "OFF")
            {
                return;
            }

            //if (Convert.ToDateTime(e.Column.Caption) < Convert.ToDateTime(System.DateTime.Now))
            //{
            //    if (UserCurrentInfo.UserID == "mineware" || UserCurrentInfo.UserID == "Kelvin")
            //    {
            //        if (CycleCode != "None" && CycleCode != "Code" && e.RowHandle == 2)
            //        {
            //            gvCyclePlan.SetRowCellValue(e.RowHandle, gvCyclePlan.FocusedColumn, CycleCode);
            //            CalcSqm();
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Error , Cycle cant be changed for past calendar days.");
            //        return;
            //    }
            //}
            if (CycleCode != "None" && CycleCode != "Code" && e.RowHandle == 2)
            {
                gvCyclePlan.SetRowCellValue(e.RowHandle, gvCyclePlan.FocusedColumn, CycleCode);
                CalcSqm();
            }
        }

        private void viewPlanningDev_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridView view;
            view = sender as GridView;

            if (tblPlanningData.Rows[view.FocusedRowHandle]["Auth"].ToString() == "Y")
            {
                e.Cancel = true;
            }
        }

        private void viewPlanningStoping_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridView view;
            view = sender as GridView;

            if (tblPlanningData.Rows[view.FocusedRowHandle]["Auth"].ToString() == "Y")
            {
                e.Cancel = true;
            }
        }

        private void viewPlanningDev_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (tblPlanningData.Rows.Count == -1 || tblPlanningData.Rows.Count == 0)
            {
                return;
            }

            if (e.Column.FieldName != "mocycle" || e.Column.FieldName != "Metresadvance")
            {
                tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Changed"] = "Y";
            }

            if (e.Column.FieldName == "MonthlyTotalSQM")
            {
                editMonthPlan.Text = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[e.RowHandle]["MonthlyTotalSQM"].ToString())), 2).ToString();

                if (pcCycle.Visible == true)
                {
                    ShowDevCycle(true);
                }
            }


        }

        private void viewPlanningStoping_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (tblPlanningData.Rows.Count == -1 || tblPlanningData.Rows.Count == 0)
            {
                return;
            }

            if (e.Column.FieldName != "mocycle" || e.Column.FieldName != "callValue")
            {
                tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Changed"] = "Y";
            }


            if (e.Column.FieldName == "MonthlyTotalSQM")
            {
                editMonthPlan.Text = Math.Round((Convert.ToDecimal(tblPlanningData.Rows[e.RowHandle]["MonthlyTotalSQM"].ToString())), 2).ToString();

                if (pcCycle.Visible == true)
                {
                    ShowStopeCycle(true);
                }
            }

        }

        private void viewPlanningStoping_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (pcCycle.Visible == true)
            {
                ShowStopeCycle(true);
            }
        }

        private void viewPlanningDev_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (pcCycle.Visible == true)
            {
                ShowDevCycle(true);
            }
        }

        private void reMinerSelectionDev_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit editor = sender as DevExpress.XtraEditors.LookUpEdit;
            DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            object value = row["SectionID"];

            string ShiftBoss = GetShiftBoss(value.ToString());
            string ShiftbossID = GetShiftBossID(value.ToString());

            viewPlanningDev.SetRowCellValue(viewPlanningDev.FocusedRowHandle, viewPlanningDev.Columns["SBName"], ShiftBoss);
            tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["ShiftBossID"] = ShiftbossID;

            //Because seccal is not counting workingdays correctly (this is quick)
            ShowDevCycle(true);
            ShowDevCycle(false);

            viewPlanningDev.PostEditor();
        }

        private void reMinerSelectionStope_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit editor = sender as DevExpress.XtraEditors.LookUpEdit;
            DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            object value = row["SectionID"];

            string ShiftBoss = GetShiftBoss(value.ToString());
            string ShiftbossID = GetShiftBossID(value.ToString());

            viewPlanningStoping.SetRowCellValue(viewPlanningStoping.FocusedRowHandle, viewPlanningStoping.Columns["SBName"], ShiftBoss);
            tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["ShiftBossID"] = ShiftbossID;

            //Because seccal is not counting workingdays correctly (this is quick)
            ShowStopeCycle(true);
            ShowStopeCycle(false);

            viewPlanningStoping.PostEditor();
        }

        public string GetShiftBoss(string SelectedMiner)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;

            _MinerData.SqlStatement = "select distinct a.SBID SECTIONID_1,SBName Name_1 from tbl_SectionComplete a inner join tbl_Code_Calendar_Section b on " +
                                      "A.PRODMONTH = B.Prodmonth AND " +
                                      "A.SBID = B.SectionID " +
                                      "where A.prodmonth = '" + SelectProdmonth + "' and a.SECTIONID = '" + SelectedMiner + "' ORDER BY a.SBName";

            _MinerData.ExecuteInstruction();

            return _MinerData.ResultsDataTable.Rows[0][1].ToString();
        }

        public string GetShiftBossID(string SelectedMiner)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;

            _MinerData.SqlStatement = "select distinct a.SBID SECTIONID_1,SBName Name_1 from tbl_SectionComplete a inner join tbl_Code_Calendar_Section b on " +
                                      "A.PRODMONTH = B.Prodmonth AND " +
                                      "A.SBID = B.SectionID " +
                                      "where A.prodmonth = '" + SelectProdmonth + "' and a.SECTIONID = '" + SelectedMiner + "' ORDER BY a.SBName";

            _MinerData.ExecuteInstruction();

            return _MinerData.ResultsDataTable.Rows[0][0].ToString();
        }

        private void gvImportedWp_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (gvImportedWp.GetRowCellValue(e.RowHandle, gvImportedWp.Columns["CanImport"]).ToString() == "N")
            {
                e.Appearance.ForeColor = Color.Red;
            }
        }



        private void barProdMonth_EditValueChanged(object sender, EventArgs e)
        {
            if (FirtsLoad != "Y")
            {
                barbtnShow_ItemClick(null, null);
            }
        }

        private void barSection_EditValueChanged(object sender, EventArgs e)
        {
            if (FirtsLoad != "Y")
            {
                barbtnShow_ItemClick(null, null);
            }
        }

        private void barActivity_EditValueChanged(object sender, EventArgs e)
        {
            if (FirtsLoad != "Y")
            {
                barbtnShow_ItemClick(null, null);
            }
        }

        private void viewPlanningStoping_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                if (e.Column.FieldName == "colProduction" || e.Column.FieldName == "VentDept"
                    || e.Column.FieldName == "colSafety" || e.Column.FieldName == "HRDept"
                    || e.Column.FieldName == "Locked" || e.Column.FieldName == "SurveyDept"
                    || e.Column.FieldName == "SafetyDept" || e.Column.FieldName == "GeologyDept"
                    || e.Column.FieldName == "RockEngDept" || e.Column.FieldName == "EngDept"


                    || e.Column.FieldName == "SafetyDeptSU"
                    || e.Column.FieldName == "RockEngDeptSU" || e.Column.FieldName == "PlanningDeptSU"
                    || e.Column.FieldName == "SurveyDeptSU" || e.Column.FieldName == "GeologyDeptSU"
                    || e.Column.FieldName == "MiningDeptSU" || e.Column.FieldName == "VentilationDeptSU"
                    || e.Column.FieldName == "DepartmentDeptSU" ||

                    e.Column.FieldName == "Answer")
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);
                    e.Handled = true;
                }
            }
        }

        private void viewPlanningDev_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                if (e.Column.FieldName == "colProduction" || e.Column.FieldName == "VentDept"
                    || e.Column.FieldName == "colSafety" || e.Column.FieldName == "HRDept"
                    || e.Column.FieldName == "Locked" || e.Column.FieldName == "SurveyDept"
                    || e.Column.FieldName == "SafetyDept" || e.Column.FieldName == "GeologyDept"
                    || e.Column.FieldName == "RockEngDept" || e.Column.FieldName == "EngDept"
                    || e.Column.FieldName == "SafetyDeptSU" || e.Column.FieldName == "DepartmentDeptSU"
                    || e.Column.FieldName == "RockEngDeptSU" || e.Column.FieldName == "PlanningDeptSU"
                    || e.Column.FieldName == "SurveyDeptSU" || e.Column.FieldName == "GeologyDeptSU"
                    || e.Column.FieldName == "MiningDeptSU" || e.Column.FieldName == "VentilationDeptSU"
                    || e.Column.FieldName == "Answer")
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);
                    e.Handled = true;
                }
            }
        }


        public static string ExtractAfterColon(string TheString)
        {
            string AfterColon = string.Empty;

            int index = TheString.IndexOf(":"); // Kry die postion van die :

            AfterColon = TheString.Substring(index + 1); // kry alles na :

            return AfterColon;
        }

        public static string ExtractBeforeColon(string TheString)
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

        private void viewPlanningStoping_DoubleClick(object sender, EventArgs e)
        {
            string Sectionid = tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["SectionID"].ToString();
            string Workplaceid = tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["WorkplaceID"].ToString();

            if (viewPlanningStoping.FocusedColumn.FieldName.ToString() == "Locked")
            {
                if (tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Auth"] == "N")
                {
                    string Authnotes = "Authorised on the " + string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + " by " + TUserInfo.UserID + " ";

                    MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
                    minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                    minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
                    minerData.SqlStatement = "update tbl_planmonth \r\n" +
                        "set Auth = 'Y' , AuthNotes = '" + Authnotes + "'   \r\n" +
                        "where prodmonth = '" + SelectProdmonth + "' and sectionid = '" + ExtractBeforeColon(Sectionid) + "' and workplaceid = '" + Workplaceid + "' ";

                    var theResult = minerData.ExecuteInstruction();
                    if (theResult.success) // check if the query was executed correctly 
                    {
                        tblBoxholeData = minerData.ResultsDataTable.Copy();
                    }

                    tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Auth"] = "Y";
                    tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Locked"] = "1";

                    for (int rows = 0; rows < gvNewCycle.RowCount; rows++)
                    {
                        if (!gvNewCycle.IsGroupRow(rows))
                        {
                            if (gvNewCycle.GetRowCellValue(rows, gvNewCycle.Columns["workplaceid"]).ToString() == tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["WorkplaceID"].ToString())
                            {
                                gvNewCycle.SetRowCellValue(rows, gvNewCycle.Columns["Auth"], "Y");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    string Authnotes = "Un-Authorised on the " + string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + " by " + TUserInfo.UserID + " ";

                    MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
                    minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                    minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
                    minerData.SqlStatement = "update tbl_planmonth \r\n" +
                        "set Auth = 'N' , AuthNotes = '" + Authnotes + "'   \r\n" +
                        "where prodmonth = '" + SelectProdmonth + "' and sectionid = '" + ExtractBeforeColon(Sectionid) + "' and workplaceid = '" + Workplaceid + "' ";

                    var theResult = minerData.ExecuteInstruction();
                    if (theResult.success) // check if the query was executed correctly 
                    {
                        tblBoxholeData = minerData.ResultsDataTable.Copy();
                    }

                    tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Auth"] = "N";
                    tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Locked"] = "0";

                    for (int rows = 0; rows < gvNewCycle.RowCount; rows++)
                    {
                        if (!gvNewCycle.IsGroupRow(rows))
                        {
                            if (gvNewCycle.GetRowCellValue(rows, gvNewCycle.Columns["workplaceid"]).ToString() == tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["WorkplaceID"].ToString())
                            {
                                gvNewCycle.SetRowCellValue(rows, gvNewCycle.Columns["Auth"], "N");
                                break;
                            }
                        }
                    }
                }
            }


            #region Preplanning
            GridView view = (GridView)sender;
            string colCaption = viewPlanningStoping.FocusedColumn.Caption;

            string dbl_result_Crew = tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["OrgUnitDay"].ToString();
            string dbl_result_Section = SelectMOSectionID;
            string dbl_result_ProdMonth = SelectProdmonth;

            //dbl_result_Crew = dbl_result_Crew + ":";

            Form frm_Cap_Sheets = new Form();

            if (colCaption == "Safety")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Safety";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }

                frm_Cap_Sheets.Text = "Safety Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();

            }

            if (colCaption == "Rock Eng")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "RockEng";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.Text = "Rock Eng Department";
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "Occ & Env")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Vent";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }

                frm_Cap_Sheets.Text = "Occ & Env Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "Survey")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Survey";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }

                frm_Cap_Sheets.Text = "Survey Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "Geology")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Geology";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }
                frm_Cap_Sheets.Text = "Geo-Science Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "Engineering")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Engineering";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }
                frm_Cap_Sheets.Text = "Engineering Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "HR")
            {
                ucGraphicsPrePlanningHR HR = new ucGraphicsPrePlanningHR();
                HR.Dock = DockStyle.Fill;
                HR.BringToFront();
                HR.dbl_rec_Crew = dbl_result_Crew;
                HR.dbl_rec_ProdMonth = dbl_result_ProdMonth;
                HR.dbl_rec_Section = dbl_result_Section;

                HR.theSystemDBTag = this.theSystemDBTag;
                HR.UserCurrentInfo = this.UserCurrentInfo;

                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }
                frm_Cap_Sheets.Controls.Add(HR);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }
            #endregion
        }

        private void viewPlanningDev_DoubleClick(object sender, EventArgs e)
        {
            string Sectionid = tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["SectionID"].ToString();
            string Workplaceid = tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["WorkplaceID"].ToString();


            if (viewPlanningDev.FocusedColumn.FieldName.ToString() == "Locked")
            {
                if (tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Auth"] == "N")
                {
                    string Authnotes = "Authorised on the " + string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + " by " + TUserInfo.UserID + " ";

                    MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
                    minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                    minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
                    minerData.SqlStatement = "update tbl_planmonth \r\n" +
                        "set Auth = 'Y' , AuthNotes = '" + Authnotes + "'   \r\n" +
                        "where prodmonth = '" + SelectProdmonth + "' and sectionid = '" + ExtractBeforeColon(Sectionid) + "' and workplaceid = '" + Workplaceid + "' ";

                    var theResult = minerData.ExecuteInstruction();
                    if (theResult.success) // check if the query was executed correctly 
                    {
                        tblBoxholeData = minerData.ResultsDataTable.Copy();
                    }

                    tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Auth"] = "Y";
                    tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Locked"] = "1";

                    for (int rows = 0; rows < gvNewCycle.RowCount; rows++)
                    {
                        if (!gvNewCycle.IsGroupRow(rows))
                        {
                            if (gvNewCycle.GetRowCellValue(rows, gvNewCycle.Columns["workplaceid"]).ToString() == tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Workplaceid"].ToString())
                            {
                                gvNewCycle.SetRowCellValue(rows, gvNewCycle.Columns["Auth"], "Y");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    
                        string Authnotes = "Un-Authorised on the " + string.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + " by " + TUserInfo.UserID + " ";

                        MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
                        minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                        minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
                        minerData.SqlStatement = "update tbl_planmonth \r\n" +
                            "set Auth = 'N' , AuthNotes = '" + Authnotes + "'   \r\n" +
                            "where prodmonth = '" + SelectProdmonth + "' and sectionid = '" + ExtractBeforeColon(Sectionid) + "' and workplaceid = '" + Workplaceid + "' ";

                        var theResult = minerData.ExecuteInstruction();
                        if (theResult.success) // check if the query was executed correctly 
                        {
                            tblBoxholeData = minerData.ResultsDataTable.Copy();
                        }

                        tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Auth"] = "N";
                        tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Locked"] = "0";

                        for (int rows = 0; rows < gvNewCycle.RowCount; rows++)
                        {
                            if (!gvNewCycle.IsGroupRow(rows))
                            {
                                if (gvNewCycle.GetRowCellValue(rows, gvNewCycle.Columns["workplaceid"]).ToString() == tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Workplaceid"].ToString())
                                {
                                    gvNewCycle.SetRowCellValue(rows, gvNewCycle.Columns["Auth"], "N");
                                    break;
                                }
                            }
                        }
                    }
            }

            #region Preplanning
            GridView view = (GridView)sender;
            string colCaption = viewPlanningDev.FocusedColumn.Caption;

            string dbl_result_Crew = tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["OrgUnitDay"].ToString();
            string dbl_result_Section = SelectMOSectionID;
            string dbl_result_ProdMonth = SelectProdmonth;

            //dbl_result_Crew = dbl_result_Crew + ":";

            Form frm_Cap_Sheets = new Form();

            if (colCaption == "Safety")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Safety";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }

                frm_Cap_Sheets.Text = "Safety Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();

            }

            if (colCaption == "Rock Eng")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "RockEng";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.Text = "Rock Eng Department";
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "Occ & Env")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Vent";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }

                frm_Cap_Sheets.Text = "Occ & Env Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "Survey")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Survey";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }

                frm_Cap_Sheets.Text = "Survey Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "Geology")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Geology";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }
                frm_Cap_Sheets.Text = "Geo-Science Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "Engineering")
            {
                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Engineering";
                Pre_PlanningFrm._Activity = barActivity.EditValue.ToString();
                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }
                frm_Cap_Sheets.Text = "Engineering Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "HR")
            {
                ucGraphicsPrePlanningHR HR = new ucGraphicsPrePlanningHR();
                HR.Dock = DockStyle.Fill;
                HR.BringToFront();
                HR.dbl_rec_Crew = dbl_result_Crew;
                HR.dbl_rec_ProdMonth = dbl_result_ProdMonth;
                HR.dbl_rec_Section = dbl_result_Section;

                HR.theSystemDBTag = this.theSystemDBTag;
                HR.UserCurrentInfo = this.UserCurrentInfo;

                try
                {
                    frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine_Test\SM.ico"); }
                frm_Cap_Sheets.Controls.Add(HR);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
            }
            #endregion
        }

        private void barEditItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void rcPlanning_Click(object sender, EventArgs e)
        {

        }

        private void viewPlanningStoping_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Answer")
            {
                if (viewPlanningStoping.GetRowCellValue(e.RowHandle, "Answer").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "SafetyDeptSU"
                || e.Column.FieldName == "RockEngDeptSU" || e.Column.FieldName == "PlanningDeptSU"
                    || e.Column.FieldName == "SurveyDeptSU" || e.Column.FieldName == "GeologyDeptSU"
                    || e.Column.FieldName == "MiningDeptSU" || e.Column.FieldName == "VentilationDeptSU"
                    || e.Column.FieldName == "DepartmentDeptSU")
            {
                if (viewPlanningStoping.GetRowCellValue(e.RowHandle, "StartUpReq").ToString() == "N")
                {
                    e.Appearance.BackColor = Color.FromArgb(240, 240, 240); //.ControlLightLight;
                }
            }

            //if (viewPlanningStoping.GetRowCellValue(e.RowHandle, viewPlanningStoping.Columns["ColColorStartUp"]).ToString() == "N")
            //   {

            //         e.Appearance.BackColor = Color.Red;
            //   }
        }

        private void viewPlanningDev_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Answer")
            {
                if (viewPlanningDev.GetRowCellValue(e.RowHandle, "Answer").ToString() == "0")
                {
                    e.Appearance.ForeColor = Color.Transparent;
                }
            }

            if (e.Column.FieldName == "SafetyDeptSU"
                || e.Column.FieldName == "RockEngDeptSU" || e.Column.FieldName == "PlanningDeptSU"
                    || e.Column.FieldName == "SurveyDeptSU" || e.Column.FieldName == "GeologyDeptSU"
                    || e.Column.FieldName == "MiningDeptSU" || e.Column.FieldName == "VentilationDeptSU"
                    || e.Column.FieldName == "DepartmentDeptSU")
            {
                if (viewPlanningDev.GetRowCellValue(e.RowHandle, "StartUpReq").ToString() == "N")
                {
                    e.Appearance.BackColor = Color.FromArgb(240, 240, 240); //.ControlLightLight;
                }
            }
        }

        private void gvNewCycle_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (gvNewCycle.FocusedColumn.FieldName.ToString() == "workplaceid" || gvNewCycle.FocusedColumn.FieldName.ToString() == "Orgunit"
                || gvNewCycle.FocusedColumn.FieldName.ToString() == "sb" || gvNewCycle.FocusedColumn.FieldName.ToString() == "description"
                || gvNewCycle.FocusedColumn.FieldName.ToString() == "CadsSqm" || gvNewCycle.FocusedColumn.FieldName.ToString() == "CycleSqm"
                || gvNewCycle.GetRowCellValue(e.RowHandle, gvNewCycle.FocusedColumn).ToString().Trim() == "OFF")
            {
                return;
            }

            //if (Convert.ToDateTime(e.Column.Caption) < Convert.ToDateTime(System.DateTime.Now))
            //{
            //    if (UserCurrentInfo.UserID == "mineware" || UserCurrentInfo.UserID == "Kelvin")
            //    {
            //        if (CycleCode != "None" && CycleCode != "Code")
            //        {
            //            gvNewCycle.SetRowCellValue(e.RowHandle, gvNewCycle.FocusedColumn, CycleCode);
            //            NewCalcSqm();
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Error , Cycle cant be changed for past calendar days.");
            //        return;
            //    }
            //}
            if (CycleCode != "None" && CycleCode != "Code")
            {
                gvNewCycle.SetRowCellValue(e.RowHandle, gvNewCycle.FocusedColumn, CycleCode);
                NewCalcSqm();
            }
        }

        private void gvNewCycle_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.CellValue.ToString().Trim() == "OFF")
                {
                    e.Appearance.BackColor = Color.Gainsboro;
                    e.Appearance.ForeColor = Color.Gainsboro;
                }

                if (e.CellValue.ToString().Trim() == "BL")
                {
                    e.Appearance.ForeColor = Color.Tomato;
                }

                if (e.CellValue.ToString().Trim() == "SUBL")
                {
                    e.Appearance.ForeColor = Color.Tomato;
                }

                if (e.CellValue.ToString().Trim() == "BFBL")
                {
                    e.Appearance.ForeColor = Color.Tomato;
                }

                if (e.CellValue.ToString().Trim() == "SR")
                {
                    e.Appearance.ForeColor = Color.Tomato;
                }

                if (e.CellValue.ToString().Trim() == "EX")
                {
                    e.Column.Visible = false;
                }
            }
            catch (Exception)
            {


            }
        }

        private void tbCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbCycle.SelectedIndex == 0)
            {
                btnExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                pcCycle.Visible = false;
                pcCycle.Height = 220;
                gcCyclePlan.Visible = true;
                pcInfo.Visible = true;
            }

            if (tbCycle.SelectedIndex == 1)
            {
                btnExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                pcCycle.Visible = true;
                pcCycle.Height = 80;
                gcCyclePlan.Visible = false;
                pcInfo.Visible = false;
            }
        }

        private void reBoxholeStope_EditValueChanged(object sender, EventArgs e)
        {
            viewPlanningStoping.PostEditor();
        }

        private void reBoxholeDev_EditValueChanged(object sender, EventArgs e)
        {
            viewPlanningDev.PostEditor();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWordEditor helpFrm = new frmWordEditor();
            helpFrm.ViewType = "View";
            helpFrm.MainCat = "Planning";
            helpFrm.SubCat = "Planning";
            helpFrm.Show();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string path = "PlanningCycle.xlsx";
            gvNewCycle.ExportToXlsx(path);
            // Open the created XLSX file with the default application.
            Process.Start(path);
        }
    }
}
