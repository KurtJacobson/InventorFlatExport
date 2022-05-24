using System;
using System.Drawing;
using System.Windows.Forms;

namespace InventorFlatExport.Custom_Controls
{

    public partial class ColorComboBox : ComboBox
    {

        private ColorDialog colorChooser = null;


        // Data for each color in the list
        public class ColorInfo
        {
            public string Text { get; set; }
            public Color Color { get; set; }

            public ColorInfo(string text, Color color)
            {
                Text = text;
                Color = color;
            }
        }

        public ColorComboBox()
        {
            InitializeComponent();

            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += OnDrawItem;
            SelectionChangeCommitted += OnSelectionChangeCommitted;

            // Initialize colors
            this.AddStandardColors();
        }

        // Populate control with standard colors
        public void AddStandardColors()
        {
            Items.Clear();
            Items.Add(new ColorInfo("Black", Color.Black));
            Items.Add(new ColorInfo("Blue", Color.Blue));
            Items.Add(new ColorInfo("Lime", Color.Lime));
            Items.Add(new ColorInfo("Cyan", Color.Cyan));
            Items.Add(new ColorInfo("Red", Color.Red));
            Items.Add(new ColorInfo("Fuchsia", Color.Fuchsia));
            Items.Add(new ColorInfo("Yellow", Color.Yellow));
            Items.Add(new ColorInfo("White", Color.White));
            Items.Add(new ColorInfo("Navy", Color.Navy));
            Items.Add(new ColorInfo("Green", Color.Green));
            Items.Add(new ColorInfo("Teal", Color.Teal));
            Items.Add(new ColorInfo("Maroon", Color.Maroon));
            Items.Add(new ColorInfo("Purple", Color.Purple));
            Items.Add(new ColorInfo("Olive", Color.Olive));
            Items.Add(new ColorInfo("Gray", Color.Gray));
            Items.Add(new ColorInfo("Custom Color...", SystemColors.Window));
        }

        // Draw list item
        protected void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                // Get this color
                ColorInfo color = (ColorInfo)Items[e.Index];

                // Fill background
                e.DrawBackground();

                // Draw color box
                Rectangle rect = new Rectangle();
                rect.X = e.Bounds.X + 2;
                rect.Y = e.Bounds.Y + 2;
                rect.Width = 18;
                rect.Height = e.Bounds.Height - 5;
                e.Graphics.FillRectangle(new SolidBrush(color.Color), rect);
                e.Graphics.DrawRectangle(SystemPens.WindowText, rect);

                // Write color name
                Brush brush;
                if ((e.State & DrawItemState.Selected) != DrawItemState.None)
                    brush = SystemBrushes.HighlightText;
                else
                    brush = SystemBrushes.WindowText;
                e.Graphics.DrawString(color.Text, Font, brush,
                    e.Bounds.X + rect.X + rect.Width + 2,
                    e.Bounds.Y + ((e.Bounds.Height - Font.Height) / 2));

                // Draw the focus rectangle if appropriate
                if ((e.State & DrawItemState.NoFocusRect) == DrawItemState.None)
                    e.DrawFocusRectangle();
            }
        }

        public new ColorInfo SelectedItem
        {
            get
            {
                return (ColorInfo)base.SelectedItem;
            }
            set
            {
                base.SelectedItem = value;
            }
        }

        public new string SelectedText
        {
            get
            {
                if (SelectedIndex >= 0)
                    return SelectedItem.Text;
                return String.Empty;
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (((ColorInfo)Items[i]).Text == value)
                    {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        public new Color SelectedValue
        {
            get
            {
                if (SelectedIndex >= 0)
                    return SelectedItem.Color;
                return Color.White;
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (((ColorInfo)Items[i]).Color == value)
                    {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void OnSelectionChangeCommitted(object sender, EventArgs e)
        {

            if (SelectedItem.Text == "Custom Color...")
            {
                this.colorChooser = new ColorDialog();
                this.colorChooser.ShowDialog();
            }
        }

    }

}



