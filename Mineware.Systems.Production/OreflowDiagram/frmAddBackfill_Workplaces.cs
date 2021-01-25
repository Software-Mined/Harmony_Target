using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class frmAddBackfill_Workplaces : DevExpress.XtraEditors.XtraForm
    {
        #region Public variables
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        public string _FormType;
        public string _Range;
        #endregion

        #region Private variables
        DataTable WPlaceNew = new DataTable();
        #endregion

        public frmAddBackfill_Workplaces()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddBackfill_Workplaces_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManDtsub1 = new MWDataManager.clsDataAccess();
            _dbManDtsub1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManDtsub1.SqlStatement = "select * from tbl_workplace where activity <> 1 ";
            _dbManDtsub1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDtsub1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDtsub1.ExecuteInstruction();

            WPlaceNew = _dbManDtsub1.ResultsDataTable;

            foreach (DataRow dr in WPlaceNew.Rows)
            {
                lbxWorkplaces.Items.Add(dr["Description"].ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lbxWorkplaces.Text == string.Empty)
            {
                MessageBox.Show("Please Select a workplace");
                return;
            }

            if (_FormType == "Permanent")
            {
                MWDataManager.clsDataAccess _dbManDtsub1 = new MWDataManager.clsDataAccess();
                _dbManDtsub1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManDtsub1.SqlStatement = "Delete from tbl_Backfill_Booking_RangeWorkplaces where Range = '" + _Range + "' and Workplace = '" + lbxWorkplaces.SelectedItem.ToString() + "'  \r\n ";
                _dbManDtsub1.SqlStatement = _dbManDtsub1.SqlStatement + "Insert into tbl_Backfill_Booking_RangeWorkplaces  values ('" + lbxWorkplaces.SelectedItem.ToString() + "'  , '" + _Range + "' , 'P' )  ";
                _dbManDtsub1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDtsub1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDtsub1.ExecuteInstruction();
            }

            if (_FormType == "Temporary")
            {
                MWDataManager.clsDataAccess _dbManDtsub1 = new MWDataManager.clsDataAccess();
                _dbManDtsub1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManDtsub1.SqlStatement = "Delete from tbl_Backfill_Booking_RangeWorkplaces where Range = '" + _Range + "' and Workplace = '" + lbxWorkplaces.SelectedItem.ToString() + "'  \r\n ";
                _dbManDtsub1.SqlStatement = _dbManDtsub1.SqlStatement + "Insert into tbl_Backfill_Booking_RangeWorkplaces  values ('" + lbxWorkplaces.SelectedItem.ToString() + "' , '" + _Range + "' , 'T' ) ";
                _dbManDtsub1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDtsub1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDtsub1.ExecuteInstruction();
            }

            this.Close();
        }

        private void Filtertxt_TextChanged(object sender, EventArgs e)
        {
            LoadBHWP();
        }

        public void LoadBHWP()
        {
            string zzzz = "*" + Filtertxt.Text;

            lbxWorkplaces.Items.Clear();

            foreach (DataRowView r in Search(WPlaceNew, zzzz))
            {
                lbxWorkplaces.Items.Add(r["Description"].ToString());
            }
        }

        public DataView Search(DataTable SearchTable, string SearchString)
        {
            DataView dv = new DataView(SearchTable);
            string SearchExpression = null;

            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchExpression = string.Format("'{0}%'", SearchString);
                dv.RowFilter = "Description like " + SearchExpression;
            }

            return dv;
        }

    }
}