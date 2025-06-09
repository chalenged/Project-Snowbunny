using Godot;
using System;

public partial class GunBase : Node2D
{
	
	[Export]
	public float recoil = 0.1f; //unsure
	[Export]
	public float fireRate = 1.0f; //Thinking should be shots per second?
	[Export]
	public float damage = 1.0f;
	[Export]
	public float speed = 150.0f; //pixels/second
	[Export]
	public PackedScene projectile = GD.Load<PackedScene>("res://Scenes/BulletBase.tscn");
	private float cooldown = 0.0f; //increases until it reaches firerate
	void Shoot() {
		if (cooldown < fireRate) return;
		RigidBody2D instance = (RigidBody2D)projectile.Instantiate();
		instance.Rotation = Rotation;
		instance.LinearVelocity = new Vector2(1.0f,0f).Rotated(Rotation) * speed;
		GetTree().Root.GetChild(0).AddChild(instance);
		instance.Position = ((Node2D)GetNode("BulletPoint")).GlobalPosition;
		cooldown = 0.0f;
	}

    public override void _Process(double delta){ //The underscore indicates the variable isn't used
        LookAt(GetGlobalMousePosition());
        CharacterBody2D player = GetNode<CharacterBody2D>("..");
        cooldown += (float)delta;
        if (Input.IsActionJustPressed("shoot")) {
            Shoot();
        }
    }

}
