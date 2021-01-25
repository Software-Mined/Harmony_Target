using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mineware.Systems.ProductionGlobal
{
    public class ProductionGlobal
    {
        #region Fields and Properties
        public static List<ProductionGlobalTSysSettings> SystemSettingsProduction = new List<ProductionGlobalTSysSettings>();        
        private static int m_ProdMonth = 0;
        private static int m_MillMonth = 0;
        private static string m_Banner = "";
        private static Decimal m_StdAdv = 0;
        private static string m_CheckMeas = "";
        private static string M_PlanType = "";
        private static string M_CleanShift = "";
        private static string M_AdjBook = "";
        private static int M_BlastQual = 0;
        private static string M_DSOrg = "N";
        private static string M_CHkMeasLevel = "MO";
        private static string M_PlanNotes = "";
        private static string M_CurDir = "";
        private static string M_CylePlan = "";
        private static string M_RepDir = "";
        private static string M_RepDirImage = "";
        public static Decimal M_HGrade = 0;
        private static string m_TempBackDateBooking = "";
        private static string m_TempCycleChange = "";
        private static string m_PlanAuth = "";
        public static Decimal M_Vampsqm = 0;
        public static Decimal M_FatFreeShift = 0;
        //private static string m_RBPlan = "";
        //private static string m_RBBook = "";
        private static int m_Prod = 0;
        private static string m_Prod2 = "";

        public static int ProdMonth { get { return m_ProdMonth; } set { m_ProdMonth = value; } }
        public static int MillMonth { get { return m_MillMonth; } set { m_MillMonth = value; } }
        public static string Banner { get { return m_Banner; } set { m_Banner = value; } }
        public static Decimal StdAdv { get { return m_StdAdv; } set { m_StdAdv = value; } }
        public static string CheckMeas { get { return m_CheckMeas; } set { m_CheckMeas = value; } }
        public static string PlanType { get { return M_PlanType; } set { M_PlanType = value; } }
        public static string CleanShift { get { return M_CleanShift; } set { M_CleanShift = value; } }
        public static string AdjBook { get { return M_AdjBook; } set { M_AdjBook = value; } }
        public static int BlastQual { get { return M_BlastQual; } set { M_BlastQual = value; } }
        public static string DSOrg { get { return M_DSOrg; } set { M_DSOrg = value; } }
        public static string CHkMeasLevel { get { return M_CHkMeasLevel; } set { M_CHkMeasLevel = value; } }
        public static string PlanNotes { get { return M_PlanNotes; } set { M_PlanNotes = value; } }
        public static string CurDir { get { return M_CurDir; } set { M_CurDir = value; } }
        public static string CylePlan { get { return M_CylePlan; } set { M_CylePlan = value; } }
        public static string RepDir { get { return M_RepDir; } set { M_RepDir = value; } }
        public static Decimal HGrade { get { return M_HGrade; } set { M_HGrade = value; } }
        public static string TempBackDateBooking { get { return m_TempBackDateBooking; } set { m_TempBackDateBooking = value; } }
        public static string TempCycleChange { get { return m_TempCycleChange; } set { m_TempCycleChange = value; } }
        public static string PlanAuth { get { return m_PlanAuth; } set { m_PlanAuth = value; } }
        public static string ServDir { get { return M_RepDir; } set { M_RepDir = value; } }
        public static Decimal Vampsqm { get { return M_Vampsqm; } set { M_Vampsqm = value; } }
        public static Decimal FatFreeShift { get { return M_FatFreeShift; } set { M_FatFreeShift = value; } }
        public static string RepDirImage { get { return M_RepDirImage; } set { M_RepDirImage = value; } }
        public static int Prod { get { return m_Prod; } set { m_Prod = value; } }
        public static string Prod2 { get { return m_Prod2; } set { m_Prod2 = value; } }
        #endregion Fields and Properties

        #region Static Methods
        /// <summary>
        /// Gets all the fields from the SYSSET table
        /// </summary>
        /// <param name="theSystemDBTag"></param>
        /// <param name="UserCurrentInfo"></param>
        public static void GetSysSettings(string theSystemDBTag, TUserCurrentInfo UserCurrentInfo)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);// ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbMan.SqlStatement = "SELECT * FROM [dbo].[tbl_SysSet]";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            DataTable SubB = _dbMan.ResultsDataTable;           


            ProdMonth = Convert.ToInt32(SubB.Rows[0]["currentproductionmonth"].ToString());
            MillMonth = Convert.ToInt32(SubB.Rows[0]["currentmillmonth"].ToString());
            Banner = SubB.Rows[0]["Banner"].ToString();
            StdAdv = Convert.ToDecimal(SubB.Rows[0]["stpadv"].ToString());
            CheckMeas = SubB.Rows[0]["CheckMeas"].ToString();
            PlanType = SubB.Rows[0]["PlanType"].ToString();
            CleanShift = SubB.Rows[0]["CleanShift"].ToString();
            AdjBook = SubB.Rows[0]["AdjBook"].ToString();
            BlastQual = Convert.ToInt32(Math.Round(Convert.ToDecimal(SubB.Rows[0]["percblastqualification"].ToString()), 0));
            DSOrg = SubB.Rows[0]["dsorg"].ToString();
            CHkMeasLevel = SubB.Rows[0]["checkmeaslvl"].ToString();
            PlanNotes = SubB.Rows[0]["PlanNotes"].ToString();
            CylePlan = "Y";
            RepDir = SubB.Rows[0]["RepDir"].ToString();
            HGrade = Convert.ToDecimal(SubB.Rows[0]["stopingpaylimit"].ToString());
            ServDir = SubB.Rows[0]["ServerPath"].ToString();
            Vampsqm = Convert.ToInt32(Math.Round(Convert.ToDecimal(SubB.Rows[0]["vampsqm"].ToString()), 0));
            FatFreeShift = Convert.ToInt32(Math.Round(Convert.ToDecimal(SubB.Rows[0]["FatFreeShift"].ToString()), 0));
            RepDirImage = SubB.Rows[0]["RepDir"].ToString();

        }        
        public static string ProdMonthAsString(DateTime theProdMonth)
        {

            string theResult = "";
            string year = theProdMonth.Year.ToString();
            string month = theProdMonth.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;
            theResult = year + month;
            return theResult;

        }
        public static int ProdMonthAsInt(DateTime theProdMonth)
        {

            string theResult = "";
            string year = theProdMonth.Year.ToString();
            string month = theProdMonth.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;
            theResult = year + month;
            return Convert.ToInt32(theResult);

        }
        public static string ExtractAfterColon(string TheString)
        {
            string AfterColon = "";

            int index = TheString.IndexOf(":"); // Kry die postion van die :

            AfterColon = TheString.Substring(index + 1); // kry alles na :

            return AfterColon;
        }
        public static string ExtractBeforeColon(string TheString)
        {
            if (TheString != "")
            {
                string BeforeColon;

                int index = TheString.IndexOf(":");

                BeforeColon = TheString.Substring(0, index);

                return BeforeColon;
            }
            else
            {
                return "";
            }
        }
        public static DateTime ProdMonthAsDate(string theProdMonth)
        {
            DateTime theResult = DateTime.Now;
            int theYear = Convert.ToInt32(theProdMonth.Substring(0, 4));
            int theMonth = Convert.ToInt32(theProdMonth.Substring(4, 2));
            theResult = new DateTime(theYear, theMonth, 1);
            return theResult;
        }
        public static ProductionGlobalTSysSettings getSystemSettingsProductionInfo(string SiteTag)
        {
            ProductionGlobalTSysSettings theResult = new ProductionGlobalTSysSettings();
            foreach (ProductionGlobalTSysSettings ui in SystemSettingsProduction)
            {
                if (ui.SiteTag == SiteTag)
                {
                    theResult = ui;
                    break;
                }
            }

            return theResult;
        }
        public static void SetProductionGlobalInfo(string sysDBTag)
        {
            DataTable siteList = TConnections.GetSiteList();
            foreach (DataRow dr in siteList.Rows)
            {
                try
                {
                    ProductionGlobalTSysSettings theProductionInfo = new ProductionGlobalTSysSettings();
                    theProductionInfo.SiteTag = dr["Name"].ToString();
                    theProductionInfo.GetSysSettings(sysDBTag, dr["Name"].ToString());
                    SystemSettingsProduction.Add(theProductionInfo);
                }
                catch (Exception)
                {


                }
            }
        }
        /// <summary>
        /// Production month to be used for system calculations
        /// </summary>
        /// <param name="ProdMonth1"></param>
        public static void ProdMonthCalc(int ProdMonth1)
        {
            //int Prod;
            Decimal month = Convert.ToDecimal(ProdMonth1);
            String PMonth = month.ToString();
            PMonth.Substring(4, 2);

            if (Convert.ToInt32(PMonth.Substring(4, 2)) > 12)
            {
                int M = Convert.ToInt32(PMonth.Substring(0, 4));
                M++;
                PMonth = M.ToString();
                PMonth = PMonth + "01";
                ProdMonth1 = Convert.ToInt32(PMonth);
            }
            else
            {
                if (Convert.ToInt32(PMonth.Substring(4, 2)) < 1)
                {
                    int M = Convert.ToInt32(PMonth.Substring(0, 4));
                    M--;
                    PMonth = M.ToString();
                    PMonth = PMonth + "12";
                    ProdMonth1 = Convert.ToInt32(PMonth);
                }
            }
            Prod = ProdMonth1;
        }
        /// <summary>
        /// Production month that that will be displayed on the front end
        /// </summary>
        /// <param name="ProdMonth1"></param>
        public static void ProdMonthVis(int ProdMonth1)
        {
            Prod2 = ProdMonth1.ToString().Substring(0, 4);

            if (ProdMonth1.ToString().Substring(4, 2) == "01")
            {
                Prod2 = "Jan-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "02")
            {
                Prod2 = "Feb-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "03")
            {
                Prod2 = "Mar-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "04")
            {
                Prod2 = "Apr-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "05")
            {
                Prod2 = "May-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "06")
            {
                Prod2 = "Jun-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "07")
            {
                Prod2 = "Jul-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "08")
            {
                Prod2 = "Aug-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "09")
            {
                Prod2 = "Sep-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "10")
            {
                Prod2 = "Oct-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "11")
            {
                Prod2 = "Nov-" + Prod2;
            }

            if (ProdMonth1.ToString().Substring(4, 2) == "12")
            {
                Prod2 = "Dec-" + Prod2;
            }

        }
        
        #endregion Static Methods
    }
}
