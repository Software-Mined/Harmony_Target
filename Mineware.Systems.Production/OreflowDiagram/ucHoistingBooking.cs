using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class ucHoistingBooking : BaseUserControl
    {
        #region Public variables
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        #endregion

        public ucHoistingBooking()
        {
            InitializeComponent();
            FormRibbonPages.Add(ribbonPage1);
            FormActiveRibbonPage = ribbonPage1;
            FormMainRibbonPage = ribbonPage1;
            RibbonControl = ribbonControl1;
        }

        private void ucHoistingBooking_Load(object sender, EventArgs e)
        {
            barProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString());
            LoadShafts();
            LoadMainGrid();
        }

        private void LoadShafts()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "select name, oreflowcode from tbl_oreflowentities " +
                                    " order by oreflowcode, name ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable Neil = _dbMan.ResultsDataTable;

            foreach (DataRow r in Neil.Rows)
            {
                if (r["oreflowcode"].ToString() == "Shaft")
                {
                    lbxShaft.Items.Add(r["name"].ToString());
                }
            }
            lbxShaft.SelectedIndex = 0;
        }

        private void LoadMainGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "select '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "' pmmonth, a.calendardate thedate, a.workingday, \r\n" +
                                  "isnull(b.ReefSkips,0) ReefSkips,isnull(b.ReefTons,0) ReefTons,isnull(b.ReefFact,0) ReefFact, \r\n" +
                                  "isnull(b.WasteSkips, 0) WasteSkips,isnull(b.WasteTons, 0) WasteTons,isnull(b.WasteFact, 0) WasteFact,isnull(b.Remarks, '') Remarks \r\n" +
                                  ",'N' Checked from (select * from dbo.tbl_Code_Calendar_Type \r\n" +
                                  "where calendarcode = 'Mill' \r\n" +
                                  "and calendardate >= (select max(startdate)  from dbo.tbl_CalendarMill where \r\n" +
                                  "millmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "') \r\n" +

                                  "and calendardate <= (select max(enddate)  from dbo.tbl_CalendarMill where \r\n" +
                                  "millmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "')) a \r\n" +

                                  "left outer join \r\n" +

                                  "(select name, b.* from dbo.tbl_Booking_Hoisting b, tbl_oreflowentities o  where \r\n" +
                                  "b.oreflowid = o.oreflowid and \r\n" +
                                  "o.name = '" + lbxShaft.Text + "' \r\n" +
                                  "and calendardate >= (select max(startdate)  from dbo.tbl_CalendarMill where \r\n" +
                                  "millmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "') \r\n" +

                                  "and calendardate <= (select max(enddate)  from dbo.tbl_CalendarMill where \r\n" +
                                  "millmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "') ) b on a.calendardate = b.calendardate \r\n" +

                                  "order by a.calendardate ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtData = _dbMan.ResultsDataTable;

            if (dtData.Rows.Count == 0)
            {
                MessageBox.Show("No Mill Month has been set up", "No Mill Month", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            gcHoistBooking.DataSource = null;
            gcHoistBooking.DataSource = dtData;
            colDate.FieldName = "thedate";//0
            colCheck.FieldName = "Checked";//9
            colFactor.FieldName = "ReefFact";//2
            colFactor1.FieldName = "WasteFact";//5
            colHoppers.FieldName = "thedate";//8
            colReefTons.FieldName = "ReefTons";//3
            colSkip.FieldName = "ReefSkips";//1
            colSkip1.FieldName = "WasteSkips";//4
            colTons.FieldName = "ReefTons";//7
            colWasteTons.FieldName = "WasteTons";//6

            colReefTons.OptionsColumn.AllowEdit = true;
            colWasteTons.OptionsColumn.AllowEdit = true;
        }

        private void gvHoistBooking_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "ReefTons")
            {
                decimal ReefFact = 0;
                decimal ReefSkips = Convert.ToDecimal(gvHoistBooking.GetRowCellValue(e.RowHandle, gvHoistBooking.Columns["ReefSkips"]));
                decimal ReefTons = Convert.ToDecimal(gvHoistBooking.GetRowCellValue(e.RowHandle, gvHoistBooking.Columns["ReefTons"]));
                if (ReefSkips > 0)
                {
                    ReefFact = Math.Round(Convert.ToDecimal(ReefTons) / (ReefSkips), 3);
                    gvHoistBooking.SetRowCellValue(e.RowHandle, gvHoistBooking.Columns["ReefFact"], ReefFact);
                }

                gvHoistBooking.SetRowCellValue(e.RowHandle, gvHoistBooking.Columns["Checked"], "Y");
            }

            if (e.Column.FieldName == "WasteTons")
            {
                decimal WasteFact = 0;
                decimal WasteSkips = Convert.ToDecimal(gvHoistBooking.GetRowCellValue(e.RowHandle, gvHoistBooking.Columns["WasteSkips"]));
                decimal WasteTons = Convert.ToDecimal(gvHoistBooking.GetRowCellValue(e.RowHandle, gvHoistBooking.Columns["WasteTons"]));
                if (WasteSkips > 0)
                {
                    WasteFact = Math.Round(WasteTons / (WasteSkips), 3);
                    gvHoistBooking.SetRowCellValue(e.RowHandle, gvHoistBooking.Columns["WasteFact"], WasteFact);
                }

                gvHoistBooking.SetRowCellValue(e.RowHandle, gvHoistBooking.Columns["Checked"], "Y");
            }
        }

        private void repSpinEditTons_EditValueChanged(object sender, EventArgs e)
        {
            gvHoistBooking.PostEditor();
        }

        private void barbtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan1.SqlStatement = "select * from tbl_oreflowentities where name = '" + lbxShaft.Text + "' ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable Neil = _dbMan1.ResultsDataTable;
            string Shaft = Neil.Rows[0]["oreflowid"].ToString();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " ";
            for (int k = 0; k <= gvHoistBooking.RowCount - 1; k++)
            {
                string Check = gvHoistBooking.GetRowCellValue(k, gvHoistBooking.Columns["Checked"]).ToString();
                string TheDate = gvHoistBooking.GetRowCellValue(k, gvHoistBooking.Columns["thedate"]).ToString();

                string ReefSkips = gvHoistBooking.GetRowCellValue(k, gvHoistBooking.Columns["ReefSkips"]).ToString();
                string ReefFact = gvHoistBooking.GetRowCellValue(k, gvHoistBooking.Columns["ReefFact"]).ToString();
                string ReefTons = gvHoistBooking.GetRowCellValue(k, gvHoistBooking.Columns["ReefTons"]).ToString();

                string WasteSkips = gvHoistBooking.GetRowCellValue(k, gvHoistBooking.Columns["WasteSkips"]).ToString();
                string WasteFact = gvHoistBooking.GetRowCellValue(k, gvHoistBooking.Columns["WasteFact"]).ToString();
                string WasteTons = gvHoistBooking.GetRowCellValue(k, gvHoistBooking.Columns["WasteTons"]).ToString();

                if (Check == "Y")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " Delete from tbl_Booking_Hoisting where oreflowid = '" + Shaft + "' and \r\n" +
                                                                "Calendardate = '" + TheDate + "' \r\n" +

                                                                "Insert into tbl_Booking_Hoisting values ('" + TheDate + "', '" + Shaft + "','" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "', \r\n" +
                                                                "'" + ReefSkips + "' , '" + ReefTons + "', '" + ReefFact + "', \r\n" +
                                                                "'" + WasteSkips + "', '" + WasteTons + "', '" + WasteFact + "' ,'') ";

                    gvHoistBooking.SetRowCellValue(k, gvHoistBooking.Columns["Checked"], "N");
                }
            }

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            var result = _dbMan.ExecuteInstruction();
            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
