using DevExpress.XtraEditors.Repository;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
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

namespace Mineware.Systems.Production.Planning
{
    public partial class ucGraphicsPrePlanningHR : BaseUserControl
    {
        private DataTable dtActions = new DataTable("dtActions");
        private DataTable dtWorkplaceData = new DataTable("dtWorkplaceData");

        StringBuilder SQLQuery = new StringBuilder();
        public DataSet dsGlobal = new DataSet();

        public String dbl_rec_Section;
        public String dbl_rec_Crew;
        public String dbl_rec_ProdMonth;

        private String sourceFile;
        private String destinationFile;
        private String FileName = string.Empty;
        private String FileName2 = string.Empty;


        private String ID = string.Empty;
        private String Workplace = string.Empty;
        private String Description = string.Empty;
        private String Recomendation = string.Empty;
        private String Priority = string.Empty;
        private String TargetDate = string.Empty;
        private String RespPerson = string.Empty;
        private String Overseer = string.Empty;


        public String _FormType;
        string ActionType;
        string ActionDescription;

        private String WP1 = string.Empty;
        private String WP2 = string.Empty;
        private String WP3 = string.Empty;
        private String WP4 = string.Empty;
        private String WP5 = string.Empty;
        private String WP1Desc = string.Empty;
        private String WP2Desc = string.Empty;
        private String WP3Desc = string.Empty;
        private String WP4Desc = string.Empty;
        private String WP5Desc = string.Empty;

        DataTable dtCrewType = new DataTable();

        ProductionGlobal.ProductionGlobal procs = new ProductionGlobal.ProductionGlobal();

        DialogResult result1;
        string RepDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDir;

        #region Private Methods
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
        #endregion

        public ucGraphicsPrePlanningHR()
        {
            InitializeComponent();
        }

        private void sqlConnector(string sqlQuery, string sqlTableIdentifier)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _sqlConnection.SqlStatement = sqlQuery;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ResultsTableName = sqlTableIdentifier;
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;


            if (!string.IsNullOrEmpty(sqlTableIdentifier))
            {
                for (int i = 0; i < dsGlobal.Tables.Count; i++)
                {
                    if (dsGlobal.Tables[i].TableName == sqlTableIdentifier)
                    {
                        dsGlobal.Tables[i].Clear();
                        dsGlobal.Tables[i].Merge(dtReceive);
                    }
                }
            }
        }

        //NOTE: Register all tables for sqlconnector to use
        private void tableRegister()
        {
           
            dsGlobal.Tables.Add(dtActions);
            dsGlobal.Tables.Add(dtWorkplaceData);

        }

        private void ucGraphicsPrePlanningHR_Load(object sender, EventArgs e)
        {
            tbCrew.EditValue = dbl_rec_Crew;
            tbSection.EditValue = dbl_rec_Section;
            tbProdMonth.EditValue = dbl_rec_ProdMonth;

            ActionType = "PPHR";
            ActionDescription = "Labour Actions";

            LoadCycles();
            LoadHR();
            loadImage();
            loadDocs();
            LoadWorkplaces();
            LoadActions();

            LoadTypes();
            LoadLabourCompliance();


        }

        void LoadLabourCompliance()
        {
            MWDataManager.clsDataAccess _data = new MWDataManager.clsDataAccess();
            _data.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _data.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _data.queryReturnType = MWDataManager.ReturnType.DataTable;
            _data.SqlStatement = " \r\n " +
                                " Select nn,job_title,personal_grade,Plan1,Act, convert(int, Var1) Var1 from( \r\n " +
                                " select '1' nn, job_title, personal_grade,isnull(max(PlanLab), 0) Plan1, isnull(sum(num), 0) Act,convert(varchar(20), isnull(sum(num), 0) - isnull(max(PlanLab), 0))  Var1 from( \r\n " +
                                " select* from( \r\n " +
                                " select Distinct 1 num,EmployeeNo employee_number, EmployeeName surname_and_Initials,  \r\n " +
                                " rtrim(WorkGroupCode) job_title,null age_category,null service_category,  \r\n " +
                                " null leave_due_Date, null MedicalDate, null personal_grade \r\n " +
                                " from tbl_EmployeeAll \r\n " +
                                " where  GangNo = '" + dbl_rec_Crew + "') a \r\n " +
                                " left outer join \r\n " +
                                " (SELECT Designation, PlanLab \r\n " +
                                " FROM vw_Preplanning_Labour_Count \r\n " +
                                " where HRStdNormID in ('" + listBox.SelectedValue + "') ) b on a.job_title = b.Designation \r\n " +
                                " ) a \r\n " +
                                " group by job_title, personal_grade \r\n " +
                                " union \r\n " +
                                " select '2' nn, 'Total' job_title, '' personal_grade, sum(Plan1) Plan1, sum(Act) Act, sum(var1) var1 from( \r\n " +
                                " select job_title, personal_grade, isnull(max(PlanLab), 0) Plan1, isnull(sum(num), 0) Act,isnull(sum(num), 0) - isnull(max(PlanLab), 0) var1 from( \r\n " +
                                " select* from( \r\n " +
                                " select Distinct 1 num,EmployeeNo employee_number, EmployeeName surname_and_Initials,  \r\n " +
                                " rtrim(WorkGroupCode) job_title, null age_category, null service_category,  \r\n " +
                                " null leave_due_Date,null MedicalDate, null personal_grade \r\n " +
                                " from tbl_EmployeeAll \r\n " +
                                " where GangNo = '" + dbl_rec_Crew + "') a \r\n " +
                                " left outer join \r\n " +
                                " (SELECT Designation, PlanLab \r\n " +
                                " FROM vw_Preplanning_Labour_Count \r\n " +
                                " where HRStdNormID in ('" + listBox.SelectedValue + "')) b on a.job_title = b.Designation \r\n " +
                                " ) a \r\n " +
                                " group by job_title, personal_grade) a \r\n " +
                                " ) a order by nn, personal_grade desc \r\n ";          


            _data.ExecuteInstruction();
            gcCrewComp.DataSource = _data.ResultsDataTable;
            colJobTitle.FieldName = "job_title";
            colPlan.FieldName = "Plan1";
            colAct.FieldName = "Act";
            colVar.FieldName = "Var1";
        }

        void LoadTypes()
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _sqlConnection.SqlStatement = " SELECT Distinct GroupName FROM [tbl_HR_Desigantion] \r\n" +
                                            "  " +
                                            "\r\n" +                
                                            " \r\n" +
                                            "";
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();
            
            dtCrewType = _sqlConnection.ResultsDataTable;

            listBox.DataSource = dtCrewType;
            listBox.ValueMember = "GroupName";
            listBox.DisplayMember = "GroupName";
        }

        private void LoadWorkplaces()
        {
            string sql = string.Empty;
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _sqlConnection.SqlStatement = "SELECT pm.Workplaceid, w.Description, ''LastInspection " +
                    "FROM tbl_PLANMONTH AS pm, " +
                    "tbl_Workplace w \r\n" +
                    "WHERE w.WORKPLACEID = pm.Workplaceid " +
                    "AND pm.Prodmonth = " + tbProdMonth.EditValue + " " +
                    "AND substring( pm.OrgUnitDS+'                ',1,12) = '" + tbCrew.EditValue + "' \r\n" +
                    " union " +
                    "SELECT pm.Workplaceid, w.Description, ''LastInspection " +
                    "FROM tbl_PLANMONTH_MOScrutiny AS pm, " +
                    "tbl_Workplace w \r\n" +
                    "WHERE w.WORKPLACEID = pm.Workplaceid " +
                    "AND pm.Prodmonth = " + tbProdMonth.EditValue + " " +
                    "AND substring( pm.OrgUnitDS+'                ',1,12) = '" + tbCrew.EditValue + "' \r\n" +
                    " and pm.Workplaceid not in ( \r\n" +
                     "SELECT pm.Workplaceid " +
                    "FROM tbl_PLANMONTH AS pm, " +
                    "tbl_Workplace w \r\n" +
                    "WHERE w.WORKPLACEID = pm.Workplaceid " +
                    "AND pm.Prodmonth = " + tbProdMonth.EditValue + " " +
                    "AND substring( pm.OrgUnitDS+'                ',1,12) = '" + tbCrew.EditValue + "' \r\n" +
                     " ) \r\n" +

                    "ORDER BY w.DESCRIPTION ";
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;            
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;
           


            // do 
            int x = 0;

            

            foreach (DataRow r in dtReceive.Rows)
            {
                if (x == 0)
                {
                  
                    WP1 = r["Workplaceid"].ToString();
                    WP1Desc = r["Description"].ToString();
                }

                if (x == 1)
                {
                   
                    WP2 = r["Workplaceid"].ToString();
                    WP2Desc = r["Description"].ToString();
                }

                if (x == 2)
                {
                  
                    WP3 = r["Workplaceid"].ToString();
                    WP3Desc = r["Description"].ToString();
                }

                if (x == 3)
                {
                    
                    WP4 = r["Workplaceid"].ToString();
                    WP4Desc = r["Description"].ToString();
                }

                if (x == 4)
                {
                    
                    WP5 = r["Workplaceid"].ToString();
                    WP5Desc = r["Description"].ToString();
                }
                x = x + 1;
            }
        }



        void LoadHR()
        {
            MWDataManager.clsDataAccess _LoadHR = new MWDataManager.clsDataAccess();
            _LoadHR.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _LoadHR.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _LoadHR.queryReturnType = MWDataManager.ReturnType.DataTable;
            _LoadHR.SqlStatement = "select  e.EmployeeNo company_number, e.EmployeeNo employee_number, EmployeeName surname_and_Initials, rtrim(WorkGroupCode) job_title, '12' age_category, \r\n " +
                                    " 'Serv' service_category,  '2019-10-12' leave_due_Date, '2019-10-12' MedicalDate , EmployeePhoto \r\n " +
                                    "  --medical_certificate_exp_date \r\n " +
                                    "  from[tbl_EmployeeAll] e \r\n " +
                                    " left outer join tbl_EmployeeAll_Photo p on e.EmployeeNo = p.EmployeeNo \r\n " +
                                    "  where Substring(GangName,0,13) = '" + dbl_rec_Crew + "'\r\n ";

            _LoadHR.ExecuteInstruction();

            gcHR.DataSource = null;

            DataTable tbl_HR = _LoadHR.ResultsDataTable;
            DataSet dsHR = new DataSet();
            if (dsHR.Tables.Count > 0)
                dsHR.Tables.Clear();
            dsHR.Tables.Add(tbl_HR);

            gcHR.DataSource = dsHR.Tables[0];


            colCompNo.FieldName = "company_number";
            colIndNo.FieldName = "employee_number";
            colName.FieldName = "surname_and_Initials";
            colOcc.FieldName = "job_title";
            colShift.FieldName = string.Empty;
            colAge.FieldName = "age_category";
            colService.FieldName = "service_category";
            colLeaveDate.FieldName = "leave_due_Date";
            colMedicalDate.FieldName = "MedicalDate";

            colLeaveDate.Visible = false;
            colService.Visible = false;
            colMedicalDate.Visible = false;
            colAge.Visible = false;

            //RepositoryItemPictureEdit item = new RepositoryItemPictureEdit();
            //gcHR.RepositoryItems.Add(item);
            //gvHR.Columns["EmpPhoto"].ColumnEdit = item;

            colPhoto.FieldName = "EmployeePhoto";

            //colCompNo.FieldName = "company_number";
            //colIndNo.FieldName = "employee_number";
            //colName.FieldName = "surname_and_Initials";
            //colOcc.FieldName = "job_title";
            //colShift.FieldName = "";
            //colAge.FieldName = "age_category";
            //colService.FieldName = "service_category";
            //colLeaveDate.FieldName = "leave_due_Date";
            //colMedicalDate.FieldName = "MedicalDate";

        }

        void LoadCycles()
        {
            MWDataManager.clsDataAccess _PrePlanningLoadCycle = new MWDataManager.clsDataAccess();
            _PrePlanningLoadCycle.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _PrePlanningLoadCycle.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningLoadCycle.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningLoadCycle.SqlStatement = " Select * from vw_Preplanning_Cycle where orgunit = '" + dbl_rec_Crew + "' and Prodmonth = '" + dbl_rec_ProdMonth + "' \r\n" +
                                                 " order by OrderNum,description ";

            var result = _PrePlanningLoadCycle.ExecuteInstruction();

            gcCycle.DataSource = null;

            DataTable tbl_Cycle = _PrePlanningLoadCycle.ResultsDataTable;
            DataSet dsCycle = new DataSet();
            if (dsCycle.Tables.Count > 0)
                dsCycle.Tables.Clear();
            dsCycle.Tables.Add(tbl_Cycle);
            gcCycle.DataSource = dsCycle.Tables[0];

            colWorkplace.FieldName = "description";
            colFL.FieldName = "FL";
            col1.FieldName = "day1";
            col2.FieldName = "day2";
            col3.FieldName = "day3";
            col4.FieldName = "day4";
            col5.FieldName = "day5";
            col6.FieldName = "day6";
            col7.FieldName = "day7";
            col8.FieldName = "day8";
            col9.FieldName = "day9";
            col10.FieldName = "day10";

            col11.FieldName = "day11";
            col12.FieldName = "day12";
            col13.FieldName = "day13";
            col14.FieldName = "day14";
            col15.FieldName = "day15";
            col16.FieldName = "day16";
            col17.FieldName = "day17";
            col18.FieldName = "day18";
            col19.FieldName = "day19";
            col20.FieldName = "day20";

            col21.FieldName = "day21";
            col22.FieldName = "day22";
            col23.FieldName = "day23";
            col24.FieldName = "day24";
            col25.FieldName = "day25";
            col26.FieldName = "day26";
            col27.FieldName = "day27";
            col28.FieldName = "day28";
            col29.FieldName = "day29";
            col30.FieldName = "day30";

            col31.FieldName = "day31";
            col32.FieldName = "day32";
            col33.FieldName = "day33";
            col34.FieldName = "day34";
            col35.FieldName = "day35";
            col36.FieldName = "day36";
            col37.FieldName = "day37";
            col38.FieldName = "day38";
            col39.FieldName = "day39";
            col40.FieldName = "day40";

            if (tbl_Cycle.Rows.Count > 0)
            {
                DateTime startdate = Convert.ToDateTime(tbl_Cycle.Rows[0]["BeginDate"].ToString());
                int columnIndex = 2;

                //Headers Date
                for (int i = 0; i < 40; i++)
                {
                    string test = gvCycle.Columns[columnIndex].Caption;


                    gvCycle.Columns[columnIndex].Caption = Convert.ToDateTime(startdate).ToString("dd MMM ddd");

                    startdate = startdate.AddDays(1);
                    columnIndex++;
                }
            }


            for (int i = 0; i < gvCycle.RowCount; i++)
            {
                string val = gvCycle.GetRowCellValue(i, gvCycle.Columns["day29"]).ToString();

                foreach (DataRow item in tbl_Cycle.Rows)
                {
                    if (val == string.Empty)
                    {
                        col29.Visible = false;
                        col30.Visible = false;

                        col31.Visible = false;
                        col32.Visible = false;
                        col33.Visible = false;
                        col34.Visible = false;
                        col35.Visible = false;
                        col36.Visible = false;
                        col37.Visible = false;
                        col38.Visible = false;
                        col39.Visible = false;
                        col40.Visible = false;
                    }
                }

            }

            for (int i = 0; i < gvCycle.RowCount; i++)
            {
                int val1 = Convert.ToInt32(tbl_Cycle.Rows[0]["TotalShifts"].ToString());// gvCycle.GetRowCellValue(i, gvCycle.Columns["TotalShifts"]).ToString();



                for (int j = val1 + 3; j < 43; j++)
                {
                    gvCycle.Columns[j].Visible = false;
                }
            }
        }

        private void gvCycle_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.CellValue.ToString().Trim() == "BL")
            {
                e.Appearance.BackColor = Color.MistyRose;
            }

            if (e.CellValue.ToString().Trim() == "SR")
            {
                e.Appearance.BackColor = Color.MistyRose;
            }

            if (e.CellValue.ToString().Trim() == "SUBL")
            {
                e.Appearance.BackColor = Color.MistyRose;
            }

            if (e.CellValue.ToString().Trim() == "OFF")
            {
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Gainsboro;
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnAuth_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btnAuth.Caption == "Unauthorise")
            {
                btnImageRemove.Enabled = true;
                btnDocRemove.Enabled = true;

                btnAddImg.Enabled = true;
                btnAddDoc.Enabled = true;
                btnSave.Enabled = true;
                txtHRNotes.Enabled = true;
                AddActBtn.Enabled = true;
                EditActBtn.Enabled = true;
                DelActBtn.Enabled = true;

               
                btnAuth.Caption = "Authorise";

                MWDataManager.clsDataAccess _PrePlanningSafetyAuth = new MWDataManager.clsDataAccess();
                _PrePlanningSafetyAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _PrePlanningSafetyAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _PrePlanningSafetyAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _PrePlanningSafetyAuth.SqlStatement = " Update tbl_PrePlanning_MonthPlan Set HRDepCapt = '" + TUserInfo.UserID + "', HRDepAuth = ''  where prodmonth = '" + tbProdMonth.EditValue + "' and Crew = '" + tbCrew.EditValue + "' ";

                var result = _PrePlanningSafetyAuth.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Unauthorised", Color.CornflowerBlue);
                }

            }
            else
            {
                btnSave_ItemClick(null, null);

                btnImageRemove.Enabled = false;
                btnDocRemove.Enabled = false;

                

                btnSave.Enabled = false;
                btnAddImg.Enabled = false;
                btnAddDoc.Enabled = false;
                txtHRNotes.Enabled = false;
                AddActBtn.Enabled = false;
                EditActBtn.Enabled = false;
                DelActBtn.Enabled = false;

                

                btnAuth.Caption = "Unauthorise";

                MWDataManager.clsDataAccess _PrePlanningSafetyAuth = new MWDataManager.clsDataAccess();
                _PrePlanningSafetyAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
                _PrePlanningSafetyAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _PrePlanningSafetyAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _PrePlanningSafetyAuth.SqlStatement = " Update tbl_PrePlanning_MonthPlan Set HRDepAuth = '" + TUserInfo.UserID + "'  where prodmonth = '" + tbProdMonth.EditValue + "' and Crew = '" + tbCrew.EditValue + "' ";

                var result = _PrePlanningSafetyAuth.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Authorised", Color.CornflowerBlue);
                }

            }

            #region Incidents
            MWDataManager.clsDataAccess _PrePlanningIncidentsAuth = new MWDataManager.clsDataAccess();
            _PrePlanningIncidentsAuth.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _PrePlanningIncidentsAuth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _PrePlanningIncidentsAuth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _PrePlanningIncidentsAuth.SqlStatement = " exec sp_Incidents_Update  'PPHR' ";
            var result2 = _PrePlanningIncidentsAuth.ExecuteInstruction();
            #endregion
        }

        private void btnAddImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
            openFileDialog1.FileName = null;
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            result1 = openFileDialog1.ShowDialog();

            GetFile();
        }

        void GetFile()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\HR");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(RepDir + "\\Preplanning\\HR");

            if (result1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                int index = 0;

                sourceFile = openFileDialog1.FileName;

                index = openFileDialog1.SafeFileName.IndexOf(".");

                if (tbCrew.EditValue.ToString() != string.Empty)
                {
                    FileName = ProductionGlobal.ProductionGlobal.ExtractAfterColon(tbCrew.EditValue.ToString());
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (tbProdMonth.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + tbProdMonth.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog1.SafeFileName.Substring(index);

                destinationFile = RepDir + "\\Preplanning\\HR" + "\\" + FileName + Ext;

                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog1.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);

                            destinationFile = RepDir + "\\Preplanning\\HR" + "\\" + FileName + Ext;
                        }
                    }

                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    System.IO.File.Copy(sourceFile, RepDir + "\\Preplanning\\HR" + "\\" + FileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\HR");

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                }

                txtAttachment.Text = destinationFile;
                PicBox.Image = Image.FromFile(openFileDialog1.FileName);

            }
        }

        void loadImage()
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\HR");

            //IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            var task = new Task<bool>(() => dir2.Exists);
            task.Start();

            if (task.Wait(200) && task.Result)
            {

                string[] files = System.IO.Directory.GetFiles(RepDir + "\\Preplanning\\HR");

                foreach (var item in files)
                {
                    string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                    int extpos = aa.IndexOf(".");

                    string ext = aa.Substring(extpos, aa.Length - extpos);

                    if (item.ToString() == RepDir + "\\Preplanning\\HR" + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(dbl_rec_Crew.ToString()) + dbl_rec_ProdMonth.ToString() + ext)
                    {
                        txtAttachment.Text = item.ToString();
                    }
                }


                if (txtAttachment.Text != string.Empty)
                {
                    using (FileStream stream = new FileStream(txtAttachment.Text, FileMode.Open, FileAccess.Read))
                    {
                        PicBox.Image = Image.FromStream(stream);
                        stream.Dispose();
                    }
                }
            }
        }

        private void btnAddDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            result1 = openFileDialog2.ShowDialog();

            GetDoc();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            string Docs = openFileDialog2.FileName;

            int indexa = Docs.LastIndexOf("\\");

            string sourcefilename = Docs.Substring(indexa + 1, (Docs.Length - indexa) - 1);

            DocsLB.Items.Add(sourcefilename);
        }

        void GetDoc()
        {
            string mianDicrectory = RepDir + "\\Preplanning\\HR\\Documents";

            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(mianDicrectory);

            if (result1 == DialogResult.OK)
            {
                int Name = r.Next(0, 50);
                FileName = string.Empty;
                string Ext = string.Empty;

                string SourcefileName = string.Empty;

                int index = 0;

                sourceFile = openFileDialog2.FileName;

                index = openFileDialog2.SafeFileName.IndexOf(".");

                SourcefileName = openFileDialog2.SafeFileName.Substring(0, index);

                if (tbCrew.EditValue.ToString() != string.Empty)
                {
                    FileName = ProductionGlobal.ProductionGlobal.ExtractAfterColon(tbCrew.EditValue.ToString());
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                if (tbProdMonth.EditValue.ToString() != string.Empty)
                {
                    FileName = FileName + tbProdMonth.EditValue.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a workplace");
                    return;
                }

                Ext = openFileDialog2.SafeFileName.Substring(index);

                destinationFile = mianDicrectory + "\\" + FileName + SourcefileName + Ext;


                if (files.Length != 0)
                {
                    foreach (FileInfo f1 in list2)
                    {
                        if (f1.Name == openFileDialog2.SafeFileName + Name.ToString())
                        {
                            Name = r.Next(0, 50);
                            destinationFile = mianDicrectory + "\\" + FileName + Ext;
                        }
                    }

                    try
                    {
                        System.IO.File.Copy(sourceFile, destinationFile, true);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    System.IO.File.Copy(sourceFile, mianDicrectory + "\\" + FileName + SourcefileName + Ext, true);

                    dir2 = new System.IO.DirectoryInfo(mianDicrectory);

                    list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                }
            }
        }

        public void loadDocs()
        {
            Random r = new Random();

            string mianDicrectory = RepDir + "\\Preplanning\\HR\\Documents";

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(mianDicrectory);

            //IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            var task = new Task<bool>(() => dir2.Exists);
            task.Start();

            if (task.Wait(200) && task.Result)
            {

                string[] files = System.IO.Directory.GetFiles(mianDicrectory);

                //Do everywhere
                DocsLB.Items.Clear();

                foreach (var item in files)
                {
                    string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                    int extpos = aa.IndexOf(".");

                    string ext = aa.Substring(extpos, aa.Length - extpos);

                    int indexa = item.LastIndexOf("\\");

                    string sourcefilename = item.Substring(indexa + 1, (item.Length - indexa) - 1);

                    int indexprodmonth = sourcefilename.IndexOf(tbProdMonth.EditValue.ToString());

                    string SourcefileCheck = sourcefilename.Substring(0, indexprodmonth + 6);

                    int NameLength = sourcefilename.Length - SourcefileCheck.Length;

                    string Docsname = sourcefilename.Substring(SourcefileCheck.Length, NameLength);


                    if (SourcefileCheck == ProductionGlobal.ProductionGlobal.ExtractAfterColon(tbCrew.EditValue.ToString()) + tbProdMonth.EditValue.ToString())
                    {
                        DocsLB.Items.Add(Docsname.ToString());
                    }
                }
            }

        }

        private void DocsLB_DoubleClick(object sender, EventArgs e)
        {
            string mianDicrectory = RepDir + "\\Preplanning\\HR\\Documents";
            if (DocsLB.SelectedIndex != -1)
            {
                string test = mianDicrectory + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(tbCrew.EditValue.ToString()) + tbProdMonth.EditValue.ToString() + DocsLB.SelectedItem.ToString();

                System.Diagnostics.Process.Start(mianDicrectory + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(tbCrew.EditValue.ToString()) + tbProdMonth.EditValue.ToString() + DocsLB.SelectedItem.ToString());
            }
        }

        private void btnImageRemove_Click(object sender, EventArgs e)
        {
            Random r = new Random();

            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(RepDir + "\\Preplanning\\HR");

            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            string[] files = System.IO.Directory.GetFiles(RepDir + "\\Preplanning\\HR");

            foreach (var item in files)
            {
                string aa = item.Substring(item.Length - 5, (item.Length) - (item.Length - 5));

                int extpos = aa.IndexOf(".");

                string ext = aa.Substring(extpos, aa.Length - extpos);

                if (item.ToString() == RepDir + "\\Preplanning\\HR" + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(tbCrew.EditValue.ToString()) + tbProdMonth.EditValue.ToString() + ext)
                {
                    txtAttachment.Text = item.ToString();

                }
            }
            if (txtAttachment.Text != string.Empty)
            {
                PicBox.Image = null;
                File.Delete(txtAttachment.Text);
            }

        }

        private void btnDocRemove_Click(object sender, EventArgs e)
        {
            string mianDicrectory = RepDir + "\\Preplanning\\HR\\Documents";
            if (DocsLB.SelectedIndex != -1)
            {
                File.Delete(mianDicrectory + "\\" + ProductionGlobal.ProductionGlobal.ExtractAfterColon(tbCrew.EditValue.ToString()) + tbProdMonth.EditValue.ToString() + DocsLB.SelectedItem.ToString());
            }

            loadDocs();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void layoutControlGroup4_Click(object sender, EventArgs e)
        {
            btnImageRemove_Click(null,null);        
        }

        private void layoutControlGroup7_Click(object sender, EventArgs e)
        {
            btnDocRemove_Click(null,null);
        }

        private void AddActBtn_Click(object sender, EventArgs e)
        {
            frmGraphicsPrePlanningActionCapture ActFrm = new frmGraphicsPrePlanningActionCapture();
            ActFrm.tbPM.Text = tbProdMonth.EditValue.ToString();
            ActFrm.tbSections.Text = tbSection.EditValue.ToString();
            ActFrm.WPEdit2.Properties.Items.Add(WP1Desc);
            if (WP2Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP2Desc);
            }
            if (WP3Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP3Desc);
            }
            if (WP4Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP4Desc);
            }
            if (WP5Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP5Desc);
            }
            ActFrm.WPEdit2.Visible = true;
            ActFrm.tbWorkplace.Visible = false;

            ActFrm._theSystemDBTag = this.theSystemDBTag;
            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;

            ActFrm.Item = ActionDescription;
            ActFrm.Type = ActionType;
            ActFrm.AllowExit = "Y";

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        private void EditActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please Click the row you want to edit first");
                return;
            }

            frmGraphicsPrePlanningActionCapture ActFrm = new frmGraphicsPrePlanningActionCapture();
            ActFrm.tbPM.Text = tbProdMonth.EditValue.ToString();
            ActFrm.tbSections.Text = tbSection.EditValue.ToString();
            ActFrm.WPEdit2.Properties.Items.Add(WP1Desc);

            if (WP2Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP2Desc);
            }

            if (WP3Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP3Desc);
            }

            if (WP4Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP4Desc);
            }

            if (WP5Desc != string.Empty)
            {
                ActFrm.WPEdit2.Properties.Items.Add(WP5Desc);
            }
            ActFrm.WPEdit2.Visible = true;
            ActFrm.tbWorkplace.Visible = false;

            ActFrm._theSystemDBTag = this.theSystemDBTag;
            ActFrm._UserCurrentInfo = this.UserCurrentInfo.Connection;

            ActFrm.Item = "Rock Engineering Actions";
            ActFrm.Type = ActionType;
            ActFrm.AllowExit = "Y";
            ActFrm.FlagEdit = "Edit";

            ActFrm.WPEdit2.EditValue = Workplace;
            ActFrm.Item = Description;
            ActFrm.ActionTxt.Text = Recomendation;
            ActFrm.ReqDate.Text = TargetDate;
            ActFrm.PriorityCmb.Text = Priority;
            ActFrm.RespPersonCmb.EditValue = RespPerson;
            ActFrm.OverseerCmb.EditValue = Overseer;
            ActFrm.ActID = ID;

            ActFrm.StartPosition = FormStartPosition.CenterScreen;
            ActFrm.ShowDialog(this);

            LoadActions();
        }

        private void DelActBtn_Click(object sender, EventArgs e)
        {
            if (ID == string.Empty)
            {
                MessageBox.Show("Please click the row you want to delete first.");
                return;
            }



            MWDataManager.clsDataAccess _DeleteAction = new MWDataManager.clsDataAccess();
            _DeleteAction.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _DeleteAction.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _DeleteAction.queryReturnType = MWDataManager.ReturnType.DataTable;
            _DeleteAction.SqlStatement = " Delete from tbl_Shec_Incidents where ID = '" + ID + "'\r\n";

            _DeleteAction.ExecuteInstruction();

            LoadActions();
        }

        private void LoadActions()
        {
            //Declarations
            string sql = string.Empty;

            _FormType = "Labour";

            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _sqlConnection.SqlStatement = "EXEC sp_PrePlanning_" + _FormType + "_LoadActions '" + WP1Desc + "','" + WP2Desc + "','" + WP3Desc + "','" + WP4Desc + "','" + WP5Desc + "'"; 
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
           
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;
           
            gcActions.DataSource = null;
            gcActions.DataSource = dtReceive;
            gcActID.FieldName = "ID";
            gcWorkplace.FieldName = "Workplace";
            gcDescription.FieldName = "Description";
            gcRecommendation.FieldName = "Action";
            gcTargetDate.FieldName = "TargetDate";
            gcPriority.FieldName = "Priority";
            gcImage.FieldName = "CompNotes";
            gcViewImage.FieldName = "Hyperlink";
            gcRespPerson.FieldName = "RespPerson";
            gcOverseer.FieldName = "HOD";
        }

        private void gvAction_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            ID = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[0]).ToString();
            Workplace = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[1]).ToString();
            Description = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[2]).ToString();
            Recomendation = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[3]).ToString();
            TargetDate = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[4]).ToString();
            Priority = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[5]).ToString();
            FileName = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[6]).ToString();

            RespPerson = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[9]).ToString();
            Overseer = gvAction.GetRowCellValue(e.RowHandle, gvAction.Columns[10]).ToString();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.Items.Count > 0)

            {

                if (listBox.SelectedIndex == -1)

                {

                    MessageBox.Show("Please select a Crew Type");

                    return;

                }



                string HRMethodID = "1000000";

                string HRStdNormID = "1000000";



                foreach (DataRow dr in dtCrewType.Rows)

                {

                    if (dr["GroupName"].ToString() == listBox.SelectedItem.ToString())

                    {

                        HRMethodID = dr["GroupName"].ToString();

                        HRStdNormID = dr["GroupName"].ToString();

                    }

                }



                MWDataManager.clsDataAccess _dbCrewLink = new MWDataManager.clsDataAccess();

                _dbCrewLink.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, TUserInfo.Site);

                _dbCrewLink.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                _dbCrewLink.queryReturnType = MWDataManager.ReturnType.DataTable;

                StringBuilder SB = new StringBuilder();

                SB.AppendLine("Delete from tbl_PrePlanning_CrewType_Linkage where CrewNo = '" + dbl_rec_Crew.ToString() + "'");

                SB.AppendLine("insert into tbl_PrePlanning_CrewType_Linkage values ('" + dbl_rec_Crew.ToString() + "','" + HRStdNormID + "','" + HRMethodID + "')");

                _dbCrewLink.SqlStatement = SB.ToString();



                SB.Clear();



                var resultCrew = _dbCrewLink.ExecuteInstruction();

                if (resultCrew.success)

                {

                    //Global.sysNotification.TsysNotification.showNotification("Data Saved", "Saved", Color.CornflowerBlue);

                }

            }



            //LoadCompliment();
        }

        private void gvHR_DoubleClick(object sender, EventArgs e)
        {
            string EmployeeNo = gvHR.GetRowCellValue(gvHR.FocusedRowHandle, gvHR.Columns["company_number"].FieldName).ToString();
            ///GetImage
            MWDataManager.clsDataAccess _dbCrewLink = new MWDataManager.clsDataAccess();
            _dbCrewLink.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, TUserInfo.Site);
            _dbCrewLink.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbCrewLink.queryReturnType = MWDataManager.ReturnType.DataTable;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("SELECT [EmpPhoto] FROM [AISABUTNADB02].[TNA].[audit].[Audit_PersonnelPhoto] where [PersonNumber]  = '" + EmployeeNo + "'");
            _dbCrewLink.SqlStatement = SB.ToString();
            SB.Clear();
            var resultCrew = _dbCrewLink.ExecuteInstruction();


            if (_dbCrewLink.ResultsDataTable.Rows.Count > 0)
            {
                byte[] img = (byte[])_dbCrewLink.ResultsDataTable.Rows[0][0];
                MemoryStream str = new MemoryStream();
                str.Write(img, 0, img.Length);
                Bitmap bit = new Bitmap(str);
            }
            
        }

        private void repositoryItemPictureEdit1_ImageLoading(object sender, DevExpress.XtraEditors.Repository.SaveLoadImageEventArgs e)
        {
            
        }

        private void gvHR_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption == "Photo" && gvHR.GetRowCellValue(e.RowHandle, gvHR.Columns["EmployeePhoto"].FieldName) != System.DBNull.Value)
            {
                byte[] img = (byte[])gvHR.GetRowCellValue(e.RowHandle, gvHR.Columns["EmployeePhoto"].FieldName);
                MemoryStream str = new MemoryStream();
                str.Write(img, 0, img.Length);
                Bitmap bit = new Bitmap(str);
                e.CellValue = bit;
            }
        }
    }
}
