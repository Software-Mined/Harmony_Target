using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Mineware.Systems.ProductionAmplats.Departmental.Engineering
{
    public partial class EquipProbBooking : DevExpress.XtraEditors.XtraForm
    {

        BookingPropEq Book;

        public EquipProbBooking(BookingPropEq _Book)
        {
            InitializeComponent();

            Book = _Book;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EquipProbBooking_Load(object sender, EventArgs e)
        {

            //this.Icon = PAS.Properties.Resources.testbutton3;
            cmbStatus.Items.Add("Awaiting Spares");
            cmbStatus.Items.Add("Closed");
            cmbStatus.Items.Add("In Progress");
            cmbStatus.Items.Add("Reported");

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            string Shift = "";

            if (ShiftLbl.Text == "Day Shift")
            {
                Shift = "Morning";
            }

            if (ShiftLbl.Text == "Afternoon Shift")
            {
                Shift = "Afternoon";
            }

            if (ShiftLbl.Text == "Night Shift")
            {
                Shift = "Night";
            }

            string Damaged = "";

            if (cbxDamaged.Checked == true)
            {
                Damaged = "Y";
            }
            else
            {
                Damaged = "N";
            }


            MWDataManager.clsDataAccess _dbManDel = new MWDataManager.clsDataAccess();

            _dbManDel.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManDel.SqlStatement = "Delete from Booking_Problems";
            _dbManDel.SqlStatement = _dbManDel.SqlStatement + " where Bookdate = '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ShiftDatelbl.Text)) + "'";
            _dbManDel.SqlStatement = _dbManDel.SqlStatement + " and equipno = '"+EquipLbl.Text+"' and shift = '"+Shift+"' ";
            _dbManDel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDel.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDel.ExecuteInstruction();

            MWDataManager.clsDataAccess _dbManInsert = new MWDataManager.clsDataAccess();

            _dbManInsert.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManInsert.SqlStatement = " Insert into Booking_Problems values( ";
            _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ShiftDatelbl.Text)) + "', '" + EquipLbl.Text + "', '" + Shift + "', ";
            _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '"+StartTimetxt.Text+"', '"+EndTimetxt.Text+"', '"+Durtxt.Text+"', ";
            _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '"+cmbType.Text+"','"+Faulttxt.Text+"', '', ";
            _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '','','', ";
            _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + Damaged + "', '" + String.Format("{0:yyyy-MM-dd}", StartDate.Value) + "', '" + String.Format("{0:yyyy-MM-dd}", EndDate.Value) + "', '" + Maximotxt.Text + "','" + TMRemarkstxt.Text + "',  '" + cmbStatus.Text + "','" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ShiftDatelbl.Text)) + "') ";

            _dbManInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManInsert.ExecuteInstruction();

            if (Book.LHDPnl.Visible == true)
            {

                Book.button7_Click(null, null);
            }
            else
            {
                Book.button8_Click(null, null);
            }

        }
    }
}
