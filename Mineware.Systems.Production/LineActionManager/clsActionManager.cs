using Mineware.Systems.Global.sysMessages;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.GlobalExtensions;
using MWDataManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mineware.Systems.Production.LineActionManager
{
    
    class clsActionManager : BaseClass
    {
        private readonly sysMessagesClass _sysMessagesClass = new sysMessagesClass();
        public string _theConnection = string.Empty;
        public static DataTable dtActions; 
        public void get_OpenActions()
        {
            DataAccess.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            DataAccess.SqlStatement = " exec sp_LineAction_Manager_LoadOpenActions";
            DataAccess.queryExecutionType = ExecutionType.GeneralSQLStatement;
            DataAccess.queryReturnType = ReturnType.DataTable;
            var errorMsg = DataAccess.ExecuteInstruction();
            dtActions = DataAccess.ResultsDataTable;
            if (errorMsg.success == false)
            {
                _sysMessagesClass.viewMessage(MessageType.Error, "SQL ERROR", ProductionRes.systemTag, "clsBookingsABS", "get_UserBookSection", errorMsg.Message, ButtonTypes.OK, MessageDisplayType.FullScreen);
                
            }
           
        }
     }
}
