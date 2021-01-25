using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionHelp;
using System;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class ucStopWorkplace : BaseUserControl
    {
        private string Workplace;
        private string WorkplaceID;
        public ucStopWorkplace()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpWorkplace);
            FormActiveRibbonPage = rpWorkplace;
            FormMainRibbonPage = rpWorkplace;
            RibbonControl = rcWorkplace;
        }

        private void ucStopWorkplace_Load(object sender, EventArgs e)
        {
            LoadWorkplaces("0");
        }

        void LoadWorkplaces(string Activity)
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan1.SqlStatement = " select w.*, e.Description Endtype, r.ShortDesc Reef, o.name Level1, wt.WpSublocation mpras, wt.WPExternalid ext1, st.StoppageType, st.Comments " +
                                        " from tbl_workplace w \r\n" +
                                        "left outer join tbl_Code_EndType e on  w.endtypeid = e.endtypeid \r\n" +
                                        "left outer join tbl_Code_Reef r on  w.reefid = r.reefid \r\n" +
                                        "left outer join tbl_OreFlowEntities o on  w.oreflowid = o.oreflowid \r\n" +
                                        "left outer join tbl_Workplace_Total wt on w.GMSIWPID = wt.GMSIWPID \r\n" +
                                        "left outer join tbl_Workplace_Stoppages st on w.WorkplaceID = st.WorkplaceID \r\n" +
                                        " where w.Activity = '" + Activity + "'  " +
                                        "order by workplaceid ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable dtWp = _dbMan1.ResultsDataTable;

            DataSet dsWp = new DataSet();

            dsWp.Tables.Add(dtWp);



            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan2.SqlStatement = " select w.*, e.Description Endtype, r.ShortDesc Reef, o.name Level1, wt.WpSublocation mpras, wt.WPExternalid ext1, st.StoppageDate, st.StoppageType, st.Distance, st.Comments " +
                                        " from tbl_workplace w \r\n" +
                                        "left outer join tbl_Code_EndType e on  w.endtypeid = e.endtypeid \r\n" +
                                        "left outer join tbl_Code_Reef r on  w.reefid = r.reefid \r\n" +
                                        "left outer join tbl_OreFlowEntities o on  w.oreflowid = o.oreflowid \r\n" +
                                        "left outer join tbl_Workplace_Total wt on w.GMSIWPID = wt.GMSIWPID \r\n" +
                                        "inner join tbl_Workplace_Stoppages_Detail st on w.WorkplaceID = st.WorkplaceID \r\n" +
                                        " where w.Activity = '" + Activity + "'  " +
                                        "order by workplaceid ";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ExecuteInstruction();

            DataTable dtDetail = _dbMan2.ResultsDataTable;

            dsWp.Tables.Add(dtDetail);

            dsWp.Relations.Clear();

            DataColumn keyColumn1 = dsWp.Tables[0].Columns[0];
            DataColumn foreignKeyColumn1 = dsWp.Tables[1].Columns[0];
            dsWp.Relations.Add("CategoriesProducts", keyColumn1, foreignKeyColumn1);


            gcWorkplaces.DataSource = dsWp.Tables[0];

            colWpID.FieldName = "WorkplaceID";
            colWpName.FieldName = "Description";
            colStopType.FieldName = "StoppageType";
            colComments.FieldName = "Comments";

            gcWorkplaces.LevelTree.Nodes.Add("CategoriesProducts", gvDetail);
            gvDetail.ViewCaption = "Workplace Detail";

            gcWorkplaces.DataSource = dsWp.Tables[0];

            colWPIDDetail.FieldName = "WorkplaceID";
            colWPDetail.FieldName = "Description";
            colStoppageDate.FieldName = "StoppageDate";
            colDist.FieldName = "Distance";
            colStoppageType.FieldName = "StoppageType";
            colDetailComments.FieldName = "Comments";






        }

        private void barActivity_EditValueChanged(object sender, EventArgs e)
        {
            if (barActivity.EditValue.ToString() == "0")
            {
                LoadWorkplaces("0");
            }
            else
            { LoadWorkplaces("1"); }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmStopWorkplace frm = new frmStopWorkplace();
            frm._connectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            frm._activity = Convert.ToInt32(barActivity.EditValue);
            frm._workplace = Workplace;
            frm._WPID = WorkplaceID;
            frm._date = System.DateTime.Now;
            frm.ShowDialog();

            if (barActivity.EditValue.ToString() == "0")
            {
                LoadWorkplaces("0");
            }
            else
            { LoadWorkplaces("1"); }
        }

        private void gvWorkplaces_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                Workplace = gvWorkplaces.GetRowCellValue(gvWorkplaces.FocusedRowHandle, gvWorkplaces.Columns["Description"]).ToString();
                WorkplaceID = gvWorkplaces.GetRowCellValue(gvWorkplaces.FocusedRowHandle, gvWorkplaces.Columns["WorkplaceID"]).ToString();
            }
            catch { return; }
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWordEditor helpFrm = new frmWordEditor();
            helpFrm.ViewType = "View";
            helpFrm.MainCat = "SystemAdmin";           
            helpFrm.SubCat = "WorkplaceStoppages";            
            helpFrm.Show();
        }
    }
}
