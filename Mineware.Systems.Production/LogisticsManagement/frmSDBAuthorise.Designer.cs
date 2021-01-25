namespace Mineware.Systems.Production.Logistics_Management
{
    partial class frmSDBAuthorise
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
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave2 = new DevExpress.XtraEditors.SimpleButton();
            this.separatorButtons = new DevExpress.XtraEditors.SeparatorControl();
            this.Notetxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel2);
            this.pnlButtons.Controls.Add(this.btnSave2);
            this.pnlButtons.Location = new System.Drawing.Point(185, 363);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(260, 35);
            this.pnlButtons.TabIndex = 20;
            // 
            // btnCancel2
            // 
            this.btnCancel2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.close;
            this.btnCancel2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnCancel2.Location = new System.Drawing.Point(147, 5);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(94, 26);
            this.btnCancel2.TabIndex = 42;
            this.btnCancel2.Text = "Cancel";
            this.btnCancel2.Click += new System.EventHandler(this.btnCancel2_Click);
            // 
            // btnSave2
            // 
            this.btnSave2.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.btnSave2.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSave2.Location = new System.Drawing.Point(30, 4);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(94, 26);
            this.btnSave2.TabIndex = 41;
            this.btnSave2.Text = "Save";
            this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
            // 
            // separatorButtons
            // 
            this.separatorButtons.Location = new System.Drawing.Point(12, 329);
            this.separatorButtons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.separatorButtons.Name = "separatorButtons";
            this.separatorButtons.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.separatorButtons.Size = new System.Drawing.Size(660, 31);
            this.separatorButtons.TabIndex = 21;
            // 
            // Notetxt
            // 
            this.Notetxt.Location = new System.Drawing.Point(33, 53);
            this.Notetxt.Multiline = true;
            this.Notetxt.Name = "Notetxt";
            this.Notetxt.Size = new System.Drawing.Size(589, 269);
            this.Notetxt.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(29, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 19);
            this.label1.TabIndex = 23;
            this.label1.Text = "Note";
            // 
            // frmSDBAuthorise
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 410);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Notetxt);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.separatorButtons);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmSDBAuthorise";
            this.Text = "Authorisation";
            this.Load += new System.EventHandler(this.frmSDBAuthorise_Load);
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlButtons;
        private DevExpress.XtraEditors.SeparatorControl separatorButtons;
        private System.Windows.Forms.TextBox Notetxt;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnCancel2;
        private DevExpress.XtraEditors.SimpleButton btnSave2;
    }
}