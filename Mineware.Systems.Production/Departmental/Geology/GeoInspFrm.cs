using DevExpress.XtraEditors;
using FastReport;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Geology
{
    public partial class GeoInspFrm : XtraForm
    {
        Report theReport3 = new Report();
        private string ReportsFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

        DialogResult result1;
        DialogResult result2;
        OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        OpenFileDialog openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
        string sourceFile;
        string destinationFile;
        string FileName = string.Empty;
        string FileName2 = string.Empty;

        #region Public variables
        public string _UserCurrentInfo;
        public string imageDir;
        #endregion

        public GeoInspFrm()
        {
            InitializeComponent();
        }

        void LoadResponsible()
        {
            //Search Edit
            MWDataManager.clsDataAccess _dbManSE = new MWDataManager.clsDataAccess();
            _dbManSE.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManSE.SqlStatement = "Select Username,UserID,PasSectionID from tbl_Users order by Username";

            _dbManSE.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSE.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSE.ResultsTableName = "Graph";
            _dbManSE.ExecuteInstruction();

            DataTable dt24 = _dbManSE.ResultsDataTable;
            DataSet ds24 = new DataSet();
            if (ds24.Tables.Count > 0)
                ds24.Tables.Clear();
            ds24.Tables.Add(dt24);

            lookupEditSB.DataSource = ds24.Tables[0];
            lookupEditSB.DisplayMember = "Username";
            lookupEditSB.ValueMember = "UserID";

            lookupEditMO.DataSource = ds24.Tables[0];
            lookupEditMO.DisplayMember = "Username";
            lookupEditMO.ValueMember = "UserID";


            //SB 
            MWDataManager.clsDataAccess _dbManSB = new MWDataManager.clsDataAccess();
            _dbManSB.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManSB.SqlStatement = "Select * from tbl_Users where passectionid in  \r\n" +
                "(Select  s.reporttosectionid sb from tbl_planning p, tbl_section s,tbl_section s2 \r\n" +
                "where  p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and s.reporttosectionid = s2.sectionid and s.prodmonth = s2.prodmonth \r\n" +
                "and workplaceid = (Select WorkplaceID from tbl_Workplace where description = '" + WPLbl.EditValue + "' )and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "') and substring(userid,0, 6) <> 'EMAIL' ";

            _dbManSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSB.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSB.ResultsTableName = "Graph";
            _dbManSB.ExecuteInstruction();

            DataTable dtSB = _dbManSB.ResultsDataTable;

            foreach (DataRow dr1 in dtSB.Rows)
            {
                cmbSB.EditValue = dr1["UserID"].ToString();
            }


            //MO
            MWDataManager.clsDataAccess _dbManMO = new MWDataManager.clsDataAccess();
            _dbManMO.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManMO.SqlStatement = "Select * from tbl_Users where passectionid in  \r\n" +
                "(Select  s2.reporttosectionid mo from tbl_planning p, tbl_section s,tbl_section s2 \r\n" +
                "where  p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and s.reporttosectionid = s2.sectionid and s.prodmonth = s2.prodmonth \r\n" +
         "and workplaceid = (Select WorkplaceID from tbl_Workplace where description = '" + WPLbl.EditValue + "' )and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "') and substring(userid,0, 6) <> 'EMAIL' ";

            _dbManMO.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMO.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMO.ResultsTableName = "Graph";
            _dbManMO.ExecuteInstruction();

            DataTable dsMO = _dbManMO.ResultsDataTable;

            foreach (DataRow dr2 in dsMO.Rows)
            {
                cmbMO.EditValue = dr2["UserID"].ToString();
            }
        }

        private void GeoInspFrm_Load(object sender, EventArgs e)
        {
            imageDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir;

            //this.Icon = PAS.Properties.Resources.testbutton3;           

            if (EditLbl.Text == "1")
            {
                tabControl1.TabPages.Remove(tabPage1);
            }
            else
            {
                if (WkLbl.Text != "7")
                {
                    if (WkLbl.Text != "6")
                    {
                        tabControl1.TabPages.Remove(tabPage1);
                    }
                }
            }

            LoadResponsible();
            LoadGrid();
            LoadChart();
            LoadReport();
        }

        void LoadReport()
        {

            string wk = WkLbl2.EditValue.ToString().Substring(3, 2);

            MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
            _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManWPST2.SqlStatement = "select top (20) * from (   \r\n" +
                                        "Select 'Z' bb,  \r\n" +
                                        "case when targetdate is null then 'Not Accepted'  \r\n" +
                                        "when targetdate is not null then 'Accepted'   \r\n" +
                                        " when CompletionDate is not null then 'Completed'  \r\n" +
                                        " when VerificationDate is not null then 'Verified' else '' end as ActionStatus   \r\n" +
                                        " ,[description] as Action, thedate datesubmitted, datediff(day, thedate, getdate()) ss   \r\n" +
                                        " from[dbo].[tbl_Shec_Incidents]  \r\n" +
                                        " where workplace = '" + WPLbl.EditValue + "'   \r\n" +


                                        " union all \r\n" +
                                        " select 'a' , '', '', null, '' \r\n" +
                                        " union all \r\n" +
                                        " select 'b ', '', '', null, '' \r\n" +
                                        " union all \r\n" +
                                        " select 'c  ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'd   ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'e    ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'f     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'g     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'h     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'i     ' , '', '', null, '' \r\n" +
                                        " union \r\n" +
                                        " select 'j     ' , '', '', null, '' \r\n" +
                                        " )a \r\n" +
                                        "  order  by bb  desc,datesubmitted \r\n";
            _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPST2.ResultsTableName = "Table2";
            _dbManWPST2.ExecuteInstruction();

            DataSet dsABS1 = new DataSet();
            dsABS1.Tables.Add(_dbManWPST2.ResultsDataTable);

            theReport3.RegisterData(dsABS1);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, '" + RRLbl.Text + "' rr,  * from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + WPLbl.EditValue + "' and captweek = '" + Convert.ToInt32(wk) + "' and captyear =  (select max(CaptYear) from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + WPLbl.EditValue + "' and captweek = '" + Convert.ToInt32(wk) + "') ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "DevSummary";
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {
                if (_dbMan.ResultsDataTable.Rows[0]["IW"].ToString() == "N")
                {
                    cbxIW.Checked = false;
                }
                else
                {
                    cbxIW.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Koppie"].ToString() == "N")
                {
                    cbxKoppie.Checked = false;
                }
                else
                {
                    cbxKoppie.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Pothole"].ToString() == "N")
                {
                    cbxPothole.Checked = false;
                }
                else
                {
                    cbxPothole.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Slump"].ToString() == "N")
                {
                    cbxSlump.Checked = false;
                }
                else
                {
                    cbxSlump.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["ReefRoll"].ToString() == "N")
                {
                    cbxReef.Checked = false;
                }
                else
                {
                    cbxReef.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Fault"].ToString() == "N")
                {
                    cbxFault.Checked = false;
                }
                else
                {
                    cbxFault.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Dykes"].ToString() == "N")
                {
                    cbxDykes.Checked = false;
                }
                else
                {
                    cbxDykes.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Other"].ToString() == "N")
                {
                    cbxOther.Checked = false;
                }
                else
                {
                    cbxOther.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["RIF"].ToString() == "N")
                {
                    cbxRIF.Checked = false;
                }
                else
                {
                    cbxRIF.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["RIH"].ToString() == "N")
                {
                    cbxRIH.Checked = false;
                }
                else
                {
                    cbxRIH.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["HWOB"].ToString() == "N")
                {
                    cbxHWOB.Checked = false;
                }
                else
                {
                    cbxHWOB.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Moderate"].ToString() == "N")
                {
                    cbxModerate.Checked = false;
                }
                else
                {
                    cbxModerate.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Complex"].ToString() == "N")
                {
                    cbxComplex.Checked = false;
                }
                else
                {
                    cbxComplex.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["LowAngle"].ToString() == "N")
                {
                    cbxLowAngle.Checked = false;
                }
                else
                {
                    cbxLowAngle.Checked = true;
                }

                txtPegToFace.EditValue = _dbMan.ResultsDataTable.Rows[0]["PegToFace"].ToString();

                cmbMO.EditValue = _dbMan.ResultsDataTable.Rows[0]["HodPerson"].ToString();
                cmbSB.EditValue = _dbMan.ResultsDataTable.Rows[0]["RespPerson"].ToString();

                txtIWNotes.Text = _dbMan.ResultsDataTable.Rows[0]["IWNote"].ToString();
                txtKoppieNotes.Text = _dbMan.ResultsDataTable.Rows[0]["KoppieNote"].ToString();
                txtPotholeNotes.Text = _dbMan.ResultsDataTable.Rows[0]["PotholeNote"].ToString();
                txtSlumpNotes.Text = _dbMan.ResultsDataTable.Rows[0]["SlumpNote"].ToString();
                txtReefNotes.Text = _dbMan.ResultsDataTable.Rows[0]["ReefRollNote"].ToString();
                txtFaultNotes.Text = _dbMan.ResultsDataTable.Rows[0]["FaultNote"].ToString();
                txtDykesNotes.Text = _dbMan.ResultsDataTable.Rows[0]["DykesNote"].ToString();
                txtOtherNotes.Text = _dbMan.ResultsDataTable.Rows[0]["OtherNote"].ToString();
                txtRIFNotes.Text = _dbMan.ResultsDataTable.Rows[0]["RIFNote"].ToString();
                txtRIHNotes.Text = _dbMan.ResultsDataTable.Rows[0]["RIHNote"].ToString();
                txtHWOBNotes.Text = _dbMan.ResultsDataTable.Rows[0]["HWOBNote"].ToString();
                txtModerateNotes.Text = _dbMan.ResultsDataTable.Rows[0]["ModerateNote"].ToString();
                txtComplexNotes.Text = _dbMan.ResultsDataTable.Rows[0]["ComplexNote"].ToString();
                txtLowAngleNotes.Text = _dbMan.ResultsDataTable.Rows[0]["LowAngleNote"].ToString();
                txtPegComment.Text = _dbMan.ResultsDataTable.Rows[0]["PegComment"].ToString();

                txtComments.Text = _dbMan.ResultsDataTable.Rows[0]["Comment"].ToString();



                txtAttachment.Text = _dbMan.ResultsDataTable.Rows[0]["picture"].ToString();

                PicBox.ImageLocation = _dbMan.ResultsDataTable.Rows[0]["picture"].ToString();

                txtAttachment2.Text = _dbMan.ResultsDataTable.Rows[0]["picture1"].ToString();

                Picbox2.ImageLocation = _dbMan.ResultsDataTable.Rows[0]["picture1"].ToString();

                if (_dbMan.ResultsDataTable.Rows[0]["WPStatus"].ToString() == "Green")
                {
                    GreenCheck.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["WPStatus"].ToString() == "Orange")
                {
                    OrangeCheck.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["WPStatus"].ToString() == "Red")
                {
                    RedCheck.Checked = true;
                }

                MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);

                _dbManImage.SqlStatement = "Select picture, Picture1 from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + WPLbl.EditValue + "' and captweek = '" + Convert.ToInt32(wk) + "' ";

                _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManImage.ResultsTableName = "Image";
                _dbManImage.ExecuteInstruction();

                MWDataManager.clsDataAccess _dbManChart = new MWDataManager.clsDataAccess();
                _dbManChart.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);

                _dbManChart.SqlStatement = " select top(10) * from (select description, a.CalendarDate aa ,substring(convert(varchar(11),a.CalendarDate),0,12) Calendardate,a.SWidth,a.CorrCut,a.Hangwall,a.Footwall, " +
                                           " case when a.allocatedWidth = 0 then null else a.allocatedWidth end as allocatedWidth ,a.Notes from [dbo].[tbl_SAMPLING_Imported_Notes] a  \r\n" +
                                  " left outer  join tbl_Workplace_Total w on convert(varchar(50),a.gmsiwpis) = w.gmsiwpid  \r\n" +
                                  " and calendardate > getdate()-2000 ) a where description = '" + WPLbl.EditValue + "'    \r\n" +
                                  "  order by aa desc  ";

                _dbManChart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManChart.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManChart.ResultsTableName = "Chart";
                _dbManChart.ExecuteInstruction();


                DataSet ReportDatasetReport = new DataSet();
                ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReport);

                DataSet ReportDatasetReportImage = new DataSet();
                ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReportImage);


                DataSet ReportDatasetChart = new DataSet();
                ReportDatasetChart.Tables.Add(_dbManChart.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetChart);

                theReport3.Load(ReportsFolder + "\\GeoInsp.frx");

                //theReport3.Design();

                pcReport.Clear();
                theReport3.Prepare();
                theReport3.Preview = pcReport;
                theReport3.ShowPrepared();
            }
            else
            {
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
                _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, '" + RRLbl.Text + "' rr,  * from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + WPLbl.EditValue + "' and captweek < '" + Convert.ToInt32(wk) + "'  order by captyear desc, actweek desc ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "DevSummary";
                _dbMan.ExecuteInstruction();

                if (_dbMan.ResultsDataTable.Rows.Count > 0)
                {

                }
            }

            if (_dbMan.ResultsDataTable.Rows.Count == 0 && WkLbl.Text != "7")
            {
                if (_dbMan.ResultsDataTable.Rows.Count == 0 && WkLbl.Text != "6")
                    MessageBox.Show("Unable to capture data for this week.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.Close();
            }
        }

        private void LoadGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = " select top(10) * from (select description, a.CalendarDate aa ,substring(convert(varchar(11),a.CalendarDate),0,12) Calendardate,a.SWidth,a.CorrCut,a.Hangwall,a.Footwall,  \r\n" +
                                  " case when a.allocatedWidth = 0 then null else a.allocatedWidth end as allocatedWidth ,a.Notes from [dbo].[tbl_SAMPLING_Imported_Notes] a   \r\n" +
                                  " left outer  join tbl_Workplace_Total w on convert(varchar(50),a.gmsiwpis) = w.gmsiwpid   \r\n" +
                                  " and calendardate > getdate()-5000 ) a where description = '" + WPLbl.EditValue.ToString() + "'  \r\n" +
                                  " order by aa desc ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            DataSet ds = new DataSet();

            ds.Tables.Add(dt);

            if (dt.Rows.Count > 0)
            {
                colDate.FieldName = "Calendardate";
                colFootWall.FieldName = "Footwall";
                colHangWall.FieldName = "Hangwall";
                colChannel.FieldName = "CorrCut";
                colStopeWidth.FieldName = "SWidth";
                colAllocSW.FieldName = "allocatedWidth";
                colNote.FieldName = "Notes";
            }

            WalkAboutGrid.DataSource = ds.Tables[0];
        }

        private void LoadChart()
        {

            chart1.Series[0].Points.Clear();

            int Rowcount = 0;

            for (int s = 0; s <= gridView1.RowCount - 1; s++)
            {

                string Day = String.Format("{0:yyyy-MM-dd}", gridView1.GetRowCellValue(Rowcount, gridView1.Columns[0]).ToString());

                chart1.Series[0].Points.AddXY(Day, gridView1.GetRowCellValue(Rowcount, gridView1.Columns[1]).ToString());
                chart1.Series[1].Points.AddXY(Day, gridView1.GetRowCellValue(Rowcount, gridView1.Columns[3]).ToString());
                chart1.Series[2].Points.AddXY(Day, gridView1.GetRowCellValue(Rowcount, gridView1.Columns[2]).ToString());
                chart1.Series[3].Points.AddXY(Day, gridView1.GetRowCellValue(Rowcount, gridView1.Columns[4]).ToString());
                if (gridView1.GetRowCellValue(Rowcount, gridView1.Columns[5]).ToString() != string.Empty)
                    chart1.Series[4].Points.AddXY(Day, gridView1.GetRowCellValue(Rowcount, gridView1.Columns[5]).ToString());
                else
                    chart1.Series[4].Points.AddXY(Day, DBNull.Value);
                Rowcount = Rowcount + 1;
            }


        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _TextBrush;

            // Get the item from the collection.
            TabPage _TabPage = tabControl1.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _TabBounds = tabControl1.GetTabRect(e.Index);


            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.
                _TextBrush = new SolidBrush(Color.Black);
                g.FillRectangle(Brushes.YellowGreen, e.Bounds);

            }
            else
            {
                _TextBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();

            }

            // Use our own font. Because we CAN.
            Font _TabFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _StringFlags = new StringFormat();
            _StringFlags.Alignment = StringAlignment.Center;
            _StringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_TabPage.Text, _TabFont, _TextBrush,
                         _TabBounds, new StringFormat(_StringFlags));

        }

        void CheckForcedFields()
        {

            if (cmbSB.EditValue.ToString() == string.Empty)
            {
                pbxSB.Visible = true;
            }
            else
            {
                pbxSB.Visible = false;
            }

            if (cmbMO.EditValue.ToString() == string.Empty)
            {
                pbxMO.Visible = true;
            }
            else
            {
                pbxMO.Visible = false;
            }

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            //Error Message for SB & MO
            try
            {
                if (cmbSB.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please enter a valid Shift Boss.");
                    //cmbSB.ShowPopup();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please enter a valid Shift Boss.");
                //cmbSB.ShowPopup();
                return;
            }

            try
            {
                if (cmbMO.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please enter a valid Mine Overseer?");
                    //cmbMO.ShowPopup();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please enter a valid Shift Boss?");
                //cmbSB.ShowPopup();
                return;
            }


            string WPStatus = string.Empty;
            if (GreenCheck.Checked == true)
            {
                WPStatus = "Green";
            }

            if (OrangeCheck.Checked == true)
            {
                WPStatus = "Orange";
            }

            if (RedCheck.Checked == true)
            {
                WPStatus = "Red";
            }

            //checkboxes
            string IW = "N";
            string Koppie = "N";
            string Pothole = "N";
            string Slump = "N";
            string ReefRoll = "N";
            string Fault = "N";
            string Dykes = "N";
            string Other = "N";
            string RIF = "N";
            string RIH = "N";
            string HWOB = "N";
            string Moderate = "N";
            string Complex = "N";
            string LowAngle = "N";

            if (cbxIW.Checked == true)
            {
                IW = "Y";
            }

            if (cbxKoppie.Checked == true)
            {
                Koppie = "Y";
            }

            if (cbxPothole.Checked == true)
            {
                Pothole = "Y";
            }

            if (cbxSlump.Checked == true)
            {
                Slump = "Y";
            }

            if (cbxReef.Checked == true)
            {
                ReefRoll = "Y";
            }

            if (cbxFault.Checked == true)
            {
                Fault = "Y";
            }

            if (cbxDykes.Checked == true)
            {
                Dykes = "Y";
            }

            if (cbxOther.Checked == true)
            {
                Other = "Y";
            }

            if (cbxRIF.Checked == true)
            {
                RIF = "Y";
            }

            if (cbxRIH.Checked == true)
            {
                RIH = "Y";
            }

            if (cbxHWOB.Checked == true)
            {
                HWOB = "Y";
            }

            if (cbxModerate.Checked == true)
            {
                Moderate = "Y";
            }

            if (cbxComplex.Checked == true)
            {
                Complex = "Y";
            }

            if (cbxLowAngle.Checked == true)
            {
                LowAngle = "Y";
            }

            string SB = string.Empty;
            string MO = string.Empty;

            


            string captweek = WkLbl2.EditValue + "      ";

            captweek = captweek.Substring(3, 2);

            MWDataManager.clsDataAccess _dbManDelete = new MWDataManager.clsDataAccess();
            _dbManDelete.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManDelete.SqlStatement = " delete from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + WPLbl.EditValue + "' and captweek = '" + captweek + "' and captyear = datepart(Year,GETDATE())";
            _dbManDelete.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDelete.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDelete.ExecuteInstruction();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = " insert into  [tbl_DPT_GeoScience_Inspection] VALUES ( '" + WPLbl.EditValue + "', datepart(Year,GETDATE()), '" + captweek + "', '" + Convert.ToInt32(WkLbl.Text) + "', \r\n getdate(), '" + TUserInfo.UserID + "', " +

                                  " '" + IW + "', '" + txtIWNotes.Text + "',  \r\n " +
                                  " '" + Koppie + "', '" + txtKoppieNotes.Text + "', \r\n" +
                                  " '" + Pothole + "', '" + txtPotholeNotes.Text + "', \r\n" +
                                  " '" + Slump + "', '" + txtSlumpNotes.Text + "', \r\n" +
                                  " '" + ReefRoll + "', '" + txtReefNotes.Text + "', \r\n" +
                                  " '" + Fault + "', '" + txtFaultNotes.Text + "', \r\n" +
                                  " '" + Dykes + "', '" + txtDykesNotes.Text + "', \r\n" +
                                  " '" + Other + "', '" + txtOtherNotes.Text + "', \r\n" +
                                  " '" + RIF + "', '" + txtRIFNotes.Text + "', \r\n" +
                                  " '" + RIH + "', '" + txtRIHNotes.Text + "', \r\n" +
                                  " '" + HWOB + "', '" + txtHWOBNotes.Text + "', \r\n" +
                                  " '" + Moderate + "', '" + txtModerateNotes.Text + "', \r\n" +
                                  " '" + Complex + "', '" + txtComplexNotes.Text + "', \r\n" +
                                  " '" + LowAngle + "', '" + txtLowAngleNotes.Text + "', \r\n" +
                                  " '" + txtPegToFace.EditValue + "', '" + txtPegComment.Text + "', \r\n" +

                                  " '" + txtComments.Text + "', \r\n" +

                                  " '" + txtAttachment.Text + "', '" + WPStatus + "' " +

                                  ",'" + txtAttachment2.Text + "', '" + SB + "', '" + MO + "' ) ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "\r\n \r\n exec sp_Incidents_Transfer_Geology  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            LoadReport();
        }

        private void AddTypeBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialog1.FileName = null;
            result1 = openFileDialog1.ShowDialog();

            GetFile();
        }

        string BusUnit = string.Empty;

        void GetFile()
        {
            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(imageDir + @"\GeologyInspection");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(imageDir + @"\GeologyInspection");

            if (result1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialog1.FileName;

                index = openFileDialog1.SafeFileName.IndexOf(".");

                if (WPLbl.EditValue.ToString() != string.Empty)
                {
                    //FileName = String.Format("{0:yyyyMMddhhmmss}", date1.Value);
                    FileName = WPLbl.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog1.SafeFileName.Substring(index);

                destinationFile = imageDir + @"\GeologyInspection" + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            destinationFile = imageDir + @"\GeologyInspection" + "\\" + FileName + Ext;
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
                    System.IO.File.Copy(sourceFile, imageDir + @"\GeologyInspection" + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(imageDir + @"\GeologyInspection");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtAttachment.Text = destinationFile;
                PicBox.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void AddImagebtn_Click(object sender, EventArgs e)
        {
            openFileDialog2.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialog2.FileName = null;
            result2 = openFileDialog2.ShowDialog();

            GetFile2();
        }

        string BusUnit2 = " ";

        void GetFile2()
        {
            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(imageDir + @"\GeologyInspection");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(imageDir + @"\GeologyInspection");

            if (result2 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName2 = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialog2.FileName;

                index = openFileDialog2.SafeFileName.IndexOf(".");

                if (WPLbl.EditValue.ToString() != string.Empty)
                {
                    FileName2 = String.Format("{0:yyyyMMddhhmmss}", date1.Value);
                    FileName2 = FileName2 + "_2";
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog2.SafeFileName.Substring(index);

                destinationFile = imageDir + @"\GeologyInspection" + "\\" + FileName2 + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog2.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            destinationFile = imageDir + @"\GeologyInspection" + "\\" + FileName2 + Ext;
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
                    System.IO.File.Copy(sourceFile, imageDir + @"\GeologyInspection" + "\\" + FileName2 + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(imageDir + @"\GeologyInspection");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtAttachment2.Text = destinationFile;
                Picbox2.Image = Image.FromFile(openFileDialog2.FileName);
            }
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GreenCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (GreenCheck.Checked == true)
            {
                RedCheck.Checked = false;
                OrangeCheck.Checked = false;
            }
        }

        private void OrangeCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (OrangeCheck.Checked == true)
            {
                RedCheck.Checked = false;
                GreenCheck.Checked = false;
            }
        }

        private void RedCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (RedCheck.Checked == true)
            {
                OrangeCheck.Checked = false;
                GreenCheck.Checked = false;
            }
        }

        private void GreenCheck_Click(object sender, EventArgs e)
        {
            RedCheck.Checked = false;
            OrangeCheck.Checked = false;


        }

        private void OrangeCheck_Click(object sender, EventArgs e)
        {
            GreenCheck.Checked = false;
            RedCheck.Checked = false;


        }

        private void RedCheck_Click(object sender, EventArgs e)
        {
            GreenCheck.Checked = false;
            OrangeCheck.Checked = false;


        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveBtn_Click(null, null);
        }

        private void btnAddImage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddTypeBtn_Click(null, null);
        }

       

        //check to see if yes or no, not checked gets red dot
        private void cbxIW_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxIW.Checked == true)
            {
                pbx1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxWaste_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxKoppie.Checked == true)
            {
                pbx2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxPothole_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxPothole.Checked == true)
            {
                pbx3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxSlump_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxSlump.Checked == true)
            {
                pbx4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxReef_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxReef.Checked == true)
            {
                pbx5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxFault_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxFault.Checked == true)
            {
                pbx6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxDykes_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxDykes.Checked == true)
            {
                pbx7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxOther_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxOther.Checked == true)
            {
                pbx8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxRIF_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxRIF.Checked == true)
            {
                pbx9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxRIH_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxRIH.Checked == true)
            {
                pbx10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxHWOB_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxHWOB.Checked == true)
            {
                pbx11.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx11.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxModerate_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxModerate.Checked == true)
            {
                pbx12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxComplex_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxComplex.Checked == true)
            {
                pbx13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void cbxLowAngle_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxLowAngle.Checked == true)
            {
                pbx14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                pbx14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // GeoInspFrm
        //    // 
        //    this.ClientSize = new System.Drawing.Size(946, 521);
        //    this.Name = "GeoInspFrm";
        //    this.ResumeLayout(false);

        //}
    }
}
