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

/* TODO
public interface DragAction
{
	void Draw();
	void Update(Vector dragDelta);
	void Finish(Vector dragDelta);
	void Abort();
}

public class CameraDrag
{
	Renderer renderer;
	Vector startTranslation;

	public CameraDrag(Renderer renderer)
	{
		startTranslation = renderer.Translation;
	}

	public void Draw()
	{
	}

	public void Update(Vector dragDelta)
	{
		renderer.Translation = startTranslation + dragDelta;
	}

	public void Finish(Vector dragDelta)
	{
		Update(dragDelta);
	}

	public void Abort()
	{
		renderer.Translation = startTranslation;
	}
}

public class ObjectMoveDrag
{
	Renderer renderer;
	IObject obj;
	Vector startPos;

	public ObjectMoveDrag(Renderer renderer, IObject obj)
	{
		this.renderer = renderer;
		this.obj = obj;
	}

	public void Draw()
	{
	}

	public void Update(Vector dragDelta)
	{
	}


}
*/

/* EOF */
