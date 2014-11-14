// 
// DroidTextLayoutBackend.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2014 Lytico (http://www.limada.org)
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

using System;
using Xwt.Drawing;
using System.Linq;
using Xwt.Backends;
using AG = Android.Graphics;

namespace Xwt.DroidBackend
{

	public class DroidTextLayoutBackend : IDisposable
	{

		private double _width;

		public double Width {
			get { return _width; }
			set {
				if (_width != value)
					_size = null;
				_width = value;
			}
		}

		public double Height { get; set; }

		private string _text;

		public string Text {
			get { return _text; }
			set {
				if (_text != value)
					_size = null;
				_text = value;
			}
		}

		private FontData _font;

		public FontData Font { get { return _font ?? FontData.Default; } set { _font = value; } }

		public TextTrimming Trimming { get; set; }

		public WrapMode WrapMode { get; set; }

		Size? _size = null;

		public Size Size {
			get {
				if (_size == null) {
					var font = this.Font;
					var size = new Size (this.Width, 0);
					_size = MeasureString (this.Text, font, size);
				}
				return _size.Value;
			}
		}

		public Size MeasureString (string text, FontData font, Size size)
		{
			Paint.SetFont (font);

			var wi = 0d;
			var he = 0d;

			var wrapper = new TextWrapper { PreferedSize = size };

			wrapper.SingleLine = w => {
				he = w.LineHeight;
				wi = w.LineWidth;
			};

			var iLines = 0;
			wrapper.MultiLine = w => {
				iLines++;
				wi = Math.Max (wi, w.LineWidth);
			};
		
			wrapper.Wrap (this, Paint);
			if (iLines > 0) {
				he = iLines * wrapper.LineHeight;
			}

			return new Size (wi, he);
		}

		AG.Paint _paint = null;

		public AG.Paint Paint {
			get {
				if (_paint == null) {
					_paint = new Android.Text.TextPaint ().Default ();
					SetPaint (_paint);
				}
				return _paint;
			}
		}

		public void SetPaint (AG.Paint dest)
		{
			dest.SetFont (this.Font);
			dest.TextAlign = AG.Paint.Align.Left;
		}

		public void Dispose ()
		{
			if (_paint != null)
				_paint.Dispose ();
			_paint = null;
		}
	}

}