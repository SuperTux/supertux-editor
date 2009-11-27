//  SuperTux Editor
//  Copyright (C) 2007 Arvid Norlander <anmaster AT berlios DOT de>
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
		private List<TilemapData> tilemaps = new List<TilemapData>();

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

		/// <summary> Base constructor for class </summary>
		/// <param name="title"> Title for undo record </param>
		/// <param name="sector"> Sector that we want resize </param>
		/// <param name="tilemap"> Tilemap we want resize, null means All tilemap in sector </param>
		/// <param name="newWidth"> Width that we want to apply </param>
		/// <param name="newHeight"> Height that we want to apply </param>
		/// <param name="oldWidth"> Used when you want to set different value </param>
		/// <param name="oldHeight"> Used when you want to set different value </param>
		internal SectorSizeChangeCommand(string title, Sector sector, Tilemap tilemap, 
						 uint newWidth, uint newHeight, 
						 uint oldWidth, uint oldHeight)
			: base(title, sector) 
		{
			this.newWidth = newWidth;
			this.newHeight = newHeight;
			this.minWidth = Math.Min(oldWidth, newWidth);
			this.minHeight = Math.Min(oldHeight, newHeight);
			if (tilemap != null){
				tilemaps.Add(new TilemapData(tilemap.SaveState(), tilemap));
				this.minWidth = 0;
				this.minHeight = 0;
			} else {
				foreach (Tilemap tmap in sector.GetObjects(typeof(Tilemap))) {
					tilemaps.Add(new TilemapData(tmap.SaveState(), tmap));
				}
			}
		}

		internal SectorSizeChangeCommand(string title, Sector sector, uint newWidth, uint newHeight)
			:this(title, sector, null, newWidth, newHeight, sector.Width, sector.Height)
		{ }

		internal SectorSizeChangeCommand(string title, Sector sector, Tilemap tilemap, uint newWidth, uint newHeight)
			:this(title, sector, tilemap, newWidth, newHeight, sector.Width, sector.Height)
		{ }
	}

	public delegate void SectorsAddRemoveHandler(Sector sector);


	public abstract class SectorAddRemoveCommand : SectorCommand {
		protected Level level;
		public event SectorsAddRemoveHandler OnSectorAdd;
		public event SectorsAddRemoveHandler OnSectorRemove;

		public override void Do() {
			if (OnSectorAdd != null)
				OnSectorAdd(sector);
		}

		public override void Undo() {
			if (OnSectorRemove != null)
				OnSectorRemove(sector);
		}

		protected SectorAddRemoveCommand(string title, Sector sector, Level level)
			: base(title, sector) {
			this.level = level;
		}

	}

	public class SectorAddCommand : SectorAddRemoveCommand {
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

	public sealed class SectorRemoveCommand : SectorAddCommand {
		public override void Do() {
			base.Undo();
		}

		public override void Undo() {
			base.Do();
		}

		public SectorRemoveCommand(string title, Sector sector, Level level) : base(title, sector, level) { }
	}


}

/* EOF */
