/*
 * $Id: GLContext.cs 4831 2007-02-10 14:18:53Z anmaster $
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

	internal abstract class GLContext
	{
		private static bool useWGLContext;

		public static GLContext CreateContext (int[] attrs,
			GLContext share, IntPtr gdkDrawable)
		{
			if(!useWGLContext) {
				try {
					return new X11GLContext(attrs,
					                        (X11GLContext) share, gdkDrawable);
				} catch(DllNotFoundException e) {
					Console.WriteLine("Failed setting up X11 opengl context: " + e.Message);
					Console.WriteLine("  (If you run Windows this is normal, on any other OS it would be bad)");
					Console.WriteLine(e.StackTrace);
					Console.WriteLine("Trying win32 API");
					useWGLContext = true;
				}

				return CreateContext(attrs, share, gdkDrawable);
			} else {
				return new W32GLContext(attrs,
				                        (W32GLContext) share, gdkDrawable);
			}
		}

		public abstract bool MakeCurrent (IntPtr gdkDrawable);
		public abstract void SwapBuffers (IntPtr gdkDrawable);
	}
}
