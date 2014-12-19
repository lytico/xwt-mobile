//
// DroidDesktopBackend.cs
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
using System.Reflection;
using System.Runtime.InteropServices;
using Xwt.Backends;

using System.Collections.Generic;
using Android.Util;
using Android.Content;

namespace Xwt.DroidBackend
{

	public class DroidDesktopBackend: DesktopBackend
	{

		public DroidDesktopBackend ()
		{
		}

		/// <summary>
		/// has to be set in MainActivity!
		/// </summary>
		/// <value>The context.</value>
		public static Context Context { get; set; }

		public static Android.Views.IWindowManager WindowManager {

			get {
				var con = Android.App.Application.Context;
				con = Context;
				return ((Android.App.Activity)con).WindowManager;
				// does not work on initialization:
				return (Android.Views.IWindowManager)con.GetSystemService (Context.WindowService); 
			}
		}

		/// <summary>
		/// metrics of the default-display
		/// </summary>
		/// <returns>The metrics.</returns>
		public static DisplayMetrics DefaultMetrics(){
			var metrics = new DisplayMetrics ();
			var d = WindowManager.DefaultDisplay;
			d.GetMetrics (metrics);
			return metrics;
		}

		public override double GetScaleFactor (object backend)
		{
			var metrics = new DisplayMetrics ();
			var d = (Android.Views.Display)backend;
			d.GetMetrics (metrics);

			return metrics.Density;
		}

		public override Point GetMouseLocation ()
		{
			return Point.Zero;
		}

		public override IEnumerable<object> GetScreens ()
		{
			yield return WindowManager.DefaultDisplay;
		}

		public override bool IsPrimaryScreen (object backend)
		{
			return backend == WindowManager.DefaultDisplay;
		}

		public override Rectangle GetScreenBounds (object backend)
		{
			var d = (Android.Views.Display)backend;
			var r = new Android.Graphics.Rect ();
			d.GetRectSize (r);
			return Xwt.Rectangle.FromLTRB (r.Left, r.Top, r.Right, r.Bottom);
		}

		public override Rectangle GetScreenVisibleBounds (object backend)
		{
			return GetScreenBounds (backend);
		}

		public override string GetScreenDeviceName (object backend)
		{
			var d = (Android.Views.Display)backend;
			return d.DisplayId.ToString ();
		}


	}
}

