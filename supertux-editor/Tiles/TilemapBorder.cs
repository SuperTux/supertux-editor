//  $Id$
using SceneGraph;
using OpenGl;

/// <summary>
///	A SceneGraph Node which draws border around currently active tilemap in application.
/// </summary>
public sealed class TilemapBorder : Node {

	public IEditorApplication application;

	public TilemapBorder(IEditorApplication application)
	{
		this.application = application;
	}

	public void Draw(Gdk.Rectangle cliprect)
	{
		if (application.CurrentTilemap == null)
			return;		//Stop, if we have no tilemap
		if (application.CurrentTilemap.Width ==  application.CurrentSector.Width &&
			application.CurrentTilemap.Width ==  application.CurrentSector.Width &&
			application.CurrentTilemap.X == 0 &&
			application.CurrentTilemap.Y == 0)
			return;		//Stop, if we have full-sized tilemap starting at [0,0]

		Tilemap tm = application.CurrentTilemap;

		gl.Disable(gl.TEXTURE_2D);
		gl.Begin(gl.LINE_LOOP);

		gl.Vertex2f(tm.X - 1,
				tm.Y - 1);
		gl.Vertex2f(tm.X + 1 + tm.Width * Tileset.TILE_WIDTH,
				tm.Y - 1);
		gl.Vertex2f(tm.X + 1 + tm.Width * Tileset.TILE_WIDTH,
				tm.Y + 1 + tm.Height * Tileset.TILE_HEIGHT);
		gl.Vertex2f(tm.X - 1,
				tm.Y + 1 + tm.Height * Tileset.TILE_HEIGHT);
		gl.End();

		gl.Enable(gl.TEXTURE_2D);
	}
}
