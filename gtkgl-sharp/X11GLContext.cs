/*
 * $Id$
 *
 * GtkGL# - OpenGL Graphics Library for the Gtk# Toolkit
 *
 * Copyright (c) 2002-2004 The Olympum Group, http://www.olympum.com/
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 * list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 *
 * - Neither the name of The Olympum Group nor the names of its contributors
 * may be used to endorse or promote products derived from this software without
 * specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
 * DAMAGE.
 */

namespace Gdk
{
	using System;
	using System.Runtime.InteropServices;
	using System.Security;

	internal sealed class X11GLContext : GLContext
	{
		private const string GL_DLL = "GL";

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		static extern bool glXMakeCurrent (IntPtr display,
			int drawableID,
			IntPtr glxcontext);

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		static extern void glXSwapBuffers (IntPtr display,
			int drawableID);

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		static extern IntPtr glXCreateContext (IntPtr display,
			IntPtr visualInfo,
			IntPtr shareList,
			bool direct);

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		static extern IntPtr glXChooseVisual (IntPtr display,
			int screen,
			int[] attribList);

		/*
		[DllImport(GL_DLL)]
		static extern void glXDestroyContext(IntPtr display,
		                                     IntPtr context);
		*/

		[DllImport("X11"), SuppressUnmanagedCodeSecurityAttribute]
		static extern void XFree (IntPtr reference);

		[DllImport("gdk-x11-2.0"), SuppressUnmanagedCodeSecurityAttribute]
		static extern IntPtr gdk_x11_get_default_xdisplay();

		[DllImport("gdk-x11-2.0"), SuppressUnmanagedCodeSecurityAttribute]
		static extern int gdk_x11_drawable_get_xid(IntPtr drawable);

		[DllImport("gdk-x11-2.0"), SuppressUnmanagedCodeSecurityAttribute]
		static extern int gdk_x11_get_default_screen();

		IntPtr xdisplay;
		IntPtr glxcontext;

		internal X11GLContext (int[] attributeList,
				X11GLContext share,
				IntPtr gdkDrawable)
		{
			// choose the visual based on attribute list
			xdisplay = gdk_x11_get_default_xdisplay();
			if (xdisplay == IntPtr.Zero)
				throw new SystemException("Couldn't get default display.");

			IntPtr visualInfo = glXChooseVisual (xdisplay,
				gdk_x11_get_default_screen(),
				attributeList);

			if (visualInfo == IntPtr.Zero)
				throw new SystemException ("No suitable glx-visual found.");

			// create glxcontext using visual
			try {
				IntPtr glxshare = IntPtr.Zero;
				if (share != null)
					glxshare = share.glxcontext;

				bool directRendering = true;
				//bool directRendering = false;

				glxcontext = glXCreateContext (xdisplay,
					visualInfo,
					glxshare,
					directRendering);
				if (glxcontext == IntPtr.Zero)
					throw new SystemException ("Failed to create glx-context.");
			} finally {
				XFree (visualInfo);
			}
		}

		~X11GLContext ()
		{
			if(glxcontext != IntPtr.Zero) {
				//glXDestroyContext (xdisplay, glxcontext);
				glxcontext = IntPtr.Zero;
			}
		}

		public override bool MakeCurrent (IntPtr gdkDrawable)
		{
			int id = gdk_x11_drawable_get_xid(gdkDrawable);
			try {
				return glXMakeCurrent(xdisplay, id, glxcontext);
			} catch (Exception ) {
				return false;
			}
		}

		public override void SwapBuffers (IntPtr gdkDrawable)
		{
			int id = gdk_x11_drawable_get_xid(gdkDrawable);
			glXSwapBuffers(xdisplay, id);
		}
	}
}
