using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Bodoconsult.Core.Drawing.Helpers;

namespace Bodoconsult.Core.Drawing
{
    public class ImageWriter : IDisposable
    {

        private string _sourceFile;
        private Bitmap _masterImage;
        private Bitmap _image;

        //public ImageWriter()
        //{

        //}

        /// <summary>
        /// Write text data into an image
        /// </summary>
        /// <param name="iwd"></param>
        public void WriteText(ImageWriterData iwd)
        {
            try
            {

                // Create string to draw.
                String drawString = iwd.Text;

                // Create font and brush.
                Font drawFont = new Font(iwd.FontName, iwd.FontSize, (FontStyle)iwd.FontStyle);
                SolidBrush drawBrush = new SolidBrush(iwd.TextColor);

                // Create point for upper-left corner of drawing.
                PointF drawPoint = new PointF(150.0F, 150.0F);

                var rect = new Rectangle(iwd.X, iwd.Y, iwd.Width, iwd.Height);

                // Draw string to screen.


                using (var graphics = Graphics.FromImage(_image))
                {
                    graphics.DrawString(drawString, drawFont, drawBrush, rect);

                }



            //    _imaging.DrawTextBox(_image, iwd.Text, iwd.X, iwd.Y, iwd.Width, iwd.Height, iwd.FontSize, TextAlignment.TextAlignmentNear, (FontStyle)iwd.FontStyle,
            //_imaging.ARGB(iwd.TextColor.A, iwd.TextColor.R, iwd.TextColor.G, iwd.TextColor.B), iwd.FontName, false, true);
            }
            catch (Exception ex)
            {
                throw new Exception($"{TargetFile}", ex);
            }
        }





        /// <summary>
        /// Target file to save the resulting image
        /// </summary>
        public string TargetFile { get; set; }

        /// <summary>
        /// Source image file for the ImageWriter
        /// </summary>
        public string SourceFile
        {
            get => _sourceFile;
            set
            {
                try
                {
                    _sourceFile = value;
                    _masterImage = new Bitmap(_sourceFile);
                }
                catch (Exception ex)
                {
                    throw new Exception($"{_sourceFile}", ex);
                }
            }
        }

        /// <summary>
        ///  Create a new image from the source file
        /// </summary>
        public void NewImage()
        {
            try
            {
                _image?.Dispose();
                _image = _masterImage.Clone(new Rectangle(0, 0, _masterImage.Width, _masterImage.Height), PixelFormat.Format32bppRgb);
            }
            catch (Exception ex)
            {
                throw new Exception($"{TargetFile}", ex);
            }
        }

        /// <summary>
        /// Save the current image to target path
        /// </summary>
        public void Save(uint compressionLevel = 80)
        {
            try
            {
                if (File.Exists(TargetFile)) File.Delete(TargetFile);

                switch (TargetFile.Substring(TargetFile.Length - 4).ToLower())
                {
                    case ".png":
                        ImageHelper.SaveAsPng(_image, TargetFile);
                        break;
                    default:
                        ImageHelper.SaveAsJpeg(_image, TargetFile, compressionLevel);
                        break;
                }

                _image.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception($"{TargetFile}", ex);
            }
        }

        public void Dispose()
        {
            try
            {

                _masterImage.Dispose();
                _image.Dispose();
            }
            catch
            {
                // ignored
            }
        }
    }
}
