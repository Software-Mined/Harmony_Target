namespace Mineware.Systems.Production.Reporting.Analysis
{
    partial class frmColourPicker
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
            this.sbtnSave = new DevExpress.XtraEditors.SimpleButton();
            this.colorPickEdit1 = new DevExpress.XtraEditors.ColorPickEdit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // sbtnSave
            // 
            this.sbtnSave.Appearance.BackColor = System.Drawing.Color.White;
            this.sbtnSave.Appearance.Options.UseBackColor = true;
            this.sbtnSave.AppearanceHovered.BackColor = System.Drawing.SystemColors.Highlight;
            this.sbtnSave.AppearanceHovered.Options.UseBackColor = true;
            this.sbtnSave.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.sbtnSave.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.sbtnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.sbtnSave.Location = new System.Drawing.Point(160, 17);
            this.sbtnSave.Name = "sbtnSave";
            this.sbtnSave.Size = new System.Drawing.Size(96, 31);
            this.sbtnSave.TabIndex = 16;
            this.sbtnSave.Text = "Save";
            this.sbtnSave.Click += new System.EventHandler(this.sbtnSave_Click);
            // 
            // colorPickEdit1
            // 
            this.colorPickEdit1.EditValue = System.Drawing.Color.Empty;
            this.colorPickEdit1.Location = new System.Drawing.Point(41, 20);
            this.colorPickEdit1.Margin = new System.Windows.Forms.Padding(4);
            this.colorPickEdit1.Name = "colorPickEdit1";
            this.colorPickEdit1.Properties.AutomaticColor = System.Drawing.Color.Black;
            this.colorPickEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorPickEdit1.Properties.ShowSystemColors = false;
            this.colorPickEdit1.Properties.ShowWebColors = false;
            this.colorPickEdit1.Properties.StoreColorAsInteger = true;
            this.colorPickEdit1.Size = new System.Drawing.Size(71, 24);
            this.colorPickEdit1.TabIndex = 17;
            // 
            // frmColourPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 74);
            this.Controls.Add(this.colorPickEdit1);
            this.Controls.Add(this.sbtnSave);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmColourPicker";
            this.Text = "Pick A Colour";
            this.Load += new System.EventHandler(this.frmColourPicker_Load);
            ((System.ComponentModel.ISupportInitialize)(this.colorPickEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton sbtnSave;
        private DevExpress.XtraEditors.ColorPickEdit colorPickEdit1;
    }
}