// 
// TextWrapper.cs
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

	public class TextWrapper
	{

		public float LineY { get; protected set; }

		public float LineWidth { get; protected set; }

		public float LineHeight { get; protected set; }

		public float Baseline { get; protected set; }

		public int LineStart { get; protected set; }

		public int CursorPos { get; protected set; }

		public bool HasLineFeed { get; protected set; }

		public Action<TextWrapper> SingleLine { get; set; }

		public Action<TextWrapper> MultiLine { get; set; }

		public double MaxHeight { get; protected set; }

		public double MaxWidth { get; protected set; }

		public Xwt.Size PreferedSize { get; set; }

		public void Wrap (DroidTextLayoutBackend layout, AG.Paint paint)
		{

			var text = layout.Text;

			var metrics = paint.GetFontMetrics ();
			Baseline = -metrics.Top;
			LineHeight = - metrics.Ascent + metrics.Descent;
			LineY = -metrics.Ascent ; //- (metrics.Ascent + metrics.Descent);

			var charWidths = new float[text.Length];
			paint.GetTextWidths (text, charWidths);

			var iLf = text.IndexOfAny (new char[]{ '\n', '\r' });
			HasLineFeed = iLf >= 0;

			var textWidth = charWidths.Sum ();

			if (!HasLineFeed && (layout.Height <= 0 || layout.Height <= LineHeight) && (layout.Width <= 0 || textWidth <= layout.Width)) {
				LineWidth = textWidth;
				SingleLine (this);
			} else {

				MaxHeight = PreferedSize.Height > 0 ? PreferedSize.Height : (layout.Height <= 0 ? float.MaxValue : layout.Height);
				MaxWidth = PreferedSize.Width > 0 ? PreferedSize.Width : (layout.Width <= 0 ? textWidth : layout.Width);

				CursorPos = 0;
				LineStart = 0;

				while (LineY <= MaxHeight && CursorPos < text.Length) {
					LineWidth = 0f;
					var whitePosY = -1f;
					var whitepos = -1;
					var newLine = false;

					while (CursorPos < text.Length) {

						newLine = text [CursorPos] == '\n';

						if (text [CursorPos] == '\r') {
							CursorPos++;
							continue;
						}

						if (newLine) {
							break;
						}

						if (char.IsWhiteSpace (text [CursorPos])) {
							whitepos = CursorPos;
							whitePosY = LineWidth;
						}
						LineWidth += charWidths [CursorPos];

						if (LineWidth > MaxWidth) {
							if (whitepos > 0) {
								CursorPos = whitepos;
								LineWidth = whitePosY;
							} 
							break;
						}
						CursorPos++;
					}

					Action line = () => {
						MultiLine (this);
						LineY += LineHeight;
					};

					line ();

					CursorPos++;
					LineStart = CursorPos;

					if (newLine && CursorPos == text.Length)
						line ();
				}
			}
		}
	}
}