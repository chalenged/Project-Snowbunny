extends CharacterBody2D

#gunrat, the rat with a gun
const SPEED = 100.0
var gravity: Vector2
var current_state
var playerDistance
var playerDistanceY
var direction
var Health = 5
var attackCooldown = 0
var bullet = preload("res://Scenes/BulletBase.tscn")

func _ready():
	$AnimatedSprite2D.play("idle")

func _physics_process(delta):
	attackCooldown += delta * $"/root/GameController".timeScale
	#get distance to player
	playerDistance = $"/root/GameController".playerPos.distance_to(position)
	direction = ($"/root/GameController".playerPos - position).normalized()
	gravity = get_gravity()
	if is_on_floor() == false:
		velocity.y += gravity.y * delta
	
	if Health < 1:
		_death()
	#get direction to player
	direction = ($"/root/GameController".playerPos - position).normalized()
	
	if direction.x > 0:
		rotation_degrees = 180
		scale.y = -1
	else:
		rotation_degrees = 0
		scale.y = 1
	
	if $AnimatedSprite2D.get_animation() != "death" && $AnimatedSprite2D.get_animation() != "hit":
		if playerDistance < 600 && playerDistance >= 300 && attackCooldown >= 5:
			current_state = 2
		elif playerDistance < 300:
			current_state = 1
		else:
			current_state = 0
		match(current_state):
			0:
				_idle_state()
			1:
				_run_state()
			2:
				_attack()
	
	move_and_slide()
	
func _idle_state():
	velocity.x = 0
	$AnimatedSprite2D.play("idle")
	
func _run_state():
	playerDistanceY = $"/root/GameController".playerPos.y - position.y
	if $AnimatedSprite2D.get_animation() != "hit":
		$AnimatedSprite2D.play("idle")
	velocity.x = -direction.x * SPEED * $"/root/GameController".timeScale
	return

func _attack():
	velocity.x = 0
	$AnimatedSprite2D.play("idle")
	attackCooldown = 0
	var instance = bullet.instantiate()
	instance.set_collision_layer_value(2,false)
	instance.set_collision_layer_value(3, true)
	instance.set_linear_velocity(direction)
	add_child(instance)
	return
	
func _death():
	$AnimatedSprite2D.play("death")
	velocity.x = randf_range(50,100) * -direction.x
	set_collision_layer_value(1, false)
	$BulletCollision.set_collision_mask_value(2, false)
	await get_tree().create_timer(1.0).timeout
	queue_free()
	return
	


func _on_bullet_collision_area_entered(area):
	velocity.x = 0
	Health = Health - 1
	$AnimatedSprite2D.frame = 0
	$AnimatedSprite2D.play("hit")
	await $AnimatedSprite2D.animation_finished
	$AnimatedSprite2D.play("idle")
	return
