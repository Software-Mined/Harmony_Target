using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.LineActionManager
{
    public partial class ucLineActionManager_ClosureRequest : BaseUserControl
    {
        public ucLineActionManager_ClosureRequest()
        {
            InitializeComponent();
        }

        //public declarations
        //
        public Report genReport = new Report();
        public DataTable dtGenReportReceive = new DataTable("dtGenReportReceive");
        public int _tabselector;

        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

        //private declarations
        //
        private DataTable dtReceive = new DataTable("Table1");
        private String sqlQueryCompiler;
        private DataSet dsGlobal = new DataSet("dsGlobal");

        private void barBtnBack_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ucLineActionManager_CloseAction ucMain = new ucLineActionManager_CloseAction();
            ucMain._selectedtab = _tabselector;
            ucMain.Dock = DockStyle.Fill;
            this.Controls.Clear();
            this.Controls.Add(ucMain);
        }

        private void ucLineActionManager_ClosureRequest_Load(object sender, EventArgs e)
        {
            registerTables();
            bkwrMain.RunWorkerAsync();
        }

        private void loadReportData()
        {
            for (int i = 0; i < dtGenReportReceive.Rows.Count; i++)
            {
                sqlQueryCompiler = " EXEC [sp_LineActionManager_Report] @Mineware_Action_ID = '" + dtGenReportReceive.Rows[i]["IncidentNumber"] + "' ";
                sqlConnector(sqlQueryCompiler, "Table1");
            }
            genReport.RegisterData(dsGlobal);
        }

        private void registerTables()
        {
            dsGlobal.Tables.Add(dtReceive);
        }

        private void sqlConnector(string sqlQuery, string sqlTableIdentifier)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);
            _sqlConnection.SqlStatement = sqlQuery;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_sqlConnection.ResultsTableName = "Receive";
            _sqlConnection.ExecuteInstruction();
            dtReceive = _sqlConnection.ResultsDataTable;

            if (!string.IsNullOrEmpty(sqlTableIdentifier))
            {
                for (int i = 0; i < dsGlobal.Tables.Count; i++)
                {
                    if (dsGlobal.Tables[i].TableName == sqlTableIdentifier)
                    {
                        dsGlobal.Tables[i].Merge(dtReceive);
                    }
                }
            }
        }

        private void bkwrMain_DoWork(object sender, DoWorkEventArgs e)
        {
            loadReportData();
        }

        private void bkwrMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            genReport.Load(_reportFolder + "WorkOrderReDoLAM.frx");
            //genReport.Design();
            genReport.Prepare();
            genReport.Preview = pcReportGen;
            genReport.ShowPrepared();

            pcReportGen.BringToFront();
        }
    }
}
