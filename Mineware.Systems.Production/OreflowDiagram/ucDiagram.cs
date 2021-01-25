using FastReport;
using Microsoft.VisualBasic.PowerPacks;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionHelp;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class ucDiagram : BaseUserControl
    {
        #region Global variables
        DataTable dt = new DataTable();
        float oX = -1;
        float oY = -1;

        Point ps = new Point();
        Point pe = new Point();
        Bitmap drawing;

        Boolean drawshaft;
        Boolean drawmill;
        Boolean drawtram;
        Boolean drawlevel;
        Boolean drawsubshaft;
        Boolean draworepass;
        Boolean draworebin;
        Boolean drawboxhole;

        Boolean drawshaftline;
        Boolean drawsubshaftline;
        Boolean drawlevelline;
        Boolean draworepassline;
        Boolean drawtramline;
        Boolean movable;

        decimal Xpos;
        decimal Ypos;
        decimal dPicXpos;
        decimal dPicYpos;

        int dsXpos;
        int dsYpos;
        int deXpos;
        int deYpos;

        System.Windows.Forms.PictureBox P = new PictureBox();
        Report theReport = new Report();

        string lblcount;
        string DesignType;

        string isChanged;

        public clsCoordinatesBinding clsCoord = new clsCoordinatesBinding();
        public clsCoordinatesBinding.ObjectCoordinates ObjectCoordinates = new clsCoordinatesBinding.ObjectCoordinates() { X1Coord = null, Y1Coord = null, X2Coord = null, Y2Coord = null, OreflowCode = string.Empty, OreflowID = string.Empty };
        #endregion

        private bool SafetyDep = false;
        private bool RockEngDep = false;
        private bool VentDep = false;
        private bool CostingDep = false;
        private bool HRDep = false;
        private bool SurveyDep = false;
        private bool GeologyDep = false;
        private bool EngDep = false;

        private bool SafetyDepAuth = false;
        private bool RockEngDepAuth = false;
        private bool VentDepAuth = false;
        private bool CostingDepAuth = false;
        private bool HRDepAuth = false;
        private bool SurveyDepAuth = false;
        private bool GeologyDepAuth = false;
        private bool EngDepAuth = false;

        private bool SafetyStartUp = false;
        private bool RockEngStartUp = false;
        private bool PlanningStartUp = false;
        private bool SurveyStartUp = false;
        private bool GeologyStartUp = false;
        private bool MiningStartup = false;
        private bool VentStartUp = false;
        private bool DepartmentStartUp = false;

        private bool SafetyStartUpAuth = false;
        private bool RockEngStartUpAuth = false;
        private bool PlanningStartUpAuth = false;
        private bool SurveyStartUpAuth = false;
        private bool GeologyStartUpAuth = false;
        private bool MiningStartupAuth = false;
        private bool VentStartUpAuth = false;
        private bool DepartmentStartUpAuth = false;

        private bool OreflowDesign = false;
        private bool OreflowBackfill = false;

        ShapeContainer canvas = new ShapeContainer();
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";

        public RectangleShape theShape = new RectangleShape();

        public ucDiagram()
        {
            InitializeComponent();
            drawPanel.VerticalScroll.Enabled = true;
            drawPanel.VerticalScroll.Visible = true;
            drawPanel.HorizontalScroll.Enabled = true;
            drawPanel.HorizontalScroll.Visible = true;
            FormRibbonPages.Add(rpDiagram);
            FormActiveRibbonPage = rpDiagram;
            FormMainRibbonPage = rpDiagram;
            RibbonControl = rcDiagram;
        }

        private void ucDiagram_Load(object sender, EventArgs e)
        {
            DesignType = "Oreflow";
            clsBackfillDrawing._DesignType = "Oreflow";
            clsBackfillDrawing._IsGragphics = "Y";

            tcMain.TabPages[0].Text = "       Surface       ";
            tcMain.TabPages[1].Text = "      Bookings       ";
            tcMain.TabPages[2].Text = "      Reports       ";

            tabControlReport.TabPages[0].Text = "   Problems Report    ";
            tabControlReport.TabPages[1].Text = "   aa    ";

            tabControlReport.TabPages.RemoveAt(1);
            tcBooking.TabPages.Remove(tpBackfillBooking);

            tcBooking.TabPages["tpTrammingBook"].Text = "      Tramming Booking       ";
            tcBooking.TabPages["tpHoistingBook"].Text = "      Hoisting Booking       ";
            tcBooking.TabPages["tpMillingBooking"].Text = "      Milling Booking       ";

            LoadBookingtramming();
            LoadBookingHoisting();
            LoadBookingMilling();

            movable = false;
            drawing = new Bitmap(drawPanel.Width, drawPanel.Height, drawPanel.CreateGraphics());
            clsBackfillDrawing.isEdit = false;
            clsBackfillDrawing._Systemtag = this.theSystemDBTag;
            clsBackfillDrawing._Userconnection = this.UserCurrentInfo.Connection;
            clsBackfillDrawing._DesignType = barDesignType.EditValue.ToString();

            dataGrid.Rows.Clear();
            dataGrid.Visible = false;

            dataGrid.ColumnCount = 28;

            dataGrid.Columns[0].HeaderText = "ID";
            dataGrid.Columns[1].HeaderText = "Name";
            dataGrid.Columns[2].HeaderText = "BFCode";
            dataGrid.Columns[3].HeaderText = "x";
            dataGrid.Columns[4].HeaderText = "y";

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " select oreflowid bfid,name,oreflowcode bfcode,x1,y1 from tbl_OreFlowEntities ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            int zz = 0;

            dataGrid.RowCount = 1000;

            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {
                dataGrid.RowCount = _dbMan.ResultsDataTable.Rows.Count + 1;
            }

            DataTable Neil = _dbMan.ResultsDataTable;
            foreach (DataRow r in Neil.Rows)
            {
                dataGrid.Rows[zz].Cells[0].Value = r["bfid"].ToString();
                dataGrid.Rows[zz].Cells[1].Value = r["name"].ToString();
                dataGrid.Rows[zz].Cells[2].Value = r["bfcode"].ToString();
                dataGrid.Rows[zz].Cells[3].Value = r["x1"].ToString();
                dataGrid.Rows[zz].Cells[4].Value = r["y1"].ToString();
                zz = zz + 1;
            }

            dataGrid.RowCount = zz;

            //accordionControl1.OptionsNavPane.NavPaneState = NavPaneState.Collapsed;

            dtReefboringsketches.Left += 5;
            dtReefboringsketches.Height = 400;

            EditBtn.Left = 40;
            EditBtn.Top = lbxDrawings.Top;
            EditBtn.Visible = false;

            SaveBtn.Enabled = false;
            DeleteBtn.Enabled = false;
            AddBtn.Enabled = false;

            panel2.Top = dtReefboringsketches.Bottom + 140;

            pnlMain.Dock = DockStyle.Fill;

            refesh();

            LoadNavBar();
            LoadNavbarGrid();

            #region BackfillBooking
            LoadBookingGrid();
            #endregion

            #region Reports
            tcMain.TabPages.Remove(tabPage3);
            #endregion

            XcoTxt.DataBindings.Add(new Binding("Text", ObjectCoordinates, "X1Coord", true, DataSourceUpdateMode.OnPropertyChanged));
            YcoTxt.DataBindings.Add(new Binding("Text", ObjectCoordinates, "Y1Coord", true, DataSourceUpdateMode.OnPropertyChanged));
            X2coTxt.DataBindings.Add(new Binding("Text", ObjectCoordinates, "X2Coord", true, DataSourceUpdateMode.OnPropertyChanged));
            Y2coTxt.DataBindings.Add(new Binding("Text", ObjectCoordinates, "Y2Coord", true, DataSourceUpdateMode.OnPropertyChanged));
            txtOreflowCode.DataBindings.Add(new Binding("Text", ObjectCoordinates, "OreflowCode", true, DataSourceUpdateMode.OnPropertyChanged));
            txtOreflowid.DataBindings.Add(new Binding("Text", ObjectCoordinates, "OreflowID", true, DataSourceUpdateMode.OnPropertyChanged));

            clsBackfillDrawing.ObjCoordinates = ObjectCoordinates;

            isChanged = "N";
        }

        public void loadDrawpanel()
        {
            drawing = new Bitmap(drawPanel.Width, drawPanel.Height, drawPanel.CreateGraphics());
        }

        public void addItemToListBox(string item, string sname, string x1, string x2)
        {
            if (label3.Text != Levellbl.Text)
            {
                label3.Text = Levellbl.Text;
            }

            Levellbl.Text = string.Empty;

            bfYtxt.Text = item;
            Levellbl.Text = "OBJ :" + sname;
            bfX1txt.Text = x1;
            bfX2txt.Text = x2;
        }

        public void refesh()
        {
            clsBackfillDrawing.DrawSurfaceLine(drawPanel, drawing, 0);
            clsBackfillDrawing.LoadShaft(drawPanel, drawing, imgReefCollection.Images[0]);
            clsBackfillDrawing.LoadSubShaft(drawPanel, drawing);
            clsBackfillDrawing.LoadTerShaft(drawPanel, drawing);
            clsBackfillDrawing.LoadInclineShaft(drawPanel, drawing);
            clsBackfillDrawing.LoadLevels(drawPanel, drawing);
            clsBackfillDrawing.LoadOrepass(drawPanel, drawing);

            if (DesignType == "Oreflow")
            {
                clsBackfillDrawing.LoadInternalOrepass(drawPanel, drawing);
            }

            if (DesignType != "Oreflow")
            {
                clsBackfillDrawing.LoadMill(drawPanel, imagePlantCollection.Images[0]);
                clsBackfillDrawing.LoadDam(drawPanel, imageBackFillCollection.Images[0]);
                clsBackfillDrawing.LoadPachuca(drawPanel, imagePatchuca.Images[0]);
                clsBackfillDrawing.LoadTank(drawPanel, imageTankCollection.Images[0]);

                if (rgbDaily.Checked == true)
                {
                    clsBackfillDrawing.Loadgrids(drawPanel, 0);
                }

                if (rgbLast7.Checked == true)
                {
                    clsBackfillDrawing.Loadgrids(drawPanel, 6);
                }

                if (rgbLast30.Checked == true)
                {
                    clsBackfillDrawing.Loadgrids(drawPanel, 29);
                }
            }
        }

        private void LoadNavBar()
        {
            //accordionControl1.Dock = DockStyle.Fill;

            //A.LinkSelectionMode = LinkSelectionModeType.OneInControl;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (DesignType == "Oreflow")
            {
                _dbMan.SqlStatement = " Select  group1='Mill',group3 ='Shaft', group4 ='SubShaft', group5 ='TerShaft', group6 ='InclineShaft', group7 ='Level'  \r\n" +
                                  ", 'Internal Orepass' group12";
            }
            else
            {
                _dbMan.SqlStatement = " Select  group1='Mill', group2 ='Tank' \r\n" +
                                  ",group8 ='Pachuca', group9 ='Dam' ,group10 = 'Transfer Range' , group11 = 'Range'";
            }
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            dt = _dbMan.ResultsDataTable;

            lbxDrawings.Items.Clear();

            foreach (DataRow r in dt.Rows)
            {
                if (DesignType == "Oreflow")
                {
                    lbxDrawings.Items.Add(r["group1"].ToString());
                    lbxDrawings.Items.Add(r["group3"].ToString());
                    lbxDrawings.Items.Add(r["group4"].ToString());
                    lbxDrawings.Items.Add(r["group5"].ToString());
                    lbxDrawings.Items.Add(r["group6"].ToString());
                    lbxDrawings.Items.Add(r["group7"].ToString());
                    lbxDrawings.Items.Add(r["group12"].ToString());
                }
                else
                {
                    lbxDrawings.Items.Add(r["group1"].ToString());
                    lbxDrawings.Items.Add(r["group2"].ToString());
                    lbxDrawings.Items.Add(r["group8"].ToString());
                    lbxDrawings.Items.Add(r["group9"].ToString());
                    lbxDrawings.Items.Add(r["group10"].ToString());
                    lbxDrawings.Items.Add(r["group11"].ToString());
                }
            }
        }

        private void LoadNavbarGrid()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'Mill' and Inactive = '1' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            dt = _dbMan.ResultsDataTable;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            dtReefboringsketches.DataSource = null;
            dtReefboringsketches.DataSource = ds.Tables[0];

            colDescription.FieldName = "Name";
            colID.FieldName = "BFID";
            AddBtn.Visible = false;
        }

        public void LoadBookingGrid()
        {
            string CurrDate = String.Format("{0:yyyy-MM-dd}", CurrentDate.Value);

            MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
            _dbMana.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMana.SqlStatement = "  declare @LastDate datetime  \r\n " +

                                  " set @LastDate = '" + CurrDate + "' \r\n " +

                                  " Select @LastDate dd , '30' days \r\n " +
                                  " union \r\n " +
                                  " select DateADD(dd,-1 ,@LastDate ) dd ,'29' days \r\n " +
                                  " union \r\n " +
                                  " select DateADD(dd,-2 ,@LastDate ) dd , '28' days \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-3 ,@LastDate) dd , '27' days \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-4 ,@LastDate ) dd , '26' days \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-5 ,@LastDate ) dd , '25' days \r\n " +
                                  " union \r\n " +
                                  " select DateADD(dd,-6 ,@LastDate ) dd , '24' days \r\n " +
                                  " union   \r\n " +
                                  " select DateADD(dd,-7 ,@LastDate ) dd , '23' days \r\n " +
                                  " union \r\n " +
                                  " select DateADD(dd,- 8,@LastDate ) dd , '22' days  \r\n " +
                                  " union \r\n " +
                                  " select DateADD(dd,-9 ,@LastDate ) dd , '21' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-10 ,@LastDate ) dd , '20' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-11 ,@LastDate ) dd , '19' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-12 ,@LastDate ) dd , '18' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-13 ,@LastDate ) dd , '17' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-14 ,@LastDate ) dd , '16' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-15 ,@LastDate ) dd , '15' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-16 ,@LastDate ) dd , '14' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-17 ,@LastDate ) dd , '13' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-18 ,@LastDate ) dd , '12' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-19 ,@LastDate ) dd , '11' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-20 ,@LastDate ) dd , '10' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-21 ,@LastDate ) dd , '9' days   \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-22 ,@LastDate ) dd , '8' days   \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-23 ,@LastDate) dd , '7' days   \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-24 ,@LastDate ) dd , '6' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-25 ,@LastDate ) dd , '5' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-26 ,@LastDate ) dd , '4' days \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-27 ,@LastDate) dd , '3' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-28 ,@LastDate ) dd , '2' days  \r\n " +
                                  " union  \r\n " +
                                  " select DateADD(dd,-29 ,@LastDate ) dd , '1' days ";
            _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMana.ExecuteInstruction();

            DataTable dt = _dbMana.ResultsDataTable;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            int stpnewCol = 2;

            foreach (DataRow r in dt.Rows)
            {
                gridView2.Columns[stpnewCol].Caption = Convert.ToDateTime(r["dd"].ToString()).ToString("dd MMM ddd");
                stpnewCol = stpnewCol + 1;
            }

            DateTime date = CurrentDate.Value.Date.AddDays(-29);
            string startdate = String.Format("{0:yyyy-MM-dd}", date.ToString());

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " exec sp_Backfill_Booking '" + startdate + "'  \r\n " +
                                  "   ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtData = _dbMan.ResultsDataTable;
            DataSet dsData = new DataSet();
            dsData.Tables.Add(dtData);

            if (dtData.Rows.Count > 0)
            {
                colRange.FieldName = "RangeName";
                colWP.FieldName = "Workplace";
                colDay1.FieldName = "Day1";
                colDay2.FieldName = "Day2";
                colDay3.FieldName = "Day3";
                colDay4.FieldName = "Day4";
                colDay5.FieldName = "Day5";
                colDay6.FieldName = "Day6";
                colDay7.FieldName = "Day7";
                colDay8.FieldName = "Day8";
                colDay9.FieldName = "Day9";
                colDay10.FieldName = "Day10";
                colDay11.FieldName = "Day11";
                colDay12.FieldName = "Day12";
                colDay13.FieldName = "Day13";
                colDay14.FieldName = "Day14";
                colDay15.FieldName = "Day15";
                colDay16.FieldName = "Day16";
                colDay17.FieldName = "Day17";
                colDay18.FieldName = "Day18";
                colDay19.FieldName = "Day19";
                colDay20.FieldName = "Day20";
                colDay21.FieldName = "Day21";
                colDay22.FieldName = "Day22";
                colDay23.FieldName = "Day23";
                colDay24.FieldName = "Day24";
                colDay25.FieldName = "Day25";
                colDay26.FieldName = "Day26";
                colDay27.FieldName = "Day27";
                colDay28.FieldName = "Day28";
                colDay29.FieldName = "Day29";
                colDay30.FieldName = "Day30";
            }
            BookingGrid.DataSource = dsData.Tables[0];
        }

        public void LoadProblemReport()
        {
            MWDataManager.clsDataAccess _dbManBookLast1 = new MWDataManager.clsDataAccess();
            _dbManBookLast1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManBookLast1.SqlStatement = "   Declare @enddate datetime   \r\n" +
                                            "   Declare @Startdate datetime    \r\n" +
                                            "   set @startdate = dateadd( dd, -1,GetDate())  \r\n" +
                                            "   set @enddate = GETDATE()   \r\n" +

                                            "  select * from ( select top(10) * from (select * from (  \r\n" +
                                            "  Select '01' aa , description problemID,  sum((convert(decimal(18,0),substring(timeto,1,2))+convert(decimal(18,0),substring(timeto,4,2))/60)-   \r\n" +
                                            "  (convert(decimal(18,0),substring(timefrom,1,2))+convert(decimal(18,0),substring(timefrom,4,2))/60)) Diff  from tbl_Backfill_Booking_Problems    \r\n" +
                                            "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))    \r\n" +
                                            "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                            "  group by description  \r\n" +
                                            "  )a   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '02'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union \r\n" +
                                            "  Select '03'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '04'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '05'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '06'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '07'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '08'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '09'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '10'aa, '' problemID, 0 Diff) a order by Diff desc) a order by aa, diff desc ";
            _dbManBookLast1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManBookLast1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManBookLast1.ResultsTableName = "BookLast1";
            _dbManBookLast1.ExecuteInstruction();

            DataTable dtBook1 = _dbManBookLast1.ResultsDataTable;
            DataSet dsBook1 = new DataSet();
            dsBook1.Tables.Add(dtBook1);

            MWDataManager.clsDataAccess _dbManBookLast7 = new MWDataManager.clsDataAccess();
            _dbManBookLast7.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManBookLast7.SqlStatement = "   Declare @enddate datetime   \r\n" +
                                            "   Declare @Startdate datetime    \r\n" +
                                            "   set @startdate = dateadd( dd, -6,GetDate())  \r\n" +
                                            "   set @enddate = GETDATE()   \r\n" +

                                            "  select * from ( select top(10) * from (select * from (  \r\n" +
                                            "  Select '01' aa , description problemID,  sum((convert(decimal(18,0),substring(timeto,1,2))+convert(decimal(18,0),substring(timeto,4,2))/60)-   \r\n" +
                                            "  (convert(decimal(18,0),substring(timefrom,1,2))+convert(decimal(18,0),substring(timefrom,4,2))/60)) Diff  from tbl_Backfill_Booking_Problems    \r\n" +
                                            "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))    \r\n" +
                                            "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                            "  group by description  \r\n" +
                                            "  )a   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '02'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union \r\n" +
                                            "  Select '03'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '04'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '05'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '06'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '07'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '08'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '09'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '10'aa, '' problemID, 0 Diff) a order by Diff desc) a order by aa, diff desc ";
            _dbManBookLast7.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManBookLast7.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManBookLast7.ResultsTableName = "BookLast7";
            _dbManBookLast7.ExecuteInstruction();

            DataTable dtBook7 = _dbManBookLast7.ResultsDataTable;
            DataSet dsBook7 = new DataSet();
            dsBook7.Tables.Add(dtBook7);

            MWDataManager.clsDataAccess _dbManBookLast30 = new MWDataManager.clsDataAccess();
            _dbManBookLast30.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManBookLast30.SqlStatement = "   Declare @enddate datetime   \r\n" +
                                            "   Declare @Startdate datetime    \r\n" +
                                            "   set @startdate = dateadd( dd, -29,GetDate())  \r\n" +
                                            "   set @enddate = GETDATE()   \r\n" +

                                            "  select * from ( select top(10) * from (select * from (  \r\n" +
                                            "  Select '01' aa , description problemID,  sum((convert(decimal(18,0),substring(timeto,1,2))+convert(decimal(18,0),substring(timeto,4,2))/60)-   \r\n" +
                                            "  (convert(decimal(18,0),substring(timefrom,1,2))+convert(decimal(18,0),substring(timefrom,4,2))/60)) Diff  from tbl_Backfill_Booking_Problems    \r\n" +
                                            "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))    \r\n" +
                                            "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                            "  group by description  \r\n" +
                                            "  )a   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '02'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union \r\n" +
                                            "  Select '03'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '04'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '05'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '06'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '07'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '08'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '09'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '10'aa, '' problemID, 0 Diff) a order by Diff desc) a order by aa, diff desc ";
            _dbManBookLast30.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManBookLast30.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManBookLast30.ResultsTableName = "BookLast30";
            _dbManBookLast30.ExecuteInstruction();

            DataTable dtBook30 = _dbManBookLast30.ResultsDataTable;
            DataSet dsBook30 = new DataSet();
            dsBook30.Tables.Add(dtBook30);

            MWDataManager.clsDataAccess _dbManSurfLast1 = new MWDataManager.clsDataAccess();
            _dbManSurfLast1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSurfLast1.SqlStatement = "   Declare @enddate datetime  \r\n" +
                                           "   Declare @Startdate datetime   \r\n" +
                                           "   set @startdate = dateadd( dd, -1,GetDate())  \r\n" +
                                           "   set @enddate = GETDATE()  \r\n\r\n" +

                                            "  select * from ( select top(10) * from (select * from (  \r\n" +
                                            "  Select '01' aa , description problemID,  sum((convert(decimal(18,0),substring(timeto,1,2))+convert(decimal(18,0),substring(timeto,4,2))/60)-   \r\n" +
                                            "  (convert(decimal(18,0),substring(timefrom,1,2))+convert(decimal(18,0),substring(timefrom,4,2))/60)) Diff  from tbl_Backfill_ProblemsSurface    \r\n" +
                                            "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))    \r\n" +
                                            "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                            "  group by description  \r\n" +
                                            "  )a   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '02'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union \r\n" +
                                            "  Select '03'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '04'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '05'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '06'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '07'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '08'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '09'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '10'aa, '' problemID, 0 Diff) a order by Diff desc) a order by aa, diff desc ";
            _dbManSurfLast1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSurfLast1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSurfLast1.ResultsTableName = "SurfLast1";
            _dbManSurfLast1.ExecuteInstruction();

            DataTable dtSurf1 = _dbManSurfLast1.ResultsDataTable;
            DataSet dsSurf1 = new DataSet();
            dsSurf1.Tables.Add(dtSurf1);

            MWDataManager.clsDataAccess _dbManSurfLast7 = new MWDataManager.clsDataAccess();
            _dbManSurfLast7.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSurfLast7.SqlStatement = "   Declare @enddate datetime  \r\n" +
                                           "   Declare @Startdate datetime   \r\n" +
                                           "   set @startdate = dateadd( dd, -6,GetDate())  \r\n" +
                                           "   set @enddate = GETDATE()  \r\n\r\n" +

                                            "  select * from ( select top(10) * from (select * from (  \r\n" +
                                            "  Select '01' aa , description problemID,  sum((convert(decimal(18,0),substring(timeto,1,2))+convert(decimal(18,0),substring(timeto,4,2))/60)-   \r\n" +
                                            "  (convert(decimal(18,0),substring(timefrom,1,2))+convert(decimal(18,0),substring(timefrom,4,2))/60)) Diff  from tbl_Backfill_ProblemsSurface    \r\n" +
                                            "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))    \r\n" +
                                            "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                            "  group by description  \r\n" +
                                            "  )a   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '02'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union \r\n" +
                                            "  Select '03'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '04'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '05'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '06'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '07'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '08'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '09'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '10'aa, '' problemID, 0 Diff) a order by Diff desc) a order by aa, diff desc ";
            _dbManSurfLast7.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSurfLast7.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSurfLast7.ResultsTableName = "SurfLast7";
            _dbManSurfLast7.ExecuteInstruction();

            DataTable dtSurf7 = _dbManSurfLast7.ResultsDataTable;
            DataSet dsSurf7 = new DataSet();
            dsSurf7.Tables.Add(dtSurf7);

            MWDataManager.clsDataAccess _dbManSurfLast30 = new MWDataManager.clsDataAccess();
            _dbManSurfLast30.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSurfLast30.SqlStatement = "   Declare @enddate datetime   \r\n" +
                                            "   Declare @Startdate datetime    \r\n" +
                                            "   set @startdate = dateadd( dd, -29,GetDate())  \r\n" +
                                            "   set @enddate = GETDATE()   \r\n" +

                                            "  select * from ( select top(10) * from (select * from (  \r\n" +
                                            "  Select '01' aa , description problemID,  sum((convert(decimal(18,0),substring(timeto,1,2))+convert(decimal(18,0),substring(timeto,4,2))/60)-   \r\n" +
                                            "  (convert(decimal(18,0),substring(timefrom,1,2))+convert(decimal(18,0),substring(timefrom,4,2))/60)) Diff  from tbl_Backfill_ProblemsSurface    \r\n" +
                                            "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))    \r\n" +
                                            "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                            "  group by description  \r\n" +
                                            "  )a   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '02'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union \r\n" +
                                            "  Select '03'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '04'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '05'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '06'aa, '' problemID, 0 Diff  \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '07'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '08'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '09'aa, '' problemID, 0 Diff   \r\n" +
                                            "  union all  \r\n" +
                                            "  Select '10'aa, '' problemID, 0 Diff) a order by Diff desc) a order by aa, diff desc ";
            _dbManSurfLast30.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSurfLast30.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSurfLast30.ResultsTableName = "SurfLast30";
            _dbManSurfLast30.ExecuteInstruction();

            DataTable dtSurf30 = _dbManSurfLast30.ResultsDataTable;
            DataSet dsSurf30 = new DataSet();
            dsSurf30.Tables.Add(dtSurf30);

            MWDataManager.clsDataAccess _dbManUnderLast1 = new MWDataManager.clsDataAccess();
            _dbManUnderLast1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManUnderLast1.SqlStatement = "   Declare @enddate datetime   \r\n" +
                                           "   Declare @Startdate datetime    \r\n" +
                                           "   set @startdate = dateadd( dd, -1,GetDate())  \r\n" +
                                           "   set @enddate = GETDATE()   \r\n" +

                                           "  select * from ( select top(10) * from (select * from (  \r\n" +
                                           "  Select '01' aa , description problemID,  sum((convert(decimal(18,0),substring(timeto,1,2))+convert(decimal(18,0),substring(timeto,4,2))/60)-   \r\n" +
                                           "  (convert(decimal(18,0),substring(timefrom,1,2))+convert(decimal(18,0),substring(timefrom,4,2))/60)) Diff  from tbl_Backfill_Problems    \r\n" +
                                           "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))    \r\n" +
                                           "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                           "  group by description  \r\n" +
                                           "  )a   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '02'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union \r\n" +
                                           "  Select '03'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '04'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '05'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '06'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '07'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '08'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '09'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '10'aa, '' problemID, 0 Diff) a order by Diff desc) a order by aa, diff desc ";
            _dbManUnderLast1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManUnderLast1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManUnderLast1.ResultsTableName = "UnderGroundLast1";
            _dbManUnderLast1.ExecuteInstruction();

            DataTable dtunder1 = _dbManUnderLast1.ResultsDataTable;
            DataSet dsunder1 = new DataSet();
            dsunder1.Tables.Add(dtunder1);

            MWDataManager.clsDataAccess _dbManUnderLast7 = new MWDataManager.clsDataAccess();
            _dbManUnderLast7.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManUnderLast7.SqlStatement = "   Declare @enddate datetime   \r\n" +
                                           "   Declare @Startdate datetime    \r\n" +
                                           "   set @startdate = dateadd( dd, -6,GetDate())  \r\n" +
                                           "   set @enddate = GETDATE()   \r\n" +

                                           "  select * from ( select top(10) * from (select * from (  \r\n" +
                                           "  Select '01' aa , description problemID,  sum((convert(decimal(18,0),substring(timeto,1,2))+convert(decimal(18,0),substring(timeto,4,2))/60)-   \r\n" +
                                           "  (convert(decimal(18,0),substring(timefrom,1,2))+convert(decimal(18,0),substring(timefrom,4,2))/60)) Diff  from tbl_Backfill_Problems    \r\n" +
                                           "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))    \r\n" +
                                           "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                           "  group by description  \r\n" +
                                           "  )a   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '02'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union \r\n" +
                                           "  Select '03'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '04'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '05'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '06'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '07'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '08'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '09'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '10'aa, '' problemID, 0 Diff) a order by Diff desc) a order by aa, diff desc ";
            _dbManUnderLast7.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManUnderLast7.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManUnderLast7.ResultsTableName = "UnderGroundLast7";
            _dbManUnderLast7.ExecuteInstruction();

            DataTable dtunder7 = _dbManUnderLast7.ResultsDataTable;
            DataSet dsunder7 = new DataSet();
            dsunder7.Tables.Add(dtunder7);

            MWDataManager.clsDataAccess _dbManUnderLast30 = new MWDataManager.clsDataAccess();
            _dbManUnderLast30.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManUnderLast30.SqlStatement = "   Declare @enddate datetime   \r\n" +
                                           "   Declare @Startdate datetime    \r\n" +
                                           "   set @startdate = dateadd( dd, -29,GetDate())  \r\n" +
                                           "   set @enddate = GETDATE()   \r\n" +

                                           "  select * from ( select top(10) * from (select * from (  \r\n" +
                                           "  Select '01' aa , description problemID,  sum((convert(decimal(18,0),substring(timeto,1,2))+convert(decimal(18,0),substring(timeto,4,2))/60)-   \r\n" +
                                           "  (convert(decimal(18,0),substring(timefrom,1,2))+convert(decimal(18,0),substring(timefrom,4,2))/60)) Diff  from tbl_Backfill_Problems    \r\n" +
                                           "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))    \r\n" +
                                           "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,Calendardate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                           "  group by description  \r\n" +
                                           "  )a   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '02'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union \r\n" +
                                           "  Select '03'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '04'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '05'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '06'aa, '' problemID, 0 Diff  \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '07'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '08'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '09'aa, '' problemID, 0 Diff   \r\n" +
                                           "  union all  \r\n" +
                                           "  Select '10'aa, '' problemID, 0 Diff) a order by Diff desc) a order by aa, diff desc ";
            _dbManUnderLast30.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManUnderLast30.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManUnderLast30.ResultsTableName = "UnderGroundLast30";
            _dbManUnderLast30.ExecuteInstruction();

            DataTable dtunder30 = _dbManUnderLast30.ResultsDataTable;
            DataSet dsunder30 = new DataSet();
            dsunder30.Tables.Add(dtunder30);

            theReport.RegisterData(dsunder1);
            theReport.RegisterData(dsunder7);
            theReport.RegisterData(dsunder30);

            theReport.RegisterData(dsBook1);
            theReport.RegisterData(dsBook7);
            theReport.RegisterData(dsBook30);

            theReport.RegisterData(dsSurf1);
            theReport.RegisterData(dsSurf7);
            theReport.RegisterData(dsSurf30);

            theReport.Load(_reportFolder + "BackfillProbReport.frx");

            //theReport.Design();

            pcReport.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport;
            theReport.ShowPrepared();
        }

        private void LoadBookingtramming()
        {
            tpTrammingBook.Controls.Clear();

            ucTrammingBooking Trambook = new ucTrammingBooking();
            Trambook._theSystemDBTag = this.theSystemDBTag;
            Trambook._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            tpTrammingBook.Controls.Add(Trambook);
            Trambook.Dock = DockStyle.Fill;
        }

        private void LoadBookingHoisting()
        {
            tpHoistingBook.Controls.Clear();

            ucHoistingBooking Hoistbook = new ucHoistingBooking();
            Hoistbook._theSystemDBTag = this.theSystemDBTag;
            Hoistbook._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            tpHoistingBook.Controls.Add(Hoistbook);
            Hoistbook.Dock = DockStyle.Fill;
        }

        private void LoadBookingMilling()
        {
            tpMillingBooking.Controls.Clear();

            ucMillingBooking Millbook = new ucMillingBooking();
            Millbook._theSystemDBTag = this.theSystemDBTag;
            Millbook._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            tpMillingBooking.Controls.Add(Millbook);
            Millbook.Dock = DockStyle.Fill;
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (EditBtn.Text == "&Design                 ")
            {
                lbxDrawings.Enabled = true;
                dtReefboringsketches.Enabled = true;

                drawPanel.Visible = false;

                drawPanel.Controls.Clear();
                refesh();

                drawPanel.Refresh();

                drawPanel.Visible = true;

                simpleButton1.Text = "&Normal";
                EditBtn.Text = "&Normal                 ";
                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                // Set the form as the parent of the ShapeContainer.
                canvas.Parent = drawPanel;
                // Set the ShapeContainer as the parent of the Shape.
                theShape.Parent = canvas;
                // Set the size of the shape.
                theShape.Size = drawPanel.Size;
                // Set the location of the shape.
                theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
                // To draw a rounded rectangle, add the following code:
                theShape.CornerRadius = 0;

                theShape.FillStyle = FillStyle.Percent05;

                theShape.Visible = true;

                drawPanel.BackColor = Color.Transparent;
                moveControls();
                SaveBtn.Enabled = true;
                DeleteBtn.Enabled = true;
                AddBtn.Enabled = true;

                theShape.SendToBack();

                accordionControl1.Controls.Add(panel7);
                panel7.Visible = false;
            }
            else
            {
                theShape.Visible = false;
                theShape.SendToBack();

                simpleButton1.Text = "&Design";
                EditBtn.Text = "&Design                 ";
                drawPanel.BackColor = Color.FromKnownColor(KnownColor.Control);
                clsBackfillDrawing.isEdit = false;
                SaveBtn.Enabled = false;
                DeleteBtn.Enabled = false;
                lbxDrawings.Enabled = false;
                dtReefboringsketches.Enabled = false;
                AddBtn.Enabled = false;
            }
        }

        private void moveControls()
        {
            movable = true;
            foreach (object ob in drawPanel.Controls)
            {
                if (ob.GetType().ToString() != "Microsoft.VisualBasic.PowerPacks.ShapeContainer")
                {
                    Control ctrl = (Control)ob;
                    ControlMover.Init(ctrl, drawPanel, drawing);
                }
                else
                {
                    clsBackfillDrawing.isEdit = true;
                }
            }
        }

        private void lbxDrawings_SelectedValueChanged(object sender, EventArgs e)
        {
            AddBtn.Enabled = false;
            SaveBtn.Enabled = false;
            DeleteBtn.Enabled = false;

            if (lbxDrawings.SelectedItem.ToString() == "Shaft")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'Shaft' and Inactive = '1' order by Name asc";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Clear();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";
                if (clsBackfillDrawing._IsGragphics == "Y")
                {
                    AddBtn.Visible = true;
                    AddBtn.Enabled = true;
                }
            }
            else if (lbxDrawings.SelectedItem.ToString() == "SubShaft")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'SubShaft' and Inactive = '1' order by Name asc ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";
                if (clsBackfillDrawing._IsGragphics == "Y")
                {
                    AddBtn.Visible = true;
                    AddBtn.Enabled = true;
                }
            }
            else if (lbxDrawings.SelectedItem.ToString() == "TerShaft")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'TerShaft' and Inactive = '1' order by Name asc";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";
                if (clsBackfillDrawing._IsGragphics == "Y")
                {
                    AddBtn.Visible = true;
                    AddBtn.Enabled = true;
                }
            }
            else if (lbxDrawings.SelectedItem.ToString() == "InclineShaft")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'InclShaft' and Inactive = '1' order by Name asc";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";
                if (clsBackfillDrawing._IsGragphics == "Y")
                {
                    AddBtn.Visible = true;
                    AddBtn.Enabled = true;
                }
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Level")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'Lvl' and Inactive = '1' and parentoreflowid = 'S5523' order by CONVERT(int ,Substring(Name,1,3)) asc ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Clear();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";
                if (clsBackfillDrawing._IsGragphics == "Y")
                {
                    AddBtn.Visible = true;
                    AddBtn.Enabled = true;
                }
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Dam")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'Dam' and Inactive = '1' order by Name asc";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";

                AddBtn.Enabled = true;
                SaveBtn.Enabled = true;
                DeleteBtn.Enabled = true;
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Range")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'Range' and Inactive = '1' order by Name asc";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";

                AddBtn.Enabled = true;
                SaveBtn.Enabled = true;
                DeleteBtn.Enabled = true;
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Mill")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'Mill' and Inactive = '1' order by Name asc";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";

                AddBtn.Enabled = false;
                DeleteBtn.Visible = true;
                DeleteBtn.Enabled = true;
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Pachuca")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'Pachuca' and Inactive = '1' order by Name asc";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";

                AddBtn.Enabled = true;
                SaveBtn.Enabled = true;
                DeleteBtn.Enabled = true;
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Plant")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = "Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'Plant' and Inactive = '1' order by Name asc ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";

                AddBtn.Enabled = false;
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Tank")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'Tank' and Inactive = '1' order by Name asc ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";

                AddBtn.Enabled = true;
                SaveBtn.Enabled = true;
                DeleteBtn.Enabled = true;
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Transfer Range")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'TRange' and Inactive = '1' order by Name asc";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";

                AddBtn.Enabled = true;
                SaveBtn.Enabled = true;
                DeleteBtn.Enabled = true;
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Internal Orepass")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " Select oreflowid BFID ,Name from tbl_OreFlowEntities where oreflowcode = 'IOrePass' and Inactive = '1' order by Name asc";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dt = _dbMan.ResultsDataTable;

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                dtReefboringsketches.DataSource = null;
                dtReefboringsketches.DataSource = ds.Tables[0];

                colDescription.FieldName = "Name";
                colID.FieldName = "BFID";

                AddBtn.Enabled = true;
                DeleteBtn.Enabled = true;
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (lbxDrawings.SelectedItem.ToString() == "Dam")
            {
                clsBackfillDrawing.UnDrawnDam(drawPanel, imageBackFillCollection.Images[0], (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Shaft")
            {
                clsBackfillDrawing.UnDrawnShaft(drawPanel, drawing, (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "SubShaft")
            {
                clsBackfillDrawing.UnDrawnSubShaft(drawPanel, drawing, (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "TerShaft")
            {
                clsBackfillDrawing.UnDrawnTerShaft(drawPanel, drawing, (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "InclineShaft")
            {
                clsBackfillDrawing.UnDrawnInclineShaft(drawPanel, drawing, (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Level")
            {
                clsBackfillDrawing.UnDrawnLevel(drawPanel, drawing, (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Orepass")
            {
                clsBackfillDrawing.UnDrawnOrePass(drawPanel, drawing, (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Range")
            {
                lbxDrawings_SelectedValueChanged(null, null);
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Mill")
            {
                clsBackfillDrawing.UnDrawnMill(drawPanel, imagePlantCollection.Images[0], (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Pachuca")
            {
                clsBackfillDrawing.UnDrawnPachuca(drawPanel, imagePatchuca.Images[0], (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Plant")
            {
                clsBackfillDrawing.UnDrawnPlant(drawPanel, imagePlantCollection.Images[0], (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Tank")
            {
                clsBackfillDrawing.UnDrawnTank(drawPanel, imageTankCollection.Images[0], (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Transfer Range")
            {
                lbxDrawings_SelectedValueChanged(null, null);
            }
            else if (lbxDrawings.SelectedItem.ToString() == "Internal Orepass")
            {
                clsBackfillDrawing.UnDrawnInternalOrepass(drawPanel, drawing, (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString()));
                lbxDrawings_SelectedValueChanged(null, null);
                lblcount = "Unsaved";
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            clsBackfillDrawing.DeleteLevels();
            clsBackfillDrawing.DeleteOrePass();
            clsBackfillDrawing.DeleteDam();
            clsBackfillDrawing.DeleteInclShaft();
            clsBackfillDrawing.DeleteShaft();
            clsBackfillDrawing.DeleteSubShaft();
            clsBackfillDrawing.DeleteTerShaft();
            clsBackfillDrawing.DeletePachuca();
            clsBackfillDrawing.DeleteMill();
            clsBackfillDrawing.DeleteTank();
            clsBackfillDrawing.DeleteInternalOrePass();

            lbxDrawings.SelectedItem = "Shaft";

            drawPanel.Visible = false;
            drawPanel.Controls.Clear();
            refesh();
            lbxDrawings_SelectedValueChanged(null, null);

            drawPanel.Refresh();

            canvas.Parent = drawPanel;
            // Set the ShapeContainer as the parent of the Shape.
            theShape.Parent = canvas;
            // Set the size of the shape.
            theShape.Size = drawPanel.Size;
            // Set the location of the shape.
            theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
            // To draw a rounded rectangle, add the following code:
            theShape.CornerRadius = 0;
            theShape.FillStyle = FillStyle.Percent05;
            theShape.SendToBack();
            drawPanel.Visible = true;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (lbxDrawings.SelectedItem.ToString() == "Range")
            {
                frmAddDamRange AddDamRangeFrm = new frmAddDamRange();
                AddDamRangeFrm.WindowState = FormWindowState.Normal;
                AddDamRangeFrm.StartPosition = FormStartPosition.CenterScreen;
                AddDamRangeFrm._theSystemDBTag = theSystemDBTag;
                AddDamRangeFrm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
                AddDamRangeFrm._OreflowType = lbxDrawings.SelectedItem.ToString();
                AddDamRangeFrm.ShowDialog();
            }
            else
            {
                frmAddOreflow addFrm = new frmAddOreflow();
                addFrm.StartPosition = FormStartPosition.CenterScreen;
                addFrm._OreflowType = lbxDrawings.SelectedItem.ToString();
                addFrm._theSystemDBTag = theSystemDBTag;
                addFrm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
                addFrm.ShowDialog();
            }

            lbxDrawings_SelectedValueChanged(null, null);
        }

        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            oX = e.X;
            oY = e.Y;

            if (drawmill)
            {
                if (oY < 107)
                {
                    P.Size = new Size(67, 58);
                    P.Parent = this.drawPanel;
                    P.BackColor = Color.Transparent;
                    P.Location = new Point(e.X, e.Y);
                    Xpos = e.X;
                    Ypos = e.Y;
                }
                else
                {
                    MessageBox.Show("Please draw Mills on the surface", "Error");
                }
            }
            else if (drawshaft)
            {
                if (oY < 107)
                {
                    P.Size = new Size(67, 58);
                    P.Parent = this.drawPanel;
                    // P.Image = OreFlow.Properties.Resources.Shaft;
                    P.BackColor = Color.Transparent;
                    P.Location = new Point(e.X, e.Y);
                    Xpos = e.X;
                    Ypos = e.Y;
                    dPicXpos = P.Location.X;
                    dPicYpos = P.Location.Y;
                }
                else
                {
                    MessageBox.Show("Please draw Shafts on the surface", "Error");
                }
            }
            else if (drawsubshaft)
            {
                if (oY > 107)
                {
                    P.Size = new Size(67, 58);
                    P.Parent = this.drawPanel;
                    // P.Image = OreFlow.Properties.Resources.Subshaft;
                    P.BackColor = Color.Transparent;
                    P.Location = new Point(e.X, e.Y);
                    Xpos = e.X;
                    Ypos = e.Y;
                    dPicXpos = P.Location.X;
                    dPicYpos = P.Location.Y;
                }
                else
                {
                    MessageBox.Show("Please draw Sub-Shafts beneath the surface", "Error");
                }
            }
            else if (drawtram)
            {
                if (oY > 107)
                {
                    P.Size = new Size(67, 58);
                    P.Parent = this.drawPanel;
                    //P.Image = OreFlow.Properties.Resources.Tram;
                    P.BackColor = Color.Transparent;
                    P.Location = new Point(e.X, e.Y);
                    Xpos = e.X;
                    Ypos = e.Y;
                }
                else
                {
                    MessageBox.Show("Please draw X-Trams beneath the surface", "Error");
                }

            }
            else if (draworebin)
            {
                if (oY > 107)
                {
                    P.Size = new Size(67, 58);
                    P.Parent = this.drawPanel;
                    // P.Image = OreFlow.Properties.Resources.truck;
                    P.BackColor = Color.Transparent;
                    P.Location = new Point(e.X, e.Y);
                    Xpos = e.X;
                    Ypos = e.Y;
                }
                else
                {
                    MessageBox.Show("Please draw Orebins beneath the surface", "Error");
                }
            }
            else if (drawboxhole)
            {
                if (oY > 107)
                {
                    P.Size = new Size(67, 58);
                    P.Parent = this.drawPanel;
                    //P.Image = OreFlow.Properties.Resources.Box;
                    P.BackColor = Color.Transparent;
                    P.Location = new Point(e.X, e.Y);
                    Xpos = e.X;
                    Ypos = e.Y;
                }
                else
                {
                    MessageBox.Show("Please draw Boxholes beneath the surface", "Error");
                }
            }
            else if (drawshaftline)
            {
                dsXpos = e.X;
                dsYpos = e.Y;
                ps = new Point(dsXpos, dsYpos);
            }
            else if (drawsubshaftline)
            {
                dsXpos = e.X;
                dsYpos = e.Y;
                ps = new Point(dsXpos, dsYpos);
            }
            else if (drawlevelline)
            {
                dPicXpos = e.X;
                dPicYpos = e.Y - 20;
                dsXpos = e.X;
                dsYpos = e.Y;
                ps = new Point(dsXpos, dsYpos);
            }
            else if (draworepassline)
            {
                dPicXpos = e.X;
                dPicYpos = e.Y;
                dsXpos = e.X;
                dsYpos = e.Y;
                ps = new Point(dsXpos, dsYpos);
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            drawPanel.Visible = false;

            drawPanel.Controls.Clear();
            refesh();
            lbxDrawings_SelectedValueChanged(null, null);

            drawPanel.Refresh();

            drawPanel.BackColor = Color.Transparent;
            moveControls();

            SaveBtn.Enabled = true;
            DeleteBtn.Enabled = true;

            drawPanel.Visible = true;

            // Set the form as the parent of the ShapeContainer.
            canvas.Parent = drawPanel;
            // Set the ShapeContainer as the parent of the Shape.
            theShape.Parent = canvas;
            // Set the size of the shape.
            theShape.Size = drawPanel.Size;
            // Set the location of the shape.
            theShape.Location = new System.Drawing.Point(drawPanel.Left - 223, drawPanel.Top + 60);
            // theShape.Location = new System.Drawing.Point(drawPanel.Left , drawPanel.Top + 60);
            // To draw a rounded rectangle, add the following code:
            theShape.CornerRadius = 0;
            theShape.FillStyle = FillStyle.Percent05;
            theShape.SendToBack();
            lblcount = "saved";
        }

        private void navBarControl1_NavPaneStateChanged(object sender, EventArgs e)
        {
            //if (navBarControl1.OptionsNavPane.NavPaneState == NavPaneState.Collapsed)
            //{
            //    panel1.Width = 30;
            //    drawPanel.Left = 0;
            //    theShape.Left = 0;
            //    theShape.Width = 2000;
            //}
            //else if (navBarControl1.OptionsNavPane.NavPaneState == NavPaneState.Expanded)
            //{
            //    panel1.Width = 270;
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            EditBtn_Click(null, null);
        }

        private void barDesignType_EditValueChanged(object sender, EventArgs e)
        {
            DesignType = barDesignType.EditValue.ToString();
            clsBackfillDrawing._DesignType = barDesignType.EditValue.ToString();

            if (barDesignType.EditValue.ToString() == "Oreflow")
            {
                tcMain.TabPages.Remove(tabPage3);

                tcMain.TabPages["tabPage2"].Text = "      Bookings       ";
                tcBooking.TabPages.Add(tpTrammingBook);
                tcBooking.TabPages.Add(tpHoistingBook);
                tcBooking.TabPages.Add(tpMillingBooking);

                tcBooking.TabPages.Remove(tpBackfillBooking);

                tcBooking.TabPages["tpTrammingBook"].Text = "      Tramming Booking       ";
                tcBooking.TabPages["tpHoistingBook"].Text = "      Hoisting Booking       ";
                tcBooking.TabPages["tpMillingBooking"].Text = "      Milling Booking       ";

                LoadBookingtramming();
                LoadBookingHoisting();
                LoadBookingMilling();
            }
            else
            {
                if (!OreflowBackfill)
                {
                    tcMain.TabPages.Remove(tabPage2);
                    tcMain.TabPages.Remove(tabPage3);

                    tcMain.TabPages.Add(tabPage2);
                    tcMain.TabPages.Add(tabPage3);
                    tcMain.TabPages["tabPage3"].Text = "      Reports       ";

                    tcBooking.TabPages.Remove(tpTrammingBook);
                    tcBooking.TabPages.Remove(tpHoistingBook);
                    tcBooking.TabPages.Remove(tpMillingBooking);

                    tcBooking.TabPages.Add(tpBackfillBooking);
                    tcBooking.TabPages["tpBackfillBooking"].Text = "      Backfill Booking       ";

                    pnl2Top.Visible = true;
                    BookingGrid.Dock = DockStyle.Fill;

                    LoadBookingGrid();
                    LoadProblemReport();
                }

            }

            drawPanel.Controls.Clear();
            refesh();
            LoadNavBar();
            LoadNavbarGrid();
        }

        private void addPatchukaBtn_Click(object sender, EventArgs e)
        {

        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedColumn.Caption == "Range")
            {
                frmAddBackfillBooking BfBooking = new frmAddBackfillBooking();
                BfBooking.lblRange.Text = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[0]).ToString();
                BfBooking.StartPosition = FormStartPosition.CenterScreen;
                BfBooking._theSystemDBTag = this.theSystemDBTag;
                BfBooking._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
                BfBooking.ShowDialog();

                LoadBookingGrid();
            }
        }

        private void ShowProbReportbtn_Click(object sender, EventArgs e)
        {
            LoadProblemReport();
        }

        private void ApplyCObtn_Click(object sender, EventArgs e)
        {
            UpdateCoordinates();
        }

        private void YcoTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void XcoTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void UpdateCoordinates()
        {
            int x1 = Convert.ToInt32(XcoTxt.Text);
            int x2 = Convert.ToInt32(X2coTxt.Text);
            int y1 = Convert.ToInt32(YcoTxt.Text);
            int y2 = Convert.ToInt32(Y2coTxt.Text);

            if (txtOreflowCode.Text == "lvl")
            {
                clsBackfillDrawing.UpdateLevels(txtOreflowid.Text, x1, y1, x2, y2, 0, 0);
                drawPanel.Controls.Clear();
                refesh();

                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                theShape.Size = drawPanel.Size;
                theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
                theShape.CornerRadius = 0;
                theShape.FillStyle = FillStyle.Percent05;
                theShape.Visible = true;
            }

            if (txtOreflowCode.Text == "IOrePass")
            {
                clsBackfillDrawing.UpdateInternalOrepass(txtOreflowid.Text, x1, y1, x2, y2, 0, 0);
                drawPanel.Controls.Clear();
                refesh();

                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                theShape.Size = drawPanel.Size;
                theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
                theShape.CornerRadius = 0;
                theShape.FillStyle = FillStyle.Percent05;
                theShape.Visible = true;
            }

            if (txtOreflowCode.Text == "OPass")
            {
                clsBackfillDrawing.UpdateOrePass(txtOreflowid.Text, x1, y1, x2, y2, 0, 0);
                drawPanel.Controls.Clear();
                refesh();

                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                theShape.Size = drawPanel.Size;
                theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
                theShape.CornerRadius = 0;
                theShape.FillStyle = FillStyle.Percent05;
                theShape.Visible = true;
            }

            if (txtOreflowCode.Text == "Dam")
            {
                clsBackfillDrawing.UpdateDam(txtOreflowid.Text, x1, y1, x2, y2);
                drawPanel.Controls.Clear();
                refesh();

                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                theShape.Size = drawPanel.Size;
                theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
                theShape.CornerRadius = 0;
                theShape.FillStyle = FillStyle.Percent05;
                theShape.Visible = true;
            }

            if (txtOreflowCode.Text == "Pachuca")
            {
                clsBackfillDrawing.UpdatePachuca(txtOreflowid.Text, x1, y1, x2, y2);
                drawPanel.Controls.Clear();
                refesh();

                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                theShape.Size = drawPanel.Size;
                theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
                theShape.CornerRadius = 0;
                theShape.FillStyle = FillStyle.Percent05;
                theShape.Visible = true;
            }

            if (txtOreflowCode.Text == "SubShaft")
            {
                clsBackfillDrawing.UpdateSubShafts(txtOreflowid.Text, x1, y1, x2, y2, 0, 0, 0, 0);
                drawPanel.Controls.Clear();
                refesh();

                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                theShape.Size = drawPanel.Size;
                theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
                theShape.CornerRadius = 0;
                theShape.FillStyle = FillStyle.Percent05;
                theShape.Visible = true;
            }

            if (txtOreflowCode.Text == "TerShaft")
            {
                clsBackfillDrawing.UpdateTerShafts(txtOreflowid.Text, x1, y1, x2, y2, 0, 0, 0, 0);
                drawPanel.Controls.Clear();
                refesh();

                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                theShape.Size = drawPanel.Size;
                theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
                theShape.CornerRadius = 0;
                theShape.FillStyle = FillStyle.Percent05;
                theShape.Visible = true;
            }

            if (txtOreflowCode.Text == "Tank")
            {
                clsBackfillDrawing.UpdateTank(txtOreflowid.Text, x1, y1, x2, y2);
                drawPanel.Controls.Clear();
                refesh();

                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                canvas.Parent = drawPanel;
                theShape.Parent = canvas;
                theShape.Size = drawPanel.Size;
                theShape.Location = new System.Drawing.Point(drawPanel.Left - 275, drawPanel.Top + 60);
                theShape.CornerRadius = 0;
                theShape.FillStyle = FillStyle.Percent05;
                theShape.Visible = true;
            }

            drawPanel.BringToFront();
            drawPanel.Visible = true;
        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWordEditor helpFrm = new frmWordEditor();
            helpFrm.ViewType = "View";
            helpFrm.MainCat = "Oreflow";
            //if (tabBookings.SelectedPage.Caption == "Stoping")
            //{
            //    helpFrm.SubCat = "Stoping";
            //}
            //else
            //{
                helpFrm.SubCat = "Oreflow";
            //}
            helpFrm.Show();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
