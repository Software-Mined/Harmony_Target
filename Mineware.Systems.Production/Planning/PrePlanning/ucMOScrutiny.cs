using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Commands;
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
    public partial class ucMOScrutiny : BaseUserControl
    {
        public ucMOScrutiny()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpPreplanning);
            FormActiveRibbonPage = rpPreplanning;
            FormMainRibbonPage = rpPreplanning;
            RibbonControl = rcPlanning;
        }


        #region public varibles

        public DataSet dsGlobal = new DataSet();

        #endregion


        #region private varibles
        string SelectProdmonth;
        string SelectMOSectionID;
        string SelectActivity;
        string CycleCode;
        string MoCycleNumNew;
        string FirtsLoad;
        string NewWorkplaceid = "";
        string DeleteWorkplaceid = "";
        string NewWorkplaceName = "";
        DataTable dtAllSections = new DataTable();
        DataTable tblOrgUnitsData = new DataTable();
        DataTable tblMinerListData = new DataTable();
        DataTable tblBoxholeData = new DataTable();
        DataTable tblPlanningData = new DataTable();
        DataTable tblPlanningCycleData = new DataTable();
        DataTable tblPlanningWorkingDays = new DataTable();
        private DataTable dtActions = new DataTable("dtActions");

        private String ID = string.Empty;
        private String Workplace = string.Empty;
        private String Description = string.Empty;
        private String Recomendation = string.Empty;
        private String Priority = string.Empty;
        private String TargetDate = string.Empty;
        private String RespPerson = string.Empty;
        private String Overseer = string.Empty;
        private String FileName = string.Empty;

        Mineware.Systems.Global.sysMessages.sysMessagesClass theMessage = new Global.sysMessages.sysMessagesClass();
        #endregion

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
            LoadActions();

            barSection.EditValue = dtAllSections.Rows[0]["SectionID"].ToString();
            barActivity.EditValue = "0";

            LoadMiners();
            barbtnShow_ItemClick(null, null);

            FirtsLoad = "N";
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

        private void LoadMiners()
        {
            MWDataManager.clsDataAccess minerData = new MWDataManager.clsDataAccess();
            minerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            minerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            minerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            minerData.SqlStatement = "Select SectionID, SectionID+' : '+Name Name from Sections_Complete where Prodmonth = '" + SelectProdmonth+"' and SectionID_2 = '"+barSection.EditValue+"'";

            var theResult = minerData.ExecuteInstruction();

            DataTable dtActivity = new DataTable();
            dtActivity = minerData.ResultsDataTable;


            if (dtActivity.Rows.Count > 0)
            {
                repSectionID.DataSource = null;
                repSectionID.DataSource = dtActivity;
                repSectionID.DisplayMember = "Name";
                repSectionID.ValueMember = "SectionID";

                reMinerSelectionStope.DataSource = null;
                reMinerSelectionStope.DataSource = dtActivity;
                reMinerSelectionStope.DisplayMember = "Name";
                reMinerSelectionStope.ValueMember = "SectionID";

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

            this.Cursor = Cursors.WaitCursor;

            LoadMiners();
            LoadOrgUnits();
            LoadCadsPlan();
            LoadPlanned();
            LoadActions();



            this.Cursor = Cursors.Default;
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

        


       

        public void LoadOrgUnits()
        {
            MWDataManager.clsDataAccess _OrgUnitsData = new MWDataManager.clsDataAccess();
            _OrgUnitsData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _OrgUnitsData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _OrgUnitsData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _OrgUnitsData.SqlStatement =  "SELECT OrgUnit+':'+ GangName CrewOrg,OrgUnit GangNo  FROM tbl_Orgunits  ";
            _OrgUnitsData.ExecuteInstruction();

            tblOrgUnitsData = _OrgUnitsData.ResultsDataTable.Copy();

            #region Stoping
            repCrewStope.DataSource = tblOrgUnitsData;
            repCrewStope.DisplayMember = "CrewOrg";
            repCrewStope.ValueMember = "GangNo";
            repCrewStope.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
           
            #endregion

            #region Development
            repCrew.DataSource = tblOrgUnitsData;
            repCrew.DisplayMember = "CrewOrg";
            repCrew.ValueMember = "GangNo";
            repCrew.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

          
            #endregion
        }

        private void LoadCadsPlan()
        {
            MWDataManager.clsDataAccess CadsData = new MWDataManager.clsDataAccess();
            CadsData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            CadsData.SqlStatement = "Select * from tbl_Workplace \r\n" +
                                    "where activity = '" + SelectActivity + "' order by Description";

            CadsData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            CadsData.queryReturnType = MWDataManager.ReturnType.DataTable;
            var theResult = CadsData.ExecuteInstruction();

            DataTable dtCadsInfo = new DataTable();
            dtCadsInfo = CadsData.ResultsDataTable;

            gcImportedWp.DataSource = null;
            gcImportedWp.DataSource = dtCadsInfo;

            colImportWorkplaceID.FieldName = "WorkplaceID";
            colImportWorkplaceDesc.FieldName = "Description";
            //colCanImport.FieldName = "CanImport";
        }

        private void LoadPlanned()
        {
            MainGridStope.Visible = false;
            MainGrid.Visible = false;
            MainGrid.Dock = DockStyle.None;
            MainGridStope.Dock = DockStyle.None;

            //colnewOrgunitDisplay.UnGroup();

            if (barActivity.EditValue.ToString() != "1")
            {
                MainGridStope.Visible = true;
                MainGridStope.Dock = DockStyle.Fill;

                MWDataManager.clsDataAccess CadsData = new MWDataManager.clsDataAccess();
                CadsData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                CadsData.SqlStatement = " exec sp_Planning_Load_Stope_MOScrutiny '" + SelectProdmonth + "' , '" + SelectMOSectionID + "' ,'" + SelectActivity + "'  ";

                CadsData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                CadsData.queryReturnType = MWDataManager.ReturnType.DataTable;
                var theResult = CadsData.ExecuteInstruction();

                tblPlanningData = CadsData.ResultsDataTable.Copy();

                MainGridStope.DataSource = null;
                MainGridStope.DataSource = tblPlanningData;

                //MWDataManager.clsDataAccess CycleData = new MWDataManager.clsDataAccess();
                //CycleData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                //CycleData.SqlStatement = "exec sp_Planning_Load_Stope_Cycle '" + SelectProdmonth + "' , '" + SelectMOSectionID + "' ,'" + SelectActivity + "'  ";

                //CycleData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //CycleData.queryReturnType = MWDataManager.ReturnType.DataTable;
                //var theResult1 = CycleData.ExecuteInstruction();

                //tblPlanningCycleData = CycleData.ResultsDataTable.Copy();

                //gcNewCycle.Visible = false;
                //gcNewCycle.DataSource = null;
                //gcNewCycle.DataSource = tblPlanningCycleData;
                //gcNewCycle.Visible = true;
            }

            if (barActivity.EditValue.ToString() == "1")
            {
                MainGrid.Visible = true;
                MainGrid.Dock = DockStyle.Fill;

                MWDataManager.clsDataAccess CadsData = new MWDataManager.clsDataAccess();
                CadsData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                CadsData.SqlStatement = "exec sp_Planning_Load_Dev_MOScrutiny '" + SelectProdmonth + "' , '" + SelectMOSectionID + "' ,'" + SelectActivity + "'  ";

                CadsData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                CadsData.queryReturnType = MWDataManager.ReturnType.DataTable;
                var theResult = CadsData.ExecuteInstruction();

                tblPlanningData = CadsData.ResultsDataTable.Copy();

                MainGrid.DataSource = null;
                MainGrid.DataSource = tblPlanningData;

                //MWDataManager.clsDataAccess CycleData = new MWDataManager.clsDataAccess();
                //CycleData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                //CycleData.SqlStatement = "exec sp_Planning_Load_Dev_Cycle '" + SelectProdmonth + "' , '" + SelectMOSectionID + "' ,'" + SelectActivity + "'  ";

                //CycleData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //CycleData.queryReturnType = MWDataManager.ReturnType.DataTable;
                //var theResult1 = CycleData.ExecuteInstruction();

                //tblPlanningCycleData = CycleData.ResultsDataTable.Copy();

                //gcNewCycle.Visible = false;
                //gcNewCycle.DataSource = null;
                //gcNewCycle.DataSource = tblPlanningCycleData;
                //gcNewCycle.Visible = true;
            }


            //if (SelectActivity != "1")
            //{
            //    colNewCadsSqm.ColumnEdit = reSeSqm;
            //    colNewCycleSqm.ColumnEdit = reSeSqm;
            //}
            //else
            //{
            //    colNewCadsSqm.ColumnEdit = reSeMAdv;
            //    colNewCycleSqm.ColumnEdit = reSeMAdv;
            //}

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
                    //gvNewCycle.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM ddd");

                    startdate = startdate.AddDays(1);
                    columnIndex++;
                }
            }

            WorkingDays();

            string _sqlWhere = "sectionid = '" + tblPlanningCycleData.Rows[0]["sb"].ToString() + "'";
            string _sqlOrder = "ShiftDay asc";

            DataTable dtGridSetup = tblPlanningWorkingDays.Select(_sqlWhere, _sqlOrder).CopyToDataTable();

            //SetVisibleIndex();

            

              
        }

        



        private void barbtnCyclePlanning_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (SelectActivity == "1")
            //{
            //    ShowDevCycle(barbtnCyclePlanning.Down);
            //}

            //if (SelectActivity == "0")
            //{
            //    ShowStopeCycle(barbtnCyclePlanning.Down);
            //}
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

        

       

      
        private void barbtnSavePlanning_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (SelectActivity == "0")
            {
                SaveMOSCrutiny();
            }
            else
            {
                SaveMOSCrutinyDev();
            }
        }

        

        public void SaveMOSCrutiny()
        {
            viewPlanningStoping.PostEditor();
            if (viewPlanningStoping.IsEditing)
                viewPlanningStoping.CloseEditor();
            if (viewPlanningStoping.FocusedRowModified)
                viewPlanningStoping.UpdateCurrentRow();
            int rowHandle = 0;
            viewPlanningStoping.SelectRow(rowHandle);
            MWDataManager.clsDataAccess _PrePlanningCycleData = new MWDataManager.clsDataAccess();
            _PrePlanningCycleData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _PrePlanningCycleData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningCycleData.queryReturnType = MWDataManager.ReturnType.DataTable;
            for (int i = 0; i < viewPlanningStoping.RowCount; i++)
            {
                string Prodmonth = SelectProdmonth;
                string SectionID = viewPlanningStoping.GetRowCellValue(i, viewPlanningStoping.Columns["SectionID"].FieldName).ToString();
                string WorkplaceID = viewPlanningStoping.GetRowCellValue(i, viewPlanningStoping.Columns["WorkplaceID"].FieldName).ToString();
                string Activity = SelectActivity;
                string orgunitDay = viewPlanningStoping.GetRowCellValue(i, viewPlanningStoping.Columns["OrgUnitDay"].FieldName).ToString();
             
                _PrePlanningCycleData.SqlStatement = _PrePlanningCycleData.SqlStatement + " exec [sp_Planning_Save_MOScrutiny] '" + Prodmonth + "' , '" + SectionID + "' , '" + WorkplaceID + "', '" + Activity + "', '" + orgunitDay + "'";
                              
            }
            var result = _PrePlanningCycleData.ExecuteInstruction();

            Global.sysNotification.TsysNotification.showNotification("Data Saved", "MO Scrutiny Saved", Color.CornflowerBlue);

            
        }

        public void SaveMOSCrutinyDev()
        {
            if (viewPlanningDev.IsEditing)
                viewPlanningDev.CloseEditor();
            if (viewPlanningDev.FocusedRowModified)
                viewPlanningDev.UpdateCurrentRow();
            viewPlanningDev.PostEditor();
            int rowHandle = 0;
            viewPlanningDev.SelectRow(rowHandle);
            MWDataManager.clsDataAccess _PrePlanningCycleData = new MWDataManager.clsDataAccess();
            _PrePlanningCycleData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _PrePlanningCycleData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningCycleData.queryReturnType = MWDataManager.ReturnType.DataTable;
            for (int i = 0; i < viewPlanningDev.RowCount; i++)
            {
                string Prodmonth = SelectProdmonth;
                string SectionID = viewPlanningDev.GetRowCellValue(i, viewPlanningDev.Columns["SectionID"].FieldName).ToString();
                string WorkplaceID = viewPlanningDev.GetRowCellValue(i, viewPlanningDev.Columns["WorkplaceID"].FieldName).ToString();
                string Activity = SelectActivity;
                string orgunitDay = viewPlanningDev.GetRowCellValue(i, viewPlanningDev.Columns["OrgUnitDay"].FieldName).ToString();

                _PrePlanningCycleData.SqlStatement = _PrePlanningCycleData.SqlStatement + " exec [sp_Planning_Save_MOScrutiny] '" + Prodmonth + "' , '" + SectionID + "' , '" + WorkplaceID + "', '" + Activity + "', '" + orgunitDay + "'";

            }
            var result = _PrePlanningCycleData.ExecuteInstruction();

            Global.sysNotification.TsysNotification.showNotification("Data Saved", "MO Scrutiny Saved", Color.CornflowerBlue);


        }


        private void gvImportedWp_DoubleClick(object sender, EventArgs e)
        {
            if (gvImportedWp.FocusedRowHandle == -1)
            {
                return;
            }

            //if (gvImportedWp.GetRowCellValue(gvImportedWp.FocusedRowHandle, gvImportedWp.Columns["CanImport"]).ToString() == "N")
            //{
            //    MessageBox.Show("Can not Import this workplace,No Miner assigned from Cads");
            //    return;
            //}

            this.Cursor = Cursors.WaitCursor;

            NewWorkplaceid =  gvImportedWp.GetRowCellValue(gvImportedWp.FocusedRowHandle, gvImportedWp.Columns["WorkplaceID"]).ToString();
            NewWorkplaceName = gvImportedWp.GetRowCellValue(gvImportedWp.FocusedRowHandle, gvImportedWp.Columns["Description"]).ToString();
            if (barActivity.EditValue.ToString() != "1")
            {    

                viewPlanningStoping.AddNewRow();
                //Global.sysNotification.TsysNotification.showNotification("Data Added", "Workplace Added", Color.CornflowerBlue);
            }

            if (barActivity.EditValue.ToString() == "1")
            {
                viewPlanningDev.AddNewRow();
                //Global.sysNotification.TsysNotification.showNotification("Data Added", "Workplace Added", Color.CornflowerBlue);


              
            }            

            WorkingDays();

          
            this.Cursor = Cursors.Default;
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

            //if (tblPlanningData.Rows[view.FocusedRowHandle]["Auth"].ToString() == "Y")
            //{
            //    e.Cancel = true;
            //}
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

           


        }

        private void viewPlanningStoping_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (tblPlanningData.Rows.Count == -1 || tblPlanningData.Rows.Count == 0)
            {
                return;
            }

            if (e.Column.FieldName != "mocycle" || e.Column.FieldName != "callValue")
            {
                //tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Changed"] = "Y";
            }


            

        }

        private void viewPlanningStoping_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            DeleteWorkplaceid = viewPlanningStoping.GetRowCellValue(viewPlanningStoping.FocusedRowHandle, viewPlanningStoping.Columns["WorkplaceID"]).ToString();
        }

        private void viewPlanningDev_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            DeleteWorkplaceid = viewPlanningDev.GetRowCellValue(viewPlanningDev.FocusedRowHandle, viewPlanningDev.Columns["WorkplaceID"]).ToString();
        }

        private void reMinerSelectionDev_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit editor = sender as DevExpress.XtraEditors.LookUpEdit;
            DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            object value = row["SectionID"];

            string ShiftBoss = GetShiftBoss(value.ToString());
            string ShiftbossID = GetShiftBossID(value.ToString());

            viewPlanningDev.SetRowCellValue(viewPlanningDev.FocusedRowHandle, viewPlanningDev.Columns["SBName"], ShiftBoss);
            //tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["ShiftBossID"] = ShiftbossID;

            //Because seccal is not counting workingdays correctly (this is quick)
            //ShowDevCycle(true);
            //ShowDevCycle(false);

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
            //tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["ShiftBossID"] = ShiftbossID;

            //Because seccal is not counting workingdays correctly (this is quick)
            //ShowStopeCycle(true);
            //ShowStopeCycle(false);

            viewPlanningStoping.PostEditor();
        }

        public string GetShiftBoss(string SelectedMiner)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;

            _MinerData.SqlStatement = "select distinct a.SECTIONID_1 SECTIONID_1,Name_1 Name_1 from Sections_Complete a " +
                                      "where A.prodmonth = '" + SelectProdmonth + "' and a.SECTIONID = '" + SelectedMiner + "' ";

            _MinerData.ExecuteInstruction();

            return _MinerData.ResultsDataTable.Rows[0][1].ToString();
        }

        public string GetShiftBossID(string SelectedMiner)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _MinerData.SqlStatement = "select distinct a.SECTIONID_1 SECTIONID_1,Name_1 Name_1 from Sections_Complete a " +
                                      "where A.prodmonth = '" + SelectProdmonth + "' and a.SECTIONID = '" + SelectedMiner + "' ";
            _MinerData.ExecuteInstruction();

            return _MinerData.ResultsDataTable.Rows[0][0].ToString();
        }

        private void gvImportedWp_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //if (gvImportedWp.GetRowCellValue(e.RowHandle, gvImportedWp.Columns["CanImport"]).ToString() == "N")
            //{
            //    e.Appearance.ForeColor = Color.Red;
            //}
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
            return;
            string Sectionid = tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["SectionID"].ToString();
            string Workplaceid = tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["WorkplaceID"].ToString();

            if (viewPlanningStoping.FocusedColumn.FieldName.ToString() == "Locked")
            {
                if (tblPlanningData.Rows[viewPlanningStoping.FocusedRowHandle]["Auth"].ToString() == "N")
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
            return;
            string Sectionid = tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["SectionID"].ToString();
            string Workplaceid = tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["WorkplaceID"].ToString();


            if (viewPlanningDev.FocusedColumn.FieldName.ToString() == "Locked")
            {
                if (tblPlanningData.Rows[viewPlanningDev.FocusedRowHandle]["Auth"].ToString() == "N")
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

       

        private void tbCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbCycle.SelectedIndex == 0)
            {
                btnExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
               
            }

            if (tbCycle.SelectedIndex == 1)
            {
                btnExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
              
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
            if (barActivity.EditValue.ToString() == "1")
            {
                string path = "MOScrutinyDev.xlsx";
                viewPlanningDev.ExportToXlsx(path);
                // Open the created XLSX file with the default application.
                Process.Start(path);
            }
            else
            {
                string path = "MOScrutinyStoping.xlsx";
                viewPlanningDev.ExportToXlsx(path);
                // Open the created XLSX file with the default application.
                Process.Start(path);
            }
        }

        private void btnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _MinerData.SqlStatement = " insert into tbl_Planmonth_MOScrutiny \r\n" +
                                        " select  \r\n" +
                                        " '" + SelectProdmonth + "' as ProdMonth  \r\n" +
                                        " ,pm.[Sectionid]  \r\n" +
                                        " ,[Workplaceid]  \r\n" +
                                        " ,[Activity]  \r\n" +
                                        " ,[FL]  \r\n" +
                                        " ,[SqmTotal]  \r\n" +
                                        " ,[OrgUnitDS]  \r\n" +
                                        " ,[OrgUnitAS]  \r\n" +
                                        " ,[OrgUnitNS]  \r\n" +
                                        " ,[Adv]  \r\n" +
                                        " ,[Mnth1]  \r\n" +
                                        " ,[Mnth2]  \r\n" +
                                        " ,[Mnth3]  \r\n" +
                                        " ,[OreFlowId]  \r\n" +
                                        " ,[CMGT]  \r\n" +
                                        " ,[SW]  \r\n" +
                                        " ,[Dens]  \r\n" +
                                        " ,[Tons]  \r\n" +
                                        " ,[Content]  \r\n" +
                                        " ,[Comments]  \r\n" +
                                        " ,[DWidth] \r\n" +
                                        " ,[DHeight]  \r\n" +
                                        " ,[Budget]  \r\n" +
                                        " ,[Cubics]  \r\n" +
                                        " ,[OpeningUp]  \r\n" +
                                        " ,[Sweeps]  \r\n" +
                                        " ,[ReSweeps]  \r\n" +
                                        " ,[Vamps]  \r\n" +
                                        " ,[ExtraBudget]  \r\n" +
                                        " ,[DefaultCycle]  \r\n" +
                                        " ,[MOCycle]  \r\n" +
                                        " ,[OffReefSqm]  \r\n" +
                                        " ,[CycleSqm]  \r\n" +
                                        " ,[DefaultCycleNum]  \r\n" +
                                        " ,[MOCycleNum]  \r\n" +
                                        " ,'N'[Auth]  \r\n" +
                                        " ,[AuthNotes]  \r\n" +
                                        " ,[PrevWP]  \r\n" +
                                        " ,[AuthDatetime]  \r\n" +
                                        " ,[DevMachine]  \r\n" +
                             " from tbl_Planmonth pm , Sections_Complete sc  \r\n" +
                             " where pm.Sectionid = sc.SECTIONID and pm.Prodmonth = sc.Prodmonth  \r\n" +
                             " and pm.Prodmonth = Convert(Varchar(10),DatePart(YYYY, DateAdd(YYYY, 0, '" + barProdMonth.EditValue + "'))) + Convert(Varchar(10),DatePart(mm, DateAdd(m, -1, '" + barProdMonth.EditValue + "'))) and sc.SECTIONID_2 = '" + SelectMOSectionID + "' and pm.activity = '" + SelectActivity + "'  \r\n" +
                             " and pm.Workplaceid not in (select Workplaceid from tbl_PlanMonth_MOScrutiny where prodmonth = '" + SelectProdmonth + "')  \r\n";



            _MinerData.ExecuteInstruction();
            Global.sysNotification.TsysNotification.showNotification("Data Copied", "Planning Coppied Succesfully", Color.CornflowerBlue);

            barbtnShow_ItemClick(null, null);

        }

        private void viewPlanningStoping_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            viewPlanningStoping.BeginUpdate();
            viewPlanningStoping.SetRowCellValue(e.RowHandle, viewPlanningStoping.Columns["WorkplaceID"], NewWorkplaceid);
            viewPlanningStoping.SetRowCellValue(e.RowHandle, viewPlanningStoping.Columns["WorkplaceDesc"], NewWorkplaceName);
            viewPlanningStoping.SetRowCellValue(e.RowHandle, viewPlanningStoping.Columns["SectionID"], "");
            viewPlanningStoping.SetRowCellValue(e.RowHandle, viewPlanningStoping.Columns["OrgUnitDay"], "");
            viewPlanningStoping.EndUpdate();
            viewPlanningStoping.PostEditor();

        }

        private void viewPlanningDev_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            viewPlanningDev.BeginUpdate();
            viewPlanningDev.SetRowCellValue(e.RowHandle, viewPlanningDev.Columns["WorkplaceID"], NewWorkplaceid);
            viewPlanningDev.SetRowCellValue(e.RowHandle, viewPlanningDev.Columns["WorkplaceDesc"], NewWorkplaceName);
            viewPlanningDev.SetRowCellValue(e.RowHandle, viewPlanningDev.Columns["SectionID"], "");
            viewPlanningDev.SetRowCellValue(e.RowHandle, viewPlanningDev.Columns["OrgUnitDay"], "");
            viewPlanningDev.EndUpdate();
            viewPlanningDev.PostEditor();

        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DeleteWorkplaceid != "")
            {
                MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
                _MinerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _MinerData.SqlStatement = " delete from tbl_Planmonth_MOScrutiny where prodmonth = '" + SelectProdmonth + "' and WorkplaceID = '" + DeleteWorkplaceid + "' ";
                _MinerData.ExecuteInstruction();
                Global.sysNotification.TsysNotification.showNotification("Data Removed", "Workplace Removed From Scrutiny Sheet", Color.CornflowerBlue);
                barbtnShow_ItemClick(null, null);
            }
            
        }

        private void AddActBtn_Click(object sender, EventArgs e)
        {
            frmMOScrutiny_Actions frmAct = new frmMOScrutiny_Actions();            
            frmAct.AllowExit = "Y";           
            //frmAct.txtRemarks.Text = action;
            frmAct.Type = "PPMO";
            frmAct.lblSection.Text = barSection.EditValue.ToString();
            frmAct.lblProdmonth.Text = SelectProdmonth;
            frmAct.UniqueDate = barProdMonth.EditValue.ToString();
            frmAct.StartPosition = FormStartPosition.CenterScreen;
            frmAct._theSystemDBTag = theSystemDBTag;
            frmAct._UserCurrentInfo = UserCurrentInfo.Connection;
            //frmAct.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            frmAct.ShowDialog();

            LoadActions();
        }

        private void LoadActions()
        {
            //Declarations
            string sql = string.Empty;

            sql = "EXEC sp_PrePlanning_MOScrutiny_LoadActions '" + SelectProdmonth + "','" + barSection.EditValue + "'";

            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _MinerData.SqlStatement = " EXEC sp_PrePlanning_MOScrutiny_LoadActions '" + SelectProdmonth + "','" + barSection.EditValue + "' ";
            _MinerData.ExecuteInstruction();

           

            gcActions.DataSource = null;
            gcActions.DataSource = _MinerData.ResultsDataTable;
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

        private void EditActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");
                return;
            }

            frmMOScrutiny_Actions ActFrm = new frmMOScrutiny_Actions();
            ActFrm.lblProdmonth.Text = SelectProdmonth;
            ActFrm.lblSection.Text = SelectMOSectionID;           
          

            ActFrm._theSystemDBTag = this.theSystemDBTag;
            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;

            ActFrm.Item = "PPMO";
            ActFrm.Type = "MO Action";
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";

            ActFrm.lblWPDesc.EditValue = Workplace;
            ActFrm.Item = Description;
            ActFrm.txtRemarks.Text = Recomendation;
            ActFrm.txtReqDate.Text = TargetDate;
            ActFrm.PriorityCmb.Text = Priority;
            ActFrm.RespPersonCmb.EditValue = RespPerson;
            ActFrm.OverseerCmb.EditValue = Overseer;
            ActFrm.ActID = ID;

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        private void barbtnApprovePrePlanning_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
