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
using System.Text;
using System.Threading.Tasks;

namespace Mineware.Systems.Production.OCRScheduling
{
    public partial class frmDaysChecklists : DevExpress.XtraEditors.XtraForm
    {

        StringBuilder sb = new StringBuilder();
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        Procedures procs = new Procedures();
        clsOCRScheduling _clsOCRScheduling = new clsOCRScheduling();
        private FormsAPI _Forms; //PduPlessis
        private PrintedForm _PrintedForm; //PduPlessis
        List<ListDrop> _items = new List<ListDrop>();
        List<ListForms> _listForms = new List<ListForms>();
        public TUserCurrentInfo UserCurrentInfo;
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

           
            MWDataManager.clsDataAccess _dbManVampWP = new MWDataManager.clsDataAccess();
            if (editSection.EditValue.ToString() == "0" || editSection.EditValue.ToString() == "")
            {
                _dbManVampWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManVampWP.SqlStatement = "  \r\n" +
                                           " sp_OCR_Scheduler_Print @Date = '" + DateEdit1.EditValue + "'  \r\n" +
                                           "  ";
                _dbManVampWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVampWP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVampWP.ExecuteInstruction();
            }
            else
            {
                _dbManVampWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManVampWP.SqlStatement = "  \r\n" +
                                            " select '" + editSection.EditValue.ToString() + "' SectionID,ca.WorkplaceID,w.Description WPName,ca.ChecklistID , f.Name, 'Y' Selected, ''Supervisor, ca.UniqueID ,COALESCE(ca.DayString,' ') DayString \r\n" +
                                            " from tbl_OCR_CheclistsAdded ca, tbl_OCR_Forms f, tbl_Workplace w  \r\n" +
                                            " where calendardate = '" + DateEdit1.EditValue + "' \r\n" +
                                            " and ca.ChecklistID = f.FormsID  \r\n" +
                                            " and ca.SectionID like '" + editSection.EditValue.ToString() + "%' " +
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

        string Prodmonth = string.Empty;

        private void frmDaysChecklists_Load(object sender, EventArgs e)
        {

            MWDataManager.clsDataAccess _dbManPM = new MWDataManager.clsDataAccess();
            _dbManPM.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManPM.SqlStatement = " select currentproductionmonth from tbl_sysset " +
                                      "  " +
string.Empty;
            _dbManPM.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManPM.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManPM.ExecuteInstruction();

            Prodmonth = _dbManPM.ResultsDataTable.Rows[0][0].ToString();

            



            ///Load Sections
            ///

            MWDataManager.clsDataAccess _dbManSec = new MWDataManager.clsDataAccess();
            _dbManSec.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            sb.Clear();
            sb.AppendLine(" select '0'SectionID,'Total Mine'Name , 0 orderby union select SectionID,Name , 1 ");
            sb.AppendLine(" orderby  from tbl_Section  ");
            sb.AppendLine(" where Hierarchicalid = 4 and prodmonth = (select currentproductionmonth from tbl_sysset) ");
            sb.AppendLine(" union ");
            sb.AppendLine(" Select SectionID, Name ,2 orderby  from tbl_SectionOther ");
            sb.AppendLine(" where HierarchicalID in (2,4) ");
            sb.AppendLine("and prodmonth = (select currentproductionmonth from tbl_sysset)  order by orderby ");
            _dbManSec.SqlStatement = sb.ToString();
            _dbManSec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSec.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSec.ExecuteInstruction();

            if (_dbManSec.ResultsDataTable.Rows.Count > 0)
            {
                SecCmb.DataSource = _dbManSec.ResultsDataTable;
                SecCmb.DisplayMember = "Name";
                SecCmb.ValueMember = "SectionID";

                editSection.EditValue = _dbManSec.ResultsDataTable.Rows[0]["SectionID"].ToString();
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


        }


        public void LoadMinerList(string prodMonth, string sectionidMO)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _MinerData.SqlStatement = " Select SectionID,  Name from tbl_Section  \r\n" +
                                      " where Hierarchicalid = '5'  \r\n" +
                                      " and prodmonth = '" + Prodmonth + "'  \r\n" +
                                      " and SectionID like '" + editSection.EditValue.ToString() + "%'  \r\n" +
                                      " order by SectionID ";
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
            _MinerData.SqlStatement = " Select SectionID,  Name from tbl_Section  \r\n" +
                                      " where Hierarchicalid = '5'  \r\n" +
                                      " and prodmonth = '" + Prodmonth + "'  \r\n" +
                                      " --and SectionID like '" + editSection.EditValue.ToString() + "%'  \r\n" +
                                      " order by SectionID ";
            _MinerData.ExecuteInstruction();
            SupervisorLookUp.DataSource = null;
            SupervisorLookUp.DataSource = _MinerData.ResultsDataTable;
            SupervisorLookUp.DisplayMember = "Name";
            SupervisorLookUp.ValueMember = "SectionID";
            SupervisorLookUp.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
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
                         UID = item.Field<int>("UniqueID"),
                         DayString = item.Field<string>("DayString")
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
                            row["CaptureDate"] = Convert.ToDateTime(DateEdit1.EditValue).ToString("ddMMyyyy");//DateTime.Now.ToString("ddMMyyyy");
                            _Forms.UniqueDataStructure.Rows.Add(row);
                        }
                        catch { continue; }
                    }
                    GetReport(ID);
                    int c = 0;
                    foreach (var s in _PrintedForm.PrintedFromID)
                    {
                        _FormsL[c].WPIDPrintedFromID = s.ToString();
                        c = c + 1;
                    }
                    _clsOCRScheduling.theData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);

                    foreach (var item in _FormsL)
                    {
                        _clsOCRScheduling.UpdatePrintList(item.WPIDPrintedFromID, item.UID.ToString(), DateEdit1.EditValue.ToString(), item.WPID, item.FID, item.DayString);
                    }

                    MoveToProd();
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
            catch (Exception vv)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
        }

        void UpdateSchedule2(string SelectedFieldNameDate, int UniqueID)
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManWPSTDetail.SqlStatement = " Exec sp_OCR_UpdateSchedulePrint  '" + SelectedFieldNameDate + "','" + UniqueID + "' \r\n";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            try
            {
                _dbManWPSTDetail.ExecuteInstruction();
            }
            catch { }
        }

        private void DateEdit1_EditValueChanged(object sender, EventArgs e)
        {

            //            MWDataManager.clsDataAccess _dbManPM = new MWDataManager.clsDataAccess();
            //            _dbManPM.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            //            _dbManPM.SqlStatement = " select currentproductionmonth from tbl_sysset " +
            //                                      "  " +
            //string.Empty;
            //            _dbManPM.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //            _dbManPM.queryReturnType = MWDataManager.ReturnType.DataTable;
            //            _dbManPM.ExecuteInstruction();

            //            Prodmonth = _dbManPM.ResultsDataTable.Rows[0][0].ToString();





            ///Load Sections
            ///

            MWDataManager.clsDataAccess _dbManSec = new MWDataManager.clsDataAccess();
            _dbManSec.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            sb.Clear();
            sb.AppendLine(" select '0'SectionID,'Total Mine'Name , 0 orderby union select SectionID,Name , 1 ");
            sb.AppendLine(" orderby  from tbl_Section  ");
            sb.AppendLine(" where Hierarchicalid = 4 and prodmonth = (select currentproductionmonth from tbl_sysset) ");
            sb.AppendLine(" union ");
            sb.AppendLine(" Select SectionID, Name ,2 orderby  from tbl_SectionOther ");
            sb.AppendLine(" where HierarchicalID in (2,4) ");
            sb.AppendLine("and prodmonth = (select currentproductionmonth from tbl_sysset)  order by orderby ");
            _dbManSec.SqlStatement = sb.ToString();
            _dbManSec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSec.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSec.ExecuteInstruction();

            if (_dbManSec.ResultsDataTable.Rows.Count > 0)
            {
                SecCmb.DataSource = _dbManSec.ResultsDataTable;
                SecCmb.DisplayMember = "Name";
                SecCmb.ValueMember = "SectionID";
                editSection.EditValue = null;
                editSection.EditValue = _dbManSec.ResultsDataTable.Rows[0]["SectionID"].ToString();
            }
            //LoadGrid();
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

                    DateTime i = DateTime.Now;
                    string newFolder = "Mineware_OCR";
                    string newFolderDay = newFolder + @"\" + i.ToString("yyyy'_'MM'_'dd");

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
            catch (Exception error) { }
        }

        private void MoveToProd()
        {
            //TUserCurrentInfo UserCurrentInfo= new TUserCurrentInfo();
            string GetFormInfoURL = string.Format(@"/api/Report/PrintReport/");
            foreach (string s in _PrintedForm.PrintedFromID)
            {
                var client = new ClientConnect();
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
            public string DayString { get; set; }
            public string WPIDPrintedFromID { get; set; }
        }


        private void SecCmb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void editSection_EditValueChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}
