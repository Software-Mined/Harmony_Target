using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mineware.Systems.GlobalConnect;
using FastReport;
using FastReport.Export.Pdf;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.ProductionGlobal;
using System.Text.RegularExpressions;
using Mineware.Systems.Global;
using MWDataManager;

namespace Mineware.Systems.Production.Departmental.Vamping
{
	public partial class ucVampingBooking : BaseUserControl
	{		
        Procedures procs = new Procedures();
		string ProdMonth = "";
        private Color _colourA;
        private Color _colourB;
        private Color _colourS;
        public ucVampingBooking()
		{
			InitializeComponent();
            FormRibbonPages.Add(rpVampingBookings);
            FormActiveRibbonPage = rpVampingBookings;
            FormMainRibbonPage = rpVampingBookings;
            RibbonControl = rcVampingBookings;
        }

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
		
		private void btnShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			MWDataManager.clsDataAccess _dbManSB = new MWDataManager.clsDataAccess();
			_dbManSB.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
			_dbManSB.SqlStatement = "exec [sp_LoadVampingBooking] '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "', '" + ExtractBeforeColon(SectionEdit.EditValue.ToString()) + "'  ";
									//"and s.Prodmonth = '" + ProdmonthEdit.EditValue.ToString() + "' and s.sectionID = '" + ExtractBeforeColon(SectionEdit.EditValue.ToString()) + "'  ";

			_dbManSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
			_dbManSB.queryReturnType = MWDataManager.ReturnType.DataTable;
			_dbManSB.ExecuteInstruction();

			DataTable dtSB = _dbManSB.ResultsDataTable;

			if (dtSB.Rows.Count == 1)
			{
				MessageBox.Show("No Workplaces found for Section : '"+ SectionEdit.EditValue.ToString() + "'  ");
				return;
			}

			DataSet dsPlan = new DataSet();
			dsPlan.Tables.Add(dtSB);

			gcMoCycle.DataSource = dsPlan.Tables[0];

			colSection.FieldName = "Section";
			colWorkplace.FieldName = "Workplace";
			colActivity.FieldName = "Activity";
			colRiskRating.FieldName = "RiskRating";

			colCycDay1.FieldName = "Day1";
			colCycDay2.FieldName = "Day2";
			colCycDay3.FieldName = "Day3";
			colCycDay4.FieldName = "Day4";
			colCycDay5.FieldName = "Day5";
			colCycDay6.FieldName = "Day6";
			colCycDay7.FieldName = "Day7";
			colCycDay8.FieldName = "Day8";
			colCycDay9.FieldName = "Day9";
			colCycDay10.FieldName = "Day10";

			colCycDay11.FieldName = "Day11";
			colCycDay12.FieldName = "Day12";
			colCycDay13.FieldName = "Day13";
			colCycDay14.FieldName = "Day14";
			colCycDay15.FieldName = "Day15";
			colCycDay16.FieldName = "Day16";
			colCycDay17.FieldName = "Day17";
			colCycDay18.FieldName = "Day18";
			colCycDay19.FieldName = "Day19";
			colCycDay20.FieldName = "Day20";

			colCycDay21.FieldName = "Day21";
			colCycDay22.FieldName = "Day22";
			colCycDay23.FieldName = "Day23";
			colCycDay24.FieldName = "Day24";
			colCycDay25.FieldName = "Day25";
			colCycDay26.FieldName = "Day26";
			colCycDay27.FieldName = "Day27";
			colCycDay28.FieldName = "Day28";
			colCycDay29.FieldName = "Day29";
			colCycDay30.FieldName = "Day30";

			colCycDay31.FieldName = "Day31";
			colCycDay32.FieldName = "Day32";
			colCycDay33.FieldName = "Day33";
			colCycDay34.FieldName = "Day34";
			colCycDay35.FieldName = "Day35";
			colCycDay36.FieldName = "Day36";
			colCycDay37.FieldName = "Day37";
			colCycDay38.FieldName = "Day38";
			colCycDay39.FieldName = "Day39";
			colCycDay40.FieldName = "Day40";

			colCycDay41.FieldName = "Day41";
			colCycDay42.FieldName = "Day42";
			colCycDay43.FieldName = "Day43";
			colCycDay44.FieldName = "Day44";
			colCycDay45.FieldName = "Day45";
			colCycDay46.FieldName = "Day46";
			colCycDay47.FieldName = "Day47";
			colCycDay48.FieldName = "Day48";
			colCycDay49.FieldName = "Day49";
			colCycDay50.FieldName = "Day50";

			colD1.FieldName = "DCode1";
			colD2.FieldName = "DCode2";
			colD3.FieldName = "DCode3";
			colD4.FieldName = "DCode4";
			colD5.FieldName = "DCode5";
			colD6.FieldName = "DCode6";
			colD7.FieldName = "DCode7";
			colD8.FieldName = "DCode8";
			colD9.FieldName = "DCode9";
			colD10.FieldName = "DCode10";

			colD11.FieldName = "DCode11";
			colD12.FieldName = "DCode12";
			colD13.FieldName = "DCode13";
			colD14.FieldName = "DCode14";
			colD15.FieldName = "DCode15";
			colD16.FieldName = "DCode16";
			colD17.FieldName = "DCode17";
			colD18.FieldName = "DCode18";
			colD19.FieldName = "DCode19";
			colD20.FieldName = "DCode20";

			colD21.FieldName = "DCode21";
			colD22.FieldName = "DCode22";
			colD23.FieldName = "DCode23";
			colD24.FieldName = "DCode24";
			colD25.FieldName = "DCode25";
			colD26.FieldName = "DCode26";
			colD27.FieldName = "DCode27";
			colD28.FieldName = "DCode28";
			colD29.FieldName = "DCode29";
			colD30.FieldName = "DCode30";

			colD31.FieldName = "DCode31";
			colD32.FieldName = "DCode32";
			colD33.FieldName = "DCode33";
			colD34.FieldName = "DCode34";
			colD35.FieldName = "DCode35";
			colD36.FieldName = "DCode36";
			colD37.FieldName = "DCode37";
			colD38.FieldName = "DCode38";
			colD39.FieldName = "DCode39";
			colD40.FieldName = "DCode40";

			colD41.FieldName = "DCode41";
			colD42.FieldName = "DCode42";
			colD43.FieldName = "DCode43";
			colD44.FieldName = "DCode44";
			colD45.FieldName = "DCode45";
			colD46.FieldName = "DCode46";
			colD47.FieldName = "DCode47";
			colD48.FieldName = "DCode48";
			colD49.FieldName = "DCode49";
			colD50.FieldName = "DCode50";

			colProgPlan.FieldName = "ProgPlan";
			colProgBook.FieldName = "ProgBook";            
			colProgVar.FieldName = "ProgVar";

			MWDataManager.clsDataAccess _dbManDates = new MWDataManager.clsDataAccess();
			_dbManDates.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
			_dbManDates.SqlStatement = "Select min(CalendarDate) from tbl_PLANNING_Vamping where sectionid like '" + ExtractBeforeColon(SectionEdit.EditValue.ToString()) + "%' and Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "'  ";
			//"and s.Prodmonth = '" + ProdmonthEdit.EditValue.ToString() + "' and s.sectionID = '" + ExtractBeforeColon(SectionEdit.EditValue.ToString()) + "'  ";

			_dbManDates.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
			_dbManDates.queryReturnType = MWDataManager.ReturnType.DataTable;
			_dbManDates.ExecuteInstruction();

			DataTable dt = _dbManDates.ResultsDataTable;

			DateTime _StartDate = Convert.ToDateTime(dt.Rows[0][0].ToString());
			int columnIndex = 4;

			for (int i = 0; i < 50; i++)
			{
				string test = gvMoCycle.Columns[columnIndex].Caption;

				gvMoCycle.Columns[columnIndex].Caption = Convert.ToDateTime(_StartDate).ToString("dd MMM ddd");

				_StartDate = _StartDate.AddDays(1);
				columnIndex++;
			}
            
            getColoursForABS();
            rgColors.Visible = true;
            gcMoCycle.Visible = true;
		}

        private void getColoursForABS()
        {
            //Load Colours       
            clsDataAccess _dbColours = new clsDataAccess();
            _dbColours.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
            _dbColours.SqlStatement = "SELECT [A_Color], [B_Color], [S_Color] FROM [dbo].[tbl_sysSet]";
            _dbColours.queryExecutionType = ExecutionType.GeneralSQLStatement;
            _dbColours.queryReturnType = ReturnType.DataTable;
            _dbColours.ExecuteInstruction();

            _colourA = Color.FromArgb(Convert.ToInt32(_dbColours.ResultsDataTable.Rows[0][0]));
            _colourB = Color.FromArgb(Convert.ToInt32(_dbColours.ResultsDataTable.Rows[0][1]));
            _colourS = Color.FromArgb(Convert.ToInt32(_dbColours.ResultsDataTable.Rows[0][2]));

            aEdit.EditValue = _colourA;
            bEdit.EditValue = _colourB;
            sEdit.EditValue = _colourS;
        }


        private void ucVampingBooking_Load(object sender, EventArgs e)
		{
			MWDataManager.clsDataAccess _dbManDate = new MWDataManager.clsDataAccess();
			_dbManDate.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
			_dbManDate.SqlStatement = "select getdate() TheDate";
			_dbManDate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
			_dbManDate.queryReturnType = MWDataManager.ReturnType.DataTable;
			_dbManDate.ExecuteInstruction();
			DataTable dt1 = _dbManDate.ResultsDataTable;

			foreach (DataRow dr1 in dt1.Rows)
			{
				ServerDate.Value = Convert.ToDateTime(dr1["TheDate"].ToString());
				BookDateEdit.EditValue = Convert.ToDateTime(dr1["TheDate"].ToString());
			}

			MWDataManager.clsDataAccess _dbManSB = new MWDataManager.clsDataAccess();
			_dbManSB.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
			_dbManSB.SqlStatement = "Select CurrentProductionMonth,A_Color,B_Color,S_Color from tbl_SYSSET ";
			//"and s.Prodmonth = '" + ProdmonthEdit.EditValue.ToString() + "' and s.sectionID = '" + ExtractBeforeColon(SectionEdit.EditValue.ToString()) + "'  ";

			_dbManSB.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
			_dbManSB.queryReturnType = MWDataManager.ReturnType.DataTable;
			_dbManSB.ExecuteInstruction();

			DataTable dtSB = _dbManSB.ResultsDataTable;
			
			
			ProdmonthEdit.EditValue = procs.ProdMonthAsDate(_dbManSB.ResultsDataTable.Rows[0]["CurrentProductionMonth"].ToString());


			lblColorA.Text = dtSB.Rows[0][1].ToString();
			lblColorB.Text = dtSB.Rows[0][2].ToString();
			lblColorS.Text = dtSB.Rows[0][3].ToString();

			ProdmonthEdit_EditValueChanged(null, null);

			//btnShow_ItemClick(null,null);
		}

		private void gvMoCycle_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
		{
            if (e.Column.FieldName != "")
            {
                if (e.Column.AbsoluteIndex < 54)
                {
                    if (gvMoCycle.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString() == "OFF")
                    {
                        e.Appearance.BackColor = Color.LightGray;
                        e.Appearance.ForeColor = Color.LightGray;
                    }
                }

                if (e.Column.AbsoluteIndex < 54)
                {
                    if (gvMoCycle.GetRowCellValue(e.RowHandle, gvMoCycle.Columns["Workplace"]).ToString() == "Total Shiftboss")
                    {
                        e.Appearance.BackColor = Color.LightGray;

                        if (gvMoCycle.GetRowCellValue(e.RowHandle, gvMoCycle.Columns["Activity"]).ToString() == "-")
                        {

                            e.Appearance.BackColor = Color.LightSteelBlue;
                        }
                    }
                    else
                    {
                        if (e.Column.AbsoluteIndex < 54)
                        {
                            int columnIndex = e.Column.AbsoluteIndex + 50;
                            string value = gvMoCycle.GetRowCellValue(e.RowHandle, gvMoCycle.Columns[columnIndex]).ToString();

                            if (value == "A")
                            {
                                e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(lblColorA.Text));
                            }

                            if (value == "B")
                            {
                                e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(lblColorB.Text));
                            }

                            if (value == "PB")
                            {
                                e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(lblColorB.Text));
                            }

                            if (value == "S")
                            {
                                e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(lblColorS.Text));
                            }
                        }
                    }
                }


                if (e.Column.AbsoluteIndex < 54)
                {
                    if (gvMoCycle.GetRowCellValue(e.RowHandle, gvMoCycle.Columns["Workplace"]).ToString() == "Total Mine Overseer")
                    {
                        e.Appearance.BackColor = Color.LightSteelBlue;
                       
                    }
                }
             }
		}

		private void btnAddBooking_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			frmVampingBookingProp frm = new frmVampingBookingProp();
			frm.StartPosition = FormStartPosition.CenterScreen;
			frm.MOLbl.Text = ExtractBeforeColon(SectionEdit.EditValue.ToString());
			frm._theSystemDBTag = theSystemDBTag;
			frm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
			frm.UserCurrentInfo = UserCurrentInfo;
			frm.VampPMlabel.Text = procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString()));

			frm.OtherBookDate.EditValue = ServerDate.Value;

			frm.ShowDialog();

			btnShow_ItemClick(null, null);
		}

		private void BookDateEdit_EditValueChanged(object sender, EventArgs e)
		{
			ServerDate.Value = Convert.ToDateTime( BookDateEdit.EditValue);
		}

		private void btnPreplan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			//frmPreplanningEngInventory frm = new frmPreplanningEngInventory();
			//frm._theSystemDBTag = theSystemDBTag;
			//frm._UserCurrentInfoConnection = UserCurrentInfo.Connection;
			//frm.StartPosition = FormStartPosition.CenterScreen;
			//frm.Show();
		}

		private void RCRockEngineering_Click(object sender, EventArgs e)
		{

		}

		private void ProdmonthEdit_EditValueChanged(object sender, EventArgs e)
		{
			
			MWDataManager.clsDataAccess _dbManWP = new MWDataManager.clsDataAccess();
			_dbManWP.ConnectionString = TConnections.GetConnectionString(theSystemDBTag, UserCurrentInfo.Connection);
			_dbManWP.SqlStatement = "Select distinct s1.ReportToSectionid+':'+s2.name Section from tbl_section s, tbl_section s1 , tbl_section s2  \r\n" +
									"where s.Prodmonth = s1.Prodmonth and s.ReportToSectionid = s1.SectionID  \r\n" +
									"and s1.Prodmonth = s2.Prodmonth and s1.ReportToSectionid = s2.SectionID  \r\n" +
									"and s.Prodmonth = '" + procs.ProdMonthAsString(Convert.ToDateTime(ProdmonthEdit.EditValue.ToString())) + "' ";

			_dbManWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
			_dbManWP.queryReturnType = MWDataManager.ReturnType.DataTable;
			_dbManWP.ExecuteInstruction();

			DataTable dt = _dbManWP.ResultsDataTable;

			repositoryItemComboBox1.Items.Clear();

			foreach (DataRow dr in dt.Rows)
			{
				repositoryItemComboBox1.Items.Add(dr["Section"].ToString());
			}

			SectionEdit.EditValue = repositoryItemComboBox1.Items[0].ToString();
		}

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void gcMoCycle_Click(object sender, EventArgs e)
        {

        }
    }
}
