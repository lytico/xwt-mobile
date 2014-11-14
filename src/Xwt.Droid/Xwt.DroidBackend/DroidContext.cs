// 
// DroidContext.cs
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
using System.Collections.Generic;
using Android.Graphics;
using AG = Android.Graphics;

namespace Xwt.DroidBackend
{

	public class DroidContext : IDisposable
	{

		public DroidContext ()
		{

		}

		AG.Canvas _canvas = null;

		public AG.Canvas Canvas {
			get { return _canvas ?? (_canvas = new AG.Canvas ()); }
			set { _canvas = value; }
		}

		Paint _paint = null;

		public Paint Paint {
			get { return _paint ?? (_paint = new Paint ().Default ()); }
			set {
				if (_paint != value && _paint != null)
					_paint.Dispose ();
				_paint = value;
			}
		}

		Path _path = null;

		public Path Path {
			get { return _path ?? (_path = new Path ()); }
			set {
				if (_path != value && _path != null)
					_path.Dispose ();
				_path = value;
			}
		}

		public double[] LineDash { get; set; }

		public object Pattern { get; set; }

		public FontData Font { get; set; }

		public Matrix CTM { get { return Canvas.Matrix; } }

		public DroidContext (DroidContext c)
			: this ()
		{
			c.SaveTo (this, false);
		}

		private Point _current;

		public Point Current {
			get { return _current; }
			set { _current = value; }
		}

		protected Stack<DroidContext> contexts;

		public void SaveTo (DroidContext c, bool p)
		{
			c.Font = this.Font;
			c.LineDash = this.LineDash;
			c.Current = this.Current;
			//c.Canvas = this.Canvas;
			c.Pattern = this.Pattern;
		}

		public void Save ()
		{
			if (this.contexts == null)
				this.contexts = new Stack<DroidContext> ();
			this.Canvas.Save (SaveFlags.All);
			this.contexts.Push (new DroidContext (this) { });
		}

		public void Restore ()
		{
			if (this.contexts == null || this.contexts.Count == 0)
				throw new InvalidOperationException ("DroidContext: Nothing to Restore");

			var c = this.contexts.Pop ();
			this.Canvas.Restore ();
			c.SaveTo (this, true);

		}

		public void Dispose ()
		{
			Paint = null;
			Path = null;
		}
	}
}