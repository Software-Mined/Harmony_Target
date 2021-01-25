namespace Mineware.Systems.Production.OreflowDiagram
{
    partial class frmAddOreflow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddOreflow));
            this.gcOreflowentities = new DevExpress.XtraGrid.GridControl();
            this.gvOreflowentities = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.separatorButtons = new DevExpress.XtraEditors.SeparatorControl();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gcOreflowentities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOreflowentities)).BeginInit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // gcOreflowentities
            // 
            this.gcOreflowentities.Location = new System.Drawing.Point(12, 24);
            this.gcOreflowentities.MainView = this.gvOreflowentities;
            this.gcOreflowentities.Name = "gcOreflowentities";
            this.gcOreflowentities.Size = new System.Drawing.Size(490, 312);
            this.gcOreflowentities.TabIndex = 4;
            this.gcOreflowentities.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOreflowentities});
            // 
            // gvOreflowentities
            // 
            this.gvOreflowentities.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDescription,
            this.colID});
            this.gvOreflowentities.DetailHeight = 458;
            this.gvOreflowentities.GridControl = this.gcOreflowentities;
            this.gvOreflowentities.Name = "gvOreflowentities";
            this.gvOreflowentities.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvOreflowentities.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvOreflowentities.OptionsBehavior.Editable = false;
            this.gvOreflowentities.OptionsBehavior.ReadOnly = true;
            this.gvOreflowentities.OptionsView.ShowGroupPanel = false;
            this.gvOreflowentities.OptionsView.ShowIndicator = false;
            // 
            // colDescription
            // 
            this.colDescription.AppearanceHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.colDescription.AppearanceHeader.Options.UseForeColor = true;
            this.colDescription.Caption = "Description";
            this.colDescription.MinWidth = 25;
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.FixedWidth = true;
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 0;
            this.colDescription.Width = 100;
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.MinWidth = 25;
            this.colID.Name = "colID";
            this.colID.OptionsColumn.FixedWidth = true;
            this.colID.Width = 100;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(511, 106);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(212, 25);
            this.txtName.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(507, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Location = new System.Drawing.Point(241, 385);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(301, 35);
            this.pnlButtons.TabIndex = 7;
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
            // separatorButtons
            // 
            this.separatorButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separatorButtons.Location = new System.Drawing.Point(6, 356);
            this.separatorButtons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.separatorButtons.Name = "separatorButtons";
            this.separatorButtons.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.separatorButtons.Size = new System.Drawing.Size(720, 21);
            this.separatorButtons.TabIndex = 8;
            // 
            // btnAdd
            // 
            this.btnAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.ImageOptions.Image")));
            this.btnAdd.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.AddBlue1;
            this.btnAdd.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnAdd.Location = new System.Drawing.Point(526, 152);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(79, 31);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add  ";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.ImageOptions.SvgImage = global::Mineware.Systems.Production.Properties.Resources.DeleteRed;
            this.btnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnDelete.Location = new System.Drawing.Point(617, 152);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 31);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete  ";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmAddOreflow
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 430);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.separatorButtons);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gcOreflowentities);
            this.IconOptions.Image = global::Mineware.Systems.Production.Properties.Resources.SM;
            this.Name = "frmAddOreflow";
            this.Text = "Add Oreflow";
            this.Load += new System.EventHandler(this.frmAddOreflow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcOreflowentities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOreflowentities)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorButtons)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcOreflowentities;
        private DevExpress.XtraGrid.Views.Grid.GridView gvOreflowentities;
        private DevExpress.XtraGrid.Columns.GridColumn colDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlButtons;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SeparatorControl separatorButtons;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
    }
}