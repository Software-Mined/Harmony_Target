using Mineware.Systems.GlobalConnect;
using MWDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mineware.Systems.ProductionGlobal
{
    public class ProductionGlobalTSysSettings
    {
        public static int _currentProductionMonth;
        public static int _AColor;
        public static int _BColor;
        public static int _SColor;
        public static string _Banner;
        public static string _AdjBook;
        public static string _RepDir;
        private static DateTime _RunDate;
        public static string _CheckMeas;
        public static Int32 _BlastQual;
        public String SiteTag;


        public void GetSysSettings(string systemDBTag, string connection)
        {
            var _dbMan = new clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(systemDBTag, connection);

            _dbMan.SqlStatement = "Select * from tbl_SysSet";
            _dbMan.queryExecutionType = ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = ReturnType.DataTable;
            var DataResult = _dbMan.ExecuteInstruction();


            foreach (DataRow dr in _dbMan.ResultsDataTable.Rows)
            {        
                _currentProductionMonth = Convert.ToInt32(dr["CurrentProductionMonth"].ToString());  
                _RunDate = Convert.ToDateTime(dr["Rundate"].ToString());
                _RepDir = dr["REPDIR"].ToString();
                _AdjBook = dr["AdjBook"].ToString();
                _Banner = dr["BANNER"].ToString();
                _CheckMeas = dr["CheckMeas"].ToString();
                _BlastQual = Convert.ToInt32(Math.Round(Convert.ToDecimal(dr["percblastqualification"].ToString()), 0));
                _AColor = Convert.ToInt32(dr["A_Color"].ToString());
                _BColor = Convert.ToInt32(dr["B_Color"].ToString());
                _SColor = Convert.ToInt32(dr["S_Color"].ToString());
            }
        }
    }
}
