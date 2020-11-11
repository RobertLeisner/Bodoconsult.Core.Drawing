namespace Bodoconsult.Core.Drawing
{

    /// <summary>
    /// Creates OpenGraph images based on a template image
    /// </summary>
    public static class OpenGraphHelper
    {

        public const string OpenGraphFileName = @"opengraph.png";

        public static ImageWriterData Header { get; set; } = new ImageWriterData
        {
            FontName = "Arial",
            FontSize = 16,
            X = 20,
            Y = 580,
            Width = 1650,
            Height = 70,
            Text = "",
            FontStyle = 1
        };

        public static ImageWriterData Body { get; set; } = new ImageWriterData
        {
            FontName = "Arial",
            FontSize = 10,
            X = 20,
            Y = 660,
            Width = 1650,
            Height = 230,
            Text = ""
        };


        public static string SourceFile
        {
            get => OpenGraphImageWriter.SourceFile;
            set => OpenGraphImageWriter.SourceFile = value;
        }


        private static ImageWriter OpenGraphImageWriter { get; } = new ImageWriter();

        /// <summary>
        /// Save the image as 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="fileName"></param>
        public static void Save(string title, string description, string fileName)
        {
            Header.Text = title;
            Body.Text = description;

            OpenGraphImageWriter.TargetFile = fileName;
            OpenGraphImageWriter.NewImage();
            OpenGraphImageWriter.WriteText(Header);
            OpenGraphImageWriter.WriteText(Body);
            OpenGraphImageWriter.Save();
        }


    }
}