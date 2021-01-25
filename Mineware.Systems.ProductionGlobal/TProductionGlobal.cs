using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mineware.Systems.ProductionGlobal
{


    public static class TProductionGlobal
    {

        public static ProductionMenuStructure SysMenu = new ProductionMenuStructure();
       
        static TProductionGlobal()
        {
            SysMenu.setMenuItems();
            
        }
    }

   

    public class TProductionAmplatsGlobalDataTables
    {
        private DataTable _resultsDataTable;
        public DataTable ResultsDataTable { get { return _resultsDataTable; } }
        private readonly MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
        public DataTable GetActivity()
        {
            try
            {
                theData.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                theData.queryReturnType = MWDataManager.ReturnType.DataTable;
                theData.SqlStatement = "SELECT Activity ,Description" +
                                        "  FROM tbl_CODE_ACTIVITY";
                theData.ExecuteInstruction();

            }
            catch (Exception except)
            {
                throw new ApplicationException(except.Message, except);
            }
            finally
            {
            }
            return theData.ResultsDataTable;

        }

        public DataTable GetPlanMOSectionsAndName(String Prodmonth)
        {
            theData.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.SqlStatement = " select distinct b.SectionID_2 As sectionid, NAME_2 As NAME from tbl_planmonth a inner join sections_complete b on " +
                                                 "a.prodmonth = b.prodmonth and " +
                                                  "a.sectionid = b.sectionid " +
                                                  "Where a.Prodmonth = '" + Prodmonth + "' ";
            theData.ExecuteInstruction();
            return theData.ResultsDataTable;


        }

        public bool GetPlan()
        {
            bool _executionResult = false;
            try
            {
                theData.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
                theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                theData.queryReturnType = MWDataManager.ReturnType.DataTable;
   
                theData.SqlStatement = "SELECT 1 Code, 'Monthly Dynamic Plan' [Descd] ";
                theData.ExecuteInstruction();
                _resultsDataTable = theData.ResultsDataTable;
               
                _executionResult = true;
            }
            catch (Exception except)
            {
                _executionResult = false;
                throw new ApplicationException(except.Message, except);
            }
            finally
            {
            }

            return _executionResult;
        }
    }
    
}
