using DevExpress.XtraEditors;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


namespace Mineware.Systems.Production.OCRScheduling
{
    public partial class AddWorkplaceOCR : DevExpress.XtraEditors.XtraForm
    {
        StringBuilder sb = new StringBuilder();
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;

        public string _SectionMO;
        public string _Month;
        Procedures procs = new Procedures();

        public AddWorkplaceOCR()
        {
            InitializeComponent();
        }

        private void AddWorkplaceOCR_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManVampWP = new MWDataManager.clsDataAccess();
            _dbManVampWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManVampWP.SqlStatement = " select WorkplaceID, Description, 'N' Selected, '' SectionID from tbl_WORKPLACE " +
                                        " --union Select WorkplaceID, Description, 'N' Selected, '' SectionID from [WORKPLACE_DOORNKOP_SURFACE]" +
                                        " " +
                                        "order by Description ";
            _dbManVampWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManVampWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManVampWP.ExecuteInstruction();


            DataTable dt1 = _dbManVampWP.ResultsDataTable;

            DataSet ds1 = new DataSet();

            if (ds1.Tables.Count > 0)
                ds1.Tables.Clear();

            ds1.Tables.Add(dt1);
            gcWorkPlaces.DataSource = ds1.Tables[0];


            gcolWPID.FieldName = "WorkplaceID";
            gcolDESCRIPTION.FieldName = "Description";
            gcolChecked.FieldName = "Selected";
            //gcolSupervisor.FieldName = "SectionID";


            LoadMinerList(_Month, _SectionMO);


            //MWDataManager.clsDataAccess _dbManSec = new MWDataManager.clsDataAccess();
            //_dbManSec.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            //_dbManSec.SqlStatement = "   Select SectionID, SectionID+': ' + Name Name from Section  \r\n" +
            //                          " where Hierarchicalid = '5'  \r\n" +
            //                            " and prodmonth = '"+_Month+"'  \r\n" +
            //                           " and SectionID like '"+ _SectionMO + "%'  \r\n" +
            //                         "order by SectionID ";
            //_dbManSec.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbManSec.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbManSec.ExecuteInstruction();


            //DataTable dt = _dbManSec.ResultsDataTable;

            //DataSet ds = new DataSet();

            //if (ds.Tables.Count > 0)
            //    ds.Tables.Clear();

            //ds.Tables.Add(dt);
            //SecLookUp.DataSource = ds.Tables[0];
            //SecLookUp.DisplayMember = "Name";
            //SecLookUp.ValueMember = "SectionID";




        }


        public void LoadMinerList(string prodMonth, string sectionidMO)
        {
            MWDataManager.clsDataAccess _MinerData = new MWDataManager.clsDataAccess();
            _MinerData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _MinerData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _MinerData.queryReturnType = MWDataManager.ReturnType.DataTable;
            sb.Clear();
            sb.AppendLine("Select SectionID,  Name from tbl_Section ");
            sb.AppendLine("where Hierarchicalid = '5' ");
            sb.AppendLine("and prodmonth = '" + _Month + "' ");
            sb.AppendLine("and SectionID like '" + _SectionMO + "%' ");
            sb.AppendLine("union ");
            sb.AppendLine("Select SectionID, Name from tbl_SectionOther ");
            sb.AppendLine("where Hierarchicalid in (3,5) ");
            sb.AppendLine("and prodmonth = '" + _Month + "' ");
            sb.AppendLine("and SectionID like '" + _SectionMO + "%' ");
            sb.AppendLine("order by SectionID");
            _MinerData.SqlStatement = sb.ToString();
            clsDataResult res = _MinerData.ExecuteInstruction();

            if (!res.success)
            {
                XtraMessageBox.Show(res.Message, "Error");
            }

            else
            {
                if (_MinerData.ResultsDataTable.Rows.Count > 0)
                {
                    SecLookUp.DataSource = _MinerData.ResultsDataTable;
                    SecLookUp.DisplayMember = "Name";
                    SecLookUp.ValueMember = "SectionID";
                }
            }


            SecLookUp.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            string SectionID = string.Empty;
            string WPID = string.Empty;

            for (int i = 0; i < viewWorkplaces.DataRowCount; i++)
            {
                SectionID = viewWorkplaces.GetRowCellValue(i, "SectionID").ToString();
                WPID = viewWorkplaces.GetRowCellValue(i, "WorkplaceID").ToString();

                if (SectionID != string.Empty)
                {
                    MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
                    _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbManWPSTDetail.SqlStatement = " Exec [sp_OCR_AddNewWorkplace]  '" + _Month + "','" + SectionID + "','" + WPID + "' \r\n" +
                                                    "  \r\n" +
                                                    "    ";

                    _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManWPSTDetail.ExecuteInstruction();

                    Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "saved", Color.CornflowerBlue);

                    this.Close();
                }
            }







        }
    }
}
