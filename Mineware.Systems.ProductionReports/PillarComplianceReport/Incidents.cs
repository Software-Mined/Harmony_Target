using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Mineware.Systems.ProductionReports.PillarComplianceReport
{
    public partial class Incidents : DevExpress.XtraReports.UI.XtraReport
    {
        public string _Connection;
        public string _Prodmonth;
        public string _Mosect;
        public string _Type;

        public Incidents()
        {
            InitializeComponent();
        }

        private void Incidents_DataSourceDemanded(object sender, EventArgs e)
        {
            //Actions
            MWDataManager.clsDataAccess _dbMandata = new MWDataManager.clsDataAccess();
            _dbMandata.ConnectionString = _Connection;
            _dbMandata.SqlStatement = "  SELECT *, substring(HOD, CHARINDEX (':', HOD)+1, LEN(HOD)) as MO,  substring(RespPerson, CHARINDEX (':', RespPerson)+1, len(RespPerson)) as RespPerson, isnull(compnotes, '') Compnotes1, \r\n" +
                                      " case when CompNotes <> '' then '\\\\10.10.101.138\\MinewarePics\\Moabkhotsong\\Actions\\' + CompNotes + '.png' \r\n" +
                                      " else 'No Image' end as Compnotes2 from tbl_Shec_Incidents \r\n" +
                                     "  where Type in( '"+ _Type + "') and Completiondate is null and WPType = '"+_Mosect+"'\r\n";

            _dbMandata.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMandata.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMandata.ResultsTableName = "Actions";
            _dbMandata.ExecuteInstruction();

            DataTable tblprodDataSummary = _dbMandata.ResultsDataTable;
            this.DataSource = tblprodDataSummary;

            //Bind the table cells to the data fields.
            colWorkplace.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Workplace]"));
            colAction.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Action]"));
            colHazard.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Priority]"));
            //colDate.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TheDate]"));
            colTargetDate.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TargetDate]"));
            colDateClosed.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CompletionDate]"));
            colRespPerson.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[RespPerson]"));
            //colClosedBy.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Action]"));
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if(colHazard.Text == "H")
            {
                picFlagGreen.Visible = false;
                picFlagOrang.Visible = false;
                picFlagRed.Visible = true;
            }
            if (colHazard.Text == "M")
            {
                picFlagGreen.Visible = false;
                picFlagOrang.Visible = true;
                picFlagRed.Visible = false;
            }
            if (colHazard.Text == "L")
            {
                picFlagGreen.Visible = true;
                picFlagOrang.Visible = false;
                picFlagRed.Visible = false;
            }
        }
    }
}
