using Godot;
using System;

public partial class BulletBase : RigidBody2D
{
    public override void _Process(double _delta)
	{
        Rotation = LinearVelocity.Angle();
    }
}
