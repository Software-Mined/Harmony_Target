namespace Mineware.Systems.Production.Reporting
{
	partial class ucReports
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
            this.pnlTestReport = new System.Windows.Forms.Panel();
            this.accMain = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.pnlSideBar = new System.Windows.Forms.Panel();
            this.pcReport2 = new FastReport.Preview.PreviewControl();
            this.accordionControlElement1 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement3 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionContentContainer1 = new DevExpress.XtraBars.Navigation.AccordionContentContainer();
            this.pnlTestReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accMain)).BeginInit();
            this.accMain.SuspendLayout();
            this.accordionContentContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTestReport
            // 
            this.pnlTestReport.Controls.Add(this.pcReport2);
            this.pnlTestReport.Controls.Add(this.accMain);
            this.pnlTestReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTestReport.Location = new System.Drawing.Point(0, 0);
            this.pnlTestReport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTestReport.Name = "pnlTestReport";
            this.pnlTestReport.Size = new System.Drawing.Size(1060, 548);
            this.pnlTestReport.TabIndex = 0;
            // 
            // accMain
            // 
            this.accMain.Controls.Add(this.accordionContentContainer1);
            this.accMain.Dock = System.Windows.Forms.DockStyle.Right;
            this.accMain.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement1});
            this.accMain.Location = new System.Drawing.Point(803, 0);
            this.accMain.Name = "accMain";
            this.accMain.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Hidden;
            this.accMain.Size = new System.Drawing.Size(257, 548);
            this.accMain.TabIndex = 11;
            this.accMain.Text = "Filter";
            this.accMain.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu;
            // 
            // pnlSideBar
            // 
            this.pnlSideBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSideBar.Location = new System.Drawing.Point(0, 0);
            this.pnlSideBar.Name = "pnlSideBar";
            this.pnlSideBar.Size = new System.Drawing.Size(257, 403);
            this.pnlSideBar.TabIndex = 12;
            // 
            // pcReport2
            // 
            this.pcReport2.BackColor = System.Drawing.Color.Transparent;
            this.pcReport2.Buttons = ((FastReport.PreviewButtons)(((((((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save) 
            | FastReport.PreviewButtons.Email) 
            | FastReport.PreviewButtons.Find) 
            | FastReport.PreviewButtons.Zoom) 
            | FastReport.PreviewButtons.Outline) 
            | FastReport.PreviewButtons.Navigator)));
            this.pcReport2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcReport2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pcReport2.Location = new System.Drawing.Point(0, 0);
            this.pcReport2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.pcReport2.Name = "pcReport2";
            this.pcReport2.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReport2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReport2.SaveInitialDirectory = null;
            this.pcReport2.Size = new System.Drawing.Size(803, 548);
            this.pcReport2.TabIndex = 4;
            this.pcReport2.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            this.pcReport2.Visible = false;
            // 
            // accordionControlElement1
            // 
            this.accordionControlElement1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement3});
            this.accordionControlElement1.Expanded = true;
            this.accordionControlElement1.Name = "accordionControlElement1";
            this.accordionControlElement1.Text = "Report Filters";
            // 
            // accordionControlElement3
            // 
            this.accordionControlElement3.ContentContainer = this.accordionContentContainer1;
            this.accordionControlElement3.Expanded = true;
            this.accordionControlElement3.Name = "accordionControlElement3";
            this.accordionControlElement3.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement3.Text = "Filter";
            // 
            // accordionContentContainer1
            // 
            this.accordionContentContainer1.Controls.Add(this.pnlSideBar);
            this.accordionContentContainer1.Name = "accordionContentContainer1";
            this.accordionContentContainer1.Size = new System.Drawing.Size(257, 403);
            this.accordionContentContainer1.TabIndex = 15;
            // 
            // ucReports
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlTestReport);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "ucReports";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(1060, 548);
            this.Controls.SetChildIndex(this.pnlTestReport, 0);
            this.pnlTestReport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accMain)).EndInit();
            this.accMain.ResumeLayout(false);
            this.accordionContentContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlTestReport;
		private FastReport.Preview.PreviewControl pcReport2;
        public DevExpress.XtraBars.Navigation.AccordionControl accMain;
        private System.Windows.Forms.Panel pnlSideBar;
        private DevExpress.XtraBars.Navigation.AccordionContentContainer accordionContentContainer1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement3;
    }
}
