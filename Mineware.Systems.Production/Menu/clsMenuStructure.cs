using Mineware.Systems.GlobalConnect;
using MWDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Mineware.Systems.Production.Menu
{
    //public static DataTable dtain = new DataTable();
    class clsMenuStructure : BaseClass
    {

        public static DataTable dtgetAllProductionAndSystemMenuItems;
        public static DataTable dtgetAllParentMenuItems;
        public static DataTable dtgetOCRParentMenuItems;
        public static DataTable dtgetAllOCRItems;
        public static DataTable dtgetOMSParentMenuItems;
        public static DataTable dtgetAllOMSItems;
        public static DataTable dtgetBCSParentMenuItems;
        public static DataTable dtgetAllBCSItems;
        public static DataTable dtgetCCParentMenuItems;
        public static DataTable dtgetAllCCItems;

        public static DataTable dtUserModules;
        public void getAllProductionAndSystemMenuItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"SELECT *,
                            Case when TopItemID = 'Production' then 1
                            when TopItemID = 'apsDepartment' then 2
                            when TopItemID = 'apsSDB' then 3
                            when TopItemID = 'apsReporting' then 4
                            when TopItemID = 'apsSystemMaint' then 5
                            when TopItemID = 'SSSec' then 6
                            when TopItemID = 'SS' then 7 end as OrderBY
                             FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.Production', 'Mineware.Systems.Settings'))
                            AND [ItemTypeID] IN ('UI', 'RP') and ItemID not in ('SSAutoReport', 'apsKPI', 'SSPasswordPolicy')
							order by OrderBY, ItemPos");
            }
            else
            {
                sb.AppendLine(@"SELECT *,
                            Case when TopItemID = 'ProductionAmplats' then 1
                            when TopItemID = 'apsDepartment' then 2
                            when TopItemID = 'apsSDB' then 3
                            when TopItemID = 'apsReporting' then 4
                            when TopItemID = 'apsSystemMaint' then 5
                            when TopItemID = 'SSSec' then 6
                            when TopItemID = 'SS' then 7 end as OrderBY
                             FROM [Syncromine].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.ProductionAmplats', 'Mineware.Systems.Settings'))
                            AND [ItemTypeID] IN ('UI', 'RP')
							order by OrderBY, ItemPos");
            }

            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtgetAllProductionAndSystemMenuItems = theData.ResultsDataTable;


        }



        /// <summary>
        /// Gets all the parent menu items
        /// </summary>
        /// <returns></returns>
        public void getAllParentMenuItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@" SELECT *     
                              FROM [Syncromine_New].[dbo].[tblProfileItems]
                              WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.Production', 'Mineware.Systems.Settings') and ItemID not in ('SS')
                              ORDER BY [Description]");
            }
            else
            {
                sb.AppendLine(@" SELECT *     
                              FROM [Syncromine].[dbo].[tblProfileItems]
                              WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.ProductionAmplats', 'Mineware.Systems.Settings')
                              ORDER BY [Description]");
            }
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtgetAllParentMenuItems = theData.ResultsDataTable;


        }

        ////OCR MENU
        ///
        public void getOCRParentMenuItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"SELECT * FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.OCR')
                            ORDER BY ItemPos ");
            }
            else
            {
                sb.AppendLine(@"SELECT * FROM [Syncromine].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.OCR')
                            ORDER BY ItemPos ");
            }
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();

            dtgetOCRParentMenuItems = theData.ResultsDataTable;
        }

        public void getAllOCRItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"SELECT *
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.OCR'))
                            AND [ItemTypeID] IN ('UI', 'RP')
                            order by ItemPos");
            }
            else
            {
                sb.AppendLine(@"SELECT *
                            FROM [Syncromine].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.OCR'))
                            AND [ItemTypeID] IN ('UI', 'RP')
                            order by ItemPos");
            }
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtgetAllOCRItems = theData.ResultsDataTable;

        }


        ////OMS MENU
        ///
        public void getOMSParentMenuItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"SELECT * FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.IS')
                            ORDER BY ItemPos ");
            }
        
            else
            {
                sb.AppendLine(@"SELECT * FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.IS')
                            ORDER BY ItemPos ");
            }
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtgetOMSParentMenuItems = theData.ResultsDataTable;

        }

        public void getAllOMSItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"SELECT *
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.IS'))
                            AND [ItemTypeID] IN ('UI', 'RP')
                            order by ItemPos");
            }
            else
            {
                sb.AppendLine(@"SELECT *
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.IS'))
                            AND [ItemTypeID] IN ('UI', 'RP')
                            order by ItemPos");
            }
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtgetAllOMSItems = theData.ResultsDataTable;

        }

        ////Minescribe MENU
        ///
        public void getBCSParentMenuItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"SELECT * FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI') AND SystemID IN ('Mineware.Systems.ProductionAmplatsBonus')
                            ORDER BY ItemPos ");
            }
            else
            {
                sb.AppendLine(@"SELECT * FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI') AND SystemID IN ('Mineware.Systems.ProductionAmplatsBonus')
                            ORDER BY ItemPos ");
            }
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtgetBCSParentMenuItems = theData.ResultsDataTable;

        }

        public void getAllBCSItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"SELECT *
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.ProductionAmplatsBonus'))
                            AND [ItemTypeID] IN ('UI', 'RP')
                            order by ItemPos");
            }
            else
            {
                sb.AppendLine(@"SELECT *
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.ProductionAmplatsBonus'))
                            AND [ItemTypeID] IN ('UI', 'RP')
                            order by ItemPos");
            }
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtgetAllBCSItems = theData.ResultsDataTable;

        }


        public void getCCParentMenuItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"SELECT * FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('SI') AND SystemID IN ('Mineware.Systems.CCA')
                            ORDER BY ItemPos ");
            }
            else
            {
                sb.AppendLine(@"SELECT * FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('SI') AND SystemID IN ('Mineware.Systems.CCA')
                            ORDER BY ItemPos ");
            }
                
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtgetCCParentMenuItems = theData.ResultsDataTable;

        }


        public void getAllCCItems()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"SELECT *
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.CCA'))
                            AND [ItemTypeID] IN ('UI', 'RP')
                            order by ItemPos");
            }
            else
            {
                sb.AppendLine(@"SELECT *
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE TopItemID IN(
                            SELECT [ItemID]     
                            FROM [Syncromine_New].[dbo].[tblProfileItems]
                            WHERE ItemTypeID IN ('MI', 'SI') AND SystemID IN ('Mineware.Systems.CCA'))
                            AND [ItemTypeID] IN ('UI', 'RP')
                            order by ItemPos");
            }
               
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtgetAllCCItems = theData.ResultsDataTable;

        }


        public void getAllUserModules()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString("SystemSettings", TUserInfo.Site);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (TUserInfo.Site == "Target")
            {
                sb.AppendLine(@"select * from tblUserSystemAccess
                            where SystemAccess in (0) 
                            and UserID = '" + TUserInfo.UserID + "'");
            }
            else
            {
                sb.AppendLine(@"select * from tblUserSystemAccess
                            where SystemAccess in (0) 
                            and UserID = '" + TUserInfo.UserID + "'");
            }
              
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtUserModules = theData.ResultsDataTable;

        }
    }
}
