using Microsoft.VisualBasic.PowerPacks;
using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Mineware.Systems.Production.OreflowDiagram
{
    public class clsBackfillDrawing
    {

        public static DataTable dt = new DataTable();

        public static int BFObjX = 0;
        public static int BFObjY = 0;
        public static int BFObjX2 = 0;
        public static int BFObjY2 = 0;
        public static string BFObjID = string.Empty;

        public static float x2 = -1;
        public static float y2 = -1;
        public static int shaftx;

        public static string type;

        public static string Name;
        public static string UndrawnName;
        public static int id;
        public static string pop = string.Empty;
        public static Boolean isEdit = false;
        public const int HitTestDelta = 15;
        public static bool dragStartPoint;
        public static bool dragEndPoint;
        public static int startx = 0;
        public static int starty = 0;
        public static Boolean fdragging = false;
        public static System.Drawing.Point oldStartPoint;
        public static System.Drawing.Point oldEndPoint;
        public static System.Drawing.Point startpoint;
        public static System.Drawing.Point endpoint;
        public static int oldmousey = 0;
        public static int oldmousex = 0;

        public static Boolean dragimage = false;

        public static DevExpress.XtraEditors.PanelControl GiantP;
        public static Bitmap GiantB;
        public static Image GiantI;

        public static System.Windows.Forms.PictureBox pcar1 = new System.Windows.Forms.PictureBox();
        public static System.Windows.Forms.PictureBox pcar2 = new System.Windows.Forms.PictureBox();
        public static System.Windows.Forms.PictureBox pcar3 = new System.Windows.Forms.PictureBox();
        public static System.Windows.Forms.PictureBox pcar4 = new System.Windows.Forms.PictureBox();
        public static System.Windows.Forms.PictureBox pcar5 = new System.Windows.Forms.PictureBox();

        public static Microsoft.VisualBasic.PowerPacks.ShapeContainer levels = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
        public static Microsoft.VisualBasic.PowerPacks.ShapeContainer shaft = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
        public static Microsoft.VisualBasic.PowerPacks.ShapeContainer subShaft = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
        public static Microsoft.VisualBasic.PowerPacks.ShapeContainer trams = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();

        public static Microsoft.VisualBasic.PowerPacks.ShapeContainer mills = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
        public static Microsoft.VisualBasic.PowerPacks.ShapeContainer boxholes = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
        public static Microsoft.VisualBasic.PowerPacks.ShapeContainer orebins = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
        public static Microsoft.VisualBasic.PowerPacks.ShapeContainer orepass = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
        public static Microsoft.VisualBasic.PowerPacks.ShapeContainer HeadGear = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();

        public static string _Systemtag;
        public static string _Userconnection;
        public static string _DesignType;
        public static clsCoordinatesBinding.ObjectCoordinates ObjCoordinates;
        public static string _IsGragphics;

        public static Boolean IsEdit
        {
            get { return isEdit; }
            set { isEdit = value; }
        }

        public static void DrawSurfaceLine(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing, int linelength)
        {
            GiantP = panel;
            GiantB = drawing;
            Graphics g = Graphics.FromImage(drawing);
            ShapeContainer s = new ShapeContainer();
            LineShape ls = new LineShape(s);
            ls.BorderColor = Color.Black;
            ls.BorderWidth = 5;
            ls.X1 = -5;
            ls.X2 = panel.Width + 800;
            ls.Y1 = 130;
            ls.Y2 = 130;
            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnLevel(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantB = drawing;
            Graphics g = Graphics.FromImage(drawing);
            ShapeContainer s = new ShapeContainer();
            LineShape ls = new LineShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(Undrawnlevels_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(Undrawnlevels_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(Undrawnlevels_MouseUp);

            ls.BorderColor = Color.Pink;
            ls.BorderWidth = 5;
            ls.X1 = 900;
            ls.X2 = 950;
            ls.Y1 = 20;
            ls.Y2 = 20;

            l.Location = new Point(ls.X2, ls.Y2);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnInternalOrepass(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantB = drawing;
            Graphics g = Graphics.FromImage(drawing);
            ShapeContainer s = new ShapeContainer();
            LineShape ls = new LineShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnInternalOrepass_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnInternalOrepass_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnInternalOrepass_MouseUp);

            ls.BorderColor = Color.Pink;
            ls.BorderWidth = 5;
            ls.X1 = 900;
            ls.X2 = 950;
            ls.Y1 = 20;
            ls.Y2 = 20;

            l.Location = new Point(ls.X2, ls.Y2);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnHeadGear(DevExpress.XtraEditors.PanelControl panel, Image drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantI = drawing;

            PictureBox PicBox = new PictureBox();
            PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBox.Image = drawing;
            PicBox.Width = 60;
            PicBox.Height = 60;

            ShapeContainer s = new ShapeContainer();

            RectangleShape RS = new RectangleShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(50, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnHeadGear_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnHeadGear_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnHeadGear_MouseUp);

            RS.Left = 900;
            RS.Width = 40;
            RS.Top = 20;
            RS.Height = 80;
            RS.BorderColor = Color.WhiteSmoke;

            s.Left = RS.Left;
            s.Top = RS.Top;
            s.Width = RS.Width;
            s.Height = RS.Height;

            l.Location = new Point(950, 20);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            RS.BackgroundImage = PicBox.Image;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnOrePass(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantB = drawing;
            Graphics g = Graphics.FromImage(drawing);
            ShapeContainer s = new ShapeContainer();
            LineShape ls = new LineShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(60, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnOPass_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnOPass_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnOPass_MouseUp);

            ls.BorderColor = Color.Pink;
            ls.BorderWidth = 5;
            ls.X1 = 950;
            ls.X2 = 900;
            ls.Y1 = 20;
            ls.Y2 = 50;

            l.Location = new Point(ls.X1, ls.Y1);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnSubShaft(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantB = drawing;
            Graphics g = Graphics.FromImage(drawing);
            ShapeContainer s = new ShapeContainer();
            LineShape ls = new LineShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(60, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnSubShafts_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnSubShafts_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnSubShafts_MouseUp);

            ls.BorderColor = Color.Pink;
            ls.BorderWidth = 5;
            ls.X1 = 950;
            ls.X2 = 950;
            ls.Y1 = 20;
            ls.Y2 = 60;

            l.Location = new Point(ls.X1 + 10, ls.Y1);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnTerShaft(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantB = drawing;
            Graphics g = Graphics.FromImage(drawing);
            ShapeContainer s = new ShapeContainer();
            LineShape ls = new LineShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(60, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnTerShafts_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnTerShafts_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnTerShafts_MouseUp);

            ls.BorderColor = Color.Pink;
            ls.BorderWidth = 5;
            ls.X1 = 950;
            ls.X2 = 950;
            ls.Y1 = 20;
            ls.Y2 = 60;

            l.Location = new Point(ls.X1 + 10, ls.Y1);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnInclineShaft(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantB = drawing;
            Graphics g = Graphics.FromImage(drawing);
            ShapeContainer s = new ShapeContainer();
            LineShape ls = new LineShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(60, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnInclineShafts_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnInclineShafts_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnInclineShafts_MouseUp);

            ls.BorderColor = Color.Pink;
            ls.BorderWidth = 5;
            ls.X1 = 950;
            ls.X2 = 950;
            ls.Y1 = 20;
            ls.Y2 = 40;

            l.Location = new Point(ls.X1 + 10, ls.Y1);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnShaft(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing, string Selecteditem /* , Image HeadGear*/)
        {
            GiantP = panel;
            GiantB = drawing;
            Graphics g = Graphics.FromImage(drawing);
            ShapeContainer s = new ShapeContainer();
            LineShape ls = new LineShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(60, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnShafts_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnShafts_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnShafts_MouseUp);

            ls.BorderColor = Color.Pink;
            ls.BorderWidth = 5;
            ls.X1 = 950;
            ls.X2 = 950;
            ls.Y1 = 20;
            ls.Y2 = 60;

            l.Location = new Point(ls.X1 + 10, ls.Y1);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnDam(DevExpress.XtraEditors.PanelControl panel, Image drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantI = drawing;

            PictureBox PicBox = new PictureBox();
            PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBox.Image = drawing;
            PicBox.Width = 75;
            PicBox.Height = 28;
            ShapeContainer s = new ShapeContainer();
            RectangleShape RS = new RectangleShape(s);

            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(60, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnDam_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnDam_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnDam_MouseUp);

            RS.Left = 900;
            RS.Width = 75;
            RS.Top = 20;
            RS.Height = 28;
            RS.BorderColor = Color.WhiteSmoke;

            RS.BackgroundImage = PicBox.Image;

            l.Location = new Point(RS.Left - 30, RS.Top);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnMill(DevExpress.XtraEditors.PanelControl panel, Image drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantI = drawing;

            PictureBox PicBox = new PictureBox();
            PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBox.Image = drawing;
            PicBox.Width = 226;
            PicBox.Height = 132;

            ShapeContainer s = new ShapeContainer();

            RectangleShape RS = new RectangleShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(50, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnMill_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnMill_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnMill_MouseUp);

            RS.Left = 900;
            RS.Width = 226;
            RS.Top = 20;
            RS.Height = 132;
            RS.BorderColor = Color.Transparent;

            s.Left = RS.Left;
            s.Top = RS.Top;
            s.Width = RS.Width;
            s.Height = RS.Height;

            l.Location = new Point(950, 20);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            RS.BackgroundImage = PicBox.Image;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnPachuca(DevExpress.XtraEditors.PanelControl panel, Image drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantI = drawing;

            PictureBox PicBox = new PictureBox();
            PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBox.Image = drawing;
            PicBox.Width = 40;
            PicBox.Height = 60;

            ShapeContainer s = new ShapeContainer();

            RectangleShape RS = new RectangleShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(60, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnPachuca_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnPachuca_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnPachuca_MouseUp);

            RS.Left = 900;
            RS.Width = 40;
            RS.Top = 20;
            RS.Height = 60;

            s.Left = RS.Left;
            s.Top = RS.Top;
            s.Width = RS.Width;
            s.Height = RS.Height;

            l.Location = new Point(950, 20);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            RS.BackgroundImage = PicBox.Image;
            RS.BorderColor = Color.WhiteSmoke;

            panel.Controls.Add(s);
        }

        public static void UnDrawnPlant(DevExpress.XtraEditors.PanelControl panel, Image drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantI = drawing;

            PictureBox PicBox = new PictureBox();
            PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBox.Image = drawing;
            PicBox.Width = 60;
            PicBox.Height = 60;

            ShapeContainer s = new ShapeContainer();

            RectangleShape RS = new RectangleShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(50, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnPlant_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnPlant_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnPlant_MouseUp);

            RS.Left = 900;
            RS.Width = 50;
            RS.Top = 20;
            RS.Height = 80;

            s.Left = RS.Left;
            s.Top = RS.Top;
            s.Width = RS.Width;
            s.Height = RS.Height;

            l.Location = new Point(950, 20);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            RS.BackgroundImage = PicBox.Image;

            panel.Controls.Add(s);
            panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
        }

        public static void UnDrawnTank(DevExpress.XtraEditors.PanelControl panel, Image drawing, string Selecteditem)
        {
            GiantP = panel;
            GiantI = drawing;

            PictureBox PicBox = new PictureBox();
            PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBox.Image = drawing;
            PicBox.Width = 40;
            PicBox.Height = 60;

            ShapeContainer s = new ShapeContainer();

            RectangleShape RS = new RectangleShape(s);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(50, 15);
            l.Parent = panel;

            s.MouseDown += new System.Windows.Forms.MouseEventHandler(UndrawnTank_MouseDown);
            s.MouseMove += new System.Windows.Forms.MouseEventHandler(UndrawnTank_MouseMove);
            s.MouseUp += new System.Windows.Forms.MouseEventHandler(UndrawnTank_MouseUp);

            RS.Left = 900;
            RS.Width = 40;
            RS.Top = 20;
            RS.Height = 60;

            s.Left = RS.Left;
            s.Top = RS.Top;
            s.Width = RS.Width;
            s.Height = RS.Height;

            l.Location = new Point(950, 20);
            l.Text = Selecteditem;
            UndrawnName = Selecteditem;

            RS.BackgroundImage = PicBox.Image;
            RS.BorderColor = Color.WhiteSmoke;

            panel.Controls.Add(s);
        }

        public static void LoadMill(DevExpress.XtraEditors.PanelControl panel, Image drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = " Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'Mill' and Inactive = '0'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            int count = 0;

            foreach (DataRow Row in dt.Rows)
            {
                count = count + 1;

                System.Windows.Forms.PictureBox P = new System.Windows.Forms.PictureBox();
                P.Size = new Size(67, 58);
                P.Parent = panel;
                P.BackColor = Color.Transparent;

                System.Windows.Forms.Label l = new Label();
                l.Height = 15;
                l.Width = 100;
                l.Parent = panel;

                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);

                }

                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                }

                //Actual picturebox
                PictureBox PicBox = new PictureBox();
                PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
                PicBox.Image = drawing;
                PicBox.Width = 226;
                PicBox.Height = 132;
                ShapeContainer s = new ShapeContainer();
                RectangleShape ls = new RectangleShape(s);

                s.Visible = false;

                s.MouseDown += new System.Windows.Forms.MouseEventHandler(Mill_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(Mill_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(Mill_MouseUp);

                ls.Width = 226;
                ls.Height = 132;
                ls.BackgroundImage = PicBox.Image;
                ls.Left = linex;
                ls.Top = liney;
                ls.BorderColor = Color.Transparent;

                string Mill = Convert.ToString(Row["Name"]);
                l.Location = new Point(linex + 150, liney + 20);
                l.Text = Mill;

                s.Left = ls.Left;
                s.Top = ls.Top;
                s.Width = ls.Width;
                s.Height = ls.Height;

                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));

                s.Visible = true;
            }
        }

        public static void Mill_DoubleClick(object sender, MouseEventArgs e)
        {
            if (!isEdit)
            {

            }
        }

        public static void Tank_DoubleClick(object sender, MouseEventArgs e)
        {
            if (!isEdit && _DesignType != "Oreflow")
            {
                FrmBackfill_Book_Surface frmadd = new FrmBackfill_Book_Surface();
                frmadd.WindowState = FormWindowState.Normal;
                frmadd.StartPosition = FormStartPosition.CenterScreen;
                frmadd.Tanktxt.Text = pop.ToString();
                frmadd._theSystemDBTag = _Systemtag;
                frmadd._UserCurrentInfoConnection = _Userconnection;
                frmadd.ShowDialog();
            }
        }

        public static Form OreFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }

        public static void Pachuca_DoubleClick(object sender, MouseEventArgs e)
        {
            if (!isEdit && _DesignType != "Oreflow")
            {
                FrmBackfillTransfer_In frmadd = new FrmBackfillTransfer_In();
                frmadd.WindowState = FormWindowState.Normal;
                frmadd.StartPosition = FormStartPosition.CenterScreen;
                frmadd.LocationTotxt.Text = pop.ToString();
                frmadd._theSystemDBTag = _Systemtag;
                frmadd._UserCurrentInfoConnection = _Userconnection;
                frmadd.ShowDialog();
            }
        }

        public static void Dam_DoubleClick(object sender, MouseEventArgs e)
        {
            if (!isEdit && _DesignType != "Oreflow")
            {
                FrmBackfillTransfer_In frmadd = new FrmBackfillTransfer_In();
                frmadd.WindowState = FormWindowState.Normal;
                frmadd.StartPosition = FormStartPosition.CenterScreen;
                frmadd.LocationTotxt.Text = pop.ToString();
                frmadd._theSystemDBTag = _Systemtag;
                frmadd._UserCurrentInfoConnection = _Userconnection;
                frmadd.ShowDialog();
            }
        }

        public static void LoadPachuca(DevExpress.XtraEditors.PanelControl panel, Image drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'Pachuca' and Inactive = '0' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            int count = 0;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.PictureBox P = new System.Windows.Forms.PictureBox();
                P.Size = new Size(67, 58);
                P.Parent = panel;
                P.BackColor = Color.Transparent;

                System.Windows.Forms.Label l = new Label();
                l.Height = 13;
                l.Width = 50;
                l.Parent = panel;

                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                }

                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                }

                //Actual picturebox
                PictureBox PicBox = new PictureBox();
                PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
                PicBox.Image = drawing;
                PicBox.Width = 40;
                PicBox.Height = 60;

                ShapeContainer s = new ShapeContainer();
                RectangleShape ls = new RectangleShape(s);

                s.MouseDown += new System.Windows.Forms.MouseEventHandler(Pachuca_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(Pachuca_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(Pachuca_MouseUp);

                ls.Width = 40;
                ls.Height = 60;
                ls.BackgroundImage = PicBox.Image;
                ls.Left = linex;
                ls.Top = liney;
                ls.BorderColor = Color.Transparent;

                string Patchuka = Convert.ToString(Row["Name"]);
                l.Location = new Point(linex + 10, liney - 20);
                l.Text = Patchuka;

                s.Left = ls.Left;
                s.Top = ls.Top;
                s.Width = ls.Width;
                s.Height = ls.Height;

                s.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Pachuca_DoubleClick);
                panel.Controls.Add(s);
            }
        }

        public static void LoadPlant(DevExpress.XtraEditors.PanelControl panel, Image drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select * from tbl_OreFlowEntities where OreFlowCode = 'Plant' and Inactive = 'N' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.PictureBox P = new System.Windows.Forms.PictureBox();
                P.Size = new Size(67, 58);
                P.Parent = panel;
                P.BackColor = Color.Transparent;

                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);

                }

                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                }

                //Actual picturebox
                PictureBox PicBox = new PictureBox();
                PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
                PicBox.Image = drawing;
                PicBox.Width = 60;
                PicBox.Height = 60;
                ShapeContainer s = new ShapeContainer();
                RectangleShape ls = new RectangleShape(s);


                s.MouseDown += new System.Windows.Forms.MouseEventHandler(Plant_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(Plant_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(Plant_MouseUp);

                ls.Width = 50;
                ls.Height = 80;
                ls.BackgroundImage = PicBox.Image;
                ls.Left = linex;
                ls.Top = liney;

                s.Left = ls.Left;
                s.Top = ls.Top;
                s.Width = ls.Width;
                s.Height = ls.Height;

                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
            }
        }

        public static void LoadShaft(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing, Image HeadGear)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'Shaft' and Inactive = '0' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.Label l = new Label();
                l.Height = 13;
                l.Parent = panel;

                int lblx = 0;
                int lbly = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    lblx = Convert.ToInt32(Row["X1"].ToString());
                }

                if (Row["Y2"] != DBNull.Value)
                {
                    lbly = Convert.ToInt32(Row["Y2"]);
                }

                string sshaft = Convert.ToString(Row["Name"]);
                l.Location = new Point(lblx, lbly);
                l.Text = sshaft;

                System.Windows.Forms.PictureBox P = new System.Windows.Forms.PictureBox();
                P.Size = new Size(67, 58);
                P.Parent = panel;
                P.BackColor = Color.Transparent;

                PictureBox PicBox = new PictureBox();
                PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
                PicBox.Image = HeadGear;
                PicBox.Width = 110;
                PicBox.Height = 160;

                ShapeContainer r = new ShapeContainer();
                RectangleShape RS = new RectangleShape(r);

                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;
                //P.Location = new Point(picx, picy);

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                    linex2 = Convert.ToInt32(Row["X2"]);
                }

                if (Row["Y1"] != DBNull.Value)
                {
                    if (Row["Y2"] != DBNull.Value)
                    {
                        liney = Convert.ToInt32(Row["Y1"]);
                        liney2 = Convert.ToInt32(Row["Y2"]);

                        if (liney == liney2)
                        {
                            liney2 = liney2 + 40;
                        }
                    }
                }

                Graphics g = Graphics.FromImage(drawing);
                ShapeContainer s = new ShapeContainer();

                LineShape ls = new LineShape(s);

                s.MouseDown += new System.Windows.Forms.MouseEventHandler(shafts_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(shafts_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(shafts_MouseUp);

                ls.BorderColor = Color.Gray;
                ls.BorderWidth = 6;
                ls.X1 = linex;
                ls.X2 = linex;
                ls.Y1 = liney;
                ls.Y2 = liney2;

                RS.Width = 110;
                RS.Height = 160;
                RS.Top = liney - (RS.Height - 30);
                //RS.Top = liney - (RS.Height - 20);
                RS.Left = linex - (RS.Width / 2);
                RS.BorderColor = Color.Transparent;

                s.Left = RS.Left;
                s.Top = RS.Top;
                s.Width = RS.Width;
                s.Height = RS.Height;

                RS.BackgroundImage = PicBox.Image;

                RS.Visible = true;

                panel.Controls.Add(r);
                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
            }
        }

        public static void LoadHeadGear(DevExpress.XtraEditors.PanelControl panel, Image drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select * from tbl_OreFlowEntities where OreFlowCode = 'HeadGear' and Inactive = 'N' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();

            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.PictureBox P = new System.Windows.Forms.PictureBox();
                P.Size = new Size(67, 58);
                P.Parent = panel;
                P.BackColor = Color.Transparent;

                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                }

                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                }

                PictureBox PicBox = new PictureBox();
                PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
                PicBox.Image = drawing;
                PicBox.Width = 60;
                PicBox.Height = 60;
                ShapeContainer s = new ShapeContainer();
                RectangleShape ls = new RectangleShape(s);

                s.MouseDown += new System.Windows.Forms.MouseEventHandler(HeadGear_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(HeadGear_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(HeadGear_MouseUp);

                ls.Width = 40;
                ls.Height = 80;
                ls.BackgroundImage = PicBox.Image;
                ls.Left = linex;
                ls.BorderColor = Color.WhiteSmoke;

                ls.Top = liney;

                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
            }
        }

        public static void LoadDam(DevExpress.XtraEditors.PanelControl panel, Image drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'Dam' and Inactive = '0' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.Label l = new Label();
                l.Height = 13;
                l.Parent = panel;

                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                }

                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                }

                //Actual picturebox
                PictureBox PicBox = new PictureBox();
                PicBox.SizeMode = PictureBoxSizeMode.Normal;
                PicBox.Image = drawing;
                PicBox.Width = 75;
                PicBox.Height = 28;
                ShapeContainer s = new ShapeContainer();
                RectangleShape ls = new RectangleShape(s);

                s.MouseDown += new System.Windows.Forms.MouseEventHandler(Dam_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(Dam_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(Dam_MouseUp);

                ls.Width = 75;
                ls.Height = 28;
                ls.BackgroundImage = PicBox.Image;
                ls.Left = linex;
                ls.Top = liney;
                ls.BorderColor = Color.Transparent;

                string Dam = Convert.ToString(Row["Name"]);
                l.Location = new Point(linex + 90, liney + 10);
                l.Text = Dam;

                s.Left = ls.Left;
                s.Top = ls.Top;
                s.Width = ls.Width;
                s.Height = ls.Height;

                s.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Dam_DoubleClick);

                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));
            }
        }

        public static void LoadSubShaft(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'SubShaft' and Inactive = '0' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.PictureBox P = new System.Windows.Forms.PictureBox();
                P.Size = new Size(67, 58);
                P.Parent = panel;

                System.Windows.Forms.Label l = new Label();
                l.Size = new Size(30, 15);
                l.Parent = panel;

                P.BackColor = Color.Transparent;
                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                    linex2 = Convert.ToInt32(Row["X2"]);
                }

                if (Row["Y1"] != DBNull.Value)
                {
                    if (Row["Y2"] != DBNull.Value)
                    {
                        liney = Convert.ToInt32(Row["Y1"]);
                        liney2 = Convert.ToInt32(Row["Y2"]);
                    }
                }

                Graphics g = Graphics.FromImage(drawing);
                ShapeContainer s = new ShapeContainer();
                LineShape ls = new LineShape(s);
                s.MouseDown += new System.Windows.Forms.MouseEventHandler(subshafts_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(subshafts_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(subshafts_MouseUp);
                ls.BorderColor = Color.Gray;
                ls.BorderWidth = 6;
                ls.X1 = linex;
                ls.X2 = linex;
                ls.Y1 = liney;
                ls.Y2 = liney2;
                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));

                picx = Convert.ToInt32(Row["X1"]);
                picy = Convert.ToInt32(Row["Y2"]);
                string level = Convert.ToString(Row["Name"]);

                l.Location = new Point(linex2 + 10, picy - 5);
                l.Text = level;
            }
        }

        public static void LoadTerShaft(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'TerShaft' and Inactive = '0' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.PictureBox P = new System.Windows.Forms.PictureBox();
                P.Size = new Size(67, 58);
                P.Parent = panel;

                System.Windows.Forms.Label l = new Label();
                l.Size = new Size(30, 15);
                l.Parent = panel;

                P.BackColor = Color.Transparent;
                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                    linex2 = Convert.ToInt32(Row["X2"]);
                }

                if (Row["Y1"] != DBNull.Value)
                {
                    if (Row["Y2"] != DBNull.Value)
                    {
                        liney = Convert.ToInt32(Row["Y1"]);
                        liney2 = Convert.ToInt32(Row["Y2"]);
                    }
                }

                Graphics g = Graphics.FromImage(drawing);
                ShapeContainer s = new ShapeContainer();
                LineShape ls = new LineShape(s);
                s.MouseDown += new System.Windows.Forms.MouseEventHandler(tershafts_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(tershafts_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(tershafts_MouseUp);
                ls.BorderColor = Color.Gray;
                ls.BorderWidth = 6;
                ls.X1 = linex;
                ls.X2 = linex;
                ls.Y1 = liney;
                ls.Y2 = liney2;
                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));

                picx = Convert.ToInt32(Row["X1"]);
                picy = Convert.ToInt32(Row["Y2"]);
                string level = Convert.ToString(Row["Name"]);

                l.Location = new Point(linex2 + 10, picy - 5);
                l.Text = level;
            }
        }

        public static void LoadInclineShaft(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = " Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'InclShaft' and Inactive = '0' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.PictureBox P = new System.Windows.Forms.PictureBox();
                P.Size = new Size(67, 58);
                P.Parent = panel;

                System.Windows.Forms.Label l = new Label();
                l.Size = new Size(30, 15);
                l.Parent = panel;

                P.BackColor = Color.Transparent;
                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                    linex2 = Convert.ToInt32(Row["X2"]);
                }

                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                    liney2 = Convert.ToInt32(Row["Y2"]);
                }

                Graphics g = Graphics.FromImage(drawing);
                ShapeContainer s = new ShapeContainer();
                LineShape ls = new LineShape(s);
                s.MouseDown += new System.Windows.Forms.MouseEventHandler(Inclineshafts_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(Inclineshafts_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(Inclineshafts_MouseUp);
                ls.BorderColor = Color.Gray;
                ls.BorderWidth = 6;
                ls.X1 = linex;
                ls.X2 = linex2;
                ls.Y1 = liney;
                ls.Y2 = liney2;
                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));

                picx = Convert.ToInt32(Row["X1"]);
                picy = Convert.ToInt32(Row["Y2"]);
                string level = Convert.ToString(Row["Name"]);
                l.Location = new Point(linex2, picy - 5);
                l.Text = level;
            }
        }

        public static void LoadLevels(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'Lvl' and Inactive = '0'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.Label l = new Label();
                l.Size = new Size(30, 15);
                l.Parent = panel;
                string levelID = Row["BFID"].ToString();

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                    linex2 = Convert.ToInt32(Row["X2"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                    liney2 = Convert.ToInt32(Row["Y2"]);
                }

                Graphics g = Graphics.FromImage(drawing);
                ShapeContainer s = new ShapeContainer();

                LineShape ls = new LineShape(s);
                s.MouseDown += new System.Windows.Forms.MouseEventHandler(levels_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(levels_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(levels_MouseUp);

                ls.BorderColor = Color.Green;
                ls.BorderWidth = 2;
                ls.X1 = linex;
                ls.X2 = linex2;
                ls.Y1 = liney;
                ls.Y2 = liney2;
                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));

                if (linex > linex2)
                {
                    int picx = Convert.ToInt32(Row["X1"]);
                    int picy = Convert.ToInt32(Row["Y2"]);
                    string level = Convert.ToString(Row["Name"]);
                    float a = picx;
                    float b = picy;
                    l.Location = new Point(linex2, picy - 5);
                    l.Text = level;
                }
                else if (linex2 > linex)
                {
                    int picx = Convert.ToInt32(Row["X1"]);
                    int picy = Convert.ToInt32(Row["Y2"]);
                    string level = Convert.ToString(Row["Name"]);
                    float a = picx;
                    float b = picy;
                    l.Location = new Point(linex2, picy - 5);
                    l.Text = level;
                }
            }
        }

        public static void LoadInternalOrepass(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'IOrePass' and Inactive = '0'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.Label l = new Label();
                l.Size = new Size(60, 15);
                l.Parent = panel;
                string levelID = Row["BFID"].ToString();

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                    linex2 = Convert.ToInt32(Row["X2"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                    liney2 = Convert.ToInt32(Row["Y2"]);
                }

                Graphics g = Graphics.FromImage(drawing);
                ShapeContainer s = new ShapeContainer();

                LineShape ls = new LineShape(s);
                s.MouseDown += new System.Windows.Forms.MouseEventHandler(InternalOrepass_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(InternalOrepass_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(InternalOrepass_MouseUp);

                ls.BorderColor = Color.Red;
                ls.BorderWidth = 2;
                ls.X1 = linex;
                ls.X2 = linex2;
                ls.Y1 = liney;
                ls.Y2 = liney2;
                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));

                if (linex > linex2)
                {
                    int picx = Convert.ToInt32(Row["X1"]);
                    int picy = Convert.ToInt32(Row["Y2"]);
                    string level = Convert.ToString(Row["Name"]);
                    float a = picx;
                    float b = picy;
                    l.Location = new Point(((linex + linex2) / 2) + 30, ((liney + liney2) / 2));
                    l.Text = level;
                }
                else if (linex2 > linex)
                {
                    int picx = Convert.ToInt32(Row["X1"]);
                    int picy = Convert.ToInt32(Row["Y2"]);
                    string level = Convert.ToString(Row["Name"]);
                    float a = picx;
                    float b = picy;
                    l.Location = new Point(((linex + linex2) / 2) + 30, ((liney + liney2) / 2));
                    l.Text = level;
                }
            }
        }

        public static void LoadOrepass(DevExpress.XtraEditors.PanelControl panel, Bitmap drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'OPass' and Inactive = '0' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                Decimal perc = 0;
                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                System.Windows.Forms.Label l = new Label();
                l.Size = new Size(80, 15);
                l.Parent = panel;
                string levelID = Row["BFID"].ToString();

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                    linex2 = Convert.ToInt32(Row["X2"]);
                }

                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                }

                if (Row["Y2"] != DBNull.Value)
                {
                    liney2 = Convert.ToInt32(Row["Y2"]);
                }
                else
                {
                    liney2 = 0;
                }

                Graphics g = Graphics.FromImage(drawing);
                ShapeContainer s = new ShapeContainer();
                LineShape ls = new LineShape(s);
                s.MouseDown += new System.Windows.Forms.MouseEventHandler(OPass_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(OPass_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(OPass_MouseUp);
                ls.BorderColor = Color.Red;
                ls.BringToFront();
                ls.BorderWidth = 2;
                ls.X1 = linex;
                ls.X2 = linex2;
                ls.Y1 = liney;
                ls.Y2 = liney2;
                panel.Controls.Add(s);
                panel.CreateGraphics().DrawImageUnscaled(drawing, new Point(0, 0));

                if (linex > linex2)
                {
                    int picx = Convert.ToInt32(Row["X1"]);
                    int picy = Convert.ToInt32(Row["Y2"]);
                    string level = Convert.ToString(Row["BFID"]);
                    float a = picx;
                    float b = picy;
                    l.Location = new Point(((linex + linex2) / 2) + 10, ((liney + liney2) / 2));
                    l.Text = level;
                }
                else if (linex2 > linex)
                {
                    int picx = Convert.ToInt32(Row["X1"]);
                    int picy = Convert.ToInt32(Row["Y2"]);
                    string level = Convert.ToString(Row["BFID"]);
                    float a = picx;
                    float b = picy;
                    l.Location = new Point(((linex + linex2) / 2) + 10, ((liney + liney2) / 2));
                    l.Text = level;
                }
            }
        }

        public static void LoadTank(DevExpress.XtraEditors.PanelControl panel, Image drawing)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select oreflowid BFID ,Name,X1,X2,Y1,Y2 from tbl_OreFlowEntities where oreflowcode = 'Tank' and Inactive = '0' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            dt.Clear();
            dt = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dt.Rows)
            {
                System.Windows.Forms.PictureBox P = new System.Windows.Forms.PictureBox();
                P.Size = new Size(67, 58);
                P.Parent = panel;
                P.BackColor = Color.Transparent;

                DevExpress.XtraEditors.LabelControl l = new DevExpress.XtraEditors.LabelControl();

                l.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                l.Height = 20;
                l.Width = 35;
                l.Parent = panel;

                int picx = 0;
                int picy = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    picx = Convert.ToInt32(Row["X1"]);
                }
                if (Row["Y1"] != DBNull.Value)
                {
                    picy = Convert.ToInt32(Row["Y1"]);
                }
                float a = picx;
                float b = picy;

                int linex = 0;
                int liney = 0;
                int linex2 = 0;
                int liney2 = 0;

                if (Row["X1"] != DBNull.Value)
                {
                    linex = Convert.ToInt32(Row["X1"]);
                }

                if (Row["Y1"] != DBNull.Value)
                {
                    liney = Convert.ToInt32(Row["Y1"]);
                }

                //Actual picturebox
                PictureBox PicBox = new PictureBox();
                PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
                PicBox.Image = drawing;
                PicBox.Width = 40;
                PicBox.Height = 60;

                ShapeContainer s = new ShapeContainer();
                RectangleShape ls = new RectangleShape(s);

                s.MouseDown += new System.Windows.Forms.MouseEventHandler(Tank_MouseDown);
                s.MouseMove += new System.Windows.Forms.MouseEventHandler(Tank_MouseMove);
                s.MouseUp += new System.Windows.Forms.MouseEventHandler(Tank_MouseUp);

                ls.Width = 40;
                ls.Height = 60;
                ls.BackgroundImage = PicBox.Image;
                ls.Left = linex;
                ls.Top = liney;
                ls.BorderColor = Color.Transparent;

                string Patchuka = Convert.ToString(Row["Name"]);
                l.Location = new Point(linex - 10, liney - 20);
                l.Text = Patchuka.ToString();
                l.BringToFront();

                s.Left = ls.Left;
                s.Top = ls.Top;
                s.Width = ls.Width;
                s.Height = ls.Height;

                s.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Tank_DoubleClick);

                panel.Controls.Add(s);
            }
        }

        public static void Loadgrids(DevExpress.XtraEditors.PanelControl panel, int count)
        {
            ////Mill Grid
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  Select * from tbl_OreFlowEntities where OreFlowCode = 'Mill' and Inactive = 'N' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            DataTable dtMill = new DataTable();
            dtMill.Clear();
            dtMill = _dbMan.ResultsDataTable;

            foreach (DataRow Row in dtMill.Rows)
            {
                DataGridView MillScadaGrid = new DataGridView();

                MillScadaGrid.Left = Convert.ToInt32(Row["X1"]);
                MillScadaGrid.Top = Convert.ToInt32(Row["Y1"]) + 150;

                MillScadaGrid.Visible = true;
                MillScadaGrid.AllowUserToResizeColumns = false;
                MillScadaGrid.AllowUserToResizeRows = false;
                MillScadaGrid.RowHeadersVisible = false;
                MillScadaGrid.ReadOnly = true;
                MillScadaGrid.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                MillScadaGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                MillScadaGrid.Parent = panel;
                MillScadaGrid.Height = 69;
                MillScadaGrid.Width = 160;

                MillScadaGrid.ColumnCount = 4;

                MillScadaGrid.Columns[0].HeaderText = "Test";
                MillScadaGrid.Columns[1].HeaderText = "Low";
                MillScadaGrid.Columns[2].HeaderText = "High";
                MillScadaGrid.Columns[3].HeaderText = "Avg";

                MillScadaGrid.Columns[0].Width = 40;
                MillScadaGrid.Columns[1].Width = 40;
                MillScadaGrid.Columns[2].Width = 40;
                MillScadaGrid.Columns[3].Width = 40;

                MillScadaGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                MillScadaGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                MillScadaGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                MillScadaGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                MillScadaGrid.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
                MillScadaGrid.Columns[1].DefaultCellStyle.BackColor = Color.Gainsboro;
                MillScadaGrid.Columns[2].DefaultCellStyle.BackColor = Color.Gainsboro;
                MillScadaGrid.Columns[3].DefaultCellStyle.BackColor = Color.Gainsboro;

                MillScadaGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                MillScadaGrid.EnableHeadersVisualStyles = false;

                MillScadaGrid.ScrollBars = ScrollBars.None;

                MillScadaGrid.Rows.Add("RD", string.Empty, string.Empty, string.Empty);
                MillScadaGrid.Rows.Add("Perm", string.Empty, string.Empty, string.Empty);

                System.Windows.Forms.Label lblMill = new Label();
                lblMill.Height = 13;
                lblMill.Width = 80;
                lblMill.Parent = panel;
                lblMill.Text = "Mill";

                lblMill.Left = MillScadaGrid.Left + 50;
                lblMill.Top = MillScadaGrid.Top - 15;

                lblMill.Visible = true;

                MillScadaGrid.BringToFront();
            }

            MWDataManager.clsDataAccess _dbMana = new MWDataManager.clsDataAccess();
            _dbMana.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMana.SqlStatement = "  Select * from tbl_OreFlowEntities where OreFlowCode = 'Mill' and Inactive = 'N' ";
            _dbMana.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMana.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMana.ExecuteInstruction();

            DataTable dtMilla = new DataTable();
            dtMilla.Clear();
            dtMilla = _dbMana.ResultsDataTable;

            #region PacGrid
            ////underground grid    
            MWDataManager.clsDataAccess _dbManUndera = new MWDataManager.clsDataAccess();
            _dbManUndera.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbManUndera.SqlStatement = "  Select * from tbl_OreFlowEntities where OreFlowCode = 'UGgrid' and OreFlowID = 'UG01' ";
            _dbManUndera.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManUndera.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManUndera.ExecuteInstruction();

            DataTable dtUnder = new DataTable();
            dtUnder.Clear();
            dtUnder = _dbManUndera.ResultsDataTable;

            DataGridView PacUnderGroundScadaGrid = new DataGridView();

            foreach (DataRow dr in dtUnder.Rows)
            {
                PacUnderGroundScadaGrid.Left = Convert.ToInt32(dr["X1"]);
                PacUnderGroundScadaGrid.Top = Convert.ToInt32(dr["Y1"]);
            }

            PacUnderGroundScadaGrid.Visible = true;
            PacUnderGroundScadaGrid.AllowUserToResizeColumns = false;
            PacUnderGroundScadaGrid.AllowUserToResizeRows = false;
            PacUnderGroundScadaGrid.RowHeadersVisible = false;
            PacUnderGroundScadaGrid.ReadOnly = true;
            PacUnderGroundScadaGrid.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PacUnderGroundScadaGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            PacUnderGroundScadaGrid.Parent = panel;
            PacUnderGroundScadaGrid.Height = 69;
            PacUnderGroundScadaGrid.Width = 160;

            PacUnderGroundScadaGrid.ColumnCount = 4;

            PacUnderGroundScadaGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            PacUnderGroundScadaGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            PacUnderGroundScadaGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            PacUnderGroundScadaGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

            PacUnderGroundScadaGrid.Columns[0].HeaderText = "Test";
            PacUnderGroundScadaGrid.Columns[1].HeaderText = "Low";
            PacUnderGroundScadaGrid.Columns[2].HeaderText = "High";
            PacUnderGroundScadaGrid.Columns[3].HeaderText = "Avg";

            PacUnderGroundScadaGrid.Columns[0].Width = 40;
            PacUnderGroundScadaGrid.Columns[1].Width = 40;
            PacUnderGroundScadaGrid.Columns[2].Width = 40;
            PacUnderGroundScadaGrid.Columns[3].Width = 40;

            PacUnderGroundScadaGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            PacUnderGroundScadaGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PacUnderGroundScadaGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PacUnderGroundScadaGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            PacUnderGroundScadaGrid.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            PacUnderGroundScadaGrid.Columns[1].DefaultCellStyle.BackColor = Color.Gainsboro;
            PacUnderGroundScadaGrid.Columns[2].DefaultCellStyle.BackColor = Color.Gainsboro;
            PacUnderGroundScadaGrid.Columns[3].DefaultCellStyle.BackColor = Color.Gainsboro;

            PacUnderGroundScadaGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            PacUnderGroundScadaGrid.EnableHeadersVisualStyles = false;

            PacUnderGroundScadaGrid.ScrollBars = ScrollBars.None;

            System.Windows.Forms.Label label = new Label();
            label.Height = 13;
            label.Width = 80;
            label.Parent = panel;
            label.Text = "Underground";

            label.Left = PacUnderGroundScadaGrid.Left + 50;
            label.Top = PacUnderGroundScadaGrid.Top - 15;

            MWDataManager.clsDataAccess _dbManUnder = new MWDataManager.clsDataAccess();
            _dbManUnder.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbManUnder.SqlStatement = "   Declare @enddate datetime  \r\n" +
                                       "   Declare @Startdate datetime   \r\n" +
                                       "   set @startdate = dateadd( dd, -" + count + ",GetDate()  )  \r\n" +
                                       "   set @enddate = GETDATE()  \r\n" +
                                       "   Select ISNULL(AvgBQT,0.00) AvgBQT,ISNULL(HighBQT,0.00) HighBQT ,ISNULL(LowBQT,0.00) LowBQT ,ISNULL(avgRD,0.00) avgRD,ISNULL(HighRD,0.00) HighRD ,ISNULL(LowRD,0.00) LowRD from (  \r\n" +
                                       "   Select avg(BQTint) AvgBQT ,MAX(BQTint) HighBQT, MIN(BQTint) LowBQT,  avg(RDint) AvgRD , MAX(RDint) HighRD, MIN(RDint) LowRD from(    \r\n" +
                                       "   Select convert(decimal(18,2),RD) RDint ,convert(decimal(18,2),BQT) BQTint ,* from tbl_Backfill_Transfer_In  \r\n" +
                                       "   where  CONVERT(datetime, substring( CONVERT(varchar(50) ,CalenderDateSent),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))  \r\n" +
                                       "   and   CONVERT(datetime, substring( CONVERT(varchar(50) ,CalenderDateSent),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))   \r\n" +
                                       "   ) a  )a   ";
            _dbManUnder.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManUnder.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManUnder.ExecuteInstruction();

            DataTable dtunder = new DataTable();
            dtunder.Clear();

            dtunder = _dbManUnder.ResultsDataTable;

            PacUnderGroundScadaGrid.DataSource = null;
            PacUnderGroundScadaGrid.Rows.Clear();

            PacUnderGroundScadaGrid.Refresh();
            PacUnderGroundScadaGrid.Update();

            if (dtunder.Rows.Count > 0)
            {
                PacUnderGroundScadaGrid.Rows.Add("RD", Math.Round(Convert.ToDecimal(dtunder.Rows[0][5]), 2).ToString(), Math.Round(Convert.ToDecimal(dtunder.Rows[0][4]), 2).ToString(), Math.Round(Convert.ToDecimal(dtunder.Rows[0][3]), 2).ToString());
                PacUnderGroundScadaGrid.Rows.Add("Perm", Math.Round(Convert.ToDecimal(dtunder.Rows[0][2]), 2).ToString(), Math.Round(Convert.ToDecimal(dtunder.Rows[0][1]), 2).ToString(), Math.Round(Convert.ToDecimal(dtunder.Rows[0][0]), 2).ToString());
            }

            PacUnderGroundScadaGrid.BringToFront();

            PacUnderGroundScadaGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(PacUnderGround_MouseUp);
            #endregion

            #region Surfacegrid
            ////Surface grid
            MWDataManager.clsDataAccess _dbManSurfa = new MWDataManager.clsDataAccess();
            _dbManSurfa.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbManSurfa.SqlStatement = "  Select * from tbl_OreFlowEntities where OreFlowCode = 'SFgrid' and OreFlowID = 'SG01' ";
            _dbManSurfa.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSurfa.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSurfa.ExecuteInstruction();

            DataTable dtSurfa = new DataTable();
            dtSurfa.Clear();
            dtSurfa = _dbManSurfa.ResultsDataTable;

            DataGridView SurfaceScadaGrid = new DataGridView();

            foreach (DataRow dr in dtSurfa.Rows)
            {
                SurfaceScadaGrid.Left = Convert.ToInt32(dr["X1"]);
                SurfaceScadaGrid.Top = Convert.ToInt32(dr["Y1"]);
            }

            SurfaceScadaGrid.Visible = true;
            SurfaceScadaGrid.AllowUserToResizeColumns = false;
            SurfaceScadaGrid.AllowUserToResizeRows = false;
            SurfaceScadaGrid.RowHeadersVisible = false;
            SurfaceScadaGrid.ReadOnly = true;
            SurfaceScadaGrid.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            SurfaceScadaGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            SurfaceScadaGrid.Parent = panel;
            SurfaceScadaGrid.Height = 69;
            SurfaceScadaGrid.Width = 160;

            SurfaceScadaGrid.ColumnCount = 4;

            SurfaceScadaGrid.Columns[0].HeaderText = "Test";
            SurfaceScadaGrid.Columns[1].HeaderText = "Low";
            SurfaceScadaGrid.Columns[2].HeaderText = "High";
            SurfaceScadaGrid.Columns[3].HeaderText = "Avg";

            SurfaceScadaGrid.Columns[0].Width = 40;
            SurfaceScadaGrid.Columns[1].Width = 40;
            SurfaceScadaGrid.Columns[2].Width = 40;
            SurfaceScadaGrid.Columns[3].Width = 40;

            SurfaceScadaGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            SurfaceScadaGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            SurfaceScadaGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            SurfaceScadaGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            SurfaceScadaGrid.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
            SurfaceScadaGrid.Columns[1].DefaultCellStyle.BackColor = Color.Gainsboro;
            SurfaceScadaGrid.Columns[2].DefaultCellStyle.BackColor = Color.Gainsboro;
            SurfaceScadaGrid.Columns[3].DefaultCellStyle.BackColor = Color.Gainsboro;

            SurfaceScadaGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            SurfaceScadaGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            SurfaceScadaGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            SurfaceScadaGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

            SurfaceScadaGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            SurfaceScadaGrid.EnableHeadersVisualStyles = false;

            SurfaceScadaGrid.ScrollBars = ScrollBars.None;

            System.Windows.Forms.Label lblSurface = new Label();
            lblSurface.Height = 13;
            lblSurface.Width = 80;
            lblSurface.Parent = panel;
            lblSurface.Text = "Surface";

            lblSurface.Left = SurfaceScadaGrid.Left + 50;
            lblSurface.Top = SurfaceScadaGrid.Top - 15;

            MWDataManager.clsDataAccess _dbManSurface = new MWDataManager.clsDataAccess();
            _dbManSurface.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbManSurface.SqlStatement = "  Declare @enddate datetime  \r\n" +
                                         "  Declare @Startdate datetime  \r\n" +
                                         "  set @startdate = dateadd( dd, -" + count + ",GetDate()  )  \r\n" +
                                         "  set @enddate = GETDATE()  \r\n" +
                                         " Select ISNULL(avgRD,0.00) avgRD,ISNULL(HighRD,0.00) HighRD ,ISNULL(LowRD,0.00) LowRD from (   \r\n" +
                                         "  Select avg(RD) AvgRD , MAX(RD) HighRD, MIN(RD) LowRD from(     \r\n" +
                                         "  Select  * from tbl_Backfill_Book_Surface  \r\n" +
                                         "  where  CONVERT(datetime, substring( CONVERT(varchar(50) ,CalenderDate),0,10) ) >= convert(datetime, substring( convert(varchar(50), @Startdate),0,10))  \r\n" +
                                         "  and   CONVERT(datetime, substring( CONVERT(varchar(50) ,CalenderDate),0,10) ) <= convert(datetime, substring( convert(varchar(50), @enddate),0,10))  \r\n" +
                                         "  ) a  )a ";
            _dbManSurface.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManSurface.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManSurface.ExecuteInstruction();

            DataTable dtSurf = new DataTable();
            dtSurf.Clear();
            dtSurf = _dbManSurface.ResultsDataTable;

            SurfaceScadaGrid.DataSource = null;
            SurfaceScadaGrid.Rows.Clear();

            SurfaceScadaGrid.Refresh();
            SurfaceScadaGrid.Update();

            if (dtSurf.Rows.Count > 0)
            {
                SurfaceScadaGrid.Rows.Add("RD", Math.Round(Convert.ToDecimal(dtSurf.Rows[0][2]), 2).ToString(), Math.Round(Convert.ToDecimal(dtSurf.Rows[0][1]), 2).ToString(), Math.Round(Convert.ToDecimal(dtSurf.Rows[0][0]), 2).ToString());
                SurfaceScadaGrid.Rows.Add("Perm", string.Empty, string.Empty, string.Empty);
            }

            SurfaceScadaGrid.BringToFront();
            SurfaceScadaGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(SurfaceGrid_MouseUp);
            #endregion
        }

        public static void PacUnderGround_MouseUp(object sender, MouseEventArgs e)
        {
            var X = sender as DataGridView;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  update tbl_OreFlowEntities set X1 = '" + X.Left + "' , Y1= '" + X.Top + "' where OreFlowID = 'UG01' and OreFlowCode = 'UGgrid'  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
        }

        public static void SurfaceGrid_MouseUp(object sender, MouseEventArgs e)
        {
            var X = sender as DataGridView;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = "  update tbl_OreFlowEntities set X1 = '" + X.Left + "' , Y1= '" + X.Top + "' where OreFlowID = 'SG01' and OreFlowCode = 'SFgrid'  ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();
        }

        public static void Creategrids(DevExpress.XtraEditors.PanelControl panel, int count)
        {

        }

        public static void Trams_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("TestOrebin");
        }

        #region Levels Move And Resize
        public static string Y = string.Empty;
        public static string Lenght = string.Empty;

        public static void levels_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
                    var X = sender as TextBox;

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = GetControl("LevelsRB", ls.X1, ls.Y1);

                    Y = ls.Y2.ToString();

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    dragStartPoint = MouseIsNearBy(oldStartPoint, f, e);
                    dragEndPoint = MouseIsNearBy(oldEndPoint, f, e);

                    if (ls.X1 > ls.X2)
                    {
                        if (e.X <= (ls.X2 + 20))
                        {
                            dragEndPoint = true;
                        }
                        if (e.X >= (ls.X1 - 20))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.X1 < ls.X2)
                    {
                        if (e.X >= ls.X2 - 20)
                        {
                            dragEndPoint = true;
                        }
                        if (e.X <= ls.X1 + 20)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeWE;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeWE;
                    }
                }
            }
        }

        public static void levels_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
                ls.Cursor = Cursors.Hand;
            }
            else if (dragStartPoint)
            {//works

                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.Cursor = Cursors.SizeWE;
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
                ls.Cursor = Cursors.SizeWE;
            }
        }

        public static ucDiagram form;

        public static void levels_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;

            Name = string.Empty;

            l.Location = new Point(ls.X2 + 50, ls.Y2);
            Name = pop.ToString();

            UpdateLevels(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 + ls.X2 - 7, ls.Y1 - 20);
            doStuff(ls.Y1.ToString(), Name, ls.X1.ToString(), ls.X2.ToString());

            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();

            //form.SaveBtn_Click(null,null);
        }
        #endregion

        #region UndrawnLevels Move And Resize
        public static void Undrawnlevels_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = UndrawnGetControl("LevelsRB", ls.X2, ls.Y2, UndrawnName);

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.X1 > ls.X2)
                    {
                        if (e.X <= (ls.X2 + 20))
                        {
                            dragEndPoint = true;
                        }
                        if (e.X >= (ls.X1 - 20))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.X1 < ls.X2)
                    {
                        if (e.X >= ls.X2 - 20)
                        {
                            dragEndPoint = true;
                        }
                        if (e.X <= ls.X1 + 20)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeWE;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeWE;
                    }
                }
            }
        }
        public static void Undrawnlevels_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void Undrawnlevels_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

            Name = string.Empty;
            Name = pop.ToString();
            BFObjID = Name;

            UpdateLevels(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 + ls.X2 - 7, ls.Y1 - 20);

            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawnInternalOrepass Move And Resize
        public static void UndrawnInternalOrepass_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = UndrawnGetControl("InternalOrepassRB", ls.X2, ls.Y2, UndrawnName);

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.X1 > ls.X2)
                    {
                        if (e.X <= (ls.X2 + 20))
                        {
                            dragEndPoint = true;
                        }
                        if (e.X >= (ls.X1 - 20))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.X1 < ls.X2)
                    {
                        if (e.X >= ls.X2 - 20)
                        {
                            dragEndPoint = true;
                        }
                        if (e.X <= ls.X1 + 20)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeWE;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeWE;
                    }
                }
            }
        }
        public static void UndrawnInternalOrepass_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void UndrawnInternalOrepass_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

            Name = string.Empty;
            Name = pop.ToString();
            BFObjID = Name;

            UpdateInternalOrepass(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 + ls.X2 - 7, ls.Y1 - 20);

            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawnOPass Move And Resize
        public static void UndrawnOPass_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = UndrawnGetControl("OrepassRB", ls.X2, ls.Y2, UndrawnName);

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.X1 > ls.X2)
                    {
                        if (e.X <= (ls.X2 + 20))
                        {
                            dragEndPoint = true;
                        }
                        if (e.X >= (ls.X1 - 20))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.X1 < ls.X2)
                    {
                        if (e.X >= ls.X2 - 20)
                        {
                            dragEndPoint = true;
                        }
                        if (e.X <= ls.X1 + 20)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeAll;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeAll;
                    }
                }
            }
        }
        public static void UndrawnOPass_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void UndrawnOPass_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            Name = string.Empty;

            BFObjID = Name;

            l.Location = new Point(ls.X2 + 50, ls.Y2);
            Name = pop.ToString();

            UpdateOrePass(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 + ls.X2 - 7, ls.Y1 - 20);

            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawnSubShafts Move And Resize
        public static void UndrawnSubShafts_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = UndrawnGetControl("SubShaftsRB", ls.X2, ls.Y2, UndrawnName);

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.Y1 > ls.Y2)
                    {
                        if (e.Y <= (ls.Y2 + 5))
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y >= (ls.Y1 - 5))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.Y1 < ls.Y2)
                    {
                        if (e.Y >= ls.Y2 - 5)
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y <= ls.Y1 + 5)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;

                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                }
            }
        }
        public static void UndrawnSubShafts_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void UndrawnSubShafts_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            Name = string.Empty;

            BFObjID = Name;

            l.Location = new Point(ls.X2 + 50, ls.Y2);
            Name = pop.ToString();

            UpdateSubShafts(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 - 31, ls.Y1 - 60, ls.X1 - 31, ls.Y1 - 70);

            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawnTerShafts Move And Resize
        public static void UndrawnTerShafts_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = UndrawnGetControl("TerShaftsRB", ls.X2, ls.Y2, UndrawnName);

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.Y1 > ls.Y2)
                    {
                        if (e.Y <= (ls.Y2 + 5))
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y >= (ls.Y1 - 5))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.Y1 < ls.Y2)
                    {
                        if (e.Y >= ls.Y2 - 5)
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y <= ls.Y1 + 5)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                }
            }
        }
        public static void UndrawnTerShafts_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void UndrawnTerShafts_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            Name = string.Empty;

            l.Location = new Point(ls.X2 + 50, ls.Y2);
            Name = pop.ToString();

            BFObjID = Name;

            UpdateTerShafts(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 - 31, ls.Y1 - 60, ls.X1 - 31, ls.Y1 - 70);
            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawnInclineShafts Move And Resize
        public static void UndrawnInclineShafts_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = UndrawnGetControl("InclineShaftsRB", ls.X2, ls.Y2, UndrawnName);

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.Y1 > ls.Y2)
                    {
                        if (e.Y <= (ls.Y2 + 5))
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y >= (ls.Y1 - 5))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.Y1 < ls.Y2)
                    {
                        if (e.Y >= ls.Y2 - 5)
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y <= ls.Y1 + 5)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                }
            }
        }
        public static void UndrawnInclineShafts_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void UndrawnInclineShafts_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            Name = string.Empty;

            l.Location = new Point(ls.X2 + 50, ls.Y2);
            l.Text = Name;
            Name = pop.ToString();

            BFObjID = Name;

            UpdateInclShafts(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 - 31, ls.Y1 - 60, ls.X1 - 31, ls.Y1 - 70);
            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawnShafts Move And Resize
        public static void UndrawnShafts_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = UndrawnGetControl("ShaftsRB", ls.X2, ls.Y2, UndrawnName);

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.Y1 > ls.Y2)
                    {
                        if (e.Y <= (ls.Y2 + 5))
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y >= (ls.Y1 - 5))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.Y1 < ls.Y2)
                    {
                        if (e.Y >= ls.Y2 - 5)
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y <= ls.Y1 + 5)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                }
            }
        }
        public static void UndrawnShafts_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void UndrawnShafts_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;

            Name = string.Empty;
            BFObjID = Name;

            l.Location = new Point(ls.X2 + 50, ls.Y2);
            l.Text = Name;
            Name = pop.ToString();

            UpdateShafts(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 - 31, ls.Y1 - 60, ls.X1 - 31, ls.Y1 - 70);
            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawnHeadgear Move and resize
        public static void UndrawnHeadGear_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    Point testpoint = new Point(ls.Left, ls.Top);

                    dragimage = MouseIsNearBy(testpoint, f, e);
                    ls.Cursor = Cursors.Hand;
                    pop = UndrawnGetControl("HeadGearRB", ls.Left, ls.Top, UndrawnName);
                }
            }
        }
        public static void UndrawnHeadGear_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void UndrawnHeadGear_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Visible = true;
            l.Size = new Size(30, 15);

            controller.Left = ls.Left;
            controller.Top = ls.Top;

            l.Location = new Point(ls.Left + 50, ls.Top);

            UpdateHeadGear(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            ls.Cursor = Cursors.Default;

            dragimage = false;

            form = new ucDiagram();
        }
        #endregion

        #region UndrawDam Move and resize
        public static void UndrawnDam_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    Point testpoint = new Point(ls.Left, ls.Top);

                    dragimage = MouseIsNearBy(testpoint, f, e);
                    ls.Cursor = Cursors.Hand;
                    pop = UndrawnGetControl("DamRB", ls.Left, ls.Top, UndrawnName);
                }
            }
        }
        public static void UndrawnDam_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void UndrawnDam_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Visible = true;
            l.Size = new Size(30, 15);

            controller.Left = ls.Left;
            controller.Top = ls.Top;

            l.Location = new Point(ls.Left + 50, ls.Top);

            UpdateDam(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            ls.Cursor = Cursors.Default;

            dragimage = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawMill Move and resize
        public static void UndrawnMill_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    Point testpoint = new Point(ls.Left, ls.Top);

                    dragimage = MouseIsNearBy(testpoint, f, e);
                    ls.Cursor = Cursors.Hand;
                    pop = UndrawnGetControl("MillRB", ls.Left, ls.Top, UndrawnName);
                }
            }
        }
        public static void UndrawnMill_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void UndrawnMill_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Visible = true;
            l.Size = new Size(30, 15);

            controller.Left = ls.Left;
            controller.Top = ls.Top;

            l.Location = new Point(ls.Left + 50, ls.Top);

            UpdateMill(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            ls.Cursor = Cursors.Default;

            dragimage = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawPachuca Move and resize
        public static void UndrawnPachuca_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    Point testpoint = new Point(ls.Left, ls.Top);

                    dragimage = MouseIsNearBy(testpoint, f, e);
                    ls.Cursor = Cursors.Hand;
                    pop = UndrawnGetControl("PachucaRB", ls.Left, ls.Top, UndrawnName);
                }
            }
        }
        public static void UndrawnPachuca_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void UndrawnPachuca_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Visible = true;
            l.Size = new Size(50, 15);

            if (isEdit)
            {
                controller.Left = ls.Left;
                controller.Top = ls.Top;
            }

            l.Location = new Point(ls.Left + 50, ls.Top);

            UpdatePachuca(UndrawnName, ls.Left, ls.Top, ls.Width, ls.Height);
            ls.Cursor = Cursors.Default;

            dragimage = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawPlant Move and resize
        public static void UndrawnPlant_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);


                    oldmousex = e.X;
                    oldmousey = e.Y;

                    Point testpoint = new Point(ls.Left, ls.Top);

                    dragimage = MouseIsNearBy(testpoint, f, e);
                    ls.Cursor = Cursors.Hand;
                    pop = UndrawnGetControl("PlantRB", ls.Left, ls.Top, UndrawnName);
                }
            }
        }
        public static void UndrawnPlant_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void UndrawnPlant_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Visible = true;
            l.Size = new Size(30, 15);

            if (isEdit)
            {
                controller.Left = ls.Left;
                controller.Top = ls.Top;
            }

            l.Location = new Point(ls.Left + 50, ls.Top);

            UpdatePlant(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            ls.Cursor = Cursors.Default;

            dragimage = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region UndrawTank Move and resize
        public static void UndrawnTank_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    Point testpoint = new Point(ls.Left, ls.Top);

                    dragimage = MouseIsNearBy(testpoint, f, e);
                    ls.Cursor = Cursors.Hand;
                    pop = UndrawnGetControl("TankRB", ls.Left, ls.Top, UndrawnName);
                }
            }
        }
        public static void UndrawnTank_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void UndrawnTank_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Visible = true;
            l.Size = new Size(30, 15);

            if (isEdit)
            {
                controller.Left = ls.Left;
                controller.Top = ls.Top;
            }

            l.Location = new Point(ls.Left + 50, ls.Top);

            UpdateTank(UndrawnName, ls.Left, ls.Top, ls.Width, ls.Height);
            ls.Cursor = Cursors.Default;

            dragimage = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        public static void doStuff(string YCor, string level, string X1, string X2)
        {
            //Change Form1 to whatever your form is called
            foreach (var frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(ucDiagram))
                {
                    ucDiagram frmTemp = (ucDiagram)frm;
                    frmTemp.addItemToListBox(YCor, level, X1, X2);
                    frmTemp.YcoTxt.Text = YCor;
                    frmTemp.XcoTxt.Text = X1;
                }
            }
        }

        #region InternalOrepass Move And Resize

        public static void InternalOrepass_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
                    var X = sender as TextBox;

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = GetControl("InternalOrepassRB", ls.X1, ls.Y1);

                    Y = ls.Y2.ToString();

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    dragStartPoint = MouseIsNearBy(oldStartPoint, f, e);
                    dragEndPoint = MouseIsNearBy(oldEndPoint, f, e);

                    if (ls.X1 > ls.X2)
                    {
                        if (e.X <= (ls.X2 + 20))
                        {
                            dragEndPoint = true;
                        }
                        if (e.X >= (ls.X1 - 20))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.X1 < ls.X2)
                    {
                        if (e.X >= ls.X2 - 20)
                        {
                            dragEndPoint = true;
                        }
                        if (e.X <= ls.X1 + 20)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeWE;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeWE;
                    }
                }
            }
        }

        public static void InternalOrepass_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
                ls.Cursor = Cursors.Hand;
            }
            else if (dragStartPoint)
            {//works

                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.Cursor = Cursors.SizeWE;
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
                ls.Cursor = Cursors.SizeWE;
            }
        }

        public static void InternalOrepass_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(50, 15);
            l.Visible = true;

            Name = string.Empty;

            l.Location = new Point(ls.X2 + 50, ls.Y2);
            Name = pop.ToString();

            UpdateInternalOrepass(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 + ls.X2 - 7, ls.Y1 - 20);
            doStuff(ls.Y1.ToString(), Name, ls.X1.ToString(), ls.X2.ToString());

            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region HeadGear Move and Resize
        public static void HeadGear_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    Point testpoint = new Point(ls.Left, ls.Top);

                    dragimage = MouseIsNearBy(testpoint, f, e);
                    ls.Cursor = Cursors.Hand;
                    pop = GetControl("HeadGearRB", ls.Left, ls.Top);
                }
            }
        }
        public static void HeadGear_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void HeadGear_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            l.Location = new Point(ls.Left + 50, ls.Top);

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Text = Name;

            if (isEdit)
            {
                controller.Left = ls.Left;
                controller.Top = ls.Top;
            }

            UpdateHeadGear(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            ls.Cursor = Cursors.Default;

            dragimage = false;
        }
        #endregion

        #region Dam Move and Resize
        public static void Dam_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var controller = sender as ShapeContainer;
                Form f = controller.ParentForm;
                RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                oldmousex = e.X;
                oldmousey = e.Y;

                Point testpoint = new Point(ls.Left, ls.Top);

                dragimage = MouseIsNearBy(testpoint, f, e);
                ls.Cursor = Cursors.Hand;
                pop = GetControl("DamRB", ls.Left, ls.Top);
            }
        }
        public static void Dam_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void Dam_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            l.Location = new Point(ls.Left + 50, ls.Top);

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Text = Name;

            if (isEdit)
            {
                controller.Left = ls.Left;
                controller.Top = ls.Top;
            }

            UpdateDam(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            doStuff(ls.Left.ToString(), Name, ls.Top.ToString(), ls.Right.ToString());

            ls.Cursor = Cursors.Default;

            dragimage = false;
        }
        #endregion

        #region Mill Move and Resize
        public static void Mill_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    Point testpoint = new Point(ls.Left, ls.Top);

                    dragimage = MouseIsNearBy(testpoint, f, e);
                    ls.Cursor = Cursors.Hand;
                    pop = GetControl("MillRB", ls.Left, ls.Top);
                }
            }
        }
        public static void Mill_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void Mill_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            l.Location = new Point(ls.Left + 50, ls.Top);

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Text = Name;

            if (isEdit)
            {
                controller.Left = ls.Left;
                controller.Top = ls.Top;
            }

            UpdateMill(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            doStuff(ls.Left.ToString(), Name, ls.Top.ToString(), ls.Right.ToString());

            ls.Cursor = Cursors.Default;

            dragimage = false;
        }
        #endregion

        #region Pachuca Move and Resize
        public static void Pachuca_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var controller = sender as ShapeContainer;
                Form f = controller.ParentForm;
                RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                oldmousex = e.X;
                oldmousey = e.Y;

                Point testpoint = new Point(ls.Left, ls.Top);

                dragimage = MouseIsNearBy(testpoint, f, e);
                ls.Cursor = Cursors.Hand;
                pop = GetControl("PachucaRB", ls.Left, ls.Top);
            }
        }
        public static void Pachuca_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void Pachuca_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(50, 15);
            l.Visible = true;
            l.Location = new Point(ls.Left + 50, ls.Top);

            Name = string.Empty;
            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Text = Name;

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Text = Name;

            if (isEdit)
            {
                controller.Left = ls.Left;
                controller.Top = ls.Top;
            }

            UpdatePachuca(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            doStuff(ls.Left.ToString(), Name, ls.Top.ToString(), ls.Right.ToString());

            ls.Cursor = Cursors.Default;

            ls.Visible = true;
            dragimage = false;
        }
        #endregion

        #region Plant Move and Resize
        public static void Plant_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    Point testpoint = new Point(ls.Left, ls.Top);

                    dragimage = MouseIsNearBy(testpoint, f, e);
                    ls.Cursor = Cursors.Hand;
                    pop = GetControl("PlantRB", ls.Left, ls.Top);
                }
            }
        }
        public static void Plant_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void Plant_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            l.Location = new Point(ls.Left + 50, ls.Top);

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Text = Name;

            controller.Left = ls.Left;
            controller.Top = ls.Top;

            UpdatePlant(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            doStuff(ls.Left.ToString(), Name, ls.Top.ToString(), ls.Right.ToString());

            ls.Cursor = Cursors.Default;

            dragimage = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region OPass Move And Resize
        public static void OPass_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;

                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    pop = GetControl("OrepassRB", ls.X1, ls.Y1);

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.X1 > ls.X2)
                    {
                        if (e.X <= (ls.X2 + 20))
                        {
                            dragEndPoint = true;
                        }
                        if (e.X >= (ls.X1 - 20))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.X1 < ls.X2)
                    {
                        if (e.X >= ls.X2 - 20)
                        {
                            dragEndPoint = true;
                        }
                        if (e.X <= ls.X1 + 20)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeAll;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeAll;
                    }
                }
            }
        }
        public static void OPass_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void OPass_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            l.Location = new Point(ls.X2 + 50, ls.Y2);

            Name = string.Empty;
            Name = pop.ToString();

            BFObjX = ls.X1;
            BFObjY = ls.Y1;
            BFObjX2 = ls.X2;
            BFObjX2 = ls.Y2;
            BFObjID = Name;
            l.Text = Name;

            UpdateOrePass(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 + ls.X2 - 7, ls.Y1 - 20);
            doStuff(ls.Y1.ToString(), Name, ls.X1.ToString(), ls.X2.ToString());

            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;
        }
        #endregion

        #region Shafts Move And Resize
        public static void shafts_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    pop = GetControl("ShaftsRB", ls.X1, ls.Y1);

                    oldmousex = e.X;
                    oldmousey = e.Y;
                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.Y1 > ls.Y2)
                    {
                        if (e.Y <= (ls.Y2 + 5))
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y >= (ls.Y1 - 5))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.Y1 < ls.Y2)
                    {
                        if (e.Y >= ls.Y2 - 5)
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y <= ls.Y1 + 5)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                }
            }
        }
        public static void shafts_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void shafts_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            Name = string.Empty;
            Name = pop.ToString();

            l.Location = new Point(ls.X2 + 50, ls.Y2);

            l.Text = Name;
            BFObjID = Name;

            UpdateShafts(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 - 31, ls.Y1 - 60, ls.X1 - 31, ls.Y1 - 70);
            doStuff(ls.Y1.ToString(), Name, ls.X1.ToString(), ls.X2.ToString());

            ls.Cursor = Cursors.Default;

            dragStartPoint = false;
            dragEndPoint = false;
        }
        #endregion

        #region Subshafts Move And Resize
        public static void subshafts_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    var X = sender as TextBox;

                    pop = GetControl("SubShaftsRB", ls.X1, ls.Y1);

                    oldmousex = e.X;
                    oldmousey = e.Y;
                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.Y1 > ls.Y2)
                    {
                        if (e.Y <= (ls.Y2 + 5))
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y >= (ls.Y1 - 5))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.Y1 < ls.Y2)
                    {
                        if (e.Y >= ls.Y2 - 5)
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y <= ls.Y1 + 5)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                }
            }
        }
        public static void subshafts_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void subshafts_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

            Name = string.Empty;
            Name = pop.ToString();

            UpdateSubShafts(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 - 31, ls.Y1 - 60, ls.X1 - 31, ls.Y1 - 70);
            doStuff(ls.Y1.ToString(), Name, ls.X1.ToString(), ls.X2.ToString());

            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;

            l.Location = new Point(ls.X2 + 50, ls.Y2);

            l.Text = Name;
            BFObjID = Name;

            ls.Cursor = Cursors.Default;

            dragStartPoint = false;
            dragEndPoint = false;
        }
        #endregion

        #region Tershafts Move And Resize
        public static void tershafts_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    pop = GetControl("TerShaftsRB", ls.X1, ls.Y1);

                    oldmousex = e.X;
                    oldmousey = e.Y;
                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.Y1 > ls.Y2)
                    {
                        if (e.Y <= (ls.Y2 + 5))
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y >= (ls.Y1 - 5))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.Y1 < ls.Y2)
                    {
                        if (e.Y >= ls.Y2 - 5)
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y <= ls.Y1 + 5)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                }
            }
        }
        public static void tershafts_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void tershafts_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

            Name = string.Empty;
            Name = pop.ToString();

            UpdateTerShafts(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 - 31, ls.Y1 - 60, ls.X1 - 31, ls.Y1 - 70);
            doStuff(ls.Y1.ToString(), Name, ls.X1.ToString(), ls.X2.ToString());

            ls.Cursor = Cursors.Default;
            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;

            l.Location = new Point(ls.X2 + 50, ls.Y2);

            l.Text = Name;
            BFObjID = Name;

            dragStartPoint = false;
            dragEndPoint = false;

            form = new ucDiagram();
            form.loadDrawpanel();
        }
        #endregion

        #region Inclineshafts Move And Resize
        public static void Inclineshafts_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    pop = GetControl("InclineShaftsRB", ls.X1, ls.Y1);

                    oldmousex = e.X;
                    oldmousey = e.Y;
                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    if (ls.Y1 > ls.Y2)
                    {
                        if (e.Y <= (ls.Y2 + 5))
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y >= (ls.Y1 - 5))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.Y1 < ls.Y2)
                    {
                        if (e.Y >= ls.Y2 - 5)
                        {
                            dragEndPoint = true;
                        }
                        if (e.Y <= ls.Y1 + 5)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                        ls.Cursor = Cursors.Hand;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                        ls.Cursor = Cursors.SizeNS;
                    }
                }
            }
        }
        public static void Inclineshafts_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void Inclineshafts_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

            Name = string.Empty;
            Name = pop.ToString();

            UpdateInclShafts(Name, ls.X1, ls.Y1, ls.X2, ls.Y2, ls.X1 - 31, ls.Y1 - 60, ls.X1 - 31, ls.Y1 - 70);
            doStuff(ls.Y1.ToString(), Name, ls.X1.ToString(), ls.X2.ToString());

            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            l.Location = new Point(ls.X2 + 50, ls.Y2);

            l.Text = Name;
            BFObjID = Name;

            ls.Cursor = Cursors.Default;
            dragStartPoint = false;
            dragEndPoint = false;
        }
        #endregion

        #region Trams Move And Resize
        public static void trams_MouseDown(object sender, MouseEventArgs e)
        {
            if (isEdit)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var controller = sender as ShapeContainer;
                    Form f = controller.ParentForm;
                    LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);

                    oldmousex = e.X;
                    oldmousey = e.Y;

                    oldStartPoint = ls.StartPoint;
                    oldEndPoint = ls.EndPoint;

                    dragStartPoint = MouseIsNearBy(oldStartPoint, f, e);
                    dragEndPoint = MouseIsNearBy(oldEndPoint, f, e);

                    if (ls.X1 > ls.X2)
                    {
                        if (e.X <= (ls.X2 + 20))
                        {
                            dragEndPoint = true;
                        }
                        if (e.X >= (ls.X1 - 20))
                        {
                            dragStartPoint = true;
                        }
                    }
                    else if (ls.X1 < ls.X2)
                    {
                        if (e.X >= ls.X2 - 20)
                        {
                            dragEndPoint = true;
                        }
                        if (e.X <= ls.X1 + 20)
                        {
                            dragStartPoint = true;
                        }
                    }

                    if (dragStartPoint == false & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = true;
                    }
                    else if (dragStartPoint == true & dragEndPoint == false)
                    {
                        dragStartPoint = true;
                        dragEndPoint = false;
                    }
                    else if (dragStartPoint == false & dragEndPoint == true)
                    {
                        dragEndPoint = true;
                        dragStartPoint = false;
                    }
                }
            }
        }
        public static void trams_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            if (dragStartPoint & dragEndPoint)
            {
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
            else if (dragStartPoint)
            {//works
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.StartPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
            }
            else if (dragEndPoint)
            {//works
                ls.EndPoint = new Point(oldStartPoint.X + e.X - oldmousex, oldStartPoint.Y + e.Y - oldmousey);
                ls.EndPoint = new Point(oldEndPoint.X + e.X - oldmousex, oldEndPoint.Y + e.Y - oldmousey);
            }
        }
        public static void trams_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            LineShape ls = (LineShape)controller.Shapes.Owner.Shapes.get_Item(0);
            //UpdateTrams(id, ls.X1, ls.Y1, ls.X2, ls.Y2, (ls.X1 + ls.X2 / 2) - 18, (ls.Y1 + ls.Y2 / 2) - 14);

            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            Name = string.Empty;

            l.Location = new Point(ls.X2 + 50, ls.Y2);
            Name = pop.ToString();

            l.Text = Name;

            BFObjX = ls.X1;
            BFObjY = ls.Y1;
            BFObjX2 = ls.X2;
            BFObjX2 = ls.Y2;
            BFObjID = Name;

            dragStartPoint = false;
            dragEndPoint = false;
        }
        #endregion

        #region Tank Move and Resize
        public static void Tank_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var controller = sender as ShapeContainer;
                Form f = controller.ParentForm;
                RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

                oldmousex = e.X;
                oldmousey = e.Y;

                Point testpoint = new Point(ls.Left, ls.Top);

                dragimage = MouseIsNearBy(testpoint, f, e);
                ls.Cursor = Cursors.Hand;
                pop = GetControl("TankRB", ls.Left, ls.Top);
            }
        }
        public static void Tank_MouseMove(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            if (isEdit)
            {
                if (dragimage)
                {
                    ls.Left = ((oldmousex + e.X) - oldmousex);
                    ls.Top = ((oldmousey + e.Y) - oldmousey);
                    ls.Cursor = Cursors.Hand;
                }
            }
        }
        public static void Tank_MouseUp(object sender, MouseEventArgs e)
        {
            var controller = sender as ShapeContainer;
            RectangleShape ls = (RectangleShape)controller.Shapes.Owner.Shapes.get_Item(0);

            System.Windows.Forms.Label l = new Label();
            l.Size = new Size(30, 15);
            l.Visible = true;
            l.Location = new Point(ls.Left + 50, ls.Top);

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Text = Name;

            Name = string.Empty;

            Name = pop.ToString();

            BFObjX = ls.Left;
            BFObjY = ls.Top;
            BFObjID = Name;

            l.Text = Name;

            if (isEdit)
            {
                controller.Left = ls.Left;
                controller.Top = ls.Top;
            }

            UpdateTank(Name, ls.Left, ls.Top, ls.Width, ls.Height);
            doStuff(ls.Left.ToString(), Name, ls.Top.ToString(), ls.Right.ToString());

            ls.Cursor = Cursors.Default;

            ls.Visible = true;
            dragimage = false;
        }
        #endregion

        public static Boolean MouseIsNearBy(Point testPoint, Form f, MouseEventArgs e)
        {
            testPoint = f.PointToScreen(testPoint);
            if ((testPoint.X - e.X) <= HitTestDelta & ((testPoint.Y - e.Y) <= HitTestDelta))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        public static string GetControl(string entity, int xpos, int ypos)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            if (entity == "LevelsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] = 'lvl' and parentoreflowid = 'S5523' ";
            }
            else if (entity == "ShaftsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] = 'Shaft'  ";
            }
            else if (entity == "SubShaftsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "'  and [oreflowcode] = 'SubShaft'  ";
            }
            else if (entity == "TerShaftsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] = 'TerShaft' ";
            }
            else if (entity == "InclineShaftsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] = 'InclShaft' ";
            }
            else if (entity == "OrepassRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] ='Opass'   ";
            }
            else if (entity == "HeadGearRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] ='HeadGear'  ";
            }
            else if (entity == "DamRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] ='Dam'  ";
            }
            else if (entity == "MillRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] ='Mill'  ";
            }
            else if (entity == "PachucaRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] ='Pachuca'  ";
            }
            else if (entity == "PlantRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] ='Plant'  ";
            }
            else if (entity == "TankRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] ='Tank'  ";
            }
            else if (entity == "InternalOrepassRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where X1 = '" + xpos + "'" + "And Y1 = '" + ypos + "' and [oreflowcode] ='IOrePass'  ";
            }
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {
                Name = _dbMan.ResultsDataTable.Rows[0]["BFID"].ToString();

                ObjCoordinates.OreflowCode = _dbMan.ResultsDataTable.Rows[0]["OreFlowCode"].ToString();
                ObjCoordinates.OreflowID = _dbMan.ResultsDataTable.Rows[0]["BFID"].ToString();
                ObjCoordinates.X1Coord = Convert.ToInt32(_dbMan.ResultsDataTable.Rows[0]["X1"].ToString());
                ObjCoordinates.Y1Coord = Convert.ToInt32(_dbMan.ResultsDataTable.Rows[0]["Y1"].ToString());
                if (_dbMan.ResultsDataTable.Rows[0]["X2"].ToString() != string.Empty)
                {
                    ObjCoordinates.X2Coord = Convert.ToInt32(_dbMan.ResultsDataTable.Rows[0]["X2"].ToString());
                }
                else
                {
                    ObjCoordinates.X2Coord = 0;
                }
                if (_dbMan.ResultsDataTable.Rows[0]["Y2"].ToString() != string.Empty)
                {
                    ObjCoordinates.Y2Coord = Convert.ToInt32(_dbMan.ResultsDataTable.Rows[0]["Y2"].ToString());
                }
                else
                {
                    ObjCoordinates.Y2Coord = 0;
                }
            }
            return Name;
        }

        public static string UndrawnGetControl(string entity, int xpos, int ypos, string Item)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            if (entity == "LevelsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] = '" + Item + "' ";
            }
            else if (entity == "ShaftsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] = '" + Item + "'";
            }
            else if (entity == "SubShaftsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where  [oreflowid] = '" + Item + "'";
            }
            else if (entity == "TerShaftsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where  [oreflowid] = '" + Item + "'";
            }
            else if (entity == "InclineShaftsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where  [oreflowid] = '" + Item + "'";
            }
            else if (entity == "TramsRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where  [oreflowid] ='" + Item + "'";
            }
            else if (entity == "MillRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities Where  [oreflowid] ='" + Item + "' ";
            }
            else if (entity == "OrepassRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] ='" + Item + "' ";
            }
            else if (entity == "HeadGearRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] ='" + Item + "' ";
            }
            else if (entity == "DamRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] ='" + Item + "' ";
            }
            else if (entity == "PachucaRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] ='" + Item + "' ";
            }
            else if (entity == "PlantRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] ='" + Item + "' ";
            }
            else if (entity == "TankRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] ='" + Item + "' ";
            }
            else if (entity == "PlantRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] ='" + Item + "' ";
            }
            else if (entity == "InternalOrepassRB")
            {
                _dbMan.SqlStatement = " Select oreflowid BFID ,* from tbl_OreFlowEntities  Where  [oreflowid] ='" + Item + "' ";
            }
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();

            if (_dbMan.ResultsDataTable.Rows.Count > 0)
            {
                Name = _dbMan.ResultsDataTable.Rows[0]["BFID"].ToString();
            }

            return Name;
        }

        public static void UpdateLevels(string ID, int xpos, int ypos, int x2pos, int y2pos, int lblx, int lbly)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X2 = '" + x2pos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y2 = '" + ypos + "'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Lvl' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateInternalOrepass(string ID, int xpos, int ypos, int x2pos, int y2pos, int lblx, int lbly)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X2 = '" + x2pos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y2 = '" + y2pos + "'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'IOrePass' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateShafts(string ID, int xpos, int ypos, int x2pos, int y2pos, int picx, int picy, int lblx, int lbly)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X2 = '" + x2pos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y2 = '" + y2pos + "'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Shaft'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateSubShafts(string ID, int xpos, int ypos, int x2pos, int y2pos, int picx, int picy, int lblx, int lbly)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X2 = '" + x2pos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y2 = '" + y2pos + "'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "'  and oreflowcode = 'SubShaft'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateTerShafts(string ID, int xpos, int ypos, int x2pos, int y2pos, int picx, int picy, int lblx, int lbly)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X2 = '" + x2pos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y2 = '" + y2pos + "'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'TerShaft'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateInclShafts(string ID, int xpos, int ypos, int x2pos, int y2pos, int picx, int picy, int lblx, int lbly)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X2 = '" + x2pos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y2 = '" + y2pos + "'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'InclShaft' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateOrePass(string ID, int xpos, int ypos, int x2pos, int y2pos, int lblx, int lbly)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X2 = '" + x2pos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y2 = '" + y2pos + "'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'OPass' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateHeadGear(string ID, int xpos, int ypos, int x2pos, int y2pos)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateDam(string ID, int xpos, int ypos, int x2pos, int y2pos)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0' ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Dam'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateMill(string ID, int xpos, int ypos, int x2pos, int y2pos)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Mill'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdatePachuca(string ID, int xpos, int ypos, int x2pos, int y2pos)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid  = '" + ID + "' and oreflowcode = 'Pachuca'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdatePlant(string ID, int xpos, int ypos, int x2pos, int y2pos)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Plant'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void UpdateTank(string ID, int xpos, int ypos, int x2pos, int y2pos)
        {
            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "X1 = '" + xpos + "', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Y1 = '" + ypos + "'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " , Inactive = '0'";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Tank'";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteOrePass()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "Inactive = '1', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'OPass' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteInternalOrePass()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + "Inactive = '1', ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'IOrePass' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteLevels()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid =  '" + ID + "' and oreflowcode = 'Lvl' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteShaft()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Shaft' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteSubShaft()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'SubShaft' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteTerShaft()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'TerShaft' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteInclShaft()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'InclShaft' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteDam()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Dam' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteMill()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Mill' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeletePlant()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Plant' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeletePachuca()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Pachuca' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

        public static void DeleteTank()
        {
            string ID = string.Empty;
            ID = Name;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(_Systemtag, _Userconnection);
            _dbMan.SqlStatement = _dbMan.SqlStatement + "update tbl_OreFlowEntities set ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " Inactive = '1'  ";
            _dbMan.SqlStatement = _dbMan.SqlStatement + " where oreflowid = '" + ID + "' and oreflowcode = 'Tank' ";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.longNumber;
            _dbMan.ExecuteInstruction();
        }

    }
}
