using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Planning
{
    public partial class ucStartupsReport : ucBaseUserControl
    {
        Report theReport = new Report();
        private string ReportsFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

        public string _ucWorkPlace;
        public string _prodMonth;
        public string _myActivity;
        public string _DepartmentName;
        public string _ActionType;
        public string _Authorised;
        public string _CalDate;

        string lblAttachment = string.Empty;
        string lblAttachmentReturned = string.Empty;
        string lblActImgAttachment = string.Empty;

        string StartupImg = string.Empty;

        string RepDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage + "\\Startups\\Actions";
        string RepDirStartup = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDirImage + "\\Startups";

        public ucStartupsReport()
        {
            InitializeComponent();
        }

        private void ucStartupsReport_Load(object sender, EventArgs e)
        {
            //Header Data
            MWDataManager.clsDataAccess _headerDetial = new MWDataManager.clsDataAccess();
            _headerDetial.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _headerDetial.SqlStatement = "declare @WPID varchar(10)  \r\n"
                                    + " set @WPID = (select WorkplaceID from tbl_Workplace where Description =  '" + _ucWorkPlace + "')  \r\n"
                                    + " Select sec.NAME MinerName, sec.NAME_1 SbName, sec.NAME_2 MoName, sec.NAME_4 PMname, sec.NAME_5 Mine \r\n"
                                    + " from tbl_Planning p , Sections_Complete sec where p.Prodmonth = sec.Prodmonth and p.SectionID = sec.SectionID \r\n"
                                    + " and p.CalendarDate = '" + _CalDate + "' and WorkplaceID = @WPID";
            _headerDetial.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _headerDetial.queryReturnType = MWDataManager.ReturnType.DataTable;
            _headerDetial.ResultsTableName = "HeaderDetial";  //get table name
            _headerDetial.ExecuteInstruction();

            DataSet DSheaderDetial = new DataSet();
            DSheaderDetial.Tables.Add(_headerDetial.ResultsDataTable);

            //WorkplaceID
            MWDataManager.clsDataAccess _dbWPID = new MWDataManager.clsDataAccess();
            _dbWPID.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbWPID.SqlStatement = " select workplaceID from tbl_WORKPLACE where description = '" + _ucWorkPlace + "'";
            _dbWPID.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbWPID.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbWPID.ResultsTableName = "WorkplaceID";  //get table name
            _dbWPID.ExecuteInstruction();

            DataSet DSWPIDl = new DataSet();
            DSWPIDl.Tables.Add(_dbWPID.ResultsDataTable);

            string _WPID = string.Empty;
            _WPID = _dbWPID.ResultsDataTable.Rows[0][0].ToString();

            //Startup Questions
            MWDataManager.clsDataAccess _StartupData = new MWDataManager.clsDataAccess();
            _StartupData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _StartupData.SqlStatement = " declare @WPID varchar(10) \r\n "
                + " set @WPID = (select WorkplaceID from tbl_WORKPLACE where Description = '" + _ucWorkPlace + "') \r\n "
                + "exec sp_Startup_" + _DepartmentName + "_Questions '" + String.Format("{0:yyyy-MM-dd}", _prodMonth) + "', @WPID \r\n ";

            _StartupData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _StartupData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _StartupData.ResultsTableName = "StartupQuestions";  //get table name
            _StartupData.ExecuteInstruction();

            DataSet StartupReportData = new DataSet();
            StartupReportData.Tables.Add(_StartupData.ResultsDataTable);

            //Startup Image
            LoadImage(_DepartmentName);
            StartupImg = lblAttachmentReturned;
            lblAttachmentReturned = lblActImgAttachment;

            //Actions
            MWDataManager.clsDataAccess _StartupActionsData = new MWDataManager.clsDataAccess();
            _StartupActionsData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _StartupActionsData.SqlStatement = "  SELECT *, substring(HOD, CHARINDEX (':', HOD)+1, LEN(HOD)) as MO,  substring(RespPerson, CHARINDEX (':', RespPerson)+1, len(RespPerson)) as RespPerson, isnull(compnotes, '') Compnotes1, \r\n" +
                                        "case when CompNotes<> '' then '" + RepDir + "\\' + CompNotes + '.png' \r\n" +
                                        "else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
                                        "where Type = '" + _ActionType + "' and Completiondate is null and workplace in ('" + _ucWorkPlace + "')";
            _StartupActionsData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _StartupActionsData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _StartupActionsData.ResultsTableName = "StartupAction";  //get table name
            _StartupActionsData.ExecuteInstruction();

            DataSet StartupActionsData = new DataSet();
            StartupActionsData.Tables.Add(_StartupActionsData.ResultsDataTable);

            //Register Data
            theReport.RegisterData(StartupReportData);
            theReport.RegisterData(StartupActionsData);
            theReport.RegisterData(DSheaderDetial);

            theReport.Load(ReportsFolder + "\\Startup" + _DepartmentName + ".frx");

            //Set the Parameters
            theReport.SetParameterValue("DeptName", _DepartmentName);
            theReport.SetParameterValue("Authorised", _Authorised);
            theReport.SetParameterValue("WPID", _WPID);
            theReport.SetParameterValue("Activity", _myActivity);
            theReport.SetParameterValue("Workplace", _ucWorkPlace);
            theReport.SetParameterValue("ReportDate", _prodMonth);
            theReport.SetParameterValue("Banner", Mineware.Systems.ProductionGlobal.ProductionGlobal.Banner);
            theReport.SetParameterValue("StartupImg", StartupImg);
            theReport.SetParameterValue("ActionImg", lblAttachmentReturned);

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        void LoadImage(string _Departement)
        {
            if (Directory.Exists((@RepDirStartup + "\\" + _Departement)))
            {
                System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(@RepDirStartup + "\\" + _Departement);
                string[] files = System.IO.Directory.GetFiles(@RepDirStartup + "\\" + _Departement);

                foreach (var item in files)
                {
                    string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                    int extpos = aa.IndexOf(".");

                    string ext = aa.Substring(extpos, aa.Length - extpos);

                    if (item == RepDirStartup + "\\" + _Departement + string.Empty + "\\" + _ucWorkPlace + _prodMonth + ext)
                    {
                        lblAttachment = item;
                    }
                }
                if (lblAttachment != string.Empty)
                {
                    lblAttachmentReturned = lblAttachment;
                }
                else
                {
                    lblAttachmentReturned = "NoImage";
                }
            }

            if (Directory.Exists(RepDir))
            {
                lblAttachment = string.Empty;

                System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir);
                string[] files = System.IO.Directory.GetFiles(RepDir);

                foreach (var item in files)
                {
                    string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                    int extpos = aa.IndexOf(".");

                    string ext = aa.Substring(extpos, aa.Length - extpos);

                    if (item == RepDir + "\\" + _ucWorkPlace + _Departement + _prodMonth + ext)
                    {
                        lblAttachment = item;
                    }
                }
                if (lblAttachment != string.Empty)
                {
                    lblActImgAttachment = lblAttachment;
                }
                else
                {
                    lblActImgAttachment = "NoImage";
                }
            }

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
