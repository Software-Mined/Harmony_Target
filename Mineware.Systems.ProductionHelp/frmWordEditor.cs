using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using Mineware.Systems.GlobalConnect;
using System.IO;
using DevExpress.XtraRichEdit;
using System.Windows;

namespace Mineware.Systems.ProductionHelp
{
    public partial class frmWordEditor : RibbonForm
    {
        private string _helpFolder = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\HelpFiles\";
        public string ViewType = "";
        public string MainCat = "";
        public string SubCat = "";       
        public frmWordEditor()
        {
            InitializeComponent();
            InitializeRichEditControl();
            ribbonControl.SelectedPage = homeRibbonPage1;
            
        }
        void InitializeRichEditControl()
        {
            
        }
        

        private void frmWordEditor_Load(object sender, EventArgs e)
        {
          


            /////GetHelpFiles
            DirectoryInfo dinfo = new DirectoryInfo(_helpFolder);
            FileInfo[] Files = dinfo.GetFiles("*.doc");
            foreach (FileInfo file in Files)
            {
                listBoxControl.Items.Add(file.Name);
            }

            if (ViewType == "New")
            {
                //richEditControl.Document.LoadDocument(_helpFolder + @"DefaultHelpFile.doc");
            }
            else 
            {
                homeRibbonPage1.Visible = false;
                insertRibbonPage1.Visible = false;
                pageLayoutRibbonPage1.Visible = false;
                referencesRibbonPage1.Visible = false;
                mailingsRibbonPage1.Visible = false;
                reviewRibbonPage1.Visible = false;
                helpRibbonPage.Visible = false;
                floatingPictureToolsFormatPage1.Visible = false;
                tableLayoutRibbonPage1.Visible = false;
                tableDesignRibbonPage1.Visible = false;
                headerFooterToolsDesignRibbonPage1.Visible = false;

                undoItem1.Visibility = BarItemVisibility.Never;
                redoItem1.Visibility = BarItemVisibility.Never;
                fileNewItem1.Visibility = BarItemVisibility.Never;
                fileOpenItem1.Visibility = BarItemVisibility.Never;
                fileSaveItem1.Visibility = BarItemVisibility.Never;
                infoRibbonPageGroup1.Visible = false;

                if (MainCat == "Bookings")
                {
                    if (SubCat == "Stoping")
                    {
                        richEditControl.Document.LoadDocument(_helpFolder + @"4.1. DailyBookings(Stoping).doc");
                        listBoxControl.SelectedValue = "4.1. DailyBookings(Stoping).doc";
                    }
                    if (SubCat == "Development")
                    {
                        richEditControl.Document.LoadDocument(_helpFolder + @"4.2. DailyBookings(Development).doc");
                        listBoxControl.SelectedValue = "4.2. DailyBookings(Development).doc";
                    }
                }

                if (MainCat == "Planning")
                {
                    
                     richEditControl.Document.LoadDocument(_helpFolder + @"3. Planning.doc");
                    listBoxControl.SelectedValue = "3. Planning.doc";

                }

                if (MainCat == "PrePlanning")
                {                    
                    richEditControl.Document.LoadDocument(_helpFolder + @"2. PrePlanning.doc");
                    listBoxControl.SelectedValue = "2. PrePlanning.doc";
                }

                if (MainCat == "Startups")
                {
                    richEditControl.Document.LoadDocument(_helpFolder + @"2. PrePlanning(Startups).doc");
                    listBoxControl.SelectedValue = "2. PrePlanning(Startups).doc";
                }

                if (MainCat == "Walkabouts")
                {
                    if (SubCat == "Ventilation Stoping")
                    {
                        richEditControl.Document.LoadDocument(_helpFolder + @"5.1. Ventilation(Stoping).doc");
                        listBoxControl.SelectedValue = "5.1. Ventilation(Stoping).doc";
                    }

                    if (SubCat == "Ventilation Development")
                    {
                        richEditControl.Document.LoadDocument(_helpFolder + @"5.1. Ventilation(Development).doc");
                        listBoxControl.SelectedValue = "5.1. Ventilation(Development).doc";
                    }
                }                

                if (MainCat == "Oreflow")
                {
                    richEditControl.Document.LoadDocument(_helpFolder + @"7. Oreflow.doc");
                    listBoxControl.SelectedValue = "7. Oreflow.doc";
                }

                if (MainCat == "LineActionManager")
                {
                    richEditControl.Document.LoadDocument(_helpFolder + @"6. LineActionManager.doc");
                    listBoxControl.SelectedValue = "6. LineActionManager.doc";
                }

                if (MainCat == "SurveyNote")
                {
                    richEditControl.Document.LoadDocument(_helpFolder + @"9. Departmental(SurveyNotes).doc");
                    listBoxControl.SelectedValue = "9. Departmental(SurveyNotes).doc";
                }

                if (MainCat == "RoutineVisit")
                {
                    richEditControl.Document.LoadDocument(_helpFolder + @"8. RoutineVisit.doc");
                    listBoxControl.SelectedValue = "8. RoutineVisit.doc";
                }

                if (MainCat == "SystemAdmin")
                {
                    if (SubCat == "UserProfileManagement")
                    {
                        richEditControl.Document.LoadDocument(_helpFolder + @"1.1. SystemAdmin(UserProfileManagement).doc");
                        listBoxControl.SelectedValue = "1.1. SystemAdmin(UserProfileManagement).doc";
                    }
                    if (SubCat == "UserManagement")
                    {
                        richEditControl.Document.LoadDocument(_helpFolder + @"1.2. SystemAdmin(UserManagement).doc");
                        listBoxControl.SelectedValue = "1.2. SystemAdmin(UserManagement).doc";
                    }
                    if (SubCat == "ProdactionUserSetup")
                    {
                        richEditControl.Document.LoadDocument(_helpFolder + @"1.3. SystemAdmin(ProdactionUserSetup).doc");
                        listBoxControl.SelectedValue = "1.3. SystemAdmin(ProdactionUserSetup).doc";
                    }
                    if (SubCat == "WorkplaceStoppages")
                    {
                        richEditControl.Document.LoadDocument(_helpFolder + @"1.5. SystemAdmin(WorkplaceStopage).doc");
                        listBoxControl.SelectedValue = "1.5. SystemAdmin(WorkplaceStopage).doc";
                    }
                }
            }
        }

        private void fileSaveItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            //richEditControl.SaveDocument(listBoxControl.SelectedItem+".doc", DocumentFormat.Doc);
        }

        private void listBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                richEditControl.Document.LoadDocument(_helpFolder + listBoxControl.SelectedItem);
            }
            catch { return; }
        }

        private void richEditControl_Click(object sender, EventArgs e)
        {

        }

        private void richEditControl_SizeChanged(object sender, EventArgs e)
        {
           

        }
    }
}