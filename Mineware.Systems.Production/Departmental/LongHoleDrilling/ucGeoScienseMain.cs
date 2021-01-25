using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.LongHoleDrilling
{
    public partial class ucGeoScienseMain : BaseUserControl
    {
        DataTable tablesum1 = new DataTable();
        DataTable UnMachine = new DataTable();
        DataTable PMachine = new DataTable();
        DataTable DrillCodes = new DataTable();
        DataTable table = new DataTable();
        string ClkCol = "";
        string FilterHole = "";
        Procedures proc = new Procedures();
        string loaded = "N";

        Report ProbHistRep = new Report();
        Report GeolBook = new Report();


        public ucGeoScienseMain()
        {
            InitializeComponent();
        }

        private void ucGeoScienseMain_Load(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.FromArgb(120, 165, 233);
            textBox2.BackColor = Color.FromArgb(255, 203, 106);
            textBox3.BackColor = Color.FromArgb(223, 0, 0);


            string mnth = "";

            if (System.DateTime.Now.Month < 10)
                mnth = "0" + Convert.ToString(System.DateTime.Now.Month);
            else
                mnth = Convert.ToString(System.DateTime.Now.Month);



            if (System.DateTime.Now.Month < 10)
                mnth = "0" + Convert.ToString(System.DateTime.Now.Month);
            else
                mnth = Convert.ToString(System.DateTime.Now.Month);

            // get can auth plan          

            Cursor = Cursors.WaitCursor;

            SEditFinYear.Text = DateTime.Now.Year.ToString();

            ProdMonthTxt.Text = Convert.ToString(Convert.ToString(System.DateTime.Now.Year) + mnth);

            Procedures procs = new Procedures();
            procs.ProdMonthVis(Convert.ToInt32(Convert.ToString(System.DateTime.Now.Year) + mnth));
            ProdMonth1Txt.Text = Procedures.Prod2;

            ProdMonthSampTxt.Text = Convert.ToString(Convert.ToString(System.DateTime.Now.Year) + mnth);
            ProdMonthSamp1Txt.Text = Procedures.Prod2;

            LoadDays();

            LoadMachines();

            //MWDataManager.clsDataAccess _dbMangg = new MWDataManager.clsDataAccess();
            //_dbMangg.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //_dbMangg.SqlStatement = "select distinct(userid) uu from [dbo].[tbl_Users_Geology] order by userid ";
            //_dbMangg.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbMangg.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbMangg.ExecuteInstruction();

            //DataTable HolesNAgg = _dbMangg.ResultsDataTable;

            //CmbBox.Items.Add("<< ALL >>");

            //foreach (DataRow row in HolesNAgg.Rows)
            //{
            //    CmbBox.Items.Add(row["uu"].ToString());
            //}

            //CmbBox.SelectedIndex = 0;

            Cursor = Cursors.Default;
           
        }

        void LoadMachines()
        {
            //loaded = "N";
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select distinct(machineno) mach from tbl_GeoScience_PlanLongTerm where machineno not in (select machineno from tbl_LongHoleDrilling_Machine)  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "order by machineno ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            UnMachine = _dbMan.ResultsDataTable;

            LstUnMachines.DataSource = UnMachine;
            LstUnMachines.DisplayMember = "mach";
            LstUnMachines.ValueMember = "mach";


            // MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select machineno mach from tbl_LongHoleDrilling_Machine  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "order by machineno ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            PMachine = _dbMan.ResultsDataTable;

            LstPMachines.DataSource = PMachine;
            LstPMachines.DisplayMember = "mach";
            LstPMachines.ValueMember = "mach";




            //// load codes
            //_dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            //_dbMan.SqlStatement = "select Code+':'+CodeDescription code from MW_PassStageDB.dbo.tbl_GeoScience_Codes ";
            //_dbMan.SqlStatement = _dbMan.SqlStatement + "order by Codeorder ";
            //_dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbMan.ExecuteInstruction();
            //DrillCodes = _dbMan.ResultsDataTable;

            //lstCodes.DataSource = DrillCodes;
            //lstCodes.DisplayMember = "code";
            //lstCodes.ValueMember = "code";


        }


        void LoadDays()
        {
            // do jan yr1  


            table.Columns.Add("MachineNo", typeof(string));
            table.Columns.Add("Workplace", typeof(string));
            table.Columns.Add("HoleNo", typeof(string));
            table.Columns.Add("PrevWorkplace", typeof(string));
            table.Columns.Add("Length", typeof(string));
            table.Columns.Add("SDate", typeof(string));
            table.Columns.Add("ADelay", typeof(string));
            // table.Columns.Add("Doc", typeof(string));


            string Addcol = "";
            //do y1m1
            for (int x = 0; x < 550; x++)
            {
                Addcol = "Col" + (x + 1);
                table.Columns.Add(Addcol, typeof(string));

                //string (Addcol, typeof(string)) = "";
            }

            // Grd18mnth.DataSource = ds.Tables[0];

            ColMach.FieldName = "MachineNo";
            ColWp.FieldName = "Workplace";
            ColHole.FieldName = "HoleNo";
            ColDep.FieldName = "PrevWorkplace";
            ColLen.FieldName = "Length";
            ColSdate.FieldName = "SDate";
            ColDuration.FieldName = "ADelay";
            // ColDuration.FieldName = "Doc";

            // Grd18mnth.Column[1].FieldName = "Col1";


            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Jan" + (x + 1);
                //gc.Caption = gc.Name;
                gc.Visible = true;
                gc.AppearanceCell.ForeColor = Color.Transparent;
                Y1M1.Columns.Add(gc);
                gc.AppearanceCell.ForeColor = Color.Transparent;
                if (x == 0) gc.FieldName = "Col1"; if (x == 1) gc.FieldName = "Col2"; if (x == 2) gc.FieldName = "Col3"; if (x == 3) gc.FieldName = "Col4"; if (x == 4) gc.FieldName = "Col5"; if (x == 5) gc.FieldName = "Col6";
                if (x == 6) gc.FieldName = "Col7"; if (x == 7) gc.FieldName = "Col8";
                if (x == 8) gc.FieldName = "Col9"; if (x == 9) gc.FieldName = "Col10"; if (x == 10) gc.FieldName = "Col11"; if (x == 11) gc.FieldName = "Col12";
                if (x == 12) gc.FieldName = "Col13"; if (x == 13) gc.FieldName = "Col14"; if (x == 14) gc.FieldName = "Col15"; if (x == 15) gc.FieldName = "Col16";
                if (x == 16) gc.FieldName = "Col17"; if (x == 17) gc.FieldName = "Col18"; if (x == 18) gc.FieldName = "Col19"; if (x == 19) gc.FieldName = "Col20";
                if (x == 20) gc.FieldName = "Col21"; if (x == 21) gc.FieldName = "Col22"; if (x == 22) gc.FieldName = "Col23"; if (x == 23) gc.FieldName = "Col24";
                if (x == 24) gc.FieldName = "Col25"; if (x == 25) gc.FieldName = "Col26"; if (x == 26) gc.FieldName = "Col27"; if (x == 27) gc.FieldName = "Col28";
                if (x == 28) gc.FieldName = "Col29"; if (x == 29) gc.FieldName = "Col30"; if (x == 30) gc.FieldName = "Col31";// if (x == 31) gc.FieldName = "Col32";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

            }


            // do  feb yr1

            for (int x = 0; x < 29; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Feb" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M2.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col32"; if (x == 1) gc.FieldName = "Col33"; if (x == 2) gc.FieldName = "Col34"; if (x == 3) gc.FieldName = "Col35";
                if (x == 4) gc.FieldName = "Col36"; if (x == 5) gc.FieldName = "Col37"; if (x == 6) gc.FieldName = "Col38"; if (x == 7) gc.FieldName = "Col39";
                if (x == 8) gc.FieldName = "Col40"; if (x == 9) gc.FieldName = "Col41"; if (x == 10) gc.FieldName = "Col42"; if (x == 11) gc.FieldName = "Col43";
                if (x == 12) gc.FieldName = "Col44"; if (x == 13) gc.FieldName = "Col45"; if (x == 14) gc.FieldName = "Col46"; if (x == 15) gc.FieldName = "Col47";
                if (x == 16) gc.FieldName = "Col48"; if (x == 17) gc.FieldName = "Col49"; if (x == 18) gc.FieldName = "Col50"; if (x == 19) gc.FieldName = "Col51";
                if (x == 20) gc.FieldName = "Col52"; if (x == 21) gc.FieldName = "Col53"; if (x == 22) gc.FieldName = "Col54"; if (x == 23) gc.FieldName = "Col55";
                if (x == 24) gc.FieldName = "Col56"; if (x == 25) gc.FieldName = "Col57"; if (x == 26) gc.FieldName = "Col58"; if (x == 27) gc.FieldName = "Col59";
                if (x == 28) gc.FieldName = "Col60"; //if (x == 29) gc.FieldName = "Col62";

                if (DTPLeep1.Value.ToString("dd") != "29")
                    if (x == 28)
                        gc.Visible = false;


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do Mar yr1

            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Mar" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M3.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col61"; if (x == 1) gc.FieldName = "Col62"; if (x == 2) gc.FieldName = "Col63"; if (x == 3) gc.FieldName = "Col64";
                if (x == 4) gc.FieldName = "Col65"; if (x == 5) gc.FieldName = "Col66"; if (x == 6) gc.FieldName = "Col67"; if (x == 7) gc.FieldName = "Col68";
                if (x == 8) gc.FieldName = "Col69"; if (x == 9) gc.FieldName = "Col70"; if (x == 10) gc.FieldName = "Col71"; if (x == 11) gc.FieldName = "Col72";
                if (x == 12) gc.FieldName = "Col73"; if (x == 13) gc.FieldName = "Col74"; if (x == 14) gc.FieldName = "Col75"; if (x == 15) gc.FieldName = "Col76";
                if (x == 16) gc.FieldName = "Col77"; if (x == 17) gc.FieldName = "Col78"; if (x == 18) gc.FieldName = "Col79"; if (x == 19) gc.FieldName = "Col80";
                if (x == 20) gc.FieldName = "Col81"; if (x == 21) gc.FieldName = "Col82"; if (x == 22) gc.FieldName = "Col83"; if (x == 23) gc.FieldName = "Col84";
                if (x == 24) gc.FieldName = "Col85"; if (x == 25) gc.FieldName = "Col86"; if (x == 26) gc.FieldName = "Col87"; if (x == 27) gc.FieldName = "Col88";
                if (x == 28) gc.FieldName = "Col89"; if (x == 29) gc.FieldName = "Col90"; if (x == 30) gc.FieldName = "Col91";// if (x == 31) gc.FieldName = "Col94";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do Apr yr1

            for (int x = 0; x < 30; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Apr" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M4.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col92"; if (x == 1) gc.FieldName = "Col93"; if (x == 2) gc.FieldName = "Col94"; if (x == 3) gc.FieldName = "Col95";
                if (x == 4) gc.FieldName = "Col96"; if (x == 5) gc.FieldName = "Col97"; if (x == 6) gc.FieldName = "Col98"; if (x == 7) gc.FieldName = "Col99";
                if (x == 8) gc.FieldName = "Col100"; if (x == 9) gc.FieldName = "Col101"; if (x == 10) gc.FieldName = "Col102"; if (x == 11) gc.FieldName = "Col103";
                if (x == 12) gc.FieldName = "Col104"; if (x == 13) gc.FieldName = "Col105"; if (x == 14) gc.FieldName = "Col106"; if (x == 15) gc.FieldName = "Col107";
                if (x == 16) gc.FieldName = "Col108"; if (x == 17) gc.FieldName = "Col109"; if (x == 18) gc.FieldName = "Col110"; if (x == 19) gc.FieldName = "Col111";
                if (x == 20) gc.FieldName = "Col112"; if (x == 21) gc.FieldName = "Col113"; if (x == 22) gc.FieldName = "Col114"; if (x == 23) gc.FieldName = "Col115";
                if (x == 24) gc.FieldName = "Col116"; if (x == 25) gc.FieldName = "Col117"; if (x == 26) gc.FieldName = "Col118"; if (x == 27) gc.FieldName = "Col119";
                if (x == 28) gc.FieldName = "Col120"; if (x == 29) gc.FieldName = "Col121"; //if (x == 30) gc.FieldName = "Col125"; 



                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }

            // do may yr1

            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1May" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M5.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col122"; if (x == 1) gc.FieldName = "Col123"; if (x == 2) gc.FieldName = "Col124"; if (x == 3) gc.FieldName = "Col125";
                if (x == 4) gc.FieldName = "Col126"; if (x == 5) gc.FieldName = "Col127"; if (x == 6) gc.FieldName = "Col128"; if (x == 7) gc.FieldName = "Col129";
                if (x == 8) gc.FieldName = "Col130"; if (x == 9) gc.FieldName = "Col131"; if (x == 10) gc.FieldName = "Col132"; if (x == 11) gc.FieldName = "Col133";
                if (x == 12) gc.FieldName = "Col134"; if (x == 13) gc.FieldName = "Col135"; if (x == 14) gc.FieldName = "Col136"; if (x == 15) gc.FieldName = "Col137";
                if (x == 16) gc.FieldName = "Col138"; if (x == 17) gc.FieldName = "Col139"; if (x == 18) gc.FieldName = "Col140"; if (x == 19) gc.FieldName = "Col141";
                if (x == 20) gc.FieldName = "Col142"; if (x == 21) gc.FieldName = "Col143"; if (x == 22) gc.FieldName = "Col144"; if (x == 23) gc.FieldName = "Col145";
                if (x == 24) gc.FieldName = "Col146"; if (x == 25) gc.FieldName = "Col147"; if (x == 26) gc.FieldName = "Col148"; if (x == 27) gc.FieldName = "Col149";
                if (x == 28) gc.FieldName = "Col150"; if (x == 29) gc.FieldName = "Col151"; if (x == 30) gc.FieldName = "Col152";// if (x == 31) gc.FieldName = "Col157";



                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do jun yr1

            for (int x = 0; x < 30; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Jun" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M6.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col153"; if (x == 1) gc.FieldName = "Col154"; if (x == 2) gc.FieldName = "Col155"; if (x == 3) gc.FieldName = "Col156";
                if (x == 4) gc.FieldName = "Col157"; if (x == 5) gc.FieldName = "Col158"; if (x == 6) gc.FieldName = "Col159"; if (x == 7) gc.FieldName = "Col160";
                if (x == 8) gc.FieldName = "Col161"; if (x == 9) gc.FieldName = "Col162"; if (x == 10) gc.FieldName = "Col163"; if (x == 11) gc.FieldName = "Col164";
                if (x == 12) gc.FieldName = "Col165"; if (x == 13) gc.FieldName = "Col166"; if (x == 14) gc.FieldName = "Col167"; if (x == 15) gc.FieldName = "Col168";
                if (x == 16) gc.FieldName = "Col169"; if (x == 17) gc.FieldName = "Col170"; if (x == 18) gc.FieldName = "Col171"; if (x == 19) gc.FieldName = "Col172";
                if (x == 20) gc.FieldName = "Col173"; if (x == 21) gc.FieldName = "Col174"; if (x == 22) gc.FieldName = "Col175"; if (x == 23) gc.FieldName = "Col176";
                if (x == 24) gc.FieldName = "Col177"; if (x == 25) gc.FieldName = "Col178"; if (x == 26) gc.FieldName = "Col179"; if (x == 27) gc.FieldName = "Col180";
                if (x == 28) gc.FieldName = "Col181"; if (x == 29) gc.FieldName = "Col182"; //if (x == 30) gc.FieldName = "Col188"; 



                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do jul yr1

            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Jul" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M7.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col183"; if (x == 1) gc.FieldName = "Col184"; if (x == 2) gc.FieldName = "Col185"; if (x == 3) gc.FieldName = "Col186";
                if (x == 4) gc.FieldName = "Col187"; if (x == 5) gc.FieldName = "Col188"; if (x == 6) gc.FieldName = "Col189"; if (x == 7) gc.FieldName = "Col190";
                if (x == 8) gc.FieldName = "Col191"; if (x == 9) gc.FieldName = "Col192"; if (x == 10) gc.FieldName = "Col193"; if (x == 11) gc.FieldName = "Col194";
                if (x == 12) gc.FieldName = "Col195"; if (x == 13) gc.FieldName = "Col196"; if (x == 14) gc.FieldName = "Col197"; if (x == 15) gc.FieldName = "Col198";
                if (x == 16) gc.FieldName = "Col199"; if (x == 17) gc.FieldName = "Col200"; if (x == 18) gc.FieldName = "Col201"; if (x == 19) gc.FieldName = "Col202";
                if (x == 20) gc.FieldName = "Col203"; if (x == 21) gc.FieldName = "Col204"; if (x == 22) gc.FieldName = "Col205"; if (x == 23) gc.FieldName = "Col206";
                if (x == 24) gc.FieldName = "Col207"; if (x == 25) gc.FieldName = "Col208"; if (x == 26) gc.FieldName = "Col209"; if (x == 27) gc.FieldName = "Col210";
                if (x == 28) gc.FieldName = "Col211"; if (x == 29) gc.FieldName = "Col212"; if (x == 30) gc.FieldName = "Col213"; //if (x == 31) gc.FieldName = "Col214";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }

            // do aug yr1

            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Aug" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M8.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col214"; if (x == 1) gc.FieldName = "Col215"; if (x == 2) gc.FieldName = "Col216"; if (x == 3) gc.FieldName = "Col217";
                if (x == 4) gc.FieldName = "Col218"; if (x == 5) gc.FieldName = "Col219"; if (x == 6) gc.FieldName = "Col220"; if (x == 7) gc.FieldName = "Col221";
                if (x == 8) gc.FieldName = "Col222"; if (x == 9) gc.FieldName = "Col223"; if (x == 10) gc.FieldName = "Col224"; if (x == 11) gc.FieldName = "Col225";
                if (x == 12) gc.FieldName = "Col226"; if (x == 13) gc.FieldName = "Col227"; if (x == 14) gc.FieldName = "Col228"; if (x == 15) gc.FieldName = "Col229";
                if (x == 16) gc.FieldName = "Col230"; if (x == 17) gc.FieldName = "Col231"; if (x == 18) gc.FieldName = "Col232"; if (x == 19) gc.FieldName = "Col233";
                if (x == 20) gc.FieldName = "Col234"; if (x == 21) gc.FieldName = "Col235"; if (x == 22) gc.FieldName = "Col236"; if (x == 23) gc.FieldName = "Col237";
                if (x == 24) gc.FieldName = "Col238"; if (x == 25) gc.FieldName = "Col239"; if (x == 26) gc.FieldName = "Col240"; if (x == 27) gc.FieldName = "Col241";
                if (x == 28) gc.FieldName = "Col242"; if (x == 29) gc.FieldName = "Col243"; if (x == 30) gc.FieldName = "Col244"; //if (x == 31) gc.FieldName = "Col252";



                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }

            // do sep yr1

            for (int x = 0; x < 30; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Sep" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M9.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col245"; if (x == 1) gc.FieldName = "Col246"; if (x == 2) gc.FieldName = "Col247"; if (x == 3) gc.FieldName = "Col248";
                if (x == 4) gc.FieldName = "Col249"; if (x == 5) gc.FieldName = "Col250"; if (x == 6) gc.FieldName = "Col251"; if (x == 7) gc.FieldName = "Col252";
                if (x == 8) gc.FieldName = "Col253"; if (x == 9) gc.FieldName = "Col254"; if (x == 10) gc.FieldName = "Col255"; if (x == 11) gc.FieldName = "Col256";
                if (x == 12) gc.FieldName = "Col257"; if (x == 13) gc.FieldName = "Col258"; if (x == 14) gc.FieldName = "Col259"; if (x == 15) gc.FieldName = "Col260";
                if (x == 16) gc.FieldName = "Col261"; if (x == 17) gc.FieldName = "Col262"; if (x == 18) gc.FieldName = "Col263"; if (x == 19) gc.FieldName = "Col264";
                if (x == 20) gc.FieldName = "Col265"; if (x == 21) gc.FieldName = "Col266"; if (x == 22) gc.FieldName = "Col267"; if (x == 23) gc.FieldName = "Col268";
                if (x == 24) gc.FieldName = "Col269"; if (x == 25) gc.FieldName = "Col270"; if (x == 26) gc.FieldName = "Col271"; if (x == 27) gc.FieldName = "Col272";
                if (x == 28) gc.FieldName = "Col273"; if (x == 29) gc.FieldName = "Col274"; //if (x == 30) gc.FieldName = "Col275";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do oct yr1

            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Oct" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M10.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col275"; if (x == 1) gc.FieldName = "Col276"; if (x == 2) gc.FieldName = "Col277"; if (x == 3) gc.FieldName = "Col278";
                if (x == 4) gc.FieldName = "Col279"; if (x == 5) gc.FieldName = "Col280"; if (x == 6) gc.FieldName = "Col281"; if (x == 7) gc.FieldName = "Col282";
                if (x == 8) gc.FieldName = "Col283"; if (x == 9) gc.FieldName = "Col284"; if (x == 10) gc.FieldName = "Col285"; if (x == 11) gc.FieldName = "Col286";
                if (x == 12) gc.FieldName = "Col287"; if (x == 13) gc.FieldName = "Col288"; if (x == 14) gc.FieldName = "Col289"; if (x == 15) gc.FieldName = "Col290";
                if (x == 16) gc.FieldName = "Col291"; if (x == 17) gc.FieldName = "Col292"; if (x == 18) gc.FieldName = "Col293"; if (x == 19) gc.FieldName = "Col294";
                if (x == 20) gc.FieldName = "Col295"; if (x == 21) gc.FieldName = "Col296"; if (x == 22) gc.FieldName = "Col297"; if (x == 23) gc.FieldName = "Col298";
                if (x == 24) gc.FieldName = "Col299"; if (x == 25) gc.FieldName = "Col300"; if (x == 26) gc.FieldName = "Col301"; if (x == 27) gc.FieldName = "Col302";
                if (x == 28) gc.FieldName = "Col303"; if (x == 29) gc.FieldName = "Col304"; if (x == 30) gc.FieldName = "Col305"; //if (x == 31) gc.FieldName = "Col315";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }

            // do nov yr1

            for (int x = 0; x < 30; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Nov" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M11.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col306"; if (x == 1) gc.FieldName = "Col307"; if (x == 2) gc.FieldName = "Col308"; if (x == 3) gc.FieldName = "Col309";
                if (x == 4) gc.FieldName = "Col310"; if (x == 5) gc.FieldName = "Col311"; if (x == 6) gc.FieldName = "Col312"; if (x == 7) gc.FieldName = "Col313";
                if (x == 8) gc.FieldName = "Col314"; if (x == 9) gc.FieldName = "Col315"; if (x == 10) gc.FieldName = "Col316"; if (x == 11) gc.FieldName = "Col317";
                if (x == 12) gc.FieldName = "Col318"; if (x == 13) gc.FieldName = "Col319"; if (x == 14) gc.FieldName = "Col320"; if (x == 15) gc.FieldName = "Col321";
                if (x == 16) gc.FieldName = "Col322"; if (x == 17) gc.FieldName = "Col323"; if (x == 18) gc.FieldName = "Col324"; if (x == 19) gc.FieldName = "Col325";
                if (x == 20) gc.FieldName = "Col326"; if (x == 21) gc.FieldName = "Col327"; if (x == 22) gc.FieldName = "Col328"; if (x == 23) gc.FieldName = "Col329";
                if (x == 24) gc.FieldName = "Col330"; if (x == 25) gc.FieldName = "Col331"; if (x == 26) gc.FieldName = "Col332"; if (x == 27) gc.FieldName = "Col333";
                if (x == 28) gc.FieldName = "Col334"; if (x == 29) gc.FieldName = "Col335";// if (x == 30) gc.FieldName = "Col346"; 

                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do dec yr1

            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr1Dec" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y1M12.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col336"; if (x == 1) gc.FieldName = "Col337"; if (x == 2) gc.FieldName = "Col338"; if (x == 3) gc.FieldName = "Col339";
                if (x == 4) gc.FieldName = "Col340"; if (x == 5) gc.FieldName = "Col341"; if (x == 6) gc.FieldName = "Col342"; if (x == 7) gc.FieldName = "Col343";
                if (x == 8) gc.FieldName = "Col344"; if (x == 9) gc.FieldName = "Col345"; if (x == 10) gc.FieldName = "Col346"; if (x == 11) gc.FieldName = "Col347";
                if (x == 12) gc.FieldName = "Col348"; if (x == 13) gc.FieldName = "Col349"; if (x == 14) gc.FieldName = "Col350"; if (x == 15) gc.FieldName = "Col351";
                if (x == 16) gc.FieldName = "Col352"; if (x == 17) gc.FieldName = "Col353"; if (x == 18) gc.FieldName = "Col354"; if (x == 19) gc.FieldName = "Col355";
                if (x == 20) gc.FieldName = "Col356"; if (x == 21) gc.FieldName = "Col357"; if (x == 22) gc.FieldName = "Col358"; if (x == 23) gc.FieldName = "Col359";
                if (x == 24) gc.FieldName = "Col360"; if (x == 25) gc.FieldName = "Col361"; if (x == 26) gc.FieldName = "Col362"; if (x == 27) gc.FieldName = "Col363";
                if (x == 28) gc.FieldName = "Col364"; if (x == 29) gc.FieldName = "Col365"; if (x == 30) gc.FieldName = "Col366"; //if (x == 31) gc.FieldName = "Col367";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // year2
            // do jan yr2

            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr2Jan" + (x + 1);
                //gc.Caption = gc.Name;
                gc.Visible = true;
                Y2M1.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col367"; if (x == 1) gc.FieldName = "Col368"; if (x == 2) gc.FieldName = "Col369"; if (x == 3) gc.FieldName = "Col370";
                if (x == 4) gc.FieldName = "Col371"; if (x == 5) gc.FieldName = "Col372"; if (x == 6) gc.FieldName = "Col373"; if (x == 7) gc.FieldName = "Col374";
                if (x == 8) gc.FieldName = "Col375"; if (x == 9) gc.FieldName = "Col376"; if (x == 10) gc.FieldName = "Col377"; if (x == 11) gc.FieldName = "Col378";
                if (x == 12) gc.FieldName = "Col379"; if (x == 13) gc.FieldName = "Col380"; if (x == 14) gc.FieldName = "Col381"; if (x == 15) gc.FieldName = "Col382";
                if (x == 16) gc.FieldName = "Col383"; if (x == 17) gc.FieldName = "Col384"; if (x == 18) gc.FieldName = "Col385"; if (x == 19) gc.FieldName = "Col386";
                if (x == 20) gc.FieldName = "Col387"; if (x == 21) gc.FieldName = "Col388"; if (x == 22) gc.FieldName = "Col389"; if (x == 23) gc.FieldName = "Col390";
                if (x == 24) gc.FieldName = "Col391"; if (x == 25) gc.FieldName = "Col392"; if (x == 26) gc.FieldName = "Col393"; if (x == 27) gc.FieldName = "Col394";
                if (x == 28) gc.FieldName = "Col395"; if (x == 29) gc.FieldName = "Col396"; if (x == 30) gc.FieldName = "Col397"; //if (x == 31) gc.FieldName = "Col410";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do  feb yr2

            for (int x = 0; x < 29; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr2Feb" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y2M2.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col398"; if (x == 1) gc.FieldName = "Col399"; if (x == 2) gc.FieldName = "Col400"; if (x == 3) gc.FieldName = "Col401";
                if (x == 4) gc.FieldName = "Col402"; if (x == 5) gc.FieldName = "Col403"; if (x == 6) gc.FieldName = "Col404"; if (x == 7) gc.FieldName = "Col405";
                if (x == 8) gc.FieldName = "Col406"; if (x == 9) gc.FieldName = "Col407"; if (x == 10) gc.FieldName = "Col408"; if (x == 11) gc.FieldName = "Col409";
                if (x == 12) gc.FieldName = "Col410"; if (x == 13) gc.FieldName = "Col411"; if (x == 14) gc.FieldName = "Col412"; if (x == 15) gc.FieldName = "Col413";
                if (x == 16) gc.FieldName = "Col414"; if (x == 17) gc.FieldName = "Col415"; if (x == 18) gc.FieldName = "Col416"; if (x == 19) gc.FieldName = "Col417";
                if (x == 20) gc.FieldName = "Col418"; if (x == 21) gc.FieldName = "Col419"; if (x == 22) gc.FieldName = "Col420"; if (x == 23) gc.FieldName = "Col421";
                if (x == 24) gc.FieldName = "Col422"; if (x == 25) gc.FieldName = "Col423"; if (x == 26) gc.FieldName = "Col424"; if (x == 27) gc.FieldName = "Col425";
                if (x == 28) gc.FieldName = "Col426";// if (x == 29) gc.FieldName = "Col427"; 

                if (DTPLeep2.Value.ToString("dd") != "29")
                    if (x == 28)
                        gc.Visible = false;

                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do Mar yr2

            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr2Mar" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y2M3.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col427"; if (x == 1) gc.FieldName = "Col428"; if (x == 2) gc.FieldName = "Col429"; if (x == 3) gc.FieldName = "Col430";
                if (x == 4) gc.FieldName = "Col431"; if (x == 5) gc.FieldName = "Col432"; if (x == 6) gc.FieldName = "Col433"; if (x == 7) gc.FieldName = "Col434";
                if (x == 8) gc.FieldName = "Col435"; if (x == 9) gc.FieldName = "Col436"; if (x == 10) gc.FieldName = "Col437"; if (x == 11) gc.FieldName = "Col438";
                if (x == 12) gc.FieldName = "Col439"; if (x == 13) gc.FieldName = "Col440"; if (x == 14) gc.FieldName = "Col441"; if (x == 15) gc.FieldName = "Col442";
                if (x == 16) gc.FieldName = "Col443"; if (x == 17) gc.FieldName = "Col444"; if (x == 18) gc.FieldName = "Col445"; if (x == 19) gc.FieldName = "Col446";
                if (x == 20) gc.FieldName = "Col447"; if (x == 21) gc.FieldName = "Col448"; if (x == 22) gc.FieldName = "Col449"; if (x == 23) gc.FieldName = "Col450";
                if (x == 24) gc.FieldName = "Col451"; if (x == 25) gc.FieldName = "Col452"; if (x == 26) gc.FieldName = "Col453"; if (x == 27) gc.FieldName = "Col454";
                if (x == 28) gc.FieldName = "Col455"; if (x == 29) gc.FieldName = "Col456"; if (x == 30) gc.FieldName = "Col457"; if (x == 31) gc.FieldName = "Col458";



                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do Apr yr2

            for (int x = 0; x < 30; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr2Apr" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y2M4.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col459"; if (x == 1) gc.FieldName = "Col460"; if (x == 2) gc.FieldName = "Col461"; if (x == 3) gc.FieldName = "Col462";
                if (x == 4) gc.FieldName = "Col463"; if (x == 5) gc.FieldName = "Col464"; if (x == 6) gc.FieldName = "Col465"; if (x == 7) gc.FieldName = "Col466";
                if (x == 8) gc.FieldName = "Col467"; if (x == 9) gc.FieldName = "Col468"; if (x == 10) gc.FieldName = "Col469"; if (x == 11) gc.FieldName = "Col470";
                if (x == 12) gc.FieldName = "Col471"; if (x == 13) gc.FieldName = "Col472"; if (x == 14) gc.FieldName = "Col473"; if (x == 15) gc.FieldName = "Col474";
                if (x == 16) gc.FieldName = "Col475"; if (x == 17) gc.FieldName = "Col476"; if (x == 18) gc.FieldName = "Col477"; if (x == 19) gc.FieldName = "Col478";
                if (x == 20) gc.FieldName = "Col479"; if (x == 21) gc.FieldName = "Col480"; if (x == 22) gc.FieldName = "Col481"; if (x == 23) gc.FieldName = "Col482";
                if (x == 24) gc.FieldName = "Col483"; if (x == 25) gc.FieldName = "Col484"; if (x == 26) gc.FieldName = "Col485"; if (x == 27) gc.FieldName = "Col486";
                if (x == 28) gc.FieldName = "Col487"; if (x == 29) gc.FieldName = "Col488";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }

            // do may yr2

            for (int x = 0; x < 31; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr2May" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y2M5.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col489"; if (x == 1) gc.FieldName = "Col490"; if (x == 2) gc.FieldName = "Col491"; if (x == 3) gc.FieldName = "Col492";
                if (x == 4) gc.FieldName = "Col493"; if (x == 5) gc.FieldName = "Col494"; if (x == 6) gc.FieldName = "Col495"; if (x == 7) gc.FieldName = "Col496";
                if (x == 8) gc.FieldName = "Col497"; if (x == 9) gc.FieldName = "Col498"; if (x == 10) gc.FieldName = "Col499"; if (x == 11) gc.FieldName = "Col500";
                if (x == 12) gc.FieldName = "Col501"; if (x == 13) gc.FieldName = "Col502"; if (x == 14) gc.FieldName = "Col503"; if (x == 15) gc.FieldName = "Col504";
                if (x == 16) gc.FieldName = "Col505"; if (x == 17) gc.FieldName = "Col506"; if (x == 18) gc.FieldName = "Col507"; if (x == 19) gc.FieldName = "Col508";
                if (x == 20) gc.FieldName = "Col509"; if (x == 21) gc.FieldName = "Col510"; if (x == 22) gc.FieldName = "Col511"; if (x == 23) gc.FieldName = "Col512";
                if (x == 24) gc.FieldName = "Col513"; if (x == 25) gc.FieldName = "Col514"; if (x == 26) gc.FieldName = "Col515"; if (x == 27) gc.FieldName = "Col516";
                if (x == 28) gc.FieldName = "Col517"; if (x == 29) gc.FieldName = "Col518"; if (x == 30) gc.FieldName = "Col519";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }


            // do jun yr1

            for (int x = 0; x < 30; x++)
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                gc.Name = "Yr2Jun" + (x + 1);
                gc.Caption = x.ToString();
                gc.Visible = true;
                Y2M6.Columns.Add(gc);

                if (x == 0) gc.FieldName = "Col520"; if (x == 1) gc.FieldName = "Col521"; if (x == 2) gc.FieldName = "Col522"; if (x == 3) gc.FieldName = "Col523";
                if (x == 4) gc.FieldName = "Col524"; if (x == 5) gc.FieldName = "Col525"; if (x == 6) gc.FieldName = "Col526"; if (x == 7) gc.FieldName = "Col527";
                if (x == 8) gc.FieldName = "Col528"; if (x == 9) gc.FieldName = "Col529"; if (x == 10) gc.FieldName = "Col530"; if (x == 11) gc.FieldName = "Col531";
                if (x == 12) gc.FieldName = "Col532"; if (x == 13) gc.FieldName = "Col533"; if (x == 14) gc.FieldName = "Col534"; if (x == 15) gc.FieldName = "Col535";
                if (x == 16) gc.FieldName = "Col536"; if (x == 17) gc.FieldName = "Col537"; if (x == 18) gc.FieldName = "Col538"; if (x == 19) gc.FieldName = "Col539";
                if (x == 20) gc.FieldName = "Col540"; if (x == 21) gc.FieldName = "Col541"; if (x == 22) gc.FieldName = "Col542"; if (x == 23) gc.FieldName = "Col543";
                if (x == 24) gc.FieldName = "Col544"; if (x == 25) gc.FieldName = "Col545"; if (x == 26) gc.FieldName = "Col546"; if (x == 27) gc.FieldName = "Col547";
                if (x == 28) gc.FieldName = "Col548"; if (x == 29) gc.FieldName = "Col549";


                gc.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            }



            Line1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

            FieldInfo fi = typeof(GridColumn).GetField("minWidth", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (GridColumn col in bandedGridView1.Columns)
            {
                string aa = col.Name;
                fi.SetValue(col, 4);

                if (aa == ColMach.Name)
                    fi.SetValue(col, 80);

                if (aa == ColWp.Name)
                    fi.SetValue(col, 150);

                if (aa == ColHole.Name)
                    fi.SetValue(col, 80);

                if (aa == ColLen.Name)
                    fi.SetValue(col, 50);

                if (aa == ColSdate.Name)
                    fi.SetValue(col, 80);

                if (aa == ColDuration.Name)
                    fi.SetValue(col, 50);

                if (aa == ColDep.Name)
                    fi.SetValue(col, 80);

                if (aa == Line1.Name)
                    fi.SetValue(col, 2);
            }

            for (int x = 0; x < bandedGridView1.Columns.Count; x++)
            {
                bandedGridView1.Columns[x].Width = 20;

            }


            ColMach.Width = 200;



        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            PnlMachine.Visible = false;
            //panel4.Visible = true;
            //SamplePnl.Visible = false;
            LoadUnassigned();
            newPnl.Visible = true;
            newPnl.Dock = DockStyle.Fill;
            //Pnl3Month.Visible = false;

            SEditFinYear.Visible = true;
            panel8.Visible = true;

            gridBand2.Caption = SEditFinYear.Text;

            gridBand3.Caption = (Convert.ToInt16(SEditFinYear.Text) + 1).ToString();


            gridBand20.Caption = SEditFinYear.Text;

            gridBand34.Caption = (Convert.ToInt16(SEditFinYear.Text) + 1).ToString();


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select  * from  ( select a.*, case when letter.date is null then 1 else 0 end as document, ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "case when comp is null then 'N' else 'Y' end as comp, case when dd is null then getdate()+20000 else dd end as lastbook  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "from (select a.*, b.MachineNo, b.advpershift from (select nodeid, MachineNo mm, Workplace, HoleNo, Length, SDate, PrevWorkplace,  AddDelay, ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "CONVERT(VARCHAR(11),SDate,106)  sss from [dbo].[tbl_GeoScience_PlanLongTerm] where sdate is not null ";

            if (FilterHole == "NotAll")
            {

                _dbMan.SqlStatement = _dbMan.SqlStatement + "and machineno in (select distinct(machine) a ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "from tbl_Users_geology where userid = '" + TUserInfo.UserID + "'  and machine <> '<< ALL >>') ";

            }



            _dbMan.SqlStatement = _dbMan.SqlStatement + ") a, [tbl_GeoScience_Machine] b where a.mm = b.MachineNo union select '9999999999', '' , '' , '' , null, null, null, null, null, 'z', null  ";


            _dbMan.SqlStatement = _dbMan.SqlStatement + ") a left outer join tbl_GeoScience_Letter letter ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "on a.mm = letter.[MachineNo] and a.workplace = letter.workplace and a.holeno = letter.holeno ";

            _dbMan.SqlStatement = _dbMan.SqlStatement + "left outer join ";

            _dbMan.SqlStatement = _dbMan.SqlStatement + "(select nodeid Comp from tbl_GeoScience_CompletedHoles) comp on a.nodeid = comp.comp ";

            _dbMan.SqlStatement = _dbMan.SqlStatement + "left outer join  ";

            _dbMan.SqlStatement = _dbMan.SqlStatement + "(select nodeid znode, max(thedate) dd from [dbo].[tbl_GeoScienceDayPlanBook] ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "where dsfrom is not null group by nodeid) ll on a.nodeid = ll.znode ";

            _dbMan.SqlStatement = _dbMan.SqlStatement + ") a where ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "(comp = 'N' or lastbook > '" + String.Format("{0:yyyy-MM-dd}", DTPStart.Value) + "' ) ";


            _dbMan.SqlStatement = _dbMan.SqlStatement + "order by a.machineno, a.SDate,  a.holeno ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable Holes = _dbMan.ResultsDataTable;

            // create ds

            DataTable table = new DataTable();
            table.Columns.Add("MachineNo", typeof(string));
            table.Columns.Add("Workplace", typeof(string));
            table.Columns.Add("HoleNo", typeof(string));
            table.Columns.Add("PrevWorkplace", typeof(string));
            table.Columns.Add("Length", typeof(string));
            table.Columns.Add("SDate", typeof(string));
            table.Columns.Add("ADelay", typeof(string));
            // table.Columns.Add("Doc", typeof(string));


            string Addcol = "";
            //do y1m1
            for (int x = 0; x < 570; x++)
            {
                Addcol = "Col" + (x + 1);
                table.Columns.Add(Addcol, typeof(string));


            }


            // get workingdays
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = "select ";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + "* from [dbo].[CALTYPE] ";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + "where calendarcode = 'Geo-Drill' ";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + "and workingday = 'N' and calendardate >= '" + String.Format("{0:yyyy-MM-dd}", DTPStart.Value) + "' ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable DaysOff = _dbMan1.ResultsDataTable;

            TimeSpan Span;
            int Span1 = 0;

            TimeSpan Lbook;
            int LBookInt = 0;


            // start query for update
            MWDataManager.clsDataAccess _dbManUpdate = new MWDataManager.clsDataAccess();
            _dbManUpdate.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManUpdate.SqlStatement = " ";
            _dbManUpdate.SqlStatement = _dbManUpdate.SqlStatement + "  ";

            foreach (DataRow r in Holes.Rows)
            {
                string Col1 = "z"; string Col2 = "z"; string Col3 = "z"; string Col4 = "z"; string Col5 = "z"; string Col6 = "z"; string Col7 = "z"; string Col8 = "z"; string Col9 = "z"; string Col10 = "z";
                string Col11 = "z"; string Col12 = "z"; string Col13 = "z"; string Col14 = "z"; string Col15 = "z"; string Col16 = "z"; string Col17 = "z"; string Col18 = "z"; string Col19 = "z"; string Col20 = "z";
                string Col21 = "z"; string Col22 = "z"; string Col23 = "z"; string Col24 = "z"; string Col25 = "z"; string Col26 = "z"; string Col27 = "z"; string Col28 = "z"; string Col29 = "z"; string Col30 = "z";
                string Col31 = "z"; string Col32 = "z"; string Col33 = "z"; string Col34 = "z"; string Col35 = "z"; string Col36 = "z"; string Col37 = "z"; string Col38 = "z"; string Col39 = "z"; string Col40 = "z";
                string Col41 = "z"; string Col42 = "z"; string Col43 = "z"; string Col44 = "z"; string Col45 = "z"; string Col46 = "z"; string Col47 = "z"; string Col48 = "z"; string Col49 = "z"; string Col50 = "z";
                string Col51 = "z"; string Col52 = "z"; string Col53 = "z"; string Col54 = "z"; string Col55 = "z"; string Col56 = "z"; string Col57 = "z"; string Col58 = "z"; string Col59 = "z"; string Col60 = "z";
                string Col61 = "z"; string Col62 = "z";

                string Col63 = "z"; string Col64 = "z"; string Col65 = "z"; string Col66 = "z"; string Col67 = "z"; string Col68 = "z"; string Col69 = "z"; string Col70 = "z";
                string Col71 = "z"; string Col72 = "z"; string Col73 = "z"; string Col74 = "z"; string Col75 = "z"; string Col76 = "z"; string Col77 = "c"; string Col78 = "z"; string Col79 = "z"; string Col80 = "z";
                string Col81 = "z"; string Col82 = "z"; string Col83 = "z"; string Col84 = "z"; string Col85 = "z"; string Col86 = "z"; string Col87 = "z"; string Col88 = "z"; string Col89 = "z"; string Col90 = "z";
                string Col91 = "z"; string Col92 = "z"; string Col93 = "z"; string Col94 = "z"; string Col95 = "z"; string Col96 = "z"; string Col97 = "z"; string Col98 = "z"; string Col99 = "z"; string Col100 = "z";

                string Col101 = ""; string Col102 = ""; string Col103 = ""; string Col104 = ""; string Col105 = ""; string Col106 = ""; string Col107 = ""; string Col108 = ""; string Col109 = ""; string Col110 = "";
                string Col111 = ""; string Col112 = ""; string Col113 = ""; string Col114 = ""; string Col115 = ""; string Col116 = ""; string Col117 = ""; string Col118 = ""; string Col119 = ""; string Col120 = "";
                string Col121 = ""; string Col122 = ""; string Col123 = ""; string Col124 = ""; string Col125 = ""; string Col126 = ""; string Col127 = ""; string Col128 = ""; string Col129 = ""; string Col130 = "";
                string Col131 = ""; string Col132 = ""; string Col133 = ""; string Col134 = ""; string Col135 = ""; string Col136 = ""; string Col137 = ""; string Col138 = ""; string Col139 = ""; string Col140 = "";
                string Col141 = ""; string Col142 = ""; string Col143 = ""; string Col144 = ""; string Col145 = ""; string Col146 = ""; string Col147 = ""; string Col148 = ""; string Col149 = ""; string Col150 = "";
                string Col151 = ""; string Col152 = ""; string Col153 = ""; string Col154 = ""; string Col155 = ""; string Col156 = ""; string Col157 = ""; string Col158 = ""; string Col159 = ""; string Col160 = "";
                string Col161 = ""; string Col162 = ""; string Col163 = ""; string Col164 = ""; string Col165 = ""; string Col166 = ""; string Col167 = ""; string Col168 = ""; string Col169 = ""; string Col170 = "";
                string Col171 = ""; string Col172 = ""; string Col173 = ""; string Col174 = ""; string Col175 = ""; string Col176 = ""; string Col177 = ""; string Col178 = ""; string Col179 = ""; string Col180 = "";
                string Col181 = ""; string Col182 = ""; string Col183 = ""; string Col184 = ""; string Col185 = ""; string Col186 = ""; string Col187 = ""; string Col188 = ""; string Col189 = ""; string Col190 = "";
                string Col191 = ""; string Col192 = ""; string Col193 = ""; string Col194 = ""; string Col195 = ""; string Col196 = ""; string Col197 = ""; string Col198 = ""; string Col199 = ""; string Col200 = "";

                string Col201 = ""; string Col202 = ""; string Col203 = ""; string Col204 = ""; string Col205 = ""; string Col206 = ""; string Col207 = ""; string Col208 = ""; string Col209 = ""; string Col210 = "";
                string Col211 = ""; string Col212 = ""; string Col213 = ""; string Col214 = ""; string Col215 = ""; string Col216 = ""; string Col217 = ""; string Col218 = ""; string Col219 = ""; string Col220 = "";
                string Col221 = ""; string Col222 = ""; string Col223 = ""; string Col224 = ""; string Col225 = ""; string Col226 = ""; string Col227 = ""; string Col228 = ""; string Col229 = ""; string Col230 = "";
                string Col231 = ""; string Col232 = ""; string Col233 = ""; string Col234 = ""; string Col235 = ""; string Col236 = ""; string Col237 = ""; string Col238 = ""; string Col239 = ""; string Col240 = "";
                string Col241 = ""; string Col242 = ""; string Col243 = ""; string Col244 = ""; string Col245 = ""; string Col246 = ""; string Col247 = ""; string Col248 = ""; string Col249 = ""; string Col250 = "";
                string Col251 = ""; string Col252 = ""; string Col253 = ""; string Col254 = ""; string Col255 = ""; string Col256 = ""; string Col257 = ""; string Col258 = ""; string Col259 = ""; string Col260 = "";
                string Col261 = ""; string Col262 = ""; string Col263 = ""; string Col264 = ""; string Col265 = ""; string Col266 = ""; string Col267 = ""; string Col268 = ""; string Col269 = ""; string Col270 = "";
                string Col271 = ""; string Col272 = ""; string Col273 = ""; string Col274 = ""; string Col275 = ""; string Col276 = ""; string Col277 = ""; string Col278 = ""; string Col279 = ""; string Col280 = "";
                string Col281 = ""; string Col282 = ""; string Col283 = ""; string Col284 = ""; string Col285 = ""; string Col286 = ""; string Col287 = ""; string Col288 = ""; string Col289 = ""; string Col290 = "";
                string Col291 = ""; string Col292 = ""; string Col293 = ""; string Col294 = ""; string Col295 = ""; string Col296 = ""; string Col297 = ""; string Col298 = ""; string Col299 = ""; string Col300 = "";

                string Col301 = ""; string Col302 = ""; string Col303 = ""; string Col304 = ""; string Col305 = ""; string Col306 = ""; string Col307 = ""; string Col308 = ""; string Col309 = ""; string Col310 = "";
                string Col311 = ""; string Col312 = ""; string Col313 = ""; string Col314 = ""; string Col315 = ""; string Col316 = ""; string Col317 = ""; string Col318 = ""; string Col319 = ""; string Col320 = "";
                string Col321 = ""; string Col322 = ""; string Col323 = ""; string Col324 = ""; string Col325 = ""; string Col326 = ""; string Col327 = ""; string Col328 = ""; string Col329 = ""; string Col330 = "";
                string Col331 = ""; string Col332 = ""; string Col333 = ""; string Col334 = ""; string Col335 = ""; string Col336 = ""; string Col337 = ""; string Col338 = ""; string Col339 = ""; string Col340 = "";
                string Col341 = ""; string Col342 = ""; string Col343 = ""; string Col344 = ""; string Col345 = ""; string Col346 = ""; string Col347 = ""; string Col348 = ""; string Col349 = ""; string Col350 = "";
                string Col351 = ""; string Col352 = ""; string Col353 = ""; string Col354 = ""; string Col355 = ""; string Col356 = ""; string Col357 = ""; string Col358 = ""; string Col359 = ""; string Col360 = "";
                string Col361 = ""; string Col362 = ""; string Col363 = ""; string Col364 = ""; string Col365 = ""; string Col366 = ""; string Col367 = ""; string Col368 = ""; string Col369 = ""; string Col370 = "";
                string Col371 = ""; string Col372 = ""; string Col373 = ""; string Col374 = ""; string Col375 = ""; string Col376 = ""; string Col377 = ""; string Col378 = ""; string Col379 = ""; string Col380 = "";
                string Col381 = ""; string Col382 = ""; string Col383 = ""; string Col384 = ""; string Col385 = ""; string Col386 = ""; string Col387 = ""; string Col388 = ""; string Col389 = ""; string Col390 = "";
                string Col391 = ""; string Col392 = ""; string Col393 = ""; string Col394 = ""; string Col395 = ""; string Col396 = ""; string Col397 = ""; string Col398 = ""; string Col399 = ""; string Col400 = "";

                string Col401 = ""; string Col402 = ""; string Col403 = ""; string Col404 = ""; string Col405 = ""; string Col406 = ""; string Col407 = ""; string Col408 = ""; string Col409 = ""; string Col410 = "";
                string Col411 = ""; string Col412 = ""; string Col413 = ""; string Col414 = ""; string Col415 = ""; string Col416 = ""; string Col417 = ""; string Col418 = ""; string Col419 = ""; string Col420 = "";
                string Col421 = ""; string Col422 = ""; string Col423 = ""; string Col424 = ""; string Col425 = ""; string Col426 = ""; string Col427 = ""; string Col428 = ""; string Col429 = ""; string Col430 = "";
                string Col431 = ""; string Col432 = ""; string Col433 = ""; string Col434 = ""; string Col435 = ""; string Col436 = ""; string Col437 = ""; string Col438 = ""; string Col439 = ""; string Col440 = "";
                string Col441 = ""; string Col442 = ""; string Col443 = ""; string Col444 = ""; string Col445 = ""; string Col446 = ""; string Col447 = ""; string Col448 = ""; string Col449 = ""; string Col450 = "";
                string Col451 = ""; string Col452 = ""; string Col453 = ""; string Col454 = ""; string Col455 = ""; string Col456 = ""; string Col457 = ""; string Col458 = ""; string Col459 = ""; string Col460 = "";
                string Col461 = ""; string Col462 = ""; string Col463 = ""; string Col464 = ""; string Col465 = ""; string Col466 = ""; string Col467 = ""; string Col468 = ""; string Col469 = ""; string Col470 = "";
                string Col471 = ""; string Col472 = ""; string Col473 = ""; string Col474 = ""; string Col475 = ""; string Col476 = ""; string Col477 = ""; string Col478 = ""; string Col479 = ""; string Col480 = "";
                string Col481 = ""; string Col482 = ""; string Col483 = ""; string Col484 = ""; string Col485 = ""; string Col486 = ""; string Col487 = ""; string Col488 = ""; string Col489 = ""; string Col490 = "";
                string Col491 = ""; string Col492 = ""; string Col493 = ""; string Col494 = ""; string Col495 = ""; string Col496 = ""; string Col497 = ""; string Col498 = ""; string Col499 = ""; string Col500 = "";

                string Col501 = ""; string Col502 = ""; string Col503 = ""; string Col504 = ""; string Col505 = ""; string Col506 = ""; string Col507 = ""; string Col508 = "";
                string Col509 = ""; string Col510 = ""; string Col511 = ""; string Col512 = ""; string Col513 = ""; string Col514 = ""; string Col515 = ""; string Col516 = "";
                string Col517 = ""; string Col518 = ""; string Col519 = ""; string Col520 = ""; string Col521 = ""; string Col522 = ""; string Col523 = ""; string Col524 = "";
                string Col525 = ""; string Col526 = ""; string Col527 = ""; string Col528 = ""; string Col529 = ""; string Col530 = ""; string Col531 = ""; string Col532 = "";
                string Col533 = ""; string Col534 = ""; string Col535 = ""; string Col536 = ""; string Col537 = ""; string Col538 = ""; string Col539 = ""; string Col540 = "";
                string Col541 = ""; string Col542 = ""; string Col543 = ""; string Col544 = ""; string Col545 = ""; string Col546 = ""; string Col547 = ""; string Col548 = "";
                string Col549 = "";

                decimal Jan1 = 0;
                decimal Feb1 = 0;
                decimal Mar1 = 0;
                decimal Apr1 = 0;
                decimal May1 = 0;
                decimal Jun1 = 0;
                decimal Jul1 = 0;
                decimal Aug1 = 0;
                decimal Sep1 = 0;
                decimal Oct1 = 0;
                decimal Nov1 = 0;
                decimal Dec1 = 0;
                decimal Jan2 = 0;
                decimal Feb2 = 0;
                decimal Mar2 = 0;
                decimal Apr2 = 0;
                decimal May2 = 0;
                decimal Jun2 = 0;

                int dur = 0;

                DateTime EndDate = DateTime.Now;
                DateTime StartDate = DateTime.Now;

                foreach (DataRow rr in DaysOff.Rows)
                {
                    Span = Convert.ToDateTime(rr["calendardate"].ToString()).Subtract(DTPStart.Value);
                    Span1 = Convert.ToInt32(Span.Days) + 1;


                    if (DTPLeep1.Value.ToString("dd") != "29")
                    {
                        Col60 = "O";
                        if (Span1 > 59)
                            Span1 = Span1 + 1;
                    }


                    if (DTPLeep2.Value.ToString("dd") != "29")
                    {
                        Col426 = "O";
                        if (Span1 > 425)
                            Span1 = Span1 + 1;
                    }


                    if (Span1 == 1) Col1 = "O"; if (Span1 == 2) Col2 = "O"; if (Span1 == 3) Col3 = "O"; if (Span1 == 4) Col4 = "O"; if (Span1 == 5) Col5 = "O";
                    if (Span1 == 6) Col6 = "O"; if (Span1 == 7) Col7 = "O"; if (Span1 == 8) Col8 = "O"; if (Span1 == 9) Col9 = "O"; if (Span1 == 10) Col10 = "O";

                    if (Span1 == 11) Col11 = "O"; if (Span1 == 12) Col12 = "O"; if (Span1 == 13) Col13 = "O"; if (Span1 == 14) Col14 = "O"; if (Span1 == 15) Col15 = "O";
                    if (Span1 == 16) Col16 = "O"; if (Span1 == 17) Col17 = "O"; if (Span1 == 18) Col18 = "O"; if (Span1 == 19) Col19 = "O"; if (Span1 == 20) Col20 = "O";

                    if (Span1 == 21) Col21 = "O"; if (Span1 == 22) Col22 = "O"; if (Span1 == 23) Col23 = "O"; if (Span1 == 24) Col24 = "O"; if (Span1 == 25) Col25 = "O";
                    if (Span1 == 26) Col26 = "O"; if (Span1 == 27) Col27 = "O"; if (Span1 == 28) Col28 = "O"; if (Span1 == 29) Col29 = "O"; if (Span1 == 30) Col30 = "O";

                    if (Span1 == 31) Col31 = "O"; if (Span1 == 32) Col32 = "O"; if (Span1 == 33) Col33 = "O"; if (Span1 == 34) Col34 = "O"; if (Span1 == 35) Col35 = "O";
                    if (Span1 == 36) Col36 = "O"; if (Span1 == 37) Col37 = "O"; if (Span1 == 38) Col38 = "O"; if (Span1 == 39) Col39 = "O"; if (Span1 == 40) Col40 = "O";

                    if (Span1 == 41) Col41 = "O"; if (Span1 == 42) Col42 = "O"; if (Span1 == 43) Col43 = "O"; if (Span1 == 44) Col44 = "O"; if (Span1 == 45) Col45 = "O";
                    if (Span1 == 46) Col46 = "O"; if (Span1 == 47) Col47 = "O"; if (Span1 == 48) Col48 = "O"; if (Span1 == 49) Col49 = "O"; if (Span1 == 50) Col50 = "O";

                    if (Span1 == 51) Col51 = "O"; if (Span1 == 52) Col52 = "O"; if (Span1 == 53) Col53 = "O"; if (Span1 == 54) Col54 = "O"; if (Span1 == 55) Col55 = "O";
                    if (Span1 == 56) Col56 = "O"; if (Span1 == 57) Col57 = "O"; if (Span1 == 58) Col58 = "O"; if (Span1 == 59) Col59 = "O"; if (Span1 == 60) Col60 = "O";

                    if (Span1 == 61) Col61 = "O"; if (Span1 == 62) Col62 = "O"; if (Span1 == 63) Col63 = "O"; if (Span1 == 64) Col64 = "O"; if (Span1 == 65) Col65 = "O";
                    if (Span1 == 66) Col66 = "O"; if (Span1 == 67) Col67 = "O"; if (Span1 == 68) Col68 = "O"; if (Span1 == 69) Col69 = "O"; if (Span1 == 70) Col70 = "O";

                    if (Span1 == 71) Col71 = "O"; if (Span1 == 72) Col72 = "O"; if (Span1 == 73) Col73 = "O"; if (Span1 == 74) Col74 = "O"; if (Span1 == 75) Col75 = "O";
                    if (Span1 == 76) Col76 = "O"; if (Span1 == 77) Col77 = "O"; if (Span1 == 78) Col78 = "O"; if (Span1 == 79) Col79 = "O"; if (Span1 == 80) Col80 = "O";

                    if (Span1 == 81) Col81 = "O"; if (Span1 == 82) Col82 = "O"; if (Span1 == 83) Col83 = "O"; if (Span1 == 84) Col84 = "O"; if (Span1 == 85) Col85 = "O";
                    if (Span1 == 86) Col86 = "O"; if (Span1 == 87) Col87 = "O"; if (Span1 == 88) Col88 = "O"; if (Span1 == 89) Col89 = "O"; if (Span1 == 90) Col90 = "O";

                    if (Span1 == 91) Col91 = "O"; if (Span1 == 92) Col92 = "O"; if (Span1 == 93) Col93 = "O"; if (Span1 == 94) Col94 = "O"; if (Span1 == 95) Col95 = "O";
                    if (Span1 == 96) Col96 = "O"; if (Span1 == 97) Col97 = "O"; if (Span1 == 98) Col98 = "O"; if (Span1 == 99) Col99 = "O"; if (Span1 == 100) Col100 = "O";

                    if (Span1 == 101) Col101 = "O"; if (Span1 == 102) Col102 = "O"; if (Span1 == 103) Col103 = "O"; if (Span1 == 104) Col104 = "O"; if (Span1 == 105) Col105 = "O";
                    if (Span1 == 106) Col106 = "O"; if (Span1 == 107) Col107 = "O"; if (Span1 == 108) Col108 = "O"; if (Span1 == 109) Col109 = "O"; if (Span1 == 110) Col110 = "O";

                    if (Span1 == 111) Col111 = "O"; if (Span1 == 112) Col112 = "O"; if (Span1 == 113) Col113 = "O"; if (Span1 == 114) Col114 = "O"; if (Span1 == 115) Col115 = "O";
                    if (Span1 == 116) Col116 = "O"; if (Span1 == 117) Col117 = "O"; if (Span1 == 118) Col118 = "O"; if (Span1 == 119) Col119 = "O"; if (Span1 == 120) Col120 = "O";

                    if (Span1 == 121) Col121 = "O"; if (Span1 == 122) Col122 = "O"; if (Span1 == 123) Col123 = "O"; if (Span1 == 124) Col124 = "O"; if (Span1 == 125) Col125 = "O";
                    if (Span1 == 126) Col126 = "O"; if (Span1 == 127) Col127 = "O"; if (Span1 == 128) Col128 = "O"; if (Span1 == 129) Col129 = "O"; if (Span1 == 130) Col130 = "O";

                    if (Span1 == 131) Col131 = "O"; if (Span1 == 132) Col132 = "O"; if (Span1 == 133) Col133 = "O"; if (Span1 == 134) Col134 = "O"; if (Span1 == 135) Col135 = "O";
                    if (Span1 == 136) Col136 = "O"; if (Span1 == 137) Col137 = "O"; if (Span1 == 138) Col138 = "O"; if (Span1 == 139) Col139 = "O"; if (Span1 == 140) Col140 = "O";

                    if (Span1 == 141) Col141 = "O"; if (Span1 == 142) Col142 = "O"; if (Span1 == 143) Col143 = "O"; if (Span1 == 144) Col144 = "O"; if (Span1 == 145) Col145 = "O";
                    if (Span1 == 146) Col146 = "O"; if (Span1 == 147) Col147 = "O"; if (Span1 == 148) Col148 = "O"; if (Span1 == 149) Col149 = "O"; if (Span1 == 150) Col150 = "O";

                    if (Span1 == 151) Col151 = "O"; if (Span1 == 152) Col152 = "O"; if (Span1 == 153) Col153 = "O"; if (Span1 == 154) Col154 = "O"; if (Span1 == 155) Col155 = "O";
                    if (Span1 == 156) Col156 = "O"; if (Span1 == 157) Col157 = "O"; if (Span1 == 158) Col158 = "O"; if (Span1 == 159) Col159 = "O"; if (Span1 == 160) Col160 = "O";

                    if (Span1 == 161) Col161 = "O"; if (Span1 == 162) Col162 = "O"; if (Span1 == 163) Col163 = "O"; if (Span1 == 164) Col164 = "O"; if (Span1 == 165) Col165 = "O";
                    if (Span1 == 166) Col166 = "O"; if (Span1 == 167) Col167 = "O"; if (Span1 == 168) Col168 = "O"; if (Span1 == 169) Col169 = "O"; if (Span1 == 170) Col170 = "O";

                    if (Span1 == 171) Col171 = "O"; if (Span1 == 172) Col172 = "O"; if (Span1 == 173) Col173 = "O"; if (Span1 == 174) Col174 = "O"; if (Span1 == 175) Col175 = "O";
                    if (Span1 == 176) Col176 = "O"; if (Span1 == 177) Col177 = "O"; if (Span1 == 178) Col178 = "O"; if (Span1 == 179) Col179 = "O"; if (Span1 == 180) Col180 = "O";


                    if (Span1 == 181) Col181 = "O"; if (Span1 == 182) Col182 = "O"; if (Span1 == 183) Col183 = "O"; if (Span1 == 184) Col184 = "O"; if (Span1 == 185) Col185 = "O";
                    if (Span1 == 186) Col186 = "O"; if (Span1 == 187) Col187 = "O"; if (Span1 == 188) Col188 = "O"; if (Span1 == 189) Col189 = "O"; if (Span1 == 190) Col190 = "O";

                    if (Span1 == 191) Col191 = "O"; if (Span1 == 192) Col192 = "O"; if (Span1 == 193) Col193 = "O"; if (Span1 == 194) Col194 = "O"; if (Span1 == 195) Col195 = "O";
                    if (Span1 == 196) Col196 = "O"; if (Span1 == 197) Col197 = "O"; if (Span1 == 198) Col198 = "O"; if (Span1 == 199) Col199 = "O"; if (Span1 == 200) Col200 = "O";

                    if (Span1 == 201) Col201 = "O"; if (Span1 == 202) Col202 = "O"; if (Span1 == 203) Col203 = "O"; if (Span1 == 204) Col204 = "O"; if (Span1 == 205) Col205 = "O";
                    if (Span1 == 206) Col206 = "O"; if (Span1 == 207) Col207 = "O"; if (Span1 == 208) Col208 = "O"; if (Span1 == 209) Col209 = "O"; if (Span1 == 210) Col210 = "O";

                    if (Span1 == 211) Col211 = "O"; if (Span1 == 212) Col212 = "O"; if (Span1 == 213) Col213 = "O"; if (Span1 == 214) Col214 = "O"; if (Span1 == 215) Col215 = "O";
                    if (Span1 == 216) Col216 = "O"; if (Span1 == 217) Col217 = "O"; if (Span1 == 218) Col218 = "O"; if (Span1 == 219) Col219 = "O"; if (Span1 == 220) Col220 = "O";

                    if (Span1 == 221) Col221 = "O"; if (Span1 == 222) Col222 = "O"; if (Span1 == 223) Col223 = "O"; if (Span1 == 224) Col224 = "O"; if (Span1 == 225) Col225 = "O";
                    if (Span1 == 226) Col226 = "O"; if (Span1 == 227) Col227 = "O"; if (Span1 == 228) Col228 = "O"; if (Span1 == 229) Col229 = "O"; if (Span1 == 230) Col230 = "O";

                    if (Span1 == 231) Col231 = "O"; if (Span1 == 232) Col232 = "O"; if (Span1 == 233) Col233 = "O"; if (Span1 == 234) Col234 = "O"; if (Span1 == 235) Col235 = "O";
                    if (Span1 == 236) Col236 = "O"; if (Span1 == 237) Col237 = "O"; if (Span1 == 238) Col238 = "O"; if (Span1 == 239) Col239 = "O"; if (Span1 == 240) Col240 = "O";

                    if (Span1 == 241) Col241 = "O"; if (Span1 == 242) Col242 = "O"; if (Span1 == 243) Col243 = "O"; if (Span1 == 244) Col244 = "O"; if (Span1 == 245) Col245 = "O";
                    if (Span1 == 246) Col246 = "O"; if (Span1 == 247) Col247 = "O"; if (Span1 == 248) Col248 = "O"; if (Span1 == 249) Col249 = "O"; if (Span1 == 250) Col250 = "O";

                    if (Span1 == 251) Col251 = "O"; if (Span1 == 252) Col252 = "O"; if (Span1 == 253) Col253 = "O"; if (Span1 == 254) Col254 = "O"; if (Span1 == 255) Col255 = "O";
                    if (Span1 == 256) Col256 = "O"; if (Span1 == 257) Col257 = "O"; if (Span1 == 258) Col258 = "O"; if (Span1 == 259) Col259 = "O"; if (Span1 == 260) Col260 = "O";

                    if (Span1 == 261) Col261 = "O"; if (Span1 == 262) Col262 = "O"; if (Span1 == 263) Col263 = "O"; if (Span1 == 264) Col264 = "O"; if (Span1 == 265) Col265 = "O";
                    if (Span1 == 266) Col266 = "O"; if (Span1 == 267) Col267 = "O"; if (Span1 == 268) Col268 = "O"; if (Span1 == 269) Col269 = "O"; if (Span1 == 270) Col270 = "O";

                    if (Span1 == 271) Col271 = "O"; if (Span1 == 272) Col272 = "O"; if (Span1 == 273) Col273 = "O"; if (Span1 == 274) Col274 = "O"; if (Span1 == 275) Col275 = "O";
                    if (Span1 == 276) Col276 = "O"; if (Span1 == 277) Col277 = "O"; if (Span1 == 278) Col278 = "O"; if (Span1 == 279) Col279 = "O"; if (Span1 == 280) Col280 = "O";


                    if (Span1 == 281) Col281 = "O"; if (Span1 == 282) Col282 = "O"; if (Span1 == 283) Col283 = "O"; if (Span1 == 284) Col284 = "O"; if (Span1 == 285) Col285 = "O";
                    if (Span1 == 286) Col286 = "O"; if (Span1 == 287) Col287 = "O"; if (Span1 == 288) Col288 = "O"; if (Span1 == 289) Col289 = "O"; if (Span1 == 290) Col290 = "O";

                    if (Span1 == 291) Col291 = "O"; if (Span1 == 292) Col292 = "O"; if (Span1 == 293) Col293 = "O"; if (Span1 == 294) Col294 = "O"; if (Span1 == 295) Col295 = "O";
                    if (Span1 == 296) Col296 = "O"; if (Span1 == 297) Col297 = "O"; if (Span1 == 298) Col298 = "O"; if (Span1 == 299) Col299 = "O"; if (Span1 == 300) Col300 = "O";

                    if (Span1 == 301) Col301 = "O"; if (Span1 == 302) Col302 = "O"; if (Span1 == 303) Col303 = "O"; if (Span1 == 304) Col304 = "O"; if (Span1 == 305) Col305 = "O";
                    if (Span1 == 306) Col306 = "O"; if (Span1 == 307) Col307 = "O"; if (Span1 == 308) Col308 = "O"; if (Span1 == 309) Col309 = "O"; if (Span1 == 310) Col310 = "O";

                    if (Span1 == 311) Col311 = "O"; if (Span1 == 312) Col312 = "O"; if (Span1 == 313) Col313 = "O"; if (Span1 == 314) Col314 = "O"; if (Span1 == 315) Col315 = "O";
                    if (Span1 == 316) Col316 = "O"; if (Span1 == 317) Col317 = "O"; if (Span1 == 318) Col318 = "O"; if (Span1 == 319) Col319 = "O"; if (Span1 == 320) Col320 = "O";

                    if (Span1 == 321) Col321 = "O"; if (Span1 == 322) Col322 = "O"; if (Span1 == 323) Col323 = "O"; if (Span1 == 324) Col324 = "O"; if (Span1 == 325) Col325 = "O";
                    if (Span1 == 326) Col326 = "O"; if (Span1 == 327) Col327 = "O"; if (Span1 == 328) Col328 = "O"; if (Span1 == 329) Col329 = "O"; if (Span1 == 330) Col330 = "O";

                    if (Span1 == 331) Col331 = "O"; if (Span1 == 332) Col332 = "O"; if (Span1 == 333) Col333 = "O"; if (Span1 == 334) Col334 = "O"; if (Span1 == 335) Col335 = "O";
                    if (Span1 == 336) Col336 = "O"; if (Span1 == 337) Col337 = "O"; if (Span1 == 338) Col338 = "O"; if (Span1 == 339) Col339 = "O"; if (Span1 == 340) Col340 = "O";

                    if (Span1 == 341) Col341 = "O"; if (Span1 == 342) Col342 = "O"; if (Span1 == 343) Col343 = "O"; if (Span1 == 344) Col344 = "O"; if (Span1 == 345) Col345 = "O";
                    if (Span1 == 346) Col346 = "O"; if (Span1 == 347) Col347 = "O"; if (Span1 == 348) Col348 = "O"; if (Span1 == 349) Col349 = "O"; if (Span1 == 350) Col350 = "O";

                    if (Span1 == 351) Col351 = "O"; if (Span1 == 352) Col352 = "O"; if (Span1 == 353) Col353 = "O"; if (Span1 == 354) Col354 = "O"; if (Span1 == 355) Col355 = "O";
                    if (Span1 == 356) Col356 = "O"; if (Span1 == 357) Col357 = "O"; if (Span1 == 358) Col358 = "O"; if (Span1 == 359) Col359 = "O"; if (Span1 == 360) Col360 = "O";

                    if (Span1 == 361) Col361 = "O"; if (Span1 == 362) Col362 = "O"; if (Span1 == 363) Col363 = "O"; if (Span1 == 364) Col364 = "O"; if (Span1 == 365) Col365 = "O";
                    if (Span1 == 366) Col366 = "O"; if (Span1 == 367) Col367 = "O"; if (Span1 == 368) Col368 = "O"; if (Span1 == 369) Col369 = "O"; if (Span1 == 370) Col370 = "O";

                    if (Span1 == 371) Col371 = "O"; if (Span1 == 372) Col372 = "O"; if (Span1 == 373) Col373 = "O"; if (Span1 == 374) Col374 = "O"; if (Span1 == 375) Col375 = "O";
                    if (Span1 == 376) Col376 = "O"; if (Span1 == 377) Col377 = "O"; if (Span1 == 378) Col378 = "O"; if (Span1 == 379) Col379 = "O"; if (Span1 == 380) Col380 = "O";


                    if (Span1 == 381) Col381 = "O"; if (Span1 == 382) Col382 = "O"; if (Span1 == 383) Col383 = "O"; if (Span1 == 384) Col384 = "O"; if (Span1 == 385) Col385 = "O";
                    if (Span1 == 386) Col386 = "O"; if (Span1 == 387) Col387 = "O"; if (Span1 == 388) Col388 = "O"; if (Span1 == 389) Col389 = "O"; if (Span1 == 390) Col390 = "O";

                    if (Span1 == 391) Col391 = "O"; if (Span1 == 392) Col392 = "O"; if (Span1 == 393) Col393 = "O"; if (Span1 == 394) Col394 = "O"; if (Span1 == 395) Col395 = "O";
                    if (Span1 == 396) Col396 = "O"; if (Span1 == 397) Col397 = "O"; if (Span1 == 398) Col398 = "O"; if (Span1 == 399) Col399 = "O"; if (Span1 == 400) Col400 = "O";

                    if (Span1 == 401) Col401 = "O"; if (Span1 == 402) Col402 = "O"; if (Span1 == 403) Col403 = "O"; if (Span1 == 404) Col404 = "O"; if (Span1 == 405) Col405 = "O";
                    if (Span1 == 406) Col406 = "O"; if (Span1 == 407) Col407 = "O"; if (Span1 == 408) Col408 = "O"; if (Span1 == 409) Col409 = "O"; if (Span1 == 410) Col410 = "O";

                    if (Span1 == 411) Col411 = "O"; if (Span1 == 412) Col412 = "O"; if (Span1 == 413) Col413 = "O"; if (Span1 == 414) Col414 = "O"; if (Span1 == 415) Col415 = "O";
                    if (Span1 == 416) Col416 = "O"; if (Span1 == 417) Col417 = "O"; if (Span1 == 418) Col418 = "O"; if (Span1 == 419) Col419 = "O"; if (Span1 == 420) Col420 = "O";

                    if (Span1 == 421) Col421 = "O"; if (Span1 == 422) Col422 = "O"; if (Span1 == 423) Col423 = "O"; if (Span1 == 424) Col424 = "O"; if (Span1 == 425) Col425 = "O";
                    if (Span1 == 426) Col426 = "O"; if (Span1 == 427) Col427 = "O"; if (Span1 == 428) Col428 = "O"; if (Span1 == 429) Col429 = "O"; if (Span1 == 430) Col430 = "O";

                    if (Span1 == 431) Col431 = "O"; if (Span1 == 432) Col432 = "O"; if (Span1 == 433) Col433 = "O"; if (Span1 == 434) Col434 = "O"; if (Span1 == 435) Col435 = "O";
                    if (Span1 == 436) Col436 = "O"; if (Span1 == 437) Col437 = "O"; if (Span1 == 438) Col438 = "O"; if (Span1 == 439) Col439 = "O"; if (Span1 == 440) Col440 = "O";

                    if (Span1 == 441) Col441 = "O"; if (Span1 == 442) Col442 = "O"; if (Span1 == 443) Col443 = "O"; if (Span1 == 444) Col444 = "O"; if (Span1 == 445) Col445 = "O";
                    if (Span1 == 446) Col446 = "O"; if (Span1 == 447) Col447 = "O"; if (Span1 == 448) Col448 = "O"; if (Span1 == 449) Col449 = "O"; if (Span1 == 450) Col450 = "O";

                    if (Span1 == 451) Col451 = "O"; if (Span1 == 452) Col452 = "O"; if (Span1 == 453) Col453 = "O"; if (Span1 == 454) Col454 = "O"; if (Span1 == 455) Col455 = "O";
                    if (Span1 == 456) Col456 = "O"; if (Span1 == 457) Col457 = "O"; if (Span1 == 458) Col458 = "O"; if (Span1 == 459) Col459 = "O"; if (Span1 == 460) Col460 = "O";

                    if (Span1 == 461) Col461 = "O"; if (Span1 == 462) Col462 = "O"; if (Span1 == 463) Col463 = "O"; if (Span1 == 464) Col464 = "O"; if (Span1 == 465) Col465 = "O";
                    if (Span1 == 466) Col466 = "O"; if (Span1 == 467) Col467 = "O"; if (Span1 == 468) Col468 = "O"; if (Span1 == 469) Col469 = "O"; if (Span1 == 470) Col470 = "O";

                    if (Span1 == 471) Col471 = "O"; if (Span1 == 472) Col472 = "O"; if (Span1 == 473) Col473 = "O"; if (Span1 == 474) Col474 = "O"; if (Span1 == 475) Col475 = "O";
                    if (Span1 == 476) Col476 = "O"; if (Span1 == 477) Col477 = "O"; if (Span1 == 478) Col478 = "O"; if (Span1 == 479) Col479 = "O"; if (Span1 == 480) Col480 = "O";


                    if (Span1 == 481) Col481 = "O"; if (Span1 == 482) Col482 = "O"; if (Span1 == 483) Col483 = "O"; if (Span1 == 484) Col484 = "O"; if (Span1 == 485) Col485 = "O";
                    if (Span1 == 486) Col486 = "O"; if (Span1 == 487) Col487 = "O"; if (Span1 == 488) Col488 = "O"; if (Span1 == 489) Col489 = "O"; if (Span1 == 490) Col490 = "O";

                    if (Span1 == 491) Col491 = "O"; if (Span1 == 492) Col492 = "O"; if (Span1 == 493) Col493 = "O"; if (Span1 == 494) Col494 = "O"; if (Span1 == 495) Col495 = "O";
                    if (Span1 == 496) Col496 = "O"; if (Span1 == 497) Col497 = "O"; if (Span1 == 498) Col498 = "O"; if (Span1 == 499) Col499 = "O"; if (Span1 == 500) Col500 = "O";

                    if (Span1 == 501) Col501 = "O"; if (Span1 == 502) Col502 = "O"; if (Span1 == 503) Col503 = "O"; if (Span1 == 504) Col504 = "O";
                    if (Span1 == 505) Col505 = "O"; if (Span1 == 506) Col506 = "O"; if (Span1 == 507) Col507 = "O"; if (Span1 == 508) Col508 = "O";
                    if (Span1 == 509) Col509 = "O"; if (Span1 == 510) Col510 = "O"; if (Span1 == 511) Col511 = "O"; if (Span1 == 512) Col512 = "O";
                    if (Span1 == 513) Col513 = "O"; if (Span1 == 514) Col514 = "O"; if (Span1 == 515) Col515 = "O"; if (Span1 == 516) Col516 = "O";
                    if (Span1 == 517) Col517 = "O"; if (Span1 == 518) Col518 = "O"; if (Span1 == 519) Col519 = "O"; if (Span1 == 520) Col520 = "O";
                    if (Span1 == 521) Col521 = "O"; if (Span1 == 522) Col522 = "O"; if (Span1 == 523) Col523 = "O"; if (Span1 == 524) Col524 = "O";
                    if (Span1 == 525) Col525 = "O"; if (Span1 == 526) Col526 = "O"; if (Span1 == 527) Col527 = "O"; if (Span1 == 528) Col528 = "O";
                    if (Span1 == 529) Col529 = "O"; if (Span1 == 530) Col530 = "O"; if (Span1 == 531) Col531 = "O"; if (Span1 == 532) Col532 = "O";
                    if (Span1 == 533) Col533 = "O"; if (Span1 == 534) Col534 = "O"; if (Span1 == 535) Col535 = "O"; if (Span1 == 536) Col536 = "O";
                    if (Span1 == 537) Col537 = "O"; if (Span1 == 538) Col538 = "O"; if (Span1 == 539) Col539 = "O"; if (Span1 == 540) Col540 = "O";
                    if (Span1 == 541) Col541 = "O"; if (Span1 == 542) Col542 = "O"; if (Span1 == 543) Col543 = "O"; if (Span1 == 544) Col544 = "O";
                    if (Span1 == 545) Col545 = "O"; if (Span1 == 546) Col546 = "O"; if (Span1 == 547) Col547 = "O"; if (Span1 == 548) Col548 = "O";
                    if (Span1 == 549) Col549 = "O";



                    //if (Col62 != "O")
                    //{

                    //    if (DTPLeep1.Value.ToString("dd") == "29")
                    //    {
                    //        Col62 = "O";
                    //    }
                    //}
                    //if (Col440 != "O")
                    //{
                    //    if (DTPLeep2.Value.ToString("dd") == "29")
                    //    {
                    //        Col440 = "O";
                    //    }

                    //}

                }



                if (r["SDate"] != DBNull.Value)
                {
                    Span = Convert.ToDateTime(r["SDate"].ToString()).Subtract(DTPStart.Value);
                    Span1 = Convert.ToInt32(Span.Days);

                    if (r["adddelay"] != DBNull.Value)
                    {
                        Span1 = Span1 + Convert.ToInt32(r["adddelay"].ToString());

                    }


                    if (DTPLeep1.Value.ToString("dd") != "29")
                    {
                        if (Span1 > 59)
                            Span1 = Span1 + 1;
                    }


                    if (DTPLeep2.Value.ToString("dd") != "29")
                    {
                        if (Span1 > 425)
                            Span1 = Span1 + 1;
                    }


                    decimal prog = 0;

                    decimal remainder = 0;

                    decimal remainder1 = 0;

                    remainder = Math.Floor(Convert.ToDecimal(r["Length"].ToString()) / Convert.ToDecimal(r["advpershift"].ToString()));

                    remainder1 = (Convert.ToDecimal(r["advpershift"].ToString()) * ((Convert.ToDecimal(r["Length"].ToString()) / Convert.ToDecimal(r["advpershift"].ToString())) - remainder)) / remainder;


                    dur = 0;

                    EndDate = Convert.ToDateTime(r["SDate"].ToString());
                    StartDate = DTPStart.Value;// = Convert.ToDateTime(r["SDate"].ToString());

                    int zz = 0;


                    Lbook = Convert.ToDateTime(r["lastbook"].ToString()).Subtract(DTPStart.Value);
                    LBookInt = Convert.ToInt32(Lbook.Days);

                    if (DTPLeep1.Value.ToString("dd") != "29")
                    {
                        if (LBookInt > 59)
                            LBookInt = LBookInt + 1;
                    }


                    if (DTPLeep2.Value.ToString("dd") != "29")
                    {
                        if (LBookInt > 425)
                            LBookInt = LBookInt + 1;
                    }

                    if (Span1 >= 0)
                    {




                        // getfirst col
                        if ((Span1 < 1) && (Col1 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col1 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(0); }
                        if ((Span1 < 2) && (Col2 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col2 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(1); }
                        if ((Span1 < 3) && (Col3 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col3 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(2); }
                        if ((Span1 < 4) && (Col4 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col4 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(3); }
                        if ((Span1 < 5) && (Col5 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col5 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(4); }
                        if ((Span1 < 6) && (Col6 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col6 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(5); }
                        if ((Span1 < 7) && (Col7 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col7 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(6); }
                        if ((Span1 < 8) && (Col8 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col8 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(7); }
                        if ((Span1 < 9) && (Col9 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col9 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(8); }
                        if ((Span1 < 10) && (Col10 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col10 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(9); }
                        if ((Span1 < 11) && (Col11 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col11 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(10); }
                        if ((Span1 < 12) && (Col12 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col12 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(11); }
                        if ((Span1 < 13) && (Col13 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col13 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(12); }
                        if ((Span1 < 14) && (Col14 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col14 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(13); }
                        if ((Span1 < 15) && (Col15 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col15 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(14); }
                        if ((Span1 < 16) && (Col16 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col16 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(15); }
                        if ((Span1 < 17) && (Col17 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col17 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(16); }
                        if ((Span1 < 18) && (Col18 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col18 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(17); }
                        if ((Span1 < 19) && (Col19 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col19 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(18); }
                        if ((Span1 < 20) && (Col20 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col20 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(19); }
                        if ((Span1 < 21) && (Col21 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col21 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(20); }
                        if ((Span1 < 22) && (Col22 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col22 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(21); }
                        if ((Span1 < 23) && (Col23 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col23 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(22); }
                        if ((Span1 < 24) && (Col24 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col24 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(23); }
                        if ((Span1 < 25) && (Col25 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col25 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(24); }
                        if ((Span1 < 26) && (Col26 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col26 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(25); }
                        if ((Span1 < 27) && (Col27 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col27 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(26); }
                        if ((Span1 < 28) && (Col28 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col28 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(27); }
                        if ((Span1 < 29) && (Col29 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col29 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(28); }
                        if ((Span1 < 30) && (Col30 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col30 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(29); }
                        if ((Span1 < 31) && (Col31 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col31 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(30); }

                        Jan1 = Math.Round(prog, 1);

                        if ((Span1 < 32) && (Col32 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col32 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(31); }
                        if ((Span1 < 33) && (Col33 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col33 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(32); }
                        if ((Span1 < 34) && (Col34 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col34 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(33); }
                        if ((Span1 < 35) && (Col35 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col35 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(34); }
                        if ((Span1 < 36) && (Col36 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col36 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(35); }
                        if ((Span1 < 37) && (Col37 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col37 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(36); }
                        if ((Span1 < 38) && (Col38 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col38 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(37); }
                        if ((Span1 < 39) && (Col39 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col39 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(38); }
                        if ((Span1 < 40) && (Col40 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col40 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(39); }

                        if ((Span1 < 41) && (Col41 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col41 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(40); }
                        if ((Span1 < 42) && (Col42 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col42 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(41); }
                        if ((Span1 < 43) && (Col43 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col43 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(42); }
                        if ((Span1 < 44) && (Col44 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col44 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(43); }
                        if ((Span1 < 45) && (Col45 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col45 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(44); }
                        if ((Span1 < 46) && (Col46 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col46 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(45); }
                        if ((Span1 < 47) && (Col47 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col47 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(46); }
                        if ((Span1 < 48) && (Col48 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col48 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(47); }
                        if ((Span1 < 49) && (Col49 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col49 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(48); }
                        if ((Span1 < 50) && (Col50 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col50 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(49); }


                        if ((Span1 < 51) && (Col51 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col51 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(50); }
                        if ((Span1 < 52) && (Col52 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col52 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(51); }
                        if ((Span1 < 53) && (Col53 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col53 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(52); }
                        if ((Span1 < 54) && (Col54 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col54 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(53); }
                        if ((Span1 < 55) && (Col55 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col55 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(54); }
                        if ((Span1 < 56) && (Col56 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col56 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(55); }
                        if ((Span1 < 57) && (Col57 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col57 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(56); }
                        if ((Span1 < 58) && (Col58 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col58 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(57); }
                        if ((Span1 < 59) && (Col59 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col59 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(58); }
                        if ((Span1 < 60) && (Col60 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col60 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(59); }
                        Feb1 = Math.Round(prog, 1) - Jan1;

                        if ((Span1 < 61) && (Col61 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col61 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(60); }
                        if ((Span1 < 62) && (Col62 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col62 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(61); }
                        if ((Span1 < 63) && (Col63 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col63 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(62); }
                        if ((Span1 < 64) && (Col64 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col64 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(63); }
                        if ((Span1 < 65) && (Col65 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col65 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(64); }
                        if ((Span1 < 66) && (Col66 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col66 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(65); }
                        if ((Span1 < 67) && (Col67 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col67 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(66); }
                        if ((Span1 < 68) && (Col68 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col68 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(67); }
                        if ((Span1 < 69) && (Col69 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col69 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(68); }
                        if ((Span1 < 70) && (Col70 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col70 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(69); }



                        if ((Span1 < 71) && (Col71 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col71 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(70); }
                        if ((Span1 < 72) && (Col72 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col72 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(71); }
                        if ((Span1 < 73) && (Col73 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col73 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(72); }
                        if ((Span1 < 74) && (Col74 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col74 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(73); }
                        if ((Span1 < 75) && (Col75 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col75 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(74); }
                        if ((Span1 < 76) && (Col76 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col76 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(75); }
                        if ((Span1 < 77) && (Col77 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col77 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(76); }
                        if ((Span1 < 78) && (Col78 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col78 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(77); }
                        if ((Span1 < 79) && (Col79 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col79 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(78); }
                        if ((Span1 < 80) && (Col80 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col80 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(79); }

                        if ((Span1 < 81) && (Col81 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col81 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(80); }
                        if ((Span1 < 82) && (Col82 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col82 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(81); }
                        if ((Span1 < 83) && (Col83 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col83 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(82); }
                        if ((Span1 < 84) && (Col84 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col84 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(83); }
                        if ((Span1 < 85) && (Col85 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col85 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(84); }
                        if ((Span1 < 86) && (Col86 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col86 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(85); }
                        if ((Span1 < 87) && (Col87 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col87 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(86); }
                        if ((Span1 < 88) && (Col88 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col88 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(87); }
                        if ((Span1 < 89) && (Col89 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col89 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(88); }
                        if ((Span1 < 90) && (Col90 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col90 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(89); }

                        if ((Span1 < 91) && (Col91 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col91 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(90); }

                        Mar1 = Math.Round(prog, 1) - Jan1 - Feb1;

                        if ((Span1 < 92) && (Col92 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col92 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(91); }
                        if ((Span1 < 93) && (Col93 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col93 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(92); }
                        if ((Span1 < 94) && (Col94 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col94 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(93); }
                        if ((Span1 < 95) && (Col95 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col95 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(94); }
                        if ((Span1 < 96) && (Col96 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col96 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(95); }
                        if ((Span1 < 97) && (Col97 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col97 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(96); }
                        if ((Span1 < 98) && (Col98 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col98 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(97); }
                        if ((Span1 < 99) && (Col99 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col99 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(98); }
                        if ((Span1 < 100) && (Col100 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col100 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(99); }

                        if ((Span1 < 101) && (Col101 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col101 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(100); }
                        if ((Span1 < 102) && (Col102 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col102 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(101); }
                        if ((Span1 < 103) && (Col103 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col103 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(102); }
                        if ((Span1 < 104) && (Col104 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col104 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(103); }
                        if ((Span1 < 105) && (Col105 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col105 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(104); }
                        if ((Span1 < 106) && (Col106 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col106 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(105); }
                        if ((Span1 < 107) && (Col107 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col107 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(106); }
                        if ((Span1 < 108) && (Col108 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col108 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(107); }
                        if ((Span1 < 109) && (Col109 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col109 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(108); }
                        if ((Span1 < 110) && (Col110 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col110 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(109); }

                        if ((Span1 < 111) && (Col111 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col111 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(110); }
                        if ((Span1 < 112) && (Col112 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col112 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(111); }
                        if ((Span1 < 113) && (Col113 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col113 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(112); }
                        if ((Span1 < 114) && (Col114 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col114 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(113); }
                        if ((Span1 < 115) && (Col115 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col115 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(114); }
                        if ((Span1 < 116) && (Col116 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col116 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(115); }
                        if ((Span1 < 117) && (Col117 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col117 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(116); }
                        if ((Span1 < 118) && (Col118 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col118 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(117); }
                        if ((Span1 < 119) && (Col119 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col119 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(118); }
                        if ((Span1 < 120) && (Col120 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col120 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(119); }

                        if ((Span1 < 121) && (Col121 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col121 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(120); }

                        Apr1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1;

                        if ((Span1 < 122) && (Col122 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col122 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(121); }
                        if ((Span1 < 123) && (Col123 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col123 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(122); }
                        if ((Span1 < 124) && (Col124 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col124 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(123); }
                        if ((Span1 < 125) && (Col125 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col125 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(124); }
                        if ((Span1 < 126) && (Col126 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col126 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(125); }
                        if ((Span1 < 127) && (Col127 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col127 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(126); }
                        if ((Span1 < 128) && (Col128 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col128 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(127); }
                        if ((Span1 < 129) && (Col129 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col129 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(128); }
                        if ((Span1 < 130) && (Col130 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col130 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(129); }

                        if ((Span1 < 131) && (Col131 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col131 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(130); }
                        if ((Span1 < 132) && (Col132 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col132 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(131); }
                        if ((Span1 < 133) && (Col133 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col133 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(132); }
                        if ((Span1 < 134) && (Col134 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col134 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(133); }
                        if ((Span1 < 135) && (Col135 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col135 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(134); }
                        if ((Span1 < 136) && (Col136 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col136 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(135); }
                        if ((Span1 < 137) && (Col137 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col137 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(136); }
                        if ((Span1 < 138) && (Col138 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col138 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(137); }
                        if ((Span1 < 139) && (Col139 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col139 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(138); }
                        if ((Span1 < 140) && (Col140 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col140 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(139); }

                        if ((Span1 < 141) && (Col141 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col141 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(140); }
                        if ((Span1 < 142) && (Col142 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col142 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(141); }
                        if ((Span1 < 143) && (Col143 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col143 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(142); }
                        if ((Span1 < 144) && (Col144 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col144 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(143); }
                        if ((Span1 < 145) && (Col145 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col145 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(144); }
                        if ((Span1 < 146) && (Col146 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col146 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(145); }
                        if ((Span1 < 147) && (Col147 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col147 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(146); }
                        if ((Span1 < 148) && (Col148 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col148 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(147); }
                        if ((Span1 < 149) && (Col149 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col149 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(148); }
                        if ((Span1 < 150) && (Col150 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col150 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(149); }


                        if ((Span1 < 151) && (Col151 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col151 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(150); }
                        if ((Span1 < 152) && (Col152 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col152 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(151); }

                        May1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1;

                        if ((Span1 < 153) && (Col153 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col153 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(152); }
                        if ((Span1 < 154) && (Col154 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col154 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(153); }
                        if ((Span1 < 155) && (Col155 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col155 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(154); }
                        if ((Span1 < 156) && (Col156 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col156 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(155); }
                        if ((Span1 < 157) && (Col157 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col157 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(156); }
                        if ((Span1 < 158) && (Col158 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col158 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(157); }
                        if ((Span1 < 159) && (Col159 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col159 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(158); }
                        if ((Span1 < 160) && (Col160 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col160 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(159); }



                        if ((Span1 < 161) && (Col161 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col161 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(160); }
                        if ((Span1 < 162) && (Col162 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col162 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(161); }
                        if ((Span1 < 163) && (Col163 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col163 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(162); }
                        if ((Span1 < 164) && (Col164 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col164 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(163); }
                        if ((Span1 < 165) && (Col165 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col165 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(164); }
                        if ((Span1 < 166) && (Col166 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col166 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(165); }
                        if ((Span1 < 167) && (Col167 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col167 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(166); }
                        if ((Span1 < 168) && (Col168 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col168 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(167); }
                        if ((Span1 < 169) && (Col169 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col169 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(168); }
                        if ((Span1 < 170) && (Col170 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col170 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(169); }


                        if ((Span1 < 171) && (Col171 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col171 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(170); }
                        if ((Span1 < 172) && (Col172 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col172 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(171); }
                        if ((Span1 < 173) && (Col173 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col173 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(172); }
                        if ((Span1 < 174) && (Col174 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col174 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(173); }
                        if ((Span1 < 175) && (Col175 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col175 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(174); }
                        if ((Span1 < 176) && (Col176 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col176 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(175); }
                        if ((Span1 < 177) && (Col177 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col177 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(176); }
                        if ((Span1 < 178) && (Col178 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col178 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(177); }
                        if ((Span1 < 179) && (Col179 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col179 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(178); }
                        if ((Span1 < 180) && (Col180 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col180 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(179); }
                        if ((Span1 < 181) && (Col181 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col181 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(180); }
                        if ((Span1 < 182) && (Col182 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col182 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(181); }

                        Jun1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1;

                        if ((Span1 < 183) && (Col183 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col183 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(182); }
                        if ((Span1 < 184) && (Col184 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col184 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(183); }
                        if ((Span1 < 185) && (Col185 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col185 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(184); }
                        if ((Span1 < 186) && (Col186 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col186 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(185); }
                        if ((Span1 < 187) && (Col187 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col187 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(186); }
                        if ((Span1 < 188) && (Col188 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col188 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(187); }
                        if ((Span1 < 189) && (Col189 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col189 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(188); }
                        if ((Span1 < 190) && (Col190 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col190 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(189); }

                        if ((Span1 < 191) && (Col191 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col191 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(190); }
                        if ((Span1 < 192) && (Col192 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col192 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(191); }
                        if ((Span1 < 193) && (Col193 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col193 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(192); }
                        if ((Span1 < 194) && (Col194 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col194 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(193); }
                        if ((Span1 < 195) && (Col195 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col195 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(194); }
                        if ((Span1 < 196) && (Col196 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col196 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(195); }
                        if ((Span1 < 197) && (Col197 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col197 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(196); }
                        if ((Span1 < 198) && (Col198 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col198 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(197); }
                        if ((Span1 < 199) && (Col199 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col199 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(198); }
                        if ((Span1 < 200) && (Col200 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col200 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(199); }


                        if ((Span1 < 201) && (Col201 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col201 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(200); }
                        if ((Span1 < 202) && (Col202 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col202 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(201); }
                        if ((Span1 < 203) && (Col203 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col203 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(202); }
                        if ((Span1 < 204) && (Col204 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col204 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(203); }
                        if ((Span1 < 205) && (Col205 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col205 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(204); }
                        if ((Span1 < 206) && (Col206 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col206 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(205); }
                        if ((Span1 < 207) && (Col207 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col207 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(206); }
                        if ((Span1 < 208) && (Col208 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col208 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(207); }
                        if ((Span1 < 209) && (Col209 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col209 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(208); }
                        if ((Span1 < 210) && (Col210 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col210 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(209); }


                        if ((Span1 < 211) && (Col211 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col211 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(210); }
                        if ((Span1 < 212) && (Col212 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col212 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(211); }
                        if ((Span1 < 213) && (Col213 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col213 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(212); }

                        Jul1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1;

                        if ((Span1 < 214) && (Col214 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col214 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(213); }
                        if ((Span1 < 215) && (Col215 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col215 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(214); }
                        if ((Span1 < 216) && (Col216 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col216 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(215); }
                        if ((Span1 < 217) && (Col217 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col217 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(216); }
                        if ((Span1 < 218) && (Col218 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col218 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(217); }
                        if ((Span1 < 219) && (Col219 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col219 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(218); }
                        if ((Span1 < 220) && (Col220 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col220 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(219); }

                        if ((Span1 < 221) && (Col221 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col221 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(220); }
                        if ((Span1 < 222) && (Col222 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col222 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(221); }
                        if ((Span1 < 223) && (Col223 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col223 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(222); }
                        if ((Span1 < 224) && (Col224 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col224 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(223); }
                        if ((Span1 < 225) && (Col225 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col225 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(224); }
                        if ((Span1 < 226) && (Col226 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col226 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(225); }
                        if ((Span1 < 227) && (Col227 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col227 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(226); }
                        if ((Span1 < 228) && (Col228 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col228 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(227); }
                        if ((Span1 < 229) && (Col229 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col229 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(228); }
                        if ((Span1 < 230) && (Col230 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col230 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(229); }


                        if ((Span1 < 231) && (Col231 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col231 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(230); }
                        if ((Span1 < 232) && (Col232 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col232 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(231); }
                        if ((Span1 < 233) && (Col233 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col233 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(232); }
                        if ((Span1 < 234) && (Col234 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col234 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(233); }
                        if ((Span1 < 235) && (Col235 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col235 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(234); }
                        if ((Span1 < 236) && (Col236 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col236 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(235); }
                        if ((Span1 < 237) && (Col237 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col237 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(236); }
                        if ((Span1 < 238) && (Col238 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col238 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(237); }
                        if ((Span1 < 239) && (Col239 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col239 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(238); }
                        if ((Span1 < 240) && (Col240 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col240 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(239); }

                        if ((Span1 < 241) && (Col241 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col241 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(240); }
                        if ((Span1 < 242) && (Col242 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col242 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(241); }
                        if ((Span1 < 243) && (Col243 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col243 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(242); }
                        if ((Span1 < 244) && (Col244 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col244 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(243); }

                        Aug1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1;

                        if ((Span1 < 245) && (Col245 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col245 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(244); }
                        if ((Span1 < 246) && (Col246 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col246 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(245); }
                        if ((Span1 < 247) && (Col247 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col247 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(246); }
                        if ((Span1 < 248) && (Col248 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col248 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(247); }
                        if ((Span1 < 249) && (Col249 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col249 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(248); }
                        if ((Span1 < 250) && (Col250 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col250 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(249); }


                        if ((Span1 < 251) && (Col251 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col251 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(250); }
                        if ((Span1 < 252) && (Col252 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col252 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(251); }
                        if ((Span1 < 253) && (Col253 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col253 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(252); }
                        if ((Span1 < 254) && (Col254 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col254 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(253); }
                        if ((Span1 < 255) && (Col255 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col255 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(254); }
                        if ((Span1 < 256) && (Col256 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col256 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(255); }
                        if ((Span1 < 257) && (Col257 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col257 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(256); }
                        if ((Span1 < 258) && (Col258 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col258 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(257); }
                        if ((Span1 < 259) && (Col259 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col259 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(258); }
                        if ((Span1 < 260) && (Col260 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col260 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(259); }

                        if ((Span1 < 261) && (Col261 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col261 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(260); }
                        if ((Span1 < 262) && (Col262 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col262 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(261); }
                        if ((Span1 < 263) && (Col263 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col263 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(262); }
                        if ((Span1 < 264) && (Col264 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col264 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(263); }

                        //Sep1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1- Aug1;                       

                        if ((Span1 < 265) && (Col265 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col265 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(264); }
                        if ((Span1 < 266) && (Col266 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col266 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(265); }
                        if ((Span1 < 267) && (Col267 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col267 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(266); }
                        if ((Span1 < 268) && (Col268 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col268 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(267); }
                        if ((Span1 < 269) && (Col269 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col269 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(268); }
                        if ((Span1 < 270) && (Col270 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col270 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(269); }


                        if ((Span1 < 271) && (Col271 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col271 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(270); }
                        if ((Span1 < 272) && (Col272 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col272 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(271); }
                        if ((Span1 < 273) && (Col273 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col273 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(272); }
                        if ((Span1 < 274) && (Col274 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col274 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(273); }
                        if ((Span1 < 275) && (Col275 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col275 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(274); }
                        Sep1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1;


                        if ((Span1 < 276) && (Col276 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col276 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(275); }
                        if ((Span1 < 277) && (Col277 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col277 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(276); }
                        if ((Span1 < 278) && (Col278 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col278 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(277); }
                        if ((Span1 < 279) && (Col279 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col279 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(278); }
                        if ((Span1 < 280) && (Col280 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col280 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(279); }

                        if ((Span1 < 281) && (Col281 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col281 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(280); }
                        if ((Span1 < 282) && (Col282 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col282 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(281); }
                        if ((Span1 < 283) && (Col283 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col283 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(282); }
                        if ((Span1 < 284) && (Col284 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col284 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(283); }
                        if ((Span1 < 285) && (Col285 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col285 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(284); }
                        if ((Span1 < 286) && (Col286 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col286 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(285); }
                        if ((Span1 < 287) && (Col287 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col287 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(286); }
                        if ((Span1 < 288) && (Col288 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col288 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(287); }
                        if ((Span1 < 289) && (Col289 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col289 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(288); }
                        if ((Span1 < 290) && (Col290 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col290 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(289); }


                        if ((Span1 < 291) && (Col291 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col291 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(290); }
                        if ((Span1 < 292) && (Col292 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col292 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(291); }
                        if ((Span1 < 293) && (Col293 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col293 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(292); }
                        if ((Span1 < 294) && (Col294 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col294 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(293); }
                        if ((Span1 < 295) && (Col295 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col295 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(294); }
                        if ((Span1 < 296) && (Col296 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col296 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(295); }
                        if ((Span1 < 297) && (Col297 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col297 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(296); }
                        if ((Span1 < 298) && (Col298 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col298 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(297); }
                        if ((Span1 < 299) && (Col299 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col299 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(298); }
                        if ((Span1 < 300) && (Col300 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col300 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(299); }

                        if ((Span1 < 301) && (Col301 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col301 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(300); }
                        if ((Span1 < 302) && (Col302 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col302 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(301); }
                        if ((Span1 < 303) && (Col303 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col303 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(302); }
                        if ((Span1 < 304) && (Col304 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col304 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(303); }
                        if ((Span1 < 305) && (Col305 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col305 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(304); }


                        Oct1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1 - Sep1;

                        if ((Span1 < 306) && (Col306 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col306 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(305); }
                        if ((Span1 < 307) && (Col307 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col307 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(306); }
                        if ((Span1 < 308) && (Col308 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col308 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(307); }
                        if ((Span1 < 309) && (Col309 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col309 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(308); }
                        if ((Span1 < 310) && (Col310 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col310 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(309); }


                        if ((Span1 < 311) && (Col311 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col311 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(310); }
                        if ((Span1 < 312) && (Col312 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col312 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(311); }
                        if ((Span1 < 313) && (Col313 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col313 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(312); }
                        if ((Span1 < 314) && (Col314 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col314 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(313); }
                        if ((Span1 < 315) && (Col315 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col315 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(314); }
                        if ((Span1 < 316) && (Col316 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col316 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(315); }
                        if ((Span1 < 317) && (Col317 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col317 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(316); }
                        if ((Span1 < 318) && (Col318 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col318 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(317); }
                        if ((Span1 < 319) && (Col319 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col319 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(318); }
                        if ((Span1 < 320) && (Col320 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col320 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(319); }

                        if ((Span1 < 321) && (Col321 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col321 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(320); }
                        if ((Span1 < 322) && (Col322 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col322 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(321); }
                        if ((Span1 < 323) && (Col323 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col323 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(322); }
                        if ((Span1 < 324) && (Col324 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col324 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(323); }
                        if ((Span1 < 325) && (Col325 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col325 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(324); }
                        if ((Span1 < 326) && (Col326 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col326 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(325); }
                        if ((Span1 < 327) && (Col327 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col327 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(326); }
                        if ((Span1 < 328) && (Col328 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col328 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(327); }
                        if ((Span1 < 329) && (Col329 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col329 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(328); }
                        if ((Span1 < 330) && (Col330 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col330 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(329); }


                        if ((Span1 < 331) && (Col331 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col331 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(330); }
                        if ((Span1 < 332) && (Col332 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col332 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(331); }
                        if ((Span1 < 333) && (Col333 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col333 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(332); }
                        if ((Span1 < 334) && (Col334 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col334 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(333); }
                        if ((Span1 < 335) && (Col335 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col335 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(334); }

                        Nov1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1 - Sep1 - Oct1;

                        if ((Span1 < 336) && (Col336 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col336 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(335); }
                        if ((Span1 < 337) && (Col337 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col337 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(336); }
                        if ((Span1 < 338) && (Col338 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col338 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(337); }
                        if ((Span1 < 339) && (Col339 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col339 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(338); }
                        if ((Span1 < 340) && (Col340 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col340 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(339); }

                        if ((Span1 < 341) && (Col341 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col341 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(340); }
                        if ((Span1 < 342) && (Col342 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col342 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(341); }
                        if ((Span1 < 343) && (Col343 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col343 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(342); }
                        if ((Span1 < 344) && (Col344 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col344 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(343); }
                        if ((Span1 < 345) && (Col345 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col345 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(344); }
                        if ((Span1 < 346) && (Col346 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col346 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(345); }
                        if ((Span1 < 347) && (Col347 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col347 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(346); }
                        if ((Span1 < 348) && (Col348 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col348 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(347); }
                        if ((Span1 < 349) && (Col349 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col349 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(348); }
                        if ((Span1 < 350) && (Col350 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col350 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(349); }


                        if ((Span1 < 351) && (Col351 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col351 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(350); }
                        if ((Span1 < 352) && (Col352 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col352 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(351); }
                        if ((Span1 < 353) && (Col353 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col353 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(352); }
                        if ((Span1 < 354) && (Col354 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col354 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(353); }
                        if ((Span1 < 355) && (Col355 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col355 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(354); }
                        if ((Span1 < 356) && (Col356 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col356 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(355); }
                        if ((Span1 < 357) && (Col357 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col357 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(356); }
                        if ((Span1 < 358) && (Col358 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col358 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(357); }
                        if ((Span1 < 359) && (Col359 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col359 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(358); }
                        if ((Span1 < 360) && (Col360 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col360 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(359); }

                        if ((Span1 < 361) && (Col361 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col361 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(360); }
                        if ((Span1 < 362) && (Col362 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col362 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(361); }
                        if ((Span1 < 363) && (Col363 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col363 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(362); }
                        if ((Span1 < 364) && (Col364 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col364 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(363); }
                        if ((Span1 < 365) && (Col365 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col365 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(364); }
                        if ((Span1 < 366) && (Col366 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col366 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(365); }

                        Dec1 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1 - Sep1 - Oct1 - Nov1;

                        if ((Span1 < 367) && (Col367 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col367 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(366); }
                        if ((Span1 < 368) && (Col368 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col368 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(367); }
                        if ((Span1 < 369) && (Col369 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col369 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(368); }
                        if ((Span1 < 370) && (Col370 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col370 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(369); }

                        if ((Span1 < 371) && (Col371 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col371 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(370); }
                        if ((Span1 < 372) && (Col372 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col372 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(371); }
                        if ((Span1 < 373) && (Col373 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col373 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(372); }
                        if ((Span1 < 374) && (Col374 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col374 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(373); }
                        if ((Span1 < 375) && (Col375 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col375 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(374); }
                        if ((Span1 < 376) && (Col376 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col376 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(375); }
                        if ((Span1 < 377) && (Col377 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col377 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(376); }
                        if ((Span1 < 378) && (Col378 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col378 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(377); }
                        if ((Span1 < 379) && (Col379 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col379 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(378); }
                        if ((Span1 < 380) && (Col380 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col380 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(379); }

                        if ((Span1 < 381) && (Col381 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col381 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(380); }
                        if ((Span1 < 382) && (Col382 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col382 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(381); }
                        if ((Span1 < 383) && (Col383 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col383 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(382); }
                        if ((Span1 < 384) && (Col384 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col384 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(383); }
                        if ((Span1 < 385) && (Col385 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col385 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(384); }
                        if ((Span1 < 386) && (Col386 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col386 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(385); }
                        if ((Span1 < 387) && (Col387 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col387 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(386); }
                        if ((Span1 < 388) && (Col388 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col388 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(387); }
                        if ((Span1 < 389) && (Col389 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col389 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(388); }
                        if ((Span1 < 390) && (Col390 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col390 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(389); }

                        if ((Span1 < 391) && (Col391 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col391 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(390); }
                        if ((Span1 < 392) && (Col392 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col392 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(391); }
                        if ((Span1 < 393) && (Col393 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col393 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(392); }
                        if ((Span1 < 394) && (Col394 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col394 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(393); }
                        if ((Span1 < 395) && (Col395 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col395 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(394); }
                        if ((Span1 < 396) && (Col396 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col396 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(395); }
                        if ((Span1 < 397) && (Col397 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col397 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(396); }

                        Jan2 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1 - Sep1 - Oct1 - Nov1 - Dec1;

                        if ((Span1 < 398) && (Col398 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col398 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(397); }
                        if ((Span1 < 399) && (Col399 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col399 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(398); }
                        if ((Span1 < 400) && (Col400 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col400 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(399); }

                        if ((Span1 < 401) && (Col401 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col401 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(400); }
                        if ((Span1 < 402) && (Col402 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col402 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(401); }
                        if ((Span1 < 403) && (Col403 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col403 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(402); }
                        if ((Span1 < 404) && (Col404 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col404 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(403); }
                        if ((Span1 < 405) && (Col405 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col405 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(404); }
                        if ((Span1 < 406) && (Col406 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col406 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(405); }
                        if ((Span1 < 407) && (Col407 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col407 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(406); }
                        if ((Span1 < 408) && (Col408 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col408 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(407); }
                        if ((Span1 < 409) && (Col409 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col409 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(408); }
                        if ((Span1 < 410) && (Col410 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col410 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(409); }


                        if ((Span1 < 411) && (Col411 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col411 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(410); }
                        if ((Span1 < 412) && (Col412 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col412 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(411); }
                        if ((Span1 < 413) && (Col413 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col413 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(412); }
                        if ((Span1 < 414) && (Col414 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col414 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(413); }
                        if ((Span1 < 415) && (Col415 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col415 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(414); }
                        if ((Span1 < 416) && (Col416 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col416 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(415); }
                        if ((Span1 < 417) && (Col417 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col417 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(416); }
                        if ((Span1 < 418) && (Col418 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col418 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(417); }
                        if ((Span1 < 419) && (Col419 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col419 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(418); }
                        if ((Span1 < 420) && (Col420 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col420 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(419); }

                        if ((Span1 < 421) && (Col421 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col421 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(420); }
                        if ((Span1 < 422) && (Col422 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col422 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(421); }
                        if ((Span1 < 423) && (Col423 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col423 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(422); }
                        if ((Span1 < 424) && (Col424 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col424 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(423); }
                        if ((Span1 < 425) && (Col425 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col425 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(424); }
                        if ((Span1 < 426) && (Col426 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col426 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(425); }


                        Feb2 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1 - Sep1 - Oct1 - Nov1 - Dec1 - Jan2;


                        if ((Span1 < 427) && (Col427 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col427 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(426); }
                        if ((Span1 < 428) && (Col428 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col428 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(427); }
                        if ((Span1 < 429) && (Col429 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col429 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(428); }
                        if ((Span1 < 430) && (Col430 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col430 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(429); }


                        if ((Span1 < 431) && (Col431 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col431 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(430); }
                        if ((Span1 < 432) && (Col432 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col432 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(431); }
                        if ((Span1 < 433) && (Col433 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col433 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(432); }
                        if ((Span1 < 434) && (Col434 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col434 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(433); }
                        if ((Span1 < 435) && (Col435 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col435 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(434); }
                        if ((Span1 < 436) && (Col436 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col436 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(435); }
                        if ((Span1 < 437) && (Col437 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col437 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(436); }
                        if ((Span1 < 438) && (Col438 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col438 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(437); }
                        if ((Span1 < 439) && (Col439 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col439 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(438); }
                        if ((Span1 < 440) && (Col440 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col440 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(439); }

                        if ((Span1 < 441) && (Col441 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col441 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(440); }
                        if ((Span1 < 442) && (Col442 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col442 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(441); }
                        if ((Span1 < 443) && (Col443 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col443 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(442); }
                        if ((Span1 < 444) && (Col444 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col444 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(443); }
                        if ((Span1 < 445) && (Col445 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col445 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(444); }
                        if ((Span1 < 446) && (Col446 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col446 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(445); }
                        if ((Span1 < 447) && (Col447 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col447 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(446); }
                        if ((Span1 < 448) && (Col448 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col448 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(447); }
                        if ((Span1 < 449) && (Col449 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col449 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(448); }
                        if ((Span1 < 450) && (Col450 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col450 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(449); }


                        if ((Span1 < 451) && (Col451 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col451 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(450); }
                        if ((Span1 < 452) && (Col452 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col452 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(451); }
                        if ((Span1 < 453) && (Col453 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col453 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(452); }
                        if ((Span1 < 454) && (Col454 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col454 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(453); }
                        if ((Span1 < 455) && (Col455 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col455 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(454); }
                        if ((Span1 < 456) && (Col456 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col456 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(455); }
                        if ((Span1 < 457) && (Col457 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col457 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(456); }
                        if ((Span1 < 458) && (Col458 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col458 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(457); }

                        Mar2 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1 - Sep1 - Oct1 - Nov1 - Dec1 - Jan2 - Feb2;

                        if ((Span1 < 459) && (Col459 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col459 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(458); }
                        if ((Span1 < 460) && (Col460 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col460 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(459); }

                        if ((Span1 < 461) && (Col461 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col461 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(460); }
                        if ((Span1 < 462) && (Col462 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col462 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(461); }
                        if ((Span1 < 463) && (Col463 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col463 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(462); }
                        if ((Span1 < 464) && (Col464 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col464 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(463); }
                        if ((Span1 < 465) && (Col465 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col465 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(464); }
                        if ((Span1 < 466) && (Col466 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col466 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(465); }
                        if ((Span1 < 467) && (Col467 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col467 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(466); }
                        if ((Span1 < 468) && (Col468 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col468 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(467); }
                        if ((Span1 < 469) && (Col469 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col469 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(468); }
                        if ((Span1 < 470) && (Col470 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col470 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(469); }


                        if ((Span1 < 471) && (Col471 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col471 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(470); }
                        if ((Span1 < 472) && (Col472 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col472 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(471); }
                        if ((Span1 < 473) && (Col473 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col473 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(472); }
                        if ((Span1 < 474) && (Col474 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col474 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(473); }
                        if ((Span1 < 475) && (Col475 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col475 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(474); }
                        if ((Span1 < 476) && (Col476 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col476 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(475); }
                        if ((Span1 < 477) && (Col477 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col477 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(476); }
                        if ((Span1 < 478) && (Col478 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col478 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(477); }
                        if ((Span1 < 479) && (Col479 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col479 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(478); }
                        if ((Span1 < 480) && (Col480 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col480 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(479); }

                        if ((Span1 < 481) && (Col481 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col481 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(480); }
                        if ((Span1 < 482) && (Col482 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col482 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(481); }
                        if ((Span1 < 483) && (Col483 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col483 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(482); }
                        if ((Span1 < 484) && (Col484 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col484 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(483); }
                        if ((Span1 < 485) && (Col485 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col485 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(484); }
                        if ((Span1 < 486) && (Col486 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col486 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(485); }
                        if ((Span1 < 487) && (Col487 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col487 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(486); }
                        if ((Span1 < 488) && (Col488 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col488 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(487); }

                        Apr2 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1 - Sep1 - Oct1 - Nov1 - Dec1 - Jan2 - Feb2 - Mar2;

                        if ((Span1 < 489) && (Col489 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col489 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(488); }
                        if ((Span1 < 490) && (Col490 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col490 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(489); }

                        if ((Span1 < 491) && (Col491 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col491 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(490); }
                        if ((Span1 < 492) && (Col492 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col492 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(491); }
                        if ((Span1 < 493) && (Col493 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col493 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(492); }
                        if ((Span1 < 494) && (Col494 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col494 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(493); }
                        if ((Span1 < 495) && (Col495 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col495 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(494); }
                        if ((Span1 < 496) && (Col496 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col496 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(495); }
                        if ((Span1 < 497) && (Col497 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col497 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(496); }
                        if ((Span1 < 498) && (Col498 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col498 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(497); }
                        if ((Span1 < 499) && (Col499 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col499 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(498); }
                        if ((Span1 < 500) && (Col500 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col500 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(499); }

                        if ((Span1 < 501) && (Col501 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col501 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(500); }
                        if ((Span1 < 502) && (Col502 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col502 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(501); }
                        if ((Span1 < 503) && (Col503 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col503 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(502); }
                        if ((Span1 < 504) && (Col504 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col504 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(503); }
                        if ((Span1 < 505) && (Col505 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col505 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(504); }
                        if ((Span1 < 506) && (Col506 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col506 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(505); }
                        if ((Span1 < 507) && (Col507 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col507 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(506); }
                        if ((Span1 < 508) && (Col508 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col508 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(507); }
                        if ((Span1 < 509) && (Col509 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col509 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(508); }
                        if ((Span1 < 510) && (Col510 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col510 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(509); }
                        if ((Span1 < 511) && (Col511 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col511 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(510); }
                        if ((Span1 < 512) && (Col512 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col512 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(511); }
                        if ((Span1 < 513) && (Col513 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col513 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(512); }
                        if ((Span1 < 514) && (Col514 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col514 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(513); }
                        if ((Span1 < 515) && (Col515 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col515 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(514); }
                        if ((Span1 < 516) && (Col516 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col516 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(515); }
                        if ((Span1 < 517) && (Col517 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col517 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(516); }
                        if ((Span1 < 518) && (Col518 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col518 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(517); }
                        if ((Span1 < 519) && (Col519 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col519 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(518); }

                        May2 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1 - Sep1 - Oct1 - Nov1 - Dec1 - Jan2 - Feb2 - Mar2 - Apr2;

                        if ((Span1 < 520) && (Col520 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col520 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(519); }
                        if ((Span1 < 521) && (Col521 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col521 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(520); }
                        if ((Span1 < 522) && (Col522 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col522 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(521); }
                        if ((Span1 < 523) && (Col523 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col523 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(522); }
                        if ((Span1 < 524) && (Col524 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col524 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(523); }
                        if ((Span1 < 525) && (Col525 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col525 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(524); }
                        if ((Span1 < 526) && (Col526 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col526 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(525); }
                        if ((Span1 < 527) && (Col527 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col527 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(526); }
                        if ((Span1 < 528) && (Col528 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col528 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(527); }
                        if ((Span1 < 529) && (Col529 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col529 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(528); }
                        if ((Span1 < 530) && (Col530 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col530 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(529); }
                        if ((Span1 < 531) && (Col531 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col531 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(530); }
                        if ((Span1 < 532) && (Col532 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col532 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(531); }
                        if ((Span1 < 533) && (Col533 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col533 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(532); }
                        if ((Span1 < 534) && (Col534 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col534 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(533); }
                        if ((Span1 < 535) && (Col535 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col535 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(534); }
                        if ((Span1 < 536) && (Col536 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col536 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(535); }
                        if ((Span1 < 537) && (Col537 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col537 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(536); }
                        if ((Span1 < 538) && (Col538 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col538 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(537); }
                        if ((Span1 < 539) && (Col539 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col539 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(538); }
                        if ((Span1 < 540) && (Col540 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col540 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(539); }
                        if ((Span1 < 541) && (Col541 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col541 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(540); }
                        if ((Span1 < 542) && (Col542 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col542 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(541); }
                        if ((Span1 < 543) && (Col543 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col543 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(542); }
                        if ((Span1 < 544) && (Col544 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col544 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(543); }
                        if ((Span1 < 545) && (Col545 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col545 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(544); }
                        if ((Span1 < 546) && (Col546 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col546 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(545); }
                        if ((Span1 < 547) && (Col547 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col547 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(546); }
                        if ((Span1 < 548) && (Col548 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col548 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(547); }
                        if ((Span1 < 549) && (Col549 != "O") && (Math.Round(prog, 2) < Convert.ToDecimal(r["Length"].ToString()))) { Col549 = "A"; prog = prog + Convert.ToDecimal(r["advpershift"].ToString()) + remainder1; dur = dur + 1; EndDate = StartDate.AddDays(548); }
                        Jun2 = Math.Round(prog, 1) - Jan1 - Feb1 - Mar1 - Apr1 - May1 - Jun1 - Jul1 - Aug1 - Sep1 - Oct1 - Nov1 - Dec1 - Jan2 - Feb2 - Mar2 - Apr2 - May2;


                        if (DTPLeep1.Value.ToString("dd") != "29")
                        {

                            if (EndDate > StartDate.AddDays(59))
                                EndDate = EndDate.AddDays(-1);
                        }


                        if (DTPLeep2.Value.ToString("dd") != "29")
                        {
                            if (EndDate > StartDate.AddDays(425))
                                EndDate = EndDate.AddDays(-1);
                        }

                        if (LBookInt == 1) { Col1 = "B"; }
                        if (LBookInt == 2) { Col2 = "B"; }
                        if (LBookInt == 3) { Col3 = "B"; }
                        if (LBookInt == 4) { Col4 = "B"; }
                        if (LBookInt == 5) { Col5 = "B"; }
                        if (LBookInt == 6) { Col6 = "B"; }
                        if (LBookInt == 7) { Col7 = "B"; }
                        if (LBookInt == 8) { Col8 = "B"; }
                        if (LBookInt == 9) { Col9 = "B"; }
                        if (LBookInt == 10) { Col10 = "B"; }
                        if (LBookInt == 11) { Col11 = "B"; }
                        if (LBookInt == 12) { Col12 = "B"; }
                        if (LBookInt == 13) { Col13 = "B"; }
                        if (LBookInt == 14) { Col14 = "B"; }
                        if (LBookInt == 15) { Col15 = "B"; }
                        if (LBookInt == 16) { Col16 = "B"; }
                        if (LBookInt == 17) { Col17 = "B"; }
                        if (LBookInt == 18) { Col18 = "B"; }
                        if (LBookInt == 19) { Col19 = "B"; }
                        if (LBookInt == 20) { Col20 = "B"; }
                        if (LBookInt == 21) { Col21 = "B"; }
                        if (LBookInt == 22) { Col22 = "B"; }
                        if (LBookInt == 23) { Col23 = "B"; }
                        if (LBookInt == 24) { Col24 = "B"; }
                        if (LBookInt == 25) { Col25 = "B"; }
                        if (LBookInt == 26) { Col26 = "B"; }
                        if (LBookInt == 27) { Col27 = "B"; }
                        if (LBookInt == 28) { Col28 = "B"; }
                        if (LBookInt == 29) { Col29 = "B"; }
                        if (LBookInt == 30) { Col30 = "B"; }
                        if (LBookInt == 31) { Col31 = "B"; }
                        if (LBookInt == 32) { Col32 = "B"; }
                        if (LBookInt == 33) { Col33 = "B"; }
                        if (LBookInt == 34) { Col34 = "B"; }
                        if (LBookInt == 35) { Col35 = "B"; }
                        if (LBookInt == 36) { Col36 = "B"; }
                        if (LBookInt == 37) { Col37 = "B"; }
                        if (LBookInt == 38) { Col38 = "B"; }
                        if (LBookInt == 39) { Col39 = "B"; }
                        if (LBookInt == 40) { Col40 = "B"; }
                        if (LBookInt == 41) { Col41 = "B"; }
                        if (LBookInt == 42) { Col42 = "B"; }
                        if (LBookInt == 43) { Col43 = "B"; }
                        if (LBookInt == 44) { Col44 = "B"; }
                        if (LBookInt == 45) { Col45 = "B"; }
                        if (LBookInt == 46) { Col46 = "B"; }
                        if (LBookInt == 47) { Col47 = "B"; }
                        if (LBookInt == 48) { Col48 = "B"; }
                        if (LBookInt == 49) { Col49 = "B"; }
                        if (LBookInt == 50) { Col50 = "B"; }
                        if (LBookInt == 51) { Col51 = "B"; }
                        if (LBookInt == 52) { Col52 = "B"; }
                        if (LBookInt == 53) { Col53 = "B"; }
                        if (LBookInt == 54) { Col54 = "B"; }
                        if (LBookInt == 55) { Col55 = "B"; }
                        if (LBookInt == 56) { Col56 = "B"; }
                        if (LBookInt == 57) { Col57 = "B"; }
                        if (LBookInt == 58) { Col58 = "B"; }
                        if (LBookInt == 59) { Col59 = "B"; }
                        if (LBookInt == 60) { Col60 = "B"; }
                        if (LBookInt == 61) { Col61 = "B"; }
                        if (LBookInt == 62) { Col62 = "B"; }
                        if (LBookInt == 63) { Col63 = "B"; }
                        if (LBookInt == 64) { Col64 = "B"; }
                        if (LBookInt == 65) { Col65 = "B"; }
                        if (LBookInt == 66) { Col66 = "B"; }
                        if (LBookInt == 67) { Col67 = "B"; }
                        if (LBookInt == 68) { Col68 = "B"; }
                        if (LBookInt == 69) { Col69 = "B"; }
                        if (LBookInt == 70) { Col70 = "B"; }
                        if (LBookInt == 71) { Col71 = "B"; }
                        if (LBookInt == 72) { Col72 = "B"; }
                        if (LBookInt == 73) { Col73 = "B"; }
                        if (LBookInt == 74) { Col74 = "B"; }
                        if (LBookInt == 75) { Col75 = "B"; }
                        if (LBookInt == 76) { Col76 = "B"; }
                        if (LBookInt == 77) { Col77 = "B"; }
                        if (LBookInt == 78) { Col78 = "B"; }
                        if (LBookInt == 79) { Col79 = "B"; }
                        if (LBookInt == 80) { Col80 = "B"; }
                        if (LBookInt == 81) { Col81 = "B"; }
                        if (LBookInt == 82) { Col82 = "B"; }
                        if (LBookInt == 83) { Col83 = "B"; }
                        if (LBookInt == 84) { Col84 = "B"; }
                        if (LBookInt == 85) { Col85 = "B"; }
                        if (LBookInt == 86) { Col86 = "B"; }
                        if (LBookInt == 87) { Col87 = "B"; }
                        if (LBookInt == 88) { Col88 = "B"; }
                        if (LBookInt == 89) { Col89 = "B"; }
                        if (LBookInt == 90) { Col90 = "B"; }
                        if (LBookInt == 91) { Col91 = "B"; }
                        if (LBookInt == 92) { Col92 = "B"; }
                        if (LBookInt == 93) { Col93 = "B"; }
                        if (LBookInt == 94) { Col94 = "B"; }
                        if (LBookInt == 95) { Col95 = "B"; }
                        if (LBookInt == 96) { Col96 = "B"; }
                        if (LBookInt == 97) { Col97 = "B"; }
                        if (LBookInt == 98) { Col98 = "B"; }
                        if (LBookInt == 99) { Col99 = "B"; }
                        if (LBookInt == 100) { Col100 = "B"; }

                        if (LBookInt == 101) { Col101 = "B"; }
                        if (LBookInt == 102) { Col102 = "B"; }
                        if (LBookInt == 103) { Col103 = "B"; }
                        if (LBookInt == 104) { Col104 = "B"; }
                        if (LBookInt == 105) { Col105 = "B"; }
                        if (LBookInt == 106) { Col106 = "B"; }
                        if (LBookInt == 107) { Col107 = "B"; }
                        if (LBookInt == 108) { Col108 = "B"; }
                        if (LBookInt == 109) { Col109 = "B"; }
                        if (LBookInt == 110) { Col110 = "B"; }
                        if (LBookInt == 111) { Col111 = "B"; }
                        if (LBookInt == 112) { Col112 = "B"; }
                        if (LBookInt == 113) { Col113 = "B"; }
                        if (LBookInt == 114) { Col114 = "B"; }
                        if (LBookInt == 115) { Col115 = "B"; }
                        if (LBookInt == 116) { Col116 = "B"; }
                        if (LBookInt == 117) { Col117 = "B"; }
                        if (LBookInt == 118) { Col118 = "B"; }
                        if (LBookInt == 119) { Col119 = "B"; }
                        if (LBookInt == 120) { Col120 = "B"; }
                        if (LBookInt == 121) { Col121 = "B"; }
                        if (LBookInt == 122) { Col122 = "B"; }
                        if (LBookInt == 123) { Col123 = "B"; }
                        if (LBookInt == 124) { Col124 = "B"; }
                        if (LBookInt == 125) { Col125 = "B"; }
                        if (LBookInt == 126) { Col126 = "B"; }
                        if (LBookInt == 127) { Col127 = "B"; }
                        if (LBookInt == 128) { Col128 = "B"; }
                        if (LBookInt == 129) { Col129 = "B"; }
                        if (LBookInt == 130) { Col130 = "B"; }
                        if (LBookInt == 131) { Col131 = "B"; }
                        if (LBookInt == 132) { Col132 = "B"; }
                        if (LBookInt == 133) { Col133 = "B"; }
                        if (LBookInt == 134) { Col134 = "B"; }
                        if (LBookInt == 135) { Col135 = "B"; }
                        if (LBookInt == 136) { Col136 = "B"; }
                        if (LBookInt == 137) { Col137 = "B"; }
                        if (LBookInt == 138) { Col138 = "B"; }
                        if (LBookInt == 139) { Col139 = "B"; }
                        if (LBookInt == 140) { Col140 = "B"; }
                        if (LBookInt == 141) { Col141 = "B"; }
                        if (LBookInt == 142) { Col142 = "B"; }
                        if (LBookInt == 143) { Col143 = "B"; }
                        if (LBookInt == 144) { Col144 = "B"; }
                        if (LBookInt == 145) { Col145 = "B"; }
                        if (LBookInt == 146) { Col146 = "B"; }
                        if (LBookInt == 147) { Col147 = "B"; }
                        if (LBookInt == 148) { Col148 = "B"; }
                        if (LBookInt == 149) { Col149 = "B"; }
                        if (LBookInt == 150) { Col150 = "B"; }
                        if (LBookInt == 151) { Col151 = "B"; }
                        if (LBookInt == 152) { Col152 = "B"; }
                        if (LBookInt == 153) { Col153 = "B"; }
                        if (LBookInt == 154) { Col154 = "B"; }
                        if (LBookInt == 155) { Col155 = "B"; }
                        if (LBookInt == 156) { Col156 = "B"; }
                        if (LBookInt == 157) { Col157 = "B"; }
                        if (LBookInt == 158) { Col158 = "B"; }
                        if (LBookInt == 159) { Col159 = "B"; }
                        if (LBookInt == 160) { Col160 = "B"; }
                        if (LBookInt == 161) { Col161 = "B"; }
                        if (LBookInt == 162) { Col162 = "B"; }
                        if (LBookInt == 163) { Col163 = "B"; }
                        if (LBookInt == 164) { Col164 = "B"; }
                        if (LBookInt == 165) { Col165 = "B"; }
                        if (LBookInt == 166) { Col166 = "B"; }
                        if (LBookInt == 167) { Col167 = "B"; }
                        if (LBookInt == 168) { Col168 = "B"; }
                        if (LBookInt == 169) { Col169 = "B"; }
                        if (LBookInt == 170) { Col170 = "B"; }
                        if (LBookInt == 171) { Col171 = "B"; }
                        if (LBookInt == 172) { Col172 = "B"; }
                        if (LBookInt == 173) { Col173 = "B"; }
                        if (LBookInt == 174) { Col174 = "B"; }
                        if (LBookInt == 175) { Col175 = "B"; }
                        if (LBookInt == 176) { Col176 = "B"; }
                        if (LBookInt == 177) { Col177 = "B"; }
                        if (LBookInt == 178) { Col178 = "B"; }
                        if (LBookInt == 179) { Col179 = "B"; }
                        if (LBookInt == 180) { Col180 = "B"; }
                        if (LBookInt == 181) { Col181 = "B"; }
                        if (LBookInt == 182) { Col182 = "B"; }
                        if (LBookInt == 183) { Col183 = "B"; }
                        if (LBookInt == 184) { Col184 = "B"; }
                        if (LBookInt == 185) { Col185 = "B"; }
                        if (LBookInt == 186) { Col186 = "B"; }
                        if (LBookInt == 187) { Col187 = "B"; }
                        if (LBookInt == 188) { Col188 = "B"; }
                        if (LBookInt == 189) { Col189 = "B"; }
                        if (LBookInt == 190) { Col190 = "B"; }
                        if (LBookInt == 191) { Col191 = "B"; }
                        if (LBookInt == 192) { Col192 = "B"; }
                        if (LBookInt == 193) { Col193 = "B"; }
                        if (LBookInt == 194) { Col194 = "B"; }
                        if (LBookInt == 195) { Col195 = "B"; }
                        if (LBookInt == 196) { Col196 = "B"; }
                        if (LBookInt == 197) { Col197 = "B"; }
                        if (LBookInt == 198) { Col198 = "B"; }
                        if (LBookInt == 199) { Col199 = "B"; }
                        if (LBookInt == 200) { Col200 = "B"; }

                        if (LBookInt == 201) { Col201 = "B"; }
                        if (LBookInt == 202) { Col202 = "B"; }
                        if (LBookInt == 203) { Col203 = "B"; }
                        if (LBookInt == 204) { Col204 = "B"; }
                        if (LBookInt == 205) { Col205 = "B"; }
                        if (LBookInt == 206) { Col206 = "B"; }
                        if (LBookInt == 207) { Col207 = "B"; }
                        if (LBookInt == 208) { Col208 = "B"; }
                        if (LBookInt == 209) { Col209 = "B"; }
                        if (LBookInt == 210) { Col210 = "B"; }
                        if (LBookInt == 211) { Col211 = "B"; }
                        if (LBookInt == 212) { Col212 = "B"; }
                        if (LBookInt == 213) { Col213 = "B"; }
                        if (LBookInt == 214) { Col214 = "B"; }
                        if (LBookInt == 215) { Col215 = "B"; }
                        if (LBookInt == 216) { Col216 = "B"; }
                        if (LBookInt == 217) { Col217 = "B"; }
                        if (LBookInt == 218) { Col218 = "B"; }
                        if (LBookInt == 219) { Col219 = "B"; }
                        if (LBookInt == 220) { Col220 = "B"; }
                        if (LBookInt == 221) { Col221 = "B"; }
                        if (LBookInt == 222) { Col222 = "B"; }
                        if (LBookInt == 223) { Col223 = "B"; }
                        if (LBookInt == 224) { Col224 = "B"; }
                        if (LBookInt == 225) { Col225 = "B"; }
                        if (LBookInt == 226) { Col226 = "B"; }
                        if (LBookInt == 227) { Col227 = "B"; }
                        if (LBookInt == 228) { Col228 = "B"; }
                        if (LBookInt == 229) { Col229 = "B"; }
                        if (LBookInt == 230) { Col230 = "B"; }
                        if (LBookInt == 231) { Col231 = "B"; }
                        if (LBookInt == 232) { Col232 = "B"; }
                        if (LBookInt == 233) { Col233 = "B"; }
                        if (LBookInt == 234) { Col234 = "B"; }
                        if (LBookInt == 235) { Col235 = "B"; }
                        if (LBookInt == 236) { Col236 = "B"; }
                        if (LBookInt == 237) { Col237 = "B"; }
                        if (LBookInt == 238) { Col238 = "B"; }
                        if (LBookInt == 239) { Col239 = "B"; }
                        if (LBookInt == 240) { Col240 = "B"; }
                        if (LBookInt == 241) { Col241 = "B"; }
                        if (LBookInt == 242) { Col242 = "B"; }
                        if (LBookInt == 243) { Col243 = "B"; }
                        if (LBookInt == 244) { Col244 = "B"; }
                        if (LBookInt == 245) { Col245 = "B"; }
                        if (LBookInt == 246) { Col246 = "B"; }
                        if (LBookInt == 247) { Col247 = "B"; }
                        if (LBookInt == 248) { Col248 = "B"; }
                        if (LBookInt == 249) { Col249 = "B"; }
                        if (LBookInt == 250) { Col250 = "B"; }
                        if (LBookInt == 251) { Col251 = "B"; }
                        if (LBookInt == 252) { Col252 = "B"; }
                        if (LBookInt == 253) { Col253 = "B"; }
                        if (LBookInt == 254) { Col254 = "B"; }
                        if (LBookInt == 255) { Col255 = "B"; }
                        if (LBookInt == 256) { Col256 = "B"; }
                        if (LBookInt == 257) { Col257 = "B"; }
                        if (LBookInt == 258) { Col258 = "B"; }
                        if (LBookInt == 259) { Col259 = "B"; }
                        if (LBookInt == 260) { Col260 = "B"; }
                        if (LBookInt == 261) { Col261 = "B"; }
                        if (LBookInt == 262) { Col262 = "B"; }
                        if (LBookInt == 263) { Col263 = "B"; }
                        if (LBookInt == 264) { Col264 = "B"; }
                        if (LBookInt == 265) { Col265 = "B"; }
                        if (LBookInt == 266) { Col266 = "B"; }
                        if (LBookInt == 267) { Col267 = "B"; }
                        if (LBookInt == 268) { Col268 = "B"; }
                        if (LBookInt == 269) { Col269 = "B"; }
                        if (LBookInt == 270) { Col270 = "B"; }
                        if (LBookInt == 271) { Col271 = "B"; }
                        if (LBookInt == 272) { Col272 = "B"; }
                        if (LBookInt == 273) { Col273 = "B"; }
                        if (LBookInt == 274) { Col274 = "B"; }
                        if (LBookInt == 275) { Col275 = "B"; }
                        if (LBookInt == 276) { Col276 = "B"; }
                        if (LBookInt == 277) { Col277 = "B"; }
                        if (LBookInt == 278) { Col278 = "B"; }
                        if (LBookInt == 279) { Col279 = "B"; }
                        if (LBookInt == 280) { Col280 = "B"; }
                        if (LBookInt == 281) { Col281 = "B"; }
                        if (LBookInt == 282) { Col282 = "B"; }
                        if (LBookInt == 283) { Col283 = "B"; }
                        if (LBookInt == 284) { Col284 = "B"; }
                        if (LBookInt == 285) { Col285 = "B"; }
                        if (LBookInt == 286) { Col286 = "B"; }
                        if (LBookInt == 287) { Col287 = "B"; }
                        if (LBookInt == 288) { Col288 = "B"; }
                        if (LBookInt == 289) { Col289 = "B"; }
                        if (LBookInt == 290) { Col290 = "B"; }
                        if (LBookInt == 291) { Col291 = "B"; }
                        if (LBookInt == 292) { Col292 = "B"; }
                        if (LBookInt == 293) { Col293 = "B"; }
                        if (LBookInt == 294) { Col294 = "B"; }
                        if (LBookInt == 295) { Col295 = "B"; }
                        if (LBookInt == 296) { Col296 = "B"; }
                        if (LBookInt == 297) { Col297 = "B"; }
                        if (LBookInt == 298) { Col298 = "B"; }
                        if (LBookInt == 299) { Col299 = "B"; }
                        if (LBookInt == 300) { Col300 = "B"; }


                        if (LBookInt == 301) { Col301 = "B"; }
                        if (LBookInt == 302) { Col302 = "B"; }
                        if (LBookInt == 303) { Col303 = "B"; }
                        if (LBookInt == 304) { Col304 = "B"; }
                        if (LBookInt == 305) { Col305 = "B"; }
                        if (LBookInt == 306) { Col306 = "B"; }
                        if (LBookInt == 307) { Col307 = "B"; }
                        if (LBookInt == 308) { Col308 = "B"; }
                        if (LBookInt == 309) { Col309 = "B"; }
                        if (LBookInt == 310) { Col310 = "B"; }
                        if (LBookInt == 311) { Col311 = "B"; }
                        if (LBookInt == 312) { Col312 = "B"; }
                        if (LBookInt == 313) { Col313 = "B"; }
                        if (LBookInt == 314) { Col314 = "B"; }
                        if (LBookInt == 315) { Col315 = "B"; }
                        if (LBookInt == 316) { Col316 = "B"; }
                        if (LBookInt == 317) { Col317 = "B"; }
                        if (LBookInt == 318) { Col318 = "B"; }
                        if (LBookInt == 319) { Col319 = "B"; }
                        if (LBookInt == 320) { Col320 = "B"; }
                        if (LBookInt == 321) { Col321 = "B"; }
                        if (LBookInt == 322) { Col322 = "B"; }
                        if (LBookInt == 323) { Col323 = "B"; }
                        if (LBookInt == 324) { Col324 = "B"; }
                        if (LBookInt == 325) { Col325 = "B"; }
                        if (LBookInt == 326) { Col326 = "B"; }
                        if (LBookInt == 327) { Col327 = "B"; }
                        if (LBookInt == 328) { Col328 = "B"; }
                        if (LBookInt == 329) { Col329 = "B"; }
                        if (LBookInt == 330) { Col330 = "B"; }
                        if (LBookInt == 331) { Col331 = "B"; }
                        if (LBookInt == 332) { Col332 = "B"; }
                        if (LBookInt == 333) { Col333 = "B"; }
                        if (LBookInt == 334) { Col334 = "B"; }
                        if (LBookInt == 335) { Col335 = "B"; }
                        if (LBookInt == 336) { Col336 = "B"; }
                        if (LBookInt == 337) { Col337 = "B"; }
                        if (LBookInt == 338) { Col338 = "B"; }
                        if (LBookInt == 339) { Col339 = "B"; }
                        if (LBookInt == 340) { Col340 = "B"; }
                        if (LBookInt == 341) { Col341 = "B"; }
                        if (LBookInt == 342) { Col342 = "B"; }
                        if (LBookInt == 343) { Col343 = "B"; }
                        if (LBookInt == 344) { Col344 = "B"; }
                        if (LBookInt == 345) { Col345 = "B"; }
                        if (LBookInt == 346) { Col346 = "B"; }
                        if (LBookInt == 347) { Col347 = "B"; }
                        if (LBookInt == 348) { Col348 = "B"; }
                        if (LBookInt == 349) { Col349 = "B"; }
                        if (LBookInt == 350) { Col350 = "B"; }
                        if (LBookInt == 351) { Col351 = "B"; }
                        if (LBookInt == 352) { Col352 = "B"; }
                        if (LBookInt == 353) { Col353 = "B"; }
                        if (LBookInt == 354) { Col354 = "B"; }
                        if (LBookInt == 355) { Col355 = "B"; }
                        if (LBookInt == 356) { Col356 = "B"; }
                        if (LBookInt == 357) { Col357 = "B"; }
                        if (LBookInt == 358) { Col358 = "B"; }
                        if (LBookInt == 359) { Col359 = "B"; }
                        if (LBookInt == 360) { Col360 = "B"; }
                        if (LBookInt == 361) { Col361 = "B"; }
                        if (LBookInt == 362) { Col362 = "B"; }
                        if (LBookInt == 363) { Col363 = "B"; }
                        if (LBookInt == 364) { Col364 = "B"; }
                        if (LBookInt == 365) { Col365 = "B"; }
                        if (LBookInt == 366) { Col366 = "B"; }
                        if (LBookInt == 367) { Col367 = "B"; }
                        if (LBookInt == 368) { Col368 = "B"; }
                        if (LBookInt == 369) { Col369 = "B"; }
                        if (LBookInt == 370) { Col370 = "B"; }
                        if (LBookInt == 371) { Col371 = "B"; }
                        if (LBookInt == 372) { Col372 = "B"; }
                        if (LBookInt == 373) { Col373 = "B"; }
                        if (LBookInt == 374) { Col374 = "B"; }
                        if (LBookInt == 375) { Col375 = "B"; }
                        if (LBookInt == 376) { Col376 = "B"; }
                        if (LBookInt == 377) { Col377 = "B"; }
                        if (LBookInt == 378) { Col378 = "B"; }
                        if (LBookInt == 379) { Col379 = "B"; }
                        if (LBookInt == 380) { Col380 = "B"; }
                        if (LBookInt == 381) { Col381 = "B"; }
                        if (LBookInt == 382) { Col382 = "B"; }
                        if (LBookInt == 383) { Col383 = "B"; }
                        if (LBookInt == 384) { Col384 = "B"; }
                        if (LBookInt == 385) { Col385 = "B"; }
                        if (LBookInt == 386) { Col386 = "B"; }
                        if (LBookInt == 387) { Col387 = "B"; }
                        if (LBookInt == 388) { Col388 = "B"; }
                        if (LBookInt == 389) { Col389 = "B"; }
                        if (LBookInt == 390) { Col390 = "B"; }
                        if (LBookInt == 391) { Col391 = "B"; }
                        if (LBookInt == 392) { Col392 = "B"; }
                        if (LBookInt == 393) { Col393 = "B"; }
                        if (LBookInt == 394) { Col394 = "B"; }
                        if (LBookInt == 395) { Col395 = "B"; }
                        if (LBookInt == 396) { Col396 = "B"; }
                        if (LBookInt == 397) { Col397 = "B"; }
                        if (LBookInt == 398) { Col398 = "B"; }
                        if (LBookInt == 399) { Col399 = "B"; }
                        if (LBookInt == 400) { Col400 = "B"; }


                        if (LBookInt == 401) { Col401 = "B"; }
                        if (LBookInt == 402) { Col402 = "B"; }
                        if (LBookInt == 403) { Col403 = "B"; }
                        if (LBookInt == 404) { Col404 = "B"; }
                        if (LBookInt == 405) { Col405 = "B"; }
                        if (LBookInt == 406) { Col406 = "B"; }
                        if (LBookInt == 407) { Col407 = "B"; }
                        if (LBookInt == 408) { Col408 = "B"; }
                        if (LBookInt == 409) { Col409 = "B"; }
                        if (LBookInt == 410) { Col410 = "B"; }
                        if (LBookInt == 411) { Col411 = "B"; }
                        if (LBookInt == 412) { Col412 = "B"; }
                        if (LBookInt == 413) { Col413 = "B"; }
                        if (LBookInt == 414) { Col414 = "B"; }
                        if (LBookInt == 415) { Col415 = "B"; }
                        if (LBookInt == 416) { Col416 = "B"; }
                        if (LBookInt == 417) { Col417 = "B"; }
                        if (LBookInt == 418) { Col418 = "B"; }
                        if (LBookInt == 419) { Col419 = "B"; }
                        if (LBookInt == 420) { Col420 = "B"; }
                        if (LBookInt == 421) { Col421 = "B"; }
                        if (LBookInt == 422) { Col422 = "B"; }
                        if (LBookInt == 423) { Col423 = "B"; }
                        if (LBookInt == 424) { Col424 = "B"; }
                        if (LBookInt == 425) { Col425 = "B"; }
                        if (LBookInt == 426) { Col426 = "B"; }
                        if (LBookInt == 427) { Col427 = "B"; }
                        if (LBookInt == 428) { Col428 = "B"; }
                        if (LBookInt == 429) { Col429 = "B"; }
                        if (LBookInt == 430) { Col430 = "B"; }
                        if (LBookInt == 431) { Col431 = "B"; }
                        if (LBookInt == 432) { Col432 = "B"; }
                        if (LBookInt == 433) { Col433 = "B"; }
                        if (LBookInt == 434) { Col434 = "B"; }
                        if (LBookInt == 435) { Col435 = "B"; }
                        if (LBookInt == 436) { Col436 = "B"; }
                        if (LBookInt == 437) { Col437 = "B"; }
                        if (LBookInt == 438) { Col438 = "B"; }
                        if (LBookInt == 439) { Col439 = "B"; }
                        if (LBookInt == 440) { Col440 = "B"; }
                        if (LBookInt == 441) { Col441 = "B"; }
                        if (LBookInt == 442) { Col442 = "B"; }
                        if (LBookInt == 443) { Col443 = "B"; }
                        if (LBookInt == 444) { Col444 = "B"; }
                        if (LBookInt == 445) { Col445 = "B"; }
                        if (LBookInt == 446) { Col446 = "B"; }
                        if (LBookInt == 447) { Col447 = "B"; }
                        if (LBookInt == 448) { Col448 = "B"; }
                        if (LBookInt == 449) { Col449 = "B"; }
                        if (LBookInt == 450) { Col450 = "B"; }
                        if (LBookInt == 451) { Col451 = "B"; }
                        if (LBookInt == 452) { Col452 = "B"; }
                        if (LBookInt == 453) { Col453 = "B"; }
                        if (LBookInt == 454) { Col454 = "B"; }
                        if (LBookInt == 455) { Col455 = "B"; }
                        if (LBookInt == 456) { Col456 = "B"; }
                        if (LBookInt == 457) { Col457 = "B"; }
                        if (LBookInt == 458) { Col458 = "B"; }
                        if (LBookInt == 459) { Col459 = "B"; }
                        if (LBookInt == 460) { Col460 = "B"; }
                        if (LBookInt == 461) { Col461 = "B"; }
                        if (LBookInt == 462) { Col462 = "B"; }
                        if (LBookInt == 463) { Col463 = "B"; }
                        if (LBookInt == 464) { Col464 = "B"; }
                        if (LBookInt == 465) { Col465 = "B"; }
                        if (LBookInt == 466) { Col466 = "B"; }
                        if (LBookInt == 467) { Col467 = "B"; }
                        if (LBookInt == 468) { Col468 = "B"; }
                        if (LBookInt == 469) { Col469 = "B"; }
                        if (LBookInt == 470) { Col470 = "B"; }
                        if (LBookInt == 471) { Col471 = "B"; }
                        if (LBookInt == 472) { Col472 = "B"; }
                        if (LBookInt == 473) { Col473 = "B"; }
                        if (LBookInt == 474) { Col474 = "B"; }
                        if (LBookInt == 475) { Col475 = "B"; }
                        if (LBookInt == 476) { Col476 = "B"; }
                        if (LBookInt == 477) { Col477 = "B"; }
                        if (LBookInt == 478) { Col478 = "B"; }
                        if (LBookInt == 479) { Col479 = "B"; }
                        if (LBookInt == 480) { Col480 = "B"; }
                        if (LBookInt == 481) { Col481 = "B"; }
                        if (LBookInt == 482) { Col482 = "B"; }
                        if (LBookInt == 483) { Col483 = "B"; }
                        if (LBookInt == 484) { Col484 = "B"; }
                        if (LBookInt == 485) { Col485 = "B"; }
                        if (LBookInt == 486) { Col486 = "B"; }
                        if (LBookInt == 487) { Col487 = "B"; }
                        if (LBookInt == 488) { Col488 = "B"; }
                        if (LBookInt == 489) { Col489 = "B"; }
                        if (LBookInt == 490) { Col490 = "B"; }
                        if (LBookInt == 491) { Col491 = "B"; }
                        if (LBookInt == 492) { Col492 = "B"; }
                        if (LBookInt == 493) { Col493 = "B"; }
                        if (LBookInt == 494) { Col494 = "B"; }
                        if (LBookInt == 495) { Col495 = "B"; }
                        if (LBookInt == 496) { Col496 = "B"; }
                        if (LBookInt == 497) { Col497 = "B"; }
                        if (LBookInt == 498) { Col498 = "B"; }
                        if (LBookInt == 499) { Col499 = "B"; }
                        if (LBookInt == 500) { Col500 = "B"; }


                        if (LBookInt == 501) { Col501 = "B"; }
                        if (LBookInt == 502) { Col502 = "B"; }
                        if (LBookInt == 503) { Col503 = "B"; }
                        if (LBookInt == 504) { Col504 = "B"; }
                        if (LBookInt == 505) { Col505 = "B"; }
                        if (LBookInt == 506) { Col506 = "B"; }
                        if (LBookInt == 507) { Col507 = "B"; }
                        if (LBookInt == 508) { Col508 = "B"; }
                        if (LBookInt == 509) { Col509 = "B"; }
                        if (LBookInt == 510) { Col510 = "B"; }
                        if (LBookInt == 511) { Col511 = "B"; }
                        if (LBookInt == 512) { Col512 = "B"; }
                        if (LBookInt == 513) { Col513 = "B"; }
                        if (LBookInt == 514) { Col514 = "B"; }
                        if (LBookInt == 515) { Col515 = "B"; }
                        if (LBookInt == 516) { Col516 = "B"; }
                        if (LBookInt == 517) { Col517 = "B"; }
                        if (LBookInt == 518) { Col518 = "B"; }
                        if (LBookInt == 519) { Col519 = "B"; }
                        if (LBookInt == 520) { Col520 = "B"; }
                        if (LBookInt == 521) { Col521 = "B"; }
                        if (LBookInt == 522) { Col522 = "B"; }
                        if (LBookInt == 523) { Col523 = "B"; }
                        if (LBookInt == 524) { Col524 = "B"; }
                        if (LBookInt == 525) { Col525 = "B"; }
                        if (LBookInt == 526) { Col526 = "B"; }
                        if (LBookInt == 527) { Col527 = "B"; }
                        if (LBookInt == 528) { Col528 = "B"; }
                        if (LBookInt == 529) { Col529 = "B"; }
                        if (LBookInt == 530) { Col530 = "B"; }
                        if (LBookInt == 531) { Col531 = "B"; }
                        if (LBookInt == 532) { Col532 = "B"; }
                        if (LBookInt == 533) { Col533 = "B"; }
                        if (LBookInt == 534) { Col534 = "B"; }
                        if (LBookInt == 535) { Col535 = "B"; }
                        if (LBookInt == 536) { Col536 = "B"; }
                        if (LBookInt == 537) { Col537 = "B"; }
                        if (LBookInt == 538) { Col538 = "B"; }
                        if (LBookInt == 539) { Col539 = "B"; }
                        if (LBookInt == 540) { Col540 = "B"; }
                        if (LBookInt == 541) { Col541 = "B"; }
                        if (LBookInt == 542) { Col542 = "B"; }
                        if (LBookInt == 543) { Col543 = "B"; }
                        if (LBookInt == 544) { Col544 = "B"; }
                        if (LBookInt == 545) { Col545 = "B"; }
                        if (LBookInt == 546) { Col546 = "B"; }
                        if (LBookInt == 547) { Col547 = "B"; }
                        if (LBookInt == 548) { Col548 = "B"; }
                        if (LBookInt == 549) { Col549 = "B"; }


                        if (r["comp"].ToString() == "Y")
                        {

                            if (LBookInt == 1) { Col1 = "C"; }
                            if (LBookInt == 2) { Col2 = "C"; }
                            if (LBookInt == 3) { Col3 = "C"; }
                            if (LBookInt == 4) { Col4 = "C"; }
                            if (LBookInt == 5) { Col5 = "C"; }
                            if (LBookInt == 6) { Col6 = "C"; }
                            if (LBookInt == 7) { Col7 = "C"; }
                            if (LBookInt == 8) { Col8 = "C"; }
                            if (LBookInt == 9) { Col9 = "C"; }
                            if (LBookInt == 10) { Col10 = "C"; }
                            if (LBookInt == 11) { Col11 = "C"; }
                            if (LBookInt == 12) { Col12 = "C"; }
                            if (LBookInt == 13) { Col13 = "C"; }
                            if (LBookInt == 14) { Col14 = "C"; }
                            if (LBookInt == 15) { Col15 = "C"; }
                            if (LBookInt == 16) { Col16 = "C"; }
                            if (LBookInt == 17) { Col17 = "C"; }
                            if (LBookInt == 18) { Col18 = "C"; }
                            if (LBookInt == 19) { Col19 = "C"; }
                            if (LBookInt == 20) { Col20 = "C"; }
                            if (LBookInt == 21) { Col21 = "C"; }
                            if (LBookInt == 22) { Col22 = "C"; }
                            if (LBookInt == 23) { Col23 = "C"; }
                            if (LBookInt == 24) { Col24 = "C"; }
                            if (LBookInt == 25) { Col25 = "C"; }
                            if (LBookInt == 26) { Col26 = "C"; }
                            if (LBookInt == 27) { Col27 = "C"; }
                            if (LBookInt == 28) { Col28 = "C"; }
                            if (LBookInt == 29) { Col29 = "C"; }
                            if (LBookInt == 30) { Col30 = "C"; }
                            if (LBookInt == 31) { Col31 = "C"; }
                            if (LBookInt == 32) { Col32 = "C"; }
                            if (LBookInt == 33) { Col33 = "C"; }
                            if (LBookInt == 34) { Col34 = "C"; }
                            if (LBookInt == 35) { Col35 = "C"; }
                            if (LBookInt == 36) { Col36 = "C"; }
                            if (LBookInt == 37) { Col37 = "C"; }
                            if (LBookInt == 38) { Col38 = "C"; }
                            if (LBookInt == 39) { Col39 = "C"; }
                            if (LBookInt == 40) { Col40 = "C"; }
                            if (LBookInt == 41) { Col41 = "C"; }
                            if (LBookInt == 42) { Col42 = "C"; }
                            if (LBookInt == 43) { Col43 = "C"; }
                            if (LBookInt == 44) { Col44 = "C"; }
                            if (LBookInt == 45) { Col45 = "C"; }
                            if (LBookInt == 46) { Col46 = "C"; }
                            if (LBookInt == 47) { Col47 = "C"; }
                            if (LBookInt == 48) { Col48 = "C"; }
                            if (LBookInt == 49) { Col49 = "C"; }
                            if (LBookInt == 50) { Col50 = "C"; }
                            if (LBookInt == 51) { Col51 = "C"; }
                            if (LBookInt == 52) { Col52 = "C"; }
                            if (LBookInt == 53) { Col53 = "C"; }
                            if (LBookInt == 54) { Col54 = "C"; }
                            if (LBookInt == 55) { Col55 = "C"; }
                            if (LBookInt == 56) { Col56 = "C"; }
                            if (LBookInt == 57) { Col57 = "C"; }
                            if (LBookInt == 58) { Col58 = "C"; }
                            if (LBookInt == 59) { Col59 = "C"; }
                            if (LBookInt == 60) { Col60 = "C"; }
                            if (LBookInt == 61) { Col61 = "C"; }
                            if (LBookInt == 62) { Col62 = "C"; }
                            if (LBookInt == 63) { Col63 = "C"; }
                            if (LBookInt == 64) { Col64 = "C"; }
                            if (LBookInt == 65) { Col65 = "C"; }
                            if (LBookInt == 66) { Col66 = "C"; }
                            if (LBookInt == 67) { Col67 = "C"; }
                            if (LBookInt == 68) { Col68 = "C"; }
                            if (LBookInt == 69) { Col69 = "C"; }
                            if (LBookInt == 70) { Col70 = "C"; }
                            if (LBookInt == 71) { Col71 = "C"; }
                            if (LBookInt == 72) { Col72 = "C"; }
                            if (LBookInt == 73) { Col73 = "C"; }
                            if (LBookInt == 74) { Col74 = "C"; }
                            if (LBookInt == 75) { Col75 = "C"; }
                            if (LBookInt == 76) { Col76 = "C"; }
                            if (LBookInt == 77) { Col77 = "C"; }
                            if (LBookInt == 78) { Col78 = "C"; }
                            if (LBookInt == 79) { Col79 = "C"; }
                            if (LBookInt == 80) { Col80 = "C"; }
                            if (LBookInt == 81) { Col81 = "C"; }
                            if (LBookInt == 82) { Col82 = "C"; }
                            if (LBookInt == 83) { Col83 = "C"; }
                            if (LBookInt == 84) { Col84 = "C"; }
                            if (LBookInt == 85) { Col85 = "C"; }
                            if (LBookInt == 86) { Col86 = "C"; }
                            if (LBookInt == 87) { Col87 = "C"; }
                            if (LBookInt == 88) { Col88 = "C"; }
                            if (LBookInt == 89) { Col89 = "C"; }
                            if (LBookInt == 90) { Col90 = "C"; }
                            if (LBookInt == 91) { Col91 = "C"; }
                            if (LBookInt == 92) { Col92 = "C"; }
                            if (LBookInt == 93) { Col93 = "C"; }
                            if (LBookInt == 94) { Col94 = "C"; }
                            if (LBookInt == 95) { Col95 = "C"; }
                            if (LBookInt == 96) { Col96 = "C"; }
                            if (LBookInt == 97) { Col97 = "C"; }
                            if (LBookInt == 98) { Col98 = "C"; }
                            if (LBookInt == 99) { Col99 = "C"; }
                            if (LBookInt == 100) { Col100 = "C"; }

                            if (LBookInt == 101) { Col101 = "C"; }
                            if (LBookInt == 102) { Col102 = "C"; }
                            if (LBookInt == 103) { Col103 = "C"; }
                            if (LBookInt == 104) { Col104 = "C"; }
                            if (LBookInt == 105) { Col105 = "C"; }
                            if (LBookInt == 106) { Col106 = "C"; }
                            if (LBookInt == 107) { Col107 = "C"; }
                            if (LBookInt == 108) { Col108 = "C"; }
                            if (LBookInt == 109) { Col109 = "C"; }
                            if (LBookInt == 110) { Col110 = "C"; }
                            if (LBookInt == 111) { Col111 = "C"; }
                            if (LBookInt == 112) { Col112 = "C"; }
                            if (LBookInt == 113) { Col113 = "C"; }
                            if (LBookInt == 114) { Col114 = "C"; }
                            if (LBookInt == 115) { Col115 = "C"; }
                            if (LBookInt == 116) { Col116 = "C"; }
                            if (LBookInt == 117) { Col117 = "C"; }
                            if (LBookInt == 118) { Col118 = "C"; }
                            if (LBookInt == 119) { Col119 = "C"; }
                            if (LBookInt == 120) { Col120 = "C"; }
                            if (LBookInt == 121) { Col121 = "C"; }
                            if (LBookInt == 122) { Col122 = "C"; }
                            if (LBookInt == 123) { Col123 = "C"; }
                            if (LBookInt == 124) { Col124 = "C"; }
                            if (LBookInt == 125) { Col125 = "C"; }
                            if (LBookInt == 126) { Col126 = "C"; }
                            if (LBookInt == 127) { Col127 = "C"; }
                            if (LBookInt == 128) { Col128 = "C"; }
                            if (LBookInt == 129) { Col129 = "C"; }
                            if (LBookInt == 130) { Col130 = "C"; }
                            if (LBookInt == 131) { Col131 = "C"; }
                            if (LBookInt == 132) { Col132 = "C"; }
                            if (LBookInt == 133) { Col133 = "C"; }
                            if (LBookInt == 134) { Col134 = "C"; }
                            if (LBookInt == 135) { Col135 = "C"; }
                            if (LBookInt == 136) { Col136 = "C"; }
                            if (LBookInt == 137) { Col137 = "C"; }
                            if (LBookInt == 138) { Col138 = "C"; }
                            if (LBookInt == 139) { Col139 = "C"; }
                            if (LBookInt == 140) { Col140 = "C"; }
                            if (LBookInt == 141) { Col141 = "C"; }
                            if (LBookInt == 142) { Col142 = "C"; }
                            if (LBookInt == 143) { Col143 = "C"; }
                            if (LBookInt == 144) { Col144 = "C"; }
                            if (LBookInt == 145) { Col145 = "C"; }
                            if (LBookInt == 146) { Col146 = "C"; }
                            if (LBookInt == 147) { Col147 = "C"; }
                            if (LBookInt == 148) { Col148 = "C"; }
                            if (LBookInt == 149) { Col149 = "C"; }
                            if (LBookInt == 150) { Col150 = "C"; }
                            if (LBookInt == 151) { Col151 = "C"; }
                            if (LBookInt == 152) { Col152 = "C"; }
                            if (LBookInt == 153) { Col153 = "C"; }
                            if (LBookInt == 154) { Col154 = "C"; }
                            if (LBookInt == 155) { Col155 = "C"; }
                            if (LBookInt == 156) { Col156 = "C"; }
                            if (LBookInt == 157) { Col157 = "C"; }
                            if (LBookInt == 158) { Col158 = "C"; }
                            if (LBookInt == 159) { Col159 = "C"; }
                            if (LBookInt == 160) { Col160 = "C"; }
                            if (LBookInt == 161) { Col161 = "C"; }
                            if (LBookInt == 162) { Col162 = "C"; }
                            if (LBookInt == 163) { Col163 = "C"; }
                            if (LBookInt == 164) { Col164 = "C"; }
                            if (LBookInt == 165) { Col165 = "C"; }
                            if (LBookInt == 166) { Col166 = "C"; }
                            if (LBookInt == 167) { Col167 = "C"; }
                            if (LBookInt == 168) { Col168 = "C"; }
                            if (LBookInt == 169) { Col169 = "C"; }
                            if (LBookInt == 170) { Col170 = "C"; }
                            if (LBookInt == 171) { Col171 = "C"; }
                            if (LBookInt == 172) { Col172 = "C"; }
                            if (LBookInt == 173) { Col173 = "C"; }
                            if (LBookInt == 174) { Col174 = "C"; }
                            if (LBookInt == 175) { Col175 = "C"; }
                            if (LBookInt == 176) { Col176 = "C"; }
                            if (LBookInt == 177) { Col177 = "C"; }
                            if (LBookInt == 178) { Col178 = "C"; }
                            if (LBookInt == 179) { Col179 = "C"; }
                            if (LBookInt == 180) { Col180 = "C"; }
                            if (LBookInt == 181) { Col181 = "C"; }
                            if (LBookInt == 182) { Col182 = "C"; }
                            if (LBookInt == 183) { Col183 = "C"; }
                            if (LBookInt == 184) { Col184 = "C"; }
                            if (LBookInt == 185) { Col185 = "C"; }
                            if (LBookInt == 186) { Col186 = "C"; }
                            if (LBookInt == 187) { Col187 = "C"; }
                            if (LBookInt == 188) { Col188 = "C"; }
                            if (LBookInt == 189) { Col189 = "C"; }
                            if (LBookInt == 190) { Col190 = "C"; }
                            if (LBookInt == 191) { Col191 = "C"; }
                            if (LBookInt == 192) { Col192 = "C"; }
                            if (LBookInt == 193) { Col193 = "C"; }
                            if (LBookInt == 194) { Col194 = "C"; }
                            if (LBookInt == 195) { Col195 = "C"; }
                            if (LBookInt == 196) { Col196 = "C"; }
                            if (LBookInt == 197) { Col197 = "C"; }
                            if (LBookInt == 198) { Col198 = "C"; }
                            if (LBookInt == 199) { Col199 = "C"; }
                            if (LBookInt == 200) { Col200 = "C"; }

                            if (LBookInt == 201) { Col201 = "C"; }
                            if (LBookInt == 202) { Col202 = "C"; }
                            if (LBookInt == 203) { Col203 = "C"; }
                            if (LBookInt == 204) { Col204 = "C"; }
                            if (LBookInt == 205) { Col205 = "C"; }
                            if (LBookInt == 206) { Col206 = "C"; }
                            if (LBookInt == 207) { Col207 = "C"; }
                            if (LBookInt == 208) { Col208 = "C"; }
                            if (LBookInt == 209) { Col209 = "C"; }
                            if (LBookInt == 210) { Col210 = "C"; }
                            if (LBookInt == 211) { Col211 = "C"; }
                            if (LBookInt == 212) { Col212 = "C"; }
                            if (LBookInt == 213) { Col213 = "C"; }
                            if (LBookInt == 214) { Col214 = "C"; }
                            if (LBookInt == 215) { Col215 = "C"; }
                            if (LBookInt == 216) { Col216 = "C"; }
                            if (LBookInt == 217) { Col217 = "C"; }
                            if (LBookInt == 218) { Col218 = "C"; }
                            if (LBookInt == 219) { Col219 = "C"; }
                            if (LBookInt == 220) { Col220 = "C"; }
                            if (LBookInt == 221) { Col221 = "C"; }
                            if (LBookInt == 222) { Col222 = "C"; }
                            if (LBookInt == 223) { Col223 = "C"; }
                            if (LBookInt == 224) { Col224 = "C"; }
                            if (LBookInt == 225) { Col225 = "C"; }
                            if (LBookInt == 226) { Col226 = "C"; }
                            if (LBookInt == 227) { Col227 = "C"; }
                            if (LBookInt == 228) { Col228 = "C"; }
                            if (LBookInt == 229) { Col229 = "C"; }
                            if (LBookInt == 230) { Col230 = "C"; }
                            if (LBookInt == 231) { Col231 = "C"; }
                            if (LBookInt == 232) { Col232 = "C"; }
                            if (LBookInt == 233) { Col233 = "C"; }
                            if (LBookInt == 234) { Col234 = "C"; }
                            if (LBookInt == 235) { Col235 = "C"; }
                            if (LBookInt == 236) { Col236 = "C"; }
                            if (LBookInt == 237) { Col237 = "C"; }
                            if (LBookInt == 238) { Col238 = "C"; }
                            if (LBookInt == 239) { Col239 = "C"; }
                            if (LBookInt == 240) { Col240 = "C"; }
                            if (LBookInt == 241) { Col241 = "C"; }
                            if (LBookInt == 242) { Col242 = "C"; }
                            if (LBookInt == 243) { Col243 = "C"; }
                            if (LBookInt == 244) { Col244 = "C"; }
                            if (LBookInt == 245) { Col245 = "C"; }
                            if (LBookInt == 246) { Col246 = "C"; }
                            if (LBookInt == 247) { Col247 = "C"; }
                            if (LBookInt == 248) { Col248 = "C"; }
                            if (LBookInt == 249) { Col249 = "C"; }
                            if (LBookInt == 250) { Col250 = "C"; }
                            if (LBookInt == 251) { Col251 = "C"; }
                            if (LBookInt == 252) { Col252 = "C"; }
                            if (LBookInt == 253) { Col253 = "C"; }
                            if (LBookInt == 254) { Col254 = "C"; }
                            if (LBookInt == 255) { Col255 = "C"; }
                            if (LBookInt == 256) { Col256 = "C"; }
                            if (LBookInt == 257) { Col257 = "C"; }
                            if (LBookInt == 258) { Col258 = "C"; }
                            if (LBookInt == 259) { Col259 = "C"; }
                            if (LBookInt == 260) { Col260 = "C"; }
                            if (LBookInt == 261) { Col261 = "C"; }
                            if (LBookInt == 262) { Col262 = "C"; }
                            if (LBookInt == 263) { Col263 = "C"; }
                            if (LBookInt == 264) { Col264 = "C"; }
                            if (LBookInt == 265) { Col265 = "C"; }
                            if (LBookInt == 266) { Col266 = "C"; }
                            if (LBookInt == 267) { Col267 = "C"; }
                            if (LBookInt == 268) { Col268 = "C"; }
                            if (LBookInt == 269) { Col269 = "C"; }
                            if (LBookInt == 270) { Col270 = "C"; }
                            if (LBookInt == 271) { Col271 = "C"; }
                            if (LBookInt == 272) { Col272 = "C"; }
                            if (LBookInt == 273) { Col273 = "C"; }
                            if (LBookInt == 274) { Col274 = "C"; }
                            if (LBookInt == 275) { Col275 = "C"; }
                            if (LBookInt == 276) { Col276 = "C"; }
                            if (LBookInt == 277) { Col277 = "C"; }
                            if (LBookInt == 278) { Col278 = "C"; }
                            if (LBookInt == 279) { Col279 = "C"; }
                            if (LBookInt == 280) { Col280 = "C"; }
                            if (LBookInt == 281) { Col281 = "C"; }
                            if (LBookInt == 282) { Col282 = "C"; }
                            if (LBookInt == 283) { Col283 = "C"; }
                            if (LBookInt == 284) { Col284 = "C"; }
                            if (LBookInt == 285) { Col285 = "C"; }
                            if (LBookInt == 286) { Col286 = "C"; }
                            if (LBookInt == 287) { Col287 = "C"; }
                            if (LBookInt == 288) { Col288 = "C"; }
                            if (LBookInt == 289) { Col289 = "C"; }
                            if (LBookInt == 290) { Col290 = "C"; }
                            if (LBookInt == 291) { Col291 = "C"; }
                            if (LBookInt == 292) { Col292 = "C"; }
                            if (LBookInt == 293) { Col293 = "C"; }
                            if (LBookInt == 294) { Col294 = "C"; }
                            if (LBookInt == 295) { Col295 = "C"; }
                            if (LBookInt == 296) { Col296 = "C"; }
                            if (LBookInt == 297) { Col297 = "C"; }
                            if (LBookInt == 298) { Col298 = "C"; }
                            if (LBookInt == 299) { Col299 = "C"; }
                            if (LBookInt == 300) { Col300 = "C"; }


                            if (LBookInt == 301) { Col301 = "C"; }
                            if (LBookInt == 302) { Col302 = "C"; }
                            if (LBookInt == 303) { Col303 = "C"; }
                            if (LBookInt == 304) { Col304 = "C"; }
                            if (LBookInt == 305) { Col305 = "C"; }
                            if (LBookInt == 306) { Col306 = "C"; }
                            if (LBookInt == 307) { Col307 = "C"; }
                            if (LBookInt == 308) { Col308 = "C"; }
                            if (LBookInt == 309) { Col309 = "C"; }
                            if (LBookInt == 310) { Col310 = "C"; }
                            if (LBookInt == 311) { Col311 = "C"; }
                            if (LBookInt == 312) { Col312 = "C"; }
                            if (LBookInt == 313) { Col313 = "C"; }
                            if (LBookInt == 314) { Col314 = "C"; }
                            if (LBookInt == 315) { Col315 = "C"; }
                            if (LBookInt == 316) { Col316 = "C"; }
                            if (LBookInt == 317) { Col317 = "C"; }
                            if (LBookInt == 318) { Col318 = "C"; }
                            if (LBookInt == 319) { Col319 = "C"; }
                            if (LBookInt == 320) { Col320 = "C"; }
                            if (LBookInt == 321) { Col321 = "C"; }
                            if (LBookInt == 322) { Col322 = "C"; }
                            if (LBookInt == 323) { Col323 = "C"; }
                            if (LBookInt == 324) { Col324 = "C"; }
                            if (LBookInt == 325) { Col325 = "C"; }
                            if (LBookInt == 326) { Col326 = "C"; }
                            if (LBookInt == 327) { Col327 = "C"; }
                            if (LBookInt == 328) { Col328 = "C"; }
                            if (LBookInt == 329) { Col329 = "C"; }
                            if (LBookInt == 330) { Col330 = "C"; }
                            if (LBookInt == 331) { Col331 = "C"; }
                            if (LBookInt == 332) { Col332 = "C"; }
                            if (LBookInt == 333) { Col333 = "C"; }
                            if (LBookInt == 334) { Col334 = "C"; }
                            if (LBookInt == 335) { Col335 = "C"; }
                            if (LBookInt == 336) { Col336 = "C"; }
                            if (LBookInt == 337) { Col337 = "C"; }
                            if (LBookInt == 338) { Col338 = "C"; }
                            if (LBookInt == 339) { Col339 = "C"; }
                            if (LBookInt == 340) { Col340 = "C"; }
                            if (LBookInt == 341) { Col341 = "C"; }
                            if (LBookInt == 342) { Col342 = "C"; }
                            if (LBookInt == 343) { Col343 = "C"; }
                            if (LBookInt == 344) { Col344 = "C"; }
                            if (LBookInt == 345) { Col345 = "C"; }
                            if (LBookInt == 346) { Col346 = "C"; }
                            if (LBookInt == 347) { Col347 = "C"; }
                            if (LBookInt == 348) { Col348 = "C"; }
                            if (LBookInt == 349) { Col349 = "C"; }
                            if (LBookInt == 350) { Col350 = "C"; }
                            if (LBookInt == 351) { Col351 = "C"; }
                            if (LBookInt == 352) { Col352 = "C"; }
                            if (LBookInt == 353) { Col353 = "C"; }
                            if (LBookInt == 354) { Col354 = "C"; }
                            if (LBookInt == 355) { Col355 = "C"; }
                            if (LBookInt == 356) { Col356 = "C"; }
                            if (LBookInt == 357) { Col357 = "C"; }
                            if (LBookInt == 358) { Col358 = "C"; }
                            if (LBookInt == 359) { Col359 = "C"; }
                            if (LBookInt == 360) { Col360 = "C"; }
                            if (LBookInt == 361) { Col361 = "C"; }
                            if (LBookInt == 362) { Col362 = "C"; }
                            if (LBookInt == 363) { Col363 = "C"; }
                            if (LBookInt == 364) { Col364 = "C"; }
                            if (LBookInt == 365) { Col365 = "C"; }
                            if (LBookInt == 366) { Col366 = "C"; }
                            if (LBookInt == 367) { Col367 = "C"; }
                            if (LBookInt == 368) { Col368 = "C"; }
                            if (LBookInt == 369) { Col369 = "C"; }
                            if (LBookInt == 370) { Col370 = "C"; }
                            if (LBookInt == 371) { Col371 = "C"; }
                            if (LBookInt == 372) { Col372 = "C"; }
                            if (LBookInt == 373) { Col373 = "C"; }
                            if (LBookInt == 374) { Col374 = "C"; }
                            if (LBookInt == 375) { Col375 = "C"; }
                            if (LBookInt == 376) { Col376 = "C"; }
                            if (LBookInt == 377) { Col377 = "C"; }
                            if (LBookInt == 378) { Col378 = "C"; }
                            if (LBookInt == 379) { Col379 = "C"; }
                            if (LBookInt == 380) { Col380 = "C"; }
                            if (LBookInt == 381) { Col381 = "C"; }
                            if (LBookInt == 382) { Col382 = "C"; }
                            if (LBookInt == 383) { Col383 = "C"; }
                            if (LBookInt == 384) { Col384 = "C"; }
                            if (LBookInt == 385) { Col385 = "C"; }
                            if (LBookInt == 386) { Col386 = "C"; }
                            if (LBookInt == 387) { Col387 = "C"; }
                            if (LBookInt == 388) { Col388 = "C"; }
                            if (LBookInt == 389) { Col389 = "C"; }
                            if (LBookInt == 390) { Col390 = "C"; }
                            if (LBookInt == 391) { Col391 = "C"; }
                            if (LBookInt == 392) { Col392 = "C"; }
                            if (LBookInt == 393) { Col393 = "C"; }
                            if (LBookInt == 394) { Col394 = "C"; }
                            if (LBookInt == 395) { Col395 = "C"; }
                            if (LBookInt == 396) { Col396 = "C"; }
                            if (LBookInt == 397) { Col397 = "C"; }
                            if (LBookInt == 398) { Col398 = "C"; }
                            if (LBookInt == 399) { Col399 = "C"; }
                            if (LBookInt == 400) { Col400 = "C"; }


                            if (LBookInt == 401) { Col401 = "C"; }
                            if (LBookInt == 402) { Col402 = "C"; }
                            if (LBookInt == 403) { Col403 = "C"; }
                            if (LBookInt == 404) { Col404 = "C"; }
                            if (LBookInt == 405) { Col405 = "C"; }
                            if (LBookInt == 406) { Col406 = "C"; }
                            if (LBookInt == 407) { Col407 = "C"; }
                            if (LBookInt == 408) { Col408 = "C"; }
                            if (LBookInt == 409) { Col409 = "C"; }
                            if (LBookInt == 410) { Col410 = "C"; }
                            if (LBookInt == 411) { Col411 = "C"; }
                            if (LBookInt == 412) { Col412 = "C"; }
                            if (LBookInt == 413) { Col413 = "C"; }
                            if (LBookInt == 414) { Col414 = "C"; }
                            if (LBookInt == 415) { Col415 = "C"; }
                            if (LBookInt == 416) { Col416 = "C"; }
                            if (LBookInt == 417) { Col417 = "C"; }
                            if (LBookInt == 418) { Col418 = "C"; }
                            if (LBookInt == 419) { Col419 = "C"; }
                            if (LBookInt == 420) { Col420 = "C"; }
                            if (LBookInt == 421) { Col421 = "C"; }
                            if (LBookInt == 422) { Col422 = "C"; }
                            if (LBookInt == 423) { Col423 = "C"; }
                            if (LBookInt == 424) { Col424 = "C"; }
                            if (LBookInt == 425) { Col425 = "C"; }
                            if (LBookInt == 426) { Col426 = "C"; }
                            if (LBookInt == 427) { Col427 = "C"; }
                            if (LBookInt == 428) { Col428 = "C"; }
                            if (LBookInt == 429) { Col429 = "C"; }
                            if (LBookInt == 430) { Col430 = "C"; }
                            if (LBookInt == 431) { Col431 = "C"; }
                            if (LBookInt == 432) { Col432 = "C"; }
                            if (LBookInt == 433) { Col433 = "C"; }
                            if (LBookInt == 434) { Col434 = "C"; }
                            if (LBookInt == 435) { Col435 = "C"; }
                            if (LBookInt == 436) { Col436 = "C"; }
                            if (LBookInt == 437) { Col437 = "C"; }
                            if (LBookInt == 438) { Col438 = "C"; }
                            if (LBookInt == 439) { Col439 = "C"; }
                            if (LBookInt == 440) { Col440 = "C"; }
                            if (LBookInt == 441) { Col441 = "C"; }
                            if (LBookInt == 442) { Col442 = "C"; }
                            if (LBookInt == 443) { Col443 = "C"; }
                            if (LBookInt == 444) { Col444 = "C"; }
                            if (LBookInt == 445) { Col445 = "C"; }
                            if (LBookInt == 446) { Col446 = "C"; }
                            if (LBookInt == 447) { Col447 = "C"; }
                            if (LBookInt == 448) { Col448 = "C"; }
                            if (LBookInt == 449) { Col449 = "C"; }
                            if (LBookInt == 450) { Col450 = "C"; }
                            if (LBookInt == 451) { Col451 = "C"; }
                            if (LBookInt == 452) { Col452 = "C"; }
                            if (LBookInt == 453) { Col453 = "C"; }
                            if (LBookInt == 454) { Col454 = "C"; }
                            if (LBookInt == 455) { Col455 = "C"; }
                            if (LBookInt == 456) { Col456 = "C"; }
                            if (LBookInt == 457) { Col457 = "C"; }
                            if (LBookInt == 458) { Col458 = "C"; }
                            if (LBookInt == 459) { Col459 = "C"; }
                            if (LBookInt == 460) { Col460 = "C"; }
                            if (LBookInt == 461) { Col461 = "C"; }
                            if (LBookInt == 462) { Col462 = "C"; }
                            if (LBookInt == 463) { Col463 = "C"; }
                            if (LBookInt == 464) { Col464 = "C"; }
                            if (LBookInt == 465) { Col465 = "C"; }
                            if (LBookInt == 466) { Col466 = "C"; }
                            if (LBookInt == 467) { Col467 = "C"; }
                            if (LBookInt == 468) { Col468 = "C"; }
                            if (LBookInt == 469) { Col469 = "C"; }
                            if (LBookInt == 470) { Col470 = "C"; }
                            if (LBookInt == 471) { Col471 = "C"; }
                            if (LBookInt == 472) { Col472 = "C"; }
                            if (LBookInt == 473) { Col473 = "C"; }
                            if (LBookInt == 474) { Col474 = "C"; }
                            if (LBookInt == 475) { Col475 = "C"; }
                            if (LBookInt == 476) { Col476 = "C"; }
                            if (LBookInt == 477) { Col477 = "C"; }
                            if (LBookInt == 478) { Col478 = "C"; }
                            if (LBookInt == 479) { Col479 = "C"; }
                            if (LBookInt == 480) { Col480 = "C"; }
                            if (LBookInt == 481) { Col481 = "C"; }
                            if (LBookInt == 482) { Col482 = "C"; }
                            if (LBookInt == 483) { Col483 = "C"; }
                            if (LBookInt == 484) { Col484 = "C"; }
                            if (LBookInt == 485) { Col485 = "C"; }
                            if (LBookInt == 486) { Col486 = "C"; }
                            if (LBookInt == 487) { Col487 = "C"; }
                            if (LBookInt == 488) { Col488 = "C"; }
                            if (LBookInt == 489) { Col489 = "C"; }
                            if (LBookInt == 490) { Col490 = "C"; }
                            if (LBookInt == 491) { Col491 = "C"; }
                            if (LBookInt == 492) { Col492 = "C"; }
                            if (LBookInt == 493) { Col493 = "C"; }
                            if (LBookInt == 494) { Col494 = "C"; }
                            if (LBookInt == 495) { Col495 = "C"; }
                            if (LBookInt == 496) { Col496 = "C"; }
                            if (LBookInt == 497) { Col497 = "C"; }
                            if (LBookInt == 498) { Col498 = "C"; }
                            if (LBookInt == 499) { Col499 = "C"; }
                            if (LBookInt == 500) { Col500 = "C"; }


                            if (LBookInt == 501) { Col501 = "C"; }
                            if (LBookInt == 502) { Col502 = "C"; }
                            if (LBookInt == 503) { Col503 = "C"; }
                            if (LBookInt == 504) { Col504 = "C"; }
                            if (LBookInt == 505) { Col505 = "C"; }
                            if (LBookInt == 506) { Col506 = "C"; }
                            if (LBookInt == 507) { Col507 = "C"; }
                            if (LBookInt == 508) { Col508 = "C"; }
                            if (LBookInt == 509) { Col509 = "C"; }
                            if (LBookInt == 510) { Col510 = "C"; }
                            if (LBookInt == 511) { Col511 = "C"; }
                            if (LBookInt == 512) { Col512 = "C"; }
                            if (LBookInt == 513) { Col513 = "C"; }
                            if (LBookInt == 514) { Col514 = "C"; }
                            if (LBookInt == 515) { Col515 = "C"; }
                            if (LBookInt == 516) { Col516 = "C"; }
                            if (LBookInt == 517) { Col517 = "C"; }
                            if (LBookInt == 518) { Col518 = "C"; }
                            if (LBookInt == 519) { Col519 = "C"; }
                            if (LBookInt == 520) { Col520 = "C"; }
                            if (LBookInt == 521) { Col521 = "C"; }
                            if (LBookInt == 522) { Col522 = "C"; }
                            if (LBookInt == 523) { Col523 = "C"; }
                            if (LBookInt == 524) { Col524 = "C"; }
                            if (LBookInt == 525) { Col525 = "C"; }
                            if (LBookInt == 526) { Col526 = "C"; }
                            if (LBookInt == 527) { Col527 = "C"; }
                            if (LBookInt == 528) { Col528 = "C"; }
                            if (LBookInt == 529) { Col529 = "C"; }
                            if (LBookInt == 530) { Col530 = "C"; }
                            if (LBookInt == 531) { Col531 = "C"; }
                            if (LBookInt == 532) { Col532 = "C"; }
                            if (LBookInt == 533) { Col533 = "C"; }
                            if (LBookInt == 534) { Col534 = "C"; }
                            if (LBookInt == 535) { Col535 = "C"; }
                            if (LBookInt == 536) { Col536 = "C"; }
                            if (LBookInt == 537) { Col537 = "C"; }
                            if (LBookInt == 538) { Col538 = "C"; }
                            if (LBookInt == 539) { Col539 = "C"; }
                            if (LBookInt == 540) { Col540 = "C"; }
                            if (LBookInt == 541) { Col541 = "C"; }
                            if (LBookInt == 542) { Col542 = "C"; }
                            if (LBookInt == 543) { Col543 = "C"; }
                            if (LBookInt == 544) { Col544 = "C"; }
                            if (LBookInt == 545) { Col545 = "C"; }
                            if (LBookInt == 546) { Col546 = "C"; }
                            if (LBookInt == 547) { Col547 = "C"; }
                            if (LBookInt == 548) { Col548 = "C"; }
                            if (LBookInt == 549) { Col549 = "C"; }


                        }


                        _dbManUpdate.SqlStatement = _dbManUpdate.SqlStatement + " update [tbl_GeoScience_PlanLongTerm] set compdate = '" + String.Format("{0:yyyy-MM-dd}", EndDate) + "', dur = '" + dur.ToString() + "' where machineno =  '" + r["MachineNo"].ToString() + "' ";
                        _dbManUpdate.SqlStatement = _dbManUpdate.SqlStatement + " and Workplace =  '" + r["Workplace"].ToString() + "'  and HoleNo =  '" + r["HoleNo"].ToString() + "' ";
                    }


                }

                string doc = "";
                if (r["document"].ToString() == "1")
                {
                    doc = r["sss"].ToString();
                }
                else
                {
                    doc = r["sss"].ToString() + "  ";
                }

                table.Rows.Add(r["MachineNo"].ToString(), r["Workplace"].ToString(), r["HoleNo"].ToString(), r["PrevWorkplace"].ToString(), r["Length"].ToString(), doc, dur.ToString(),//r["AddDelay"].ToString(),
                    Col1, Col2, Col3, Col4, Col5, Col6, Col7, Col8, Col9, Col10, Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20,
                    Col21, Col22, Col23, Col24, Col25, Col26, Col27, Col28, Col29, Col30, Col31, Col32, Col33, Col34, Col35, Col36, Col37, Col38, Col39, Col40,
                    Col41, Col42, Col43, Col44, Col45, Col46, Col47, Col48, Col49, Col50, Col51, Col52, Col53, Col54, Col55, Col56, Col57, Col58, Col59, Col60,
                    Col61, Col62, Col63, Col64, Col65, Col66, Col67, Col68, Col69, Col70, Col71, Col72, Col73, Col74, Col75, Col76, Col77, Col78, Col79, Col80,
                    Col81, Col82, Col83, Col84, Col85, Col86, Col87, Col88, Col89, Col90, Col91, Col92, Col93, Col94, Col95, Col96, Col97, Col98, Col99, Col100,

                    Col101, Col102, Col103, Col104, Col105, Col106, Col107, Col108, Col109, Col110, Col111, Col112, Col113, Col114, Col115, Col116, Col117, Col118, Col119, Col120,
                    Col121, Col122, Col123, Col124, Col125, Col126, Col127, Col128, Col129, Col130, Col131, Col132, Col133, Col134, Col135, Col136, Col137, Col138, Col139, Col140,
                    Col141, Col142, Col143, Col144, Col145, Col146, Col147, Col148, Col149, Col150, Col151, Col152, Col153, Col154, Col155, Col156, Col157, Col158, Col159, Col160,
                    Col161, Col162, Col163, Col164, Col165, Col166, Col167, Col168, Col169, Col170, Col171, Col172, Col173, Col174, Col175, Col176, Col177, Col178, Col179, Col180,
                    Col181, Col182, Col183, Col184, Col185, Col186, Col187, Col188, Col189, Col190, Col191, Col192, Col193, Col194, Col195, Col196, Col197, Col198, Col199, Col200,

                    Col201, Col202, Col203, Col204, Col205, Col206, Col207, Col208, Col209, Col210, Col211, Col212, Col213, Col214, Col215, Col216, Col217, Col218, Col219, Col220,
                    Col221, Col222, Col223, Col224, Col225, Col226, Col227, Col228, Col229, Col230, Col231, Col232, Col233, Col234, Col235, Col236, Col237, Col238, Col239, Col240,
                    Col241, Col242, Col243, Col244, Col245, Col246, Col247, Col248, Col249, Col250, Col251, Col252, Col253, Col254, Col255, Col256, Col257, Col258, Col259, Col260,
                    Col261, Col262, Col263, Col264, Col265, Col266, Col267, Col268, Col269, Col270, Col271, Col272, Col273, Col274, Col275, Col276, Col277, Col278, Col279, Col280,
                    Col281, Col282, Col283, Col284, Col285, Col286, Col287, Col288, Col289, Col290, Col291, Col292, Col293, Col294, Col295, Col296, Col297, Col298, Col299, Col300,


                    Col301, Col302, Col303, Col304, Col305, Col306, Col307, Col308, Col309, Col310, Col311, Col312, Col313, Col314, Col315, Col316, Col317, Col318, Col319, Col320,
                    Col321, Col322, Col323, Col324, Col325, Col326, Col327, Col328, Col329, Col330, Col331, Col332, Col333, Col334, Col335, Col336, Col337, Col338, Col339, Col340,
                    Col341, Col342, Col343, Col344, Col345, Col346, Col347, Col348, Col349, Col350, Col351, Col352, Col353, Col354, Col355, Col356, Col357, Col358, Col359, Col360,
                    Col361, Col362, Col363, Col364, Col365, Col366, Col367, Col368, Col369, Col370, Col371, Col372, Col373, Col374, Col375, Col376, Col377, Col378, Col379, Col380,
                    Col381, Col382, Col383, Col384, Col385, Col386, Col387, Col388, Col389, Col390, Col391, Col392, Col393, Col394, Col395, Col396, Col397, Col398, Col399, Col400,

                   Col401, Col402, Col403, Col404, Col405, Col406, Col407, Col408, Col409, Col410, Col411, Col412, Col413, Col414, Col415, Col416,
                   Col417, Col418, Col419, Col420, Col421, Col422, Col423, Col424, Col425, Col426, Col427, Col428, Col429, Col430, Col431, Col432,
                   Col433, Col434, Col435, Col436, Col437, Col438, Col439, Col440, Col441, Col442, Col443, Col444, Col445, Col446, Col447, Col448,
                   Col449, Col450, Col451, Col452, Col453, Col454, Col455, Col456, Col457, Col458, Col459, Col460, Col461, Col462, Col463, Col464,
                   Col465, Col466, Col467, Col468, Col469, Col470, Col471, Col472, Col473, Col474, Col475, Col476, Col477, Col478, Col479, Col480,
                   Col481, Col482, Col483, Col484, Col485, Col486, Col487, Col488, Col489, Col490, Col491, Col492, Col493, Col494, Col495, Col496,
                   Col497, Col498, Col499, Col500, Col501, Col502, Col503, Col504, Col505, Col506, Col507, Col508, Col509, Col510, Col511, Col512,
                   Col513, Col514, Col515, Col516, Col517, Col518, Col519, Col520, Col521, Col522, Col523, Col524, Col525, Col526, Col527, Col528,
                   Col529, Col530, Col531, Col532, Col533, Col534, Col535, Col536, Col537, Col538, Col539, Col540, Col541, Col542, Col543, Col544,
                   Col545, Col546, Col547, Col548, Col549, Jan1, Feb1, Mar1, Apr1, May1, Jun1, Jul1, Aug1, Sep1, Oct1, Nov1, Dec1, Jan2, Feb2, Mar2, Apr2, May2, Jun2

                    );





            }

            _dbManUpdate.SqlStatement = _dbManUpdate.SqlStatement + "  ";
            _dbManUpdate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManUpdate.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManUpdate.ExecuteInstruction();

            DataSet ds = new DataSet();

            if (ds.Tables.Count > 0)
                ds.Tables.Clear();

            ds.Tables.Add(table);

            Grd18mnth.DataSource = ds.Tables[0];



            DataTable TableSum = new DataTable();
            TableSum.Columns.Add("MachineNo", typeof(string));

            TableSum.Columns.Add("Jan1", typeof(float));
            TableSum.Columns.Add("Feb1", typeof(float));
            TableSum.Columns.Add("Mar1", typeof(float));
            TableSum.Columns.Add("Apr1", typeof(float));
            TableSum.Columns.Add("May1", typeof(float));
            TableSum.Columns.Add("Jun1", typeof(float));
            TableSum.Columns.Add("Jul1", typeof(float));
            TableSum.Columns.Add("Aug1", typeof(float));
            TableSum.Columns.Add("Sep1", typeof(float));
            TableSum.Columns.Add("Oct1", typeof(float));
            TableSum.Columns.Add("Nov1", typeof(float));
            TableSum.Columns.Add("Dec1", typeof(float));

            TableSum.Columns.Add("Jan2", typeof(float));
            TableSum.Columns.Add("Feb2", typeof(float));
            TableSum.Columns.Add("Mar2", typeof(float));
            TableSum.Columns.Add("Apr2", typeof(float));
            TableSum.Columns.Add("May2", typeof(float));
            TableSum.Columns.Add("Jun2", typeof(float));


            string mach = "";
            string mach1 = "";

            decimal jan = 0;
            decimal feb = 0;
            decimal mar = 0;
            decimal apr = 0;
            decimal may = 0;
            decimal jun = 0;
            decimal jul = 0;
            decimal aug = 0;
            decimal sep = 0;
            decimal oct = 0;
            decimal nov = 0;
            decimal dec = 0;

            decimal jan2 = 0;
            decimal feb2 = 0;
            decimal mar2 = 0;
            decimal apr2 = 0;
            decimal may2 = 0;
            decimal jun2 = 0;

            for (int x = 0; x < table.Rows.Count; x++)
            {
                if (table.Rows[x]["MachineNo"].ToString() != "")
                {
                    if (x > 0)
                    {
                        if (mach != table.Rows[x]["MachineNo"].ToString())
                        {
                            TableSum.Rows.Add(mach, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec, jan2, feb2, mar2, apr2, may2, jun2);

                            jan = 0;
                            feb = 0;
                            mar = 0;
                            apr = 0;
                            may = 0;
                            jun = 0;
                            jul = 0;
                            aug = 0;
                            sep = 0;
                            oct = 0;
                            nov = 0;
                            dec = 0;

                            jan2 = 0;
                            feb2 = 0;
                            mar2 = 0;
                            apr2 = 0;
                            may2 = 0;
                            jun2 = 0;

                        }
                    }

                    mach = table.Rows[x]["MachineNo"].ToString();
                    jan = jan + Convert.ToDecimal(table.Rows[x]["Col550"].ToString());
                    feb = feb + Convert.ToDecimal(table.Rows[x]["Col551"].ToString());
                    mar = mar + Convert.ToDecimal(table.Rows[x]["Col552"].ToString());
                    apr = apr + Convert.ToDecimal(table.Rows[x]["Col553"].ToString());
                    may = may + Convert.ToDecimal(table.Rows[x]["Col554"].ToString());
                    jun = jun + Convert.ToDecimal(table.Rows[x]["Col555"].ToString());
                    jul = jul + Convert.ToDecimal(table.Rows[x]["Col556"].ToString());
                    aug = aug + Convert.ToDecimal(table.Rows[x]["Col557"].ToString());
                    sep = sep + Convert.ToDecimal(table.Rows[x]["Col558"].ToString());
                    oct = oct + Convert.ToDecimal(table.Rows[x]["Col559"].ToString());
                    nov = nov + Convert.ToDecimal(table.Rows[x]["Col560"].ToString());
                    dec = dec + Convert.ToDecimal(table.Rows[x]["Col561"].ToString());

                    jan2 = jan2 + Convert.ToDecimal(table.Rows[x]["Col562"].ToString());
                    feb2 = feb2 + Convert.ToDecimal(table.Rows[x]["Col563"].ToString());
                    mar2 = mar2 + Convert.ToDecimal(table.Rows[x]["Col564"].ToString());
                    apr2 = apr2 + Convert.ToDecimal(table.Rows[x]["Col565"].ToString());
                    may2 = may2 + Convert.ToDecimal(table.Rows[x]["Col566"].ToString());
                    jun2 = jun2 + Convert.ToDecimal(table.Rows[x]["Col567"].ToString());

                }

            }

            Grdmnth.Visible = true;
            Grd18mnth.Visible = true;


            TableSum.Rows.Add(mach, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec, jan2, feb2, mar2, apr2, may2, jun2);

            DataSet ds1 = new DataSet();

            ds1.Tables.Add(TableSum);

            if (ds1.Tables.Count > 0)
                ds1.Tables.Clear();

            ds1.Tables.Add(TableSum);




            Grdmnth.DataSource = ds1.Tables[0];


            bandedGridView2.OptionsView.ShowFooter = true;
            bandedGridView2.Columns[1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;



            ColMch.FieldName = "MachineNo";
            ColS11.FieldName = "Jan1";
            ColS12.FieldName = "Feb1";
            ColS13.FieldName = "Mar1";
            ColS14.FieldName = "Apr1";
            ColS15.FieldName = "May1";
            ColS16.FieldName = "Jun1";
            ColS17.FieldName = "Jul1";
            ColS18.FieldName = "Aug1";
            ColS19.FieldName = "Sep1";
            ColS20.FieldName = "Oct1";
            ColS21.FieldName = "Nov1";
            ColS22.FieldName = "Dec1";

            ColS23.FieldName = "Jan2";
            ColS24.FieldName = "Feb2";
            ColS25.FieldName = "Mar2";
            ColS26.FieldName = "Apr2";
            ColS27.FieldName = "May2";
            ColS28.FieldName = "Jun2";


            bandedGridView2.OptionsView.ShowFooter = true;
            bandedGridView2.Columns[1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            Grdmnth.RefreshDataSource();



            Grd18mnth.Visible = true;


            //PnlMachine.Visible = false;

            tablesum1 = table;

            gridagain();
        }

        void gridagain()
        {
            DataTable TableSum = new DataTable();
            TableSum.Columns.Add("MachineNo", typeof(string));

            TableSum.Columns.Add("Jan1", typeof(float));
            TableSum.Columns.Add("Feb1", typeof(float));
            TableSum.Columns.Add("Mar1", typeof(float));
            TableSum.Columns.Add("Apr1", typeof(float));
            TableSum.Columns.Add("May1", typeof(float));
            TableSum.Columns.Add("Jun1", typeof(float));
            TableSum.Columns.Add("Jul1", typeof(float));
            TableSum.Columns.Add("Aug1", typeof(float));
            TableSum.Columns.Add("Sep1", typeof(float));
            TableSum.Columns.Add("Oct1", typeof(float));
            TableSum.Columns.Add("Nov1", typeof(float));
            TableSum.Columns.Add("Dec1", typeof(float));

            TableSum.Columns.Add("Jan2", typeof(float));
            TableSum.Columns.Add("Feb2", typeof(float));
            TableSum.Columns.Add("Mar2", typeof(float));
            TableSum.Columns.Add("Apr2", typeof(float));
            TableSum.Columns.Add("May2", typeof(float));
            TableSum.Columns.Add("Jun2", typeof(float));

            string mach = "";
            string mach1 = "";

            decimal jan = 0;
            decimal feb = 0;
            decimal mar = 0;
            decimal apr = 0;
            decimal may = 0;
            decimal jun = 0;
            decimal jul = 0;
            decimal aug = 0;
            decimal sep = 0;
            decimal oct = 0;
            decimal nov = 0;
            decimal dec = 0;

            decimal jan2 = 0;
            decimal feb2 = 0;
            decimal mar2 = 0;
            decimal apr2 = 0;
            decimal may2 = 0;
            decimal jun2 = 0;



            for (int x = 0; x < tablesum1.Rows.Count; x++)
            {
                if (tablesum1.Rows[x]["MachineNo"].ToString() != "")
                {
                    if (x > 0)
                    {
                        if (mach != tablesum1.Rows[x]["MachineNo"].ToString())
                        {
                            if (mach != "z")
                                TableSum.Rows.Add(mach, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec, jan2, feb2, mar2, apr2, may2, jun2);

                            jan = 0;
                            feb = 0;
                            mar = 0;
                            apr = 0;
                            may = 0;
                            jun = 0;
                            jul = 0;
                            aug = 0;
                            sep = 0;
                            oct = 0;
                            nov = 0;
                            dec = 0;

                            jan2 = 0;
                            feb2 = 0;
                            mar2 = 0;
                            apr2 = 0;
                            may2 = 0;
                            jun2 = 0;

                        }
                    }

                    mach = tablesum1.Rows[x]["MachineNo"].ToString();
                    jan = jan + Convert.ToDecimal(tablesum1.Rows[x]["Col550"].ToString());
                    feb = feb + Convert.ToDecimal(tablesum1.Rows[x]["Col551"].ToString());
                    mar = mar + Convert.ToDecimal(tablesum1.Rows[x]["Col552"].ToString());
                    apr = apr + Convert.ToDecimal(tablesum1.Rows[x]["Col553"].ToString());
                    may = may + Convert.ToDecimal(tablesum1.Rows[x]["Col554"].ToString());
                    jun = jun + Convert.ToDecimal(tablesum1.Rows[x]["Col555"].ToString());
                    jul = jul + Convert.ToDecimal(tablesum1.Rows[x]["Col556"].ToString());
                    aug = aug + Convert.ToDecimal(tablesum1.Rows[x]["Col557"].ToString());
                    sep = sep + Convert.ToDecimal(tablesum1.Rows[x]["Col558"].ToString());
                    oct = oct + Convert.ToDecimal(tablesum1.Rows[x]["Col559"].ToString());
                    nov = nov + Convert.ToDecimal(tablesum1.Rows[x]["Col560"].ToString());
                    dec = dec + Convert.ToDecimal(tablesum1.Rows[x]["Col561"].ToString());

                    jan2 = jan2 + Convert.ToDecimal(tablesum1.Rows[x]["Col562"].ToString());
                    feb2 = feb2 + Convert.ToDecimal(tablesum1.Rows[x]["Col563"].ToString());
                    mar2 = mar2 + Convert.ToDecimal(tablesum1.Rows[x]["Col564"].ToString());
                    apr2 = apr2 + Convert.ToDecimal(tablesum1.Rows[x]["Col565"].ToString());
                    may2 = may2 + Convert.ToDecimal(tablesum1.Rows[x]["Col566"].ToString());
                    jun2 = jun2 + Convert.ToDecimal(tablesum1.Rows[x]["Col567"].ToString());


                }

            }
            if (mach != "z")
                TableSum.Rows.Add(mach, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec, jan2, feb2, mar2, apr2, may2, jun2);

            DataSet ds1 = new DataSet();

            ds1.Tables.Add(TableSum);

            if (ds1.Tables.Count > 0)
                ds1.Tables.Clear();

            ds1.Tables.Add(TableSum);

            Grdmnth.DataSource = ds1.Tables[0];

            bandedGridView2.OptionsView.ShowFooter = true;
            bandedGridView2.Columns[1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            ColMch.FieldName = "MachineNo";
            ColS11.FieldName = "Jan1";
            ColS12.FieldName = "Feb1";
            ColS13.FieldName = "Mar1";
            ColS14.FieldName = "Apr1";
            ColS15.FieldName = "May1";
            ColS16.FieldName = "Jun1";
            ColS17.FieldName = "Jul1";
            ColS18.FieldName = "Aug1";
            ColS19.FieldName = "Sep1";
            ColS20.FieldName = "Oct1";
            ColS21.FieldName = "Nov1";
            ColS22.FieldName = "Dec1";

            ColS23.FieldName = "Jan2";
            ColS24.FieldName = "Feb2";
            ColS25.FieldName = "Mar2";
            ColS26.FieldName = "Apr2";
            ColS27.FieldName = "May2";
            ColS28.FieldName = "Jun2";

            bandedGridView2.OptionsView.ShowFooter = true;
            bandedGridView2.Columns[1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            Grdmnth.RefreshDataSource();

            Grd18mnth.Visible = true;


        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            newPnl.Visible = false;
            PnlMachine.Visible = true;
            PnlMachine.Dock = DockStyle.Fill;


            SEditFinYear.Visible = false;
        }

        private void LstUnMachines_Click(object sender, EventArgs e)
        {
            int selectedIndex = LstUnMachines.SelectedIndex;

            Object selectedItem = LstUnMachines.SelectedItem;

            LblChg2.Text = LstUnMachines.SelectedValue.ToString();
        }

        private void LblChg2_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = true;
            TxtMPerShift.Visible = true;
            chBNightShift.Visible = true;

            //label4.Visible = true;
            // GridCycle.Visible = true;
            // LstCycleCode.Visible = true;
            //BtnSaveMachine.Visible = true;
            // label5.Visible = true;
            // label6.Visible = true;
            // label7.Visible = true;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "select * from tbl_GeoScience_Machine where machineno = '" + LblChg2.Text + "'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "order by machineno ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable MMAc = _dbMan.ResultsDataTable;

            if (MMAc.Rows.Count > 0)
            {
                TxtMPerShift.Text = MMAc.Rows[0]["advpershift"].ToString();
                if (MMAc.Rows[0]["ns"].ToString() == "N")
                    chBNightShift.Checked = false;
                else
                    chBNightShift.Checked = true;

            }
        }

        private void BtnSaveMachine_Click(object sender, EventArgs e)
        {
            
        }

        private void LstCycleCode_MouseDown(object sender, MouseEventArgs e)
        {
            if (LstCycleCode.Items.Count == 0)
                return;

            int index = LstCycleCode.IndexFromPoint(e.X, e.Y);
            if (index > -1)
            {
                string s = LstCycleCode.Items[index].ToString();
                lblSelected.Text = proc.ExtractBeforeColon(LstCycleCode.Text);
                DragDropEffects dde1 = DoDragDrop(s,
                    DragDropEffects.All);
            }
        }

        private void GridCycle_Click(object sender, EventArgs e)
        {
           
        }

        private void GridCycle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCycle.ReadOnly = true;
            if (e.ColumnIndex == 1 && GridCycle.Rows[e.RowIndex].Cells[0].Value.ToString() != "")
                GridCycle.ReadOnly = false;

            if (lblSelected.Text != "None")
            {
                if (e.ColumnIndex > 0)
                {
                    if (e.RowIndex >= 0)
                    {

                        GridCycle.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = lblSelected.Text;



                    }
                }

            }

            GridCycle.ReadOnly = true;
        }

        private void LstPMachines_Click(object sender, EventArgs e)
        {
            int selectedIndex = LstPMachines.SelectedIndex;

            Object selectedItem = LstPMachines.SelectedItem;

            LblChg2.Text = LstPMachines.SelectedValue.ToString();
        }



        void LoadUnassigned()
        {
           
                MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
                _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan1.SqlStatement = "select substring(machineno + '                                   ',1,15) + substring(workplace + '                                           ',1,40)+  substring(holeno + '                                       ',1,30) aa " +
                                        ", * from [dbo].[tbl_GeoScience_PlanLongTerm] where machineno like '" + FilterTxt.Text + "%' and [Nodeid] not in (select Nodeid from [tbl_GeoScience_DeletedHoles]) " +
                                        "and sdate is null order by aa ";
                _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan1.ExecuteInstruction();

                DataTable HolesNA1 = _dbMan1.ResultsDataTable;

                UnProcList.Items.Clear();

                foreach (DataRow row in HolesNA1.Rows)
                {
                    UnProcList.Items.Add(row["aa"].ToString());
                }

                FilterHole = "All";

          

        }

        private void FilterTxt_TextChanged(object sender, EventArgs e)
        {
           // LoadUnassigned();
        }

        
        string Mach = "";
        string wp = "";
        string hole = "";
        string ss = "";
        string clickcol = "0";

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void FilterTxt_TextChanged_1(object sender, EventArgs e)
        {
            LoadUnassigned();
        }

        private void bandedGridView1_RowCellClick_1(object sender, RowCellClickEventArgs e)
        {
          
        }

        private void Grd18mnth_DoubleClick_1(object sender, EventArgs e)
        {
            
        }

       

        private void bandedGridView1_CustomDrawCell_1(object sender, RowCellCustomDrawEventArgs e)
        {
          
        }

        private void bandedGridView1_RowCellClick_2(object sender, RowCellClickEventArgs e)
        {
           
        }

        private void Grd18mnth_DoubleClick(object sender, EventArgs e)
        {
          
        }
        string aa = "Y";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (  aa == "Y" )
            {
                navBarControl1.Width = 0;
                 aa = "N";
            }
            else
            {
                aa = "Y";
                navBarControl1.Width = 185;
            }

        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Convert.ToDecimal(TxtMPerShift.Text) < Convert.ToDecimal(0.0001))
            {
                MessageBox.Show("A metre per Day has to be captured", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ns = "N";
            if (chBNightShift.Checked == true)
                ns = "Y";

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " delete from tbl_GeoScience_Machine where machineno = '" + LblChg2.Text + "' insert into tbl_GeoScience_Machine values ('" + LblChg2.Text + "' " +
                                  " , '" + TxtMPerShift.Text + "' " +
                                  ", '', '" + ns + "' ) ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Info", "Data saved successfuly.", Color.CornflowerBlue);

            LoadMachines();
        }

        private void bandedGridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {



          
        }

        private void UnProcList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //frmGeologyProp frm = new frmGeologyProp();
            //frm._theSystemDBTag = theSystemDBTag;
            //frm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            //frm.Machlabel.Text = UnProcList.Text.Substring(0, 10).Trim();
            //frm.WPlabel.Text = UnProcList.Text.Substring(11, 39).Trim();
            //frm.Holelabel.Text = UnProcList.Text.Substring(50, 25).Trim();
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.ShowDialog(this);
            navBarItem3_LinkClicked(null, null);
        }




        private void bandedGridView1_CustomDrawCell_2(object sender, RowCellCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            Graphics g = e.Graphics;

            Brush hb = Brushes.LightGray;

            GridView View = sender as GridView;

            //for (int i = 8; i < 8; i++)
            //{
            //    if (e.Column == View.Columns[i])
            //    {
            //        if (!View.GetRowCellValue(e.RowHandle, e.Column).Equals(DBNull.Value))
            //            if (View.GetRowCellValue(e.RowHandle, e.Column) == "O")
            //                e.Appearance.BackColor = Color.Red;
            //    }
            //}

            string ss = "";
            if (e.Column == View.Columns[5])
            {
                if (!View.GetRowCellValue(e.RowHandle, e.Column).Equals(DBNull.Value))
                {
                    ss = View.GetRowCellValue(e.RowHandle, e.Column).ToString().Length.ToString();
                    if ((View.GetRowCellValue(e.RowHandle, e.Column).ToString()).Length == 13)
                    {
                        e.Graphics.DrawImage(pictureBox3.Image, e.Bounds.X - 150, e.Bounds.Y, 18, 18);
                        e.Handled = false;
                    }
                }


                //e.Appearance.BackColor = Color.Red;
            }




            for (int i = 8; i < 459; i++)
            {
                if (e.Column == View.Columns[i])
                {
                    e.Appearance.ForeColor = Color.Transparent;

                }
            }


            if ((e.Column == View.Columns[6]) && (e.RowHandle == bandedGridView1.RowCount - 1))
            {
                e.Appearance.ForeColor = Color.Transparent;
            }


            for (int i = 8; i < 556; i++)
            {
                if (e.Column == View.Columns[i] && e.RowHandle < bandedGridView1.RowCount - 1)
                {
                    if (!View.GetRowCellValue(e.RowHandle, e.Column).Equals(DBNull.Value))
                    {
                        if (View.GetRowCellValue(e.RowHandle, e.Column) == "O")
                        {
                            e.Appearance.BackColor = Color.Gainsboro;
                            e.Appearance.ForeColor = Color.Gainsboro;

                        }

                        if (View.GetRowCellValue(e.RowHandle, e.Column) == "A")
                        {
                            e.Appearance.BackColor = Color.FromArgb(120, 165, 233);
                            e.Appearance.ForeColor = Color.FromArgb(120, 165, 233);

                        }

                        if (View.GetRowCellValue(e.RowHandle, e.Column) == "B")
                        {
                            e.Appearance.BackColor = Color.FromArgb(255, 203, 106);
                            e.Appearance.ForeColor = Color.FromArgb(255, 203, 106);

                        }


                        if (View.GetRowCellValue(e.RowHandle, e.Column) == "C")
                        {
                            e.Appearance.BackColor = Color.FromArgb(223, 0, 0);
                            e.Appearance.ForeColor = Color.FromArgb(223, 0, 0);

                        }


                    }
                }
            }


            if (e.Column.Name == "Yr1Jan1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Feb1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Mar1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Apr1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1May1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Jun1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Jul1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Aug1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Sep1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Oct1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Nov1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr1Dec1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }

            if (e.Column.Name == "Yr2Jan1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr2Feb1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr2Mar1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr2Apr1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr2May1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }
            if (e.Column.Name == "Yr2Jun1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }

            if (e.Column.Name == "ColMach" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height * 4)); // left
            }

            if (e.Column.Name == "ColWp" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                //g.FillRectangle(hb, new Rectangle(r.X - 2, 0, 1, r.Height * 4)); // left
                // g.FillRectangle(hb, new Rectangle(r.X - 5, r.Y - 1, 1, r.Height + 4)); 

            }

            if (e.Column.Name == "ColHole" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
                //g.FillRectangle(hb, new Rectangle(r.X - 5, r.Y - 1, 1, r.Height + 4));
            }

            if (e.Column.Name == "ColLen" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }


            if (e.Column.Name == "Line1" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }

            if (e.Column.Name == "ColDep" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }

            if (e.Column.Name == "ColSdate" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }

            if (e.Column.Name == "ColDuration" && e.RowHandle < bandedGridView1.RowCount - 1)
            {
                g.FillRectangle(hb, new Rectangle(r.X - 2, r.Y - 1, 1, r.Height + 4)); // left
            }



            if (e.Column.Name != "Line1" && r.Y > 50)
            {
                // g.FillRectangle(hb, new Rectangle(r.X - 5, r.Y - 6, r.Width + 5, 5));

            }
        }

        private void bandedGridView1_RowCellStyle_1(object sender, RowCellStyleEventArgs e)
        {
            Brush hb = Brushes.LightGray;

            GridView View = sender as GridView;

            for (int i = 8; i < 459; i++)
            {
                if (e.Column == View.Columns[i])
                {
                    e.Appearance.ForeColor = Color.Transparent;

                }
            }


            if ((e.Column == View.Columns[6]) && (e.RowHandle == bandedGridView1.RowCount - 1))
            {
                e.Appearance.ForeColor = Color.Transparent;
            }


            for (int i = 8; i < 556; i++)
            {
                if (e.Column == View.Columns[i] && e.RowHandle < bandedGridView1.RowCount - 1)
                {
                    if (!View.GetRowCellValue(e.RowHandle, e.Column).Equals(DBNull.Value))
                    {
                        if (View.GetRowCellValue(e.RowHandle, e.Column) == "O")
                        {
                            e.Appearance.BackColor = Color.Gainsboro;
                            e.Appearance.ForeColor = Color.Gainsboro;

                        }

                        if (View.GetRowCellValue(e.RowHandle, e.Column) == "A")
                        {
                            e.Appearance.BackColor = Color.FromArgb(120, 165, 233);
                            e.Appearance.ForeColor = Color.FromArgb(120, 165, 233);

                        }

                        if (View.GetRowCellValue(e.RowHandle, e.Column) == "B")
                        {
                            e.Appearance.BackColor = Color.FromArgb(255, 203, 106);
                            e.Appearance.ForeColor = Color.FromArgb(255, 203, 106);

                        }


                        if (View.GetRowCellValue(e.RowHandle, e.Column) == "C")
                        {
                            e.Appearance.BackColor = Color.FromArgb(223, 0, 0);
                            e.Appearance.ForeColor = Color.FromArgb(223, 0, 0);

                        }


                    }
                }
            }
        }

        private void bandedGridView1_RowClick(object sender, RowClickEventArgs e)
        {
          
        }

        private void Grd18mnth_DoubleClick_2(object sender, EventArgs e)
        {
            //if (clickcol == "ColSdate")
            //{

            //    frmGeologyProp frm = new frmGeologyProp();
            //    frm._theSystemDBTag = theSystemDBTag;
            //    frm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            //    frm.Machlabel.Text = Mach;
            //    frm.WPlabel.Text = wp;
            //    frm.Holelabel.Text = hole;
            //    frm.StartPosition = FormStartPosition.CenterScreen;
            //    frm.ShowDialog(this);
            //    navBarItem3_LinkClicked(null, null);

            //}
            //else
            //{

            //    DiamondDrillfrm frm = new DiamondDrillfrm();
            //    frm._theSystemDBTag = theSystemDBTag;
            //    frm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            //    frm.WPtextBox.Text = wp;
            //    frm.machtextBox.Text = Mach;
            //    frm.BoreholeIDtxt.Text = hole;
            //    if (ss.Length == 13)
            //    {
            //        frm.Editlabel.Text = "Y";
            //    }
            //    frm.StartPosition = FormStartPosition.CenterScreen;
            //    frm.ShowDialog(this);
            //    navBarItem3_LinkClicked(null, null);

            //    //DiamondDrillfrm DiaFrm = (DiamondDrillfrm)IsDiamondAlreadyOpen(typeof(DiamondDrillfrm));
            //    //if (DiaFrm == null)
            //    //{
            //    //    DiaFrm = new DiamondDrillfrm();
            //    //    DiaFrm.WPtextBox.Text = wp;
            //    //    DiaFrm.machtextBox.Text = Mach;
            //    //    DiaFrm.BoreholeIDtxt.Text = hole;
            //    //    DiaFrm.NDIDTxt.Text = "";



            //    //    if (ss.Length == 13)
            //    //        DiaFrm.Editlabel.Text = "Y";

            //    //    // RepFrm.Text = "Crew Ranking Report";
            //    //    DiaFrm.Show();
            //    //}
            //    //else
            //    //{
            //    //    DiaFrm.WindowState = FormWindowState.Maximized;
            //    //    DiaFrm.Select();
            //    //}
            //}
        }

        private void bandedGridView1_RowCellClick_3(object sender, RowCellClickEventArgs e)
        {
            Mach = bandedGridView1.GetRowCellValue(e.RowHandle, bandedGridView1.Columns[0]).ToString();
            wp = bandedGridView1.GetRowCellValue(e.RowHandle, bandedGridView1.Columns[1]).ToString();
            hole = bandedGridView1.GetRowCellValue(e.RowHandle, bandedGridView1.Columns[2]).ToString();
            ss = bandedGridView1.GetRowCellValue(e.RowHandle, bandedGridView1.Columns[5]).ToString();
            clickcol = e.Column.ToString();
            clickcol = e.Column.ToString();
        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            PnlMachine.Visible = false;
            LoadUnassigned();
            newPnl.Visible = false;
            Pnl3Month.Visible = true;
            Pnl3Month.Dock = DockStyle.Fill;            

            button2_Click_1(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void bandedGridView3_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {


           
        }

        string wp3mnth = "";
        string wp3mnthBH = "";
        string clickcol3Mnth = "";

        private void bandedGridView3_RowCellClick(object sender, RowCellClickEventArgs e)
        {


            string cellValue = lblSelected.Text;

            clickcol3Mnth = e.Column.ToString();

            if (cellValue != "None")
                if (bandedGridView3.GetRowCellValue(e.RowHandle, bandedGridView3.FocusedColumn).ToString() != "OFF")
                    bandedGridView3.SetRowCellValue(e.RowHandle, bandedGridView3.FocusedColumn, cellValue);

            wp3mnth = bandedGridView3.GetRowCellValue(e.RowHandle, bandedGridView3.Columns[1]).ToString();
            wp3mnthBH = bandedGridView3.GetRowCellValue(e.RowHandle, bandedGridView3.Columns[2]).ToString();
        }

        private void Grd3Mnth_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }







        private void button2_Click_1(object sender, EventArgs e)
        {
            Procedures procs = new Procedures();
            // get next mnth            
            String PMonth = ProdMonthTxt.Text;
            String PMonthOrig = ProdMonthTxt.Text;
            Decimal year = Convert.ToDecimal(PMonth.Substring(0, 4));
            Decimal month = Convert.ToDecimal(PMonth.Substring(4, 2));
            String NewMonth = "";
            Decimal NewProdMonth = 0;

            PMonth.Substring(4, 2);



            if (month == 12)
            {
                year = year + 1;
                month = 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm2lbl.Text = year.ToString() + NewMonth;

            }
            else
            {
                year = year;
                month = month + 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm2lbl.Text = year.ToString() + NewMonth;

            }

            procs.ProdMonthVis(Convert.ToInt32(pm2lbl.Text));
            pm2lbl2.Text = Procedures.Prod2;



            PMonth = pm2lbl.Text;
            year = Convert.ToDecimal(PMonth.Substring(0, 4));
            month = Convert.ToDecimal(PMonth.Substring(4, 2));
            NewMonth = "";
            NewProdMonth = 0;

            PMonth.Substring(4, 2);

            if (month == 12)
            {
                year = year + 1;
                month = 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm3lbl.Text = year.ToString() + NewMonth;

            }
            else
            {
                year = year;
                month = month + 1;
                if (month < 10)
                    NewMonth = "0" + month.ToString();
                else
                    NewMonth = month.ToString();

                pm3lbl.Text = year.ToString() + NewMonth;

            }

            procs.ProdMonthVis(Convert.ToInt32(pm3lbl.Text));
            pm3lbl2.Text = Procedures.Prod2;

            int daymnth1 = 0;
            int daymnth2 = 0;
            int daymnth3 = 0;

            dateTimePicker1.Value = new DateTime(Convert.ToInt32(PMonthOrig.Substring(0, 4)), Convert.ToInt32(PMonthOrig.Substring(4, 2)), 01);
            int day1 = 35;

            int day2 = 0;
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(day1);

            day2 = dateTimePicker1.Value.Day;
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(day2 * -1);

            daymnth1 = dateTimePicker1.Value.Day;


            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(day1);

            day2 = dateTimePicker1.Value.Day;
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(day2 * -1);

            daymnth2 = dateTimePicker1.Value.Day;

            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(day1);

            day2 = dateTimePicker1.Value.Day;
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(day2 * -1);

            daymnth3 = dateTimePicker1.Value.Day;






            MWDataManager.clsDataAccess _dbMan3Mnth = new MWDataManager.clsDataAccess();
            _dbMan3Mnth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
 
            _dbMan3Mnth.SqlStatement = " exec sp_GeoScience3MnthPlan1 '" + ProdMonthTxt.Text + "','" + pm2lbl.Text + "','" + pm3lbl.Text + "'  ";

            _dbMan3Mnth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3Mnth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3Mnth.ExecuteInstruction();

            DataTable dt = _dbMan3Mnth.ResultsDataTable;

            DataSet ds = new DataSet();

            if (ds.Tables.Count > 0)
                ds.Tables.Clear();

            ds.Tables.Add(dt);
            Grd3Mnth.Visible = true;

            Grd3Mnth.DataSource = ds.Tables[0];


            mnth1.Caption = ProdMonth1Txt.Text;
            mnth2.Caption = pm2lbl2.Text;
            mnth3.Caption = pm3lbl2.Text;

            MnthMach.FieldName = "MachineNo";
            MnthWp.FieldName = "workplace";
            MnthHole.FieldName = "holeno";
            MnthShift.FieldName = "ShiftLbl";
            MnthSDate.FieldName = "sdate";

            Mnth1d1.FieldName = "M1D1";
            Mnth1d2.FieldName = "M1D2";
            Mnth1d3.FieldName = "M1D3";
            Mnth1d4.FieldName = "M1D4";
            Mnth1d5.FieldName = "M1D5";

            Mnth1d6.FieldName = "M1D6";
            Mnth1d7.FieldName = "M1D7";
            Mnth1d8.FieldName = "M1D8";
            Mnth1d9.FieldName = "M1D9";
            Mnth1d10.FieldName = "M1D10";
            Mnth1d11.FieldName = "M1D11";
            Mnth1d12.FieldName = "M1D12";
            Mnth1d13.FieldName = "M1D13";
            Mnth1d14.FieldName = "M1D14";
            Mnth1d15.FieldName = "M1D15";
            Mnth1d16.FieldName = "M1D16";
            Mnth1d17.FieldName = "M1D17";
            Mnth1d18.FieldName = "M1D18";
            Mnth1d19.FieldName = "M1D19";
            Mnth1d20.FieldName = "M1D20";
            Mnth1d21.FieldName = "M1D21";
            Mnth1d22.FieldName = "M1D22";
            Mnth1d23.FieldName = "M1D23";
            Mnth1d24.FieldName = "M1D24";
            Mnth1d25.FieldName = "M1D25";
            Mnth1d26.FieldName = "M1D26";
            Mnth1d27.FieldName = "M1D27";
            Mnth1d28.FieldName = "M1D28";
            Mnth1d29.FieldName = "M1D29";
            Mnth1d30.FieldName = "M1D30";
            Mnth1d31.FieldName = "M1D31";

            Mnth2d1.FieldName = "M2D1";
            Mnth2d2.FieldName = "M2D2";
            Mnth2d3.FieldName = "M2D3";
            Mnth2d4.FieldName = "M2D4";
            Mnth2d5.FieldName = "M2D5";
            Mnth2d6.FieldName = "M2D6";
            Mnth2d7.FieldName = "M2D7";
            Mnth2d8.FieldName = "M2D8";
            Mnth2d9.FieldName = "M2D9";
            Mnth2d10.FieldName = "M2D10";
            Mnth2d11.FieldName = "M2D11";
            Mnth2d12.FieldName = "M2D12";
            Mnth2d13.FieldName = "M2D13";
            Mnth2d14.FieldName = "M2D14";
            Mnth2d15.FieldName = "M2D15";
            Mnth2d16.FieldName = "M2D16";
            Mnth2d17.FieldName = "M2D17";
            Mnth2d18.FieldName = "M2D18";
            Mnth2d19.FieldName = "M2D19";
            Mnth2d20.FieldName = "M2D20";
            Mnth2d21.FieldName = "M2D21";
            Mnth2d22.FieldName = "M2D22";
            Mnth2d23.FieldName = "M2D23";
            Mnth2d24.FieldName = "M2D24";
            Mnth2d25.FieldName = "M2D25";
            Mnth2d26.FieldName = "M2D26";
            Mnth2d27.FieldName = "M2D27";
            Mnth2d28.FieldName = "M2D28";
            Mnth2d29.FieldName = "M2D29";
            Mnth2d30.FieldName = "M2D30";
            Mnth2d31.FieldName = "M2D31";

            Mnth3d1.FieldName = "M3D1";
            Mnth3d2.FieldName = "M3D2";
            Mnth3d3.FieldName = "M3D3";
            Mnth3d4.FieldName = "M3D4";
            Mnth3d5.FieldName = "M3D5";
            Mnth3d6.FieldName = "M3D6";
            Mnth3d7.FieldName = "M3D7";
            Mnth3d8.FieldName = "M3D8";
            Mnth3d9.FieldName = "M3D9";
            Mnth3d10.FieldName = "M3D10";
            Mnth3d11.FieldName = "M3D11";
            Mnth3d12.FieldName = "M3D12";
            Mnth3d13.FieldName = "M3D13";
            Mnth3d14.FieldName = "M3D14";
            Mnth3d15.FieldName = "M3D15";
            Mnth3d16.FieldName = "M3D16";
            Mnth3d17.FieldName = "M3D17";
            Mnth3d18.FieldName = "M3D18";
            Mnth3d19.FieldName = "M3D19";
            Mnth3d20.FieldName = "M3D20";
            Mnth3d21.FieldName = "M3D21";
            Mnth3d22.FieldName = "M3D22";
            Mnth3d23.FieldName = "M3D23";
            Mnth3d24.FieldName = "M3D24";
            Mnth3d25.FieldName = "M3D25";
            Mnth3d26.FieldName = "M3D26";
            Mnth3d27.FieldName = "M3D27";
            Mnth3d28.FieldName = "M3D28";
            Mnth3d29.FieldName = "M3D29";
            Mnth3d30.FieldName = "M3D30";
            Mnth3d31.FieldName = "M3D31";

            if (daymnth1 == 28)
            {
                Mnth1d29.Visible = false;
                Mnth1d30.Visible = false;
                Mnth1d31.Visible = false;
                gridBand102.Visible = false;
                gridBand103.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth1 == 29)
            {
                Mnth1d30.Visible = false;
                Mnth1d31.Visible = false;
                gridBand103.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth1 == 30)
            {
                Mnth1d31.Visible = false;
                gridBand104.Visible = false;
            }

            if (daymnth2 == 28)
            {
                Mnth2d29.Visible = false;
                Mnth2d30.Visible = false;
                Mnth2d31.Visible = false;
                gridBand133.Visible = false;
                gridBand124.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth2 == 29)
            {
                Mnth2d30.Visible = false;
                Mnth2d31.Visible = false;
                gridBand124.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth2 == 30)
            {
                Mnth2d31.Visible = false;
                gridBand135.Visible = false;
            }

            if (daymnth3 == 28)
            {
                Mnth3d29.Visible = false;
                Mnth3d30.Visible = false;
                Mnth3d31.Visible = false;
                gridBand165.Visible = false;
                gridBand166.Visible = false;
                gridBand167.Visible = false;
            }

            if (daymnth3 == 29)
            {
                Mnth3d30.Visible = false;
                Mnth3d31.Visible = false;
                gridBand166.Visible = false;
                gridBand167.Visible = false;
            }

            if (daymnth3 == 30)
            {
                Mnth3d31.Visible = false;
                gridBand167.Visible = false;
            }

        }

        private void bandedGridView3_CustomDrawCell_1(object sender, RowCellCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            Graphics g = e.Graphics;

            Brush hb = Brushes.LightGray;

            GridView View = sender as GridView;

            string ss = "";



            for (int i = 4; i < bandedGridView3.Columns.Count - 1; i++)
            {
                if (e.Column == View.Columns[i] && e.RowHandle < bandedGridView3.RowCount)
                {
                    if (!View.GetRowCellValue(e.RowHandle, e.Column).Equals(DBNull.Value))
                    {
                        e.Appearance.ForeColor = Color.Black;

                        ss = View.GetRowCellValue(e.RowHandle, e.Column).ToString();
                        if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() == "OFF")
                        {
                            e.Appearance.BackColor = Color.Gainsboro;
                            e.Appearance.ForeColor = Color.Gainsboro;

                        }


                        if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() == "NP")
                        {
                            e.Appearance.ForeColor = Color.White;
                        }


                    }

                }
            }
        }

        private void Grd3Mnth_DragEnter_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void bandedGridView3_RowCellClick_1(object sender, RowCellClickEventArgs e)
        {


            string cellValue = lblSelected.Text;

            clickcol3Mnth = e.Column.ToString();

            if (cellValue != "None")
                if (bandedGridView3.GetRowCellValue(e.RowHandle, bandedGridView3.FocusedColumn).ToString() != "OFF")
                    bandedGridView3.SetRowCellValue(e.RowHandle, bandedGridView3.FocusedColumn, cellValue);

            wp3mnth = bandedGridView3.GetRowCellValue(e.RowHandle, bandedGridView3.Columns[1]).ToString();
            wp3mnthBH = bandedGridView3.GetRowCellValue(e.RowHandle, bandedGridView3.Columns[2]).ToString();
        }

        private void lstCodes_MouseDown(object sender, MouseEventArgs e)
        {
            if (lstCodes.Items.Count == 0)
                return;

            int index = lstCodes.IndexFromPoint(e.X, e.Y);
            if (index > -1)
            {
                string s = lstCodes.Items[index].ToString();
                lblSelected.Text = proc.ExtractBeforeColon(lstCodes.Text);
                DragDropEffects dde1 = DoDragDrop(s,
                    DragDropEffects.All);
            }
        }

        private void ProdMonth1Txt_TextChanged(object sender, EventArgs e)
        {
            if (loaded == "Y")
                navBarItem6_LinkClicked(null, null);
        }

        private void ProdMonthTxt_Click(object sender, EventArgs e)
        {
            Procedures procs = new Procedures();
            procs.ProdMonthCalc(Convert.ToInt32(ProdMonthTxt.Text));
            ProdMonthTxt.Text = Procedures.Prod.ToString();
            procs.ProdMonthVis(Convert.ToInt32(ProdMonthTxt.Text));
            ProdMonth1Txt.Text = Procedures.Prod2;
        }

        private void WorkOrderBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //FrmNotesGeology frm = new FrmNotesGeology();
            //frm._theSystemDBTag = theSystemDBTag;
            //frm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            //frm.WPlabel.Text = wp3mnth + "                             ";
            //frm.BHLbl.Text = wp3mnthBH + "                             ";
            //frm.WPlabel2.Text = wp3mnth;
            //frm.BHLbl2.Text = wp3mnthBH;
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.ShowDialog(this);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {           

            //FrmNotesDrillPTO frm = new FrmNotesDrillPTO();
            //frm._theSystemDBTag = theSystemDBTag;
            //frm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            //frm.WPlabel.Text = wp3mnth + "                             ";
            //frm.BHLbl.Text = wp3mnthBH + "                             ";
            //frm.WPlabel2.Text = wp3mnth;
            //frm.BHLbl2.Text = wp3mnthBH;
            //frm.StartPosition = FormStartPosition.CenterScreen;
            //frm.ShowDialog(this);
        }

        private void ProbHistItem_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            GeoProbHistPnl.Dock = DockStyle.Fill;
            GeoProbHistPnl.Visible = true;
            LoadReportHistReport();
        }

        void LoadReportHistReport()
        {
            //PanelInvisible();
            GeoProbHistPnl.Dock = DockStyle.Fill;
            GeoProbHistPnl.Visible = true;


            DataSet dsProbHist1 = new DataSet();
            DataSet dsProbHist2 = new DataSet();
            DataSet dsProbHist3 = new DataSet();
            DataSet dsProbHist4 = new DataSet();
            DataSet dsProbHist5 = new DataSet();



            String mach = "";

            if (Radio2.SelectedIndex == 0)
                mach = "%";
            if (Radio2.SelectedIndex == 1)
                mach = "P%";
            if (Radio2.SelectedIndex == 2)
                mach = "H%";
            if (Radio2.SelectedIndex == 3)
                mach = "L%";


            String cost = "";

            if (radioGroup1.SelectedIndex == 0)
                cost = "%";
            if (radioGroup1.SelectedIndex == 1)
                cost = "OR%";
            if (radioGroup1.SelectedIndex == 2)
                cost = "IB%";
            if (radioGroup1.SelectedIndex == 3)
                cost = "C%";



            MWDataManager.clsDataAccess _dbManSamp2 = new MWDataManager.clsDataAccess();
            _dbManSamp2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSamp2.SqlStatement = " exec sp_GeoScienceProbHistGraph1 '" + mach + "', '" + cost + "' " +
                                            "  " +
                                            "   ";

            _dbManSamp2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSamp2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSamp2.ResultsTableName = "Table2";
            _dbManSamp2.ExecuteInstruction();

            DataTable dtPH1 = _dbManSamp2.ResultsDataTable;


            dsProbHist1.Tables.Add(dtPH1);

            ProbHistRep.RegisterData(dsProbHist1);



            MWDataManager.clsDataAccess _dbManSamp3 = new MWDataManager.clsDataAccess();
            _dbManSamp3.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSamp3.SqlStatement = " select '" + SysSettings.Banner + "' banner, '" + ProdMonthSamp1Txt.Text + "' mm " +
                                            "  " +
                                            "   ";

            _dbManSamp3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSamp3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSamp3.ResultsTableName = "Table3";
            _dbManSamp3.ExecuteInstruction();

            DataTable dtPH2 = _dbManSamp3.ResultsDataTable;


            dsProbHist2.Tables.Add(dtPH2);

            ProbHistRep.RegisterData(dsProbHist2);


            MWDataManager.clsDataAccess _dbManSamp4 = new MWDataManager.clsDataAccess();
            _dbManSamp4.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbManSamp4.SqlStatement = " select top(10)  *, case when ProbResp = 'OPERATIONAL' then 'SandyBrown' else 'SeaGreen' end as c  from  \r\n";
            if (Radio.SelectedIndex == 0)
                _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + "(select b.ProbResp, sum(probdur/60) dd from  [dbo].[tbl_GeoScience_ProbBook] a, \r\n";
            else
                _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + "(select b.ProbResp, count(ProblemID) dd from  [dbo].[tbl_GeoScience_ProbBook] a, \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " [dbo].[tbl_GeoScience_PlanLongTerm] c , [tbl_GeoScience_Problems] b \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + "where \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " a.hole = c.holeno  and c.costtype like '" + cost + "'  \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + "and a.problemid = b.probid \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + "and (year(thedate)*100)+month(thedate) = '" + ProdMonthSampTxt.Text + "'  \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " and hole in (  select distinct(holeno) a from [dbo].[tbl_GeoScience_PlanLongTerm]   \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " where machineno in (  select machineno from [dbo].[tbl_LongHoleDrilling_Machine]  where company like  '" + mach + "') )  group by b.ProbResp) a \r\n";


            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " union   select 'Drilling 5m', sum(aa)*9 aa, 'SeaGreen'  from   (select 1 aa  from  [tbl_GeoScienceDayPlanBook]  a, \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " [dbo].[tbl_GeoScience_PlanLongTerm] c  \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + "where \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " a.nodeid = c.nodeid  and c.costtype like '" + cost + "'  \r\n";

            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " and (year(thedate)*100)+month(thedate) = '" + ProdMonthSampTxt.Text + "'  \r\n";

            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " and a.holeno in (  select distinct(holeno) a from [dbo].[tbl_GeoScience_PlanLongTerm]  \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " where machineno in (  select machineno from [dbo].[tbl_LongHoleDrilling_Machine]  where company like  '" + mach + "') )  \r\n";
            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " and nsbook > 5) a  \r\n";

            _dbManSamp4.SqlStatement = _dbManSamp4.SqlStatement + " order by dd desc  \r\n";

            _dbManSamp4.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSamp4.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSamp4.ResultsTableName = "Table4";
            _dbManSamp4.ExecuteInstruction();

            DataTable dtPH3 = _dbManSamp4.ResultsDataTable;


            dsProbHist3.Tables.Add(dtPH3);

            ProbHistRep.RegisterData(dsProbHist3);

            MWDataManager.clsDataAccess _dbManSamp5 = new MWDataManager.clsDataAccess();
            _dbManSamp5.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            if (Radio.SelectedIndex == 0)
                _dbManSamp5.SqlStatement = " select top(10)  *, case when ProbResp = 'OPERATIONAL' then 'SandyBrown' else 'SeaGreen' end as c  from (select a.Probdesc, b.ProbResp, sum(probdur/60) dd from  ";
            else
                _dbManSamp5.SqlStatement = " select top(10)  *, case when ProbResp = 'OPERATIONAL' then 'SandyBrown' else 'SeaGreen' end as c  from (select a.Probdesc, b.ProbResp, count(ProblemID) dd from  ";

            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + "[dbo].[tbl_GeoScience_ProbBook] a,  \r\n";
            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + " [dbo].[tbl_GeoScience_PlanLongTerm] c , [tbl_GeoScience_Problems] b \r\n";

            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + "where a.problemid = b.probid and \r\n";
            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + " a.hole = c.holeno  and c.costtype like '" + cost + "'  \r\n";


            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + "and (year(thedate)*100)+month(thedate) = '" + ProdMonthSampTxt.Text + "' \r\n";



            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + " and hole in ( ";

            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + " select distinct(holeno) a from [dbo].[tbl_GeoScience_PlanLongTerm] ";
            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + " where machineno in ( ";
            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + " select machineno from [dbo].[tbl_LongHoleDrilling_Machine] ";
            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + " where company like  '" + mach + "' ) ) ";

            _dbManSamp5.SqlStatement = _dbManSamp5.SqlStatement + "group by a.Probdesc, b.ProbResp) a order by dd desc   \r\n";


            _dbManSamp5.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSamp5.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSamp5.ResultsTableName = "Table5";
            _dbManSamp5.ExecuteInstruction();

            DataTable dtPH4 = _dbManSamp5.ResultsDataTable;


            dsProbHist4.Tables.Add(dtPH4);

            ProbHistRep.RegisterData(dsProbHist4);


            MWDataManager.clsDataAccess _dbManSamp21 = new MWDataManager.clsDataAccess();
            _dbManSamp21.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSamp21.SqlStatement = " exec sp_GeoScienceProbHistGraph2 '" + mach + "', '" + cost + "'  " +
                                            "  " +
                                            "   ";

            _dbManSamp21.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSamp21.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSamp21.ResultsTableName = "TableNew";
            _dbManSamp21.ExecuteInstruction();

            DataTable dtPH11 = _dbManSamp21.ResultsDataTable;

            dsProbHist5.Tables.Add(dtPH11);

            ProbHistRep.RegisterData(dsProbHist5);

            ProbHistRep.Load(TGlobalItems.ReportsFolder+"\\ProblemHistoryGeology.frx");

            //ProbHistRep.Design();

            SampPC.Clear();
            ProbHistRep.Prepare();
            ProbHistRep.Preview = SampPC;
            ProbHistRep.ShowPrepared();

           


        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadReportHistReport();
        }

        private void ProdMonthSampTxt_Click(object sender, EventArgs e)
        {
            Procedures procs = new Procedures();
            procs.ProdMonthCalc(Convert.ToInt32(ProdMonthSampTxt.Text));
            ProdMonthSampTxt.Text = Procedures.Prod.ToString();
            procs.ProdMonthVis(Convert.ToInt32(ProdMonthSampTxt.Text));
            ProdMonthSamp1Txt.Text = Procedures.Prod2;
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ProbIDchkBx.Visible = true;
            GeoProbHistPnl.Dock = DockStyle.Fill;
            GeoProbHistPnl.Visible = true;


            DataSet dsGoeBook = new DataSet();
            DataSet dsGoeBook1 = new DataSet();


            MWDataManager.clsDataAccess _dbManSamp3 = new MWDataManager.clsDataAccess();
            _dbManSamp3.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSamp3.SqlStatement = " select '" + SysSettings.Banner + "' banner, '" + ProdMonthSamp1Txt.Text + "' mm " +
                                            "  " +
                                            "   ";

            _dbManSamp3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSamp3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSamp3.ResultsTableName = "Table3";
            _dbManSamp3.ExecuteInstruction();

            DataTable dtPH2 = _dbManSamp3.ResultsDataTable;


            dsGoeBook1.Tables.Add(dtPH2);

            GeolBook.RegisterData(dsGoeBook1);

            string isaa = "N";

            if (CmbBox.SelectedIndex != 0)
            {

                //MWDataManager.clsDataAccess _dbManTest = new MWDataManager.clsDataAccess();
                //_dbManTest.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                //_dbManTest.SqlStatement = " select * from [dbo].[tbl_Users_Geology] where userid =  '" + CmbBox.Text + "' and machine <> '<< ALL >>'" +
                //                                "  " +
                //                                "   ";

                //_dbManTest.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbManTest.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbManTest.ResultsTableName = "Tableaa";
                //_dbManTest.ExecuteInstruction();


                //if (_dbManTest.ResultsDataTable.Rows.Count > 0)
                    isaa = "Y";


            }



            String mach = "";

            if (Radio2.SelectedIndex == 0)
                mach = "%";
            if (Radio2.SelectedIndex == 1)
                mach = "P%";
            if (Radio2.SelectedIndex == 2)
                mach = "H%";
            if (Radio2.SelectedIndex == 3)
                mach = "L%";


            String cost = "";

            if (radioGroup1.SelectedIndex == 0)
                cost = "%";
            if (radioGroup1.SelectedIndex == 1)
                cost = "OR%";
            if (radioGroup1.SelectedIndex == 2)
                cost = "IB%";
            if (radioGroup1.SelectedIndex == 3)
                cost = "C%";

            string user = "";

            if (CmbBox.SelectedIndex == 0)
            {
                user = "%";
            }
            else
            {
                if (isaa == "N")
                    user = "%";
                else
                    user = CmbBox.Text + "%";
            }


            MWDataManager.clsDataAccess _dbMan3Mnth = new MWDataManager.clsDataAccess();
            _dbMan3Mnth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);


            if (ProbIDchkBx.Checked != true)
            {
                _dbMan3Mnth.SqlStatement = " exec sp_GeoScienceBookReportFinal '" + ProdMonthSampTxt.Text + "',  '" + user + "','" + cost + "','" + mach + "' ";
            }
            else
            {
                _dbMan3Mnth.SqlStatement = " exec sp_GeoScienceBookReportProbFinal '" + ProdMonthSampTxt.Text + "',  '" + user + "','" + cost + "','" + mach + "' ";
            }
            _dbMan3Mnth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3Mnth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3Mnth.ExecuteInstruction();

            DataTable dt = _dbMan3Mnth.ResultsDataTable;


            dsGoeBook.Tables.Add(dt);

            GeolBook.RegisterData(dsGoeBook);
           
            GeolBook.Load(TGlobalItems.ReportsFolder + "\\GeolBook.frx");

            //GeolBook.Design();

            SampPC.Clear();
            GeolBook.Prepare();
            GeolBook.Preview = SampPC;
            GeolBook.ShowPrepared();

        }

        private void Savebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void Closebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
