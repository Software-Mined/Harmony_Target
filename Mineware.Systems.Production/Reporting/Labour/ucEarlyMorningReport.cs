using DevExpress.Utils.Drawing;
using DevExpress.XtraNavBar;
using DevExpress.XtraNavBar.ViewInfo;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting.Labour
{
    public partial class ucEarlyMorningReport : BaseUserControl
    {
        string Loaded = "N";
        Report theReport = new Report();
        Report theReportDetail = new Report();
        Report theReportDetail2 = new Report();
        Procedures procs = new Procedures();
        string ChangeParam1;
        string ChangeParam2;
        string ChangeParam3;
        private String Param1;

        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";


        public ucEarlyMorningReport()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpMODaily);
            FormActiveRibbonPage = rpMODaily;
            FormMainRibbonPage = rpMODaily;
            RibbonControl = rcMODaily;
        }

        private void loadSecondReport(string Parameter1)
        {
            if (!String.IsNullOrEmpty(Parameter1) && Parameter1 != "")
            {
                // get Sasfety Rep
                MWDataManager.clsDataAccess _dbManSrep = new MWDataManager.clsDataAccess();
                _dbManSrep.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManSrep.SqlStatement = "SELECT '' SrepName, ''Altname, ''srep, ''AltSrep";
                _dbManSrep.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSrep.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSrep.ResultsTableName = "EarlyMornShift";
                _dbManSrep.ExecuteInstruction();

                DataTable dtInfo = _dbManSrep.ResultsDataTable;

                string userPath1 = Application.StartupPath;

                string builtPath = userPath1 + @"";

                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " " +
                           " SELECT '' upath,  \r\n" +
                           "'' upathnoimage,  \r\n" +
                           " '' sanum,  \r\n" +
                           " '' saname,  \r\n" +
                           " '' anum,  \r\n" +
                           " '' aname,  \r\n" +
                           " 1 zzz,  \r\n" +
                           " * \r\n" +
                           " FROM \r\n" +
                           " ( \r\n" +
                           " SELECT e.*, \r\n" +
                           " e.WorkGroupCode OccDescription, \r\n" +
                           " '' PatGrade \r\n" +
                           " FROM dbo.[tbl_EmployeeAll] e \r\n" +
                           "  \r\n" +
                           " WHERE GangNo = '" + Parameter1 + "' \r\n" +
                           " ) MainA \r\n" +
                           " LEFT OUTER JOIN \r\n" +
                           " ( \r\n" +
                           " SELECT empno, \r\n" +
                           " MAX(total)tt, \r\n" +
                           " MAX(ug) ugnum, \r\n" +
                           " MAX(UGTime) ug, \r\n" +
                           " SUBSTRING(CONVERT(VARCHAR, MAX(time1), 108), 0, 6) otherTime, \r\n" +
                           " MAX(stn) stn \r\n" +
                           " FROM \r\n" +
                           " ( \r\n" +
                           " SELECT *, \r\n" +
                           " CASE \r\n" +
                           " WHEN Clk_Time = MainSubAGroup.time1 \r\n" +
                           " THEN Clk_AreaDes \r\n" +
                           " ELSE '' \r\n" +
                           " END AS stn \r\n" +
                           " FROM \r\n" +
                           " ( \r\n" +
                           " SELECT CASE \r\n" +
                           " WHEN(((clk.Clk_AreaDes LIKE '%ndergroun%') \r\n" +
                           " OR(clk.Clk_AreaDes LIKE '%UG%') OR(clk.Clk_AreaDes LIKE '%U/G%')) \r\n" +
                           " ) \r\n" +
                           " THEN 1 \r\n" +
                           " ELSE 0 \r\n" +
                           " END AS UG, \r\n" +
                           " 1 Total, \r\n" +
                           " CASE \r\n" +
                           " WHEN(((clk.Clk_AreaDes LIKE '%ndergroun%') \r\n" +
                           " OR(clk.Clk_AreaDes LIKE '%UG%') OR(clk.Clk_AreaDes LIKE '%U/G%')) \r\n" +
                           " ) \r\n" +
                           " THEN SUBSTRING(CONVERT(VARCHAR, clk.Clk_Time, 108), 0, 6) \r\n" +
                           " ELSE '0' \r\n" +
                           " END AS UGTime,  \r\n" +
                           " EmployeeNo empno, \r\n" +
                           " GangNo gang, \r\n " +
                           " a.*,  \r\n" +
                           " clk.* \r\n" +
                           " FROM tbl_EmployeeAll AS a \r\n" +
                           " LEFT OUTER JOIN tbl_EmployeeAll_EmployeeClockings AS clk ON a.EmployeeNo = clk.Clk_EmpNumber COLLATE Latin1_General_CI_AS \r\n" +
                           " WHERE GangNo = '" + Parameter1 + "' \r\n" +
                           " AND CONVERT(VARCHAR(10), clk.Clk_date, 111) = CONVERT(VARCHAR(10), GETDATE(), 111) \r\n" +
                           " ) MainSubA \r\n" +
                           " LEFT OUTER JOIN \r\n" +
                           " ( \r\n" +
                           " SELECT Clk_EmpNumber empno1, \r\n" +
                           " MAX(Clk_Time) time1 \r\n" +
                           " FROM tbl_EmployeeAll_EmployeeClockings \r\n" +
                           " WHERE CONVERT(VARCHAR(10), Clk_date, 111) = CONVERT(VARCHAR(10), GETDATE(), 111) \r\n" +
                           " GROUP BY Clk_EmpNumber \r\n" +
                           " ) MainSubAGroup ON MainSubAGroup.empno1 = MainSubA.empno COLLATE Latin1_General_CI_AS \r\n" +
                           " ) MainSubB \r\n" +
                           " GROUP BY empno \r\n" +
                           " ) MainB ON MainA.EmployeeNo = MainB.empno COLLATE Latin1_General_CI_AS   order by PATGRade; ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "EarlyMornShift";
                _dbMan.ExecuteInstruction();

                if (_dbMan.ResultsDataTable.Rows.Count != 0)
                {
                    DataSet dsDetail = new DataSet();
                    dsDetail.Tables.Add(_dbMan.ResultsDataTable);
                    theReportDetail.RegisterData(dsDetail);
                    theReportDetail.Load(_reportFolder + "\\LabEmpDetail.frx");
                    // theReportDetail.Design();                   
                    theReportDetail.Show();
                    Application.Idle += new System.EventHandler(this.Application_Idle);

                }
            }
        }

        private void ucEarlyMorningReport_Load(object sender, EventArgs e)
        {
            // get sections
            MWDataManager.clsDataAccess _dbManGetISAfterStart = new MWDataManager.clsDataAccess();
            _dbManGetISAfterStart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetISAfterStart.SqlStatement = "select distinct(lbl) lbl from vw_MorningReportData \r\n" +
                                                "where MainLbl in ('2) Stoping','3) Development') \r\n" +
                                                "order by lbl ";
            _dbManGetISAfterStart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetISAfterStart.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetISAfterStart.ExecuteInstruction();

            DataTable dtSections = _dbManGetISAfterStart.ResultsDataTable;

            DevExpress.XtraNavBar.NavBarItem itema = navBarControl1.Items.Add();
            itema.Caption = "";
            navBarControl1.ActiveGroup.ItemLinks.Add(itema);

            if (SysSettings.Banner == "Tshepong Operations")
            {
                listBox2.Items.Add("Tshepong - Top Panels Early Morning Report (Kgs)");
                listBox2.Items.Add("Phakisa - Top Panels Early Morning Report (Kgs)");
            }
            else
                listBox2.Items.Add("Top Panels Early Morning Report (Kgs)");

            foreach (DataRow dr in dtSections.Rows)
            {
                listBox2.Items.Add(dr["lbl"].ToString());
            }

            itema.Visible = false;
            listBox2.SelectedIndex = -1;

            // get Engineers
            MWDataManager.clsDataAccess _dbManGetISAfterStart1 = new MWDataManager.clsDataAccess();
            _dbManGetISAfterStart1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetISAfterStart1.SqlStatement = "select distinct(lbl) lbl from [dbo].[vw_MorningReportData] where lbl like '%Eng%'  \r\n " +
                                            "order by lbl  ";
            _dbManGetISAfterStart1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetISAfterStart1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetISAfterStart1.ExecuteInstruction();

            DataTable dtSections1 = _dbManGetISAfterStart1.ResultsDataTable;

            navBarControl1.ActiveGroup = navBarGroupEng;

            DevExpress.XtraNavBar.NavBarItem itema1 = navBarControl1.Items.Add();
            itema1.Caption = "";
            navBarControl1.ActiveGroup.ItemLinks.Add(itema1);

            foreach (DataRow dr in dtSections1.Rows)
            {
                listBox3.Items.Add(dr["lbl"].ToString());
            }

            itema1.Visible = false;
            listBox3.SelectedIndex = -1;

            // getother
            MWDataManager.clsDataAccess _dbManGetOther = new MWDataManager.clsDataAccess();
            _dbManGetOther.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetOther.SqlStatement = "select * from ( select distinct(Lbl) Lbl from( \r\n" +
                                          "select MainLbl, lbl   from [dbo].[vw_MorningReportData] where lbl not like '%Eng%') a) tmp where \r\n" +
                                          " lbl not in ( select distinct(lbl) lbl from vw_MorningReportData  \r\n " +
                                          " where MainLbl in ('2) Stoping','3) Development')) order by lbl  ";
            _dbManGetOther.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetOther.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetOther.ExecuteInstruction();

            DataTable dtSections3 = _dbManGetOther.ResultsDataTable;

            foreach (DataRow dr in dtSections3.Rows)
            {
                listBox4.Items.Add(dr["lbl"].ToString());
            }

            listBox4.SelectedIndex = -1;
            Loaded = "Y";
            navBarGroupMining.GroupClientHeight = this.Height / 3 - 100;
            navBarGroupEng.GroupClientHeight = this.Height / 3 - 100; 
            navBarGroupServices.GroupClientHeight = this.Height / 3 - 100;
        }

        private void loadMining()
        {
            DataTable Neil = new DataTable();
            string sec1 = "";

            if (SysSettings.Banner == "Tshepong Operations")
            {
                if ((ChangeParam1 == "Tshepong - Top Panels Early Morning Report (Kgs)") || (ChangeParam1 == "Phakisa - Top Panels Early Morning Report (Kgs)"))
                    sec1 = ChangeParam1;
                else
                    sec1 = procs.ExtractBeforeColon(ChangeParam1);
            }
            else
            {
                if (ChangeParam1 == "Top Panels Early Morning Report (Kgs)")
                    sec1 = ChangeParam1;
                else
                    sec1 = procs.ExtractBeforeColon(ChangeParam1);
            }

            string sec2 = "";

            if (SysSettings.Banner == "Tshepong Operations")
            {
                if ((ChangeParam1 == "Tshepong - Top Panels Early Morning Report (Kgs)") || (ChangeParam1 == "Phakisa - Top Panels Early Morning Report (Kgs)"))
                {
                    sec2 = ChangeParam1;
                }
                else
                {
                    sec2 = procs.ExtractBeforeColon(ChangeParam1);
                    sec2 = sec2 + "         ";
                    sec2 = sec2.Substring(1, 3);
                }
            }
            else
            {
                if (ChangeParam1 == "Top Panels Early Morning Report (Kgs)")
                {
                    sec2 = ChangeParam1;
                }
                else
                {
                    sec2 = procs.ExtractBeforeColon(ChangeParam1);
                    sec2 = sec2 + "         ";
                    sec2 = sec2.Substring(1, 3);
                }
            }

            string done = "N";

            if (sec1 == "Top Panels Early Morning Report (cmgt)")
            {
                done = "Y";

                MWDataManager.clsDataAccess _dbManabc = new MWDataManager.clsDataAccess();
                _dbManabc.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManabc.SqlStatement = " select  sum( UGYFinal-(total-ug)) ugoutFinal from  [dbo].[tbl_GangClock]  \r\n" +
                                         " where gangno in ( select OrgUnitDS COLLATE Latin1_General_CI_AS from Top20Panels where title = 'cmgt' )  ";
                _dbManabc.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManabc.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManabc.ResultsTableName = "Lossesabc";  //get table name
                _dbManabc.ExecuteInstruction();

                DataSet ReportDatasetabc = new DataSet();
                ReportDatasetabc.Tables.Add(_dbManabc.ResultsDataTable);
                theReport.RegisterData(ReportDatasetabc);

                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " select '" + ChangeParam1 + "' lbl, *, ISNULL  ( total , 0 ) aa1   \r\n" +
                                      " , ISNULL  ( ug , 0 ) ug1, ISNULL  ( total , 0 ) total1, ISNULL  ( SqmTotal , 0 ) SqmTotal1, ISNULL  ( adv , 0.0 ) adv1, actnum1-compl var1,   ActNum1- ISNULL  ( total , 0 ) var2,   ISNULL  ( total , 0 ) -  ISNULL  ( ug , 0 )  other \r\n" +
                                      " , case when ISNULL  (  ActNum1 , 0 ) > 0 then (ISNULL  ( SqmTotal , 0 ) + ISNULL  ( adv , 0.0 ))/ISNULL  (  ActNum1 , 0 ) else 0 end as sqmm from (select  case when substring(gangno,10,3) in( '110') And substring(gangno,13,1) in( '1')  \r\n" +
                                      " then '3) Stoping'  when substring(gangno,10,3) in( '100') then '2) Development'  \r\n" +
                                      " when substring(gangno,10,3) in( '001') then '1) Management'  when substring(gangno,10,3) in( '110') \r\n" +
                                      " And substring(gangno,13,1) in( '2')  then '4) Cleaning'  when substring(gangno,10,3) in( '102', '121') \r\n" +
                                      " then '5) Construction'  when substring(gangno,10,3) in( '112') then '6) Equiping'  \r\n" +
                                      " when substring(gangno,10,3) in( '120') then '7) Tramming'  else '8) Other' END as calsaa,  \r\n" +
                                      " aa,  gangno a \r\n" +
                                      " , GangName xx,  convert(numeric(7,0),actcomp) compl, convert(numeric(7,0),ActNum) ActNum1 \r\n" +
                                      " from ( \r\n" +
                                      " select OrgUnit gangno, substring(OrgUnit,6,1)+'0'+substring(OrgUnit,7,1) aa, * from dbo.vw_orgunits  \r\n" +
                                      " where orgunit in (select distinct(OrgUnitDS) from Top20Panels  where title = 'cmgt') \r\n" +
                                      " ) a \r\n" +
                                      ") a left outer join \r\n" +
                                      " (select  GangNo gang, convert(numeric(7,0), UG) UG, convert(numeric(7,0),Total) Total, convert(numeric(7,0), UGOut) UGOut,  convert(numeric(7,0), UGYFinal-(total-ug)) ugoutFinal from  [dbo].[tbl_GangClock]  \r\n" +
                                      " where GangNo in (select distinct(OrgUnitDS) from Top20Panels  where title = 'cmgt' \r\n" +
                                      " ) ) b  on a.a = b.gang \r\n" +
                                      " left outer join \r\n " +
                                      " (select org, sum(SqmTotal) SqmTotal, sum(adv) adv from \r\n" +
                                      " ( select  case when OrgUnitDS <> '' then  \r\n" +
                                      " OrgUnitDS when OrgUnitDS = ''  \r\n" +
                                      " and Activity <> 1 then 'No Stp Gang Assigned'   \r\n" +
                                      " when OrgUnitDS = ''  \r\n" +
                                      " and Activity = 1 then 'No Dev Gang Assigned'  \r\n" +
                                      " end as org, SqmTotal, \r\n " +
                                      " case when activity = 1 then Adv else 0  end as adv  \r\n" +
                                      " from tbl_PlanMonth p , tbl_Section s , tbl_Section s1  \r\n" +
                                      " where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and  \r\n" +
                                      " s.ReportToSectionid = s1.sectionid and s.prodmonth = s1.prodmonth and   \r\n" +
                                      " OrgUnitDS IN (select distinct(OrgUnitDS) COLLATE Latin1_General_CI_AS from Top20Panels where title = 'cmgt' ) and p.Prodmonth =  \r\n" +
                                      "(select currentproductionmonth from tbl_sysset)) c \r\n" +
                                      " group by org) c  on a.a = c.org  order by calsaa , a  ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "Losses";  //get table name
                _dbMan.ExecuteInstruction();

                DataSet ReportDataset = new DataSet();
                ReportDataset.Tables.Add(_dbMan.ResultsDataTable);
                theReport.RegisterData(ReportDataset);
            }

            if (done == "N")
            {
                MWDataManager.clsDataAccess _dbManabc = new MWDataManager.clsDataAccess();
                _dbManabc.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManabc.SqlStatement = " select sum(UGOutFinal) ugoutFinal,  '" + UserCurrentInfo.Connection + "' Banner  from [dbo].[vw_MorningReportData] where mo = '" + sec1 + "' \r\n ";
                _dbManabc.SqlStatement = _dbManabc.SqlStatement + " or Top20  = '" + sec1 + "' ";
                _dbManabc.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManabc.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManabc.ResultsTableName = "Lossesabc";  //get table name
                _dbManabc.ExecuteInstruction();

                DataSet ReportDatasetabc = new DataSet();
                ReportDatasetabc.Tables.Add(_dbManabc.ResultsDataTable);
                theReport.RegisterData(ReportDatasetabc);

                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = "Select '" + sec1 + "' lbl, * from vw_MorningReportData where mo = '" + sec1 + "' \r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " or Top20  = '" + sec1 + "' \r\n";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "Losses";  //get table name
                _dbMan.ExecuteInstruction();

                DataSet ReportDataset = new DataSet();
                ReportDataset.Tables.Add(_dbMan.ResultsDataTable);
                theReport.RegisterData(ReportDataset);

            }

            theReport.Load(_reportFolder + "\\EarlyMorningShiftRprt.frx");
            //theReport.Design();
            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
            Application.Idle += new System.EventHandler(this.Application_Idle);
        }



        private void navBarControl1_CustomDrawLink(object sender, DevExpress.XtraNavBar.ViewInfo.CustomDrawNavBarElementEventArgs e)
        {
            if (e.ObjectInfo.State == DevExpress.Utils.Drawing.ObjectState.Pressed)
            {
                // if a link is not hot tracked or pressed it is drawn in the normal way
                if (e.ObjectInfo.State == ObjectState.Hot || e.ObjectInfo.State == ObjectState.Pressed)
                {
                    Rectangle rect = e.RealBounds;
                    rect.Inflate(-1, -1);
                    LinearGradientBrush brush;
                    Rectangle imageRect;
                    Rectangle textRect;
                    StringFormat textFormat = new StringFormat();
                    textFormat.LineAlignment = StringAlignment.Center;

                    // identifying the painted link
                    NavLinkInfoArgs linkInfo = e.ObjectInfo as NavLinkInfoArgs;
                    if (linkInfo.Link.Group.GroupCaptionUseImage == NavBarImage.Large)
                    {
                        // adjusting the rectangles for the image and text and specifying the text's alignment
                        // if a large image is displayed within a link
                        imageRect = rect;
                        imageRect.Inflate(-(rect.Width - 32) / 2, -2);
                        textRect = rect;
                        int textHeight = Convert.ToInt16(e.Graphics.MeasureString(e.Caption,
                          e.Appearance.Font).Height);
                        textFormat.Alignment = StringAlignment.Center;
                    }
                    else
                    {
                        // adjusting the rectangles for the image and text and specifying the text's alignment
                        // if a small image is displayed within a link
                        imageRect = rect;
                        imageRect.Width = 16;
                        imageRect.Offset(2, 2);
                        textRect = new Rectangle(rect.Left + 23, rect.Top, rect.Width - 23, rect.Height);
                        textFormat.Alignment = StringAlignment.Near;
                    }

                    // creating different brushes for the hot tracked and pressed states of a link
                    if (e.ObjectInfo.State == ObjectState.Hot)
                    {
                        brush = new LinearGradientBrush(rect, Color.Orange, Color.PeachPuff,
                          LinearGradientMode.Horizontal);
                        // shifting image and text up when a link is hot tracked
                        imageRect.Offset(0, -1);
                        textRect.Offset(0, -1);
                    }
                    else
                        brush = new LinearGradientBrush(rect, Color.YellowGreen, Color.YellowGreen,
                          LinearGradientMode.Horizontal);

                    // painting borders
                    e.Graphics.FillRectangle(new SolidBrush(Color.PeachPuff), e.RealBounds);
                    // painting background
                    e.Graphics.FillRectangle(brush, rect);
                    // painting image
                    if (e.Image != null)
                        e.Graphics.DrawImageUnscaled(e.Image, imageRect);
                    // painting caption
                    e.Graphics.DrawString(e.Caption, e.Appearance.Font, new SolidBrush(Color.Black),
                      textRect, textFormat);
                    // prohibiting default link painting
                    e.Handled = true;
                }

            }
        }

        private void navBarControl1_SelectedLinkChanged(object sender, NavBarSelectedLinkChangedEventArgs e)
        {
            navBarControl1.SelectedLink.Item.AppearancePressed.ForeColor = Color.Red;
            ChangeParam1 = navBarControl1.SelectedLink.Caption;
        }


        private void ChangeParam1Chnage()
        {
            if (Loaded == "Y")
            {
                this.Cursor = Cursors.WaitCursor;
                loadMining();
                this.Cursor = Cursors.Default;
            }
        }


        private void pcReport_Click(object sender, EventArgs e)
        {
            Param1 = "";
            if (theReport.GetParameterValue("Param1") != null)
            {
                Param1 = theReport.GetParameterValue("Param1").ToString();
                if (!String.IsNullOrEmpty(Param1))
                {
                    loadSecondReport(Param1);
                }
                else
                {
                    loadSecondReport("");
                }
                theReport.SetParameterValue("Param1", null);
            }
        }



        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                ChangeParam1 = listBox2.SelectedItem.ToString();
                ChangeParam1Chnage();
                //Changelbl.Text = listBox2.SelectedItem.ToString();
                listBox3.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                ChangeParam1 = "";
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex >= 0)
            {
                ChangeParam1 = listBox3.SelectedItem.ToString();
                ChangeParam1Chnage();
                //Changelbl.Text = listBox2.SelectedItem.ToString();
                listBox2.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;
                ChangeParam1 = "";
            }
        }



        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex >= 0)
            {
                ChangeParam1 = listBox4.SelectedItem.ToString();
                ChangeParam1Chnage();
                listBox2.SelectedIndex = -1;
                listBox3.SelectedIndex = -1;
                ChangeParam1 = "";
            }
        }


        void Loadother()
        {
            string sec3 = "";

            sec3 = procs.ExtractBeforeColon(ChangeParam3);


            MWDataManager.clsDataAccess _dbManabc = new MWDataManager.clsDataAccess();
            _dbManabc.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManabc.SqlStatement = "  select  sum( UGYFinal) ugoutFinal from  [dbo].[tbl_GangClock]   \r\n" +
                                    "  where gangno in ( \r\n" +
                                    " select distinct(GangNo) from dbo.tbl_EmployeeAll where SUBSTRING(GangNo, 5,1) not in ('1','3') and SUBSTRING(GangNo, 5,1) = '" + sec3 + "') ";
            _dbManabc.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManabc.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManabc.ResultsTableName = "Lossesabc";
            _dbManabc.ExecuteInstruction();

            DataSet ReportDatasetabc = new DataSet();
            ReportDatasetabc.Tables.Add(_dbManabc.ResultsDataTable);
            theReport.RegisterData(ReportDatasetabc);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " select '" + ChangeParam2 + "' lbl, *, ISNULL  ( total , 0 ) aa1   \r\n" +
                                  " , ISNULL  ( ug , 0 ) ug1, ISNULL  ( total , 0 ) total1, ISNULL  ( SqmTotal , 0 ) SqmTotal1, ISNULL  ( adv , 0.0 ) adv1, actnum1-compl var1,   ActNum1- ISNULL  ( total , 0 ) var2,   ISNULL  ( total , 0 ) -  ISNULL  ( ug , 0 )  other \r\n" +
                                  " , case when ISNULL  (  ActNum1 , 0 ) > 0 then (ISNULL  ( SqmTotal , 0 ) + ISNULL  ( adv , 0.0 ))/ISNULL  (  ActNum1 , 0 ) else 0 end as sqmm from (select  \r\n" +
                                  " case when substring(gangno,6,7) in( '1200001') then '  Management' \r\n" +
                                  "   when substring(gangno,6,7) in( '1200002') then ' Eng Sup'  \r\n" +
                                  "   when substring(gangno,10,3) in( '168') then ' Lamproom'  \r\n" +
                                  "  when substring(gangno,11,1) in( '3') then ' Shaft'  \r\n" +
                                  "  when substring(gangno,6,7) in( '1100001') then '  Management' \r\n" +
                                  "  when substring(gangno,6,7) in( '1300001') then '  Management' \r\n" +
                                  "  when substring(gangno,6,7) in( '1400001') then '  Management' \r\n" +
                                  "  when substring(gangno,6,7) in( '1500001') then '  Management' \r\n" +
                                  "  when substring(gangno,6,7) in( '1600001') then '  Management' \r\n" +
                                  "  when substring(gangno,6,7) in( '1700001') then '  Management' \r\n" +
                                  "  when substring(gangno,6,7) in( '1100002') then ' Eng Sup' \r\n" +
                                  "   when substring(gangno,6,7) in( '1300002') then ' Eng Sup' \r\n" +
                                  "  when substring(gangno,6,7) in( '1400002') then ' Eng Sup' \r\n" +
                                  "  when substring(gangno,6,7) in( '1500002') then ' Eng Sup' \r\n" +
                                  "   when substring(gangno,6,7) in( '1600002') then ' Eng Sup' \r\n" +
                                  "   when substring(gangno,6,7) in( '1700002') then ' Eng Sup' \r\n" +
                                  "  when substring(gangno,8,3) in( '411') then ' Backfill' \r\n" +
                                  "  when substring(gangno,10,3) in( '545') and SUBSTRING(GangNo, 5,1) = '5' then ' BPF Dpt' \r\n" +
                                   " when substring(gangno,6,2) in( '13') and SUBSTRING(GangNo, 5,1) = '5' then ' Survey' \r\n" +
                                   "  when substring(gangno,10,3) in( '565') and SUBSTRING(GangNo, 5,1) = '5' then ' Rock Mechanics' \r\n " +
                                   "  when substring(gangno,10,3) in( '530') and SUBSTRING(GangNo, 5,1) = '5' then ' Geology' \r\n" +
                                   "  when substring(gangno,10,3) in( '501') and SUBSTRING(GangNo, 5,1) = '5' then ' Vent'  \r\n" +
                                  " else ' other'  END as calsaa,  \r\n" +
                                  " aa,  gangno a \r\n" +
                                  " , GangName xx,  convert(numeric(7,0),actcomp) compl, convert(numeric(7,0),ActNum) ActNum1 \r\n" +
                                  " from ( \r\n " +
                                  " select OrgUnit gangno, substring(OrgUnit,6,1)+'0'+substring(OrgUnit,7,1) aa, * from dbo.vw_orgunits  \r\n" +
                                  " where OrgUnit in (select distinct(GangNo) from dbo.tbl_EmployeeAll where SUBSTRING(GangNo, 5,1) not in ('1','3') and SUBSTRING(GangNo, 5,1) = '" + sec3 + "') \r\n" +
                                  " ) a \r\n" +
                                  " union  select '3) Stoping' calsaa, '" + sec3 + "' aa, 'No Stp Gang Assigned' a, '' xx, convert(numeric(7,0),0) compl , convert(numeric(7,0),0) Actnum  \r\n" +
                                  " union  select '2) Development' calsaa, '" + sec3 + "' aa, 'No Dev Gang Assigned' a, '' xx, convert(numeric(7,0),0) compl , convert(numeric(7,0),0) Actnum) a \r\n" +
                                  " left outer join \r\n" +
                                  " (select  GangNo gang, convert(numeric(7,0), UG) UG, convert(numeric(7,0),Total) Total, convert(numeric(7,0), UGOut) UGOut,  convert(numeric(7,0), UGYFinal) ugoutFinal from  [dbo].[tbl_GangClock]  \r\n" +
                                  " where GangNo in (select distinct(GangNo) from dbo.EmployeeAll where SUBSTRING(GangNo, 5,1) not in ('1','3') and SUBSTRING(GangNo, 5,1) = '" + sec3 + "') ) b  on a.a = b.gang \r\n" +
                                  " left outer join  \r\n" +
                                  " (select org, sum(SqmTotal) SqmTotal, sum(adv) adv from \r\n" +
                                  " ( select  case when SUBSTRING(OrgUnitDS,1,13) <> '' then  \r\n" +
                                  " SUBSTRING(OrgUnitDS,1,13)  when SUBSTRING(OrgUnitDS,1,13) = ''  \r\n" +
                                  " and Activity <> 1 then 'No Stp Gang Assigned'   \r\n" +
                                  " when SUBSTRING(OrgUnitDS,1,13) = '' \r\n " +
                                  " and Activity = 1 then 'No Dev Gang Assigned' \r\n" +
                                  " end as org, SqmTotal,  \r\n" +
                                  " case when activity = 1 then Adv else 0  end as adv  \r\n" +
                                  " from tbl_PlanMonth p , tbl_Section s , tbl_Section s1 \r\n " +
                                  " where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and \r\n " +
                                  " s.ReportToSectionid = s1.sectionid and s.prodmonth = s1.prodmonth and   \r\n" +
                                  " s1.ReportToSectionid = '" + sec3 + "' and p.Prodmonth =  \r\n" +
                                  " (select max(p.prodmonth) pm from tbl_Planning p, tbl_Section s , tbl_Section s1  \r\n" +
                                  " where p.sectionid = s.sectionid and p.prodmonth = s.prodmonth and  \r\n" +
                                  " s.ReportToSectionid = s1.sectionid and s.prodmonth = s1.prodmonth and  \r\n" +
                                  " calendardate = CONVERT(VARCHAR(10),GETDATE()-100,111) and s1.ReportToSectionid = '" + sec3 + "')) c  \r\n" +
                                  " group by org) c  on a.a = c.org where  calsaa not in ('2) Development','3) Stoping')   order by calsaa , a  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "Losses";
            _dbMan.ExecuteInstruction();

            DataSet ReportDataset = new DataSet();
            ReportDataset.Tables.Add(_dbMan.ResultsDataTable);

            theReport.RegisterData(ReportDataset);
            theReport.Load("EarlyMorningShiftRprt.frx");
            //theReport.Design();
            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();

            Application.Idle += new System.EventHandler(this.Application_Idle);

        }

        private void ChangeLbl3_TextChanged(object sender, EventArgs e)
        {
            if (ChangeLbl3.Text != "")
            {
                if (Loaded == "Y")
                {
                    this.Cursor = Cursors.WaitCursor;
                    Loadother();
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void lblTestChange_TextChanged(object sender, EventArgs e)
        {
            loadSecondReport(Param1);
        }

        private void ReportLbl_TextChanged(object sender, EventArgs e) => EarlyMornShiftDC2();

        private void Application_Idle(Object sender, EventArgs e)
        {
            if (theReportDetail.GetParameterValue("Param1") != null)
            {
                ReportLbl.Text = theReportDetail.GetParameterValue("Param1").ToString();
            }

            if (theReportDetail.GetParameterValue("Param3") != null)
            {
                ClickLbl.Text = theReportDetail.GetParameterValue("Param3").ToString();
            }

        }
        void EarlyMornShiftDC2()
        {
            DataTable Dates = new DataTable();
            DataTable Data = new DataTable();
            string Date = "";

            for (int s = 0; s <= 7; s++)
            {
                Dates.Columns.Add();
            }

            for (int s = 0; s <= 9; s++)
            {
                Data.Columns.Add();
            }

            string emp = ReportLbl.Text;
            //first dataset
            MWDataManager.clsDataAccess _dbManBanner = new MWDataManager.clsDataAccess();
            _dbManBanner.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManBanner.SqlStatement = "  select EmployeeNo, EmployeeName, GangNo, GangName from tbl_EmployeeAll  where employeeno = '" + emp + "' \r\n";
            _dbManBanner.SqlStatement = _dbManBanner.SqlStatement + "  ";
            _dbManBanner.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManBanner.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManBanner.ResultsTableName = "banner";
            _dbManBanner.ExecuteInstruction();

            DataSet ReportBanner = new DataSet();
            ReportBanner.Tables.Add(_dbManBanner.ResultsDataTable);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " select *,Substring(Convert(varchar(10), Clk_Date, 111), 1, 10) Date,substring(CONVERT(varchar, Clk_Time, 108), 0, 6) Time \r\n";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " from  HGSQLHR001.[TATEAM].dbo.vw_Clockings_WPAS  where Clk_EmpNumber = '" + emp + "' \r\n";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " -- and Substring(Convert(varchar(10),Clk_Time,111),1,10) >= convert(varchar(10), GETDATE() - 7, 111) \r\n";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "  order by Clk_Date asc ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "EarlyMornShift";
            _dbMan.ExecuteInstruction();

            int count = 0;
            int col = -1;
            int count2 = 0;

            Data.Rows.Add();

            foreach (DataRow dr in _dbMan.ResultsDataTable.Rows)
            {
                string dte = dr["Date"].ToString();
                if (Date != dr["Date"].ToString())
                {
                    count = 0;

                    col = col + 1;
                    Dates.Rows.Add();

                    Date = dr["Date"].ToString();
                    Dates.Rows[count][col] = Date;
                    Data.Rows[count][col] = dr["Time"].ToString() + ":    " + dr["Clk_AreaDes"].ToString();
                    Data.Rows[count][9] = dr["Time"].ToString();
                }
                else
                {

                    count = count + 1;
                    if (count > count2)
                    {
                        count2 = count;
                        Data.Rows.Add();
                    }

                    Data.Rows[count][col] = dr["Time"].ToString() + ":    " + dr["Clk_AreaDes"].ToString();

                    Data.Rows[count][9] = dr["Time"].ToString();
                }

            }

            for (int s = 0; s <= Data.Rows.Count - 1; s++)
            {

                if (Data.Rows[s][9] == null)
                {
                    Data.Rows.RemoveAt(s);
                }
                else
                {
                    if (Data.Rows[s][9].ToString() == "")
                    {
                        Data.Rows.RemoveAt(s);
                    }

                }
            }

            MWDataManager.clsDataAccess _dbMan40Day = new MWDataManager.clsDataAccess();
            _dbMan40Day.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan40Day.SqlStatement = " Exec sp_GetGangReportData_Employee '" + emp + "'  " +
                                  " ";
            _dbMan40Day.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan40Day.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan40Day.ResultsTableName = "Gang";  //get table name
            _dbMan40Day.ExecuteInstruction();

            DataSet ReportDetail2 = new DataSet();
            ReportDetail2.Tables.Add(_dbMan40Day.ResultsDataTable);

            Dates.TableName = "Dates";
            DataSet ReportDataset = new DataSet();
            ReportDataset.Tables.Add(Dates);

            Data.TableName = "Data";
            DataSet ReportData = new DataSet();
            ReportData.Tables.Add(Data);

            theReportDetail2.RegisterData(ReportDetail2);
            theReportDetail2.RegisterData(ReportDataset);
            theReportDetail2.RegisterData(ReportData);
            theReportDetail2.RegisterData(ReportBanner);
            theReportDetail2.Load(_reportFolder + "\\EarlyMorningEmpDetail.frx");
            //theReportDetail2.Design();

            FastReport.Utils.XmlItem item = FastReport.Utils.Config.Root.FindItem("Forms").FindItem("PreviewForm");
            item.SetProp("Maximized", "0");
            item.SetProp("Left", Convert.ToString(700));
            item.SetProp("Top", Convert.ToString(Bottom - 350));
            item.SetProp("Width", "900");
            item.SetProp("Height", "550");
            theReportDetail2.Show(false);

        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void barStaticItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
