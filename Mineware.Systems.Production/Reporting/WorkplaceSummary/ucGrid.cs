using DevExpress.XtraEditors;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.Production.Reporting.WorkplaceSummary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Reporting.WorkplaceSummary
{
    public partial class ucGrid : BaseUserControl
    {
        clsWorkplaceSummary _clsWorkplaceSummary = new clsWorkplaceSummary();
        string Department = "";
        string Type = "";
        public ucGrid()
        {
            InitializeComponent();
        }

        private void ucGrid_Load(object sender, EventArgs e)
        {
            timer.Start();
            
        }

        private void gvGrid_DoubleClick(object sender, EventArgs e)
        {
            clsWorkplaceSummary.SelectedProdmonth = gvGrid.GetRowCellValue(gvGrid.FocusedRowHandle, gvGrid.Columns["Prodmonth"].FieldName).ToString();
            clsWorkplaceSummary.SelectedWorkplace = gvGrid.GetRowCellValue(gvGrid.FocusedRowHandle, gvGrid.Columns["Description"].FieldName).ToString();
            if(gvGrid.GetRowCellValue(gvGrid.FocusedRowHandle, gvGrid.Columns["CaptDate"].FieldName).ToString() != "")
            {
                clsWorkplaceSummary.SelectedCaptDate = Convert.ToDateTime(gvGrid.GetRowCellValue(gvGrid.FocusedRowHandle, gvGrid.Columns["CaptDate"].FieldName).ToString());
            }
            clsWorkplaceSummary.SelectedRiskRating = gvGrid.GetRowCellValue(gvGrid.FocusedRowHandle, gvGrid.Columns["RiskRating"].FieldName).ToString();
            clsWorkplaceSummary.SelectedActivity = gvGrid.GetRowCellValue(gvGrid.FocusedRowHandle, gvGrid.Columns["Activity"].FieldName).ToString();
            clsWorkplaceSummary.SelectedMOSection = gvGrid.GetRowCellValue(gvGrid.FocusedRowHandle, gvGrid.Columns["MOID"].FieldName).ToString();
            clsWorkplaceSummary.SelectedMinerSection = gvGrid.GetRowCellValue(gvGrid.FocusedRowHandle, gvGrid.Columns["MinerID"].FieldName).ToString();
            clsWorkplaceSummary.SelectedCrew = gvGrid.GetRowCellValue(gvGrid.FocusedRowHandle, gvGrid.Columns["Crew"].FieldName).ToString();



        }

        private void timer_Tick(object sender, EventArgs e)
        {

            LoadGrid();
        }

        void LoadGrid()
        {
            gbHeader.Caption = clsWorkplaceSummary.Department + ' ' + clsWorkplaceSummary.Type;

            //Walkabouts
            if (clsWorkplaceSummary.Type == "Walkabout" && clsWorkplaceSummary.Department == "Rock Engineering" && (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Type))
            {
                Department = clsWorkplaceSummary.Department;
                Type = clsWorkplaceSummary.Type;
                _clsWorkplaceSummary._connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _clsWorkplaceSummary.getRoutineVisitData();
                
            }
            else if (clsWorkplaceSummary.Type == "Walkabout" && clsWorkplaceSummary.Department == "Ventillation" && (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Type))
            {
                Department = clsWorkplaceSummary.Department;
                Type = clsWorkplaceSummary.Type;
                _clsWorkplaceSummary._connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _clsWorkplaceSummary.getVentVisitData();
            }
            else if (clsWorkplaceSummary.Type == "Walkabout" && clsWorkplaceSummary.Department == "Geology" && (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Type))
            {
                Department = clsWorkplaceSummary.Department;
                Type = clsWorkplaceSummary.Type;
                _clsWorkplaceSummary._connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _clsWorkplaceSummary.getGeologyVisitData();
            }

            //Pre-Planning
            else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Geology" && (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Type))
            {
                Department = clsWorkplaceSummary.Department;
                Type = clsWorkplaceSummary.Type;
                _clsWorkplaceSummary._connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _clsWorkplaceSummary.getGeoPrePlanData();
            }

            else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Rock Engineering" && (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Type))
            {
                Department = clsWorkplaceSummary.Department;
                Type = clsWorkplaceSummary.Type;
                _clsWorkplaceSummary._connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _clsWorkplaceSummary.getREPrePlanData();
            }

            else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Safety" && (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Type))
            {
                Department = clsWorkplaceSummary.Department;
                Type = clsWorkplaceSummary.Type;
                _clsWorkplaceSummary._connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _clsWorkplaceSummary.getSafetyPrePlanData();
            }

            else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Ventillation" && (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Type))
            {
                Department = clsWorkplaceSummary.Department;
                Type = clsWorkplaceSummary.Type;
                _clsWorkplaceSummary._connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _clsWorkplaceSummary.getVentPrePlanData();
            }

            else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Engineering" && (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Type))
            {
                Department = clsWorkplaceSummary.Department;
                Type = clsWorkplaceSummary.Type;
                _clsWorkplaceSummary._connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _clsWorkplaceSummary.getEngPrePlanData();
            }

            else if (clsWorkplaceSummary.Type == "Pre-Planning" && clsWorkplaceSummary.Department == "Survey" && (Department != clsWorkplaceSummary.Department || Type != clsWorkplaceSummary.Type))
            {
                Department = clsWorkplaceSummary.Department;
                Type = clsWorkplaceSummary.Type;
                _clsWorkplaceSummary._connection = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
                _clsWorkplaceSummary.getSurveyPrePlanData();
            }           


           
            gcGrid.DataSource = clsWorkplaceSummary.dtData;
            colProdmonth.FieldName = "Prodmonth";
            colSection.FieldName = "MOSec";
            colCrew.FieldName = "Crew";
            colWorkplaceName.FieldName = "Description";
            colDateCaptured.FieldName = "CaptDate";
            colRiskRating.FieldName = "RiskRating";
            colActivity.FieldName = "Activity";
            colMOSection.FieldName = "MOID";
            colMinerSection.FieldName = "MinerID";

            if (clsWorkplaceSummary.Type == "Walkabout" && clsWorkplaceSummary.Department == "Geology")
            {
                colRiskRating.Visible = false;
            }
            else
            {
                colRiskRating.Visible = true;
            }

            if (clsWorkplaceSummary.Type == "Pre-Planning")
            {
                colDateCaptured.Visible = false;
                //colWorkplaceName.Visible = false;
            }
            else
            {
                colDateCaptured.Visible = true;
                colWorkplaceName.Visible = true;
            }

            colProdmonth.SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            colSection.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
        }
    }
}
