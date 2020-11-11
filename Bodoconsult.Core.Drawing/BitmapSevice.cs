using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Bodoconsult.Core.Drawing
{

    /// <summary>
    /// Service class for working with bitmap images
    /// </summary>
    public class BitmapSevice : IDisposable
    {

        /// <summary>
        /// Current bitmap
        /// </summary>
        public Bitmap Bitmap { get; set; }



        /// <summary>
        /// Load a bitmap from a file
        /// </summary>
        /// <param name="fullPath">full file name of the source file to load</param>
        public void LoadBitmap(string fullPath)
        {
            Bitmap = new Bitmap(fullPath);
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Bitmap?.Dispose();
        }

        #region Resize image

        /// <summary>
        /// Resize the image
        /// </summary>
        /// <param name="maxWidth">Maximum with of the new image</param>
        /// <param name="maxHeight">Maximum height of the new image</param>
        /// <param name="imageResolution">Target resolution for the image</param>
        public void ResizeImage(int maxWidth, int maxHeight, float imageResolution = 72)
        {
            int newWidth;
            int newHeight;

            //first we check if the image needs rotating (eg phone held vertical when taking a picture for example)
            foreach (var prop in Bitmap.PropertyItems)
            {
                if (prop.Id == 0x0112)
                {
                    int orientationValue = Bitmap.GetPropertyItem(prop.Id).Value[0];
                    var rotateFlipType = GetRotateFlipType(orientationValue);
                    Bitmap.RotateFlip(rotateFlipType);
                    break;
                }
            }

            //check if the with or height of the image exceeds the maximum specified, if so calculate the new dimensions
            if (Bitmap.Width > maxWidth || Bitmap.Height > maxHeight)
            {
                var ratioX = (double)maxWidth / Bitmap.Width;
                var ratioY = (double)maxHeight / Bitmap.Height;
                var ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(Bitmap.Width * ratio);
                newHeight = (int)(Bitmap.Height * ratio);
            }
            else
            {
                newWidth = Bitmap.Width;
                newHeight = Bitmap.Height;
            }

            //start the resize with a new image
            var newImage = new Bitmap(newWidth, newHeight);

            //set the new resolution
            //newImage.SetResolution(imageResolution, imageResolution);

            //start the resizing
            using (var graphics = Graphics.FromImage(newImage))
            {
                //set some encoding specs
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(Bitmap, 0, 0, newWidth, newHeight);
            }

            Bitmap.Dispose();
            Bitmap = newImage;

        }

        //=== image padding
        public Image ApplyPaddingToImage(Image image, Color backColor)
        {
            //get the maximum size of the image dimensions
            var maxSize = Math.Max(image.Height, image.Width);
            var squareSize = new Size(maxSize, maxSize);

            //create a new square image
            var squareImage = new Bitmap(squareSize.Width, squareSize.Height);

            using (var graphics = Graphics.FromImage(squareImage))
            {
                //fill the new square with a color
                graphics.FillRectangle(new SolidBrush(backColor), 0, 0, squareSize.Width, squareSize.Height);

                //put the original image on top of the new square
                graphics.DrawImage(image, (squareSize.Width / 2) - (image.Width / 2), (squareSize.Height / 2) - (image.Height / 2), image.Width, image.Height);
            }

            //return the image
            return squareImage;
        }


        //=== get encoder info
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();

            for (var j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType.ToLower() == mimeType.ToLower())
                {
                    return encoders[j];
                }
            }

            return null;
        }


        //=== determine image rotation
        private RotateFlipType GetRotateFlipType(int rotateValue)
        {
            var flipType = RotateFlipType.RotateNoneFlipNone;

            switch (rotateValue)
            {
                case 1:
                    flipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    flipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    flipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    flipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    flipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    flipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    flipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    flipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    flipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return flipType;
        }


        ////== convert image to base64
        //public string ConvertImageToBase64(Image image)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        //convert the image to byte array
        //        image.Save(ms, ImageFormat.Jpeg);
        //        var bin = ms.ToArray();

        //        //convert byte array to base64 string
        //        return Convert.ToBase64String(bin);
        //    }
        //}

        #endregion


        /// <summary>
        /// Adjust brightness, contrast and gamma
        /// </summary>
        /// <param name="brightness">Brightness factor: default 1F means not change </param>
        /// <param name="contrast">Contrast factor: default 1F means not change</param>
        /// <param name="gamma">Gamma factor: default 1F means not change</param>
        public void AdjustBcg(float brightness = 1F, float contrast = 1F, float gamma = 1F)
        {

            var adjustedImage = new Bitmap(Bitmap.Width, Bitmap.Height);


            var adjustedBrightness = brightness - 1.0f;


            // create matrix that will brighten and contrast the image
            float[][] ptsArray =
            {
                new[] {contrast, 0, 0, 0, 0}, // scale red
                new[] {0, contrast, 0, 0, 0}, // scale green
                new[] {0, 0, contrast, 0, 0}, // scale blue
                new[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                new[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}
            };

            var imageAttributes = new ImageAttributes();
            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
            using (var g = Graphics.FromImage(adjustedImage))
            {
                g.DrawImage(Bitmap, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height)
                    , 0, 0, Bitmap.Width, Bitmap.Height,
                    GraphicsUnit.Pixel, imageAttributes);
            }

            Bitmap.Dispose();
            Bitmap = adjustedImage;
        }

        /// <summary>
        /// Adjust the saturation of an image.
        /// </summary>
        /// <remarks>Based on https://www.codeproject.com/Tips/78995/Image-colour-manipulation-with-ColorMatrix</remarks>
        /// <param name="saturation">Satuation factor from -1F til +1F. -1F means complement colors. Around 0.5F means a faded image.</param>
        public void AdjustSaturation(float saturation)
        {
            var rWeight = 0.3086f;
            var gWeight = 0.6094f;
            var bWeight = 0.0820f;

            var a = (1.0f - saturation) * rWeight + saturation;
            var b = (1.0f - saturation) * rWeight;
            var c = (1.0f - saturation) * rWeight;
            var d = (1.0f - saturation) * gWeight;
            var e = (1.0f - saturation) * gWeight + saturation;
            var f = (1.0f - saturation) * gWeight;
            var g = (1.0f - saturation) * bWeight;
            var h = (1.0f - saturation) * bWeight;
            var i = (1.0f - saturation) * bWeight + saturation;

            // Create a Graphics

            var bmp = (Bitmap)Bitmap.Clone();


            using (var gr = Graphics.FromImage(Bitmap))
            {
                // ColorMatrix elements
                float[][] ptsArray = {
                                     new[] {a,  b,  c,  0, 0},
                                     new[] {d,  e,  f,  0, 0},
                                     new[] {g,  h,  i,  0, 0},
                                     new float[] {0,  0,  0,  1, 0},
                                     new float[] {0, 0, 0, 0, 1}
                                 };
                // Create ColorMatrix
                var clrMatrix = new ColorMatrix(ptsArray);
                // Create ImageAttributes
                var imgAttribs = new ImageAttributes();
                // Set color matrix
                imgAttribs.SetColorMatrix(clrMatrix,
                    ColorMatrixFlag.Default,
                    ColorAdjustType.Default);
                // Draw Image with no effects
                gr.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                // Draw Image with image attributes
                gr.DrawImage(bmp,
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, bmp.Width, bmp.Height,
                    GraphicsUnit.Pixel, imgAttribs);
            }

            bmp.Dispose();
        }


        public void RoundCorners(int cornerRadius, Color backcolor, int BorderWidth, Color BorderColor, bool shadow = false, int shadowOffset=15)
        {
            cornerRadius *= 2;

            //cornerRadius = 0;

            //var roundedImage = new Bitmap(Bitmap.Width, Bitmap.Height);
            var roundedImage = (Bitmap)Bitmap.Clone();

            GraphicsPath gp = null;

            if (cornerRadius > 0)
            {
                gp = new GraphicsPath();
                gp.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
                gp.AddArc(0 + roundedImage.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
                gp.AddArc(0 + roundedImage.Width - cornerRadius, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                gp.AddArc(0, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                gp.CloseFigure();
            }


            using (var g = Graphics.FromImage(roundedImage))
            {

                // Rounded corners
                g.SmoothingMode = SmoothingMode.HighQuality;

                if (cornerRadius > 0) g.SetClip(gp);


                //if (BorderWidth > 0)
                //{
                //    if (cornerRadius > 0)
                //    {
                var gp1 = new GraphicsPath();
                gp1.AddArc(1, 1, cornerRadius, cornerRadius, 180, 90);
                gp1.AddArc(1 + roundedImage.Width - cornerRadius - 2, 1, cornerRadius, cornerRadius, 270, 90);
                gp1.AddArc(1 + roundedImage.Width - cornerRadius - 2, 1 + roundedImage.Height - cornerRadius - 2, cornerRadius, cornerRadius, 0, 90);
                gp1.AddArc(1, 1 + roundedImage.Height - cornerRadius - 2, cornerRadius, cornerRadius, 90, 90);
                gp1.CloseFigure();
                g.DrawPath(new Pen(BorderColor, BorderWidth), gp1);
                //}
                //else
                //{
                //    var gp1 = new GraphicsPath();
                //    gp1.AddRectangle(new Rectangle(1, 1, roundedImage.Width - 2, roundedImage.Height - 4));
                //    gp1.CloseFigure();
                //    g.DrawPath(new Pen(BorderColor, BorderWidth), gp1);
                //}

                //}
            }

            var roundedImageEnd = new Bitmap(Bitmap.Width, Bitmap.Height);

            using (var g = Graphics.FromImage(roundedImageEnd))
            {
                g.FillRectangle(new SolidBrush(backcolor), new Rectangle(0, 0, Bitmap.Width, Bitmap.Height));



                if (cornerRadius > 0) g.SetClip(gp);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(roundedImage, new Rectangle(0, 0, Bitmap.Width, Bitmap.Height),
                    new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), GraphicsUnit.Pixel);

            }

            //roundedImageEnd.MakeTransparent(Color.LightGray);
            roundedImage.Dispose();

            if (!shadow)
            {
                
                Bitmap.Dispose();
                Bitmap = roundedImageEnd;
                return;
            }



            var shadowedImageEnd = new Bitmap(roundedImageEnd.Width + shadowOffset,
                roundedImageEnd.Height+ shadowOffset);
            using (var g = Graphics.FromImage(shadowedImageEnd))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                // Fill Background
                g.FillRectangle(new SolidBrush(backcolor), new Rectangle(0, 0, shadowedImageEnd.Width, shadowedImageEnd.Height));

                var left = shadowOffset;
                var top = shadowOffset;

                var gp1 = new GraphicsPath();
                gp1.AddArc(left, top, cornerRadius, cornerRadius, 180, 90);
                gp1.AddArc(left + roundedImageEnd.Width - cornerRadius, top, cornerRadius, cornerRadius, 270, 90);
                gp1.AddArc(left + roundedImageEnd.Width - cornerRadius, top + roundedImageEnd.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                gp1.AddArc(left, top + roundedImageEnd.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                gp1.CloseFigure();


                //var brush = new PathGradientBrush(gp1)
                //{
                //    CenterPoint = new PointF(left + roundedImage.Width / 2, top + roundedImage.Height / 2),
                //    CenterColor = Color.Black,
                //    SurroundColors = new[] { Color.White }
                //};


                //g.FillPath(brush, gp1);
                //g.SetClip(gp);


                // this is where we create the shadow effect, so we will use a 
                // pathgradientbursh and assign our GraphicsPath that we created of a 
                // Rounded Rectangle
                using (var brush = new PathGradientBrush(gp1))
                {
                    // set the wrapmode so that the colors will layer themselves
                    // from the outer edge in
                    brush.WrapMode = WrapMode.Clamp;

                    // Create a color blend to manage our colors and positions and
                    // since we need 3 colors set the default length to 3
                    var colorBlend = new ColorBlend(3)
                    {

                        // here is the important part of the shadow making process, remember
                        // the clamp mode on the colorblend object layers the colors from
                        // the outside to the center so we want our transparent color first
                        // followed by the actual shadow color. Set the shadow color to a 
                        // slightly transparent DimGray, I find that it works best.|
                        Colors = new[]
                        {
                            Color.Transparent,
                            Color.FromArgb(180, Color.DarkGray),
                            Color.FromArgb(180, Color.DarkGray)
                        },
                        Positions = new[] { 0f, .1f, 1f }
                    };



                    // our color blend will control the distance of each color layer
                    // we want to set our transparent color to 0 indicating that the 
                    // transparent color should be the outer most color drawn, then
                    // our Dimgray color at about 10% of the distance from the edge

                    // assign the color blend to the pathgradientbrush
                    brush.InterpolationColors = colorBlend;

                    // fill the shadow with our pathgradientbrush

                    g.FillPath(brush, gp1);
                }

                if (cornerRadius > 0) g.SetClip(gp);
                // g.DrawImage(roundedImageEnd, new Rectangle(0, 0, roundedImageEnd.Width, roundedImageEnd.Height - ChartData.ChartStyle.CorrectiveFactor), new Rectangle(0, 0, roundedImageEnd.Width, roundedImage.HeightEnd - ChartData.ChartStyle.CorrectiveFactor), GraphicsUnit.Pixel);


                g.DrawImage(roundedImageEnd, new Rectangle(0, 0, roundedImageEnd.Width, roundedImageEnd.Height), new Rectangle(0, 0, roundedImageEnd.Width, roundedImageEnd.Height), GraphicsUnit.Pixel);
            }

            roundedImageEnd.Dispose();
            Bitmap.Dispose();
            Bitmap = shadowedImageEnd;

        }


        #region Save image

        /// <summary>
        /// Save the bitmap as PNG file
        /// </summary>
        /// <param name="fullPath">full file name of the target file</param>
        public void SaveAsPng(string fullPath)
        {

            Bitmap.Save(fullPath, ImageFormat.Png);

        }

        /// <summary>
        /// Save the bitmap as JPEG file
        /// </summary>
        /// <param name="fullPath">full file name of the target file</param>
        /// <param name="compressionLevel">Compression level from 1 to 100 with 1 lowest quality and 100 highest quality</param>
        public void SaveAsJpeg(string fullPath, uint compressionLevel = 80)
        {

            //save the image to a memorystream to apply the compression level
            var jpgEncoder = GetEncoderInfo("image/jpeg");
            using (var ms = new MemoryStream())
            {
                //var encoderParameters = new EncoderParameters(1);
                //encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, (byte)compressionLevel);

                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compressionLevel);

                Bitmap.Save(ms, jpgEncoder, encoderParameters);

                //save the image as byte array here if you want the return type to be a Byte Array instead of Image
                //byte[] imageAsByteArray = ms.ToArray();

                //write to file
                var file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Close();
                ms.Close();
                file.Dispose();
                ms.Dispose();
            }

        }

        #endregion
    }
}
