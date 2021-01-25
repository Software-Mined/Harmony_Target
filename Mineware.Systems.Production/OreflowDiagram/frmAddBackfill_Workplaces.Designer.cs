namespace Mineware.Systems.Production.OreflowDiagram
{
    partial class frmAddBackfill_Workplaces
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddBackfill_Workplaces));
            this.Filtertxt = new System.Windows.Forms.TextBox();
            this.lbxWorkplaces = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // Filtertxt
            // 
            this.Filtertxt.Location = new System.Drawing.Point(75, 8);
            this.Filtertxt.Margin = new System.Windows.Forms.Padding(4);
            this.Filtertxt.Name = "Filtertxt";
            this.Filtertxt.Size = new System.Drawing.Size(267, 25);
            this.Filtertxt.TabIndex = 185;
            this.Filtertxt.TextChanged += new System.EventHandler(this.Filtertxt_TextChanged);
            // 
            // lbxWorkplaces
            // 
            this.lbxWorkplaces.FormattingEnabled = true;
            this.lbxWorkplaces.ItemHeight = 17;
            this.lbxWorkplaces.Location = new System.Drawing.Point(12, 42);
            this.lbxWorkplaces.Margin = new System.Windows.Forms.Padding(4);
            this.lbxWorkplaces.Name = "lbxWorkplaces";
            this.lbxWorkplaces.Size = new System.Drawing.Size(330, 174);
            this.lbxWorkplaces.TabIndex = 183;
            this.lbxWorkplaces.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 19);
            this.label1.TabIndex = 184;
            this.label1.Text = "Filter :";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Location = new System.Drawing.Point(31, 220);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(301, 35);
            this.pnlButtons.TabIndex = 186;
            // 
            // btnCancel
            // 
            this.btnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCancel.ImageOptions.SvgImage")));
            this.btnCancel.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnCancel.Location = new System.Drawing.Point(152, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 27);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.SaveBlue2;
            this.btnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnSave.Location = new System.Drawing.Point(31, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 27);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmAddBackfill_Workplaces
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 266);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.Filtertxt);
            this.Controls.Add(this.lbxWorkplaces);
            this.Controls.Add(this.label1);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmAddBackfill_Workplaces";
            this.Text = "Add Workplace";
            this.Load += new System.EventHandler(this.frmAddBackfill_Workplaces_Load);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Filtertxt;
        private System.Windows.Forms.ListBox lbxWorkplaces;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlButtons;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
    }
}