extends CharacterBody2D


signal hit
const SPEED = 100.0
const JUMP_VELOCITY = -400.0
var gravity: Vector2
var current_state
func _physics_process(delta): 
	
	#get gravity
	gravity = get_gravity()
	if is_on_floor() == false:
		velocity.y += gravity.y * delta
	if $"../BlobPlayer".position.distance_to(position) < 300:
		current_state = 1
	else:
		current_state = 0
	#current_state = 0
	match(current_state):
		0:
			_idle_state()
		1:
			_angry_state()
	move_and_slide()
	
func _idle_state():
	velocity.x = velocity.x * 0
	$AnimatedSprite2D.play("idle")
	return
func _angry_state():
	var direction = ($"../BlobPlayer".position - position).normalized() 
	$AnimatedSprite2D.play("idle")
	velocity.x = direction.x * SPEED
	return
	
	
	
