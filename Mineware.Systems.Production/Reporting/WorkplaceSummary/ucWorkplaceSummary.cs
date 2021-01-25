using DevExpress.XtraEditors;
using Mineware.Systems.Global;
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
    public partial class ucWorkplaceSummary : BaseUserControl
    {
        public ucWorkplaceSummary()
        {
            InitializeComponent();
            FormRibbonPages.Add(rpPreplanning);
            FormActiveRibbonPage = rpPreplanning;
            FormMainRibbonPage = rpPreplanning;
            RibbonControl = rcPreplanning;
        }
        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            //navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
        }

        private void tileBar_Click(object sender, EventArgs e)
        {

        }

        void widgetView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {

            try
            {

                //SplashScreenManager.Default.Properties.AllowGlowEffect = true;
                // Launch a long-running operation while
                // the Overlay Form overlaps the current form.
                //splashScreenManager.ShowWaitForm();
                if (e.Document == documentData)
                {
                    //myForm.labelStatus.Text = "Dashboard - Loading Incidents..";
                   
                    e.Control = new ucGrid();
                }
                if (e.Document == documentReport)
                {
                    //myForm.labelStatus.Text = "Dashboard - Loading KPI..";
                   
                    e.Control = new ucReport();
                }
                
                if (e.Control == null)
                {

                    e.Control = new System.Windows.Forms.Control();
                }
            }

            finally
            {



            }



        }

        private void ucWorkplaceSummary_Load(object sender, EventArgs e)
        {
            this.widgetView1.QueryControl += widgetView1_QueryControl;
        }

        private void btnPPRE_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clsWorkplaceSummary.Type = "Pre-Planning";
            clsWorkplaceSummary.Department = "Rock Engineering";
        }

        private void btnPPGeo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clsWorkplaceSummary.Type = "Pre-Planning";
            clsWorkplaceSummary.Department = "Geology";
        }

        private void btnPPVent_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clsWorkplaceSummary.Type = "Pre-Planning";
            clsWorkplaceSummary.Department = "Ventillation";
        }

        private void btnPPSafety_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clsWorkplaceSummary.Type = "Pre-Planning";
            clsWorkplaceSummary.Department = "Safety";
        }

        private void btnPPSurvey_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clsWorkplaceSummary.Type = "Pre-Planning";
            clsWorkplaceSummary.Department = "Survey";
        }

        private void btnPPEng_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clsWorkplaceSummary.Type = "Pre-Planning";
            clsWorkplaceSummary.Department = "Engineering";
        }

        private void btnDepRE_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clsWorkplaceSummary.Type = "Walkabout";
            clsWorkplaceSummary.Department = "Rock Engineering";
        }

        private void btnDepGeo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clsWorkplaceSummary.Type = "Walkabout";
            clsWorkplaceSummary.Department = "Geology";
        }

        private void btnDepVent_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clsWorkplaceSummary.Type = "Walkabout";
            clsWorkplaceSummary.Department = "Ventillation";
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
