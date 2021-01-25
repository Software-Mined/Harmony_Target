using Mineware.Systems.GlobalConnect;
using Mineware.Systems.ProductionGlobal;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class frmSDBAddAuthorise : DevExpress.XtraEditors.XtraForm
    {
        public frmSDBAddAuthorise()
        {
            InitializeComponent();
        }

        #region private variables        
        string destinationFile = string.Empty;
        DataTable dtMiner = new DataTable();

        string SelectedSectionIDSecMan = string.Empty;
        string SelectedSectionIDMan = string.Empty;
        string SelectedSectionIDMO = string.Empty;
        string SelectedSectionIDSB = string.Empty;
        string SelectedSectionIDMiner = string.Empty;
        string FirstLoad = "Y";
        #endregion

        #region public variables
        public string _UserCurrentInfoConnection;
        public string lblFrmtype;
        public string lblAuthID;
        public string lblWorkplace;
        public string lblActivity;
        public string lblCrew;
        public string lblMiner;

        public DataTable dtGang = new DataTable();
        public DataTable dtActivity = new DataTable();
        public DataTable dtWorkplace = new DataTable();
        public DataTable dtMinerRec = new DataTable();
        #endregion

        public static string ExtractBeforeColon(string TheString)
        {
            if (TheString != string.Empty)
            {
                string BeforeColon;
                int index = TheString.IndexOf(":");
                BeforeColon = TheString.Substring(0, index);
                return BeforeColon;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ExtractAfterColon(string TheString)
        {
            string AfterColon = string.Empty;
            int index = TheString.IndexOf(":");
            AfterColon = TheString.Substring(index + 1);
            return AfterColon;
        }

        ProductionGlobalTSysSettings ProductionAmplatsGlobalTSysSettings = new ProductionGlobalTSysSettings();

        private void frmSDBAddAuthorise_Load(object sender, EventArgs e)
        {
            LoadMainAct();
            loadWp();
            LoadCrew();
            LoadMiner();

            if (lblFrmtype == "Edit")
            {
                lbxMainAct.Enabled = false;
                FilterActtxt.Enabled = false;
                FilterTasktxt.Enabled = false;
                DPEnddate.Enabled = false;

                for (int i = 0; i < lbxCrew.Items.Count; i++)
                {
                    if (ExtractBeforeColon(lbxCrew.Items[i].ToString()) == ExtractBeforeColon(lblCrew))
                    {
                        lbxCrew.SelectedIndex = i;
                    }
                }

                for (int i = 0; i < lbxMiner.Items.Count; i++)
                {
                    if (ExtractAfterColon(lbxMiner.Items[i].ToString()) == lblMiner)
                    {
                        lbxMiner.SelectedIndex = i;
                    }
                }

                for (int i = 0; i < lbxWP.Items.Count; i++)
                {
                    if (lbxWP.Items[i].ToString() == lblWorkplace)
                    {
                        lbxWP.SelectedIndex = i;
                    }
                }
            }

        }

        private void LoadMainAct()
        {
            DataTable dtSub1 = new DataTable();
            dtSub1 = dtActivity.Copy();
            DataSet ds = new DataSet();
            ds.Clear();
            ds.Tables.Clear();
            ds.Tables.Add(dtSub1);

            lbxMainAct.DataSource = ds.Tables[0];

            ds.Dispose();

            lbxMainAct.ValueMember = "Description";
        }

        private void loadWp()
        {
            DataTable dtSub = new DataTable();
            dtSub = dtWorkplace.Copy();
            DataSet ds = new DataSet();
            ds.Clear();
            ds.Tables.Clear();
            ds.Tables.Add(dtSub);

            lbxWP.DataSource = ds.Tables[0];
            lbxWP.ValueMember = "Workplaces";
        }

        private void LoadCrew()
        {
            DataTable dtSub = new DataTable();
            dtSub = dtGang.Copy();
            DataSet ds = new DataSet();
            ds.Clear();
            ds.Tables.Clear();
            ds.Tables.Add(dtSub);

            lbxCrew.DataSource = ds.Tables[0];
            lbxCrew.ValueMember = "crew";
        }

        private void LoadMiner()
        {
            DataTable dtMiner = new DataTable();
            dtMiner = dtMinerRec.Copy();
            DataSet ds = new DataSet();
            ds.Clear();
            ds.Tables.Clear();
            ds.Tables.Add(dtMiner);

            lbxMiner.DataSource = ds.Tables[0];
            lbxMiner.ValueMember = "Description";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbxMainAct_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcEndDate();
        }

        private void CalcEndDate()
        {
            if (lbxCrew.SelectedIndex == -1)
            {
                return;
            }

            if (lbxMainAct.SelectedIndex == -1)
            {
                return;
            }

            DateTime EndDateAuto;

            string Mainact = lbxMainAct.SelectedValue.ToString();
            string StartDate = String.Format("{0:yyyy-MM-dd}", DPStartdate.Value.Date);

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
            _dbMan.SqlStatement = " Select sum(Duration) WDCount from tbl_SDB_Activity_Schedule where MainAct = '" + Mainact + "'  ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            var Result = _dbMan.ExecuteInstruction();

            string mainactDur = "0";

            if (Result.success)
            {
                mainactDur = _dbMan.ResultsDataTable.Rows[0]["WDCount"].ToString();
            }

            MWDataManager.clsDataAccess _dbManCal = new MWDataManager.clsDataAccess();
            _dbManCal.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
            _dbManCal.SqlStatement = " Select * from tbl_Code_Calendar_Type \r\n" +
                                    "where  CalendarDate >= '" + StartDate + "' \r\n" +
                                    "and calendarcode = (select substring( max(sectionid) ,1,4) from tbl_Planning where CalendarDate = '" + StartDate + "') \r\n" +
                                    "order by CalendarDate asc  ";

            _dbManCal.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCal.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCal.ExecuteInstruction();

            DataTable dtCal = _dbManCal.ResultsDataTable;

            int countDays = 0;

            foreach (DataRow dr in dtCal.Rows)
            {
                if (dr["Workingday"].ToString() == "Y")
                {
                    countDays = countDays + 1;
                }

                if (countDays.ToString() == mainactDur)
                {
                    EndDateAuto = Convert.ToDateTime(dr["CalendarDate"]);
                    DPEnddate.Value = EndDateAuto;
                    break;
                }
            }
        }

        private void FilterActtxt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string filter = "%" + FilterActtxt.Text + "%";

                string slqwhere = "description like '%" + FilterActtxt.Text + "%' ";
                string slqorderby = "Description asc";

                DataTable dtSub = dtActivity.Select(slqwhere, slqorderby).CopyToDataTable();
                DataSet ds = new DataSet();

                ds.Tables.Add(dtSub);

                lbxMainAct.DataSource = ds.Tables[0];

                lbxMainAct.ValueMember = "Description";
            }
            catch (Exception)
            {

            }
        }

        private void FilterTasktxt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string slqwhere = "Workplaces like '%" + FilterTasktxt.Text + "%' ";
                string slqorderby = "Workplaces asc";

                DataTable dtSub = dtWorkplace.Select(slqwhere, slqorderby).CopyToDataTable();
                DataSet ds = new DataSet();

                ds.Tables.Add(dtSub);

                lbxWP.DataSource = ds.Tables[0];
                lbxWP.ValueMember = "Workplaces";
            }
            catch (Exception)
            {

            }
        }

        private void FilterCrewtxt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string slqwhere = "crew like '%" + FilterCrewtxt.Text + "%' ";
                string slqorderby = "crew asc";

                DataTable dtSub = dtGang.Select(slqwhere, slqorderby).CopyToDataTable(); ;
                DataSet ds = new DataSet();

                ds.Tables.Add(dtSub);

                lbxCrew.DataSource = ds.Tables[0];
                lbxCrew.ValueMember = "crew";
            }
            catch (Exception)
            {

            }
        }

        private void DPStartdate_ValueChanged(object sender, EventArgs e)
        {
            //CalcEndDate();
        }

        private void DPStartdate_CloseUp(object sender, EventArgs e)
        {
            CalcEndDate();
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (lbxMainAct.SelectedIndex < 0 && lblFrmtype != "Edit" && lblFrmtype != "EditMiner")
            {
                MessageBox.Show("Please Select a activity");
                return;
            }

            if (lbxWP.SelectedIndex < 0 && lblFrmtype != "Edit" && lblFrmtype != "EditMiner")
            {
                MessageBox.Show("Please Select a Workplace");
                return;
            }

            try
            {
                /////////////Kelvin New//////////////////
                //SelectedSectionIDProdMan = lbxCrew.SelectedItem.ToString().Substring(0, 1);
                SelectedSectionIDSecMan = lbxCrew.SelectedValue.ToString().Substring(0, 2);

                SelectedSectionIDMan = lbxCrew.SelectedValue.ToString().Substring(0, 3);
                SelectedSectionIDMO = lbxCrew.SelectedValue.ToString().Substring(0, 4);
                SelectedSectionIDSB = lbxCrew.SelectedValue.ToString().Substring(0, 5);
                SelectedSectionIDMiner = lbxCrew.SelectedValue.ToString().Substring(0, 6);

                ////Create section if doesnt exist///////
                MWDataManager.clsDataAccess _dbManSecCheck = new MWDataManager.clsDataAccess();
                _dbManSecCheck.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                _dbManSecCheck.SqlStatement = " Select sectionid from tbl_sdb_Section where SectionID = '" + SelectedSectionIDMiner + "' \r\n" +
                                      " and prodmonth = (select max(prodmonth) from tbl_sdb_Section) \r\n" +
                                      "   ";
                _dbManSecCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManSecCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
                var result = _dbManSecCheck.ExecuteInstruction();

                if (result.success == false)
                {
                    ///Insert Miner////
                    MWDataManager.clsDataAccess _dbManSecInsert = new MWDataManager.clsDataAccess();
                    _dbManSecInsert.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                    _dbManSecInsert.SqlStatement =

                                          "-----Man \r\n" +
                                          "  Begin try insert into tbl_sdb_Section values ( \r\n" +
                                          " (select max(prodmonth) from tbl_sdb_Section), \r\n" +
                                          " '" + SelectedSectionIDMan + "', \r\n" +
                                          " 'Section Manager " + SelectedSectionIDMan + "', \r\n" +
                                          " 'Section Manager " + SelectedSectionIDMan + "', \r\n" +
                                          " '" + SelectedSectionIDSecMan + "', \r\n" +
                                          " 'TUMELA LOW'  ) end try begin catch end catch\r\n" +

                                            "-----MO \r\n" +
                                          "  Begin try insert into tbl_sdb_Section values ( \r\n" +
                                          " (select max(prodmonth) from tbl_sdb_Section), \r\n" +
                                          " '" + SelectedSectionIDMO + "', \r\n" +
                                          " 'Mine Overseer " + SelectedSectionIDMO + "', \r\n" +
                                          " 'Mine Overseer " + SelectedSectionIDMO + "', \r\n" +
                                          " '" + SelectedSectionIDMan + "', \r\n" +
                                          " '4'  ) end try begin catch end catch\r\n" +

                                            "-----SB \r\n" +
                                          "  Begin try insert into tbl_sdb_Section values ( \r\n" +
                                          " (select max(prodmonth) from tbl_sdb_Section), \r\n" +
                                          " '" + SelectedSectionIDSB + "', \r\n" +
                                          " 'Shift Boss " + SelectedSectionIDSB + "', \r\n" +
                                          " 'Shift Boss " + SelectedSectionIDSB + "', \r\n" +
                                          " '" + SelectedSectionIDMO + "', \r\n" +
                                          " '5'  ) end try begin catch end catch\r\n" +

                                          "-----Miner \r\n" +
                                          " Begin try insert into tbl_sdb_Section values ( \r\n" +
                                          " (select max(prodmonth) from tbl_sdb_Section), \r\n" +
                                          " '" + SelectedSectionIDMiner + "', \r\n" +
                                          " 'Miner " + SelectedSectionIDMiner + "', \r\n" +
                                          " 'Miner " + SelectedSectionIDMiner + "', \r\n" +
                                          " '" + SelectedSectionIDSB + "', \r\n" +
                                          " '6'  ) end try begin catch end catch\r\n" +
string.Empty;

                    _dbManSecInsert.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _dbManSecInsert.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _dbManSecInsert.ExecuteInstruction();
                }
            }
            catch { return; }

            string StartDate = String.Format("{0:yyyy-MM-dd}", DPStartdate.Value.Date.ToString());

            string EndDate = String.Format("{0:yyyy-MM-dd}", DPEnddate.Value.Date.ToString());

            TimeSpan difference = Convert.ToDateTime(EndDate).Date - Convert.ToDateTime(StartDate).Date;
            int Duration = (int)difference.TotalDays;

            string gang = string.Empty;

            string Miner = string.Empty;

            if (lblFrmtype != "Edit")
            {
                if (string.IsNullOrEmpty(lbxCrew.SelectedValue.ToString()))
                {
                    gang = " ";
                }
                else
                {
                    gang = ProductionGlobal.ProductionGlobal.ExtractBeforeColon(lbxCrew.SelectedValue.ToString());
                }

                if (string.IsNullOrEmpty(SelectedSectionIDMiner))
                {
                    Miner = string.Empty;
                }
                else
                {
                    Miner = SelectedSectionIDMiner;
                }
            }



            if (lblFrmtype == "Edit")
            {
                MWDataManager.clsDataAccess _dbManPlanSum = new MWDataManager.clsDataAccess();
                _dbManPlanSum.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);

                _dbManPlanSum.SqlStatement = "Select * from tbl_SDB_Activity_PlanSum where authid = '" + lblAuthID + "'  ";

                _dbManPlanSum.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPlanSum.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPlanSum.ExecuteInstruction();

                string editOrigDate = String.Format("{0:yyyy-MM-dd}", DPOrigdate.Value.Date);

                string editStartdate = String.Format("{0:yyyy-MM-dd}", DPStartdate.Value.Date);

                TimeSpan Editdiff = Convert.ToDateTime(editStartdate).Date - Convert.ToDateTime(editOrigDate).Date;
                int EditDur = (int)Editdiff.TotalDays;

                MWDataManager.clsDataAccess _dbManPlanSumUpdate = new MWDataManager.clsDataAccess();
                _dbManPlanSumUpdate.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);

                _dbManPlanSumUpdate.SqlStatement = "update tbl_SDB_Activity_PlanSum set projectstart = '" + editStartdate + "',Workplace = '" + lbxWP.Text + "' where authid = '" + lblAuthID + "'  ";

                _dbManPlanSumUpdate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManPlanSumUpdate.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManPlanSumUpdate.ExecuteInstruction();

                Miner = ExtractBeforeColon(lbxMiner.SelectedItem.ToString());
                gang = lbxCrew.SelectedItem.ToString();

                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);

                _dbMan.SqlStatement = " update tbl_SDB_Activity_Authorisation set Crew = '" + gang + "',MinerID = (Select sectionid from tbl_sdb_Section where EmployeeName = '" + Miner + "' and prodmonth = '" + ProductionGlobalTSysSettings._currentProductionMonth + "'),StartDate = '" + StartDate + "',Workplace = '" + lbxWP.Text + "'  \r\n" +
                                      " where AuthID = '" + lblAuthID + "'  \r\n\r\n";
                _dbMan.SqlStatement = _dbMan.SqlStatement + "\r\n\r\n  exec sp_SDB_NewActivity_Schedule_OnCHangeAuthid '0', '" + lblAuthID + "'  exec  sp_SDB_UpdateProjectEnd  '" + lblAuthID + "' ";

                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
            }

            if (lblFrmtype == "Add")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);

                _dbMan.SqlStatement = " insert into tbl_SDB_Activity_Authorisation (AuthID,MainActID,Workplace,StartDate,EndDate,Duration,Authorise,UserID,AuthDate,DocShortCut,Crew,MinerID) \r\n" +
                                      "values (  (select top 1( 'AT' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(AuthID,3,8))+1+100000),3,8))  \r\n" +
                                      "  from (select * from tbl_SDB_Activity_Authorisation  \r\n" +
                                      "  union \r\n" +
                                      "  select 'AT0001', (Select MainActID from tbl_SDB_Activity  where Description = '" + lbxMainAct.SelectedValue.ToString() + "'),'" + lbxWP.Text + "','" + StartDate + "','" + EndDate + "'," + Duration + ",'N','" + TUserInfo.UserID + "','','" + destinationFile + "' ,'" + gang + "','" + Miner + "' )a order by AuthID desc)     \r\n" +
                                      ",(Select MainActID from tbl_SDB_Activity  where Description = '" + lbxMainAct.SelectedValue.ToString() + "'),'" + lbxWP.Text + "','" + StartDate + "','" + EndDate + "' ," + Duration + ",'N','" + TUserInfo.UserID + "','','" + destinationFile + "' \r\n" +
                                      " , '" + gang + "' ,  '" + Miner + "' )  ";

                _dbMan.SqlStatement = _dbMan.SqlStatement + "\r\n\r\n  exec sp_SDB_NewActivity_Schedule";

                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();


                MWDataManager.clsDataAccess _dbManMain = new MWDataManager.clsDataAccess();
                _dbManMain.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, _UserCurrentInfoConnection);
                _dbManMain.SqlStatement = _dbManMain.SqlStatement + "declare @AUthid varchar(10)  \r\n";

                _dbManMain.SqlStatement = _dbManMain.SqlStatement + "set @AUthid = (select Max(authid) from tbl_SDB_Activity_Authorisation )  \r\n";

                _dbManMain.SqlStatement = _dbManMain.SqlStatement + " exec sp_SDB_NewActivity_Schedule_OnCHangeAuthid '0',@AUthid  ";

                _dbManMain.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManMain.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManMain.ExecuteInstruction();
            }

            Close();
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}