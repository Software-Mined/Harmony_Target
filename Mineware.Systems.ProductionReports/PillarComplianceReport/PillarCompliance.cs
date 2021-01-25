using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Mineware.Systems.ProductionGlobal;
using System.Data;

namespace Mineware.Systems.ProductionReports.PillarComplianceReport
{
    public partial class PillarCompliance : XtraReport
    {
        public string _Prodmonth;
        public string _Mosect;
        public string _MosectName;
        public string _Connection;

        public PillarCompliance()
        {
            InitializeComponent();
        }

        private void PillarCompliance_DataSourceDemanded(object sender, EventArgs e)
        {
            //Set Titles
            lblCompanyName.Text = ProductionGlobal.ProductionGlobalTSysSettings._Banner;
            lblReportName.Text = "Pillar Compliance Report";
            txtProdmonth.Text = _Prodmonth;
            txtSection.Text = _Mosect;
            txtSectionName.Text = _MosectName;
            txtDate.Text = System.DateTime.Today.ToShortDateString();


            MWDataManager.clsDataAccess _PrePlanningData = new MWDataManager.clsDataAccess();
            _PrePlanningData.ConnectionString = _Connection;
            _PrePlanningData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningData.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningData.SqlStatement = "exec sp_Report_Pillar_Compliance '" + _Prodmonth + "','" + _Mosect + "' ";
            _PrePlanningData.ResultsTableName = "ProdDataSummary";
            var result = _PrePlanningData.ExecuteInstruction();
            
            DataTable tblprodDataSummary = _PrePlanningData.ResultsDataTable;
            this.DataSource = tblprodDataSummary;            

            //Bind the table cells to the data fields.
            colWorkplace.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Description]"));
            colPillarNumber.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[PillarNumber]"));
            colPillarHeight.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[EffectivePillarHeight]"));
            colPillarWidth.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[EffectivePillarWidth]"));
            colStrikeDim.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[StrikeDimmension]"));
            colDipDim.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[DipDimmension]"));
            colEffectiveRatio.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[EffectiveRation]"));

            ///Incidents SubReport
            Incidents _incidents = new Incidents();            
            _incidents._Connection = _Connection;
            _incidents._Mosect = _Mosect;
            _incidents._Prodmonth = _Prodmonth;
            _incidents._Type = "PPRE";
            subReportIncidents.ReportSource = _incidents;
           

        }
    }
}
