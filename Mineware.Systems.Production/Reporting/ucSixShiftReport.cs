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
    public partial class ucSixShiftReport : BaseUserControl
    {
        #region Fields and Properties      
        private string _FirstLoad = "Y";
        private Report _theReport;
        private Report _TempReport = new Report();
        /// <summary>
        /// The report to show
        /// </summary>
        public Report TheReport
        {
            get
            {
                return _theReport;
            }
            set
            {
                LoadReport(value);
            }
        }

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
        public ucSixShiftReport()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpSixShift);
            FormActiveRibbonPage = rpSixShift;
            FormMainRibbonPage = rpSixShift;
            RibbonControl = rcSixShift;
        }
        #endregion Constructor

        #region Events

        #endregion Events 

        #region Methods  
        /// <summary>
        /// Load the report after adding data
        /// </summary>
        /// <param name="report"></param>
        public void LoadReport(Report report)
        {
            pcReport2.Clear();
            _theReport = report;
            _theReport.Prepare();
            _theReport.Preview = pcReport2;

            //if (_designReport)
            //_theReport.Design();            

            _theReport.ShowPrepared();
            pcReport2.Visible = true;

            this.Cursor = Cursors.Default;
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
        #endregion Methods 

        private void ucSixShiftReport_Load(object sender, EventArgs e)
        {
            cbxBookingProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobalTSysSettings._currentProductionMonth.ToString());

            MWDataManager.clsDataAccess _dbManGetISAfterStart = new MWDataManager.clsDataAccess();
            _dbManGetISAfterStart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetISAfterStart.SqlStatement = "select sectionid, name from (select s.SectionID, MAX(name) name from dbo.tbl_PlanMonth_Cads c, tbl_Section s where c.Prodmonth = s.Prodmonth and c.MOSectionid = s.SectionID " +
                                        "and c.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "'  group by s.SECTIONid " +
                                        "union " +
                                        "select s2.SectionID, s2.name from tbl_Planning_Vamping v, tbl_Section s , tbl_Section s1, tbl_Section s2 " +
                                        "where v.Sectionid = s.SectionID and v.Prodmonth = s.Prodmonth  " +
                                        " and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth   " +
                                        " and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                        " and v.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' " +
                                        "group by s2.SectionID, s2.name) a group by sectionid, name " +
                                        "order by SECTIONid ";

            _dbManGetISAfterStart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetISAfterStart.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetISAfterStart.ExecuteInstruction();

            LookUpEditSection.DataSource = _dbManGetISAfterStart.ResultsDataTable;
            LookUpEditSection.DisplayMember = "name";
            LookUpEditSection.ValueMember = "sectionid";

            cbxBookingMO.EditValue = _dbManGetISAfterStart.ResultsDataTable.Rows[0][0];

            EditItemActivity.EditValue = "Stoping";

            //Otherradio.EditValue = "Sqm";

            
        }

        void Load6Shift()
        {
            this.Cursor = Cursors.WaitCursor;
            if (cbxBookingMO.EditValue.ToString() == string.Empty)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("Please select a Section", "Missing Section", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MWDataManager.clsDataAccess _dbMan6Shift = new MWDataManager.clsDataAccess();

            if (EditItemActivity.EditValue.ToString() == "Stoping")
            {
                if (cbxBookingMO.EditValue.ToString() != "Total Mine")
                {
                    //first dataset
                    _dbMan6Shift.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbMan6Shift.SqlStatement = " exec Report_6Shift '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "', '" + cbxBookingMO.EditValue.ToString() + "', '" + ProductionGlobalTSysSettings._CheckMeas.ToString() + "', '" + LookUpEditSection.GetDisplayText(cbxBookingMO.EditValue) + "' ";
                    _dbMan6Shift.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan6Shift.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan6Shift.ResultsTableName = "6Shift";
                    _dbMan6Shift.ExecuteInstruction();
                }
                else
                {
                    _dbMan6Shift.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbMan6Shift.SqlStatement = " exec Report_6ShiftTot '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "', '" + cbxBookingMO.EditValue.ToString() + "', '" + ProductionGlobalTSysSettings._CheckMeas.ToString() + "', '" + LookUpEditSection.GetDisplayText(cbxBookingMO.EditValue) + "' ";
                    _dbMan6Shift.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan6Shift.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan6Shift.ResultsTableName = "6Shift";
                    _dbMan6Shift.ExecuteInstruction();
                }
            }

            if (EditItemActivity.EditValue.ToString() == "Development")
            {
                if (cbxBookingMO.EditValue.ToString() != "Total Mine")
                {
                    //first dataset
                    _dbMan6Shift.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbMan6Shift.SqlStatement = " exec Report_6ShiftDev '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "', '" + cbxBookingMO.EditValue.ToString() + "', '" + ProductionGlobalTSysSettings._CheckMeas.ToString() + "', '" + LookUpEditSection.GetDisplayText(cbxBookingMO.EditValue) + "' ";
                    _dbMan6Shift.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan6Shift.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan6Shift.ResultsTableName = "6Shift";
                    _dbMan6Shift.ExecuteInstruction();
                }
                else
                {
                    _dbMan6Shift.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbMan6Shift.SqlStatement = " exec Report_6ShiftTotDev '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "', '" + cbxBookingMO.EditValue.ToString() + "', '" + ProductionGlobalTSysSettings._CheckMeas.ToString() + "', '" + LookUpEditSection.GetDisplayText(cbxBookingMO.EditValue) + "' ";
                    _dbMan6Shift.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan6Shift.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan6Shift.ResultsTableName = "6Shift";
                    _dbMan6Shift.ExecuteInstruction();
                }
            }

            if (EditItemActivity.EditValue.ToString() == "Vamping")
            {
                //if (Otherradio.SelectedIndex == 1)
                //{
                //    _dbMan6Shift.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                //    _dbMan6Shift.SqlStatement = " exec [Report_6ShiftVamp] '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' ";
                //    _dbMan6Shift.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //    _dbMan6Shift.queryReturnType = MWDataManager.ReturnType.DataTable;
                //    _dbMan6Shift.ResultsTableName = "6Shift";
                //    _dbMan6Shift.ExecuteInstruction();
                //}
                //else
                //{
                _dbMan6Shift.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbMan6Shift.SqlStatement = " exec [Report_6ShiftVampAdv] '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' ";
                    _dbMan6Shift.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMan6Shift.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMan6Shift.ResultsTableName = "6Shift";
                    _dbMan6Shift.ExecuteInstruction();
                //}
            }

            string Prodaa = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(cbxBookingProdMonth.EditValue));

            ProductionGlobal.ProductionGlobal.ProdMonthVis(Convert.ToInt32(Prodaa));
            string ProdMonthDesc = ProductionGlobal.ProductionGlobal.Prod2;

            MWDataManager.clsDataAccess _dbManData = new MWDataManager.clsDataAccess();
            _dbManData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManData.SqlStatement = "select '" + ProductionGlobalTSysSettings._Banner + "' banner, '" + ProdMonthDesc + "' ppppp ";
            _dbManData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManData.ResultsTableName = "Titile";  //get table name
            _dbManData.ExecuteInstruction();

            DataSet ReportTit = new DataSet();
            ReportTit.Tables.Add(_dbManData.ResultsDataTable);
            _TempReport.RegisterData(ReportTit);

            DataSet ReportDataset = new DataSet();
            ReportDataset.Tables.Add(_dbMan6Shift.ResultsDataTable);

            _TempReport.RegisterData(ReportDataset);

            _TempReport.SetParameterValue("6Shift", "None");
            _TempReport.SetParameterValue("6Shift2", "None");
            _TempReport.SetParameterValue("6Shift3", "None");

            if (EditItemActivity.EditValue.ToString() == "Stoping")
            {
                if (cbxBookingMO.EditValue.ToString() != "Total Mine")
                {
                    _TempReport.Load(_reportFolder + "6Shift.frx");
                }
                else
                {
                    _TempReport.Load(_reportFolder + "6ShiftTotal.frx");
                }
            }

            if (EditItemActivity.EditValue.ToString() == "Development")
            {
                if (cbxBookingMO.EditValue.ToString() != "Total Mine")
                {
                    _TempReport.Load(_reportFolder + "6ShiftDev.frx");
                }
                else
                {
                    _TempReport.Load(_reportFolder + "6ShiftTotalDev.frx");
                }
            }

            if (EditItemActivity.EditValue.ToString() == "Vamping")
            {
                //if (Otherradio.SelectedIndex == 0)
                //    _TempReport.Load(_reportFolder + "6ShiftVampAdv.frx");
                //else
                    _TempReport.Load(_reportFolder + "6ShiftVamp.frx");
            }

            LoadReport(_TempReport);
        }

        private void ProdMonthTxt_ValueChanged(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManGetISAfterStart = new MWDataManager.clsDataAccess();
            _dbManGetISAfterStart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetISAfterStart.SqlStatement = "select sectionid, case when name = '' then sectionid else name end as name \r\n" +
                                        " from (select s.SectionID, MAX(name) name from dbo.tbl_PlanMonth_Cads c, tbl_Section s where c.Prodmonth = s.Prodmonth and c.MOSectionid = s.SectionID " +
                                        "and c.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "'  group by s.SECTIONid " +
                                        "union " +
                                        "select s2.SectionID, s2.name from tbl_Planning_Vamping v, tbl_Section s , tbl_Section s1, tbl_Section s2 " +
                                        "where v.Sectionid = s.SectionID and v.Prodmonth = s.Prodmonth  " +
                                        " and s.ReportToSectionid = s1.SectionID and s.Prodmonth = s1.Prodmonth   " +
                                        " and s1.ReportToSectionid = s2.SectionID and s1.Prodmonth = s2.Prodmonth " +
                                        " and v.Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(cbxBookingProdMonth.EditValue)) + "' " +
                                        "group by s2.SectionID, s2.name) a group by sectionid, name " +
                                        "order by SECTIONid ";

            _dbManGetISAfterStart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetISAfterStart.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetISAfterStart.ExecuteInstruction();

            LookUpEditSection.DataSource = _dbManGetISAfterStart.ResultsDataTable;
            LookUpEditSection.DisplayMember = "name";
            LookUpEditSection.ValueMember = "sectionid";

            cbxBookingMO.EditValue = _dbManGetISAfterStart.ResultsDataTable.Rows[0][0];

            if (_FirstLoad == "N")
                Load6Shift();

            
        }

        private void SectionsCombo_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
                Load6Shift();

            _FirstLoad = "N";
        }

        private void ActivityGroupBox_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
                Load6Shift();
        }

        private void Otherradio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
                Load6Shift();
        }

        private void mwPlanPM_EditValueChanged(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
                Load6Shift();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (_FirstLoad == "N")
                Load6Shift();
        }

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_FirstLoad == "N")
                Load6Shift();
        }
    }
}
