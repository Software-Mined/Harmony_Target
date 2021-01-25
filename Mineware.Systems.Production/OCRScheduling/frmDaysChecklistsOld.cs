using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mineware.Systems.Global;
using Mineware.Systems.HarmonyPASGlobal;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.GlobalConnect;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using DevExpress.XtraEditors.Controls;
using Newtonsoft.Json;
using Mineware.Systems.HarmonyPAS.SysAdminScreens.OCRScheduling.Models;
using DevExpress.XtraPdfViewer;
using DevExpress.Pdf;

namespace Mineware.Systems.HarmonyPAS.SysAdminScreens.OCRScheduling
{
    public partial class frmDaysChecklists : DevExpress.XtraEditors.XtraForm
    {


        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        Procedures procs = new Procedures();

        private Mineware.Systems.HarmonyPAS.SysAdminScreens.OCRScheduling.Models.FormsAPI _Forms; //PduPlessis
        private Mineware.Systems.HarmonyPAS.SysAdminScreens.OCRScheduling.Models.PrintedForm _PrintedForm; //PduPlessis
        List<ListDrop> _items = new List<ListDrop>();
        List<ListForms> _listForms = new List<ListForms>();
        private BackgroundWorker MovetoProd = new BackgroundWorker();

        private void MovetoProd_DoWork(object sender, DoWorkEventArgs e)
        {
            MoveToProd();
        }


        public frmDaysChecklists()
        {
            InitializeComponent();
        }

        void LoadGrid()
        {

            string Section = procs.ExtractBeforeColon(SecCmb.Text);
            MWDataManager.clsDataAccess _dbManVampWP = new MWDataManager.clsDataAccess();
            if (Section == "Total Mine" || Section == "")
            {
                _dbManVampWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManVampWP.SqlStatement = "  \r\n" +
                                            " select so.ReportToSectionid SectionID,ca.WorkplaceID,w.Description WPName,ca.ChecklistID , f.Name, 'Y' Selected, ''Supervisor, ca.UniqueID        \r\n" +
                                            " from tbl_OCR_CheclistsAdded ca, tbl_OCR_Forms f , Workplace w ,(SELECT SectionID,ReportToSectionid FROM SECTION GROUP BY SectionID,ReportToSectionid UNION ALL SELECT SectionID,ReportToSectionid FROM SECTIONOTHER GROUP BY SectionID,ReportToSectionid) so \r\n" +
                                            " where calendardate = '"+ DateEdit1.EditValue + "'  \r\n" +
                                            " and ca.ChecklistID = f.FormsID  \r\n" +
                                            " and w.WorkplaceID = ca.WorkplaceID" +
                                            "  AND LEFT(ca.SectionID,5) = so.SectionID" +
                                            " ";
                _dbManVampWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVampWP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVampWP.ExecuteInstruction();
            }
            else
            {
                _dbManVampWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManVampWP.SqlStatement = "  \r\n" +
                                            "  select '"+ Section + "' SectionID,ca.WorkplaceID,w.Description WPName,ca.ChecklistID , f.Name, 'Y' Selected, ''Supervisor, ca.UniqueID   \r\n" +
                                            " from tbl_OCR_CheclistsAdded ca, tbl_OCR_Forms f, Workplace w  \r\n" +
                                            " where calendardate = '"+DateEdit1.EditValue+"' \r\n" +
                                            " and ca.ChecklistID = f.FormsID  \r\n" +
                                            " and ca.SectionID like '" + Section + "%' " +
                                            " and w.WorkplaceID = ca.WorkplaceID" +
                                            " ";
                _dbManVampWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVampWP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVampWP.ExecuteInstruction();
            }


            DataTable dt1 = _dbManVampWP.ResultsDataTable;

            DataSet ds1 = new DataSet();

            if (ds1.Tables.Count > 0)
                ds1.Tables.Clear();

            ds1.Tables.Add(dt1);
            gcWorkPlaces.DataSource = ds1.Tables[0];


            gcolWPID.FieldName = "WorkplaceID";
            gcolDESCRIPTION.FieldName = "WPName";
            gcolSelect.FieldName = "Selected";

            gcolChecklistID.FieldName = "ChecklistID";
            gcolChecklistName.FieldName = "Name";
            gcolSuperviser.FieldName = "Supervisor";

            gcolUniqueID.FieldName = "UniqueID";
        }

        string Prodmonth = "";

        private void frmDaysChecklists_Load(object sender, EventArgs e)
        {

            MWDataManager.clsDataAccess _dbManPM = new MWDataManager.clsDataAccess();
            _dbManPM.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManPM.SqlStatement = " select currentproductionmonth from sysset " +
                                      "  " +
                                      "";
            _dbManPM.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManPM.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManPM.ExecuteInstruction();

            Prodmonth = _dbManPM.ResultsDataTable.Rows[0][0].ToString();

            LoadGrid();
           


            ///Load Sections
            ///

            MWDataManager.clsDataAccess _dbManSec = new MWDataManager.clsDataAccess();
            _dbManSec.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManSec.SqlStatement = " select 'Total Mine'+':'+'0' Sec, 0 orderby union select SectionID+':'+Name Sec, 1 orderby  from Section  " +
                                      " where Hierarchicalid = 4 and prodmonth = (select currentproductionmonth from sysset) " +
                                      " " +
                                       " union    \r\n" +
                                             " Select SectionID+': ' + Name Section,2 orderby  from SectionOther    \r\n" +
                                             " where HierarchicalID = 2 and prodmonth = (select currentproductionmonth from sysset)  order by orderby  \r\n";
            _dbManSec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSec.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSec.ExecuteInstruction();

            if (_dbManSec.ResultsDataTable.Rows.Count > 0)
            {
                SecCmb.DataSource = _dbManSec.ResultsDataTable;
                SecCmb.DisplayMember = "Sec";
                SecCmb.ValueMember = "Sec";
            }



            if (procs.ExtractAfterColon(SecCmb.Text) != "0")
            {
                LoadMinerList(Prodmonth, procs.ExtractBeforeColon(SecCmb.Text));
            }
            else
            {
                LoadMinerListTot(Prodmonth);
            }
        }


        public void LoadMinerList(string prodMonth, string sectionidMO)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _MinerData.SqlStatement = "   Select SectionID,  Name from Section  \r\n" +
                                      " where Hierarchicalid = '5'  \r\n" +
                                        " and prodmonth = '" + Prodmonth + "'  \r\n" +
                                       " and SectionID like '" + procs.ExtractBeforeColon(SecCmb.Text) + "%'  \r\n" +
                                     "order by SectionID ";
            _MinerData.ExecuteInstruction();
            SupervisorLookUp.DataSource = null;
            SupervisorLookUp.DataSource = _MinerData.ResultsDataTable;
            SupervisorLookUp.DisplayMember = "Name";
            SupervisorLookUp.ValueMember = "SectionID";

            SupervisorLookUp.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

        }

        public void LoadMinerListTot(string prodMonth)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _MinerData.SqlStatement = "   Select SectionID,  Name from Section  \r\n" +
                                      " where Hierarchicalid = '5'  \r\n" +
                                        " and prodmonth = '" + Prodmonth + "'  \r\n" +
                                       " --and SectionID like '" + procs.ExtractBeforeColon(SecCmb.Text) + "%'  \r\n" +
                                     "order by SectionID ";
            _MinerData.ExecuteInstruction();
            SupervisorLookUp.DataSource = null;
            SupervisorLookUp.DataSource = _MinerData.ResultsDataTable;
            SupervisorLookUp.DisplayMember = "Name";
            SupervisorLookUp.ValueMember = "SectionID";

            SupervisorLookUp.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

        }

        private void SecCmb_TextChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void PrintChecklistsBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
                 DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
                try
                {
                    DataTable table = ((DataView)viewWorkplaces.DataSource).Table;
                var myEnumerable = table.AsEnumerable();
                _listForms.Clear();
                _listForms =
                    (from item in myEnumerable
                     select new ListForms
                     {
                         Section = item.Field<string>("SectionID"),
                         WPID = item.Field<string>("WorkplaceID"),
                         WPName = item.Field<string>("WPName"),
                         FID = item.Field<string>("ChecklistID"),
                         UID = item.Field<int>("UniqueID")
                     }).ToList();

                List<string> ChecklistIDList = table.AsEnumerable().Select(x => x["ChecklistID"].ToString()).Distinct().ToList();

                foreach (var ID in ChecklistIDList)
                {

                    string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                    var client = new Models.ClientConnect();
                    var param = new Dictionary<string, string>();
                    param.Add("FormID", ID);

                    var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                    _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                    _Forms.UniqueDataStructure.TableName = _Forms.TableName;

                    List<ListForms> _FormsL = new List<ListForms>();
                    _FormsL = _listForms.Where(i => i.FID.Contains(ID.ToString())).ToList();
                    _Forms.UniqueDataStructure.Rows[0].Delete();
                    foreach (var form in _FormsL)
                    {
                        try
                        {

                            DataRow row;
                            row = _Forms.UniqueDataStructure.NewRow();

                            row["MOSectionID"] = form.Section.ToString();
                            row["Workplaceid"] = form.WPID.ToString();
                            row["Description"] = form.WPName.ToString();
                            row["CaptureDate"] = DateTime.Now.ToString("ddMMyyyy");

                            _Forms.UniqueDataStructure.Rows.Add(row);

                        }
                        catch
                        {
                            continue;
                        }


                    }
                    GetReport(ID);
                    
                    MoveToProd();
                    //if (MoveToProdind == true)
                    //{
                    //MovetoProd.DoWork += MovetoProd_DoWork;
                    //MovetoProd.RunWorkerAsync();

                    // MoveToProd();

                    foreach (var form in _FormsL)
                    {
                        DateTime dt = (DateTime)DateEdit1.EditValue;
                        UpdateSchedule2("Day" + dt.Day.ToString(), form.UID);
                    }

                }
                DateTime it = DateTime.Now;
                string newFolder = "Mineware_OCR";
                string newFolderDay = newFolder + @"\" + it.ToString("yyyy'_'MM'_'dd");
                Process.Start("explorer.exe", newFolderDay);

            }
            catch
            {
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
        }

      

        void UpdateSchedule2(string SelectedFieldNameDate, int UniqueID)
        {
                MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
                _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManWPSTDetail.SqlStatement = " Exec sp_OCR_UpdateSchedulePrint  '" + SelectedFieldNameDate + "','" + UniqueID + "' \r\n" +
                                                "  \r\n" +
                                                "    ";

                _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            try
            {
                _dbManWPSTDetail.ExecuteInstruction();
            }
            catch
            {

            }
                
           
        }

        private void DateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

              private void GetReport(string FormsID)
        {
            string GetReportRL = string.Format(@"/api/Report/GetReport/");
            var client = new ClientConnect();
            var param = new Dictionary<string, string>();
            var header = new Dictionary<string, string>();
            param.Add("FormsID", FormsID);
            _Forms.UniqueDataStructure.AcceptChanges();
            DataSet TheData = new DataSet();
            TheData.Tables.Clear();
            TheData.Tables.Add(_Forms.UniqueDataStructure);
            string JSOResult;
            JSOResult = JsonConvert.SerializeObject(TheData, Formatting.Indented);
            try
            {


                var response = Task.Run(() => client.PostWithBodyAndParameters(GetReportRL, param, JSOResult)).Result;
                _PrintedForm = JsonConvert.DeserializeObject<PrintedForm>(response);
                string txtPDF = _PrintedForm.PDFLocation;

                if (File.Exists(@_PrintedForm.PDFLocation))
                {

                    //    //Process.Start("explorer.exe", _PrintedForm.PDFLocation);
                    //    PdfViewer i = new PdfViewer();
                    //    DevExpress.XtraEditors.XtraForm jj = new DevExpress.XtraEditors.XtraForm();
                    //    i.LoadDocument(@_PrintedForm.PDFLocation);
                    //    i.Dock = DockStyle.Fill;
                    //    i.ZoomMode = PdfZoomMode.FitToWidth;
                    //    i.NavigationPanePageVisibility = PdfNavigationPanePageVisibility.None;

                    //jj.Controls.Add(i);
                    //i.CreateRibbon();
                    //jj.Width = 600;
                    //    jj.Height = 800;

                    //    jj.ShowIcon = false;
                    //    jj.Text = "OCR - DOCUMENT PREVIEW";
                    //jj.ShowDialog();
                    //if (barCheckItem1.Checked == true)
                    //{
                    //    PdfPrinterSettings ps = new PdfPrinterSettings();
                    //    i.Print(ps);
                    //}

                    DateTime i = DateTime.Now;
                    string newFolder = "Mineware_OCR";
                    string newFolderDay = newFolder+@"\"+i.ToString("yyyy'_'MM'_'dd");

                    string path = System.IO.Path.Combine(
                       Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                       newFolder
                    );

                    if (!System.IO.Directory.Exists(path))
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(path);
                        }
                        catch (IOException ie)
                        {
                            Console.WriteLine("IO Error: " + ie.Message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("General Error: " + e.Message);
                        }
                    }

                    string pathDay = System.IO.Path.Combine(
                      Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                      newFolderDay
                   );

                    if (!System.IO.Directory.Exists(pathDay))
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(pathDay);
                        }
                        catch (IOException ie)
                        {
                            Console.WriteLine("IO Error: " + ie.Message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("General Error: " + e.Message);
                        }
                    }

                    string sourceFile = System.IO.Path.Combine(Path.GetDirectoryName(@_PrintedForm.PDFLocation), Path.GetFileName(@_PrintedForm.PDFLocation));
                    string destFile = System.IO.Path.Combine(pathDay, Path.GetFileName(@_PrintedForm.PDFLocation));
                    System.IO.File.Copy(sourceFile, destFile, true);


                }

            }
            catch (Exception error)
            {

            }
        }

        private void MoveToProd()
        {
            string GetFormInfoURL = string.Format(@"/api/Report/PrintReport/");
            foreach (string s in _PrintedForm.PrintedFromID)
            {

                var client = new ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("PrintedFromID", s);
                param.Add("PrintedByName", "Dolf");

                var response =Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;
            }
        }


        public class ListDrop
        {
            public string TypeCode { get; set; }
            public string ComplName { get; set; }
        }

        public class ListForms
        {
            public string Section { get; set; }
            public string WPID { get; set; }
            public string WPName { get; set; }
            public string FID { get; set; }
            public int UID { get; set; }
        }

        private void SecCmb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
