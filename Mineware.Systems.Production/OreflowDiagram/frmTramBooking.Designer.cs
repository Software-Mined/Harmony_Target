namespace Mineware.Systems.Production.OreflowDiagram
{
    partial class frmTramBooking
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTramBooking));
            this.gcTramBooking = new DevExpress.XtraGrid.GridControl();
            this.gvTramBooking = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colShift = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDestination = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colHoppers = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colHopperFactor = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colTons = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colLoco = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCrew = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDate = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.TramDateProp = new System.Windows.Forms.DateTimePicker();
            this.ShiftRadio = new DevExpress.XtraEditors.RadioGroup();
            this.LocoBox = new System.Windows.Forms.ListBox();
            this.CrewBox = new System.Windows.Forms.ListBox();
            this.HFactcmb = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HoppersEdit = new DevExpress.XtraEditors.TextEdit();
            this.TonsEdit = new DevExpress.XtraEditors.TextEdit();
            this.DestRadio = new DevExpress.XtraEditors.RadioGroup();
            this.btnAddBooking = new DevExpress.XtraEditors.SimpleButton();
            this.btnDestAdd = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.label10 = new System.Windows.Forms.Label();
            this.txtProbNote = new System.Windows.Forms.TextBox();
            this.btnAddProb = new DevExpress.XtraEditors.SimpleButton();
            this.spinProb = new DevExpress.XtraEditors.SpinEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbProb = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gcTramProb = new DevExpress.XtraGrid.GridControl();
            this.gvTramProb = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colProbShift = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colProbDescription = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colProbHours = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colProbNotes = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.ShiftProbRadio = new DevExpress.XtraEditors.RadioGroup();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.BHLBL = new System.Windows.Forms.Label();
            this.lblDisp = new System.Windows.Forms.Label();
            this.btnDestDel = new DevExpress.XtraEditors.SimpleButton();
            this.label12 = new System.Windows.Forms.Label();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcTramBooking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTramBooking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftRadio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HoppersEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TonsEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DestRadio.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinProb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTramProb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTramProb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftProbRadio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gcTramBooking
            // 
            this.gcTramBooking.Location = new System.Drawing.Point(18, 51);
            this.gcTramBooking.MainView = this.gvTramBooking;
            this.gcTramBooking.Name = "gcTramBooking";
            this.gcTramBooking.Size = new System.Drawing.Size(1127, 171);
            this.gcTramBooking.TabIndex = 9;
            this.gcTramBooking.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTramBooking});
            // 
            // gvTramBooking
            // 
            this.gvTramBooking.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.gvTramBooking.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colShift,
            this.colDestination,
            this.colHoppers,
            this.colHopperFactor,
            this.colTons,
            this.colLoco,
            this.colCrew,
            this.colDate});
            this.gvTramBooking.DetailHeight = 458;
            this.gvTramBooking.GridControl = this.gcTramBooking;
            this.gvTramBooking.Name = "gvTramBooking";
            this.gvTramBooking.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvTramBooking.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvTramBooking.OptionsBehavior.Editable = false;
            this.gvTramBooking.OptionsBehavior.ReadOnly = true;
            this.gvTramBooking.OptionsView.ShowBands = false;
            this.gvTramBooking.OptionsView.ShowGroupPanel = false;
            this.gvTramBooking.OptionsView.ShowIndicator = false;
            this.gvTramBooking.DoubleClick += new System.EventHandler(this.gvTramBooking_DoubleClick);
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = " ";
            this.gridBand1.Columns.Add(this.colShift);
            this.gridBand1.Columns.Add(this.colDestination);
            this.gridBand1.Columns.Add(this.colHoppers);
            this.gridBand1.Columns.Add(this.colHopperFactor);
            this.gridBand1.Columns.Add(this.colTons);
            this.gridBand1.Columns.Add(this.colLoco);
            this.gridBand1.Columns.Add(this.colCrew);
            this.gridBand1.Columns.Add(this.colDate);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 987;
            // 
            // colShift
            // 
            this.colShift.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colShift.AppearanceHeader.Options.UseForeColor = true;
            this.colShift.Caption = "Shift";
            this.colShift.MinWidth = 25;
            this.colShift.Name = "colShift";
            this.colShift.OptionsColumn.AllowEdit = false;
            this.colShift.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colShift.OptionsColumn.AllowMove = false;
            this.colShift.OptionsColumn.AllowSize = false;
            this.colShift.OptionsColumn.FixedWidth = true;
            this.colShift.OptionsFilter.AllowAutoFilter = false;
            this.colShift.OptionsFilter.AllowFilter = false;
            this.colShift.Visible = true;
            this.colShift.Width = 120;
            // 
            // colDestination
            // 
            this.colDestination.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDestination.AppearanceHeader.Options.UseForeColor = true;
            this.colDestination.Caption = "Destination";
            this.colDestination.MinWidth = 25;
            this.colDestination.Name = "colDestination";
            this.colDestination.OptionsColumn.AllowEdit = false;
            this.colDestination.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDestination.OptionsColumn.AllowMove = false;
            this.colDestination.OptionsColumn.AllowSize = false;
            this.colDestination.OptionsColumn.FixedWidth = true;
            this.colDestination.OptionsFilter.AllowAutoFilter = false;
            this.colDestination.OptionsFilter.AllowFilter = false;
            this.colDestination.Visible = true;
            this.colDestination.Width = 200;
            // 
            // colHoppers
            // 
            this.colHoppers.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colHoppers.AppearanceHeader.Options.UseForeColor = true;
            this.colHoppers.AppearanceHeader.Options.UseTextOptions = true;
            this.colHoppers.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colHoppers.Caption = "Hoppers";
            this.colHoppers.MinWidth = 25;
            this.colHoppers.Name = "colHoppers";
            this.colHoppers.OptionsColumn.AllowEdit = false;
            this.colHoppers.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colHoppers.OptionsColumn.AllowMove = false;
            this.colHoppers.OptionsColumn.AllowSize = false;
            this.colHoppers.OptionsColumn.FixedWidth = true;
            this.colHoppers.OptionsFilter.AllowAutoFilter = false;
            this.colHoppers.OptionsFilter.AllowFilter = false;
            this.colHoppers.Visible = true;
            this.colHoppers.Width = 107;
            // 
            // colHopperFactor
            // 
            this.colHopperFactor.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colHopperFactor.AppearanceHeader.Options.UseForeColor = true;
            this.colHopperFactor.AppearanceHeader.Options.UseTextOptions = true;
            this.colHopperFactor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colHopperFactor.Caption = "Hopper Factor";
            this.colHopperFactor.MinWidth = 25;
            this.colHopperFactor.Name = "colHopperFactor";
            this.colHopperFactor.OptionsColumn.AllowEdit = false;
            this.colHopperFactor.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colHopperFactor.OptionsColumn.AllowMove = false;
            this.colHopperFactor.OptionsColumn.AllowSize = false;
            this.colHopperFactor.OptionsColumn.FixedWidth = true;
            this.colHopperFactor.OptionsFilter.AllowAutoFilter = false;
            this.colHopperFactor.OptionsFilter.AllowFilter = false;
            this.colHopperFactor.Visible = true;
            this.colHopperFactor.Width = 133;
            // 
            // colTons
            // 
            this.colTons.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colTons.AppearanceHeader.Options.UseForeColor = true;
            this.colTons.AppearanceHeader.Options.UseTextOptions = true;
            this.colTons.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTons.Caption = "Tons";
            this.colTons.MinWidth = 25;
            this.colTons.Name = "colTons";
            this.colTons.OptionsColumn.AllowEdit = false;
            this.colTons.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colTons.OptionsColumn.AllowMove = false;
            this.colTons.OptionsColumn.AllowSize = false;
            this.colTons.OptionsColumn.FixedWidth = true;
            this.colTons.OptionsFilter.AllowAutoFilter = false;
            this.colTons.OptionsFilter.AllowFilter = false;
            this.colTons.Visible = true;
            this.colTons.Width = 120;
            // 
            // colLoco
            // 
            this.colLoco.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colLoco.AppearanceHeader.Options.UseForeColor = true;
            this.colLoco.AppearanceHeader.Options.UseTextOptions = true;
            this.colLoco.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLoco.Caption = "Loco";
            this.colLoco.MinWidth = 25;
            this.colLoco.Name = "colLoco";
            this.colLoco.OptionsColumn.AllowEdit = false;
            this.colLoco.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colLoco.OptionsColumn.AllowMove = false;
            this.colLoco.OptionsColumn.AllowSize = false;
            this.colLoco.OptionsColumn.FixedWidth = true;
            this.colLoco.OptionsFilter.AllowAutoFilter = false;
            this.colLoco.OptionsFilter.AllowFilter = false;
            this.colLoco.Visible = true;
            this.colLoco.Width = 100;
            // 
            // colCrew
            // 
            this.colCrew.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colCrew.AppearanceHeader.Options.UseForeColor = true;
            this.colCrew.AppearanceHeader.Options.UseTextOptions = true;
            this.colCrew.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCrew.Caption = "Crew";
            this.colCrew.MinWidth = 25;
            this.colCrew.Name = "colCrew";
            this.colCrew.OptionsColumn.AllowEdit = false;
            this.colCrew.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colCrew.OptionsColumn.AllowMove = false;
            this.colCrew.OptionsColumn.AllowSize = false;
            this.colCrew.OptionsColumn.FixedWidth = true;
            this.colCrew.OptionsFilter.AllowAutoFilter = false;
            this.colCrew.OptionsFilter.AllowFilter = false;
            this.colCrew.Visible = true;
            this.colCrew.Width = 107;
            // 
            // colDate
            // 
            this.colDate.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDate.AppearanceHeader.Options.UseForeColor = true;
            this.colDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDate.Caption = "Date";
            this.colDate.DisplayFormat.FormatString = "dd-MMM-yyyy";
            this.colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colDate.MinWidth = 25;
            this.colDate.Name = "colDate";
            this.colDate.OptionsColumn.AllowEdit = false;
            this.colDate.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDate.OptionsColumn.AllowMove = false;
            this.colDate.OptionsColumn.AllowSize = false;
            this.colDate.OptionsColumn.FixedWidth = true;
            this.colDate.OptionsFilter.AllowAutoFilter = false;
            this.colDate.OptionsFilter.AllowFilter = false;
            this.colDate.Visible = true;
            this.colDate.Width = 100;
            // 
            // TramDateProp
            // 
            this.TramDateProp.CalendarForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TramDateProp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.TramDateProp.Location = new System.Drawing.Point(568, 7);
            this.TramDateProp.Name = "TramDateProp";
            this.TramDateProp.Size = new System.Drawing.Size(125, 25);
            this.TramDateProp.TabIndex = 10;
            this.TramDateProp.ValueChanged += new System.EventHandler(this.TramDateProp_ValueChanged);
            // 
            // ShiftRadio
            // 
            this.ShiftRadio.Location = new System.Drawing.Point(18, 270);
            this.ShiftRadio.Name = "ShiftRadio";
            this.ShiftRadio.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Day"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Afternoon"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Night")});
            this.ShiftRadio.Size = new System.Drawing.Size(97, 174);
            this.ShiftRadio.TabIndex = 11;
            // 
            // LocoBox
            // 
            this.LocoBox.FormattingEnabled = true;
            this.LocoBox.ItemHeight = 17;
            this.LocoBox.Location = new System.Drawing.Point(142, 270);
            this.LocoBox.Name = "LocoBox";
            this.LocoBox.Size = new System.Drawing.Size(216, 174);
            this.LocoBox.TabIndex = 12;
            // 
            // CrewBox
            // 
            this.CrewBox.FormattingEnabled = true;
            this.CrewBox.ItemHeight = 17;
            this.CrewBox.Location = new System.Drawing.Point(388, 270);
            this.CrewBox.Name = "CrewBox";
            this.CrewBox.Size = new System.Drawing.Size(216, 174);
            this.CrewBox.TabIndex = 13;
            // 
            // HFactcmb
            // 
            this.HFactcmb.FormattingEnabled = true;
            this.HFactcmb.Location = new System.Drawing.Point(659, 348);
            this.HFactcmb.Margin = new System.Windows.Forms.Padding(4);
            this.HFactcmb.Name = "HFactcmb";
            this.HFactcmb.Size = new System.Drawing.Size(159, 25);
            this.HFactcmb.TabIndex = 74;
            this.HFactcmb.SelectedIndexChanged += new System.EventHandler(this.HFactcmb_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(655, 324);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 19);
            this.label5.TabIndex = 73;
            this.label5.Text = "Hopper Factor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(654, 259);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 19);
            this.label1.TabIndex = 75;
            this.label1.Text = "Hoppers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(656, 390);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 19);
            this.label2.TabIndex = 76;
            this.label2.Text = "Tons";
            // 
            // HoppersEdit
            // 
            this.HoppersEdit.EditValue = "0";
            this.HoppersEdit.Location = new System.Drawing.Point(657, 283);
            this.HoppersEdit.Name = "HoppersEdit";
            this.HoppersEdit.Properties.Mask.EditMask = "n0";
            this.HoppersEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.HoppersEdit.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.HoppersEdit.Size = new System.Drawing.Size(77, 24);
            this.HoppersEdit.TabIndex = 77;
            this.HoppersEdit.EditValueChanged += new System.EventHandler(this.HoppersEdit_EditValueChanged);
            // 
            // TonsEdit
            // 
            this.TonsEdit.EditValue = "0";
            this.TonsEdit.Location = new System.Drawing.Point(659, 413);
            this.TonsEdit.Name = "TonsEdit";
            this.TonsEdit.Properties.Mask.EditMask = "n0";
            this.TonsEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.TonsEdit.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.TonsEdit.Size = new System.Drawing.Size(75, 24);
            this.TonsEdit.TabIndex = 78;
            // 
            // DestRadio
            // 
            this.DestRadio.Location = new System.Drawing.Point(930, 259);
            this.DestRadio.Name = "DestRadio";
            this.DestRadio.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Shaft")});
            this.DestRadio.Size = new System.Drawing.Size(171, 170);
            this.DestRadio.TabIndex = 79;
            // 
            // btnAddBooking
            // 
            this.btnAddBooking.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnAddBooking.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnAddBooking.Appearance.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAddBooking.Appearance.Options.UseBackColor = true;
            this.btnAddBooking.Appearance.Options.UseFont = true;
            this.btnAddBooking.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddBooking.ImageOptions.Image")));
            this.btnAddBooking.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.AddBlue2;
            this.btnAddBooking.ImageOptions.SvgImageSize = new System.Drawing.Size(18, 18);
            this.btnAddBooking.Location = new System.Drawing.Point(785, 407);
            this.btnAddBooking.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnAddBooking.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnAddBooking.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddBooking.Name = "btnAddBooking";
            this.btnAddBooking.Size = new System.Drawing.Size(95, 29);
            this.btnAddBooking.TabIndex = 168;
            this.btnAddBooking.Text = "        &Add            ";
            this.btnAddBooking.Click += new System.EventHandler(this.btnAddBooking_Click);
            // 
            // btnDestAdd
            // 
            this.btnDestAdd.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnDestAdd.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnDestAdd.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnDestAdd.Appearance.Options.UseBackColor = true;
            this.btnDestAdd.Appearance.Options.UseFont = true;
            this.btnDestAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDestAdd.ImageOptions.Image")));
            this.btnDestAdd.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.AddBlue2;
            this.btnDestAdd.ImageOptions.SvgImageSize = new System.Drawing.Size(18, 18);
            this.btnDestAdd.Location = new System.Drawing.Point(1114, 297);
            this.btnDestAdd.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnDestAdd.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnDestAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnDestAdd.Name = "btnDestAdd";
            this.btnDestAdd.Size = new System.Drawing.Size(31, 41);
            this.btnDestAdd.TabIndex = 169;
            this.btnDestAdd.Click += new System.EventHandler(this.btnDestAdd_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtProbNote);
            this.panel1.Controls.Add(this.btnAddProb);
            this.panel1.Controls.Add(this.spinProb);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.cmbProb);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.gcTramProb);
            this.panel1.Controls.Add(this.ShiftProbRadio);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 499);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1180, 273);
            this.panel1.TabIndex = 170;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelControl2.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelControl2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.InformationBlue;
            this.labelControl2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.labelControl2.Location = new System.Drawing.Point(12, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(208, 24);
            this.labelControl2.TabIndex = 190;
            this.labelControl2.Text = "Double Click to delete booking.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label10.Location = new System.Drawing.Point(921, 8);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 19);
            this.label10.TabIndex = 182;
            this.label10.Text = "Notes";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // txtProbNote
            // 
            this.txtProbNote.Location = new System.Drawing.Point(925, 43);
            this.txtProbNote.Multiline = true;
            this.txtProbNote.Name = "txtProbNote";
            this.txtProbNote.Size = new System.Drawing.Size(242, 210);
            this.txtProbNote.TabIndex = 181;
            this.txtProbNote.TextChanged += new System.EventHandler(this.txtProbNote_TextChanged);
            // 
            // btnAddProb
            // 
            this.btnAddProb.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddProb.Appearance.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.btnAddProb.Appearance.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAddProb.Appearance.Options.UseBackColor = true;
            this.btnAddProb.Appearance.Options.UseFont = true;
            this.btnAddProb.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddProb.ImageOptions.Image")));
            this.btnAddProb.Location = new System.Drawing.Point(817, 184);
            this.btnAddProb.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnAddProb.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnAddProb.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddProb.Name = "btnAddProb";
            this.btnAddProb.Size = new System.Drawing.Size(95, 29);
            this.btnAddProb.TabIndex = 180;
            this.btnAddProb.Text = "        &Add            ";
            this.btnAddProb.Click += new System.EventHandler(this.btnAddProb_Click);
            // 
            // spinProb
            // 
            this.spinProb.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinProb.Location = new System.Drawing.Point(713, 181);
            this.spinProb.Name = "spinProb";
            this.spinProb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinProb.Size = new System.Drawing.Size(84, 24);
            this.spinProb.TabIndex = 179;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(710, 157);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 19);
            this.label9.TabIndex = 178;
            this.label9.Text = "Hours";
            // 
            // cmbProb
            // 
            this.cmbProb.FormattingEnabled = true;
            this.cmbProb.Location = new System.Drawing.Point(714, 98);
            this.cmbProb.Margin = new System.Windows.Forms.Padding(4);
            this.cmbProb.Name = "cmbProb";
            this.cmbProb.Size = new System.Drawing.Size(198, 25);
            this.cmbProb.TabIndex = 177;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(710, 71);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 19);
            this.label8.TabIndex = 176;
            this.label8.Text = "Problem";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(594, 63);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 19);
            this.label7.TabIndex = 174;
            this.label7.Text = "Shift";
            // 
            // gcTramProb
            // 
            this.gcTramProb.Location = new System.Drawing.Point(12, 43);
            this.gcTramProb.MainView = this.gvTramProb;
            this.gcTramProb.Name = "gcTramProb";
            this.gcTramProb.Size = new System.Drawing.Size(568, 210);
            this.gcTramProb.TabIndex = 13;
            this.gcTramProb.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTramProb});
            // 
            // gvTramProb
            // 
            this.gvTramProb.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand2});
            this.gvTramProb.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colProbShift,
            this.colProbDescription,
            this.colProbHours,
            this.colProbNotes});
            this.gvTramProb.DetailHeight = 458;
            this.gvTramProb.GridControl = this.gcTramProb;
            this.gvTramProb.Name = "gvTramProb";
            this.gvTramProb.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvTramProb.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvTramProb.OptionsBehavior.Editable = false;
            this.gvTramProb.OptionsBehavior.ReadOnly = true;
            this.gvTramProb.OptionsView.ShowBands = false;
            this.gvTramProb.OptionsView.ShowGroupPanel = false;
            this.gvTramProb.OptionsView.ShowIndicator = false;
            this.gvTramProb.DoubleClick += new System.EventHandler(this.gvTramProb_DoubleClick);
            // 
            // gridBand2
            // 
            this.gridBand2.Caption = " ";
            this.gridBand2.Columns.Add(this.colProbShift);
            this.gridBand2.Columns.Add(this.colProbDescription);
            this.gridBand2.Columns.Add(this.colProbHours);
            this.gridBand2.Columns.Add(this.colProbNotes);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 0;
            this.gridBand2.Width = 560;
            // 
            // colProbShift
            // 
            this.colProbShift.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colProbShift.AppearanceHeader.Options.UseForeColor = true;
            this.colProbShift.Caption = "Shift";
            this.colProbShift.MinWidth = 25;
            this.colProbShift.Name = "colProbShift";
            this.colProbShift.OptionsColumn.AllowEdit = false;
            this.colProbShift.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colProbShift.OptionsColumn.AllowMove = false;
            this.colProbShift.OptionsColumn.AllowSize = false;
            this.colProbShift.OptionsColumn.FixedWidth = true;
            this.colProbShift.OptionsFilter.AllowAutoFilter = false;
            this.colProbShift.OptionsFilter.AllowFilter = false;
            this.colProbShift.Visible = true;
            this.colProbShift.Width = 120;
            // 
            // colProbDescription
            // 
            this.colProbDescription.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colProbDescription.AppearanceHeader.Options.UseForeColor = true;
            this.colProbDescription.Caption = "Description";
            this.colProbDescription.MinWidth = 25;
            this.colProbDescription.Name = "colProbDescription";
            this.colProbDescription.OptionsColumn.AllowEdit = false;
            this.colProbDescription.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colProbDescription.OptionsColumn.AllowMove = false;
            this.colProbDescription.OptionsColumn.AllowSize = false;
            this.colProbDescription.OptionsColumn.FixedWidth = true;
            this.colProbDescription.OptionsFilter.AllowAutoFilter = false;
            this.colProbDescription.OptionsFilter.AllowFilter = false;
            this.colProbDescription.Visible = true;
            this.colProbDescription.Width = 200;
            // 
            // colProbHours
            // 
            this.colProbHours.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colProbHours.AppearanceHeader.Options.UseForeColor = true;
            this.colProbHours.AppearanceHeader.Options.UseTextOptions = true;
            this.colProbHours.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProbHours.Caption = "Hours";
            this.colProbHours.MinWidth = 25;
            this.colProbHours.Name = "colProbHours";
            this.colProbHours.OptionsColumn.AllowEdit = false;
            this.colProbHours.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colProbHours.OptionsColumn.AllowMove = false;
            this.colProbHours.OptionsColumn.AllowSize = false;
            this.colProbHours.OptionsColumn.FixedWidth = true;
            this.colProbHours.OptionsFilter.AllowAutoFilter = false;
            this.colProbHours.OptionsFilter.AllowFilter = false;
            this.colProbHours.Visible = true;
            this.colProbHours.Width = 107;
            // 
            // colProbNotes
            // 
            this.colProbNotes.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colProbNotes.AppearanceHeader.Options.UseForeColor = true;
            this.colProbNotes.AppearanceHeader.Options.UseTextOptions = true;
            this.colProbNotes.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProbNotes.Caption = "Notes";
            this.colProbNotes.MinWidth = 25;
            this.colProbNotes.Name = "colProbNotes";
            this.colProbNotes.OptionsColumn.AllowEdit = false;
            this.colProbNotes.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colProbNotes.OptionsColumn.AllowMove = false;
            this.colProbNotes.OptionsColumn.AllowSize = false;
            this.colProbNotes.OptionsColumn.FixedWidth = true;
            this.colProbNotes.OptionsFilter.AllowAutoFilter = false;
            this.colProbNotes.OptionsFilter.AllowFilter = false;
            this.colProbNotes.Visible = true;
            this.colProbNotes.Width = 133;
            // 
            // ShiftProbRadio
            // 
            this.ShiftProbRadio.Location = new System.Drawing.Point(593, 93);
            this.ShiftProbRadio.Name = "ShiftProbRadio";
            this.ShiftProbRadio.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Day"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Afternoon"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Night")});
            this.ShiftProbRadio.Size = new System.Drawing.Size(99, 129);
            this.ShiftProbRadio.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(385, 241);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 19);
            this.label3.TabIndex = 171;
            this.label3.Text = "Crew";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(139, 240);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 19);
            this.label4.TabIndex = 172;
            this.label4.Text = "Loco";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(15, 240);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 19);
            this.label6.TabIndex = 173;
            this.label6.Text = "Shift";
            // 
            // BHLBL
            // 
            this.BHLBL.AutoSize = true;
            this.BHLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.BHLBL.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BHLBL.Location = new System.Drawing.Point(101, 11);
            this.BHLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BHLBL.Name = "BHLBL";
            this.BHLBL.Size = new System.Drawing.Size(65, 20);
            this.BHLBL.TabIndex = 174;
            this.BHLBL.Text = "Boxhole";
            // 
            // lblDisp
            // 
            this.lblDisp.AutoSize = true;
            this.lblDisp.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblDisp.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDisp.Location = new System.Drawing.Point(15, 11);
            this.lblDisp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDisp.Name = "lblDisp";
            this.lblDisp.Size = new System.Drawing.Size(69, 20);
            this.lblDisp.TabIndex = 175;
            this.lblDisp.Text = "Boxhole:";
            // 
            // btnDestDel
            // 
            this.btnDestDel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnDestDel.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnDestDel.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnDestDel.Appearance.Options.UseBackColor = true;
            this.btnDestDel.Appearance.Options.UseFont = true;
            this.btnDestDel.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.DeleteRed;
            this.btnDestDel.ImageOptions.SvgImageSize = new System.Drawing.Size(18, 18);
            this.btnDestDel.Location = new System.Drawing.Point(1114, 347);
            this.btnDestDel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnDestDel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnDestDel.Margin = new System.Windows.Forms.Padding(4);
            this.btnDestDel.Name = "btnDestDel";
            this.btnDestDel.Size = new System.Drawing.Size(31, 41);
            this.btnDestDel.TabIndex = 176;
            this.btnDestDel.Click += new System.EventHandler(this.btnDestDel_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label12.Location = new System.Drawing.Point(525, 11);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 19);
            this.label12.TabIndex = 188;
            this.label12.Text = "Date";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelControl1.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelControl1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.InformationBlue;
            this.labelControl1.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.labelControl1.Location = new System.Drawing.Point(799, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(208, 24);
            this.labelControl1.TabIndex = 189;
            this.labelControl1.Text = "Double Click to delete booking.";
            // 
            // frmTramBooking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 772);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnDestDel);
            this.Controls.Add(this.lblDisp);
            this.Controls.Add(this.BHLBL);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnDestAdd);
            this.Controls.Add(this.btnAddBooking);
            this.Controls.Add(this.DestRadio);
            this.Controls.Add(this.TonsEdit);
            this.Controls.Add(this.HoppersEdit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HFactcmb);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CrewBox);
            this.Controls.Add(this.LocoBox);
            this.Controls.Add(this.ShiftRadio);
            this.Controls.Add(this.TramDateProp);
            this.Controls.Add(this.gcTramBooking);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmTramBooking";
            this.Opacity = 0.98D;
            this.Text = "Tramming Bookings";
            this.Load += new System.EventHandler(this.frmTramBooking_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcTramBooking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTramBooking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftRadio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HoppersEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TonsEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DestRadio.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinProb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTramProb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTramProb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftProbRadio.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcTramBooking;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvTramBooking;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colShift;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDestination;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colHoppers;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colHopperFactor;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTons;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLoco;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCrew;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDate;
        private DevExpress.XtraEditors.RadioGroup ShiftRadio;
        private System.Windows.Forms.ListBox LocoBox;
        private System.Windows.Forms.ListBox CrewBox;
        private System.Windows.Forms.ComboBox HFactcmb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit HoppersEdit;
        private DevExpress.XtraEditors.TextEdit TonsEdit;
        private DevExpress.XtraEditors.RadioGroup DestRadio;
        private DevExpress.XtraEditors.SimpleButton btnAddBooking;
        private DevExpress.XtraEditors.SimpleButton btnDestAdd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private System.Windows.Forms.Label lblDisp;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtProbNote;
        private DevExpress.XtraEditors.SimpleButton btnAddProb;
        private DevExpress.XtraEditors.SpinEdit spinProb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbProb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraGrid.GridControl gcTramProb;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvTramProb;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProbShift;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProbDescription;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProbHours;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProbNotes;
        private DevExpress.XtraEditors.RadioGroup ShiftProbRadio;
        public System.Windows.Forms.Label BHLBL;
        public System.Windows.Forms.DateTimePicker TramDateProp;
        private DevExpress.XtraEditors.SimpleButton btnDestDel;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}