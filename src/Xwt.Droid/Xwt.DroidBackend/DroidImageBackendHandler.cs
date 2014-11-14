// 
// DroidImageBackendHandler.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2014 Lytico (www.limada.org)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Xwt.Backends;
using System;
using System.Collections.Generic;
using System.IO;
using Xwt.Drawing;
using AG = Android.Graphics;

namespace Xwt.DroidBackend
{

	public class DroidImageBackendHandler : ImageBackendHandler
	{

		public override object LoadFromStream (Stream stream)
		{
			return DroidImage.LoadFromStream (stream);
		}

		public override bool IsBitmap (object backend)
		{
			var img = (DroidImage)backend;
			return img.IsBitmap;
		}

		public override Size GetSize (object backend)
		{
			var img = (DroidImage)backend;
			return img.Size;
		}

		public override object CopyBitmap (object backend)
		{
			var img = (DroidImage)backend;
			return img.CopyBitmap ();
		}

		public override void SaveToStream (object backend, Stream stream, ImageFileType fileType)
		{
			var img = (DroidImage)backend;
			img.SaveToStream (stream, fileType);
		}

		public override ImageFormat GetFormat (object backend)
		{
			var img = (DroidImage)backend;
			return img.Format;
		}

		public override object CreateCustomDrawn (ImageDrawCallback drawCallback)
		{
			return new DroidImage (drawCallback);
		}

		public override object ConvertToBitmap (object backend, double width, double height, double scaleFactor, ImageFormat format)
		{
			var img = (DroidImage)backend;
			return img.ConvertToBitmap (width, height, scaleFactor, format);
		}

		public override bool HasMultipleSizes (object handle)
		{
			throw new NotImplementedException ();
		}

		public override Image GetStockIcon (string id)
		{
			throw new NotImplementedException ();
		}

		public override void CopyBitmapArea (object srcHandle, int srcX, int srcY, int width, int height, object destHandle, int destX, int destY)
		{
			throw new NotImplementedException ();
		}

		public override object CropBitmap (object handle, int srcX, int srcY, int width, int height)
		{
			throw new NotImplementedException ();
		}

		public override void SetBitmapPixel (object handle, int x, int y, Color color)
		{
			throw new NotImplementedException ();
		}

		public override Color GetBitmapPixel (object handle, int x, int y)
		{
			throw new NotImplementedException ();
		}
	}
}