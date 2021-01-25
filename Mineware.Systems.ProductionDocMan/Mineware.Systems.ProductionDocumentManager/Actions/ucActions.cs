using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Mineware.Systems.DocumentManager.Models;
using Newtonsoft.Json;
using DevExpress.XtraPdfViewer;
using System.IO;
using System.Diagnostics;

namespace Mineware.Systems.DocumentManager
{
    public partial class ucActions : UserControl
    {
        private FormsAPI _Forms;
        Procedures procs = new Procedures();
        private PrintedForm _PrintedForm;
        List<ListDrop> _items = new List<ListDrop>();
        List<DataRow> list = new List<DataRow>();
        List<ListForms> _listForms = new List<ListForms>();
        private bool MoveToProdind = false;
        private int _PrintBatchNumber = 20;
        private bool _loaded = false;
        //public TUserCurrentInfo UserCurrentInfo;
        private BackgroundWorker MovetoProd = new BackgroundWorker();

        public ucActions()
        {
            InitializeComponent();
        }
        
        private void ucVentAuth_Load(object sender, EventArgs e)
        {
            btnPrint_Click(null, null);

        }

       




        private void btnPrint_Click(object sender, EventArgs e)
        {
            DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            MoveToProdind = true;
            try
            {
                //DataTable table = ((DataView)gvGenOpen.DataSource).Table;
                //Int32[] selectedRowHandles = gvGenOpen.GetSelectedRows();
                _listForms.Clear();
                //for (int i = 0; i < selectedRowHandles.Length; i++)
                //{
                //    int selectedRowHandle = selectedRowHandles[i];
                //    if (selectedRowHandle >= 0)
                //    {
                        _listForms.Add(new ListForms
                        {
                            Section = "A114"
                        ,
                            WPID = ""
                        ,
                            WPName = ""
                        ,
                            FID = "150"
                        ,
                            UID = 0
                        });
                //    }


                //}

                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", "150");

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;
                OCRCL.SelectedValue = "150";
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

                            row["Section"] = form.Section.ToString();
                            //row["Workplaceid"] = form.WPID.ToString();
                            //row["Description"] = form.WPName.ToString();
                            //DateTime dt = System.DateTime.Today;
                            //row["CaptureDate"] = dt.ToString("ddMMyyyy");
                            ////row["CaptureDate"] = DateTime.Now.ToString("ddMMyyyy");

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
                //string newFolder = "Mineware_OCR";
                //string newFolderDay = newFolder + @"\" + it.ToString("yyyy'_'MM'_'dd") + "_BulkPrint";
                //Process.Start("explorer.exe", newFolderDay);
                pdfViewer1.LoadDocument(@_PrintedForm.PDFLocation);
            }
            catch (Exception error)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
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
                param.Add("PrintedByName", clsUserInfo.UserID);

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

        private void btnPrintChecklist_Click(object sender, EventArgs e)
        {
            pdfViewer1.Print();
        }
    }
}
