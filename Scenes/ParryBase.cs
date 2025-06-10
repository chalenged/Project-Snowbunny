using Godot;
using System;

public partial class ParryBase : Node2D
{
    public float ParryTime = 0.5f;

    private float lifeTime = 0.0f;

    public override void _Process(double delta) {
        lifeTime+= (float)delta;
        if (lifeTime > ParryTime) QueueFree();
    }
}
