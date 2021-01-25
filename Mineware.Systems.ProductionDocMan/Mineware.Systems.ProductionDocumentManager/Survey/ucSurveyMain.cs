using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using FastReport;
using System.IO;
using System.Text.RegularExpressions;
using FastReport.Utils;


//fingerprint references
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using DevExpress.XtraEditors;

namespace Mineware.Systems.DocumentManager
{
    public partial class ucSurveyMain : UserControl
    {
        Report theReport = new Report();


        string Answer = "";

        // Fingerprint variables
        IntPtr g_biokeyHandle2 = IntPtr.Zero;
        bool gConnected = false;
        byte[] g_FPBuffer;
        int g_FPBufferSize = 0;
        bool g_bIsTimeToDie = false;
        IntPtr g_Handle = IntPtr.Zero;
        IntPtr g_biokeyHandle = IntPtr.Zero;
        IntPtr g_FormHandle = IntPtr.Zero;
        int g_nWidth = 0;
        int g_nHeight = 0;
        bool g_IsRegister = false;
        int g_RegisterTimeCount = 0;
        int g_RegisterCount = 0;
        int MyID = 0;
        const int REGISTER_FINGER_COUNT = 3;

        byte[][] g_RegTmps = new byte[3][];
        byte[] g_RegTmp = new byte[2048];
        byte[] g_VerTmp = new byte[2048];
        string txtFPrintID;

        const int MESSAGE_FP_RECEIVED = 0x0400 + 6;

        [DllImport("user32.dll", EntryPoint = "SendMessageA", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        string FingerprintScanner = "N";


        string Notetype = "";

        string RepDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir;
        string Mine = "";
        public ucSurveyMain()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            
            
            string User = Environment.UserName;

            UserLbl.Text = User;

            //MWDataManager.clsDataAccess _dbManUser = new MWDataManager.clsDataAccess();
            //_dbManUser.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            //_dbManUser.SqlStatement = "select * from tbl_Users where UserID = '" + clsUserInfo.UserID + "' ";

            //_dbManUser.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbManUser.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbManUser.ResultsTableName = "Logon On";
            ////textBox1.Text = _dbManGraph.SqlStatement;
            //_dbManUser.ExecuteInstruction();

            //DataTable dtData = _dbManUser.ResultsDataTable;

            //if (dtData.Rows.Count < 1)
            //{
            //    MessageBox.Show("User has not been setup PLEASE CONTACT SYSTEM ADMINISTRATOR");
            //    Application.Exit();

            //}
            //else
            //{

            //    foreach (DataRow dr1 in dtData.Rows)
            //    {
            //        if (dr1["BusUnit"].ToString() == "TTRB")
            //        {
            //            MineLbl.Text = "TT";

            //            if (dr1["Lvl1"].ToString() == "Y")
            //                LvlLblRB.Text = "Lvl1";

            //            if (dr1["Lvl2"].ToString() == "Y")
            //                LvlLblRB.Text = "Lvl2";

            //            if (dr1["Lvl3"].ToString() == "Y")
            //                LvlLblRB.Text = "Lvl3";

            //            if (dr1["Lvl4"].ToString() == "Y")
            //                LvlLblRB.Text = "Lvl4";

            //            if (dr1["Lvl5"].ToString() == "Y")
            //                LvlLblRB.Text = "Lvl5";

            //            if (dr1["Lvl6"].ToString() == "Y")
            //                LvlLblRB.Text = "Lvl6";

            //            if (dr1["Lvl7"].ToString() == "Y")
            //                LvlLblRB.Text = "Lvl7";

            //            if (dr1["Lvl8"].ToString() == "Y")
            //                LvlLblRB.Text = "Lvl8";



            //        }
            //        else
            //        {
            //            if (dr1["Lvl1"].ToString() == "Y")
            //                LvlLbl.Text = "Lvl1";

            //            if (dr1["Lvl2"].ToString() == "Y")
            //                LvlLbl.Text = "Lvl2";

            //            if (dr1["Lvl3"].ToString() == "Y")
            //                LvlLbl.Text = "Lvl3";

            //            if (dr1["Lvl4"].ToString() == "Y")
            //                LvlLbl.Text = "Lvl4";

            //            if (dr1["Lvl5"].ToString() == "Y")
            //                LvlLbl.Text = "Lvl5";

            //            if (dr1["Lvl6"].ToString() == "Y")
            //                LvlLbl.Text = "Lvl6";

            //            if (dr1["Lvl7"].ToString() == "Y")
            //                LvlLbl.Text = "Lvl7";

            //            if (dr1["Lvl8"].ToString() == "Y")
            //                LvlLbl.Text = "Lvl8";



            //            MineLbl.Text = dr1["BusUnit"].ToString();
            //        }
            //        SectionLbl.Text = dr1["PasSectionid"].ToString();
            //}
            MineLbl.Text = "AM";
            Mine = "Amandelbult";

            //if (MineLbl.Text == "MP")
            //        Mine = "MW_MponengMine";
            //    if (MineLbl.Text == "GN")
            //        Mine = "MW_GreatNoligwaMine";
            //    if (MineLbl.Text == "KP")
            //        Mine = "MW_KopanangMine";
            //    if (MineLbl.Text == "SV")
            //        Mine = "MW_SavukaMine";
            //    if (MineLbl.Text == "TT")
            //        Mine = "MW_TauTonaMine";
            //    if (MineLbl.Text == "MK")
            //        Mine = "MW_MoabKhotsonMine";

            //MWDataManager.clsDataAccess _dbManSysSet = new MWDataManager.clsDataAccess();
            ////_dbManSysSet.ConnectionString = "server=zadcbpfpsql01.ag.ad.local;database=MW_PassStageDB;user id=MINEWARE;password=P@55@123 ";
            //_dbManSysSet.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            //_dbManSysSet.SqlStatement = "select * from dbo.tbl_SYSSET ";
            //_dbManSysSet.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbManSysSet.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbManSysSet.ResultsTableName = "SysSet";
            ////textBox1.Text = _dbManGraph.SqlStatement;
            //_dbManSysSet.ExecuteInstruction();

            //DataTable dtData2 = _dbManSysSet.ResultsDataTable;

            //RepDir = dtData2.Rows[0]["SERVERPATH"].ToString();


            LoadListBox();

            //LoadloadFP();
            //}



           
        }

        void LoadListBox()
        {
            //return;

            ListBox.Items.Clear();


                    MWDataManager.clsDataAccess _dbManLoadList = new MWDataManager.clsDataAccess();
                    _dbManLoadList.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                    _dbManLoadList.SqlStatement = "SELECT * FROM [MW_PassStageDB].[dbo].[tbl_SurveyNotesEmailData] " +
                                                "where emailadd in "+
                                                "(select email from [MW_PassStageDB].[dbo].[vw_GlobalUsers] where complogin = '" + UserLbl.Text + "') ";
                    _dbManLoadList.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManLoadList.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManLoadList.ResultsTableName = "Notes";
                    _dbManLoadList.ExecuteInstruction();



                    DataTable dt = _dbManLoadList.ResultsDataTable;

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ListBox.Items.Add(dr["mine"].ToString() + dr["TSNID"].ToString() + dr["Lvl"].ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("All notes Authorised");
                        Application.Exit();
                    }

                    
                    ListBox.SelectedIndex = 0;
                    NoteNoLbl.Text = (ListBox.SelectedIndex + 1).ToString();
                    NoteTotNoLbl.Text = ListBox.Items.Count.ToString();
               // }

                //else
                //{
                //    /////Load all notes\\\\\\\\\\\
                //    MWDataManager.clsDataAccess _dbManNotes = new MWDataManager.clsDataAccess();
                //    //_dbManNotes.ConnectionString = "server=zadcbpfpsql01.ag.ad.local;database="+Mine+";user id=MINEWARE;password=P@55@123 ";
                //    _dbManNotes.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                //    _dbManNotes.SqlStatement = "select a.*, b.SectionID, b.WorkplaceID from MW_MponengMine.dbo.TempStopNote_Autorisation a " +
                //                                " left outer join dbo.TempStopNote b on a.TSNID = b.TSNID " +
                //                                " where " + LvlLbl.Text + "User is null and b.sectionid like '" + SectionLbl.Text + "%' ";
                //    _dbManNotes.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //    _dbManNotes.queryReturnType = MWDataManager.ReturnType.DataTable;
                //    _dbManNotes.ResultsTableName = "Notes";
                //    //textBox1.Text = _dbManGraph.SqlStatement;
                //    _dbManNotes.ExecuteInstruction();

                //    DataTable dt = _dbManNotes.ResultsDataTable;

                //    if (dt.Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            ListBox.Items.Add(dr["TSNID"].ToString());
                //        }
                //    }
                //    ListBox.SelectedIndex = 0;
                //    NoteNoLbl.Text = (ListBox.SelectedIndex + 1).ToString();
                //    NoteTotNoLbl.Text = ListBox.Items.Count.ToString();
                //}

         
        }

        private void LoadSurveyNote()
        {

           // return;

            if (FPNoLbl.Text == "")
                return;

            MineLbl.Text = FPNoLbl.Text.Substring(0, 2);
            string noteno = Convert.ToString(Convert.ToInt32(FPNoLbl.Text.Substring(4, 5)));

            LvlLbl.Text = FPNoLbl.Text.Substring(9, 1);

                //return;

            string NMine = "";

            if (MineLbl.Text == "GN")
                NMine = "mw_GreatNoligwaMine";
            if (MineLbl.Text == "MK")
                NMine = "mw_MoabKhotsonMine";
            if (MineLbl.Text == "KP")
                NMine = "mw_KopanangMine";
            if (MineLbl.Text == "MP")
                NMine = "mw_MponengMine";
            if (MineLbl.Text == "TT")
                NMine = "mw_TauTonaMine";
            if (MineLbl.Text == "SV")
                NMine = "mw_SavukaMine";





           
                MWDataManager.clsDataAccess _dbManEmptyCubby = new MWDataManager.clsDataAccess();
               // _dbManEmptyCubby.ConnectionString = "server=zadcbpfpsql01.ag.ad.local;database=" + Mine + ";user id=MINEWARE;password=P@55@123 ";
                _dbManEmptyCubby.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _dbManEmptyCubby.SqlStatement = " select top(1) * from (select * from ( select a.*,  " +
                                           "   b.lvl1date, s1.sigfileno sigfileno1,   \r\n " +
                                           "   b.lvl2date, s2.sigfileno sigfileno2,   \r\n " +
                                           "   b.lvl3date, s3.sigfileno sigfileno3,  \r\n " +
                                           "   b.lvl4date, s4.sigfileno sigfileno4,  \r\n " +
                                           "   b.lvl5date, s5.sigfileno sigfileno5,  \r\n " +
                                           "   b.lvl6date, s6.sigfileno sigfileno6,  \r\n " +
                                           "   b.lvl7date, s7.sigfileno sigfileno7,  \r\n " +
                                           "   b.lvl8date, s8.sigfileno sigfileno8,  \r\n " +
                                           "  b.lvl9date, s9.sigfileno sigfileno9,  s11.sigfileno sigfilenouser  \r\n " +
                                           "  ,b.Lvl1Remks,  b.Lvl2Remks,  b.Lvl3Remks,  b.Lvl4Remks,  b.Lvl5Remks,  b.Lvl6Remks,  b.Lvl7Remks,  b.Lvl8Remks, b.Lvl9Remks \r\n " +
                                           "  , us1.username usnamelvl1, us2.username usnamelvl2, us3.username usnamelvl3, us4.username usnamelvl4, us5.username usnamelvl5, us6.username usnamelvl6, us7.username usnamelvl7, us8.username usnamelvl8, us9.username usnamelvl9 " +
                                           " ,case when us9.username is null then 'N' else 'Y' end as auth " +

                                           "  \r\n " +
                                           "  from (  \r\n " +
                                           "  select  TSNID ID, NoteType NoteType, w.WorkplaceID +':'+ description WP, MOSection +':'+ s1.Name MOSection,  TheDate Date, StopRemark StopRemark,   \r\n " +
                                           "  GenRemark Remark, PegNo PegFrom,  '0' a, PegDist Peg, Length Length, Width Depth, Height Height, ImageDate,    \r\n " +
                                           " SecManager +':'+ s2.Name ManagerSection, ToPeg2 PegTo, PegtoPegVal PegDist,  PegtoFLPVal PegFace, LHS LHS, RHS RHS, HW HW, FW FW,  \r\n " +
                                           "  CarrRailVal CarryRail, PegatChain ChainAtPeg,  PegatChainVal ChainAtPegVal, ChainatLP Chain, LPVal LP, PegReqRail PegToRail,  \r\n " +
                                           " ReqRailVal PegToRailVal,  RailVal Rail, PegSGradeVal1 SGrade1, PegSGradeVal2 SGrade2, PegSGradeVal3 SGrade3,  \r\n " +
                                           " PegSGradeVal4 SGrade4, PegStopDist StopDistPeg,  PegStopDistVal StopDistPegVal, PegVal PegVal, ToPeg2Face PegToFace,  \r\n " +
                                           "  AdvToDate AdvToDate, PegHoldDist HolDistPeg,  PegHoldDistVal HoldingDistPegVal, Grade Grade, PanelType PanelType, (select banner from " + NMine + ".dbo.SysSet) banner,  \r\n " +
                                           "  HolingNoteNo HolingNoteNo, Notes Notes, w.Description, s.Sectionid SID,s.Name SName,s1.Sectionid S1ID,s1.Name S1Name,s2.Sectionid S2ID,s2.Name S2Name, username   , w.WorkplaceID, null TheDate    \r\n " +

                                           " from  " + NMine + ".dbo.TempStopNote t, " + NMine + ".dbo.vw_Workplace_Total w ," + NMine + ".dbo.Section s," + NMine + ".dbo. Section s1," + NMine + ".dbo.Section s2   \r\n " +
                                           "  where TSNID = '" + noteno + "' and t.Workplaceid = convert(varchar(10),w.WorkplaceID)  \r\n " +
                                           "  and t.Sectionid = s.Sectionid and s.prodmonth = (select CurrentProductionMonth from " + NMine + ".dbo.SysSet) and s.ReportToSectionid = s1.Sectionid and s1.prodmonth =   \r\n " +
                                           " (select CurrentProductionMonth from " + NMine + ".dbo.SysSet) and s1.ReportToSectionid = s2.Sectionid and s2.prodmonth = (select CurrentProductionMonth from " + NMine + ".dbo.SysSet)   \r\n " +


                                           //" union  select  TSNID ID, NoteType NoteType, WorkplaceID WP, null MOSection,  TheDate Date, null StopRemark,   \r\n " +
                                           //"  notes Remark, null PegFrom,  '0' a, null Peg, null Length, null Depth, null Height, picture ImageDate,    \r\n " +
                                           //" null ManagerSection, null PegTo, null PegDist,  null PegFace, null LHS, null RHS, null HW, null FW,  \r\n " +
                                           //"  null CarryRail, null ChainAtPeg,  null ChainAtPegVal, null Chain, null LP, null PegToRail,  \r\n " +
                                           //" null PegToRailVal,  null Rail, null SGrade1, null SGrade2, null SGrade3,  \r\n " +
                                           //" null SGrade4, null StopDistPeg,  null StopDistPegVal, null PegVal, null PegToFace,  \r\n " +
                                           //"  null AdvToDate, null HolDistPeg,  null HoldingDistPegVal, null Grade, null PanelType, (select banner from " + NMine + ".dbo.SysSet) banner,  \r\n " +
                                           //"  null HolingNoteNo, null Notes, WorkplaceID Description, null SID,null SName,null S1ID,null S1Name,null S2ID,null S2Name, username   , WorkplaceID, TheDate    \r\n " +

                                           //" from  " + NMine + ".dbo.[tbl_ReefBoringLetters] t   \r\n " +
            
                                           //"  where TSNID = '" + noteno + "' \r\n " +
                                           
                                           
                                           
                                           
                                           
                                           " ) a \r\n " +
                                           " left outer join " + NMine + ".[dbo].[TempStopNote_Autorisation] b on a.id = b.TSNID \r\n " +


                                         //   "left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me on b.lvl1user = me.complogin \r\n " +
                                         " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl1 = 'Y') me on b.lvl1user = me.complogin \r\n " +


                                         "left outer join " + NMine + ".[dbo].[CPMUsers] us1 on me.userid = us1.userid \r\n " +

                                         //"left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me1 on b.lvl2user = me1.complogin    \r\n " +
                                         " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl2 = 'Y') me1 on b.lvl2user = me1.complogin \r\n " +

                                         "left outer join " + NMine + ".[dbo].[CPMUsers] us2 on me1.userid = us2.userid \r\n " +

                                         //"left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me2 on b.lvl3user = me2.complogin   \r\n " +
                                         " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl3 = 'Y') me2 on b.lvl3user = me2.complogin \r\n " +
                                        
                                         "left outer join " + NMine + ".[dbo].[CPMUsers] us3 on me2.userid = us3.userid \r\n " +

                                        // "left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me3 on b.lvl4user = me3.complogin   \r\n " +
                                          " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl4 = 'Y') me3 on b.lvl4user = me3.complogin \r\n " +
                                         "left outer join " + NMine + ".[dbo].[CPMUsers] us4 on me3.userid = us4.userid \r\n " +

                                        // "left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me4 on b.lvl5user = me4.complogin   \r\n " +
                                         " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl5 = 'Y') me4 on b.lvl5user = me4.complogin \r\n " +
                                         //" left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl4 = 'Y') me3 on b.lvl4user = me3.complogin \r\n " +
                                         "left outer join " + NMine + ".[dbo].[CPMUsers] us5 on me4.userid = us5.userid \r\n " +

                                         " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl6 = 'Y') me5 on b.lvl6user = me5.complogin \r\n " +
                                         //"left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me5 on b.lvl6user = me5.complogin  \r\n " +
                                         "left outer join " + NMine + ".[dbo].[CPMUsers] us6 on me5.userid = us6.userid \r\n " +

                                         " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl7 = 'Y') me6 on b.lvl7user = me6.complogin \r\n " +
                                         //"left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me6 on b.lvl7user = me6.complogin   \r\n " +
                                         "left outer join " + NMine + ".[dbo].[CPMUsers] us7 on me6.userid = us7.userid   \r\n " +

                                         " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl8 = 'Y') me7 on b.lvl8user = me7.complogin \r\n " +
                                        // "left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me7 on b.lvl8user = me7.complogin   \r\n " +
                                         "left outer join " + NMine + ".[dbo].[CPMUsers] us8 on me7.userid = us8.userid \r\n " +

                                         " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl9 = 'Y') me8 on b.lvl9user = me8.complogin \r\n " +
                                         //" left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me8 on b.lvl9user = me8.complogin  \r\n " +
                                         "left outer join " + NMine + ".[dbo].[CPMUsers] us9 on me8.userid = us9.userid \r\n " +

                                         " left outer join (select * from " + NMine + ".[dbo].CPMUsers_Department_Survey where lvl9 = 'Y') me9 on b.lvl9user = me9.complogin \r\n " +
                                        // "left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me9 on b.lvl9user = me9.complogin  \r\n " +
                                         "left outer join " + NMine + ".[dbo].[CPMUsers] s10 on me9.userid = s10.userid \r\n " +


                                          " left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s1 on us1.userid = s1.userid \r\n " +
                                       " left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s2 on us2.userid = s2.userid \r\n " +
                                       " left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s3 on us3.userid = s3.userid \r\n " +
                                       " left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s4 on us4.userid = s4.userid \r\n " +
                                       " left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s5 on us5.userid = s5.userid \r\n " +
                                       " left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s6 on us6.userid = s6.userid \r\n " +
                                       " left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s7 on us7.userid = s7.userid \r\n " +
                                       " left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s8 on us8.userid = s8.userid \r\n " +
                                       " left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s9 on us9.userid = s9.userid  \r\n " +
                                       //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s11 on s10.userid = s11.userid \r\n " +

                                       " left outer join  " + NMine + ".[dbo].[CPMUsers] us11 on a.username = us11.username \r\n " +
                                       " left outer join  " + NMine + ".[dbo].[CPMUsers_Attachments] s11 on us11.userid = s11.userid \r\n " +

                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s1 on b.lvl1user = s1.userid \r\n " +
                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s2 on b.lvl2user = s2.userid \r\n " +
                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s3 on b.lvl3user = s3.userid \r\n " +
                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s4 on b.lvl4user = s4.userid \r\n " +
                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s5 on b.lvl5user = s5.userid \r\n " +
                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s6 on b.lvl6user = s6.userid \r\n " +
                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s7 on b.lvl7user = s7.userid \r\n " +
                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s8 on b.lvl8user = s8.userid \r\n " +
                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s9 on b.lvl9user = s9.userid  \r\n " +
                                           //" left outer join " + NMine + ".[dbo].[CPMUsers] s10 on a.username = s10.username \r\n " +
                                           //" left outer join " + NMine + ". [dbo].[CPMUsers_Attachments] s11 on s10.userid = s11.userid \r\n " +

                                            //" left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me on b.lvl1user = me.complogin \r\n " +
                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s1 on me.userid = s1.userid \r\n " +

                                            //"  left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me1 on b.lvl2user = me1.complogin  \r\n " +
                                            //"  left outer join " + NMine + ".[dbo].CPMUsers_Attachments s2 on me1.userid = s2.userid \r\n " +

                                            //"  left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me2 on b.lvl3user = me2.complogin   \r\n " +
                                            //"  left outer join " + NMine + ".[dbo].CPMUsers_Attachments s3 on me2.userid = s3.userid \r\n " +

                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me3 on b.lvl4user = me3.complogin   \r\n " +
                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s4 on me3.userid = s4.userid \r\n " +

                                            //"  left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me4 on b.lvl5user = me4.complogin  \r\n " +
                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s5 on me4.userid = s5.userid \r\n " +

                                            //"  left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me5 on b.lvl6user = me5.complogin   \r\n " +
                                            //"  left outer join " + NMine + ".[dbo].CPMUsers_Attachments s6 on me5.userid = s6.userid \r\n " +

                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me6 on b.lvl7user = me6.complogin   \r\n " +
                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s7 on me6.userid = s7.userid   \r\n " +

                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me7 on b.lvl8user = me7.complogin   \r\n " +
                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s8 on me7.userid = s8.userid \r\n " +

                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me8 on b.lvl9user = me8.complogin  \r\n " +
                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s9 on me8.userid = s9.userid \r\n " +

                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me9 on b.lvl8user = me9.complogin  \r\n " +
                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s10 on me9.userid = s10.userid \r\n " +

                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Department_Survey me10 on b.lvl8user = me10.complogin  \r\n " +
                                            // " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s11 on me10.userid = s11.userid \r\n " +




                                             //" left outer join " + NMine + ".[dbo].[CPMUsers] us1 on b.lvl1user = us1.userid \r\n " +
                                             //" left outer join " + NMine + ".[dbo].[CPMUsers] us2 on b.lvl2user = us2.userid \r\n " +
                                             //" left outer join " + NMine + ".[dbo].[CPMUsers] us3 on b.lvl3user = us3.userid \r\n " +
                                             //" left outer join " + NMine + ".[dbo].[CPMUsers] us4 on b.lvl4user = us4.userid \r\n " +
                                             //" left outer join " + NMine + ".[dbo].[CPMUsers] us5 on b.lvl5user = us5.userid \r\n " +
                                             //" left outer join " + NMine + ".[dbo].[CPMUsers] us6 on b.lvl6user = us6.userid \r\n " +
                                             //" left outer join " + NMine + ".[dbo].[CPMUsers] us7 on b.lvl7user = us7.userid \r\n " +
                                             //" left outer join " + NMine + ".[dbo].[CPMUsers] us8 on b.lvl8user = us8.userid \r\n " +
                                             //" left outer join " + NMine + ".[dbo].[CPMUsers] us9 on b.lvl9user = us9.userid \r\n " +

                                            //"left outer join mw_MoabKhotsonMine.[dbo].[CPMUsers] us1 on b.lvl1user = us1.username  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].[CPMUsers] us2 on b.lvl2user = us2.username  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].[CPMUsers] us3 on b.lvl3user = us3.username  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].[CPMUsers] us4 on b.lvl4user = us4.username  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].[CPMUsers] us5 on b.lvl5user = us5.username  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].[CPMUsers] us6 on b.lvl6user = us6.username  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].[CPMUsers] us7 on b.lvl7user = us7.username  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].[CPMUsers] us8 on b.lvl8user = us8.username  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].[CPMUsers] us9 on b.lvl9user = us9.username \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s1 on us1.userid = s1.userid  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s2 on us2.userid = s2.userid  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s3 on us3.userid = s3.userid  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s4 on us4.userid = s4.userid  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s5 on us5.userid = s5.userid  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s6 on us6.userid = s6.userid  \r\n " + 
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s7 on us7.userid = s7.userid  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s8 on us8.userid = s8.userid  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s9 on us9.userid = s9.userid  \r\n " +
                                            //"left outer join mw_MoabKhotsonMine.[dbo].CPMUsers_Attachments s11 on us1.userid = s11.userid \r\n " +



                                             "  ) a) a union "+
                                             "  select a.*,  " +
                                           "   b.lvl1date, s1.sigfileno sigfileno1,   \r\n " +
                                           "   b.lvl2date, s2.sigfileno sigfileno2,   \r\n " +
                                           "   b.lvl3date, s3.sigfileno sigfileno3,  \r\n " +
                                           "   b.lvl4date, s4.sigfileno sigfileno4,  \r\n " +
                                           "   b.lvl5date, s5.sigfileno sigfileno5,  \r\n " +
                                           "   b.lvl6date, s6.sigfileno sigfileno6,  \r\n " +
                                           "   b.lvl7date, s7.sigfileno sigfileno7,  \r\n " +
                                           "   b.lvl8date, s8.sigfileno sigfileno8,  \r\n " +
                                           "  b.lvl9date, s9.sigfileno sigfileno9,  s11.sigfileno sigfilenouser  \r\n " +
                                           "  ,b.Lvl1Remks,  b.Lvl2Remks,  b.Lvl3Remks,  b.Lvl4Remks,  b.Lvl5Remks,  b.Lvl6Remks,  b.Lvl7Remks,  b.Lvl8Remks, b.Lvl9Remks \r\n " +
                                           "  , us1.username usnamelvl1, us2.username usnamelvl2, us3.username usnamelvl3, us4.username usnamelvl4, us5.username usnamelvl5, us6.username usnamelvl6, us7.username usnamelvl7, us8.username usnamelvl8, us9.username usnamelvl9 " +
                                           " ,case when us7.username is null then 'N' else 'Y' end as auth " +

                                           "  \r\n " +
                                           "  from (  \r\n " +
                                           //"  select  TSNID ID, NoteType NoteType, w.WorkplaceID +':'+ description WP, MOSection +':'+ s1.Name MOSection,  TheDate Date, StopRemark StopRemark,   \r\n " +
                                           //"  GenRemark Remark, PegNo PegFrom,  '0' a, PegDist Peg, Length Length, Width Depth, Height Height, ImageDate,    \r\n " +
                                           //" SecManager +':'+ s2.Name ManagerSection, ToPeg2 PegTo, PegtoPegVal PegDist,  PegtoFLPVal PegFace, LHS LHS, RHS RHS, HW HW, FW FW,  \r\n " +
                                           //"  CarrRailVal CarryRail, PegatChain ChainAtPeg,  PegatChainVal ChainAtPegVal, ChainatLP Chain, LPVal LP, PegReqRail PegToRail,  \r\n " +
                                           //" ReqRailVal PegToRailVal,  RailVal Rail, PegSGradeVal1 SGrade1, PegSGradeVal2 SGrade2, PegSGradeVal3 SGrade3,  \r\n " +
                                           //" PegSGradeVal4 SGrade4, PegStopDist StopDistPeg,  PegStopDistVal StopDistPegVal, PegVal PegVal, ToPeg2Face PegToFace,  \r\n " +
                                           //"  AdvToDate AdvToDate, PegHoldDist HolDistPeg,  PegHoldDistVal HoldingDistPegVal, Grade Grade, PanelType PanelType, (select banner from " + NMine + ".dbo.SysSet) banner,  \r\n " +
                                           //"  HolingNoteNo HolingNoteNo, Notes Notes, w.Description, s.Sectionid SID,s.Name SName,s1.Sectionid S1ID,s1.Name S1Name,s2.Sectionid S2ID,s2.Name S2Name, username   , w.WorkplaceID, null TheDate    \r\n " +

                                           //" from  " + NMine + ".dbo.TempStopNote t, " + NMine + ".dbo.Workplace w ," + NMine + ".dbo.Section s," + NMine + ".dbo. Section s1," + NMine + ".dbo.Section s2   \r\n " +
                                           //"  where TSNID = '" + noteno + "' and t.Workplaceid = w.Workplaceid  \r\n " +
                                           //"  and t.Sectionid = s.Sectionid and s.prodmonth = (select CurrentProductionMonth from " + NMine + ".dbo.SysSet) and s.ReportToSectionid = s1.Sectionid and s1.prodmonth =   \r\n " +
                                           //" (select CurrentProductionMonth from " + NMine + ".dbo.SysSet) and s1.ReportToSectionid = s2.Sectionid and s2.prodmonth = (select CurrentProductionMonth from " + NMine + ".dbo.SysSet)   \r\n " +


                                           " select  TSNID ID, NoteType NoteType, WorkplaceID WP, null MOSection,  TheDate Date, null StopRemark,   \r\n " +
                    "  notes Remark, null PegFrom,  '0' a, null Peg, null Length, null Depth, null Height, picture ImageDate,    \r\n " +
                    " null ManagerSection, null PegTo, null PegDist,  null PegFace, null LHS, null RHS, null HW, null FW,  \r\n " +
                    "  null CarryRail, null ChainAtPeg,  null ChainAtPegVal, null Chain, null LP, null PegToRail,  \r\n " +
                    " null PegToRailVal,  null Rail, null SGrade1, null SGrade2, null SGrade3,  \r\n " +
                    " null SGrade4, null StopDistPeg,  null StopDistPegVal, null PegVal, null PegToFace,  \r\n " +
                    "  null AdvToDate, null HolDistPeg,  null HoldingDistPegVal, null Grade, null PanelType, (select banner from " + NMine + ".dbo.SysSet) banner,  \r\n " +
                    "  null HolingNoteNo, null Notes, WorkplaceID Description, null SID,null SName,null S1ID,null S1Name,null S2ID,null S2Name, username   , WorkplaceID, TheDate    \r\n " +

                                           " from  " + NMine + ".dbo.[tbl_ReefBoringLetters] t   \r\n " +

                                           "  where TSNID = '" + noteno + "' \r\n " +





                                           " ) a \r\n " +
                                           " left outer join " + NMine + ".[dbo].[tbl_ReefBoringLetters_Autorisation] b on a.id = b.TSNID \r\n " +


                                           //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s1 on b.lvl1user = s1.userid \r\n " +
                    //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s2 on b.lvl2user = s2.userid \r\n " +
                    //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s3 on b.lvl3user = s3.userid \r\n " +
                    //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s4 on b.lvl4user = s4.userid \r\n " +
                    //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s5 on b.lvl5user = s5.userid \r\n " +
                    //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s6 on b.lvl6user = s6.userid \r\n " +
                    //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s7 on b.lvl7user = s7.userid \r\n " +
                    //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s8 on b.lvl8user = s8.userid \r\n " +
                    //" left outer join " + NMine + ".[dbo].[CPMUsers_Attachments] s9 on b.lvl9user = s9.userid  \r\n " +
                    //" left outer join " + NMine + ".[dbo].[CPMUsers] s10 on a.username = s10.username \r\n " +
                    //" left outer join " + NMine + ". [dbo].[CPMUsers_Attachments] s11 on s10.userid = s11.userid \r\n " +

                                            " left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me on b.lvl1user = me.complogin \r\n " +
                                             " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s1 on me.userid = s1.userid \r\n " +

                                            "  left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me1 on b.lvl2user = me1.complogin  \r\n " +
                                            "  left outer join " + NMine + ".[dbo].CPMUsers_Attachments s2 on me1.userid = s2.userid \r\n " +

                                            "  left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me2 on b.lvl3user = me2.complogin   \r\n " +
                                            "  left outer join " + NMine + ".[dbo].CPMUsers_Attachments s3 on me2.userid = s3.userid \r\n " +

                                             " left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me3 on b.lvl4user = me3.complogin   \r\n " +
                                             " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s4 on me3.userid = s4.userid \r\n " +

                                            "  left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me4 on b.lvl5user = me4.complogin  \r\n " +
                                             " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s5 on me4.userid = s5.userid \r\n " +

                                            "  left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me5 on b.lvl6user = me5.complogin   \r\n " +
                                            "  left outer join " + NMine + ".[dbo].CPMUsers_Attachments s6 on me5.userid = s6.userid \r\n " +

                                             " left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me6 on b.lvl7user = me6.complogin   \r\n " +
                                             " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s7 on me6.userid = s7.userid   \r\n " +

                                             " left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me7 on b.lvl8user = me7.complogin   \r\n " +
                                             " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s8 on me7.userid = s8.userid \r\n " +

                                             " left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me8 on b.lvl9user = me8.complogin  \r\n " +
                                             " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s9 on me8.userid = s9.userid \r\n " +

                                             " left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me9 on b.lvl8user = me9.complogin  \r\n " +
                                             " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s10 on me9.userid = s10.userid \r\n " +

                                             " left outer join " + NMine + ".[dbo].[CPMUsers_Department_ReefBoring] me10 on b.lvl8user = me10.complogin  \r\n " +
                                             " left outer join " + NMine + ".[dbo].CPMUsers_Attachments s11 on a.username = s11.userid \r\n " +




                                             " left outer join " + NMine + ".[dbo].[CPMUsers] us1 on me.userid = us1.userid \r\n " +
                                             " left outer join " + NMine + ".[dbo].[CPMUsers] us2 on me1.userid = us2.userid \r\n " +
                                             " left outer join " + NMine + ".[dbo].[CPMUsers] us3 on me2.userid = us3.userid \r\n " +
                                             " left outer join " + NMine + ".[dbo].[CPMUsers] us4 on me3.userid = us4.userid \r\n " +
                                             " left outer join " + NMine + ".[dbo].[CPMUsers] us5 on me4.userid = us5.userid \r\n " +
                                             " left outer join " + NMine + ".[dbo].[CPMUsers] us6 on me5.userid = us6.userid \r\n " +
                                             " left outer join " + NMine + ".[dbo].[CPMUsers] us7 on me6.userid = us7.userid \r\n " +
                                             " left outer join " + NMine + ".[dbo].[CPMUsers] us8 on b.lvl8user = us8.userid \r\n " +
                                             " left outer join " + NMine + ".[dbo].[CPMUsers] us9 on a.username = us9.userid \r\n " +



                                             "  ";


                _dbManEmptyCubby.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManEmptyCubby.queryReturnType = MWDataManager.ReturnType.DataTable;
             
                _dbManEmptyCubby.ResultsTableName = "EmptyCubby";
                _dbManEmptyCubby.ExecuteInstruction();

                DataTable Neil = _dbManEmptyCubby.ResultsDataTable;

                string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();            

                MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
                _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];


                Notetype = "";


                //MWDataManager.clsDataAccess _dbManSysSet = new MWDataManager.clsDataAccess();
                ////_dbManSysSet.ConnectionString = "server=zadcbpfpsql01.ag.ad.local;database=MW_PassStageDB;user id=MINEWARE;password=P@55@123 ";
                //_dbManSysSet.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                //_dbManSysSet.SqlStatement = "select * from " + NMine + ".dbo.SYSSET ";
                //_dbManSysSet.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbManSysSet.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbManSysSet.ResultsTableName = "SysSet";
                ////textBox1.Text = _dbManGraph.SqlStatement;
                //_dbManSysSet.ExecuteInstruction();

                //DataTable dtData2 = _dbManSysSet.ResultsDataTable;

                //RepDir = dtData2.Rows[0]["repdir"].ToString();


                

            if (Neil.Rows[0]["notetype"].ToString() == "RB")
            {

                Notetype = "RB";

                if (MineLbl.Text == "GN")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\ReefDrilling\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "MK")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\ReefDrilling\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "KP")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\ReefDrilling\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "MP")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\ReefDrilling\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "TT")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\ReefDrilling\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "SV")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\ReefDrilling\" + ImageDateLbl + ".png" + "' ";
            }
            else
            {


                if (MineLbl.Text == "GN")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "MK")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "KP")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "MP")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "TT")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";
                if (MineLbl.Text == "SV")
                    _dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";

            }
                //_dbManEmptyCubby1.SqlStatement = "Select '" + RepDir + @"\Mponeng\SurveyLetters\" + ImageDateLbl + ".png" + "' ";

                _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManEmptyCubby1.ResultsTableName = "Image";
                _dbManEmptyCubby1.ExecuteInstruction();

                DataSet dsEmpty = new DataSet();
                // DataSet dsGraph = new DataSet();
                dsEmpty.Tables.Add(_dbManEmptyCubby.ResultsDataTable);

                theReport.RegisterData(dsEmpty);

                DataSet dsEmpty1 = new DataSet();
                // DataSet dsGraph = new DataSet();
                dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

                theReport.RegisterData(dsEmpty1);

           

            if (Notetype != "RB")
            {

                if (MineLbl.Text == "MP" || MineLbl.Text == "TT" || MineLbl.Text == "SV")
                {                    
                    theReport.Load(" \\\\zadcbpfpweb01.ag.ad.local\\WPAS\\surveyNoteDevA3.frx");
                }
                else
                {
                    theReport.Load(" \\\\zadcbpfpweb01.ag.ad.local\\WPAS\\surveyNote.frx");
                }
            }
            else
                theReport.Load(" \\\\zadcbpfpweb01.ag.ad.local\\WPAS\\ReefboringNote2.frx");

           // theReport.Load("c:\\WPAS\\surveyNote.frx");
           //theReport.Design();

            ReportPC.Clear();
            theReport.Prepare();
            theReport.Preview = ReportPC;
            theReport.ShowPrepared();

        }

        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        void LoadloadFP()
        {
           
            try
            {
                g_FormHandle = this.Handle;

                ConnectScanner();

                //Load Fingerprints
                MWDataManager.clsDataAccess _dbManFP = new MWDataManager.clsDataAccess();
                _dbManFP.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                
                    _dbManFP.SqlStatement = " select * from [vw_GlobalUsers] where fprintid is not null";
               
                _dbManFP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManFP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManFP.ExecuteInstruction();
                DataTable dtFP = _dbManFP.ResultsDataTable;

                int MyID;
                int size;

                foreach (DataRow dr in dtFP.Rows)
                {
                    MyID = Convert.ToInt32(dr["FUserID"].ToString());

                    byte[] NewArray = new byte[2048];                   

                    txtFPrintID = dr["FPrintID"].ToString();
                    string qq = dr["FPrintID"].ToString();
                    size = Convert.ToInt32(dr["FPrintSize"].ToString());

                    for (int i = 0; i < 2047; i++)
                    {
                        //Find the comma         
                        if (txtFPrintID.Length > 0)
                        {
                            NewArray[i] = Convert.ToByte(qq.Substring(0, qq.IndexOf(',')));
                            qq = qq.Substring(qq.IndexOf(',') + 1, qq.Length - (qq.IndexOf(',') + 1));
                        }
                    }

                    fpslib.BIOKEY_DB_ADD(g_biokeyHandle, MyID, size, NewArray);

                }
                FingerprintScanner = "Y";
            }
            catch
            {
                MessageBox.Show("No Fingerprint Device Detected");
                FingerprintScanner = "N";
            }
        }

        public void ConnectScanner()
        {
            if (!gConnected)
            {
                int ret = 0;
                byte[] paramValue = new byte[64];

                // Enable log
                Array.Clear(paramValue, 0, paramValue.Length);
                paramValue[0] = 1;

                zkfpri.sensorSetParameterEx(g_Handle, 1100, paramValue, 4);


                ret = zkfpri.sensorInit();
                if (ret != 0)
                {
                    MessageBox.Show("Init Failed, ret=" + ret.ToString());
                    return;
                }
                g_Handle = zkfpri.sensorOpen(0);

                Array.Clear(paramValue, 0, paramValue.Length);
                zkfpri.sensorGetVersion(paramValue, paramValue.Length);

                ret = paramValue.Length;
                Array.Clear(paramValue, 0, paramValue.Length);
                zkfpri.sensorGetParameterEx(g_Handle, 1, paramValue, ref ret);
                g_nWidth = BitConverter.ToInt32(paramValue, 0);

                //this.picFP.Width = g_nWidth;



                ret = paramValue.Length;
                Array.Clear(paramValue, 0, paramValue.Length);
                zkfpri.sensorGetParameterEx(g_Handle, 2, paramValue, ref ret);
                g_nHeight = BitConverter.ToInt32(paramValue, 0);

                //this.picFP.Height = g_nHeight;

                ret = paramValue.Length;
                Array.Clear(paramValue, 0, paramValue.Length);
                zkfpri.sensorGetParameterEx(g_Handle, 106, paramValue, ref ret);
                g_FPBufferSize = BitConverter.ToInt32(paramValue, 0);



                g_FPBuffer = new byte[g_FPBufferSize];
                Array.Clear(g_FPBuffer, 0, g_FPBuffer.Length);




                // get vid&pid
                ret = paramValue.Length;
                Array.Clear(paramValue, 0, paramValue.Length);
                zkfpri.sensorGetParameterEx(g_Handle, 1015, paramValue, ref ret);
                int nVid = BitConverter.ToInt16(paramValue, 0);
                int nPid = BitConverter.ToInt16(paramValue, 2);

                // Manufacturer
                ret = paramValue.Length;
                Array.Clear(paramValue, 0, paramValue.Length);
                zkfpri.sensorGetParameterEx(g_Handle, 1101, paramValue, ref ret);
                string manufacturer = System.Text.Encoding.ASCII.GetString(paramValue);
                // Product
                ret = paramValue.Length;
                Array.Clear(paramValue, 0, paramValue.Length);
                zkfpri.sensorGetParameterEx(g_Handle, 1102, paramValue, ref ret);
                string product = System.Text.Encoding.ASCII.GetString(paramValue);
                // SerialNumber
                ret = paramValue.Length;
                Array.Clear(paramValue, 0, paramValue.Length);
                zkfpri.sensorGetParameterEx(g_Handle, 1103, paramValue, ref ret);
                string serialNumber = System.Text.Encoding.ASCII.GetString(paramValue);


                // char paramValue[64] = {0};
                // int ret = sizeof(paramValue);

                // Get Vendor Info
                ret = paramValue.Length;
                zkfpri.sensorGetParameterEx(g_Handle, 1104, paramValue, ref ret); // Read customized data, the check.






                // Fingerprint Alg
                short[] iSize = new short[24];
                iSize[0] = (short)g_nWidth;
                iSize[1] = (short)g_nHeight;
                iSize[20] = (short)g_nWidth;
                iSize[21] = (short)g_nHeight; ;
                g_biokeyHandle = fpslib.BIOKEY_INIT(0, iSize, null, null, 0);
                if (g_biokeyHandle == IntPtr.Zero)
                {
                    MessageBox.Show("BIOKEY_INIT failed");
                    return;
                }

                // Set allow 360 angle of Press Finger
                fpslib.BIOKEY_SET_PARAMETER(g_biokeyHandle, 4, 180);

                // Set Matching threshold
                fpslib.BIOKEY_MATCHINGPARAM(g_biokeyHandle, 0, fpslib.THRESHOLD_MIDDLE);

                // Init RegTmps
                for (int i = 0; i < 3; i++)
                {
                    g_RegTmps[i] = new byte[2048];
                }

                Thread captureThread = new Thread(new ThreadStart(DoCapture));
                captureThread.IsBackground = true;
                captureThread.Start();
                g_bIsTimeToDie = false;

                gConnected = true;
                //btnRegister.Enabled = true;
                //btnVerify.Enabled = true;
                //btnConnect.Text = "Disconnect Sensor";

               // txtPrompt.Text = "Please put your finger on the sensor";


            }
            else
            {
                FreeSensor();

                fpslib.BIOKEY_DB_CLEAR(g_biokeyHandle);
                fpslib.BIOKEY_CLOSE(g_biokeyHandle);

                gConnected = false;
                //btnRegister.Enabled = false;
                //btnVerify.Enabled = false;
                //btnConnect.Text = "Connect Sensor";
            }
        }


        private void FreeSensor()
        {
            g_bIsTimeToDie = true;
            Thread.Sleep(100);
            zkfpri.sensorClose(g_Handle);

            // Disable log
            byte[] paramValue = new byte[4];
            paramValue[0] = 0;
            zkfpri.sensorSetParameterEx(g_Handle, 1100, paramValue, 4);

            zkfpri.sensorFree();
        }

        private void DoCapture()
        {
            while (!g_bIsTimeToDie)
            {
                int ret = zkfpri.sensorCapture(g_Handle, g_FPBuffer, g_FPBufferSize);

                if (ret > 0)
                {
                    SendMessage(g_FormHandle, MESSAGE_FP_RECEIVED, IntPtr.Zero, IntPtr.Zero);
                }
                Thread.Sleep(30);


            }
        }


        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MESSAGE_FP_RECEIVED:
                    {
                        try
                        {
                            int ret = 0;
                            int id = 0;
                            int score = 0;
                            int quality = 0;


                            if (g_IsRegister)
                            {

                            }
                            else
                            {
                                byte[] NewArray = new byte[2048];

                                string qq = txtFPrintID;
                                for (int i = 0; i < 2047; i++)
                                {
                                    //Find the comma         
                                    if (txtFPrintID.Length > 0)
                                    {
                                        NewArray[i] = Convert.ToByte(qq.Substring(0, qq.IndexOf(',')));
                                        qq = qq.Substring(qq.IndexOf(',') + 1, qq.Length - (qq.IndexOf(',') + 1));
                                    }
                                }

                                Array.Clear(g_VerTmp, 0, g_VerTmp.Length);
                                if ((ret = fpslib.BIOKEY_EXTRACT(g_biokeyHandle, g_FPBuffer, g_VerTmp, 0)) > 0)
                                {
                                    // Get fingerprint quality
                                    quality = fpslib.BIOKEY_GETLASTQUALITY();
                                    //txtQuality.Text = quality.ToString();

                                    ret = fpslib.BIOKEY_IDENTIFYTEMP(g_biokeyHandle, g_VerTmp, ref id, ref score);


                                    if (ret > 0)
                                    {
                                        
                                        //TextLbl.Text = "Identification success"; //'" + lblUerID.Text + "'";
                                        FinalUserLbl.Text = id.ToString();


                                        if (FinalUserLbl.Text != "")
                                        {


                                            MWDataManager.clsDataAccess _dbManzz = new MWDataManager.clsDataAccess();
                                            _dbManzz.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                                            _dbManzz.SqlStatement = " select * from vw_GlobalUsers where FUserID = '" + FinalUserLbl.Text + "'  ";
                                            _dbManzz.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                                            _dbManzz.queryReturnType = MWDataManager.ReturnType.DataTable;
                                            _dbManzz.ExecuteInstruction();


                                            FinalUseraaLbl.Text = _dbManzz.ResultsDataTable.Rows[0][1].ToString();
                                            CompName.Text = _dbManzz.ResultsDataTable.Rows[0][5].ToString();
                                        }

                                        


                                    }
                                    else
                                    {
                                        //TextLbl.Text = string.Format("Identification failed, score={0}", score);
                                        TextLbl.Text = "Identification failed";
                                    }
                                }
                                else
                                {
                                    TextLbl.Text = "Extract template failed";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }


        private void FPNoLbl_TextChanged(object sender, EventArgs e)
        {
           // return;
            LoadSurveyNote();
          //  LoadloadFP();
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            FPNoLbl.Text = ListBox.SelectedItem.ToString();
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {

        }

        private void PrevBtn_Click(object sender, EventArgs e)
        {

            if (ListBox.SelectedIndex > 0)
            {
                ListBox.SelectedIndex = ListBox.SelectedIndex -1;
                NoteNoLbl.Text = (ListBox.SelectedIndex + 1).ToString();
                NoteTotNoLbl.Text = ListBox.Items.Count.ToString();
            }
            else
            {
                ListBox.SelectedIndex = ListBox.Items.Count - 1;
                NoteNoLbl.Text = (ListBox.SelectedIndex + 1).ToString();
                NoteTotNoLbl.Text = ListBox.Items.Count.ToString();
            }

        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            if (ListBox.SelectedIndex < ListBox.Items.Count -1)
            {
                ListBox.SelectedIndex = ListBox.SelectedIndex + 1;
                NoteNoLbl.Text = (ListBox.SelectedIndex + 1).ToString();
                NoteTotNoLbl.Text = ListBox.Items.Count.ToString();
            }
            else
            {
                ListBox.SelectedIndex = 0;
                NoteNoLbl.Text = (ListBox.SelectedIndex + 1).ToString();
                NoteTotNoLbl.Text = ListBox.Items.Count.ToString();
            }
        }

        private void AcceptBTN_Click(object sender, EventArgs e)
        {
            if (FingerprintScanner == "N")
                return;


            TextLbl.Text = "Place Finger on Scanner to Authorise";
            Authlabel.Text = "Y";
            Answer = "Accept SN";



            //set user lable
            //UserLbl.Text = "MINEWARE";

            g_IsRegister = false;


            
            
            
            
           // AcceptBtn1_Click(null, null);          
        }

        private void AcceptBtn1_Click(object sender, EventArgs e)
        {

            if (CompName.Text != UserLbl.Text)
            {

                MessageBox.Show("Incorrect user trying to Authorise note");

                return;
            }

            TextLbl.Text = "Identification success";


            string NLvl = "";

            if (LvlLbl.Text == "1")
                NLvl = "Lvl1";
            if (LvlLbl.Text == "2")
                NLvl = "Lvl2";
            if (LvlLbl.Text == "3")
                NLvl = "Lvl3";
            if (LvlLbl.Text == "4")
                NLvl = "Lvl4";
            if (LvlLbl.Text == "5")
                NLvl = "Lvl5";
            if (LvlLbl.Text == "6")
                NLvl = "Lvl6";
            if (LvlLbl.Text == "7")
                NLvl = "Lvl7";
            if (LvlLbl.Text == "8")
                NLvl = "Lvl";

            string NMine = "";

            if (MineLbl.Text == "GN")
                NMine = "mw_GreatNoligwaMine";
            if (MineLbl.Text == "MK")
                NMine = "mw_MoabKhotsonMine";
            if (MineLbl.Text == "KP")
                NMine = "mw_KopanangMine";
            if (MineLbl.Text == "MP")
                NMine = "mw_MponengMine";
            if (MineLbl.Text == "TT")
                NMine = "mw_TauTonaMine";
            if (MineLbl.Text == "SV")
                NMine = "mw_SavukaMine";

            if (Authlabel.Text == "Y")
           {

                MWDataManager.clsDataAccess _dbManUpdateStuff = new MWDataManager.clsDataAccess();
                _dbManUpdateStuff.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];


                if (Notetype != "RB")
                _dbManUpdateStuff.SqlStatement = " update " + NMine + ".dbo.TempStopNote_Autorisation set " + NLvl + "date = getdate(), " + NLvl + "remks = '" + RemBox.Text + "', " + NLvl + "OK = 'Yes',  " + NLvl + "User = '" + UserLbl.Text + "' where tsnid =  '" + Convert.ToString(Convert.ToInt32(FPNoLbl.Text.Substring(4, 5))) + "' ";


                if (Notetype == "RB")
                    _dbManUpdateStuff.SqlStatement = " update " + NMine + ".dbo.tbl_ReefBoringLetters_Autorisation set " + NLvl + "date = getdate(), " + NLvl + "remks = '" + RemBox.Text + "', " + NLvl + "OK = 'Yes',  " + NLvl + "User = '" + UserLbl.Text + "' where tsnid =  '" + Convert.ToString(Convert.ToInt32(FPNoLbl.Text.Substring(4, 5))) + "' ";
                
                
                _dbManUpdateStuff.SqlStatement = _dbManUpdateStuff.SqlStatement + " exec " + NMine + ".dbo.sp_SurveyLetterEmailData ";

                _dbManUpdateStuff.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManUpdateStuff.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManUpdateStuff.ExecuteInstruction();

                RemBox.Text = _dbManUpdateStuff.SqlStatement.ToString();


                MessageFrm MsgFrm = new MessageFrm();
                MsgFrm.Text = "Record Update";
                Procedures.MsgText = "Survey Note updated";
                MsgFrm.Show();



                RemBox.Text = "";
                FinalUserLbl.Text = "";
                CompName.Text = "";
                FinalUseraaLbl.Text = "";

                Authlabel.Text = "N";


           }

                    

            LoadListBox();

        }

        private void FinalUserLbl_TextChanged(object sender, EventArgs e)
        {
            //if (FinalUserLbl.Text != "")
            //{
            //    if (Answer == "Accept SN")
            //    {
            //        AcceptBtn1_Click(null, null);
            //        //MessageBox.Show("Accept");
            //    }

            //    if (Answer == "Decline SN")
            //    {
            //        DeclineBtn1_Click(null, null);
            //    }
            //}
        }

        private void DeclineBTN_Click(object sender, EventArgs e)
        {
            if (FingerprintScanner == "N")
                return;


            TextLbl.Text = "Place Finger on Scanner to Authorise";
            Answer = "Decline SN";
            Authlabel.Text = "Y";
            //set user lable
            //UserLbl.Text = "MINEWARE";

            g_IsRegister = false;
           
        }

        private void DeclineBtn1_Click(object sender, EventArgs e)
        {

            if (CompName.Text != UserLbl.Text)
            {

                MessageBox.Show("Incorrect user trying to Authorise note");

                return;
            }

            TextLbl.Text = "Identification success";


            string NLvl = "";

            if (LvlLbl.Text == "1")
                NLvl = "Lvl1";
            if (LvlLbl.Text == "2")
                NLvl = "Lvl2";
            if (LvlLbl.Text == "3")
                NLvl = "Lvl3";
            if (LvlLbl.Text == "4")
                NLvl = "Lvl4";
            if (LvlLbl.Text == "5")
                NLvl = "Lvl5";
            if (LvlLbl.Text == "6")
                NLvl = "Lvl6";
            if (LvlLbl.Text == "7")
                NLvl = "Lvl7";
            if (LvlLbl.Text == "8")
                NLvl = "Lvl";

            string NMine = "";

            if (MineLbl.Text == "GN")
                NMine = "mw_GreatNoligwaMine";
            if (MineLbl.Text == "MK")
                NMine = "mw_MoabKhotsonMine";
            if (MineLbl.Text == "KP")
                NMine = "mw_KopanangMine";
            if (MineLbl.Text == "MP")
                NMine = "mw_MponengMine";
            if (MineLbl.Text == "TT")
                NMine = "mw_TauTonaMine";
            if (MineLbl.Text == "SV")
                NMine = "mw_SavukaMine";

           


            if (Authlabel.Text == "Y")
            {

                MWDataManager.clsDataAccess _dbManUpdateStuff = new MWDataManager.clsDataAccess();
                _dbManUpdateStuff.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

                if (Notetype != "RB")
                _dbManUpdateStuff.SqlStatement = " update " + NMine + ".dbo.TempStopNote_Autorisation set status = 'Declined', " + NLvl + "date = getdate(), " + NLvl + "remks = '" + RemBox.Text + "', " + NLvl + "OK = 'Dec',  " + NLvl + "User = '" + UserLbl.Text + "' where tsnid =  '" + Convert.ToString(Convert.ToInt32(FPNoLbl.Text.Substring(4, 5))) + "' ";

                if (Notetype == "RB")
                    _dbManUpdateStuff.SqlStatement = " update " + NMine + ".dbo.tbl_ReefBoringLetters_Autorisation set status = 'Declined', " + NLvl + "date = getdate(), " + NLvl + "remks = '" + RemBox.Text + "', " + NLvl + "OK = 'Dec',  " + NLvl + "User = '" + UserLbl.Text + "' where tsnid =  '" + Convert.ToString(Convert.ToInt32(FPNoLbl.Text.Substring(4, 5))) + "' ";

                _dbManUpdateStuff.SqlStatement = _dbManUpdateStuff.SqlStatement + " exec " + NMine + ".dbo.sp_SurveyLetterEmailData ";

                _dbManUpdateStuff.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManUpdateStuff.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManUpdateStuff.ExecuteInstruction();

                // RemBox.Text = _dbManUpdateStuff.SqlStatement.ToString();


                MessageFrm MsgFrm = new MessageFrm();
                MsgFrm.Text = "Record Update";
                Procedures.MsgText = "Survey Note updated";
                MsgFrm.Show();


                RemBox.Text = "";
                FinalUserLbl.Text = "";
                CompName.Text = "";
                FinalUseraaLbl.Text = "";
                Authlabel.Text = "N";
           

            }


            LoadListBox();
        }

        private void FinalUseraaLbl_TextChanged(object sender, EventArgs e)
        {
            //if (FinalUseraaLbl.Text != "")
            //{

            //    if (Answer == "Accept SN")
            //    {
            //        AcceptBtn1_Click(null, null);
            //        //MessageBox.Show("Accept");
            //    }

            //    if (Answer == "Decline SN")
            //    {
            //        DeclineBtn1_Click(null, null);
            //    }
            //}
        }

        private void CompName_TextChanged(object sender, EventArgs e)
        {
            if (CompName.Text != "")
            {

                if (Answer == "Accept SN")
                {
                    AcceptBtn1_Click(null, null);
                    //CompName.Text = "";
                    //MessageBox.Show("Accept");
                }

                if (Answer == "Decline SN")
                {
                    DeclineBtn1_Click(null, null);
                   // CompName.Text = "";
                }
            }
        }


        
    }
}
