extends KinematicBody2D

const MAX_SPEED = 200;

func _ready():
	# Called when the node is added to the scene for the first time.
	# Initialization here
	pass

func _physics_process(delta):
	
	var movement = Vector2(0.0, 0.0);
	
	if(Input.is_action_pressed("ui_up")):
		movement.y -= 1.0
	
	if(Input.is_action_pressed("ui_down")):
		movement.y += 1.0
		
	if(Input.is_action_pressed("ui_left")):
		movement.x -= 1.0
	
	if(Input.is_action_pressed("ui_right")):
		movement.x += 1.0
	
	move_and_slide(movement.normalized() * MAX_SPEED)
	
	pass
