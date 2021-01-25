using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Mineware.Systems.ProductionGlobal;
using System.Data;
using DevExpress.XtraReports.Parameters;
using System.Data.Common;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Configuration;
using Mineware.Systems.Global.CustomControls;
using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.Native.Interaction;

namespace Mineware.Systems.ProductionReports.KPIReport
{
    public partial class KPIReportDev : DevExpress.XtraReports.UI.XtraReport
    {
        public string _Prodmonth;
        public string _Mosect;
        public string _MosectName;
        public string _Connection;
        public string _Type;

        MWProdmonthEdit mWProdmonth = new MWProdmonthEdit();
        LookUpEdit lookUpEditSection = new LookUpEdit();
        LookUpEdit lookUpEditType = new LookUpEdit();

        public KPIReportDev()
        {
            InitializeComponent();
        }

        private void KPIReport_DataSourceDemanded(object sender, EventArgs e)
        {
            //Set Titles
            lblCompanyName.Text = ProductionGlobal.ProductionGlobalTSysSettings._Banner;
            lblReportName.Text = "KPI Report";
            txtProdmonth.Text = _Prodmonth;          
            

            //Prev Month
            int PrevMonth = Convert.ToInt32(_Prodmonth) - 1;

            //KPI
            MWDataManager.clsDataAccess _dbMandata = new MWDataManager.clsDataAccess();
            _dbMandata.ConnectionString = _Connection;            
            _dbMandata.SqlStatement = " exec KPI_Dashboard '" + _Prodmonth + "','" + PrevMonth + "','" + System.DateTime.Today.ToShortDateString() + "', '" + SysSettings.Banner + "' ";                                            
          
            _dbMandata.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMandata.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMandata.ResultsTableName = "Actions";
            _dbMandata.ExecuteInstruction();

            DataTable tblprodDataSummary = _dbMandata.ResultsDataTable;
            this.DataSource = tblprodDataSummary;

           





        }

       

        private void KPIReport_ParametersRequestBeforeShow(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
          

            //MWDataManager.clsDataAccess _PrePlanningLoadSections = new MWDataManager.clsDataAccess();
            //_PrePlanningLoadSections.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            //_PrePlanningLoadSections.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_PrePlanningLoadSections.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_PrePlanningLoadSections.SqlStatement = " Select moid Sectionid_2,moname Name_2 \r\n" +
            //                                        "from [dbo].[tbl_sectioncomplete] \r\n" +
            //                                        "where prodmonth = '" + _Prodmonth + "' \r\n " +
            //                                        " Group By moid,moname \r\n " +
            //                                        " Order By moid,moname";
            //_PrePlanningLoadSections.ExecuteInstruction();

            //MWDataManager.clsDataAccess _Type = new MWDataManager.clsDataAccess();
            //_Type.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            //_Type.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_Type.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_Type.SqlStatement = "  Select 'All Actions' ID, 'All Actions' Department  \r\n" +
            //                      "  union  \r\n" +
            //                      "  Select 'PPRE' ID, 'Rock Engineering' Department  \r\n" +
            //                      "  union  \r\n" +
            //                      "  Select 'PPS' ID, 'Safety' Department  \r\n" +
            //                      "  union  \r\n" +
            //                      "  Select 'PPVT' ID, 'Ventillation' Department  \r\n" +
            //                      "  union  \r\n" +
            //                      "  Select 'PPSR' ID, 'Survey' Department  \r\n" +
            //                      "  union  \r\n" +
            //                      "  Select 'PPEG' ID, 'Engineering' Department  \r\n" +
            //                      "  union   \r\n" +
            //                      "  Select 'PPGL' ID, 'Geology'  Department  \r\n" +
            //                      "   union  \r\n" +
            //                      "  Select 'WED' ID, 'WED' Department  \r\n" +
            //                     " union   \r\n" +
            //                     " select 'PPS' ID, 'Safety' Department \r\n " +
            //                     "  \r\n " +
            //                     "  ";
            //_Type.ExecuteInstruction();

            //DataTable tbl_Sections = _PrePlanningLoadSections.ResultsDataTable;
            //DataTable tbl_Type = _Type.ResultsDataTable;

            foreach (ParameterInfo info in e.ParametersInformation)
            {
                //if (info.Parameter.Name == "Section")
                //{
                    
                //    lookUpEditSection.Properties.DataSource = tbl_Sections;
                //    lookUpEditSection.Properties.DisplayMember = "Name_2";
                //    lookUpEditSection.Properties.ValueMember = "Sectionid_2";
                //    lookUpEditSection.Properties.Columns.Add(new
                //        LookUpColumnInfo("Name_2", 0, "Section"));
                //    info.Editor = lookUpEditSection;
                //}

                if (info.Parameter.Name == "Prodmonth")
                {
                    string SelectProdmonth = ProductionGlobalTSysSettings._currentProductionMonth.ToString();
                    mWProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth);
                   
                    info.Editor = mWProdmonth;
                    mWProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth);
                    info.Parameter.Value = mWProdmonth;

                    this.Parameters["Prodmonth"].Value = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth); 


                }
                //if (info.Parameter.Name == "Type")
                //{

                    
                //    lookUpEditType.Properties.DataSource = tbl_Type;
                //    lookUpEditType.Properties.DisplayMember = "Department";
                //    lookUpEditType.Properties.ValueMember = "ID";
                //    lookUpEditType.Properties.Columns.Add(new
                //        LookUpColumnInfo("Department", 0, "Department"));
                //    info.Editor = lookUpEditType;
                //    lookUpEditType.EditValue = "All Actions";
                //    this.Parameters["Type"].Value = "All Actions";

                //}

            }
        }
    }
}
