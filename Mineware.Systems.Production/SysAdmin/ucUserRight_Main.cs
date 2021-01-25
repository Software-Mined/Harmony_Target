using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionHelp;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.Configuration;
using System.Windows.Forms;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class ucUserRight_Main : BaseUserControl
    {
        #region Data Fields	
        static string colCaption = string.Empty;
        string AuthNum = "0", AuthNum2 = "0";
        int count = 0;
        DataTable dtUsers = new DataTable();
        #endregion

        #region Constructor

        public ucUserRight_Main()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpUserRight);
            FormActiveRibbonPage = rpUserRight;
            FormMainRibbonPage = rpUserRight;
            RibbonControl = rcUserRight;
        }

        #endregion


        #region Methods/Functions

        private void ucUserRight_Main_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _RockEng = new MWDataManager.clsDataAccess();
            _RockEng.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection); ;
            _RockEng.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _RockEng.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_RockEng.SqlStatement = "Select  Sectionid + ':' + name DisplaySect  from tbl_Section where prodmonth = (Select max(prodmonth) from tbl_planning where CalendarDate >= GETDATE() - 1) order by Hierarchicalid";
            _RockEng.SqlStatement = "Select  Sectionid + ':' + name DisplaySect from tbl_Section where prodmonth = (Select max(prodmonth) from tbl_planning ) order by Hierarchicalid";
            _RockEng.ExecuteInstruction();

            foreach (DataRow dr in _RockEng.ResultsDataTable.Rows)
            {
                repComboSection.Items.Add(dr["DisplaySect"].ToString());
            }


            LoadUserRightsData();
        }

        void LoadUserRightsData()
        {
            MWDataManager.clsDataAccess _RockEng = new MWDataManager.clsDataAccess();
            _RockEng.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection); ;
            _RockEng.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _RockEng.queryReturnType = MWDataManager.ReturnType.DataTable;
            _RockEng.SqlStatement = "exec [sp_Dept_UserRights]";
            _RockEng.ExecuteInstruction();
            dtUsers = _RockEng.ResultsDataTable;
            gcUserRight.DataSource = dtUsers;

            colUserID.FieldName = "UserID";

            colSection.FieldName = "Section";

            colDepartment.FieldName = "Department";

            ///Pre-Planning
            ///
            colPlanGeoDept.FieldName = "PreplanGeology";
            colPlanRockEngDept.FieldName = "PreplanRockEng";
            colPlanSafetyDept.FieldName = "PreplanSafety";
            colPlanSurveyDept.FieldName = "PreplanSurvey";
            colPlanVentDept.FieldName = "PreplanVentilation";
            colPlanHRDept.FieldName = "PreplanHR";
            colPlanEngDept.FieldName = "PreplanEngineering";

            ///Start-Ups
            ///
            colDeptGeology.FieldName = "StartupGeology";
            colDeptMining.FieldName = "StartupMining";
            colDeptPlanning.FieldName = "StartupPlanning";
            colDeptRockEng.FieldName = "StartupRockEng";
            colDeptSafety.FieldName = "StartupSafety";
            colDeptSurvey.FieldName = "StartupSurvey";
            colDeptVent.FieldName = "StartupVent";
            colDeptDpt.FieldName = "StartupDepartment";

            ///Bookings
            colBookDayShift.FieldName = "BK_DS";
            colBookNS.FieldName = "BK_NS";
            colBookBackDate.FieldName = "BK_BackDate";

            ///Planning
            ///
            colPlanning.FieldName = "Planning";

            ///Departmental Capture
            ///
            colCaptDept.FieldName = "CaptureDepartment";
            colCaptGeo.FieldName = "CaptureGeology";
            colCaptMining.FieldName = "CaptureMining";
            colCaptPlanning.FieldName = "CapturePlanning";
            colCaptRockEng.FieldName = "CaptureRockEng";
            colCaptSurvey.FieldName = "CaptureSurvey";
            colCaptSafety.FieldName = "CaptureSafety";
            colCaptVent.FieldName = "CaptureVent";

            colOreflowDesign.FieldName = "OreflowDesign";
            colOreflowBackfill.FieldName = "OreflowBackfill";

            colSnrMineSurveyor.FieldName = "Survey_SnrMineSurveyor";
            colGeologist.FieldName = "Survey_Geologist";
            colRockMechanic.FieldName = "Survey_RockMech";
            colSnrMinePlanner.FieldName = "Survey_SnrMinePlanner";
            colMO.FieldName = "Survey_MO";
            colSecManager.FieldName = "Survey_SecMan";
            colSurvPlanMan.FieldName = "Survey_SurvPlanMan";
            colProdManager.FieldName = "Survey_ProdMan";

            //Services
            colServicesBook.FieldName = "SDB_Booking";
            colServicesPlan.FieldName = "SDB_Plan";
            colServicesInitActTasks.FieldName = "SDB_InitializeActandTask";
            colServicesMapBluePrint.FieldName = "SDB_MapBlueprint";
            colServicesAuth.FieldName = "SDB_Authorise";

            //MOScrutiny
            colMOScrutiny.FieldName = "MO_Scrutiny";

            //OCR Schedular
            colOCRSchedular.FieldName = "OCR_Schedular";

            //Vamping
            colVampPreInsp.FieldName = "Vamping_PreInspection";
            colVampPlan.FieldName = "Vamping_Planning";
            colVampBook.FieldName = "Vamping_Booking";

            //Longhole Drilling

            colLongholeDrillingPlanning.FieldName = "LongHoleDrilling_Planning";
            colLongholeDrillingBookings.FieldName = "LongHoleDrilling_Booking";
            colLongholeDrillingSetup.FieldName = "LongHoleDrilling_Setup";



        }

        private void SaveUserRights()
        {
            gvUserRights.BeginUpdate();
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection); 
            
            string _Section = string.Empty;
            var EditedRow = dtUsers.Select("UserID = '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "UserID").ToString() + "'");
            foreach (var dr in EditedRow)
            {
                dr["Section"] = gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Section").ToString();              
            }
            dtUsers.AcceptChanges();            


            if (!string.IsNullOrEmpty(gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Section").ToString()))
                {
                    try
                    {
                        _Section = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Section").ToString());
                    }
                    catch
                    {
                        _Section = gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Section").ToString();
                    }
                }

                _dbMan.SqlStatement = "delete from tbl_Users_Synchromine where UserID = '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "UserID").ToString() + "' \r\n ";

                _dbMan.SqlStatement += " insert into tbl_Users_Synchromine (UserID, BK_DS, BK_NS,BK_BackDate, Planning, PreplanSafety, PreplanRockEng, PreplanVentilation, PreplanHR, PreplanSurvey, \r\n " +
                    " PreplanGeology, PreplanEngineering, StartupSafety, StartupRockEng, StartupPlanning, StartupSurvey, StartupGeology, StartupMining, StartupVent, StartupDepartment,  \r\n " +
                    " CaptureSafety, CaptureRockEng, CapturePlanning, CaptureSurvey, CaptureGeology, CaptureMining, CaptureVent, CaptureDepartment, OreflowDesign, OreflowBackfill, " +
                    " Survey_SnrMineSurveyor,Survey_Geologist,Survey_RockMech,Survey_SnrMinePlanner,Survey_MO,Survey_SecMan,Survey_SurvPlanMan,Survey_ProdMan,SDB_Plan,SDB_Booking,SDB_InitializeActandTask,SDB_MapBlueprint,SDB_Authorise, \r\n " +
                    " MO_Scrutiny,OCR_Schedular,Vamping_PreInspection, Vamping_Planning, Vamping_Booking,LongHoleDrilling_Planning,LongHoleDrilling_Booking,LongHoleDrilling_Setup, Section) \r\n " +
                    " VALUES ( \r\n ";
                _dbMan.SqlStatement += " '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "UserID").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "BK_DS").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "BK_NS").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "BK_BackDate").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Planning").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "PreplanSafety").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "PreplanRockEng").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "PreplanVentilation").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "PreplanHR").ToString() + "','" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "PreplanSurvey").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "PreplanGeology").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "PreplanEngineering").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "StartupSafety").ToString() + "','" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "StartupRockEng").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "StartupPlanning").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "StartupSurvey").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "StartupGeology").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "StartupMining").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "StartupVent").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "StartupDepartment").ToString() + "' \r\n "

                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "CaptureSafety").ToString() + "','" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "CaptureRockEng").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "CapturePlanning").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "CaptureSurvey").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "CaptureGeology").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "CaptureMining").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "CaptureDepartment").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "CaptureVent").ToString() + "' \r\n "

                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "OreflowDesign").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "OreflowBackfill").ToString() + "' \r\n "

                    //Survey Doc Manager

                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Survey_SnrMineSurveyor").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Survey_Geologist").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Survey_RockMech").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Survey_SnrMinePlanner").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Survey_MO").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Survey_SecMan").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Survey_SurvPlanMan").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Survey_ProdMan").ToString() + "' \r\n "

                    //Services
           
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "SDB_Plan").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "SDB_Booking").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "SDB_InitializeActandTask").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "SDB_MapBlueprint").ToString() + "' \r\n "
                    + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "SDB_Authorise").ToString() + "'  \r\n "

                    //MOScrutiny
                     + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "MO_Scrutiny").ToString() + "'  \r\n "

                     //OCRSchedular
                     + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "OCR_Schedular").ToString() + "'  \r\n "

                     //Vamping
                     + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Vamping_PreInspection").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Vamping_Planning").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "Vamping_Booking").ToString() + "' \r\n "

                     //Longhole Drilling
                     + ", '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "LongHoleDrilling_Planning").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "LongHoleDrilling_Booking").ToString() + "', '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, "LongHoleDrilling_Setup").ToString() + "' \r\n "

                    + ", '" + _Section + "') \r\n ";
            
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            gvUserRights.EndUpdate();

        }

        #endregion

        #region Events

        private void gvUserRights_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null || e.Column.Caption == string.Empty)
            {
                e.Appearance.BackColor = Color.Gray;

                return;
            }

            if (e.Column.FieldName == "PreplanGeology" || e.Column.FieldName == "PreplanHR" ||
                e.Column.FieldName == "PreplanRockEng" || e.Column.FieldName == "PreplanSafety" || e.Column.FieldName == "PreplanSurvey" ||
                e.Column.FieldName == "PreplanVentilation" || e.Column.FieldName == "PreplanEngineering" ||

                e.Column.FieldName == "StartupGeology" || e.Column.FieldName == "StartupPlanning" || e.Column.FieldName == "StartupMining" ||
                e.Column.FieldName == "StartupRockEng" || e.Column.FieldName == "StartupSafety" || e.Column.FieldName == "StartupSurvey" ||
                e.Column.FieldName == "StartupVent" || e.Column.FieldName == "StartupDepartment"
                
                || e.Column.FieldName == "BK_DS" || e.Column.FieldName == "BK_NS" || e.Column.FieldName == "BK_BackDate" || e.Column.Caption == "Planning   "

                || e.Column.Caption == "Planning " || e.Column.Caption == "Safety " || e.Column.Caption == "Survey "
                || e.Column.Caption == "Mining " || e.Column.Caption == "Geology " || e.Column.Caption == "Rock Engineering "
                || e.Column.Caption == "Department " || e.Column.Caption == "Ventilation"

               ||e.Column.FieldName == "OreflowDesign" || e.Column.FieldName == "OreflowBackfill"

               //Doc Manager              
               || e.Column.FieldName == "Survey_SnrMineSurveyor" || e.Column.FieldName == "Survey_Geologist" || e.Column.FieldName == "Survey_RockMech"
               || e.Column.FieldName == "Survey_SnrMinePlanner" || e.Column.FieldName == "Survey_MO" || e.Column.FieldName == "Survey_SecMan" 
               || e.Column.FieldName == "Survey_SurvPlanMan" || e.Column.FieldName == "Survey_ProdMan"

               //Services
               //SDB_Plan,SDB_Booking,SDB_InitializeActandTask,SDB_MapBlueprint,SDB_Authorise
               || e.Column.FieldName == "SDB_Plan" || e.Column.FieldName == "SDB_Booking" || e.Column.FieldName == "SDB_InitializeActandTask"
               || e.Column.FieldName == "SDB_MapBlueprint" || e.Column.FieldName == "SDB_Authorise"

                //MOScrutiny
                || e.Column.FieldName == "MO_Scrutiny"

                //OCR_Schedular
                || e.Column.FieldName == "OCR_Schedular"

                //Vamping
                || e.Column.FieldName == "Vamping_PreInspection" || e.Column.FieldName == "Vamping_Planning" || e.Column.FieldName == "Vamping_Booking"

                //Longhole Drilling
                || e.Column.FieldName == "LongHoleDrilling_Planning" || e.Column.FieldName == "LongHoleDrilling_Booking" || e.Column.FieldName == "LongHoleDrilling_Setup"
               )
             {
                e.Info.Caption = string.Empty;
                e.Painter.DrawObject(e.Info);

                e.Appearance.DrawVString(e.Cache, e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds,
                new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);

                e.Handled = true;

            }
        }


        private void gvUserRights_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            //try
            //{
            //	if (e.Column.FieldName == "PreplanGeology" || e.Column.FieldName == "PreplanHR" ||
            //		e.Column.FieldName == "PreplanRockEng" || e.Column.FieldName == "PreplanSafety" || e.Column.FieldName == "PreplanSurvey" ||
            //		e.Column.FieldName == "PreplanVentilation" || e.Column.FieldName == "PreplanEngineering" ||

            //		e.Column.FieldName == "StartupGeology" || e.Column.FieldName == "StartupPlanning" || e.Column.FieldName == "StartupMining" ||
            //		e.Column.FieldName == "StartupRockEng" || e.Column.FieldName == "StartupSafety" || e.Column.FieldName == "StartupSurvey" ||
            //		e.Column.FieldName == "StartupVent" || e.Column.FieldName == "StartupDepartment")
            //	{
            //		if (e.CellValue.ToString() == "2")
            //		{
            //			count += 2;
            //		}

            //		if (count == 578)
            //		{
            //			gvUserRights.SetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns["Planning   "], "2");
            //		}
            //	}
            //}
            //         catch {  }

        }

        private void gvUserRights_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //MWDataManager.clsDataAccess _RockEngSec = new MWDataManager.clsDataAccess();
            //_RockEngSec.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);;
            //_RockEngSec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_RockEngSec.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_RockEngSec.SqlStatement = "select SectionID + ':' + Name Section from tbl_Section order by Hierarchicalid, SectionID asc";
            //_RockEngSec.ExecuteInstruction();


            //if (e.Column.Caption == "Section")
            //{
            //	RepositoryItemComboBox cb = new RepositoryItemComboBox();
            //	cb.Items.Add(_RockEngSec.ResultsDataTable);
            //}
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void repComboSection_EditValueChanged(object sender, EventArgs e)
        {
            //SaveUserRights();


            //string _Section = string.Empty;

            //var EditedRow = dt.Select(
            //                               " ProdMonth = '" + clsBookingParam.ProdMonth + "' "
            //                               + "and SectionID = '" + ProbBook.TheSection + "' "
            //                               + "and WP = '" + ProbBook.TheWorkpalce + "' "
            //                               + "and CalendarDate = '" + clsBookingParam.BookDate + "'"
            //                               + "and Activity = '" + ProbBook.TheActivity + "' ");
            //foreach (var dr in EditedRow)
            //{
            //    dr["ProblemID"] = ProbBook.ProblemID;
            //    dr["SBossNotes"] = ProbBook.SBossNotes;
            //    dr["CausedLostBlast"] = _lost;
            //}
            //clsBookingParam.dtStopingBooking.AcceptChanges();
            //btnSave.Enabled = true;


            //gvUserRights.BeginUpdate();

            //if (!string.IsNullOrEmpty(gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns["Section"]).ToString()))
            //{
            //    try
            //    {
            //        _Section = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns["Section"]).ToString());
            //    }
            //    catch
            //    {
            //        _Section = gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns["Section"]).ToString();
            //    }
            //}

            //MWDataManager.clsDataAccess _RockEngSec = new MWDataManager.clsDataAccess();
            //_RockEngSec.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, this.UserCurrentInfo.Connection);;
            //_RockEngSec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_RockEngSec.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_RockEngSec.SqlStatement = "update tbl_Users_Synchromine set Section = '" + _Section + "' where UserID = '" + gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns["UserID"]).ToString() + "' ";
            //_RockEngSec.ExecuteInstruction();
            //gvUserRights.EndUpdate();

        }

        private void gvUserRights_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            SaveUserRights();
        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWordEditor helpFrm = new frmWordEditor();
            helpFrm.ViewType = "View";
            helpFrm.MainCat = "SystemAdmin";
            helpFrm.SubCat = "ProdactionUserSetup";
            helpFrm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void gvUserRights_DoubleClick(object sender, EventArgs e)
        {
            colCaption = gvUserRights?.FocusedColumn?.Caption;

            Form ucDept_Capt = new Form();
            ucDept_Capt.Controls.Clear();

            string s = gvUserRights.FocusedColumn.FieldName;

            ///Authorise User
            if (//gvUserRights.FocusedColumn.FieldName != "UserID" || gvUserRights.FocusedColumn.FieldName != "Section"
                 gvUserRights.FocusedColumn.FieldName == "PreplanHR" || gvUserRights.FocusedColumn.FieldName == "PreplanEngineering"
                || gvUserRights.FocusedColumn.FieldName == "PreplanGeology" || gvUserRights.FocusedColumn.FieldName == "PreplanVentilation"
                || gvUserRights.FocusedColumn.FieldName == "PreplanPlanning" || gvUserRights.FocusedColumn.FieldName == "PreplanRockEng"
                || gvUserRights.FocusedColumn.FieldName == "PreplanSurvey" || gvUserRights.FocusedColumn.FieldName == "PreplanSafety"
                || gvUserRights.FocusedColumn.FieldName == "StartupGeology" || gvUserRights.FocusedColumn.FieldName == "StartupMining"
                || gvUserRights.FocusedColumn.FieldName == "StartupPlanning" || gvUserRights.FocusedColumn.FieldName == "StartupSafety"
                || gvUserRights.FocusedColumn.FieldName == "StartupRockEng" || gvUserRights.FocusedColumn.FieldName == "StartupSurvey"
                || gvUserRights.FocusedColumn.FieldName == "StartupVent" || gvUserRights.FocusedColumn.FieldName == "StartupDepartment"
                
                )
            {
                if (gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns[gvUserRights.FocusedColumn.FieldName]).ToString() != "0")
                {
                    if (gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns[gvUserRights.FocusedColumn.FieldName]).ToString() == "1")
                    {
                        AuthNum = "2";
                    }
                    else
                    {
                        AuthNum = "0";
                    }
                }
                else
                {
                    AuthNum = "1";
                }
                gvUserRights.SetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns[gvUserRights.FocusedColumn.FieldName], AuthNum);
                SaveUserRights();

            }

            //Planning And Dept. Captures
            if (gvUserRights.FocusedColumn.FieldName == "Planning" || gvUserRights.FocusedColumn.FieldName == "CaptureDepartment"
                || gvUserRights.FocusedColumn.FieldName == "CaptureGeology" || gvUserRights.FocusedColumn.FieldName == "CaptureMining"
                || gvUserRights.FocusedColumn.FieldName == "CapturePlanning" || gvUserRights.FocusedColumn.FieldName == "CaptureRockEng"
                || gvUserRights.FocusedColumn.FieldName == "CaptureSurvey" || gvUserRights.FocusedColumn.FieldName == "CaptureSafety"
                || gvUserRights.FocusedColumn.FieldName == "CaptureVent" || gvUserRights.FocusedColumn.FieldName == "BK_DS"
                || gvUserRights.FocusedColumn.FieldName == "BK_NS" || gvUserRights.FocusedColumn.FieldName == "BK_BackDate"
                || gvUserRights.FocusedColumn.FieldName == "OreflowDesign" || gvUserRights.FocusedColumn.FieldName == "OreflowBackfill"
                || gvUserRights.FocusedColumn.FieldName == "Survey_SnrMineSurveyor" || gvUserRights.FocusedColumn.FieldName == "Survey_Geologist" || gvUserRights.FocusedColumn.FieldName == "Survey_RockMech"
                || gvUserRights.FocusedColumn.FieldName == "Survey_SnrMinePlanner" || gvUserRights.FocusedColumn.FieldName == "Survey_MO" || gvUserRights.FocusedColumn.FieldName == "Survey_SecMan"
                || gvUserRights.FocusedColumn.FieldName == "Survey_SurvPlanMan" || gvUserRights.FocusedColumn.FieldName == "Survey_ProdMan"
                //Services
                || gvUserRights.FocusedColumn.FieldName == "SDB_Plan" || gvUserRights.FocusedColumn.FieldName == "SDB_Booking"
                || gvUserRights.FocusedColumn.FieldName == "SDB_InitializeActandTask" || gvUserRights.FocusedColumn.FieldName == "SDB_MapBlueprint"
                || gvUserRights.FocusedColumn.FieldName == "SDB_Authorise"

                || gvUserRights.FocusedColumn.FieldName == "MO_Scrutiny" || gvUserRights.FocusedColumn.FieldName == "OCR_Schedular"

                || gvUserRights.FocusedColumn.FieldName == "Vamping_PreInspection" || gvUserRights.FocusedColumn.FieldName == "Vamping_Planning" || gvUserRights.FocusedColumn.FieldName == "Vamping_Booking"

                 || gvUserRights.FocusedColumn.FieldName == "LongHoleDrilling_Planning" || gvUserRights.FocusedColumn.FieldName == "LongHoleDrilling_Booking" || gvUserRights.FocusedColumn.FieldName == "LongHoleDrilling_Setup"

                )
            {
                if (gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns[gvUserRights.FocusedColumn.FieldName]).ToString() == "0")
                {
                    AuthNum2 = "1";
                }

                if (gvUserRights.GetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns[gvUserRights.FocusedColumn.FieldName]).ToString() == "1")
                {
                    AuthNum2 = "0";
                }
                gvUserRights.SetRowCellValue(gvUserRights.FocusedRowHandle, gvUserRights.Columns[gvUserRights.FocusedColumn.FieldName], AuthNum2);
                SaveUserRights();
            }

        }


        #endregion

    }
}
