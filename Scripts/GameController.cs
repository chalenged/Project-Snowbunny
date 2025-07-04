using Godot;
using System.Diagnostics;
using System;

public partial class GameController : Node
{
	public static GameController Instance { get ; private set; }
	public Node CurrentScene {get; set;}

	public Vector2 playerPos {get; set;}
	public int playerMovementDirection = 0;
	public float timeScale = 0.0f;

	public int ammoCur = 0;
	public int ammoMax = 0;

	private CanvasLayer PauseMenuNode;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(-1);
		Instance = this;
		playerPos = new Vector2(0,0);

		// Make Game Controller unpausable.
		ProcessMode = Node.ProcessModeEnum.Always;

		// Preload pause menu
		var PauseMenu = ResourceLoader.Load<PackedScene>("res://Scenes/pause_menu.tscn").Instantiate();
		// Add pause menu as child of Game Controller
		AddChild(PauseMenu);

		PauseMenuNode = GetNode<CanvasLayer>("PauseMenu");
		PauseMenuNode.Visible = false;
		


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) 
	{
		//Node2D player = (Node2D)(GetTree().GetFirstNodeInGroup("player"));
		//playerPos = player.Position;
		//Should we use physics process? this node processes _before_ player movement so it will be delayed one physics process, maybe player should update this itself? would also work better if player is not in scene (dead)
		//Debug.Print($"{playerPos}");

		if (Input.IsActionJustPressed("scene_reload"))
		{
			GetTree().ReloadCurrentScene();
		}

		if (Input.IsActionJustPressed("game_pause"))
		{
			GetTree().Paused = !GetTree().Paused;
			PauseMenuNode.Visible = GetTree().Paused;

		}


	}

	public CharacterBody2D GetPlayer() {
		return (CharacterBody2D)GetTree().GetFirstNodeInGroup("player");
	}

	public void GotoScene(string path)
	{
		CurrentScene = GetTree().CurrentScene;

		//Make sure code is done running before changing scenes
		CallDeferred(MethodName.DeferredGotoScene, path);
	}

	public void DeferredGotoScene(string path)
	{
		// It is now safe to remove the current scene.
		CurrentScene.Free();

		// Load a new scene.
		var nextScene = GD.Load<PackedScene>(path);

		// Instance the new scene.
		CurrentScene = nextScene.Instantiate();

		// Add it to the active scene, as child of root.
		GetTree().Root.AddChild(CurrentScene);

		// Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
		GetTree().CurrentScene = CurrentScene;
	}
}
