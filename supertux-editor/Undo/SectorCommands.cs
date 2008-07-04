//  $Id$
//
//  Copyright (C) 2007 Arvid Norlander <anmaster AT berlios DOT de>
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
//  02111-1307, USA.
using System;
using System.Collections.Generic;

namespace Undo {
	public abstract class SectorCommand : Command {
		protected Sector sector;
		protected SectorCommand(string title, Sector sector): base(title) {
			this.sector = sector;
		}
	}


	internal sealed class SectorSizeChangeCommand : SectorCommand {
		private struct TilemapData {
			public TileBlock.StateData OldState;
			public Tilemap Tilemap;

			public TilemapData(TileBlock.StateData OldState, Tilemap Tilemap) {
				this.OldState = OldState;
				this.Tilemap = Tilemap;
			}
		}
		private List<TilemapData> tilemaps;

		private uint newWidth;
		private uint newHeight;
		private uint minWidth;
		private uint minHeight;

		public override void Do() {
			foreach (TilemapData tilemapdata in tilemaps) {
				//skip this tilemap, if it's smaller than resizing parameters
				if (tilemapdata.Tilemap.Width < minWidth && tilemapdata.Tilemap.Height < minHeight)
					continue;
				tilemapdata.Tilemap.Resize(newWidth, newHeight, 0);
			}
			sector.EmitSizeChanged();
		}

		public override void Undo() {
			foreach (TilemapData tilemapdata in tilemaps) {
				tilemapdata.Tilemap.RestoreState(tilemapdata.OldState);
			}
			sector.EmitSizeChanged();
		}

		private SectorSizeChangeCommand(string title, Sector sector, List<TilemapData> tilemaps, uint newWidth, uint newHeight, uint oldWidth, uint oldHeight)
			: base(title, sector) {
			this.tilemaps = tilemaps;
			this.newWidth = newWidth;
			this.newHeight = newHeight;
			this.minWidth = Math.Min(oldWidth, newWidth);
			this.minHeight = Math.Min(oldHeight, newHeight);
		}

		internal SectorSizeChangeCommand(string title, Sector sector, uint newWidth, uint newHeight, uint oldWidth, uint oldHeight)
			: this(title, sector, new List<TilemapData>(), newWidth, newHeight, oldWidth, oldHeight) {
			foreach (Tilemap tilemap in sector.GetObjects(typeof(Tilemap))) {
				tilemaps.Add(new TilemapData(tilemap.SaveState(), tilemap));
			}
		}

		internal SectorSizeChangeCommand(string title, Sector sector, Tilemap tilemap, uint newWidth, uint newHeight, uint oldWidth, uint oldHeight)
			: this(title, sector, new List<TilemapData>(){new TilemapData(tilemap.SaveState(), tilemap)}, newWidth, newHeight, oldWidth, oldHeight) {
			}

		internal SectorSizeChangeCommand(string title, Sector sector, uint newWidth, uint newHeight)
			:this(title, sector, newWidth, newHeight, 0, 0)
		{ }
	}

	public delegate void SectorsAddRemoveHandler();


	public abstract class SectorAddRemoveCommand : SectorCommand {
		protected Level level;
		public event SectorsAddRemoveHandler OnSectorAddRemove;

		public override void Do() {
			if (OnSectorAddRemove != null)
				OnSectorAddRemove();
		}

		public override void Undo() {
			if (OnSectorAddRemove != null)
				OnSectorAddRemove();
		}

		protected SectorAddRemoveCommand(string title, Sector sector, Level level)
			: base(title, sector) {
			this.level = level;
		}

	}

	public sealed class SectorAddCommand : SectorAddRemoveCommand {
		public override void Do() {
			level.Sectors.Add(sector);
			base.Do();
		}

		public override void Undo() {
			level.Sectors.Remove(sector);
			base.Undo();
		}

		public SectorAddCommand(string title, Sector sector, Level level) : base(title, sector, level) { }
	}

	public sealed class SectorRemoveCommand : SectorAddRemoveCommand {
		public override void Do() {
			level.Sectors.Remove(sector);
			base.Do();
		}

		public override void Undo() {
			level.Sectors.Add(sector);
			base.Undo();
		}

		public SectorRemoveCommand(string title, Sector sector, Level level) : base(title, sector, level) { }
	}


}
