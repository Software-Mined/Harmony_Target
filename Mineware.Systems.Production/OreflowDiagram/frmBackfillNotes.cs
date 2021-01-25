using Mineware.Systems.GlobalConnect;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class frmBackfillNotes : DevExpress.XtraEditors.XtraForm
    {
        #region public variables
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        public string _FormType;
        public string _Description;
        public string _From;
        public string _To;
        public string _Date;
        public string _Range;
        public string _Workplace;
        #endregion

        public frmBackfillNotes()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmBackfillNotes_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_FormType == "BookSurface")
            {
                if (Notestxt.Text != string.Empty)
                {
                    MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
                    _dbMana.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbMana.SqlStatement = _dbMana.SqlStatement + " insert into tbl_Backfill_ProblemsSurface (Description,TimeFrom,TimeTo,Notes,CalendarDate)  values \r\n " +
                                          "('" + _Description + "', '" + _From + "' , '" + _To + "' ,'" + Notestxt.Text + "', '" + _Date + "')   ";
                    _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
                    var result = _dbMana.ExecuteInstruction();

                    if (result.success)
                    {
                        Global.sysNotification.TsysNotification.showNotification("Data Saved", "Problem Added", Color.CornflowerBlue);
                    }
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to Save the Record With an empty note  ?", "Empty Notes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                        _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                        _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_Backfill_ProblemsSurface (Description,TimeFrom,TimeTo,Notes,CalendarDate)  values \r\n " +
                                          "('" + _Description + "', '" + _From + "' , '" + _To + "' ,'" + Notestxt.Text + "', '" + _Date + "')   ";
                        _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _dbMan.ExecuteInstruction();

                        Global.sysNotification.TsysNotification.showNotification("Data Saved", "Problem Added", Color.CornflowerBlue);
                        this.Close();
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (_FormType == "Transfer")
            {
                if (Notestxt.Text != string.Empty)
                {
                    MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
                    _dbMana.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbMana.SqlStatement = _dbMana.SqlStatement + " insert into tbl_Backfill_Problems (Description,TimeFrom,TimeTo,Notes,CalendarDate)  values \r\n " +
                                          "('" + _Description + "', '" + _From + "' , '" + _To + "' ,'" + Notestxt.Text + "' , '" + _Date + "')   ";
                    _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMana.ExecuteInstruction();

                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Problem Added", Color.CornflowerBlue);
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to Save the Record With an empty note  ?", "Empty Notes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                        _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                        _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_Backfill_Problems (Description,TimeFrom,TimeTo,Notes,CalendarDate)  values \r\n " +
                                          "('" + _Description + "', '" + _From + "' , '" + _To + "' ,'" + Notestxt.Text + "' , '" + _Date + "')   ";
                        _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _dbMan.ExecuteInstruction();

                        Global.sysNotification.TsysNotification.showNotification("Data Saved", "Problem Added", Color.CornflowerBlue);
                        this.Close();
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (_FormType == "AddBooking")
            {
                if (Notestxt.Text != string.Empty)
                {
                    MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
                    _dbMana.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                    _dbMana.SqlStatement = _dbMana.SqlStatement + " insert into tbl_Backfill_Booking_Problems (Description,TimeFrom,TimeTo,Notes,Range,Workplace,CalendarDate)  values \r\n " +
                                          "('" + _Description + "', '" + _From + "' , '" + _To + "' ,'" + Notestxt.Text + "','" + _Range + "' , '" + _Workplace + "' , '" + _Date + "')   ";
                    _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbMana.ExecuteInstruction();

                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Problem Added", Color.CornflowerBlue);
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to Save the Record With an empty note  ?", "Empty Notes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                        _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                        _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_Backfill_Booking_Problems (Description,TimeFrom,TimeTo,Notes,Range,Workplace,CalendarDate)  values \r\n " +
                                          "('" + _Description + "', '" + _From + "' , '" + _To + "' ,'" + Notestxt.Text + "','" + _Range + "' , '" + _Workplace + "' , '" + _Date + "')   ";
                        _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                        _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                        _dbMan.ExecuteInstruction();

                        Global.sysNotification.TsysNotification.showNotification("Data Saved", "Problem Added", Color.CornflowerBlue);
                        this.Close();
                    }
                    else
                    {
                        return;
                    }
                }
            }

            this.Close();
        }
    }
}