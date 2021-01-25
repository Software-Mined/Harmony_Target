using DevExpress.XtraEditors;
using FastReport;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Survey
{
    public partial class CubbyStopNotes : XtraForm
    {
        public CubbyStopNotes()
        {
            InitializeComponent();
        }
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        DataTable dtSections = new DataTable();
        DataTable dtWP = new DataTable();
        Report theReport = new Report();

        public ucSurveyNote Service;
        public string _theSystemDBTag;
        public string _UserCurrentInfo;

        public bool IsChanged;
        public string Peg = "N";
        string ChartType;
        string StopRemark;
        public string ImageType;
        public string noteType;
        decimal TotPegValue = 0;

        BindingSource bs1 = new BindingSource();


        void LoadWorkplaces()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbMan.SqlStatement = "Select Distinct(a.WorkplaceID) as WorkplaceID, b.Description from " +
                                   "Planmonth a, Workplace b where " +
                                   "a.WorkplaceID = b.WorkplaceID and " +
                                   "a.activity = 1 and a.ProdMonth >= '" + ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString() + "' ";

            if (SectionCmb.SelectedIndex > 0)
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + " and a.sectionid like '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text) + "%' ";
            }
            _dbMan.SqlStatement = _dbMan.SqlStatement + "Order by a.WorkplaceID ";


            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtWP = new DataTable();
            dtWP = _dbMan.ResultsDataTable;

            if (noteType == "CN")
            {

            }

            if (noteType == "SN")
            {
                AdvWorkplaceCmb.Items.Clear();
                ExWpcmb.Items.Clear();

                foreach (DataRow dr in dtWP.Rows)
                {

                    AdvWorkplaceCmb.Items.Add(dr["WorkplaceID"].ToString() + ":" + dr["Description"].ToString());
                    ExWpcmb.Items.Add(dr["WorkplaceID"].ToString() + ":" + dr["Description"].ToString());

                }
                AdvWorkplaceCmb.Text = AdvWorkplaceCmb.Text;
                ExWpcmb.Text = AdvWorkplaceCmb.Text;
            }
        }

        public void LoadEmptyCubby()
        {
            CubbyRightbtn.Visible = false;
            DrillRigLeftBtn.Visible = false;
            DrillCubbyRightbtn.Visible = false;
            DrillCubbyLeft.Visible = false;
            DrillRigRightBtn.Visible = false;
            PegBtn.Visible = false;

            ChartType = "EmptyCubby";
            noteType = "CN";


            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";


                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }

                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            string wp = string.Empty;

            if (WPList.SelectedItem == null)
            {
                wp = string.Empty;
            }
            else
            {
                wp = WPList.SelectedItem.ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyCubby = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby.SqlStatement =

                _dbManEmptyCubby.SqlStatement = "  select a.*,  " +
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

                                       "  from (  \r\n " +

                " select  '" + IDtxt.Text + "' ID,'" + noteType + "' NoteType,'" + wp + "' WP,'" + SectionCmb.Text + "' MOSection, " +
                                       " '" + date.Value.ToShortDateString() + "' Date, '" + StopRemark + "' StopRemark, '" + RemarkTxt.Text + "' Remark, '" + PegNoFrom1Cmb.Text + "' PegFrom,  " +
                                       "'0' a, '" + PegDistTxt.Text + "' Peg,'" + LengthTxt.Text + "' Length,'" + DepthTxt.Text + "' Depth,'" + HeightTxt.Text + "' Height, '" + ImageDate.Text + "' ImageDate, " +
                                       " '" + SectionManCmb.Text + "' ManagerSection, '" + PegTo1cmb.Text + "' PegTo,'" + PegDistTxt.Text + "' PegDist, " +
                                       " '" + PegFaceTxt.Text + "'PegFace, '" + CarryLHSTxt.Text + "' LHS, '" + CarryRHStxt.Text + "' RHS, '" + CarryHWTxt.Text + "' HW, '" + CarryFWtxt.Text + "' FW,'" + CarryRailTxt.Text + "' CarryRail,'" + ChainAtPegCmb.Text + "' ChainAtPeg, " +
                                       " '" + ChainAtPegTxt.Text + "' ChainAtPegVal, '" + ChainTxt.Text + "' Chain, '" + LPTxt.Text + "' LP, '" + PegToReqRailCmb.Text + "' PegToRail, '" + PegToReqRailTxt.Text + "' PegToRailVal, " +
                                       " '" + RailTxt.Text + "' Rail, '" + SGradesPegTxt1.Text + "' SGrade1, '" + SGradesPegTxt1Add.Text + "'SGrade2, '" + SGradesPegTxt2.Text + "'SGrade3, '" + SGradesPegTxt2Add.Text + "'SGrade4,'" + StopDistPegCmb.Text + "' StopDistPeg, " +
                                       " '" + StopDistPegTxt.Text + "' StopDistPegVal, '" + PegValTxt.Text + "' PegVal,'" + PegToFaceTxt.Text + "' PegToFace,'" + AdvToDateTxt.Text + "' AdvToDate, '" + HolDistPegCmb.Text + "' HolDistPeg, " +
                                       " '" + HolingDistPegTxt.Text + "' HoldingDistPegVal, '" + GradeTxt.Text + "' Grade,'" + PanelTypetxt.Text + "' PanelType,'" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, '" + HolingNoteNoTxt.Text + "' HolingNoteNo, '" + NotesTxt.Text + "' Notes, '" + TUserInfo.Name + "' username  " +

            " ) a \r\n " +

                                     " left outer join  [tbl_SurveyNote_Autorisation] b on a.id = b.TSNID \r\n " +

                                     " left outer join [dbo].[tbl_Users_Attachments] s1 on b.lvl1user = s1.userid \r\n " +

                                     " left outer join [dbo].[tbl_Users_Attachments] s2 on b.lvl2user = s2.userid \r\n " +
                                     " left outer join [dbo].[tbl_Users_Attachments] s3 on b.lvl3user = s3.userid \r\n " +
                                     " left outer join [dbo].[tbl_Users_Attachments] s4 on b.lvl4user = s4.userid \r\n " +
                                     " left outer join [dbo].[tbl_Users_Attachments] s5 on b.lvl5user = s5.userid \r\n " +
                                     " left outer join [dbo].[tbl_Users_Attachments] s6 on b.lvl6user = s6.userid \r\n " +
                                     "  left outer join [dbo].[tbl_Users_Attachments] s7 on b.lvl7user = s7.userid \r\n " +
                                     " left outer join [dbo].[tbl_Users_Attachments] s8 on b.lvl8user = s8.userid \r\n " +
                                     " left outer join [dbo].[tbl_Users_Attachments] s9 on b.lvl9user = s9.userid  \r\n " +

                                     " left outer join [dbo].[tbl_Users] s10 on a.username = s10.username \r\n " +
                                     " left outer join [dbo].[tbl_Users_Attachments] s11 on s10.userid = s11.userid \r\n " +

                                        " left outer join [dbo].[tbl_Users] us1 on b.lvl1user = us1.userid \r\n " +
                                        " left outer join [dbo].[tbl_Users] us2 on b.lvl2user = us2.userid \r\n " +
                                        " left outer join [dbo].[tbl_Users] us3 on b.lvl3user = us3.userid \r\n " +
                                        " left outer join [dbo].[tbl_Users] us4 on b.lvl4user = us4.userid \r\n " +
                                        " left outer join [dbo].[tbl_Users] us5 on b.lvl5user = us5.userid \r\n " +
                                        " left outer join [dbo].[tbl_Users] us6 on b.lvl6user = us6.userid \r\n " +
                                        " left outer join [dbo].[tbl_Users] us7 on b.lvl7user = us7.userid \r\n " +
                                        " left outer join [dbo].[tbl_Users] us8 on b.lvl8user = us8.userid \r\n " +
                                        " left outer join [dbo].[tbl_Users] us9 on b.lvl9user = us9.userid \r\n ";

            _dbManEmptyCubby.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby.ResultsTableName = "EmptyCubby";
            _dbManEmptyCubby.ExecuteInstruction();




            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters\" + ImageDate.Text + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby.ResultsDataTable);

            theReport.RegisterData(dsEmpty);


            DataSet dsEmpty1 = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);


            theReport.Load(_reportFolder + "surveyNote.frx");

            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        public void LoadStopeNote()
        {
            noteType = "SN";

            HeightTxt.Visible = false;
            DepthTxt.Visible = false;
            LengthTxt.Visible = false;
            label10.Visible = false;
            label9.Visible = false;
            label8.Visible = false;
            label13.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            AddBtn.Visible = false;
            CubbyRightbtn.Visible = false;
            DrillRigLeftBtn.Visible = false;
            DrillCubbyRightbtn.Visible = false;
            DrillCubbyLeft.Visible = false;
            DrillRigRightBtn.Visible = false;
            PegBtn.Visible = false;

            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";

                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }

                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyStop.SqlStatement = "Select '" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section,'" + SectionManCmb.Text + "' SectionMan, '" + AdvWorkplaceCmb.Text + "' AdvWP,  " +
                                            "'" + AdvWPPegcmb.Text + "' AdvPeg,'" + AdvWPPegValtxt.Text + "' AdvPegVal, " +
                                            "'" + ExWpcmb.Text + "' EXpWp, '" + ExWpPegcmb.Text + "' ExpPeg, '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner " +
                                             " , '' CaptuserName, '' captusersig \r\n " +
                                             " , '' lvl1name, '' lvl1usersig \r\n " +
                                             " , '' lvl2name, '' lvl2usersig \r\n " +
                                             " , '' lvl3name, '' lvl3usersig \r\n " +
                                             " , '' lvl4name, '' lvl4usersig \r\n " +
                                             " , '' lvl5name, '' lvl5usersig \r\n " +
                                             " , '' lvl6name, '' lvl6usersig \r\n " +
                                             " , '' lvl7name, '' lvl7usersig \r\n " +
                                             " , '' lvl8name, '' lvl8usersig \r\n " +
                                             " , '' lvl9name, '' lvl9usersig \r\n " +
string.Empty;
            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyStope";
            _dbManEmptyStop.ExecuteInstruction();

            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyStop.ResultsDataTable);
            theReport.RegisterData(dsEmpty);

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\" + ImageDate.Text + ".png" + "' ";

            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty1 = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);
            theReport.Load(_reportFolder + "stopNote.frx");
            

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        public void LoadDevNote()
        {
            HeightTxt.Visible = false;
            DepthTxt.Visible = false;
            LengthTxt.Visible = false;
            label10.Visible = false;
            label9.Visible = false;
            label8.Visible = false;
            label13.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            AddBtn.Visible = false;
            CubbyRightbtn.Visible = false;
            DrillRigLeftBtn.Visible = false;
            DrillCubbyRightbtn.Visible = false;
            DrillCubbyLeft.Visible = false;
            DrillRigRightBtn.Visible = false;
            PegBtn.Visible = false;

            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";


                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }


                //TotPegValue = Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyStop.SqlStatement = "Select '" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section,'" + SectionManCmb.Text + "' SectionMan, '" + PegNoFrom1Cmb.Text + "' PegNoFrom1,  '" + PegTo1cmb.Text + "' PegTo1, '" + PegDistTxt.Text + "' PegDist, \r\n" +
                                            "'" + PegFrom2cmb.Text + "' PegFrom2,'" + PanelTypetxt.Text + "' Paneltype, '" + PegFaceTxt.Text + "' PegFace, '" + CarryLHSTxt.Text + "' CarryLHSTxt, \r\n" +
                                            "'" + CarryHWTxt.Text + "' CarryHWTxt, '" + SGradesPegTxt1.Text + "' SGradesPeg1, '" + SGradesPegTxt1Add.Text + "' SGradespeg1Add, '" + SGradesPegTxt2.Text + "' SGradesPeg2, '" + PegToReqRailTxt.Text + "' PegToReqRail, '" + EndInCoverTillPegCmb.Text + "' EndInCoverTillPegCmb, '" + EndInCoverTillPegTxt.Text + "' EndInCoverTillPegTxt, \r\n" +
                                           " '" + HolDistPegCmb.Text + "' HolDistPegCmb, '" + HolingDistPegTxt.Text + "'HolDistPegTxt,'" + StopDistPegCmb.Text + "' StopDistPegCmb, '" + StopDistPegTxt.Text + "' StopDistPegTxt, '" + CarryRHStxt.Text + "' CarryRHS, '" + CarryFWtxt.Text + "' CarryFW, '" + CarryRailTxt.Text + "' CarryRail, \r\n" +
                                           " '" + CarryRailTxt.Text + "' CarryRail, '" + RailTxt.Text + "' Rail, '" + GradeTxt.Text + "' Grade, '" + BackChainTxt.Text + "' BackChain,'" + BackChainValueTxt.Text + "' BackChainValue, '" + FrontChainTxt.Text + "' FrontChain,'" + FrontChainValueTxt.Text + "' FrontChainValue,  \r\n" +
                                           " '" + BgpTxt.Text + "' Bgp, '" + BGPTxt2.Text + "' BGP2, '" + FgpTxt.Text + "' Fgp, '" + FGPTxt2.Text + "' FGP2, '" + ScaleTxt.Text + "' Scale, '" + ProductionGlobalTSysSettings._Banner + "' banner " +
                                             " , '' CaptuserName, '' captusersig \r\n " +
                                             " , '' lvl1name, '' lvl1usersig \r\n " +
                                             " , '' lvl2name, '' lvl2usersig \r\n " +
                                             " , '' lvl3name, '' lvl3usersig \r\n " +
                                             " , '' lvl4name, '' lvl4usersig \r\n " +
                                             " , '' lvl5name, '' lvl5usersig \r\n " +
                                             " , '' lvl6name, '' lvl6usersig \r\n " +
                                             " , '' lvl7name, '' lvl7usersig \r\n " +
                                             " , '' lvl8name, '' lvl8usersig \r\n " +
                                             " , '' lvl9name, '' lvl9usersig \r\n " +
string.Empty;

            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty);


            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters" + @"\" + ImageDate.Text + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);

            ////MessageBox.Show(GraphType);
            theReport.Load(_reportFolder + "DevelopmentNote.frx");
           
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        public void LoadStopingNote()
        {


            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";


                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }


                //TotPegValue = Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyStop.SqlStatement = "Select '" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section,'" + SectionManCmb.Text + "' SectionMan, \r\n" +
                   " '" + SNTopOldPeg.Text + "' SNTopOldPeg,'" + SNTopToNewPeg.Text + "' SNTopToNewPeg,'" + SNTopValue.Text + "' SNTopValue, \r\n" +
                   " '" + SNTopNewPeg.Text + "' SNTopNewPeg,'" + SNTopFLP.Text + "'SNTopFLP,'" + SNTopFLPValue.Text + "' SNTopFLPValue, \r\n" +

                   " '" + SNAsgOldPeg.Text + "' SNAsgOldPeg,'" + SNAsgToNewPeg.Text + "' SNAsgToNewPeg,'" + SNAsgValue.Text + "' SNAsgValue, \r\n" +
                   " '" + SNAsgNewPeg.Text + "' SNAsgNewPeg,'" + SNAsgFLP.Text + "'SNAsgFLP,'" + SNAsgFLPValue.Text + "' SNAsgFLPValue, \r\n" +

                   " '" + SNScale.Text + "' SNScale, '" + ProductionGlobalTSysSettings._Banner + "' banner " +
                                             " , '' CaptuserName, '' captusersig \r\n " +
                                             " , '' lvl1name, '' lvl1usersig \r\n " +
                                             " , '' lvl2name, '' lvl2usersig \r\n " +
                                             " , '' lvl3name, '' lvl3usersig \r\n " +
                                             " , '' lvl4name, '' lvl4usersig \r\n " +
                                             " , '' lvl5name, '' lvl5usersig \r\n " +
                                             " , '' lvl6name, '' lvl6usersig \r\n " +
                                             " , '' lvl7name, '' lvl7usersig \r\n " +
                                             " , '' lvl8name, '' lvl8usersig \r\n " +
                                             " , '' lvl9name, '' lvl9usersig \r\n " +
string.Empty;


            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyNote";
            _dbManEmptyStop.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty);


            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters" + @"\" + ImageDate.Text + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);

            ////MessageBox.Show(GraphType);

            theReport.Load(_reportFolder + "StopingNote.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();



        }

        public void LoadStoppingNote()
        {


            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";


                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }


                //TotPegValue = Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyStop.SqlStatement = "Select '" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section,'" + SectionManCmb.Text + "' SectionMan, \r\n" +
                   " '" + StopFacetxt.Text + "' StopFace,'" + PegNumtxt.Text + "' PegNum,'" + PegNumValuetxt.Text + "' PegNumValue, \r\n" +
                   " '" + StopFace2txt.Text + "' StopFace2,'" + PegNum2txt.Text + "' PegNum2,'" + PegNumValue2txt.Text + "' PegNumValue2, \r\n" +
                   " '" + StoppingScaletxt.Text + "' StoppingScale, '" + ProductionGlobalTSysSettings._Banner + "' banner " +
                                             " , '' CaptuserName, '' captusersig \r\n " +
                                             " , '' lvl1name, '' lvl1usersig \r\n " +
                                             " , '' lvl2name, '' lvl2usersig \r\n " +
                                             " , '' lvl3name, '' lvl3usersig \r\n " +
                                             " , '' lvl4name, '' lvl4usersig \r\n " +
                                             " , '' lvl5name, '' lvl5usersig \r\n " +
                                             " , '' lvl6name, '' lvl6usersig \r\n " +
                                             " , '' lvl7name, '' lvl7usersig \r\n " +
                                             " , '' lvl8name, '' lvl8usersig \r\n " +
                                             " , '' lvl9name, '' lvl9usersig \r\n " +
string.Empty;


            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyNote";
            _dbManEmptyStop.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty);


            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters" + @"\" + ImageDate.Text + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty1 = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);
            theReport.Load(_reportFolder + "StoppingNote.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();



        }

        public void LoadCoverNote()
        {
            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";


                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }

                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyStop.SqlStatement = "Select '" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section,'" + SectionManCmb.Text + "' SectionMan, \r\n" +
                   " '" + CurrPegtxt.Text + "' CurrPeg,'" + CurrPegValuetxt.Text + "' CurrPegValue,'" + WorkFacePegtxt.Text + "' WorkFacePeg, \r\n" +
                   " '" + WorkFacePegValuetxt.Text + "' WorkFacePegValue,'" + DrillCubbyPegtxt.Text + "' DrillCubbyPeg,'" + DrillCubbyPegValuetxt.Text + "' DrillCubbyPegValue, \r\n" +
                   " '" + CoverScaletxt.Text + "' CoverScale, '" + ProductionGlobalTSysSettings._Banner + "' banner " +
                                             " , '' CaptuserName, '' captusersig \r\n " +
                                             " , '' lvl1name, '' lvl1usersig \r\n " +
                                             " , '' lvl2name, '' lvl2usersig \r\n " +
                                             " , '' lvl3name, '' lvl3usersig \r\n " +
                                             " , '' lvl4name, '' lvl4usersig \r\n " +
                                             " , '' lvl5name, '' lvl5usersig \r\n " +
                                             " , '' lvl6name, '' lvl6usersig \r\n " +
                                             " , '' lvl7name, '' lvl7usersig \r\n " +
                                             " , '' lvl8name, '' lvl8usersig \r\n " +
                                             " , '' lvl9name, '' lvl9usersig \r\n " +
string.Empty;


            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyNote";
            _dbManEmptyStop.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty);


            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters" + @"\" + ImageDate.Text + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty1 = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);
            theReport.Load(_reportFolder + "CoverNote.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        public void LoadHolingNotificationNote()
        {
            //noteType = "SN";

            HeightTxt.Visible = false;
            DepthTxt.Visible = false;
            LengthTxt.Visible = false;
            label10.Visible = false;
            label9.Visible = false;
            label8.Visible = false;
            label13.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            AddBtn.Visible = false;
            CubbyRightbtn.Visible = false;
            DrillRigLeftBtn.Visible = false;
            DrillCubbyRightbtn.Visible = false;
            DrillCubbyLeft.Visible = false;
            DrillRigRightBtn.Visible = false;
            PegBtn.Visible = false;

            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";


                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }

                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyStop.SqlStatement = "Select '" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section,'" + SectionManCmb.Text + "' SectionMan, \r\n" +
                                            "'" + FacesAtxt.Text + "' FacesA,'" + HoleIntoTxt.Text + "' HoleInto, '" + FacesBtxt.Text + "' FacesB, '" + MinesCloseToTxt.Text + "' MinesCloseTo, \r\n" +
                                            "'" + DistanceTxt.Text + "' Distance, '" + FollowingFacePegTxt.Text + "' FacePeg, '" + FollowingFaceTxt.Text + "' FollowingFace, \r\n" +
                                            "'" + FollowingFacePegTxt2.Text + "' FacePeg2, '" + FollowingFaceTxt2.Text + "'Face2,'" + HolNoteScaleTxt.Text + "' Scale, \r\n" +
                                            "'" + FollowingEndPnlTxt.Text + "' FollowingEndPnl, '" + MentionedEndPnlTxt.Text + "' MentionedEndPnl, '" + HNGeneralRemarks.Text + "' GeneralRemarks, \r\n" +
                                            "'" + StopTheFollowingFaceTxt.Text + "' StopTheFollowingFace, '" + StopTheFollowingFace2Txt.Text + "' StopTheFollowingFace2, '" + ProductionGlobalTSysSettings._Banner + "' banner " +
                                             " , '' CaptuserName, '' captusersig \r\n " +
                                             " , '' lvl1name, '' lvl1usersig \r\n " +
                                             " , '' lvl2name, '' lvl2usersig \r\n " +
                                             " , '' lvl3name, '' lvl3usersig \r\n " +
                                             " , '' lvl4name, '' lvl4usersig \r\n " +
                                             " , '' lvl5name, '' lvl5usersig \r\n " +
                                             " , '' lvl6name, '' lvl6usersig \r\n " +
                                             " , '' lvl7name, '' lvl7usersig \r\n " +
                                             " , '' lvl8name, '' lvl8usersig \r\n " +
                                             " , '' lvl9name, '' lvl9usersig \r\n " +
string.Empty;

            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty);


            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters" + @"\" + ImageDate.Text + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty1 = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);
            theReport.Load(_reportFolder + "HolingNotification.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        public void LoadOpenCastSurveyNote()
        {
            //noteType = "SN";

            HeightTxt.Visible = false;
            DepthTxt.Visible = false;
            LengthTxt.Visible = false;
            label10.Visible = false;
            label9.Visible = false;
            label8.Visible = false;
            label13.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            AddBtn.Visible = false;
            CubbyRightbtn.Visible = false;
            DrillRigLeftBtn.Visible = false;
            DrillCubbyRightbtn.Visible = false;
            DrillCubbyLeft.Visible = false;
            DrillRigRightBtn.Visible = false;
            PegBtn.Visible = false;

            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";


                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }

                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyStop.SqlStatement = "Select '" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section,'" + SectionManCmb.Text + "' SectionMan, \r\n" +
                                            "'" + OpenCastScaleTxt.Text + "' Scale,'" + OCGeneralRemarks.Text + "' GeneralRemarks, \r\n" +
                                            "'" + ProductionGlobalTSysSettings._Banner + "' banner " +
                                             " , '' CaptuserName, '' captusersig \r\n " +
                                             " , '' lvl1name, '' lvl1usersig \r\n " +
                                             " , '' lvl2name, '' lvl2usersig \r\n " +
                                             " , '' lvl3name, '' lvl3usersig \r\n " +
                                             " , '' lvl4name, '' lvl4usersig \r\n " +
                                             " , '' lvl5name, '' lvl5usersig \r\n " +
                                             " , '' lvl6name, '' lvl6usersig \r\n " +
                                             " , '' lvl7name, '' lvl7usersig \r\n " +
                                             " , '' lvl8name, '' lvl8usersig \r\n " +
                                             " , '' lvl9name, '' lvl9usersig \r\n " +
string.Empty;

            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty);


            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters" + @"\" + ImageDate.Text + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty1 = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);
            theReport.Load(_reportFolder + "OpenCastSurveyNote.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        public void LoadStartNote()
        {

            HeightTxt.Visible = false;
            DepthTxt.Visible = false;
            LengthTxt.Visible = false;
            label10.Visible = false;
            label9.Visible = false;
            label8.Visible = false;
            label13.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            AddBtn.Visible = false;
            CubbyRightbtn.Visible = false;
            DrillRigLeftBtn.Visible = false;
            DrillCubbyRightbtn.Visible = false;
            DrillCubbyLeft.Visible = false;
            DrillRigRightBtn.Visible = false;
            PegBtn.Visible = false;

            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";


                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }

                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyStop.SqlStatement = "Select '" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section,'" + SectionManCmb.Text + "' SectionMan, \r\n" +
                                            "'" + NameOfFaceTxt.Text + "' NameOfFace,'" + NameOfFacePeg.Text + "' NameOfFacePeg, '" + NameOfFaceTxt2.Text + "' NameOfFace2, \r\n" +
                                            "'" + holingNoteTxt.Text + "' holingNote, '" + SurveyTxt.Text + "' Survey, '" + SafelyIntoTxt.Text + "' SafelyInto, \r\n" +
                                            "'" + SNScaleTxt.Text + "' Scale, '" + SNGeneralRemarks.Text + "' GeneralRemarks, '" + HolingNoteRefNo.Text + "' HolingNoteRefNo, " +
                                            "'" + ProductionGlobalTSysSettings._Banner + "' banner " +
                                             " , '' CaptuserName, '' captusersig \r\n " +
                                             " , '' lvl1name, '' lvl1usersig \r\n " +
                                             " , '' lvl2name, '' lvl2usersig \r\n " +
                                             " , '' lvl3name, '' lvl3usersig \r\n " +
                                             " , '' lvl4name, '' lvl4usersig \r\n " +
                                             " , '' lvl5name, '' lvl5usersig \r\n " +
                                             " , '' lvl6name, '' lvl6usersig \r\n " +
                                             " , '' lvl7name, '' lvl7usersig \r\n " +
                                             " , '' lvl8name, '' lvl8usersig \r\n " +
                                             " , '' lvl9name, '' lvl9usersig \r\n " +
string.Empty;

            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty);


            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters" + @"\" + ImageDate.Text + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty1 = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);
            theReport.Load(_reportFolder + "StartNote.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();



        }

        public void LoadStopPanelReportNote()
        {
            HeightTxt.Visible = false;
            DepthTxt.Visible = false;
            LengthTxt.Visible = false;
            label10.Visible = false;
            label9.Visible = false;
            label8.Visible = false;
            label13.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            AddBtn.Visible = false;
            CubbyRightbtn.Visible = false;
            DrillRigLeftBtn.Visible = false;
            DrillCubbyRightbtn.Visible = false;
            DrillCubbyLeft.Visible = false;
            DrillRigRightBtn.Visible = false;
            PegBtn.Visible = false;

            if (IsChanged == false)
            {
                MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
                _dbManID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManID.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote ";


                _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManID.ExecuteInstruction();

                if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
                {
                    _dbManID.ResultsDataTable.Rows[0][0] = "0";
                }

                IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();
            }

            IsChanged = false;

            MWDataManager.clsDataAccess _dbManEmptyStop = new MWDataManager.clsDataAccess();
            _dbManEmptyStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyStop.SqlStatement = "Select '" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section,'" + SectionManCmb.Text + "' SectionMan, \r\n" +
                                            "'" + ReasonPnlStoppedTxt.Text + "' ReasonPnlStopped,'" + SupFromFacePermTxt.Text + "' SupFromFacePerm, '" + TypeSup1Txt.Text + "' TypeSup1, \r\n" +
                                            "'" + SupFromFaceTempTxt.Text + "' SupFromFaceTemp, '" + TypeSup2Txt.Text + "' TypeSup2, '" + TopPegTxt.Text + "' TopPeg, \r\n" +
                                            "'" + DistFaceTopTxt.Text + "' DistFaceTop, '" + BotPegTxt.Text + "' BotPeg, '" + DistFaceBotTxt.Text + "' DistFaceBot, \r\n" +
                                            "'" + FaceTxt.Text + "' Face,'" + FaceMTxt.Text + "' FaceM, '" + GullyTxt.Text + "' Gully, \r\n" +
                                            "'" + GullyMTxt.Text + "' GullyM,'" + RseTxt.Text + "' Rse, '" + RseMTxt.Text + "' RseM, \r\n" +
                                            "'" + ReSweepsTxt.Text + "' ReSweeps,'" + ReSweepsMTxt.Text + "' ReSweepsM, '" + CurOreResCatTxt.Text + "' CurOreResCat, \r\n" +
                                            "'" + OreBlockSizeTxt.Text + "' OreBlockSize,'" + NewOreResCattxt.Text + "' NewOreResCat, '" + OreBlockvaluesTxt.Text + "' OreBlockvalues, \r\n" +
                                            "'" + RockEngRepNoTxt.Text + "' RockEngRepNo,'" + SuppAndSafeGN.Text + "' SuppAndSafeGN, '" + TonsGN.Text + "' TonsGN,'" + ReclamationGN.Text + "'ReclamationGN,  \r\n" +
                                            "'" + ProductionGlobalTSysSettings._Banner + "' banner " +
                                             " , '' CaptuserName, '' captusersig \r\n " +
                                             " , '' lvl1name, '' lvl1usersig \r\n " +
                                             " , '' lvl2name, '' lvl2usersig \r\n " +
                                             " , '' lvl3name, '' lvl3usersig \r\n " +
                                             " , '' lvl4name, '' lvl4usersig \r\n " +
                                             " , '' lvl5name, '' lvl5usersig \r\n " +
                                             " , '' lvl6name, '' lvl6usersig \r\n " +
                                             " , '' lvl7name, '' lvl7usersig \r\n " +
                                             " , '' lvl8name, '' lvl8usersig \r\n " +
                                             " , '' lvl9name, '' lvl9usersig \r\n " +
string.Empty;

            _dbManEmptyStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyStop.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyStop.ResultsTableName = "EmptyDev";
            _dbManEmptyStop.ExecuteInstruction();

            DataSet dsEmpty = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyStop.ResultsDataTable);

            theReport.RegisterData(dsEmpty);

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters" + @"\" + ImageDate.Text + ".png" + "' ";

            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();

            DataSet dsEmpty1 = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            theReport.RegisterData(dsEmpty1);
            theReport.Load(_reportFolder + "StopPanelReportNote.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

        }

        void EditCubby()
        {
            HeightTxt.Visible = false;
            DepthTxt.Visible = false;
            LengthTxt.Visible = false;
            label10.Visible = false;
            label9.Visible = false;
            label8.Visible = false;
            label13.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            AddBtn.Visible = false;
            CubbyRightbtn.Visible = false;
            DrillRigLeftBtn.Visible = false;
            DrillCubbyRightbtn.Visible = false;
            DrillCubbyLeft.Visible = false;
            DrillRigRightBtn.Visible = false;
            PegBtn.Visible = false;
            getImagePnl.Dock = DockStyle.Right;
            panel6.Dock = DockStyle.Fill;

            noteType = "CN";

            MWDataManager.clsDataAccess _dbManSecMan = new MWDataManager.clsDataAccess();
            _dbManSecMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManSecMan.SqlStatement = "select s.Sectionid SID,s.Name SName,s1.Sectionid S1ID,s1.Name S1Name,s2.Sectionid S2ID,s2.Name S2Name,t.*,w.Description from  tbl_SurveyNote t , tbl_Workplace w,tbl_Section s,tbl_Section s1,tbl_Section s2 where t.TSNID = '" + IDtxt.Text + "' ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " and t.Workplaceid = w.Workplaceid";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " and t.Sectionid = s.Sectionid and s.prodmonth = (select CurrentProductionMonth from tbl_SysSet)";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " and s.ReportToSectionid = s1.Sectionid and s1.prodmonth = (select CurrentProductionMonth from tbl_SysSet)";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " and s1.ReportToSectionid = s2.Sectionid and s2.prodmonth = (select CurrentProductionMonth from tbl_SysSet) ";



            _dbManSecMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSecMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSecMan.ExecuteInstruction();

            date.Value = Convert.ToDateTime(_dbManSecMan.ResultsDataTable.Rows[0]["TheDate"].ToString());
            SectionManCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["S2ID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["S2Name"].ToString();
            SectionCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["S1ID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["S1Name"].ToString();
            SBCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["SID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["SName"].ToString();
            //Callie
            //WPList.SelectedItem = _dbManSecMan.ResultsDataTable.Rows[0]["Workplaceid"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["Description"].ToString();
            WPList.SelectedItem = _dbManSecMan.ResultsDataTable.Rows[0]["Description"].ToString();

            PegNoFrom1Cmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegNo"].ToString();
            PegTo1cmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ToPeg2"].ToString();
            PegDistTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegtoPegVal"].ToString();
            PegFrom2cmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ToPeg2"].ToString();
            PanelTypetxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PanelType"].ToString();
            PegFaceTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegtoFLPVal"].ToString();
            CarryLHSTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["LHS"].ToString();
            CarryRHStxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["RHS"].ToString();
            CarryHWTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["HW"].ToString();
            CarryFWtxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["FW"].ToString();
            CarryRailTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["CarrRailVal"].ToString();
            ChainAtPegCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegatChain"].ToString();
            ChainAtPegTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegatChainVal"].ToString();
            ChainTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ChainatLP"].ToString();
            LPTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["LPVal"].ToString();
            PegToReqRailCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegReqRail"].ToString();
            PegToReqRailTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ReqRailVal"].ToString();
            RailTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["RailVal"].ToString();
            SGradesPegTxt1.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegSGradeVal1"].ToString();
            SGradesPegTxt1Add.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegSGradeVal2"].ToString();
            SGradesPegTxt2.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegSGradeVal3"].ToString();
            SGradesPegTxt2Add.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegSGradeVal4"].ToString();
            StopDistPegCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegStopDist"].ToString();
            StopDistPegTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegStopDistVal"].ToString();
            PegValueCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ToPeg2"].ToString();
            PegValTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegVal"].ToString();
            PegToFaceTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ToPeg2Face"].ToString();
            PegToFaceCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ToPeg2"].ToString();
            AdvToDateTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["AdvtoDate"].ToString();
            HolDistPegCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegHoldDist"].ToString();
            HolingDistPegTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegHoldDistVal"].ToString();
            GradeTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["Grade"].ToString();
            ImageDate.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ImageDate"].ToString();
            HolingNoteNoTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["HolingNoteNo"].ToString();
            NotesTxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["Notes"].ToString();

            LoadEmptyCubby();
        }

        public void EditStopNote()
        {
            HeightTxt.Visible = false;
            DepthTxt.Visible = false;
            LengthTxt.Visible = false;
            label10.Visible = false;
            label9.Visible = false;
            label8.Visible = false;
            label13.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            AddBtn.Visible = false;
            CubbyRightbtn.Visible = false;
            DrillRigLeftBtn.Visible = false;
            DrillCubbyRightbtn.Visible = false;
            DrillCubbyLeft.Visible = false;
            DrillRigRightBtn.Visible = false;
            PegBtn.Visible = false;
            getImagePnl.Dock = DockStyle.Right;
            panel7.Dock = DockStyle.Fill;
            panel6.Visible = false;
            panel7.Visible = true;
            WorkplaceCmb.Visible = false;
            

            noteType = "SN";

            MWDataManager.clsDataAccess _dbManSecMan = new MWDataManager.clsDataAccess();
            _dbManSecMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManSecMan.SqlStatement = "select  s.Sectionid SID,s.Name SName,s1.Sectionid S1ID,s1.Name S1Name,s2.Sectionid S2ID,s2.Name S2Name,t.TSNID,t.NoteType, t.Workplaceid AdvWPID, w1.Description AdvWP, t.Workplaceid2 ExWpID, ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " w2.Description ExWp, t.* from  tbl_SurveyNote t ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " left outer join Workplace w1 ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " on t.Workplaceid = w1.Workplaceid ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " left outer join Workplace w2 ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " on t.Workplaceid2 = w2.Workplaceid ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " left outer join tbl_Section s ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " on t.Sectionid = s.Sectionid and s.prodmonth = (select CurrentProductionmonth from tbl_SysSet) ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " left outer join tbl_Section s1  ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " on s.ReporttoSectionid = s1.Sectionid and s1.prodmonth = (select CurrentProductionmonth from tbl_SysSet)";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " left outer join tbl_Section s2  ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " on s1.ReporttoSectionid = s2.Sectionid and s2.prodmonth = (select CurrentProductionmonth from tbl_SysSet) ";
            _dbManSecMan.SqlStatement = _dbManSecMan.SqlStatement + " where t.TSNID = '" + IDtxt.Text + "' ";

            _dbManSecMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSecMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSecMan.ExecuteInstruction();

            date.Value = Convert.ToDateTime(_dbManSecMan.ResultsDataTable.Rows[0]["TheDate"].ToString());
            SectionManCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["S2ID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["S2Name"].ToString();
            SectionCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["S1ID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["S1Name"].ToString();
            SBCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["SID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["SName"].ToString();
            //AdvWorkplaceCmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["AdvWPID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["AdvWP"].ToString();
            AdvWPPegcmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegNo"].ToString();
            AdvWPPegValtxt.Text = _dbManSecMan.ResultsDataTable.Rows[0]["PegDist"].ToString();
            // ExWpcmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ExWpID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["ExWp"].ToString();
            ExWpPegcmb.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ToPeg2"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["ToPeg2Val"].ToString();
            ImageDate.Text = _dbManSecMan.ResultsDataTable.Rows[0]["ImageDate"].ToString();
            //Callie
            //WPList.SelectedItem = _dbManSecMan.ResultsDataTable.Rows[0]["AdvWPID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["AdvWP"].ToString();
            //WP2List.SelectedItem = _dbManSecMan.ResultsDataTable.Rows[0]["ExWpID"].ToString() + ":" + _dbManSecMan.ResultsDataTable.Rows[0]["ExWp"].ToString();
            WPList.SelectedItem = _dbManSecMan.ResultsDataTable.Rows[0]["AdvWP"].ToString();
            WP2List.SelectedItem = _dbManSecMan.ResultsDataTable.Rows[0]["ExWp"].ToString();

            LoadStopeNote();
        }

        void loadSections()
        {

            ///////////////////////////Load Section Managers//////////////////////////
            SectionManCmb.Items.Clear();

            MWDataManager.clsDataAccess _dbManSecMan = new MWDataManager.clsDataAccess();
            _dbManSecMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManSecMan.SqlStatement = "select * from tbl_Section where prodmonth = '" + ProductionGlobalTSysSettings._currentProductionMonth + "' and Hierarchicalid = 3 ";

            _dbManSecMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSecMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSecMan.ExecuteInstruction();

            dtSections = _dbManSecMan.ResultsDataTable;
            foreach (DataRow dr in dtSections.Rows)
            {

                SectionManCmb.Items.Add(dr["sectionid"].ToString() + ":" + dr["Name"].ToString());

            }
            /////////////////////////////////////////////////////////////////////////


            SectionCmb.Items.Clear();
            dtSections.Clear();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbMan.SqlStatement = " select Distinct(s2.Sectionid),s2.Name from tbl_Planmonth p, tbl_Section s,tbl_Section s1, tbl_Section s2 where p.prodmonth = '" + ProductionGlobalTSysSettings._currentProductionMonth + "' ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " and p.prodmonth = s.prodmonth and p.sectionid = s.Sectionid ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " and s.Prodmonth = s1.Prodmonth and s.ReportToSectionid = s1.Sectionid ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " and s1.Prodmonth = s2.Prodmonth and s1.ReporttoSectionid = s2.Sectionid and p.activity = '1'";


            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dtSections = _dbMan.ResultsDataTable;

            //SectionCmb.Items.Add("All");
            foreach (DataRow dr in dtSections.Rows)
            {

                SectionCmb.Items.Add(dr["sectionid"].ToString() + ":" + dr["Name"].ToString());

            }

        }

        void LoadAllWorkplaces()
        {
            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbManWP.SqlStatement = " select Description from tbl_Workplace_Total where 0=0 ";
            if (WorkplacesChecked.Checked == false)
                _dbManWP.SqlStatement = _dbManWP.SqlStatement + " and GMSIWPID in (select GMSIWPID from tbl_Workplace) ";
            _dbManWP.SqlStatement = _dbManWP.SqlStatement + "order by Description ";
            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            dtWP = _dbManWP.ResultsDataTable;

            bs1.DataSource = dtWP;

            WPList.DataSource = bs1;
            WPList.DisplayMember = "Description";

            WPList.SelectedIndex = 0;

        }

        private void CubbyStopNotes_Load(object sender, EventArgs e)
        {
            //this.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            IDtxt.Enabled = false;

            SectionCmb.Items.Add("A114:Mafumo, Zacarias Joaquim");
            SBCmb.Items.Add("A1141:Fafa");

            LoadAllWorkplaces();

            panel6.Visible = false;
            gbCoverNote.Visible = false;
            gbHolingNotification.Visible = false;
            gbOpenCastSurveyNote.Visible = false;
            gbStartNote.Visible = false;
            gbStopPanelReportNote.Visible = false;
            gbStopingNote.Visible = false;
            gbStoppingNote.Visible = false;


            if (noteType == "DN")
            {
                panel6.Visible = true;
                LoadDevNote();
            }

            if (noteType == "STN")
            {
                gbStopingNote.Visible = true;
                LoadStopingNote();
            }

            if (noteType == "SSN")
            {
                gbStopingNote.Visible = true;
                LoadStoppingNote();
            }

            if (noteType == "CVN")
            {
                gbCoverNote.Visible = true;
                LoadCoverNote();
            }

            if (noteType == "HNN")
            {
                gbHolingNotification.Visible = true;
                LoadHolingNotificationNote();
            }

            if (noteType == "OCN")
            {
                gbOpenCastSurveyNote.Visible = true;
                LoadOpenCastSurveyNote();
            }

            if (noteType == "SRN")
            {
                gbStartNote.Visible = true;
                LoadStartNote();
            }

            if (noteType == "SPR")
            {
                gbStopPanelReportNote.Visible = true;
                LoadStopPanelReportNote();
            }

            if (noteType == "CN")
            {

                if (IsChanged != true)
                {
                    ImageDate.Text = Convert.ToInt64(DateTime.Now.ToBinary()).ToString();
                    LoadEmptyCubby();
                }
                else
                {
                    EditCubby();
                }
            }
            else
            {
                loadSections();
                if (IsChanged != true)
                {
                    ImageDate.Text = Convert.ToInt64(DateTime.Now.ToBinary()).ToString();
                }
                else
                {

                }

            }

            PegBtn.Visible = false;

        }

        private void SectionCmb_SelectedIndexChanged(object sender, EventArgs e)
        {

            ////////Load SB//////////////////
            SBCmb.Items.Clear();
            dtSections.Clear();

            MWDataManager.clsDataAccess _dbManSB = new MWDataManager.clsDataAccess();
            _dbManSB.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManSB.SqlStatement = " select Distinct(S1.Sectionid),s1.Name from tbl_Planmonth p, tbl_Section s,tbl_Section s1, tbl_Section s2 ";
            _dbManSB.SqlStatement = _dbManSB.SqlStatement + " where p.prodmonth = '" + ProductionGlobalTSysSettings._currentProductionMonth + "'  and p.prodmonth = s.prodmonth  ";
            _dbManSB.SqlStatement = _dbManSB.SqlStatement + " and p.sectionid = s.Sectionid  and s.Prodmonth = s1.Prodmonth and s.ReportToSectionid = s1.Sectionid   ";
            _dbManSB.SqlStatement = _dbManSB.SqlStatement + " and s1.Prodmonth = s2.Prodmonth and s1.ReporttoSectionid = s2.Sectionid and p.activity = '1' ";
            _dbManSB.SqlStatement = _dbManSB.SqlStatement + " and s2.Sectionid = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text) + "' ";

            _dbManSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSB.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSB.ExecuteInstruction();

            dtSections = _dbManSB.ResultsDataTable;

            foreach (DataRow dr in dtSections.Rows)
            {
                SBCmb.Items.Add(dr["sectionid"].ToString() + ":" + dr["Name"].ToString());
            }
        }

        private void WorkplaceCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string WP = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(WorkplaceCmb.Text);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbMan.SqlStatement = "Select distinct(SectionID) sectionid from tbl_Planmonth " +
                                "where ProdMonth = (select max(Prodmonth) prodmonth from tbl_Planmonth " +
                                "where activity = 1) and activity = 1 " +
                                "and workplaceid = '" + WP + "'  Order by SectionID ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dtSections = _dbMan.ResultsDataTable;

            if (dtSections.Rows.Count != 0)
            {
                for (int x = 0; x <= SectionCmb.Items.Count - 1; x++)
                {

                    if (SectionCmb.Items[x].ToString() == dtSections.Rows[0][0].ToString())
                    {
                        SectionCmb.SelectedIndex = x;
                    }

                }
            }

            ///////////////////////Peg No///////////////////////////

            PegNoFrom1Cmb.Items.Clear();
            PegTo1cmb.Items.Clear();
            PegFrom2cmb.Items.Clear();
            ChainAtPegCmb.Items.Clear();
            PegToReqRailCmb.Items.Clear();
            StopDistPegCmb.Items.Clear();
            PegValueCmb.Items.Clear();
            PegToFaceCmb.Items.Clear();
            HolDistPegCmb.Items.Clear();

            MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
            _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManPeg.SqlStatement = "Select * from Peg where workplaceid = '" + WP + "' " +
                                     " order by value desc, Pegid ";

            _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManPeg.ExecuteInstruction();

            DataTable dtPeg = _dbManPeg.ResultsDataTable;

            foreach (DataRow dr in dtPeg.Rows)
            {
                PegNoFrom1Cmb.Items.Add(dr["PegID"].ToString());
                PegTo1cmb.Items.Add(dr["PegID"].ToString());
                PegFrom2cmb.Items.Add(dr["PegID"].ToString());
                ChainAtPegCmb.Items.Add(dr["PegID"].ToString());
                PegToReqRailCmb.Items.Add(dr["PegID"].ToString());
                StopDistPegCmb.Items.Add(dr["PegID"].ToString());
                PegValueCmb.Items.Add(dr["PegID"].ToString());
                PegToFaceCmb.Items.Add(dr["PegID"].ToString());
                HolDistPegCmb.Items.Add(dr["PegID"].ToString());
            }
            PegNoFrom1Cmb.Items.Add("Start:0.0");
            PegTo1cmb.Items.Add("Start:0.0");
            PegFrom2cmb.Items.Add("Start:0.0");
            ChainAtPegCmb.Items.Add("Start:0.0");
            PegToReqRailCmb.Items.Add("Start:0.0");
            StopDistPegCmb.Items.Add("Start:0.0");
            PegValueCmb.Items.Add("Start:0.0");
            PegToFaceCmb.Items.Add("Start:0.0");
            HolDistPegCmb.Items.Add("Start:0.0");
            PegNoFrom1Cmb.SelectedIndex = 0;
        }

        void loadCubbyLeft()
        {
            ChartType = "NormalCubbyLeft";

            MWDataManager.clsDataAccess _dbManCubbyLeft = new MWDataManager.clsDataAccess();
            _dbManCubbyLeft.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManCubbyLeft.SqlStatement = "Select '" + Peg + "','" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section, '" + WorkplaceCmb.Text + "' WP,  " +
                                            "'" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(PegNoFrom1Cmb.Text) + "' + '+' + '" + PegDistTxt.Text + "' [PegDist], '" + DepthTxt.Text + "' Depth, '" + HeightTxt.Text + "' Height, " +
                                            "'" + LengthTxt.Text + "' Length, '" + RemarkTxt.Text + "' Remark, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + TotPegValue.ToString() + "' TotPeg ";

            _dbManCubbyLeft.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCubbyLeft.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCubbyLeft.ResultsTableName = "CubbyLeft";
            _dbManCubbyLeft.ExecuteInstruction();

            DataSet dsCubbyLeft = new DataSet();
            dsCubbyLeft.Tables.Add(_dbManCubbyLeft.ResultsDataTable);

            theReport.RegisterData(dsCubbyLeft);
            theReport.Load(_reportFolder + "CubbyLeft.frx");
           

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            Peg = "N";
            ImageType = "NCL";
            if (PegNoFrom1Cmb.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Peg", "Select Peg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PegDistTxt.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a Peg Distance", "Enter Peg Distance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TotPegValue = Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
            loadCubbyLeft();
            PegBtn_Click(null, null);
        }

        void LoadCubbyRight()
        {
            ChartType = "NormalCubbyRight";
            MWDataManager.clsDataAccess _dbManCubbyRight = new MWDataManager.clsDataAccess();
            _dbManCubbyRight.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManCubbyRight.SqlStatement = "Select '" + Peg + "','" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section, '" + WorkplaceCmb.Text + "' WP,  " +
                                            "'" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(PegNoFrom1Cmb.Text) + "' + '+' + '" + PegDistTxt.Text + "' [PegDist], '" + DepthTxt.Text + "' Depth, '" + HeightTxt.Text + "' Height, " +
                                            "'" + LengthTxt.Text + "' Length, '" + RemarkTxt.Text + "' Remark, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + TotPegValue.ToString() + "' TotPeg ";

            _dbManCubbyRight.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCubbyRight.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCubbyRight.ResultsTableName = "CubbyRight";
            _dbManCubbyRight.ExecuteInstruction();

            DataSet dsCubbyRight = new DataSet();
            dsCubbyRight.Tables.Add(_dbManCubbyRight.ResultsDataTable);

            theReport.RegisterData(dsCubbyRight);
            theReport.Load(_reportFolder + "CubbyRight.frx");
            

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void CubbyRightbtn_Click(object sender, EventArgs e)
        {
            Peg = "N";
            ImageType = "NCR";
            if (PegNoFrom1Cmb.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Peg", "Select Peg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PegDistTxt.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a Peg Distance", "Enter Peg Distance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TotPegValue = Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
            LoadCubbyRight();
            PegBtn_Click(null, null);
        }

        void LoadDrillRigLeft()
        {
            ChartType = "DrillLeft";
            MWDataManager.clsDataAccess _dbManDrillRigLeft = new MWDataManager.clsDataAccess();
            _dbManDrillRigLeft.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManDrillRigLeft.SqlStatement = "Select '" + Peg + "','" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section, '" + WorkplaceCmb.Text + "' WP,  " +
                                            "'" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(PegNoFrom1Cmb.Text) + "' + '+' + '" + PegDistTxt.Text + "' [PegDist], '" + DepthTxt.Text + "' Depth, '" + HeightTxt.Text + "' Height, " +
                                            "'" + LengthTxt.Text + "' Length, '" + RemarkTxt.Text + "' Remark, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + TotPegValue.ToString() + "' TotPeg ";

            _dbManDrillRigLeft.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDrillRigLeft.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDrillRigLeft.ResultsTableName = "DrillRigLeft";
            _dbManDrillRigLeft.ExecuteInstruction();

            DataSet dsDrillRigleft = new DataSet();
            dsDrillRigleft.Tables.Add(_dbManDrillRigLeft.ResultsDataTable);

            theReport.RegisterData(dsDrillRigleft);
            theReport.Load(_reportFolder + "DrillRigLeft.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void DrillRigLeftBtn_Click(object sender, EventArgs e)
        {
            Peg = "N";
            ImageType = "NRL";
            if (PegNoFrom1Cmb.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Peg", "Select Peg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PegDistTxt.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a Peg Distance", "Enter Peg Distance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TotPegValue = Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
            LoadDrillRigLeft();
            PegBtn_Click(null, null);
        }

        void LoadDrillRigRight()
        {
            ChartType = "DrilRight";
            MWDataManager.clsDataAccess _dbManDrillRigRight = new MWDataManager.clsDataAccess();
            _dbManDrillRigRight.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManDrillRigRight.SqlStatement = "Select '" + Peg + "','" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section, '" + WorkplaceCmb.Text + "' WP,  " +
                                            "'" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(PegNoFrom1Cmb.Text) + "' + '+' + '" + PegDistTxt.Text + "' [PegDist], '" + DepthTxt.Text + "' Depth, '" + HeightTxt.Text + "' Height, " +
                                            "'" + LengthTxt.Text + "' Length, '" + RemarkTxt.Text + "' Remark, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + TotPegValue.ToString() + "' TotPeg ";

            _dbManDrillRigRight.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDrillRigRight.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDrillRigRight.ResultsTableName = "DrillRigRight";
            _dbManDrillRigRight.ExecuteInstruction();

            DataSet dsDrillRigRight = new DataSet();
            dsDrillRigRight.Tables.Add(_dbManDrillRigRight.ResultsDataTable);

            theReport.RegisterData(dsDrillRigRight);
            theReport.Load(_reportFolder + "DrillRigRight.frx");
            
            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void DrillRigRightBtn_Click(object sender, EventArgs e)
        {
            Peg = "N";
            ImageType = "NRR";
            if (PegNoFrom1Cmb.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Peg", "Select Peg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PegDistTxt.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a Peg Distance", "Enter Peg Distance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TotPegValue = Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
            LoadDrillRigRight();
            PegBtn_Click(null, null);
        }

        void LoadCubbyL()
        {
            ChartType = "CubbyLeft";
            MWDataManager.clsDataAccess _dbMancLeft = new MWDataManager.clsDataAccess();
            _dbMancLeft.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbMancLeft.SqlStatement = "Select '" + Peg + "','" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section, '" + WorkplaceCmb.Text + "' WP,  " +
                                            "'" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(PegNoFrom1Cmb.Text) + "' + '+' + '" + PegDistTxt.Text + "' [PegDist], '" + DepthTxt.Text + "' Depth, '" + HeightTxt.Text + "' Height, " +
                                            "'" + LengthTxt.Text + "' Length, '" + RemarkTxt.Text + "' Remark, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + TotPegValue.ToString() + "' TotPeg ";


            _dbMancLeft.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMancLeft.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMancLeft.ResultsTableName = "CLeft";
            _dbMancLeft.ExecuteInstruction();


            DataSet dsCLeft = new DataSet();
            dsCLeft.Tables.Add(_dbMancLeft.ResultsDataTable);

            theReport.RegisterData(dsCLeft);
            theReport.Load(_reportFolder + "CubbyLeftside.frx");
            
            // theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void DrillCubbyLeft_Click(object sender, EventArgs e)
        {
            Peg = "N";
            ImageType = "DCL";
            if (PegNoFrom1Cmb.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Peg", "Select Peg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PegDistTxt.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a Peg Distance", "Enter Peg Distance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TotPegValue = Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
            LoadCubbyL();
            PegBtn_Click(null, null);
        }

        void LoadCubbyRightSide()
        {
            ChartType = "CubbyRight";
            MWDataManager.clsDataAccess _dbMancRight = new MWDataManager.clsDataAccess();
            _dbMancRight.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbMancRight.SqlStatement = "Select '" + Peg + "','" + Peg + "','" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section, '" + WorkplaceCmb.Text + "' WP,  " +
                                            "'" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(PegNoFrom1Cmb.Text) + "' + '+' + '" + PegDistTxt.Text + "' [PegDist], '" + DepthTxt.Text + "' Depth, '" + HeightTxt.Text + "' Height, " +
                                            "'" + LengthTxt.Text + "' Length, '" + RemarkTxt.Text + "' Remark, '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + TotPegValue.ToString() + "' TotPeg ";


            _dbMancRight.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMancRight.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMancRight.ResultsTableName = "CRight";
            _dbMancRight.ExecuteInstruction();

            DataSet dsRight = new DataSet();

            dsRight.Tables.Add(_dbMancRight.ResultsDataTable);

            theReport.RegisterData(dsRight);
            theReport.Load(_reportFolder + "CubbyRightSide.frx");
            

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void DrillCubbyRightbtn_Click(object sender, EventArgs e)
        {
            Peg = "N";
            ImageType = "DCR";
            if (PegNoFrom1Cmb.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Peg", "Select Peg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PegDistTxt.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a Peg Distance", "Enter Peg Distance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TotPegValue = Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
            LoadCubbyRightSide();
            PegBtn_Click(null, null);
        }

        private void PegBtn_Click(object sender, EventArgs e)
        {
            if (PegNoFrom1Cmb.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Peg", "Select Peg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PegDistTxt.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a Peg Distance", "Enter Peg Distance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ChartType == "EmptyCubby")
            {
                Peg = "Y";
                LoadEmptyCubby();
            }

            if (ChartType == "NormalCubbyLeft")
            {
                Peg = "Y";
                loadCubbyLeft();
            }
            if (ChartType == "NormalCubbyRight")
            {
                Peg = "Y";
                LoadCubbyRight();
            }
            if (ChartType == "DrillLeft")
            {
                Peg = "Y";
                LoadDrillRigLeft();
            }
            if (ChartType == "DrilRight")
            {
                Peg = "Y";
                LoadDrillRigRight();
            }
            if (ChartType == "CubbyLeft")
            {
                Peg = "Y";
                LoadCubbyL();
            }
            if (ChartType == "CubbyRight")
            {
                Peg = "Y";
                LoadCubbyRightSide();
            }


        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void SaveCubby()
        {
            string Latest = string.Empty;

            if (noteType == "SPR")
            {
                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;

                string MissfiresChecked = "N";
                string BarricadedChecked = "N";
                string ReclaimedChecked = "N";
                string PnlSweptChecked = "N";
                String StrikeGullyChecked = "N";
                String StopeChecked = "N";
                string CleanedChecked = "N";
                string AllStuffChecked = "N";

                if (MissfiresYes.Checked == true)
                {
                    MissfiresChecked = "Y";
                }

                if (Barricadedyes.Checked == true)
                {
                    BarricadedChecked = "Y";
                }

                if (ReclaimedYes.Checked == true)
                {
                    ReclaimedChecked = "Y";
                }

                if (PnlSweptYes.Checked == true)
                {
                    PnlSweptChecked = "Y";
                }

                if (SrikeGullyYes.Checked == true)
                {
                    StrikeGullyChecked = "Y";
                }

                if (StopeYes.Checked == true)
                {
                    StopeChecked = "Y";
                }

                if (CleanedYes.Checked == true)
                {
                    CleanedChecked = "Y";
                }

                if (AllStuffYes.Checked == true)
                {
                    AllStuffChecked = "Y";
                }


                if (SectionCmb.Text == string.Empty)
                {
                    MO = string.Empty;
                }
                else
                {
                    MO = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text);
                }

                if (SectionManCmb.Text == string.Empty)
                {
                    SecMan = string.Empty;
                }
                else
                {
                    SecMan = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionManCmb.Text);
                }

                if (SBCmb.Text == string.Empty)
                {
                    SB = string.Empty;
                }
                else
                {
                    SB = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text);
                }

                if (StopRemark.Substring(0, 1) == "+")
                {
                    StopRemark = StopRemark.Substring(1, StopRemark.Length - 1);
                }


                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSave.SqlStatement = "Delete from   tbl_SurveyNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "Delete from  tbl_SurveyNote_StopPanelReportNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID, \r\n" +
                                           " TheDate,MOSection, \r\n" +
                                           "UserName,IsCompleted,ImageType,IsDeleted,IsLatest,ImageDate,  \r\n" +
                                           " SecManager  )" +

                                           " values('" + IDtxt.Text + "','" + noteType + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(WPList.Text.ToString()) + "','" + SB + "', \r\n" +
                                           " '" + date.Value + "','" + MO + "', \r\n" +
                                           " '" + TUserInfo.UserID + "','N','" + ImageType + "','N','" + Latest + "', '" + ImageDate.Text + "', \r\n" +
                                           " '" + SecMan + "') \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote_StopPanelReportNote(TSNID,\r\n" +

                                          " ReasonPnlStopped,SupFromFacePerm,TypeOfSupport,   \r\n" +
                                          " SupFromFaceTemp,TypeOfSupport2,TopPeg,  \r\n" +
                                          " DistToFace,BotPeg,DistFace2,  \r\n" +
                                          " Face,FaceMetres,GullyTons,  \r\n" +
                                          " GullyMetres,Rse,RseMetres,  \r\n" +
                                          " ReSweepsTons,ReSweepsMetres,CurrentOreReserveCategory,  \r\n" +
                                          " OreBlockSize,NewOreReserveCategory,RemainingOreBlockvalues,  \r\n" +
                                          " RockEngReportNumber,MissfiresChecked,BarricadedChecked,  \r\n" +
                                          " ReclaimedChecked,PnlSweptChecked,StrikeGullyChecked,  \r\n" +
                                          " StopeChecked,CleanedChecked,AllStuffChecked, \r\n" +
                                          " SuppAndSafeGN,TonsGN,ReclamationGN)" +

                                          " values('" + IDtxt.Text + "', \r\n" +

                                          "  '" + ReasonPnlStoppedTxt.Text + "', '" + SupFromFacePermTxt.Text + "', '" + TypeSup1Txt.Text + "',  \r\n" +
                                          " '" + SupFromFaceTempTxt.Text + "', '" + TypeSup2Txt.Text + "','" + TopPegTxt.Text + "', \r\n" +
                                          " '" + DistFaceTopTxt.Text + "', '" + BotPegTxt.Text + "','" + DistFaceBotTxt.Text + "', \r\n" +
                                          " '" + FaceTxt.Text + "', '" + FaceMTxt.Text + "','" + GullyTxt.Text + "', \r\n" +
                                          " '" + GullyMTxt.Text + "', '" + RseTxt.Text + "','" + RseMTxt.Text + "', \r\n" +
                                          " '" + ReSweepsTxt.Text + "', '" + ReSweepsMTxt.Text + "','" + CurOreResCatTxt.Text + "', \r\n" +
                                          " '" + OreBlockSizeTxt.Text + "', '" + NewOreResCattxt.Text + "','" + OreBlockvaluesTxt.Text + "', \r\n" +
                                          " '" + RockEngRepNoTxt.Text + "', '" + MissfiresChecked + "','" + BarricadedChecked + "', \r\n" +
                                          " '" + ReclaimedChecked + "', '" + PnlSweptChecked + "','" + StrikeGullyChecked + "', \r\n" +
                                          " '" + StopeChecked + "', '" + CleanedChecked + "','" + AllStuffChecked + "',  \r\n" +
                                          " '" + SuppAndSafeGN.Text + "', '" + TonsGN.Text + "','" + ReclamationGN.Text + "' )";


                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();

                LoadStopPanelReportNote();

                Service.LoadNotes();


                return;
            }

            if (noteType == "CVN")
            {
                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;
                if (SectionCmb.Text == string.Empty)
                {
                    MO = string.Empty;
                }
                else
                {
                    MO = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text);
                }

                if (SectionManCmb.Text == string.Empty)
                {
                    SecMan = string.Empty;
                }
                else
                {
                    SecMan = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionManCmb.Text);
                }

                if (SBCmb.Text == string.Empty)
                {
                    SB = string.Empty;
                }
                else
                {
                    SB = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text);
                }

                if (StopRemark.Substring(0, 1) == "+")
                {
                    StopRemark = StopRemark.Substring(1, StopRemark.Length - 1);
                }


                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSave.SqlStatement = "Delete from  tbl_SurveyNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "Delete from  tbl_SurveyNote_CoverNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID, \r\n" +
                                           " TheDate,MOSection, \r\n" +
                                           "UserName,IsCompleted,ImageType,IsDeleted,IsLatest,ImageDate,  \r\n" +
                                           " SecManager  )" +

                                           " values('" + IDtxt.Text + "','" + noteType + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(WPList.Text.ToString()) + "','" + SB + "', \r\n" +
                                           " '" + date.Value + "','" + MO + "', \r\n" +
                                           " '" + TUserInfo.UserID + "','N','" + ImageType + "','N','" + Latest + "', '" + ImageDate.Text + "', \r\n" +
                                           " '" + SecMan + "') \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote_CoverNote(TSNID,\r\n" +

                                          " AtPegNum, AtPegNumVal,   \r\n" +
                                          "AtPegNum2, AtPegNumVal2, \r\n" +
                                          "StopFace,  StopFace2 ,   \r\n" +
                                          "Scale)  \r\n" +


                                          " values('" + IDtxt.Text + "', \r\n" +

                                          "   '" + CurrPegtxt.Text + "', '" + CurrPegValuetxt.Text + "',  \r\n" +
                                          "  '" + WorkFacePegtxt.Text + "','" + WorkFacePegValuetxt.Text + "',  \r\n" +
                                          "  '" + DrillCubbyPegtxt.Text + "','" + DrillCubbyPegValuetxt.Text + "',    \r\n" +
                                          "'" + CoverScaletxt.Text + "')";

                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();

                LoadCoverNote();

                Service.LoadNotes();


                return;
            }

            if (noteType == "SSN")
            {
                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;
                if (SectionCmb.Text == string.Empty)
                {
                    MO = string.Empty;
                }
                else
                {
                    MO = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text);
                }

                if (SectionManCmb.Text == string.Empty)
                {
                    SecMan = string.Empty;
                }
                else
                {
                    SecMan = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionManCmb.Text);
                }

                if (SBCmb.Text == string.Empty)
                {
                    SB = string.Empty;
                }
                else
                {
                    SB = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text);
                }

                if (StopRemark.Substring(0, 1) == "+")
                {
                    StopRemark = StopRemark.Substring(1, StopRemark.Length - 1);
                }

                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSave.SqlStatement = "Delete from  tbl_SurveyNote where TSNID = '" + IDtxt.Text + "'";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "Delete from  tbl_SurveyNote_StoppingNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID, \r\n" +
                                           " TheDate,MOSection, \r\n" +
                                           "UserName,IsCompleted,ImageType,IsDeleted,IsLatest,ImageDate,  \r\n" +
                                           " SecManager  )" +
                                           " values('" + IDtxt.Text + "','" + noteType + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(WPList.Text.ToString()) + "','" + SB + "', \r\n" +
                                           " '" + date.Value + "','" + MO + "', \r\n" +
                                           " '" + TUserInfo.UserID + "','N','" + ImageType + "','N','" + Latest + "', '" + ImageDate.Text + "', \r\n" +
                                           " '" + SecMan + "') \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote_StoppingNote(TSNID,\r\n" +
                                           " StopTheFollowingface1,StopTheFollowingfacePeg1,StopTheFollowingfaceValue1,   \r\n" +
                                           " StopTheFollowingface2,StopTheFollowingfacePeg2,StopTheFollowingfaceValue2,  \r\n" +
                                           " Scale)" +
                                           " values('" + IDtxt.Text + "',\r\n" +
                                           "  '" + StopFacetxt.Text + "', '" + PegNumtxt.Text + "', '" + PegNumValuetxt.Text + "',  \r\n" +
                                           " '" + StopFace2txt.Text + "', '" + PegNum2txt.Text + "','" + PegNumValue2txt.Text + "', \r\n" +
                                           "'" + StoppingScaletxt.Text + "')";

                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();

                LoadStoppingNote();
                Service.LoadNotes();
                return;
            }

            if (noteType == "STN")
            {
                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;
                if (SectionCmb.Text == string.Empty)
                {
                    MO = string.Empty;
                }
                else
                {
                    MO = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text);
                }

                if (SectionManCmb.Text == string.Empty)
                {
                    SecMan = string.Empty;
                }
                else
                {
                    SecMan = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionManCmb.Text);
                }

                if (SBCmb.Text == string.Empty)
                {
                    SB = string.Empty;
                }
                else
                {
                    SB = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text);
                }

                if (StopRemark.Substring(0, 1) == "+")
                {
                    StopRemark = StopRemark.Substring(1, StopRemark.Length - 1);
                }


                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSave.SqlStatement = "Delete from  tbl_SurveyNote where TSNID = '" + IDtxt.Text + "'  \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "Delete from  tbl_SurveyNote_StopingNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID, \r\n" +
                                           " TheDate,MOSection, \r\n" +
                                           "UserName,IsCompleted,ImageType,IsDeleted,IsLatest,ImageDate,  \r\n" +
                                           " SecManager  )" +

                                           " values('" + IDtxt.Text + "','" + noteType + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(WPList.Text.ToString()) + "','" + SB + "', \r\n" +
                                           " '" + date.Value + "','" + MO + "' ,\r\n" +
                                           " '" + TUserInfo.UserID + "','N','" + ImageType + "','N','" + Latest + "', '" + ImageDate.Text + "', \r\n" +
                                           " '" + SecMan + "') \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote_StopingNote(TSNID, \r\n" +

                                           " SNOldPeg,SNToNewPeg,SNNewPegValue,   \r\n" +
                                           " SNNewPeg,FLP,SNFLPValue,  \r\n" +

                                           " AsgOldPeg,AsgToNewPeg,AsgValue,   \r\n" +
                                           " AsgNewPeg,AsgFLP,AsgFLPValue  \r\n" +

                                           " ,Scale)" +
                                           " values('" + IDtxt.Text + "',\r\n" +

                                           "  '" + SNTopOldPeg.Text + "', '" + SNTopToNewPeg.Text + "', '" + SNTopValue.Text + "',        \r\n" +
                                           " '" + SNTopNewPeg.Text + "', '" + SNTopFLP.Text + "','" + SNTopFLPValue.Text + "', \r\n" +

                                           "  '" + SNAsgOldPeg.Text + "', '" + SNAsgToNewPeg.Text + "', '" + SNAsgValue.Text + "',        \r\n" +
                                           " '" + SNAsgNewPeg.Text + "', '" + SNAsgFLP.Text + "','" + SNAsgFLPValue.Text + "', \r\n" +
                                           "'" + SNScale.Text + "')";

                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();

                LoadStopingNote();
                Service.LoadNotes();
                return;
            }

            if (noteType == "DN")
            {
                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;

                if (SectionCmb.Text == string.Empty)
                {
                    MO = string.Empty;
                }
                else
                {
                    MO = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text);
                }

                if (SectionManCmb.Text == string.Empty)
                {
                    SecMan = string.Empty;
                }
                else
                {
                    SecMan = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionManCmb.Text);
                }

                if (SBCmb.Text == string.Empty)
                {
                    SB = string.Empty;
                }
                else
                {
                    SB = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text);
                }

                if (StopRemark.Substring(0, 1) == "+")
                {
                    StopRemark = StopRemark.Substring(1, StopRemark.Length - 1);
                }

                ////Get Workplaceid
                MWDataManager.clsDataAccess _dbManWPID = new MWDataManager.clsDataAccess();
                _dbManWPID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManWPID.SqlStatement = "select * from vw_Workplace_Total where Description = '" + WPList.Text.ToString() + "'  \r\n\r\n";
                _dbManWPID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPID.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPID.ExecuteInstruction();
                string WPID = "";
                if (_dbManWPID.ResultsDataTable.Rows.Count > 0)
                {
                    WPID = _dbManWPID.ResultsDataTable.Rows[0][0].ToString();
                }
                else
                {
                    return;
                }





                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSave.SqlStatement = "Delete from  tbl_SurveyNote where TSNID = '" + IDtxt.Text + "'  \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "Delete from  tbl_SurveyNote_DevNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID, \r\n" +
                                           " TheDate,MOSection, \r\n" +
                                           "UserName,IsCompleted,ImageType,IsDeleted,IsLatest,ImageDate,  \r\n" +
                                           " SecManager  )" +

                                           " values('" + IDtxt.Text + "','" + noteType + "','" + WPID + "','" + SB + "', \r\n" +
                                           " '" + date.Value + "','" + MO + "', \r\n" +
                                           " '" + TUserInfo.UserID + "','N','" + ImageType + "','N','" + Latest + "', '" + ImageDate.Text + "', \r\n" +
                                           " '" + SecMan + "') \r\n\r\n" +

                                           "insert into  tbl_SurveyNote_DevNote(TSNID, \r\n" +

                                           " OldPeg,ToNewPeg,NewPegValue,   \r\n" +
                                           " NewPeg,ToFLP,FLPValue,  \r\n" +
                                           " SurveyLineToLHS,SurveyLineRHS,GradeLineToHW,GradeLineFW,   \r\n" +
                                           " RailElevationOldPeg,RailElevationOldPegValue1,RailElevationOldPegValue2,  \r\n" +
                                           " RailElevationNewPeg,RailElevationNewPegValue1,RailElevationNewPegValue2,  \r\n" +
                                           " EndInCoverTillPeg,EndInCoverTillPegValue,HolingDistPeg,  \r\n" +
                                           " HolingDistPegValue,StopingDistPeg,StopingDistPegValue,  \r\n" +
                                           " GradeAt,BackChain,BackChainValue,  \r\n" +
                                           " FrontChain,FrontChainValue,Bgp,  \r\n" +
                                           " BgpValue,Fgp,FgpValue  \r\n" +

                                           " ,Scale)" +
                                           " values('" + IDtxt.Text + "', \r\n" +

                                           "  '" + PegNoFrom1Cmb.Text + "', '" + PegTo1cmb.Text + "', '" + PegDistTxt.Text + "', \r\n" +
                                           " '" + PegFrom2cmb.Text + "', '" + PanelTypetxt.Text + "','" + PegFaceTxt.Text + "', \r\n" +
                                           "  '" + CarryLHSTxt.Text + "', '" + CarryRHStxt.Text + "', '" + CarryHWTxt.Text + "', '" + CarryFWtxt.Text + "', \r\n" +
                                           " '" + SGradesPegTxt1.Text + "', '" + SGradesPegTxt1Add.Text + "','" + CarryRailTxt.Text + "', \r\n" +
                                           " '" + SGradesPegTxt2.Text + "', '" + PegToReqRailTxt.Text + "','" + RailTxt.Text + "', \r\n" +
                                           " '" + EndInCoverTillPegCmb.Text + "', '" + EndInCoverTillPegTxt.Text + "','" + HolDistPegCmb.Text + "', \r\n" +
                                           " '" + HolingDistPegTxt.Text + "', '" + StopDistPegCmb.Text + "','" + StopDistPegTxt.Text + "', \r\n" +
                                           " '" + GradeTxt.Text + "', '" + BackChainTxt.Text + "','" + BackChainValueTxt.Text + "', \r\n" +
                                           " '" + FrontChainTxt.Text + "', '" + FrontChainValueTxt.Text + "','" + BgpTxt.Text + "', \r\n" +
                                           " '" + BGPTxt2.Text + "', '" + FgpTxt.Text + "','" + FGPTxt2.Text + "', \r\n" +
                                           "'" + ScaleTxt.Text + "')";


                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();

                LoadDevNote();

                return;
            }

            if (noteType == "SRN")
            {
                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;
                if (SectionCmb.Text == string.Empty)
                {
                    MO = string.Empty;
                }
                else
                {
                    MO = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text);
                }

                if (SectionManCmb.Text == string.Empty)
                {
                    SecMan = string.Empty;
                }
                else
                {
                    SecMan = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionManCmb.Text);
                }

                if (SBCmb.Text == string.Empty)
                {
                    SB = string.Empty;
                }
                else
                {
                    SB = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text);
                }

                if (StopRemark.Substring(0, 1) == "+")
                {
                    StopRemark = StopRemark.Substring(1, StopRemark.Length - 1);
                }


                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSave.SqlStatement = "Delete from  tbl_SurveyNote where TSNID = '" + IDtxt.Text + "'  \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "Delete from  tbl_SurveyNote_StartNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID, \r\n" +
                                           " TheDate,MOSection ,\r\n" +
                                           "UserName,IsCompleted,ImageType,IsDeleted,IsLatest,ImageDate,  \r\n" +
                                           " SecManager  )" +

                                           " values('" + IDtxt.Text + "','" + noteType + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(WPList.Text.ToString()) + "','" + SB + "', \r\n" +
                                           " '" + date.Value + "','" + MO + "' ,\r\n" +
                                           " '" + TUserInfo.UserID + "','N','" + ImageType + "','N','" + Latest + "', '" + ImageDate.Text + "', \r\n" +
                                           " '" + SecMan + "') \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote_StartNote(TSNID,\r\n" +

                                           " NameAndDescriptionOfFace,NameAndDescriptionOfFacePeg,NameAndDescriptionOfFacePegValue,   \r\n" +
                                           " HolingNoteOf,ReceivedBackBySurvey,HoleSafelyInto,HolingNoteRefNo  \r\n" +

                                           " ,Scale)" +
                                           " values('" + IDtxt.Text + "', \r\n" +

                                           "  '" + NameOfFaceTxt.Text + "', '" + NameOfFacePeg.Text + "', '" + NameOfFaceTxt2.Text + "', \r\n" +
                                           " '" + holingNoteTxt.Text + "', '" + SurveyTxt.Text + "','" + SafelyIntoTxt.Text + "', '" + HolingNoteRefNo.Text + "', \r\n" +

                                           "'" + SNScaleTxt.Text + "')";



                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();

                LoadStartNote();


                return;
            }

            if (noteType == "OCN")
            {
                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;
                if (SectionCmb.Text == string.Empty)
                {
                    MO = string.Empty;
                }
                else
                {
                    MO = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text);
                }

                if (SectionManCmb.Text == string.Empty)
                {
                    SecMan = string.Empty;
                }
                else
                {
                    SecMan = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionManCmb.Text);
                }

                if (SBCmb.Text == string.Empty)
                {
                    SB = string.Empty;
                }
                else
                {
                    SB = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text);
                }

                if (StopRemark.Substring(0, 1) == "+")
                {
                    StopRemark = StopRemark.Substring(1, StopRemark.Length - 1);
                }


                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSave.SqlStatement = "Delete from  tbl_SurveyNote where TSNID = '" + IDtxt.Text + "'  \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "Delete from  tbl_SurveyNote_OpenCastSurveyNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID, \r\n" +
                                           " TheDate,MOSection, \r\n" +
                                           "UserName,IsCompleted,ImageType,IsDeleted,IsLatest,ImageDate,  \r\n" +
                                           " SecManager  )" +

                                           " values('" + IDtxt.Text + "','" + noteType + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(WPList.Text.ToString()) + "','" + SB + "', \r\n" +
                                           " '" + date.Value + "','" + MO + "', \r\n" +
                                           " '" + TUserInfo.UserID + "','N','" + ImageType + "','N','" + Latest + "', '" + ImageDate.Text + "', \r\n" +
                                           " '" + SecMan + "') \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote_OpenCastSurveyNote(TSNID, \r\n" +

                                           " Scale)" +
                                           " values('" + IDtxt.Text + "' ,\r\n" +

                                           "'" + OpenCastScaleTxt.Text + "')";



                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();

                LoadOpenCastSurveyNote();


                return;
            }

            if (noteType == "HNN")
            {
                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;
                if (SectionCmb.Text == string.Empty)
                {
                    MO = string.Empty;
                }
                else
                {
                    MO = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text);
                }

                if (SectionManCmb.Text == string.Empty)
                {
                    SecMan = string.Empty;
                }
                else
                {
                    SecMan = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionManCmb.Text);
                }

                if (SBCmb.Text == string.Empty)
                {
                    SB = string.Empty;
                }
                else
                {
                    SB = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text);
                }

                if (StopRemark.Substring(0, 1) == "+")
                {
                    StopRemark = StopRemark.Substring(1, StopRemark.Length - 1);
                }


                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSave.SqlStatement = "Delete from  tbl_SurveyNote where TSNID = '" + IDtxt.Text + "'  \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "Delete from  tbl_SurveyNote_HolingNotificationNote where TSNID = '" + IDtxt.Text + "' \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID, \r\n" +
                                           " TheDate,MOSection ,\r\n" +
                                           "UserName,IsCompleted,ImageType,IsDeleted,IsLatest,ImageDate,  \r\n" +
                                           " SecManager  )" +

                                           " values('" + IDtxt.Text + "','" + noteType + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(WPList.Text.ToString()) + "','" + SB + "', \r\n" +
                                           " '" + date.Value + "','" + MO + "', \r\n" +
                                           " '" + TUserInfo.UserID + "','N','" + ImageType + "','N','" + Latest + "', '" + ImageDate.Text + "', \r\n" +
                                           " '" + SecMan + "') \r\n\r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote_HolingNotificationNote(TSNID,\r\n" +

                                           " FollowingFaceA,FaceHoleInto,FollowingFaceB, \r\n" +
                                           " FaceMinedCloseTo,DistanceBetweenTwoEnds,StopFollowingFace1, \r\n" +
                                           " StopFollowingFacePeg1,StopFollowingFacePegValue1,StopFollowingFace2, \r\n" +
                                           " StopFollowingFacePeg2,StopFollowingFacePegValue2,FollowingEndPnl, \r\n" +
                                           " MentionedEndPnl \r\n" +

                                           " ,Scale)" +
                                           " values('" + IDtxt.Text + "', \r\n" +

                                           "  '" + FacesAtxt.Text + "', '" + HoleIntoTxt.Text + "', '" + FacesBtxt.Text + "', \r\n" +
                                           " '" + MinesCloseToTxt.Text + "', '" + DistanceTxt.Text + "','" + StopTheFollowingFaceTxt.Text + "', \r\n" +
                                           "  '" + FollowingFacePegTxt.Text + "', '" + FollowingFaceTxt.Text + "', '" + StopTheFollowingFace2Txt.Text + "', \r\n" +
                                           " '" + FollowingFacePegTxt2.Text + "', '" + FollowingFaceTxt2.Text + "','" + FollowingEndPnlTxt.Text + "', \r\n" +
                                           " '" + MentionedEndPnlTxt.Text + "', \r\n" +
                                           "'" + HolNoteScaleTxt.Text + "')";

                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();

                LoadHolingNotificationNote();


                return;
            }


            if (noteType == "CN")
            {

                if (IDtxt.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter an ID for the Cubby ", "Missing ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (PegNoFrom1Cmb.Text == string.Empty)
                {
                    MessageBox.Show("Please select a Peg from the Peg list ", "Missing Peg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (noteType == "SN")
            {
                TotPegValue = 0; //Convert.ToDecimal(ProductionGlobal.ProductionGlobal.ExtractAfterColon(PegNoFrom1Cmb.Text).ToString()) + Convert.ToDecimal(PegDistTxt.Text);
                MWDataManager.clsDataAccess _dbManCheck = new MWDataManager.clsDataAccess();
                _dbManCheck.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManCheck.SqlStatement = "Select Workplaceid from  tbl_SurveyNote where WorkplaceID = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(AdvWorkplaceCmb.Text) + "' and NoteType = 'SN'";


                _dbManCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCheck.queryReturnType = MWDataManager.ReturnType.DataTable;

                _dbManCheck.ExecuteInstruction();

                if (_dbManCheck.ResultsDataTable.Rows.Count > 0)
                {
                    MWDataManager.clsDataAccess _dbManCheck2 = new MWDataManager.clsDataAccess();
                    _dbManCheck2.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                    _dbManCheck2.SqlStatement = "Select MAX(TSNID) from  tbl_SurveyNote where WorkplaceID = '" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(AdvWorkplaceCmb.Text) + "'";


                    _dbManCheck2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManCheck2.queryReturnType = MWDataManager.ReturnType.DataTable;

                    _dbManCheck2.ExecuteInstruction();
                    string id = _dbManCheck2.ResultsDataTable.Rows[0][0].ToString();

                    if (Convert.ToInt32(id) < Convert.ToInt32(IDtxt.Text))
                    {
                        Latest = "Y";

                        MWDataManager.clsDataAccess _dbManUpdate = new MWDataManager.clsDataAccess();
                        _dbManUpdate.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                        _dbManUpdate.SqlStatement = "Update  tbl_SurveyNote Set IsLatest = 'N' where TSNID = '" + id + "'";


                        _dbManUpdate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbManUpdate.queryReturnType = MWDataManager.ReturnType.DataTable;

                        _dbManUpdate.ExecuteInstruction();

                    }
                    else
                    {
                        Latest = "N";
                    }
                }
                else
                {
                    Latest = "Y";
                }

                MWDataManager.clsDataAccess _dbManSaveStop = new MWDataManager.clsDataAccess();
                _dbManSaveStop.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSaveStop.SqlStatement = "Delete from  tbl_SurveyNote where TSNID = '" + IDtxt.Text + "'";

                _dbManSaveStop.SqlStatement = _dbManSaveStop.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID,TheDate,StopRemark,GenRemark,PegNo,PegValue,PegDist,UserName,IsCompleted,Length,Width,Height,IsPeg,ImageType,TotPegValue,IsDeleted,IsLatest,ImageDate)  " +
                                           // "SecManager, ToPeg2,ToPeg2Val,Workplaceid2,MOSection) " +

                                           "values('" + IDtxt.Text + "','" + noteType + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(AdvWorkplaceCmb.Text) + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text) + "', " +
                                           " '" + date.Value + "', '" + StopRemark + "', '" + RemarkTxt.Text + "', '" + AdvWPPegcmb.Text + "',  " +
                                           "'0', '" + AdvWPPegValtxt.Text + "', '" + TUserInfo.Name + "','N','" + LengthTxt.Text + "','" + DepthTxt.Text + "','" + HeightTxt.Text + "','" + Peg + "','" + ImageType + "', '" + TotPegValue + "','N','" + Latest + "', '" + ImageDate.Text + "') ";


                _dbManSaveStop.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSaveStop.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSaveStop.ExecuteInstruction();
            }
            else
            {
                return;

                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;
                if (SectionCmb.Text == string.Empty)
                {
                    MO = string.Empty;
                }
                else
                {
                    MO = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text);
                }

                if (SectionManCmb.Text == string.Empty)
                {
                    SecMan = string.Empty;
                }
                else
                {
                    SecMan = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionManCmb.Text);
                }

                if (SBCmb.Text == string.Empty)
                {
                    SB = string.Empty;
                }
                else
                {
                    SB = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SBCmb.Text);
                }



                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManSave.SqlStatement = "Delete from  tbl_SurveyNote where TSNID = '" + IDtxt.Text + "'";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + "insert into  tbl_SurveyNote(TSNID,NoteType,WorkplaceID,SectionID,TheDate,StopRemark,GenRemark,PegNo,PegValue,PegDist,UserName,IsCompleted,Length,Width,Height,IsPeg,ImageType,TotPegValue,IsDeleted,IsLatest,ImageDate,  \r\n" +
                                           " SecManager, ToPeg2,PegtoPegVal,PegtoFLPVal,LHS,RHS,HW,FW,CarrRailVal,PegatChain,PegatChainVal,ChainatLP, \r\n" +
                                           " LPVal,PegReqRail,ReqRailVal,RailVal,PegSGradeVal1,PegSGradeVal2,PegSGradeVal3,PegSGradeVal4,PegStopDist,PegStopDistVal, \r\n" +
                                           " PegVal,ToPeg2Face,AdvToDate,PegHoldDist,PegHoldDistVal,Grade,PanelType,MOSection, HolingNoteNo, Notes, SBDate, MODate, UMDate, SBName, MOName, UMName \r\n" +
                                           ",EndInCoverTillPeg,EndInCoverTillPegValue,BackChain,FrontChain \r\n" +
                                           ",Bgp,BgpValue,Fgp,FgpValue,Scale)" +
                                           " values('" + IDtxt.Text + "','" + noteType + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(WPList.Text.ToString()) + "','" + SB + "', \r\n" +
                                           " '" + date.Value + "', '" + StopRemark + "', '" + RemarkTxt.Text + "', '" + PegNoFrom1Cmb.Text + "',  \r\n" +
                                           " '0', '" + PegDistTxt.Text + "', '" + TUserInfo.Name + "','N','" + LengthTxt.Text + "','" + DepthTxt.Text + "','" + HeightTxt.Text + "','" + Peg + "','" + ImageType + "', '" + TotPegValue + "','N','" + Latest + "', '" + ImageDate.Text + "', \r\n" +
                                           " '" + SecMan + "', '" + PegTo1cmb.Text + "','" + PegDistTxt.Text + "', \r\n" +
                                           " '" + PegFaceTxt.Text + "', '" + CarryLHSTxt.Text + "', '" + CarryRHStxt.Text + "', '" + CarryHWTxt.Text + "', '" + CarryFWtxt.Text + "','" + CarryRailTxt.Text + "','" + ChainAtPegCmb.Text + "', \r\n" +
                                           " '" + ChainAtPegTxt.Text + "', '" + ChainTxt.Text + "', '" + LPTxt.Text + "', '" + PegToReqRailCmb.Text + "', '" + PegToReqRailTxt.Text + "', \r\n" +
                                           " '" + RailTxt.Text + "', '" + SGradesPegTxt1.Text + "', '" + SGradesPegTxt1Add.Text + "', '" + SGradesPegTxt2.Text + "', '" + SGradesPegTxt2Add.Text + "','" + StopDistPegCmb.Text + "', \r\n" +
                                           " '" + StopDistPegTxt.Text + "', '" + PegValTxt.Text + "','" + PegToFaceTxt.Text + "','" + AdvToDateTxt.Text + "', '" + HolDistPegCmb.Text + "', \r\n" +
                                           " '" + HolingDistPegTxt.Text + "', '" + GradeTxt.Text + "','" + PanelTypetxt.Text + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(SectionCmb.Text) + "','" + HolingNoteNoTxt.Text + "','" + NotesTxt.Text + "', null, null, null, null, null, null  \r\n" +
                                           ",'" + EndInCoverTillPegCmb.Text + "','" + EndInCoverTillPegTxt.Text + "','" + BackChainTxt.Text + "','" + FrontChainTxt.Text + "' \r\n" +
                                           ",'" + BgpTxt.Text + "','" + BGPTxt2.Text + "','" + FgpTxt.Text + "','" + FGPTxt2.Text + "','" + ScaleTxt.Text + "')";


                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();
            }

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (WPList.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Workplace", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SectionCmb.SelectedIndex == -1 || SectionManCmb.SelectedIndex == -1 || SBCmb.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a Section Manager, Section and a ShiftBoss", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StopRemark = PegNoFrom1Cmb.Text + "+" + PegDistTxt.Text;
            SaveCubby();
        }

        private void PegNoFrom1Cmb_TextChanged(object sender, EventArgs e)
        {
            if (PegNoFrom1Cmb.SelectedIndex > 0)
            {
                if (noteType == "SN")
                {
                    Leave.Text = "1";
                    LoadStopeNote();

                }
                else
                {
                    if (ChartType == "EmptyCubby")
                    {
                        Peg = "Y";
                        LoadEmptyCubby();
                    }

                    if (ChartType == "NormalCubbyLeft")
                    {
                        Peg = "Y";
                        loadCubbyLeft();
                    }
                    if (ChartType == "NormalCubbyRight")
                    {
                        Peg = "Y";
                        LoadCubbyRight();
                    }
                    if (ChartType == "DrillLeft")
                    {
                        Peg = "Y";
                        LoadDrillRigLeft();
                    }
                    if (ChartType == "DrilRight")
                    {
                        Peg = "Y";
                        LoadDrillRigRight();
                    }
                    if (ChartType == "CubbyLeft")
                    {
                        Peg = "Y";
                        LoadCubbyL();
                    }
                    if (ChartType == "CubbyRight")
                    {
                        Peg = "Y";
                        LoadCubbyRightSide();
                    }
                }
            }
        }

        private void PegDistTxt_Leave(object sender, EventArgs e)
        {
            if (Leave.Text == "0")
            {
                if (noteType == "SN")
                {
                    Leave.Text = "1";
                    LoadStopeNote();
                    RemarkTxt.Focus();

                }
            }
        }

        private void PegDistTxt_Enter(object sender, EventArgs e)
        {
            Leave.Text = "0";
        }

        private void SectionCmb_Enter(object sender, EventArgs e)
        {
            Leave.Text = "0";
        }

        private void WorkplaceCmb_Enter(object sender, EventArgs e)
        {
            Leave.Text = "0";
        }

        private void PegNoFrom1Cmb_Enter(object sender, EventArgs e)
        {
            Leave.Text = "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CaptureScreenShot();
            SaveScreenShot("MyControlShot.png", ImageFormat.Png);

        }

        private void SaveScreenShot(string filename, ImageFormat format)
        {
            Bitmap screenShot = CaptureScreenShot();
        }

        private Bitmap CaptureScreenShot()
        {
            // get the bounding area of the screen containing (0,0)
            // remember in a multidisplay environment you don't know which display holds this point
            Rectangle bounds = Screen.GetBounds(Point.Empty);

            // create the bitmap to copy the screen shot to
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

            // now copy the screen image to the graphics device from the bitmap
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            return bitmap;
        }

        public static Form IsGetImageOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }

        private void PegTo1cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            PegFrom2cmb.Text = PegTo1cmb.Text;
            PegValueCmb.Text = PegTo1cmb.Text;
            PegToFaceCmb.Text = PegTo1cmb.Text;
        }


        string MyFilter = string.Empty;

        private void Searchtxt_TextChanged(object sender, EventArgs e)
        {

            MyFilter = Searchtxt.Text;
            bs1.Filter = string.Format(" [Description] LIKE '%{0}%'", MyFilter);

            WPList.DataSource = bs1;
            WPList.DisplayMember = "Description";

        }

        private void WPList_SelectedIndexChanged(object sender, EventArgs e)
        {

            loadSections();

            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbManWP.SqlStatement = "select Distinct(s2.Sectionid) MOID,s2.Name MOName,s1.Sectionid SBID,s1.Name SBName,S3.Sectionid ManID,s3.Name ManName";
            _dbManWP.SqlStatement = _dbManWP.SqlStatement + " from tbl_Planmonth p, tbl_Section s,tbl_Section s1, tbl_Section s2,tbl_Section s3,tbl_Workplace w ";
            _dbManWP.SqlStatement = _dbManWP.SqlStatement + " where w.description = '" + WPList.SelectedItem.ToString() + "' and w.Workplaceid = p.Workplaceid and p.prodmonth = '" + ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth + "'  and p.prodmonth = s.prodmonth and p.sectionid = s.Sectionid  ";
            _dbManWP.SqlStatement = _dbManWP.SqlStatement + " and s.Prodmonth = s1.Prodmonth and s.ReportToSectionid = s1.Sectionid  and s1.Prodmonth = s2.Prodmonth ";
            _dbManWP.SqlStatement = _dbManWP.SqlStatement + " and s1.ReporttoSectionid = s2.Sectionid and s2.Prodmonth = s3.Prodmonth and s2.ReporttoSectionid = s3.Sectionid ";
            _dbManWP.SqlStatement = _dbManWP.SqlStatement + " and p.activity = '1'";




            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            DataTable dtWorkplace = _dbManWP.ResultsDataTable;
            //int x = 0;


            foreach (DataRow dr in dtWorkplace.Rows)
            {
                SectionManCmb.Text = dr["ManID"].ToString() + ":" + dr["ManName"].ToString();
                SectionCmb.Text = dr["MOID"].ToString() + ":" + dr["MOName"].ToString();
                SBCmb.Text = dr["SBID"].ToString() + ":" + dr["SBName"].ToString();


            }


            if (noteType == "CN")
            {

                /////////////////////Load Pegs////////////////////////////
                PegNoFrom1Cmb.Items.Clear();
                PegTo1cmb.Items.Clear();
                PegFrom2cmb.Items.Clear();
                ChainAtPegCmb.Items.Clear();
                PegToReqRailCmb.Items.Clear();
                StopDistPegCmb.Items.Clear();
                PegValueCmb.Items.Clear();
                PegToFaceCmb.Items.Clear();
                HolDistPegCmb.Items.Clear();
                MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
                _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManPeg.SqlStatement = "Select * from tbl_Peg p, tbl_workplace w where p.workplaceid = w.workplaceid and  description = '" + WPList.SelectedItem.ToString() + "' " +
                                         " order by value desc, Pegid ";


                _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPeg.ExecuteInstruction();

                DataTable dtPeg = _dbManPeg.ResultsDataTable;
                //int x = 0;


                foreach (DataRow dr in dtPeg.Rows)
                {
                    PegNoFrom1Cmb.Items.Add(dr["PegID"].ToString()); //+ ":" + dr["Value"].ToString());
                    PegTo1cmb.Items.Add(dr["PegID"].ToString());
                    PegFrom2cmb.Items.Add(dr["PegID"].ToString());
                    ChainAtPegCmb.Items.Add(dr["PegID"].ToString());
                    PegToReqRailCmb.Items.Add(dr["PegID"].ToString());
                    StopDistPegCmb.Items.Add(dr["PegID"].ToString());
                    PegValueCmb.Items.Add(dr["PegID"].ToString());
                    PegToFaceCmb.Items.Add(dr["PegID"].ToString());
                    HolDistPegCmb.Items.Add(dr["PegID"].ToString());

                }
                PegNoFrom1Cmb.Items.Add("Start:0.0");
                PegTo1cmb.Items.Add("Start:0.0");
                PegFrom2cmb.Items.Add("Start:0.0");
                ChainAtPegCmb.Items.Add("Start:0.0");
                PegToReqRailCmb.Items.Add("Start:0.0");
                StopDistPegCmb.Items.Add("Start:0.0");
                PegValueCmb.Items.Add("Start:0.0");
                PegToFaceCmb.Items.Add("Start:0.0");
                HolDistPegCmb.Items.Add("Start:0.0");
                PegNoFrom1Cmb.SelectedIndex = 0;
            }

            if (noteType == "SN")
            {
                AdvWorkplaceCmb.Text = WPList.SelectedItem.ToString();

                string WP = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(AdvWorkplaceCmb.Text);
                AdvWPPegcmb.Items.Clear();

                MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
                _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManPeg.SqlStatement = "Select * from Peg where workplaceid = '" + WP + "' " +
                                         " order by value desc, Pegid ";


                _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPeg.ExecuteInstruction();

                DataTable dtPeg = _dbManPeg.ResultsDataTable;

                foreach (DataRow dr in dtPeg.Rows)
                {
                    AdvWPPegcmb.Items.Add(dr["PegID"].ToString());


                }
                AdvWPPegcmb.Items.Add("Start:0.0");

                AdvWPPegcmb.SelectedIndex = 0;
            }

            if (noteType == "DN")
            {

                /////////////////////Load Pegs////////////////////////////
                PegNoFrom1Cmb.Items.Clear();
                PegTo1cmb.Items.Clear();
                PegFrom2cmb.Items.Clear();
                ChainAtPegCmb.Items.Clear();
                PegToReqRailCmb.Items.Clear();
                StopDistPegCmb.Items.Clear();
                PegValueCmb.Items.Clear();
                PegToFaceCmb.Items.Clear();
                HolDistPegCmb.Items.Clear();
                MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
                _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManPeg.SqlStatement = "Select * from tbl_Peg p, tbl_workplace w where p.workplaceid = w.workplaceid and  description = '" + WPList.SelectedItem.ToString() + "' " +
                                         " order by value desc, Pegid ";


                _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPeg.ExecuteInstruction();

                DataTable dtPeg = _dbManPeg.ResultsDataTable;
                //int x = 0;


                foreach (DataRow dr in dtPeg.Rows)
                {
                    PegNoFrom1Cmb.Items.Add(dr["PegID"].ToString());
                    PegTo1cmb.Items.Add(dr["PegID"].ToString());
                    PegFrom2cmb.Items.Add(dr["PegID"].ToString());
                    ChainAtPegCmb.Items.Add(dr["PegID"].ToString());
                    PegToReqRailCmb.Items.Add(dr["PegID"].ToString());
                    StopDistPegCmb.Items.Add(dr["PegID"].ToString());
                    PegValueCmb.Items.Add(dr["PegID"].ToString());
                    PegToFaceCmb.Items.Add(dr["PegID"].ToString());
                    HolDistPegCmb.Items.Add(dr["PegID"].ToString());

                }
                PegNoFrom1Cmb.Items.Add("Start:0.0");
                PegTo1cmb.Items.Add("Start:0.0");
                PegFrom2cmb.Items.Add("Start:0.0");
                ChainAtPegCmb.Items.Add("Start:0.0");
                PegToReqRailCmb.Items.Add("Start:0.0");
                StopDistPegCmb.Items.Add("Start:0.0");
                PegValueCmb.Items.Add("Start:0.0");
                PegToFaceCmb.Items.Add("Start:0.0");
                HolDistPegCmb.Items.Add("Start:0.0");
                PegNoFrom1Cmb.SelectedIndex = 0;
            }

            if (noteType == "STN")
            {

                /////////////////////Load Pegs////////////////////////////
                SNTopOldPeg.Items.Clear();
                SNTopToNewPeg.Items.Clear();
                SNTopNewPeg.Items.Clear();

                SNAsgOldPeg.Items.Clear();
                SNAsgToNewPeg.Items.Clear();
                SNAsgNewPeg.Items.Clear();


                MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
                _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManPeg.SqlStatement = "Select * from tbl_Peg p, tbl_workplace w where p.workplaceid = w.workplaceid and  description = '" + WPList.SelectedItem.ToString() + "' " +
                                         " order by value desc, Pegid ";

                _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPeg.ExecuteInstruction();

                DataTable dtPeg = _dbManPeg.ResultsDataTable;


                foreach (DataRow dr in dtPeg.Rows)
                {
                    SNTopOldPeg.Items.Add(dr["PegID"].ToString());
                    SNTopToNewPeg.Items.Add(dr["PegID"].ToString());
                    SNTopNewPeg.Items.Add(dr["PegID"].ToString());
                    SNAsgOldPeg.Items.Add(dr["PegID"].ToString());
                    SNAsgToNewPeg.Items.Add(dr["PegID"].ToString());
                    SNAsgNewPeg.Items.Add(dr["PegID"].ToString());

                }
                SNTopOldPeg.Items.Add("Start:0.0");
                SNTopToNewPeg.Items.Add("Start:0.0");
                SNTopNewPeg.Items.Add("Start:0.0");
                SNAsgOldPeg.Items.Add("Start:0.0");
                SNAsgToNewPeg.Items.Add("Start:0.0");
                SNAsgNewPeg.Items.Add("Start:0.0");

                SNTopOldPeg.SelectedIndex = 0;

            }

            if (noteType == "SSN")
            {

                /////////////////////Load Pegs////////////////////////////
                PegNumtxt.Items.Clear();
                PegNum2txt.Items.Clear();


                MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
                _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManPeg.SqlStatement = "Select * from tbl_Peg p, tbl_workplace w where p.workplaceid = w.workplaceid and  description = '" + WPList.SelectedItem.ToString() + "' " +
                                         " order by value desc, Pegid ";


                _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPeg.ExecuteInstruction();

                DataTable dtPeg = _dbManPeg.ResultsDataTable;


                foreach (DataRow dr in dtPeg.Rows)
                {
                    PegNumtxt.Items.Add(dr["PegID"].ToString());
                    PegNum2txt.Items.Add(dr["PegID"].ToString());

                }
                PegNumtxt.Items.Add("Start:0.0");
                PegNum2txt.Items.Add("Start:0.0");
            }

            if (noteType == "SSN")
            {

                /////////////////////Load Pegs////////////////////////////
                CurrPegtxt.Items.Clear();
                WorkFacePegtxt.Items.Clear();
                DrillCubbyPegtxt.Items.Clear();


                MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
                _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManPeg.SqlStatement = "Select * from tbl_Peg p, tbl_workplace w where p.workplaceid = w.workplaceid and  description = '" + WPList.SelectedItem.ToString() + "' " +
                                         " order by value desc, Pegid ";


                _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPeg.ExecuteInstruction();

                DataTable dtPeg = _dbManPeg.ResultsDataTable;


                foreach (DataRow dr in dtPeg.Rows)
                {
                    CurrPegtxt.Items.Add(dr["PegID"].ToString());
                    WorkFacePegtxt.Items.Add(dr["PegID"].ToString());
                    DrillCubbyPegtxt.Items.Add(dr["PegID"].ToString());
                }
                CurrPegtxt.Items.Add("Start:0.0");
                WorkFacePegtxt.Items.Add("Start:0.0");
                DrillCubbyPegtxt.Items.Add("Start:0.0");
            }

            if (noteType == "HNN")
            {

                /////////////////////Load Pegs////////////////////////////

                FollowingFacePegTxt.Items.Clear();
                FollowingFacePegTxt2.Items.Clear();


                MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
                _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManPeg.SqlStatement = "Select * from tbl_Peg p, tbl_workplace w where p.workplaceid = w.workplaceid and  description = '" + WPList.SelectedItem.ToString() + "' " +
                                         " order by value desc, Pegid ";


                _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPeg.ExecuteInstruction();

                DataTable dtPeg = _dbManPeg.ResultsDataTable;
                //int x = 0;


                foreach (DataRow dr in dtPeg.Rows)
                {
                    FollowingFacePegTxt.Items.Add(dr["PegID"].ToString());
                    FollowingFacePegTxt2.Items.Add(dr["PegID"].ToString());

                }
                FollowingFacePegTxt.Items.Add("Start:0.0");
                FollowingFacePegTxt2.Items.Add("Start:0.0");

            }

            if (noteType == "OCN")
            {

                /////////////////////Load Pegs////////////////////////////

                MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
                _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManPeg.SqlStatement = "Select * from tbl_Peg p, tbl_workplace w where p.workplaceid = w.workplaceid and  description = '" + WPList.SelectedItem.ToString() + "' " +
                                         " order by value desc, Pegid ";


                _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPeg.ExecuteInstruction();

                DataTable dtPeg = _dbManPeg.ResultsDataTable;
                //int x = 0;


                foreach (DataRow dr in dtPeg.Rows)
                {
                    FollowingFacePegTxt.Items.Add(dr["PegID"].ToString());
                    FollowingFacePegTxt2.Items.Add(dr["PegID"].ToString());

                }

                FollowingFacePegTxt.Items.Add("Start:0.0");
                FollowingFacePegTxt2.Items.Add("Start:0.0");

            }
        }

        private void WP2List_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (noteType == "SN")
            {
                ExWpcmb.Text = WP2List.SelectedItem.ToString();

                string WP = ExWpcmb.Text;
                ExWpPegcmb.Items.Clear();

                MWDataManager.clsDataAccess _dbManPeg = new MWDataManager.clsDataAccess();
                _dbManPeg.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

                _dbManPeg.SqlStatement = "Select * from Peg where workplaceid = '" + WP + "' " +
                                         " order by value desc, Pegid ";


                _dbManPeg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPeg.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPeg.ExecuteInstruction();

                DataTable dtPeg = _dbManPeg.ResultsDataTable;
                //int x = 0;


                foreach (DataRow dr in dtPeg.Rows)
                {
                    ExWpPegcmb.Items.Add(dr["PegID"].ToString() + ":" + dr["Value"].ToString());


                }
                ExWpPegcmb.Items.Add("Start:0.0");

                ExWpPegcmb.SelectedIndex = 0;
            }
        }


        private void SNTopToNewPeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            SNTopNewPeg.Text = SNTopToNewPeg.Text;
        }

        private void SNAsgToNewPeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            SNAsgNewPeg.Text = SNAsgToNewPeg.Text;
        }

        private void StopPanelReportNote_Paint(object sender, PaintEventArgs e)
        {

        }

        private void WorkplacesChecked_CheckedChanged(object sender, EventArgs e)
        {
            if (WorkplacesChecked.Checked == true)
            {
                LoadAllWorkplaces();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (WPList.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Workplace", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (SectionCmb.SelectedIndex == -1 || SectionManCmb.SelectedIndex == -1 || SBCmb.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a Section Manager, Section and a ShiftBoss", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StopRemark = PegNoFrom1Cmb.Text + "+" + PegDistTxt.Text;
            SaveCubby();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            btnGetImage_Click(null, null);
        }

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            GetImage ImFrm = (GetImage)IsGetImageOpen(typeof(GetImage));
            if (ImFrm == null)
            {
                ImFrm = new GetImage(this);
                // RepFrm.Text = "Crew Ranking Report";
                ImFrm.OpenFormtxt.Text = "Cubby";
                ImFrm.Show();
            }
            else
            {
                ImFrm.WindowState = FormWindowState.Maximized;
                ImFrm.Select();
            }
        }

        private void getImagePnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gbDevelopmentNote_Enter(object sender, EventArgs e)
        {

        }

        private void label230_Click(object sender, EventArgs e)
        {

        }

        private void gbStartNote_Enter(object sender, EventArgs e)
        {

        }

        private void SNScaleTxt_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void HolingNoteRefNo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
