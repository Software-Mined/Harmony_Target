using System;
using System.Linq;

namespace Mineware.Systems.Production.Reporting.Analysis
{
    public partial class frmColourPicker : DevExpress.XtraEditors.XtraForm
    {
        #region Private Variables
        public string RowNum;
        public string FormType;
        #endregion

        public ucPerformance mainFrm;
        public frmPerformanceDetial sysAdmin;

        public frmColourPicker(ucPerformance _Main)
        {
            InitializeComponent();
            mainFrm = _Main;
        }


        public frmColourPicker(frmPerformanceDetial _sysAdmin)
        {
            InitializeComponent();
            sysAdmin = _sysAdmin;
        }

        public frmColourPicker()
        {
            InitializeComponent();
        }



        private void frmColourPicker_Load(object sender, EventArgs e)
        {

        }

        private void sbtnSave_Click(object sender, EventArgs e)
        {
            if (FormType == "Performance")
            {
                string aa = colorPickEdit1.EditValue.ToString();

                mainFrm.gridView7.SetRowCellValue(Convert.ToInt32(RowNum), mainFrm.gridView7.Columns["ColColour"], Convert.ToInt32(colorPickEdit1.EditValue).ToString());
                this.Close();
            }

            if (FormType == "Detial")
            {
                string aa = colorPickEdit1.EditValue.ToString();

                sysAdmin.gridView7.SetRowCellValue(Convert.ToInt32(RowNum), sysAdmin.gridView7.Columns["ColColour"], Convert.ToInt32(colorPickEdit1.EditValue).ToString());
                this.Close();
            }
        }
    }
}