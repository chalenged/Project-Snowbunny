using Godot;
using System.Diagnostics;
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
	public float prevTimeScale = DEFAULT_TIMESCALE;
	public bool bulletTime = false;
	public Vector2 velocity;
	public int facing = 1;
	public int spriteX = 1;
		



	[Signal]
	public delegate void PlayerJumpedEventHandler();


	public override void _Ready()
	{
		this.AddToGroup("player", true); //Adds to group for easier reference, can use GetTree().GetFirstNodeInGroup("player") in any script to find player (will return nill if no player!!!)
		//JumpParticleNode.EmitFlags = 8;
		GameController.Instance.timeScale = DEFAULT_TIMESCALE;
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

		// Set player position global variable
		GameController.Instance.playerPos = this.Position;
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
			coyoteFrames = Math.Max(coyoteFrames-(1*GameController.Instance.timeScale),0);
			// Jump higher if jump button is held down and fall faster if jump is released
			if (!jumpRelease && velocity.Y < 0)
			{
				velocity.Y += gravity.Y * delta * 0.8f * GameController.Instance.timeScale * GameController.Instance.timeScale;
			}
			else if (jumpRelease && velocity.Y < 100.0f)
			{
				velocity.Y += gravity.Y * delta * 2 * GameController.Instance.timeScale * GameController.Instance.timeScale;
			}
			else
			{
				velocity.Y += gravity.Y * delta * GameController.Instance.timeScale * GameController.Instance.timeScale;
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
				
				/*if (grounded)
				{
				}*/
				velocity.Y = JUMP_VELOCITY * GameController.Instance.timeScale;
				EmitSignal(SignalName.PlayerJumped);
				coyoteFrames = 0;
			}
			else
			{
				velocity.Y = JUMP_VELOCITY * 0.85f * GameController.Instance.timeScale;
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
		if (GameController.Instance.timeScale != targetTimeScale)
		{
			GameController.Instance.timeScale = Mathf.MoveToward(GameController.Instance.timeScale, targetTimeScale, (DEFAULT_TIMESCALE-DEFAULT_BULLETTIME)/BULLETTIME_FRAMES);
		}
		
		// Check if timeScale has changed and adjust velocity
		if (GameController.Instance.timeScale != prevTimeScale)
		{
			velocity = velocity*(GameController.Instance.timeScale/prevTimeScale);
			prevTimeScale = GameController.Instance.timeScale;
		}
	}

	public void MovementLogic(float delta)
	{
		// Get the input direction and handle the movement/deceleration.
		float direction = Input.GetAxis("ui_left", "ui_right");
		facing = (int)direction;
		GameController.Instance.playerMovementDirection = facing;
		if (direction!=0)
		{
			velocity.X = direction * SPEED * GameController.Instance.timeScale;
			if (spriteX != facing && grounded)
			{
				if (facing == 1)
				{
					spriteX = 1;
				}
				else if (facing == -1)
				{
					spriteX = -1;
				}
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, SPEED/(5/GameController.Instance.timeScale/GameController.Instance.timeScale));
		}
	}
	
}
