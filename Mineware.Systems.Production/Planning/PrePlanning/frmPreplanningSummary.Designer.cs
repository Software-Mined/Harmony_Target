namespace Mineware.Systems.Production.Planning
{
    partial class frmPreplanningSummary
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
            this.pcReport = new FastReport.Preview.PreviewControl();
            this.SuspendLayout();
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
            this.pcReport.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.pcReport.Name = "pcReport";
            this.pcReport.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReport.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReport.SaveInitialDirectory = null;
            this.pcReport.Size = new System.Drawing.Size(1209, 838);
            this.pcReport.TabIndex = 88;
            this.pcReport.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            // 
            // frmPreplanningSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1209, 838);
            this.Controls.Add(this.pcReport);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPreplanningSummary";
            this.Text = "Pre-Planning Report";
            this.Load += new System.EventHandler(this.frmPreplanningSummary_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private FastReport.Preview.PreviewControl pcReport;
    }
}