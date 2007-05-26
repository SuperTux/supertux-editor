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
