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
    private static string m_Email = "";
    private static string m_PasSectionid = "";
    private static string m_SystemAdmin = "";
    private static string m_RockMech = "";
    private static string m_Survey = "";
    private static string m_Sampling = "";
    private static string m_UserCat = "";
    private static string m_Site = "";
    private static string m_ChiefAuth = "";
    private static string m_ProdManAuth = "";
    private static string m_HRManAuth = "";
    private static string m_FinManAuth = "";
    private static string m_ChiefPlanAuth = "";

    private static string m_ProdMonth = "";

    private static string m_Survey_SnrMineSurveyor = "";
    private static string m_Survey_Geologist = "";
    private static string m_Survey_RockMech = "";
    private static string m_Survey_SnrMinePlanner = "";
    private static string m_Survey_MO = "";
    private static string m_Survey_SecMan = "";
    private static string m_Survey_SurvPlanMan = "";
    private static string m_Survey_ProdMan = "";


    public static string UserID { get { return m_UserID; } set { m_UserID = value; }}
    public static string UserName { get { return m_UserName; } set { m_UserName = value; }}
    public static string Email { get { return m_Email; } set { m_Email = value; } }
    public static string PasSectionid { get { return m_PasSectionid; } set { m_PasSectionid = value; } }
    public static string SystemAdmin { get { return m_SystemAdmin; } set { m_SystemAdmin = value; } }
    public static string RockMech { get { return m_RockMech; } set { m_RockMech = value; } }
    public static string Survey { get { return m_Survey; } set { m_Survey = value; } }
    public static string Sampling { get { return m_Sampling; } set { m_Sampling = value; } }
    public static string UserCat { get { return m_UserCat; } set { m_UserCat = value; } }
    public static string Site { get { return m_Site; } set { m_Site = value; } }
    public static string ChiefAuth { get { return m_ChiefAuth; } set { m_ChiefAuth = value; } }
    public static string ProdManAuth { get { return m_ProdManAuth; } set { m_ProdManAuth = value; } }
    public static string HRManAuth { get { return m_HRManAuth; } set { m_HRManAuth = value; } }
    public static string FinManAuth { get { return m_FinManAuth; } set { m_FinManAuth = value; } }
    public static string ChiefPlanAuth {get { return m_ChiefPlanAuth; } set { m_ChiefPlanAuth = value; } }

    public static string ProdMonth { get { return m_ProdMonth; } set { m_ProdMonth = value; } }
    public static string Survey_SnrMineSurveyor { get { return m_Survey_SnrMineSurveyor; } set { m_Survey_SnrMineSurveyor = value; } }
    public static string Survey_Geologist { get { return m_Survey_Geologist; } set { m_Survey_Geologist = value; } }
    public static string Survey_RockMech { get { return m_Survey_RockMech; } set { m_Survey_RockMech = value; } }
    public static string Survey_SnrMinePlanner { get { return m_Survey_SnrMinePlanner; } set { m_Survey_SnrMinePlanner = value; } }
    public static string Survey_MO { get { return m_Survey_MO; } set { m_Survey_MO = value; } }
    public static string Survey_SecMan { get { return m_Survey_SecMan; } set { m_Survey_SecMan = value; } }
    public static string Survey_SurvPlanMan { get { return m_Survey_SurvPlanMan; } set { m_Survey_SurvPlanMan = value; } }
    public static string Survey_ProdMan { get { return m_Survey_ProdMan; } set { m_Survey_ProdMan = value; } }



    #endregion class properties and globals
}


