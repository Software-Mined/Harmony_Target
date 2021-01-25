#region Comments and History
/*
 * =======================================================================================
 *   Author   : Schalk Kotze
 *   Date     : 02 May 2010
 *   Purpose  : Static class for Global Vars
 *              No Instance constructor needed
 *              
 * =======================================================================================
*/
#endregion Comments and History

static class clsUserInfo
{
    
 #region class properties and globals

    private static  string m_UserID = "";
    private static string m_UserName = "";
    private static string m_UserBookSection = "";
    private static int m_Hier = 0;
    private static string m_Tram = "";
    private static string m_Hoist = "";
    private static string m_mill = "";
    private static string m_book = "";
    private static string m_dropraise = "N";
    private static string m_sys = "N";
    private static string m_plan = "N";
    private static string m_samp = "N";
    private static string m_Surv = "N";
    private static string m_Expl = "N";
    private static string m_Diamond = "N";
    private static string m_TempBackDateBooking = "N";
    private static string m_TempCycleChange = "N";
    private static string m_PlanAuth = "";
    private static string m_CalChange = "";
    private static string m_MOMeas = "N";
    public static string m_GradeAuth = "N";
    public static string m_RBPlan = "N";
    public static string m_RBBook = "N";
    public static string m_NSBook = "N";
    public static string m_Geol = "N";
    public static string m_SampWO = "N";



    public static string m_SNCreate = "N";
    public static string m_SNAuthLvl1 = "N";
    public static string m_SNAuthLvl2 = "N";
    public static string m_SNAuthLvl3 = "N";
    public static string m_SNAuthLvl4 = "N";
    public static string m_SNAuthLvl5 = "N";
    public static string m_SNAuthLvl6 = "N";
    public static string m_SNAuthLvl7 = "N";
    public static string m_SNAuthLvl8 = "N";

    public static string m_SNLogon = "N";


    
   
 //   public static string M_Surv = "";
  
    
    
    
    
    
    public static string UserID { get { return m_UserID; } set { m_UserID = value; }}
    public static string UserName { get { return m_UserName; } set { m_UserName = value; }}
    public static string UserBookSection { get { return m_UserBookSection; } set { m_UserBookSection = value; } }
    public static int Hier { get { return m_Hier; } set { m_Hier = value; } }

    public static string Tram { get { return m_Tram; } set { m_Tram = value; } }
    public static string Hoist { get { return m_Hoist; } set { m_Hoist = value; } }
    public static string mill { get { return m_mill; } set { m_mill = value; } }
    public static string book { get { return m_book; } set { m_book = value; } }
    public static string dropraise { get { return m_dropraise; } set { m_dropraise = value; } }
    public static string sys { get { return m_sys; } set { m_sys = value; } }
    public static string plan { get { return m_plan; } set { m_plan = value; } }
    public static string samp { get { return m_samp; } set { m_samp = value; } }
    public static string Surv { get { return m_Surv; } set { m_Surv = value; } }
    public static string Expl { get { return m_Expl; } set { m_Expl = value; } }
    public static string DiamondDril { get { return m_Diamond; } set { m_Diamond = value; } }
    public static string TempBackDateBooking { get { return m_TempBackDateBooking; } set { m_TempBackDateBooking = value; } }
    public static string TempCycleChange { get { return m_TempCycleChange; } set { m_TempCycleChange = value; } }
    public static string PlanAuth { get { return m_PlanAuth; } set { m_PlanAuth = value; } }
    public static string CalChange { get { return m_CalChange; } set { m_CalChange = value; } }
    public static string MOMeas { get { return m_MOMeas; } set { m_MOMeas = value; } }
    public static string GradeAuth { get { return m_GradeAuth; } set { m_GradeAuth = value; } }

    public static string RBPlan { get { return m_RBPlan; } set { m_RBPlan = value; } }
    public static string RBBook { get { return m_RBBook; } set { m_RBBook = value; } }

    public static string NSBook { get { return m_NSBook; } set { m_NSBook = value; } }
    public static string Geol { get { return m_Geol; } set { m_Geol = value; } }
    public static string SampWO { get { return m_SampWO; } set { m_SampWO = value; } }



    public static string SNCreate { get { return m_SNCreate; } set { m_SNCreate = value; } }    
   

 #endregion class properties and globals
}


