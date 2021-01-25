using FastReport;
using Mineware.Systems.GlobalConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.RockEngineering
{
    public partial class RockEngFrmLedge : DevExpress.XtraEditors.XtraForm
    {
        private string ReportsFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        public string LTO = "";
        Report theReport3 = new Report();

        DialogResult result1;
        OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

        DialogResult resultDocpic1;
        OpenFileDialog openFileDialogDOcpic1 = new System.Windows.Forms.OpenFileDialog();

        DialogResult resultDocpic2;
        OpenFileDialog openFileDialogDOcpic2 = new System.Windows.Forms.OpenFileDialog();

        string DocAttachment1;
        string DocAttachment2;

        string sourceFile;
        string destinationFile;
        string FileName = string.Empty;

        public string imageDir;
        public string _UserCurrentInfo;

        int cat1Checked = 0;
        int cat2Checked = 0;
        int cat3Checked = 0;
        int cat4Checked = 0;
        int cat5Checked = 0;
        int cat6Checked = 0;
        int cat7Checked = 0;
        int cat8Checked = 0;
        int cat9Checked = 0;
        int cat10Checked = 0;
        int cat11Checked = 0;
        int cat12Checked = 0;
        int cat13Checked = 0;
        int cat14Checked = 0;
        int cat15Checked = 0;
        int cat16Checked = 0;
       

        public RockEngFrmLedge()
        {
            InitializeComponent();
        }

        string s = string.Empty;
        string wp = string.Empty;
        public string ActType = string.Empty;



        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]

            s = base64String.Trim().Replace(" ", "+");
            s = base64String.Trim().Replace("_", "/");

            if (s.Length % 4 > 0)
                s = s.PadRight(s.Length + 4 - s.Length % 4, '=');

            decimal aa = s.Length;

            byte[] imageBytes = Convert.FromBase64String(s);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);


            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);


            MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
            _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManImage.SqlStatement = "update [tbl_DPT_RockMechInspection] set picture = '" + s + "' where workplace = '" + WPLbl.EditValue + "' and captweek = '" + WkLbl2.EditValue + "' and captyear = datepart(year,getdate()) ";
            
            _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManImage.ResultsTableName = "Image";
            _dbManImage.ExecuteInstruction();
            
            return image;
        }
        
        void comboBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void RockEngFrm_Load(object sender, EventArgs e)
        {
            if (LTO == "Y")
            {
                RCRockEngineering.Visible = false;
                tabCapture.Visible = false;

                LoadReportLTO();
            }
            else
            {
                tabControl.Dock = DockStyle.Fill;
                tabControl.Visible = false;

                pbx2SVG.SvgImage = pbxWhiteSVG.SvgImage;             
                pbx3SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx4SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx5SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx6SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx7SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx8SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx9SVG.SvgImage = pbxWhiteSVG.SvgImage;
                pbx10SVG.SvgImage = pbxWhiteSVG.SvgImage;              
                

                Cat3.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat4.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat2.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat6.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat6.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat7.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat8.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat3.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat4.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat5.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat6.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat7.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat8.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat9.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                Cat10.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
                
                imageDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir;
               
                Cat2.SelectedIndex = 0;     
                Cat3.SelectedIndex = 0;
                Cat4.SelectedIndex = 0;
                Cat5.SelectedIndex = 0;
                Cat6.SelectedIndex = 0;
                Cat7.SelectedIndex = 0;
                Cat8.SelectedIndex = 0;
                Cat9.SelectedIndex = 0;
                Cat10.SelectedIndex = 0;           

                if (EditLbl.Text != "Y")
                {
                    tabControl.TabPages.Remove(tabCapture);
                    RCRockEngineering.Visible = false;
                }

                tabControl.Visible = true;

                LoadResponsible();
                LoadReport();
                tabControl.Visible = true;
            }
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
                                    "and workplaceid = (Select WorkplaceID from tbl_Workplace where description = '" + WPLbl.EditValue + "' )and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "') ";

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
                                    "and workplaceid = (Select WorkplaceID from tbl_Workplace where description = '" + WPLbl.EditValue + "' )and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "') ";

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

        void LoadReportLTO()
        {
            Cursor = Cursors.WaitCursor;

            MWDataManager.clsDataAccess _dbManWPST21 = new MWDataManager.clsDataAccess();
            _dbManWPST21.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManWPST21.SqlStatement = "select * from  tbl_LicenceToOperate_Seismic where wpdescription = '" + WPLbl.EditValue + "' order by thedate desc";

            _dbManWPST21.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPST21.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPST21.ResultsTableName = "Graph";
            _dbManWPST21.ExecuteInstruction();

            DataSet dsABS111 = new DataSet();
            dsABS111.Tables.Add(_dbManWPST21.ResultsDataTable);

            if (EditLbl.Text == "Y")
            {
                //if (_dbManWPST21.ResultsDataTable.Rows.Count > 0)
                //Cat24Txt.Text = _dbManWPST21.ResultsDataTable.Rows[0]["risk"].ToString();
            }

            theReport3.RegisterData(dsABS111);

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
                                         "       where workplace = '" + WPLbl.EditValue + "'   \r\n" +

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
            _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, " +
                                " RiskRating Risk, '" + RRlabel.EditValue + "' rr,  * from [tbl_DPT_RockMechInspection] " +
                                 "  where workplace = '" + WPLbl.EditValue + "'\r\n" +
                                "   and captweek = (select max(CaptWeek)\r\n" +
                                 "  from[tbl_DPT_RockMechInspection]\r\n" +
                                 "  where workplace = '" + WPLbl.EditValue + "') \r\n" +
                                 "  and captyear = (select max(CaptYear)\r\n" +
                                "   from[tbl_DPT_RockMechInspection]\r\n" +
                                 "  where workplace = '" + WPLbl.EditValue + "' and captweek = (select max(CaptWeek)\r\n" +
                                 "  from[tbl_DPT_RockMechInspection]\r\n" +
                                "   where workplace = '" + WPLbl.EditValue + "') )  \r\n";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "DevSummary";
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {               

                string BlankImage = Application.StartupPath + "\\" + "Neil.bmp";

                MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
               
                _dbManImage.SqlStatement = "Select RiskRating Risk,'" + BlankImage + "' pp, Picture,document1,document2  \r\n" +
                                    "from [tbl_DPT_RockMechInspection] \r\n" +
                                     "  where workplace = '" + WPLbl.EditValue + "'\r\n" +
                                    "   and captweek = (select max(CaptWeek)\r\n" +
                                     "  from[tbl_DPT_RockMechInspection]\r\n" +
                                     "  where workplace = '" + WPLbl.EditValue + "') \r\n" +
                                     "  and captyear = (select max(CaptYear)\r\n" +
                                    "   from[tbl_DPT_RockMechInspection]\r\n" +
                                     "  where workplace = '" + WPLbl.EditValue + "' and captweek = (select max(CaptWeek)\r\n" +
                                     "  from[tbl_DPT_RockMechInspection]\r\n" +
                                    "   where workplace = '" + WPLbl.EditValue + "') )  \r\n";
                _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManImage.ResultsTableName = "Image";
                _dbManImage.ExecuteInstruction();

                DataSet ReportDatasetReport = new DataSet();
                ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReport);

                DataSet ReportDatasetReportImage = new DataSet();
                ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReportImage);

                theReport3.Load(ReportsFolder + "\\RockEngLedge.frx");

                //theReport3.Design();

                pcReport.Clear();
                theReport3.Prepare();
                theReport3.Preview = pcReport;
                theReport3.ShowPrepared();
                tabControl.Visible = true;
                tabCapture.PageVisible = false;
                Cursor = Cursors.Default;

            }
        }

        void LoadReport()
        {
            Cursor = Cursors.WaitCursor;

            MWDataManager.clsDataAccess _dbManWPST21 = new MWDataManager.clsDataAccess();
            _dbManWPST21.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManWPST21.SqlStatement = "select * from  tbl_LicenceToOperate_Seismic where wpdescription = '" + WPLbl.EditValue + "' order by thedate desc";

            _dbManWPST21.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPST21.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPST21.ResultsTableName = "Graph";
            _dbManWPST21.ExecuteInstruction();

            DataSet dsABS111 = new DataSet();
            dsABS111.Tables.Add(_dbManWPST21.ResultsDataTable);

            if (EditLbl.Text == "Y")
            {
                //if (_dbManWPST21.ResultsDataTable.Rows.Count > 0)
                    //Cat24Txt.Text = _dbManWPST21.ResultsDataTable.Rows[0]["risk"].ToString();
            }

            theReport3.RegisterData(dsABS111);

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
                                         "       where workplace = '" + WPLbl.EditValue + "'   \r\n" +

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
            _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, RiskRating Risk, '" + RRlabel.EditValue + "' rr,  * from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' and CaptYear = datepart(YYYY,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "') and CaptWeek = '" + WkLbl2.EditValue + "'  ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "DevSummary";
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {
                cmbSB.EditValue = _dbMan.ResultsDataTable.Rows[0]["RespSB"].ToString();
                cmbMO.EditValue = _dbMan.ResultsDataTable.Rows[0]["RespMO"].ToString();

                if (_dbMan.ResultsDataTable.Rows[0]["Cat1Checked"].ToString().Substring(0, 1) == "A")
                {
                    Cat1A.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat1Checked"].ToString().Substring(0, 1) == "B")
                {
                    Cat1B.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat1Checked"].ToString().Substring(0, 1) == "S")
                {
                    Cat1S.Checked = true;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat1Checked"].ToString().Substring(1, 1) == "Y")
                {
                    Cat1P.Checked = true;
                }
                else
                {
                    Cat1P.Checked = false;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "0")
                {
                    Cat2.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "1")
                {
                    Cat2.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "2")
                {
                    Cat2.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "3")
                {
                    Cat2.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "4")
                {
                    Cat2.SelectedIndex = 4;
                }



                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "0")
                {
                    Cat3.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "1")
                {
                    Cat3.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "2")
                {
                    Cat3.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "3")
                {
                    Cat3.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "4")
                {
                    Cat3.SelectedIndex = 4;
                }




                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "0")
                {
                    Cat4.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "1")
                {
                    Cat4.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "2")
                {
                    Cat4.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "3")
                {
                    Cat4.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "4")
                {
                    Cat4.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "0")
                {
                    Cat5.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "1")
                {
                    Cat5.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "2")
                {
                    Cat5.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "3")
                {
                    Cat5.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "4")
                {
                    Cat5.SelectedIndex = 4;
                }

                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "0")
                {
                    Cat6.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "1")
                {
                    Cat6.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "2")
                {
                    Cat6.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "3")
                {
                    Cat6.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "4")
                {
                    Cat6.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "0")
                {
                    Cat7.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "1")
                {
                    Cat7.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "2")
                {
                    Cat7.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "3")
                {
                    Cat7.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "4")
                {
                    Cat7.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "0")
                {
                    Cat8.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "1")
                {
                    Cat8.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "2")
                {
                    Cat8.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "3")
                {
                    Cat8.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "4")
                {
                    Cat8.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "0")
                {
                    Cat9.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "1")
                {
                    Cat9.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "2")
                {
                    Cat9.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "3")
                {
                    Cat9.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "4")
                {
                    Cat9.SelectedIndex = 4;
                }


                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "0")
                {
                    Cat10.SelectedIndex = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "1")
                {
                    Cat10.SelectedIndex = 1;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "2")
                {
                    Cat10.SelectedIndex = 2;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "3")
                {
                    Cat10.SelectedIndex = 3;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "4")
                {
                    Cat10.SelectedIndex = 4;
                }


                


                
                
                Cat1Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat1Note"].ToString();
                Cat2Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat2Note"].ToString();
                Cat3Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat3Note"].ToString();
                Cat2Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat4Note"].ToString();
                Cat5Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat5Note"].ToString();
                Cat6Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat6Note"].ToString();
                Cat7Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat7Note"].ToString();
                Cat8Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat8Note"].ToString();
                Cat9Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat9Note"].ToString();
                Cat10Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat10Note"].ToString();
           
               

                commentsTxt.Text = _dbMan.ResultsDataTable.Rows[0]["GeneralComments"].ToString();

                DocAttachment1 = _dbMan.ResultsDataTable.Rows[0]["Document1"].ToString();
                DocAttachment2 = _dbMan.ResultsDataTable.Rows[0]["Document2"].ToString();

                txtAttachment.Text = _dbMan.ResultsDataTable.Rows[0]["picture"].ToString();

                if (_dbMan.ResultsDataTable.Rows[0]["WPASuser"].ToString() == "Tablet")
                {
                    if (_dbMan.ResultsDataTable.Rows[0]["picture"].ToString() != string.Empty)
                    {
                        PicBox.Image = Base64ToImage(_dbMan.ResultsDataTable.Rows[0]["picture"].ToString());
                    }

                    PicBox.Image.Save(Application.StartupPath + "\\" + "Neil.bmp");
                }
                else
                {
                    PicBox.ImageLocation = _dbMan.ResultsDataTable.Rows[0]["picture"].ToString();
                }

                DocPic1.ImageLocation = _dbMan.ResultsDataTable.Rows[0]["Document1"].ToString();
                DocPic2.ImageLocation = _dbMan.ResultsDataTable.Rows[0]["Document2"].ToString();

                //if (_dbMan.ResultsDataTable.Rows[0]["WPStatus"].ToString() == "Green")
                //{
                //    GreenCheck.Checked = true;
                //}

                //if (_dbMan.ResultsDataTable.Rows[0]["WPStatus"].ToString() == "Orange")
                //{
                //    OrangeCheck.Checked = true;
                //}

                //if (_dbMan.ResultsDataTable.Rows[0]["WPStatus"].ToString() == "Red")
                //{
                //    RedCheck.Checked = true;
                //}

                string BlankImage = Application.StartupPath + "\\" + "Neil.bmp";

                //MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                //_dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);

                //_dbManImage.SqlStatement = "Select RiskRating Risk,'" + BlankImage + "' pp, Picture,document1,document2 from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' and captweek = '" + WkLbl2.EditValue + "' and captyear = (select max(CaptYear) from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' and captweek = '" + WkLbl2.EditValue + "') ";

                //_dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbManImage.ResultsTableName = "Image";
                //_dbManImage.ExecuteInstruction();

                MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
                _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);

                _dbManImage.SqlStatement = "Select RiskRating Risk,'" + BlankImage + "' pp, Picture,document1,document2 from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' and  captyear = (select max(CaptYear) from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "' ) ";

                _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManImage.ResultsTableName = "Image";
                _dbManImage.ExecuteInstruction();

                DataSet ReportDatasetReport = new DataSet();
                ReportDatasetReport.Tables.Add(_dbMan.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReport);

                DataSet ReportDatasetReportImage = new DataSet();
                ReportDatasetReportImage.Tables.Add(_dbManImage.ResultsDataTable);

                theReport3.RegisterData(ReportDatasetReportImage);

                theReport3.Load(ReportsFolder + "\\RockEngLedge.frx");

                //theReport3.Design();

                pcReport.Clear();
                theReport3.Prepare();
                theReport3.Preview = pcReport;
                theReport3.ShowPrepared();
            }
            else
            {
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
                _dbMan.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, '" + RRlabel.EditValue + "' rr,  * from [tbl_DPT_RockMechInspection] where workplace = '" + WPLbl.EditValue + "'  order by captyear desc, convert(decimal(18,0),actweek)   desc ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "DevSummary";
                _dbMan.ExecuteInstruction();

                if (_dbMan.ResultsDataTable.Rows.Count > 0)
                {
                    if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "0")
                    {
                        Cat2.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "1")
                    {
                        Cat2.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "2")
                    {
                        Cat2.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat2Checked"].ToString() == "3")
                    {
                        Cat2.SelectedIndex = 3;
                    }



                    if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "0")
                    {
                        Cat3.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "1")
                    {
                        Cat3.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "2")
                    {
                        Cat3.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat3Checked"].ToString() == "3")
                    {
                        Cat3.SelectedIndex = 3;
                    }



                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "0")
                    {
                        Cat4.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "1")
                    {
                        Cat4.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "2")
                    {
                        Cat4.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat4Checked"].ToString() == "3")
                    {
                        Cat4.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "0")
                    {
                        Cat5.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "1")
                    {
                        Cat5.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "2")
                    {
                        Cat5.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat5Checked"].ToString() == "3")
                    {
                        Cat5.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "0")
                    {
                        Cat6.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "1")
                    {
                        Cat6.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "2")
                    {
                        Cat6.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat6Checked"].ToString() == "3")
                    {
                        Cat6.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "0")
                    {
                        Cat7.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "1")
                    {
                        Cat7.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "2")
                    {
                        Cat7.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat7Checked"].ToString() == "3")
                    {
                        Cat7.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "0")
                    {
                        Cat8.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "1")
                    {
                        Cat8.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "2")
                    {
                        Cat8.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat8Checked"].ToString() == "3")
                    {
                        Cat8.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "0")
                    {
                        Cat9.SelectedIndex = 0;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "1")
                    {
                        Cat9.SelectedIndex = 1;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "2")
                    {
                        Cat9.SelectedIndex = 2;
                    }

                    if (_dbMan.ResultsDataTable.Rows[0]["Cat9Checked"].ToString() == "3")
                    {
                        Cat9.SelectedIndex = 3;
                    }


                    if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "0")
                    {
                        Cat10.SelectedIndex = 0;
                    }
                    if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "1")
                    {
                        Cat10.SelectedIndex = 1;
                    }
                    if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "2")
                    {
                        Cat10.SelectedIndex = 2;
                    }
                    if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "3")
                    {
                        Cat10.SelectedIndex = 3;
                    }
                    if (_dbMan.ResultsDataTable.Rows[0]["Cat10Checked"].ToString() == "4")
                    {
                        Cat10.SelectedIndex = 4;
                    }



                 


                
                    
                    Cat1Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat1Note"].ToString();
                    Cat2Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat2Note"].ToString();
                    Cat3Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat3Note"].ToString();
                    Cat4Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat4Note"].ToString();
                    Cat5Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat5Note"].ToString();
                    Cat6Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat6Note"].ToString();
                    Cat7Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat7Note"].ToString();
                    Cat8Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat8Note"].ToString();
                    Cat9Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat9Note"].ToString();
                    Cat10Note.Text = _dbMan.ResultsDataTable.Rows[0]["Cat10Note"].ToString();
                  

                    commentsTxt.Text = _dbMan.ResultsDataTable.Rows[0]["GeneralComments"].ToString();
                }

            }

            Cursor = Cursors.Default;
        }

        void LoadPercentageCompleted()
        {
            decimal Total;
            decimal Act;
            
            Total = 9;
            Act = 0;

            //N/A Exclusions
            if (Cat3.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat4.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat2.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat5.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat6.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat7.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat8.SelectedIndex == 4)
            {
                Total = Total - 1;
            }
            

            if (Cat9.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

            if (Cat10.SelectedIndex == 4)
            {
                Total = Total - 1;
            }

           

            
            //Calc
            if (Cat3.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat4.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat2.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat5.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat6.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat7.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat8.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat9.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            if (Cat10.SelectedIndex < 1)
            {
                Act = Act + 1;
            }

            

            
            
            RRlabel.EditValue = Convert.ToString(Math.Round(Convert.ToDecimal((Act / Total) * 100), 2));
        }

        private bool Forceinstructions()
        {
            bool TestFailed = false;

            if (Cat3.SelectedIndex > 0 && Cat3.SelectedIndex < 4 && string.IsNullOrEmpty(Cat2Note.Text))
            {
                TestFailed = true;
                Cat2Note.Focus();
            }

            //if (Cat3.SelectedIndex > 0 && Cat3.SelectedIndex < 4 && string.IsNullOrEmpty(Cat3Note.Text))
            //{
            //    TestFailed = true;
            //    Cat3Note.Focus();
            //}

            if (Cat2.SelectedIndex > 0 && Cat2.SelectedIndex < 4 && string.IsNullOrEmpty(Cat2Note.Text))
            {
                TestFailed = true;
                Cat2Note.Focus();
            }

            //if (Cat5.SelectedIndex > 0 && Cat5.SelectedIndex < 4 && string.IsNullOrEmpty(Cat5Note.Text))
            //{
            //    TestFailed = true;
            //    Cat5Note.Focus();
            //}

            //if (Cat6.SelectedIndex > 0 && Cat6.SelectedIndex < 4 && string.IsNullOrEmpty(Cat6Note.Text))
            //{
            //    TestFailed = true;
            //    Cat6Note.Focus();
            //}

            //if (Cat7.SelectedIndex > 0 && Cat7.SelectedIndex < 4 && string.IsNullOrEmpty(Cat7Note.Text))
            //{
            //    TestFailed = true;
            //    Cat7Note.Focus();
            //}

            if (Cat3.SelectedIndex > 0 && Cat3.SelectedIndex < 4 && string.IsNullOrEmpty(Cat3Note.Text))
            {
                TestFailed = true;
                Cat3Note.Focus();
            }

            if (Cat4.SelectedIndex > 0 && Cat4.SelectedIndex < 4 && string.IsNullOrEmpty(Cat4Note.Text))
            {
                TestFailed = true;
                Cat4Note.Focus();
            }

            if (Cat5.SelectedIndex > 0 && Cat5.SelectedIndex < 4 && string.IsNullOrEmpty(Cat5Note.Text))
            {
                TestFailed = true;
                Cat5Note.Focus();
            }

            if (Cat6.SelectedIndex > 0 && Cat6.SelectedIndex < 4 && string.IsNullOrEmpty(Cat6Note.Text))
            {
                TestFailed = true;
                Cat6Note.Focus();
            }

            if (Cat7.SelectedIndex > 0 && Cat7.SelectedIndex < 4 && string.IsNullOrEmpty(Cat7Note.Text))
            {
                TestFailed = true;
                Cat7Note.Focus();
            }

            if (Cat8.SelectedIndex > 0 && Cat8.SelectedIndex < 4 && string.IsNullOrEmpty(Cat8Note.Text))
            {
                TestFailed = true;
                Cat8Note.Focus();
            }

            if (Cat9.SelectedIndex > 0 && Cat9.SelectedIndex < 4 && string.IsNullOrEmpty(Cat9Note.Text))
            {
                TestFailed = true;
                Cat9Note.Focus();
            }

            if (Cat10.SelectedIndex > 0 && Cat10.SelectedIndex < 4 && string.IsNullOrEmpty(Cat10Note.Text))
            {
                TestFailed = true;
                Cat10Note.Focus();
            }

          
            
            
            return TestFailed;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            bool testfailed = Forceinstructions();

            if (testfailed)
            {
                MessageBox.Show("Please fill in all Deviation Notes");
                return;
            }

            //Error Message for SB & MO
            try
            {
                if (cmbSB.EditValue.ToString() == "")
                {
                    MessageBox.Show("Please enter a valid Shift Boss?");
                    //cmbSB.ShowPopup();
                    return;
                }
            }
            catch {
                MessageBox.Show("Please enter a valid Shift Boss?");
                //cmbSB.ShowPopup();
                return;
            }
            try
            {
                if (cmbMO.EditValue.ToString() == "[EditValue is null]")
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

            string Cat1Check = string.Empty;

            if (Cat1A.Checked == true)
            {
                Cat1Check = "A";
            }
            if (Cat1B.Checked == true)
            {
                Cat1Check = "B";
            }
            if (Cat1S.Checked == true)
            {
                Cat1Check = "S";
            }
            if (Cat1A.Checked == false && Cat1B.Checked == false && Cat1S.Checked == false)
            {
                Cat1Check = "N";
            }
            if (Cat1P.Checked == true)
            {
                Cat1Check = Cat1Check + "Y";
            }
            if (Cat1P.Checked == false)
            {
                Cat1Check = Cat1Check + "N";
            }

            string SB = string.Empty;
            string MO = string.Empty;

            if (cmbSB.EditValue.ToString() != string.Empty)
            {
                SB = cmbSB.EditValue.ToString();
            }
            else
            {
                MessageBox.Show("No Shift Boss selected.");
                return;
            }

            if (cmbMO.EditValue.ToString() != string.Empty)
            {
                MO = cmbMO.EditValue.ToString();
            }
            else
            {
                MessageBox.Show("No Mine Overseer selected.");
                return;
            }
            
            


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbMan.SqlStatement = "Delete From [tbl_DPT_RockMechInspection] Where workplace = '" + WPLbl.EditValue + "' and CaptYear = datepart(YYYY,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "') and CaptWeek = '" + WkLbl2.EditValue + "' \r\n \r\n";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into [tbl_DPT_RockMechInspection] VALUES ( '" + WPLbl.EditValue + "' , datepart(YYYY,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "') , datepart(ISOWK,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "') , datepart(ISOWK,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "')  , getdate() , '" + TUserInfo.UserID + "' , \r\n " +

                                  " '" + cmbSB.EditValue.ToString() + "' , '" + cmbMO.EditValue.ToString() + "' , '" + RRlabel.EditValue + "', \r\n" +

                                  " '" + Cat1Check + "' , '" + Cat1Note.Text + "' , \r\n" +
                                  " '" + cat2Checked + "' , '" + Cat2Note.Text + "' , \r\n" +
                                  " '" + cat3Checked + "' , '" + Cat3Note.Text + "' , \r\n" +
                                  " '" + cat4Checked + "' , '" + Cat4Note.Text + "' , \r\n" +
                                  " '" + cat5Checked + "' , '" + Cat5Note.Text + "' , \r\n" +
                                  " '" + cat6Checked + "' , '" + Cat6Note.Text + "' , \r\n" +
                                  " '" + cat7Checked + "' , '" + Cat7Note.Text + "' , \r\n" +
                                  " '" + cat8Checked + "' , '" + Cat8Note.Text + "' , \r\n" +
                                  " '" + cat9Checked + "' , '" + Cat9Note.Text + "' , \r\n" +
                                  " '" + cat10Checked + "' , '" + Cat10Note.Text + "' , \r\n" +

                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +

                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +

                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +
                                  " '' , '' , \r\n" +

                                  "  '" + txtAttachment.Text + "' ,  '" + commentsTxt.Text + "' ,  '" + WPStatus + "', \r\n" +
                                  
                                  " '" + DocAttachment1 + "','" + DocAttachment2 + "','') ";

            _dbMan.SqlStatement = _dbMan.SqlStatement + "\r\n \r\n exec sp_Incidents_Transfer_Ledge  ";

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
            destinationFile = string.Empty;
            
            BusUnit = ProductionGlobal.ProductionGlobalTSysSettings._Banner;

            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(imageDir + @"\RockEngineering");

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
                    FileName = WPLbl.EditValue + "_Doc1";
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog1.SafeFileName.Substring(index);
                
                destinationFile = imageDir + @"\RockEngineering" + "\\" + FileName + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            
                            destinationFile = imageDir + @"\RockEngineering" + "\\" + FileName + Ext;
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
                    System.IO.File.Copy(sourceFile, imageDir + @"\RockEngineering" + "\\" + FileName + Ext, true);
                    dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering");
                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtAttachment.Text = destinationFile;
                PicBox.Image = Image.FromFile(txtAttachment.Text);
            }
        }

        void GetFileDoc()
        {
            destinationFile = string.Empty;

            BusUnit = ProductionGlobal.ProductionGlobalTSysSettings._Banner;

            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering\Documents");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(imageDir + @"\RockEngineering\Documents");

            if (resultDocpic1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialogDOcpic1.FileName;

                index = openFileDialogDOcpic1.SafeFileName.IndexOf(".");

                if (WPLbl.EditValue.ToString() != string.Empty)
                {
                    FileName = WPLbl.EditValue + "_Doc1";
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialogDOcpic1.SafeFileName.Substring(index);

                destinationFile = imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialogDOcpic1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext;//+ FileName + Name.ToString() + Ext
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
                    System.IO.File.Copy(sourceFile, imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering\Documents");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                    DocPic1.Image = Image.FromFile(openFileDialogDOcpic1.FileName);
                }

                DocAttachment1 = destinationFile;
                DocPic1.Image = Image.FromFile(DocAttachment1);
            }
        }

        void GetFileDoc2()
        {
            destinationFile = string.Empty;
            BusUnit = ProductionGlobal.ProductionGlobalTSysSettings._Banner;

            Random r = new Random();
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering\Documents");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(imageDir + @"\RockEngineering\Documents");

            if (resultDocpic2 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialogDOcpic2.FileName;

                index = openFileDialogDOcpic2.SafeFileName.IndexOf(".");

                if (WPLbl.EditValue.ToString() != string.Empty)
                {
                    FileName = WPLbl.EditValue + "_Doc2";
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialogDOcpic2.SafeFileName.Substring(index);

                destinationFile = imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialogDOcpic2.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext;
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
                    System.IO.File.Copy(sourceFile, imageDir + @"\RockEngineering\Documents" + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(imageDir + @"\RockEngineering\Documents");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                    DocPic2.Image = Image.FromFile(openFileDialogDOcpic2.FileName);
                }

                DocAttachment2 = destinationFile;
                DocPic2.Image = Image.FromFile(DocAttachment2);

            }
        }

        private void RockEnginSavebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Cat1A.Checked == false && Cat1B.Checked == false && Cat1S.Checked == false)
            {
                MessageBox.Show("Please do the ABS-P declaration ");
                return;
            }

            SaveBtn_Click(null, null);
        }

        private void RockEnginAddImagebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddTypeBtn_Click(null, null);
        }
        
        private void Cat1ACheck_CheckedChanged(object sender, EventArgs e)
        {
            Cat1B.Checked = false;
            Cat1S.Checked = false;
        }

        private void Cat1BCheck_CheckedChanged(object sender, EventArgs e)
        {
            Cat1A.Checked = false;
            Cat1S.Checked = false;
        }

        private void Cat1SCheck_CheckedChanged(object sender, EventArgs e)
        {
            Cat1A.Checked = false;
            Cat1B.Checked = false;
        }
        
        private void AdjustBookGB_Enter(object sender, EventArgs e)
        {

        }
        
        private void btnaddImage2_Click(object sender, EventArgs e)
        {
            openFileDialogDOcpic1.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialogDOcpic1.FileName = null;
            resultDocpic1 = openFileDialogDOcpic1.ShowDialog();

            GetFileDoc();
        }

        private void btnaddImage3_Click(object sender, EventArgs e)
        {
            openFileDialogDOcpic2.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialogDOcpic2.FileName = null;
            resultDocpic2 = openFileDialogDOcpic2.ShowDialog();

            GetFileDoc2();
        }

        private void date1_ValueChanged(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfo);
            _dbManWPSTDetail.SqlStatement = "Select DATEPART(ISOWK,'" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "')";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            WkLbl2.EditValue = _dbManWPSTDetail.ResultsDataTable.Rows[0][0].ToString();
            WkLbl.Text = WkLbl2.EditValue.ToString();
        }

        private void cmbx1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void label79_Click(object sender, EventArgs e)
        {

        }

        private void Cat1NotesTxt_TextChanged(object sender, EventArgs e)
        {

        }

        //Callie
        private void Cat2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat3.SelectedIndex == 0)
            {
                pbx2SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat2Checked = 0;
            }

            if (Cat3.SelectedIndex == 1)
            {

                pbx2SVG.SvgImage = pbxRedSVG.SvgImage;
                cat2Checked = 1;
            }

            if (Cat3.SelectedIndex == 2)
            {

                pbx2SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat2Checked = 2;
            }

            if (Cat3.SelectedIndex == 3)
            {

                pbx2SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat2Checked = 3;
            }
            if (Cat3.SelectedIndex == 4)
            {

                pbx2SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat2Checked = 4;
            }
            LoadPercentageCompleted();
        }

        

        private void Cat4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat2.SelectedIndex == 0)
            {
                pbx2SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat2Checked = 0;
            }

            if (Cat2.SelectedIndex == 1)
            {

                pbx2SVG.SvgImage = pbxRedSVG.SvgImage;
                cat2Checked = 1;
            }

            if (Cat2.SelectedIndex == 2)
            {

                pbx2SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat2Checked = 2;
            }

            if (Cat2.SelectedIndex == 3)
            {

                pbx2SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat2Checked = 3;
            }
            if (Cat2.SelectedIndex == 4)
            {

                pbx2SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat2Checked = 4;
            }

            LoadPercentageCompleted();
        }

       

        

        private void Cat8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat3.SelectedIndex == 0)
            {
                pbx3SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat3Checked = 0;
            }

            if (Cat3.SelectedIndex == 1)
            {

                pbx3SVG.SvgImage = pbxRedSVG.SvgImage;
                cat3Checked = 1;
            }

            if (Cat3.SelectedIndex == 2)
            {

                pbx3SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat3Checked = 2;
            }

            if (Cat3.SelectedIndex == 3)
            {

                pbx3SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat3Checked = 3;
            }
            if (Cat3.SelectedIndex == 4)
            {

                pbx3SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat3Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat4.SelectedIndex == 0)
            {
                pbx4SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat4Checked = 0;
            }

            if (Cat4.SelectedIndex == 1)
            {

                pbx4SVG.SvgImage = pbxRedSVG.SvgImage;
                cat4Checked = 1;
            }

            if (Cat4.SelectedIndex == 2)
            {

                pbx4SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat4Checked = 2;
            }

            if (Cat4.SelectedIndex == 3)
            {

                pbx4SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat4Checked = 3;
            }
            if (Cat4.SelectedIndex == 4)
            {

                pbx4SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat4Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat5.SelectedIndex == 0)
            {
                pbx5SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat5Checked = 0;
            }

            if (Cat5.SelectedIndex == 1)
            {

                pbx5SVG.SvgImage = pbxRedSVG.SvgImage;
                cat5Checked = 1;
            }

            if (Cat5.SelectedIndex == 2)
            {

                pbx5SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat5Checked = 2;
            }

            if (Cat5.SelectedIndex == 3)
            {

                pbx5SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat5Checked = 3;
            }
            if (Cat5.SelectedIndex == 4)
            {

                pbx5SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat5Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat6.SelectedIndex == 0)
            {
                pbx6SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat6Checked = 0;
            }

            if (Cat6.SelectedIndex == 1)
            {

                pbx6SVG.SvgImage = pbxRedSVG.SvgImage;
                cat6Checked = 1;
            }

            if (Cat6.SelectedIndex == 2)
            {

                pbx6SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat6Checked = 2;
            }

            if (Cat6.SelectedIndex == 3)
            {

                pbx6SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat6Checked = 3;
            }
            if (Cat6.SelectedIndex == 4)
            {

                pbx6SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat6Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat7.SelectedIndex == 0)
            {
                pbx7SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat7Checked = 0;
            }

            if (Cat7.SelectedIndex == 1)
            {

                pbx7SVG.SvgImage = pbxRedSVG.SvgImage;
                cat7Checked = 1;
            }

            if (Cat7.SelectedIndex == 2)
            {

                pbx7SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat7Checked = 2;
            }

            if (Cat7.SelectedIndex == 3)
            {

                pbx7SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat7Checked = 3;
            }
            if (Cat7.SelectedIndex == 4)
            {

                pbx2SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat2Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat8.SelectedIndex == 0)
            {
                pbx8SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat8Checked = 0;
            }

            if (Cat8.SelectedIndex == 1)
            {

                pbx8SVG.SvgImage = pbxRedSVG.SvgImage;
                cat8Checked = 1;
            }

            if (Cat8.SelectedIndex == 2)
            {

                pbx8SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat8Checked = 2;
            }

            if (Cat8.SelectedIndex == 3)
            {

                pbx8SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat8Checked = 3;
            }
            if (Cat8.SelectedIndex == 4)
            {

                pbx8SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat8Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat14_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat9.SelectedIndex == 0)
            {
                pbx9SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat9Checked = 0;
            }

            if (Cat9.SelectedIndex == 1)
            {

                pbx9SVG.SvgImage = pbxRedSVG.SvgImage;
                cat9Checked = 1;
            }

            if (Cat9.SelectedIndex == 2)
            {

                pbx9SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat9Checked = 2;
            }

            if (Cat9.SelectedIndex == 3)
            {

                pbx9SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat9Checked = 3;
            }
            if (Cat9.SelectedIndex == 4)
            {

                pbx9SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat9Checked = 4;
            }
            LoadPercentageCompleted();
        }

        private void Cat15_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cat10.SelectedIndex == 0)
            {
                pbx10SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat2Checked = 0;
            }

            if (Cat10.SelectedIndex == 1)
            {

                pbx10SVG.SvgImage = pbxRedSVG.SvgImage;
                cat10Checked = 1;
            }

            if (Cat10.SelectedIndex == 2)
            {

                pbx10SVG.SvgImage = pbxOrangeSVG.SvgImage;
                cat10Checked = 2;
            }

            if (Cat10.SelectedIndex == 3)
            {

                pbx10SVG.SvgImage = pbxYellowSVG.SvgImage;
                cat10Checked = 3;
            }
            if (Cat10.SelectedIndex == 4)
            {

                pbx10SVG.SvgImage = pbxWhiteSVG.SvgImage;
                cat10Checked = 4;
            }
            LoadPercentageCompleted();
        }

        

        

        

        private void btnImage_Click(object sender, EventArgs e)
        {
            AddTypeBtn_Click(null, null);
        }
        private DrawItemEventArgs lastDrawn;
        private void Cat2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat3_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat4_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat5_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat6_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }

        private void Cat7_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox combo = sender as ComboBox;
                if (e.Index == combo.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    if (lastDrawn != null)
                        lastDrawn.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                                     lastDrawn.Bounds
                                                    );
                    lastDrawn = e;
                }
                else
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds
                                            );

                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                      new SolidBrush(combo.ForeColor),
                                      new Point(e.Bounds.X, e.Bounds.Y)
                                     );
            }
        }
    }
}
