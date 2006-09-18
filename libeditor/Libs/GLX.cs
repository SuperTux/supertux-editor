/*
 * GtkGL# - OpenGL Graphics Library for the Gtk# Toolkit
 *
 * Copyright (c) 2002-2003 The Olympum Group, http://www.olympum.com/
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

namespace OpenGl
{
	using System;
	using System.Runtime.InteropServices;

	public class GLX
	{
		public const int None                 = 0;  // end of GLX attribute list
		public const int GLX_USE_GL           = 1;  // support GLX rendering
		public const int GLX_BUFFER_SIZE      = 2;  // depth of the color buffer
		public const int GLX_LEVEL            = 3;  // level in plane stacking
		public const int GLX_RGBA             = 4;  // true if RGBA mode
		public const int GLX_DOUBLEBUFFER     = 5;  // double buffering supported
		public const int GLX_STEREO           = 6;  // stereo buffering supported
		public const int GLX_AUX_BUFFERS      = 7;  // number of aux buffers
		public const int GLX_RED_SIZE         = 8;  // number of red component bits
		public const int GLX_GREEN_SIZE       = 9;  // number of green component bits
		public const int GLX_BLUE_SIZE        = 10; // number of blue component bits
		public const int GLX_ALPHA_SIZE       = 11; // number of alpha component bits
		public const int GLX_DEPTH_SIZE       = 12; // number of depth bits
		public const int GLX_STENCIL_SIZE     = 13; // number of stencil bits
		public const int GLX_ACCUM_RED_SIZE   = 14; // number of red accum bits
		public const int GLX_ACCUM_GREEN_SIZE = 15; // number of green accum bits
		public const int GLX_ACCUM_BLUE_SIZE  = 16; // number of blue accum bits
		public const int GLX_ACCUM_ALPHA_SIZE = 17; // number of alpha accum bits

	}
}
