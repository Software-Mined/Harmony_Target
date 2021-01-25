using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Mineware.Systems.Global.ReportsControls;
using Mineware.Systems.Global.sysMessages;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.ProductionGlobal;
using System.Drawing;
using Mineware.Systems.GlobalConnect;
using System.Configuration;

namespace Mineware.Systems.Production.Reporting.PlanningNewStyle
{
    class PlanningReportNewStyleProperties : BaseClass
    {
        private DateTime _Prodmonth;
        public DateTime Prodmonth
        {
            get { return _Prodmonth; }
            set {
                _Prodmonth = value;
                UpdateSumOnArg e = new UpdateSumOnArg();
                OnUpdateSumOnRequest(e);
                UpdateMonthArg ee = new UpdateMonthArg();
                OnUpdateMonthRequest(this,ee);
            }
        }

        private string _HierarchicalID1;
        public string HierarchicalID1 { get { return _HierarchicalID1; } set { _HierarchicalID1 = value; } }

        private string _section1;
        public string section1 { get { return _section1; } set { _section1 = value; } }

        private Boolean _ShowAuth;
        public Boolean ShowAuth { get { return _ShowAuth; } set { _ShowAuth = value; } }

        private string _ActivityType;
        public string ActivityType
        {
            get
            {
                return _ActivityType;
            }
            set
            {
                _ActivityType = value;
                bool Stope = false;
                bool Dev = false;
                bool Sundry = false;
                bool SweepVamp = false;
                if (_ActivityType == "0") { Stope = true; Dev = false; } else { Stope = false; Dev = true; }
                switch (Convert.ToInt16(_ActivityType))
                {
                    case 0:
                        Stope = true;
                        Dev = false;
                        Sundry = false;
                        SweepVamp = false;
                        break;
                    case 1:
                        Stope = false;
                        Dev = true;
                        Sundry = false;
                        SweepVamp = false;
                        break;
                    default:
                        Stope = false;
                        Dev = false;
                        Sundry = true;
                        SweepVamp = true;
                        break;
                }
                UpdateActivitySelectionArg e = new UpdateActivitySelectionArg(Stope, Dev, Sundry, SweepVamp); OnUpdateActivitySelection(e);
            }
        }

        public string pmonth;

        //private DataTable _SumOn;
        //public DataTable SumOn
        //{
        //    get
        //    {
        //        _SumOn = loadSumOn();
        //        return _SumOn;
        //    }

        //}

        public DataTable LoadAllLevel()
        {
            MWDataManager.clsDataAccess Bus_Logic = new MWDataManager.clsDataAccess();
            Bus_Logic.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];            
            Bus_Logic.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            Bus_Logic.queryReturnType = MWDataManager.ReturnType.DataTable;
            Bus_Logic.SqlStatement = "select HierarchicalID from tbl_section where prodmonth = '" + Prodmonth.ToString("yyyyMM") + "' and SectionID = '" + _NAME + "'";
            Bus_Logic.ExecuteInstruction();

            if (Bus_Logic.ResultsDataTable.Rows.Count > 0)
            {
                int _hierID = Convert.ToInt32(Bus_Logic.ResultsDataTable.Rows[0]["HierarchicalID"]);

                Bus_Logic.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                Bus_Logic.queryReturnType = MWDataManager.ReturnType.DataTable;


                //if (Thelevel == null)
                //{
                //    Thelevel = "0";
                //}
                Bus_Logic.SqlStatement = "select distinct HierarchicalID,  " +
                                        "Description = (select  " +
                                        "case when Hierarchicalid = 1 then 'Business Coach' " +
                                        "when Hierarchicalid = 2 then 'Mine Manager'  " +
                                        "when Hierarchicalid = 3 then 'Mining Manager' " +
                                        "when Hierarchicalid = 4 then 'Mine Overseer' " +
                                        "when Hierarchicalid = 5 then 'Coach' " +
                                        "when Hierarchicalid = 6 then 'Miner' " +
                                        "end) from tbl_Section where HierarchicalID > " + _hierID + " and  " +
                                        "ProdMonth = '" + Prodmonth.ToString("yyyyMM") + "'  " +
                                        "order by HierarchicalID";
                Bus_Logic.ExecuteInstruction();
            }

            //else
                //Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Error", "There's no data for your selection", Color.Red);

            return Bus_Logic.ResultsDataTable;
        }

        //public DataTable loadSumOn()
        //{
        //    if (_NAME != null && _Prodmonth != null)
        //    {
        //        MWDataManager.clsDataAccess Bus_Logic = new MWDataManager.clsDataAccess();
        //        Bus_Logic.ConnectionString = TConnections.GetConnectionString(systemDBTag, UserCurrentInfo.Connection);
        //        Bus_Logic.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

        //        Bus_Logic.queryReturnType = MWDataManager.ReturnType.DataTable;

        //        Bus_Logic.SqlStatement = "select sectionid from section where prodmonth = '" + Prodmonth.ToString("yyyyMM") + "' and name= '" + _NAME + "'";

        //        Bus_Logic.ExecuteInstruction();

        //        DataTable dt = new DataTable();
        //        dt = Bus_Logic.ResultsDataTable;


        //        foreach (DataRow ds in dt.Rows)
        //        {
        //            _section1 = ds["sectionid"].ToString();
        //        }

        //        Bus_Logic.SqlStatement = "Select HierarchicalID from Section where SectionID = '" + _section1 + "' AND ProdMonth = '" + Prodmonth.ToString("yyyyMM") + "'";
        //        Bus_Logic.ExecuteInstruction();

        //        DataTable dt1 = new DataTable();
        //        dt1 = Bus_Logic.ResultsDataTable;

        //        foreach (DataRow dr in dt1.Rows)
        //        {
        //            HierarchicalID = Convert.ToInt32(dr["HierarchicalID"]);
        //        }

        //        DataTable dtLevels = LoadAllLevel(HierarchicalID);

        //        if (dtLevels != null)
        //        {
        //            return Bus_Logic.ResultsDataTable;
        //        }

        //        else { return null; }
        //    }
        //    else { return null; }
        //}

        public event UpdateMonth OnUpdateMonthRequest;
        protected void UpdateMonthRequest(UpdateMonthArg ee)
        {
            if (OnUpdateMonthRequest != null)
            {
                OnUpdateMonthRequest(this, ee);
            }
        }
        public delegate void UpdateMonth(object sender, UpdateMonthArg ee);

        public class UpdateMonthArg : EventArgs
        {

        }

        public event UpdateSumOn UpdateSumOnRequest;
        protected void OnUpdateSumOnRequest(UpdateSumOnArg e)
        {
            if (UpdateSumOnRequest != null)
            {
                UpdateSumOnRequest(this,e);
            }
        }
        public delegate void UpdateSumOn(object sender, UpdateSumOnArg e);

        public class UpdateSumOnArg : EventArgs
        {

        }

        public event UpdateActivitySelection UpdateActivitySelectionRequest;
        protected void OnUpdateActivitySelection(UpdateActivitySelectionArg e)
        {
            if (UpdateActivitySelectionRequest != null)
            {
                UpdateActivitySelectionRequest(this, e);
            }
        }

        public delegate void UpdateActivitySelection(object sender, UpdateActivitySelectionArg e);
        public class UpdateActivitySelectionArg : EventArgs
        {

            public UpdateActivitySelectionArg(bool stoping, bool dev, bool sundry, bool sweepvamp)
            {
                Stoping = stoping;
                Dev = dev;
                Sundry = sundry;
                Sweepvamp = sweepvamp;
            }

            public bool Stoping { get; set; }
            public bool Dev { get; set; }
            public bool Sundry { get; set; }
            public bool Sweepvamp { get; set; }
        }   

        private string _PlanType;
        public string PlanType { get { return _PlanType; } set { _PlanType = value; } }   

        private bool _Tons;
        public bool Tons { get { return _Tons; } set { _Tons = value; } }

        private string  _SectionID;
        public string SectionID { get { return _SectionID; } set { _SectionID = value;  } }

        private string _NAME;
        public string NAME { get { return _NAME; } set { _NAME = value; UpdateSumOnArg e = new UpdateSumOnArg(); OnUpdateSumOnRequest(e); } }

        private int _HierarchicalID;
        public int HierarchicalID { get { return _HierarchicalID; } set { _HierarchicalID = value; } }

        private bool _ProdSupervisor;
        public bool ProdSupervisor { get { return _ProdSupervisor; } set { _ProdSupervisor = value; } }

        private bool _Miner;
        public bool Miner { get { return _Miner; } set { _Miner = value; } }

        private bool _Workplace;
        public bool Workplace { get { return _Workplace; } set { _Workplace = value; } }

        private bool _Workplaceid;
        public bool Workplaceid { get { return _Workplaceid; } set { _Workplaceid = value; } }

        private bool _Stopped;
        public bool Stopped { get { return _Stopped; } set { _Stopped = value; } }

        private bool _DayCrew;
        public bool DayCrew { get { return _DayCrew; } set { _DayCrew = value; } }

        private bool _AfternoonCrew;
        public bool AfternoonCrew { get { return _AfternoonCrew; } set { _AfternoonCrew = value; } }

        private bool _NightCrew;
        public bool NightCrew { get { return _NightCrew; } set { _NightCrew = value; } }

        private bool _RovingCrew;
        public bool RovingCrew { get { return _RovingCrew; } set { _RovingCrew = value; } }

        private bool _CrewStrength;
        public bool CrewStrength { get { return _CrewStrength; } set { _CrewStrength = value; } }

        private bool _MiningMethod;
        public bool MiningMethod { get { return _MiningMethod; } set { _MiningMethod = value; } }

        private bool _SQM;
        public bool SQM { get { return _SQM; } set { _SQM = value; } }

        private bool _OnReefSqm;
        public bool OnReefSqm { get { return _OnReefSqm; } set { _OnReefSqm = value; } }

        private bool _OffReefSqm;
        public bool OffReefSqm { get { return _OffReefSqm; } set { _OffReefSqm = value; } }

        private bool _TargetSqm;
        public bool TargetSqm { get { return _TargetSqm; } set { _TargetSqm = value; } }

        private bool _Facelength;
        public bool Facelength { get { return _Facelength; } set { _Facelength = value; } }

        private bool _OnReefFL;
        public bool OnReefFL { get { return _OnReefFL; } set { _OnReefFL = value; } }

        private bool _OffReefFL;
        public bool OffReefFL { get { return _OffReefFL; } set { _OffReefFL = value; } }

        private bool _Advance;
        public bool Advance { get { return _Advance; } set { _Advance = value; } }

        private bool _OnReefAdvance;
        public bool OnReefAdvance { get { return _OnReefAdvance; } set { _OnReefAdvance = value; } }

        private bool _OffReefAdvance;
        public bool OffReefAdvance { get { return _OffReefAdvance; } set { _OffReefAdvance = value; } }

        private bool _ChannelWidth;
        public bool ChannelWidth { get { return _ChannelWidth; } set { _ChannelWidth = value; } }

        private bool _ChannelWidthTons;
        public bool ChannelWidthTons { get { return _ChannelWidthTons; } set { _ChannelWidthTons = value; } }

        private bool _StopeWidth;
        public bool StopeWidth { get { return _StopeWidth; } set { _StopeWidth = value; } }

        private bool _IdealStopeWidth;
        public bool IdealStopeWidth { get { return _IdealStopeWidth; } set { _IdealStopeWidth = value; } }

        private bool _OnReefTons;
        public bool OnReefTons { get { return _OnReefTons; } set { _OnReefTons = value; } }

        private bool _OffReefTons;
        public bool OffReefTons { get { return _OffReefTons; } set { _OffReefTons = value; } }

        private bool _gt;
        public bool gt { get { return _gt; } set { _gt = value; } }

        private bool _cmgt;
        public bool cmgt { get { return _cmgt; } set { _cmgt = value; } }

        private bool _KG;
        public bool KG { get { return _KG; } set { _KG = value; } }

        private bool _CubicMetres;
        public bool CubicMetres { get { return _CubicMetres; } set { _CubicMetres = value; } }

        private bool _CubicTons;
        public bool CubicTons { get { return _CubicTons; } set { _CubicTons = value; } }

        private bool _CubicKg;
        public bool CubicKg { get { return _CubicKg; } set { _CubicKg = value; } }

        private bool _CubicGt;
        public bool CubicGt { get { return _CubicGt; } set { _CubicGt = value; } }

        private bool _cmkgt;
        public bool cmkgt { get { return _cmkgt; } set { _cmkgt = value; } }

        private bool _UraniumKg;
        public bool UraniumKg { get { return _UraniumKg; } set { _UraniumKg = value; } }
        
        private bool _PrimSec;
        public bool PrimSec { get { return _PrimSec; } set { _PrimSec = value; } }

        private bool _TotalMetresIncl;
        public bool TotalMetresIncl { get { return _TotalMetresIncl; } set { _TotalMetresIncl = value; } }

        private bool _TotalMetresExcl;
        public bool TotalMetresExcl { get { return _TotalMetresExcl; } set { _TotalMetresExcl = value; } }

        private bool _OnReefMetres;
        public bool OnReefMetres { get { return _OnReefMetres; } set { _OnReefMetres = value; } }

        private bool _OffReefMetres;
        public bool OffReefMetres { get { return _OffReefMetres; } set { _OffReefMetres = value; } }

        private bool _MainAdvance;
        public bool MainAdvance { get { return _MainAdvance; } set { _MainAdvance = value; } }

        private bool _MainOnReefMetres;
        public bool MainOnReefMetres { get { return _MainOnReefMetres; } set { _MainOnReefMetres = value; } }

        private bool _MainOffReefMetres;
        public bool MainOffReefMetres { get { return _MainOffReefMetres; } set { _MainOffReefMetres = value; } }

        private bool _SecMetres;
        public bool SecMetres { get { return _SecMetres; } set { _SecMetres = value; } }

        private bool _SecOnReefMetres;
        public bool SecOnReefMetres { get { return _SecOnReefMetres; } set { _SecOnReefMetres = value; } }

        private bool _SecOffReefMetres;
        public bool SecOffReefMetres { get { return _SecOffReefMetres; } set { _SecOffReefMetres = value; } }

        private bool _CapitalMetres;
        public bool CapitalMetres { get { return _CapitalMetres; } set { _CapitalMetres = value; } }

        private bool _CapitalOnReefMetres;
        public bool CapitalOnReefMetres { get { return _CapitalOnReefMetres; } set { _CapitalOnReefMetres = value; } }

        private bool _CapitalOffReefMetres;
        public bool CapitalOffReefMetres { get { return _CapitalOffReefMetres; } set { _CapitalOffReefMetres = value; } }

        private bool _EquivMetres;
        public bool EquivMetres { get { return _EquivMetres; } set { _EquivMetres = value; } }

        private bool _EquivOnReefMetres;
        public bool EquivOnReefMetres { get { return _EquivOnReefMetres; } set { _EquivOnReefMetres = value; } }

        private bool _EquivOffReefMetres;
        public bool EquivOffReefMetres { get { return _EquivOffReefMetres; } set { _EquivOffReefMetres = value; } }

        private bool _DrillRigg;
        public bool DrillRigg { get { return _DrillRigg; } set { _DrillRigg = value; } }       
    }
}
