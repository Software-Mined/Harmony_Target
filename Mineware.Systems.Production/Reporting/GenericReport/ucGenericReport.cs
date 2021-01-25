using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.Global.sysMessages;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.GlobalExtensions;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting.GenericReport
{
    public partial class ucGenericReport : BaseUserControl
    {
        private Report _theReport = new Report();
        private sysMessagesClass _sysMessagesClass = new sysMessagesClass();
        private clsGenericReportData _clsGenericReportData = new clsGenericReportData();
        private clsGenericReportSettings reportSettings = new clsGenericReportSettings();
        public DataTable GenericReportData;
        private DataTable dtExistReports;
        private DataTable dtExistReportOptions;
        Procedures procs = new Procedures();
        private Thread theReportThread;
        //private string theSystemDBTag = "AMPLATS";
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        private string _theConnection;
        Object theReportSettings;

        public string theConnection { get { return _theConnection; } set { _theConnection = value; } }
        public string theHierID;
        public string theName;
        private string RunType;
        public string PMonth;
        public string FPMonth;
        public string TPMonth;
        public string CDate;
        public string FDate;
        public string TDate;
        private bool ErrFound;
        private bool FoundNoSections;
        private string getDay;
        private string getMonth;
        private string Banner;
        private string SummaryOn;

        private string LevGM, LevMN, LevMNM, LevMO, LevSB, LevMiner, LevWP;
        private string PlanDyn, PlanLock, PlanDynProg, PlanLockProg, Book, Meas, PlanBuss, FC, AbsVar;
        private string DateType, StopeLedge, AuthPlanOnly;

        private string StpSqm, StpSqmOn, StpSqmOff, StpSqmOS, StpSqmOSF, StpCmgt, StpCmgtTotal, StpGT, StpGTTotal;
        private string StpSW, StpSWIdeal, StpSWFault, StpCW;
        private string StpKG, StpFL, StpFLOn, StpFLOff, StpFLOS, StpAdv, StpAdvOn, StpAdvOff;
        private string StpTons, StpTonsOn, StpTonsOff, StpTonsOS, StpTonsFault;
        private string StpCubics, StpCubGT, StpCubTons, StpCubKG, StpMeasSweeps, StpLabour, StpShftInfo;

        private string DevAdv, DevAdvOn, DevAdvOff, DevKG, DevEH, DevEW;

        private void cbByProdmonth_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void chkbxCheck_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void rpLevGM_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;

        }

        private void rpLevMN_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void rpLevMNM_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void rpLevMO_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void rpLevSB_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void rpLevMiner_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void rpLevWP_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void ceCopyLayout_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void rpPlanBuss_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void rpSumPerMonth_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = false;
        }

        private void pgSettingsMain_ShownEditor(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }

        private void cbByProdmonth_MouseHover(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }

        private void rpLevGM_MouseHover(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }

        private void rpLevMN_MouseHover(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }

        private void rpLevMNM_MouseHover(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }

        private void rpLevMO_MouseHover(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }

        private void rpLevSB_MouseHover(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }

        private void rpLevMiner_MouseHover(object sender, EventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }

        private void dockPanel1_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            prepareReport();
        }

        private string Level;

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //prepareReport();
            simpleButton1_Click(null, null);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            get_Parametres();
            theReportThread = new Thread(new ParameterizedThreadStart(createReport));
            theReportThread.SetApartmentState(ApartmentState.STA);
            theReportThread.Start(reportSettings);

            Report GenReport = new Report();
            DataSet repUserActivitySet = new DataSet();

            RunType = string.Empty;
            if (reportSettings.useProdmonth == true)
                RunType = "P";
            else
            {
                if (reportSettings.useMonthToMonth == true)
                    RunType = "M";
                else
                    RunType = "D";
            }


            string Levelaa = "[(" + reportSettings.Level + ")]".Replace(" ", "'");
            Levelaa = Levelaa.Replace("(", "('");
            Levelaa = Levelaa.Replace(")", "')");
            Levelaa = Levelaa.Replace(",", "' ,");
            Levelaa = Levelaa.Replace(" ", string.Empty);
            Levelaa = Levelaa.Replace(",", ",'");


            MWDataManager.clsDataAccess _dbManLoadEndtype1 = new MWDataManager.clsDataAccess();
            _dbManLoadEndtype1.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbManLoadEndtype1.SqlStatement = " delete from tbl_GenericRep_LevelsSelected ";

            _dbManLoadEndtype1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLoadEndtype1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManLoadEndtype1.ExecuteInstruction();




            string getValue = string.Empty;
            for (int i = 0; i < CheckComboLevel.Items.Count; i++)
            {
                if (CheckComboLevel.Items[i].CheckState == CheckState.Checked)
                {
                    getValue = CheckComboLevel.Items[i].Value.ToString();

                    MWDataManager.clsDataAccess _dbManLoadEndtype = new MWDataManager.clsDataAccess();
                    _dbManLoadEndtype.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _dbManLoadEndtype.SqlStatement = "insert into tbl_GenericRep_LevelsSelected values ( '" + getValue + "' ) ";

                    _dbManLoadEndtype.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManLoadEndtype.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManLoadEndtype.ExecuteInstruction();
                }
            }
            // Response.Write(getValue);


            //MWDataManager.clsDataAccess _dbManLoadEndtype = new MWDataManager.clsDataAccess();
            //_dbManLoadEndtype.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //_dbManLoadEndtype.SqlStatement = "insert into tbl_GenericRep_LevelsSelected values ( '" + Levelaa + "' ) ";

            //_dbManLoadEndtype.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbManLoadEndtype.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbManLoadEndtype.ExecuteInstruction();


            MWDataManager.clsDataAccess _GenericReportData = new MWDataManager.clsDataAccess();
            try
            {
                _GenericReportData.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                _GenericReportData.SqlStatement = "sp_GenericReport";
                _GenericReportData.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                _GenericReportData.ResultsTableName = "GenericReportData";

                SqlParameter[] _paramCollection =
                    {
                        _GenericReportData.CreateParameter("@NAME_5", SqlDbType.VarChar, 1, LevGM),   //reportSettings .LevMN ),
						_GenericReportData.CreateParameter("@NAME_4", SqlDbType.VarChar, 1, LevMNM),   //reportSettings .LevMNM ),
						_GenericReportData.CreateParameter("@NAME_3", SqlDbType.VarChar, 1, LevMN),   //reportSettings .LevMNM ),
						_GenericReportData.CreateParameter("@NAME_2", SqlDbType.VarChar, 1, LevMO),   //reportSettings .LevMO ),
						_GenericReportData.CreateParameter("@NAME_1", SqlDbType.VarChar, 1, LevSB),   //reportSettings .LevSB ),
						_GenericReportData.CreateParameter("@NAME", SqlDbType.VarChar, 1, LevMiner),   //reportSettings .LevMiner ),
						_GenericReportData.CreateParameter("@Workplace", SqlDbType.VarChar, 1, LevWP),   //reportSettings .LevWP ),

						_GenericReportData.CreateParameter("@RunLevel", SqlDbType.VarChar, 1,theHierID),
                        _GenericReportData.CreateParameter("@SectionID", SqlDbType.VarChar, 12, reportSettings.SectionID), //theName), //reportSettings .SectionID ),

						_GenericReportData.CreateParameter("@ProdMonth", SqlDbType.VarChar, 6, PMonth), //reportSettings .Prodmonth ),
						_GenericReportData.CreateParameter("@FromMonth", SqlDbType.VarChar, 6, FPMonth), //reportSettings .Prodmonth ),
						_GenericReportData.CreateParameter("@ToMonth", SqlDbType.VarChar, 6, TPMonth),//PMonth),  //reportSettings .Prodmonth ),     
						_GenericReportData.CreateParameter("@Calendardate", SqlDbType.VarChar, 10, (String.Format("{0:yyyy-MM-dd}", reportSettings.CalendarDate))), //String.Format("{0:yyyy-MM-dd}", reportSettings .DateTo )),                   
						_GenericReportData.CreateParameter("@FromDate", SqlDbType.VarChar, 10, (String.Format("{0:yyyy-MM-dd}",reportSettings.DateFrom))), //String.Format("{0:yyyy-MM-dd}", reportSettings .DateTo )),
						_GenericReportData.CreateParameter("@ToDate", SqlDbType.VarChar, 10, (String.Format("{0:yyyy-MM-dd}",reportSettings.DateTo))), //String.Format("{0:yyyy-MM-dd}", reportSettings .DateFrom )),

						_GenericReportData.CreateParameter("@ReportType", SqlDbType.VarChar, 1, RunType),
                        _GenericReportData.CreateParameter("@TotalsPerMonth", SqlDbType.VarChar, 1,SummaryOn),

                        _GenericReportData.CreateParameter("@PlanMonth", SqlDbType.VarChar,1,PlanDyn),
                        _GenericReportData.CreateParameter("@PlanMonthLock", SqlDbType.VarChar, 1,PlanLock),
                        _GenericReportData.CreateParameter("@PlanProg", SqlDbType.VarChar, 1,PlanDynProg),
                        _GenericReportData.CreateParameter("@PlanProgLock", SqlDbType.VarChar, 1,PlanLockProg),
                        _GenericReportData.CreateParameter("@Book", SqlDbType.VarChar, 2,Book),
                        _GenericReportData.CreateParameter("@Meas", SqlDbType.VarChar, 2,Meas),
                        _GenericReportData.CreateParameter("@PlanBuss", SqlDbType.VarChar, 2, PlanBuss),
                        _GenericReportData.CreateParameter("@Abs", SqlDbType.VarChar, 2,AbsVar),
                        _GenericReportData.CreateParameter("@FC", SqlDbType.VarChar, 2,FC),

                        _GenericReportData.CreateParameter("@TheStopeLedge", SqlDbType.VarChar, 1, reportSettings.StopeLedge),
                        _GenericReportData.CreateParameter("@Level", SqlDbType.VarChar, 30, reportSettings.Level),
                    };

                _GenericReportData.ParamCollection = _paramCollection;
                _GenericReportData.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_GenericReportDataStope.ExecuteInstruction();
                clsDataResult errorMsg1 = _GenericReportData.ExecuteInstruction();
                if (errorMsg1.success == false)
                {
                    System.Windows.Forms.MessageBox.Show(errorMsg1.ToString());
                    // _sysMessagesClass.viewMessage(MessageType.Error, "SQL ERROR", DBTag, "clsSICReportData", "get_Sections", errorMsg.Message, ButtonTypes.OK, MessageDisplayType.FullScreen);
                    // return theData.ResultsDataTable;
                }
            }
            catch (Exception _exception)
            {
                throw new ApplicationException("Report Section:GenericReportData:" + _exception.Message, _exception);

            }

            string theReportHeadings = string.Empty; ;
            if (reportSettings.useProdmonth == true)
                theReportHeadings = "Production Month : " + PMonth + " up to " + CDate;
            if (reportSettings.useMonthToMonth == true)
                theReportHeadings = "Production Month : " + FPMonth + " to " + TPMonth;
            if (reportSettings.useDateToDate == true)
                theReportHeadings = "From  : " + FDate + " to " + TDate;

            string theTypeOfReport = string.Empty;
            if ((reportSettings.StpSqm == true) ||
                (reportSettings.StpSqmOn == true) ||
                (reportSettings.StpSqmOff == true) ||
                (reportSettings.StpSqmOS == true) ||
                (reportSettings.StpSqmOSF == true) ||
                (reportSettings.StpCmgt == true) ||
                (reportSettings.StpCmgtTotal == true) ||
                (reportSettings.StpGT == true) ||
                (reportSettings.StpGTTotal == true) ||
                (reportSettings.StpSW == true) ||
                (reportSettings.StpSWIdeal == true) ||
                (reportSettings.StpSWFault == true) ||
                (reportSettings.StpCW == true) ||
                (reportSettings.StpKG == true) ||
                (reportSettings.StpFL == true) ||
                (reportSettings.StpFLOn == true) ||
                (reportSettings.StpFLOff == true) ||
                (reportSettings.StpFLOS == true) ||
                (reportSettings.StpAdv == true) ||
                (reportSettings.StpAdvOn == true) ||
                (reportSettings.StpAdvOff == true) ||
                (reportSettings.StpTons == true) ||
                (reportSettings.StpTonsOn == true) ||
                (reportSettings.StpTonsOff == true) ||
                (reportSettings.StpTonsOS == true) ||
                (reportSettings.StpTonsFault == true) ||
                (reportSettings.StpMeasSweeps == true) ||
                (reportSettings.StpCubics == true) ||
                (reportSettings.StpCubGT == true) ||
                (reportSettings.StpCubTons == true) ||
                (reportSettings.StpCubKG == true) ||
                (reportSettings.StpLabour == true) ||
                (reportSettings.StpShftInfo == true))
            {
                theTypeOfReport = "Stoping";
            }
            if ((reportSettings.DevAdv == true) ||
                (reportSettings.DevAdvOn == true) ||
                (reportSettings.DevAdvOff == true) ||
                (reportSettings.DevEH == true) ||
                (reportSettings.DevEW == true) ||
                (reportSettings.DevTons == true) ||
                (reportSettings.DevTonsOn == true) ||
                (reportSettings.DevTonsOff == true) ||
                (reportSettings.DevCmgt == true) ||
                (reportSettings.DevCmgtTotal == true) ||
                (reportSettings.DevGT == true) ||
                (reportSettings.DevGTTotal == true) ||
                (reportSettings.DevKG == true) ||
                //(reportSettings. == true) ||
                (reportSettings.DevGTTotal == true) ||
                (reportSettings.DevCubics == true) ||
                (reportSettings.DevCubGT == true) ||
                (reportSettings.DevCubTons == true) ||
                (reportSettings.DevCubKG == true) ||
                (reportSettings.DevLabour == true) ||
                (reportSettings.DevShftInfo == true) ||
                (reportSettings.DevDrillRig == true))
            {
                if (theTypeOfReport == "Stoping")
                {
                    theTypeOfReport = "Both";
                }
                else
                {
                    theTypeOfReport = "Development";
                }
            }
            string thesection = "For Section : " + theName;

            MWDataManager.clsDataAccess _GenericCheckBoxes = new MWDataManager.clsDataAccess();
            try
            {
                _GenericCheckBoxes.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                _GenericCheckBoxes.SqlStatement = "sp_GenericReport_CheckBoxes";
                _GenericCheckBoxes.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                _GenericCheckBoxes.ResultsTableName = "GenericCheckBoxes";

                SqlParameter[] _paramCollection =
                    {
                        _GenericCheckBoxes.CreateParameter("@Section", SqlDbType.VarChar, 100, thesection),
                        _GenericCheckBoxes.CreateParameter("@Banner", SqlDbType.VarChar, 30, Banner),
                        _GenericCheckBoxes.CreateParameter("@theReportHeadings", SqlDbType.VarChar, 100,theReportHeadings),
                        _GenericCheckBoxes.CreateParameter("@theTypeOfReport", SqlDbType.VarChar, 100,theTypeOfReport),
                        _GenericCheckBoxes.CreateParameter("@RunLevel", SqlDbType.Int, 0,theHierID),
                        _GenericCheckBoxes.CreateParameter("@PlanDyn", SqlDbType.VarChar, 2,PlanDyn),
                        _GenericCheckBoxes.CreateParameter("@PlanLock", SqlDbType.VarChar, 2,PlanLock),
                        _GenericCheckBoxes.CreateParameter("@PlanDynProg", SqlDbType.VarChar, 2,PlanDynProg),
                        _GenericCheckBoxes.CreateParameter("@PlanLockProg", SqlDbType.VarChar, 2,PlanLockProg),
                        _GenericCheckBoxes.CreateParameter("@Book", SqlDbType.VarChar, 2,Book),
                        _GenericCheckBoxes.CreateParameter("@Meas", SqlDbType.VarChar, 2,Meas),
                        _GenericCheckBoxes.CreateParameter("@PlanBuss", SqlDbType.VarChar, 2, PlanBuss),
                        _GenericCheckBoxes.CreateParameter("@FC", SqlDbType.VarChar, 2,FC),
                        _GenericCheckBoxes.CreateParameter("@AbsVar", SqlDbType.VarChar, 2,AbsVar),

                        _GenericCheckBoxes.CreateParameter("@AuthPlanOnly", SqlDbType.VarChar, 1,AuthPlanOnly),
                        _GenericCheckBoxes.CreateParameter("@StopeLedge", SqlDbType.VarChar, 1,reportSettings.StopeLedge),

                        _GenericCheckBoxes.CreateParameter("@StpSqm", SqlDbType.VarChar, 2,StpSqm),
                        _GenericCheckBoxes.CreateParameter("@StpSqmOn", SqlDbType.VarChar, 2,StpSqmOn),
                        _GenericCheckBoxes.CreateParameter("@StpSqmOff", SqlDbType.VarChar, 2,StpSqmOff),
                        _GenericCheckBoxes.CreateParameter("@StpSqmOS", SqlDbType.VarChar, 2,StpSqmOS),
                        _GenericCheckBoxes.CreateParameter("@StpSqmOSF", SqlDbType.VarChar, 2,StpSqmOSF),
                        _GenericCheckBoxes.CreateParameter("@StpCmgt", SqlDbType.VarChar, 2,StpCmgt),
                        _GenericCheckBoxes.CreateParameter("@StpCmgtTotal", SqlDbType.VarChar, 2,StpCmgtTotal),
                        _GenericCheckBoxes.CreateParameter("@StpGT", SqlDbType.VarChar, 2,StpGT),
                        _GenericCheckBoxes.CreateParameter("@StpGTTotal", SqlDbType.VarChar, 2,StpGTTotal),
                        _GenericCheckBoxes.CreateParameter("@StpSW", SqlDbType.VarChar, 2,StpSW),
                        _GenericCheckBoxes.CreateParameter("@StpSWIdeal", SqlDbType.VarChar, 2,StpSWIdeal),
                        _GenericCheckBoxes.CreateParameter("@StpSWFault", SqlDbType.VarChar, 2,StpSWFault),
                        _GenericCheckBoxes.CreateParameter("@StpCW", SqlDbType.VarChar, 2,StpCW),
                        _GenericCheckBoxes.CreateParameter("@StpKG", SqlDbType.VarChar, 2,StpKG),
                        _GenericCheckBoxes.CreateParameter("@StpFL", SqlDbType.VarChar, 2,StpFL),
                        _GenericCheckBoxes.CreateParameter("@StpFLOn", SqlDbType.VarChar, 2,StpFLOn),
                        _GenericCheckBoxes.CreateParameter("@StpFLOff", SqlDbType.VarChar, 2,StpFLOff),
                        _GenericCheckBoxes.CreateParameter("@StpFLOS", SqlDbType.VarChar, 2,StpFLOS),
                        _GenericCheckBoxes.CreateParameter("@StpAdv", SqlDbType.VarChar, 2,StpAdv),
                        _GenericCheckBoxes.CreateParameter("@StpAdvOn", SqlDbType.VarChar, 2,StpAdvOn),
                        _GenericCheckBoxes.CreateParameter("@StpAdvOff", SqlDbType.VarChar, 2,StpAdvOff),
                        _GenericCheckBoxes.CreateParameter("@StpTons", SqlDbType.VarChar, 2,StpTons),
                        _GenericCheckBoxes.CreateParameter("@StpTonsOn", SqlDbType.VarChar, 2,StpTonsOn),
                        _GenericCheckBoxes.CreateParameter("@StpTonsOff", SqlDbType.VarChar, 2,StpTonsOff),
                        _GenericCheckBoxes.CreateParameter("@StpTonsOS", SqlDbType.VarChar, 2,StpTonsOS),
                        _GenericCheckBoxes.CreateParameter("@StpTonsFault", SqlDbType.VarChar, 2,StpTonsFault),
                        _GenericCheckBoxes.CreateParameter("@StpCubics", SqlDbType.VarChar, 2,StpCubics),
                        _GenericCheckBoxes.CreateParameter("@StpCubTons", SqlDbType.VarChar, 2,StpCubTons),
                        _GenericCheckBoxes.CreateParameter("@StpCubGT", SqlDbType.VarChar, 2,StpCubGT),
                        _GenericCheckBoxes.CreateParameter("@StpCubKG", SqlDbType.VarChar, 2,StpCubKG),
                        _GenericCheckBoxes.CreateParameter("@StpMeasSweeps", SqlDbType.VarChar, 2,StpMeasSweeps),
                        _GenericCheckBoxes.CreateParameter("@StpLabour", SqlDbType.VarChar, 2,StpLabour),
                        _GenericCheckBoxes.CreateParameter("@StpShftInfo", SqlDbType.VarChar, 2,StpShftInfo),

                        _GenericCheckBoxes.CreateParameter("@DevAdv", SqlDbType.VarChar, 2,DevAdv),
                        _GenericCheckBoxes.CreateParameter("@DevAdvOn", SqlDbType.VarChar, 2,DevAdvOn),
                        _GenericCheckBoxes.CreateParameter("@DevAdvOff", SqlDbType.VarChar, 2,DevAdvOff),
                        _GenericCheckBoxes.CreateParameter("@DevEH", SqlDbType.VarChar, 2,DevEH),
                        _GenericCheckBoxes.CreateParameter("@DevEW", SqlDbType.VarChar, 2,DevEW),
                        _GenericCheckBoxes.CreateParameter("@DevTons", SqlDbType.VarChar, 2,DevTons),
                        _GenericCheckBoxes.CreateParameter("@DevTonsOn", SqlDbType.VarChar, 2,DevTonsOn),
                        _GenericCheckBoxes.CreateParameter("@DevTonsOff", SqlDbType.VarChar, 2,DevTonsOff),
                        _GenericCheckBoxes.CreateParameter("@DevCmgt", SqlDbType.VarChar, 2,DevCmgt),
                        _GenericCheckBoxes.CreateParameter("@DevCmgtTotal", SqlDbType.VarChar, 2, DevCmgtTotal),
                        _GenericCheckBoxes.CreateParameter("@DevGT", SqlDbType.VarChar, 2,DevGT),
                        _GenericCheckBoxes.CreateParameter("@DevGTTotal", SqlDbType.VarChar, 2, DevGTTotal),
                        _GenericCheckBoxes.CreateParameter("@DevKG", SqlDbType.VarChar, 2,DevKG),
                        _GenericCheckBoxes.CreateParameter("@DevCubics", SqlDbType.VarChar, 2,DevCubics),
                        _GenericCheckBoxes.CreateParameter("@DevCubTons", SqlDbType.VarChar, 2,DevCubTons),
                        _GenericCheckBoxes.CreateParameter("@DevCubGT", SqlDbType.VarChar, 2,DevCubGT),
                        _GenericCheckBoxes.CreateParameter("@DevCubKG", SqlDbType.VarChar, 2,DevCubKG),
                        _GenericCheckBoxes.CreateParameter("@DevLabour", SqlDbType.VarChar, 2,DevLabour),
                        _GenericCheckBoxes.CreateParameter("@DevShftInfo", SqlDbType.VarChar, 2,DevShftInfo),
                        _GenericCheckBoxes.CreateParameter("@DevDrillRig", SqlDbType.VarChar, 2,DevDrillRig),
                    };

                _GenericCheckBoxes.ParamCollection = _paramCollection;
                _GenericCheckBoxes.queryReturnType = MWDataManager.ReturnType.DataTable;
                clsDataResult errorMsg = _GenericCheckBoxes.ExecuteInstruction();
                if (errorMsg.success == false)
                {
                    _sysMessagesClass.viewMessage(MessageType.Error, "SQL ERROR", theSystemDBTag, "ucGenericReport", "createReport", errorMsg.Message, ButtonTypes.OK, MessageDisplayType.FullScreen);
                }
            }
            catch (Exception _exception)
            {
                throw new ApplicationException("Report Section:GenericCheckBoxes:" + _exception.Message, _exception);
            }

            //if (_GenericReportData.ResultsDataTable.Rows.Count != 0)
            DataSet repGenericReportDataSet = new DataSet();

            DataTable dtGenericStpPage2;
            dtGenericStpPage2 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericStpPage2 = _GenericReportData.ResultsDataTable.Copy();

            DataTable dtGenericStpPage3;
            dtGenericStpPage3 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericStpPage3 = _GenericReportData.ResultsDataTable.Copy();

            DataTable dtGenericStpPage4;
            dtGenericStpPage4 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericStpPage4 = _GenericReportData.ResultsDataTable.Copy();

            DataTable dtGenericStpPage5;
            dtGenericStpPage5 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericStpPage5 = _GenericReportData.ResultsDataTable.Copy();

            DataTable dtGenericStpPage6;
            dtGenericStpPage6 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericStpPage6 = _GenericReportData.ResultsDataTable.Copy();

            DataTable dtGenericDevPage1;
            dtGenericDevPage1 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericDevPage1 = _GenericReportData.ResultsDataTable.Copy();

            DataTable dtGenericDevPage2;
            dtGenericDevPage2 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericDevPage2 = _GenericReportData.ResultsDataTable.Copy();

            DataTable dtGenericDevPage3;
            dtGenericDevPage3 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericDevPage3 = _GenericReportData.ResultsDataTable.Copy();

            DataTable dtGenericDevPage4;
            dtGenericDevPage4 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericDevPage4 = _GenericReportData.ResultsDataTable.Copy();

            DataTable dtGenericDevPage5;
            dtGenericDevPage5 = _GenericReportData.ResultsDataTable.Clone();
            dtGenericDevPage5 = _GenericReportData.ResultsDataTable.Copy();

            repGenericReportDataSet.Tables.Add(_GenericReportData.ResultsDataTable);
            repGenericReportDataSet.Tables.Add(_GenericCheckBoxes.ResultsDataTable);
            GenReport.RegisterData(repGenericReportDataSet);
            GenReport.RegisterData(dtGenericStpPage2, "GenericStpPage2");
            GenReport.RegisterData(dtGenericStpPage3, "GenericStpPage3");
            GenReport.RegisterData(dtGenericStpPage4, "GenericStpPage4");
            GenReport.RegisterData(dtGenericStpPage5, "GenericStpPage5");
            GenReport.RegisterData(dtGenericStpPage6, "GenericStpPage6");
            GenReport.RegisterData(dtGenericDevPage1, "GenericDevPage1");
            GenReport.RegisterData(dtGenericDevPage2, "GenericDevPage2");
            GenReport.RegisterData(dtGenericDevPage3, "GenericDevPage3");
            GenReport.RegisterData(dtGenericDevPage4, "GenericDevPage4");
            GenReport.RegisterData(dtGenericDevPage5, "GenericDevPage5");

            GenReport.Load(_reportFolder + "GenericReport.frx");
            GenReport.SetParameterValue("ReportProperties", theReportSettings);
            GenReport.SetParameterValue("logo", TGlobalItems.ClientLogo);
            GenReport.SetParameterValue("logoMineware", TGlobalItems.CompanyLogo);
            GenReport.SetParameterValue("CopyLayout", reportSettings.CopyLayout);

            //LoadReport(GenReport);

            _theReport = GenReport;
            //_theReport.Design();
            _theReport.Prepare();
            _theReport.Preview = pcReport2;
            _theReport.ShowPrepared();
        }

        private void pgSettingsMain_Click(object sender, EventArgs e)
        {

        }

        private string DevTons, DevTonsOn, DevTonsOff, DevCubics, DevCubGT, DevCubTons, DevCubKG;
        private string DevLabour, DevShftInfo, DevDrillRig, DevCmgt, DevCmgtTotal, DevGT, DevGTTotal;

        private DataTable dtMinMaxMonthsForSection;
        private DataTable dtMinMaxDatesForSection;
        private DataTable dtMinMaxMonths;
        private DataTable dtSections;
        private DataTable dtHierID;
        private DataTable dtSysset;
        private DataTable dtLevSum;
        private DataTable dtExistReportNames;
        private DataTable dtActivity;

        private DataTable dtLevel;
        public ucGenericReport()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpMeasList);
            FormActiveRibbonPage = rpMeasList;
            FormMainRibbonPage = rpMeasList;
            RibbonControl = rcMeaslist;           
            this.MouseWheel += new MouseEventHandler(Form_MouseWheel);
           
        }
        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void ucGenericReport_Load(object sender, EventArgs e)
        {
            //ActiveReport.UserCurrentInfo = this.UserCurrentInfo;
            reportSettings.UserCurrentInfo = this.UserCurrentInfo.Connection;
            reportSettings.DBTag = theSystemDBTag;  // this.theSystemDBTag;

            //accordionContentContainer1.Height = this.Height - 50;
            //accordionContentContainer1.Height = pcReport2.Height + 10;

            setReportSetttings();
        }
        private void setReportSetttings()
        {
            Load_ExistReportNames();

            reportSettings.useProdmonth = true;
            reportSettings.useMonthToMonth = false;
            reportSettings.useDateToDate = false;
            reportSettings.CopyLayout = false;

            reportSettings.AuthPlanOnly = false;
            reportSettings.StopeLedge = "0";

            reportSettings.LevGM = true;
            reportSettings.LevMN = true;
            reportSettings.LevMNM = true;
            reportSettings.LevMO = true;
            reportSettings.LevSB = true;
            reportSettings.LevMiner = true;
            reportSettings.LevWP = true;

            reportSettings.PlanDyn = false;
            reportSettings.PlanLock = false;
            reportSettings.PlanDynProg = false;
            reportSettings.PlanLockProg = false;
            reportSettings.Book = false;
            reportSettings.Meas = false;
            reportSettings.PlanBuss = false;
            reportSettings.FC = false;
            reportSettings.AbsVar = false;

            reportSettings.StpSqm = false;
            reportSettings.StpSqmOn = false;
            reportSettings.StpSqmOff = false;
            reportSettings.StpSqmOS = false;
            reportSettings.StpSqmOSF = false;
            reportSettings.StpCmgt = false;
            reportSettings.StpCmgtTotal = false;
            reportSettings.StpGT = false;
            reportSettings.StpGTTotal = false;
            reportSettings.StpSW = false;
            reportSettings.StpSWIdeal = false;
            reportSettings.StpSWFault = false;
            reportSettings.StpCW = false;
            reportSettings.StpKG = false;
            reportSettings.StpFL = false;
            reportSettings.StpFLOn = false;
            reportSettings.StpFLOff = false;
            reportSettings.StpFLOS = false;
            reportSettings.StpAdv = false;
            reportSettings.StpAdvOn = false;
            reportSettings.StpAdvOff = false;
            reportSettings.StpTons = false;
            reportSettings.StpTonsOn = false;
            reportSettings.StpTonsOff = false;
            reportSettings.StpTonsOS = false;
            reportSettings.StpTonsFault = false;
            reportSettings.StpCubics = false;
            reportSettings.StpCubGT = false;
            reportSettings.StpCubTons = false;
            reportSettings.StpCubKG = false;
            reportSettings.StpMeasSweeps = false;
            reportSettings.StpLabour = false;
            reportSettings.StpShftInfo = false;

            //Load_BussPlanUnits();

            reportSettings.DevAdv = false;
            reportSettings.DevAdvOn = false;
            reportSettings.DevAdvOff = false;
            reportSettings.DevEH = false;
            reportSettings.DevEW = false;
            reportSettings.DevTons = false;
            reportSettings.DevTonsOn = false;
            reportSettings.DevTonsOff = false;
            reportSettings.DevCmgt = false;
            reportSettings.DevCmgtTotal = false;
            reportSettings.DevGT = false;
            reportSettings.DevGTTotal = false;
            reportSettings.DevKG = false;
            reportSettings.DevCubics = false;
            reportSettings.DevCubGT = false;
            reportSettings.DevCubTons = false;
            reportSettings.DevCubKG = false;
            reportSettings.DevLabour = false;
            reportSettings.DevShftInfo = false;
            reportSettings.DevDrillRig = false;

            setDateSelection();

            setScreenSelection();

            Load_LevelOfSummaries();
            LoadSections();
            LoadActivity();
            LoadLevel();

            reportSettings.Level = "0";

            pgSettingsMain.SelectedObject = reportSettings;
        }

        private void setDateSelection()
        {
            string _Prodmonth;

            _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            dtSysset = _clsGenericReportData.get_Sysset();
            if (dtSysset.Rows.Count > 0)
            {
                getMonth = dtSysset.Rows[0]["CurrentProductionMonth"].ToString();
                Banner = dtSysset.Rows[0]["Banner"].ToString();
                //_HierID = (Convert.ToInt32(dtSysset.Rows[0]["MOHierarchicalID"].ToString()) + 2).ToString();
            }
            //getMonth = TProductionGlobal.getSystemSettingsProductioInfo(UserCurrentInfo.Connection).CurrentProductionMonth.ToString();
            _Prodmonth = getMonth;
            PMonth = getMonth;
            FPMonth = getMonth;
            TPMonth = getMonth;
            GettheDay(DateTime.Now);
            CDate = getDay;
            FDate = getDay;
            TDate = getDay;

            int theYear = Convert.ToInt32(_Prodmonth.Substring(0, 4));
            int theMonth = Convert.ToInt32(_Prodmonth.Substring(4, 2));
            DateTime theDate = new DateTime(theYear, theMonth, 1);

            reportSettings.Prodmonth = theDate.Date;
            reportSettings.ProdmonthFrom = theDate.Date;
            reportSettings.ProdmonthTo = theDate.Date;
            reportSettings.CalendarDate = DateTime.Now;
            reportSettings.DateFrom = DateTime.Now;
            reportSettings.DateTo = DateTime.Now;
        }

        private void setScreenSelection()
        {

            rowSaveReport.Properties.Value = reportSettings.SaveReport;

            if (reportSettings.useProdmonth == true)
            {
                //reportSettings.AuthPlanOnly = false;
                //rowAuthPlanOnly.Visible = false;
                Load_LevelVisibles("A");
            }
            if (reportSettings.useDateToDate == true)
            {
                rowFromDate.Visible = reportSettings.useDateToDate;
                //reportSettings.AuthPlanOnly = false;
                //rowAuthPlanOnly.Visible = true;
                Load_LevelVisibles(string.Empty);
            }
            if (reportSettings.useMonthToMonth == true)
            {
                //reportSettings.AuthPlanOnly = false;
                //rowAuthPlanOnly.Visible = true;
                Load_LevelVisibles("A");
            }
            rowCalendarDate.Visible = reportSettings.useProdmonth;
            rowFromDate.Visible = reportSettings.useDateToDate;
            rowToDate.Visible = reportSettings.useDateToDate;
            rowProdmoth.Visible = reportSettings.useProdmonth;
            rowFromProdmonth.Visible = reportSettings.useMonthToMonth;
            rowToProdmonth.Visible = reportSettings.useMonthToMonth;
            rowProdmoth.Properties.Value = reportSettings.Prodmonth;

            rowAuthPlanOnly.Properties.Value = reportSettings.AuthPlanOnly;
            rowStopeLedge.Properties.Value = reportSettings.StopeLedge;

            rowGM.Properties.Value = reportSettings.LevGM;
            rowMN.Properties.Value = reportSettings.LevMN;
            rowMNM.Properties.Value = reportSettings.LevMNM;
            rowMO.Properties.Value = reportSettings.LevMO;
            rowSB.Properties.Value = reportSettings.LevSB;
            rowMiner.Properties.Value = reportSettings.LevMiner;
            rowWP.Properties.Value = reportSettings.LevWP;

            rowGM.Enabled = true;
            rowMN.Enabled = true;
            rowMNM.Enabled = true;
            rowMO.Enabled = true;
            rowSB.Enabled = true;
            rowMiner.Enabled = true;
            rowWP.Enabled = true;

            rowPlanDyn.Properties.Value = reportSettings.PlanDyn;
            rowPlanLock.Properties.Value = reportSettings.PlanLock;
            rowPlanDynProg.Properties.Value = reportSettings.PlanDynProg;
            rowPlanLockProg.Properties.Value = reportSettings.PlanLockProg;
            rowBook.Properties.Value = reportSettings.Book;
            rowMeas.Properties.Value = reportSettings.Meas;
            rowPlanBuss.Properties.Value = reportSettings.PlanBuss;
            rowFC.Properties.Value = reportSettings.FC;
            rowAbsVar.Properties.Value = reportSettings.AbsVar;

            rowStpSqm.Properties.Value = reportSettings.StpSqm;
            rowStpSqmOn.Properties.Value = reportSettings.StpSqmOn;
            rowStpSqmOff.Properties.Value = reportSettings.StpSqmOff;
            rowStpSqmOS.Properties.Value = reportSettings.StpSqmOS;
            rowStpSqmOSF.Properties.Value = reportSettings.StpSqmOSF;
            rowStpCmgt.Properties.Value = reportSettings.StpCmgt;
            rowStpCmgtTotal.Properties.Value = reportSettings.StpCmgtTotal;
            rowStpGT.Properties.Value = reportSettings.StpGT;
            rowStpGTTotal.Properties.Value = reportSettings.StpGTTotal;
            rowStpSW.Properties.Value = reportSettings.StpSW;
            rowStpSWIdeal.Properties.Value = reportSettings.StpSWIdeal;
            rowStpSWFault.Properties.Value = reportSettings.StpSWFault;
            rowStpCW.Properties.Value = reportSettings.StpCW;
            rowStpKG.Properties.Value = reportSettings.StpKG;
            rowStpFL.Properties.Value = reportSettings.StpFL;
            rowStpFLOn.Properties.Value = reportSettings.StpFLOn;
            rowStpFLOff.Properties.Value = reportSettings.StpFLOff;
            rowStpFLOS.Properties.Value = reportSettings.StpFLOS;
            rowStpAdv.Properties.Value = reportSettings.StpAdv;
            rowStpAdvOn.Properties.Value = reportSettings.StpAdvOn;
            rowStpAdvOff.Properties.Value = reportSettings.StpAdvOff;
            rowStpTons.Properties.Value = reportSettings.StpTons;
            rowStpTonsOn.Properties.Value = reportSettings.StpTonsOn;
            rowStpTonsOff.Properties.Value = reportSettings.StpTonsOff;
            rowStpTonsOS.Properties.Value = reportSettings.StpTonsOS;
            rowStpTonsFault.Properties.Value = reportSettings.StpTonsFault;
            rowStpCubics.Properties.Value = reportSettings.StpCubics;
            rowStpCubGT.Properties.Value = reportSettings.StpCubGT;
            rowStpCubTons.Properties.Value = reportSettings.StpCubTons;
            rowStpCubKG.Properties.Value = reportSettings.StpCubKG;
            rowStpMeasSweeps.Properties.Value = reportSettings.StpMeasSweeps;
            rowStpLabour.Properties.Value = reportSettings.StpLabour;
            rowStpShftInfo.Properties.Value = reportSettings.StpShftInfo;

            rowDevAdv.Properties.Value = reportSettings.DevAdv;
            rowDevAdvOff.Properties.Value = reportSettings.DevAdvOn;
            rowDevAdvOff.Properties.Value = reportSettings.DevAdvOff;
            rowDevEH.Properties.Value = reportSettings.DevEH;
            rowDevEW.Properties.Value = reportSettings.DevEW;
            rowDevTons.Properties.Value = reportSettings.DevTons;
            rowDevTonsOn.Properties.Value = reportSettings.DevTonsOn;
            rowDevTonsOff.Properties.Value = reportSettings.DevTonsOff;
            rowDevCmgt.Properties.Value = reportSettings.DevCmgt;
            rowDevCmgtTotal.Properties.Value = reportSettings.DevCmgtTotal;
            rowDevGT.Properties.Value = reportSettings.DevGT;
            rowDevGTTotal.Properties.Value = reportSettings.DevGTTotal;
            rowDevKG.Properties.Value = reportSettings.DevKG;
            rowDevCubics.Properties.Value = reportSettings.DevCubics;
            rowDevCubGT.Properties.Value = reportSettings.DevCubGT;
            rowDevCubTons.Properties.Value = reportSettings.DevCubTons;
            rowDevCubKG.Properties.Value = reportSettings.DevCubKG;
            rowDevLabour.Properties.Value = reportSettings.DevLabour;
            rowDevShftInfo.Properties.Value = reportSettings.DevShftInfo;
            rowDevDrillRig.Properties.Value = reportSettings.DevDrillRig;
        }
        public void LoadSections()
        {
            if (reportSettings.useProdmonth == true)
            {
                _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                dtSections = _clsGenericReportData.get_Sections(procs.ProdMonthAsString(reportSettings.Prodmonth));
                if (dtSections.Rows.Count > 0)
                {
                    rpSection.DataSource = dtSections;
                    rpSection.DisplayMember = "Name";
                    rpSection.ValueMember = "SectionID";
                }
            }

            if (reportSettings.useMonthToMonth == true)
            {
                _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                dtSections = _clsGenericReportData.get_Sections(procs.ProdMonthAsString(reportSettings.ProdmonthFrom));
                if (dtSections.Rows.Count > 0)
                {
                    rpSection.DataSource = dtSections;
                    rpSection.DisplayMember = "Name";
                    rpSection.ValueMember = "SectionID";
                }

            }
            if (reportSettings.useDateToDate == true)
            {
                try
                {
                    _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    dtMinMaxMonths = _clsGenericReportData.get_MinMaxMonths(reportSettings.DateFrom.ToString(), reportSettings.DateTo.ToString());

                    DataTable y = dtMinMaxMonths;

                    if (y.Rows.Count != 0)
                    {
                        if (((y.Rows[0]["MinProd"] == DBNull.Value) || (y.Rows[0]["MinProd"].ToString() == string.Empty)) &
                            ((y.Rows[0]["MaxProd"] == DBNull.Value) || (y.Rows[0]["MaxProd"].ToString() == string.Empty)))
                        {
                        }
                        else
                        {
                            PMonth = y.Rows[0]["MinProd"].ToString();
                            FPMonth = y.Rows[0]["MinProd"].ToString();
                            TPMonth = y.Rows[0]["MaxProd"].ToString();
                        }
                    }
                }
                catch (Exception _ex)
                {
                    throw new ApplicationException(_ex.Message, _ex);
                }
            }
            if ((reportSettings.useMonthToMonth == true) ||
                (reportSettings.useDateToDate == true))
            {
                if (((FPMonth == null) || (FPMonth == string.Empty)) &
                    ((TPMonth == null) || (TPMonth == string.Empty)))
                {
                }
                else
                {
                    try
                    {
                        FoundNoSections = false;

                        _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        dtSections = _clsGenericReportData.get_Sections(procs.ProdMonthAsString(reportSettings.ProdmonthFrom), procs.ProdMonthAsString(reportSettings.ProdmonthTo));

                        if (dtSections.Rows.Count != 0)
                        {
                            rpSection.DataSource = dtSections;
                            rpSection.DisplayMember = "Name";
                            rpSection.ValueMember = "SectionID";
                        }
                        else
                        {
                            FoundNoSections = true;
                        }
                    }
                    catch (Exception _ex)
                    {
                        throw new ApplicationException(_ex.Message, _ex);
                    }
                }
            }
            if ((reportSettings.useProdmonth == true) ||
                (reportSettings.useMonthToMonth == true))
            {
                if (((FPMonth == null) || (FPMonth == string.Empty)) &
                    ((TPMonth == null) || (TPMonth == string.Empty)))
                {
                }
                else
                {
                    try
                    {
                        Load_Dates();
                    }
                    catch (Exception _ex)
                    {
                        throw new ApplicationException(_ex.Message, _ex);
                    }
                }
            }

        }
        public void LoadActivity()
        {
            _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            dtActivity = _clsGenericReportData.get_Activity();
            if (dtActivity.Rows.Count > 0)
            {
                rpStopeLedge.DataSource = dtActivity;
                rpStopeLedge.DisplayMember = "ActDesc";
                rpStopeLedge.ValueMember = "Act";
            }
        }

        public void LoadLevel()
        {
            _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            dtLevel = _clsGenericReportData.get_Level();
            if (dtLevel.Rows.Count > 0)
            {
                CheckComboLevel.DataSource = dtLevel;
                CheckComboLevel.DisplayMember = "Name";
                CheckComboLevel.ValueMember = "OreflowID";
            }
        }
        public void Load_LevelOfSummaries()
        {
            _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            dtLevSum = _clsGenericReportData.get_LevSum();

            foreach (DataRow dr in dtLevSum.Rows)
            {
                if (Convert.ToInt32(dr["HierarchicalID"].ToString()) == 1)
                    rowGM.Properties.Caption = dr["Description"].ToString();
                if (Convert.ToInt32(dr["HierarchicalID"].ToString()) == 2)
                    rowMN.Properties.Caption = dr["Description"].ToString();
                if (Convert.ToInt32(dr["HierarchicalID"].ToString()) == 3)
                    rowMNM.Properties.Caption = dr["Description"].ToString();
                if (Convert.ToInt32(dr["HierarchicalID"].ToString()) == 4)
                    rowMO.Properties.Caption = dr["Description"].ToString();
                if (Convert.ToInt32(dr["HierarchicalID"].ToString()) == 5)
                    rowSB.Properties.Caption = dr["Description"].ToString();
                if (Convert.ToInt32(dr["HierarchicalID"].ToString()) == 6)
                    rowMiner.Properties.Caption = dr["Description"].ToString();
            }
        }
        private void Load_Dates()
        {
            try
            {

                if (reportSettings.useMonthToMonth == true)
                {
                    _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    dtMinMaxDatesForSection = _clsGenericReportData.get_MinMaxDatesForSection(procs.ProdMonthAsString(reportSettings.ProdmonthFrom), procs.ProdMonthAsString(reportSettings.ProdmonthTo), reportSettings.SectionID, theHierID);

                    DataTable y = dtMinMaxDatesForSection;

                    if ((y.Rows[0]["StartDate"] != DBNull.Value) &
                        (y.Rows[0]["EndDate"] != DBNull.Value))
                    {
                        GettheDay(Convert.ToDateTime(y.Rows[0]["StartDate"].ToString()));
                        FDate = getDay;
                        reportSettings.DateFrom = Convert.ToDateTime(FDate);
                        if ((reportSettings.DateTo >= Convert.ToDateTime(y.Rows[0]["StartDate"].ToString())) &&
                            (reportSettings.DateTo <= Convert.ToDateTime(y.Rows[0]["EndDate"].ToString())))
                        {
                            GettheDay(reportSettings.DateTo);
                            TDate = getDay;
                        }
                        else
                        {
                            if ((DateTime.Now >= Convert.ToDateTime(y.Rows[0]["StartDate"].ToString())) &&
                                (DateTime.Now <= Convert.ToDateTime(y.Rows[0]["EndDate"].ToString())))
                            {
                                GettheDay(DateTime.Now);
                                TDate = getDay;
                            }
                            else
                            {
                                GettheDay(Convert.ToDateTime(y.Rows[0]["EndDate"].ToString()));
                                TDate = getDay;
                            }
                        }
                        reportSettings.DateTo = Convert.ToDateTime(TDate);
                    }
                }
                if (reportSettings.useProdmonth == true)
                {
                    _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    dtMinMaxDatesForSection = _clsGenericReportData.get_MinMaxDatesForSection(procs.ProdMonthAsString(reportSettings.Prodmonth), procs.ProdMonthAsString(reportSettings.Prodmonth), reportSettings.SectionID, theHierID);

                    DataTable y = dtMinMaxDatesForSection;

                    if ((y.Rows[0]["StartDate"] != DBNull.Value) &
                        (y.Rows[0]["EndDate"] != DBNull.Value))
                    {
                        if ((reportSettings.CalendarDate >= Convert.ToDateTime(y.Rows[0]["StartDate"].ToString())) &&
                            (reportSettings.CalendarDate <= Convert.ToDateTime(y.Rows[0]["EndDate"].ToString())))
                        {
                            GettheDay(reportSettings.CalendarDate);
                            CDate = getDay;
                        }
                        else
                        {
                            if ((DateTime.Now >= Convert.ToDateTime(y.Rows[0]["StartDate"].ToString())) &&
                                (DateTime.Now <= Convert.ToDateTime(y.Rows[0]["EndDate"].ToString())))
                            {
                                GettheDay(DateTime.Now);
                                CDate = getDay;
                            }
                            else
                            {
                                GettheDay(Convert.ToDateTime(y.Rows[0]["EndDate"].ToString()));
                                CDate = getDay;
                            }
                        }
                        reportSettings.CalendarDate = Convert.ToDateTime(CDate);
                    }
                }

                if (reportSettings.useDateToDate == true)
                {
                    _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    dtMinMaxMonthsForSection = _clsGenericReportData.get_MinMaxMonthsForSection(reportSettings.DateFrom.ToString(), reportSettings.DateTo.ToString(), reportSettings.SectionID, theHierID);

                    DataTable y = dtMinMaxMonthsForSection;

                    if (y.Rows.Count != 0)
                    {
                        if ((y.Rows[0]["BeginMonth"].ToString() != null) &
                            (y.Rows[0]["EndMonth"].ToString() != null))
                        {
                            PMonth = y.Rows[0]["BeginMonth"].ToString();
                            FPMonth = y.Rows[0]["BeginMonth"].ToString();
                            TPMonth = y.Rows[0]["EndMonth"].ToString();
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                throw new ApplicationException(_ex.Message, _ex);
            }
        }

        public void LoadHierID()
        {
            if (reportSettings.useProdmonth == true)
            {
                _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                dtHierID = _clsGenericReportData.get_HierID(procs.ProdMonthAsString(reportSettings.Prodmonth), reportSettings.SectionID);

                if (dtHierID.Rows.Count != 0)
                {
                    theHierID = dtHierID.Rows[0]["HierarchicalID"].ToString();
                    theName = dtHierID.Rows[0]["Name"].ToString();
                }
            }
            if (reportSettings.useMonthToMonth == true)
            {
                _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                dtHierID = _clsGenericReportData.get_HierID(procs.ProdMonthAsString(reportSettings.ProdmonthFrom), reportSettings.SectionID);

                if (dtHierID.Rows.Count != 0)
                {
                    theHierID = dtHierID.Rows[0]["HierarchicalID"].ToString();
                    theName = dtHierID.Rows[0]["Name"].ToString();
                }
            }

            if (reportSettings.useDateToDate == true)
            {
                _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                dtHierID = _clsGenericReportData.get_HierID(procs.ProdMonthAsString(reportSettings.ProdmonthFrom), reportSettings.SectionID);

                if (dtHierID.Rows.Count != 0)
                {
                    theHierID = dtHierID.Rows[0]["HierarchicalID"].ToString();
                    theName = dtHierID.Rows[0]["Name"].ToString();
                }

                _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                dtMinMaxMonthsForSection = _clsGenericReportData.get_MinMaxMonthsForSection(reportSettings.DateFrom.ToString("yyyy/MM/dd"), reportSettings.DateTo.ToString("yyyy/MM/dd"), reportSettings.SectionID, theHierID);

                DataTable y = dtMinMaxMonthsForSection;

                if (y.Rows.Count != 0)
                {
                    if ((y.Rows[0]["BeginMonth"].ToString() != null) &
                        (y.Rows[0]["EndMonth"].ToString() != null))
                    {
                        PMonth = y.Rows[0]["BeginMonth"].ToString();
                        FPMonth = y.Rows[0]["BeginMonth"].ToString();
                        TPMonth = y.Rows[0]["EndMonth"].ToString();

                        _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        dtHierID = _clsGenericReportData.get_HierID(PMonth, reportSettings.SectionID);

                        if (dtHierID.Rows.Count != 0)
                        {
                            theHierID = dtHierID.Rows[0]["HierarchicalID"].ToString();
                            theName = dtHierID.Rows[0]["Name"].ToString();
                        }
                    }
                }
            }

        }

        public void prepareReport()
        {
            bool theResult = false;

            ErrFound = false;
            CheckForErrors();

            if (ErrFound == false)
            {
                theReportThread = new Thread(new ParameterizedThreadStart(createReport));
                theReportThread.SetApartmentState(ApartmentState.STA);
                theReportThread.Start(reportSettings);
                theResult = true;
            }
            //return theResult;
        }

        private void get_Parametres()
        {
            if (reportSettings.useProdmonth == true)
                DateType = "P";
            if (reportSettings.useMonthToMonth == true)
                DateType = "M";
            if (reportSettings.useDateToDate == true)
                DateType = "D";

            SummaryOn = "N";
            if (reportSettings.AuthPlanOnly == true)
                SummaryOn = "Y";

            CDate = String.Format("{0:yyyy-MM-dd}", reportSettings.CalendarDate);
            FDate = String.Format("{0:yyyy-MM-dd}", reportSettings.DateFrom);
            TDate = String.Format("{0:yyyy-MM-dd}", reportSettings.DateTo);

            LevGM = "N";
            LevMN = "N";
            LevMNM = "N";
            LevMO = "N";
            LevSB = "N";
            LevMiner = "N";
            LevWP = "N";
            if (reportSettings.LevGM == true)
                LevGM = "Y";
            if (reportSettings.LevMN == true)
                LevMN = "Y";
            if (reportSettings.LevMNM == true)
                LevMNM = "Y";
            if (reportSettings.LevMO == true)
                LevMO = "Y";
            if (reportSettings.LevSB == true)
                LevSB = "Y";
            if (reportSettings.LevMiner == true)
                LevMiner = "Y";
            if (reportSettings.LevWP == true)
                LevWP = "Y";

            PlanDyn = "N";
            if (reportSettings.PlanDyn == true)
                PlanDyn = "Y";
            PlanLock = "N";
            if (reportSettings.PlanLock == true)
                PlanLock = "Y";
            PlanDynProg = "N";
            if (reportSettings.PlanDynProg == true)
                PlanDynProg = "Y";
            PlanLockProg = "N";
            if (reportSettings.PlanLockProg == true)
                PlanLockProg = "Y";
            Book = "N";
            if (reportSettings.Book == true)
                Book = "Y";
            Meas = "N";
            if (reportSettings.Meas == true)
                Meas = "Y";
            PlanBuss = "N";
            if (reportSettings.PlanBuss == true)
                PlanBuss = "Y";
            FC = "N";
            if (reportSettings.FC == true)
                FC = "Y";
            AbsVar = "N";
            if (reportSettings.AbsVar == true)
                AbsVar = "Y";

            AuthPlanOnly = "N";
            if (reportSettings.AuthPlanOnly == true)
                AuthPlanOnly = "Y";

            StpSqm = "N";
            if (reportSettings.StpSqm == true)
                StpSqm = "Y";
            StpSqmOn = "N";
            if (reportSettings.StpSqmOn == true)
                StpSqmOn = "Y";
            StpSqmOff = "N";
            if (reportSettings.StpSqmOff == true)
                StpSqmOff = "Y";
            StpSqmOS = "N";
            if (reportSettings.StpSqmOS == true)
                if (reportSettings.PlanBuss == true)
                    StpSqmOS = "Y";
            StpSqmOSF = "N";
            if (reportSettings.StpSqmOSF == true)
                if (reportSettings.PlanBuss == true)
                    StpSqmOSF = "Y";
            StpCmgt = "N";
            if (reportSettings.StpCmgt == true)
                StpCmgt = "Y";
            StpCmgtTotal = "N";
            if (reportSettings.StpCmgtTotal == true)
                StpCmgtTotal = "Y";
            StpGT = "N";
            if (reportSettings.StpGT == true)
                StpGT = "Y";
            StpGTTotal = "N";
            if (reportSettings.StpGTTotal == true)
                StpGTTotal = "Y";
            StpSW = "N";
            if (reportSettings.StpSW == true)
                StpSW = "Y";
            StpSWIdeal = "N";
            if (reportSettings.StpSWIdeal == true)
                StpSWIdeal = "Y";
            StpSWFault = "N";
            if (reportSettings.StpSWFault == true)
                if (reportSettings.PlanBuss == true)
                    StpSWFault = "Y";
            StpCW = "N";
            if (reportSettings.StpCW == true)
                StpCW = "Y";
            StpKG = "N";
            if (reportSettings.StpKG == true)
                StpKG = "Y";
            StpFL = "N";
            if (reportSettings.StpFL == true)
                StpFL = "Y";
            StpFLOn = "N";
            if (reportSettings.StpFLOn == true)
                StpFLOn = "Y";
            StpFLOff = "N";
            if (reportSettings.StpFLOff == true)
                StpFLOff = "Y";
            StpFLOS = "N";
            if (reportSettings.StpFLOS == true)
                if (reportSettings.PlanBuss == true)
                    StpFLOS = "Y";
            StpAdv = "N";
            if (reportSettings.StpAdv == true)
                StpAdv = "Y";
            StpAdvOn = "N";
            if (reportSettings.StpAdvOn == true)
                StpAdvOn = "Y";
            StpAdvOff = "N";
            if (reportSettings.StpAdvOff == true)
                StpAdvOff = "Y";
            StpTons = "N";
            if (reportSettings.StpTons == true)
                StpTons = "Y";
            StpTonsOn = "N";
            if (reportSettings.StpTonsOn == true)
                StpTonsOn = "Y";
            StpTonsOff = "N";
            if (reportSettings.StpTonsOff == true)
                StpTonsOff = "Y";
            StpTonsOS = "N";
            if (reportSettings.StpTonsOS == true)
                if (reportSettings.PlanBuss == true)
                    StpTonsOS = "Y";
            StpTonsFault = "N";
            if (reportSettings.StpTonsFault == true)
                if (reportSettings.PlanBuss == true)
                    StpTonsFault = "Y";
            StpCubics = "N";
            if (reportSettings.StpCubics == true)
                StpCubics = "Y";
            StpCubGT = "N";
            if (reportSettings.StpCubGT == true)
                StpCubGT = "Y";
            StpCubTons = "N";
            if (reportSettings.StpCubTons == true)
                StpCubTons = "Y";
            StpCubKG = "N";
            if (reportSettings.StpCubKG == true)
                StpCubKG = "Y";
            StpMeasSweeps = "N";
            if (reportSettings.StpMeasSweeps == true)
                StpMeasSweeps = "Y";
            StpLabour = "N";
            if (reportSettings.StpLabour == true)
                StpLabour = "Y";
            StpShftInfo = "N";
            if (reportSettings.StpShftInfo == true)
                StpShftInfo = "Y";

            DevAdv = "N";
            if (reportSettings.DevAdv == true)
                DevAdv = "Y";
            DevAdvOn = "N";
            if (reportSettings.DevAdvOn == true)
                DevAdvOn = "Y";
            DevAdvOff = "N";
            if (reportSettings.DevAdvOff == true)
                DevAdvOff = "Y";
            DevEH = "N";
            if (reportSettings.DevEH == true)
                DevEH = "Y";
            DevEW = "N";
            if (reportSettings.DevEW == true)
                DevEW = "Y";
            DevTons = "N";
            if (reportSettings.DevTons == true)
                DevTons = "Y";
            DevTonsOn = "N";
            if (reportSettings.DevTonsOn == true)
                DevTonsOn = "Y";
            DevTonsOff = "N";
            if (reportSettings.DevTonsOff == true)
                DevTonsOff = "Y";
            DevCmgt = "N";
            if (reportSettings.DevCmgt == true)
                DevCmgt = "Y";
            DevCmgtTotal = "N";
            if (reportSettings.DevCmgtTotal == true)
                DevCmgtTotal = "Y";
            DevGT = "N";
            if (reportSettings.DevGT == true)
                DevGT = "Y";
            DevGTTotal = "N";
            if (reportSettings.DevGTTotal == true)
                DevGTTotal = "Y";
            DevKG = "N";
            if (reportSettings.DevKG == true)
                DevKG = "Y";
            DevCubics = "N";
            if (reportSettings.DevCubics == true)
                DevCubics = "Y";
            DevCubGT = "N";
            if (reportSettings.DevCubGT == true)
                DevCubGT = "Y";
            DevCubTons = "N";
            if (reportSettings.DevCubTons == true)
                DevCubTons = "Y";
            DevCubKG = "N";
            if (reportSettings.DevCubKG == true)
                DevCubKG = "Y";
            DevLabour = "N";
            if (reportSettings.DevLabour == true)
                DevLabour = "Y";
            DevShftInfo = "N";
            if (reportSettings.DevShftInfo == true)
                DevShftInfo = "Y";
            DevDrillRig = "N";
            if (reportSettings.DevDrillRig == true)
                DevDrillRig = "Y";
        }

        private void createReport(Object theReportSettings)
        {




        }



        private void CheckForErrors()
        {
            bool theError = false;
            if (reportSettings.useMonthToMonth == true)
            {
                if (reportSettings.ProdmonthFrom > reportSettings.ProdmonthTo)
                {

                    _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "The From Month Selected is later than the To Month Selected", ButtonTypes.OK, MessageDisplayType.Small);
                    ErrFound = true;
                    theError = true;
                }
            }
            if (reportSettings.useMonthToMonth == true)
            {
                if (FoundNoSections == true)
                {

                    _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "No Sections Found for Months Selected", ButtonTypes.OK, MessageDisplayType.Small);
                    ErrFound = true;
                    theError = true;
                }
            }
            if (theError == false)
            {
                if (reportSettings.useDateToDate == true)
                {
                    if (((FPMonth == null) || (FPMonth == string.Empty)) &
                        ((TPMonth == null) || (TPMonth == string.Empty)))
                    {
                        _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "Could not Find Sections to Load for Date Selection", ButtonTypes.OK, MessageDisplayType.Small);
                        ErrFound = true;
                        theError = true;
                    }
                }
            }
            if (theError == false)
            {
                if (reportSettings.useDateToDate == true)
                {
                    if (reportSettings.DateFrom > reportSettings.DateTo)
                    {
                        _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "The From Date Selected is later than the To Date Selected", ButtonTypes.OK, MessageDisplayType.Small);
                        ErrFound = true;
                        theError = true;
                    }
                }
            }
            if (theError == false)
            {
                if ((theName == null) || (theName == string.Empty))
                {
                    _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "Please Select a Section", ButtonTypes.OK, MessageDisplayType.Small);
                    ErrFound = true;
                    theError = true;
                }
            }
            if (theError == false)
            {
                if ((reportSettings.PlanDyn == false) &
                    (reportSettings.PlanLock == false) &
                    (reportSettings.PlanDynProg == false) &
                    (reportSettings.PlanLockProg == false) &
                    (reportSettings.Book == false) &
                    (reportSettings.Meas == false) &
                    (reportSettings.PlanBuss == false) &
                    (reportSettings.FC == false) &
                    (reportSettings.AbsVar == false))
                {
                    _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "Please Select a Group", ButtonTypes.OK, MessageDisplayType.Small);
                    ErrFound = true;
                    theError = true;
                }
            }
            if (theError == false)
            {
                if ((reportSettings.LevGM == false) &
                    (reportSettings.LevMN == false) &
                    (reportSettings.LevMNM == false) &
                    (reportSettings.LevMO == false) &
                    (reportSettings.LevSB == false) &
                    (reportSettings.LevMiner == false) &
                    (reportSettings.LevWP == false))
                {
                    _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "Please Select Level of Summary", ButtonTypes.OK, MessageDisplayType.Small);
                    ErrFound = true;
                    theError = true;
                }
            }
            if (reportSettings.StopeLedge == null)
            {
                _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "Please Select an Activity", ButtonTypes.OK, MessageDisplayType.Small);
                ErrFound = true;
                theError = true;
            }
            if (theError == false)
            {
                if ((reportSettings.StpSqm == false) &
                    (reportSettings.StpSqmOn == false) &
                    (reportSettings.StpSqmOff == false) &
                    (reportSettings.StpSqmOS == false) &
                    (reportSettings.StpSqmOSF == false) &
                    (reportSettings.StpCmgt == false) &
                    (reportSettings.StpCmgtTotal == false) &
                    (reportSettings.StpGT == false) &
                    (reportSettings.StpGTTotal == false) &
                    (reportSettings.StpSW == false) &
                    (reportSettings.StpSWIdeal == false) &
                    (reportSettings.StpSWFault == false) &
                    (reportSettings.StpCW == false) &
                    (reportSettings.StpKG == false) &
                    (reportSettings.StpFL == false) &
                    (reportSettings.StpFLOn == false) &
                    (reportSettings.StpFLOff == false) &
                    (reportSettings.StpFLOS == false) &
                    (reportSettings.StpAdv == false) &
                    (reportSettings.StpAdvOn == false) &
                    (reportSettings.StpAdvOff == false) &
                    (reportSettings.StpTons == false) &
                    (reportSettings.StpTonsOn == false) &
                    (reportSettings.StpTonsOff == false) &
                    (reportSettings.StpTonsOS == false) &
                    (reportSettings.StpTonsFault == false) &
                    (reportSettings.StpMeasSweeps == false) &
                    (reportSettings.StpCubics == false) &
                    (reportSettings.StpCubGT == false) &
                    (reportSettings.StpCubTons == false) &
                    (reportSettings.StpCubKG == false) &
                    (reportSettings.StpLabour == false) &
                    (reportSettings.StpShftInfo == false) &
                    (reportSettings.DevAdv == false) &
                    (reportSettings.DevAdvOn == false) &
                    (reportSettings.DevAdvOff == false) &
                    (reportSettings.DevEH == false) &
                    (reportSettings.DevEW == false) &
                    (reportSettings.DevTons == false) &
                    (reportSettings.DevTonsOn == false) &
                    (reportSettings.DevTonsOff == false) &
                    (reportSettings.DevCmgt == false) &
                    (reportSettings.DevCmgtTotal == false) &
                    (reportSettings.DevGT == false) &
                    (reportSettings.DevGTTotal == false) &
                    (reportSettings.DevKG == false) &
                    (reportSettings.DevCubics == false) &
                    (reportSettings.DevCubGT == false) &
                    (reportSettings.DevCubTons == false) &
                    (reportSettings.DevCubKG == false) &
                    (reportSettings.DevLabour == false) &
                    (reportSettings.DevShftInfo == false) &
                    (reportSettings.DevDrillRig == false))
                {
                    _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "Please Select a Unit (Stopng or Development)", ButtonTypes.OK, MessageDisplayType.Small);
                    ErrFound = true;
                    theError = true;
                }
            }
        }
        private void Load_ExistReportNames()
        {
            try
            {
                _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                dtExistReports = _clsGenericReportData.get_ExistReports(UserCurrentInfo.UserID, string.Empty);

                if (dtExistReports.Rows.Count != 0)
                {
                    rpExistReports.DataSource = dtExistReports;
                    rpExistReports.DisplayMember = "ReportName";
                    rpExistReports.ValueMember = "ReportName";
                }

            }
            catch (Exception _ex)
            {
                throw new ApplicationException(_ex.Message, _ex);
            }
        }

        private void Load_ExistReportInfo()
        {
            try
            {
                _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                dtExistReportOptions = _clsGenericReportData.get_ExistReports(UserCurrentInfo.UserID, reportSettings.ExistReports);
                DataTable a = dtExistReportOptions;

                if (reportSettings.ExistReports != string.Empty)
                    reportSettings.SaveReport = reportSettings.ExistReports;
                rowSaveReport.Properties.Value = reportSettings.SaveReport;

                if (a.Rows.Count != 0)
                {
                    /*
					PMonth = a.Rows[0]["ProdMonth"].ToString();
					FPMonth = a.Rows[0]["FromProdMonth"].ToString();
					TPMonth = a.Rows[0]["ToProdMonth"].ToString();

					string aa = a.Rows[0]["ProdMonth"].ToString().Substring(0, 4) + "-" +
								a.Rows[0]["ProdMonth"].ToString().Substring(4, 2) + "-01";
					reportSettings.Prodmonth = Convert.ToDateTime(aa);
					aa = a.Rows[0]["FromProdMonth"].ToString().Substring(0, 4) + "-" +
						 a.Rows[0]["FromProdMonth"].ToString().Substring(4, 2) + "-01";
					reportSettings.ProdmonthFrom = Convert.ToDateTime(aa);
					aa = a.Rows[0]["ToProdMonth"].ToString().Substring(0, 4) + "-" +
						 a.Rows[0]["ToProdMonth"].ToString().Substring(4, 2) + "-01";
					reportSettings.ProdmonthTo = Convert.ToDateTime(aa);
					if (a.Rows[0]["CalendarDate"] == null)
					{
						reportSettings.CalendarDate = DateTime.Now;
						CDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
					}
					else
					{
						if (a.Rows[0]["CalendarDate"].ToString() == "")
						{
							reportSettings.CalendarDate = DateTime.Now;
							CDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
						}
						else
						{
							reportSettings.CalendarDate = Convert.ToDateTime(a.Rows[0]["CalendarDate"].ToString());
							CDate = String.Format("{0:yyyy-MM-dd}", a.Rows[0]["CalendarDate"].ToString());
						}
					}
					reportSettings.DateFrom = DateTime.Now;
					reportSettings.DateTo = DateTime.Now;

					//FDate = String.Format("{0:yyyy-MM-dd}", a.Rows[0]["FromDate"].ToString());
					//TDate = String.Format("{0:yyyy-MM-dd}", a.Rows[0]["ToDate"].ToString());

					if (a.Rows[0]["DateType"].ToString() == "P")
					{
						reportSettings.useProdmonth = true;
						reportSettings.useMonthToMonth = false;
						reportSettings.useDateToDate = false;
					}
					if (a.Rows[0]["DateType"].ToString() == "M")
					{
						reportSettings.useProdmonth = false;
						reportSettings.useMonthToMonth = true;
						reportSettings.useDateToDate = false;
					}
					if (a.Rows[0]["DateType"].ToString() == "D")
					{
						reportSettings.useProdmonth = false;
						reportSettings.useMonthToMonth = false;
						reportSettings.useDateToDate = true;
					}
					//rowProdmoth.Properties.Value = reportSettings.Prodmonth;
					//rowFromProdmonth.Properties.Value = reportSettings.ProdmonthFrom;
					//rowToProdmonth.Properties.Value = reportSettings.ProdmonthTo;

					//rowCalendarDate.Properties.Value = reportSettings.CalendarDate;
					//rowFromDate.Properties.Value = reportSettings.DateFrom;
					//rowToDate.Properties.Value = reportSettings.DateTo;

					rowProdmoth.Visible = reportSettings.useProdmonth;
					rowCalendarDate.Visible = reportSettings.useProdmonth;

					rowFromDate.Visible = reportSettings.useDateToDate;
					rowToDate.Visible = reportSettings.useDateToDate;

					rowFromProdmonth.Visible = reportSettings.useMonthToMonth;
					rowToProdmonth.Visible = reportSettings.useMonthToMonth;
					*/
                    LoadSections();
                    reportSettings.SectionID = a.Rows[0]["SectionID"].ToString();
                    rowSection.Properties.Value = reportSettings.SectionID;

                    LoadHierID();
                    set_TheLevels();

                    LoadActivity();

                    reportSettings.StopeLedge = a.Rows[0]["StopeLedge"].ToString();
                    rowStopeLedge.Properties.Value = reportSettings.StopeLedge;

                    if (a.Rows[0]["AuthPlanOnly"].ToString() == "Y")
                        reportSettings.AuthPlanOnly = true;
                    else
                        reportSettings.AuthPlanOnly = false;
                    rowAuthPlanOnly.Properties.Value = reportSettings.AuthPlanOnly;

                    if (a.Rows[0]["LevelGM"].ToString() == "Y")
                        reportSettings.LevGM = true;
                    else
                        reportSettings.LevGM = false;
                    if (a.Rows[0]["LevelMN"].ToString() == "Y")
                        reportSettings.LevMN = true;
                    else
                        reportSettings.LevMN = false;
                    if (a.Rows[0]["LevelMNM"].ToString() == "Y")
                        reportSettings.LevMNM = true;
                    else
                        reportSettings.LevMNM = false;
                    if (a.Rows[0]["LevelMO"].ToString() == "Y")
                        reportSettings.LevMO = true;
                    else
                        reportSettings.LevMO = false;
                    if (a.Rows[0]["LevelSB"].ToString() == "Y")
                        reportSettings.LevSB = true;
                    else
                        reportSettings.LevSB = false;
                    if (a.Rows[0]["LevelMiner"].ToString() == "Y")
                        reportSettings.LevMiner = true;
                    else
                        reportSettings.LevMiner = false;
                    if (a.Rows[0]["LevelWP"].ToString() == "Y")
                        reportSettings.LevWP = true;
                    else
                        reportSettings.LevWP = false;

                    rowGM.Properties.Value = reportSettings.LevGM;
                    rowMN.Properties.Value = reportSettings.LevMN;
                    rowMNM.Properties.Value = reportSettings.LevMNM;
                    rowMO.Properties.Value = reportSettings.LevMO;
                    rowSB.Properties.Value = reportSettings.LevSB;
                    rowMiner.Properties.Value = reportSettings.LevMiner;
                    rowWP.Properties.Value = reportSettings.LevWP;


                    if (reportSettings.useDateToDate == true)
                        Load_LevelVisibles(string.Empty);
                    else
                        Load_LevelVisibles("A");

                    if (a.Rows[0]["PlanDyn"].ToString() == "Y")
                        reportSettings.PlanDyn = true;
                    else
                        reportSettings.PlanDyn = false;
                    if (a.Rows[0]["PlanLock"].ToString() == "Y")
                        reportSettings.PlanLock = true;
                    else
                        reportSettings.PlanLock = false;
                    if (a.Rows[0]["PlanDynProg"].ToString() == "Y")
                        reportSettings.PlanDynProg = true;
                    else
                        reportSettings.PlanDynProg = false;
                    if (a.Rows[0]["PlanLockProg"].ToString() == "Y")
                        reportSettings.PlanLockProg = true;
                    else
                        reportSettings.PlanLockProg = false;
                    if (a.Rows[0]["Book"].ToString() == "Y")
                        reportSettings.Book = true;
                    else
                        reportSettings.Book = false;
                    if (a.Rows[0]["Meas"].ToString() == "Y")
                        reportSettings.Meas = true;
                    else
                        reportSettings.Meas = false;
                    if (a.Rows[0]["PlanBuss"].ToString() == "Y")
                        reportSettings.PlanBuss = true;
                    else
                        reportSettings.PlanBuss = false;
                    if (a.Rows[0]["FC"].ToString() == "Y")
                        reportSettings.FC = true;
                    else
                        reportSettings.FC = false;
                    if (a.Rows[0]["AbsVar"].ToString() == "Y")
                        reportSettings.AbsVar = true;
                    else
                        reportSettings.AbsVar = false;
                    rowPlanDyn.Properties.Value = reportSettings.PlanDyn;
                    rowPlanLock.Properties.Value = reportSettings.PlanLock;
                    rowPlanDynProg.Properties.Value = reportSettings.PlanDynProg;
                    rowPlanLockProg.Properties.Value = reportSettings.PlanLockProg;
                    rowBook.Properties.Value = reportSettings.Book;
                    rowMeas.Properties.Value = reportSettings.Meas;
                    rowPlanBuss.Properties.Value = reportSettings.PlanBuss;
                    rowFC.Properties.Value = reportSettings.FC;
                    rowAbsVar.Properties.Value = reportSettings.AbsVar;

                    if (a.Rows[0]["StpSqm"].ToString() == "Y")
                        reportSettings.StpSqm = true;
                    else
                        reportSettings.StpSqm = false;
                    if (a.Rows[0]["StpSqmOn"].ToString() == "Y")
                        reportSettings.StpSqmOn = true;
                    else
                        reportSettings.StpSqmOn = false;
                    if (a.Rows[0]["StpSqmOff"].ToString() == "Y")
                        reportSettings.StpSqmOff = true;
                    else
                        reportSettings.StpSqmOff = false;
                    if (a.Rows[0]["StpSqmOS"].ToString() == "Y")
                        reportSettings.StpSqmOS = true;
                    else
                        reportSettings.StpSqmOS = false;
                    if (a.Rows[0]["StpSqmOSF"].ToString() == "Y")
                        reportSettings.StpSqmOSF = true;
                    else
                        reportSettings.StpSqmOSF = false;

                    if (a.Rows[0]["StpCmgt"].ToString() == "Y")
                        reportSettings.StpCmgt = true;
                    else
                        reportSettings.StpCmgt = false;
                    if (a.Rows[0]["StpCmgtTot"].ToString() == "Y")
                        reportSettings.StpCmgtTotal = true;
                    else
                        reportSettings.StpCmgtTotal = false;
                    if (a.Rows[0]["StpGT"].ToString() == "Y")
                        reportSettings.StpGT = true;
                    else
                        reportSettings.StpGT = false;
                    if (a.Rows[0]["StpGTTot"].ToString() == "Y")
                        reportSettings.StpGTTotal = true;
                    else
                        reportSettings.StpGTTotal = false;
                    if (a.Rows[0]["StpSW"].ToString() == "Y")
                        reportSettings.StpSW = true;
                    else
                        reportSettings.StpSW = false;
                    if (a.Rows[0]["StpSWIdeal"].ToString() == "Y")
                        reportSettings.StpSWIdeal = true;
                    else
                        reportSettings.StpSWIdeal = false;
                    if (a.Rows[0]["StpSWFault"].ToString() == "Y")
                        reportSettings.StpSWFault = true;
                    else
                        reportSettings.StpSWFault = false;
                    if (a.Rows[0]["StpCW"].ToString() == "Y")
                        reportSettings.StpCW = true;
                    else
                        reportSettings.StpCW = false;
                    if (a.Rows[0]["StpKG"].ToString() == "Y")
                        reportSettings.StpKG = true;
                    else
                        reportSettings.StpKG = false;

                    if (a.Rows[0]["StpFL"].ToString() == "Y")
                        reportSettings.StpFL = true;
                    else
                        reportSettings.StpFL = false;
                    if (a.Rows[0]["StpFLOn"].ToString() == "Y")
                        reportSettings.StpFLOn = true;
                    else
                        reportSettings.StpFLOn = false;
                    if (a.Rows[0]["StpFLOff"].ToString() == "Y")
                        reportSettings.StpFLOff = true;
                    else
                        reportSettings.StpFLOff = false;
                    if (a.Rows[0]["StpFLOS"].ToString() == "Y")
                        reportSettings.StpFLOS = true;
                    else
                        reportSettings.StpFLOS = false;

                    if (a.Rows[0]["StpAdv"].ToString() == "Y")
                        reportSettings.StpAdv = true;
                    else
                        reportSettings.StpAdv = false;
                    if (a.Rows[0]["StpAdvOn"].ToString() == "Y")
                        reportSettings.StpAdvOn = true;
                    else
                        reportSettings.StpAdvOn = false;
                    if (a.Rows[0]["StpAdvOff"].ToString() == "Y")
                        reportSettings.StpAdvOff = true;
                    else
                        reportSettings.StpAdvOff = false;

                    if (a.Rows[0]["StpTons"].ToString() == "Y")
                        reportSettings.StpTons = true;
                    else
                        reportSettings.StpTons = false;
                    if (a.Rows[0]["StpTonsOn"].ToString() == "Y")
                        reportSettings.StpTonsOn = true;
                    else
                        reportSettings.StpTonsOn = false;
                    if (a.Rows[0]["StpTonsOff"].ToString() == "Y")
                        reportSettings.StpTonsOff = true;
                    else
                        reportSettings.StpTonsOff = false;
                    if (a.Rows[0]["StpTonsOS"].ToString() == "Y")
                        reportSettings.StpTonsOS = true;
                    else
                        reportSettings.StpTonsOS = false;
                    if (a.Rows[0]["StpTonsFault"].ToString() == "Y")
                        reportSettings.StpTonsFault = true;
                    else
                        reportSettings.StpTonsFault = false;

                    if (a.Rows[0]["StpCubics"].ToString() == "Y")
                        reportSettings.StpCubics = true;
                    else
                        reportSettings.StpCubics = false;
                    if (a.Rows[0]["StpCubGT"].ToString() == "Y")
                        reportSettings.StpCubGT = true;
                    else
                        reportSettings.StpCubGT = false;
                    if (a.Rows[0]["StpCubTons"].ToString() == "Y")
                        reportSettings.StpCubTons = true;
                    else
                        reportSettings.StpCubTons = false;
                    if (a.Rows[0]["StpCubKG"].ToString() == "Y")
                        reportSettings.StpCubKG = true;
                    else
                        reportSettings.StpCubKG = false;

                    if (a.Rows[0]["StpMeasSweeps"].ToString() == "Y")
                        reportSettings.StpMeasSweeps = true;
                    else
                        reportSettings.StpMeasSweeps = false;

                    if (a.Rows[0]["StpLabour"].ToString() == "Y")
                        reportSettings.StpLabour = true;
                    else
                        reportSettings.StpLabour = false;
                    if (a.Rows[0]["StpShftInfo"].ToString() == "Y")
                        reportSettings.StpShftInfo = true;
                    else
                        reportSettings.StpShftInfo = false;

                    if (a.Rows[0]["DevAdv"].ToString() == "Y")
                        reportSettings.DevAdv = true;
                    else
                        reportSettings.DevAdv = false;
                    if (a.Rows[0]["DevAdvOn"].ToString() == "Y")
                        reportSettings.DevAdvOn = true;
                    else
                        reportSettings.DevAdvOn = false;
                    if (a.Rows[0]["DevAdvOff"].ToString() == "Y")
                        reportSettings.DevAdvOff = true;
                    else
                        reportSettings.DevAdvOff = false;

                    if (a.Rows[0]["DevEH"].ToString() == "Y")
                        reportSettings.DevEH = true;
                    else
                        reportSettings.DevEH = false;
                    if (a.Rows[0]["DevEW"].ToString() == "Y")
                        reportSettings.DevEW = true;
                    else
                        reportSettings.DevEW = false;

                    if (a.Rows[0]["DevTons"].ToString() == "Y")
                        reportSettings.DevTons = true;
                    else
                        reportSettings.DevTons = false;
                    if (a.Rows[0]["DevTonsOn"].ToString() == "Y")
                        reportSettings.DevTonsOn = true;
                    else
                        reportSettings.DevTonsOn = false;
                    if (a.Rows[0]["DevTonsOff"].ToString() == "Y")
                        reportSettings.DevTonsOff = true;
                    else
                        reportSettings.DevTonsOff = false;
                    if (a.Rows[0]["DevCmgt"].ToString() == "Y")
                        reportSettings.DevCmgt = true;
                    else
                        reportSettings.DevCmgt = false;
                    if (a.Rows[0]["DevCmgtTot"].ToString() == "Y")
                        reportSettings.DevCmgtTotal = true;
                    else
                        reportSettings.DevCmgtTotal = false;
                    if (a.Rows[0]["DevGT"].ToString() == "Y")
                        reportSettings.DevGT = true;
                    else
                        reportSettings.DevGT = false;
                    if (a.Rows[0]["DevGTTot"].ToString() == "Y")
                        reportSettings.DevGTTotal = true;
                    else
                        reportSettings.DevGTTotal = false;

                    if (a.Rows[0]["DevKG"].ToString() == "Y")
                        reportSettings.DevKG = true;
                    else
                        reportSettings.DevKG = false;

                    if (a.Rows[0]["DevCubics"].ToString() == "Y")
                        reportSettings.DevCubics = true;
                    else
                        reportSettings.DevCubics = false;
                    if (a.Rows[0]["DevCubGT"].ToString() == "Y")
                        reportSettings.DevCubGT = true;
                    else
                        reportSettings.DevCubGT = false;
                    if (a.Rows[0]["DevCubTons"].ToString() == "Y")
                        reportSettings.DevCubTons = true;
                    else
                        reportSettings.DevCubTons = false;
                    if (a.Rows[0]["DevCubKG"].ToString() == "Y")
                        reportSettings.DevCubKG = true;
                    else
                        reportSettings.DevCubKG = false;

                    if (a.Rows[0]["DevLabour"].ToString() == "Y")
                        reportSettings.DevLabour = true;
                    else
                        reportSettings.DevLabour = false;
                    if (a.Rows[0]["DevShftInfo"].ToString() == "Y")
                        reportSettings.DevShftInfo = true;
                    else
                        reportSettings.DevShftInfo = false;
                    if (a.Rows[0]["DevDrillRig"].ToString() == "Y")
                        reportSettings.DevDrillRig = true;
                    else
                        reportSettings.DevDrillRig = false;

                    rowStpSqm.Properties.Value = reportSettings.StpSqm;
                    rowStpSqmOn.Properties.Value = reportSettings.StpSqmOn;
                    rowStpSqmOff.Properties.Value = reportSettings.StpSqmOff;
                    rowStpSqmOS.Properties.Value = reportSettings.StpSqmOS;
                    rowStpSqmOSF.Properties.Value = reportSettings.StpSqmOSF;
                    rowStpCmgt.Properties.Value = reportSettings.StpCmgt;
                    rowStpCmgtTotal.Properties.Value = reportSettings.StpCmgtTotal;
                    rowStpGT.Properties.Value = reportSettings.StpGT;
                    rowStpGTTotal.Properties.Value = reportSettings.StpGTTotal;
                    rowStpSW.Properties.Value = reportSettings.StpSW;
                    rowStpSWIdeal.Properties.Value = reportSettings.StpSWIdeal;
                    rowStpSWFault.Properties.Value = reportSettings.StpSWFault;
                    rowStpKG.Properties.Value = reportSettings.StpKG;
                    rowStpFL.Properties.Value = reportSettings.StpFL;
                    rowStpFLOn.Properties.Value = reportSettings.StpFLOn;
                    rowStpFLOff.Properties.Value = reportSettings.StpFLOff;
                    rowStpFLOS.Properties.Value = reportSettings.StpFLOS;
                    rowStpAdv.Properties.Value = reportSettings.StpAdv;
                    rowStpAdvOn.Properties.Value = reportSettings.StpAdvOn;
                    rowStpAdvOff.Properties.Value = reportSettings.StpAdvOff;
                    rowStpTons.Properties.Value = reportSettings.StpTons;
                    rowStpTonsOn.Properties.Value = reportSettings.StpTonsOn;
                    rowStpTonsOff.Properties.Value = reportSettings.StpTonsOff;
                    rowStpTonsOS.Properties.Value = reportSettings.StpTonsOS;
                    rowStpTonsFault.Properties.Value = reportSettings.StpTonsFault;
                    rowStpCubics.Properties.Value = reportSettings.StpCubics;
                    rowStpCubGT.Properties.Value = reportSettings.StpCubGT;
                    rowStpCubTons.Properties.Value = reportSettings.StpCubTons;
                    rowStpCubKG.Properties.Value = reportSettings.StpCubKG;
                    rowStpMeasSweeps.Properties.Value = reportSettings.StpMeasSweeps;
                    rowStpLabour.Properties.Value = reportSettings.StpLabour;
                    rowStpShftInfo.Properties.Value = reportSettings.StpShftInfo;

                    rowDevAdv.Properties.Value = reportSettings.DevAdv;
                    rowDevAdvOn.Properties.Value = reportSettings.DevAdvOn;
                    rowDevAdvOff.Properties.Value = reportSettings.DevAdvOff;
                    rowDevEH.Properties.Value = reportSettings.DevEH;
                    rowDevEW.Properties.Value = reportSettings.DevEW;
                    rowDevTons.Properties.Value = reportSettings.DevTons;
                    rowDevTonsOn.Properties.Value = reportSettings.DevTonsOn;
                    rowDevTonsOff.Properties.Value = reportSettings.DevTonsOff;
                    rowDevCmgt.Properties.Value = reportSettings.DevCmgt;
                    rowDevCmgtTotal.Properties.Value = reportSettings.DevCmgtTotal;
                    rowDevGT.Properties.Value = reportSettings.DevGT;
                    rowDevGTTotal.Properties.Value = reportSettings.DevGTTotal;
                    rowDevKG.Properties.Value = reportSettings.DevKG;
                    rowDevCubics.Properties.Value = reportSettings.DevCubics;
                    rowDevCubGT.Properties.Value = reportSettings.DevCubGT;
                    rowDevCubTons.Properties.Value = reportSettings.DevCubTons;
                    rowDevCubKG.Properties.Value = reportSettings.DevCubKG;
                    rowDevLabour.Properties.Value = reportSettings.DevLabour;
                    rowDevShftInfo.Properties.Value = reportSettings.DevShftInfo;
                    rowDevDrillRig.Properties.Value = reportSettings.DevDrillRig;


                    //if (reportSettings.PlanBuss == false)
                    //{
                    //    rowStpSqmOS.Visible = false;
                    //    rowStpSqmOSF.Visible = false;
                    //    rowStpFLOS.Visible = false;
                    //    rowStpSWFault.Visible = false;
                    //    rowStpTonsOS.Visible = false;
                    //    rowStpTonsFault.Visible = false;
                    //}
                    //else
                    //{
                    //    rowStpSqmOS.Visible = true;
                    //    rowStpSqmOSF.Visible = true;
                    //    rowStpFLOS.Visible = true;
                    //    rowStpSWFault.Visible = true;
                    //    rowStpTonsOS.Visible = true;
                    //    rowStpTonsFault.Visible = true;
                    //}
                    pgSettingsMain.SelectedObject = reportSettings;
                }

            }
            catch (Exception _ex)
            {
                throw new ApplicationException(_ex.Message, _ex);
            }
        }

        private void set_TheLevels()
        {
            rpLevGM.ReadOnly = false;
            rpLevMN.ReadOnly = false;
            rpLevMNM.ReadOnly = false;
            rpLevMO.ReadOnly = false;
            rpLevSB.ReadOnly = false;
            rpLevMiner.ReadOnly = false;
            rpLevWP.ReadOnly = false;

            rpLevGM.Enabled = true;
            rpLevMN.Enabled = true;
            rpLevMNM.Enabled = true;
            rpLevMO.Enabled = true;
            rpLevSB.Enabled = true;
            rpLevMiner.Enabled = true;
            rpLevWP.Enabled = true;

            reportSettings.LevGM = true;
            reportSettings.LevMN = true;
            reportSettings.LevMNM = true;
            reportSettings.LevMO = true;
            reportSettings.LevSB = true;
            reportSettings.LevMiner = true;
            reportSettings.LevWP = true;

            if (theHierID == "2")
            {
                reportSettings.LevGM = false;

                rpLevGM.ReadOnly = true;
                rpLevGM.Enabled = false;
            }
            if (theHierID == "3")
            {
                reportSettings.LevGM = false;
                reportSettings.LevMN = false;

                rpLevGM.ReadOnly = true;
                rpLevMN.ReadOnly = true;

                rpLevGM.Enabled = false;
                rpLevMN.Enabled = false;
            }
            if (theHierID == "4")
            {
                reportSettings.LevGM = false;
                reportSettings.LevMN = false;
                reportSettings.LevMNM = false;

                rpLevGM.ReadOnly = true;
                rpLevMN.ReadOnly = true;
                rpLevMNM.ReadOnly = true;

                rpLevGM.Enabled = false;
                rpLevMN.Enabled = false;
                rpLevMNM.Enabled = false;
            }
            if (theHierID == "5")
            {
                reportSettings.LevGM = false;
                reportSettings.LevMN = false;
                reportSettings.LevMNM = false;
                reportSettings.LevMO = false;

                rpLevGM.ReadOnly = true;
                rpLevMN.ReadOnly = true;
                rpLevMNM.ReadOnly = true;
                rpLevMO.ReadOnly = true;

                rpLevGM.Enabled = false;
                rpLevMN.Enabled = false;
                rpLevMNM.Enabled = false;
                rpLevMO.Enabled = false;
            }
            if (theHierID == "6")
            {
                reportSettings.LevGM = false;
                reportSettings.LevMN = false;
                reportSettings.LevMNM = false;
                reportSettings.LevMO = false;
                reportSettings.LevSB = false; ;

                rpLevGM.ReadOnly = true;
                rpLevMN.ReadOnly = true;
                rpLevMNM.ReadOnly = true;
                rpLevMO.ReadOnly = true;
                rpLevSB.ReadOnly = true;

                rpLevGM.Enabled = false;
                rpLevMN.Enabled = false;
                rpLevMNM.Enabled = false;
                rpLevMO.Enabled = false;
                rpLevSB.Enabled = false;
            }
        }
        private void GettheDay(DateTime GettheDay)
        {
            if (Convert.ToInt32(GettheDay.Month.ToString()) < 10)
            {
                getDay = GettheDay.Year.ToString() + "-0" + GettheDay.Month.ToString();
            }
            else
            {
                getDay = GettheDay.Year.ToString() + "-" + GettheDay.Month.ToString();
            }
            if (Convert.ToInt32(GettheDay.Day.ToString()) < 10)
            {
                getDay = getDay + "-0" + GettheDay.Day.ToString();
            }
            else
            {
                getDay = getDay + "-" + GettheDay.Day.ToString();
            }
        }
        private void GettheMonth(DateTime GettheMonth)
        {
            if (Convert.ToInt32(GettheMonth.Month.ToString()) < 10)
            {
                getMonth = GettheMonth.Year.ToString() + "0" + GettheMonth.Month.ToString();
            }
            else
            {
                getMonth = GettheMonth.Year.ToString() + GettheMonth.Month.ToString();
            }
        }

        private void rpSaveReport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            pgSettingsMain.PostEditor();
            ErrFound = false;
            CheckForErrors();
            if (ErrFound == false)
            {
                if ((reportSettings.SaveReport == null) || (reportSettings.SaveReport == string.Empty))
                {
                    _sysMessagesClass.viewMessage(MessageType.Info, "Generic Report", "Please enter a Name for the Report to be Saved", ButtonTypes.OK, MessageDisplayType.Small);
                    ErrFound = true;
                }
            }
            if (ErrFound == false)
            {
                get_Parametres();
                int thePMonth = 0;
                int theFPMonth = 0;
                int theTPMonth = 0;
                thePMonth = Convert.ToInt32(PMonth.ToString());
                theFPMonth = Convert.ToInt32(FPMonth.ToString());
                theTPMonth = Convert.ToInt32(TPMonth.ToString());
                try
                {
                    _clsGenericReportData.connectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _clsGenericReportData.saveReport(UserCurrentInfo.UserID, reportSettings.SaveReport,
                            DateType, thePMonth, theFPMonth, theTPMonth,
                            CDate, FDate, TDate,
                            reportSettings.SectionID, theHierID, theName,
                            LevGM, LevMN, LevMNM,
                            LevMO, LevSB, LevMiner, LevWP,
                            PlanDyn, PlanLock, PlanDynProg, PlanLockProg,
                            Book, Meas, PlanBuss, FC, AbsVar,
                            AuthPlanOnly, reportSettings.StopeLedge,
                            StpSqm, StpSqmOn, StpSqmOff, StpSqmOS, StpSqmOSF,
                            StpCmgt, StpCmgtTotal, StpGT, StpGTTotal,
                            StpSW, StpSWIdeal, StpSWFault,
                            StpCW, StpKG,
                            StpFL, StpFLOn, StpFLOff, StpFLOS,
                            StpAdv, StpAdvOn, StpAdvOff,
                            StpTons, StpTonsOn, StpTonsOff, StpTonsOS, StpTonsFault,
                            StpCubics, StpCubTons, StpCubGT, StpCubKG,
                            StpMeasSweeps, StpLabour, StpShftInfo,
                            DevAdv, DevAdvOn, DevAdvOff,
                            DevEH, DevEW,
                            DevTons, DevTonsOn, DevTonsOff,
                            DevCmgt, DevCmgtTotal, DevGT, DevGTTotal,
                            DevKG,
                            DevCubics, DevCubTons, DevCubGT, DevCubKG,
                            DevLabour, DevShftInfo, DevDrillRig);

                    Load_ExistReportNames();
                    //reportSettings.ExistReportName = reportSettings.SaveReportName;
                    //rowExistReportName.Properties.Value = reportSettings.ExistReportName;

                    //reportSettings.SaveReportName = "";

                    //rowSaveReportName.Properties.Value = reportSettings.SaveReportName;

                    //Load_ExistReportInfo();


                }
                catch (Exception _ex)
                {
                    throw new ApplicationException(_ex.Message, _ex);
                }
            }
        }
        private void rpSection_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();

            LoadSections();
            LoadHierID();

            set_TheLevels();
            rowGM.Properties.Value = reportSettings.LevGM;
            rowMN.Properties.Value = reportSettings.LevMN;
            rowMNM.Properties.Value = reportSettings.LevMNM;
            rowMO.Properties.Value = reportSettings.LevMO;
            rowSB.Properties.Value = reportSettings.LevSB;
            rowMiner.Properties.Value = reportSettings.LevMiner;
            rowWP.Properties.Value = reportSettings.LevWP;

            pgSettingsMain.SelectedObject = reportSettings;
        }
        private void rpExistReports_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
            Load_ExistReportInfo();
            pgSettingsMain.PostEditor();
        }
        private void rpSaveReport_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
        }
        private void editProdmonth_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
            GettheMonth(reportSettings.Prodmonth);
            PMonth = getMonth;
            LoadSections();
        }
        private void editToMonth_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
            GettheMonth(reportSettings.ProdmonthTo);
            TPMonth = getMonth;
            LoadSections();
        }
        private void editFromMonth_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
            GettheMonth(reportSettings.ProdmonthFrom);
            FPMonth = getMonth;
            LoadSections();
        }
        private void dteFromDate_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
            GettheDay(reportSettings.DateFrom);
            FDate = getDay;
            LoadSections();
        }

        private void dteToDate_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
            GettheDay(reportSettings.DateTo);
            TDate = getDay;
            LoadSections();
        }

        private void dteCalendarDate_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
            GettheDay(reportSettings.CalendarDate);
            CDate = getDay;
        }
        private void cbByProdmonth_EditValueChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
            setScreenSelection();
        }

        private void rpPlanBuss_CheckStateChanged(object sender, EventArgs e)
        {
            pgSettingsMain.PostEditor();
            Load_BussPlanUnits();
        }
        private void Load_BussPlanUnits()
        {
            reportSettings.StpSqmOS = false;
            reportSettings.StpSqmOSF = false;
            reportSettings.StpFLOS = false;
            reportSettings.StpSWFault = false;
            reportSettings.StpTonsOS = false;
            reportSettings.StpTonsFault = false;

            rowStpSqmOS.Properties.Value = reportSettings.StpSqmOS;
            rowStpSqmOSF.Properties.Value = reportSettings.StpSqmOSF;
            rowStpFLOS.Properties.Value = reportSettings.StpFLOS;
            rowStpSWFault.Properties.Value = reportSettings.StpSWFault;
            rowStpTonsOS.Properties.Value = reportSettings.StpTonsOS;
            rowStpTonsFault.Properties.Value = reportSettings.StpTonsFault;

            if (reportSettings.PlanBuss == false)
            {
                rowStpSqmOS.Visible = false;
                rowStpSqmOSF.Visible = false;
                rowStpFLOS.Visible = false;
                rowStpSWFault.Visible = false;
                rowStpTonsOS.Visible = false;
                rowStpTonsFault.Visible = false;
            }
            else
            {
                rowStpSqmOS.Visible = true;
                rowStpSqmOSF.Visible = true;
                rowStpFLOS.Visible = true;
                rowStpSWFault.Visible = true;
                rowStpTonsOS.Visible = true;
                rowStpTonsFault.Visible = true;
            }
        }
        private void Load_LevelVisibles(string _type)
        {
            reportSettings.PlanDyn = false;
            reportSettings.PlanLock = false;
            reportSettings.Meas = false;
            reportSettings.PlanBuss = false;

            rowPlanDyn.Properties.Value = reportSettings.PlanDyn;
            rowPlanLock.Properties.Value = reportSettings.PlanLock;
            rowMeas.Properties.Value = reportSettings.Meas;
            rowPlanBuss.Properties.Value = reportSettings.PlanBuss;

            if (_type == "A")
            {
                rowPlanDyn.Visible = true;
                //rowPlanLock.Visible = true;
                rowPlanLock.Visible = false;
                rowPlanLockProg.Visible = false;
                rowMeas.Visible = true;
                //rowPlanBuss.Visible = true;
                rowPlanBuss.Visible = false;

            }
            else
            {
                rowPlanDyn.Visible = false;
                rowPlanLock.Visible = false;
                rowMeas.Visible = false;
                rowPlanBuss.Visible = false;
            }
            Load_BussPlanUnits();
        }
    }

    internal class ListItem
    {
        public bool Selected { get; internal set; }
    }
}