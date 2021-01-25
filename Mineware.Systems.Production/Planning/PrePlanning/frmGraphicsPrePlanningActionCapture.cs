using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Planning
{
    public partial class frmGraphicsPrePlanningActionCapture : DevExpress.XtraEditors.XtraForm
    {
        OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        OpenFileDialog openFileDialog2 = new System.Windows.Forms.OpenFileDialog();

        DialogResult result1;

        public string ActID = string.Empty;

        public string ItemSaveID = string.Empty;

        public string Item;
        public string Type;
        public string AllowExit;

        public string _theSystemDBTag;
        public string _UserCurrentInfo;

        public string WPID = string.Empty;
        public string FlagEdit = "N";
        string ImageSaved = "N";

        string ImageDir = string.Empty;
        string TempDir = string.Empty;

        string sourceFile;
        string destinationFile;
        string destinationFiletmp;
        private string FileName = string.Empty;
        string RepDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage;

        public frmGraphicsPrePlanningActionCapture()
        {
            InitializeComponent();
        }

        private void LoadEdit()
        {
            MWDataManager.clsDataAccess _ActionEdit = new MWDataManager.clsDataAccess();
            _ActionEdit.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _ActionEdit.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionEdit.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionEdit.SqlStatement = " Update tbl_Shec_Incidents \r\n" +
                                       "  set  Action = '" + ActionTxt.Text + "',  Priority = '" + PriorityCmb.Text + "', CompNotes = '" + ActID + "', \r\n " +
                                       "  RespPerson = '" + RespPersonCmb.EditValue + "', HOD = '" + OverseerCmb.EditValue + "'\r\n" +
                                       "  where ID = '" + ActID + "' \r\n" +
                                       "  ";
            _ActionEdit.ExecuteInstruction();
        }

        private void SaveImage()
        {
            PicBox.ImageLocation = null;

            string oldName = ImageDir;
            string Oldnametmp = TempDir;
            string newName = RepDir + ItemSaveID + ".png";

            if (FlagEdit == "Edit")
            {
                newName = RepDir + ActID + ".png";
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

        private void frmGraphicsPrePlanningActionCapture_Load(object sender, EventArgs e)
        {
            ItemLbl.Text = Item;

            if (Item == "Secondary Support Requirements")
            {
                label9.Visible = true;
                SupTypeCmp.Visible = true;
                label10.Visible = true;
                textBox1.Visible = true;
                //SecSupPnl.Visible = true;
                //layoutControl2.Visible = true;
            }
            else
            {
                label9.Visible = false;
                SupTypeCmp.Visible = false;
                label10.Visible = false;
                textBox1.Visible = false;
                //SecSupPnl.Visible = false;
                //layoutControl2.Visible = false;
            }

            MWDataManager.clsDataAccess _LoadUser = new MWDataManager.clsDataAccess();
            _LoadUser.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _LoadUser.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadUser.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadUser.SqlStatement = " Select USERID + ':' + Name Person, '' Section from [Syncromine_New].[dbo].tblUsers   ";
            _LoadUser.ExecuteInstruction();

            DataTable tbl_User = _LoadUser.ResultsDataTable;

            RespPersonCmb.Properties.DataSource = tbl_User;
            RespPersonCmb.Properties.DisplayMember = "Person";
            RespPersonCmb.Properties.ValueMember = "Person";

            OverseerCmb.Properties.DataSource = tbl_User;
            OverseerCmb.Properties.DisplayMember = "Person";
            OverseerCmb.Properties.ValueMember = "Person";

            PriorityCmb.Items.Add("H");
            PriorityCmb.Items.Add("M");
            PriorityCmb.Items.Add("L");

            SupTypeCmp.Items.Add("Wire Mesh & Lacing 1m Block Pattern");
            SupTypeCmp.Items.Add("Wire Mesh and Lacing 1.5m Diamond Pattern");
            SupTypeCmp.Items.Add("Trussers");
            SupTypeCmp.Items.Add("Long Anchors 3.6m");
            SupTypeCmp.Items.Add("Rock Pop Sets 1.5m");
            SupTypeCmp.Items.Add("Bullhorn Sets");

            LoadImage();
        }

        private void WPEdit2_EditValueChanged(object sender, EventArgs e)
        {
            WPID = WPEdit2.EditValue.ToString();
        }

        private void btnAddImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (WPID == string.Empty)
            {
                MessageBox.Show("Please select a workplace");
                return;
            }

            openFileDialog3.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialog3.FileName = null;
            openFileDialog3.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            result1 = openFileDialog3.ShowDialog();

            GetFile();
        }

        void GetFile()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\Actions");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(RepDir + "\\Preplanning\\Actions");

            if (result1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialog3.FileName;

                index = openFileDialog3.SafeFileName.IndexOf(".");

                if (WPEdit2.EditValue.ToString() != string.Empty)
                {
                    string WP = string.Empty;

                    if (AnswerLbl.Text == "N/A")
                        WP = WPEdit2.EditValue.ToString();
                    if (AnswerLbl.Text != "N/A")
                        WP = tbWorkplace.Text;

                    FileName = ProductionGlobal.ProductionGlobal.ExtractAfterColon(WP);
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (tbPM.Text != string.Empty)
                {
                    FileName = FileName + tbPM.Text;
                }

                Ext = openFileDialog3.SafeFileName.Substring(index);

                destinationFile = RepDir + "\\Preplanning\\Actions" + "\\" + FileName + Ext;
                destinationFiletmp = RepDir + "\\Preplanning\\Actions" + "\\" + FileName + "Tmp" + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog3.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = RepDir + "\\Preplanning\\Actions" + "\\" + FileName + Ext;
                            destinationFiletmp = RepDir + "\\Preplanning\\Actions" + "\\" + FileName + "Tmp" + Ext;
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
                    catch
                    {

                    }
                }
                else
                {
                    System.IO.File.Copy(sourceFile, RepDir + "\\Preplanning\\Actions" + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\Actions");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                if (FlagEdit != "N")
                {
                    txtAttachment.Text = destinationFiletmp;
                    TempDir = destinationFiletmp;
                }
                else
                {
                    txtAttachment.Text = destinationFile;
                }

                ImageDir = destinationFile;

                using (FileStream stream = new FileStream(txtAttachment.Text, FileMode.Open, FileAccess.Read))
                {
                    PicBox.Image = Image.FromStream(stream);
                    stream.Dispose();
                }
            }
        }

        private void LoadImage()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\Actions");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(RepDir + "\\Preplanning\\Actions");

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                if (item.ToString() == RepDir + "\\Preplanning\\Actions" + "\\" + ActID + ext)
                {
                    txtAttachment.Text = item.ToString();
                }
            }

            if (txtAttachment.Text != string.Empty)
            {
                using (FileStream stream = new FileStream(txtAttachment.Text, FileMode.Open, FileAccess.Read))
                {
                    PicBox.Image = Image.FromStream(stream);
                    stream.Dispose();
                }
            }

        }

        private void btnSaveAct_Click(object sender, EventArgs e)
        {
            if (WPID == "label2")
            {
                MessageBox.Show("Please select a workplace");
                return;
            }

            if (PriorityCmb.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Priority for the Action");
                return;
            }

            if (RespPersonCmb.Text == string.Empty)
            {
                MessageBox.Show("Please select a Responsible Person");
                return;
            }

            if (OverseerCmb.Text == string.Empty)
            {
                MessageBox.Show("Please select a Overseer");
                return;
            }

            if (FlagEdit == "Edit")
            {
                LoadEdit();
                SaveImage();
                Close();
                return;
            }

            MWDataManager.clsDataAccess _GetID = new MWDataManager.clsDataAccess();
            _GetID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _GetID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GetID.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GetID.SqlStatement = " select isnull(max(id)+1,1) a from  [tbl_Shec_Incidents] where ID not like 'WED%'";

            _GetID.ExecuteInstruction();

            ItemSaveID = _GetID.ResultsDataTable.Rows[0][0].ToString();

            //do checks
            string hasSaved = "N";

            string WP = string.Empty;

            if (AnswerLbl.Text == "N/A")
                WP = WPEdit2.EditValue.ToString();
            if (AnswerLbl.Text != "N/A")
                WP = tbWorkplace.Text.ToString();

            string image = string.Empty;

            if (ImageSaved == "Y")
            {
                image = ItemSaveID;
            }

            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _ActionSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionSave.SqlStatement = " INSERT INTO tbl_Shec_Incidents \r\n" +
                " VALUES \r\n" +
                "('" + ItemSaveID + "', '" + Type + "' \r\n" +
                ",'" + tbSections.Text + "','" + WP + "','" + Item + "','" + ActionTxt.Text + "',null,null \r\n" +
                ",'" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + "','','','" + PriorityCmb.Text + "','" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(RespPersonCmb.Text.ToString()) + "',null,'" + ProductionGlobal.ProductionGlobal.ExtractBeforeColon(OverseerCmb.Text.ToString()) + "',null,'' \r\n" +
                ",'','','','','','','','','','','','','','',''  \r\n" +
                ",'','','','','','','','','' ,'' ,'" + String.Format("{0:yyyy-MM-dd}", ReqDate.Value) + "',null,null,null,'" + TUserInfo.UserID + "','" + image + "',''  \r\n" +
                ",null,null,'','','','',null,null,'','') ";

            var ActionResult = _ActionSave.ExecuteInstruction();
            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Action saved", Color.CornflowerBlue);

                SaveImage();
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            btnAddImg_ItemClick(null, null);
        }

        private void PicBox_Click(object sender, EventArgs e)
        {

        }
    }
}