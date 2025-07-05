using Godot;
using System;

public partial class SpriteEmitter : Node2D
{

	[Export]
	public PackedScene sprite;
	[Export]
	public float spriteXVelocity = 1.6f;

	private int timer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void EmitSprite()
	{
		var spriteInstance = sprite.Instantiate<AnimatedSprite2D>();
		var spriteScript = (SpriteAnim)spriteInstance;
		//var spriteNode = spriteInstance.GetNode<AnimatedSprite2D>("DustAnim");
		//spriteNode.Position = this.Position;
		spriteInstance.Position = this.GlobalPosition;
		//spriteScript.xVel = 100.0f;

		GetTree().Root.GetChild(1).AddChild(spriteInstance);
	}

	public void EmitSprite(float xVelocity)
	{
		var spriteInstance = sprite.Instantiate<AnimatedSprite2D>();
		var spriteScript = (SpriteAnim)spriteInstance;
		spriteScript.xVel = xVelocity;
		spriteInstance.Position = this.GlobalPosition;
		GetTree().Root.GetChild(1).AddChild(spriteInstance);
	}

	public void OnPlayerJumped()
	{
		int facing = GameController.Instance.playerMovementDirection;
		if (facing <= 0)
		{
			EmitSprite(spriteXVelocity);
		}
		if (facing >= 0)
		{
			EmitSprite(-spriteXVelocity);
		}
	}
}
