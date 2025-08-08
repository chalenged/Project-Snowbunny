using Godot;
using System;

public partial class TransitionScreen : CanvasLayer
{
	private float xOffset = 0.0f;
	private float viewPort = 0.0f;
	[Export]
	public  float offsetMultiplier = 1.5f;


	[Export]
	public float targetOffsetMargin = 10.0f;

	[Export]
	public int transitionFrames = 80;

	public float targetOffset = 0.0f;
	public bool transitionDone = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		viewPort = GetViewport().GetVisibleRect().Size.X;
		targetOffset = viewPort*offsetMultiplier;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Offset.X < targetOffset-targetOffsetMargin || Offset.X > targetOffset+targetOffsetMargin)
		{
			SetOffset(new Vector2(Offset.X+(targetOffset/transitionFrames),Offset.Y));
			transitionDone = false;
		}
		else if (!transitionDone)
		{
			transitionDone = true;
		}
	}
}
