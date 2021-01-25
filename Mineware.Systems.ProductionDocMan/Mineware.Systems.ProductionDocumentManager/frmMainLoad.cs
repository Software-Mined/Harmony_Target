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
using Mineware.Systems.DocumentManager;
using System.Configuration;
using MWDataManager;
using Mineware.Systems.ProductionAmplatsDocMan.Actions;

namespace DocumentManager
{
    public partial class frmMainLoad : DevExpress.XtraEditors.XtraForm
    {
        private readonly clsGetSections _clsGetSections = new clsGetSections();
        public frmMainLoad()
        {
            InitializeComponent();
        }
        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            try
            {
                navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
            }
            catch { }
        }

        private void frmMainLoad_Load(object sender, EventArgs e)
        {            

            ///LoadUserRights
            MWDataManager.clsDataAccess _dbManUser = new MWDataManager.clsDataAccess();
            _dbManUser.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbManUser.SqlStatement = "  select u.UserID, UserName, SystemAdmin, Email, PasSectionid,   \r\n" +
                                     " RockMech , Survey, Sampling, UserCat, Site, ChiefAuth,   \r\n" +
                                     " ProdManAuth, HRManAuth, FinManAuth, ChiefPlanAuth ,  \r\n" +
                                     " s.Survey_SnrMineSurveyor,s.Survey_Geologist,s.Survey_RockMech,s.Survey_SnrMinePlanner,  \r\n" +
                                     " s.Survey_MO,s.Survey_SecMan,s.Survey_SurvPlanMan,s.Survey_ProdMan, (select currentproductionmonth from tbl_sysset) pm  \r\n" +
                                     " from tbl_Users u, tbl_Users_Synchromine s  \r\n" +
                                     " Where u.UserID = 'MINEWARE' and u.UserID = s.UserID  \r\n";
            _dbManUser.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManUser.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManUser.ExecuteInstruction();

            if (_dbManUser.ResultsDataTable.Rows.Count > 0)
            {
                clsUserInfo.UserID = _dbManUser.ResultsDataTable.Rows[0]["UserID"].ToString();
                clsUserInfo.UserName = _dbManUser.ResultsDataTable.Rows[0]["UserName"].ToString();               
                clsUserInfo.Email = _dbManUser.ResultsDataTable.Rows[0]["Email"].ToString();
                clsUserInfo.PasSectionid = _dbManUser.ResultsDataTable.Rows[0]["PasSectionid"].ToString();
                clsUserInfo.SystemAdmin = _dbManUser.ResultsDataTable.Rows[0]["SystemAdmin"].ToString();
                clsUserInfo.RockMech = _dbManUser.ResultsDataTable.Rows[0]["RockMech"].ToString();
                clsUserInfo.Survey = _dbManUser.ResultsDataTable.Rows[0]["Survey"].ToString();
                clsUserInfo.Sampling = _dbManUser.ResultsDataTable.Rows[0]["Sampling"].ToString();
                clsUserInfo.UserCat = _dbManUser.ResultsDataTable.Rows[0]["UserCat"].ToString();
                clsUserInfo.Site = _dbManUser.ResultsDataTable.Rows[0]["Site"].ToString();
                clsUserInfo.ChiefAuth = _dbManUser.ResultsDataTable.Rows[0]["ChiefAuth"].ToString();
                clsUserInfo.ProdManAuth = _dbManUser.ResultsDataTable.Rows[0]["ProdManAuth"].ToString();
                clsUserInfo.HRManAuth = _dbManUser.ResultsDataTable.Rows[0]["HRManAuth"].ToString();
                clsUserInfo.FinManAuth = _dbManUser.ResultsDataTable.Rows[0]["FinManAuth"].ToString();
                clsUserInfo.ChiefPlanAuth = _dbManUser.ResultsDataTable.Rows[0]["ChiefPlanAuth"].ToString();
                clsUserInfo.ProdMonth = _dbManUser.ResultsDataTable.Rows[0]["pm"].ToString();
                clsUserInfo.Survey_SnrMineSurveyor = _dbManUser.ResultsDataTable.Rows[0]["Survey_SnrMineSurveyor"].ToString();
                clsUserInfo.Survey_Geologist = _dbManUser.ResultsDataTable.Rows[0]["Survey_Geologist"].ToString();
                clsUserInfo.Survey_RockMech = _dbManUser.ResultsDataTable.Rows[0]["Survey_RockMech"].ToString();
                clsUserInfo.Survey_SnrMinePlanner = _dbManUser.ResultsDataTable.Rows[0]["Survey_SnrMinePlanner"].ToString();
                clsUserInfo.Survey_MO = _dbManUser.ResultsDataTable.Rows[0]["Survey_MO"].ToString();
                clsUserInfo.Survey_SecMan = _dbManUser.ResultsDataTable.Rows[0]["Survey_SecMan"].ToString();
                clsUserInfo.Survey_SurvPlanMan = _dbManUser.ResultsDataTable.Rows[0]["Survey_SurvPlanMan"].ToString();
                clsUserInfo.Survey_ProdMan = _dbManUser.ResultsDataTable.Rows[0]["Survey_ProdMan"].ToString();
              
                lblUser.Text = _dbManUser.ResultsDataTable.Rows[0]["UserName"].ToString();
            }
            else
            {
                MessageBox.Show("Please contact your system administrator to setup a user account.", "No User Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Ventilation
            if (clsUserInfo.SystemAdmin == "Y" || clsUserInfo.ChiefAuth == "Y" || clsUserInfo.ProdManAuth == "Y" || clsUserInfo.HRManAuth == "Y" || clsUserInfo.FinManAuth == "Y" || clsUserInfo.ChiefPlanAuth == "Y")
            {
                Mineware.Systems.DocumentManager.Ventilation.ucVentAuth newAuth = new Mineware.Systems.DocumentManager.Ventilation.ucVentAuth();
                VentNavigationPage.Controls.Add(newAuth);
                newAuth.Dock = DockStyle.Fill;
            }
            else { VentTileBarItem.Visible = false; };

            //Survey
            if (clsUserInfo.Survey == "Y" || clsUserInfo.SystemAdmin == "Y" || clsUserInfo.ChiefAuth == "Y" || clsUserInfo.ProdManAuth == "Y" || clsUserInfo.HRManAuth == "Y" || clsUserInfo.FinManAuth == "Y" || clsUserInfo.ChiefPlanAuth == "Y")
            {
                ucSurveyNote newAuthSurvey = new ucSurveyNote();
                SurveyNavigationPage.Controls.Add(newAuthSurvey);
                newAuthSurvey.Dock = DockStyle.Fill;
            }
            else { SurveyTileBarItem.Visible = false; };

            //Actions

            DataTable dt = _clsGetSections.get_UserBookSection(clsUserInfo.ProdMonth, clsUserInfo.UserID);
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbMan.SqlStatement = "exec sp_Update_OCR_SelectedSection";
            for (int x = 0; x < dt.Rows.Count; x++)
            {
                _dbMan.SqlStatement = _dbMan.SqlStatement + "  Update tbl_OCR_SelectedWorkplaces set selected = 'Y'   \r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "  Where Section =  '"+dt.Rows[x]["SectionID"].ToString() + "'  \r\n";
            }
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            ucLineActionManager_Report newAuthActions = new ucLineActionManager_Report();
            ActionsNavigationPage.Controls.Add(newAuthActions);
            newAuthActions.Dock = DockStyle.Fill;

            VentTileBarItem.Visible = false;
            SurveyTileBarItem.Visible = false;
            VentNavigationPage.PageVisible = false;
            SurveyNavigationPage.PageVisible = false;

            navigationFrame.SelectedPageIndex = 2;
        }

       

       

        private void frmMainLoad_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void tileBarItemExit_ItemClick(object sender, TileItemEventArgs e)
        {
            Application.Exit();
        }

        private void tileBarActions_ItemClick(object sender, TileItemEventArgs e)
        {

        }
    }
}