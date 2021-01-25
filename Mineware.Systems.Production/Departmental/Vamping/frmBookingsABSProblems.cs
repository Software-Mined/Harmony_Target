using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.GlobalConnect;
using MWDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Vamping
{
    public partial class frmBookingsABSProblems : DevExpress.XtraEditors.XtraForm
    {
        #region Fields and Properties        
        private DataTable dt_Codes = new DataTable();
        private DataTable dt_ProblemList = new DataTable();
        private DataTable dt_1 = new DataTable();
        private DataTable dt_2 = new DataTable();
        private DataTable dt_3 = new DataTable();
        private DataTable dt_4 = new DataTable();
        private DataTable dt_5 = new DataTable();
        private DataTable dt_6 = new DataTable();
        private DataTable dt_7 = new DataTable();
        private DataTable dt_8 = new DataTable();
        private DataTable dt_9 = new DataTable();
        private DataTable dt_10 = new DataTable();
        private DataTable dt_11 = new DataTable();
        private DataTable dt_12 = new DataTable();
        private DataTable dt_13 = new DataTable();
        private DataTable dt_14 = new DataTable();
        private DataTable dt_15 = new DataTable();
        private DataTable dt_16 = new DataTable();
        private DataTable explanation = new DataTable();
        private bool IsPlanStop = false;

        /// <summary>
        /// The connection string
        /// </summary>
        public string TheConnection { get; set; }
        /// <summary>
        /// The section id
        /// </summary>
        public string TheSection { get; set; }
        /// <summary>
        /// The workplace name
        /// </summary>
        public string TheWorkplace { get; set; }
        /// <summary>
        /// The note id
        /// </summary>
        public string NoteID { get; set; }
        /// <summary>
        /// The id of the problem
        /// </summary>
        public string ProblemID { get; set; }
        /// <summary>
        /// The boss note 
        /// </summary>
        public string SBossNotes { get; set; }
        /// <summary>
        /// THe description of the problem
        /// </summary>
        public string ProblemDesc { get; set; }
        /// <summary>
        /// THe date of the problem
        /// </summary>
        public DateTime TheDate { get; set; }
        /// <summary>
        /// The activity
        /// </summary>
        public int TheActivity { get; set; }

        public Dictionary<string, string> Problems { get; set; }

        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;

        public string _LostBlastRecon = "N";
        public string _AddProblem;
        public string _EditProblem;

        public string _Date;
        public string _Cycle;
        public string _Sqm;
        public string _Problem;
        public string _LostBlastCheck;
        public string _ShiftbossNotes;
        public string _MinerID;
        public string _WPAct;
        public string _workplaceID;
        public string _prodmonth;


        #endregion Fields and Properties

        #region Constructor
        /// <summary>
        /// The constructor
        /// </summary>
        public frmBookingsABSProblems()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Events
        private void ucBookingsABSProblems_Load(object sender, EventArgs e)
        {
            //this.Icon = new System.Drawing.Icon(@"C:\Mineware\Syncromine\SM.ico");
            this.Text += " (" + TheWorkplace + ")";

            gc1.Visible = false;
            gc2.Visible = false;
            gc3.Visible = false;
            gc4.Visible = false;
            gc5.Visible = false;
            gc6.Visible = false;
            gc7.Visible = false;
            gc8.Visible = false;
            gc9.Visible = false;
            gc10.Visible = false;
            gc11.Visible = false;
            gc12.Visible = false;
            gc13.Visible = false;
            gc14.Visible = false;
            gc15.Visible = false;
            gc16.Visible = false;

            dt_Codes = get_Problems_Types(TheActivity);
            int x = 0;
            if (dt_Codes?.Rows.Count == 0)
            {
                MessageBox.Show("There are no Problem Types in the System.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                foreach (DataRow r in dt_Codes.Rows)
                {
                    x += 1;

                    dt_ProblemList = get_Problems_Groups(TheActivity, r["ProblemType"].ToString());

                    if (x == 1)
                    {
                        dt_1 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc1Notes.Caption = r["ProblemType"].ToString();
                        gc1.DataSource = dt_1;
                        gc1.Visible = true;
                        continue;
                    }

                    if (x == 2)
                    {
                        dt_2 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc2Notes.Caption = r["ProblemType"].ToString();
                        gc2.DataSource = dt_2;
                        gc2.Visible = true;
                        continue;
                    }

                    if (x == 3)
                    {
                        dt_3 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc3Notes.Caption = r["ProblemType"].ToString();
                        gc3.DataSource = dt_3;
                        gc3.Visible = true;
                        continue;
                    }

                    if (x == 4)
                    {
                        dt_4 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc4Notes.Caption = r["ProblemType"].ToString();
                        gc4.DataSource = dt_4;
                        gc4.Visible = true;
                        continue;
                    }

                    if (x == 5)
                    {
                        dt_5 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc5Notes.Caption = r["ProblemType"].ToString();
                        gc5.DataSource = dt_5;
                        gc5.Visible = true;
                        continue;
                    }

                    if (x == 6)
                    {
                        dt_6 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc6Notes.Caption = r["ProblemType"].ToString();
                        gc6.DataSource = dt_6;
                        gc6.Visible = true;
                        continue;
                    }

                    if (x == 7)
                    {
                        dt_7 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc7Notes.Caption = r["ProblemType"].ToString();
                        gc7.DataSource = dt_7;
                        gc7.Visible = true;
                        continue;
                    }

                    if (x == 8)
                    {
                        dt_8 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc8Notes.Caption = r["ProblemType"].ToString();
                        gc8.DataSource = dt_8;
                        gc8.Visible = true;
                        continue;
                    }

                    if (x == 9)
                    {
                        dt_9 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc9Notes.Caption = r["ProblemType"].ToString();
                        gc9.DataSource = dt_9;
                        gc9.Visible = true;
                        continue;
                    }

                    if (x == 10)
                    {
                        dt_10 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc10Notes.Caption = r["ProblemType"].ToString();
                        gc10.DataSource = dt_10;
                        gc10.Visible = true;
                        continue;
                        // gv10.Appearance.ColumnHeadersDefaultCellStyle.BackColor = Color.Yellow;
                    }

                    if (x == 11)
                    {
                        dt_11 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc11Notes.Caption = r["ProblemType"].ToString();
                        gc11.DataSource = dt_11;
                        gc11.Visible = true;
                        continue;
                    }

                    if (x == 12)
                    {
                        dt_12 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc12Notes.Caption = r["ProblemType"].ToString();
                        gc12.DataSource = dt_12;
                        gc12.Visible = true;
                        continue;
                    }

                    if (x == 13)
                    {
                        dt_13 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc13Notes.Caption = r["ProblemType"].ToString();
                        gc13.DataSource = dt_13;
                        gc13.Visible = true;
                        continue;
                    }

                    if (x == 14)
                    {
                        dt_14 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc14Notes.Caption = r["ProblemType"].ToString();
                        gc14.DataSource = dt_14;
                        gc14.Visible = true;
                        continue;
                    }

                    if (x == 15)
                    {
                        dt_15 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc15Notes.Caption = r["ProblemType"].ToString();
                        gc15.DataSource = dt_15;
                        gc15.Visible = true;
                        continue;
                    }

                    if (x == 16)
                    {
                        dt_16 = dt_ProblemList; //_Problem_List.ResultsDataTable;
                        gc16Notes.Caption = r["ProblemType"].ToString();
                        gc16.DataSource = dt_16;
                        gc16.Visible = true;
                        continue;
                    }
                }
            }

            //Setup plan stop            
            gc17Notes.Caption = "17.Planned Stoppages";
            if (TheActivity == 0)
            {
                gc17.DataSource = get_PlannedStoppages(TheActivity);
            }
            if (TheActivity == 1)
            {
                gc17.DataSource = get_PlannedStoppagesDev();
            }
            gc17.Visible = true;

            txtSearch.Text = string.Empty;

            if (Problems.Count > 0)
            {
                var val = Problems.Values.FirstOrDefault().Split(';');

                txtSBossNotes.Text = val[1];
                ProblemID = Problems.Keys.FirstOrDefault();
                lbNoteID.Text = Problems.Keys.FirstOrDefault() + ":" + ProblemDesc;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_LostBlastRecon == "Y")
            {
                SaveProblem();
            }
            else
            {
                if (lbNoteID.Text == string.Empty)
                {
                    MessageBox.Show("Please select a problem.", "Unselected Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (txtSBossNotes.Text == string.Empty)
                {
                    MessageBox.Show("Please provide a note.", "No Notes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    try
                    {
                        string bookCode = "PR";
                        if (IsPlanStop)
                        {
                            bookCode = "PS";
                            //_clsBookingsABS.SavePlanStoppage(lbSection.Text, workplace, Convert.ToDateTime(TheDate.ToString("yyyy-MM-dd")), ProblemID);

                        }
                        else
                        {
                            //_clsBookingsABS.SaveProblem(lbSection.Text, workplace, Convert.ToDateTime(TheDate.ToString("yyyy-MM-dd")), ProblemID, txtSBossNotes.Text);
                        }

                        Problems.Clear();
                        Problems.Add(ProblemID, bookCode + ";" + txtSBossNotes.Text);

                        Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Action Closed", Color.CornflowerBlue);
                        this.Close();
                    }
                    catch
                    {
                        Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Not Saved", "Error", Color.Red);
                        return;
                    }
                }
            }
        }

        public void SaveProblem()
        {
            string LBlast = "N";

            _Problem = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(lbNoteID.Text);
            _ShiftbossNotes = txtSBossNotes.Text;

            clsDataAccess theData = new clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection); ;

            theData.SqlStatement = " update tbl_Planning_Vamping \r\n" +
            "set BookCode = 'PR', \r\n" +
            " BookProb = '" + _Problem + "' \r\n" +
            " where workplaceid = '" + _workplaceID + "' \r\n" +
            " and Calendardate = '" + _Date + "' \r\n" +
            " and SectionID = '" + _MinerID + "' \r\n" +
            " and activity = '" + TheActivity + "' \r\n\r\n";

            if (cbxLostBlast.Checked == true)
            {
                LBlast = "Y";
            }

            //theData.SqlStatement = theData.SqlStatement + " delete from tbl_ProblemBook \r\n";
            //theData.SqlStatement = theData.SqlStatement + "where workplaceid =  '" + _workplaceID + "' \r\n";
            //theData.SqlStatement = theData.SqlStatement + "and sectionid = '" + _MinerID + "' and activity = '" + TheActivity + "' and calendardate = '" + String.Format("{0:yyyy-MM-dd}", _Date) + "' \r\n\r\n";

            //theData.SqlStatement = theData.SqlStatement + " insert into tbl_ProblemBook \r\n";
            //theData.SqlStatement = theData.SqlStatement + "values ( '" + _workplaceID + "',  '" + _prodmonth + "', \r\n";
            //theData.SqlStatement = theData.SqlStatement + " '" + _MinerID + "', '" + TheActivity + "' , '" + String.Format("{0:yyyy-MM-dd}", _Date) + "', \r\n";
            //theData.SqlStatement = theData.SqlStatement + " '" + _Problem + "', 'D', 'N', '" + _ShiftbossNotes + "', \r\n";
            //theData.SqlStatement = theData.SqlStatement + " null, null, '" + String.Format("{0:yyyy-MM-dd}", _Date) + "', '" + LBlast + "', \r\n";
            //theData.SqlStatement = theData.SqlStatement + " null, '', '" + String.Format("{0:yyyy-MM-dd}", _Date) + "', '', null, null )  \r\n\r\n";

            theData.queryExecutionType = ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = ReturnType.DataTable;
            var errorMsg = theData.ExecuteInstruction();

            this.Close();
        }

        private void txtSBossNotes_TextChanged(object sender, EventArgs e)
        {
            SBossNotes = txtSBossNotes.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NoteID = string.Empty;
            ProblemID = string.Empty;
            SBossNotes = string.Empty;
            lbNoteID.Text = string.Empty;
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterInfo();
        }
        private void gv_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        #endregion Events

        #region Methods
        /// <summary>
        /// Filter the information
        /// </summary>
        public void FilterInfo()
        {
            gv1.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv2.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv3.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv4.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv5.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv6.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv7.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv8.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv9.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv10.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv11.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv12.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv13.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv14.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv15.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv16.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");
            gv17.Columns[0].FilterInfo = new ColumnFilterInfo("[ProblemNote] LIKE '%" + txtSearch.Text + "%'");

        }
        /// <summary>
        /// A Colonoscopy
        /// </summary>
        /// <param name="TheString">The string to Colonoscopies</param>
        /// <returns></returns>
        public string ExtractAfterColon(string TheString)
        {
            string AfterColon;

            int index = TheString.IndexOf(":"); // Kry die postion van die :

            AfterColon = TheString.Substring(index + 1); // kry alles na :

            return AfterColon;
        }
        /// <summary>
        /// A Colonoscopy
        /// </summary>
        /// <param name="TheString">The string to Colonoscopies</param>
        /// <returns></returns>
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

        public string ExtractBeforePoint(string TheString)
        {
            if (TheString != string.Empty)
            {
                string BeforeColon;

                int index = TheString.IndexOf(".");

                BeforeColon = TheString.Substring(0, index);

                return BeforeColon;
            }
            else
            {
                return string.Empty;
            }
        }

        public DataTable get_Problems_Types(int TheActivity)
        {
            string where = TheActivity == 0 ? "Stoping = 1" : "Dev = 1";
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = _UserCurrentInfoConnection ;
            theData.SqlStatement = @"  SELECT DISTINCT [ProblemGroup] AS [ProblemType] ,CONVERT(INT,SUBSTRING([ProblemGroup],1,CHARINDEX('.', [ProblemGroup],0) - 1)) AS [ID]
                                      FROM  [tbl_Code_Problem_Main]
                                      WHERE " + where +
                                      @"ORDER BY CONVERT(INT, SUBSTRING([ProblemGroup], 1, CHARINDEX('.', [ProblemGroup], 0) - 1))";
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            var errorMsg = theData.ExecuteInstruction();

            return theData.ResultsDataTable;
        }

        public DataTable get_Problems_Groups(int TheActivity, string ProblemGroup)
        {
            string where = TheActivity == 0 ? "Stoping = 1" : "Dev = 1";
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = _UserCurrentInfoConnection;
            theData.SqlStatement = @"SELECT ProblemID + ':' + ProblemDesc AS ProblemNote		                            
                                    FROM [dbo].[tbl_Code_Problem_Main]   
                                    where [ProblemGroup] = '" + ProblemGroup + @"' AND " + where;
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            var errorMsg = theData.ExecuteInstruction();

            return theData.ResultsDataTable;
        }

        public DataTable get_PlannedStoppages(int TheActivity)
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = _UserCurrentInfoConnection;
            theData.SqlStatement = @"SELECT [Code] + ':' + [Description] ProblemNote FROM 
	                                (SELECT [Code], [Description], CASE WHEN [Stope] = 'Y' THEN '0' WHEN [Dev] = 'Y' THEN '1' 
										                                WHEN [Ledge] = 'Y' THEN '9' END AS [Activity] FROM [Mineware_Reporting].[dbo].tbl_Central_Code_Cycle 
	                                 WHERE Code <> '')A WHERE [Activity] = '" + TheActivity + "' and code <> 'BL' and  code <> 'BLH' and  code <> 'BV' and  code <> 'SUBL'";

            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            var errorMsg = theData.ExecuteInstruction();

            return theData.ResultsDataTable;
        }

        public DataTable get_PlannedStoppagesDev()
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = _UserCurrentInfoConnection;
            theData.SqlStatement = @"SELECT [Code] + ':' + [Description] ProblemNote
                                        FROM [Mineware_Reporting].[dbo].tbl_Central_Code_Cycle 
                                        Where Dev = 'Y' and code <> 'BL' and  code <> 'BLH' and  code <> 'BV' and  code <> 'SUBL'";
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            var errorMsg = theData.ExecuteInstruction();

            return theData.ResultsDataTable;
        }

        #endregion Methods               

        private void gv1_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }

        }

        private void gv2_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv3_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv4_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv5_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv6_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv7_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv8_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv9_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv10_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv11_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv12_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv13_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv14_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv15_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv16_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gv17_Click(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            //If the grid view is gv17 then it is planned stoppage (change flag for saving see btnSave event)
            IsPlanStop = view.Name == "gv17";

            ProblemID = ExtractBeforeColon(view.FocusedValue.ToString());
            ProblemDesc = ExtractBeforePoint(view.Columns[0].Caption.ToString());

            if (view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]) != null)
            {
                var noteid = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProblemNote"]);
                lbNoteID.Text = noteid.ToString();
            }
        }

        private void gc12_Click(object sender, EventArgs e)
        {

        }
    }
}
