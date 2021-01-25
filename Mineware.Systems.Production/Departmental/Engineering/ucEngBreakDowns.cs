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
using DevExpress.XtraGrid.Views.Grid;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.Production.Planning;

namespace Mineware.Systems.Production.Departmental.Engineering
{
    public partial class ucEngBreakDowns : BaseUserControl
    {
        public ucEngBreakDowns()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpUser);
            FormActiveRibbonPage = rpUser;
            FormMainRibbonPage = rpUser;
            RibbonControl = ribbonControl1;
        }

        //string UserCurrent = "Amplats";
        

        public void LoadInfo()
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo.Connection);

            _dbMan.SqlStatement = "select * from (Select case when bdtype = 'Weekly Service' then 1 else 2 end as ord, substring(convert(varchar(50),ReportedDate,8),1,5) tt, " +
                                  "convert(varchar(50),ReportedDate,106) dd, \r\n" +
                                  "convert(varchar(50), StartDate, 106) + ' ' + substring(convert(varchar(50), StartDate, 8), 1, 5) ss, \r\n" +
                                  "convert(varchar(50), EndDate, 106) + ' ' + substring(convert(varchar(50), EndDate, 8), 1, 5) ee \r\n" +


                                   ",*  from [dbo].[tbl_Eng_BreakDown]  where convert(datetime,convert(varchar(50),ReportedDate,106)) = '"+String.Format("{0:yyyy-MM-dd}",dateTimePicker1.Value)+ "'  \r\n" +

                                   " union Select case when bdtype = 'Weekly Service' then 1 else 2 end as ord, substring(convert(varchar(50),ReportedDate,8),1,5) tt, " +
                                  "convert(varchar(50),ReportedDate,106) dd, \r\n" +
                                  "convert(varchar(50), StartDate, 106) + ' ' + substring(convert(varchar(50), StartDate, 8), 1, 5) ss, \r\n" +
                                  "convert(varchar(50), EndDate, 106) + ' ' + substring(convert(varchar(50), EndDate, 8), 1, 5) ee \r\n" +


                                   ",*  from [dbo].[tbl_Eng_BreakDown]  where convert(datetime,convert(varchar(50),ReportedDate,106)) <> '" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "' and BDStatus <> 'Closed'  \r\n" +


                                   ") a order by ord, ReportedDate ";

            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dt = _dbMan.ResultsDataTable;


            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            BreakDownGrid.DataSource = ds.Tables[0];

            colEquipID.FieldName = "EquipNo";
            colTime.FieldName = "tt";
            colBDType.FieldName = "BDType";
            colDateRep.FieldName = "dd";
            ColStatus.FieldName = "BDStatus";
            colRepBy.FieldName = "ReportedBy";
            colRepTo.FieldName = "ReportedTo";
            colLocation.FieldName = "Location";
            colDam.FieldName = "Damaged";
            ColStartDate.FieldName = "ss";
            ColEndDate.FieldName = "ee";

            ColRem.FieldName = "Remarks";


        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmEngBreakDown EngBreakDownFrm = new frmEngBreakDown();
            EngBreakDownFrm.theSystemDBTag = this.theSystemDBTag;
            EngBreakDownFrm.UserCurrentInfo = this.UserCurrentInfo.Connection;
            EngBreakDownFrm.ShowDialog();
            LoadInfo();
        }

        private void ucEngBreakDowns_Load(object sender, EventArgs e)
        {
            Mineware.Systems.ProductionGlobal.ProductionGlobal.GetSysSettings(theSystemDBTag, UserCurrentInfo);
            LoadInfo();
        }

        private void BreakDownGrid_Click(object sender, EventArgs e)
        {

        }

        private void bandedGridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView View = sender as GridView;

            string val = "";
            string val1 = "";

            for (int i = 4; i < bandedGridView2.Columns.Count; i++)
            {
                val = View.GetRowCellValue(e.RowHandle, View.Columns[5]).ToString();
                val1 = View.GetRowCellValue(e.RowHandle, View.Columns[8]).ToString();

                if (val == "Weekly Service")
                {
                    e.Appearance.BackColor = Color.Beige;
                }

                if (val1 != "Closed")
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else
                {
                    e.Appearance.ForeColor = Color.Black;
                }                
            }
        }

        private void bandedGridView2_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            EqipSelectlabel.Text = bandedGridView2.GetRowCellValue(e.RowHandle, bandedGridView2.Columns[0]).ToString();
            DateSelectlabel.Text = bandedGridView2.GetRowCellValue(e.RowHandle, bandedGridView2.Columns[4]).ToString()+" "+ bandedGridView2.GetRowCellValue(e.RowHandle, bandedGridView2.Columns[1]).ToString();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (EqipSelectlabel.Text != "NN")
            {
                frmEngBreakDown EngBreakDownFrm = new frmEngBreakDown();
                EngBreakDownFrm.Equiplabe.Text = EqipSelectlabel.Text;
                EngBreakDownFrm.Datelabel.Text = DateSelectlabel.Text;
                EngBreakDownFrm.theSystemDBTag = this.theSystemDBTag;
                EngBreakDownFrm.UserCurrentInfo = this.UserCurrentInfo.Connection;
                EngBreakDownFrm.ShowDialog();
                LoadInfo();
            }
        }

        private void bandedGridView2_DoubleClick(object sender, EventArgs e)
        {
            if (EqipSelectlabel.Text != "NN")
            {
                frmEngBreakDown EngBreakDownFrm = new frmEngBreakDown();
                EngBreakDownFrm.Equiplabe.Text = EqipSelectlabel.Text;
                EngBreakDownFrm.Datelabel.Text = DateSelectlabel.Text;
                EngBreakDownFrm.theSystemDBTag = this.theSystemDBTag;
                EngBreakDownFrm.UserCurrentInfo = this.UserCurrentInfo.Connection;
                EngBreakDownFrm.ShowDialog();
                LoadInfo();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void DateSelectlabel_Click(object sender, EventArgs e)
        {

        }

        private void btnAddEquip_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Mineware.Systems.Production.Departmental.Engineering.frmAddEquipment  Equip = new Mineware.Systems.Production.Departmental.Engineering.frmAddEquipment();
            Equip.theSystemDBTag = this.theSystemDBTag;
            Equip.UserCurrentInfo = this.UserCurrentInfo.Connection;
            Equip.ShowDialog();
            LoadInfo();
        }

        private void btnAddBreakdown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Mineware.Systems.Production.Departmental.Engineering.frmAddBreakCat Equip = new Mineware.Systems.Production.Departmental.Engineering.frmAddBreakCat();
            Equip.theSystemDBTag = this.theSystemDBTag;
            Equip.UserCurrentInfo = this.UserCurrentInfo.Connection;
            Equip.ShowDialog();
            LoadInfo();
        }
    }
}
