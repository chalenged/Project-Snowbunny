using Godot;
using System;

public partial class BlobPlayer : CharacterBody2D
{
	public const float SPEED = 300.0f;
	public const float JUMP_VELOCITY = -400.0f;
	public const int MAX_JUMPS = 2;
	public const float MAX_COYOTE = 7.0f;
	public const float DEFAULT_TIMESCALE = 1.0f;
	public const float DEFAULT_BULLETTIME = 0.3f;
	public const int BULLETTIME_FRAMES = 15;
	public float coyoteFrames = 0.0f;
	public int jumps = 0;
	public bool grounded = false;
	public bool jumpRelease = true;
	public Vector2 gravity;
	public float timeScale = DEFAULT_TIMESCALE;
	public float prevTimeScale = DEFAULT_TIMESCALE;
	public bool bulletTime = false;


	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		// Get gravity at the beginning of every frame
		gravity = GetGravity();
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
				velocity.Y += gravity.Y * (float)delta * 0.8f * timeScale * timeScale;
			}
			else if (jumpRelease && velocity.Y < 100.0f)
			{
				velocity.Y += gravity.Y * (float)delta * 2 * timeScale * timeScale;
			}
			else
			{
				velocity.Y += gravity.Y * (float)delta * timeScale * timeScale;
			}
		}

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
				velocity.Y = JUMP_VELOCITY * timeScale;
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
			
		// Toggle Bullet time
		if (Input.IsActionJustPressed("ui_up"))
		{
			bulletTime = !bulletTime;
		}
		
		// Gradually change timeScale depending on BULLETTIME_FRAMES
		if (bulletTime && timeScale != DEFAULT_BULLETTIME)
		{
			timeScale = Mathf.MoveToward(timeScale, DEFAULT_BULLETTIME, (DEFAULT_TIMESCALE-DEFAULT_BULLETTIME)/BULLETTIME_FRAMES);
		}
		else if (!bulletTime && timeScale != DEFAULT_TIMESCALE)
		{
			timeScale = Mathf.MoveToward(timeScale, DEFAULT_TIMESCALE, (DEFAULT_TIMESCALE-DEFAULT_BULLETTIME)/BULLETTIME_FRAMES);
		}
		
		// Check if timeScale has changed and adjust velocity
		if (timeScale != prevTimeScale)
		{
			velocity = velocity*(timeScale/prevTimeScale);
			prevTimeScale = timeScale;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		float direction = Input.GetAxis("ui_left", "ui_right");
		if (Math.Abs(direction)>0)
		{
			velocity.X = direction * SPEED * timeScale;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, SPEED/(5/timeScale/timeScale));
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
