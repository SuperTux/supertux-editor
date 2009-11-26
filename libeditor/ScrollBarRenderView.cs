//  SuperTux Editor
//  Copyright (C) 2006 Matthias Braun <matze@braunis.de>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Gtk;

public class ScrollBarRenderView : Table
{
	private RenderView renderview;

	public ScrollBarRenderView(RenderView renderview)
		: base(2, 2, false)
	{
		this.renderview = renderview;
		Attach(renderview, 0, 1, 0, 1,
		       AttachOptions.Expand | AttachOptions.Fill,
		       AttachOptions.Expand | AttachOptions.Fill, 0, 0);

		Adjustment hadjustment = new Adjustment(0, -10, 10, 1, 2, 2);
		HScrollbar hscroll = new HScrollbar(hadjustment);
		Attach(hscroll, 0, 1, 1, 2,
		         AttachOptions.Expand | AttachOptions.Fill, 0, 0, 0);

		Adjustment vadjustment = new Adjustment(0, -10, 10, 1, 2, 2);
		VScrollbar vscroll = new VScrollbar(vadjustment);
		Attach(vscroll, 1, 2, 0, 1,
		         0, AttachOptions.Expand | AttachOptions.Fill, 0, 0);

		renderview.SetAdjustments(hadjustment, vadjustment);
	}

	public RenderView Renderer {
		get {
			return renderview;
		}
	}
}

/* EOF */
