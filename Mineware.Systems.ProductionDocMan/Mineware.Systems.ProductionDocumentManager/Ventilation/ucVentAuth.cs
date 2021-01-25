using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Mineware.Systems.DocumentManager.Ventilation
{
    public partial class ucVentAuth : UserControl
    {
        public ucVentAuth()
        {
            InitializeComponent();
        }
        
        private void ucVentAuth_Load(object sender, EventArgs e)
        {
            LoadAuthGrid();
        }

        private void LoadAuthGrid()
        {
            MWDataManager.clsDataAccess _dbMan3Mnth = new MWDataManager.clsDataAccess();
            _dbMan3Mnth.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
            _dbMan3Mnth.SqlStatement = " Select * from vw_Dept_Inspections_Authorise order by ord,activity desc,SBSection,CaptDate";
            _dbMan3Mnth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan3Mnth.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan3Mnth.ExecuteInstruction();

            DataTable dt = _dbMan3Mnth.ResultsDataTable;
            DataSet ds = new DataSet();

            if (ds.Tables.Count > 0)
                ds.Tables.Clear();

            ds.Tables.Add(dt);

            gcAuth.DataSource = ds.Tables[0];

            colActivity.FieldName = "activity";
            MnthMOID.FieldName = "MOSection";
            MnthSBID.FieldName = "SBSection";
            MnthMinerSecID.FieldName = "MinerSection";
            MnthMiner.FieldName = "MinerName";
            MnthCrew.FieldName = "CrewName";
            MnthWp.FieldName = "WPDesc";
            MnthWPID.FieldName = "WPID";
            MnthDueDate.FieldName = "CaptDate";
            colCrewID.FieldName = "CrewID";
            MnthDaysRem.FieldName = "DayslastInspec";
            colChecklistID.FieldName = "Section";
            colRiskRating.FieldName = "maxRR";
            colNotes.FieldName = "Notes";
            colOberverName.FieldName = "Observername";
        }

        private void gvAuth_DoubleClick(object sender, EventArgs e)
        {
            int focusedRow = gvAuth.FocusedRowHandle;

            string SelectedSectionID = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["MinerSection"]).ToString();
            string SelectedCrewID = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["CrewID"]).ToString();
            string SelectedCrewName = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["CrewName"]).ToString();
            string SelectedActivity = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["activity"]).ToString();
            string SelectedCalendardate = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["CaptDate"]).ToString();
            string SelectedchecklistID = gvAuth.GetRowCellValue(gvAuth.FocusedRowHandle, gvAuth.Columns["Section"]).ToString();

            if (gvAuth.FocusedColumn.FieldName == "MinerSection" || gvAuth.FocusedColumn.FieldName == "MinerName")
            {
                if (SelectedActivity == "Stope" || SelectedActivity == "Ledge")
                {
                    //ProductionAmplats.Departmental.VentInspections.frmInspection frmEdit = new ProductionAmplats.Departmental.VentInspections.frmInspection();
                    frmInspection frmEdit = new frmInspection();
                    frmEdit.dbl_rec_MinerSection.Text = SelectedSectionID;
                    frmEdit.dbl_rec_Date.Text = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(SelectedCalendardate));
                    frmEdit.dbl_rec_Crew = SelectedCrewID + ":" + SelectedCrewName;
                    frmEdit.tbCrew2.EditValue = SelectedCrewID + ":" + SelectedCrewName;
                    //frmEdit.tbDpInspecDate.Enabled = false;
                    frmEdit.ShowDialog();

                    LoadAuthGrid();
                    if (gvAuth.RowCount >= focusedRow)
                    {
                        gvAuth.FocusedRowHandle = focusedRow;
                    }
                    return;
                }

                if (SelectedActivity == "Development")
                {
                    frmDevelopmentInspection frmDev = new frmDevelopmentInspection();
                    frmDev.dbl_rec_MinerSection.Text = SelectedSectionID;
                    frmDev.dbl_rec_Date.Text = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(SelectedCalendardate));
                    frmDev.dbl_rec_Crew = SelectedCrewID + ":" + SelectedCrewName;
                    frmDev.tbCrew.EditValue = SelectedCrewID + ":" + SelectedCrewName;
                    frmDev.txtDpInspecDate.Enabled = false;
                    frmDev.ShowDialog();

                    LoadAuthGrid();
                    if (gvAuth.RowCount >= focusedRow)
                    {
                        gvAuth.FocusedRowHandle = focusedRow;
                    }
                    return;
                }

                if (SelectedActivity != "Development" || SelectedActivity != "Stope" || SelectedActivity != "Ledge")
                {
                    frmVentOtherChecklist frmChkLst = new frmVentOtherChecklist();
                    frmChkLst.cbxWorkplace.EditValue = SelectedSectionID;
                    frmChkLst._monthDate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(SelectedCalendardate));
                    frmChkLst.chkListName = SelectedActivity;
                    frmChkLst.ricbxCheckList.Items.Add(SelectedchecklistID);
                    
                    frmChkLst.cbxSection.Enabled = false;
                    frmChkLst.ShowDialog();

                    LoadAuthGrid();
                    if (gvAuth.RowCount >= focusedRow)
                    {
                        gvAuth.FocusedRowHandle = focusedRow;
                    }
                    return;
                }
            }
        }

        private void ribbonCntrlMain_Click(object sender, EventArgs e)
        {

        }

        private void gvAuth_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Notes")
            {
                string Section = gvAuth.GetRowCellValue(e.RowHandle,gvAuth.Columns["MinerSection"]).ToString();
                string CalDate = gvAuth.GetRowCellValue(e.RowHandle, gvAuth.Columns["CaptDate"]).ToString();
                string Activty = gvAuth.GetRowCellValue(e.RowHandle, gvAuth.Columns["activity"]).ToString();
                string Notes = gvAuth.GetRowCellValue(e.RowHandle, gvAuth.Columns["Notes"]).ToString();

                MWDataManager.clsDataAccess _dbMan3Mnth = new MWDataManager.clsDataAccess();
                _dbMan3Mnth.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                _dbMan3Mnth.SqlStatement = " insert into tbl_Dept_Inspection_Notes values ('"+ Activty + "','"+ CalDate + "','"+ Section + "','"+ Notes + "')";
                _dbMan3Mnth.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan3Mnth.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan3Mnth.ExecuteInstruction();
            }
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
