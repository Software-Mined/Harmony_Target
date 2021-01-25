using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class frmSystemSettings : DevExpress.XtraEditors.XtraForm
    {
        Procedures procs = new Procedures();
        public frmSystemSettings()
        {
            InitializeComponent();
        }

        private void frmSystemSettings_Load(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbMan.SqlStatement = " select Banner, MINECALLFACTOR, STOPINGPAYLIMIT, DEVELOPMENTPAYLIMIT,  \r\n" +
                                    " CurrentProductionMonth, ROCKDENSITY, RUNDATE, CURRENTMILLMONTH, SERVERPATH, BROKENROCKDENSITY, \r\n" +
                                    " PERCBLASTQUALIFICATION, REPDIR, Version, CheckMeas, A_Color, B_Color,S_Color, AdjBook \r\n" +
                                    " from tbl_SysSet \r\n" +
                                    "  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable SubB = _dbMan.ResultsDataTable;

            if (SubB.Rows.Count > 0)
            {
                txtBanner.EditValue = SubB.Rows[0]["Banner"].ToString();
                editProdmonth.EditValue = procs.ProdMonthAsDate(SubB.Rows[0]["CurrentProductionMonth"].ToString());
                MillEdit.EditValue = procs.ProdMonthAsDate(SubB.Rows[0]["CURRENTMILLMONTH"].ToString());
                RunDateEdit.EditValue = Convert.ToDateTime(SubB.Rows[0]["RUNDATE"].ToString());
                weekDaysEdit.EditValue = SubB.Rows[0]["CheckMeas"].ToString();
                RDSpinEdit.EditValue = Convert.ToDecimal(SubB.Rows[0]["ROCKDENSITY"].ToString());
                BRDSpinEdit.EditValue = Convert.ToDecimal(SubB.Rows[0]["BROKENROCKDENSITY"].ToString());
                spinEditBlastQual.EditValue = Convert.ToDecimal(SubB.Rows[0]["PERCBLASTQUALIFICATION"].ToString());
                VersionTxt.EditValue = SubB.Rows[0]["Version"].ToString();
                txtServer.EditValue = SubB.Rows[0]["SERVERPATH"].ToString();
                txtReport.EditValue = SubB.Rows[0]["REPDIR"].ToString();

                Color _colourA = Color.FromArgb(Convert.ToInt32(_dbMan.ResultsDataTable.Rows[0]["A_Color"]));
                Color _colourB = Color.FromArgb(Convert.ToInt32(_dbMan.ResultsDataTable.Rows[0]["B_Color"]));
                Color _colourS = Color.FromArgb(Convert.ToInt32(_dbMan.ResultsDataTable.Rows[0]["S_Color"]));

                colorEditA.EditValue = _colourA;
                colorEditB.EditValue = _colourB;
                colorEditS.EditValue = _colourS;

                if (SubB.Rows[0]["AdjBook"].ToString() == "Y")
                {
                    cbxProbRecon.Checked = true;
                }
                else
                {
                    cbxProbRecon.Checked = false;
                }
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveProb_Click(object sender, EventArgs e)
        {
            string AdjBook = "N";
            if (cbxProbRecon.Checked == true)
            {
                AdjBook = "Y";
            }
            else
            {
                AdjBook = "N";
            }
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _dbMan.SqlStatement = " UPDATE tbl_SysSet set Banner = '" + txtBanner.EditValue + "',  \r\n" +
                                    " CurrentProductionMonth = '" + procs.ProdMonthAsInt(Convert.ToDateTime(editProdmonth.EditValue)) + "', ROCKDENSITY = '" + RDSpinEdit.EditValue + "', \r\n" +
                                    " RUNDATE = '" + RunDateEdit.EditValue + "', CURRENTMILLMONTH = '" + procs.ProdMonthAsInt(Convert.ToDateTime(MillEdit.EditValue)) + "', \r\n" +
                                    " SERVERPATH = '" + txtServer.EditValue + "', BROKENROCKDENSITY = '" + BRDSpinEdit.EditValue + "', \r\n" +
                                    " PERCBLASTQUALIFICATION = '" + spinEditBlastQual.EditValue + "', REPDIR = '" + txtReport.EditValue + "', \r\n" +
                                    " Version = '" + VersionTxt.EditValue + "', CheckMeas = '" + weekDaysEdit.EditValue + "', \r\n" +
                                    " AdjBook = '" + AdjBook + "' \r\n" +
                                    // " A_Color = '" + Color.FromArgb(colorEditA..ToString()) + "', B_Color = '" + Color.FromArgb(colorEditB.EditValue) + "',S_Color = '" + Color.FromArgb(colorEditS.EditValue) + "'  \r\n" +
                                    "  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            Global.sysNotification.TsysNotification.showNotification("Data Saved", "System Settings Updated.", Color.CornflowerBlue);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}