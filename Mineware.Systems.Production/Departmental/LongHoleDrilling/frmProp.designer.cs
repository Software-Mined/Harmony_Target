namespace Mineware.Systems.Production.Departmental.LongHoleDrilling
{
    partial class frmProp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProp));
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Delaytxt = new System.Windows.Forms.NumericUpDown();
            this.PevHolelabel = new System.Windows.Forms.Label();
            this.Starttm = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Ringlabel = new System.Windows.Forms.Label();
            this.WPlabel = new System.Windows.Forms.Label();
            this.PrevWPList = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colWorkplace = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRing = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHole = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbonControl2 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem5 = new DevExpress.XtraBars.BarEditItem();
            this.barEditItem6 = new DevExpress.XtraBars.BarEditItem();
            this.barEditItem7 = new DevExpress.XtraBars.BarEditItem();
            this.barEditItem8 = new DevExpress.XtraBars.BarEditItem();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.label8 = new System.Windows.Forms.Label();
            this.Holelbl = new System.Windows.Forms.Label();
            this.Machlabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Delaytxt)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(563, 345);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 17);
            this.label7.TabIndex = 53;
            this.label7.Text = "(Working Days)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(444, 308);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 19);
            this.label6.TabIndex = 52;
            this.label6.Text = "Transport Days";
            // 
            // Delaytxt
            // 
            this.Delaytxt.Location = new System.Drawing.Point(491, 338);
            this.Delaytxt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Delaytxt.Name = "Delaytxt";
            this.Delaytxt.Size = new System.Drawing.Size(57, 25);
            this.Delaytxt.TabIndex = 51;
            // 
            // PevHolelabel
            // 
            this.PevHolelabel.AutoSize = true;
            this.PevHolelabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PevHolelabel.Location = new System.Drawing.Point(359, 103);
            this.PevHolelabel.Name = "PevHolelabel";
            this.PevHolelabel.Size = new System.Drawing.Size(166, 19);
            this.PevHolelabel.TabIndex = 50;
            this.PevHolelabel.Text = "Machine Commencement";
            this.PevHolelabel.Visible = false;
            this.PevHolelabel.TextChanged += new System.EventHandler(this.PevHolelabel_TextChanged);
            // 
            // Starttm
            // 
            this.Starttm.CustomFormat = "dd MMM yyyy";
            this.Starttm.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.Starttm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Starttm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Starttm.Location = new System.Drawing.Point(412, 251);
            this.Starttm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Starttm.Name = "Starttm";
            this.Starttm.Size = new System.Drawing.Size(144, 23);
            this.Starttm.TabIndex = 49;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(359, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(186, 19);
            this.label5.TabIndex = 32;
            this.label5.Text = "Available Date (Before Move)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(19, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 19);
            this.label4.TabIndex = 31;
            this.label4.Text = "Preceding  Hole";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(129, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 17);
            this.label3.TabIndex = 30;
            this.label3.Text = "Ring";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(75, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 29;
            this.label2.Text = "Workplace";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 28;
            this.label1.Text = "Machine";
            // 
            // Ringlabel
            // 
            this.Ringlabel.AutoSize = true;
            this.Ringlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ringlabel.ForeColor = System.Drawing.Color.Blue;
            this.Ringlabel.Location = new System.Drawing.Point(196, 94);
            this.Ringlabel.Name = "Ringlabel";
            this.Ringlabel.Size = new System.Drawing.Size(46, 17);
            this.Ringlabel.TabIndex = 27;
            this.Ringlabel.Text = "label2";
            // 
            // WPlabel
            // 
            this.WPlabel.AutoSize = true;
            this.WPlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WPlabel.ForeColor = System.Drawing.Color.Blue;
            this.WPlabel.Location = new System.Drawing.Point(161, 59);
            this.WPlabel.Name = "WPlabel";
            this.WPlabel.Size = new System.Drawing.Size(46, 17);
            this.WPlabel.TabIndex = 1;
            this.WPlabel.Text = "label2";
            // 
            // PrevWPList
            // 
            this.PrevWPList.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.PrevWPList.FormattingEnabled = true;
            this.PrevWPList.ItemHeight = 16;
            this.PrevWPList.Location = new System.Drawing.Point(531, 24);
            this.PrevWPList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PrevWPList.Name = "PrevWPList";
            this.PrevWPList.Size = new System.Drawing.Size(248, 148);
            this.PrevWPList.TabIndex = 3;
            this.PrevWPList.Visible = false;
            this.PrevWPList.SelectedIndexChanged += new System.EventHandler(this.PrevWPList_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridControl1);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.Holelbl);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.Delaytxt);
            this.panel2.Controls.Add(this.PevHolelabel);
            this.panel2.Controls.Add(this.Starttm);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.Ringlabel);
            this.panel2.Controls.Add(this.Machlabel);
            this.panel2.Controls.Add(this.WPlabel);
            this.panel2.Controls.Add(this.PrevWPList);
            this.panel2.Location = new System.Drawing.Point(12, 104);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(693, 414);
            this.panel2.TabIndex = 28;
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(23, 208);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.ribbonControl2;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(314, 185);
            this.gridControl1.TabIndex = 56;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colWorkplace,
            this.colRing,
            this.colHole});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            // 
            // colWorkplace
            // 
            this.colWorkplace.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colWorkplace.AppearanceHeader.Options.UseForeColor = true;
            this.colWorkplace.Caption = "Workplace";
            this.colWorkplace.MinWidth = 25;
            this.colWorkplace.Name = "colWorkplace";
            this.colWorkplace.OptionsColumn.AllowEdit = false;
            this.colWorkplace.Visible = true;
            this.colWorkplace.VisibleIndex = 0;
            this.colWorkplace.Width = 161;
            // 
            // colRing
            // 
            this.colRing.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colRing.AppearanceHeader.Options.UseForeColor = true;
            this.colRing.Caption = "Ring";
            this.colRing.MinWidth = 25;
            this.colRing.Name = "colRing";
            this.colRing.OptionsColumn.AllowEdit = false;
            this.colRing.Visible = true;
            this.colRing.VisibleIndex = 1;
            this.colRing.Width = 66;
            // 
            // colHole
            // 
            this.colHole.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colHole.AppearanceHeader.Options.UseForeColor = true;
            this.colHole.Caption = "Hole";
            this.colHole.MinWidth = 25;
            this.colHole.Name = "colHole";
            this.colHole.OptionsColumn.AllowEdit = false;
            this.colHole.Visible = true;
            this.colHole.VisibleIndex = 2;
            this.colHole.Width = 67;
            // 
            // ribbonControl2
            // 
            this.ribbonControl2.AllowKeyTips = false;
            this.ribbonControl2.AllowMdiChildButtons = false;
            this.ribbonControl2.AllowMinimizeRibbon = false;
            this.ribbonControl2.AllowTrimPageText = false;
            this.ribbonControl2.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.DarkBlue;
            this.ribbonControl2.DrawGroupsBorderMode = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl2.ExpandCollapseItem.Id = 0;
            this.ribbonControl2.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl2.ExpandCollapseItem,
            this.ribbonControl2.SearchEditItem,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barEditItem5,
            this.barEditItem6,
            this.barEditItem7,
            this.barEditItem8});
            this.ribbonControl2.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ribbonControl2.MaxItemId = 13;
            this.ribbonControl2.Name = "ribbonControl2";
            this.ribbonControl2.OptionsPageCategories.ShowCaptions = false;
            this.ribbonControl2.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage2});
            this.ribbonControl2.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl2.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl2.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl2.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl2.ShowToolbarCustomizeItem = false;
            this.ribbonControl2.Size = new System.Drawing.Size(717, 87);
            this.ribbonControl2.Toolbar.ShowCustomizeItem = false;
            this.ribbonControl2.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            this.ribbonControl2.TransparentEditorsMode = DevExpress.Utils.DefaultBoolean.False;
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "                                         ";
            this.barButtonItem3.Id = 3;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Save";
            this.barButtonItem4.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.barButtonItem4.Id = 5;
            this.barButtonItem4.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "Add Image";
            this.barButtonItem5.Id = 7;
            this.barButtonItem5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem5.ImageOptions.Image")));
            this.barButtonItem5.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem5.ImageOptions.LargeImage")));
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "Print Grid";
            this.barButtonItem6.Id = 8;
            this.barButtonItem6.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem6.ImageOptions.Image")));
            this.barButtonItem6.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem6.ImageOptions.LargeImage")));
            this.barButtonItem6.Name = "barButtonItem6";
            // 
            // barEditItem5
            // 
            this.barEditItem5.Caption = "                                                                                 " +
    "            ";
            this.barEditItem5.Edit = null;
            this.barEditItem5.EditWidth = 1;
            this.barEditItem5.Id = 9;
            this.barEditItem5.Name = "barEditItem5";
            // 
            // barEditItem6
            // 
            this.barEditItem6.Caption = "                                                                                 " +
    "         ";
            this.barEditItem6.Edit = null;
            this.barEditItem6.EditWidth = 1;
            this.barEditItem6.Id = 10;
            this.barEditItem6.Name = "barEditItem6";
            // 
            // barEditItem7
            // 
            this.barEditItem7.Caption = "                                                                           ";
            this.barEditItem7.Edit = null;
            this.barEditItem7.EditWidth = 1;
            this.barEditItem7.Id = 11;
            this.barEditItem7.Name = "barEditItem7";
            // 
            // barEditItem8
            // 
            this.barEditItem8.Caption = "                                                                      ";
            this.barEditItem8.Edit = null;
            this.barEditItem8.EditWidth = 1;
            this.barEditItem8.Id = 12;
            this.barEditItem8.Name = "barEditItem8";
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "ribbonPage1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem4);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Options";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(161, 129);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 17);
            this.label8.TabIndex = 55;
            this.label8.Text = "Hole";
            // 
            // Holelbl
            // 
            this.Holelbl.AutoSize = true;
            this.Holelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Holelbl.ForeColor = System.Drawing.Color.Blue;
            this.Holelbl.Location = new System.Drawing.Point(228, 129);
            this.Holelbl.Name = "Holelbl";
            this.Holelbl.Size = new System.Drawing.Size(51, 17);
            this.Holelbl.TabIndex = 54;
            this.Holelbl.Text = "Holelbl";
            // 
            // Machlabel
            // 
            this.Machlabel.AutoSize = true;
            this.Machlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Machlabel.ForeColor = System.Drawing.Color.Blue;
            this.Machlabel.Location = new System.Drawing.Point(91, 25);
            this.Machlabel.Name = "Machlabel";
            this.Machlabel.Size = new System.Drawing.Size(46, 17);
            this.Machlabel.TabIndex = 0;
            this.Machlabel.Text = "label1";
            // 
            // frmProp
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 531);
            this.Controls.Add(this.ribbonControl2);
            this.Controls.Add(this.panel2);
            this.IconOptions.ShowIcon = false;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmProp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.Load += new System.EventHandler(this.frmGeologyProp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Delaytxt)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown Delaytxt;
        private System.Windows.Forms.Label PevHolelabel;
        public System.Windows.Forms.DateTimePicker Starttm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label Ringlabel;
        public System.Windows.Forms.Label WPlabel;
        private System.Windows.Forms.ListBox PrevWPList;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Label Machlabel;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarEditItem barEditItem5;
        private DevExpress.XtraBars.BarEditItem barEditItem6;
        private DevExpress.XtraBars.BarEditItem barEditItem7;
        private DevExpress.XtraBars.BarEditItem barEditItem8;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label Holelbl;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colWorkplace;
        private DevExpress.XtraGrid.Columns.GridColumn colRing;
        private DevExpress.XtraGrid.Columns.GridColumn colHole;
    }
}