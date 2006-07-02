using Sprites;
using System;
using LispReader;
using DataStructures;
using SceneGraph;

// TODO: Write better documentation.
/// <summary>Attribute that marks a class as an object for use in levels/worldmaps.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
public sealed class SupertuxObjectAttribute : Attribute
{
	/// <summary>Tells where the object can be used.</summary>
	public enum Usage {
		/// <summary>Can be used anywhere. This is the default.</summary>
		Any,
		/// <summary>Can only be used on worldmaps.</summary>
		WorldmapOnly,
		/// <summary>Can only be used in "normal" levels.</summary>
		LevelOnly
	}

	public string Name;
	public string IconSprite;
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

[SupertuxObject("mrbomb", "images/creatures/mr_bomb/mr_bomb.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrBomb : SimpleObject
{
	public MrBomb() {
		Sprite = SpriteManager.Create("images/creatures/mr_bomb/mr_bomb.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("snowball", "images/creatures/snowball/snowball.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Snowball : SimpleObject
{
	public Snowball() {
		Sprite = SpriteManager.Create("images/creatures/snowball/snowball.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("stalactite", "images/creatures/stalactite/stalactite.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Stalactite : SimpleObject
{
	public Stalactite() {
		Sprite = SpriteManager.Create("images/creatures/stalactite/stalactite.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("mriceblock", "images/creatures/mr_iceblock/mr_iceblock.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrIceBlock : SimpleObject
{
	public MrIceBlock() {
		Sprite = SpriteManager.Create("images/creatures/mr_iceblock/mr_iceblock.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("bouncingsnowball", "images/creatures/bouncing_snowball/bouncing_snowball.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class BouncingSnowball : SimpleObject
{
	public BouncingSnowball() {
		Sprite = SpriteManager.Create("images/creatures/bouncing_snowball/bouncing_snowball.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("flyingsnowball", "images/creatures/flying_snowball/flying_snowball.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class FlyingSnowball : SimpleObject
{
	public FlyingSnowball() {
		Sprite = SpriteManager.Create("images/creatures/flying_snowball/flying_snowball.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("jumpy", "images/creatures/jumpy/jumpy.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Jumpy : SimpleObject
{
	public Jumpy() {
		Sprite = SpriteManager.Create("images/creatures/jumpy/jumpy.sprite");
		Sprite.Action = "left-up";
	}
}

[SupertuxObject("spiky", "images/creatures/spiky/spiky.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Spiky : SimpleObject
{
	public Spiky() {
		Sprite = SpriteManager.Create("images/creatures/spiky/spiky.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("sspiky", "images/creatures/spiky/sleepingspiky.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SleepSpiky : SimpleObject
{
	public SleepSpiky() {
		Sprite = SpriteManager.Create("images/creatures/spiky/sleepingspiky.sprite");
		Sprite.Action = "sleeping-left";
	}
}

[SupertuxObject("spawnpoint", "images/engine/editor/spawnpoint.png", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SpawnPoint : SimpleObject
{
	[LispChild("name")]
	public string Name;
	
	public SpawnPoint() {
		Sprite = SpriteManager.CreateFromImage("images/engine/editor/spawnpoint.png");
		Sprite.Action = "default";
	}
}

[SupertuxObject("flame", "images/creatures/flame/flame.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
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

[SupertuxObject("fish", "images/creatures/fish/fish.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Fish : SimpleObject
{
	public Fish() {
		Sprite = SpriteManager.Create("images/creatures/fish/fish.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("mrtree", "images/creatures/mr_tree/mr_tree.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class MrTree : SimpleObject
{
	public MrTree() {
		Sprite = SpriteManager.Create("images/creatures/mr_tree/mr_tree.sprite");
		Sprite.Action = "large-left";
	}
}

[SupertuxObject("poisonivy", "images/creatures/poison_ivy/poison_ivy.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class PoisonIvy : SimpleObject
{
	public PoisonIvy() {
		Sprite = SpriteManager.Create("images/creatures/poison_ivy/poison_ivy.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("zeekling", "images/creatures/zeekling/zeekling.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Zeekling : SimpleObject
{
	public Zeekling() {
		Sprite = SpriteManager.Create("images/creatures/zeekling/zeekling.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("snail", "images/creatures/snail/snail.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Snail : SimpleObject
{
	public Snail() {
		Sprite = SpriteManager.Create("images/creatures/snail/snail.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("totem", "images/creatures/totem/totem.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Totem : SimpleObject
{
	public Totem() {
		Sprite = SpriteManager.Create("images/creatures/totem/totem.sprite");
		Sprite.Action = "walking-left";
	}
}

[SupertuxObject("kugelblitz", "images/creatures/kugelblitz/kugelblitz.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Kugelblitz : SimpleObject
{
	public Kugelblitz() {
		Sprite = SpriteManager.Create("images/creatures/kugelblitz/kugelblitz.sprite");
		Sprite.Action = "falling";
	}
}

[SupertuxObject("dispenser", "images/creatures/dispenser/dispenser.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Dispenser : SimpleObject
{
	[LispChild("badguy")]
	public string Badguy = "";
	[LispChild("cycle")]
	public float Cycle = 1;
	[LispChild("launchdirection")]
	public string LaunchDirection = "";	

	public Dispenser() {
		Sprite = SpriteManager.Create("images/creatures/dispenser/dispenser.sprite");
		Sprite.Action = "working-left";
	}
}

[SupertuxObject("yeti", "images/creatures/yeti/yeti.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Yeti : SimpleObject
{
	[LispChild("dead-script")]
	[EditScriptSetting]
	public String DeadScript = "";

	public Yeti() {
		Sprite = SpriteManager.Create("images/creatures/yeti/yeti.sprite");
		Sprite.Action = "left";
	}
}

[SupertuxObject("stalactite_yeti", "images/engine/editor/stalactite_yeti.png", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class StalactiteYeti : SimpleObject
{
	public StalactiteYeti() {
		Sprite = SpriteManager.Create("images/creatures/stalactite/stalactite.sprite");
		Sprite.Action = "normal";
	}
}

/// <summary>
/// Base class for Doors and 
/// </summary>
public abstract class DoorBase : SimpleObject
{
	[LispChild("sector")]
	public string Sector;
	[LispChild("spawnpoint")]
	public string Spawnpoint;
}

[SupertuxObject("door", "images/objects/door/door.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Door : DoorBase
{
	public Door() {
		Sprite = SpriteManager.Create("images/objects/door/door.sprite");
		Sprite.Action = "closed";
	}
}

[SupertuxObject("hatch", "images/objects/hatch/hatch.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Hatch : DoorBase
{
 	public Hatch() {
		Sprite = SpriteManager.Create("images/objects/hatch/hatch.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("switch", "images/objects/switch/switch.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
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
			if(value != "") {
				Sprite = SpriteManager.Create(value);
				Sprite.Action = "off";
			}
		}
	}
	private string spriteFile = "";

	[LispChild("script")]
	[EditScriptSetting]
	public string Script = "";

	public Switch() {
		Sprite = SpriteManager.Create("images/objects/switch/switch.sprite");
		Sprite.Action = "off";
	}
}

[SupertuxObject("trampoline", "images/objects/trampoline/trampoline.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Trampoline : SimpleObject
{
	public Trampoline() {
		Sprite = SpriteManager.Create("images/objects/trampoline/trampoline.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("firefly", "images/objects/firefly/firefly.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Firefly : SimpleObject
{
	public Firefly() {
		Sprite = SpriteManager.Create("images/objects/firefly/firefly.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("angrystone", "images/creatures/angrystone/angrystone.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class AngryStone : SimpleObject
{
	public AngryStone() {
		Sprite = SpriteManager.Create("images/creatures/angrystone/angrystone.sprite");
	}
}

/// <summary>Base class for platforms.</summary>
public abstract class PlatformBase : IGameObject, IObject, IPathObject, Node
{
	[LispChild("name", Optional = true, Default = "")]
	public string Name = "";
	[LispChild("running", Optional = true, Default = true)]
	public bool Running = true;
	[ChooseResourceSetting]	
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			spriteFile = value;
			if(value != "")
				Sprite = SpriteManager.Create(value);
		}
	}
	private string spriteFile = "";
	
	private Sprite Sprite;
	
	private Path path;
	[LispChild("path")]
	public Path Path {
		get {
			return path;
		}
		set {
			path = value;
		}
	}
	
	public virtual bool Resizable {
		get {
			return false;
		}
	}

	public PlatformBase()
	{
		Sprite = SpriteManager.Create("images/objects/flying_platform/flying_platform.sprite");
		path = new Path();
		path.Nodes.Add(new Path.Node());
	}
	
	public void Draw()
	{
		Sprite.Draw(Path.Nodes[0].Pos);
	}
	
	public virtual Node GetSceneGraphNode() {
		return this;
	}
	
	public virtual void ChangeArea(RectangleF NewArea) {
		Vector translation = new Vector(NewArea.Left - Path.Nodes[0].X, NewArea.Top - Path.Nodes[0].Y);
		Path.Move(translation);
	}
	
	public virtual RectangleF Area {
		get {
			float x = Path.Nodes[0].X;
			float y = Path.Nodes[0].Y;
			
			return new RectangleF(x - Sprite.Offset.X, y - Sprite.Offset.Y, Sprite.Width, Sprite.Height);
		}
	}	
}

[SupertuxObject("platform", "images/objects/flying_platform/flying_platform.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class FlyingPlatform : PlatformBase
{
}

[SupertuxObject("hurting_platform", "images/objects/sawblade/sawblade.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class HurtingPlatform : PlatformBase
{
}

[SupertuxObject("willowisp", "images/creatures/willowisp/willowisp.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class WilloWisp : SimpleObject
{
	[LispChild("sector")]
	public string Sector = "";
	[LispChild("spawnpoint")]
	public string SpawnPoint = "";
	
	public WilloWisp() {
		Sprite = SpriteManager.Create("images/creatures/willowisp/willowisp.sprite");
		Sprite.Action = "idle";
	}
}

[SupertuxObject("darttrap", "images/creatures/darttrap/darttrap.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class DartTrap : SimpleObject
{
	[LispChild("initial-delay")]
	public float initial_delay = 0;
	[LispChild("fire-delay")]
	public float fire_delay = 2;
	[LispChild("ammo")]
	public int ammo = -1;
	
	public DartTrap() {
		Sprite = SpriteManager.Create("images/creatures/darttrap/darttrap.sprite");
		Sprite.Action = "idle-left";
	}
}

[SupertuxObject("skullyhop", "images/creatures/skullyhop/skullyhop.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SkullyHop : SimpleObject
{
	public SkullyHop() {
		Sprite = SpriteManager.Create("images/creatures/skullyhop/skullyhop.sprite");
		Sprite.Action = "standing-left";
	}
}

[SupertuxObject("igel", "images/creatures/igel/igel.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Igel : SimpleObject
{
	public Igel() {
		Sprite = SpriteManager.Create("images/creatures/igel/igel.sprite");
		Sprite.Action = "walking-left";
	}
}

[SupertuxObject("rock", "images/objects/rock/rock.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Rock : SimpleObject
{
	public Rock() {
		Sprite = SpriteManager.Create("images/objects/rock/rock.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("candle", "images/objects/candle/candle.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Candle : SimpleObject
{
	[LispChild("name", Optional = true, Default = "")]
	public string Name = "";
	[LispChild("burning", Optional = true, Default = true)]
	public bool Burning = true;

	public Candle() {
		Sprite = SpriteManager.Create("images/objects/candle/candle.sprite");
		Sprite.Action = "on";
	}
}

[SupertuxObject("unstable_tile", "images/objects/unstable_tile/unstable_tile.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
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
			if(value != "") {
				Sprite = SpriteManager.Create(value);
				Sprite.Action = "normal";
			}
		}
	}
	private string spriteFile = "";

	public UnstableTile() {
		Sprite = SpriteManager.Create("images/objects/unstable_tile/unstable_tile.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("weak_block", "images/objects/strawbox/strawbox.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class WeakBlock : SimpleObject
{
	public WeakBlock() {
		Sprite = SpriteManager.Create("images/objects/strawbox/strawbox.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("infoblock", "images/objects/bonus_block/infoblock.sprite", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class InfoBlock : SimpleObject
{
	[LispChild("message", Translatable = true)]
	[EditScriptSetting]
	public string Message = "";

	public InfoBlock() {
		Sprite = SpriteManager.Create("images/objects/bonus_block/infoblock.sprite");
	}
}

[SupertuxObject("powerup", "images/engine/editor/powerup.png", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
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
				if(value != "")
					Sprite = SpriteManager.Create(value);
			} catch(Exception e) {
				ErrorDialog.Exception(e);
			}
		}
	}
	private string spriteFile = "";
	[LispChild("script", Optional = true, Default = "")]
	[EditScriptSetting]	
	public string Script = "";
	[LispChild("disable-physics", Optional = true, Default = false)]
	public bool DisablePhysics;
	
	public Powerup() {
	}
}

[SupertuxObject("scriptedobject", "images/engine/editor/scriptedobject.png")]
public sealed class ScriptedObject : SimpleObject
{
	[LispChild("name")]
	public string Name = "";
	[ChooseResourceSetting]	
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			spriteFile = value;
			if(value != "")
				Sprite = SpriteManager.Create(value);
		}
	}
	private string spriteFile = "";
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

[SupertuxObject("wind", "images/engine/editor/wind.png", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Wind : SimpleObjectArea
{
	[LispChild("name", Optional = true, Default = "")]
	public string Name = "";

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

[SupertuxObject("ambient_sound", "images/engine/editor/ambientsound.png")]
public sealed class AmbientSound : SimpleObjectArea
{
	[LispChild("sample")]
	[ChooseResourceSetting]	
	public string Sample = "";
	[LispChild("distance_factor")]
	public float DistanceFactor;
	[LispChild("distance_bias")]
	public float DistanceBias;
	[LispChild("volume")]
	public float Volume;
	
	public AmbientSound() {
		Sprite = SpriteManager.CreateFromImage("images/engine/editor/ambientsound.png");
		Sprite.Action = "default";
		Color = new Drawing.Color(0, 0, 0.8f, 0.8f);
	}
}

[SupertuxObject("sequencetrigger", "images/engine/editor/sequencetrigger.png")]
public sealed class SequenceTrigger : SimpleObjectArea
{
	[LispChild("sequence")]
	public string Sequence = "";

	public SequenceTrigger() {
		Color = new Drawing.Color(.8f, 0, 0, 0.8f);
	}
}

[SupertuxObject("scripttrigger", "images/engine/editor/scripttrigger.png")]
public sealed class ScriptTrigger : SimpleObjectArea
{
	[LispChild("script")]
	[EditScriptSetting]
	public string Script = "";
	[LispChild("button")]
	public bool IsButton;

	public ScriptTrigger() {
		Color = new Drawing.Color(.8f, 0, .8f, 0.8f);
	}
}

[SupertuxObject("secretarea",  "images/engine/editor/secretarea.png")]
public sealed class SecretArea : SimpleObjectArea
{
	public SecretArea() {
		Color = new Drawing.Color(0, .8f, 0, 0.8f);
	}
}

[SupertuxObject("particles-rain", "images/engine/editor/rain.png")]
public sealed class RainParticles : IGameObject
{
	[LispChild("z-pos", Optional = true, Default = 0)]
	public int ZPos = 0;
}

[SupertuxObject("particles-ghosts", "images/engine/editor/ghostparticles.png")]
public sealed class GhostParticles : IGameObject
{
	[LispChild("z-pos", Optional = true, Default = -200)]
	public int ZPos = -200;
}

[SupertuxObject("particles-snow", "images/engine/editor/snow.png")]
public sealed class SnowParticles : IGameObject
{
	[LispChild("z-pos", Optional = true, Default = -200)]
	public int ZPos = -200;
}

[SupertuxObject("particles-clouds", "images/engine/editor/clouds.png")]
public sealed class CloudParticles : IGameObject
{
	[LispChild("z-pos", Optional = true, Default = -200)]
	public int ZPos = -200;
}

[SupertuxObject("leveltime", "images/engine/editor/clock.png", Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class LevelTime : IGameObject
{
	[LispChild("time")]
	public float Time;
}
