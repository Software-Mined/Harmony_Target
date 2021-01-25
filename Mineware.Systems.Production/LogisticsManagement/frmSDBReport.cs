using DevExpress.XtraEditors;
using FastReport;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class frmSDBReport : XtraForm
    {
        public frmSDBReport()
        {
            InitializeComponent();
        }


        Report theReport = new Report();


        string RepDir = Mineware.Systems.ProductionGlobal.ProductionGlobal.RepDir;
        private string _reportFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports\";
        private void frmSDBReport_Load(object sender, EventArgs e)
        {
            theReport.Load(_reportFolder + "\\SDB.frx");
            //theReport.Design();

            pcReport2.Clear();
            theReport.Prepare();
            theReport.Preview = pcReport2;
            theReport.ShowPrepared();
        }
    }
}
