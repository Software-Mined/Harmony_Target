namespace Mineware.Systems.Production.OreflowDiagram
{
    partial class FrmBackfill_Book_Surface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBackfill_Book_Surface));
            this.pnlBookings = new System.Windows.Forms.Panel();
            this.dtBookSurface = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTank = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTonnage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRetention = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Leveltxt = new DevExpress.XtraEditors.TextEdit();
            this.Tonnagetxt = new DevExpress.XtraEditors.TextEdit();
            this.RDtxt = new DevExpress.XtraEditors.TextEdit();
            this.Retentiontxt = new DevExpress.XtraEditors.TextEdit();
            this.BookTimePicker = new System.Windows.Forms.DateTimePicker();
            this.DPdate = new System.Windows.Forms.DateTimePicker();
            this.Tanktxt = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.addSurfBookingbtn = new DevExpress.XtraEditors.SimpleButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlProblem = new System.Windows.Forms.Panel();
            this.cmbProblems = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ProblemStartTime = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.ProblemStopTime = new System.Windows.Forms.DateTimePicker();
            this.AddProbbtn = new DevExpress.XtraEditors.SimpleButton();
            this.ProblemsGrid = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProbID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProbDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProbFrom = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProbTo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProbNote = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.pnlBookings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtBookSurface)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Leveltxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tonnagetxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RDtxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Retentiontxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tanktxt.Properties)).BeginInit();
            this.pnlProblem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProblemsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBookings
            // 
            this.pnlBookings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBookings.Controls.Add(this.labelControl1);
            this.pnlBookings.Controls.Add(this.dtBookSurface);
            this.pnlBookings.Controls.Add(this.Leveltxt);
            this.pnlBookings.Controls.Add(this.Tonnagetxt);
            this.pnlBookings.Controls.Add(this.RDtxt);
            this.pnlBookings.Controls.Add(this.Retentiontxt);
            this.pnlBookings.Controls.Add(this.BookTimePicker);
            this.pnlBookings.Controls.Add(this.DPdate);
            this.pnlBookings.Controls.Add(this.Tanktxt);
            this.pnlBookings.Controls.Add(this.label7);
            this.pnlBookings.Controls.Add(this.addSurfBookingbtn);
            this.pnlBookings.Controls.Add(this.label6);
            this.pnlBookings.Controls.Add(this.label5);
            this.pnlBookings.Controls.Add(this.label4);
            this.pnlBookings.Controls.Add(this.label3);
            this.pnlBookings.Controls.Add(this.label2);
            this.pnlBookings.Controls.Add(this.label1);
            this.pnlBookings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBookings.Location = new System.Drawing.Point(0, 0);
            this.pnlBookings.Name = "pnlBookings";
            this.pnlBookings.Size = new System.Drawing.Size(654, 407);
            this.pnlBookings.TabIndex = 0;
            // 
            // dtBookSurface
            // 
            this.dtBookSurface.Location = new System.Drawing.Point(17, 31);
            this.dtBookSurface.MainView = this.gridView1;
            this.dtBookSurface.Name = "dtBookSurface";
            this.dtBookSurface.Size = new System.Drawing.Size(632, 180);
            this.dtBookSurface.TabIndex = 5;
            this.dtBookSurface.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTime,
            this.colTank,
            this.colLevel,
            this.colTonnage,
            this.colRD,
            this.colDate,
            this.colRetention});
            this.gridView1.DetailHeight = 458;
            this.gridView1.GridControl = this.dtBookSurface;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // colTime
            // 
            this.colTime.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colTime.AppearanceHeader.Options.UseForeColor = true;
            this.colTime.Caption = "Time";
            this.colTime.MinWidth = 25;
            this.colTime.Name = "colTime";
            this.colTime.OptionsColumn.AllowEdit = false;
            this.colTime.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colTime.OptionsColumn.AllowMove = false;
            this.colTime.OptionsColumn.AllowSize = false;
            this.colTime.OptionsColumn.FixedWidth = true;
            this.colTime.Visible = true;
            this.colTime.VisibleIndex = 0;
            this.colTime.Width = 100;
            // 
            // colTank
            // 
            this.colTank.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colTank.AppearanceHeader.Options.UseForeColor = true;
            this.colTank.Caption = "Tanks";
            this.colTank.MinWidth = 25;
            this.colTank.Name = "colTank";
            this.colTank.OptionsColumn.AllowEdit = false;
            this.colTank.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colTank.OptionsColumn.AllowMove = false;
            this.colTank.OptionsColumn.AllowSize = false;
            this.colTank.OptionsColumn.FixedWidth = true;
            this.colTank.Visible = true;
            this.colTank.VisibleIndex = 1;
            this.colTank.Width = 218;
            // 
            // colLevel
            // 
            this.colLevel.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colLevel.AppearanceHeader.Options.UseForeColor = true;
            this.colLevel.Caption = "Level";
            this.colLevel.MinWidth = 25;
            this.colLevel.Name = "colLevel";
            this.colLevel.OptionsColumn.AllowEdit = false;
            this.colLevel.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colLevel.OptionsColumn.AllowMove = false;
            this.colLevel.OptionsColumn.AllowSize = false;
            this.colLevel.OptionsColumn.FixedWidth = true;
            this.colLevel.Visible = true;
            this.colLevel.VisibleIndex = 2;
            this.colLevel.Width = 67;
            // 
            // colTonnage
            // 
            this.colTonnage.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colTonnage.AppearanceHeader.Options.UseForeColor = true;
            this.colTonnage.Caption = "Tonnage";
            this.colTonnage.MinWidth = 25;
            this.colTonnage.Name = "colTonnage";
            this.colTonnage.OptionsColumn.AllowEdit = false;
            this.colTonnage.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colTonnage.OptionsColumn.AllowMove = false;
            this.colTonnage.OptionsColumn.AllowSize = false;
            this.colTonnage.OptionsColumn.FixedWidth = true;
            this.colTonnage.Visible = true;
            this.colTonnage.VisibleIndex = 3;
            this.colTonnage.Width = 80;
            // 
            // colRD
            // 
            this.colRD.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colRD.AppearanceHeader.Options.UseForeColor = true;
            this.colRD.Caption = "RD";
            this.colRD.MinWidth = 25;
            this.colRD.Name = "colRD";
            this.colRD.OptionsColumn.AllowEdit = false;
            this.colRD.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colRD.OptionsColumn.AllowMove = false;
            this.colRD.OptionsColumn.AllowSize = false;
            this.colRD.OptionsColumn.FixedWidth = true;
            this.colRD.Visible = true;
            this.colRD.VisibleIndex = 4;
            this.colRD.Width = 80;
            // 
            // colDate
            // 
            this.colDate.Caption = "Date";
            this.colDate.MinWidth = 25;
            this.colDate.Name = "colDate";
            this.colDate.OptionsColumn.AllowEdit = false;
            this.colDate.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDate.OptionsColumn.AllowMove = false;
            this.colDate.OptionsColumn.AllowSize = false;
            this.colDate.OptionsColumn.FixedWidth = true;
            this.colDate.Width = 80;
            // 
            // colRetention
            // 
            this.colRetention.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colRetention.AppearanceHeader.Options.UseForeColor = true;
            this.colRetention.Caption = "Retention";
            this.colRetention.MinWidth = 25;
            this.colRetention.Name = "colRetention";
            this.colRetention.OptionsColumn.AllowEdit = false;
            this.colRetention.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colRetention.OptionsColumn.AllowMove = false;
            this.colRetention.OptionsColumn.AllowSize = false;
            this.colRetention.OptionsColumn.FixedWidth = true;
            this.colRetention.Visible = true;
            this.colRetention.VisibleIndex = 5;
            this.colRetention.Width = 80;
            // 
            // Leveltxt
            // 
            this.Leveltxt.EditValue = "0.00";
            this.Leveltxt.Location = new System.Drawing.Point(482, 238);
            this.Leveltxt.Name = "Leveltxt";
            this.Leveltxt.Properties.Mask.EditMask = "n2";
            this.Leveltxt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.Leveltxt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.Leveltxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Leveltxt.Size = new System.Drawing.Size(144, 24);
            this.Leveltxt.TabIndex = 8;
            // 
            // Tonnagetxt
            // 
            this.Tonnagetxt.EditValue = "0.00";
            this.Tonnagetxt.Location = new System.Drawing.Point(482, 285);
            this.Tonnagetxt.Name = "Tonnagetxt";
            this.Tonnagetxt.Properties.Mask.EditMask = "n2";
            this.Tonnagetxt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.Tonnagetxt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.Tonnagetxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Tonnagetxt.Size = new System.Drawing.Size(144, 24);
            this.Tonnagetxt.TabIndex = 12;
            // 
            // RDtxt
            // 
            this.RDtxt.EditValue = "0.00";
            this.RDtxt.Location = new System.Drawing.Point(482, 330);
            this.RDtxt.Name = "RDtxt";
            this.RDtxt.Properties.Mask.EditMask = "n2";
            this.RDtxt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.RDtxt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.RDtxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RDtxt.Size = new System.Drawing.Size(144, 24);
            this.RDtxt.TabIndex = 14;
            // 
            // Retentiontxt
            // 
            this.Retentiontxt.EditValue = "0.00";
            this.Retentiontxt.Location = new System.Drawing.Point(482, 376);
            this.Retentiontxt.Name = "Retentiontxt";
            this.Retentiontxt.Properties.Mask.EditMask = "n2";
            this.Retentiontxt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.Retentiontxt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.Retentiontxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Retentiontxt.Size = new System.Drawing.Size(144, 24);
            this.Retentiontxt.TabIndex = 16;
            // 
            // BookTimePicker
            // 
            this.BookTimePicker.CustomFormat = "HH:mm";
            this.BookTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.BookTimePicker.Location = new System.Drawing.Point(284, 258);
            this.BookTimePicker.Name = "BookTimePicker";
            this.BookTimePicker.ShowUpDown = true;
            this.BookTimePicker.Size = new System.Drawing.Size(71, 25);
            this.BookTimePicker.TabIndex = 7;
            // 
            // DPdate
            // 
            this.DPdate.ImeMode = System.Windows.Forms.ImeMode.On;
            this.DPdate.Location = new System.Drawing.Point(25, 258);
            this.DPdate.Name = "DPdate";
            this.DPdate.Size = new System.Drawing.Size(211, 25);
            this.DPdate.TabIndex = 6;
            this.DPdate.ValueChanged += new System.EventHandler(this.DPdate_ValueChanged);
            // 
            // Tanktxt
            // 
            this.Tanktxt.Location = new System.Drawing.Point(24, 312);
            this.Tanktxt.Name = "Tanktxt";
            this.Tanktxt.Properties.ReadOnly = true;
            this.Tanktxt.Size = new System.Drawing.Size(125, 24);
            this.Tanktxt.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(21, 290);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 19);
            this.label7.TabIndex = 20;
            this.label7.Text = "Tank";
            // 
            // addSurfBookingbtn
            // 
            this.addSurfBookingbtn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.addSurfBookingbtn.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.addSurfBookingbtn.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(1)), true);
            this.addSurfBookingbtn.Appearance.Options.UseBackColor = true;
            this.addSurfBookingbtn.Appearance.Options.UseFont = true;
            this.addSurfBookingbtn.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.AddBlue2;
            this.addSurfBookingbtn.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.addSurfBookingbtn.Location = new System.Drawing.Point(36, 350);
            this.addSurfBookingbtn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.addSurfBookingbtn.LookAndFeel.UseDefaultLookAndFeel = false;
            this.addSurfBookingbtn.Margin = new System.Windows.Forms.Padding(4);
            this.addSurfBookingbtn.Name = "addSurfBookingbtn";
            this.addSurfBookingbtn.Size = new System.Drawing.Size(100, 30);
            this.addSurfBookingbtn.TabIndex = 18;
            this.addSurfBookingbtn.Text = "        &Add            ";
            this.addSurfBookingbtn.Click += new System.EventHandler(this.addSurfBookingbtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(479, 356);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 19);
            this.label6.TabIndex = 17;
            this.label6.Text = "Retention";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(479, 309);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 19);
            this.label5.TabIndex = 15;
            this.label5.Text = "RD";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(479, 264);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 19);
            this.label4.TabIndex = 13;
            this.label4.Text = "Tonnage";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(479, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 19);
            this.label3.TabIndex = 11;
            this.label3.Text = "Level";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(280, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 19);
            this.label2.TabIndex = 10;
            this.label2.Text = "Time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(21, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 19);
            this.label1.TabIndex = 9;
            this.label1.Text = "Date";
            // 
            // pnlProblem
            // 
            this.pnlProblem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProblem.Controls.Add(this.cmbProblems);
            this.pnlProblem.Controls.Add(this.label10);
            this.pnlProblem.Controls.Add(this.label9);
            this.pnlProblem.Controls.Add(this.ProblemStartTime);
            this.pnlProblem.Controls.Add(this.label8);
            this.pnlProblem.Controls.Add(this.ProblemStopTime);
            this.pnlProblem.Controls.Add(this.AddProbbtn);
            this.pnlProblem.Controls.Add(this.ProblemsGrid);
            this.pnlProblem.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProblem.Location = new System.Drawing.Point(0, 407);
            this.pnlProblem.Name = "pnlProblem";
            this.pnlProblem.Size = new System.Drawing.Size(654, 366);
            this.pnlProblem.TabIndex = 1;
            // 
            // cmbProblems
            // 
            this.cmbProblems.BackColor = System.Drawing.Color.White;
            this.cmbProblems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProblems.FormattingEnabled = true;
            this.cmbProblems.Location = new System.Drawing.Point(24, 38);
            this.cmbProblems.Name = "cmbProblems";
            this.cmbProblems.Size = new System.Drawing.Size(182, 25);
            this.cmbProblems.TabIndex = 26;
            this.cmbProblems.SelectedIndexChanged += new System.EventHandler(this.cmbProblems_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label10.Location = new System.Drawing.Point(21, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 19);
            this.label10.TabIndex = 21;
            this.label10.Text = "Problems";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(227, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 19);
            this.label9.TabIndex = 25;
            this.label9.Text = "From";
            // 
            // ProblemStartTime
            // 
            this.ProblemStartTime.CustomFormat = "HH:mm";
            this.ProblemStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ProblemStartTime.Location = new System.Drawing.Point(274, 36);
            this.ProblemStartTime.Name = "ProblemStartTime";
            this.ProblemStartTime.ShowUpDown = true;
            this.ProblemStartTime.Size = new System.Drawing.Size(67, 25);
            this.ProblemStartTime.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(372, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 19);
            this.label8.TabIndex = 23;
            this.label8.Text = "To";
            // 
            // ProblemStopTime
            // 
            this.ProblemStopTime.CustomFormat = "HH:mm";
            this.ProblemStopTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ProblemStopTime.Location = new System.Drawing.Point(411, 35);
            this.ProblemStopTime.Name = "ProblemStopTime";
            this.ProblemStopTime.ShowUpDown = true;
            this.ProblemStopTime.Size = new System.Drawing.Size(69, 25);
            this.ProblemStopTime.TabIndex = 22;
            // 
            // AddProbbtn
            // 
            this.AddProbbtn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.AddProbbtn.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.AddProbbtn.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(1)), true);
            this.AddProbbtn.Appearance.Options.UseBackColor = true;
            this.AddProbbtn.Appearance.Options.UseFont = true;
            this.AddProbbtn.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.AddBlue2;
            this.AddProbbtn.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.AddProbbtn.Location = new System.Drawing.Point(520, 33);
            this.AddProbbtn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.AddProbbtn.LookAndFeel.UseDefaultLookAndFeel = false;
            this.AddProbbtn.Margin = new System.Windows.Forms.Padding(4);
            this.AddProbbtn.Name = "AddProbbtn";
            this.AddProbbtn.Size = new System.Drawing.Size(100, 30);
            this.AddProbbtn.TabIndex = 21;
            this.AddProbbtn.Text = "        &Add            ";
            this.AddProbbtn.Click += new System.EventHandler(this.AddProbbtn_Click);
            // 
            // ProblemsGrid
            // 
            this.ProblemsGrid.Location = new System.Drawing.Point(12, 86);
            this.ProblemsGrid.MainView = this.gridView3;
            this.ProblemsGrid.Name = "ProblemsGrid";
            this.ProblemsGrid.Size = new System.Drawing.Size(632, 253);
            this.ProblemsGrid.TabIndex = 6;
            this.ProblemsGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProbID,
            this.colProbDescription,
            this.colProbFrom,
            this.colProbTo,
            this.colProbNote});
            this.gridView3.DetailHeight = 458;
            this.gridView3.GridControl = this.ProblemsGrid;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView3.OptionsBehavior.Editable = false;
            this.gridView3.OptionsBehavior.ReadOnly = true;
            this.gridView3.OptionsView.ColumnAutoWidth = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.OptionsView.ShowIndicator = false;
            this.gridView3.DoubleClick += new System.EventHandler(this.gridView3_DoubleClick);
            // 
            // colProbID
            // 
            this.colProbID.Caption = "Problem ID";
            this.colProbID.MinWidth = 25;
            this.colProbID.Name = "colProbID";
            this.colProbID.OptionsColumn.AllowEdit = false;
            this.colProbID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colProbID.OptionsColumn.AllowMove = false;
            this.colProbID.OptionsColumn.AllowSize = false;
            this.colProbID.OptionsColumn.FixedWidth = true;
            this.colProbID.Width = 100;
            // 
            // colProbDescription
            // 
            this.colProbDescription.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colProbDescription.AppearanceHeader.Options.UseForeColor = true;
            this.colProbDescription.Caption = "Problem Description";
            this.colProbDescription.MinWidth = 25;
            this.colProbDescription.Name = "colProbDescription";
            this.colProbDescription.OptionsColumn.AllowEdit = false;
            this.colProbDescription.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colProbDescription.OptionsColumn.AllowMove = false;
            this.colProbDescription.OptionsColumn.AllowSize = false;
            this.colProbDescription.OptionsColumn.FixedWidth = true;
            this.colProbDescription.Visible = true;
            this.colProbDescription.VisibleIndex = 0;
            this.colProbDescription.Width = 200;
            // 
            // colProbFrom
            // 
            this.colProbFrom.AppearanceCell.Options.UseTextOptions = true;
            this.colProbFrom.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colProbFrom.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colProbFrom.AppearanceHeader.Options.UseForeColor = true;
            this.colProbFrom.Caption = "Time From ";
            this.colProbFrom.MinWidth = 25;
            this.colProbFrom.Name = "colProbFrom";
            this.colProbFrom.OptionsColumn.AllowEdit = false;
            this.colProbFrom.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colProbFrom.OptionsColumn.AllowMove = false;
            this.colProbFrom.OptionsColumn.AllowSize = false;
            this.colProbFrom.OptionsColumn.FixedWidth = true;
            this.colProbFrom.Visible = true;
            this.colProbFrom.VisibleIndex = 1;
            this.colProbFrom.Width = 67;
            // 
            // colProbTo
            // 
            this.colProbTo.AppearanceCell.Options.UseTextOptions = true;
            this.colProbTo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colProbTo.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colProbTo.AppearanceHeader.Options.UseForeColor = true;
            this.colProbTo.Caption = "Time To";
            this.colProbTo.MinWidth = 25;
            this.colProbTo.Name = "colProbTo";
            this.colProbTo.OptionsColumn.AllowEdit = false;
            this.colProbTo.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colProbTo.OptionsColumn.AllowMove = false;
            this.colProbTo.OptionsColumn.AllowSize = false;
            this.colProbTo.OptionsColumn.FixedWidth = true;
            this.colProbTo.Visible = true;
            this.colProbTo.VisibleIndex = 2;
            this.colProbTo.Width = 80;
            // 
            // colProbNote
            // 
            this.colProbNote.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colProbNote.AppearanceHeader.Options.UseForeColor = true;
            this.colProbNote.Caption = "Notes";
            this.colProbNote.MinWidth = 25;
            this.colProbNote.Name = "colProbNote";
            this.colProbNote.OptionsColumn.AllowEdit = false;
            this.colProbNote.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colProbNote.OptionsColumn.AllowMove = false;
            this.colProbNote.OptionsColumn.AllowSize = false;
            this.colProbNote.OptionsColumn.FixedWidth = true;
            this.colProbNote.Visible = true;
            this.colProbNote.VisibleIndex = 3;
            this.colProbNote.Width = 280;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Location = new System.Drawing.Point(167, 778);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(301, 35);
            this.pnlButtons.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCancel.ImageOptions.SvgImage")));
            this.btnCancel.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnCancel.Location = new System.Drawing.Point(152, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 27);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSave.ImageOptions.SvgImage")));
            this.btnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSave.Location = new System.Drawing.Point(31, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 27);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelControl1.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelControl1.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.InformationBlue;
            this.labelControl1.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.labelControl1.Location = new System.Drawing.Point(12, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(229, 24);
            this.labelControl1.TabIndex = 173;
            this.labelControl1.Text = "Double Click on a Record to delete";
            // 
            // FrmBackfill_Book_Surface
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 819);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlProblem);
            this.Controls.Add(this.pnlBookings);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "FrmBackfill_Book_Surface";
            this.Text = "Book Surface";
            this.Load += new System.EventHandler(this.FrmBackfill_Book_Surface_Load);
            this.pnlBookings.ResumeLayout(false);
            this.pnlBookings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtBookSurface)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Leveltxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tonnagetxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RDtxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Retentiontxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tanktxt.Properties)).EndInit();
            this.pnlProblem.ResumeLayout(false);
            this.pnlProblem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProblemsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBookings;
        private System.Windows.Forms.Panel pnlProblem;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.SimpleButton addSurfBookingbtn;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit Retentiontxt;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit RDtxt;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit Tonnagetxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit Leveltxt;
        private System.Windows.Forms.DateTimePicker BookTimePicker;
        private System.Windows.Forms.DateTimePicker DPdate;
        private DevExpress.XtraGrid.GridControl dtBookSurface;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colTime;
        private DevExpress.XtraGrid.Columns.GridColumn colTank;
        private DevExpress.XtraGrid.Columns.GridColumn colLevel;
        private DevExpress.XtraGrid.Columns.GridColumn colTonnage;
        private DevExpress.XtraGrid.Columns.GridColumn colRD;
        private DevExpress.XtraGrid.Columns.GridColumn colDate;
        private DevExpress.XtraGrid.Columns.GridColumn colRetention;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbProblems;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker ProblemStartTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker ProblemStopTime;
        private DevExpress.XtraEditors.SimpleButton AddProbbtn;
        private DevExpress.XtraGrid.GridControl ProblemsGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.Columns.GridColumn colProbID;
        private DevExpress.XtraGrid.Columns.GridColumn colProbDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colProbFrom;
        private DevExpress.XtraGrid.Columns.GridColumn colProbTo;
        private DevExpress.XtraGrid.Columns.GridColumn colProbNote;
        private System.Windows.Forms.Panel pnlButtons;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        public DevExpress.XtraEditors.TextEdit Tanktxt;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}