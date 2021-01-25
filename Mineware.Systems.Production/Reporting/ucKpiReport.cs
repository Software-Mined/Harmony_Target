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
    public partial class ucKpiReport : BaseUserControl
    {
        #region Fields and Properties           
        private Report _theReport = new Report();
        private Report _theReport1 = new Report();
        private Report _theReport2 = new Report();
        private Report _theReport3 = new Report();
        private Report _theReport4 = new Report();
        private Report _theReport5 = new Report();
        DateTime startdate = DateTime.Now;
        DateTime enddate = DateTime.Now;
        string millmonth = string.Empty;

        BindingSource bs = new BindingSource();
        BindingSource bs1 = new BindingSource();

        DataTable Emp;

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
        public ucKpiReport()
        {
            InitializeComponent();

            //accMain.Header.MouseClick += Header_MouseClick;
            FormRibbonPages.Add(rpKPI);
            FormActiveRibbonPage = rpKPI;
            FormMainRibbonPage = rpKPI;
            RibbonControl = rcKPI;
        }


        #endregion Constructor

        #region Events

        private void ucKpiReport_Load(object sender, EventArgs e)
        {
            //this.Icon = PAS.Properties.Resources.testbutton3;

            trkbarProdMill.Value = 3;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select MAX(calendardate) aa from tbl_Planning where workingday = 'Y' " +
                                    "and calendardate < getdate()-1 ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable Date = _dbMan.ResultsDataTable;

            barDate.EditValue = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Date.Rows[0][0].ToString()));
            //tabKPIReport.SelectedTabPage = tabSummary;

            LoadKpiData();
        }


        private void trkbarProdMill_LocationChanged(object sender, EventArgs e)
        {
            numLbl.Text = trkbarProdMill.Value.ToString();
        }

        private void numLbl_TextChanged(object sender, EventArgs e)
        {
            LoadGraph();
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
            _theReport.Preview = PrevPrvCntrl;

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

        public void LoadKpiData()
        {
            //this.Cursor = Cursors.WaitCursor;

            Label OldLbl = new Label();
            Label NewLbl = new Label();
            Label newstplbl = new Label();


            #region Graphs
           

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " select * from (select MAX(prodmonth) zz from tbl_Planning where CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "') a ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " (select MAX(prodmonth) zzold from tbl_Planning where CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "' ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " and Prodmonth <> (select MAX(prodmonth) zz from tbl_Planning where CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "')) b ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "    ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();


            DataTable SubA = _dbMan.ResultsDataTable;

            OldLbl.Text = SubA.Rows[0]["zzold"].ToString();
            NewLbl.Text = SubA.Rows[0]["zz"].ToString();


            MWDataManager.clsDataAccess _dbMana1 = new MWDataManager.clsDataAccess();
            _dbMana1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMana1.SqlStatement = " select * from (select MAX(prodmonth) zz from tbl_Planning where activity <> 1 and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "') a ";
            _dbMana1.SqlStatement = _dbMana1.SqlStatement + " , ";
            _dbMana1.SqlStatement = _dbMana1.SqlStatement + " (select MAX(prodmonth) zzold from tbl_Planning where activity <> 1 and  CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "' ";
            _dbMana1.SqlStatement = _dbMana1.SqlStatement + " and Prodmonth <> (select MAX(prodmonth) zz from tbl_Planning where activity <> 1 and  CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "')) b ";
            _dbMana1.SqlStatement = _dbMana1.SqlStatement + "    ";
            _dbMana1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMana1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMana1.ExecuteInstruction();


            DataTable SubA1 = _dbMana1.ResultsDataTable;


            newstplbl.Text = SubA1.Rows[0]["zz"].ToString();

            //Procedures procs = new Procedures();
            //procs.ProdMonthCalc(Convert.ToInt32(OldLbl.Text));
            ProductionGlobal.ProductionGlobal.ProdMonthCalc(Convert.ToInt32(OldLbl.Text));
            OldLbl.Text = ProductionGlobal.ProductionGlobal.Prod.ToString();
            //procs.ProdMonthVis(Convert.ToInt32(OldLbl.Text));
            ProductionGlobal.ProductionGlobal.ProdMonthCalc(Convert.ToInt32(OldLbl.Text));

            xtraTabPage1.Text = "Previous Production Month  " + ProductionGlobal.ProductionGlobal.Prod2;
            //lab1.Text = Procedures.Prod2;


            //procs.ProdMonthCalc(Convert.ToInt32(NewLbl.Text));
            ProductionGlobal.ProductionGlobal.ProdMonthCalc(Convert.ToInt32(NewLbl.Text));
            //OldLbl.Text = Procedures.Prod.ToString();
            //procs.ProdMonthVis(Convert.ToInt32(NewLbl.Text));
            ProductionGlobal.ProductionGlobal.ProdMonthCalc(Convert.ToInt32(NewLbl.Text));

            tabCurr.Text = "Current Production Month  " + ProductionGlobal.ProductionGlobal.Prod2;
            // lab2.Text = Procedures.Prod2;

            //tabPage8.Text = "Summary";

            //get old

            MWDataManager.clsDataAccess _dbManold = new MWDataManager.clsDataAccess();
            _dbManold.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManold.SqlStatement = " exec dbo.KPI_Stoping '" + OldLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "', '" + ProductionGlobalTSysSettings._Banner.ToString() + "' ";
            _dbManold.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManold.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManold.ResultsTableName = "Previous";  //get table name
            _dbManold.ExecuteInstruction();

            DataSet ReportDatasetold = new DataSet();
            ReportDatasetold.Tables.Add(_dbManold.ResultsDataTable);
            _theReport.RegisterData(ReportDatasetold);

            MWDataManager.clsDataAccess _dbManoldDev = new MWDataManager.clsDataAccess();
            _dbManoldDev.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManoldDev.SqlStatement = " exec dbo.KPI_Dev '" + OldLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "' ";
            _dbManoldDev.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManoldDev.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManoldDev.ResultsTableName = "PreviousDev";  //get table name
            _dbManoldDev.ExecuteInstruction();

            DataSet ReportDatasetoldDev = new DataSet();
            ReportDatasetoldDev.Tables.Add(_dbManoldDev.ResultsDataTable);
            _theReport.RegisterData(ReportDatasetoldDev);

            _theReport.Load(_reportFolder + "SICold.frx");

            //_theReport.Design();

            PrevPrvCntrl.Clear();
            _theReport.Prepare();
            _theReport.Preview = PrevPrvCntrl;
            _theReport.ShowPrepared();
            PrevPrvCntrl.Visible = true;


            MWDataManager.clsDataAccess _dbManold1 = new MWDataManager.clsDataAccess();
            _dbManold1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManold1.SqlStatement = " exec dbo.KPI_Stoping '" + NewLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "', '" + ProductionGlobalTSysSettings._Banner.ToString() + "'  ";
            _dbManold1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManold1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManold1.ResultsTableName = "Previous";  //get table name
            _dbManold1.ExecuteInstruction();

            DataSet ReportDatasetold1 = new DataSet();
            ReportDatasetold1.Tables.Add(_dbManold1.ResultsDataTable);
            _theReport1.RegisterData(ReportDatasetold1);


            MWDataManager.clsDataAccess _dbManoldDev1 = new MWDataManager.clsDataAccess();
            _dbManoldDev1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManoldDev1.SqlStatement = " exec dbo.KPI_Dev '" + NewLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "' ";
            _dbManoldDev1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManoldDev1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManoldDev1.ResultsTableName = "PreviousDev";  //get table name
            _dbManoldDev1.ExecuteInstruction();

            DataSet ReportDatasetoldDev1 = new DataSet();
            ReportDatasetoldDev1.Tables.Add(_dbManoldDev1.ResultsDataTable);
            _theReport1.RegisterData(ReportDatasetoldDev1);

            _theReport1.Load(_reportFolder + "SICnew.frx");

            // _theReport.Design();

            CurrentPrevCntrl.Clear();
            _theReport1.Prepare();
            _theReport1.Preview = CurrentPrevCntrl;
            _theReport1.ShowPrepared();
            CurrentPrevCntrl.Visible = true;




            MWDataManager.clsDataAccess _dbManoldSum = new MWDataManager.clsDataAccess();
            _dbManoldSum.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManoldSum.SqlStatement = " exec dbo.KPI_Sum  '" + NewLbl.Text + "', '" + OldLbl.Text + "', '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "', '" + ProductionGlobalTSysSettings._Banner.ToString() + "' ";
            _dbManoldSum.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManoldSum.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManoldSum.ResultsTableName = "Sum";  //get table name
            _dbManoldSum.ExecuteInstruction();

            DataSet ReportDatasetoldSum = new DataSet();
            ReportDatasetoldSum.Tables.Add(_dbManoldSum.ResultsDataTable);
            _theReport2.RegisterData(ReportDatasetoldSum);


            MWDataManager.clsDataAccess _dbManSafetyold = new MWDataManager.clsDataAccess();
            _dbManSafetyold.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (ProductionGlobalTSysSettings._Banner.ToString() != "Mponeng")
            {
                _dbManSafetyold.SqlStatement = " select '" + ProductionGlobal.ProductionGlobal.Prod2 + "' lbl, [Inj], count([Inj]) num from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from tbl_sysset) group by [Inj] ";
            }
            else
            {
                _dbManSafetyold.SqlStatement = " select '" + ProductionGlobal.ProductionGlobal.Prod2 + "' lbl,  'All Injuries'  Inj,  count([Inj]) num  from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] " +
                                               "where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from tbl_sysset) and inj not in ('Fatality') " +
                                               "union " +
                                               "select '" + ProductionGlobal.ProductionGlobal.Prod2 + "' lbl,  'Lost Time Injury'  Inj,  count([Inj]) num  from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main]  " +
                                               "where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from tbl_sysset) and inj not in ('Fatality', 'Dressing Case') " +
                                               "union " +
                                               "select '" + ProductionGlobal.ProductionGlobal.Prod2 + "' lbl,  'Serious Injury'  Inj,  count([Inj]) num  from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] " +
                                               "where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from tbl_sysset) and inj not in ('Fatality', 'Dressing Case','LTI (Lost time injury)') ";
                //  "union " +
                //  "select '" + ProductionGlobal.ProductionGlobal.Prod2 + "' lbl,  'Fatality'  Inj,  count([Inj]) num  from [MW_PassStageDB].dbo.[SafetyFigures_Main]  " +
                //  "where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS from sysset) and inj in ('Fatality') ";


            }
            _dbManSafetyold.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSafetyold.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSafetyold.ResultsTableName = "SumSafetyOld";  //get table name
            _dbManSafetyold.ExecuteInstruction();

            DataSet ReportDatasetSafetyOld = new DataSet();
            ReportDatasetSafetyOld.Tables.Add(_dbManSafetyold.ResultsDataTable);
            _theReport2.RegisterData(ReportDatasetSafetyOld);


            MWDataManager.clsDataAccess _dbManSafetyold1 = new MWDataManager.clsDataAccess();
            _dbManSafetyold1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSafetyold1.SqlStatement = " select * from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] where themonth = '" + OldLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS  from tbl_sysset) and [Inj] = 'LTI (Lost time injury)'  ";
            _dbManSafetyold1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSafetyold1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSafetyold1.ResultsTableName = "SumSafetyOld1";  //get table name
            _dbManSafetyold1.ExecuteInstruction();

            DataSet ReportDatasetSafetyOld1 = new DataSet();
            ReportDatasetSafetyOld1.Tables.Add(_dbManSafetyold1.ResultsDataTable);
            _theReport2.RegisterData(ReportDatasetSafetyOld1);

            MWDataManager.clsDataAccess _dbManSafetyold2 = new MWDataManager.clsDataAccess();
            _dbManSafetyold2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSafetyold2.SqlStatement = " select '" + ProductionGlobal.ProductionGlobal.Prod2 + "' lbl, [Inj], count([Inj]) num from [Mineware_Reporting].dbo.[tbl_SafetyFigures_Main] where themonth = '" + NewLbl.Text + "' and [Opp] = (select banner COLLATE Latin1_General_CI_AS  from tbl_sysset) group by [Inj]  ";
            _dbManSafetyold2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSafetyold2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSafetyold2.ResultsTableName = "SumSafetyOld2";  //get table name
            _dbManSafetyold2.ExecuteInstruction();

            DataSet ReportDatasetSafetyOld2 = new DataSet();
            ReportDatasetSafetyOld2.Tables.Add(_dbManSafetyold2.ResultsDataTable);
            _theReport2.RegisterData(ReportDatasetSafetyOld2);



            _theReport2.Load(_reportFolder + "SICSum.frx");

            //_theReport.Design();

            SumPrevCntrl.Clear();
            _theReport2.Prepare();
            _theReport2.Preview = SumPrevCntrl;
            _theReport2.ShowPrepared();
            SumPrevCntrl.Visible = true;

            // get millmonth
            MWDataManager.clsDataAccess _dbManSqm1 = new MWDataManager.clsDataAccess();
            _dbManSqm1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSqm1.SqlStatement = " select * from tbl_CalendarMill where enddate >= '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "' and startdate <= '" + String.Format("{0:yyyy-MM-dd}", barDate.EditValue) + "'  ";
            _dbManSqm1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSqm1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSqm1.ResultsTableName = "SumSqm11111";  //get table name
            _dbManSqm1.ExecuteInstruction();

            if (_dbManSqm1.ResultsDataTable.Rows.Count == 0)
            {
                MessageBox.Show("No Mill month created please contact the system admin");
                return;

            }

            millmonth = _dbManSqm1.ResultsDataTable.Rows[0]["millmonth"].ToString();
            startdate = Convert.ToDateTime(_dbManSqm1.ResultsDataTable.Rows[0][2].ToString());
            enddate = Convert.ToDateTime(_dbManSqm1.ResultsDataTable.Rows[0][3].ToString());

            MWDataManager.clsDataAccess _dbManSqm = new MWDataManager.clsDataAccess();
            _dbManSqm.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSqm.SqlStatement = " select '" + millmonth + "' mmill, '" + ProductionGlobalTSysSettings._Banner.ToString() + "' banner1, a.*, case when plansqm > 0 then plansqmcmgt/plansqm else 0 end as plancmgt, " +
                                       "case when onreefsqm > 0 then Bookcmgt/onreefsqm else onreefsqm end as bookcmgt1  " +
                                       " from (select SUM(sqm) plansqm, SUM(booksqm+AdjSqm) booksqm,  SUM(sqm*Pm.cmgt) plansqmcmgt, sum((convert(decimal(18,0),(OnReefFL*bookadv))+AdjSqm))  onreefsqm, " +
                                       " sum((convert(decimal(18,0),(OnReefFL*bookadv))+AdjSqm)*pm.CMGT)  Bookcmgt," +
                                       "CalendarDate from tbl_Planning p   " +
                                       ", tbl_PlanMonth pm  " +
                                       "where p.prodmonth = pm.prodmonth and p.workplaceid = pm.workplaceid and p.sectionid = pm.sectionid and p.activity = pm.activity and CalendarDate >= (  " +
                                       "select StartDate  from tbl_CalendarMill where MillMonth = '" + millmonth + "') and CalendarDate <= ( " +


                                       "select EndDate from tbl_CalendarMill where MillMonth = '" + millmonth + "') group by CalendarDate) a " +
                                       //"left outer join MILLING m on a.CalendarDate = m.CalendarDate "+



                                       "order by a.CalendarDate  ";
            _dbManSqm.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSqm.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSqm.ResultsTableName = "SumSqm";  //get table name
            _dbManSqm.ExecuteInstruction();

            DataSet ReportDatasetSqm = new DataSet();
            ReportDatasetSqm.Tables.Add(_dbManSqm.ResultsDataTable);
            _theReport3.RegisterData(ReportDatasetSqm);


            // get factors
            MWDataManager.clsDataAccess _dbManSqm2 = new MWDataManager.clsDataAccess();
            _dbManSqm2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSqm2.SqlStatement = " select a.*, isnull(m.actualtons,0) actualtons, isnull(m.concdesp,0) concdesp  from ( " +
                                        "select *, isnull(onreefsqm * tomill,0) tons, " +
                                     " isnull(BookGrams*dens*OffReefFact/100*MCF/100* REC/100,0) cont,  " +
                                      " case when  onreefsqm * tomill > 0 then  (BookGrams*dens*OffReefFact/100*MCF/100* REC/100)/ " +
                                      " (onreefsqm * tomill)*1000 else 0 end as calpulpval " +
                                      "  from  " +
                                      " (select CalendarDate DD , isnull(sum((convert(decimal(18,0),(OnReefFL*bookadv))+AdjSqm)),0) onreefsqm, " +
                                      " isnull(sum((convert(decimal(18,0),(OnReefFL*bookadv))+AdjSqm)*pm.CMGT/100/1000),0) BookGrams " +
                                      " from tbl_Planning p, tbl_PlanMonth pm " +
                                      " where  " +
                                      " p.Prodmonth = pm.Prodmonth and p.SectionID = pm.SectionID and p.WorkplaceID = pm.Workplaceid and p.Activity = pm.Activity and " +

                                      " CalendarDate+10 >= ( " +

                                      " select StartDate  from tbl_CalendarMill " +
                                      " where MillMonth = '" + millmonth + "') and CalendarDate <=  " +
                                      " ( " +

                                      " select EndDate+10 from tbl_CalendarMill " +
                                      " where MillMonth = '" + millmonth + "') group by CalendarDate) a " +
                                     " , " +
                                     " ( select * from tbl_SYSSET_MonthlyFactors   where prodmonth = '" + millmonth + "') b " +
                                      " , (select max(dens) dens from tbl_PlanMonth where Prodmonth = '" + millmonth + "') c " +
                                      ") a left outer join tbl_MILLING m on a.DD = m.CalendarDate " +

                                      " order by a.DD ";
            _dbManSqm2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSqm2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSqm2.ResultsTableName = "SumSqm11111";  //get table name
            _dbManSqm2.ExecuteInstruction();

            //SqmToMill.Text = _dbManSqm2.ResultsDataTable.Rows[0]["tomill"].ToString(); ;


            Emp = _dbManSqm2.ResultsDataTable;
            int xx = Emp.Rows.Count;

            bs.DataSource = Emp;


            LoadGraph();
            #endregion


            // do oreflow
            MWDataManager.clsDataAccess _dbManGetISAfterStart = new MWDataManager.clsDataAccess();
            _dbManGetISAfterStart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetISAfterStart.SqlStatement = " select * from tbl_SYSSET_MonthlyFactors where Prodmonth = '" + millmonth + "'  ";
            _dbManGetISAfterStart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetISAfterStart.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetISAfterStart.ExecuteInstruction();

            DataTable dtSections = _dbManGetISAfterStart.ResultsDataTable;

            //cmbSections.Items.Clear();

            foreach (DataRow dr in dtSections.Rows)
            {
                TramFactlbl.Text = Math.Round(Convert.ToDecimal(dr["tram"].ToString()), 0).ToString(); //  cmbSections.Items.Add(dr["sectionid"].ToString() + ":" + dr["name"].ToString());
            }



            LoadGrid("R");
            LoadGrid_Graph();



            Stoping.Visible = false;

            //Report _theReport = new Report();

            MWDataManager.clsDataAccess _dbManMill = new MWDataManager.clsDataAccess();
            _dbManMill.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManMill.SqlStatement = "select case when a.Heading = 'Plan Tons' then 1 " +
                                  "when a.Heading = 'Book Tons' then 2 " +
                                  "when a.Heading = 'Tram Tons' then 3   " +
                                   "when a.Heading = 'Smart Rail' then 4   " +
                                  "when a.Heading = 'Hoist Tons' then 5  " +
                                  "else 6 end as q, * from  " +
                                  "(select * from tbl_Temp_OreFlow_Reef_Data where userid = '" + TUserInfo.UserID + "') a ";
            _dbManMill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMill.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMill.ResultsTableName = "Reef_Data";
            _dbManMill.ExecuteInstruction();


            string lbl1 = string.Empty;
            string lbl2 = "Progressive";

            //if (AdjRadio.Checked == true)
            lbl1 = "Using Tramming Width " + TramFactlbl.Text + "cm";


            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = "select '" + lbl1 + "' lbl, * from tbl_Temp_OreFlow_Reef_Heading where userid = '" + TUserInfo.UserID + "'";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ResultsTableName = "Reef_Heading";
            _dbMan1.ExecuteInstruction();

            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan2.SqlStatement =
            " select '" + lbl2 + "' llbl, '" + 'R' + "' Type, '" + millmonth + "' MillMonth, *, " +
            "case when progbook = '' then null else progbook end as progbook1, " +
            "case when progtram = '' then null else progtram end as progtram1, " +
            "case when proghoist = '' then null else proghoist end as proghoist1, " +
            "case when progmill = '' then null else progmill end as progmill1, " +
            "case when progmill = '' then null else progplan end as progplan1, " +

            "case when progsrtons = '' then null else progsrtons end as progsrtons1, " +

            " case when substring(calendardate,1,3) in ('01', '15') then substring(calendardate,1,3) + ' ' +  " +
            " substring(calendardate,3,4) " +
            " else substring(calendardate,1,3) end as lbl " +
            " from tbl_Temp_OreFlow_Data_Graph where userid = '" + TUserInfo.UserID + "' order by xxxx";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ResultsTableName = "Graph_Data";
            _dbMan2.ExecuteInstruction();


            MWDataManager.clsDataAccess _dbMan3 = new MWDataManager.clsDataAccess();
            _dbMan3.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan3.SqlStatement = string.Empty;

            //  if (GraphradioGroup.SelectedIndex == 0)
            //  {

            _dbMan3.SqlStatement = _dbMan3.SqlStatement + " select max(progplan1) progplan1, max(progbook1) progbook1, max(progtram1) progtram1 ";
            _dbMan3.SqlStatement = _dbMan3.SqlStatement + ", max(proghoist1) proghoist1, max(progmill1) progmill1 ";
            // }
            // else
            // {
            //     _dbMan3.SqlStatement = _dbMan3.SqlStatement + " select sum(progplan1) progplan1, sum(progbook1) progbook1, sum(progtram1) progtram1 ";
            //     _dbMan3.SqlStatement = _dbMan3.SqlStatement + ", sum(proghoist1) proghoist1, sum(progmill1) progmill1 ";


            //  }

            _dbMan3.SqlStatement = _dbMan3.SqlStatement + "from (select '" + 'R' + "' Type, '" + millmonth + "' MillMonth, *, " +
            "case when progbook = '' then 0.0 else convert(numeric,progbook) end as progbook1, " +
            "case when progtram = '' then 0.0 else convert(numeric,progtram) end as progtram1, " +
            "case when proghoist = '' then 0.0 else convert(numeric,proghoist) end as proghoist1, " +
            "case when progmill = '' then 0.0 else convert(numeric,progmill) end as progmill1, " +
            "case when progmill = '' then 0.0 else convert(numeric,progplan) end as progplan1, " +

            " case when substring(calendardate,1,3) in ('01', '15') then substring(calendardate,1,3) + ' ' +  " +
            " substring(calendardate,3,4) " +
            " else substring(calendardate,1,3) end as lbl " +
            " from tbl_Temp_OreFlow_Data_Graph where userid = '" + TUserInfo.UserID + "') a";
            _dbMan3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3.ResultsTableName = "Graph_Data1";
            _dbMan3.ExecuteInstruction();



            DataSet ds = new DataSet();
            ds.Tables.Add(_dbManMill.ResultsDataTable);
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(_dbMan1.ResultsDataTable);

            DataSet ds2 = new DataSet();
            ds2.Tables.Add(_dbMan2.ResultsDataTable);
            DataSet ds3 = new DataSet();

            DataSet ds4 = new DataSet();
            ds4.Tables.Add(_dbMan3.ResultsDataTable);


            if (_dbMan.ResultsDataTable.Rows.Count < 1)
            {
                MessageBox.Show("There is no data for selected criteria");
                return;
            }

            _theReport4.RegisterData(ds);
            _theReport4.RegisterData(ds1);
            _theReport4.RegisterData(ds2);
            _theReport4.RegisterData(ds4);

            _theReport4.Load(_reportFolder + "OreFlow.frx");

            //_theReport4.Design();


            _theReport4.Prepare();
            _theReport4.Preview = Oreflowpc;
            _theReport4.ShowPrepared();
            Oreflowpc.Visible = true;


            // top20
            //LoadTop20();



            //this.Cursor = Cursors.Default;
            //CycleTab.Visible = true;

        }

        void LoadGraph()
        {
            _theReport.RegisterData(bs, "bs");

            int xx = Emp.Rows.Count;

            DataTable aaaaa = new DataTable();

            aaaaa.Columns.Add();

            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();
            aaaaa.Columns.Add();

            aaaaa.Rows.Add();

            string test = string.Empty;

            int x = 0;
            int x1 = 0;

            int backdays = Convert.ToInt32(numLbl.Text);

            decimal progtons = 0;
            decimal progCont = 0;

            decimal mprogtons = 0;
            decimal mprogCont = 0;

            string neilaa = string.Empty;

            foreach (DataRow dr1 in Emp.Rows)
            {
                if ((x1 > 9) && (x1 < Emp.Rows.Count - 9))
                {
                    test = Emp.Rows[x][0].ToString();
                    aaaaa.Rows[x][0] = (Convert.ToDateTime(Emp.Rows[x1][0])).ToString("dd-MMM");
                    aaaaa.Rows[x][1] = Emp.Rows[x1 - backdays][12];

                    progtons = progtons + Convert.ToDecimal(Emp.Rows[x1 - backdays][12]);
                    aaaaa.Rows[x][2] = progtons.ToString();

                    progCont = progCont + Convert.ToDecimal(Emp.Rows[x1 - backdays][13]);
                    aaaaa.Rows[x][3] = progCont.ToString();

                    aaaaa.Rows[x][4] = "0";

                    if (progtons > 0)
                    {
                        aaaaa.Rows[x][4] = progCont * 1000 / progtons;

                    }


                    if (Convert.ToDecimal(Emp.Rows[x1 - backdays][1]) > 0)
                        aaaaa.Rows[x][5] = Math.Round(Convert.ToDecimal(Emp.Rows[x1 - backdays][2]) * 1000 * 100 / Convert.ToDecimal(Emp.Rows[x1 - backdays][1]), 0);

                    string aa = aaaaa.Rows[x][5].ToString();


                    aaaaa.Rows[x][6] = Math.Round(Convert.ToDecimal(Emp.Rows[x1 - backdays][12]), 0);
                    neilaa = Math.Round(Convert.ToDecimal(Emp.Rows[x1 - backdays][10]), 2).ToString();

                    aaaaa.Rows[x][7] = Math.Round(Convert.ToDecimal(Emp.Rows[x1 - backdays][13]), 2);

                    neilaa = Math.Round(Convert.ToDecimal(Emp.Rows[x1 - backdays][13]), 2).ToString();

                    if (Convert.ToDecimal(Emp.Rows[x1 - backdays][12]) * Convert.ToDecimal(Emp.Rows[x1 - backdays][13]) > 0)
                    {
                        aaaaa.Rows[x][8] = Math.Round(Convert.ToDecimal(Emp.Rows[x1 - backdays][13]) * 1000 / Convert.ToDecimal(Emp.Rows[x1 - backdays][12]), 2);

                    }

                    if (progtons > 0)
                    {
                        aaaaa.Rows[x][9] = Math.Round(progCont * 1000 / progtons, 2);

                    }

                    aaaaa.Rows[x][10] = ProductionGlobalTSysSettings._Banner.ToString();
                    aaaaa.Rows[x][11] = millmonth;
                    aaaaa.Rows[x][12] = backdays;

                    aaaaa.Rows[x][13] = Math.Round(Convert.ToDecimal(Emp.Rows[x1][16]), 2);

                    if ((Convert.ToDecimal(Emp.Rows[x1][16]) * Convert.ToDecimal(Emp.Rows[x1][15])) > 0)
                        mprogtons = mprogtons + Convert.ToDecimal(Emp.Rows[x1][15]);

                    if ((Convert.ToDecimal(Emp.Rows[x1][16]) * Convert.ToDecimal(Emp.Rows[x1][15])) > 0)
                        mprogCont = mprogCont + (Convert.ToDecimal(Emp.Rows[x1][16]) * Convert.ToDecimal(Emp.Rows[x1][15]));

                    aaaaa.Rows[x][14] = "0.00";
                    if (mprogCont * mprogtons > 0)
                        aaaaa.Rows[x][14] = Math.Round(mprogCont / mprogtons, 2);


                    x = x + 1;
                    aaaaa.Rows.Add();
                }
                x1 = x1 + 1;
                // EquipTypeCMB.Items.Add(new RadioGroupItem(x, dr1["EquipType"].ToString()));
            }
            aaaaa.Rows[x].Delete();

            DataSet Headers = new DataSet();
            Headers.Tables.Add(aaaaa);

            _theReport5.RegisterData(Headers);

            //foreach (DataRow r in Neil.Rows)
            //{
            //    Propfrm.PlanWPSection.Items.Add(r["SectionID"].ToString() + ":" + r["name"].ToString());
            //}




            _theReport5.Load(_reportFolder + "SICSumGraph.frx");

            //_theReport.Design();

            GraphReport.Clear();
            _theReport5.Prepare();
            _theReport5.Preview = GraphReport;
            _theReport5.ShowPrepared();


            _theReport5.RegisterData(Headers);
            _theReport5.Load(_reportFolder + "SICSumGraphDetail.frx");

            //_theReport.Design();
            GradesReport.Clear();
            _theReport5.Prepare();
            _theReport5.Preview = GradesReport;
            _theReport5.ShowPrepared();
            GradesReport.Visible = true;
        }

        public void LoadGrid(string Type)
        {
            Cursor = Cursors.WaitCursor;
            Stoping.Dispose();

            DataGridView dt = new DataGridView();

            Stoping = dt;
            Stoping.Parent = pnlOverFlow;
            Stoping.Visible = false;

            string month2 = millmonth;

            Stoping.RowCount = 600;
            Stoping.ColumnCount = 100;

            //Fill grid with blanks
            for (int a = 0; a < 43; a++)
            {
                for (int aa = 0; aa < 9; aa++)
                {
                    if (aa > 0)
                        Stoping.Rows[aa].Cells[a].Value = "0";
                    else
                        Stoping.Rows[aa].Cells[a].Value = string.Empty;
                }
            }

            //Stoping.Rows[0].Cells[0].Value = "Calendar Date";
            Stoping.Rows[1].Cells[0].Value = "Plan Tons";
            Stoping.Rows[2].Cells[0].Value = "Book Tons";
            Stoping.Rows[3].Cells[0].Value = "Tram Tons";
            Stoping.Rows[4].Cells[0].Value = "Smart Rail";
            Stoping.Rows[5].Cells[0].Value = "Hoist Tons";
            Stoping.Rows[6].Cells[0].Value = "Mill Tons";
            Stoping.Rows[7].Cells[0].Value = "Mill Kgs";

            //MessageBox.Show(Type.ToString());







            string fact = string.Empty;


            fact = "Using " + TramFactlbl.Text + "cm Tramming Width";


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "  declare @dens1 decimal(18,5) " +


                                   "set @dens1 = (select max(dens) dd from tbl_PlanMonth where Prodmonth = '" + millmonth + "') " +



                                  "select *,  case when plantons1a is null then 0 else plantons1a end as plantons1ab, case when booktons1a is null then 0 else booktons1a end as booktons1ab, " +


                                    "case when booktons is null then 0 else booktons end as BookTons1,case when plantons is null then 0 else plantons end as plantons," +
                                  " case when tramtons is null then 0 else tramtons end as tramtons1," +
                                  " case when SRTons is null then 0 else SRTons end as SRTons1," +
                                  " case when hoisttons is null then 0 else hoisttons end as hoisttons1," +
                                  " case when milltons is null then 0 else milltons end as milltons1," +
                                  " case when millkg is null then 0 else millkg end as millkg1" +
                                  " from (select 'MINEWARE' UserName, '" + millmonth + "' MillMonth, " +
                                  " calendardate,sum(tons) plantons,  ";
            // if (SysSettings.AdjBook == "Y")
            //{
            _dbMan.SqlStatement = _dbMan.SqlStatement + " sum(booktons + AdjTons) booktons  ";
            // }
            // else
            //  {
            //  _dbMan.SqlStatement = _dbMan.SqlStatement + " sum(booktons) booktons ";
            //  }



            //if (AdjRadio.Checked == true)
            //    _dbMan.SqlStatement = _dbMan.SqlStatement + " ,sum(p.sqm * '" + TramFactlbl.Text + "' * @dens1/100) + sum(Adv/(Adv +0.000000001)* tons) plantons1a,  sum((p.BookSqm+AdjSqm) * '" + TramFactlbl.Text + "' * @dens1/100) + sum(bookadv*BookWidth*BookHeight* @dens1) booktons1a  from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and calendardate >= '" + StartDate.Value + "' ";
            //else

            _dbMan.SqlStatement = _dbMan.SqlStatement + ", sum(tons) plantons1a, sum(booktons) booktons1a from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and calendardate >= '" + startdate + "' ";

            _dbMan.SqlStatement = _dbMan.SqlStatement + " and calendardate <= '" + enddate + "' and w.reefwaste = '" + Type.ToString() + "' group by calendardate) a" +

                                  " left outer join" +
                                  " (select calendardate cal, sum(tons) tramtons from tbl_Booking_Oreflow  " +
                                  " where reefwaste = '" + Type.ToString() + "' and calendardate >= '" + startdate + "' " +
                                  " and calendardate <= '" + enddate + "' and ToOreFlowID like 'M%' group by calendardate) b on a.calendardate = b.cal" +

                                  " left outer join";
            //if (Type == "R")
            _dbMan.SqlStatement = _dbMan.SqlStatement + " (select calendardate cal1, sum(reeftons) hoisttons from tbl_Booking_Hoisting";
            // else
            //     _dbMan.SqlStatement = _dbMan.SqlStatement + " (select calendardate cal1, sum(wastetons) hoisttons from dbo.BOOKINGHoisting";

            _dbMan.SqlStatement = _dbMan.SqlStatement + string.Empty +
            " where calendardate >= '" + startdate + "'" +
            " and calendardate <= '" + enddate + "' group by calendardate) c on a.calendardate = c.cal1" +
            " left outer join";

            //if (Type == "R")
            _dbMan.SqlStatement = _dbMan.SqlStatement + " (select calendardate cal2, sum(actualtons) milltons, sum(kgprod) millkg from tbl_Milling";
            // else
            //     _dbMan.SqlStatement = _dbMan.SqlStatement + " (select calendardate cal2, sum(0) milltons, sum(0) millkg from dbo.milling";

            _dbMan.SqlStatement = _dbMan.SqlStatement + string.Empty +
            " where calendardate >= '" + startdate + "'" +
            " and calendardate <= '" + enddate + "' group by calendardate) d on a.calendardate = d.cal2 " +

            " left outer join (select calendardate cal123, SUM(tons) SRTons from tbl_Booking_SmartRail where calendardate >= '" + startdate + "' and calendardate <= '" + enddate + "' and oreflow = '1' " +
            " group by calendardate) sRail on a.calendardate = sRail.cal123 " +


            " order by calendardate ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt1 = _dbMan.ResultsDataTable;

            double ProgPlan = 0;
            double ProgBook = 0;
            double ProgTram = 0;
            double ProgHoist = 0;
            double ProgMill = 0;
            double ProgMillKg = 0;
            double srail = 0;

            int x = 1;

            foreach (DataRow dr in dt1.Rows)
            {
                Stoping.Rows[0].Cells[x].Value = Convert.ToDateTime(dr["calendardate"].ToString()).ToString("dd MMM ddd");
                Stoping.Rows[1].Cells[x].Value = dr["plantons1ab"].ToString();
                Stoping.Rows[2].Cells[x].Value = dr["booktons1ab"].ToString();
                Stoping.Rows[3].Cells[x].Value = dr["TramTons1"].ToString();

                if (Type == "R")
                    Stoping.Rows[4].Cells[x].Value = dr["srtons1"].ToString();
                else
                    Stoping.Rows[4].Cells[x].Value = "0";
                Stoping.Rows[5].Cells[x].Value = dr["HoistTons1"].ToString();
                Stoping.Rows[6].Cells[x].Value = dr["MillTons1"].ToString();
                Stoping.Rows[7].Cells[x].Value = dr["MillKg1"].ToString();



                ProgPlan = ProgPlan + Convert.ToDouble(dr["plantons1ab"]);


                ProgBook = ProgBook + Convert.ToDouble(dr["booktons1ab"]);
                ProgTram = ProgTram + Convert.ToDouble(dr["TramTons1"]);
                ProgHoist = ProgHoist + Convert.ToDouble(dr["HoistTons1"]);
                ProgMill = ProgMill + Convert.ToDouble(dr["MillTons1"]);
                ProgMillKg = ProgMillKg + Convert.ToDouble(dr["MillKg1"]);

                srail = srail + Convert.ToDouble(dr["srtons1"]);
                if (Type != "R")
                    srail = 0;

                x++;
            }

            Stoping.Rows[0].Cells[41].Value = "Progressive";

            Stoping.Rows[1].Cells[41].Value = ProgPlan;
            Stoping.Rows[2].Cells[41].Value = ProgBook;
            Stoping.Rows[3].Cells[41].Value = ProgTram;

            Stoping.Rows[4].Cells[41].Value = srail;

            Stoping.Rows[5].Cells[41].Value = ProgHoist;
            Stoping.Rows[6].Cells[41].Value = ProgMill;
            Stoping.Rows[7].Cells[41].Value = ProgMillKg;

            Cursor = Cursors.Default;
            Stoping.Dock = DockStyle.Fill;
            Stoping.Visible = true;
            Stoping.BringToFront();

            //Insert Heading
            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan2.SqlStatement = " delete tbl_Temp_OreFlow_Reef_Heading where userid = '" + TUserInfo.UserID.ToString() + "' ";

            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " insert into tbl_Temp_OreFlow_Reef_Heading values(  '" + TUserInfo.UserID + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + ProductionGlobalTSysSettings._Banner.ToString() + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[0].Value + "', '" + Stoping.Rows[0].Cells[1].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[2].Value + "', '" + Stoping.Rows[0].Cells[3].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[4].Value + "', '" + Stoping.Rows[0].Cells[5].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[6].Value + "', '" + Stoping.Rows[0].Cells[7].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[8].Value + "', '" + Stoping.Rows[0].Cells[9].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[10].Value + "', '" + Stoping.Rows[0].Cells[11].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[12].Value + "', '" + Stoping.Rows[0].Cells[13].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[14].Value + "', '" + Stoping.Rows[0].Cells[15].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[16].Value + "', '" + Stoping.Rows[0].Cells[17].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[18].Value + "', '" + Stoping.Rows[0].Cells[19].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[20].Value + "', '" + Stoping.Rows[0].Cells[21].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[22].Value + "', '" + Stoping.Rows[0].Cells[23].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[24].Value + "', '" + Stoping.Rows[0].Cells[25].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[26].Value + "', '" + Stoping.Rows[0].Cells[27].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[28].Value + "', '" + Stoping.Rows[0].Cells[29].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[30].Value + "', '" + Stoping.Rows[0].Cells[31].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[32].Value + "', '" + Stoping.Rows[0].Cells[33].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[34].Value + "', '" + Stoping.Rows[0].Cells[35].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[36].Value + "', '" + Stoping.Rows[0].Cells[37].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[38].Value + "', '" + Stoping.Rows[0].Cells[39].Value + "', ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + " '" + Stoping.Rows[0].Cells[40].Value + "', '" + Stoping.Rows[0].Cells[41].Value + "') ";

            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ExecuteInstruction();

            //Insert Data
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = " delete tbl_Temp_OreFlow_Reef_Data where userid = '" + TUserInfo.UserID.ToString() + "' ";//


            for (int y = 1; y < 7; y++)//rows
            {
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " insert into tbl_Temp_OreFlow_Reef_Data values(  '" + TUserInfo.UserID + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[0].Value + "', '" + Stoping.Rows[y].Cells[1].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[2].Value + "', '" + Stoping.Rows[y].Cells[3].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[4].Value + "', '" + Stoping.Rows[y].Cells[5].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[6].Value + "', '" + Stoping.Rows[y].Cells[7].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[8].Value + "', '" + Stoping.Rows[y].Cells[9].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[10].Value + "', '" + Stoping.Rows[y].Cells[11].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[12].Value + "', '" + Stoping.Rows[y].Cells[13].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[14].Value + "', '" + Stoping.Rows[y].Cells[15].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[16].Value + "', '" + Stoping.Rows[y].Cells[17].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[18].Value + "', '" + Stoping.Rows[y].Cells[19].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[20].Value + "', '" + Stoping.Rows[y].Cells[21].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[22].Value + "', '" + Stoping.Rows[y].Cells[23].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[24].Value + "', '" + Stoping.Rows[y].Cells[25].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[26].Value + "', '" + Stoping.Rows[y].Cells[27].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[28].Value + "', '" + Stoping.Rows[y].Cells[29].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[30].Value + "', '" + Stoping.Rows[y].Cells[31].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[32].Value + "', '" + Stoping.Rows[y].Cells[33].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[34].Value + "', '" + Stoping.Rows[y].Cells[35].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[36].Value + "', '" + Stoping.Rows[y].Cells[37].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[38].Value + "', '" + Stoping.Rows[y].Cells[39].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[40].Value + "', '" + Stoping.Rows[y].Cells[41].Value + "') ";
            }

            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();
        }

        public void LoadGrid_Graph()
        {
            Cursor = Cursors.WaitCursor;
            Stoping.Dispose();

            DataGridView dt = new DataGridView();

            Stoping = dt;
            Stoping.Parent = pnlOverFlow;
            Stoping.Visible = false;

            string month2 = millmonth;

            Stoping.RowCount = 600;
            Stoping.ColumnCount = 100;

            //Fill grid with blanks
            for (int a = 0; a < 13; a++)
            {
                for (int aa = 0; aa < 60; aa++)
                {
                    Stoping.Rows[aa].Cells[a].Value = string.Empty;
                }
            }


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "  declare @dens1 decimal(18,5) " +


                                 "set @dens1 = (select max(dens) dd from tbl_PlanMonth where Prodmonth = '" + millmonth + "') " +



                                "select *,  case when plantons1a is null then 0 else plantons1a end as plantons1ab, case when booktons1a is null then 0 else booktons1a end as booktons1ab, " +


                                  "case when booktons is null then 0 else booktons end as BookTons1,case when plantons is null then 0 else plantons end as plantons," +
                                  " case when tramtons is null then 0 else tramtons end as tramtons1," +
                                   " case when SRTons is null then 0 else SRTons end as SRTons1," +
                                  " case when hoisttons is null then 0 else hoisttons end as hoisttons1," +
                                  " case when milltons is null then 0 else milltons end as milltons1," +
                                  " case when millkg is null then 0 else millkg end as millkg1" +
                                  " from (select 'MINEWARE' UserName, '" + millmonth + "' MillMonth, " +
                                  " calendardate, sum(tons) plantons, ";
            //if (SysSettings.AdjBook == "Y")
            //{
            _dbMan.SqlStatement = _dbMan.SqlStatement + " sum(booktons + AdjTons) booktons  ";
            // }
            // else
            // {
            //   _dbMan.SqlStatement = _dbMan.SqlStatement + " sum(booktons) booktons ";
            //
            // }



            //if (AdjRadio.Checked == true)
            _dbMan.SqlStatement = _dbMan.SqlStatement + " ,sum(p.sqm * '" + TramFactlbl.Text + "' * @dens1/100) + sum(Adv/(Adv +0.000000001)* tons) plantons1a,  sum((p.BookSqm+AdjSqm) * '" + TramFactlbl.Text + "' * @dens1/100) + sum(bookadv*BookWidth*BookHeight* @dens1) booktons1a  from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and calendardate >= '" + startdate + "' ";
            // else

            //     _dbMan.SqlStatement = _dbMan.SqlStatement + ", sum(tons) plantons1a, sum(booktons) booktons1a from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and calendardate >= '" + startdate + "' ";


            // _dbMan.SqlStatement = _dbMan.SqlStatement + " from tbl_Planning p, tbl_Workplace w where p.workplaceid = w.workplaceid and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "' " +
            _dbMan.SqlStatement = _dbMan.SqlStatement + " and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", enddate) + "' and w.reefwaste = '" + 'R' + "' group by calendardate) a" +

                                " left outer join" +
                                " (select calendardate cal, sum(tons) tramtons from tbl_Booking_Oreflow" +
                                " where reefwaste = '" + 'R' + "' and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", startdate) + "' " +
                                " and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", enddate) + "' and ToOreFlowID  like 'M%' group by calendardate) b on a.calendardate = b.cal" +

                                " left outer join";
            //if (Type == "R")
            _dbMan.SqlStatement = _dbMan.SqlStatement + " (select calendardate cal1, sum(reeftons) hoisttons from tbl_Booking_Hoisting";
            // else
            //     _dbMan.SqlStatement = _dbMan.SqlStatement + " (select calendardate cal1, sum(wastetons) hoisttons from dbo.BOOKINGHoisting";

            _dbMan.SqlStatement = _dbMan.SqlStatement + string.Empty +
            " where calendardate >= '" + String.Format("{0:yyyy-MM-dd}", startdate) + "' " +
            " and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", enddate) + "' group by calendardate) c on a.calendardate = c.cal1" +
            " left outer join";

            //if (Type == "R")
            _dbMan.SqlStatement = _dbMan.SqlStatement + " (select calendardate cal2, sum(actualtons) milltons, sum(kgprod) millkg from tbl_Milling";
            // else
            //    _dbMan.SqlStatement = _dbMan.SqlStatement + " (select calendardate cal2, sum(0) milltons, sum(0) millkg from dbo.milling";

            _dbMan.SqlStatement = _dbMan.SqlStatement + string.Empty +
            " where calendardate >= '" + String.Format("{0:yyyy-MM-dd}", startdate) + "' " +
            " and calendardate <= '" + String.Format("{0:yyyy-MM-dd}", enddate) + "' group by calendardate) d on a.calendardate = d.cal2" +

             " left outer join (select calendardate cal123, SUM(tons) SRTons from tbl_Booking_SmartRail where calendardate >= '" + startdate + "' and calendardate <= '" + enddate + "'  and oreflow = '1' " +
            " group by calendardate) sRail on a.calendardate = sRail.cal123 " +


            " order by calendardate ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt1 = _dbMan.ResultsDataTable;

            double ProgPlan = 0;
            double ProgBook = 0;
            double ProgTram = 0;
            double ProgHoist = 0;
            double ProgMill = 0;
            double ProgMillKg = 0;
            double Srail = 0;

            int x = 0;

            foreach (DataRow dr in dt1.Rows)
            {
                Stoping.Rows[x].Cells[0].Value = TUserInfo.UserID.ToString();
                Stoping.Rows[x].Cells[1].Value = Convert.ToDateTime(dr["calendardate"].ToString()).ToString("dd MMM ddd");
                Stoping.Rows[x].Cells[2].Value = dr["plantons1ab"].ToString();
                Stoping.Rows[x].Cells[3].Value = dr["booktons1ab"].ToString();
                Stoping.Rows[x].Cells[4].Value = dr["TramTons1"].ToString();
                //  if (Type == "R")
                Stoping.Rows[x].Cells[5].Value = dr["SRTons1"].ToString();
                //  else
                //     Stoping.Rows[x].Cells[5].Value = "0";

                Stoping.Rows[x].Cells[6].Value = dr["HoistTons1"].ToString();
                Stoping.Rows[x].Cells[7].Value = dr["MillTons1"].ToString();
                Stoping.Rows[x].Cells[8].Value = dr["MillKg1"].ToString();
                // if (GraphradioGroup.SelectedIndex == 0)
                // {
                ProgPlan = ProgPlan + Convert.ToDouble(dr["plantons1ab"]);
                ProgBook = ProgBook + Convert.ToDouble(dr["booktons1ab"]);
                ProgTram = ProgTram + Convert.ToDouble(dr["TramTons1"]);


                ProgHoist = ProgHoist + Convert.ToDouble(dr["HoistTons1"]);
                ProgMill = ProgMill + Convert.ToDouble(dr["MillTons1"]);
                ProgMillKg = ProgMillKg + Convert.ToDouble(dr["MillKg1"]);

                Srail = Srail + Convert.ToDouble(dr["SRTons1"]); ;
                //if (Type != "R")
                //    Srail = 0;
                //      ProgPlan = Convert.ToDouble(dr["plantons1ab"]);
                //      ProgBook = Convert.ToDouble(dr["booktons1ab"]);
                //       ProgTram = Convert.ToDouble(dr["TramTons1"]);
                //      ProgHoist = Convert.ToDouble(dr["HoistTons1"]);
                //       ProgMill = Convert.ToDouble(dr["MillTons1"]);
                //      ProgMillKg = Convert.ToDouble(dr["MillKg1"]);
                //      Srail = Convert.ToDouble(dr["SRTons1"]);
                //       if (Type != "R")
                //           Srail = 0;

                //     }

                Stoping.Rows[x].Cells[8].Value = ProgPlan;
                if (Convert.ToDateTime(dr["calendardate"].ToString()) < DateTime.Now)
                {
                    Stoping.Rows[x].Cells[9].Value = ProgBook;
                    Stoping.Rows[x].Cells[10].Value = ProgTram;
                    Stoping.Rows[x].Cells[11].Value = ProgHoist;
                    Stoping.Rows[x].Cells[12].Value = ProgMill;
                    Stoping.Rows[x].Cells[13].Value = ProgMillKg;


                    Stoping.Rows[x].Cells[14].Value = Srail;
                    Stoping.Rows[x].Cells[15].Value = "0";
                }

                x++;
            }

            Cursor = Cursors.Default;
            Stoping.Dock = DockStyle.Fill;
            Stoping.Visible = true;
            Stoping.BringToFront();

            //Insert
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = " delete tbl_Temp_OreFlow_Data_Graph where userid = '" + TUserInfo.UserID.ToString() + "' ";//



            for (int y = 0; y < x; y++)//rows
            {


                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " insert into tbl_Temp_OreFlow_Data_Graph values( ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[0].Value + "', '" + Stoping.Rows[y].Cells[1].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[2].Value + "', '" + Stoping.Rows[y].Cells[3].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[4].Value + "', '" + Stoping.Rows[y].Cells[5].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[6].Value + "', '" + Stoping.Rows[y].Cells[7].Value + "', ";

                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[8].Value + "', '" + Stoping.Rows[y].Cells[9].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[10].Value + "', '" + Stoping.Rows[y].Cells[11].Value + "', ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + Stoping.Rows[y].Cells[12].Value + "', '" + Stoping.Rows[y].Cells[13].Value + "', '" + y + "',  '" + Stoping.Rows[y].Cells[5].Value + "', '" + Stoping.Rows[y].Cells[14].Value + "' ) ";



            }
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

        }

        #endregion Methods 

        private void barbtnClose_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
