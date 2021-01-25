using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Mineware.Systems.DocumentManager.Ventilation
{
    public partial class ucVentChecklistReport : BaseUserControl
    {
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

            if (_FrmType == "Stoping")
            {
                // do fieldbookinfo

                MWDataManager.clsDataAccess _dbManField = new MWDataManager.clsDataAccess();
                _dbManField.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _dbManField.SqlStatement = "select  'Amandelbult Mine' mine, * from tbl_Dept_Inspection_VentCapture_FeildBook where calendardate = '" + _ucMonthDate + "' and Section = '" + _ucSection + "' ";
                _dbManField.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManField.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManField.ResultsTableName = "FieldBook";  //get table name
                _dbManField.ExecuteInstruction();

                DataSet ReportDataTable1 = new DataSet();
                ReportDataTable1.Tables.Add(_dbManField.ResultsDataTable);



                //Fire Protection
                MWDataManager.clsDataAccess _dbManbanner = new MWDataManager.clsDataAccess();
                _dbManbanner.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _dbManbanner.SqlStatement = "exec sp_Dept_Insection_VentStoping_Questions " + Days + ", '" + _ucSection + "', '" + _ucMonthDate + "' ";
                _dbManbanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManbanner.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManbanner.ResultsTableName = "StopingData";  //get table name
                _dbManbanner.ExecuteInstruction();

                DataSet ReportDataTable = new DataSet();
                ReportDataTable.Tables.Add(_dbManbanner.ResultsDataTable);


                //General
                MWDataManager.clsDataAccess _dbGenData = new MWDataManager.clsDataAccess();
                _dbGenData.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
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
                frmInspection insCrew = new frmInspection();
                MWDataManager.clsDataAccess _dbRefBayData = new MWDataManager.clsDataAccess();
                _dbRefBayData.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _dbRefBayData.SqlStatement = "exec sp_Dept_Insection_VentStoping_RefugeBay '" + _prodMonth + "', '" + _ucMonthDate + "' " +
                " ,'" + _ucSection + "' ";
                _dbRefBayData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbRefBayData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbRefBayData.ResultsTableName = "RefugeBayData";  //get table name
                _dbRefBayData.ExecuteInstruction();
                
                DataSet dtRef = new DataSet();
                dtRef.Tables.Add(_dbRefBayData.ResultsDataTable);


                ///HeaderData
                ///
                MWDataManager.clsDataAccess _dbHeaderData = new MWDataManager.clsDataAccess();
                _dbHeaderData.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _dbHeaderData.SqlStatement = "Select * from tbl_sectionComplete where sectionid = '" + _ucSection + "' \r\n" +
                                            "and prodmonth = (Select max(prodmonth) from tbl_Planning where calendardate = '" + _ucMonthDate + "' ) ";
                _dbHeaderData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbHeaderData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbHeaderData.ResultsTableName = "HeaderData";  //get table name
                _dbHeaderData.ExecuteInstruction();

                DataSet dsHeader = new DataSet();
                dsHeader.Tables.Add(_dbHeaderData.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManInc = new MWDataManager.clsDataAccess();
                _dbManInc.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _dbManInc.SqlStatement = "select * from tbl_Shec_IncidentsVent where thedate = '" + _ucMonthDate + "' and WPType = '" + _ucSection + "' and Type = 'VSA' ";
                _dbManInc.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManInc.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManInc.ResultsTableName = "Incident";  //get table name
                _dbManInc.ExecuteInstruction();

                DataSet DSIncident = new DataSet();
                DSIncident.Tables.Add(_dbManInc.ResultsDataTable);

                //MWDataManager.clsDataAccess _dbManIncF = new MWDataManager.clsDataAccess();
                //_dbManIncF.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                //_dbManIncF.SqlStatement = "select * from tbl_Shec_IncidentsVent where thedate = '" + _ucMonthDate + "' and WPType = '" + _ucSection + "' and Type = 'VSAF' ";
                //_dbManIncF.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbManIncF.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbManIncF.ResultsTableName = "IncidentF";  //get table name
                //_dbManIncF.ExecuteInstruction();

                //DataSet DSIncidentF = new DataSet();
                //DSIncidentF.Tables.Add(_dbManIncF.ResultsDataTable);


                //theReport.RegisterData(repFire);
                theReport.RegisterData(ReportDataTable1);
                theReport.RegisterData(ReportDataTable);
                theReport.RegisterData(genDt);
                //theReport.RegisterData(winchDt);
                //theReport.RegisterData(stationTemp);
                theReport.RegisterData(dtRef);
                theReport.RegisterData(dsHeader);

                theReport.RegisterData(DSIncident);
                //theReport.RegisterData(DSIncidentF);


                theReport.Load("MasterStopeReport.frx");

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

                MWDataManager.clsDataAccess _dbManField = new MWDataManager.clsDataAccess();
                _dbManField.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _dbManField.SqlStatement = "select  'Amandelbult Mine' mine, * from tbl_Dept_Inspection_VentCapture_FeildBook where calendardate = '" + _ucMonthDate + "' and Section = '" + _ucSection + "'  ";
                _dbManField.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManField.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManField.ResultsTableName = "FieldBook";  //get table name
                _dbManField.ExecuteInstruction();

                DataSet ReportDataTableFB = new DataSet();
                ReportDataTableFB.Tables.Add(_dbManField.ResultsDataTable);


                MWDataManager.clsDataAccess _dbManbanner = new MWDataManager.clsDataAccess();
                _dbManbanner.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _dbManbanner.SqlStatement = "Exec sp_Dept_Insection_VentDevelopment_Questions " + Days + ",'" + _ucSection + "','" + _ucMonthDate + "' ";
                _dbManbanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManbanner.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManbanner.ResultsTableName = "DevMainData";  //get table name
                _dbManbanner.ExecuteInstruction();

                DataSet ReportDatasetbanner = new DataSet();
                ReportDatasetbanner.Tables.Add(_dbManbanner.ResultsDataTable);


                //Per Section
                MWDataManager.clsDataAccess _dbManbannersec = new MWDataManager.clsDataAccess();
                _dbManbannersec.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _dbManbannersec.SqlStatement = "exec sp_Dept_Insection_Vent_Questions_Stoping_PerSection_Report '" + _ucMonthDate + "', '" + _ucSection + "' ";
                _dbManbannersec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManbannersec.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManbannersec.ResultsTableName = "DevPerSection";  //get table name
                _dbManbannersec.ExecuteInstruction();

                DataSet ReportDataTableSect = new DataSet();
                ReportDataTableSect.Tables.Add(_dbManbannersec.ResultsDataTable);

                ///HeaderData
                ///
                MWDataManager.clsDataAccess _dbManbanner1 = new MWDataManager.clsDataAccess();
                _dbManbanner1.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _dbManbanner1.SqlStatement = "Select *, '" + UserID + "' insp,  '" + String.Format("{0:yyyy-MM-dd}", _ucMonthDate) + "'  dd from tbl_SectionComplete where sectionid = '" + _ucSection + "' \r\n" +
                                            "and prodmonth = (Select max(prodmonth) from tbl_Planning where calendardate = '" + _ucMonthDate + "' ) ";
                _dbManbanner1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManbanner1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManbanner1.ResultsTableName = "HeaderData";  //get table name
                _dbManbanner1.ExecuteInstruction();

                MWDataManager.clsDataAccess _dbManInc = new MWDataManager.clsDataAccess();
                _dbManInc.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
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

                theReport.Load("MasterStopeDevReport.frx");

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
                _batDb.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
                _batDb.SqlStatement = " exec sp_Dept_Insection_Vent_OtherQuestions '" + String.Format("{0:yyyy-MM-dd}", _ucMonthDate) + "', \r\n "
                + " '" + _ucSection + "', '" + _ucWorkPlace + "', '" + _ucCheckListID + "' \r\n ";
                _batDb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _batDb.queryReturnType = MWDataManager.ReturnType.DataTable;


                //Actions                
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];
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

                    theReport.Load("BatteryBay.frx");

                }


                if (_ucCheckListID == "Booster Fans")
                {
                    _batDb.ResultsTableName = "BoosterFansData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("BoosterFans.frx");

                }


                if (_ucCheckListID == "UG Conveyor Belt")
                {
                    _batDb.ResultsTableName = "UGConveyorBeltData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("UGConveyorBelt.frx");

                }


                if (_ucCheckListID == "Mini Subs")
                {
                    _batDb.ResultsTableName = "MiniSubsData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("MiniSubs.frx");

                }


                if (_ucCheckListID == "Sub Station")
                {
                    _batDb.ResultsTableName = "SubStationData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("SubStation.frx");

                }


                if (_ucCheckListID == "UG Store")
                {
                    _batDb.ResultsTableName = "UGStoreData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("UGStore.frx");

                }


                if (_ucCheckListID == "Toilets UI")
                {
                    _batDb.ResultsTableName = "ToiletsUIData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("ToiletsUI.frx");

                }


                if (_ucCheckListID == "Loco Survey")
                {
                    _batDb.ResultsTableName = "LocoSurveyData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("LocoSurvey.frx");

                }


                if (_ucCheckListID == "Workshop - Completed")
                {
                    _batDb.ResultsTableName = "WorkshopCompletedData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("WorkshopCompleted.frx");

                }


                if (_ucCheckListID == "Diamond Drill Inspection")
                {
                    _batDb.ResultsTableName = "DiamondDrillData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("DiamondDrillInspection.frx");
                }



                if (_ucCheckListID == "Volume")
                {
                    _batDb.ResultsTableName = "VolumeData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("Volume.frx");

                }

                if (_ucCheckListID == "Change House Laundry")
                {
                    _batDb.ResultsTableName = "ChangeHouseLaundryData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("ChangeHouseLaundry.frx");

                }

                if (_ucCheckListID == "Trackless Equipment Inspection")
                {
                    _batDb.ResultsTableName = "TracklessEquipmentData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);


                    theReport.RegisterData(batDs);

                    theReport.Load("TracklessEquipment.frx");

                }

                if (_ucCheckListID == "Refuge Bay")
                {
                    _batDb.ResultsTableName = "RefugeBayData";  //get table name
                    _batDb.ExecuteInstruction();

                    DataSet batDs = new DataSet();
                    batDs.Tables.Add(_batDb.ResultsDataTable);

                    theReport.RegisterData(batDs);

                    theReport.Load("RefugeBay.frx");

                }

                //Parameters
                theReport.SetParameterValue("Banner", "Amandelbult Mine");
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
