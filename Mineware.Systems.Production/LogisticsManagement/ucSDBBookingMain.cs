using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class ucSDBBookingMain : BaseUserControl
    {
        public ucSDBBookingMain()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpSDB_Booking);
            FormActiveRibbonPage = rpSDB_Booking;
            FormMainRibbonPage = rpSDB_Booking;
            RibbonControl = rcSDB_Booking;
        }


        #region Private variables
        string ActiveMinerID;
        string Firstload = "Y";
        DateTime EndDate = DateTime.Now;
        #endregion

        private void ucSDBBookingMain_Load(object sender, EventArgs e)
        {
            barProdmonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(ProductionGlobalTSysSettings._currentProductionMonth.ToString());

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "EXEC sp_SDB_Insert_Sections ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            barbtnShow_ItemClick(null, null);

            Firstload = "N";
        }

        private void barbtnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {


            //frmScheduleActivity Serv1Frm = new frmScheduleActivity();
            //Serv1Frm._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            //Serv1Frm.lblMainAct = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["Description"]).ToString();
            //Serv1Frm.lblMainActID = gvActivity.GetRowCellValue(gvActivity.FocusedRowHandle, gvActivity.Columns["mainactidaa"]).ToString();
            //Serv1Frm.lblFrmType = "Add";
            //Serv1Frm.lblDay = gvActivity.FocusedColumn.FieldName.ToString();
            //Serv1Frm.ShowDialog();

            // return;


            gcSDB.Visible = false;

            this.Cursor = Cursors.WaitCursor;

            LoadServices();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "Select max(BeginDate) BeginDate,  Max(EndDate) EndDate from tbl_Code_Calendar_Section where Prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "' and Sectionid like '" + barSection.EditValue.ToString() + "%' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {
                StartDate.Value = Convert.ToDateTime(_dbMan.ResultsDataTable.Rows[0][0].ToString());
                EndDate = Convert.ToDateTime(_dbMan.ResultsDataTable.Rows[0][1].ToString());
            }

            this.Cursor = Cursors.Default;
        }

        public void LoadServices()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "exec [dbo].[sp_SDB_BookingScreenMain] '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "',  '" + barSection.EditValue.ToString() + "',  '" + TUserInfo.UserID + "' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan1.SqlStatement = " select miner +':'+ name MinerName, miner    from [dbo].[tbl_SDB_ReportingBooking] a , \r\n" +
                                  "(select * from [dbo].[tbl_SDB_SECTION] a, (select sectionid ss, max(prodmonth) pm from [dbo].[tbl_SDB_SECTION] group by sectionid) b  \r\n" +
                                  "where a.sectionid = b.ss and a.prodmonth = b.pm) b  \r\n" +
                                   "where a.miner = b.sectionid and username = '" + TUserInfo.UserID + "'   and  substring(miner,1,4) = '" + barSection.EditValue.ToString() + "' group by miner, name order by miner +':'+ name \r\n";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            DataTable dtMain = _dbMan1.ResultsDataTable;

            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan2.SqlStatement = "select miner , orgunit, miner + orgunit zz from [dbo].[tbl_SDB_ReportingBooking] where username = '" + TUserInfo.UserID + "'   and  substring(miner,1,4) = '" + barSection.EditValue.ToString() + "' " +
                                    "group by  miner, orgunit order by miner, orgunit ";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ExecuteInstruction();

            DataTable dtcrew = _dbMan2.ResultsDataTable;

            MWDataManager.clsDataAccess _dbMan3 = new MWDataManager.clsDataAccess();
            _dbMan3.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan3.SqlStatement = "select  workplace, miner + orgunit  aa, miner + orgunit+workplace zzz, miner, orgunit, MainActDescriptionStart,  projectstart from [dbo].[tbl_SDB_ReportingBooking] where username = '" + TUserInfo.UserID + "'   and  substring(miner,1,4) = '" + barSection.EditValue.ToString() + "' group by  miner, orgunit, MainActDescriptionStart,  projectstart " +
                                   " , workplace order by miner, orgunit, workplace, projectstart   ";
            _dbMan3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3.ExecuteInstruction();

            DataTable dtDetail1 = _dbMan3.ResultsDataTable;

            DataTable dtDetail2 = _dbMan3.ResultsDataTable;

            string miner = string.Empty;
            string crew = string.Empty;
            string Wp = string.Empty;
            string Act = string.Empty;

            SelectionTreeView.Nodes.Clear();

            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                if (miner != dtMain.Rows[i]["Miner"].ToString())
                {
                    TreeNode minernode = new TreeNode(dtMain.Rows[i]["MinerName"].ToString());
                    minernode.NodeFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular, GraphicsUnit.Pixel);
                    minernode.ForeColor = Color.DimGray;

                    miner = dtMain.Rows[i]["Miner"].ToString();

                    SelectionTreeView.Nodes.Add(minernode);

                    //now add crew
                    for (int crewnum = 0; crewnum < dtcrew.Rows.Count; crewnum++)
                    {
                        if (dtcrew.Rows[crewnum]["miner"].ToString() == miner)
                        {
                            if (crew != dtcrew.Rows[crewnum]["orgunit"].ToString())
                            {
                                TreeNode crewnode = new TreeNode(dtcrew.Rows[crewnum]["orgunit"].ToString());
                                crewnode.NodeFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular, GraphicsUnit.Pixel);
                                crewnode.ForeColor = Color.DimGray;
                                crew = dtcrew.Rows[crewnum]["orgunit"].ToString();

                                minernode.Nodes.Add(crewnode);

                                // do workplace
                                for (int wpnum = 0; wpnum < dtDetail1.Rows.Count; wpnum++)
                                {
                                    if (dtDetail1.Rows[wpnum]["miner"].ToString() == miner)
                                    {
                                        if (dtDetail1.Rows[wpnum]["orgunit"].ToString() == crew)
                                        {
                                            if (dtDetail1.Rows[wpnum]["workplace"].ToString() != Wp)
                                            {
                                                TreeNode wpnode = new TreeNode(dtDetail1.Rows[wpnum]["workplace"].ToString());
                                                wpnode.NodeFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular, GraphicsUnit.Pixel);
                                                wpnode.ForeColor = Color.DimGray;
                                                Wp = dtDetail1.Rows[wpnum]["Workplace"].ToString();

                                                crewnode.Nodes.Add(wpnode);

                                                // do activities
                                                for (int Actnum = 0; Actnum < dtDetail2.Rows.Count; Actnum++)
                                                {
                                                    if (dtDetail2.Rows[Actnum]["miner"].ToString() == miner)
                                                    {
                                                        if (dtDetail2.Rows[Actnum]["orgunit"].ToString() == crew)
                                                        {
                                                            if (dtDetail2.Rows[Actnum]["workplace"].ToString() == Wp)
                                                            {

                                                                TreeNode actnode = new TreeNode(dtDetail2.Rows[Actnum]["MainActDescriptionStart"].ToString());
                                                                actnode.NodeFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular, GraphicsUnit.Pixel);
                                                                actnode.ForeColor = Color.DimGray;
                                                                Act = dtDetail2.Rows[Actnum]["MainActDescriptionStart"].ToString();

                                                                wpnode.Nodes.Add(actnode);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void barProdmonth_EditValueChanged(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan3 = new MWDataManager.clsDataAccess();
            _dbMan3.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan3.SqlStatement = "select  SectionID,Name from tbl_section where prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "'  and Hierarchicalid = 4 ";
            _dbMan3.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3.ExecuteInstruction();

            DataTable dtSection = new DataTable();
            dtSection = _dbMan3.ResultsDataTable;


            repSleSection.DataSource = null;
            repSleSection.DataSource = dtSection;
            repSleSection.DisplayMember = "Name";
            repSleSection.ValueMember = "SectionID";
            repSleSection.PopulateViewColumns();
            //repSleSection.View.Columns[0].Visible = false;

            barSection.EditValue = dtSection.Rows[0][0].ToString();

            if (Firstload == "N")
            {
                barbtnShow_ItemClick(null, null);
            }
        }

        private void SelectionTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (SelectionTreeView.SelectedNode != null)
            {
                SelectedLbl.Text = string.Empty;

                try
                {
                    Minerlabel.Visible = true;
                    Minerlabel.Text = SelectionTreeView.SelectedNode.Parent.Text.ToString();
                }
                catch { }

                try
                {
                    SelectedLbl.Visible = true;
                    SelectedLbl.Text = SelectionTreeView.SelectedNode.Text.ToString();

                }
                catch { }

            }
        }

        private void SelectedLbl_TextChanged(object sender, EventArgs e)
        {
            if (SelectedLbl.Text != string.Empty)
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbManSave.SqlStatement = "select convert(varchar(11), actstart, 113) sss, convert(varchar(11), actend, 113) eee, * from [dbo].[tbl_SDB_ReportingBooking] where username = '" + TUserInfo.UserID + "' and MainActDescriptionStart = '" + SelectedLbl.Text + "' and workplace = '" + Minerlabel.Text + "' " +
                                           "order by actstart, actend , order1 ";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSave.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManSave.ExecuteInstruction();

                DataTable dtDetail = _dbManSave.ResultsDataTable;

                if (dtDetail.Rows.Count > 0)
                {
                    ActiveMinerID = dtDetail.Rows[0]["Miner"].ToString();
                }

                gcSDB.DataSource = dtDetail;

                SDB1Task.FieldName = "SubActDescription";
                colssss.FieldName = "sss";
                coleee.FieldName = "eee";
                gcAuthID.FieldName = "AuthID";
                ColCompFinal.FieldName = "TaskComp";
                bandedGridColumn1.FieldName = "Day1Book";
                bandedGridColumn2.FieldName = "Day2Book";
                bandedGridColumn3.FieldName = "Day3Book";
                bandedGridColumn4.FieldName = "Day4Book";
                bandedGridColumn5.FieldName = "Day5Book";
                bandedGridColumn6.FieldName = "Day6Book";
                bandedGridColumn7.FieldName = "Day7Book";
                bandedGridColumn8.FieldName = "Day8Book";
                bandedGridColumn9.FieldName = "Day9Book";
                bandedGridColumn10.FieldName = "Day10Book";

                bandedGridColumn11.FieldName = "Day11Book";
                bandedGridColumn12.FieldName = "Day12Book";
                bandedGridColumn13.FieldName = "Day13Book";
                bandedGridColumn14.FieldName = "Day14Book";
                bandedGridColumn15.FieldName = "Day15Book";
                bandedGridColumn16.FieldName = "Day16Book";
                bandedGridColumn17.FieldName = "Day17Book";
                bandedGridColumn18.FieldName = "Day18Book";
                bandedGridColumn19.FieldName = "Day19Book";
                bandedGridColumn20.FieldName = "Day20Book";

                bandedGridColumn21.FieldName = "Day21Book";
                bandedGridColumn22.FieldName = "Day22Book";
                bandedGridColumn23.FieldName = "Day23Book";
                bandedGridColumn24.FieldName = "Day24Book";
                bandedGridColumn25.FieldName = "Day25Book";
                bandedGridColumn26.FieldName = "Day26Book";
                bandedGridColumn27.FieldName = "Day27Book";
                bandedGridColumn28.FieldName = "Day28Book";
                bandedGridColumn29.FieldName = "Day29Book";
                bandedGridColumn30.FieldName = "Day30Book";

                bandedGridColumn31.FieldName = "Day31Book";
                bandedGridColumn32.FieldName = "Day32Book";
                bandedGridColumn33.FieldName = "Day33Book";
                bandedGridColumn34.FieldName = "Day34Book";
                bandedGridColumn35.FieldName = "Day35Book";
                bandedGridColumn36.FieldName = "Day36Book";
                bandedGridColumn37.FieldName = "Day37Book";
                bandedGridColumn38.FieldName = "Day38Book";
                bandedGridColumn39.FieldName = "Day39Book";
                bandedGridColumn40.FieldName = "Day40Book";

                bandedGridColumn41.FieldName = "Day41Book";
                bandedGridColumn42.FieldName = "Day42Book";
                bandedGridColumn43.FieldName = "Day43Book";
                bandedGridColumn44.FieldName = "Day44Book";
                bandedGridColumn45.FieldName = "Day45Book";
                bandedGridColumn46.FieldName = "Day46Book";
                bandedGridColumn47.FieldName = "Day47Book";
                bandedGridColumn48.FieldName = "Day48Book";
                bandedGridColumn49.FieldName = "Day49Book";
                bandedGridColumn50.FieldName = "Day50Book";

                bandedGridColumn51.FieldName = "Day51Book";
                bandedGridColumn52.FieldName = "Day52Book";
                bandedGridColumn53.FieldName = "Day53Book";
                bandedGridColumn54.FieldName = "Day54Book";
                bandedGridColumn55.FieldName = "Day55Book";
                bandedGridColumn56.FieldName = "Day56Book";
                bandedGridColumn57.FieldName = "Day57Book";
                bandedGridColumn58.FieldName = "Day58Book";
                bandedGridColumn59.FieldName = "Day59Book";
                bandedGridColumn60.FieldName = "Day60Book";

                bandedGridColumn61.FieldName = "Day1";
                bandedGridColumn62.FieldName = "Day2";
                bandedGridColumn63.FieldName = "Day3";
                bandedGridColumn64.FieldName = "Day4";
                bandedGridColumn65.FieldName = "Day5";
                bandedGridColumn66.FieldName = "Day6";
                bandedGridColumn67.FieldName = "Day7";
                bandedGridColumn68.FieldName = "Day8";
                bandedGridColumn69.FieldName = "Day9";
                bandedGridColumn70.FieldName = "Day10";

                bandedGridColumn71.FieldName = "Day11";
                bandedGridColumn72.FieldName = "Day12";
                bandedGridColumn73.FieldName = "Day13";
                bandedGridColumn74.FieldName = "Day14";
                bandedGridColumn75.FieldName = "Day15";
                bandedGridColumn76.FieldName = "Day16";
                bandedGridColumn77.FieldName = "Day17";
                bandedGridColumn78.FieldName = "Day18";
                bandedGridColumn79.FieldName = "Day19";
                bandedGridColumn80.FieldName = "Day20";

                bandedGridColumn91.FieldName = "Day21";
                bandedGridColumn92.FieldName = "Day22";
                bandedGridColumn93.FieldName = "Day23";
                bandedGridColumn94.FieldName = "Day24";
                bandedGridColumn95.FieldName = "Day25";
                bandedGridColumn96.FieldName = "Day26";
                bandedGridColumn97.FieldName = "Day27";
                bandedGridColumn98.FieldName = "Day28";
                bandedGridColumn99.FieldName = "Day29";
                bandedGridColumn100.FieldName = "Day30";

                bandedGridColumn101.FieldName = "Day31";
                bandedGridColumn102.FieldName = "Day32";
                bandedGridColumn103.FieldName = "Day33";
                bandedGridColumn104.FieldName = "Day34";
                bandedGridColumn105.FieldName = "Day35";
                bandedGridColumn106.FieldName = "Day36";
                bandedGridColumn107.FieldName = "Day37";
                bandedGridColumn108.FieldName = "Day38";
                bandedGridColumn109.FieldName = "Day39";
                bandedGridColumn110.FieldName = "Day40";

                bandedGridColumn111.FieldName = "Day41";
                bandedGridColumn112.FieldName = "Day42";
                bandedGridColumn113.FieldName = "Day43";
                bandedGridColumn114.FieldName = "Day44";
                bandedGridColumn115.FieldName = "Day45";
                bandedGridColumn116.FieldName = "Day46";
                bandedGridColumn117.FieldName = "Day47";
                bandedGridColumn118.FieldName = "Day48";
                bandedGridColumn119.FieldName = "Day49";
                bandedGridColumn120.FieldName = "Day50";

                bandedGridColumn121.FieldName = "Day51";
                bandedGridColumn122.FieldName = "Day52";
                bandedGridColumn123.FieldName = "Day53";
                bandedGridColumn124.FieldName = "Day54";
                bandedGridColumn125.FieldName = "Day55";
                bandedGridColumn126.FieldName = "Day56";
                bandedGridColumn127.FieldName = "Day57";
                bandedGridColumn128.FieldName = "Day58";
                bandedGridColumn129.FieldName = "Day59";
                bandedGridColumn130.FieldName = "Day60";

                DateTime x = StartDate.Value;

                bandedGridColumn1.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn2.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn3.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn4.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn5.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn6.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn7.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn8.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn9.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn10.Caption = x.ToString("dd MMM ddd");

                x = x.AddDays(1);
                bandedGridColumn11.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn12.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn13.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn14.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn15.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn16.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn17.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn18.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn19.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn20.Caption = x.ToString("dd MMM ddd");

                x = x.AddDays(1);
                bandedGridColumn21.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn22.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn23.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn24.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn25.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn26.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn27.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn28.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn29.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn30.Caption = x.ToString("dd MMM ddd");

                x = x.AddDays(1);
                bandedGridColumn31.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn32.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn33.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn34.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn35.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn36.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn37.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn38.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn39.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn40.Caption = x.ToString("dd MMM ddd");

                x = x.AddDays(1);
                bandedGridColumn41.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn42.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn43.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn44.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn45.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn46.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn47.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn48.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn49.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn50.Caption = x.ToString("dd MMM ddd");

                x = x.AddDays(1);
                bandedGridColumn51.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn52.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn53.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn54.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn55.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn56.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn57.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn58.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn59.Caption = x.ToString("dd MMM ddd");
                x = x.AddDays(1);
                bandedGridColumn60.Caption = x.ToString("dd MMM ddd");

                gcSDB.Visible = true;
            }
        }

        private void gvSDB_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            Graphics g = e.Graphics;

            Brush hb = Brushes.LightGray;

            GridView View = sender as GridView;

            if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() == "OFF")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }

            if (View.GetRowCellValue(e.RowHandle, e.Column).ToString() != "OFF")
            {
                if (e.Column.Name == "bandedGridColumn1")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day1").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn2")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day2").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn3")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day3").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn4")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day4").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn5")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day5").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn6")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day6").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn7")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day7").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn8")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day8").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn9")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day9").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn10")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day10").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                // 10
                if (e.Column.Name == "bandedGridColumn11")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day11").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn12")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day12").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn13")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day13").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn14")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day14").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn15")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day15").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn16")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day16").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn17")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day17").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn18")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day18").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn19")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day19").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn20")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day20").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                // 20
                if (e.Column.Name == "bandedGridColumn21")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day21").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn22")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day22").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn23")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day23").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn24")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day24").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn25")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day25").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn26")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day26").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn27")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day27").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn28")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day28").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn29")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day29").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn30")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day30").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                //30
                if (e.Column.Name == "bandedGridColumn31")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day31").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn32")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day32").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn33")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day33").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn34")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day34").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn35")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day35").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn36")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day36").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn37")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day37").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn38")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day38").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn39")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day39").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn40")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day40").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                //40
                if (e.Column.Name == "bandedGridColumn41")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day41").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn42")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day42").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn43")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day43").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn44")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day44").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn45")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day45").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn46")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day46").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn47")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day47").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn48")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day48").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn49")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day49").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn50")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day50").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                //50
                if (e.Column.Name == "bandedGridColumn51")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day51").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn52")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day52").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn53")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day53").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn54")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day54").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn55")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day55").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn56")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day56").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn57")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day57").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn58")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day58").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn59")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day59").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

                if (e.Column.Name == "bandedGridColumn60")
                {
                    if (View.GetRowCellValue(e.RowHandle, "Day60").ToString() != string.Empty)
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                    }
                }

            }
        }

        private void gvSDB_DoubleClick(object sender, EventArgs e)
        {
            
            //Check if User Has rights for SDB Bookings
            MWDataManager.clsDataAccess _dbManRights = new MWDataManager.clsDataAccess();
            _dbManRights.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManRights.SqlStatement = " select isnull(SDB_Booking,0) SDB_Booking from tbl_Users_Synchromine where UserID =  '" + TUserInfo.UserID + "' ";
            _dbManRights.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRights.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRights.ExecuteInstruction();
            if(_dbManRights.ResultsDataTable.Rows.Count > 0)
            {
                if (_dbManRights.ResultsDataTable.Rows[0][0].ToString() == "0")
                {
                    MessageBox.Show("You don't Have rights to Book,Please Contact you Administrator");
                    return;
                }
            }

            if (gvSDB.FocusedRowHandle < 0)
                return;

            string aa = gvSDB.GetRowCellValue(gvSDB.FocusedRowHandle, gvSDB.Columns["TaskComp"]).ToString();

            if (gvSDB.GetRowCellValue(gvSDB.FocusedRowHandle, gvSDB.Columns["TaskComp"]).ToString() == "Y")
            {
                MessageBox.Show("This task has been marked as completed", "Complete Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            frmBookingServices BookingServ = new frmBookingServices();
            BookingServ.WpNamelbl.Text = Minerlabel.Text;
            BookingServ.Actlabel.Text = SelectedLbl.Text;
            BookingServ.Tasklabel.Text = gvSDB.GetRowCellValue(gvSDB.FocusedRowHandle, gvSDB.Columns["SubActDescription"]).ToString();
            BookingServ.AuthIDLbl.Text = gvSDB.GetRowCellValue(gvSDB.FocusedRowHandle, gvSDB.Columns["AuthID"]).ToString();
            BookingServ.ssslabel.Text = gvSDB.GetRowCellValue(gvSDB.FocusedRowHandle, gvSDB.Columns["sss"]).ToString();
            BookingServ.eelabel.Text = gvSDB.GetRowCellValue(gvSDB.FocusedRowHandle, gvSDB.Columns["eee"]).ToString();
            BookingServ.lblMinerID = ActiveMinerID;
            BookingServ.StartPosition = FormStartPosition.CenterScreen;
            BookingServ.lblProdmonth = barProdmonth.EditValue.ToString();
            BookingServ.lblMO = barSection.EditValue.ToString();
            BookingServ._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            BookingServ.ShowDialog();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = "exec [dbo].[sp_SDB_BookingScreenMain] '" + ProductionGlobal.ProductionGlobal.ProdMonthAsString(Convert.ToDateTime(barProdmonth.EditValue)) + "',  '" + barSection.EditValue.ToString() + "',  '" + TUserInfo.UserID + "' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            SelectedLbl_TextChanged(null, null);

            return;
        }

        private void barSection_EditValueChanged(object sender, EventArgs e)
        {
            if (Firstload == "N")
            {
                barbtnShow_ItemClick(null, null);
            }
        }

        private void SelectionTreeView_DoubleClick(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            frmSDBReport Serv1Frm = new frmSDBReport();

            Serv1Frm.ShowDialog();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void gcSDB_Click(object sender, EventArgs e)
        {

        }
    }
}
