using Godot;
using System.Diagnostics;
using System;

public partial class GameController : Node
{
	public static GameController Instance { get ; private set; }

	public Vector2 playerPos {get; set;}
	public float timeScale = 0.0f;

	public int ammoCur = 0;
	public int ammoMax = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		playerPos = new Vector2(0,0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) 
	{
		//Node2D player = (Node2D)(GetTree().GetFirstNodeInGroup("player"));
		//playerPos = player.Position;
		//Should we use physics process? this node processes _before_ player movement so it will be delayed one physics process, maybe player should update this itself? would also work better if player is not in scene (dead)
		//Debug.Print($"{playerPos}");
	}

	public CharacterBody2D GetPlayer() {
		return (CharacterBody2D)GetTree().GetFirstNodeInGroup("player");
	}
}
