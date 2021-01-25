
namespace Mineware.Systems.Production.Departmental.LongHoleDrilling
{
    partial class frmProblems
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
            this.StartTime = new DevExpress.XtraEditors.TimeEdit();
            this.EndTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtSearch = new DevExpress.XtraEditors.TextEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gc1 = new DevExpress.XtraGrid.GridControl();
            this.gv1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gc1Notes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.gc3 = new DevExpress.XtraGrid.GridControl();
            this.gv3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gc3Notes = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv3)).BeginInit();
            this.SuspendLayout();
            // 
            // StartTime
            // 
            this.StartTime.EditValue = new System.DateTime(2021, 1, 14, 0, 0, 0, 0);
            this.StartTime.Location = new System.Drawing.Point(53, 25);
            this.StartTime.Name = "StartTime";
            this.StartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartTime.Properties.Mask.EditMask = "t";
            this.StartTime.Size = new System.Drawing.Size(125, 24);
            this.StartTime.TabIndex = 0;
            // 
            // EndTime
            // 
            this.EndTime.EditValue = new System.DateTime(2021, 1, 14, 0, 0, 0, 0);
            this.EndTime.Location = new System.Drawing.Point(240, 25);
            this.EndTime.Name = "EndTime";
            this.EndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EndTime.Properties.Mask.EditMask = "t";
            this.EndTime.Size = new System.Drawing.Size(125, 24);
            this.EndTime.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 28);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(27, 17);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Start";
            this.labelControl1.Click += new System.EventHandler(this.labelControl1_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(212, 28);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(22, 17);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "End";
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(19, 87);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(1017, 24);
            this.textEdit1.TabIndex = 4;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(19, 64);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(62, 17);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "Comments";
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.simpleButton1.ImageOptions.SvgImageSize = new System.Drawing.Size(18, 18);
            this.simpleButton1.Location = new System.Drawing.Point(367, 137);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(94, 29);
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "Save";
            // 
            // simpleButton2
            // 
            this.simpleButton2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.close;
            this.simpleButton2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.simpleButton2.Location = new System.Drawing.Point(503, 137);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(94, 29);
            this.simpleButton2.TabIndex = 7;
            this.simpleButton2.Text = "Close";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.EndTime);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.StartTime);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.textEdit1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 465);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1060, 180);
            this.panelControl1.TabIndex = 8;
            this.panelControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelControl1_Paint);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.panel3);
            this.layoutControl1.Controls.Add(this.txtSearch);
            this.layoutControl1.Controls.Add(this.panel1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1060, 465);
            this.layoutControl1.TabIndex = 56;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gc3);
            this.panel3.Location = new System.Drawing.Point(511, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(543, 453);
            this.panel3.TabIndex = 2;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(48, 6);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(449, 24);
            this.txtSearch.StyleController = this.layoutControl1;
            this.txtSearch.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gc1);
            this.panel1.Location = new System.Drawing.Point(6, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(491, 427);
            this.panel1.TabIndex = 8;
            // 
            // gc1
            // 
            this.gc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gc1.Location = new System.Drawing.Point(0, 0);
            this.gc1.MainView = this.gv1;
            this.gc1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gc1.Name = "gc1";
            this.gc1.Size = new System.Drawing.Size(491, 427);
            this.gc1.TabIndex = 8;
            this.gc1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv1});
            // 
            // gv1
            // 
            this.gv1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gc1Notes});
            this.gv1.DetailHeight = 458;
            this.gv1.GridControl = this.gc1;
            this.gv1.Name = "gv1";
            this.gv1.OptionsCustomization.AllowFilter = false;
            this.gv1.OptionsFilter.AllowFilterEditor = false;
            this.gv1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gv1.OptionsView.RowAutoHeight = true;
            this.gv1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gv1.OptionsView.ShowGroupPanel = false;
            this.gv1.RowHeight = 27;
            // 
            // gc1Notes
            // 
            this.gc1Notes.AppearanceHeader.BackColor = System.Drawing.Color.Transparent;
            this.gc1Notes.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gc1Notes.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gc1Notes.AppearanceHeader.Options.UseBackColor = true;
            this.gc1Notes.AppearanceHeader.Options.UseFont = true;
            this.gc1Notes.AppearanceHeader.Options.UseForeColor = true;
            this.gc1Notes.Caption = "gridColumn1";
            this.gc1Notes.FieldName = "ProblemNote";
            this.gc1Notes.MinWidth = 23;
            this.gc1Notes.Name = "gc1Notes";
            this.gc1Notes.OptionsColumn.AllowEdit = false;
            this.gc1Notes.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gc1Notes.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gc1Notes.OptionsColumn.ReadOnly = true;
            this.gc1Notes.OptionsColumn.ShowInCustomizationForm = false;
            this.gc1Notes.Visible = true;
            this.gc1Notes.VisibleIndex = 0;
            this.gc1Notes.Width = 97;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.splitterItem1,
            this.layoutControlItem4});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1060, 465);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtSearch;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(493, 26);
            this.layoutControlItem1.Text = "Search";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(39, 17);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.panel1;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(493, 429);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // splitterItem1
            // 
            this.splitterItem1.AllowHotTrack = true;
            this.splitterItem1.Location = new System.Drawing.Point(493, 0);
            this.splitterItem1.Name = "splitterItem1";
            this.splitterItem1.Size = new System.Drawing.Size(12, 455);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.panel3;
            this.layoutControlItem4.Location = new System.Drawing.Point(505, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(545, 455);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // gc3
            // 
            this.gc3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc3.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gc3.Location = new System.Drawing.Point(0, 0);
            this.gc3.MainView = this.gv3;
            this.gc3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gc3.Name = "gc3";
            this.gc3.Size = new System.Drawing.Size(543, 453);
            this.gc3.TabIndex = 11;
            this.gc3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv3});
            // 
            // gv3
            // 
            this.gv3.Appearance.ViewCaption.BackColor = System.Drawing.Color.Red;
            this.gv3.Appearance.ViewCaption.Options.UseBackColor = true;
            this.gv3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gc3Notes});
            this.gv3.DetailHeight = 458;
            this.gv3.GridControl = this.gc3;
            this.gv3.Name = "gv3";
            this.gv3.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gv3.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gv3.OptionsView.ShowGroupedColumns = true;
            this.gv3.OptionsView.ShowGroupPanel = false;
            // 
            // gc3Notes
            // 
            this.gc3Notes.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold);
            this.gc3Notes.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gc3Notes.AppearanceHeader.Options.UseFont = true;
            this.gc3Notes.AppearanceHeader.Options.UseForeColor = true;
            this.gc3Notes.Caption = "gridColumn1";
            this.gc3Notes.FieldName = "ProblemNote";
            this.gc3Notes.MinWidth = 23;
            this.gc3Notes.Name = "gc3Notes";
            this.gc3Notes.OptionsColumn.AllowEdit = false;
            this.gc3Notes.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gc3Notes.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gc3Notes.OptionsColumn.ReadOnly = true;
            this.gc3Notes.Visible = true;
            this.gc3Notes.VisibleIndex = 0;
            this.gc3Notes.Width = 97;
            // 
            // frmProblems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 645);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panelControl1);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmProblems";
            this.Text = "Problems";
            ((System.ComponentModel.ISupportInitialize)(this.StartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gc1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gc3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TimeEdit StartTime;
        private DevExpress.XtraEditors.TimeEdit EndTime;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraGrid.GridControl gc3;
        private DevExpress.XtraGrid.Views.Grid.GridView gv3;
        private DevExpress.XtraGrid.Columns.GridColumn gc3Notes;
        private DevExpress.XtraEditors.TextEdit txtSearch;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gc1;
        private DevExpress.XtraGrid.Views.Grid.GridView gv1;
        private DevExpress.XtraGrid.Columns.GridColumn gc1Notes;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.SplitterItem splitterItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
    }
}