using DocumentManager.Properties;

namespace Mineware.Systems.DocumentManager
{
    partial class FPMessage
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
            this.TextLbl = new System.Windows.Forms.Label();
            this.DeclineBTN = new DevExpress.XtraEditors.SimpleButton();
            this.RemBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSectionMan = new System.Windows.Forms.Label();
            this.lblSection = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblNotetype = new System.Windows.Forms.Label();
            this.Lvllbl = new System.Windows.Forms.Label();
            this.lblUerID = new System.Windows.Forms.Label();
            this.UserLbl = new System.Windows.Forms.Label();
            this.DeclineBtn1 = new System.Windows.Forms.Button();
            this.AcceptBtn1 = new System.Windows.Forms.Button();
            this.FPTypeLbl = new System.Windows.Forms.Label();
            this.FPNoLbl = new System.Windows.Forms.Label();
            this.AcceptBTN = new DevExpress.XtraEditors.SimpleButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pcReport = new FastReport.Preview.PreviewControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextLbl
            // 
            this.TextLbl.AutoSize = true;
            this.TextLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.TextLbl.ForeColor = System.Drawing.Color.Red;
            this.TextLbl.Location = new System.Drawing.Point(17, 26);
            this.TextLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TextLbl.Name = "TextLbl";
            this.TextLbl.Size = new System.Drawing.Size(279, 30);
            this.TextLbl.TabIndex = 41;
            this.TextLbl.Text = "Survey Note Authorisation";
            this.TextLbl.TextChanged += new System.EventHandler(this.TextLbl_TextChanged);
            // 
            // DeclineBTN
            // 
            this.DeclineBTN.Appearance.BackColor = System.Drawing.Color.DarkSalmon;
            this.DeclineBTN.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F);
            this.DeclineBTN.Appearance.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DeclineBTN.Appearance.Options.UseBackColor = true;
            this.DeclineBTN.Appearance.Options.UseFont = true;
            this.DeclineBTN.Appearance.Options.UseForeColor = true;
            this.DeclineBTN.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.DeclineBTN.ImageOptions.ImageIndex = 0;
            this.DeclineBTN.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.DeclineBTN.ImageOptions.SvgImage = global::DocumentManager.Properties.Resources.CircularCrossRed;
            this.DeclineBTN.Location = new System.Drawing.Point(164, 248);
            this.DeclineBTN.Margin = new System.Windows.Forms.Padding(4);
            this.DeclineBTN.Name = "DeclineBTN";
            this.DeclineBTN.Size = new System.Drawing.Size(120, 40);
            this.DeclineBTN.TabIndex = 42;
            this.DeclineBTN.Text = "Decline     ";
            this.DeclineBTN.Click += new System.EventHandler(this.DeclineBTN_Click);
            // 
            // RemBox
            // 
            this.RemBox.Location = new System.Drawing.Point(22, 91);
            this.RemBox.Margin = new System.Windows.Forms.Padding(4);
            this.RemBox.MaxLength = 299;
            this.RemBox.Multiline = true;
            this.RemBox.Name = "RemBox";
            this.RemBox.Size = new System.Drawing.Size(277, 129);
            this.RemBox.TabIndex = 43;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(18, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 19);
            this.label1.TabIndex = 44;
            this.label1.Text = "Notes";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelControl1);
            this.panel1.Controls.Add(this.Lvllbl);
            this.panel1.Controls.Add(this.AcceptBTN);
            this.panel1.Controls.Add(this.DeclineBTN);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TextLbl);
            this.panel1.Controls.Add(this.RemBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 738);
            this.panel1.TabIndex = 45;
            // 
            // lblSectionMan
            // 
            this.lblSectionMan.AutoSize = true;
            this.lblSectionMan.Location = new System.Drawing.Point(61, 24);
            this.lblSectionMan.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSectionMan.Name = "lblSectionMan";
            this.lblSectionMan.Size = new System.Drawing.Size(45, 19);
            this.lblSectionMan.TabIndex = 56;
            this.lblSectionMan.Text = "label2";
            this.lblSectionMan.Visible = false;
            // 
            // lblSection
            // 
            this.lblSection.AutoSize = true;
            this.lblSection.Location = new System.Drawing.Point(81, 39);
            this.lblSection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(45, 19);
            this.lblSection.TabIndex = 55;
            this.lblSection.Text = "label2";
            this.lblSection.Visible = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(81, 39);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(45, 19);
            this.lblDate.TabIndex = 54;
            this.lblDate.Text = "label2";
            this.lblDate.Visible = false;
            // 
            // lblNotetype
            // 
            this.lblNotetype.AutoSize = true;
            this.lblNotetype.Location = new System.Drawing.Point(47, 39);
            this.lblNotetype.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNotetype.Name = "lblNotetype";
            this.lblNotetype.Size = new System.Drawing.Size(45, 19);
            this.lblNotetype.TabIndex = 53;
            this.lblNotetype.Text = "label2";
            this.lblNotetype.Visible = false;
            // 
            // Lvllbl
            // 
            this.Lvllbl.AutoSize = true;
            this.Lvllbl.Location = new System.Drawing.Point(93, 68);
            this.Lvllbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lvllbl.Name = "Lvllbl";
            this.Lvllbl.Size = new System.Drawing.Size(45, 19);
            this.Lvllbl.TabIndex = 52;
            this.Lvllbl.Text = "label2";
            this.Lvllbl.Visible = false;
            // 
            // lblUerID
            // 
            this.lblUerID.AutoSize = true;
            this.lblUerID.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUerID.ForeColor = System.Drawing.Color.Red;
            this.lblUerID.Location = new System.Drawing.Point(-5, 39);
            this.lblUerID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUerID.Name = "lblUerID";
            this.lblUerID.Size = new System.Drawing.Size(254, 28);
            this.lblUerID.TabIndex = 51;
            this.lblUerID.Text = "Survey Note Authorisation";
            this.lblUerID.UseWaitCursor = true;
            this.lblUerID.Visible = false;
            // 
            // UserLbl
            // 
            this.UserLbl.AutoSize = true;
            this.UserLbl.Location = new System.Drawing.Point(126, 29);
            this.UserLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserLbl.Name = "UserLbl";
            this.UserLbl.Size = new System.Drawing.Size(22, 19);
            this.UserLbl.TabIndex = 50;
            this.UserLbl.Text = "M";
            this.UserLbl.UseWaitCursor = true;
            this.UserLbl.Visible = false;
            this.UserLbl.TextChanged += new System.EventHandler(this.UserLbl_TextChanged);
            // 
            // DeclineBtn1
            // 
            this.DeclineBtn1.Location = new System.Drawing.Point(19, 24);
            this.DeclineBtn1.Margin = new System.Windows.Forms.Padding(4);
            this.DeclineBtn1.Name = "DeclineBtn1";
            this.DeclineBtn1.Size = new System.Drawing.Size(162, 47);
            this.DeclineBtn1.TabIndex = 49;
            this.DeclineBtn1.Text = "button1";
            this.DeclineBtn1.UseVisualStyleBackColor = true;
            this.DeclineBtn1.UseWaitCursor = true;
            this.DeclineBtn1.Visible = false;
            this.DeclineBtn1.Click += new System.EventHandler(this.DeclineBtn1_Click);
            // 
            // AcceptBtn1
            // 
            this.AcceptBtn1.Location = new System.Drawing.Point(32, 33);
            this.AcceptBtn1.Margin = new System.Windows.Forms.Padding(4);
            this.AcceptBtn1.Name = "AcceptBtn1";
            this.AcceptBtn1.Size = new System.Drawing.Size(162, 47);
            this.AcceptBtn1.TabIndex = 48;
            this.AcceptBtn1.Text = "button1";
            this.AcceptBtn1.UseVisualStyleBackColor = true;
            this.AcceptBtn1.UseWaitCursor = true;
            this.AcceptBtn1.Visible = false;
            this.AcceptBtn1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FPTypeLbl
            // 
            this.FPTypeLbl.AutoSize = true;
            this.FPTypeLbl.Location = new System.Drawing.Point(47, 10);
            this.FPTypeLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FPTypeLbl.Name = "FPTypeLbl";
            this.FPTypeLbl.Size = new System.Drawing.Size(45, 19);
            this.FPTypeLbl.TabIndex = 47;
            this.FPTypeLbl.Text = "label3";
            this.FPTypeLbl.UseWaitCursor = true;
            this.FPTypeLbl.Visible = false;
            // 
            // FPNoLbl
            // 
            this.FPNoLbl.AutoSize = true;
            this.FPNoLbl.Location = new System.Drawing.Point(114, 10);
            this.FPNoLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FPNoLbl.Name = "FPNoLbl";
            this.FPNoLbl.Size = new System.Drawing.Size(45, 19);
            this.FPNoLbl.TabIndex = 46;
            this.FPNoLbl.Text = "label2";
            this.FPNoLbl.UseWaitCursor = true;
            this.FPNoLbl.Visible = false;
            // 
            // AcceptBTN
            // 
            this.AcceptBTN.Appearance.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.AcceptBTN.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AcceptBTN.Appearance.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.AcceptBTN.Appearance.Options.UseBackColor = true;
            this.AcceptBTN.Appearance.Options.UseFont = true;
            this.AcceptBTN.Appearance.Options.UseForeColor = true;
            this.AcceptBTN.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.AcceptBTN.ImageOptions.ImageIndex = 0;
            this.AcceptBTN.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.AcceptBTN.ImageOptions.SvgImage = global::DocumentManager.Properties.Resources.CircularTickGreen;
            this.AcceptBTN.Location = new System.Drawing.Point(36, 248);
            this.AcceptBTN.Margin = new System.Windows.Forms.Padding(4);
            this.AcceptBTN.Name = "AcceptBTN";
            this.AcceptBTN.Size = new System.Drawing.Size(120, 40);
            this.AcceptBTN.TabIndex = 45;
            this.AcceptBTN.Text = "Accept     ";
            this.AcceptBTN.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(51, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(155, 184);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pcReport);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(327, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1082, 738);
            this.panel2.TabIndex = 46;
            // 
            // pcReport
            // 
            this.pcReport.BackColor = System.Drawing.Color.Transparent;
            this.pcReport.Buttons = ((FastReport.PreviewButtons)(((((((FastReport.PreviewButtons.Print | FastReport.PreviewButtons.Save) 
            | FastReport.PreviewButtons.Email) 
            | FastReport.PreviewButtons.Find) 
            | FastReport.PreviewButtons.Zoom) 
            | FastReport.PreviewButtons.Outline) 
            | FastReport.PreviewButtons.Navigator)));
            this.pcReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcReport.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pcReport.Location = new System.Drawing.Point(0, 0);
            this.pcReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pcReport.Name = "pcReport";
            this.pcReport.PageOffset = new System.Drawing.Point(10, 10);
            this.pcReport.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pcReport.SaveInitialDirectory = null;
            this.pcReport.Size = new System.Drawing.Size(1082, 738);
            this.pcReport.TabIndex = 2;
            this.pcReport.UIStyle = FastReport.Utils.UIStyle.Office2010Black;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblUerID);
            this.panelControl1.Controls.Add(this.lblSectionMan);
            this.panelControl1.Controls.Add(this.UserLbl);
            this.panelControl1.Controls.Add(this.pictureBox1);
            this.panelControl1.Controls.Add(this.lblNotetype);
            this.panelControl1.Controls.Add(this.FPNoLbl);
            this.panelControl1.Controls.Add(this.FPTypeLbl);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Controls.Add(this.lblSection);
            this.panelControl1.Controls.Add(this.AcceptBtn1);
            this.panelControl1.Controls.Add(this.DeclineBtn1);
            this.panelControl1.Location = new System.Drawing.Point(22, 441);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(246, 121);
            this.panelControl1.TabIndex = 57;
            this.panelControl1.Visible = false;
            // 
            // FPMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1409, 738);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.LookAndFeel.SkinName = "Office 2019 White";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FPMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authorisation";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FPMessage_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label TextLbl;
        public DevExpress.XtraEditors.SimpleButton DeclineBTN;
        private System.Windows.Forms.TextBox RemBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private FastReport.Preview.PreviewControl pcReport;
        public DevExpress.XtraEditors.SimpleButton AcceptBTN;
        public System.Windows.Forms.Label FPTypeLbl;
        public System.Windows.Forms.Label FPNoLbl;
        private System.Windows.Forms.Button AcceptBtn1;
        private System.Windows.Forms.Button DeclineBtn1;
        private System.Windows.Forms.Label UserLbl;
        private System.Windows.Forms.Label lblUerID;
        private System.Windows.Forms.Label Lvllbl;
        public System.Windows.Forms.Label lblNotetype;
        public System.Windows.Forms.Label lblDate;
        public System.Windows.Forms.Label lblSection;
        public System.Windows.Forms.Label lblSectionMan;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}