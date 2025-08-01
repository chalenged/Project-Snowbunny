using Godot;
using System;

public partial class BackgroundLayer : TextureRect
{

	private Vector2 cameraPos;

	[Export]
	private float xOffset;
	[Export]
	private float yOffset;
	[Export]
	private float xMult;
	[Export]
	private float yMult;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	
	public override void _PhysicsProcess(double delta)
	{
		cameraPos = GameController.Instance.cameraPos;
		Position = new Vector2((cameraPos.X*xMult)+xOffset,(cameraPos.Y*yMult)+yOffset);
		Position = new Vector2((float)Math.Round(Position.X,1),(float)Math.Round(Position.Y,1));
	}
}
