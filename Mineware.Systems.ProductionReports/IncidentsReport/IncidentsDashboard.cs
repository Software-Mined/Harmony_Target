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

namespace Mineware.Systems.ProductionReports.IncidentsReport
{
    public partial class IncidentsDashboard : DevExpress.XtraReports.UI.XtraReport
    {
        public string _Prodmonth;
        public string _Mosect;
        public string _MosectName;
        public string _Connection;
        public string _Type;
        public DataTable tblprodDataSummary = new DataTable();
        Procedures procs = new Procedures();

        MWProdmonthEdit mWProdmonth = new MWProdmonthEdit();
        LookUpEdit lookUpEditSection = new LookUpEdit();
        LookUpEdit lookUpEditType = new LookUpEdit();
        LookUpEdit lookUpEditUser = new LookUpEdit();


        public IncidentsDashboard()
        {
            InitializeComponent();
        }

        private void PillarCompliance_DataSourceDemanded(object sender, EventArgs e)
        {
            //Set Titles
            lblCompanyName.Text = ProductionGlobal.ProductionGlobalTSysSettings._Banner;
            lblReportName.Text = "Open Incidents Report";
            txtProdmonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(Parameters["Prodmonth"].Value));
            txtSection.Text = Parameters["Section"].Value.ToString();
            txtSectionName.Text = lookUpEditSection.Properties.GetDisplayText(Parameters["Section"].Value.ToString());
            txtDate.Text = System.DateTime.Today.ToShortDateString();

            //Actions
            MWDataManager.clsDataAccess _dbMandata = new MWDataManager.clsDataAccess();
            _dbMandata.ConnectionString = _Connection;
            if (Parameters["Type"].Value.ToString() == "All Actions")
            {
                _dbMandata.SqlStatement = " exec sp_GetIncidents_Prodmonth '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(Parameters["Prodmonth"].Value)) + "', '" + Parameters["Section"].Value + "' ";                                             
            }
            else if (Parameters["Type"].Value.ToString() != "All Actions")
            {
                _dbMandata.SqlStatement = " exec sp_GetIncidents_Type '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(Parameters["Prodmonth"].Value)) + "', '" + Parameters["Section"].Value + "', '" + Parameters["Type"].Value + "' ";                                  
            }
            else if (Parameters["ResponsiblePerson"].Value.ToString() != "All Users")
            {
                _dbMandata.SqlStatement = " exec sp_GetIncidents_Users '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(Parameters["Prodmonth"].Value)) + "', '" + Parameters["Section"].Value + "', '" + Parameters["Type"].Value + "', '" + Parameters["ResponsiblePerson"].Value + "' ";
            }
            _dbMandata.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMandata.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMandata.ResultsTableName = "Actions";
            _dbMandata.ExecuteInstruction();
            tblprodDataSummary = null;
            tblprodDataSummary = _dbMandata.ResultsDataTable;
            this.DataSource = tblprodDataSummary;

            //Bind the table cells to the data fields.
            colWorkplace.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Workplace]"));            
            colAction.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ActionTitleNew]"));
            colHazard.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Priority]"));            
            colTargetDate.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Target_Date]"));
            colDateClosed.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Complete_Date]"));
            colRespPerson.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Responsible_Person]"));
            colClosedBy.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Action_Closed_By]"));




        }

        private void colHazard_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //if (colHazard.Text == "H")
            //{
            //    picFlagGreen.Visible = false;
            //    picFlagOrang.Visible = false;
            //    picFlagRed.Visible = true;
            //}
            //if (colHazard.Text == "M")
            //{
            //    picFlagGreen.Visible = false;
            //    picFlagOrang.Visible = true;
            //    picFlagRed.Visible = false;
            //}
            //if (colHazard.Text == "L")
            //{
            //    picFlagGreen.Visible = true;
            //    picFlagOrang.Visible = false;
            //    picFlagRed.Visible = false;
            //}
        }

        private void IncidentsDashboard_ParametersRequestBeforeShow(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
          

            MWDataManager.clsDataAccess _PrePlanningLoadSections = new MWDataManager.clsDataAccess();
            _PrePlanningLoadSections.ConnectionString = _Connection;
            _PrePlanningLoadSections.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningLoadSections.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningLoadSections.SqlStatement = " Select moid Sectionid_2,moname Name_2 \r\n" +
                                                    "from [dbo].[tbl_sectioncomplete] \r\n" +
                                                    "where prodmonth = '" + _Prodmonth + "' \r\n " +
                                                    " Group By moid,moname \r\n " +
                                                    " Order By moid,moname";
            _PrePlanningLoadSections.ExecuteInstruction();

            MWDataManager.clsDataAccess _Type = new MWDataManager.clsDataAccess();
            _Type.ConnectionString = _Connection;
            _Type.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _Type.queryReturnType = MWDataManager.ReturnType.DataTable;
            _Type.SqlStatement = "  Select 'All Actions' ID, 'All Actions' Department  \r\n" +
                                  "  union  \r\n" +
                                  "  Select 'PPRE' ID, 'Rock Engineering' Department  \r\n" +
                                  "  union  \r\n" +
                                  "  Select 'PPS' ID, 'Safety' Department  \r\n" +
                                  "  union  \r\n" +
                                  "  Select 'PPVT' ID, 'Ventillation' Department  \r\n" +
                                  "  union  \r\n" +
                                  "  Select 'PPSR' ID, 'Survey' Department  \r\n" +
                                  "  union  \r\n" +
                                  "  Select 'PPEG' ID, 'Engineering' Department  \r\n" +
                                  "  union   \r\n" +
                                  "  Select 'PPGL' ID, 'Geology'  Department  \r\n" +
                                  "   union  \r\n" +
                                  "  Select 'WED' ID, 'WED' Department  \r\n" +
                                 " union   \r\n" +
                                 " select 'PPS' ID, 'Safety' Department \r\n " +
                                 "  \r\n " +
                                 "  ";
            _Type.ExecuteInstruction();


            MWDataManager.clsDataAccess _Users = new MWDataManager.clsDataAccess();
            _Users.ConnectionString = _Connection;
            _Users.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _Users.queryReturnType = MWDataManager.ReturnType.DataTable;
            _Users.SqlStatement = " SELECT [USERID] ,[Name] +' '+[LastName] UserName FROM [Syncromine_New].[dbo].[tblUsers] ";
            _Users.ExecuteInstruction();

            DataTable tbl_Sections = _PrePlanningLoadSections.ResultsDataTable;
            DataTable tbl_Type = _Type.ResultsDataTable;
            DataTable tbl_Users = _Users.ResultsDataTable;

            foreach (ParameterInfo info in e.ParametersInformation)
            {
                if (info.Parameter.Name == "Section")
                {
                    
                    lookUpEditSection.Properties.DataSource = tbl_Sections;
                    lookUpEditSection.Properties.DisplayMember = "Name_2";
                    lookUpEditSection.Properties.ValueMember = "Sectionid_2";
                    lookUpEditSection.Properties.Columns.Add(new
                        LookUpColumnInfo("Name_2", 0, "Section"));
                    info.Editor = lookUpEditSection;
                }

                if (info.Parameter.Name == "Prodmonth")
                {
                    string SelectProdmonth = ProductionGlobalTSysSettings._currentProductionMonth.ToString();
                    mWProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth);
                   
                    info.Editor = mWProdmonth;
                    mWProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth);
                    info.Parameter.Value = mWProdmonth;

                    this.Parameters["Prodmonth"].Value = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth); 

                }

                if (info.Parameter.Name == "Type")
                {                    
                    lookUpEditType.Properties.DataSource = tbl_Type;
                    lookUpEditType.Properties.DisplayMember = "Department";
                    lookUpEditType.Properties.ValueMember = "ID";
                    lookUpEditType.Properties.Columns.Add(new
                        LookUpColumnInfo("Department", 0, "Department"));
                    info.Editor = lookUpEditType;
                    lookUpEditType.EditValue = "All Actions";
                    this.Parameters["Type"].Value = "All Actions";
                }

                if (info.Parameter.Name == "ResponsiblePerson")
                {


                    lookUpEditUser.Properties.DataSource = tbl_Users;
                    lookUpEditUser.Properties.DisplayMember = "USERID";
                    lookUpEditUser.Properties.ValueMember = "UserName";
                    lookUpEditUser.Properties.Columns.Add(new
                        LookUpColumnInfo("UserName", 0, "UserName"));
                    info.Editor = lookUpEditUser;
                    this.Parameters["ResponsiblePerson"].Value = "All Users";

                }
            }
        }

        private void colWorkplace_PreviewDoubleClick(object sender, PreviewMouseEventArgs e)
        {
            
        }

        private void colAction_PreviewDoubleClick(object sender, PreviewMouseEventArgs e)
        {
            //frmResponciblePersonFeedback frmCloseAction = new frmResponciblePersonFeedback();
            //object val = e.Brick.TextValue.ToString();
            //object ActionID = procs.ExtractBeforeColon(e.Brick.TextValue.ToString());
            //object ActionDesc = procs.ExtractAfterColon(e.Brick.TextValue.ToString());
            //DataRow[] Workplace = tblprodDataSummary.Select("Mineware_Action_ID = '" + ActionID + "'");
            //foreach (DataRow row in Workplace)
            //{
            //    frmCloseAction.Workplace = row["Workplace"].ToString();
            //}            
            //frmCloseAction.ConnectionString = _Connection;            
            //frmCloseAction.ActionId = ActionID.ToString();
            //frmCloseAction.ActionDesc = ActionDesc.ToString();
            //frmCloseAction.ShowDialog();



            //IncidentsDashboard_ParametersRequestSubmit(null,null);




        }

        private void IncidentsDashboard_ParametersRequestSubmit(object sender, ParametersRequestEventArgs e)
        {
            LoadReport();
        }

        void LoadReport()
        {
            //Set Titles
            lblCompanyName.Text = ProductionGlobal.ProductionGlobalTSysSettings._Banner;
            lblReportName.Text = "Open Incidents Report";
            txtProdmonth.Text = ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(Parameters["Prodmonth"].Value));
            txtSection.Text = Parameters["Section"].Value.ToString();
            txtSectionName.Text = lookUpEditSection.Properties.GetDisplayText(Parameters["Section"].Value.ToString());
            txtDate.Text = System.DateTime.Today.ToShortDateString();


            //Actions
            MWDataManager.clsDataAccess _dbMandata = new MWDataManager.clsDataAccess();
            _dbMandata.ConnectionString = _Connection;
            if (Parameters["Type"].Value.ToString() == "All Actions")
            {
                _dbMandata.SqlStatement = " exec sp_GetIncidents_Prodmonth '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(Parameters["Prodmonth"].Value)) + "', '" + Parameters["Section"].Value + "' ";

            }
            else
            {
                _dbMandata.SqlStatement = " exec sp_GetIncidents_Type '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(Parameters["Prodmonth"].Value)) + "', '" + Parameters["Section"].Value + "', '" + Parameters["Type"].Value + "' ";

            }
            _dbMandata.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMandata.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMandata.ResultsTableName = "Actions";
            _dbMandata.ExecuteInstruction();
            tblprodDataSummary = null;
            tblprodDataSummary = _dbMandata.ResultsDataTable;
            this.DataSource = null;
            this.DataSource = tblprodDataSummary;

            //Bind the table cells to the data fields.
            colWorkplace.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Workplace]"));
            colAction.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ActionTitleNew]"));
            colHazard.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Priority]"));
            colTargetDate.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Target_Date]"));
            colDateClosed.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Complete_Date]"));
            colRespPerson.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Responsible_Person]"));
            colClosedBy.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Action_Closed_By]"));
        }
    }
}
