using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views.Grid;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionHelp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Survey
{
    public partial class ucSurveyNote : BaseUserControl
    {
        public DataSet dsGlobal = new DataSet();
        private DataTable dtActions = new DataTable("dtActions");

        //Action Variables
        string ActionType;
        string ActionDescription;

        private String ID = string.Empty;
        private String Workplace = string.Empty;
        private String Description = string.Empty;
        private String Recomendation = string.Empty;
        private String Priority = string.Empty;
        private String TargetDate = string.Empty;
        private String RespPerson = string.Empty;
        private String Overseer = string.Empty;
        //Use this for Syncromine
        private void sqlConnector(string sqlQuery, string sqlTableIdentifier)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _sqlConnection.SqlStatement = sqlQuery;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ResultsTableName = sqlTableIdentifier;
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;


            if (!string.IsNullOrEmpty(sqlTableIdentifier))
            {
                for (int i = 0; i < dsGlobal.Tables.Count; i++)
                {
                    if (dsGlobal.Tables[i].TableName == sqlTableIdentifier)
                    {
                        dsGlobal.Tables[i].Clear();
                        dsGlobal.Tables[i].Merge(dtReceive);
                    }
                }
            }
        }

        //NOTE: Register all tables for sqlconnector to use
        private void tableRegister()
        {
            dsGlobal.Tables.Add(dtSummaryDetail);
        }

        #region Datatables
        private DataTable dtSummaryDetail = new DataTable("dtSummaryDetail");
        #endregion

        public ucSurveyNote()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpSurveyNotes);
            FormActiveRibbonPage = rpSurveyNotes;
            FormMainRibbonPage = rpSurveyNotes;
            RibbonControl = RCSurveyNotes;
        }

        StringBuilder SQL = new StringBuilder();

        string TypeLabel = string.Empty;
        string GridSelectLabel = string.Empty;

        public static string _theSystemDBTag;
        public static string _UserCurrentInfo;

        string _userCurrentInfoConnection = string.Empty;

        DialogResult result1;
        OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        string sourceFile;
        string destinationFile;

        string NoteNo = string.Empty;
        string Declined = string.Empty;

        //string OutputDirectory;
        string FileName = string.Empty;
        List<string> listWP = new List<string>();  //for WP
        List<string> listWP2 = new List<string>(); //for WP

        Report theReport = new Report();
        Report theReportGeoSum = new Report();
        Report theReport1 = new Report();
        Report theReport11 = new Report();
        Report WPNewRep = new Report();
        Report MorningReport = new Report();
        Report RockSumReport = new Report();

        Report MorningReportReturn = new Report();

        Report SiesRep = new Report();

        DataTable dtPeg = new DataTable();
        DataTable dtCubby = new DataTable();
        DataTable dtSampling = new DataTable();
        BindingSource bs1 = new BindingSource();
        DataTable Survey1 = new DataTable();
        DataTable Survey2 = new DataTable();
        DataTable dtWP = new DataTable();
        BindingSource bsa = new BindingSource();

        public string WpNo = string.Empty;
        public string WpDesc = string.Empty;
        public string Date = string.Empty;
        public string SW = string.Empty;
        public string CorrCut = string.Empty;
        public string StdSw = string.Empty;
        public string Grade = string.Empty;
        public string Density = string.Empty;
        public string EditClicked = string.Empty;
        public string PegID = string.Empty;
        public string FormText = string.Empty;
        public string HangWall = string.Empty;
        public string FootWall = string.Empty;
        int x = 0;


        //Cubby Note values
        public string CubbyID;
        public string Section;
        public string TheDate;
        public string PegNum;
        public string PegDist;
        public string Depth;
        public string Length;
        //public string Width;
        public string Remark;
        public string Image;

        DialogResult result;

        //////FOR EDIT PURPOSES//////
        public string PegNo = string.Empty;
        public string PegValue = string.Empty;
        public string Letter1 = string.Empty;
        public string Letter2 = string.Empty;
        public string Letter3 = string.Empty;

        bool keyPress;

        DataTable dtMain = new DataTable();
        DataTable dtSummary = new DataTable();
        //DataTable dtSummaryDetail = new DataTable();

        DataSet dsSummary = new DataSet();

        string isloaded;
        public string MoringRepSec = string.Empty;
        public string MoringRepWP = string.Empty;
        public string MoringRepSeis = string.Empty;
        public string MoringRepRisk = string.Empty;


        string s;
        string newwp = string.Empty;


        private void ucSurveyNote_Load(object sender, EventArgs e)
        {
            _userCurrentInfoConnection = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            LoadNotes();
            LoadActions();
        }

        public static Form IsCubbyStopAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }

        private Rectangle Transform(Graphics g, int degree, Rectangle r)
        {
            g.TranslateTransform(r.Width / 2, r.Height / 2);
            g.RotateTransform(degree);
            float cos = (float)Math.Round(Math.Cos(degree * (Math.PI / 180)), 2);
            float sin = (float)Math.Round(Math.Sin(degree * (Math.PI / 180)), 2);
            Rectangle r1 = r;
            r1.X = (int)(r.X * cos + r.Y * sin);

            r1.X = -89;
            r1.Y = (int)(r.X * (-sin) + r.Y * cos);
            r1.Y = r1.Y - 8;
            return r1;
        }

        public void LoadNotes()
        {
            dataGridView1.Dispose();

            DataGridView dt1 = new DataGridView();
            dataGridView1 = dt1;
            dataGridView1.CellDoubleClick += dataGridView2_CellDoubleClick;
            dataGridView1.CellClick += dataGridView2_CellClick;

            dataGridView1.Parent = panel1;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.ColumnCount = 10;

            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[0].Width = 40;

            dataGridView1.Columns[1].HeaderText = "Note Type";
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[1].Visible = false;

            dataGridView1.Columns[2].HeaderText = "Workplace";
            dataGridView1.Columns[2].Width = 170;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[3].HeaderText = "Section ID";
            dataGridView1.Columns[3].Width = 50;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[4].HeaderText = "The Date";
            dataGridView1.Columns[4].Width = 80;
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[5].HeaderText = "Stop Remark";
            dataGridView1.Columns[5].Width = 80;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Columns[6].HeaderText = "User";
            dataGridView1.Columns[6].Width = 70;

            dataGridView1.Columns[7].HeaderText = "SB Acc Date";
            dataGridView1.Columns[7].Width = 80;
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[8].HeaderText = "MO Acc Date";
            dataGridView1.Columns[8].Width = 80;
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[9].HeaderText = "Gen. Remark";
            dataGridView1.Columns[9].Width = 300;
            dataGridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;




            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            MWDataManager.clsDataAccess _dbManInsert = new MWDataManager.clsDataAccess();
            _dbManInsert.ConnectionString = _userCurrentInfoConnection;
            _dbManInsert.SqlStatement = "insert into  [tbl_SurveyNote_Autorisation] " +


                                    "select [TSNID], 'Not Authorised', null, null, null, null, null, null, null, null, null, null " +
                                    ", null, null, null, null, null, null, null, null, null, null " +
                                    ", null, null, null, null, null, null, null, null, null, null, null,null,null,null,null,null " +
                                    "   from  tbl_SurveyNote where [TSNID] not in  " +
                                    "(select [TSNID] from  [tbl_SurveyNote_Autorisation])  ";
            _dbManInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert.ExecuteInstruction();

            string mine = string.Empty;


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = _userCurrentInfoConnection;
            _dbMan.SqlStatement = "Select convert(varchar(10),a.tsnid) Note, a.*, b.Description, CONVERT(VARCHAR(11),thedate,106) UseDate, 'No' aa, \r\n" +


                                     "case when lvl7ok is not null and status <> 'Declined' then 'Authorised' \r\n" +
                                     "when status = 'Declined' then 'Declined' else 'Still Active' end as status \r\n" +




                                     //z.status "+
                                     ", isnull(lvl1ok, 'No') Lvl1, \r\n" +

                                     "case  " +
                                     "when lvl2ok is null and status = 'Declined' then 'Dec' \r\n" +
                                     "when lvl2ok is null  and lvl1ok is not null  and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl2ok is null and status <> 'Declined' then 'No' \r\n" +


                                     "else lvl2ok end as Lvl2, \r\n" +

                                     "case  " +
                                     "when lvl3ok is null and status = 'Declined' then 'Dec' \r\n" +
                                     "when lvl3ok is null and lvl2ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl3ok is null and status <> 'Declined' then 'No' \r\n" +
                                     "else lvl3ok end as Lvl3, " +

                                     "case  " +
                                     "when lvl4ok is null and status = 'Declined' then 'Dec' \r\n" +
                                     "when lvl4ok is null and lvl3ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl4ok is null and status <> 'Declined' then 'No' \r\n" +
                                     "else lvl4ok end as Lvl4, \r\n" +


                                     "case  " +
                                     "when lvl5ok is null and status = 'Declined' then 'Dec' \r\n" +
                                     "when lvl5ok is null and lvl4ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl5ok is null and status <> 'Declined' then 'No' \r\n" +
                                     "else lvl5ok end as Lvl5, \r\n" +

                                     "case  " +
                                     "when lvl6ok is null and status = 'Declined' then 'Dec' \r\n" +
                                     "when lvl6ok is null and lvl5ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl6ok is null and status <> 'Declined' then 'No' \r\n" +
                                     "else lvl6ok end as Lvl6, \r\n" +

                                     "case  " +
                                     "when lvl7ok is null and status = 'Declined' then 'Dec' \r\n" +
                                     "when lvl7ok is null and lvl6ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl7ok is null and status <> 'Declined' then 'No' \r\n" +
                                     "else lvl7ok end as Lvl7, \r\n" +

                                     "case  " +
                                     "when lvl8ok is null and status = 'Declined' then 'Dec' \r\n" +
                                     "when lvl8ok is null and lvl7ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl8ok is null and status <> 'Declined' then 'No' \r\n" +
                                     "else lvl8ok end as Lvl8, \r\n" +

                                     "case  " +
                                     "when lvl9ok is null and status = 'Declined' then 'Dec' \r\n" +
                                     "when lvl9ok is null and lvl8ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl9ok is null and status <> 'Declined' then 'No' \r\n" +
                                     "else lvl9ok end as Lvl9, \r\n" +

                                     " 'No' SB, 'No' MO ," +

                                     "datediff(day,thedate,getdate()) datediff1, case when lvl7date is not null then convert(varchar(10),datediff(day,thedate,lvl7date) ) " +
                                    "when lvl7date is null and status <> 'Declined' then convert(varchar(10),datediff(day,thedate,getdate())) " +
                                    "when status = 'Declined' then '****' " +

                                    " else null end as FinalDays " +

                                     "from  tbl_SurveyNote a, vw_Workplace_Total b,  [tbl_SurveyNote_Autorisation] z  where  a.[TSNID] = z.[TSNID] and  " +
                                    "a.IsCompleted = 'N' and " +
                                    "a.WorkplaceID COLLATE Latin1_General_CI_AS = convert(varchar(10),b.WorkplaceID) and a.IsDeleted = 'N' " +
                                    "Order By a.TSNID ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            dtCubby = dt;

            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);

            gridControl1.DataSource = ds1.Tables[0];

            NoteCol.FieldName = "Note";
            StatCol.FieldName = "status";

            WorkplaceCol.FieldName = "Description";
            SectionCol.FieldName = "SectionID";
            UserCol.FieldName = "UserName";
            DateCol.FieldName = "UseDate";
            NoteTypeCol.FieldName = "NoteType";

            Lvl1Col.FieldName = "Lvl1";
            Lvl2Col.FieldName = "Lvl2";
            Lvl3Col.FieldName = "Lvl3";
            Lvl4Col.FieldName = "Lvl4";
            Lvl5Col.FieldName = "Lvl5";
            Lvl6Col.FieldName = "Lvl6";
            Lvl7Col.FieldName = "Lvl7";
            Lvl8Col.FieldName = "Lvl8";
            Lvl10Col.FieldName = "Lvl9";

            Lvl9Col.FieldName = "SB";
            Lvl11Col.FieldName = "MO";
            Lvl11Col.Visible = false;

            NoteCol.Width = 30;

            StatCol.Width = 80;

            WorkplaceCol.Width = 100;
            SectionCol.Width = 50;

            UserCol.Width = 100;

            DateCol.Width = 80;

            DaysGoneCol.Width = 40;

            Lvl1Col.Width = 15;
            Lvl2Col.Width = 15;
            Lvl3Col.Width = 15;
            Lvl4Col.Width = 15;
            Lvl5Col.Width = 15;
            Lvl6Col.Width = 15;
            Lvl7Col.Width = 15;
            Lvl8Col.Width = 15;
            Lvl9Col.Width = 15;
            Lvl10Col.Width = 15;
            Lvl11Col.Width = 15;

            DaysGoneCol.FieldName = "FinalDays";

            //Do Drop down grid

            //Do stringbuilder 

            string SQL = string.Empty;
            SQL = string.Empty;

            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = _userCurrentInfoConnection;
            _dbMan2.SqlStatement = " select * from ( select  convert(varchar(10),a.tsnid) Note, '01 Created' Processcode, username nuser, 'Not Autorised' Status,  CONVERT(VARCHAR(11),thedate,106) TheDate, case when lvl1date is not null then null  else  datediff(dd,thedate,getdate()) end as  aa, '' notes  from  [tbl_SurveyNote]  a,   tbl_SurveyNote_Autorisation b where a.tsnid = b.tsnid \r\n " +
" union \r\n " +
" select  '" + mine + "'+convert(varchar(10),a.tsnid) Note, '02 Checked By' Processcode,   b.username nuser, case when lvl1ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
" end as Status,CONVERT(VARCHAR(11),lvl1date,106) lvl1date, case when lvl1date is null then null  else  datediff(dd,thedate,lvl1date) end as  aa, lvl1remks  from  [tbl_SurveyNote_Autorisation] a left outer join  [tbl_SurveyNote] c on a.tsnid = c.tsnid left outer join [dbo].tbl_Users_Department_Survey me on a.lvl1user = me.complogin left outer join tbl_Users b on b.userid = me.userid \r\n " +
" union \r\n " +
" select  '" + mine + "'+convert(varchar(10),a.tsnid) Note, '03 Snr Mine Surveyor' Processcode,   b.username nuser, case when lvl2ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
" end as Status,CONVERT(VARCHAR(11),lvl2date,106) lvl2date, case when lvl2date is null then null  else  datediff(dd,lvl1date,lvl2date) end as  aa, lvl2remks  " +
" from  [tbl_SurveyNote_Autorisation] a left outer join  [tbl_SurveyNote] c on a.tsnid = c.tsnid left outer join [dbo].tbl_Users_Department_Survey me on a.lvl2user = me.complogin left outer join tbl_Users b on b.userid = me.userid \r\n " +
" union \r\n " +
" select  '" + mine + "'+convert(varchar(10),a.tsnid) Note, '04 Geologist' Processcode,   b.username nuser, case when lvl3ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
" end as Status,CONVERT(VARCHAR(11),lvl3date,106) lvl3date,  case when lvl3date is null then null  else  datediff(dd,lvl2date,lvl3date) end as  aa,  lvl3remks " +
" from  [tbl_SurveyNote_Autorisation] a left outer join  [tbl_SurveyNote] c on a.tsnid = c.tsnid left outer join [dbo].tbl_Users_Department_Survey me on a.lvl3user = me.complogin left outer join tbl_Users b on b.userid = me.userid \r\n " +
" union \r\n " +
" select  '" + mine + "'+convert(varchar(10),a.tsnid) Note, '05 Vent. Dept.' Processcode,   b.username nuser, case when lvl4ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
" end as Status,CONVERT(VARCHAR(11),lvl4date,106) lvl4date,  case when lvl4date is null then null  else  datediff(dd,lvl3date,lvl4date) end as  aa,  lvl4remks " +
"from  [tbl_SurveyNote_Autorisation] a left outer join  [tbl_SurveyNote] c on a.tsnid = c.tsnid  left outer join [dbo].tbl_Users_Department_Survey me on a.lvl4user = me.complogin left outer join tbl_Users b on b.userid = me.userid \r\n " +
" union \r\n " +
" select  '" + mine + "'+convert(varchar(10),a.tsnid) Note, '06 Snr Mine Planner' Processcode,   b.username nuser, case when lvl5ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
" end as Status,CONVERT(VARCHAR(11),lvl5date,106) lvl5date,  case when lvl5date is null then null  else  datediff(dd,lvl4date,lvl5date) end as  aa,  lvl5remks  " +
" from  [tbl_SurveyNote_Autorisation] a left outer join  [tbl_SurveyNote] c on a.tsnid = c.tsnid  left outer join [dbo].tbl_Users_Department_Survey me on a.lvl5user = me.complogin left outer join tbl_Users b on b.userid = me.userid \r\n " +
" union \r\n " +
" select  '" + mine + "'+convert(varchar(10),a.tsnid) Note, '07 Mineoverseer' Processcode,   b.username nuser, case when lvl6ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
" end as Status,CONVERT(VARCHAR(11),lvl6date,106) lvl6date,  case when lvl6date is null then null  else  datediff(dd,lvl5date,lvl6date) end as  aa,  lvl6remks " +
" from  [tbl_SurveyNote_Autorisation] a left outer join  [tbl_SurveyNote] c on a.tsnid = c.tsnid  left outer join [dbo].tbl_Users_Department_Survey me on a.lvl6user = me.complogin left outer join tbl_Users b on b.userid = me.userid \r\n " +
" union \r\n " +
" select  '" + mine + "'+convert(varchar(10),a.tsnid) Note, '08 Sect. Mananger' Processcode,   b.username nuser, case when lvl7ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
" end as Status,CONVERT(VARCHAR(11),lvl7date,106) lvl7date,  case when lvl7date is null then null  else  datediff(dd,lvl6date,lvl7date) end as  aa,   lvl7remks " +
" from  [tbl_SurveyNote_Autorisation] a left outer join  [tbl_SurveyNote] c on a.tsnid = c.tsnid  left outer join [dbo].tbl_Users_Department_Survey me on a.lvl7user = me.complogin left outer join tbl_Users b on b.userid = me.userid \r\n " +
" union \r\n " +
" select  '" + mine + "'+convert(varchar(10),a.tsnid) Note, '09 Shift Boss Accepted' Processcode,   b.username nuser, case when lvl8ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
" end as Status,CONVERT(VARCHAR(11),lvl8date,106) lvl8date,  case when lvl8date is null then null  else  datediff(dd,lvl7date,lvl8date) end as  aa,  lvl8remks " +
" from  [tbl_SurveyNote_Autorisation] a left outer join  [tbl_SurveyNote] c on a.tsnid = c.tsnid  left outer join [dbo].tbl_Users_Department_Survey me on a.lvl8user = me.complogin left outer join tbl_Users b on b.userid = me.userid ) a order by note, Processcode \r\n ";

            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ExecuteInstruction();

            //sqlConnector(SQL, "dtSummaryDetail");

            DataTable dtSummaryDetail = _dbMan2.ResultsDataTable;
            ds1.Tables.Add(dtSummaryDetail);

            DataColumn keyColumn1 = ds1.Tables[0].Columns[0];
            DataColumn foreignKeyColumn1 = ds1.Tables[1].Columns[0];
            ds1.Relations.Add("CategoriesProducts", keyColumn1, foreignKeyColumn1);

            GridView cardView1 = new GridView(gridControl1);
            gridControl1.LevelTree.Nodes.Add("CategoriesProducts", cardView1);
            cardView1.ViewCaption = "Audit Trail";
            cardView1.PopulateColumns(ds1.Tables[1]);

            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit mEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            mEdit.MaxLength = 50000;
            gridControl1.RepositoryItems.Add(mEdit);
            cardView1.Columns[4].ColumnEdit = mEdit;
            cardView1.OptionsView.RowAutoHeight = true;
            cardView1.OptionsView.ShowIndicator = false;

            cardView1.OptionsView.ShowGroupPanel = false;
            cardView1.Columns[0].Visible = false;
            cardView1.Columns[1].Caption = "Process Code";
            cardView1.Columns[2].Caption = "User";
            cardView1.Columns[3].Caption = "Status";
            cardView1.Columns[3].Visible = false;


            cardView1.Columns[4].Caption = "Date";
            cardView1.Columns[5].Caption = "Days";
            cardView1.Columns[6].Caption = "Notes";

            cardView1.Columns[1].Width = 120;
            cardView1.Columns[2].Width = 200;
            cardView1.Columns[3].Width = 100;
            cardView1.Columns[4].Width = 80;

            cardView1.Columns[5].Width = 60;

            cardView1.Columns[6].Width = 600;


            cardView1.OptionsSelection.EnableAppearanceFocusedRow = false;
            cardView1.OptionsBehavior.Editable = false;

            cardView1.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

            cardView1.ColumnPanelRowHeight = 30;
            cardView1.OptionsView.RowAutoHeight = true;
            cardView1.OptionsView.ColumnAutoWidth = false;

            // End detail grid

            CriteriaOperator expr1 = new BinaryOperator("status", "Authorised");
            CriteriaOperator expr2 = new BinaryOperator("status", "Still Active");
            CriteriaOperator expr3 = new BinaryOperator("status", "Declined");

            bandedGridView1.ActiveFilterCriteria = GroupOperator.Or(new CriteriaOperator[] { expr1, expr2, expr3 });

        }

        public void LoadNotes2()
        {

            dataGridView1.Dispose();

            DataGridView dt1 = new DataGridView();
            dataGridView1 = dt1;
            dataGridView1.CellDoubleClick += dataGridView2_CellDoubleClick;
            dataGridView1.CellClick += dataGridView2_CellClick;

            dataGridView1.Parent = panel1;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.ColumnCount = 10;

            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[0].Width = 40;

            dataGridView1.Columns[1].HeaderText = "Note Type";
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[1].Visible = false;

            dataGridView1.Columns[2].HeaderText = "Workplace";
            dataGridView1.Columns[2].Width = 170;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[3].HeaderText = "Section ID";
            dataGridView1.Columns[3].Width = 50;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[4].HeaderText = "The Date";
            dataGridView1.Columns[4].Width = 80;
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[5].HeaderText = "Stop Remark";
            dataGridView1.Columns[5].Width = 80;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Columns[6].HeaderText = "User";
            dataGridView1.Columns[6].Width = 70;

            dataGridView1.Columns[7].HeaderText = "SB Acc Date";
            dataGridView1.Columns[7].Width = 80;
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[8].HeaderText = "MO Acc Date";
            dataGridView1.Columns[8].Width = 80;
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns[9].HeaderText = "Gen. Remark";
            dataGridView1.Columns[9].Width = 300;
            dataGridView1.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            MWDataManager.clsDataAccess _dbManInsert = new MWDataManager.clsDataAccess();
            _dbManInsert.ConnectionString = _userCurrentInfoConnection;
            _dbManInsert.SqlStatement = " delete from tbl_ReefBoringLetters where username is null " +

                                           "delete from [dbo].[tbl_ReefBoringLetters_Autorisation] " +
                                           "where status is null " +






                                    "insert into tbl_ReefBoringLetters_Autorisation " +


                                    "select [TSNID], 'Not Authorised', null, null, null, null, null, null, null, null, null, null " +
                                    ", null, null, null, null, null, null, null, null, null, null " +
                                    ", null, null, null, null, null, null, null, null, null, null, null,null,null,null,null,null " +
                                    "   from tbl_ReefBoringLetters where [TSNID] not in  " +
                                    "(select [TSNID] from [tbl_ReefBoringLetters_Autorisation])  ";
            _dbManInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert.ExecuteInstruction();


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = _userCurrentInfoConnection;
            _dbMan.SqlStatement = "Select a.*, CONVERT(VARCHAR(11),thedate,106) UseDate, 'No' aa, z.status " +
                                     ", isnull(lvl1ok, 'No') Lvl1, isnull(lvl2ok, 'No') Lvl2, isnull(lvl3ok, 'No') Lvl3 " +
                                     ", isnull(lvl4ok, 'No') Lvl4, isnull(lvl5ok, 'No') Lvl5, isnull(lvl6ok, 'No') Lvl6 " +
                                     ", isnull(lvl7ok, 'No') Lvl7, isnull(lvl8ok, 'No') Lvl8, isnull(lvl9ok, 'No') Lvl9, datediff(day,thedate,getdate()) datediff1 " +

                                     "from tbl_ReefBoringLetters a, [tbl_ReefBoringLetters_Autorisation] z  where  a.[TSNID] = z.[TSNID] and  " +
                                    "a.IsCompleted = 'N' and " +
                                    " a.IsDeleted = 'N' " +
                                    "Order By a.TSNID ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;
            dtCubby = dt;

            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);

            gridControl1.DataSource = ds1.Tables[0];

            NoteCol.FieldName = "TSNID";
            StatCol.FieldName = "status";

            WorkplaceCol.FieldName = "WorkplaceID";
            SectionCol.FieldName = "SectionID";
            UserCol.FieldName = "UserName";
            DateCol.FieldName = "UseDate";

            Lvl1Col.FieldName = "Lvl1";
            Lvl2Col.FieldName = "Lvl2";
            Lvl3Col.FieldName = "Lvl3";
            Lvl4Col.FieldName = "Lvl4";
            Lvl5Col.FieldName = "Lvl5";
            Lvl6Col.FieldName = "Lvl6";
            Lvl7Col.FieldName = "Lvl7";
            Lvl8Col.FieldName = "Lvl8";
            Lvl9Col.FieldName = "Lvl9";


            UserCol.Caption = "CreateNotes";

            Lvl1Col.Caption = " Section\n Manager";
            Lvl2Col.Caption = " Production\n Manager";
            Lvl3Col.Caption = " Geoscience\n Manager";
            Lvl4Col.Caption = " Survey Planning\n Manager";
            Lvl5Col.Caption = " MRM\n Manager";


            Lvl4Col.Visible = true;
            Lvl5Col.Visible = true;
            Lvl6Col.Visible = false;
            Lvl7Col.Visible = false;
            Lvl8Col.Visible = false;
            Lvl9Col.Visible = false;
            Lvl10Col.Visible = false;
            Lvl11Col.Visible = false;
            gridBand4.Visible = false;


            NoteCol.Width = 30;

            StatCol.Width = 80;

            WorkplaceCol.Width = 100;
            SectionCol.Width = 50;

            UserCol.Width = 100;

            DateCol.Width = 80;

            DaysGoneCol.Width = 40;

            Lvl1Col.Width = 15;
            Lvl2Col.Width = 15;
            Lvl3Col.Width = 15;
            Lvl4Col.Width = 15;
            Lvl5Col.Width = 15;
            Lvl6Col.Width = 15;
            Lvl7Col.Width = 15;
            Lvl8Col.Width = 15;
            Lvl9Col.Width = 15;
            Lvl10Col.Width = 15;
            Lvl11Col.Width = 15;

            DaysGoneCol.FieldName = "datediff1";

            //Do Drop down grid
            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = _userCurrentInfoConnection;
            _dbMan2.SqlStatement = " select  tsnid Note, '01 Created' Processcode, username nuser, 'Not Autorised' Status,  CONVERT(VARCHAR(11),thedate,106) TheDate,  notes  from [dbo].[tbl_ReefBoringLetters] \r\n " +
                    " union \r\n " +
                    " select  tsnid Note, '02 Checked By' Processcode,   username nuser, case when lvl1ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
                    " end as Status,CONVERT(VARCHAR(11),lvl1date,106) lvl1date,  lvl1remks  from [dbo].[tbl_ReefBoringLetters_Autorisation] a left outer join tbl_Users b on a.lvl1user = b.userid \r\n " +
                    " union \r\n " +
                    " select  tsnid Note, '03 Snr Mine Surveyor' Processcode,   username nuser, case when lvl2ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
                    " end as Status,CONVERT(VARCHAR(11),lvl2date,106) lvl2date,  lvl2remks  from [dbo].[tbl_ReefBoringLetters_Autorisation] a left outer join tbl_Users b on a.lvl2user = b.userid \r\n " +
                    " union \r\n " +
                    " select  tsnid Note, '04 Geologist' Processcode,   username nuser, case when lvl3ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
                    " end as Status,CONVERT(VARCHAR(11),lvl3date,106) lvl3date,  lvl3remks  from [dbo].[tbl_ReefBoringLetters_Autorisation] a left outer join tbl_Users b on a.lvl3user = b.userid \r\n " +
                    " union \r\n " +
                    " select  tsnid Note, '05 Snr Plan Manager' Processcode,   username nuser, case when lvl4ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
                    " end as Status,CONVERT(VARCHAR(11),lvl4date,106) lvl4date,  lvl4remks  from [dbo].[tbl_ReefBoringLetters_Autorisation] a left outer join tbl_Users b on a.lvl4user = b.userid \r\n " +
                    " union \r\n " +
                    " select  tsnid Note, '06 Snr Mine Planner' Processcode,   username nuser, case when lvl5ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
                    " end as Status,CONVERT(VARCHAR(11),lvl5date,106) lvl5date,  lvl5remks  from [dbo].[tbl_ReefBoringLetters_Autorisation] a left outer join tbl_Users b on a.lvl5user = b.userid \r\n " +
                    " union \r\n " +
                    " select  tsnid Note, '07 Sec Manager' Processcode,   username nuser, case when lvl6ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
                    " end as Status,CONVERT(VARCHAR(11),lvl6date,106) lvl6date,  lvl6remks  from [dbo].[tbl_ReefBoringLetters_Autorisation] a left outer join tbl_Users b on a.lvl6user = b.userid \r\n " +
                    " union \r\n " +
                    " select  tsnid Note, '08 Rock Mech' Processcode,   username nuser, case when lvl7ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
                    " end as Status,CONVERT(VARCHAR(11),lvl7date,106) lvl7date,  lvl7remks  from [dbo].[tbl_ReefBoringLetters_Autorisation] a left outer join tbl_Users b on a.lvl7user = b.userid \r\n " +
                    " union \r\n " +
                    " select  tsnid Note, '09 Mineoverseer' Processcode,   username nuser, case when lvl8ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
                    " end as Status,CONVERT(VARCHAR(11),lvl8date,106) lvl8date,  lvl8remks  from [dbo].[tbl_ReefBoringLetters_Autorisation] a left outer join tbl_Users b on a.lvl8user = b.userid \r\n " +
                    " union \r\n " +
                    " select  tsnid Note, '10 Shiftboss' Processcode,   username nuser, case when lvl9ok = 'Dec' then 'Declined' else 'Not Autorised' \r\n " +
                    " end as Status,CONVERT(VARCHAR(11),lvl9date,106) lvl9date,  lvl9remks  from [dbo].[tbl_ReefBoringLetters_Autorisation] a left outer join tbl_Users b on a.lvl9user = b.userid ";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ExecuteInstruction();
            DataTable dtSummaryDetail = _dbMan2.ResultsDataTable;
            ds1.Tables.Add(dtSummaryDetail);

            DataColumn keyColumn1 = ds1.Tables[0].Columns[0];
            DataColumn foreignKeyColumn1 = ds1.Tables[1].Columns[0];
            ds1.Relations.Add("CategoriesProducts", keyColumn1, foreignKeyColumn1);


            GridView cardView1 = new GridView(gridControl1);
            gridControl1.LevelTree.Nodes.Add("CategoriesProducts", cardView1);
            cardView1.ViewCaption = "Audit Trail";
            cardView1.PopulateColumns(ds1.Tables[1]);

            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit mEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            mEdit.MaxLength = 50000;
            gridControl1.RepositoryItems.Add(mEdit);
            cardView1.Columns[4].ColumnEdit = mEdit;
            cardView1.OptionsView.RowAutoHeight = true;
            cardView1.OptionsView.ShowIndicator = false;

            cardView1.OptionsView.ShowGroupPanel = false;
            cardView1.Columns[0].Visible = false;
            cardView1.Columns[1].Caption = "Process Code";
            cardView1.Columns[2].Caption = "User";
            cardView1.Columns[3].Caption = "Status";

            cardView1.Columns[4].Caption = "Date";
            cardView1.Columns[5].Caption = "Notes";


            cardView1.OptionsSelection.EnableAppearanceFocusedRow = false;
            cardView1.OptionsBehavior.Editable = false;

            cardView1.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

            cardView1.ColumnPanelRowHeight = 30;
            cardView1.OptionsView.RowAutoHeight = true;
            cardView1.OptionsView.ColumnAutoWidth = false;

            // End detail grid

            CriteriaOperator expr1 = new BinaryOperator("status", "Not Authorised");
            CriteriaOperator expr2 = new BinaryOperator("status", "Active");
            CriteriaOperator expr3 = new BinaryOperator("status", "Declined");

            bandedGridView1.ActiveFilterCriteria = GroupOperator.Or(new CriteriaOperator[] { expr1, expr2, expr3 });
        }

        private void bandedGridView1_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {

            if (e.Column != null)
            {
                if ((e.Column.Name == "Lvl1Col") || (e.Column.Name == "Lvl2Col") || (e.Column.Name == "Lvl3Col") || (e.Column.Name == "Lvl4Col") || (e.Column.Name == "Lvl5Col") || (e.Column.Name == "Lvl6Col")
                    || (e.Column.Name == "Lvl7Col") || (e.Column.Name == "Lvl8Col") || (e.Column.Name == "Lvl9Col") || (e.Column.Name == "Lvl10Col") || (e.Column.Name == "Lvl11Col"))
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);
                    e.Handled = true;
                }
            }
        }

        private void bandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            FPMessage FPMessagefrm = new FPMessage();
            FPMessagefrm.FPTypeLbl.Text = "Survey Note";
            FPMessagefrm.FPNoLbl.Text = NoteNo;
            FPMessagefrm.UserLbl.Text = UserCurrentInfo.UserID;
            FPMessagefrm.lblNotetype.Text = bandedGridView1.GetRowCellValue(bandedGridView1.FocusedRowHandle, bandedGridView1.Columns["NoteType"]).ToString();
            FPMessagefrm.lblDate.Text = bandedGridView1.GetRowCellValue(bandedGridView1.FocusedRowHandle, bandedGridView1.Columns["UseDate"]).ToString();
            FPMessagefrm.lblSection.Text = bandedGridView1.GetRowCellValue(bandedGridView1.FocusedRowHandle, bandedGridView1.Columns["SectionID"]).ToString();
            FPMessagefrm.lblSectionMan.Text = bandedGridView1.GetRowCellValue(bandedGridView1.FocusedRowHandle, bandedGridView1.Columns["MO"]).ToString();
            FPMessagefrm._theSystemDBTag = this.theSystemDBTag;
            FPMessagefrm._UserCurrentInfo = this.UserCurrentInfo.Connection;
            if (Declined == "Declined")
            {

                FPMessagefrm.AcceptBTN.Enabled = false;
                FPMessagefrm.DeclineBTN.Enabled = false;

            }
            FPMessagefrm.Show();
            LoadNotes();
        }

        private void bandedGridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            NoteNo = bandedGridView1.GetRowCellValue(e.RowHandle, bandedGridView1.Columns[0]).ToString();

            NoteNo = NoteNo.Trim();

            Declined = bandedGridView1.GetRowCellValue(e.RowHandle, bandedGridView1.Columns[1]).ToString();
        }

        private void bandedGridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            string category = bandedGridView1.GetRowCellDisplayText(e.RowHandle, bandedGridView1.Columns["status"]);


            bandedGridView1.Columns[0].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[1].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[2].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[3].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[4].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[5].AppearanceCell.ForeColor = Color.Black;

            bandedGridView1.Columns[6].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[7].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[8].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[9].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[10].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[11].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[12].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[13].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[14].AppearanceCell.ForeColor = Color.Black;
            bandedGridView1.Columns[15].AppearanceCell.ForeColor = Color.Black;
            //  bandedGridView1.Columns[16].AppearanceCell.ForeColor = Color.Black;

            if (category == "Declined")
            {
                bandedGridView1.Columns[0].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[1].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[2].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[3].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[4].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[5].AppearanceCell.ForeColor = Color.Blue;

                bandedGridView1.Columns[6].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[7].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[8].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[9].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[10].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[11].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[12].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[13].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[14].AppearanceCell.ForeColor = Color.Blue;
                bandedGridView1.Columns[15].AppearanceCell.ForeColor = Color.Blue;


            }

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {

                if (TypeLabel == "CubbyStop")
                {

                    CubbyStopNotes CubbyFrm = new CubbyStopNotes();

                    FormText = "CubbyNote";
                    CubbyFrm.ImageType = Image;
                    CubbyFrm.IsChanged = true;
                    CubbyFrm.IDtxt.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                    if (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() == "CN")
                    {
                        CubbyFrm.HeightTxt.Visible = true;
                        CubbyFrm.DepthTxt.Visible = true;
                        CubbyFrm.LengthTxt.Visible = true;
                        CubbyFrm.label10.Visible = true;
                        CubbyFrm.label9.Visible = true;
                        CubbyFrm.label8.Visible = true;
                        CubbyFrm.label13.Visible = true;
                        CubbyFrm.label11.Visible = true;
                        CubbyFrm.label12.Visible = true;
                        CubbyFrm.AddBtn.Visible = true;
                        CubbyFrm.CubbyRightbtn.Visible = true;
                        CubbyFrm.DrillRigLeftBtn.Visible = true;
                        CubbyFrm.DrillCubbyRightbtn.Visible = true;
                        CubbyFrm.DrillCubbyLeft.Visible = true;
                        CubbyFrm.DrillRigRightBtn.Visible = true;
                        CubbyFrm.PegBtn.Visible = true;
                        CubbyFrm.noteType = "CN";
                        CubbyFrm.getImagePnl.Dock = DockStyle.Right;
                        //CubbyFrm.panel8.Dock = DockStyle.Left;
                        //CubbyFrm.panel8.Visible = true;
                        CubbyFrm.panel6.Dock = DockStyle.Fill;
                        CubbyFrm.panel7.Visible = false;
                        //CubbyFrm.panel8.Width = 207;
                        //CubbyFrm.Search2lbl.Visible = false;
                        CubbyFrm.SearchWP2txt.Visible = false;
                        //CubbyFrm.WP2lbl.Visible = false;
                        CubbyFrm.WP2List.Visible = false;
                    }
                    else
                    {
                        FormText = "StopeNote";
                        CubbyFrm.HeightTxt.Visible = false;
                        CubbyFrm.DepthTxt.Visible = false;
                        CubbyFrm.LengthTxt.Visible = false;
                        CubbyFrm.label10.Visible = false;
                        CubbyFrm.label9.Visible = false;
                        CubbyFrm.label8.Visible = false;
                        CubbyFrm.label13.Visible = false;
                        CubbyFrm.label11.Visible = false;
                        CubbyFrm.label12.Visible = false;
                        CubbyFrm.AddBtn.Visible = false;
                        CubbyFrm.CubbyRightbtn.Visible = false;
                        CubbyFrm.DrillRigLeftBtn.Visible = false;
                        CubbyFrm.DrillCubbyRightbtn.Visible = false;
                        CubbyFrm.DrillCubbyLeft.Visible = false;
                        CubbyFrm.DrillRigRightBtn.Visible = false;
                        CubbyFrm.PegBtn.Visible = false;
                        CubbyFrm.panel7.Visible = true;
                        CubbyFrm.getImagePnl.Dock = DockStyle.Right;
                        CubbyFrm.panel7.Dock = DockStyle.Fill;
                        CubbyFrm.panel6.Visible = false;
                        //CubbyFrm.label4.Visible = false;
                        CubbyFrm.WorkplaceCmb.Visible = false;
                        //CubbyFrm.panel8.Dock = DockStyle.Left;
                        //CubbyFrm.Search2lbl.Visible = true;
                        CubbyFrm.SearchWP2txt.Visible = true;
                        CubbyFrm.WP2lbl.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        CubbyFrm.WP2List.Visible = true;
                        CubbyFrm.WP2Search.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        //CubbyFrm.panel8.Width = 396;
                    }

                    CubbyFrm.WindowState = FormWindowState.Maximized;
                    CubbyFrm.ShowDialog();

                    LoadNotes();

                }
            }
            else
            {

            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                if (TypeLabel == "Sampling")
                {
                    Date = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    WpNo = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    WpDesc = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    SW = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    CorrCut = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    StdSw = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    Grade = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                    Density = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                }
                else if (TypeLabel == "Peg Values")
                {
                    WpNo = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    WpDesc = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    PegID = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                }
            }
            if (e.RowIndex > -1)
            {
                GridSelectLabel = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }

        private void NewNotebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (TypeLabel == "ReefBoring")
            {
                ReefBoringNote ReefBoringNote = new ReefBoringNote();
                ReefBoringNote.panel8.Dock = DockStyle.Left;
                ReefBoringNote.panel8.Visible = true;
                ReefBoringNote.panel8.Width = 207;


                ReefBoringNote.WindowState = FormWindowState.Maximized;
                ReefBoringNote.Show();
            }
            else
            {
                CubbyStopNotes CubbyFrm = new CubbyStopNotes();

                FormText = "CubbyNote";
                CubbyFrm.noteType = "CN";
                CubbyFrm.getImagePnl.Dock = DockStyle.Right;
               
                CubbyFrm.SearchWP2txt.Visible = false;
                
                CubbyFrm.WP2List.Visible = false;
                CubbyFrm.panel6.Dock = DockStyle.Fill;
                CubbyFrm.panel7.Visible = false;


                CubbyFrm.WindowState = FormWindowState.Maximized;
                CubbyFrm.Show();

            }

        }

        private void CompletedNotebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void RemoveNoteNB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void NewStopNotebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }

        private void btnDevNote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CubbyStopNotes CubbyFrm = new CubbyStopNotes();

            FormText = "Developement Note";
            CubbyFrm.noteType = "DN";

            CubbyFrm.getImagePnl.Dock = DockStyle.Right;
            
            CubbyFrm.SearchWP2txt.Visible = false;
           
            CubbyFrm.WP2List.Visible = false;
            CubbyFrm.gbDevelopmentNote.Dock = DockStyle.Fill;
            CubbyFrm.gbDevelopmentNote.Height = 530;
            CubbyFrm.panel7.Visible = false;

            CubbyFrm._theSystemDBTag = this.theSystemDBTag;
            CubbyFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;
            CubbyFrm.WindowState = FormWindowState.Maximized;

            CubbyFrm.ShowDialog();

            LoadNotes();
        }

        private void btnCoverNote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CubbyStopNotes CubbyFrm = new CubbyStopNotes();

            FormText = "Cover Note";
            CubbyFrm.noteType = "CVN";
            CubbyFrm.getImagePnl.Dock = DockStyle.Right;
            
            CubbyFrm.SearchWP2txt.Visible = false;
            
            CubbyFrm.WP2List.Visible = false;
            CubbyFrm.gbCoverNote.Dock = DockStyle.Fill;
            CubbyFrm.panel7.Visible = false;
            CubbyFrm._theSystemDBTag = this.theSystemDBTag;
            CubbyFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;

            CubbyFrm.WindowState = FormWindowState.Maximized;
            CubbyFrm.Show();
            LoadNotes();
        }

        private void btnStartNote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CubbyStopNotes CubbyFrm = new CubbyStopNotes();

            FormText = "Start Note";

            CubbyFrm.noteType = "SRN";
            //CubbyFrm.PnlSection.Dock = DockStyle.Top;
            CubbyFrm.getImagePnl.Dock = DockStyle.Right;
           
            CubbyFrm.SearchWP2txt.Visible = false;
            
            CubbyFrm.WP2List.Visible = false;
            CubbyFrm.gbStartNote.Dock = DockStyle.Fill;
            CubbyFrm.panel7.Visible = false;
            CubbyFrm._theSystemDBTag = this.theSystemDBTag;
            CubbyFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;


            CubbyFrm.WindowState = FormWindowState.Maximized;
            CubbyFrm.Show();
            LoadNotes();
        }

        private void btnStopingNote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CubbyStopNotes CubbyFrm = new CubbyStopNotes();

            FormText = "Stoping Note";

            CubbyFrm.getImagePnl.Dock = DockStyle.Right;
            
            CubbyFrm.SearchWP2txt.Visible = false;
            
            CubbyFrm.WP2List.Visible = false;

            CubbyFrm.gbStopingNote.Dock = DockStyle.Fill;
            CubbyFrm.noteType = "STN";
            CubbyFrm.panel7.Visible = false;
            CubbyFrm._theSystemDBTag = this.theSystemDBTag;
            CubbyFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;


            CubbyFrm.WindowState = FormWindowState.Maximized;
            CubbyFrm.Show();
            LoadNotes();
        }

        private void OpencastSurveyNotebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CubbyStopNotes CubbyFrm = new CubbyStopNotes();
            FormText = "Open Cast Survey Note";

            CubbyFrm.noteType = "OCN";
            CubbyFrm.getImagePnl.Dock = DockStyle.Right;
           
            CubbyFrm.SearchWP2txt.Visible = false;
           
            CubbyFrm.WP2List.Visible = false;
            CubbyFrm.gbOpenCastSurveyNote.Dock = DockStyle.Fill;
            CubbyFrm.panel7.Visible = false;
            CubbyFrm._theSystemDBTag = this.theSystemDBTag;
            CubbyFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;


            CubbyFrm.WindowState = FormWindowState.Maximized;
            CubbyFrm.Show();
            LoadNotes();
        }

        private void HolingNotificationNotebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CubbyStopNotes CubbyFrm = new CubbyStopNotes();

            FormText = "Holing Notification Note";

            CubbyFrm.noteType = "HNN";
            CubbyFrm.getImagePnl.Dock = DockStyle.Right;
          
            CubbyFrm.SearchWP2txt.Visible = false;
            
            CubbyFrm.WP2List.Visible = false;
            CubbyFrm.gbHolingNotification.Dock = DockStyle.Fill;
            CubbyFrm.gbHolingNotification.Height = 530;
            CubbyFrm.panel7.Visible = false;
            CubbyFrm._theSystemDBTag = this.theSystemDBTag;
            CubbyFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;


            CubbyFrm.WindowState = FormWindowState.Maximized;
            CubbyFrm.Show();
            LoadNotes();
        }

        private void StopPanelReportNotebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CubbyStopNotes CubbyFrm = new CubbyStopNotes();

            FormText = "Stop Panel Report Note";

            CubbyFrm.noteType = "SPR";
            CubbyFrm.getImagePnl.Dock = DockStyle.Right;
           
            CubbyFrm.SearchWP2txt.Visible = false;
            
            CubbyFrm.WP2List.Visible = false;
            CubbyFrm.gbStopPanelReportNote.Dock = DockStyle.Fill;
            CubbyFrm.panel7.Visible = false;
            CubbyFrm._theSystemDBTag = this.theSystemDBTag;
            CubbyFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;


            CubbyFrm.WindowState = FormWindowState.Maximized;
            CubbyFrm.Show();
            LoadNotes();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnStoppingNote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void RCSurveyNotes_Click(object sender, EventArgs e)
        {

        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWordEditor helpFrm = new frmWordEditor();
            helpFrm.ViewType = "View";
            helpFrm.MainCat = "SurveyNote";
            helpFrm.SubCat = "SurveyNote";
            helpFrm.Show();
        }
        
        private void LoadActions()
        {
            //Declarations
            MWDataManager.clsDataAccess _LoadWP = new MWDataManager.clsDataAccess();
            _LoadWP.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _LoadWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadWP.SqlStatement = " EXEC sp_Survey_LoadActions ";
                                    
            _LoadWP.ExecuteInstruction();

            DataTable tbl_WP = _LoadWP.ResultsDataTable;
            

            gcActions.DataSource = null;
            gcActions.DataSource = tbl_WP;
            gcActID.FieldName = "ID";
            gcWorkplace.FieldName = "Workplace";
            gcDescription.FieldName = "Description";
            gcRecommendation.FieldName = "Action";
            gcTargetDate.FieldName = "TargetDate";
            gcPriority.FieldName = "Priority";
            gcImage.FieldName = "CompNotes";
            gcViewImage.FieldName = "Hyperlink";
            gcRespPerson.FieldName = "RespPerson";
            gcOverseer.FieldName = "HOD";
        }

        private void AddActBtn_Click(object sender, EventArgs e)
        {
            frmActionCapture ActFrm = new frmActionCapture();
            ActFrm.tbPM.Text = SysSettings.ProdMonth.ToString();
            ActFrm.tbSections.Text = Section;
            //ActFrm.WPEdit2.Properties.Items.Add(WpDesc);
            
            //ActFrm.WPEdit2.Visible = true;
            ActFrm.tbWorkplace.Visible = false;

            ActFrm._theSystemDBTag = this.theSystemDBTag;
            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;

            ActFrm.Item = "Survey Actions";
            ActFrm.Type = "SR";
            ActFrm.AllowExit = "Y";

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        private void EditActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");
                return;
            }

            frmActionCapture ActFrm = new frmActionCapture();
            ActFrm.tbPM.Text = SysSettings.ProdMonth.ToString();
            ActFrm.tbSections.Text = Section;
            
            
            //ActFrm.WPEdit2.Visible = true;
            ActFrm.tbWorkplace.Visible = false;

            ActFrm._theSystemDBTag = this.theSystemDBTag;
            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;

            ActFrm.Item = "Survey Actions";
            ActFrm.Type = "SR"; ;
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";

            ActFrm.LookUpEditWorkplace.EditValue = Workplace;
            ActFrm.Item = Description;
            ActFrm.ActionTxt.Text = Recomendation;
            ActFrm.ReqDate.Text = TargetDate;
            ActFrm.PriorityCmb.Text = Priority;
            ActFrm.RespPersonCmb.EditValue = RespPerson;
            ActFrm.OverseerCmb.EditValue = Overseer;
            ActFrm.ActID = ID;

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        private void DelActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please click the row you want to delete first.");
                return;
            }



            MWDataManager.clsDataAccess _DeleteAction = new MWDataManager.clsDataAccess();
            _DeleteAction.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _DeleteAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _DeleteAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _DeleteAction.SqlStatement = " Delete from tbl_Shec_Incidents where ID = '" + ID + "'\r\n";

            _DeleteAction.ExecuteInstruction();

            LoadActions();
        }

        private void gvAction_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            ID = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[0]).ToString();
            Workplace = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[1]).ToString();
            Description = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[2]).ToString();
            Recomendation = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[3]).ToString();
            TargetDate = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[4]).ToString();
            Priority = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[5]).ToString();
            FileName = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[6]).ToString();

            RespPerson = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[9]).ToString();
            Overseer = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[10]).ToString();
        }
    }
}
