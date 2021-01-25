using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

using Newtonsoft.Json;
using DevExpress.XtraPdfViewer;
using System.IO;
using System.Diagnostics;
using Mineware.Systems.Production.LineActionManager.Models;
using Mineware.Systems.GlobalConnect;
using FastReport;

namespace Mineware.Systems.Production.LineActionManager
{
    public partial class frmLineActionManager_Report : DevExpress.XtraEditors.XtraForm
    {
        private string fileID;
        private FormsAPI _Forms;
        private string ReportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        Procedures procs = new Procedures();
        private PrintedForm _PrintedForm;
        List<ListDrop> _items = new List<ListDrop>();
        List<DataRow> list = new List<DataRow>();
        Report theReport = new Report();
        Report theReport2 = new Report();
        Report theReport3 = new Report();
        Report theReport4 = new Report();
        List<ListForms> _listForms = new List<ListForms>();
        private bool MoveToProdind = false;
        private int _PrintBatchNumber = 20;
        private bool _loaded = false;
        public DataTable dt = new DataTable();
        DataTable dtNew = new DataTable();
        //public TUserCurrentInfo UserCurrentInfo;
        private BackgroundWorker MovetoProd = new BackgroundWorker();
        public frmLineActionManager_Report()
        {
            InitializeComponent();
        }

        #region Private variables
        private DataTable dtReceive = new DataTable("Table1");
        private String sqlQueryCompiler;
        private DataSet dsGlobal = new DataSet("dsGlobal");
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        #endregion

        #region public variables
        public Report genReport = new Report();
        public DataTable dtGenReportReceive = new DataTable("dtGenReportReceive");
        public string connection;
        #endregion

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void loadReportData()
        {
            for (int i = 0; i < dtGenReportReceive.Rows.Count; i++)
            {
                sqlQueryCompiler = " EXEC [sp_LineActionManager_Report] @Mineware_Action_ID = '" + dtGenReportReceive.Rows[i]["IncidentNumber"] + "' ";
                sqlConnector(sqlQueryCompiler, "Table1");
            }
            genReport.RegisterData(dsGlobal);
        }

        private void sqlConnector(string sqlQuery, string sqlTableIdentifier)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, connection);
            _sqlConnection.SqlStatement = sqlQuery;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();
            dtReceive = _sqlConnection.ResultsDataTable;

            if (!string.IsNullOrEmpty(sqlTableIdentifier))
            {
                for (int i = 0; i < dsGlobal.Tables.Count; i++)
                {
                    if (dsGlobal.Tables[i].TableName == sqlTableIdentifier)
                    {
                        dsGlobal.Tables[i].Merge(dtReceive);
                    }
                }
            }
        }

        private void bkwrMain_DoWork(object sender, DoWorkEventArgs e)
        {
            loadReportData();
        }

        private void bkwrMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void frmLineActionManager_Report_Load(object sender, EventArgs e)
        {
            simpleButton1_Click(null, null);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count <= 5)
            {
                Load5Incidents();
            }

            if (dt.Rows.Count > 5 && dt.Rows.Count <= 10)
            {
                Load5to10Incidents();
            }

            if (dt.Rows.Count > 10 && dt.Rows.Count <= 15)
            {
                Load10to15Incidents();
            }

            if (dt.Rows.Count > 15 && dt.Rows.Count <= 20)
            {
                Load15to20Incidents();
            }

        }

        void Load5Incidents()
        {
            if (dt.Rows.Count < 5)
            {
                DataRow workRow = dt.NewRow();
                DataRow workRow2 = dt.NewRow();
                DataRow workRow3 = dt.NewRow();
                DataRow workRow4 = dt.NewRow();
                DataRow workRow5 = dt.NewRow();
                dt.Rows.Add(workRow);
                dt.Rows.Add(workRow2);
                dt.Rows.Add(workRow3);
                dt.Rows.Add(workRow4);
                dt.Rows.Add(workRow5);
            }

            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();

            DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            MoveToProdind = true;
            try
            {
                _listForms.Clear();
                _listForms.Add(new ListForms
                {
                    Section = "A114"
                ,
                    WPID = ""
                ,
                    WPName = ""
                ,
                    FID = "150"
                ,
                    UID = 0
                });

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", "150");

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                OCRCL.SelectedValue = "150";
                List<ListForms> _FormsL = new List<ListForms>();
                _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                _Forms.UniqueDataStructure.Rows[0].Delete();

                var allResponses = new List<ListForms>();
                for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                {
                    _Forms.UniqueDataStructure.Rows.Clear();
                    foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                    {
                        try
                        {
                            DataRow row3;
                            row3 = _Forms.UniqueDataStructure.NewRow();

                            row3["Section"] = form.Section.ToString();

                            _Forms.UniqueDataStructure.Rows.Add(row3);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                    if (MoveToProdind == true)
                    {
                        MoveToProd();
                    }
                }

                DateTime it = DateTime.Now;


                Cursor = Cursors.WaitCursor;

            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            DataSet ReportDatasetWPDetail = new DataSet();
            ReportDatasetWPDetail.Tables.Add(dtNew);

            MWDataManager.clsDataAccess _dbManFileID = new MWDataManager.clsDataAccess();
            _dbManFileID.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManFileID.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

            _dbManFileID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFileID.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFileID.ResultsTableName = "FileID";
            _dbManFileID.ExecuteInstruction();

            fileID = _dbManFileID.ResultsDataTable.Rows[0][0].ToString();

            #region API PrintedFromID Return

            var theCurrentPrintedFromID = _PrintedForm.PrintedFromID[0].ToString();
            var theBarcode = "PFID:" + theCurrentPrintedFromID + " PN:1/1 FI:" + fileID;
            #endregion

            ////Demo
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan.SqlStatement = " select '" + theBarcode + "' Barcode ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "Barcode";
            _dbMan.ExecuteInstruction();

            DataSet ReportDataset = new DataSet();
            ReportDataset.Tables.Add(_dbMan.ResultsDataTable);



            MWDataManager.clsDataAccess _dbManInsert = new MWDataManager.clsDataAccess();
            _dbManInsert.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManInsert.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
            "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '1') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '2') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '3') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '4') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '5') \r\n";

            _dbManInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert.ResultsTableName = "FileID";
            _dbManInsert.ExecuteInstruction();


            theReport.RegisterData(ReportDatasetWPDetail);
            theReport.RegisterData(ReportDataset);

        
            theReport.Load(ReportFolder + "CloseIncidents.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
          
            theReport.Preview = pcReport;
          
            theReport.ShowPrepared();
        

            pcReport.Visible = true;
            pcReport.BringToFront();

            Cursor = DefaultCursor;
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            tab1.PageVisible = true;
            tab2.PageVisible = false;
            tab3.PageVisible = false;
            tab4.PageVisible = false;

            dt.Rows.Clear();
        }

        void Load5to10Incidents()
        {
            
                dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();

                List<DataRow> rows_to_remove = new List<DataRow>();
                foreach (DataRow row1 in dt.Rows)
                {
                    foreach (DataRow row2 in dtNew.Rows)
                    {
                        if (row1["IncidentNumber"].ToString() == row2["IncidentNumber"].ToString())
                        {
                            rows_to_remove.Add(row1);
                        }
                    }
                }

                DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
                MoveToProdind = true;
                try
                {
                    _listForms.Clear();
                    _listForms.Add(new ListForms
                    {
                        Section = "A114"
                    ,
                        WPID = ""
                    ,
                        WPName = ""
                    ,
                        FID = "150"
                    ,
                        UID = 0
                    });

                    string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                    var client = new Models.ClientConnect();
                    var param = new Dictionary<string, string>();
                    param.Add("FormID", "150");

                    var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                    _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                    _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                    OCRCL.SelectedValue = "150";
                    List<ListForms> _FormsL = new List<ListForms>();
                    _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                    _Forms.UniqueDataStructure.Rows[0].Delete();

                    var allResponses = new List<ListForms>();
                    for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                    {
                        _Forms.UniqueDataStructure.Rows.Clear();
                        foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                        {
                            try
                            {
                                DataRow row3;
                                row3 = _Forms.UniqueDataStructure.NewRow();

                                row3["Section"] = form.Section.ToString();

                                _Forms.UniqueDataStructure.Rows.Add(row3);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                        if (MoveToProdind == true)
                        {
                            MoveToProd();
                        }
                    }

                    DateTime it = DateTime.Now;



                    Cursor = Cursors.WaitCursor;

                }
                catch (Exception error)
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
                }
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

                DataSet ReportDatasetWPDetail = new DataSet();
                ReportDatasetWPDetail.Tables.Add(dtNew);

                MWDataManager.clsDataAccess _dbManFileID = new MWDataManager.clsDataAccess();
                _dbManFileID.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManFileID.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

                _dbManFileID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManFileID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManFileID.ResultsTableName = "FileID";
                _dbManFileID.ExecuteInstruction();

                fileID = _dbManFileID.ResultsDataTable.Rows[0][0].ToString();

                #region API PrintedFromID Return

                var theCurrentPrintedFromID = _PrintedForm.PrintedFromID[0].ToString();
                var theBarcode = "PFID:" + theCurrentPrintedFromID + " PN:1/1 FI:" + fileID;
                #endregion

                ////Demo
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbMan.SqlStatement = " select '" + theBarcode + "' Barcode ";

                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "Barcode";
                _dbMan.ExecuteInstruction();

                DataSet ReportDataset = new DataSet();
                ReportDataset.Tables.Add(_dbMan.ResultsDataTable);




                if (dt.Rows.Count < 5)
                {
                    DataRow workRow = dt.NewRow();
                    DataRow workRow2 = dt.NewRow();
                    DataRow workRow3 = dt.NewRow();
                    DataRow workRow4 = dt.NewRow();
                    DataRow workRow5 = dt.NewRow();
                    dt.Rows.Add(workRow);
                    dt.Rows.Add(workRow2);
                    dt.Rows.Add(workRow3);
                    dt.Rows.Add(workRow4);
                    dt.Rows.Add(workRow5);
                }



                MWDataManager.clsDataAccess _dbManInsert = new MWDataManager.clsDataAccess();
                _dbManInsert.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManInsert.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
                "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '1') \r\n" +

                "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
               "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '2') \r\n" +

                "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
               "values('" + dtNew.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '3') \r\n" +

                "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
               "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '4') \r\n" +

                "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
               "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '5') \r\n";

                _dbManInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManInsert.ResultsTableName = "FileID";
                _dbManInsert.ExecuteInstruction();

                foreach (DataRow row2 in rows_to_remove)
                {
                    dt.Rows.Remove(row2);
                    dt.AcceptChanges();
                }


                ////Print next 5
                ///
                //DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle2 = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
                MoveToProdind = true;
                try
                {
                    _listForms.Clear();
                    _listForms.Add(new ListForms
                    {
                        Section = "A114"
                    ,
                        WPID = ""
                    ,
                        WPName = ""
                    ,
                        FID = "150"
                    ,
                        UID = 0
                    });

                    string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                    var client = new Models.ClientConnect();
                    var param = new Dictionary<string, string>();
                    param.Add("FormID", "150");

                    var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                    _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                    _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                    OCRCL.SelectedValue = "150";
                    List<ListForms> _FormsL = new List<ListForms>();
                    _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                    _Forms.UniqueDataStructure.Rows[0].Delete();

                    var allResponses = new List<ListForms>();
                    for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                    {
                        _Forms.UniqueDataStructure.Rows.Clear();
                        foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                        {
                            try
                            {
                                DataRow row3;
                                row3 = _Forms.UniqueDataStructure.NewRow();

                                row3["Section"] = form.Section.ToString();

                                _Forms.UniqueDataStructure.Rows.Add(row3);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                        if (MoveToProdind == true)
                        {
                            MoveToProd();
                        }
                    }

                    DateTime it = DateTime.Now;



                    Cursor = Cursors.WaitCursor;

                }
                catch (Exception error)
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
                }
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

                if (dt.Rows.Count < 5)
                {
                    DataRow workRow = dt.NewRow();
                    DataRow workRow2 = dt.NewRow();
                    DataRow workRow3 = dt.NewRow();
                    DataRow workRow4 = dt.NewRow();
                    DataRow workRow5 = dt.NewRow();
                    dt.Rows.Add(workRow);
                    dt.Rows.Add(workRow2);
                    dt.Rows.Add(workRow3);
                    dt.Rows.Add(workRow4);
                    dt.Rows.Add(workRow5);
                }

                dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
                DataSet ReportDatasetWPDetail2 = new DataSet();
                ReportDatasetWPDetail2.Tables.Add(dtNew);

                MWDataManager.clsDataAccess _dbManFileID2 = new MWDataManager.clsDataAccess();
                _dbManFileID2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManFileID2.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

                _dbManFileID2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManFileID2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManFileID2.ResultsTableName = "FileID";
                _dbManFileID2.ExecuteInstruction();

                fileID = _dbManFileID2.ResultsDataTable.Rows[0][0].ToString();

                #region API PrintedFromID Return

                var theCurrentPrintedFromID2 = _PrintedForm.PrintedFromID[0].ToString();
                var theBarcode2 = "PFID:" + theCurrentPrintedFromID2 + " PN:1/1 FI:" + fileID;
                #endregion

                ////Demo
                MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
                _dbMan2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbMan2.SqlStatement = " select '" + theBarcode2 + "' Barcode ";

                _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan2.ResultsTableName = "Barcode";
                _dbMan2.ExecuteInstruction();

                DataSet ReportDataset2 = new DataSet();
                ReportDataset2.Tables.Add(_dbMan2.ResultsDataTable);


                MWDataManager.clsDataAccess _dbManInsert2 = new MWDataManager.clsDataAccess();
                _dbManInsert2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManInsert2.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
                "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '1') \r\n" +

                "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
               "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '2') \r\n" +

                "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
               "values('" + dtNew.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '3') \r\n" +

                "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
               "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '4') \r\n" +

                "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
               "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '5') \r\n";

                _dbManInsert2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManInsert2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManInsert2.ResultsTableName = "FileID";
                _dbManInsert2.ExecuteInstruction();


                theReport.RegisterData(ReportDatasetWPDetail);
                theReport.RegisterData(ReportDataset);

                theReport2.RegisterData(ReportDatasetWPDetail2);
                theReport2.RegisterData(ReportDataset2);


                theReport.Load(ReportFolder + "CloseIncidents.frx");
                theReport2.Load(ReportFolder + "CloseIncidents.frx");
                // theReport.Design();

                pcReport.Clear();
                theReport.Prepare();
                theReport2.Prepare();
                theReport.Preview = pcReport;
                theReport2.Preview = pcReport2;
                theReport.ShowPrepared();
                theReport2.ShowPrepared();

                pcReport.Visible = true;
                pcReport.BringToFront();

                Cursor = DefaultCursor;
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
                tab1.PageVisible = true;
                tab2.PageVisible = true;
                tab3.PageVisible = false;
                tab4.PageVisible = false;
                dt.Rows.Clear();

        }
        void Load10to15Incidents()
        {
            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();

            List<DataRow> rows_to_remove = new List<DataRow>();
            foreach (DataRow row1 in dt.Rows)
            {
                foreach (DataRow row2 in dtNew.Rows)
                {
                    if (row1["IncidentNumber"].ToString() == row2["IncidentNumber"].ToString())
                    {
                        rows_to_remove.Add(row1);
                    }
                }
            }

            DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            MoveToProdind = true;
            try
            {
                _listForms.Clear();
                _listForms.Add(new ListForms
                {
                    Section = "A114"
                ,
                    WPID = ""
                ,
                    WPName = ""
                ,
                    FID = "150"
                ,
                    UID = 0
                });

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", "150");

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                OCRCL.SelectedValue = "150";
                List<ListForms> _FormsL = new List<ListForms>();
                _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                _Forms.UniqueDataStructure.Rows[0].Delete();

                var allResponses = new List<ListForms>();
                for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                {
                    _Forms.UniqueDataStructure.Rows.Clear();
                    foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                    {
                        try
                        {
                            DataRow row3;
                            row3 = _Forms.UniqueDataStructure.NewRow();

                            row3["Section"] = form.Section.ToString();

                            _Forms.UniqueDataStructure.Rows.Add(row3);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                    if (MoveToProdind == true)
                    {
                        MoveToProd();
                    }
                }

                DateTime it = DateTime.Now;



                Cursor = Cursors.WaitCursor;

            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            DataSet ReportDatasetWPDetail = new DataSet();
            ReportDatasetWPDetail.Tables.Add(dtNew);

            MWDataManager.clsDataAccess _dbManFileID = new MWDataManager.clsDataAccess();
            _dbManFileID.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManFileID.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

            _dbManFileID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFileID.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFileID.ResultsTableName = "FileID";
            _dbManFileID.ExecuteInstruction();

            fileID = _dbManFileID.ResultsDataTable.Rows[0][0].ToString();

            #region API PrintedFromID Return

            var theCurrentPrintedFromID = _PrintedForm.PrintedFromID[0].ToString();
            var theBarcode = "PFID:" + theCurrentPrintedFromID + " PN:1/1 FI:" + fileID;
            #endregion

            ////Demo
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan.SqlStatement = " select '" + theBarcode + "' Barcode ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "Barcode";
            _dbMan.ExecuteInstruction();

            DataSet ReportDataset = new DataSet();
            ReportDataset.Tables.Add(_dbMan.ResultsDataTable);




            if (dt.Rows.Count < 5)
            {
                DataRow workRow = dt.NewRow();
                DataRow workRow2 = dt.NewRow();
                DataRow workRow3 = dt.NewRow();
                DataRow workRow4 = dt.NewRow();
                DataRow workRow5 = dt.NewRow();
                dt.Rows.Add(workRow);
                dt.Rows.Add(workRow2);
                dt.Rows.Add(workRow3);
                dt.Rows.Add(workRow4);
                dt.Rows.Add(workRow5);
            }



            MWDataManager.clsDataAccess _dbManInsert = new MWDataManager.clsDataAccess();
            _dbManInsert.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManInsert.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
            "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '1') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '2') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '3') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '4') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '5') \r\n";

            _dbManInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert.ResultsTableName = "FileID";
            _dbManInsert.ExecuteInstruction();

            foreach (DataRow row2 in rows_to_remove)
            {
                dt.Rows.Remove(row2);
                dt.AcceptChanges();
            }


            ////Print report 2
            ///


            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();

            List<DataRow> rows_to_remove2 = new List<DataRow>();
            foreach (DataRow row1 in dt.Rows)
            {
                foreach (DataRow row2 in dtNew.Rows)
                {
                    if (row1["IncidentNumber"].ToString() == row2["IncidentNumber"].ToString())
                    {
                        rows_to_remove2.Add(row1);
                    }
                }
            }

            MoveToProdind = true;
            try
            {
                _listForms.Clear();
                _listForms.Add(new ListForms
                {
                    Section = "A114"
                ,
                    WPID = ""
                ,
                    WPName = ""
                ,
                    FID = "150"
                ,
                    UID = 0
                });

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", "150");

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                OCRCL.SelectedValue = "150";
                List<ListForms> _FormsL = new List<ListForms>();
                _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                _Forms.UniqueDataStructure.Rows[0].Delete();

                var allResponses = new List<ListForms>();
                for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                {
                    _Forms.UniqueDataStructure.Rows.Clear();
                    foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                    {
                        try
                        {
                            DataRow row3;
                            row3 = _Forms.UniqueDataStructure.NewRow();

                            row3["Section"] = form.Section.ToString();

                            _Forms.UniqueDataStructure.Rows.Add(row3);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                    if (MoveToProdind == true)
                    {
                        MoveToProd();
                    }
                }

                DateTime it = DateTime.Now;



                Cursor = Cursors.WaitCursor;

            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            if (dt.Rows.Count < 5)
            {
                DataRow workRow = dt.NewRow();
                DataRow workRow2 = dt.NewRow();
                DataRow workRow3 = dt.NewRow();
                DataRow workRow4 = dt.NewRow();
                DataRow workRow5 = dt.NewRow();
                dt.Rows.Add(workRow);
                dt.Rows.Add(workRow2);
                dt.Rows.Add(workRow3);
                dt.Rows.Add(workRow4);
                dt.Rows.Add(workRow5);
            }

            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
            DataSet ReportDatasetWPDetail2 = new DataSet();
            ReportDatasetWPDetail2.Tables.Add(dtNew);

            MWDataManager.clsDataAccess _dbManFileID2 = new MWDataManager.clsDataAccess();
            _dbManFileID2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManFileID2.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

            _dbManFileID2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFileID2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFileID2.ResultsTableName = "FileID";
            _dbManFileID2.ExecuteInstruction();

            fileID = _dbManFileID2.ResultsDataTable.Rows[0][0].ToString();

            #region API PrintedFromID Return

            var theCurrentPrintedFromID2 = _PrintedForm.PrintedFromID[0].ToString();
            var theBarcode2 = "PFID:" + theCurrentPrintedFromID2 + " PN:1/1 FI:" + fileID;
            #endregion

            ////Demo
            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan2.SqlStatement = " select '" + theBarcode2 + "' Barcode ";

            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ResultsTableName = "Barcode";
            _dbMan2.ExecuteInstruction();

            DataSet ReportDataset2 = new DataSet();
            ReportDataset2.Tables.Add(_dbMan2.ResultsDataTable);


            MWDataManager.clsDataAccess _dbManInsert2 = new MWDataManager.clsDataAccess();
            _dbManInsert2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManInsert2.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
            "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '1') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '2') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '3') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '4') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '5') \r\n";

            _dbManInsert2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert2.ResultsTableName = "FileID";
            _dbManInsert2.ExecuteInstruction();


            foreach (DataRow row3 in rows_to_remove2)
            {
                dt.Rows.Remove(row3);
                dt.AcceptChanges();
            }

            ////Print Report 3
            ///
            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();

            List<DataRow> rows_to_remove3 = new List<DataRow>();
            foreach (DataRow row1 in dt.Rows)
            {
                foreach (DataRow row2 in dtNew.Rows)
                {
                    if (row1["IncidentNumber"].ToString() == row2["IncidentNumber"].ToString())
                    {
                        rows_to_remove3.Add(row1);
                    }
                }
            }

            MoveToProdind = true;
            try
            {
                _listForms.Clear();
                _listForms.Add(new ListForms
                {
                    Section = "A114"
                ,
                    WPID = ""
                ,
                    WPName = ""
                ,
                    FID = "150"
                ,
                    UID = 0
                });

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", "150");

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                OCRCL.SelectedValue = "150";
                List<ListForms> _FormsL = new List<ListForms>();
                _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                _Forms.UniqueDataStructure.Rows[0].Delete();

                var allResponses = new List<ListForms>();
                for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                {
                    _Forms.UniqueDataStructure.Rows.Clear();
                    foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                    {
                        try
                        {
                            DataRow row4;
                            row4 = _Forms.UniqueDataStructure.NewRow();

                            row4["Section"] = form.Section.ToString();

                            _Forms.UniqueDataStructure.Rows.Add(row4);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                    if (MoveToProdind == true)
                    {
                        MoveToProd();
                    }
                }

                DateTime it = DateTime.Now;



                Cursor = Cursors.WaitCursor;

            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            if (dt.Rows.Count < 5)
            {
                DataRow workRow = dt.NewRow();
                DataRow workRow2 = dt.NewRow();
                DataRow workRow3 = dt.NewRow();
                DataRow workRow4 = dt.NewRow();
                DataRow workRow5 = dt.NewRow();
                dt.Rows.Add(workRow);
                dt.Rows.Add(workRow2);
                dt.Rows.Add(workRow3);
                dt.Rows.Add(workRow4);
                dt.Rows.Add(workRow5);
            }

            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
            DataSet ReportDatasetWPDetail3 = new DataSet();
            ReportDatasetWPDetail3.Tables.Add(dtNew);

            MWDataManager.clsDataAccess _dbManFileID3 = new MWDataManager.clsDataAccess();
            _dbManFileID3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManFileID3.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

            _dbManFileID3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFileID3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFileID3.ResultsTableName = "FileID";
            _dbManFileID3.ExecuteInstruction();

            fileID = _dbManFileID3.ResultsDataTable.Rows[0][0].ToString();

            #region API PrintedFromID Return

            var theCurrentPrintedFromID3 = _PrintedForm.PrintedFromID[0].ToString();
            var theBarcode3 = "PFID:" + theCurrentPrintedFromID3 + " PN:1/1 FI:" + fileID;
            #endregion

            ////Demo
            MWDataManager.clsDataAccess _dbMan3 = new MWDataManager.clsDataAccess();
            _dbMan3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan3.SqlStatement = " select '" + theBarcode3 + "' Barcode ";

            _dbMan3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3.ResultsTableName = "Barcode";
            _dbMan3.ExecuteInstruction();

            DataSet ReportDataset3 = new DataSet();
            ReportDataset3.Tables.Add(_dbMan3.ResultsDataTable);


            MWDataManager.clsDataAccess _dbManInsert3 = new MWDataManager.clsDataAccess();
            _dbManInsert3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManInsert3.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
            "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '1') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '2') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '3') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '4') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '5') \r\n";

            _dbManInsert3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert3.ResultsTableName = "FileID";
            _dbManInsert3.ExecuteInstruction();

            ///


            theReport.RegisterData(ReportDatasetWPDetail);
            theReport.RegisterData(ReportDataset);

            theReport2.RegisterData(ReportDatasetWPDetail2);
            theReport2.RegisterData(ReportDataset2);

            theReport3.RegisterData(ReportDatasetWPDetail3);
            theReport3.RegisterData(ReportDataset3);


            theReport.Load(ReportFolder + "CloseIncidents.frx");
            theReport2.Load(ReportFolder + "CloseIncidents.frx");
            theReport3.Load(ReportFolder + "CloseIncidents.frx");
            // theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport2.Prepare();
            theReport3.Prepare();
            theReport.Preview = pcReport;
            theReport2.Preview = pcReport2;
            theReport3.Preview = pcReport3;
            theReport.ShowPrepared();
            theReport2.ShowPrepared();
            theReport3.ShowPrepared();

            pcReport.Visible = true;
            pcReport.BringToFront();

            Cursor = DefaultCursor;
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            tab1.PageVisible = true;
            tab2.PageVisible = true;
            tab3.PageVisible = true;
            tab4.PageVisible = false;
            dt.Rows.Clear();
        }
        void Load15to20Incidents()
        {
            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();

            List<DataRow> rows_to_remove = new List<DataRow>();
            foreach (DataRow row1 in dt.Rows)
            {
                foreach (DataRow row2 in dtNew.Rows)
                {
                    if (row1["IncidentNumber"].ToString() == row2["IncidentNumber"].ToString())
                    {
                        rows_to_remove.Add(row1);
                    }
                }
            }

            DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            MoveToProdind = true;
            try
            {
                _listForms.Clear();
                _listForms.Add(new ListForms
                {
                    Section = "A114"
                ,
                    WPID = ""
                ,
                    WPName = ""
                ,
                    FID = "150"
                ,
                    UID = 0
                });

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", "150");

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                OCRCL.SelectedValue = "150";
                List<ListForms> _FormsL = new List<ListForms>();
                _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                _Forms.UniqueDataStructure.Rows[0].Delete();

                var allResponses = new List<ListForms>();
                for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                {
                    _Forms.UniqueDataStructure.Rows.Clear();
                    foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                    {
                        try
                        {
                            DataRow row3;
                            row3 = _Forms.UniqueDataStructure.NewRow();

                            row3["Section"] = form.Section.ToString();

                            _Forms.UniqueDataStructure.Rows.Add(row3);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                    if (MoveToProdind == true)
                    {
                        MoveToProd();
                    }
                }

                DateTime it = DateTime.Now;



                Cursor = Cursors.WaitCursor;

            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            DataSet ReportDatasetWPDetail = new DataSet();
            ReportDatasetWPDetail.Tables.Add(dtNew);

            MWDataManager.clsDataAccess _dbManFileID = new MWDataManager.clsDataAccess();
            _dbManFileID.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManFileID.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

            _dbManFileID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFileID.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFileID.ResultsTableName = "FileID";
            _dbManFileID.ExecuteInstruction();

            fileID = _dbManFileID.ResultsDataTable.Rows[0][0].ToString();

            #region API PrintedFromID Return

            var theCurrentPrintedFromID = _PrintedForm.PrintedFromID[0].ToString();
            var theBarcode = "PFID:" + theCurrentPrintedFromID + " PN:1/1 FI:" + fileID;
            #endregion

            ////Demo
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan.SqlStatement = " select '" + theBarcode + "' Barcode ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "Barcode";
            _dbMan.ExecuteInstruction();

            DataSet ReportDataset = new DataSet();
            ReportDataset.Tables.Add(_dbMan.ResultsDataTable);




            if (dt.Rows.Count < 5)
            {
                DataRow workRow = dt.NewRow();
                DataRow workRow2 = dt.NewRow();
                DataRow workRow3 = dt.NewRow();
                DataRow workRow4 = dt.NewRow();
                DataRow workRow5 = dt.NewRow();
                dt.Rows.Add(workRow);
                dt.Rows.Add(workRow2);
                dt.Rows.Add(workRow3);
                dt.Rows.Add(workRow4);
                dt.Rows.Add(workRow5);
            }



            MWDataManager.clsDataAccess _dbManInsert = new MWDataManager.clsDataAccess();
            _dbManInsert.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManInsert.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
            "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '1') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '2') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '3') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '4') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '5') \r\n";

            _dbManInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert.ResultsTableName = "FileID";
            _dbManInsert.ExecuteInstruction();

            foreach (DataRow row2 in rows_to_remove)
            {
                dt.Rows.Remove(row2);
                dt.AcceptChanges();
            }


            ////Print report 2
            ///

            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
            List<DataRow> rows_to_remove2 = new List<DataRow>();
            foreach (DataRow row1 in dt.Rows)
            {
                foreach (DataRow row2 in dtNew.Rows)
                {
                    if (row1["IncidentNumber"].ToString() == row2["IncidentNumber"].ToString())
                    {
                        rows_to_remove2.Add(row1);
                    }
                }
            }

            MoveToProdind = true;
            try
            {
                _listForms.Clear();
                _listForms.Add(new ListForms
                {
                    Section = "A114"
                ,
                    WPID = ""
                ,
                    WPName = ""
                ,
                    FID = "150"
                ,
                    UID = 0
                });

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", "150");

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                OCRCL.SelectedValue = "150";
                List<ListForms> _FormsL = new List<ListForms>();
                _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                _Forms.UniqueDataStructure.Rows[0].Delete();

                var allResponses = new List<ListForms>();
                for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                {
                    _Forms.UniqueDataStructure.Rows.Clear();
                    foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                    {
                        try
                        {
                            DataRow row3;
                            row3 = _Forms.UniqueDataStructure.NewRow();

                            row3["Section"] = form.Section.ToString();

                            _Forms.UniqueDataStructure.Rows.Add(row3);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                    if (MoveToProdind == true)
                    {
                        MoveToProd();
                    }
                }

                DateTime it = DateTime.Now;



                Cursor = Cursors.WaitCursor;

            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            if (dt.Rows.Count < 5)
            {
                DataRow workRow = dt.NewRow();
                DataRow workRow2 = dt.NewRow();
                DataRow workRow3 = dt.NewRow();
                DataRow workRow4 = dt.NewRow();
                DataRow workRow5 = dt.NewRow();
                dt.Rows.Add(workRow);
                dt.Rows.Add(workRow2);
                dt.Rows.Add(workRow3);
                dt.Rows.Add(workRow4);
                dt.Rows.Add(workRow5);
            }

            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
            DataSet ReportDatasetWPDetail2 = new DataSet();
            ReportDatasetWPDetail2.Tables.Add(dtNew);

            MWDataManager.clsDataAccess _dbManFileID2 = new MWDataManager.clsDataAccess();
            _dbManFileID2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManFileID2.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

            _dbManFileID2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFileID2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFileID2.ResultsTableName = "FileID";
            _dbManFileID2.ExecuteInstruction();

            fileID = _dbManFileID2.ResultsDataTable.Rows[0][0].ToString();

            #region API PrintedFromID Return

            var theCurrentPrintedFromID2 = _PrintedForm.PrintedFromID[0].ToString();
            var theBarcode2 = "PFID:" + theCurrentPrintedFromID2 + " PN:1/1 FI:" + fileID;
            #endregion

            ////Demo
            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan2.SqlStatement = " select '" + theBarcode2 + "' Barcode ";

            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ResultsTableName = "Barcode";
            _dbMan2.ExecuteInstruction();

            DataSet ReportDataset2 = new DataSet();
            ReportDataset2.Tables.Add(_dbMan2.ResultsDataTable);


            MWDataManager.clsDataAccess _dbManInsert2 = new MWDataManager.clsDataAccess();
            _dbManInsert2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManInsert2.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
            "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '1') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '2') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '3') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '4') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID2 + "', '" + System.DateTime.Today + "', '5') \r\n";

            _dbManInsert2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert2.ResultsTableName = "FileID";
            _dbManInsert2.ExecuteInstruction();

            foreach (DataRow row3 in rows_to_remove2)
            {
                dt.Rows.Remove(row3);
                dt.AcceptChanges();
            }

            ////Print Report 3
            ///
            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
            List<DataRow> rows_to_remove3 = new List<DataRow>();
            foreach (DataRow row1 in dt.Rows)
            {
                foreach (DataRow row2 in dtNew.Rows)
                {
                    if (row1["IncidentNumber"].ToString() == row2["IncidentNumber"].ToString())
                    {
                        rows_to_remove3.Add(row1);
                    }
                }
            }

            MoveToProdind = true;
            try
            {
                _listForms.Clear();
                _listForms.Add(new ListForms
                {
                    Section = "A114"
                ,
                    WPID = ""
                ,
                    WPName = ""
                ,
                    FID = "150"
                ,
                    UID = 0
                });

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", "150");

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                OCRCL.SelectedValue = "150";
                List<ListForms> _FormsL = new List<ListForms>();
                _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                _Forms.UniqueDataStructure.Rows[0].Delete();

                var allResponses = new List<ListForms>();
                for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                {
                    _Forms.UniqueDataStructure.Rows.Clear();
                    foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                    {
                        try
                        {
                            DataRow row4;
                            row4 = _Forms.UniqueDataStructure.NewRow();

                            row4["Section"] = form.Section.ToString();

                            _Forms.UniqueDataStructure.Rows.Add(row4);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                    if (MoveToProdind == true)
                    {
                        MoveToProd();
                    }
                }

                DateTime it = DateTime.Now;



                Cursor = Cursors.WaitCursor;

            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            if (dt.Rows.Count < 5)
            {
                DataRow workRow = dt.NewRow();
                DataRow workRow2 = dt.NewRow();
                DataRow workRow3 = dt.NewRow();
                DataRow workRow4 = dt.NewRow();
                DataRow workRow5 = dt.NewRow();
                dt.Rows.Add(workRow);
                dt.Rows.Add(workRow2);
                dt.Rows.Add(workRow3);
                dt.Rows.Add(workRow4);
                dt.Rows.Add(workRow5);
            }

            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
            DataSet ReportDatasetWPDetail3 = new DataSet();
            ReportDatasetWPDetail3.Tables.Add(dtNew);

            MWDataManager.clsDataAccess _dbManFileID3 = new MWDataManager.clsDataAccess();
            _dbManFileID3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManFileID3.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

            _dbManFileID3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFileID3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFileID3.ResultsTableName = "FileID";
            _dbManFileID3.ExecuteInstruction();

            fileID = _dbManFileID3.ResultsDataTable.Rows[0][0].ToString();

            #region API PrintedFromID Return

            var theCurrentPrintedFromID3 = _PrintedForm.PrintedFromID[0].ToString();
            var theBarcode3 = "PFID:" + theCurrentPrintedFromID3 + " PN:1/1 FI:" + fileID;
            #endregion

            ////Demo
            MWDataManager.clsDataAccess _dbMan3 = new MWDataManager.clsDataAccess();
            _dbMan3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan3.SqlStatement = " select '" + theBarcode3 + "' Barcode ";

            _dbMan3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3.ResultsTableName = "Barcode";
            _dbMan3.ExecuteInstruction();

            DataSet ReportDataset3 = new DataSet();
            ReportDataset3.Tables.Add(_dbMan3.ResultsDataTable);


            MWDataManager.clsDataAccess _dbManInsert3 = new MWDataManager.clsDataAccess();
            _dbManInsert3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManInsert3.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
            "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '1') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '2') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '3') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '4') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID3 + "', '" + System.DateTime.Today + "', '5') \r\n";

            _dbManInsert3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert3.ResultsTableName = "FileID";
            _dbManInsert3.ExecuteInstruction();


            foreach (DataRow row4 in rows_to_remove3)
            {
                dt.Rows.Remove(row4);
                dt.AcceptChanges();
            }

            ////Print Report 4
            ///
            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
            List<DataRow> rows_to_remove4 = new List<DataRow>();
            foreach (DataRow row1 in dt.Rows)
            {
                foreach (DataRow row2 in dtNew.Rows)
                {
                    if (row1["IncidentNumber"].ToString() == row2["IncidentNumber"].ToString())
                    {
                        rows_to_remove4.Add(row1);
                    }
                }
            }
            MoveToProdind = true;
            try
            {
                _listForms.Clear();
                _listForms.Add(new ListForms
                {
                    Section = "A114"
                ,
                    WPID = ""
                ,
                    WPName = ""
                ,
                    FID = "150"
                ,
                    UID = 0
                });

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", "150");

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                OCRCL.SelectedValue = "150";
                List<ListForms> _FormsL = new List<ListForms>();
                _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                _Forms.UniqueDataStructure.Rows[0].Delete();

                var allResponses = new List<ListForms>();
                for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                {
                    _Forms.UniqueDataStructure.Rows.Clear();
                    foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                    {
                        try
                        {
                            DataRow row5;
                            row5 = _Forms.UniqueDataStructure.NewRow();

                            row5["Section"] = form.Section.ToString();

                            _Forms.UniqueDataStructure.Rows.Add(row5);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                    if (MoveToProdind == true)
                    {
                        MoveToProd();
                    }
                }

                DateTime it = DateTime.Now;



                Cursor = Cursors.WaitCursor;

            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            if (dt.Rows.Count < 5)
            {
                DataRow workRow = dt.NewRow();
                DataRow workRow2 = dt.NewRow();
                DataRow workRow3 = dt.NewRow();
                DataRow workRow4 = dt.NewRow();
                DataRow workRow5 = dt.NewRow();
                dt.Rows.Add(workRow);
                dt.Rows.Add(workRow2);
                dt.Rows.Add(workRow3);
                dt.Rows.Add(workRow4);
                dt.Rows.Add(workRow5);
            }

            dtNew = dt.Rows.Cast<System.Data.DataRow>().Take(5).CopyToDataTable();
            DataSet ReportDatasetWPDetail4 = new DataSet();
            ReportDatasetWPDetail4.Tables.Add(dtNew);

            MWDataManager.clsDataAccess _dbManFileID4 = new MWDataManager.clsDataAccess();
            _dbManFileID4.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManFileID4.SqlStatement = " select filename from minewareocr_amp.dbo.tblforms where formsid = '150'  ";

            _dbManFileID4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFileID4.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFileID4.ResultsTableName = "FileID";
            _dbManFileID4.ExecuteInstruction();

            fileID = _dbManFileID4.ResultsDataTable.Rows[0][0].ToString();

            #region API PrintedFromID Return

            var theCurrentPrintedFromID4 = _PrintedForm.PrintedFromID[0].ToString();
            var theBarcode4 = "PFID:" + theCurrentPrintedFromID4 + " PN:1/1 FI:" + fileID;
            #endregion

            ////Demo
            MWDataManager.clsDataAccess _dbMan4 = new MWDataManager.clsDataAccess();
            _dbMan4.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbMan4.SqlStatement = " select '" + theBarcode4 + "' Barcode ";

            _dbMan4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan4.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan4.ResultsTableName = "Barcode";
            _dbMan4.ExecuteInstruction();

            DataSet ReportDataset4 = new DataSet();
            ReportDataset4.Tables.Add(_dbMan4.ResultsDataTable);


            MWDataManager.clsDataAccess _dbManInsert4 = new MWDataManager.clsDataAccess();
            _dbManInsert4.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManInsert4.SqlStatement = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
            "values('" + dtNew.Rows[0]["IncidentNumber"] + "', '" + theCurrentPrintedFromID4 + "', '" + System.DateTime.Today + "', '1') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[1]["IncidentNumber"] + "', '" + theCurrentPrintedFromID4 + "', '" + System.DateTime.Today + "', '2') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dt.Rows[2]["IncidentNumber"] + "', '" + theCurrentPrintedFromID4 + "', '" + System.DateTime.Today + "', '3') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[3]["IncidentNumber"] + "', '" + theCurrentPrintedFromID + "', '" + System.DateTime.Today + "', '4') \r\n" +

            "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
           "values('" + dtNew.Rows[4]["IncidentNumber"] + "', '" + theCurrentPrintedFromID4 + "', '" + System.DateTime.Today + "', '5') \r\n";

            _dbManInsert4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert4.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert4.ResultsTableName = "FileID";
            _dbManInsert4.ExecuteInstruction();

            ///


            theReport.RegisterData(ReportDatasetWPDetail);
            theReport.RegisterData(ReportDataset);

            theReport2.RegisterData(ReportDatasetWPDetail2);
            theReport2.RegisterData(ReportDataset2);

            theReport3.RegisterData(ReportDatasetWPDetail3);
            theReport3.RegisterData(ReportDataset3);

            theReport4.RegisterData(ReportDatasetWPDetail4);
            theReport4.RegisterData(ReportDataset4);

            theReport.Load(ReportFolder + "CloseIncidents.frx");
            theReport2.Load(ReportFolder + "CloseIncidents.frx");
            theReport3.Load(ReportFolder + "CloseIncidents.frx");
            theReport4.Load(ReportFolder + "CloseIncidents.frx");
            // theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport2.Prepare();
            theReport3.Prepare();
            theReport4.Prepare();

            theReport.Preview = pcReport;
            theReport2.Preview = pcReport2;
            theReport3.Preview = pcReport3;
            theReport4.Preview = pcReport4;

            theReport.ShowPrepared();
            theReport2.ShowPrepared();
            theReport3.ShowPrepared();
            theReport4.ShowPrepared();

            pcReport.Visible = true;
            pcReport.BringToFront();

            Cursor = DefaultCursor;
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            tab1.PageVisible = true;
            tab2.PageVisible = true;
            tab3.PageVisible = true;
            tab4.PageVisible = true;
            dt.Rows.Clear();
        }

        private void GetReport(string FormsID)
        {
            string GetReportRL = string.Format(@"/api/Report/GetReport/");
            var client = new ClientConnect();
            var param = new Dictionary<string, string>();
            var header = new Dictionary<string, string>();
            param.Add("FormsID", FormsID);
            _Forms.UniqueDataStructure.AcceptChanges();
            DataSet TheData = new DataSet();
            TheData.Tables.Clear();
            if (_Forms.UniqueDataStructure.DataSet != null)
            {
                DataSet dss = _Forms.UniqueDataStructure.DataSet;
                dss.Tables.Remove(_Forms.UniqueDataStructure);
            }

            TheData.Tables.Add(_Forms.UniqueDataStructure);
            string JSOResult;
            JSOResult = JsonConvert.SerializeObject(TheData, Formatting.Indented);
            try
            {
                var response = Task.Run(() => client.PostWithBodyAndParameters(GetReportRL, param, JSOResult)).Result;
                _PrintedForm = JsonConvert.DeserializeObject<PrintedForm>(response);
                string txtPDF = _PrintedForm.PDFLocation;

                if (File.Exists(@_PrintedForm.PDFLocation))
                {
                    if (MoveToProdind)
                    {

                        DateTime i = DateTime.Now;
                        string newFolder = "Mineware_OCR";
                        string newFolderDay = newFolder + @"\" + i.ToString("yyyy'_'MM'_'dd") + "_BulkPrint";

                        string path = System.IO.Path.Combine(
                           Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                           newFolder
                        );

                        if (!System.IO.Directory.Exists(path))
                        {
                            try
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }
                            catch (IOException ie)
                            {
                                Console.WriteLine("IO Error: " + ie.Message);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("General Error: " + e.Message);
                            }
                        }

                        string pathDay = System.IO.Path.Combine(
                          Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                          newFolderDay
                       );

                        if (!System.IO.Directory.Exists(pathDay))
                        {
                            try
                            {
                                System.IO.Directory.CreateDirectory(pathDay);
                            }
                            catch (IOException ie)
                            {
                                Console.WriteLine("IO Error: " + ie.Message);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("General Error: " + e.Message);
                            }
                        }

                        string sourceFile = System.IO.Path.Combine(Path.GetDirectoryName(@_PrintedForm.PDFLocation), Path.GetFileName(@_PrintedForm.PDFLocation));
                        string destFile = System.IO.Path.Combine(pathDay, Path.GetFileName(@_PrintedForm.PDFLocation));
                        System.IO.File.Copy(sourceFile, destFile, true);

                    }
                    else
                    {
                        PdfViewer i = new PdfViewer();
                        DevExpress.XtraEditors.XtraForm jj = new DevExpress.XtraEditors.XtraForm();
                        i.LoadDocument(@_PrintedForm.PDFLocation);
                        i.Dock = DockStyle.Fill;
                        i.ZoomMode = PdfZoomMode.FitToWidth;
                        i.NavigationPanePageVisibility = PdfNavigationPanePageVisibility.None;
                        jj.Controls.Add(i);
                        jj.Width = 600;
                        jj.Height = 800;

                        jj.ShowIcon = false;
                        jj.Text = "CHECKLIST EXAMPLE - CANNOT BE PRINTED";
                        jj.ShowDialog();
                    }

                }

            }
            catch (Exception error)
            {

            }
        }

        private void MoveToProd()
        {

            string GetFormInfoURL = string.Format(@"/api/Report/PrintReport/");
            foreach (string s in _PrintedForm.PrintedFromID)
            {

                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("PrintedFromID", s);
                param.Add("PrintedByName", TUserInfo.UserID);

                var response = Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;
            }
        }

        public class ListDrop
        {
            public string TypeCode { get; set; }
            public string ComplName { get; set; }
        }

        public class ListForms
        {
            public string Section { get; set; }
            public string WPID { get; set; }
            public string WPName { get; set; }
            public string FID { get; set; }
            public int UID { get; set; }
        }

        private void btnPrintChecklist_Click(object sender, EventArgs e)
        {
            if(tab1.PageVisible == true)
            {
                pcReport.Print();
            }
            if (tab2.PageVisible == true)
            {
                pcReport2.Print();
            }
            if (tab3.PageVisible == true)
            {
                pcReport3.Print();
            }
            if (tab4.PageVisible == true)
            {
                pcReport4.Print();
            }
        }

        private void stackPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}