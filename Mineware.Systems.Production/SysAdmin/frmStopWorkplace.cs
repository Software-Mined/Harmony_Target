using System;
using System.Drawing;
using System.Linq;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class frmStopWorkplace : DevExpress.XtraEditors.XtraForm
    {
        public string _connectionString;
        public int _activity;
        public string _WPID;
        public string _workplace;
        public DateTime _date;
        public frmStopWorkplace()
        {
            InitializeComponent();
        }

        private void frmStopWorkplace_Load(object sender, EventArgs e)
        {
            barEditDate.EditValue = _date;
            barEditWorkplace.EditValue = _workplace;
            if (_activity == 1)
            {
                DevPnl.Visible = true;
                StopingPnl.Visible = false;
            }
            else
            {
                DevPnl.Visible = false;
                StopingPnl.Visible = true;
            }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void btnSaveProb_Click(object sender, EventArgs e)
        {
            //Main
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = _connectionString;
            _dbMan1.SqlStatement = " begin try ";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " insert into tbl_Workplace_Stoppages values ( \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + _WPID + "', \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + cbxStoppageType.EditValue + "',\r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + memoComments.EditValue + "'\r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " ) \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " end try \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " begin catch \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " update tbl_Workplace_Stoppages \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " set StoppageType =  '" + cbxStoppageType.EditValue + "', \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " Comments =  '" + memoComments.EditValue + "' \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " where WorkplaceID = '" + _WPID + "' \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " end catch \r\n";


            //Detail      
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " begin try  insert into tbl_Workplace_Stoppages_Detail values ( \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + _WPID + "', \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + barEditDate.EditValue + "',\r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + cbxStoppageType.EditValue + "',\r\n";
            if (_activity == 1)
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + txtDevPegToFace.EditValue + "',\r\n";
            else
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + textEditAdv.EditValue + "',\r\n";

            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " '" + memoComments.EditValue + "'\r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " ) \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " end try \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " begin catch \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " update tbl_Workplace_Stoppages_Detail \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " set StoppageDate =  '" + barEditDate.EditValue + "', \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " StoppageType =  '" + cbxStoppageType.EditValue + "', \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " Comments =  '" + memoComments.EditValue + "', \r\n";
            if (_activity == 1)
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " Distance = '" + txtDevPegToFace.EditValue + "'\r\n";
            else
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " Distance = '" + textEditAdv.EditValue + "'\r\n";

            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " where WorkplaceID = '" + _WPID + "' and StoppageDate = '" + barEditDate.EditValue + "' and StoppageType =  '" + cbxStoppageType.EditValue + "'  \r\n";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " end catch \r\n";

            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            Global.sysNotification.TsysNotification.showNotification("Data Saved", "Workplace Stoppages", Color.CornflowerBlue);
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}