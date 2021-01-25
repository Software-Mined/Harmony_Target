using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class frmEquipmentProperties : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Global parameters
        /// </summary>
        public string EquipmentID;
        public string _ConnectionString;
        public string Edit;
        Procedures procs = new Procedures();

        public frmEquipmentProperties()
        {
            InitializeComponent();
        }

        void LoadProblems()
        {
            if (Edit == "Y" && EquipmentID != string.Empty)
            {

                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = _ConnectionString;
                _dbMan.SqlStatement = " select * from dbo.tbl_Equipement where EquipmentID = '"+ EquipmentID + "'";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                DataTable dt = _dbMan.ResultsDataTable;

                foreach (DataRow dr in dt.Rows)
                {
                    txtEquipmentID.Text = dr["EquipmentID"].ToString();
                    txtEquipmentName.Text = dr["EquipmentName"].ToString();
                    txtEquipmentType.Text = dr["EquipmentType"].ToString();
                    txtEquipmentMake.Text = dr["EquipmentMake"].ToString();
                    if(dr["EquipmentValid"].ToString()== "0")
                    {
                        radYes.Checked = false;
                        radNo.Checked = true;
                    }
                    if (dr["EquipmentValid"].ToString() == "1")
                    {
                        radYes.Checked = true;
                        radNo.Checked = false;
                    }
                }
            }
           

            

        }

        private void frmProblemProperties_Load(object sender, EventArgs e)
        {
            LoadProblems();
            txtEquipmentType.SelectedIndex = -1;

            if(Edit == "Y")
            {
                txtEquipmentID.Enabled = false;
            }
          

        }

        void SaveEquipment()
        {
            int IsValid = 1;
            //do checks
            if (txtEquipmentID.Text == string.Empty)
            {
                MessageBox.Show("Please enter the Problem Id", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtEquipmentName.Text == string.Empty)
            {
                MessageBox.Show("Please enter the Problem Description", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtEquipmentType.Text == string.Empty)
            {
                MessageBox.Show("Please select the Enquirer Id", "Insufficient information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }         
          
          
            if (radYes.Checked == true)
            {
                IsValid = 1;
            }
            else
            {
                IsValid = 0;
            }



            if (Edit == "N")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = _ConnectionString;
                _dbMan.SqlStatement = " insert into tbl_Equipement  " +
                                      " VALUES ( '" + txtEquipmentID.Text.ToString() + "', '" + txtEquipmentName.Text.ToString() + "', " +
                                      " '" + txtEquipmentType.Text.ToString() + "', '" + txtEquipmentMake.Text.ToString() + "',   " +
                                      " '" + IsValid + "' )";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
            }
            else
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = _ConnectionString;

                _dbMan.SqlStatement = " update tbl_Equipement set EquipmentID = '" + txtEquipmentID.Text.ToString() + "', EquipmentName = '" + txtEquipmentName.Text.ToString() + "', " +
                                      " EquipmentType =  '" + txtEquipmentType.Text.ToString() + "', EquipmentMake =   '" + txtEquipmentMake.Text.ToString() + "', " +
                                      " EquipmentValid  = '" + IsValid + "'";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

            }

            //if (EditID == "")
            //{
            //    MessageFrm MsgFrm = new MessageFrm();
            //    MsgFrm.Text = "Record Inserted";
            //    Procedures.MsgText = "Problem Added";
            //    MsgFrm.Show();
            //}
            //else
            //{
            //    MessageFrm MsgFrm = new MessageFrm();
            //    MsgFrm.Text = "Record Updated";
            //    Procedures.MsgText = "Problem Updated";
            //    MsgFrm.Show();
            //}            
            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "Equipment Saved.", Color.CornflowerBlue);
        }


        private void btnSaveProb_Click_1(object sender, EventArgs e)
        {
            SaveEquipment();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radYes_CheckedChanged(object sender, EventArgs e)
        {
            if (radYes.Checked == true)
            {
                radNo.Checked = false;
            }
            else
            {
                radNo.Checked = true;
            }
        }

        private void radNo_CheckedChanged(object sender, EventArgs e)
        {
            if (radYes.Checked == true)
            {
                radNo.Checked = false;
            }
            else
            {
                radNo.Checked = true;
            }
        }
    }
}