using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Mineware.Systems.Global.ReportsControls;
using Mineware.Systems.Global.sysMessages;
using FastReport;
using Mineware.Systems.Global;
using System.Threading;
using Mineware.Systems.ProductionGlobal;
using System.Data.SqlClient;
using System.Drawing;
using Mineware.Systems.GlobalConnect;
using System.Text;
using System.Configuration;

namespace Mineware.Systems.Production.Reporting.PlanningNewStyle
{
	public partial class ucPlanningReportNewStyle : BaseUserControl
	{
		private Thread theReportThread;
		new string theSystemDBTag = "Amplats";
		string reportType = "";
        Procedures procs = new Procedures();
		string activityType = "";
		private string _theConnection;
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        public string theConnection { get { return _theConnection; } set { _theConnection = value; } }
		DataTable dtSections = new DataTable();
		DataTable dtReportTables;
		public DataTable section = new DataTable();
		StringBuilder sb = new StringBuilder();
		private PlanningReportNewStyleProperties reportSettings = new PlanningReportNewStyleProperties();
		private DevExpress.XtraVerticalGrid.PropertyGridControl pgPlanSettings;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpM2M;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpTons;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpVolume;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iProdmonth;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iActivity;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iPlan;
		private Global.CustomControls.MWRepositoryItemProdMonth rpProdmonth;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpActivity;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpPlan;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpSectionid;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpSummaryon;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpProdSupervisor;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpMiner;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpWorkplace;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpWorkplaceid;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpStopped;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpDayCrew;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpAfternoonCrew;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpNightCrew;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpRovingCrew;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCrewStrength;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpSQM;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOnReefSqm;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOffReefSqm;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpFaceLength;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOnReefFL;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOffReefFL;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpAdvance;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOnReefAdvance;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOffReefAdvance;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpChannelWidth;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpChannelWidthTons;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpStopeWidth;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpIdealStopeWidth;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOffReefTons;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOnReefTons;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOnReefVolume;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOffReefVolume;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpGT;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCMGT;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpKG;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCMKGT;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpUraniumKg;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCubicmetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCubicTons;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCubicKg;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCubicGt;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpRemarks;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iSectionid;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iSummaryon;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iAurtPlan;
		private DevExpress.XtraVerticalGrid.Rows.CategoryRow iDetail;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iProdsupervisor;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iMiner;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iWorkplace;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iWorkplaceid;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iStopped;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iDaycrew;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iAfternoonCrew;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iNightCrew;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iRovingCrew;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCrewStrength;
		private DevExpress.XtraVerticalGrid.Rows.CategoryRow iStoping;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iSQM;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOnReefSqm;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOffReefSqm;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iFacelength;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOnReefFL;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOffReefFL;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iAdvance;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOnReefAdvance;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOffReefAdvance;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iChannelWidth;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iChannelWidthTons;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iStopeWidth;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iIdealStopeWidth;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iTons;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOnReefTons;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOffReefTons;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iGT;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCMGT;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iKG;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCMKGT;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iUraniumKg;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCubicMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCubicTons;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCubicKg;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCubicGt;

		public string Miner;
		public string ShowAuth = "N";
		public string Workplace;
		public string Workplaceid;
		public string Stopped;
		public string DayCrew;
		public string AfternoonCrew;
		public string NightCrew;
		public string RovingCrew;
		public string CrewStrength;
		public string MiningMethod;
		public string SQM;
		public string OnReefSqm;
		public string OffReefSqm;
		public string Facelength;
		public string OnReefFL;
		public string OffReefFL;
		public string Advance;
		public string OnReefAdvance;
		public string OffReefAdvance;
		public string ChannelWidth;
		public string ChannelWidthTons;
		public string StopeWidth;
		public string IdealStopeWidth;
		public string Tons;
		public string OnReefTons;
		public string OffReefTons;
		public string gt;
		public string cmgt;
		public string KG;
		public string CubicMetres;
		public string CubicTons;
		public string CubicKg;
		public string CubicGt;
		public string ProdSupervisor;
		public string section1;
		public string HierarchicalID;
		public string PrimSec;
		public string TotalMetersIncl;
		public string TotalMetresExcl;
		public string OnReefMetres;
		public string OffReefMetres;
		public string MainAdvance;
		public string MainOnReefMetres;
		public string MainOffReefMetres;
		public string SecMetres;
		public string SecOnReefMetres;
		public string SecOffReefMetres;
		public string CapitalMetres;
		public string CapitalOnReefMetres;
		public string CapitalOffReefMetres;
		public string EquivMetres;
		public string EquivOnReefMetres;
		public string EquivOffReefMetres;
		public string DrillRigg;
		public string cmkgt;
		public string UraniumKg;
		string Thelevel;
		private string _pmonth;
		private DevExpress.XtraVerticalGrid.Rows.CategoryRow iDevelopment;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iPrimSec;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iTotalMeters;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iTotalMetersExcl;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOnReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOffReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iMainAdvance;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iMainOnReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iMainOffReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iSecMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iSecOnReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iSecOffReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCapitalMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCapitalOnReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCapitalOffReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iEquivMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iEquivOnReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iEquivOffReefMetres;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iTonsDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOnReefTonsDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iOffReefTonsDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iGTDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCMGTDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iKgDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCMKGTDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iUraniumKgDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCubicMetresDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCubicTonsDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCubicKgDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iCubicgtDev;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iDrillRigg;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpPrimSec;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpTotalMetersIncl;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpTotalMetersExcl;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOnReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOffReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpMainAdvance;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpMainOnReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpMainOffReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpSecMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpSecOnReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpSecOffReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCapitalMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCapitalOnReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCapitalOffReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpEquivMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpEquivOnReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpEquivOffReefMetres;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpDrillRigg;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOnReefTonsDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpOffReefTonsDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpGTDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCMGTDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpKGDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCubicMetresDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCubicTonsDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCubicKgDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpCubicgtDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpRemarksDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpTonsDev;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpMiningMethod;
		private DevExpress.XtraVerticalGrid.Rows.PGridEditorRow iMiningMethod;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpShowAurt;
        private DevExpress.XtraBars.Ribbon.RibbonControl rcPlanning;
        private DevExpress.XtraBars.BarButtonItem btnShow;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private FastReport.Preview.PreviewControl pcReport2;
        bool result;

		public ucPlanningReportNewStyle()
		{
			InitializeComponent();
            FormRibbonPages.Add(ribbonPage1);
            FormActiveRibbonPage = ribbonPage1;
            FormMainRibbonPage = ribbonPage1;
            RibbonControl = rcPlanning;

        }
		

		void GetLevel()
		{
			if (reportSettings.HierarchicalID == 1)

				Thelevel = "Sectionid_5";

			if (reportSettings.HierarchicalID == 2)

				Thelevel = "Sectionid_4";

			if (reportSettings.HierarchicalID == 3)

				Thelevel = "Sectionid_3";

			if (reportSettings.HierarchicalID == 4)

				Thelevel = "Sectionid_2";

			if (reportSettings.HierarchicalID == 5)

				Thelevel = "Sectionid_1";

			if (reportSettings.HierarchicalID == 6)

				Thelevel = "Sectionid";
		}

		private void createReport(Object theReportSettings)
		{
			

            
           
        }

		void LoadReportType()
		{
			if (reportSettings.PlanType == "1")
			{ reportType = "Monthly Dynamic Plan"; }
			else if (reportSettings.PlanType == "2")
			{ reportType = "Monthly Lock Plan"; }
			if (reportSettings.PlanType == "3")
			{ reportType = "Cycle Dynamic Plan"; }
			else if (reportSettings.PlanType == "4")
			{ reportType = "Cycle Lock Plan"; }
		}

		void LoadActivityType()
		{
			if (reportSettings.ActivityType == "0")
			{
				activityType = "Stoping";
			}

			else if (reportSettings.ActivityType == "1")
			{
				activityType = "Development";
			}

			else if (reportSettings.ActivityType == "8")
			{
				activityType = "Sweeping";
			}

		}
        #pragma warning disable CS0109
        private new void InitializeComponent()
        #pragma warning restore CS0109
        {
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            this.pgPlanSettings = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.rpM2M = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpTons = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpVolume = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpProdmonth = new Mineware.Systems.Global.CustomControls.MWRepositoryItemProdMonth();
            this.rpActivity = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpPlan = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpSectionid = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpSummaryon = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpProdSupervisor = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpMiner = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpWorkplace = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpWorkplaceid = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpStopped = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpDayCrew = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpAfternoonCrew = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpNightCrew = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpRovingCrew = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCrewStrength = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpSQM = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOnReefSqm = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOffReefSqm = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpFaceLength = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOnReefFL = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOffReefFL = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpAdvance = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOnReefAdvance = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOffReefAdvance = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpChannelWidth = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpChannelWidthTons = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpStopeWidth = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpIdealStopeWidth = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOffReefTons = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOnReefTons = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOnReefVolume = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOffReefVolume = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpGT = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCMGT = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpKG = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCMKGT = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpUraniumKg = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCubicmetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCubicTons = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCubicKg = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCubicGt = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpRemarks = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpPrimSec = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpTotalMetersIncl = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpTotalMetersExcl = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOnReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOffReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpMainAdvance = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpMainOnReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpMainOffReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpSecMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpSecOnReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpSecOffReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCapitalMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCapitalOnReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCapitalOffReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpEquivMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpEquivOnReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpEquivOffReefMetres = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpDrillRigg = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOnReefTonsDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpOffReefTonsDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpGTDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCMGTDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpKGDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCubicMetresDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCubicTonsDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCubicKgDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpCubicgtDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpRemarksDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpTonsDev = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpMiningMethod = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpShowAurt = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.iProdmonth = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iActivity = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iPlan = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iSectionid = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iSummaryon = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iAurtPlan = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iDetail = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.iProdsupervisor = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iMiner = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iWorkplace = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iWorkplaceid = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iStopped = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iDaycrew = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iAfternoonCrew = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iNightCrew = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iRovingCrew = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCrewStrength = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iMiningMethod = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iStoping = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.iSQM = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOnReefSqm = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOffReefSqm = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iFacelength = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOnReefFL = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOffReefFL = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iAdvance = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOnReefAdvance = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOffReefAdvance = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iChannelWidth = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iChannelWidthTons = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iStopeWidth = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iIdealStopeWidth = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iTons = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOnReefTons = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOffReefTons = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iGT = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCMGT = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iKG = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCMKGT = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iUraniumKg = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCubicMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCubicTons = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCubicKg = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCubicGt = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iDevelopment = new DevExpress.XtraVerticalGrid.Rows.CategoryRow();
            this.iPrimSec = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iTotalMeters = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iTotalMetersExcl = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOnReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOffReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iMainAdvance = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iMainOnReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iMainOffReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iSecMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iSecOnReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iSecOffReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCapitalMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCapitalOnReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCapitalOffReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iEquivMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iEquivOnReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iEquivOffReefMetres = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iTonsDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOnReefTonsDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iOffReefTonsDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iGTDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCMGTDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iKgDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCMKGTDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iUraniumKgDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCubicMetresDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCubicTonsDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCubicKgDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iCubicgtDev = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.iDrillRigg = new DevExpress.XtraVerticalGrid.Rows.PGridEditorRow();
            this.rcPlanning = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnShow = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.pcReport2 = new FastReport.Preview.PreviewControl();
            ((System.ComponentModel.ISupportInitialize)(this.pgPlanSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpM2M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpProdmonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpActivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSectionid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSummaryon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpProdSupervisor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMiner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpWorkplace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpWorkplaceid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpStopped)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpDayCrew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpAfternoonCrew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpNightCrew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpRovingCrew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCrewStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSQM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefSqm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefSqm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpFaceLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefFL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefFL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpAdvance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefAdvance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefAdvance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpChannelWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpChannelWidthTons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpStopeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIdealStopeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefTons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefTons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpGT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCMGT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpKG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCMKGT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpUraniumKg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicmetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicTons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicKg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicGt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpRemarks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpPrimSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTotalMetersIncl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTotalMetersExcl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMainAdvance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMainOnReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMainOffReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSecMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSecOnReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSecOffReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCapitalMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCapitalOnReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCapitalOffReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEquivMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEquivOnReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEquivOffReefMetres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpDrillRigg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefTonsDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefTonsDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpGTDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCMGTDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpKGDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicMetresDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicTonsDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicKgDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicgtDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpRemarksDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTonsDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMiningMethod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpShowAurt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcPlanning)).BeginInit();
            this.SuspendLayout();
            // 
            // pgPlanSettings
            // 
            this.pgPlanSettings.Cursor = System.Windows.Forms.Cursors.Default;
            this.pgPlanSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.pgPlanSettings.Location = new System.Drawing.Point(0, 110);
            this.pgPlanSettings.Name = "pgPlanSettings";
            this.pgPlanSettings.OptionsBehavior.PropertySort = DevExpress.XtraVerticalGrid.PropertySort.NoSort;
            this.pgPlanSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pgPlanSettings.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rpM2M,
            this.rpTons,
            this.rpVolume,
            this.rpProdmonth,
            this.rpActivity,
            this.rpPlan,
            this.rpSectionid,
            this.rpSummaryon,
            this.rpProdSupervisor,
            this.rpMiner,
            this.rpWorkplace,
            this.rpWorkplaceid,
            this.rpStopped,
            this.rpDayCrew,
            this.rpAfternoonCrew,
            this.rpNightCrew,
            this.rpRovingCrew,
            this.rpCrewStrength,
            this.rpSQM,
            this.rpOnReefSqm,
            this.rpOffReefSqm,
            this.rpFaceLength,
            this.rpOnReefFL,
            this.rpOffReefFL,
            this.rpAdvance,
            this.rpOnReefAdvance,
            this.rpOffReefAdvance,
            this.rpChannelWidth,
            this.rpChannelWidthTons,
            this.rpStopeWidth,
            this.rpIdealStopeWidth,
            this.rpOffReefTons,
            this.rpOnReefTons,
            this.rpOnReefVolume,
            this.rpOffReefVolume,
            this.rpGT,
            this.rpCMGT,
            this.rpKG,
            this.rpCMKGT,
            this.rpUraniumKg,
            this.rpCubicmetres,
            this.rpCubicTons,
            this.rpCubicKg,
            this.rpCubicGt,
            this.rpRemarks,
            this.rpPrimSec,
            this.rpTotalMetersIncl,
            this.rpTotalMetersExcl,
            this.rpOnReefMetres,
            this.rpOffReefMetres,
            this.rpMainAdvance,
            this.rpMainOnReefMetres,
            this.rpMainOffReefMetres,
            this.rpSecMetres,
            this.rpSecOnReefMetres,
            this.rpSecOffReefMetres,
            this.rpCapitalMetres,
            this.rpCapitalOnReefMetres,
            this.rpCapitalOffReefMetres,
            this.rpEquivMetres,
            this.rpEquivOnReefMetres,
            this.rpEquivOffReefMetres,
            this.rpDrillRigg,
            this.rpOnReefTonsDev,
            this.rpOffReefTonsDev,
            this.rpGTDev,
            this.rpCMGTDev,
            this.rpKGDev,
            this.rpCubicMetresDev,
            this.rpCubicTonsDev,
            this.rpCubicKgDev,
            this.rpCubicgtDev,
            this.rpRemarksDev,
            this.rpTonsDev,
            this.rpMiningMethod,
            this.rpShowAurt});
            this.pgPlanSettings.Rows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.iProdmonth,
            this.iActivity,
            this.iPlan,
            this.iSectionid,
            this.iSummaryon,
            this.iAurtPlan,
            this.iDetail,
            this.iStoping,
            this.iDevelopment});
            this.pgPlanSettings.Size = new System.Drawing.Size(378, 617);
            this.pgPlanSettings.TabIndex = 4;
            this.pgPlanSettings.Click += new System.EventHandler(this.pgPlanSettings_Click);
            // 
            // rpM2M
            // 
            this.rpM2M.AutoHeight = false;
            this.rpM2M.Caption = "Check";
            this.rpM2M.Name = "rpM2M";
            // 
            // rpTons
            // 
            this.rpTons.AutoHeight = false;
            this.rpTons.Caption = "Check";
            this.rpTons.Name = "rpTons";
            // 
            // rpVolume
            // 
            this.rpVolume.AutoHeight = false;
            this.rpVolume.Caption = "Check";
            this.rpVolume.Name = "rpVolume";
            // 
            // rpProdmonth
            // 
            this.rpProdmonth.AutoHeight = false;
            this.rpProdmonth.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.SpinUp, "1", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.SpinDown, "2", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.rpProdmonth.Mask.EditMask = "yyyyMM";
            this.rpProdmonth.Mask.IgnoreMaskBlank = false;
            this.rpProdmonth.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            this.rpProdmonth.Mask.UseMaskAsDisplayFormat = true;
            this.rpProdmonth.Name = "rpProdmonth";
            // 
            // rpActivity
            // 
            this.rpActivity.AutoHeight = false;
            this.rpActivity.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpActivity.Name = "rpActivity";
            this.rpActivity.NullText = "";
            // 
            // rpPlan
            // 
            this.rpPlan.AutoHeight = false;
            this.rpPlan.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpPlan.Name = "rpPlan";
            this.rpPlan.NullText = "";
            // 
            // rpSectionid
            // 
            this.rpSectionid.AutoHeight = false;
            this.rpSectionid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpSectionid.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("sectionid", "SectionID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "NAME")});
            this.rpSectionid.Name = "rpSectionid";
            this.rpSectionid.NullText = "";
            this.rpSectionid.ValueMember = "1";
            this.rpSectionid.EditValueChanged += new System.EventHandler(this.rpSectionid_EditValueChanged);
            // 
            // rpSummaryon
            // 
            this.rpSummaryon.AutoHeight = false;
            this.rpSummaryon.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpSummaryon.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("HierarchicalID", "HierarchicalID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Description", "Description")});
            this.rpSummaryon.Name = "rpSummaryon";
            this.rpSummaryon.NullText = "";
            this.rpSummaryon.EditValueChanged += new System.EventHandler(this.rpSummaryon_EditValueChanged);
            // 
            // rpProdSupervisor
            // 
            this.rpProdSupervisor.AutoHeight = false;
            this.rpProdSupervisor.Caption = "Check";
            this.rpProdSupervisor.Name = "rpProdSupervisor";
            // 
            // rpMiner
            // 
            this.rpMiner.AutoHeight = false;
            this.rpMiner.Caption = "Check";
            this.rpMiner.Name = "rpMiner";
            // 
            // rpWorkplace
            // 
            this.rpWorkplace.AutoHeight = false;
            this.rpWorkplace.Caption = "Check";
            this.rpWorkplace.Name = "rpWorkplace";
            // 
            // rpWorkplaceid
            // 
            this.rpWorkplaceid.AutoHeight = false;
            this.rpWorkplaceid.Caption = "Check";
            this.rpWorkplaceid.Name = "rpWorkplaceid";
            // 
            // rpStopped
            // 
            this.rpStopped.AutoHeight = false;
            this.rpStopped.Caption = "Check";
            this.rpStopped.Name = "rpStopped";
            // 
            // rpDayCrew
            // 
            this.rpDayCrew.AutoHeight = false;
            this.rpDayCrew.Caption = "Check";
            this.rpDayCrew.Name = "rpDayCrew";
            // 
            // rpAfternoonCrew
            // 
            this.rpAfternoonCrew.AutoHeight = false;
            this.rpAfternoonCrew.Caption = "Check";
            this.rpAfternoonCrew.Name = "rpAfternoonCrew";
            // 
            // rpNightCrew
            // 
            this.rpNightCrew.AutoHeight = false;
            this.rpNightCrew.Caption = "Check";
            this.rpNightCrew.Name = "rpNightCrew";
            // 
            // rpRovingCrew
            // 
            this.rpRovingCrew.AutoHeight = false;
            this.rpRovingCrew.Caption = "Check";
            this.rpRovingCrew.Name = "rpRovingCrew";
            // 
            // rpCrewStrength
            // 
            this.rpCrewStrength.AutoHeight = false;
            this.rpCrewStrength.Caption = "Check";
            this.rpCrewStrength.Name = "rpCrewStrength";
            // 
            // rpSQM
            // 
            this.rpSQM.AutoHeight = false;
            this.rpSQM.Caption = "Check";
            this.rpSQM.Name = "rpSQM";
            // 
            // rpOnReefSqm
            // 
            this.rpOnReefSqm.AutoHeight = false;
            this.rpOnReefSqm.Caption = "Check";
            this.rpOnReefSqm.Name = "rpOnReefSqm";
            // 
            // rpOffReefSqm
            // 
            this.rpOffReefSqm.AutoHeight = false;
            this.rpOffReefSqm.Caption = "Check";
            this.rpOffReefSqm.Name = "rpOffReefSqm";
            // 
            // rpFaceLength
            // 
            this.rpFaceLength.AutoHeight = false;
            this.rpFaceLength.Caption = "Check";
            this.rpFaceLength.Name = "rpFaceLength";
            // 
            // rpOnReefFL
            // 
            this.rpOnReefFL.AutoHeight = false;
            this.rpOnReefFL.Caption = "Check";
            this.rpOnReefFL.Name = "rpOnReefFL";
            // 
            // rpOffReefFL
            // 
            this.rpOffReefFL.AutoHeight = false;
            this.rpOffReefFL.Caption = "Check";
            this.rpOffReefFL.Name = "rpOffReefFL";
            // 
            // rpAdvance
            // 
            this.rpAdvance.AutoHeight = false;
            this.rpAdvance.Caption = "Check";
            this.rpAdvance.Name = "rpAdvance";
            // 
            // rpOnReefAdvance
            // 
            this.rpOnReefAdvance.AutoHeight = false;
            this.rpOnReefAdvance.Caption = "Check";
            this.rpOnReefAdvance.Name = "rpOnReefAdvance";
            // 
            // rpOffReefAdvance
            // 
            this.rpOffReefAdvance.AutoHeight = false;
            this.rpOffReefAdvance.Caption = "Check";
            this.rpOffReefAdvance.Name = "rpOffReefAdvance";
            // 
            // rpChannelWidth
            // 
            this.rpChannelWidth.AutoHeight = false;
            this.rpChannelWidth.Caption = "Check";
            this.rpChannelWidth.Name = "rpChannelWidth";
            // 
            // rpChannelWidthTons
            // 
            this.rpChannelWidthTons.AutoHeight = false;
            this.rpChannelWidthTons.Caption = "Check";
            this.rpChannelWidthTons.Name = "rpChannelWidthTons";
            // 
            // rpStopeWidth
            // 
            this.rpStopeWidth.AutoHeight = false;
            this.rpStopeWidth.Caption = "Check";
            this.rpStopeWidth.Name = "rpStopeWidth";
            // 
            // rpIdealStopeWidth
            // 
            this.rpIdealStopeWidth.AutoHeight = false;
            this.rpIdealStopeWidth.Caption = "Check";
            this.rpIdealStopeWidth.Name = "rpIdealStopeWidth";
            // 
            // rpOffReefTons
            // 
            this.rpOffReefTons.AutoHeight = false;
            this.rpOffReefTons.Caption = "Check";
            this.rpOffReefTons.Name = "rpOffReefTons";
            // 
            // rpOnReefTons
            // 
            this.rpOnReefTons.AutoHeight = false;
            this.rpOnReefTons.Caption = "Check";
            this.rpOnReefTons.Name = "rpOnReefTons";
            // 
            // rpOnReefVolume
            // 
            this.rpOnReefVolume.AutoHeight = false;
            this.rpOnReefVolume.Caption = "Check";
            this.rpOnReefVolume.Name = "rpOnReefVolume";
            // 
            // rpOffReefVolume
            // 
            this.rpOffReefVolume.AutoHeight = false;
            this.rpOffReefVolume.Caption = "Check";
            this.rpOffReefVolume.Name = "rpOffReefVolume";
            // 
            // rpGT
            // 
            this.rpGT.AutoHeight = false;
            this.rpGT.Caption = "Check";
            this.rpGT.Name = "rpGT";
            // 
            // rpCMGT
            // 
            this.rpCMGT.AutoHeight = false;
            this.rpCMGT.Caption = "Check";
            this.rpCMGT.Name = "rpCMGT";
            // 
            // rpKG
            // 
            this.rpKG.AutoHeight = false;
            this.rpKG.Caption = "Check";
            this.rpKG.Name = "rpKG";
            // 
            // rpCMKGT
            // 
            this.rpCMKGT.AutoHeight = false;
            this.rpCMKGT.Caption = "Check";
            this.rpCMKGT.Name = "rpCMKGT";
            // 
            // rpUraniumKg
            // 
            this.rpUraniumKg.AutoHeight = false;
            this.rpUraniumKg.Caption = "Check";
            this.rpUraniumKg.Name = "rpUraniumKg";
            // 
            // rpCubicmetres
            // 
            this.rpCubicmetres.AutoHeight = false;
            this.rpCubicmetres.Caption = "Check";
            this.rpCubicmetres.Name = "rpCubicmetres";
            // 
            // rpCubicTons
            // 
            this.rpCubicTons.AutoHeight = false;
            this.rpCubicTons.Caption = "Check";
            this.rpCubicTons.Name = "rpCubicTons";
            // 
            // rpCubicKg
            // 
            this.rpCubicKg.AutoHeight = false;
            this.rpCubicKg.Caption = "Check";
            this.rpCubicKg.Name = "rpCubicKg";
            // 
            // rpCubicGt
            // 
            this.rpCubicGt.AutoHeight = false;
            this.rpCubicGt.Caption = "Check";
            this.rpCubicGt.Name = "rpCubicGt";
            // 
            // rpRemarks
            // 
            this.rpRemarks.AutoHeight = false;
            this.rpRemarks.Caption = "Check";
            this.rpRemarks.Name = "rpRemarks";
            // 
            // rpPrimSec
            // 
            this.rpPrimSec.AutoHeight = false;
            this.rpPrimSec.Caption = "Check";
            this.rpPrimSec.Name = "rpPrimSec";
            // 
            // rpTotalMetersIncl
            // 
            this.rpTotalMetersIncl.AutoHeight = false;
            this.rpTotalMetersIncl.Caption = "Check";
            this.rpTotalMetersIncl.Name = "rpTotalMetersIncl";
            // 
            // rpTotalMetersExcl
            // 
            this.rpTotalMetersExcl.AutoHeight = false;
            this.rpTotalMetersExcl.Caption = "Check";
            this.rpTotalMetersExcl.Name = "rpTotalMetersExcl";
            // 
            // rpOnReefMetres
            // 
            this.rpOnReefMetres.AutoHeight = false;
            this.rpOnReefMetres.Caption = "Check";
            this.rpOnReefMetres.Name = "rpOnReefMetres";
            // 
            // rpOffReefMetres
            // 
            this.rpOffReefMetres.AutoHeight = false;
            this.rpOffReefMetres.Caption = "Check";
            this.rpOffReefMetres.Name = "rpOffReefMetres";
            // 
            // rpMainAdvance
            // 
            this.rpMainAdvance.AutoHeight = false;
            this.rpMainAdvance.Caption = "Check";
            this.rpMainAdvance.Name = "rpMainAdvance";
            // 
            // rpMainOnReefMetres
            // 
            this.rpMainOnReefMetres.AutoHeight = false;
            this.rpMainOnReefMetres.Caption = "Check";
            this.rpMainOnReefMetres.Name = "rpMainOnReefMetres";
            // 
            // rpMainOffReefMetres
            // 
            this.rpMainOffReefMetres.AutoHeight = false;
            this.rpMainOffReefMetres.Caption = "Check";
            this.rpMainOffReefMetres.Name = "rpMainOffReefMetres";
            // 
            // rpSecMetres
            // 
            this.rpSecMetres.AutoHeight = false;
            this.rpSecMetres.Caption = "Check";
            this.rpSecMetres.Name = "rpSecMetres";
            // 
            // rpSecOnReefMetres
            // 
            this.rpSecOnReefMetres.AutoHeight = false;
            this.rpSecOnReefMetres.Caption = "Check";
            this.rpSecOnReefMetres.Name = "rpSecOnReefMetres";
            // 
            // rpSecOffReefMetres
            // 
            this.rpSecOffReefMetres.AutoHeight = false;
            this.rpSecOffReefMetres.Caption = "Check";
            this.rpSecOffReefMetres.Name = "rpSecOffReefMetres";
            // 
            // rpCapitalMetres
            // 
            this.rpCapitalMetres.AutoHeight = false;
            this.rpCapitalMetres.Caption = "Check";
            this.rpCapitalMetres.Name = "rpCapitalMetres";
            // 
            // rpCapitalOnReefMetres
            // 
            this.rpCapitalOnReefMetres.AutoHeight = false;
            this.rpCapitalOnReefMetres.Caption = "Check";
            this.rpCapitalOnReefMetres.Name = "rpCapitalOnReefMetres";
            // 
            // rpCapitalOffReefMetres
            // 
            this.rpCapitalOffReefMetres.AutoHeight = false;
            this.rpCapitalOffReefMetres.Caption = "Check";
            this.rpCapitalOffReefMetres.Name = "rpCapitalOffReefMetres";
            // 
            // rpEquivMetres
            // 
            this.rpEquivMetres.AutoHeight = false;
            this.rpEquivMetres.Caption = "Check";
            this.rpEquivMetres.Name = "rpEquivMetres";
            // 
            // rpEquivOnReefMetres
            // 
            this.rpEquivOnReefMetres.AutoHeight = false;
            this.rpEquivOnReefMetres.Caption = "Check";
            this.rpEquivOnReefMetres.Name = "rpEquivOnReefMetres";
            // 
            // rpEquivOffReefMetres
            // 
            this.rpEquivOffReefMetres.AutoHeight = false;
            this.rpEquivOffReefMetres.Caption = "Check";
            this.rpEquivOffReefMetres.Name = "rpEquivOffReefMetres";
            // 
            // rpDrillRigg
            // 
            this.rpDrillRigg.AutoHeight = false;
            this.rpDrillRigg.Caption = "Check";
            this.rpDrillRigg.Name = "rpDrillRigg";
            // 
            // rpOnReefTonsDev
            // 
            this.rpOnReefTonsDev.AutoHeight = false;
            this.rpOnReefTonsDev.Caption = "Check";
            this.rpOnReefTonsDev.Name = "rpOnReefTonsDev";
            // 
            // rpOffReefTonsDev
            // 
            this.rpOffReefTonsDev.AutoHeight = false;
            this.rpOffReefTonsDev.Caption = "Check";
            this.rpOffReefTonsDev.Name = "rpOffReefTonsDev";
            // 
            // rpGTDev
            // 
            this.rpGTDev.AutoHeight = false;
            this.rpGTDev.Caption = "Check";
            this.rpGTDev.Name = "rpGTDev";
            // 
            // rpCMGTDev
            // 
            this.rpCMGTDev.AutoHeight = false;
            this.rpCMGTDev.Caption = "Check";
            this.rpCMGTDev.Name = "rpCMGTDev";
            // 
            // rpKGDev
            // 
            this.rpKGDev.AutoHeight = false;
            this.rpKGDev.Caption = "Check";
            this.rpKGDev.Name = "rpKGDev";
            // 
            // rpCubicMetresDev
            // 
            this.rpCubicMetresDev.AutoHeight = false;
            this.rpCubicMetresDev.Caption = "Check";
            this.rpCubicMetresDev.Name = "rpCubicMetresDev";
            // 
            // rpCubicTonsDev
            // 
            this.rpCubicTonsDev.AutoHeight = false;
            this.rpCubicTonsDev.Caption = "Check";
            this.rpCubicTonsDev.Name = "rpCubicTonsDev";
            // 
            // rpCubicKgDev
            // 
            this.rpCubicKgDev.AutoHeight = false;
            this.rpCubicKgDev.Caption = "Check";
            this.rpCubicKgDev.Name = "rpCubicKgDev";
            // 
            // rpCubicgtDev
            // 
            this.rpCubicgtDev.AutoHeight = false;
            this.rpCubicgtDev.Caption = "Check";
            this.rpCubicgtDev.Name = "rpCubicgtDev";
            // 
            // rpRemarksDev
            // 
            this.rpRemarksDev.AutoHeight = false;
            this.rpRemarksDev.Caption = "Check";
            this.rpRemarksDev.Name = "rpRemarksDev";
            // 
            // rpTonsDev
            // 
            this.rpTonsDev.AutoHeight = false;
            this.rpTonsDev.Caption = "Check";
            this.rpTonsDev.Name = "rpTonsDev";
            // 
            // rpMiningMethod
            // 
            this.rpMiningMethod.AutoHeight = false;
            this.rpMiningMethod.Name = "rpMiningMethod";
            // 
            // rpShowAurt
            // 
            this.rpShowAurt.AutoHeight = false;
            this.rpShowAurt.Name = "rpShowAurt";
            // 
            // iProdmonth
            // 
            this.iProdmonth.IsChildRowsLoaded = true;
            this.iProdmonth.Name = "iProdmonth";
            this.iProdmonth.Properties.Caption = "Production Month";
            this.iProdmonth.Properties.FieldName = "Prodmonth";
            this.iProdmonth.Properties.RowEdit = this.rpProdmonth;
            // 
            // iActivity
            // 
            this.iActivity.Height = 18;
            this.iActivity.IsChildRowsLoaded = true;
            this.iActivity.Name = "iActivity";
            this.iActivity.Properties.Caption = "Activity";
            this.iActivity.Properties.FieldName = "ActivityType";
            this.iActivity.Properties.RowEdit = this.rpActivity;
            // 
            // iPlan
            // 
            this.iPlan.IsChildRowsLoaded = true;
            this.iPlan.Name = "iPlan";
            this.iPlan.Properties.Caption = "Plan";
            this.iPlan.Properties.FieldName = "PlanType";
            this.iPlan.Properties.RowEdit = this.rpPlan;
            // 
            // iSectionid
            // 
            this.iSectionid.Height = 18;
            this.iSectionid.IsChildRowsLoaded = true;
            this.iSectionid.Name = "iSectionid";
            this.iSectionid.Properties.Caption = "SectionID";
            this.iSectionid.Properties.FieldName = "NAME";
            this.iSectionid.Properties.RowEdit = this.rpSectionid;
            // 
            // iSummaryon
            // 
            this.iSummaryon.IsChildRowsLoaded = true;
            this.iSummaryon.Name = "iSummaryon";
            this.iSummaryon.Properties.Caption = "Summary On";
            this.iSummaryon.Properties.FieldName = "HierarchicalID";
            this.iSummaryon.Properties.RowEdit = this.rpSummaryon;
            // 
            // iAurtPlan
            // 
            this.iAurtPlan.Height = 22;
            this.iAurtPlan.IsChildRowsLoaded = true;
            this.iAurtPlan.Name = "iAurtPlan";
            this.iAurtPlan.Properties.Caption = "Show Non Aurthorised";
            this.iAurtPlan.Properties.FieldName = "ShowAuth";
            this.iAurtPlan.Properties.RowEdit = this.rpShowAurt;
            // 
            // iDetail
            // 
            this.iDetail.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.iProdsupervisor,
            this.iMiner,
            this.iWorkplace,
            this.iWorkplaceid,
            this.iStopped,
            this.iDaycrew,
            this.iAfternoonCrew,
            this.iNightCrew,
            this.iRovingCrew,
            this.iCrewStrength,
            this.iMiningMethod});
            this.iDetail.Name = "iDetail";
            this.iDetail.Properties.Caption = "Detail";
            // 
            // iProdsupervisor
            // 
            this.iProdsupervisor.IsChildRowsLoaded = true;
            this.iProdsupervisor.Name = "iProdsupervisor";
            this.iProdsupervisor.Properties.Caption = "Production Supervisor";
            this.iProdsupervisor.Properties.FieldName = "ProdSupervisor";
            this.iProdsupervisor.Properties.RowEdit = this.rpProdSupervisor;
            // 
            // iMiner
            // 
            this.iMiner.IsChildRowsLoaded = true;
            this.iMiner.Name = "iMiner";
            this.iMiner.Properties.Caption = "Miner";
            this.iMiner.Properties.FieldName = "Miner";
            this.iMiner.Properties.RowEdit = this.rpMiner;
            // 
            // iWorkplace
            // 
            this.iWorkplace.IsChildRowsLoaded = true;
            this.iWorkplace.Name = "iWorkplace";
            this.iWorkplace.Properties.Caption = "Workplace";
            this.iWorkplace.Properties.FieldName = "Workplace";
            this.iWorkplace.Properties.RowEdit = this.rpWorkplace;
            // 
            // iWorkplaceid
            // 
            this.iWorkplaceid.IsChildRowsLoaded = true;
            this.iWorkplaceid.Name = "iWorkplaceid";
            this.iWorkplaceid.Properties.Caption = "Workplace ID";
            this.iWorkplaceid.Properties.FieldName = "Workplaceid";
            this.iWorkplaceid.Properties.RowEdit = this.rpWorkplaceid;
            // 
            // iStopped
            // 
            this.iStopped.IsChildRowsLoaded = true;
            this.iStopped.Name = "iStopped";
            this.iStopped.Properties.Caption = "Stopped";
            this.iStopped.Properties.FieldName = "Stopped";
            this.iStopped.Properties.RowEdit = this.rpStopped;
            this.iStopped.Visible = false;
            // 
            // iDaycrew
            // 
            this.iDaycrew.IsChildRowsLoaded = true;
            this.iDaycrew.Name = "iDaycrew";
            this.iDaycrew.Properties.Caption = "Day Crew";
            this.iDaycrew.Properties.FieldName = "DayCrew";
            this.iDaycrew.Properties.RowEdit = this.rpDayCrew;
            // 
            // iAfternoonCrew
            // 
            this.iAfternoonCrew.IsChildRowsLoaded = true;
            this.iAfternoonCrew.Name = "iAfternoonCrew";
            this.iAfternoonCrew.Properties.Caption = "Afternoon Crew";
            this.iAfternoonCrew.Properties.FieldName = "AfternoonCrew";
            this.iAfternoonCrew.Properties.RowEdit = this.rpAfternoonCrew;
            this.iAfternoonCrew.Visible = false;
            // 
            // iNightCrew
            // 
            this.iNightCrew.IsChildRowsLoaded = true;
            this.iNightCrew.Name = "iNightCrew";
            this.iNightCrew.Properties.Caption = "Night Crew";
            this.iNightCrew.Properties.FieldName = "NightCrew";
            this.iNightCrew.Properties.RowEdit = this.rpNightCrew;
            this.iNightCrew.Visible = false;
            // 
            // iRovingCrew
            // 
            this.iRovingCrew.Height = 17;
            this.iRovingCrew.IsChildRowsLoaded = true;
            this.iRovingCrew.Name = "iRovingCrew";
            this.iRovingCrew.Properties.Caption = "Roving Crew";
            this.iRovingCrew.Properties.FieldName = "RovingCrew";
            this.iRovingCrew.Properties.RowEdit = this.rpRovingCrew;
            this.iRovingCrew.Visible = false;
            // 
            // iCrewStrength
            // 
            this.iCrewStrength.IsChildRowsLoaded = true;
            this.iCrewStrength.Name = "iCrewStrength";
            this.iCrewStrength.Properties.Caption = "Crew Strength";
            this.iCrewStrength.Properties.FieldName = "CrewStrength";
            this.iCrewStrength.Properties.RowEdit = this.rpCrewStrength;
            this.iCrewStrength.Visible = false;
            // 
            // iMiningMethod
            // 
            this.iMiningMethod.Height = 17;
            this.iMiningMethod.IsChildRowsLoaded = true;
            this.iMiningMethod.Name = "iMiningMethod";
            this.iMiningMethod.Properties.Caption = "Mining Method";
            this.iMiningMethod.Properties.FieldName = "MiningMethod";
            this.iMiningMethod.Properties.RowEdit = this.rpMiningMethod;
            this.iMiningMethod.Visible = false;
            // 
            // iStoping
            // 
            this.iStoping.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.iSQM,
            this.iOnReefSqm,
            this.iOffReefSqm,
            this.iFacelength,
            this.iOnReefFL,
            this.iOffReefFL,
            this.iAdvance,
            this.iOnReefAdvance,
            this.iOffReefAdvance,
            this.iChannelWidth,
            this.iChannelWidthTons,
            this.iStopeWidth,
            this.iIdealStopeWidth,
            this.iTons,
            this.iOnReefTons,
            this.iOffReefTons,
            this.iGT,
            this.iCMGT,
            this.iKG,
            this.iCMKGT,
            this.iUraniumKg,
            this.iCubicMetres,
            this.iCubicTons,
            this.iCubicKg,
            this.iCubicGt});
            this.iStoping.Name = "iStoping";
            this.iStoping.Properties.Caption = "Stoping";
            // 
            // iSQM
            // 
            this.iSQM.IsChildRowsLoaded = true;
            this.iSQM.Name = "iSQM";
            this.iSQM.Properties.Caption = "SQM";
            this.iSQM.Properties.FieldName = "SQM";
            this.iSQM.Properties.RowEdit = this.rpSQM;
            // 
            // iOnReefSqm
            // 
            this.iOnReefSqm.IsChildRowsLoaded = true;
            this.iOnReefSqm.Name = "iOnReefSqm";
            this.iOnReefSqm.Properties.Caption = "On Reef Sqm";
            this.iOnReefSqm.Properties.FieldName = "OnReefSqm";
            this.iOnReefSqm.Properties.RowEdit = this.rpOnReefSqm;
            // 
            // iOffReefSqm
            // 
            this.iOffReefSqm.IsChildRowsLoaded = true;
            this.iOffReefSqm.Name = "iOffReefSqm";
            this.iOffReefSqm.Properties.Caption = "Off Reef Sqm";
            this.iOffReefSqm.Properties.FieldName = "OffReefSqm";
            this.iOffReefSqm.Properties.RowEdit = this.rpOffReefSqm;
            // 
            // iFacelength
            // 
            this.iFacelength.IsChildRowsLoaded = true;
            this.iFacelength.Name = "iFacelength";
            this.iFacelength.Properties.Caption = "Face length";
            this.iFacelength.Properties.FieldName = "Facelength";
            this.iFacelength.Properties.RowEdit = this.rpFaceLength;
            // 
            // iOnReefFL
            // 
            this.iOnReefFL.IsChildRowsLoaded = true;
            this.iOnReefFL.Name = "iOnReefFL";
            this.iOnReefFL.Properties.Caption = "On Reef FL";
            this.iOnReefFL.Properties.FieldName = "OnReefFL";
            this.iOnReefFL.Properties.RowEdit = this.rpOnReefFL;
            // 
            // iOffReefFL
            // 
            this.iOffReefFL.IsChildRowsLoaded = true;
            this.iOffReefFL.Name = "iOffReefFL";
            this.iOffReefFL.Properties.Caption = "Off Reef FL";
            this.iOffReefFL.Properties.FieldName = "OffReefFL";
            this.iOffReefFL.Properties.RowEdit = this.rpOffReefFL;
            // 
            // iAdvance
            // 
            this.iAdvance.IsChildRowsLoaded = true;
            this.iAdvance.Name = "iAdvance";
            this.iAdvance.Properties.Caption = "Advance";
            this.iAdvance.Properties.FieldName = "Advance";
            this.iAdvance.Properties.RowEdit = this.rpAdvance;
            // 
            // iOnReefAdvance
            // 
            this.iOnReefAdvance.IsChildRowsLoaded = true;
            this.iOnReefAdvance.Name = "iOnReefAdvance";
            this.iOnReefAdvance.Properties.Caption = "On Reef Advance";
            this.iOnReefAdvance.Properties.FieldName = "OnReefAdvance";
            this.iOnReefAdvance.Properties.RowEdit = this.rpOnReefAdvance;
            // 
            // iOffReefAdvance
            // 
            this.iOffReefAdvance.IsChildRowsLoaded = true;
            this.iOffReefAdvance.Name = "iOffReefAdvance";
            this.iOffReefAdvance.Properties.Caption = "Off Reef Advance";
            this.iOffReefAdvance.Properties.FieldName = "OffReefAdvance";
            this.iOffReefAdvance.Properties.RowEdit = this.rpOffReefAdvance;
            // 
            // iChannelWidth
            // 
            this.iChannelWidth.IsChildRowsLoaded = true;
            this.iChannelWidth.Name = "iChannelWidth";
            this.iChannelWidth.Properties.Caption = "Channel Width";
            this.iChannelWidth.Properties.FieldName = "ChannelWidth";
            this.iChannelWidth.Properties.RowEdit = this.rpChannelWidth;
            this.iChannelWidth.Visible = false;
            // 
            // iChannelWidthTons
            // 
            this.iChannelWidthTons.IsChildRowsLoaded = true;
            this.iChannelWidthTons.Name = "iChannelWidthTons";
            this.iChannelWidthTons.Properties.Caption = "Channel Width Tons";
            this.iChannelWidthTons.Properties.FieldName = "ChannelWidthTons";
            this.iChannelWidthTons.Properties.RowEdit = this.rpChannelWidthTons;
            // 
            // iStopeWidth
            // 
            this.iStopeWidth.IsChildRowsLoaded = true;
            this.iStopeWidth.Name = "iStopeWidth";
            this.iStopeWidth.Properties.Caption = "Stope Width";
            this.iStopeWidth.Properties.FieldName = "StopeWidth";
            this.iStopeWidth.Properties.RowEdit = this.rpStopeWidth;
            // 
            // iIdealStopeWidth
            // 
            this.iIdealStopeWidth.IsChildRowsLoaded = true;
            this.iIdealStopeWidth.Name = "iIdealStopeWidth";
            this.iIdealStopeWidth.Properties.Caption = "Ideal Stope Width";
            this.iIdealStopeWidth.Properties.FieldName = "IdealStopeWidth";
            this.iIdealStopeWidth.Properties.RowEdit = this.rpIdealStopeWidth;
            this.iIdealStopeWidth.Visible = false;
            // 
            // iTons
            // 
            this.iTons.IsChildRowsLoaded = true;
            this.iTons.Name = "iTons";
            this.iTons.Properties.Caption = "Tons";
            this.iTons.Properties.FieldName = "Tons";
            this.iTons.Properties.RowEdit = this.rpTons;
            // 
            // iOnReefTons
            // 
            this.iOnReefTons.IsChildRowsLoaded = true;
            this.iOnReefTons.Name = "iOnReefTons";
            this.iOnReefTons.Properties.Caption = "On Reef Tons";
            this.iOnReefTons.Properties.FieldName = "OnReefTons";
            this.iOnReefTons.Properties.RowEdit = this.rpOnReefTons;
            // 
            // iOffReefTons
            // 
            this.iOffReefTons.IsChildRowsLoaded = true;
            this.iOffReefTons.Name = "iOffReefTons";
            this.iOffReefTons.Properties.Caption = "Off Reef Tons";
            this.iOffReefTons.Properties.FieldName = "OffReefTons";
            this.iOffReefTons.Properties.RowEdit = this.rpOffReefTons;
            // 
            // iGT
            // 
            this.iGT.IsChildRowsLoaded = true;
            this.iGT.Name = "iGT";
            this.iGT.Properties.Caption = "gt";
            this.iGT.Properties.FieldName = "gt";
            this.iGT.Properties.RowEdit = this.rpGT;
            // 
            // iCMGT
            // 
            this.iCMGT.IsChildRowsLoaded = true;
            this.iCMGT.Name = "iCMGT";
            this.iCMGT.Properties.Caption = "cmgt";
            this.iCMGT.Properties.FieldName = "cmgt";
            this.iCMGT.Properties.RowEdit = this.rpCMGT;
            // 
            // iKG
            // 
            this.iKG.IsChildRowsLoaded = true;
            this.iKG.Name = "iKG";
            this.iKG.Properties.Caption = "Ounces";
            this.iKG.Properties.FieldName = "KG";
            this.iKG.Properties.RowEdit = this.rpKG;
            // 
            // iCMKGT
            // 
            this.iCMKGT.IsChildRowsLoaded = true;
            this.iCMKGT.Name = "iCMKGT";
            this.iCMKGT.Properties.Caption = "Cmkgt";
            this.iCMKGT.Properties.FieldName = "cmkgt";
            this.iCMKGT.Properties.RowEdit = this.rpCMKGT;
            this.iCMKGT.Visible = false;
            // 
            // iUraniumKg
            // 
            this.iUraniumKg.IsChildRowsLoaded = true;
            this.iUraniumKg.Name = "iUraniumKg";
            this.iUraniumKg.Properties.Caption = "Uranium Kg";
            this.iUraniumKg.Properties.FieldName = "UraniumKg";
            this.iUraniumKg.Properties.RowEdit = this.rpUraniumKg;
            this.iUraniumKg.Visible = false;
            // 
            // iCubicMetres
            // 
            this.iCubicMetres.IsChildRowsLoaded = true;
            this.iCubicMetres.Name = "iCubicMetres";
            this.iCubicMetres.Properties.Caption = "Cubic Metres";
            this.iCubicMetres.Properties.FieldName = "CubicMetres";
            this.iCubicMetres.Properties.RowEdit = this.rpCubicmetres;
            this.iCubicMetres.Visible = false;
            // 
            // iCubicTons
            // 
            this.iCubicTons.IsChildRowsLoaded = true;
            this.iCubicTons.Name = "iCubicTons";
            this.iCubicTons.Properties.Caption = "Cubic Tons";
            this.iCubicTons.Properties.FieldName = "CubicTons";
            this.iCubicTons.Properties.RowEdit = this.rpCubicTons;
            this.iCubicTons.Visible = false;
            // 
            // iCubicKg
            // 
            this.iCubicKg.IsChildRowsLoaded = true;
            this.iCubicKg.Name = "iCubicKg";
            this.iCubicKg.Properties.Caption = "Cubic Kg";
            this.iCubicKg.Properties.FieldName = "CubicKg";
            this.iCubicKg.Properties.RowEdit = this.rpCubicKg;
            this.iCubicKg.Visible = false;
            // 
            // iCubicGt
            // 
            this.iCubicGt.IsChildRowsLoaded = true;
            this.iCubicGt.Name = "iCubicGt";
            this.iCubicGt.Properties.Caption = "Cubic Gt";
            this.iCubicGt.Properties.FieldName = "CubicGt";
            this.iCubicGt.Properties.RowEdit = this.rpCubicGt;
            this.iCubicGt.Visible = false;
            // 
            // iDevelopment
            // 
            this.iDevelopment.ChildRows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.iPrimSec,
            this.iTotalMeters,
            this.iTotalMetersExcl,
            this.iOnReefMetres,
            this.iOffReefMetres,
            this.iMainAdvance,
            this.iMainOnReefMetres,
            this.iMainOffReefMetres,
            this.iSecMetres,
            this.iSecOnReefMetres,
            this.iSecOffReefMetres,
            this.iCapitalMetres,
            this.iCapitalOnReefMetres,
            this.iCapitalOffReefMetres,
            this.iEquivMetres,
            this.iEquivOnReefMetres,
            this.iEquivOffReefMetres,
            this.iTonsDev,
            this.iOnReefTonsDev,
            this.iOffReefTonsDev,
            this.iGTDev,
            this.iCMGTDev,
            this.iKgDev,
            this.iCMKGTDev,
            this.iUraniumKgDev,
            this.iCubicMetresDev,
            this.iCubicTonsDev,
            this.iCubicKgDev,
            this.iCubicgtDev,
            this.iDrillRigg});
            this.iDevelopment.Name = "iDevelopment";
            this.iDevelopment.Properties.Caption = "Development";
            // 
            // iPrimSec
            // 
            this.iPrimSec.Expanded = false;
            this.iPrimSec.IsChildRowsLoaded = true;
            this.iPrimSec.Name = "iPrimSec";
            this.iPrimSec.Properties.Caption = "Prim/Sec";
            this.iPrimSec.Properties.FieldName = "PrimSec";
            this.iPrimSec.Properties.RowEdit = this.rpPrimSec;
            // 
            // iTotalMeters
            // 
            this.iTotalMeters.Height = 17;
            this.iTotalMeters.Name = "iTotalMeters";
            this.iTotalMeters.Properties.Caption = "TotalMeters(Incl Equiv)";
            this.iTotalMeters.Properties.FieldName = "TotalMetresIncl";
            this.iTotalMeters.Properties.RowEdit = this.rpTotalMetersIncl;
            // 
            // iTotalMetersExcl
            // 
            this.iTotalMetersExcl.Name = "iTotalMetersExcl";
            this.iTotalMetersExcl.Properties.Caption = "Total Meters(Excl Equiv)";
            this.iTotalMetersExcl.Properties.FieldName = "TotalMetresExcl";
            this.iTotalMetersExcl.Properties.RowEdit = this.rpTotalMetersExcl;
            // 
            // iOnReefMetres
            // 
            this.iOnReefMetres.Name = "iOnReefMetres";
            this.iOnReefMetres.Properties.Caption = "On Reef Metres";
            this.iOnReefMetres.Properties.FieldName = "OnReefMetres";
            this.iOnReefMetres.Properties.RowEdit = this.rpOnReefMetres;
            // 
            // iOffReefMetres
            // 
            this.iOffReefMetres.Name = "iOffReefMetres";
            this.iOffReefMetres.Properties.Caption = "Off Reef Metres";
            this.iOffReefMetres.Properties.FieldName = "OffReefMetres";
            this.iOffReefMetres.Properties.RowEdit = this.rpOffReefMetres;
            // 
            // iMainAdvance
            // 
            this.iMainAdvance.Name = "iMainAdvance";
            this.iMainAdvance.Properties.Caption = "Main Advance";
            this.iMainAdvance.Properties.FieldName = "MainAdvance";
            this.iMainAdvance.Properties.RowEdit = this.rpMainAdvance;
            // 
            // iMainOnReefMetres
            // 
            this.iMainOnReefMetres.Name = "iMainOnReefMetres";
            this.iMainOnReefMetres.Properties.Caption = "Main On Reef Metres";
            this.iMainOnReefMetres.Properties.FieldName = "MainOnReefMetres";
            this.iMainOnReefMetres.Properties.RowEdit = this.rpMainOnReefMetres;
            // 
            // iMainOffReefMetres
            // 
            this.iMainOffReefMetres.Name = "iMainOffReefMetres";
            this.iMainOffReefMetres.Properties.Caption = "MainOff Reef Metres";
            this.iMainOffReefMetres.Properties.FieldName = "MainOffReefMetres";
            this.iMainOffReefMetres.Properties.RowEdit = this.rpMainOffReefMetres;
            // 
            // iSecMetres
            // 
            this.iSecMetres.Name = "iSecMetres";
            this.iSecMetres.Properties.Caption = "Sec Metres";
            this.iSecMetres.Properties.FieldName = "SecMetres";
            this.iSecMetres.Properties.RowEdit = this.rpSecMetres;
            // 
            // iSecOnReefMetres
            // 
            this.iSecOnReefMetres.Height = 17;
            this.iSecOnReefMetres.Name = "iSecOnReefMetres";
            this.iSecOnReefMetres.Properties.Caption = "Sec On Reef Metres";
            this.iSecOnReefMetres.Properties.FieldName = "SecOnReefMetres";
            this.iSecOnReefMetres.Properties.RowEdit = this.rpSecOnReefMetres;
            // 
            // iSecOffReefMetres
            // 
            this.iSecOffReefMetres.Name = "iSecOffReefMetres";
            this.iSecOffReefMetres.Properties.Caption = "Sec Off Reef Metres";
            this.iSecOffReefMetres.Properties.FieldName = "SecOffReefMetres";
            this.iSecOffReefMetres.Properties.RowEdit = this.rpSecOffReefMetres;
            // 
            // iCapitalMetres
            // 
            this.iCapitalMetres.Name = "iCapitalMetres";
            this.iCapitalMetres.Properties.Caption = "Capital Metres";
            this.iCapitalMetres.Properties.FieldName = "CapitalMetres";
            this.iCapitalMetres.Properties.RowEdit = this.rpCapitalMetres;
            // 
            // iCapitalOnReefMetres
            // 
            this.iCapitalOnReefMetres.Name = "iCapitalOnReefMetres";
            this.iCapitalOnReefMetres.Properties.Caption = "Capital On Reef Metres";
            this.iCapitalOnReefMetres.Properties.FieldName = "CapitalOnReefMetres";
            this.iCapitalOnReefMetres.Properties.RowEdit = this.rpCapitalOnReefMetres;
            // 
            // iCapitalOffReefMetres
            // 
            this.iCapitalOffReefMetres.Name = "iCapitalOffReefMetres";
            this.iCapitalOffReefMetres.Properties.Caption = "Capital Off Reef Metres";
            this.iCapitalOffReefMetres.Properties.FieldName = "CapitalOffReefMetres";
            this.iCapitalOffReefMetres.Properties.RowEdit = this.rpCapitalOffReefMetres;
            // 
            // iEquivMetres
            // 
            this.iEquivMetres.Name = "iEquivMetres";
            this.iEquivMetres.Properties.Caption = "Equiv Metres";
            this.iEquivMetres.Properties.FieldName = "EquivMetres";
            this.iEquivMetres.Properties.RowEdit = this.rpEquivMetres;
            // 
            // iEquivOnReefMetres
            // 
            this.iEquivOnReefMetres.Name = "iEquivOnReefMetres";
            this.iEquivOnReefMetres.Properties.Caption = "Equiv On Reef Metres";
            this.iEquivOnReefMetres.Properties.FieldName = "EquivOnReefMetres";
            this.iEquivOnReefMetres.Properties.RowEdit = this.rpEquivOnReefMetres;
            // 
            // iEquivOffReefMetres
            // 
            this.iEquivOffReefMetres.Name = "iEquivOffReefMetres";
            this.iEquivOffReefMetres.Properties.Caption = "Equiv Off Reef Metres";
            this.iEquivOffReefMetres.Properties.FieldName = "EquivOffReefMetres";
            this.iEquivOffReefMetres.Properties.RowEdit = this.rpEquivOffReefMetres;
            // 
            // iTonsDev
            // 
            this.iTonsDev.Name = "iTonsDev";
            this.iTonsDev.Properties.Caption = "Tons";
            this.iTonsDev.Properties.FieldName = "Tons";
            this.iTonsDev.Properties.RowEdit = this.rpTonsDev;
            // 
            // iOnReefTonsDev
            // 
            this.iOnReefTonsDev.Name = "iOnReefTonsDev";
            this.iOnReefTonsDev.Properties.Caption = "On Reef Tons";
            this.iOnReefTonsDev.Properties.FieldName = "OnReefTons";
            this.iOnReefTonsDev.Properties.RowEdit = this.rpOnReefTonsDev;
            // 
            // iOffReefTonsDev
            // 
            this.iOffReefTonsDev.Name = "iOffReefTonsDev";
            this.iOffReefTonsDev.Properties.Caption = "Off Reef Tons";
            this.iOffReefTonsDev.Properties.FieldName = "OffReefTons";
            this.iOffReefTonsDev.Properties.RowEdit = this.rpOffReefTonsDev;
            // 
            // iGTDev
            // 
            this.iGTDev.Name = "iGTDev";
            this.iGTDev.Properties.Caption = "g/t";
            this.iGTDev.Properties.FieldName = "gt";
            this.iGTDev.Properties.RowEdit = this.rpGTDev;
            // 
            // iCMGTDev
            // 
            this.iCMGTDev.Name = "iCMGTDev";
            this.iCMGTDev.Properties.Caption = "Cmgt";
            this.iCMGTDev.Properties.FieldName = "cmgt";
            this.iCMGTDev.Properties.RowEdit = this.rpCMGTDev;
            // 
            // iKgDev
            // 
            this.iKgDev.Name = "iKgDev";
            this.iKgDev.Properties.Caption = "Ounces";
            this.iKgDev.Properties.FieldName = "KG";
            this.iKgDev.Properties.RowEdit = this.rpKGDev;
            // 
            // iCMKGTDev
            // 
            this.iCMKGTDev.IsChildRowsLoaded = true;
            this.iCMKGTDev.Name = "iCMKGTDev";
            this.iCMKGTDev.Properties.Caption = "Cmkgt";
            this.iCMKGTDev.Properties.FieldName = "cmkgt";
            this.iCMKGTDev.Properties.RowEdit = this.rpCMKGT;
            this.iCMKGTDev.Visible = false;
            // 
            // iUraniumKgDev
            // 
            this.iUraniumKgDev.IsChildRowsLoaded = true;
            this.iUraniumKgDev.Name = "iUraniumKgDev";
            this.iUraniumKgDev.Properties.Caption = "Uranium Kg";
            this.iUraniumKgDev.Properties.FieldName = "UraniumKg";
            this.iUraniumKgDev.Properties.RowEdit = this.rpUraniumKg;
            this.iUraniumKgDev.Visible = false;
            // 
            // iCubicMetresDev
            // 
            this.iCubicMetresDev.Name = "iCubicMetresDev";
            this.iCubicMetresDev.Properties.Caption = "Cubic Metres";
            this.iCubicMetresDev.Properties.FieldName = "CubicMetres";
            this.iCubicMetresDev.Properties.RowEdit = this.rpCubicMetresDev;
            this.iCubicMetresDev.Visible = false;
            // 
            // iCubicTonsDev
            // 
            this.iCubicTonsDev.Name = "iCubicTonsDev";
            this.iCubicTonsDev.Properties.Caption = "Cubic Tons";
            this.iCubicTonsDev.Properties.FieldName = "CubicTons";
            this.iCubicTonsDev.Properties.RowEdit = this.rpCubicTonsDev;
            this.iCubicTonsDev.Visible = false;
            // 
            // iCubicKgDev
            // 
            this.iCubicKgDev.Name = "iCubicKgDev";
            this.iCubicKgDev.Properties.Caption = "Cubic Kg";
            this.iCubicKgDev.Properties.FieldName = "CubicKg";
            this.iCubicKgDev.Properties.RowEdit = this.rpCubicKgDev;
            this.iCubicKgDev.Visible = false;
            // 
            // iCubicgtDev
            // 
            this.iCubicgtDev.Name = "iCubicgtDev";
            this.iCubicgtDev.Properties.Caption = "Cubic g/t";
            this.iCubicgtDev.Properties.FieldName = "CubicGt";
            this.iCubicgtDev.Properties.RowEdit = this.rpCubicgtDev;
            this.iCubicgtDev.Visible = false;
            // 
            // iDrillRigg
            // 
            this.iDrillRigg.Name = "iDrillRigg";
            this.iDrillRigg.Properties.Caption = "Drill Rigg";
            this.iDrillRigg.Properties.FieldName = "DrillRigg";
            this.iDrillRigg.Properties.RowEdit = this.rpDrillRigg;
            this.iDrillRigg.Visible = false;
            // 
            // rcPlanning
            // 
            this.rcPlanning.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.DarkBlue;
            this.rcPlanning.ExpandCollapseItem.Id = 0;
            this.rcPlanning.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcPlanning.ExpandCollapseItem,
            this.rcPlanning.SearchEditItem,
            this.btnShow,
            this.btnClose});
            this.rcPlanning.Location = new System.Drawing.Point(0, 0);
            this.rcPlanning.MaxItemId = 3;
            this.rcPlanning.Name = "rcPlanning";
            this.rcPlanning.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.rcPlanning.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.rcPlanning.Size = new System.Drawing.Size(1290, 110);
            this.rcPlanning.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // btnShow
            // 
            this.btnShow.Caption = "Show";
            this.btnShow.Id = 1;
            this.btnShow.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.ZoomBlue22;
            this.btnShow.Name = "btnShow";
            this.btnShow.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnShow_ItemClick);
            // 
            // btnClose
            // 
            this.btnClose.Caption = "Close";
            this.btnClose.Id = 2;
            this.btnClose.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.close;
            this.btnClose.Name = "btnClose";
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.Report;
            this.ribbonPage1.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Planning Report";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnShow);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnClose);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Options";
            // 
            // pcReport2
            // 
            this.pcReport2.BackColor = System.Drawing.Color.Transparent;
            this.pcReport2.Buttons = ((FastReport.PreviewButtons)(((((((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save) 
            | FastReport.PreviewButtons.Email) 
            | FastReport.PreviewButtons.Find) 
            | FastReport.PreviewButtons.Zoom) 
            | FastReport.PreviewButtons.Outline) 
            | FastReport.PreviewButtons.Navigator)));
            this.pcReport2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcReport2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pcReport2.Location = new System.Drawing.Point(378, 110);
            this.pcReport2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.pcReport2.Name = "pcReport2";
            this.pcReport2.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReport2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReport2.SaveInitialDirectory = null;
            this.pcReport2.Size = new System.Drawing.Size(912, 617);
            this.pcReport2.TabIndex = 6;
            this.pcReport2.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            // 
            // ucPlanningReportNewStyle
            // 
            this.Appearance.ForeColor = System.Drawing.Color.White;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.Controls.Add(this.pcReport2);
            this.Controls.Add(this.pgPlanSettings);
            this.Controls.Add(this.rcPlanning);
            this.Name = "ucPlanningReportNewStyle";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(1290, 727);
            this.Load += new System.EventHandler(this.ucPlanningReportNewStyle_Load_1);
            this.Controls.SetChildIndex(this.rcPlanning, 0);
            this.Controls.SetChildIndex(this.pgPlanSettings, 0);
            this.Controls.SetChildIndex(this.pcReport2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pgPlanSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpM2M)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpProdmonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpActivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSectionid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSummaryon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpProdSupervisor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMiner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpWorkplace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpWorkplaceid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpStopped)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpDayCrew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpAfternoonCrew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpNightCrew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpRovingCrew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCrewStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSQM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefSqm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefSqm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpFaceLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefFL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefFL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpAdvance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefAdvance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefAdvance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpChannelWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpChannelWidthTons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpStopeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIdealStopeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefTons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefTons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpGT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCMGT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpKG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCMKGT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpUraniumKg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicmetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicTons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicKg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicGt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpRemarks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpPrimSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTotalMetersIncl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTotalMetersExcl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMainAdvance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMainOnReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMainOffReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSecMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSecOnReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSecOffReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCapitalMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCapitalOnReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCapitalOffReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEquivMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEquivOnReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEquivOffReefMetres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpDrillRigg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOnReefTonsDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpOffReefTonsDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpGTDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCMGTDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpKGDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicMetresDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicTonsDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicKgDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCubicgtDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpRemarksDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTonsDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMiningMethod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpShowAurt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcPlanning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private DataTable Get_ReportDetail_Info()
		{
			MWDataManager.clsDataAccess _ReportDetail = new MWDataManager.clsDataAccess();

			_ReportDetail.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
			_ReportDetail.SqlStatement = "Select 1 Pos,'All' RepDetail " +
											"union " +
											"Select 2 Pos,'Summary' " +
											"union " +
											"Select 3 Pos,'Section Detail'";
			_ReportDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

			_ReportDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
			_ReportDetail.ExecuteInstruction();

			return _ReportDetail.ResultsDataTable;
		}

		private void ucPlanningReportNewStyle_Load_1(object sender, EventArgs e)
		{

		
			iCMKGT.Visible = false;
			iCMKGTDev.Visible = false;
			iUraniumKg.Visible = false;
			iUraniumKgDev.Visible = false;
			
			reportSettings.ProdSupervisor = true;
			reportSettings.SQM = true;
			reportSettings.OnReefSqm = true;
			reportSettings.OffReefSqm = true;
			reportSettings.TargetSqm = true;
			reportSettings.Facelength = true;
			reportSettings.OnReefFL = true;
			reportSettings.OffReefFL = true;
			reportSettings.Tons = true;
			reportSettings.OnReefTons = true;
			reportSettings.OffReefTons = true;
			reportSettings.gt = true;
			reportSettings.cmgt = true;
			reportSettings.KG = true;
			reportSettings.cmkgt = true;
			reportSettings.UraniumKg = true;
			reportSettings.CubicMetres = true;
			reportSettings.CubicKg = true;
			reportSettings.CubicTons = true;
			reportSettings.CubicGt = true;
			reportSettings.PrimSec = true;
			reportSettings.TotalMetresIncl = true;
			reportSettings.TotalMetresExcl = true;
			reportSettings.OnReefMetres = true;
			reportSettings.OffReefMetres = true;
			reportSettings.MainAdvance = true;
			reportSettings.MainOnReefMetres = true;
			reportSettings.MainOffReefMetres = true;
			reportSettings.SecMetres = true;
			reportSettings.SecOnReefMetres = true;
			reportSettings.SecOffReefMetres = true;
			reportSettings.CapitalMetres = true;
			reportSettings.CapitalOnReefMetres = true;
			reportSettings.CapitalOffReefMetres = true;
			reportSettings.EquivMetres = true;
			reportSettings.EquivOnReefMetres = true;
			reportSettings.EquivOffReefMetres = true;
			reportSettings.DrillRigg = true;
			reportSettings.Miner = true;
			reportSettings.Workplace = true;
			reportSettings.Workplaceid = true;
			reportSettings.Stopped = true;
			reportSettings.DayCrew = true;
			reportSettings.AfternoonCrew = true;
			reportSettings.NightCrew = true;
			reportSettings.RovingCrew = true;
			reportSettings.CrewStrength = true;
			reportSettings.MiningMethod = true;
			reportSettings.HierarchicalID = 2;
			reportSettings.setUserCurrentInfo(this.UserCurrentInfo);
			reportSettings.setSystemDBTag(this.theSystemDBTag);
			reportSettings.UpdateSumOnRequest += reportSettings_UpdateSumOnRequest;
			reportSettings.UpdateActivitySelectionRequest += reportSettings_UpdateActivitySelectionRequest;
			reportSettings.OnUpdateMonthRequest += reportSettings_UpdateMonthRequest;

			reportSettings.pmonth = ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString();
			int theYear = Convert.ToInt32(reportSettings.pmonth.Substring(0, 4));
			int theMonth = Convert.ToInt32(reportSettings.pmonth.Substring(4, 2));
			DateTime theDate = new DateTime(theYear, theMonth, 1);
			reportSettings.Prodmonth = theDate.Date;

			iDevelopment.Visible = false;
			iStoping.Visible = false;
            TProductionAmplatsGlobalDataTables BMEBL = new TProductionAmplatsGlobalDataTables();			

			DataTable dtActivityData = BMEBL.GetActivity();

			rpActivity.DataSource = dtActivityData;
			rpActivity.DisplayMember = "Description";
			rpActivity.ValueMember = "Activity";

			if (BMEBL.GetPlan() == true)
			{
				rpPlan.DataSource = BMEBL.ResultsDataTable;
				rpPlan.DisplayMember = "Descd";
				rpPlan.ValueMember = "Code";
			}

            reportSettings.Prodmonth = procs.ProdMonthAsDate(ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString());

			reportSettings.ActivityType = "0";
			reportSettings.PlanType = "1";

			pgPlanSettings.SelectedObject = reportSettings;

			if (reportSettings.ActivityType == "")
			{
				MessageBox.Show("Please select a value", "", MessageBoxButtons.OK);
				result = false;
			}
			else if (reportSettings.PlanType == "")
			{
				MessageBox.Show("Please select a value", "", MessageBoxButtons.OK);
				result = false;
			}
			else if (reportSettings.SectionID == "")
			{
				MessageBox.Show("Please select a value", "", MessageBoxButtons.OK);
				result = false;
			}
			else if (Convert.ToString(reportSettings.HierarchicalID) == "")
			{
				MessageBox.Show("Please select a value", "", MessageBoxButtons.OK);
				result = false;
			}
			else
				result = true;
		}

		void reportSettings_UpdateActivitySelectionRequest(object sender, PlanningReportNewStyleProperties.UpdateActivitySelectionArg e)
		{
			iDevelopment.Visible = e.Dev;
			iStoping.Visible = e.Stoping;
		}

		void reportSettings_UpdateMonthRequest(object sender, PlanningReportNewStyleProperties.UpdateMonthArg e)
		{
			SectionsLoad();
		}

		public void SectionsLoad()
		{
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.SqlStatement = " select distinct 'Shift Boss' Lvl, b.SectionID_1 As sectionid, NAME_1 As NAME, 4 OrderBy from \r\n " +
                                     " tbl_planmonth a  inner join sections_complete b\r\n " +
                                     " on a.prodmonth = b.prodmonth\r\n " +
                                    "  and a.sectionid = b.sectionid\r\n " +
                                    "  Where a.Prodmonth = '" + procs.ProdMonthAsString(reportSettings.Prodmonth)+"'\r\n " +
                                    "  union\r\n " +
                                    "  select distinct 'MO' Lvl,b.SectionID_2 As sectionid, NAME_2 As NAME, 3 OrderBy from\r\n " +
                                     " tbl_planmonth a  inner join sections_complete b\r\n " +
                                    "  on a.prodmonth = b.prodmonth\r\n " +
                                    "  and a.sectionid = b.sectionid\r\n " +
                                     " Where a.Prodmonth = '" + procs.ProdMonthAsString(reportSettings.Prodmonth)+"'\r\n " +
                                     " union\r\n " +
                                    "  select distinct 'Section Manager' Lvl,b.SectionID_3 As sectionid, NAME_3 As NAME, 2 OrderBy from\r\n " +
                                    "  tbl_planmonth a  inner join sections_complete b\r\n " +
                                    "  on a.prodmonth = b.prodmonth\r\n " +
                                    "  and a.sectionid = b.sectionid\r\n " +
                                    "  Where a.Prodmonth = '" + procs.ProdMonthAsString(reportSettings.Prodmonth)+"'\r\n " +
                                    "  union\r\n " +
                                    "  select distinct 'Production Manager' Lvl,b.SectionID_4 As sectionid, NAME_4 As NAME, 1 OrderBy from\r\n " +
                                     " tbl_planmonth a  inner join sections_complete b\r\n " +
                                    "  on a.prodmonth = b.prodmonth\r\n " +
                                    "  and a.sectionid = b.sectionid\r\n " +
                                    "  Where a.Prodmonth = '" + procs.ProdMonthAsString(reportSettings.Prodmonth)+"'\r\n " +
                                    "   union\r\n " +
                                    "  select distinct 'Mine' Lvl,b.SectionID_5 As sectionid, NAME_5 As NAME, 0 OrderBy from\r\n " +
                                    "  tbl_planmonth a  inner join sections_complete b\r\n " +
                                    "  on a.prodmonth = b.prodmonth\r\n " +
                                    "  and a.sectionid = b.sectionid \r\n " +
                                    "  Where a.Prodmonth = '" + procs.ProdMonthAsString(reportSettings.Prodmonth)+"' \r\n " +
                                    " order by OrderBy ";
            theData.ExecuteInstruction();
            section = null;
            section = theData.ResultsDataTable;
            rpSectionid.DataSource = section;
            rpSectionid.DisplayMember = "NAME";
            rpSectionid.ValueMember = "sectionid";           

            reportSettings.SectionID = section.Rows[0]["sectionid"].ToString();
            reportSettings.NAME = section.Rows[0]["NAME"].ToString();
        }

		void reportSettings_UpdateSumOnRequest(object sender, PlanningReportNewStyleProperties.UpdateSumOnArg e)
		{
			pgPlanSettings.PostEditor();


			DataTable dtlevels = reportSettings.LoadAllLevel();

			reportSettings.pmonth = ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString();
			rpSummaryon.DataSource = dtlevels;

			if (dtlevels != null)
			{
				rpSummaryon.DisplayMember = "Description";
				rpSummaryon.ValueMember = "HierarchicalID";
			}

			//reportSettings.HierarchicalID = reportSettings.SumOn.Rows[0]["HierarchicalID"].ToString();
		}

		private void rpSectionid_EditValueChanged(object sender, EventArgs e)
		{
            //reportSettings.SectionID = reportSettings.NAME;
            pgPlanSettings.FocusNext();
            pgPlanSettings.PostEditor();
            reportSettings.SectionID = reportSettings.NAME;
        }

		private void pgPlanSettings_Click(object sender, EventArgs e)
		{

		}

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            theReportThread = new Thread(new ParameterizedThreadStart(createReport));
            theReportThread.SetApartmentState(ApartmentState.STA);
            theReportThread.Start(reportSettings);
            if (reportSettings.HierarchicalID == 0)
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Error", "Please select a Summary On level", Color.Red);

                return;
            }

            if (reportSettings.ActivityType == null)
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Error", "Please make a user selection for the report", Color.Red);

               
            }
            else
            {
                if (reportSettings.PlanType == null)
                {
                    Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Error", "Please make a user selection for the report", Color.Red);
                    return;

                }
                else
                {
                    if (reportSettings.SectionID == null)
                    {
                        Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Error", "Please make a user selection for the report", Color.Red);
                        return;

                    }
                   
                }
            }

            dtReportTables = new DataTable();
            dtReportTables.Clear();
            dtSections.Clear();

            if (Convert.ToBoolean(reportSettings.ShowAuth) == false)
                ShowAuth = "N";
            else
                ShowAuth = "Y";

            if (Convert.ToBoolean(reportSettings.ProdSupervisor) == false)
                ProdSupervisor = "N";
            else
                ProdSupervisor = "Y";

            if (Convert.ToBoolean(reportSettings.Miner) == false)
                Miner = "N";
            else
                Miner = "Y";

            if (Convert.ToBoolean(reportSettings.Workplace) == false)
                Workplace = "N";
            else
                Workplace = "Y";

            if (Convert.ToBoolean(reportSettings.Workplaceid) == false)
                Workplaceid = "N";
            else
                Workplaceid = "Y";

            if (Convert.ToBoolean(reportSettings.Stopped) == false)
                Stopped = "N";
            else
                Stopped = "Y";

            if (Convert.ToBoolean(reportSettings.DayCrew) == false)
                DayCrew = "N";
            else
                DayCrew = "Y";

            if (Convert.ToBoolean(reportSettings.AfternoonCrew) == false)
                AfternoonCrew = "N";
            else
                AfternoonCrew = "Y";

            if (Convert.ToBoolean(reportSettings.NightCrew) == false)
                NightCrew = "N";
            else
                NightCrew = "Y";

            if (Convert.ToBoolean(reportSettings.RovingCrew) == false)
                RovingCrew = "N";
            else
                RovingCrew = "Y";

            if (Convert.ToBoolean(reportSettings.CrewStrength) == false)
                CrewStrength = "N";
            else
                CrewStrength = "Y";

            if (Convert.ToBoolean(reportSettings.MiningMethod) == false)
                MiningMethod = "N";
            else
                MiningMethod = "Y";

            if (reportSettings.ActivityType == "0")
            {
                if (Convert.ToBoolean(reportSettings.SQM) == false)
                    SQM = "N";
                else
                    SQM = "Y";

                if (Convert.ToBoolean(reportSettings.OnReefSqm) == false)
                    OnReefSqm = "N";
                else
                    OnReefSqm = "Y";

                if (Convert.ToBoolean(reportSettings.OffReefSqm) == false)
                    OffReefSqm = "N";
                else
                    OffReefSqm = "Y";

                if (Convert.ToBoolean(reportSettings.Facelength) == false)
                    Facelength = "N";
                else
                    Facelength = "Y";

                if (Convert.ToBoolean(reportSettings.OnReefFL) == false)
                    OnReefFL = "N";
                else
                    OnReefFL = "Y";

                if (Convert.ToBoolean(reportSettings.OffReefFL) == false)
                    OffReefFL = "N";
                else
                    OffReefFL = "Y";

                if (Convert.ToBoolean(reportSettings.Advance) == false)
                    Advance = "N";
                else
                    Advance = "Y";

                if (Convert.ToBoolean(reportSettings.OnReefAdvance) == false)
                    OnReefAdvance = "N";
                else
                    OnReefAdvance = "Y";

                if (Convert.ToBoolean(reportSettings.OffReefAdvance) == false)
                    OffReefAdvance = "N";
                else
                    OffReefAdvance = "Y";

                if (Convert.ToBoolean(reportSettings.ChannelWidth) == false)
                    ChannelWidth = "N";
                else
                    ChannelWidth = "Y";

                if (Convert.ToBoolean(reportSettings.ChannelWidthTons) == false)
                    ChannelWidthTons = "N";
                else
                    ChannelWidthTons = "Y";

                if (Convert.ToBoolean(reportSettings.StopeWidth) == false)
                    StopeWidth = "N";
                else
                    StopeWidth = "Y";

                if (Convert.ToBoolean(reportSettings.IdealStopeWidth) == false)
                    IdealStopeWidth = "N";
                else
                    IdealStopeWidth = "Y";

                if (Convert.ToBoolean(reportSettings.Tons) == false)
                    Tons = "N";
                else
                    Tons = "Y";

                if (Convert.ToBoolean(reportSettings.OnReefTons) == false)
                    OnReefTons = "N";
                else
                    OnReefTons = "Y";

                if (Convert.ToBoolean(reportSettings.OffReefTons) == false)
                    OffReefTons = "N";
                else
                    OffReefTons = "Y";

                if (Convert.ToBoolean(reportSettings.gt) == false)
                    gt = "N";
                else
                    gt = "Y";

                if (Convert.ToBoolean(reportSettings.cmgt) == false)
                    cmgt = "N";
                else
                    cmgt = "Y";

                if (Convert.ToBoolean(reportSettings.KG) == false)
                    KG = "N";
                else
                    KG = "Y";

                if (Convert.ToBoolean(reportSettings.CubicMetres) == false)
                    CubicMetres = "N";
                else
                    CubicMetres = "Y";

                if (Convert.ToBoolean(reportSettings.CubicTons) == false)
                    CubicTons = "N";
                else
                    CubicTons = "Y";

                if (Convert.ToBoolean(reportSettings.CubicKg) == false)
                    CubicKg = "N";
                else
                    CubicKg = "Y";

                if (Convert.ToBoolean(reportSettings.CubicGt) == false)
                    CubicGt = "N";
                else
                    CubicGt = "Y";

                if (Convert.ToBoolean(reportSettings.cmkgt) == false)
                    cmkgt = "N";
                else
                    cmkgt = "Y";



            }

            if (reportSettings.ActivityType == "1")
            {
                if (Convert.ToBoolean(reportSettings.PrimSec) == false)
                    PrimSec = "N";
                else
                    PrimSec = "Y";

                if (Convert.ToBoolean(reportSettings.TotalMetresIncl) == false)
                    TotalMetersIncl = "N";
                else
                    TotalMetersIncl = "Y";

                if (Convert.ToBoolean(reportSettings.TotalMetresExcl) == false)
                    TotalMetresExcl = "N";
                else
                    TotalMetresExcl = "Y";

                if (Convert.ToBoolean(reportSettings.OnReefMetres) == false)
                    OnReefMetres = "N";
                else
                    OnReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.OffReefMetres) == false)
                    OffReefMetres = "N";
                else
                    OffReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.MainAdvance) == false)
                    MainAdvance = "N";
                else
                    MainAdvance = "Y";

                if (Convert.ToBoolean(reportSettings.MainOnReefMetres) == false)
                    MainOnReefMetres = "N";
                else
                    MainOnReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.MainOffReefMetres) == false)
                    MainOffReefMetres = "N";
                else
                    MainOffReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.SecMetres) == false)
                    SecMetres = "N";
                else
                    SecMetres = "Y";

                if (Convert.ToBoolean(reportSettings.SecOnReefMetres) == false)
                    SecOnReefMetres = "N";
                else
                    SecOnReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.SecOffReefMetres) == false)
                    SecOffReefMetres = "N";
                else
                    SecOffReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.CapitalMetres) == false)
                    CapitalMetres = "N";
                else
                    CapitalMetres = "Y";

                if (Convert.ToBoolean(reportSettings.CapitalOnReefMetres) == false)
                    CapitalOnReefMetres = "N";
                else
                    CapitalOnReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.CapitalOffReefMetres) == false)
                    CapitalOffReefMetres = "N";
                else
                    CapitalOffReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.EquivMetres) == false)
                    EquivMetres = "N";
                else
                    EquivMetres = "Y";

                if (Convert.ToBoolean(reportSettings.EquivOnReefMetres) == false)
                    EquivOnReefMetres = "N";
                else
                    EquivOnReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.EquivOffReefMetres) == false)
                    EquivOffReefMetres = "N";
                else
                    EquivOffReefMetres = "Y";

                if (Convert.ToBoolean(reportSettings.DrillRigg) == false)
                    DrillRigg = "N";
                else
                    DrillRigg = "Y";

                if (Convert.ToBoolean(reportSettings.Tons) == false)
                    Tons = "N";
                else
                    Tons = "Y";

                if (Convert.ToBoolean(reportSettings.OnReefTons) == false)
                    OnReefTons = "N";
                else
                    OnReefTons = "Y";

                if (Convert.ToBoolean(reportSettings.OffReefTons) == false)
                    OffReefTons = "N";
                else
                    OffReefTons = "Y";

                if (Convert.ToBoolean(reportSettings.gt) == false)
                    gt = "N";
                else
                    gt = "Y";

                if (Convert.ToBoolean(reportSettings.cmgt) == false)
                    cmgt = "N";
                else
                    cmgt = "Y";

                if (Convert.ToBoolean(reportSettings.KG) == false)
                    KG = "N";
                else
                    KG = "Y";

                if (Convert.ToBoolean(reportSettings.cmkgt) == false)
                    cmkgt = "N";
                else
                    cmkgt = "Y";




                if (Convert.ToBoolean(reportSettings.CubicMetres) == false)
                    CubicMetres = "N";
                else
                    CubicMetres = "Y";

                if (Convert.ToBoolean(reportSettings.CubicTons) == false)
                    CubicTons = "N";
                else
                    CubicTons = "Y";

                if (Convert.ToBoolean(reportSettings.CubicKg) == false)
                    CubicKg = "N";
                else
                    CubicKg = "Y";

                if (Convert.ToBoolean(reportSettings.CubicGt) == false)
                    CubicGt = "N";
                else
                    CubicGt = "Y";

            }
            string theProdMonth = reportSettings.Prodmonth.ToString("yyyyMM");


            MWDataManager.clsDataAccess _dbManBanner = new MWDataManager.clsDataAccess();
            _dbManBanner.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbManBanner.SqlStatement = " select Banner from tbl_Sysset ";

            _dbManBanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManBanner.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManBanner.ResultsTableName = "Banner";
            _dbManBanner.ExecuteInstruction();


            DataSet DsBanner = new DataSet();
            DsBanner.Tables.Add(_dbManBanner.ResultsDataTable);



            MWDataManager.clsDataAccess _PlanningReport_Stopping = new MWDataManager.clsDataAccess();
            MWDataManager.clsDataAccess _PlanningReport_Stopping_summary = new MWDataManager.clsDataAccess();

            if (reportSettings.ActivityType == "0")
            {
                if (reportSettings.PlanType == "0")
                {
                    try
                    {
                        _PlanningReport_Stopping_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Stopping_summary.SqlStatement = "SP_Planning_Stope_Summary";
                        _PlanningReport_Stopping_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Stopping_summary.ResultsTableName = "PlanningReportStoppingSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                                _PlanningReport_Stopping_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                                _PlanningReport_Stopping_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID   ),
                                _PlanningReport_Stopping_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                                _PlanningReport_Stopping_summary.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),

                        };
                        _PlanningReport_Stopping_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Stopping_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Stopping_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from tbl_planmonth a ");
                    sb.AppendLine("inner join sections_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        foreach (DataRow ds in dtSections.Rows)
                        {


                            _PlanningReport_Stopping.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                            _PlanningReport_Stopping.SqlStatement = "SP_Planning_Stope";
                            _PlanningReport_Stopping.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                            _PlanningReport_Stopping.ResultsTableName = "PlanningReportStopping";

                            SqlParameter[] _paramCollection9 =
                            {
                                _PlanningReport_Stopping.CreateParameter("@Prodmonth", SqlDbType.Int , 50,procs.ProdMonthAsInt(reportSettings.Prodmonth) ),
                                _PlanningReport_Stopping.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID  ),
                                _PlanningReport_Stopping.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),

                            };
                            _PlanningReport_Stopping.ParamCollection = _paramCollection9;
                            _PlanningReport_Stopping.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _PlanningReport_Stopping.ExecuteInstruction();

                            DataTable dtPlanningStoping = new DataTable();
                            dtPlanningStoping = _PlanningReport_Stopping.ResultsDataTable;

                            dtReportTables.Merge(dtPlanningStoping);
                            dtReportTables.AcceptChanges();
                        }
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }
                }

                if (reportSettings.PlanType == "1")
                {
                    try
                    {
                        _PlanningReport_Stopping_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Stopping_summary.SqlStatement = "SP_Dynamic_Planning_Stope_Summary";
                        _PlanningReport_Stopping_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Stopping_summary.ResultsTableName = "PlanningReportStoppingSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                                _PlanningReport_Stopping_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                                _PlanningReport_Stopping_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID   ),
                                _PlanningReport_Stopping_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                                _PlanningReport_Stopping_summary.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),

                        };
                        _PlanningReport_Stopping_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Stopping_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Stopping_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from tbl_planmonth a ");
                    sb.AppendLine("inner join sections_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' and Activity in (0,3) ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        _PlanningReport_Stopping.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Stopping.SqlStatement = "SP_Dynamic_Planning_Stope";
                        _PlanningReport_Stopping.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Stopping.ResultsTableName = "PlanningReportStopping";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Stopping.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                            _PlanningReport_Stopping.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID ),
                            _PlanningReport_Stopping.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),
                        };
                        _PlanningReport_Stopping.ParamCollection = _paramCollection9;
                        _PlanningReport_Stopping.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Stopping.ExecuteInstruction();

                        dtReportTables = _PlanningReport_Stopping.ResultsDataTable;
                        dtReportTables.TableName = "Table2";
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }
                }

                if (reportSettings.PlanType == "2")
                {
                    try
                    {
                        _PlanningReport_Stopping_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Stopping_summary.SqlStatement = "SP_Lock_Planning_Stope_Summary";
                        _PlanningReport_Stopping_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Stopping_summary.ResultsTableName = "PlanningReportStoppingSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                                _PlanningReport_Stopping_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                                _PlanningReport_Stopping_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID  ),
                                _PlanningReport_Stopping_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                                _PlanningReport_Stopping_summary.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),

                        };
                        _PlanningReport_Stopping_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Stopping_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Stopping_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from tbl_planmonth a ");
                    sb.AppendLine("inner join sections_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        _PlanningReport_Stopping.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Stopping.SqlStatement = "SP_Lock_Planning_Stope";
                        _PlanningReport_Stopping.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Stopping.ResultsTableName = "PlanningReportStopping";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Stopping.CreateParameter("@Prodmonth", SqlDbType.VarChar, 6, theProdMonth),
                            _PlanningReport_Stopping.CreateParameter("@SectionID", SqlDbType.VarChar, 0,reportSettings.SectionID),
                            _PlanningReport_Stopping.CreateParameter("@ShowAuth", SqlDbType.VarChar, 0,ShowAuth)

                        };

                        _PlanningReport_Stopping.ParamCollection = _paramCollection9;
                        _PlanningReport_Stopping.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Stopping.ExecuteInstruction();

                        dtReportTables = _PlanningReport_Stopping.ResultsDataTable;
                        dtReportTables.TableName = "Table2";
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }
                }

                if (reportSettings.PlanType == "3")
                {
                    try
                    {
                        _PlanningReport_Stopping_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Stopping_summary.SqlStatement = "SP_Dynamic_Planning_Stope_Summary_Cycle";
                        _PlanningReport_Stopping_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Stopping_summary.ResultsTableName = "PlanningReportStoppingSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                                _PlanningReport_Stopping_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                                _PlanningReport_Stopping_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID  ),
                                _PlanningReport_Stopping_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                                _PlanningReport_Stopping_summary.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),

                        };
                        _PlanningReport_Stopping_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Stopping_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Stopping_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from tbl_planmonth a ");
                    sb.AppendLine("inner join sections_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' and Activity in (0,3) ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        _PlanningReport_Stopping.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Stopping.SqlStatement = "SP_Dynamic_Planning_Stope_Cycle";
                        _PlanningReport_Stopping.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Stopping.ResultsTableName = "PlanningReportStopping";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Stopping.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                            _PlanningReport_Stopping.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID    ),
                            _PlanningReport_Stopping.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),
                        };
                        _PlanningReport_Stopping.ParamCollection = _paramCollection9;
                        _PlanningReport_Stopping.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Stopping.ExecuteInstruction();

                        dtReportTables = _PlanningReport_Stopping.ResultsDataTable;
                        dtReportTables.TableName = "Table2";
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }
                }

                if (reportSettings.PlanType == "4")
                {
                    try
                    {
                        _PlanningReport_Stopping_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Stopping_summary.SqlStatement = "SP_Lock_Planning_Stope_Summary_Cycle";
                        _PlanningReport_Stopping_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Stopping_summary.ResultsTableName = "PlanningReportStoppingSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                                _PlanningReport_Stopping_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                                _PlanningReport_Stopping_summary.CreateParameter("@Section", SqlDbType.VarChar  , 0,reportSettings.SectionID   ),
                                _PlanningReport_Stopping_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                                _PlanningReport_Stopping_summary.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),

                        };
                        _PlanningReport_Stopping_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Stopping_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Stopping_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from tbl_planmonth a ");
                    sb.AppendLine("inner join sections_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        _PlanningReport_Stopping.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Stopping.SqlStatement = "SP_Lock_Planning_Stope_Cycle";
                        _PlanningReport_Stopping.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Stopping.ResultsTableName = "PlanningReportStopping";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Stopping.CreateParameter("@Prodmonth", SqlDbType.VarChar, 6, theProdMonth),
                            _PlanningReport_Stopping.CreateParameter("@SectionID", SqlDbType.VarChar, 0,reportSettings.SectionID),
                            _PlanningReport_Stopping.CreateParameter("@ShowAuth", SqlDbType.VarChar, 0,ShowAuth)

                        };

                        _PlanningReport_Stopping.ParamCollection = _paramCollection9;
                        _PlanningReport_Stopping.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Stopping.ExecuteInstruction();

                        dtReportTables = _PlanningReport_Stopping.ResultsDataTable;
                        dtReportTables.TableName = "Table2";
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }
                }
            }

            MWDataManager.clsDataAccess _PlanningReport_Dev = new MWDataManager.clsDataAccess();
            MWDataManager.clsDataAccess _PlanningReport_Dev_summary = new MWDataManager.clsDataAccess();

            if (reportSettings.ActivityType == "1")
            {
                if (reportSettings.PlanType == "0")
                {
                    try
                    {
                        _PlanningReport_Dev_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev_summary.SqlStatement = "SP_Planning_Dev_Summary";
                        _PlanningReport_Dev_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev_summary.ResultsTableName = "PlanningReportDevSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                                _PlanningReport_Dev_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                                _PlanningReport_Dev_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID   ),
                                _PlanningReport_Dev_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),

                        };
                        _PlanningReport_Dev_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from tbl_planmonth a ");
                    sb.AppendLine("inner join sections_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        foreach (DataRow ds in dtSections.Rows)
                        {
                            _PlanningReport_Dev.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                            _PlanningReport_Dev.SqlStatement = "SP_Planning_Dev";
                            _PlanningReport_Dev.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                            _PlanningReport_Dev.ResultsTableName = "PlanningReportDev";

                            SqlParameter[] _paramCollection9 =
                            {
                                _PlanningReport_Dev.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                                _PlanningReport_Dev.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,ds["Name"].ToString ()   ),
                            };
                            _PlanningReport_Dev.ParamCollection = _paramCollection9;
                            _PlanningReport_Dev.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _PlanningReport_Dev.ExecuteInstruction();

                            DataTable dtDevPlanning = new DataTable();
                            dtDevPlanning = _PlanningReport_Dev.ResultsDataTable;

                            dtReportTables.Merge(dtDevPlanning);
                            dtReportTables.AcceptChanges();
                        }
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                    }
                }

                if (reportSettings.PlanType == "1")
                {
                    try
                    {
                        _PlanningReport_Dev_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev_summary.SqlStatement = "SP_Dynamic_Planning_Dev_Summary";
                        _PlanningReport_Dev_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev_summary.ResultsTableName = "PlanningReportDevSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                            _PlanningReport_Dev_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID  ),
                            _PlanningReport_Dev_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                            _PlanningReport_Dev_summary.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),

                        };
                        _PlanningReport_Dev_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from tbl_planmonth a ");
                    sb.AppendLine("inner join sections_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        _PlanningReport_Dev.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev.SqlStatement = "SP_Dynamic_Planning_Dev";
                        _PlanningReport_Dev.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev.ResultsTableName = "PlanningReportDev";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev.CreateParameter("@Prodmonth", SqlDbType.VarChar, 6, theProdMonth),
                            _PlanningReport_Dev.CreateParameter("@SectionID", SqlDbType.VarChar, 0, reportSettings.SectionID),
                            _PlanningReport_Dev.CreateParameter("@ShowAuth", SqlDbType.VarChar, 0, ShowAuth),
                        };
                        _PlanningReport_Dev.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev.ExecuteInstruction();

                        dtReportTables = _PlanningReport_Dev.ResultsDataTable;
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                    }
                }

                if (reportSettings.PlanType == "2")
                {
                    try
                    {
                        _PlanningReport_Dev_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev_summary.SqlStatement = "SP_Lock_Planning_Dev_Summary";
                        _PlanningReport_Dev_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev_summary.ResultsTableName = "PlanningReportDevSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                            _PlanningReport_Dev_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID   ),
                            _PlanningReport_Dev_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                            _PlanningReport_Dev_summary.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),
                        };

                        _PlanningReport_Dev_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from tbl_planmonth a ");
                    sb.AppendLine("inner join sections_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        _PlanningReport_Dev.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev.SqlStatement = "SP_Lock_Planning_Dev";
                        _PlanningReport_Dev.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev.ResultsTableName = "PlanningReportDev";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev.CreateParameter("@Prodmonth", SqlDbType.VarChar, 6, theProdMonth),
                            _PlanningReport_Dev.CreateParameter("@SectionID", SqlDbType.VarChar, 0,reportSettings.SectionID),
                            _PlanningReport_Dev.CreateParameter("@ShowAuth", SqlDbType.VarChar, 0,ShowAuth),
                        };

                        _PlanningReport_Dev.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev.ExecuteInstruction();

                        dtReportTables = _PlanningReport_Dev.ResultsDataTable;
                        dtReportTables.AcceptChanges();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }
                }

                if (reportSettings.PlanType == "3")
                {
                    try
                    {
                        _PlanningReport_Dev_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev_summary.SqlStatement = "SP_Dynamic_Planning_Dev_Summary_Cycle";
                        _PlanningReport_Dev_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev_summary.ResultsTableName = "PlanningReportDevSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                            _PlanningReport_Dev_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID  ),
                            _PlanningReport_Dev_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                            _PlanningReport_Dev_summary.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),

                        };
                        _PlanningReport_Dev_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from planmonth a  ");
                    sb.AppendLine("inner join section_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        _PlanningReport_Dev.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev.SqlStatement = "SP_Dynamic_Planning_Dev_Cycle";
                        _PlanningReport_Dev.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev.ResultsTableName = "PlanningReportDev";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev.CreateParameter("@Prodmonth", SqlDbType.VarChar, 6, theProdMonth),
                            _PlanningReport_Dev.CreateParameter("@SectionID", SqlDbType.VarChar, 0,reportSettings.SectionID),
                            _PlanningReport_Dev.CreateParameter("@ShowAuth", SqlDbType.VarChar, 0, ShowAuth),
                        };
                        _PlanningReport_Dev.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev.ExecuteInstruction();

                        dtReportTables = _PlanningReport_Dev.ResultsDataTable;
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                    }
                }

                if (reportSettings.PlanType == "4")
                {
                    try
                    {
                        _PlanningReport_Dev_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev_summary.SqlStatement = "SP_Lock_Planning_Dev_Summary_Cycle";
                        _PlanningReport_Dev_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev_summary.ResultsTableName = "PlanningReportDevSummary";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                            _PlanningReport_Dev_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID  ),
                            _PlanningReport_Dev_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                            _PlanningReport_Dev_summary.CreateParameter("@ShowAuth", SqlDbType.VarChar  , 0,ShowAuth   ),
                        };

                        _PlanningReport_Dev_summary.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev_summary.ExecuteInstruction();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                    }

                    GetLevel();

                    MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                    _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    sb.Clear();
                    sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                    sb.AppendLine("from planmonth a ");
                    sb.AppendLine("inner join section_complete b on ");
                    sb.AppendLine("a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' ");
                    _thelevel.SqlStatement = sb.ToString();
                    _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _thelevel.ExecuteInstruction();
                    _thelevel.ResultsTableName = "sections";
                    dtSections = _thelevel.ResultsDataTable;

                    try
                    {
                        _PlanningReport_Dev.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev.SqlStatement = "SP_Lock_Planning_Dev_Cycle";
                        _PlanningReport_Dev.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev.ResultsTableName = "PlanningReportDev";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev.CreateParameter("@Prodmonth", SqlDbType.VarChar, 6, theProdMonth),
                            _PlanningReport_Dev.CreateParameter("@SectionID", SqlDbType.VarChar, 0,reportSettings.SectionID),
                            _PlanningReport_Dev.CreateParameter("@ShowAuth", SqlDbType.VarChar, 0,ShowAuth),
                        };

                        _PlanningReport_Dev.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev.ExecuteInstruction();

                        dtReportTables = _PlanningReport_Dev.ResultsDataTable;
                        dtReportTables.AcceptChanges();
                    }
                    catch (Exception _exception)
                    {
                        throw new ApplicationException("Report Section:PlanningReportStopping:" + _exception.Message, _exception);
                    }
                }
            }

            if (reportSettings.ActivityType == "2")
            {
                string PlanCode = "";

                if (reportSettings.PlanType == "1" || reportSettings.PlanType == "3")
                    PlanCode = "MP";

                else
                    PlanCode = "LP";

                try
                {
                    _PlanningReport_Dev_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _PlanningReport_Dev_summary.SqlStatement = "SP_Planning_Sundry_Summary";
                    _PlanningReport_Dev_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                    _PlanningReport_Dev_summary.ResultsTableName = "PlanningReportSundrySummary";

                    SqlParameter[] _paramCollection9 =
                    {
                        _PlanningReport_Dev_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                        _PlanningReport_Dev_summary.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,reportSettings.SectionID   ),
                        _PlanningReport_Dev_summary.CreateParameter("@SumLevel", SqlDbType.Int  , 0,reportSettings.HierarchicalID   ),
                        _PlanningReport_Dev_summary.CreateParameter("@PlanCode", SqlDbType.VarChar, 0,PlanCode),

                    };

                    _PlanningReport_Dev_summary.ParamCollection = _paramCollection9;
                    _PlanningReport_Dev_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _PlanningReport_Dev_summary.ExecuteInstruction();
                }
                catch (Exception _exception)
                {
                    throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                }

                GetLevel();

                MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                sb.Clear();
                sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                sb.AppendLine("from PLANMONTH_SUNDRYMINING a ");
                sb.AppendLine("inner join section_complete b on ");
                sb.AppendLine("a.prodmonth = b.prodmonth and ");
                sb.AppendLine("a.sectionid = b.sectionid ");
                sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " and b." + Thelevel + "  = '" + reportSettings.section1 + "' ");
                _thelevel.SqlStatement = sb.ToString();
                _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                _thelevel.ExecuteInstruction();
                _thelevel.ResultsTableName = "sections";
                dtSections = _thelevel.ResultsDataTable;

                try
                {
                    foreach (DataRow ds in dtSections.Rows)
                    {
                        _PlanningReport_Dev.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev.SqlStatement = "SP_Planning_Sundry";
                        _PlanningReport_Dev.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev.ResultsTableName = "PlanningReportSundry";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev.CreateParameter("@Prodmonth", SqlDbType.VarChar , 6, theProdMonth),
                            _PlanningReport_Dev.CreateParameter("@SectionID", SqlDbType.VarChar  , 0,ds["Name"].ToString ()   ),
                            _PlanningReport_Dev.CreateParameter("@PlanCode", SqlDbType.VarChar, 0,PlanCode),

                        };
                        _PlanningReport_Dev.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev.ExecuteInstruction();

                        DataTable dtDevPlanningSundry = new DataTable();
                        dtDevPlanningSundry = _PlanningReport_Dev.ResultsDataTable;

                        dtReportTables.Merge(dtDevPlanningSundry);
                        dtReportTables.AcceptChanges();
                    }
                }
                catch (Exception _exception)
                {
                    throw new ApplicationException("Report Section:PlanningReportSundry:" + _exception.Message, _exception);
                }
            }

            if (reportSettings.ActivityType == "8")
            {
                string PlanCode = "";

                if (reportSettings.PlanType == "1" || reportSettings.PlanType == "3")
                {
                    PlanCode = "MP";
                }
                else
                {
                    PlanCode = "LP";
                }

                try
                {
                    _PlanningReport_Dev_summary.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                    _PlanningReport_Dev_summary.SqlStatement = "SP_Planning_SweepVamp_Summary";
                    _PlanningReport_Dev_summary.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                    _PlanningReport_Dev_summary.ResultsTableName = "PlanningReportSweepVampSummary";

                    SqlParameter[] _paramCollection9 =
                    {
                        _PlanningReport_Dev_summary.CreateParameter("@Prodmonth", SqlDbType.VarChar, 6, theProdMonth),
                        _PlanningReport_Dev_summary.CreateParameter("@SectionID", SqlDbType.VarChar, 0,reportSettings.SectionID),
                        _PlanningReport_Dev_summary.CreateParameter("@SumLevel", SqlDbType.Int, 0,reportSettings.HierarchicalID),
                        _PlanningReport_Dev_summary.CreateParameter("@PlanCode", SqlDbType.VarChar, 0,PlanCode),
                    };

                    _PlanningReport_Dev_summary.ParamCollection = _paramCollection9;
                    _PlanningReport_Dev_summary.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _PlanningReport_Dev_summary.ExecuteInstruction();
                }
                catch (Exception _exception)
                {
                    throw new ApplicationException("Report Section:PlanningReportDev:" + _exception.Message, _exception);
                }

                GetLevel();

                MWDataManager.clsDataAccess _thelevel = new MWDataManager.clsDataAccess();
                _thelevel.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                _thelevel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                sb.Clear();
                sb.AppendLine("select distinct NAME_2 Name, b.SectionID_2 Sectionid ");
                sb.AppendLine("from Planning_Vamping a ");
                sb.AppendLine("inner join section_complete b on ");
                sb.AppendLine("a.prodmonth = b.prodmonth and ");
                sb.AppendLine("a.sectionid = b.sectionid ");
                sb.AppendLine("where a.prodmonth = " + procs.ProdMonthAsString(reportSettings.Prodmonth) + " ");
                _thelevel.SqlStatement = sb.ToString(); ;
                _thelevel.queryReturnType = MWDataManager.ReturnType.DataTable;
                _thelevel.ExecuteInstruction();
                _thelevel.ResultsTableName = "sections";
                dtSections = _thelevel.ResultsDataTable;

                try
                {
                    foreach (DataRow ds in dtSections.Rows)
                    {
                        _PlanningReport_Dev.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                        _PlanningReport_Dev.SqlStatement = "SP_Planning_SweepVamp";
                        _PlanningReport_Dev.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                        _PlanningReport_Dev.ResultsTableName = "PlanningReportSweepVamp";

                        SqlParameter[] _paramCollection9 =
                        {
                            _PlanningReport_Dev.CreateParameter("@Prodmonth", SqlDbType.VarChar, 6, theProdMonth),
                            _PlanningReport_Dev.CreateParameter("@Section", SqlDbType.VarChar, 0,ds["Name"].ToString ()),
                            _PlanningReport_Dev.CreateParameter("@PlanCode", SqlDbType.VarChar, 0,PlanCode),
                        };
                        _PlanningReport_Dev.ParamCollection = _paramCollection9;
                        _PlanningReport_Dev.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _PlanningReport_Dev.ExecuteInstruction();

                        DataTable dtDevSweepVamp = new DataTable();
                        dtDevSweepVamp = _PlanningReport_Dev.ResultsDataTable;

                        dtReportTables.Merge(dtDevSweepVamp);
                        dtReportTables.AcceptChanges();
                    }
                }
                catch (Exception _exception)
                {
                    throw new ApplicationException("Report Section:PlanningReportSweepVamp:" + _exception.Message, _exception);
                }
            }

            if (reportSettings.ActivityType == "0")
            {
                DataSet PlanningReportStoppingset = new DataSet();
                PlanningReportStoppingset.Tables.Clear();
                DataTable dtStoping = new DataTable();
                dtStoping.Columns.Add("Miner", typeof(string));
                dtStoping.Columns.Add("Workplace", typeof(string));
                dtStoping.Columns.Add("Workplaceid", typeof(string));
                dtStoping.Columns.Add("Stopped", typeof(string));
                dtStoping.Columns.Add("DayCrew", typeof(string));
                dtStoping.Columns.Add("AfternoonCrew", typeof(string));
                dtStoping.Columns.Add("NightCrew", typeof(string));
                dtStoping.Columns.Add("RovingCrew", typeof(string));
                dtStoping.Columns.Add("CrewStrength", typeof(string));
                dtStoping.Columns.Add("MiningMethod", typeof(string));
                dtStoping.Columns.Add("ProdSupervisor", typeof(string));
                dtStoping.Columns.Add("SQM", typeof(string));
                dtStoping.Columns.Add("OnReefSqm", typeof(string));
                dtStoping.Columns.Add("OffReefSqm", typeof(string));
                dtStoping.Columns.Add("Facelength", typeof(string));
                dtStoping.Columns.Add("OnReefFL", typeof(string));
                dtStoping.Columns.Add("OffReefFL", typeof(string));
                dtStoping.Columns.Add("Advance", typeof(string));
                dtStoping.Columns.Add("OnReefAdvance", typeof(string));
                dtStoping.Columns.Add("OffReefAdvance", typeof(string));
                dtStoping.Columns.Add("ChannelWidth", typeof(string));
                dtStoping.Columns.Add("ChannelWidthTons", typeof(string));
                dtStoping.Columns.Add("StopeWidth", typeof(string));
                dtStoping.Columns.Add("IdealStopeWidth", typeof(string));
                dtStoping.Columns.Add("Tons", typeof(string));
                dtStoping.Columns.Add("OnReefTons", typeof(string));
                dtStoping.Columns.Add("OffReefTons", typeof(string));
                dtStoping.Columns.Add("gt", typeof(string));
                dtStoping.Columns.Add("cmgt", typeof(string));
                dtStoping.Columns.Add("KG", typeof(string));
                dtStoping.Columns.Add("CubicMetres", typeof(string));
                dtStoping.Columns.Add("CubicTons", typeof(string));
                dtStoping.Columns.Add("CubicKg", typeof(string));
                dtStoping.Columns.Add("CubicGt", typeof(string));
                dtStoping.Columns.Add("cmkgt", typeof(string));
                dtStoping.Columns.Add("UraniumKg", typeof(string));
                dtStoping.Columns.Add("ShowAuth", typeof(string));

                dtStoping.Rows.Add(Miner, Workplace, Workplaceid, Stopped, DayCrew, AfternoonCrew, NightCrew, RovingCrew, CrewStrength, MiningMethod,
                    ProdSupervisor, SQM, OnReefSqm, OffReefSqm, Facelength, OnReefFL, OffReefFL, Advance, OnReefAdvance,
                    OffReefAdvance, ChannelWidth, ChannelWidthTons, StopeWidth, IdealStopeWidth, Tons, OnReefTons, OffReefTons,
                    gt, cmgt, KG, CubicMetres, CubicTons, CubicKg, CubicGt, cmkgt, UraniumKg, ShowAuth);

                Report theReport = new Report();

                theReport.Clear();

                PlanningReportStoppingset.Tables.Add(_PlanningReport_Stopping_summary.ResultsDataTable);
                PlanningReportStoppingset.Tables.Add(dtStoping);

                PlanningReportStoppingset.Tables.Add(dtReportTables.Copy());

                theReport.RegisterData(PlanningReportStoppingset);
                theReport.RegisterData(DsBanner);
                LoadReportType();
                LoadActivityType();


                theReport.Load(_reportFolder + "\\PlanningReport_Stoping.frx");

                theReport.SetParameterValue("logo", TGlobalItems.ClientLogo);
                theReport.SetParameterValue("logoMineware", TGlobalItems.CompanyLogo);
                theReport.SetParameterValue("Summary", reportSettings.SectionID);
                theReport.SetParameterValue("Type", reportType);
                theReport.SetParameterValue("ActivityType", activityType);
                theReport.SetParameterValue("Prodmonth", procs.ProdMonthAsString(reportSettings.Prodmonth));
                theReport.SetParameterValue("SumLevel", reportSettings.HierarchicalID);
                //theReport.Refresh();

                //theReport.Design();
                theReport.Prepare();
                theReport.Preview = pcReport2;
                theReport.ShowPrepared();

            }

            if (reportSettings.ActivityType == "1")
            {
                //dtReportTables.TableName = "PlanningReportDev";
                DataSet PlanningReportDevset = new DataSet();
                PlanningReportDevset.Tables.Clear();
                DataTable dtDev = new DataTable();
                dtDev.Columns.Add("Miner", typeof(string));
                dtDev.Columns.Add("Workplace", typeof(string));
                dtDev.Columns.Add("Workplaceid", typeof(string));
                dtDev.Columns.Add("Stopped", typeof(string));
                dtDev.Columns.Add("DayCrew", typeof(string));
                dtDev.Columns.Add("AfternoonCrew", typeof(string));
                dtDev.Columns.Add("NightCrew", typeof(string));
                dtDev.Columns.Add("RovingCrew", typeof(string));
                dtDev.Columns.Add("CrewStrength", typeof(string));
                dtDev.Columns.Add("MiningMethod", typeof(string));
                dtDev.Columns.Add("ProdSupervisor", typeof(string));
                dtDev.Columns.Add("PrimSec", typeof(string));
                dtDev.Columns.Add("TotalMetersIncl", typeof(string));
                dtDev.Columns.Add("TotalMetresExcl", typeof(string));
                dtDev.Columns.Add("OnReefMetres", typeof(string));
                dtDev.Columns.Add("OffReefMetres", typeof(string));
                dtDev.Columns.Add("MainAdvance", typeof(string));
                dtDev.Columns.Add("OffReefFL", typeof(string));
                dtDev.Columns.Add("Advance", typeof(string));
                dtDev.Columns.Add("MainOnReefMetres", typeof(string));
                dtDev.Columns.Add("MainOffReefMetres", typeof(string));
                dtDev.Columns.Add("SecMetres", typeof(string));
                dtDev.Columns.Add("SecOnReefMetres", typeof(string));
                dtDev.Columns.Add("SecOffReefMetres", typeof(string));
                dtDev.Columns.Add("CapitalMetres", typeof(string));
                dtDev.Columns.Add("CapitalOnReefMetres", typeof(string));
                dtDev.Columns.Add("CapitalOffReefMetres", typeof(string));
                dtDev.Columns.Add("EquivMetres", typeof(string));
                dtDev.Columns.Add("EquivOnReefMetres", typeof(string));
                dtDev.Columns.Add("EquivOffReefMetres", typeof(string));
                dtDev.Columns.Add("Tons", typeof(string));
                dtDev.Columns.Add("OnReefTons", typeof(string));
                dtDev.Columns.Add("OffReefTons", typeof(string));
                dtDev.Columns.Add("gt", typeof(string));
                dtDev.Columns.Add("cmgt", typeof(string));
                dtDev.Columns.Add("KG", typeof(string));
                dtDev.Columns.Add("CubicMetres", typeof(string));
                dtDev.Columns.Add("CubicTons", typeof(string));
                dtDev.Columns.Add("CubicKg", typeof(string));
                dtDev.Columns.Add("CubicGt", typeof(string));
                dtDev.Columns.Add("DrillRigg", typeof(string));
                dtDev.Columns.Add("cmkgt", typeof(string));
                dtDev.Columns.Add("UraniumKg", typeof(string));
                dtDev.Columns.Add("ShowAuth", typeof(string));

                dtDev.Rows.Add(Miner, Workplace, Workplaceid, Stopped, DayCrew, AfternoonCrew, NightCrew, RovingCrew, CrewStrength, MiningMethod,
                    ProdSupervisor, PrimSec, TotalMetersIncl, TotalMetresExcl, OnReefMetres, OffReefMetres, MainAdvance, OffReefFL, Advance,
                    MainOnReefMetres, MainOffReefMetres, SecMetres, SecOnReefMetres, SecOffReefMetres, CapitalMetres, CapitalOnReefMetres,
                    CapitalOffReefMetres, EquivMetres, EquivOnReefMetres, EquivOffReefMetres, Tons, OnReefTons, OffReefTons, gt, cmgt, KG,
                    CubicMetres, CubicTons, CubicKg, CubicGt, DrillRigg, cmkgt, UraniumKg, ShowAuth);

                Report theReport = new Report();

                theReport.Clear();

                PlanningReportDevset.Tables.Add(_PlanningReport_Dev_summary.ResultsDataTable);
                PlanningReportDevset.Tables.Add(dtDev);

                PlanningReportDevset.Tables.Add(dtReportTables);

                theReport.RegisterData(PlanningReportDevset);
                theReport.RegisterData(DsBanner);
                LoadReportType();
                LoadActivityType();

                theReport.Load(_reportFolder + "\\PlanningReport_Dev.frx");

                theReport.SetParameterValue("logo", TGlobalItems.ClientLogo);
                theReport.SetParameterValue("logoMineware", TGlobalItems.CompanyLogo);
                theReport.SetParameterValue("Summary", reportSettings.SectionID);
                theReport.SetParameterValue("Type", reportType);
                theReport.SetParameterValue("ActivityType", activityType);
                theReport.SetParameterValue("Activity", reportSettings.ActivityType);
                theReport.SetParameterValue("Prodmonth", procs.ProdMonthAsString(reportSettings.Prodmonth));
                theReport.SetParameterValue("SumLevel", reportSettings.HierarchicalID);
                theReport.Refresh();

                //theReport.Design();
                theReport.Prepare();
                theReport.Preview = pcReport2;
                theReport.ShowPrepared();

            }

            if (reportSettings.ActivityType == "2")
            {
                dtReportTables.TableName = "PlanningReportSundry";
                DataSet PlanningReportSundryset = new DataSet();
                DataTable dtSundry = new DataTable();
                dtSundry.Columns.Add("Miner", typeof(string));
                dtSundry.Columns.Add("Workplace", typeof(string));
                dtSundry.Columns.Add("Workplaceid", typeof(string));
                dtSundry.Columns.Add("Stopped", typeof(string));
                dtSundry.Columns.Add("DayCrew", typeof(string));
                dtSundry.Columns.Add("AfternoonCrew", typeof(string));
                dtSundry.Columns.Add("NightCrew", typeof(string));
                dtSundry.Columns.Add("RovingCrew", typeof(string));
                dtSundry.Columns.Add("CrewStrength", typeof(string));
                dtSundry.Columns.Add("ProdSupervisor", typeof(string));
                dtSundry.Rows.Add(Miner, Workplace, Workplaceid, Stopped, DayCrew, AfternoonCrew, NightCrew, RovingCrew, CrewStrength, ProdSupervisor);
                Report theReport = new Report();
                PlanningReportSundryset.Tables.Add(_PlanningReport_Dev_summary.ResultsDataTable);
                PlanningReportSundryset.Tables.Add(dtSundry);
                PlanningReportSundryset.Tables.Add(dtReportTables);
                theReport.RegisterData(PlanningReportSundryset);
                theReport.RegisterData(DsBanner);
                theReport.Load(_reportFolder + "\\PlanningReport_Sundry.frx");

                LoadReportType();
                LoadActivityType();
                theReport.SetParameterValue("logo", TGlobalItems.ClientLogo);
                theReport.SetParameterValue("logoMineware", TGlobalItems.CompanyLogo);
                theReport.SetParameterValue("Summary", reportSettings.SectionID);
                theReport.SetParameterValue("Type", reportType);
                theReport.SetParameterValue("ActivityType", activityType);
                theReport.SetParameterValue("Banner", SysSettings.Banner);
                theReport.SetParameterValue("Prodmonth", procs.ProdMonthAsString(reportSettings.Prodmonth));

                //theReport.Design();
                theReport.Prepare();
                theReport.Preview = pcReport2;
                theReport.ShowPrepared();

            }

            if (reportSettings.ActivityType == "8")
            {
                dtReportTables.TableName = "PlanningReportSweepVamp";
                DataSet PlanningReportSweepVampset = new DataSet();
                //DataTable dt = new DataTable();
                Report theReport = new Report();
                PlanningReportSweepVampset.Tables.Add(_PlanningReport_Dev_summary.ResultsDataTable);
                PlanningReportSweepVampset.Tables.Add(dtReportTables);
                theReport.RegisterData(PlanningReportSweepVampset);
                theReport.RegisterData(DsBanner);
                theReport.Load(_reportFolder + "\\PlanningReport_SweepVamp.frx");

                LoadReportType();
                LoadActivityType();
                theReport.SetParameterValue("logo", TGlobalItems.ClientLogo);
                theReport.SetParameterValue("logoMineware", TGlobalItems.CompanyLogo);
                theReport.SetParameterValue("Summary", reportSettings.SectionID);
                theReport.SetParameterValue("Type", reportType);
                theReport.SetParameterValue("ActivityType", activityType);
                theReport.SetParameterValue("Prodmonth", procs.ProdMonthAsString(reportSettings.Prodmonth));

                //theReport.Design();
                theReport.Prepare();
                theReport.Preview = pcReport2;
                theReport.ShowPrepared();

            }
           


        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void rpSummaryon_EditValueChanged(object sender, EventArgs e)
        {
            reportSettings.HierarchicalID = reportSettings.HierarchicalID;
        }
    }
}
