// $Id$
using System;
using OpenGl;
using DataStructures;
using System.Collections.Generic;

namespace Drawing
{

	public class DrawingContext
	{
		private List<DrawingRequest> requests = new List<DrawingRequest>();

		private abstract class DrawingRequest
		{
			public int     Layer;
			public GlState State;

			public abstract void Draw();
		}

		private class SurfaceRequest : DrawingRequest
		{
			public Vector  Pos;
			public Surface Surface;

			public override void Draw() {
				Surface.Draw(Pos);
			}
		}

		public void DrawSurface(Surface surface, Vector position, int layer)
		{
			if(surface == null)
				throw new Exception("trying to draw null surface");

			SurfaceRequest request = new SurfaceRequest();
			request.State   = transform.State;
			request.Layer   = layer;
			request.Pos     = position;
			request.Surface = surface;

			requests.Add(request);
		}

		private class FillRectRequest : DrawingRequest
		{
			public RectangleF  Rect;
			public Color       Color;

			public override void Draw() {
				gl.Disable(gl.TEXTURE_2D);
				//gl.PolygonMode(gl.FRONT_AND_BACK, gl.FILL);

				gl.Begin(gl.QUADS);
				gl.Vertex2f(Rect.Left, Rect.Top);
				gl.Vertex2f(Rect.Right, Rect.Top);
				gl.Vertex2f(Rect.Right, Rect.Bottom);
				gl.Vertex2f(Rect.Left, Rect.Bottom);
				gl.End();

				//gl.PolygonMode(gl.FRONT_AND_BACK, gl.FILL);
				gl.Enable(gl.TEXTURE_2D);
			}
		}

		public void DrawFilledRect(RectangleF rect, Color color, int layer)
		{
			FillRectRequest request = new FillRectRequest();
			request.State = transform.State;
			request.Layer = layer;
			request.Rect  = rect;
			request.Color = color;

			requests.Add(request);
		}

		private class RequestComparer : IComparer<DrawingRequest>
		{
			public int Compare(DrawingRequest request1,
			                   DrawingRequest request2) {
			    if(request1.Layer < request2.Layer)
			    	return 1;
			    return 0;
			}
		}
		static RequestComparer comparer = new RequestComparer();

		public void DoDrawing()
		{
			requests.Sort(comparer);
			foreach(DrawingRequest request in requests) {
				request.Draw();
			}
			requests.RemoveRange(0, requests.Count);
		}

		public void PushTransform()
		{
		}

		public void PopTransform()
		{
		}

		public Vector Translation {
			get {
				return transform.Translation;
			}
			set {
				transform.Translation = value;
			}
		}
		public float Alpha {
			get {
				return transform.Alpha;
			}
			set {
				transform.Alpha = value;
			}
		}

		private Transform       transform      = new Transform();
		private List<Transform> transformStack = new List<Transform>();

		private class Transform
		{
			public Vector  Translation;
			public GlState State;
			public float   Alpha;
		}

	}

}
