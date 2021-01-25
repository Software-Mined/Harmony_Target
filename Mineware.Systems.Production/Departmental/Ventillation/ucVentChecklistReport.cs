using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Ventillation
{
    public partial class ucVentChecklistReport : BaseUserControl
    {
        private string ReportFileName = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        
        Report theReport = new Report();

        public string _ucMonthDate;
        public string _ucWorkPlace;
        public string _ucCheckListID;
        public string _ucSection;
        public string _prodMonth;
        public string _picPath;
        public string _ucCrew;

        public string ImageForReport;

        public string _FrmType;
        public string _TotScore, _TotWeight, _SWpercentage;
        public string _Auth;
        public ucVentChecklistReport()
        {
            InitializeComponent();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int Days = 45;
            int Activity = 0;
            if (_FrmType == "Stoping")
            {
                MWDataManager.clsDataAccess _dbManField = new MWDataManager.clsDataAccess();
                _dbManField.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManField.SqlStatement = "select  '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' mine, * from tbl_Dept_Inspection_VentCapture_FeildBook where calendardate = '" + _ucMonthDate + "' and Section = '" + _ucSection + "' ";
                _dbManField.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManField.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManField.ResultsTableName = "FieldBook";  //get table name
                _dbManField.ExecuteInstruction();

                DataSet ReportDataTable1 = new DataSet();
                ReportDataTable1.Tables.Add(_dbManField.ResultsDataTable);



                //Per Workplace Data
                MWDataManager.clsDataAccess _dbManbanner = new MWDataManager.clsDataAccess();
                _dbManbanner.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManbanner.SqlStatement = "exec sp_Dept_Insection_VentStoping_Questions " + Days + ", '" + _ucSection + "', '" + _ucMonthDate + "' ";
                _dbManbanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManbanner.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManbanner.ResultsTableName = "StopingData";  //get table name
                _dbManbanner.ExecuteInstruction();

                DataSet ReportDataTable = new DataSet();
                ReportDataTable.Tables.Add(_dbManbanner.ResultsDataTable);


                //General
                MWDataManager.clsDataAccess _dbGenData = new MWDataManager.clsDataAccess();
                _dbGenData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbGenData.SqlStatement = "exec sp_Dept_Insection_Vent_Questions_Stoping_PerSection '" + _ucMonthDate + "'," +
                " '" + _ucSection + "' ";
                _dbGenData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbGenData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbGenData.ResultsTableName = "StopingGeneralData";  //get table name
                _dbGenData.ExecuteInstruction();

                DataSet genDt = new DataSet();
                genDt.Tables.Add(_dbGenData.ResultsDataTable);

                ///Refuge Bay
                ///
                MWDataManager.clsDataAccess _dbRefBayData = new MWDataManager.clsDataAccess();
                _dbRefBayData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbRefBayData.SqlStatement = "exec sp_Dept_Insection_Vent_RefugeBay '" + _prodMonth + "', '" + _ucMonthDate + "' " +
                " ,'" + _ucSection + "', " + Activity + " ";
                _dbRefBayData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbRefBayData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbRefBayData.ResultsTableName = "RefugeBayData";  //get table name
                _dbRefBayData.ExecuteInstruction();
                
                DataSet dtRef = new DataSet();
                dtRef.Tables.Add(_dbRefBayData.ResultsDataTable);

                //Available Temperature
                MWDataManager.clsDataAccess _AvlTemp = new MWDataManager.clsDataAccess();
                _AvlTemp.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _AvlTemp.SqlStatement = "exec sp_Dept_Insection_Vent_Questions_Stoping_AvailableTemp '" + _ucMonthDate + "','" + _ucSection + "', " + Activity + " ";
                _AvlTemp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _AvlTemp.queryReturnType = MWDataManager.ReturnType.DataTable;
                _AvlTemp.ResultsTableName = "AvailableTemp";  //get table name
                _AvlTemp.ExecuteInstruction();

                DataSet dsAvlTemp = new DataSet();
                dsAvlTemp.Tables.Add(_AvlTemp.ResultsDataTable);

                //Noise Measurements
                MWDataManager.clsDataAccess _NoiseData = new MWDataManager.clsDataAccess();
                _NoiseData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _NoiseData.SqlStatement = "exec sp_Dept_Insection_VentNoiseMeasurement  " + Days + ",  " + Activity + ", '" + _ucSection + "','" + _ucMonthDate + "' ";
                _NoiseData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _NoiseData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _NoiseData.ResultsTableName = "NoiseMeasurements";  //get table name
                _NoiseData.ExecuteInstruction();

                DataSet dsNoise = new DataSet();
                dsNoise.Tables.Add(_NoiseData.ResultsDataTable);



                ///HeaderData
                ///
                MWDataManager.clsDataAccess _dbHeaderData = new MWDataManager.clsDataAccess();
                _dbHeaderData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbHeaderData.SqlStatement = "Select * from tbl_sectionComplete where sectionid = '" + _ucSection + "' \r\n" +
                                            "and prodmonth = (Select max(prodmonth) from tbl_Planning where calendardate = '" + _ucMonthDate + "' ) ";
                _dbHeaderData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbHeaderData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbHeaderData.ResultsTableName = "HeaderData";  //get table name
                _dbHeaderData.ExecuteInstruction();

                DataSet dsHeader = new DataSet();
                dsHeader.Tables.Add(_dbHeaderData.ResultsDataTable);

                //Actions
                MWDataManager.clsDataAccess _dbManInc = new MWDataManager.clsDataAccess();
                _dbManInc.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManInc.SqlStatement = "select * from tbl_Shec_IncidentsVent where thedate = '" + _ucMonthDate + "' and WPType = '" + _ucSection + "' and Type = 'VSA' ";
                _dbManInc.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManInc.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManInc.ResultsTableName = "Incident";  //get table name
                _dbManInc.ExecuteInstruction();

                DataSet DSIncident = new DataSet();
                DSIncident.Tables.Add(_dbManInc.ResultsDataTable);

                theReport.RegisterData(ReportDataTable1);
                theReport.RegisterData(ReportDataTable);
                theReport.RegisterData(genDt);
                theReport.RegisterData(dsAvlTemp);
                theReport.RegisterData(dsNoise);
                theReport.RegisterData(dtRef);
                theReport.RegisterData(dsHeader);
                theReport.RegisterData(DSIncident);

                theReport.Load(ReportFileName + "MasterStopeReport.frx");

                theReport.SetParameterValue("VetnImage", ImageForReport);
                theReport.SetParameterValue("PicPath", _picPath);
                theReport.SetParameterValue("Auth", _Auth);

                //theReport.Design();

                pcReport.Clear();
                theReport.Prepare();
                theReport.Preview = pcReport;
                theReport.ShowPrepared();
            }

            if (_FrmType == "Development")
            {
                Activity = 1;

                MWDataManager.clsDataAccess _dbManField = new MWDataManager.clsDataAccess();
                _dbManField.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManField.SqlStatement = "select  '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' mine, * from tbl_Dept_Inspection_VentCapture_FeildBook where calendardate = '" + _ucMonthDate + "' and Section = '" + _ucSection + "'  ";
                _dbManField.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManField.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManField.ResultsTableName = "FieldBook";  //get table name
                _dbManField.ExecuteInstruction();

                DataSet ReportDataTableFB = new DataSet();
                ReportDataTableFB.Tables.Add(_dbManField.ResultsDataTable);


                MWDataManager.clsDataAccess _dbManbanner = new MWDataManager.clsDataAccess();
                _dbManbanner.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManbanner.SqlStatement = "Exec sp_Dept_Insection_VentDevelopment_Questions " + Days + ",'" + _ucSection + "','" + _ucMonthDate + "' ";
                _dbManbanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManbanner.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManbanner.ResultsTableName = "DevMainData";  //get table name
                _dbManbanner.ExecuteInstruction();

                DataSet ReportDatasetbanner = new DataSet();
                ReportDatasetbanner.Tables.Add(_dbManbanner.ResultsDataTable);


                //Per Section
                MWDataManager.clsDataAccess _dbManbannersec = new MWDataManager.clsDataAccess();
                _dbManbannersec.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManbannersec.SqlStatement = "exec sp_Dept_Insection_Vent_Questions_Stoping_PerSection_Report '" + _ucMonthDate + "', '" + _ucSection + "' ";
                _dbManbannersec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManbannersec.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManbannersec.ResultsTableName = "DevPerSection";  //get table name
                _dbManbannersec.ExecuteInstruction();

                DataSet ReportDataTableSect = new DataSet();
                ReportDataTableSect.Tables.Add(_dbManbannersec.ResultsDataTable);

                ///Refuge Bay
                ///
                MWDataManager.clsDataAccess _dbRefBayData = new MWDataManager.clsDataAccess();
                _dbRefBayData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbRefBayData.SqlStatement = "exec sp_Dept_Insection_Vent_RefugeBay '" + _prodMonth + "', '" + _ucMonthDate + "' " +
                " ,'" + _ucSection + "', " + Activity + " ";
                _dbRefBayData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbRefBayData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbRefBayData.ResultsTableName = "RefugeBayData";  //get table name
                _dbRefBayData.ExecuteInstruction();

                DataSet dtRef = new DataSet();
                dtRef.Tables.Add(_dbRefBayData.ResultsDataTable);

                //Available Temperature
                MWDataManager.clsDataAccess _AvlTemp = new MWDataManager.clsDataAccess();
                _AvlTemp.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _AvlTemp.SqlStatement = "exec sp_Dept_Insection_Vent_Questions_Stoping_AvailableTemp '" + _ucMonthDate + "','" + _ucSection + "', " + Activity + " ";
                _AvlTemp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _AvlTemp.queryReturnType = MWDataManager.ReturnType.DataTable;
                _AvlTemp.ResultsTableName = "AvailableTemp";  //get table name
                _AvlTemp.ExecuteInstruction();

                DataSet dsAvlTemp = new DataSet();
                dsAvlTemp.Tables.Add(_AvlTemp.ResultsDataTable);

                //Noise Measurements
                MWDataManager.clsDataAccess _NoiseData = new MWDataManager.clsDataAccess();
                _NoiseData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _NoiseData.SqlStatement = "exec sp_Dept_Insection_VentNoiseMeasurement  " + Days + ",  " + Activity + ", '" + _ucSection + "','" + _ucMonthDate + "' ";
                _NoiseData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _NoiseData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _NoiseData.ResultsTableName = "NoiseMeasurements";  //get table name
                _NoiseData.ExecuteInstruction();

                DataSet dsNoise = new DataSet();
                dsNoise.Tables.Add(_NoiseData.ResultsDataTable);

                ///HeaderData
                ///
                MWDataManager.clsDataAccess _dbManbanner1 = new MWDataManager.clsDataAccess();
                _dbManbanner1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManbanner1.SqlStatement = "Select *, '" + UserID + "' insp,  '" + String.Format("{0:yyyy-MM-dd}", _ucMonthDate) + "'  dd from tbl_SectionComplete where sectionid = '" + _ucSection + "' \r\n" +
                                            "and prodmonth = (Select max(prodmonth) from tbl_Planning where calendardate = '" + _ucMonthDate + "' ) ";
                _dbManbanner1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManbanner1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManbanner1.ResultsTableName = "HeaderData";  //get table name
                _dbManbanner1.ExecuteInstruction();

                MWDataManager.clsDataAccess _dbManInc = new MWDataManager.clsDataAccess();
                _dbManInc.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManInc.SqlStatement = "select * from tbl_Shec_IncidentsVent where thedate = '" + _ucMonthDate + "' and WPType = '" + _ucSection + "' and Type = 'VSA' ";
                _dbManInc.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManInc.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManInc.ResultsTableName = "Incident";  //get table name
                _dbManInc.ExecuteInstruction();

                DataSet DSIncident = new DataSet();
                DSIncident.Tables.Add(_dbManInc.ResultsDataTable);

                DataSet dsHeader = new DataSet();
                dsHeader.Tables.Add(_dbManbanner1.ResultsDataTable);

                theReport.RegisterData(ReportDataTableFB);
                theReport.RegisterData(ReportDatasetbanner);
                theReport.RegisterData(ReportDataTableSect);
                theReport.RegisterData(dsHeader);
                theReport.RegisterData(DSIncident);
                theReport.RegisterData(dtRef);
                theReport.RegisterData(dsAvlTemp);
                theReport.RegisterData(dsNoise);

                theReport.Load(ReportFileName + "MasterStopeDevReport.frx");

                theReport.SetParameterValue("VetnImage", ImageForReport);
                theReport.SetParameterValue("PicPath", _picPath);
                theReport.SetParameterValue("Auth", _Auth);

                //theReport.Design();

                pcReport.Clear();
                theReport.Prepare();
                theReport.Preview = pcReport;
                theReport.ShowPrepared();
            }

            if (_FrmType == "Other")
            {

                MWDataManager.clsDataAccess _batDb = new MWDataManager.clsDataAccess();
                _batDb.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _batDb.SqlStatement = " exec sp_Dept_Insection_Vent_OtherQuestions '" + String.Format("{0:yyyy-MM-dd}", _ucMonthDate) + "', \r\n "
                + " '" + _ucSection + "', '" + _ucWorkPlace + "', '" + _ucCheckListID + "' \r\n ";
                _batDb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _batDb.queryReturnType = MWDataManager.ReturnType.DataTable;


                //Actions                
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = "select * from  tbl_Shec_IncidentsVent where Workplace = '" + _ucWorkPlace + "' and TheDate = '" + String.Format("{0:yyyy-MM-dd}", _ucMonthDate) + "' ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "Actions";
                _dbMan.ExecuteInstruction();

                DataSet ds1 = new DataSet();
                ds1.Tables.Add(_dbMan.ResultsDataTable);

                theReport.RegisterData(ds1);

                if (_ucCheckListID == "Battery Bay")
                {
                    _batDb.ResultsTableName = "BatteryBayData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);

                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\BatteryBay.frx");

                }


                if (_ucCheckListID == "Booster Fans")
                {
                    _batDb.ResultsTableName = "BoosterFansData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\BoosterFans.frx");

                }


                if (_ucCheckListID == "UG Conveyor Belt")
                {
                    _batDb.ResultsTableName = "UGConveyorBeltData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\UGConveyorBelt.frx");

                }


                if (_ucCheckListID == "Mini Subs")
                {
                    _batDb.ResultsTableName = "MiniSubsData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\MiniSubs.frx");

                }


                if (_ucCheckListID == "Sub Station")
                {
                    _batDb.ResultsTableName = "SubStationData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\SubStation.frx");

                }


                if (_ucCheckListID == "UG Store")
                {
                    _batDb.ResultsTableName = "UGStoreData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\UGStore.frx");

                }


                if (_ucCheckListID == "Toilets UI")
                {
                    _batDb.ResultsTableName = "ToiletsUIData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\ToiletsUI.frx");

                }


                if (_ucCheckListID == "Loco Survey")
                {
                    _batDb.ResultsTableName = "LocoSurveyData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\LocoSurvey.frx");

                }


                if (_ucCheckListID == "Workshop - Completed")
                {
                    _batDb.ResultsTableName = "WorkshopCompletedData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\WorkshopCompleted.frx");

                }


                if (_ucCheckListID == "Diamond Drill Inspection")
                {
                    _batDb.ResultsTableName = "DiamondDrillData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\DiamondDrillInspection.frx");
                }



                if (_ucCheckListID == "Volume")
                {
                    _batDb.ResultsTableName = "VolumeData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\Volume.frx");

                }

                if (_ucCheckListID == "Change House Laundry")
                {
                    _batDb.ResultsTableName = "ChangeHouseLaundryData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\ChangeHouseLaundry.frx");

                }

                if (_ucCheckListID == "Trackless Equipment Inspection")
                {
                    _batDb.ResultsTableName = "TracklessEquipmentData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\TracklessEquipment.frx");

                }

                if (_ucCheckListID == "Refuge Bay")
                {
                    _batDb.ResultsTableName = "RefugeBayData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);

                    theReport.RegisterData(batDs);

                    theReport.Load(TGlobalItems.ReportsFolder + "\\RefugeBay.frx");

                }

                //Parameters
                theReport.SetParameterValue("Banner", ProductionGlobal.ProductionGlobalTSysSettings._Banner);
                theReport.SetParameterValue("Image", _picPath);
                theReport.SetParameterValue("Workplace", _ucWorkPlace);
                theReport.SetParameterValue("Section", _ucSection);
                theReport.SetParameterValue("CaptureDate", String.Format("{0:yyyy-MM-dd}", _ucMonthDate));
                theReport.SetParameterValue("Auth", _Auth);

                //theReport.Design();
                
                pcReport.Clear();
                theReport.Prepare();
                theReport.Preview = pcReport;
                theReport.ShowPrepared();
            }
        }

        private void ucVentChecklistReport_Load(object sender, EventArgs e)
        {
            barButtonItem2_ItemClick(null, null);
        }

    }
}
