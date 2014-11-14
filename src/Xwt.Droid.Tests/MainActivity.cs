using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xwt;

namespace SimpleDroidApp
{
	[Activity (Label = "Xwt.Droid Tests", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var tk = Toolkit.CreateToolkit<Xwt.DroidBackend.DroidEngine> (false);
			tk.SetActive ();
			Xwt.Application.Initialize (tk);

			Xwt.DroidBackend.DroidDesktopBackend.Context = this;

			SetContentView (new DrawingView (this));
		}

		protected override void OnStart ()
		{
			base.OnStart ();
		}

		protected override void OnStop ()
		{
			base.OnStop ();
		}

		protected override void OnPause ()
		{
			base.OnPause ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
		}

		protected override void OnRestart ()
		{
			base.OnRestart ();
		}
	}

}


