using Godot;
using System;

public partial class BlobPlayer : CharacterBody2D
{
	[Export]
	public float SPEED = 300.0f;
	[Export]
	public float JUMP_VELOCITY = -400.0f;
	[Export]
	public int MAX_JUMPS = 2;
	[Export]
	public float MAX_COYOTE = 7.0f;
	[Export]
	public float DEFAULT_BULLETTIME = 0.3f;
	[Export]
	public int BULLETTIME_FRAMES = 15;

	public const float DEFAULT_TIMESCALE = 1.0f;
	public float targetTimeScale = 1.0f;
	public float coyoteFrames = 0.0f;
	public int jumps = 0;
	public bool grounded = false;
	public bool jumpRelease = true;
	public Vector2 gravity;
	public float timeScale = DEFAULT_TIMESCALE;
	public float prevTimeScale = DEFAULT_TIMESCALE;
	public bool bulletTime = false;
	public Vector2 velocity;
	public int facing = 1;
	public int spriteX = 1;
	
	public GpuParticles2D JumpParticleNode;
	public Material JumpParticleMaterial = GD.Load<Material>("res://Scenes/blob_jump_particle_material.tres");
	public Material JumpParticleMaterialFlip = GD.Load<Material>("res://Scenes/blob_jump_particle_material_flip.tres");


	//[Signal]
	//public delegate void PlayerJumpedEventHandler();


	public override void _Ready()
	{
		JumpParticleNode = GetNode<GpuParticles2D>("JumpParticle");
		//JumpParticleNode.EmitFlags = 8;
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;
		// Get gravity at the beginning of every frame
		gravity = GetGravity();
		
		// Check if grounded and handle gravity calculations on player velocity.
		GroundedCheck((float)delta);


		// Handle Bullettime input
		BulletTimeLogic((float)delta);

		// Handle horizontal movement
		MovementLogic((float)delta);

		// Handle jump input
		JumpLogic((float)delta);
		//velocity.X = (int)velocity.X;
		//velocity.Y = (int)velocity.Y;

		//Apply calculated velocity to player
		Velocity = velocity;
		MoveAndSlide();

	}




	public void GroundedCheck(float delta)
	{
		// Check if player is on floor and resets jumps to max and adds gravity if not.
		if (IsOnFloor())
		{
			grounded = true;
			jumpRelease = true;
			jumps = MAX_JUMPS;
			coyoteFrames = MAX_COYOTE;
		}
		else
		{
			grounded = false;
			coyoteFrames = Math.Max(coyoteFrames-(1*timeScale),0);
			// Jump higher if jump button is held down and fall faster if jump is released
			if (!jumpRelease && velocity.Y < 0)
			{
				velocity.Y += gravity.Y * delta * 0.8f * timeScale * timeScale;
			}
			else if (jumpRelease && velocity.Y < 100.0f)
			{
				velocity.Y += gravity.Y * delta * 2 * timeScale * timeScale;
			}
			else
			{
				velocity.Y += gravity.Y * delta * timeScale * timeScale;
			}
		}
	}

	public void JumpLogic(float delta)
	{
		// Remove 1 jump when CoyoteFrames run out
		if (jumps == MAX_JUMPS && coyoteFrames == 0)
		{
			jumps -= 1;
		}
		// Handle jump.
		if (Input.IsActionJustPressed("ui_accept") && jumps>0)
		{
			jumpRelease = false;
			jumps = Math.Max(jumps-1,0);
			if (coyoteFrames>0)
			{
				// TODO MAKE THIS SHIT WORK WITH XYZ SCALECURVE
				//JumpParticleNode.GetProcessMaterial().GetParamTexture(ScaleCurve);
				//JumpParticleNode.ProcessMaterial;
				
				JumpParticleNode.EmitParticle(this.Transform,new Vector2((float)facing*-40.0f,0), new Color(1,1,1,1), new Color(1,1,1,1), 5);
				velocity.Y = JUMP_VELOCITY * timeScale;
				//EmitSignal(SignalName.PlayerJumped);
			}
			else
			{
				velocity.Y = JUMP_VELOCITY * 0.85f * timeScale;
			}
		}
		
		// Check if spacebar is not held
		if (Input.IsActionJustReleased("ui_accept") && !jumpRelease)
		{
			jumpRelease = true;
		}
	}
	
	public void BulletTimeLogic(float delta)
	{
		// Toggle Bullet time
		if (Input.IsActionJustPressed("ui_up"))
		{
			bulletTime = !bulletTime;
			if (bulletTime)
			{
				targetTimeScale = DEFAULT_BULLETTIME;
			}
			else
			{
				targetTimeScale = DEFAULT_TIMESCALE;
			}
		}
		
		// Gradually change timeScale depending on BULLETTIME_FRAMES
		if (timeScale != targetTimeScale)
		{
			timeScale = Mathf.MoveToward(timeScale, targetTimeScale, (DEFAULT_TIMESCALE-DEFAULT_BULLETTIME)/BULLETTIME_FRAMES);
		}
		
		// Check if timeScale has changed and adjust velocity
		if (timeScale != prevTimeScale)
		{
			velocity = velocity*(timeScale/prevTimeScale);
			prevTimeScale = timeScale;
		}
	}

	public void MovementLogic(float delta)
	{
		// Get the input direction and handle the movement/deceleration.
		float direction = Input.GetAxis("ui_left", "ui_right");
		facing = (int)direction;
		if (direction!=0)
		{
			velocity.X = direction * SPEED * timeScale;
			if (spriteX != facing && grounded)
			{
				if (facing == 1)
				{
					spriteX = 1;
					JumpParticleNode.SetProcessMaterial(JumpParticleMaterial);
				}
				else if (facing == -1)
				{
					spriteX = -1;
					JumpParticleNode.SetProcessMaterial(JumpParticleMaterialFlip);
				}
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, SPEED/(5/timeScale/timeScale));
		}

		
	}
}
