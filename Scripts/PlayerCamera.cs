using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
	private Vector2 viewPort;
	private float cameraYOffset;
	private float cameraXOffset;

	[Export]
	private float cameraSpeed = 1.5f;
	private float minCameraSpeed = 0.3f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var viewPort = GetViewport().GetVisibleRect().Size;
		cameraYOffset = viewPort.Y/2;
		cameraXOffset = viewPort.X/2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var cameraPos = this.GlobalPosition;
		cameraPos.Y = Math.Clamp(cameraPos.Y, this.LimitTop+cameraYOffset, this.LimitBottom-cameraYOffset);
		cameraPos.X = Math.Clamp(cameraPos.X, this.LimitLeft+cameraXOffset, this.LimitRight-cameraXOffset);
		var playerPos = GameController.Instance.playerPos;
		var distance = Position.DistanceTo(playerPos);
		Position = Position.MoveToward(playerPos,(float)delta*cameraSpeed*distance);
		Position = Position.MoveToward(playerPos, minCameraSpeed);
		Position = new Vector2((float)Math.Round(Position.X,1),(float)Math.Round(Position.Y,1));
		GameController.Instance.cameraPos = cameraPos;
	}
}
