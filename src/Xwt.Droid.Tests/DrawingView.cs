using System;

using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xwt;
using Xwt.Tests;
using Android.Util;
using Xwt.Drawing;
using Xwt.DroidBackend;

namespace Xwt.Droid.Tests
{

	public class DrawingView: View
	{

		public DrawingView (Android.Content.Context context) :
			base (context)
		{
			Initialize ();
		}

		public DrawingView (Android.Content.Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public DrawingView (Android.Content.Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		Font _font = null;

		Font Font {
			get { return _font ?? (_font = Font.SystemSerifFont); }

		}

		private void Initialize ()
		{ 
			this.DefaultSettings ();
		}

		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{

			base.OnDraw (canvas);
			canvas.DrawColor (Colors.White.ToDroid ());

			using (var ctx = canvas.XwtContext ()) {

				var p = new ReferencePainter ();
				p.Font = this.Font;

				if (true) {
					p.Figures (ctx, 5, 5);
					p.Transforms (ctx, 5, 150);
					p.Texts (ctx, 5, 250);
					p.PatternsAndImages (ctx, 205, 5);
				} else {
					TestDrawing (ctx, 5, 5);
				}
			}
		}

		protected void TestDrawing (Xwt.Drawing.Context ctx, double x, double y)
		{
			ctx.Save ();
			ctx.Translate (x, y);

			var tl = new TextLayout (ctx);
			tl.Text = "\nX\n";
			tl.Width = 200;

			new ReferencePainter ().DrawText (ctx, tl, ref y);

			ctx.Restore ();
		}

		protected void TestDrawing2 (Xwt.Drawing.Context ctx, double x, double y)
		{
			ctx.Save ();
			ctx.Translate (x, y);
			var arcColor = new Color (1, 0, 1);
			ctx.Arc (100, 100, 15, 0, 360);
			ctx.ClosePath ();
			arcColor.Alpha = 0.4;
			ctx.SetColor (arcColor);
			ctx.StrokePreserve ();
			ctx.Fill ();

			ctx.Restore ();
		}

		protected void TestDrawing1 (Xwt.Drawing.Context ctx, double x, double y)
		{
			ctx.Save ();
			ctx.Translate (x, y);

			var r = 5;
			var l = 10;
			var t = 10;
			var w = 50;
			var h = 30;


			// top left  
			//ctx.Arc(l + r, t + r, r, 180, 270);
			ctx.Rectangle (l, t, w, h);
			ctx.SetColor (Colors.Black);
			ctx.Stroke ();

			ctx.Restore ();
		}
	}
}
