
using DevExpress.XtraEditors;
using FastReport;
using System;
using System.Configuration;
using System.Data;

using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Mineware.Systems.DocumentManager
{
    public partial class FPMessage : XtraForm
    {
        Report theReport = new Report();
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        string Answer = string.Empty;
        string _RepDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir;
        public string _theSystemDBTag;
        public string _UserCurrentInfo;


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

        public FPMessage()
        {
            InitializeComponent();
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Commented out for later
            //if (FingerprintScanner == "N")
            //    return;

            if (FingerprintScanner == "N")
            {
                button1_Click(null, null);
                return;
            }



            TextLbl.Text = "Place Finger on Scanner to Authorise";
            Answer = "Accept SN";



            //set user lable
            //Uncommented for later
            //UserLbl.Text = "MINEWARE";

            g_IsRegister = false;

        }

        private void FPMessage_Load(object sender, EventArgs e)
        {
            //if (FPTypeLbl.Text == "Survey Note")
            //{
            //    LoadCoverNote();
            //}
            ////LoadHolingNotificationNote();
            //return;


            //try
            //{
            //    g_FormHandle = this.Handle;

            //    //ConnectScanner();

            //    //Load Fingerprints
            //    MWDataManager.clsDataAccess _dbManFP = new MWDataManager.clsDataAccess();
            //    _dbManFP.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            //    _dbManFP.SqlStatement = " select * from tbl_Users_Attachments where fprintid is not null";
            //    _dbManFP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManFP.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManFP.ExecuteInstruction();
            //    DataTable dtFP = _dbManFP.ResultsDataTable;

            //    int MyID;
            //    int size;

            //    foreach (DataRow dr in dtFP.Rows)
            //    {
            //        MyID = Convert.ToInt32(dr["FUserID"].ToString());

            //        byte[] NewArray = new byte[2048];

            //        txtFPrintID = dr["FPrintID"].ToString();
            //        string qq = dr["FPrintID"].ToString();
            //        size = Convert.ToInt32(dr["FPrintSize"].ToString());

            //        for (int i = 0; i < 2047; i++)
            //        {
            //            //Find the comma         
            //            if (txtFPrintID.Length > 0)
            //            {
            //                NewArray[i] = Convert.ToByte(qq.Substring(0, qq.IndexOf(',')));
            //                qq = qq.Substring(qq.IndexOf(',') + 1, qq.Length - (qq.IndexOf(',') + 1));
            //            }
            //        }

            //        //fpslib.BIOKEY_DB_ADD(g_biokeyHandle, MyID, size, NewArray);

            //    }
            //    FingerprintScanner = "Y";
            //}
            //catch
            //{
            //MessageBox.Show("No Fingerprint Device Detected");
            FingerprintScanner = "N";
            //}



            if (FPTypeLbl.Text == "Survey Note")
            {
                if (lblNotetype.Text == "DN")
                {
                    Loaddev();
                }

                if (lblNotetype.Text == "SRN")
                {
                    LoadStartNote();
                }

                if (lblNotetype.Text == "OCN")
                {
                    LoadOpenCastSurveyNote();
                }

                if (lblNotetype.Text == "STN")
                {
                    LoadStopingNote();
                }

                if (lblNotetype.Text == "SSN")
                {
                    LoadStoppingNote();
                }

                if (lblNotetype.Text == "SPR")
                {
                    LoadStopPanelReportNote();
                }

                if (lblNotetype.Text == "HNN")
                {
                    LoadHolingNotificationNote();
                }

                if (lblNotetype.Text == "CVN")
                {
                    LoadCoverNote();
                }



                /////commented out for later 
                //return;

                // LoadSurveyNote();
                LoadUserLvl();
            }
        }

        private void Loaddev()
        {
            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyStop.SqlStatement = "Select * from ( \r\n" +

"Select a.TSNID ID, '" + lblDate.Text + "' Date, b.SectionID Section, b.SecManager SectionMan, OldPeg PegNoFrom1,  ToNewPeg PegTo1, NewPegValue PegDist,  \r\n" +
" NewPeg PegFrom2, ToFLP Paneltype, FLPValue PegFace, SurveyLineToLHS CarryLHSTxt,  \r\n" +
" GradeLineToHW CarryHWTxt, RailElevationOldPeg SGradesPeg1, RailElevationOldPegValue1 SGradespeg1Add, RailElevationNewPeg SGradesPeg2, RailElevationNewPegValue1 PegToReqRail, EndInCoverTillPeg EndInCoverTillPegCmb, EndInCoverTillPegValue EndInCoverTillPegTxt, \r\n" +
" HolingDistPeg HolDistPegCmb, HolingDistPegValue HolDistPegTxt, StopingDistPeg StopDistPegCmb, StopingDistPegValue StopDistPegTxt, SurveyLineRHS CarryRHS, GradeLineFW CarryFW, RailElevationOldPegValue2 CarryRail,  \r\n" +
" RailElevationNewPegValue2 Rail, GradeAt Grade, BackChain BackChain, BackChainValue BackChainValue, FrontChain FrontChain, FrontChainValue FrontChainValue,  \r\n" +
" Bgp Bgp, BgpValue BGP2, Fgp Fgp, FgpValue FGP2, Scale Scale, ' ' banner \r\n" +
" ,b.ImageDate ,b.UserName  \r\n" +
" from PAS_TGT_Syncromine_New.dbo.tbl_SurveyNote_DevNote a,  [tbl_SurveyNote] b \r\n" +
" where  a.TSNID = b.TSNID and a.TSNID = '" + FPNoLbl.Text + "'  \r\n" +
" )a  \r\n" +
" left outer join  \r\n" +
" (Select* from vw_SurverLetters_Signature )b on a.ID = b.tsnid and a.UserName = b.username ";
            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();

            DataSet dsEmpty1 = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);


            DataTable Neil = _dbManEmptyStop.ResultsDataTable;

            string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby1.SqlStatement = "Select '" + _RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty);

            theReport.Load("DevelopmentNote.frx");

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        private void LoadStartNote()
        {
            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyStop.SqlStatement = "Select * from ( \r\n" +

"Select a.TSNID ID, '" + lblDate.Text + "' Date, b.SectionID Section, b.SecManager SectionMan, \r\n" +
"  NameAndDescriptionOfFace NameOfFace, NameAndDescriptionOfFacePeg NameOfFacePeg, NameAndDescriptionOfFacePegValue NameOfFace2,  \r\n" +
" HolingNoteOf holingNote, ReceivedBackBySurvey Survey, HoleSafelyInto SafelyInto, \r\n" +
" Scale Scale, HolingNoteRefNo HolingNoteRefNo,  ' ' banner \r\n" +

" ,b.ImageDate ,b.UserName  \r\n" +
" from PAS_TGT_Syncromine_New.dbo.tbl_SurveyNote_StartNote a,  [tbl_SurveyNote] b  \r\n" +

"where  a.TSNID = b.TSNID and a.TSNID = '" + FPNoLbl.Text + "'  \r\n" +
" )a  \r\n" +
" left outer join  \r\n" +
" (Select* from vw_SurverLetters_Signature )b on a.ID = b.tsnid and a.UserName = b.username ";


            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();



            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);

            DataTable Neil = _dbManEmptyStop.ResultsDataTable;

            string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby1.SqlStatement = "Select '" + _RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty);

            theReport.Load("StartNote.frx");

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        private void LoadOpenCastSurveyNote()
        {
            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyStop.SqlStatement = "Select * from (  \r\n" +

"Select a.TSNID ID, '" + lblDate.Text + "' Date, b.SectionID Section, b.SecManager SectionMan,   \r\n" +
"  Scale Scale,   \r\n" +
"' ' banner   \r\n" +
" ,b.ImageDate ,b.UserName  \r\n" +

" from PAS_TGT_Syncromine_New.dbo.tbl_SurveyNote_OpenCastSurveyNote a,  [tbl_SurveyNote] b \r\n" +

"where  a.TSNID = b.TSNID and a.TSNID = '" + FPNoLbl.Text + "'   \r\n" +
" )a   \r\n" +
" left outer join  \r\n" +
" (Select* from vw_SurverLetters_Signature )b on a.ID = b.tsnid and a.UserName = b.username ";

            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();



            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);

            DataTable Neil = _dbManEmptyStop.ResultsDataTable;

            string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby1.SqlStatement = "Select '" + _RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty);

            theReport.Load("OpencastSurveyNote.frx");


            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void LoadStopingNote()
        {

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyStop.SqlStatement = "Select * from ( \r\n" +

"Select a.TSNID ID, '" + lblDate.Text + "' Date, b.SectionID Section, b.SecManager SectionMan,  \r\n" +
"  SNOldPeg SNTopOldPeg, SNToNewPeg SNTopToNewPeg, SNNewPegValue SNTopValue,   \r\n" +
" SNNewPeg SNTopNewPeg, FLP SNTopFLP, SNFLPValue SNTopFLPValue,   \r\n" +
" AsgOldPeg SNAsgOldPeg, AsgToNewPeg SNAsgToNewPeg, AsgValue SNAsgValue,   \r\n" +
" AsgNewPeg SNAsgNewPeg, AsgFLP SNAsgFLP, AsgFLPValue SNAsgFLPValue,  \r\n" +
" Scale SNScale, ' ' banner   \r\n" +
" ,b.ImageDate ,b.UserName  \r\n" +

" from PAS_TGT_Syncromine_New.dbo.tbl_SurveyNote_StopingNote a,  [tbl_SurveyNote] b  \r\n" +
"where  a.TSNID = b.TSNID and a.TSNID = '" + FPNoLbl.Text + "'   \r\n" +
" )a  \r\n" +
" left outer join  \r\n" +
" (Select* from vw_SurverLetters_Signature )b on a.ID = b.tsnid and a.UserName = b.username ";

            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyNote";
            _dbManEmptyStop.ExecuteInstruction();



            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);

            DataTable Neil = _dbManEmptyStop.ResultsDataTable;

            string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby1.SqlStatement = "Select '" + _RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty);

            theReport.Load("StopingNote.frx");
            //theReport.Load("surveyNoteDevA3.frx");

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        private void LoadStoppingNote()
        {

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyStop.SqlStatement = "Select * from ( \r\n" +

"Select a.TSNID ID, '" + lblDate.Text + "' Date, b.SectionID Section, b.SecManager SectionMan,  \r\n" +
"  StopTheFollowingface1 StopFace, StopTheFollowingfacePeg1 PegNum, StopTheFollowingfaceValue1 PegNumValue,  \r\n" +
" StopTheFollowingface2 StopFace2, StopTheFollowingfacePeg2 PegNum2, StopTheFollowingfaceValue2 PegNumValue2,  \r\n" +
" Scale StoppingScale, ' ' banner  \r\n" +

" ,b.ImageDate ,b.UserName  \r\n" +

" from PAS_TGT_Syncromine_New.dbo.tbl_SurveyNote_StoppingNote a,  [tbl_SurveyNote] b  \r\n" +
"where  a.TSNID = b.TSNID and a.TSNID = '" + FPNoLbl.Text + "'  \r\n" +
" )a   \r\n" +
" left outer join  \r\n" +
" (Select* from vw_SurverLetters_Signature )b on a.ID = b.tsnid and a.UserName = b.username ";

            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyNote";
            _dbManEmptyStop.ExecuteInstruction();



            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);

            DataTable Neil = _dbManEmptyStop.ResultsDataTable;

            string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby1.SqlStatement = "Select '" + _RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty);

            theReport.Load("StoppingNote.frx");

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        private void LoadStopPanelReportNote()
        {

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyStop.SqlStatement = "Select * from (  \r\n" +

"Select a.TSNID ID, '" + lblDate.Text + "' Date, b.SectionID Section, b.SecManager SectionMan,  \r\n" +
"  ReasonPnlStopped ReasonPnlStopped, SupFromFacePerm SupFromFacePerm, TypeOfSupport TypeSup1,   \r\n" +
" SupFromFaceTemp SupFromFaceTemp, TypeOfSupport2 TypeSup2, TopPeg TopPeg, \r\n" +
" DistToFace DistFaceTop, BotPeg BotPeg, DistFace2 DistFaceBot,   \r\n" +
" Face Face, FaceMetres FaceM, GullyTons Gully,   \r\n" +
" GullyMetres GullyM, Rse Rse, RseMetres RseM,   \r\n" +
" ReSweepsTons ReSweeps, ReSweepsMetres ReSweepsM, CurrentOreReserveCategory CurOreResCat,  \r\n" +
" OreBlockSize OreBlockSize, NewOreReserveCategory NewOreResCat, RemainingOreBlockvalues OreBlockvalues,   \r\n" +
" MissfiresChecked MissfiresChecked, BarricadedChecked BarricadedChecked, ReclaimedChecked ReclaimedChecked,  \r\n" +
" PnlSweptChecked PnlSweptChecked, StrikeGullyChecked StrikeGullyChecked, StopeChecked StopeChecked,   \r\n" +
" CleanedChecked CleanedChecked, AllStuffChecked AllStuffChecked, SuppAndSafeGN SuppAndSafeGN, TonsGN TonsGN,ReclamationGN ReclamationGN, ' ' banner,RockEngReportNumber RockEngRepNo  \r\n" +

"  , b.ImageDate ,b.UserName    \r\n" +
"   from PAS_TGT_Syncromine_New.dbo.tbl_SurveyNote_StopPanelReportNote a,  [tbl_SurveyNote] b   \r\n" +
"where  a.TSNID = b.TSNID and a.TSNID = '" + FPNoLbl.Text + "'   \r\n" +
" )a   \r\n" +
" left outer join   \r\n" +
" (Select* from vw_SurverLetters_Signature )b on a.ID = b.tsnid and a.UserName = b.username  ";

            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();



            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);

            DataTable Neil = _dbManEmptyStop.ResultsDataTable;

            string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby1.SqlStatement = "Select '" + _RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty);

            theReport.Load("StopPanelReportNote.frx");
            //theReport.Load("surveyNoteDevA3.frx");

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        private void LoadHolingNotificationNote()
        {

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyStop.SqlStatement = "Select * from (  \r\n" +

"Select a.TSNID ID, '" + lblDate.Text + "' Date, b.SectionID Section, b.SecManager SectionMan,   \r\n" +
"  FollowingFaceA FacesA, FaceHoleInto HoleInto, FollowingFaceB FacesB, FaceMinedCloseTo MinesCloseTo,  \r\n" +
"  DistanceBetweenTwoEnds Distance, StopFollowingFacePeg1 FacePeg, StopFollowingFacePegValue1 FollowingFace,   \r\n" +
" StopFollowingFacePeg2 FacePeg2, StopFollowingFacePegValue2 Face2, Scale Scale,   \r\n" +
" FollowingEndPnl FollowingEndPnl, MentionedEndPnl MentionedEndPnl,   \r\n" +
" StopFollowingFace1 StopTheFollowingFace, StopFollowingFace2 StopTheFollowingFace2, ' ' banner  \r\n" +

" ,b.ImageDate ,b.UserName  \r\n" +
" from PAS_TGT_Syncromine_New.dbo.tbl_SurveyNote_HolingNotificationNote a,  [tbl_SurveyNote]  b  \r\n" +
"where  a.TSNID = b.TSNID and a.TSNID = '" + FPNoLbl.Text + "'   \r\n" +
" )a  \r\n" +
" left outer join  \r\n" +
" (Select* from vw_SurverLetters_Signature )b on a.ID = b.tsnid and a.UserName = b.username  ";


            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();



            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);

            DataTable Neil = _dbManEmptyStop.ResultsDataTable;

            string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby1.SqlStatement = "Select '" + _RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty);

            theReport.Load("HolingNotification.frx");

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        private void LoadCoverNote()
        {

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyStop.SqlStatement = "Select * from (  \r\n" +

"Select a.TSNID ID, '" + lblDate.Text + "' Date, b.SectionID Section, b.SecManager SectionMan,  \r\n" +
"  AtPegNum CurrPeg, AtPegNumVal CurrPegValue, AtPegNum2 WorkFacePeg,   \r\n" +
" AtPegNumVal2 WorkFacePegValue, StopFace DrillCubbyPeg, StopFace2 DrillCubbyPegValue, Scale CoverScale,  \r\n" +
" ' ' banner  \r\n" +

" ,b.ImageDate ,b.UserName  \r\n" +
" from PAS_TGT_Syncromine_New.dbo.tbl_SurveyNote_CoverNote a,  [tbl_SurveyNote] b  \r\n" +
"where  a.TSNID = b.TSNID and a.TSNID = '" + FPNoLbl.Text + "'   \r\n" +
" )a   \r\n" +
" left outer join  \r\n" +
" (Select* from vw_SurverLetters_Signature )b on a.ID = b.tsnid and a.UserName = b.username ";


            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyNote";
            _dbManEmptyStop.ExecuteInstruction();



            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);

            DataTable Neil = _dbManEmptyStop.ResultsDataTable;

            string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby1.SqlStatement = "Select '" + _RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty);

            theReport.Load("CoverNote.frx");


            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        private void LoadUserLvl()
        {
            MWDataManager.clsDataAccess _dbManCHKBy = new MWDataManager.clsDataAccess();
            _dbManCHKBy.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManCHKBy.SqlStatement = "select " +
                                        "case when lvl1date is null then 'Checked By'  " +
                                        "when lvl2date is null then 'Snr Mine Surveyor'  " +
                                        "when lvl3date is null then 'Geologist'  " +
                                        "when lvl4date is null then 'Rock Mechanic' " +
                                        "when lvl5date is null then 'Snr Mine Planner' " +
                                        "when lvl6date is null then 'Sec Manager'  " +
                                        "when lvl7date is null then 'Surv. Plan Manager'  " +
                                        "when lvl8date is null then 'Prod Manager' " +
                                        "when lvl9date is null then 'Mine Overseer' " +
                                        " end as wwho from [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] " +
                                        "where tsnid = '" + FPNoLbl.Text + "' ";
            _dbManCHKBy.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCHKBy.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCHKBy.ResultsTableName = "EmptyCubby";
            _dbManCHKBy.ExecuteInstruction();

            DataTable NeilChBy = _dbManCHKBy.ResultsDataTable;

            Lvllbl.Text = NeilChBy.Rows[0]["wwho"].ToString();

            Lvllbl.Visible = true;


        }

        private void LoadSurveyNote()
        {


            MWDataManager.clsDataAccess _dbManEmptyCubby = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby.SqlStatement = "  select top(1) * from ( select a.*,  " +
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




                                       " , case when status = 'Declined' then 'D' when (select banner from tbl_sysset) <> 'Moab Khotsong' and us9.username is null then 'N' " +
                                       " when (select banner from tbl_sysset) = 'Moab Khotsong' and us7.username is null then 'N' " +
                                       "  " +

                                       " else 'Y' end as auth " +




                                       "  from (  \r\n " +





                                       "  select  TSNID ID, NoteType NoteType, w.WorkplaceID +':'+ description WP, MOSection +':'+ s1.Name MOSection,  TheDate Date,'' StopRemark,   \r\n " +
                                       "  '' Remark, '' PegFrom,  '0' a, '' Peg, '' Length, '' Depth, '' Height, ImageDate,    \r\n " +
                                       " SecManager +':'+ s2.Name ManagerSection, '' PegTo, '' PegDist,  '' PegFace, '' LHS, '' RHS, '' HW, '' FW,  \r\n " +
                                       "  '' CarryRail, '' ChainAtPeg,  '' ChainAtPegVal, '' Chain, '' LP, '' PegToRail,  \r\n " +
                                       " '' PegToRailVal,  '' Rail, '' SGrade1, '' SGrade2, '' SGrade3,  \r\n " +
                                       " '' SGrade4, '' StopDistPeg,  '' StopDistPegVal, '' PegVal, '' PegToFace,  \r\n " +
                                       "  '' AdvToDate, '' HolDistPeg,  '' HoldingDistPegVal, '' Grade, '' PanelType,'Amandelbult Mine' banner,  \r\n " +
                                       "  HolingNoteNo HolingNoteNo, Notes Notes, w.Description, s.Sectionid SID,s.Name SName,s1.Sectionid S1ID,s1.Name S1Name,s2.Sectionid S2ID,s2.Name S2Name, username    \r\n " +
                                       " from [Mineware_Reporting].[dbo].tbl_SurveyNote t, tbl_Workplace w , tbl_Section s, tbl_Section s1, tbl_Section s2   \r\n " +
                                       "  where TSNID = '" + FPNoLbl.Text + "' and t.Workplaceid = w.gmsiwpid  \r\n " +
                                       "  and t.Sectionid = s.Sectionid and s.prodmonth = (select CurrentProductionMonth from tbl_SysSet) and s.ReportToSectionid = s1.Sectionid and s1.prodmonth =   \r\n " +
                                       " (select CurrentProductionMonth from tbl_SysSet) and s1.ReportToSectionid = s2.Sectionid and s2.prodmonth = (select CurrentProductionMonth from tbl_SysSet)   \r\n " +


                                       " ) a \r\n " +

                                       " left outer join [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] b on a.id = b.TSNID \r\n " +



                                         "left outer join [dbo].tbl_Users_Department_Survey me on b.lvl1user = me.complogin \r\n " +
                                         "left outer join [dbo].[tbl_Users] us1 on me.userid = us1.userid \r\n " +

                                         "left outer join [dbo].tbl_Users_Department_Survey me1 on b.lvl2user = me1.complogin    \r\n " +
                                         "left outer join [dbo].[tbl_Users] us2 on me1.userid = us2.userid \r\n " +

                                         "left outer join [dbo].tbl_Users_Department_Survey me2 on b.lvl3user = me2.complogin   \r\n " +
                                         "left outer join [dbo].[tbl_Users] us3 on me2.userid = us3.userid \r\n " +

                                         "left outer join [dbo].tbl_Users_Department_Survey me3 on b.lvl4user = me3.complogin   \r\n " +
                                         "left outer join [dbo].[tbl_Users] us4 on me3.userid = us4.userid \r\n " +

                                         "left outer join [dbo].tbl_Users_Department_Survey me4 on b.lvl5user = me4.complogin   \r\n " +
                                         "left outer join [dbo].[tbl_Users] us5 on me4.userid = us5.userid \r\n " +

                                         "left outer join [dbo].tbl_Users_Department_Survey me5 on b.lvl6user = me5.complogin  \r\n " +
                                         "left outer join [dbo].[tbl_Users] us6 on me5.userid = us6.userid \r\n " +

                                         "left outer join [dbo].tbl_Users_Department_Survey me6 on b.lvl7user = me6.complogin   \r\n " +
                                         "left outer join [dbo].[tbl_Users] us7 on me6.userid = us7.userid   \r\n " +

                                         "left outer join [dbo].tbl_Users_Department_Survey me7 on b.lvl8user = me7.complogin   \r\n " +
                                         "left outer join [dbo].[tbl_Users] us8 on me7.userid = us8.userid \r\n " +

                                         " left outer join [dbo].tbl_Users_Department_Survey me8 on b.lvl9user = me8.complogin  \r\n " +
                                         "left outer join [dbo].[tbl_Users] us9 on me8.userid = us9.userid \r\n " +

                                         "left outer join [dbo].tbl_Users_Department_Survey me9 on b.lvl9user = me9.complogin  \r\n " +
                                         "left outer join [dbo].[tbl_Users] s10 on me9.userid = s10.userid \r\n " +



                                       " left outer join [dbo].[tbl_Users_Attachments] s1 on us1.userid = s1.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s2 on us2.userid = s2.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s3 on us3.userid = s3.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s4 on us4.userid = s4.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s5 on us5.userid = s5.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s6 on us6.userid = s6.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s7 on us7.userid = s7.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s8 on us8.userid = s8.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s9 on us9.userid = s9.userid  \r\n " +

                                       " left outer join [dbo].[tbl_Users] us11 on a.username = us11.username \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s11 on us11.userid = s11.userid \r\n " +



                                        "   ) a   ";




            _dbManEmptyCubby.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby.ResultsTableName = "EmptyCubby";
            _dbManEmptyCubby.ExecuteInstruction();


            DataTable Neil = _dbManEmptyCubby.ResultsDataTable;

            string ImageDateLbl = Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            _dbManEmptyCubby1.SqlStatement = "Select '" + _RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


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

            if (true)
            {
                theReport.Load("surveyNoteDevA3.frx");
            }


            //theReport.Design();



            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();




        }

        String Lvl = string.Empty;

        private void button1_Click(object sender, EventArgs e)
        {

            if (FPTypeLbl.Text == "Survey Note")
            {
                // check if correct lvl
                MWDataManager.clsDataAccess _dbManaa = new MWDataManager.clsDataAccess();
                _dbManaa.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                if (Lvllbl.Text == "Checked By")
                    _dbManaa.SqlStatement = "select * from [tbl_Users_Synchromine] where UserID = '"+clsUserInfo.UserID+"' ";
                if (Lvllbl.Text == "Snr Mine Surveyor")
                    _dbManaa.SqlStatement = "select Survey_SnrMineSurveyor from [tbl_Users_Synchromine] where UserID = '" + clsUserInfo.UserID + "' and  Survey_SnrMineSurveyor = '1' ";
                if (Lvllbl.Text == "Geologist")
                    _dbManaa.SqlStatement = "select Survey_Geologist from [tbl_Users_Synchromine] where UserID = '" + clsUserInfo.UserID + "' and  Survey_Geologist = '1' ";
                if (Lvllbl.Text == "Rock Mechanic")
                    _dbManaa.SqlStatement = "select Survey_RockMech from [tbl_Users_Synchromine] where UserID = '" + clsUserInfo.UserID + "' and  Survey_RockMech = '1' ";
                if (Lvllbl.Text == "Snr Mine Planner")
                    _dbManaa.SqlStatement = "select Survey_SnrMinePlanner from [tbl_Users_Synchromine] where UserID = '" + clsUserInfo.UserID + "' and  Survey_SnrMinePlanner = '1' ";
                if (Lvllbl.Text == "Sec Manager")
                    _dbManaa.SqlStatement = "select Survey_SecMan from [tbl_Users_Synchromine] where UserID = '" + clsUserInfo.UserID + "' and  Survey_SecMan = '1' ";
                if (Lvllbl.Text == "Surv. Plan Manager")
                    _dbManaa.SqlStatement = "select Survey_SurvPlanMan from [tbl_Users_Synchromine] where UserID = '" + clsUserInfo.UserID + "' and  Survey_SurvPlanMan = '1'";
                if (Lvllbl.Text == "Prod Manager")
                    _dbManaa.SqlStatement = "select Survey_ProdMan from [tbl_Users_Synchromine] where UserID = '" + clsUserInfo.UserID + "' and  Survey_ProdMan = '1' ";
                if (Lvllbl.Text == "Mine Overseer")
                    _dbManaa.SqlStatement = "select Survey_MO from [tbl_Users_Synchromine] where UserID = '" + clsUserInfo.UserID + "' and  Survey_SnrMineSurveyor = '1' ";

                _dbManaa.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManaa.queryReturnType = MWDataManager.ReturnType.DataTable;

                _dbManaa.ExecuteInstruction();

                DataTable Neil1111 = _dbManaa.ResultsDataTable;

                if (Neil1111.Rows.Count < 1)
                {

                    MessageBox.Show("User " + UserLbl.Text + " is not set up to Authorise on this level ", "User Set Up", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                AuthSurveyNote();
            }
        }




        private void AuthSurveyNote()
        {
            // first 

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            if (Lvllbl.Text == "Checked By")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set Lvl1date = getdate(), lvl1remks = '" + RemBox.Text + "', lvl1OK = 'Yes',  lvl1User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Snr Mine Surveyor")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set Lvl2date = getdate(), lvl2remks = '" + RemBox.Text + "', lvl2OK = 'Yes',  lvl2User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Geologist")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set Lvl3date = getdate(), lvl3remks = '" + RemBox.Text + "', lvl3OK = 'Yes',  lvl3User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Rock Mechanic")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set Lvl4date = getdate(), lvl4remks = '" + RemBox.Text + "', lvl4OK = 'Yes',  lvl4User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Snr Mine Planner")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set Lvl5date = getdate(), lvl5remks = '" + RemBox.Text + "', lvl5OK = 'Yes',  lvl5User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Sec Manager")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set Lvl6date = getdate(), lvl6remks = '" + RemBox.Text + "', lvl6OK = 'Yes',  lvl6User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Surv. Plan Manager")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set Lvl7date = getdate(), lvl7remks = '" + RemBox.Text + "', lvl7OK = 'Yes',  lvl7User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Prod Manager")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set Lvl8date = getdate(), lvl8remks = '" + RemBox.Text + "', lvl8OK = 'Yes',  lvl8User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Mine Overseer")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set Lvl9date = getdate(), lvl9remks = '" + RemBox.Text + "', lvl9OK = 'Yes',  lvl9User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
            LoadUserLvl();


        }

        private void DeclineBtn1_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            if (Lvllbl.Text == "Checked By")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set status = 'Declined', Lvl1date = getdate(), lvl1remks = '" + RemBox.Text + "', lvl1OK = 'Dec',  lvl1User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Snr Mine Surveyor")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set status = 'Declined', Lvl2date = getdate(), lvl2remks = '" + RemBox.Text + "', lvl2OK = 'Dec',  lvl2User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Geologist")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set status = 'Declined', Lvl3date = getdate(), lvl3remks = '" + RemBox.Text + "', lvl3OK = 'Dec',  lvl3User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Rock Mechanic")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set status = 'Declined', Lvl4date = getdate(), lvl4remks = '" + RemBox.Text + "', lvl4OK = 'Dec',  lvl4User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Snr Mine Planner")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set status = 'Declined',  Lvl5date = getdate(), lvl5remks = '" + RemBox.Text + "', lvl5OK = 'Dec',  lvl5User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Sec Manager")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set status = 'Declined', Lvl6date = getdate(), lvl6remks = '" + RemBox.Text + "', lvl6OK = 'Dec',  lvl6User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Surv. Plan Manager")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set status = 'Declined', Lvl7date = getdate(), lvl7remks = '" + RemBox.Text + "', lvl7OK = 'Dec',  lvl7User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";
            if (Lvllbl.Text == "Prod Manager")
                _dbMan.SqlStatement = " update [Mineware_Reporting].[dbo].[tbl_SurveyNote_Autorisation] set status = 'Declined', Lvl8date = getdate(), lvl8remks = '" + RemBox.Text + "', lvl8OK = 'Dec',  lvl8User = '" + UserLbl.Text + "' where tsnid =  '" + FPNoLbl.Text + "' ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();


            LoadUserLvl();


            Close();
        }

        private void DeclineBTN_Click(object sender, EventArgs e)
        {
            //Commented out for later
            if (FingerprintScanner == "N")
            {
                DeclineBtn1_Click(null, null);
                return;
            }

            TextLbl.Text = "Place Finger on Scanner to Authorise";

            Answer = "Decline SN";
            UserLbl.Text = "MINEWARE";
        }


        //public void ConnectScanner()
        //{
        //    if (!gConnected)
        //    {
        //        int ret = 0;
        //        byte[] paramValue = new byte[64];

        //        // Enable log
        //        Array.Clear(paramValue, 0, paramValue.Length);
        //        paramValue[0] = 1;

        //        zkfpri.sensorSetParameterEx(g_Handle, 1100, paramValue, 4);

        //        ret = zkfpri.sensorInit();
        //        if (ret != 0)
        //        {
        //            MessageBox.Show("Init Failed, ret=" + ret.ToString());
        //            return;
        //        }
        //        g_Handle = zkfpri.sensorOpen(0);

        //        Array.Clear(paramValue, 0, paramValue.Length);
        //        zkfpri.sensorGetVersion(paramValue, paramValue.Length);

        //        ret = paramValue.Length;
        //        Array.Clear(paramValue, 0, paramValue.Length);
        //        zkfpri.sensorGetParameterEx(g_Handle, 1, paramValue, ref ret);
        //        g_nWidth = BitConverter.ToInt32(paramValue, 0);

        //        //this.picFP.Width = g_nWidth;



        //        ret = paramValue.Length;
        //        Array.Clear(paramValue, 0, paramValue.Length);
        //        zkfpri.sensorGetParameterEx(g_Handle, 2, paramValue, ref ret);
        //        g_nHeight = BitConverter.ToInt32(paramValue, 0);

        //        //this.picFP.Height = g_nHeight;

        //        ret = paramValue.Length;
        //        Array.Clear(paramValue, 0, paramValue.Length);
        //        zkfpri.sensorGetParameterEx(g_Handle, 106, paramValue, ref ret);
        //        g_FPBufferSize = BitConverter.ToInt32(paramValue, 0);



        //        g_FPBuffer = new byte[g_FPBufferSize];
        //        Array.Clear(g_FPBuffer, 0, g_FPBuffer.Length);




        //        // get vid&pid
        //        ret = paramValue.Length;
        //        Array.Clear(paramValue, 0, paramValue.Length);
        //        zkfpri.sensorGetParameterEx(g_Handle, 1015, paramValue, ref ret);
        //        int nVid = BitConverter.ToInt16(paramValue, 0);
        //        int nPid = BitConverter.ToInt16(paramValue, 2);

        //        // Manufacturer
        //        ret = paramValue.Length;
        //        Array.Clear(paramValue, 0, paramValue.Length);
        //        zkfpri.sensorGetParameterEx(g_Handle, 1101, paramValue, ref ret);
        //        string manufacturer = System.Text.Encoding.ASCII.GetString(paramValue);
        //        // Product
        //        ret = paramValue.Length;
        //        Array.Clear(paramValue, 0, paramValue.Length);
        //        zkfpri.sensorGetParameterEx(g_Handle, 1102, paramValue, ref ret);
        //        string product = System.Text.Encoding.ASCII.GetString(paramValue);
        //        // SerialNumber
        //        ret = paramValue.Length;
        //        Array.Clear(paramValue, 0, paramValue.Length);
        //        zkfpri.sensorGetParameterEx(g_Handle, 1103, paramValue, ref ret);
        //        string serialNumber = System.Text.Encoding.ASCII.GetString(paramValue);


        //        // char paramValue[64] = {0};
        //        // int ret = sizeof(paramValue);

        //        // Get Vendor Info
        //        ret = paramValue.Length;
        //        zkfpri.sensorGetParameterEx(g_Handle, 1104, paramValue, ref ret); // Read customized data, the check.






        //        // Fingerprint Alg
        //        short[] iSize = new short[24];
        //        iSize[0] = (short)g_nWidth;
        //        iSize[1] = (short)g_nHeight;
        //        iSize[20] = (short)g_nWidth;
        //        iSize[21] = (short)g_nHeight; ;
        //        g_biokeyHandle = fpslib.BIOKEY_INIT(0, iSize, null, null, 0);
        //        if (g_biokeyHandle == IntPtr.Zero)
        //        {
        //            MessageBox.Show("BIOKEY_INIT failed");
        //            return;
        //        }

        //        // Set allow 360 angle of Press Finger
        //        fpslib.BIOKEY_SET_PARAMETER(g_biokeyHandle, 4, 180);

        //        // Set Matching threshold
        //        fpslib.BIOKEY_MATCHINGPARAM(g_biokeyHandle, 0, fpslib.THRESHOLD_MIDDLE);

        //        // Init RegTmps
        //        for (int i = 0; i < 3; i++)
        //        {
        //            g_RegTmps[i] = new byte[2048];
        //        }

        //        Thread captureThread = new Thread(new ThreadStart(DoCapture));
        //        captureThread.IsBackground = true;
        //        captureThread.Start();
        //        g_bIsTimeToDie = false;

        //        gConnected = true;
        //        //btnRegister.Enabled = true;
        //        //btnVerify.Enabled = true;
        //        //btnConnect.Text = "Disconnect Sensor";

        //        //txtPrompt.Text = "Please put your finger on the sensor";


        //    }
        //    else
        //    {
        //        FreeSensor();

        //        fpslib.BIOKEY_DB_CLEAR(g_biokeyHandle);
        //        fpslib.BIOKEY_CLOSE(g_biokeyHandle);

        //        gConnected = false;
        //        //btnRegister.Enabled = false;
        //        //btnVerify.Enabled = false;
        //        //btnConnect.Text = "Connect Sensor";
        //    }
        //}

        //Fingerprint Method
        //private void DoCapture()
        //{
        //    while (!g_bIsTimeToDie)
        //    {
        //        int ret = zkfpri.sensorCapture(g_Handle, g_FPBuffer, g_FPBufferSize);

        //        if (ret > 0)
        //        {
        //            SendMessage(g_FormHandle, MESSAGE_FP_RECEIVED, IntPtr.Zero, IntPtr.Zero);
        //        }
        //        Thread.Sleep(30);


        //    }
        //}

        //Fingerprint Method
        //private void FreeSensor()
        //{
        //    g_bIsTimeToDie = true;
        //    Thread.Sleep(100);
        //    zkfpri.sensorClose(g_Handle);

        //    // Disable log
        //    byte[] paramValue = new byte[4];
        //    paramValue[0] = 0;
        //    zkfpri.sensorSetParameterEx(g_Handle, 1100, paramValue, 4);

        //    zkfpri.sensorFree();
        //}

        //Fingerprint Method
        //protected override void DefWndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case MESSAGE_FP_RECEIVED:
        //            {
        //                try
        //                {
        //                    int ret = 0;
        //                    int id = 0;
        //                    int score = 0;
        //                    int quality = 0;


        //                    if (g_IsRegister)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        byte[] NewArray = new byte[2048];

        //                        string qq = txtFPrintID;
        //                        for (int i = 0; i < 2047; i++)
        //                        {
        //                            //Find the comma         
        //                            if (txtFPrintID.Length > 0)
        //                            {
        //                                NewArray[i] = Convert.ToByte(qq.Substring(0, qq.IndexOf(',')));
        //                                qq = qq.Substring(qq.IndexOf(',') + 1, qq.Length - (qq.IndexOf(',') + 1));
        //                            }
        //                        }

        //                        Array.Clear(g_VerTmp, 0, g_VerTmp.Length);
        //                        if ((ret = fpslib.BIOKEY_EXTRACT(g_biokeyHandle, g_FPBuffer, g_VerTmp, 0)) > 0)
        //                        {
        //                            // Get fingerprint quality
        //                            quality = fpslib.BIOKEY_GETLASTQUALITY();
        //                            //txtQuality.Text = quality.ToString();

        //                            ret = fpslib.BIOKEY_IDENTIFYTEMP(g_biokeyHandle, g_VerTmp, ref id, ref score);


        //                            if (ret > 0)
        //                            {
        //                                //TextLbl.Text = string.Format("Identification success, id={0}, score={1}", id, score);
        //                                TextLbl.Text = "Identification success"; //'" + lblUerID.Text + "'";
        //                                lblUerID.Text = id.ToString();

        //                                MWDataManager.clsDataAccess _dbManzz = new MWDataManager.clsDataAccess();
        //                                _dbManzz.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
        //                                _dbManzz.SqlStatement = " select c.complogin,* from tbl_Users_Attachments a, tbl_Users b, tbl_Users_Department_Survey c where a.userid = b.userid and a.userid = c.userid and FUserID = '" + lblUerID.Text + "' ";
        //                                _dbManzz.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //                                _dbManzz.queryReturnType = MWDataManager.ReturnType.DataTable;
        //                                _dbManzz.ExecuteInstruction();
        //                                //MessageBox.Show(_dbMan.ResultsDataTable.Rows[0][0].ToString());

        //                                UserLbl.Text = _dbManzz.ResultsDataTable.Rows[0][0].ToString();

        //                                // do if qualify


        //                            }
        //                            else
        //                            {
        //                                //TextLbl.Text = string.Format("Identification failed, score={0}", score);
        //                                TextLbl.Text = "Identification failed";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            TextLbl.Text = "Extract template failed";
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message.ToString());
        //                }
        //            }
        //            break;

        //        default:
        //            base.DefWndProc(ref m);
        //            break;
        //    }
        //}

        private void TextLbl_TextChanged(object sender, EventArgs e)
        {

        }

        private void UserLbl_TextChanged(object sender, EventArgs e)
        {
            if (Answer == "Accept SN")
            {
                button1_Click(null, null);
            }

            if (Answer == "Decline SN")
            {
                DeclineBtn1_Click(null, null);
            }
        }
    }
}
