namespace Mineware.Systems.Production.OreflowDiagram
{
    partial class ucTrammingBooking
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gcTramBooking = new DevExpress.XtraGrid.GridControl();
            this.gvTramBooking = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colBHID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colBHName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colDSTons = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDSHopper = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colASTons = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colASHopper = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand4 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colNSTons = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colNSHopper = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.BHRadio = new DevExpress.XtraEditors.RadioGroup();
            this.TramDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbxBHlvl = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.gcTramBooking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTramBooking)).BeginInit();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BHRadio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gcTramBooking
            // 
            this.gcTramBooking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTramBooking.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcTramBooking.Location = new System.Drawing.Point(207, 0);
            this.gcTramBooking.MainView = this.gvTramBooking;
            this.gcTramBooking.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcTramBooking.Name = "gcTramBooking";
            this.gcTramBooking.Size = new System.Drawing.Size(900, 670);
            this.gcTramBooking.TabIndex = 8;
            this.gcTramBooking.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTramBooking});
            // 
            // gvTramBooking
            // 
            this.gvTramBooking.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand3,
            this.gridBand4});
            this.gvTramBooking.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colBHID,
            this.colBHName,
            this.colDSTons,
            this.colDSHopper,
            this.colASTons,
            this.colASHopper,
            this.colNSTons,
            this.colNSHopper});
            this.gvTramBooking.DetailHeight = 431;
            this.gvTramBooking.GridControl = this.gcTramBooking;
            this.gvTramBooking.Name = "gvTramBooking";
            this.gvTramBooking.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvTramBooking.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvTramBooking.OptionsBehavior.Editable = false;
            this.gvTramBooking.OptionsBehavior.ReadOnly = true;
            this.gvTramBooking.OptionsView.ColumnAutoWidth = false;
            this.gvTramBooking.OptionsView.ShowGroupPanel = false;
            this.gvTramBooking.OptionsView.ShowIndicator = false;
            this.gvTramBooking.DoubleClick += new System.EventHandler(this.gvTramBooking_DoubleClick);
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = " ";
            this.gridBand1.Columns.Add(this.colBHID);
            this.gridBand1.Columns.Add(this.colBHName);
            this.gridBand1.MinWidth = 13;
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 320;
            // 
            // colBHID
            // 
            this.colBHID.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colBHID.AppearanceHeader.Options.UseForeColor = true;
            this.colBHID.Caption = "ID";
            this.colBHID.MinWidth = 25;
            this.colBHID.Name = "colBHID";
            this.colBHID.OptionsColumn.AllowEdit = false;
            this.colBHID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colBHID.OptionsColumn.AllowMove = false;
            this.colBHID.OptionsColumn.AllowSize = false;
            this.colBHID.OptionsColumn.FixedWidth = true;
            this.colBHID.OptionsFilter.AllowAutoFilter = false;
            this.colBHID.OptionsFilter.AllowFilter = false;
            this.colBHID.Visible = true;
            this.colBHID.Width = 120;
            // 
            // colBHName
            // 
            this.colBHName.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colBHName.AppearanceHeader.Options.UseForeColor = true;
            this.colBHName.Caption = "Name";
            this.colBHName.MinWidth = 25;
            this.colBHName.Name = "colBHName";
            this.colBHName.OptionsColumn.AllowEdit = false;
            this.colBHName.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colBHName.OptionsColumn.AllowMove = false;
            this.colBHName.OptionsColumn.AllowSize = false;
            this.colBHName.OptionsColumn.FixedWidth = true;
            this.colBHName.OptionsFilter.AllowAutoFilter = false;
            this.colBHName.OptionsFilter.AllowFilter = false;
            this.colBHName.Visible = true;
            this.colBHName.Width = 200;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gridBand2.AppearanceHeader.Options.UseForeColor = true;
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "Day Shift";
            this.gridBand2.Columns.Add(this.colDSTons);
            this.gridBand2.Columns.Add(this.colDSHopper);
            this.gridBand2.MinWidth = 13;
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 1;
            this.gridBand2.Width = 240;
            // 
            // colDSTons
            // 
            this.colDSTons.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDSTons.AppearanceHeader.Options.UseForeColor = true;
            this.colDSTons.AppearanceHeader.Options.UseTextOptions = true;
            this.colDSTons.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDSTons.Caption = "DSTons";
            this.colDSTons.MinWidth = 25;
            this.colDSTons.Name = "colDSTons";
            this.colDSTons.OptionsColumn.AllowEdit = false;
            this.colDSTons.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDSTons.OptionsColumn.AllowMove = false;
            this.colDSTons.OptionsColumn.AllowSize = false;
            this.colDSTons.OptionsColumn.FixedWidth = true;
            this.colDSTons.OptionsFilter.AllowAutoFilter = false;
            this.colDSTons.OptionsFilter.AllowFilter = false;
            this.colDSTons.Visible = true;
            this.colDSTons.Width = 107;
            // 
            // colDSHopper
            // 
            this.colDSHopper.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDSHopper.AppearanceHeader.Options.UseForeColor = true;
            this.colDSHopper.AppearanceHeader.Options.UseTextOptions = true;
            this.colDSHopper.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDSHopper.Caption = "DSHopper";
            this.colDSHopper.MinWidth = 25;
            this.colDSHopper.Name = "colDSHopper";
            this.colDSHopper.OptionsColumn.AllowEdit = false;
            this.colDSHopper.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDSHopper.OptionsColumn.AllowMove = false;
            this.colDSHopper.OptionsColumn.AllowSize = false;
            this.colDSHopper.OptionsColumn.FixedWidth = true;
            this.colDSHopper.OptionsFilter.AllowAutoFilter = false;
            this.colDSHopper.OptionsFilter.AllowFilter = false;
            this.colDSHopper.Visible = true;
            this.colDSHopper.Width = 133;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gridBand3.AppearanceHeader.Options.UseForeColor = true;
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.Caption = "Afternoon Shift";
            this.gridBand3.Columns.Add(this.colASTons);
            this.gridBand3.Columns.Add(this.colASHopper);
            this.gridBand3.MinWidth = 13;
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 2;
            this.gridBand3.Width = 220;
            // 
            // colASTons
            // 
            this.colASTons.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colASTons.AppearanceHeader.Options.UseForeColor = true;
            this.colASTons.AppearanceHeader.Options.UseTextOptions = true;
            this.colASTons.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colASTons.Caption = "ASTons";
            this.colASTons.MinWidth = 25;
            this.colASTons.Name = "colASTons";
            this.colASTons.OptionsColumn.AllowEdit = false;
            this.colASTons.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colASTons.OptionsColumn.AllowMove = false;
            this.colASTons.OptionsColumn.AllowSize = false;
            this.colASTons.OptionsColumn.FixedWidth = true;
            this.colASTons.OptionsFilter.AllowAutoFilter = false;
            this.colASTons.OptionsFilter.AllowFilter = false;
            this.colASTons.Visible = true;
            this.colASTons.Width = 120;
            // 
            // colASHopper
            // 
            this.colASHopper.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colASHopper.AppearanceHeader.Options.UseForeColor = true;
            this.colASHopper.AppearanceHeader.Options.UseTextOptions = true;
            this.colASHopper.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colASHopper.Caption = "ASHopper";
            this.colASHopper.MinWidth = 25;
            this.colASHopper.Name = "colASHopper";
            this.colASHopper.OptionsColumn.AllowEdit = false;
            this.colASHopper.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colASHopper.OptionsColumn.AllowMove = false;
            this.colASHopper.OptionsColumn.AllowSize = false;
            this.colASHopper.OptionsColumn.FixedWidth = true;
            this.colASHopper.OptionsFilter.AllowAutoFilter = false;
            this.colASHopper.OptionsFilter.AllowFilter = false;
            this.colASHopper.Visible = true;
            this.colASHopper.Width = 100;
            // 
            // gridBand4
            // 
            this.gridBand4.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gridBand4.AppearanceHeader.Options.UseForeColor = true;
            this.gridBand4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand4.Caption = "Night Shift";
            this.gridBand4.Columns.Add(this.colNSTons);
            this.gridBand4.Columns.Add(this.colNSHopper);
            this.gridBand4.MinWidth = 13;
            this.gridBand4.Name = "gridBand4";
            this.gridBand4.VisibleIndex = 3;
            this.gridBand4.Width = 207;
            // 
            // colNSTons
            // 
            this.colNSTons.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colNSTons.AppearanceHeader.Options.UseForeColor = true;
            this.colNSTons.AppearanceHeader.Options.UseTextOptions = true;
            this.colNSTons.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNSTons.Caption = "NSTons";
            this.colNSTons.MinWidth = 25;
            this.colNSTons.Name = "colNSTons";
            this.colNSTons.OptionsColumn.AllowEdit = false;
            this.colNSTons.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colNSTons.OptionsColumn.AllowMove = false;
            this.colNSTons.OptionsColumn.AllowSize = false;
            this.colNSTons.OptionsColumn.FixedWidth = true;
            this.colNSTons.OptionsFilter.AllowAutoFilter = false;
            this.colNSTons.OptionsFilter.AllowFilter = false;
            this.colNSTons.Visible = true;
            this.colNSTons.Width = 107;
            // 
            // colNSHopper
            // 
            this.colNSHopper.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colNSHopper.AppearanceHeader.Options.UseForeColor = true;
            this.colNSHopper.AppearanceHeader.Options.UseTextOptions = true;
            this.colNSHopper.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNSHopper.Caption = "NSHopper";
            this.colNSHopper.MinWidth = 25;
            this.colNSHopper.Name = "colNSHopper";
            this.colNSHopper.OptionsColumn.AllowEdit = false;
            this.colNSHopper.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colNSHopper.OptionsColumn.AllowMove = false;
            this.colNSHopper.OptionsColumn.AllowSize = false;
            this.colNSHopper.OptionsColumn.FixedWidth = true;
            this.colNSHopper.OptionsFilter.AllowAutoFilter = false;
            this.colNSHopper.OptionsFilter.AllowFilter = false;
            this.colNSHopper.Visible = true;
            this.colNSHopper.Width = 100;
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.Controls.Add(this.BHRadio);
            this.pnlFilter.Controls.Add(this.TramDate);
            this.pnlFilter.Controls.Add(this.lbxBHlvl);
            this.pnlFilter.Controls.Add(this.label3);
            this.pnlFilter.Controls.Add(this.label2);
            this.pnlFilter.Controls.Add(this.label1);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(207, 670);
            this.pnlFilter.TabIndex = 9;
            // 
            // BHRadio
            // 
            this.BHRadio.Location = new System.Drawing.Point(27, 38);
            this.BHRadio.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BHRadio.Name = "BHRadio";
            this.BHRadio.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Reef"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Waste")});
            this.BHRadio.Size = new System.Drawing.Size(152, 97);
            this.BHRadio.TabIndex = 0;
            this.BHRadio.SelectedIndexChanged += new System.EventHandler(this.BHRadio_SelectedIndexChanged);
            // 
            // TramDate
            // 
            this.TramDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.TramDate.Location = new System.Drawing.Point(27, 184);
            this.TramDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TramDate.Name = "TramDate";
            this.TramDate.Size = new System.Drawing.Size(152, 22);
            this.TramDate.TabIndex = 1;
            this.TramDate.ValueChanged += new System.EventHandler(this.TramDate_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Reef/Waste";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tramming Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Level";
            // 
            // lbxBHlvl
            // 
            this.lbxBHlvl.FormattingEnabled = true;
            this.lbxBHlvl.ItemHeight = 16;
            this.lbxBHlvl.Location = new System.Drawing.Point(27, 256);
            this.lbxBHlvl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbxBHlvl.Name = "lbxBHlvl";
            this.lbxBHlvl.Size = new System.Drawing.Size(152, 292);
            this.lbxBHlvl.TabIndex = 2;
            this.lbxBHlvl.DoubleClick += new System.EventHandler(this.lbxBHlvl_DoubleClick);
            // 
            // ucTrammingBooking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcTramBooking);
            this.Controls.Add(this.pnlFilter);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ucTrammingBooking";
            this.Size = new System.Drawing.Size(1107, 670);
            this.Load += new System.EventHandler(this.ucTrammingBooking_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcTramBooking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTramBooking)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BHRadio.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcTramBooking;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvTramBooking;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colBHID;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colBHName;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDSTons;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDSHopper;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colASTons;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colASHopper;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colNSTons;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colNSHopper;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker TramDate;
        private DevExpress.XtraEditors.RadioGroup BHRadio;
        private System.Windows.Forms.ListBox lbxBHlvl;
    }
}
