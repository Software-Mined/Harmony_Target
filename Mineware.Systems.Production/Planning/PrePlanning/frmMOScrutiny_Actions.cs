using DevExpress.XtraEditors;
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
    public partial class frmMOScrutiny_Actions : DevExpress.XtraEditors.XtraForm
    {
        string repImgDir = string.Empty;  //Path to store Images
        DialogResult result1;

        //Private data fields
        private String sourceFile;
        private String destinationFile;
        private String FileName = string.Empty;
        

        //Public variables
        public string WPID = string.Empty;
        public string FlagEdit = "N";
        public string ActID = string.Empty;
        public string ItemSaveID = string.Empty;
        public string Section;
        public string FormType = string.Empty;
        public string Item;
        public string Type;
        public string AllowExit;

        public string UniqueDate;

        string ImageDir = string.Empty;
        string TempDir = string.Empty;

        string ImageSaved = "N";
        public string hasSaved = string.Empty;

        public string _theSystemDBTag;
        public string _UserCurrentInfo;

        

        public frmMOScrutiny_Actions()
        {
            InitializeComponent();
        }

        

        private void frmDept_Actions_Load(object sender, EventArgs e)
        {
            repImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir;

            loadImage();

            ///this.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            ///
           


            //LoadWorkplaces
            MWDataManager.clsDataAccess _LoadWP = new MWDataManager.clsDataAccess();
            _LoadWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _LoadWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadWP.SqlStatement = " Select w.Description from tbl_Planmonth pm, tbl_Workplace w, Sections_Complete sc  \r\n" +
                                    " where pm.SectionID = sc.SectionID and pm.Prodmonth = sc.Prodmonth \r\n" +
                                    " and pm.WorkplaceID = w.WorkplaceID \r\n" +
                                    " and sc.SectionID_2 = '"+lblSection.Text+"' \r\n" +
                                    " and pm.Prodmonth = '" + lblProdmonth.Text + "' \r\n";
            _LoadWP.ExecuteInstruction();

            lblWPDesc.Properties.DataSource = _LoadWP.ResultsDataTable;
            lblWPDesc.Properties.ValueMember = "Description";
            lblWPDesc.Properties.DisplayMember = "Description";

            txtReqDate.EditValue = DateTime.Now.Date;

            //LoadSBandMo
            MWDataManager.clsDataAccess _LoadUser = new MWDataManager.clsDataAccess();
            _LoadUser.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _LoadUser.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadUser.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadUser.SqlStatement = " Select USERID + ':' + Name Person, '" + Section + "' Section from [Syncromine_New].[dbo].tblUsers   ";
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
            _LoadSB.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo); ;
            _LoadSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadSB.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadSB.SqlStatement = " Select USERID + ':' + Name Person, '" + Section + "' Section from [Syncromine_New].[dbo].tblUsers    ";
            _LoadSB.ExecuteInstruction();

            DataTable tbl_UserSB = _LoadSB.ResultsDataTable;

            OverseerCmb.Properties.DataSource = tbl_UserSB;
            OverseerCmb.Properties.DisplayMember = "Person";
            OverseerCmb.Properties.ValueMember = "Person";
            OverseerCmb.Properties.PopulateColumns();
            OverseerCmb.Properties.Columns[0].Width = 60;
            OverseerCmb.Properties.Columns[1].Width = 40;

            PriorityCmb.Properties.Items.Clear();
            PriorityCmb.Properties.Items.Add("H");
            PriorityCmb.Properties.Items.Add("M");
            PriorityCmb.Properties.Items.Add("L");

            
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


        ///Get Image
        void GetImageFile()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir + "\\Startups\\Actions");
            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            string[] files = System.IO.Directory.GetFiles(repImgDir + "\\Startups\\Actions");

            if (result1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = ofdOpenImageFile.FileName;

                index = ofdOpenImageFile.SafeFileName.IndexOf(".");

                if (lblWPDesc.Text != string.Empty)
                {
                    FileName += lblWPDesc.Text;
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                if (FormType != string.Empty)
                {
                    FileName += FormType;
                }

                if (UniqueDate != string.Empty)
                {
                    FileName += String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(UniqueDate));
                }

                Ext = ofdOpenImageFile.SafeFileName.Substring(index);

                destinationFile = repImgDir + "\\Startups\\Actions" + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == ofdOpenImageFile.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            destinationFile = repImgDir + "\\Startups\\Actions" + "\\" + FileName + Ext;
                        }
                    }
                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);
                    }
                    catch { }
                }
                else
                {
                    System.IO.File.Copy(sourceFile, repImgDir + "\\Startups\\Actions" + "\\" + FileName + Ext, true);
                    dir2 = new System.IO.DirectoryInfo(repImgDir);
                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtPathAttachment.EditValue = destinationFile;
                using (FileStream stream = new FileStream(txtPathAttachment.EditValue.ToString(), FileMode.Open, FileAccess.Read))
                {
                    PicBox.Image = Image.FromStream(stream);
                    stream.Dispose();
                }

            }
        }

        public void loadImage()
        {
            txtPathAttachment.Text = string.Empty;
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir + "\\Startups\\Actions");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(repImgDir + "\\Startups\\Actions");

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                if (item.ToString() == repImgDir + "\\Startups\\Actions" + "\\" + lblWPDesc.Text + FormType + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(UniqueDate)) + ext)
                {
                    txtPathAttachment.Text = item.ToString();
                }
            }
            if (txtPathAttachment.Text != string.Empty)
            {
                using (FileStream stream = new FileStream(txtPathAttachment.Text, FileMode.Open, FileAccess.Read))
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

        

        private void SaveImage()
        {
            PicBox.ImageLocation = null;

            string oldName = ImageDir;
            string Oldnametmp = TempDir;
            string newName = repImgDir + ItemSaveID + ".png";

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

                if (FlagEdit != "EditMode = New")
                    File.Delete(Oldnametmp);
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            ofdOpenImageFile.InitialDirectory = folderBrowserDialog1.SelectedPath;
            ofdOpenImageFile.FileName = null;
            ofdOpenImageFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            result1 = ofdOpenImageFile.ShowDialog();

            GetImageFile();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveAct_Click(object sender, EventArgs e)
        {
            if (lblWPDesc.Text == string.Empty)
            {
                XtraMessageBox.Show("Please select a workplace");
                return;
            }

            if (PriorityCmb.SelectedIndex == -1)
            {
                XtraMessageBox.Show("Please select a Priority for the Action");
                return;
            }

            if (RespPersonCmb.Text == "[EditValue is null]")
            {
                XtraMessageBox.Show("Please select a Responsible Person");
                return;
            }

            if (OverseerCmb.Text == "[EditValue is null]")
            {
                XtraMessageBox.Show("Please select a Overseer");
                return;
            }

            //if (FlagEdit == "Edit")
            //{
            //	LoadEdit();
            //	SaveImage();               

            //	Close();

            //	return;
            //}

            MWDataManager.clsDataAccess _GetID = new MWDataManager.clsDataAccess();
            _GetID.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _GetID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _GetID.queryReturnType = MWDataManager.ReturnType.DataTable;
            _GetID.SqlStatement = " select max(id)+1 a from  [tbl_Shec_Incidents] ";

            _GetID.ExecuteInstruction();

            ItemSaveID = _GetID.ResultsDataTable.Rows[0][0].ToString();

            //do checks
            hasSaved = "N";

            string image = string.Empty;

            if (ImageSaved == "Y")
            {
                image = ItemSaveID;
            }

            string sqlQuery = string.Empty;
            ///Edit
            if (ActID != string.Empty)
            {
                sqlQuery = "Update tbl_Shec_Incidents \r\n" +
                    " set  Action = '" + txtRemarks.Text + "',  Priority = '" + PriorityCmb.Text + "', CompNotes = '" + ActID + "', \r\n " +
                    " RespPerson = '" + RespPersonCmb.EditValue + "', HOD = '" + OverseerCmb.EditValue + "'\r\n" +
                    " where ID = '" + ActID + "' \r\n ";
                SaveImage();
            }

            if (ActID == string.Empty)
            {
                sqlQuery = " INSERT INTO tbl_Shec_Incidents \r\n" +
                    " VALUES \r\n" +
                    "((select isnull( max(id)+1, '1')a from  [tbl_Shec_Incidents] ), 'PPMO', '" + lblSection.Text + "' " +
                    ", '" + lblWPDesc.Text + "','MO Actions','" + txtRemarks.Text + "',null,null " +
                    ",'" + String.Format("{0:yyyy-MM-dd}", txtReqDate.EditValue) + "','','','" + PriorityCmb.Text + "','" + RespPersonCmb.Text.ToString() + "',null,'" + OverseerCmb.Text.ToString() + "',null,'' " +
                    ",'','','','','','','','','','','','','','','' " +
                    ",'','','','','','','','','' ,'' ,'" + String.Format("{0:yyyy-MM-dd}", txtReqDate.EditValue) + "',null,null,null,'" + TUserInfo.UserID + "','" + image + "','' " +
                    ",null,null,'','','','',null,null,'','') ";
            }

            MWDataManager.clsDataAccess _ActionSave = new MWDataManager.clsDataAccess();
            _ActionSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo); ;
            _ActionSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _ActionSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _ActionSave.SqlStatement = sqlQuery;

            var ActionResult = _ActionSave.ExecuteInstruction();
            if (ActionResult.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Action saved", Color.CornflowerBlue);
                hasSaved = "Y";
            }

            this.Close();
        }
    }
}