using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    class ControlMover
    {
        public static int pop;
        public static string ss;
        public static int startx;
        public static int starty;

        // public static Bitmap B;
        //  public static Panel P;
        public enum Direction
        {
            Any,
            Horizontal,
            Vertical
        }

        public static void Init(Control control, DevExpress.XtraEditors.PanelControl p, Bitmap b)
        {
            Init(control, Direction.Any, p, b);
        }

        public static void Init(Control control, Direction direction, DevExpress.XtraEditors.PanelControl p, Bitmap b)
        {
            Init(control, control, direction, p, b);
        }

        public static void Init(Control control, Control container, Direction direction, DevExpress.XtraEditors.PanelControl p, Bitmap b)
        {
            bool Dragging = false;
            Point DragStart = Point.Empty;
            control.MouseDown += delegate (object sender, MouseEventArgs e)
            {
                Dragging = true;
                startx = control.Location.X;
                starty = control.Location.Y;
                ss = string.Empty;
                if (control.Tag != null)
                {
                    // pop = PAS.drawingClass.GetControl(control.Tag.ToString(), startx, starty);
                }

                DragStart = new Point(e.X, e.Y);
                control.Capture = true;
            };
            control.MouseUp += delegate (object sender, MouseEventArgs e)
            {
                Dragging = false;
                control.Capture = false;
                if (control.Tag != null)
                {
                    if (control.Tag.ToString() == "Mills")
                    {
                        //  PAS.drawingClass.UpdateMills(pop, control.Location.X, control.Location.Y, control.Location.X, control.Location.Y - 10);

                    }
                    else if (control.Tag.ToString() == "Orepass")
                    {
                        //  PAS.drawingClass.UpdateOrepass(pop, control.Location.X, control.Location.Y, 0, 0);

                    }
                    else if (control.Tag.ToString() == "Orebins")
                    {
                        //  PAS.drawingClass.UpdateOrebin(pop, control.Location.X, control.Location.Y, 0, 0);
                        p.Refresh();

                    }
                    else if (control.Tag.ToString() == "Boxholes")
                    {
                        //PAS.drawingClass.UpdateBoxholes(pop, control.Location.X, control.Location.Y, 0, 0);

                    }
                    else if (control.Tag.ToString() == "Car1")
                    {
                        //PAS.drawingClass.UpdateMiningLevelCars(pop, control.Location.X, control.Location.Y, control.Location.X + 5, control.Location.Y - 12);
                        //PAS.drawingClass.LoadCar1(p);
                        p.Invalidate();
                    }
                    else if (control.Tag.ToString() == "Car2")
                    {
                        //PAS.drawingClass.UpdateMiningLevelCars(pop, control.Location.X, control.Location.Y, control.Location.X + 5, control.Location.Y - 12);
                        //PAS.drawingClass.LoadCar2(p);
                    }
                    else if (control.Tag.ToString() == "Car3")
                    {
                        //PAS.drawingClass.UpdateMiningLevelCars(pop, control.Location.X, control.Location.Y, control.Location.X + 5, control.Location.Y - 12);
                        //PAS.drawingClass.LoadCar3(p);
                    }
                    else if (control.Tag.ToString() == "Car4")
                    {
                        //PAS.drawingClass.UpdateMiningLevelCars(pop, control.Location.X, control.Location.Y, control.Location.X + 5, control.Location.Y - 12);
                        //PAS.drawingClass.LoadCar4(p);
                    }
                    else if (control.Tag.ToString() == "Car5")
                    {
                        // PAS.drawingClass.UpdateMiningLevelCars(pop, control.Location.X, control.Location.Y, control.Location.X + 5, control.Location.Y - 12);
                        // PAS.drawingClass.LoadCar5(p);
                    }

                }

            };
            control.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                if (Dragging)
                {
                    if (direction != Direction.Vertical)
                        container.Left = Math.Max(0, e.X + container.Left - DragStart.X);
                    if (direction != Direction.Horizontal)
                        container.Top = Math.Max(0, e.Y + container.Top - DragStart.Y);
                }
            };
        }
    }
}
