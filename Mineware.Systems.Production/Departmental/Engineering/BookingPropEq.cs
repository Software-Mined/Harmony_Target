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
    public partial class BookingPropEq : DevExpress.XtraEditors.XtraForm
    {
        EquipBookingFrm Book;
        public BookingPropEq(EquipBookingFrm _Book)
        {
            InitializeComponent();
            Book = _Book;
        }

        BindingSource bs1 = new BindingSource();

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            BookingLostTimeFrm BookingLostTimeFrm = (BookingLostTimeFrm)IsBookingFormAlreadyOpen(typeof(BookingLostTimeFrm));
            if (BookingLostTimeFrm == null)
            {
                BookingLostTimeFrm = new BookingLostTimeFrm(this);
                BookingLostTimeFrm.VisiblePanel = "Drill";
                BookingLostTimeFrm.button1_Click(null, null);
                BookingLostTimeFrm.dateLbl.Text = LblDate.Text;
                BookingLostTimeFrm.EquiNo.Text = DrEquipLbl.Text;
                BookingLostTimeFrm.Shiftlbl.Text = ShiftLbl.Text; 
                //BookingLostTimeFrm.Text = "Planning Report";
                BookingLostTimeFrm.Show();
            }
            else
            {
                BookingLostTimeFrm.WindowState = FormWindowState.Maximized;
                BookingLostTimeFrm.Select();
            }
        }

        public static Form IsBookingFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            BookingLostTimeFrm BookingLostTimeFrm = (BookingLostTimeFrm)IsBookingFormAlreadyOpen(typeof(BookingLostTimeFrm));
            if (BookingLostTimeFrm == null)
            {
                BookingLostTimeFrm = new BookingLostTimeFrm(this);
                //BookingLostTimeFrm.Text = "Planning Report";

                //BookingLostTimeFrm.VisiblePanel = "Drill";
                BookingLostTimeFrm.button1_Click(null, null);

                BookingLostTimeFrm.dateLbl.Text = dateLbl.Text;
                BookingLostTimeFrm.EquiNo.Text = EquipLbl.Text;
                BookingLostTimeFrm.Shiftlbl.Text = shiftTypeLbl.Text; 

                BookingLostTimeFrm.Show();
            }
            else
            {
                BookingLostTimeFrm.WindowState = FormWindowState.Maximized;
                BookingLostTimeFrm.Select();
            }
        }

        private void BookingPropEq_Load(object sender, EventArgs e)
        {
            //this.Icon = PAS.Properties.Resources.testbutton3;
           

        }

        public void button3_Click(object sender, EventArgs e)
        {
            WorkplaceGrid.Visible = false;
            //WorkplaceGrid.ColumnCount = 1;
            //WorkplaceGrid.RowCount = 1000;
            //WorkplaceGrid.ColumnCount = 1;


            //WorkplaceGrid.Columns[0].Width = 250;
            //WorkplaceGrid.Columns[0].HeaderText = "Workplace";
            //WorkplaceGrid.Columns[0].ReadOnly = true;
            //WorkplaceGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            //WorkplaceGrid.Columns[0].Frozen = true;

            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManWP.SqlStatement = "select Description Description,workplaceid from Workplace where workplaceid not in (select muckbay from Booking_Buckets where    ";
            _dbManWP.SqlStatement = _dbManWP.SqlStatement + " equipno = '" + EquipLbl.Text + "'and bookdate = '" + String.Format("{0:yyyy-MM-dd}", dateLbl.Text) + "'and shift = '"+shiftTypeLbl.Text+"' )"; 
            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            bs1.DataSource = _dbManWP.ResultsDataTable;

           

            WorkplaceGrid.DataSource = bs1;

            WorkplaceGrid.Columns[0].Width = 250;
            WorkplaceGrid.Columns[0].HeaderText = "Workplace";
            WorkplaceGrid.Columns[0].ReadOnly = true;
            WorkplaceGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            WorkplaceGrid.Columns[0].Frozen = true;

            WorkplaceGrid.Columns[1].Width = 250;
            WorkplaceGrid.Columns[1].HeaderText = "Workplaceid";
            WorkplaceGrid.Columns[1].Visible = false;
            WorkplaceGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            WorkplaceGrid.Columns[1].Frozen = true;

            int x = 0;

            //foreach (DataRow dr in _dbManWP.ResultsDataTable.Rows)
            //{
            //    WorkplaceGrid.Rows[x].Cells[0].Value = dr["Workplaceid"].ToString() + " : " + dr["Description"].ToString();
            //    x = x + 1;
            //}

            //WorkplaceGrid.RowCount = x;


            WorkplaceGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            
            WorkplaceGrid.BorderStyle = BorderStyle.FixedSingle;
            WorkplaceGrid.Visible = true;

            BucketGrid.Visible = false;
        
            BucketGrid.RowCount = 1000;
            BucketGrid.ColumnCount = 2;


            BucketGrid.Columns[0].Width = 200;
            BucketGrid.Columns[0].HeaderText = "Workplace";
            BucketGrid.Columns[0].ReadOnly = true;
            BucketGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            BucketGrid.Columns[0].Frozen = false;

            BucketGrid.Columns[1].Width = 60;
            BucketGrid.Columns[1].HeaderText = "Buckets";
            BucketGrid.Columns[1].ReadOnly = true;
            BucketGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            BucketGrid.Columns[1].Frozen = false;

            MWDataManager.clsDataAccess _dbManBuckets = new MWDataManager.clsDataAccess();
            _dbManBuckets.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManBuckets.SqlStatement = " select w.description ,Muckbay, buckets from Booking_Buckets s  ";
            _dbManBuckets.SqlStatement =   _dbManBuckets.SqlStatement + " left outer join Workplace w on s.muckbay = w.workplaceid  ";
            _dbManBuckets.SqlStatement = _dbManBuckets.SqlStatement + " where equipno = '" + EquipLbl.Text + "' and shift = '" + shiftTypeLbl.Text + "' and bookdate = '" + String.Format("{0:yyyy-MM-dd}", dateLbl.Text) + "' ";
            _dbManBuckets.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManBuckets.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManBuckets.ExecuteInstruction();

            int xx = 0;
            int Totalbuckets = 0;

            if (_dbManBuckets.ResultsDataTable.Rows.Count > 0)
            {

                foreach (DataRow dr in _dbManBuckets.ResultsDataTable.Rows)
                {
                    BucketGrid.Rows[xx].Cells[0].Value = dr["muckbay"].ToString() + " : " + dr["description"].ToString();
                    BucketGrid.Rows[xx].Cells[1].Value = dr["buckets"].ToString();
                    Totalbuckets = Totalbuckets + Convert.ToInt16(dr["buckets"].ToString());
                    xx = xx + 1;
                }

                BucketGrid.RowCount = xx;
                TotBucketsTxt.Text = Totalbuckets.ToString();
            }
            else
            {
                BucketGrid.RowCount = 1;
            }
            

            BucketGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            //LHDGrid.BackgroundColor = Color.White;
            BucketGrid.BorderStyle = BorderStyle.FixedSingle;
            BucketGrid.Visible = true;

           

        }

        public static Form IsPropFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BookingBucketsProp BookingBucketsProp = (BookingBucketsProp)IsPropFormAlreadyOpen(typeof(BookingBucketsProp));
            if (BookingBucketsProp == null)
            {
                BookingBucketsProp = new BookingBucketsProp(this);
                BookingBucketsProp.Workplacelbl.Text = WorkplaceGrid.CurrentRow.Cells[0].Value.ToString();
                BookingBucketsProp.WPID.Text = WorkplaceGrid.CurrentRow.Cells[1].Value.ToString();
                BookingBucketsProp.EquipLbl.Text = EquipLbl.Text;
                BookingBucketsProp.shiftTypeLbl.Text = shiftTypeLbl.Text;
                BookingBucketsProp.dateLbl.Text = dateLbl.Text;
                BookingBucketsProp.FactLbl.Text = FactLbl.Text;

                BookingBucketsProp.Show();


            }
            else
            {
                BookingBucketsProp.WindowState = FormWindowState.Normal;
                BookingBucketsProp.Select();
            }

            
        }

        private void EHEostxt_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void EHEostxt_Leave(object sender, EventArgs e)
        {
            EHVartxt.Text = Convert.ToString(Convert.ToDouble(EHEostxt.Text) - Convert.ToDouble(EHSostxt.Text));

            if (Convert.ToDecimal(EHVartxt.Text) > 8)
            {
                MessageBox.Show("Engine Hours can not be more than 8 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToDecimal(EHVartxt.Text) < 0)
            {
                MessageBox.Show("Engine Hours can not be more less than 0 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void TransEostxt_Leave(object sender, EventArgs e)
        {
            TransVartxt.Text = Convert.ToString(Convert.ToDouble(TransEostxt.Text) - Convert.ToDouble(TransSostxt.Text));

            if (Convert.ToDecimal(TransVartxt.Text) > 8)
            {
                MessageBox.Show("Engine Hours can not be more than 8 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToDecimal(TransVartxt.Text) < 0)
            {
                MessageBox.Show("Engine Hours can not be more less than 0 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void button4_Click(object sender, EventArgs e)
        {
            //////////////////////////////////// Load Delays/////////////////////////////////////////

            OPPDelaysGrid.Visible = false;

            OPPDelaysGrid.RowCount = 1000;
            OPPDelaysGrid.ColumnCount = 5;


            OPPDelaysGrid.Columns[0].Width = 50;
            OPPDelaysGrid.Columns[0].HeaderText = "Start";
            OPPDelaysGrid.Columns[0].ReadOnly = true;
            OPPDelaysGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            OPPDelaysGrid.Columns[0].Frozen = false;

            OPPDelaysGrid.Columns[1].Width = 50;
            OPPDelaysGrid.Columns[1].HeaderText = "End";
            OPPDelaysGrid.Columns[1].ReadOnly = true;
            OPPDelaysGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            OPPDelaysGrid.Columns[1].Frozen = false;

            OPPDelaysGrid.Columns[2].Width = 50;
            OPPDelaysGrid.Columns[2].HeaderText = "Duration";
            OPPDelaysGrid.Columns[2].ReadOnly = true;
            OPPDelaysGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            OPPDelaysGrid.Columns[2].Frozen = false;

            OPPDelaysGrid.Columns[3].Width = 240;
            OPPDelaysGrid.Columns[3].HeaderText = "Problems";
            OPPDelaysGrid.Columns[3].ReadOnly = true;
            OPPDelaysGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            OPPDelaysGrid.Columns[3].Frozen = false;

            OPPDelaysGrid.Columns[4].Width = 160;
            OPPDelaysGrid.Columns[4].HeaderText = "Remarks";
            OPPDelaysGrid.Columns[4].ReadOnly = true;
            OPPDelaysGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            OPPDelaysGrid.Columns[4].Frozen = false;

            

         


            MWDataManager.clsDataAccess _dbManLoad = new MWDataManager.clsDataAccess();
            _dbManLoad.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManLoad.SqlStatement = " select * from booking_Delays where bookdate = '" + String.Format("{0:yyyy-MM-dd}", dateLbl.Text) + "' ";
            _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " and equipno = '" + EquipLbl.Text + "' and shift = '" + shiftTypeLbl.Text + "' ";
            _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " order by starttime ";
            _dbManLoad.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLoad.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManLoad.ExecuteInstruction();

            int y = 0;

            foreach (DataRow dr in _dbManLoad.ResultsDataTable.Rows)
            {
                OPPDelaysGrid.Rows[y].Cells[0].Value = dr["StartTime"].ToString();
                OPPDelaysGrid.Rows[y].Cells[1].Value = dr["EndTime"].ToString();
                OPPDelaysGrid.Rows[y].Cells[2].Value = dr["Duration"].ToString();
                OPPDelaysGrid.Rows[y].Cells[3].Value = dr["Problem"].ToString();
                OPPDelaysGrid.Rows[y].Cells[4].Value = dr["Remarks"].ToString();

                y = y + 1;
            }

            OPPDelaysGrid.RowCount = y;


            OPPDelaysGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            //LHDGrid.BackgroundColor = Color.White;
            OPPDelaysGrid.BorderStyle = BorderStyle.FixedSingle;
            OPPDelaysGrid.Visible = true;
        }

        DialogResult result;

        private void DeleteBtn_Click(object sender, EventArgs e)
        {

           result =  MessageBox.Show("Are you sure you want to delete this delay?", "Delete Delay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                MWDataManager.clsDataAccess _dbManDelete = new MWDataManager.clsDataAccess();
                _dbManDelete.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

                _dbManDelete.SqlStatement = " delete from Booking_Delays where bookdate = '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dateLbl.Text).ToShortDateString()) + "' \r\n";
                _dbManDelete.SqlStatement = _dbManDelete.SqlStatement + " and equipno = '" + EquipLbl.Text + "' and starttime = '" + OPPDelaysGrid.CurrentRow.Cells[0].Value + "' and problem = '" + OPPDelaysGrid.CurrentRow.Cells[3].Value + "' ";
                _dbManDelete.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDelete.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDelete.ExecuteInstruction();

                //MessageFrm MsgFrm = new MessageFrm();
                //MsgFrm.Text = "Record Deleted";
                //Procedures.MsgText = "Record Deleted";
                //MsgFrm.Show();
            }

            button4_Click(null, null);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (LHDPnl.Visible == true)
            {

                if (Convert.ToDecimal(EHVartxt.Text) > 20)
                {
                    MessageBox.Show("Engine Hours are to Hight", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(EHVartxt.Text) < 0)
                {
                    MessageBox.Show("Engine Hours are less than zero", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(TransVartxt.Text) > 20)
                {
                    MessageBox.Show("Transmission Hours are to High", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(TransVartxt.Text) < 0)
                {
                    MessageBox.Show("Transmission Hours are less than zero", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

               // EHVartxt.Text = Convert.ToString(Convert.ToDecimal(EHEostxt.Text) - Convert.ToDecimal(EHSostxt.Text));
                //TransVartxt.Text = Convert.ToString(Convert.ToDecimal(TransEostxt.Text) - Convert.ToDecimal(TransSostxt.Text));

                string Shift = "";
                if (shiftTypeLbl.Text == "Day Shift")
                {
                    Shift = "Morning";
                }

                if (shiftTypeLbl.Text == "Afternoon Shift")
                {
                    Shift = "Afternoon";
                }

                if (shiftTypeLbl.Text == "Night Shift")
                {
                    Shift = "Night";
                }

               // MessageBox.Show(ShiftLbl.Text);

                EHVartxt.Text = Convert.ToString(Convert.ToDecimal(EHEostxt.Text) - Convert.ToDecimal(EHSostxt.Text));
                TransVartxt.Text = Convert.ToString(Convert.ToDecimal(TransEostxt.Text) - Convert.ToDecimal(TransSostxt.Text));

                MWDataManager.clsDataAccess _dbManDelete = new MWDataManager.clsDataAccess();
                _dbManDelete.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

                _dbManDelete.SqlStatement = " delete from Booking_MachineBooking where bookdate = '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dateLbl.Text)) + "' \r\n";
                _dbManDelete.SqlStatement = _dbManDelete.SqlStatement + " and equipno = '" + EquipLbl.Text + "' and shift = '" + Shift + "' ";
                _dbManDelete.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDelete.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDelete.ExecuteInstruction();


                MWDataManager.clsDataAccess _dbManInsert = new MWDataManager.clsDataAccess();
                _dbManInsert.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

                _dbManInsert.SqlStatement = " insert into Booking_MachineBooking values('" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dateLbl.Text)) + "','" + EquipLbl.Text + "', '" + Shift + "', '" + OperatorCmb.Text + "', \r\n";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + MOCmb.Text + "','" + ShiftBossCmb.Text + "', '" + EHSostxt.Text + "', '" + EHEostxt.Text + "', '" + EHVartxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + TransSostxt.Text + "', '" + TransEostxt.Text + "', '" + TransVartxt.Text + "', '" + TotBucketsTxt.Text + "', '" + FuelUsdTxt.Text + "') ";
                _dbManInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManInsert.ExecuteInstruction();

                Book.LoadLHD();
                Book.LoadDumpTruck();

                

                //MessageFrm MsgFrm = new MessageFrm();
                //MsgFrm.Text = "Record Saved";
                //Procedures.MsgText = "Record Saved";
                //MsgFrm.Show();
            }

            if (DrillRigPnl.Visible == true)
            {
                if (Convert.ToDecimal(EngHrsVarTxt.Text) > 20)
                {
                    MessageBox.Show("Engine Hours are to Hight", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(EngHrsVarTxt.Text) < 0)
                {
                    MessageBox.Show("Engine Hours are less than zero", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(PPLVarTxt.Text) > 20)
                {
                    MessageBox.Show("Engine Hours are to Hight", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(PPLVarTxt.Text) < 0)
                {
                    MessageBox.Show("Engine Hours are less than zero", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(PPRVarTxt.Text) > 20)
                {
                    MessageBox.Show("Engine Hours are to Hight", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(PPRVarTxt.Text) < 0)
                {
                    MessageBox.Show("Engine Hours are less than zero", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                if (Convert.ToDecimal(PCLVarTxt.Text) > 20)
                {
                    MessageBox.Show("Engine Hours are to Hight", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(PCLVarTxt.Text) < 0)
                {
                    MessageBox.Show("Engine Hours are less than zero", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(PCRVarTxt.Text) > 20)
                {
                    MessageBox.Show("Engine Hours are to Hight", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(PCRVarTxt.Text) < 0)
                {
                    MessageBox.Show("Engine Hours are less than zero", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(ComVarTxt.Text) > 20)
                {
                    MessageBox.Show("Engine Hours are to Hight", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Convert.ToDecimal(ComVarTxt.Text) < 0)
                {
                    MessageBox.Show("Engine Hours are less than zero", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                

                

                MWDataManager.clsDataAccess _dbManDelete = new MWDataManager.clsDataAccess();
                _dbManDelete.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

                _dbManDelete.SqlStatement = " delete from Booking_MachineBookingDr where bookdate = '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(LblDate.Text)) + "' \r\n";
                _dbManDelete.SqlStatement = _dbManDelete.SqlStatement + " and equipno = '" + DrEquipLbl.Text + "' and shift = '" + ShiftLbl.Text + "' ";
                _dbManDelete.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDelete.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDelete.ExecuteInstruction();


                MWDataManager.clsDataAccess _dbManInsert = new MWDataManager.clsDataAccess();
                _dbManInsert.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

                _dbManInsert.SqlStatement = " Insert into Booking_MachineBookingDR values (  ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(LblDate.Text)) + "', '" + DrEquipLbl.Text + "', '" + ShiftLbl.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + OpTxt.Text + "', '" + MOTxt.Text + "', '" + SBTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + LBPercTxt.Text + "', '" + LBRotTxt.Text + "', '" + LBFeedTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + RBPercTxt.Text + "', '" + RBRotTxt.Text + "', '" + RBFeedTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + ReamLeftPercTxt.Text + "', '" + ReamLeftRotTxt.Text + "', '" + ReamLeftFeedTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + ReamRightPercTxt.Text + "', '" + ReamRightRotTxt.Text + "', '" + ReamRightFeedTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + EngHrsSosTxt.Text + "', '" + EngHrsEosTxt.Text + "', '" + EngHrsVarTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + PPLSosTxt.Text + "', '" + PPLEosTxt.Text + "', '" + PPLVarTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + PPRSosTxt.Text + "', '" + PPREosTxt.Text + "', '" + PPRVarTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + PCLSosTxt.Text + "', '" + PCLEosTxt.Text + "', '" + PCLVarTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + PCRSosTxt.Text + "', '" + PCREosTxt.Text + "', '" + PCRVarTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + ComSosTxt.Text + "', '" + ComEosTxt.Text + "', '" + ComVarTxt.Text + "', ";
                _dbManInsert.SqlStatement = _dbManInsert.SqlStatement + " '" + PosEOSTxt.Text + "', '" + FuelUsedTxt.Text + "') ";

                _dbManInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManInsert.ExecuteInstruction();

                Book.LoadDrillRig();

                //MessageFrm MsgFrm = new MessageFrm();
                //MsgFrm.Text = "Record Saved";
                //Procedures.MsgText = "Record Saved";
                //MsgFrm.Show();
            }


            this.Close();

        }

        private void WPFilterTxt_TextChanged(object sender, EventArgs e)
        {
            if (WPFilterTxt.Text == "")
                bs1.Filter = bs1.Filter;// + string.Format("and [Employee Number] LIKE '{0}%'", '%');
            else
                bs1.Filter = string.Format("[Description] LIKE '%{0}%'", WPFilterTxt.Text);
        }

        DialogResult resulta;

        private void button2_Click(object sender, EventArgs e)
        {

            resulta = MessageBox.Show("Are you sure you want to delete this Workplace", "Delete Workplace", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (resulta == System.Windows.Forms.DialogResult.Yes)
            {

                MWDataManager.clsDataAccess _dbManDelete = new MWDataManager.clsDataAccess();
                _dbManDelete.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _dbManDelete.SqlStatement = "delete from Booking_Buckets  where Bookdate = '" + String.Format("{0:yyyy-MM-dd}", dateLbl.Text) + "' and equipno = '" + EquipLbl.Text + "' and shift = '" + shiftTypeLbl.Text + "'  ";
                _dbManDelete.SqlStatement = _dbManDelete.SqlStatement + " and muckbay = '" + BucketGrid.CurrentRow.Cells[0].Value + "' ";
                _dbManDelete.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDelete.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDelete.ExecuteInstruction();
            }

            //MessageFrm MsgFrm = new MessageFrm();
            //MsgFrm.Text = "Record Deleted";
            //Procedures.MsgText = "Record Deleted";
            //MsgFrm.Show();
        }

        public void button5_Click(object sender, EventArgs e)
        {
            //////////////////////////////////// Load Delays/////////////////////////////////////////

            DROppDelaysGrid.Visible = false;

            DROppDelaysGrid.RowCount = 1000;
            DROppDelaysGrid.ColumnCount = 5;


            DROppDelaysGrid.Columns[0].Width = 50;
            DROppDelaysGrid.Columns[0].HeaderText = "Start";
            DROppDelaysGrid.Columns[0].ReadOnly = true;
            DROppDelaysGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            DROppDelaysGrid.Columns[0].Frozen = false;

            DROppDelaysGrid.Columns[1].Width = 50;
            DROppDelaysGrid.Columns[1].HeaderText = "End";
            DROppDelaysGrid.Columns[1].ReadOnly = true;
            DROppDelaysGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            DROppDelaysGrid.Columns[1].Frozen = false;

            DROppDelaysGrid.Columns[2].Width = 50;
            DROppDelaysGrid.Columns[2].HeaderText = "Duration";
            DROppDelaysGrid.Columns[2].ReadOnly = true;
            DROppDelaysGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            DROppDelaysGrid.Columns[2].Frozen = false;

            DROppDelaysGrid.Columns[3].Width = 240;
            DROppDelaysGrid.Columns[3].HeaderText = "Problems";
            DROppDelaysGrid.Columns[3].ReadOnly = true;
            DROppDelaysGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            DROppDelaysGrid.Columns[3].Frozen = false;

            DROppDelaysGrid.Columns[4].Width = 160;
            DROppDelaysGrid.Columns[4].HeaderText = "Remarks";
            DROppDelaysGrid.Columns[4].ReadOnly = true;
            DROppDelaysGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            DROppDelaysGrid.Columns[4].Frozen = false;






            MWDataManager.clsDataAccess _dbManLoad = new MWDataManager.clsDataAccess();
            _dbManLoad.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManLoad.SqlStatement = " select * from booking_Delays where bookdate = '" + String.Format("{0:yyyy-MM-dd}", LblDate.Text) + "' ";
            _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " and equipno = '" + DrEquipLbl.Text + "' and shift = '" + ShiftLbl.Text + "' ";
            _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " order by starttime ";
            _dbManLoad.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLoad.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManLoad.ExecuteInstruction();

            int y = 0;

            foreach (DataRow dr in _dbManLoad.ResultsDataTable.Rows)
            {
                DROppDelaysGrid.Rows[y].Cells[0].Value = dr["StartTime"].ToString();
                DROppDelaysGrid.Rows[y].Cells[1].Value = dr["EndTime"].ToString();
                DROppDelaysGrid.Rows[y].Cells[2].Value = dr["Duration"].ToString();
                DROppDelaysGrid.Rows[y].Cells[3].Value = dr["Problem"].ToString();
                DROppDelaysGrid.Rows[y].Cells[4].Value = dr["Remarks"].ToString();

                y = y + 1;
            }

            if (y == 0)
            {
                DROppDelaysGrid.RowCount = 1;
            }
            else
            {
                DROppDelaysGrid.RowCount = y;
            }

           // DROppDelaysGrid.RowCount = y;


            DROppDelaysGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            
            DROppDelaysGrid.BorderStyle = BorderStyle.FixedSingle;
            DROppDelaysGrid.Visible = true;
        }

        public void button6_Click(object sender, EventArgs e)
        {
            WPGrid.Visible = false;
            

            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManWP.SqlStatement = "select wp,WPID, holes, tottime, water, air from Booking_WorkplaceDR where bookdate = '" + String.Format("{0:yyyy-MM-dd}", LblDate.Text) + "'   ";
            _dbManWP.SqlStatement = _dbManWP.SqlStatement + " and equipno = '" + DrEquipLbl.Text + "' and shift = '" + ShiftLbl.Text + "' order by wp ";
            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            bs1.DataSource = _dbManWP.ResultsDataTable;



            WPGrid.DataSource = bs1;

            WPGrid.Columns[0].Width = 150;
            WPGrid.Columns[0].HeaderText = "Workplace";
            WPGrid.Columns[0].ReadOnly = true;
            WPGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            WPGrid.Columns[0].Frozen = true;

            WPGrid.Columns[1].Width = 90;
            WPGrid.Columns[1].HeaderText = "Workplaceid";
            WPGrid.Columns[1].Visible = false;
            WPGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            WPGrid.Columns[1].Frozen = true;
            

            WPGrid.Columns[2].Width = 40;
            WPGrid.Columns[2].HeaderText = "Holes";
            WPGrid.Columns[2].Visible = true;
            WPGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            WPGrid.Columns[2].Frozen = true;
            WPGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            WPGrid.Columns[3].Width = 40;
            WPGrid.Columns[3].HeaderText = "Time";
            WPGrid.Columns[3].Visible = true;
            WPGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            WPGrid.Columns[3].Frozen = true;
            WPGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            WPGrid.Columns[4].Width = 40;
            WPGrid.Columns[4].HeaderText = "Water";
            WPGrid.Columns[4].Visible = true;
            WPGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            WPGrid.Columns[4].Frozen = true;
            WPGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            WPGrid.Columns[5].Width = 40;
            WPGrid.Columns[5].HeaderText = "Air";
            WPGrid.Columns[5].Visible = true;
            WPGrid.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            WPGrid.Columns[5].Frozen = true;
            WPGrid.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            int x = 0;

            //foreach (DataRow dr in _dbManWP.ResultsDataTable.Rows)
            //{
            //    WorkplaceGrid.Rows[x].Cells[0].Value = dr["WP"].ToString() ;
            //    WorkplaceGrid.Rows[x].Cells[2].Value = dr["Holes"].ToString();
            //    WorkplaceGrid.Rows[x].Cells[3].Value = dr["tottime"].ToString();
            //    WorkplaceGrid.Rows[x].Cells[4].Value = dr["water"].ToString();
            //    WorkplaceGrid.Rows[x].Cells[5].Value = dr["air"].ToString();
                
            //    x = x + 1;
            //}

            WorkplaceGrid.RowCount = x;


            WPGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;


            WPGrid.BorderStyle = BorderStyle.FixedSingle;
            WPGrid.Visible = true;
        }

        private void AddWPBtn_Click(object sender, EventArgs e)
        {
            DRWorkplaceFrm BookingLostTimeFrm = (DRWorkplaceFrm)IsBookingFormAlreadyOpen(typeof(DRWorkplaceFrm));
            if (BookingLostTimeFrm == null)
            {
                BookingLostTimeFrm = new DRWorkplaceFrm(this);
                //BookingLostTimeFrm.Text = "Planning Report";

                BookingLostTimeFrm.dateLbl.Text = LblDate.Text;
                BookingLostTimeFrm.EquiNo.Text = DrEquipLbl.Text;
                BookingLostTimeFrm.Shiftlbl.Text = ShiftLbl.Text;
                BookingLostTimeFrm.button1_Click(null, null);

              

                BookingLostTimeFrm.Show();
            }
            else
            {
                BookingLostTimeFrm.WindowState = FormWindowState.Maximized;
                BookingLostTimeFrm.Select();
            }
        }

        private void EngHrsEosTxt_Leave(object sender, EventArgs e)
        {
            EngHrsVarTxt.Text = Convert.ToString(Convert.ToDouble(EngHrsEosTxt.Text) - Convert.ToDouble(EngHrsSosTxt.Text));

            if (Convert.ToDecimal(EngHrsVarTxt.Text) > 8)
            {
                MessageBox.Show("Engine Hours can not be more than 8 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToDecimal(EngHrsVarTxt.Text) < 0)
            {
                MessageBox.Show("Engine Hours can not be more less than 0 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void PPLEosTxt_Leave(object sender, EventArgs e)
        {
            PPLVarTxt.Text = Convert.ToString(Convert.ToDouble(PPLEosTxt.Text) - Convert.ToDouble(PPLSosTxt.Text));

            if (Convert.ToDecimal(PPLVarTxt.Text) > 8)
            {
                MessageBox.Show("Engine Hours can not be more than 8 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToDecimal(PPLVarTxt.Text) < 0)
            {
                MessageBox.Show("Engine Hours can not be more less than 0 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void PPREosTxt_Leave(object sender, EventArgs e)
        {
            PPRVarTxt.Text = Convert.ToString(Convert.ToDouble(PPREosTxt.Text) - Convert.ToDouble(PPRSosTxt.Text));

            if (Convert.ToDecimal(PPRVarTxt.Text) > 8)
            {
                MessageBox.Show("Engine Hours can not be more than 8 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToDecimal(PPRVarTxt.Text) < 0)
            {
                MessageBox.Show("Engine Hours can not be more less than 0 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void PCLEosTxt_Leave(object sender, EventArgs e)
        {
            PCLVarTxt.Text = Convert.ToString(Convert.ToDouble(PCLEosTxt.Text) - Convert.ToDouble(PCLSosTxt.Text));

            if (Convert.ToDecimal(PCLVarTxt.Text) > 8)
            {
                MessageBox.Show("Engine Hours can not be more than 8 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToDecimal(PCLVarTxt.Text) < 0)
            {
                MessageBox.Show("Engine Hours can not be more less than 0 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void PCREosTxt_Leave(object sender, EventArgs e)
        {
            PCRVarTxt.Text = Convert.ToString(Convert.ToDouble(PCREosTxt.Text) - Convert.ToDouble(PCRSosTxt.Text));

            if (Convert.ToDecimal(PCRVarTxt.Text) > 8)
            {
                MessageBox.Show("Engine Hours can not be more than 8 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToDecimal(PCRVarTxt.Text) < 0)
            {
                MessageBox.Show("Engine Hours can not be more less than 0 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void ComEosTxt_Leave(object sender, EventArgs e)
        {
            ComVarTxt.Text = Convert.ToString(Convert.ToDouble(ComEosTxt.Text) - Convert.ToDouble(ComSosTxt.Text));

            if (Convert.ToDecimal(ComVarTxt.Text) > 8)
            {
                MessageBox.Show("Engine Hours can not be more than 8 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToDecimal(ComVarTxt.Text) < 0)
            {
                MessageBox.Show("Engine Hours can not be more less than 0 hours", "Engine Hours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            EquipProbBooking EquipProb = (EquipProbBooking)IsBookingFormAlreadyOpen(typeof(EquipProbBooking));
            if (EquipProb == null)
            {
                EquipProb = new EquipProbBooking(this);
                EquipProb.StartTimetxt.Text = String.Format("{0:hh:mm}",Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                EquipProb.EndTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));

                string startdate;

                startdate = Convert.ToDateTime(EquipProb.StartDate.Value).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.StartTimetxt.Text).ToShortTimeString();

                DateTime sDate;

                sDate = Convert.ToDateTime(startdate);

                string enddate;

                enddate = Convert.ToDateTime(EquipProb.EndDate.Value).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.EndTimetxt.Text).ToShortTimeString();

                DateTime eDate;

                eDate = Convert.ToDateTime(enddate);

                TimeSpan dur;

                dur = eDate.Subtract(sDate);

                int Duration = 0;

                EquipProb.Durtxt.Text = Duration.ToString();

                Duration = Convert.ToInt32(dur.Minutes);

                EquipProb.EquipLbl.Text = EquipLbl.Text;
                EquipProb.ShiftLbl.Text = shiftTypeLbl.Text;

                EquipProb.ShiftDatelbl.Text = Convert.ToDateTime(DateTime.Now).ToShortDateString();

                MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();

                _dbManProb.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _dbManProb.SqlStatement = "select * from Codes_Problems";
                _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManProb.ExecuteInstruction();

                foreach (DataRow dr in _dbManProb.ResultsDataTable.Rows)
                {
                    EquipProb.cmbType.Items.Add(dr["Problem"].ToString());
                }

                EquipProb.Show();
            }
            else
            {
                EquipProb.WindowState = FormWindowState.Maximized;
                EquipProb.Select();
            }
        }

        public void button7_Click(object sender, EventArgs e)
        {
            //////////////////////////////////// Load Delays/////////////////////////////////////////

            ProblemGrid.Visible = false;

            ProblemGrid.RowCount = 1000;
            ProblemGrid.ColumnCount = 7;


            ProblemGrid.Columns[0].Width = 90;
            ProblemGrid.Columns[0].HeaderText = "Start Date";
            ProblemGrid.Columns[0].ReadOnly = true;
            ProblemGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            ProblemGrid.Columns[0].Frozen = false;

            ProblemGrid.Columns[1].Width = 50;
            ProblemGrid.Columns[1].HeaderText = "Start Time";
            ProblemGrid.Columns[1].ReadOnly = true;
            ProblemGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            ProblemGrid.Columns[1].Frozen = false;

            ProblemGrid.Columns[2].Width = 90;
            ProblemGrid.Columns[2].HeaderText = "End Date";
            ProblemGrid.Columns[2].ReadOnly = true;
            ProblemGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            ProblemGrid.Columns[2].Frozen = false;

            ProblemGrid.Columns[3].Width = 50;
            ProblemGrid.Columns[3].HeaderText = "End Time";
            ProblemGrid.Columns[3].ReadOnly = true;
            ProblemGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            ProblemGrid.Columns[3].Frozen = false;

            ProblemGrid.Columns[4].Width = 50;
            ProblemGrid.Columns[4].HeaderText = "Duration";
            ProblemGrid.Columns[4].ReadOnly = true;
            ProblemGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            ProblemGrid.Columns[4].Frozen = false;

            ProblemGrid.Columns[5].Width = 240;
            ProblemGrid.Columns[5].HeaderText = "Problems";
            ProblemGrid.Columns[5].ReadOnly = true;
            ProblemGrid.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            ProblemGrid.Columns[5].Frozen = false;

            ProblemGrid.Columns[6].Width = 160;
            ProblemGrid.Columns[6].HeaderText = "Remarks";
            ProblemGrid.Columns[6].ReadOnly = true;
            ProblemGrid.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            ProblemGrid.Columns[6].Frozen = false;
        

            string Shift = "";

            if (shiftTypeLbl.Text == "Day Shift")
            {
                Shift = "Morning";
            }

            if (shiftTypeLbl.Text == "Afternoon Shift")
            {
                Shift = "Afternoon";
            }

            if (shiftTypeLbl.Text == "Night Shift")
            {
                Shift = "Night";
            }



            MWDataManager.clsDataAccess _dbManLoad = new MWDataManager.clsDataAccess();
            _dbManLoad.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManLoad.SqlStatement = " select * from booking_Problems where bookdate <= '" + String.Format("{0:yyyy-MM-dd}", dateLbl.Text) + "' ";
            _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " and equipno = '" + EquipLbl.Text + "' and shift = '" + Shift + "' ";
            _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " order by starttime ";
            _dbManLoad.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLoad.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManLoad.ExecuteInstruction();

            int y = 0;

            foreach (DataRow dr in _dbManLoad.ResultsDataTable.Rows)
            {
                if (dr["Status"].ToString() != "Closed")
                {
                    ProblemGrid.Rows[y].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    ProblemGrid.Rows[y].DefaultCellStyle.ForeColor = Color.Black;
                }

                ProblemGrid.Rows[y].Cells[0].Value = Convert.ToDateTime(dr["StartDate"].ToString()).ToShortDateString();
                ProblemGrid.Rows[y].Cells[1].Value = dr["StartTime"].ToString();
                ProblemGrid.Rows[y].Cells[2].Value = Convert.ToDateTime(dr["EndDate"].ToString()).ToShortDateString();
                ProblemGrid.Rows[y].Cells[3].Value = dr["EndTime"].ToString();
                ProblemGrid.Rows[y].Cells[4].Value = dr["Duration"].ToString();
                ProblemGrid.Rows[y].Cells[5].Value = dr["Problem"].ToString();
                ProblemGrid.Rows[y].Cells[6].Value = dr["Remarks"].ToString();

                y = y + 1;
            }

            ProblemGrid.RowCount = y;


            ProblemGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            //LHDGrid.BackgroundColor = Color.White;
            ProblemGrid.BorderStyle = BorderStyle.FixedSingle;
            ProblemGrid.Visible = true;
        }

        private void ProblemGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EquipProbBooking EquipProb = (EquipProbBooking)IsBookingFormAlreadyOpen(typeof(EquipProbBooking));
            if (EquipProb == null)
            {
                EquipProb = new EquipProbBooking(this);
                EquipProb.StartTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[1].Value.ToString()));
                EquipProb.StartDate.Value = Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[0].Value.ToString());
                if (ProblemGrid.CurrentRow.DefaultCellStyle.ForeColor == Color.Red)
                {
                    EquipProb.EndTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                }
                else
                {
                    EquipProb.EndTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[3].Value.ToString()));
                    EquipProb.EndDate.Value = Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[2].Value.ToString());
                }

                string startdate;

                startdate = Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[0].Value.ToString()).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.StartTimetxt.Text).ToShortTimeString();

                DateTime sDate;

                sDate = Convert.ToDateTime(startdate);

                string enddate;

                enddate = Convert.ToDateTime(EquipProb.EndDate.Value).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.EndTimetxt.Text).ToShortTimeString();

                DateTime eDate;

                eDate = Convert.ToDateTime(enddate);

                TimeSpan dur;

                dur = eDate.Subtract(sDate);

                int Duration = 0;

                Duration = Convert.ToInt32(dur.TotalHours);

                EquipProb.Durtxt.Text = Duration.ToString();

               

                EquipProb.EquipLbl.Text = EquipLbl.Text;
                EquipProb.ShiftLbl.Text = shiftTypeLbl.Text;

                string Shift = "";

                if (shiftTypeLbl.Text == "Day Shift")
                {
                    Shift = "Morning";
                }

                if (shiftTypeLbl.Text == "Afternoon Shift")
                {
                    Shift = "Afternoon";
                }

                if (shiftTypeLbl.Text == "Night Shift")
                {
                    Shift = "Night";
                }


                MWDataManager.clsDataAccess _dbManLoad = new MWDataManager.clsDataAccess();
                _dbManLoad.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _dbManLoad.SqlStatement = " select * from booking_Problems where bookdate <= '" + String.Format("{0:yyyy-MM-dd}", dateLbl.Text) + "' ";
                _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " and equipno = '" + EquipLbl.Text + "' and shift = '" + Shift + "' ";
                _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " order by starttime ";
                _dbManLoad.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManLoad.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManLoad.ExecuteInstruction();

                int y = 0;

                foreach (DataRow dr in _dbManLoad.ResultsDataTable.Rows)
                {
                    EquipProb.cmbType.Text = dr["Problem"].ToString();
                    EquipProb.Faulttxt.Text = dr["Remarks"].ToString();
                    if (dr["Damaged"].ToString() == "Y")
                    {
                        EquipProb.cbxDamaged.Checked = true;
                    }
                    else
                    {
                        EquipProb.cbxDamaged.Checked = false;
                    }

                    EquipProb.Maximotxt.Text = dr["MaxNo"].ToString();

                    EquipProb.TMRemarkstxt.Text = dr["SandRemarks"].ToString();

                    EquipProb.cmbStatus.Text = dr["Status"].ToString();
                    EquipProb.ShiftDatelbl.Text = Convert.ToDateTime(dr["ShiftDate"].ToString()).ToShortDateString();
                   
                }


                MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();

                _dbManProb.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _dbManProb.SqlStatement = "select * from Codes_Problems";
                _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManProb.ExecuteInstruction();

                foreach (DataRow dr in _dbManProb.ResultsDataTable.Rows)
                {
                    EquipProb.cmbType.Items.Add(dr["Problem"].ToString());
                }

                EquipProb.Show();
            }
            else
            {
                EquipProb.WindowState = FormWindowState.Maximized;
                EquipProb.Select();
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
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

            MWDataManager.clsDataAccess _dbManDel = new MWDataManager.clsDataAccess();

            _dbManDel.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManDel.SqlStatement = "Delete from Booking_Problems";
            _dbManDel.SqlStatement = _dbManDel.SqlStatement + " where Bookdate = '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dateLbl.Text)) + "'";
            _dbManDel.SqlStatement = _dbManDel.SqlStatement + " and equipno = '" + EquipLbl.Text + "' and shift = '" + Shift + "' ";
            _dbManDel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDel.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDel.ExecuteInstruction();

            //MessageFrm MsgFrm = new MessageFrm();
            //MsgFrm.Text = "Record Deleted";
            //Procedures.MsgText = "Record Deleted";
            //MsgFrm.Show();
        }

        public void button8_Click(object sender, EventArgs e)
        {
            //////////////////////////////////// Load Delays/////////////////////////////////////////

            DRProbGrid.Visible = false;

            DRProbGrid.RowCount = 1000;
            DRProbGrid.ColumnCount = 7;


            DRProbGrid.Columns[0].Width = 90;
            DRProbGrid.Columns[0].HeaderText = "Start Date";
            DRProbGrid.Columns[0].ReadOnly = true;
            DRProbGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            DRProbGrid.Columns[0].Frozen = false;

            DRProbGrid.Columns[1].Width = 50;
            DRProbGrid.Columns[1].HeaderText = "Start Time";
            DRProbGrid.Columns[1].ReadOnly = true;
            DRProbGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            DRProbGrid.Columns[1].Frozen = false;

            DRProbGrid.Columns[2].Width = 90;
            DRProbGrid.Columns[2].HeaderText = "End Date";
            DRProbGrid.Columns[2].ReadOnly = true;
            DRProbGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            DRProbGrid.Columns[2].Frozen = false;

            DRProbGrid.Columns[3].Width = 50;
            DRProbGrid.Columns[3].HeaderText = "End Time";
            DRProbGrid.Columns[3].ReadOnly = true;
            DRProbGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            DRProbGrid.Columns[3].Frozen = false;

            DRProbGrid.Columns[4].Width = 50;
            DRProbGrid.Columns[4].HeaderText = "Duration";
            DRProbGrid.Columns[4].ReadOnly = true;
            DRProbGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            DRProbGrid.Columns[4].Frozen = false;

            DRProbGrid.Columns[5].Width = 240;
            DRProbGrid.Columns[5].HeaderText = "Problems";
            DRProbGrid.Columns[5].ReadOnly = true;
            DRProbGrid.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            DRProbGrid.Columns[5].Frozen = false;

            DRProbGrid.Columns[6].Width = 160;
            DRProbGrid.Columns[6].HeaderText = "Remarks";
            DRProbGrid.Columns[6].ReadOnly = true;
            DRProbGrid.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            DRProbGrid.Columns[6].Frozen = false;


            string Shift = "";

            if (shiftTypeLbl.Text == "Day Shift")
            {
                Shift = "Morning";
            }

            if (shiftTypeLbl.Text == "Afternoon Shift")
            {
                Shift = "Afternoon";
            }

            if (shiftTypeLbl.Text == "Night Shift")
            {
                Shift = "Night";
            }



            MWDataManager.clsDataAccess _dbManLoad = new MWDataManager.clsDataAccess();
            _dbManLoad.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManLoad.SqlStatement = " select * from booking_Problems where bookdate <= '" + String.Format("{0:yyyy-MM-dd}", dateLbl.Text) + "' ";
            _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " and equipno = '" + EquipLbl.Text + "' and shift = '" + Shift + "' ";
            _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " order by starttime ";
            _dbManLoad.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLoad.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManLoad.ExecuteInstruction();

            int y = 0;

            foreach (DataRow dr in _dbManLoad.ResultsDataTable.Rows)
            {
                if (dr["Status"].ToString() != "Closed")
                {
                    DRProbGrid.Rows[y].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    DRProbGrid.Rows[y].DefaultCellStyle.ForeColor = Color.Black;
                }

                DRProbGrid.Rows[y].Cells[0].Value = Convert.ToDateTime(dr["StartDate"].ToString()).ToShortDateString();
                DRProbGrid.Rows[y].Cells[1].Value = dr["StartTime"].ToString();
                DRProbGrid.Rows[y].Cells[2].Value = Convert.ToDateTime(dr["EndDate"].ToString()).ToShortDateString();
                DRProbGrid.Rows[y].Cells[3].Value = dr["EndTime"].ToString();
                DRProbGrid.Rows[y].Cells[4].Value = dr["Duration"].ToString();
                DRProbGrid.Rows[y].Cells[5].Value = dr["Problem"].ToString();
                DRProbGrid.Rows[y].Cells[6].Value = dr["Remarks"].ToString();

                y = y + 1;
            }

            DRProbGrid.RowCount = y;


            DRProbGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            //LHDGrid.BackgroundColor = Color.White;
            DRProbGrid.BorderStyle = BorderStyle.FixedSingle;
            DRProbGrid.Visible = true;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            EquipProbBooking EquipProb = (EquipProbBooking)IsBookingFormAlreadyOpen(typeof(EquipProbBooking));
            if (EquipProb == null)
            {
                EquipProb = new EquipProbBooking(this);
                EquipProb.StartTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                EquipProb.EndTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));

                string startdate;

                startdate = Convert.ToDateTime(EquipProb.StartDate.Value).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.StartTimetxt.Text).ToShortTimeString();

                DateTime sDate;

                sDate = Convert.ToDateTime(startdate);

                string enddate;

                enddate = Convert.ToDateTime(EquipProb.EndDate.Value).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.EndTimetxt.Text).ToShortTimeString();

                DateTime eDate;

                eDate = Convert.ToDateTime(enddate);

                TimeSpan dur;

                dur = eDate.Subtract(sDate);

                int Duration = 0;

                EquipProb.Durtxt.Text = Duration.ToString();

                Duration = Convert.ToInt32(dur.Minutes);

                EquipProb.EquipLbl.Text = DrEquipLbl.Text;
                EquipProb.ShiftLbl.Text = ShiftLbl.Text;

                EquipProb.ShiftDatelbl.Text = Convert.ToDateTime(DateTime.Now).ToShortDateString();

                MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();

                _dbManProb.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _dbManProb.SqlStatement = "select * from Codes_Problems";
                _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManProb.ExecuteInstruction();

                foreach (DataRow dr in _dbManProb.ResultsDataTable.Rows)
                {
                    EquipProb.cmbType.Items.Add(dr["Problem"].ToString());
                }

                EquipProb.Show();
            }
            else
            {
                EquipProb.WindowState = FormWindowState.Maximized;
                EquipProb.Select();
            }
        }

        private void DRProbGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EquipProbBooking EquipProb = (EquipProbBooking)IsBookingFormAlreadyOpen(typeof(EquipProbBooking));
            if (EquipProb == null)
            {
                EquipProb = new EquipProbBooking(this);
                EquipProb.StartTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(DRProbGrid.CurrentRow.Cells[1].Value.ToString()));
                EquipProb.StartDate.Value = Convert.ToDateTime(DRProbGrid.CurrentRow.Cells[0].Value.ToString());
                if (ProblemGrid.CurrentRow.DefaultCellStyle.ForeColor == Color.Red)
                {
                    EquipProb.EndTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
                }
                else
                {
                    EquipProb.EndTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(DRProbGrid.CurrentRow.Cells[3].Value.ToString()));
                    EquipProb.EndDate.Value = Convert.ToDateTime(DRProbGrid.CurrentRow.Cells[2].Value.ToString());
                }

                string startdate;

                startdate = Convert.ToDateTime(DRProbGrid.CurrentRow.Cells[0].Value.ToString()).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.StartTimetxt.Text).ToShortTimeString();

                DateTime sDate;

                sDate = Convert.ToDateTime(startdate);

                string enddate;

                enddate = Convert.ToDateTime(EquipProb.EndDate.Value).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.EndTimetxt.Text).ToShortTimeString();

                DateTime eDate;

                eDate = Convert.ToDateTime(enddate);

                TimeSpan dur;

                dur = eDate.Subtract(sDate);

                int Duration = 0;

                Duration = Convert.ToInt32(dur.TotalHours);

                EquipProb.Durtxt.Text = Duration.ToString();



                EquipProb.EquipLbl.Text = DrEquipLbl.Text;
                EquipProb.ShiftLbl.Text = ShiftLbl.Text;

                string Shift = "";

                if (shiftTypeLbl.Text == "Day Shift")
                {
                    Shift = "Morning";
                }

                if (shiftTypeLbl.Text == "Afternoon Shift")
                {
                    Shift = "Afternoon";
                }

                if (shiftTypeLbl.Text == "Night Shift")
                {
                    Shift = "Night";
                }


                MWDataManager.clsDataAccess _dbManLoad = new MWDataManager.clsDataAccess();
                _dbManLoad.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _dbManLoad.SqlStatement = " select * from booking_Problems where bookdate <= '" + String.Format("{0:yyyy-MM-dd}", LblDate.Text) + "' ";
                _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " and equipno = '" + DrEquipLbl.Text + "' and shift = '" + Shift + "' ";
                _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " order by starttime ";
                _dbManLoad.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManLoad.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManLoad.ExecuteInstruction();

                int y = 0;

                foreach (DataRow dr in _dbManLoad.ResultsDataTable.Rows)
                {
                    EquipProb.cmbType.Text = dr["Problem"].ToString();
                    EquipProb.Faulttxt.Text = dr["Remarks"].ToString();
                    if (dr["Damaged"].ToString() == "Y")
                    {
                        EquipProb.cbxDamaged.Checked = true;
                    }
                    else
                    {
                        EquipProb.cbxDamaged.Checked = false;
                    }

                    EquipProb.Maximotxt.Text = dr["MaxNo"].ToString();

                    EquipProb.TMRemarkstxt.Text = dr["SandRemarks"].ToString();

                    EquipProb.cmbStatus.Text = dr["Status"].ToString();
                    EquipProb.ShiftDatelbl.Text = Convert.ToDateTime(dr["ShiftDate"].ToString()).ToShortDateString();

                }


                MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();

                _dbManProb.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _dbManProb.SqlStatement = "select * from Codes_Problems";
                _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManProb.ExecuteInstruction();

                foreach (DataRow dr in _dbManProb.ResultsDataTable.Rows)
                {
                    EquipProb.cmbType.Items.Add(dr["Problem"].ToString());
                }

                EquipProb.Show();
            }
            else
            {
                EquipProb.WindowState = FormWindowState.Maximized;
                EquipProb.Select();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
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

            MWDataManager.clsDataAccess _dbManDel = new MWDataManager.clsDataAccess();

            _dbManDel.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManDel.SqlStatement = "Delete from Booking_Problems";
            _dbManDel.SqlStatement = _dbManDel.SqlStatement + " where Bookdate = '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(LblDate.Text)) + "'";
            _dbManDel.SqlStatement = _dbManDel.SqlStatement + " and equipno = '" + DrEquipLbl.Text + "' and shift = '" + Shift + "' ";
            _dbManDel.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDel.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDel.ExecuteInstruction();

            //MessageFrm MsgFrm = new MessageFrm();
            //MsgFrm.Text = "Record Deleted";
            //Procedures.MsgText = "Record Deleted";
            //MsgFrm.Show();
        }

        
      


        
    }
}
