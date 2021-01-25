using DevExpress.XtraEditors;
using Mineware.Systems.GlobalConnect;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.Dashboard.UserControls
{
    public partial class ucCards : XtraUserControl
    {
        private DataTable dtGetData = new DataTable("dtGetData");
        private BackgroundWorker bgwDataCards = new BackgroundWorker();

        public ucCards()
        {
            InitializeComponent();
            bgwDataCards.RunWorkerCompleted += bgwDataCards_RunWorkerCompleted;

        }

        private void bgwDataCards_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gcCards.DataSource = dtGetData;

            //cvCrads.CardCaptionFormat = "Record # {0}, {2}";

            colValue1.FieldName = "Value1";
            colValue1.Caption = "Value1Name";
            colValue2.FieldName = "Value2";
            colValue2.Caption = "Value2Name";
            colPerc.FieldName = "Perc";
            colIndicator.Caption = string.Empty;
            colIndicator.FieldName = "Indicator";
            this.gcCards.Enabled = true;
        }


        private void ucUsers_Load(object sender, EventArgs e)
        {
            bgwDataCards.RunWorkerAsync();
            this.gcCards.Enabled = false;

            MWDataManager.clsDataAccess _dbManCards = new MWDataManager.clsDataAccess();
            _dbManCards.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, TUserInfo.Site);
            _dbManCards.SqlStatement = " select * from tbl_Dashboard_Cards \r\n";
            _dbManCards.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManCards.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManCards.ExecuteInstruction();

            try { dtGetData = _dbManCards.ResultsDataTable; }
            catch { return; }

        }

        private void cvCrads_CustomCardCaptionImage(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionImageEventArgs e)
        {
            //string HeaderCaption = cvCrads.GetRowCellValue(e.RowHandle, "CardName").ToString();
            //if (HeaderCaption == "Users")
            //{ e.Image = svgImageCollection1.GetImage(0); }
            //if (HeaderCaption == "Stoping")
            //{ e.Image = svgImageCollection1.GetImage(1); }
            //if (HeaderCaption == "Development")
            //{ e.Image = svgImageCollection1.GetImage(2); }
            //if (HeaderCaption == "Labour")
            //{ e.Image = svgImageCollection1.GetImage(3); }
        }

        private void cvCrads_CustomDrawCardCaption(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventArgs e)
        {
            string HeaderCaption = cvCrads.GetRowCellValue(e.RowHandle, "CardName").ToString();
            string FieldCaption1 = cvCrads.GetRowCellValue(e.RowHandle, "Value1Name").ToString();
            string FieldCaption2 = cvCrads.GetRowCellValue(e.RowHandle, "Value2Name").ToString();
            string Indicator = cvCrads.GetRowCellValue(e.RowHandle, "Indicator").ToString();
            e.CardCaption = HeaderCaption;
            colValue1.Caption = FieldCaption1;
            colValue2.Caption = FieldCaption2;

        }
    }
}
