using Godot;
using System;

public partial class SpriteAnim : AnimatedSprite2D
{
	[Export]
	public float lifeTime = 100.0f;
	[Export]
	public float xVel = 1.6f;
	[Export]
	public float yVel = 0.0f;

	private Vector2 vel = new Vector2(0.0f,0.0f);
	private float timeScale = 1.0f;
	private float velMultiplier = 1.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var dir = GameController.Instance.playerMovementDirection;
		xVel = xVel * -dir;
		vel.X = xVel;

		if (dir < 0) FlipH = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		timeScale = GameController.Instance.timeScale;
		SpeedScale = timeScale;
		this.Position += vel;
		vel *= new Vector2(0.97f,1.0f);
		lifeTime -= 1.0f*timeScale;
		if (lifeTime<0)
		{
			this.QueueFree();
		}
	}
}
