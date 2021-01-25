using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using Mineware.Systems.Global;
using DevExpress.XtraScheduler.Commands;
using Mineware.Systems.GlobalConnect;
using FastReport;
using FastReport.Export.Pdf;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.ProductionGlobal;

namespace Mineware.Systems.Production.Departmental.Vamping
{
    public partial class ucVampPlanning : BaseUserControl
    {
        DataTable dtWPTot = new DataTable();
        Report theReport = new Report();
        Procedures procs = new Procedures();
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        public ucVampPlanning()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpVamping);
            FormActiveRibbonPage = rpVamping;
            FormMainRibbonPage = rpVamping;
            RibbonControl = rcVamping;
        }       

        public string ExtractAfterColon(string TheString)
        {
            string AfterColon;

            int index = TheString.IndexOf(":"); // Kry die postion van die :

            AfterColon = TheString.Substring(index + 1); // kry alles na :

            return AfterColon;
        }
        public string ExtractBeforeColon(string TheString)
        {
            if (TheString != "")
            {
                string BeforeColon;
                int index = TheString.IndexOf(":");
                BeforeColon = TheString.Substring(0, index);
                return BeforeColon;
            }
            else
            {
                return "";
            }
        }

        public DataView Search(DataTable SearchTable, string SearchString)
        {
            DataView dv = new DataView(SearchTable);
            string SearchExpression = null;

            if (!String.IsNullOrEmpty(SearchString))//(Filtertxt.Text))
            {

                SearchExpression = string.Format("'{0}%'", SearchString);//Filtertxt.Text);
                dv.RowFilter = "Description like " + SearchExpression;
            }

            //DataTable dtResult = 
            //MessageBox.Show(SearchTable.Rows.Count.ToString());
            return dv;
        }

        private void Closebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void ucVampPlanning_Load(object sender, EventArgs e)
        {
            ProdmonthEdit.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobal.ProductionGlobalTSysSettings._currentProductionMonth.ToString());

            MWDataManager.clsDataAccess _dbMan11 = new MWDataManager.clsDataAccess();
            _dbMan11.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan11.SqlStatement = "select Code code1, * from tbl_Code_Cycle_Vamps order by Code ";

            _dbMan11.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan11.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan11.ExecuteInstruction();

            DataTable Neilzz1 = _dbMan11.ResultsDataTable;
            CycleCodeLstOther.Items.Clear();

            foreach (DataRow r in Neilzz1.Rows)
            {
                CycleCodeLstOther.Items.Add(r["code1"].ToString() + ":" + r["description"].ToString());
            }



            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWP.SqlStatement = "Select distinct s1.ReportToSectionid+':'+s2.name Section from tbl_section s, tbl_section s1 , tbl_section s2  \r\n" +
                                    "where s.Prodmonth = s1.Prodmonth and s.ReportToSectionid = s1.SectionID  \r\n" +
                                    "and s1.Prodmonth = s2.Prodmonth and s1.ReportToSectionid = s2.SectionID  \r\n" +
                                    "and s.Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "' ";

            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            DataTable dt = _dbManWP.ResultsDataTable;

            repositoryItemComboBox2.Items.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                repositoryItemComboBox2.Items.Add(dr["Section"].ToString());
            }
        }

        private void CycleCodeLstOther_MouseDown(object sender, MouseEventArgs e)
        {
            //IsCycleCode = true;

            if (CycleCodeLstOther.Items.Count == 0)
                return;
            int index = CycleCodeLstOther.IndexFromPoint(e.X, e.Y);
            if (index > -1)
            {
                string s = CycleCodeLstOther.Items[index].ToString();
                lblCode.Text = ExtractBeforeColon(CycleCodeLstOther.Items[index].ToString());
                DragDropEffects dde1 = DoDragDrop(s,
                    DragDropEffects.All);
            }
        }

        private void OtherCycleGrid_DragDrop(object sender, DragEventArgs e)
        {
           
        }

        private void TotWpFilterTxt_TextChanged(object sender, EventArgs e)
        {
            TotWpList.Items.Clear();

            string zzzz = "*" + TotWpFilterTxt.Text;


            foreach (DataRowView r in Search(dtWPTot, zzzz))
            {
                if (r["workplaceid"].ToString() == "")
                    TotWpList.Items.Add("NotAssig" + ":" + r["description"].ToString());
                else
                    TotWpList.Items.Add(r["workplaceid"].ToString() + ":" + r["description"].ToString());

            }

        }

        private void TotWpList_DoubleClick(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManGetInfo = new MWDataManager.clsDataAccess();
            _dbManGetInfo.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManGetInfo.SqlStatement = " select * from tbl_VampingPreInspectionSheet where workplaceid = '" + ExtractAfterColon(TotWpList.SelectedItem.ToString()) + "' ";
            _dbManGetInfo.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManGetInfo.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManGetInfo.ResultsTableName = "sysset";
            _dbManGetInfo.ExecuteInstruction();

            DataTable GetInfo = _dbManGetInfo.ResultsDataTable;

            foreach (DataRow r in GetInfo.Rows)
            {
                Metertxt.Text = r["vampmetres"].ToString();
                SwpTonsEdit.Text = r["TotalTons"].ToString();
                Gradetxt.Text = r["TotalValue"].ToString();
                ContentTxt.Text = r["TotalContent"].ToString();
                VampSqmEdit.Text = r["Totalsqm"].ToString();
            }


            frmVampingProp frm = new frmVampingProp();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm._theSystemDBTag = theSystemDBTag;
            frm._UserCurrentInfoConnection = UserCurrentInfo.Connection;

            frm.lblprodmonth.Text = procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString()));
            frm.lblsection.Text = ExtractBeforeColon(SectionEdit.EditValue.ToString());
            frm.lblWorkplace.Text = TotWpList.SelectedItem.ToString();
            frm.lblSaved.Text = "N";

            frm.ShowDialog();

            if (frm.lblSaved.Text == "Y")
            {
                SecNamelbl.Text = frm.lbxMiners.SelectedItem.ToString();
                OrgDlbl.Text = frm.lbxGang.SelectedItem.ToString();
                BoxHolelbl.Text = frm.lbxBoxHole.SelectedItem.ToString();

                SecNamelbl.Visible = true;
                OrgDlbl.Visible = true;
                BoxHolelbl.Visible = true;

                lcgInfo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                SecNamelbl.Text = "";
                OrgDlbl.Text = "";
                BoxHolelbl.Text = "";

                SecNamelbl.Visible = false;
                OrgDlbl.Visible = false;
                BoxHolelbl.Visible = false;
            }
            

        }

        private void SectionEdit_EditValueChanged(object sender, EventArgs e)
        {
            SSectionLbl.Text = SectionEdit.EditValue.ToString();


            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWP.SqlStatement = "select w.workplaceid, w.description, w.activity from tbl_VampingPreInspectionSheet wptot left outer join tbl_workplace w on wptot.workplaceid = w.description and section  = '" + ExtractBeforeColon(SSectionLbl.Text) + "'   " +
                                    " and auth = 'Y' and del = 'N' order by w.description ";

            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            dtWPTot = _dbManWP.ResultsDataTable;


            TotWpFilterTxt_TextChanged(null, null);
        }

        

        private void ProdmonthEdit_EditValueChanged_1(object sender, EventArgs e)
        {
            
        }

        private void repositoryItemSpinEdit1_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void repositoryItemSpinEdit1_Click(object sender, EventArgs e)
        {

        }

        private void repositoryItemSpinEdit1_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            string _sMonth = ProdmonthEdit.EditValue.ToString().Substring(4, 2);

            int _iMonth = Convert.ToInt32(ProdmonthEdit.EditValue.ToString().Substring(4, 2));
            int Year = Convert.ToInt32(ProdmonthEdit.EditValue.ToString().Substring(0, 4));

            if (_iMonth >= 12)
            {
                Year = Year + 1;
                _sMonth = "01";

                ProdmonthEdit.EditValue = Convert.ToString(Year) + _sMonth;
            }

            if (_iMonth <= 1)
            {
                Year = Year - 1;
                _sMonth = "12";

                ProdmonthEdit.EditValue = Year.ToString() + _sMonth;
            }

            //return;

            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWP.SqlStatement = "Select distinct s1.ReportToSectionid+':'+s2.name Section from tbl_section s, tbl_section s1 , tbl_section s2  \r\n" +
                                    "where s.Prodmonth = s1.Prodmonth and s.ReportToSectionid = s1.SectionID  \r\n" +
                                    "and s1.Prodmonth = s2.Prodmonth and s1.ReportToSectionid = s2.SectionID  \r\n" +
                                    "and s.Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "' ";

            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            DataTable dt = _dbManWP.ResultsDataTable;

            repositoryItemComboBox2.Items.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                repositoryItemComboBox2.Items.Add(dr["Section"].ToString());
            }  
        }

        private void SecNamelbl_TextChanged(object sender, EventArgs e)
        {
            if (SecNamelbl.Text == "")
            {
                return;
            }


            MWDataManager.clsDataAccess _dbManSB = new MWDataManager.clsDataAccess();
            _dbManSB.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSB.SqlStatement = "Select distinct s1.SectionID Section from tbl_section s, tbl_section s1 , tbl_section s2  \r\n" +
                                    "where s.Prodmonth = s1.Prodmonth and s.ReportToSectionid = s1.SectionID  \r\n" +
                                    "and s1.Prodmonth = s2.Prodmonth and s1.ReportToSectionid = s2.SectionID  \r\n" +
                                    "and s.Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "' and s.sectionID = '"+ExtractBeforeColon( SecNamelbl.Text )+"'  ";

            _dbManSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSB.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSB.ExecuteInstruction();

            DataTable dtSB = _dbManSB.ResultsDataTable;

            
           string Shiftboss = dtSB.Rows[0][0].ToString();
            
            


            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWP.SqlStatement = "declare @StartDate datetime    \r\n"+
                                    "declare @EndDate datetime    \r\n"+
                                    "declare @CalCode varchar(200)    \r\n" +

                                    "set @StartDate = (Select BeginDate from vw_Seccal_totalShifts_Calc where Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "' and Sectionid = '"+ Shiftboss + "')    \r\n" +
                                    "set @EndDate = (Select EndDate from vw_Seccal_totalShifts_Calc where Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "' and Sectionid = '" + Shiftboss + "')    \r\n" +

                                    "set @CalCode = (Select CalendarCode from vw_Seccal_totalShifts_Calc where Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "' and Sectionid = '" + Shiftboss + "')    \r\n" +

                                    "Select @StartDate StartDate,@EndDate EndDate,'MO Cycle' CycleName    \r\n" +

                                    ", max(Day1) Day1, max(Day2) Day2, max(Day3) Day3, max(Day4) Day4, max(Day5) Day5    \r\n" +
                                    ", max(Day6) Day6, max(Day7) Day7, max(Day8) Day8, max(Day9) Day9, max(Day10) Day10    \r\n" +

                                    ", max(Day11) Day11, max(Day12) Day12, max(Day13) Day13, max(Day14) Day14, max(Day15) Day15    \r\n" +
                                    ", max(Day16) Day16, max(Day17) Day17, max(Day18) Day18, max(Day19) Day19, max(Day20) Day20    \r\n" +

                                    ", max(Day21) Day21, max(Day22) Day22, max(Day23) Day23, max(Day24) Day24, max(Day25) Day25    \r\n" +
                                    ", max(Day26) Day26, max(Day27) Day27, max(Day28) Day28, max(Day29) Day29, max(Day30) Day30    \r\n" +

                                    ", max(Day31) Day31, max(Day32) Day32, max(Day33) Day33, max(Day34) Day34, max(Day35) Day35    \r\n" +
                                    ", max(Day36) Day36, max(Day37) Day37, max(Day38) Day38, max(Day39) Day39, max(Day40) Day40    \r\n" +

                                    ", max(Day41) Day41, max(Day42) Day42, max(Day43) Day43, max(Day44) Day44, max(Day45) Day45    \r\n" +
                                    ", max(Day46) Day46, max(Day47) Day47, max(Day48) Day48, max(Day49) Day49, max(Day50) Day50    \r\n" +

                                    "from(    \r\n" +
                                    "Select     \r\n" +

                                    "case when CalendarDate = @StartDate and Workingday = 'N' then 'OFF' else '' end as Day1    \r\n" +
                                    ",case when CalendarDate = @StartDate + 1 and Workingday = 'N' then 'OFF' else '' end as Day2    \r\n" +
                                    ",case when CalendarDate = @StartDate + 2 and Workingday = 'N' then 'OFF' else '' end as Day3    \r\n" +
                                    ",case when CalendarDate = @StartDate + 3 and Workingday = 'N' then 'OFF' else '' end as Day4    \r\n" +
                                    ",case when CalendarDate = @StartDate + 4 and Workingday = 'N' then 'OFF' else '' end as Day5    \r\n" +
                                    ",case when CalendarDate = @StartDate + 5 and Workingday = 'N' then 'OFF' else '' end as Day6    \r\n" +
                                    ",case when CalendarDate = @StartDate + 6 and Workingday = 'N' then 'OFF' else '' end as Day7    \r\n" +
                                    ",case when CalendarDate = @StartDate + 7 and Workingday = 'N' then 'OFF' else '' end as Day8    \r\n" +
                                    ",case when CalendarDate = @StartDate + 8 and Workingday = 'N' then 'OFF' else '' end as Day9    \r\n" +
                                    ",case when CalendarDate = @StartDate + 9 and Workingday = 'N' then 'OFF' else '' end as Day10    \r\n" +

                                    ",case when CalendarDate = @StartDate + 10 and Workingday = 'N' then 'OFF' else '' end as Day11    \r\n" +
                                    ",case when CalendarDate = @StartDate + 11 and Workingday = 'N' then 'OFF' else '' end as Day12    \r\n" +
                                    ",case when CalendarDate = @StartDate + 12 and Workingday = 'N' then 'OFF' else '' end as Day13    \r\n" +
                                    ",case when CalendarDate = @StartDate + 13 and Workingday = 'N' then 'OFF' else '' end as Day14    \r\n" +
                                    ",case when CalendarDate = @StartDate + 14 and Workingday = 'N' then 'OFF' else '' end as Day15    \r\n" +
                                    ",case when CalendarDate = @StartDate + 15 and Workingday = 'N' then 'OFF' else '' end as Day16    \r\n" +
                                    ",case when CalendarDate = @StartDate + 16 and Workingday = 'N' then 'OFF' else '' end as Day17    \r\n" +
                                    ",case when CalendarDate = @StartDate + 17 and Workingday = 'N' then 'OFF' else '' end as Day18    \r\n" +
                                    ",case when CalendarDate = @StartDate + 18 and Workingday = 'N' then 'OFF' else '' end as Day19    \r\n" +
                                    ",case when CalendarDate = @StartDate + 19 and Workingday = 'N' then 'OFF' else '' end as Day20    \r\n" +

                                    ",case when CalendarDate = @StartDate + 20 and Workingday = 'N' then 'OFF' else '' end as Day21    \r\n" +
                                    ",case when CalendarDate = @StartDate + 21 and Workingday = 'N' then 'OFF' else '' end as Day22    \r\n" +
                                    ",case when CalendarDate = @StartDate + 22 and Workingday = 'N' then 'OFF' else '' end as Day23    \r\n" +
                                    ",case when CalendarDate = @StartDate + 23 and Workingday = 'N' then 'OFF' else '' end as Day24    \r\n" +
                                    ",case when CalendarDate = @StartDate + 24 and Workingday = 'N' then 'OFF' else '' end as Day25    \r\n" +
                                    ",case when CalendarDate = @StartDate + 25 and Workingday = 'N' then 'OFF' else '' end as Day26    \r\n" +
                                    ",case when CalendarDate = @StartDate + 26 and Workingday = 'N' then 'OFF' else '' end as Day27    \r\n" +
                                    ",case when CalendarDate = @StartDate + 27 and Workingday = 'N' then 'OFF' else '' end as Day28    \r\n" +
                                    ",case when CalendarDate = @StartDate + 28 and Workingday = 'N' then 'OFF' else '' end as Day29    \r\n" +
                                    ",case when CalendarDate = @StartDate + 29 and Workingday = 'N' then 'OFF' else '' end as Day30    \r\n" +

                                    ",case when CalendarDate = @StartDate + 30 and Workingday = 'N' then 'OFF' else '' end as Day31    \r\n" +
                                    ",case when CalendarDate = @StartDate + 31 and Workingday = 'N' then 'OFF' else '' end as Day32    \r\n" +
                                    ",case when CalendarDate = @StartDate + 32 and Workingday = 'N' then 'OFF' else '' end as Day33    \r\n" +
                                    ",case when CalendarDate = @StartDate + 33 and Workingday = 'N' then 'OFF' else '' end as Day34    \r\n" +
                                    ",case when CalendarDate = @StartDate + 34 and Workingday = 'N' then 'OFF' else '' end as Day35    \r\n" +
                                    ",case when CalendarDate = @StartDate + 35 and Workingday = 'N' then 'OFF' else '' end as Day36    \r\n" +
                                    ",case when CalendarDate = @StartDate + 36 and Workingday = 'N' then 'OFF' else '' end as Day37    \r\n" +
                                    ",case when CalendarDate = @StartDate + 37 and Workingday = 'N' then 'OFF' else '' end as Day38    \r\n" +
                                    ",case when CalendarDate = @StartDate + 38 and Workingday = 'N' then 'OFF' else '' end as Day39    \r\n" +
                                    ",case when CalendarDate = @StartDate + 39 and Workingday = 'N' then 'OFF' else '' end as Day40    \r\n" +

                                    ",case when CalendarDate = @StartDate + 40 and Workingday = 'N' then 'OFF' else '' end as Day41    \r\n" +
                                    ",case when CalendarDate = @StartDate + 41 and Workingday = 'N' then 'OFF' else '' end as Day42    \r\n" +
                                    ",case when CalendarDate = @StartDate + 42 and Workingday = 'N' then 'OFF' else '' end as Day43    \r\n" +
                                    ",case when CalendarDate = @StartDate + 43 and Workingday = 'N' then 'OFF' else '' end as Day44    \r\n" +
                                    ",case when CalendarDate = @StartDate + 44 and Workingday = 'N' then 'OFF' else '' end as Day45    \r\n" +
                                    ",case when CalendarDate = @StartDate + 45 and Workingday = 'N' then 'OFF' else '' end as Day46    \r\n" +
                                    ",case when CalendarDate = @StartDate + 46 and Workingday = 'N' then 'OFF' else '' end as Day47    \r\n" +
                                    ",case when CalendarDate = @StartDate + 47 and Workingday = 'N' then 'OFF' else '' end as Day48    \r\n" +
                                    ",case when CalendarDate = @StartDate + 48 and Workingday = 'N' then 'OFF' else '' end as Day49    \r\n" +
                                    ",case when CalendarDate = @StartDate + 49 and Workingday = 'N' then 'OFF' else '' end as Day50    \r\n" +

                                    "from(    \r\n" +

                                    "Select * from[dbo].[tbl_Code_Calendar_Type] where CalendarCode = @CalCode and CalendarDate >= @StartDate and CalendarDate <= @EndDate)a)a  \r\n" +

                                    "union     \r\n" +

                                    "Select @StartDate StartDate,@EndDate EndDate,'SQM' Cycle    \r\n" +

                                    ", '' Day1, '' Day2, '' Day3, '' Day4, ''Day5    \r\n" +
                                    ", '' Day6, '' Day7, '' Day8, '' Day9, '' Day10    \r\n" +

                                    ", '' Day11, '' Day12, '' Day13, '' Day14, '' Day15    \r\n" +
                                    ", '' Day16, '' Day17, '' Day18, '' Day19, '' Day20    \r\n" +

                                    ", '' Day21, '' Day22, '' Day23, '' Day24, '' Day25    \r\n" +
                                    ", '' Day26, '' Day27, '' Day28, '' Day29, '' Day30    \r\n" +

                                    ", '' Day31, '' Day32, '' Day33, '' Day34, '' Day35    \r\n" +
                                    ", '' Day36, '' Day37, '' Day38, '' Day39, '' Day40    \r\n"+

                                    ", '' Day41, '' Day42, '' Day43, '' Day44, '' Day45    \r\n" +
                                    ", '' Day46, '' Day47, '' Day48, '' Day49, '' Day50    \r\n" +

                                    " ";

            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            DataTable dt = _dbManWP.ResultsDataTable;

            Startdate.Value = Convert.ToDateTime( dt.Rows[0][0].ToString() );

            gcMoCycle.DataSource = dt;

            colCycName.FieldName = "CycleName";

            colCycDay1.FieldName = "Day1";
            colCycDay2.FieldName = "Day2";
            colCycDay3.FieldName = "Day3";
            colCycDay4.FieldName = "Day4";
            colCycDay5.FieldName = "Day5";
            colCycDay6.FieldName = "Day6";
            colCycDay7.FieldName = "Day7";
            colCycDay8.FieldName = "Day8";
            colCycDay9.FieldName = "Day9";
            colCycDay10.FieldName = "Day10";

            colCycDay11.FieldName = "Day11";
            colCycDay12.FieldName = "Day12";
            colCycDay13.FieldName = "Day13";
            colCycDay14.FieldName = "Day14";
            colCycDay15.FieldName = "Day15";
            colCycDay16.FieldName = "Day16";
            colCycDay17.FieldName = "Day17";
            colCycDay18.FieldName = "Day18";
            colCycDay19.FieldName = "Day19";
            colCycDay20.FieldName = "Day20";

            colCycDay21.FieldName = "Day21";
            colCycDay22.FieldName = "Day22";
            colCycDay23.FieldName = "Day23";
            colCycDay24.FieldName = "Day24";
            colCycDay25.FieldName = "Day25";
            colCycDay26.FieldName = "Day26";
            colCycDay27.FieldName = "Day27";
            colCycDay28.FieldName = "Day28";
            colCycDay29.FieldName = "Day29";
            colCycDay30.FieldName = "Day30";

            colCycDay31.FieldName = "Day31";
            colCycDay32.FieldName = "Day32";
            colCycDay33.FieldName = "Day33";
            colCycDay34.FieldName = "Day34";
            colCycDay35.FieldName = "Day35";
            colCycDay36.FieldName = "Day36";
            colCycDay37.FieldName = "Day37";
            colCycDay38.FieldName = "Day38";
            colCycDay39.FieldName = "Day39";
            colCycDay40.FieldName = "Day40";

            colCycDay41.FieldName = "Day41";
            colCycDay42.FieldName = "Day42";
            colCycDay43.FieldName = "Day43";
            colCycDay44.FieldName = "Day44";
            colCycDay45.FieldName = "Day45";
            colCycDay46.FieldName = "Day46";
            colCycDay47.FieldName = "Day47";
            colCycDay48.FieldName = "Day48";
            colCycDay49.FieldName = "Day49";
            colCycDay50.FieldName = "Day50";


            DateTime _StartDate = Convert.ToDateTime(dt.Rows[0][0].ToString());
            DateTime _EndDate = Convert.ToDateTime(dt.Rows[0][1].ToString());
            int columnIndex = 2;

            TimeSpan dtDays = _EndDate - _StartDate;

            int NumberOfDays = Convert.ToInt32( dtDays.TotalDays);

            lblEndColumns.Text = NumberOfDays.ToString();

            for (int i = 0; i < 50; i++)
            {

                //if (columnIndex == NumberOfDays +2)
                //{
                //    gvMoCycle.Columns[columnIndex].Visible = false;
                //    return;
                //}

                string test = gvMoCycle.Columns[columnIndex].Caption;

                gvMoCycle.Columns[columnIndex].Caption = Convert.ToDateTime(_StartDate).ToString("dd MMM ddd");

                _StartDate = _StartDate.AddDays(1);
                columnIndex++;
            }

            //return;

            MWDataManager.clsDataAccess _dbManCheckPlan = new MWDataManager.clsDataAccess();
            _dbManCheckPlan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManCheckPlan.SqlStatement = "Select * from tbl_Planning_Vamping \r\n"+
                                           "where Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "' and sectionID = '" + ExtractBeforeColon(SecNamelbl.Text) + "'  \r\n"+
                                           "and WorkplaceID = '"+ ExtractBeforeColon(TotWpList.SelectedItem.ToString()) + "'  ";

            _dbManCheckPlan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCheckPlan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCheckPlan.ExecuteInstruction();

            DataTable dtCheckPlan = _dbManCheckPlan.ResultsDataTable;

            columnIndex = 2;


            foreach (DataRow dr in dtCheckPlan.Rows)
            {
                string Headerdate = Convert.ToDateTime(gvMoCycle.Columns[columnIndex].Caption.Substring(0,7)).ToString();

                if (dr["Calendardate"].ToString() == Headerdate && dr["WorkingDay"].ToString() == "Y")
                {
                    gvMoCycle.SetRowCellValue(0, gvMoCycle.Columns[columnIndex], dr["PlanActivity"].ToString());
                }
                columnIndex++;
            }

            int Count = 0;

            if (dtCheckPlan.Rows.Count == 0)
            {
                for (int i = 2; i < gvMoCycle.Columns.Count-3; i++)
                {
                    string value = gvMoCycle.GetRowCellValue(0, gvMoCycle.Columns[i]).ToString();

                    if ( i == Convert.ToInt32( lblEndColumns.Text)+3)
                    {
                        break;
                    }

                    if (value.Trim() != "OFF")
                    {
                        if (Count == 3)
                        {
                            gvMoCycle.SetRowCellValue(0, gvMoCycle.Columns[i],"PB");
                            Count = 0;
                        }
                        else
                        {
                            gvMoCycle.SetRowCellValue(0, gvMoCycle.Columns[i], "BR");
                            Count = Count + 1;
                        }
                    }                    
                }
               
            }

            CalcSQM();
           
        }

        private void gcMoCycle_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.gcMoCycle.PointToClient(new Point(e.X, e.Y));
            int row = gvMoCycle.CalcHitInfo(p.X, p.Y).RowHandle;
            if (row > -1)
            {
                if (gvMoCycle.FocusedColumn.Name.ToString() == "FL" || gvMoCycle.GetRowCellValue(row, gvMoCycle.CalcHitInfo(p.X, p.Y).Column.FieldName).ToString() == "OFF")
                {
                    return;
                }
                else if (gvMoCycle.CalcHitInfo(p.X, p.Y).Column.FieldName != null)
                {
                    this.gvMoCycle.SetRowCellValue(row, gvMoCycle.CalcHitInfo(p.X, p.Y).Column.FieldName, lblCode.Text);
                    CalcSQM();
                }
            }
        }

        private void gcMoCycle_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void gcMoCycle_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void gvMoCycle_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (gvMoCycle.FocusedColumn.FieldName.ToString() == "CycleName" || gvMoCycle.GetRowCellValue(gvMoCycle.FocusedRowHandle, gvMoCycle.FocusedColumn).ToString() == "OFF")
            {
                return;
            }
            else if (lblCode.Text != "None" && lblCode.Text != "Code")
            {
                gvMoCycle.SetRowCellValue(gvMoCycle.FocusedRowHandle, gvMoCycle.FocusedColumn, lblCode.Text);
                CalcSQM();
            }
        }

        private void CalcSQM()
        {
            PlanThisMonthTxt.Text = "0";

            for (int i = 2; i < gvMoCycle.Columns.Count - 1; i++)
            {
                //PlanThisMonthTxt.Text = "0";

                //DataGridViewColumn column = OtherCycleGrid.Columns[i];

                if (gvMoCycle.Columns[i].Visible == true)
                {

                    //OtherCycleGrid.Rows[3].Cells[i].Value = "";

                    gvMoCycle.SetRowCellValue(1, gvMoCycle.Columns[i],"");

                    if (i < 52)
                    {
                        string ValueOfCell = gvMoCycle.GetRowCellValue(0, gvMoCycle.Columns[i]).ToString();

                        if (ValueOfCell != null)

                        {

                            if (ValueOfCell == "BR" || ValueOfCell == "VPB")
                            {
                                decimal SQM = Math.Floor( Convert.ToDecimal(lblVampSQM.Text) );

                                gvMoCycle.SetRowCellValue(1, gvMoCycle.Columns[i], SQM);
                                //OtherCycleGrid.Rows[3].Cells[i].Value = lblVampSQM.Text;



                                PlanThisMonthTxt.Text = (Convert.ToInt32(PlanThisMonthTxt.Text) + Math.Floor(Convert.ToDecimal(lblVampSQM.Text) ) ).ToString();



                                RemText.Text = (Convert.ToInt32(VampSqmEdit.Text) - Convert.ToInt32(PlanThisMonthTxt.Text)).ToString();

                                //OtherCycleGrid.Rows[3].Cells[i].Style.ForeColor = Color.Black;

                                RemText.ForeColor = Color.DarkGray;

                                if (Convert.ToInt32(RemText.Text) < 0)
                                {
                                    if (lblColorRedSqm.Text == "ColorRedSqm")
                                    {
                                        lblColorRedSqm.Text = Convert.ToString(i);
                                    }

                                    
                                    //OtherCycleGrid.Rows[3].Cells[i].Style.ForeColor = Color.Red;

                                    RemText.ForeColor = Color.IndianRed;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Savebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvMoCycle.RowCount == 0)
            {
                MessageBox.Show("No Cycle found ,can't save cycle", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string Workplaceid = ExtractBeforeColon(TotWpList.SelectedItem.ToString());

            if (SectionEdit.EditValue.ToString() == "")
            {

                MessageBox.Show("Please select a section.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

           

            string section = ExtractBeforeColon(SecNamelbl.Text);

            string bhole = ExtractBeforeColon(BoxHolelbl.Text);

            string OreflowID = "";


            MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
            _dbManSave.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSave.SqlStatement = " select '' a ";

            int zz = 0;

            int sqm = 0;
            Decimal tons = 0;
            Decimal content = 0;

            int shiftday = 0;
            string DefaultCycle = "";
            string DefaultMO = "";

            string DefaultCycleNum = "";
            string DefaultMONum = "";


            for (int i = 2; i < 52; i++)
            {

                if (gvMoCycle.Columns[i].Visible == true)
                {
                    string WDay = "Y";

                    if (gvMoCycle.GetRowCellValue(0,gvMoCycle.Columns[i]).ToString() != "OFF")
                    {
                        shiftday = shiftday + 1;
                    }
                    else
                        WDay = "N";

                    // create string
                    if (gvMoCycle.GetRowCellValue(0, gvMoCycle.Columns[i]).ToString() != "OFF")
                    {
                        //if (OtherCycleGrid[i, 0].Style.BackColor != Color.FromArgb(Convert.ToInt32(ColorB.Text)))
                        //{
                            DefaultCycle = DefaultCycle + (gvMoCycle.GetRowCellValue(0, gvMoCycle.Columns[i]).ToString() + "         ").Substring(0, 4);
                            //DefaultCycleNum = DefaultCycleNum + (OtherCycleGrid.Rows[1].Cells[i].Value.ToString() + "         ").Substring(0, 4);
                        //}
                        //else
                        //{
                        //    DefaultCycle = DefaultCycle + ("ZZ         ").Substring(0, 4);
                        //    // DefaultCycleNum = DefaultCycleNum + ("           ").Substring(0, 4);

                        //}

                    }
                    else
                    {
                        DefaultCycle = DefaultCycle + ("OFF" + "         ").Substring(0, 4);
                        //DefaultCycleNum = DefaultCycleNum + ("           ").Substring(0, 4);
                    }

                    if (gvMoCycle.GetRowCellValue(0, gvMoCycle.Columns[i]).ToString() != "OFF")
                    {
                        //if (OtherCycleGrid[i, 0].Style.BackColor != Color.FromArgb(Convert.ToInt32(ColorB.Text)))
                        //{
                            DefaultMO = DefaultMO + (gvMoCycle.GetRowCellValue(0, gvMoCycle.Columns[i]).ToString() + "         ").Substring(0, 4);
                            //DefaultMONum = DefaultMONum + (OtherCycleGrid.Rows[4].Cells[i].Value.ToString() + "         ").Substring(0, 4);
                        //}
                        //else
                        //{
                        //    DefaultMO = DefaultMO + ("ZZ         ").Substring(0, 4);
                        //    // DefaultMONum = DefaultMONum + ("            ").Substring(0, 4);


                        //}

                    }
                    else
                    {
                        DefaultMO = DefaultMO + ("OFF" + "         ").Substring(0, 4);
                        //DefaultMONum = DefaultMONum + ("            ").Substring(0, 4);
                    }

                    string mocycle = "";
                    if (gvMoCycle.GetRowCellValue(0, gvMoCycle.Columns[i]).ToString() != "OFF")
                        mocycle = gvMoCycle.GetRowCellValue(0, gvMoCycle.Columns[i]).ToString();
                    else
                        mocycle = "";

                    int sqm1 = 0;

                    if (gvMoCycle.GetRowCellValue(1, gvMoCycle.Columns[i]).ToString() != "OFF")
                        if (gvMoCycle.GetRowCellValue(1, gvMoCycle.Columns[i]).ToString() != "")
                            sqm1 = Convert.ToInt32(gvMoCycle.GetRowCellValue(1, gvMoCycle.Columns[i]).ToString());

                    decimal plantons = sqm1 / Convert.ToDecimal(VampSqmEdit.Text) * Convert.ToDecimal(SwpTonsEdit.Text);


                    decimal plancont = (sqm1 / Convert.ToDecimal(VampSqmEdit.Text) * Convert.ToDecimal(SwpTonsEdit.Text)) * Convert.ToDecimal(Gradetxt.Text);


                    decimal planfact = Convert.ToDecimal(SwpTonsEdit.Text) / Convert.ToDecimal(VampSqmEdit.Text);


                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + "BEGIN TRY insert into tbl_PLANNING_Vamping values ";
                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + "('" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "', '" + Workplaceid + "', '" + section + "', '" + String.Format("{0:yyyy-MM-dd}", Startdate.Value.AddDays(zz)) + "', ";
                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + "'" + shiftday + "', '" + WDay + "', '" + OrgDlbl.Text + "','" + bhole + "','','" + mocycle + "', 0,'" + sqm1 + "','" + plantons + "','" + Gradetxt.Text + "','" + plancont + "','" + planfact + "','" + planfact + "', '' , null, null, null, null, null, null, null, null, '', '', '') ";
                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + "END TRY ";
                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + "BEGIN CATCH ";
                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + "     update tbl_PLANNING_Vamping set ";
                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + "     Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "', SectionID = '" + section + "', ShiftDay = '" + shiftday + "', WorkingDay = '" + WDay + "' ";
                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + ", OrgUnitDS = '" + OrgDlbl.Text + "', BH = '" + bhole + "',PlanActivity = '" + mocycle + "', planadv = 0, plansqm = '" + sqm1 + "', PlanTons = '" + plantons + "' ";
                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + ",PlanGrade = '" + Gradetxt.Text + "', PlanContent = '" + plancont + "', plansqmfact = '" + planfact + "', plantonfact = '" + planfact + "', auth = null, authdate = null ";
                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + " where WorkplaceID = '" + Workplaceid + "' and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", Startdate.Value.AddDays(zz)) + "' ";

                    _dbManSave.SqlStatement = _dbManSave.SqlStatement + "END CATCH ";                    


                    zz = zz + 1;
                }


            }

            _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSave.ExecuteInstruction();


            ///Insert Zeros Planmonth
            ///

            try
            {
                MWDataManager.clsDataAccess _dbManSave2 = new MWDataManager.clsDataAccess();
                _dbManSave2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManSave2.SqlStatement = " Exec sp_Insert_Zeros_PlanMonth_Vamping '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "','" + section + "', '" + Workplaceid + "',  '0' ";

                _dbManSave2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave2.ExecuteInstruction();
            }
            catch
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Cycle saved", Color.CornflowerBlue);
                return;
            }



            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Cycle saved", Color.CornflowerBlue);
        }

        private void gvMoCycle_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (gvMoCycle.GetRowCellValue(0,e.Column).ToString() == "OFF")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }

            if (e.Column.VisibleIndex == Convert.ToInt32(lblEndColumns.Text)+2)
            {
                e.Column.Visible = false;
            }


            if (lblColorRedSqm.Text != "ColorRedSqm")
            {
                if (e.Column.VisibleIndex >= Convert.ToInt32(lblColorRedSqm.Text) && e.Column.Visible == true)
                {
                    //string test = gvMoCycle.GetRowCellValue(1, e.Column).ToString();

                    try
                    {
                        if (Convert.ToInt32(gvMoCycle.GetRowCellValue(1, e.Column)) > 0)
                        {
                            if (e.RowHandle == 1)
                            {
                                e.Appearance.ForeColor = Color.IndianRed;
                            }                            
                        }
                    }
                    catch (Exception)
                    {
                    }                   
                }
            }              

        }

        private void btnViewInspReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " select '" + SysSettings.Banner + "' Banner, convert(varchar, calendardate, 106) CalDate,  * from tbl_VampingPreInspectionSheet where workplaceid = '" + ExtractAfterColon(TotWpList.SelectedItem.ToString()) + "'  \r\n ";//and Calendardate = '" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "' \r\n ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ResultsTableName = "GradeSheet";
                _dbMan.ExecuteInstruction();

                DataTable dt = _dbMan.ResultsDataTable;


                DataSet ReportDatasetLosses = new DataSet();
                ReportDatasetLosses.Tables.Add(_dbMan.ResultsDataTable);
                theReport.RegisterData(ReportDatasetLosses);

                theReport.Load(_reportFolder + "\\VampingInspectionPrev.frx");

                // theReport.Design();

                //btnSave_Click(null, null);


                theReport.Prepare();

                theReport.ShowPrepared();
            }
            catch { return; }
        }

        private void btnAddWP_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void RCRockEngineering_Click(object sender, EventArgs e)
        {

        }

        

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
