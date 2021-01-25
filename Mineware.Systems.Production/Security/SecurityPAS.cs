using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mineware.Systems.Production.Security
{
    /// <summary>
    /// Handles all security for PAS system
    /// </summary>
    internal class SecurityPAS
    {
        #region Fields and Properties
        private static Dictionary<string, int> _pluginPermissions = new Dictionary<string, int>();

        private static string _SyncrominePermissions = string.Empty;

        private static string _sdbPermissions = string.Empty;
        private static string _bookSection = string.Empty;

        public static string BookSection { get { return _bookSection; } private set { _bookSection = value; } }
        #endregion Fields and Properties

        /// <summary>
        /// Sets up the class to have a specific users permissions
        /// </summary>
        /// <param name="userID"></param>
        public static void GetPermissionsForUser(string userID)
        {
            setupMenuItemPermissions(userID);
            setupSyncrominePermissions(userID);
            setupSDBPermissions(userID);
            setupBookSection(userID);
        }

        /// <summary>
        /// Gets the menu item permissions
        /// </summary>
        private static void setupMenuItemPermissions(string userID)
        {
            //Setup Menu Items PAS
            _pluginPermissions.Clear();

            ///PAS
            _pluginPermissions.Add(TProductionGlobal.SysMenu.miProductionAnalysisSystem_Production_MinewareSystemsProduction.ItemID
                                    , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miProductionAnalysisSystem_Production_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miBlastingCompliance_apsBlastingComp_MinewareSystemsProduction.ItemID
                                   , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miBlastingCompliance_apsBlastingComp_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miBookingServices_apsSDBBook_MinewareSystemsProduction.ItemID
                                   , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miBookingServices_apsSDBBook_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miCalendarSetup_apsCalendar_MinewareSystemsProduction.ItemID
                                   , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miCalendarSetup_apsCalendar_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miCrewAnalysis_apsCrewAnalysis_MinewareSystemsProduction.ItemID
                                   , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miCrewAnalysis_apsCrewAnalysis_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miCyclesSetup_apsCycle_MinewareSystemsProduction.ItemID
                                   , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miCyclesSetup_apsCycle_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miDailyBookings_apsDailyBookings_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miDailyBookings_apsDailyBookings_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miDepartments_apsDepartment_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miDepartments_apsDepartment_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miDocumentManager_apsReportingDocManage_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miDocumentManager_apsReportingDocManage_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miGenericReport_apsGenericReport_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miGenericReport_apsGenericReport_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miGeology_apsDepartmentGeology_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miGeology_apsDepartmentGeology_MinewareSystemsProduction.ItemID));


            _pluginPermissions.Add(TProductionGlobal.SysMenu.miInitializeActivitiesandTasks_apsSDBSetupActivities_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miInitializeActivitiesandTasks_apsSDBSetupActivities_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miLineActionManager_apsLineActionManager_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miLineActionManager_apsLineActionManager_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miLogisticsManagement_apsSDB_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miLogisticsManagement_apsSDB_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miLostBlastAnalysis_apsReportingLostBlastAnalysis_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miLostBlastAnalysis_apsReportingLostBlastAnalysis_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miMapBlueprint_apsSDBMapBluePrint_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miMapBlueprint_apsSDBMapBluePrint_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miMeasuringList_apsReportingMeasList_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miMeasuringList_apsReportingMeasList_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miMODaily_apsReportingMoDaily_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miMODaily_apsReportingMoDaily_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miOreFlowDiagram_apsReportingOreflow_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miOreFlowDiagram_apsReportingOreflow_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miPlanningReport_apsReportingPlanningReport_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miPlanningReport_apsReportingPlanningReport_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miPlanningServices_apsSDBPlan_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miPlanningServices_apsSDBPlan_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miPlanning_apsPlanning_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miPlanning_apsPlanning_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miPlanvsBook_apsPlanVsBook_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miPlanvsBook_apsPlanVsBook_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miProblemsSetup_apsProblems_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miProblemsSetup_apsProblems_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miProductionUserSetup_apsUserRights_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miProductionUserSetup_apsUserRights_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miReports_apsReporting_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miReports_apsReporting_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miRockEng_apsDepartmentRockEng_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miRockEng_apsDepartmentRockEng_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miSixShiftRecon_apsReportingSixShift_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miSixShiftRecon_apsReportingSixShift_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miSurvey_apsDepartmentSurvey_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miSurvey_apsDepartmentSurvey_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miSystemAdmin_apsSystemMaint_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miSystemAdmin_apsSystemMaint_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miWorkplaceAdmin_apsWorkplace_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miWorkplaceAdmin_apsWorkplace_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miWorstPerformer_apsReportingWorstPerformer_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miWorstPerformer_apsReportingWorstPerformer_MinewareSystemsProduction.ItemID));


            ///New    
            ///
            _pluginPermissions.Add(TProductionGlobal.SysMenu.miMonthEndRecon_apsMonthEndRecon_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miMonthEndRecon_apsMonthEndRecon_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miKPIReport_apsKPI_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miKPIReport_apsKPI_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miWorkplaceStoppages_apsWorkplaceStoppages_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miWorkplaceStoppages_apsWorkplaceStoppages_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miShiftbossCycleReport_apsShiftBossCycleReport_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miShiftbossCycleReport_apsShiftBossCycleReport_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miEquipmentSetup_apsEquipmentSetup_MinewareSystemsProduction.ItemID
                                  , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miEquipmentSetup_apsEquipmentSetup_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miReconCompliance_apsReconCompliance_MinewareSystemsProduction.ItemID
                                 , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miReconCompliance_apsReconCompliance_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miVentilation_apsDepartmentVentilation_MinewareSystemsProduction.ItemID
                                 , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miVentilation_apsDepartmentVentilation_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miGenericPlanningReport_apsPlanningReportNew_MinewareSystemsProduction.ItemID
                                 , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miGenericPlanningReport_apsPlanningReportNew_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miHighRiskReport_apsHighRiskReport_MinewareSystemsProduction.ItemID
                                 , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miHighRiskReport_apsHighRiskReport_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miEmailSetup_apsEmailSetup_MinewareSystemsProduction.ItemID
                                 , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miEmailSetup_apsEmailSetup_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miOCRSchedular_apsOCRSchedular_MinewareSystemsProduction.ItemID
                                 , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miOCRSchedular_apsOCRSchedular_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miVamping_apsDepartmentVamping_MinewareSystemsProduction.ItemID
                                 , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miVamping_apsDepartmentVamping_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miWorkplaceSummary_apsWorkplaceSummary_MinewareSystemsProduction.ItemID
                                , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miWorkplaceSummary_apsWorkplaceSummary_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miStandardAndNorms_apsStdAndNorms_MinewareSystemsProduction.ItemID
                               , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miStandardAndNorms_apsStdAndNorms_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miLabourReport_miEarlyMorningLabour_MinewareSystemsProduction.ItemID
                               , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miLabourReport_miEarlyMorningLabour_MinewareSystemsProduction.ItemID));

            

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miEngineeringBreakdowns_apsEngineeringBreakdowns_MinewareSystemsProduction.ItemID
                              , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miEngineeringBreakdowns_apsEngineeringBreakdowns_MinewareSystemsProduction.ItemID));

            _pluginPermissions.Add(TProductionGlobal.SysMenu.miEngineering_apDepartmentEngineering_MinewareSystemsProduction.ItemID
                              , TUserInfo.theSecurityLevel(TProductionGlobal.SysMenu.miEngineering_apDepartmentEngineering_MinewareSystemsProduction.ItemID));
        }

        /// <summary>
        /// Gets the permissions for the preplanning
        /// </summary>
        private static void setupSyncrominePermissions(string userID)
        {
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbAction.SqlStatement = @"Select case when PreplanSafety <> 0 then 'Y' else 'N' end as SafetyDep,
                                     case when PreplanRockEng <> 0 then 'Y' else 'N' end as  [RockEngDep],
                                     case when PreplanVentilation <> 0 then 'Y' else 'N' end as  [VentDep],
                                     case when PreplanVentilation <> 0 then 'Y' else 'N' end as  [CostingDep],
                                     case when PreplanHR <> 0 then 'Y' else 'N' end as  [HRDep],
                                     case when PreplanSurvey <> 0 then 'Y' else 'N' end as  [SurveyDep],
                                     case when PreplanGeology <> 0 then 'Y' else 'N' end as  [GeologyDep],
                                     case when PreplanEngineering <> 0 then 'Y' else 'N' end as  [EngDep],

                                     case when PreplanSafety = 2 then 'Y' else 'N' end as [SafetyDepAuth],
                                     case when PreplanRockEng = 2 then 'Y' else 'N' end as [RockEngDepAuth],
                                     case when PreplanVentilation = 2 then 'Y' else 'N' end as [VentDepAuth],
                                     case when PreplanVentilation = 2 then 'Y' else 'N' end as [CostingDepAuth],
                                     case when PreplanHR = 2 then 'Y' else 'N' end as [HRDepAuth],
                                     case when PreplanSurvey = 2 then 'Y' else 'N' end as [SurveyDepAuth],
                                     case when PreplanGeology = 2 then 'Y' else 'N' end as [GeologyDepAuth],
                                     case when PreplanEngineering = 2 then 'Y' else 'N' end as [EngDepAuth],

                                     case when startupsafety <> 0 then 'Y' else 'N' end as SafetyStartUp,
                                     case when startuprockeng <> 0 then 'Y' else 'N' end as  [RockEngStartUp],
                                     case when startupPlanning <> 0 then 'Y' else 'N' end as  [PlanningStartUp],
                                     case when StartupSurvey <> 0 then 'Y' else 'N' end as  [SurveyStartUp],
                                     case when StartupGeology <> 0 then 'Y' else 'N' end as  [GeologyStartUp],
                                     case when StartupMining <> 0 then 'Y' else 'N' end as  [MiningStartup],
                                     case when StartupVent <> 0 then 'Y' else 'N' end as  [VentStartUp],
                                     case when StartupDepartment <> 0 then 'Y' else 'N' end as  [DepartmentStartUp],

                                     case when startupsafety = 2 then 'Y' else 'N' end as SafetyStartUpAuth,
                                     case when startuprockeng = 2 then 'Y' else 'N' end as [RockEngStartUpAuth],
                                     case when startupPlanning = 2 then 'Y' else 'N' end as [PlanningStartUpAuth],
                                     case when StartupSurvey = 2 then 'Y' else 'N' end as [SurveyStartUpAuth],
                                     case when StartupGeology = 2 then 'Y' else 'N' end as [GeologyStartUpAuth],
                                     case when StartupMining = 2 then 'Y' else 'N' end as [MiningStartupAuth],
                                     case when StartupVent = 2 then 'Y' else 'N' end as [VentStartUpAuth],
                                     case when StartupDepartment = 2 then 'Y' else 'N' end as [DepartmentStartUpAuth],
                                     case when OreflowDesign <> 0 then 'Y' else 'N' end as [OreflowDesign],
									 case when OreflowBackfill <> 0 then 'Y' else 'N' end as [OreflowBackfill],
                                     isnull(SDB_Plan,'N') [Plan],
									 isnull(SDB_InitializeActandTask,'N') InitializeActandTask,
									 isnull(SDB_MapBlueprint,'N') MapBlueprint,
									 isnull(SDB_Booking,'N') Booking,
									 isnull(SDB_Authorise,'N') Authorise,
									 isnull(SDB_UserSetup,'N') UserSetup,
                                     
                                     case when convert(decimal(18,0),isnull(PreplanSafety,0)) +
                                     convert(decimal(18,0),isnull(PreplanRockEng,0)) +
                                     convert(decimal(18,0),isnull(PreplanVentilation,0)) +
                                     convert(decimal(18,0),isnull(PreplanHR,0)) +
                                     convert(decimal(18,0),isnull(PreplanSurvey,0)) +
                                     convert(decimal(18,0),isnull(PreplanGeology,0)) + 
                                     convert(decimal(18,0),isnull(PreplanEngineering,0))
                                     > 0 then 'Y' else 'N' end as PreplanAccess,
                                     case when planning = 1 then 'Y' when planning = 2 then 'Y' else 'N' end as Planning,
                                     case when MO_Scrutiny = 1 then 'Y' else 'N' end as MO_Scrutiny,

                                     case when BK_DS = 1 then 'Y' else 'N' end as BK_DS,
									 case when BK_NS = '1' then 'Y' else 'N' end as BK_NS,

                                     case when CaptureRockEng = 1 then 'Y' else 'N' end as CaptureRockEng,
									 case when CaptureGeology = 1 then 'Y' else 'N' end as CaptureGeology,
									 case when CaptureVent = 1 then 'Y' else 'N' end as CaptureVent,
									 case when CaptureSurvey = 1 then 'Y' else 'N' end as CaptureSurvey,

                                     case when OCR_Schedular = 1 then 'Y' else 'N' end as OCRSchedular,

                                     case when Vamping_PreInspection = 1 then 'Y' else 'N' end as Vamping_PreInspection,
                                     case when Vamping_Planning = 1 then 'Y' else 'N' end as Vamping_Planning,
                                     case when Vamping_Booking = 1 then 'Y' else 'N' end as Vamping_Booking,

                                     case when LongHoleDrilling_Planning = 1 then 'Y' else 'N' end as LongHoleDrilling_Planning,
                                     case when LongHoleDrilling_Booking = 1 then 'Y' else 'N' end as LongHoleDrilling_Booking,
                                     case when LongHoleDrilling_Setup = 1 then 'Y' else 'N' end as LongHoleDrilling_Setup,

                                     case when Section = '' then 'Total Mine' else isnull(Section, 'Total Mine') end as Section
                                     FROM [dbo].[tbl_Users_Synchromine]
                                     WHERE [UserID] = '" + userID + "'";

            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            _SyncrominePermissions = string.Empty;
            if (_dbAction.ResultsDataTable?.Rows?.Count == 1)
            {
                for (int x = 0; x < _dbAction.ResultsDataTable.Columns.Count; x++)
                    _SyncrominePermissions += _dbAction.ResultsDataTable.Rows[0][x].ToString();
            }
            else
            {
                //Give the user no privlages           
                _SyncrominePermissions = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";
            }

        }
        /// <summary>
        /// Gets the permissions for the SDB
        /// </summary>
        /// <param name="userID"></param>
        private static void setupSDBPermissions(string userID)
        {
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbAction.SqlStatement = @"SELECT [Plan],[InitializeActandTask],[MapBlueprint],[Booking],[Authorise],[UsersSetup]
                                    FROM [dbo].[tbl_SDB_Users]
                                    WHERE [UserID] = '" + userID + "'";
            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            _sdbPermissions = string.Empty;
            if (_dbAction.ResultsDataTable.Rows.Count == 1)
            {
                for (int x = 0; x < _dbAction.ResultsDataTable.Columns.Count; x++)
                    _sdbPermissions += _dbAction.ResultsDataTable.Rows[0][x].ToString();
            }
        }

        /// <summary>
        /// Sets the book section
        /// </summary>
        /// <param name="userID">The user id to set the book section for</param>
        private static void setupBookSection(string userID)
        {
            MWDataManager.clsDataAccess _dbAction = new MWDataManager.clsDataAccess();
            _dbAction.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbAction.SqlStatement = @"SELECT *
                                    FROM [dbo].[tbl_Users]
                                    WHERE [UserID] = '" + userID + "'";
            _dbAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbAction.ExecuteInstruction();

            _bookSection = string.Empty;
            if (_dbAction.ResultsDataTable.Rows.Count == 1)
            {
                _bookSection = _dbAction.ResultsDataTable.Rows[0]["PasSectionid"].ToString();
            }
        }

        /// <summary>
        /// If a user has permission to a plugin item
        /// </summary>
        /// <param name="itemID">The itemID of the plugin menu</param>
        /// <returns>true if user has permission</returns>
        public static bool HasPermissionPluginItem(string itemID)
        {
            if (_pluginPermissions.ContainsKey(itemID))
            {
                int level = _pluginPermissions[itemID];
                return level == 2 || level == 1;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If the user has permission to perform an action on preplanning 
        /// </summary>
        /// <param name="permission">The preplanning permission you want to check</param>        
        /// <returns></returns>
        public static bool HasSyncrominePermission(SyncrominePermissions permission)
        {
            if (string.IsNullOrWhiteSpace(_SyncrominePermissions))
            {
                return false;
            }
            else
            {
                return _SyncrominePermissions[(int)permission] == 'Y';
            }
        }

        /// <summary>
        /// If the user has permission to perform an action on SDB 
        /// </summary>
        /// <param name="permission">The SDB permission you want to check</param>        
        /// <returns></returns>
        public static bool HasPermissionSDB(SDBPermissions permission)
        {
            if (string.IsNullOrWhiteSpace(_sdbPermissions))
            {
                return false;
            }
            else
            {
                return _sdbPermissions[(int)permission] == 'Y';
            }
        }

    }

    /// <summary>
    /// The pre planning permissions
    /// </summary>
    public enum SyncrominePermissions
    {
        SafetyDep = 0,
        RockEngDep = 1,
        VentDep = 2,
        CostingDep = 3,
        HRDep = 4,
        SurveyDep = 5,
        GeologyDep = 6,
        EngDep = 7,
        SafetyDepAuth = 8,
        RockEngDepAuth = 9,
        VentDepAuth = 10,
        CostingDepAuth = 11,
        HRDepAuth = 12,
        SurveyDepAuth = 13,
        GeologyDepAuth = 14,
        EngDepAuth = 15,

        SafetyStartUp = 16,
        RockEngStartUp = 17,
        PlanningStartUp = 18,
        SurveyStartUp = 19,
        GeologyStartUp = 20,
        MiningStartup = 21,
        VentStartUp = 22,
        DepartmentStartUp = 23,
        SafetyStartUpAuth = 24,
        RockEngStartUpAuth = 25,
        PlanningStartUpAuth = 26,
        SurveyStartUpAuth = 27,
        GeologyStartUpAuth = 28,
        MiningStartupAuth = 29,
        VentStartUpAuth = 30,
        DepartmentStartUpAuth = 31,
        OreflowDesign = 32,
        OreflowBackfill = 33,
        
        SDBPlan = 34,
        SDBInitTasks = 35,
        SDBMapBluePrint = 36,
        SDBBookings = 37,
        SDBAuth= 38,
        SDBUserSetup = 39,
        
        PrePlanAccess = 40,
        Planning = 41,
        MOScrutiny = 42,

        Book_DS = 43,
        Book_NS = 44,

        CaptureRockEng = 45,
        CaptureGeology = 46,
        CaptureVent = 47,
        CaptureSurvey = 48,

        OCRSchedular = 49,

        Vamping_PreInspection = 50,
        Vamping_Planning = 51,
        Vamping_Booking = 52,

        LongHoleDrilling_Planning = 53,
        LongHoleDrilling_Booking = 54,
        LongHoleDrilling_Setup = 55,


    }

    /// <summary>
    /// The SDB Permissions
    /// </summary>
    public enum SDBPermissions
    {
        Plan = 0,
        InitializeActandTask = 1,
        MapBlueprint = 2,
        Booking = 3,
        Authorise = 4,
        UsersSetup = 5
    }

}
