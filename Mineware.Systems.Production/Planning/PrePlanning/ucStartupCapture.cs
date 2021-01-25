using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionHelp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Planning
{
    public partial class ucStartupCapture : BaseUserControl
    {
        StringBuilder SQLQuery = new StringBuilder();
        DialogResult Result;

        public ucStartupCapture()
        {
            InitializeComponent();
        }

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
            dsGlobal.Tables.Add(dtWorkplaceData);
            dsGlobal.Tables.Add(dtActions);
            dsGlobal.Tables.Add(dtData);
            dsGlobal.Tables.Add(dtAuth);
            dsGlobal.Tables.Add(dtDataEdit);
            dsGlobal.Tables.Add(dtDataEdit2);
            dsGlobal.Tables.Add(dtCycleData);
            dsGlobal.Tables.Add(dtEngEquipt);
            dsGlobal.Tables.Add(dtQuestions);
            dsGlobal.Tables.Add(dtSubQuestions);
            dsGlobal.Tables.Add(dtSave);
        }

        #region Datatables
        private DataTable dtWorkplaceData = new DataTable("dtWorkplaceData");
        private DataTable dtActions = new DataTable("dtActions");
        private DataTable dtData = new DataTable("dtData");
        private DataTable dtAuth = new DataTable("dtAuth");
        private DataTable dtDataEdit = new DataTable("dtDataEdit");
        private DataTable dtDataEdit2 = new DataTable("dtDataEdit2");
        private DataTable dtCycleData = new DataTable("dtCycleData");
        private DataTable dtSubQuestion = new DataTable("dtSubQuestion");
        private DataTable dtEngEquipt = new DataTable("dtEngEquipt");
        private DataTable dtQuestions = new DataTable("dtQuestions");
        private DataTable dtSubQuestions = new DataTable("dtSubQuestions");
        private DataTable dtSave = new DataTable("dtSave");
        private BindingSource bsMainQuestions = new BindingSource();
        private BindingSource bsAnswers = new BindingSource();
        #endregion

        #region DataSet
        public DataSet dsGlobal = new DataSet();
        #endregion

        #region Variables
        public String _FormType;
        public String _Crew;
        public String _WP;
        public String _WPID;
        public String _Section;
        public String _ProdMonth;
        public String _Activity;
        public String _Authorised;

        Boolean frmloaded;

        private String RepDir = string.Empty;
        private String RepDirDoc = string.Empty;
        private String ActionType = string.Empty;
        private String ActionDescription = string.Empty;
        private String ID = string.Empty;
        private String Workplace = string.Empty;
        private String Description = string.Empty;
        private String Recomendation = string.Empty;
        private String Priority = string.Empty;
        private String TargetDate = string.Empty;
        private String RespPerson = string.Empty;
        private String Overseer = string.Empty;

        private String SourceFile;
        private String DestinationFile;
        private String FileName = string.Empty;
        #endregion

        private void ucStartupCapture_Load(object sender, EventArgs e)
        {
            RepDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage;
            RepDirDoc = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage + @"\\Startups\\" + _FormType + "\\Documents";

            if (_FormType == "RockEng")
            {
                ActionType = "SUARE";
                ActionDescription = "Rock Engineering Actions";
            }

            if (_FormType == "Ventilation")
            {
                ActionType = "SUAV";
                ActionDescription = "Ventilation Actions";
            }

            if (_FormType == "Survey")
            {
                ActionType = "c";
                ActionDescription = "Survey Actions";
            }

            if (_FormType == "Safety")
            {
                ActionType = "SUAS";
                ActionDescription = "Safety Actions";
            }

            if (_FormType == "Geology")
            {
                ActionType = "SUAG";
                ActionDescription = "Geology Actions";
            }

            if (_FormType == "Mining")
            {
                ActionType = "SUAM";
                ActionDescription = "Mining Actions";
            }

            if (_FormType == "Planning")
            {
                ActionType = "SUAP";
                ActionDescription = "Planning Actions";
            }

            if (_FormType == "Department")
            {
                ActionType = "SUAD";
                ActionDescription = "Department Actions";
            }

            tbCrew.EditValue = _Crew;
            tbProdMonth.EditValue = _ProdMonth;

            tableRegister();
            LoadActions();
            LoadStartupData();
            LoadCycles();
            loadImage();
            loadDocs();
            LoadAuthorised();

            frmloaded = true;

            pnlStartup.Visible = true;
        }

        private void LoadStartupData()
        {
            string sql = string.Empty;
            if (_Activity == "Stoping")
            {
                sql = " declare @WPID varchar(10) \r\n "
                    + " set @WPID = (select WorkplaceID from tbl_Workplace where Description = '" + txtStartupWorkplace.EditValue.ToString() + "') \r\n "
                    + "EXEC sp_Startup_" + _FormType + "_Questions '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth)) + "', @WPID \r\n ";
            }

            if (_Activity == "Development")
            {
                sql = " declare @WPID varchar(10) \r\n "
                    + " set @WPID = (select WorkplaceID from tbl_Workplace where Description = '" + txtStartupWorkplace.EditValue.ToString() + "') \r\n "
                    + "EXEC sp_Startup_" + _FormType + "_Questions '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth)) + "', @WPID \r\n ";
            }

            gcStartup.DataSource = null;

            sqlConnector(sql, "dtData");

            gcStartup.DataSource = dtData;

            colQuestion.FieldName = "Question";
            colAnswer.FieldName = "YesNo";
            colComment.FieldName = "Comments";
            colQuestID.FieldName = "QuestID";
            colAction.FieldName = "Action";
            colDefaultAnswer.FieldName = "DefaultAnswer";
        }

        #region Procedures

        //Load Cycle
        void LoadCycles()
        {
            //Declarations
            string sql = string.Empty;

            sql = "Select * from [vw_Dept_Startup_Cycle] where workplaceid = '" + _WPID + "' " +
                    " and pm = (select max(pm) from [vw_Dept_Startup_Cycle]  where workplaceid = '" + _WPID + "' )  order by OrderNum,description ";

            gcCycle.DataSource = null;

            sqlConnector(sql, "dtCycleData");

            gcCycle.DataSource = dtCycleData;

            colWorkplace.FieldName = "description";
            colFL.FieldName = "FL";
            col1.FieldName = "day1";
            col2.FieldName = "day2";
            col3.FieldName = "day3";
            col4.FieldName = "day4";
            col5.FieldName = "day5";
            col6.FieldName = "day6";
            col7.FieldName = "day7";
            col8.FieldName = "day8";
            col9.FieldName = "day9";
            col10.FieldName = "day10";

            col11.FieldName = "day11";
            col12.FieldName = "day12";
            col13.FieldName = "day13";
            col14.FieldName = "day14";
            col15.FieldName = "day15";
            col16.FieldName = "day16";
            col17.FieldName = "day17";
            col18.FieldName = "day18";
            col19.FieldName = "day19";
            col20.FieldName = "day20";

            col21.FieldName = "day21";
            col22.FieldName = "day22";
            col23.FieldName = "day23";
            col24.FieldName = "day24";
            col25.FieldName = "day25";
            col26.FieldName = "day26";
            col27.FieldName = "day27";
            col28.FieldName = "day28";
            col29.FieldName = "day29";
            col30.FieldName = "day30";

            col31.FieldName = "day31";
            col32.FieldName = "day32";
            col33.FieldName = "day33";
            col34.FieldName = "day34";
            col35.FieldName = "day35";
            col36.FieldName = "day36";
            col37.FieldName = "day37";
            col38.FieldName = "day38";
            col39.FieldName = "day39";
            col40.FieldName = "day40";

            if (dtCycleData.Rows.Count > 0)
            {
                DateTime startdate = Convert.ToDateTime(dtCycleData.Rows[0]["BeginDate"].ToString());
                int columnIndex = 2;

                //Headers Date
                for (int i = 0; i < 40; i++)
                {
                    string test = gvCycle.Columns[columnIndex].Caption;


                    gvCycle.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM ddd");

                    startdate = startdate.AddDays(1);
                    columnIndex++;
                }
            }


            for (int i = 0; i < gvCycle.RowCount; i++)
            {
                string val = gvCycle.GetRowCellValue(i, gvCycle.Columns["day20"]).ToString();

                foreach (DataRow item in dtCycleData.Rows)
                {
                    if (val == string.Empty)
                    {
                        col20.Visible = false;
                        col21.Visible = false;
                        col22.Visible = false;
                        col23.Visible = false;
                        col24.Visible = false;
                        col25.Visible = false;
                        col26.Visible = false;
                        col27.Visible = false;
                        col28.Visible = false;
                        col29.Visible = false;

                        col30.Visible = false;
                        col31.Visible = false;
                        col32.Visible = false;
                        col33.Visible = false;
                        col34.Visible = false;
                        col35.Visible = false;
                        col36.Visible = false;
                        col37.Visible = false;
                        col38.Visible = false;
                        col39.Visible = false;
                        col40.Visible = false;
                    }
                }

            }

            for (int i = 0; i < gvCycle.RowCount; i++)
            {
                //int val1 = Convert.ToInt32(dtCycleData.Rows[0]["TotalShifts"].ToString());

                //for (int j = val1 + 3; j < 43; j++)
                //{
                //	gvCycle.Columns[j].Visible = false;
                //}
            }

        }

        //Gets Actions
        private void LoadActions()
        {
            //Declarations
            string sql = string.Empty;

            sql = " EXEC [sp_Startup_LoadActions] '" + txtStartupWorkplace.EditValue.ToString() + "' , '" + ActionType + "' ";

            gcActions.DataSource = null;

            sqlConnector(sql, "dtActions");

            gcActions.DataSource = dtActions;

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

        //Load Authorised
        private void LoadAuthorised()
        {
            //Declarations
            string sql = string.Empty;
            _Authorised = "Not Authorised";

            sql = " declare @WPID varchar(10) \r\n "
                + " set @WPID = (select WorkplaceID from tbl_Workplace where Description = '" + txtStartupWorkplace.EditValue.ToString() + "') \r\n "
                + " Select Max(" + _FormType + "Auth) DepAuth from [tbl_Startup_Main] \r\n "
                + "	where WorkplaceID = @WPID \r\n ";

            sqlConnector(sql, "dtAuth");

            if (dtAuth.Rows[0][0].ToString() != string.Empty && dtAuth.Rows.Count > 0)
            {
                btnStartupSave.Enabled = false;
                btnStartupAddImg.Enabled = false;
                btnStartupAddDoc.Enabled = false;

                btnStartupAddAct.Enabled = false;
                btnStartupEditAct.Enabled = false;
                btnStartupDeleteAct.Enabled = false;

                for (int col = 0; col < gvStartup.Columns.Count; col++)
                {
                    gvStartup.Columns[col].OptionsColumn.AllowEdit = false;
                }

                _Authorised = "Authorised";
                btnStartupAuth.Caption = "Unauthorise";
            }
        }

        ///Get Image
        public void GetFile()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Startups\\" + _FormType + string.Empty);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(RepDir + "\\Startups\\" + _FormType + string.Empty);

            if (Result == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                SourceFile = ofdOpenImageFile.FileName;

                index = ofdOpenImageFile.SafeFileName.IndexOf(".");

                if (txtStartupWorkplace.EditValue.ToString() != string.Empty)
                {
                    FileName = txtStartupWorkplace.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (_ProdMonth != string.Empty)
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth));
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = ofdOpenImageFile.SafeFileName.Substring(index);

                DestinationFile = RepDir + "\\Startups\\" + _FormType + string.Empty + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == ofdOpenImageFile.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            DestinationFile = RepDir + "\\Startups\\" + _FormType + string.Empty + "\\" + FileName + Ext;
                        }
                    }

                    try
                    {
                        System.IO.File.Copy(SourceFile, DestinationFile, true);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    System.IO.File.Copy(SourceFile, RepDir + "\\Startups\\" + _FormType + string.Empty + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(RepDir);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtStartupAttachment.Text = DestinationFile;
                PicBox.Image = Image.FromFile(ofdOpenImageFile.FileName);
            }
        }

        public void loadImage()
        {
            txtStartupAttachment.Text = string.Empty;
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Startups\\" + _FormType + string.Empty);

            var task = new Task<bool>(() => dir2.Exists);
            task.Start();

            if (task.Wait(20) && task.Result)
            {

                IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                string[] files = System.IO.Directory.GetFiles(RepDir + "\\Startups\\" + _FormType + string.Empty);

                foreach (var item in files)
                {
                    string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                    int extpos = aa.IndexOf(".");

                    string ext = aa.Substring(extpos, aa.Length - extpos);

                    if (item.ToString() == RepDir + "\\Startups\\" + _FormType + string.Empty + "\\" + txtStartupWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth)) + ext)
                    {
                        txtStartupAttachment.Text = item.ToString();
                    }
                }
                if (txtStartupAttachment.Text != string.Empty)
                {
                    using (FileStream stream = new FileStream(txtStartupAttachment.Text, FileMode.Open, FileAccess.Read))
                    {
                        PicBox.Image = System.Drawing.Image.FromStream(stream);
                        stream.Dispose();
                    }
                }
                else
                {
                    PicBox.Image = null;
                }
            }
        }

        //Get Document
        void GetDoc()
        {
            string mianDicrectory = RepDirDoc;

            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);
            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(mianDicrectory);

            if (Result == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                string SourcefileName = string.Empty;

                int index = 0;

                SourceFile = ofdOpenDocFile.FileName;

                index = ofdOpenDocFile.SafeFileName.IndexOf(".");

                SourcefileName = ofdOpenDocFile.SafeFileName.Substring(0, index);


                if (!txtStartupWorkplace.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + txtStartupWorkplace.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                if (!_ProdMonth.Equals(string.Empty))
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth));
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = ofdOpenDocFile.SafeFileName.Substring(index);

                DestinationFile = mianDicrectory + "\\" + FileName + SourcefileName + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == ofdOpenDocFile.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            DestinationFile = mianDicrectory + "\\" + FileName + Ext;
                        }
                    }
                    try
                    {
                        System.IO.File.Copy(SourceFile, DestinationFile, true);
                        DocsLB.Items.Add((SourcefileName + Ext));
                    }
                    catch { }
                }
                else
                {
                    System.IO.File.Copy(SourceFile, mianDicrectory + "\\" + FileName + SourcefileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(mianDicrectory);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }
            }
        }

        public void loadDocs()
        {

            Random r = new Random();

            string mianDicrectory = RepDirDoc;

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);

            var task = new Task<bool>(() => dir2.Exists);
            task.Start();

            if (task.Wait(200) && task.Result)
            {

                IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);


                string[] files = System.IO.Directory.GetFiles(mianDicrectory);

                //Do everywhere
                if (DocsLB.InvokeRequired)
                    DocsLB.Invoke(new Action(() => DocsLB.Items.Clear()));
                else
                    DocsLB.Items.Clear();

                foreach (var item in files)
                {
                    string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                    int extpos = aa.IndexOf(".");

                    string ext = aa.Substring(extpos, aa.Length - extpos);

                    int indexa = item.LastIndexOf("\\");

                    string sourcefilename = item.Substring(indexa + 1, (item.Length - indexa) - 1);

                    int indexprodmonth = sourcefilename.IndexOf(String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth)));

                    string SourcefileCheck = sourcefilename.Substring(0, indexprodmonth + 10);

                    int NameLength = sourcefilename.Length - SourcefileCheck.Length;

                    string Docsname = sourcefilename.Substring(SourcefileCheck.Length, NameLength);


                    if (SourcefileCheck == txtStartupWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth)))
                    {
                        if (DocsLB.InvokeRequired)
                            DocsLB.Invoke(new Action(() => DocsLB.Items.Add(Docsname.ToString())));
                        else
                            DocsLB.Items.Add(Docsname.ToString());
                    }
                }
            }



        }

        #endregion

        #region gvStartup Capture
        private void gvStartup_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //return;
            string Wp1Haz = "N";

            string answer = gvStartup.GetRowCellValue(e.RowHandle, "YesNo").ToString();
            string action = gvStartup.GetRowCellValue(gvStartup.FocusedRowHandle, gvStartup.Columns["Action"]).ToString();
            string Question = gvStartup.GetRowCellValue(gvStartup.FocusedRowHandle, gvStartup.Columns["Question"]).ToString();
            string DefaultAnswer = gvStartup.GetRowCellValue(gvStartup.FocusedRowHandle, gvStartup.Columns["DefaultAnswer"]).ToString();

            //If checked(Y), then Unchecked(N)
            if (e.Column.FieldName.ToString() == "YesNo" && (answer != DefaultAnswer))
            {
                foreach (DataRow dr in dtActions.Rows)
                {
                    if (dr["Description"].ToString() == Question)
                    {
                        Wp1Haz = "Y";
                    }
                }

                //If there is an action
                if (Wp1Haz == "N")
                {
                    frmDept_Actions frmAct = new frmDept_Actions();
                    frmAct.FormType = _FormType;
                    frmAct.AllowExit = "Y";
                    frmAct.lblItemResults.Text = Question;
                    frmAct.txtRemarks.Text = action;
                    frmAct.Type = ActionType;
                    frmAct.lblWPDesc.Text = txtStartupWorkplace.EditValue.ToString();
                    frmAct.UniqueDate = _ProdMonth.ToString();
                    frmAct.StartPosition = FormStartPosition.CenterScreen;
                    frmAct._theSystemDBTag = theSystemDBTag;
                    frmAct._UserCurrentInfo = UserCurrentInfo.Connection;
                    //frmAct.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                    frmAct.ShowDialog();

                    LoadActions();
                }
            }

            if (e.Column.FieldName.ToString() == "YesNo" && (answer == DefaultAnswer))
            {
                gvStartup.SetRowCellValue(gvStartup.FocusedRowHandle, gvStartup.Columns["Comments"], string.Empty);
            }
        }
        #endregion

        #region gcCycle
        private void gvCycle_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.CellValue.ToString().Trim() == "BL" || e.CellValue.ToString().Trim() == "SR"
                || e.CellValue.ToString().Trim() == "SUBL")
            {
                e.Appearance.BackColor = Color.MistyRose;
            }

            if (e.CellValue.ToString().Trim() == "OFF")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }
        }
        #endregion

        #region gcAction

        private void gvAction_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            ID = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["ID"]).ToString();
            Workplace = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Workplace"]).ToString();
            Description = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Description"]).ToString();
            Recomendation = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Action"]).ToString();
            TargetDate = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["TargetDate"]).ToString();
            Priority = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Priority"]).ToString();
            FileName = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["CompNotes"]).ToString();

            RespPerson = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["RespPerson"]).ToString();
            Overseer = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["HOD"]).ToString();
        }

        #endregion

        #region Events

        private void txtStartupCalDate_EditValueChanged(object sender, EventArgs e)
        {
            LoadStartupData();
        }

        private void DocsLB_DoubleClick(object sender, EventArgs e)
        {
            string mianDicrectory = RepDirDoc;
            if (DocsLB.SelectedIndex != -1)
            {
                string test = mianDicrectory + "\\" + txtStartupWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth)) + DocsLB.SelectedItem.ToString();

                System.Diagnostics.Process.Start(mianDicrectory + "\\" + txtStartupWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth)) + DocsLB.SelectedItem.ToString());
            }
        }

        private void btnStartupSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gvStartup.PostEditor();

            MWDataManager.clsDataAccess _StartupSave = new MWDataManager.clsDataAccess();
            _StartupSave.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            string sql = " declare @WPID varchar(10) \r\n "
                + " set @WPID =  '" + _WPID + "' \r\n " +
                " delete from tbl_Startup_" + _FormType + "_Capture \r\n " +
                " where CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth)) + "' and \r\n " +
                " WorkplaceID = @WPID \r\n";

            for (int row = 0; row < gvStartup.RowCount; row++)
            {
                //Declarations
                string quetID = gvStartup.GetRowCellValue(row, gvStartup.Columns["QuestID"]).ToString();
                string answer = gvStartup.GetRowCellValue(row, gvStartup.Columns["YesNo"]).ToString();
                string comm = gvStartup.GetRowCellValue(row, gvStartup.Columns["Comments"]).ToString();

                sql += " insert into [tbl_Startup_" + _FormType + "_Capture] (Calendardate, SectionID, CrewID, WorkplaceID, QuestID, Answer, Comments) \r\n "
                        + " values('" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth)) + "','" + _Section + "', \r\n "
                        + " '" + _Crew + "', @WPID, '" + quetID + "','" + answer + "', \r\n"
                        + " '" + comm + "' ) \r\n ";
            }

            sql += " update [tbl_Startup_Main] \r\n "
                + " set " + _FormType + "Capt = '" + TUserInfo.UserID + "' \r\n "
                + " where WorkplaceID = @WPID";

            _StartupSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _StartupSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _StartupSave.SqlStatement = sql;

            var ActionResult = _StartupSave.ExecuteInstruction();

            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", _FormType + " Captured", Color.CornflowerBlue);
            }
            else
            {
                Global.sysNotification.TsysNotification.showNotification("Save Failed", _FormType + " not captured", Color.Red);
            }
        }

        private void btnStartupAddImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ofdOpenImageFile.InitialDirectory = folderBrowserDialog1.SelectedPath;
            ofdOpenImageFile.FileName = null;
            ofdOpenImageFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            Result = ofdOpenImageFile.ShowDialog();

            GetFile();
        }

        private void btnStartupAddDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Result = ofdOpenDocFile.ShowDialog();
            GetDoc();
        }

        private void btnStartupAuth_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.SafetyStartUpAuth))
            {
                MessageBox.Show("You don't Have rights to Authorise,Please Contact you Administrator");
                return;
            }

            if (btnStartupAuth.Caption == "Unauthorise")
            {
                btnStartupAuth.Caption = "Authorise";
                btnStartupSave.Enabled = true;
                btnStartupAddImg.Enabled = true;
                btnStartupAddDoc.Enabled = true;
                btnStartupAddAct.Enabled = true;
                btnStartupEditAct.Enabled = true;
                btnStartupDeleteAct.Enabled = true;

                for (int col = 0; col < gvStartup.Columns.Count; col++)
                {
                    gvStartup.Columns[col].OptionsColumn.AllowEdit = true;
                }

                MWDataManager.clsDataAccess _StartupAuth = new MWDataManager.clsDataAccess();
                _StartupAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _StartupAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _StartupAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _StartupAuth.SqlStatement = " declare @WPID varchar(10) \r\n "
                             + " set @WPID = (select WorkplaceID from tbl_Workplace where Description = '" + txtStartupWorkplace.EditValue.ToString() + "') \r\n "
                             + " update [tbl_Startup_Main] \r\n "
                             + "	set " + _FormType + "Auth = '' \r\n "
                             + "	where WorkplaceID = @WPID \r\n ";
                var result = _StartupAuth.ExecuteInstruction();
                if (result.success)
                {
                    LoadAuthorised();
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Unauthorised", Color.CornflowerBlue);
                }
            }
            else
            {
                btnStartupSave_ItemClick(null, null);

                btnStartupSave.Enabled = false;
                btnStartupAddImg.Enabled = false;
                btnStartupAddDoc.Enabled = false;
                btnStartupAddAct.Enabled = false;
                btnStartupEditAct.Enabled = false;
                btnStartupDeleteAct.Enabled = false;

                for (int col = 0; col < gvStartup.Columns.Count; col++)
                {
                    gvStartup.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvAction.Columns.Count; col++)
                {
                    gvAction.Columns[col].OptionsColumn.AllowEdit = false;
                }
                btnStartupAuth.Caption = "Unauthorise";

                MWDataManager.clsDataAccess _StartupAuth = new MWDataManager.clsDataAccess();
                _StartupAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _StartupAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _StartupAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _StartupAuth.SqlStatement = " declare @WPID varchar(10) \r\n "
                                            + " set @WPID = (select WorkplaceID from tbl_Workplace where Description = '" + txtStartupWorkplace.EditValue.ToString() + "') \r\n "
                                            + " update [tbl_Startup_Main] \r\n "
                                            + "	set " + _FormType + "Auth = '" + TUserInfo.UserID.ToString() + "' \r\n "
                                            + "	where WorkplaceID = @WPID  \r\n ";
                var result = _StartupAuth.ExecuteInstruction();
                if (result.success)
                {
                    LoadAuthorised();
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Authorised", Color.CornflowerBlue);
                }
            }
        }

        private void btnStartupReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ucStartupsReport startUps = new ucStartupsReport();
            startUps._DepartmentName = _FormType;
            startUps._ActionType = ActionType;
            startUps._Authorised = _Authorised;
            startUps._prodMonth = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth));
            startUps._CalDate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(txtStartupCalDate.EditValue));
            startUps._ucWorkPlace = txtStartupWorkplace.EditValue.ToString();
            startUps._myActivity = _Activity;
            startUps.theSystemDBTag = theSystemDBTag;
            startUps.UserCurrentInfo = UserCurrentInfo;

            Form ucDept_Capt = new Form();
            ucDept_Capt.Controls.Clear();
            ucDept_Capt.WindowState = FormWindowState.Maximized;
            ucDept_Capt.Controls.Add(startUps);
            startUps.Dock = DockStyle.Fill;
            ucDept_Capt.Text = _FormType + " Startup Report";
            try
            {
                //ucDept_Capt.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            }
            catch { }

            ucDept_Capt.ShowDialog();
        }

        private void btnStartupClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnStartupAddAct_Click(object sender, EventArgs e)
        {
            frmDept_Actions frmAct = new frmDept_Actions();
            frmAct.FormType = _FormType;
            frmAct.AllowExit = "Y";
            frmAct.lblItemResults.Text = _FormType + " StartUp Action";
            frmAct.Type = ActionType;
            frmAct.lblWPDesc.Text = txtStartupWorkplace.EditValue.ToString();
            frmAct.UniqueDate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth));
            frmAct.StartPosition = FormStartPosition.CenterScreen;
            frmAct._theSystemDBTag = theSystemDBTag;
            frmAct._UserCurrentInfo = UserCurrentInfo.Connection;
            //frmAct.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            frmAct.ShowDialog();

            LoadActions();
        }

        private void btnStartupEditAct_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");
                return;
            }

            frmDept_Actions ActFrm = new frmDept_Actions();
            ActFrm.FormType = _FormType;
            ActFrm.Type = ActionType;
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";
            ActFrm.lblWPDesc.Text = Workplace;
            ActFrm.lblItemResults.Text = Description;
            ActFrm.UniqueDate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(_ProdMonth));
            ActFrm.txtRemarks.Text = Recomendation;
            ActFrm.txtReqDate.Text = TargetDate;
            ActFrm.PriorityCmb.Text = Priority;
            ActFrm.RespPersonCmb.EditValue = RespPerson;
            ActFrm.OverseerCmb.EditValue = Overseer;

            ActFrm.ActID = ID;

            ActFrm._theSystemDBTag = theSystemDBTag;
            ActFrm._UserCurrentInfo = UserCurrentInfo.Connection;

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        private void btnStartupDeleteAct_Click(object sender, EventArgs e)
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
            _DeleteAction.SqlStatement = " Delete from tbl_Shec_Incidents where ID = '" + ID + "'   \r\n";

            _DeleteAction.ExecuteInstruction();

            LoadActions();
        }

        private void repStartupYesNo_CheckedChanged(object sender, EventArgs e)
        {
            gvStartup.PostEditor();
        }


        #endregion

        private void lcgSafetyDocs_Click(object sender, EventArgs e)
        {

        }

        private void lcgSubDocSafety_Click(object sender, EventArgs e)
        {

        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWordEditor helpFrm = new frmWordEditor();
            helpFrm.ViewType = "View";
            helpFrm.MainCat = "Startups";
            helpFrm.SubCat = "Startups";
            helpFrm.Show();
        }
    }
}
