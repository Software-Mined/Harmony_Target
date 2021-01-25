namespace Mineware.Systems.Production.Logistics_Management
{
    partial class frmSDBReport
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
            this.pcReport2 = new FastReport.Preview.PreviewControl();
            this.SuspendLayout();
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
            this.pcReport2.Size = new System.Drawing.Size(1064, 598);
            this.pcReport2.TabIndex = 5;
            this.pcReport2.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            // 
            // frmSDBReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 598);
            this.Controls.Add(this.pcReport2);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmSDBReport";
            this.Text = "SDB Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSDBReport_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private FastReport.Preview.PreviewControl pcReport2;
    }
}