using DevExpress.XtraBars.Navigation;
using Mineware.Menu.Structure;
using Mineware.Systems.GlobalConnect;
using Mineware.Systems.Production.Security;
using Mineware.Systems.Production.SysAdmin;
using Mineware.Systems.ProductionHelp;
using MWDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mineware.Systems.Production.Menu
{
    public partial class frmMenu : DevExpress.XtraEditors.XtraForm
    {
        #region Fields and Properties
        private List<ucMenuItem> _panelMenuItems;
        private List<AccordionControlElement> _accordMenuItems;
        private List<AccordionControlElement> _accordMenuItemsOCR;
        private List<AccordionControlElement> _accordMenuItemsOMS;
        private List<AccordionControlElement> _accordMenuItemsMinescribe;
        private List<AccordionControlElement> _accordMenuItemsCC;
        clsMenuStructure _clsMenuStructure = new clsMenuStructure();
        /// <summary>
        /// The menu items for the left hand menu
        /// </summary>
        public List<AccordionControlElement> AccordMenuItems
        {
            set
            {
                _accordMenuItems = value;
                accordMain.Elements.Clear();
                accordMain.Elements.AddRange(_accordMenuItems.ToArray());


            }
        }

        public List<AccordionControlElement> AccordMenuItemsOMS
        {
            set
            {
                _accordMenuItemsOMS = value;
                accordOMS.Elements.Clear();
                accordOMS.Elements.AddRange(_accordMenuItemsOMS.ToArray());
            }
        }

        public List<AccordionControlElement> AccordMenuItemsMinescribe
        {
            set
            {
                _accordMenuItemsMinescribe = value;
                accordBCS.Elements.Clear();
                accordBCS.Elements.AddRange(_accordMenuItemsMinescribe.ToArray());
            }
        }

        public List<AccordionControlElement> AccordMenuItemsOCR
        {
            set
            {
                _accordMenuItemsOCR = value;
                accordOCR.Elements.Clear();
                accordOCR.Elements.AddRange(_accordMenuItemsOCR.ToArray());
            }
        }


        public List<AccordionControlElement> AccordMenuItemsCC
        {
            set
            {
                _accordMenuItemsCC = value;
                accordCC.Elements.Clear();
                accordCC.Elements.AddRange(_accordMenuItemsCC.ToArray());
            }
        }

        private List<ProfileItem> _profileItems = new List<ProfileItem>();
        private List<ProfileItem> _parentProfileItems = new List<ProfileItem>();
        private List<ProfileItem> _profileItemsOCR = new List<ProfileItem>();
        private List<ProfileItem> _parentProfileItemsOCR = new List<ProfileItem>();
        private List<ProfileItem> _profileItemsOMS = new List<ProfileItem>();
        private List<ProfileItem> _parentProfileItemsOMS = new List<ProfileItem>();
        private List<ProfileItem> _profileItemsBCS = new List<ProfileItem>();
        private List<ProfileItem> _parentProfileItemsBCS = new List<ProfileItem>();
        private List<ProfileItem> _profileItemsCC = new List<ProfileItem>();
        private List<ProfileItem> _parentProfileItemsCC = new List<ProfileItem>();

        private TUserCurrentInfo UserCurrentInfo;

        private MethodInfo dynMethod;

        /// <summary>
        /// Set the colour of the menu
        /// </summary>
        public Color BackgroundColor
        {
            set
            {
                this.BackColor = value;
            }
        }

        /// <summary>
        /// If a button has been clicked set to true
        /// </summary>
        private bool _buttonClicked = false;
        #endregion Fields and Properties

        #region Constructor
        public frmMenu()
        {
            InitializeComponent();

            UserCurrentInfo = new TUserCurrentInfo();
            UserCurrentInfo.SetUserInfo(TUserInfo.UserID);
            UserCurrentInfo.Connection = TUserInfo.Site;

            GetMenuItems();
            CreateMenu();
            //CreatePanelMenu();

            //Set the menu colour
            BackgroundColor = Color.LightSlateGray;
            this.TopMost = true;
        }
        #endregion Constructor

        #region Events
        /// <summary>
        /// Set the location of the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMenu_Load(object sender, EventArgs e)
        {
            //Set it to 2/3 of the size of the main screen
            this.Size = new Size(Convert.ToInt32(this.Owner.Size.Width * 0.75), Convert.ToInt32(this.Owner.Size.Height * 0.65));

            //Set menu to display bottom left on main form
            int thisX = this.Owner.Location.X + 10;
            int thisY = (this.Owner.Location.Y + this.Owner.Size.Height) - (this.Height + 35);
            this.Location = new Point(thisX, thisY);

            tileControlOCR.ItemSize = 120;
            (tileControlOCR as DevExpress.XtraEditors.ITileControl).Properties.LargeItemWidth = 180;
          
            //use reflection to call private method
            dynMethod = this.Owner.GetType().GetMethod("LoadSelectedItemScreen", BindingFlags.NonPublic | BindingFlags.Instance);

            
            LoadTileVisibilities();
            SetModuleVisibilites();



        }

        void SetModuleVisibilites()
        {
           
            foreach (DataRow row in clsMenuStructure.dtUserModules.Rows)
            {
                if(row["SystemID"].ToString() == "Mineware.Systems.Production")
                {
                    dockPanelPAS.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                }
                if (row["SystemID"].ToString() == "Mineware.Systems.ProductionAmplatsBonus")
                {
                    dockPanelBSC.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                }
                if (row["SystemID"].ToString() == "Mineware.Systems.OCR")
                {
                    dockPanelOCR.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                }
                if (row["SystemID"].ToString() == "Mineware.Systems.CCA")
                {
                    dpCC.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                }
                if (row["SystemID"].ToString() == "Mineware.Systems.IS")
                {
                    dockPanelOMS.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                }

               
            }

           
        }

        private void Elem_Click(object sender, EventArgs e)
        {
            var elem = sender as AccordionControlElement;

            var pfi = _profileItems.Where(o => o.Description == elem.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void Elem_ClickOCR(object sender, EventArgs e)
        {
            var elem = sender as AccordionControlElement;

            var pfi = _profileItemsOCR.Where(o => o.Description == elem.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void Elem_ClickOMS(object sender, EventArgs e)
        {
            var elem = sender as AccordionControlElement;

            var pfi = _profileItemsOMS.Where(o => o.Description == elem.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void Elem_ClickMinescribe(object sender, EventArgs e)
        {
            //var elem = sender as AccordionControlElement;

            //var pfi = _profileItemsMinescribe.Where(o => o.Description == elem.Text).FirstOrDefault();

            //if (pfi != null)
            //{
            //    dynMethod.Invoke(this.Owner, new object[] { pfi });
            //}

            //this.Close();
        }

        /// <summary>
        /// Close the form when clicking outside the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMenu_Deactivate(object sender, EventArgs e)
        {
            if (!_buttonClicked)
                this.Close();
        }
        #endregion Events

        //TODO:Fix menu structure then put security back
        #region Methods
        /// <summary>
        /// Creating a panel menu
        /// </summary>
        /// 
        /// <summary>
        /// Creating an accordion menu
        /// </summary>
        private void CreateMenu()
        {
            List<AccordionControlElement> accordMenuItems = new List<AccordionControlElement>();
            //TODO: 
            foreach (var item in _parentProfileItems)
            {

                AccordionControlElement ace = new AccordionControlElement(ElementStyle.Group);
                ace.Text = item.Description;

                ace.ImageOptions.SvgImage = item.SvgImage;
                Size size = new Size(16, 16);
                ace.ImageOptions.SvgImageSize = size;
                ace.Expanded = true;


                foreach (var itemChild in _profileItems.Where(o => o.TopItemID == item.ItemID).ToArray())
                {

                    var elem = new AccordionControlElement(ElementStyle.Item);
                    elem.Text = itemChild.Description;
                    if (Security.SecurityPAS.HasPermissionPluginItem(itemChild.ItemID))
                    {
                        elem.Enabled = true;
                    }
                    else 
                    {
                        if (itemChild.ItemID != "SSUsers" && itemChild.ItemID != "SSProfile" && itemChild.ItemID != "SSDepartments" && itemChild.ItemID != "SSPasswordPolicy")
                        {
                            elem.Enabled = false;
                        }
                    }
                    elem.ImageOptions.SvgImage = itemChild.SvgImage;
                    Size size2 = new Size(14, 14);
                    elem.ImageOptions.SvgImageSize = size2;
                    elem.Click += Elem_Click;
                    ace.Elements.Add(elem);

                }
                accordMenuItems.Add(ace);

            }

            AccordMenuItems = accordMenuItems;

            /////OCR

            List<AccordionControlElement> accordMenuItemsOCR = new List<AccordionControlElement>();
            //TODO: 
            foreach (var item in _parentProfileItemsOCR)
            {
                //If the user has permission then show menu item
                //if (Security.SecurityPAS.HasPermissionPluginItem(item.ItemID))
                //{
                AccordionControlElement ace = new AccordionControlElement(ElementStyle.Group);
                ace.Text = item.Description;
                ace.ImageOptions.SvgImage = item.SvgImage;
                Size size = new Size(16, 16);
                ace.ImageOptions.SvgImageSize = size;
                ace.Expanded = true;


                foreach (var itemChild in _profileItemsOCR.Where(o => o.TopItemID == item.ItemID).ToArray())
                {
                    //If the user has permission then show menu item
                    //if (Security.SecurityPAS.HasPermissionPluginItem(itemChild.ItemID))
                    //{
                    var elem = new AccordionControlElement(ElementStyle.Item);
                    elem.ImageOptions.SvgImage = itemChild.SvgImage;                    
                    Size size2 = new Size(14, 14);
                    elem.ImageOptions.SvgImageSize = size2;
                    elem.Text = itemChild.Description;
                    elem.Click += Elem_ClickOCR;
                    ace.Elements.Add(elem);
                    //}
                }
                accordMenuItemsOCR.Add(ace);
                //}
            }

            AccordMenuItemsOCR = accordMenuItemsOCR;


            /////OMS

            List<AccordionControlElement> accordMenuItemsOMS = new List<AccordionControlElement>();
            //TODO: 
            foreach (var item in _parentProfileItemsOMS)
            {
                //If the user has permission then show menu item
                //if (Security.SecurityPAS.HasPermissionPluginItem(item.ItemID))
                //{
                AccordionControlElement ace = new AccordionControlElement(ElementStyle.Group);
                ace.Text = item.Description;
                ace.ImageOptions.SvgImage = item.SvgImage;
                Size size = new Size(16, 16);
                ace.ImageOptions.SvgImageSize = size;
                ace.Expanded = true;


                foreach (var itemChild in _profileItemsOMS.Where(o => o.TopItemID == item.ItemID).ToArray())
                {
                    //If the user has permission then show menu item
                    //if (Security.SecurityPAS.HasPermissionPluginItem(itemChild.ItemID))
                    //{
                    var elem = new AccordionControlElement(ElementStyle.Item);
                    elem.Text = itemChild.Description;
                   
                    elem.ImageOptions.SvgImage = itemChild.SvgImage;
                    Size size2 = new Size(14, 14);
                    elem.ImageOptions.SvgImageSize = size2;
                    elem.Click += Elem_ClickOMS;
                    ace.Elements.Add(elem);
                    //}
                }
                accordMenuItemsOMS.Add(ace);
                //}
            }

            AccordMenuItemsOMS = accordMenuItemsOMS;

            /////Minescribe

            List<AccordionControlElement> accordMenuItemsMinescribe = new List<AccordionControlElement>();
            //TODO: 
            foreach (var item in _parentProfileItemsBCS)
            {
                //If the user has permission then show menu item
                //if (Security.SecurityPAS.HasPermissionPluginItem(item.ItemID))
                //{
                AccordionControlElement ace = new AccordionControlElement(ElementStyle.Group);
                ace.Text = item.Description;
                ace.ImageOptions.SvgImage = item.SvgImage;
                Size size = new Size(16, 16);
                ace.ImageOptions.SvgImageSize = size;
                ace.Expanded = true;


                foreach (var itemChild in _profileItemsBCS.Where(o => o.TopItemID == item.ItemID).ToArray())
                {
                    //If the user has permission then show menu item
                    //if (Security.SecurityPAS.HasPermissionPluginItem(itemChild.ItemID))
                    //{
                    var elem = new AccordionControlElement(ElementStyle.Item);
                    elem.Text = itemChild.Description;                    
                    elem.ImageOptions.SvgImage = itemChild.SvgImage;
                    Size size2 = new Size(14, 14);
                    elem.ImageOptions.SvgImageSize = size2;
                    elem.Click += Elem_ClickMinescribe;
                    ace.Elements.Add(elem);
                    //}
                }
                accordMenuItemsMinescribe.Add(ace);
                //}
            }

            AccordMenuItemsMinescribe = accordMenuItemsMinescribe;


            /////Minescribe

            List<AccordionControlElement> accordMenuItemsCC = new List<AccordionControlElement>();
            //TODO: 
            foreach (var item in _parentProfileItemsCC)
            {
                //If the user has permission then show menu item
                //if (Security.SecurityPAS.HasPermissionPluginItem(item.ItemID))
                //{
                AccordionControlElement ace = new AccordionControlElement(ElementStyle.Group);
                ace.Text = item.Description;
                ace.ImageOptions.SvgImage = item.SvgImage;
                Size size = new Size(16, 16);
                ace.ImageOptions.SvgImageSize = size;
                ace.Expanded = true;


                foreach (var itemChild in _profileItemsCC.Where(o => o.TopItemID == item.ItemID).ToArray())
                {
                    //If the user has permission then show menu item
                    //if (Security.SecurityPAS.HasPermissionPluginItem(itemChild.ItemID))
                    //{
                    var elem = new AccordionControlElement(ElementStyle.Item);
                    elem.Text = itemChild.Description;
                    elem.ImageOptions.SvgImage = itemChild.SvgImage;
                    Size size2 = new Size(14, 14);
                    elem.ImageOptions.SvgImageSize = size2;
                    elem.Click += Elem_ClickMinescribe;
                    ace.Elements.Add(elem);
                    //}
                }
                accordMenuItemsCC.Add(ace);
                //}
            }

            AccordMenuItemsCC = accordMenuItemsCC;
        }

        /// <summary>
        /// Get the menu items from syncromine DB
        /// </summary>
        private void GetMenuItems()
        {
            ///PAS
            //get all UI and report items
            try
            {
                if (clsMenuStructure.dtgetAllProductionAndSystemMenuItems.Rows.Count == 0)
                {
                   
                }
            }
            catch
            {
                _clsMenuStructure.getAllUserModules();
                _clsMenuStructure.getAllProductionAndSystemMenuItems();
                _clsMenuStructure.getAllParentMenuItems();
                _clsMenuStructure.getOMSParentMenuItems();
                _clsMenuStructure.getAllOMSItems();
                _clsMenuStructure.getAllBCSItems();
                _clsMenuStructure.getBCSParentMenuItems();
                _clsMenuStructure.getOCRParentMenuItems();
                _clsMenuStructure.getAllOCRItems();
                _clsMenuStructure.getAllCCItems();
                _clsMenuStructure.getCCParentMenuItems();
            }
            var theSubItems = clsMenuStructure.dtgetAllProductionAndSystemMenuItems;
            if (theSubItems != null)
            {
                foreach (DataRow dr in theSubItems.Rows)
                {
                    ProfileItem _ProfileItemSub = new ProfileItem();
                    _ProfileItemSub.loadDataFromRow(dr);
                    _profileItems.Add(_ProfileItemSub);
                    //_profileItems


                }
            }

            //get all the parent menu items
            var theSubItems2 = clsMenuStructure.dtgetAllParentMenuItems;
            if (theSubItems2 != null)
            {
                foreach (DataRow dr in theSubItems2.Rows)
                {
                    ProfileItem _ProfileItemSub = new ProfileItem();
                    _ProfileItemSub.loadDataFromRow(dr);
                    _parentProfileItems.Add(_ProfileItemSub);
                }
            }



            ///OCR
            var theSubItemsOCR = clsMenuStructure.dtgetAllOCRItems;
            if (theSubItemsOCR != null)
            {
                foreach (DataRow dr in theSubItemsOCR.Rows)
                {
                    ProfileItem _ProfileItemSubOCR = new ProfileItem();
                    _ProfileItemSubOCR.loadDataFromRow(dr);
                    _profileItemsOCR.Add(_ProfileItemSubOCR);
                }
            }

            //get all the parent menu items
            var theParentItemsOCR = clsMenuStructure.dtgetOCRParentMenuItems;
            if (theParentItemsOCR != null)
            {
                foreach (DataRow dr in theParentItemsOCR.Rows)
                {
                    ProfileItem _ProfileItemParentOCR = new ProfileItem();
                    _ProfileItemParentOCR.loadDataFromRow(dr);
                    _parentProfileItemsOCR.Add(_ProfileItemParentOCR);
                }
            }

            ///OMS
            var theSubItemsOMS = clsMenuStructure.dtgetAllOMSItems;
            if (theSubItemsOCR != null)
            {
                foreach (DataRow dr in theSubItemsOMS.Rows)
                {
                    ProfileItem _ProfileItemSubOMS = new ProfileItem();
                    _ProfileItemSubOMS.loadDataFromRow(dr);
                    _profileItemsOMS.Add(_ProfileItemSubOMS);
                }
            }

            //get all the parent menu items
            var theParentItemsOMS = clsMenuStructure.dtgetOMSParentMenuItems;
            if (theParentItemsOMS != null)
            {
                foreach (DataRow dr in theParentItemsOMS.Rows)
                {
                    ProfileItem _ProfileItemParentOMS = new ProfileItem();
                    _ProfileItemParentOMS.loadDataFromRow(dr);
                    _parentProfileItemsOMS.Add(_ProfileItemParentOMS);
                }
            }


            ///Minescribe
            var theSubItemsMinescribe = clsMenuStructure.dtgetAllBCSItems;
            if (theSubItemsMinescribe != null)
            {
                foreach (DataRow dr in theSubItemsMinescribe.Rows)
                {
                    ProfileItem _ProfileItemSubMinescribe = new ProfileItem();
                    _ProfileItemSubMinescribe.loadDataFromRow(dr);
                    _profileItemsBCS.Add(_ProfileItemSubMinescribe);
                }
            }

            //get all the parent menu items
            var theParentItemsMinescribe = clsMenuStructure.dtgetBCSParentMenuItems;
            if (theParentItemsMinescribe != null)
            {
                foreach (DataRow dr in theParentItemsMinescribe.Rows)
                {
                    ProfileItem _ProfileItemParentMinescribe = new ProfileItem();
                    _ProfileItemParentMinescribe.loadDataFromRow(dr);
                    _parentProfileItemsBCS.Add(_ProfileItemParentMinescribe);
                }
            }



            ///Call Centre
            var theSubItemsCC = clsMenuStructure.dtgetAllCCItems;
            if (theSubItemsCC != null)
            {
                foreach (DataRow dr in theSubItemsCC.Rows)
                {
                    ProfileItem _ProfileItemSubCC = new ProfileItem();
                    _ProfileItemSubCC.loadDataFromRow(dr);
                    _profileItemsCC.Add(_ProfileItemSubCC);
                }
            }

            //get all the parent menu items
            var theParentItemsCC = clsMenuStructure.dtgetCCParentMenuItems;
            if (theParentItemsCC != null)
            {
                foreach (DataRow dr in theParentItemsCC.Rows)
                {
                    ProfileItem _ProfileItemParentCC = new ProfileItem();
                    _ProfileItemParentCC.loadDataFromRow(dr);
                    _parentProfileItemsCC.Add(_ProfileItemParentCC);
                }
            }

        }

       


        #endregion Methods 

        private void tileBookings_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            //PicMain_Click(null,null);

            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tilePlanning_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileOreflow_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileMODailyRep_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileLostBlastAnalysis_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tilePlanvsBook_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tile6Shift_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileReconRep_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileBlastComp_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tilePlanningRep_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileCrewAnalysis_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileMeasList_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileLineAction_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileRoutineVisits_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileGeology_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileSurvey_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }



        private void tileReportAutomization_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }




        private void tileHelpFile_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmWordEditor FrmWordEditor = new frmWordEditor();
            FrmWordEditor.ViewType = "New";
            FrmWordEditor.Show();
        }

        private void tileUserAccess_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileOCRConnections_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsOCR.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileOCRDesigner_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsOCR.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileOCRVerification_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsOCR.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileOCRPrint_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsOCR.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

      

        private void dockPanelOCR_Click(object sender, EventArgs e)
        {
            navigationFrame.SelectedPage = navigationPageOCR;            
            tileControlOCR.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        private void dockPanelOMS_Click(object sender, EventArgs e)
        {
            navigationFrame.SelectedPage = navigationPageIntergratedSchedular;
            tileControlIntergratedSchedular.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        private void dockPanelMSC_Click(object sender, EventArgs e)
        {
            navigationFrame.SelectedPage = navigationPageBonus;
            tileControlBonus.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        private void dockPanelPAS_Click(object sender, EventArgs e)
        {
            navigationFrame.SelectedPage = navigationPageMain;
            tileControlProduction.Dock = System.Windows.Forms.DockStyle.Fill;
           
        }

        private void panelContainer_ActiveChildChanged(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            LoadTileVisibilities();
        }


        void LoadTileVisibilities()
        {
            #region Production
            //Planning
            bool HasPlanAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Planning);
            bool HasPrePlanAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.PrePlanAccess);
            bool HasMOPrePlanAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.MOScrutiny);
            if (HasPlanAccess == false && HasPrePlanAccess == false && HasMOPrePlanAccess == false)
            {
                tilePlanning.Enabled = false;
            }
            else
            {
                tilePlanning.Enabled = true;
            }            

            //Bookings
            bool HasBookingAccessDS = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Book_DS);
            bool HasBookingAccessNS = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.Book_NS);
            if (HasBookingAccessDS == false && HasBookingAccessNS == false)
            {
                tileBookings.Enabled = false;
            }
            else
            {
                tileBookings.Enabled = true;
            }

            //Rock Eng
            bool HasREAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureRockEng);
            if (HasREAccess == false)
            {
                tileRoutineVisits.Enabled = false;
            }
            else
            {
                tileRoutineVisits.Enabled = true;
            }

            //Geology
            bool HasGeologyAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureGeology);
            if (HasGeologyAccess == false)
            {
                tileGeology.Enabled = false;
            }
            else
            {
                tileGeology.Enabled = true;
            }

            //Vent
            bool HasVentAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureVent);
            if (HasVentAccess == false)
            {
                tileVentilation.Enabled = false;
            }
            else
            {
                tileVentilation.Enabled = true;
            }


            //Survey
            bool HasSurveyAccess = SecurityPAS.HasSyncrominePermission(Security.SyncrominePermissions.CaptureSurvey);
            if (HasSurveyAccess == false)
            {
                tileSurvey.Enabled = false;
            }
            else
            {
                tileSurvey.Enabled = true;
            }
            #endregion

            if (panelContainer.ActiveChild.Text == "Production Analysis System")
            {
                navigationFrame.SelectedPage = navigationPageMain;

            }

            if (panelContainer.ActiveChild.Text == "OCR Scanning Solution")
            {
                navigationFrame.SelectedPage = navigationPageOCR;

            }

            if (panelContainer.ActiveChild.Text == "Intergrated Schedular")
            {
                navigationFrame.SelectedPage = navigationPageIntergratedSchedular;

            }

            if (panelContainer.ActiveChild.Text == "Call Centre")
            {
                navigationFrame.SelectedPage = navigationPageCallCentre;

            }

            if (panelContainer.ActiveChild.Text == "Bonus Control System")
            {
                navigationFrame.SelectedPage = navigationPageBonus;

            }












        }

        private void tileMonthEndRecon_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileKPI_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileSettings_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmSystemSettings frmSettings = new frmSystemSettings();
            frmSettings.Show();
            frmSettings.BringToFront();
        }

        private void tileReleaseNotes_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            frmReleaseNotes frm = new frmReleaseNotes();
            frm.Show();
            frm.BringToFront();
        }

        private void tileVentilation_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItems.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileControlProduction_Click(object sender, EventArgs e)
        {

        }

        private void navigationPageCallCentre_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void dpCC_Click(object sender, EventArgs e)
        {
            navigationFrame.SelectedPage = navigationPageCallCentre;
            tileControlCallCentre.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        private void tileItemProduction_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemSchedules_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsOMS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemPlanPerf_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsOMS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemAdmin_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsCC.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemCalls_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsCC.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemTramming_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemSafety_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemShift_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemCrewBonus_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemMinersBonus_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemTrammingBonus_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemSBBonus_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemEngBonus_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemBonusReports_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemGangMapping_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemEngParam_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemMiningParam_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }

        private void tileItemGenParam_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            _buttonClicked = true;

            var pfi = _profileItemsBCS.Where(o => o.Description == e.Item.Text).FirstOrDefault();

            if (pfi != null)
            {
                dynMethod.Invoke(this.Owner, new object[] { pfi });
            }

            this.Close();
        }
    }
}
