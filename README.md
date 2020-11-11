# What does the library

Bodoconsult.Core.Drawing library is a simple library based on System.Drawing for essential image manipulation like resizing, adjusting brightness, contrast or gamma and saturation.

# How to use the library

The source code contain NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

## Using class ImageTools

### Reduce a image to a new size if the current file sizes is to big


		var fi = new FileInfo(@"C:\test\test.jpg");

		var maxImageSize = 1000000; // byte

		var maxPicWidth = 800; // pixel
		var maxPicWidth = 800; // pixel

		ImageTools.GenerateWebImage(fi, maxImageSize, maxPicWidth, maxPicHeight);


### Generate a thumbnail for an image

		var fi = new FileInfo(@"C:\test\test.jpg");
		
		var thumb = @"C:\test\test_t.jpg"

		var thumbWidth = 60;	// Thumbnail width
		var thumbHeight = 60;	// Thumbnail height

		ImageTools.GenerateThumb(fi, thumb, thumbWidth, thumbHeight);

## Using class BitmapService

The class BitmapServices is intended to be use if you have to do more than one manipulation with one image.

### Resize an image

		BitmapSevice _service;
		
		_service.LoadBitmap(source);
		
		_service.ResizeImage(400, 400);

		_service.SaveAsJpeg(target);

### Adjust brightness, contrast or gamma


		var brightness = 1.4f;
		var contrast = 0.8f;
		var gamma = 1f;

		BitmapSevice _service;
		
		_service.LoadBitmap(source);
		
		_service.AdjustBcg(brightness, contrast, gamma);

		_service.SaveAsJpeg(target);
		

### Adjust saturation

		var saturation = -1F;

		BitmapSevice _service;
		
		_service.LoadBitmap(source);
		
		_service.AdjustSaturation(saturation);

		_service.SaveAsJpeg(target);

### Combining two or more manipulations

		var saturation = -1F;
		var brightness = 1.4f;
		var contrast = 0.8f;
		var gamma = 1f;
		
		BitmapSevice _service;
		
		_service.LoadBitmap(source);
		
		_service.AdjustBcg(brightness, contrast, gamma);
		_service.AdjustSaturation(saturation);

		_service.SaveAsJpeg(target);


## Using class OpenGraphHelper

OpenGraphHelper helps creating OpenGraph images for websites automatically.



			// Load master file once
            OpenGraphHelper.SourceFile = Path.Combine(TestHelper.GetTestDataFolder(), "OpenGraphBasis.png");

            // Act 1
            OpenGraphHelper.Save("Page 1", "Anything to write", @C:\Test\test1.png");

            // Act 2
            OpenGraphHelper.Save("Page 2", "Test description", @C:\Test\test2.png");


# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software development company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

