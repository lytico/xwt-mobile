// 
// FontData.cs
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

using Android.Graphics;
using Xwt.Drawing;

namespace Xwt.DroidBackend
{

	public class FontData
	{

		public FontData ()
		{
			Stretch = FontStretch.Normal;
			Weight = FontWeight.Normal;
			Style = FontStyle.Normal;
		}

        internal FontData (Typeface typeface) {
            _family = typeface.FamilyName();
            _style = typeface.Style.ToXwt ();
            _typeface = typeface;
        }

		Typeface _typeface = null;

		public Typeface Typeface {
			get {
				if (_typeface == null) {
					_typeface = Typeface.Create (Family, this.Style.ToDroid ());
				}

				return _typeface;
			}
		}

		string _family = null;

		public string Family {
			get { return _family; }
			set {
				if (value != _family) {
					_typeface = null;
				}
				_family = value;
			}
		}

		FontStyle _style;

		public FontStyle Style {
			get { return _style; }
			set {
				if (_style != value)
					_typeface = null;
				_style = value;
			}
		}

		public double Size { 
			get; 
			set; 
		}

		public FontWeight Weight { get; set; }

		public FontStretch Stretch { get; set; }

		public void CopyFrom (Font font)
		{
			Family = font.Family;
			Size = font.Size;
			Style = font.Style;
			Weight = font.Weight;
			Stretch = font.Stretch;
		}

		public void CopyFrom (FontData font)
		{
			Family = font.Family;
			Size = font.Size;
			Style = font.Style;
			Weight = font.Weight;
			Stretch = font.Stretch;
		}

		public FontData Clone() {
			var result = new FontData ();
			result.CopyFrom (this);
			return result;
		}

		public static FontData Default { 
			get {
				return new FontData(Typeface.Default) { 
					Size = 10,
				};
			}
		}
	}
}