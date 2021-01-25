using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class ucMillingBooking : BaseUserControl
    {
        #region Public variables
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        #endregion

        public ucMillingBooking()
        {
            InitializeComponent();
            FormRibbonPages.Add(ribbonPage1);
            FormActiveRibbonPage = ribbonPage1;
            FormMainRibbonPage = ribbonPage1;
            RibbonControl = ribbonControl1;
        }

        private void ucMillingBooking_Load(object sender, EventArgs e)
        {
            barProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString());
            LoadMills();
            LoadMainGrid();
        }

        private void LoadMills()
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
                if (r["oreflowcode"].ToString() == "Mill")
                {
                    lbxMill.Items.Add(r["name"].ToString());
                }
            }
            lbxMill.SelectedIndex = 0;
        }

        private void LoadMainGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "select '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "' pmmonth, a.calendardate thedate, a.workingday \r\n" +
                                  ",isnull(ActualTons,0) ActualTons,isnull(ConcDesp,0) ConcDesp,isnull(KGProd,0) KGProd,isnull(KGDesp,0) KGDesp \r\n" +
                                  ",isnull(PerAct, 0) PerAct,isnull(PerProg, 0) PerProg,isnull(ActualGrade, 0) ActualGrade,isnull(ProgGrade, 0) ProgGrade \r\n" +
                                  ",isnull(Residue, 0) Residue,isnull(Remarks, '') Remarks,isnull(ProgRes, 0) ProgRes,isnull(SifftedTons, 0) SifftedTons \r\n" +
                                  ",isnull(runtime, 0) runtime " +
                                  ",'N' Checked from (select * from dbo.tbl_Code_Calendar_Type \r\n" +
                                  "where calendarcode = 'Mill' \r\n" +
                                  "and calendardate >= (select max(startdate)  from dbo.tbl_CalendarMill where \r\n" +
                                  "millmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "') \r\n" +

                                  "and calendardate <= (select max(enddate)  from dbo.tbl_CalendarMill where \r\n" +
                                  "millmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "')) a \r\n" +

                                  "left outer join \r\n" +

                                  "(select name, b.* from dbo.tbl_Milling b, tbl_oreflowentities o  where \r\n" +
                                  "b.oreflowid = o.oreflowid and \r\n" +
                                  "o.name = '" + lbxMill.Text + "' \r\n" +
                                  "and calendardate >= (select max(startdate)  from dbo.tbl_CalendarMill where \r\n" +
                                  "millmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "') \r\n" +

                                  "and calendardate <= (select max(enddate)  from dbo.tbl_CalendarMill where \r\n" +
                                  "millmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue.ToString())) + "') ) b on a.calendardate = b.calendardate \r\n" +

                                  "order by a.calendardate ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtdata = _dbMan.ResultsDataTable;
            if (dtdata.Rows.Count == 0)
            {
                MessageBox.Show("No Mill Month has been set up", "No Mill Month", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            gcMillBooking.DataSource = null;
            gcMillBooking.DataSource = dtdata;

            colDate.FieldName = "thedate";//0
            colTonnes.FieldName = "ActualTons";//1
            colPulpGrade.FieldName = "ConcDesp";//2
            colSifttedTons.FieldName = "SifftedTons";//3
            colKgDesp.FieldName = "KGDesp";//4
            colPercAct.FieldName = "PerAct";//5
            colPercProg.FieldName = "PerProg";//6
            colActualGrade.FieldName = "ActualGrade";//7
            colProgGrade.FieldName = "ProgGrade";//8
            colResidue.FieldName = "Residue";//9
            colProgResidue.FieldName = "ProgRes";//10
            colSiloTons.FieldName = "runtime";//11
            colSapare.FieldName = string.Empty;//12
            colRemarks.FieldName = "Remarks";//13
            colChecked.FieldName = "Checked";
        }

        private void gvMillBooking_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "ConcDesp" || e.Column.FieldName == "ActualTons" || e.Column.FieldName == "Remarks")
            {
                gvMillBooking.SetRowCellValue(e.RowHandle, gvMillBooking.Columns["Checked"], "Y");
            }

        }

        private void barbtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan1.SqlStatement = "select * from tbl_oreflowentities where name = '" + lbxMill.Text + "' ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable Neil = _dbMan1.ResultsDataTable;
            string mill = Neil.Rows[0]["oreflowid"].ToString();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "select '' a ";
            for (int k = 0; k <= gvMillBooking.RowCount - 1; k++)
            {
                string IsChecked = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["Checked"]).ToString();
                string TheDate = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["thedate"]).ToString();
                string ActualTons = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["ActualTons"]).ToString();
                string ConcDesp = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["ConcDesp"]).ToString();
                string KGDesp = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["KGDesp"]).ToString();
                string PerAct = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["PerAct"]).ToString();
                string PerProg = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["PerProg"]).ToString();
                string ActualGrade = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["ActualGrade"]).ToString();
                string ProgGrade = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["ProgGrade"]).ToString();
                string Residue = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["Residue"]).ToString();
                string Remarks = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["Remarks"]).ToString();
                string ProgRes = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["ProgRes"]).ToString();
                string SiloTons = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["runtime"]).ToString();
                string SiffedTons = gvMillBooking.GetRowCellValue(k, gvMillBooking.Columns["SifftedTons"]).ToString();

                if (IsChecked == "Y")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " Delete from tbl_Milling where oreflowid = '" + mill + "' and ";
                    _dbMan.SqlStatement = _dbMan.SqlStatement + "Calendardate = '" + TheDate + "' ";

                    _dbMan.SqlStatement = _dbMan.SqlStatement + "Insert into tbl_Milling values ('" + mill + "', '" + TheDate + "' ,";
                    _dbMan.SqlStatement = _dbMan.SqlStatement + "'" + ActualTons + "' , '" + ConcDesp + "', '0', ";
                    _dbMan.SqlStatement = _dbMan.SqlStatement + "'" + KGDesp + "', '" + PerAct + "', '" + PerProg + "' , ";
                    _dbMan.SqlStatement = _dbMan.SqlStatement + "'" + ActualGrade + "', '" + ProgGrade + "', '" + Residue + "' , ";
                    _dbMan.SqlStatement = _dbMan.SqlStatement + "'" + Remarks + "', '" + ProgRes + "', '" + SiloTons + "','" + SiffedTons + "' ) ";

                    gvMillBooking.SetRowCellValue(k, gvMillBooking.Columns["Checked"], "N");
                }
            }

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            var result = _dbMan.ExecuteInstruction();
            if (result.success)
            {
                Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
