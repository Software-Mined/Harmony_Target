using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.Dashboard.UserControls
{
    public partial class ucOCR : XtraUserControl
    {
        private DataTable dtGetData = new DataTable("dtGetData");
        private BackgroundWorker bgwDataOCR = new BackgroundWorker();
        public ucOCR()
        {
            InitializeComponent();
            bgwDataOCR.RunWorkerCompleted += bgwDataOCR_RunWorkerCompleted;
        }

        private void bgwDataOCR_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}
