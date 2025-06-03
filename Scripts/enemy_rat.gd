extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -400.0



#func _physics_process(delta):
	
func _physics_process(delta):
	var current_state
	const IDLE = 0
	current_state = IDLE
	
	$AnimatedSprite2D.play("idle")
	#match(current_state):
		#current_state.0: _idle_state()
		#current_state.CHASE: _chase_state()
		#current_state.ATTACK: _attack_state()
	
#func _idle_state():
	#$AnimationPlayer.play("idle")
	#return
