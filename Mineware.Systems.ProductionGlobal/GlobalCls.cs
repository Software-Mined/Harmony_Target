using System;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;

public enum ReportTypes 
{
    repDaily = 1,
    repLosses = 2,
    repMonthEndRecon = 3,
    repMonthEndReconDev = 4,
    repCrewPerformance = 5,
    repPlanChangeAudit = 6,
    repLostBlastAnal = 7,
    repCrewRanking = 8,
    repTopFiveLosses = 9,
    repProductionAnalysis = 10,
    repSBRanking = 11,
    repSBPerformance = 12,
    rep6Shift = 13,
    repPlanning = 14
}

public class Procedures
{
    private static string m_BookFrm = "";
    public static string BookFrm { get { return m_BookFrm; } set { m_BookFrm = value; } }

    private static string m_PropFrm1 = "";
    public static string PropFrm1 { get { return m_PropFrm1; } set { m_PropFrm1 = value; } }

    private static string m_ServicesFrm = "";
    public static string ServicesFrm { get { return m_ServicesFrm; } set { m_ServicesFrm = value; } }
    
    private static int m_Prod = 0;
    private static string m_Prod2 = "";

    public static int Prod { get { return m_Prod; } set { m_Prod = value; } }
    public static string Prod2 { get { return m_Prod2; } set { m_Prod2 = value; } }

    private static string m_MsgText = "";
    public static string MsgText { get { return m_MsgText; } set { m_MsgText = value; } }

    private static string m_MsgInfo = "";
    public static string MsgInfo { get { return m_MsgInfo; } set { m_MsgInfo = value; } }

    public string ProdMonthAsString(DateTime theProdMonth)
    {

        string theResult = "";
        string year = theProdMonth.Year.ToString();
        string month = theProdMonth.Month.ToString();
        if (month.Length == 1)
            month = "0" + month;
        theResult = year + month;
        return theResult;

    }

    public DateTime ProdMonthAsDate(string theProdMonth)
    {
        DateTime theResult = DateTime.Now;
        int theYear = Convert.ToInt32(theProdMonth.Substring(0, 4));
        int theMonth = Convert.ToInt32(theProdMonth.Substring(4, 2));
        theResult = new DateTime(theYear, theMonth, 1);
        return theResult;

    }

    public string ProdMonthAsDate2(DateTime theProdMonth)
    {
        string theResult = "";
        string year = theProdMonth.Year.ToString();
        string month = theProdMonth.Month.ToString();
        if (month.Length == 1)
            month = "0" + month;
        theResult = year + month;
        return theResult;

    }

    public int ProdMonthAsInt(DateTime theProdMonth)
    {

        string theResult = "";
        string year = theProdMonth.Year.ToString();
        string month = theProdMonth.Month.ToString();
        if (month.Length == 1)
            month = "0" + month;
        theResult = year + month;
        return Convert.ToInt32(theResult);

    }

    public decimal spinMonth(decimal TheMonth)
    {
        if (TheMonth != 0)
        {
            decimal _themonth;
            Decimal month = TheMonth;
            String PMonth = month.ToString();
            PMonth.Substring(4, 2);
            if (Convert.ToInt32(PMonth.Substring(4, 2)) > 12)
            {
                // MessageBox.Show(PMonth);
                int M = Convert.ToInt32(PMonth.Substring(0, 4));
                M++;
                PMonth = M.ToString();
                PMonth = PMonth + "01";
                _themonth = Convert.ToInt32(PMonth);
                return _themonth;
            }
            else
            {
                if (Convert.ToInt32(PMonth.Substring(4, 2)) < 1)
                {
                    int M = Convert.ToInt32(PMonth.Substring(0, 4));
                    M--;
                    PMonth = M.ToString();
                    PMonth = PMonth + "12";
                    _themonth = Convert.ToDecimal(PMonth);
                    return _themonth;
                }
                else
                {
                    _themonth = TheMonth;
                    return _themonth;
                }
            }
        }
        else
            return 0;

    }

    //Production month to be used for system calculations
    public void ProdMonthCalc(int ProdMonth1)
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
        Procedures.Prod = ProdMonth1;
    }

    //Production month that that will be displayed on the front end
    public void ProdMonthVis(int ProdMonth1)
    {


        Procedures.Prod2 = ProdMonth1.ToString().Substring(0, 4);

        if (ProdMonth1.ToString().Substring(4, 2) == "01")
        {
            Procedures.Prod2 = "Jan-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "02")
        {
            Procedures.Prod2 = "Feb-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "03")
        {
            Procedures.Prod2 = "Mar-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "04")
        {
            Procedures.Prod2 = "Apr-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "05")
        {
            Procedures.Prod2 = "May-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "06")
        {
            Procedures.Prod2 = "Jun-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "07")
        {
            Procedures.Prod2 = "Jul-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "08")
        {
            Procedures.Prod2 = "Aug-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "09")
        {
            Procedures.Prod2 = "Sep-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "10")
        {
            Procedures.Prod2 = "Oct-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "11")
        {
            Procedures.Prod2 = "Nov-" + Procedures.Prod2;
        }

        if (ProdMonth1.ToString().Substring(4, 2) == "12")
        {
            Procedures.Prod2 = "Dec-" + Procedures.Prod2;
        }
    }

    //extracts the string value before the colon
    public string ExtractBeforeColon(string TheString)
    {
        if (TheString != "")
        {
        string BeforeColon;

        int index = TheString.IndexOf(":");

        BeforeColon = TheString.Substring(0, index); 

        return BeforeColon;
        }
        else { return "";
        }
    }

    //extracts the string value after the colon
    public string ExtractAfterColon(string TheString)
    {
        string AfterColon;

        int index = TheString.IndexOf(":"); // Kry die postion van die :

        AfterColon = TheString.Substring(index + 1); // kry alles na :

        return AfterColon;
    }

    

    public DataTable GetSections(string ProdMonth, string HierId, string SectionId)
    {
        MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
        _dbMan.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];

        _dbMan.SqlStatement = " Select SECTIONid, Name,  Hierarchicalid Hier " +
                              "from Section s where s.Prodmonth = '" + ProdMonth.ToString() + "' and HierarchicalType = 'Pro' ";
                             if (HierId.ToString() != "NO")
                              {
                              _dbMan.SqlStatement = _dbMan.SqlStatement +" and Hierarchicalid = '"+ HierId.ToString() +"' " ;
                              }
                             
                              _dbMan.SqlStatement = _dbMan.SqlStatement + "and Sectionid like '" + SectionId.ToString() + "%' ";
                              
                              _dbMan.SqlStatement = _dbMan.SqlStatement +" order by SECTIONid ";
        _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
        _dbMan.ResultsTableName = "GetSections"; 
        _dbMan.ExecuteInstruction();

        DataTable dt1 = _dbMan.ResultsDataTable;
        return dt1;
    }

    public DataView Search(DataTable SearchTable, string SearchString)
    {
        DataView dv = new DataView(SearchTable);
        string SearchExpression = null;

        if (!String.IsNullOrEmpty(SearchString))//(Filtertxt.Text))
        {

            SearchExpression = string.Format("'{0}%'", SearchString);//Filtertxt.Text);
            dv.RowFilter = "Description like " + SearchExpression;
        }

        //DataTable dtResult = 
        //MessageBox.Show(SearchTable.Rows.Count.ToString());
        return dv;
    }


//    public Image Base64ToImage(string base64String, string wwww1)
//    {
//        // Convert Base64 String to byte[]
//        string s;

//        s = base64String.Trim().Replace(" ", "+");
//        s = base64String.Trim().Replace("_", "/");

//        if (s.Length % 4 > 0)
//            s = s.PadRight(s.Length + 4 - s.Length % 4, '=');

//        decimal aa = s.Length;



//        byte[] imageBytes = Convert.FromBase64String(s);
//        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);


//        ms.Write(imageBytes, 0, imageBytes.Length);
//        Image image = Image.FromStream(ms, true);


//        MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
//        _dbManImage.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
//        _dbManImage.SqlStatement = "update [dbo].[tbl_RockMechInspection] set picture = '" + s + "' where workplace = '" + wwww1 + "' and (captyear*100)+captweek = (select max(captyear*100+captweek) from  [dbo].[tbl_RockMechInspection] where workplace = '" + wwww1 + "') ";
//        _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
//        _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
//        _dbManImage.ResultsTableName = "Image";
//        _dbManImage.ExecuteInstruction();


//        return image;

//    }
}

public class SysSettings
{
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

    

    //gets all the fields from the SYSSET table
    public void GetSysSettings()
    {
        MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
        _dbMan.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];

        _dbMan.SqlStatement = "select * from tbl_sysset ";
        _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
        _dbMan.ExecuteInstruction();
        DataTable SubB = _dbMan.ResultsDataTable;
       

        SysSettings.ProdMonth = Convert.ToInt32(SubB.Rows[0]["currentproductionmonth"].ToString());
        SysSettings.MillMonth = Convert.ToInt32(SubB.Rows[0]["currentmillmonth"].ToString());
        SysSettings.Banner = SubB.Rows[0]["Banner"].ToString();
        SysSettings.StdAdv = Convert.ToDecimal(SubB.Rows[0]["stpadv"].ToString());
        SysSettings.CheckMeas = SubB.Rows[0]["CheckMeas"].ToString();
        SysSettings.PlanType = SubB.Rows[0]["PlanType"].ToString();
        SysSettings.CleanShift = SubB.Rows[0]["CleanShift"].ToString();
        SysSettings.AdjBook = SubB.Rows[0]["AdjBook"].ToString();
        SysSettings.BlastQual = Convert.ToInt32(Math.Round(Convert.ToDecimal(SubB.Rows[0]["percblastqualification"].ToString()),0));
        SysSettings.DSOrg = SubB.Rows[0]["dsorg"].ToString();
        SysSettings.CHkMeasLevel = SubB.Rows[0]["checkmeaslvl"].ToString();
        SysSettings.PlanNotes = SubB.Rows[0]["PlanNotes"].ToString();
        SysSettings.CylePlan = "Y";
        SysSettings.RepDir = SubB.Rows[0]["RepDir"].ToString();
        SysSettings.HGrade = Convert.ToDecimal(SubB.Rows[0]["stopingpaylimit"].ToString());
        SysSettings.ServDir = SubB.Rows[0]["ServerPath"].ToString();
        SysSettings.Vampsqm = Convert.ToInt32(Math.Round(Convert.ToDecimal(SubB.Rows[0]["vampsqm"].ToString()), 0));
        SysSettings.FatFreeShift = Convert.ToInt32(Math.Round(Convert.ToDecimal(SubB.Rows[0]["FatFreeShift"].ToString()), 0));
        SysSettings.RepDirImage = SubB.Rows[0]["RepDir"].ToString();
        
    }

    //sets the logged on user information
    public void SetUserInfo()
    {
        MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
        _dbMan.ConnectionString = ConfigurationManager.AppSettings["SQLConnectionStr"];

        _dbMan.SqlStatement = "select c.*, b.hierarchicalid, isnull(GeolSampWorksOrder,'N') SampWO, "+
                            "CompLogin, Lvl1, Lvl2, Lvl3, Lvl4, Lvl5, Lvl6, Lvl7, Lvl8, CreateNotes " +

                             "from CpmUsers c " +
                              "left outer join (select * from Section where prodmonth = (select currentproductionmonth from sysset)) b " +
                              "on c.passectionid = b.sectionid " +
                              "left outer join cpmusers_Department_Geoscience d " +
                              "on c.userid = d.userid " +
                              "left outer join CPMUsers_Department_Survey e " +
                              "on c.userid = e.userid " +
                              "where c.UserID = '" + TUserInfo.UserID + "'";
        _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
        _dbMan.ExecuteInstruction();
        DataTable SubA = _dbMan.ResultsDataTable;
               
        clsUserInfo.UserName = SubA.Rows[0]["username"].ToString();
        clsUserInfo.UserBookSection = SubA.Rows[0]["Passectionid"].ToString();
        clsUserInfo.Hier = Convert.ToInt32(SubA.Rows[0]["hierarchicalid"]);

        clsUserInfo.Tram = SubA.Rows[0]["Tram"].ToString();
        clsUserInfo.Hoist = SubA.Rows[0]["Hoist"].ToString();
        clsUserInfo.mill = SubA.Rows[0]["mill"].ToString();
        clsUserInfo.book = SubA.Rows[0]["pasbook"].ToString();
        clsUserInfo.dropraise = SubA.Rows[0]["dropraise"].ToString();
        clsUserInfo.sys = SubA.Rows[0]["systemadmin"].ToString();
        clsUserInfo.plan = SubA.Rows[0]["pasplan"].ToString();
        clsUserInfo.samp = SubA.Rows[0]["sampling"].ToString();
        clsUserInfo.Surv = SubA.Rows[0]["survey"].ToString();
        clsUserInfo.Expl = SubA.Rows[0]["Explosive"].ToString();
        clsUserInfo.DiamondDril = SubA.Rows[0]["DiamondDrilling"].ToString();
        clsUserInfo.TempBackDateBooking = SubA.Rows[0]["ChiefAuth"].ToString();
        clsUserInfo.TempCycleChange = SubA.Rows[0]["ProdmanAuth"].ToString();
        clsUserInfo.PlanAuth = SubA.Rows[0]["ChiefPlanAuth"].ToString();
        clsUserInfo.CalChange = SubA.Rows[0]["CalChange"].ToString();
        clsUserInfo.MOMeas = SubA.Rows[0]["FinManAuth"].ToString();
        clsUserInfo.GradeAuth = SubA.Rows[0]["HrManAuth"].ToString();
        clsUserInfo.RBPlan = SubA.Rows[0]["ReefBoringPlanning"].ToString();
        clsUserInfo.RBBook = SubA.Rows[0]["ReefBoringBooking"].ToString();
        clsUserInfo.NSBook = SubA.Rows[0]["NSBooking"].ToString();
        clsUserInfo.SampWO = SubA.Rows[0]["SampWO"].ToString();
        clsUserInfo.SNCreate = SubA.Rows[0]["CreateNotes"].ToString();
        


      

    }

 
    

}

public class clsValidations
{
    public enum ValidationType
    {
        MWInteger5,
        MWCleanText,
        MWDouble5D1,
        MWDouble5D2,
        MWDouble5D3,
        MWDouble5D4,
        MWDate
    }
    public ValidationType MWValidationType;

    public clsValidations()
    {
    }

    public string _MWInput;
    public string MWInput { set { _MWInput = value; } }

    public bool Validate()
    {
        try
        {
            switch (MWValidationType)
            {
                case ValidationType.MWCleanText:

                    // Clean Text without ' and / \ only a-z A-Z and -

                    Regex ValFactor1 = new Regex(@"^\s*[a-zA-Z0-9,\s\-]+\s*$");
                    if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }

                case ValidationType.MWDate:
                    break;
                case ValidationType.MWInteger5:

                    // Limits to 5 left

                    ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}$");
                    if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }


                case ValidationType.MWDouble5D1:

                    // Limits to 5 left and only 1 decimal

                    ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}(?:\.\d{0,1})?$");
                    if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }

                case ValidationType.MWDouble5D2:

                    // Limits to 5 left and only 2 decimals

                    ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}(?:\.\d{0,2})?$");
                    if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }


                case ValidationType.MWDouble5D3:

                    // Limits to 5 left and only 1 decimals

                    ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}(?:\.\d{0,3})?$");
                    if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }

                case ValidationType.MWDouble5D4:

                    // Limits to 5 left and only 1 decimals

                    ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}(?:\.\d{0,4})?$");
                    if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }


            }
            return true;
        }
        catch
        {
            return false;
        }
    }

   


    

    public class clsValidation
    {
        public enum ValidationType
        {
            MWInteger5,
            MWInteger12,
            MWCleanText,
            MWDouble5D1,
            MWDouble5D2,
            MWDouble12D2,
            MWDouble5D3,
            MWDouble5D4,
            MWBinary,
            MWDate
        }
        public ValidationType MWValidationType;

        public string _MWInput;
        public string MWInput { set { _MWInput = value; } }

        public bool Validate()
        {
            try
            {
                switch (MWValidationType)
                {
                    case ValidationType.MWCleanText:

                        // Clean Text without ' and / \ only a-z A-Z and -

                        Regex ValFactor1 = new Regex(@"^\s*[a-zA-Z0-9,\s\-]+\s*$");
                        if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }

                    case ValidationType.MWDate:
                        break;
                    case ValidationType.MWInteger5:

                        // Limits to 5 left

                        ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}$");
                        if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }
                    case ValidationType.MWInteger12:

                        // Limits to 12 left

                        ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,12}$");
                        if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }

                    case ValidationType.MWBinary:

                        // Limits to 1 left

                        ValFactor1 = new Regex(@"(^([0]|[1])$)\d{0,1}$"); //Regex(@"^(?=.*[1]?.*$)\d{0,1}$");
                        if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }

                    case ValidationType.MWDouble5D1:

                        // Limits to 5 left and only 1 decimal

                        ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}(?:\.\d{0,2})?$");
                        if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }

                    case ValidationType.MWDouble5D2:

                        // Limits to 5 left and only 2 decimals

                        //ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}(?:\.\d{0,2})?$");
                        ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}(?:\.\d{0,1})?$");
                        if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }

                    case ValidationType.MWDouble12D2:

                        // Limits to 12 left and only 2 decimals

                        ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,12}(?:\.\d{0,2})?$");
                        if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }
                    case ValidationType.MWDouble5D3:

                        // Limits to 5 left and only 1 decimals

                        ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}(?:\.\d{0,3})?$");
                        if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }

                    case ValidationType.MWDouble5D4:

                        // Limits to 5 left and only 1 decimals

                        ValFactor1 = new Regex(@"^(?=.*[0-9]?.*$)\d{0,5}(?:\.\d{0,4})?$");
                        if (ValFactor1.IsMatch(_MWInput)) { return true; } else { return false; }


                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
