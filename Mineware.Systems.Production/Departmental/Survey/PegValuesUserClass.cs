using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Survey
{
    public partial class PegValuesUserClass : BaseUserControl
    {
        #region Data Feilds
        StringBuilder sbSqlQuery = new StringBuilder();
        #endregion

        #region Constructors
        public PegValuesUserClass()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpPegValue);
            FormActiveRibbonPage = rpPegValue;
            FormMainRibbonPage = rpPegValue;
            RibbonControl = rcPegValue;
        }
        #endregion

        #region Methods/Functions

        private void PegValuesUserClass_Load(object sender, EventArgs e)
        {
            LoadPegValues();
        }

        internal void LoadPegValues()
        {
            sbSqlQuery.Clear();
            sbSqlQuery.AppendLine(" select w.workplaceid, w.description,b.pegid,b.value, b.letter1, b.letter2, b.letter3 ");
            sbSqlQuery.AppendLine(" from tbl_Workplace w left outer join ");
            sbSqlQuery.AppendLine(" (select a.* from tbl_Peg a inner join ");
            sbSqlQuery.AppendLine(" (select workplaceid, max(value) value from tbl_Peg group by workplaceid) b on ");
            sbSqlQuery.AppendLine(" a.workplaceid = b.workplaceid and a.value = b.value ) b on ");
            sbSqlQuery.AppendLine(" b.workplaceid = w.workplaceid where w.Activity = '1' ");

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = sbSqlQuery.ToString();

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtPegData = _dbMan.ResultsDataTable;
            gcPegValues.DataSource = dtPegData;

            colWorkplaceID.FieldName = "workplaceid";
            colDescripion.FieldName = "description";
            colPegID.FieldName = "pegid";
            colValue.FieldName = "value";
            colLetter1.FieldName = "letter1";
            colLetter2.FieldName = "letter2";
            colLetter3.FieldName = "letter3";
        }

        #endregion

        #region Events

        private void gvPegValues_DoubleClick(object sender, EventArgs e)
        {
            string myWP = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["description"]).ToString();
            string WorkplaceID = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["workplaceid"]).ToString();
            string PegID = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["pegid"]).ToString();
            string PegValue = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["value"]).ToString();
            string Letter1 = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["letter1"]).ToString();
            string Letter2 = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["letter2"]).ToString();
            string Letter3 = gvPegValues.GetRowCellValue(gvPegValues.FocusedRowHandle, gvPegValues.Columns["letter3"]).ToString();

            if (gvPegValues.FocusedColumn.Caption == "Workplace")
            {
                PegValuesCaptureForm pegForm = new PegValuesCaptureForm();
                pegForm.lblWpID.Text = WorkplaceID;
                pegForm.lblWpDescription.Text = myWP;
                pegForm._ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                pegForm.StartPosition = FormStartPosition.CenterParent;
                pegForm.ShowDialog();

                LoadPegValues();
            }
        }

        #endregion

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
