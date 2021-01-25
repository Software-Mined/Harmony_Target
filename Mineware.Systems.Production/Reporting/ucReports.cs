using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
//using Mineware.Systems.HarmonyPASGlobal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting
{
    public partial class ucReports : BaseUserControl
    {
        #region Fields and Properties           
        private Report _theReport;
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
        public ucReports()
        {
            InitializeComponent();
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
        #endregion Methods 
    }
}
