using Godot;
using System;

public partial class TransitionScreen : CanvasLayer
{
	//public float xOffset = 0.0f;
	private float viewPort = 0.0f;
	[Export]
	public  float offsetMultiplier = 1.5f;


	[Export]
	public float targetOffsetMargin = 40.0f;

	[Export]
	public int transitionFrames = 80;

	public float defaultTargetOffset = 0.0f;
	public float targetOffset = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		viewPort = GetViewport().GetVisibleRect().Size.X;
		defaultTargetOffset = viewPort*offsetMultiplier;
		targetOffset = defaultTargetOffset;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Offset.X < targetOffset-targetOffsetMargin || Offset.X > targetOffset+targetOffsetMargin)
		{
			SetOffset(Offset.MoveToward(new Vector2(targetOffset, Offset.Y),defaultTargetOffset/transitionFrames));
		}

	}

	


}
