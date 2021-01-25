namespace Mineware.Systems.Production.Departmental.Ventillation
{
	partial class ucVentChecklistReport
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
            this.barProbAnalysisShow = new DevExpress.XtraBars.BarButtonItem();
            this.rgpReportFilter = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.pcReport = new FastReport.Preview.PreviewControl();
            this.SuspendLayout();
            // 
            // barProbAnalysisShow
            // 
            this.barProbAnalysisShow.Caption = "Show";
            this.barProbAnalysisShow.Id = 14;
            this.barProbAnalysisShow.Name = "barProbAnalysisShow";
            // 
            // rgpReportFilter
            // 
            this.rgpReportFilter.Name = "rgpReportFilter";
            this.rgpReportFilter.Text = "Filter";
            // 
            // pcReport
            // 
            this.pcReport.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pcReport.Buttons = ((FastReport.PreviewButtons)((((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save) 
            | FastReport.PreviewButtons.Zoom) 
            | FastReport.PreviewButtons.Navigator)));
            this.pcReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcReport.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pcReport.Location = new System.Drawing.Point(0, 0);
            this.pcReport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pcReport.Name = "pcReport";
            this.pcReport.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReport.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReport.SaveInitialDirectory = null;
            this.pcReport.Size = new System.Drawing.Size(975, 548);
            this.pcReport.TabIndex = 88;
            this.pcReport.UIStyle = FastReport.Utils.UIStyle.Office2007Black;
            // 
            // ucVentChecklistReport
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pcReport);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "ucVentChecklistReport";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(975, 548);
            this.Load += new System.EventHandler(this.ucVentChecklistReport_Load);
            this.Controls.SetChildIndex(this.pcReport, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraBars.BarButtonItem barProbAnalysisShow;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup rgpReportFilter;
		private FastReport.Preview.PreviewControl pcReport;
	}
}
