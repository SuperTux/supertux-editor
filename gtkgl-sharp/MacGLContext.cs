namespace Gdk
{
	using System;
	using OpenTK.Platform;
	using OpenTK.Graphics;
	using OpenTK.Graphics.OpenGL;

	internal sealed class MacGLContext : GLContext
	{
		internal OpenTK.Platform.IWindowInfo windowinfo;
		internal GraphicsContext ctx;
		internal MacGLContext (int[] attributeList,
		                       MacGLContext share,
		                       IntPtr gdkDrawable)
		{
			if (gdkDrawable == IntPtr.Zero) {
				Console.WriteLine ("ERROR: gdkDrawable is NULL");
			}
			windowinfo = OpenTK.Platform.Utilities.CreateMacOSCarbonWindowInfo (gdkDrawable, true, true);
			try {
				ctx = new GraphicsContext (GraphicsMode.Default, windowinfo);
			}
			catch(Exception e) {
				Console.WriteLine (e.Message);
				Console.WriteLine (e.StackTrace);
			}
		}

		~MacGLContext ()
		{
			if (ctx != null) {
				ctx.Dispose ();
				ctx = null;
			}
			if (windowinfo != null) {
				windowinfo.Dispose ();
				windowinfo = null;
			}
		}

		public override bool MakeCurrent (IntPtr gdkDrawable)
		{
			try {
				ctx.MakeCurrent(windowinfo);
				return true;
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				return false;
			}
		}

		public override void SwapBuffers (IntPtr gdkDrawable)
		{
			try {
				ctx.SwapBuffers();
			}
			catch(Exception e) {
				Console.WriteLine (e.Message);
			}
		}
	}
}
