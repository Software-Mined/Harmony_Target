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

namespace Mineware.Systems.Production.Departmental.Vamping
{
    public partial class frmAddOldWp : XtraForm
    {

        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;

        public TUserCurrentInfo Userinfo;

        public frmAddOldWp()
        {
            InitializeComponent();
        }



        private void frmAddOldWp_Load(object sender, EventArgs e)
        {

            LoadVampWP();
        }


        public void LoadVampWP()
        {
            MWDataManager.clsDataAccess _dbManVampWP = new MWDataManager.clsDataAccess();
            _dbManVampWP.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManVampWP.SqlStatement = "select wt.GMSIWPID WorkplaceID,wt.Description,wt.Activity from tbl_Workplace_Total wt left outer join tbl_WORKPLACE w on w.Description = wt.Description where w.Description is null order by Description,Activity";
            _dbManVampWP.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManVampWP.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManVampWP.ExecuteInstruction();

            DataTable dtVampWp = new DataTable();

            dtVampWp = _dbManVampWP.ResultsDataTable;

            gcOldWorkplaces.DataSource = dtVampWp;


            //BindingSource bswp = new BindingSource();

            //bswp.DataSource = dtVampWp;

            //VampwplistBox.Items.Clear();
            //VampwplistBox.DataSource = bswp;
            //VampwplistBox.ValueMember = "Description";
            //VampwplistBox.DisplayMember = "Description";

            //NewWpLbl.Visible = false;
        }

        private void RockEnginSavebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Int32[] selectedRowHandles = gvOldWorkplaces.GetSelectedRows();
            if (selectedRowHandles.Length > 0)
            {

                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int selectedRowHandle = selectedRowHandles[i];
                    if (selectedRowHandle >= 0)
                    {
                        SaveWorkplace(gvOldWorkplaces.GetRowCellValue(selectedRowHandle, "WorkplaceID").ToString());
                    }
                }
                this.Close();
            }      
        }

        void SaveVampWP()
        {
            if (NewWpLbl.Visible == false)
            {
                MessageBox.Show("Please Select a workplace to activiate", "Enter Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

         

            MWDataManager.clsDataAccess _dbManVampCheck = new MWDataManager.clsDataAccess();
            _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);

            _dbManVampCheck.SqlStatement = "select CONVERT(varchar(1),activity) activity from  tbl_WORKPLACE where Description = '" + NewWpLbl.Text + "' ";
            _dbManVampCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManVampCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManVampCheck.ExecuteInstruction();

            DataTable DataVamp = _dbManVampCheck.ResultsDataTable;


            string act = DataVamp.Rows[0]["activity"].ToString();

  

            //if (act != "1")
            //{
            //    _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            //    _dbManVampCheck.SqlStatement = "Insert into workplace " +
            //                                  " select 'S'+substring(convert(varchar(10),1+b  + 100000),2,5) workplaceid, oreflowid, r.reefid, endtypeid, w.description, '0' act, " +
            //                                  "'R' reefwaste,  case when  gmsiwpid like '%200' then 'GN' else null end as stpcode, null, null, Line, w.direction, null, null, null, 0, 0, GMSIWPID, null  from WORKPLACE w, reef r, " +
            //                                  "(select max(convert(int,substring(workplaceid,2,5))) b from workplacetotal where Activity = 0) b " +
            //                                  "where w.reefid = r.shortdesc and w.Description = '" + NewWpLbl.Text + "'   ";
            //    _dbManVampCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManVampCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManVampCheck.ExecuteInstruction();

            //}
            //else
            //{
            //    _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            //    _dbManVampCheck.SqlStatement = "Insert into workplace " +
            //                                  "select 'D'+substring(convert(varchar(10),1+b  + 100000),2,5)  workplaceid, oreflowid, reefid, endtypeid, description, 1 act, " +
            //                                  " reefwaste,  case when  wpid like '%200' then 'GN' else null end as stpcode ,EndWidth , EndHeight, Line, direction, null, null, null, 0, 0, wpid, null  " +
            //                                  "  from ( " +
            //                                  " select row_number()over (order by w.description) a, " +
            //                                  "  oreflowid, r.reefid, e.endtypeid, w.description, w.GMSIWPID wpid, w.Line, w.direction, e.EndHeight, e.EndWidth, e.ReefWaste  " +

            //                                  "  from WORKPLACE w, reef r, ENDTYPE e " +
            //                                  " where  w.reefid = r.shortdesc and w.EndTypeID = e.ProcessCode and  w.activity = 1  and w.Description = '" + NewWpLbl.Text + "' ) a, " +

            //                                  " (select max(convert(int,substring(workplaceid,2,5))) b from workplace where Activity = 1) b ";
            //    _dbManVampCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //    _dbManVampCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
            //    _dbManVampCheck.ExecuteInstruction();
            //}

            Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("Data Saved", "saved", Color.CornflowerBlue);
            this.Close();
        }

        private void RockEnginAddImagebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void WpIDtxt_TextChanged(object sender, EventArgs e)
        {
           
        }
        string Workplace = "";
        string Workplaceid = "";
        private void VampwplistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Workplace = VampwplistBox.SelectedValue.ToString();
                //Workplaceid = VampwplistBox.SelectedValue.ToString();
                NewWpLbl.Visible = true;
            }
            catch
            {
                NewWpLbl.Visible = false;
                return;
            }
        }

        private void SearchControl1_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveWorkplaces()
        {
            DataTable table = ((DataView)gvOldWorkplaces.DataSource).Table;
            Int32[] selectedRowHandles = gvOldWorkplaces.GetSelectedRows();
            for (int i = 0; i < selectedRowHandles.Length; i++)
            {
                int selectedRowHandle = selectedRowHandles[i];
                if (selectedRowHandle >= 0)
                {

                    gvOldWorkplaces.GetRowCellValue(selectedRowHandle, "WorkplaceID").ToString();

                }


            }
        }

        private void SaveWorkplace(string WorkplaceID)
        {
            string UserID = Userinfo.UserID;
            MWDataManager.clsDataAccess _dbManVampCheck = new MWDataManager.clsDataAccess();
            _dbManVampCheck.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbManVampCheck.SqlStatement = " EXEC sp_workplace_Old_Workplaces_Insert '" + WorkplaceID + "','" + UserID + "'";
            _dbManVampCheck.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManVampCheck.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManVampCheck.ExecuteInstruction();

            if (_dbManVampCheck.ExecuteInstruction().success)
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("WP:" + WorkplaceID + " Saved", "saved", Color.CornflowerBlue);
            }
            else
            {
                Mineware.Systems.Global.sysNotification.TsysNotification.showNotification("WP:"+ WorkplaceID + " not Saved", "saved", Color.Red);
            }     
        }
    }
}