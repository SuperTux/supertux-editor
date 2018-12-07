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
using Drawing;

#region Milestone 2

[SupertuxObject("mrbomb", "images/creatures/mr_bomb/mr_bomb.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrBomb : SimpleDirObject
{
	public MrBomb() {
		DefaultSpriteFile = "images/creatures/mr_bomb/mr_bomb.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("haywire", "images/creatures/haywire/haywire.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Haywire : SimpleDirObject
{
	public Haywire() {
		DefaultSpriteFile = "images/creatures/haywire/haywire.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("goldbomb", "images/creatures/gold_bomb/gold_bomb.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class GoldBomb : SimpleDirObject
{
	public GoldBomb() {
		DefaultSpriteFile = "images/creatures/gold_bomb/gold_bomb.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("short_fuse", "images/creatures/short_fuse/short_fuse.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Short_Fuse : SimpleDirObject
{
	public Short_Fuse() {
		DefaultSpriteFile = "images/creatures/short_fuse/short_fuse.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("snowball", "images/creatures/snowball/snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Snowball : SimpleDirObject
{
	public Snowball() {
		DefaultSpriteFile = "images/creatures/snowball/snowball.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("smartball", "images/creatures/snowball/smart-snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrsSnowball : SimpleDirObject
{
	public MrsSnowball() {
		DefaultSpriteFile = "images/creatures/snowball/smart-snowball.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("captainsnowball", "images/creatures/snowball/cpt-snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class CaptainSnowball : SimpleDirObject
{
	public CaptainSnowball() {
		DefaultSpriteFile = "images/creatures/snowball/cpt-snowball.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("kamikazesnowball", "images/creatures/snowball/kamikaze-snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Kamikazesnowball : SimpleDirObject
{
	public Kamikazesnowball() {
		DefaultSpriteFile = "images/creatures/snowball/kamikaze-snowball.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("crystallo", "images/creatures/crystallo/crystallo.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Crystallo : SimpleDirObject
{
	[LispChild("radius", Optional = true, Default = 100f)]
	public float Radius = 100f;

  //FIXME: this shouldn't be necessary
	public override void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left - Radius;
		Y = NewArea.Top;
		if(Sprite != null) {
			X += Sprite.Offset.X;
			Y += Sprite.Offset.Y;
		}
	}

	public Crystallo() {
		DefaultSpriteFile = "images/creatures/crystallo/crystallo.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "editor";
	}
}

[SupertuxObject("stalactite", "images/creatures/stalactite/stalactite.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Stalactite : SimpleBadguyObject
{
	public Stalactite() {
		DefaultSpriteFile = "images/creatures/stalactite/stalactite.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "normal";
	}
}

[SupertuxObject("mriceblock", "images/creatures/mr_iceblock/mr_iceblock.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrIceBlock : SimpleDirObject
{
	public MrIceBlock() {
		DefaultSpriteFile = "images/creatures/mr_iceblock/mr_iceblock.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("smartblock", "images/creatures/mr_iceblock/smart_block/smart_block.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SmartBlock : SimpleDirObject
{
	public SmartBlock() {
		DefaultSpriteFile = "images/creatures/mr_iceblock/smart_block/smart_block.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("bouncingsnowball",
                "images/creatures/bouncing_snowball/bouncing_snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class BouncingSnowball : SimpleDirObject
{
	public BouncingSnowball() {
		DefaultSpriteFile = "images/creatures/bouncing_snowball/bouncing_snowball.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("flyingsnowball",
                "images/creatures/flying_snowball/flying_snowball.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class FlyingSnowball : SimpleBadguyObject
{
  //FIXME: this shouldn't be necessary
	public override void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left - Sprite.Offset.X;
		Y = NewArea.Top - 83;
		if(Sprite != null) {
			X += Sprite.Offset.X;
			Y += Sprite.Offset.Y;
		}
	}

	public FlyingSnowball() {
		DefaultSpriteFile = "images/creatures/flying_snowball/flying_snowball.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "editor";
	}
}

[SupertuxObject("jumpy", "images/creatures/snowjumpy/snowjumpy.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "left-up")]
public sealed class Jumpy : SimpleBadguyObject
{
	public Jumpy() {
		DefaultSpriteFile = "images/creatures/snowjumpy/snowjumpy.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left-up";
	}
}

[SupertuxObject("spiky", "images/creatures/spiky/spiky.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Spiky : SimpleDirObject
{
	public Spiky() {
		DefaultSpriteFile = "images/creatures/spiky/spiky.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
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
		DefaultSpriteFile = "images/creatures/spiky/sleepingspiky.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "sleeping-left";
	}
}

[SupertuxObject("livefire", "images/creatures/livefire/livefire.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "left")]
public sealed class LiveFire : SimpleDirObject
{
	protected override void DirectionChanged() {
		Sprite.Action = (Direction == Directions.right) ? "right" : "left";
	}

	public LiveFire() {
		DefaultSpriteFile = "images/creatures/livefire/livefire.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("livefire_asleep", "images/creatures/livefire/livefire.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "waking-left")]
public sealed class LiveFireAsleep : SimpleDirObject
{
	protected override void DirectionChanged() {
		Sprite.Action = (Direction == Directions.right) ? "waking-right" : "waking-left";
	}

	public LiveFireAsleep() {
		DefaultSpriteFile = "images/creatures/livefire/livefire.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "waking-left";
	}
}

[SupertuxObject("livefire_dormant", "images/creatures/livefire/livefire.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "sleeping-left")]
public sealed class LiveFireDormant : SimpleDirObject
{
	protected override void DirectionChanged() {
		Sprite.Action = (Direction == Directions.right) ? "sleeping-right" : "sleeping-left";
	}

	public LiveFireDormant() {
		DefaultSpriteFile = "images/creatures/livefire/livefire.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "sleeping-left";
	}
}

[SupertuxObject("flame", "images/creatures/flame/flame.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Flame : SimpleSpriteObject
{
	[LispChild("radius", Optional = true, Default = 100f)]
	public float Radius = 100f;
	[LispChild("speed", Optional = true, Default = 2f)]
	public float Speed = 2;

  //FIXME: this shouldn't be necessary
	public override void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left - 96;
		Y = NewArea.Top - 96;
		if(Sprite != null) {
			X += Sprite.Offset.X;
			Y += Sprite.Offset.Y;
		}
	}

	public Flame() {
		DefaultSpriteFile = "images/creatures/flame/flame.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "editor";
	}
}

[SupertuxObject("iceflame", "images/creatures/flame/iceflame.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class IceFlame : SimpleSpriteObject
{
	[LispChild("radius", Optional = true, Default = 100f)]
	public float Radius = 100f;
	[LispChild("speed", Optional = true, Default = 2f)]
	public float Speed = 2;

  //FIXME: this shouldn't be necessary
	public override void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left - 96;
		Y = NewArea.Top - 96;
		if(Sprite != null) {
			X += Sprite.Offset.X;
			Y += Sprite.Offset.Y;
		}
	}

	public IceFlame() {
		DefaultSpriteFile = "images/creatures/flame/iceflame.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "editor";
	}
}

[SupertuxObject("ghostflame", "images/creatures/flame/ghostflame.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class GhostFlame : SimpleSpriteObject
{
	[LispChild("radius", Optional = true, Default = 100f)]
	public float Radius = 100f;

	[LispChild("speed", Optional = true, Default = 2f)]
	public float Speed = 2;

	//FIXME: this shouldn't be necessary
	public override void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left - 96;
		Y = NewArea.Top - 96;
		if(Sprite != null) {
			X += Sprite.Offset.X;
			Y += Sprite.Offset.Y;
		}
	}

	public GhostFlame() {
		DefaultSpriteFile = "images/creatures/flame/ghostflame.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "editor";
	}
}

[SupertuxObject("fish", "images/creatures/fish/fish.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Fish : SimpleBadguyObject
{
	public Fish() {
		DefaultSpriteFile = "images/creatures/fish/fish.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "normal";
	}
}

[SupertuxObject("snowman", "images/creatures/snowman/snowman.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Snowman : SimpleDirObject
{
	public Snowman() {
		DefaultSpriteFile = "images/creatures/snowman/snowman.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "walk-left";
	}
}

[SupertuxObject("owl", "images/creatures/owl/owl.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Owl : SimpleDirObject
{
	[LispChild("carry", Optional = true)]
	public string Carry;

	public Owl() {
		DefaultSpriteFile = "images/creatures/owl/owl.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("skydive", "images/creatures/skydive/skydive.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Skydive : SimpleDirObject
{
	[LispChild("carry", Optional = true)]
	public string Carry;

	public Skydive() {
		DefaultSpriteFile = "images/creatures/skydive/skydive.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
	}
}

[SupertuxObject("icecrusher", "images/creatures/icecrusher/icecrusher.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "crushing")]
public sealed class IceCrusher : SimpleSpriteObject
{
	public IceCrusher() {
		DefaultSpriteFile = "images/creatures/icecrusher/icecrusher.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "crushing";
	}
}

[SupertuxObject("dispenser", "images/creatures/dispenser/dispenser.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "dropper")]
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

	private DispenserTypes dispenserType = DispenserTypes.dropper;
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
	[PropertyProperties(Tooltip = "Badguys which dispenser will shoot. To add badguy just drag it here from badguy list.")]
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
		DefaultSpriteFile = "images/creatures/dispenser/dispenser.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "dropper";
		badguy.Add("snowball");
	}

	public override object Clone() {
		Dispenser aClone = (Dispenser) MemberwiseClone();
		aClone.badguy = new List<string>(aClone.badguy);
		return aClone;
	}
}

#endregion /* Milestone 2 */

[SupertuxObject("mrtree", "images/creatures/mr_tree/mr_tree.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrTree : SimpleDirObject
{
	public MrTree() {
		DefaultSpriteFile = "images/creatures/mr_tree/mr_tree.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("stumpy", "images/creatures/mr_tree/stumpy.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Stumpy : SimpleDirObject
{
	public Stumpy() {
		DefaultSpriteFile = "images/creatures/mr_tree/stumpy.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("poisonivy", "images/creatures/poison_ivy/poison_ivy.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class PoisonIvy : SimpleDirObject
{
	public PoisonIvy() {
		DefaultSpriteFile = "images/creatures/poison_ivy/poison_ivy.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("walkingleaf", "images/creatures/walkingleaf/walkingleaf.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class WalkingLeaf : SimpleDirObject
{
	public WalkingLeaf() {
		DefaultSpriteFile = "images/creatures/walkingleaf/walkingleaf.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("zeekling", "images/creatures/zeekling/zeekling.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Zeekling : SimpleDirObject
{
	public Zeekling() {
		DefaultSpriteFile = "images/creatures/zeekling/zeekling.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("dart", "images/creatures/dart/dart.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
		ObjectListAction = "flying-left")]
public sealed class Dart : SimpleDirObject
{
	public Dart() {
		DefaultSpriteFile = "images/creatures/dart/dart.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
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
		DefaultSpriteFile = "images/creatures/snail/snail.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("totem", "images/creatures/totem/totem.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "walking-left")]
public sealed class Totem : SimpleSpriteObject
{
	[LispChild("dead-script", Optional = true, Default = "")]
	[EditScriptSetting]
	public String DeadScript = String.Empty;

	public Totem() {
		DefaultSpriteFile = "images/creatures/totem/totem.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "walking-left";
	}
}

[SupertuxObject("kugelblitz", "images/creatures/kugelblitz/kugelblitz.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "falling")]
public sealed class Kugelblitz : SimpleSpriteObject
{
	public Kugelblitz() {
		DefaultSpriteFile = "images/creatures/kugelblitz/kugelblitz.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "falling";
	}
}

[SupertuxObject("angrystone", "images/creatures/angrystone/angrystone.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "idle")]
public sealed class AngryStone : SimpleSpriteObject
{
	public AngryStone() {
		DefaultSpriteFile = "images/creatures/angrystone/angrystone.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "idle";
	}
}

[SupertuxObject("spidermite", "images/creatures/spidermite/spidermite.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Spidermite : SimpleBadguyObject
{
	public Spidermite() {
		DefaultSpriteFile = "images/creatures/spidermite/spidermite.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "left";
	}
}

[SupertuxObject("plant", "images/creatures/plant/plant.sprite",
                Target = SupertuxObjectAttribute.Usage.None)]
public sealed class Plant : SimpleSpriteObject
{
	public Plant() {
		DefaultSpriteFile = "images/creatures/plant/plant.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
	}
}

[SupertuxObject("nolok_01", "images/creatures/nolok/nolok.sprite",
                Target = SupertuxObjectAttribute.Usage.None)]
public sealed class Nolok_01 : SimpleSpriteObject
{
	public Nolok_01() {
		DefaultSpriteFile = "images/creatures/nolok/nolok.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
	}
}

[SupertuxObject("willowisp",
                "images/creatures/willowisp/willowisp.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "idle")]
public sealed class WilloWisp : SimpleSpriteObject, IPathObject
{
	[LispChild("sector", Optional = true, Default = ""), ChooseSectorSetting()]
	public string Sector = String.Empty;
	[LispChild("spawnpoint", Optional = true, Default = "")]
	public string SpawnPoint = String.Empty;
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
		get { return path; }
		set { path = value; }
	}

	private string pathRef = String.Empty;

	[LispChild("path-ref", Optional = true, Default="")]
	public string PathRef {
		get { return pathRef; }
		set { pathRef = value; }
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
		DefaultSpriteFile = "images/creatures/willowisp/willowisp.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
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
		DefaultSpriteFile = "images/creatures/darttrap/darttrap.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
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
		DefaultSpriteFile = "images/creatures/skullyhop/skullyhop.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "standing-left";
	}
}

[SupertuxObject("igel", "images/creatures/igel/igel.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "walking-left")]
public sealed class Igel : SimpleDirObject
{
	public Igel() {
		DefaultSpriteFile = "images/creatures/igel/igel.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "walking-left";
	}
}

[SupertuxObject("walking_candle", "images/creatures/mr_candle/mr-candle.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class WalkingCandle : SimpleDirObject
{
	[ChooseColorSetting]
	[LispChild("color", Optional = true, Default = "Color(1, 1, 1, 1)")]
	public Color color = new Color(1, 1, 1);

	public WalkingCandle() {
		DefaultSpriteFile = "images/creatures/mr_candle/mr-candle.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
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
		DefaultSpriteFile = "images/creatures/toad/toad.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "idle-left";
	}
}

[SupertuxObject("mole", "images/creatures/mole/mole.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "idle")]
public sealed class Mole : SimpleBadguyObject
{
	public Mole() {
		DefaultSpriteFile = "images/creatures/mole/mole.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "idle";
	}
}

#region Bosses
[SupertuxObject("yeti", "images/creatures/yeti/yeti.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "stand-left")]
public sealed class Yeti : SimpleSpriteObject
{
	[LispChild("dead-script", Optional = true, Default = "")]
	[EditScriptSetting]
	public String DeadScript = String.Empty;

	public Yeti() {
		DefaultSpriteFile = "images/creatures/yeti/yeti.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "stand-left";
	}
}

[SupertuxObject("yeti_stalactite", "images/engine/editor/stalactite_yeti.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class StalactiteYeti : SimpleSpriteObject
{
	public StalactiteYeti() {
		DefaultSpriteFile = "images/creatures/stalactite/stalactite.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "normal";
	}
}

[SupertuxObject("ghosttree", "images/creatures/ghosttree/ghosttree.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "default")]
public sealed class GhostTree : SimpleSpriteObject
{
	[LispChild("dead-script")]
	[EditScriptSetting]
	public string DeadScript = String.Empty;

	public GhostTree() {
		DefaultSpriteFile = "images/creatures/ghosttree/ghosttree.sprite";
		Sprite = SpriteManager.Create(DefaultSpriteFile);
		Sprite.Action = "default";
	}
}
#endregion Bosses

/* EOF */
