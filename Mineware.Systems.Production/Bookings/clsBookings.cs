using Mineware.Systems.Global.sysMessages;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.GlobalExtensions;
using MWDataManager;
using System;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.Bookings
{
    class clsBookings : BaseClass
    {
        private readonly sysMessagesClass _sysMessagesClass = new sysMessagesClass();
        public string _theConnection = string.Empty;
        public DataTable get_UserBookSection(string _prodmonth, string _userid)
        {
            DataAccess.ConnectionString = _theConnection;
            DataAccess.SqlStatement = " Select Section from [tbl_Users_Synchromine] " +
                                   " where UserID = '" + _userid + "' ";
            DataAccess.queryExecutionType = ExecutionType.GeneralSQLStatement;
            DataAccess.queryReturnType = ReturnType.DataTable;
            DataAccess.ExecuteInstruction();

            var _sectionid = string.Empty;
            //string _sqlStatement = "";
            sb.Clear();
            foreach (DataRow dr in DataAccess.ResultsDataTable.Rows)
            {
                _sectionid = dr["Section"].ToString();

                DataAccess.SqlStatement = " Select HierarchicalID from tbl_Section " +
                                       " where Prodmonth = '" + _prodmonth + "' and " +
                                       " SectionID = '" + dr["Section"] + "' ";
                DataAccess.queryExecutionType = ExecutionType.GeneralSQLStatement;
                DataAccess.queryReturnType = ReturnType.DataTable;
                DataAccess.ExecuteInstruction();

                var _hierid = 0;
                if (DataAccess.ResultsDataTable.Rows.Count > 0)
                {
                    _hierid = Convert.ToInt32(DataAccess.ResultsDataTable.Rows[0]["HierarchicalID"].ToString());
                }
                if (_hierid >= 0 &&
                    _hierid <= 5)
                {
                    DataAccess.SqlStatement = " Select SectionID, Name " +
                                           " from tbl_Section s where s.Prodmonth = '" + _prodmonth + "' and " +
                                           " Hierarchicalid > '" + _hierid + "' " +
                                           " order by SectionID ";
                    DataAccess.queryExecutionType = ExecutionType.GeneralSQLStatement;
                    DataAccess.queryReturnType = ReturnType.DataTable;
                    DataAccess.ExecuteInstruction();

                    sb.AppendLine("select distinct SectionID_2 SectionID, Name_2 Name from tbl_PlanMonth a ");
                    sb.AppendLine("inner join [Sections_Complete] b on ");
                    sb.AppendLine(" a.prodmonth = b.prodmonth and ");
                    sb.AppendLine("  a.sectionid = b.sectionid ");
                    sb.AppendLine("where a.prodmonth = '" + _prodmonth + "'   ");
                    if (_hierid == 1)
                    {

                        sb.AppendLine(" and b.SectionID_5 = '" + _sectionid + "' ");


                    }
                    if (_hierid == 2)
                    {
                        sb.AppendLine(" and b.SectionID_4 = '" + _sectionid + "' ");
                    }
                    if (_hierid == 3)
                    {
                        sb.AppendLine(" and b.SectionID_3 = '" + _sectionid + "' ");
                    }
                    if (_hierid == 4)
                    {
                        sb.AppendLine(" and b.SectionID_2 = '" + _sectionid + "' ");
                    }
                    if (_hierid == 5)
                    {
                        sb.AppendLine(" v b.SectionID_1 = '" + _sectionid + "' ");
                    }
                    sb.AppendLine(" union");
                }
            }
            DataAccess.queryExecutionType = ExecutionType.GeneralSQLStatement;
            DataAccess.queryReturnType = ReturnType.DataTable;
            DataAccess.SqlStatement = sb.ToString();
            DataAccess.SqlStatement = DataAccess.SqlStatement.Substring(0, DataAccess.SqlStatement.Length - 7);
            var errorMsg = DataAccess.ExecuteInstruction();
            if (errorMsg.success == false)
            {
                _sysMessagesClass.viewMessage(MessageType.Error, "SQL ERROR", ProductionRes.systemTag, "clsBookingsABS", "get_UserBookSection", errorMsg.Message, ButtonTypes.OK, MessageDisplayType.FullScreen);
                return DataAccess.ResultsDataTable;
            }
            return DataAccess.ResultsDataTable;
        }
    }
}
