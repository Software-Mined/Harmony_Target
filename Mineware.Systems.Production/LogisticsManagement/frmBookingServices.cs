using DevExpress.XtraEditors.Repository;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class frmBookingServices : DevExpress.XtraEditors.XtraForm
    {
        public frmBookingServices()
        {
            InitializeComponent();
        }

        #region private variables
        string lblReturnWorkplace;
        string lblABS;
        string lblRowHandle;

        string lblAColor;
        string lblBColor;
        string lblSColor;
        DataTable dt = new DataTable();

        #endregion

        #region public variables
        public string _UserCurrentInfoConnection;
        public string lblProblem;
        public string lblProbComment;
        public string lblProdmonth;
        public string lblMO;
        public string editDone;
        public string lblMinerID;
        #endregion


        private void frmBookingServices_Load(object sender, EventArgs e)
        {
            lblAColor = ProductionGlobalTSysSettings._AColor.ToString();
            lblBColor = ProductionGlobalTSysSettings._BColor.ToString();
            lblSColor = ProductionGlobalTSysSettings._SColor.ToString();

            //loadMiner();
            LoadBookingGrid();
        }

        //private void loadMiner()
        //{
        //    MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
        //    _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
        //    _dbMan.SqlStatement = "Select *,SectionID+':'+Name Description from tbl_SDB_SECTION where prodmonth = '" + ProductionAmplatsGlobalTSysSettings._currentProductionMonth + "' and Hierarchicalid = '6' ";
        //    _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
        //    _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    _dbMan.ExecuteInstruction();

        //    DataTable dtSub = _dbMan.ResultsDataTable;

        //    foreach (DataRow dr in dtSub.Rows)
        //    {
        //        lbxMiners.Items.Add(dr["Description"].ToString());
        //    }
        //}

        private void LoadBookingGrid()
        {
            string Date = String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date);

            if (Convert.ToDateTime(ssslabel.Text) < Convert.ToDateTime(Date))
            {
                Whylabel.Text = "Reason for late start";
                Whylabel.Visible = true;
                latecomboBox.Visible = true;
            }

            if (Convert.ToDateTime(ssslabel.Text) > Convert.ToDateTime(Date))
            {
                Whylabel.Text = "Reason for late start";
                Whylabel.Visible = true;
                latecomboBox.Visible = true;
            }

            MWDataManager.clsDataAccess _dbManRep = new MWDataManager.clsDataAccess();
            _dbManRep.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
            _dbManRep.SqlStatement = " select a.*, b.AmountYesNo, AmountMeters, AmountTime, AmountCompleted, AmountQuantity, AmountSQM, AmountYesNoa \r\n" +
                                     " ,isnull(b.ExtraABSP,'') ExtraABSP,isnull(b.ABSP,'')ABSP,isnull(b.WPStatus,'')WPStatus,isnull(b.ABSNote,'')ABSNote \r\n" +
                                     "  ,b.ProbComments,b.Problemid,b.ProbDescription, Completed from ( \r\n" +
                                     " select top(1)  'Plan' lb, crew, a.authid, a.mainactid, c.description MainActDesc , a.subactid, d.description SubActDesc , a.workplace, a.measureType, a.starton starton,'' Amountaa,'' Problems,'' Comments, b.minerid mm \r\n" +
                                     "FROM [tbl_SDB_Activity_PlanDay] a, [dbo].[tbl_SDB_Activity_Authorisation] b, [dbo].[tbl_SDB_Activity] c, [dbo].[tbl_SDB_SubActivity] d, \r\n" +
                                     "[dbo].[tbl_SDB_Activity_PlanSum] e \r\n" +
                                     "where \r\n" +
                                     "b.mainactid = c.mainactid and a.subactid = d.subactid and \r\n" +
                                     "a.authid = b.authid and a.mainactid = b.mainactid \r\n" +
                                     "and a.authid = e.authid and a.mainactid = e.mainactid and a.subactid = e.subactid  \r\n" +
                                     "and a.workplace =  '" + WpNamelbl.Text + "' and   c.description+' ('+ convert(varchar(11),projectstart,106) + ')' = '" + Actlabel.Text + "'  \r\n" +
                                     "and a.AuthID =  '" + AuthIDLbl.Text + "'  and actstart  = '" + ssslabel.Text + "' \r\n" +
                                     "and b.minerid = '" + lblMinerID + "'  \r\n" +
                                     ") a \r\n" +
                                     "left outer join \r\n" +
                                     " ( Select * \r\n" +
                                     " ,case when AmountYesNoa = '0.00' then 'No' \r\n" +
                                     " when AmountYesNoa = '' then  'No'  else AmountYesNoa \r\n" +
                                     "  end as AmountYesNo  \r\n" +
                                      "from ( \r\n" +
                                     "Select case when measuretype = 'Meters' and Amount = '' then '0.00'   \r\n" +
                                     "when measuretype = 'Meters'  then Amount end as AmountMeters \r\n" +
                                     ",case when measuretype = 'Time' then Amount end as AmountTime \r\n" +
                                     ",case when measuretype = '% Complete' then Amount end as AmountCompleted \r\n" +
                                     ",case when measuretype = 'Quantity' then convert(decimal(18,1),Amount) end as AmountQuantity \r\n" +
                                     ",case when measuretype = 'SQM' then convert(decimal(18,0),Amount) end as AmountSQM \r\n" +
                                     ",case when measuretype = 'Yes/No' then Amount end as AmountYesNoa \r\n" +
                                     ",case when WPStatus = 'BP' then 'B' \r\n" +
                                     "when WPStatus = 'SP' then 'S' else WPStatus end as WPStatus \r\n" +
                                     ",case when WPStatus = 'BP' then 'B' \r\n" +
                                     "when WPStatus = 'SP' then 'S' else WPStatus end as ExtraABSP \r\n" +
                                     ", case when WPStatus = 'BP' or WPStatus = 'SP' then 'P' end as ABSP \r\n" +
                                     ",ABSNote  \r\n" +
                                     ",substring(miner,0,9) minerid,Authid Authid1,Mainactid Mainactid1,Workplace Workplace1,Subactid Subactid1,calendarDate,Problemid,Comments ProbComments,Completed from [tbl_SDB_Bookings] )a \r\n" +
                                     "Left Outer join  \r\n" +
                                     "(Select ProblemID ProbID,Description ProbDescription from [tbl_SDB_PROBLEM]) b on a.Problemid = b.ProbID  \r\n" +
                                     "where calendardate = '" + Date + "') b on a.Authid = b.AuthID1 and a.mainactid = b.mainactid1 and a.subactid = b.subactid1 \r\n" +
                                     "and a.workplace = b.workplace1 \r\n";

            _dbManRep.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManRep.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManRep.ResultsTableName = "Booking";
            _dbManRep.ExecuteInstruction();

            dt = _dbManRep.ResultsDataTable;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            gcBooking.DataSource = ds.Tables[0];

            lblMinerID = dt.Rows[0]["mm"].ToString();

            colAuthID.FieldName = "authid";
            colMainActID.FieldName = "mainactid";
            colSubActID.FieldName = "subactid";
            colWorkplace.FieldName = "workplace";
            colTaskDescription.FieldName = "SubActDesc";
            colActivity.FieldName = "MainActDesc";
            colType.FieldName = "measureType";

            colPercComplete.FieldName = "AmountCompleted";
            colQuantity.FieldName = "AmountQuantity";
            colSQM.FieldName = "AmountSQM";
            colTime.FieldName = "AmountTime";
            colYesOrNo.FieldName = "AmountYesNo";
            colMeters.FieldName = "AmountMeters";

            colAmount.FieldName = "Amountaa";
            colProblem.FieldName = "ProbDescription";
            colComments.FieldName = "ProbComments";
            colStatus.FieldName = "WPStatus";

            colABSnotes.FieldName = "ABSNote";
            colABSExtra.FieldName = "ExtraABSP";
            colASBP.FieldName = "ABSP";

            if (_dbManRep.ResultsDataTable.Rows.Count > 0)
            {
                if(_dbManRep.ResultsDataTable.Rows[0]["Completed"].ToString() == "Y")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string Date = String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date);

            int count = 0;

            // get prodmonth 

            MWDataManager.clsDataAccess _dbManPM = new MWDataManager.clsDataAccess();
            _dbManPM.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
            _dbManPM.SqlStatement = " select max(prodmonth) pm from tbl_Code_Calendar_Section where CalendarCode = '" + lblMO + "' and begindate <= '" + Date + "' and enddate >= '" + Date + "' ";
            _dbManPM.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManPM.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManPM.ExecuteInstruction();

            DataTable Neil = _dbManPM.ResultsDataTable;

            string pm = Neil.Rows[0]["pm"].ToString();

            string authid = string.Empty;

            string Comp = "N";

            if (checkBox1.Checked == true)
                Comp = "Y";

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);

            _dbMan.SqlStatement = " ";
            for (int i = 0; i < gvBooking.RowCount; i++)
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "Delete from tbl_SDB_Bookings where AuthID = '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["authid"]) + "' and MainActID = '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["mainactid"]) + "'  and calendarDate = '" + Date + "'  and Miner = '" + lblMinerID + "' and subactid = '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["subactid"]) + "'  and startdate = '" + ssslabel.Text + "'  \r\n\r\n";

                authid = gvBooking.GetRowCellValue(i, gvBooking.Columns["authid"]).ToString();

                _dbMan.SqlStatement = _dbMan.SqlStatement + "Insert into tbl_SDB_Bookings (AuthID,MainActID,Workplace \r\n" +
                                       " ,calendarDate,SubActID,Task,Miner  \r\n" +
                                       " ,MeasureType,Amount,problemID,Comments,WPStatus,Completed, prodmonth, startdate) \r\n" +
                                      "values ('" + gvBooking.GetRowCellValue(i, gvBooking.Columns["authid"]) + "','" + gvBooking.GetRowCellValue(i, gvBooking.Columns["mainactid"]) + "' , '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["workplace"]) + "'  \r\n" +
                                      " ,'" + Date + "' , '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["subactid"]) + "' ,'" + gvBooking.GetRowCellValue(i, gvBooking.Columns["SubActDesc"]) + "', '" + lblMinerID + "'  \r\n" +
                                      " ,'" + gvBooking.GetRowCellValue(i, gvBooking.Columns["measureType"]) + "', ";

                if (gvBooking.GetRowCellValue(i, gvBooking.Columns["measureType"]).ToString() == "Meters")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["AmountMeters"]) + "' ";
                }

                if (gvBooking.GetRowCellValue(i, gvBooking.Columns["measureType"]).ToString() == "Time")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["AmountTime"]) + "' ";
                }

                if (gvBooking.GetRowCellValue(i, gvBooking.Columns["measureType"]).ToString() == "% Complete")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["AmountCompleted"]) + "' ";
                }

                if (gvBooking.GetRowCellValue(i, gvBooking.Columns["measureType"]).ToString() == "Quantity")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["AmountQuantity"]) + "' ";
                }

                if (gvBooking.GetRowCellValue(i, gvBooking.Columns["measureType"]).ToString() == "SQM")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["AmountSQM"]) + "' ";
                }

                if (gvBooking.GetRowCellValue(i, gvBooking.Columns["measureType"]).ToString() == "Yes/No")
                {
                    _dbMan.SqlStatement = _dbMan.SqlStatement + " '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["AmountYesNo"]) + "' ";
                }

                _dbMan.SqlStatement = _dbMan.SqlStatement + ",(Select Problemid from tbl_SDB_PROBLEM where Description = '" + gvBooking.GetRowCellValue(i, gvBooking.Columns["ProbDescription"]) + "' ),'" + gvBooking.GetRowCellValue(i, gvBooking.Columns["ProbComments"]) + "' \r\n" +
                                       " ,'" + gvBooking.GetRowCellValue(i, gvBooking.Columns["WPStatus"]) + "' , '" + Comp + "', '" + pm + "', '" + ssslabel.Text + "' ) \r\n\r\n";

            }

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvBooking_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string ss = string.Empty;

            string measuretype = gvBooking.GetRowCellValue(e.RowHandle, gvBooking.Columns["measureType"]).ToString();

            string Fieldname = string.Empty;

            if (gvBooking.GetRowCellValue(e.RowHandle, e.Column) != null)
            {
                ss = gvBooking.GetRowCellValue(e.RowHandle, e.Column).ToString();
            }

            if (e.Column.FieldName == "AmountMeters")
                e.Appearance.BackColor = Color.LightGray;

            if (e.Column.FieldName == "AmountTime")
                e.Appearance.BackColor = Color.LightGray;

            if (e.Column.FieldName == "AmountCompleted")
                e.Appearance.BackColor = Color.LightGray;

            if (e.Column.FieldName == "AmountQuantity")
                e.Appearance.BackColor = Color.LightGray;

            if (e.Column.FieldName == "AmountSQM")
                e.Appearance.BackColor = Color.LightGray;

            if (e.Column.FieldName == "AmountYesNo")
                e.Appearance.BackColor = Color.LightGray;

            if (measuretype == "Meters")
                Fieldname = "AmountMeters";

            if (measuretype == "Time")
                Fieldname = "AmountTime";

            if (measuretype == "% Complete")
                Fieldname = "AmountCompleted";

            if (measuretype == "Quantity")
                Fieldname = "AmountQuantity";

            if (measuretype == "SQM")
                Fieldname = "AmountSQM";

            if (measuretype == "Yes/No")
                Fieldname = "AmountYesNo";

            if (e.Column.FieldName == Fieldname)
            {
                e.Appearance.BackColor = Color.Transparent;
            }


            if (e.Column.Name == "colStatus")
            {
                if (ss == "Safe")
                {
                    e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(lblAColor));
                }

                if (ss == "UnSafe")
                {
                    e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(lblSColor));
                }

                if (ss == "No Vis.")
                {
                    e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(lblBColor));
                }
            }
        }

        private void gvBooking_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null)
            {
                if ((e.Column.FieldName == "AmountMeters") || (e.Column.FieldName == "AmountTime") || (e.Column.FieldName == "AmountCompleted") || (e.Column.FieldName == "AmountQuantity")
                    || (e.Column.FieldName == "AmountSQM") || (e.Column.FieldName == "AmountYesNo"))
                {
                    e.Info.Caption = string.Empty;
                    e.Painter.DrawObject(e.Info);
                    e.Appearance.DrawVString(e.Cache, " " + e.Column.ToString(), e.Appearance.Font, e.Appearance.GetForeBrush(e.Cache), e.Bounds, new DevExpress.Utils.StringFormatInfo(new StringFormat()), 270);
                    e.Handled = true;
                }
            }
        }

        private void gvBooking_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Column.FieldName == "AmountYesNo")
            {
                RepositoryItemCheckEdit ritem = new RepositoryItemCheckEdit();
                ritem.ValueChecked = "Y";
                ritem.ValueUnchecked = "N";
                e.RepositoryItem = ritem;
            }
        }

        private void gvBooking_DoubleClick(object sender, EventArgs e)
        {
            if (gvBooking.FocusedColumn.FieldName == "AmountYesNo")
            {
                string ss = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["AmountYesNo"]).ToString();

                if (ss == "Y")
                {
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["AmountYesNo"], "No");
                }

                if (ss == "N")
                {
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["AmountYesNo"], "Yes");
                }
            }

            lblProblem = string.Empty;
            lblProbComment = string.Empty;

            int rowH = gvBooking.FocusedRowHandle;

            string date = String.Format("{0:yyyy-MM-dd}", DPdate.Value.Date.ToString());

            decimal amount = 0;

            if (gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["Amountaa"]).ToString() == string.Empty)
            {
                amount = 0;
            }
            else
            {
                amount = Convert.ToDecimal(gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["Amountaa"]).ToString());
            }


            if (gvBooking.FocusedColumn.FieldName == "ProbDescription")
            {
                if (gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["WPStatus"]).ToString() == string.Empty)
                {
                    MessageBox.Show("Please Update Panel first before Booking a Problem");
                    return;
                }

                frmSDBBookingProblems Serv1Frm = new frmSDBBookingProblems(this);
                Serv1Frm.StartPosition = FormStartPosition.CenterScreen;
                Serv1Frm.lblCalendarDate = date;
                Serv1Frm.lblAuthID = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["authid"]).ToString();
                Serv1Frm.lblMainActID = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["mainactid"]).ToString();
                Serv1Frm.lblMiner = lblMinerID;
                Serv1Frm.lblWorkplace = WpNamelbl.Text;
                Serv1Frm.lblSubactID = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["subactid"]).ToString();
                Serv1Frm._UserCurrentInfoConnection = _UserCurrentInfoConnection;
                Serv1Frm.ShowDialog();

                gvBooking.SetRowCellValue(rowH, gvBooking.Columns["ProbDescription"], lblProblem);
                gvBooking.SetRowCellValue(rowH, gvBooking.Columns["ProbComments"], lblProbComment);

                int focusedRow = gvBooking.FocusedRowHandle;

                gvBooking.FocusedRowHandle = focusedRow;
                gvBooking.SetMasterRowExpanded(focusedRow, !gvBooking.GetMasterRowExpanded(focusedRow));

                Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);
            }
        }

        private void gvBooking_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "WPStatus")
            {
                string currStat = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["WPStatus"]).ToString();
                string workplace = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["workplace"]).ToString();
                string ABSNotes = string.Empty;

                if (string.IsNullOrEmpty(gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ABSNote"]).ToString()))
                {
                    ABSNotes = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ABSNote"]).ToString();
                }

                if (currStat == "PB")
                {
                    frmABS ABS = new frmABS(this);
                    ABS.WPlabel35.Text = workplace;
                    ABS.Rowlabel = gvBooking.FocusedRowHandle.ToString();//Section;
                    ABS.Actlabel35 = "SDB";
                    ABS.Commentstxt.Text = ABSNotes;
                    ABS.PicLbl = "0";
                    ABS.Catlabel35 = "PB";
                    ABS.DPdate.Value = DPdate.Value;
                    ABS.loadPic();
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ExtraABSP"], "PB");
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ABSP"], "B");
                    ABS.StartPosition = FormStartPosition.CenterScreen;
                    ABS.ShowDialog();
                }

                if (currStat == "B")
                {
                    frmABS ABS = new frmABS(this);
                    ABS.WPlabel35.Text = workplace;
                    ABS.Rowlabel = gvBooking.FocusedRowHandle.ToString();
                    ABS.Actlabel35 = "SDB";
                    ABS.Commentstxt.Text = ABSNotes;
                    ABS.PicLbl = "0";
                    ABS.Catlabel35 = "B";
                    ABS.DPdate.Value = DPdate.Value;
                    ABS.loadPic();
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ExtraABSP"], "B");
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ABSP"], "B");
                    ABS.StartPosition = FormStartPosition.CenterScreen;
                    ABS.ShowDialog();
                }

                if (currStat == "PS")
                {
                    frmABS ABS = new frmABS(this);
                    ABS.WPlabel35.Text = workplace;
                    ABS.Rowlabel = gvBooking.FocusedRowHandle.ToString();//Section;
                    ABS.Actlabel35 = "SDB";// NotesTxt.Text;
                    ABS.Commentstxt.Text = ABSNotes;
                    ABS.PicLbl = "0";
                    ABS.Catlabel35 = "PS";
                    ABS.DPdate.Value = DPdate.Value;
                    ABS.loadPic();
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ExtraABSP"], "PS");
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ABSP"], "S");
                    ABS.StartPosition = FormStartPosition.CenterScreen;
                    ABS.ShowDialog();
                }

                if (currStat == "S")
                {
                    frmABS ABS = new frmABS(this);
                    ABS.WPlabel35.Text = workplace;
                    ABS.Rowlabel = gvBooking.FocusedRowHandle.ToString();//Section;
                    ABS.Actlabel35 = "SDB";// NotesTxt.Text;
                    ABS.Commentstxt.Text = ABSNotes;
                    ABS.PicLbl = "0";
                    ABS.Catlabel35 = "S";
                    ABS.DPdate.Value = DPdate.Value;
                    ABS.loadPic();
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ExtraABSP"], "S");
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ABSP"], "S");
                    ABS.StartPosition = FormStartPosition.CenterScreen;
                    ABS.ShowDialog();
                }

                if (currStat == "A")
                {
                    frmABS ABS = new frmABS(this);
                    ABS.WPlabel35.Text = workplace;
                    ABS.Rowlabel = gvBooking.FocusedRowHandle.ToString();//Section;
                    ABS.Actlabel35 = "SDB";// NotesTxt.Text;
                    ABS.Commentstxt.Text = ABSNotes;
                    ABS.PicLbl = "0";
                    ABS.Catlabel35 = "A";
                    ABS.DPdate.Value = DPdate.Value;
                    ABS.loadPic();
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ExtraABSP"], "A");
                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["ABSP"], "A");
                    ABS.StartPosition = FormStartPosition.CenterScreen;
                    ABS.ShowDialog();
                }

            }
        }

        private void repWorkplaceStatus_EditValueChanged(object sender, EventArgs e)
        {
            gvBooking.PostEditor();
        }

        private void gvBooking_ShowingEditor(object sender, CancelEventArgs e)
        {
            string Editname = string.Empty;

            string ss = gvBooking.FocusedColumn.FieldName.ToString();
            string measuretype = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["measureType"]).ToString();

            if (ss == "AmountMeters")
                Editname = "Meters";

            if (ss == "AmountTime")
                Editname = "Time";

            if (ss == "AmountCompleted")
                Editname = "% Complete";

            if (ss == "AmountQuantity")
                Editname = "Quantity";

            if (ss == "AmountSQM")
                Editname = "SQM";

            if (ss == "AmountYesNo")
                Editname = "Yes/No";

            if (Editname != measuretype && ss != "WPStatus")
            {
                e.Cancel = true;
            }
        }

        private void DPdate_ValueChanged(object sender, EventArgs e)
        {
            LoadBookingGrid();
        }
    }
}