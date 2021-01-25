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
    public partial class frmVampingProp : DevExpress.XtraEditors.XtraForm
    {
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;

        DataTable dtMiners = new DataTable();
        DataTable dtGang = new DataTable();
        DataTable dtBoxhole = new DataTable();


        public frmVampingProp()
        {
            InitializeComponent();
        }

        public string ExtractAfterColon(string TheString)
        {
            string AfterColon;

            int index = TheString.IndexOf(":"); // Kry die postion van die :

            AfterColon = TheString.Substring(index + 1); // kry alles na :

            return AfterColon;
        }

        public string ExtractBeforeColon(string TheString)
        {
            if (TheString != "")
            {
                string BeforeColon;
                int index = TheString.IndexOf(":");
                BeforeColon = TheString.Substring(0, index);
                return BeforeColon;
            }
            else
            {
                return "";
            }
        }

        public DataView Search(DataTable SearchTable, string SearchString)
        {
            DataView dv = new DataView(SearchTable);
            string SearchExpression = null;

            if (!String.IsNullOrEmpty(SearchString))//(Filtertxt.Text))
            {

                SearchExpression = string.Format("'{0}%'", SearchString);//Filtertxt.Text);
                dv.RowFilter = "Description like " + SearchExpression;
            }

            //DataTable dtResult = 
            //MessageBox.Show(SearchTable.Rows.Count.ToString());
            return dv;
        }

        private void Closebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmVampingProp_Load(object sender, EventArgs e)
        {
            LoadMiners();
            LoadGang();
            loadBoxhole();

            MWDataManager.clsDataAccess _dbMan11 = new MWDataManager.clsDataAccess();
            _dbMan11.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan11.SqlStatement = "Select distinct  SectionID,BH,OrgunitDS from tbl_PLANNING_Vamping where workplaceid = '"+ExtractBeforeColon(WorkplaceEdit.EditValue.ToString()) +"' and prodmonth = '"+lblprodmonth.Text+"' ";

            _dbMan11.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan11.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan11.ExecuteInstruction();

            DataTable dt = _dbMan11.ResultsDataTable;

            foreach (DataRow dr in dt.Rows)
            {
                //Miners
                for (int i = 0; i < lbxMiners.Items.Count; i++)
                {
                    if (ExtractBeforeColon(lbxMiners.Items[i].ToString()) == dr["SectionID"].ToString())
                    {
                        lbxMiners.SelectedIndex = i;
                    }
                }

                //Gang
                for (int i = 0; i < lbxGang.Items.Count; i++)
                {
                    if (lbxGang.Items[i].ToString() == dr["OrgunitDS"].ToString())
                    {
                        lbxGang.SelectedIndex = i;
                    }
                }

                //Boxhole
                for (int i = 0; i < lbxBoxHole.Items.Count; i++)
                {
                    if (ExtractBeforeColon(lbxBoxHole.Items[i].ToString()) == dr["BH"].ToString())
                    {
                        lbxBoxHole.SelectedIndex = i;
                    }
                }
            }

        }


        private void LoadMiners()
        {
            MWDataManager.clsDataAccess _dbMan11 = new MWDataManager.clsDataAccess();
            _dbMan11.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan11.SqlStatement = "Select distinct s.SectionID+':'+s.name Section,s.SectionID+':'+s.name Description  from tbl_section s, tbl_section s1 , tbl_section s2  \r\n" +
                                    "where s.Prodmonth = s1.Prodmonth and s.ReportToSectionid = s1.SectionID  \r\n"+
                                    "and s1.Prodmonth = s2.Prodmonth and s1.ReportToSectionid = s2.SectionID  \r\n"+
                                    "and s.Prodmonth = '"+lblprodmonth.Text+"' and s1.ReportToSectionid = '"+lblsection.Text+"' ";

            _dbMan11.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan11.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan11.ExecuteInstruction();

            dtMiners = _dbMan11.ResultsDataTable;

            foreach (DataRow r in dtMiners.Rows)
            {
                lbxMiners.Items.Add(r["Section"].ToString());
            }
        }

        private void LoadGang()
        {
            MWDataManager.clsDataAccess _dbMan1 = new MWDataManager.clsDataAccess();
            _dbMan1.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan1.SqlStatement = "  select gangno , GangName, gangno + ':' + GangName Description from tbl_EmployeeAll e  group by GangNo , gangname  \r\n" +
                                   "  order by GangNo ";
            _dbMan1.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan1.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan1.ResultsTableName = "sysset";
            _dbMan1.ExecuteInstruction();

            dtGang = _dbMan1.ResultsDataTable;

            foreach (DataRow drOrg in dtGang.Rows)
            {
                lbxGang.Items.Add(drOrg["gangno"].ToString() + ':' + drOrg["gangname"].ToString());
            }
        }

        private void loadBoxhole()
        {
            MWDataManager.clsDataAccess _dbMan2 = new MWDataManager.clsDataAccess();
            _dbMan2.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
            _dbMan2.SqlStatement = " select OreFlowID + ':' + Name Description, * from tbl_oreflowentities where " +
                                   "oreflowcode = 'BH' order by LevelNumber, name";
            _dbMan2.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan2.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan2.ResultsTableName = "sysset";
            _dbMan2.ExecuteInstruction();

            dtBoxhole = _dbMan2.ResultsDataTable;

            foreach (DataRow r in dtBoxhole.Rows)
            {
                lbxBoxHole.Items.Add(r["OreFlowID"].ToString() + ":" + r["Name"].ToString());
            }
        }

        private void lblWorkplace_TextChanged(object sender, EventArgs e)
        {
            WorkplaceEdit.EditValue = lblWorkplace.Text;
        }

        private void Savebtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (lbxMiners.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a Miner");
                return;
            }

            if (lbxGang.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a Gang");
                return;
            }

            if (lbxBoxHole.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select a Boxhole");
                return;
            }

            lblSaved.Text = "Y";

            this.Close();
        }

        private void TotWpFilterTxt_TextChanged(object sender, EventArgs e)
        {
            lbxMiners.Items.Clear();

            string zzzz = "*" + TotWpFilterTxt.Text;


            foreach (DataRowView r in Search(dtMiners, zzzz))
            {
                //if (r["workplaceid"].ToString() == "")
                    lbxMiners.Items.Add(r["description"].ToString());
                //else
                //    lbxMiners.Items.Add(r["workplaceid"].ToString() + ":" + r["description"].ToString());

            }
        }

        private void GangFilter_TextChanged(object sender, EventArgs e)
        {
            lbxGang.Items.Clear();

            string zzzz = "*" + GangFilter.Text;

            foreach (DataRowView r in Search(dtGang, zzzz))
            {
                //if (r["workplaceid"].ToString() == "")
                    lbxGang.Items.Add(r["description"].ToString());
                //else
                //    lbxGang.Items.Add(r["workplaceid"].ToString() + ":" + r["description"].ToString());

            }
        }

        private void BoxholeFilter_TextChanged(object sender, EventArgs e)
        {
            lbxBoxHole.Items.Clear();

            string zzzz = "*" + BoxholeFilter.Text;

            foreach (DataRowView r in Search(dtBoxhole, zzzz))
            {
                //if (r["workplaceid"].ToString() == "")
                    lbxBoxHole.Items.Add(r["description"].ToString());
                //else
                //    lbxBoxHole.Items.Add(r["workplaceid"].ToString() + ":" + r["description"].ToString());

            }
        }
    }
}