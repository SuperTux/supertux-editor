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

/// <summary>
/// Used to make it simpler to change common tooltip strings.
/// </summary>
internal static class ToolTipStrings {
	/// <summary>
	/// For the "Name" attribute used for scripting.
	/// </summary>
	internal const string ScriptingName = "Used to refer to the object from a script. If it isn't set the object can't be scripted.";
}

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
public sealed class Firefly : SimpleSpriteObject
{
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

	[LispChild("script", Optional = true, Default = "")]
	public string Script;

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
	[LispChild("color", Optional = true, Default = "Color(1, 1, 1, 1)")]
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
	[LispChild("color", Optional = true, Default = "Color(0, 0, 0, 1)")]
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
	private Drawing.Color magiccolor = new Drawing.Color( 0f, 0f, 0f );

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
	[LispChild("color", Optional = true, Default = "Color(1, 1, 1, 1)")]
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
public sealed class Candle : SimpleColorObject
{
	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name = String.Empty;
	[PropertyProperties(Tooltip = "If enabled the candle will be burning initially.")]
	[LispChild("burning", Optional = true, Default = true)]
	public bool Burning = true;
	[PropertyProperties(Tooltip = "If enabled the candle will flicker while on.")]
	[LispChild("flicker", Optional = true, Default = true)]
	public bool Flicker = true;
	[PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
  [ChooseResourceSetting]
  [LispChild("sprite", Optional = true, Default = "images/objects/candle/candle.sprite")]
  public string SpriteFile {
    get {
      return spriteFile;
    }
    set {
      if (!String.IsNullOrEmpty(value)) {
        Sprite newSprite = SpriteManager.Create(value);
        newSprite.Action = "on";
        Sprite = newSprite;     //save new sprite after (no exception only)
      }
      spriteFile = value;
    }
  }
  private string spriteFile = "images/objects/candle/candle.sprite";
	[ChooseColorSetting]
	[LispChild("color", Optional = true, Default = "Color(1, 1, 1, 1)")]
	public Drawing.Color LightColor {
		get {
			return lightcolor;
		}
		set {
			lightcolor.Red = value.Red;
			lightcolor.Green = value.Green;
			lightcolor.Blue = value.Blue;
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

	public Candle() {
		Sprite = SpriteManager.Create("images/objects/candle/candle.sprite");
		Sprite.Action = "on";
	}
}

[SupertuxObject("torch", "images/objects/torch/torch1.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "on")]
public sealed class Torch : SimpleObject
{
	private string spriteFile = "images/objects/torch/torch1.sprite";

	[LispChild("sprite")]
        public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value)) {
				Sprite = SpriteManager.Create(value);
			}
			spriteFile = value;
		}
	}

	public Torch() {
		Sprite = SpriteManager.Create(spriteFile);
	}
}

#endregion Light

#region Switches

[SupertuxObject("switch", "images/objects/switch/switch-0.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Switch : SimpleObject
{
	[PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value)) {
				Sprite newSprite = SpriteManager.Create(value);
				newSprite.Action = "off";
				Sprite = newSprite;	//save new sprite after (no exception only)
			}
			spriteFile = value;
		}
	}
	private string spriteFile = "images/objects/switch/switch.sprite";

	[LispChild("script")]
	[EditScriptSetting]
	public string Script = String.Empty;

	[LispChild("off-script", Optional = true, Default = "")]
	[EditScriptSetting]
	public string OffScript = String.Empty;

	public Switch() {
		Sprite = SpriteManager.Create("images/objects/switch/switch.sprite");
		Sprite.Action = "off";
	}
}

[SupertuxObject("pushbutton", "images/objects/pushbutton/pushbutton.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly,
                ObjectListAction = "off")]
public sealed class PushButton : SimpleSpriteObject
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

	[PropertyProperties(Tooltip = "Enables 3rd state of ispy (and ignores left/right setting)", RedrawOnChange = true)]
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
public sealed class Trampoline : SimpleSpriteObject
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

[SupertuxObject("rustytrampoline", "images/objects/rustytrampoline/rustytram-poline.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class RustyTrampoline : SimpleSpriteObject
{
	[LispChild("portable", Optional = true, Default = true)]
	public bool Portable = true;

	[LispChild("counter", Optional = true, Default = 3)]
	public int Counter = 3;

	public RustyTrampoline() {
		Sprite = SpriteManager.Create("images/objects/rusty-trampoline/rusty-trampoline.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("rock", "images/objects/rock/rock.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Rock : SimpleSpriteObject
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
                "images/objects/unstable_tile/snow.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class UnstableTile : SimpleObject
{
  [PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
  [ChooseResourceSetting]
  [LispChild("sprite")]
  public string SpriteFile {
    get {
      return spriteFile;
    }
    set {
      if (!String.IsNullOrEmpty(value)) {
        Sprite newSprite = SpriteManager.Create(value);
        newSprite.Action = "normal";
        Sprite = newSprite;     //save new sprite after 
      }
      spriteFile = value;
    }
  }
  private string spriteFile = "images/objects/unstable_tile/snow.sprite";

	public UnstableTile() {
		Sprite = SpriteManager.Create("images/objects/unstable_tile/snow.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("weak_block", "images/objects/weak_block/meltbox.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class WeakBlock : SimpleSpriteObject
{
  [PropertyProperties(Tooltip = "Enable for straw which burns adjacent blocks.")]
	[LispChild("linked", Optional = false, Default = true)]
	public bool Linked {
		get {
			return linked;
		}
		set {
			linked = value;
			if( value == false ){
				Sprite = SpriteManager.Create("images/objects/weak_block/meltbox.sprite");
			} else {
				Sprite = SpriteManager.Create("images/objects/weak_block/strawbox.sprite");
			}
			Sprite.Action = "normal";
		}
	}
	private bool linked = false;
	public WeakBlock() {
		Sprite = SpriteManager.Create("images/objects/weak_block/meltbox.sprite");
		Sprite.Action = "normal";
	}
}

[SupertuxObject("bonusblock","images/objects/bonus_block/bonusblock.sprite",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class BonusBlock : SimpleSpriteObject
{
	public int hit_counter = 1;
	[PropertyProperties(Tooltip = "Number of times BonusBlock can be hit (use 0 for infinite)")]
	[LispChild("count", Optional = true, Default = 1)]
	public int HitCounter {
		get {
			return hit_counter;
		}
		set {
			hit_counter = value;
		}
	}
	
	//TODO enumerate contents for easy selection
	/*public enum ContentTypes {
		coin,
		firegrow,
    icegrow,
    star,
    tuxdoll,
    custom,
    script,
    light,
    trampoline,
    porttrampoline,
    rock
	}*/

	private string contents = "coin";
	
	[PropertyProperties(Tooltip = "Contents of BonusBlock", RedrawOnChange = true)]
	[LispChild("contents", Optional = true, Default = "coin")]
	public string Content {
		get {
  	    return contents;
		}
		set {
			contents = value;
			//TODO change sprites automatically for content or leave up to user?
			/*if (value == firegrow)
				
			else if (value == icegrow)
				
			else*/
		}
	}
	
	//TODO custom powerup support
	
	[PropertyProperties(Tooltip = "Script to run when BonusBlock is hit.  Only used if Content is set to \"script\"")]
	[LispChild("script", Optional = true)]
	[EditScriptSetting]
	public string Script = String.Empty;
	
  public BonusBlock() {
    Sprite = SpriteManager.Create("images/objects/bonus_block/bonusblock.sprite");
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

[SupertuxObject("coin",
                "images/objects/coin/path_coin.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Coin : SimplePathObject
{
	[LispChild("collect-script", Optional = true, Default = "")]
	[EditScriptSetting]
	public string Script = String.Empty;

	public Coin() {
		Sprite = SpriteManager.Create("images/objects/coin/coin.sprite");
		Sprite.Action = "editor-path";
	}
}

[SupertuxObject("heavycoin",
                "images/objects/coin/heavy_coin.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class HeavyCoin : SimpleSpriteObject
{
	public HeavyCoin() {
		Sprite = SpriteManager.Create("images/objects/coin/coin.sprite");
		Sprite.Action = "editor-heavy";
	}
}
#endregion TileLike

[SupertuxObject("powerup", "images/engine/editor/powerup.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Powerup : SimpleObject
{
	[PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value))
				Sprite = SpriteManager.Create(value);
			spriteFile = value;
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
	[PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value))
				Sprite = SpriteManager.Create(value);
			spriteFile = value;
		}
	}
	private string spriteFile = String.Empty;
	[LispChild("z-pos", Optional = true, Default = -10, AlternativeName = "layer")]
	public int Layer = -10;
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
		Color = new Drawing.Color(0, 0.8f, 0.8f, 0.8f);
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
		Color = new Drawing.Color(0, 0, 0, 0.8f);
	}
}

[SupertuxObject("secretarea",  "images/engine/editor/secretarea.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SecretArea : SimpleObjectArea
{
	[PropertyProperties(Tooltip = "Fade the tilemap with this name when the player finds the secret area. Optional.")]
	[LispChild("fade-tilemap", Optional = true, Default = "")]
	public string FadeTilemap = String.Empty;
  [PropertyProperties(Tooltip = "Run this script when the player finds the secret area. Optional.")]
	[LispChild("script", Optional = true, Default = "")]
	[EditScriptSetting]
	public string Script = String.Empty;
  [PropertyProperties(Tooltip = "Alternative message to display when the player finds the secret area. Optional.")]
	[LispChild("message", Optional = true, Default = "")]
	public string Message = String.Empty;

	public SecretArea() {
		Color = new Drawing.Color(0, .8f, 0, 0.8f);
	}
}

[SupertuxObject("climbable",  "images/engine/editor/climbable.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class Climbable : SimpleObjectArea
{
	[LispChild("message", Optional = true, Default = "", Translatable = true)]
	public string Message = String.Empty;

	public Climbable() {
		Color = new Drawing.Color(.8f, .8f, 0, 0.8f);
	}
}

#endregion AreaObjects

#region Particles

[SupertuxObject("particles-rain", "images/engine/editor/rain.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class RainParticles : IGameObject, ILayer
{
	public string Name {get {return "";}}	//= it can't have a name
	private int layer = 0;
	[LispChild("z-pos", Optional = true, Default = 0)]
	public int Layer { get { return layer; } set { layer = value; }}	//property around field layer needed to implement ILayer
}

[SupertuxObject("particles-ghosts", "images/engine/editor/ghostparticles.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class GhostParticles : IGameObject, ILayer
{
	public string Name {get {return "";}}	//= it can't have a name
	private int layer = -200;
	[LispChild("z-pos", Optional = true, Default = -200)]
	public int Layer { get { return layer; } set { layer = value; }}	//property around field layer needed to implement ILayer
}

[SupertuxObject("particles-snow", "images/engine/editor/snow.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class SnowParticles : IGameObject, ILayer
{
	public string Name {get {return "";}}	//= it can't have a name
	private int layer = -200;
	[LispChild("z-pos", Optional = true, Default = -200)]
	public int Layer { get { return layer; } set { layer = value; }}	//property around field layer needed to implement ILayer
}

[SupertuxObject("particles-clouds", "images/engine/editor/clouds.png",
                Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class CloudParticles : IGameObject, ILayer
{
	public string Name {get {return "";}}	//= it can't have a name
	private int layer = -200;
	[LispChild("z-pos", Optional = true, Default = -200)]
	public int Layer { get { return layer; } set { layer = value; }}	//property around field layer needed to implement ILayer
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
public sealed class Thunderstorm : IGameObject, ILayer
{
	private string name = String.Empty;
	[PropertyProperties(Tooltip = ToolTipStrings.ScriptingName)]
	[LispChild("name", Optional = true, Default = "")]
	public string Name {
		get {
			return name;
		}
		set {
			name = value;
		}
	}

	private int layer = -101;
	[PropertyProperties(Tooltip = "Layer in which lightning appears.")]
	[LispChild("z-pos", Optional = true, Default = -101, AlternativeName = "layer")]
	public int Layer {
		get {
			return layer;
		}
		set {
			layer = value;
		}
	}

	[PropertyProperties(Tooltip = "If enabled the thunderstorm will be running initially.")]
	[LispChild("running", Optional = true, Default = true)]
	public bool Running = true;

	[PropertyProperties(Tooltip = "Time between last lightning and next thunder")]
	[LispChild("interval", Optional = true, Default = 10f)]
	public float Interval = 10;
}

[SupertuxObject("pneumatic-platform", "images/engine/editor/pneumaticplatform.png", 
			    Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class PneumaticPlatform : SimpleObject
{
	[PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value)) {
				Sprite newSprite = SpriteManager.Create(value);
				newSprite.Action = "default";
				Sprite = newSprite;	//save new sprite after (no exception only)
			}
			spriteFile = value;
		}
	}
	private string spriteFile = "images/objects/platforms/small.sprite";
	
	public override RectangleF Area {
		get {
			if(Sprite != null)
				return new RectangleF(X - Sprite.Offset.X, Y - Sprite.Offset.Y,
				                          Sprite.Width*2, Sprite.Height+2);
			else
				return new RectangleF(X, Y, 32, 32);
		}
	}
	
	public override void Draw(Gdk.Rectangle cliprect) {
		if(Sprite == null)
			return;
		if (cliprect.IntersectsWith((Gdk.Rectangle) Area))
		{
			Sprite.Draw(new Vector(X, Y));
			Sprite.Draw(new Vector(X+Sprite.Width, Y));
		}
	}
	
	public PneumaticPlatform() {
		Sprite = SpriteManager.Create("images/objects/platforms/small.sprite");
		Sprite.Action = "default";
	}
}

[SupertuxObject("bicycle-platform", "images/engine/editor/bicycleplatform.png", 
			    Target = SupertuxObjectAttribute.Usage.LevelOnly)]
public sealed class BicyclePlatform : SimpleObject
{
	[PropertyProperties(Tooltip = "File describing \"skin\" for object.", RedrawOnChange = true)]
	[ChooseResourceSetting]
	[LispChild("sprite")]
	public string SpriteFile {
		get {
			return spriteFile;
		}
		set {
			if (!String.IsNullOrEmpty(value)) {
				Sprite newSprite = SpriteManager.Create(value);
				newSprite.Action = "default";
				Sprite = newSprite;	//save new sprite after (no exception only)
			}
			spriteFile = value;
		}
	}
	private string spriteFile = "images/objects/platforms/small.sprite";
	
	public override RectangleF Area {
		get {
			if(Sprite != null)
				return new RectangleF(X - Sprite.Offset.X - 128 - Sprite.Width/2, Y - Sprite.Offset.Y-Sprite.Height/2,
				                          Sprite.Width+256, Sprite.Height);
			else
				return new RectangleF(X, Y, 32, 32);
		}
	}
	
	//FIXME: this shouldn't be necessary
	public override void ChangeArea(RectangleF NewArea) {
		X = NewArea.Left + Sprite.Offset.X + 128 + Sprite.Width/2;
		Y = NewArea.Top + Sprite.Offset.Y + Sprite.Height/2;
		if(Sprite != null) {
			X += Sprite.Offset.X;
			Y += Sprite.Offset.Y;
		}
	}
	
	public override void Draw(Gdk.Rectangle cliprect) {
		if(Sprite == null)
			return;
		if (cliprect.IntersectsWith((Gdk.Rectangle) Area))
		{
			Sprite.Draw(new Vector(X-128-Sprite.Width/2, Y-Sprite.Height/2));
			Sprite.Draw(new Vector(X+128-Sprite.Width/2, Y-Sprite.Height/2));
		}
	}
	
	public BicyclePlatform() {
		Sprite = SpriteManager.Create("images/objects/platforms/small.sprite");
		Sprite.Action = "default";
	}
}

/* EOF */
