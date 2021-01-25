namespace DocumentManager
{
    partial class frmMainLoad
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.TileItemElement tileItemElement9 = new DevExpress.XtraEditors.TileItemElement();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainLoad));
            DevExpress.XtraEditors.TileItemElement tileItemElement10 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement11 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement12 = new DevExpress.XtraEditors.TileItemElement();
            this.tileBar = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroupTables = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.VentTileBarItem = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.SurveyTileBarItem = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.tileBarActions = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.navigationFrame = new DevExpress.XtraBars.Navigation.NavigationFrame();
            this.VentNavigationPage = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.SurveyNavigationPage = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.ActionsNavigationPage = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.tileBar1 = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup3 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.tileBarItemExit = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.lblUser = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.formAssistant = new DevExpress.XtraBars.FormAssistant();
            this.fluentDesignFormContainer1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer();
            this.fluentDesignFormControl1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl();
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame)).BeginInit();
            this.navigationFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // tileBar
            // 
            this.tileBar.AllowDragTilesBetweenGroups = false;
            this.tileBar.AllowGlyphSkinning = true;
            this.tileBar.AllowSelectedItem = true;
            this.tileBar.AppearanceGroupText.Font = new System.Drawing.Font("Segoe UI Semibold", 7F, System.Drawing.FontStyle.Bold);
            this.tileBar.AppearanceGroupText.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tileBar.AppearanceGroupText.Options.UseFont = true;
            this.tileBar.AppearanceGroupText.Options.UseForeColor = true;
            this.tileBar.BackColor = System.Drawing.Color.White;
            this.tileBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.tileBar.DropDownButtonWidth = 30;
            this.tileBar.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.tileBar.Groups.Add(this.tileBarGroupTables);
            this.tileBar.IndentBetweenGroups = 8;
            this.tileBar.IndentBetweenItems = 8;
            this.tileBar.ItemBorderVisibility = DevExpress.XtraEditors.TileItemBorderVisibility.Always;
            this.tileBar.ItemImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopRight;
            this.tileBar.ItemImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Squeeze;
            this.tileBar.ItemPadding = new System.Windows.Forms.Padding(6, 4, 10, 4);
            this.tileBar.Location = new System.Drawing.Point(0, 0);
            this.tileBar.LookAndFeel.SkinName = "The Bezier";
            this.tileBar.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tileBar.Margin = new System.Windows.Forms.Padding(2);
            this.tileBar.MaxId = 7;
            this.tileBar.MaximumSize = new System.Drawing.Size(0, 144);
            this.tileBar.MinimumSize = new System.Drawing.Size(117, 100);
            this.tileBar.Name = "tileBar";
            this.tileBar.Padding = new System.Windows.Forms.Padding(24, 14, 24, 14);
            this.tileBar.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.None;
            this.tileBar.SelectedItem = this.tileBarActions;
            this.tileBar.SelectionBorderWidth = 2;
            this.tileBar.SelectionColorMode = DevExpress.XtraBars.Navigation.SelectionColorMode.UseItemBackColor;
            this.tileBar.ShowGroupText = false;
            this.tileBar.Size = new System.Drawing.Size(1266, 115);
            this.tileBar.TabIndex = 1;
            this.tileBar.Text = "tileBar";
            this.tileBar.WideTileWidth = 160;
            this.tileBar.SelectedItemChanged += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileBar_SelectedItemChanged);
            // 
            // tileBarGroupTables
            // 
            this.tileBarGroupTables.Items.Add(this.VentTileBarItem);
            this.tileBarGroupTables.Items.Add(this.SurveyTileBarItem);
            this.tileBarGroupTables.Items.Add(this.tileBarActions);
            this.tileBarGroupTables.Name = "tileBarGroupTables";
            this.tileBarGroupTables.Text = "Actions";
            // 
            // VentTileBarItem
            // 
            this.VentTileBarItem.AppearanceItem.Normal.BackColor = System.Drawing.Color.SteelBlue;
            this.VentTileBarItem.AppearanceItem.Normal.Options.UseBackColor = true;
            this.VentTileBarItem.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement9.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            tileItemElement9.ImageOptions.ImageUri.Uri = "Cube;Size32x32;GrayScaled";
            tileItemElement9.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage")));
            tileItemElement9.Text = "Ventilation";
            tileItemElement9.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopLeft;
            this.VentTileBarItem.Elements.Add(tileItemElement9);
            this.VentTileBarItem.Name = "VentTileBarItem";
            // 
            // SurveyTileBarItem
            // 
            this.SurveyTileBarItem.AppearanceItem.Normal.BackColor = System.Drawing.Color.LightSlateGray;
            this.SurveyTileBarItem.AppearanceItem.Normal.Options.UseBackColor = true;
            this.SurveyTileBarItem.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement10.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            tileItemElement10.ImageOptions.ImageUri.Uri = "Cube;Size32x32;GrayScaled";
            tileItemElement10.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage1")));
            tileItemElement10.Text = "Survey";
            tileItemElement10.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopLeft;
            this.SurveyTileBarItem.Elements.Add(tileItemElement10);
            this.SurveyTileBarItem.Id = 2;
            this.SurveyTileBarItem.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.SurveyTileBarItem.Name = "SurveyTileBarItem";
            // 
            // tileBarActions
            // 
            this.tileBarActions.AppearanceItem.Normal.BackColor = System.Drawing.Color.Coral;
            this.tileBarActions.AppearanceItem.Normal.Options.UseBackColor = true;
            this.tileBarActions.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement11.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.BottomRight;
            tileItemElement11.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage2")));
            tileItemElement11.ImageOptions.SvgImageSize = new System.Drawing.Size(60, 60);
            tileItemElement11.Text = "Actions";
            tileItemElement11.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopLeft;
            this.tileBarActions.Elements.Add(tileItemElement11);
            this.tileBarActions.Id = 6;
            this.tileBarActions.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.tileBarActions.Name = "tileBarActions";
            this.tileBarActions.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileBarActions_ItemClick);
            // 
            // navigationFrame
            // 
            this.navigationFrame.Controls.Add(this.VentNavigationPage);
            this.navigationFrame.Controls.Add(this.SurveyNavigationPage);
            this.navigationFrame.Controls.Add(this.ActionsNavigationPage);
            this.navigationFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationFrame.Location = new System.Drawing.Point(0, 115);
            this.navigationFrame.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.navigationFrame.Name = "navigationFrame";
            this.navigationFrame.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.VentNavigationPage,
            this.SurveyNavigationPage,
            this.ActionsNavigationPage});
            this.navigationFrame.SelectedPage = this.VentNavigationPage;
            this.navigationFrame.Size = new System.Drawing.Size(1266, 622);
            this.navigationFrame.TabIndex = 0;
            this.navigationFrame.Text = "navigationFrame";
            // 
            // VentNavigationPage
            // 
            this.VentNavigationPage.Caption = "VentNavigationPage";
            this.VentNavigationPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VentNavigationPage.Name = "VentNavigationPage";
            this.VentNavigationPage.Size = new System.Drawing.Size(1266, 622);
            // 
            // SurveyNavigationPage
            // 
            this.SurveyNavigationPage.Caption = "SurveyNavigationPage";
            this.SurveyNavigationPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SurveyNavigationPage.Name = "SurveyNavigationPage";
            this.SurveyNavigationPage.Size = new System.Drawing.Size(1266, 622);
            // 
            // ActionsNavigationPage
            // 
            this.ActionsNavigationPage.Caption = "ActionsNavigationPage";
            this.ActionsNavigationPage.Name = "ActionsNavigationPage";
            this.ActionsNavigationPage.Size = new System.Drawing.Size(1266, 622);
            // 
            // tileBar1
            // 
            this.tileBar1.AllowGlyphSkinning = true;
            this.tileBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tileBar1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.tileBar1.Groups.Add(this.tileBarGroup3);
            this.tileBar1.IndentBetweenGroups = 8;
            this.tileBar1.IndentBetweenItems = 8;
            this.tileBar1.ItemPadding = new System.Windows.Forms.Padding(6, 4, 10, 4);
            this.tileBar1.Location = new System.Drawing.Point(1156, -15);
            this.tileBar1.MaxId = 1;
            this.tileBar1.Name = "tileBar1";
            this.tileBar1.Padding = new System.Windows.Forms.Padding(24, 14, 24, 14);
            this.tileBar1.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.tileBar1.Size = new System.Drawing.Size(110, 108);
            this.tileBar1.TabIndex = 3;
            this.tileBar1.Text = "tileBar1";
            // 
            // tileBarGroup3
            // 
            this.tileBarGroup3.Items.Add(this.tileBarItemExit);
            this.tileBarGroup3.Name = "tileBarGroup3";
            // 
            // tileBarItemExit
            // 
            this.tileBarItemExit.AppearanceItem.Normal.BackColor = System.Drawing.Color.Red;
            this.tileBarItemExit.AppearanceItem.Normal.Options.UseBackColor = true;
            this.tileBarItemExit.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement12.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopCenter;
            tileItemElement12.ImageOptions.SvgImage = global::DocumentManager.Properties.Resources.CloseRed;
            tileItemElement12.Text = "Exit";
            tileItemElement12.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.BottomCenter;
            this.tileBarItemExit.Elements.Add(tileItemElement12);
            this.tileBarItemExit.Id = 0;
            this.tileBarItemExit.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Medium;
            this.tileBarItemExit.Name = "tileBarItemExit";
            this.tileBarItemExit.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileBarItemExit_ItemClick);
            // 
            // lblUser
            // 
            this.lblUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUser.AutoSize = true;
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblUser.Location = new System.Drawing.Point(1006, 53);
            this.lblUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(90, 23);
            this.lblUser.TabIndex = 7;
            this.lblUser.Text = "UserName";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(931, 53);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Welcome";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureEdit1.EditValue = global::DocumentManager.Properties.Resources.EmployeeBlue;
            this.pictureEdit1.Location = new System.Drawing.Point(896, 46);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.ZoomPercent = 90D;
            this.pictureEdit1.Size = new System.Drawing.Size(38, 36);
            this.pictureEdit1.TabIndex = 8;
            // 
            // fluentDesignFormContainer1
            // 
            this.fluentDesignFormContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fluentDesignFormContainer1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormContainer1.Name = "fluentDesignFormContainer1";
            this.fluentDesignFormContainer1.Size = new System.Drawing.Size(0, 0);
            this.fluentDesignFormContainer1.TabIndex = 0;
            // 
            // fluentDesignFormControl1
            // 
            this.fluentDesignFormControl1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormControl1.Name = "fluentDesignFormControl1";
            this.fluentDesignFormControl1.Size = new System.Drawing.Size(1264, 0);
            this.fluentDesignFormControl1.TabIndex = 11;
            this.fluentDesignFormControl1.TabStop = false;
            // 
            // frmMainLoad
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1266, 737);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tileBar1);
            this.Controls.Add(this.navigationFrame);
            this.Controls.Add(this.tileBar);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.IconOptions.Image = global::DocumentManager.Properties.Resources.SM;
            this.LookAndFeel.SkinName = "The Bezier";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMainLoad";
            this.Text = "Syncromine Document Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMainLoad_FormClosed);
            this.Load += new System.EventHandler(this.frmMainLoad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame)).EndInit();
            this.navigationFrame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileBar tileBar;
        private DevExpress.XtraBars.Navigation.NavigationFrame navigationFrame;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroupTables;
        private DevExpress.XtraBars.Navigation.TileBarItem VentTileBarItem;
        private DevExpress.XtraBars.Navigation.TileBarItem SurveyTileBarItem;
        private DevExpress.XtraBars.Navigation.NavigationPage VentNavigationPage;
        private DevExpress.XtraBars.Navigation.NavigationPage SurveyNavigationPage;
        private DevExpress.XtraBars.Navigation.TileBar tileBar1;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup3;
        private DevExpress.XtraBars.Navigation.TileBarItem tileBarItemExit;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraBars.FormAssistant formAssistant;
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer fluentDesignFormContainer1;
        //private DevExpress.XtraBars.Navigation.AccordionControl accordionControl1;
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl fluentDesignFormControl1;
        private DevExpress.XtraBars.Navigation.TileBarItem tileBarActions;
        private DevExpress.XtraBars.Navigation.NavigationPage ActionsNavigationPage;
    }
}