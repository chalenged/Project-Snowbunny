using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{

	private VBoxContainer RoomListNode;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RoomListNode = GetNode<VBoxContainer>("Control/MenuMargin/ScrollContainer/SceneList");
		var RoomList = ResourceLoader.ListDirectory("res://Scenes/Rooms");
		foreach (string roomName in RoomList)
		{
			var RoomButton = ResourceLoader.Load<PackedScene>("res://Scenes/scene_button.tscn").Instantiate();
			var ButtonLabel = RoomButton.GetNode<Label>("Label");
			ButtonLabel.Text = roomName;
			RoomListNode.AddChild(RoomButton);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
