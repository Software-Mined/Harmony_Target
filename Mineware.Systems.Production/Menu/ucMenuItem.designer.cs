namespace Mineware.Systems.Production.Menu
{
    partial class ucMenuItem
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
            this.picMain = new System.Windows.Forms.PictureBox();
            this.lblMain = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.SuspendLayout();
            // 
            // picMain
            // 
            this.picMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picMain.Location = new System.Drawing.Point(0, 0);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(128, 128);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            // 
            // lblMain
            // 
            this.lblMain.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblMain.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(1)), true);
            this.lblMain.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblMain.Appearance.Options.UseBackColor = true;
            this.lblMain.Appearance.Options.UseFont = true;
            this.lblMain.Appearance.Options.UseForeColor = true;
            this.lblMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMain.Location = new System.Drawing.Point(0, 103);
            this.lblMain.Name = "lblMain";
            this.lblMain.Padding = new System.Windows.Forms.Padding(5, 0, 0, 5);
            this.lblMain.Size = new System.Drawing.Size(125, 25);
            this.lblMain.TabIndex = 1;
            this.lblMain.Text = "Menu Item Name";
            // 
            // ucMenuItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMain);
            this.Controls.Add(this.picMain);
            this.Name = "ucMenuItem";
            this.Size = new System.Drawing.Size(128, 128);
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picMain;
        private DevExpress.XtraEditors.LabelControl lblMain;
    }
}
