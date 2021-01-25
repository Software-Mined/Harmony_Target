using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class ucSDBPlanning : BaseUserControl
    {
        public ucSDBPlanning()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpSDB_Planning);
            FormActiveRibbonPage = rpSDB_Planning;
            FormMainRibbonPage = rpSDB_Planning;
            RibbonControl = rcSDBPlanning;
        }

        #region Private variables
        DialogResult result;
        OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        string CanEdit = string.Empty;
        string CanDelete = string.Empty;
        string CanEditAuth = "N";
        string lblAllowAuth = "N";
        string sourceFile;
        string destinationFile;
        string MainDirectory = string.Empty;
        string endcolumn = "1";
        string Authid = string.Empty;
        string lblPrevStarton;

        DataTable dtBgwGang = new DataTable();
        DataTable dtBgwWorkplace = new DataTable();
        DataTable dtBgwActivity = new DataTable();
        DataTable dtBgwMiner = new DataTable();
        #endregion

        private void barbtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbManRights = new MWDataManager.clsDataAccess();
            _dbManRights.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManRights.SqlStatement = " select isnull(SDB_Plan,0) SDB_Plan from tbl_Users_Synchromine where UserID =  '" + TUserInfo.UserID + "' ";
            _dbManRights.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRights.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRights.ExecuteInstruction();
            if (_dbManRights.ResultsDataTable.Rows.Count > 0)
            {
                if (_dbManRights.ResultsDataTable.Rows[0][0].ToString() == "0")
                {
                    MessageBox.Show("You don't Have rights to Plan,Please Contact you Administrator");
                    return;
                }
            }

            if (bgwAddData.IsBusy)
            {
                MessageBox.Show("System is Loading data,Please try again in a few seconds");
                return;
            }

            frmSDBAddAuthorise Serv1Frm = new frmSDBAddAuthorise();
            Serv1Frm.StartPosition = FormStartPosition.CenterScreen;
            Serv1Frm.lblFrmtype = "Add";
            Serv1Frm.dtGang = dtBgwGang;
            Serv1Frm.dtActivity = dtBgwActivity;
            Serv1Frm.dtWorkplace = dtBgwWorkplace;
            Serv1Frm.dtMinerRec = dtBgwMiner;
            Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            Serv1Frm.ShowDialog();

            int focusedRow = gvAuth.FocusedRowHandle;
            gvAuth.BeginDataUpdate();
            LoadAuthGrid();
            gvAuth.EndDataUpdate();

            gvAuth.FocusedRowHandle = focusedRow;
            gvAuth.SetMasterRowExpanded(focusedRow, true);

            //Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);
        }

        private void ucSDBPlanning_Load(object sender, EventArgs e)
        {
            gcAuth.Visible = false;

            ProdMonthTxt.Text = Convert.ToString(ProductionGlobalTSysSettings._currentProductionMonth);
            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(ProdMonthTxt.Text));
            ProdMonth1Txt.Text = ProductionGlobal.ProductionGlobal.Prod2;

            bgwAddData.RunWorkerAsync();

            LoadWorkplaceDropDown();

            if (Security.SecurityPAS.HasPermissionSDB(Security.SDBPermissions.Authorise))
            {
                lblAllowAuth = "Y";
                CanEditAuth = "Y";
                CanEdit = "Y";
            }

            if (Security.SecurityPAS.HasPermissionSDB(Security.SDBPermissions.Plan))
            {
                CanEdit = "Y";
            }

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " Select * from tbl_SDB_Users where userid = '" + UserCurrentInfo.UserID + "'   ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Authorise"].ToString() == "Y")
                {
                    CanEdit = "Y";
                }
            }

            LoadAuthGrid();
        }

        private void LoadWorkplaceDropDown()
        {
            MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
            _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbManMain.SqlStatement = "Select Workplace from tbl_SDB_Activity_Authorisation group by Workplace";
            _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMain.ExecuteInstruction();

            DataTable dtWp = _dbManMain.ResultsDataTable;

            repSleWorkplace.DataSource = dtWp;
            repSleWorkplace.ValueMember = "Workplace";
            repSleWorkplace.DisplayMember = "Workplace";
            repSleWorkplace.PopulateViewColumns();
        }

        private void LoadAuthGrid()
        {
            if (barWorkplace.EditValue == null)
            {
                //return;
            }

            MWDataManager.clsDataAccess _dbManMain1 = new MWDataManager.clsDataAccess();
            _dbManMain1.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbManMain1.SqlStatement = "select min(begindate) mm, max(enddate) ee from tbl_Code_Calendar_Section where prodmonth = '" + ProdMonthTxt.Value + "' ";
            _dbManMain1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMain1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMain1.ExecuteInstruction();

            DataTable dt1 = _dbManMain1.ResultsDataTable;

            DateTime StartDate = Convert.ToDateTime(_dbManMain1.ResultsDataTable.Rows[0]["mm"].ToString());
            DateTime EndDate = Convert.ToDateTime(_dbManMain1.ResultsDataTable.Rows[0]["ee"].ToString());

            int grid6FocRow = gvAuth.FocusedRowHandle;

            if (grid6FocRow < 0)
            {
                grid6FocRow = 0;
            }

            gcAuth.Refresh();

            MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
            _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);

            _dbManMain.SqlStatement = "  select * from ( select *, enddate-5 newend from ( Select *, year(startdate)*100 +month(startdate) mmm  , substring(minerid,1,4) mo from ( \r\n" +
                                     " Select a.*,    \r\n" +
                                     " case when docshortcut = '' then 'N' else 'Y' end as DocIsAttached from (    \r\n" +
                                     " Select a.*,b.Description from (   \r\n" +
                                     " Select * from tbl_SDB_Activity_Authorisation where authid not in (SELECT [Exclusion] FROM [dbo].[tbl_Exclusions_SDB]) ) a   \r\n" +
                                     " left outer join (Select * from tbl_SDB_Activity)b on a.MainActID = b.MainActID ) a  )a   \r\n" +
                                     " left outer join   \r\n" +
                                     " (Select name EmployeeName, substring(Sectionid , 0, 9) EmployeeNum from tbl_SDB_SECTION where prodmonth = (Select max(prodmonth) from tbl_SDB_SECTION))b on a.minerid = b.EmployeeNum ) a    \r\n" +

                                    " left outer join  \r\n" +
                                    " (  select authid aaaaa, max(calendardate) NewEE from [dbo].[tbl_SDB_Activity_PlanDay]  \r\n" +
                                    " group by authid) xxxx on a.authid = xxxx.aaaaa  \r\n" +
                                    // " where workplace = '" + barWorkplace.EditValue.ToString() + "' \r\n" +
                                    ")  tbl1   ";

            _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMain.ExecuteInstruction();

            DataTable dt = _dbManMain.ResultsDataTable;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " Select a.*, isnull(b.[Description],'DQlick to Add New Task') Task from (  \r\n" +
                                  " Select * from vw_SDB_PlanDay_Tasks where authid not in (SELECT [Exclusion] FROM [dbo].[tbl_Exclusions_SDB])  )a  \r\n" +
                                  " left outer join   \r\n" +
                                  " (Select [Description],SubActID from tbl_SDB_SubActivity ) b on a.subact = b.SubActID  \r\n" +
                                  " where a.authid in (Select authid from tbl_SDB_Activity_Authorisation " +

                                  //where workplace = '"+barWorkplace.EditValue.ToString()+"' 


                                  " group by authid ) \r\n" +
                                  "order by a.authid, ord, right('00000'+starton,4), order1, dur  \r\n";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtMain = _dbMan.ResultsDataTable;

            DataSet ds = new DataSet();

            ds.Tables.Add(dt);
            ds.Tables.Add(dtMain);

            ds.Relations.Clear();

            DataColumn keyColumn1 = ds.Tables[0].Columns[0];
            DataColumn foreignKeyColumn1 = ds.Tables[1].Columns[0];
            ds.Relations.Add("TaskDetail", keyColumn1, foreignKeyColumn1);

            gcAuth.DataSource = ds.Tables[0];
            gcAuth.LevelTree.Nodes.Add("TaskDetail", bandedGridView5);

            colAuthID.FieldName = "AuthID";
            colCrew.FieldName = "Crew";
            colMiner.FieldName = "EmployeeName";
            colActDescription.FieldName = "Description";
            colWorkplaceAuth.FieldName = "Workplace";
            colStartDate.FieldName = "StartDate";
            colEndDate.FieldName = "EndDate";
            colDurationAuth.FieldName = "Duration";
            colAuthorise.FieldName = "Authorise";
            colUserID.FieldName = "UserID";
            colAuthDate.FieldName = "AuthDate";
            colDocument.FieldName = "DocIsAttached";
            ColMO1.FieldName = "mo";
            ColMonth.FieldName = "mmm";

            colEndDate.Visible = false;
            colEndDate1.FieldName = "NewEE";

            colSubAuthID.FieldName = "authid";
            colASubact.FieldName = "Task";
            colStarton.FieldName = "Starton";
            colSS.FieldName = "ss";
            colEE.FieldName = "ee";
            colLastDay.FieldName = "nn";
            colADur.FieldName = "dur";
            colADay1.FieldName = "Day1";
            colADay2.FieldName = "Day2";
            colADay3.FieldName = "Day3";
            colADay4.FieldName = "Day4";
            colADay5.FieldName = "Day5";
            colADay6.FieldName = "Day6";
            colADay7.FieldName = "Day7";
            colADay8.FieldName = "Day8";
            colADay9.FieldName = "Day9";
            colADay10.FieldName = "Day10";
            colADay11.FieldName = "Day11";
            colADay12.FieldName = "Day12";
            colADay13.FieldName = "Day13";
            colADay14.FieldName = "Day14";
            colADay15.FieldName = "Day15";
            colADay16.FieldName = "Day16";
            colADay17.FieldName = "Day17";
            colADay18.FieldName = "Day18";
            colADay19.FieldName = "Day19";
            colADay20.FieldName = "Day20";
            colADay21.FieldName = "Day21";
            colADay22.FieldName = "Day22";
            colADay23.FieldName = "Day23";
            colADay24.FieldName = "Day24";
            colADay25.FieldName = "Day25";
            colADay26.FieldName = "Day26";
            colADay27.FieldName = "Day27";
            colADay28.FieldName = "Day28";
            colADay29.FieldName = "Day29";
            colADay30.FieldName = "Day30";

            colADay31.FieldName = "Day31";
            colADay32.FieldName = "Day32";
            colADay33.FieldName = "Day33";
            colADay34.FieldName = "Day34";
            colADay35.FieldName = "Day35";
            colADay36.FieldName = "Day36";
            colADay37.FieldName = "Day37";
            colADay38.FieldName = "Day38";
            colADay39.FieldName = "Day39";
            colADay40.FieldName = "Day40";

            colADay41.FieldName = "Day41";
            colADay42.FieldName = "Day42";
            colADay43.FieldName = "Day43";
            colADay44.FieldName = "Day44";
            colADay45.FieldName = "Day45";

            gcAuth.DataSource = null;
            gcAuth.DataSource = ds.Tables[0];

            gvAuth.FocusedRowHandle = grid6FocRow;

            bandedGridView5.CloseEditor();

            gcAuth.Visible = true;
        }

        private void gvAuth_DoubleClick(object sender, EventArgs e)
        {
            if (gvAuth.RowCount < 0)
            {
                return;
            }

            if (bgwAddData.IsBusy)
            {
                MessageBox.Show("System is Loading data,Please try again in a few seconds");
                return;
            }

            string Auth = string.Empty;

            if (gvAuth.FocusedColumn.FieldName == "DocIsAttached")
                Auth = "N";

            if (gvAuth.FocusedColumn.FieldName == "Authorise")
                Auth = "N";


            if (Auth != "N")
            {
                if (CanEdit == "N")
                {
                    MessageBox.Show("Not Allowed to Edit");
                    return;
                }


                if (gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Authorise"]).ToString() == "Y")
                {
                    if (CanEditAuth != "Y")
                    {
                        MessageBox.Show("Not Allowed to Edit,Already Authorised");
                        return;
                    }
                }

                frmSDBAddAuthorise Serv1Frm = new frmSDBAddAuthorise();
                Serv1Frm.StartPosition = FormStartPosition.CenterScreen;
                Serv1Frm.lblFrmtype = "Edit";
                Serv1Frm.lblCrew = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Crew"]).ToString();
                Serv1Frm.lblAuthID = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString();
                Serv1Frm.lblActivity = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Description"]).ToString();
                Serv1Frm.lblWorkplace = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Workplace"]).ToString();
                Serv1Frm.lblMiner = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["EmployeeName"]).ToString();
                Serv1Frm.DPStartdate.Value = Convert.ToDateTime(gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["StartDate"]));
                Serv1Frm.DPEnddate.Value = Convert.ToDateTime(gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["EndDate"]));
                Serv1Frm.DPOrigdate.Value = Convert.ToDateTime(gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["StartDate"]));
                Serv1Frm.dtActivity = dtBgwActivity;
                Serv1Frm.dtWorkplace = dtBgwWorkplace;
                Serv1Frm.dtGang = dtBgwGang;
                Serv1Frm.dtMinerRec = dtBgwMiner;
                Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
                Serv1Frm.ShowDialog();

                int focusedRow = gvAuth.FocusedRowHandle;

                bool expanded = gvAuth.GetMasterRowExpanded(focusedRow);

                gvAuth.BeginDataUpdate();
                LoadAuthGrid();
                gvAuth.EndDataUpdate();

                gvAuth.FocusedRowHandle = focusedRow;

                gvAuth.SetMasterRowExpanded(focusedRow, expanded);

                Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);

                gcAuth.Refresh();
            }

            if (gvAuth.FocusedColumn.FieldName == "DocIsAttached")
            {
                //openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
                //openFileDialog1.FileName = null;
                //result = openFileDialog1.ShowDialog();

                GetFile();
            }

            if (gvAuth.FocusedColumn.FieldName == "Authorise")
            {
                if (lblAllowAuth == "N")
                {
                    MessageBox.Show("Not Allowed to Authorise Planning");
                    return;
                }

                frmSDBAuthorise Serv1Frm = new frmSDBAuthorise();
                Serv1Frm.StartPosition = FormStartPosition.CenterScreen;
                Serv1Frm.lblAuthID = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString();
                Serv1Frm.lblMainAct = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Description"]).ToString();
                Serv1Frm.lblWP = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Workplace"]).ToString();
                Serv1Frm.lblUserID = TUserInfo.UserID.ToString();
                Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
                Serv1Frm.ShowDialog();


                int focusedRow = gvAuth.FocusedRowHandle;
                gvAuth.BeginDataUpdate();
                LoadAuthGrid();
                gvAuth.EndDataUpdate();

                gvAuth.FocusedRowHandle = focusedRow;
                gvAuth.SetMasterRowExpanded(focusedRow, true);

                Global.sysNotification.TsysNotification.showNotification("Data Authorised", "Record Authorised Succesfully", Color.CornflowerBlue);
            }

            gcAuth.Refresh();
        }

        private void GetFile()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(MainDirectory);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(MainDirectory);

            if (result == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                string FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialog1.FileName;

                index = openFileDialog1.SafeFileName.IndexOf(".");
                FileName = openFileDialog1.SafeFileName.Substring(0, index);
                Ext = openFileDialog1.SafeFileName.Substring(index);

                destinationFile = MainDirectory + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = MainDirectory + "\\" + FileName + Ext;//+ FileName + Name.ToString() + Ext
                        }

                    }
                    System.IO.File.Copy(sourceFile, destinationFile, true);

                    Global.sysNotification.TsysNotification.showNotification("File Attached", "File Attached Succesfully", Color.CornflowerBlue);
                }
                else
                {
                    System.IO.File.Copy(sourceFile, destinationFile, true);

                    Global.sysNotification.TsysNotification.showNotification("File Attached", "File Attached Succesfully", Color.CornflowerBlue);

                    dir2 = new System.IO.DirectoryInfo(MainDirectory);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
                _dbManMain.SqlStatement = " update tbl_SDB_Activity_Authorisation set DocShortCut = '" + destinationFile + "' \r\n" +
                                          " where AuthID = '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString() + "' and MainActID = (Select MainActID from tbl_SDB_Activity where Description =  '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Description"]).ToString() + "') and Workplace = '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Workplace"]).ToString() + "'  ";

                _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMain.ExecuteInstruction();

                LoadAuthGrid();
            }

        }

        private void bandedGridView5_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView View1 = sender as GridView;

            Rectangle r = e.Bounds;
            Graphics g = e.Graphics;

            Brush hb = Brushes.Gray;

            string ss = string.Empty;

            endcolumn = View1.GetRowCellValue(e.RowHandle, View1.Columns["nn"]).ToString();

            if (Convert.ToInt32(endcolumn) < 2)
            {
                View1.Columns["Day2"].Visible = false;
                View1.Columns["Day3"].Visible = false;
                View1.Columns["Day4"].Visible = false;
                View1.Columns["Day5"].Visible = false;
                View1.Columns["Day6"].Visible = false;
                View1.Columns["Day7"].Visible = false;
                View1.Columns["Day8"].Visible = false;
                View1.Columns["Day9"].Visible = false;
                View1.Columns["Day10"].Visible = false;
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;


                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 3)
            {
                View1.Columns["Day3"].Visible = false;
                View1.Columns["Day4"].Visible = false;
                View1.Columns["Day5"].Visible = false;
                View1.Columns["Day6"].Visible = false;
                View1.Columns["Day7"].Visible = false;
                View1.Columns["Day8"].Visible = false;
                View1.Columns["Day9"].Visible = false;
                View1.Columns["Day10"].Visible = false;
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 4)
            {
                View1.Columns["Day4"].Visible = false;
                View1.Columns["Day5"].Visible = false;
                View1.Columns["Day6"].Visible = false;
                View1.Columns["Day7"].Visible = false;
                View1.Columns["Day8"].Visible = false;
                View1.Columns["Day9"].Visible = false;
                View1.Columns["Day10"].Visible = false;
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 5)
            {
                View1.Columns["Day5"].Visible = false;
                View1.Columns["Day6"].Visible = false;
                View1.Columns["Day7"].Visible = false;
                View1.Columns["Day8"].Visible = false;
                View1.Columns["Day9"].Visible = false;
                View1.Columns["Day10"].Visible = false;
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 6)
            {
                View1.Columns["Day6"].Visible = false;
                View1.Columns["Day7"].Visible = false;
                View1.Columns["Day8"].Visible = false;
                View1.Columns["Day9"].Visible = false;
                View1.Columns["Day10"].Visible = false;
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 7)
            {
                View1.Columns["Day7"].Visible = false;
                View1.Columns["Day8"].Visible = false;
                View1.Columns["Day9"].Visible = false;
                View1.Columns["Day10"].Visible = false;
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 8)
            {
                View1.Columns["Day8"].Visible = false;
                View1.Columns["Day9"].Visible = false;
                View1.Columns["Day10"].Visible = false;
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 9)
            {
                View1.Columns["Day9"].Visible = false;
                View1.Columns["Day10"].Visible = false;
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }


            if (Convert.ToInt32(endcolumn) < 10)
            {
                View1.Columns["Day10"].Visible = false;
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 11)
            {
                View1.Columns["Day11"].Visible = false;
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 12)
            {
                View1.Columns["Day12"].Visible = false;
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 13)
            {
                View1.Columns["Day13"].Visible = false;
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 14)
            {
                View1.Columns["Day14"].Visible = false;
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 15)
            {
                View1.Columns["Day15"].Visible = false;
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 16)
            {
                View1.Columns["Day16"].Visible = false;
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 17)
            {
                View1.Columns["Day17"].Visible = false;
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 18)
            {
                View1.Columns["Day18"].Visible = false;
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 19)
            {
                View1.Columns["Day19"].Visible = false;
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 20)
            {
                View1.Columns["Day20"].Visible = false;
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 21)
            {
                View1.Columns["Day21"].Visible = false;
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 22)
            {
                View1.Columns["Day22"].Visible = false;
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 23)
            {
                View1.Columns["Day23"].Visible = false;
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 24)
            {
                View1.Columns["Day24"].Visible = false;
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 25)
            {
                View1.Columns["Day25"].Visible = false;
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 26)
            {
                View1.Columns["Day26"].Visible = false;
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 27)
            {
                View1.Columns["Day27"].Visible = false;
                View1.Columns["Day28"].Visible = false;
                View1.Columns["Day29"].Visible = false;
                View1.Columns["Day30"].Visible = false;

                View1.Columns["Day31"].Visible = false;
                View1.Columns["Day32"].Visible = false;
                View1.Columns["Day33"].Visible = false;
                View1.Columns["Day34"].Visible = false;
                View1.Columns["Day35"].Visible = false;
                View1.Columns["Day36"].Visible = false;
                View1.Columns["Day37"].Visible = false;
                View1.Columns["Day38"].Visible = false;
                View1.Columns["Day39"].Visible = false;
                View1.Columns["Day40"].Visible = false;
                View1.Columns["Day41"].Visible = false;
                View1.Columns["Day42"].Visible = false;
                View1.Columns["Day43"].Visible = false;
                View1.Columns["Day44"].Visible = false;
                View1.Columns["Day45"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 28)
            {
                View1.Columns["Day28"].Visible = false; View1.Columns["Day29"].Visible = false; View1.Columns["Day30"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 29)
            {
                View1.Columns["Day29"].Visible = false; View1.Columns["Day30"].Visible = false;
            }

            if (Convert.ToInt32(endcolumn) < 30)
            {
                View1.Columns["Day30"].Visible = false;
            }

            if (View1.GetRowCellValue(e.RowHandle, e.Column) != null)
            {
                ss = View1.GetRowCellValue(e.RowHandle, e.Column).ToString();
            }

            if (e.RowHandle > 1 && e.Column.FieldName == "Day1")
            {
                g.FillRectangle(hb, new Rectangle(r.Left - 3, r.Top - 1, 2, r.Height + 8));
            }

            if (e.Column.FieldName == "ss" || e.Column.FieldName == "ee")
            {
                e.Appearance.BackColor = Color.White;
                e.Appearance.ForeColor = Color.White;
            }

            if (ss == "Y")
            {
                e.Appearance.BackColor = Color.Tomato;
                e.Appearance.ForeColor = Color.Tomato;
            }

            if (ss == "OFF")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }

            if (ss == "DQlick to Add New Task" && e.RowHandle == 0)
            {
                e.Appearance.ForeColor = Color.Gainsboro;
            }

            if (e.RowHandle == 0 || e.RowHandle == 1)
            {
                e.Appearance.BackColor = Color.Gainsboro;
            }


        }

        private void bandedGridView5_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView View1 = sender as GridView;

            string ss = View1.GetRowCellValue(View1.FocusedRowHandle, View1.Columns["authid"]).ToString();

            for (int i = 0; i < gvAuth.RowCount; i++)
            {
                if (gvAuth.GetRowCellValue(i, gvAuth.Columns["AuthID"]).ToString() == ss)
                {
                    gvAuth.FocusedRowHandle = i;
                }
            }

            if (e.Column.FieldName == "Starton")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " exec  sp_SDB_UpdateProjectEnd  '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString() + "'         update tbl_SDB_Activity_PlanSum set  Starton = '" + View1.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString() + "' , Processed = 'N' \r\n" +
                                      " where AuthID = '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString() + "' \r\n" +
                                      " and Workplace = '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Workplace"]).ToString() + "' \r\n" +
                                      " and MainActID = (Select MainActID from tbl_SDB_Activity where Description = '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Description"]).ToString() + "') \r\n" +
                                      " and SubActID = (Select SubActID from tbl_SDB_SubActivity where Description = '" + View1.GetRowCellValue(e.RowHandle, View1.Columns["Task"]).ToString() + "') and starton = '" + lblPrevStarton + "' ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "\r\n\r\n  exec sp_SDB_NewActivity_Schedule_OnCHangeAuthid '" + lblPrevStarton + "', '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString() + "'  ";

                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
            }

            if (e.Column.FieldName == "dur")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = "  exec  sp_SDB_UpdateProjectEnd  '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString() + "'           update tbl_SDB_Activity_PlanSum set  Duration = '" + View1.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString() + "' , Processed = 'N' \r\n" +
                                      " where AuthID = '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString() + "' \r\n" +
                                      " and Workplace = '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Workplace"]).ToString() + "' \r\n" +
                                      " and MainActID = (Select max(MainActID) from tbl_SDB_Activity where Description = '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Description"]).ToString() + "') \r\n" +
                                      " and SubActID = (Select SubActID from tbl_SDB_SubActivity where Description = '" + View1.GetRowCellValue(e.RowHandle, View1.Columns["Task"]).ToString() + "')  and starton = '" + View1.GetRowCellValue(e.RowHandle, View1.Columns["Starton"]).ToString() + "'  ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "\r\n\r\n exec sp_SDB_NewActivity_Schedule_OnCHangeAuthid '" + lblPrevStarton + "', '" + gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString() + "' ";

                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
            }

            int focusedRow = bandedGridView5.FocusedRowHandle;
            int focusedRow1 = gvAuth.FocusedRowHandle;
            bandedGridView5.BeginUpdate();
            LoadAuthGrid();
            bandedGridView5.EndUpdate();

            bandedGridView5.FocusedRowHandle = focusedRow;
            gvAuth.SetMasterRowExpanded(focusedRow1, !gvAuth.GetMasterRowExpanded(focusedRow1));
        }

        private void repSeStart_Enter(object sender, EventArgs e)
        {
            SpinEdit view = sender as SpinEdit;

            lblPrevStarton = view.Value.ToString();
        }

        private void gvAuth_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //string authid = gvAuth.GetRowCellValue(e.RowHandle, gvAuth.Columns["AuthID"]).ToString();
        }

        private void barDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvAuth.FocusedRowHandle == -1)
            {
                MessageBox.Show("Please select a record to delete");
                return;
            }

            string workplace = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Workplace"]).ToString();
            string ActDesc = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Description"]).ToString();

            result = MessageBox.Show("Are you sure you want to delete the Plan for workplace " + workplace + " and activity " + ActDesc + " ", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string AUthid = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["AuthID"]).ToString();

                MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
                _dbMan1.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
                _dbMan1.SqlStatement = "insert into tbl_Exclusions_SDB values('" + AUthid + "') ";
                _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan1.ExecuteInstruction();

                var result = _dbMan1.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Delete", "Planning Deleted", Color.CornflowerBlue);
                }

                LoadAuthGrid();
            }
        }

        private void bgwAddData_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadGang();
            loadWorkplace();
            loadActivity();
            loadMiner();
        }

        private void LoadGang()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " Select Gangno + ':' + Gangname crew from tbl_EmployeeAll where  substring(gangno,len(gangno)-2,len(gangno)) in (Select * from tbl_SDB_OrgunitExclusions) \r\n" +
                                  " group by GangNo,GangName order by gangno   ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dtBgwGang = _dbMan.ResultsDataTable;
        }

        private void loadActivity()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " Select * from tbl_SDB_Activity \r\n" +
                                  " where MainActID in (Select distinct MainActID from tbl_SDB_Activity_Schedule)  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dtBgwActivity = _dbMan.ResultsDataTable;
        }

        private void loadWorkplace()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "Select Description Workplaces from tbl_Workplace_Total ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dtBgwWorkplace = _dbMan.ResultsDataTable;
        }

        private void loadMiner()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "Select *,EmployeeName+':'+Name Description from tbl_SDB_SECTION where prodmonth = '" + ProductionGlobalTSysSettings._currentProductionMonth + "' and Hierarchicalid = '6' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dtBgwMiner = _dbMan.ResultsDataTable;
        }

        private void barWorkplace_EditValueChanged(object sender, EventArgs e)
        {
            LoadAuthGrid();
        }

        private void rcSDBPlanning_Click(object sender, EventArgs e)
        {
            barWorkplace_EditValueChanged(null, null);
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
