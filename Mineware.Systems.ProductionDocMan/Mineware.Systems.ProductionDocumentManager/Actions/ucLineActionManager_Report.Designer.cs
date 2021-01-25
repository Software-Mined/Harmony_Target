namespace Mineware.Systems.ProductionAmplatsDocMan.Actions
{
    partial class ucLineActionManager_Report
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
            this.stackPanel1 = new DevExpress.Utils.Layout.StackPanel();
            this.btnPrintChecklist = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.OCRCL = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tab1 = new DevExpress.XtraTab.XtraTabPage();
            this.pcReport = new FastReport.Preview.PreviewControl();
            this.tab2 = new DevExpress.XtraTab.XtraTabPage();
            this.pcReport2 = new FastReport.Preview.PreviewControl();
            this.tab3 = new DevExpress.XtraTab.XtraTabPage();
            this.pcReport3 = new FastReport.Preview.PreviewControl();
            this.tab4 = new DevExpress.XtraTab.XtraTabPage();
            this.pcReport4 = new FastReport.Preview.PreviewControl();
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel1)).BeginInit();
            this.stackPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OCRCL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.tab1.SuspendLayout();
            this.tab2.SuspendLayout();
            this.tab3.SuspendLayout();
            this.tab4.SuspendLayout();
            this.SuspendLayout();
            // 
            // stackPanel1
            // 
            this.stackPanel1.Appearance.BackColor = System.Drawing.Color.White;
            this.stackPanel1.Appearance.Options.UseBackColor = true;
            this.stackPanel1.Controls.Add(this.btnPrintChecklist);
            this.stackPanel1.Controls.Add(this.simpleButton1);
            this.stackPanel1.Controls.Add(this.OCRCL);
            this.stackPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.stackPanel1.Location = new System.Drawing.Point(0, 0);
            this.stackPanel1.Name = "stackPanel1";
            this.stackPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.stackPanel1.Size = new System.Drawing.Size(1200, 50);
            this.stackPanel1.TabIndex = 3;
            // 
            // btnPrintChecklist
            // 
            this.btnPrintChecklist.ImageOptions.SvgImage = global::DocumentManager.Properties.Resources.PrintBlue;
            this.btnPrintChecklist.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.btnPrintChecklist.Location = new System.Drawing.Point(1090, 10);
            this.btnPrintChecklist.Name = "btnPrintChecklist";
            this.btnPrintChecklist.Size = new System.Drawing.Size(107, 29);
            this.btnPrintChecklist.TabIndex = 221;
            this.btnPrintChecklist.Text = "Print";
            this.btnPrintChecklist.Click += new System.EventHandler(this.btnPrintChecklist_Click_1);
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.simpleButton1.Location = new System.Drawing.Point(950, 10);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(134, 29);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "Print Checklists";
            this.simpleButton1.Visible = false;
            // 
            // OCRCL
            // 
            this.OCRCL.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("150", "150: Close Actions", System.Windows.Forms.CheckState.Checked)});
            this.OCRCL.Location = new System.Drawing.Point(760, 5);
            this.OCRCL.Name = "OCRCL";
            this.OCRCL.Size = new System.Drawing.Size(184, 39);
            this.OCRCL.TabIndex = 220;
            this.OCRCL.Visible = false;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 50);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.tab1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1200, 934);
            this.xtraTabControl1.TabIndex = 24;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tab1,
            this.tab2,
            this.tab3,
            this.tab4});
            // 
            // tab1
            // 
            this.tab1.Controls.Add(this.pcReport);
            this.tab1.Name = "tab1";
            this.tab1.Size = new System.Drawing.Size(1198, 905);
            this.tab1.Text = "Page1";
            // 
            // pcReport
            // 
            this.pcReport.BackColor = System.Drawing.Color.Transparent;
            this.pcReport.Buttons = ((FastReport.PreviewButtons)(((((((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save) 
            | FastReport.PreviewButtons.Email) 
            | FastReport.PreviewButtons.Find) 
            | FastReport.PreviewButtons.Zoom) 
            | FastReport.PreviewButtons.Outline) 
            | FastReport.PreviewButtons.Navigator)));
            this.pcReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcReport.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pcReport.Location = new System.Drawing.Point(0, 0);
            this.pcReport.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.pcReport.Name = "pcReport";
            this.pcReport.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReport.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReport.SaveInitialDirectory = null;
            this.pcReport.Size = new System.Drawing.Size(1198, 905);
            this.pcReport.TabIndex = 22;
            this.pcReport.ToolbarVisible = false;
            this.pcReport.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            // 
            // tab2
            // 
            this.tab2.Controls.Add(this.pcReport2);
            this.tab2.Name = "tab2";
            this.tab2.Size = new System.Drawing.Size(1198, 920);
            this.tab2.Text = "Page2";
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
            this.pcReport2.Size = new System.Drawing.Size(1198, 920);
            this.pcReport2.TabIndex = 23;
            this.pcReport2.ToolbarVisible = false;
            this.pcReport2.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            // 
            // tab3
            // 
            this.tab3.Controls.Add(this.pcReport3);
            this.tab3.Name = "tab3";
            this.tab3.Size = new System.Drawing.Size(1198, 920);
            this.tab3.Text = "Page3";
            // 
            // pcReport3
            // 
            this.pcReport3.BackColor = System.Drawing.Color.Transparent;
            this.pcReport3.Buttons = ((FastReport.PreviewButtons)(((((((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save) 
            | FastReport.PreviewButtons.Email) 
            | FastReport.PreviewButtons.Find) 
            | FastReport.PreviewButtons.Zoom) 
            | FastReport.PreviewButtons.Outline) 
            | FastReport.PreviewButtons.Navigator)));
            this.pcReport3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcReport3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pcReport3.Location = new System.Drawing.Point(0, 0);
            this.pcReport3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.pcReport3.Name = "pcReport3";
            this.pcReport3.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReport3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReport3.SaveInitialDirectory = null;
            this.pcReport3.Size = new System.Drawing.Size(1198, 920);
            this.pcReport3.TabIndex = 23;
            this.pcReport3.ToolbarVisible = false;
            this.pcReport3.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            // 
            // tab4
            // 
            this.tab4.Controls.Add(this.pcReport4);
            this.tab4.Name = "tab4";
            this.tab4.Size = new System.Drawing.Size(1198, 920);
            this.tab4.Text = "Page4";
            // 
            // pcReport4
            // 
            this.pcReport4.BackColor = System.Drawing.Color.Transparent;
            this.pcReport4.Buttons = ((FastReport.PreviewButtons)(((((((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save) 
            | FastReport.PreviewButtons.Email) 
            | FastReport.PreviewButtons.Find) 
            | FastReport.PreviewButtons.Zoom) 
            | FastReport.PreviewButtons.Outline) 
            | FastReport.PreviewButtons.Navigator)));
            this.pcReport4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcReport4.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pcReport4.Location = new System.Drawing.Point(0, 0);
            this.pcReport4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.pcReport4.Name = "pcReport4";
            this.pcReport4.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReport4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReport4.SaveInitialDirectory = null;
            this.pcReport4.Size = new System.Drawing.Size(1198, 920);
            this.pcReport4.TabIndex = 23;
            this.pcReport4.ToolbarVisible = false;
            this.pcReport4.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            // 
            // ucLineActionManager_Report
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.stackPanel1);
            this.Name = "ucLineActionManager_Report";
            this.Size = new System.Drawing.Size(1200, 984);
            this.Load += new System.EventHandler(this.frmLineActionManager_Report_Load);
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel1)).EndInit();
            this.stackPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OCRCL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.tab1.ResumeLayout(false);
            this.tab2.ResumeLayout(false);
            this.tab3.ResumeLayout(false);
            this.tab4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.StackPanel stackPanel1;
        private DevExpress.XtraEditors.SimpleButton btnPrintChecklist;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.CheckedListBoxControl OCRCL;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage tab1;
        private FastReport.Preview.PreviewControl pcReport;
        private DevExpress.XtraTab.XtraTabPage tab2;
        private FastReport.Preview.PreviewControl pcReport2;
        private DevExpress.XtraTab.XtraTabPage tab3;
        private FastReport.Preview.PreviewControl pcReport3;
        private DevExpress.XtraTab.XtraTabPage tab4;
        private FastReport.Preview.PreviewControl pcReport4;
    }
}