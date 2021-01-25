using DevExpress.XtraGrid.Views.Grid;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
//using Mineware.Systems.HarmonyPASGlobal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Survey
{
    public partial class ucDocManager : BaseUserControl
    {
        #region Fields and Properties           
        private Report _theReport;
        /// <summary>
        /// The report to show
        /// </summary>
        public Report TheReport
        {
            get
            {
                return _theReport;
            }
            set
            {
                LoadReport(value);
            }
        }

        /// <summary>
        /// The folder name where the reports are located
        /// </summary>
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

#if DEBUG
        private bool _designReport = true;
#else
        private bool _designReport = false;
#endif
        #endregion Fields and Properties  

        #region Constructor
        public ucDocManager()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpDocManager);
            FormActiveRibbonPage = rpDocManager;
            FormMainRibbonPage = rpDocManager;
            RibbonControl = rcDeptSafety;
        }
        #endregion Constructor

        #region Events

        private void ucDocManager_Load(object sender, EventArgs e)
        {
            //this.Icon = PAS.Properties.Resources.testbutton3;
            LoadNotes();
        }

        private void gcActiveNotes_DoubleClick(object sender, EventArgs e)
        {
            LoadSurveyNote();
        }

        private void Print1Btn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            //_dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //_dbManWPSTDetail.SqlStatement = " insert into  tbl_TempStopNote_MOPrint " +
            //                                " Values( '" + NoteNolabel.Text + "', '" + clsUserInfo.UserID + "', getdate()) ";
            //_dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbManWPSTDetail.ExecuteInstruction();

            //LoadNotes();

            //_theReport.Print();
        }

        private void gcActiveNotes_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            NoteNolabel.Text = gcActiveNotes.GetRowCellValue(e.RowHandle, gcActiveNotes.Columns[7]).ToString();
            NoteNolabel.Text = NoteNolabel.Text + "                ";
            NoteNolabel.Text = NoteNolabel.Text.Substring(2, 10);
            NoteNolabel.Text = NoteNolabel.Text.Trim();
        }

        private void btnTransfere_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to transfer " + NoteNolabel.Text + " to completed", "Record Transfered", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                //MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                //_dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                //_dbMan.SqlStatement = " delete tbl_TempStopNote_MOExclude where tsnid = '" + NoteNolabel.Text + "' ";
                //_dbMan.SqlStatement = _dbMan.SqlStatement + " insert into  tbl_TempStopNote_MOExclude ";
                //_dbMan.SqlStatement = _dbMan.SqlStatement + " Values( '" + NoteNolabel.Text + "', '" + clsUserInfo.UserID + "', getdate()) ";

                //_dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbMan.ExecuteInstruction();
            }

            LoadNotes();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        #endregion Events 

        #region Methods  
        /// <summary>
        /// Load the report after adding data
        /// </summary>
        /// <param name="report"></param>
        public void LoadReport(Report report)
        {
            _theReport = report;
            _theReport.Preview = pcReport;

            if (_designReport)
                _theReport.Design();
            else
                _theReport.Show();
        }

        /// <summary>
        /// Used to load data on the report
        /// </summary>
        public void LoadData()
        {
            DataSet ds = new DataSet();
            MWDataManager.clsDataAccess _databaseConnection = new MWDataManager.clsDataAccess();
            _databaseConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _databaseConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _databaseConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _databaseConnection.SqlStatement = "PUT SQL HERE";

            var ActionResult = _databaseConnection.ExecuteInstruction();

            if (ActionResult.success)
            {
                _databaseConnection.ResultsDataTable.TableName = "TABLENAME";
                ds.Tables.Add(_databaseConnection.ResultsDataTable);
            }


            _theReport.RegisterData(ds);

            //Show the report
            LoadReport(_theReport);
        }

        public void LoadNotes()
        {
            string sect = string.Empty; // TUserCurrentInfo.UserBookSection + "%";
            sect = "%";

            btnTransfere.Enabled = false;
            Print1Btn.Enabled = false;

            //if (clsUserInfo.UserBookSection.Length == 4)
            //{
            //    sect = clsUserInfo.UserBookSection + "%";
            //    btnTransfere.Enabled = true;
            //    Print1Btn.Enabled = true;
            //}


            string mine = string.Empty;

            mine = ProductionGlobalTSysSettings._Banner.ToString();


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " select case when printed is not null then 'Printed' else null end as pp, *,  \r\n  " +
                                     "case when lvl7 = 'Yes' Then ''  \r\n  " +
                                     "when lvl7 = 'Dec' Then 'Declined Section Manager'  \r\n  " +
                                     "when lvl7 = 'No' Then 'Awaiting Authorisation Section Manager'  \r\n  " +

                                     "when lvl6 = 'Dec' Then 'Declined Mine Overseer'  \r\n  " +
                                     "when lvl6 = 'No' Then 'Awaiting Authorisation Mine Overseer'  \r\n  " +

                                     "when lvl5 = 'Dec' Then 'Declined Ventilation'  \r\n  " +
                                     "when lvl5 = 'No' Then 'Awaiting Authorisation Ventilation'  \r\n  " +

                                     "when lvl4 = 'Dec' Then 'Declined Rock Mechanic'  \r\n  " +
                                     "when lvl4 = 'No' Then 'Awaiting Authorisation Rock Mechanic'  \r\n  " +

                                     "when lvl3 = 'Dec' Then 'Declined Geologist'  \r\n  " +
                                     "when lvl3 = 'No' Then 'Awaiting Authorisation Geologist'  \r\n  " +

                                     "when lvl2 = 'Dec' Then 'Declined Snr Mine Surveyor'  \r\n  " +
                                     "when lvl2 = 'No' Then 'Awaiting Authorisation Snr Mine Surveyor'  \r\n  " +


                                     "end as CodeN, \r\n  " +

                                     "case when lvl7 = 'Yes' Then 'Yes' end as finN \r\n  " +


                                     " from (Select '" + mine + "'+convert(varchar(10),a.tsnid) Note, a.*, b.Description, CONVERT(VARCHAR(11),thedate,106) UseDate, 'No' aa,  \r\n  " +


                                     "case when lvl7ok is not null and status <> 'Declined' then 'Authorised'  \r\n " +
                                     "when status = 'Declined' then 'Declined' else 'Still Active' end as status  \r\n " +




                                     //z.status "+
                                     ", isnull(lvl1ok, 'No') Lvl1, \r\n" +

                                     "case  " +
                                     "when lvl2ok is null and status = 'Declined' then '' \r\n" +
                                     "when lvl2ok is null  and lvl1ok is not null  and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl2ok is null and status <> 'Declined' then 'No1' \r\n" +


                                     "else lvl2ok end as Lvl2, \r\n" +

                                     "case  " +
                                     "when lvl3ok is null and status = 'Declined' then '' \r\n" +
                                     "when lvl3ok is null and lvl2ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl3ok is null and status <> 'Declined' then 'No1' \r\n" +
                                     "else lvl3ok end as Lvl3, \r\n" +

                                     "case  " +
                                     "when lvl4ok is null and status = 'Declined' then '' \r\n" +
                                     "when lvl4ok is null and lvl3ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl4ok is null and status <> 'Declined' then 'No1' \r\n" +
                                     "else lvl4ok end as Lvl4, \r\n" +


                                     "case  " +
                                     "when lvl5ok is null and status = 'Declined' then '' \r\n" +
                                     "when lvl5ok is null and lvl4ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl5ok is null and status <> 'Declined' then 'No1' \r\n" +
                                     "else lvl5ok end as Lvl5, \r\n" +

                                     "case  " +
                                     "when lvl6ok is null and status = 'Declined' then '' \r\n" +
                                     "when lvl6ok is null and lvl5ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl6ok is null and status <> 'Declined' then 'No1' \r\n" +
                                     "else lvl6ok end as Lvl6, \r\n" +

                                     "case  " +
                                     "when lvl7ok is null and status = 'Declined' then '' \r\n" +
                                     "when lvl7ok is null and lvl6ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl7ok is null and status <> 'Declined' then 'No1' \r\n" +
                                     "else lvl7ok end as Lvl7, \r\n" +

                                     "case  " +
                                     "when lvl8ok is null and status = 'Declined' then '' \r\n" +
                                     "when lvl8ok is null and lvl7ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl8ok is null and status <> 'Declined' then 'No1' \r\n" +
                                     "else lvl8ok end as Lvl8, \r\n" +

                                     "case  " +
                                     "when lvl9ok is null and status = 'Declined' then '' \r\n" +
                                     "when lvl9ok is null and lvl8ok is not null and status <> 'Declined' then 'No' \r\n" +
                                     "when lvl9ok is null and status <> 'Declined' then 'No1' \r\n" +
                                     "else lvl9ok end as Lvl9, \r\n" +



                                     "case   when 'SBDate' is null and status = 'Declined' then ''  \r\n" +
                                     "when 'SBDate' is null and lvl9ok is not null and status <> 'Declined' then 'No'  \r\n" +
                                     "when 'SBDate' is null and status <> 'Declined' then 'No1'  \r\n" +
                                     "else 'Yes' end as SB,  \r\n" +

                                     "case   when 'MODate' is null and status = 'Declined' then ''  \r\n" +
                                     "when 'MODate' is null and lvl9ok is not null and status <> 'Declined' then 'No'  \r\n" +
                                     "when 'MODate' is null and status <> 'Declined' then 'No1'  \r\n" +
                                     "else 'Yes'  " +

                                     "end as MO,  " +


                                     //isnull(lvl2ok, 'No') Lvl2, isnull(lvl3ok, 'No') Lvl3 " +
                                     //                          ", isnull(lvl4ok, 'No') Lvl4, isnull(lvl5ok, 'No') Lvl5, isnull(lvl6ok, 'No') Lvl6 " +
                                     //                          ", isnull(lvl7ok, 'No') Lvl7, isnull(lvl8ok, 'No') Lvl8, isnull(lvl9ok, 'No') Lvl9, 


                                     "datediff(day,thedate,getdate()) datediff1, case when lvl7date is not null then convert(varchar(10),datediff(day,thedate,lvl7date) ) \r\n" +
                                    "when lvl7date is null and status <> 'Declined' then convert(varchar(10),datediff(day,thedate,getdate())) \r\n" +
                                    "when status = 'Declined' then '****'  \r\n" +

                                    " else null end as FinalDays  \r\n" +

                                     "from  tbl_SurveyNote a, vw_Workplace_Total b,  tbl_SurveyNote_Autorisation z  where  a.[TSNID] = z.[TSNID] and  \r\n" +
                                    "a.IsCompleted = 'N' and \r\n" +
                                    "a.WorkplaceID = b.WorkplaceID and a.IsDeleted = 'N' and lvl1ok is not null and status <> 'Declined' and a.tsnid not in (select tsnid from tbl_TempStopNote_MOExclude)) a \r\n" +
                                    " left outer join (select distinct(tsnid) Printed from tbl_TempStopNote_MOPrint) b on a.tsnid = b.printed where  sectionid like '" + sect + "' Order By a.TSNID desc ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);

            gvActiveNotes.DataSource = ds1.Tables[0];

            ColNote.FieldName = "Note";
            col1SecID.FieldName = "SectionID";

            col1Wp.FieldName = "Description";
            col1Date.FieldName = "UseDate";
            col1Days.FieldName = "FinalDays";

            col1Code.FieldName = "CodeN";
            col1Author.FieldName = "finN";
            col1Print.FieldName = "pp";

            gvActiveNotes.Visible = true;


            return;


            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan2.SqlStatement = " select case when printed is not null then 'Printed' else null end as pp,*,  \r\n  " +
                                     "case when lvl7 = 'Yes' Then ''  \r\n  " +
                                     "when lvl7 = 'Dec' Then 'Declined Section Manager'  \r\n  " +
                                     "when lvl7 = 'No' Then 'Awaiting Authorisation Section Manager'  \r\n  " +

                                     "when lvl6 = 'Dec' Then 'Declined Mine Overseer'  \r\n  " +
                                     "when lvl6 = 'No' Then 'Awaiting Authorisation Mine Overseer'  \r\n  " +

                                     "when lvl5 = 'Dec' Then 'Declined Ventilation'  \r\n  " +
                                     "when lvl5 = 'No' Then 'Awaiting Authorisation Ventilation'  \r\n  " +

                                     "when lvl4 = 'Dec' Then 'Declined Rock Mechanic'  \r\n  " +
                                     "when lvl4 = 'No' Then 'Awaiting Authorisation Rock Mechanic'  \r\n  " +

                                     "when lvl3 = 'Dec' Then 'Declined Geologist'  \r\n  " +
                                     "when lvl3 = 'No' Then 'Awaiting Authorisation Geologist'  \r\n  " +

                                     "when lvl2 = 'Dec' Then 'Declined Snr Mine Surveyor'  \r\n  " +
                                     "when lvl2 = 'No' Then 'Awaiting Authorisation Snr Mine Surveyor'  \r\n  " +

                                     "when lvl1 = 'Dec' Then 'Declined Checked By'  \r\n  " +


                                     "end as CodeN, \r\n  " +

                                     "case when lvl7 = 'Yes' Then 'Yes' end as finN \r\n  " +


                                     " from (Select '" + mine + "'+convert(varchar(10),a.tsnid) Note, a.*, b.Description, CONVERT(VARCHAR(11),thedate,106) UseDate, 'No' aa,  \r\n  " +


                                     "case when lvl7ok is not null and status <> 'Declined' then 'Authorised'  \r\n " +
                                     "when status = 'Declined' then 'Declined' else 'Still Active' end as status  \r\n " +




                                     //z.status "+
                                     ", isnull(lvl1ok, 'No') Lvl1, " +

                                     "case  " +
                                     "when lvl2ok is null and status = 'Declined' then '' " +
                                     "when lvl2ok is null  and lvl1ok is not null  and status <> 'Declined' then 'No' " +
                                     "when lvl2ok is null and status <> 'Declined' then 'No1' " +


                                     "else lvl2ok end as Lvl2, " +

                                     "case  " +
                                     "when lvl3ok is null and status = 'Declined' then '' " +
                                     "when lvl3ok is null and lvl2ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl3ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl3ok end as Lvl3, " +

                                     "case  " +
                                     "when lvl4ok is null and status = 'Declined' then '' " +
                                     "when lvl4ok is null and lvl3ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl4ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl4ok end as Lvl4, " +


                                     "case  " +
                                     "when lvl5ok is null and status = 'Declined' then '' " +
                                     "when lvl5ok is null and lvl4ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl5ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl5ok end as Lvl5, " +

                                     "case  " +
                                     "when lvl6ok is null and status = 'Declined' then '' " +
                                     "when lvl6ok is null and lvl5ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl6ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl6ok end as Lvl6, " +

                                     "case  " +
                                     "when lvl7ok is null and status = 'Declined' then '' " +
                                     "when lvl7ok is null and lvl6ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl7ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl7ok end as Lvl7, " +

                                     "case  " +
                                     "when lvl8ok is null and status = 'Declined' then '' " +
                                     "when lvl8ok is null and lvl7ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl8ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl8ok end as Lvl8, " +

                                     "case  " +
                                     "when lvl9ok is null and status = 'Declined' then '' " +
                                     "when lvl9ok is null and lvl8ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl9ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl9ok end as Lvl9, " +



                                     "case   when SBDate is null and status = 'Declined' then ''  " +
                                     "when SBDate is null and lvl9ok is not null and status <> 'Declined' then 'No'  " +
                                     "when SBDate is null and status <> 'Declined' then 'No1'  " +
                                     "else 'Yes' end as SB,  " +

                                     "case   when MODate is null and status = 'Declined' then ''  " +
                                     "when MODate is null and lvl9ok is not null and status <> 'Declined' then 'No'  " +
                                     "when MODate is null and status <> 'Declined' then 'No1'  " +
                                     "else 'Yes'  " +

                                     "end as MO,  " +


                                     //isnull(lvl2ok, 'No') Lvl2, isnull(lvl3ok, 'No') Lvl3 " +
                                     //                          ", isnull(lvl4ok, 'No') Lvl4, isnull(lvl5ok, 'No') Lvl5, isnull(lvl6ok, 'No') Lvl6 " +
                                     //                          ", isnull(lvl7ok, 'No') Lvl7, isnull(lvl8ok, 'No') Lvl8, isnull(lvl9ok, 'No') Lvl9, 


                                     "datediff(day,thedate,getdate()) datediff1, case when lvl7date is not null then convert(varchar(10),datediff(day,thedate,lvl7date) ) " +
                                    "when lvl7date is null and status <> 'Declined' then convert(varchar(10),datediff(day,thedate,getdate())) " +
                                    "when status = 'Declined' then '****' " +

                                    " else null end as FinalDays " +

                                     "from [Mineware_Reporting].[dbo].tbl_SurveyNote a, tbl_Workplace b, [Mineware_Reporting].[dbo].tbl_SurveyNote_Autorisation z  where  a.[TSNID] = z.[TSNID] and  " +
                                    "a.IsCompleted = 'N' and " +
                                    "a.WorkplaceID = b.WorkplaceID and a.IsDeleted = 'N' and status = 'Declined' and a.TSNID NOT in (select TSNID from tbl_TempStopNote_MOExclude) " +

                                    "union " +

                                    "Select '" + mine + "'+convert(varchar(10),a.tsnid) Note, a.*, b.Description, CONVERT(VARCHAR(11),thedate,106) UseDate, 'No' aa,  \r\n  " +


                                     "case when lvl7ok is not null and status <> 'Declined' then 'Authorised'  \r\n " +
                                     "when status = 'Declined' then 'Declined' else 'Still Active' end as status  \r\n " +




                                     //z.status "+
                                     ", isnull(lvl1ok, 'No') Lvl1, " +

                                     "case  " +
                                     "when lvl2ok is null and status = 'Declined' then '' " +
                                     "when lvl2ok is null  and lvl1ok is not null  and status <> 'Declined' then 'No' " +
                                     "when lvl2ok is null and status <> 'Declined' then 'No1' " +


                                     "else lvl2ok end as Lvl2, " +

                                     "case  " +
                                     "when lvl3ok is null and status = 'Declined' then '' " +
                                     "when lvl3ok is null and lvl2ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl3ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl3ok end as Lvl3, " +

                                     "case  " +
                                     "when lvl4ok is null and status = 'Declined' then '' " +
                                     "when lvl4ok is null and lvl3ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl4ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl4ok end as Lvl4, " +


                                     "case  " +
                                     "when lvl5ok is null and status = 'Declined' then '' " +
                                     "when lvl5ok is null and lvl4ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl5ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl5ok end as Lvl5, " +

                                     "case  " +
                                     "when lvl6ok is null and status = 'Declined' then '' " +
                                     "when lvl6ok is null and lvl5ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl6ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl6ok end as Lvl6, " +

                                     "case  " +
                                     "when lvl7ok is null and status = 'Declined' then '' " +
                                     "when lvl7ok is null and lvl6ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl7ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl7ok end as Lvl7, " +

                                     "case  " +
                                     "when lvl8ok is null and status = 'Declined' then '' " +
                                     "when lvl8ok is null and lvl7ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl8ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl8ok end as Lvl8, " +

                                     "case  " +
                                     "when lvl9ok is null and status = 'Declined' then '' " +
                                     "when lvl9ok is null and lvl8ok is not null and status <> 'Declined' then 'No' " +
                                     "when lvl9ok is null and status <> 'Declined' then 'No1' " +
                                     "else lvl9ok end as Lvl9, " +



                                     "case   when SBDate is null and status = 'Declined' then ''  " +
                                     "when SBDate is null and lvl9ok is not null and status <> 'Declined' then 'No'  " +
                                     "when SBDate is null and status <> 'Declined' then 'No1'  " +
                                     "else 'Yes' end as SB,  " +

                                     "case   when MODate is null and status = 'Declined' then ''  " +
                                     "when MODate is null and lvl9ok is not null and status <> 'Declined' then 'No'  " +
                                     "when MODate is null and status <> 'Declined' then 'No1'  " +
                                     "else 'Yes'  " +

                                     "end as MO,  " +


                                     //isnull(lvl2ok, 'No') Lvl2, isnull(lvl3ok, 'No') Lvl3 " +
                                     //                          ", isnull(lvl4ok, 'No') Lvl4, isnull(lvl5ok, 'No') Lvl5, isnull(lvl6ok, 'No') Lvl6 " +
                                     //                          ", isnull(lvl7ok, 'No') Lvl7, isnull(lvl8ok, 'No') Lvl8, isnull(lvl9ok, 'No') Lvl9, 


                                     "datediff(day,thedate,getdate()) datediff1, case when lvl7date is not null then convert(varchar(10),datediff(day,thedate,lvl7date) ) " +
                                    "when lvl7date is null and status <> 'Declined' then convert(varchar(10),datediff(day,thedate,getdate())) " +
                                    "when status = 'Declined' then '****' " +

                                    " else null end as FinalDays " +

                                     "from [Mineware_Reporting].[dbo].tbl_SurveyNote a, vw_Workplace_Total b, [Mineware_Reporting].[dbo].tbl_SurveyNote_Autorisation z  where  a.[TSNID] = z.[TSNID] and  " +
                                    "a.IsCompleted = 'N' and " +
                                    "a.WorkplaceID = b.WorkplaceID and a.TSNID in (select TSNID from tbl_TempStopNote_MOExclude) " +






                                    ") a left outer join (select distinct(tsnid) Printed from tbl_TempStopNote_MOPrint) b on a.tsnid = b.printed " +
                                    "  where  sectionid like '" + sect + "' Order By a.TSNID desc ";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ExecuteInstruction();

            DataTable dt1 = _dbMan2.ResultsDataTable;

            DataSet ds2 = new DataSet();
            ds2.Tables.Add(dt1);

            gridControl1.DataSource = ds2.Tables[0];

            bandedGridColumn1.FieldName = "Note";
            bandedGridColumn2.FieldName = "SectionID";

            bandedGridColumn3.FieldName = "Description";
            bandedGridColumn4.FieldName = "UseDate";
            bandedGridColumn5.FieldName = "FinalDays";

            bandedGridColumn6.FieldName = "CodeN";
            bandedGridColumn7.FieldName = "finN";
            bandedGridColumn8.FieldName = "pp";

            gridControl1.Visible = true;


        }

        private void LoadSurveyNote()
        {
            MWDataManager.clsDataAccess _dbManEmptyCubby = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbManEmptyCubby.SqlStatement = "  select top(1) * from ( select a.*,  " +
                                       "   b.lvl1date, s1.sigfileno sigfileno1,   \r\n " +
                                       "   b.lvl2date, s2.sigfileno sigfileno2,   \r\n " +
                                       "   b.lvl3date, s3.sigfileno sigfileno3,  \r\n " +
                                       "   b.lvl4date, s4.sigfileno sigfileno4,  \r\n " +
                                       "   b.lvl5date, s5.sigfileno sigfileno5,  \r\n " +
                                       "   b.lvl6date, s6.sigfileno sigfileno6,  \r\n " +
                                       "   b.lvl7date, s7.sigfileno sigfileno7,  \r\n " +
                                       "   b.lvl8date, s8.sigfileno sigfileno8,  \r\n " +
                                        "  b.lvl9date, s9.sigfileno sigfileno9 --,  s11.sigfileno sigfilenouser  \r\n " +
                                         "  ,b.Lvl1Remks,  b.Lvl2Remks,  b.Lvl3Remks,  b.Lvl4Remks,  b.Lvl5Remks,  b.Lvl6Remks,  b.Lvl7Remks,  b.Lvl8Remks, b.Lvl9Remks \r\n " +
                                       "  , us1.username usnamelvl1, us2.username usnamelvl2, us3.username usnamelvl3, us4.username usnamelvl4, us5.username usnamelvl5, us6.username usnamelvl6, us7.username usnamelvl7, us8.username usnamelvl8, us9.username usnamelvl9 " +




                                       " , case when status = 'Declined' then 'D' when (select banner from tbl_sysset) <> 'Moab Khotsong' and us9.username is null then 'N' " +
                                       " when (select banner from tbl_sysset) = 'Moab Khotsong' and us7.username is null then 'N' " +
                                       "  " +

                                       " else 'Y' end as auth " +




                                       "  from (  \r\n " +





                                       "  select  TSNID ID, NoteType NoteType, w.WorkplaceID +':'+ description WP, MOSection +':'+ s1.Name MOSection,  TheDate Date, " +
                                       " /*StopRemark StopRemark,   \r\n " +
                                       "  GenRemark Remark, PegNo PegFrom,  '0' a, PegDist Peg, Length Length, Width Depth, Height Height, ImageDate,    \r\n " +
                                       " SecManager +':'+ s2.Name ManagerSection, ToPeg2 PegTo, PegtoPegVal PegDist,  PegtoFLPVal PegFace, LHS LHS, RHS RHS, HW HW, FW FW,  \r\n " +
                                       "  CarrRailVal CarryRail, PegatChain ChainAtPeg,  PegatChainVal ChainAtPegVal, ChainatLP Chain, LPVal LP, PegReqRail PegToRail,  \r\n " +
                                       " ReqRailVal PegToRailVal,  RailVal Rail, PegSGradeVal1 SGrade1, PegSGradeVal2 SGrade2, PegSGradeVal3 SGrade3,  \r\n " +
                                       " PegSGradeVal4 SGrade4, PegStopDist StopDistPeg,  PegStopDistVal StopDistPegVal, PegVal PegVal, ToPeg2Face PegToFace,  \r\n " +
                                       "  AdvToDate AdvToDate, PegHoldDist HolDistPeg,  PegHoldDistVal HoldingDistPegVal, Grade Grade, PanelType PanelType*/ " +
                                       " '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner  \r\n " +
                                       " -- ,HolingNoteNo HolingNoteNo, Notes Notes, w.Description, s.Sectionid SID,s.Name SName,s1.Sectionid S1ID,s1.Name S1Name,s2.Sectionid S2ID,s2.Name S2Name, username    \r\n " +
                                       " from  tbl_SurveyNote t, vw_Workplace_Total w ,tbl_Section s, tbl_Section s1,tbl_Section s2   \r\n " +
                                       "  where TSNID = '" + 1 + "' and t.Workplaceid = w.Workplaceid  \r\n " +
                                       "  and t.Sectionid = s.Sectionid and s.prodmonth = (select CurrentProductionMonth from tbl_SysSet) and s.ReportToSectionid = s1.Sectionid and s1.prodmonth =   \r\n " +
                                       " (select CurrentProductionMonth from tbl_SysSet) and s1.ReportToSectionid = s2.Sectionid and s2.prodmonth = (select CurrentProductionMonth from tbl_SysSet)   \r\n " +


                                       " ) a \r\n " +

                                       " left outer join  [tbl_SurveyNote_Autorisation] b on a.id = b.TSNID \r\n " +

                                         //" left outer join [dbo].[tbl_Users_Attachments] s1 on b.lvl1user = s1.userid \r\n " +

                                         //" left outer join [dbo].[tbl_Users_Attachments] s2 on b.lvl2user = s2.username \r\n " +
                                         //" left outer join [dbo].[tbl_Users_Attachments] s3 on b.lvl3user = s3.username \r\n " +
                                         //" left outer join [dbo].[tbl_Users_Attachments] s4 on b.lvl4user = s4.username \r\n " +
                                         //" left outer join [dbo].[tbl_Users_Attachments] s5 on b.lvl5user = s5.username \r\n " +
                                         //" left outer join [dbo].[tbl_Users_Attachments] s6 on b.lvl6user = s6.username \r\n " +
                                         //" left outer join [dbo].[tbl_Users_Attachments] s7 on b.lvl7user = s7.username \r\n " +
                                         //" left outer join [dbo].[tbl_Users_Attachments] s8 on b.lvl8user = s8.username \r\n " +
                                         //" left outer join [dbo].[tbl_Users_Attachments] s9 on b.lvl9user = s9.username  \r\n " +

                                         //" left outer join [dbo].[tbl_Users] s10 on a.username = s10.username \r\n " +
                                         //" left outer join [dbo].[tbl_Users_Attachments] s11 on s10.userid = s11.username \r\n "+


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

                                       //" left outer join [dbo].[tbl_Users] us1 on b.lvl1user = us1.username \r\n " +
                                       //" left outer join [dbo].[tbl_Users] us2 on b.lvl2user = us2.username \r\n " +
                                       //" left outer join [dbo].[tbl_Users] us3 on b.lvl3user = us3.username \r\n " +
                                       //" left outer join [dbo].[tbl_Users] us4 on b.lvl4user = us4.username \r\n " +
                                       //" left outer join [dbo].[tbl_Users] us5 on b.lvl5user = us5.username \r\n " +
                                       //" left outer join [dbo].[tbl_Users] us6 on b.lvl6user = us6.username \r\n " +
                                       //" left outer join [dbo].[tbl_Users] us7 on b.lvl7user = us7.username \r\n " +
                                       //" left outer join [dbo].[tbl_Users] us8 on b.lvl8user = us8.username \r\n " +
                                       //" left outer join [dbo].[tbl_Users] us9 on b.lvl9user = us9.username \r\n " +
                                       //" left outer join [dbo].[tbl_Users] s10 on a.username = s10.username \r\n " +


                                       " left outer join [dbo].[tbl_Users_Attachments] s1 on us1.userid = s1.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s2 on us2.userid = s2.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s3 on us3.userid = s3.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s4 on us4.userid = s4.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s5 on us5.userid = s5.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s6 on us6.userid = s6.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s7 on us7.userid = s7.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s8 on us8.userid = s8.userid \r\n " +
                                       " left outer join [dbo].[tbl_Users_Attachments] s9 on us9.userid = s9.userid  \r\n " +

                                       "-- left outer join [dbo].[tbl_Users] us11 on a.username = us11.username \r\n " +
                                       "-- left outer join [dbo].[tbl_Users_Attachments] s11 on us11.userid = s11.userid \r\n " +



                                        "   ) a   ";




            //"Select '" + Peg + "','" + IDtxt.Text + "' ID, '" + date.Value.ToShortDateString() + "' Date, '" + SectionCmb.Text + "' Section, '" + WorkplaceCmb.Text + "' WP,  " +
            //                                    "'" + procs.ExtractBeforeColon(PegNoFrom1Cmb.Text) + "' + '+' + '" + PegDistTxt.Text + "' [PegDist], '" + DepthTxt.Text + "' Depth, '" + HeightTxt.Text + "' Height, " +
            //                                    "'" + LengthTxt.Text + "' Length, '" + RemarkTxt.Text + "' Remark, '" + ProductionAmplatsGlobalTProductionAmplatsGlobalT._Banner.ToString() + "' banner ";


            _dbManEmptyCubby.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby.ResultsTableName = "EmptyCubby";
            _dbManEmptyCubby.ExecuteInstruction();

            //RemBox.Text = _dbManEmptyCubby.SqlStatement;

            DataTable Neil = _dbManEmptyCubby.ResultsDataTable;

            string ImageDateLbl = string.Empty;   //Neil.Rows[0]["ImageDate"].ToString();

            MWDataManager.clsDataAccess _dbManEmptyCubby1 = new MWDataManager.clsDataAccess();
            _dbManEmptyCubby1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbManEmptyCubby1.SqlStatement = "Select '" + ProductionGlobalTSysSettings._RepDir + @"\SurveyLetters\" + ImageDateLbl + ".png" + "' ";


            _dbManEmptyCubby1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEmptyCubby1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEmptyCubby1.ResultsTableName = "Image";
            _dbManEmptyCubby1.ExecuteInstruction();


            DataSet dsEmpty = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty.Tables.Add(_dbManEmptyCubby.ResultsDataTable);

            //_theReport.RegisterData(dsEmpty);


            DataSet dsEmpty1 = new DataSet();
            // DataSet dsGraph = new DataSet();
            dsEmpty1.Tables.Add(_dbManEmptyCubby1.ResultsDataTable);

            _theReport.RegisterData(dsEmpty1);
            ////MessageBox.Show(GraphType);


            if (ProductionGlobalTSysSettings._Banner.ToString() == "Mponeng" || ProductionGlobalTSysSettings._Banner.ToString() == "Tau Tona" || ProductionGlobalTSysSettings._Banner.ToString() == "Savuka")
            {

                _theReport.Load(_reportFolder + "surveyNoteDevA3.frx");
            }
            else
            {
                _theReport.Load(_reportFolder + "surveyNote.frx");
            }
            //_theReport.Design();



            pcReport.Clear();
            _theReport.Prepare();
            _theReport.Preview = pcReport;
            _theReport.ShowPrepared();
            pcReport.Visible = true;
        }


        #endregion Methods 
    }
}
