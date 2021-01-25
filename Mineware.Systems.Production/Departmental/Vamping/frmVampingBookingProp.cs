using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Mineware.Systems.GlobalConnect;
using DevExpress.XtraEditors.Repository;

namespace Mineware.Systems.Production.Departmental.Vamping
{
	public partial class frmVampingBookingProp : DevExpress.XtraEditors.XtraForm
	{
		public frmVampingBookingProp()
		{
			InitializeComponent();
		}

		public string _theSystemDBTag;
		public string _UserCurrentInfoConnection;
		public TUserCurrentInfo UserCurrentInfo;
        private Dictionary<string, string> _problems = new Dictionary<string, string>();
        DataTable Neil = new DataTable();

		string _SyssetCheckMeasDay = "";

		public string ExtractAfterColon(string TheString)
		{
			string AfterColon;

			int index = TheString.IndexOf(":"); // Kry die postion van die :

			AfterColon = TheString.Substring(index + 1); // kry alles na :

			return AfterColon;
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

		private void Closebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			this.Close();
		}

		private void frmVampingBookingProp_Load(object sender, EventArgs e)
		{
			MWDataManager.clsDataAccess _dbManDian = new MWDataManager.clsDataAccess();
			_dbManDian.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
			_dbManDian.SqlStatement = "select getdate()+0.5 aa, * from tbl_sysset";
			_dbManDian.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
			_dbManDian.queryReturnType = MWDataManager.ReturnType.DataTable;
			_dbManDian.ExecuteInstruction();
			DataTable dt = _dbManDian.ResultsDataTable;

			ToDaydate.Value = Convert.ToDateTime(dt.Rows[0]["aa"].ToString());

			foreach (DataRow dr in dt.Rows)
			{
				label10.Text = dr["A_Color"].ToString();
				label11.Text = dr["B_Color"].ToString();
				label12.Text = dr["S_Color"].ToString();
				_SyssetCheckMeasDay = dr["CheckMeas"].ToString();
			}


			LoadVampingSec();
		}

		public void LoadVampingSec()
		{
			MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
			_dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);

			_dbMan.SqlStatement = " select s1.sectionid sec, s1.name nn,s1.sectionid+':'+s1.name Description   from  tbl_section s, tbl_section s1, tbl_workplace w,  " +
                "tbl_PLANNING_Vamping pm,vw_Seccal_totalShifts_Calc sc where pm.sectionid = s.sectionid and pm.prodmonth = s.prodmonth " +
				// "  and pm.Activity = 'VMP' " +
				"and s.reporttosectionid = s1.sectionid and s.prodmonth = s1.prodmonth and pm.workplaceid = w.workplaceid " +
				 " and s1.reporttosectionid = '" + MOLbl.Text + "'   " + //and pm.activity = 'VMP'
				 "and sc.Sectionid like '" + MOLbl.Text + "%' and sc.BeginDate <= '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' and sc.EndDate >= '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' " +
				 "and pm.Prodmonth = sc.Prodmonth group by s1.sectionid , s1.name order by s1.sectionid , s1.name ";

			_dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
			_dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
			_dbMan.ExecuteInstruction();

			Neil = _dbMan.ResultsDataTable;

			lbxMiners.Items.Clear();

			foreach (DataRow dr in Neil.Rows)
			{
				lbxMiners.Items.Add(dr["sec"].ToString() + ":" + dr["nn"].ToString());
			}
		}

		private void WPFiltertxt_TextChanged(object sender, EventArgs e)
		{

		}

		private void lbxMiners_DoubleClick(object sender, EventArgs e)
		{
			if (OneDayBackLbl.Text == "1")
			{
				//if (clsUserInfo.TempBackDateBooking == "N")
				//{
				//    if (OtherBookDate.Value < dttoday.Value)
				//    {
				//        MessageBox.Show("You can only book one day back", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//        return;
				//    }
				//}
			}

			if (Convert.ToDateTime(OtherBookDate.EditValue) > ToDaydate.Value)
			{
				MessageBox.Show("Future Bookings not allowed", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			SectLb.Text = ExtractBeforeColon(lbxMiners.SelectedItem.ToString());
			//OtherBookGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;
			LoadVamping();
		}



		public void LoadVamping()
		{

			ChckMeasDayLbl.Visible = false;




			//if (String.Format("{0:ddd}", OtherBookDate.Value) == _SyssetCheckMeasDay)
			//{
			//	//if (Convert.ToDecimal(ShiftLbl.Text) >= 2)
			//	// {
			//	ChckMeasDayLbl.Text = "Check Measurement Day";

			//	if (SysSettings.CHkMeasLevel == "MO")
			//		ShiftLbl.Text = "Check Measurement Day Mineoverseer";



			//	ChckMeasDayLbl.Visible = true;
			//	if (ChckMeasDayLbl.Text == "Check Measurement Day")
			//	{
			//		//gridStopingBook.Columns[11].Visible = true;
			//		//gridStopingBook.Columns[12].Visible = true;
			//	}
			//}
			//else
			//{
				// check mond was holiday
				MWDataManager.clsDataAccess _dbManCheck = new MWDataManager.clsDataAccess();
				_dbManCheck.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
				_dbManCheck.SqlStatement = "select  \r\n " +
											"convert(datetime,('" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "')) - (datepart(dw,'" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "')-2) dd, case when max(checkLastDay)  = 0 then 'N' else 'Y' end as WasTherePrevWorkingDay, \r\n " +
											"case when max(MonYesNo)  > 0 then 'Y' else 'N' end as WasTherePrevMonThisMonth  \r\n " +

											"from (  \r\n " +
											" select  \r\n " +
											"case when calendardate < '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' And workingday = 'Y' then 1 else 0  \r\n " +
											"end as checkLastDay,  \r\n " +
											"case when datepart(dw,calendardate)-2 = 1 and shiftday > 1 then 1 else 0 end as MonYesNo \r\n " +


                                             "from tbl_planning p, tbl_section s where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and  \r\n " +
											 "calendardate <= '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' and s.reporttosectionid = '" + SectLb.Text + "' and  \r\n " +
											"calendardate >= convert(datetime,('" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "')) - (datepart(dw,'" + String.Format("{0:yyyy-MM-dd}", dtBookDate.Value) + "')-2) \r\n " +
											"and \r\n " +
											 "p.prodmonth = (select max(p.prodmonth) from planning p, section s  \r\n " +
											 "where p.prodmonth = s.prodmonth and p.sectionid = s.sectionid and  \r\n " +
											 "calendardate = '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' and s.reporttosectionid = '" + SectLb.Text + "' )  \r\n " +
											") a  ";
				_dbManCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
				_dbManCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
				//_dbManCheck.ExecuteInstruction();

				////AAAAAAA

				DataTable NeilCheck = _dbManCheck.ResultsDataTable;





				MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
				_dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
				_dbMan.SqlStatement = "  select a.*, isnull(progbooksqm,0) progbooksqm1, rv.progsqmbook, isnull(progbookadv,0) progbookadv ,  isnull(adjsqmprev,0) adjsqmprev,  isnull(sqmleft,0) sqmleft, isnull(mofc,0) mofc ,CONVERT(decimal(18,1) , isnull(bookadv,0.0)) bookadv1 , madv  \r\n" +
					",isnull(progbookadv,0) + isnull(adjsqmprev,0)  ProgBookAdv, isnull(progbooksqm,0) + isnull(adjsqmprev,0)  ProgBookSqmNew, 0.0 inputMoFC    \r\n" +
					" ,CONVERT(decimal(18,0) ,isnull(BookContent,0)) BookContent1,CONVERT(decimal(18,0) , isnull(BookTons,0)) BookTons1,CONVERT(decimal(18,0) , isnull(booksqm,0)) booksqm1,isnull(bookprob,'') bookprob1,isnull(bookadv,0) bookadv1  \r\n" +
					" from (select pm.Sectionid sec,pm.Prodmonth,pm.OrgUnitds OrgUnit,w.Workplaceid + ':' + w.Description Workplace, w.activity act, w.Description, w.Workplaceid wp,convert(decimal(18,2),pm.planGrade) Grade,pm.planContent PlanContent, \r\n" +
					" pm.planadv PlanMeter,9 Activity,pm.planTons PlanTons, \r\n" +
					" bookcontent BookContent, \r\n" +
					" 0 BookMeter, \r\n" +
					" BookTons BookTons , PlanTonFact, PlanActivity, booksqm booksqm, Bookactivity, bookprob, abscode, absnote, bookadv  \r\n" +
                    " from tbl_section s, tbl_section s1, tbl_workplace w, \r\n" +
                    " tbl_PLANNING_Vamping pm \r\n" +
					//" left outer join (SELECT * FROM Booking_Vamping WHERE calendardate = '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.Value) + "')bv " +
					//" on pm.Prodmonth = bv.prodmonth and pm.Sectionid = bv.Sectionid and pm.Workplaceid = bv.Workplaceid " +
					//" and pm.Activity = bv.Activity " +
					" where pm.workingday = 'Y' and pm.sectionid = s.sectionid and pm.prodmonth = s.prodmonth \r\n" +
					//" and pm.Activity = 'VMP' " +
					" and s.reporttosectionid = s1.sectionid and s.prodmonth = s1.prodmonth and pm.workplaceid = w.workplaceid \r\n" +
					" and s.reporttosectionid = '" + SectLb.Text + "'  and calendardate =  '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "'  \r\n" +//and pm.activity = 'VMP' 
					" ) a \r\n" +

					" left outer join \r\n" +

					//" (select prodmonth pm1, WorkplaceID wp1, SectionID ss1, SUM(BookSqm) progbooksqm "+
					// " from  PLANNING_Vamping pm where calendardate <   '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.Value) + "'  group by prodmonth, WorkplaceID, SectionID) b " +
					// " on a.wp =b.wp1 and a.Prodmonth = b.pm1 "+

					" (select prodmonth pm1, pm.WorkplaceID wp1, pm.SectionID ss1, SUM(BookSqm) progbooksqm, SUM(Bookadv) progbookadv , SUM(adjsqm) adjsqmprev  from  \r\n" +
                    "   tbl_PLANNING_Vamping pm  \r\n" +
                    "   left outer join  tbl_PLANNING_Vamping_Recon rv  on pm.WorkplaceID = rv.workplaceid and pm.SectionID = rv.sectionid and  \r\n" +
					"   pm.CalendarDate = rv.calendardate \r\n" +


					 "  where pm.calendardate <   '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "'  group by pm.prodmonth, pm.WorkplaceID, pm.SectionID) b  on a.wp =b.wp1 and  \r\n" +
					 "  a.Prodmonth = b.pm1   \r\n" +

				" left outer join \r\n" +

                "  (select * from tbl_PLANNING_Vamping_Recon  where calendardate =  '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' )  rv on a.sec = rv.sectionid and a.wp = rv.workplaceid \r\n" +

			   "  left outer join  \r\n" +
                  "  ( select prodmonth, WorkplaceID, SectionID, SUM(plansqm) sqmleft  from tbl_PLANNING_Vamping \r\n" +
				 "   where CalendarDate >= '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "'  group by prodmonth, WorkplaceID, SectionID) fut on \r\n " +
				 "   a.Prodmonth = fut.Prodmonth and a.wp = fut.WorkplaceID and a.sec = fut.SectionID \r\n" +

				 "   left outer join \r\n" +

                 "   (select workplaceid wpa, vampmetres/(convert(decimal(18,8),vamptotalsqm)+0.00001)   madv from [dbo].[tbl_VampingPreInspectionSheet]) dd \r\n" +
				 "    on a.Description = dd.wpa";
				_dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
				_dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
				_dbMan.ExecuteInstruction();


				if (_dbMan.ResultsDataTable.Rows.Count == 0)
				{
					MessageBox.Show("No Planning has been entered for                                                    " +
					   "Section   " + SectLb.Text + "", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
					Cursor = Cursors.Default;
					return;
				}
				else
				{
					dtBooking.Enabled = true;
				}

				DataTable Neil = _dbMan.ResultsDataTable;

				DataSet ds = new DataSet();
				ds.Tables.Clear();
				ds.Tables.Add(Neil);

				dtBooking.DataSource = ds.Tables[0];

				colWorkplace.FieldName = "Workplace";
				colGang.FieldName = "OrgUnit";
				colGrade.FieldName = "Grade";
				colPlanAct.FieldName = "PlanActivity";
				colADV.FieldName = "bookadv1";
				colSQM.FieldName = "booksqm1";
				colTons.FieldName = "BookTons1";
				colContent.FieldName = "BookContent1";
				colBookingCode.FieldName = "Bookactivity";
				colVampingProblems.FieldName = "bookprob1";
				colStatus.FieldName = "abscode";
				colABSNotes.FieldName = "absnote";


				colProgBookadv.FieldName = "progbookadv";
				colReconMeas.FieldName = "progbooksqm1";
				colMoForeCast.FieldName = "inputMoFC";

				colRecMoFCAST.FieldName = "mofc";
				colRecProgBookSQM.FieldName = "ProgBookSqmNew";
				colRecReconMeas.FieldName = "progsqmbook";

				colsecondADV.FieldName = "madv";

				colPlantonFact.FieldName = "PlanTonFact";

				colSqmLeft.FieldName = "sqmleft";

				colSect.FieldName = "sec";

				if (ChckMeasDayLbl.Visible == true)
				{
					colRecMoFCAST.Visible = true;
					colRecReconMeas.Visible = true;
					colRecProgBookSQM.Visible = true;

					colRecMoFCAST.OptionsColumn.AllowEdit = false;

					colProgBookadv.Visible = true;
					colReconMeas.Visible = true;
					colMoForeCast.Visible = true;
				}

            ////Problems
            /////Add problem
            string prob = _dbMan.ResultsDataTable.Rows[0]["bookprob"].ToString();

            _problems.Clear();

            if (!string.IsNullOrWhiteSpace(prob))
            {
                //string probDesc = _dbMan.ResultsDataTable.Rows[0]["ProblemDesc"].ToString();
                //string SBNotes = _dbMan.ResultsDataTable.Rows[0]["SBossNotes"].ToString();
                _problems.Add(prob, "PR" + ";" + "");

                //lblProbNote.Text = probDesc;
                //lblProbNote.Visible = true;
            }
            else
            {
                //lblProbNote.Visible = false;
            }

            //Need to take out
            //ChckMeasDayLbl.Visible = true;
            //}


            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
			_dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
			_dbMan1.SqlStatement = "  select Code +':'+Description aa from tbl_Code_Cycle_Vamps " +
									"where Code not in ('','BR') order by code ";



			_dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
			_dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
			_dbMan1.ExecuteInstruction();

			DataTable Neil1111 = _dbMan1.ResultsDataTable;

			ritemBookingCode.Items.Clear();

			ritemBookingCode.Items.Add("BR:Brushing");
			ritemBookingCode.Items.Add("PR:Problem");

			foreach (DataRow r in Neil1111.Rows)
			{
				//cellSub.Items.Add(r["aa"].ToString());
				ritemBookingCode.Items.Add(r["aa"].ToString());
			}
		}

		private void OtherBookDate_ValueChanged(object sender, EventArgs e)
		{
			//LoadVampingSec();
		}

		private void OtherBookDate_CloseUp(object sender, EventArgs e)
		{
			LoadVampingSec();
		}

		private void gvBooking_DoubleClick(object sender, EventArgs e)
		{
			if (gvBooking.FocusedColumn.FieldName == "Workplace")
			{
				string _workplace = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["Workplace"]).ToString();
				string _ABSCode = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["Workplace"]).ToString();
				string _ABSNote = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["Workplace"]).ToString();


                // Need to bring in the ABS


                var ABSChoice = new frmBookingABS();
                //ABSChoice.UserCurrentInfo = UserCurrentInfo;
                ABSChoice.SectionID = MOLbl.Text;
                ABSChoice.WpID = ExtractAfterColon(_workplace);
                ABSChoice.Activity = 0;
                ABSChoice.WpID = ExtractBeforeColon(_workplace);
                ABSChoice.TheDate = DateTime.Now;
                ABSChoice.dtTheDate.EditValue = Convert.ToDateTime(OtherBookDate.EditValue);
                
                ABSChoice.connection = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection); 
                
               

                ABSChoice.StartPosition = FormStartPosition.CenterScreen;
                ABSChoice.ShowDialog();

                gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["abscode"], ABSChoice.absCode);
                gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["absnote"], ABSChoice.txtAdditionalNotes.Text);
            }
		}

		private void Savebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			if (ChckMeasDayLbl.Visible == true)
			{
				for (int k = 0; k <= gvBooking.RowCount - 1; k++)
				{
					if (gvBooking.GetRowCellValue(k, gvBooking.Columns["progsqmbook"]).ToString() == "")
					{
						MessageBox.Show("No Recon has been entered for " +
						  "Workplace                                       " + gvBooking.GetRowCellValue(k, gvBooking.Columns["Workplace"]).ToString() + "", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

						return;
					}


					if (gvBooking.GetRowCellValue(k, gvBooking.Columns["mofc"]).ToString() == "")
					{
						MessageBox.Show("No MO Forecast has been entered for " +
						  "Workplace                                       " + gvBooking.GetRowCellValue(k, gvBooking.Columns["Workplace"]).ToString() + "", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

						return;
					}

				}
			}


			MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
			_dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
			_dbMan.SqlStatement = "select '' a ";

			decimal sqmvar = 0;
			decimal Tonvar = 0;
			decimal Contvar = 0;

			for (int k = 0; k <= gvBooking.RowCount - 1; k++)
			{
				string bookactivity = gvBooking.GetRowCellValue(k, gvBooking.Columns["Bookactivity"]).ToString();
				string booksqm = gvBooking.GetRowCellValue(k, gvBooking.Columns["booksqm1"]).ToString();
				string booktons = gvBooking.GetRowCellValue(k, gvBooking.Columns["BookTons1"]).ToString();
				string bookadv = gvBooking.GetRowCellValue(k, gvBooking.Columns["bookadv1"]).ToString();
				string bookgrade = gvBooking.GetRowCellValue(k, gvBooking.Columns["Grade"]).ToString();
				string bookcontent = gvBooking.GetRowCellValue(k, gvBooking.Columns["BookContent1"]).ToString();
				string bookprob = gvBooking.GetRowCellValue(k, gvBooking.Columns["bookprob1"]).ToString();
				string abscode = gvBooking.GetRowCellValue(k, gvBooking.Columns["abscode"]).ToString();
				string absnote = gvBooking.GetRowCellValue(k, gvBooking.Columns["absnote"]).ToString();
				string Workplace = gvBooking.GetRowCellValue(k, gvBooking.Columns["Workplace"]).ToString();
				//string Gang = gvBooking.GetRowCellValue(k, gvBooking.Columns["Workplace"]).ToString();



				_dbMan.SqlStatement = _dbMan.SqlStatement + " update tbl_PLANNING_Vamping set bookactivity = '" + bookactivity + "', booksqm = '" + booksqm + "', booktons = '" + booktons + "', bookgrade = '" + bookgrade + "', \r\n";
				_dbMan.SqlStatement = _dbMan.SqlStatement + " bookcontent = '" + bookcontent + "' , bookprob = '" + bookprob + "' , abscode = '" + abscode + "', absnote = '" + absnote + "', BookAdv = '" + bookadv + "' \r\n";
				_dbMan.SqlStatement = _dbMan.SqlStatement + " where workplaceid = '" + ExtractBeforeColon(Workplace) + "' \r\n";
				_dbMan.SqlStatement = _dbMan.SqlStatement + " and calendardate = '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "'  \r\n\r\n";

				if (ChckMeasDayLbl.Visible == true)
				{
					string ReconMeas = gvBooking.GetRowCellValue(k, gvBooking.Columns["progsqmbook"]).ToString();
					string ProbBookSqm = gvBooking.GetRowCellValue(k, gvBooking.Columns["ProgBookSqmNew"]).ToString();
					string PlantonFact = gvBooking.GetRowCellValue(k, gvBooking.Columns["PlanTonFact"]).ToString();
					string Grade = gvBooking.GetRowCellValue(k, gvBooking.Columns["Grade"]).ToString();
					string sqmleft = gvBooking.GetRowCellValue(k, gvBooking.Columns["sqmleft"]).ToString();
					string RecMoFc = gvBooking.GetRowCellValue(k, gvBooking.Columns["mofc"]).ToString();
					string Section = gvBooking.GetRowCellValue(k, gvBooking.Columns["sec"]).ToString();

					sqmvar = Convert.ToDecimal(ReconMeas) - Convert.ToDecimal(ProbBookSqm);
					Tonvar = (Convert.ToDecimal(ReconMeas) - Convert.ToDecimal(ProbBookSqm)) * Convert.ToDecimal(PlantonFact);
					Contvar = (Convert.ToDecimal(ReconMeas) - Convert.ToDecimal(ProbBookSqm)) * Convert.ToDecimal(PlantonFact) * Convert.ToDecimal(Grade);

					_dbMan.SqlStatement = _dbMan.SqlStatement + " delete from tbl_PLANNING_Vamping_Recon \r\n";
					_dbMan.SqlStatement = _dbMan.SqlStatement + " where workplaceid = '" + ExtractBeforeColon(Workplace) + "' \r\n";
					_dbMan.SqlStatement = _dbMan.SqlStatement + " and calendardate = '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' \r\n\r\n";

					_dbMan.SqlStatement = _dbMan.SqlStatement + " insert into tbl_PLANNING_Vamping_Recon \r\n";
					_dbMan.SqlStatement = _dbMan.SqlStatement + " values ('" + ExtractBeforeColon(Workplace) + "',  '" + Section + "' \r\n";
					_dbMan.SqlStatement = _dbMan.SqlStatement + " , '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "', '" + ProbBookSqm + "', '" + ReconMeas + "' \r\n";
					_dbMan.SqlStatement = _dbMan.SqlStatement + " , '" + sqmvar + "', '" + Tonvar + "', '" + Contvar + "','" + sqmleft + "', 0, 0, '" + RecMoFc + "') \r\n\r\n";

				}

			}
			_dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
			_dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
			_dbMan.ExecuteInstruction();



			if (ChckMeasDayLbl.Visible == true)
			{
				string pm = VampPMlabel.Text;
				string mo = SectLb.Text;
				mo = mo.Substring(0, 4);
				mo = mo + "%";

				MWDataManager.clsDataAccess _dbManFact = new MWDataManager.clsDataAccess();
				_dbManFact.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
				_dbManFact.SqlStatement = "update [tbl_PLANNING_Vamping_Recon] set [Fact] = \r\n" +
									   "(select (isnull(SUM(booksqm),0)+isnull(SUM(adjsqm),0))/ (isnull(SUM(plansqm),0)+0.001) fact \r\n" +
									   "from (select  \r\n" +
									   "case when v.CalendarDate < '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' then v.PlanSqm else 0 end as plansqm, \r\n" +
									   "case when v.CalendarDate < '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' then v.BookSqm else 0 end as BookSqm, \r\n" +
                                       "AdjSqm  from tbl_PLANNING_Vamping v  \r\n" +
									   "left outer join  \r\n" +
                                       "dbo.tbl_PLANNING_Vamping_Recon rv on v.WorkplaceID = rv.workplaceid and v.SectionID = rv.sectionid and  \r\n" +
									   "v.CalendarDate = rv.calendardate \r\n" +

									   "where v.prodmonth = '" + pm + "' \r\n" +
									   "and v.sectionid like '" + mo + "' \r\n" +
									   "and v.CalendarDate <= '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' \r\n" +
									   ") a) " +
									   " where sectionid like '" + mo + "' and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' \r\n";
				_dbManFact.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
				_dbManFact.queryReturnType = MWDataManager.ReturnType.longNumber;
				_dbManFact.ExecuteInstruction();


				_dbManFact.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
				_dbManFact.SqlStatement = "update [tbl_PLANNING_Vamping_Recon] set [PerBasedFC] = " +
									   "([ProgSqmBook]) +([RemSqmPlan]*[Fact]) \r\n" +

									   " where sectionid like '" + mo + "' and CalendarDate = '" + String.Format("{0:yyyy-MM-dd}", OtherBookDate.EditValue) + "' ";
				_dbManFact.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
				_dbManFact.queryReturnType = MWDataManager.ReturnType.longNumber;
				_dbManFact.ExecuteInstruction();
			}

			LoadVampingSec();
		}

		private void ritemBookingCode_EditValueChanged(object sender, EventArgs e)
		{

		}

		private void ritemBookingCode_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void gvBooking_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
		{
			if (e.Column.FieldName == "Bookactivity")
			{
				if (gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["Bookactivity"]).ToString() == "PR:Problem")
				{
					string Workplace = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["Workplace"]).ToString();

					//var ProbBook = new frmBookingsABSProblems();
					//ProbBook.TheConnection = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
     //               ProbBook._UserCurrentInfoConnection = _UserCurrentInfoConnection;
     //               ProbBook._theSystemDBTag = _theSystemDBTag;
     //               ProbBook.TheSection = lbxMiners.SelectedItem.ToString();
					//ProbBook.TheWorkplace = Workplace;
					//ProbBook.TheActivity = Convert.ToInt32(0);
					//ProbBook.TheDate = DateTime.Now;
					//ProbBook.ShowDialog();

                    frmBookingsABSProblems ucProbs = new frmBookingsABSProblems();
                    //Setup the labels
                    ucProbs.TheWorkplace = Workplace;
                    ucProbs.TheSection = lbxMiners.SelectedItem.ToString(); 
                    ucProbs.TheDate = DateTime.Now;
                    ucProbs.TheActivity = Convert.ToInt32(0); 
                    ucProbs.NoteID = "NoteId";
                    ucProbs._UserCurrentInfoConnection = _UserCurrentInfoConnection;
                    ucProbs._theSystemDBTag = _theSystemDBTag;
                    ucProbs._UserCurrentInfoConnection = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection); 
                    ucProbs.Problems = _problems;

                    //if (!string.IsNullOrEmpty(lblProbNote.Text))
                    //    ucProbs.ProblemDesc = lblProbNote.Text;

                    ucProbs.ShowDialog(this);

                    //lblProbNote.Text = ProductionGlobal.ProductionGlobal.ExtractAfterColon(ucProbs.lbNoteID.Text);
                    //lblProbNote.Visible = true;


                    gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["bookprob1"], ucProbs.ProblemID);
					//gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["bookprob1"], ProbBook.ProblemID);
				}
			}

			if (e.Column.FieldName == "booksqm1" && lblChanged.Text == "N")
			{
				string adv = "0";
				
				//Get
				string SQM = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["booksqm1"]).ToString();


				string Madv = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["madv"]).ToString();


				string PlantonFact = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["PlanTonFact"]).ToString();
				string Grade = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["Grade"]).ToString();

				//Calc
				if (gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["madv"]).ToString() != "")
				{
					adv = String.Format("{0:0.0}", Convert.ToDecimal(SQM) * Convert.ToDecimal(Madv));
				}
				string tons = Convert.ToString(Math.Round(Convert.ToDecimal(SQM) * Convert.ToDecimal(PlantonFact), 0));
				string content = Convert.ToString(Math.Round(Convert.ToDecimal(SQM) * Convert.ToDecimal(PlantonFact) * Convert.ToDecimal(Grade), 0));

				//Set
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["bookadv1"], adv);
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["BookTons1"], tons);
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["BookContent1"], content);

				lblChanged.Text = "Y";
			}

			if (e.Column.FieldName == "bookadv1" && lblChanged.Text == "N")
			{
				lblChanged.Text = "Y";

				//Get
				string SQM = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["booksqm1"]).ToString();
				string adv = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["bookadv1"]).ToString();
				string Madv = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["madv"]).ToString();
				string PlantonFact = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["PlanTonFact"]).ToString();
				string Grade = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["Grade"]).ToString();

				string sqm = "";
				//Calc
				if (Madv != "")
				{
					sqm = Convert.ToString(Convert.ToDecimal(adv) / Convert.ToDecimal(Madv));
				}

				else
				{
					sqm = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["booksqm1"]).ToString();
				}

				string tons = Convert.ToString(Math.Round(Convert.ToDecimal(SQM) * Convert.ToDecimal(PlantonFact), 0));
				string content = Convert.ToString(Math.Round(Convert.ToDecimal(SQM) * Convert.ToDecimal(PlantonFact) * Convert.ToDecimal(Grade), 0));

				//Set
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["booksqm1"], sqm);
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["BookTons1"], tons);
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["BookContent1"], content);
			}

			if (e.Column.FieldName == "progsqmbook" && lblChanged.Text == "N")
			{
				lblChanged.Text = "Y";
				//Get
				string reconmeas = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["progsqmbook"]).ToString();
				string adv = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["bookadv1"]).ToString();

				//Calc
				string AdvReconMeas = String.Format("{0:0.0}", Convert.ToDecimal(reconmeas) * Convert.ToDecimal(adv));

				//Set
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["progbooksqm1"], AdvReconMeas);
			}

			if (e.Column.FieldName == "progbooksqm1" && lblChanged.Text == "N")
			{
				lblChanged.Text = "Y";
				//Get
				string advreconmeas = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["progbooksqm1"]).ToString();
				string adv = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["bookadv1"]).ToString();

				//Calc
				string ReconMeas = String.Format("{0:0.0}", Convert.ToDecimal(advreconmeas) * Convert.ToDecimal(adv));

				//Set
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["progsqmbook"], ReconMeas);
			}

			if (e.Column.FieldName == "mofc" && lblChanged.Text == "N")
			{
				lblChanged.Text = "Y";
				//Get
				string mofc = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["mofc"]).ToString();
				string adv = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["bookadv1"]).ToString();

				//Calc
				string mofcCalc = String.Format("{0:0.0}", Convert.ToDecimal(mofc) * Convert.ToDecimal(adv));

				//Set
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["inputMoFC"], mofcCalc);
			}

			if (e.Column.FieldName == "inputMoFC" && lblChanged.Text == "N")
			{
				lblChanged.Text = "Y";
				//Get
				string mofc = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["inputMoFC"]).ToString();
				string adv = gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["bookadv1"]).ToString();

				//Calc
				string mofcCalc = String.Format("{0:0.0}", Convert.ToDecimal(mofc) * Convert.ToDecimal(adv));

				//Set
				gvBooking.SetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["mofc"], mofcCalc);
			}
		}

		private void gvBooking_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
		{
			if (e.Column.FieldName == "abscode")
			{
				if (gvBooking.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString() == "Safe")
				{
					e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(label10.Text));
					e.Appearance.ForeColor = Color.Black;
				}

				if (gvBooking.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString() == "Un Safe")
				{
					e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(label11.Text));
					e.Appearance.ForeColor = Color.Black;
				}

				if (gvBooking.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString() == "No Vis.")
				{
					e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(label12.Text));
					e.Appearance.ForeColor = Color.Black;
				}
			}
		}

		private void gvBooking_ShownEditor(object sender, EventArgs e)
		{

		}

		private void gvBooking_ShowingEditor(object sender, CancelEventArgs e)
		{
			lblChanged.Text = "N";

			if (gvBooking.GetRowCellValue(gvBooking.FocusedRowHandle, gvBooking.Columns["abscode"]).ToString() == "")
			{
				MessageBox.Show("Please do you workplace safe declaration before Booking an activity");
				e.Cancel = true;
			}
		}

		private void labelControl3_Click(object sender, EventArgs e)
		{

		}
	}
}