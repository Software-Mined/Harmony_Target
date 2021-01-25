namespace Mineware.Systems.Production.SysAdmin
{
    partial class FrmWorkplaces
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWorkplaces));
            this.rcWorkplace = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
            this.rpWorkplace = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWPID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.rcWorkplace)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rcWorkplace
            // 
            this.rcWorkplace.AllowKeyTips = false;
            this.rcWorkplace.AllowMdiChildButtons = false;
            this.rcWorkplace.AllowMinimizeRibbon = false;
            this.rcWorkplace.AllowTrimPageText = false;
            this.rcWorkplace.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.rcWorkplace.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.DarkBlue;
            this.rcWorkplace.ExpandCollapseItem.Id = 0;
            this.rcWorkplace.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rcWorkplace.ExpandCollapseItem,
            this.rcWorkplace.SearchEditItem,
            this.btnSave,
            this.btnCancel});
            this.rcWorkplace.Location = new System.Drawing.Point(0, 0);
            this.rcWorkplace.MaxItemId = 27;
            this.rcWorkplace.Name = "rcWorkplace";
            this.rcWorkplace.OptionsPageCategories.ShowCaptions = false;
            this.rcWorkplace.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpWorkplace});
            this.rcWorkplace.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcWorkplace.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcWorkplace.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.rcWorkplace.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Show;
            this.rcWorkplace.ShowToolbarCustomizeItem = false;
            this.rcWorkplace.Size = new System.Drawing.Size(857, 135);
            this.rcWorkplace.Toolbar.ShowCustomizeItem = false;
            this.rcWorkplace.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // btnSave
            // 
            this.btnSave.Caption = "Save";
            this.btnSave.Id = 7;
            this.btnSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.Image")));
            this.btnSave.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.LargeImage")));
            this.btnSave.LargeWidth = 50;
            this.btnSave.Name = "btnSave";
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Caption = "Cancel";
            this.btnCancel.Id = 22;
            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
            this.btnCancel.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.LargeImage")));
            this.btnCancel.LargeWidth = 50;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // rpWorkplace
            // 
            this.rpWorkplace.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            this.rpWorkplace.Name = "rpWorkplace";
            this.rpWorkplace.Text = "Workplace";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnSave);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnCancel);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtWPID);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 135);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(857, 353);
            this.panel1.TabIndex = 2;
            // 
            // radioButton1
            // 
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(49, 72);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(248, 25);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Stoping";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(369, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Workplace Description";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(150, 208);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(116, 25);
            this.comboBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Workplace ID";
            // 
            // txtWPID
            // 
            this.txtWPID.Enabled = false;
            this.txtWPID.Location = new System.Drawing.Point(104, 17);
            this.txtWPID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtWPID.Name = "txtWPID";
            this.txtWPID.Size = new System.Drawing.Size(116, 25);
            this.txtWPID.TabIndex = 0;
            // 
            // FrmWorkplaces
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 488);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rcWorkplace);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmWorkplaces";
            this.Text = "Add Workplaces";
            this.Load += new System.EventHandler(this.FrmWorkplaces_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rcWorkplace)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl rcWorkplace;
        public DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.BarButtonItem btnCancel;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpWorkplace;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWPID;
        private System.Windows.Forms.Label label2;
        //private SearchLookUpEdit searchLookUpEdit1;
        //private GridView searchLookUpEdit1View;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}