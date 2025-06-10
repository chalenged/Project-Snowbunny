using Godot;
using System;

public partial class BaseReload : Node2D //this class does nothing, just establishes some basic things for inheritance
{
	[Signal]
	public delegate void ReloadSuccessEventHandler(int level);
    public float maxSpeed = 5.0f;
    public float lifespan = 0.0f;
    public override void _Ready() {

    }
    public override void _Draw() {
       
    }

    public override void _Process(double delta)
    {
        lifespan += (float)delta * GameController.Instance.timeScale;
        if (lifespan > maxSpeed) {
            QueueFree();
        }
    }
}
