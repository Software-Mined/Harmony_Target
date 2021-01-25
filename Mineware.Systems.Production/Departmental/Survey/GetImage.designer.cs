namespace Mineware.Systems.Production.Departmental.Survey
{
    partial class GetImage
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aaaaaaaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panPic = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.OpenFormtxt = new System.Windows.Forms.Label();
            this.DDLetterlabel = new System.Windows.Forms.Label();
            this.ActIDLbl = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aaaaaaaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1053, 30);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aaaaaaaToolStripMenuItem
            // 
            this.aaaaaaaToolStripMenuItem.Image = global::Mineware.Systems.Production.Properties.Resources.PictureBox_32x32;
            this.aaaaaaaToolStripMenuItem.Name = "aaaaaaaToolStripMenuItem";
            this.aaaaaaaToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.aaaaaaaToolStripMenuItem.Text = "Capture Image";
            this.aaaaaaaToolStripMenuItem.Click += new System.EventHandler(this.aaaaaaaToolStripMenuItem_Click);
            // 
            // panPic
            // 
            this.panPic.BackColor = System.Drawing.Color.Transparent;
            this.panPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panPic.Location = new System.Drawing.Point(0, 30);
            this.panPic.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panPic.Name = "panPic";
            this.panPic.Size = new System.Drawing.Size(1053, 540);
            this.panPic.TabIndex = 2;
            this.panPic.Visible = false;
            // 
            // OpenFormtxt
            // 
            this.OpenFormtxt.AutoSize = true;
            this.OpenFormtxt.Location = new System.Drawing.Point(633, 10);
            this.OpenFormtxt.Name = "OpenFormtxt";
            this.OpenFormtxt.Size = new System.Drawing.Size(45, 19);
            this.OpenFormtxt.TabIndex = 0;
            this.OpenFormtxt.Text = "label1";
            this.OpenFormtxt.Visible = false;
            // 
            // DDLetterlabel
            // 
            this.DDLetterlabel.AutoSize = true;
            this.DDLetterlabel.Location = new System.Drawing.Point(496, 8);
            this.DDLetterlabel.Name = "DDLetterlabel";
            this.DDLetterlabel.Size = new System.Drawing.Size(45, 19);
            this.DDLetterlabel.TabIndex = 3;
            this.DDLetterlabel.Text = "label1";
            this.DDLetterlabel.Visible = false;
            this.DDLetterlabel.Click += new System.EventHandler(this.DDLetterlabel_Click);
            // 
            // ActIDLbl
            // 
            this.ActIDLbl.AutoSize = true;
            this.ActIDLbl.Location = new System.Drawing.Point(717, 8);
            this.ActIDLbl.Name = "ActIDLbl";
            this.ActIDLbl.Size = new System.Drawing.Size(45, 19);
            this.ActIDLbl.TabIndex = 4;
            this.ActIDLbl.Text = "label1";
            this.ActIDLbl.Visible = false;
            // 
            // GetImage
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 570);
            this.Controls.Add(this.ActIDLbl);
            this.Controls.Add(this.DDLetterlabel);
            this.Controls.Add(this.OpenFormtxt);
            this.Controls.Add(this.panPic);
            this.Controls.Add(this.menuStrip1);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GetImage";
            this.Text = "Get Image";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.GetImage_Load);
            this.SizeChanged += new System.EventHandler(this.GetImage_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GetImage_Paint);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aaaaaaaToolStripMenuItem;
        private System.Windows.Forms.Panel panPic;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        public System.Windows.Forms.Label OpenFormtxt;
        public System.Windows.Forms.Label DDLetterlabel;
        public System.Windows.Forms.Label ActIDLbl;

    }
}