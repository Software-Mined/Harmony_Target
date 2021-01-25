using Mineware.Systems.GlobalConnect;
using MWDataManager;
using System;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Mineware.Systems.Production.OCRScheduling
{
    class clsOCRScheduling : BaseClass
    {

        public DataTable GetActivity()
        {
            clsDataAccess newData = new clsDataAccess();
            try
            {
                sb.Clear();
                sb.AppendLine("select Activity as [Code], Description as [Desc] from CODE_ACTIVITY");
                newData.ConnectionString = DataAccess.ConnectionString;
                newData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                newData.queryReturnType = MWDataManager.ReturnType.DataTable;
                newData.SqlStatement = sb.ToString();
                newData.ExecuteInstruction();

            }
            catch (Exception _exception)
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Error", _exception.Message, Color.Red);
            }
            return newData.ResultsDataTable;
        }

        public void UpdatePrintList(string PritedFromID, string UniqueID, string Date, string WorkplaceID, string ChecklistID, string DayString)
        {
            try
            {
                sb.Clear();
                sb.AppendLine("EXEC [dbo].[sp_OCR_Update_Checklist_Added_PrintedFromID] ");
                sb.AppendLine("@PrintedFromID = '" + PritedFromID + "',");
                sb.AppendLine("@UniqueID = '" + UniqueID + "',");
                sb.AppendLine("@Date = '" + Date + "',");
                sb.AppendLine("@WorkplaceID = '" + WorkplaceID + "',");
                sb.AppendLine("@ChecklistID = '" + ChecklistID + "',");
                sb.AppendLine("@DayString = '" + DayString + "'");
                DataAccess.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                DataAccess.SqlStatement = sb.ToString();
                DataAccess.ExecuteInstruction();

            }
            catch (Exception _exception)
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Error", _exception.Message, Color.Red);
            }

        }
    }
}
