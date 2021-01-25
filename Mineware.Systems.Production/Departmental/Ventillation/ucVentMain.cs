using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Ventillation
{

    public partial class ucVentMain : BaseUserControl
    {

        public ucVentMain()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpVentMain);
            FormActiveRibbonPage = rpVentMain;
            FormMainRibbonPage = rpVentMain;
            RibbonControl = rcVentMain;
        }

        #region Private variables
        clsVentScheduling _clsVentScheduling = new clsVentScheduling();
        public string DayNum = string.Empty;
        public string slctdName;
        string SelectedCrewID;
        string SelectedCrewName;
        string CurrMonth;
        string SelectedCalendardate = string.Empty;
        string SelectedWorkplaceID = string.Empty;
        string SelectedSectionID = string.Empty;
        string SelectedUniqueID = string.Empty;
        string SelectedFieldNameDate = string.Empty;
        string barcode1 = string.Empty;
        string SelectedWorkplace = string.Empty;
        string InvalidDate = "N";
        #endregion


        #region Methods
        void LoadGrid()
        {
            MWDataManager.clsDataAccess _dbMan3Mnth = new MWDataManager.clsDataAccess();
            _dbMan3Mnth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);

            _dbMan3Mnth.SqlStatement = " exec sp_Dept_Inspections_MainGrid_New ";

            _dbMan3Mnth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3Mnth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3Mnth.ExecuteInstruction();

            DataTable dt = _dbMan3Mnth.ResultsDataTable;

            DataSet ds = new DataSet();

            if (ds.Tables.Count > 0)
                ds.Tables.Clear();

            ds.Tables.Add(dt);
            Grd3Mnth.Visible = false;

            Grd3Mnth.DataSource = ds.Tables[0];

            MnthMOID.FieldName = "MOSection";

            MnthSBID.FieldName = "SBSection";
            MnthMinerSecID.FieldName = "MinerSection";
            MnthMiner.FieldName = "MinerName";
            MnthCrew.FieldName = "CrewName";
            MnthWp.FieldName = "WPDesc";
            MnthWPID.FieldName = "WPID";
            MnthDueDate.FieldName = "DueDate";
            colCrewID.FieldName = "CrewID";
            MnthDaysRem.FieldName = "DayslastInspec";
            StpInt.FieldName = "gap";

            Mnth1d1.FieldName = "M1D1";
            Mnth1d2.FieldName = "M1D2";
            Mnth1d3.FieldName = "M1D3";
            Mnth1d4.FieldName = "M1D4";
            Mnth1d5.FieldName = "M1D5";
            Mnth1d6.FieldName = "M1D6";
            Mnth1d7.FieldName = "M1D7";
            Mnth1d8.FieldName = "M1D8";
            Mnth1d9.FieldName = "M1D9";
            Mnth1d10.FieldName = "M1D10";
            Mnth1d11.FieldName = "M1D11";
            Mnth1d12.FieldName = "M1D12";
            Mnth1d13.FieldName = "M1D13";
            Mnth1d14.FieldName = "M1D14";
            Mnth1d15.FieldName = "M1D15";
            Mnth1d16.FieldName = "M1D16";
            Mnth1d17.FieldName = "M1D17";
            Mnth1d18.FieldName = "M1D18";
            Mnth1d19.FieldName = "M1D19";
            Mnth1d20.FieldName = "M1D20";
            Mnth1d21.FieldName = "M1D21";
            Mnth1d22.FieldName = "M1D22";
            Mnth1d23.FieldName = "M1D23";
            Mnth1d24.FieldName = "M1D24";
            Mnth1d25.FieldName = "M1D25";
            Mnth1d26.FieldName = "M1D26";
            Mnth1d27.FieldName = "M1D27";
            Mnth1d28.FieldName = "M1D28";
            Mnth1d29.FieldName = "M1D29";
            Mnth1d30.FieldName = "M1D30";
            Mnth1d31.FieldName = "M1D31";

            Mnth2d1.FieldName = "M2D1";
            Mnth2d2.FieldName = "M2D2";
            Mnth2d3.FieldName = "M2D3";
            Mnth2d4.FieldName = "M2D4";
            Mnth2d5.FieldName = "M2D5";
            Mnth2d6.FieldName = "M2D6";
            Mnth2d7.FieldName = "M2D7";
            Mnth2d8.FieldName = "M2D8";
            Mnth2d9.FieldName = "M2D9";
            Mnth2d10.FieldName = "M2D10";
            Mnth2d11.FieldName = "M2D11";
            Mnth2d12.FieldName = "M2D12";
            Mnth2d13.FieldName = "M2D13";
            Mnth2d14.FieldName = "M2D14";
            Mnth2d15.FieldName = "M2D15";
            Mnth2d16.FieldName = "M2D16";
            Mnth2d17.FieldName = "M2D17";
            Mnth2d18.FieldName = "M2D18";
            Mnth2d19.FieldName = "M2D19";
            Mnth2d20.FieldName = "M2D20";
            Mnth2d21.FieldName = "M2D21";
            Mnth2d22.FieldName = "M2D22";
            Mnth2d23.FieldName = "M2D23";
            Mnth2d24.FieldName = "M2D24";
            Mnth2d25.FieldName = "M2D25";
            Mnth2d26.FieldName = "M2D26";
            Mnth2d27.FieldName = "M2D27";
            Mnth2d28.FieldName = "M2D28";
            Mnth2d29.FieldName = "M2D29";
            Mnth2d30.FieldName = "M2D30";
            Mnth2d31.FieldName = "M2D31";

            Mnth3d1.FieldName = "M3D1";
            Mnth3d2.FieldName = "M3D2";
            Mnth3d3.FieldName = "M3D3";
            Mnth3d4.FieldName = "M3D4";
            Mnth3d5.FieldName = "M3D5";
            Mnth3d6.FieldName = "M3D6";
            Mnth3d7.FieldName = "M3D7";
            Mnth3d8.FieldName = "M3D8";
            Mnth3d9.FieldName = "M3D9";
            Mnth3d10.FieldName = "M3D10";
            Mnth3d11.FieldName = "M3D11";
            Mnth3d12.FieldName = "M3D12";
            Mnth3d13.FieldName = "M3D13";
            Mnth3d14.FieldName = "M3D14";
            Mnth3d15.FieldName = "M3D15";
            Mnth3d16.FieldName = "M3D16";
            Mnth3d17.FieldName = "M3D17";
            Mnth3d18.FieldName = "M3D18";
            Mnth3d19.FieldName = "M3D19";
            Mnth3d20.FieldName = "M3D20";
            Mnth3d21.FieldName = "M3D21";
            Mnth3d22.FieldName = "M3D22";
            Mnth3d23.FieldName = "M3D23";
            Mnth3d24.FieldName = "M3D24";
            Mnth3d25.FieldName = "M3D25";
            Mnth3d26.FieldName = "M3D26";
            Mnth3d27.FieldName = "M3D27";
            Mnth3d28.FieldName = "M3D28";
            Mnth3d29.FieldName = "M3D29";
            Mnth3d30.FieldName = "M3D30";
            Mnth3d31.FieldName = "M3D31";

            Grd3Mnth.Visible = true;

            string pm2 = string.Empty;
            string pm3 = string.Empty;

            string pm1a = string.Empty;

            string pm2a = string.Empty;
            string pm3a = string.Empty;

            // get next mnth            
            String PMonth = CurrMonth;
            String PMonthOrig = CurrMonth;
            Decimal year = Convert.ToDecimal(PMonth.Substring(0, 4));
            Decimal month = Convert.ToDecimal(PMonth.Substring(4, 2));
            String NewMonth = string.Empty;
            Decimal NewProdMonth = 0;

            PMonth.Substring(4, 2);

            if (month == 12)
            {
                year = year + 1;
                month = 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm2 = year.ToString() + NewMonth;

            }
            else
            {
                year = year;
                month = month + 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm2 = year.ToString() + NewMonth;

            }

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(pm2) - 1);
            pm1a = ProductionGlobal.ProductionGlobal.Prod2;

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(pm2));
            pm2a = ProductionGlobal.ProductionGlobal.Prod2;



            PMonth = pm2;
            year = Convert.ToDecimal(PMonth.Substring(0, 4));
            month = Convert.ToDecimal(PMonth.Substring(4, 2));
            NewMonth = string.Empty;
            NewProdMonth = 0;

            PMonth.Substring(4, 2);

            if (month == 12)
            {
                year = year + 1;
                month = 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm3 = year.ToString() + NewMonth;

            }
            else
            {
                year = year;
                month = month + 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm3 = year.ToString() + NewMonth;

            }

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(pm3));
            pm3a = ProductionGlobal.ProductionGlobal.Prod2;

            mnth1.Caption = pm1a;
            mnth2.Caption = pm2a;
            mnth3.Caption = pm3a;

            int daymnth1 = 0;
            int daymnth2 = 0;
            int daymnth3 = 0;

            DateTime Date1 = DateTime.Today;

            Date1 = new DateTime(Convert.ToInt32(PMonthOrig.Substring(0, 4)), Convert.ToInt32(PMonthOrig.Substring(4, 2)), 01);
            int day1 = 35;

            int day2 = 0;
            Date1 = Date1.AddDays(day1);

            day2 = Date1.Day;
            Date1 = Date1.AddDays(day2 * -1);

            daymnth1 = Date1.Day;


            Date1 = Date1.AddDays(day1);

            day2 = Date1.Day;
            Date1 = Date1.AddDays(day2 * -1);

            daymnth2 = Date1.Day;

            Date1 = Date1.AddDays(day1);

            day2 = Date1.Day;
            Date1 = Date1.AddDays(day2 * -1);

            daymnth3 = Date1.Day;

            if (daymnth1 == 28)
            {
                Mnth1d29.Visible = false;
                Mnth1d30.Visible = false;
                Mnth1d31.Visible = false;
                gridBand102.Visible = false;
                gridBand103.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth1 == 29)
            {
                Mnth1d30.Visible = false;
                Mnth1d31.Visible = false;
                gridBand103.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth1 == 30)
            {
                Mnth1d31.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth2 == 28)
            {
                Mnth2d29.Visible = false;
                Mnth2d30.Visible = false;
                Mnth2d31.Visible = false;
                gridBand133.Visible = false;
                gridBand124.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth2 == 29)
            {
                Mnth2d30.Visible = false;
                Mnth2d31.Visible = false;
                gridBand124.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth2 == 30)
            {
                Mnth2d31.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth3 == 28)
            {
                Mnth3d29.Visible = false;
                Mnth3d30.Visible = false;
                Mnth3d31.Visible = false;
                gridBand165.Visible = false;
                gridBand166.Visible = false;
                gridBand167.Visible = false;
            }

            if (daymnth3 == 29)
            {
                Mnth3d30.Visible = false;
                Mnth3d31.Visible = false;
                gridBand166.Visible = false;
                gridBand167.Visible = false;
            }

            if (daymnth3 == 30)
            {
                Mnth3d31.Visible = false;
                gridBand167.Visible = false;
            }


        }

        void LoadDevelopment()
        {
            MWDataManager.clsDataAccess _dbMan3Mnth = new MWDataManager.clsDataAccess();
            _dbMan3Mnth.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection); ;

            _dbMan3Mnth.SqlStatement = " exec sp_Dept_Inspections_MainGrid_New_Dev ";

            _dbMan3Mnth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3Mnth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3Mnth.ExecuteInstruction();

            DataTable dt = _dbMan3Mnth.ResultsDataTable;

            DataSet ds = new DataSet();

            if (ds.Tables.Count > 0)
                ds.Tables.Clear();

            ds.Tables.Add(dt);
            Grd3MnthDev.Visible = false;


            Grd3MnthDev.DataSource = ds.Tables[0];

            MnthMOIDdev.FieldName = "MOSection";

            MnthSBIDdev.FieldName = "SBSection";
            MnthMinerSecDevID.FieldName = "MinerSection";
            MnthMinerDev.FieldName = "MinerName";
            MnthCrewDev.FieldName = "CrewName";
            MnthWpDev.FieldName = "WPDesc";
            MnthWPIDdev.FieldName = "WPID";
            MnthDueDateDev.FieldName = "DueDate";
            colCrewDevID.FieldName = "CrewID";
            MnthDaysRemDev.FieldName = "DayslastInspec";
            DevInt.FieldName = "gap";
            //MnthPerc.FieldName = "sdate";

            MnthDev1d1.FieldName = "M1D1";
            MnthDev1d2.FieldName = "M1D2";
            MnthDev1d3.FieldName = "M1D3";
            MnthDev1d4.FieldName = "M1D4";
            MnthDev1d5.FieldName = "M1D5";
            MnthDev1d6.FieldName = "M1D6";
            MnthDev1d7.FieldName = "M1D7";
            MnthDev1d8.FieldName = "M1D8";
            MnthDev1d9.FieldName = "M1D9";
            MnthDev1d10.FieldName = "M1D10";
            MnthDev1d11.FieldName = "M1D11";
            MnthDev1d12.FieldName = "M1D12";
            MnthDev1d13.FieldName = "M1D13";
            MnthDev1d14.FieldName = "M1D14";
            MnthDev1d15.FieldName = "M1D15";
            MnthDev1d16.FieldName = "M1D16";
            MnthDev1d17.FieldName = "M1D17";
            MnthDev1d18.FieldName = "M1D18";
            MnthDev1d19.FieldName = "M1D19";
            MnthDev1d20.FieldName = "M1D20";
            MnthDev1d21.FieldName = "M1D21";
            MnthDev1d22.FieldName = "M1D22";
            MnthDev1d23.FieldName = "M1D23";
            MnthDev1d24.FieldName = "M1D24";
            MnthDev1d25.FieldName = "M1D25";
            MnthDev1d26.FieldName = "M1D26";
            MnthDev1d27.FieldName = "M1D27";
            MnthDev1d28.FieldName = "M1D28";
            MnthDev1d29.FieldName = "M1D29";
            MnthDev1d30.FieldName = "M1D30";
            MnthDev1d31.FieldName = "M1D31";

            MnthDev2d1.FieldName = "M2D1";
            MnthDev2d2.FieldName = "M2D2";
            MnthDev2d3.FieldName = "M2D3";
            MnthDev2d4.FieldName = "M2D4";
            MnthDev2d5.FieldName = "M2D5";
            MnthDev2d6.FieldName = "M2D6";
            MnthDev2d7.FieldName = "M2D7";
            MnthDev2d8.FieldName = "M2D8";
            MnthDev2d9.FieldName = "M2D9";
            MnthDev2d10.FieldName = "M2D10";
            MnthDev2d11.FieldName = "M2D11";
            MnthDev2d12.FieldName = "M2D12";
            MnthDev2d13.FieldName = "M2D13";
            MnthDev2d14.FieldName = "M2D14";
            MnthDev2d15.FieldName = "M2D15";
            MnthDev2d16.FieldName = "M2D16";
            MnthDev2d17.FieldName = "M2D17";
            MnthDev2d18.FieldName = "M2D18";
            MnthDev2d19.FieldName = "M2D19";
            MnthDev2d20.FieldName = "M2D20";
            MnthDev2d21.FieldName = "M2D21";
            MnthDev2d22.FieldName = "M2D22";
            MnthDev2d23.FieldName = "M2D23";
            MnthDev2d24.FieldName = "M2D24";
            MnthDev2d25.FieldName = "M2D25";
            MnthDev2d26.FieldName = "M2D26";
            MnthDev2d27.FieldName = "M2D27";
            MnthDev2d28.FieldName = "M2D28";
            MnthDev2d29.FieldName = "M2D29";
            MnthDev2d30.FieldName = "M2D30";
            MnthDev2d31.FieldName = "M2D31";

            MnthDev3d1.FieldName = "M3D1";
            MnthDev3d2.FieldName = "M3D2";
            MnthDev3d3.FieldName = "M3D3";
            MnthDev3d4.FieldName = "M3D4";
            MnthDev3d5.FieldName = "M3D5";
            MnthDev3d6.FieldName = "M3D6";
            MnthDev3d7.FieldName = "M3D7";
            MnthDev3d8.FieldName = "M3D8";
            MnthDev3d9.FieldName = "M3D9";
            MnthDev3d10.FieldName = "M3D10";
            MnthDev3d11.FieldName = "M3D11";
            MnthDev3d12.FieldName = "M3D12";
            MnthDev3d13.FieldName = "M3D13";
            MnthDev3d14.FieldName = "M3D14";
            MnthDev3d15.FieldName = "M3D15";
            MnthDev3d16.FieldName = "M3D16";
            MnthDev3d17.FieldName = "M3D17";
            MnthDev3d18.FieldName = "M3D18";
            MnthDev3d19.FieldName = "M3D19";
            MnthDev3d20.FieldName = "M3D20";
            MnthDev3d21.FieldName = "M3D21";
            MnthDev3d22.FieldName = "M3D22";
            MnthDev3d23.FieldName = "M3D23";
            MnthDev3d24.FieldName = "M3D24";
            MnthDev3d25.FieldName = "M3D25";
            MnthDev3d26.FieldName = "M3D26";
            MnthDev3d27.FieldName = "M3D27";
            MnthDev3d28.FieldName = "M3D28";
            MnthDev3d29.FieldName = "M3D29";
            MnthDev3d30.FieldName = "M3D30";
            MnthDev3d31.FieldName = "M3D31";

            Grd3MnthDev.Visible = true;

            string pm2 = string.Empty;
            string pm3 = string.Empty;

            string pm1a = string.Empty;

            string pm2a = string.Empty;
            string pm3a = string.Empty;

            // get next mnth            
            String PMonth = CurrMonth;
            String PMonthOrig = CurrMonth;
            Decimal year = Convert.ToDecimal(PMonth.Substring(0, 4));
            Decimal month = Convert.ToDecimal(PMonth.Substring(4, 2));
            String NewMonth = string.Empty;
            Decimal NewProdMonth = 0;

            PMonth.Substring(4, 2);



            if (month == 12)
            {
                year = year + 1;
                month = 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm2 = year.ToString() + NewMonth;

            }
            else
            {
                year = year;
                month = month + 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm2 = year.ToString() + NewMonth;

            }

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(pm2) - 1);
            pm1a = ProductionGlobal.ProductionGlobal.Prod2;

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(pm2));
            pm2a = ProductionGlobal.ProductionGlobal.Prod2;



            PMonth = pm2;
            year = Convert.ToDecimal(PMonth.Substring(0, 4));
            month = Convert.ToDecimal(PMonth.Substring(4, 2));
            NewMonth = string.Empty;
            NewProdMonth = 0;

            PMonth.Substring(4, 2);

            if (month == 12)
            {
                year = year + 1;
                month = 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm3 = year.ToString() + NewMonth;

            }
            else
            {
                year = year;
                month = month + 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm3 = year.ToString() + NewMonth;

            }

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(pm3));
            pm3a = ProductionGlobal.ProductionGlobal.Prod2;

            gridBand32.Caption = pm1a;
            gridBand185.Caption = pm2a;
            gridBand217.Caption = pm3a;

            int daymnth1 = 0;
            int daymnth2 = 0;
            int daymnth3 = 0;

            DateTime Date1 = DateTime.Today;

            Date1 = new DateTime(Convert.ToInt32(PMonthOrig.Substring(0, 4)), Convert.ToInt32(PMonthOrig.Substring(4, 2)), 01);
            int day1 = 35;

            int day2 = 0;
            Date1 = Date1.AddDays(day1);

            day2 = Date1.Day;
            Date1 = Date1.AddDays(day2 * -1);

            daymnth1 = Date1.Day;


            Date1 = Date1.AddDays(day1);

            day2 = Date1.Day;
            Date1 = Date1.AddDays(day2 * -1);

            daymnth2 = Date1.Day;

            Date1 = Date1.AddDays(day1);

            day2 = Date1.Day;
            Date1 = Date1.AddDays(day2 * -1);

            daymnth3 = Date1.Day;

            if (daymnth1 == 28)
            {
                MnthDev1d29.Visible = false;
                MnthDev1d30.Visible = false;
                MnthDev1d31.Visible = false;
                gridBand102.Visible = false;
                gridBand103.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth1 == 29)
            {
                MnthDev1d30.Visible = false;
                MnthDev1d31.Visible = false;
                gridBand103.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth1 == 30)
            {
                MnthDev1d31.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth2 == 28)
            {
                MnthDev2d29.Visible = false;
                MnthDev2d30.Visible = false;
                MnthDev2d31.Visible = false;
                gridBand133.Visible = false;
                gridBand124.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth2 == 29)
            {
                MnthDev2d30.Visible = false;
                MnthDev2d31.Visible = false;
                gridBand124.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth2 == 30)
            {
                MnthDev2d31.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth3 == 28)
            {
                MnthDev3d29.Visible = false;
                MnthDev3d30.Visible = false;
                MnthDev3d31.Visible = false;
                gridBand165.Visible = false;
                gridBand166.Visible = false;
                gridBand167.Visible = false;
            }

            if (daymnth3 == 29)
            {
                MnthDev3d30.Visible = false;
                MnthDev3d31.Visible = false;
                gridBand166.Visible = false;
                gridBand167.Visible = false;
            }

            if (daymnth3 == 30)
            {
                MnthDev3d31.Visible = false;
                gridBand167.Visible = false;
            }


        }

        void LoadOtherChecklists()
        {
            MWDataManager.clsDataAccess _dbMan3MnthOther = new MWDataManager.clsDataAccess();
            _dbMan3MnthOther.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection); ;

            _dbMan3MnthOther.SqlStatement = " exec sp_Dept_Inspections_MainGrid_New_Other '" + txtChecklistName1.EditValue.ToString() + "' ";

            _dbMan3MnthOther.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3MnthOther.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3MnthOther.ExecuteInstruction();

            DataTable dt1 = _dbMan3MnthOther.ResultsDataTable;

            DataSet ds1 = new DataSet();

            if (ds1.Tables.Count > 0)
                ds1.Tables.Clear();

            ds1.Tables.Add(dt1);
            Grd3MnthOther.Visible = false;
            Grd3MnthOther.DataSource = null;
            Grd3MnthOther.DataSource = ds1.Tables[0];

            MnthMOIDother.FieldName = "MOSection";

            MnthSBIDother.FieldName = "SBSection";
            MnthMinerSecIDother.FieldName = "MinerSection";
            MnthMinerOther.FieldName = "MinerName";
            MnthCrewOther.FieldName = "CrewName";
            MnthWPother.FieldName = "WPDesc";
            MnthWPIDother.FieldName = "WPID";
            MnthDueDateOther.FieldName = "DueDate";
            colCrewOtherID.FieldName = "CrewID";
            MnthDaysRemOther.FieldName = "DayslastInspec";
            OtherInt.FieldName = "gap";
            //MnthPerc.FieldName = "sdate";

            MnthOther1d1.FieldName = "M1D1";
            MnthOther1d2.FieldName = "M1D2";
            MnthOther1d3.FieldName = "M1D3";
            MnthOther1d4.FieldName = "M1D4";
            MnthOther1d5.FieldName = "M1D5";
            MnthOther1d6.FieldName = "M1D6";
            MnthOther1d7.FieldName = "M1D7";
            MnthOther1d8.FieldName = "M1D8";
            MnthOther1d9.FieldName = "M1D9";
            MnthOther1d10.FieldName = "M1D10";
            MnthOther1d11.FieldName = "M1D11";
            MnthOther1d12.FieldName = "M1D12";
            MnthOther1d13.FieldName = "M1D13";
            MnthOther1d14.FieldName = "M1D14";
            MnthOther1d15.FieldName = "M1D15";
            MnthOther1d16.FieldName = "M1D16";
            MnthOther1d17.FieldName = "M1D17";
            MnthOther1d18.FieldName = "M1D18";
            MnthOther1d19.FieldName = "M1D19";
            MnthOther1d20.FieldName = "M1D20";
            MnthOther1d21.FieldName = "M1D21";
            MnthOther1d22.FieldName = "M1D22";
            MnthOther1d23.FieldName = "M1D23";
            MnthOther1d24.FieldName = "M1D24";
            MnthOther1d25.FieldName = "M1D25";
            MnthOther1d26.FieldName = "M1D26";
            MnthOther1d27.FieldName = "M1D27";
            MnthOther1d28.FieldName = "M1D28";
            MnthOther1d29.FieldName = "M1D29";
            MnthOther1d30.FieldName = "M1D30";
            MnthOther1d31.FieldName = "M1D31";

            MnthOther2d1.FieldName = "M2D1";
            MnthOther2d2.FieldName = "M2D2";
            MnthOther2d3.FieldName = "M2D3";
            MnthOther2d4.FieldName = "M2D4";
            MnthOther2d5.FieldName = "M2D5";
            MnthOther2d6.FieldName = "M2D6";
            MnthOther2d7.FieldName = "M2D7";
            MnthOther2d8.FieldName = "M2D8";
            MnthOther2d9.FieldName = "M2D9";
            MnthOther2d10.FieldName = "M2D10";
            MnthOther2d11.FieldName = "M2D11";
            MnthOther2d12.FieldName = "M2D12";
            MnthOther2d13.FieldName = "M2D13";
            MnthOther2d14.FieldName = "M2D14";
            MnthOther2d15.FieldName = "M2D15";
            MnthOther2d16.FieldName = "M2D16";
            MnthOther2d17.FieldName = "M2D17";
            MnthOther2d18.FieldName = "M2D18";
            MnthOther2d19.FieldName = "M2D19";
            MnthOther2d20.FieldName = "M2D20";
            MnthOther2d21.FieldName = "M2D21";
            MnthOther2d22.FieldName = "M2D22";
            MnthOther2d23.FieldName = "M2D23";
            MnthOther2d24.FieldName = "M2D24";
            MnthOther2d25.FieldName = "M2D25";
            MnthOther2d26.FieldName = "M2D26";
            MnthOther2d27.FieldName = "M2D27";
            MnthOther2d28.FieldName = "M2D28";
            MnthOther2d29.FieldName = "M2D29";
            MnthOther2d30.FieldName = "M2D30";
            MnthOther2d31.FieldName = "M2D31";

            MnthOther3d1.FieldName = "M3D1";
            MnthOther3d2.FieldName = "M3D2";
            MnthOther3d3.FieldName = "M3D3";
            MnthOther3d4.FieldName = "M3D4";
            MnthOther3d5.FieldName = "M3D5";
            MnthOther3d6.FieldName = "M3D6";
            MnthOther3d7.FieldName = "M3D7";
            MnthOther3d8.FieldName = "M3D8";
            MnthOther3d9.FieldName = "M3D9";
            MnthOther3d10.FieldName = "M3D10";
            MnthOther3d11.FieldName = "M3D11";
            MnthOther3d12.FieldName = "M3D12";
            MnthOther3d13.FieldName = "M3D13";
            MnthOther3d14.FieldName = "M3D14";
            MnthOther3d15.FieldName = "M3D15";
            MnthOther3d16.FieldName = "M3D16";
            MnthOther3d17.FieldName = "M3D17";
            MnthOther3d18.FieldName = "M3D18";
            MnthOther3d19.FieldName = "M3D19";
            MnthOther3d20.FieldName = "M3D20";
            MnthOther3d21.FieldName = "M3D21";
            MnthOther3d22.FieldName = "M3D22";
            MnthOther3d23.FieldName = "M3D23";
            MnthOther3d24.FieldName = "M3D24";
            MnthOther3d25.FieldName = "M3D25";
            MnthOther3d26.FieldName = "M3D26";
            MnthOther3d27.FieldName = "M3D27";
            MnthOther3d28.FieldName = "M3D28";
            MnthOther3d29.FieldName = "M3D29";
            MnthOther3d30.FieldName = "M3D30";
            MnthOther3d31.FieldName = "M3D31";

            Grd3MnthOther.Visible = true;

            string pm2 = string.Empty;
            string pm3 = string.Empty;

            string pm1a = string.Empty;

            string pm2a = string.Empty;
            string pm3a = string.Empty;

            // get next mnth            
            String PMonth = CurrMonth;
            String PMonthOrig = CurrMonth;
            Decimal year = Convert.ToDecimal(PMonth.Substring(0, 4));
            Decimal month = Convert.ToDecimal(PMonth.Substring(4, 2));
            String NewMonth = string.Empty;
            Decimal NewProdMonth = 0;

            PMonth.Substring(4, 2);



            if (month == 12)
            {
                year = year + 1;
                month = 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm2 = year.ToString() + NewMonth;

            }
            else
            {
                year = year;
                month = month + 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm2 = year.ToString() + NewMonth;

            }

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(pm2) - 1);
            pm1a = ProductionGlobal.ProductionGlobal.Prod2;

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(pm2));
            pm2a = ProductionGlobal.ProductionGlobal.Prod2;



            PMonth = pm2;
            year = Convert.ToDecimal(PMonth.Substring(0, 4));
            month = Convert.ToDecimal(PMonth.Substring(4, 2));
            NewMonth = string.Empty;
            NewProdMonth = 0;

            PMonth.Substring(4, 2);

            if (month == 12)
            {
                year = year + 1;
                month = 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm3 = year.ToString() + NewMonth;

            }
            else
            {
                year = year;
                month = month + 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm3 = year.ToString() + NewMonth;

            }

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(pm3));
            pm3a = ProductionGlobal.ProductionGlobal.Prod2;

            gridBand279.Caption = pm1a;
            gridBand311.Caption = pm2a;
            gridBand343.Caption = pm3a;

            int daymnth1 = 0;
            int daymnth2 = 0;
            int daymnth3 = 0;

            DateTime Date1 = DateTime.Today;

            Date1 = new DateTime(Convert.ToInt32(PMonthOrig.Substring(0, 4)), Convert.ToInt32(PMonthOrig.Substring(4, 2)), 01);
            int day1 = 35;

            int day2 = 0;
            Date1 = Date1.AddDays(day1);

            day2 = Date1.Day;
            Date1 = Date1.AddDays(day2 * -1);

            daymnth1 = Date1.Day;


            Date1 = Date1.AddDays(day1);

            day2 = Date1.Day;
            Date1 = Date1.AddDays(day2 * -1);

            daymnth2 = Date1.Day;

            Date1 = Date1.AddDays(day1);

            day2 = Date1.Day;
            Date1 = Date1.AddDays(day2 * -1);

            daymnth3 = Date1.Day;

            if (daymnth1 == 28)
            {
                MnthOther1d29.Visible = false;
                MnthOther1d30.Visible = false;
                MnthOther1d31.Visible = false;
                gridBand102.Visible = false;
                gridBand103.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth1 == 29)
            {
                MnthOther1d30.Visible = false;
                MnthOther1d31.Visible = false;
                gridBand103.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth1 == 30)
            {
                MnthOther1d31.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth2 == 28)
            {
                MnthOther2d29.Visible = false;
                MnthOther2d30.Visible = false;
                MnthOther2d31.Visible = false;
                gridBand133.Visible = false;
                gridBand124.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth2 == 29)
            {
                MnthOther2d30.Visible = false;
                MnthOther2d31.Visible = false;
                gridBand124.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth2 == 30)
            {
                MnthOther2d31.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth3 == 28)
            {
                MnthOther3d29.Visible = false;
                MnthOther3d30.Visible = false;
                MnthOther3d31.Visible = false;
                gridBand165.Visible = false;
                gridBand166.Visible = false;
                gridBand167.Visible = false;
            }

            if (daymnth3 == 29)
            {
                MnthOther3d30.Visible = false;
                MnthOther3d31.Visible = false;
                gridBand166.Visible = false;
                gridBand167.Visible = false;
            }

            if (daymnth3 == 30)
            {
                MnthOther3d31.Visible = false;
                gridBand167.Visible = false;
            }


        }

        void LoadChecklists()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select ChecklistName Name from tbl_Dept_Vent_OtherChecklist";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable Data = _dbMan.ResultsDataTable;

            ////Load data into the checklist dropdown
            foreach (DataRow dr in Data.Rows)
            {
                repTxtChecklistName1.Items.Add(dr["Name"].ToString());
            }

            txtChecklistName1.EditValue = _dbMan.ResultsDataTable.Rows[0][0].ToString();
        }
        #endregion

        private void ucVentMain_Load(object sender, EventArgs e)
        {
            CurrMonth = Convert.ToString((DateTime.Now.Year * 100) + DateTime.Now.Month);
            tabOtherChecklist.PageVisible = false;
            LoadGrid();
            LoadChecklists();
            LoadDevelopment();
            LoadOtherChecklists();
        }

        private void btnAddInspection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (tabOtherChecklist.Focus())
            {
                if (SelectedWorkplaceID != string.Empty)
                {
                    frmVentOtherChecklist frmChkLst = new frmVentOtherChecklist();
                    frmChkLst._theSystemDBTag = theSystemDBTag;
                    frmChkLst._UserCurrentInfo = UserCurrentInfo.Connection;
                    frmChkLst.cbxWorkplace.EditValue = SelectedWorkplaceID;
                    frmChkLst.cbxSection.EditValue = SelectedSectionID;
                    slctdName = txtChecklistName1.EditValue.ToString();
                    frmChkLst.chkListName = txtChecklistName1.EditValue.ToString();
                    frmChkLst.ricbxCheckList.Items.Add(txtChecklistName1.EditValue.ToString());

                    frmChkLst.ShowDialog();

                    LoadOtherChecklists();
                }
                else
                {
                    MessageBox.Show("Please Select a Workplace");
                    return;
                }
            }

            if (tabDevelopment.Focus())
            {
                if (SelectedWorkplaceID != string.Empty)
                {
                    frmDevelopmentInspection frmDev = new frmDevelopmentInspection();
                    frmDev.dbl_rec_MinerSection.Text = SelectedSectionID;
                    frmDev.dbl_rec_Date.Text = SelectedCalendardate;
                    frmDev.dbl_rec_WPID.Text = SelectedWorkplaceID;
                    frmDev.WorkplaceMain = SelectedWorkplace;
                    frmDev.dbl_rec_Crew = SelectedCrewID + ":" + SelectedCrewName;
                    frmDev.tbCrew.EditValue = SelectedCrewID + ":" + SelectedCrewName;
                    frmDev._theSystemDBTag = theSystemDBTag;
                    frmDev._UserCurrentInfo = UserCurrentInfo.Connection;
                    frmDev.ShowDialog();

                    LoadDevelopment();
                }
                else
                {
                    MessageBox.Show("Please Select a Workplace");
                    return;
                }
            }

            if (tabStoping.Focus())
            {
                if (SelectedWorkplaceID != string.Empty)
                {
                    frmInspection frmEdit = new frmInspection();
                    frmEdit.dbl_rec_MinerSection.Text = SelectedSectionID;
                    frmEdit.dbl_rec_Date.Text = SelectedCalendardate;
                    frmEdit.dbl_rec_WPID.Text = SelectedWorkplaceID;
                    frmEdit.getWorkplace = SelectedWorkplace;
                    frmEdit.dbl_rec_Crew = SelectedCrewID + ":" + SelectedCrewName;
                    frmEdit.tbCrew.EditValue = SelectedCrewID + ":" + SelectedCrewName;
                    frmEdit._theSystemDBTag = theSystemDBTag;
                    frmEdit._UserCurrentInfo = UserCurrentInfo.Connection;
                    frmEdit.ShowDialog();

                    LoadGrid();
                }
                else
                {
                    MessageBox.Show("Please Select a Workplace");
                    return;
                }
            }
        }

        private void gvInsp_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "gap")
            {
                string wpid = gvInsp.GetRowCellValue(e.RowHandle, gvInsp.Columns["WPID"]).ToString();
                string GapValue = gvInsp.GetRowCellValue(e.RowHandle, gvInsp.Columns["gap"]).ToString();

                MWDataManager.clsDataAccess _dbMan3MnthOther = new MWDataManager.clsDataAccess();
                _dbMan3MnthOther.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection); ;

                _dbMan3MnthOther.SqlStatement = " delete from tbl_Dept_Inspection_VentInterval_Stoping where workplace = '" + wpid + "'  insert into tbl_Dept_Inspection_VentInterval_Stoping values ( \r\n" +
                                                " '" + wpid + "','" + GapValue + "' )";

                _dbMan3MnthOther.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan3MnthOther.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan3MnthOther.ExecuteInstruction();
            }
        }

        private void gvInsp_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            Graphics g = e.Graphics;

            Brush hb = Brushes.LightGray;

            GridView View = sender as GridView;

            string ss = string.Empty;

            int interval = 45;

            if (View.GetRowCellValue(e.RowHandle, "gap").ToString() != string.Empty)
                interval = Convert.ToInt32(View.GetRowCellValue(e.RowHandle, "gap").ToString());

            try
            {
                for (int i = 4; i < gvInsp.Columns.Count - 1; i++)
                {
                    if (e.Column == View.Columns[i] && e.RowHandle < gvInsp.RowCount)
                    {
                        if (!View.GetRowCellValue(e.RowHandle, e.Column).Equals(DBNull.Value))
                        {
                            e.Appearance.ForeColor = Color.Black;

                            ss = View.GetRowCellValue(e.RowHandle, e.Column).ToString();
                            if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() == "OFF")
                            {
                                e.Appearance.BackColor = Color.Gainsboro;
                                e.Appearance.ForeColor = Color.Gainsboro;
                            }

                            if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() == "NP")
                            {
                                e.Appearance.ForeColor = Color.White;
                            }
                        }
                    }
                }
            }
            catch { }

            try
            {
                if (e.CellValue.ToString() != string.Empty && e.CellValue.ToString() != "OFF")
                {
                    if (e.Column.FieldName == "DayslastInspec")
                    {
                        if (Convert.ToInt32(e.CellValue) < 0)
                        {
                            e.Appearance.ForeColor = Color.Red;
                        }
                    }

                    if (e.Column.FieldName != "MOSection" && e.Column.FieldName != "SBSection" && e.Column.FieldName != "MinerSection" && e.Column.FieldName != "MinerName" &&
                        e.Column.FieldName != "CrewName" && e.Column.FieldName != "WPDesc" && e.Column.FieldName != "WPID" && e.Column.FieldName != "DueDate" &&
                        e.Column.FieldName != "CrewID" && e.Column.FieldName != "DayslastInspec")
                    {
                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 > interval)
                        {
                            Color redColorLight = Color.FromArgb(255, 255, 14, 14);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = redColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= interval - 2)
                        {
                            Color redColorLight = Color.FromArgb(200, 255, 165, 0);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = redColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval - 3 && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= interval - 5)
                        {
                            Color redColorLight = Color.FromArgb(150, 255, 215, 0);
                            Color TextColorLight = Color.FromArgb(100, 255, 215, 0);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = TextColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval - 6 && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= interval - 15)
                        {
                            Color redColorLight = Color.FromArgb(100, 255, 215, 0);
                            Color TextColorLight = Color.FromArgb(50, 255, 215, 0);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = redColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval - 16 && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= 0)
                        {
                            e.Appearance.ForeColor = Color.Transparent;
                        }
                    }
                }
            }
            catch { }
        }

        private void gvInsp_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.VisibleIndex >= 5)
            {
                SelectedFieldNameDate = e.Column.FieldName;
                SelectedCalendardate = e.Column.Caption;

                DateTime Calendardate = DateTime.Today;
                try
                {
                    Calendardate = Convert.ToDateTime(SelectedCalendardate);
                }
                catch
                {
                    return;
                }
            }

            SelectedSectionID = gvInsp.GetRowCellValue(e.RowHandle, gvInsp.Columns["MinerSection"]).ToString();
            SelectedWorkplaceID = gvInsp.GetRowCellValue(e.RowHandle, gvInsp.Columns["WPID"]).ToString();
            SelectedWorkplace = gvInsp.GetRowCellValue(e.RowHandle, gvInsp.Columns["WPDesc"]).ToString();
            SelectedCrewID = gvInsp.GetRowCellValue(e.RowHandle, gvInsp.Columns["CrewID"]).ToString();
            SelectedCrewName = gvInsp.GetRowCellValue(e.RowHandle, gvInsp.Columns["CrewName"]).ToString();

            barcode1 = String.Format("{0:ddMMyyyy}", DateTime.Now);

            Cursor = Cursors.Default;
        }

        private void gvInspDev_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "gap")
            {
                string wpid = gvInspDev.GetRowCellValue(e.RowHandle, gvInsp.Columns["WPID"]).ToString();
                string GapValue = gvInspDev.GetRowCellValue(e.RowHandle, gvInsp.Columns["gap"]).ToString();

                MWDataManager.clsDataAccess _dbMan3MnthOther = new MWDataManager.clsDataAccess();
                _dbMan3MnthOther.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection); ;

                _dbMan3MnthOther.SqlStatement = " delete from tbl_Dept_Inspection_VentInterval_Development where workplace = '" + wpid + "'  insert into tbl_Dept_Inspection_VentInterval_Development values ( \r\n" +
                                                " '" + wpid + "','" + GapValue + "' )";

                _dbMan3MnthOther.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan3MnthOther.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan3MnthOther.ExecuteInstruction();
            }
        }

        private void gvInspDev_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            Graphics g = e.Graphics;

            Brush hb = Brushes.LightGray;

            GridView View = sender as GridView;

            string ss = string.Empty;

            int interval = 90;

            if (View.GetRowCellValue(e.RowHandle, "gap").ToString() != string.Empty)
                interval = Convert.ToInt32(View.GetRowCellValue(e.RowHandle, "gap").ToString());

            try
            {
                for (int i = 4; i < gvInspDev.Columns.Count - 1; i++)
                {
                    if (e.Column == View.Columns[i] && e.RowHandle < gvInspDev.RowCount)
                    {
                        if (!View.GetRowCellValue(e.RowHandle, e.Column).Equals(DBNull.Value))
                        {
                            e.Appearance.ForeColor = Color.Black;

                            ss = View.GetRowCellValue(e.RowHandle, e.Column).ToString();
                            if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() == "OFF")
                            {
                                e.Appearance.BackColor = Color.Gainsboro;
                                e.Appearance.ForeColor = Color.Gainsboro;
                            }


                            if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() == "NP")
                            {
                                e.Appearance.ForeColor = Color.White;
                            }
                        }

                    }
                }
            }
            catch { }

            try
            {
                if (e.CellValue.ToString() != string.Empty && e.CellValue.ToString() != "OFF")
                {
                    if (e.Column.FieldName == "DayslastInspec")
                    {
                        if (Convert.ToInt32(e.CellValue) < 0)
                        {
                            e.Appearance.ForeColor = Color.Red;
                        }
                    }

                    if (e.Column.FieldName != "MOSection" && e.Column.FieldName != "SBSection" && e.Column.FieldName != "MinerSection" && e.Column.FieldName != "MinerName" &&
                        e.Column.FieldName != "CrewName" && e.Column.FieldName != "WPDesc" && e.Column.FieldName != "WPID" && e.Column.FieldName != "DueDate" &&
                        e.Column.FieldName != "CrewID" && e.Column.FieldName != "DayslastInspec")
                    {
                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 > interval)
                        {
                            Color redColorLight = Color.FromArgb(255, 255, 14, 14);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = redColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= interval - 2)
                        {
                            Color redColorLight = Color.FromArgb(200, 255, 165, 0);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = redColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval - 3 && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= interval - 5)
                        {
                            Color redColorLight = Color.FromArgb(150, 255, 215, 0);
                            Color TextColorLight = Color.FromArgb(100, 255, 215, 0);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = TextColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval - 6 && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= interval - 15)
                        {
                            Color redColorLight = Color.FromArgb(100, 255, 215, 0);
                            Color TextColorLight = Color.FromArgb(50, 255, 215, 0);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = redColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval - 16 && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= 0)
                        {
                            e.Appearance.ForeColor = Color.Transparent;
                        }
                    }
                }
            }
            catch { }
        }

        private void gvInspDev_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.CellValue.ToString() != string.Empty && e.CellValue.ToString() != "OFF")
                {
                    if (e.Column.FieldName != "MOSection" && e.Column.FieldName != "SBSection" && e.Column.FieldName != "MinerSection" && e.Column.FieldName != "MinerName" &&
                        e.Column.FieldName != "CrewName" && e.Column.FieldName != "WPDesc" && e.Column.FieldName != "WPID" && e.Column.FieldName != "DueDate" &&
                        e.Column.FieldName != "CrewID" && e.Column.FieldName != "DayslastInspec")
                    {
                        if (Convert.ToInt32(e.CellValue) == 0)
                        {
                            e.RepositoryItem = repImageComboInspec;
                        }
                    }
                }
            }
            catch { }
        }

        private void gvInspDev_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.VisibleIndex >= 5)
            {
                SelectedFieldNameDate = e.Column.FieldName;
                SelectedCalendardate = e.Column.Caption;

                DateTime Calendardate = DateTime.Today;
                try
                {
                    Calendardate = Convert.ToDateTime(SelectedCalendardate);
                }
                catch
                {
                    return;
                }
            }

            SelectedSectionID = gvInspDev.GetRowCellValue(e.RowHandle, gvInspDev.Columns["MinerSection"]).ToString();
            SelectedWorkplaceID = gvInspDev.GetRowCellValue(e.RowHandle, gvInspDev.Columns["WPID"]).ToString();
            SelectedWorkplace = gvInspDev.GetRowCellValue(e.RowHandle, gvInspDev.Columns["WPDesc"]).ToString();
            SelectedCrewID = gvInspDev.GetRowCellValue(e.RowHandle, gvInspDev.Columns["CrewID"]).ToString();
            SelectedCrewName = gvInspDev.GetRowCellValue(e.RowHandle, gvInspDev.Columns["CrewName"]).ToString();


            barcode1 = String.Format("{0:ddMMyyyy}", DateTime.Now);

            Cursor = Cursors.Default;
        }

        private void gcInspOther_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            Graphics g = e.Graphics;

            Brush hb = Brushes.LightGray;

            GridView View = sender as GridView;

            string ss = string.Empty;

            int interval = 90;

            if (View.GetRowCellValue(e.RowHandle, "gap").ToString() != string.Empty)
                interval = Convert.ToInt32(View.GetRowCellValue(e.RowHandle, "gap").ToString());

            try
            {
                for (int i = 4; i < gcInspOther.Columns.Count - 1; i++)
                {
                    if (e.Column == View.Columns[i] && e.RowHandle < gcInspOther.RowCount)
                    {
                        if (!View.GetRowCellValue(e.RowHandle, e.Column).Equals(DBNull.Value))
                        {
                            e.Appearance.ForeColor = Color.Black;

                            ss = View.GetRowCellValue(e.RowHandle, e.Column).ToString();
                            if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() == "OFF")
                            {
                                e.Appearance.BackColor = Color.Gainsboro;
                                e.Appearance.ForeColor = Color.Gainsboro;
                            }


                            if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() == "NP")
                            {
                                e.Appearance.ForeColor = Color.White;
                            }
                        }
                    }
                }
            }
            catch { }

            try
            {
                if (e.CellValue.ToString() != string.Empty && e.CellValue.ToString() != "OFF")
                {
                    if (e.Column.FieldName == "DayslastInspec")
                    {
                        if (Convert.ToInt32(e.CellValue) < 0)
                        {
                            e.Appearance.ForeColor = Color.Red;
                        }
                    }

                    if (e.Column.FieldName != "MOSection" && e.Column.FieldName != "SBSection" && e.Column.FieldName != "MinerSection" && e.Column.FieldName != "MinerName" &&
                        e.Column.FieldName != "CrewName" && e.Column.FieldName != "WPDesc" && e.Column.FieldName != "WPID" && e.Column.FieldName != "DueDate" &&
                        e.Column.FieldName != "CrewID" && e.Column.FieldName != "DayslastInspec")
                    {
                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 > interval)
                        {
                            Color redColorLight = Color.FromArgb(255, 255, 14, 14);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = redColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= interval - 2)
                        {
                            Color redColorLight = Color.FromArgb(200, 255, 165, 0);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = redColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval - 3 && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= interval - 5)
                        {
                            Color redColorLight = Color.FromArgb(150, 255, 215, 0);
                            Color TextColorLight = Color.FromArgb(100, 255, 215, 0);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = TextColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval - 6 && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= interval - 10)
                        {
                            Color redColorLight = Color.FromArgb(100, 255, 215, 0);
                            Color TextColorLight = Color.FromArgb(50, 255, 215, 0);

                            e.Appearance.BackColor = redColorLight;
                            e.Appearance.ForeColor = redColorLight;
                        }

                        if (Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 <= interval - 11 && Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column).ToString()) * -1 >= 0)
                        {
                            e.Appearance.ForeColor = Color.Transparent;
                        }
                    }
                }
            }
            catch { }
        }

        private void gcInspOther_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "gap")
            {
                string wpid = gcInspOther.GetRowCellValue(e.RowHandle, gvInsp.Columns["WPID"]).ToString();
                string GapValue = gcInspOther.GetRowCellValue(e.RowHandle, gvInsp.Columns["gap"]).ToString();

                MWDataManager.clsDataAccess _dbMan3MnthOther = new MWDataManager.clsDataAccess();
                _dbMan3MnthOther.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection); ;

                _dbMan3MnthOther.SqlStatement = " delete from tbl_Dept_Inspection_VentInterval_Other where workplace = '" + wpid + "' insert into tbl_Dept_Inspection_VentInterval_Other values ( \r\n" +
                                                "  (Select CheckListID from tbl_Dept_Vent_OtherChecklist where CheckListName = '" + txtChecklistName1.EditValue.ToString() + "' ) ,'" + wpid + "','" + GapValue + "' )";

                _dbMan3MnthOther.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan3MnthOther.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan3MnthOther.ExecuteInstruction();
                //LoadOtherChecklists();
            }
        }

        private void gcInspOther_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            ///Add Images in a cell
			try
            {
                if (e.CellValue.ToString() != string.Empty && e.CellValue.ToString() != "OFF")
                {
                    if (e.Column.FieldName != "MOSection" && e.Column.FieldName != "SBSection" && e.Column.FieldName != "MinerSection" && e.Column.FieldName != "MinerName" &&
                        e.Column.FieldName != "CrewName" && e.Column.FieldName != "WPDesc" && e.Column.FieldName != "WPID" && e.Column.FieldName != "DueDate" &&
                        e.Column.FieldName != "CrewID" && e.Column.FieldName != "DayslastInspec")
                    {
                        if (Convert.ToInt32(e.CellValue) == 0)
                        {
                            e.RepositoryItem = repImageComboInspec;
                        }
                    }
                }
            }
            catch { }
        }

        private void gcInspOther_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.VisibleIndex >= 5)
            {
                SelectedFieldNameDate = e.Column.FieldName;
                SelectedCalendardate = e.Column.Caption;

                DateTime Calendardate = DateTime.Today;
                try
                {
                    Calendardate = Convert.ToDateTime(SelectedCalendardate);
                }
                catch
                {
                    return;
                }
            }

            SelectedSectionID = gcInspOther.GetRowCellValue(e.RowHandle, gcInspOther.Columns["MinerSection"]).ToString();
            SelectedWorkplaceID = gcInspOther.GetRowCellValue(e.RowHandle, gcInspOther.Columns["WPID"]).ToString();
            SelectedWorkplace = gcInspOther.GetRowCellValue(e.RowHandle, gcInspOther.Columns["WPDesc"]).ToString();
            SelectedCrewID = gcInspOther.GetRowCellValue(e.RowHandle, gcInspOther.Columns["CrewID"]).ToString();
            SelectedCrewName = gcInspOther.GetRowCellValue(e.RowHandle, gcInspOther.Columns["CrewName"]).ToString();

            barcode1 = String.Format("{0:ddMMyyyy}", DateTime.Now);

            Cursor = Cursors.Default;
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmChecklistWorkplaces frm = new frmChecklistWorkplaces();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.checklistName = txtChecklistName1.EditValue.ToString();
            frm._theSystemDBTag = theSystemDBTag;
            frm._UserCurrentInfo = UserCurrentInfo.Connection;
            frm.ShowDialog();

            LoadOtherChecklists();
        }

        private void gvInsp_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.CellValue.ToString() != string.Empty && e.CellValue.ToString() != "OFF")
                {
                    if (e.Column.FieldName != "MOSection" && e.Column.FieldName != "SBSection" && e.Column.FieldName != "MinerSection" && e.Column.FieldName != "MinerName" &&
                        e.Column.FieldName != "CrewName" && e.Column.FieldName != "WPDesc" && e.Column.FieldName != "WPID" && e.Column.FieldName != "DueDate" &&
                        e.Column.FieldName != "CrewID" && e.Column.FieldName != "DayslastInspec")
                    {
                        if (Convert.ToInt32(e.CellValue) == 0)
                        {
                            e.RepositoryItem = repImageComboInspec;
                        }
                    }
                }
            }
            catch { }
        }

        private void tabInspControl_Click(object sender, EventArgs e)
        {
            SelectedCalendardate = string.Empty;
            SelectedWorkplaceID = string.Empty;
            SelectedSectionID = string.Empty;
            SelectedUniqueID = string.Empty;
            SelectedFieldNameDate = string.Empty;
            barcode1 = string.Empty;
            SelectedWorkplace = string.Empty;

            if (tabOtherChecklist.Focus())
            {
                txtChecklistName.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnAddOtherWorkplaces.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                txtChecklistName.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnAddOtherWorkplaces.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        private void txtChecklistName1_EditValueChanged(object sender, EventArgs e)
        {
            LoadOtherChecklists();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }


}
