extends CharacterBody2D


const SPEED = 100.0
const JUMP_VELOCITY = -400.0
var gravity: Vector2
var current_state
var playerDistance
var playerDistanceY
var direction
var Health = 5

#runs when scene is loaded for the first time
func _ready():
	#disable attack hurtbox and hide attack sprite
	$TestAttackHurtbox.set_collision_mask_value(3, false)
	$AnimatedSprite2D.play("idle")
	
func _physics_process(delta): 
	
	#get distance to player
	playerDistance = $"/root/GameController".playerPos.distance_to(position)
	#get gravity
	gravity = get_gravity()
	#handle gravity
	if is_on_floor() == false:
		velocity.y += gravity.y * delta
	
	if Health < 1:
		death()
	#get direction to player
	direction = ($"/root/GameController".playerPos - position).normalized()
	
	if direction.x > 0:
		rotation_degrees = 180
		scale.y = -1
	else:
		rotation_degrees = 0
		scale.y = 1
	
	#check if player is between 300 and 50 pixels, moves towards player if they are
	if (playerDistance <= 300 and playerDistance > 35):
		current_state = 1
	#attack player if they are within 50 pixels
	elif playerDistance <= 35:
		current_state = 2
	
	#idle state if player is too far away
	else:
		current_state = 0
	match(current_state):
		0:
			_idle_state()
		1:
			_angry_state()
		2:
			_attack()
	
	move_and_slide()
	
func _idle_state():
	velocity.x = velocity.x * 0
	if $AnimatedSprite2D.is_playing() == false:
		$AnimatedSprite2D.play("idle")
	return
	
func _angry_state():
	playerDistanceY = $"/root/GameController".playerPos.y - position.y
	direction = ($"/root/GameController".playerPos - position).normalized() 
	$AnimatedSprite2D.play("idle")
	velocity.x = direction.x * SPEED
	return
	
func _attack():
	velocity.x = 0
	$AnimatedSprite2D.play("attack")
	$TestAttackHurtbox.set_collision_mask_value(3, true)
	await get_tree().create_timer(1.0).timeout
	$TestAttackHurtbox.set_collision_mask_value(3, false)
	await get_tree().create_timer(1.0).timeout
	return


func _on_bullet_collision_area_entered(_area):
	Health = Health - 1
	print(Health)
	$AnimatedSprite2D.frame = 0
	$AnimatedSprite2D.play ("hit")
	return 
	
func death():
	$AnimatedSprite2D.play("death")
	#$PlayerCollision.disabled = true
	await get_tree().create_timer(1.0).timeout
	queue_free()
	return
