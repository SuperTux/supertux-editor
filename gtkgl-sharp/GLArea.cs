/*
 * $Id: GLArea.cs 4703 2007-01-28 13:44:03Z anmaster $
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

namespace Gtk
{
	using System;
	using System.Runtime.InteropServices;
	using Gdk;

	public class GLArea : DrawingArea
	{
		GLContext context;

		public GLArea (int[] attributeList)
		{
			DoubleBuffered = false;
			context = GLContext.CreateContext (attributeList, null, IntPtr.Zero);
		}

		public GLArea (int[] attributeList, GLArea share)
		{
			DoubleBuffered = false;
			context = GLContext.CreateContext (attributeList,
			                                   share  != null ? share.context : null, IntPtr.Zero);
		}

		public bool MakeCurrent ()
		{
			return context.MakeCurrent (this.GdkWindow.Handle);
		}

		public void SwapBuffers ()
		{
			context.SwapBuffers (this.GdkWindow.Handle);
		}
	}
}
