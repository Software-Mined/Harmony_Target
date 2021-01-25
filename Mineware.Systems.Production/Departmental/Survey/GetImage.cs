using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Configuration;
using FastReport;
using System.IO;
using DevExpress.XtraEditors;

namespace Mineware.Systems.Production.Departmental.Survey
{

    
    /// <summary>
    /// Structure for image type information. Image type information
    /// is available for each supported image format.
    /// </summary>
    internal struct ImageType
    {
        static ImageType[] fmt;					// array of image types
        static int fmtIndex;				// image type index
        internal static string fmtFilter;				// filter for array

        internal ImageFormat format;					// image type format
        internal string extension;				// image type extension
        internal string filter;

        

        // image type filter

        /// <summary>
        /// Creates and initializes and ImageType. An ImageType structure
        /// contains the ImageFormat associated file extension and filter.
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="ext"></param>
        /// <param name="description"></param>
        internal ImageType(ImageFormat fmt, string ext, string description)
        {
            format = fmt;
            extension = ext;
            filter = description + " (*." + extension + ")|*." + extension;
        }

        /// <summary>
        /// Static method to create and initialize an array of ImageType structures.
        /// The resulting array contains an ImageType for each supported image format. 
        /// </summary>
        internal static void InitFormats()
        {
            fmt = new ImageType[10];				// create image type formats
            fmt[0] = new ImageType(ImageFormat.Bmp, "bmp", "Bitmap");
            fmt[1] = new ImageType(ImageFormat.Emf, "emf", "Enhanced Metafile Format");
            fmt[2] = new ImageType(ImageFormat.Exif, "exif", "Exchange Image Format");
            fmt[3] = new ImageType(ImageFormat.Gif, "gif", "Graphics Interchange Format");
            fmt[4] = new ImageType(ImageFormat.Icon, "ico", "Windows ICON Format");
            fmt[5] = new ImageType(ImageFormat.Jpeg, "jpg", "Joint Photographic Experts Group");
            fmt[6] = new ImageType(ImageFormat.MemoryBmp, "mbmp", "Memory Bitmap");
            fmt[7] = new ImageType(ImageFormat.Png, "png", "Portable Network Graphics");
            fmt[8] = new ImageType(ImageFormat.Tiff, "tif", "Tag Image File Format");
            fmt[9] = new ImageType(ImageFormat.Wmf, "wmf", "Windows Metafile Format");

            //	create an overall filter for all image types
            fmtFilter = fmt[7].filter;
            for (int i = 1; i < fmt.Length; i++)
                fmtFilter += "|" + fmt[i].filter;
            fmtIndex = 7;								// set default format
        }

        /// <summary>
        /// Sets the default image format. The default format is specified
        /// as an index into the image format array fmt.
        /// </summary>
        /// <param name="i">index into image format array fmt</param>
        /// <returns></returns>
        internal static ImageType SetImageType(int i)
        {
            if (fmt == null)							// if no format array
                InitFormats();							//   create one

            if (i < 0) i = 0;							// make sure new index not too small
            if (i >= fmt.Length)						// make sure new index not too large
                i = fmt.Length - 1;
            fmtIndex = i;								// set new format index
            return fmt[fmtIndex];						// return selected format
        }
    }


    public partial class GetImage : XtraForm
    {
        private static int m_HDelay = 5;
        private Bitmap capBM = null;
        private bool aspect = false;
        private Control curCtl = null;
        private bool capBMSaved = false;				// capture bitmap not yet saved
        public CubbyStopNotes Cubby;
        //public DiamondDrillfrm Diamond;
        //public ReefBoringNote ReefBoring;
        public ReefBoringNote ReefBoring;
        //public PrePlanningRockEngFrm RockEng;
        //public PrePlanningRockEngFrmMponeng RockEngMP;
        //public PrePlanningActionCapture ActCapt;
        Report theReport = new Report();
        Procedures procs = new Procedures();
        string RepDir = ProductionGlobal.ProductionGlobalTSysSettings._RepDir;
        public GetImage(CubbyStopNotes _Cubby)
        {
            InitializeComponent();

            imageDir = RepDir + @"\SurveyLetters";		// set initial image directory
            curCtl = this.panPic;						// control containing image
            oldTKey = TransparencyKey;					// save old transparency key
            ImageType.InitFormats();					// initialize formats
            saveFileDialog1 = InitSFD(null);            // initialize save dialog
            Cubby = _Cubby;
           
        }

        public GetImage(ReefBoringNote _ReefBoring)
        {
            InitializeComponent();

            imageDir = RepDir + @"\SurveyLetters";		// set initial image directory
            curCtl = this.panPic;						// control containing image
            oldTKey = TransparencyKey;					// save old transparency key
            ImageType.InitFormats();					// initialize formats
            saveFileDialog1 = InitSFD(null);            // initialize save dialog
            ReefBoring = _ReefBoring;

        }

       

        


        

        public static int HideDelay					// user acessible member
        {
            get { return m_HDelay; }
            set
            {
                if (value < 0) m_HDelay = 0;		// delay minimally for other waiting theads to execute
                if (value > 1000) m_HDelay = 1000;	// delay no more than 1 sec
                else m_HDelay = value;	// delay the specified number of milliseconds
            }
        }

        public static Size GetDesktopSize()
        {
            int width = USER32.GetSystemMetrics(USER32.SM_CXSCREEN);	// width of desktop
            int height = USER32.GetSystemMetrics(USER32.SM_CYSCREEN);	// height of desktop
            return new Size(width, height);
        }

        /// <summary>
        /// Captures the desktop to a bitmap image
        /// </summary>
        /// <returns>bitmap image of the desktop</returns>
        public static Bitmap Desktop()
        {
            int width = USER32.GetSystemMetrics(USER32.SM_CXSCREEN);	// width of desktop
            int height = USER32.GetSystemMetrics(USER32.SM_CYSCREEN);	// height of desktop
            IntPtr desktopHWND = USER32.GetDesktopWindow();				// window handle for desktop
            return Window(desktopHWND, 0, 0, width, height);					// return bitmap for desktop
        }												// end method Desktop

        /// <summary>
        /// Captures the desktop work area to a bitmap image
        /// </summary>
        /// <returns>bitmap image of desktop work area</returns>
        public static Bitmap DesktopWA(Control ctl)
        {
            Rectangle wa = Screen.GetWorkingArea(ctl);				// working area of screen
            IntPtr desktopHWND = USER32.GetDesktopWindow();			// window handle for desktop
            return Window(desktopHWND, wa.X, wa.Y, wa.Width, wa.Height);	// return bitmap for desktop
        }

        public static Bitmap Control(System.Windows.Forms.Control ctl)
        {
            return Control(ctl, false, false);			// capture entire control
        }

        private Bitmap Dispose(Bitmap bm)
        {
            if (bm == null)
                return null;
            bm.Dispose();
            return null;
        }

        private void DisposeImages()
        {
            curCtl.BackgroundImage = Dispose(curCtl.BackgroundImage);	// no background image
            capBM = Dispose(capBM);										// no captured bitmap
        }


        	/// <summary>
		/// TestCapture demonstrates image capture.
		/// </summary>
        /// 
        private string imageDir;
        private Color oldTKey;

									// end method Control
        private bool viewportOpen = false;
        public long date = 0;
        string Dates = "";
        string AreaRockEng = "";
        string CrewRockEng = "";
        string WPIDRockEng = "";

        private void OpenViewport()
        {
            ////////////To load Binary to Date/////////////////////
            if (OpenFormtxt.Text == "Cubby")
            {
                if (Cubby.ImageDate.Text == "label16")
                {
                    date = Convert.ToInt64(DateTime.Now.ToBinary());
                }
                else
                { 
                    date = Convert.ToInt64(Cubby.ImageDate.Text); //Convert.ToInt64(DateTime.Now.ToBinary());
                }
                Dates = DateTime.FromBinary(date).ToString();
            }

            if (OpenFormtxt.Text == "ReefBoring")
            {
                date = Convert.ToInt64(ReefBoring.ImageDate.Text);//Convert.ToInt64(DateTime.Now.ToBinary());
                Dates = DateTime.FromBinary(date).ToString();
            }


            



          
            //////////////////////////////////////////////////////

           
            //////////////////////////////////////////////////////

            if (viewportOpen)							// if viewport already open
                return;									//   return

           								//   return

            TransparencyKey = BackColor;				// make client background transparent
            DisposeImages();							// set background & capture bm to null
            panPic.Visible = false;						// disappear picture panel
            viewportOpen = true;						// viewport is now open
            Refresh();									// refresh client area
           
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
       

        private void GetImage_Load(object sender, EventArgs e)
        {
            //TransparencyKey = BackColor;
            OpenViewport();
        }
        

        private void aaaaaaaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (aaaaaaaToolStripMenuItem.Text == "Set Image")
           // {
                CaptureView();
           // }
           // else
           // {
                SaveImage();
                this.Close();
                //loadReport();
                if (OpenFormtxt.Text == "Cubby")
                {
                    if (Cubby.noteType == "CN")
                    {
                        Cubby.LoadEmptyCubby();
                    }

                    if (Cubby.noteType == "SN")
                    {
                        Cubby.LoadStopeNote();
                    }
                }

               


                if (OpenFormtxt.Text == "ReefBoring")
                {
                    ReefBoring.LoadEmptyCubby();
                }


          //  }
        }

        public void loadReport()
        {
           

            //MWDataManager.clsDataAccess _dbManEmptyCubby = new MWDataManager.clsDataAccess();
            //_dbManEmptyCubby.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];

            //_dbManEmptyCubby.SqlStatement = "Select '" + SysSettings.RepDir + @"\" + SavedPath +"' ";


            //_dbManEmptyCubby.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            //_dbManEmptyCubby.queryReturnType = MWDataManager.ReturnType.DataTable;
            //_dbManEmptyCubby.ResultsTableName = "Image";
            //_dbManEmptyCubby.ExecuteInstruction();


            //DataSet dsEmpty = new DataSet();
            //// DataSet dsGraph = new DataSet();
            //dsEmpty.Tables.Add(_dbManEmptyCubby.ResultsDataTable);

            //theReport.RegisterData(dsEmpty);
            //////MessageBox.Show(GraphType);


            //theReport.Load("ImageRep.frx");
            //theReport.Design();



            //Cubby.pcReport.Clear();
            //theReport.Prepare();
            //theReport.Preview = Cubby.pcReport;
            //theReport.ShowPrepared();
        }

        private void CaptureView()
        {

           // if (aaaaaaaToolStripMenuItem.Text == "Set Image")
            //{
                

                //viewportOpen = false;						// viewport is closed
                TransparencyKey = Color.Red; 				// restore old transparency key
                //timer1.Enabled = true;
                panPic.Visible = true;						// show picture panel
                Refresh();

                capBM = Control(this, true, true);

                PicInCtl(panPic, false);						// place new image in current control
                capBMSaved = false;

             //   aaaaaaaToolStripMenuItem.Text = "Save Image";
           // }
        }


        private SaveFileDialog InitSFD(SaveFileDialog s)
        {
            if (s != null)								// if save dialog exists
                return s;								//   return
            s = new SaveFileDialog();					// create save file dialog
            s.InitialDirectory = imageDir;				// initial directory
            s.Title = "Save Captured Image";			// title
            s.CheckPathExists = true;					// make sure path exists

            //	create filter
            s.Filter = ImageType.fmtFilter;				// use filter for all formats
            return s;
        }

        string SavedPath = "";


        public void SaveImage()
        {

            
            if (OpenFormtxt.Text == "Diamond"  || OpenFormtxt.Text == "Diamond2")
            {
                if (SysSettings.Banner == "Mponeng")
                    imageDir = imageDir + @"\Mponeng" + @"\DrillingLetters";


                 

                System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(imageDir);
                // Take a snapshot of the file system.            

                IEnumerable<System.IO.FileInfo> list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                             

                // DialogResult dr = saveFileDialog1.ShowDialog(this);
                //if (dr != DialogResult.OK)					// if not ok
                //return;									//   forget it		

                string fn = "";


               // SavedPath = date + "a.png";

                SavedPath = "a.png";

                fn = imageDir + @"\" + SavedPath;//saveFileDialog1.FileName;	//imageDir + @"\" + DateTime.Now.ToShortDateString(); //

                foreach (FileInfo fi in list1) // Checks if File Exists
                {
                    if (fi.Name == SavedPath)
                    {
                        if (OpenFormtxt.Text == "Diamond2")
                        {
                            DDLetterlabel.Text = DDLetterlabel.Text + "Diamond2";

                            fn = imageDir + @"\" + DDLetterlabel.Text + ".png";
                        }
                        else
                        {
                            fn = imageDir + @"\" + DDLetterlabel.Text + ".png";
                            // fn = imageDir + @"\a.png";
                        }


                       
                    }
                }



                //SavedPath = fn;
                ImageType it = ImageType.SetImageType(saveFileDialog1.FilterIndex);	// set default image type to selected filter format
                //Cubby.ImageDate.Text = Convert.ToInt64(date).ToString();						
                capBM.Save(fn, it.format);				//   use generic image save
                capBMSaved = true;
            }
            else
            {
                System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(imageDir);
                // Take a snapshot of the file system.
                IEnumerable<System.IO.FileInfo> list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);


                // DialogResult dr = saveFileDialog1.ShowDialog(this);
                //if (dr != DialogResult.OK)					// if not ok
                //return;									//   forget it		

                string fn = "";


                SavedPath = date + ".png";

                fn = imageDir + @"\" + SavedPath;//saveFileDialog1.FileName;	//imageDir + @"\" + DateTime.Now.ToShortDateString(); //

                foreach (FileInfo fi in list1) // Checks if File Exists
                {
                    if (fi.Name == SavedPath)
                    {
                        fn = imageDir + @"\" + date + ".png";
                    }
                }



                //SavedPath = fn;
                ImageType it = ImageType.SetImageType(saveFileDialog1.FilterIndex);	// set default image type to selected filter format
                //Cubby.ImageDate.Text = Convert.ToInt64(date).ToString();

                if (OpenFormtxt.Text == "ReefBoring")
                    ReefBoring.PicLbl.Text = fn;
				
		
                capBM.Save(fn, it.format);				//   use generic image save
                capBMSaved = true;

            }
           					
        }

        private void PicInCtl(Control c, bool oldImage)
        {

            if (capBM == null)			// if viewport open or no bitmap
                return;	

            Control pc = panPic;
            //Control pc = c.Parent;						// get parent container control
            if (pc == null) 
                return;	
            
            

            //	update size of control's client area
            Size cs = pc.ClientSize;					// assume parent's entire client area
            if ((cs.Width == 0) || (cs.Height == 0))		// if zero width or height
                return;									//   return

            //	calc control's client area to retain image aspect
            if (aspect)									// if retaining aspect
            {
                int mxW = cs.Width;					//   get max width
                int mxH = cs.Height;				//   get max height
                int asW =							//   calc aspect width from max height
                    (capBM.Width * mxH) / capBM.Height;
                int asH =							//   calc aspect height from max width
                    (capBM.Height * mxW) / capBM.Width;

                if (asW <= mxW)							//   if max height yields in range width
                {
                    cs.Width = asW;						//     use aspect width
                    cs.Height = mxH;					//     use max height
                }
                else									//   if max width yields in range height
                {
                    cs.Width = mxW;						//     use max width
                    cs.Height = asH;					//     use aspect height
                }
            }

            //	check existing image size
            //c.ClientSize = cs;							// set client area for control
            //if (oldImage && (c.BackgroundImage != null))		// if old background image exists
            //{
            //    if ((c.BackgroundImage.Width == cs.Width)	//   if image already client area size
            //        && (c.BackgroundImage.Height == cs.Height))
            //    {
            //        return;										//     return
            //    }
            //}

            //	update image size for control's new client area
            Dispose(c.BackgroundImage);					// dispose of previous background image
            c.BackgroundImage = new Bitmap(capBM, cs);	// resize background image
        }

        private Image Dispose(Image img)
        {
            if (img == null)
                return null;
            img.Dispose();
            return null;
        }

        public static Bitmap Control(System.Windows.Forms.Control ctl, bool client, bool under)
        {
            Bitmap bmp;							// capture bitmap
            Rectangle ctlR;							// capture area rectangle in control coordinates
            Rectangle scrR;							// capture area rectangle in screen coordinates

            //	get capture rectangle in control
            //	coordinates and in screen coordinates
            if (client)									// if capturing client area
            {
                ctlR = ctl.ClientRectangle;				//   get rectangle in control coordinates
                scrR = ctl.RectangleToScreen(ctlR);		//   get rectangle in screen coordinates
            }
            else										// if capturing entire control
            {
                scrR = ctl.Bounds;						//   get rectangle in parent coordinates
                if (ctl.Parent != null)								//   if parent exists
                    scrR = ctl.Parent.RectangleToScreen(scrR);		//     map to screen coordinates
                ctlR = ctl.RectangleToClient(scrR);					//   get rectangle in control coordinates
            }

            //	capture an area under the control
            if (under)									// if capture area is under control
            {
                bool prvV = ctl.Visible;				//   save control visibility
                if (prvV)								//   if control visible
                {
                    ctl.Visible = false;				//     make control invisible
                    Thread.Sleep(m_HDelay);				//     allow time for control to become invisible
                    //     prior to image capture
                }

                //	Capture the bitmap using desktop window handle and screen coordinates
                //	for the capture area. Note, the control window handle can NOT be used
                //  for capturing an area under the control.
                IntPtr desktopHWND = USER32.GetDesktopWindow();	// get window handle for desktop
                bmp = Window(desktopHWND, scrR);						// get bitmap for capture area under control
                if (ctl.Visible != prvV)				//   if control visibility was changed
                    ctl.Visible = prvV;					//     restore previous visibility
            }

            //	capture an area on the control
            else										// if capture area not under control
            {
                //	Capture the bitmap using control window handle and control coordinates
                //	for capture area.
                bmp = Window(ctl.Handle, ctlR);			//   get bitmap using control window handle
            }
            return bmp;									// return requested bitmap
        }


        public static Bitmap Window(IntPtr handle, Rectangle r)
        {
            return Window(handle, r.X, r.Y, r.Width, r.Height);
        }

        public static Bitmap Window(IntPtr wndHWND, int x, int y, int width, int height)
        {
            IntPtr wndHDC = USER32.GetDC(wndHWND);		// get context for window 

            //	create compatibile capture context and bitmap
            IntPtr capHDC = GDI32.CreateCompatibleDC(wndHDC);
            IntPtr capBMP = GDI32.CreateCompatibleBitmap(wndHDC, width, height);

            //	make sure bitmap non-zero
            if (capBMP == IntPtr.Zero)					// if no compatible bitmap
            {
                USER32.ReleaseDC(wndHWND, wndHDC);		//   release window context
                GDI32.DeleteDC(capHDC);					//   delete capture context
                return null;							//   return null bitmap
            }

            //	select compatible bitmap in compatible context
            //	copy window context to compatible context
            //  select previous bitmap back into compatible context
            IntPtr prvHDC = (IntPtr)GDI32.SelectObject(capHDC, capBMP);
            GDI32.BitBlt(capHDC, 0, 0, width, height, wndHDC, x, y, GDI32.SRCCOPY);
            GDI32.SelectObject(capHDC, prvHDC);

            //	create GDI+ bitmap for window
            Bitmap bmp = System.Drawing.Image.FromHbitmap(capBMP);

            //	release window and capture resources
            USER32.ReleaseDC(wndHWND, wndHDC);			// release window context
            GDI32.DeleteDC(capHDC);						// delete capture context
            GDI32.DeleteObject(capBMP);					// delete capture bitmap

            //	return bitmap image to user
            return bmp;									// return bitmap
        }


        public class USER32
        {
            public const int SM_CXSCREEN = 0;
            public const int SM_CYSCREEN = 1;

            [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll", EntryPoint = "GetDC")]
            public static extern IntPtr GetDC(IntPtr ptr);

            [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
            public static extern int GetSystemMetrics(int abc);

            [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
            public static extern IntPtr GetWindowDC(Int32 ptr);

            [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        }

        public class GDI32
        {
            public const int SRCCOPY = 13369376;

            [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
            public static extern IntPtr DeleteDC(IntPtr hDc);

            [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
            public static extern IntPtr DeleteObject(IntPtr hDc);

            [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
            public static extern bool BitBlt(IntPtr hdcDest, int xDest,
                int yDest, int wDest, int hDest, IntPtr hdcSource,
                int xSrc, int ySrc, int RasterOp);

            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc,
                int nWidth, int nHeight);

            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

            [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        }

        private void GetImage_SizeChanged(object sender, EventArgs e)
        {
            PicInCtl(curCtl, true);
        }

        private void GetImage_Paint(object sender, PaintEventArgs e)
        {
            //if (crossHairs && viewportOpen)								// if viewport open and doing crosshairs
           // {
                int rad = 20;									//   set radius
                Rectangle r = ClientRectangle;						//   get client area rectangle
                Point c = new Point(r.X + r.Width / 2,			//   find its center point
                    r.Y + r.Height / 2);
                Rectangle cr = new Rectangle(c.X - rad, c.Y - rad		//   create rectange about center
                    , 2 * rad, 2 * rad);								//   with width & height twice radius
                Pen p = new Pen(Color.Black);
                e.Graphics.DrawEllipse(p, cr);							//   draw a circular scope
                e.Graphics.DrawLine(p, c.X - rad, c.Y, c.X + rad, c.Y);		//   draw horizontal crosshair
                e.Graphics.DrawLine(p, c.X, c.Y - rad, c.X, c.Y + rad);		//   draw vertical crosshair
           // }
        }

        private void DDLetterlabel_Click(object sender, EventArgs e)
        {

        }
    }
}
