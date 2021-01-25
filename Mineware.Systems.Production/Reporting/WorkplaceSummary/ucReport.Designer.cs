
namespace Mineware.Systems.Production.Reporting.WorkplaceSummary
{
    partial class ucReport
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
            this.pcReport = new FastReport.Preview.PreviewControl();
            this.timer = new System.Windows.Forms.Timer(this.components);
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
            this.pcReport.Size = new System.Drawing.Size(1132, 708);
            this.pcReport.TabIndex = 89;
            this.pcReport.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // ucReport
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pcReport);
            this.Name = "ucReport";
            this.ShowIInfo = false;
            this.Size = new System.Drawing.Size(1132, 708);
            this.Load += new System.EventHandler(this.ucReport_Load);
            this.Controls.SetChildIndex(this.pcReport, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastReport.Preview.PreviewControl pcReport;
        private System.Windows.Forms.Timer timer;
    }
}
