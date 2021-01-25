using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using Mineware.Systems.Global;
using DevExpress.XtraScheduler.Commands;
using Mineware.Systems.GlobalConnect;
using FastReport;
using FastReport.Export.Pdf;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Drawing;
using System.Drawing.Drawing2D;
using DevExpress.XtraNavBar.ViewInfo;
using DevExpress.XtraNavBar;

namespace Mineware.Systems.Production.Reporting.Labour
{
    public partial class ucGangReport : BaseUserControl
    {
        public ucGangReport()
        {
            InitializeComponent();
            FormRibbonPages.Add(ribbonPage1);
            FormActiveRibbonPage = ribbonPage1;
            FormMainRibbonPage = ribbonPage1;
            RibbonControl = RCRockEngineering;
        }

        string SelectProdmonth;
        private Report repGang = new Report();
        private string Loaded;
        private String selectedMO;
        private String selectedGang;
        private String report;
        private String click;
        private DataTable dtGangList = new DataTable();
        private DataTable dtClickEmp = new DataTable();
        private Procedures procs = new Procedures();

        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

        private void employeeDC()
        {
            MWDataManager.clsDataAccess _dbManGetISAfterStart = new MWDataManager.clsDataAccess();
            _dbManGetISAfterStart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetISAfterStart.SqlStatement = " ";

            _dbManGetISAfterStart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetISAfterStart.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetISAfterStart.ResultsTableName = "Emp";
            _dbManGetISAfterStart.ExecuteInstruction();

            if(_dbManGetISAfterStart.ResultsDataTable.Rows.Count > 0)
            {
                dtClickEmp = _dbManGetISAfterStart.ResultsDataTable;
                DataSet dsReport = new DataSet();
                DataTable dtReportData = new DataTable();
                dtReportData = _dbManGetISAfterStart.ResultsDataTable;
                dsReport.Tables.Add(dtReportData);
                //repGang.RegisterData(dsReport);

                //repGang.Load(_reportFolder + "\\StopeIncentiveCalcSheet.frx");

                ////repGang.Design();

                //pcReport.Clear();
                //repGang.Prepare();
                //repGang.Preview = pcReport;
                //repGang.ShowPrepared();

            }
            
        }

        private void loadReport()
        {
            if (Loaded == "Y")
            { 
                MWDataManager.clsDataAccess _dbManGetISAfterStart = new MWDataManager.clsDataAccess();
                _dbManGetISAfterStart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManGetISAfterStart.SqlStatement = " EXEC sp_GetGangReportData " + procs.ExtractBeforeColon(selectedGang) + " ";

                _dbManGetISAfterStart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManGetISAfterStart.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManGetISAfterStart.ResultsTableName = "Gang";
                _dbManGetISAfterStart.ExecuteInstruction();
                DataTable dtReportData = new DataTable();
                dtReportData = _dbManGetISAfterStart.ResultsDataTable;
                if (dtReportData.Rows.Count > 0)
                {
                    DataSet dsReport = new DataSet();
                    dtReportData = _dbManGetISAfterStart.ResultsDataTable;
                    dsReport.Tables.Add(dtReportData);
                    repGang.RegisterData(dsReport);

                    repGang.Load(_reportFolder + "\\StopeIncentiveCalcSheet.frx");

                    //repGang.Design();

                    pcReport.Clear();
                    repGang.Prepare();
                    repGang.Preview = pcReport;
                    repGang.ShowPrepared();

                    Application.Idle += new System.EventHandler(this.Application_Idle);
                }
                else
                {

                    MessageBox.Show("No Data Available","Notification",MessageBoxButtons.OK,MessageBoxIcon.Information);

                }
               
            }
            Report reGang_Click = new Report();
        }

        private void Application_Idle(Object sender, EventArgs e)
        {
            if (repGang.GetParameterValue("Param1") != null)
            {
                report = repGang.GetParameterValue("Param1").ToString();
            }
            if (repGang.GetParameterValue("Param3") != null)
            {
                click = repGang.GetParameterValue("Param3").ToString();
                employeeDC();
            }


        }

        private void loadGangs()
        {
            MWDataManager.clsDataAccess _dbManGetISAfterStart = new MWDataManager.clsDataAccess();
            _dbManGetISAfterStart.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (selectedMO == "Top Panels Early Morning Report (Kgs)")
            {
                _dbManGetISAfterStart.SqlStatement = " SELECT Gang +':'+ GangName AS Gang FROM[dbo].[vw_MorningReportData]  WHERE Top20 = '" + selectedMO + "' ";
            }
            else
            {
                
                _dbManGetISAfterStart.SqlStatement = " SELECT Gang +':'+ GangName AS Gang FROM[dbo].[vw_MorningReportData]  WHERE MO = '" + selectedMO + "' ";

                
            }
            _dbManGetISAfterStart.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetISAfterStart.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetISAfterStart.ExecuteInstruction();
            dtGangList = _dbManGetISAfterStart.ResultsDataTable;

            lbGangs.Items.Clear();


            foreach (DataRow row in dtGangList.Rows)
            {
                lbGangs.Items.Add(row["Gang"].ToString());
            }
          
            Loaded = "Y";
        }

        private void ucGangReport_Load(object sender, EventArgs e)
        {

            SelectProdmonth = ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString();

           

            barProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth);

            //barProdmonth.EditValue = THarmonyPASGlobal.getSystemSettingsProductionInfo(UserCurrentInfo.Connection).CurrentProductionMonth.ToString();

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

            lbMining.Items.Add("Top Panels Early Morning Report (Kgs)");            

            foreach (DataRow dr in dtSections.Rows)
            {
                lbMining.Items.Add(dr["lbl"].ToString());
            }

            itema.Visible = false;
            lbMining.SelectedIndex = -1;


            // get Engineers
            MWDataManager.clsDataAccess _dbManGetISAfterStart1 = new MWDataManager.clsDataAccess();
            _dbManGetISAfterStart1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetISAfterStart1.SqlStatement = "select distinct(lbl) lbl   from [dbo].[vw_MorningReportData] where lbl like '%Eng%'   " +
                                            "order by lbl  ";

            _dbManGetISAfterStart1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetISAfterStart1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetISAfterStart1.ExecuteInstruction();

            DataTable dtSections1 = _dbManGetISAfterStart1.ResultsDataTable;

            navBarControl1.ActiveGroup = navBarGroup2;

            DevExpress.XtraNavBar.NavBarItem itema1 = navBarControl1.Items.Add();
            itema1.Caption = "";
            navBarControl1.ActiveGroup.ItemLinks.Add(itema1);


            foreach (DataRow dr in dtSections1.Rows)
            {
                lbEngineering.Items.Add(dr["lbl"].ToString());
            }

            itema1.Visible = false;
            lbEngineering.SelectedIndex = -1;

            // getother
            MWDataManager.clsDataAccess _dbManGetOther = new MWDataManager.clsDataAccess();
            _dbManGetOther.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetOther.SqlStatement = "select * from ( select distinct(Lbl) Lbl from( " +
                                            "select MainLbl, lbl   from [dbo].[vw_MorningReportData] where lbl not like '%Eng%') a) tmp where " +
                                            " lbl not in ( select distinct(lbl) lbl from vw_MorningReportData   " +
                                            " where MainLbl in ('2) Stoping','3) Development')) order by lbl  ";

            _dbManGetOther.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetOther.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetOther.ExecuteInstruction();

            DataTable dtSections3 = _dbManGetOther.ResultsDataTable;

            foreach (DataRow dr in dtSections3.Rows)
            {
                lbServices.Items.Add(dr["lbl"].ToString());
            }

            lbServices.SelectedIndex = -1;

            Loaded = "Y";

            lbServices.SelectedIndex = -1;
        }

        private void repositoryItemSpinEdit1_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            string _sMonth = barProdmonth.EditValue.ToString().Substring(4, 2);

            int _iMonth = Convert.ToInt32(barProdmonth.EditValue.ToString().Substring(4, 2));
            int Year = Convert.ToInt32(barProdmonth.EditValue.ToString().Substring(0, 4));

            if (_iMonth >= 12)
            {
                Year = Year + 1;
                _sMonth = "01";

                barProdmonth.EditValue = Convert.ToString(Year) + _sMonth;
            }

            if (_iMonth <= 1)
            {
                Year = Year - 1;
                _sMonth = "12";

                barProdmonth.EditValue = Year.ToString() + _sMonth;
            }
        }

        private void lbMining_SelectedIndexChanged(object sender, EventArgs e)
        {
            Loaded = "N";
            //get gangs
            selectedMO = procs.ExtractBeforeColon(lbMining.SelectedItem.ToString());
            loadGangs();
            
        }

        private void lbEngineering_SelectedIndexChanged(object sender, EventArgs e)
        {
            Loaded = "N";
            //get gangs
            selectedMO = procs.ExtractBeforeColon(lbEngineering.SelectedItem.ToString());
            loadGangs();
        }

        private void lbServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            Loaded = "N";
            //get gangs
            selectedMO = procs.ExtractBeforeColon(lbServices.SelectedItem.ToString());
            loadGangs();
        }

        private void lbGangs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbGangs.SelectedIndex != -1)
            {
                //load report
                selectedGang = procs.ExtractBeforeColon(lbGangs.Text);
                loadReport();
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
