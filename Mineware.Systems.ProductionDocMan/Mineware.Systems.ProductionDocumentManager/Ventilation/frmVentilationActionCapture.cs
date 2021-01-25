using DevExpress.XtraEditors;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.DocumentManager.Ventilation
{
    public partial class frmVentilationActionCapture : DevExpress.XtraEditors.XtraForm
    {
        //Global varialbles
        //string repImgDir = ProductionGlobal.ProductionAmplatsGlobalTSysSettings._RepDir + @"\VentilationInspections\OtherInspections\ActionImages";
        Procedures procs = new Procedures();
        string repImgDir = @"C:\Images\Amandelbult\VentilationInspections\StandardInspections\ActionsImages";
        DialogResult result1;
        public string AllowExit;
        public string WPID = string.Empty;
        public string FlagEdit = "N";
        string ImageSaved = "N";
        string ImageDir = string.Empty;
        string TempDir = string.Empty;
        public string ActID = string.Empty;
        public string ItemSaveID = string.Empty;
        public string _theSystemDBTag;
        public string _UserCurrentInfo;
        public string Item;
        public string Type;
        string sourceFile;
        string destinationFile;
        string destinationFiletmp;
        private string FileName = string.Empty;

        public frmVentilationActionCapture()
        {
            InitializeComponent();
        }


        private void LoadEdit()
        {
            MWDataManager.clsDataAccess _ActionEdit = new MWDataManager.clsDataAccess();
            _ActionEdit.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _ActionEdit.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionEdit.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionEdit.SqlStatement = " Update tbl_Shec_IncidentsVent \r\n" +
                                       "  set  Action = '" + ActionTxt.Text + "',  Priority = '" + PriorityCmb.Text + "', CompNotes = '" + ActID + "', \r\n " +
                                       "  RespPerson = '" + RespPersonCmb.EditValue + "', HOD = '" + OverseerCmb.EditValue + "'\r\n" +
                                       "  where ID = '" + ActID + "' \r\n" +
                                       "  ";

            _ActionEdit.ExecuteInstruction();

        }

        private void SaveImage()
        {
            //PicBox.ImageLocation = null;

            string oldName = ImageDir;
            string Oldnametmp = TempDir;
            string newName = repImgDir + ItemSaveID + ".png";

            if (File.Exists(destinationFiletmp) && ImageSaved == "Y")
            {
                File.Delete(destinationFiletmp);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
        }

        private void LoadImage()
        {
            txtAttachment.EditValue = string.Empty;

            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(repImgDir);

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                if (item.ToString() == repImgDir + "\\" + ActID + ext)
                {
                    //txtAttachment.Text = item.ToString();
                    txtAttachment.EditValue = item.ToString();
                }
            }

            if (txtAttachment.EditValue.ToString() != string.Empty)
            {
                using (FileStream stream = new FileStream(txtAttachment.EditValue.ToString(), FileMode.Open, FileAccess.Read))
                {
                    PicBox.Image = Image.FromStream(stream);
                    stream.Dispose();
                }
            }

        }

        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtWorkplace.EditValue.ToString() == string.Empty)
            {
                XtraMessageBox.Show("Please select a workplace");
                return;
            }

            if (PriorityCmb.Text == string.Empty)
            {
                XtraMessageBox.Show("Please select a Priority for the Action");
                return;
            }

            if (RespPersonCmb.Text == string.Empty)
            {
                XtraMessageBox.Show("Please select a Responsible Person");
                return;
            }

            if (OverseerCmb.Text == string.Empty)
            {
                XtraMessageBox.Show("Please select a Overseer");
                return;
            }

            if (FlagEdit == "Edit")
            {
                LoadEdit();
                //SaveImage();               
                NewSaveImage();

                Close();

                return;
            }

            MWDataManager.clsDataAccess _GetID = new MWDataManager.clsDataAccess();
            _GetID.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _GetID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GetID.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GetID.SqlStatement = " select isnull( max(id)+1, 1) a from  [tbl_Shec_IncidentsVent] ";

            _GetID.ExecuteInstruction();

            ItemSaveID = _GetID.ResultsDataTable.Rows[0][0].ToString();

            //do checks
            string hasSaved = "N";



            string WP = string.Empty;
            if (cbxWorkplace.EditValue == null && cbxWorkplace.Visibility == DevExpress.XtraBars.BarItemVisibility.Always)
            {
                XtraMessageBox.Show("Please select a workplace");
                return;
            }
            if (AnswerLbl.Text == "N/A")
                WP = cbxWorkplace.EditValue.ToString();
            if (AnswerLbl.Text != "N/A")
                WP = txtWorkplace.EditValue.ToString();

            string image = string.Empty;

            if (ImageSaved == "Y")
            {
                image = ItemSaveID;
            }



            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _ActionSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionSave.SqlStatement = " INSERT INTO tbl_Shec_IncidentsVent \r\n" +
                " VALUES \r\n" +
                "((select isnull( max(id)+1, '1')a from  [tbl_Shec_IncidentsVent] ), '" + Type + "' " +
                ",'" + txtSection.EditValue + "','" + WP + "','" + Item + "','" + ActionTxt.Text + "',null,null " +
                ",'" + String.Format("{0:yyyy - MM - dd}", Datelabel.Text) + "','','','" + PriorityCmb.Text + "','" + RespPersonCmb.Text.ToString() + "',null,'" + OverseerCmb.Text.ToString() + "',null,'' " +
                ",'','','','','','','','','','','','','','','' " +
                ",'','','','','','','','','' ,'' ,'" + String.Format("{0:yyyy-MM-dd}", ReqDate.Value) + "',null,null,null,'" + clsUserInfo.UserID + "','" + image + "','' " +
                ",null,null,'','','','',null,null,'','') ";

            //_ActionSave.ExecuteInstruction();

            var ActionResult = _ActionSave.ExecuteInstruction();
            Global.sysNotification.TsysNotification.showNotification("Data Saved", "Action saved", Color.CornflowerBlue);
            


            this.Close();
        }


        private void NewSaveImage()
        {
            PicBox.ImageLocation = null;

            string oldName = ImageDir;
            string Oldnametmp = TempDir;
            string newName = repImgDir + "\\" + ItemSaveID + ".png";

            if (FlagEdit == "Edit")
            {
                newName = repImgDir + ActID + ".png";
            }

            if (File.Exists(oldName) && ImageSaved == "Y")
            {
                File.Copy(oldName, newName, true);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(oldName);

                if (FlagEdit != "N")
                    File.Delete(Oldnametmp);
            }
        }

        private void AddImBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (txtWorkplace.EditValue.ToString() == string.Empty)
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
                openFileDialog1.FileName = null;
                openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                result1 = openFileDialog1.ShowDialog();

                GetFile();
            }
            catch (Exception)
            {
                throw;
            }
        }

        void GetFile()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(repImgDir);

            if (result1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialog1.FileName;

                index = openFileDialog1.SafeFileName.IndexOf(".");


                if (txtProdMonth.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + txtProdMonth.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (txtWorkplace.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + txtWorkplace.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (txtSection.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + txtSection.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog1.SafeFileName.Substring(index);

                destinationFile = repImgDir + "\\" + FileName + Ext;
                destinationFiletmp = repImgDir + "\\" + FileName + "Tmp" + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = repImgDir + "\\" + FileName + Ext;
                            destinationFiletmp = repImgDir + "\\" + FileName + "Tmp" + Ext;
                            //if (FlagEdit == "Edit")
                            //{

                            //}
                        }
                    }

                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);

                        if (FlagEdit != "N")
                        {
                            System.IO.File.Copy(sourceFile, destinationFiletmp, true);
                        }

                        ImageSaved = "Y";
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("Error coping the file\n" + ex.Message);
                    }
                }
                else
                {
                    System.IO.File.Copy(sourceFile, repImgDir + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(repImgDir);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                if (FlagEdit != "N")
                {
                    txtAttachment.EditValue = destinationFiletmp;
                    TempDir = destinationFiletmp;
                }
                else
                {
                    txtAttachment.EditValue = destinationFile;
                }

                ImageDir = destinationFile;

                using (FileStream stream = new FileStream(txtAttachment.EditValue.ToString(), FileMode.Open, FileAccess.Read))
                {
                    PicBox.Image = Image.FromStream(stream);
                    stream.Dispose();
                }

                //PicBox.Image = Image.FromFile(txtAttachment.Text);

            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmVentilationActionCapture_Load(object sender, EventArgs e)
        {
            try
            {
                txtProdMonth.EditValue = procs.ProdMonthAsString(Convert.ToDateTime(txtProdMonth.EditValue.ToString()));
            }
            catch (Exception)
            {

            }

            ItemLbl.Text = Item;

            if (Item == "Secondary Support Requirements")
            {
                label9.Visible = true;
                SupTypeCmp.Visible = true;
                label10.Visible = true;
                textBox1.Visible = true;
            }
            else
            {
                label9.Visible = false;
                SupTypeCmp.Visible = false;
                label10.Visible = false;
                textBox1.Visible = false;
            }


            if (!String.IsNullOrEmpty(txtWorkplace.EditValue.ToString()))
            {
                //LoadSBandMo
                MWDataManager.clsDataAccess _LoadUser = new MWDataManager.clsDataAccess();
                _LoadUser.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _LoadUser.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _LoadUser.queryReturnType = MWDataManager.ReturnType.DataTable;
                _LoadUser.SqlStatement = " Select USERID + ':' + UserName Person, '" + txtSection.EditValue + "' Section from tbl_Users   ";
                _LoadUser.ExecuteInstruction();

                DataTable tbl_User = _LoadUser.ResultsDataTable;

                RespPersonCmb.Properties.DataSource = tbl_User;
                RespPersonCmb.Properties.DisplayMember = "Person";
                RespPersonCmb.Properties.ValueMember = "Person";
                RespPersonCmb.Properties.PopulateColumns();
                RespPersonCmb.Properties.Columns[0].Width = 60;
                RespPersonCmb.Properties.Columns[1].Width = 40;

                //LoadSBandMo
                MWDataManager.clsDataAccess _LoadSB = new MWDataManager.clsDataAccess();
                _LoadSB.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _LoadSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _LoadSB.queryReturnType = MWDataManager.ReturnType.DataTable;
                _LoadSB.SqlStatement = " Select USERID + ':' + UserName Person, '" + txtSection.EditValue + "' Section from tbl_Users   ";
                _LoadSB.ExecuteInstruction();

                DataTable tbl_UserSB = _LoadSB.ResultsDataTable;

                OverseerCmb.Properties.DataSource = tbl_UserSB;
                OverseerCmb.Properties.DisplayMember = "Person";
                OverseerCmb.Properties.ValueMember = "Person";
                OverseerCmb.Properties.PopulateColumns();
                OverseerCmb.Properties.Columns[0].Width = 60;
                OverseerCmb.Properties.Columns[1].Width = 40;
            }
            else
            {

                //LoadUsers
                MWDataManager.clsDataAccess _LoadUser = new MWDataManager.clsDataAccess();
                _LoadUser.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _LoadUser.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _LoadUser.queryReturnType = MWDataManager.ReturnType.DataTable;
                _LoadUser.SqlStatement = " Select USERID + ':' + UserName Person, '" + txtSection.EditValue + "' Section from tbl_Users  ";
                _LoadUser.ExecuteInstruction();

                DataTable tbl_User = _LoadUser.ResultsDataTable;

                RespPersonCmb.Properties.DataSource = tbl_User;
                RespPersonCmb.Properties.DisplayMember = "Person";
                RespPersonCmb.Properties.ValueMember = "Person";
                RespPersonCmb.Properties.PopulateColumns();
                RespPersonCmb.Properties.Columns[0].Width = 60;
                RespPersonCmb.Properties.Columns[1].Width = 40;

                //LoadSBandMo
                MWDataManager.clsDataAccess _LoadSB = new MWDataManager.clsDataAccess();
                _LoadSB.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _LoadSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _LoadSB.queryReturnType = MWDataManager.ReturnType.DataTable;
                _LoadSB.SqlStatement = " Select USERID + ':' + UserName Person, '" + txtSection.EditValue + "' Section from tbl_Users   ";
                _LoadSB.ExecuteInstruction();

                DataTable tbl_UserSB = _LoadSB.ResultsDataTable;

                OverseerCmb.Properties.DataSource = tbl_UserSB;
                OverseerCmb.Properties.DisplayMember = "Person";
                OverseerCmb.Properties.ValueMember = "Person";
                OverseerCmb.Properties.PopulateColumns();
                OverseerCmb.Properties.Columns[0].Width = 60;
                OverseerCmb.Properties.Columns[1].Width = 40;
            }

            if (FlagEdit == "Edit")
                LoadImage();

            PriorityCmb.Items.Add("A");
            PriorityCmb.Items.Add("B");
            PriorityCmb.Items.Add("C");

            SupTypeCmp.Items.Add("Wire Mesh & Lacing 1m Block Pattern");
            SupTypeCmp.Items.Add("Wire Mesh and Lacing 1.5m Diamond Pattern");
            SupTypeCmp.Items.Add("Trussers");
            SupTypeCmp.Items.Add("Long Anchors 3.6m");
            SupTypeCmp.Items.Add("Rock Pop Sets 1.5m");
            SupTypeCmp.Items.Add("Bullhorn Sets");

        }

        private void cbxWorkplace_EditValueChanged(object sender, EventArgs e)
        {
            txtWorkplace.EditValue = cbxWorkplace.EditValue.ToString();
        }
    }
}