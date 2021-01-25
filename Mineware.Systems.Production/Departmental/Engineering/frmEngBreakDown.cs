using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Mineware.Systems.GlobalConnect;

namespace Mineware.Systems.Production.Departmental.Engineering
{
    public partial class frmEngBreakDown : DevExpress.XtraEditors.XtraForm
    {
        public frmEngBreakDown()
        {
            InitializeComponent();
        }

        Mineware.Systems.Components.GlobalItems procs = new Mineware.Systems.Components.GlobalItems();

        DataTable dtWP = new DataTable();

        public string theSystemDBTag;
        public string UserCurrentInfo;

        private void loadWorkplace()
        {
            Wplistbox.Items.Clear();


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbMan.SqlStatement = "Select * from tbl_Workplace";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dtWP = _dbMan.ResultsDataTable;

            foreach (DataRow dr in dtWP.Rows)
            {
                Wplistbox.Items.Add(dr["Description"]);                
            }
        }


        private void frmEngBreakDown_Load(object sender, EventArgs e)
        {

            BDownTSEdit.EditValue = System.DateTime.Now.ToString("HH:mm");
            StartTime.EditValue = System.DateTime.Now.ToString("HH:mm");
            EndTime.EditValue = System.DateTime.Now.ToString("HH:mm");

            // load report by

            MWDataManager.clsDataAccess _dbManLab = new MWDataManager.clsDataAccess();
            _dbManLab.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);
            if (TUserInfo.Site == "Target")
            {
                _dbManLab.SqlStatement = "select userid + ':' + Name + LastName Employee from Syncromine_New.dbo.tblUsers ";
            }
            else { _dbManLab.SqlStatement = "select userid + ':' + Name + LastName Employee from [Syncromine_New].[dbo].tblUsers "; }
                

            _dbManLab.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManLab.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManLab.ExecuteInstruction();

            DataTable dt1Lab = _dbManLab.ResultsDataTable;


            sleReportto.Properties.DataSource = dt1Lab;
            sleReportto.Properties.ValueMember = "Employee";
            sleReportto.Properties.DisplayMember = "Employee";
            sleReportto.Properties.PopulateViewColumns();
            sleReportto.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            //sleReportto.Properties.View.Columns[0].Caption = "Employee";

            //foreach (DataRow dr in dt1Lab.Rows)
            //{
            //    Reporttocmd.Properties.Items.Add(dr["aa"].ToString());
            //}



            // load mach
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbMan.SqlStatement = "Select * from [dbo].[tbl_Eng_Equip_Inventory] order by EquipType, Equipno ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt1 = _dbMan.ResultsDataTable;

            foreach (DataRow dr in dt1.Rows)
            {
                Equipcmd.Properties.Items.Add(dr["Equipno"].ToString());
            }



            // load type

            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbMan.SqlStatement = "Select * from [dbo].tbl_Eng_Equip_BreakCat order by [UNCatID] ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;

            foreach (DataRow dr in dt.Rows)
            {
                Typecmd.Properties.Items.Add(dr["EquipType"].ToString());
            }

            // load location
            loadWorkplace();


            if (Equiplabe.Text != "No")
            {
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

                _dbMan.SqlStatement = "Select * from [dbo].[tbl_Eng_BreakDown] where EquipNo = '" + Equiplabe.Text + "' and ReportedDate = '" + Datelabel.Text + "' ";

                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                DataTable dtz = _dbMan.ResultsDataTable;

                foreach (DataRow dr in dtz.Rows)
                {
                    sleReportto.EditValue = dr["ReportedBy"].ToString();
                    Equipcmd.Text =  dr["EquipNo"].ToString();
                    Equipcmd.ReadOnly = true;
                    BDownDate.Value = Convert.ToDateTime(Datelabel.Text);
                    BDownDate.Enabled = false;
                    BDownTSEdit.EditValue = Datelabel.Text.Substring(12, 5);
                    BDownTSEdit.ReadOnly = true;

                    Typecmd.Text = dr["BDType"].ToString();

                    Wplistbox.Text = dr["location"].ToString();
                    Wplabel.Text = dr["location"].ToString();

                    if (dr["Damaged"].ToString() == "Y")
                        Damagedrgb.Checked = true;

                    Statuscmd.Text = dr["BDStatus"].ToString();

                    StartDate.Value = Convert.ToDateTime(dr["startdate"].ToString());
                    StartTime.EditValue = Convert.ToDateTime(dr["startdate"].ToString()).ToString("HH:mm");

                    EndDate.Value = Convert.ToDateTime(dr["Enddate"].ToString());
                    EndTime.EditValue = Convert.ToDateTime(dr["Enddate"].ToString()).ToString("HH:mm");

                    lbxNotes.Text = dr["Remarks"].ToString();

                }


            }

        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            string Damaged = "N";

            if (Damagedrgb.Checked == true)
                Damaged = "Y";


            string BDDate = BDownDate.Value.ToString("yyyy-MM-dd") + " " + BDownTSEdit.Text;

            string StartDate1 = StartDate.Value.ToString("yyyy-MM-dd") + " " + StartTime.Text;
            string EndDate1 = EndDate.Value.ToString("yyyy-MM-dd") + " " + EndTime.Text;

            DateTime ss = Convert.ToDateTime(StartDate.Value.ToString("yyyy-MM-dd") + " " + StartTime.Text);
            DateTime ee = Convert.ToDateTime(EndDate.Value.ToString("yyyy-MM-dd") + " " + EndTime.Text);

            int totalMinutes = 0;
            TimeSpan outresult = ee.Subtract(ss);
            totalMinutes = totalMinutes + ((ee.Subtract(ss).Days) * 24 * 60) + ((ee.Subtract(ss).Hours) * 60) + (ee.Subtract(ss).Minutes);
            //return totalMinutes;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();           
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);
            _dbMan.SqlStatement = "delete from [tbl_Eng_BreakDown] where EquipNo = '" + Equipcmd.Text + "' and ReportedDate = '" + BDDate + "' \r\n" +
                                  "insert into [tbl_Eng_BreakDown]  \r\n" +
                                  "(UNID, EquipNo, ReportedDate, ReportedBy, ReportedTo, BDType, Location, Damaged, BDStatus, StartDate, EndDate, Duration, Remarks)  \r\n" +
                                  "values(1, '" + Equipcmd.Text + "', '" + BDDate + "', '" + sleReportto.EditValue.ToString() + "', '" + TUserInfo.UserID + "',  \r\n" +
                                  "'" + Typecmd.Text + "', '" + Wplabel.Text + "', '" + Damaged + "', '" + Statuscmd.Text + "','" + StartDate1 + "','" + EndDate1 + "','" + totalMinutes+ "','" + lbxNotes.Text + "')      ";


            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();


            
            alertControl1.Show(this, "Information", "Record Saved Successfuly.");

            Close();

        }

        private void Filtertxt_TextChanged(object sender, EventArgs e)
        {
            string FilterString = "*" + Filtertxt.Text;

            Wplistbox.Items.Clear();

            //foreach (DataRowView dr in procs.Search(dtWP, FilterString))
            //{
            //    Wplistbox.Items.Add(dr["Workplace"].ToString());
            //}
        }

        private void Wplistbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Wplabel.Text = Wplistbox.SelectedItem.ToString();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void lbxNotes_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
