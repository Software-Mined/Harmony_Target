using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class ucTrammingBooking : UserControl
    {
        #region Public variables
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        #endregion

        public ucTrammingBooking()
        {
            InitializeComponent();
        }

        private void ucTrammingBooking_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManLvl = new MWDataManager.clsDataAccess();
            _dbManLvl.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManLvl.SqlStatement = "select name, oreflowcode from tbl_oreflowentities where parentoreflowid = 'S5523' " +
                                    " order by oreflowcode, name ";
            _dbManLvl.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLvl.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManLvl.ExecuteInstruction();

            DataTable Neil = _dbManLvl.ResultsDataTable;

            foreach (DataRow r in Neil.Rows)
            {
                if (r["oreflowcode"].ToString() == "lvl")
                {
                    lbxBHlvl.Items.Add(r["name"].ToString());
                }
            }
            lbxBHlvl.SelectedIndex = 0;

            LoadBookingGrid();
        }

        private void LoadBookingGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select a.*, o.name, o.parentoreflowid, o.reeftype, o1.hopperfactor, dayhoppers, daytons \r\n" +
                        ", afthoppers, afttons , nhoppers, ntons, o1.name lvl from (select distinct(mainBHID) from (select distinct(OreFlowId) mainBHID from tbl_planning p, tbl_planmonth pm \r\n" +
                        "where p.sectionid = pm.sectionid and p.workplaceid = pm.workplaceid \r\n" +
                        "and p.prodmonth = pm.prodmonth and p.activity = pm.activity and \r\n" +

                        "calendardate = '" + String.Format("{0:yyyy-MM-dd}", TramDate.Value) + "' \r\n" +

                        " union \r\n" +
                        " select distinct(bh) aa from (select substring(Sectionid,1,6) aaa, * from dbo.tbl_PLANNING_Vamping where calendardate = '" + String.Format("{0:yyyy-MM-dd}", TramDate.Value) + "') a, \r\n" +
                        " (select * from tbl_Code_Calendar_Section where BeginDate  <= '" + String.Format("{0:yyyy-MM-dd}", TramDate.Value) + "' and EndDate >= '" + String.Format("{0:yyyy-MM-dd}", TramDate.Value) + "') b \r\n" +
                        " where a.aaa = b.Sectionid and a.Prodmonth = b.Prodmonth) a) a \r\n" +

                        "left outer join \r\n" +
                        "tbl_oreflowentities o on a.mainBHID = o.oreflowid \r\n" +

                        "left outer join \r\n" +
                        " tbl_oreflowentities o1 on o.parentoreflowid = o1.oreflowid \r\n" +
                        "left outer join \r\n" +

                        "(select fromoreflowid, sum(tons) DayTons,sum( hoppers) DayHoppers from dbo.tbl_Booking_Oreflow where calendardate = '" + String.Format("{0:yyyy-MM-dd}", TramDate.Value) + "' \r\n" +
                        "and shift = '" + 'D' + "' group by fromoreflowid) dayshift on a.mainBHID = dayshift.fromoreflowid \r\n" +
                        "left outer join \r\n" +

                        "(select fromoreflowid,  sum(tons) AftTons,  sum(hoppers) AftHoppers from dbo.tbl_Booking_Oreflow where calendardate = '" + String.Format("{0:yyyy-MM-dd}", TramDate.Value) + "' \r\n" +
                        "and shift = '" + 'A' + "' group by fromoreflowid) Aft on a.mainBHID = aft.fromoreflowid \r\n" +

                        "left outer join \r\n" +

                        "(select fromoreflowid,  sum(tons) NTons,  sum(hoppers) NHoppers from dbo.tbl_Booking_Oreflow where calendardate = '" + String.Format("{0:yyyy-MM-dd}", TramDate.Value) + "' \r\n" +
                        "and shift = '" + 'N' + "' group by fromoreflowid) N on a.mainBHID = N.fromoreflowid \r\n");
            string aa = "Waste";

            if (BHRadio.SelectedIndex == 1)
            {
                sb.AppendLine("where o.reeftype = '" + aa.ToString() + "' \r\n");
            }
            else
            {
                sb.AppendLine("where o.reeftype <> '" + aa.ToString() + "' \r\n");
            }
            if (lbxBHlvl.SelectedItem.ToString() == "All")
            {
                sb.AppendLine("and o1.name <> '" + aa.ToString() + "' ");
            }
            else
            {
                sb.AppendLine("and o1.name = '" + lbxBHlvl.SelectedItem.ToString() + "' ");
            }
            sb.AppendLine("order by o.name ");
            _dbMan.SqlStatement = sb.ToString();
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            gcTramBooking.DataSource = null;
            gcTramBooking.DataSource = _dbMan.ResultsDataTable; ;

            colBHID.FieldName = "mainBHID";
            colBHName.FieldName = "name";
            colDSHopper.FieldName = "dayhoppers";
            colDSTons.FieldName = "daytons";
            colASHopper.FieldName = "afthoppers";
            colASTons.FieldName = "afttons";
            colNSHopper.FieldName = "ntons";
            colNSTons.FieldName = "nhoppers";
        }

        private void lbxBHlvl_DoubleClick(object sender, EventArgs e)
        {
            LoadBookingGrid();
        }

        private void TramDate_ValueChanged(object sender, EventArgs e)
        {
            LoadBookingGrid();
        }

        private void gvTramBooking_DoubleClick(object sender, EventArgs e)
        {
            string BHID = gvTramBooking.GetRowCellValue(gvTramBooking.FocusedRowHandle, gvTramBooking.Columns["mainBHID"]).ToString() + ":" + gvTramBooking.GetRowCellValue(gvTramBooking.FocusedRowHandle, gvTramBooking.Columns["name"]).ToString();

            frmTramBooking frmBook = new frmTramBooking();
            frmBook.BHLBL.Text = BHID;
            frmBook.TramDateProp.Value = TramDate.Value;
            frmBook._theSystemDBTag = this._theSystemDBTag;
            frmBook._UserCurrentInfoConnection = _UserCurrentInfoConnection;
            frmBook.StartPosition = FormStartPosition.CenterScreen;
            frmBook.ShowDialog();

            LoadBookingGrid();
        }

        private void BHRadio_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBookingGrid();
        }

        private void lbxBHlvl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
