using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPdfViewer;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.Production.OCRScheduling.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OCRScheduling
{

    public partial class frmBulkPrint : DevExpress.XtraEditors.XtraForm
    {
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        Procedures procs = new Procedures();
        string Prodmonth = string.Empty;
        private Mineware.Systems.Production.OCRScheduling.Models.FormsAPI _Forms; //PduPlessis
        private Mineware.Systems.Production.OCRScheduling.Models.PrintedForm _PrintedForm; //PduPlessis
        List<ListDrop> _items = new List<ListDrop>();
        List<DataRow> list = new List<DataRow>();
        List<ListForms> _listForms = new List<ListForms>();
        private bool MoveToProdind = false;
        private int _PrintBatchNumber = 20;
        private bool _loaded = false;
        public TUserCurrentInfo UserCurrentInfo;
        private BackgroundWorker MovetoProd = new BackgroundWorker();

        private void MovetoProd_DoWork(object sender, DoWorkEventArgs e)
        {
            MoveToProd();
        }

        public frmBulkPrint()
        {
            InitializeComponent();
        }



        void LoadGrid()
        {
            //layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never; 
            string Section = editSection.EditValue.ToString();
            MWDataManager.clsDataAccess _dbManVampWP = new MWDataManager.clsDataAccess();
            if (Section == "0" || Section == "")
            {
                _dbManVampWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManVampWP.SqlStatement = "  " +
                                     "  select distinct W.WorkplaceID, W.Description,case when W.Activity = 1 then 'Development' else 'Stoping' end as Activity, 'Y'  Selected, ''Supervisor,Sectionid_2 Sectionid,'' DivisionCode \r\n" +
                                     " from tbl_WORKPLACE W, tbl_PLANMONTH P ,SECTIONS_COMPLETE S \r\n" +
                                    "  where p.WorkplaceID = w.WorkplaceID \r\n" +
                                    "  and p.Prodmonth = '" + Prodmonth + "' AND  p.Sectionid=S.SectionID \r\n" +

                                   
                                            " ";
                _dbManVampWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVampWP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVampWP.ExecuteInstruction();
            }
            else
            {
                _dbManVampWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManVampWP.SqlStatement = "  " +
                                        " select distinct Sectionid_2 Sectionid, W.WorkplaceID, W.Description,case when W.Activity = 1 then 'Development' else 'Stoping' end as Activity, 'Y'  Selected, ''Supervisor,'' DivisionCode \r\n" +
                                    " from tbl_WORKPLACE W, tbl_PLANMONTH P ,SECTIONS_COMPLETE S \r\n" +
                                    " where p.WorkplaceID = w.WorkplaceID \r\n" +
                                    "  and p.Prodmonth = '" + Prodmonth + "' \r\n" +
                                    "  and P.Sectionid like '" + Section + "%' AND p.Sectionid=S.SectionID \r\n" +

                                    "  order by Selected desc \r\n" +
                                                                " \r\n" +
                                            " " +
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
            viewWorkplaces.ExpandAllGroups();


            gcolWPID.FieldName = "WorkplaceID";
            gcolDESCRIPTION.FieldName = "Description";
            gcolSelect.FieldName = "Selected";
            gcolSupervisor.FieldName = "Supervisor";
            gcolActivity.FieldName = "Activity";
        }

        private void frmBulkPrint_Load(object sender, EventArgs e)
        {

            MWDataManager.clsDataAccess _dbManPM = new MWDataManager.clsDataAccess();
            _dbManPM.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManPM.SqlStatement = " select currentproductionmonth from tbl_sysset " +
                                     "  ";
            _dbManPM.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManPM.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManPM.ExecuteInstruction();

            Prodmonth = _dbManPM.ResultsDataTable.Rows[0][0].ToString();

           




            MWDataManager.clsDataAccess _dbManSec = new MWDataManager.clsDataAccess();
            _dbManSec.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManSec.SqlStatement = " select '0'SectionID,'Total Mine'Name, 0 orderby union select SectionID, Name , 1 orderby  from tbl_Section  " +
                                      " where Hierarchicalid = 4 and prodmonth = (select currentproductionmonth from tbl_sysset) " +
                                      " order by orderby";
            _dbManSec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSec.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSec.ExecuteInstruction();




            if (_dbManSec.ResultsDataTable.Rows.Count > 0)
            {
                LookupEditSection.DataSource = _dbManSec.ResultsDataTable;
                LookupEditSection.DisplayMember = "Name";
                LookupEditSection.ValueMember = "SectionID";
                editSection.EditValue = _dbManSec.ResultsDataTable.Rows[0][0].ToString();

            }

            LoadGrid();

            if (editSection.EditValue.ToString() != "0")
            {
                LoadMinerList(Prodmonth, editSection.EditValue.ToString());
            }
            else
            {
                LoadMinerListTot(Prodmonth);
            }
            FillFormGroups();
            radioGroup1.SelectedIndex = 0;
            FillFormTypes();
            LoadChecklists();
            _loaded = true;
        }

        public void LoadMinerList(string prodMonth, string sectionidMO)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _MinerData.SqlStatement = "   Select SectionID,  Name from tbl_Section  \r\n" +
                                      " where Hierarchicalid = '5'  \r\n" +
                                        " and prodmonth = '" + Prodmonth + "'  \r\n" +
                                       " and SectionID like '" + editSection.EditValue.ToString() + "%'  \r\n" +
                                     "order by SectionID ";
            _MinerData.ExecuteInstruction();
            SupervisorSearchLookUp.DataSource = null;
            SupervisorSearchLookUp.DataSource = _MinerData.ResultsDataTable;
            SupervisorSearchLookUp.DisplayMember = "Name";
            SupervisorSearchLookUp.ValueMember = "SectionID";

            SupervisorSearchLookUp.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

        }

        public void LoadMinerListTot(string prodMonth)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _MinerData.SqlStatement = "   Select SectionID,  Name from tbl_Section  \r\n" +
                                      " where Hierarchicalid = '5'  \r\n" +
                                        " and prodmonth = '" + Prodmonth + "'  \r\n" +
                                       " --and SectionID like '" + editSection.EditValue.ToString() + "%'  \r\n" +
                                     "order by SectionID ";
            _MinerData.ExecuteInstruction();
            SupervisorSearchLookUp.DataSource = null;
            SupervisorSearchLookUp.DataSource = _MinerData.ResultsDataTable;
            SupervisorSearchLookUp.DisplayMember = "Name";
            SupervisorSearchLookUp.ValueMember = "SectionID";

            SupervisorSearchLookUp.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

        }

        void LoadChecklists()
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManWPSTDetail.SqlStatement = " SELECT Convert(Varchar(50),FormsID) +':'+ Name  Form,TypeCode  \r\n" +
                                            " FROM [dbo].[vw_OCR_Form_List]  WHERE GroupName = '" + radioGroup1.EditValue.ToString() + "' \r\n" +

                                            "    ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            DataTable dt3 = _dbManWPSTDetail.ResultsDataTable;


            // int dd = 1;
            OCRCL.Items.Clear();

            for (int i = 1; i < dt3.Rows.Count; i++)

            {
                OCRCL.Items.Add(_dbManWPSTDetail.ResultsDataTable.Rows[i][0].ToString());
            }

            var myEnumerable = dt3.AsEnumerable();
            _items.Clear();
            _items =
                (from item in myEnumerable
                 select new ListDrop
                 {
                     TypeCode = item.Field<string>("TypeCode"),
                     ComplName = item.Field<string>("Form")
                 }).ToList();


        }

        private void SearchTxt_TextChanged(object sender, EventArgs e)
        {
            OCRCL.Items.Clear();
            OCRCL.Items.AddRange(
                    _items.Where(i => i.ComplName.Contains(SearchTxt.Text.ToString())).Select(i => i.ComplName.ToString()).ToArray());
        }

       

        private void viewWorkplaces_DoubleClick(object sender, EventArgs e)
        {
            return;
            if (viewWorkplaces.GetRowCellValue(viewWorkplaces.FocusedRowHandle, viewWorkplaces.Columns["Selected"]) == "N")
            {
                viewWorkplaces.SetRowCellValue(viewWorkplaces.FocusedRowHandle, viewWorkplaces.Columns["Selected"], "Y");
            }
            else
            {
                viewWorkplaces.SetRowCellValue(viewWorkplaces.FocusedRowHandle, viewWorkplaces.Columns["Selected"], "N");
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OCRCL.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please select a Form", "Select a Form");
                return;
            }
            if (viewWorkplaces.SelectedRowsCount == 0)
            {
                MessageBox.Show(this, "Please select at least one workplace to print", "Select a workplace");
                return;
            }
            DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            MoveToProdind = true;
            try
            {
                DataTable table = ((DataView)viewWorkplaces.DataSource).Table;
                Int32[] selectedRowHandles = viewWorkplaces.GetSelectedRows();
                _listForms.Clear();
                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int selectedRowHandle = selectedRowHandles[i];
                    if (selectedRowHandle >= 0)
                    {
                        _listForms.Add(new ListForms
                        {
                            Section = viewWorkplaces.GetRowCellValue(selectedRowHandle, "Sectionid").ToString()
                        ,
                            WPID = viewWorkplaces.GetRowCellValue(selectedRowHandle, "WorkplaceID").ToString()
                        ,
                            WPName = viewWorkplaces.GetRowCellValue(selectedRowHandle, "Description").ToString()
                        ,
                            FID = procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString())
                        ,
                            UID = 0
                        });
                    }


                }

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;

                List<ListForms> _FormsL = new List<ListForms>();
                _FormsL = _listForms.Where(i => i.FID.Contains(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()).ToString())).ToList();
                _Forms.UniqueDataStructure.Rows[0].Delete();

                var allResponses = new List<ListForms>();
                for (int i = 0; i < _FormsL.Count; i = i + _PrintBatchNumber)
                {
                    _Forms.UniqueDataStructure.Rows.Clear();
                    foreach (var form in _FormsL.Skip(i).Take(_PrintBatchNumber))
                    {
                        try
                        {

                            DataRow row;
                            row = _Forms.UniqueDataStructure.NewRow();

                            row["MOSectionID"] = form.Section.ToString();
                            row["Workplaceid"] = form.WPID.ToString();
                            row["Description"] = form.WPName.ToString();
                            DateTime dt = (DateTime)dtDateSelection.EditValue;
                            row["CaptureDate"] = dt.ToString("ddMMyyyy");
                            //row["CaptureDate"] = DateTime.Now.ToString("ddMMyyyy");

                            _Forms.UniqueDataStructure.Rows.Add(row);


                        }
                        catch
                        {
                            continue;
                        }
                    }
                    GetReport(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                    if (MoveToProdind == true)
                    {
                        MoveToProd();
                    }
                }

                DateTime it = DateTime.Now;
                string newFolder = "Mineware_OCR";
                string newFolderDay = newFolder + @"\" + it.ToString("yyyy'_'MM'_'dd") + "_BulkPrint";
                Process.Start("explorer.exe", newFolderDay);
            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {


            object SelectedValue = radioGroup2.EditValue;
            if (radioGroup2.SelectedIndex == 0)
            {
                OCRCL.Items.Clear();
                OCRCL.Items.AddRange(
                        _items.Select(i => i.ComplName.ToString()).ToArray());
            }
            else
            {
                OCRCL.Items.Clear();
                OCRCL.Items.AddRange(
                        _items.Where(i => i.TypeCode.Contains(SelectedValue.ToString())).Select(i => i.ComplName.ToString()).ToArray());
            }
        }

        private void FillFormTypes()
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManWPSTDetail.SqlStatement = " SELECT TypeCode, CAST(TypeCode AS VARCHAR(10)) + '-' + TypeName AS ComplName  \r\n" +
                                            " FROM [dbo].[vw_OCR_Form_List] WHERE GroupName = '" + radioGroup1.EditValue.ToString() + "' \r\n" +

                                             "  GROUP BY TypeName, TypeCode  ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            DataTable dt_FormList = _dbManWPSTDetail.ResultsDataTable;


            radioGroup2.Properties.Items.Clear();
            RadioGroupItem rdi_all = new RadioGroupItem();
            rdi_all.Description = "All";
            rdi_all.Value = "All";
            radioGroup2.Properties.Items.Add(rdi_all);
            foreach (DataRow row in dt_FormList.Rows)
            {
                RadioGroupItem rdi = new RadioGroupItem();
                rdi.Description = row[1].ToString();
                rdi.Value = row[0].ToString();
                rdi.Tag = row[0].ToString();

                radioGroup2.Properties.Items.Add(rdi);
            }

        }

        private void GetFromInfo(string FormsID)
        {
            try
            {
                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", FormsID);

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;

                if (MoveToProdind == true)
                {

                }
                else
                {
                    _Forms.UniqueDataStructure.Rows[0].Delete();

                    DataRow row;
                    row = _Forms.UniqueDataStructure.NewRow();
                    DateTime dtPrint = (DateTime)dtDateSelection.EditValue;
                    row["MOSectionID"] = "EXAMPLE";
                    row["Workplaceid"] = "EXAMPLE";
                    row["Description"] = "EXAMPLE";
                    row["CaptureDate"] = dtPrint.ToString("ddMMyyyy");

                    _Forms.UniqueDataStructure.Rows.Add(row);
                }
            }
            catch
            {

            }
            GetReport(FormsID);
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
            if (_Forms.UniqueDataStructure.DataSet != null)
            {
                DataSet dss = _Forms.UniqueDataStructure.DataSet;
                dss.Tables.Remove(_Forms.UniqueDataStructure);
            }

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
                    if (MoveToProdind)
                    {

                        DateTime i = DateTime.Now;
                        string newFolder = "Mineware_OCR";
                        string newFolderDay = newFolder + @"\" + i.ToString("yyyy'_'MM'_'dd") + "_BulkPrint";

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
                    else
                    {
                        PdfViewer i = new PdfViewer();
                        DevExpress.XtraEditors.XtraForm jj = new DevExpress.XtraEditors.XtraForm();
                        i.LoadDocument(@_PrintedForm.PDFLocation);
                        i.Dock = DockStyle.Fill;
                        i.ZoomMode = PdfZoomMode.FitToWidth;
                        i.NavigationPanePageVisibility = PdfNavigationPanePageVisibility.None;
                        jj.Controls.Add(i);
                        jj.Width = 600;
                        jj.Height = 800;

                        jj.ShowIcon = false;
                        jj.Text = "CHECKLIST EXAMPLE - CANNOT BE PRINTED";
                        jj.ShowDialog();
                    }

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

                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("PrintedFromID", s);
                param.Add("PrintedByName", UserCurrentInfo.UserID);

                var response = Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;
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

        private void OCRCL_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            try
            {
                MoveToProdind = false;
                GetFromInfo(procs.ExtractBeforeColon(OCRCL.SelectedItem.ToString()));
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            }
            catch
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
                return;
            }
        }

        private void FillFormGroups()
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManWPSTDetail.SqlStatement = " SELECT GroupName \r\n" +
                                            " FROM [dbo].[vw_OCR_Form_List] \r\n" +

                                             "  GROUP BY GroupName  ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            DataTable dt_FormList = _dbManWPSTDetail.ResultsDataTable;


            radioGroup1.Properties.Items.Clear();
            foreach (DataRow row in dt_FormList.Rows)
            {
                RadioGroupItem rdi = new RadioGroupItem();
                rdi.Description = row[0].ToString();
                rdi.Value = row[0].ToString();
                rdi.Tag = row[0].ToString();

                radioGroup1.Properties.Items.Add(rdi);
            }

        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFormTypes();
            radioGroup2.SelectedIndex = 0;
            LoadChecklists();

            if (UserCurrentInfo.Connection == "Tshepong Operations")
            {
                if (radioGroup1.EditValue.ToString() == "Tshepong Mine")
                {
                    string filterString = "[DivisionCode] ='" + "JB" + "'";
                    viewWorkplaces.Columns["DivisionCode"].FilterInfo = new ColumnFilterInfo(filterString);
                }
                else
                {
                    string filterString = "[DivisionCode] ='" + "JJ" + "'";
                    viewWorkplaces.Columns["DivisionCode"].FilterInfo = new ColumnFilterInfo(filterString);
                }

            }
        }

        private void SecCmb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtDateSelection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void dtDateSelection_EditValueChanged(object sender, EventArgs e)
        {
            if (!_loaded) return;
            DateTime dtPrint = (DateTime)dtDateSelection.EditValue;
            if (dtPrint.Date < DateTime.Now)
            {
                dtDateSelection.EditValue = DateTime.Now;
                MessageBox.Show("Date not valid, please select a valid date", "Date not valid");
            }
        }

        private void editSection_EditValueChanged(object sender, EventArgs e)
        {
            LoadGrid();

            if (editSection.EditValue.ToString() != "0")
            {
                LoadMinerList(Prodmonth, editSection.EditValue.ToString());
            }
            else
            {
                LoadMinerListTot(Prodmonth);
            }
        }
    }
}
