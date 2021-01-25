using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Ventillation
{
    public partial class frmChecklistWorkplaces : DevExpress.XtraEditors.XtraForm
    {
        public frmChecklistWorkplaces()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pubic parameter
        /// </summary>
        public string checklistName;
        public string _theSystemDBTag;
        public string _UserCurrentInfo;

        /// <summary>
        /// Load Workplaces into the list boxes
        /// </summary>

        void loadWorkplaces()
        {

            ///Load Checklist workplaces
            ///
            string checklistWp = "Select Description Des from tbl_WORKPLACE \r\n " +
                " where workplaceid in (Select WorkplaceID from tbl_Dept_Inspection_VentOther_Workplaces where Category = '" + checklistName + "') order by Description";

            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _sqlConnection.SqlStatement = checklistWp;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();

            DataTable dtChecklistWp = _sqlConnection.ResultsDataTable;

            ////Load data into the checklist workplaces
            foreach (DataRow dr in dtChecklistWp.Rows)
            {
                lbxCkecklistWPs.Items.Add(dr["Des"].ToString());
            }

            ///Load All work places
            ///
            string allWp = "Select Description wp from tbl_WORKPLACE \r\n " +
                " where workplaceid not in (Select WorkplaceID from tbl_Dept_Inspection_VentOther_Workplaces where Category = '" + checklistName + "') order by Description";

            MWDataManager.clsDataAccess _allWockplaces = new MWDataManager.clsDataAccess();
            _allWockplaces.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _allWockplaces.SqlStatement = allWp;
            _allWockplaces.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _allWockplaces.queryReturnType = MWDataManager.ReturnType.DataTable;
            _allWockplaces.ExecuteInstruction();

            DataTable dtWp = _allWockplaces.ResultsDataTable;

            ////Add all workplaces into the list box
            foreach (DataRow dr in dtWp.Rows)
            {
                lbxWorkplaces.Items.Add(dr["wp"].ToString());
            }
        }

        private void frmChecklistWorkplaces_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.Sizable;
            //this.WindowState = FormWindowState.Maximized;
            cmbxChecklistName.EditValue = checklistName;
            loadWorkplaces();
        }

        private void btnSaveWorkplace_Click(object sender, EventArgs e)
        {
            string selectedWp = lbxWorkplaces.SelectedItem.ToString();

            string saveWp = " insert into tbl_Dept_Inspection_VentOther_Workplaces(WorkplaceID, Category) \r\n"
                + " values((select WorkplaceID from tbl_Workplace where Description = '" + selectedWp + "' ), '" + cmbxChecklistName.EditValue.ToString() + "') \r\n ";
            MWDataManager.clsDataAccess _saveWorkplace = new MWDataManager.clsDataAccess();

            for (int i = 0; i < lbxCkecklistWPs.Items.Count; i++)
            {
                if (lbxCkecklistWPs.Items[i].ToString() == selectedWp)
                {
                    MessageBox.Show("Workplace already exist");
                    break;
                }
                else
                {
                    _saveWorkplace.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
                    _saveWorkplace.SqlStatement = saveWp;
                    _saveWorkplace.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                    _saveWorkplace.queryReturnType = MWDataManager.ReturnType.DataTable;
                    _saveWorkplace.ExecuteInstruction();

                    var result = _saveWorkplace.ExecuteInstruction();
                    if (result.success)
                    {
                        //MessageBox.Show("Workplace saved succesful");
                        lbxCkecklistWPs.Items.Add(selectedWp);
                        lbxWorkplaces.Items.Remove(selectedWp);
                        break;
                    }

                }
            }

            if (lbxCkecklistWPs.Items.Count == 0)
            {
                _saveWorkplace.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
                _saveWorkplace.SqlStatement = saveWp;
                _saveWorkplace.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _saveWorkplace.queryReturnType = MWDataManager.ReturnType.DataTable;
                _saveWorkplace.ExecuteInstruction();

                var result = _saveWorkplace.ExecuteInstruction();
                if (result.success)
                {
                    //MessageBox.Show("Workplace saved succesful");
                    lbxCkecklistWPs.Items.Add(selectedWp);
                    lbxWorkplaces.Items.Remove(selectedWp);
                }
            }
        }

        private void btnDeleteWorkplace_Click(object sender, EventArgs e)
        {
            string selectedCheckWp = lbxCkecklistWPs.SelectedItem.ToString();

            string removeWp = " delete from tbl_Dept_Inspection_VentOther_Workplaces \r\n" +
                " where WorkplaceID = (select WorkplaceID from tbl_Workplace where Description = '" + selectedCheckWp + "' ) \r\n ";

            MWDataManager.clsDataAccess _deleteWp = new MWDataManager.clsDataAccess();
            _deleteWp.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _deleteWp.SqlStatement = removeWp;
            _deleteWp.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _deleteWp.queryReturnType = MWDataManager.ReturnType.DataTable;
            _deleteWp.ExecuteInstruction();

            var result = _deleteWp.ExecuteInstruction();
            if (result.success)
            {
                lbxCkecklistWPs.Items.Remove(selectedCheckWp);
                lbxWorkplaces.Items.Add(selectedCheckWp);
                //XtraMessageBox.Show("Workplace deleted succesful");
            }

        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string checklistWp = "Select Description Des from tbl_WORKPLACE \r\n " +
                " where workplaceid not in (Select WorkplaceID from tbl_Dept_Inspection_VentOther_Workplaces where Category = '" + checklistName + "') and description like '" + textBox1.Text + "%' order by Description";

            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfo);
            _sqlConnection.SqlStatement = checklistWp;
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ExecuteInstruction();

            DataTable dtChecklistWp = _sqlConnection.ResultsDataTable;

            lbxWorkplaces.Items.Clear();

            ////Load data into the checklist workplaces
            foreach (DataRow dr in dtChecklistWp.Rows)
            {
                lbxWorkplaces.Items.Add(dr["Des"].ToString());
            }
        }
    }
}