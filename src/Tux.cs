using SceneGraph;
using DataStructures;
using Sprites;

public class Tux : GameObject {
	private Sprite Sprite;
	//private Controller Controller;
	//private Vector Speed;

	public Tux(Sector Sector) {
		//Controller = Application.Controller;
		Sprite = SpriteManager.Create("images/snowball/snowball.sprite");
	}

	public override void SetupGraphics(Layer Layer) {
		Layer.Add(10f, Sprite);
	}

	public override void Update(float ElapsedTime) {

	}
}

