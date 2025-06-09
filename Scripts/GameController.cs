using Godot;
using System;

public partial class GameController : Node
{
	public static GameController Instance { get ; private set; }

	public Vector2 playerPos {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		playerPos = new Vector2(0,0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
