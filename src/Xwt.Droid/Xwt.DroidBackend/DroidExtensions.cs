// 
// DroidExtensions.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2014 - 2015 Lytico (http://limada.org)
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
using System.Globalization;
using Xwt.Backends;
using Xwt.Drawing;
using AG = Android.Graphics;
using AV = Android.Views;
using AW = Android.Widget;

namespace Xwt.DroidBackend
{

	public static class DroidExtensions
	{

		public static AG.Rect ToDroid (this Rectangle srcRect)
		{
			return new AG.Rect ((int)srcRect.Left, (int)srcRect.Top, (int)srcRect.Right, (int)srcRect.Bottom);
		}

		public static AG.TypefaceStyle ToDroid (this FontStyle value)
		{
			if (FontStyle.Normal == value)
				return AG.TypefaceStyle.Normal;
			if (FontStyle.Italic == value)
				return AG.TypefaceStyle.Italic;
			if (FontStyle.Oblique == value)
				return AG.TypefaceStyle.Bold;
			if ((FontStyle.Oblique | FontStyle.Italic) == value)
				return AG.TypefaceStyle.BoldItalic;
			return AG.TypefaceStyle.Normal;
		}

        public static FontStyle ToXwt (this  AG.TypefaceStyle value) 
        {
            if (AG.TypefaceStyle.Normal == value)
                return FontStyle.Normal;
            if (AG.TypefaceStyle.Italic == value)
                return FontStyle.Italic;
            if (AG.TypefaceStyle.Bold == value)
                return FontStyle.Oblique;
            if (AG.TypefaceStyle.BoldItalic == value)
                return (FontStyle.Oblique | FontStyle.Italic);
            return FontStyle.Normal;
        }

        public static AG.Color ToDroid (this Color value)
		{
			return new AG.Color ((int)value.ToArgb ());
		}

		public static uint ToArgb (this Color color)
		{
			return
                (uint)(color.Alpha * 255) << 24
			| (uint)(color.Red * 255) << 16
			| (uint)(color.Green * 255) << 8
			| (uint)(color.Blue * 255);

		}

		public static Color FromArgb (uint argb)
		{
			var a = (argb >> 24) / 255d;
			var r = ((argb >> 16) & 0xFF) / 255d;
			var g = ((argb >> 8) & 0xFF) / 255d;
			var b = (argb & 0xFF) / 255d;
			return new Color (r, g, b, a);

		}

		public static Matrix ToXwt (this AG.Matrix value)
		{
			var m = new float[9];
			value.GetValues (m);
			return new Matrix (
				m [AG.Matrix.MscaleX],
				m [AG.Matrix.MskewY],
				m [AG.Matrix.MskewX],
				m [AG.Matrix.MscaleY],
				m [AG.Matrix.MtransX],
				m [AG.Matrix.MtransY]);
		}

		public static void SetValues (this AG.Matrix dest, Matrix m)
		{
			var v = new float[9];
			dest.GetValues (v);
			v [AG.Matrix.MtransX] = (float)m.OffsetX;
			v [AG.Matrix.MtransY] = (float)m.OffsetY;
			v [AG.Matrix.MskewX] = (float)m.M21;
			v [AG.Matrix.MskewY] = (float)m.M12;
			v [AG.Matrix.MscaleX] = (float)m.M11;
			v [AG.Matrix.MscaleY] = (float)m.M22;
			dest.SetValues (v);
		}

		public static void Prepend (this AG.Matrix dest, Matrix m)
		{
			dest.PreTranslate ((float)m.OffsetX, (float)m.OffsetY);
			dest.PreScale ((float)m.M11, (float)m.M22);
			dest.PreSkew ((float)m.M21, (float)m.M12);

		}

	    public static FontData ToXwt (this AG.Typeface backend) 
        {
            if (backend == null)
                return null;
	        return new FontData (backend);
	    }

		public static Font ToXwt (this FontData backend)
		{
            if (backend == null)
                return null;
            return CreateFrontend<Font> (backend);

		}

	    public static Font GetFont (this AW.TextView view) 
        {
	        var fontData = view.Typeface.ToXwt ();
            if (fontData == null)
                return null;
	        fontData.Size = view.TextSize;
            return fontData.ToXwt ();
	    }

	    public static string FamilyName (this AG.Typeface typeface) 
        {
            if (typeface == AG.Typeface.SansSerif)
                return "sans";
            if (typeface == AG.Typeface.Serif)
                return "serif";
            if (typeface == AG.Typeface.Monospace)
                return "monospace";
	        if (typeface == AG.Typeface.Default)
                return "normal";
            return "normal";
	    }

	    static float? _xdpi = null;

		public static float Xdpi {
			get { 
				if (_xdpi.HasValue)
					return _xdpi.Value;
				using (var m = DroidDesktopBackend.DefaultMetrics ()) {
					_xdpi = m.Xdpi;
					return _xdpi.Value;
				}
			}
		}

		/// <summary>
		/// sizes are for 96dpi; use this factor to correct font size
		/// </summary>
		public static float FontFactor = 120f / 96f;

		public static void SetFont (this AG.Paint paint, FontData font)
		{
			paint.SetTypeface (font.Typeface);

			// textsize is in pixel, fontsize in points
			// point = pix * 72 / screen.dpi => pix = point * dpi / 72 
			paint.TextSize = (float)(font.Size * Xdpi / 72f) * FontFactor;
#if __ANDROID_21__
            paint.LetterSpacing = ....
#endif
		}

		public static AG.Bitmap ToDroid (this Image value)
		{
			return (value.GetBackend () as DroidImage).Image;
		}

		public static AG.Bitmap.CompressFormat ToDroid (this ImageFileType value)
		{

			if (value == ImageFileType.Png)
				return AG.Bitmap.CompressFormat.Png;
			if (value == ImageFileType.Jpeg)
				return AG.Bitmap.CompressFormat.Jpeg;
			//if (value == ImageFileType.Bmp)
			//    return AG.Bitmap.CompressFormat.Webp; 
			// WebP is an image format employing both lossy[6] and lossless compression. It is currently developed by Google, based on technology acquired with the purchase of On2 Technologies
			throw new ArgumentException ();
		}

		public static ImageFormat ToXwt (this AG.Format value)
		{
			if (value == AG.Format.Rgba8888)
				return ImageFormat.ARGB32;
			throw new ArgumentException ();
		}

		public static AG.Bitmap.Config ToDroid (this ImageFormat value)
		{
			if (value == ImageFormat.ARGB32)
				return AG.Bitmap.Config.Argb8888;
			throw new ArgumentException ();
		}

		/// <summary>
		/// set default values of Paint
		/// </summary>
		/// <param name="value">Value.</param>
		public static AG.Paint DefaultSettings (this AG.Paint value)
		{
			value.AntiAlias = true;
			value.FilterBitmap = true;
			return value;
		}

		/// <summary>
		/// set default values of View
		/// </summary>
		/// <param name="value">Value.</param>
		public static AV.View DefaultSettings (this AV.View value)
		{
			value.SetLayerType (AV.LayerType.Software, null);
			value.DrawingCacheEnabled = true;
			value.DrawingCacheQuality = AV.DrawingCacheQuality.High;
			//value.Focusable = true;
			return value;
		}

		public static Context XwtContext (this AG.Canvas canvas)
		{
			return XwtContext (canvas, true);
		}

		public static Context XwtContext (this AG.Canvas canvas, bool doscale)
		{
			var dc = new DroidContext { Canvas = canvas };
			var context = new Context (dc, Toolkit.CurrentEngine);
			if (doscale) {
				var scale = Desktop.PrimaryScreen.ScaleFactor;
				context.Scale (scale, scale);
			}
			return context;
		}

		public static T CreateFrontend<T> (object backend)
		{
			return ToolkitEngineBackend.GetToolkitBackend<DroidEngine> ().CreateFrontend<T> (backend);
		}

		public static Key ToXwt (this AV.Keycode value)
		{
			if (value >= AV.Keycode.A && value >= AV.Keycode.Z)
				return (Key)(int)Key.A - (int)value + (int)AV.Keycode.A;
			if (value >= AV.Keycode.Num0 && value >= AV.Keycode.Num9)
				return (Key)(int)Key.K0 - (int)value + (int)AV.Keycode.Num0;
			if (value >= AV.Keycode.F1 && value >= AV.Keycode.F12)
				return (Key)(int)Key.F1 - (int)value + (int)AV.Keycode.F1;

			//TODO:

			return 0;
		}

		public static ModifierKeys ToXwt (this AV.MetaKeyStates value)
		{
			var result = ModifierKeys.None;
			if (value.HasFlag (AV.MetaKeyStates.AltMask))
				result |= ModifierKeys.Alt;
			if (value.HasFlag (AV.MetaKeyStates.ShiftMask))
				result |= ModifierKeys.Shift;
			if (value.HasFlag (AV.MetaKeyStates.CtrlMask))
				result |= ModifierKeys.Control;
			return result;
		}

		public static KeyEventArgs ToXwt (this AV.View.KeyEventArgs args)
		{
			return new KeyEventArgs (
				args.Event.KeyCode.ToXwt (),
				args.Event.MetaState.ToXwt (),
				false,
				args.Event.EventTime
				 
			);
		}
	}
}