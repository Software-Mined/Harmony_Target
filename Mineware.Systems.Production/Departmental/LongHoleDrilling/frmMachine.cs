using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.LongHoleDrilling
{
    public partial class frmMachine : DevExpress.XtraEditors.XtraForm
    {
        public string _connection;
        DataTable UnMachine = new DataTable();
        DataTable PMachine = new DataTable();
        public frmMachine()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void frmMachine_Load(object sender, EventArgs e)
        {
            LoadMachines();
        }
        void LoadMachines()
        {
            //loaded = "N";
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = _connection;
            _dbMan.SqlStatement = "select distinct(machineno) mach from vw_LongHoleDrilling_PlanLongTerm where machineno not in (select [EquipmentID] from tbl_Equipement where EquipmentType = 'Raisebore' )   ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "order by mach ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
            UnMachine = _dbMan.ResultsDataTable;

            LstUnMachines.DataSource = UnMachine;
            LstUnMachines.DisplayMember = "mach";
            LstUnMachines.ValueMember = "mach";


             MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = _connection;
            _dbMan2.SqlStatement = "select [EquipmentID] mach from tbl_Equipement where EquipmentType = 'Raisebore'  ";
            _dbMan2.SqlStatement = _dbMan2.SqlStatement + "order by mach ";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ExecuteInstruction();

            PMachine = _dbMan2.ResultsDataTable;

            LstPMachines.DataSource = PMachine;
            LstPMachines.DisplayMember = "mach";
            LstPMachines.ValueMember = "mach";




            //// load codes
            //_dbMan.ConnectionString = _connection;
            //_dbMan.SqlStatement = "select Code+':'+CodeDescription code from MW_PassStageDB.dbo.tbl_GeoScience_Codes ";
            //_dbMan.SqlStatement = _dbMan.SqlStatement + "order by Codeorder ";
            //_dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbMan.ExecuteInstruction();
            //DrillCodes = _dbMan.ResultsDataTable;

            //lstCodes.DataSource = DrillCodes;
            //lstCodes.DisplayMember = "code";
            //lstCodes.ValueMember = "code";


        }

        private void LstPMachines_Click(object sender, EventArgs e)
        {
            int selectedIndex = LstPMachines.SelectedIndex;

            Object selectedItem = LstPMachines.SelectedItem;

            LblChg2.Text = LstPMachines.SelectedValue.ToString();
        }

        private void LstUnMachines_Click(object sender, EventArgs e)
        {
            int selectedIndex = LstUnMachines.SelectedIndex;

            Object selectedItem = LstUnMachines.SelectedItem;

            LblChg2.Text = LstUnMachines.SelectedValue.ToString();
        }

        private void LblChg2_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = true;
            TxtMPerShift.Visible = true;
            chBNightShift.Visible = true;

            //label4.Visible = true;
            // GridCycle.Visible = true;
            // LstCycleCode.Visible = true;
            //BtnSaveMachine.Visible = true;
            // label5.Visible = true;
            // label6.Visible = true;
            // label7.Visible = true;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = _connection;
            _dbMan.SqlStatement = "select * from tbl_LongHoleDrilling_Machine where machineno = '" + LblChg2.Text + "'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "order by machineno ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable MMAc = _dbMan.ResultsDataTable;

            if (MMAc.Rows.Count > 0)
            {
                TxtMPerShift.Text = MMAc.Rows[0]["advpershift"].ToString();
                if (MMAc.Rows[0]["ns"].ToString() == "N")
                    chBNightShift.Checked = false;
                else
                    chBNightShift.Checked = true;

            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Convert.ToDecimal(TxtMPerShift.Text) < Convert.ToDecimal(0.0001))
            {
                MessageBox.Show("A metre per Day has to be captured", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ns = "N";
            if (chBNightShift.Checked == true)
                ns = "Y";

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = _connection;
            _dbMan.SqlStatement = " delete from tbl_LongHoleDrilling_Machine where machineno = '" + LblChg2.Text + "' insert into tbl_LongHoleDrilling_Machine values ('" + LblChg2.Text + "' " +
                                  " , '" + TxtMPerShift.Text + "' " +
                                  ", '', '" + ns + "' ) ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Info", "Data saved successfuly.", Color.CornflowerBlue);

            LoadMachines();
        }
    }
}