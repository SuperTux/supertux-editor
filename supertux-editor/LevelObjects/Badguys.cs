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

using Sprites;
using System;
using LispReader;
using DataStructures;
using System.Collections.Generic;
using SceneGraph;
using OpenGl;

[SupertuxObject("mrbomb", "images/creatures/mr_bomb/mr_bomb.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrBomb : SimpleDirObject
{
	[PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
	[ChooseResourceSetting]
	[LispChild("sprite", Optional = true, Default = "")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value)){
				//TODO: find out why cherry's sprite causes problems. Particles?
				Sprite newSprite= SpriteManager.Create(value);
				newSprite.Action = "left";
				Sprite = newSprite;	//save new sprite after (no exception only)
			}
			spriteFile = value;
		}
	}
	private string spriteFile = String.Empty;
	public MrBomb() {
		Sprite = SpriteManager.Create("images/creatures/mr_bomb/mr_bomb.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("haywire", "images/creatures/haywire/haywire.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Haywire : SimpleDirObject
{
	public Haywire() {
		Sprite = SpriteManager.Create("images/creatures/haywire/haywire.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("snowball", "images/creatures/snowball/snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Snowball : SimpleDirObject
{
	public Snowball() {
		Sprite = SpriteManager.Create("images/creatures/snowball/snowball.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("smartball", "images/creatures/snowball/smart-snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrsSnowball : SimpleDirObject
{
	public MrsSnowball() {
		Sprite = SpriteManager.Create("images/creatures/snowball/smart-snowball.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("captainsnowball", "images/creatures/snowball/cpt-snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class CaptainSnowball : SimpleDirObject
{
	public CaptainSnowball() {
		Sprite = SpriteManager.Create("images/creatures/snowball/cpt-snowball.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("kamikazesnowball", "images/creatures/snowball/kamikaze-snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Snowshot : SimpleDirObject
{
	public Snowshot() {
		Sprite = SpriteManager.Create("images/creatures/snowball/kamikaze-snowball.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("crystallo", "images/creatures/crystallo/crystallo.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Crystallo : SimpleDirObject
{
	[LispChild("radius", Optional = true, Default = 100f)]
	public float Radius = 100f;

	public Crystallo() {
		Sprite = SpriteManager.Create("images/creatures/crystallo/crystallo.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("stalactite", "images/creatures/stalactite/stalactite.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Stalactite : SimpleObject
{
	public Stalactite() {
		Sprite = SpriteManager.Create("images/creatures/stalactite/stalactite.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("mriceblock", "images/creatures/mr_iceblock/mr_iceblock.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrIceBlock : SimpleDirObject
{
	public MrIceBlock() {
		Sprite = SpriteManager.Create("images/creatures/mr_iceblock/mr_iceblock.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("bouncingsnowball",
                "images/creatures/bouncing_snowball/bouncing_snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class BouncingSnowball : SimpleDirObject
{
	public BouncingSnowball() {
		Sprite = SpriteManager.Create("images/creatures/bouncing_snowball/bouncing_snowball.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("flyingsnowball",
                "images/creatures/flying_snowball/flying_snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class FlyingSnowball : SimpleObject
{
	public FlyingSnowball() {
		Sprite = SpriteManager.Create("images/creatures/flying_snowball/flying_snowball.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("jumpy", "images/creatures/snowjumpy/snowjumpy.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "left-up")]
public sealed class Jumpy : SimpleObject
{
	public Jumpy() {
		Sprite = SpriteManager.Create("images/creatures/snowjumpy/snowjumpy.sprite");
		Sprite.Action = "left-up";
	}
}

[SupertuxObject("spiky", "images/creatures/spiky/spiky.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Spiky : SimpleDirObject
{
	public Spiky() {
		Sprite = SpriteManager.Create("images/creatures/spiky/spiky.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("sspiky", "images/creatures/spiky/sleepingspiky.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "sleeping-left")]
public sealed class SleepSpiky : SimpleDirObject
{
	protected override void DirectionChanged() {
		Sprite.Action = (Direction == Directions.right) ? "sleeping-right" : "sleeping-left";
	}

	public SleepSpiky() {
		Sprite = SpriteManager.Create("images/creatures/spiky/sleepingspiky.sprite");
		Sprite.Action = "sleeping-left";
	}
}

[SupertuxObject("flame", "images/creatures/flame/flame.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Flame : SimpleObject
{
	[LispChild("radius", Optional = true, Default = 100f)]
	public float Radius = 100f;
	[LispChild("speed", Optional = true, Default = 2f)]
	public float Speed = 2;

	public Flame() {
		Sprite = SpriteManager.Create("images/creatures/flame/flame.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("fish", "images/creatures/fish/fish.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Fish : SimpleObject
{
	public Fish() {
		Sprite = SpriteManager.Create("images/creatures/fish/fish.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("snowman", "images/creatures/snowman/snowman.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Snowman : SimpleDirObject
{
	public Snowman() {
		Sprite = SpriteManager.Create("images/creatures/snowman/snowman.sprite");
		Sprite.Action = "walk-left";
	}
}

[SupertuxObject("mrtree", "images/creatures/mr_tree/mr_tree.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrTree : SimpleDirObject
{
	public MrTree() {
		Sprite = SpriteManager.Create("images/creatures/mr_tree/mr_tree.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("stumpy", "images/creatures/mr_tree/stumpy.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Stumpy : SimpleDirObject
{
	public Stumpy() {
		Sprite = SpriteManager.Create("images/creatures/mr_tree/stumpy.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("poisonivy", "images/creatures/poison_ivy/poison_ivy.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class PoisonIvy : SimpleDirObject
{
	public PoisonIvy() {
		Sprite = SpriteManager.Create("images/creatures/poison_ivy/poison_ivy.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("walkingleaf", "images/creatures/walkingleaf/walkingleaf.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class WalkingLeaf : SimpleDirObject
{
	public WalkingLeaf() {
		Sprite = SpriteManager.Create("images/creatures/walkingleaf/walkingleaf.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("zeekling", "images/creatures/zeekling/zeekling.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Zeekling : SimpleDirObject
{
	public Zeekling() {
		Sprite = SpriteManager.Create("images/creatures/zeekling/zeekling.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("dart", "images/creatures/dart/dart.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
		ObjectListAction = "flying-left")]
public sealed class Dart : SimpleDirObject
{
	public Dart() {
		Sprite = SpriteManager.Create("images/creatures/dart/dart.sprite");
		Sprite.Action = "flying-left";
	}

	protected override void DirectionChanged () {
		if (direction == Directions.right) {
			Sprite.Action = "flying-right";
		} else {
			Sprite.Action = "flying-left";
		}
	}
}

[SupertuxObject("snail", "images/creatures/snail/snail.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Snail : SimpleDirObject
{
	public Snail() {
		Sprite = SpriteManager.Create("images/creatures/snail/snail.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("totem", "images/creatures/totem/totem.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "walking-left")]
public sealed class Totem : SimpleObject
{
	public Totem() {
		Sprite = SpriteManager.Create("images/creatures/totem/totem.sprite");
		Sprite.Action = "walking-left";
	}
}

[SupertuxObject("kugelblitz", "images/creatures/kugelblitz/kugelblitz.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "falling")]
public sealed class Kugelblitz : SimpleObject
{
	public Kugelblitz() {
		Sprite = SpriteManager.Create("images/creatures/kugelblitz/kugelblitz.sprite");
		Sprite.Action = "falling";
	}
}

[SupertuxObject("dispenser", "images/creatures/dispenser/dispenser.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "working")]
public sealed class Dispenser : SimpleDirObject
{
	private bool random;

	[PropertyProperties(Tooltip = "Put a tick here to make badguys order random")]
	[LispChild("random", Optional = true, Default = false)]
	public bool Random {
		get {
			return random;
		}
		set {
			random = value;
		}
	}


	/// <summary>
	/// Type of dispenser.
	/// </summary>
	public enum DispenserTypes {
		rocketlauncher,
		cannon,
		dropper
	}

	private DispenserTypes dispenserType = DispenserTypes.cannon;
	private List<string> badguy = new List<string>();

	[PropertyProperties(Tooltip = "Type of dispenser.", RedrawOnChange = true)]
	[LispChild("type", Optional = true, Default = DispenserTypes.dropper)]
	public DispenserTypes DispenserType {
		get {
			return dispenserType;
		}
		set {
			dispenserType = value;
			if (value == DispenserTypes.rocketlauncher)
				Sprite.Action = (Direction == Directions.right) ? "working-right" : "working-left";
			else if (value == DispenserTypes.cannon)
				Sprite.Action = "working";
			else
				Sprite.Action = "dropper";
		}
	}

	[ChooseBadguySetting]
	[PropertyProperties(Tooltip = "Badguys which will dispenser shoot. To add badguy just drag it here from badguy list.")]
	[LispChild("badguy")]
	public List<string> Badguy {
		get {
			return badguy;
		}
		set {
			badguy = value;
		}
	}

	protected override void DirectionChanged() {
		if (dispenserType == DispenserTypes.rocketlauncher) {
			Sprite.Action = (Direction == Directions.right) ? "working-right" : "working-left";
		}
	}

	[LispChild("cycle")]
	public float Cycle = 5;

	public Dispenser() {
		Sprite = SpriteManager.Create("images/creatures/dispenser/dispenser.sprite");
		Sprite.Action = "working";
		badguy.Add("kamikazesnowball");
	}

	public override object Clone() {
		Dispenser aClone = (Dispenser) MemberwiseClone();
		aClone.badguy = new List<string>(aClone.badguy);
		return aClone;
	}
}

[SupertuxObject("angrystone", "images/creatures/angrystone/angrystone.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "idle")]
public sealed class AngryStone : SimpleObject
{
	public AngryStone() {
		Sprite = SpriteManager.Create("images/creatures/angrystone/angrystone.sprite");
		Sprite.Action = "idle";
	}
}

[SupertuxObject("icecrusher", "images/creatures/icecrusher/icecrusher.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "idle")]
public sealed class IceCrusher : SimpleObject
{
	public IceCrusher() {
		Sprite = SpriteManager.Create("images/creatures/icecrusher/icecrusher.sprite");
		Sprite.Action = "idle";
	}
}


[SupertuxObject("spidermite", "images/creatures/spidermite/spidermite.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Spidermite : SimpleObject
{
	public Spidermite() {
		Sprite = SpriteManager.Create("images/creatures/spidermite/spidermite.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("plant", "images/creatures/plant/plant.sprite",
                Target = SupertuxObjectAttribute.Usage.None)]
public sealed class Plant : SimpleObject
{
	public Plant() {
		Sprite = SpriteManager.Create("images/creatures/plant/plant.sprite");
	}
}

[SupertuxObject("nolok_01", "images/creatures/nolok/nolok.sprite",
                Target = SupertuxObjectAttribute.Usage.None)]
public sealed class Nolok_01 : SimpleObject
{
	public Nolok_01() {
		Sprite = SpriteManager.Create("images/creatures/nolok/nolok.sprite");
	}
}

[SupertuxObject("willowisp",
                "images/creatures/willowisp/willowisp.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "idle")]
public sealed class WilloWisp : SimpleObject, IPathObject
{
	[LispChild("sector"), ChooseSectorSetting()]
	public string Sector = String.Empty;
	[LispChild("spawnpoint")]
	public string SpawnPoint = String.Empty;
	[LispChild("name")]
	public string Name;
	[LispChild("flyspeed", Optional = true, Default=64f)]
	public float FlySpeed = 64f;
	[LispChild("track-range", Optional = true, Default=384f)]
	public float TrackRange = 384f;
	[LispChild("vanish-range", Optional = true, Default=512f)]
	public float VanishRange = 512f;
	[LispChild("hit-script", Optional = true, Default = "")]
	[EditScriptSetting]
	public string HitScript;

	private Path path;
	[LispChild("path", Optional = true, Default = null)]
	public Path Path {
		get {
			return path;
		}
		set {
			path = value;
		}
	}

	public bool PathRemovable {
		get { return true; }
	}

	public override void ChangeArea(RectangleF NewArea) {
		base.ChangeArea(NewArea);
		if (path != null) {
			Vector translation = new Vector(NewArea.Left - Path.Nodes[0].X,
			                                NewArea.Top - Path.Nodes[0].Y);
			Path.Move(translation);
		}
	}

	public override RectangleF Area {
		get {
			if (path != null) {
				X = Path.Nodes[0].X;	//object is always at the first path node
				Y = Path.Nodes[0].Y;
			}
			return base.Area;
		}
	}

	public WilloWisp() {
		Sprite = SpriteManager.Create("images/creatures/willowisp/willowisp.sprite");
		Sprite.Action = "idle";
	}
}

[SupertuxObject("darttrap", "images/creatures/darttrap/darttrap.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "idle-left")]
public sealed class DartTrap : SimpleDirObject
{
	[LispChild("initial-delay")]
	public float initial_delay = 0;
	[LispChild("fire-delay")]
	public float fire_delay = 2;
	[LispChild("ammo")]
	public int ammo = -1;

	protected override void DirectionChanged() {
		Sprite.Action = (Direction == Directions.right) ? "idle-right" : "idle-left";
	}

	public DartTrap() {
		Sprite = SpriteManager.Create("images/creatures/darttrap/darttrap.sprite");
		Sprite.Action = "idle-left";
	}
}

[SupertuxObject("skullyhop", "images/creatures/skullyhop/skullyhop.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "standing-left")]
public sealed class SkullyHop : SimpleDirObject
{
	protected override void DirectionChanged() {
		Sprite.Action = (Direction == Directions.right) ? "standing-right" : "standing-left";
	}

	public SkullyHop() {
		Sprite = SpriteManager.Create("images/creatures/skullyhop/skullyhop.sprite");
		Sprite.Action = "standing-left";
	}
}

[SupertuxObject("igel", "images/creatures/igel/igel.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "walking-left")]
public sealed class Igel : SimpleDirObject
{
	public Igel() {
		Sprite = SpriteManager.Create("images/creatures/igel/igel.sprite");
		Sprite.Action = "walking-left";
	}
}

[SupertuxObject("toad", "images/creatures/toad/toad.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction ="idle-left")]
public sealed class Toad : SimpleDirObject
{
	protected override void DirectionChanged() {
		Sprite.Action = (Direction == Directions.right) ? "idle-right" : "idle-left";
	}

	public Toad() {
		Sprite = SpriteManager.Create("images/creatures/toad/toad.sprite");
		Sprite.Action = "idle-left";
	}
}

[SupertuxObject("mole", "images/creatures/mole/mole.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "idle")]
public sealed class Mole : SimpleObject
{
	public Mole() {
		Sprite = SpriteManager.Create("images/creatures/mole/mole.sprite");
		Sprite.Action = "idle";
	}
}

#region Bosses
[SupertuxObject("yeti", "images/creatures/yeti/yeti.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "stand-left")]
public sealed class Yeti : SimpleObject
{

	[LispChild("dead-script", Optional = true, Default = "")]
	[EditScriptSetting]
	public String DeadScript = String.Empty;

	public Yeti() {
		Sprite = SpriteManager.Create("images/creatures/yeti/yeti.sprite");
		Sprite.Action = "stand-left";
	}
}

[SupertuxObject("yeti_stalactite", "images/engine/editor/stalactite_yeti.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class StalactiteYeti : SimpleObject
{
	public StalactiteYeti() {
		Sprite = SpriteManager.Create("images/creatures/stalactite/stalactite.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("ghosttree", "images/creatures/ghosttree/ghosttree.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "default")]
public sealed class GhostTree : SimpleObject
{
	public GhostTree() {
		Sprite = SpriteManager.Create("images/creatures/ghosttree/ghosttree.sprite");
		Sprite.Action = "default";
	}
}
#endregion Bosses

/* EOF */
