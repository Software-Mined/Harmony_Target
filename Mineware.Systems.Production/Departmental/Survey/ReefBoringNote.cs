using DevExpress.XtraEditors;
using FastReport;
using Mineware.Systems.GlobalConnect;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace Mineware.Systems.Production.Departmental.Survey
{
    public partial class ReefBoringNote : XtraForm
    {
        public ucSurveyNote Service;

        DataTable dtSections = new DataTable();
        DataTable dtWP = new DataTable();
        Report theReport = new Report();


        DialogResult result1;
        OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        string sourceFile;
        string destinationFile;
        string FileName = string.Empty;


        public ReefBoringNote()
        {
            InitializeComponent();
        }

        private void ReefBoringNote_Load(object sender, EventArgs e)
        {
            //this.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");

            //Load ID Txt
            MWDataManager.clsDataAccess _dbManID = new MWDataManager.clsDataAccess();
            _dbManID.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];

            _dbManID.SqlStatement = "Select MAX(TSNID) from tbl_ReefBoringLetters ";


            _dbManID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManID.queryReturnType = MWDataManager.ReturnType.DataTable;

            _dbManID.ExecuteInstruction();

            if (_dbManID.ResultsDataTable.Rows[0][0] == DBNull.Value)
            {
                _dbManID.ResultsDataTable.Rows[0][0] = "0";
            }

            IDtxt.Text = (Convert.ToInt32(_dbManID.ResultsDataTable.Rows[0][0].ToString()) + 1).ToString();

            ///Load Workplaces

            LoadAllWorkplaces();

            MWDataManager.clsDataAccess _dbInsertEmptyCubby = new MWDataManager.clsDataAccess();
            _dbInsertEmptyCubby.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];

            _dbInsertEmptyCubby.SqlStatement = "insert into tbl_ReefBoringLetters(TSNID,NoteType,WorkplaceID,TheDate,UserName,IsCompleted,Comments,Picture,IsDeleted,notes ) \r\n" +
                                           " values('" + IDtxt.Text + "','RB','None', " +
                                           " '" + date.Value + "', '" + TUserInfo.Name + "', 'N', 'none','none',  " +
                                           " 'N','" + txtNotes.Text + "' ) " +


                                           " INSERT INTO [dbo].[tbl_ReefBoringLetters_Autorisation] \r\n " +

                                           " values      ('" + IDtxt.Text + "', null, null, null, null, null, null, null, null, null, null \r\n " +
                                           " , null, null, null, null, null, null, null, null, null, null \r\n " +
                                           " , null, null, null, null, null, null, null, null, null, null \r\n " +
                                           " , null, null, null, null, null, null, null) ";


            _dbInsertEmptyCubby.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbInsertEmptyCubby.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbInsertEmptyCubby.ExecuteInstruction();



            LoadEmptyCubby();





            ImageDate.Text = Convert.ToInt64(DateTime.Now.ToBinary()).ToString();
        }


        public void LoadEmptyCubby()
        {

            MWDataManager.clsDataAccess _dbManEmptyCubby = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbManEmptyCubby.SqlStatement = " select a.*,  " +
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
                                       " ,case when us5.username is null then 'N' else 'Y' end as auth " +

                                       "  \r\n " +
                                       "  from (  \r\n " +


                                       " select  TSNID ID, NoteType NoteType, WorkplaceID WP, null MOSection,  TheDate Date, null StopRemark,   \r\n " +
                "  notes Remark, null PegFrom,  '0' a, null Peg, null Length, null Depth, null Height, '" + PicLbl.Text + "' ImageDate,    \r\n " +
                " null ManagerSection, null PegTo, null PegDist,  null PegFace, null LHS, null RHS, null HW, null FW,  \r\n " +
                "  null CarryRail, null ChainAtPeg,  null ChainAtPegVal, null Chain, null LP, null PegToRail,  \r\n " +
                " null PegToRailVal,  null Rail, null SGrade1, null SGrade2, null SGrade3,  \r\n " +
                " null SGrade4, null StopDistPeg,  null StopDistPegVal, null PegVal, null PegToFace,  \r\n " +
                "  null AdvToDate, null HolDistPeg,  null HoldingDistPegVal, null Grade, null PanelType, (select banner from dbo.SysSet) banner,  \r\n " +
                "  null HolingNoteNo, null Notes, WorkplaceID Description, null SID,null SName,null S1ID,null S1Name,null S2ID,null S2Name, username   , WorkplaceID, TheDate    \r\n " +

                                       " from  dbo.[tbl_ReefBoringLetters] t   \r\n " +

                                       "  where TSNID = '" + IDtxt.Text + "' \r\n " +





                                       " ) a \r\n " +
                                       " left outer join .[dbo].[TempStopNote_Autorisation] b on a.id = b.TSNID \r\n " +




                                        " left outer join [dbo].tbl_Users_Department_Survey me on b.lvl1user = me.complogin \r\n " +
                                         " left outer join [dbo].tbl_Users_Attachments s1 on me.userid = s1.userid \r\n " +

                                        "  left outer join [dbo].tbl_Users_Department_Survey me1 on b.lvl2user = me1.complogin  \r\n " +
                                        "  left outer join [dbo].tbl_Users_Attachments s2 on me1.userid = s2.userid \r\n " +

                                        "  left outer join [dbo].tbl_Users_Department_Survey me2 on b.lvl3user = me2.complogin   \r\n " +
                                        "  left outer join [dbo].tbl_Users_Attachments s3 on me2.userid = s3.userid \r\n " +

                                         " left outer join [dbo].tbl_Users_Department_Survey me3 on b.lvl4user = me3.complogin   \r\n " +
                                         " left outer join [dbo].tbl_Users_Attachments s4 on me3.userid = s4.userid \r\n " +

                                        "  left outer join [dbo].tbl_Users_Department_Survey me4 on b.lvl5user = me4.complogin  \r\n " +
                                         " left outer join [dbo].tbl_Users_Attachments s5 on me4.userid = s5.userid \r\n " +

                                        "  left outer join [dbo].tbl_Users_Department_Survey me5 on b.lvl6user = me5.complogin   \r\n " +
                                        "  left outer join [dbo].tbl_Users_Attachments s6 on me5.userid = s6.userid \r\n " +

                                         " left outer join [dbo].tbl_Users_Department_Survey me6 on b.lvl7user = me6.complogin   \r\n " +
                                         " left outer join [dbo].tbl_Users_Attachments s7 on me6.userid = s7.userid   \r\n " +

                                         " left outer join [dbo].tbl_Users_Department_Survey me7 on b.lvl8user = me7.complogin   \r\n " +
                                         " left outer join [dbo].tbl_Users_Attachments s8 on me7.userid = s8.userid \r\n " +

                                         " left outer join [dbo].tbl_Users_Department_Survey me8 on b.lvl9user = me8.complogin  \r\n " +
                                         " left outer join [dbo].tbl_Users_Attachments s9 on me8.userid = s9.userid \r\n " +

                                         " left outer join [dbo].tbl_Users_Department_Survey me9 on b.lvl8user = me9.complogin  \r\n " +
                                         " left outer join [dbo].tbl_Users_Attachments s10 on me9.userid = s10.userid \r\n " +

                                         " left outer join [dbo].tbl_Users_Department_Survey me10 on b.lvl8user = me10.complogin  \r\n " +
                                         " left outer join [dbo].tbl_Users_Attachments s11 on a.username = s11.userid \r\n " +




                                         " left outer join [dbo].[tbl_Users] us1 on b.lvl1user = us1.userid \r\n " +
                                         " left outer join [dbo].[tbl_Users] us2 on b.lvl2user = us2.userid \r\n " +
                                         " left outer join [dbo].[tbl_Users] us3 on b.lvl3user = us3.userid \r\n " +
                                         " left outer join [dbo].[tbl_Users] us4 on b.lvl4user = us4.userid \r\n " +
                                         " left outer join [dbo].[tbl_Users] us5 on b.lvl5user = us5.userid \r\n " +
                                         " left outer join [dbo].[tbl_Users] us6 on b.lvl6user = us6.userid \r\n " +
                                         " left outer join [dbo].[tbl_Users] us7 on b.lvl7user = us7.userid \r\n " +
                                         " left outer join [dbo].[tbl_Users] us8 on b.lvl8user = us8.userid \r\n " +
                                         " left outer join [dbo].[tbl_Users] us9 on a.username = us9.userid \r\n " +



                                         "  ";


            _dbManEmptyCubby.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby.queryReturnType = MWDataManager.ReturnType.DataTable;

            _dbManEmptyCubby.ResultsTableName = "EmptyCubby";
            _dbManEmptyCubby.ExecuteInstruction();




            DataSet EmptyDs = new DataSet();

            EmptyDs.Tables.Add(_dbManEmptyCubby.ResultsDataTable);

            theReport.RegisterData(EmptyDs);



            theReport.Load("ReefboringNote2.frx");


            //theReport.Design();


            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();


        }



        void LoadAllWorkplaces()
        {
            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbManWP.SqlStatement = "select workplacemain from  ATIC_CADSPLAN where workplacemain not in ( select workplaceid from tbl_ReefBoringLetters)   group by workplacemain " +
                                    "order by workplacemain ";
            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            dtWP = _dbManWP.ResultsDataTable;

            foreach (DataRow wp in dtWP.Rows)
            {
                WPList.Items.Add(wp["workplacemain"].ToString() /* + ":" + wp["Description"].ToString() */);
                //WP2List.Items.Add(wp["Workplaceid"].ToString() + ":" + wp["Description"].ToString());
            }
        }

        private void Searchtxt_TextChanged(object sender, EventArgs e)
        {
            //string zzzz = "*" + Searchtxt.Text;



            //WPList.Items.Clear();

            //foreach (DataRowView r in ProductionGlobal.ProductionGlobal.Search(dtWP, zzzz))
            //{
            //    WPList.Items.Add(r["workplacemain"].ToString() /* + ":" + r["Description"].ToString() */);

            //}
        }

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            
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

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            //StopRemark = PegNoFrom1Cmb.Text + "+" + PegDistTxt.Text;
            SaveCubby();
            // Close();
            //EditCubby();
            LoadEmptyCubby();
        }

        void SaveCubby()
        {
            string Latest = string.Empty;

            if (WPList.Text == string.Empty)
            {
                MessageBox.Show("Please select a workplace");

            }
            else
            {



                Latest = string.Empty;
                string MO = string.Empty;
                string SecMan = string.Empty;
                string SB = string.Empty;
                //if (SectionCmb.Text == "")
                //{
                //    MO = "";
                //}
                //else
                //{
                //    MO = procs.ExtractBeforeColon(SectionCmb.Text);
                //}

                //if (SectionManCmb.Text == "")
                //{
                //    SecMan = "";
                //}
                //else
                //{
                //    SecMan = procs.ExtractBeforeColon(SectionManCmb.Text);
                //}

                //if (SBCmb.Text == "")
                //{
                //    SB = "";
                //}
                //else
                //{
                //    SB = procs.ExtractBeforeColon(SBCmb.Text);
                //}

                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];

                _dbManSave.SqlStatement = "Delete from tbl_ReefBoringLetters where TSNID = '" + IDtxt.Text + "' Delete from tbl_ReefBoringLetters_Autorisation where TSNID = '" + IDtxt.Text + "'  \r\n";

                _dbManSave.SqlStatement = _dbManSave.SqlStatement + " insert into tbl_ReefBoringLetters(TSNID,NoteType,WorkplaceID,TheDate,UserName,IsCompleted,Comments,Picture,IsDeleted,notes ) \r\n" +
                                           " values('" + IDtxt.Text + "','RB','" + WPList.SelectedItem.ToString() + "', " +
                                           " '" + date.Value + "', '" + TUserInfo.UserID + "', 'N', 'Comments','" + PicLbl.Text + "',  " +
                                           " 'N','" + txtNotes.Text + "' )" +






                                               " INSERT INTO [dbo].[tbl_ReefBoringLetters_Autorisation] \r\n " +

                                               " values      ('" + IDtxt.Text + "', 'Active', null, null, null, null, null, null, null, null, null \r\n " +
                                               " , null, null, null, null, null, null, null, null, null, null \r\n " +
                                               " , null, null, null, null, null, null, null, null, null, null \r\n " +
                                               " , null, null, null, null, null, null, null) ";





                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();








                // MWDataManager.clsDataAccess _dbSaveAtho = new MWDataManager.clsDataAccess();
                //_dbManSave.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];

                //_dbManSave.SqlStatement = "Delete from tbl_ReefBoringLetters where TSNID = '" + IDtxt.Text + "'  \r\n";

                //_dbManSave.SqlStatement = _dbManSave.SqlStatement + " insert into tbl_ReefBoringLetters(TSNID,NoteType,WorkplaceID,TheDate,UserName,IsCompleted,Comments,Picture,IsDeleted,notes ) \r\n" +
                //                           " values('" + IDtxt.Text + "','RB','" + WPList.SelectedItem.ToString() + "', " +
                //                           " '" + date.Value + "', '" + clsUserInfo.UserName + "', 'N', 'Comments','Picture',  " +
                //                           " 'N','" + txtNotes.Text + "' )";


                //_dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbManSave.ExecuteInstruction();




                Service.LoadNotes2();

                //MessageFrm MsgFrm = new MessageFrm();
                //MsgFrm.Text = "Records Inserted";
                //Procedures.MsgText = "Records Added";
                //MsgFrm.Show();
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PicLbl_TextChanged(object sender, EventArgs e)
        {
            LoadEmptyCubby();
        }

        private void ReefBoringNote_FormClosed(object sender, FormClosedEventArgs e)
        {
            MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
            _dbManSave.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbManSave.SqlStatement = "delete from tbl_ReefBoringLetters_Autorisation where tsnid in (  \r\n" +
                                      "select tsnid from tbl_ReefBoringLetters where workplaceid = 'None')  \r\n" +


                                      "delete from tbl_ReefBoringLetters where tsnid in (  \r\n" +
                                      "select tsnid from tbl_ReefBoringLetters where workplaceid = 'None')  \r\n";
            _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSave.ExecuteInstruction();

            Service.LoadNotes2();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GetImage ImFrm = (GetImage)IsGetImageOpen(typeof(GetImage));
            if (ImFrm == null)
            {
                ImFrm = new GetImage(this);
                // RepFrm.Text = "Crew Ranking Report";
                ImFrm.OpenFormtxt.Text = "ReefBoring";
                ImFrm.Show();
            }
            else
            {
                ImFrm.WindowState = FormWindowState.Maximized;
                ImFrm.Select();
            }
        }
    }
}
