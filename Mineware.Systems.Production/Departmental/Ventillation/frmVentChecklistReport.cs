using System;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Departmental.Ventillation
{
    public partial class frmVentChecklistReport : DevExpress.XtraEditors.XtraForm
    {

        public string theSystemDBTag;
        public string UserCurrentInfo;
        public string checkListID;
        public string monthDate;
        public string workPlace;
        public string section;
        public string prodMonth;
        public string crew;
        public string Frmtype;
        public string PicPath;
        public string feildBook;
        public string pageNum;
        public string ObsName;
        public string Authorise;


        public string _totScore, _totWeight, _percentage;

        public frmVentChecklistReport()
        {
            InitializeComponent();
        }

        private void frmVentChecklistReport_Load(object sender, EventArgs e)
        {
            ucVentChecklistReport ucReport = new ucVentChecklistReport();
            ucReport.theSystemDBTag = theSystemDBTag;
            ucReport.UserCurrentInfo.Connection = UserCurrentInfo;
            ucReport._ucCheckListID = checkListID;
            ucReport._ucMonthDate = String.Format("{0:yyyy-MM-dd}", monthDate);
            ucReport._ucWorkPlace = workPlace;
            ucReport._ucSection = section;
            ucReport._prodMonth = prodMonth;
            ucReport._ucCrew = crew;
            ucReport._FrmType = Frmtype;
            ucReport._picPath = PicPath;
            ucReport._TotScore = _totScore;
            ucReport._TotWeight = _totWeight;
            ucReport._SWpercentage = _percentage;
            ucReport._Auth = Authorise;
            this.Controls.Add(ucReport);
            ucReport.Dock = DockStyle.Fill;
        }
    }
}