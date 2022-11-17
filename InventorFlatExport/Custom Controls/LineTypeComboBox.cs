using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace InventorFlatExport.Custom_Controls
{

    public partial class LineTypeComboBox : ComboBox
    {

        // Data for each line type in the list
        public class LineInfo
        {
            public string Text { get; set; }
            public DashStyle Style { get; set; }

            public LineInfo(string text, DashStyle style)
            {
                Text = text;
                Style = style;
            }
        }

        public LineTypeComboBox()
        {
            InitializeComponent();

            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += OnDrawItem;
            SelectionChangeCommitted += OnSelectionChangeCommitted;

            // Initialize options
            this.PopulateLineStyles();
        }

        // Populate control with line style options
        public void PopulateLineStyles()
        {
            Items.Clear();
            Items.Add(new LineInfo("Continuous", DashStyle.Solid));
            Items.Add(new LineInfo("Dashed", DashStyle.Dash));
            Items.Add(new LineInfo("Dash Dot", DashStyle.DashDot));
            Items.Add(new LineInfo("Dash Double Dot", DashStyle.DashDotDot));
        }

        // Draw list item
        protected void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                // Get this line style
                LineInfo lineStyle = (LineInfo)Items[e.Index];

                // Fill background
                e.DrawBackground();

                // Draw line style
                Point p1 = new Point(e.Bounds.Left + 5, e.Bounds.Y + e.Bounds.Height / 2);
                Point p2 = new Point(e.Bounds.Left + 70, e.Bounds.Y + e.Bounds.Height / 2);

                using (Pen linePen = new Pen(e.ForeColor, 1))
                {

                    float[] dashLengths = { 10, 1, 1, 1, };
                    linePen.DashPattern = dashLengths;
                    //DashDot.DashStyle = lineStyle.Style;
                    e.Graphics.DrawLine(linePen, p1, p2);
                }

                // Write line style name
                Brush brush;
                if ((e.State & DrawItemState.Selected) != DrawItemState.None)
                    brush = SystemBrushes.HighlightText;
                else
                    brush = SystemBrushes.WindowText;

                e.Graphics.DrawString(lineStyle.Text, Font, brush,
                    e.Bounds.X + p2.X + 2,
                    e.Bounds.Y + ((e.Bounds.Height - Font.Height) / 2));

                // Draw the focus rectangle if appropriate
                if ((e.State & DrawItemState.NoFocusRect) == DrawItemState.None)
                    e.DrawFocusRectangle();
            }
        }

        public new LineInfo SelectedItem
        {
            get
            {
                return (LineInfo)base.SelectedItem;
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
                    if (((LineInfo)Items[i]).Text == value)
                    {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        public new DashStyle SelectedValue
        {
            get
            {
                if (SelectedIndex >= 0)
                    return SelectedItem.Style;
                return DashStyle.Solid;
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (((LineInfo)Items[i]).Style == value)
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
                return;
            }
        }

    }

}
