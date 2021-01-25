using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace Mineware.Systems.Production.Departmental.Ventillation
{
    public partial class frmVentOtherChecklist : DevExpress.XtraEditors.XtraForm
    {
        #region Data Fields

        public string _theSystemDBTag;
        public string _UserCurrentInfo;
        public string checklisID;
        public string _monthDate;
        public string _workPlace;
        public string _section;

        string ext;
        string repDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\OtherInspections\DocumentFiles";    //Path to store files
        string repImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\OtherInspections\Images";  //Path to store Images
        string ActionsImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\OtherInspections\ActionImages";
        DialogResult fileResults;

        private String ID = string.Empty;
        private String Workplace = string.Empty;
        private String Description = string.Empty;
        private String Recomendation = string.Empty;
        private String Priority = string.Empty;
        private String TargetDate = string.Empty;
        private String RespPerson = string.Empty;
        private String Overseer = string.Empty;

        DialogResult result1;

        string section;
        string workPlace;

        string FirstLoad = "Y";

        //Private data fields
        private String FileName = string.Empty;
        private String sourceFile;
        private String destinationFile;

        private DataTable dtActions = new DataTable("dtActions");

        //Public Declarations
        //Tables
        public DataSet dsGlobal = new DataSet();

        /// <summary>
        /// Object declaration and inicialisation
        /// </summary>
        public string chkListName;

        #endregion

        #region Constructor

        public frmVentOtherChecklist()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods / Functions

        private void sqlConnector(string sqlQuery, string sqlTableIdentifier)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            //_sqlConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection); //Fix Inheritance "ucBaseControl Class"
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

        void CHeckAuth()
        {
            MWDataManager.clsDataAccess _CheckAuth = new MWDataManager.clsDataAccess();
            _CheckAuth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _CheckAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _CheckAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _CheckAuth.SqlStatement = "Select * from tbl_Dept_Inspection_VentAuthorised_OtherChecklists \r\n" +
                                      "where  CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + "' and SectionID = '" + cbxSection.EditValue.ToString() + "' and checklistID = (select CheckListID from tbl_Dept_Vent_OtherChecklist where CheckListName = '" + cbxChecklistId.EditValue.ToString() + "')  and workplaceid = (select WorkplaceID from tbl_workplace where Description = '" + workPlace + "') ";
            _CheckAuth.ExecuteInstruction();

            if (_CheckAuth.ResultsDataTable.Rows.Count > 0)
            {
                btnAuthOther.Caption = "UnAuthorise";

                for (int col = 0; col < gvEnvCondControl.Columns.Count; col++)
                {
                    gvEnvCondControl.Columns[col].OptionsColumn.AllowEdit = false;
                }

                //for (int col = 0; col < gvEmergencyControl.Columns.Count; col++)
                //{
                //	gvEmergencyControl.Columns[col].OptionsColumn.AllowEdit = false;
                //}

                //for (int col = 0; col < gvFireControl.Columns.Count; col++)
                //{
                //	gvFireControl.Columns[col].OptionsColumn.AllowEdit = false;
                //}

                //for (int col = 0; col < gvGenControl.Columns.Count; col++)
                //{
                //	gvGenControl.Columns[col].OptionsColumn.AllowEdit = false;
                //}

                for (int col = 0; col < gvAction.Columns.Count; col++)
                {
                    gvAction.Columns[col].OptionsColumn.AllowEdit = false;
                }



                SaveBtn.Enabled = false;
                AddImBtn.Enabled = false;
                btnAddDocument.Enabled = false;

                btnAddActReg.Enabled = false;
                btnEditAcReg.Enabled = false;
                btnRemActReg.Enabled = false;

                AddActBtn.Enabled = false;
                EditActBtn.Enabled = false;
                DelActBtn.Enabled = false;
            }
            else
            {
                btnAuthOther.Caption = "Authorise";

                for (int col = 0; col < gvEnvCondControl.Columns.Count; col++)
                {
                    gvEnvCondControl.Columns[col].OptionsColumn.AllowEdit = true;
                }

                //for (int col = 0; col < gvEmergencyControl.Columns.Count; col++)
                //{
                //	gvEmergencyControl.Columns[col].OptionsColumn.AllowEdit = true;
                //}

                //for (int col = 0; col < gvFireControl.Columns.Count; col++)
                //{
                //	gvFireControl.Columns[col].OptionsColumn.AllowEdit = true;
                //}

                //for (int col = 0; col < gvGenControl.Columns.Count; col++)
                //{
                //	gvGenControl.Columns[col].OptionsColumn.AllowEdit = true;
                //}

                for (int col = 0; col < gvAction.Columns.Count; col++)
                {
                    gvAction.Columns[col].OptionsColumn.AllowEdit = true;
                }


                SaveBtn.Enabled = true;
                AddImBtn.Enabled = true;
                btnAddDocument.Enabled = true;

                btnAddActReg.Enabled = true;
                btnEditAcReg.Enabled = true;
                btnRemActReg.Enabled = true;
            }
        }

        //Load data from database
        public void loadData()
        {
            ///Fire protection
            colNumber.Visible = false;
            colCondition.Visible = false;
            colExDate.Visible = false;
            colInstSide.Visible = false;
            colSigns.Visible = false;
            colRem1.Visible = false;
            colElectrical.Visible = false;
            colWaste.Visible = false;
            colCombustM.Visible = false;
            colOther.Visible = false;
            colRem2.Visible = false;
            colElectCables.Visible = false;
            colOtherElect.Visible = false;
            colPlastic.Visible = false;
            colFireBrk.Visible = false;
            colFireLocal.Visible = false;
            colPosition.Visible = false;
            colDistance.Visible = false;
            colAutoFire.Visible = false;
            colLastInspDate.Visible = false;

            ///Emergency Table
            colQuestionID.Visible = false;
            colQuestion1.Visible = false;
            colQuestion2.Visible = false;
            colQuestion3.Visible = false;
            colAnswer1.Visible = false;
            colAnswer2.Visible = false;
            colAnswer3.Visible = false;


            //General Table
            colRefN.Visible = false;
            colMini1.Visible = false;
            colMini2.Visible = false;
            colMini3.Visible = false;
            colMini4.Visible = false;
            colMini5.Visible = false;
            colMini6.Visible = false;
            colMini7.Visible = false;
            colMini8.Visible = false;
            colMini9.Visible = false;
            colMini10.Visible = false;
            colMini11.Visible = false;
            colMini12.Visible = false;
            colMini13.Visible = false;
            colMini14.Visible = false;

            colSub1.Visible = false;
            colSub2.Visible = false;
            colSub3.Visible = false;
            colSub4.Visible = false;
            colSub5.Visible = false;
            colSub6.Visible = false;
            colSub7.Visible = false;
            colSub8.Visible = false;
            colSub9.Visible = false;
            colSub10.Visible = false;
            colSub11.Visible = false;
            colSub12.Visible = false;
            colSub13.Visible = false;
            colSub14.Visible = false;

            //Other General
            colWaterSub.Visible = false;
            colFireMethod.Visible = false;
            colFireHydrant.Visible = false;
            colFullLenngth.Visible = false;
            colWaterPressure.Visible = false;
            colReturnBelt.Visible = false;
            colSurfAmount.Visible = false;
            colSealArrange.Visible = false;
            colTransBunded.Visible = false;
            colTransType.Visible = false;


            //Group Controls
            lgcEmergency.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lgcFireProtection.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lgcGeneral.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            colAnswer1.Caption = "Answer";
            colAnswer2.Caption = "Answer";
            colAnswer3.Caption = "Answer";

            string checkListID = "(select CheckListID from tbl_Dept_Vent_OtherChecklist where CheckListName = '" + chkListName + "')";
            string img = string.Empty;
            string doc = string.Empty;

            if (ext == ".jpg" && ext != string.Empty && ext != null)
            {
                img = repImgDir + "\\" + cbxChecklistId.EditValue.ToString() + cbxWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + ext;
                doc = repImgDir + "\\" + cbxChecklistId.EditValue.ToString() + cbxWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + ext;
            }
            else
            {
                img = repImgDir + "\\" + cbxChecklistId.EditValue.ToString() + cbxWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + ext;
                doc = repImgDir + "\\" + cbxChecklistId.EditValue.ToString() + cbxWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + ext;
            }


            PicBox.Image = null;
            DocsLB.Items.Clear();
            try
            {
                Bitmap MyImage1 = new Bitmap(img);
                PicBox.Image = (Image)MyImage1;
                txtAttachment.EditValue = img;
            }

            catch { }


            CHeckAuth();
            loadDocs();
            LoadActions();

            //Environmental Conditions
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = " exec sp_Dept_Insection_Vent_OtherQuestions '" + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + "', \r\n "
                + " '" + cbxSection.EditValue + "', '" + cbxWorkplace.EditValue.ToString() + "', '" + chkListName + "' \r\n ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt1 = _dbMan.ResultsDataTable;

            gvEnvCondition.DataSource = dt1;

            colQuestID.FieldName = "QuestionID";
            colSubCategory.FieldName = "SubCategory";
            colQuestion.FieldName = "QuestionDescription";
            colAnswer.FieldName = "Answer";

            if (chkListName == "Battery Bay" || chkListName == "UG Conveyor Belt")
            {
                colQuestion.Width = 600;
                colSubCategory.Width = 150;
            }

            if (chkListName == "Sub Station" || chkListName == "Mini Subs"
                || chkListName == "Refuge Bay" || chkListName == "UG Store")
            {
                colQuestion.Width = 500;
                colSubCategory.Width = 150;
            }

            if (chkListName == "Sub Station" || chkListName == "Workshop - Completed")
            {
                colQuestion.Width = 400;
                colSubCategory.Width = 150;
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

                sourceFile = ofdOpenImageFile.FileName;


                index = ofdOpenImageFile.SafeFileName.IndexOf(".");

                if (cbxChecklistId.EditValue.ToString() != string.Empty)
                {
                    FileName = ProductionGlobal.ProductionGlobal.ExtractAfterColon(cbxChecklistId.EditValue.ToString());
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (!cbxWorkplace.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + cbxWorkplace.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                if (txtProdMonth.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + string.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue);
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = ofdOpenImageFile.SafeFileName.Substring(index);

                destinationFile = repImgDir + "\\" + FileName + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == ofdOpenImageFile.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = repImgDir + "\\" + FileName + Ext;
                        }
                    }

                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    destinationFile = repImgDir + "\\" + FileName + Ext;
                    System.IO.File.Copy(sourceFile, destinationFile, true);

                    dir2 = new System.IO.DirectoryInfo(repImgDir);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                }

                txtAttachment.EditValue = destinationFile;
                PicBox.Image = Image.FromFile(ofdOpenImageFile.FileName);

            }
        }

        void loadImage()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(repImgDir);

            if (PicBox.Image != null)
            {
                PicBox.Image.Dispose();
                PicBox.Image = null;
            }


            if (txtAttachment.EditValue.ToString() != string.Empty)
            {
                using (FileStream stream = new FileStream(txtAttachment.EditValue.ToString(), FileMode.Open, FileAccess.Read))
                {
                    PicBox.Image = System.Drawing.Image.FromStream(stream);
                    stream.Dispose();
                }
            }
        }

        void GetDoc()
        {
            string mianDicrectory = repDir;

            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);
            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(mianDicrectory);

            if (fileResults == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                string SourcefileName = string.Empty;

                int index = 0;

                sourceFile = openFileDialog1.FileName;

                index = openFileDialog1.SafeFileName.IndexOf(".");

                SourcefileName = openFileDialog1.SafeFileName.Substring(0, index);

                if (cbxChecklistId.EditValue.ToString() != string.Empty)
                {
                    FileName = ProductionGlobal.ProductionGlobal.ExtractAfterColon(cbxChecklistId.EditValue.ToString());
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (!cbxWorkplace.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + cbxWorkplace.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                if (txtProdMonth.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + string.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue);
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }


                Ext = openFileDialog1.SafeFileName.Substring(index);

                destinationFile = mianDicrectory + "\\" + FileName + SourcefileName + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            destinationFile = mianDicrectory + "\\" + FileName + Ext;
                        }
                    }

                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);
                        DocsLB.Items.Add(SourcefileName + Ext);
                    }
                    catch { }
                }
                else
                {
                    System.IO.File.Copy(sourceFile, mianDicrectory + "\\" + FileName + SourcefileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(mianDicrectory);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                    DocsLB.Items.Add(SourcefileName + Ext);
                }
            }
        }

        public void loadDocs()
        {

            Random r = new Random();

            string mianDicrectory = repDir;

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);


            string[] files = System.IO.Directory.GetFiles(mianDicrectory);

            //Do everywhere
            DocsLB.Items.Clear();

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                ext = aa.Substring(extpos, aa.Length - extpos);

                int indexa = item.LastIndexOf("\\");

                string sourcefilename = item.Substring(indexa + 1, (item.Length - indexa) - 1);

                int indexprodmonth = sourcefilename.IndexOf(String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue));

                string SourcefileCheck = sourcefilename.Substring(0, indexprodmonth + 10);

                int NameLength = sourcefilename.Length - SourcefileCheck.Length;

                string Docsname = sourcefilename.Substring(SourcefileCheck.Length, NameLength);

                if (SourcefileCheck == cbxChecklistId.EditValue.ToString() + cbxWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue))
                {
                    DocsLB.Items.Add(Docsname);
                }

            }
            //DocsLB.Items.Add(Docsname.ToString());
        }

        //Gets Actions
        private void LoadActions()
        {
            //Declarations
            string sql = string.Empty;

            sql = "select * from  tbl_Shec_IncidentsVent where Workplace = '" + cbxWorkplace.EditValue.ToString() + "' and TheDate = '" + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + "' ";

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = sql;
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt1 = _dbMan.ResultsDataTable;
            DataSet ds1 = new DataSet();


            ds1.Tables.Add(dt1);
            gcActions.DataSource = ds1.Tables[0];

            gcActID.FieldName = "ID";
            gcWorkplace.FieldName = "Workplace";
            gcDescription.FieldName = "Description";
            gcRecommendation.FieldName = "Action";
            gcTargetDate.FieldName = "TargetDate";
            gcPriority.FieldName = "Priority";
            gcImage.FieldName = "CompNotes";
            gcViewImage.FieldName = "Hyperlink";
            gcRespPerson.FieldName = "RespPerson";
            gcOverseer.FieldName = "HOD";
        }

        void CalcTotalPercent()
        {
            double ScoreSum = 0, percentage = 0;
            if (chkListName == "Refuge Chamber Report" && doCalc == "Y")
            {
                doCalc = "N";
                for (int i = 0; i < gvEnvCondControl.RowCount; i++)
                {
                    string score = gvEnvCondControl.GetRowCellValue(i, gvEnvCondControl.Columns["YesNo"]).ToString();
                    if (score == "Y" || score == "N")
                    {
                        if (score == "Y")
                        {
                            ScoreSum += 1;
                        }

                        if (score == "N" && ScoreSum > 0)
                        {
                            ScoreSum -= 1;
                        }

                    }
                }

                percentage = ((ScoreSum / Convert.ToDouble(30)) * 100);
                doCalc = "N";
            }

            //doCalc = "N";
            gvEnvCondControl.SetRowCellValue(39, gvEnvCondControl.Columns["YesNo"], (Math.Round(percentage, 0) + " %"));
            doCalc = "Y";
        }

        #endregion


        #region Events

        //Use this for Syncromine
        private void cbxChecklistId_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Get the item's selected index
            //checkID = repositoryItemComboBox4.Items.IndexOf(e.Item);

            chkListName = "select CheckListID from tbl_Dept_Vent_OtherChecklist where CheckListName = '"
                + ricbxCheckList.Items.IndexOf(e) + "'";
            loadData();
        }

        private void frmVentOtherChecklist_Load(object sender, EventArgs e)
        {
            txtProdMonth.EditValue = DateTime.Now.Date;

            cbxChecklistId.EditValue = chkListName;

            ////Start Windows Form in Fullscreen
            //this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;

            ////Add workplaces into combobox
            ///
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = "Select Description wDes from tbl_workplace where workplaceid = '" + cbxWorkplace.EditValue.ToString() + "'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable wDat = _dbMan.ResultsDataTable;

            ////Load data into the workplace dropdown
            foreach (DataRow dr in wDat.Rows)
            {
                cbxWorkplace.EditValue = dr["wDes"].ToString();
            }

            ////Add Section into combobox
            ///
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = "select SectionID secID from tbl_SECTION where prodmonth = (select max( prodmonth)pm from tbl_section ) and Hierarchicalid = '4' order by sectionid ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();


            DataTable secDat = _dbMan.ResultsDataTable;

            ////Load data into the checklist dropdown
            foreach (DataRow dr in secDat.Rows)
            {
                ricbxSection.Items.Add(dr["secID"].ToString());
            }
            //string aa = secDat.Rows[0][0].ToString();

            cbxSection.EditValue = secDat.Rows[0][0].ToString();

            _workPlace = cbxWorkplace.EditValue.ToString();
            _section = cbxSection.EditValue.ToString();

            chkListName = cbxChecklistId.EditValue.ToString();

            section = cbxSection.EditValue.ToString();
            workPlace = cbxWorkplace.EditValue.ToString();

            MWDataManager.clsDataAccess _CheckAUthSecurity = new MWDataManager.clsDataAccess();
            _CheckAUthSecurity.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _CheckAUthSecurity.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _CheckAUthSecurity.queryReturnType = MWDataManager.ReturnType.DataTable;
            _CheckAUthSecurity.SqlStatement = "  Select ProfileID from [Syncromine_New].[dbo].tblUsers users  \r\n" +
                                       ",[Syncromine_New].[dbo].[tblUserProfileLink] link \r\n" +
                                       "  where users.USERID = link.UserID and users.USERID = '" + TUserInfo.UserID + "'";
            _CheckAUthSecurity.ExecuteInstruction();

            btnAuthOther.Enabled = false;

            DataTable dtRights = _CheckAUthSecurity.ResultsDataTable;

            foreach (DataRow dr in dtRights.Rows)
            {
                if (dr["ProfileID"].ToString() == "SYSADMIN"
                   || dr["ProfileID"].ToString() == "MRMVentAut")
                {
                    btnAuthOther.Enabled = true;
                }
            }

            FirstLoad = "N";

            loadData();
        }

        private void cbxChecklistId_EditValueChanged(object sender, EventArgs e)
        {
            chkListName = cbxChecklistId.EditValue.ToString();
            if (FirstLoad == "N")
                loadData();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ///Environmental Condition
            try
            {
                gvEnvCondControl.PostEditor();

                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
                _dbMan.SqlStatement = "  declare @checkListID varchar(50)\r\n " +
                    " set @checkListID = (select CheckListID from tbl_Dept_Vent_OtherChecklist where CheckListName = '" + chkListName + "') \r\n " +
                    " delete from tbl_Dept_Vent_OtherQuestionAnswer \r\n " +
                    " where ChecklistID = @checkListID  \r\n " +
                    " and Workplace = '" + workPlace + "' and Month = '" + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + "' \r\n ";

                //Looping through the GridView row data
                for (int counter = 0; counter < gvEnvCondControl.RowCount; counter++)
                {
                    string QuestID = gvEnvCondControl.GetRowCellValue(counter, gvEnvCondControl.Columns["QuestionID"]).ToString();
                    string Answer = gvEnvCondControl.GetRowCellValue(counter, gvEnvCondControl.Columns["Answer"]).ToString();

                    if (gvEnvCondControl.GetRowCellValue(counter, "ValueType").ToString() == "DateTime")
                    {
                        Answer = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Answer));
                    }


                    _dbMan.SqlStatement += "\r\n insert into tbl_Dept_Vent_OtherQuestionAnswer(Month, Section, Workplace, CheckListID, QuestionID, Answer) \r\n "
                        + " values('" + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + "', '" + cbxSection.EditValue + "', \r\n "
                        + " '" + cbxWorkplace.EditValue.ToString() + "', @checkListID , " + QuestID + ", '" + Answer + "') \r\n";
                }

                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;

                var results = _dbMan.ExecuteInstruction();

                if (results.success)
                {
                    Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Save", "Data saved succesful.", Color.CornflowerBlue);

                }
                else
                {
                    Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Save", "Checklist " + chkListName.ToString() + ".\nEnvironmental conditions record did not save", Color.Red);
                }

            }
            catch (Exception ex)
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Information", "Environmental \n" + ex.Message, Color.Red);
            }

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void cbxChecklistId_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnLoadReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtAttachment.EditValue == null)
            {
                txtAttachment.EditValue = string.Empty;
            }

            Form frmReport = new Form();

            ucVentChecklistReport ucReport = new ucVentChecklistReport();
            ucReport.theSystemDBTag = _theSystemDBTag;
            ucReport.UserCurrentInfo.Connection = _UserCurrentInfo;
            ucReport._ucCheckListID = chkListName;
            ucReport._ucMonthDate = String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue);
            ucReport._ucWorkPlace = workPlace;
            ucReport._ucSection = section;
            ucReport._FrmType = "Other";
            if (txtAttachment.EditValue.ToString() != string.Empty)
            {
                ucReport._picPath = txtAttachment.EditValue.ToString();
            }

            frmReport.Controls.Add(ucReport);
            frmReport.WindowState = FormWindowState.Maximized;
            frmReport.StartPosition = FormStartPosition.CenterParent;
            frmReport.Text = chkListName + " Ventilation Report";
            frmReport.Icon = new System.Drawing.Icon(@"C:\MIMSNew\Images\SM.ico");
            ucReport.Dock = DockStyle.Fill;
            frmReport.Show();
        }

        private void gvEnvCondControl_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            ///Refuge Chamber Calc
            ///
            //decimal devisor = Convert.ToDecimal(30);
            //decimal Compliance;

            //int row = Convert.ToInt32(gvEnvCondControl.GetRowCellValue(e.RowHandle, gvEnvCondControl.Columns["QuestionID"]).ToString() == "109"? e.RowHandle : gvEnvCondControl.FocusedRowHandle);
            //if (gvEnvCondControl.GetRowCellValue(e.RowHandle, gvEnvCondControl.Columns["QuestionID"]).ToString() == "108")
            //{
            //Compliance = (Math.Round(Convert.ToDecimal(gvEnvCondControl.GetRowCellValue(e.RowHandle, "YesNo")) / devisor, 1));

            //	gvEnvCondControl.SetRowCellValue(e.RowHandle, gvEnvCondControl.Columns["QuestionID"], Compliance);
            //} 


        }

        private void AddImBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ofdOpenImageFile.InitialDirectory = folderBrowserDialog1.SelectedPath;
            ofdOpenImageFile.FileName = null;
            ofdOpenImageFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            result1 = ofdOpenImageFile.ShowDialog();

            GetFile();
        }

        private void btnAddDocument_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fileResults = openFileDialog1.ShowDialog();
            GetDoc();
        }

        private void ofdOpenDocFile_FileOk(object sender, CancelEventArgs e)
        {
            string Docs = openFileDialog1.FileName;

            int indexa = Docs.LastIndexOf("\\");

            string sourcefilename = Docs.Substring(indexa + 1, (Docs.Length - indexa) - 1);

            //DocsLB.Items.Add(procs.ExtractAfterColon(CrewEdit.EditValue.ToString()) + PMEdit.EditValue.ToString() + sourcefilename);
            DocsLB.Items.Add(sourcefilename);
        }

        private void DocsLB_DoubleClick(object sender, EventArgs e)
        {
            string mianDicrectory = repDir;
            if (DocsLB.SelectedIndex != -1)
            {
                string test = mianDicrectory + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(cbxChecklistId.EditValue.ToString()) + cbxWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + DocsLB.SelectedItem.ToString();

                System.Diagnostics.Process.Start(mianDicrectory + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(cbxChecklistId.EditValue.ToString()) + cbxWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + DocsLB.SelectedItem.ToString());
            }
        }

        private void btnDocRemove_Click(object sender, EventArgs e)
        {
            string mianDicrectory = repDir;
            if (DocsLB.SelectedIndex != -1)
            {
                string test = mianDicrectory + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(cbxChecklistId.EditValue.ToString()) + cbxWorkplace.EditValue.ToString() + txtProdMonth.EditValue.ToString() + DocsLB.SelectedItem.ToString();

                System.Diagnostics.Process.Start(mianDicrectory + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(cbxChecklistId.EditValue.ToString()) + cbxWorkplace.EditValue.ToString() + txtProdMonth.EditValue.ToString() + DocsLB.SelectedItem.ToString());
            }
        }

        private void btnImgRemove_Click(object sender, EventArgs e)
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);


            string[] files = System.IO.Directory.GetFiles(repImgDir);

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                string day = String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue);

                string Test = repImgDir + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(cbxChecklistId.EditValue.ToString()) + cbxWorkplace.EditValue.ToString() + day + ext;

                string img = @repImgDir + "\\" + cbxChecklistId.EditValue.ToString() + cbxWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + ".png";

                try
                {
                    File.Delete(img);
                    PicBox.Image = null;
                }
                catch (Exception)
                {


                }


            }

        }

        private void btnAddRegister_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (string.IsNullOrEmpty(cbxSection.EditValue.ToString()))
            {
                XtraMessageBox.Show("Please select a section");
                return;
            }


            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            //ActFrm.txtProdMonth.EditValue = String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue);
            //ActFrm.txtSection.EditValue = cbxSection.EditValue;
            //ActFrm.txtWorkplace.EditValue = cbxWorkplace.EditValue;
            //ActFrm.PriorityCmb.Items.Add("A");
            //ActFrm.PriorityCmb.Items.Add("B");
            //ActFrm.PriorityCmb.Items.Add("C");

            ActFrm._theSystemDBTag = this._theSystemDBTag;
            ActFrm._UserCurrentInfo = this._UserCurrentInfo;

            ActFrm.Item = "Ventilation Actions";
            ActFrm.Type = "PPEG";
            //ActFrm.AllowExit = "Y";


            ActFrm.StartPosition = FormStartPosition.CenterScreen;

            ActFrm.ShowDialog(this);



            //LoadActions();

        }

        private void gvEnvCondControl_CustomDrawCell_1(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption == "Answer")
            {
                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == string.Empty)
                {
                    e.Appearance.BackColor = Color.MistyRose;
                }

                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Free Text")
                {
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }

                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Unit"
                    || gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Decimal(1)"
                    || gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Numeric(10)")
                {
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                }

                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "DateTime")
                {
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }

            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            //ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtProdMonth.EditValue.ToString()));
            //ActFrm.txtSection.EditValue = cbxSection.EditValue.ToString();
            //ActFrm.txtWorkplace.EditValue = cbxWorkplace.EditValue.ToString();
            //ActFrm.AnswerLbl.Text = "Other Vent";

            //ActFrm.txtWorkplace.EditValue = cbxWorkplace.EditValue.ToString();

            //ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //ActFrm.txtWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            ActFrm._theSystemDBTag = _theSystemDBTag;
            ActFrm._UserCurrentInfo = _UserCurrentInfo;

            ActFrm.Item = "Vent Schedule Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";


            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();

        }

        private void gvEmergencyControl_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "Yes/No")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Yes");
                    ritem.Items.Add("No");
                    ritem.Items.Add("N/A");

                    e.RepositoryItem = ritem;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "Yes/No")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Yes");
                    ritem.Items.Add("No");
                    ritem.Items.Add("N/A");
                    e.RepositoryItem = ritem;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "Full")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    e.RepositoryItem = repFullAvailable;
                }
            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "NoneObserved")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    e.RepositoryItem = repObserved;
                }
            }



            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "Yes/No")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Yes");
                    ritem.Items.Add("No");
                    ritem.Items.Add("N/A");

                    e.RepositoryItem = ritem;
                }

            }

            ///Displayed
            ///

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "Displayed")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();

                    e.RepositoryItem = repNotDiplay;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "Displayed")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();

                    e.RepositoryItem = repNotDiplay;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "Displayed")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();


                    e.RepositoryItem = repNotDiplay;
                }

            }


            ///Required
            ///

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "Required")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Required");
                    ritem.Items.Add("Not Installed");
                    ritem.Items.Add("Installed");


                    e.RepositoryItem = ritem;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "Required")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Required");
                    ritem.Items.Add("Not Installed");
                    ritem.Items.Add("Installed");


                    e.RepositoryItem = ritem;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "Required")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Required");
                    ritem.Items.Add("Not Installed");
                    ritem.Items.Add("Installed");


                    e.RepositoryItem = ritem;
                }

            }


            ///In Order
            ///

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "InOrder")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("In Order");
                    ritem.Items.Add("Not In Order");
                    ritem.Items.Add("N/A");

                    e.RepositoryItem = ritem;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "InOrder")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("In Order");
                    ritem.Items.Add("Not In Order");
                    ritem.Items.Add("N/A");

                    e.RepositoryItem = ritem;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "InOrder")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("In Order");
                    ritem.Items.Add("Not In Order");
                    ritem.Items.Add("N/A");


                    e.RepositoryItem = ritem;
                }

            }

            ///NoneObseved
            ///

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "NoneObseved")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("None Obseved");
                    ritem.Items.Add("Satisfactory");
                    ritem.Items.Add("Non Satisfactory");

                    e.RepositoryItem = ritem;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "NoneObseved")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("None Obseved");
                    ritem.Items.Add("Satisfactory");
                    ritem.Items.Add("Non Satisfactory");

                    e.RepositoryItem = ritem;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "NoneObseved")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("None Obseved");
                    ritem.Items.Add("Satisfactory");
                    ritem.Items.Add("Non Satisfactory");


                    e.RepositoryItem = ritem;
                }

            }

            ///FanSound
            ///

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "FanSound")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Required");
                    ritem.Items.Add("Not Installed");
                    ritem.Items.Add("Installed");
                    ritem.Items.Add("Fan Sound Attenuated");

                    e.RepositoryItem = ritem;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "FanSound")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Required");
                    ritem.Items.Add("Not Installed");
                    ritem.Items.Add("Installed");
                    ritem.Items.Add("Fan Sound Attenuated");
                    e.RepositoryItem = ritem;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "FanSound")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Required");
                    ritem.Items.Add("Not Installed");
                    ritem.Items.Add("Installed");
                    ritem.Items.Add("Fan Sound Attenuated");

                    e.RepositoryItem = ritem;
                }

            }


            ///Clean
            ///

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "Clean")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Clean & Impervious");
                    ritem.Items.Add("Not Concreted");
                    ritem.Items.Add("Concreted & Dirty");

                    e.RepositoryItem = ritem;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "Clean")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Clean & Impervious");
                    ritem.Items.Add("Not Concreted");
                    ritem.Items.Add("Concreted & Dirty");

                    e.RepositoryItem = ritem;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "Clean")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Clean & Impervious");
                    ritem.Items.Add("Not Concreted");
                    ritem.Items.Add("Concreted & Dirty");
                    e.RepositoryItem = ritem;
                }

            }


            ///Satisfactory
            ///

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "Satisfactory")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Satisfactory");
                    ritem.Items.Add("Non Satisfactory");
                    ritem.Items.Add("Not Tested");


                    e.RepositoryItem = ritem;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "Satisfactory")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Satisfactory");
                    ritem.Items.Add("Non Satisfactory");
                    ritem.Items.Add("Not Tested");

                    e.RepositoryItem = ritem;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "Satisfactory")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Satisfactory");
                    ritem.Items.Add("Non Satisfactory");
                    ritem.Items.Add("Not Tested");

                    e.RepositoryItem = ritem;
                }

            }


            ///Installed
            ///

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "Installed")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    e.RepositoryItem = repAvailable;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "Installed")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    e.RepositoryItem = repAvailable;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "Installed")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();

                    e.RepositoryItem = repAvailable;
                }

            }


            ///Available
            ///

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType1").ToString() == "Available")
            {
                if (e.Column.FieldName == "EscapeAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Clean & Impervious");
                    ritem.Items.Add("Not Concreted");
                    ritem.Items.Add("Concreted & Dirty");

                    e.RepositoryItem = ritem;
                }

            }


            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType2").ToString() == "Available")
            {
                if (e.Column.FieldName == "LayoutAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Clean & Impervious");
                    ritem.Items.Add("Not Concreted");
                    ritem.Items.Add("Concreted & Dirty");

                    e.RepositoryItem = ritem;
                }

            }

            if (gvEmergencyControl.GetRowCellValue(e.RowHandle, "ValueType3").ToString() == "Available")
            {
                if (e.Column.FieldName == "MilestoneAnswer")
                {
                    RepositoryItemComboBox ritem = new RepositoryItemComboBox();
                    ritem.Items.Add("Clean & Impervious");
                    ritem.Items.Add("Not Concreted");
                    ritem.Items.Add("Concreted & Dirty");
                    e.RepositoryItem = ritem;
                }

            }

        }

        private void txtProdMonth_EditValueChanged(object sender, EventArgs e)
        {
            if (FirstLoad == "N")
                loadData();
        }

        private void gvGenControl_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "WaterPressure")
            {
                if (e.CellValue != string.Empty)
                {
                    double num = Convert.ToDouble(e.CellValue);
                    if (num < 150 && num > 0)
                        e.Appearance.BackColor = Color.MistyRose;
                }

            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repDir);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);


            string[] files = System.IO.Directory.GetFiles(repDir);

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                int indexa = item.LastIndexOf("\\");

                string sourcefilename = item.Substring(indexa + 1, (item.Length - indexa) - 1);

                int indexprodmonth = sourcefilename.IndexOf(String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue));

                string SourcefileCheck = sourcefilename.Substring(0, indexprodmonth + 10);

                int NameLength = sourcefilename.Length - SourcefileCheck.Length;

                string Docsname = sourcefilename.Substring(SourcefileCheck.Length, NameLength);



                string Test = repDir + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(cbxChecklistId.EditValue.ToString()) + cbxWorkplace.EditValue.ToString();

                Test = Test + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue);

                Test = Test + ext;

                if (item.ToString() == repDir + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(cbxChecklistId.EditValue.ToString()) + cbxWorkplace.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue).ToString() + ext)
                {
                    DocsLB.Items.RemoveAt(DocsLB.SelectedIndex);
                    File.Delete(Test);
                }
            }

        }

        private void gvFireControl_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Distance")
            {
                if (e.CellValue != string.Empty)
                {
                    double num = Convert.ToDouble(e.CellValue);
                    if (num < 5.0 && num > 0)
                        e.Appearance.BackColor = Color.MistyRose;
                }

            }
        }

        private void btnAuthOther_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbAuth = new MWDataManager.clsDataAccess();
            _dbAuth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbAuth.SqlStatement = " declare @checkListID varchar(50), @WorkplaceID varchar(50) \r\n "
                        + " set @checkListID = (select CheckListID from tbl_Dept_Vent_OtherChecklist where CheckListName = '" + chkListName + "') \r\n "
                        + " set @WorkplaceID = (select WorkplaceID from tbl_Workplace where Description = '" + workPlace + "') \r\n "
                        + " delete from tbl_Dept_Inspection_VentAuthorised_OtherChecklists where WorkplaceID  = @WorkplaceID and \r\n "
                + " CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + "' and  ChecklistID = @checkListID  \r\n ";
            _dbAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbAuth.queryReturnType = MWDataManager.ReturnType.DataTable;

            if (btnAuthOther.Caption == "Authorise")
            {
                _dbAuth.SqlStatement += " insert into tbl_Dept_Inspection_VentAuthorised_OtherChecklists (WorkplaceID, CalendarDate, SectionID, ChecklistID) \r\n "
                                + " values(@WorkplaceID, '" + String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue) + "', '" + cbxSection.EditValue.ToString() + "', @checkListID)";

            }

            var uathResult = _dbAuth.ExecuteInstruction();

            if (uathResult.success)
            {
                CHeckAuth();
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace authorised", Color.CornflowerBlue);
            }
            else
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace not authorised", Color.CornflowerBlue);
            }
        }

        #endregion

        private void gvEnvCondControl_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {

            if (e.Column.Caption == "Answer")
            {
                //Number
                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Unit")
                {
                    e.RepositoryItem = Int;

                }

                //Numeric(10)
                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Numeric(10)")
                {
                    e.RepositoryItem = repRefNumber;
                }

                //Decimal(1)
                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Decimal(1)")
                {
                    e.RepositoryItem = repRefOneDec;
                }

                //YesNo
                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Yes/No")
                {
                    e.RepositoryItem = repYesNoNA;
                }


                //Other
                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Other")
                {
                    e.RepositoryItem = repRefAbandoned;
                }

                //Good/Bad
                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "Good/Bad")
                {
                    e.RepositoryItem = repGoodBad;
                }

                //DateTime
                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "DateTime")
                {
                    e.RepositoryItem = repDate;
                }

                //InUse/Available
                if (gvEnvCondControl.GetRowCellValue(e.RowHandle, "ValueType").ToString() == "InUse/Available")
                {
                    e.RepositoryItem = repInUseAvl;
                }
            }
        }

        private void gvEnvCondControl_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

            if (chkListName == "Refuge Chamber Report")
            {
                if (gvEnvCondControl.GetRowCellValue(gvEnvCondControl.FocusedRowHandle, gvEnvCondControl.Columns["ValueType"]).ToString() == string.Empty)
                {
                    e.Cancel = true;
                }
            }
        }

        string doCalc = "Y";
        private void gvEnvCondControl_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //if (gvEnvCondControl.GetRowCellValue(gvEnvCondControl.FocusedRowHandle, "QuestionID").ToString() == "4")
            //{
            if (gvEnvCondControl.GetRowCellValue(gvEnvCondControl.FocusedRowHandle, "Answer").ToString() == "No"
                || gvEnvCondControl.GetRowCellValue(gvEnvCondControl.FocusedRowHandle, "Answer").ToString() == "N/A"
                || gvEnvCondControl.GetRowCellValue(gvEnvCondControl.FocusedRowHandle, "Answer").ToString() == "Not Installed"
                || gvEnvCondControl.GetRowCellValue(gvEnvCondControl.FocusedRowHandle, "Answer").ToString() == "Bad")
            {
                frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                ActFrm._theSystemDBTag = _theSystemDBTag;
                ActFrm._UserCurrentInfo = _UserCurrentInfo;
                //ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue);
                //ActFrm.txtSection.EditValue = cbxSection.EditValue;
                //ActFrm.txtWorkplace.EditValue = cbxWorkplace.EditValue;
                //ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtProdMonth.EditValue.ToString()));
                //ActFrm.ReqDate.Value = Convert.ToDateTime(txtProdMonth.EditValue.ToString());
                //ActFrm.btnClose.Enabled = false;
                //ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //ActFrm.cbxWorkplace.EditValue = cbxWorkplace.EditValue;
                ActFrm.Item = gvEnvCondControl.GetRowCellValue(gvEnvCondControl.FocusedRowHandle, "QuestionDescription").ToString();
                ActFrm.AnswerLbl.Text = gvEnvCondControl.GetRowCellValue(gvEnvCondControl.FocusedRowHandle, "Answer").ToString();
                ActFrm.Type = "VSA";
                ActFrm.ShowDialog(this);
            }
            //}

            LoadActions();
        }

        private void btnEditAcReg_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");
                return;
            }

            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            //ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtProdMonth.EditValue.ToString())); //String.Format("{0:yyyy-MM-dd}", tbProdMonth.EditValue.ToString());
            //ActFrm.txtSection.EditValue = cbxSection.EditValue;

            //ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            //ActFrm._theSystemDBTag = this._theSystemDBTag;
            //ActFrm._UserCurrentInfo = this._UserCurrentInfo;

            //ActFrm.Item = "Inspection Actions";
            //ActFrm.Type = "VSA";
            //ActFrm.AllowExit = "Y";
            //ActFrm.FlagEdit = "Edit";

            //ActFrm.txtWorkplace.EditValue = cbxWorkplace.EditValue.ToString();
            //ActFrm.Item = Description;
            ActFrm.ActionTxt.Text = Recomendation;
            ActFrm.ReqDate.Text = TargetDate;
            //ActFrm.PriorityCmb.Text = Priority;
            ActFrm.RespPersonCmb.EditValue = RespPerson;
            ActFrm.OverseerCmb.EditValue = Overseer;

            ActFrm.ActID = ID;

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog();


            LoadActions();
        }

        private void gvAction_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            ID = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["ID"]).ToString();
            Workplace = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Workplace"]).ToString();
            Description = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Description"]).ToString();
            Recomendation = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Action"]).ToString();
            TargetDate = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["TargetDate"]).ToString();
            Priority = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["Priority"]).ToString();
            FileName = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["CompNotes"]).ToString();

            RespPerson = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["RespPerson"]).ToString();
            Overseer = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns["HOD"]).ToString();
        }

        private void btnRemActReg_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please click the row you want to delete first.");
                return;
            }


            MWDataManager.clsDataAccess _DeleteAction = new MWDataManager.clsDataAccess();
            _DeleteAction.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _DeleteAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _DeleteAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _DeleteAction.SqlStatement = " Delete from tbl_Shec_Incidents where ID = '" + ID + "'   \r\n" +
                                         " Delete from tbl_Shec_IncidentsVent where ID = '" + ID + "'    \r\n";

            _DeleteAction.ExecuteInstruction();

            string mianDicrectory = ActionsImgDir;

            string Image = mianDicrectory + "\\" + ID + ".png  ";

            if (File.Exists(Image))
            {
                File.Delete(Image);
            }

            LoadActions();
        }

        private void btnAddActReg_Click(object sender, EventArgs e)
        {
            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            //ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(txtProdMonth.EditValue.ToString()));
            //ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", txtProdMonth.EditValue);
            //ActFrm.txtSection.EditValue = cbxSection.EditValue.ToString();
            //ActFrm.cbxWorkplace.EditValue = cbxWorkplace.EditValue.ToString();
            //ActFrm.WPComboEdit.Items.Add(cbxWorkplace.EditValue.ToString());
            //if (cbxWorkplace.EditValue.ToString() != string.Empty)
            //{
            //    ActFrm.WPComboEdit.Items.Add(cbxWorkplace.EditValue.ToString());
            //}

            //ActFrm.txtWorkplace.EditValue = cbxWorkplace.EditValue.ToString();

            //ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            //ActFrm.txtWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            ActFrm._theSystemDBTag = _theSystemDBTag;
            ActFrm._UserCurrentInfo = _UserCurrentInfo;

            ActFrm.Item = "Vent Schedule Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }
    }
}
