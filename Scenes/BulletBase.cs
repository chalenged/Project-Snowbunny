using Godot;
using System;

public partial class BulletBase : RigidBody2D
{
	public float speed = 1000.0f;
	public override void _Process(double _delta)
	{
		Rotation = LinearVelocity.Angle();
		LinearVelocity = LinearVelocity.Normalized() * (speed * GameController.Instance.timeScale) ;
	}
}
