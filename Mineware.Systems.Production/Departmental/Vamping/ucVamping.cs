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
using DevExpress.XtraGrid.Views.Grid;
using FastReport;
using System.IO;
using Mineware.Systems.Production.Security;

namespace Mineware.Systems.Production.Departmental.Vamping
{
    public partial class ucVamping : BaseUserControl
    {
        DialogResult result1;
        OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        string sourceFile;
        string destinationFile;
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        string FileName = "";

        string ImageDir = "";
        DataTable dt = new DataTable();
        Report theReport11 = new Report();
        Report theReport = new Report();

        decimal VampTons;
        decimal VampContent;
        decimal VampTotSqm;
        decimal VampWidth;
        decimal SweepTons;
        decimal SweepContent;
        decimal SweepTotSqm;
        decimal ReSweepTons;
        decimal ReSweepContent;
        decimal ReSweepTotSqm;

        decimal AbnormalTons;
        decimal AbnormalContent;

        decimal TotalSqm;
        decimal TotalTons;
        decimal TotalValue;
        decimal TotalContent;

        decimal GoldKG;
        decimal Revenue;

        public ucVamping()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpVamping);
            FormActiveRibbonPage = rpVamping;
            FormMainRibbonPage = rpVamping;
            RibbonControl = rcVamping;
        }

        private void ucVamping_Load(object sender, EventArgs e)
        {
            bool HasAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Vamping_PreInspection);
            if (HasAccess != true)
            {
                tabControl1.TabPages.Remove(tabPage2);
            }

            bool HasAccess2 = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Vamping_Planning);
            if (HasAccess2 != true)
            {
                tabControl1.TabPages.Remove(tabPage1);
            }

            tabPage1.Text = "PreInspection";
            tabPage2.Text = "Authorization";

            MWDataManager.clsDataAccess _dbManWPST2 = new MWDataManager.clsDataAccess();
            _dbManWPST2.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWPST2.SqlStatement = "Select * from tbl_sysset  \r\n";
            _dbManWPST2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWPST2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWPST2.ExecuteInstruction();

            if (_dbManWPST2.ResultsDataTable.Rows.Count > 0)
            {
                ImageDir = _dbManWPST2.ResultsDataTable.Rows[0]["REPDIR"].ToString();
            }

            this.Cursor = Cursors.WaitCursor;
            LoadPreInspection();
            this.Cursor = Cursors.Default;
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

        private void LoadPreInspection()
        {
            //Load Workplaces
            //WPcombo.Items.Clear();
            // listWP.Clear();
            //lbWP.Items.Clear();

            // WPGridView.RowCount = 5000;


            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWP.SqlStatement = " select * from vw_VampingWorkplace order by Workplace ";
            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            DataTable dtWP = new DataTable();

            dtWP = _dbManWP.ResultsDataTable;

            BindingSource bsa = new BindingSource();

            bsa.DataSource = dtWP;
            WPGridView.DataSource = bsa;

            if (WPGridView.RowCount > 0)
            {

                WPGridView.Columns[0].Width = 200;
                WPGridView.Columns[1].Width = 120;
                WPGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                WPGridView.Columns[2].Width = 120;
                WPGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                WPGridView.Columns[3].Width = 120;
                WPGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                WPGridView.Columns[3].Visible = false;
            }
            else
            {
                MessageBox.Show("No workplaces found");
            }
           
            //int x = 0;
            //foreach (DataRow dr in dtWP.Rows)
            //{
            //    //WPcombo.Items.Add(dr["WP"].ToString());
            //    //listWP.Add(dr["WP"].ToString());
            //    WPGridView.Rows[x].Cells[0].Value = dr["wp"].ToString();
            //    WPGridView.Rows[x].Cells[1].Value = dr["lbl"].ToString();
            //    WPGridView.Rows[x].Cells[2].Value = dr["cmgt"].ToString();
            //    x = x + 1;

            //    //lbWP.Items.Add(dr["WP"].ToString());
            //}

            //WPGridView.RowCount = x;

            //Load Section
            SectionCombo.Items.Clear();
            MWDataManager.clsDataAccess _dbManSections = new MWDataManager.clsDataAccess();
            _dbManSections.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSections.SqlStatement = " select distinct(Section) Section from ( \r\n " +
                                         " select SectionID + ':' + Name Section from tbl_section where prodmonth = (select CurrentProductionMonth from tbl_sysset)  and hierarchicalid = 4" +

                                          // " select sc.SectionID_2 + ':' + sc.Name_2 Section from planmonth pm, Sections_Complete sc \r\n "+
                                          // " where pm.prodmonth = (select CurrentProductionMonth from sysset) \r\n "+
                                          //  " and pm.prodmonth = sc.prodmonth \r\n "+
                                          //   " and pm.sectionid = sc.sectionid  \r\n " +
                                          " )a order by Section ";
            _dbManSections.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSections.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSections.ExecuteInstruction();

            DataTable dtSections = _dbManSections.ResultsDataTable;

            foreach (DataRow dr in dtSections.Rows)
            {
                SectionCombo.Items.Add(dr["Section"].ToString());
            }

            //Load Factors
            MWDataManager.clsDataAccess _dbManFactors = new MWDataManager.clsDataAccess();
            _dbManFactors.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManFactors.SqlStatement = " select * from dbo.tbl_SYSSET_MonthlyFactors " +
                                         " where prodmonth = (select currentproductionmonth from tbl_sysset) ";
            _dbManFactors.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManFactors.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManFactors.ExecuteInstruction();

            DataTable dtFactors = _dbManFactors.ResultsDataTable;

            foreach (DataRow dr in dtFactors.Rows)
            {
                txtMCF.Text = dr["MCF"].ToString();
                txtFactor.Text = dr["Rec"].ToString();
                txtGoldPrice.Text = dr["GoldPrice"].ToString();
            }

            txtAttachment.Text = "";

            txtOfficer.Text = TUserInfo.UserID;
            txtReference.Text = DateTime.Now.ToString("yyMMddHHmmss");

        }


        private void LoadUnAuthGrid()
        { }

        private void LoadVampAuthGrid()
        {
            UnAuthGrid.ColumnCount = 0;
            UnAuthGrid.RowCount = 1;


            UnAuthGrid.ColumnCount = 4;
            UnAuthGrid.RowCount = 5000;

            

            UnAuthGrid.Columns[0].Width = 200;
            UnAuthGrid.Columns[1].Width = 140;
            UnAuthGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            UnAuthGrid.Columns[2].Width = 120;
            UnAuthGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            UnAuthGrid.Columns[3].Width = 80;
            UnAuthGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;


            DataGridViewButtonColumn EditColumn = new DataGridViewButtonColumn();
            EditColumn.Text = "Authorise";
            EditColumn.UseColumnTextForButtonValue = true;

            UnAuthGrid.Columns.Add(EditColumn);

            UnAuthGrid.Columns[4].Width = 100;

            UnAuthGrid.Columns[0].HeaderText = "Workplace";
            UnAuthGrid.Columns[1].HeaderText = "Date";
            UnAuthGrid.Columns[2].HeaderText = "Grade Officer";
            UnAuthGrid.Columns[3].HeaderText = "g/t";

            UnAuthGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            UnAuthGrid.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


            MWDataManager.clsDataAccess _dbManUnAuth = new MWDataManager.clsDataAccess();
            _dbManUnAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManUnAuth.SqlStatement = "SELECT [Workplaceid], Calendardate, VampValue, GradeOfficer " +
                                            "FROM [tbl_VampingPreInspectionSheet] where auth <> 'Y' and Workplaceid like '%" + txtFilterWorkplaceUnAuth.Text+"%' order by Calendardate ";
            _dbManUnAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManUnAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManUnAuth.ExecuteInstruction();

            dt = _dbManUnAuth.ResultsDataTable;
            int x = 0;
            foreach (DataRow dr in dt.Rows)
            {
                UnAuthGrid.Rows[x].Cells[0].Value = dr["Workplaceid"].ToString();
                UnAuthGrid.Rows[x].Cells[1].Value = Convert.ToDateTime(dr["calendardate"].ToString()).ToString("dd-MMM-yyyy");

                UnAuthGrid.Rows[x].Cells[2].Value = dr["gradeofficer"].ToString();
                UnAuthGrid.Rows[x].Cells[3].Value = dr["VampValue"].ToString();

                x = x + 1;
            }

            UnAuthGrid.RowCount = x + 1;
            UnAuthGrid.Rows[x].Height = 0;



            AuthGrid.ColumnCount = 0;
            AuthGrid.RowCount = 1;


            AuthGrid.ColumnCount = 4;
            AuthGrid.RowCount = 5000;


            AuthGrid.Columns[0].Width = 180;
            AuthGrid.Columns[1].Width = 100;
            AuthGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            AuthGrid.Columns[2].Width = 100;
            AuthGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            AuthGrid.Columns[3].Width = 100;
            AuthGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            DataGridViewButtonColumn EditColumn1 = new DataGridViewButtonColumn();
            EditColumn1.Text = "De-Activate";
            EditColumn1.UseColumnTextForButtonValue = true;

            AuthGrid.Columns.Add(EditColumn1);

            AuthGrid.Columns[4].Width = 100;

            AuthGrid.Columns[0].HeaderText = "Workplace";
            AuthGrid.Columns[1].HeaderText = "Date";
            AuthGrid.Columns[2].HeaderText = "Grade Officer";
            AuthGrid.Columns[3].HeaderText = "g/t";

            AuthGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            AuthGrid.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


            MWDataManager.clsDataAccess _dbManAuth = new MWDataManager.clsDataAccess();
            _dbManAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManAuth.SqlStatement = "SELECT [Workplaceid], Calendardate, VampValue, GradeOfficer " +
                                            "FROM [tbl_VampingPreInspectionSheet] where auth = 'Y' and Workplaceid like '%" + txtFilterWorkplaceAuth.Text+"%' and Del <> 'Y' order by Calendardate ";
            _dbManAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManAuth.ExecuteInstruction();

            dt = _dbManAuth.ResultsDataTable;
            x = 0;
            foreach (DataRow dr in dt.Rows)
            {
                AuthGrid.Rows[x].Height = 20;
                AuthGrid.Rows[x].Cells[0].Value = dr["Workplaceid"].ToString();
                AuthGrid.Rows[x].Cells[1].Value = Convert.ToDateTime(dr["calendardate"].ToString()).ToString("dd-MMM-yyyy");

                AuthGrid.Rows[x].Cells[2].Value = dr["gradeofficer"].ToString();
                AuthGrid.Rows[x].Cells[3].Value = dr["VampValue"].ToString();

                x = x + 1;
            }

            AuthGrid.RowCount = x + 1;
            AuthGrid.Rows[x].Height = 0;





            DeActGrid.ColumnCount = 0;
            DeActGrid.RowCount = 1;


            DeActGrid.ColumnCount = 4;
            DeActGrid.RowCount = 5000;
            
            DeActGrid.Columns[0].Width = 180;
            DeActGrid.Columns[1].Width = 100;
            DeActGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DeActGrid.Columns[2].Width = 100;
            DeActGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DeActGrid.Columns[3].Width = 100;
            DeActGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            DataGridViewButtonColumn EditColumn2 = new DataGridViewButtonColumn();
            EditColumn2.Text = "Re-Activate";
            EditColumn2.UseColumnTextForButtonValue = true;

            DeActGrid.Columns.Add(EditColumn2);

            DeActGrid.Columns[4].Width = 100;

            DeActGrid.Columns[0].HeaderText = "Workplace";
            DeActGrid.Columns[1].HeaderText = "Date";
            DeActGrid.Columns[2].HeaderText = "Grade Officer";
            DeActGrid.Columns[3].HeaderText = "g/t";

            DeActGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            DeActGrid.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


            MWDataManager.clsDataAccess _dbManDeAct = new MWDataManager.clsDataAccess();
            _dbManDeAct.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManDeAct.SqlStatement = "SELECT [Workplaceid], Calendardate, VampValue, GradeOfficer " +
                                            "FROM [tbl_VampingPreInspectionSheet] where auth = 'Y' and Del = 'Y' and WorkplaceID like '%" + txtFilterWorkplaceDeAct.Text+"%' order by Calendardate ";
            _dbManDeAct.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDeAct.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDeAct.ExecuteInstruction();

            dt = _dbManDeAct.ResultsDataTable;
            x = 0;
            foreach (DataRow dr in dt.Rows)
            {
                DeActGrid.Rows[x].Height = 20;
                DeActGrid.Rows[x].Cells[0].Value = dr["Workplaceid"].ToString();
                DeActGrid.Rows[x].Cells[1].Value = Convert.ToDateTime(dr["calendardate"].ToString()).ToString("dd-MMM-yyyy");

                DeActGrid.Rows[x].Cells[2].Value = dr["gradeofficer"].ToString();
                DeActGrid.Rows[x].Cells[3].Value = dr["VampValue"].ToString();

                x = x + 1;
            }

            DeActGrid.RowCount = x + 1;
            DeActGrid.Rows[x].Height = 0;



        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowProgressPage(true);
            if (tabControl1.SelectedIndex == 0)
            {
                LoadPreInspection();
            }

            if (tabControl1.SelectedIndex == 1)
            {
                LoadVampAuthGrid();
            }
            this.ShowProgressPage(false);
        }

        private void AddOldWPBtn_Click(object sender, EventArgs e)
        {
            frmAddOldWp Propfrm = new frmAddOldWp();

            Propfrm._theSystemDBTag = theSystemDBTag;
            Propfrm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
            Propfrm.Userinfo = UserCurrentInfo;

            Propfrm.ShowDialog(this);

            if (tabControl1.SelectedIndex == 0)
            {
                LoadPreInspection();
            }

            if (tabControl1.SelectedIndex == 1)
            {
                LoadVampAuthGrid();
            }

        }

        private void btnSavePreInspect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                SavePreInspection();
            }
        }

        private void SavePreInspection()
        {
            CalChange();

            ShowRep();

            string OldGold = "N";

            if (OldGoldCheck.Checked == true)
                OldGold = "Y";

            if (ExtractBeforeColon(SectionCombo.Text) == "")
            {
                MessageBox.Show("Please select a section", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ExtractBeforeColon(SectionCombo.Text) == "")
            {
                MessageBox.Show("Please select a section", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " delete tbl_VampingPreInspectionSheet where workplaceid = '" + txtWP.Text + "' \r\n " +// and calendardate = '" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "' \r\n " +
                " insert into tbl_VampingPreInspectionSheet \r\n " +
                " select '" + txtWP.Text + "' WP, '" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "' Date, '" + txtOfficer.Text + "' Officer, '" + txtReference.Text + "' Reference, \r\n " +
                " '" + ExtractBeforeColon(SectionCombo.Text) + "' Section, '" + txtVampMetres.Text + "' VampMetres, '" + VampWidth + "' VampWidth, '" + txtVampHeight.Text + "' VampHeight,  \r\n " +
                " '" + txtVampSqm.Text + "' VampSqm, '" + VampTons + "' VampTons, '" + txtVampValue.Text + "' VampValue, '" + VampContent + "' VampContent, '" + VampTotSqm + "' VampTotSqm, \r\n " +
                " '" + txtSweepHeight.Text + "' SweepHeight, '" + txtSweepSqm.Text + "' SweepSqm, '" + SweepTons + "' SweepTons, '" + txtSweepValue.Text + "' SweepValue, '" + SweepContent + "' SweepContent, '" + SweepTotSqm + "' SweepTotSqm, \r\n " +
                " '" + txtReSweepHeight.Text + "' ReSweepHeight, '" + txtReSweepSqm.Text + "' ReSweepSqm, '" + ReSweepTons + "' ReSweepTons, '" + txtReSweepValue.Text + "' ReSweepValue, '" + ReSweepContent + "' ReSweepContent, '" + ReSweepTotSqm + "' ReSweepTotSqm, \r\n " +
                " '" + txtSQM.Text + "' AbnormalSqm, '" + txtHeight.Text + "' AbnormalHeight, '" + AbnormalTons + "' AbnormalTons, '" + txtValue.Text + "' AbnormalValue, '" + AbnormalContent + "' AbnormalContent, \r\n" +
                " '" + TotalSqm + "' TotSqm, '" + TotalTons + "' TotTons, '" + TotalValue + "' TotValue, '" + TotalContent + "' TotContent, '" + txtMCF.Text + "' MCF, '" + txtFactor.Text + "' Factor, \r\n " +
                " '" + txtGoldPrice.Text + "' GoldPrice, '" + GoldKG + "' GoldKG, '" + Revenue + "' Revenue, '" + txtAttachment.Text + "' picture \r\n " +
                " , 'N', '', null, 'N', '', null, '" + RemTxt.Text + "', '" + OldGold + "' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();


            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Inspection saved", Color.CornflowerBlue);
        }

        private void CalChange()
        {

            VampTons = Math.Round(((Convert.ToDecimal(txtVampHeight.Text) * Convert.ToDecimal(txtVampSqm.Text)) * Convert.ToDecimal(1.67)), 0);
            VampContent = Math.Round((VampTons * Convert.ToDecimal(txtVampValue.Text)), 0);
            VampTotSqm = Math.Round((Convert.ToDecimal(txtVampSqm.Text)), 0);
            VampWidth = 0;
            if ((Convert.ToDecimal(txtVampSqm.Text) > 0) && (Convert.ToDecimal(txtVampMetres.Text) > 0))
                VampWidth = Math.Round((Convert.ToDecimal(txtVampSqm.Text) / Convert.ToDecimal(txtVampMetres.Text)), 2);

            SweepTons = Math.Round(((Convert.ToDecimal(txtSweepHeight.Text) * Convert.ToDecimal(txtSweepSqm.Text)) * Convert.ToDecimal(1.67)), 0);
            SweepContent = Math.Round((SweepTons * Convert.ToDecimal(txtSweepValue.Text)), 0);
            SweepTotSqm = Math.Round((Convert.ToDecimal(txtSweepSqm.Text)), 0);

            ReSweepTons = Math.Round(((Convert.ToDecimal(txtReSweepHeight.Text) * Convert.ToDecimal(txtReSweepSqm.Text)) * Convert.ToDecimal(1.67)), 0);
            ReSweepContent = Math.Round((ReSweepTons * Convert.ToDecimal(txtReSweepValue.Text)), 0);
            ReSweepTotSqm = Math.Round((Convert.ToDecimal(txtReSweepSqm.Text)), 0);

            AbnormalTons = Math.Round((Convert.ToDecimal(txtSQM.Text) * Convert.ToDecimal(txtHeight.Text) * Convert.ToDecimal(1.67)), 0);
            AbnormalContent = Math.Round((AbnormalTons * Convert.ToDecimal(txtValue.Text)), 0);

            TotalSqm = Math.Round((VampTotSqm + SweepTotSqm + ReSweepTotSqm), 0);
            TotalTons = Math.Round((VampTons + SweepTons + ReSweepTons), 0);
            TotalContent = Math.Round((VampContent + SweepContent + ReSweepContent), 0);

            TotalValue = Math.Round(Convert.ToDecimal(0.00000001), 2);
            if ((TotalTons > 0) && (TotalContent > 0))
                TotalValue = Math.Round((TotalContent / TotalTons), 2);

            GoldKG = 0;
            if ((TotalContent * Convert.ToDecimal(txtMCF.Text) * Convert.ToDecimal(txtFactor.Text)) > 0)
                GoldKG = Math.Round((TotalContent * (Convert.ToDecimal(txtMCF.Text) / 100) * (Convert.ToDecimal(txtFactor.Text) / 100) / 1000), 3);

            Revenue = Math.Round(Convert.ToDecimal(0.00000001), 2);
            if ((TotalContent * Convert.ToDecimal(txtMCF.Text) * Convert.ToDecimal(txtFactor.Text)) > 0)
                Revenue = Math.Round(((TotalContent * (Convert.ToDecimal(txtMCF.Text) / 100) * (Convert.ToDecimal(txtFactor.Text) / 100) / 1000) * 430000), 2);
            // End the calcs

            if (txtAttachment.Text == "")
                txtAttachment.Text = @"\\zacddat02.ag.ad.local\MineWarePics$\VampingInspection\NoImage.png";


        }

        private void ShowRep()
        {
            CalChange();

            //if (txtAttachment.Text == "")
            //    txtAttachment.Text = @"\\afzadcfss01.ag.ad.local\MineWarePic$\VampingInspection\NoImage.png";

            //for now
            //if (txtAttachment.Text == "")
            //    txtAttachment.Text = ImageDir + "\\VampingInspection\\NoImage.png";

            string oldgold = "Current Vamps";

            if (OldGoldCheck.Checked == true)
                oldgold = "Old Gold";

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " select '" + SysSettings.Banner + "' Banner, '" + txtWP.Text + "' WP, '" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "' Date, '" + txtOfficer.Text + "' Officer, '" + txtReference.Text + "' Reference, \r\n " +
                " '" + SectionCombo.Text + "' Section, '" + txtVampMetres.Text + "' VampMetres, '" + VampWidth + "' VampWidth, '" + txtVampHeight.Text + "' VampHeight,  \r\n " +
                " '" + txtVampSqm.Text + "' VampSqm, '" + VampTons + "' VampTons, '" + txtVampValue.Text + "' VampValue, '" + VampContent + "' VampContent, '" + VampTotSqm + "' VampTotSqm, \r\n " +
                " '" + txtSweepHeight.Text + "' SweepHeight, '" + txtSweepSqm.Text + "' SweepSqm, '" + SweepTons + "' SweepTons, '" + txtSweepValue.Text + "' SweepValue, '" + SweepContent + "' SweepContent, '" + SweepTotSqm + "' SweepTotSqm, \r\n " +
                " '" + txtReSweepHeight.Text + "' ReSweepHeight, '" + txtReSweepSqm.Text + "' ReSweepSqm, '" + ReSweepTons + "' ReSweepTons, '" + txtReSweepValue.Text + "' ReSweepValue, '" + ReSweepContent + "' ReSweepContent, '" + ReSweepTotSqm + "' ReSweepTotSqm, \r\n " +
                " '" + txtSQM.Text + "' AbnormalSqm, '" + txtHeight.Text + "' AbnormalHeight, '" + AbnormalTons + "' AbnormalTons, '" + txtValue.Text + "' AbnormalValue, '" + AbnormalContent + "' AbnormalContent, \r\n" +
                " '" + TotalSqm + "' TotSqm, '" + TotalTons + "' TotTons, '" + TotalValue + "' TotValue, '" + TotalContent + "' TotContent, '" + txtMCF.Text + "' MCF, '" + txtFactor.Text + "' Factor, \r\n " +
                " '" + txtGoldPrice.Text + "' GoldPrice, '" + GoldKG + "' GoldKG, '" + Revenue + "' Revenue, '" + txtAttachment.Text + "' picture, '" + RemTxt.Text + "' rem \r\n " +

                " , '" + oldgold + "' OldGold ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ResultsTableName = "GradeSheet";  //get table name
            _dbMan.ExecuteInstruction();

            DataSet ReportDatasetLosses = new DataSet();
            ReportDatasetLosses.Tables.Add(_dbMan.ResultsDataTable);
            theReport11.RegisterData(ReportDatasetLosses);

            theReport11.Load(_reportFolder + "\\VampingInspection.frx");

            //theReport11.Design();

            //btnSave_Click(null, null);

            pcReport2.Clear();
            theReport11.Prepare();
            theReport11.Preview = pcReport2;
            theReport11.ShowPrepared();
            pcReport2.Visible = true;

        }

        private void btnCreateInspectionReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CalChange();

            ShowRep();
        }

        private void UnAuthGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex == 0))
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " select '" + SysSettings.Banner + "' Banner, convert(varchar, calendardate, 106) CalDate,  * from tbl_VampingPreInspectionSheet where workplaceid = '" + UnAuthGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "'  \r\n ";//and Calendardate = '" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "' \r\n ";
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

                previewControl1.Clear();
                theReport.Prepare();
                theReport.Preview = previewControl1;

                theReport.ShowPrepared();
                previewControl1.Visible = true;

            }
        }

        private void UnAuthGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex == 4))
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " update tbl_VampingPreInspectionSheet  " +// and calendardate = '" + String.Format("{0:yyyy-MM-dd}", date1.Value) + "' \r\n " +
                    " set  Auth = 'Y', \r\n " +
                    " AuthPer = '" + TUserInfo.UserID + "',  \r\n " +
                    " AuthDate = '" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "'  \r\n " +
                    " where workplaceid = '" + UnAuthGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "' \r\n ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Record Updated", Color.CornflowerBlue);

                LoadVampAuthGrid();
            }
        }

        private void AuthGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex == 4))
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " update tbl_VampingPreInspectionSheet  " +
                    " set  Del = 'Y', \r\n " +
                    " DelPer = '" + TUserInfo.UserID + "',  \r\n " +
                    " DelDate = '" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "'  \r\n " +
                    " where workplaceid = '" + AuthGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "' \r\n ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Record Updated", Color.CornflowerBlue);

                LoadVampAuthGrid();
            }
        }

        private void AuthGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex == 0))
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " select '" + SysSettings.Banner + "' Banner, convert(varchar, calendardate, 106) CalDate,  * from tbl_VampingPreInspectionSheet where workplaceid = '" + AuthGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "'  \r\n ";
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

                previewControl1.Clear();
                theReport.Prepare();
                theReport.Preview = previewControl1;

                theReport.ShowPrepared();
                previewControl1.Visible = true;

            }
        }

        private void DeActGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex == 4))
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " update tbl_VampingPreInspectionSheet  " +
                    " set  Del = 'N' \r\n " +
                    //" DelPer = '" + clsUserInfo.UserName + "',  \r\n " +
                    //" DelDate = '" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "'  \r\n " +
                    " where workplaceid = '" + DeActGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "' \r\n ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Record Updated", Color.CornflowerBlue);

                LoadVampAuthGrid();
            }
        }

        private void DeActGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex == 0))
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _dbMan.SqlStatement = " select '" + SysSettings.Banner + "' Banner, convert(varchar, calendardate, 106) CalDate,  * from tbl_VampingPreInspectionSheet where workplaceid = '" + DeActGrid.Rows[e.RowIndex].Cells[0].Value.ToString() + "'  \r\n ";
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

                previewControl1.Clear();
                theReport.Prepare();
                theReport.Preview = previewControl1;

                theReport.ShowPrepared();
                previewControl1.Visible = true;

            }
        }

        private void WPGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtWP.Text = WPGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtVampValue.Text = WPGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtSweepValue.Text = WPGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtReSweepValue.Text = WPGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtValue.Text = WPGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            OldGoldCheck.Checked = false;
            if (WPGridView.Rows[e.RowIndex].Cells[2].Value.ToString() == "Old")
                OldGoldCheck.Checked = true;

            date1_CloseUp(null, null);
        }

        private void date1_CloseUp(object sender, EventArgs e)
        {
            //Check if the record exists, then load fields
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " Select Name_2,a.* from (      \r\n " +
                                  " select * from tbl_VampingPreInspectionSheet where workplaceid = '" + txtWP.Text + "' )a  \r\n " +
                                  "left outer join (   \r\n"+
                                  "Select Sectionid_2, Name_2 from sections_Complete where prodmonth = (select max(prodmonth) from tbl_PLANMONTH ))b \r\n"+
                                  " on a.section = b.Sectionid_2 ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            txtVampMetres.Text = "0.00";
            txtVampHeight.Text = "0.00";
            txtSweepHeight.Text = "0.00";
            txtReSweepHeight.Text = "0.00";

            txtVampSqm.Text = "0";
            txtSweepSqm.Text = "0";
            txtReSweepSqm.Text = "0";

            RemTxt.Text = "";

            //txtVampValue.Text = "0.00";
            //txtSweepValue.Text = "0.00";
            //txtReSweepValue.Text = "0.00";

            txtSQM.Text = "0";
            txtHeight.Text = "0.00";
            // txtValue.Text = "0";


            SectionCombo.Text = "";
            // txtReference.Text = "0";

            txtAttachment.Text = "";

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    VampTons = Convert.ToDecimal(dr["VampTons"].ToString());
                    VampContent = Convert.ToDecimal(dr["VampContent"].ToString());
                    VampTotSqm = Convert.ToDecimal(dr["VampTotalSqm"].ToString());
                    VampWidth = Convert.ToDecimal(dr["VampWidth"].ToString());
                    SweepTons = Convert.ToDecimal(dr["SweepTons"].ToString());
                    SweepContent = Convert.ToDecimal(dr["SweepContent"].ToString());
                    SweepTotSqm = Convert.ToDecimal(dr["SweepTotalSqm"].ToString());
                    ReSweepTons = Convert.ToDecimal(dr["ReSweepTons"].ToString());
                    ReSweepContent = Convert.ToDecimal(dr["ReSweepContent"].ToString());
                    ReSweepTotSqm = Convert.ToDecimal(dr["ReSweepTotalSqm"].ToString());

                    AbnormalTons = Convert.ToDecimal(dr["AbnormalTons"].ToString());
                    AbnormalContent = Convert.ToDecimal(dr["AbnormalContent"].ToString());

                    TotalSqm = Convert.ToDecimal(dr["TotalSqm"].ToString());
                    TotalTons = Convert.ToDecimal(dr["TotalTons"].ToString());
                    TotalValue = Convert.ToDecimal(dr["TotalValue"].ToString());
                    TotalContent = Convert.ToDecimal(dr["TotalContent"].ToString());

                    GoldKG = Convert.ToDecimal(dr["GoldKG"].ToString());
                    Revenue = Convert.ToDecimal(dr["Revenue"].ToString());

                    txtVampMetres.Text = dr["VampMetres"].ToString();
                    txtVampHeight.Text = dr["VampHeight"].ToString();
                    txtSweepHeight.Text = dr["SweepHeight"].ToString();
                    txtReSweepHeight.Text = dr["ReSweepHeight"].ToString();

                    txtVampSqm.Text = dr["VampSqm"].ToString();
                    txtVampValue.Text = dr["VampValue"].ToString();
                    txtSweepSqm.Text = dr["SweepSqm"].ToString();
                    txtSweepValue.Text = dr["SweepValue"].ToString();
                    txtReSweepSqm.Text = dr["ReSweepSqm"].ToString();
                    txtReSweepValue.Text = dr["ReSweepValue"].ToString();

                    //txtVampValue.Text = dr["VampValue"].ToString();
                    //txtSweepValue.Text = dr["SweepValue"].ToString();
                    //txtReSweepValue.Text = dr["ReSweepValue"].ToString();

                    txtSQM.Text = dr["AbnormalSqm"].ToString();
                    txtHeight.Text = dr["AbnormalHeight"].ToString();
                    txtValue.Text = dr["AbnormalValue"].ToString();

                    txtOfficer.Text = dr["GradeOfficer"].ToString();
                    SectionCombo.Text = dr["Section"].ToString() + ":" + dr["Name_2"].ToString();
                    txtReference.Text = dr["Reference"].ToString();

                    txtAttachment.Text = dr["Picture"].ToString();
                    RemTxt.Text = dr["rem"].ToString();

                    if (dr["OldGold"].ToString() == "Y")
                        OldGoldCheck.Checked = true;
                }


            }
            btnCreateInspectionReport_ItemClick(null, null);
        }

        private void btnAttachFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialog1.FileName = null;
            result1 = openFileDialog1.ShowDialog();

            //Now get and copy the file
            GetFile();
            ShowRep();
        }


        void GetFile()
        {
            Random r = new Random();
            //System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(@"C:\Images");
            // System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(@"\\172.18.16.77\images\GradeSheets");
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(ImageDir +"\\VampingInspection");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            //string[] files = System.IO.Directory.GetFiles(@"C:\Images");
            //string[] files = System.IO.Directory.GetFiles(@"\\172.18.16.77\images\GradeSheets");
            string[] files = System.IO.Directory.GetFiles(ImageDir + "\\VampingInspection");

            if (result1 == DialogResult.OK)
            {


                int Name = r.Next(0, 50);
                FileName = "";
                string Ext = "";

                int index = 0;

                sourceFile = openFileDialog1.FileName;


                index = openFileDialog1.SafeFileName.IndexOf(".");
                //FileName = openFileDialog1.SafeFileName.Substring(0, index);

                if (txtWP.Text != "")
                {
                    FileName = String.Format("{0:yyyyMMddhhmmss}", date1.Value);
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog1.SafeFileName.Substring(index);

                //destinationFile = @"C:\Images" + "\\" + FileName + Ext;
                destinationFile = ImageDir + "\\VampingInspection" + "\\" + FileName + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            //destinationFile = @"C:\Images" + "\\" + FileName + Ext;//+ FileName + Name.ToString() + Ext
                            destinationFile = ImageDir + "\\VampingInspection" + "\\" + FileName + Ext;//+ FileName + Name.ToString() + Ext
                        }

                    }

                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);
                    }
                    catch
                    {

                    }

                    //MessageFrm MsgFrm = new MessageFrm();
                    //MsgFrm.Text = "Attachment";
                    //Procedures.MsgText = "File Attached";
                    //MsgFrm.Show();
                }
                else
                {
                    //System.IO.File.Copy(sourceFile, destinationFile, true);
                    System.IO.File.Copy(sourceFile, ImageDir + "\\VampingInspection" + "\\" + FileName + Ext, true);

                    //MessageFrm MsgFrm = new MessageFrm();
                    //MsgFrm.Text = "Attachment";
                    //Procedures.MsgText = "File Attached";
                    //MsgFrm.Show();

                    dir2 = new System.IO.DirectoryInfo(ImageDir + "\\VampingInspection");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }

                txtAttachment.Text = destinationFile;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            //string zzzz = "*" + txtFilter.Text;            

            MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
            _dbManWP.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManWP.SqlStatement = " select * from vw_VampingWorkplace where workplace like '"+ txtFilter.Text + "%' order by Workplace ";
            _dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManWP.ExecuteInstruction();

            DataTable dtWP = new DataTable();

            dtWP = _dbManWP.ResultsDataTable;

            BindingSource bsa = new BindingSource();

            bsa.DataSource = dtWP;
            WPGridView.DataSource = bsa;

            txtWP.Text = dtWP.Rows[0]["Workplace"].ToString();
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

        private void RCVentilation_Click(object sender, EventArgs e)
        {

        }

        private void txtFilterWorkplace_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            LoadVampAuthGrid();
        }

        private void txtFilterWorkplaceUnAuth_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            LoadVampAuthGrid();
        }

        private void txtFilterWorkplaceDeAct_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            LoadVampAuthGrid();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
