using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class ucWorkplaces : BaseUserControl
    {
        public ucWorkplaces()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpWorkplace);
            FormActiveRibbonPage = rpWorkplace;
            FormMainRibbonPage = rpWorkplace;
            RibbonControl = rcWorkplace;
        }

        private void ucWorkplaces_Load(object sender, EventArgs e)
        {
            LoadWorkplaces();
            LoadEndType();
            LoadReefType();
        }

        ///Load Functions/Methods
        ///Workplaces function
        void LoadWorkplaces()
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan1.SqlStatement = " select w.*, e.Description Endtype, r.ShortDesc Reef, o.name Level1, wt.WpSublocation mpras, wt.WPExternalid ext1 from tbl_workplace w \r\n" +
                                        "left outer join tbl_Code_EndType e on  w.endtypeid = e.endtypeid \r\n" +
                                        "left outer join tbl_Code_Reef r on  w.reefid = r.reefid \r\n" +
                                        "left outer join tbl_OreFlowEntities o on  w.oreflowid = o.oreflowid \r\n" +
                                        "left outer join tbl_Workplace_Total wt on w.GMSIWPID = wt.GMSIWPID \r\n" +
                                        "  " +
                                        "order by workplaceid ";

            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable dtWp = _dbMan1.ResultsDataTable;

            DataSet dsWp = new DataSet();

            dsWp.Tables.Add(dtWp);

            gcWorkplaces.DataSource = dsWp.Tables[0];

            colWpID.FieldName = "WorkplaceID";
            colWpName.FieldName = "Description";
            colRw.FieldName = "ReefWaste";
            colReefType.FieldName = "Reef";
            colLine.FieldName = "Line";
            colLevel.FieldName = "Level1";
            colRR.FieldName = "RiskRating";
            colGMSIwpID.FieldName = "GMSIWPID";
        }

        ///End Type function
        void LoadEndType()
        {
            MWDataManager.clsDataAccess _dbManEnd = new MWDataManager.clsDataAccess();
            _dbManEnd.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManEnd.SqlStatement = " select *,case when Mach = 'Y' then 'Yes' else 'No' end as MachNew from tbl_Code_EndType order by endtypeid";
            _dbManEnd.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManEnd.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManEnd.ExecuteInstruction();

            DataTable dtEndType = _dbManEnd.ResultsDataTable;
            DataSet dsEndType = new DataSet();
            dsEndType.Tables.Add(dtEndType);

            gcEndType.DataSource = dsEndType.Tables[0];

            colEndTypeID.FieldName = "EndTypeID";
            colEndDecs.FieldName = "Description";
            colEndHeaight.FieldName = "EndHeight";
            colEndWidth.FieldName = "EndWidth";
            colEndProcessCode.FieldName = "ProcessCode";
            colEndReefWaste.FieldName = "ReefWaste";
            colEndRate.FieldName = "Rate";
            colEndDetRatio.FieldName = "DetRatio";
            colEndMach.FieldName = "MachNew";
        }

        ///Reef Type function
        ///
        void LoadReefType()
        {
            MWDataManager.clsDataAccess _dbManReef = new MWDataManager.clsDataAccess();
            _dbManReef.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManReef.SqlStatement = " select * from tbl_Code_Reef ";
            _dbManReef.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManReef.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManReef.ExecuteInstruction();

            DataTable dtReefType = _dbManReef.ResultsDataTable;

            DataSet dsReef = new DataSet();
            dsReef.Tables.Add(dtReefType);

            gcReefType.DataSource = dsReef.Tables[0];

            colReefID.FieldName = "ReefID";
            colDescription.FieldName = "ShortDesc";
            colDefaultAdv.FieldName = "DefaultAdv";
            colDetRatio.FieldName = "DetRatio";
        }

        private void pnlMainWP_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSaveReefType_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManReef = new MWDataManager.clsDataAccess();
            _dbManReef.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManReef.SqlStatement = " ";

            for (int row = 0; row < gvReefType.RowCount; row++)
            {
                string reefid = gvReefType.GetRowCellValue(row, gvReefType.Columns["ReefID"]).ToString();
                string Detratio = gvReefType.GetRowCellValue(row, gvReefType.Columns["DetRatio"]).ToString();

                _dbManReef.SqlStatement = _dbManReef.SqlStatement + "update tbl_Code_Reef set Detratio = '" + Detratio + "' where reefid = '" + reefid + "' \r\n";
            }

            _dbManReef.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManReef.queryReturnType = MWDataManager.ReturnType.DataTable;
            clsDataResult result = _dbManReef.ExecuteInstruction();

            if (result.success == true)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Reef Type Updated", Color.CornflowerBlue);
                LoadReefType();
            }


        }

        private void btnSaveEndType_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManReef = new MWDataManager.clsDataAccess();
            _dbManReef.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManReef.SqlStatement = " ";

            for (int row = 0; row < gvEndType.RowCount; row++)
            {
                string reefid = gvEndType.GetRowCellValue(row, gvEndType.Columns["EndTypeID"]).ToString();
                string Detratio = gvEndType.GetRowCellValue(row, gvEndType.Columns["DetRatio"]).ToString();
                string Rate = gvEndType.GetRowCellValue(row, gvEndType.Columns["Rate"]).ToString();
                string Mach = gvEndType.GetRowCellValue(row, gvEndType.Columns["MachNew"]).ToString();

                if (Mach == "Yes")
                {
                    Mach = "Y";
                }

                if (Mach == "No")
                {
                    Mach = "N";
                }

                _dbManReef.SqlStatement = _dbManReef.SqlStatement + "update tbl_Code_EndType set Rate = '" + Rate + "',Detratio = '" + Detratio + "' , mach = '" + Mach + "' where EndTypeID = '" + reefid + "' \r\n";
            }

            _dbManReef.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManReef.queryReturnType = MWDataManager.ReturnType.DataTable;
            clsDataResult result = _dbManReef.ExecuteInstruction();

            if (result.success == true)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "End Type Updated", Color.CornflowerBlue);
                LoadEndType();
            }
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
