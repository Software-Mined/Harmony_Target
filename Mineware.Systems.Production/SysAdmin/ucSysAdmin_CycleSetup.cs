using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class ucSysAdmin_CycleSetup : BaseUserControl
    {
        public ucSysAdmin_CycleSetup()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpCalendars);
            FormActiveRibbonPage = rpCalendars;
            FormMainRibbonPage = rpCalendars;
            RibbonControl = rcCalendars;
        }


        private string CodeDragDrop = "None";
        private bool IsCycleCode;
        string CycleName;
        string CycleType;

        string lblCode = "Blank";
        string lblType;

        public static string ExtractBeforeColon(string TheString)
        {
            if (TheString != string.Empty)
            {
                string BeforeColon;

                int index = TheString.IndexOf(":");

                BeforeColon = TheString.Substring(0, index);

                return BeforeColon;
            }
            else
            {
                return string.Empty;
            }
        }

        private void ucSysAdmin_CycleSetup_Load(object sender, EventArgs e)
        {
            LoadCycleCodes();
            LoadCycleNames();
            LoadMOCyclesList();
            LoadMOCycleGridExcep();
            LoadMOCycleGrid();
            LoadMOCycleLitExcep();

            if (gvCycleList.RowCount > 0)
            {
                LoadRawData();
            }
        }


        public void LoadCycleCodes()
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            theData.SqlStatement = "SELECT * FROM  tbl_Central_Code_Cycle";
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();

            DataTable dtCyleCode = theData.ResultsDataTable;

            gcCylceCodes.DataSource = dtCyleCode;
            colCodeDescription.FieldName = "Description";
            colCodesID.FieldName = "Code";
        }

        public void LoadCycleNames()
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            theData.SqlStatement = " select distinct(Name) Name,'Stoping' CycType from tbl_Cycle_RawData where Type = 'S'   \r\n" +
                                    "union   \r\n" +
                                    "select distinct(Name) Name,'Development' CycType from tbl_Cycle_RawData where Type = 'D' order by Name";
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();

            DataTable dtCyleName = theData.ResultsDataTable;

            gcCycleList.DataSource = dtCyleName;
            colCycDescription.FieldName = "Name";
            colCycTypes.FieldName = "CycType";
        }

        public void LoadMOCyclesList()
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            theData.SqlStatement = "select Distinct Name,Type from tbl_Cycle_RawData";
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();

            DataTable dtCyleName = theData.ResultsDataTable;

            gcCycleDrag.DataSource = dtCyleName;
            colDragName.FieldName = "Name";
            colDragType.FieldName = "Type";

            lbxExceptCycles.Items.Clear();
            foreach (DataRow dr in dtCyleName.Rows)
            {
                lbxExceptCycles.Items.Add(dr["Name"].ToString());
            }
        }

        public void LoadMOCycleGrid()
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            theData.SqlStatement = "select Distinct SectionID+ ':' + a.name sec,isnull(StopingCycle,'') StopingCycle,isnull(DevCycle,'') DevCycle from (Select a.sectionid, name from tbl_SECTION a, (    \r\n" +
                                    "Select sectionid, max(prodmonth)pm from tbl_SECTION where Hierarchicalid = '4' and Prodmonth = (select currentproductionmonth from tbl_sysset) group by sectionid) b where a.SectionID = b.SectionID and a.Prodmonth = b.pm) a   \r\n" +
                                    "left outer join   \r\n" +
                                    "(   \r\n" +

                                    "select a.sec,case when mo.StopingCycle is null then '' else mo.StopingCycle end as StopingCycle,   \r\n" +
                                    "case when mo.DevCycle is null then '' else mo.DevCycle end as DevCycle, c.Name from(   \r\n" +


                                    "Select distinct s3.sectionid sec, s3.Name   \r\n" +
                                    "from tbl_section s, tbl_section s2, tbl_section s3   \r\n" +
                                    "where s.reporttosectionid = s2.sectionid  and s.prodmonth = s2.prodmonth   \r\n" +
                                    "and s2.reporttosectionid = s3.sectionid     and s2.prodmonth = s3.prodmonth   \r\n" +
                                    "and s.Prodmonth = (select currentproductionmonth from tbl_sysset)   \r\n" +

                                    ") a   \r\n" +
                                    " left outer join   \r\n" +
                                    " tbl_Cycle_MOCycleConfig mo on a.sec = mo.Sectionid   \r\n" +
                                    " left outer join   \r\n" +
                                    " (select Distinct sectionid, name from tbl_section where  Hierarchicalid = 4) c   \r\n" +
                                    " on a.sec = c.sectionid  where c.Name is not null   \r\n" +

                                    " ) b on a.SectionID = b.sec  ";

            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();

            DataTable dtMoConfig = theData.ResultsDataTable;

            gcMoCycleCOnfig.DataSource = dtMoConfig;
            colMoConfigSec.FieldName = "sec";
            colMoConfigStope.FieldName = "StopingCycle";
            colMoConfigDev.FieldName = "DevCycle";
        }

        public void LoadMOCycleGridExcep()
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            theData.SqlStatement = "select c.Workplaceid, c.Cycle,w.Description,w.Workplaceid +':'+ w.Description Wp from tbl_Cycle_WPExceptions c    \r\n" +
            " left outer join tbl_WORKPLACE w    \r\n" +
            " on c.Workplaceid = w.WorkplaceID    \r\n" +
            " order by c.workplaceid";
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();

            DataTable dtWPExceptions = theData.ResultsDataTable;

            gcWPExceptions.DataSource = dtWPExceptions;
            colWPName.FieldName = "Wp";
            colWPCycle.FieldName = "Cycle";
        }

        public void LoadMOCycleLitExcep()
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            theData.SqlStatement = "Select workplaceid+':'+Description NewWP from tbl_WORKPLACE where  workplaceid not in (Select workplaceid from tbl_Cycle_WPExceptions)  ";
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();

            DataTable dtWPExceptions = theData.ResultsDataTable;

            foreach (DataRow dr in dtWPExceptions.Rows)
            {
                lbxWpExceptions.Items.Add(dr["NewWP"].ToString());
            }
        }

        private void gvCycleList_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            LoadRawData();
        }



        private void LoadRawData()
        {
            CycleName = gvCycleList.GetRowCellValue(gvCycleList.FocusedRowHandle, gvCycleList.Columns["Name"]).ToString();
            CycleType = gvCycleList.GetRowCellValue(gvCycleList.FocusedRowHandle, gvCycleList.Columns["CycType"]).ToString();

            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (CycleType == "Stoping")
            {
                theData.SqlStatement = "SELECT * FROM tbl_Cycle_RawData where name = '" + CycleName + "' order by fl";

                colCycSetupEndtype.Visible = false;
                colCycSetupFL.Visible = true;
            }

            if (CycleType == "Development")
            {
                theData.SqlStatement = "select a.*,CONVERT(varchar(5),e.endtypeid) + ':' + e.description Endtypeaa from dbo.tbl_Code_EndType e left outer join  \r\n" +
                                       "(select * from dbo.tbl_Cycle_RawData where Name = '" + CycleName.ToString() + "') a on e.EndTypeID = a.fl  \r\n" +
                                       "order by EndTypeID";

                colCycSetupEndtype.Visible = true;
                colCycSetupFL.Visible = false;
            }

            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();

            DataTable dtCycleRawData = theData.ResultsDataTable;

            gcCycleRawData.DataSource = null;
            gcCycleRawData.DataSource = dtCycleRawData;

            colCycSetupEndtype.FieldName = "Endtypeaa";
            colCycSetupAdv.FieldName = "AdvBlast";
            colCycSetupFL.FieldName = "FL";

            colCycSetupDay1.FieldName = "Day1";
            colCycSetupDay2.FieldName = "Day2";
            colCycSetupDay3.FieldName = "Day3";
            colCycSetupDay4.FieldName = "Day4";
            colCycSetupDay5.FieldName = "Day5";
            colCycSetupDay6.FieldName = "Day6";
            colCycSetupDay7.FieldName = "Day7";
            colCycSetupDay8.FieldName = "Day8";
            colCycSetupDay9.FieldName = "Day9";
            colCycSetupDay10.FieldName = "Day10";

            colCycSetupDay11.FieldName = "Day11";
            colCycSetupDay12.FieldName = "Day12";
            colCycSetupDay13.FieldName = "Day13";
            colCycSetupDay14.FieldName = "Day14";
            colCycSetupDay15.FieldName = "Day15";
            colCycSetupDay16.FieldName = "Day16";
            colCycSetupDay17.FieldName = "Day17";
            colCycSetupDay18.FieldName = "Day18";
            colCycSetupDay19.FieldName = "Day19";
            colCycSetupDay20.FieldName = "Day20";

            colCycSetupDay21.FieldName = "Day21";
            colCycSetupDay22.FieldName = "Day22";
            colCycSetupDay23.FieldName = "Day23";
            colCycSetupDay24.FieldName = "Day24";
            colCycSetupDay25.FieldName = "Day25";
        }

        private void gcCycleRawData_DragOver(object sender, DragEventArgs e)
        {
            //e.Effect = DragDropEffects.All;
        }

        private void gcCycleRawData_DragEnter(object sender, DragEventArgs e)
        {
            //e.Effect = DragDropEffects.Copy;
        }

        private void gcCycleRawData_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.gcCycleRawData.PointToClient(new Point(e.X, e.Y));
            int row = gvCycleRawData.CalcHitInfo(p.X, p.Y).RowHandle;
            if (row > -1)
            {
                if (gvCycleRawData.FocusedColumn.Name.ToString() == "FL")
                {
                    return;
                }
                else if (gvCycleRawData.CalcHitInfo(p.X, p.Y).Column.FieldName != null)
                {
                    this.gvCycleRawData.SetRowCellValue(row, gvCycleRawData.CalcHitInfo(p.X, p.Y).Column.FieldName, CodeDragDrop);
                }
            }
        }

        private void gvCycleRawData_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (gvCycleRawData.FocusedColumn.FieldName.ToString() == "FL" || gvCycleRawData.FocusedColumn.FieldName.ToString() == "Endtypeaa" || gvCycleRawData.FocusedColumn.FieldName.ToString() == "AdvBlast")
            {
                return;
            }
            else if (CodeDragDrop != "None" && CodeDragDrop != "Code")
            {
                gvCycleRawData.SetRowCellValue(gvCycleRawData.FocusedRowHandle, gvCycleRawData.FocusedColumn, CodeDragDrop);
            }
        }

        private void gvCylceCodes_MouseDown(object sender, MouseEventArgs e)
        {
            //IsCycleCode = true;
            //Point p = this.gcCycleList.PointToClient(new Point(e.X, e.Y));
            //int row = gvCylceCodes.CalcHitInfo(p.X, p.Y).RowHandle;

            //if (gvCylceCodes.RowCount == 0)
            //    return;

            //if (row > -1)
            //{
            //    string s = gvCylceCodes.GetRowCellValue(row, gvCylceCodes.Columns["Code"]).ToString();
            //    CodeDragDrop = gvCylceCodes.GetRowCellValue(row, gvCylceCodes.Columns["Code"]).ToString();
            //    //DragDropEffects dde1 = DoDragDrop(s,
            //    //    DragDropEffects.All);
            //}
        }

        private void gvCycleRawData_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (gvCycleRawData.GetRowCellValue(e.RowHandle, e.Column).ToString() == "BL" || gvCycleRawData.GetRowCellValue(e.RowHandle, e.Column).ToString() == "SR")
            {
                e.Appearance.ForeColor = Color.Tomato;
            }
        }

        private void barbtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string Type = string.Empty;
            CycleType = gvCycleList.GetRowCellValue(gvCycleList.FocusedRowHandle, gvCycleList.Columns["CycType"]).ToString();
            CycleName = gvCycleList.GetRowCellValue(gvCycleList.FocusedRowHandle, gvCycleList.Columns["Name"]).ToString();
            if (CycleType == "Stoping")
            {
                Type = "S";
            }

            if (CycleType == "Development")
            {
                Type = "D";
            }

            string SqlStatement = string.Empty;

            SqlStatement = SqlStatement + "delete from Code_Cycle_RawData  where Name = '" + CycleName + "' and Type = '" + Type + "'   \r\n\r\n";

            for (int x = 0; x < gvCycleRawData.RowCount; x++)
            {
                string FL = string.Empty;

                if (CycleType == "Stoping")
                {
                    FL = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["FL"]).ToString();
                }
                if (CycleType == "Development")
                {
                    FL = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Endtypeaa"]).ToString();
                }

                string Adv = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["AdvBlast"]).ToString();
                string Day1 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day1"]).ToString();
                string Day2 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day2"]).ToString();
                string Day3 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day3"]).ToString();
                string Day4 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day4"]).ToString();
                string Day5 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day5"]).ToString();
                string Day6 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day6"]).ToString();
                string Day7 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day7"]).ToString();
                string Day8 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day8"]).ToString();
                string Day9 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day9"]).ToString();
                string Day10 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day10"]).ToString();
                string Day11 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day11"]).ToString();
                string Day12 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day12"]).ToString();
                string Day13 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day13"]).ToString();
                string Day14 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day14"]).ToString();
                string Day15 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day15"]).ToString();
                string Day16 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day16"]).ToString();
                string Day17 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day17"]).ToString();
                string Day18 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day18"]).ToString();
                string Day19 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day19"]).ToString();
                string Day20 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day20"]).ToString();
                string Day21 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day21"]).ToString();
                string Day22 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day22"]).ToString();
                string Day23 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day23"]).ToString();
                string Day24 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day24"]).ToString();
                string Day25 = gvCycleRawData.GetRowCellValue(x, gvCycleRawData.Columns["Day25"]).ToString();

                SqlStatement = SqlStatement + " Insert into tbl_Cycle_RawData(Name,FL,AdvBlast,Type,Day1,Day2,Day3,Day4,Day5,Day6,Day7,Day8,Day9,Day10,Day11,Day12,Day13,Day14,Day15,   \r\n" +
                 " Day16,Day17,Day18,Day19,Day20,Day21,Day22,Day23,Day24,Day25 ) values(    \r\n" +
                 " '" + CycleName + "','" + FL + "','" + Adv + "' ,'" + Type + "', '" + Day1 + "', '" + Day2 + "',    \r\n" +
                 " '" + Day3 + "','" + Day4 + "','" + Day5 + "','" + Day6 + "',     \r\n" +
                 " '" + Day7 + "','" + Day8 + "','" + Day9 + "','" + Day10 + "',     \r\n" +
                 " '" + Day11 + "','" + Day12 + "','" + Day13 + "','" + Day14 + "',     \r\n" +
                 " '" + Day15 + "','" + Day16 + "','" + Day17 + "','" + Day18 + "',     \r\n" +
                 " '" + Day19 + "','" + Day20 + "','" + Day21 + "','" + Day22 + "',     \r\n" +
                 " '" + Day23 + "','" + Day24 + "','" + Day25 + "')  \r\n\r\n";

            }


            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            theData.SqlStatement = SqlStatement;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();
            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Default Cycle Saved.", Color.CornflowerBlue);
        }

        private void btnAddWPException_Click(object sender, EventArgs e)
        {
            string SqlStatement = string.Empty;

            if (lbxWpExceptions.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a workplace first");
                return;
            }

            if (lbxExceptCycles.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Cycle first");
                return;
            }

            string WPID = ExtractBeforeColon(lbxWpExceptions.SelectedItem.ToString());
            string Cycle = lbxExceptCycles.SelectedItem.ToString();


            SqlStatement = SqlStatement + " insert into tbl_Cycle_WPExceptions values('" + WPID + "','" + Cycle + "') ";

            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            theData.SqlStatement = SqlStatement;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Exception added", Color.CornflowerBlue);

            LoadMOCycleLitExcep();
        }

        private void btnDeleteWPException_Click(object sender, EventArgs e)
        {
            string SqlStatement = string.Empty;

            if (gvWPExceptions.RowCount > 0)
            {
                if (gvWPExceptions.FocusedRowHandle == -1)
                {
                    MessageBox.Show("Please select a Workplace before trying to Delete");
                    return;
                }

                string CycleWPID = gvWPExceptions.GetRowCellValue(gvWPExceptions.FocusedRowHandle, gvWPExceptions.Columns["Wp"]).ToString();


                SqlStatement = " Delete from tbl_Cycle_WPExceptions where workplaceid = '" + ExtractBeforeColon(CycleWPID) + "'  ";

                MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
                theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                theData.SqlStatement = SqlStatement;
                theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                theData.queryReturnType = MWDataManager.ReturnType.DataTable;
                theData.ExecuteInstruction();

                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Cycle deleted", Color.CornflowerBlue);

                LoadMOCycleLitExcep();
            }
        }

        private void gcCycleDrag_MouseDown(object sender, MouseEventArgs e)
        {
            if (gvCycleDrag.RowCount == 0)
                return;

            if (gvCycleDrag.FocusedRowHandle > -1)
            {
                lblCode = gvCycleDrag.GetRowCellValue(gvCycleDrag.FocusedRowHandle, gvCycleDrag.Columns["Name"]).ToString();
                //lblType = gvCycleDrag.GetRowCellValue(gvCycleDrag.FocusedRowHandle, gvCycleDrag.Columns["Type"]).ToString();
                DragDropEffects dde1 = DoDragDrop(lblCode,
                    DragDropEffects.All);
            }


        }

        private void gcMoCycleCOnfig_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.gcMoCycleCOnfig.PointToClient(new Point(e.X, e.Y));
            int row = gvMoCycleCOnfig.CalcHitInfo(p.X, p.Y).RowHandle;

            if (row > -1)
            {
                if (lblCode == "Blank")
                {
                    MessageBox.Show("Please Select a Cycle");
                    return;
                }

                string test = gvMoCycleCOnfig.CalcHitInfo(p.X, p.Y).Column.FieldName;

                if (gvMoCycleCOnfig.CalcHitInfo(p.X, p.Y).Column.FieldName == "DevCycle" && lblType == "Stope")
                {
                    MessageBox.Show("Can't apply a Stoping Cycle to Developement");
                    return;
                }

                if (gvMoCycleCOnfig.CalcHitInfo(p.X, p.Y).Column.FieldName == "StopingCycle" && lblType == "Dev")
                {
                    MessageBox.Show("Can't apply a Developement Cycle to Stoping");
                    return;
                }

                this.gvMoCycleCOnfig.SetRowCellValue(row, gvMoCycleCOnfig.CalcHitInfo(p.X, p.Y).Column.FieldName, lblCode);
                lblCode = "Blank";


                SaveMoCycleConfig();
            }
        }


        private void SaveMoCycleConfig()
        {
            string SqlStatement = string.Empty;

            SqlStatement = SqlStatement + " delete from tbl_Cycle_MOCycleConfig   \r\n\r\n";

            for (int x = 0; x <= gvMoCycleCOnfig.RowCount - 1; x++)
            {
                string MO = gvMoCycleCOnfig.GetRowCellValue(x, gvMoCycleCOnfig.Columns["sec"]).ToString();
                string StpCycle = gvMoCycleCOnfig.GetRowCellValue(x, gvMoCycleCOnfig.Columns["StopingCycle"]).ToString();
                string DevCycle = gvMoCycleCOnfig.GetRowCellValue(x, gvMoCycleCOnfig.Columns["DevCycle"]).ToString();

                SqlStatement = SqlStatement + " insert into tbl_Cycle_MOCycleConfig values('" + ExtractBeforeColon(MO) + "','" + StpCycle + "','" + DevCycle + "')   \r\n\r\n";
            }


            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            theData.SqlStatement = SqlStatement;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Cycle Applied", Color.CornflowerBlue);

            LoadMOCycleGrid();
        }

        private void gvCylceCodes_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            IsCycleCode = true;

            if (gvCylceCodes.RowCount == 0)
                return;

            if (gvCylceCodes.FocusedRowHandle > -1)
            {
                string s = gvCylceCodes.GetRowCellValue(gvCylceCodes.FocusedRowHandle, gvCylceCodes.Columns["Code"]).ToString();
                CodeDragDrop = gvCylceCodes.GetRowCellValue(gvCylceCodes.FocusedRowHandle, gvCylceCodes.Columns["Code"]).ToString();
            }
        }

        private void gvCycleDrag_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (gvCycleDrag.RowCount == 0)
                return;

            if (gvCycleDrag.FocusedRowHandle > -1)
            {
                lblCode = gvCycleDrag.GetRowCellValue(gvCycleDrag.FocusedRowHandle, gvCycleDrag.Columns["Name"]).ToString();
                lblType = gvCycleDrag.GetRowCellValue(gvCycleDrag.FocusedRowHandle, gvCycleDrag.Columns["Type"]).ToString();
            }
        }

        private void gcMoCycleCOnfig_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void gcMoCycleCOnfig_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
