using DevExpress.XtraEditors;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.Production.Departmental.LongHoleDrilling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Bookings
{
    public partial class ucBookings_LongHoleDrilling : BaseUserControl
    {
        private string bookWorkplace;
        public ucBookings_LongHoleDrilling()
        {
            InitializeComponent();

            FormRibbonPages.Add(rpLongHoleDrilling);
            FormActiveRibbonPage = rpLongHoleDrilling;
            FormMainRibbonPage = rpLongHoleDrilling;
            RibbonControl = rcLongHoleDrilling;
        }

        private void gvBookings_DoubleClick(object sender, EventArgs e)
        {
            frmBookings frm = new frmBookings();
            frm._Connection = TConnections.GetConnectionString(theSystemDBTag, TUserInfo.Site);
            frm.editWorkplace.EditValue = bookWorkplace;
            frm.ShowDialog();

            LoadBookings();
        }

        private void gvBookings_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            bookWorkplace = gvBookings.GetRowCellValue(e.RowHandle, gvBookings.Columns["Workplace"].FieldName).ToString();
        }


        void LoadBookings()
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, TUserInfo.Site);
            _dbMan1.SqlStatement = " exec sp_LongHoleDrilling_LoadBookingsMain  ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable HolesNA1 = _dbMan1.ResultsDataTable;

            gcBookings.DataSource = HolesNA1;

            colBookWorkplace.FieldName = "Workplace";
            colBookRing.FieldName = "RingName";
            colBookHole.FieldName = "HoleNo";
            colBookMachine.FieldName = "MachineNo";
            colBookLength.FieldName = "Length";
            colBookDur.FieldName = "Dur";
            colBookStartDate.FieldName = "StartDate";

            DateTime SDate = Convert.ToDateTime(_dbMan1.ResultsDataTable.Rows[0]["StartDate"].ToString());

            string Day1 = SDate.ToString("dd MMM yyyy");
            string Day2 = SDate.AddDays(+1).ToString("dd MMM yyyy");
            string Day3 = SDate.AddDays(+2).ToString("dd MMM yyyy");
            string Day4 = SDate.AddDays(+3).ToString("dd MMM yyyy");
            string Day5 = SDate.AddDays(+4).ToString("dd MMM yyyy");
            string Day6 = SDate.AddDays(+5).ToString("dd MMM yyyy");
            string Day7 = SDate.AddDays(+6).ToString("dd MMM yyyy");
            string Day8 = SDate.AddDays(+7).ToString("dd MMM yyyy");
            string Day9 = SDate.AddDays(+8).ToString("dd MMM yyyy");
            string Day10 = SDate.AddDays(+9).ToString("dd MMM yyyy");
            string Day11 = SDate.AddDays(+10).ToString("dd MMM yyyy");
            string Day12 = SDate.AddDays(+11).ToString("dd MMM yyyy");
            string Day13 = SDate.AddDays(+12).ToString("dd MMM yyyy");
            string Day14 = SDate.AddDays(+13).ToString("dd MMM yyyy");
            string Day15 = SDate.AddDays(+14).ToString("dd MMM yyyy");
            string Day16 = SDate.AddDays(+15).ToString("dd MMM yyyy");
            string Day17 = SDate.AddDays(+16).ToString("dd MMM yyyy");
            string Day18 = SDate.AddDays(+17).ToString("dd MMM yyyy");
            string Day19 = SDate.AddDays(+18).ToString("dd MMM yyyy");
            string Day20 = SDate.AddDays(+19).ToString("dd MMM yyyy");
            string Day21 = SDate.AddDays(+20).ToString("dd MMM yyyy");
            string Day22 = SDate.AddDays(+21).ToString("dd MMM yyyy");
            string Day23 = SDate.AddDays(+22).ToString("dd MMM yyyy");
            string Day24 = SDate.AddDays(+23).ToString("dd MMM yyyy");
            string Day25 = SDate.AddDays(+24).ToString("dd MMM yyyy");
            string Day26 = SDate.AddDays(+25).ToString("dd MMM yyyy");
            string Day27 = SDate.AddDays(+26).ToString("dd MMM yyyy");
            string Day28 = SDate.AddDays(+27).ToString("dd MMM yyyy");
            string Day29 = SDate.AddDays(+28).ToString("dd MMM yyyy");
            string Day30 = SDate.AddDays(+29).ToString("dd MMM yyyy");
            string Day31 = SDate.AddDays(+30).ToString("dd MMM yyyy");
            string Day32 = SDate.AddDays(+31).ToString("dd MMM yyyy");
            string Day33 = SDate.AddDays(+32).ToString("dd MMM yyyy");
            string Day34 = SDate.AddDays(+33).ToString("dd MMM yyyy");
            string Day35 = SDate.AddDays(+34).ToString("dd MMM yyyy");
            string Day36 = SDate.AddDays(+35).ToString("dd MMM yyyy");
            string Day37 = SDate.AddDays(+36).ToString("dd MMM yyyy");
            string Day38 = SDate.AddDays(+37).ToString("dd MMM yyyy");
            string Day39 = SDate.AddDays(+38).ToString("dd MMM yyyy");
            string Day40 = SDate.AddDays(+39).ToString("dd MMM yyyy");

            gridBandPM.Caption = Day1 + " - " + Day39;
            h1.Caption = Day1;
            h2.Caption = Day2;
            h3.Caption = Day3;
            h4.Caption = Day4;
            h5.Caption = Day5;
            h6.Caption = Day6;
            h7.Caption = Day7;
            h8.Caption = Day8;
            h9.Caption = Day9;
            h10.Caption = Day10;
            h11.Caption = Day11;
            h12.Caption = Day12;
            h13.Caption = Day13;
            h14.Caption = Day14;
            h15.Caption = Day15;
            h16.Caption = Day16;
            h17.Caption = Day17;
            h18.Caption = Day18;
            h19.Caption = Day19;
            h20.Caption = Day20;
            h21.Caption = Day21;
            h22.Caption = Day22;
            h23.Caption = Day23;
            h24.Caption = Day24;
            h25.Caption = Day25;
            h26.Caption = Day26;
            h27.Caption = Day27;
            h28.Caption = Day28;
            h29.Caption = Day29;
            h30.Caption = Day30;
            h31.Caption = Day31;
            h32.Caption = Day32;
            h33.Caption = Day33;
            h34.Caption = Day34;
            h35.Caption = Day35;
            h36.Caption = Day36;
            h37.Caption = Day37;
            h38.Caption = Day38;
            h39.Caption = Day39;
            h40.Caption = Day40;

            colBookProgPlan.FieldName = "ProgPlan";
            colBookProgBook.FieldName = "ProgBook";

            colBook1.FieldName = "Day1Book";
            colBook2.FieldName = "Day2Book";
            colBook3.FieldName = "Day3Book";
            colBook4.FieldName = "Day4Book";
            colBook5.FieldName = "Day5Book";
            colBook6.FieldName = "Day6Book";
            colBook7.FieldName = "Day7Book";
            colBook8.FieldName = "Day8Book";
            colBook9.FieldName = "Day9Book";
            colBook10.FieldName = "Day10Book";
            colBook11.FieldName = "Day11Book";
            colBook12.FieldName = "Day12Book";
            colBook13.FieldName = "Day13Book";
            colBook14.FieldName = "Day14Book";
            colBook15.FieldName = "Day15Book";
            colBook16.FieldName = "Day16Book";
            colBook17.FieldName = "Day17Book";
            colBook18.FieldName = "Day18Book";
            colBook19.FieldName = "Day19Book";
            colBook20.FieldName = "Day20Book";
            colBook21.FieldName = "Day21Book";
            colBook22.FieldName = "Day22Book";
            colBook23.FieldName = "Day23Book";
            colBook24.FieldName = "Day24Book";
            colBook25.FieldName = "Day25Book";
            colBook26.FieldName = "Day26Book";
            colBook27.FieldName = "Day27Book";
            colBook28.FieldName = "Day28Book";
            colBook29.FieldName = "Day29Book";
            colBook30.FieldName = "Day30Book";
            colBook31.FieldName = "Day31Book";
            colBook32.FieldName = "Day32Book";
            colBook33.FieldName = "Day33Book";
            colBook34.FieldName = "Day34Book";
            colBook35.FieldName = "Day35Book";
            colBook36.FieldName = "Day36Book";
            colBook37.FieldName = "Day37Book";
            colBook38.FieldName = "Day38Book";
            colBook39.FieldName = "Day39Book";
            colBook40.FieldName = "Day40Book";

            colPlan1.FieldName = "Day1Plan";
            colPlan2.FieldName = "Day2Plan";
            colPlan3.FieldName = "Day3Plan";
            colPlan4.FieldName = "Day4Plan";
            colPlan5.FieldName = "Day5Plan";
            colPlan6.FieldName = "Day6Plan";
            colPlan7.FieldName = "Day7Plan";
            colPlan8.FieldName = "Day8Plan";
            colPlan9.FieldName = "Day9Plan";
            colPlan10.FieldName = "Day10Plan";
            colPlan11.FieldName = "Day11Plan";
            colPlan12.FieldName = "Day12Plan";
            colPlan13.FieldName = "Day13Plan";
            colPlan14.FieldName = "Day14Plan";
            colPlan15.FieldName = "Day15Plan";
            colPlan16.FieldName = "Day16Plan";
            colPlan17.FieldName = "Day17Plan";
            colPlan18.FieldName = "Day18Plan";
            colPlan19.FieldName = "Day19Plan";
            colPlan20.FieldName = "Day20Plan";
            colPlan21.FieldName = "Day21Plan";
            colPlan22.FieldName = "Day22Plan";
            colPlan23.FieldName = "Day23Plan";
            colPlan24.FieldName = "Day24Plan";
            colPlan25.FieldName = "Day25Plan";
            colPlan26.FieldName = "Day26Plan";
            colPlan27.FieldName = "Day27Plan";
            colPlan28.FieldName = "Day28Plan";
            colPlan29.FieldName = "Day29Plan";
            colPlan30.FieldName = "Day30Plan";
            colPlan31.FieldName = "Day31Plan";
            colPlan32.FieldName = "Day32Plan";
            colPlan33.FieldName = "Day33Plan";
            colPlan34.FieldName = "Day34Plan";
            colPlan35.FieldName = "Day35Plan";
            colPlan36.FieldName = "Day36Plan";
            colPlan37.FieldName = "Day37Plan";
            colPlan38.FieldName = "Day38Plan";
            colPlan39.FieldName = "Day39Plan";
            colPlan40.FieldName = "Day40Plan";

        }

        private void ucBookings_LongHoleDrilling_Load(object sender, EventArgs e)
        {
            tbDpInspecDate.EditValue = System.DateTime.Today;
            LoadBookings();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
