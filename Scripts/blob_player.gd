extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0
const MAX_JUMPS = 2
const MAX_COYOTE = 5
const DEFAULT_TIMESCALE = 1.0
const DEFAULT_BULLETTIME = 0.3
const BULLETTIME_FRAMES = 15
var coyoteFrames: float = 0.0
var jumps: int = 0
var grounded: bool = false
var jumpRelease: bool = true
var gravity
var timeScale: float = DEFAULT_TIMESCALE
var prevTimeScale: float = timeScale
var bulletTime: bool = false


func _physics_process(delta: float) -> void:
	# Get gravity at the beginning of every frame
	gravity = get_gravity()
	# Check if player is on floor and resets jumps to max and adds gravity if not.
	if is_on_floor():
		grounded = true
		jumpRelease = true
		jumps = MAX_JUMPS
		coyoteFrames = MAX_COYOTE
	else:
		grounded = false
		coyoteFrames = max(coyoteFrames-(1*timeScale),0)
		# Jump higher if jump button is held down and fall faster if jump is released
		if !jumpRelease and velocity.y < 0:
			velocity += gravity * delta * 0.8 * timeScale * timeScale
		elif jumpRelease and velocity.y < 100:
			velocity += gravity * delta * 2 * timeScale * timeScale
		else:
			velocity += gravity * delta * timeScale * timeScale
	
	# Remove 1 jump when CoyoteFrames run out
	if jumps == MAX_JUMPS and coyoteFrames == 0:
		jumps -= 1
	# Handle jump.
	if Input.is_action_just_pressed("ui_accept") and jumps>0:
		jumpRelease = false
		jumps = max(jumps-1,0)
		if coyoteFrames>0:
			velocity.y = JUMP_VELOCITY * timeScale
		else:
			velocity.y = JUMP_VELOCITY * 0.85 * timeScale
	
	# Check if spacebar is not held
	if Input.is_action_just_released("ui_accept") and !jumpRelease:
		jumpRelease = true;
		
	# Toggle Bullet time
	if Input.is_action_just_pressed("ui_up"):
		bulletTime = !bulletTime
		
	# Gradually change timeScale depending on BULLETTIME_FRAMES
	if bulletTime and timeScale != DEFAULT_BULLETTIME:
		timeScale = move_toward(timeScale, DEFAULT_BULLETTIME, (DEFAULT_TIMESCALE-DEFAULT_BULLETTIME)/BULLETTIME_FRAMES)
	elif !bulletTime and timeScale != DEFAULT_TIMESCALE:
		timeScale = move_toward(timeScale, DEFAULT_TIMESCALE, (DEFAULT_TIMESCALE-DEFAULT_BULLETTIME)/BULLETTIME_FRAMES)
		
	# Check if timeScale has changed and adjust velocity
	if timeScale != prevTimeScale:
		#velocity.x = velocity.x*(timeScale/prevTimeScale)
		#velocity.y = velocity.y*(timeScale/prevTimeScale)
		velocity = velocity*(timeScale/prevTimeScale)
		#print (timeScale/prevTimeScale)
		prevTimeScale = timeScale
		
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction := Input.get_axis("ui_left", "ui_right")
	if direction:
		velocity.x = direction * SPEED * timeScale
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED/(5/timeScale/timeScale))
		

	move_and_slide()
