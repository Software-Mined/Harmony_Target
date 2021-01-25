
namespace Mineware.Systems.Production.Reporting.WorkplaceSummary
{
    partial class ucGrid
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleIconSet formatConditionRuleIconSet1 = new DevExpress.XtraEditors.FormatConditionRuleIconSet();
            DevExpress.XtraEditors.FormatConditionIconSet formatConditionIconSet1 = new DevExpress.XtraEditors.FormatConditionIconSet();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon1 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon2 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            DevExpress.XtraEditors.FormatConditionIconSetIcon formatConditionIconSetIcon3 = new DevExpress.XtraEditors.FormatConditionIconSetIcon();
            this.gcGrid = new DevExpress.XtraGrid.GridControl();
            this.gvGrid = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gbHeader = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colProdmonth = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colSection = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCrew = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colWorkplaceName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colActivity = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDateCaptured = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colRiskRating = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandAct = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand15 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandDep = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.blankBandfinal = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colWorkplaceID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colMinerSection = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colMOSection = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.AuthCheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.ImageEdit = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gcGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuthCheckEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // gcGrid
            // 
            this.gcGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcGrid.Location = new System.Drawing.Point(0, 0);
            this.gcGrid.MainView = this.gvGrid;
            this.gcGrid.Name = "gcGrid";
            this.gcGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.AuthCheckEdit,
            this.ImageEdit});
            this.gcGrid.Size = new System.Drawing.Size(1007, 636);
            this.gcGrid.TabIndex = 3;
            this.gcGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvGrid});
            // 
            // gvGrid
            // 
            this.gvGrid.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gbHeader,
            this.bandAct,
            this.gridBand15,
            this.bandDep,
            this.blankBandfinal});
            this.gvGrid.ColumnPanelRowHeight = 100;
            this.gvGrid.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colProdmonth,
            this.colSection,
            this.colCrew,
            this.colWorkplaceName,
            this.colDateCaptured,
            this.colWorkplaceID,
            this.colActivity,
            this.colRiskRating,
            this.colMinerSection,
            this.colMOSection});
            gridFormatRule1.Name = "Format0";
            formatConditionIconSet1.CategoryName = "Symbols";
            formatConditionIconSetIcon1.PredefinedName = "Flags3_1.png";
            formatConditionIconSetIcon2.PredefinedName = "Flags3_2.png";
            formatConditionIconSetIcon2.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            formatConditionIconSetIcon2.ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSetIcon3.PredefinedName = "Flags3_3.png";
            formatConditionIconSetIcon3.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            formatConditionIconSetIcon3.ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon1);
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon2);
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon3);
            formatConditionIconSet1.Name = "Flags3";
            formatConditionIconSet1.ValueType = DevExpress.XtraEditors.FormatConditionValueType.Number;
            formatConditionRuleIconSet1.IconSet = formatConditionIconSet1;
            gridFormatRule1.Rule = formatConditionRuleIconSet1;
            this.gvGrid.FormatRules.Add(gridFormatRule1);
            this.gvGrid.GridControl = this.gcGrid;
            this.gvGrid.Name = "gvGrid";
            this.gvGrid.OptionsView.AllowCellMerge = true;
            this.gvGrid.OptionsView.ColumnAutoWidth = false;
            this.gvGrid.DoubleClick += new System.EventHandler(this.gvGrid_DoubleClick);
            // 
            // gbHeader
            // 
            this.gbHeader.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.gbHeader.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gbHeader.AppearanceHeader.Options.UseFont = true;
            this.gbHeader.AppearanceHeader.Options.UseForeColor = true;
            this.gbHeader.AppearanceHeader.Options.UseTextOptions = true;
            this.gbHeader.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gbHeader.Columns.Add(this.colProdmonth);
            this.gbHeader.Columns.Add(this.colSection);
            this.gbHeader.Columns.Add(this.colCrew);
            this.gbHeader.Columns.Add(this.colWorkplaceName);
            this.gbHeader.Columns.Add(this.colActivity);
            this.gbHeader.Columns.Add(this.colDateCaptured);
            this.gbHeader.Columns.Add(this.colRiskRating);
            this.gbHeader.MinWidth = 19;
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.VisibleIndex = 0;
            this.gbHeader.Width = 732;
            // 
            // colProdmonth
            // 
            this.colProdmonth.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 7.2F);
            this.colProdmonth.AppearanceCell.Options.UseFont = true;
            this.colProdmonth.AppearanceCell.Options.UseTextOptions = true;
            this.colProdmonth.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colProdmonth.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colProdmonth.AppearanceHeader.Options.UseForeColor = true;
            this.colProdmonth.AppearanceHeader.Options.UseTextOptions = true;
            this.colProdmonth.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProdmonth.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colProdmonth.Caption = "Production Month";
            this.colProdmonth.MinWidth = 25;
            this.colProdmonth.Name = "colProdmonth";
            this.colProdmonth.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
            this.colProdmonth.Visible = true;
            this.colProdmonth.Width = 80;
            // 
            // colSection
            // 
            this.colSection.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colSection.AppearanceCell.Options.UseFont = true;
            this.colSection.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colSection.AppearanceHeader.Options.UseFont = true;
            this.colSection.AppearanceHeader.Options.UseForeColor = true;
            this.colSection.AppearanceHeader.Options.UseTextOptions = true;
            this.colSection.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSection.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colSection.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colSection.Caption = "MO Section";
            this.colSection.FieldName = "ActType";
            this.colSection.MinWidth = 36;
            this.colSection.Name = "colSection";
            this.colSection.OptionsColumn.AllowEdit = false;
            this.colSection.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
            this.colSection.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.colSection.Visible = true;
            this.colSection.Width = 180;
            // 
            // colCrew
            // 
            this.colCrew.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colCrew.AppearanceCell.Options.UseFont = true;
            this.colCrew.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colCrew.AppearanceHeader.Options.UseFont = true;
            this.colCrew.AppearanceHeader.Options.UseForeColor = true;
            this.colCrew.AppearanceHeader.Options.UseTextOptions = true;
            this.colCrew.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCrew.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCrew.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colCrew.Caption = "Crew";
            this.colCrew.FieldName = "Crew";
            this.colCrew.MinWidth = 36;
            this.colCrew.Name = "colCrew";
            this.colCrew.OptionsColumn.AllowEdit = false;
            this.colCrew.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
            this.colCrew.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.colCrew.Visible = true;
            this.colCrew.Width = 120;
            // 
            // colWorkplaceName
            // 
            this.colWorkplaceName.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colWorkplaceName.AppearanceCell.Options.UseFont = true;
            this.colWorkplaceName.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colWorkplaceName.AppearanceHeader.Options.UseFont = true;
            this.colWorkplaceName.AppearanceHeader.Options.UseForeColor = true;
            this.colWorkplaceName.AppearanceHeader.Options.UseTextOptions = true;
            this.colWorkplaceName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colWorkplaceName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colWorkplaceName.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colWorkplaceName.Caption = "Workplace";
            this.colWorkplaceName.FieldName = "Workplace";
            this.colWorkplaceName.MinWidth = 36;
            this.colWorkplaceName.Name = "colWorkplaceName";
            this.colWorkplaceName.OptionsColumn.AllowEdit = false;
            this.colWorkplaceName.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
            this.colWorkplaceName.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colWorkplaceName.Visible = true;
            this.colWorkplaceName.Width = 180;
            // 
            // colActivity
            // 
            this.colActivity.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colActivity.AppearanceHeader.Options.UseForeColor = true;
            this.colActivity.AppearanceHeader.Options.UseTextOptions = true;
            this.colActivity.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colActivity.Caption = "Activity";
            this.colActivity.MinWidth = 25;
            this.colActivity.Name = "colActivity";
            this.colActivity.Visible = true;
            this.colActivity.Width = 50;
            // 
            // colDateCaptured
            // 
            this.colDateCaptured.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.2F);
            this.colDateCaptured.AppearanceCell.Options.UseFont = true;
            this.colDateCaptured.AppearanceCell.Options.UseTextOptions = true;
            this.colDateCaptured.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDateCaptured.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDateCaptured.AppearanceHeader.Options.UseForeColor = true;
            this.colDateCaptured.AppearanceHeader.Options.UseTextOptions = true;
            this.colDateCaptured.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDateCaptured.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colDateCaptured.Caption = "Date Captured";
            this.colDateCaptured.MinWidth = 25;
            this.colDateCaptured.Name = "colDateCaptured";
            this.colDateCaptured.OptionsColumn.AllowEdit = false;
            this.colDateCaptured.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colDateCaptured.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colDateCaptured.Visible = true;
            this.colDateCaptured.Width = 72;
            // 
            // colRiskRating
            // 
            this.colRiskRating.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 7.2F);
            this.colRiskRating.AppearanceCell.Options.UseFont = true;
            this.colRiskRating.AppearanceCell.Options.UseTextOptions = true;
            this.colRiskRating.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colRiskRating.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colRiskRating.AppearanceHeader.Options.UseForeColor = true;
            this.colRiskRating.AppearanceHeader.Options.UseTextOptions = true;
            this.colRiskRating.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colRiskRating.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colRiskRating.Caption = "Risk Rating";
            this.colRiskRating.MinWidth = 25;
            this.colRiskRating.Name = "colRiskRating";
            this.colRiskRating.OptionsColumn.AllowEdit = false;
            this.colRiskRating.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colRiskRating.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colRiskRating.Visible = true;
            this.colRiskRating.Width = 50;
            // 
            // bandAct
            // 
            this.bandAct.MinWidth = 19;
            this.bandAct.Name = "bandAct";
            this.bandAct.Visible = false;
            this.bandAct.VisibleIndex = -1;
            this.bandAct.Width = 94;
            // 
            // gridBand15
            // 
            this.gridBand15.Name = "gridBand15";
            this.gridBand15.Visible = false;
            this.gridBand15.VisibleIndex = -1;
            this.gridBand15.Width = 100;
            // 
            // bandDep
            // 
            this.bandDep.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold);
            this.bandDep.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bandDep.AppearanceHeader.Options.UseFont = true;
            this.bandDep.AppearanceHeader.Options.UseForeColor = true;
            this.bandDep.AppearanceHeader.Options.UseTextOptions = true;
            this.bandDep.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandDep.Caption = "Departments";
            this.bandDep.MinWidth = 19;
            this.bandDep.Name = "bandDep";
            this.bandDep.Visible = false;
            this.bandDep.VisibleIndex = -1;
            this.bandDep.Width = 23;
            // 
            // blankBandfinal
            // 
            this.blankBandfinal.MinWidth = 19;
            this.blankBandfinal.Name = "blankBandfinal";
            this.blankBandfinal.Visible = false;
            this.blankBandfinal.VisibleIndex = -1;
            this.blankBandfinal.Width = 925;
            // 
            // colWorkplaceID
            // 
            this.colWorkplaceID.Caption = "WorkplaceID";
            this.colWorkplaceID.MinWidth = 25;
            this.colWorkplaceID.Name = "colWorkplaceID";
            this.colWorkplaceID.Width = 94;
            // 
            // colMinerSection
            // 
            this.colMinerSection.Caption = "MinerSec";
            this.colMinerSection.MinWidth = 25;
            this.colMinerSection.Name = "colMinerSection";
            this.colMinerSection.Width = 94;
            // 
            // colMOSection
            // 
            this.colMOSection.Caption = "MOSection";
            this.colMOSection.MinWidth = 25;
            this.colMOSection.Name = "colMOSection";
            this.colMOSection.Width = 94;
            // 
            // AuthCheckEdit
            // 
            this.AuthCheckEdit.AutoHeight = false;
            this.AuthCheckEdit.Name = "AuthCheckEdit";
            this.AuthCheckEdit.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.AuthCheckEdit.ValueChecked = "1";
            this.AuthCheckEdit.ValueUnchecked = "0";
            // 
            // ImageEdit
            // 
            this.ImageEdit.AutoHeight = false;
            this.ImageEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ImageEdit.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", "Capt", 1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", "Auth", 0)});
            this.ImageEdit.Name = "ImageEdit";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // ucGrid
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcGrid);
            this.Name = "ucGrid";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(1007, 636);
            this.Load += new System.EventHandler(this.ucGrid_Load);
            this.Controls.SetChildIndex(this.gcGrid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AuthCheckEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcGrid;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvGrid;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gbHeader;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colSection;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandAct;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCrew;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWorkplaceName;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand15;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandDep;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand blankBandfinal;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit AuthCheckEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox ImageEdit;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDateCaptured;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProdmonth;
        private System.Windows.Forms.Timer timer;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colActivity;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colRiskRating;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colWorkplaceID;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMinerSection;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMOSection;
    }
}
