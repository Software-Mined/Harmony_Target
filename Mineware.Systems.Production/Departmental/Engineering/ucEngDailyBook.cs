using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;

namespace Mineware.Systems.Production.Departmental.Engineering
{
    public partial class ucEngDailyBook : BaseUserControl
    {
        public ucEngDailyBook()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpUser);
            FormActiveRibbonPage = rpUser;
            FormMainRibbonPage = rpUser;
            RibbonControl = ribbonControl1;
        }

        //string UserCurrent = "Amplats";

        public void LoadInfo()
        {
            string Shift = "D";
            if (radioGroup2.SelectedIndex == 1)
                Shift = "A";
            if (radioGroup2.SelectedIndex == 2)
                Shift = "N";


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);

            _dbMan.SqlStatement = " select a.equipno noee, b.*,c.bb from [dbo].[tbl_Eng_Equip_Inventory] a \r\n" +
            "left outer join ( \r\n" +
            "select * from[dbo].[tbl_Eng_Booking_Machine] \r\n" +
            "where bookdate = '" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "' and shift = '" + Shift + "' ) b on a.equipno = b.equipno \r\n" +
            "left outer join \r\n" +
            "(select convert(datetime, substring(convert(varchar(20), bookdate, 120), 1, 10)) dd, equipno, sum(buckets) bb \r\n" +
            "from[dbo].[tbl_Eng_Booking_Buckets] \r\n" +
            "where  substring(convert(varchar(20), bookdate, 120), 1, 10) = '" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "'  and shift = '" + Shift + "' \r\n" +
            "group by  convert(datetime, substring(convert(varchar(20), bookdate, 120), 1, 10)), equipno) c on a.equipno = c.equipno " +
                                  "order by noee, mo     ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            BookGrid.DataSource = ds.Tables[0];

            ColaEquipID.FieldName = "noee";
            ColaOperator.FieldName = "Operator";
            ColaMO.FieldName = "MO";
            ColaSB.FieldName = "SB";
            ColaEngSOS.FieldName = "EngSOS";
            ColaEngEOS.FieldName = "EngEOS";
            ColaEngVar.FieldName = "EngDur";

            ColaTransSOS.FieldName = "TransSOS";
            ColaTransEOS.FieldName = "TransEOS";
            ColaTransDur.FieldName = "TransDur";

            ColaBuckets.FieldName = "bb";

        }



        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (EqipSelectlabel.Text != "NN")
            {
                string Shift = "D";
                if (radioGroup2.SelectedIndex == 1)
                    Shift = "A";
                if (radioGroup2.SelectedIndex == 2)
                    Shift = "N";

                Mineware.Systems.Production.Departmental.Engineering.frmEngBooking EngBooking = new Mineware.Systems.Production.Departmental.Engineering.frmEngBooking();
                EngBooking.Equiplabe.Text = EqipSelectlabel.Text;
                EngBooking.Datelabel.Text = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                EngBooking.Shiftlbl.Text = Shift;

                EngBooking.theSystemDBTag = this.theSystemDBTag;
                EngBooking.UserCurrentInfo = this.UserCurrentInfo.Connection;

                EngBooking.ShowDialog();
                LoadInfo();
            }
        }

        private void ucEngDailyBook_Load(object sender, EventArgs e)
        {
            Mineware.Systems.ProductionGlobal.ProductionGlobal.GetSysSettings(theSystemDBTag, UserCurrentInfo);
            LoadInfo();
        }

        private void bandedGridView2a_DoubleClick(object sender, EventArgs e)
        {
            string Shift = "D";
            if (radioGroup2.SelectedIndex == 1)
                Shift = "A";
            if (radioGroup2.SelectedIndex == 2)
                Shift = "N";

            Mineware.Systems.Production.Departmental.Engineering.frmEngBooking EngBooking = new Mineware.Systems.Production.Departmental.Engineering.frmEngBooking();
            EngBooking.Equiplabe.Text = EqipSelectlabel.Text;
            EngBooking.Datelabel.Text = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            EngBooking.Shiftlbl.Text = Shift;

            EngBooking.theSystemDBTag = this.theSystemDBTag;
            EngBooking.UserCurrentInfo = this.UserCurrentInfo.Connection;

            EngBooking.ShowDialog();
            LoadInfo();
        }

        private void bandedGridView2a_Click(object sender, EventArgs e)
        {
            
        }

        private void bandedGridView2a_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            EqipSelectlabel.Text = bandedGridView2a.GetRowCellValue(e.RowHandle, bandedGridView2a.Columns[0]).ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {  
            LoadInfo();
        }
    }
}
