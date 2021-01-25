using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.LongHoleDrilling
{
    public partial class frmBookings : DevExpress.XtraEditors.XtraForm
    {
        public string _Connection;
        string ABS;
        string ProbID;
        string ProbNotes;
        public frmBookings()
        {
            InitializeComponent();
        }

        private void frmBookings_Load(object sender, EventArgs e)
        {
            editDate.EditValue = System.DateTime.Today;

            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = _Connection;
            _dbMan1.SqlStatement = " select b.Workplace, b.RingName, b.HoleNo, b.Length, b.MachineNo,DSBook from[tbl_LongHoleDrilling_DayPlanBook] b \r\n " +
                                   " inner join tbl_LongHoleDrilling_PlanLongTerm p on b.Workplace = p.Workplace and b.HoleNo = p.HoleNo and b.RingName = p.RingName \r\n" +
                                   " where b.Workplace = '" + editWorkplace.EditValue + "' and TheDate = Convert(varchar(10),'" + editDate.EditValue + "',120) \r\n" +
                                   " order by b.RingName, b.HoleNo\r\n" +
                                   " \r\n";   
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable HolesNA1 = _dbMan1.ResultsDataTable;

            gcBookings.DataSource = HolesNA1;
           
            colBookRing1.FieldName = "RingName";
            colBookHole1.FieldName = "HoleNo";
            colBookAdv1.FieldName = "DSBook";
        }

        private void btnProblem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmProblems frm = new frmProblems();
            frm._Connection = _Connection;
            frm._Workplace = editWorkplace.EditValue.ToString();
            frm.ShowDialog();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ///Save ABS
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = _Connection;
            _dbMan1.SqlStatement = " update [tbl_LongHoleDrilling_DayPlanBook]  \r\n " +
                                   " set [ABS] = '"+ABS+"', \r\n" +
                                   " [ABSNotes] = '" + txtABSNotes.Text + "' \r\n" +
                                   " where Workplace = '" + editWorkplace.EditValue + "' and TheDate = Convert(Varchar(10),'" + editDate.EditValue + "' ,120) \r\n" +
                                   " \r\n" +
                                   " \r\n";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();


            //Save Bookings
            MWDataManager.clsDataAccess _dbManSave2 = new MWDataManager.clsDataAccess();
            _dbManSave2.ConnectionString = _Connection;
            _dbManSave2.SqlStatement = " \r\n";
            for (int i = 0; i < gvBookings.RowCount; i++)
            {
              
                _dbManSave2.SqlStatement = _dbManSave2.SqlStatement + " Update tbl_LongHoleDrilling_DayPlanBook \r\n";
                _dbManSave2.SqlStatement = _dbManSave2.SqlStatement + " set DSBook =  '" + gvBookings.GetRowCellValue(i, gvBookings.Columns["DSBook"].FieldName).ToString() + "' \r\n";
                _dbManSave2.SqlStatement = _dbManSave2.SqlStatement + " where WORKPLACE = '" + editWorkplace.EditValue + "' and RINGNAME = '" + gvBookings.GetRowCellValue(i, gvBookings.Columns["RingName"].FieldName).ToString() + "'  and [HoleNo] = '" + gvBookings.GetRowCellValue(i, gvBookings.Columns["HoleNo"].FieldName).ToString() + "'  \r\n";
                _dbManSave2.SqlStatement = _dbManSave2.SqlStatement + " and TheDate = Convert(Varchar(10),'" + editDate.EditValue + "',120) ";


            }

            _dbManSave2.SqlStatement = _dbManSave2.SqlStatement + " \r\n";

            _dbManSave2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSave2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSave2.ExecuteInstruction();
        }

        private void btnSafe_Click(object sender, EventArgs e)
        {
            ABS = "A";
            this.btnSafe.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.btnSafe.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnSafe.Appearance.BackColor = Color.LightSteelBlue;
            this.btnSafe.Appearance.Options.UseBackColor = true;

            this.btnUnSafe.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.btnUnSafe.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnUnSafe.Appearance.BackColor = Color.Transparent;
            this.btnUnSafe.Appearance.Options.UseBackColor = true;

            this.btnNoVist.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.btnNoVist.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnNoVist.Appearance.BackColor = Color.Transparent;
            this.btnNoVist.Appearance.Options.UseBackColor = true;
            
        }

        private void btnUnSafe_Click(object sender, EventArgs e)
        {
            ABS = "B";
            this.btnUnSafe.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.btnUnSafe.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnUnSafe.Appearance.BackColor = Color.LightSteelBlue;
            this.btnUnSafe.Appearance.Options.UseBackColor = true;

            this.btnSafe.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.btnSafe.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnSafe.Appearance.BackColor = Color.Transparent;
            this.btnSafe.Appearance.Options.UseBackColor = true;

            this.btnNoVist.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.btnNoVist.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnNoVist.Appearance.BackColor = Color.Transparent;
            this.btnNoVist.Appearance.Options.UseBackColor = true;

        }

        private void btnNoVist_Click(object sender, EventArgs e)
        {
            ABS = "S";
            this.btnNoVist.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.btnNoVist.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnNoVist.Appearance.BackColor = Color.LightSteelBlue;
            this.btnNoVist.Appearance.Options.UseBackColor = true;

            this.btnUnSafe.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.btnUnSafe.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnUnSafe.Appearance.BackColor = Color.Transparent;
            this.btnUnSafe.Appearance.Options.UseBackColor = true;

            this.btnSafe.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.btnSafe.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnSafe.Appearance.BackColor = Color.Transparent;
            this.btnSafe.Appearance.Options.UseBackColor = true;
        }
    }
}