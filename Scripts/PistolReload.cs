using Godot;
using System;
using System.Diagnostics;


public partial class PistolReload : BaseReload
{
	public const float LENGTH = 60.0f;
	public float target = 0.0f; //target middle in seconds
	public float precision = 0.2f;
	public float leniancy = 0.6f;
	public float timeLength = 2.0f; //how long reload will last
	public override void _Draw() {
		DrawRect(new Rect2(-30.0f,-50.0f,LENGTH,10.0f), Colors.Black);
		DrawRect(new Rect2(-30.0f + target * (LENGTH/timeLength) - (LENGTH*(leniancy/timeLength)/2),-50.0f,LENGTH*(leniancy/timeLength),10.0f), Colors.Gray);
		DrawRect(new Rect2(-30.0f + target * (LENGTH/timeLength) - (LENGTH*(precision/timeLength)/2),-50.0f,LENGTH*(precision/timeLength),10.0f), Colors.WhiteSmoke);
		DrawRect(new Rect2(-30.0f + lifespan * (LENGTH/timeLength)-2.0f,-50.0f,2,10.0f), Colors.Red);
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		QueueRedraw();
		if (Input.IsActionJustPressed("reload")) {
			Debug.Print($"{lifespan} {target}");
			if (Math.Abs(lifespan - target) < precision/2) {
				EmitSignal(SignalName.ReloadSuccess, 1);
			} else if (Math.Abs(lifespan - target) < leniancy/2){
				EmitSignal(SignalName.ReloadSuccess, 0);
			} else {
				EmitSignal(SignalName.ReloadSuccess, -1);
			}
			QueueFree();
		}
	}
}
