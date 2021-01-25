using DevExpress.XtraEditors;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.SysAdmin
{
    public partial class ucSysAdmin_Problems : XtraUserControl
    {
        
        public ucSysAdmin_Problems()
        {
            InitializeComponent();        
            //ScreenType = GlobalExtensions.ScreenType.PopupScreen;          

        }

        private void ucSysAdmin_Problems_Load(object sender, EventArgs e)
        {
            LoadProblems();
            LoadProbGroups();
            //this.Top = 800;
        }

        void LoadProblems()
        {
            ///Load Problems data
            ///
            MWDataManager.clsDataAccess _dbProb = new MWDataManager.clsDataAccess();
            _dbProb.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbProb.SqlStatement = "Select p.ProblemID,p.ProblemGroup ProblemGroupCode, e.EnquirerID, \r\n " +
                                   "p.ProblemDesc Description, 0 ExtraInfo, 'Y' Valid, '' EmailGroup, '' HQCat, e.Description \r\n " +
                                   "from(select *, 'PRO' ENQUIRERid from tbl_Code_Problem_Main) p \r\n " +
                                   "left outer join tbl_Enquirer e on p.ENQUIRERid = e.ENQUIRERid \r\n " +
                                   "order by problemid";
            _dbProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbProb.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbProb.ExecuteInstruction();

            DataTable dtProb = _dbProb.ResultsDataTable;

            DataSet dsProbs = new DataSet();

            dsProbs.Tables.Add(dtProb);

            gcAdmProblems.DataSource = dsProbs.Tables[0];

            colAdmProbID.FieldName = "ProblemID";
            colAdmnProb.FieldName = "Description";
            colAdmnDept.FieldName = "EnquirerID";
            colExtraInfo.FieldName = "ExtraInfo";
            colValid.FieldName = "Valid";
            colEmailGrp.FieldName = "EmailGroup";
            colProbGrp.FieldName = "ProblemGroupCode";
            colHOGrp.FieldName = "HQCat";
        }

        void LoadProbGroups()
        {
            MWDataManager.clsDataAccess _dbProbGrp = new MWDataManager.clsDataAccess();
            _dbProbGrp.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbProbGrp.SqlStatement = " select * from (select case when substring(problemgroupcode,2,1) = '.' then " +
                                    "'0'+ problemgroupcode else problemgroupcode end as aa, * from tbl_Code_ProblemGroup) a " +
                                    "order by aa";
            _dbProbGrp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbProbGrp.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbProbGrp.ExecuteInstruction();

            DataTable dtProbGrp = _dbProbGrp.ResultsDataTable;
            DataSet dsProbGroup = new DataSet();

            dsProbGroup.Tables.Add(dtProbGrp);

            gcProbGroup.DataSource = dsProbGroup.Tables[0];

            colGrpID.FieldName = "ProblemGroupCode";
            colGrpName.FieldName = "Description";
            colGrpValid.FieldName = "Valid";

        }

        private void btnAddAdmn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmProblemProperties frmProp = new SysAdmin.frmProblemProperties();
            //frmProp.UserCurrentInfo = UserCurrentInfo.Connection;
            frmProp.Show();
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void ucSysAdmin_Problems_ActiveRibbonTabChanged(object sender, ActiveRibbonTabEventArgs e)
        {

        }
    }
}
