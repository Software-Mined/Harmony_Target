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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using Mineware.Systems.Global;
using DevExpress.XtraGrid.Views.Base;

namespace Mineware.Systems.Production.Departmental.LongHoleDrilling
{
    public partial class frmProp : DevExpress.XtraEditors.XtraForm
    {
        // public frmGeologyMain MainFrm1;
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;
        DataTable Holes = new DataTable();
        public frmProp()
        {
            InitializeComponent();
           // MainFrm1 = _MainFrm1;
        }
        private void frmGeologyProp_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan1.SqlStatement = "select 'Machine Commencement' Machine, 'Machine Commencement'Workplace, null RINGNAME,0 HoleNo,null SDate,null PrevMachineNo,null AddDelay,null sss, 1 Orderby union ";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + "select Distinct  m.Machine mm, p.Workplace,RINGNAME, Convert(int,HoleNo)HoleNo, SDate, PrevMachineNo, AddDelay, CONVERT(VARCHAR(11),SDate,106)  sss , 2 Orderby  ";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + "from [dbo].[tbl_LongHoleDrilling_PlanLongTerm] p inner join tbl_LongHoleDrilling_MachineSetup  m on p.Workplace = m.Workplace where  m.Machine = '"+Machlabel.Text+"' and SDate is not null Order by OrderBy, RingName, HoleNo ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();

            Holes = _dbMan1.ResultsDataTable;

            gridControl1.DataSource = Holes;
            colWorkplace.FieldName = "Workplace";
            colRing.FieldName = "RINGNAME";
            colHole.FieldName = "HoleNo";

            //if (Holes.Rows.Count > 0)
            //{
            //    gridView1.Se
            //}

            //PrevWPList.Items.Add("Machine Commencement");

            //foreach (DataRow row in Holes.Rows)
            //{
            //    PrevWPList.Items.Add(((row["Workplace"].ToString() + "                                                         ").Substring(0, 25) + row["HoleNo"].ToString() + "                              ").Substring(0, 50)) + row["HoleNo"].ToString() + "                              ").Substring(0, 50));
            //}

            //if (Holes.Rows.Count > 0)
            //{
            //    PrevWPList.SelectedIndex = 1;
            //}
        }

        private void PrevWPList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PrevWPList.Text != "Machine Commencement")
                PevHolelabel.Text = PrevWPList.Text.Substring(24, 26).Trim();
            else
                PevHolelabel.Text = "Machine Commencement";
        }

        private void PevHolelabel_TextChanged(object sender, EventArgs e)
        {
            Starttm.Enabled = true;
            if (PrevWPList.Text != "Machine Commencement")
            {
                MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
                _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan1.SqlStatement = "Select SDate+DaysReq+AddDelay EndDate from( ";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " select *, Convert(int, HoleLength/ AdvPerShift) DaysReq from(\r\n";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + "  select Workplace, p.RingName, p.HoleNo,p.HoleLength, SDate+AddDelay SDate, p.MachineNo, Convert(decimal(18,2),m.AdvPerShift)AdvPerShift, Convert(decimal(18, 2), p.Length)Length, p.AddDelay \r\n";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " from[dbo].[tbl_LongHoleDrilling_PlanLongTerm] p\r\n";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " inner join[tbl_LongHoleDrilling_Machine] m on p.MachineNo = m.MachineNo\r\n";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + " where p.SDate is not null)a )A\r\n";
                _dbMan1.SqlStatement = _dbMan1.SqlStatement + "where machineno = '" + Machlabel.Text + "' and holeno = '" + PevHolelabel.Text + "' and RingName = '" + Ringlabel.Text + "' ";
                _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan1.ExecuteInstruction();

                DataTable PevHoles = _dbMan1.ResultsDataTable;

                if (PevHoles.Rows.Count > 0)
                {
                    Starttm.Value = Convert.ToDateTime(PevHoles.Rows[0]["EndDate"].ToString()).AddDays(1);
                    Starttm.Enabled = false;

                }

            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan1.SqlStatement = "update [dbo].[vw_LongHoleDrilling_PlanLongTerm] set sdate = '" + String.Format("{0:yyyy-MM-dd}", Starttm.Value) + "'";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " ,adddelay = '" + Delaytxt.Text + "', prevworkplace =  '" + PevHolelabel.Text + "' ";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + "where machineno = '" + Machlabel.Text + "' and workplace = '" + WPlabel.Text + "'  and holeno = '" + Ringlabel.Text + "' ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();
            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Info", "Data saved successfuly.", Color.CornflowerBlue);
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);         
            _dbMan1.SqlStatement = "update [dbo].[tbl_LongHoleDrilling_PlanLongTerm] set sdate = '" + String.Format("{0:yyyy-MM-dd}", Starttm.Value) + "'";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + " ,adddelay = '" + Delaytxt.Text + "', prevworkplace =  '" + PevHolelabel.Text + "' ";
            _dbMan1.SqlStatement = _dbMan1.SqlStatement + "where machineno = '" + Machlabel.Text + "' and workplace = '" + WPlabel.Text + "'  and holeno = '" + Holelbl.Text + "' and ringname = '" + Ringlabel.Text + "' ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ExecuteInstruction();


            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = "exec sp_LongholeDrilling_InsertBookings '" + WPlabel.Text + "' , '" + Ringlabel.Text + "', '" + Holelbl.Text + "' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Info", "Data saved successfuly.", Color.CornflowerBlue);
            this.Close();
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (PrevWPList.Text != "Machine Commencement")
            {
                if (e.RowHandle == 0)
                {
                    PevHolelabel.Text = "Machine Commencement";
                }
                else
                {
                    PevHolelabel.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["HoleNo"].FieldName.ToString()).ToString();
                }
            }
            else
                PevHolelabel.Text = "Machine Commencement";
        }
    }
}
