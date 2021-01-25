using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class ucSDB_AddAcitvites : BaseUserControl
    {
        public ucSDB_AddAcitvites()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpSDB_AddActivities);
            FormActiveRibbonPage = rpSDB_AddActivities;
            FormMainRibbonPage = rpSDB_AddActivities;
            RibbonControl = rcSDB_AddActivities;
        }

        private void ucSDB_AddAcitvites_Load(object sender, EventArgs e)
        {
            LoadActivity();
        }

        #region private variables
        DialogResult result;
        OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        string sourceFile;
        string destinationFile;
        string BusUnit = string.Empty;
        string MainDirectory = @"\\zacddat02.ag.ad.local\MineWarePics$\Mponeng\Services";
        DataTable dtTaskCheck = new DataTable();
        #endregion

        private void LoadActivity()
        {
            MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
            _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbManMain.SqlStatement = " Select *,case when docshortcut = '' then 'N' else 'Y' end as DocIsAttached from tbl_SDB_Activity order by Description  ";

            _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMain.ExecuteInstruction();

            DataTable dtMain = _dbManMain.ResultsDataTable;
            DataSet dsMain = new DataSet();

            dsMain.Tables.Add(dtMain);

            colID.FieldName = "MainActID";
            colDescription.FieldName = "Description";
            colDoc.FieldName = "DocIsAttached";
            colShortcut.FieldName = "DocShortCut";

            gcMainActivity.DataSource = dsMain.Tables[0];

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " Select *,case when docshortcut = '' then 'N' else 'Y' end as DocIsAttached from tbl_SDB_SubActivity order by Description";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dtTaskCheck = _dbMan.ResultsDataTable;

            DataTable dtSub = _dbMan.ResultsDataTable;
            DataSet dsSub = new DataSet();

            dsSub.Tables.Add(dtSub);

            colSubActID.FieldName = "SubActID";
            colSubDescription.FieldName = "Description";
            colDefaultDuration.FieldName = "DefaultDuration";
            colMeasurementType.FieldName = "MeasureType";
            colAmount.FieldName = "Amount";
            colSubDoc.FieldName = "DocIsAttached";
            colSubShortCut.FieldName = "DocShortCut";
            colWorking.FieldName = "WorkingAboveBelow";
            colImpact.FieldName = "Impact";

            gcSubActivity.DataSource = dsSub.Tables[0];
        }

        private void barbtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbManRights = new MWDataManager.clsDataAccess();
            _dbManRights.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManRights.SqlStatement = " select isnull(SDB_InitializeActandTask,0) SDB_InitializeActandTask from tbl_Users_Synchromine where UserID =  '" + TUserInfo.UserID + "' ";
            _dbManRights.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRights.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRights.ExecuteInstruction();
            if (_dbManRights.ResultsDataTable.Rows.Count > 0)
            {
                if (_dbManRights.ResultsDataTable.Rows[0][0].ToString() == "0")
                {
                    MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                    return;
                }
            }

            if (tabControl1.SelectedIndex == 0)
            {
                frmAddActivity Serv1Frm = new frmAddActivity();
                Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
                Serv1Frm.Text = "Activity";
                Serv1Frm.separatorButtons.Top = Serv1Frm.Height - 150;
                Serv1Frm.pnlButtons.Top = Serv1Frm.Height - 120;
                Serv1Frm.Height = 300;
                // Serv1Frm.Width = 500;
                Serv1Frm.lblActType = "Main";
                Serv1Frm.lblFrmtype = "Add";
                Serv1Frm.ShowDialog();
                LoadActivity();

                Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);
            }

            if (tabControl1.SelectedIndex == 1)
            {
                frmAddActivity Serv1Frm = new frmAddActivity();
                Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
                Serv1Frm.Text = "Task";
                //Serv1Frm.Height = 430;
                Serv1Frm.lblActType = "Sub";
                Serv1Frm.lblFrmtype = "Add";
                Serv1Frm.cmbImpact.Items.Add("H");
                Serv1Frm.cmbImpact.Items.Add("M");
                Serv1Frm.cmbImpact.Items.Add("L");
                Serv1Frm._dtTaskCheck = dtTaskCheck;
                Serv1Frm.ShowDialog();
                LoadActivity();

                Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);
            }
        }

        private void gvMainActivity_DoubleClick(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManRights = new MWDataManager.clsDataAccess();
            _dbManRights.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManRights.SqlStatement = " select isnull(SDB_InitializeActandTask,0) SDB_InitializeActandTask from tbl_Users_Synchromine where UserID =  '" + TUserInfo.UserID + "' ";
            _dbManRights.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRights.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRights.ExecuteInstruction();
            if (_dbManRights.ResultsDataTable.Rows.Count > 0)
            {
                if (_dbManRights.ResultsDataTable.Rows[0][0].ToString() == "0")
                {
                    MessageBox.Show("You don't Have rights to Initialise Activities and Tasks,Please Contact you Administrator");
                    return;
                }
            }

            if (gvMainActivity.FocusedColumn.FieldName == "DocIsAttached" && gvMainActivity.GetRowCellValue(gvMainActivity.FocusedRowHandle, gvMainActivity.Columns["DocIsAttached"]).ToString() == "N")
            {
                openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
                openFileDialog1.FileName = null;
                result = openFileDialog1.ShowDialog();

                GetFileMain();
                return;
            }

            frmAddActivity Serv1Frm = new frmAddActivity();
            Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            Serv1Frm.Height = 302;
            Serv1Frm.Width = 786;
            Serv1Frm.Text = "Activity";
            Serv1Frm.lblActType = "Main";
            Serv1Frm.lblFrmtype = "Edit";
            Serv1Frm.lblID = gvMainActivity.GetRowCellValue(gvMainActivity.FocusedRowHandle, gvMainActivity.Columns["MainActID"]).ToString();
            Serv1Frm.Descriptiontxt.Text = gvMainActivity.GetRowCellValue(gvMainActivity.FocusedRowHandle, gvMainActivity.Columns["Description"]).ToString();
            Serv1Frm.lblDestination = gvMainActivity.GetRowCellValue(gvMainActivity.FocusedRowHandle, gvMainActivity.Columns["DocShortCut"]).ToString();
            Serv1Frm.ShowDialog();
            LoadActivity();

            Global.sysNotification.TsysNotification.showNotification("Data Edited", "Record Edited Succesfully", Color.CornflowerBlue);
        }

        private void GetFileMain()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(@"C:\Services");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(@"C:\Services");

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
                    Global.sysNotification.TsysNotification.showNotification("File Attached", "File Attached", Color.CornflowerBlue);
                }
                else
                {
                    System.IO.File.Copy(sourceFile, destinationFile, true);
                    Global.sysNotification.TsysNotification.showNotification("File Attached", "File Attached", Color.CornflowerBlue);

                    dir2 = new System.IO.DirectoryInfo(@"C:\Services");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
                _dbManMain.SqlStatement = " update tbl_SDB_Activity set DocShortCut = '" + destinationFile + "' \r\n" +
                                          " where MainActID = (Select MainActID from tbl_SDB_Activity where Description = '" + gvMainActivity.GetRowCellValue(gvMainActivity.FocusedRowHandle, gvMainActivity.Columns["Description"]).ToString() + "' ) ";

                _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMain.ExecuteInstruction();

                LoadActivity();
            }

        }

        private void GetFileSub()
        {
            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(@"C:\Services");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(@"C:\Services");

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
                    Global.sysNotification.TsysNotification.showNotification("File Attached", "File Attached", Color.CornflowerBlue);
                }
                else
                {
                    System.IO.File.Copy(sourceFile, destinationFile, true);
                    Global.sysNotification.TsysNotification.showNotification("File Attached", "File Attached", Color.CornflowerBlue);

                    dir2 = new System.IO.DirectoryInfo(@"C:\Services");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);
                _dbManMain.SqlStatement = " update tbl_SDB_SubActivity set DocShortCut = '" + destinationFile + "' \r\n" +
                                          " where SubActID = (Select SubActID from tbl_SDB_SubActivity where Description = '" + gvSubActivity.GetRowCellValue(gvSubActivity.FocusedRowHandle, gvSubActivity.Columns["Description"]).ToString() + "' )";

                _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMain.ExecuteInstruction();

                LoadActivity();
            }
        }

        private void gvSubActivity_DoubleClick(object sender, EventArgs e)
        {
            if (gvSubActivity.FocusedColumn.FieldName == "DocIsAttached" && gvSubActivity.GetRowCellValue(gvSubActivity.FocusedRowHandle, gvSubActivity.Columns["DocIsAttached"]).ToString() == "N")
            {
                openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
                openFileDialog1.FileName = null;
                result = openFileDialog1.ShowDialog();

                GetFileSub();
                return;
            }

            frmAddActivity Serv1Frm = new frmAddActivity();
            Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            Serv1Frm.Text = "Tasks";
            Serv1Frm.Height = 380;
            Serv1Frm.Width = 786;
            Serv1Frm.lblActType = "Sub";
            Serv1Frm.lblFrmtype = "Edit";
            Serv1Frm.lblID = gvSubActivity.GetRowCellValue(gvSubActivity.FocusedRowHandle, gvSubActivity.Columns["SubActID"]).ToString();
            Serv1Frm.Descriptiontxt.Text = gvSubActivity.GetRowCellValue(gvSubActivity.FocusedRowHandle, gvSubActivity.Columns["Description"]).ToString();
            Serv1Frm.spinEditDuration.Text = gvSubActivity.GetRowCellValue(gvSubActivity.FocusedRowHandle, gvSubActivity.Columns["DefaultDuration"]).ToString();
            Serv1Frm.cmbMeasureType.Text = gvSubActivity.GetRowCellValue(gvSubActivity.FocusedRowHandle, gvSubActivity.Columns["MeasureType"]).ToString();
            Serv1Frm.Amounttxt.Text = gvSubActivity.GetRowCellValue(gvSubActivity.FocusedRowHandle, gvSubActivity.Columns["Amount"]).ToString();
            Serv1Frm.lblDestination = gvSubActivity.GetRowCellValue(gvSubActivity.FocusedRowHandle, gvSubActivity.Columns["DocShortCut"]).ToString();

            Serv1Frm.ShowDialog();
            LoadActivity();

            Global.sysNotification.TsysNotification.showNotification("Data Edited", "Record Edited Succesfully", Color.CornflowerBlue);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void barbtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void barbtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
