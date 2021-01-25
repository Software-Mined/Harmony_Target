namespace Mineware.Systems.DocumentManager
{
    partial class MessageFrm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageFrm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.TextLbl = new System.Windows.Forms.Label();
            this.InfoPic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.InfoPic)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 900;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TextLbl
            // 
            this.TextLbl.AutoSize = true;
            this.TextLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLbl.ForeColor = System.Drawing.Color.Red;
            this.TextLbl.Location = new System.Drawing.Point(67, 31);
            this.TextLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TextLbl.Name = "TextLbl";
            this.TextLbl.Size = new System.Drawing.Size(148, 25);
            this.TextLbl.TabIndex = 45;
            this.TextLbl.Text = "Record Saved";
            // 
            // InfoPic
            // 
            this.InfoPic.Location = new System.Drawing.Point(16, 15);
            this.InfoPic.Margin = new System.Windows.Forms.Padding(4);
            this.InfoPic.Name = "InfoPic";
            this.InfoPic.Size = new System.Drawing.Size(43, 41);
            this.InfoPic.TabIndex = 46;
            this.InfoPic.TabStop = false;
            this.InfoPic.Visible = false;
            // 
            // MessageFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(705, 159);
            this.Controls.Add(this.InfoPic);
            this.Controls.Add(this.TextLbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MessageFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MessageFrm";
            ((System.ComponentModel.ISupportInitialize)(this.InfoPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.PictureBox InfoPic;
        private System.Windows.Forms.Label TextLbl;
    }
}