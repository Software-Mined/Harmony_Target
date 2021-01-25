using Mineware.Systems.GlobalConnect;
using MWDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mineware.Systems.Production.Reporting.WorkplaceSummary
{
    class clsWorkplaceSummary
    {
        public static string Type;
        public static string Department;
        public string _connection;

        public static string SelectedWorkplace = "";
        public static string SelectedProdmonth;
        public static DateTime SelectedCaptDate;
        public static string SelectedActivity;
        public static string SelectedRiskRating;
        public static string SelectedMOSection;
        public static string SelectedMinerSection;
        public static string SelectedCrew;

        //Walkabouts
        public static DataTable dtData;

        public void getRoutineVisitData()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _connection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(@"select Prodmonth, CaptDate, WorkplaceID, Description, Crew,
                            MOID, MOName, SBID, SBName,  MinerID,  MinerName , MOSec, Convert(decimal(18,2),RiskRating)RiskRating,
                            Activity from (
							select Distinct pm.Prodmonth, CaptDate, pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName ,sc.MOID+':'+sc.MOName MOSec, RE.RiskRating,
                            case when pm.Activity = 1 then 'Dev' when Description like '%LED%' then 'Ledge' when pm.Activity = 0 then 'Stp'  else '' end as Activity
                            from tbl_Planmonth pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID
                            inner join tbl_DPT_RockMechInspection RE on re.Workplace = w.Description)a
                            union
							select Prodmonth, CaptDate, WorkplaceID, Description, Crew,
                            MOID, MOName, SBID, SBName,  MinerID,  MinerName , MOSec, Convert(decimal(18,2),RiskRating)RiskRating,
                             Activity
							 from (
                            select Distinct pm.Prodmonth, CaptDate, pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName ,sc.MOID+':'+sc.MOName MOSec, RE.RiskRating,
                            'Vamps' Activity
                            from tbl_Planning_Vamping pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID
                            inner join tbl_DPT_RockMechInspection_Vamping RE on re.Workplace = w.Description)a                       
							order by Prodmonth Desc");
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtData = theData.ResultsDataTable;
        }

        public void getGeologyVisitData()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _connection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(@"select Distinct pm.Prodmonth, CaptDate, pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName ,sc.MOID+':'+sc.MOName MOSec, 0 RiskRating,
                            case when pm.Activity = 1 then 'Dev' when pm.Activity = 0 then 'Stp' when Description like '%LED%' then 'Ledge' else '' end as Activity
                            from tbl_Planmonth pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID
                            inner join tbl_DPT_GeoScience_Inspection RE on re.Workplace = w.Description
                            order by Prodmonth Desc");
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtData = theData.ResultsDataTable;
        }

        public void getVentVisitData()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _connection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(@"select Distinct *, Calendardate CaptDate from (select Distinct pm.Prodmonth,  pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName ,sc.MOID+':'+sc.MOName MOSec, 0 RiskRating,
                            case when pm.Activity = 1 then 'Dev' when pm.Activity = 0 then 'Stp' when Description like '%LED%' then 'Ledge' else '' end as Activity
                            from tbl_Planmonth pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID)a
                            inner join (
                            select * from(
                            select Calendardate,Workplace,Section, Month from tbl_Dept_Inspection_VentCapture_Stoping group by Calendardate,Workplace,Section, Month 
                            union
                            select Calendardate,Workplace,Section, Month from tbl_Dept_Inspection_VentCapture_Development group by Calendardate,Workplace,Section, Month )A							
                            )b  on a.Workplaceid = b.Workplace and a.Prodmonth = b.Month
                            order by Prodmonth Desc");
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtData = theData.ResultsDataTable;
        }

        //PrePlanning

        public void getREPrePlanData()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _connection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(@"select Distinct pm.Prodmonth,  pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName , '' CaptDate,sc.MOID+':'+sc.MOName MOSec, rr.Answer RiskRating,
                            case when pm.Activity = 1 then 'Dev' when pm.Activity = 0 then 'Stp' when Description like '%LED%' then 'Ledge' else '' end as Activity
                            from tbl_Planmonth pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID
                            inner join tbl_PrePlanning_RockEng re on pm.Workplaceid = re.Workplace and pm.Prodmonth = re.Prodmonth
                            left outer join vw_Preplanning_Rating_Workplace rr on pm.Workplaceid = rr.workplace and pm.Prodmonth = rr.prodmonth
                            order by Prodmonth Desc");
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtData = theData.ResultsDataTable;
        }

        public void getEngPrePlanData()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _connection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(@"select Distinct pm.Prodmonth,  pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName , '' CaptDate,sc.MOID+':'+sc.MOName MOSec, rr.Answer RiskRating,
                            case when pm.Activity = 1 then 'Dev' when pm.Activity = 0 then 'Stp' when Description like '%LED%' then 'Ledge' else '' end as Activity
                            from tbl_Planmonth pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID
                            inner join tbl_PrePlanning_Engineering re on pm.Workplaceid = re.Workplace and pm.Prodmonth = re.Prodmonth
                            left outer join vw_Preplanning_Rating_Workplace rr on pm.Workplaceid = rr.workplace and pm.Prodmonth = rr.prodmonth
                            order by Prodmonth Desc");
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtData = theData.ResultsDataTable;
        }

        public void getGeoPrePlanData()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _connection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(@"select Distinct pm.Prodmonth,  pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName , '' CaptDate,sc.MOID+':'+sc.MOName MOSec, rr.Answer RiskRating,
                            case when pm.Activity = 1 then 'Dev' when pm.Activity = 0 then 'Stp' when Description like '%LED%' then 'Ledge' else '' end as Activity
                            from tbl_Planmonth pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID
                            inner join tbl_PrePlanning_Geology re on pm.Workplaceid = re.Workplace and pm.Prodmonth = re.Prodmonth
                            left outer join vw_Preplanning_Rating_Workplace rr on pm.Workplaceid = rr.workplace and pm.Prodmonth = rr.prodmonth
                            order by Prodmonth Desc");
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtData = theData.ResultsDataTable;
        }

        public void getSafetyPrePlanData()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _connection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(@"select Distinct pm.Prodmonth,  pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName , '' CaptDate,sc.MOID+':'+sc.MOName MOSec, rr.Answer RiskRating,
                            case when pm.Activity = 1 then 'Dev' when pm.Activity = 0 then 'Stp' when Description like '%LED%' then 'Ledge' else '' end as Activity
                            from tbl_Planmonth pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID
                            inner join tbl_PrePlanning_Safety re on pm.Workplaceid = re.Workplace and pm.Prodmonth = re.Prodmonth
                            left outer join vw_Preplanning_Rating_Workplace rr on pm.Workplaceid = rr.workplace and pm.Prodmonth = rr.prodmonth
                            order by Prodmonth Desc");
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtData = theData.ResultsDataTable;
        }

        public void getSurveyPrePlanData()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _connection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(@"select Distinct pm.Prodmonth,  pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName , '' CaptDate,sc.MOID+':'+sc.MOName MOSec, rr.Answer RiskRating,
                            case when pm.Activity = 1 then 'Dev' when pm.Activity = 0 then 'Stp' when Description like '%LED%' then 'Ledge' else '' end as Activity
                            from tbl_Planmonth pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID
                            inner join tbl_PrePlanning_Survey re on pm.Workplaceid = re.Workplace and pm.Prodmonth = re.Prodmonth
                            left outer join vw_Preplanning_Rating_Workplace rr on pm.Workplaceid = rr.workplace and pm.Prodmonth = rr.prodmonth
                            order by Prodmonth Desc");
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtData = theData.ResultsDataTable;
        }

        public void getVentPrePlanData()
        {
            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = _connection;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(@"select Distinct pm.Prodmonth,  pm.WorkplaceID, Description,Substring(OrgUnitDS,0,13) Crew,
                            sc.MOID, sc.MOName, sc.SBID, sc.SBName, sc.SectionID MinerID, sc.Name MinerName , '' CaptDate,sc.MOID+':'+sc.MOName MOSec, rr.Answer RiskRating,
                            case when pm.Activity = 1 then 'Dev' when pm.Activity = 0 then 'Stp' when Description like '%LED%' then 'Ledge' else '' end as Activity
                            from tbl_Planmonth pm
                            inner join tbl_Workplace w on pm.Workplaceid = w.WorkplaceID
                            inner join tbl_SectionComplete sc on sc.SectionID = pm.Sectionid and sc.Prodmonth = pm.Prodmonth
                            inner join vw_Seccal_totalShifts_Calc s on  s.Prodmonth = pm.Prodmonth and s.Sectionid = sc.SBID
                            inner join tbl_PrePlanning_Vent re on pm.Workplaceid = re.Workplace and pm.Prodmonth = re.Prodmonth
                            left outer join vw_Preplanning_Rating_Workplace rr on pm.Workplaceid = rr.workplace and pm.Prodmonth = rr.prodmonth
                            order by Prodmonth Desc");
            theData.SqlStatement = sb.ToString();
            theData.ExecuteInstruction();
            dtData = theData.ResultsDataTable;
        }
    }
}
