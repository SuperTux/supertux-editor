using System.Collections.Generic;
using DataStructures;

public abstract class WorldmapObject : GameObject {
    protected WorldmapSector Sector;
    protected List<WorldmapTux> TouchedPlayers = new List<WorldmapTux>();

    public WorldmapObject(WorldmapSector Sector) {
        this.Sector = Sector;
    }

    public abstract FieldPos Pos {
        get;
    }

    public virtual void Use(WorldmapTux Player) {

    }
    public virtual void Touch(WorldmapTux Player, bool touch) {
        if(touch)
            TouchedPlayers.Add(Player);
        else
            TouchedPlayers.Remove(Player);
    }
}
