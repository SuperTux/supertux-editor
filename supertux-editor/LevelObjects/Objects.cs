//  $Id$
using Sprites;
using System;
using Lisp;
using LispReader;
using DataStructures;
using SceneGraph;
using OpenGl;

/// <summary>
/// Used to make it simpler to change common tooltip strings.
/// </summary>
internal static class ToolTipStrings {
	/// <summary>
	/// For the "Name" attribute used for scripting.
	/// </summary>
	internal const string ScriptingName = "Used to refer to the object from a script. If it isn't set the object can't be scripted.";
}


// TODO: Write better documentation.
/// <summary>Attribute that marks a class as an object for use in levels/worldmaps.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
public sealed class SupertuxObjectAttribute : Attribute
{
	/// <summary>Tells when the object will be shown in the object list.</summary>
	public enum Usage {
		/// <summary>Should only be shown everywhere. This is the default.</summary>
		Any,
		/// <summary>Should not be shown at all.</summary>
		None,
		/// <summary>Should only be shown for worldmaps.</summary>
		WorldmapOnly,
		/// <summary>Should only be shown for "normal" levels.</summary>
		LevelOnly
	}

	public string Name;
	public string IconSprite;
	public string ObjectListAction;
	/// <summary>A <see cref="Usage"/> enum describing where the object can be used.</summary>
	public Usage Target;

	/// <summary>Constructs for <see cref="SupertuxObjectAttribute"/>.</summary>
	/// <param name="Name">Name of object in the level file</param>
	/// <param name="IconSprite">Icon used in the object list in the editor</param>
	public SupertuxObjectAttribute(string Name, string IconSprite) {
		this.Name = Name;
		this.IconSprite = IconSprite;
	}
}

#region Badguys

[SupertuxObject("mrbomb", "images/creatures/mr_bomb/mr_bomb.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrBomb : SimpleDirObject
{
	[ChooseResourceSetting]
	[LispChild("sprite", Optional = true, Default = "")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			spriteFile = value;
			if (!String.IsNullOrEmpty(value))
				try { //TODO: find out why cherry's sprite causes problems. Particles?
					Sprite = SpriteManager.Create(value);
					Sprite.Action = "left";
				} catch {
					Sprite = SpriteManager.Create("images/creatures/mr_bomb/mr_bomb.sprite");
					Sprite.Action = "right";
				}
		}
	}
	private string spriteFile = String.Empty;
	public MrBomb() {
		Sprite = SpriteManager.Create("images/creatures/mr_bomb/mr_bomb.sprite");
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
public sealed class SmartBall : SimpleDirObject
{
	public SmartBall() {
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
public sealed class KamikazeSnowball : SimpleDirObject
{
	public KamikazeSnowball() {
		Sprite = SpriteManager.Create("images/creatures/snowball/kamikaze-snowball.sprite");
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

[SupertuxObject("jumpy", "images/creatures/jumpy/jumpy.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "left-up")]
public sealed class Jumpy : SimpleObject
{
	public Jumpy() {
		Sprite = SpriteManager.Create("images/creatures/jumpy/jumpy.sprite");
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
public sealed class WalinkgLeaf : SimpleDirObject
{
	public WalinkgLeaf() {
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

[SupertuxObject("mrrocket", "images/creatures/mr_rocket/mr_rocket.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrRocket : SimpleDirObject
{
	public MrRocket() {
		Sprite = SpriteManager.Create("images/creatures/mr_rocket/mr_rocket.sprite");
		Sprite.Action = "left";
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

[LispCustomSerializer(typeof(StringList))]
public class StringList : System.Collections.Generic.List<string>, ILispSerializer
{
	/// <summary>
	///		Creates an instance from the serialized object in
	///		<paramref name="list"/>
	/// </summary>
	/// <param name="list">The serialized object</param>
	/// <returns>The unserialized object</returns>
	/// <seealso cref="Write"/>
	public object Read(List list)
	{
		StringList obj = new StringList();

		for (int i = 1; i < list.Length; i++) {
			obj.Add((string)list[i]);
		}
		
		return obj;
	}
	/// <summary>
	///		Seralizes <paramref name="Object"/> using <paramref name="writer"/>
	/// </summary>
	/// <param name="writer">
	///		A <see cref="Writer"/> that <paramref name="Object"/> should be
	///		seralized to.</param>
	/// <param name="name">
	///		Name that should be used for the serialized lisp tree.
	/// </param>
	/// <param name="Object">
	///		The object to write.
	/// </param>
	/// <seealso cref="Read"/>
	public void Write(Writer writer, string name, object Object)
	{
		StringList WrittenList = (StringList) Object;

		if (WrittenList.Count < 1) return;

		string[] vals = new string[WrittenList.Count];
		for (int i = 0; i < WrittenList.Count; i++) {
			vals[i] = WrittenList[i];
		}
		writer.Write(name, vals);
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
	private StringList badguy = new StringList();

	[PropertyProperties(Tooltip = "Type of dispenser to shoot from.")]
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
	[PropertyProperties(Tooltip = "Type of badguys the dispenser will create (lowercase). Use exactly this string to separate badguy names: ', '")]
	[LispChild("badguy")]
	public StringList Badguy {
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
	public float Cycle = 1;

	public Dispenser() {
		Sprite = SpriteManager.Create("images/creatures/dispenser/dispenser.sprite");
		Sprite.Action = "dropper";
		badguy.Add("snowball");

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

	private Path path = new Path();
	[LispChild("path", Optional = true, Default = null)]
	public Path Path {
		get {
			return path;
		}
		set {
			path = value;
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

#endregion Badguys

#region SpawnAndDoors
[SupertuxObject("spawnpoint", "images/engine/editor/spawnpoint.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SpawnPoint : SimpleObject {
	[LispChild("name")]
	public string Name;

	public SpawnPoint() {
		Sprite = SpriteManager.CreateFromImage("images/engine/editor/spawnpoint.png");
		Sprite.Action = "default";
	}
}

[SupertuxObject("firefly", "images/engine/editor/resetpoint.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Firefly : SimpleObject
{
	[ChooseResourceSetting]
	[LispChild("sprite", Optional = true, Default = "")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			spriteFile = value;
			if(!String.IsNullOrEmpty(value))
				Sprite = SpriteManager.Create(value);
		}
	}
	private string spriteFile = String.Empty;
	public Firefly() {
		Sprite = SpriteManager.Create("images/objects/resetpoints/default-resetpoint.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("door", "images/objects/door/door.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "closed")]
public sealed class Door : SimpleObject
{
	[LispChild("sector"), ChooseSectorSetting()]
	public string Sector;
	[LispChild("spawnpoint")]
	public string Spawnpoint;

	public Door() {
		Sprite = SpriteManager.Create("images/objects/door/door.sprite");
		Sprite.Action = "closed";
	}
}
#endregion SpawnAndDoors

#region Light

[SupertuxObject("spotlight", "images/objects/spotlight/spotlight_base.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Spotlight : SimpleColorObject
{
	[LispChild("angle")]
	public float Angle;

	[ChooseColorSetting(UseAlpha = true)]
	[LispChild("color", Optional = true )]
	public Drawing.Color color = new Drawing.Color( 1f, 1f, 1f );

	public Spotlight() {
		Sprite = SpriteManager.Create("images/objects/spotlight/spotlight_base.sprite");
		Sprite.Action = "default";
	}
	public override void Draw(Gdk.Rectangle cliprect) {
		if (!cliprect.IntersectsWith((Gdk.Rectangle) Area))
			return;
		// Draw sprite
		if(Sprite == null)
			return;

		Sprite.Draw(new Vector(X, Y));
		// Draw a color rectangle
		DrawColor(color);
	}
}

[SupertuxObject("magicblock", "images/objects/magicblock/magicblock.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MagicBlock : SimpleColorObject
{
	[ChooseColorSetting]
	[LispChild("color")]
	public Drawing.Color MagicColor {
		get {
			return magiccolor;
		}
		set { //Limit color to 8 useful values (white red green blue yellow violet cyan black)
			magiccolor.Red = (value.Red >= 0.5f?1f:0);
			magiccolor.Green = (value.Green >= 0.5f?1f:0);
			magiccolor.Blue = (value.Blue >= 0.5f?1f:0);
		}
	}
	private Drawing.Color magiccolor = new Drawing.Color( 1f, 0f, 0f );

	public override void Draw(Gdk.Rectangle cliprect) {
		if (!cliprect.IntersectsWith((Gdk.Rectangle) Area))
			return;
		// Draw sprite
		if(Sprite == null)
			return;

		Sprite.Draw(new Vector(X, Y));
		// Draw a color rectangle
		DrawColor(magiccolor);
	}
	public MagicBlock() {
		Sprite = SpriteManager.Create("images/objects/magicblock/magicblock.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("lantern", "images/objects/lantern/lantern.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Lantern : SimpleColorObject
{
	[ChooseColorSetting]
	[LispChild("color")]
	public Drawing.Color LightColor {
		get {
			return lightcolor;
		}
		set { ////Limit color to 8 useful values (white red green blue yellow violet cyan black)
			lightcolor.Red = (value.Red >= 0.5f?1f:0);
			lightcolor.Green = (value.Green >= 0.5f?1f:0);
			lightcolor.Blue = (value.Blue >= 0.5f?1f:0);
		}
	}
	private Drawing.Color lightcolor = new Drawing.Color( 1f, 1f, 1f );

	public override void Draw(Gdk.Rectangle cliprect) {
		if (!cliprect.IntersectsWith((Gdk.Rectangle) Area))
			return;
		// Draw sprite
		if(Sprite == null)
			return;

		Sprite.Draw(new Vector(X, Y));
		// Draw a color rectangle
		DrawColor(lightcolor);
	}
	public Lantern() {
		Sprite = SpriteManager.Create("images/objects/lantern/lantern.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("candle", "images/objects/candle/candle.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "on")]
public sealed class Candle : SimpleObject
{
	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name = String.Empty;
	[PropertyProperties(Tooltip = "If enabled the candle will be burning initially.")]
	[LispChild("burning", Optional = true, Default = true)]
	public bool Burning = true;

	public Candle() {
		Sprite = SpriteManager.Create("images/objects/candle/candle.sprite");
		Sprite.Action = "on";
	}
}

#endregion Light

#region Switches

[SupertuxObject("switch", "images/objects/switch/switch-0.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Switch : SimpleObject
{
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			spriteFile = value;
			if (!String.IsNullOrEmpty(value)) {
				Sprite = SpriteManager.Create(value);
				Sprite.Action = "off";
			}
		}
	}
	private string spriteFile = "images/objects/switch/switch.sprite";

	[LispChild("script")]
	[EditScriptSetting]
	public string Script = String.Empty;

	public Switch() {
		Sprite = SpriteManager.Create("images/objects/switch/switch.sprite");
		Sprite.Action = "off";
	}
}

[SupertuxObject("pushbutton", "images/objects/pushbutton/pushbutton.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "off")]
public sealed class PushButton : SimpleObject
{
	[LispChild("script")]
	[EditScriptSetting]
	public string Script = String.Empty;

	public PushButton() {
		Sprite = SpriteManager.Create("images/objects/pushbutton/pushbutton.sprite");
		Sprite.Action = "off";
	}
}

[SupertuxObject("ispy", "images/objects/ispy/ispy.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "idle-left")]
public sealed class Ispy : SimpleDirObject
{
	[LispChild("script")]
	[EditScriptSetting]
	public string Script = String.Empty;

	[LispChild("facing-down", Optional = true, Default = false)]
	public bool FacingDown {
		get {
			return facingDown;
		}
		set {
			facingDown = value;
			this.DirectionChanged();
		}
	}

	private bool facingDown;

	protected override void DirectionChanged() {
		if (facingDown) {
			Sprite.Action = "idle-down";
		} else {
			Sprite.Action = (Direction == Directions.left) ? "idle-left" : "idle-right";
		}
	}

	public Ispy() {
		Sprite = SpriteManager.Create("images/objects/ispy/ispy.sprite");
		Sprite.Action = "idle-right";
	}
}

#endregion Switches

#region Portables
[SupertuxObject("trampoline", "images/objects/trampoline/trampoline.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Trampoline : SimpleObject
{
	[PropertyProperties(Tooltip = "If enabled Tux can carry the trampoline around.")]
	[LispChild("portable", Optional = true, Default = true)]
	public bool Portable {
		get {
			return portable;
		}
		set {
			portable = value;
			if( value == false ){
				Sprite = SpriteManager.Create("images/objects/trampoline/trampoline_fix.sprite");
			} else {
				Sprite = SpriteManager.Create("images/objects/trampoline/trampoline.sprite");
			}
			Sprite.Action = "normal";
		}
	}
	private bool portable = true;
	public Trampoline() {
		Sprite = SpriteManager.Create("images/objects/trampoline/trampoline.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("rock", "images/objects/rock/rock.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Rock : SimpleObject
{
	public Rock() {
		Sprite = SpriteManager.Create("images/objects/rock/rock.sprite");
		Sprite.Action = "normal";
	}
}

#endregion Portables

#region Platforms

[SupertuxObject("platform",
                "images/objects/flying_platform/flying_platform.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class FlyingPlatform : SimplePathObject
{
	public FlyingPlatform()
	{
		SpriteFile = "images/objects/flying_platform/flying_platform.sprite";
	}
}

[SupertuxObject("hurting_platform",
                "images/objects/sawblade/sawblade.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class HurtingPlatform : SimplePathObject
{
	public HurtingPlatform()
	{
		SpriteFile = "images/objects/sawblade/sawblade.sprite";
	}
}

#endregion Platforms

#region TileLike

[SupertuxObject("unstable_tile",
                "images/objects/unstable_tile/unstable_tile.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class UnstableTile : SimpleObject
{
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			spriteFile = value;
			if (!String.IsNullOrEmpty(value)) {
				Sprite = SpriteManager.Create(value);
				Sprite.Action = "normal";
			}
		}
	}
	private string spriteFile = "images/objects/unstable_tile/unstable_tile.sprite";

	public UnstableTile() {
		Sprite = SpriteManager.Create("images/objects/unstable_tile/unstable_tile.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("weak_block", "images/objects/strawbox/strawbox.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class WeakBlock : SimpleObject
{
	public WeakBlock() {
		Sprite = SpriteManager.Create("images/objects/strawbox/strawbox.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("infoblock", "images/objects/bonus_block/infoblock.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class InfoBlock : SimpleObject
{
	[LispChild("message", Translatable = true)]
	[EditScriptSetting]
	public string Message = String.Empty;

	public InfoBlock() {
		Sprite = SpriteManager.Create("images/objects/bonus_block/infoblock.sprite");
	}
}

#endregion TileLike

[SupertuxObject("powerup", "images/engine/editor/powerup.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Powerup : SimpleObject
{
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			spriteFile = value;
			try {
				if (!String.IsNullOrEmpty(value))
					Sprite = SpriteManager.Create(value);
			} catch(Exception e) {
				ErrorDialog.Exception(e);
			}
		}
	}
	private string spriteFile = String.Empty;
	[LispChild("script", Optional = true, Default = "")]
	[EditScriptSetting]
	public string Script = String.Empty;
	[LispChild("disable-physics", Optional = true, Default = false)]
	public bool DisablePhysics;

	public Powerup() {
		Sprite = SpriteManager.CreateFromImage("images/engine/editor/powerup.png");
		Sprite.Action = "default";
	}
}

[SupertuxObject("scriptedobject", "images/engine/editor/scriptedobject.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class ScriptedObject : SimpleObject
{
	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name")]
	public string Name = String.Empty;
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			spriteFile = value;
			if (!String.IsNullOrEmpty(value))
				Sprite = SpriteManager.Create(value);
		}
	}
	private string spriteFile = String.Empty;
	[LispChild("z-pos", Optional = true, Default = -10)]
	public int ZPos = -10;
	[LispChild("visible")]
	public bool Visible = true;
	[LispChild("physic-enabled")]
	public bool PhysicEnabled = false;
	[LispChild("solid")]
	public bool Solid = false;

	public ScriptedObject() {
		Sprite = SpriteManager.CreateFromImage("images/engine/editor/scriptedobject.png");
		Sprite.Action = "default";
	}
}

#region AreaObjects

[SupertuxObject("wind", "images/engine/editor/wind.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Wind : SimpleObjectArea
{
	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name = String.Empty;

	[PropertyProperties(Tooltip = "If enabled the wind will be blowing initially.")]
	[LispChild("blowing", Optional = true, Default = true)]
	public bool Blowing = true;

	[LispChild("speed-x")]
	public float SpeedX = 0;

	[LispChild("speed-y")]
	public float SpeedY = 0;

	[LispChild("acceleration")]
	public float Acceleration = 0;

	public Wind() {
		Color = new Drawing.Color(.8f, 0, 0.8f, 0.8f);
	}
}

[SupertuxObject("ambient_sound", "images/engine/editor/ambientsound.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class AmbientSound : SimpleObjectArea
{
	[LispChild("sample")]
	[ChooseResourceSetting]
	public string Sample = String.Empty;
	[LispChild("distance_factor")]
	public float DistanceFactor;
	[LispChild("distance_bias")]
	public float DistanceBias;
	[LispChild("volume")]
	public float Volume;
	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name = String.Empty;

	public AmbientSound() {
		Sprite = SpriteManager.CreateFromImage("images/engine/editor/ambientsound.png");
		Sprite.Action = "default";
		Color = new Drawing.Color(0, 0, 0.8f, 0.8f);
	}
}

[SupertuxObject("sequencetrigger", "images/engine/editor/sequencetrigger.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SequenceTrigger : SimpleObjectArea
{
	[LispChild("sequence")]
	public string Sequence = String.Empty;

	public SequenceTrigger() {
		Color = new Drawing.Color(.8f, 0, 0, 0.8f);
	}
}

[SupertuxObject("scripttrigger", "images/engine/editor/scripttrigger.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class ScriptTrigger : SimpleObjectArea
{
	[LispChild("script")]
	[EditScriptSetting]
	public string Script = String.Empty;
	[LispChild("button")]
	public bool IsButton;

	public ScriptTrigger() {
		Color = new Drawing.Color(.8f, 0, .8f, 0.8f);
	}
}

[SupertuxObject("invisible_wall", "images/engine/editor/invisible_wall.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class InvisibleWall : SimpleObjectArea
{
	public InvisibleWall() {
		Color = new Drawing.Color(0, .8f, 0, 0.8f);
	}
}

[SupertuxObject("secretarea",  "images/engine/editor/secretarea.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SecretArea : SimpleObjectArea
{
	[PropertyProperties(Tooltip = "Fade the tilemap with this name when the player finds the secret area. Optional.")]
	[LispChild("fade-tilemap", Optional = true, Default = "")]
	public string FadeTilemap = String.Empty;

	public SecretArea() {
		Color = new Drawing.Color(0, .8f, 0, 0.8f);
	}
}

// TODO: This image isn't good for this, make one that fit with the
//       other areas.
[SupertuxObject("climbable",  "images/tiles/forest/ladder.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Climbable : SimpleObjectArea
{
	public Climbable() {
		Color = new Drawing.Color(.8f, .8f, 0, 0.8f);
	}
}

#endregion AreaObjects

#region Particles

[SupertuxObject("particles-rain", "images/engine/editor/rain.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class RainParticles : IGameObject
{
	[LispChild("z-pos", Optional = true, Default = 0)]
	public int ZPos = 0;
}

[SupertuxObject("particles-ghosts", "images/engine/editor/ghostparticles.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class GhostParticles : IGameObject
{
	[LispChild("z-pos", Optional = true, Default = -200)]
	public int ZPos = -200;
}

[SupertuxObject("particles-snow", "images/engine/editor/snow.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SnowParticles : IGameObject
{
	[LispChild("z-pos", Optional = true, Default = -200)]
	public int ZPos = -200;
}

[SupertuxObject("particles-clouds", "images/engine/editor/clouds.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class CloudParticles : IGameObject
{
	[LispChild("z-pos", Optional = true, Default = -200)]
	public int ZPos = -200;
}

#endregion Particles

[SupertuxObject("leveltime", "images/engine/editor/clock.png",
                Target = SupertuxObjectAttribute.Usage.None)]
public sealed class LevelTime : IGameObject
{
	[PropertyProperties(Tooltip = "Time in seconds")]
	[LispChild("time")]
	public float Time;
}

[SupertuxObject("thunderstorm", "images/engine/editor/thunderstorm.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Thunderstorm : IGameObject
{
	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name = String.Empty;

	[PropertyProperties(Tooltip = "If enabled the thunderstorm will be running initially.")]
	[LispChild("running", Optional = true, Default = true)]
	public bool Running = true;

	[PropertyProperties(Tooltip = "Time between last lightning and next thunder")]
	[LispChild("interval", Optional = true, Default = 10f)]
	public float Interval = 10;
}
