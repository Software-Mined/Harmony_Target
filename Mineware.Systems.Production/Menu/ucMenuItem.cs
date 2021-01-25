using System.Drawing;
using System.Windows.Forms;

namespace Mineware.Systems.Production.Menu
{
    public partial class ucMenuItem : UserControl
    {
        #region Fields and Properties
        /// <summary>
        /// The colour of the background of the control
        /// </summary>
        public Color BackGroundColor { set { this.BackColor = value; } }

        /// <summary>
        /// The size of the menu item
        /// </summary>
        public MenuItemSize SizeOfItem
        {
            set
            {
                switch (value)
                {
                    case MenuItemSize.ExtraSmall:
                        this.Size = new Size(64, 64);
                        lblMain.Visible = false;
                        break;
                    case MenuItemSize.Small:
                        this.Size = new Size(128, 128);
                        lblMain.Visible = false;
                        break;
                    case MenuItemSize.Medium:
                        this.Size = new Size(256, 128);
                        lblMain.Visible = true;
                        break;
                    case MenuItemSize.Large:
                        this.Size = new Size(512, 256);
                        lblMain.Visible = true;
                        break;
                }
            }
        }

        /// <summary>
        /// The image to be displayed
        /// </summary>
        public Image Icon { set { picMain.BackgroundImage = value; } }

        /// <summary>
        /// The text to be displayed on the label
        /// </summary>
        public string LabelText
        {
            set
            {
                lblMain.Text = value;
                ToolTip tt = new ToolTip();
                tt.SetToolTip(picMain, value);
            }
        }

        /// <summary>
        /// The color of the text
        /// </summary>
        public Color LabelTextColor { set { lblMain.ForeColor = value; } }

        public new System.EventHandler OnClick { set { picMain.Click += new System.EventHandler(value); } }
        #endregion Fields and Properties

        #region Constructor
        /// <summary>
        /// A menu item for the planning menu
        /// </summary>
        public ucMenuItem()
        {
            InitializeComponent();
        }
        #endregion Constructor
    }
}