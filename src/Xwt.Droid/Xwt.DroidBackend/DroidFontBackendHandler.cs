// 
// DroidFontBackendHandler.cs
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


using Xwt.Drawing;
using Xwt.Backends;
using Android.Graphics;
using System;

namespace Xwt.DroidBackend {

    public class DroidFontBackendHandler : FontBackendHandler {

        public override object GetSystemDefaultFont () {
			return FontData.Default;
        }

		public override object GetSystemDefaultMonospaceFont ()
		{
			return new FontData { Family = "sans-monospace", Size = 10 };
		}

		public override object GetSystemDefaultSansSerifFont ()
		{
			return new FontData { Family = "sans-serif", Size = 10 };
		}

		public override object GetSystemDefaultSerifFont ()
		{
			return new FontData { Family = "serif", Size = 10 };
		}

        public override System.Collections.Generic.IEnumerable<string> GetInstalledFonts () {
            yield break;
        }

		public override object Create (string fontName, double size, FontStyle style, FontWeight weight, FontStretch stretch) {
			return new FontData { Family = fontName, Size = size, Style = style, Weight = weight, Stretch = stretch };
		}

		public override object Copy (object handle) {
			return ((FontData)handle).Clone ();
		}

		public override object SetSize (object handle, double size) {
			var d = ((FontData)handle).Clone ();
			d.Size = size;
			return d;
		}

		public override object SetFamily (object handle, string family) {
			var d = ((FontData)handle).Clone ();
			d.Family = family;
			return d;
		}

		public override object SetStyle (object handle, FontStyle style) {
			var d = ((FontData)handle).Clone ();
			d.Style = style;
			return d;
		}

		public override object SetWeight (object handle, FontWeight weight) {
			var d = ((FontData)handle).Clone ();
			d.Weight = weight;
			return d;
		}

		public override object SetStretch (object handle, FontStretch stretch) {
			var d = ((FontData)handle).Clone ();
			d.Stretch = stretch;
			return d;
		}

		public override double GetSize (object handle) {
			var d = (FontData) handle;
			return d.Size;
		}

		public override  string GetFamily (object handle) {
			var d = (FontData) handle;
			return d.Family;
		}

		public override  FontStyle GetStyle (object handle) {
			var d = (FontData) handle;
			return d.Style;
		}

		public override FontWeight GetWeight (object handle) {
			var d = (FontData) handle;
			return d.Weight;
		}

		public override FontStretch GetStretch (object handle) {
			var d = (FontData) handle;
			return d.Stretch;
		}

    }
}