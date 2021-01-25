using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using FastReport;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using Mineware.Systems.ProductionHelp;
using Mineware.Systems.ProductionReports.PillarComplianceReport;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Planning
{
    public partial class ucGraphicsPrePlanMain : BaseUserControl
    {
        #region Fields and Properties
        private String dbl_result_Crew;
        private String dbl_result_ProdMonth;
        private String dbl_result_Section;
        private string HasPreplanRights = "N";
        static string colCaption = string.Empty;
        private string SelectProdmonth;
        private string FirstLoad = "Y";

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

        private Procedures procs = new Procedures();
        Report theReport = new Report();

        #endregion Fields and Properties

        #region Constructor
        public ucGraphicsPrePlanMain()
        {
            InitializeComponent();

            FormRibbonPages.Add(rpPreplanning);
            FormActiveRibbonPage = rpPreplanning;
            FormMainRibbonPage = rpPreplanning;
            RibbonControl = rcPreplanning;
        }
        #endregion Constructor

        #region Events
        private void ucGraphicsPrePlanMain_Load(object sender, EventArgs e)
        {
            //ShowProgressPage(true);

            mwRepositoryItemProdMonth1.EditValueChanged += ProdmonthChangedcustomMethod;

            //Setup global variables
            //TODO: Remove this code when ONLogin event is working on the plugin class
            Mineware.Systems.ProductionGlobal.ProductionGlobal.GetSysSettings(theSystemDBTag, UserCurrentInfo);

            //Get the users permissions
            GetDepSetup();

            //Set production month
            SelectProdmonth = ProductionGlobalTSysSettings._currentProductionMonth.ToString();
            barProdMonth.EditValue = ProductionGlobal.ProductionGlobal.ProdMonthAsDate(SelectProdmonth);

            xtraTabControl1.TabPages[0].Text = "   Departments   ";
            xtraTabControl1.TabPages[1].Text = "   Security      ";

            xtraTabControl1.TabPages[1].PageVisible = false;

            pictureBox1.Image = imageCollection1.Images[2];
            pictureBox2.Image = imageCollection1.Images[3];
            pictureBox3.Image = imageCollection1.Images[4];
            pictureBox4.Image = imageCollection1.Images[5];
            pictureBox5.Image = imageCollection1.Images[6];

            FirstLoad = "N";
        }
        DateTime Prodmonth = DateTime.Now;
        private void ProdmonthChangedcustomMethod(object sender, EventArgs e)
        {
            Mineware.Systems.Global.CustomControls.MWProdmonthEdit prodmonthobj = (Mineware.Systems.Global.CustomControls.MWProdmonthEdit)sender;
            barProdMonth.EditValue = prodmonthobj.EditValue.ToString();
            //Prodmonth = (DateTime)barProdMonth.EditValue; 
        }

        private void barProdMonth_EditValueChanged(object sender, EventArgs e)
        {
            //Prodmonth = (DateTime)barProdMonth.EditValue;
            System.Threading.Tasks.Task.Run(() => LoadSections());

            
        }

        private void MO_EditValueChanged(object sender, EventArgs e)
        {
            LoadPreplanning();
            InsertPreplanMonth();            
            LoadStartupData(Prodmonth, MOSection.EditValue.ToString());
            
            
        }

        private void gvPrePlanMainGraphics_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                if (e.Column.Name == "colProduction" || e.Column.Name == "colServices"
                    || e.Column.Name == "colSafety" || e.Column.Name == "colRockEng"
                    || e.Column.Name == "colOccAndEnv" || e.Column.Name == "colHR"
                    || e.Column.Name == "colSurvey" || e.Column.Name == "colGeoScience"
                    || e.Column.Name == "colEngineering" || e.Column.Name == "colAuth"
                    || e.Column.Name == "colPlanRR")
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);
                    e.Handled = true;
                }
            }
        }

        private void gvPrePlanMainGraphics_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            colCaption = gvPrePlanMainGraphics.FocusedColumn.Caption;

            dbl_result_Crew = gvPrePlanMainGraphics.GetFocusedRowCellValue(gvPrePlanMainGraphics.Columns["Crew"]).ToString();
            dbl_result_Section = MOSection.EditValue.ToString();
            dbl_result_ProdMonth = gvPrePlanMainGraphics.GetFocusedRowCellValue(gvPrePlanMainGraphics.Columns["ProdMonth"]).ToString();

            string Activity = gvPrePlanMainGraphics.GetFocusedRowCellValue(gvPrePlanMainGraphics.Columns["ActType"]).ToString();

            string _sqlWhere = "UserID = '" + TUserInfo.UserID + "'";
            string _sqlOrder = "UserID asc";

            Form frm_Cap_Sheets = new Form();

            if (colCaption == "Crew")
            {
                ucGraphicsPrePlanningReports rep = new ucGraphicsPrePlanningReports();
                rep.Dock = DockStyle.Fill;
                rep._Crew = dbl_result_Crew;
                rep._ProdMonth = dbl_result_ProdMonth;
                rep._Section = dbl_result_Section;
                rep._Activity = Activity;
                rep.theSystemDBTag = this.theSystemDBTag;
                rep.UserCurrentInfo = this.UserCurrentInfo;

                frm_Cap_Sheets.Text = "Crew Department";
                frm_Cap_Sheets.Controls.Clear();
                frm_Cap_Sheets.Controls.Add(rep);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                //frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");

                frm_Cap_Sheets.ShowDialog();
            }

            if (colCaption == "Safety")
            {
                if (!SafetyDep)
                {
                    MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                    return;
                }

                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Safety";
                Pre_PlanningFrm._Activity = Activity;
                try
                {
                    //frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { }

                frm_Cap_Sheets.Text = "Safety Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
                LoadPreplanning();

            }

            if (colCaption == "Rock Eng")
            {
                if (!RockEngDep)
                {
                    MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                    return;
                }

                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "RockEng";
                Pre_PlanningFrm._Activity = Activity;
                try
                {
                    //frm_Cap_Sheets.Icon = new Mineware.Systems.Production.Properties.Resources.SM;
                }
                catch
                {

                }
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.Text = "Rock Eng Department";
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
                LoadPreplanning();
            }

            if (colCaption == "Occ & Env")
            {
                if (!VentDep)
                {
                    MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                    return;
                }

                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Vent";
                Pre_PlanningFrm._Activity = Activity;
                try
                {
                    //frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { }

                frm_Cap_Sheets.Text = "Occ & Env Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
                LoadPreplanning();
            }

            if (colCaption == "Survey")
            {
                if (!SurveyDep)
                {
                    MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                    return;
                }

                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Survey";
                Pre_PlanningFrm._Activity = Activity;
                try
                {
                    //frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { }

                frm_Cap_Sheets.Text = "Survey Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
                LoadPreplanning();
            }

            if (colCaption == "GeoScience")
            {
                if (!GeologyDep)
                {
                    MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                    return;
                }

                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Geology";
                Pre_PlanningFrm._Activity = Activity;
                try
                {
                    //frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { }
                frm_Cap_Sheets.Text = "Geo-Science Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
                LoadPreplanning();
            }

            if (colCaption == "Engineering")
            {
                if (!EngDep)
                {
                    MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                    return;
                }

                ucGraphicsPreplanning Pre_PlanningFrm = new ucGraphicsPreplanning();
                Pre_PlanningFrm.Dock = DockStyle.Fill;
                Pre_PlanningFrm.BringToFront();
                Pre_PlanningFrm._Crew = dbl_result_Crew;
                Pre_PlanningFrm._Prodmonth = dbl_result_ProdMonth;
                Pre_PlanningFrm.tbSection.EditValue = dbl_result_Section;
                Pre_PlanningFrm.theSystemDBTag = this.theSystemDBTag;
                Pre_PlanningFrm.UserCurrentInfo = this.UserCurrentInfo;
                Pre_PlanningFrm._FormType = "Engineering";
                Pre_PlanningFrm._Activity = Activity;
                try
                {
                    //frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { }
                frm_Cap_Sheets.Text = "Engineering Department";
                frm_Cap_Sheets.Controls.Add(Pre_PlanningFrm);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
                LoadPreplanning();

            }

            if (colCaption == "HR")
            {
                if (!HRDep)
                {
                    MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                    return;
                }

                ucGraphicsPrePlanningHR HR = new ucGraphicsPrePlanningHR();
                HR.Dock = DockStyle.Fill;
                HR.BringToFront();
                HR.dbl_rec_Crew = dbl_result_Crew;
                HR.dbl_rec_ProdMonth = dbl_result_ProdMonth;
                HR.dbl_rec_Section = dbl_result_Section;
                HR.theSystemDBTag = this.theSystemDBTag;
                HR.UserCurrentInfo = this.UserCurrentInfo;
                //Eng.AllowAuth = dtGridSetup.Rows[0]["EngDepAuth"].ToString();
                try
                {
                    //frm_Cap_Sheets.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { }
                frm_Cap_Sheets.Text = "HR Department";
                frm_Cap_Sheets.Controls.Add(HR);
                frm_Cap_Sheets.WindowState = FormWindowState.Maximized;
                frm_Cap_Sheets.ShowDialog();
                LoadPreplanning();
            }

        }

        private void gvUser_DoubleClick(object sender, EventArgs e)
        {
            UserID = gvUser.GetRowCellValue(gvUser.FocusedRowHandle, gvUser.Columns["UserID"]).ToString();

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan.SqlStatement = string.Empty;

            _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_PrePlanning_UserSetup  VALUES ( \r\n ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + UserID + "',  'N',  'N', 'N', 'N',   ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " 'N', 'N', 'N', 'N', 'N',\r\n";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "  'N', 'N', 'N',   ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " 'N', 'N', 'N', 'N')";


            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            Global.sysNotification.TsysNotification.showNotification("Data Saved", "User Inserted", Color.CornflowerBlue);

            LoadPrePlanUsers();
            LoadUsers();
        }

        private void gvDepSetup_DoubleClick(object sender, EventArgs e)
        {
            if (gvDepSetup.FocusedColumn.FieldName == "UserID" || gvDepSetup.FocusedColumn.FieldName == "UserName")
            {
                string useraa = gvDepSetup.GetRowCellValue(gvDepSetup.FocusedRowHandle, gvDepSetup.Columns["UserName"]).ToString();
                string User = gvDepSetup.GetRowCellValue(gvDepSetup.FocusedRowHandle, gvDepSetup.Columns["UserID"]).ToString();

                if (MessageBox.Show("Are you sure you want to delete the '" + useraa + "'?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    MWDataManager.clsDataAccess _dbManEngEquiptment = new MWDataManager.clsDataAccess();
                    _dbManEngEquiptment.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                    _dbManEngEquiptment.SqlStatement = " delete from tbl_PrePlanning_UserSetup where UserID = '" + User + "' ";
                    _dbManEngEquiptment.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManEngEquiptment.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManEngEquiptment.ExecuteInstruction();
                }

                LoadPrePlanUsers();
                LoadUsers();
            }


            string rights = "Y";
            if (gvDepSetup.FocusedColumn.FieldName != "UserName" && gvDepSetup.FocusedColumn.FieldName != "UserID")
            {

                if (gvDepSetup.GetRowCellValue(gvDepSetup.FocusedRowHandle, gvDepSetup.Columns[gvDepSetup.FocusedColumn.FieldName]).ToString() == "Y")
                {
                    rights = "N";
                }
                if (gvDepSetup.GetRowCellValue(gvDepSetup.FocusedRowHandle, gvDepSetup.Columns[gvDepSetup.FocusedColumn.FieldName]).ToString() == "N")
                {
                    rights = "Y";
                }
                gvDepSetup.SetRowCellValue(gvDepSetup.FocusedRowHandle, gvDepSetup.Columns[gvDepSetup.FocusedColumn.FieldName], rights);
                SaveDepSetup();
            }
        }

        private void gvDepSetup_RowCellClick(object sender, RowCellClickEventArgs e)
        {

        }

        private void gvDepSetup_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                if ((e.Column.Name == "gcSafety") || (e.Column.Name == "gcRockEng") || (e.Column.Name == "gcVentilation") || (e.Column.Name == "gcCosting") || (e.Column.Name == "gcHR") || (e.Column.Name == "gcSurvey")
                    || (e.Column.Name == "gcGeology") || (e.Column.Name == "gcEngineering") ||
                    (e.Column.Name == "gcSafetyAuth") || (e.Column.Name == "gcRockEngAuth") || (e.Column.Name == "gcVentilationAuth") || (e.Column.Name == "gcCostingAuth") || (e.Column.Name == "gcHRAuth") || (e.Column.Name == "gcSurveyAuth")
                    || (e.Column.Name == "gcGeologyAuth") || (e.Column.Name == "gcEngineeringAuth"))
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);
                    e.Handled = true;
                }
            }
        }

        private void gvStartupMain_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null)
            {
                return;
            }

            if (e.Column.FieldName == "GeologyDept" || e.Column.FieldName == "PlanningDept" || e.Column.FieldName == "MiningDept" ||
                e.Column.FieldName == "RockEngDept" || e.Column.FieldName == "SafetyDept" || e.Column.FieldName == "SurveyDept" ||
                e.Column.FieldName == "VentilationDept" || e.Column.FieldName == "DepartmentDept")
            {
                e.Info.Caption = string.Empty;
                e.Painter.DrawObject(e.Info);

                e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds,
                new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);

                e.Handled = true;
            }
        }

        private void gvStartupMain_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "GeologyDept" || e.Column.FieldName == "PlanningDept" || e.Column.FieldName == "MiningDept" ||
                e.Column.FieldName == "RockEngDept" || e.Column.FieldName == "SafetyDept" || e.Column.FieldName == "SurveyDept" ||
                e.Column.FieldName == "VentilationDept" || e.Column.FieldName == "DepartmentDept")
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                e.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            }
        }

        private void gvStartupMain_DoubleClick(object sender, EventArgs e)
        {
            string myWP = gvStartupMain.GetRowCellValue(gvStartupMain.FocusedRowHandle, gvStartupMain.Columns["WorkplaceDesc"]).ToString();
            string WorkplaceID = gvStartupMain.GetRowCellValue(gvStartupMain.FocusedRowHandle, gvStartupMain.Columns["WorkplaceID"]).ToString();
            string TheDate = gvStartupMain.GetRowCellValue(gvStartupMain.FocusedRowHandle, gvStartupMain.Columns["TheDate"]).ToString();
            string TheActivity = gvStartupMain.GetRowCellValue(gvStartupMain.FocusedRowHandle, gvStartupMain.Columns["WPActivity"]).ToString();

            colCaption = gvStartupMain.FocusedColumn.Caption;

            Form ucDept_Capt = new Form();
            ucDept_Capt.Controls.Clear();

            if (!SafetyStartUp)
            {
                MessageBox.Show("You don't Have rights to Capture,Please Contact you Administrator");
                return;
            }

            ucStartupCapture sfDept = new ucStartupCapture();
            //sfDept._FormType = colCaption.Trim(new Char[] {' ', '*', '.', '@' });
            if (colCaption == "Rock Eng")
            {
                sfDept._FormType = "RockEng";
            }
            else
            {
                sfDept._FormType = colCaption.Trim(new Char[] { ' ', '*', '.', '@' });
            }

            if (colCaption == "Vent")
            {
                sfDept._FormType = "Ventilation";
            }


            sfDept._Activity = TheActivity;
            sfDept._WP = myWP;
            sfDept._WPID = WorkplaceID;
            sfDept.txtStartupWorkplace.EditValue = myWP;
            sfDept._Section = MOSection.EditValue.ToString();
            sfDept._ProdMonth = barProdMonth.EditValue.ToString();
            sfDept.txtStartupCalDate.EditValue = TheDate;
            sfDept.Dock = DockStyle.Fill;
            sfDept.theSystemDBTag = this.theSystemDBTag;
            sfDept.UserCurrentInfo = this.UserCurrentInfo;

            ucDept_Capt.Controls.Add(sfDept);
            ucDept_Capt.WindowState = FormWindowState.Maximized;
            ucDept_Capt.Text = colCaption + " Departmental Capture";
            //ucDept_Capt.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");

            ucDept_Capt.ShowDialog();


            LoadStartupData(Prodmonth, MOSection.EditValue.ToString());

            if (colCaption == "Workplace")
            {
                ucStartupsReport startUps = new ucStartupsReport();
                startUps._DepartmentName = "All";
                startUps._prodMonth = String.Format("{0:yyyy-MM-dd}", TheDate);
                startUps._ucWorkPlace = myWP;
                startUps._myActivity = TheActivity;
                startUps.theSystemDBTag = theSystemDBTag;
                startUps.UserCurrentInfo = UserCurrentInfo;

                Form ucDept_Report = new Form();
                ucDept_Report.Controls.Clear();
                ucDept_Report.WindowState = FormWindowState.Maximized;
                ucDept_Report.Controls.Add(startUps);
                ucDept_Report.WindowState = FormWindowState.Maximized;
                startUps.Dock = DockStyle.Fill;
                ucDept_Report.Text = "All Startup Department";
                try
                {
                    // ucDept_Report.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
                }
                catch { }

                ucDept_Report.ShowDialog();
            }
        }


        #endregion Events

        #region Methods
        public string ExtractBeforeColon(string TheString)
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

        private void LoadUsers()
        {
            MWDataManager.clsDataAccess _LoadUsers = new MWDataManager.clsDataAccess();
            _LoadUsers.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadUsers.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadUsers.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadUsers.SqlStatement = " Select UserID, Name, LastName From [Syncromine_New].[dbo].tblUsers Where UserID not in ( Select UserID From tbl_PrePlanning_UserSetup ) Order By Name";

            _LoadUsers.ExecuteInstruction();

            colUserID.FieldName = "UserID";
            colName.FieldName = "Name";
            colSurname.FieldName = "LastName";

            //If running on seperate thread then invoke
            if (gcUser.InvokeRequired)
            {
                gcUser.Invoke((Action)delegate { gcUser.DataSource = _LoadUsers.ResultsDataTable; });
            }
            else
            {
                gcUser.DataSource = _LoadUsers.ResultsDataTable;
            }


            System.Threading.Tasks.Task.Run(() => LoadPrePlanUsers());
        }

        private void LoadSections()
        {
            MWDataManager.clsDataAccess _PrePlanningLoadSections = new MWDataManager.clsDataAccess();
            _PrePlanningLoadSections.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _PrePlanningLoadSections.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningLoadSections.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningLoadSections.SqlStatement = " Select moid Sectionid_2,moname Name_2 \r\n" +
                                                    "from [dbo].[tbl_sectioncomplete] \r\n" +
                                                    "where prodmonth = '" + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue.ToString())) + "' \r\n " +
                                                    " Group By moid,moname \r\n " +
                                                    " Order By moid,moname";
            _PrePlanningLoadSections.ExecuteInstruction();

            DataTable tbl_Sections = _PrePlanningLoadSections.ResultsDataTable;

            if (tbl_Sections.Rows.Count > 0)
            {

                MOSections.DataSource = tbl_Sections;
                MOSections.DisplayMember = "Name_2";
                MOSections.ValueMember = "Sectionid_2";

                MOSection.EditValue = tbl_Sections.Rows[0]["Sectionid_2"].ToString();
            }
        }

        private void InsertPreplanMonth()
        {
            MWDataManager.clsDataAccess _dbManSave8 = new MWDataManager.clsDataAccess();
            _dbManSave8.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbManSave8.SqlStatement = string.Empty;
            _dbManSave8.SqlStatement = _dbManSave8.SqlStatement + "  \r\n";

            for (int row = 0; row < gvPrePlanMainGraphics.RowCount; row++)
            {
                string Crew = gvPrePlanMainGraphics.GetRowCellValue(row, gvPrePlanMainGraphics.Columns["Crew"]).ToString();

                if (!string.IsNullOrEmpty(Crew))
                {
                    _dbManSave8.SqlStatement = _dbManSave8.SqlStatement + " begin try insert into tbl_PrePlanning_MonthPlan values( \r\n" +
                        "  " + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue.ToString())) + ", \r\n" +
                        " '" + MOSection.EditValue.ToString() + "', \r\n" +
                        " '" + Crew + "', \r\n" +
                        " '',  '',  '',  '',  '',  '',  '',  '',  '',  '', \r\n" +
                        " '',  '',  '', '', '',  '',  '',  '',  '',  '',  '',  '',  '',  '' \r\n" +
                        "  ) end try \r\n" +
                        " begin catch \r\n" +

                        "   end catch \r\n";
                }
            }

            _dbManSave8.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSave8.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSave8.ExecuteInstruction();
        }

        private void LoadPreplanning()
        {
            MWDataManager.clsDataAccess _PrePlanningLoadGrid = new MWDataManager.clsDataAccess();
            _PrePlanningLoadGrid.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _PrePlanningLoadGrid.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningLoadGrid.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningLoadGrid.SqlStatement = " Exec sp_PrePlanning_Load_Main " + ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue.ToString())) + ",'" + MOSection.EditValue + "' ";

            _PrePlanningLoadGrid.ExecuteInstruction();

            DataTable tbl_LoadData = _PrePlanningLoadGrid.ResultsDataTable;

            if (tbl_LoadData.Rows.Count > 0)
            {
                if (gcPrePlanMainGraphics.InvokeRequired)
                {
                    gcPrePlanMainGraphics.Invoke((Action)delegate { gcPrePlanMainGraphics.DataSource = tbl_LoadData; });
                }
                else
                {
                    gcPrePlanMainGraphics.DataSource = tbl_LoadData;
                }
            }
            else
            {
                if (MOSection.EditValue != null)
                {
                    if (gcPrePlanMainGraphics.InvokeRequired)
                    {
                        gcPrePlanMainGraphics.Invoke((Action)delegate { gcPrePlanMainGraphics.DataSource = null; });
                    }
                    else
                    {
                        gcPrePlanMainGraphics.DataSource = null;
                    }

                    MessageBox.Show("No Planning for Section " + MOSection.EditValue.ToString() + " ");
                }
            }

        }

        void LoadPrePlanUsers()
        {
            //xtraTabControl1.TabPages.RemoveAt(1);

            return;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbMan.SqlStatement = " \r\n" +
                                "  Select a.*,  \r\n" +
                                " isnull(b.SafetyDep,'N')SafetyDep , \r\n" +
                                " isnull(b.RockEngDep,'N')RockEngDep,  \r\n" +
                                " isnull(b.VentDep,'N')VentDep , \r\n" +
                                " isnull(b.CostingDep,'N')CostingDep , \r\n" +
                                " isnull(b.HRDep,'N')HRDep , \r\n" +
                                " isnull(b.SurveyDep,'N')SurveyDep , \r\n" +
                                " isnull(b.GeologyDep,'N')GeologyDep , \r\n" +
                                " isnull(b.EngDep,'N')EngDep,  \r\n" +

                                " isnull(b.SafetyDepAuth ,'N')SafetyDepAuth , \r\n" +
                                " isnull(b.RockEngDepAuth ,'N')RockEngDepAuth ,  \r\n" +
                                " isnull(b.VentDepAuth ,'N')VentDepAuth  , \r\n" +
                                " isnull(b.CostingDepAuth ,'N')CostingDepAuth  , \r\n" +
                                " isnull(b.HRDepAuth ,'N')HRDepAuth  , \r\n" +
                                " isnull(b.SurveyDepAuth ,'N')SurveyDepAuth  , \r\n" +
                                " isnull(b.GeologyDepAuth ,'N')GeologyDepAuth  , \r\n" +
                                " isnull(b.EngDepAuth ,'N')EngDepAuth   \r\n" +

                                " from ( \r\n" +
                                " select UserID,Name UserName from [Syncromine_New].[dbo].tblUsers)a \r\n" +
                                " left outer join ( \r\n" +
                                " select * from [tbl_PrePlanning_UserSetup])b \r\n" +
                                "  on a.UserID = b.UserID \r\n" +
                                    " where b.UserID is not null order by username ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            gcUserID.FieldName = "UserID";
            gcUserName.FieldName = "UserName";
            gcSafety.FieldName = "SafetyDep";
            gcRockEng.FieldName = "RockEngDep";
            gcVentilation.FieldName = "VentDep";
            gcHR.FieldName = "HRDep";
            gcSurvey.FieldName = "SurveyDep";
            gcGeology.FieldName = "GeologyDep";
            gcEngineering.FieldName = "EngDep";

            gcSafetyAuth.FieldName = "SafetyDepAuth";
            gcRockEngAuth.FieldName = "RockEngDepAuth";
            gcVentilationAuth.FieldName = "VentDepAuth";
            gcHRAuth.FieldName = "HRDepAuth";
            gcSurveyAuth.FieldName = "SurveyDepAuth";
            gcGeologyAuth.FieldName = "GeologyDepAuth";
            gcEngineeringAuth.FieldName = "EngDepAuth";

            if (gcDepSetup.InvokeRequired)
            {
                gcDepSetup.Invoke((Action)delegate { gcDepSetup.DataSource = _dbMan.ResultsDataTable; });
            }
            else
            {
                gcDepSetup.DataSource = _dbMan.ResultsDataTable;
            }

            foreach (DataRow dr in _dbMan.ResultsDataTable.Rows)
            {
                if (TUserInfo.UserID == dr["UserID"].ToString())
                {
                    HasPreplanRights = "Y";
                }
            }

        }

        private void SaveDepSetup()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);

            _dbMan.SqlStatement = "delete from tbl_PrePlanning_UserSetup";
            bool aa1 = Convert.ToBoolean(1);

            for (int i = 0; i < gvDepSetup.RowCount; i++)
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_PrePlanning_UserSetup  VALUES ( \r\n ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvDepSetup.GetRowCellValue(i, "UserID").ToString() + "',  '" + gvDepSetup.GetRowCellValue(i, "SafetyDep").ToString() + "',\r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "  '" + gvDepSetup.GetRowCellValue(i, "RockEngDep").ToString() + "', \r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvDepSetup.GetRowCellValue(i, "VentDep").ToString() + "', '" + gvDepSetup.GetRowCellValue(i, "CostingDep").ToString() + "',   ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvDepSetup.GetRowCellValue(i, "HRDep").ToString() + "', '" + gvDepSetup.GetRowCellValue(i, "SurveyDep").ToString() + "', '" + gvDepSetup.GetRowCellValue(i, "GeologyDep").ToString() + "', '" + gvDepSetup.GetRowCellValue(i, "EngDep").ToString() + "',  '" + gvDepSetup.GetRowCellValue(i, "SafetyDepAuth").ToString() + "',\r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "  '" + gvDepSetup.GetRowCellValue(i, "RockEngDepAuth").ToString() + "', \r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvDepSetup.GetRowCellValue(i, "VentDepAuth").ToString() + "', '" + gvDepSetup.GetRowCellValue(i, "CostingDepAuth").ToString() + "',   ";
                _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvDepSetup.GetRowCellValue(i, "HRDepAuth").ToString() + "', '" + gvDepSetup.GetRowCellValue(i, "SurveyDepAuth").ToString() + "', '" + gvDepSetup.GetRowCellValue(i, "GeologyDepAuth").ToString() + "', '" + gvDepSetup.GetRowCellValue(i, "EngDepAuth").ToString() + "' )";

            }
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
        }

        private void GetDepSetup()
        {
            SafetyDep = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.SafetyDep);
            RockEngDep = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.RockEngDep);
            VentDep = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.VentDep);
            CostingDep = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CostingDep);
            HRDep = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.HRDep);
            SurveyDep = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.SurveyDep);
            GeologyDep = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.GeologyDep);
            EngDep = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.EngDep);

            SafetyDepAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.SafetyDepAuth);
            RockEngDepAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.RockEngDepAuth);
            VentDepAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.VentDepAuth);
            CostingDepAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CostingDepAuth);
            HRDepAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.HRDepAuth);
            SurveyDepAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.SurveyDepAuth);
            GeologyDepAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.GeologyDepAuth);
            EngDepAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.EngDepAuth);

            SafetyStartUp = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.SafetyStartUp);
            RockEngStartUp = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.RockEngStartUp);
            PlanningStartUp = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.PlanningStartUp);
            SurveyStartUp = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.SurveyStartUp);
            GeologyStartUp = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.GeologyStartUp);
            MiningStartup = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.MiningStartup);
            VentStartUp = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.VentStartUp);
            DepartmentStartUp = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.DepartmentStartUp);

            SafetyStartUpAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.SurveyDepAuth);
            RockEngStartUpAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.RockEngStartUpAuth);
            PlanningStartUpAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.PlanningStartUpAuth);
            SurveyStartUpAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.SurveyStartUpAuth);
            GeologyStartUpAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.GeologyStartUpAuth);
            MiningStartupAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.MiningStartupAuth);
            VentStartUpAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.VentStartUpAuth);
            DepartmentStartUpAuth = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.DepartmentStartUpAuth);

            OreflowDesign = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.OreflowDesign);
            OreflowBackfill = Security.SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.OreflowBackfill);
        }

        private void LoadStartupData(DateTime ProdmonthEdit, string SectionidEdit)
        {
            MWDataManager.clsDataAccess _dbStartMain = new MWDataManager.clsDataAccess();
            _dbStartMain.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbStartMain.SqlStatement = "exec [sp_Startup_Main_Section] '"+procs.ProdMonthAsString(Prodmonth)+"', '"+SectionidEdit+"'  \r\n ";

            _dbStartMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbStartMain.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbStartMain.ExecuteInstruction();


            if (gcStartupMain.InvokeRequired)
            {
                gcStartupMain.Invoke((Action)delegate { gcStartupMain.DataSource = _dbStartMain.ResultsDataTable; });
            }
            else
            {
                gcStartupMain.DataSource = _dbStartMain.ResultsDataTable;
            }
        }
        #endregion Methods

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void gvPrePlanMainGraphics_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Answer")
                {
                    if (gvPrePlanMainGraphics.GetRowCellValue(e.RowHandle, "Answer").ToString() == "0")
                    {
                        e.Appearance.ForeColor = Color.Transparent;
                    }
                }
            }
            catch { }
        }

        private void barbtnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmPreplanningSummary frmadd = new frmPreplanningSummary();
            frmadd.WindowState = FormWindowState.Maximized;
            frmadd._Prodmonth = ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue.ToString())).ToString();
            frmadd._Mosect = MOSection.EditValue.ToString();
            frmadd._systemDBTag = this.theSystemDBTag;
            frmadd._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            frmadd._Report = "Summary";
            frmadd.StartPosition = FormStartPosition.CenterScreen;

            try
            {
                //frmadd.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            }
            catch (Exception)
            {

            }
            frmadd.ShowDialog();
        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWordEditor helpFrm = new frmWordEditor();
            helpFrm.ViewType = "View";
            helpFrm.MainCat = "PrePlanning";            
            helpFrm.SubCat = "PrePlanning";            
            helpFrm.Show();
        }

        private void btnCompReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            PillarCompliance Report = new PillarCompliance();
            Report._Connection = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            Report._Prodmonth = procs.ProdMonthAsString(Convert.ToDateTime(barProdMonth.EditValue));
            Report._Mosect = MOSection.EditValue.ToString();
            Report._MosectName = MOSections.GetDisplayText(MOSection.EditValue.ToString());
            ReportPrintTool tool = new ReportPrintTool(Report);
            tool.ShowPreview();

            //HighRisk Report = new HighRisk();
            //ReportPrintTool tool = new ReportPrintTool(Report);
            //tool.ShowPreview();

            return;

            frmPreplanningSummary frmadd = new frmPreplanningSummary();
            frmadd.WindowState = FormWindowState.Maximized;
            frmadd._Prodmonth = ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue.ToString())).ToString();
            frmadd._Mosect = MOSection.EditValue.ToString();
            frmadd._systemDBTag = this.theSystemDBTag;
            frmadd._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            frmadd._Report = "RE Comp";
            frmadd.StartPosition = FormStartPosition.CenterScreen;

            try
            {
                //frmadd.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            }
            catch (Exception)
            {

            }
            frmadd.ShowDialog();
        }

        private void btnHighRisk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmPreplanningSummary frmadd = new frmPreplanningSummary();
            frmadd.WindowState = FormWindowState.Maximized;
            frmadd._Prodmonth = ProductionGlobal.ProductionGlobal.ProdMonthAsInt(Convert.ToDateTime(barProdMonth.EditValue.ToString())).ToString();
            frmadd._Mosect = MOSection.EditValue.ToString();
            frmadd._systemDBTag = this.theSystemDBTag;
            frmadd._UserCurrentInfoConnection = this.UserCurrentInfo.Connection;
            frmadd._Report = "High Risk";
            frmadd.StartPosition = FormStartPosition.CenterScreen;

            try
            {
                //frmadd.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            }
            catch (Exception)
            {

            }
            frmadd.ShowDialog();
        }

        private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadPreplanning();
            InsertPreplanMonth();
            LoadStartupData(Prodmonth, MOSection.EditValue.ToString());
            //System.Threading.Tasks.Task.Run(() => LoadPreplanning());
            //System.Threading.Tasks.Task.Run(() => InsertPreplanMonth());
            //System.Threading.Tasks.Task.Run(() => LoadStartupData(SelectProdmonth, MOSection.EditValue.ToString()));
            //ShowProgressPage(false);
        }
    }
}
