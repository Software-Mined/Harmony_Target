using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Mineware.Systems.GlobalConnect;

namespace Mineware.Systems.Production.Departmental.Engineering
{
    public partial class frmEngBooking : DevExpress.XtraEditors.XtraForm
    {
        public frmEngBooking()
        {
            InitializeComponent();
        }

        private Procedures procs = new Procedures();

        public string theSystemDBTag;
        public string UserCurrentInfo;

        private void loadBuckets()
        {
            MWDataManager.clsDataAccess _dbManA = new MWDataManager.clsDataAccess();
            _dbManA.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);
            _dbManA.SqlStatement = "select  substring(convert(varchar(20), bookdate, 120),1,10) dd ,  \r\n"+
            "substring(convert(varchar(20), bookdate, 114), 1, 5) tt , * from tbl_Eng_Booking_Buckets \r\n"+
            "where equipno = '" + Equiplabe.Text + "' and substring(convert(varchar(20), bookdate, 120), 1, 10) = '" + Datelabel.Text + "' and shift = '" + Shiftlbl.Text + "'  order by bookdate  \r\n";
            _dbManA.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManA.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManA.ExecuteInstruction();

            DataTable dt1 = _dbManA.ResultsDataTable;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);

            BookGrid.DataSource = ds.Tables[0];

            ColBTime.FieldName = "tt";
            ColBBuckets.FieldName = "Buckets";

        }

        private void frmEngBooking_Load(object sender, EventArgs e)
        {
            StartTime.EditValue = System.DateTime.Now.ToString("HH:mm");
            BucTime.EditValue = System.DateTime.Now.ToString("HH:mm");

            //load labour
            MWDataManager.clsDataAccess _dbMan8aa = new MWDataManager.clsDataAccess();
            _dbMan8aa.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbMan8aa.SqlStatement = "select userid + ':' + Name + LastName nn from [Syncromine_New].[dbo].tblUsers \r\n";

            _dbMan8aa.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan8aa.queryReturnType = MWDataManager.ReturnType.DataTable;         
            _dbMan8aa.ExecuteInstruction();

            DataTable dt = _dbMan8aa.ResultsDataTable;

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    opcmd.Properties.Items.Add(dr["nn"].ToString());
                }
            }


            // load mo
            MWDataManager.clsDataAccess _dbManA = new MWDataManager.clsDataAccess();
            _dbManA.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);
            _dbManA.SqlStatement = "select a.* from [dbo].[tbl_Section] a, \r\n" +
                                    "(select sectionid, Max(prodmonth)pm from[dbo].[tbl_Section] \r\n" +
                                    "where Hierarchicalid = '5' group by sectionid) b \r\n" +
                                    "where a.SectionID = b.sectionid and a.prodmonth = b.pm order by sectionid \r\n";
            _dbManA.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManA.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManA.ExecuteInstruction();

            DataTable dt1 = _dbManA.ResultsDataTable;

            foreach (DataRow dr in dt1.Rows)
            {
                mocmd.Properties.Items.Add(dr["sectionid"].ToString()+":"+ dr["name"].ToString());
            }            
                        // load current info
            _dbManA.SqlStatement = "  select * from [tbl_Eng_Booking_Machine] where BookDate ='" + Datelabel.Text + "' and EquipNo = '" + Equiplabe.Text + "'   \r\n" +
                                  "and shift = '" + Shiftlbl.Text + "'  ";
            _dbManA.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManA.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManA.ExecuteInstruction();

            DataTable dt2 = _dbManA.ResultsDataTable;
            if (dt2.Rows.Count > 0)
            {
                foreach (DataRow dr in dt2.Rows)
                {
                    opcmd.Text = dr["operator"].ToString();
                    mocmd.Text = dr["MO"].ToString();
                    sbcmd.Text = dr["sb"].ToString();

                    EngSTtxt.Text = dr["engsos"].ToString();
                    EngEndtxt.Text = dr["engeos"].ToString();
                    EngVartxt.Text = dr["engdur"].ToString();


                    TransSTtxt.Text = dr["transsos"].ToString();
                    TransEndTtxt.Text = dr["transeos"].ToString();
                    TransVarTtxt.Text = dr["transdur"].ToString();
                }
            }
            else
            {
                // get defauts
                MWDataManager.clsDataAccess _dbManAz = new MWDataManager.clsDataAccess();
                _dbManAz.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);
                _dbManAz.SqlStatement = "  select top(1) * from [tbl_Eng_Booking_Machine] where EquipNo = '" + Equiplabe.Text + "'   \r\n" +
                                                  "order by BookDate desc  ";
                _dbManAz.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManAz.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManAz.ExecuteInstruction();

                DataTable dt3 = _dbManAz.ResultsDataTable;
                if (dt3.Rows.Count > 0)
                {
                    foreach (DataRow dr1 in dt3.Rows)
                    {
                        opcmd.Text = dr1["operator"].ToString();
                        mocmd.Text = dr1["MO"].ToString();
                        sbcmd.Text = dr1["sb"].ToString();

                        EngSTtxt.Text = dr1["engsos"].ToString();
                        EngEndtxt.Text = dr1["engeos"].ToString();
                        EngVartxt.Text = dr1["engdur"].ToString();


                        TransSTtxt.Text = dr1["transsos"].ToString();
                        TransEndTtxt.Text = dr1["transeos"].ToString();
                        TransVarTtxt.Text = dr1["transdur"].ToString();
                    }
                }



            }

            loadBuckets();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {          
             
            MWDataManager.clsDataAccess _dbManA = new MWDataManager.clsDataAccess();
            _dbManA.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);
            _dbManA.SqlStatement = " delete from tbl_Eng_Booking_Machine where EquipNo = '" + Equiplabe.Text + "' and shift =  '" + Shiftlbl.Text + "' \r\n";

            _dbManA.SqlStatement = _dbManA.SqlStatement +
                                   "insert into  [tbl_Eng_Booking_Machine] (BookDate, EquipNo, Shift, Operator, MO, SB, EngSOS, EngEOS, EngDur, TransSOS, TransEOS, TransDur, Buckets )  \r\n" +
                                   "values ('" + Datelabel.Text + "','" + Equiplabe.Text + "', '" + Shiftlbl.Text + "', '" + opcmd.Text + "', '" + mocmd.Text + "'  \r\n " +
                                   ", '" + sbcmd.Text + "', '" + EngSTtxt.Text + "', '" + EngEndtxt.Text + "', '" + EngVartxt.Text + "', '" +TransSTtxt.Text + "', '" + TransEndTtxt.Text + "', '" + TransVarTtxt.Text + "', '" + BucketTtxt.Text + "' )  ";

            _dbManA.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManA.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManA.ExecuteInstruction();
      

             alertControl1.Show(this, "Information", "Record Saved Successfuly.");
            this.Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManA = new MWDataManager.clsDataAccess();
            _dbManA.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);
            _dbManA.SqlStatement = " delete from tbl_Eng_Booking_Machine where EquipNo = '" + Equiplabe.Text + "' and shift =  '" + Shiftlbl.Text + "' \r\n";

            _dbManA.SqlStatement = _dbManA.SqlStatement +
                                   "insert into  [tbl_Eng_Booking_Machine] (BookDate, EquipNo, Shift, Operator, MO, SB, EngSOS, EngEOS, EngDur, TransSOS, TransEOS, TransDur, Buckets )  \r\n" +
                                   "values ('" + Datelabel.Text + "','" + Equiplabe.Text + "', '" + Shiftlbl.Text + "', '" + opcmd.Text + "', '" + mocmd.Text + "'  \r\n " +
                                   ", '" + sbcmd.Text + "', '" + EngSTtxt.Text + "', '" + EngEndtxt.Text + "', '" + EngVartxt.Text + "', '" + TransSTtxt.Text + "', '" + TransEndTtxt.Text + "', '" + TransVarTtxt.Text + "', '" + BucketTtxt.Text + "' )  ";

            _dbManA.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManA.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManA.ExecuteInstruction();


            string StartDate1 = Datelabel.Text + " " + BucTime.Text;

            MWDataManager.clsDataAccess _dbManb = new MWDataManager.clsDataAccess();
            _dbManb.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbManb.SqlStatement = " delete from tbl_Eng_Booking_Buckets where EquipNo = '" + Equiplabe.Text + "' and BookDate =  '" + StartDate1+ "' \r\n"+

             " insert into tbl_Eng_Booking_Buckets values (   '" + StartDate1 + "','" + Equiplabe.Text + "', '" + Shiftlbl.Text + "'  ,  '" + BucketTtxt.Text + "' ) \r\n";

            _dbManb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManb.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManb.ExecuteInstruction();


            loadBuckets();

            alertControl1.Show(this, "Information", "Record Saved Successfuly.");

        }

        private void mocmd_SelectedIndexChanged(object sender, EventArgs e)
        {
            // load sb
            sbcmd.Properties.Items.Clear();

            MWDataManager.clsDataAccess _dbManA = new MWDataManager.clsDataAccess();
            _dbManA.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);
            _dbManA.SqlStatement = "select a.* from [dbo].[tbl_Section] a, \r\n" +
                                    "(select sectionid, Max(prodmonth)pm from[dbo].[tbl_Section] \r\n" +
                                    "where Hierarchicalid = '6' group by sectionid) b \r\n" +
                                    "where a.SectionID = b.sectionid and a.prodmonth = b.pm and reporttosectionid = '" + procs.ExtractBeforeColon(mocmd.Text) + "' order by sectionid \r\n";
            _dbManA.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManA.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManA.ExecuteInstruction();

            DataTable dt1 = _dbManA.ResultsDataTable;

            foreach (DataRow dr in dt1.Rows)
            {
                sbcmd.Properties.Items.Add(dr["sectionid"].ToString() + ":" + dr["name"].ToString());
            }
        }

        private void groupControl4_Click(object sender, EventArgs e)
        {
            Mineware.Systems.Production.Departmental.Engineering.BookingLostTimeFrm LostTime = new Mineware.Systems.Production.Departmental.Engineering.BookingLostTimeFrm();
            LostTime.theSystemDBTag = this.theSystemDBTag;
            LostTime.UserCurrentInfo = this.UserCurrentInfo;
            LostTime.ShowDialog();
        }

        private void ProblemGrid_DoubleClick(object sender, EventArgs e)
        {
            //Mineware.Systems.Production.Departmental.Engineering.EquipProbBooking EquipProb = (Mineware.Systems.Production.Departmental.Engineering.EquipProbBooking)IsBookingFormAlreadyOpen(typeof(Mineware.Systems.Production.Departmental.Engineering.EquipProbBooking));
            //if (EquipProb == null)
            //{
            //    EquipProb = new Mineware.Systems.Production.Departmental.Engineering.EquipProbBooking(this);
            //    EquipProb.StartTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[1].Value.ToString()));
            //    EquipProb.StartDate.Value = Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[0].Value.ToString());
            //    if (ProblemGrid.CurrentRow.DefaultCellStyle.ForeColor == Color.Red)
            //    {
            //        EquipProb.EndTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(DateTime.Now.ToShortTimeString()));
            //    }
            //    else
            //    {
            //        EquipProb.EndTimetxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[3].Value.ToString()));
            //        EquipProb.EndDate.Value = Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[2].Value.ToString());
            //    }

            //    string startdate;

            //    startdate = Convert.ToDateTime(ProblemGrid.CurrentRow.Cells[0].Value.ToString()).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.StartTimetxt.Text).ToShortTimeString();

            //    DateTime sDate;

            //    sDate = Convert.ToDateTime(startdate);

            //    string enddate;

            //    enddate = Convert.ToDateTime(EquipProb.EndDate.Value).ToShortDateString() + " " + Convert.ToDateTime(EquipProb.EndTimetxt.Text).ToShortTimeString();

            //    DateTime eDate;

            //    eDate = Convert.ToDateTime(enddate);

            //    TimeSpan dur;

            //    dur = eDate.Subtract(sDate);

            //    int Duration = 0;

            //    Duration = Convert.ToInt32(dur.TotalHours);

            //    EquipProb.Durtxt.Text = Duration.ToString();



            //    EquipProb.EquipLbl.Text = EquipLbl.Text;
            //    EquipProb.ShiftLbl.Text = shiftTypeLbl.Text;

            //    string Shift = "";

            //    if (shiftTypeLbl.Text == "Day Shift")
            //    {
            //        Shift = "Morning";
            //    }

            //    if (shiftTypeLbl.Text == "Afternoon Shift")
            //    {
            //        Shift = "Afternoon";
            //    }

            //    if (shiftTypeLbl.Text == "Night Shift")
            //    {
            //        Shift = "Night";
            //    }


            //    MWDataManager.clsDataAccess _dbManLoad = new MWDataManager.clsDataAccess();
            //    _dbManLoad.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            //    _dbManLoad.SqlStatement = " select * from booking_Problems where bookdate <= '" + String.Format("{0:yyyy-MM-dd}", dateLbl.Text) + "' ";
            //    _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " and equipno = '" + EquipLbl.Text + "' and shift = '" + Shift + "' ";
            //    _dbManLoad.SqlStatement = _dbManLoad.SqlStatement + " order by starttime ";
            //    _dbManLoad.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManLoad.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManLoad.ExecuteInstruction();

            //    int y = 0;

            //    foreach (DataRow dr in _dbManLoad.ResultsDataTable.Rows)
            //    {
            //        EquipProb.cmbType.Text = dr["Problem"].ToString();
            //        EquipProb.Faulttxt.Text = dr["Remarks"].ToString();
            //        if (dr["Damaged"].ToString() == "Y")
            //        {
            //            EquipProb.cbxDamaged.Checked = true;
            //        }
            //        else
            //        {
            //            EquipProb.cbxDamaged.Checked = false;
            //        }

            //        EquipProb.Maximotxt.Text = dr["MaxNo"].ToString();

            //        EquipProb.TMRemarkstxt.Text = dr["SandRemarks"].ToString();

            //        EquipProb.cmbStatus.Text = dr["Status"].ToString();
            //        EquipProb.ShiftDatelbl.Text = Convert.ToDateTime(dr["ShiftDate"].ToString()).ToShortDateString();

            //    }


            //    MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();

            //    _dbManProb.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            //    _dbManProb.SqlStatement = "select * from Codes_Problems";
            //    _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManProb.ExecuteInstruction();

            //    foreach (DataRow dr in _dbManProb.ResultsDataTable.Rows)
            //    {
            //        EquipProb.cmbType.Items.Add(dr["Problem"].ToString());
            //    }

            //    EquipProb.Show();
            //}
            //else
            //{
            //    EquipProb.WindowState = FormWindowState.Maximized;
            //    EquipProb.Select();
            //}
        }

    }
}
