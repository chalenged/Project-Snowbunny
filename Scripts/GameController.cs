using Godot;
using System.Diagnostics;
using System.Collections.Generic;
using System;

public partial class GameController : Node
{
	[Signal]
	public delegate void DamagePlayerEventHandler(int damage);
	public static GameController Instance { get ; private set; }
	public Node CurrentScene;
	public string CurrentSceneString = "";

	public Vector2 playerPos;
	public Vector2 cameraPos;
	public int playerMovementDirection = 0;
	public float timeScale = 0.0f;
	public int TargetPlayerSpawn = 0;
	public int PlayerHealth = 0;

	public int ammoCur = 0;
	public int ammoMax = 0;

	//TODO PLAYERS SPAWN NODE SYSTEM!!!
	public List<Node2D> SpawnNodeList = new List<Node2D>();

	private CanvasLayer PauseMenuNode;
	//private PackedScene PlayerScene;
	private TransitionScreen TransitionScreenScript;
	public int transitionTimer = 0;
	public int transitionBuffer = 30;
	public bool transitionQueued = false;
	public string queuedScene = "";

	public void DoIt(int damage) {
		GD.Print("BLEH?");
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DamagePlayer += DoIt;
		EmitSignal(SignalName.DamagePlayer, 1);
		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(-1);
		Instance = this;
		playerPos = new Vector2(0,0);
		//var PlayerScene = ResourceLoader.Load<PackedScene>("res://Scenes/blob_player.tscn").Instantiate();

		// Make Game Controller unpausable.
		ProcessMode = Node.ProcessModeEnum.Always;

		// Preload UI stuff
		var PauseMenu = ResourceLoader.Load<PackedScene>("res://Scenes/pause_menu.tscn").Instantiate();
		var TransitionScreenInstance = ResourceLoader.Load<PackedScene>("res://Scenes/transition_screen.tscn").Instantiate();
		TransitionScreenScript = (TransitionScreen)TransitionScreenInstance;
		// Add pause menu as child of Game Controller
		AddChild(PauseMenu);
		AddChild(TransitionScreenInstance);

		PauseMenuNode = GetNode<CanvasLayer>("PauseMenu");
		PauseMenuNode.Visible = false;

		CurrentSceneString = GetTree().CurrentScene.SceneFilePath;
		GotoScene(CurrentSceneString);

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
<<<<<<< Updated upstream
			EmitSignal(SignalName.DamagePlayer, 1);
			TransitionToScene(CurrentSceneString);
=======
			EmitSignal(SignalName.DamagePlayer, 1);
			GotoScene(CurrentSceneString);
>>>>>>> Stashed changes
		}

		if (Input.IsActionJustPressed("game_pause"))
		{
			TogglePause();
		}

		if(transitionQueued)
		{
			if (transitionTimer <= 0)
			{
				GotoScene(queuedScene);
			}
			else
			{
				transitionTimer--;
			}

		}


	}

	public CharacterBody2D GetPlayer() {
		return (CharacterBody2D)GetTree().GetFirstNodeInGroup("player");
	}

	public void TransitionToScene(string path)
	{
		TransitionScreenScript.targetOffset = 0.0f;
		queuedScene = path;
		transitionTimer = TransitionScreenScript.transitionFrames+transitionBuffer;
		transitionQueued = true;
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
		transitionQueued = false;
		queuedScene = "";

		CurrentSceneString = path;

		SpawnNodeList.Clear();

		// Load a new scene.
		var nextScene = GD.Load<PackedScene>(path);

		// Instance the new scene.
		CurrentScene = nextScene.Instantiate();

		// Add it to the active scene, as child of root.
		GetTree().Root.AddChild(CurrentScene);

		// Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
		GetTree().CurrentScene = CurrentScene;
		RoomStart();
	}

	public void TogglePause()
	{
		Pause(!GetTree().Paused);
	}

	public void Pause(bool pause)
	{
		GetTree().Paused = pause;
		PauseMenuNode.Visible = GetTree().Paused;
	}

	private void RoomStart()
	{
		CallDeferred(MethodName.DeferredRoomStart);
	}

	private void DeferredRoomStart()
	{
		TransitionScreenScript.SetOffset(new Vector2(0.0f, TransitionScreenScript.Offset.Y));
		TransitionScreenScript.targetOffset = TransitionScreenScript.defaultTargetOffset;
		var PlayerInstance = ResourceLoader.Load<PackedScene>("res://Scenes/blob_player.tscn").Instantiate();
		var CameraInstance = ResourceLoader.Load<PackedScene>("res://Scenes/player_camera.tscn").Instantiate();
		playerPos = new Vector2(0,0);
		GD.Print("Room has started!");
		GD.Print(SpawnNodeList.Count + " Spawn nodes found!");
		var PlayerNode2D = (Node2D)PlayerInstance;
		var CameraNode2D = (Node2D)CameraInstance;
		var SpawnPosition = SpawnNodeList[TargetPlayerSpawn].GlobalPosition;
		PlayerNode2D.Position = SpawnPosition;
		CameraNode2D.Position = SpawnPosition;
		GetTree().CurrentScene.AddChild(PlayerInstance);
		GetTree().CurrentScene.AddChild(CameraInstance);
		Pause(false);
		
		


	}
}
