// 
// DroidContextBackendHandler.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2014 Lytico (http://limada.org)
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
using System.Linq;
using Xwt.Backends;
using System.Globalization;
using AG = Android.Graphics;
using XD = Xwt.Drawing;

namespace Xwt.DroidBackend
{

	public class DroidContextBackendHandler : ContextBackendHandler
	{

		public virtual object CreateContext (Widget w)
		{
			var b = (IDroidCanvasBackend)w.GetBackend ();

			var ctx = new DroidContext ();
			if (b.Context != null) {
				ctx.Canvas = b.Context.Canvas;
			}
			return ctx;
		}

		public override void Save (object backend)
		{
			var c = (DroidContext)backend;
			c.Save ();
		}

		public override void Restore (object backend)
		{
			var c = (DroidContext)backend;
			c.Restore ();
		}

		public override void NewPath (object backend)
		{
			var c = (DroidContext)backend;
			c.Path = null;
		}

		public override void ClosePath (object backend)
		{
			var c = (DroidContext)backend;
			c.Path.Close ();
		}

		public override object CreatePath ()
		{
			return new AG.Path ();
		}

		public override object CopyPath (object backend)
		{
			return new AG.Path ((AG.Path)backend);
		}

		public override void AppendPath (object backend, object otherBackend)
		{
			((AG.Path)backend).AddPath ((AG.Path)otherBackend);
		}

		public override void Clip (object backend)
		{
			var c = (DroidContext)backend;
			c.Canvas.ClipPath (c.Path);
			c.Path = null;
		}

		public override void ClipPreserve (object backend)
		{
			var c = (DroidContext)backend;
			c.Canvas.ClipPath (c.Path);
		}

		public void ResetClip (object backend)
		{
			var c = (DroidContext)backend;
			using (var p = new AG.Path ())
				c.Canvas.ClipPath (p); 
		}

		const double degrees = System.Math.PI / 180d;

		public override void Arc (object backend, double xc, double yc, double radius, double angle1, double angle2)
		{
			var c = (DroidContext)backend;
			if (angle1 > 0 && angle2 == 0)
				angle2 = 360;

			if ((angle2 - angle1) == 360)
				c.Path.AddCircle ((float)(xc), (float)(yc), (float)radius, AG.Path.Direction.Cw);
			else
				c.Path.ArcTo (
					new AG.RectF ((float)(xc- radius), (float)(yc- radius), (float)(xc+radius), (float)(yc+radius)), 
					(float)angle1, 
					(float)(angle2 - angle1), 
					false);
		}

		public override void CurveTo (object backend, double x1, double y1, double x2, double y2, double x3, double y3)
		{
			var c = (DroidContext)backend;
			c.Path.CubicTo ((float)x1, (float)y1, (float)x2, (float)y2, (float)x3, (float)y3);
			c.Current = new Point (x3, y3);
		}

		public override void LineTo (object backend, double x, double y)
		{
			var c = (DroidContext)backend;
			c.Path.LineTo ((float)x, (float)y);
			c.Current = new Point (x, y);
		}

		public override void MoveTo (object backend, double x, double y)
		{
			var c = (DroidContext)backend;
			c.Path.Close ();
			c.Path.MoveTo ((float)x, (float)y);
			c.Current = new Point (x, y);
		}

		public override void Rectangle (object backend, double x, double y, double width, double height)
		{
			var c = (DroidContext)backend;
			c.Path.AddRect ((float)x, (float)y, (float)(x + width), (float)(y + height), Android.Graphics.Path.Direction.Cw);
		}

		public override void RelCurveTo (object backend, double dx1, double dy1, double dx2, double dy2, double dx3, double dy3)
		{
			var c = (DroidContext)backend;
			c.Path.RCubicTo ((float)dx1, (float)dy1, (float)dx2, (float)dy2, (float)dx3, (float)dy3);
			c.Current += new Size (dx3, dy3);
		}

		public override void RelLineTo (object backend, double dx, double dy)
		{
			var c = (DroidContext)backend;
			c.Path.RLineTo ((float)dx, (float)dy);
			c.Current += new Size (dx, dy);
		}

		public override void RelMoveTo (object backend, double dx, double dy)
		{
			var c = (DroidContext)backend;
			c.Path.RMoveTo ((float)dx, (float)dy);
			c.Current += new Size (dx, dy);
		}

		public override void DrawTextLayout (object backend, Drawing.TextLayout layout, double x, double y)
		{
			var c = (DroidContext)backend;

			var tl = (DroidTextLayoutBackend)layout.GetBackend ();
			var strw = c.Paint.StrokeWidth;
			var style = c.Paint.GetStyle ();
			tl.SetPaint (c.Paint);
			try {
				c.Paint.StrokeWidth = 0.3f;
				c.Paint.SetStyle (AG.Paint.Style.FillAndStroke);

				var text = layout.Text;
				var fx = (float)x;
				var fy = (float)y;

				var wrapper = new TextWrapper ();
				wrapper.SingleLine = w => 
					c.Canvas.DrawText (text, fx, fy + w.LineY, c.Paint);
				wrapper.MultiLine = w => {
					var st = text.Substring (w.LineStart, w.CursorPos - w.LineStart);
					if (w.LineY + w.LineHeight > w.MaxHeight && w.CursorPos < text.Length)
						st += ((char)0x2026).ToString ();
					c.Canvas.DrawText (st, fx, fy + w.LineY, c.Paint);
				};
				wrapper.Wrap (tl, c.Paint);
			} finally {
				c.Paint.StrokeWidth = strw;
				c.Paint.SetStyle (style);
			}
		}

		public override void Fill (object backend)
		{
			FillPreserve (backend);
			NewPath (backend);
		}

		public override void FillPreserve (object backend)
		{
			var c = (DroidContext)backend;
			c.Paint.SetStyle (AG.Paint.Style.Fill);
			c.Canvas.DrawPath (c.Path, c.Paint);
		}

		public override void Stroke (object backend)
		{
			StrokePreserve (backend);
			NewPath (backend);
		}

		public override void StrokePreserve (object backend)
		{
			var c = (DroidContext)backend;
			c.Paint.SetStyle (AG.Paint.Style.Stroke);
			c.Canvas.DrawPath (c.Path, c.Paint);
		}

		public override void SetColor (object backend, Drawing.Color color)
		{
			var c = (DroidContext)backend;
			c.Paint.Color = color.ToDroid ();
		}

		public override void SetLineWidth (object backend, double width)
		{
			var c = (DroidContext)backend;
			c.Paint.StrokeWidth = (float)width;
		}

		public void SetFont (object backend, Drawing.Font font)
		{
			var c = (DroidContext)backend;
			c.Font = (FontData)font.GetBackend ();
		}

		public override void ModifyCTM (object backend, Drawing.Matrix m)
		{
			var c = (DroidContext)backend;
			var am = c.CTM;
			am.Prepend (m);
			c.Canvas.Matrix = am;
		}

		public override Drawing.Matrix GetCTM (object backend)
		{
			var c = (DroidContext)backend;
			return c.CTM.ToXwt ();
		}

		public void ResetTransform (object backend)
		{
			var c = (DroidContext)backend;
			c.CTM.Reset ();
		}

		public override void Rotate (object backend, double angle)
		{
			var c = (DroidContext)backend;
			c.Canvas.Rotate ((float)angle);
		}

		/// <summary>
		/// Sets the scale to 1
		/// </summary>
		/// <param name="backend">Backend.</param>
		public void ResetScale (object backend)
		{
			var c = (DroidContext)backend;
			var v = new float[9];
			c.CTM.GetValues (v);
			c.Canvas.Scale (1f / v [AG.Matrix.MscaleX], 1f / v [AG.Matrix.MscaleY]);
		}

		public override void Scale (object backend, double scaleX, double scaleY)
		{
			var c = (DroidContext)backend;
			ResetScale (backend);
			c.Canvas.Scale ((float)scaleX, (float)scaleY);
		}

		public override void Translate (object backend, double tx, double ty)
		{
			var c = (DroidContext)backend;
			c.Canvas.Translate ((float)tx, (float)ty);
		}

		public override void Dispose (object backend)
		{
			var c = (DroidContext)backend;
			c.Dispose ();
		}

		public override void DrawImage (object backend, ImageDescription img, double x, double y)
		{
			var c = (DroidContext)backend;
			var dimg = (DroidImage)img.Backend; 
			if (img.Size == dimg.Size)
				c.Canvas.DrawBitmap (dimg.Image, (float)x, (float)y, c.Paint);
			else {
				c.Canvas.DrawBitmap (dimg.Image, 
					new Rectangle (0, 0, dimg.Size.Width, dimg.Size.Height).ToDroid (), 
					new Rectangle (x, y, img.Size.Width, img.Size.Height).ToDroid (), 
					c.Paint);
			}
		}

		public override void DrawImage (object backend, ImageDescription img, Rectangle srcRect, Rectangle destRect)
		{
			var c = (DroidContext)backend;
			var dimg = (DroidImage)img.Backend; 
			c.Canvas.DrawBitmap (dimg.Image, srcRect.ToDroid (), destRect.ToDroid (), c.Paint);
		}

		public override void SetLineDash (object backend, double offset, params double[] pattern)
		{
			var c = (DroidContext)backend;
			if (pattern == null || pattern.Length == 0) {
				c.Paint.SetPathEffect (null);
				return;
			}
			var eff = new AG.DashPathEffect (pattern.Select (p => (float)p).ToArray (), 0);
			c.Paint.SetPathEffect (eff);
		}

		public override void SetPattern (object backend, object p)
		{
			var c = (DroidContext)backend;
			c.Paint.SetShader (p as AG.Shader);
		}

		double _globalAlpha = 1d;

		public override void SetGlobalAlpha (object backend, double globalAlpha)
		{
			var c = (DroidContext)backend;
			//c.Paint.Alpha = (int)(globalAlpha * 255);
			_globalAlpha = globalAlpha;
		}

		public override double GetScaleFactor (object backend)
		{
			return 1;
		}

		// TODO:

		public override bool IsPointInStroke (object backend, double x, double y)
		{
			throw new NotImplementedException ();
		}

		public override bool IsPointInFill (object backend, double x, double y)
		{
			throw new NotImplementedException ();
		}

		public override void ArcNegative (object backend, double xc, double yc, double radius, double angle1, double angle2)
		{
			throw new NotImplementedException ();
		}

	}
}