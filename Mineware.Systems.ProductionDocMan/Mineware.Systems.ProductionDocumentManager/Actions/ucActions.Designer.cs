namespace Mineware.Systems.DocumentManager
{
    partial class ucActions
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
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.pdfViewer1 = new DevExpress.XtraPdfViewer.PdfViewer();
            this.stackPanel1 = new DevExpress.Utils.Layout.StackPanel();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.OCRCL = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.btnPrintChecklist = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel1)).BeginInit();
            this.stackPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OCRCL)).BeginInit();
            this.SuspendLayout();
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "45",
            "30",
            "20"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // groupControl1
            // 
            this.groupControl1.CaptionImageOptions.SvgImage = global::DocumentManager.Properties.Resources.WarningRed;
            this.groupControl1.CaptionImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.groupControl1.Controls.Add(this.pdfViewer1);
            this.groupControl1.Controls.Add(this.stackPanel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.LookAndFeel.SkinName = "Office 2019 White";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1220, 612);
            this.groupControl1.TabIndex = 49;
            this.groupControl1.Text = "Action Manager";
            // 
            // pdfViewer1
            // 
            this.pdfViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfViewer1.Location = new System.Drawing.Point(2, 72);
            this.pdfViewer1.Name = "pdfViewer1";
            this.pdfViewer1.Size = new System.Drawing.Size(1216, 538);
            this.pdfViewer1.TabIndex = 2;
            // 
            // stackPanel1
            // 
            this.stackPanel1.Controls.Add(this.btnPrintChecklist);
            this.stackPanel1.Controls.Add(this.simpleButton1);
            this.stackPanel1.Controls.Add(this.OCRCL);
            this.stackPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.stackPanel1.Location = new System.Drawing.Point(2, 28);
            this.stackPanel1.Name = "stackPanel1";
            this.stackPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.stackPanel1.Size = new System.Drawing.Size(1216, 44);
            this.stackPanel1.TabIndex = 1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.SvgImage = global::DocumentManager.Properties.Resources.PrintBlue;
            this.simpleButton1.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.simpleButton1.Location = new System.Drawing.Point(966, 7);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(134, 29);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "Print Checklists";
            this.simpleButton1.Visible = false;
            this.simpleButton1.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // OCRCL
            // 
            this.OCRCL.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("150", "150: Close Actions", System.Windows.Forms.CheckState.Checked)});
            this.OCRCL.Location = new System.Drawing.Point(776, 2);
            this.OCRCL.Name = "OCRCL";
            this.OCRCL.Size = new System.Drawing.Size(184, 39);
            this.OCRCL.TabIndex = 220;
            this.OCRCL.Visible = false;
            // 
            // btnPrintChecklist
            // 
            this.btnPrintChecklist.ImageOptions.SvgImage = global::DocumentManager.Properties.Resources.PrintBlue;
            this.btnPrintChecklist.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.btnPrintChecklist.Location = new System.Drawing.Point(1106, 7);
            this.btnPrintChecklist.Name = "btnPrintChecklist";
            this.btnPrintChecklist.Size = new System.Drawing.Size(107, 29);
            this.btnPrintChecklist.TabIndex = 221;
            this.btnPrintChecklist.Text = "Print";
            this.btnPrintChecklist.Click += new System.EventHandler(this.btnPrintChecklist_Click);
            // 
            // ucActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "ucActions";
            this.Size = new System.Drawing.Size(1220, 612);
            this.Load += new System.EventHandler(this.ucVentAuth_Load);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel1)).EndInit();
            this.stackPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OCRCL)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
#pragma warning disable CS0169 // The field 'ucActions.defaultLookAndFeel1' is never used
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
#pragma warning restore CS0169 // The field 'ucActions.defaultLookAndFeel1' is never used
        private DevExpress.Utils.Layout.StackPanel stackPanel1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraPdfViewer.PdfViewer pdfViewer1;
        private DevExpress.XtraEditors.CheckedListBoxControl OCRCL;
        private DevExpress.XtraEditors.SimpleButton btnPrintChecklist;
    }
}
