using Godot;
using System;
using System.Diagnostics;

public partial class GunBase : Node2D
{
	
	[Export]
	public float recoil = 0.1f; //unsure
	[Export]
	public float fireRate = 0.2f; //It's actually seconds per shot, so i.e. 2 = 2 seconds between shots
	[Export]
	public float damage = 1.0f;
	[Export]
	public float speed = 150.0f; //pixels/second
	[Export]
	public int magSize = 7;
	[Export]
	public float reloadSpeed = 2.0f;
	[Export]
	public float parryWindow = 0.5f;
	[Export]
	public float inaccuracy = 0.0f; // double for extra precision
	[Export]
	public PackedScene projectile = GD.Load<PackedScene>("res://Scenes/BulletBase.tscn");
	[Export]
	public PackedScene parry = GD.Load<PackedScene>("res://Scenes/ParryBase.tscn");
	[Export]
	public PackedScene reload = GD.Load<PackedScene>("res://Scenes/PistolReload.tscn");
	
	public float cooldown = 0.0f; //increases until it reaches firerate
	public float reloadCooldown = 0.0f;
	public bool reloading = false;
	public int mag = 0;
	public RandomNumberGenerator rng;


	public override void _Ready() {
		mag = magSize; //mag should start full
		GameController.Instance.ammoMax = mag;
		rng = new RandomNumberGenerator();
		rng.Randomize();
		AddToGroup("gun");
	}
	
	public virtual void Shoot() {
		if (cooldown < fireRate) return;
		mag--;
		BulletBase instance = (BulletBase)projectile.Instantiate();
		instance.Rotation = Rotation;
		if (Scale.X == -1) {
			instance.Rotation += (float)Math.PI;
		}
		var rand = (float)(rng.RandfRange(-(inaccuracy/2)*(float)(Math.PI/180),(inaccuracy/2)*(float)(Math.PI/180)));
		instance.LinearVelocity = new Vector2(1.0f,0f).Rotated(instance.Rotation+rand) * speed;
		instance.speed = speed;
		//instance.LinearVelocity = instance.LinearVelocity*((float)(rng.Randfn(-inaccuracy,inaccuracy)*(180/Math.PI)));
		GetTree().Root.GetChild(1).AddChild(instance);
		//GetNode<CharacterBody2D>("..").AddChild(instance);
		instance.Position = ((Node2D)GetNode("BulletPoint")).GlobalPosition;
		cooldown = 0.0f;
	}

	public virtual void Reload() {
		reloading = true;
		var parryInstance = parry.Instantiate<Node2D>();
		AddChild(parryInstance);
		var reloadInstance = (PistolReload)(reload.Instantiate());
		reloadInstance.ReloadSuccess += OnReloadSuccess;
		reloadInstance.maxSpeed = reloadSpeed;
		GetNode("..").AddChild(reloadInstance);
		reloadInstance.target = rng.RandfRange(reloadInstance.leniancy/2, reloadSpeed-reloadInstance.leniancy/2);
		//Debug.Print($"{reloadInstance.target}");
	}

	public override void _Process(double delta){ 
		LookAt(GetGlobalMousePosition());
		//CharacterBody2D player = GetNode<CharacterBody2D>("..");
		cooldown += (float)delta * GameController.Instance.timeScale;
		if (reloading) {
			reloadCooldown += (float)delta * GameController.Instance.timeScale;
			if (reloadCooldown >= reloadSpeed) {
				mag = magSize;
				reloading = false;
				reloadCooldown = 0.0f;
			}
		}
		if (Math.Abs(Math.Floor(((Rotation + Math.PI/2)/(Math.PI/2))/2))%2 == 1) { // if aiming Left (watch out it's MATH!)
			Rotation += (float)Math.PI;
			var nScale = Scale;
			nScale.X = -1;
			Scale = nScale;
		} else {
			var nScale = Scale;
			nScale.X = 1;
			Scale = nScale;
		}
		if (Input.IsActionPressed("shoot")) {
			if (mag > 0 && !reloading) {
				Shoot();
			}
		}
		if (Input.IsActionJustPressed("reload")) {
			if (mag < magSize && !reloading) {
				Reload();
			}
		}
		GameController.Instance.ammoCur = mag;
	}

	public virtual void OnReloadSuccess(int level) {
		Debug.Print($"{level}");
		if (level >= 0) {
			reloading = false;
			mag = magSize;
			reloadCooldown = 0.0f;
		} else {
			reloadCooldown -= 1.0f;
		}
	}
}
