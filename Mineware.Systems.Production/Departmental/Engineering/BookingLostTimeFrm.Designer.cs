namespace Mineware.Systems.Production.Departmental.Engineering
{
    partial class BookingLostTimeFrm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.EquiNo = new System.Windows.Forms.Label();
            this.dateLbl = new System.Windows.Forms.Label();
            this.Shiftlbl = new System.Windows.Forms.Label();
            this.LostTimeGrid = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FilterTxt = new System.Windows.Forms.TextBox();
            this.StartTxt = new System.Windows.Forms.TextBox();
            this.EndTxt = new System.Windows.Forms.TextBox();
            this.DurationTxt = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Changelbl = new System.Windows.Forms.Label();
            this.ABSLbl = new System.Windows.Forms.Label();
            this.CloseBtn = new DevExpress.XtraEditors.SimpleButton();
            this.SaveBtn = new DevExpress.XtraEditors.SimpleButton();
            this.label6 = new System.Windows.Forms.Label();
            this.Remarkstxt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.ProbGroupGrid = new System.Windows.Forms.DataGridView();
            this.ProbGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.LostTimeGrid)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProbGroupGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProbGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // EquiNo
            // 
            this.EquiNo.AutoSize = true;
            this.EquiNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EquiNo.ForeColor = System.Drawing.Color.Black;
            this.EquiNo.Location = new System.Drawing.Point(22, 18);
            this.EquiNo.Name = "EquiNo";
            this.EquiNo.Size = new System.Drawing.Size(76, 16);
            this.EquiNo.TabIndex = 60;
            this.EquiNo.Text = "Drill Rig 2";
            // 
            // dateLbl
            // 
            this.dateLbl.AutoSize = true;
            this.dateLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateLbl.ForeColor = System.Drawing.Color.Black;
            this.dateLbl.Location = new System.Drawing.Point(131, 18);
            this.dateLbl.Name = "dateLbl";
            this.dateLbl.Size = new System.Drawing.Size(39, 16);
            this.dateLbl.TabIndex = 61;
            this.dateLbl.Text = "date";
            // 
            // Shiftlbl
            // 
            this.Shiftlbl.AutoSize = true;
            this.Shiftlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Shiftlbl.ForeColor = System.Drawing.Color.Black;
            this.Shiftlbl.Location = new System.Drawing.Point(254, 18);
            this.Shiftlbl.Name = "Shiftlbl";
            this.Shiftlbl.Size = new System.Drawing.Size(63, 16);
            this.Shiftlbl.TabIndex = 62;
            this.Shiftlbl.Text = "Morning";
            // 
            // LostTimeGrid
            // 
            this.LostTimeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LostTimeGrid.Location = new System.Drawing.Point(25, 37);
            this.LostTimeGrid.Name = "LostTimeGrid";
            this.LostTimeGrid.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LostTimeGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.LostTimeGrid.RowHeadersWidth = 30;
            this.LostTimeGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Red;
            this.LostTimeGrid.Size = new System.Drawing.Size(1019, 73);
            this.LostTimeGrid.TabIndex = 63;
            this.LostTimeGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.LostTimeGrid_CellMouseDown);
            this.LostTimeGrid.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.LostTimeGrid_CellMouseMove);
            this.LostTimeGrid.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.LostTimeGrid_CellMouseUp);
            this.LostTimeGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.LostTimeGrid_CellPainting);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 199);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 64;
            this.label2.Text = "Start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 65;
            this.label3.Text = "End";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 66;
            this.label4.Text = "Duration";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(445, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 67;
            this.label5.Text = "Filter";
            // 
            // FilterTxt
            // 
            this.FilterTxt.Location = new System.Drawing.Point(448, 215);
            this.FilterTxt.Name = "FilterTxt";
            this.FilterTxt.Size = new System.Drawing.Size(235, 20);
            this.FilterTxt.TabIndex = 68;
            this.FilterTxt.TextChanged += new System.EventHandler(this.FilterTxt_TextChanged);
            // 
            // StartTxt
            // 
            this.StartTxt.Location = new System.Drawing.Point(25, 215);
            this.StartTxt.Name = "StartTxt";
            this.StartTxt.Size = new System.Drawing.Size(73, 20);
            this.StartTxt.TabIndex = 69;
            // 
            // EndTxt
            // 
            this.EndTxt.Location = new System.Drawing.Point(134, 215);
            this.EndTxt.Name = "EndTxt";
            this.EndTxt.Size = new System.Drawing.Size(73, 20);
            this.EndTxt.TabIndex = 70;
            // 
            // DurationTxt
            // 
            this.DurationTxt.Location = new System.Drawing.Point(244, 215);
            this.DurationTxt.Name = "DurationTxt";
            this.DurationTxt.Size = new System.Drawing.Size(73, 20);
            this.DurationTxt.TabIndex = 71;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Changelbl);
            this.panel1.Controls.Add(this.ABSLbl);
            this.panel1.Controls.Add(this.CloseBtn);
            this.panel1.Controls.Add(this.SaveBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 563);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1075, 88);
            this.panel1.TabIndex = 74;
            // 
            // Changelbl
            // 
            this.Changelbl.AutoSize = true;
            this.Changelbl.Location = new System.Drawing.Point(24, 48);
            this.Changelbl.Name = "Changelbl";
            this.Changelbl.Size = new System.Drawing.Size(41, 13);
            this.Changelbl.TabIndex = 37;
            this.Changelbl.Text = "label16";
            this.Changelbl.Visible = false;
            // 
            // ABSLbl
            // 
            this.ABSLbl.AutoSize = true;
            this.ABSLbl.Location = new System.Drawing.Point(24, 15);
            this.ABSLbl.Name = "ABSLbl";
            this.ABSLbl.Size = new System.Drawing.Size(41, 13);
            this.ABSLbl.TabIndex = 8;
            this.ABSLbl.Text = "label16";
            this.ABSLbl.Visible = false;
            // 
            // CloseBtn
            // 
            this.CloseBtn.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CloseBtn.Appearance.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.CloseBtn.Appearance.Options.UseBackColor = true;
            this.CloseBtn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            //            this.CloseBtn.Image = global::PAS.Properties.Resources.Close;
            this.CloseBtn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.CloseBtn.Location = new System.Drawing.Point(689, 19);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(136, 48);
            this.CloseBtn.TabIndex = 36;
            this.CloseBtn.Text = "&Close                 ";
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.SaveBtn.Appearance.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.SaveBtn.Appearance.Options.UseBackColor = true;
            this.SaveBtn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.SaveBtn.Enabled = false;
            //this.SaveBtn.Image = global::PAS.Properties.Resources.Save;
            this.SaveBtn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.SaveBtn.Location = new System.Drawing.Point(466, 19);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(136, 48);
            this.SaveBtn.TabIndex = 35;
            this.SaveBtn.Text = "&Save                 ";
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(121, 515);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 75;
            this.label6.Text = "Remarks";
            // 
            // Remarkstxt
            // 
            this.Remarkstxt.Location = new System.Drawing.Point(179, 512);
            this.Remarkstxt.Name = "Remarkstxt";
            this.Remarkstxt.Size = new System.Drawing.Size(694, 20);
            this.Remarkstxt.TabIndex = 76;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(385, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 77;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(528, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 78;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(339, 196);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(63, 20);
            this.textBox3.TabIndex = 79;
            this.textBox3.Visible = false;
            // 
            // ProbGroupGrid
            // 
            this.ProbGroupGrid.AllowUserToAddRows = false;
            this.ProbGroupGrid.AllowUserToDeleteRows = false;
            this.ProbGroupGrid.BackgroundColor = System.Drawing.Color.White;
            this.ProbGroupGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProbGroupGrid.Location = new System.Drawing.Point(25, 250);
            this.ProbGroupGrid.Name = "ProbGroupGrid";
            this.ProbGroupGrid.ReadOnly = true;
            this.ProbGroupGrid.Size = new System.Drawing.Size(245, 233);
            this.ProbGroupGrid.TabIndex = 80;
            this.ProbGroupGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProbGroupGrid_CellClick);
            this.ProbGroupGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProbGroupGrid_CellClick);
            // 
            // ProbGrid
            // 
            this.ProbGrid.AllowUserToAddRows = false;
            this.ProbGrid.AllowUserToDeleteRows = false;
            this.ProbGrid.BackgroundColor = System.Drawing.Color.White;
            this.ProbGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProbGrid.Location = new System.Drawing.Point(448, 250);
            this.ProbGrid.Name = "ProbGrid";
            this.ProbGrid.ReadOnly = true;
            this.ProbGrid.Size = new System.Drawing.Size(460, 233);
            this.ProbGrid.TabIndex = 81;
            this.ProbGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProbGrid_CellClick);
            // 
            // BookingLostTimeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 651);
            this.Controls.Add(this.ProbGrid);
            this.Controls.Add(this.ProbGroupGrid);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Remarkstxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.DurationTxt);
            this.Controls.Add(this.EndTxt);
            this.Controls.Add(this.StartTxt);
            this.Controls.Add(this.FilterTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LostTimeGrid);
            this.Controls.Add(this.Shiftlbl);
            this.Controls.Add(this.dateLbl);
            this.Controls.Add(this.EquiNo);
            this.Name = "BookingLostTimeFrm";
            this.Text = "Booking Lost Time";
            this.Load += new System.EventHandler(this.BookingLostTimeFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LostTimeGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProbGroupGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProbGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView LostTimeGrid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox FilterTxt;
        private System.Windows.Forms.TextBox StartTxt;
        private System.Windows.Forms.TextBox EndTxt;
        private System.Windows.Forms.TextBox DurationTxt;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label Changelbl;
        public System.Windows.Forms.Label ABSLbl;
        private DevExpress.XtraEditors.SimpleButton CloseBtn;
        private DevExpress.XtraEditors.SimpleButton SaveBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Remarkstxt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridView ProbGroupGrid;
        private System.Windows.Forms.DataGridView ProbGrid;
        public System.Windows.Forms.Label EquiNo;
        public System.Windows.Forms.Label dateLbl;
        public System.Windows.Forms.Label Shiftlbl;
    }
}