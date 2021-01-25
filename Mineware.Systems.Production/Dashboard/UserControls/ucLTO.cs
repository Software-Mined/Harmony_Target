using DevExpress.XtraEditors;
using FastReport;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.Production.Departmental.RockEngineering;
using Mineware.Systems.ProductionGlobal;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Dashboard.UserControls
{
    public partial class ucLTO : XtraUserControl
    {
        private DataTable dtGetData = new DataTable("dtGetData");
        private DataTable dtGetDataDev = new DataTable("dtGetDataDev");
        private BackgroundWorker bgwDataLTO = new BackgroundWorker();
        private Report theReport = new Report();
        private string ReportsFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        StringBuilder sbSqlQuery = new StringBuilder();

        #region Private Variables
        private string Attachment;
        private string Picture;
        private string SectionID;
        private string SelectedField;
        private string WPID;
        private string Workplace;
        private string SelectedCol;
        private string ActType;
        private string ADate;

        private string BDate;
        private string CrewID;
        private readonly string LastDateBook;

        DataSet dsLToOp = new DataSet();
        DataSet dsLToOpDev = new DataSet();

        #endregion

        public ucLTO()
        {
            InitializeComponent();
            bgwDataLTO.RunWorkerCompleted += bgwDataLTO_RunWorkerCompleted;
        }

        private void bgwDataLTO_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (dsLToOp.Tables.Count > 0)
                dsLToOp.Tables.Clear();

            dsLToOp.Tables.Add(dtGetData);

            gcLTOStope.DataSource = dsLToOp.Tables[0];

            gcSection.FieldName = "MOSection";
            gcWorkplace.FieldName = "WPDescription";
            gcCritSkills.FieldName = "ColCritSkill";//1
            gcMajHazards.FieldName = "ColMajHazards";//4
            gcProduction.FieldName = "ColProd";//6
            gcGeoScience.FieldName = "ColGeol";//7
            gcRockEng.FieldName = "ColRock";//8
            gcSeismic.FieldName = "ColSeismic";//8a
            gcVentilation.FieldName = "ColVent";//9
            gcPlanRate.FieldName = "ColPlanRate";//10
            gcLTOStope.Enabled = true;


            if (dsLToOpDev.Tables.Count > 0)
                dsLToOpDev.Tables.Clear();

            dsLToOpDev.Tables.Add(dtGetDataDev);

            gcLTODev.DataSource = dsLToOpDev.Tables[0];

            gcSectionDev.FieldName = "MOSection";
            gcWorkplaceDev.FieldName = "WPDescription";
            gcCritSkillsDev.FieldName = "ColCritSkill";//1
            gcMajHazardsDev.FieldName = "ColMajHazards";//4
            gcProductionDev.FieldName = "ColProd";//6
            gcGeoScienceDev.FieldName = "ColGeol";//7
            gcRockEngDev.FieldName = "ColRock";//8
            //gcSeismicDev.FieldName = "ColSeismic";//8a
            gcVentilationDev.FieldName = "ColVent";//9
            gcPlanRateDev.FieldName = "ColPlanRate";//10
            gcLTODev.Enabled = true;
        }

        private void ucLTO_Load(object sender, EventArgs e)
        {
            Picture = ProductionGlobal.ProductionGlobalTSysSettings._RepDir;

            bgwDataLTO.RunWorkerAsync();
            gcLTOStope.Enabled = false;
            LoadLicenceToOperate();

        }

        private void LoadLicenceToOperate()
        {

            MWDataManager.clsDataAccess _dbManLToOp = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618 // Type or member is obsolete
            _dbManLToOp.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618 // Type or member is obsolete
            _dbManLToOp.SqlStatement = "select WPDescription+orgdescription aa,max(substring(section,1,4)) MOSection, WPDescription, OrgDescription,  max(ColCritSkill) ColCritSkill,  max(ColRiskProfile) ColRiskProfile   \r\n" +
                             " ,max(ColDeviations) ColDeviations,  max(ColMajHazards) ColMajHazards,  max(ColEnviro) ColEnviro,  max(ColProd) ColProd  \r\n" +
                            " ,max(ColGeol) ColGeol,  max(ColRock) ColRock,  max(ColVent) ColVent,  sum(ColPlanRate) ColPlanRate,   \r\n" +
                            " case when max(colPlanRate) > 18 and max(colRock) > 80 then  4   \r\n" +
                            " when max(colPlanRate) > 15 and max(colRock) > 70 then  5   \r\n" +
                            " when max(colPlanRate) > 10 and max(colRock) > 60 then  4   \r\n" +
                            " when max(colPlanRate) > 10  then  3   \r\n" +
                            " else 0  end as Weighting  \r\n" +
                            " from(  \r\n" +
                            "select MajCat, SubCat, section, wpdescription, orgdescription, \r\n" +
                            " case when a.majcat = '1) Critical Skills' and day30 = '3' then 3  \r\n" +
                            " when a.majcat = '1) Critical Skills' and day30 = '2' then 2  \r\n" +
                            " end as ColCritSkill,  \r\n" +
                            " case when a.majcat = '2) Workplace Risk Profiles' and day30 = '3' then 3  \r\n" +
                            " when a.majcat = '2) Workplace Risk Profiles' and day30 = '2' then 2  \r\n" +
                             "end as ColRiskProfile,  \r\n" +
                            " case when a.majcat = '3) Workplace Deviations' and day30 = '3' then 3  \r\n" +
                             "when a.majcat = '3) Workplace Deviations' and day30 = '2' then 2  \r\n" +
                            " end as ColDeviations,  \r\n" +
                            " case when a.majcat = '4) Major Hazards' and day30 = '3' then 3  \r\n" +
                            " when a.majcat = '4) Major Hazards' and day30 = '2' then '2'  \r\n" +
                            " end as ColMajHazards,  \r\n" +
                            " case when a.majcat = '5) Enviromental Conditions' and day30 = '3' then 3  \r\n" +
                            " when a.majcat = '5) Enviromental Conditions' and day30 = '2' then 2  \r\n" +
                            " end as ColEnviro,  \r\n" +
                            " case when a.majcat = '6) Production' then day30  \r\n" +                           
                            " end as ColProd,  \r\n" +
                            " case when a.majcat = '7) Geology Mapping' and day30 = '3' then 'High'  \r\n" +
                            " when a.majcat = '7) Geology Mapping' and day30 = '2' then 'Medium'  \r\n" +
                            " end as ColGeol,  \r\n" +
                            " case when a.majcat = '8) Rock Engineering Visits' and convert(decimal, (day30)) > 0 then convert(decimal, (day30))  \r\n" +
                             " end as ColRock,  \r\n" +
                            " case when a.majcat = '9) Ventilation Visits' and convert(decimal, (day30)) = 3 then 'High'  \r\n" +
                             "when a.majcat = '9) Ventilation Visits' and day30 = '2' then 'Medium'  \r\n" +
                            " end as ColVent,  \r\n" +
                            " case when a.majcat like '10)%' and day30 <> 'Off' and convert(smallint, (day30)) > 0 then convert(smallint, (day30))  \r\n" +
                             "end as ColPlanRate  \r\n" +
                             "from[dbo].[tbl_LicenceToOperateSum] a where substring(wpid, 1, 1) = 'S')  a  \r\n" +
                            "group by wpdescription, orgdescription  \r\n" +
                            " order by Weighting Desc \r\n" +
                            " ";
            _dbManLToOp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLToOp.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManLToOp.ExecuteInstruction();

            dtGetData = _dbManLToOp.ResultsDataTable;

            ///Dev

            MWDataManager.clsDataAccess _dbManLToOpDev = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManLToOpDev.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManLToOpDev.SqlStatement = "select WPDescription+orgdescription aa,max(substring(section,1,4)) MOSection, WPDescription, OrgDescription,  max(ColCritSkill) ColCritSkill,  max(ColRiskProfile) ColRiskProfile   \r\n" +
                             " ,max(ColDeviations) ColDeviations,  max(ColMajHazards) ColMajHazards,  max(ColEnviro) ColEnviro,  max(ColProd) ColProd  \r\n" +
                            " ,max(ColGeol) ColGeol,  max(ColRock) ColRock,  max(ColVent) ColVent,  sum(ColPlanRate) ColPlanRate,   \r\n" +
                            " case when max(colPlanRate) > 18 and max(colRock) > 80 then  4   \r\n" +
                            " when max(colPlanRate) > 15 and max(colRock) > 70 then  5   \r\n" +
                            " when max(colPlanRate) > 10 and max(colRock) > 60 then  4   \r\n" +
                            " when max(colPlanRate) > 10  then  3   \r\n" +
                            " else 0  end as Weighting  \r\n" +
                            " from(  \r\n" +
                            "select MajCat, SubCat, section, wpdescription, orgdescription, \r\n" +
                            " case when a.majcat = '1) Critical Skills' and day30 = '3' then 3  \r\n" +
                            " when a.majcat = '1) Critical Skills' and day30 = '2' then 2  \r\n" +
                            " end as ColCritSkill,  \r\n" +
                            " case when a.majcat = '2) Workplace Risk Profiles' and day30 = '3' then 3  \r\n" +
                            " when a.majcat = '2) Workplace Risk Profiles' and day30 = '2' then 2  \r\n" +
                             "end as ColRiskProfile,  \r\n" +
                            " case when a.majcat = '3) Workplace Deviations' and day30 = '3' then 3  \r\n" +
                             "when a.majcat = '3) Workplace Deviations' and day30 = '2' then 2  \r\n" +
                            " end as ColDeviations,  \r\n" +
                            " case when a.majcat = '4) Major Hazards' and day30 = '3' then 3  \r\n" +
                            " when a.majcat = '4) Major Hazards' and day30 = '2' then '2'  \r\n" +
                            " end as ColMajHazards,  \r\n" +
                            " case when a.majcat = '5) Enviromental Conditions' and day30 = '3' then 3  \r\n" +
                            " when a.majcat = '5) Enviromental Conditions' and day30 = '2' then 2  \r\n" +
                            " end as ColEnviro,  \r\n" +
                            " case when a.majcat = '6) Production' then day30  \r\n" +
                            " end as ColProd,  \r\n" +
                            " case when a.majcat = '7) Geology Mapping' and day30 = '3' then 'High'  \r\n" +
                            " when a.majcat = '7) Geology Mapping' and day30 = '2' then 'Medium'  \r\n" +
                            " end as ColGeol,  \r\n" +
                            " case when a.majcat = '8) Rock Engineering Visits' and convert(decimal, (day30)) > 0 then convert(decimal, (day30))  \r\n" +
                             " end as ColRock,  \r\n" +
                            " case when a.majcat = '9) Ventilation Visits' and convert(decimal, (day30)) = 3 then 'High'  \r\n" +
                             "when a.majcat = '9) Ventilation Visits' and day30 = '2' then 'Medium'  \r\n" +
                            " end as ColVent,  \r\n" +
                            " case when a.majcat like '10)%' and day30 <> 'Off' and convert(smallint, (day30)) > 0 then convert(smallint, (day30))  \r\n" +
                             "end as ColPlanRate  \r\n" +
                             "from[dbo].[tbl_LicenceToOperateSum] a where substring(wpid, 1, 1) = 'D')  a  \r\n" +
                            "group by wpdescription, orgdescription  \r\n" +
                            " order by Weighting Desc \r\n" +
                            " ";
            _dbManLToOpDev.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLToOpDev.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManLToOpDev.ExecuteInstruction();

            dtGetDataDev = _dbManLToOpDev.ResultsDataTable;

        }

        private Image Base64ToImage(string base64String, string WorkplaceName)
        {
            Picture = base64String.Trim().Replace(" ", "+");
            Picture = base64String.Trim().Replace("_", "/");

            if (Picture.Length % 4 > 0)
                Picture = Picture.PadRight(Picture.Length + 4 - Picture.Length % 4, '=');

            decimal Length = Picture.Length;

            byte[] imageBytes = Convert.FromBase64String(Picture);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);

            MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618 
            _dbManImage.SqlStatement = "update [dbo].[tbl_RockMechInspection] set picture = '" + Picture + "' where workplace = '" + WorkplaceName + "' and (captyear*100)+captweek = (select max(captyear*100+captweek) from  [dbo].[tbl_RockMechInspection] where workplace = '" + WorkplaceName + "') ";
            _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManImage.ResultsTableName = "Image";
            _dbManImage.ExecuteInstruction();

            return image;
        }

        private void gcLTO_DoubleClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string Shaft = string.Empty;

            if (SelectedField == "Workplace (D-Qlik)")
            {
                ///////////////////////1.Comp A//////////////////////////////////////////////////////
                MWDataManager.clsDataAccess _dbManCompA = new MWDataManager.clsDataAccess();
                _dbManCompA.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManCompA.SqlStatement = "exec [dbo].sp_LicenceToOperate_CompA '" + Workplace + "'";
                _dbManCompA.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCompA.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCompA.ResultsTableName = "DetailsCompA";
                _dbManCompA.ExecuteInstruction();

                DataSet ReportDatasetCompADetail = new DataSet();
                ReportDatasetCompADetail.Tables.Add(_dbManCompA.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManCompA1 = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManCompA1.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManCompA1.SqlStatement = "select * from [ZAWW2K16SQL01].[AngloQlikView].[dbo].[vw_PaperlessActionManagerPrint_AllActions] ";
                _dbManCompA1.SqlStatement = _dbManCompA1.SqlStatement + " where datesubmitted > getdate()-30 and taskname like '%CompA%' and workplace = '" + Workplace + "' and actionstatus <> 'Not A Deficiency'and actionstatus <> 'Not Applicable'  ";
                _dbManCompA1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCompA1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCompA1.ResultsTableName = "PaperlessCompA";
                _dbManCompA1.ExecuteInstruction();

                DataSet ReportDatasetCompADetailA = new DataSet();
                ReportDatasetCompADetailA.Tables.Add(_dbManCompA1.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManCompA2 = new MWDataManager.clsDataAccess();
                _dbManCompA2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManCompA2.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
                _dbManCompA2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCompA2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCompA2.ResultsTableName = "HeaderCompA";
                _dbManCompA2.ExecuteInstruction();

                DataSet ReportDatasetCompADetailB = new DataSet();
                ReportDatasetCompADetailB.Tables.Add(_dbManCompA2.ResultsDataTable);

                ///////////////////////2.Call Centre Application//////////////////////////////////////     
                MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
                _dbMan1.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbMan1.SqlStatement = " select * from  [AGA_ONE].[dbo].[vw_rpt_MW_CCA] where worktypecat = 'STP' \r\n " +
                                        " and shaftid = '" + Shaft + "' and answerdate = CONVERT(VARCHAR(10),getdate(), 20) \r\n " +
                                        " and wpequipno = '" + Workplace + "' \r\n " +
                                        " and ragind = 'R' ";
                _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan1.ResultsTableName = "Data1";
                _dbMan1.ExecuteInstruction();

                string Visible = "N";

                if (_dbMan1.ResultsDataTable.Rows.Count > 0)
                {
                    Visible = "Y";
                }

                DataSet ReportDatasetReport1 = new DataSet();
                ReportDatasetReport1.Tables.Add(_dbMan1.ResultsDataTable);

                MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbMan2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbMan2.SqlStatement = " select MAX(col1) col1, MAX(col2) col2, MAX(col3) col3,MAX(col4) col4,MAX(col5) col5, \r\n " +
                                            " MAX(col6) col6, MAX(col7) col7, MAX(col8) col8 , MAX(col9) col9 , MAX(col10) col10, \r\n " +
                                            " MAX(col11) col11,MAX(col12) col12,MAX(col13) col13,MAX(col14) col14,MAX(col15) col15, \r\n " +
                                            " MAX(col16) col16,MAX(col17) col17,MAX(col18) col18,MAX(col19) col19,MAX(col20) col20, \r\n " +
                                            " MAX(col21) col21,MAX(col22) col22,MAX(col23) col23,MAX(col24) col24,MAX(col25) col25, \r\n " +
                                            " MAX(col26) col26,MAX(col27) col27,MAX(col28) col28,MAX(col29) col29,MAX(col30) col30, \r\n " +
                                            " MAX(col31) col31 \r\n " +
                                            " from(select  \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate(), 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate(), 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col1, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-1, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-1, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col2, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-2, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-2, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col3, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-3, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-3, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col4, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-4, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-4, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col5, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-5, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-5, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col6, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-6, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-6, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col7, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-7, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-7, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col8, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-8, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-8, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col9, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-9, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-9, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col10, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-10, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-10, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col11, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-11, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-11, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col12, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-12, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-12, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col13, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-13, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-13, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col14, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-14, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-14, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col15, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-15, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-15, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col16, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-16, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-16, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col17, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-17, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-17, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col18, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-18, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-18, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col19, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-19, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-19, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col20, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-20, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-20, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col21, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-21, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-21, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col22, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-22, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-22, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col23, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-23, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-23, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col24, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-24, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-24, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col25, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-25, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-25, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col26, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-26, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-26, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col27, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-27, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-27, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col28, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-28, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-28, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col29, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-29, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-29, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col30, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-30, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-30, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col31, \r\n " +
                                            " * from ( \r\n " +
                                            " select calendardate, max(workingday) wd from tbl_planning p, tbl_Workplace w \r\n " +
                                            " where p.workplaceid = w.workplaceid and calendardate > getdate()-30 \r\n " +
                                            " and calendardate < getdate() \r\n " +
                                            " and description =  '" + Workplace + "' group by calendardate) a \r\n " +
                                            " left outer join \r\n " +
                                            " (select answerdate from  [AGA_ONE].[dbo].[vw_rpt_MW_CCA] where worktypecat = 'STP' \r\n " +
                                            " and shaftid = '" + Shaft + "' and answerdate >= CONVERT(VARCHAR(10),getdate()-30, 20) \r\n " +
                                            " and wpequipno = '" + Workplace + "'  and ShiftCode = 'D' \r\n " +
                                            " group by answerdate) b on a.calendardate = b.answerdate )a  ";
                _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan2.ResultsTableName = "Data2";
                _dbMan2.ExecuteInstruction();

                DataSet ReportDatasetReport2 = new DataSet();
                ReportDatasetReport2.Tables.Add(_dbMan2.ResultsDataTable);

                //////N/Shift
                MWDataManager.clsDataAccess _dbMan2NS = new MWDataManager.clsDataAccess();
                _dbMan2NS.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbMan2NS.SqlStatement = " select MAX(col1) col1, MAX(col2) col2, MAX(col3) col3,MAX(col4) col4,MAX(col5) col5, \r\n " +
                                            " MAX(col6) col6, MAX(col7) col7, MAX(col8) col8 , MAX(col9) col9 , MAX(col10) col10, \r\n " +
                                            " MAX(col11) col11,MAX(col12) col12,MAX(col13) col13,MAX(col14) col14,MAX(col15) col15, \r\n " +
                                            " MAX(col16) col16,MAX(col17) col17,MAX(col18) col18,MAX(col19) col19,MAX(col20) col20, \r\n " +
                                            " MAX(col21) col21,MAX(col22) col22,MAX(col23) col23,MAX(col24) col24,MAX(col25) col25, \r\n " +
                                            " MAX(col26) col26,MAX(col27) col27,MAX(col28) col28,MAX(col29) col29,MAX(col30) col30, \r\n " +
                                            " MAX(col31) col31 \r\n " +
                                            " from(select  \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate(), 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate(), 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col1, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-1, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-1, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col2, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-2, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-2, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col3, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-3, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-3, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col4, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-4, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-4, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col5, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-5, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-5, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col6, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-6, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-6, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col7, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-7, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-7, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col8, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-8, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-8, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col9, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-9, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-9, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col10, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-10, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-10, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col11, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-11, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-11, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col12, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-12, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-12, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col13, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-13, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-13, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col14, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-14, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-14, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col15, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-15, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-15, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col16, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-16, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-16, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col17, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-17, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-17, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col18, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-18, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-18, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col19, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-19, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-19, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col20, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-20, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-20, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col21, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-21, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-21, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col22, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-22, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-22, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col23, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-23, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-23, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col24, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-24, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-24, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col25, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-25, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-25, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col26, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-26, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-26, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col27, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-27, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-27, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col28, \r\n " +
                                            " case  \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-28, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-28, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else ''  \r\n " +
                                            " end as col29, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-29, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-29, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col30, \r\n " +
                                            " case \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-30, 20) and wd = 'N' then 'OFF' \r\n " +
                                            " when calendardate = CONVERT(VARCHAR(10),getdate()-30, 20) and wd = 'Y' And AnswerDate IS not null then 'OK' \r\n " +
                                            " else '' \r\n " +
                                            " end as col31, \r\n " +
                                            " * from ( \r\n " +
                                            " select calendardate, max(workingday) wd from tbl_planning p, tbl_Workplace w \r\n " +
                                            " where p.workplaceid = w.workplaceid and calendardate > getdate()-30 \r\n " +
                                            " and calendardate < getdate() \r\n " +
                                            " and description =  '" + Workplace + "' group by calendardate) a \r\n " +
                                            " left outer join \r\n " +
                                            " (select answerdate from  [AGA_ONE].[dbo].[vw_rpt_MW_CCA] where worktypecat = 'STP' \r\n " +
                                            " and shaftid = '" + Shaft + "' and answerdate >= CONVERT(VARCHAR(10),getdate()-30, 20) \r\n " +
                                            " and wpequipno = '" + Workplace + "' and ShiftCode = 'N' \r\n " +
                                            " group by answerdate) b on a.calendardate = b.answerdate )a ";
                _dbMan2NS.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan2NS.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan2NS.ResultsTableName = "DataNS";
                _dbMan2NS.ExecuteInstruction();

                DataSet ReportDatasetReport2NS = new DataSet();
                ReportDatasetReport2NS.Tables.Add(_dbMan2NS.ResultsDataTable);

                MWDataManager.clsDataAccess _dbMan3 = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbMan3.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbMan3.SqlStatement = " select Top 10 QuestionDescr, num, colOrder from( select \r\n " +
                                        " QuestionDescr, count(QuestionDescr) num, 'a' colOrder from  [AGA_ONE].[dbo].[vw_rpt_MW_CCA] where worktypecat = 'STP' \r\n " +
                                        " and shaftid = '" + Shaft + "' and answerdate >= CONVERT(VARCHAR(10),getdate()-30, 20) \r\n " +
                                        " and wpequipno = '" + Workplace + "' \r\n " +
                                        " and ragind = 'R' group by QuestionDescr \r\n " +
                                        " union  \r\n " +
                                        " select ' ',0,'z1' \r\n " +
                                        " union \r\n " +
                                        " select '  ',0,'z2' \r\n " +
                                        " union  \r\n " +
                                        " select '   ',0,'z3' \r\n " +
                                        " union  \r\n " +
                                        " select '    ',0,'z4' \r\n " +
                                        " union  \r\n " +
                                        " select '     ',0,'z5' \r\n " +
                                        " union \r\n " +
                                        " select '      ',0,'z6' \r\n " +
                                        " union  \r\n " +
                                        " select '       ',0,'z7' \r\n " +
                                        " union  \r\n " +
                                        " select '        ',0,'z8' \r\n " +
                                        " union  \r\n " +
                                        " select '         ',0,'z9' \r\n " +
                                        " union \r\n " +
                                        " select '          ',0,'z10')a \r\n " +
                                        " Order by colOrder, num desc ";
                _dbMan3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan3.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan3.ResultsTableName = "Data3";
                _dbMan3.ExecuteInstruction();

                DataSet ReportDatasetReport3 = new DataSet();
                ReportDatasetReport3.Tables.Add(_dbMan3.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManDate = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManDate.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManDate.SqlStatement = "select getdate()";
                _dbManDate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDate.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDate.ResultsTableName = "Date";
                _dbManDate.ExecuteInstruction();

                DataSet ReportDatasetReport4 = new DataSet();
                ReportDatasetReport4.Tables.Add(_dbManDate.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManGenData = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManGenData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManGenData.SqlStatement = " select '" + Workplace + "' WP,  '" + ProductionGlobalTSysSettings._Banner + "' mine, '" + Visible + "' mine ";
                _dbManGenData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGenData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGenData.ResultsTableName = "GenData";
                _dbManGenData.ExecuteInstruction();

                DataSet ReportDatasetReport5 = new DataSet();
                ReportDatasetReport5.Tables.Add(_dbManGenData.ResultsDataTable);

                //////////////////////3.Critical Skills////////////////////////////////////// 
                MWDataManager.clsDataAccess _dbManWPData = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManWPData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManWPData.SqlStatement = "select  top(1) pfnumber  miner,pm.orgunitds org, w.Description \r\n " +
                                            "from tbl_planning p, tbl_Workplace w, tbl_planmonth pm, tbl_section s \r\n " +
                                            "where p.calendardate = CONVERT(VARCHAR(10),getdate(), 20) \r\n " +
                                            "and p.workplaceid = w.workplaceid and p.prodmonth = pm.prodmonth \r\n " +
                                            "and p.workplaceid = pm.workplaceid and p.activity = pm.activity and p.sectionid = pm.sectionid \r\n " +
                                            "and p.prodmonth = s.prodmonth and p.sectionid = s.sectionid \r\n " +
                                            "and w.description = '" + Workplace + "'  order by calendardate desc, p.orgunitds desc ";
                _dbManWPData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPData.ResultsTableName = "RawData";
                _dbManWPData.ExecuteInstruction();

                string Miner = string.Empty;
                string Org = string.Empty;
                string WP = string.Empty;

                if (_dbManWPData.ResultsDataTable.Rows.Count > 0)
                {
                    Miner = _dbManWPData.ResultsDataTable.Rows[0]["miner"].ToString();
                    Org = _dbManWPData.ResultsDataTable.Rows[0]["org"].ToString().Substring(0, 15);
                    WP = _dbManWPData.ResultsDataTable.Rows[0]["description"].ToString();
                }

                MWDataManager.clsDataAccess _dbManDateCS = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManDateCS.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManDateCS.SqlStatement = "select getdate() , '" + Miner + "' mm, '" + Org + "'  org ";
                _dbManDateCS.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDateCS.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDateCS.ResultsTableName = "DateCS";
                _dbManDateCS.ExecuteInstruction();

                DataSet ReportDatasetReportCS = new DataSet();
                ReportDatasetReportCS.Tables.Add(_dbManDateCS.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManMinerMain = new MWDataManager.clsDataAccess();
                _dbManMinerMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManMinerMain.SqlStatement = " select a.*, case when ug is null then 'N' else 'Y' end as UG from (select 'a' a, reader_description, max(clock_time) cc \r\n " +
                                               " from [tbl_LicenceToOperate_Labour_Import]  \r\n " +
                                               " where emp_Empno = '" + Miner + "'   and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n " +
                                               " group by reader_description) a  left outer join \r\n " +
                                               " (select 'a' a, max(clock_time) ug from [tbl_LicenceToOperate_Labour_Import]  \r\n " +
                                               " where emp_Empno = '" + Miner + "'   and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n " +
                                               " and reader_code like '%UG%') b on a.a = b.a \r\n " +
                                               " order by cc ";
                _dbManMinerMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMinerMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMinerMain.ResultsTableName = "MinerToDay";
                _dbManMinerMain.ExecuteInstruction();

                ReportDatasetReportCS.Tables.Add(_dbManMinerMain.ResultsDataTable);

                ////Last Clocking Miner
                MWDataManager.clsDataAccess _dbManMinerMainLastClock = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManMinerMainLastClock.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManMinerMainLastClock.SqlStatement = " select top(1)  clock_time, READER_DESCRIPTION, emp_empno  from [tbl_LicenceToOperate_Labour_Import]   \r\n  " +
                                                        " where emp_Empno = '" + Miner + "' \r\n " +
                                                        " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20) order by clock_time desc ";
                _dbManMinerMainLastClock.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMinerMainLastClock.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMinerMainLastClock.ResultsTableName = "MinerToDayLastClock";
                _dbManMinerMainLastClock.ExecuteInstruction();

                ReportDatasetReportCS.Tables.Add(_dbManMinerMainLastClock.ResultsDataTable);

                // miner histrory
                MWDataManager.clsDataAccess _dbManMinerHist = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManMinerHist.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManMinerHist.SqlStatement = " select emp_empno, max(emp_name) name \r\n" +
                                               " ,max(Day40) Day40, max(Day39) Day39, max(Day38) Day38, max(Day37) Day37, max(Day36) Day36, max(Day35) Day35 \r\n" +
                                               " ,max(Day34) Day34, max(Day33) Day33, max(Day32) Day32, max(Day31) Day31, max(Day30) Day30, max(Day29) Day29 \r\n" +
                                               " ,max(Day28) Day28, max(Day27) Day27, max(Day26) Day26, max(Day25) Day25 \r\n" +
                                               " ,max(Day24) Day24, max(Day23) Day23, max(Day22) Day22, max(Day21) Day21, max(Day20) Day20, max(Day19) Day19 \r\n" +
                                               " ,max(Day18) Day18, max(Day17) Day17, max(Day16) Day16, max(Day15) Day15 \r\n" +
                                               " ,max(Day14) Day14, max(Day13) Day13, max(Day12) Day12, max(Day11) Day11, max(Day10) Day10 \r\n" +
                                               "  from (SELECT emp_empno, emp_name, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate(), 20) then day2 else '' end as Day40, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-1, 20) then day2 else '' end as Day39, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-2, 20) then day2 else '' end as Day38, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-3, 20) then day2 else '' end as Day37, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-4, 20) then day2 else '' end as Day36, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-5, 20) then day2 else '' end as Day35, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-6, 20) then day2 else '' end as Day34, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-7, 20) then day2 else '' end as Day33, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-8, 20) then day2 else '' end as Day32, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-9, 20) then day2 else '' end as Day31, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-10, 20) then day2 else '' end as Day30, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-11, 20) then day2 else '' end as Day29, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-12, 20) then day2 else '' end as Day28, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-13, 20) then day2 else '' end as Day27, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-14, 20) then day2 else '' end as Day26, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-15, 20) then day2 else '' end as Day25, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-16, 20) then day2 else '' end as Day24, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-17, 20) then day2 else '' end as Day23, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-18, 20) then day2 else '' end as Day22, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-19, 20) then day2 else '' end as Day21, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-20, 20) then day2 else '' end as Day20, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-21, 20) then day2 else '' end as Day19, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-22, 20) then day2 else '' end as Day18, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-23, 20) then day2 else '' end as Day17, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-24, 20) then day2 else '' end as Day16, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-25, 20) then day2 else '' end as Day15, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-26, 20) then day2 else '' end as Day14, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-27, 20) then day2 else '' end as Day13, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-28, 20) then day2 else '' end as Day12, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-29, 20) then day2 else '' end as Day11, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-30, 20) then day2 else '' end as Day10 \r\n" +
                                               " from  MW_PassStageDB.dbo.ATTENDANCE \r\n" +
                                               " where   \r\n" +
                                               " convert(varchar(20),attendance_date)+emp_empno in  \r\n" +
                                               " (select convert(varchar(20),attendance_date)+emp_empno from  \r\n" +
                                               " MW_PassStageDB.dbo.ATTENDANCE where emp_Empno ='" + Miner + "' ) \r\n" +
                                               " and emp_empno in (select distinct(emp_empno) a from  [tbl_LicenceToOperate_Labour_Import] \r\n" +
                                               " where emp_Empno = '" + Miner + "' \r\n" +
                                               " and CONVERT(VARCHAR(10),clock_time, 20) >= CONVERT(VARCHAR(10),getdate()-30, 20))) a \r\n" +
                                               " group by emp_empno \r\n " +
                                               " order by emp_empno desc ";
                _dbManMinerHist.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMinerHist.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMinerHist.ResultsTableName = "MinerHist";
                _dbManMinerHist.ExecuteInstruction();

                string MinerPF = string.Empty;

                if (_dbManMinerHist.ResultsDataTable.Rows.Count > 0)
                { MinerPF = _dbManMinerHist.ResultsDataTable.Rows[0]["emp_empno"].ToString(); }

                DataSet ReportDatasetReportCS1 = new DataSet();
                ReportDatasetReportCS1.Tables.Add(_dbManMinerHist.ResultsDataTable);

                // teamleader main
                MWDataManager.clsDataAccess _dbManTeamMain = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManTeamMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManTeamMain.SqlStatement = " declare @minerTL varchar(20) \r\n" +
                                                " set @minerTL = (select  max(clock_time) ug from [tbl_LicenceToOperate_Labour_Import]  \r\n" +
                                                " where gang_number = '" + Org + "' and wage_Description like '%TEAM%'  \r\n" +
                                                " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n" +
                                                "  and reader_code like '%UG%'  and  (reader_description like '%in%')) " +
                                                " select a.*, case when @minerTL is null then 'N' else 'Y' end as UG from (select 'a' a,'" + Org + "' oo, reader_description, max(clock_time) cc from  [dbo].[tbl_LicenceToOperate_Labour_Import]  \r\n" +
                                                " where gang_number = '" + Org + "' and wage_Description like '%TEAM%'  \r\n" +
                                                " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n" +
                                                " group by reader_description ) a   \r\n" +
                                                " order by cc ";
                _dbManTeamMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManTeamMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManTeamMain.ResultsTableName = "TeamToDay";
                _dbManTeamMain.ExecuteInstruction();

                DataSet ReportDatasetReportCS2 = new DataSet();
                ReportDatasetReportCS2.Tables.Add(_dbManTeamMain.ResultsDataTable);

                ////Last Clocking TL
                MWDataManager.clsDataAccess _dbManTLMainLastClock = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManTLMainLastClock.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManTLMainLastClock.SqlStatement = " select top(1) clock_time, READER_DESCRIPTION, emp_empno  from [tbl_LicenceToOperate_Labour_Import] \r\n " +
                                                      " where gang_number = '" + Org + "' and wage_Description like '%TEAM%' \r\n" +
                                                      " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20) order by clock_time desc ";
                _dbManTLMainLastClock.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManTLMainLastClock.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManTLMainLastClock.ResultsTableName = "TLToDayLastClock";
                _dbManTLMainLastClock.ExecuteInstruction();

                ReportDatasetReportCS.Tables.Add(_dbManTLMainLastClock.ResultsDataTable);

                // teamleader main
                MWDataManager.clsDataAccess _dbManTeamHist = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManTeamHist.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManTeamHist.SqlStatement = " select  '" + Org + "'  oo, emp_empno, max(emp_name) name  \r\n" +
                                               " ,max(Day40) Day40, max(Day39) Day39, max(Day38) Day38, max(Day37) Day37, max(Day36) Day36, max(Day35) Day35 \r\n" +
                                               " ,max(Day34) Day34, max(Day33) Day33, max(Day32) Day32, max(Day31) Day31, max(Day30) Day30, max(Day29) Day29 \r\n" +
                                               " ,max(Day28) Day28, max(Day27) Day27, max(Day26) Day26, max(Day25) Day25 \r\n" +
                                               " ,max(Day24) Day24, max(Day23) Day23, max(Day22) Day22, max(Day21) Day21, max(Day20) Day20, max(Day19) Day19 \r\n" +
                                               " ,max(Day18) Day18, max(Day17) Day17, max(Day16) Day16, max(Day15) Day15, \r\n" +
                                               " max(Day14) Day14, max(Day13) Day13, max(Day12) Day12, max(Day11) Day11, max(Day10) Day10 from (SELECT emp_empno, emp_name, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate(), 20) then day2 else '' end as Day40, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-1, 20) then day2 else '' end as Day39, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-2, 20) then day2 else '' end as Day38, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-3, 20) then day2 else '' end as Day37, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-4, 20) then day2 else '' end as Day36, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-5, 20) then day2 else '' end as Day35, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-6, 20) then day2 else '' end as Day34, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-7, 20) then day2 else '' end as Day33, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-8, 20) then day2 else '' end as Day32, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-9, 20) then day2 else '' end as Day31, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-10, 20) then day2 else '' end as Day30, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-11, 20) then day2 else '' end as Day29, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-12, 20) then day2 else '' end as Day28, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-13, 20) then day2 else '' end as Day27, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-14, 20) then day2 else '' end as Day26, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-15, 20) then day2 else '' end as Day25, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-16, 20) then day2 else '' end as Day24, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-17, 20) then day2 else '' end as Day23, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-18, 20) then day2 else '' end as Day22, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-19, 20) then day2 else '' end as Day21, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-20, 20) then day2 else '' end as Day20, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-21, 20) then day2 else '' end as Day19, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-22, 20) then day2 else '' end as Day18, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-23, 20) then day2 else '' end as Day17, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-24, 20) then day2 else '' end as Day16, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-25, 20) then day2 else '' end as Day15, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-26, 20) then day2 else '' end as Day14, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-27, 20) then day2 else '' end as Day13, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-28, 20) then day2 else '' end as Day12, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-29, 20) then day2 else '' end as Day11, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-30, 20) then day2 else '' end as Day10 \r\n" +
                                               "  from  MW_PassStageDB.dbo.ATTENDANCE \r\n" +
                                               "  where convert(varchar(20),attendance_date)+emp_empno in   \r\n" +
                                               " (select convert(varchar(20),attendance_date)+emp_empno from   \r\n" +
                                               " MW_PassStageDB.dbo.ATTENDANCE where gang_number = '" + Org + "'  ) \r\n" +
                                               " and emp_empno in (select distinct(emp_empno) a from  [tbl_LicenceToOperate_Labour_Import] \r\n" +
                                               " where gang_number = '" + Org + "' and wage_Description like '%STOPE TEAM%' \r\n" +
                                               " and CONVERT(VARCHAR(10),clock_time, 20) >= CONVERT(VARCHAR(10),getdate()-30, 20))) a \r\n" +
                                               " group by emp_empno \r\n" +
                                               " order by emp_empno desc ";
                _dbManTeamHist.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManTeamHist.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManTeamHist.ResultsTableName = "TeamHist";
                _dbManTeamHist.ExecuteInstruction();

                DataSet ReportDatasetReportCS3 = new DataSet();
                ReportDatasetReportCS3.Tables.Add(_dbManTeamHist.ResultsDataTable);

                string Team = string.Empty;

                if (_dbManTeamHist.ResultsDataTable.Rows.Count > 0)
                { Team = _dbManTeamHist.ResultsDataTable.Rows[0]["emp_empno"].ToString(); }

                string UserPath = @"\\\\afzavrdat01\\vr\\LMSNew\\Images";
                string MinPic = string.Empty;
                string TLPic = string.Empty;
                string Pic = string.Empty;
                string Min = string.Empty;
                string TL = string.Empty;

                if (_dbManMinerMainLastClock.ResultsDataTable.Rows.Count > 0)
                {
                    Pic = _dbManMinerMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                    MinPic = UserPath + @"\" + Pic + ".jpg";
                    Min = _dbManMinerMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                }

                if (_dbManMinerHist.ResultsDataTable.Rows.Count > 0)
                {
                    Pic = _dbManMinerHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                    MinPic = UserPath + @"\" + Pic + ".jpg";
                    Min = _dbManMinerHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                }

                if (_dbManTLMainLastClock.ResultsDataTable.Rows.Count > 0)
                {
                    Pic = _dbManTLMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                    TLPic = UserPath + @"\" + Pic + ".jpg";
                    TL = _dbManTLMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                }

                if (_dbManTeamHist.ResultsDataTable.Rows.Count > 0)
                {
                    Pic = _dbManTeamHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                    TLPic = UserPath + @"\" + Pic + ".jpg";
                    TL = _dbManTeamHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                }

                MWDataManager.clsDataAccess _dbManGenDataCS = new MWDataManager.clsDataAccess();
                _dbManGenDataCS.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManGenDataCS.SqlStatement = " select *, b.employeename mmname, c.employeename tlname from ( select '" + MinPic + "' minerpic, '" + TLPic + "' tlpic, '" + ProductionGlobalTSysSettings._Banner + "' mine,  '" + Team + "' TeamCompNo,  '" + MinerPF + "' MinerCompNo , '" + WP + "' wp, '" + Min + "' mm, '" + TL + "' tt \r\n";
                _dbManGenDataCS.SqlStatement = _dbManGenDataCS.SqlStatement + "  ) a  left outer join employeeall b on a.mm = b.employeeno   left outer join employeeall c on a.tt = c.employeeno ";
                _dbManGenDataCS.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGenDataCS.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGenDataCS.ResultsTableName = "GenDataCS";
                _dbManGenDataCS.ExecuteInstruction();

                DataSet ReportDatasetReportCS4 = new DataSet();
                ReportDatasetReportCS4.Tables.Add(_dbManGenDataCS.ResultsDataTable);

                //////////////4.Workplace Risk Profiles/////////////////
                MWDataManager.clsDataAccess _dbManWPST2RP = new MWDataManager.clsDataAccess();
                _dbManWPST2RP.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManWPST2RP.SqlStatement = " select top (20) * from ( select 'z' bb, ActionStatus, action, datesubmitted, datediff(day,datesubmitted,getdate()) ss, Hazard, DateActionClosed   from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]  \r\n" +
                                            " where workplace = '" + Workplace + "' and disciplinename = 'RMS'  \r\n" +
                                            " and datesubmitted = (  \r\n" +
                                            " select max(datesubmitted) dd from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]    \r\n" +
                                            " where workplace = '" + Workplace + "' and disciplinename = 'RMS') group by Action, ActionStatus, DateSubmitted,Hazard, DateActionClosed   \r\n" +
                                            " union all \r\n" +
                                            " select 'a' , '', '', null, '', '' , '' \r\n" +
                                            " union all \r\n" +
                                            " select 'b ', '', '', null, '', '' , '' \r\n" +
                                            " union all \r\n" +
                                            " select 'c  ' , '', '', null, '', '' , '' \r\n" +
                                            " union \r\n" +
                                            " select 'd   ' , '', '', null, '', '' , '' \r\n" +
                                            " union \r\n" +
                                            " select 'e    ' , '', '', null, '', '' , '' \r\n" +
                                            " union \r\n" +
                                            " select 'f     ' , '', '', null, '', '' , '' \r\n" +
                                            " union \r\n" +
                                            " select 'g     ' , '', '', null, '', '' , '' \r\n" +
                                            " union \r\n" +
                                            " select 'h     ' , '', '', null, '', '' , '' \r\n" +
                                            " union \r\n" +
                                            " select 'i     ' , '', '', null, '', '' , '' \r\n" +
                                            " union \r\n" +
                                            " select 'j     ' , '', '', null, '', '' , '' \r\n" +
                                            " )a \r\n" +
                                            " order  by bb ";
                _dbManWPST2RP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPST2RP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPST2RP.ResultsTableName = "HazardsRP";
                _dbManWPST2RP.ExecuteInstruction();

                DataSet dsABS1 = new DataSet();
                dsABS1.Tables.Add(_dbManWPST2RP.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManVentDetailRP = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManVentDetailRP.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManVentDetailRP.SqlStatement = "exec sp_LicenceToOperate_TempReadingWpRiskProf '" + Workplace + "'";
                _dbManVentDetailRP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVentDetailRP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVentDetailRP.ResultsTableName = "TempDetailsRP";
                _dbManVentDetailRP.ExecuteInstruction();

                DataSet ReportDatasetReportRP1 = new DataSet();
                ReportDatasetReportRP1.Tables.Add(_dbManVentDetailRP.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManHeaderRP = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManHeaderRP.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManHeaderRP.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
                _dbManHeaderRP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManHeaderRP.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManHeaderRP.ResultsTableName = "HeaderRP";
                _dbManHeaderRP.ExecuteInstruction();

                DataSet ReportDatasetReportRP3 = new DataSet();
                ReportDatasetReportRP3.Tables.Add(_dbManHeaderRP.ResultsDataTable);

                ///////////////Workplace Deviations///////////////////
                MWDataManager.clsDataAccess _dbManVentDetailWPD = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManVentDetailWPD.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManVentDetailWPD.SqlStatement = "exec [sp_LicenceToOperate_3)WorkplaceDeviations] '" + Workplace + "'";
                _dbManVentDetailWPD.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVentDetailWPD.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVentDetailWPD.ResultsTableName = "TempDetailsWPD";
                _dbManVentDetailWPD.ExecuteInstruction();

                DataSet ReportDatasetReportWPD1 = new DataSet();
                ReportDatasetReportWPD1.Tables.Add(_dbManVentDetailWPD.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManVentDetailGraphWPD = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManVentDetailGraphWPD.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManVentDetailGraphWPD.SqlStatement = "exec [sp_LicenceToOperate_3)WorkplaceDeviationsGraph] '" + Workplace + "'";

                _dbManVentDetailGraphWPD.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVentDetailGraphWPD.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVentDetailGraphWPD.ResultsTableName = "TempDetailsGraphWPD";
                _dbManVentDetailGraphWPD.ExecuteInstruction();

                DataSet ReportDatasetReportWPD2 = new DataSet();
                ReportDatasetReportWPD2.Tables.Add(_dbManVentDetailGraphWPD.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManHeaderWPD = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManHeaderWPD.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManHeaderWPD.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
                _dbManHeaderWPD.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManHeaderWPD.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManHeaderWPD.ResultsTableName = "HeaderWPD";
                _dbManHeaderWPD.ExecuteInstruction();

                DataSet ReportDatasetReportWPD3 = new DataSet();
                ReportDatasetReportWPD3.Tables.Add(_dbManHeaderWPD.ResultsDataTable);

                //////////////////Major Hazards / Critical Controls Deviations//////////////////
                MWDataManager.clsDataAccess _dbManVentDetailMH = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManVentDetailMH.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManVentDetailMH.SqlStatement = "exec [dbo].[sp_LicenceToOperate_MajHazards] '" + Workplace + "'";
                _dbManVentDetailMH.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVentDetailMH.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVentDetailMH.ResultsTableName = "TempDetailsMH";
                _dbManVentDetailMH.ExecuteInstruction();

                DataSet ReportDatasetReportMH1 = new DataSet();
                ReportDatasetReportMH1.Tables.Add(_dbManVentDetailMH.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManHeaderMH = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManHeaderMH.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManHeaderMH.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
                _dbManHeaderMH.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManHeaderMH.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManHeaderMH.ResultsTableName = "HeaderMH";
                _dbManHeaderMH.ExecuteInstruction();

                DataSet ReportDatasetReportMH3 = new DataSet();
                ReportDatasetReportMH3.Tables.Add(_dbManHeaderMH.ResultsDataTable);

                //////////////////////////Workplace Environmental Conditions//////////////////////////////////////
                MWDataManager.clsDataAccess _dbManVentDetailWEC = new MWDataManager.clsDataAccess();
                _dbManVentDetailWEC.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManVentDetailWEC.SqlStatement = "exec [dbo].[sp_LicenceToOperate_TempReading] '" + Workplace + "'";
                _dbManVentDetailWEC.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVentDetailWEC.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVentDetailWEC.ResultsTableName = "TempDetailsWEC";
                _dbManVentDetailWEC.ExecuteInstruction();

                DataSet ReportDatasetReportWEC1 = new DataSet();
                ReportDatasetReportWEC1.Tables.Add(_dbManVentDetailWEC.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManVentPPWEC = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManVentPPWEC.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManVentPPWEC.SqlStatement = "select * from [ZAWW2K16SQL01].[AngloQlikView].[dbo].[vw_PaperlessActionManagerPrint_AllActions] \r\n";
                _dbManVentPPWEC.SqlStatement = _dbManVentPPWEC.SqlStatement + " where datesubmitted > getdate()-30 and stepname like '%tempe%' and workplace = '" + Workplace + "' and actionstatus not in ( 'Not A Deficiency', 'Not Applicable' )    ";
                _dbManVentPPWEC.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVentPPWEC.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVentPPWEC.ResultsTableName = "PaperlessWEC";
                _dbManVentPPWEC.ExecuteInstruction();

                DataSet ReportDatasetReportWEC2 = new DataSet();
                ReportDatasetReportWEC2.Tables.Add(_dbManVentPPWEC.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManHeaderWEC = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManHeaderWEC.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManHeaderWEC.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
                _dbManHeaderWEC.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManHeaderWEC.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManHeaderWEC.ResultsTableName = "HeaderWEC";
                _dbManHeaderWEC.ExecuteInstruction();

                DataSet ReportDatasetReportWEC3 = new DataSet();
                ReportDatasetReportWEC3.Tables.Add(_dbManHeaderWEC.ResultsDataTable);

                //////////////////Production////////////////////////////
                MWDataManager.clsDataAccess _dbManWPDetailProd = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManWPDetailProd.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManWPDetailProd.SqlStatement = " select '" + ProductionGlobalTSysSettings._Banner + "' Baner, '" + TUserInfo.UserID + "' username, *, (select distsupred from tbl_sysset) distsupred, (select distsuporange from tbl_sysset) distsupOrange, (select distswpred from tbl_sysset) distswpred, (select distswporange from tbl_sysset) distswpOrange from tbl_Workplace where description = '" + Workplace + "' ";
                _dbManWPDetailProd.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPDetailProd.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPDetailProd.ResultsTableName = "WorkplaceDetailsProd";
                _dbManWPDetailProd.ExecuteInstruction();

                DataSet ReportDatasetReportProd = new DataSet();
                ReportDatasetReportProd.Tables.Add(_dbManWPDetailProd.ResultsDataTable);

                if (_dbManWPDetailProd.ResultsDataTable.Rows.Count >= 1)
                {
                    WPID = _dbManWPDetailProd.ResultsDataTable.Rows[0]["workplaceid"].ToString();
                }

                MWDataManager.clsDataAccess _dbManPSDetailProd = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618 // Type or member is obsolete
                _dbManPSDetailProd.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618 // Type or member is obsolete
                _dbManPSDetailProd.SqlStatement = " select p.description, count(description) aa from tbl_ProblemBook pb, tbl_Problem p  \r\n" +
                                                  " where pb.problemid = p.problemid and  workplaceid = '" + WPID + "' and calendardate > getdate() - 30 \r\n" +
                                                  " group by p.description ";
                _dbManPSDetailProd.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPSDetailProd.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPSDetailProd.ResultsTableName = "Planned Stoppage Graph";
                _dbManPSDetailProd.ExecuteInstruction();

                DataSet ReportDatasetReportPSProd = new DataSet();
                ReportDatasetReportPSProd.Tables.Add(_dbManPSDetailProd.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManProbDetailProd = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManProbDetailProd.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManProbDetailProd.SqlStatement = " select c.description, count(description) aa  from tbl_planning p, mw_passstagedb.dbo.Central_Code_Cycle c  \r\n" +
                                                    " where p.bookprob = c.code  COLLATE Latin1_General_CI_AS and  workplaceid = '" + WPID + "' and calendardate > getdate() - 30 \r\n" +
                                                    " group by c.description ";
                _dbManProbDetailProd.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManProbDetailProd.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManProbDetailProd.ResultsTableName = "Problem GraphProd";
                _dbManProbDetailProd.ExecuteInstruction();

                DataSet ReportDatasetReportProbProd = new DataSet();
                ReportDatasetReportProbProd.Tables.Add(_dbManProbDetailProd.ResultsDataTable);

                /////New////////
                MWDataManager.clsDataAccess _dbManVentDetailProd = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManVentDetailProd.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManVentDetailProd.SqlStatement = "exec [dbo].[sp_LicenceToOperate_TempReadingProd] '" + Workplace + "'";
                _dbManVentDetailProd.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVentDetailProd.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVentDetailProd.ResultsTableName = "TempDetailsProd";
                _dbManVentDetailProd.ExecuteInstruction();

                DataSet ReportDatasetReportProd1 = new DataSet();
                ReportDatasetReportProd1.Tables.Add(_dbManVentDetailProd.ResultsDataTable);

                ////////////////////////////Rock Engineer Dept.////////////////////////////
                MWDataManager.clsDataAccess _dbManRMDetailRE = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManRMDetailRE.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManRMDetailRE.SqlStatement = "select * from [dbo].[tbl_RockMechInspection] a, tbl_Workplace w " +
                                                "where a.workplace = w.description and workplace  = '" + Workplace + "' " +
                                                "and captdate = (select  " +
                                                "max(captdate) from [dbo].[tbl_RockMechInspection] where workplace  = '" + Workplace + "' ) ";
                _dbManRMDetailRE.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManRMDetailRE.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManRMDetailRE.ResultsTableName = "RMDetailsRE";
                _dbManRMDetailRE.ExecuteInstruction();

                string WPLbl = "00";
                string WkLbl = "00";
                string WkLbl2 = "00";
                string RRLbl = "0";

                if (_dbManRMDetailRE.ResultsDataTable.Rows.Count > 0)
                {
                    WPLbl = Workplace;
                    WkLbl = _dbManRMDetailRE.ResultsDataTable.Rows[0]["captweek"].ToString();
                    WkLbl2 = _dbManRMDetailRE.ResultsDataTable.Rows[0]["captweek"].ToString();
                    RRLbl = _dbManRMDetailRE.ResultsDataTable.Rows[0]["riskrating"].ToString();
                }

                MWDataManager.clsDataAccess _dbManWPST2RE = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManWPST2RE.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManWPST2RE.SqlStatement = " select top (20) * from ( select 'z' bb, ActionStatus, action, datesubmitted, datediff(day,datesubmitted,getdate()) ss  from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]  \r\n" +
                                             " where workplace = '" + WPLbl + "' and disciplinename = 'RMS' and hazard = 'A'  \r\n" +
                                             " and datesubmitted = (  \r\n" +
                                             " select max(datesubmitted) dd from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]    \r\n" +
                                             " where workplace = '" + WPLbl + "' and disciplinename = 'RMS' and hazard = 'A') group by Action, ActionStatus, DateSubmitted  \r\n" +
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
                                             " order  by bb";
                _dbManWPST2RE.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPST2RE.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPST2RE.ResultsTableName = "Table2RE";
                _dbManWPST2RE.ExecuteInstruction();

                DataSet dsABS1RE = new DataSet();
                dsABS1RE.Tables.Add(_dbManWPST2RE.ResultsDataTable);

                string Path = Application.StartupPath + "\\" + "Neil.bmp";

                MWDataManager.clsDataAccess _dbManRE = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManRE.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManRE.SqlStatement = " select '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + RRLbl + "' rr,  * from [dbo].[tbl_RockMechInspection] where workplace = '" + WPLbl + "' and captweek = convert(decimal(18,0),'" + WkLbl + "') ";
                _dbManRE.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManRE.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManRE.ResultsTableName = "DevSummary";
                _dbManRE.ExecuteInstruction();

                Image TabletImage = null;

                if (_dbManRE.ResultsDataTable.Rows.Count > 0)
                {
                    if (_dbManRE.ResultsDataTable.Rows[0]["WPASuser"].ToString() == "Tablet")
                    {
                        if (_dbManRE.ResultsDataTable.Rows[0]["picture"].ToString() != string.Empty)
                        {
                            TabletImage = Base64ToImage(_dbManRE.ResultsDataTable.Rows[0]["picture"].ToString(), WPLbl);
                        }
                        TabletImage.Save(Application.StartupPath + "\\" + "Neil.bmp");
                    }
                }

                MWDataManager.clsDataAccess _dbManImageRE = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManImageRE.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManImageRE.SqlStatement = " Select picture, '" + Path + "' pp from [dbo].[tbl_RockMechInspection] where workplace = '" + WPLbl + "' and captweek = '" + WkLbl + "' ";
                _dbManImageRE.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManImageRE.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManImageRE.ResultsTableName = "ImageRE";
                _dbManImageRE.ExecuteInstruction();

                MWDataManager.clsDataAccess _dbManWPST21 = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManWPST21.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManWPST21.SqlStatement = "select * from  tbl_LicenceToOperate_Seismic where wpdescription = '" + WPLbl + "' order by thedate desc";
                _dbManWPST21.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPST21.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPST21.ResultsTableName = "Graphree";
                _dbManWPST21.ExecuteInstruction();

                DataSet dsABS111 = new DataSet();
                dsABS111.Tables.Add(_dbManWPST21.ResultsDataTable);

                DataSet ReportDatasetReportRE = new DataSet();
                ReportDatasetReportRE.Tables.Add(_dbManRE.ResultsDataTable);

                DataSet ReportDatasetReportImageRE = new DataSet();
                ReportDatasetReportImageRE.Tables.Add(_dbManImageRE.ResultsDataTable);

                ////////////////////////////GeoScience Dept.////////////////////////////////////////////////////////
                MWDataManager.clsDataAccess _dbManRMDetailG = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManRMDetailG.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManRMDetailG.SqlStatement = " select * from [dbo].[tbl_GeoScienceInspection] a, tbl_Workplace w \r\n" +
                                               " where a.workplace = w.description and workplace  = '" + Workplace + "' \r\n" +
                                               " and captdate = (select max(captdate) from [dbo].[tbl_GeoScienceInspection] where workplace  = '" + Workplace + "' ) ";
                _dbManRMDetailG.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManRMDetailG.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManRMDetailG.ResultsTableName = "RMDetailsG";
                _dbManRMDetailG.ExecuteInstruction();

                string WPLblG = "0";
                string WkLblG = "0";
                string WkLbl2G = "0";
                string RRLblG = "0";

                if (_dbManRMDetailG.ResultsDataTable.Rows.Count > 0)
                {
                    WPLblG = Workplace;
                    WkLblG = _dbManRMDetailG.ResultsDataTable.Rows[0]["actweek"].ToString();
                    WkLbl2G = _dbManRMDetailG.ResultsDataTable.Rows[0]["captweek"].ToString();
                    RRLblG = _dbManRMDetailG.ResultsDataTable.Rows[0]["riskrating"].ToString();
                }

                MWDataManager.clsDataAccess _dbManWPST2G = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManWPST2G.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManWPST2G.SqlStatement = " select top (20) * from ( select 'z' bb, ActionStatus, action, datesubmitted, datediff(day,datesubmitted,getdate()) ss  from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]  \r\n" +
                                            " where workplace = '" + WPLblG + "' and disciplinename = 'RMS' and hazard = 'A'  \r\n" +
                                            " and datesubmitted = (  \r\n" +
                                            " select max(datesubmitted) dd from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]    \r\n" +
                                            " where workplace = '" + WPLblG + "' and disciplinename = 'RMS' and hazard = 'A') group by Action, ActionStatus, DateSubmitted  \r\n" +
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
                                            " order  by bb ";
                _dbManWPST2G.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPST2G.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPST2G.ResultsTableName = "Table2G";
                _dbManWPST2G.ExecuteInstruction();

                DataSet dsABS1G = new DataSet();
                dsABS1G.Tables.Add(_dbManWPST2G.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManG = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManG.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManG.SqlStatement = " select '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + RRLblG + "' rr,  * from [dbo].[tbl_GeoScienceInspection] where workplace = '" + WPLblG + "' and captweek = '" + WkLbl2G + "' ";
                _dbManG.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManG.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManG.ResultsTableName = "DevSummaryG";
                _dbManG.ExecuteInstruction();

                MWDataManager.clsDataAccess _dbManImageG = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManImageG.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManImageG.SqlStatement = " Select picture from [dbo].[tbl_GeoScienceInspection] where workplace = '" + WPLblG + "' and captweek = '" + WkLbl2G + "' ";
                _dbManImageG.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManImageG.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManImageG.ResultsTableName = "ImageG";
                _dbManImageG.ExecuteInstruction();

                DataSet ReportDatasetReportG = new DataSet();
                ReportDatasetReportG.Tables.Add(_dbManG.ResultsDataTable);

                DataSet ReportDatasetReportImageG = new DataSet();
                ReportDatasetReportImageG.Tables.Add(_dbManImageG.ResultsDataTable);

                ///CompA
                theReport.RegisterData(ReportDatasetCompADetail);
                theReport.RegisterData(ReportDatasetCompADetailA);
                theReport.RegisterData(ReportDatasetCompADetailB);

                ///CallCentreApp
                theReport.RegisterData(ReportDatasetReport2NS);
                theReport.RegisterData(ReportDatasetReport1);
                theReport.RegisterData(ReportDatasetReport2);
                theReport.RegisterData(ReportDatasetReport3);
                theReport.RegisterData(ReportDatasetReport4);
                theReport.RegisterData(ReportDatasetReport5);

                ///CriticalSkills
                theReport.RegisterData(ReportDatasetReportCS);
                theReport.RegisterData(ReportDatasetReportCS1);
                theReport.RegisterData(ReportDatasetReportCS2);
                theReport.RegisterData(ReportDatasetReportCS3);
                theReport.RegisterData(ReportDatasetReportCS4);

                ///Risk Profiles
                theReport.RegisterData(dsABS1);
                theReport.RegisterData(ReportDatasetReportRP1);
                theReport.RegisterData(ReportDatasetReportRP3);

                ///Wp Deviations
                theReport.RegisterData(ReportDatasetReportWPD3);
                theReport.RegisterData(ReportDatasetReportWPD2);
                theReport.RegisterData(ReportDatasetReportWPD1);

                //Major Hazards
                theReport.RegisterData(ReportDatasetReportMH1);
                theReport.RegisterData(ReportDatasetReportMH3);

                ////WP Environmental Conditions
                theReport.RegisterData(ReportDatasetReportWEC1);
                theReport.RegisterData(ReportDatasetReportWEC2);
                theReport.RegisterData(ReportDatasetReportWEC3);

                /////////////Production
                theReport.RegisterData(ReportDatasetReportProd1);
                theReport.RegisterData(ReportDatasetReportProd);
                theReport.RegisterData(ReportDatasetReportPSProd);
                theReport.RegisterData(ReportDatasetReportProbProd);

                ///Rock Engineering
                theReport.RegisterData(ReportDatasetReportImageRE);
                theReport.RegisterData(ReportDatasetReportRE);
                theReport.RegisterData(dsABS1RE);
                theReport.RegisterData(dsABS111);

                ///GeoInp
                theReport.RegisterData(dsABS1G);
                theReport.RegisterData(ReportDatasetReportG);
                theReport.RegisterData(ReportDatasetReportImageG);

                theReport.Load("AllReports.frx");

                //CallCentreApp.Design();
                theReport.Show();
            }

            if (SelectedField == "Production")
            {
                MWDataManager.clsDataAccess _dbManWPDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManWPDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                if (ProductionGlobalTSysSettings._Banner == "Moab Khotsong")
                {
                    if (Workplace.Substring(0, 2) == "61" || Workplace.Substring(0, 2) == "64" || Workplace.Substring(0, 2) == "68" || Workplace.Substring(0, 2) == "70" || Workplace.Substring(0, 2) == "73" || Workplace.Substring(0, 2) == "74" || Workplace.Substring(0, 2) == "76")
                    { _dbManWPDetail.SqlStatement = " select 'Sweeps' SweepLbl, '" + ProductionGlobalTSysSettings._Banner + "' Baner, '" + TUserInfo.UserID + "' username, *, (select distsupred1 from tbl_sysset) distsupred, (select distsuporange1 from tbl_sysset) distsupOrange, (select distswpred1 from tbl_sysset) distswpred, (select distswporange1 from tbl_sysset) distswpOrange from tbl_Workplace where description = '" + Workplace + "' "; }
                    else
                    { _dbManWPDetail.SqlStatement = " select 'Backfill' SweepLbl,'" + ProductionGlobalTSysSettings._Banner + "' Baner, '" + TUserInfo.UserID + "' username, *, (select distsupred from tbl_sysset) distsupred, (select distsuporange from tbl_sysset) distsupOrange, (select distswpred from tbl_sysset) distswpred, (select distswporange from tbl_sysset) distswpOrange from tbl_Workplace where description = '" + Workplace + "' "; }
                }
                else
                {
                    if (ProductionGlobalTSysSettings._Banner == "Mponeng")
                    { _dbManWPDetail.SqlStatement = " select 'Backfill' SweepLbl,'" + ProductionGlobalTSysSettings._Banner + "' Baner, '" + TUserInfo.UserID + "' username, *, (select distsupred from tbl_sysset) distsupred, (select distsuporange from tbl_sysset) distsupOrange, (select distswpred from tbl_sysset) distswpred, (select distswporange from tbl_sysset) distswpOrange from tbl_Workplace where description = '" + Workplace + "' "; }
                    else
                    { _dbManWPDetail.SqlStatement = " select 'Sweeps' SweepLbl,'" + ProductionGlobalTSysSettings._Banner + "' Baner, '" + TUserInfo.UserID + "' username, *, (select distsupred from tbl_sysset) distsupred, (select distsuporange from tbl_sysset) distsupOrange, (select distswpred from tbl_sysset) distswpred, (select distswporange from tbl_sysset) distswpOrange from tbl_Workplace where description = '" + Workplace + "' "; }
                }
                _dbManWPDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPDetail.ResultsTableName = "WorkplaceDetails";
                _dbManWPDetail.ExecuteInstruction();

                DataSet ReportDatasetReport = new DataSet();
                ReportDatasetReport.Tables.Add(_dbManWPDetail.ResultsDataTable);

                if (_dbManWPDetail.ResultsDataTable.Rows.Count >= 1)
                {
                    WPID = _dbManWPDetail.ResultsDataTable.Rows[0]["workplaceid"].ToString();
                }

                MWDataManager.clsDataAccess _dbManPSDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManPSDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManPSDetail.SqlStatement = " select p.description, count(description) aa from tbl_ProblemBook pb, tbl_Problem p  \r\n" +
                                              " where pb.problemid = p.problemid and  workplaceid = '" + WPID + "' and calendardate > getdate() - 30 \r\n" +
                                              " group by p.description ";
                _dbManPSDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPSDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPSDetail.ResultsTableName = "Planned Stoppage Graph";
                _dbManPSDetail.ExecuteInstruction();

                DataSet ReportDatasetReportPS = new DataSet();
                ReportDatasetReportPS.Tables.Add(_dbManPSDetail.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManProbDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManProbDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManProbDetail.SqlStatement = " select c.description, count(description) aa  from tbl_planning p, mw_passstagedb.dbo.Central_Code_Cycle c  \r\n " +
                                                " where p.bookprob = c.code  COLLATE Latin1_General_CI_AS and  workplaceid = '" + WPID + "' and calendardate > getdate() - 30 \r\n" +
                                                " group by c.description ";
                _dbManProbDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManProbDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManProbDetail.ResultsTableName = "Problem Graph";
                _dbManProbDetail.ExecuteInstruction();

                DataSet ReportDatasetReportProb = new DataSet();
                ReportDatasetReportProb.Tables.Add(_dbManProbDetail.ResultsDataTable);

                /////New////////
                MWDataManager.clsDataAccess _dbManVentDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManVentDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManVentDetail.SqlStatement = "exec [dbo].[sp_LicenceToOperate_TempReadingProd] '" + Workplace + "'";
                _dbManVentDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVentDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVentDetail.ResultsTableName = "TempDetails";
                _dbManVentDetail.ExecuteInstruction();

                DataSet ReportDatasetReport1 = new DataSet();
                ReportDatasetReport1.Tables.Add(_dbManVentDetail.ResultsDataTable);

                theReport.RegisterData(ReportDatasetReport1);
                theReport.RegisterData(ReportDatasetReport);
                theReport.RegisterData(ReportDatasetReportPS);
                theReport.RegisterData(ReportDatasetReportProb);



                theReport.Load("WPLicToOpProd.frx");

                theReport.Show();
            }

            //if (SelectedField == "Comp A")
            //{
            //    MWDataManager.clsDataAccess _dbManVentDetail = new MWDataManager.clsDataAccess();
            //    _dbManVentDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManVentDetail.SqlStatement = "exec [dbo].sp_LicenceToOperate_CompA '" + Workplace + "'";
            //    _dbManVentDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManVentDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManVentDetail.ResultsTableName = "TempDetails";
            //    _dbManVentDetail.ExecuteInstruction();

            //    DataSet ReportDatasetReport1 = new DataSet();
            //    ReportDatasetReport1.Tables.Add(_dbManVentDetail.ResultsDataTable);                

            //    MWDataManager.clsDataAccess _dbManVentPP = new MWDataManager.clsDataAccess();
            //    _dbManVentPP.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManVentPP.SqlStatement = "select * from [ZAWW2K16SQL01].[AngloQlikView].[dbo].[vw_PaperlessActionManagerPrint_AllActions] ";
            //    _dbManVentPP.SqlStatement = _dbManVentPP.SqlStatement + " where datesubmitted > getdate()-30 and taskname like '%CompA%' and workplace = '" + Workplace + "' and actionstatus <> 'Not A Deficiency'and actionstatus <> 'Not Applicable'  ";
            //    _dbManVentPP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManVentPP.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManVentPP.ResultsTableName = "Paperless";
            //    _dbManVentPP.ExecuteInstruction();

            //    DataSet ReportDatasetReport2 = new DataSet();
            //    ReportDatasetReport2.Tables.Add(_dbManVentPP.ResultsDataTable);               

            //    MWDataManager.clsDataAccess _dbManHeader = new MWDataManager.clsDataAccess();
            //    _dbManHeader.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManHeader.SqlStatement = "select '" + ProductionAmplatsGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
            //    _dbManHeader.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManHeader.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManHeader.ResultsTableName = "Header";
            //    _dbManHeader.ExecuteInstruction();

            //    DataSet ReportDatasetReport3 = new DataSet();
            //    ReportDatasetReport3.Tables.Add(_dbManHeader.ResultsDataTable);

            //    TempRep.RegisterData(ReportDatasetReport1);
            //    TempRep.RegisterData(ReportDatasetReport2);
            //    TempRep.RegisterData(ReportDatasetReport3);
            //    TempRep.Load("WPLicToOpCompA.frx");
            //    //TempRep.Design();
            //    TempRep.Show();
            //}

            //if (SelectedField == "Risk Profiles")
            //{
            //    ////Hazards
            //    MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
            //    _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManWPST2.SqlStatement = " select top (20) *,   (select  min(datediff(day,datesubmitted,getdate())) dd from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions] \r\n" +
            //                               " where workplace = '" + Workplace + "'  \r\n" +
            //                               " and (disciplinename + taskname like 'RMSSafe%' or taskname like 'MP-MPI%') and actionstatus <> 'Not A Deficiency' \r\n" +
            //                               " ) dd  from ( select 'z' bb, ActionStatus, action, datesubmitted, datediff(day,datesubmitted,getdate()) ss, Hazard, DateActionClosed   from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]  \r\n" +
            //                               " where workplace = '" + Workplace + "' \r\n" +
            //                               " and (disciplinename + taskname like 'RMSSafe%' or taskname like 'MP-MPI%') and actionstatus <> 'Not A Deficiency' \r\n" +
            //                               " and datesubmitted = (  \r\n" +
            //                               " select max(datesubmitted) dd from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions] \r\n" +
            //                               " where workplace = '" + Workplace + "' " +
            //                               " and (disciplinename + taskname like 'RMSSafe%' or taskname like 'MP-MPI%') and actionstatus <> 'Not A Deficiency' \r\n" +
            //                               " ) group by Action, ActionStatus, DateSubmitted,Hazard, DateActionClosed \r\n" +
            //                               " union all \r\n" +
            //                               " select 'a' , '', '', null, '', '' , '' \r\n" +
            //                               " union all \r\n" +
            //                               " select 'b ', '', '', null, '', '' , '' \r\n" +
            //                               " union all \r\n" +
            //                               " select 'c  ' , '', '', null, '', '' , '' \r\n" +
            //                               " union \r\n" +
            //                               " select 'd   ' , '', '', null, '', '' , '' \r\n" +
            //                               " union \r\n" +
            //                               " select 'e    ' , '', '', null, '', '' , '' \r\n" +
            //                               " union \r\n" +
            //                               " select 'f     ' , '', '', null, '', '' , '' \r\n" +
            //                               " union \r\n" +
            //                               " select 'g     ' , '', '', null, '', '' , '' \r\n" +
            //                               " union \r\n" +
            //                               " select 'h     ' , '', '', null, '', '' , '' \r\n" +
            //                               " union \r\n" +
            //                               " select 'i     ' , '', '', null, '', '' , '' \r\n" +
            //                               " union \r\n" +
            //                               " select 'j     ' , '', '', null, '', '' , '' \r\n" +
            //                               " )a order  by bb \r\n";
            //    _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManWPST2.ResultsTableName = "Hazards";
            //    _dbManWPST2.ExecuteInstruction();

            //    DataSet dsABS1 = new DataSet();
            //    dsABS1.Tables.Add(_dbManWPST2.ResultsDataTable);               

            //    MWDataManager.clsDataAccess _dbManVentDetail = new MWDataManager.clsDataAccess();
            //    _dbManVentDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManVentDetail.SqlStatement = "exec sp_LicenceToOperate_TempReadingWpRiskProf '" + Workplace + "'";

            //    _dbManVentDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManVentDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManVentDetail.ResultsTableName = "TempDetails";
            //    _dbManVentDetail.ExecuteInstruction();

            //    DataSet ReportDatasetReport1 = new DataSet();
            //    ReportDatasetReport1.Tables.Add(_dbManVentDetail.ResultsDataTable);              

            //    MWDataManager.clsDataAccess _dbManHeader = new MWDataManager.clsDataAccess();
            //    _dbManHeader.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManHeader.SqlStatement = "select '" + ProductionAmplatsGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
            //    _dbManHeader.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManHeader.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManHeader.ResultsTableName = "Header";
            //    _dbManHeader.ExecuteInstruction();

            //    DataSet ReportDatasetReport3 = new DataSet();
            //    ReportDatasetReport3.Tables.Add(_dbManHeader.ResultsDataTable);

            //    TempRep.RegisterData(dsABS1);
            //    TempRep.RegisterData(ReportDatasetReport1);
            //    TempRep.RegisterData(ReportDatasetReport3);
            //    TempRep.Load("WPLicToOpWPRiskProf.frx");
            //    // TempRep.Design();
            //    TempRep.Show();
            //}

            if (SelectedField == "Major Hazards")
            {
                MWDataManager.clsDataAccess _dbManVentDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManVentDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManVentDetail.SqlStatement = "exec [dbo].[sp_LicenceToOperate_MajHazards] '" + Workplace + "'";
                _dbManVentDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManVentDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManVentDetail.ResultsTableName = "TempDetails";
                _dbManVentDetail.ExecuteInstruction();

                DataSet ReportDatasetReport1 = new DataSet();
                ReportDatasetReport1.Tables.Add(_dbManVentDetail.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManHeader = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManHeader.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManHeader.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
                _dbManHeader.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManHeader.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManHeader.ResultsTableName = "Header";
                _dbManHeader.ExecuteInstruction();

                DataSet ReportDatasetReport3 = new DataSet();
                ReportDatasetReport3.Tables.Add(_dbManHeader.ResultsDataTable);

                theReport.RegisterData(ReportDatasetReport1);
                theReport.RegisterData(ReportDatasetReport3);
                theReport.Load("WPLicToOpMajHazard.frx");
                // MajHazRep.Design();
                theReport.Show();
            }

            //if (SelectedField == "Workplace Deviations")
            //{
            //    MWDataManager.clsDataAccess _dbManVentDetail = new MWDataManager.clsDataAccess();
            //    _dbManVentDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManVentDetail.SqlStatement = "exec [sp_LicenceToOperate_3)WorkplaceDeviations] '" + Workplace + "'";
            //    _dbManVentDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManVentDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManVentDetail.ResultsTableName = "TempDetails";
            //    _dbManVentDetail.ExecuteInstruction();

            //    DataSet ReportDatasetReport1 = new DataSet();
            //    ReportDatasetReport1.Tables.Add(_dbManVentDetail.ResultsDataTable);     

            //    MWDataManager.clsDataAccess _dbManVentDetailGraph = new MWDataManager.clsDataAccess();
            //    _dbManVentDetailGraph.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManVentDetailGraph.SqlStatement = "exec [sp_LicenceToOperate_3)WorkplaceDeviationsGraph] '" + Workplace + "'";
            //    _dbManVentDetailGraph.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManVentDetailGraph.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManVentDetailGraph.ResultsTableName = "TempDetailsGraph";
            //    _dbManVentDetailGraph.ExecuteInstruction();

            //    DataSet ReportDatasetReport2 = new DataSet();
            //    ReportDatasetReport2.Tables.Add(_dbManVentDetailGraph.ResultsDataTable);                

            //    MWDataManager.clsDataAccess _dbManHeader = new MWDataManager.clsDataAccess();
            //    _dbManHeader.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManHeader.SqlStatement = "select '" + ProductionAmplatsGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
            //    _dbManHeader.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManHeader.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManHeader.ResultsTableName = "Header";
            //    _dbManHeader.ExecuteInstruction();

            //    DataSet ReportDatasetReport3 = new DataSet();
            //    ReportDatasetReport3.Tables.Add(_dbManHeader.ResultsDataTable);

            //    WPDevRep.RegisterData(ReportDatasetReport1);
            //    WPDevRep.RegisterData(ReportDatasetReport2);
            //    WPDevRep.RegisterData(ReportDatasetReport3);
            //    WPDevRep.Load("WPLicToOpWPDev.frx");
            //    //WPDevRep.Design();
            //    WPDevRep.Show();
            //}

            //if (SelectedField == "Workplace Environmental Conditions")
            //{
            //    MWDataManager.clsDataAccess _dbManVentDetail = new MWDataManager.clsDataAccess();
            //    _dbManVentDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManVentDetail.SqlStatement = "exec [dbo].[sp_LicenceToOperate_TempReading] '" + Workplace + "'";
            //    _dbManVentDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManVentDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManVentDetail.ResultsTableName = "TempDetails";
            //    _dbManVentDetail.ExecuteInstruction();

            //    DataSet ReportDatasetReport1 = new DataSet();
            //    ReportDatasetReport1.Tables.Add(_dbManVentDetail.ResultsDataTable);                

            //    MWDataManager.clsDataAccess _dbManVentPP = new MWDataManager.clsDataAccess();
            //    _dbManVentPP.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManVentPP.SqlStatement = "select * from [ZAWW2K16SQL01].[AngloQlikView].[dbo].[vw_PaperlessActionManagerPrint_AllActions] \r\n";
            //    _dbManVentPP.SqlStatement = _dbManVentPP.SqlStatement + " where datesubmitted > getdate()-30 and stepname like '%tempe%' and workplace = '" + Workplace + "' and actionstatus not in ( 'Not A Deficiency', 'Not Applicable' ) ";
            //    _dbManVentPP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManVentPP.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManVentPP.ResultsTableName = "Paperless";
            //    _dbManVentPP.ExecuteInstruction();

            //    DataSet ReportDatasetReport2 = new DataSet();
            //    ReportDatasetReport2.Tables.Add(_dbManVentPP.ResultsDataTable);                

            //    MWDataManager.clsDataAccess _dbManHeader = new MWDataManager.clsDataAccess();
            //    _dbManHeader.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //    _dbManHeader.SqlStatement = "select '" + ProductionAmplatsGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
            //    _dbManHeader.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManHeader.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManHeader.ResultsTableName = "Header";
            //    _dbManHeader.ExecuteInstruction();

            //    DataSet ReportDatasetReport3 = new DataSet();
            //    ReportDatasetReport3.Tables.Add(_dbManHeader.ResultsDataTable);

            //    TempRep.RegisterData(ReportDatasetReport1);
            //    TempRep.RegisterData(ReportDatasetReport2);
            //    TempRep.RegisterData(ReportDatasetReport3);
            //    TempRep.Load("WPLicToOpTemp.frx");
            //    //TempRep.Design();
            //    TempRep.Show();
            //}

            if (SelectedField == "Critical Skills")
            {
                MWDataManager.clsDataAccess _dbManWPData = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManWPData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManWPData.SqlStatement = "select  top(1) pfnumber  miner,pm.orgunitds org, w.Description \r\n" +
                                            "from tbl_planning p, tbl_Workplace w, tbl_planmonth pm, tbl_section s \r\n" +
                                            "where p.calendardate = CONVERT(VARCHAR(10),getdate(), 20) \r\n" +
                                            "and p.workplaceid = w.workplaceid and p.prodmonth = pm.prodmonth \r\n" +
                                            "and p.workplaceid = pm.workplaceid and p.activity = pm.activity and p.sectionid = pm.sectionid \r\n" +
                                            "and p.prodmonth = s.prodmonth and p.sectionid = s.sectionid \r\n" +
                                            "and w.description = '" + Workplace + "'  order by calendardate desc, p.orgunitds desc ";
                _dbManWPData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPData.ResultsTableName = "RawData";
                _dbManWPData.ExecuteInstruction();

                string Miner = string.Empty;
                string Org = string.Empty;
                string WP = string.Empty;

                if (_dbManWPData.ResultsDataTable.Rows.Count > 0)
                {

                    Miner = _dbManWPData.ResultsDataTable.Rows[0]["miner"].ToString();
                    Org = _dbManWPData.ResultsDataTable.Rows[0]["org"].ToString().Substring(0, 15);
                    WP = _dbManWPData.ResultsDataTable.Rows[0]["description"].ToString();
                }


                MWDataManager.clsDataAccess _dbManDate = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManDate.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManDate.SqlStatement = "select getdate() , '" + Miner + "' mm, '" + Org + "'  org ";
                _dbManDate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDate.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDate.ResultsTableName = "Date";
                _dbManDate.ExecuteInstruction();

                DataSet ReportDatasetReport = new DataSet();
                ReportDatasetReport.Tables.Add(_dbManDate.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManMinerMain = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManMinerMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManMinerMain.SqlStatement = " select a.*, case when ug is null then 'N' else 'Y' end as UG from (select 'a' a, reader_description, max(clock_time) cc \r\n" +
                                               " from [tbl_LicenceToOperate_Labour_Import]  \r\n" +
                                               " where emp_Empno = '" + Miner + "'   and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n" +
                                               " group by reader_description) a  left outer join \r\n" +
                                               " (select 'a' a, max(clock_time) ug from [tbl_LicenceToOperate_Labour_Import]  \r\n" +
                                               " where emp_Empno = '" + Miner + "'   and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n" +
                                               " and (reader_code like '%UG%' or  reader_description like '%Stair%')) b on a.a = b.a \r\n" +
                                               " order by cc   ";
                _dbManMinerMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMinerMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMinerMain.ResultsTableName = "MinerToDay";
                _dbManMinerMain.ExecuteInstruction();

                ReportDatasetReport.Tables.Add(_dbManMinerMain.ResultsDataTable);

                ////Last Clocking Miner
                MWDataManager.clsDataAccess _dbManMinerMainLastClock = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManMinerMainLastClock.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManMinerMainLastClock.SqlStatement = " select top(1)  clock_time, READER_DESCRIPTION, emp_empno  from [tbl_LicenceToOperate_Labour_Import] \r\n" +
                                                        " where emp_Empno = '" + Miner + "' \r\n" +
                                                        " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20) order by clock_time desc  ";
                _dbManMinerMainLastClock.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMinerMainLastClock.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMinerMainLastClock.ResultsTableName = "MinerToDayLastClock";
                _dbManMinerMainLastClock.ExecuteInstruction();

                ReportDatasetReport.Tables.Add(_dbManMinerMainLastClock.ResultsDataTable);

                // miner histrory
                MWDataManager.clsDataAccess _dbManMinerHist = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManMinerHist.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManMinerHist.SqlStatement = " select emp_empno, max(emp_name) name \r\n" +
                                               " ,max(Day40) Day40, max(Day39) Day39, max(Day38) Day38, max(Day37) Day37, max(Day36) Day36, max(Day35) Day35 \r\n" +
                                               " ,max(Day34) Day34, max(Day33) Day33, max(Day32) Day32, max(Day31) Day31, max(Day30) Day30, max(Day29) Day29 \r\n" +
                                               " ,max(Day28) Day28, max(Day27) Day27, max(Day26) Day26, max(Day25) Day25 \r\n" +
                                               " ,max(Day24) Day24, max(Day23) Day23, max(Day22) Day22, max(Day21) Day21, max(Day20) Day20, max(Day19) Day19 \r\n" +
                                               " ,max(Day18) Day18, max(Day17) Day17, max(Day16) Day16, max(Day15) Day15 \r\n" +
                                               " ,max(Day14) Day14, max(Day13) Day13, max(Day12) Day12, max(Day11) Day11, max(Day10) Day10 \r\n" +
                                               "  from (SELECT emp_empno, emp_name, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate(), 20) then day2 else '' end as Day40, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-1, 20) then day2 else '' end as Day39, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-2, 20) then day2 else '' end as Day38, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-3, 20) then day2 else '' end as Day37, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-4, 20) then day2 else '' end as Day36, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-5, 20) then day2 else '' end as Day35, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-6, 20) then day2 else '' end as Day34, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-7, 20) then day2 else '' end as Day33, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-8, 20) then day2 else '' end as Day32, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-9, 20) then day2 else '' end as Day31, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-10, 20) then day2 else '' end as Day30, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-11, 20) then day2 else '' end as Day29, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-12, 20) then day2 else '' end as Day28, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-13, 20) then day2 else '' end as Day27, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-14, 20) then day2 else '' end as Day26, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-15, 20) then day2 else '' end as Day25, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-16, 20) then day2 else '' end as Day24, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-17, 20) then day2 else '' end as Day23, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-18, 20) then day2 else '' end as Day22, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-19, 20) then day2 else '' end as Day21, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-20, 20) then day2 else '' end as Day20, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-21, 20) then day2 else '' end as Day19, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-22, 20) then day2 else '' end as Day18, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-23, 20) then day2 else '' end as Day17, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-24, 20) then day2 else '' end as Day16, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-25, 20) then day2 else '' end as Day15, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-26, 20) then day2 else '' end as Day14, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-27, 20) then day2 else '' end as Day13, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-28, 20) then day2 else '' end as Day12, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-29, 20) then day2 else '' end as Day11, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-30, 20) then day2 else '' end as Day10 \r\n" +
                                               " from  MW_PassStageDB.dbo.ATTENDANCE \r\n" +
                                               " where  convert(varchar(20),attendance_date)+emp_empno in  \r\n" +
                                               " (select convert(varchar(20),attendance_date)+emp_empno from  \r\n" +
                                               " MW_PassStageDB.dbo.ATTENDANCE where emp_Empno ='" + Miner + "' ) \r\n" +
                                               " and emp_empno in (select distinct(emp_empno) a from  [tbl_LicenceToOperate_Labour_Import] \r\n" +
                                               " where emp_Empno = '" + Miner + "' \r\n" +
                                               " and CONVERT(VARCHAR(10),clock_time, 20) >= CONVERT(VARCHAR(10),getdate()-30, 20))) a \r\n" +
                                               " group by emp_empno \r\n " +
                                               " order by emp_empno desc ";
                _dbManMinerHist.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMinerHist.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMinerHist.ResultsTableName = "MinerHist";
                _dbManMinerHist.ExecuteInstruction();

                string MinerPF = string.Empty;

                if (_dbManMinerHist.ResultsDataTable.Rows.Count > 0)
                    MinerPF = _dbManMinerHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();

                DataSet ReportDatasetReport1 = new DataSet();
                ReportDatasetReport1.Tables.Add(_dbManMinerHist.ResultsDataTable);

                // teamleader main
                MWDataManager.clsDataAccess _dbManTeamMain = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManTeamMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManTeamMain.SqlStatement = " declare @minerTL varchar(20) \r\n" +
                                                " set @minerTL = (select max(ug) ug from (select  max(clock_time) ug from [tbl_LicenceToOperate_Labour_Import]  \r\n" +
                                                " where gang_number = '" + Org + "' and wage_Description like '%TEAM%'  \r\n" +
                                                " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n" +
                                                " and reader_code like '%UG%'  and  (reader_description like '%in%') " +
                                                " union " +
                                                " select  max(clock_time) ug from [tbl_LicenceToOperate_Labour_Import]  " +
                                                " where gang_number = '" + Org + "' and wage_Description like '%TEAM%'   " +
                                                " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)   " +
                                                " and reader_description like '%Staircase%') a) " +
                                                " select a.*, case when @minerTL is null then 'N' else 'Y' end as UG from (select 'a' a,'" + Org + "' oo, reader_description, max(clock_time) cc from  [dbo].[tbl_LicenceToOperate_Labour_Import]  \r\n" +
                                                " where gang_number = '" + Org + "' and wage_Description like '%TEAM%'  \r\n" +
                                                " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n" +
                                                " group by reader_description ) a   \r\n" +
                                                " order by cc ";
                _dbManTeamMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManTeamMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManTeamMain.ResultsTableName = "TeamToDay";
                _dbManTeamMain.ExecuteInstruction();

                DataSet ReportDatasetReport2 = new DataSet();
                ReportDatasetReport2.Tables.Add(_dbManTeamMain.ResultsDataTable);

                ////Last Clocking TL
                MWDataManager.clsDataAccess _dbManTLMainLastClock = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManTLMainLastClock.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManTLMainLastClock.SqlStatement = " select top(1) clock_time, READER_DESCRIPTION, emp_empno  from [tbl_LicenceToOperate_Labour_Import] \r\n" +
                                                      " where gang_number = '" + Org + "' and wage_Description like '%TEAM%' \r\n" +
                                                      " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20) order by clock_time desc  ";
                _dbManTLMainLastClock.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManTLMainLastClock.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManTLMainLastClock.ResultsTableName = "TLToDayLastClock";
                _dbManTLMainLastClock.ExecuteInstruction();

                ReportDatasetReport.Tables.Add(_dbManTLMainLastClock.ResultsDataTable);

                // teamleader main
                MWDataManager.clsDataAccess _dbManTeamHist = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManTeamHist.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManTeamHist.SqlStatement = " select  '" + Org + "' oo, emp_empno, max(emp_name) name  \r\n" +
                                               " ,max(Day40) Day40, max(Day39) Day39, max(Day38) Day38, max(Day37) Day37, max(Day36) Day36, max(Day35) Day35 \r\n" +
                                               " ,max(Day34) Day34, max(Day33) Day33, max(Day32) Day32, max(Day31) Day31, max(Day30) Day30, max(Day29) Day29 \r\n" +
                                               " ,max(Day28) Day28, max(Day27) Day27, max(Day26) Day26, max(Day25) Day25 \r\n" +
                                               " ,max(Day24) Day24, max(Day23) Day23, max(Day22) Day22, max(Day21) Day21, max(Day20) Day20, max(Day19) Day19 \r\n" +
                                               " ,max(Day18) Day18, max(Day17) Day17, max(Day16) Day16, max(Day15) Day15, \r\n" +
                                               " max(Day14) Day14, max(Day13) Day13, max(Day12) Day12, max(Day11) Day11, max(Day10) Day10 \r\n" +
                                               "  from (SELECT emp_empno, emp_name, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate(), 20) then day2 else '' end as Day40, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-1, 20) then day2 else '' end as Day39, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-2, 20) then day2 else '' end as Day38, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-3, 20) then day2 else '' end as Day37, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-4, 20) then day2 else '' end as Day36, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-5, 20) then day2 else '' end as Day35, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-6, 20) then day2 else '' end as Day34, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-7, 20) then day2 else '' end as Day33, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-8, 20) then day2 else '' end as Day32, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-9, 20) then day2 else '' end as Day31, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-10, 20) then day2 else '' end as Day30, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-11, 20) then day2 else '' end as Day29, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-12, 20) then day2 else '' end as Day28, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-13, 20) then day2 else '' end as Day27, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-14, 20) then day2 else '' end as Day26, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-15, 20) then day2 else '' end as Day25, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-16, 20) then day2 else '' end as Day24, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-17, 20) then day2 else '' end as Day23, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-18, 20) then day2 else '' end as Day22, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-19, 20) then day2 else '' end as Day21, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-20, 20) then day2 else '' end as Day20, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-21, 20) then day2 else '' end as Day19, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-22, 20) then day2 else '' end as Day18, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-23, 20) then day2 else '' end as Day17, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-24, 20) then day2 else '' end as Day16, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-25, 20) then day2 else '' end as Day15, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-26, 20) then day2 else '' end as Day14, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-27, 20) then day2 else '' end as Day13, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-28, 20) then day2 else '' end as Day12, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-29, 20) then day2 else '' end as Day11, \r\n" +
                                               " case when attendance_date = CONVERT(VARCHAR(10),getdate()-30, 20) then day2 else '' end as Day10 \r\n" +
                                               "  from  MW_PassStageDB.dbo.ATTENDANCE \r\n" +
                                               "  where   \r\n" +
                                               "  convert(varchar(20),attendance_date)+emp_empno in  \r\n" +
                                               " (select convert(varchar(20),attendance_date)+emp_empno from   \r\n" +
                                               " MW_PassStageDB.dbo.ATTENDANCE where gang_number = '" + Org + "'  ) \r\n" +
                                               " and emp_empno in (select distinct(emp_empno) a from  [tbl_LicenceToOperate_Labour_Import] \r\n" +
                                               " where gang_number = '" + Org + "' and wage_Description like '%STOPE TEAM%' \r\n" +
                                               " and CONVERT(VARCHAR(10),clock_time, 20) >= CONVERT(VARCHAR(10),getdate()-30, 20))) a \r\n" +
                                               " group by emp_empno \r\n" +
                                               " order by emp_empno desc ";
                _dbManTeamHist.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManTeamHist.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManTeamHist.ResultsTableName = "TeamHist";
                _dbManTeamHist.ExecuteInstruction();

                DataSet ReportDatasetReport3 = new DataSet();
                ReportDatasetReport3.Tables.Add(_dbManTeamHist.ResultsDataTable);

                string Team = string.Empty;

                if (_dbManTeamHist.ResultsDataTable.Rows.Count > 0)
                { Team = _dbManTeamHist.ResultsDataTable.Rows[0]["emp_empno"].ToString(); }

                string UserPath = @"\\\\afzavrdat01\\vr\\LMSNew\\Images";
                string MinPic = string.Empty;
                string TLPic = string.Empty;
                string Pic = string.Empty;
                string Min = string.Empty;
                string TL = string.Empty;

                if (_dbManMinerMainLastClock.ResultsDataTable.Rows.Count > 0)
                {
                    Pic = _dbManMinerMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                    MinPic = UserPath + @"\" + Pic + ".jpg";
                    Min = _dbManMinerMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                }

                if (_dbManMinerHist.ResultsDataTable.Rows.Count > 0)
                {
                    Pic = _dbManMinerHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                    MinPic = UserPath + @"\" + Pic + ".jpg";
                    Min = _dbManMinerHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                }

                if (_dbManTLMainLastClock.ResultsDataTable.Rows.Count > 0)
                {
                    Pic = _dbManTLMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                    TLPic = UserPath + @"\" + Pic + ".jpg";
                    TL = _dbManTLMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                }

                if (_dbManTeamHist.ResultsDataTable.Rows.Count > 0)
                {
                    Pic = _dbManTeamHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                    TLPic = UserPath + @"\" + Pic + ".jpg";
                    TL = _dbManTeamHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                }

                MWDataManager.clsDataAccess _dbManGenData = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManGenData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManGenData.SqlStatement = " select *, b.employeename mmname, c.employeename tlname from ( select '" + MinPic + "' minerpic, '" + TLPic + "' tlpic, '" + ProductionGlobalTSysSettings._Banner + "' mine,  '" + Team + "' TeamCompNo,  '" + MinerPF + "' MinerCompNo , '" + WP + "' wp, '" + Min + "' mm, '" + TL + "' tt ";
                _dbManGenData.SqlStatement = _dbManGenData.SqlStatement + "  ) a  left outer join employeeall b on a.mm = b.employeeno   left outer join employeeall c on a.tt = c.employeeno ";
                _dbManGenData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGenData.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGenData.ResultsTableName = "GenData";
                _dbManGenData.ExecuteInstruction();

                DataSet ReportDatasetReport4 = new DataSet();
                ReportDatasetReport4.Tables.Add(_dbManGenData.ResultsDataTable);

                theReport.RegisterData(ReportDatasetReport);
                theReport.RegisterData(ReportDatasetReport1);
                theReport.RegisterData(ReportDatasetReport2);
                theReport.RegisterData(ReportDatasetReport3);
                theReport.RegisterData(ReportDatasetReport4);
                theReport.Load("WPLicToOpLab.frx");
                theReport.Show();
            }

            ///Rock Eng Needed

            if (SelectedField == "Workplace")
            {
                MWDataManager.clsDataAccess _dbManWPDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManWPDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManWPDetail.SqlStatement = " select '" + ProductionGlobalTSysSettings._Banner + "' Baner, '" + TUserInfo.UserID + "' username, * from tbl_Workplace where description = '" + Workplace + "' ";
                _dbManWPDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPDetail.ResultsTableName = "WorkplaceDetails";
                _dbManWPDetail.ExecuteInstruction();

                DataSet ReportDatasetReport = new DataSet();
                ReportDatasetReport.Tables.Add(_dbManWPDetail.ResultsDataTable);

                if (_dbManWPDetail.ResultsDataTable.Rows.Count >= 1)
                {
                    WPID = _dbManWPDetail.ResultsDataTable.Rows[0]["workplaceid"].ToString();
                }

                MWDataManager.clsDataAccess _dbManSection = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManSection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManSection.SqlStatement = " select s.pfnumber from tbl_planning p, tbl_Section s where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and  p.workplaceid = '" + WPID + "' \r\n" +
                                             " and calendardate = (select max(calendardate) dd from tbl_planning where workplaceid = '" + WPID + "' and calendardate < getdate()) ";
                _dbManSection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSection.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSection.ResultsTableName = "ccc";
                _dbManSection.ExecuteInstruction();

                string UserPath = @"\\\\afzavrdat01\\vr\\LMSNew\\Images";
                string Pic = string.Empty;
                string Miner = string.Empty;

                if (_dbManSection.ResultsDataTable.Rows.Count > 0)
                {
                    Pic = _dbManSection.ResultsDataTable.Rows[0]["pfnumber"].ToString();
                    Miner = UserPath + @"\" + Pic + ".jpg";
                }

                MWDataManager.clsDataAccess _dbManCrewDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManCrewDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManCrewDetail.SqlStatement = " select substring(max(orgunitds),1,15) crew, max(orgunitds) xxx, '" + Miner + "' miner from tbl_planning where workplaceid =  '" + WPID + "' \r\n" +
                                                " and calendardate = (select max(calendardate) from tbl_planning where workplaceid = '" + WPID + "') ";
                _dbManCrewDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCrewDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCrewDetail.ResultsTableName = "Crew Details";
                _dbManCrewDetail.ExecuteInstruction();

                DataSet ReportDatasetReportCrew = new DataSet();
                ReportDatasetReportCrew.Tables.Add(_dbManCrewDetail.ResultsDataTable);

                if (_dbManCrewDetail.ResultsDataTable.Rows.Count >= 1)
                {
                    CrewID = _dbManCrewDetail.ResultsDataTable.Rows[0]["crew"].ToString();
                }

                MWDataManager.clsDataAccess _dbManMinerDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManMinerDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManMinerDetail.SqlStatement = " SELECT * from  MW_PassStageDB.dbo.ATTENDANCE where \r\n" +
                                                 " attendance_date <= getdate()-1 and attendance_date > getdate()-2 \r\n" +
                                                 " and emp_empno = '" + Pic + "' ";
                _dbManMinerDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMinerDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMinerDetail.ResultsTableName = "Miner Details";
                _dbManMinerDetail.ExecuteInstruction();

                DataSet ReportDatasetReportMiner = new DataSet();
                ReportDatasetReportMiner.Tables.Add(_dbManMinerDetail.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManPSDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManPSDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManPSDetail.SqlStatement = " select p.description, count(description) aa from tbl_ProblemBook pb, tbl_Problem p  \r\n" +
                                                " where pb.problemid = p.problemid and  workplaceid = '" + WPID + "' and calendardate > getdate() - 30 \r\n" +
                                                " group by p.description ";
                _dbManPSDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPSDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPSDetail.ResultsTableName = "Planned Stoppage Graph";
                _dbManPSDetail.ExecuteInstruction();

                DataSet ReportDatasetReportPS = new DataSet();
                ReportDatasetReportPS.Tables.Add(_dbManPSDetail.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManProbDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManProbDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManProbDetail.SqlStatement = " select c.description, count(description) aa  from tbl_planning p, mw_passstagedb.dbo.Central_Code_Cycle c   \r\n" +
                                                " where p.bookprob = c.code  COLLATE Latin1_General_CI_AS and  workplaceid = '" + WPID + "' and calendardate > getdate() - 30 \r\n" +
                                                " group by c.description ";
                _dbManProbDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManProbDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManProbDetail.ResultsTableName = "Problem Graph";
                _dbManProbDetail.ExecuteInstruction();

                DataSet ReportDatasetReportProb = new DataSet();
                ReportDatasetReportProb.Tables.Add(_dbManProbDetail.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManSPDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManSPDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManSPDetail.SqlStatement = " select userid, CONVERT(VARCHAR(11),thedate,20) dd from [dbo].[tbl_WPStopDoc] where workplace = '" + Workplace + "'  \r\n " +
                                              " and thedate  = (select max(thedate) from [dbo].[tbl_WPStopDoc] where workplace = '" + Workplace + "' " +
                                              " and lastbookdate = '" + LastDateBook + "') \r\n" +
                                              " group by  userid, CONVERT(VARCHAR(11),thedate,20) ";
                _dbManSPDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSPDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSPDetail.ResultsTableName = "Stop Procedure";
                _dbManSPDetail.ExecuteInstruction();

                DataSet ReportDatasetReportSP = new DataSet();

                ReportDatasetReportSP.Tables.Add(_dbManSPDetail.ResultsDataTable);

                if (_dbManSPDetail.ResultsDataTable.Rows.Count >= 1)
                {
                    ADate = _dbManSPDetail.ResultsDataTable.Rows[0]["dd"].ToString();
                }

                MWDataManager.clsDataAccess _dbManFailDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManFailDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManFailDetail.SqlStatement = " select stepname, action, hazard,  majorhazard, criticalcontrol, datesubmitted , \r\n  " +
                                                " dateactionclosed, fixedby from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]  \r\n" +
                                                " where datesubmitted  >= getdate()-30 \r\n" +
                                                " and workplace = '" + Workplace + "' and answer = 'No' and hazard = 'A' \r\n" +
                                                " union select '' stepname, ''action, ''hazard,  ''majorhazard, ''criticalcontrol, ''datesubmitted    , ''dateactionclosed, ''fixedby  order by datesubmitted ";
                _dbManFailDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManFailDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManFailDetail.ResultsTableName = "Failures";
                _dbManFailDetail.ExecuteInstruction();

                DataSet ReportDatasetReportFail = new DataSet();
                ReportDatasetReportFail.Tables.Add(_dbManFailDetail.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManStDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManStDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManStDetail.SqlStatement = " select userid, CONVERT(VARCHAR(11),thedate,20) dd from [dbo].[tbl_WPStartDoc] where workplace = '" + Workplace + "'  \r\n " +
                                              " and thedate  = (select max(thedate) from [dbo].[tbl_WPStartDoc] where workplace = '" + Workplace + "' \r\n" +
                                              " and lastbookdate = '" + LastDateBook + "') \r\n" +
                                              " group by  userid, CONVERT(VARCHAR(11),thedate,20) ";
                _dbManStDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManStDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManStDetail.ResultsTableName = "Start Procedure";
                _dbManStDetail.ExecuteInstruction();

                DataSet ReportDatasetReportSttt = new DataSet();
                ReportDatasetReportSttt.Tables.Add(_dbManStDetail.ResultsDataTable);

                if (_dbManSPDetail.ResultsDataTable.Rows.Count >= 1)
                {
                    BDate = _dbManSPDetail.ResultsDataTable.Rows[0]["dd"].ToString();
                }

                MWDataManager.clsDataAccess _dbManStartDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManStartDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManStartDetail.SqlStatement = " select stepname, action, hazard,  majorhazard, criticalcontrol, datesubmitted , \r\n  " +
                                                 " dateactionclosed, fixedby from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions] \r\n  " +
                                                 " where [activityname] like 'Start Procedure' and CONVERT(VARCHAR(11),datesubmitted,106)  = '" + BDate + "' \r\n " +
                                                 " and workplace = '" + Workplace + "' and answer = 'No' \r\n " +
                                                 //GN Moab
                                                 "  union select stepname, action, hazard,  majorhazard, criticalcontrol, datesubmitted  \r\n  " +
                                                 " , dateactionclosed, fixedby from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions] \r\n  " +
                                                 " where taskname = 'MK-IAPI-Extended Break Start Up Checklist' and CONVERT(VARCHAR(11),datesubmitted,106)  = '" + LastDateBook + "' \r\n " +
                                                 " and workplace = '" + Workplace + "' and answer = 'No' \r\n " +
                                                 //Kop
                                                 "  union select stepname, action, hazard,  majorhazard, criticalcontrol, datesubmitted  \r\n " +
                                                 " , dateactionclosed, fixedby from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]  \r\n" +
                                                 " where taskname = 'KP-IAPI-December Break Start Up Checklist' and CONVERT(VARCHAR(11),datesubmitted,106)  = '" + LastDateBook + "' \r\n" +
                                                 " and workplace = '" + Workplace + "' and answer = 'No' \r\n" +
                                                 //Mponeng
                                                 "  union select stepname, action, hazard,  majorhazard, criticalcontrol, datesubmitted  \r\n" +
                                                 " , dateactionclosed, fixedby from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions] \r\n " +
                                                 " where taskname = 'MP-IAPI-2016 Day Shift Miners Extended Break Start Up' and CONVERT(VARCHAR(11),datesubmitted,106)  = '" + LastDateBook + "' \r\n" +
                                                 " and workplace = '" + Workplace + "' and answer = 'No' \r\n" +
                                                 "  union select stepname, action, hazard,  majorhazard, criticalcontrol, datesubmitted   \r\n" +
                                                 " , dateactionclosed, fixedby from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]  \r\n" +
                                                 " where taskname = 'MP-IAPI-Christmas Break Start Up 2016' and CONVERT(VARCHAR(11),datesubmitted,106)  = '" + LastDateBook + "' \r\n" +
                                                 " and workplace = '" + Workplace + "' and answer = 'No' \r\n" +
                                                 "  union select stepname, action, hazard,  majorhazard, criticalcontrol, datesubmitted  \r\n " +
                                                 " , dateactionclosed, fixedby from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]  \r\n" +
                                                 " where taskname = 'MP-IAPI-2016 Night Shift Miners Extended Break Start Up' and CONVERT(VARCHAR(11),datesubmitted,106)  = '" + LastDateBook + "' \r\n" +
                                                 " and workplace = '" + Workplace + "' and answer = 'No' \r\n" +
                                                 //Tau Tona Sav
                                                 "  union select stepname, action, hazard,  majorhazard, criticalcontrol, datesubmitted   \r\n" +
                                                 " , dateactionclosed, fixedby from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions]  \r\n" +
                                                 " where taskname = 'TT-IAPI-Extended Break Stoping Start-Up' and CONVERT(VARCHAR(11),datesubmitted,106)  = '" + LastDateBook + "' \r\n" +
                                                 " and workplace = '" + Workplace + "' and answer = 'No' \r\n" +
                                                 "  union select stepname, action, hazard,  majorhazard, criticalcontrol, datesubmitted   \r\n" +
                                                 " , dateactionclosed, fixedby from [MW_PassStageDB].[dbo].[vw_PaperlessActionManagerPrint_AllActions] \r\n " +
                                                 " where taskname = 'TT-IAPI-Extended Break Development Start-Up' and CONVERT(VARCHAR(11),datesubmitted,106)  = '" + LastDateBook + "' \r\n " +
                                                 " and workplace = '" + Workplace + "' and answer = 'No' \r\n" +
                                                 " union select '' stepname, ''action, ''hazard,  ''majorhazard, ''criticalcontrol, ''datesubmitted    , ''dateactionclosed, ''fixedby ";
                _dbManStartDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManStartDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManStartDetail.ResultsTableName = "StartDeviation";
                _dbManStartDetail.ExecuteInstruction();

                DataSet ReportDatasetReportStart = new DataSet();
                ReportDatasetReportStart.Tables.Add(_dbManStartDetail.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManLabourDetail = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManLabourDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManLabourDetail.SqlStatement = " select thedate, convert(decimal(18,2),avg(ds_ug_dur_min))/60 tt   \r\n" +
                                                  " from [tb_ClockingHistory] where gang = '" + CrewID + "' and thedate > getdate()-30  \r\n" +
                                                  " group by thedate order by thedate ";
                _dbManLabourDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManLabourDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManLabourDetail.ResultsTableName = "Labour";
                _dbManLabourDetail.ExecuteInstruction();

                DataSet ReportDatasetReportLabour = new DataSet();
                ReportDatasetReportLabour.Tables.Add(_dbManLabourDetail.ResultsDataTable);

                MWDataManager.clsDataAccess _dbManLabourDetail2 = new MWDataManager.clsDataAccess();
                #pragma warning disable CS0618
                _dbManLabourDetail2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                #pragma warning restore CS0618
                _dbManLabourDetail2.SqlStatement = " select *,  hr+':'+mina tt from (select empno,emp_name, occupation, wage_code, convert(decimal(18,2),avg(ds_ug_dur_min))/60 tt11,   \r\n" +
                                                   " case when floor(convert(decimal(18,2),avg(ds_ug_dur_min))/60) > 9 then convert(varchar(10),floor(convert(decimal(18,2),avg(ds_ug_dur_min))/60)) else   \r\n " +
                                                   "  '0'+ convert(varchar(10),floor(convert(decimal(18,2),avg(ds_ug_dur_min))/60)) end as hr,   \r\n" +
                                                   "  case when floor(((convert(decimal(18,2),avg(ds_ug_dur_min))/60)-floor(convert(decimal(18,2),avg(ds_ug_dur_min))/60))*60) > 9   \r\n" +
                                                   " then convert(varchar(10),floor(((convert(decimal(18,2),avg(ds_ug_dur_min))/60)-floor(convert(decimal(18,2),avg(ds_ug_dur_min))/60))*60))  \r\n" +
                                                   "  else  '0'+ convert(varchar(10),floor(((convert(decimal(18,2),avg(ds_ug_dur_min))/60)-floor(convert(decimal(18,2),avg(ds_ug_dur_min))/60))*60))  \r\n" +
                                                   "  end as mina from  [tb_ClockingHistory] where gang = '" + CrewID + "' and thedate > getdate()-30 \r\n" +
                                                   " group by empno,emp_name, occupation, wage_code) a order by wage_code desc ";
                _dbManLabourDetail2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManLabourDetail2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManLabourDetail2.ResultsTableName = "LabourDetail";
                _dbManLabourDetail2.ExecuteInstruction();

                DataSet ReportDatasetReportLabour2 = new DataSet();
                ReportDatasetReportLabour2.Tables.Add(_dbManLabourDetail2.ResultsDataTable);

                theReport.RegisterData(ReportDatasetReport);
                theReport.RegisterData(ReportDatasetReportCrew);
                theReport.RegisterData(ReportDatasetReportMiner);
                theReport.RegisterData(ReportDatasetReportPS);
                theReport.RegisterData(ReportDatasetReportProb);
                theReport.RegisterData(ReportDatasetReportSP);
                theReport.RegisterData(ReportDatasetReportFail);
                theReport.RegisterData(ReportDatasetReportLabour);
                theReport.RegisterData(ReportDatasetReportLabour2);
                theReport.RegisterData(ReportDatasetReportStart);
                theReport.RegisterData(ReportDatasetReportSttt);
                theReport.Load("WPLicToOp.frx");

                theReport.Show();
            }
            Cursor = Cursors.Default;
        }

        private void gvLTO_RowCellClick_1(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            Workplace = gvLTOStope.GetRowCellValue(e.RowHandle, gvLTOStope.Columns[1]).ToString();
            SelectedField = e.Column.ToString();

            if (SelectedField == "Rock Eng Dept.")
            { SelectedCol = gvLTOStope.GetRowCellValue(e.RowHandle, gvLTOStope.Columns[10]).ToString(); }

            if (SelectedField == "GeoScience Dept.")
            { SelectedCol = gvLTOStope.GetRowCellValue(e.RowHandle, gvLTOStope.Columns[11]).ToString(); }

            ActType = "Stp";
        }

        private void gvLTODev_RowCellClick_1(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            Workplace = gvLTODev.GetRowCellValue(e.RowHandle, gvLTODev.Columns[1]).ToString();
            SelectedField = e.Column.ToString();

            if (SelectedField == "Rock Eng Dept.")
            { SelectedCol = gvLTODev.GetRowCellValue(e.RowHandle, gvLTODev.Columns[10]).ToString(); }

            if (SelectedField == "GeoScience Dept.")
            { SelectedCol = gvLTODev.GetRowCellValue(e.RowHandle, gvLTODev.Columns[11]).ToString(); }

            ActType = "Dev";
        }

        private void gvLTOStope_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {


        }

        private void gvLTOStope_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //if (e.Column.VisibleIndex > 2 && e.RowHandle < 100)
            //{
            //    if (e.Column.FieldName == "ColGeol" || e.Column.FieldName == "ColVent")
            //    {
            //        string aa = gvLTOStope.GetRowCellValue(e.RowHandle, e.Column).ToString();

            //        if (aa == "2")
            //        {

            //            gvLTOStope.SetRowCellValue(e.RowHandle, e.Column, "Medium");
            //        }
            //        if (aa == "3")
            //        {
            //            gvLTOStope.SetRowCellValue(e.RowHandle, e.Column, "High");
            //        }

            //    }
            //}
        }

        private void gvLTOStope_DoubleClick(object sender, EventArgs e)
        {
            Workplace = gvLTOStope.GetRowCellValue(gvLTOStope.FocusedRowHandle, gvLTOStope.Columns["WPDescription"]).ToString();
            SectionID = gvLTOStope.GetRowCellValue(gvLTOStope.FocusedRowHandle, gvLTOStope.Columns["MOSection"]).ToString();
            SelectedCol = gvLTOStope.FocusedColumn.Caption;

            if (SelectedCol == "Rock Engineering")
            {
                LoadRockEng();

            }

            if (SelectedCol == "Geology")
            {
                LoadGeologyReport();
            }

            if (SelectedCol == "Ventilation")
            {
                LoadVentReport();
            }

            if (SelectedCol == "Critical Skills")
            {
                LoadCriticalSkills();
            }

            if (SelectedCol == "Major Hazards")
            {
                LoadMajorHazard();
            }


            if (SelectedCol == "Production")
            {
                LoadProduction();
                
            }

            if (SelectedCol != "Rock Engineering")
            {
                //theReport.Design();
                FastReport.Utils.XmlItem itemDaily = FastReport.Utils.Config.Root.FindItem("Forms").FindItem("PreviewForm");
                //itemDaily.SetProp("Maximized", "0");
                //itemDaily.SetProp("Left", Convert.ToString(Right - 450));
                //itemDaily.SetProp("Top", "50");
                //itemDaily.SetProp("Width", "1500");
                //itemDaily.SetProp("Height", "1000");
                theReport.Show(false);
                theReport.Preview.ZoomPageWidth();
            }
            //theReport.Design();
            //theReport.Prepare();



        }

        string RRLabel = string.Empty, RRLbl = string.Empty;

        private void LoadRockEng()
        {
            RockEngFrm _rockEngFrm = new RockEngFrm();
            _rockEngFrm.WPLbl.EditValue = Workplace;
            #pragma warning disable CS0618
            _rockEngFrm._UserCurrentInfo = TUserInfo.Site;
            #pragma warning restore CS0618
            _rockEngFrm.LTO = "Y";
            _rockEngFrm.Show();
            return;
            
        }

        private void LoadGeologyReport()
        {
            MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManWPST2.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManWPST2.SqlStatement = "select top (20) * from (   \r\n" +
                                        "Select 'Z' bb,  \r\n" +
                                        "case when targetdate is null then 'Not Accepted'  \r\n" +
                                        "when targetdate is not null then 'Accepted'   \r\n" +
                                        " when CompletionDate is not null then 'Completed'  \r\n" +
                                        " when VerificationDate is not null then 'Verified' else '' end as ActionStatus   \r\n" +
                                        " ,[description] as Action, thedate datesubmitted, datediff(day, thedate, getdate()) ss   \r\n" +
                                        " from[dbo].[tbl_Shec_Incidents]  \r\n" +
                                            "       where workplace = '" + Workplace + "'   \r\n" +
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

            theReport.RegisterData(dsABS1);

            MWDataManager.clsDataAccess _dbManDevSummmary = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManDevSummmary.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManDevSummmary.SqlStatement = " select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' banner, '" + RRLbl + "' rr,  * \r\n" +
                " from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + Workplace + "' \r\n" +
                " and CaptDate = (select max(CaptDate) from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + Workplace + "')";
            _dbManDevSummmary.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDevSummmary.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDevSummmary.ResultsTableName = "DevSummary";
            _dbManDevSummmary.ExecuteInstruction();

            DataSet dsDevSummary = new DataSet();
            dsDevSummary.Tables.Add(_dbManDevSummmary.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManImage = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManImage.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManImage.SqlStatement = "Select picture, Picture1 from  [tbl_DPT_GeoScience_Inspection] \r\n"
                        + " where workplace = '" + Workplace + "' \r\n"
                        + " and captdate = (select max(captdate) from  [tbl_DPT_GeoScience_Inspection] where workplace = '" + Workplace + "')";
            _dbManImage.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManImage.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManImage.ResultsTableName = "Image";
            _dbManImage.ExecuteInstruction();

            DataSet dsImage = new DataSet();
            dsImage.Tables.Add(_dbManImage.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManChart = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManChart.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManChart.SqlStatement = " select top(10) * from (select description, a.CalendarDate aa ,substring(convert(varchar(11),a.CalendarDate),0,12) Calendardate,a.SWidth,a.CorrCut,a.Hangwall,a.Footwall, \r\n" +
                                " case when a.allocatedWidth = 0 then null else a.allocatedWidth end as allocatedWidth ,a.Notes from [dbo].[tbl_SAMPLING_Imported_Notes] a  \r\n" +
                                " left outer  join tbl_Workplace_Total w on convert(varchar(50),a.gmsiwpis) = w.gmsiwpid  \r\n" +
                                " and calendardate > getdate()-2000 ) a where description = '" + Workplace + "'    \r\n" +
                                "  order by aa desc  ";
            _dbManChart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManChart.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManChart.ResultsTableName = "Chart";
            _dbManChart.ExecuteInstruction();

            DataSet dsChart = new DataSet();
            dsChart.Tables.Add(_dbManChart.ResultsDataTable);

            //LoadImage("GeologyInspection");

            //theReport.RegisterData("Image", Attachment);

            theReport.RegisterData(dsDevSummary);
            theReport.RegisterData(dsImage);
            theReport.RegisterData(dsChart);

            theReport.Load(ReportsFolder + "\\GeoInsp.frx");
            //theReport.Design();
            theReport.Prepare();

            FastReport.Utils.XmlItem itemDaily = FastReport.Utils.Config.Root.FindItem("Forms").FindItem("PreviewForm");
            itemDaily.SetProp("Maximized", "0");
            itemDaily.SetProp("Left", Convert.ToString(Right - 450));
            itemDaily.SetProp("Top", "50");
            itemDaily.SetProp("Width", "1500");
            itemDaily.SetProp("Height", "1000");
            theReport.Show();
            //theReport.Preview.ZoomPageWidth();
        }

        private void LoadVentReport()
        {
            // do fieldbookinfo
            MWDataManager.clsDataAccess _dbManField = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManField.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManField.SqlStatement = "select Top 1 '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' mine, *" +
                " from tbl_Dept_Inspection_VentCapture_FeildBook where calendardate = (select max(Calendardate) from tbl_Dept_Inspection_VentCapture_FeildBook where Section like '%" + SectionID + "%') "; // and Section = '" + SectionID + "' ";
            _dbManField.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManField.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManField.ResultsTableName = "FieldBook";  //get table name
            _dbManField.ExecuteInstruction();

            DataSet dsFieldBook = new DataSet();
            dsFieldBook.Tables.Add(_dbManField.ResultsDataTable);

            //Fire Protection
            MWDataManager.clsDataAccess _dbManVentData = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManVentData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManVentData.SqlStatement = "declare @calDate varchar(50), @section varchar(50), @WorkplaceID varchar(10) \r\n"
                        + " set @workplaceID = (select WorkplaceID from tbl_Workplace where Description = '" + Workplace + "') \r\n"
                        + " set @section = (select max(Section) from tbl_Dept_Inspection_VentCapture_Stoping where Workplace = @workplaceID) \r\n"
                        + " set @calDate = (select max(Calendardate) from tbl_Dept_Inspection_VentCapture_Stoping where Workplace = @WorkplaceID) \r\n"
                        + " exec sp_Dept_Insection_Vent_Stoping_Main @calDate, @section ";
            _dbManVentData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManVentData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManVentData.ResultsTableName = "StopingData";  //get table name
            _dbManVentData.ExecuteInstruction();

            DataSet dsVetData = new DataSet();
            dsVetData.Tables.Add(_dbManVentData.ResultsDataTable);

            //General
            MWDataManager.clsDataAccess _dbManGenData = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManGenData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManGenData.SqlStatement = "declare @calDate varchar(50), @section varchar(50), @WorkplaceID varchar(10) \r\n"
                        + " set @workplaceID = (select WorkplaceID from tbl_Workplace where Description = '" + Workplace + "') \r\n"
                        + " set @section = (select max(Section) from [tbl_Dept_Inspection_VentCapture_Stoping_PerSection] where Workplace = @workplaceID) \r\n"
                        + " set @calDate = (select max(Calendardate) from [tbl_Dept_Inspection_VentCapture_Stoping_PerSection] where Workplace = @WorkplaceID) \r\n"
                        + " exec sp_Dept_Insection_Vent_Questions_Stoping_PerSection_Report_All @calDate, @section";
            _dbManGenData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGenData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGenData.ResultsTableName = "StopingGeneralData";  //get table name
            _dbManGenData.ExecuteInstruction();

            DataSet dsGenData = new DataSet();
            dsGenData.Tables.Add(_dbManGenData.ResultsDataTable);

            ///Winch Installation
            MWDataManager.clsDataAccess _dbManWinchData = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManWinchData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManWinchData.SqlStatement = " declare @calDate varchar(50), @section varchar(50) \r\n "
                        + " set @section = (select max(Section) from [tbl_Dept_Inspection_VentCapture_StopingWinch] where Section like '%" + SectionID + "%') \r\n"
                        + " set @calDate = (select max(Calendardate) from [tbl_Dept_Inspection_VentCapture_StopingWinch] where Section like '%" + SectionID + "%') \r\n"
                        + " exec sp_Dept_Insection_Vent_Questions_Stoping_Winch @calDate, @section";
            _dbManWinchData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWinchData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWinchData.ResultsTableName = "StopingWinchData";  //get table name
            _dbManWinchData.ExecuteInstruction();

            DataSet dsWinchData = new DataSet();
            dsWinchData.Tables.Add(_dbManWinchData.ResultsDataTable);

            ///Station Temp
            MWDataManager.clsDataAccess _dbManStationTemp = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618 // Type or member is obsolete
            _dbManStationTemp.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618 // Type or member is obsolete
            _dbManStationTemp.SqlStatement = " declare @calDate varchar(15), @section varchar(50) \r\n"
                        + " set @section = (select max(Section) from[tbl_Dept_Inspection_VentCapture_StationTemp] where Section like '%" + SectionID + "%') \r\n"
                        + " set @calDate = format(Cast((select max(Calendardate) from[tbl_Dept_Inspection_VentCapture_StationTemp] where Section like '%" + SectionID + "%') as date), 'yyyyMMdd' ) \r\n"
                        + " exec sp_Dept_Insection_Vent_Stoping_StationTempReport @calDate, @section";
            _dbManStationTemp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManStationTemp.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManStationTemp.ResultsTableName = "StationTempData";  //get table name
            _dbManStationTemp.ExecuteInstruction();

            DataSet dsStationTemp = new DataSet();
            dsStationTemp.Tables.Add(_dbManStationTemp.ResultsDataTable);

            ///Refuge Bay
            //frmInspection insCrew = new frmInspection();
            MWDataManager.clsDataAccess _dbManRefugeBay = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618 // Type or member is obsolete
            _dbManRefugeBay.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618 // Type or member is obsolete
            _dbManRefugeBay.SqlStatement = " declare @calDate varchar(15), @section varchar(50), @prodMonth varchar(10), @crew varchar(20) \r\n"
                        + " set @crew = (select max(Crew) from tbl_Dept_Inspection_VentCapture_RefugeBay where Section like '%" + SectionID + "%') \r\n"
                        + " set @prodMonth = (select max(MonthDate) from tbl_Dept_Inspection_VentCapture_RefugeBay where Section like '%" + SectionID + "%') \r\n"
                        + " set @section = (select max(Section) from tbl_Dept_Inspection_VentCapture_RefugeBay where Section like '%" + SectionID + "%') \r\n"
                        + " set @calDate = (select max(Calendardate) from tbl_Dept_Inspection_VentCapture_RefugeBay where Section like '%" + SectionID + "%' ) \r\n"
                        + " exec sp_Dept_Insection_Vent_Stoping_RefugeBayReport @prodMonth, @calDate, @section, @crew";
            _dbManRefugeBay.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRefugeBay.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRefugeBay.ResultsTableName = "RefugeBayData";  //get table name
            _dbManRefugeBay.ExecuteInstruction();

            DataSet dsRefugeBay = new DataSet();
            dsRefugeBay.Tables.Add(_dbManRefugeBay.ResultsDataTable);

            ///HeaderData
            MWDataManager.clsDataAccess _dbManHeaderData = new MWDataManager.clsDataAccess();
            #pragma warning disable CS0618
            _dbManHeaderData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            #pragma warning restore CS0618
            _dbManHeaderData.SqlStatement = " declare @calDate varchar(15), @section varchar(50), @WorkplaceID varchar(10) \r\n"
                        + " set @workplaceID = (select WorkplaceID from tbl_Workplace where Description = '" + Workplace + "') \r\n"
                        + " set @section = (select max(SectionID) from tbl_Planning where WorkplaceID = @workplaceID) \r\n"
                        + " set @calDate = format(Cast((select max(Calendardate) from tbl_Planning where WorkplaceID = @WorkplaceID) as date), 'yyyyMMdd' ) \r\n"
                        + " Select* from tbl_sectionComplete where sectionid = @section \r\n"
                        + " and prodmonth = (Select max(prodmonth) from tbl_Planning where calendardate = @calDate ) ";
            _dbManHeaderData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManHeaderData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManHeaderData.ResultsTableName = "HeaderData";  //get table name
            _dbManHeaderData.ExecuteInstruction();

            DataSet dsVentHeaderData = new DataSet();
            dsVentHeaderData.Tables.Add(_dbManHeaderData.ResultsDataTable);

            ///Fire Rating
            MWDataManager.clsDataAccess _dbManFireRating = new MWDataManager.clsDataAccess();
            _dbManFireRating.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManFireRating.SqlStatement = "declare @calDate varchar(15), @section varchar(50) \r\n"
                        + " set @section = (select max(Section) from[tbl_Dept_Inspection_VentCapture_FireRating] where Workplace = '" + Workplace + "') \r\n"
                        + " set @calDate =  format(Cast((select max(Calendardate) from [tbl_Dept_Inspection_VentCapture_FireRating] where Workplace = '" + Workplace + "')  as date), 'yyyyMMdd' ) \r\n"
                        + " exec sp_Dept_Insection_Vent_Stoping_FireRating @section, @calDate ";
            _dbManFireRating.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFireRating.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFireRating.ResultsTableName = "FireRating";  //get table name
            _dbManFireRating.ExecuteInstruction();

            DataSet repFire = new DataSet();
            repFire.Tables.Add(_dbManFireRating.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManIncidents = new MWDataManager.clsDataAccess();
            _dbManIncidents.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManIncidents.SqlStatement = "declare @calDate varchar(15), @section varchar(50) \r\n"
                    + " set @section = (select max(WPType) from tbl_Shec_IncidentsVent where Workplace = '" + Workplace + "') \r\n"
                    + " set @calDate =  format(Cast((select max(TheDate) from tbl_Shec_IncidentsVent where Workplace = '" + Workplace + "')as date), 'yyyyMMdd' )  \r\n"
                    + " select * from tbl_Shec_IncidentsVent where  Workplace = '" + Workplace + "' and thedate = @calDate and WPType = @section and Type = 'VSA' ";
            _dbManIncidents.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManIncidents.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManIncidents.ResultsTableName = "Incident";  //get table name
            _dbManIncidents.ExecuteInstruction();

            DataSet dsIncident = new DataSet();
            dsIncident.Tables.Add(_dbManIncidents.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManIncF = new MWDataManager.clsDataAccess();
            _dbManIncF.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManIncF.SqlStatement = "declare @calDate varchar(15), @section varchar(50) \r\n"
                    + " set @section = (select max(WPType) from tbl_Shec_IncidentsVent where Workplace = '" + Workplace + "') \r\n"
                    + " set @calDate = format(Cast((select max(TheDate) from tbl_Shec_IncidentsVent where Workplace = '" + Workplace + "')  as date), 'yyyyMMdd' )\r\n"
                    + " select* from tbl_Shec_IncidentsVent where Workplace = '" + Workplace + "' and thedate = @calDate and WPType = @section and Type = 'VSAF' ";
            _dbManIncF.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManIncF.ResultsTableName = "IncidentF";  //get table name
            _dbManIncF.ExecuteInstruction();

            DataSet DSIncidentF = new DataSet();
            //DSIncidentF.Tables.Add(_dbManIncF.ResultsDataTable);

            LoadImage("VentilationInspections\\StandardInspections");

            theReport.RegisterData(repFire);
            theReport.RegisterData(dsFieldBook);
            theReport.RegisterData(dsVetData);
            theReport.RegisterData(dsGenData);
            theReport.RegisterData(dsWinchData);
            theReport.RegisterData(dsStationTemp);
            theReport.RegisterData(dsRefugeBay);
            theReport.RegisterData(dsVentHeaderData);

            theReport.RegisterData(dsIncident);
            theReport.RegisterData(DSIncidentF);

            theReport.SetParameterValue("VentImage", Attachment);

            theReport.Load(ReportsFolder + "\\MasterStopeReport.frx");
        }

        private void LoadCriticalSkills()
        {
            MWDataManager.clsDataAccess _dbManRawData = new MWDataManager.clsDataAccess();
            _dbManRawData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManRawData.SqlStatement = "select  top(1) PFNumber  miner,\r\n" +
                                        "pm.OrgUnitDS+'                                         ' org, w.Description  \r\n" +
                                        "from tbl_Planning p, tbl_Workplace w, tbl_PlanMonth pm, tbl_Section s  \r\n" +
                                        "where p.calendardate = CONVERT(VARCHAR(10),getdate(), 20)  \r\n" +
                                        "and p.workplaceid = w.workplaceid and p.prodmonth = pm.prodmonth  \r\n" +
                                        "and p.workplaceid = pm.workplaceid and p.activity = pm.activity and p.sectionid = pm.sectionid  \r\n" +
                                        "and p.prodmonth = s.prodmonth and p.sectionid = s.sectionid  \r\n" +
                                        "and w.description = '" + Workplace + "'  order by calendardate desc, pm.OrgUnitDS desc ";
            _dbManRawData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRawData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRawData.ResultsTableName = "RawData";
            _dbManRawData.ExecuteInstruction();

            string miner = string.Empty;
            string org = string.Empty;
            string wp = string.Empty;

            if (_dbManRawData.ResultsDataTable.Rows.Count > 0)
            {

                miner = _dbManRawData.ResultsDataTable.Rows[0]["miner"].ToString();
                org = _dbManRawData.ResultsDataTable.Rows[0]["org"].ToString().Substring(0, 12);
                wp = _dbManRawData.ResultsDataTable.Rows[0]["description"].ToString();
            }

            string clocking = string.Empty;
            if (ProductionGlobal.ProductionGlobalTSysSettings._Banner == "Amandelbult Mine")
            {
                clocking = "UNDERGROUND";
            }
            if (ProductionGlobal.ProductionGlobalTSysSettings._Banner == "Tumela")
            {
                clocking = "SKYWALK";
            }

            MWDataManager.clsDataAccess _dbManDate = new MWDataManager.clsDataAccess();
            _dbManDate.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManDate.SqlStatement = "select getdate() , '" + miner + "' mm, '" + org + "'  org ";
            _dbManDate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDate.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDate.ResultsTableName = "Date";
            _dbManDate.ExecuteInstruction();

            DataSet dsDate = new DataSet();
            dsDate.Tables.Add(_dbManDate.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManMinerToDay = new MWDataManager.clsDataAccess();
            _dbManMinerToDay.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManMinerToDay.SqlStatement = " select a.*, case when ug is null then 'N' else 'Y' end as UG from (select 'a' a, reader_description, max(clock_time) cc \r\n" +
                        " from [tbl_LicenceToOperate_Labour_Import]   \r\n" +
                        " where emp_Empno = '" + miner + "'   and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)   \r\n" +
                        " group by reader_description) a  left outer join  \r\n" +
                        " (select 'a' a, max(clock_time) ug from [tbl_LicenceToOperate_Labour_Import]   \r\n" +
                        " where emp_Empno = '" + miner + "'   and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)   \r\n" +
                        " and (   reader_description like '%" + clocking + "%' or reader_description like '%UG%')) b on a.a = b.a " +

                        //"  and reader_description like '%Underground%') b on a.a = b.a " +                
                        " order by cc   ";
            _dbManMinerToDay.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMinerToDay.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMinerToDay.ResultsTableName = "MinerToDay";
            _dbManMinerToDay.ExecuteInstruction();

            DataSet dsMinerToDay = new DataSet();
            dsMinerToDay.Tables.Add(_dbManMinerToDay.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManMinerLastClock = new MWDataManager.clsDataAccess();
            _dbManMinerLastClock.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManMinerLastClock.SqlStatement = " select top(1)  clock_time, READER_DESCRIPTION, emp_empno  from [tbl_LicenceToOperate_Labour_Import] \r\n" +
                " where emp_Empno = '" + miner + "' \r\n" +
                " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20) order by clock_time desc  ";
            _dbManMinerLastClock.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMinerLastClock.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMinerLastClock.ResultsTableName = "MinerToDayLastClock";
            _dbManMinerLastClock.ExecuteInstruction();

            DataSet dsManMinerLastClock = new DataSet();
            dsManMinerLastClock.Tables.Add(_dbManMinerLastClock.ResultsDataTable);

            // miner histrory
            MWDataManager.clsDataAccess _dbManMinerHist = new MWDataManager.clsDataAccess();
            _dbManMinerHist.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManMinerHist.SqlStatement = " select emp_empno, max(emp_name) name \r\n" +
                                           " ,max(Day40) Day40, max(Day39) Day39, max(Day38) Day38, max(Day37) Day37, max(Day36) Day36, max(Day35) Day35 \r\n" +
                                           " ,max(Day34) Day34, max(Day33) Day33, max(Day32) Day32, max(Day31) Day31, max(Day30) Day30, max(Day29) Day29 \r\n" +

                                           " ,max(Day28) Day28, max(Day27) Day27, max(Day26) Day26, max(Day25) Day25 \r\n" +
                                           " ,max(Day24) Day24, max(Day23) Day23, max(Day22) Day22, max(Day21) Day21, max(Day20) Day20, max(Day19) Day19 \r\n" +

                                           " ,max(Day18) Day18, max(Day17) Day17, max(Day16) Day16, max(Day15) Day15 \r\n" +
                                           " ,max(Day14) Day14, max(Day13) Day13, max(Day12) Day12, max(Day11) Day11, max(Day10) Day10 \r\n" +

                                           "  from (SELECT emp_empno, emp_name, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate(), 20) then day2 else '' end as Day40, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-1, 20) then day2 else '' end as Day39, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-2, 20) then day2 else '' end as Day38, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-3, 20) then day2 else '' end as Day37, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-4, 20) then day2 else '' end as Day36, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-5, 20) then day2 else '' end as Day35, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-6, 20) then day2 else '' end as Day34, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-7, 20) then day2 else '' end as Day33, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-8, 20) then day2 else '' end as Day32, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-9, 20) then day2 else '' end as Day31, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-10, 20) then day2 else '' end as Day30, \r\n" +

                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-11, 20) then day2 else '' end as Day29, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-12, 20) then day2 else '' end as Day28, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-13, 20) then day2 else '' end as Day27, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-14, 20) then day2 else '' end as Day26, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-15, 20) then day2 else '' end as Day25, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-16, 20) then day2 else '' end as Day24, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-17, 20) then day2 else '' end as Day23, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-18, 20) then day2 else '' end as Day22, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-19, 20) then day2 else '' end as Day21, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-20, 20) then day2 else '' end as Day20, \r\n" +

                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-21, 20) then day2 else '' end as Day19, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-22, 20) then day2 else '' end as Day18, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-23, 20) then day2 else '' end as Day17, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-24, 20) then day2 else '' end as Day16, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-25, 20) then day2 else '' end as Day15, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-26, 20) then day2 else '' end as Day14, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-27, 20) then day2 else '' end as Day13, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-28, 20) then day2 else '' end as Day12, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-29, 20) then day2 else '' end as Day11, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-30, 20) then day2 else '' end as Day10 \r\n" +
                                           "  from  tbl_Attendance \r\n" +
                                           "  where   \r\n" +
                                           "  convert(varchar(20),attendance_date)+emp_empno in  \r\n" +
                                           " (select convert(varchar(20),attendance_date)+emp_empno from  \r\n" +
                                           " tbl_Attendance where emp_Empno ='" + miner + "' ) \r\n" +
                                           " and emp_empno in (select distinct(emp_empno) a from  [tbl_LicenceToOperate_Labour_Import] \r\n" +
                                           " where emp_Empno = '" + miner + "' \r\n" +
                                           " and CONVERT(VARCHAR(10),clock_time, 20) >= CONVERT(VARCHAR(10),getdate()-30, 20))) a \r\n" +
                                           " group by emp_empno \r\n " +

                                           " order by emp_empno desc \r\n";
            _dbManMinerHist.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMinerHist.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMinerHist.ResultsTableName = "MinerHist";
            _dbManMinerHist.ExecuteInstruction();

            //string Minerpf = "";

            //if (_dbManMinerHist.ResultsDataTable.Rows.Count > 0)
            //    Minerpf = _dbManMinerHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();

            //DataSet ReportDatasetReport1 = new DataSet();
            //ReportDatasetReport1.Tables.Add(_dbManMinerHist.ResultsDataTable);



            // teamleader main
            MWDataManager.clsDataAccess _dbManTeamToDay = new MWDataManager.clsDataAccess();
            _dbManTeamToDay.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManTeamToDay.SqlStatement = "  declare @minerTL varchar(20) \r\n" +
                                           " set @minerTL = (select max(ug) ug from (select  max(clock_time) ug from [tbl_LicenceToOperate_Labour_Import]  \r\n" +
                                           " where gang_number = '" + org + "' and wage_Description like '%TEAM%'  \r\n" +
                                           " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n" +
                                           " and   reader_description like '%" + clocking + "%' \r\n" +
                                           " union \r\n" +
                                           " select  max(clock_time) ug from [tbl_LicenceToOperate_Labour_Import] \r\n" +
                                           " where gang_number = '" + org + "' and wage_Description like '%TEAM%' \r\n" +
                                           " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20) \r\n" +
                                           " and reader_description like '%Staircase%') a) \r\n" +
                                           " select a.*, case when @minerTL is null then 'N' else 'Y' end as UG \r\n" +
                                           " from (select 'a' a,'" + org + "' oo, reader_description, max(clock_time) cc from  [dbo].[tbl_LicenceToOperate_Labour_Import]  \r\n" +
                                           " where gang_number = '" + org + "' and wage_Description like '%TEAM%'  \r\n" +
                                           " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20)  \r\n" +
                                           " group by reader_description ) a   \r\n" +
                                           " order by cc   \r\n";
            _dbManTeamToDay.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManTeamToDay.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManTeamToDay.ResultsTableName = "TeamToDay";
            _dbManTeamToDay.ExecuteInstruction();

            DataSet dsTeamToDay = new DataSet();
            dsTeamToDay.Tables.Add(_dbManTeamToDay.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManTLMainLastClock = new MWDataManager.clsDataAccess();
            _dbManTLMainLastClock.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManTLMainLastClock.SqlStatement = " select top(1) clock_time, READER_DESCRIPTION, emp_empno  from [tbl_LicenceToOperate_Labour_Import] \r\n" +
                " where gang_number = '" + org + "' and wage_Description like '%TEAM%' \r\n" +
                " and CONVERT(VARCHAR(10),clock_time, 20) = CONVERT(VARCHAR(10),getdate(), 20) order by clock_time desc \r\n";
            _dbManTLMainLastClock.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManTLMainLastClock.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManTLMainLastClock.ResultsTableName = "TLToDayLastClock";
            _dbManTLMainLastClock.ExecuteInstruction();

            DataSet dsTLToDayLastClock = new DataSet();
            dsTLToDayLastClock.Tables.Add(_dbManTLMainLastClock.ResultsDataTable);

            //// teamleader main
            MWDataManager.clsDataAccess _dbManTeamHist = new MWDataManager.clsDataAccess();
            _dbManTeamHist.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManTeamHist.SqlStatement = " select  '" + org + "'  oo, emp_empno, max(emp_name) name  \r\n" +
                                           " ,max(Day40) Day40, max(Day39) Day39, max(Day38) Day38, max(Day37) Day37, max(Day36) Day36, max(Day35) Day35 \r\n" +
                                           " ,max(Day34) Day34, max(Day33) Day33, max(Day32) Day32, max(Day31) Day31, max(Day30) Day30, max(Day29) Day29 \r\n" +

                                           " ,max(Day28) Day28, max(Day27) Day27, max(Day26) Day26, max(Day25) Day25 \r\n" +
                                           " ,max(Day24) Day24, max(Day23) Day23, max(Day22) Day22, max(Day21) Day21, max(Day20) Day20, max(Day19) Day19 \r\n" +

                                           " ,max(Day18) Day18, max(Day17) Day17, max(Day16) Day16, max(Day15) Day15, \r\n" +
                                           " max(Day14) Day14, max(Day13) Day13, max(Day12) Day12, max(Day11) Day11, max(Day10) Day10 \r\n" +

                                           "  from (SELECT emp_empno, emp_name, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate(), 20) then day2 else '' end as Day40, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-1, 20) then day2 else '' end as Day39, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-2, 20) then day2 else '' end as Day38, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-3, 20) then day2 else '' end as Day37, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-4, 20) then day2 else '' end as Day36, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-5, 20) then day2 else '' end as Day35, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-6, 20) then day2 else '' end as Day34, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-7, 20) then day2 else '' end as Day33, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-8, 20) then day2 else '' end as Day32, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-9, 20) then day2 else '' end as Day31, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-10, 20) then day2 else '' end as Day30, \r\n" +

                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-11, 20) then day2 else '' end as Day29, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-12, 20) then day2 else '' end as Day28, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-13, 20) then day2 else '' end as Day27, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-14, 20) then day2 else '' end as Day26, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-15, 20) then day2 else '' end as Day25, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-16, 20) then day2 else '' end as Day24, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-17, 20) then day2 else '' end as Day23, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-18, 20) then day2 else '' end as Day22, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-19, 20) then day2 else '' end as Day21, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-20, 20) then day2 else '' end as Day20, \r\n" +

                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-21, 20) then day2 else '' end as Day19, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-22, 20) then day2 else '' end as Day18, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-23, 20) then day2 else '' end as Day17, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-24, 20) then day2 else '' end as Day16, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-25, 20) then day2 else '' end as Day15, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-26, 20) then day2 else '' end as Day14, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-27, 20) then day2 else '' end as Day13, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-28, 20) then day2 else '' end as Day12, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-29, 20) then day2 else '' end as Day11, \r\n" +
                                           " case when attendance_date = CONVERT(VARCHAR(10),getdate()-30, 20) then day2 else '' end as Day10 \r\n" +
                                           "  from  tbl_Attendance \r\n" +
                                           "  where   \r\n" +
                                           "  convert(varchar(20),attendance_date)+emp_empno in  \r\n" +
                                           " (select convert(varchar(20),attendance_date)+emp_empno from   \r\n" +
                                           " tbl_Attendance where gang_number = '" + org + "'  ) \r\n" +
                                           " and emp_empno in (select distinct(emp_empno) a from  [tbl_LicenceToOperate_Labour_Import] \r\n" +
                                           " where gang_number = '" + org + "' and wage_Description like '%STOPE TEAM%' \r\n" +
                                           " and CONVERT(VARCHAR(10),clock_time, 20) >= CONVERT(VARCHAR(10),getdate()-30, 20))) a \r\n" +
                                           " group by emp_empno \r\n" +

                                           " order by emp_empno desc \r\n";

            _dbManTeamHist.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManTeamHist.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManTeamHist.ResultsTableName = "TeamHist";
            _dbManTeamHist.ExecuteInstruction();

            DataSet ReportDatasetReport3 = new DataSet();
            ReportDatasetReport3.Tables.Add(_dbManTeamHist.ResultsDataTable);


            string Team = "";

            if (_dbManTeamHist.ResultsDataTable.Rows.Count > 0)
                Team = _dbManTeamHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();


            string userPath = @"http://10.1.1.113/api/resource/GetEmployeeImage?EmployeeNumber=";
            string minpic = string.Empty;
            string tlpic = string.Empty;
            string pic = string.Empty;

            string min = string.Empty;
            string tl = string.Empty;

            if (_dbManMinerLastClock.ResultsDataTable.Rows.Count > 0)
            {
                //pic = _dbManMinerLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                //minpic = userPath + pic;

                min = _dbManMinerLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
            }


            //if (_dbManMinerHist.ResultsDataTable.Rows.Count > 0)
            //{
            //    pic = _dbManMinerHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
            //    minpic = userPath + pic;

            //    min = _dbManMinerHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
            //}



            if (_dbManTLMainLastClock.ResultsDataTable.Rows.Count > 0)
            {
                pic = _dbManTLMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
                tlpic = userPath + pic;
                tl = _dbManTLMainLastClock.ResultsDataTable.Rows[0]["emp_empno"].ToString();
            }

            //if (_dbManTeamHist.ResultsDataTable.Rows.Count > 0)
            //{
            //    pic = _dbManTeamHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
            //    tlpic = userPath + pic;
            //    tl = _dbManTeamHist.ResultsDataTable.Rows[0]["emp_empno"].ToString();
            //}

            //if (VerifyGrid.CurrentRow == null)
            //{
            //    MessageBox.Show("Please select a incident", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}



            MWDataManager.clsDataAccess _dbManGenData = new MWDataManager.clsDataAccess();
            _dbManGenData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManGenData.SqlStatement = " select *, b.employeename mmname, c.employeename tlname \r\n" +
                " from ( select '" + minpic + "' minerpic, '" + tlpic + "' tlpic, \r\n" +
                " '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' mine,  '" + "Team" + "' TeamCompNo,  '" + "Minerpf" + "' MinerCompNo , '" + wp + "' wp, '" + min + "' mm, '" + tl + "' tt \r\n" +
                " ) a  left outer join tbl_EmployeeAll b on a.mm = b.employeeno   left outer join tbl_EmployeeAll c on a.tt = c.employeeno ";
            _dbManGenData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGenData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGenData.ResultsTableName = "GenData";
            _dbManGenData.ExecuteInstruction();

            DataSet dsGenData = new DataSet();
            dsGenData.Tables.Add(_dbManGenData.ResultsDataTable);


            theReport.RegisterData(dsDate);
            theReport.RegisterData(dsMinerToDay);
            theReport.RegisterData(dsManMinerLastClock);
            theReport.RegisterData(dsTeamToDay);
            theReport.RegisterData(dsTLToDayLastClock);
            theReport.RegisterData(dsGenData);

            theReport.Load(ReportsFolder + "\\WPLicToOpLab.frx");
            theReport.Design();
        }

        private void LoadProduction()
        {
            MWDataManager.clsDataAccess _dbManWPDetail = new MWDataManager.clsDataAccess();
            _dbManWPDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            if (Workplace.Substring(0, 2) == "61" || Workplace.Substring(0, 2) == "64" || Workplace.Substring(0, 2) == "68" || Workplace.Substring(0, 2) == "70" || Workplace.Substring(0, 2) == "73" || Workplace.Substring(0, 2) == "74" || Workplace.Substring(0, 2) == "76")
                _dbManWPDetail.SqlStatement = " select 'Sweeps' SweepLbl, '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' Baner, '" + TUserInfo.Name + "' username, *, \r\n " +
                    " (select A_Color from tbl_SysSet) distsupred, (select B_Color from tbl_SysSet) distsupOrange, (select A_Color from tbl_SysSet) distswpred, \r\n" +
                    " (select B_Color from tbl_SysSet) distswpOrange from tbl_Workplace where description = '" + Workplace + "' ";
            else
                _dbManWPDetail.SqlStatement = " select 'Backfill' SweepLbl,'" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' Baner, '" + TUserInfo.Name + "' username, *, \r\n" +
                    " (select A_Color from tbl_SysSet) distsupred, (select B_Color from tbl_SysSet) distsupOrange, (select A_Color from tbl_SysSet) distswpred, \r\n" +
                    " (select B_Color from tbl_SysSet) distswpOrange from tbl_Workplace where description = '" + Workplace + "' ";
            _dbManWPDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPDetail.ResultsTableName = "WorkplaceDetails";
            _dbManWPDetail.ExecuteInstruction();

            DataSet dsWPDetails = new DataSet();
            dsWPDetails.Tables.Add(_dbManWPDetail.ResultsDataTable);

            if (_dbManWPDetail.ResultsDataTable.Rows.Count < 1)
            {
            }
            else
            {
                WPID = _dbManWPDetail.ResultsDataTable.Rows[0]["workplaceid"].ToString();
            }

            MWDataManager.clsDataAccess _dbManPSDetail = new MWDataManager.clsDataAccess();
            _dbManPSDetail.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManPSDetail.SqlStatement = " select pr.ProblemDesc, count(pr.ProblemID) aa from tbl_Planning p, tbl_Code_Problem_Main pr \r\n " +
                    " where p.BookProb = pr.problemid   and p.workplaceid = 'S00288' and calendardate > getdate() - 30  group by pr.ProblemDesc    ";
            _dbManPSDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManPSDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManPSDetail.ResultsTableName = "Planned Stoppage Graph";
            _dbManPSDetail.ExecuteInstruction();

            DataSet dsPSDetails = new DataSet();
            dsPSDetails.Tables.Add(_dbManPSDetail.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManProbGraph = new MWDataManager.clsDataAccess();
            _dbManProbGraph.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManProbGraph.SqlStatement = "  select c.description, count(description) aa  from tbl_Planning p, tbl_Code_Cycle c \r\n " +
                     " where p.BookCode = c.Code  COLLATE Latin1_General_CI_AS and workplaceid = '" + WPID + "' and calendardate > getdate() - 30  group by c.description ";
            _dbManProbGraph.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManProbGraph.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManProbGraph.ResultsTableName = "Problem Graph";
            _dbManProbGraph.ExecuteInstruction();

            DataSet dsProbGraph = new DataSet();
            dsProbGraph.Tables.Add(_dbManProbGraph.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManTempReading = new MWDataManager.clsDataAccess();
            _dbManTempReading.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManTempReading.SqlStatement = "exec [dbo].[sp_LicenceToOperate_TempReadingProd] '" + Workplace + "'";
            _dbManTempReading.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManTempReading.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManTempReading.ResultsTableName = "TempDetails";
            _dbManTempReading.ExecuteInstruction();

            DataSet dsTempReading = new DataSet();
            dsTempReading.Tables.Add(_dbManTempReading.ResultsDataTable);

            theReport.RegisterData(dsWPDetails);
            theReport.RegisterData(dsPSDetails);
            theReport.RegisterData(dsProbGraph);
            theReport.RegisterData(dsTempReading);

            theReport.Load(ReportsFolder + "\\WPLicToOpProd.frx");
        }

        private void LoadMajorHazard()
        {
            MWDataManager.clsDataAccess _dbManTempDetails = new MWDataManager.clsDataAccess();
            _dbManTempDetails.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManTempDetails.SqlStatement = "exec [dbo].[sp_LicenceToOperate_MajHazards] '" + Workplace + "'";
            _dbManTempDetails.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManTempDetails.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManTempDetails.ResultsTableName = "TempDetails";
            _dbManTempDetails.ExecuteInstruction();

            DataSet dsTempDetails = new DataSet();
            dsTempDetails.Tables.Add(_dbManTempDetails.ResultsDataTable);

            MWDataManager.clsDataAccess _dbManHeader = new MWDataManager.clsDataAccess();
            _dbManHeader.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManHeader.SqlStatement = "select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
            _dbManHeader.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManHeader.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManHeader.ResultsTableName = "Header";
            _dbManHeader.ExecuteInstruction();

            DataSet dsHeader = new DataSet();
            dsHeader.Tables.Add(_dbManHeader.ResultsDataTable);


            //MWDataManager.clsDataAccess _dbManTempReading = new MWDataManager.clsDataAccess();
            //_dbManTempReading.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            //_dbManTempReading.SqlStatement = "Select '' P --exec [dbo].[sp_LicenceToOperate_TempReading] '" + Workplace + "'";
            //_dbManTempReading.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbManTempReading.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbManTempReading.ResultsTableName = "TempDetails";
            //_dbManTempReading.ExecuteInstruction();

            //DataSet dsTempReading = new DataSet();
            //dsTempReading.Tables.Add(_dbManTempReading.ResultsDataTable);


            MWDataManager.clsDataAccess _dbManPaperless = new MWDataManager.clsDataAccess();
            _dbManPaperless.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManPaperless.SqlStatement = "select Start_Date Datesubmitted,criticalControl subtaskname,Action_Parent_Type stepname,action_status Actionstatus from tbl_Incidents \r\n"
                        + " where Start_Date > getdate()-30 and Action_Parent_Type like '%tempe%' and workplace = '" + Workplace + "' \r\n"
                        + " and action_status not in ( 'Not A Deficiency', 'Not Applicable' ) ";
            _dbManPaperless.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManPaperless.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManPaperless.ResultsTableName = "Paperless";
            _dbManPaperless.ExecuteInstruction();

            DataSet dsPaperless = new DataSet();
            dsPaperless.Tables.Add(_dbManPaperless.ResultsDataTable);


            MWDataManager.clsDataAccess _dbManHeaderWEC = new MWDataManager.clsDataAccess();
            _dbManHeaderWEC.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManHeaderWEC.SqlStatement = "select '" + ProductionGlobal.ProductionGlobalTSysSettings._Banner + "' mine, '" + Workplace + "' workplace ";
            _dbManHeaderWEC.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManHeaderWEC.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManHeaderWEC.ResultsTableName = "HeaderWEC";
            _dbManHeaderWEC.ExecuteInstruction();

            DataSet dsHeaderWEC = new DataSet();
            dsHeaderWEC.Tables.Add(_dbManHeaderWEC.ResultsDataTable);

            theReport.RegisterData(dsTempDetails);
            theReport.RegisterData(dsHeader);
            //MajHazRep.RegisterData(dsTempReading);
            theReport.RegisterData(dsPaperless);
            theReport.RegisterData(dsHeaderWEC);

            theReport.Load(ReportsFolder + "\\WPLicToOpMajHazard.frx");

        }

        private void gvLTODev_DoubleClick(object sender, EventArgs e)
        {
            Workplace = gvLTODev.GetRowCellValue(gvLTODev.FocusedRowHandle, gvLTOStope.Columns["WPDescription"]).ToString();
            SectionID = gvLTODev.GetRowCellValue(gvLTODev.FocusedRowHandle, gvLTOStope.Columns["MOSection"]).ToString();
            SelectedCol = gvLTODev.FocusedColumn.Caption;

            if (SelectedCol == "Rock Eng Dept.")
            {
                LoadRockEng();

            }

            if (SelectedCol == "Geo Dept.")
            {
                LoadGeologyReport();
            }

            if (SelectedCol == "Vent Dept.")
            {
                LoadVentReport();
            }

            if (SelectedCol == "Critical Skills")
            {
                LoadCriticalSkills();
            }

            if (SelectedCol == "Major Hazards")
            {
                LoadMajorHazard();
            }


            if (SelectedCol == "Prod.")
            {
                LoadProduction();
            }


            
        }

        private void LoadImage(string _Department)
        {
            if (Directory.Exists((@Picture + "\\" + _Department)))
            {
                System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(@Picture + "\\" + _Department);
                string[] files = System.IO.Directory.GetFiles(@Picture + "\\" + _Department);

                foreach (var item in files)
                {
                    string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                    int extpos = aa.IndexOf(".");

                    string ext = aa.Substring(extpos, aa.Length - extpos);

                    if (item == @Picture + "\\" + _Department + string.Empty + "\\" + Workplace + ext)
                    {
                        Attachment = item;
                    }
                }
                if (Attachment == string.Empty || Attachment == null)
                {
                    Attachment = "NoImage";
                }
            }
        }

    }
}
