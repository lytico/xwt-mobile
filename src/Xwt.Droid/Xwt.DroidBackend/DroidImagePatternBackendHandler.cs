// 
// DroidImagePatternBackendHandler.cs
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
using Xwt.Backends;
using AG = Android.Graphics;

namespace Xwt.DroidBackend
{

	public class DroidImagePatternBackendHandler : ImagePatternBackendHandler
	{

		public override object Create (ImageDescription img)
		{
			var image = img.Backend as DroidImage;
			return new AG.BitmapShader (image.Image, AG.Shader.TileMode.Repeat, AG.Shader.TileMode.Repeat);
		}

	}

	public class DroidGradientBackendHandler : GradientBackendHandler {

		public override object CreateLinear (double x0, double y0, double x1, double y1)
		{
			return new AG.LinearGradient ((float)x0, (float)y0, (float)x1, (float)y1,null,null,AG.Shader.TileMode.Clamp);
		}

		public override object CreateRadial (double cx0, double cy0, double radius0, double cx1, double cy1, double radius1)
		{
			return new AG.RadialGradient ((float)cx0, (float)cy0, (float)radius0, null, null, AG.Shader.TileMode.Clamp);
		}

		public override void AddColorStop (object backend, double position, Xwt.Drawing.Color color)
		{

		}

	}
}