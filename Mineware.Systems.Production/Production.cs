using Mineware.Menu.Structure;
using Mineware.Plugin.Interface;
using Mineware.Systems.Global;
using Mineware.Systems.Global.ReportsControls;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.GlobalExtensions;
using Mineware.Systems.Printing;
using Mineware.Systems.Production.Bookings;
using Mineware.Systems.Production.Dashboard;
using Mineware.Systems.Production.Menu;
using Mineware.Systems.Production.Planning;
using Mineware.Systems.Production.SysAdmin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mineware.Systems.Production.Security;
using System.Diagnostics;
using Mineware.Systems.ProductionGlobal;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace Mineware.Systems.Production
{
    public class Production : PluginInterface
    {
        public string SystemTag => ProductionRes.systemTag;

        public string SystemDBTag => ProductionRes.systemDBTag;

        public global::DevExpress.XtraNavBar.NavBarItem getApplicationSettingsNavBarItem()
        {
            return null;
        }

        public BaseUserControl getApplicationSettingsScreen()
        {
            return null;
        }

        public BaseUserControl getMainMenuAdditionalItem()
        {
            return null;
        }

        public global::DevExpress.XtraEditors.TileItem getMainMenuItem()
        {
            return null;
        }

        public BaseUserControl getMenuItem(string itemID)
        {
            BaseUserControl theResult = null;
            
            //Booking
            if (itemID == TProductionGlobal.SysMenu.miDailyBookings_apsDailyBookings_MinewareSystemsProduction.ItemID)
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Book_DS);
                if (HasAccess == true)
                {
                    theResult = new ucBookingProduction_Main();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }

                //Load nightshift bookings
                var mimsMainFrm = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                var dynMethod = mimsMainFrm.GetType().GetMethod("LoadSelectedItemScreen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var pfi = new ProfileItem();
                pfi.ItemID = "BookingsNS";
                pfi.SystemID = TProductionGlobal.SysMenu.miDailyBookings_apsDailyBookings_MinewareSystemsProduction.SystemID;
                pfi.Description = "Bookings Night Shift";
                dynMethod.Invoke(mimsMainFrm, new object[] { pfi });

                //Load VampingBookings
                var mimsMainFrm2 = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                var dynMethod2 = mimsMainFrm.GetType().GetMethod("LoadSelectedItemScreen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var pfi2 = new ProfileItem();
                pfi2.ItemID = "VampingBookings";
                pfi2.SystemID = TProductionGlobal.SysMenu.miDailyBookings_apsDailyBookings_MinewareSystemsProduction.SystemID;
                pfi2.Description = "VampingBookings";
                dynMethod2.Invoke(mimsMainFrm, new object[] { pfi2 });


                //Load LongHoleDrillingBookings
                var mimsMainFrm3 = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                var dynMethod3 = mimsMainFrm.GetType().GetMethod("LoadSelectedItemScreen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var pfi3 = new ProfileItem();
                pfi3.ItemID = "LongHoleDrillingBookings";
                pfi3.SystemID = TProductionGlobal.SysMenu.miDailyBookings_apsDailyBookings_MinewareSystemsProduction.SystemID;
                pfi3.Description = "LongHoleDrillingBookings";
                dynMethod2.Invoke(mimsMainFrm, new object[] { pfi3 });




            }

            //Night shift booking
            if (itemID == "BookingsNS")
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Book_NS);
                if (HasAccess == true)
                {
                    theResult = new ucBookingProduction_MainNS();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }

            //vamping bookings
            if (itemID == "VampingBookings")
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Vamping_Booking);
                if (HasAccess == true)
                {
                    theResult = new Departmental.Vamping.ucVampingBooking();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }

            //Long Hole Drilling bookings
            if (itemID == "LongHoleDrillingBookings")
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.LongHoleDrilling_Booking);
                if (HasAccess == true)
                {
                    theResult = new Bookings.ucBookings_LongHoleDrilling();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }

            //Planning
            if (itemID == TProductionGlobal.SysMenu.miPlanning_apsPlanning_MinewareSystemsProduction.ItemID)
            {

                bool HasPlanAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Planning);
                if (HasPlanAccess == true)
                {
                    theResult = new ucPlanningMain();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }

                ///MOScrutiny
                bool HasMOScrutinyAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.MOScrutiny);

                if (HasMOScrutinyAccess == true)
                {
                    //Load preplanning
                    var mimsMainFrm = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                    var dynMethod = mimsMainFrm.GetType().GetMethod("LoadSelectedItemScreen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var pfi = new ProfileItem();
                    pfi.ItemID = "MOScrutiny";
                    pfi.SystemID = TProductionGlobal.SysMenu.miPlanning_apsPlanning_MinewareSystemsProduction.SystemID;
                    pfi.Description = "MOScrutiny";
                    dynMethod.Invoke(mimsMainFrm, new object[] { pfi });
                }
               
                bool HasPrePlanAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.PrePlanAccess);

                if (HasPrePlanAccess == true)
                {
                    //Load preplanning
                    var mimsMainFrm = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                    var dynMethod = mimsMainFrm.GetType().GetMethod("LoadSelectedItemScreen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var pfi = new ProfileItem();
                    pfi.ItemID = "PrePlanning";
                    pfi.SystemID = TProductionGlobal.SysMenu.miPlanning_apsPlanning_MinewareSystemsProduction.SystemID;
                    pfi.Description = "Pre-Planning";
                    dynMethod.Invoke(mimsMainFrm, new object[] { pfi });
                }


                bool HasLongHoleAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.LongHoleDrilling_Planning);

                if (HasLongHoleAccess == true)
                {
                    //Load preplanning
                    var mimsMainFrm = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                    var dynMethod = mimsMainFrm.GetType().GetMethod("LoadSelectedItemScreen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var pfi = new ProfileItem();
                    pfi.ItemID = "LongholeDrillingPlanning";
                    pfi.SystemID = TProductionGlobal.SysMenu.miPlanning_apsPlanning_MinewareSystemsProduction.SystemID;
                    pfi.Description = "LongholeDrillingPlanning";
                    dynMethod.Invoke(mimsMainFrm, new object[] { pfi });
                }



            }

            //MOScrutiny
            if (itemID == "MOScrutiny")
            {
                bool HasMOScrutinyAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.MOScrutiny);

                if (HasMOScrutinyAccess == true)
                {
                    theResult = new ucMOScrutiny();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }

            //PrePlanning
            if (itemID == "PrePlanning")
            {
                bool HasPrePlanAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.PrePlanAccess);

                if (HasPrePlanAccess == true)
                {
                    theResult = new ucGraphicsPrePlanMain();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }


            //LongholeDrillingPlanning
            if (itemID == "LongholeDrillingPlanning")
            {
                bool HasPrePlanAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.LongHoleDrilling_Planning);

                if (HasPrePlanAccess == true)
                {
                    theResult = new Mineware.Systems.Production.Departmental.LongHoleDrilling.ucLongHoleDrilling();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }




            //System Admin Screens
            if (itemID == TProductionGlobal.SysMenu.miWorkplaceAdmin_apsWorkplace_MinewareSystemsProduction.ItemID)
            {
                theResult = new ucWorkplaces();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            if (itemID == TProductionGlobal.SysMenu.miProblemsSetup_apsProblems_MinewareSystemsProduction.ItemID)
            {
                var mimsActiveFrm = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                ucSysAdmin_Problems theResultPopUp = new ucSysAdmin_Problems();
                
                //theResult.CanClose = true;
                XtraForm PopupForm = new XtraForm();
                PopupForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                PopupForm.ShowInTaskbar = false;
                PopupForm.Text = String.Format("{0} - {1}", TUserInfo.Site, itemID);
                PopupForm.Width = mimsActiveFrm.Width - 20;
                PopupForm.Height = mimsActiveFrm.Height - 250;
                PopupForm.Controls.Add(theResultPopUp);
                theResultPopUp.Dock = DockStyle.Fill;
                PopupForm.StartPosition = FormStartPosition.Manual;
                PopupForm.Top = mimsActiveFrm.Top + 250;
                PopupForm.Left = mimsActiveFrm.Left;
                PopupForm.Show();
                ////theResult = PopupForm;
                ////theResult.ScreenStatus = ScreenStatus.Edit;
                ////theResult.Controls.Add(PopupForm);
                ////theResult.CanClose = true;
            }
            
           

            if (itemID == TProductionGlobal.SysMenu.miCalendarSetup_apsCalendar_MinewareSystemsProduction.ItemID)
            {
                theResult = new ucSysAdmin_Calendars();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            if (itemID == TProductionGlobal.SysMenu.miCyclesSetup_apsCycle_MinewareSystemsProduction.ItemID)
            {
                theResult = new ucSysAdmin_CycleSetup();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            if (itemID == TProductionGlobal.SysMenu.miProductionUserSetup_apsUserRights_MinewareSystemsProduction.ItemID)
            {
                theResult = new ucUserRight_Main();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //Analysis
            if (itemID == TProductionGlobal.SysMenu.miCrewAnalysis_apsCrewAnalysis_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.Analysis.ucPerformance();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //PlanVsBook
            if (itemID == TProductionGlobal.SysMenu.miPlanvsBook_apsPlanVsBook_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.PlanvsBook.ucPlanVsBook();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //BlastingComp
            if (itemID == TProductionGlobal.SysMenu.miBlastingCompliance_apsBlastingComp_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.BlastingCompliance.ucBlastingCompliance();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //LineAction_OpenActions
            if (itemID == TProductionGlobal.SysMenu.miLineActionManager_apsLineActionManager_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.LineActionManager.ucLineActionManager_CloseAction();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }
            

            //Doc Manager
            if (itemID == TProductionGlobal.SysMenu.miDocumentManager_apsReportingDocManage_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Departmental.Survey.ucDocManager();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //Lost Blast Analysis
            if (itemID == TProductionGlobal.SysMenu.miLostBlastAnalysis_apsReportingLostBlastAnalysis_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.Analysis.ucLostBlastAnalysis();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //Measuring List
            if (itemID == TProductionGlobal.SysMenu.miMeasuringList_apsReportingMeasList_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucMeasList();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //MO Daily
            if (itemID == TProductionGlobal.SysMenu.miMODaily_apsReportingMoDaily_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucMODailyReport();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //Ore Flow Diagram
            if (itemID == TProductionGlobal.SysMenu.miOreFlowDiagram_apsReportingOreflow_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.OreflowDiagram.ucDiagram();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //Planning Report
            if (itemID == TProductionGlobal.SysMenu.miPlanningReport_apsReportingPlanningReport_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucPlanningReport();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //Six Shift Report
            if (itemID == TProductionGlobal.SysMenu.miSixShiftRecon_apsReportingSixShift_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucSixShiftReport();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //Worst Performer
            if (itemID == TProductionGlobal.SysMenu.miWorstPerformer_apsReportingWorstPerformer_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucWorstPerformer();
                //theResult = new Mineware.Systems.Production.Reporting.ucKpiReport();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            ///SDB
            ///Add
            if (itemID == TProductionGlobal.SysMenu.miInitializeActivitiesandTasks_apsSDBSetupActivities_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Logistics_Management.ucSDB_AddAcitvites();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }
            //Map BluePrint
            if (itemID == TProductionGlobal.SysMenu.miMapBlueprint_apsSDBMapBluePrint_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Logistics_Management.ucSDB_Activities();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }
            //plan
            if (itemID == TProductionGlobal.SysMenu.miPlanningServices_apsSDBPlan_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Logistics_Management.ucSDBPlanning();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }
            //Book
            if (itemID == TProductionGlobal.SysMenu.miBookingServices_apsSDBBook_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Logistics_Management.ucSDBBookingMain();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }

            //Departmental Capture
            if (itemID == TProductionGlobal.SysMenu.miRockEng_apsDepartmentRockEng_MinewareSystemsProduction.ItemID)
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureRockEng);
                if (HasAccess == true)
                {
                    theResult = new Mineware.Systems.Production.Departmental.RockEngineering.ucRoutinVisit();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }

            //Departmental Capture
            if (itemID == TProductionGlobal.SysMenu.miGeology_apsDepartmentGeology_MinewareSystemsProduction.ItemID)
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureGeology);
                if (HasAccess == true)
                {
                    theResult = new Mineware.Systems.Production.Departmental.Geology.ucGeoWalkAbout();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }

            //Ventilation Inspection
            if (itemID == TProductionGlobal.SysMenu.miVentilation_apsDepartmentVentilation_MinewareSystemsProduction.ItemID)
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureVent);
                if (HasAccess == true)
                {
                    theResult = new Mineware.Systems.Production.Departmental.Ventillation.ucVentMain();
                    theResult.CanClose = true; // set the CanClose to true if the can close   
                }

            }

            //Departmental Capture
            if (itemID == TProductionGlobal.SysMenu.miSurvey_apsDepartmentSurvey_MinewareSystemsProduction.ItemID)
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureSurvey);
                if (HasAccess == true)
                {
                    theResult = new Mineware.Systems.Production.Departmental.Survey.ucSurveyNote();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }

                var mimsMainFrm = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                var dynMethod = mimsMainFrm.GetType().GetMethod("LoadSelectedItemScreen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var pfi = new ProfileItem();
                pfi.ItemID = "PegsCapture";
                pfi.SystemID = TProductionGlobal.SysMenu.miSurvey_apsDepartmentSurvey_MinewareSystemsProduction.SystemID;
                pfi.Description = "Pegs-Capture";
                dynMethod.Invoke(mimsMainFrm, new object[] { pfi });
            }

            //PegsCapture
            if (itemID == "PegsCapture")
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureSurvey);
                if (HasAccess == true)
                {
                    theResult = new Mineware.Systems.Production.Departmental.Survey.PegValuesUserClass();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }

            //Oreflow Diagram
            if (itemID == TProductionGlobal.SysMenu.miOreFlowDiagram_apsReportingOreflow_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.OreflowDiagram.ucDiagram();
                theResult.CanClose = true; // set the CanClose to true if the can close
            }



            //Generic Report
            if (itemID == TProductionGlobal.SysMenu.miGenericReport_apsGenericReport_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.GenericReport.ucGenericReport();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }

            //Month End Recon
            if (itemID == TProductionGlobal.SysMenu.miMonthEndRecon_apsMonthEndRecon_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucMonthEndRecon();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }

            //KPI
            if (itemID == TProductionGlobal.SysMenu.miKPIReport_apsKPI_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucKpiReport();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }


            //Workplace Stoppage
            if (itemID == TProductionGlobal.SysMenu.miWorkplaceStoppages_apsWorkplaceStoppages_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.SysAdmin.ucStopWorkplace();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }

            //Shiftboss Cycle Report
            if (itemID == TProductionGlobal.SysMenu.miShiftbossCycleReport_apsShiftBossCycleReport_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucShiftbossCycle();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }

            ////Equipment Setup
            if (itemID == TProductionGlobal.SysMenu.miEquipmentSetup_apsEquipmentSetup_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.SysAdmin.ucEquipmentType();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }


            //Departmental Capture
            if (itemID == TProductionGlobal.SysMenu.miEngineering_apDepartmentEngineering_MinewareSystemsProduction.ItemID)
            {
                //bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureSurvey);
                //if (HasAccess == true)
                //{
                //theResult = new Mineware.Systems.Production.SysAdmin.ucEquipmentType();
                theResult = new Mineware.Systems.Production.Departmental.Engineering.ucEngDailyBook();
                theResult.CanClose = true; // set the CanClose to true if the can close
                //}

              
            }

           

            if (itemID == TProductionGlobal.SysMenu.miEngineeringBreakdowns_apsEngineeringBreakdowns_MinewareSystemsProduction.ItemID)
            {               
                theResult = new Mineware.Systems.Production.Departmental.Engineering.ucEngBreakDowns();
                theResult.CanClose = true; 
            }

            //Recon Compliance
            if (itemID == TProductionGlobal.SysMenu.miReconCompliance_apsReconCompliance_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucReconCompliance();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }

            

            //Planning Report New Style
            if (itemID == TProductionGlobal.SysMenu.miGenericPlanningReport_apsPlanningReportNew_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.PlanningNewStyle.ucPlanningReportNewStyle();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }

            //High Risk
            if (itemID == TProductionGlobal.SysMenu.miHighRiskReport_apsHighRiskReport_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.ucHighRisk();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }


            //Email Setup
            if (itemID == TProductionGlobal.SysMenu.miEmailSetup_apsEmailSetup_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.SysAdmin.ucEmailSetup();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }

            //OCR Schedular
            if (itemID == TProductionGlobal.SysMenu.miOCRSchedular_apsOCRSchedular_MinewareSystemsProduction.ItemID)
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.OCRSchedular);
                if (HasAccess == true)
                {
                    theResult = new Mineware.Systems.Production.OCRScheduling.ucOCRSchedulingMain();
                    theResult.CanClose = true; // set the CanClose to true if the can close      
                }

            }

            //Vamping
            if (itemID == TProductionGlobal.SysMenu.miVamping_apsDepartmentVamping_MinewareSystemsProduction.ItemID)
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Vamping_PreInspection);
                if (HasAccess == true)
                {
                    theResult = new Mineware.Systems.Production.Departmental.Vamping.ucVamping();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }

                var mimsMainFrm = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                var dynMethod = mimsMainFrm.GetType().GetMethod("LoadSelectedItemScreen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var pfi = new ProfileItem();
                pfi.ItemID = "VampingPlanning";
                pfi.SystemID = TProductionGlobal.SysMenu.miVamping_apsDepartmentVamping_MinewareSystemsProduction.SystemID;
                pfi.Description = "Vamping Planning";
                dynMethod.Invoke(mimsMainFrm, new object[] { pfi });
            }

            //VampingPlanning
            if (itemID == "VampingPlanning")
            {
                bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Vamping_Planning);
                if (HasAccess == true)
                {
                    theResult = new Mineware.Systems.Production.Departmental.Vamping.ucVampPlanning();
                    theResult.CanClose = true; // set the CanClose to true if the can close
                }
            }


            //WorkplaceSummary
            if (itemID == TProductionGlobal.SysMenu.miWorkplaceSummary_apsWorkplaceSummary_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.WorkplaceSummary.ucWorkplaceSummary();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }

            //HR
            if (itemID == TProductionGlobal.SysMenu.miStandardAndNorms_apsStdAndNorms_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.SysAdmin.ucHRStandardAndNorms();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }

            //Early Morning Labour
            if (itemID == TProductionGlobal.SysMenu.miLabourReport_miEarlyMorningLabour_MinewareSystemsProduction.ItemID)
            {
                theResult = new Mineware.Systems.Production.Reporting.Labour.ucEarlyMorningReport();
                theResult.CanClose = true; // set the CanClose to true if the can close            

            }          


            return theResult;
        }

        public mainMenu getMenuStructure()
        {
            return TProductionGlobal.SysMenu.theMenu;
        }

        public List<clsParameters> getParameters()
        {
            return null;
        }

        public ReportSettings getReportSettings(string itemID)
        {
            return null;
        }

        public BaseUserControl getStartScreen()
        {
            ucDashboardWidgetView _ucDashboardWidgetView = new ucDashboardWidgetView();
            if (!Debugger.IsAttached)
            {
                _ucDashboardWidgetView.ShowProgressPage(true);
            }
            return _ucDashboardWidgetView;
            


        }

        public string getSystemDBTag()
        {
            return ProductionRes.systemDBTag;
        }

        public string getSystemTag()
        {
            return ProductionRes.systemTag;
        }

        public global::DevExpress.XtraNavBar.NavBarItem getUserSettingsNavBarItem()
        {
            return null;
        }

        public BaseUserControl getUserSettingsScreen(ScreenStatus _theScreenStatus, string _userID, TUserCurrentInfo userInfo, string theConnection)
        {
            return null;
        }

        public void InitializeModule()
        {
            TProductionGlobal.SysMenu.setMenuItems();
            TProductionGlobal.SysMenu.theMenu.systemDBTag = ProductionRes.systemDBTag;
            TProductionGlobal.SysMenu.theMenu.systemTag = ProductionRes.systemTag;
        }

        public void LoggedOn()
        {
            ProductionGlobal.ProductionGlobal.SetProductionGlobalInfo(ProductionRes.systemDBTag);


            //Setup the security
            Mineware.Systems.Production.Security.SecurityPAS.GetPermissionsForUser(TUserInfo.UserID);

            // Add menu==================================================
            // Gets the main mims form mainLoadScreen
            try
            {

                var mainform = System.Windows.Forms.Application.OpenForms["MIMSMain"];
                foreach (var ctrl in mainform.Controls)
                {
                    if (ctrl is DevExpress.XtraBars.Ribbon.RibbonStatusBar)
                    {
                        var tmp = ctrl as DevExpress.XtraBars.Ribbon.RibbonStatusBar;

                        if (tmp.ItemLinks.Where(o => o.DisplayHint == "Menu").Count() == 0)
                        {
                            DevExpress.XtraBars.BarButtonItem btn = new DevExpress.XtraBars.BarButtonItem();
                            btn.AllowDrawArrowInMenu = true;
                            btn.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;

                            btn.Caption = string.Empty;

                            btn.Caption = "";

                            btn.Hint = "Menu";
                            btn.ImageOptions.Image = Mineware.Systems.Production.Properties.Resources.StartMenu;
                            btn.ItemClick += Btn_ItemClick;
                            tmp.Invoke((Action)delegate { tmp.ItemLinks.Insert(0, btn); });
                        }
                    }

                    //Hide Dolf's menu
                    if (ctrl is DevExpress.XtraBars.Ribbon.RibbonControl)
                    {
                        var tmp = ctrl as DevExpress.XtraBars.Ribbon.RibbonControl;
                        //tmp.ApplicationButtonImageOptions.Image = Mineware.Systems.Production.Properties.Resources.StartMenu;
                        tmp.ApplicationButtonImageOptions.SvgImage = Mineware.Systems.Production.Properties.Resources.HomeBlue;
                        Size size = new Size(16, 16);                        
                        tmp.ApplicationButtonImageOptions.SvgImageSize = size;
                        //tmp.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
                    }

                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        /// <summary>
        /// When the menu button is clicked show menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmMenu menu = new frmMenu();
            var menuParent = System.Windows.Forms.Application.OpenForms["MIMSMain"];
            menu.Show(menuParent);
        }

        public IReportModule getReport(string itemID)
        {
            //if (itemID == TProductionGlobal.SysMenu.miUserLogonHistory_SSUsersReport_MinewareSystemsSettings.ItemID)
            //{
            //    return new UserActivityReport();
            //}

            return null;
        }
    }
}
