using System.Drawing;

namespace Bodoconsult.Core.Drawing
{
    public class ImageWriterData
    {
        public int Width;
        public int Height;

        public ImageWriterData()
        {
            FontSize = 12;
            FontName = "Arial";
            FontStyle = 0;
            TextColor = Color.Black;
        }

        /// <summary>
        /// Name of the font to use, i.e. "Arial", "Times New Roman", ...
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Font size in pt
        /// </summary>
        public float FontSize { get; set; }

        /// <summary>
        /// Text to write
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Font style. Use the foloowing numbers:
        /// 0= Regular,
        /// 1= Bold,
        /// 2=Italic,
        /// 3= BoldItalic,
        /// 4= Underline,
        /// 5= BoldUnderline,
        /// 6= ItalicUnderline,
        /// 7= BoldItalicUnderline,
        /// 8= Strikeout,
        /// 9= BoldStrikeout,
        /// 10= ItalicStrikeout,
        /// 11= BoldItalicStrikeout,
        /// 12= UnderlineStrikeout,
        /// 13= BoldUnderlineStrikeout,
        /// 14= ItalicUnderlineStrikeout,
        /// 15= BoldItalicUnderlineStrikeout,
        /// </summary>
        public int FontStyle { get; set; }


        /// <summary>
        /// x coordinate in the image to the text in
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// x coordinate in the image to the text in
        /// </summary>
        public int Y { get; set; }


        public Color TextColor { get; set; }
    }
}
