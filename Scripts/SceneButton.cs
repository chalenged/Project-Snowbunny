using Godot;
using System;

public partial class SceneButton : Button
{
	private string RoomPath = "res://Scenes/Rooms/";
	private string RoomName;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RoomName = GetNode<Label>("Label").Text;
		RoomPath += RoomName; 
	}

	public override void _Pressed()
	{
		GameController.Instance.TransitionToScene(RoomPath);
	}


}
