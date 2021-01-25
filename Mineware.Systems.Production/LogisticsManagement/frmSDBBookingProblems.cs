using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class frmSDBBookingProblems : DevExpress.XtraEditors.XtraForm
    {
        public frmBookingServices BookingServFrm;

        public frmSDBBookingProblems(frmBookingServices _BookingServ)
        {
            InitializeComponent();
            BookingServFrm = _BookingServ;
        }

        public frmSDBBookingProblems()
        {
            InitializeComponent();
        }

        #region private variables
        DataTable dtProblems = new DataTable();
        string lblProblem;
        #endregion

        #region public variables
        public string _UserCurrentInfoConnection;
        public string lblCalendarDate;
        public string lblAuthID;
        public string lblMainActID;
        public string lblMiner;
        public string lblWorkplace;
        public string lblSubactID;
        #endregion

        private void frmSDBBookingProblems_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
            _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
            _dbManMain.SqlStatement = "  Select (problemid + ':'+Description) Description,ProblemGroupCode  from (  SELECT *  FROM [dbo].[tbl_SDB_PROBLEM])a   ";
            _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManMain.ExecuteInstruction();

            dtProblems = _dbManMain.ResultsDataTable;

            foreach (DataRow dr in dtProblems.Rows)
            {
                if (dr["ProblemGroupCode"].ToString() == "15.BackFill")
                {
                    lbxBackfill.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "1.Safety & Env.")
                {
                    lbxSafetyEnv.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "10.Geology")
                {
                    lbxGeology.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "11.Services")
                {
                    lbxServices.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "12.Trackless")
                {
                    lbxTrackless.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "13.Drilling")
                {
                    lbxDrilling.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "14.Drill Rig")
                {
                    lbxDrillRig.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "16.Shafts")
                {
                    lbxShafts.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "2.Support")
                {
                    lbxSupport.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "3.Mining")
                {
                    lbxMining.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "4.Blasting")
                {
                    lbxBlasting.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "5.Cleaning")
                {
                    lbxCleaning.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "6.Logistics")
                {
                    lbxLogistics.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "7.Labour")
                {
                    lbxLabour.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "8.Equip. Fail")
                {
                    lbxEquipFail.Items.Add(dr["Description"].ToString());
                }

                if (dr["ProblemGroupCode"].ToString() == "9.Materials")
                {
                    lbxMaterials.Items.Add(dr["Description"].ToString());
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string date = String.Format("{0:yyyy-MM-dd}", lblCalendarDate);

            if (lblProblem == "Problem")
            {
                MessageBox.Show("Please Select a Problem");
                return;
            }

            BookingServFrm.lblProblem = ProductionGlobal.ProductionGlobal.ExtractAfterColon(lblProblem);
            BookingServFrm.lblProbComment = Commentstxt.Text;

            Global.sysNotification.TsysNotification.showNotification("Data Added", "Record Added Succesfully", Color.CornflowerBlue);

            Close();
        }

        private void lbxServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxServices.SelectedItem.ToString();
        }

        private void lbxBlasting_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxBlasting.SelectedItem.ToString();
        }

        private void lbxBackfill_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxBackfill.SelectedItem.ToString();
        }

        private void lbxTrackless_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxTrackless.SelectedItem.ToString();
        }

        private void lbxGeology_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxGeology.SelectedItem.ToString();
        }

        private void lbxDrilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxDrilling.SelectedItem.ToString();
        }

        private void lbxDrillRig_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxDrillRig.SelectedItem.ToString();
        }

        private void lbxShafts_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxShafts.SelectedItem.ToString();
        }

        private void lbxSafetyEnv_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxSafetyEnv.SelectedItem.ToString();
        }

        private void lbxCleaning_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxCleaning.SelectedItem.ToString();
        }

        private void lbxLogistics_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxLogistics.SelectedItem.ToString();
        }

        private void lbxLabour_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxLabour.SelectedItem.ToString();
        }

        private void lbxSupport_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxSupport.SelectedItem.ToString();
        }

        private void lbxMining_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxMining.SelectedItem.ToString();
        }

        private void lbxEquipFail_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxEquipFail.SelectedItem.ToString();
        }

        private void lbxMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProblem = lbxMaterials.SelectedItem.ToString();
        }
    }
}