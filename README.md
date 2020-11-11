# What does the library

Bodoconsult.Core.Drawing library is a simple library based on System.Drawing for essential image manipulation like resizing, adjusting brightness, contrast or gamma and saturation.

# How to use the library

The source code contain NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

## Resize an image

		BitmapSevice _service;
		
		_service.LoadBitmap(source);
		
		_service.ResizeImage(400, 400);

		_service.SaveAsJpeg(target);

## Adjust brightness, contrast or gamma


		var brightness = 1.4f;
		var contrast = 0.8f;
		var gamma = 1f;

		BitmapSevice _service;
		
		_service.LoadBitmap(source);
		
		_service.AdjustBcg(brightness, contrast, gamma);

		_service.SaveAsJpeg(target);
		

## Adjust saturation

		var saturation = -1F;

		BitmapSevice _service;
		
		_service.LoadBitmap(source);
		
		_service.AdjustSaturation(saturation);

		_service.SaveAsJpeg(target);

## Combining two or more manipulations

		var saturation = -1F;
		var brightness = 1.4f;
		var contrast = 0.8f;
		var gamma = 1f;
		
		BitmapSevice _service;
		
		_service.LoadBitmap(source);
		
		_service.AdjustBcg(brightness, contrast, gamma);
		_service.AdjustSaturation(saturation);

		_service.SaveAsJpeg(target);


# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software development company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

