using DevExpress.XtraGrid.Views.Grid;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionHelp;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPdfViewer;

using Mineware.Systems.Production.OCRScheduling.Models;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System.Threading.Tasks;
using System.Configuration;

namespace Mineware.Systems.Production.LineActionManager
{
    public partial class ucLineActionManager_CloseAction : BaseUserControl
    {
        private Report _theReport = new Report();
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        Procedures procs = new Procedures();
        clsActionManager _clsActionManager = new clsActionManager();
        public ucLineActionManager_CloseAction()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpLineAction);
            FormActiveRibbonPage = rpLineAction;
            FormMainRibbonPage = rpLineAction;
            RibbonControl = rcLineAction;

            bwLoad.DoWork += bwLoad_DoWork;
            bwLoad.RunWorkerCompleted += bwLoad_RunWorkerCompleted;
            bwLoadGrid.DoWork += bwLoadGrid_DoWork;
            bwLoadGrid.RunWorkerCompleted += bwLoadGrid_RunWorkerCompleted;
        }

        public DataTable dtGenReport = new DataTable("dtGenReport");
        public DataTable dtCloseActions = new DataTable("dtCloseActions");
        public int _selectedtab = 0;


        //private declarations
        private BackgroundWorker bwLoad = new BackgroundWorker();
        private BackgroundWorker bwLoadGrid = new BackgroundWorker();
        private String Report_ID_Scan_Date;
        private String Report_ID;
        private String Mineware_Action_IDFP;
        private String sqlQuery;
        private Random rdm = new Random();
        private DataSet dsGlobal = new DataSet("dsGlobal");
        private DataTable dtGetActions = new DataTable("dtGetActions");
        private DataTable dtGetActionsUnMailed = new DataTable("dtGetActionsUnMailed");
        public DataTable dtShowOpenActions_Gen = new DataTable("dtShowOpenActions_Gen");
        private DataTable dtGetScans = new DataTable("dtGetScans");
        private DataTable dtActionDetailClose = new DataTable("dtActionDetailClose");
        private DataTable dtActionClosed = new DataTable("dtShowClosedActions_Gen");
        private DataTable dtOperations = new DataTable("dtOperations");
        private int[] selectedrows;
        private bool expanded = true;

        DataTable dtFlagWorkplaces = new DataTable();
        DataTable dtFlagSect = new DataTable();



        private void registerTables()
        {
            dsGlobal.Tables.Clear();
            dsGlobal.Tables.Add(dtGetActions);
            dsGlobal.Tables.Add(dtGetActionsUnMailed);
            dsGlobal.Tables.Add(dtShowOpenActions_Gen);
            dsGlobal.Tables.Add(dtGetScans);
            dsGlobal.Tables.Add(dtGenReport);
            dsGlobal.Tables.Add(dtCloseActions);
            dsGlobal.Tables.Add(dtActionDetailClose);
            dsGlobal.Tables.Add(dtActionClosed);
            dsGlobal.Tables.Add(dtOperations);
        }

        private void sqlConnector(string sqlQuery, string sqlTableIdentifier)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
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

        private void handleControls()
        {
            int[] rows = gvGenOpen.GetSelectedRows();
            if (rows.Length > 0)
            {
                rpg_OpenActions.Enabled = true;
            }
            else
            {
                //rpg_OpenActions.Enabled = false;
            }
        }

        private void loadReportID()
        {
            sqlQuery = " SELECT DISTINCT Report_ID, b.Location, b.ScanDate AS ScannedDate " +
                       " FROM[dbo].[tbl_Incidents_Emailed] AS a " +
                       " RIGHT OUTER JOIN [MinewareOCR_AMP].[dbo].[tblWorkNotes] AS b ON a.Report_ID = b.WorkNoteNumber  " +
                       " LEFT OUTER JOIN [dbo].[tbl_Incidents_Scanned] as c ON c.Mineware_Action_ID = a.Mineware_Action_ID" +
                       " WHERE Report_ID <> '' ";

            sqlConnector(sqlQuery, "dtGetScans");
        }

        private void InsertActions()
        {

            int rowcount = gvGenOpen.SelectedRowsCount / 5;

            for (int irow = -1; irow < rowcount; irow++)
            {
                int[] rows = gvGenOpen.GetSelectedRows();

                //Creates new Report_ID
                string ID_Part1 = "MAN-";
                string ID_Part2 = "Amandelbult" + "-";
                string ID_Part3 = DateTime.Now.ToString("MMddmmss") + "-";
                int ID_Part4 = rdm.Next(1, 999);
                string ID_Complete = ID_Part1 + ID_Part2 + ID_Part3 + Convert.ToString(ID_Part4);

                //string DateGenerated = String.Format("{0:MM/dd/yy HH:mm:ss.sss}", DateTime.Today.Date);
                string DateGenerated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.sss");

                //Loops though selected rows
                int getCount = 0;
                int number = 1;

                foreach (int i in rows)
                {

                    if (i >= 0)
                    {
                        Mineware_Action_IDFP = gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["IncidentNumber"]).ToString();
                        //sqlQuery = "INSERT INTO [tbl_Incidents_SelectedOCR] select * from tbl_Incidents where Mineware_Action_ID = '" + Mineware_Action_IDFP + "'";
                        sqlQuery = "Insert into tbl_LineActionManager_CloseActions (Mineware_Action_ID, Report_ID, Date_Generated, ForcePrint) \r\n" +
                                   "values('" + Mineware_Action_IDFP + "', '" + ID_Complete + "', '" + DateGenerated + "', '" + number + "') \r\n";
                        gvGenOpen.UnselectRow(i);
                        sqlConnector(sqlQuery, null);
                        if (getCount >= 4)
                        {
                            Array.Resize(ref rows, rows.Length - 5);
                            getCount = 0;
                            break;
                        }
                        getCount++;
                        number++;
                    }

                }
            }
        }

        private void loadOpenActions_Gen()
        {

            _clsActionManager._theConnection = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _clsActionManager.get_OpenActions();
            dtShowOpenActions_Gen = clsActionManager.dtActions;

        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            registerTables();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bwLoadGrid.RunWorkerAsync();
        }

        private void bwLoadGrid_DoWork(object sender, DoWorkEventArgs e)
        {
            loadOperation();
            loadReportID();
        }

        private void loadOperation()
        {
            try
            {
                loadOpenActions_Gen();
                gvGenOpen.ExpandAllGroups();

            }
            catch
            {

            }
        }

        private void bwLoadGrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gcGenOpen.DataSource = dtShowOpenActions_Gen;
            gvGenOpen.Columns["Section"].Group();
            handleControls();
            //this.ShowProgressPanel = false;
            gvGenOpen.ExpandAllGroups();

            gcGenOpen.Visible = true;
        }

        private void ucLineActionManager_CloseAction_Load(object sender, EventArgs e)
        {
            gcGenOpen.Visible = false;
            bwLoad.RunWorkerAsync();

            ucLineActionManager_AdditionalUsers ucAdditional = new ucLineActionManager_AdditionalUsers();
            ucAdditional.UserCurrentInfo = this.UserCurrentInfo;
            xtpAdditional.Controls.Add(ucAdditional);
            ucAdditional.Dock = DockStyle.Fill;
            ucAdditional.Show();

            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbMan1.SqlStatement = "select workplace from tbl_Incidents \r\n" +
                                   " where workplace not in (Select description from tbl_Workplace_Total \r\n" +
                                   "union \r\n" +
                                   "Select description from tbl_Workplace_Total_Floc) \r\n" +
                                   "group by workplace order by workplace ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            dtFlagWorkplaces = _dbMan1.ResultsDataTable;

            foreach (DataRow dr in _dbMan1.ResultsDataTable.Rows)
            {
                repImgWorkplace.Items.Add(dr["workplace"].ToString(), dr["workplace"].ToString(), 0);
            }

            MWDataManager.clsDataAccess _dbSect = new MWDataManager.clsDataAccess();
            _dbSect.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbSect.SqlStatement = "select distinct Section , 'Y' Flags from tbl_Incidents \r\n" +
                                    "where Section not in (select distinct sectionid from tbl_Section where Hierarchicalid = '4' \r\n" +
                                    "union \r\n" +
                                    "Select '') \r\n" +

                                    "union \r\n" +

                                    "select distinct SectionID , 'N' from tbl_Section where Hierarchicalid = '4'  ";
            _dbSect.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbSect.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbSect.ExecuteInstruction();

            dtFlagSect = _dbSect.ResultsDataTable;

            foreach (DataRow dr in _dbSect.ResultsDataTable.Rows)
            {
                if (dr["Flags"].ToString() == "Y")
                {
                    repImgMoSection.Items.Add(dr["Section"].ToString(), dr["Section"].ToString(), 0);
                }

                if (dr["Flags"].ToString() == "N")
                {
                    repImgMoSection.Items.Add(dr["Section"].ToString(), dr["Section"].ToString(), -1);
                }
            }
        }

        private void accordionControlElement1_Click(object sender, EventArgs e)
        {

        }

        private void barBtnGenReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvGenOpen.SelectedRowsCount != 0)
            {
                selectedrows = gvGenOpen.GetSelectedRows();
                //InsertActions();
                ForcePrint_Open();
                generateReport_OpenActions();
                
            }
            else
            {
                MessageBox.Show("Please Select Incidents before generating Report");
            }
        }

        private void ForcePrint_Open()
        {

            int rowcount = gvGenOpen.SelectedRowsCount / 5;
            MWDataManager.clsDataAccess _dbSect = new MWDataManager.clsDataAccess();
            _dbSect.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbSect.SqlStatement = "delete from [tbl_Incidents_SelectedOCR]";
            _dbSect.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbSect.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbSect.ExecuteInstruction();
            for (int irow = -1; irow < rowcount; irow++)
            {
                int[] rows = gvGenOpen.GetSelectedRows();

                //Creates new Report_ID
                string ID_Part1 = "MAN-";
                string ID_Part2 = "Amandelbult" + "-";
                string ID_Part3 = DateTime.Now.ToString("MMddmmss") + "-";
                int ID_Part4 = rdm.Next(1, 999);
                string ID_Complete = ID_Part1 + ID_Part2 + ID_Part3 + Convert.ToString(ID_Part4);

                

                //Loops though selected rows
                int getCount = 0;
               
                foreach (int i in rows)
                {
                    if (i >= 0)
                    {
                        Mineware_Action_IDFP = gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["IncidentNumber"]).ToString();
                        sqlQuery =  "INSERT INTO [tbl_Incidents_SelectedOCR] select * from tbl_Incidents where Mineware_Action_ID = '" + Mineware_Action_IDFP + "'";
                        gvGenOpen.UnselectRow(i);
                        sqlConnector(sqlQuery, null);
                        if (getCount >= 4)
                        {
                            Array.Resize(ref rows, rows.Length - 5);
                            getCount = 0;
                            break;
                        }
                        getCount++;
                    }

                }
            }
        }
        private void generateReport_OpenActions()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Report_ID", typeof(String));
            dt.Columns.Add("DocumentLink", typeof(String));
            dt.Columns.Add("IncidentNumber", typeof(String));
            dt.Columns.Add("Workplace", typeof(String));
            dt.Columns.Add("Action", typeof(String));
            dt.Columns.Add("Mine", typeof(String));
            dt.Columns.Add("Section", typeof(String));
            dt.Columns.Add("ReportedBy", typeof(String));
            dt.Columns.Add("Hazard", typeof(String));
            dt.Columns.Add("ActionDate", typeof(String));
            dt.Columns.Add("RequiredDate", typeof(String));
            //dt.Columns.Add("CompletedDate", typeof(String));
            DataRow dr = dt.NewRow();

            //int[] rows = gvGenOpen.GetSelectedRows();
            foreach (int i in selectedrows)
            {
                if (i >= 0)
                {
                    dt.Rows.Add(gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Report_ID"]).ToString()
                        , gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["DocumentLink"]).ToString()
                        , gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["IncidentNumber"]).ToString()
                        , gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Workplace"]).ToString().TrimStart()
                        , gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Action"]).ToString()
                        , gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Mine"]).ToString()
                        , gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Section"]).ToString()
                        , gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["ResponsiblePerson"]).ToString()
                        , gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Hazard"]).ToString()
                        , String.Format("{0:dd MMM yyyy}", gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["ActionDate"])).ToString()
                        , String.Format("{0:dd MMM yyyy}", gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["RequiredDate"])).ToString());
                        //, String.Format("{0:ddMMyyyy}", DateTime.Now).ToString());

                }
            }

            var list = dt.Rows.OfType<DataRow>().Distinct().Select(x => x.Field<string>("Mine")).ToList();

            dtGenReport = dt;

            frmLineActionManager_Report ucClose = new frmLineActionManager_Report();
            ucClose.connection = UserCurrentInfo.Connection;
            ucClose.dt = dt;
            ucClose.dtGenReportReceive = dtGenReport;
            ucClose.Dock = DockStyle.Fill;
            ucClose.StartPosition = FormStartPosition.CenterScreen;
            ucClose.ShowDialog();
        }

        private void barBtnCloseActions_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvGenOpen.FocusedRowHandle == -1)
            {
                MessageBox.Show("Please select a Row Before trying to Close an Action");
                return;
            }

            try
            {
                string Mineware_Action_ID = string.Empty;
                string ScannedDocument = string.Empty;
                string Report_ID = string.Empty;
                string rowID = string.Empty;
                string respFeedback = string.Empty;
                string CloseDate = String.Format("{0:yyyy-MM-dd}", DateTime.Today.Date);
                string CloseBy = string.Empty;
                int row = 1;

                MWDataManager.clsDataAccess _sqlUpdateAll = new MWDataManager.clsDataAccess();
                _sqlUpdateAll.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
                _sqlUpdateAll.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _sqlUpdateAll.queryReturnType = MWDataManager.ReturnType.DataTable;

                int[] rows = gvGenOpen.GetSelectedRows();
                foreach (int i in rows)
                {
                    if (i != -1)
                    {
                        row = row + i;

                        if (gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["RespFeedback"]) != null)
                        {
                            respFeedback = gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["RespFeedback"]).ToString();
                        }
                        if (gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["DocumentLink"]) != null)
                        {
                            ScannedDocument = gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["DocumentLink"]).ToString();
                        }
                        if (gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Report_ID"]) != null)
                        {
                            Report_ID = gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Report_ID"]).ToString();
                        }
                        if (gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["IncidentNumber"]) != null)
                        {
                            Mineware_Action_ID = gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["IncidentNumber"]).ToString();
                        }
                        if (gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Action_Close_Date"]) != null)
                        {
                            //CloseDate = Convert.ToDateTime( gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Action_Close_Date"])).ToString();
                            CloseDate = String.Format("{0:yyyy-MM-dd}", gvGenOpen.GetRowCellValue(i, gvGenOpen.Columns["Action_Close_Date"]));

                        }
                        CloseBy = TUserInfo.UserID;

                        rowID = "Closed_" + row;

                        _sqlUpdateAll.SqlStatement = _sqlUpdateAll.SqlStatement + "exec [sp_LineActionManager_Update_ActionClosure]  '" + Mineware_Action_ID + "','" + string.Format("{0:yyyy-MM-dd}", CloseDate) + "' , '" + respFeedback + "' , '" + Report_ID + "' , '" + CloseBy + "'  \r\n\r\n";
                    }
                }

                _sqlUpdateAll.ExecuteInstruction();
            }
            catch
            {

            }
        }

        private void barBtnAddActions_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmLineActionManagerAdhoc frmAdhoc = new frmLineActionManagerAdhoc();
            frmAdhoc.UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            frmAdhoc.StartPosition = FormStartPosition.CenterScreen;
            frmAdhoc.ShowDialog();
        }

        private void barBtnExpandCollapse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (expanded)
            {
                gvGenOpen.CollapseAllGroups();
                expanded = false;
                return;
            }

            if (!expanded)
            {
                gvGenOpen.ExpandAllGroups();
                expanded = true;
                return;
            }
        }

        private void barBtnShowClosedActions_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                rpg_OpenActions.Enabled = false;
                //rpg_AddAction.Enabled = false;
            }
            else
            {
                rpg_OpenActions.Enabled = true;
                //rpg_AddAction.Enabled = true;
            }
        }

        private void gvGenOpen_DoubleClick(object sender, EventArgs e)
        {
            if (gvGenOpen.RowCount > 0)
            {
                //if (gvGenOpen.FocusedRowHandle == -1)
                //{
                //    MessageBox.Show("Please select a Incident Before trying to Edit");
                //    return;
                //}

                string workplace = gvGenOpen.GetRowCellValue(gvGenOpen.FocusedRowHandle, gvGenOpen.Columns["Workplace"]).ToString().TrimStart();
                string Section = gvGenOpen.GetRowCellValue(gvGenOpen.FocusedRowHandle, gvGenOpen.Columns["Section"]).ToString();
                string MinewareActionID = gvGenOpen.GetRowCellValue(gvGenOpen.FocusedRowHandle, gvGenOpen.Columns["IncidentNumber"]).ToString();
                string Hazard = gvGenOpen.GetRowCellValue(gvGenOpen.FocusedRowHandle, gvGenOpen.Columns["Hazard"]).ToString();

                string Action = gvGenOpen.GetRowCellValue(gvGenOpen.FocusedRowHandle, gvGenOpen.Columns["Action"]).ToString();
                string RespFB = gvGenOpen.GetRowCellValue(gvGenOpen.FocusedRowHandle, gvGenOpen.Columns["RespFeedback"]).ToString();

                string CloseDate = gvGenOpen.GetRowCellValue(gvGenOpen.FocusedRowHandle, gvGenOpen.Columns["ActionDate"]).ToString();

                frmLineActionManager_Edit frmEdit = new frmLineActionManager_Edit();
                frmEdit.UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
                frmEdit.lblIncidentNumber.Text = MinewareActionID;
                frmEdit.txtMineoverseer.EditValue = Section;
                frmEdit.currHaz = Hazard.ToUpper();
                frmEdit.txtWorkplace.Text = workplace;
                frmEdit.txtAction.EditValue = Action;
                frmEdit.txtRespFeedback.EditValue = RespFB;
                frmEdit.CloseDate = String.Format("{0:yyyy-MM-dd}", CloseDate);

                frmEdit.StartPosition = FormStartPosition.CenterScreen;
                frmEdit.ShowDialog();

                //loadOpenActions_Gen();
                //gvGenOpen.ExpandAllGroups();
            }
        }

        private void gvGenOpen_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //if (e.Column.FieldName == "Workplace")
            //{
            //    foreach (DataRow dr in dtFlagWorkplaces.Rows)
            //    {
            //        if (e.CellValue.ToString() == dr["workplace"].ToString())
            //        {
            //            e.RepositoryItem = repImgWorkplace;
            //        }
            //    }
            //}

            if (e.Column.FieldName == string.Empty)
            {

            }
        }

        private void gvGenOpen_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //Image warningImage = imageCollection1.Images[0];

            //if (e.Column.FieldName == "Workplace")
            //{
            //    e.DefaultDraw();

            //    foreach (DataRow dr in dtFlagWorkplaces.Rows)
            //    {
            //        if (e.CellValue.ToString().Trim() == dr["workplace"].ToString().Trim())
            //        {
            //            Point NewPosition = e.Bounds.Location;

            //            NewPosition.Y = NewPosition.Y + 10;

            //            e.Cache.DrawImage(warningImage, NewPosition);
            //        }
            //    }
            //}
        }

        private void barBtnCloseCloseActions_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWordEditor helpFrm = new frmWordEditor();
            helpFrm.ViewType = "View";
            helpFrm.MainCat = "LineActionManager";
            helpFrm.SubCat = "LineActionManager";
            helpFrm.Show();
        }

        private void barBtnPrintExp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        
    }
}
