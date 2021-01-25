using DevExpress.XtraEditors;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
//using Mineware.Systems.HarmonyPASGlobal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting
{
    public partial class ucMeasList : BaseUserControl
    {
        #region Fields and Properties 
        string _FirstLoad = "Y";
        private Report _theReport = new Report();
        private Report _theReportError = new Report();
        DataTable dtReport = new DataTable();
        DataTable dtGetDate = new DataTable();
        /// <summary>
        /// The report to show
        /// </summary>


        /// <summary>
        /// The folder name where the reports are located
        /// </summary>
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

#if DEBUG
        private bool _designReport = true;
#else
        private bool _designReport = false;
#endif
        #endregion Fields and Properties  

        #region Constructor
        public ucMeasList()
        {
            InitializeComponent();            
            FormRibbonPages.Add(rpMeasList);
            FormActiveRibbonPage = rpMeasList;
            FormMainRibbonPage = rpMeasList;
            RibbonControl = rcMeaslist;
        }
        #endregion Constructor

        #region Events
        

        private void ucMeasList_Load(object sender, EventArgs e)
        {
            ProdMonth1Txt.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobalTSysSettings._currentProductionMonth.ToString());

            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
            _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManMonth.SqlStatement = " select getdate() aa ";
            _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMonth.ResultsTableName = "aa";  //get table name
            _dbManMonth.ExecuteInstruction();

            dtGetDate = _dbManMonth.ResultsDataTable;
            //dateTime.Value = Convert.ToDateTime(dtGetDate.Rows[0]["aa"].ToString());

            LoadSections();
            //cmbSections.SelectedText = "Stoping";
            _FirstLoad = "N";

            LoadMeasuringList();
        }

       

      
        private void showBtn_Click(object sender, EventArgs e)
        {
            if (SelectGroup.EditValue.ToString() == "Vamping")
                DoVamping();


            //if (TUserInfo.MOMeas == "Y")
            //    btnAddWP.Visible = true;

            //if (SelectGroup.SelectedIndex == 1)
            //    btnAddWP.Visible = false;

            //string Section = procs.ExtractBeforeColon(cmbSections.EditValue.ToString());
            string Section = cmbSections.EditValue.ToString();
            string month = ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(ProdMonth1Txt.EditValue)).ToString();
            //  _theReport1.Prepare();

            // do check
            MWDataManager.clsDataAccess _dbManCheck = new MWDataManager.clsDataAccess();
            if (SelectGroup.EditValue.ToString() == "Stoping")
            {
                //MWDataManager.clsDataAccess _dbManCheck = new MWDataManager.clsDataAccess();
                _dbManCheck.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManCheck.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, '" + cmbSections.EditValue + "' Section, '" + ProdMonth1Txt.EditValue + "' month1, aa,  bb from (  " +

                                            // miner not attached                                                            
                                            " select 'Section ' +s.SectionID+ ' Has no Miner attached' aa, \r\n" +
                                            " 2 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (s.pfnumber = '' or s.pfnumber = '-' ) \r\n" +

                                            // SB not attached 
                                            "union " +
                                            "select 'Section ' +s1.SectionID+ ' Has no Shift Overseer attached' aa,  \r\n" +
                                            " 1 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (s1.pfnumber = '' or s1.pfnumber = '-' ) \r\n" +

                                             // no mpras
                                             //"union " +
                                             //"select 'Workplace ' +w.Description +' Has no MPRAS Number (ExtID. ' +wt.WPExternalid+')' aa, \r\n" +
                                             //" 4 bb " +
                                             //"from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                             //"where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                             //"and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                             //"and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                             //"and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                             //"and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (WpSublocation = '' ) \r\n" +

                                             // no gang
                                             "union " +
                                            "select 'Workplace ' +w.Description +' Has no gang attached  (ExtID. ' +wt.WPExternalid+')' aa, \r\n " +
                                            " 3  bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (OrgUnitDS = '' ) \r\n" +



                                           // next

                                           "union " +



                                            // miner not attached                                                            
                                            " select 'Section ' +s.SectionID+ ' Has no Miner attached' aa, \r\n" +
                                            " 2 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (s.pfnumber = '' or s.pfnumber = '-' ) \r\n" +

                                            // SB not attached 
                                            "union " +
                                            "select 'Section ' +s1.SectionID+ ' Has no Shift Overseer attached' aa,  \r\n" +
                                            " 1 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (s1.pfnumber = '' or s1.pfnumber = '-' ) \r\n" +

                                             // no mpras
                                             //"union " +
                                             //"select 'Workplace ' +w.Description +' Has no MPRAS Number (ExtID. ' +wt.WPExternalid+')' aa, \r\n" +
                                             //" 4 bb " +

                                             //"from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                             //"where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                             //"and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                             //"and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                             //"and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                             //"and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (WpSublocation = '' ) \r\n" +

                                             // no gang
                                             "union " +
                                            "select 'Workplace ' +w.Description +' Has no gang attached  (ExtID. ' +wt.WPExternalid+')' aa, \r\n " +
                                            " 3  bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and (crew = '' ) \r\n" +
                                            ") a " +
                                            "where aa <> ''  ";
                _dbManCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCheck.ResultsTableName = "MeasureList1";  //get table name
                _dbManCheck.ExecuteInstruction();


            }


            if (SelectGroup.EditValue.ToString() == "Development")
            {

                //MWDataManager.clsDataAccess _dbManCheck = new MWDataManager.clsDataAccess();
                _dbManCheck.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManCheck.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, '" + cmbSections.EditValue + "' Section, '" + ProdMonth1Txt.EditValue + "' month1, aa,  bb from (  " +

                                            // miner not attached                                                            
                                            " select 'Section ' +s.SectionID+ ' Has no Miner attached' aa, \r\n" +
                                            " 2 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (s.pfnumber = '' or s.pfnumber = '-' ) \r\n" +

                                            // SB not attached 
                                            "union " +
                                            "select 'Section ' +s1.SectionID+ ' Has no Shift Overseer attached' aa,  \r\n" +
                                            " 1 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (s1.pfnumber = '' or s1.pfnumber = '-' ) \r\n" +

                                             // no mpras
                                             //"union " +
                                             //"select 'Workplace ' +w.Description +' Has no MPRAS Number (ExtID. ' +wt.WPExternalid+')' aa, \r\n" +
                                             //" 4 bb " +
                                             //"from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                             //"where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                             //"and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                             //"and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                             //"and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                             //"and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (WpSublocation = '' ) \r\n" +

                                             // no gang
                                             "union " +
                                            "select 'Workplace ' +w.Description +' Has no gang attached  (ExtID. ' +wt.WPExternalid+')' aa, \r\n " +
                                            " 3  bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (OrgUnitDS = '' ) \r\n" +



                                           // next

                                           "union " +



                                            // miner not attached                                                            
                                            " select 'Section ' +s.SectionID+ ' Has no Miner attached' aa, \r\n" +
                                            " 2 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (s.pfnumber = '' or s.pfnumber = '-' ) \r\n" +

                                            // SB not attached 
                                            "union " +
                                            "select 'Section ' +s1.SectionID+ ' Has no Shift Overseer attached' aa,  \r\n" +
                                            " 1 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (s1.pfnumber = '' or s1.pfnumber = '-' ) \r\n" +

                                             // no mpras
                                             //"union " +
                                             //"select 'Workplace ' +w.Description +' Has no MPRAS Number (ExtID. ' +wt.WPExternalid+')' aa, \r\n" +
                                             //" 4 bb " +

                                             //"from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                             //"where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                             //"and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                             //"and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                             //"and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                             //"and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (WpSublocation = '' ) \r\n" +

                                             // no gang
                                             "union " +
                                            "select 'Workplace ' +w.Description +' Has no gang attached  (ExtID. ' +wt.WPExternalid+')' aa, \r\n " +
                                            " 3  bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and (crew = '' ) \r\n" +
                                            ") a " +
                                            "where aa <> ''  ";
                _dbManCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCheck.ResultsTableName = "MeasureList1";  //get table name
                _dbManCheck.ExecuteInstruction();

            }



            string cansave = "N";

            //if (TUserInfo.MOMeas == "Y")
            //    cansave = "Y";


            if (SelectGroup.EditValue.ToString() != "Vamping")
            {
                dtGetDate = _dbManCheck.ResultsDataTable;


                // if (SelectGroup.SelectedIndex < 2)
                //{
                if (dtGetDate.Rows.Count > 0)
                {
                    cansave = "N";
                }
            }




            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();

            if (SelectGroup.EditValue.ToString() != "Vamping")
            {
                //MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
                _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManMonth.SqlStatement = "select MAX(calendardate) dd from tbl_Planning p, tbl_Section s, tbl_Section s1 " +
                                            "where p.SectionID = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and p.Prodmonth = '" + month + "' and s1.ReportToSectionid = '" + Section + "'  " +

                                            "and workingday = 'Y' " +


                                            "and calendardate <  " +
                                            "( " +
                                            "select MAX(calendardate) dd from tbl_Planning p, tbl_Section s, tbl_Section s1 " +
                                            "where p.SectionID = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and p.Prodmonth = '" + month + "' and s1.ReportToSectionid = '" + Section + "' and workingday = 'Y'  " +
                                            "and calendardate <  " +
                                            "( " +
                                            "select MAX(calendardate) dd from tbl_Planning p, tbl_Section s, tbl_Section s1 " +
                                            "where p.SectionID = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and p.Prodmonth = '" + month + "' and s1.ReportToSectionid = '" + Section + "' and workingday = 'Y' " +

                                            ") " +

                                            ") ";
                _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMonth.ResultsTableName = "MeasureMonth";  //get table name
                _dbManMonth.ExecuteInstruction();
            }
            //else
            //{
            //    // MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
            //    _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //    _dbManMonth.SqlStatement = "select MAX(calendardate) dd from tbl_Planning_Vamping p, tbl_Section s, tbl_Section s1 " +
            //                                "where p.SectionID = s.SectionID and p.Prodmonth = s.Prodmonth " +
            //                                "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
            //                                "and p.Prodmonth = '" + month + "' and s1.ReportToSectionid = '" + Section + "'  ";
            //    _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManMonth.ResultsTableName = "MeasureMonth";  //get table name
            //    _dbManMonth.ExecuteInstruction();

            //}





            string PastMonthEnd = "N";
            if (SelectGroup.EditValue.ToString() != "Vamping")
            {

                if (_dbManMonth.ResultsDataTable.Rows.Count < 1)
                    return;


                //if (Convert.ToDateTime(_dbManMonth.ResultsDataTable.Rows[0][0]) < dateTime.Value.AddDays(-2))//DateTime.Now)
                //{
                //    PastMonthEnd = "Y";
                //    cansave = "N";
                //    MessageBox.Show("Prodmonth has closed data will not be transfered", "Enter Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}


                if (PastMonthEnd != "Y")
                {
                    // get mosec
                    if (SelectGroup.EditValue.ToString() == "Stoping")
                    {

                        MWDataManager.clsDataAccess _dbManLosses1 = new MWDataManager.clsDataAccess();
                        _dbManLosses1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                        _dbManLosses1.SqlStatement = " insert into [tbl_MOMeasList] " +
                                                       "select " +
                                                       "p.Prodmonth, s.Sectionid, Workplaceid, Activity, 1 Tick1,1 Tick2,1 Tick3,1 Tick4,1 Tick5,1 Tick6,1 Tick7, 0 Printed  " +
                                                       ", sss.nodeid, sUBSTRING(OrgUnitDS,1,15), '' MONotes, 'Y', '" + cansave + "', GETDATE()  " +
                                                       "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Section sss " +
                                                       "where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and " +
                                                       "s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                                                       "s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                                                       "s2.SectionID = sss.sectionid and s.Prodmonth = sss.prodmonth and " +
                                                       "p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "' and activity <> 1" +
                                                       "and workplaceid + convert(Varchar(10),convert(int,activity))+p.sectionid+sUBSTRING(OrgUnitDS,1,15) not in ( " +



                                                       "select workplaceid + convert(Varchar(10),activity)+p.sectionid+crew  a from tbl_MOMeasList p, tbl_Section s, tbl_Section s1, tbl_Section s2 " +
                                                       "where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and  " +
                                                       "s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                                                       "s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                                                       "p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "') ";
                        _dbManLosses1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbManLosses1.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _dbManLosses1.ResultsTableName = "MeasureList1";  //get table name
                        _dbManLosses1.ExecuteInstruction();



                        MWDataManager.clsDataAccess _dbManLosses12 = new MWDataManager.clsDataAccess();
                        _dbManLosses12.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                        _dbManLosses12.SqlStatement = " insert into [tbl_MOMeasListDetailAudit] " +
                                                       "select " +
                                                       "p.Prodmonth, s.Sectionid, Workplaceid, Activity, 1 Tick1,1 Tick2,1 Tick3,1 Tick4,1 Tick5,1 Tick6,1 Tick7, 0 Printed  " +
                                                       ", sss.nodeid, sUBSTRING(OrgUnitDS,1,15), '' MONotes, 'Y', '" + cansave + "', GETDATE(), GETDATE(),   '" + TUserInfo.UserID + "' " +
                                                       "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Section sss " +
                                                       "where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and " +
                                                       "s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                                                       "s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                                                       "s2.SectionID = sss.sectionid and s.Prodmonth = sss.prodmonth and " +
                                                       "p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "' and activity <> 1";
                        //  "and workplaceid + convert(Varchar(10),convert(int,activity))+p.sectionid+sUBSTRING(OrgUnitDS,1,15) not in ( " +



                        //"select workplaceid + convert(Varchar(10),activity)+p.sectionid+crew  a from MOMeasList p, tbl_Section s, tbl_Section s1, tbl_Section s2 " +
                        // "where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and  " +
                        // "s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                        // "s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                        //  "p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "') ";
                        _dbManLosses12.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbManLosses12.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _dbManLosses12.ResultsTableName = "MeasureList1";  //get table name
                        _dbManLosses12.ExecuteInstruction();
                    }

                    if (SelectGroup.EditValue.ToString() == "Development")
                    {

                        MWDataManager.clsDataAccess _dbManLosses1 = new MWDataManager.clsDataAccess();
                        _dbManLosses1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                        _dbManLosses1.SqlStatement = " insert into [tbl_MOMeasListDetailAudit] " +
                                                       "select " +
                                                       "p.Prodmonth, s.Sectionid, Workplaceid, Activity, 1 Tick1,1 Tick2,1 Tick3,1 Tick4,1 Tick5,1 Tick6,1 Tick7, 0 Printed  " +
                                                       ", sss.nodeid, sUBSTRING(OrgUnitDS,1,15), '' MONotes, 'Y', '" + cansave + "', GETDATE(), GETDATE(),   '" + TUserInfo.UserID + "'  " +
                                                       "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Section sss " +
                                                       "where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and " +
                                                       "s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                                                       "s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                                                       "s2.SectionID = sss.sectionid and s.Prodmonth = sss.prodmonth and " +
                                                       "p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "' and activity = 1";
                        // "and workplaceid + convert(Varchar(10),convert(int,activity))+p.sectionid+sUBSTRING(OrgUnitDS,1,15) not in ( " +



                        // "select workplaceid + convert(Varchar(10),activity)+p.sectionid+crew  a from MOMeasList p, tbl_Section s, tbl_Section s1, tbl_Section s2 " +
                        // "where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and  " +
                        // "s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                        // "s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                        // "p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "') ";
                        _dbManLosses1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbManLosses1.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _dbManLosses1.ResultsTableName = "MeasureList1";  //get table name
                        _dbManLosses1.ExecuteInstruction();


                        MWDataManager.clsDataAccess _dbManLosses12 = new MWDataManager.clsDataAccess();
                        _dbManLosses12.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                        _dbManLosses12.SqlStatement = " insert into [tbl_MOMeasList] " +
                                                       "select " +
                                                       "p.Prodmonth, s.Sectionid, Workplaceid, Activity, 1 Tick1,1 Tick2,1 Tick3,1 Tick4,1 Tick5,1 Tick6,1 Tick7, 0 Printed  " +
                                                       ", sss.nodeid, sUBSTRING(OrgUnitDS,1,15), '' MONotes, 'Y', '" + cansave + "', GETDATE()  " +
                                                       "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Section sss " +
                                                       "where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and " +
                                                       "s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                                                       "s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                                                       "s2.SectionID = sss.sectionid and s.Prodmonth = sss.prodmonth and " +
                                                       "p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "' and activity = 1" +
                                                       "and workplaceid + convert(Varchar(10),convert(int,activity))+p.sectionid+sUBSTRING(OrgUnitDS,1,15) not in ( " +



                                                       "select workplaceid + convert(Varchar(10),activity)+p.sectionid+crew  a from tbl_MOMeasList p, tbl_Section s, tbl_Section s1, tbl_Section s2 " +
                                                       "where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and  " +
                                                       "s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                                                       "s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                                                       "p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "') ";
                        _dbManLosses12.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbManLosses12.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _dbManLosses12.ResultsTableName = "MeasureList1";  //get table name
                        _dbManLosses12.ExecuteInstruction();
                    }


                    // update to delete by changing latest to N

                    MWDataManager.clsDataAccess _dbManupdate = new MWDataManager.clsDataAccess();

                    if (SelectGroup.EditValue.ToString() == "Stoping")
                    {
                        _dbManupdate.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                        _dbManupdate.SqlStatement = "update tbl_MOMeasList set islatest = 'N' , auth = 'N' , thedate = getdate() " +
                                                    "where PRODMONTH = '" + month + "' and  workplaceid + convert(Varchar(10),activity)+sectionid + Crew in " +
                                                    "  (select workplaceid + convert(Varchar(10),activity)+p.sectionid + Crew a from tbl_MOMeasList p, tbl_Section s,  " +
                                                    "  tbl_Section s1, tbl_Section s2 where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and   " +
                                                     " s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and s1.prodmonth = s2.prodmonth  " +
                                                     " and s1.reporttosectionid = s2.sectionid and p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "' and Activity <> 1 " +
                                                     " and workplaceid + convert(Varchar(10),convert(decimal(5,0),activity))+p.sectionid +Crew " +
                                                    "  not in  ( " +
                                                      "select workplaceid + convert(Varchar(10),convert(decimal(5,0),activity))+p.sectionid +sUBSTRING(OrgUnitDS,1,15)  " +
                                                      "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, sectionTot sss where p.prodmonth = s.prodmonth  " +
                                                      "and p.sectionid = s.sectionid and s.prodmonth = s1.prodmonth  " +
                                                     " and s.reporttosectionid = s1.sectionid and s1.prodmonth = s2.prodmonth  " +
                                                     " and s1.reporttosectionid = s2.sectionid and s2.SectionID = sss.sectionid  " +
                                                     " and s.Prodmonth = sss.prodmonth and p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "' and Activity <> 1)) ";


                        _dbManupdate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbManupdate.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _dbManupdate.ResultsTableName = "MeasureList1";  //get table name
                        _dbManupdate.ExecuteInstruction();


                    }

                    if (SelectGroup.EditValue.ToString() == "Development")
                    {
                        _dbManupdate.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                        _dbManupdate.SqlStatement = " DECLARE @MyList TABLE (Value  varchar(50)) " +

                                                    "insert into @MyList select workplaceid + convert(Varchar(10),convert(decimal(5,0),activity))+p.sectionid +sUBSTRING(OrgUnitDS,1,15) aa  " +
                                                    "  from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, sectionTot sss where p.prodmonth = s.prodmonth  and p.sectionid = s.sectionid  " +
                                                     " and s.prodmonth = s1.prodmonth   and s.reporttosectionid = s1.sectionid and s1.prodmonth = s2.prodmonth    " +
                                                    "  and s1.reporttosectionid = s2.sectionid and s2.SectionID = sss.sectionid   and s.Prodmonth = sss.prodmonth and p.PRODMONTH = '" + month + "' " +
                                                     " and s2.SectionID = '" + Section + "' and Activity = 1 " +




                                                    " update tbl_MOMeasList set islatest = 'N' , auth = 'N' , thedate = getdate() " +
                                                    "where PRODMONTH = '" + month + "' and  workplaceid + convert(Varchar(10),activity)+sectionid + Crew in " +
                                                    "  (select workplaceid + convert(Varchar(10),activity)+p.sectionid + Crew a from tbl_MOMeasList p, tbl_Section s,  " +
                                                    "  tbl_Section s1, tbl_Section s2 where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and   " +
                                                     " s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and s1.prodmonth = s2.prodmonth  " +
                                                     " and s1.reporttosectionid = s2.sectionid and p.PRODMONTH = '" + month + "' and s2.SectionID = '" + Section + "' and Activity = 1 " +
                                                     " and workplaceid + convert(Varchar(10),convert(decimal(5,0),activity))+p.sectionid +Crew " +
                                                    "  not in  ( select * from @MyList)) ";


                        _dbManupdate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbManupdate.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _dbManupdate.ResultsTableName = "MeasureList1";  //get table name
                        _dbManupdate.ExecuteInstruction();


                    }


                    //do transfer

                    MWDataManager.clsDataAccess _dbManupdate1 = new MWDataManager.clsDataAccess();

                    if (SelectGroup.EditValue.ToString() == "Stoping")
                    {
                        // wont work1


                        if (cansave == "Y")
                        {
                            _dbManupdate1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                            _dbManupdate1.SqlStatement = "update tbl_MOMeasList set auth = 'N' " +
                                                        "where SECTIONID in (select SECTIONID from Sections_Complete  " +
                                                        "where PRODMONTH = '" + month + "' and SectionID_2 = '" + Section + "') and PRODMONTH = '" + month + "' and Activity <> 1 ";
                            _dbManupdate1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManupdate1.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManupdate1.ResultsTableName = "MeasureList1";  //get table name
                            _dbManupdate1.ExecuteInstruction();

                            _dbManupdate1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                            _dbManupdate1.SqlStatement = "update tbl_MOMeasList set auth = 'Y'  " +
                                                    "where SECTIONID in (select SECTIONID from Sections_Complete  " +
                                                    "where PRODMONTH = '" + month + "' and SectionID_2 = '" + Section + "') and PRODMONTH = '" + month + "' and islatest = 'Y' and Activity <> 1 ";
                            _dbManupdate1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManupdate1.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManupdate1.ResultsTableName = "MeasureList1";  //get table name
                            _dbManupdate1.ExecuteInstruction();





                            _dbManupdate1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                            _dbManupdate1.SqlStatement = "update tbl_MOMeasAddWorkplaces set auth = 'N' " +
                                                        "where SECTIONID in (select SECTIONID from Sections_Complete  " +
                                                        "where PRODMONTH = '" + month + "' and SectionID_2 = '" + Section + "') and PRODMONTH = '" + month + "' and Activity <> 1 ";
                            _dbManupdate1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManupdate1.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManupdate1.ResultsTableName = "MeasureList1";  //get table name
                            _dbManupdate1.ExecuteInstruction();

                            _dbManupdate1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                            _dbManupdate1.SqlStatement = "update tbl_MOMeasAddWorkplaces set auth = 'Y'  " +
                                                    "where SECTIONID in (select SECTIONID from Sections_Complete  " +
                                                    "where PRODMONTH = '" + month + "' and SectionID_2 = '" + Section + "') and PRODMONTH = '" + month + "' and islatest = 'Y' and Activity <> 1 ";
                            _dbManupdate1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManupdate1.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManupdate1.ResultsTableName = "MeasureList1";  //get table name
                            _dbManupdate1.ExecuteInstruction();

                        }




                    }


                    if (SelectGroup.EditValue.ToString() == "Development")
                    {

                        if (cansave == "Y")
                        {
                            _dbManupdate1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                            _dbManupdate1.SqlStatement = "update tbl_MOMeasList set auth = 'N' " +
                                                        "where SECTIONID in (select SECTIONID from Sections_Complete  " +
                                                        "where PRODMONTH = '" + month + "' and SectionID_2 = '" + Section + "') and PRODMONTH = '" + month + "' and Activity = 1 ";
                            _dbManupdate1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManupdate1.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManupdate1.ResultsTableName = "MeasureList1";  //get table name
                            _dbManupdate1.ExecuteInstruction();


                            _dbManupdate1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                            _dbManupdate1.SqlStatement = "update tbl_MOMeasList set auth = 'Y'  " +
                                                    "where SECTIONID in (select SECTIONID from Sections_Complete  " +
                                                    "where PRODMONTH = '" + month + "' and SectionID_2 = '" + Section + "') and PRODMONTH = '" + month + "' and islatest = 'Y' and Activity = 1 ";
                            _dbManupdate1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                            _dbManupdate1.queryReturnType = MWDataManager.ReturnType.DataTable;
                            _dbManupdate1.ResultsTableName = "MeasureList1";  //get table name
                            _dbManupdate1.ExecuteInstruction();

                        }




                    }

                }
            }








            if (SelectGroup.EditValue.ToString() == "Stoping")
            {

                MWDataManager.clsDataAccess _dbManLosses = new MWDataManager.clsDataAccess();
                _dbManLosses.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManLosses.SqlStatement = "select  a.*, b.tick1, b.tick2, b.tick3, b.tick4, b.tick5, b.tick6, MONotes  from (select s2.sectionid mo, s2.name moname, " +
                                             " s2.PFNumber mono, s1.PFNumber sbno, s.PFNumber minno, '" + month + "' pm, " +


                                            " '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, convert(varchar,convert(int,p.activity))+s.sectionid +':' +p.workplaceid aa, s1.sectionid sbid, s1.name sbname , s.sectionid minid, s.name minname, " +
                                            " p.workplaceid, w.description, case when SUBSTRING(orgunitds, 17 ,150) <> '' then SUBSTRING(orgunitds, 1 ,150) else SUBSTRING(orgunitds, 1 ,150) end as orgunitds, sqmtotal callsqm, p.activity, wt.WpSublocation Mprass, wpexternalid    " +
                                            " from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            " where " +
                                            " p.workplaceid = w.workplaceid and w.GMSIWPID  = wt.GMSIWPID and " +
                                            " p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and " +
                                            " s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                                            " s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                                            " p.activity <> 1 and p.prodmonth = '" + month + "' " +
                                            " and s2.sectionid = '" + Section + "') a " +
                                            " left outer join " +
                                            " (select * from tbl_MOMeasList where prodmonth = '" + month + "' ) b on a.minid = b.sectionid and a.workplaceid = b.workplaceid and a.activity = b.activity " +

                                            " union " +

                                            " select s2.sectionid mo,  s2.name moname , " +
                                            " s2.PFNumber mono, s1.PFNumber sbno, s.PFNumber minno, '" + month + "' pm, " +



                                            "'" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, \r\n " +
                                            " convert(varchar,convert(int,a.activity))+a.sectionid +':' +a.workplaceid aa,  \r\n " +
                                            " s1.sectionid sbid, s1.name sbname, s.sectionid minid, s.name minname, a.WorkplaceID, w.Description, \r\n " +
                                            "   case when SUBSTRING(Crew, 17 ,150) <> '' then SUBSTRING(Crew, 1 ,150) else SUBSTRING(Crew, 1 ,150) end as orgunitds , 0 callsqm, a.activity, wt.WpSublocation Mprass, wpexternalid  , a.tick1, a.tick2, a.tick3, a.tick4, a.tick5, \r\n " +
                                            " a.tick6, MONotes \r\n " +
                                            " from tbl_MOMeasAddWorkplaces a, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt    \r\n " +
                                            " where a.prodmonth = '" + month + "' and w.GMSIWPID  = wt.GMSIWPID \r\n " +

                                             " and a.prodmonth = s.prodmonth and a.sectionid = s.sectionid and  s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and  s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid " +


                                            //" and a.prodmonth = sc.Prodmonth \r\n " +
                                            //" and a.sectionid = sc.SECTIONID \r\n " +
                                            " and a.workplaceid = w.WorkplaceID \r\n " +
                                            "  " +
                                            " and s2.SECTIONID =  '" + Section + "' \r\n " +
                                            " order by a.sbid, a.minid, a.orgunitds  ";
                _dbManLosses.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManLosses.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManLosses.ResultsTableName = "MeasureList";  //get table name
                _dbManLosses.ExecuteInstruction();




                MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
                _dbMana.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMana.SqlStatement = "select '" + cansave + "' cansave, max(calendardate)+1 cc from tbl_Planning a, tbl_Section s, tbl_Section s1, tbl_Section s2  \r\n " +
                                        "where a.prodmonth = s.prodmonth and a.sectionid = s.sectionid and  s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and  s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid \r\n " +
                                        "  and a.prodmonth = '" + month + "' \r\n " +
                                         " and s2.SECTIONID =  '" + Section + "'  ";
                _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMana.ResultsTableName = "MeasureListDate";  //get table name
                _dbMana.ExecuteInstruction();

                if (cansave == "Y")
                {
                    //MessageFrm MsgFrm = new MessageFrm();
                    //MsgFrm.Text = "Record Saved";
                    //Procedures.MsgText = "Measuring List Successfully Transferred To Survey";
                    //MsgFrm.Width = 500;
                    //MsgFrm.Show();
                    XtraMessageBox.Show("Measuring List Successfully Transferred To Survey");
                }


                DataSet ReportDatasetLosses = new DataSet();
                ReportDatasetLosses.Tables.Add(_dbManLosses.ResultsDataTable);
                _theReport.RegisterData(ReportDatasetLosses);

                DataSet ReportDatasetLosses1 = new DataSet();
                ReportDatasetLosses1.Tables.Add(_dbMana.ResultsDataTable);
                _theReport.RegisterData(ReportDatasetLosses1);



                _theReport.SetParameterValue("Bob", "None");
                _theReport.SetParameterValue("Bob2", "None");
                _theReport.SetParameterValue("Bob3", "None");


                if (cansave == "Y")
                {

                    _theReport.Load(_reportFolder + "MeasuringListRepTrans.frx");

                }
                else
                {
                    if (dtGetDate.Rows.Count > 0)
                        _theReport.Load(_reportFolder + "MeasuringListRepError.frx");
                    else
                        _theReport.Load(_reportFolder + "MeasuringListRep.frx");

                }

                //_theReport.Design();

                pcReport2.Clear();
                _theReport.Prepare();
                _theReport.Preview = pcReport2;
                _theReport.ShowPrepared();
                pcReport2.Visible = true;
            }

            if (SelectGroup.EditValue.ToString() == "Development")
            {
                MWDataManager.clsDataAccess _dbManLosses = new MWDataManager.clsDataAccess();
                _dbManLosses.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManLosses.SqlStatement = "select a.*, b.tick1, b.tick2, b.tick3, b.tick4, b.tick5, b.tick6, MONotes  from (select s2.sectionid mo, s2.name moname, " +

                                             " s2.PFNumber mono, s1.PFNumber sbno, s.PFNumber minno, '" + month + "' pm, " +


                                            "'" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, convert(varchar,convert(int,p.activity))+s.sectionid +':' +p.workplaceid aa, s1.sectionid sbid, s1.name sbname , s.sectionid minid, s.name minname, " +
                                            " p.workplaceid, w.description, SUBSTRING(orgunitds, 1 ,150) orgunitds, Adv callAdv, p.activity, wt.WpSublocation Mprass, wpexternalid, w.reefwaste   " +
                                            " from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            " where " +
                                            " p.workplaceid = w.workplaceid and w.GMSIWPID  = wt.GMSIWPID and " +
                                            " p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and " +
                                            " s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and " +
                                            " s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and " +
                                            " p.activity = 1 and p.prodmonth = '" + month + "' " +
                                            " and s2.sectionid = '" + Section + "') a " +
                                            " left outer join " +
                                            " (select * from tbl_MOMeasList where prodmonth = '" + month + "'  and islatest = 'Y' ) b on a.minid = b.sectionid and a.workplaceid = b.workplaceid and a.activity = b.activity " +
                                            " order by a.sbid, a.minid, a.orgunitds  ";
                _dbManLosses.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManLosses.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManLosses.ResultsTableName = "MeasureList";  //get table name
                _dbManLosses.ExecuteInstruction();


                MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
                _dbMana.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMana.SqlStatement = "select '" + cansave + "' cansave, max(calendardate)+1 cc from tbl_Planning a, tbl_Section s, tbl_Section s1, tbl_Section s2  \r\n " +
                                        "where a.prodmonth = s.prodmonth and a.sectionid = s.sectionid and  s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and  s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid \r\n " +
                                        "  and a.prodmonth = '" + ProdMonth1Txt.EditValue + "' \r\n " +
                                         " and s2.SECTIONID =  '" + Section + "'  ";
                _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMana.ResultsTableName = "MeasureListDate";  //get table name
                _dbMana.ExecuteInstruction();

                if (cansave == "Y")
                {
                    //MessageFrm MsgFrm = new MessageFrm();
                    //MsgFrm.Text = "Record Saved";
                    //Procedures.MsgText = "Measuring List Successfully Transferred To Survey";
                    //MsgFrm.Width = 500;
                    //MsgFrm.Show();
                    XtraMessageBox.Show("Measuring List Successfully Transferred To Survey");
                }

                DataSet ReportDatasetLosses = new DataSet();
                ReportDatasetLosses.Tables.Add(_dbManLosses.ResultsDataTable);
                _theReport.RegisterData(ReportDatasetLosses);

                DataSet ReportDatasetLosses1 = new DataSet();
                ReportDatasetLosses1.Tables.Add(_dbMana.ResultsDataTable);
                _theReport.RegisterData(ReportDatasetLosses1);

                _theReport.SetParameterValue("Bob", "None");
                _theReport.SetParameterValue("Bob2", "None");
                _theReport.SetParameterValue("Bob3", "None");

                _theReport.Load(_reportFolder + "MeasureListDevRep.frx");


                if (cansave == "Y")
                {

                    _theReport.Load(_reportFolder + "MeasureListDevRepTrans.frx");

                }
                else
                {
                    if (dtGetDate.Rows.Count > 0)
                        _theReport.Load(_reportFolder + "MeasureListDevRepError.frx");
                    else
                        _theReport.Load(_reportFolder + "MeasureListDevRep.frx");

                }


                // _theReport.Design();

                pcReport2.Clear();
                _theReport.Prepare();
                _theReport.Preview = pcReport2;
                _theReport.ShowPrepared();


            }

            if (SelectGroup.EditValue.ToString() != "Vamping")
            {

                //environmentSettings1.ReportSettings.ReportPrinted += new EventHandler(PrintBtn_Click);
                if (dtGetDate.Rows.Count > 0)
                {

                    DataSet ReportDatasetValid = new DataSet();
                    ReportDatasetValid.Tables.Add(_dbManCheck.ResultsDataTable);
                    _theReport.RegisterData(ReportDatasetValid);


                    _theReport.Load(_reportFolder + "MeasListError.frx");

                    FastReport.Utils.XmlItem item = FastReport.Utils.Config.Root.FindItem("Forms").FindItem("PreviewForm");
                    item.SetProp("Maximized", "0");
                    item.SetProp("Left", Convert.ToString(Right - 800));
                    item.SetProp("Top", "0");
                    item.SetProp("Width", "800");


                    item.SetProp("Height", "650");

                    _theReport.Prepare();
                    _theReport.ShowPrepared();
                    // _theReport1.Preview.ZoomWholePage();

                    cansave = "N";
                }
            }

            //if (SelectGroup.SelectedIndex == 2)
            //    btnAddWP.Visible = false;


            //Application.Idle += new System.EventHandler(this.Application_Idle);
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        #endregion Events 

        #region Methods  
        /// <summary>
        /// Load the report after adding data
        /// </summary>
        /// <param name="report"></param>
        public void LoadReport(Report report)
        {
            _theReport = report;
            _theReport.Preview = pcReport2;

            if (_designReport)
                _theReport.Design();
            else
                _theReport.Show();
        }

        /// <summary>
        /// Used to load data on the report
        /// </summary>
        public void LoadData()
        {
            DataSet ds = new DataSet();
            MWDataManager.clsDataAccess _databaseConnection = new MWDataManager.clsDataAccess();
            _databaseConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _databaseConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _databaseConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _databaseConnection.SqlStatement = "PUT SQL HERE";

            var ActionResult = _databaseConnection.ExecuteInstruction();

            if (ActionResult.success)
            {
                _databaseConnection.ResultsDataTable.TableName = "TABLENAME";
                ds.Tables.Add(_databaseConnection.ResultsDataTable);
            }


            _theReport.RegisterData(ds);

            //Show the report
            LoadReport(_theReport);
        }

        public void LoadMeasuringList()
        {
            string Section = cmbSections.EditValue.ToString();

            string month = ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(ProdMonth1Txt.EditValue)).ToString();

            MWDataManager.clsDataAccess _dbManCheck = new MWDataManager.clsDataAccess();
            if (SelectGroup.EditValue.ToString() == "Stoping")
            {
                //MWDataManager.clsDataAccess _dbManCheck = new MWDataManager.clsDataAccess();
                _dbManCheck.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManCheck.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, '" + cmbSections.EditValue.ToString() + "' Section, '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(ProdMonth1Txt.EditValue)) + "' month1, aa,  bb from (  " +

                                            // miner not attached                                                            
                                            " select 'Section ' +s.SectionID+ ' Has no Miner attached' aa, \r\n" +
                                            " 2 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where   comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and  p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (s.pfnumber = '' or s.pfnumber = '-' ) \r\n" +

                                            // SB not attached 
                                            "union " +
                                            "select 'Section ' +s1.SectionID+ ' Has no Shift Overseer attached' aa,  \r\n" +
                                            " 1 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where  comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (s1.pfnumber = '' or s1.pfnumber = '-' ) \r\n" +

                                            // no mpras
                                            "union " +
                                            "select 'Workplace ' +w.Description +' Has no MPRAS Number (ExtID. ' +wt.WPExternalid+')' aa, \r\n" +
                                            " 4 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where  comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (WpSublocation = '' ) \r\n" +

                                             // no gang
                                             "union " +
                                            "select 'Workplace ' +w.Description +' Has no gang attached  (ExtID. ' +wt.WPExternalid+')' aa, \r\n " +
                                            " 3  bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where  comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (OrgUnitDS = '' ) \r\n" +

                                           // next
                                           "union " +

                                            // miner not attached                                                            
                                            " select 'Section ' +s.SectionID+ ' Has no Miner attached' aa, \r\n" +
                                            " 2 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (s.pfnumber = '' or s.pfnumber = '-' ) \r\n" +

                                            // SB not attached 
                                            "union " +
                                            "select 'Section ' +s1.SectionID+ ' Has no Shift Overseer attached' aa,  \r\n" +
                                            " 1 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (s1.pfnumber = '' or s1.pfnumber = '-' ) \r\n" +

                                            // no mpras
                                            "union " +
                                            "select 'Workplace ' +w.Description +' Has no MPRAS Number (ExtID. ' +wt.WPExternalid+')' aa, \r\n" +
                                            " 4 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and  (WpSublocation = '' ) \r\n" +

                                             // no gang
                                             "union " +
                                            "select 'Workplace ' +w.Description +' Has no gang attached  (ExtID. ' +wt.WPExternalid+')' aa, \r\n " +
                                            " 3  bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity <> 1 and (crew = '' ) \r\n" +
                                            ") a " +
                                            "where aa <> ''  ";
                _dbManCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCheck.ResultsTableName = "MeasureList1";  //get table name
                _dbManCheck.ExecuteInstruction();
            }

            if (SelectGroup.EditValue.ToString() == "Development")
            {
                //MWDataManager.clsDataAccess _dbManCheck = new MWDataManager.clsDataAccess();
                _dbManCheck.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManCheck.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, '" + Section + "' Section, '" + month + "' month1, aa,  bb from (  " +

                                            // miner not attached                                                            
                                            " select 'Section ' +s.SectionID+ ' Has no Miner attached' aa, \r\n" +
                                            " 2 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where  comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (s.pfnumber = '' or s.pfnumber = '-' ) \r\n" +

                                            // SB not attached 
                                            "union " +
                                            "select 'Section ' +s1.SectionID+ ' Has no Shift Overseer attached' aa,  \r\n" +
                                            " 1 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where  comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (s1.pfnumber = '' or s1.pfnumber = '-' ) \r\n" +

                                            // no mpras
                                            "union " +
                                            "select 'Workplace ' +w.Description +' Has no MPRAS Number (ExtID. ' +wt.WPExternalid+')' aa, \r\n" +
                                            " 4 bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where  comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n" +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (WpSublocation = '' ) \r\n" +

                                             // no gang
                                             "union " +
                                            "select 'Workplace ' +w.Description +' Has no gang attached  (ExtID. ' +wt.WPExternalid+')' aa, \r\n " +
                                            " 3  bb " +
                                            "from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            "where  comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth \r\n " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth \r\n" +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth \r\n" +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID \r\n" +
                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (OrgUnitDS = '' ) \r\n" +

                                           // next
                                           "union " +

                                            // miner not attached                                                            
                                            " select 'Section ' +s.SectionID+ ' Has no Miner attached' aa, \r\n" +
                                            " 2 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (s.pfnumber = '' or s.pfnumber = '-' ) \r\n" +

                                            // SB not attached 
                                            "union " +
                                            "select 'Section ' +s1.SectionID+ ' Has no Shift Overseer attached' aa,  \r\n" +
                                            " 1 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (s1.pfnumber = '' or s1.pfnumber = '-' ) \r\n" +

                                            // no mpras
                                            "union " +
                                            "select 'Workplace ' +w.Description +' Has no MPRAS Number (ExtID. ' +wt.WPExternalid+')' aa, \r\n" +
                                            " 4 bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and  (WpSublocation = '' ) \r\n" +

                                             // no gang
                                             "union " +
                                            "select 'Workplace ' +w.Description +' Has no gang attached  (ExtID. ' +wt.WPExternalid+')' aa, \r\n " +
                                            " 3  bb " +

                                            "from tbl_MOMeasAddWorkplaces p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt " +
                                            "where p.Sectionid = s.SectionID and p.Prodmonth = s.Prodmonth " +
                                            "and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth " +
                                            "and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                            "and p.Workplaceid = w.WorkplaceID and w.GMSIWPID = wt.GMSIWPID " +

                                            "and s2.SectionID = '" + Section + "' and p.Prodmonth = '" + month + "' and p.Activity = 1 and (crew = '' ) \r\n" +
                                            ") a " +
                                            "where aa <> ''  ";
                _dbManCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManCheck.ResultsTableName = "MeasureList1";  //get table name
                _dbManCheck.ExecuteInstruction();
            }

            if (SelectGroup.EditValue.ToString() == "Stoping")
            {
                MWDataManager.clsDataAccess _dbManLosses = new MWDataManager.clsDataAccess();
                _dbManLosses.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManLosses.SqlStatement = "select * from (select  a.*, b.tick1, b.tick2, b.tick3, b.tick4, b.tick5, b.tick6, MONotes  from (select s2.sectionid mo, s2.name moname, \r\n" +
                                             " s2.PFNumber mono, s1.PFNumber sbno, s.PFNumber minno, '" + month + "' pm, \r\n" +

                                            " '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, convert(varchar,convert(int,p.activity))+s.sectionid +':' +p.workplaceid aa, s1.sectionid sbid, s1.name sbname , s.sectionid minid, s.name minname, \r\n" +
                                            " p.workplaceid, w.description, case when SUBSTRING(orgunitds,1 ,12) <> '' then SUBSTRING(orgunitds, 1 ,12) else SUBSTRING(orgunitds, 1 ,12) end as orgunitds, sqmtotal callsqm, p.activity, wt.WpSublocation Mprass, wpexternalid    \r\n" +
                                            " from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            " where comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and \r\n" +
                                            " p.workplaceid = w.workplaceid and w.GMSIWPID  = wt.GMSIWPID and \r\n" +
                                            " p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and \r\n" +
                                            " s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and \r\n" +
                                            " s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and \r\n" +
                                            " p.activity <> 1 and p.prodmonth = '" + month + "' \r\n" +
                                            " and s2.sectionid = '" + Section + "') a \r\n" +
                                            " left outer join \r\n" +
                                            " (select * from tbl_MOMeasList where prodmonth = '" + month + "' ) b on a.minid = b.sectionid and a.workplaceid = b.workplaceid and a.activity = b.activity \r\n" +

                                            " union \r\n" +

                                            " select s2.sectionid mo,  s2.name moname , \r\n" +
                                            " s2.PFNumber mono, s1.PFNumber sbno, s.PFNumber minno, '" + month + "' pm, \r\n" +

                                            "'" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, \r\n \r\n" +
                                            " convert(varchar,convert(int,a.activity))+a.sectionid +':' +a.workplaceid aa,  \r\n \r\n" +
                                            " s1.sectionid sbid, s1.name sbname, s.sectionid minid, s.name minname, a.WorkplaceID, w.Description, \r\n " +
                                            "   case when SUBSTRING(Crew, 17 ,150) <> '' then SUBSTRING(Crew, 1 ,150) else SUBSTRING(Crew, 1 ,150) end as orgunitds , 0 callsqm, a.activity, wt.WpSublocation Mprass, wpexternalid  , a.tick1, a.tick2, a.tick3, a.tick4, a.tick5, \r\n " +
                                            " a.tick6, MONotes \r\n " +
                                            " from tbl_MOMeasAddWorkplaces a, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt    \r\n " +
                                            " where a.prodmonth = '" + month + "' and w.GMSIWPID  = wt.GMSIWPID \r\n " +

                                             " and a.prodmonth = s.prodmonth and a.sectionid = s.sectionid and  s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and  s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid " +

                                            " and a.workplaceid = w.WorkplaceID \r\n " +
                                            "  " +
                                            " and s2.SECTIONID =  '" + Section + "' ) a \r\n " +
                                            "  left outer join (  select workplaceid sswpid, sum(sqmtotal) sqmtotal from tbl_Survey_Imported  \r\n " +
                                            " where prodmonth = '" + month + "' group by workplaceid) s on a.Workplaceid = s.sswpid \r\n " +
                                            " order by a.sbid, a.minid, a.orgunitds  ";
                _dbManLosses.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManLosses.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManLosses.ResultsTableName = "MeasureList";  //get table name
                _dbManLosses.ExecuteInstruction();


                MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
                _dbMana.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMana.SqlStatement = "select '' cansave, max(calendardate)+1 cc from tbl_Planning a, tbl_Section s, tbl_Section s1, tbl_Section s2  \r\n " +
                                        "where a.prodmonth = s.prodmonth and a.sectionid = s.sectionid and  s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and  s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid \r\n " +
                                        "  and a.prodmonth = '" + month + "' \r\n " +
                                         " and s2.SECTIONID =  '" + Section + "'  ";
                _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMana.ResultsTableName = "MeasureListDate";  //get table name
                _dbMana.ExecuteInstruction();


                DataSet ReportDatasetLosses = new DataSet();
                ReportDatasetLosses.Tables.Add(_dbManLosses.ResultsDataTable);
                _theReport.RegisterData(ReportDatasetLosses);

                DataSet ReportDatasetLosses1 = new DataSet();
                ReportDatasetLosses1.Tables.Add(_dbMana.ResultsDataTable);
                _theReport.RegisterData(ReportDatasetLosses1);

                _theReport.SetParameterValue("Bob", "None");
                _theReport.SetParameterValue("Bob2", "None");
                _theReport.SetParameterValue("Bob3", "None");

                _theReport.Load(_reportFolder + "MeasuringListRep.frx");

                //_theReport.Design();

                pcReport2.Clear();
                _theReport.Prepare();
                _theReport.Preview = pcReport2;
                _theReport.ShowPrepared();
                pcReport2.Visible = true;
            }

            if (SelectGroup.EditValue.ToString() == "Development")
            {
                MWDataManager.clsDataAccess _dbManLosses = new MWDataManager.clsDataAccess();
                _dbManLosses.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManLosses.SqlStatement = "select a.*, b.tick1, b.tick2, b.tick3, b.tick4, b.tick5, b.tick6, MONotes  from (select s2.sectionid mo, s2.name moname, \r\n" +

                                             " s2.PFNumber mono, s1.PFNumber sbno, s.PFNumber minno, '" + month + "' pm, \r\n" +


                                            "'" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, convert(varchar,convert(int,p.activity))+s.sectionid +':' +p.workplaceid aa, s1.sectionid sbid, s1.name sbname , s.sectionid minid, s.name minname, \r\n" +
                                            " p.workplaceid, w.description, SUBSTRING(orgunitds, 1 ,12) orgunitds, Adv callAdv, p.activity, wt.WpSublocation Mprass, wpexternalid, w.reefwaste  \r\n " +
                                            " from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                                            " where  comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and \r\n" +
                                            " p.workplaceid = w.workplaceid and w.GMSIWPID  = wt.GMSIWPID and \r\n" +
                                            " p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and \r\n" +
                                            " s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and \r\n" +
                                            " s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and \r\n" +
                                            " p.activity = 1 and p.prodmonth = '" + month + "' \r\n" +
                                            " and s2.sectionid = '" + Section + "') a \r\n" +
                                            " left outer join \r\n" +
                                            " (select * from tbl_MOMeasList where prodmonth = '" + month + "'  and islatest = 'Y' ) b on a.minid = b.sectionid and a.workplaceid = b.workplaceid and a.activity = b.activity \r\n" +
                                            " order by a.sbid, a.minid, a.orgunitds  \r\n";
                //_dbManLosses.SqlStatement = "select * from (select  a.*, b.tick1, b.tick2, b.tick3, b.tick4, b.tick5, b.tick6, MONotes  from (select s2.sectionid mo, s2.name moname, \r\n" +
                //                                 " s2.PFNumber mono, s1.PFNumber sbno, s.PFNumber minno, '" + month + "' pm, \r\n" +


                //                                " '" + ProductionAmplatsGlobalTSysSettings._Banner.ToString().ToString() + "' banner, convert(varchar,convert(int,p.activity))+s.sectionid +':' +p.workplaceid aa, s1.sectionid sbid, s1.name sbname , s.sectionid minid, s.name minname, \r\n" +
                //                                " p.workplaceid, w.description, case when SUBSTRING(orgunitds,1 ,12) <> '' then SUBSTRING(orgunitds, 1 ,12) else SUBSTRING(orgunitds, 1 ,12) end as orgunitds, Adv callAdv, p.activity, wt.WpSublocation Mprass, wpexternalid    \r\n" +
                //                                " from tbl_PlanMonth p, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt \r\n" +
                //                                " where comments not in ('Survey Zero Cads Plan','Survey Zero Plan') and \r\n" +
                //                                " p.workplaceid = w.workplaceid and w.GMSIWPID  = wt.GMSIWPID and \r\n" +
                //                                " p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and \r\n" +
                //                                " s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and \r\n" +
                //                                " s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid and \r\n" +
                //                                " p.activity = 1 and p.prodmonth = '" + month + "' \r\n" +
                //                                " and s2.sectionid = '" + Section + "') a \r\n" +
                //                                " left outer join \r\n" +
                //                                " (select * from tbl_MOMeasList where prodmonth = '" + month + "' ) b on a.minid = b.sectionid and a.workplaceid = b.workplaceid and a.activity = b.activity \r\n" +

                //                                " union \r\n" +

                //                                " select s2.sectionid mo,  s2.name moname , \r\n" +
                //                                " s2.PFNumber mono, s1.PFNumber sbno, s.PFNumber minno, '" + month + "' pm, \r\n" +



                //                                "'" + ProductionAmplatsGlobalTSysSettings._Banner.ToString().ToString() + "' banner, \r\n \r\n" +
                //                                " convert(varchar,convert(int,a.activity))+a.sectionid +':' +a.workplaceid aa,  \r\n \r\n" +
                //                                " s1.sectionid sbid, s1.name sbname, s.sectionid minid, s.name minname, a.WorkplaceID, w.Description, \r\n " +
                //                                "   case when SUBSTRING(Crew, 17 ,150) <> '' then SUBSTRING(Crew, 1 ,150) else SUBSTRING(Crew, 1 ,150) end as orgunitds , 0 callsqm, a.activity, wt.WpSublocation Mprass, wpexternalid  , a.tick1, a.tick2, a.tick3, a.tick4, a.tick5, \r\n " +
                //                                " a.tick6, MONotes \r\n " +
                //                                " from tbl_MOMeasAddWorkplaces a, tbl_Section s, tbl_Section s1, tbl_Section s2, tbl_Workplace w, tbl_Workplace_Total wt    \r\n " +
                //                                " where a.prodmonth = '" + month + "' and w.GMSIWPID  = wt.GMSIWPID \r\n " +

                //                                 " and a.prodmonth = s.prodmonth and a.sectionid = s.sectionid and  s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and  s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid " +


                //                                //" and a.prodmonth = sc.Prodmonth \r\n " +
                //                                //" and a.sectionid = sc.SECTIONID \r\n " +
                //                                " and a.workplaceid = w.WorkplaceID \r\n " +
                //                                "  " +
                //                                " and s2.SECTIONID =  '" + Section + "' ) a \r\n " +
                //                                "  left outer join (  select workplaceid sswpid, sum(Adv) TotalAdv from tbl_Survey_ImportedDev  \r\n " +
                //                                " where prodmonth = '" + month + "' group by workplaceid) s on a.Workplaceid = s.sswpid \r\n " +
                //                                " order by a.sbid, a.minid, a.orgunitds  ";

                _dbManLosses.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManLosses.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManLosses.ResultsTableName = "MeasureList";  //get table name
                _dbManLosses.ExecuteInstruction();


                MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
                _dbMana.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMana.SqlStatement = "select '' cansave, max(calendardate)+1 cc from tbl_Planning a, tbl_Section s, tbl_Section s1, tbl_Section s2  \r\n " +
                                        "where a.prodmonth = s.prodmonth and a.sectionid = s.sectionid and  s.prodmonth = s1.prodmonth and s.reporttosectionid = s1.sectionid and  s1.prodmonth = s2.prodmonth and s1.reporttosectionid = s2.sectionid \r\n " +
                                        "  and a.prodmonth = '" + month + "' \r\n " +
                                         " and s2.SECTIONID =  '" + Section + "'  ";
                _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMana.ResultsTableName = "MeasureListDate";  //get table name
                _dbMana.ExecuteInstruction();


                DataSet ReportDatasetLosses = new DataSet();
                ReportDatasetLosses.Tables.Add(_dbManLosses.ResultsDataTable);
                _theReport.RegisterData(ReportDatasetLosses);

                DataSet ReportDatasetLosses1 = new DataSet();
                ReportDatasetLosses1.Tables.Add(_dbMana.ResultsDataTable);
                _theReport.RegisterData(ReportDatasetLosses1);

                _theReport.SetParameterValue("Bob", "None");
                _theReport.SetParameterValue("Bob2", "None");
                _theReport.SetParameterValue("Bob3", "None");

                _theReport.Load(_reportFolder + "MeasureListDevRep.frx");

                //_theReport.Design();

                pcReport2.Clear();
                _theReport.Prepare();
                _theReport.Preview = pcReport2;
                _theReport.ShowPrepared();
                pcReport2.Visible = true;
            }

            if (SelectGroup.EditValue.ToString() != "Vamping")
            {
                dtReport = _dbManCheck.ResultsDataTable;
            }

            if (SelectGroup.EditValue.ToString() != "Vamping")
            {
                // environmentSettings1.ReportSettings.ReportPrinted += new EventHandler(PrintBtn_Click);
                if (dtReport.Rows.Count > 0)
                {
                    DataSet ReportDatasetValid = new DataSet();
                    ReportDatasetValid.Tables.Add(_dbManCheck.ResultsDataTable);
                    _theReportError.RegisterData(ReportDatasetValid);

                    _theReportError.Load(_reportFolder + "MeasListError.frx");

                    //_theReport.Design();

                    FastReport.Utils.XmlItem item = FastReport.Utils.Config.Root.FindItem("Forms").FindItem("PreviewForm");
                    item.SetProp("Maximized", "0");
                    item.SetProp("Left", Convert.ToString(Right - 800));
                    item.SetProp("Top", "0");
                    item.SetProp("Width", "800");
                    item.SetProp("Height", "650");
                    _theReportError.Prepare();
                }
            }

            if (SelectGroup.EditValue.ToString() == "Vamping")
                DoVamping();
        }

        private void DoVamping()
        {
            string Section = cmbSections.EditValue.ToString();
            string month = ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(ProdMonth1Txt.EditValue)).ToString();

            MWDataManager.clsDataAccess _dbManMonth = new MWDataManager.clsDataAccess();
            _dbManMonth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManMonth.SqlStatement = " select '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner, '" + Section + "' Section, '" + month + "' month1, " +
                                        "a.ss +a.wp+':'+Description aa, * from (select v.prodmonth pm, s.SectionID ss, v.WorkplaceID wp, s2.SectionID + ':'+s2.Name mo, " +
                                        "s1.SectionID + ':'+s1.Name SB, s.SectionID + ':'+s.Name miner, " +
                                        "w.Description, wt.WpSublocation Mprass, wpexternalid , OrgUnitDS , " +
                                        "sum(plansqm) plansqm, SUM(BookSqm) booksqm from tbl_Planning_Vamping v, tbl_Workplace w, tbl_Workplace_Total wt, " +
                                        "tbl_Section s, tbl_Section s1, tbl_Section s2 " +
                                        "where v.workplaceid = w.workplaceid and w.GMSIWPID = wt.GMSIWPID  and v.Prodmonth = s.Prodmonth and v.SectionID = s.SectionID " +
                                        "and  s.Prodmonth = s1.Prodmonth and s.ReportToSectionid = s1.SectionID " +
                                        "and  s1.Prodmonth = s2.Prodmonth and s1.ReportToSectionid = s2.SectionID " +

                                        "and s2.sectionid = '" + Section + "' and v.prodmonth = '" + month + "' " +


                                        "group by v.prodmonth, s2.SectionID ,s2.Name , " +
                                        "s1.SectionID ,s1.Name , s.SectionID ,s.Name, s.SectionID, v.WorkplaceID , " +
                                        "w.Description, wt.WpSublocation , wpexternalid , OrgUnitDS) a " +

                                        "left outer join [tbl_MOMeasListVamp] b " +
                                        "on a.pm = b.prodmonth and a.ss = b.SectionID " +
                                        "and a.wp = b.WorkplaceID " +
                                         " " +
                                        "order by a.sb, a.miner, a.description ";
            _dbManMonth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMonth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMonth.ResultsTableName = "MeasureMonth";  //get table name
            _dbManMonth.ExecuteInstruction();

            dtReport = _dbManMonth.ResultsDataTable;


            DataSet ReportDatasetLosses1 = new DataSet();
            ReportDatasetLosses1.Tables.Add(_dbManMonth.ResultsDataTable);
            _theReport.RegisterData(ReportDatasetLosses1);



            _theReport.SetParameterValue("Bob", "None");
            _theReport.SetParameterValue("Bob2", "None");
            _theReport.SetParameterValue("Bob3", "None");

            _theReport.Load(_reportFolder + "MeasuringListVamp.frx");

            //_theReport.Design();



            pcReport2.Clear();
            _theReport.Prepare();
            _theReport.Preview = pcReport2;
            _theReport.ShowPrepared();
            pcReport2.Visible = true;
        }

        private void LoadSections()
        {
            MWDataManager.clsDataAccess _PrePlanningLoadSections = new MWDataManager.clsDataAccess();
            _PrePlanningLoadSections.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _PrePlanningLoadSections.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningLoadSections.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningLoadSections.SqlStatement = " Select moid Sectionid_2,moname Name_2 \r\n" +
                                                    "from [dbo].[tbl_sectioncomplete] \r\n" +
                                                    "where prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(ProdMonth1Txt.EditValue.ToString())) + "' \r\n " +
                                                    " Group By moid,moname \r\n " +
                                                    " Order By moid,moname";
            _PrePlanningLoadSections.ExecuteInstruction();

            DataTable tbl_Sections = _PrePlanningLoadSections.ResultsDataTable;


            LookUpEditSection.DataSource = tbl_Sections;
            LookUpEditSection.DisplayMember = "Name_2";
            LookUpEditSection.ValueMember = "Sectionid_2";

            cmbSections.EditValue = tbl_Sections.Rows[0]["Sectionid_2"].ToString();
        }
        #endregion Methods 

        private void pnlSideBar_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void cmbSections_EditValueChanged_1(object sender, EventArgs e)
        {

        }

        private void ProdMonth1Txt_EditValueChanged_1(object sender, EventArgs e)
        {

        }

        private void SelectGroup_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

        }

        private void accMain_Click(object sender, EventArgs e)
        {

        }

        private void btnShow_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadMeasuringList();
            }
        }

        private void ProdMonth1Txt_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadMeasuringList();
            }
        }

        private void cmbSections_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadMeasuringList();
            }
        }

        private void SelectGroup_EditValueChanged_1(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
            {
                LoadMeasuringList();
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
