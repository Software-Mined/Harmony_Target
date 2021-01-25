using System;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Logistics_Management
{
    public partial class frmABS : DevExpress.XtraEditors.XtraForm
    {
        public frmABS()
        {
            InitializeComponent();
        }


        public frmBookingServices BookingServFrm;
        public frmABS(frmBookingServices _BookingServFrm)
        {
            InitializeComponent();
            BookingServFrm = _BookingServFrm;
        }


        #region Public Variables
        public string Rowlabel;
        public string Actlabel35;
        public string PicLbl;
        public string Catlabel35;
        #endregion





        public void loadPic()
        {
            pictureBox7.Image = picCross.Image;
            pictureBox8.Image = picCross.Image;
            pictureBox9.Image = picCross.Image;
            pictureBox10.Image = picCross.Image;
            pictureBox11.Image = picCross.Image;
            pictureBox12.Image = picCross.Image;
            pictureBox13.Image = picCross.Image;
            pictureBox14.Image = picCross.Image;
            pictureBox15.Image = picCross.Image;
            pictureBox16.Image = picCross.Image;
            pictureBox17.Image = picCross.Image;

            pictureBox18.Image = picCross.Image;
            pictureBox19.Image = picCross.Image;
            pictureBox20.Image = picCross.Image;
            pictureBox21.Image = picCross.Image;
            pictureBox22.Image = picCross.Image;
            pictureBox23.Image = picCross.Image;
            pictureBox24.Image = picCross.Image;

            if (PicLbl == "1")
                pictureBox7.Image = picCheck.Image;
            if (PicLbl == "2")
                pictureBox8.Image = picCheck.Image;
            if (PicLbl == "3")
                pictureBox9.Image = picCheck.Image;
            if (PicLbl == "4")
                pictureBox10.Image = picCheck.Image;
            if (PicLbl == "5")
                pictureBox11.Image = picCheck.Image;
            if (PicLbl == "6")
                pictureBox12.Image = picCheck.Image;
            if (PicLbl == "7")
                pictureBox13.Image = picCheck.Image;
            if (PicLbl == "8")
                pictureBox14.Image = picCheck.Image;
            if (PicLbl == "9")
                pictureBox15.Image = picCheck.Image;
            if (PicLbl == "10")
                pictureBox16.Image = picCheck.Image;
            if (PicLbl == "11")
                pictureBox17.Image = picCheck.Image;

            if (PicLbl == "12")
                pictureBox18.Image = picCheck.Image;
            if (PicLbl == "13")
                pictureBox19.Image = picCheck.Image;
            if (PicLbl == "14")
                pictureBox20.Image = picCheck.Image;
            if (PicLbl == "15")
                pictureBox21.Image = picCheck.Image;
            if (PicLbl == "16")
                pictureBox22.Image = picCheck.Image;
            if (PicLbl == "17")
                pictureBox23.Image = picCheck.Image;
            if (PicLbl == "18")
                pictureBox24.Image = picCheck.Image;

        }

        private void frmABS_Load(object sender, EventArgs e)
        {
            if (Catlabel35 == "A")
            {
                pictureBox4.Image = picCheck.Image;
                pictureBox5.Image = picCross.Image;
                pictureBox6.Image = picCross.Image;

                label43.Visible = false;
                PrecPicBox25.Visible = false;
            }

            if (Catlabel35 == "S")
            {
                pictureBox4.Image = picCross.Image;
                pictureBox5.Image = picCross.Image;
                pictureBox6.Image = picCheck.Image;

                //Spanel.Location = new Point(10, 350);
                // Spanel.Size = new Size(540, 285);
                Spanel.Visible = true;
            }

            if (Catlabel35 == "PS")
            {
                pictureBox4.Image = picCross.Image;
                pictureBox5.Image = picCross.Image;
                pictureBox6.Image = picCheck.Image;

                PrecPicBox25.Image = picCheck.Image;

                //Spanel.Location = new Point(10, 350);
                //Spanel.Size = new Size(540, 285);
                Spanel.Visible = true;
            }

            if (Catlabel35 == "B")
            {
                pictureBox4.Image = picCross.Image;
                pictureBox5.Image = picCheck.Image;
                pictureBox6.Image = picCross.Image;

                //Bpanel.Location = new Point(10, 350);
                // Bpanel.Size = new Size(540, 239);
                Bpanel.Visible = true;
            }

            if (Catlabel35 == "PB")
            {
                pictureBox4.Image = picCross.Image;
                pictureBox5.Image = picCheck.Image;
                pictureBox6.Image = picCross.Image;

                PrecPicBox25.Image = picCheck.Image;

                //Bpanel.Location = new Point(10, 350);
                // Bpanel.Size = new Size(540, 239);
                Bpanel.Visible = true;
            }
        }

        private void PrecPicBox25_Click(object sender, EventArgs e)
        {
            if (PrecLbl.Text == "Y")
            {
                PrecLbl.Text = "N";
            }
            else
            {
                PrecLbl.Text = "Y";
            }

            if (PrecLbl.Text == "Y")
            {
                PrecPicBox25.Image = picCheck.Image;
            }
            else
            {
                PrecPicBox25.Image = picCross.Image;
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            PicLbl = "1";
            loadPic();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            PicLbl = "2";
            loadPic();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            PicLbl = "3";
            loadPic();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            PicLbl = "4";
            loadPic();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            PicLbl = "5";
            loadPic();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            PicLbl = "6";
            loadPic();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            PicLbl = "7";
            loadPic();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            PicLbl = "8";
            loadPic();
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            PicLbl = "9";
            loadPic();
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            PicLbl = "10";
            loadPic();
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            PicLbl = "11";
            loadPic();
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            PicLbl = "12";
            loadPic();
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            PicLbl = "13";
            loadPic();
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            PicLbl = "14";
            loadPic();
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            PicLbl = "15";
            loadPic();
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            PicLbl = "16";
            loadPic();
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            PicLbl = "17";
            loadPic();
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            PicLbl = "18";
            loadPic();
        }

        private void btnSaveAct_Click(object sender, EventArgs e)
        {
            if (Catlabel35 != "A")
            {
                if (PicLbl == string.Empty)
                {
                    MessageBox.Show("A ABS Classifiction has not been selected.", "Unselected Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (Catlabel35 == "S" || Catlabel35 == "PS")
            {
                if (Convert.ToInt64(PicLbl) > 11)
                {
                    MessageBox.Show("A ABS Classifiction has not been selected.", "Unselected Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else
            {
                if (Catlabel35 != "A")
                {
                    if (Convert.ToInt64(PicLbl) < 12)
                    {
                        MessageBox.Show("A ABS Classifiction has not been selected.", "Unselected Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

            }

            if (pictureBox6.Image == picCheck.Image)
            {
                if (Commentstxt.Text == string.Empty)
                {
                    MessageBox.Show("When ABS is set to S notes are required.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Commentstxt.Focus();
                    return;
                }
            }

            string ABSCode;
            string ABSPCode = string.Empty;
            string ABSNote = Commentstxt.Text.ToString();

            BookingServFrm.gvBooking.SetRowCellValue(Convert.ToInt32(Rowlabel), BookingServFrm.gvBooking.Columns["ABSNote"], Commentstxt.Text.ToString());

            if (pictureBox4.Image == picCheck.Image)
            {
                BookingServFrm.gvBooking.SetRowCellValue(Convert.ToInt32(Rowlabel), BookingServFrm.gvBooking.Columns["ExtraABSP"], "A");
                ABSCode = "A";
            }

            if (pictureBox6.Image == picCheck.Image)
            {
                BookingServFrm.gvBooking.SetRowCellValue(Convert.ToInt32(Rowlabel), BookingServFrm.gvBooking.Columns["ExtraABSP"], "S");
                ABSCode = "S";
                if (PrecPicBox25.Image == picCheck.Image)
                {
                    BookingServFrm.gvBooking.SetRowCellValue(Convert.ToInt32(Rowlabel), BookingServFrm.gvBooking.Columns["ExtraABSP"], "PS");
                    ABSPCode = "PS";
                }
            }
            else
            {
                BookingServFrm.gvBooking.SetRowCellValue(Convert.ToInt32(Rowlabel), BookingServFrm.gvBooking.Columns["ExtraABSP"], "B");
                ABSCode = "B";
                if (PrecPicBox25.Image == picCheck.Image)
                {
                    BookingServFrm.gvBooking.SetRowCellValue(Convert.ToInt32(Rowlabel), BookingServFrm.gvBooking.Columns["ExtraABSP"], "PB");
                    ABSPCode = "PB";
                }
            }

            for (int i = 0; i < BookingServFrm.gvBooking.RowCount; i++)
            {
                if (BookingServFrm.gvBooking.GetRowCellValue(i, BookingServFrm.gvBooking.Columns["workplace"]).ToString() == WPlabel37.Text)
                {
                    BookingServFrm.gvBooking.SetRowCellValue(i, BookingServFrm.gvBooking.Columns["ExtraABSP"], ABSCode);
                    BookingServFrm.gvBooking.SetRowCellValue(i, BookingServFrm.gvBooking.Columns["WPStatus"], ABSCode);
                    BookingServFrm.gvBooking.SetRowCellValue(i, BookingServFrm.gvBooking.Columns["ABSP"], ABSPCode);
                    BookingServFrm.gvBooking.SetRowCellValue(i, BookingServFrm.gvBooking.Columns["ABSNote"], ABSNote);
                }
            }

            BookingServFrm.editDone = "Y";

            Close();
        }
    }
}