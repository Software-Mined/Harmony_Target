using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraScheduler;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class ucSysAdmin_Calendars : BaseUserControl
    {
        public ucSysAdmin_Calendars()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpCalendars);
            FormActiveRibbonPage = rpCalendars;
            FormMainRibbonPage = rpCalendars;
            RibbonControl = rcCalendars;
        }


        #region Private Variables
        String SelectProdmonth;
        string Calendarcode;

        const int Working = 0;
        const int NonWorking = 1;
        static Color colWorking = new Color();
        static Color colNonWorking = new Color();


        AppointmentLabel lbWorkingDay;
        AppointmentLabel lbNonWorkingDay;

        #endregion

        private void ucSysAdmin_Calendars_Load(object sender, EventArgs e)
        {
            //SelectProdmonth = "201910";

            barProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString());

            //barProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth);


            colNonWorking = Color.MistyRose;

            LoadSections();
            LoadSeccal();
            LoadCaltypes();
        }

        private void LoadCaltypes()
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan1.SqlStatement = " Select * from tbl_Code_Calendar  ";

            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable dtCaltype = _dbMan1.ResultsDataTable;

            DataSet dsCaltype = new DataSet();

            dsCaltype.Tables.Add(dtCaltype);

            gcCaltypes.DataSource = dsCaltype.Tables[0];

            colCalTypes.FieldName = "CalendarCode";
            colCalDescription.FieldName = "Description";
        }

        private void LoadSections()
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan1.SqlStatement = " Select * from tbl_Section where prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "' ";

            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable dtSect = _dbMan1.ResultsDataTable;

            DataSet dsSect = new DataSet();

            dsSect.Tables.Add(dtSect);

            gcSection.DataSource = dsSect.Tables[0];

            colSectSectionID.FieldName = "SectionID";
            colSectName.FieldName = "Name";
            colSectHierarchicalID.FieldName = "Hierarchicalid";
            colSectReportToSetionID.FieldName = "ReportToSectionid";
            colSectEmpNo.FieldName = "PFNumber";
        }


        private void LoadSeccal()
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan1.SqlStatement = " Select seccal.*,sec.Name from tbl_code_calendar_section seccal  \r\n" +
                                   "left outer join tbl_Section sec on seccal.Sectionid = sec.SectionID  \r\n" +
                                   "                               and seccal.Prodmonth = sec.Prodmonth \r\n" +
                                   "where sec.prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "' ";

            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable dtSeccal = _dbMan1.ResultsDataTable;

            DataSet dsSeccal = new DataSet();

            dsSeccal.Tables.Add(dtSeccal);

            gcSeccal.DataSource = dsSeccal.Tables[0];

            colSeccalSectionID.FieldName = "Sectionid";
            colSeccalName.FieldName = "Name";
            colSeccalStartDate.FieldName = "BeginDate";
            colSeccalEndDate.FieldName = "EndDate";
            colSeccalWorkingDays.FieldName = "TotalShifts";
        }

        private void barProdmonth_EditValueChanged(object sender, EventArgs e)
        {
            LoadSections();
            LoadSeccal();
        }

        private void gvCaltypes_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.RowHandle != -1)
            {
                Calendarcode = gvCaltypes.GetRowCellValue(e.RowHandle, gvCaltypes.Columns["CalendarCode"]).ToString();
                loadDates(Calendarcode);
            }
        }


        private void loadDates(string calType)
        {
            ssWorkingDays.Appointments.Mappings.AllDay = "AllDay";
            ssWorkingDays.Appointments.Mappings.Subject = "Heading";
            ssWorkingDays.Appointments.Mappings.Start = "StartTime";
            ssWorkingDays.Appointments.Mappings.End = "EndTime";
            ssWorkingDays.Appointments.Mappings.Label = "theID";
            ssWorkingDays.Appointments.DataSource = getWorkingDays(calType);
        }


        public DataTable getWorkingDays(string calType)
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            try
            {
                theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                theData.queryReturnType = MWDataManager.ReturnType.DataTable;
                theData.SqlStatement = "SELECT [CalendarCode] \r\n" +
                                       "      ,cast(1 as bit) AllDay \r\n" +
                                       "	  ,CASE WHEN  [Workingday] = 'Y' THEN 'Working Day' ELSE 'Non Working Day' END Heading \r\n" +
                                       "	  ,CASE WHEN  [Workingday] = 'Y' THEN 0 ELSE 1 END theID \r\n" +
                                       "      ,[CalendarDate] StartTime \r\n" +
                                       "	  ,[CalendarDate] + 0.7 EndTime \r\n" +
                                       "      ,[Workingday] \r\n" +
                                       "  FROM [dbo].[tbl_Code_Calendar_Type] WHERE CalendarCode = '" + calType + "' \r\n";

                theData.ExecuteInstruction();
                return theData.ResultsDataTable;

            }
            catch (Exception e)
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Error", e.Message, Color.Red);
                return null;
            }

        }

        private void dnMain_Click(object sender, EventArgs e)
        {

        }

        private void dnMain_CustomDrawDayNumberCell(object sender, DevExpress.XtraEditors.Calendar.CustomDrawDayNumberCellEventArgs e)
        {
            AppointmentBaseCollection appList = ssWorkingDays.GetAppointments(e.Date, e.Date.AddHours(0.7));
            Rectangle rec = new Rectangle(e.Bounds.Location, e.Bounds.Size);
            
            if (appList.Count == 1)
            {
            #pragma warning disable CS0618
                switch (appList[0].LabelId)
            #pragma warning restore CS0618
                {
                    case NonWorking:
                        System.Drawing.SolidBrush nwBrush = new System.Drawing.SolidBrush(colNonWorking);
                        e.Graphics.FillRectangle(nwBrush, rec);
                        break;
                    case Working:

                        System.Drawing.SolidBrush wBrush = new System.Drawing.SolidBrush(colWorking);


                        e.Graphics.FillRectangle(wBrush, rec);
                        break;

                }
            }
        }

        private void scWorkingNonWorkingDays_EditAppointmentFormShowing(object sender, AppointmentFormEventArgs e)
        {
            Appointment apt = e.Appointment;

            AppointmentBaseCollection appList = ssWorkingDays.GetAppointments(apt.Start, apt.End);
            if (appList.Count == 0)
            {
                apt.AllDay = true;
                apt.Subject = lbWorkingDay.DisplayName;
                apt.LabelId = Working;
                ssWorkingDays.Appointments.Add(apt);
                saveDateInfo(Calendarcode, apt.Start, true);
            }
            else
            {
                if (appList[0].LabelId == Working)
                {
                    appList[0].LabelId = NonWorking;
                    appList[0].Subject = lbNonWorkingDay.DisplayName;
                    saveDateInfo(Calendarcode, appList[0].Start, false);
                }
                else
                {
                    appList[0].LabelId = Working;
                    appList[0].Subject = lbWorkingDay.DisplayName;
                    saveDateInfo(Calendarcode, appList[0].Start, true);
                }

            }

            //scWorkingNonWorking.Refresh();
            e.Handled = true;
        }


        public bool saveDateInfo(string CalendarCode, DateTime theDay, bool isWorking)
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            bool Result = false;
            try
            {
                theData.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
                theData.queryReturnType = MWDataManager.ReturnType.DataTable;
                theData.SqlStatement = "SP_Manage_CalcType";
                char WorkDay;
                if (isWorking) { WorkDay = 'Y'; } else { WorkDay = 'N'; }
                SqlParameter[] _paramCollectionS =
                            {
                             theData.CreateParameter("@CalendarCode", SqlDbType.VarChar, 20,CalendarCode ),
                             theData.CreateParameter("@theDay", SqlDbType.Date  , 50,theDay.ToShortDateString() ),
                             theData.CreateParameter("@isWorking", SqlDbType.Char, 1,WorkDay  )

                            };


                theData.ParamCollection = _paramCollectionS;
                theData.ExecuteInstruction();
                Result = true;
            }
            catch (Exception e)
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Error", e.Message, Color.Red);
                Result = false;
            }

            return Result;
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void gcSection_Click(object sender, EventArgs e)
        {

        }
    }
}
