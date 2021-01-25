using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPdfViewer;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.Production.OCRScheduling.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OCRScheduling
{
    public partial class ucOCRSchedulingMain : BaseUserControl
    {
        StringBuilder sb = new StringBuilder();
        clsOCRScheduling _clsOCRScheduling = new clsOCRScheduling();
        Procedures procs = new Procedures();
        private BackgroundWorker MovetoProd = new BackgroundWorker();
        private Mineware.Systems.Production.OCRScheduling.Models.FormsAPI _Forms = new Mineware.Systems.Production.OCRScheduling.Models.FormsAPI(); //PduPlessis
        private Mineware.Systems.Production.OCRScheduling.Models.PrintedForm _PrintedForm1; //PduPlessis
        Mineware.Systems.Production.OCRScheduling.Models.PrintedForm _PrintedForm = new Mineware.Systems.Production.OCRScheduling.Models.PrintedForm();
        List<ListDrop> _items = new List<ListDrop>();
        List<DataRow> list = new List<DataRow>();
        private bool MoveToProdind = false;
        private Mineware.Systems.Global.CustomControls.MWRepositoryItemProdMonth riProdmonth;
        public string DayNum = string.Empty;
        private DateTime startdate;
        public ucOCRSchedulingMain()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpOCR);
            FormActiveRibbonPage = rpOCR;
            FormMainRibbonPage = rpOCR;
            RibbonControl = rcOCR;
        }

        private void MovetoProd_DoWork(object sender, DoWorkEventArgs e)
        {
            MoveToProd();
        }

        private void MovetoProd_RunWorkerCompleted(object sender, DoWorkEventArgs e)
        {
            MessageBox.Show(string.Empty);
        }

        //private void ScrollToRight()
        //{
        //    DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo info = gvOCRScheduling.GetViewInfo() as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo;
        //    gvOCRScheduling.LeftCoord = info.Bounds.Right+550;
        //}

        private void ucOCRSchedulingMain_Load(object sender, EventArgs e)
        {
            

            _clsOCRScheduling.theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            SelectedPageGroup.Visible = false;
            ChecklistInfoGroup.Visible = false;

            string CurrMonth = DateTime.Now.ToString("yyyyMM");


            int ProdMonth;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("SELECT DISTINCT TOP 1 [Prodmonth] ProdMonth  FROM [SECCAL] WHERE BeginDate <= CAST(GETDATE()  AS DATE) and EndDate >= CAST(GETDATE()  AS DATE)");

            _dbMan.SqlStatement = "select case when pm1 is null then prodmonth else pm1 end as prodmonth  from (select 'a' lbl, (year(getdate())*100) + month(getdate()) prodmonth ) a \r\n" +
                                     "left outer join   \r\n" +
                                     "(SELECT DISTINCT TOP 1[Prodmonth] pm1, 'a' lbl  \r\n" +

                                     "FROM [vw_Seccal_totalShifts_Calc] WHERE BeginDate <= CAST(GETDATE()  AS DATE) and EndDate >= CAST(GETDATE()  AS DATE)) b   \r\n" +
                                     "on a.lbl = b.lbl ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable SubB = _dbMan.ResultsDataTable;

            ProdMonth = Convert.ToInt32(SubB.Rows[0]["ProdMonth"].ToString());



            try
            {
                //OCRCL.SelectedIndex = 0;
                lbcForms.SelectedIndex = 0;
            }
            catch
            {
                //return;
            }
            //layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            FillFormGroups();
            radioGroup1.SelectedIndex = 0;
            FillFormTypes();
            LoadChecklists();
            EditProdmonth.EditValue = procs.ProdMonthAsDate(ProdMonth.ToString());
            updateSections(ProdMonth);
            InsertBlankRecordsIFNoneYet();
            LoadGrid();




        }


        void InsertBlankRecordsIFNoneYet()
        {
            ///Insert Blank Record
            ///

            MWDataManager.clsDataAccess _dbManWPSTDetail2 = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail2.SqlStatement = " begin try insert into tbl_OCR_Scheduling ([MillMonth],[SectionID],[WorkplaceID],[Day1],[Day2],[Day3],[Day4],[Day5],[Day6],[Day7],[Day8],[Day9],[Day10],[Day11],[Day12],[Day13],[Day14],[Day15],[Day16],[Day17],[Day18],[Day19],[Day20],[Day21],[Day22],[Day23],[Day24],[Day25],[Day26],[Day27],[Day28],[Day29],[Day30],[Day31] )\r\n" +

                                                " Select month, a.SectionID, a.WorkplaceID  \r\n" +
                                                 " ,Day1,Day2,Day3,Day4,Day5,Day6,Day7,Day8,Day9,Day10,Day11,Day12,Day13,Day14,Day15,Day16,Day17,Day18,Day19  \r\n" +
                                                " ,Day20,Day21,Day22,Day23,Day24,Day25,Day26,Day27,Day28,Day29,Day30,Day31 from (  \r\n" +
                                                "  select '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "' month, a.SectionID, a.WorkplaceID, \r\n" +
                                                "  isnull(b.Day1, 0)Day1, isnull(b.Day2, 0)Day2, isnull(b.Day3, 0)Day3, isnull(b.Day4, 0)Day4, isnull(b.Day5, 0)Day5, isnull(b.Day6, 0)Day6, isnull(b.Day7, 0)Day7, isnull(b.Day8, 0)Day8, isnull(b.Day9, 0)Day9, isnull(b.Day10, 0)Day10,  \r\n" +
                                                "   isnull(b.Day11, 0)Day11, isnull(b.Day12, 0)Day12, isnull(b.Day13, 0)Day13, isnull(b.Day14, 0)Day14, isnull(b.Day15, 0)Day15, isnull(b.Day16, 0)Day16, isnull(b.Day17, 0)Day17, isnull(b.Day18, 0)Day18, isnull(b.Day19, 0)Day19, isnull(b.Day20, 0)Day20,  \r\n" +
                                                "   isnull(b.Day21, 0)Day21, isnull(b.Day22, 0)Day22, isnull(b.Day23, 0)Day23, isnull(b.Day24, 0)Day24, isnull(b.Day25, 0)Day25, isnull(b.Day26, 0)Day26, isnull(b.Day27, 0)Day27, isnull(b.Day28, 0)Day28, isnull(b.Day29, 0)Day29, isnull(b.Day30, 0)Day30, isnull(b.Day31, 0) Day31, b.MillMonth mm  \r\n" +
                                                "  from(  \r\n" +
                                                "  select Distinct p.Prodmonth, SectionID, p.WorkplaceID + ':' + w.Description Workplace, p.WorkplaceID  \r\n" +
                                                "  from tbl_planmonth p, tbl_workplace w \r\n" +
                                                "  where p.prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "' \r\n" +
                                                "  and p.WorkplaceID = w.WorkplaceID \r\n" +
                                                " )a \r\n" +
                                                "  left outer join( \r\n" +
                                                "  select * from tbl_OCR_Scheduling where millmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "'  )b \r\n" +
                                                "  on a.Prodmonth = b.MillMonth \r\n" +
                                                "  and a.SectionID = b.SectionID \r\n" +
                                                "  and a.WorkplaceID = b.WorkplaceID  )a \r\n" +
                                                "  Where a.month not in (select MillMonth from tbl_OCR_Scheduling) \r\n" +
                                                "  and a.SectionID not in (select SectionID from tbl_OCR_Scheduling where MillMonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "') \r\n" +
                                                 " and a.WorkplaceID not in (select WorkplaceID from tbl_OCR_Scheduling where MillMonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "') \r\n" +
                                                "  end try begin catch end catch \r\n" +




                                               "    ";

            _dbManWPSTDetail2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail2.ExecuteInstruction();
        }


        void listView1_DragEnter(object sender, DragEventArgs e)
        {

        }

        void listView1_DragDrop(object sender, DragEventArgs e)
        {

        }


        void LoadGrid()
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            ///Total Mine
            if (EditSection.EditValue.ToString() == "Total Mine")
            {
                //MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
                _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManWPSTDetail.SqlStatement = " exec sp_OCR_SchedulingGrid '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "'  \r\n" +

                                                      "   ";

                _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPSTDetail.ExecuteInstruction();
            }
            else
            {
                _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManWPSTDetail.SqlStatement = " exec sp_OCR_SchedulingGridSec '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "', '" + EditSection.EditValue.ToString() + "'  \r\n" +

                                                      "   ";

                _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPSTDetail.ExecuteInstruction();
            }


            DataTable dt1 = _dbManWPSTDetail.ResultsDataTable;

            DataSet ds1 = new DataSet();

            gcOCRScheduling.DataSource = null;

            if (ds1.Tables.Count > 0)
                ds1.Tables.Clear();

            ds1.Tables.Add(dt1);
            gcOCRScheduling.DataSource = ds1.Tables[0];
            gvOCRScheduling.ExpandAllGroups();

            colUniqueID.FieldName = "UniqueID";
            ColWPID.FieldName = "WorkplaceID";
            col1SecID.FieldName = "SectionID";
            colSect_2.FieldName = "Sectionid_2";
            colSupervisorName.FieldName = "SBName";
            col1Wp.FieldName = "Workplace";
            Col1Day1.FieldName = "Day1";
            Col1Day2.FieldName = "Day2";
            Col1Day3.FieldName = "Day3";
            Col1Day4.FieldName = "Day4";
            Col1Day5.FieldName = "Day5";
            Col1Day6.FieldName = "Day6";
            Col1Day7.FieldName = "Day7";
            Col1Day8.FieldName = "Day8";
            Col1Day9.FieldName = "Day9";
            Col1Day10.FieldName = "Day10";
            Col1Day11.FieldName = "Day11";
            Col1Day12.FieldName = "Day12";
            Col1Day13.FieldName = "Day13";
            Col1Day14.FieldName = "Day14";
            Col1Day15.FieldName = "Day15";
            Col1Day16.FieldName = "Day16";
            Col1Day17.FieldName = "Day17";
            Col1Day18.FieldName = "Day18";
            Col1Day19.FieldName = "Day19";
            Col1Day20.FieldName = "Day20";
            Col1Day21.FieldName = "Day21";
            Col1Day22.FieldName = "Day22";
            Col1Day23.FieldName = "Day23";
            Col1Day24.FieldName = "Day24";
            Col1Day25.FieldName = "Day25";
            Col1Day26.FieldName = "Day26";
            Col1Day27.FieldName = "Day27";
            Col1Day28.FieldName = "Day28";
            Col1Day29.FieldName = "Day29";
            Col1Day30.FieldName = "Day30";
            Col1Day31.FieldName = "Day31";


            Day1Cycle.FieldName = "DAY1CYCLE";
            Day2Cycle.FieldName = "DAY2CYCLE";
            Day3Cycle.FieldName = "DAY3CYCLE";
            Day4Cycle.FieldName = "DAY4CYCLE";
            Day5Cycle.FieldName = "DAY5CYCLE";
            Day6Cycle.FieldName = "DAY6CYCLE";
            Day7Cycle.FieldName = "DAY7CYCLE";
            Day8Cycle.FieldName = "DAY8CYCLE";
            Day9Cycle.FieldName = "DAY9CYCLE";
            Day10Cycle.FieldName = "DAY10CYCLE";
            Day11Cycle.FieldName = "DAY11CYCLE";
            Day12Cycle.FieldName = "DAY12CYCLE";
            Day13Cycle.FieldName = "DAY13CYCLE";
            Day14Cycle.FieldName = "DAY14CYCLE";
            Day15Cycle.FieldName = "DAY15CYCLE";
            Day16Cycle.FieldName = "DAY16CYCLE";
            Day17Cycle.FieldName = "DAY17CYCLE";
            Day18Cycle.FieldName = "DAY18CYCLE";
            Day19Cycle.FieldName = "DAY19CYCLE";
            Day20Cycle.FieldName = "DAY20CYCLE";
            Day21Cycle.FieldName = "DAY21CYCLE";
            Day22Cycle.FieldName = "DAY22CYCLE";
            Day23Cycle.FieldName = "DAY23CYCLE";
            Day24Cycle.FieldName = "DAY24CYCLE";
            Day25Cycle.FieldName = "DAY25CYCLE";
            Day26Cycle.FieldName = "DAY26CYCLE";
            Day27Cycle.FieldName = "DAY27CYCLE";
            Day28Cycle.FieldName = "DAY28CYCLE";
            Day29Cycle.FieldName = "DAY29CYCLE";
            Day30Cycle.FieldName = "DAY30CYCLE";
            Day31Cycle.FieldName = "DAY31CYCLE";







            ///Load Headers and off days
            ///
            MWDataManager.clsDataAccess _dbManWPST1 = new MWDataManager.clsDataAccess();
            _dbManWPST1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPST1.SqlStatement = "  " +

                                        "  declare @NO1 datetime  \r\n" +
                                        " declare @NO2 datetime  \r\n" +
                                        " declare @NO3 datetime  \r\n" +
                                        " declare @NO4 datetime  \r\n" +
                                        " declare @NO5 datetime  \r\n" +
                                        " declare @NO6 datetime  \r\n" +
                                        " declare @NO7 datetime  \r\n" +
                                        " declare @NO8 datetime  \r\n" +
                                        " declare @NO9 datetime  \r\n" +
                                        " declare @NO10 datetime  \r\n" +
                                        " declare @NO11 datetime  \r\n" +
                                        " declare @NO12 datetime  \r\n" +
                                        " declare @NO13 datetime  \r\n" +
                                        " declare @NO14 datetime  \r\n" +

                                        "declare @NO15 datetime  \r\n" +
                                        "declare @NO16 datetime  \r\n" +
                                        "declare @NO17 datetime  \r\n" +
                                        "declare @NO18 datetime  \r\n" +
                                        "declare @NO19 datetime  \r\n" +
                                        "declare @NO20 datetime  \r\n" +
                                        "declare @NO21 datetime  \r\n" +

                                        "declare @NO22 datetime  \r\n" +
                                        "declare @NO23 datetime  \r\n" +
                                        "declare @NO24 datetime  \r\n" +
                                        "declare @NO25 datetime  \r\n" +
                                        "declare @NO26 datetime  \r\n" +
                                        "declare @NO27 datetime  \r\n" +
                                        "declare @NO28 datetime  \r\n" +
                                         "declare @NO29 datetime  \r\n" +
                                           "declare @NO30 datetime  \r\n" +
                                            "declare @NO31 datetime  \r\n" +


                                        " set @NO1 = (  \r\n" +
                                        " select Startdate from tbl_CALENDARMILL where millmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "' \r\n" +
                                        " )  \r\n" +

                                        " set @NO2 = @NO1+ 1  \r\n" +
                                        " set @NO3 = @NO2+ 1  \r\n" +
                                        " set @NO4 = @NO3+ 1  \r\n" +
                                        " set @NO5 = @NO4+ 1  \r\n" +
                                        " set @NO6 = @NO5+ 1  \r\n" +
                                        " set @NO7 = @NO6+ 1  \r\n" +
                                        " set @NO8 = @NO7+ 1  \r\n" +
                                        " set @NO9 = @NO8+ 1  \r\n" +
                                        " set @NO10 = @NO9+ 1  \r\n" +
                                        " set @NO11 = @NO10+ 1  \r\n" +
                                        " set @NO12 = @NO11+ 1  \r\n" +
                                        " set @NO13 = @NO12+ 1  \r\n" +
                                        " set @NO14 = @NO13+ 1  \r\n" +


                                        "set @NO15 = @NO13+ 2  \r\n" +
                                        "set @NO16 = @NO13+ 3  \r\n" +
                                        "set @NO17 = @NO13+ 4  \r\n" +
                                        "set @NO18 = @NO13+ 5  \r\n" +
                                        "set @NO19 = @NO13+ 6  \r\n" +
                                        "set @NO20 = @NO13+ 7  \r\n" +
                                        "set @NO21 = @NO13+ 8  \r\n" +

                                        "set @NO22 = @NO13+ 9  \r\n" +
                                        "set @NO23 = @NO13+ 10  \r\n" +
                                        "set @NO24 = @NO13+ 11  \r\n" +
                                        "set @NO25 = @NO13+ 12  \r\n" +
                                        "set @NO26 = @NO13+ 13  \r\n" +
                                        "set @NO27 = @NO13+ 14  \r\n" +
                                        "set @NO28 = @NO13+ 15  \r\n" +
                                         "set @NO29 = @NO13+ 16  \r\n" +
                                         "set @NO30 = @NO13+ 17  \r\n" +
                                          "set @NO31 = @NO13+ 18  \r\n" +


                                        " select @NO1 dd, '   ' +substring(datename(dw,@NO1),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO1,106),1,6) a,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO2),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO2,106),1,6) b,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO3),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO3,106),1,6) c,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO4),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO4,106),1,6) d,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO5),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO5,106),1,6) e,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO6),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO6,106),1,6) f,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO7),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO7,106),1,6) g,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO8),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO8,106),1,6) h,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO9),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO9,106),1,6) i,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO10),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO10,106),1,6) j,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO11),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO11,106),1,6) k,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO12),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO12,106),1,6) l,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO13),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO13,106),1,6) m,  \r\n" +
                                        " '   ' +substring(datename(dw,@NO14),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO14,106),1,6) n,  \r\n" +

            " '   ' +substring(datename(dw,@NO15),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO15,106),1,6) o,  \r\n" +
            " '   ' +substring(datename(dw,@NO16),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO16,106),1,6) p,  \r\n" +
            " '   ' +substring(datename(dw,@NO17),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO17,106),1,6) q,  \r\n" +
            " '   ' +substring(datename(dw,@NO18),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO18,106),1,6) r,  \r\n" +
            " '   ' +substring(datename(dw,@NO19),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO19,106),1,6) s,  \r\n" +
            " '   ' +substring(datename(dw,@NO20),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO20,106),1,6) t,  \r\n" +
            " '   ' +substring(datename(dw,@NO21),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO21,106),1,6) u,  \r\n" +
            " '   ' +substring(datename(dw,@NO22),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO22,106),1,6) v,  \r\n" +
            " '   ' +substring(datename(dw,@NO23),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO23,106),1,6) w,  \r\n" +
            " '   ' +substring(datename(dw,@NO24),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO24,106),1,6) x,  \r\n" +
            " '   ' +substring(datename(dw,@NO25),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO25,106),1,6) y,  \r\n" +
            " '   ' +substring(datename(dw,@NO26),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO26,106),1,6) z,  \r\n" +
            " '   ' +substring(datename(dw,@NO27),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO27,106),1,6) za,  \r\n" +
            " '   ' +substring(datename(dw,@NO28),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO28,106),1,6) zb, " +
            " '   ' +substring(datename(dw,@NO29),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO29,106),1,6) zc,  \r\n" +
            " '   ' +substring(datename(dw,@NO30),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO30,106),1,6) zd,  \r\n" +
            " '   ' +substring(datename(dw,@NO31),1,3) + '   '+ substring(CONVERT(VARCHAR(11),@NO31,106),1,6) ze  \r\n" +
            "  \r\n";



            _dbManWPST1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPST1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPST1.ExecuteInstruction();

            DataTable dt2 = _dbManWPST1.ResultsDataTable;

            DataSet ds2 = new DataSet();

            if (ds2.Tables.Count > 0)
                ds2.Tables.Clear();

            ds2.Tables.Add(dt2);
            try
            {
                startdate = (DateTime)dt2.Rows[0]["dd"];
            }
            catch
            {
                //MessageBox.Show("No Mill Calendar for selected Period");
                //return;
            }

            Col1Day1.Caption = dt2.Rows[0]["a"].ToString();
            Col1Day2.Caption = dt2.Rows[0]["b"].ToString();
            Col1Day3.Caption = dt2.Rows[0]["c"].ToString();
            Col1Day4.Caption = dt2.Rows[0]["d"].ToString();
            Col1Day5.Caption = dt2.Rows[0]["e"].ToString();
            Col1Day6.Caption = dt2.Rows[0]["f"].ToString();
            Col1Day7.Caption = dt2.Rows[0]["g"].ToString();
            Col1Day8.Caption = dt2.Rows[0]["h"].ToString();
            Col1Day9.Caption = dt2.Rows[0]["i"].ToString();
            Col1Day10.Caption = dt2.Rows[0]["j"].ToString();
            Col1Day11.Caption = dt2.Rows[0]["k"].ToString();
            Col1Day12.Caption = dt2.Rows[0]["l"].ToString();
            Col1Day13.Caption = dt2.Rows[0]["m"].ToString();
            Col1Day14.Caption = dt2.Rows[0]["n"].ToString();

            Col1Day15.Caption = dt2.Rows[0]["o"].ToString();
            Col1Day16.Caption = dt2.Rows[0]["p"].ToString();
            Col1Day17.Caption = dt2.Rows[0]["q"].ToString();
            Col1Day18.Caption = dt2.Rows[0]["r"].ToString();
            Col1Day19.Caption = dt2.Rows[0]["s"].ToString();
            Col1Day20.Caption = dt2.Rows[0]["t"].ToString();
            Col1Day21.Caption = dt2.Rows[0]["u"].ToString();
            Col1Day22.Caption = dt2.Rows[0]["v"].ToString();
            Col1Day23.Caption = dt2.Rows[0]["w"].ToString();
            Col1Day24.Caption = dt2.Rows[0]["x"].ToString();
            Col1Day25.Caption = dt2.Rows[0]["y"].ToString();
            Col1Day26.Caption = dt2.Rows[0]["z"].ToString();
            Col1Day27.Caption = dt2.Rows[0]["za"].ToString();
            Col1Day28.Caption = dt2.Rows[0]["zb"].ToString();
            Col1Day29.Caption = dt2.Rows[0]["zc"].ToString();
            Col1Day30.Caption = dt2.Rows[0]["zd"].ToString();
            Col1Day31.Caption = dt2.Rows[0]["ze"].ToString();

            Col1Day1.Tag = startdate.ToString();
            Col1Day2.Tag = startdate.AddDays(1).ToString();
            Col1Day3.Tag = startdate.AddDays(2).ToString();
            Col1Day4.Tag = startdate.AddDays(3).ToString();
            Col1Day5.Tag = startdate.AddDays(4).ToString();
            Col1Day6.Tag = startdate.AddDays(5).ToString();
            Col1Day7.Tag = startdate.AddDays(6).ToString();
            Col1Day8.Tag = startdate.AddDays(7).ToString();
            Col1Day9.Tag = startdate.AddDays(8).ToString();
            Col1Day10.Tag = startdate.AddDays(9).ToString();
            Col1Day11.Tag = startdate.AddDays(10).ToString();
            Col1Day12.Tag = startdate.AddDays(11).ToString();
            Col1Day13.Tag = startdate.AddDays(12).ToString();
            Col1Day14.Tag = startdate.AddDays(13).ToString();

            Col1Day15.Tag = startdate.AddDays(14).ToString();
            Col1Day16.Tag = startdate.AddDays(15).ToString();
            Col1Day17.Tag = startdate.AddDays(16).ToString();
            Col1Day18.Tag = startdate.AddDays(17).ToString();
            Col1Day19.Tag = startdate.AddDays(18).ToString();
            Col1Day20.Tag = startdate.AddDays(19).ToString();
            Col1Day21.Tag = startdate.AddDays(20).ToString();
            Col1Day22.Tag = startdate.AddDays(21).ToString();
            Col1Day23.Tag = startdate.AddDays(22).ToString();
            Col1Day24.Tag = startdate.AddDays(23).ToString();
            Col1Day25.Tag = startdate.AddDays(24).ToString();
            Col1Day26.Tag = startdate.AddDays(25).ToString();
            Col1Day27.Tag = startdate.AddDays(26).ToString();
            Col1Day28.Tag = startdate.AddDays(27).ToString();
            Col1Day29.Tag = startdate.AddDays(28).ToString();
            Col1Day30.Tag = startdate.AddDays(29).ToString();
            Col1Day31.Tag = startdate.AddDays(30).ToString();


           
            if (EditSection.EditValue.ToString() == "Other")
            {
                DialogResult result = MessageBox.Show("No Planning Exist for Month: " + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + ". Copy Planning from Previous Month?", "No Planning", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    insertPlanningOther();
                }
                else if (result == DialogResult.No)
                {
                    //no...
                }

            }



        }


        void insertMultipleChecklists(string CurrMonth, string SectionID, string Workplaceid)
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail2 = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail2.SqlStatement = " begin try insert into tbl_OCR_Scheduling \r\n" +
                                            " select '" + CurrMonth + "' month, '" + SectionID + "' SectionID, '" + Workplaceid + "' WorkplaceID,  \r\n" +
                                            " 0,  0,  0,0, 0, 0, 0, 0, 0, 0,  \r\n" +
                                            "  0, 0, 0, 0,0,0,0,0,0,0,  \r\n" +
                                            "  0, 0,0, 0,0,0,0,0,0,0,0   \r\n" +

                                               "  end try begin catch end catch SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]  ";

            _dbManWPSTDetail2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail2.ExecuteInstruction();

            AddWorkplace(_dbManWPSTDetail2.ResultsDataTable.Rows[0][0].ToString(), Workplaceid, Convert.ToDateTime(SelectedCalendardate), procs.ExtractBeforeColon(Checklist), DayNum);
            UpdateSchedule(SelectedFieldNameDate, _dbManWPSTDetail2.ResultsDataTable.Rows[0][0].ToString());
            //MessageBox.Show(_dbManWPSTDetail2.ResultsDataTable.Rows[0][0].ToString());

        }

        void LoadChecklists()
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail.SqlStatement = " SELECT Convert(Varchar(50),FormsID) +':'+ Name  Form,TypeCode  \r\n" +
                                            " FROM [dbo].[vw_OCR_Form_List] WHERE GroupName = '" + radioGroup1.EditValue.ToString() + "' \r\n" +

                                            "    ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            DataTable dt3 = _dbManWPSTDetail.ResultsDataTable;

            //OCRCL.Items.Clear();
            lbcForms.Items.Clear();
            // int dd = 1;

            for (int i = 0; i < dt3.Rows.Count; i++)

            {
                //OCRCL.Items.Add(_dbManWPSTDetail.ResultsDataTable.Rows[i][0].ToString());
                lbcForms.Items.Add(_dbManWPSTDetail.ResultsDataTable.Rows[i][0].ToString());
            }

            var myEnumerable = dt3.AsEnumerable();
            _items.Clear();
            _items =
                (from item in myEnumerable
                 select new ListDrop
                 {
                     TypeCode = item.Field<string>("TypeCode"),
                     ComplName = item.Field<string>("Form")
                 }).ToList();

        }

        public void updateSections(int prodMonth)
        {


            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            sb.Clear();
            sb.AppendLine("Select SectionID+': ' + Name Section, SectionID,'Prod' AS Type from tbl_Section ");
            sb.AppendLine("where HierarchicalID = 4 and prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "' ");
            sb.AppendLine("union ");
            sb.AppendLine("Select SectionID+': ' + Name Section, SectionID,'Other' AS Type  from tbl_SectionOther ");
            sb.AppendLine("where HierarchicalID in (2,4) and prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + "' ");
            _dbManWPSTDetail.SqlStatement = sb.ToString();
            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            clsDataResult result = _dbManWPSTDetail.ExecuteInstruction();

            if (!result.success)
            {
                XtraMessageBox.Show(result.Message, "Error");
            }

            else
            {
                if (_dbManWPSTDetail.ResultsDataTable.Rows.Count > 0)
                {
                    LookUpEditSection.DataSource = _dbManWPSTDetail.ResultsDataTable;
                    LookUpEditSection.DisplayMember = "Section";
                    LookUpEditSection.ValueMember = "SectionID";

                    EditSection.EditValue = _dbManWPSTDetail.ResultsDataTable.Rows[0]["SectionID"].ToString();

                }
            }

        }

        private void ribbonMenu_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OCRCL_DragEnter(object sender, DragEventArgs e)
        {
            //e.Effect = DragDropEffects.Copy;
        }

        private void OCRCL_DragDrop(object sender, DragEventArgs e)
        {
            //OCRCL.Items.Add(e.Data.ToString());
        }

        string Checklist = string.Empty;

        private void OCRCL_SelectedIndexChanged(object sender, EventArgs e)
        {



            //if (OCRCL.SelectedItems.Count > 0)
            //{
            //    if (OCRCL.SelectedItems[0] != null)
            //    {
            //        Checklist = OCRCL.SelectedItem.ToString();
            //    }
            //    else
            //    {
            //        Checklist = "";
            //    }
            //}

            //listView1.SelectedItems[0].SubItems[2].Text.


        }

        private void gvOCRScheduling_DoubleClick(object sender, EventArgs e)
        {
            if (gvOCRScheduling.FocusedColumn.FieldName != "Workplace")
            {
                DayNum = gvOCRScheduling.FocusedColumn.Name.Substring(gvOCRScheduling.FocusedColumn.Name.IndexOf("Day"));
                if (lbcForms.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a Checklists");
                    return;
                }
                if (InvalidDate == "Y")
                {
                    return;
                }
                string aa1 = gvOCRScheduling.GetRowCellValue(gvOCRScheduling.FocusedRowHandle, gvOCRScheduling.Columns["UniqueID"]).ToString();
                string SectionID = gvOCRScheduling.GetRowCellValue(gvOCRScheduling.FocusedRowHandle, gvOCRScheduling.Columns["SectionID"]).ToString();
                //gvOCRScheduling.SetRowCellValue(gvOCRScheduling.FocusedRowHandle, gvQuote.Columns[6], Total.ToString("0.##"));


                string aa = gvOCRScheduling.GetRowCellValue(gvOCRScheduling.FocusedRowHandle, gvOCRScheduling.FocusedColumn).ToString();
                string WorkplaceID = procs.ExtractBeforeColon(gvOCRScheduling.GetRowCellValue(gvOCRScheduling.FocusedRowHandle, gvOCRScheduling.Columns["Workplace"]).ToString());







                if (aa != "0" && aa != string.Empty)
                {

                    //gvOCRScheduling.SetRowCellValue(gvOCRScheduling.FocusedRowHandle, gvOCRScheduling.FocusedColumn, 1);
                    //gvOCRScheduling.SetRowCellValue(gvOCRScheduling.FocusedRowHandle, gvOCRScheduling.FocusedColumn, procs.ExtractBeforeColon(Checklist));
                }
                if (aa == string.Empty || aa == "0")
                {
                    AddWorkplace(aa1, WorkplaceID, Convert.ToDateTime(SelectedCalendardate), procs.ExtractBeforeColon(Checklist), DayNum);// string WPDesc, DateTime Calendardate, string ChecklistID
                                                                                                                                          //gvOCRScheduling.SetRowCellValue(gvOCRScheduling.FocusedRowHandle, gvOCRScheduling.FocusedColumn, 1);
                    UpdateSchedule(SelectedFieldNameDate, SelectedUniqueID);
                    LoadGrid();
                }

                if (aa == "1" || aa == "2")
                {
                    ///Check IF Checklist Exist
                    ///

                    MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
                    _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManWPSTDetail.SqlStatement = " select * from tbl_OCR_CheclistsAdded where UniqueID =   '" + aa1 + "' \r\n" +
                                                    "  \r\n" +
                                                    "    ";

                    _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManWPSTDetail.ExecuteInstruction();


                    if (_dbManWPSTDetail.ResultsDataTable.Rows.Count > 0)
                    {
                        insertMultipleChecklists(procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)), SectionID, WorkplaceID);
                        LoadGrid();
                        // return;
                    }
                    else
                    {

                        AddWorkplace(aa1, WorkplaceID, Convert.ToDateTime(SelectedCalendardate), procs.ExtractBeforeColon(Checklist), DayNum);
                        LoadGrid();




                    }


                }

                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Checklist Added", Checklist, Color.CornflowerBlue);
            }


        }

        //public Mineware.Systems.HarmonyPlanning.PrePlanning.PlanningDefaults PlanningClass;

        private void btnAddWorktype_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddWorkplaceOCR Propfrm = new AddWorkplaceOCR();

            Propfrm._theSystemDBTag = theSystemDBTag;
            Propfrm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            Propfrm._SectionMO = EditSection.EditValue.ToString();
            Propfrm._Month = procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue));

            Propfrm.ShowDialog();
            LoadGrid();

        }


        void UpdateSchedule(string SelectedFieldNameDate, string UniqueID)
        {
            if (Checklist != string.Empty)
            {
                MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
                _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManWPSTDetail.SqlStatement = " Exec sp_OCR_UpdateScheduleAttached  '" + SelectedFieldNameDate + "','" + UniqueID + "' \r\n" +
                                                "  \r\n" +
                                                "    ";

                _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPSTDetail.ExecuteInstruction();
            }
            else
            {
                MessageBox.Show("Please select a checklist", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }


        void UpdateSchedule2(string SelectedFieldNameDate, string UniqueID)
        {

            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail.SqlStatement = " Exec sp_OCR_UpdateSchedulePrint  '" + SelectedFieldNameDate + "','" + UniqueID + "' \r\n" +
                                            "  \r\n" +
                                            "    ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

        }


        void UpdateSchedule3(string SelectedFieldNameDate, string UniqueID)
        {

            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail.SqlStatement = " Exec sp_OCR_UpdateScheduleDelete  '" + SelectedFieldNameDate + "','" + UniqueID + "' \r\n" +
                                            "  \r\n" +
                                            "    ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

        }



        private void AddWorkplace(string UniqueID, string WPDesc, DateTime Calendardate, string ChecklistID, string DayNum)
        {
            if (ChecklistID != string.Empty)
            {
                MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
                _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManWPSTDetail.SqlStatement = " Exec sp_OCR_AddWorkplace_2  '" + UniqueID + "','" + Calendardate + "', '" + SelectedSectionID + "' , '" + SelectedWorkplaceID + "', '" + ChecklistID + "' \r\n" +
                                                " ,'" + DayNum + "' \r\n" +
                                                "    ";

                _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManWPSTDetail.ExecuteInstruction();
            }
            else
            {
                MessageBox.Show("Please select a checklist", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }





        }

        private void gcOCRScheduling_Click(object sender, EventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }
        string SelectedCalendardate = string.Empty;
        string SelectedWorkplaceID = string.Empty;
        string SelectedSectionID = string.Empty;
        string SelectedUniqueID = string.Empty;
        string SelectedFieldNameDate = string.Empty;
        string barcode1 = string.Empty;
        string SelectedWorkplace = string.Empty;
        string InvalidDate = "N";
        DateTime? DateSelected;
        private void gvOCRScheduling_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            DateSelected = null;
            try
            {
                if (Convert.ToDateTime(e.Column.Caption) <= Convert.ToDateTime(System.DateTime.Today.AddDays(-1)))
                {
                    InvalidDate = "Y";
                    //return;
                }
                else
                {
                    InvalidDate = "N";
                }
            }
            catch
            {
                return;
            }
            try
            {
                DateSelected = Convert.ToDateTime(e.Column.Tag);
            }
            catch
            {

            }

            SelectedFieldNameDate = e.Column.FieldName;
            SelectedCalendardate = e.Column.Caption;
            SelectedSectionID = gvOCRScheduling.GetRowCellValue(e.RowHandle, gvOCRScheduling.Columns["SectionID"]).ToString();
            SelectedWorkplaceID = gvOCRScheduling.GetRowCellValue(e.RowHandle, gvOCRScheduling.Columns["WorkplaceID"]).ToString();
            SelectedWorkplace = gvOCRScheduling.GetRowCellValue(e.RowHandle, gvOCRScheduling.Columns["Workplace"]).ToString();
            SelectedUniqueID = gvOCRScheduling.GetRowCellValue(e.RowHandle, gvOCRScheduling.Columns["UniqueID"]).ToString();

            DateTime Calendardate = DateTime.Today;
            try
            {
                Calendardate = Convert.ToDateTime(SelectedCalendardate);
            }
            catch
            {
                return;
            }




            //Get Selected Checklist Name for Display

            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail.SqlStatement = "  select a.*, Convert(Varchar(20),b.FormsID)+':'+b.Name Name  from ( select * from tbl_OCR_CheclistsAdded where UniqueID = '" + SelectedUniqueID + "' and calendardate = '" + Calendardate + "')a \r\n" +
                                            "  left outer join (  Select FormsID, Name from [dbo].[tbl_OCR_Forms])b on a.ChecklistID = b.FormsID \r\n" +
                                            "    ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            if (_dbManWPSTDetail.ResultsDataTable.Rows.Count > 0)
            {
                SelectedPageGroup.Visible = true;
                ChecklistInfoGroup.Visible = true;
                ChecklistSelectedItem.EditValue = _dbManWPSTDetail.ResultsDataTable.Rows[0]["Name"].ToString();
            }
            else
            {
                SelectedPageGroup.Visible = false;
                ChecklistInfoGroup.Visible = false;
                ChecklistSelectedItem.EditValue = string.Empty;
            }


            ////NEW GET PARAMTERS FOR OCR APP

            string aa = e.Column.Caption;


            barcode1 = String.Format("{0:ddMMyyyy}", DateTime.Now);
            //OCRGen.StartInfo.Arguments = clsUserInfo.UserID + wp;
            //string bb = wp.Replace(" ", "_");
            MWDataManager.clsDataAccess _dbManbarcode = new MWDataManager.clsDataAccess();
            _dbManbarcode.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManbarcode.SqlStatement = " select '" + SelectedWorkplaceID + "' + workplaceid+ '" + barcode1 + "' barcode, convert(decimal(18,0), activity) activity, workplaceid WPID from tbl_workplace where description = '" + procs.ExtractAfterColon(SelectedWorkplace) + "' ";
            _dbManbarcode.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManbarcode.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManbarcode.ResultsTableName = "Bcode";
            _dbManbarcode.ExecuteInstruction();
            DataSet ReportDatasetReportBar = new DataSet();
            ReportDatasetReportBar.Tables.Add(_dbManbarcode.ResultsDataTable);

            string activity = _dbManbarcode.ResultsDataTable.Rows[0]["activity"].ToString();
            string WPID = _dbManbarcode.ResultsDataTable.Rows[0]["WPID"].ToString();

            MWDataManager.clsDataAccess _dbManSection = new MWDataManager.clsDataAccess();
            _dbManSection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSection.SqlStatement = "  \r\n" +
           " select Distinct pm.Prodmonth,sc.SectionID, sc.Sectionid_2 SecMO from tbl_planmonth pm , Sections_Complete sc where workplaceid = '" + WPID + "' \r\n" +
                " and activity = '" + activity + "'  and pm.prodmonth = (select max(prodmonth) PM from tbl_planmonth where workplaceid = '" + SelectedWorkplaceID + "')  and pm.Prodmonth = sc.Prodmonth and pm.Sectionid = sc.SectionID \r\n";
            _dbManSection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSection.ResultsTableName = "Bcode";
            _dbManSection.ExecuteInstruction();
            string Section = string.Empty;
            string SectionMO = string.Empty;
            string ProdMonth = string.Empty;
            try
            {
                Section = _dbManSection.ResultsDataTable.Rows[0]["SectionID"].ToString();
                SectionMO = _dbManSection.ResultsDataTable.Rows[0]["SecMO"].ToString();
                ProdMonth = _dbManSection.ResultsDataTable.Rows[0]["Prodmonth"].ToString();
            }
            catch
            {
                MWDataManager.clsDataAccess _dbManSection2 = new MWDataManager.clsDataAccess();
                _dbManSection2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManSection2.SqlStatement = "  Select CurrentProductionMonth Prodmonth from tbl_sysset ";
                _dbManSection2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSection2.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSection2.ResultsTableName = "Bcode";
                _dbManSection2.ExecuteInstruction();

                Section = SelectedSectionID;
                ProdMonth = _dbManSection2.ResultsDataTable.Rows[0]["Prodmonth"].ToString();
            }
            Cursor = Cursors.WaitCursor;
            /// new
            MWDataManager.clsDataAccess _dbManWPSTDetail22 = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail22.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail22.SqlStatement = " exec sp_OCR_WorkplaceDetailAll '" + ProdMonth + "' ,'" + WPID + "', '" + Section + "', '" + activity + "' \r\n" +

            " ";
            _dbManWPSTDetail22.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail22.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail22.ExecuteInstruction();
            string bb = procs.ExtractAfterColon(SelectedWorkplace).Replace(" ", "_");
            string ActivityDesc = "Stoping";
            if (activity == "1")
                ActivityDesc = "Development";
            if (activity == "9")
                ActivityDesc = "Ledging";
            string aaa = string.Empty;



            MWDataManager.clsDataAccess _dbManWPSTDetailSB = new MWDataManager.clsDataAccess();
            _dbManWPSTDetailSB.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetailSB.SqlStatement = "   \r\n" +

                                          "    select '.' SBNAME, '.' SBEMPNO, '' aa, Sectionid_2 MOSec from Sections_Complete  \r\n" +
                                         " where sectionID = '" + Section + "'   \r\n" +
                                         " and prodmonth = (Select CurrentProductionMonth from tbl_sysset)    \r\n" +

                                            "  \r\n" +
                 " ";
            _dbManWPSTDetailSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetailSB.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetailSB.ExecuteInstruction();


            MWDataManager.clsDataAccess _dbManWPSTDetailMO = new MWDataManager.clsDataAccess();
            _dbManWPSTDetailMO.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetailMO.SqlStatement = "   \r\n" +
                                                "    select '.' MONAME, '.' MOEMPNO from Sections_Complete  \r\n" +
                                         " where sectionID = '" + Section + "'   \r\n" +
                                         " and prodmonth = (Select CurrentProductionMonth from tbl_sysset)    \r\n" +
                 " ";
            _dbManWPSTDetailMO.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetailMO.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetailMO.ExecuteInstruction();
            string blank = ".";
            try
            {
                if (SysSettings.Banner == "Joel Mine")
                {
                    aaa = "'JC' " + WPID + " " + bb + " " + activity + " " + ActivityDesc + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["aa"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["MOSec"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["SBEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["SBNAME"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO.ResultsDataTable.Rows[0]["MOEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO.ResultsDataTable.Rows[0]["MONAME"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["MOSec"].ToString() + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + blank + " " + blank + " " + blank + " " + blank + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + string.Empty;
                }
                else
                {
                    aaa = string.Empty + WPID + " " + bb + " " + activity + " " + ActivityDesc + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["aa"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["MOSec"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["SBEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["SBNAME"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO.ResultsDataTable.Rows[0]["MOEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO.ResultsDataTable.Rows[0]["MONAME"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["MOSec"].ToString() + " " + _dbManWPSTDetailSB.ResultsDataTable.Rows[0]["MOSec"].ToString() + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + blank + " " + blank + " " + blank + " " + blank + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + string.Empty;
                }
                // aaa = "" + SelectedWorkplaceID + " " + bb + " " + activity + " " + ActivityDesc + " " + _dbManWPSTDetail22.ResultsDataTable.Rows[0][4].ToString().Replace(" ", "_") + " " + _dbManWPSTDetail22.ResultsDataTable.Rows[0][5].ToString().Replace(" ", "_") + " " + _dbManWPSTDetail22.ResultsDataTable.Rows[0][6].ToString().Replace(" ", "_") + " " + _dbManWPSTDetail22.ResultsDataTable.Rows[0][7].ToString().Replace(" ", "_") + " " + _dbManWPSTDetail22.ResultsDataTable.Rows[0][8].ToString().Replace(" ", "_") + " " + _dbManWPSTDetail22.ResultsDataTable.Rows[0][9].ToString().Replace(" ", "_") + " " + SectionMO + " " + SectionMO + " " + SectionMO + "  " + SectionMO + " " + System.DateTime.Today.ToShortDateString() + "";
            }
            catch
            {
                try
                {
                    MWDataManager.clsDataAccess _dbManWPSTDetailSB2 = new MWDataManager.clsDataAccess();
                    _dbManWPSTDetailSB2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManWPSTDetailSB2.SqlStatement = " select '.' SBNAME, '.' SBEMPNO, '' aa, substring('" + Section + "',0,4) MOSec from tbl_Section  \r\n" +
                                                    " where sectionID = '" + Section + "'  \r\n" +
                                                    " and prodmonth = (Select CurrentProductionMonth from tbl_sysset)  \r\n" +
                                                    " and Hierarchicalid = 5  \r\n" +
                         " ";
                    _dbManWPSTDetailSB2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManWPSTDetailSB2.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManWPSTDetailSB2.ExecuteInstruction();


                    MWDataManager.clsDataAccess _dbManWPSTDetailMO2 = new MWDataManager.clsDataAccess();
                    _dbManWPSTDetailMO2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManWPSTDetailMO2.SqlStatement = " select '.' MONAME, '.' MOEMPNO from tbl_Section  \r\n" +
                                                    " where sectionID = substring('" + Section + "',0,4)  \r\n" +
                                                    " and prodmonth = (Select CurrentProductionMonth from tbl_sysset)  \r\n" +
                                                    " and Hierarchicalid = 4  \r\n" +
                         " ";
                    _dbManWPSTDetailMO2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManWPSTDetailMO2.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManWPSTDetailMO2.ExecuteInstruction();

                    //aaa = "" + SelectedWorkplaceID + " " + bb + " " + activity + " " + ActivityDesc + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["aa"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["MOSec"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["SBEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["SBNAME"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO2.ResultsDataTable.Rows[0]["MOEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO2.ResultsDataTable.Rows[0]["MONAME"].ToString().Replace(" ", "_") + " " + SectionMO + " " + SectionMO + " " + SectionMO + "  " + SectionMO + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + "";
                }
                catch
                {
                    MWDataManager.clsDataAccess _dbManWPSTDetailSB2 = new MWDataManager.clsDataAccess();
                    _dbManWPSTDetailSB2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManWPSTDetailSB2.SqlStatement = " select '.' SBNAME, '.' SBEMPNO, '' aa, substring('" + Section + "',0,4) MOSec from tbl_SectionOther  \r\n" +
                                                    " where sectionID = '" + Section + "'  \r\n" +
                                                    " and prodmonth = (Select CurrentProductionMonth from tbl_sysset)  \r\n" +
                                                    " and Hierarchicalid = 3  \r\n" +
                         " ";
                    _dbManWPSTDetailSB2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManWPSTDetailSB2.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManWPSTDetailSB2.ExecuteInstruction();


                    MWDataManager.clsDataAccess _dbManWPSTDetailMO2 = new MWDataManager.clsDataAccess();
                    _dbManWPSTDetailMO2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManWPSTDetailMO2.SqlStatement = " select '.' MONAME, '.' MOEMPNO from tbl_SectionOther  \r\n" +
                                                    " where sectionID = substring('" + Section + "',0,4)  \r\n" +
                                                    " and prodmonth = (Select CurrentProductionMonth from tbl_sysset)  \r\n" +
                                                    " and Hierarchicalid = 2  \r\n" +
                         " ";
                    _dbManWPSTDetailMO2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManWPSTDetailMO2.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManWPSTDetailMO2.ExecuteInstruction();

                    //if (SysSettings.Banner == "Joel Mine")
                    //{
                    //    aaa = "'JC' " + WPID + " " + bb + " " + activity + " " + ActivityDesc + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["aa"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["MOSec"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["SBEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["SBNAME"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO2.ResultsDataTable.Rows[0]["MOEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO2.ResultsDataTable.Rows[0]["MONAME"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["MOSec"].ToString() + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + blank + " " + blank + " " + blank + " " + blank + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + "";
                    //}
                    //else
                    //{
                    //    aaa = "" + WPID + " " + bb + " " + activity + " " + ActivityDesc + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["aa"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["MOSec"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["SBEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["SBNAME"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO2.ResultsDataTable.Rows[0]["MOEMPNO"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailMO2.ResultsDataTable.Rows[0]["MONAME"].ToString().Replace(" ", "_") + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["MOSec"].ToString() + " " + _dbManWPSTDetailSB2.ResultsDataTable.Rows[0]["MOSec"].ToString() + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + blank + " " + blank + " " + blank + " " + blank + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + " " + DateTime.Now.ToString("ddMMyyyy") + "";
                    //}
                }


            }
            Param = aaa;

            Cursor = Cursors.Default;
        }
        string Param = string.Empty;
        string Expaned = "Y";
       

        private void MonthItem1_EditValueChanged(object sender, EventArgs e)
        {
            // string aa = Month1.EditValue.ToString();
        }

        private void SearchTxt_TextChanged(object sender, EventArgs e)
        {

            //OCRCL.Items.Clear();
            //OCRCL.Items.AddRange(
            //        _items.Where(i => i.ComplName.Contains(SearchTxt.Text.ToString())).Select(i => i.ComplName.ToString()).ToArray());

        }

        private void SecCmb_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadGrid();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            try
            {
                _Forms = new Mineware.Systems.Production.OCRScheduling.Models.FormsAPI();
                DayNum = gvOCRScheduling.FocusedColumn.Name.Substring(gvOCRScheduling.FocusedColumn.Name.IndexOf("Day"));
                //string code = gvOCRScheduling.FocusedColumn.Name.Substring(gvOCRScheduling.FocusedColumn.Name.IndexOf("Day"));
                clsOCRScheduling _clsOCRScheduling = new clsOCRScheduling();
                //_clsOCRScheduling.theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

                MoveToProdind = true;
                GetFromInfo(procs.ExtractBeforeColon(ChecklistSelectedItem.EditValue.ToString()));
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);


                UpdateSchedule2(SelectedFieldNameDate, SelectedUniqueID);
                //_clsOCRScheduling.UpdatePrintList(_PrintedForm.PrintedFromID[0].ToString(), SelectedUniqueID, SelectedCalendardate, SelectedWorkplaceID, procs.ExtractBeforeColon(ChecklistSelectedItem.EditValue.ToString()));


            }
            catch
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
                //MessageBox.Show(@"C:\Mineware\Syncromine\OCRScheduling\OCRApp\OCRWebApiAccess.exe", "Cant Find OCR Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }




            LoadGrid();
        }

        private void OCRCL_DoubleClick(object sender, EventArgs e)
        {
            //DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            //try
            //{
            //    MoveToProdind = false;
            //    GetFromInfo(procs.ExtractBeforeColon(Checklist.ToString()));
            //    DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            //}
            //catch
            //{
            //    DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
            //    //MessageBox.Show(@"C:\Mineware\Syncromine\OCRScheduling\OCRApp\OCRWebApiAccess.exe", "Cant Find OCR Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBulkPrint Propfrm = new frmBulkPrint();

            Propfrm._theSystemDBTag = theSystemDBTag;
            Propfrm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            Propfrm.dtDateSelection.EditValue = System.DateTime.Today;
            Propfrm.UserCurrentInfo = UserCurrentInfo;
            Propfrm.ShowDialog();
        }

        private void gvOCRScheduling_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            GridView View = sender as GridView;
            string ss = string.Empty;
            if (e.Column.Caption != "Supervisor" && e.Column.Caption != "Workplace" && e.Column.Caption != string.Empty)
            {

                try
                {
                    if (Convert.ToDateTime(e.Column.Caption) == Convert.ToDateTime(System.DateTime.Today))
                    {
                        e.Appearance.BackColor = Color.LightBlue;
                    }
                }
                catch
                {
                    return;
                }
            }


            //if (e.Column.Caption != "Supervisor" && e.Column.Caption != "Workplace")
            //{
            //    if (Convert.ToDateTime(e.Column.Caption) < Convert.ToDateTime(System.DateTime.Today))
            //    {
            //        e.Appearance.BackColor = Color.NavajoWhite;
            //    }
            //}


            if (e.Column.AbsoluteIndex == 2)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY1CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 3)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY2CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 4)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY3CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 5)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY4CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 6)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY5CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 7)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY6CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 8)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY7CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 9)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY8CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 10)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY9CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 11)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY10CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 12)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY11CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 13)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY12CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 14)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY13CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 15)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY14CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 16)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY15CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 17)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY16CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 18)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY17CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 19)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY18CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 20)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY19CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 21)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY20CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 22)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY21CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 23)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY22CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 24)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY23CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 25)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY24CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 26)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY25CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 27)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY26CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 28)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY27CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 29)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY28CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 30)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY29CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }


            if (e.Column.AbsoluteIndex == 31)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY30CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            if (e.Column.AbsoluteIndex == 32)
            {
                e.DisplayText = View.GetRowCellValue(e.RowHandle, "DAY31CYCLE").ToString();
                e.Appearance.ForeColor = Color.DarkRed;

            }

            //    if (View.GetRowCellValue(e.RowHandle, "Day4Cycle").ToString() != "")
            //{
            //    if (e.Column.AbsoluteIndex == 2)
            //    {
            //        e.DisplayText = View.GetRowCellValue(e.RowHandle, "Day4Cycle").ToString();
            //        e.Appearance.ForeColor = Color.Gainsboro;
            //        if (View.GetRowCellValue(e.RowHandle, "Day4Cycle").ToString() == "BL")
            //            e.Appearance.ForeColor = Color.RosyBrown;

            //    }
            //}

            if (e.DisplayText.ToString() == "OFF" || e.DisplayText.ToString() == " OFF" || e.DisplayText.ToString() == "OFF " || e.DisplayText.ToString() == "")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.DisplayText = string.Empty;
            }

            if (e.DisplayText.ToString() == "EX")
            {
                e.DisplayText = string.Empty;
            }
        }

        private void TodayBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmDaysChecklists Propfrm = new frmDaysChecklists();

            Propfrm._theSystemDBTag = theSystemDBTag;
            Propfrm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            Propfrm.barEditItem3.EditValue = System.DateTime.Today;
            Propfrm.DateEdit1.EditValue = System.DateTime.Today;
            Propfrm.editSection.EditValue = EditSection.EditValue.ToString();
            Propfrm.UserCurrentInfo = UserCurrentInfo;

            Propfrm.ShowDialog();

            LoadGrid();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFormTypes();
            radioGroup2.SelectedIndex = 0;
            LoadChecklists();


        }

        private void gvOCRScheduling_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {

           

        }

        private void DeleteBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Get Selected Checklist Name for Display

            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail.SqlStatement = "  delete from tbl_OCR_CheclistsAdded where UniqueID = '" + SelectedUniqueID + "' and calendardate = '" + Convert.ToDateTime(SelectedCalendardate) + "' and workplaceid = '" + SelectedWorkplaceID + "' \r\n" +
                                            "  \r\n" +
                                            "    ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();


            UpdateSchedule3(SelectedFieldNameDate, SelectedUniqueID);

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Info", "Checklist Deleted", Color.CornflowerBlue);
            LoadGrid();
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {

            object SelectedValue = radioGroup2.EditValue;
            if (radioGroup2.SelectedIndex == 0)
            {
                lbcForms.Items.Clear();
                lbcForms.Items.AddRange(
                        _items.Select(i => i.ComplName.ToString()).ToArray());
            }
            else
            {
                lbcForms.Items.Clear();
                lbcForms.Items.AddRange(
                        _items.Where(i => i.TypeCode.Contains(SelectedValue.ToString())).Select(i => i.ComplName.ToString()).ToArray());
            }



        }

        private void PrinterBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {



        }

      
        private void gvOCRScheduling_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void GetFromInfo(string FormsID)
        {
            try
            {
                string GetFormInfoURL = string.Format(@"/api/Forms/GetFormInfo/");
                var client = new Models.ClientConnect();
                var param = new Dictionary<string, string>();
                param.Add("FormID", FormsID);

                var response = System.Threading.Tasks.Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

                _Forms = JsonConvert.DeserializeObject<Models.FormsAPI>(response);

                _Forms.UniqueDataStructure.TableName = _Forms.TableName;

                if (MoveToProdind == true)
                {
                    _Forms.UniqueDataStructure.Rows[0].Delete();

                    DataRow row;
                    row = _Forms.UniqueDataStructure.NewRow();

                    row["MOSectionID"] = gvOCRScheduling.GetRowCellValue(gvOCRScheduling.FocusedRowHandle, "Sectionid_2");
                    row["Workplaceid"] = procs.ExtractBeforeColon(gvOCRScheduling.GetRowCellValue(gvOCRScheduling.FocusedRowHandle, "Workplace").ToString());
                    row["Description"] = procs.ExtractAfterColon(gvOCRScheduling.GetRowCellValue(gvOCRScheduling.FocusedRowHandle, "Workplace").ToString());
                    row["CaptureDate"] = DateSelected.Value.ToString("ddMMyyyy");//DateTime.Now.ToString("ddMMyyyy");

                    _Forms.UniqueDataStructure.Rows.Add(row);
                }
                else
                {
                    _Forms.UniqueDataStructure.Rows[0].Delete();

                    DataRow row;
                    row = _Forms.UniqueDataStructure.NewRow();

                    row["MOSectionID"] = "EXAMPLE";
                    row["Workplaceid"] = "EXAMPLE";
                    row["Description"] = "EXAMPLE";
                    row["CaptureDate"] = DateTime.Now.ToString("ddMMyyyy");

                    _Forms.UniqueDataStructure.Rows.Add(row);
                }



            }
            catch
            {

            }

            GetReport(FormsID);

            if (MoveToProdind == true)
            {
                MovetoProd.DoWork += MovetoProd_DoWork;
                MovetoProd.RunWorkerAsync();
                //MovetoProd.RunWorkerCompleted();

            }

        }

        private void GetReport(string FormsID)
        {
            string GetReportRL = string.Format(@"/api/Report/GetReport/");
            var client = new ClientConnect();
            var param = new Dictionary<string, string>();
            var header = new Dictionary<string, string>();
            param.Add("FormsID", FormsID);
            _Forms.UniqueDataStructure.AcceptChanges();
            DataSet TheData = new DataSet();
            TheData.Tables.Clear();
            TheData.Tables.Add(_Forms.UniqueDataStructure);
            string JSOResult;
            JSOResult = JsonConvert.SerializeObject(TheData, Formatting.Indented);
            try
            {
                //this.pdfViewer1.LoadDocument(@"..\..\Report.pdf"

                var response = Task.Run(() => client.PostWithBodyAndParameters(GetReportRL, param, JSOResult)).Result;

                _PrintedForm = JsonConvert.DeserializeObject<PrintedForm>(response);
                string txtPDF = _PrintedForm.PDFLocation;

                if (File.Exists(@_PrintedForm.PDFLocation))
                {
                    if (MoveToProdind)
                    {
                        //Process.Start("explorer.exe", _PrintedForm.PDFLocation);
                        PdfViewer i = new PdfViewer();
                        DevExpress.XtraEditors.XtraForm jj = new DevExpress.XtraEditors.XtraForm();
                        i.LoadDocument(@_PrintedForm.PDFLocation);
                        i.Dock = DockStyle.Fill;
                        i.ZoomMode = PdfZoomMode.FitToWidth;
                        i.NavigationPanePageVisibility = PdfNavigationPanePageVisibility.None;
                        jj.Controls.Add(i);
                        jj.Width = 600;
                        jj.Height = 800;

                        jj.ShowIcon = false;
                        jj.Text = "OCR - DOCUMENT PREVIEW";
                        i.CreateRibbon();
                        jj.ShowDialog();
                        //PdfPrinterSettings ps = new PdfPrinterSettings();
                        //i.Print(ps);
                    }
                    else
                    {
                        PdfViewer i = new PdfViewer();
                        DevExpress.XtraEditors.XtraForm jj = new DevExpress.XtraEditors.XtraForm();
                        i.LoadDocument(@_PrintedForm.PDFLocation);
                        i.Dock = DockStyle.Fill;
                        i.ZoomMode = PdfZoomMode.FitToWidth;
                        i.NavigationPanePageVisibility = PdfNavigationPanePageVisibility.None;
                        jj.Controls.Add(i);
                        jj.Width = 600;
                        jj.Height = 800;

                        jj.ShowIcon = false;
                        jj.Text = "CHECKLIST EXAMPLE - CANNOT BE PRINTED";
                        jj.ShowDialog();
                    }

                }

            }
            catch (Exception error)
            {

            }
        }

        private void MoveToProd()
        {
            string GetFormInfoURL = string.Format(@"/api/Report/PrintReport/");
            //foreach (var s in _PrintedForm.PrintedFromID)
            //{

            var client = new ClientConnect();
            var param = new Dictionary<string, string>();
            param.Add("PrintedFromID", _PrintedForm.PrintedFromID[0].ToString());
            param.Add("PrintedByName", UserCurrentInfo.UserID);

            var response = Task.Run(() => client.GetWithParameters(GetFormInfoURL, param)).Result;

            _clsOCRScheduling.theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _clsOCRScheduling.UpdatePrintList(_PrintedForm.PrintedFromID[0].ToString(), SelectedUniqueID, SelectedCalendardate, SelectedWorkplaceID, procs.ExtractBeforeColon(ChecklistSelectedItem.EditValue.ToString()), DayNum);

            //}
        }

        private void FillFormTypes()
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail.SqlStatement = " SELECT TypeCode, CAST(TypeCode AS VARCHAR(10)) + '-' + TypeName AS ComplName  \r\n" +
                                            " FROM [dbo].[vw_OCR_Form_List] WHERE GroupName = '" + radioGroup1.EditValue.ToString() + "'\r\n" +
                                             "  GROUP BY TypeName, TypeCode  ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            DataTable dt_FormList = _dbManWPSTDetail.ResultsDataTable;


            radioGroup2.Properties.Items.Clear();
            RadioGroupItem rdi_all = new RadioGroupItem();
            rdi_all.Description = "All";
            rdi_all.Value = "All";
            radioGroup2.Properties.Items.Add(rdi_all);
            foreach (DataRow row in dt_FormList.Rows)
            {
                RadioGroupItem rdi = new RadioGroupItem();
                rdi.Description = row[1].ToString();
                rdi.Value = row[0].ToString();
                rdi.Tag = row[0].ToString();

                radioGroup2.Properties.Items.Add(rdi);
            }

        }

        private void FillFormGroups()
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail.SqlStatement = " SELECT GroupName \r\n" +
                                            " FROM [dbo].[vw_OCR_Form_List] \r\n" +

                                             "  GROUP BY GroupName  ";

            _dbManWPSTDetail.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail.ExecuteInstruction();

            DataTable dt_FormList = _dbManWPSTDetail.ResultsDataTable;


            radioGroup1.Properties.Items.Clear();
            foreach (DataRow row in dt_FormList.Rows)
            {
                RadioGroupItem rdi = new RadioGroupItem();
                rdi.Description = row[0].ToString();
                rdi.Value = row[0].ToString();
                rdi.Tag = row[0].ToString();

                radioGroup1.Properties.Items.Add(rdi);
            }

        }

        public class ListDrop
        {
            public string TypeCode { get; set; }
            public string ComplName { get; set; }
        }

        
       

        void insertPlanningOther()
        {
            MWDataManager.clsDataAccess _dbManWPSTDetail2 = new MWDataManager.clsDataAccess();
            _dbManWPSTDetail2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPSTDetail2.SqlStatement = "[dbo].[sp_OCR_CopyPlanningOther] @Month =" + procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue)) + ",@SectionID='" + EditSection.EditValue.ToString() + "' ";

            _dbManWPSTDetail2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPSTDetail2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPSTDetail2.ExecuteInstruction();


            //MessageBox.Show(_dbManWPSTDetail2.ResultsDataTable.Rows[0][0].ToString());

        }

      

        private void lbcForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbcForms.SelectedItems.Count > 0)
            {
                if (lbcForms.SelectedItems[0] != null)
                {
                    Checklist = lbcForms.SelectedItem.ToString();
                }
                else
                {
                    Checklist = string.Empty;
                }
            }
        }

        private void lbcForms_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraSplashScreen.IOverlaySplashScreenHandle handle = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            try
            {
                MoveToProdind = false;
                GetFromInfo(procs.ExtractBeforeColon(Checklist.ToString()));
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);

            }
            catch
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(handle);
                //MessageBox.Show(@"C:\Mineware\Syncromine\OCRScheduling\OCRApp\OCRWebApiAccess.exe", "Cant Find OCR Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void EditProdmonth_EditValueChanged_1(object sender, EventArgs e)
        {
            string i = procs.ProdMonthAsString(Convert.ToDateTime(EditProdmonth.EditValue));
            _clsOCRScheduling.theData.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            SelectedPageGroup.Visible = false;
            ChecklistInfoGroup.Visible = false;
            updateSections(SysSettings.ProdMonth);
            //FillFormGroups();
            LoadChecklists();
            InsertBlankRecordsIFNoneYet();
            LoadGrid();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void EditSection_EditValueChanged(object sender, EventArgs e)
        {
            InsertBlankRecordsIFNoneYet();
            LoadGrid();
        }

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {

        }
    }
}
