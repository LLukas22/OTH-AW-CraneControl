using System.Drawing;
using System.Windows.Forms;

namespace CraneControl
{
    public class CustomProgressBar : PictureBox
    {
        public CustomProgressBar()
        {
            BorderStyle = BorderStyle.FixedSingle;
        }

        public int min { get; set; }
        public int max { get; set; }

        public Brush progressBrush { get; set; }
        public Font progressFont { get; set; }
        public Brush progressFontBrush { get; set; }

        public void Refresh(int currentValue)
        {
            var bmp = new Bitmap(Width, Height);
            using (var graphic = Graphics.FromImage(bmp))
            {
                graphic.Clear(Color.Transparent);
                graphic.FillRectangle(progressBrush, new Rectangle(0, 0, currentValue * Width / 100, Height));
                var width = (int)(Width / 2 - graphic.MeasureString($"{currentValue}", progressFont).Width / 2);
                var height = Height / 2 - progressFont.Height / 2;
                graphic.DrawString($"{currentValue}", progressFont, progressFontBrush, width, height);
                Image = bmp;
            }
        }
    }
}