using System;
using System.Configuration;
using System.Data;

namespace Mineware.Systems.Production.Reporting.GenericReport
{
    public class clsGenericReportSettings
    {
        public string _DBTag;
        public string DBTag { get { return _DBTag; } set { _DBTag = value; } }
        public string _UserCurrentInfo;
        public string UserCurrentInfo { get { return _UserCurrentInfo; } set { _UserCurrentInfo = value; } }

        private string theMonth;
        private string theHierID;
        //public string DBTag = "";

        public string _SaveReport;
        public string SaveReport { get { return _SaveReport; } set { _SaveReport = value; } }
        public string _ExistReports;
        public string ExistReports { get { return _ExistReports; } set { _ExistReports = value; } }

        private bool _useProdmonth;
        private bool _useMonthToMonth;
        private bool _useDateToDate;
        private DateTime _Prodmonth;
        private DateTime _ProdmonthFrom;
        private DateTime _ProdmonthTo;
        private DateTime _CalendarDate;
        private DateTime _DateFrom;
        private DateTime _DateTo;

        private string _SectionID;
        private string _LevelID;

        private bool _AuthPlanOnly;
        private string _StopeLedge;

        private bool _LevGM;
        private bool _LevMN;
        private bool _LevMNM;
        private bool _LevMO;
        private bool _LevSB;
        private bool _LevMiner;
        private bool _LevWP;

        private bool _PlanDyn;
        private bool _PlanLock;
        private bool _PlanDynProg;
        private bool _PlanLockProg;
        private bool _Book;
        private bool _Meas;
        private bool _PlanBuss;
        private bool _FC;
        private bool _AbsVar;

        private bool _StpSqm;
        private bool _StpSqmOn;
        private bool _StpSqmOff;
        private bool _StpSqmOS;
        private bool _StpSqmOSF;
        private bool _StpCmgt;
        private bool _StpCmgtTotal;
        private bool _StpGT;
        private bool _StpGTTotal;
        private bool _StpSW;
        private bool _StpSWIdeal;
        private bool _StpSWFault;
        private bool _StpCW;
        private bool _StpKG;
        private bool _StpFL;
        private bool _StpFLOn;
        private bool _StpFLOff;
        private bool _StpFLOS;
        private bool _StpAdv;
        private bool _StpAdvOn;
        private bool _StpAdvOff;
        private bool _StpTons;
        private bool _StpTonsOn;
        private bool _StpTonsOff;
        private bool _StpTonsOS;
        private bool _StpTonsFault;
        private bool _StpCubics;
        private bool _StpCubGT;
        private bool _StpCubTons;
        private bool _StpCubKG;
        private bool _StpMeasSweeps;
        private bool _StpLabour;
        private bool _StpShftInfo;

        private bool _DevAdv;
        private bool _DevAdvOn;
        private bool _DevAdvOff;
        private bool _DevEH;
        private bool _DevEW;
        private bool _DevTons;
        private bool _DevTonsOn;
        private bool _DevTonsOff;
        private bool _DevCmgt;
        private bool _DevCmgtTotal;
        private bool _DevGT;
        private bool _DevGTTotal;
        private bool _DevKG;
        private bool _DevCubics;
        private bool _DevCubTons;
        private bool _DevCubGT;
        private bool _DevCubKG;
        private bool _DevLabour;
        private bool _DevShftInfo;
        private bool _DevDrillRig;

        public bool useProdmonth
        {
            get
            {
                return _useProdmonth;
            }
            set
            {
                if (_useProdmonth != true && value == true)
                {
                    _useProdmonth = value;
                    _useMonthToMonth = false;
                    _useDateToDate = false;
                }
            }
        }

        public bool useMonthToMonth
        {
            get
            {
                return _useMonthToMonth;
            }
            set
            {
                if (_useMonthToMonth != true && value == true)
                {
                    _useMonthToMonth = value;
                    _useProdmonth = false;
                    _useDateToDate = false;
                }
            }
        }

        public bool useDateToDate
        {
            get
            {
                return _useDateToDate;
            }
            set
            {
                if (_useDateToDate != true && value == true)
                {
                    _useDateToDate = value;
                    _useProdmonth = false;
                    _useMonthToMonth = false;
                }
            }
        }

        private bool _CopyLayout;
        public bool CopyLayout
        {
            get
            {
                return _CopyLayout;
            }
            set
            {
                _CopyLayout = value;
            }
        }


        public DateTime Prodmonth
        {
            get
            {
                return _Prodmonth;
            }
            set
            {
                _Prodmonth = value;
            }
        }

        public DateTime ProdmonthFrom { get { return _ProdmonthFrom; } set { _ProdmonthFrom = value; } }
        public DateTime ProdmonthTo { get { return _ProdmonthTo; } set { _ProdmonthTo = value; } }

        public DateTime DateFrom
        {
            get
            {
                return _DateFrom;
            }
            set
            {
                _DateFrom = value;
            }
        }

        public DateTime DateTo { get { return _DateTo; } set { _DateTo = value; } }
        public DateTime CalendarDate { get { return _CalendarDate; } set { _CalendarDate = value; } }

        public string SectionID
        {
            get
            {
                return _SectionID;
            }
            set
            {
                _SectionID = value;

                if (Convert.ToInt32(_Prodmonth.Month.ToString()) < 10)
                {
                    theMonth = _Prodmonth.Year.ToString() + "0" + _Prodmonth.Month.ToString();
                }
                else
                {
                    theMonth = _Prodmonth.Year.ToString() + _Prodmonth.Month.ToString();
                }

                try
                {
                    MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                    _dbMan.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _dbMan.SqlStatement = " select HierarchicalID, Name from tbl_Section where " +
                                            " ProdMonth = '" + theMonth + "' " +
                                            " and sectionID  = '" + _SectionID + "' ";
                    _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan.ExecuteInstruction();
                    DataTable y = _dbMan.ResultsDataTable;

                    if (y.Rows.Count != 0)
                    {
                        theHierID = y.Rows[0]["HierarchicalID"].ToString();
                    }
                }
                catch (Exception _ex)
                {
                    throw new ApplicationException(_ex.Message, _ex);
                }

                _LevGM = true;
                _LevMN = true;
                _LevMNM = true;
                _LevMO = true;
                _LevSB = true;
                _LevWP = true;

                if (theHierID == "2")
                {
                    _LevGM = false;
                }
                if (theHierID == "3")
                {
                    _LevGM = false;
                    _LevMN = false;
                }
                if (theHierID == "4")
                {
                    _LevGM = false;
                    _LevMN = false;
                    _LevMNM = false;
                }
                if (theHierID == "5")
                {
                    _LevGM = false;
                    _LevMN = false;
                    _LevMNM = false;
                    _LevMO = false;
                }
                if (theHierID == "6")
                {
                    _LevGM = false;
                    _LevMN = false;
                    _LevMNM = false;
                    _LevMO = false;
                    _LevSB = false;
                }
            }
        }




        public string Level { get { return _LevelID; } set { _LevelID = value; } }

        public bool AuthPlanOnly { get { return _AuthPlanOnly; } set { _AuthPlanOnly = value; } }
        public string StopeLedge { get { return _StopeLedge; } set { _StopeLedge = value; } }

        public bool LevGM { get { return _LevGM; } set { _LevGM = value; } }
        public bool LevMN { get { return _LevMN; } set { _LevMN = value; } }
        public bool LevMNM { get { return _LevMNM; } set { _LevMNM = value; } }
        public bool LevMO { get { return _LevMO; } set { _LevMO = value; } }
        public bool LevSB { get { return _LevSB; } set { _LevSB = value; } }
        public bool LevMiner { get { return _LevMiner; } set { _LevMiner = value; } }
        public bool LevWP { get { return _LevWP; } set { _LevWP = value; } }

        public bool PlanDyn { get { return _PlanDyn; } set { _PlanDyn = value; } }
        public bool PlanLock { get { return _PlanLock; } set { _PlanLock = value; } }
        public bool PlanDynProg { get { return _PlanDynProg; } set { _PlanDynProg = value; } }
        public bool PlanLockProg { get { return _PlanLockProg; } set { _PlanLockProg = value; } }
        public bool Book { get { return _Book; } set { _Book = value; } }
        public bool Meas { get { return _Meas; } set { _Meas = value; } }
        public bool PlanBuss { get { return _PlanBuss; } set { _PlanBuss = value; } }
        public bool FC { get { return _FC; } set { _FC = value; } }
        public bool AbsVar { get { return _AbsVar; } set { _AbsVar = value; } }

        public bool StpSqm { get { return _StpSqm; } set { _StpSqm = value; } }
        public bool StpSqmOn { get { return _StpSqmOn; } set { _StpSqmOn = value; } }
        public bool StpSqmOff { get { return _StpSqmOff; } set { _StpSqmOff = value; } }
        public bool StpSqmOS { get { return _StpSqmOS; } set { _StpSqmOS = value; } }
        public bool StpSqmOSF { get { return _StpSqmOSF; } set { _StpSqmOSF = value; } }
        public bool StpCmgt { get { return _StpCmgt; } set { _StpCmgt = value; } }
        public bool StpCmgtTotal { get { return _StpCmgtTotal; } set { _StpCmgtTotal = value; } }
        public bool StpGT { get { return _StpGT; } set { _StpGT = value; } }
        public bool StpGTTotal { get { return _StpGTTotal; } set { _StpGTTotal = value; } }
        public bool StpSW { get { return _StpSW; } set { _StpSW = value; } }
        public bool StpSWIdeal { get { return _StpSWIdeal; } set { _StpSWIdeal = value; } }
        public bool StpSWFault { get { return _StpSWFault; } set { _StpSWFault = value; } }
        public bool StpCW { get { return _StpCW; } set { _StpCW = value; } }
        public bool StpKG { get { return _StpKG; } set { _StpKG = value; } }
        public bool StpFL { get { return _StpFL; } set { _StpFL = value; } }
        public bool StpFLOn { get { return _StpFLOn; } set { _StpFLOn = value; } }
        public bool StpFLOff { get { return _StpFLOff; } set { _StpFLOff = value; } }
        public bool StpFLOS { get { return _StpFLOS; } set { _StpFLOS = value; } }
        public bool StpAdv { get { return _StpAdv; } set { _StpAdv = value; } }
        public bool StpAdvOn { get { return _StpAdvOn; } set { _StpAdvOn = value; } }
        public bool StpAdvOff { get { return _StpAdvOff; } set { _StpAdvOff = value; } }
        public bool StpTons { get { return _StpTons; } set { _StpTons = value; } }
        public bool StpTonsOn { get { return _StpTonsOn; } set { _StpTonsOn = value; } }
        public bool StpTonsOff { get { return _StpTonsOff; } set { _StpTonsOff = value; } }
        public bool StpTonsOS { get { return _StpTonsOS; } set { _StpTonsOS = value; } }
        public bool StpTonsFault { get { return _StpTonsFault; } set { _StpTonsFault = value; } }
        public bool StpCubics { get { return _StpCubics; } set { _StpCubics = value; } }
        public bool StpCubGT { get { return _StpCubGT; } set { _StpCubGT = value; } }
        public bool StpCubTons { get { return _StpCubTons; } set { _StpCubTons = value; } }
        public bool StpCubKG { get { return _StpCubKG; } set { _StpCubKG = value; } }
        public bool StpMeasSweeps { get { return _StpMeasSweeps; } set { _StpMeasSweeps = value; } }
        public bool StpLabour { get { return _StpLabour; } set { _StpLabour = value; } }
        public bool StpShftInfo { get { return _StpShftInfo; } set { _StpShftInfo = value; } }
        public bool DevAdv { get { return _DevAdv; } set { _DevAdv = value; } }
        public bool DevAdvOn { get { return _DevAdvOn; } set { _DevAdvOn = value; } }
        public bool DevAdvOff { get { return _DevAdvOff; } set { _DevAdvOff = value; } }
        public bool DevEH { get { return _DevEH; } set { _DevEH = value; } }
        public bool DevEW { get { return _DevEW; } set { _DevEW = value; } }
        public bool DevTons { get { return _DevTons; } set { _DevTons = value; } }
        public bool DevTonsOn { get { return _DevTonsOn; } set { _DevTonsOn = value; } }
        public bool DevTonsOff { get { return _DevTonsOff; } set { _DevTonsOff = value; } }
        public bool DevCmgt { get { return _DevCmgt; } set { _DevCmgt = value; } }
        public bool DevCmgtTotal { get { return _DevCmgtTotal; } set { _DevCmgtTotal = value; } }
        public bool DevGT { get { return _DevGT; } set { _DevGT = value; } }
        public bool DevGTTotal { get { return _DevGTTotal; } set { _DevGTTotal = value; } }
        public bool DevKG { get { return _DevKG; } set { _DevKG = value; } }
        public bool DevCubics { get { return _DevCubics; } set { _DevCubics = value; } }
        public bool DevCubGT { get { return _DevCubGT; } set { _DevCubGT = value; } }
        public bool DevCubTons { get { return _DevCubTons; } set { _DevCubTons = value; } }
        public bool DevCubKG { get { return _DevCubKG; } set { _DevCubKG = value; } }
        public bool DevLabour { get { return _DevLabour; } set { _DevLabour = value; } }
        public bool DevShftInfo { get { return _DevShftInfo; } set { _DevShftInfo = value; } }
        public bool DevDrillRig { get { return _DevDrillRig; } set { _DevDrillRig = value; } }
    }
}
