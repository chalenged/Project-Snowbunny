extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0
const MAX_JUMPS = 2
var jumps: int = 0
var grounded: bool = false
var jumpRelease: bool = true
var gravity


func _physics_process(delta: float) -> void:
	# Get gravity at the beginning of every frame
	gravity = get_gravity()
	# Check if player is on floor and resets jumps to max and adds gravity if not.
	if is_on_floor():
		grounded = true
		jumps = MAX_JUMPS
	else:
		grounded = false
		# Jump higher if jump button is held down and fall faster if jump is released
		if !jumpRelease and velocity.y < 0:
			velocity += gravity * delta * 0.8
		elif jumpRelease and velocity.y < 100:
			velocity += gravity * delta * 2
		else:
			velocity += gravity * delta

	# Handle jump.
	if Input.is_action_just_pressed("ui_accept") and jumps>0:
		if grounded:
			velocity.y = JUMP_VELOCITY
			jumpRelease = false
		else:
			velocity.y = JUMP_VELOCITY*0.85
			jumpRelease = false
	
	# Check if spacebar is not held
	if Input.is_action_just_released("ui_accept") and !jumpRelease:
		jumpRelease = true;
		
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction := Input.get_axis("ui_left", "ui_right")
	if direction:
		velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

	move_and_slide()
