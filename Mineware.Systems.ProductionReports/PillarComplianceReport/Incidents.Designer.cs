namespace Mineware.Systems.ProductionReports.PillarComplianceReport
{
    partial class Incidents
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.colWorkplace = new DevExpress.XtraReports.UI.XRTableCell();
            this.colAction = new DevExpress.XtraReports.UI.XRTableCell();
            this.colHazard = new DevExpress.XtraReports.UI.XRTableCell();
            this.picFlagRed = new DevExpress.XtraReports.UI.XRPictureBox();
            this.picFlagOrang = new DevExpress.XtraReports.UI.XRPictureBox();
            this.picFlagGreen = new DevExpress.XtraReports.UI.XRPictureBox();
            this.colTargetDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.colDateClosed = new DevExpress.XtraReports.UI.XRTableCell();
            this.colRespPerson = new DevExpress.XtraReports.UI.XRTableCell();
            this.colClosedBy = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table2});
            this.Detail.HeightF = 20F;
            this.Detail.Name = "Detail";
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // table2
            // 
            this.table2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.table2.KeepTogether = true;
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(3.814697E-06F, 0F);
            this.table2.Name = "table2";
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(807.0267F, 20F);
            this.table2.StylePriority.UseBorders = false;
            this.table2.StylePriority.UseTextAlignment = false;
            this.table2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colWorkplace,
            this.colAction,
            this.colHazard,
            this.colTargetDate,
            this.colDateClosed,
            this.colRespPerson,
            this.colClosedBy});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 11.5D;
            // 
            // colWorkplace
            // 
            this.colWorkplace.BorderColor = System.Drawing.Color.LightGray;
            this.colWorkplace.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colWorkplace.BorderWidth = 0.2F;
            this.colWorkplace.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.colWorkplace.Name = "colWorkplace";
            this.colWorkplace.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.colWorkplace.StylePriority.UseBorderColor = false;
            this.colWorkplace.StylePriority.UseBorders = false;
            this.colWorkplace.StylePriority.UseBorderWidth = false;
            this.colWorkplace.StylePriority.UseFont = false;
            this.colWorkplace.StylePriority.UsePadding = false;
            this.colWorkplace.Weight = 0.20783454592344969D;
            // 
            // colAction
            // 
            this.colAction.BorderColor = System.Drawing.Color.LightGray;
            this.colAction.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colAction.BorderWidth = 0.2F;
            this.colAction.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.colAction.Name = "colAction";
            this.colAction.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.colAction.StylePriority.UseBorderColor = false;
            this.colAction.StylePriority.UseBorders = false;
            this.colAction.StylePriority.UseBorderWidth = false;
            this.colAction.StylePriority.UseFont = false;
            this.colAction.StylePriority.UsePadding = false;
            this.colAction.StylePriority.UseTextAlignment = false;
            this.colAction.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.colAction.Weight = 0.32819815014507331D;
            // 
            // colHazard
            // 
            this.colHazard.BorderColor = System.Drawing.Color.LightGray;
            this.colHazard.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colHazard.BorderWidth = 0.2F;
            this.colHazard.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.picFlagRed,
            this.picFlagOrang,
            this.picFlagGreen});
            this.colHazard.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.colHazard.Name = "colHazard";
            this.colHazard.StylePriority.UseBorderColor = false;
            this.colHazard.StylePriority.UseBorders = false;
            this.colHazard.StylePriority.UseBorderWidth = false;
            this.colHazard.StylePriority.UseFont = false;
            this.colHazard.StylePriority.UseTextAlignment = false;
            this.colHazard.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colHazard.Weight = 0.055020761358219553D;
            // 
            // picFlagRed
            // 
            this.picFlagRed.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.picFlagRed.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(global::Mineware.Systems.ProductionReports.Properties.Resources.FlagRed1, true);
            this.picFlagRed.LocationFloat = new DevExpress.Utils.PointFloat(10.83334F, 1.999995F);
            this.picFlagRed.Name = "picFlagRed";
            this.picFlagRed.SizeF = new System.Drawing.SizeF(19.16672F, 17.5F);
            this.picFlagRed.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.picFlagRed.StylePriority.UseBorders = false;
            // 
            // picFlagOrang
            // 
            this.picFlagOrang.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.picFlagOrang.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(global::Mineware.Systems.ProductionReports.Properties.Resources.FlagOrange1, true);
            this.picFlagOrang.LocationFloat = new DevExpress.Utils.PointFloat(10.83334F, 1.999995F);
            this.picFlagOrang.Name = "picFlagOrang";
            this.picFlagOrang.SizeF = new System.Drawing.SizeF(19.16672F, 17.5F);
            this.picFlagOrang.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.picFlagOrang.StylePriority.UseBorders = false;
            // 
            // picFlagGreen
            // 
            this.picFlagGreen.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.picFlagGreen.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(global::Mineware.Systems.ProductionReports.Properties.Resources.FlagGreen1, true);
            this.picFlagGreen.LocationFloat = new DevExpress.Utils.PointFloat(10.83337F, 2F);
            this.picFlagGreen.Name = "picFlagGreen";
            this.picFlagGreen.SizeF = new System.Drawing.SizeF(19.16672F, 17.5F);
            this.picFlagGreen.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.picFlagGreen.StylePriority.UseBorders = false;
            // 
            // colTargetDate
            // 
            this.colTargetDate.BorderColor = System.Drawing.Color.LightGray;
            this.colTargetDate.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colTargetDate.BorderWidth = 0.2F;
            this.colTargetDate.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.colTargetDate.Name = "colTargetDate";
            this.colTargetDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.colTargetDate.StylePriority.UseBorderColor = false;
            this.colTargetDate.StylePriority.UseBorders = false;
            this.colTargetDate.StylePriority.UseBorderWidth = false;
            this.colTargetDate.StylePriority.UseFont = false;
            this.colTargetDate.StylePriority.UsePadding = false;
            this.colTargetDate.StylePriority.UseTextAlignment = false;
            this.colTargetDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colTargetDate.TextFormatString = "{0:dd MMM yyyy}";
            this.colTargetDate.Weight = 0.10037597166453582D;
            // 
            // colDateClosed
            // 
            this.colDateClosed.BorderColor = System.Drawing.Color.LightGray;
            this.colDateClosed.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colDateClosed.BorderWidth = 0.2F;
            this.colDateClosed.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.colDateClosed.Name = "colDateClosed";
            this.colDateClosed.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.colDateClosed.StylePriority.UseBorderColor = false;
            this.colDateClosed.StylePriority.UseBorders = false;
            this.colDateClosed.StylePriority.UseBorderWidth = false;
            this.colDateClosed.StylePriority.UseFont = false;
            this.colDateClosed.StylePriority.UsePadding = false;
            this.colDateClosed.StylePriority.UseTextAlignment = false;
            this.colDateClosed.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colDateClosed.TextFormatString = "{0:dd MMM yyyy}";
            this.colDateClosed.Weight = 0.10381488696677065D;
            // 
            // colRespPerson
            // 
            this.colRespPerson.BorderColor = System.Drawing.Color.LightGray;
            this.colRespPerson.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colRespPerson.BorderWidth = 0.2F;
            this.colRespPerson.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.colRespPerson.Multiline = true;
            this.colRespPerson.Name = "colRespPerson";
            this.colRespPerson.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.colRespPerson.StylePriority.UseBorderColor = false;
            this.colRespPerson.StylePriority.UseBorders = false;
            this.colRespPerson.StylePriority.UseBorderWidth = false;
            this.colRespPerson.StylePriority.UseFont = false;
            this.colRespPerson.StylePriority.UsePadding = false;
            this.colRespPerson.StylePriority.UseTextAlignment = false;
            this.colRespPerson.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colRespPerson.Weight = 0.14094538208458191D;
            // 
            // colClosedBy
            // 
            this.colClosedBy.BorderColor = System.Drawing.Color.LightGray;
            this.colClosedBy.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colClosedBy.BorderWidth = 0.2F;
            this.colClosedBy.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.colClosedBy.Multiline = true;
            this.colClosedBy.Name = "colClosedBy";
            this.colClosedBy.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.colClosedBy.StylePriority.UseBorderColor = false;
            this.colClosedBy.StylePriority.UseBorders = false;
            this.colClosedBy.StylePriority.UseBorderWidth = false;
            this.colClosedBy.StylePriority.UseFont = false;
            this.colClosedBy.StylePriority.UsePadding = false;
            this.colClosedBy.StylePriority.UseTextAlignment = false;
            this.colClosedBy.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.colClosedBy.Weight = 0.17388834403268424D;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table1});
            this.GroupHeader1.HeightF = 24.66667F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.StylePriority.UseTextAlignment = false;
            this.GroupHeader1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // table1
            // 
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(3.814697E-06F, 0F);
            this.table1.Name = "table1";
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(807.0267F, 24.66667F);
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell3,
            this.tableCell4,
            this.tableCell5,
            this.tableCell7,
            this.tableCell8,
            this.xrTableCell1,
            this.xrTableCell2});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 1D;
            // 
            // tableCell3
            // 
            this.tableCell3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableCell3.BorderColor = System.Drawing.Color.White;
            this.tableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell3.BorderWidth = 0.2F;
            this.tableCell3.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableCell3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StylePriority.UseBackColor = false;
            this.tableCell3.StylePriority.UseBorderColor = false;
            this.tableCell3.StylePriority.UseBorders = false;
            this.tableCell3.StylePriority.UseBorderWidth = false;
            this.tableCell3.StylePriority.UseFont = false;
            this.tableCell3.StylePriority.UseForeColor = false;
            this.tableCell3.StylePriority.UseTextAlignment = false;
            this.tableCell3.Text = "Workplace";
            this.tableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell3.Weight = 0.2078345360082863D;
            // 
            // tableCell4
            // 
            this.tableCell4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableCell4.BorderColor = System.Drawing.Color.White;
            this.tableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell4.BorderWidth = 0.2F;
            this.tableCell4.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableCell4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StylePriority.UseBackColor = false;
            this.tableCell4.StylePriority.UseBorderColor = false;
            this.tableCell4.StylePriority.UseBorders = false;
            this.tableCell4.StylePriority.UseBorderWidth = false;
            this.tableCell4.StylePriority.UseFont = false;
            this.tableCell4.StylePriority.UseForeColor = false;
            this.tableCell4.StylePriority.UseTextAlignment = false;
            this.tableCell4.Text = "Action";
            this.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell4.Weight = 0.32819812097664591D;
            // 
            // tableCell5
            // 
            this.tableCell5.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableCell5.BorderColor = System.Drawing.Color.White;
            this.tableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell5.BorderWidth = 0.2F;
            this.tableCell5.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableCell5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tableCell5.Name = "tableCell5";
            this.tableCell5.StylePriority.UseBackColor = false;
            this.tableCell5.StylePriority.UseBorderColor = false;
            this.tableCell5.StylePriority.UseBorders = false;
            this.tableCell5.StylePriority.UseBorderWidth = false;
            this.tableCell5.StylePriority.UseFont = false;
            this.tableCell5.StylePriority.UseForeColor = false;
            this.tableCell5.StylePriority.UseTextAlignment = false;
            this.tableCell5.Text = "Hazard";
            this.tableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell5.Weight = 0.055020757151133422D;
            // 
            // tableCell7
            // 
            this.tableCell7.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableCell7.BorderColor = System.Drawing.Color.White;
            this.tableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell7.BorderWidth = 0.2F;
            this.tableCell7.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableCell7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tableCell7.Name = "tableCell7";
            this.tableCell7.StylePriority.UseBackColor = false;
            this.tableCell7.StylePriority.UseBorderColor = false;
            this.tableCell7.StylePriority.UseBorders = false;
            this.tableCell7.StylePriority.UseBorderWidth = false;
            this.tableCell7.StylePriority.UseFont = false;
            this.tableCell7.StylePriority.UseForeColor = false;
            this.tableCell7.StylePriority.UseTextAlignment = false;
            this.tableCell7.Text = "Target Date";
            this.tableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell7.Weight = 0.10037596709223236D;
            // 
            // tableCell8
            // 
            this.tableCell8.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableCell8.BorderColor = System.Drawing.Color.White;
            this.tableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell8.BorderWidth = 0.2F;
            this.tableCell8.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableCell8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.StylePriority.UseBackColor = false;
            this.tableCell8.StylePriority.UseBorderColor = false;
            this.tableCell8.StylePriority.UseBorders = false;
            this.tableCell8.StylePriority.UseBorderWidth = false;
            this.tableCell8.StylePriority.UseFont = false;
            this.tableCell8.StylePriority.UseForeColor = false;
            this.tableCell8.StylePriority.UseTextAlignment = false;
            this.tableCell8.Text = "Date Closed";
            this.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell8.Weight = 0.10381488222387078D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.xrTableCell1.BorderColor = System.Drawing.Color.White;
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.BorderWidth = 0.2F;
            this.xrTableCell1.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseBorderColor = false;
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseBorderWidth = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseForeColor = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Responsible Person";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.14094546005408207D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.xrTableCell2.BorderColor = System.Drawing.Color.White;
            this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell2.BorderWidth = 0.2F;
            this.xrTableCell2.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UseBorderColor = false;
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseBorderWidth = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseForeColor = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Closed By";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.1738882466838183D;
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.BackColor = System.Drawing.Color.Snow;
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // Incidents
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.GroupHeader1});
            this.DrawGrid = false;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
            this.Version = "20.1";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.Incidents_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.XRTableCell tableCell5;
        private DevExpress.XtraReports.UI.XRTableCell tableCell7;
        private DevExpress.XtraReports.UI.XRTableCell tableCell8;
        private DevExpress.XtraReports.UI.XRTable table2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell colWorkplace;
        private DevExpress.XtraReports.UI.XRTableCell colAction;
        private DevExpress.XtraReports.UI.XRTableCell colHazard;
        private DevExpress.XtraReports.UI.XRTableCell colTargetDate;
        private DevExpress.XtraReports.UI.XRTableCell colDateClosed;
        private DevExpress.XtraReports.UI.XRTableCell colRespPerson;
        private DevExpress.XtraReports.UI.XRTableCell colClosedBy;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        private DevExpress.XtraReports.UI.XRPictureBox picFlagGreen;
        private DevExpress.XtraReports.UI.XRPictureBox picFlagRed;
        private DevExpress.XtraReports.UI.XRPictureBox picFlagOrang;
    }
}
