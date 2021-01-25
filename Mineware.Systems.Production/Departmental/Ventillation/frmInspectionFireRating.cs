using DevExpress.XtraEditors;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Ventillation
{
    public partial class frmInspectionFireRating : DevExpress.XtraEditors.XtraForm
    {
        public frmInspectionFireRating()
        {
            InitializeComponent();
        }

        public string _theSystemDBTag;
        public string _UserCurrentInfo;

        public string _workPlace;
        public string _section;
        public string _crew;
        public string _prodMonth;

        //Private data fields
        //private String FileName = "";
        private String sourceFile;
        private String destinationFile;
        private String FileName = string.Empty;

        //Strings
        private String formloaded = "N";
        private String ID = string.Empty;
        private String Workplace = string.Empty;
        private String Description = string.Empty;
        private String Recomendation = string.Empty;
        private String Priority = string.Empty;
        private String TargetDate = string.Empty;
        private String RespPerson = string.Empty;
        private String Overseer = string.Empty;

        DialogResult result1;

        string repDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\FireInspections\Documents";    //Path to store files
        string repImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\FireInspections";  //Path to store Images
        string ActionsImgDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir + @"\VentilationInspections\FireInspections\ActionsImages";
        DialogResult fileResults;

        string docalcparam = "Y";
        string FirstLoad = "Y";

        private void frmInspectionFireRating_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;

            //wp = tbSection.EditValue.ToString().Substring(0, 4);
            tbProdMonth.EditValue = _prodMonth;
            tbDpInspecDate.EditValue = String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);
            tbCrew.EditValue = _crew;
            tbSection.EditValue = _section;

            FirstLoad = "N";
            LoadData();

            LoadFireActions();

        }

        void LoadData()
        {
            MWDataManager.clsDataAccess _CheckAuth = new MWDataManager.clsDataAccess();
            _CheckAuth.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _CheckAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _CheckAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _CheckAuth.SqlStatement = "Select * from tbl_Dept_Inspection_VentAuthorise \r\n" +
                                      "where activity = '0' and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + "' and SectionID = '" + tbSection.EditValue.ToString() + "' ";
            _CheckAuth.ExecuteInstruction();

            if (_CheckAuth.ResultsDataTable.Rows.Count > 0)
            {
                for (int col = 0; col < gvFireRating.Columns.Count; col++)
                {
                    gvFireRating.Columns[col].OptionsColumn.AllowEdit = false;
                }

                for (int col = 0; col < gvAction.Columns.Count; col++)
                {
                    gvAction.Columns[col].OptionsColumn.AllowEdit = false;
                }

                SaveBtn.Enabled = false;
                AddImBtn.Enabled = false;
                btnAddStopDoc.Enabled = false;

                btnAddMinAct.Enabled = false;
                btnEditMinAct.Enabled = false;
                btnDeleteMinAct.Enabled = false;
            }


            

            string tblQuery = "exec sp_Dept_Insection_Vent_Stoping_FireRating '" + _section + "', '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + "'  \r\n "
                            + " \r\n ";



            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = tblQuery;
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt1 = _dbMan.ResultsDataTable;
            DataSet ds1 = new DataSet();

            gcFireRating.DataSource = null;
            ds1.Tables.Add(dt1);
            gcFireRating.DataSource = ds1.Tables[0];

            colFireItem.FieldName = "Question";
            colFireYN.FieldName = "YesNo";
            colFireHaz.FieldName = "Hazard";
            colFireScore.FieldName = "Score";
            colFireWeight.FieldName = "Weight";
            colFireRem.FieldName = "Remarks";
            colFireResPrsn.FieldName = "ReponsiblePerson";
            colFireComDate.FieldName = "CompletionDate";
            colFireQuestID.FieldName = "QuestID";
            colFireSubCat.FieldName = "QuestionSubCat";

            CalcTotalPercent();

            LoadFireActions();

            loadImage();
            loadDocs();


        }

        //Gets Actions
        void LoadFireActions()
        {
            MWDataManager.clsDataAccess _dbManAct = new MWDataManager.clsDataAccess();
            _dbManAct.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _dbManAct.SqlStatement = "EXEC [sp_Dept_Ventilation_LoadFireRatingActions] '" + tbSection.EditValue + "', "
                + " '" + _workPlace + "', '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + "' ";
            _dbManAct.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManAct.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManAct.ExecuteInstruction();

            DataTable dtActions = _dbManAct.ResultsDataTable;

            gcActions.DataSource = null;
            gcActions.DataSource = dtActions;
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
            gcSection.FieldName = "WPType";

        }

        void CalcTotalPercent()
        {
            lblTotalVal.Text = string.Empty;
            lblTotWeight.Text = string.Empty;
            lblPercentage.Text = string.Empty;

            double[] ScoreArr = new double[50];
            double[] WeightArr = new double[50];
            double ScoreSum = 0, WeightSum = 0;
            for (int i = 0; i < gvFireRating.RowCount; i++)
            {
                string score = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["Score"]).ToString();
                string weight = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["Weight"]).ToString();

                ScoreArr[i] += Convert.ToDouble(score);
                WeightArr[i] += Convert.ToDouble(weight);
                ScoreSum += ScoreArr[i];
                WeightSum += WeightArr[i];
            }

            lblTotalVal.Text = ScoreSum.ToString("0.##");
            lblTotWeight.Text = WeightSum.ToString("0.##");
            lblPercentage.Text = ((ScoreSum / WeightSum) * 100).ToString("0.##") + " %";
        }

        private void SaveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);

            _dbMan.SqlStatement = " delete from tbl_Dept_Inspection_VentCapture_FireRating \r\n " +
                        " where section = '" + _section + "' and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + "' \r\n ";


            for (int i = 0; i < gvFireRating.RowCount; i++)
            {
                string questID = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["QuestID"]).ToString();
                string yesNo = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["YesNo"]).ToString();
                string hazard = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["Hazard"]).ToString();
                string score = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["Score"]).ToString();
                string weight = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["Weight"]).ToString();
                string rem = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["Remarks"]).ToString();
                string resPerson = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["ReponsiblePerson"]).ToString();
                string comDate = gvFireRating.GetRowCellValue(i, gvFireRating.Columns["CompletionDate"]).ToString();

                _dbMan.SqlStatement += " insert into tbl_Dept_Inspection_VentCapture_FireRating(QuestID, Section, " +
                                " Workplace, ProdMonth, CalendarDate, YesNo, " +
                            " Hazard, Score, MarksWeight, Remarks, ReponsiblePerson, CompletionDate) \r\n"
                            + " values('" + questID + "', '" + _section + "', '" + _workPlace + "', '" + tbDpInspecDate.EditValue + "', \r\n "
                            + " '" + tbDpInspecDate.EditValue + "', '" + yesNo + "', '" + hazard + "', '" + score + "', \r\n "
                            + " '" + weight + "', '" + rem + "','" + resPerson + "', '" + comDate + "') ";


            }

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;


            var results = _dbMan.ExecuteInstruction();

            if (results.success)
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Save", "Fire Rating Saved", Color.CornflowerBlue);
            }
            else
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Save", "Record did not save", Color.Red);
            }

        }

        private void tbDpInspecDate_EditValueChanged(object sender, EventArgs e)
        {
            if (FirstLoad == "N")
                LoadData();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gvFireRating_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Hazard")
                {

                    if (!string.IsNullOrEmpty(e.CellValue.ToString()))
                    {
                        if (e.CellValue.ToString() == "A")
                        {
                            e.Appearance.BackColor = Color.MistyRose;
                            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        }

                        if (e.CellValue.ToString() == "B")
                        {
                            e.Appearance.BackColor = Color.LightYellow;
                            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        }
                    }
                }

                if (e.Column.FieldName == "Score" || e.Column.FieldName == "Weight")
                {
                    e.Appearance.BackColor = Color.LightGray;
                }
            }
            catch (Exception)
            {
            }
        }

        private void AddImBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ofdOpenImageFile.InitialDirectory = folderBrowserDialog1.SelectedPath;
            ofdOpenImageFile.FileName = null;
            ofdOpenImageFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            result1 = ofdOpenImageFile.ShowDialog();

            GetFile();
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

                if (!tbSection.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + tbSection.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                if (tbProdMonth.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
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
                    System.IO.File.Copy(sourceFile, repImgDir + "\\" + FileName + Ext, true);
                    dir2 = new System.IO.DirectoryInfo(repImgDir);
                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtAttachment.EditValue = destinationFile;
                PicBox.Image = System.Drawing.Image.FromFile(ofdOpenImageFile.FileName);

            }
        }

        private void btnAddStopDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fileResults = openFileDialog1.ShowDialog();
            GetDoc();
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

                //if (!tbCrew.EditValue.Equals(""))
                //{
                //	FileName = ProductionGlobal.ProductionGlobal.ExtractAfterColon(tbCrew.EditValue.ToString());
                //}
                //else
                //{
                //	XtraMessageBox.Show("Please select a workplace");
                //	return;
                //}

                if (!tbSection.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + tbSection.EditValue.ToString();
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
                    return;
                }

                if (!tbProdMonth.EditValue.Equals(string.Empty))
                {
                    FileName = FileName + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue); ;
                }
                else
                {
                    XtraMessageBox.Show("Please select a workplace");
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
                        DocsLB.Items.Add((SourcefileName + Ext));
                    }
                    catch
                    {

                    }

                }
                else
                {
                    System.IO.File.Copy(sourceFile, mianDicrectory + "\\" + FileName + SourcefileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(mianDicrectory);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

            }
        }

        public void loadImage()
        {
            txtAttachment.Text = string.Empty;
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(repImgDir);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(repImgDir);

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                if (item.ToString() == repImgDir + "\\" + tbSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + ext)
                {
                    txtAttachment.Text = item.ToString();
                }
            }


            if (txtAttachment.Text != string.Empty)
            {
                using (FileStream stream = new FileStream(txtAttachment.Text, FileMode.Open, FileAccess.Read))
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

                string ext = aa.Substring(extpos, aa.Length - extpos);

                int indexa = item.LastIndexOf("\\");

                string sourcefilename = item.Substring(indexa + 1, (item.Length - indexa) - 1);

                int indexprodmonth = sourcefilename.IndexOf(String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue));

                string SourcefileCheck = sourcefilename.Substring(0, indexprodmonth + 10);

                int NameLength = sourcefilename.Length - SourcefileCheck.Length;

                string Docsname = sourcefilename.Substring(SourcefileCheck.Length, NameLength);

                if (SourcefileCheck == tbSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue))
                {
                    DocsLB.Items.Add(Docsname.ToString());
                }
            }

        }

        private void DocsLB_DoubleClick(object sender, EventArgs e)
        {
            string mianDicrectory = repDir;
            if (DocsLB.SelectedIndex != -1)
            {
                string test = mianDicrectory + "\\" + tbSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + DocsLB.SelectedItem.ToString();

                System.Diagnostics.Process.Start(mianDicrectory + "\\" + tbSection.EditValue.ToString() + String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue) + DocsLB.SelectedItem.ToString());
            }
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVentChecklistReport report = new frmVentChecklistReport();
            report.theSystemDBTag = _theSystemDBTag;
            report.UserCurrentInfo = _UserCurrentInfo;
            report.monthDate = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);
            report.section = tbSection.EditValue.ToString();
            report.prodMonth = tbProdMonth.EditValue.ToString();
            report.workPlace = _workPlace;
            report.crew = tbCrew.EditValue.ToString();
            report.Frmtype = "FireRating";
            report.StartPosition = FormStartPosition.CenterScreen;
            report.PicPath = txtAttachment.Text;
            report._totScore = lblTotalVal.Text;
            report._totWeight = lblTotWeight.Text;
            report._percentage = lblPercentage.Text;

            report.Show();

        }

        private void gvFireRating_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = false;

            //if (gvFireRating.GetRowCellValue(gvFireRating.FocusedRowHandle, gvFireRating.Columns["Hazard"]).ToString() == "Hazard"
            //	|| gvFireRating.GetRowCellValue(gvFireRating.FocusedRowHandle, gvFireRating.Columns["Score"]).ToString() == "Score"
            //	|| gvFireRating.GetRowCellValue(gvFireRating.FocusedRowHandle, gvFireRating.Columns["MarksWeight"]).ToString() == "MarksWeight")
            //{
            //	e.Cancel = true;
            //}
        }

        private void gvFireRating_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            CalcTotalPercent();

            string qID = gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["QuestID"]).ToString();

            string score = gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["Score"]).ToString();


            if (e.Column.FieldName.ToString() == "YesNo")
            {

                string yn = gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["YesNo"]).ToString();
                if (qID == "2" || qID == "23" || qID == "44" || qID == "45")
                {
                    if (yn == "N")
                    {
                        string wieght = gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["Weight"]).ToString();
                        gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Score"], wieght);

                        if (Convert.ToInt32(wieght) > 0)
                        {
                            gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Hazard"], string.Empty);
                        }
                    }

                    if (yn == "Y")
                    {
                        string wieght = gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["Weight"]).ToString();
                        gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Score"], 0);

                        if (wieght == "20")
                        {
                            gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Hazard"], "A");
                        }

                        if (wieght == "15")
                        {
                            gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Hazard"], "A");
                        }

                        if (wieght == "10")
                        {
                            gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Hazard"], "B");
                        }
                    }
                }

                if (qID == "3" || qID == "4" || qID == "5" || qID == "6" || qID == "7" || qID == "8" || qID == "9" || qID == "10" || qID == "11"
                    || qID == "12" || qID == "13" || qID == "14" || qID == "15" || qID == "16" || qID == "17" || qID == "18" || qID == "19"
                    || qID == "20" || qID == "21" || qID == "22" || qID == "24" || qID == "25" || qID == "26" || qID == "27" || qID == "28" || qID == "29"
                    || qID == "30" || qID == "31" || qID == "32" || qID == "33" || qID == "34" || qID == "35" || qID == "36" || qID == "37"
                    || qID == "38" || qID == "39" || qID == "40" || qID == "41" || qID == "42" || qID == "43" || qID == "46" || qID == "47"
                    || qID == "48" || qID == "49" || qID == "50")
                {
                    if (yn == "Y")
                    {
                        string wieght = gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["Weight"]).ToString();
                        gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Score"], wieght);

                        if (Convert.ToInt32(wieght) > 0)
                        {
                            gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Hazard"], string.Empty);
                        }
                    }

                    if (yn == "N")
                    {
                        string wieght = gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["Weight"]).ToString();
                        gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Score"], 0);

                        if (wieght == "20")
                        {
                            gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Hazard"], "A");
                        }

                        if (wieght == "15")
                        {
                            gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Hazard"], "A");
                        }

                        if (wieght == "10")
                        {
                            gvFireRating.SetRowCellValue(e.RowHandle, gvFireRating.Columns["Hazard"], "B");
                        }
                    }
                }

                if (gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["Hazard"]).ToString() != string.Empty)
                {
                    frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
                    //ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbProdMonth.EditValue.ToString()));
                    //ActFrm.txtSection.EditValue = tbSection.EditValue;
                    //ActFrm.WPComboEdit.Items.Add(_workPlace);

                    //ActFrm.txtWorkplace.EditValue = _workPlace;

                    ActFrm.Item = gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["Question"]).ToString();

                    //ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    //ActFrm.txtWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    //ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);


                    ActFrm._theSystemDBTag = _theSystemDBTag;
                    ActFrm._UserCurrentInfo = _UserCurrentInfo;

                    //ActFrm.Item = "Vent Schedule Actions";
                    ActFrm.Type = "VSAF";
                    ActFrm.AllowExit = "Y";


                    ActFrm.StartPosition = FormStartPosition.CenterScreen;
                    ActFrm.ShowDialog(this);

                    LoadFireActions();
                }
            }
        }

        private void repCheckYesNo_EditValueChanged(object sender, EventArgs e)
        {
            gvFireRating.PostEditor();
        }

        private void gvFireRating_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (gvFireRating.GetRowCellValue(e.RowHandle, gvFireRating.Columns["QuestID"]).ToString() != "51" && e.Column.FieldName == "YesNo")
            {
                e.RepositoryItem = repCheckYesNo;
            }
        }

        private void btnAddMinAct_Click(object sender, EventArgs e)
        {
            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            //ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbProdMonth.EditValue.ToString()));
            //ActFrm.txtSection.EditValue = tbSection.EditValue;
            //ActFrm.WPComboEdit.Items.Add(_workPlace);

            //ActFrm.txtWorkplace.EditValue = _workPlace;

            //ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            //ActFrm.txtWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;


            //ActFrm.Datelabel.Text = String.Format("{0:yyyy-MM-dd}", tbDpInspecDate.EditValue);


            ActFrm._theSystemDBTag = _theSystemDBTag;
            ActFrm._UserCurrentInfo = _UserCurrentInfo;

            ActFrm.Item = "Vent Schedule Actions";
            ActFrm.Type = "VSAF";
            ActFrm.AllowExit = "Y";


            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadFireActions();

        }


        private void btnEditMinAct_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");
                return;
            }

            frmVentilationActionCapture ActFrm = new frmVentilationActionCapture();
            //ActFrm.txtProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(tbProdMonth.EditValue.ToString())); //String.Format("{0:yyyy-MM-dd}", tbProdMonth.EditValue.ToString());
            //ActFrm.txtSection.EditValue = tbSection.EditValue;

            //ActFrm.cbxWorkplace.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            ActFrm._theSystemDBTag = this._theSystemDBTag;
            ActFrm._UserCurrentInfo = this._UserCurrentInfo;

            ActFrm.Item = "Inspection Actions";
            ActFrm.Type = "VSA";
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";

            //ActFrm.txtWorkplace.EditValue = Workplace;
            ActFrm.Item = Description;
            ActFrm.ActionTxt.Text = Recomendation;
            ActFrm.ReqDate.Text = TargetDate;
            //ActFrm.PriorityCmb.Text = Priority;
            ActFrm.RespPersonCmb.EditValue = RespPerson;
            ActFrm.OverseerCmb.EditValue = Overseer;

            ActFrm.ActID = ID;

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadFireActions();

        }

        private void gvAction_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
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

        private void btnDeleteMinAct_Click(object sender, EventArgs e)
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

            LoadFireActions();
        }

        private void gvFireRating_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            LoadData();
        }

        private void gcFireRating_TextChanged(object sender, EventArgs e)
        {
            // do incident
        }
    }
}