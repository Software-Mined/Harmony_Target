using MWDataManager;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace Mineware.Systems.DocumentManager
{
    class clsGetSections 
    {
        StringBuilder sb = new StringBuilder();
        public string _theConnection = string.Empty;
        public DataTable get_UserBookSection(string _prodmonth, string _userid)
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            theData.SqlStatement = " Select Section from [tbl_Users_Synchromine] " +
                                   " where UserID = '" + _userid + "' ";
            theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = ReturnType.DataTable;
            theData.ExecuteInstruction();

            var _sectionid = string.Empty;
            //string _sqlStatement = "";
            sb.Clear();
            foreach (DataRow dr in theData.ResultsDataTable.Rows)
            {
                _sectionid = dr["Section"].ToString();

                theData.SqlStatement = " Select HierarchicalID from tbl_Section " +
                                       " where Prodmonth = '" + _prodmonth + "' and " +
                                       " SectionID = '" + dr["Section"] + "' ";
                theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
                theData.queryReturnType = ReturnType.DataTable;
                theData.ExecuteInstruction();

                var _hierid = 0;
                if (theData.ResultsDataTable.Rows.Count > 0)
                {
                    _hierid = Convert.ToInt32(theData.ResultsDataTable.Rows[0]["HierarchicalID"].ToString());
                }
                if (_hierid >= 0 &&
                    _hierid <= 5)
                {
                    theData.SqlStatement = " Select SectionID, Name " +
                                           " from tbl_Section s where s.Prodmonth = '" + _prodmonth + "' and " +
                                           " Hierarchicalid > '" + _hierid + "' " +
                                           " order by SectionID ";
                    theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
                    theData.queryReturnType = ReturnType.DataTable;
                    theData.ExecuteInstruction();

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
            theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = ReturnType.DataTable;
            theData.SqlStatement = sb.ToString();
            theData.SqlStatement = theData.SqlStatement.Substring(0, theData.SqlStatement.Length - 7);
            var errorMsg = theData.ExecuteInstruction();
            //if (errorMsg.success == false)
            //{
            //    _sysMessagesClass.viewMessage(MessageType.Error, "SQL ERROR", ProductionRes.systemTag, "clsBookingsABS", "get_UserBookSection", errorMsg.Message, ButtonTypes.OK, MessageDisplayType.FullScreen);
            //    return theData.ResultsDataTable;
            //}
            return theData.ResultsDataTable;
        }
    }
}
