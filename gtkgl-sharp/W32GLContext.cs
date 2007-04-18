/*
 * $Id: W32GLContext.cs 4703 2007-01-28 13:44:03Z anmaster $
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

	internal sealed class W32GLContext : GLContext
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct PIXELFORMATDESCRIPTOR
		{
			public ushort nSize;
			public ushort nVersion;
			public uint dwFlags;
			public byte iPixelType;
			public byte cColorBits;
			public byte cRedBits;
			public byte cRedShift;
			public byte cGreenBits;
			public byte cGreenShift;
			public byte cBlueBits;
			public byte cBlueShift;
			public byte cAlphaBits;
			public byte cAlphaShift;
			public byte cAccumBits;
			public byte cAccumRedBits;
			public byte cAccumGreenBits;
			public byte cAccumBlueBits;
			public byte cAccumAlphaBits;
			public byte cDepthBits;
			public byte cStencilBits;
			public byte cAuxBuffers;
			public byte iLayerType;
			public byte bReserved;
			public uint dwLayerMask;
			public uint dwVisibleMask;
			public uint dwDamageMask;
		}

		/* pixel types */
		internal const byte PFD_TYPE_RGBA = 0;
		internal const byte PFD_TYPE_COLORINDEX = 1;

		/* layer types */
		internal const uint PFD_MAIN_PLANE = 0;
		internal const uint PFD_OVERLAY_PLANE = 1;
		internal const uint PFD_UNDERLAY_PLANE = 0xff; // (-1)

		/* PIXELFORMATDESCRIPTOR flags */
		internal const uint PFD_DOUBLEBUFFER = 0x00000001;
		internal const uint PFD_STEREO = 0x00000002;
		internal const uint PFD_DRAW_TO_WINDOW = 0x00000004;
		internal const uint PFD_DRAW_TO_BITMAP = 0x00000008;
		internal const uint PFD_SUPPORT_GDI = 0x00000010;
		internal const uint PFD_SUPPORT_OPENGL = 0x00000020;
		internal const uint PFD_GENERIC_FORMAT = 0x00000040;
		internal const uint PFD_NEED_PALETTE = 0x00000080;
		internal const uint PFD_NEED_SYSTEM_PALETTE = 0x00000100;
		internal const uint PFD_SWAP_EXCHANGE = 0x00000200;
		internal const uint PFD_SWAP_COPY = 0x00000400;
		internal const uint PFD_SWAP_LAYER_BUFFERS = 0x00000800;
		internal const uint PFD_GENERIC_ACCELERATED = 0x00001000;
		internal const uint PFD_SUPPORT_DIRECTDRAW = 0x00002000;

		/* PIXELFORMATDESCRIPTOR flags for use in ChoosePixelFormat only */
		internal const uint PFD_DEPTH_DONTCARE = 0x20000000;
		internal const uint PFD_DOUBLEBUFFER_DONTCARE = 0x40000000;
		internal const uint PFD_STEREO_DONTCARE = 0x80000000;

		public const string GL_DLL = "opengl32.dll";

		[DllImport("libgdk-win32-2.0-0.dll"), SuppressUnmanagedCodeSecurityAttribute]
		static extern IntPtr gdk_win32_drawable_get_handle(IntPtr d);

		[DllImport("user32"), SuppressUnmanagedCodeSecurityAttribute]
		static extern IntPtr GetDC(IntPtr hwnd);

		[DllImport("gdi32"), SuppressUnmanagedCodeSecurityAttribute]
		static extern int ChoosePixelFormat(IntPtr hdc, IntPtr /*PIXELFORMATDESCRIPTOR*/ pfd);

		[DllImport("gdi32"), SuppressUnmanagedCodeSecurityAttribute]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetPixelFormat(IntPtr hdc, int iPixelFormat, IntPtr /*PIXELFORMATDESCRIPTOR*/ pfd);

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		static extern IntPtr wglCreateContext(IntPtr hdc);

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool wglMakeCurrent(IntPtr hdc, IntPtr hglrc);

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool wglDeleteContext(IntPtr hglrc);

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool wglShareLists(IntPtr context1, IntPtr context2);

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		static extern IntPtr wglSwapBuffers(IntPtr hdc);

		[DllImport(GL_DLL), SuppressUnmanagedCodeSecurityAttribute]
		static extern void glFlush();

		IntPtr deviceContext;
		IntPtr renderingContext;
		bool initialized;
		int[] attributes;
		W32GLContext share;

		internal W32GLContext(int[] attributeList,
			W32GLContext share,
			IntPtr gdkDrawable)
		{
			this.attributes = attributeList;
			this.share = share;

			// force .net to load the OpenGL dll :-/
			glFlush();
		}

		~W32GLContext()
		{
			if (renderingContext != IntPtr.Zero) {
				wglDeleteContext(renderingContext);
				renderingContext = IntPtr.Zero;
			}
		}

		public unsafe override bool MakeCurrent(IntPtr gdkDrawable)
		{
			if (!initialized) {
				IntPtr hwnd = gdk_win32_drawable_get_handle(gdkDrawable);
				deviceContext = GetDC(hwnd);

				// pfd Tells Windows How We Want Things To Be
				PIXELFORMATDESCRIPTOR pfd = new PIXELFORMATDESCRIPTOR();
				// Size Of This Pixel Format Descriptor
				pfd.nSize = (ushort)sizeof(PIXELFORMATDESCRIPTOR);
				pfd.nVersion = 1;
				pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL;
				pfd.iPixelType = PFD_TYPE_COLORINDEX;
				pfd.cColorBits = 32;

				for (int i = 0; i < attributes.Length; ++i) {
					switch (attributes[i]) {
					case GLContextAttributes.AccumAlphaSize:
						pfd.cAccumAlphaBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.AccumBlueSize:
						pfd.cAccumBlueBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.AccumGreenSize:
						pfd.cAccumGreenBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.AccumRedSize:
						pfd.cAccumRedBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.AlphaSize:
						pfd.cAlphaBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.AuxBuffers:
						pfd.cAuxBuffers = (byte)attributes[++i];
						break;
					case GLContextAttributes.BlueSize:
						pfd.cBlueBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.BufferSize:
						// ?!?
						++i;
						break;
					case GLContextAttributes.DepthSize:
						pfd.cDepthBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.DoubleBuffer:
						pfd.dwFlags |= PFD_DOUBLEBUFFER;
						break;
					case GLContextAttributes.GreenSize:
						pfd.cGreenBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.Level:
						// ?
						++i;
						break;
					case GLContextAttributes.None:
						// ?
						break;
					case GLContextAttributes.RedSize:
						pfd.cRedBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.Rgba:
						pfd.iPixelType = PFD_TYPE_RGBA;
						break;
					case GLContextAttributes.StencilSize:
						pfd.cStencilBits = (byte)attributes[++i];
						break;
					case GLContextAttributes.Stereo:
						pfd.dwFlags |= PFD_STEREO;
						break;
					}
				}

				// Tell the graphics hardware how to display pixels.
				int pixelFormat = ChoosePixelFormat(deviceContext, new IntPtr(&pfd));
				if (pixelFormat == 0)
					return false;

				if (!SetPixelFormat(deviceContext, pixelFormat, new IntPtr(&pfd)))
					throw new Exception("Couldn't set pixelformat");

				renderingContext = wglCreateContext(deviceContext);
				if (renderingContext == IntPtr.Zero)
					throw new Exception("Couldn't create rendering context");

				if (share != null) {
					if (share.renderingContext != IntPtr.Zero) {
						Console.WriteLine("DoSharing");
						if (!wglShareLists(share.renderingContext, renderingContext))
							throw new Exception("Can't share opengl contexts");
					} else
						throw new Exception("Trying to share with uninitialized context...");
				} else {
					Console.WriteLine("Share zero");
				}

				initialized = true;
			}

			return wglMakeCurrent(deviceContext, renderingContext);
		}

		public override void SwapBuffers(IntPtr gdkDrawable)
		{
			wglSwapBuffers(deviceContext);
		}
	}
}
