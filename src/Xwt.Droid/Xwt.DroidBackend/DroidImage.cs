// 
// DroidImage.cs
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

namespace Xwt.DroidBackend {

	public class DroidImage:IDisposable {

		public DroidImage(int width, int height, ImageFormat format) {
			this.Image = AG.Bitmap.CreateBitmap (width, height, format.ToDroid());
		}

		public DroidImage (ImageDrawCallback drawCallback) {
			this._drawCallback = drawCallback;
		}

		public DroidImage (AG.Bitmap image) {
			this.Image = image;
		}

		AG.Bitmap _image = null;
		public  AG.Bitmap Image {
			get {
				return _image;
			}
			protected set { _image = value; }
		}

		ImageDrawCallback _drawCallback = null;

		protected void Dispose(bool disposing) {
			Image = null;
		}

		public void Dispose() {
			Dispose(true);
		}

		~DroidImage() {
			Dispose(false);
		}

		public static DroidImage LoadFromStream (Stream stream) {
			return new DroidImage (AG.BitmapFactory.DecodeStream (stream));
		}

		public void SaveToStream (Stream stream, ImageFileType fileType) {
			if (fileType == ImageFileType.Bmp)
				throw new ArgumentException (string.Format("{0} is not supported", fileType));
			Image.Compress (fileType.ToDroid (), 100, stream);
		}

		public bool IsBitmap  {
			get {return Image is AG.Bitmap;}
		}

		public Size Size { get { return new Size (Image.Width, Image.Height); } }

		public DroidImage CopyBitmap () {
			return new DroidImage (Image.Copy (Image.GetConfig (), true));
		}

		public ImageFormat Format { get { return Image.GetBitmapInfo ().Format.ToXwt (); } }

		public object ConvertToBitmap (double width, double height, double scaleFactor, ImageFormat format) {
			if (_drawCallback !=null) {
				var image = new DroidImage((int)width,(int)height,format);
				using(var canvas = new AG.Canvas(image.Image))
				using (var dc = new DroidContext{Canvas=canvas})				{
					canvas.Scale ((float)scaleFactor, (float)scaleFactor);
					_drawCallback(dc,new Xwt.Rectangle(0,0,width,height));
				}
				return image;
			}

			throw new NotSupportedException();
		}
	}
    
}