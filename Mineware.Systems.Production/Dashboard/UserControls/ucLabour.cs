using DevExpress.XtraEditors;
using Mineware.Systems.GlobalConnect;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Dashboard.UserControls
{
    public partial class ucLabour : XtraUserControl
    {
        #region private
        private DataTable dtGetData = new DataTable("dtGetData");
        private BackgroundWorker bgwDataLabour = new BackgroundWorker();
        private decimal TLClocked = 0;
        private decimal TLTotal = 0;
        private decimal TLClockedDaily = 0;
        private decimal TLTotalDaily = 0;
        //private decimal TLProgPer = 0;
        private decimal TLPerc = 0;
        private decimal TLTotalPerc = 0;

        private decimal RDOClocked = 0;
        private decimal RDOTotal = 0;
        private decimal RDOClockedDaily = 0;
        private decimal RDOTotalDaily = 0;
        private decimal RDOPerc = 0;
        private decimal RDOTotalPerc = 0;

        private decimal WOClocked = 0;
        private decimal WOTotal = 0;
        private decimal WOClockedDaily = 0;
        private decimal WOTotalDaily = 0;
        private decimal WOPerc = 0;
        private decimal WOTotalPerc = 0;
        #endregion

        public ucLabour()
        {
            InitializeComponent();
            bgwDataLabour.RunWorkerCompleted += bgwData_RunWorkerCompleted;
        }

        private void bgwData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TLPerc = 0;
            TLTotalPerc = 0;
            RDOPerc = 0;
            RDOTotalPerc = 0;

            foreach (DataRow dr in dtGetData.Rows)
            {
                if (dr["TeamLeaderTotal"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(dr["TeamLeaderTotal"].ToString()) > 0)
                    {
                        TLPerc = Math.Round((Convert.ToDecimal(dr["TeamLeaderUG"].ToString()) / Convert.ToDecimal(dr["TeamLeaderTotal"].ToString())) * 100, 0);
                        arcScaleComponent1.Value = Convert.ToInt16(TLPerc);
                        TLClockedDaily = Math.Round(Convert.ToDecimal(dr["TeamLeaderUG"].ToString()), 0);
                        TLTotalDaily = Math.Round(Convert.ToDecimal(dr["TeamLeaderTotal"].ToString()), 0);
                    }
                }

                if (dr["TLTotalProg"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(dr["TLTotalProg"].ToString()) > 0)
                    {
                        TLTotalPerc = Math.Round((Convert.ToDecimal(dr["TLUGProg"].ToString()) / Convert.ToDecimal(dr["TLTotalProg"].ToString())) * 100, 0);
                        TLClocked = Convert.ToDecimal(dr["TLUGProg"].ToString());
                        TLTotal = Convert.ToDecimal(dr["TLTotalProg"].ToString());
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////
                if (dr["RDOTotal"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(dr["RDOTotal"].ToString()) > 0)
                    {
                        RDOPerc = Math.Round((Convert.ToDecimal(dr["RDOUG"].ToString()) / Convert.ToDecimal(dr["RDOTotal"].ToString())) * 100, 0);
                        arcScaleComponent3.Value = Convert.ToInt16(RDOPerc);
                        RDOClockedDaily = Math.Round(Convert.ToDecimal(dr["RDOUG"].ToString()), 0);
                        RDOTotalDaily = Math.Round(Convert.ToDecimal(dr["RDOTotal"].ToString()), 0);
                    }
                }

                if (dr["RDOTotalProg"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(dr["RDOTotalProg"].ToString()) > 0)
                    {
                        RDOTotalPerc = Math.Round((Convert.ToDecimal(dr["RDOUGProg"].ToString()) / Convert.ToDecimal(dr["RDOTotalProg"].ToString())) * 100, 0);
                        RDOClocked = Convert.ToDecimal(dr["RDOUGProg"].ToString());
                        RDOTotal = Convert.ToDecimal(dr["RDOTotalProg"].ToString());
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////              
                if (dr["WOTotal"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(dr["WOTotal"].ToString()) > 0)
                    {
                        WOPerc = Math.Round((Convert.ToDecimal(dr["WOUG"].ToString()) / Convert.ToDecimal(dr["WOTotal"].ToString())) * 100, 0);
                        arcScaleComponent7.Value = Convert.ToInt16(WOPerc);
                        WOClockedDaily = Math.Round(Convert.ToDecimal(dr["WOUG"].ToString()), 0);
                        WOTotalDaily = Math.Round(Convert.ToDecimal(dr["WOTotal"].ToString()), 0);
                    }
                }

                if (dr["WOTotalProg"] != DBNull.Value)
                {
                    if (Convert.ToDecimal(dr["WOTotalProg"].ToString()) > 0)
                    {
                        WOTotalPerc = Math.Round((Convert.ToDecimal(dr["WOUGProg"].ToString()) / Convert.ToDecimal(dr["WOTotalProg"].ToString())) * 100, 0);
                        WOClocked = Convert.ToDecimal(dr["WOUGProg"].ToString());
                        WOTotal = Convert.ToDecimal(dr["WOTotalProg"].ToString());
                    }
                }

            }
            this.gaugeControl1.Enabled = true;
            this.gaugeControl2.Enabled = true;
            this.gaugeControl3.Enabled = true;
            this.gaugeControl4.Enabled = true;
        }



        private void LoadCriticalSkills()
        {

            this.Cursor = Cursors.WaitCursor;
            // bgwDataLabour.RunWorkerAsync();
            try
            {
                MWDataManager.clsDataAccess _dbManDate = new MWDataManager.clsDataAccess();
                _dbManDate.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbManDate.SqlStatement = " select Min(Calendardate) from tbl_Planning where prodmonth = ( \r\n";
                _dbManDate.SqlStatement = _dbManDate.SqlStatement + " select Min(Prodmonth) from tbl_Planning where calendardate = CONVERT(VARCHAR(10),GETDATE(),111)) ";
                _dbManDate.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManDate.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManDate.ExecuteInstruction();

                DateTime CalDate = Convert.ToDateTime(_dbManDate.ResultsDataTable.Rows[0][0].ToString());
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _dbMan.SqlStatement = " select SUM(teamleadertotal)teamleadertotal,SUM(TeamLeaderUG)TeamLeaderUG, \r\n" +
                                        " SUM(RDOTotal) RDOTotal,SUM(RDOUG) RDOUG, \r\n" +
                                        " SUM(WOTotal)WOTotal,SUM(WOUG)WOUG,  \r\n" +
                                        " SUM(TLTotalProg)TLTotalProg,SUM(TLUGProg)TLUGProg ,    \r\n" +
                                        " SUM(RDOTotalProg)RDOTotalProg,SUM(RDOUGProg)RDOUGProg,   \r\n" +
                                        " SUM(WOTotalProg)WOTotalProg,SUM(WOUGProg)WOUGProg  \r\n" +
                                        " from( Select  mo,  \r\n" +
                                        " case when OccDescription in ('Fix Term Mining Team Supervisor', 'Mining Team Supervisor')  then total else 0 end as teamleadertotal,    \r\n" +
                                        " case when OccDescription in ('Fix Term Mining Team Supervisor', 'Mining Team Supervisor') then UnderGround else 0 end as TeamLeaderUG ,    \r\n" +
                                        " case when OccDescription like '%rock Drill%'  then total else 0 end as RDOTotal,    \r\n" +
                                        " case when OccDescription like '%rock Drill%' then UnderGround else 0 end as RDOUG,    \r\n" +
                                        " case when OccDescription like '%winch Operator%'  then total else 0 end as WOTotal,    \r\n" +
                                        " case when OccDescription like '%winch Operator%' then UnderGround else 0 end as WOUG  \r\n" +
                                        " from  Critical_Skill ) c   \r\n" +
                                        " left outer join(  \r\n" +
                                        " select MO, SUM(TLtotal)TLTotalProg, SUM(TLUG) TLUGProg,  \r\n" +
                                        " SUM(RDOTotal) RDOTotalProg, SUM(RDOUG) RDOUGProg,  \r\n" +
                                        " SUM(WOTotal) WOTotalProg, SUM(WOUG) WOUGProg  \r\n" +
                                        " From( Select mo,  \r\n" +
                                        " case when OccDescription in ('Fix Term Mining Team Supervisor', 'Mining Team Supervisor')  then total else 0 end as TLtotal,  \r\n" +
                                        " case when OccDescription in ('Fix Term Mining Team Supervisor', 'Mining Team Supervisor') then UnderGround else 0 end as TLUG,     \r\n" +
                                        " case when OccDescription like '%rock Drill%' then total else 0 end as RDOTotal,    \r\n" +
                                        " case when OccDescription like '%rock Drill%' then UnderGround else 0 end as RDOUG,    \r\n" +
                                        " case when OccDescription like '%winch Operator%' then total else 0 end as WOTotal,    \r\n" +
                                        " case when OccDescription like '%winch Operator%' then UnderGround else 0 end as WOUG  \r\n" +
                                        " from dbo.tbl_Personal_CriticalSkill_Prog  \r\n" +
                                        " where ClockDate >= '" + CalDate.Date + "' \r\n" +
                                        " ) sub1  group by MO ) p on c.mo = p.MO ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();
                dtGetData = _dbMan.ResultsDataTable;
            }
            catch
            {
                dtGetData = null;
            }



        }

        private void ucGuages_Load(object sender, EventArgs e)
        {
            bgwDataLabour.RunWorkerAsync();
            this.gaugeControl1.Enabled = false;
            this.gaugeControl2.Enabled = false;
            this.gaugeControl3.Enabled = false;
            this.gaugeControl4.Enabled = false;
            LoadCriticalSkills();
        }
    }
}
