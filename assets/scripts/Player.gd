extends KinematicBody2D

export (int) var speed = 400
export (Vector2) var deadzone = Vector2(0.1,0.1)

var m_moveVelocity = Vector2();

func _ready():
	# Called when the node is added to the scene for the first time.
	# Initialization here	
	pass

func _process(delta):
	pass

	
func getInput():
	var x = Input.get_joy_axis(0, JOY_AXIS_0)
	if(abs(x) < deadzone.x):
		x = 0
	
	var y = Input.get_joy_axis(0, JOY_AXIS_1)
	if(abs(y) < deadzone.y):
		y = 0
		
	
	m_moveVelocity = Vector2(x, y)

#	m_moveVelocity = Vector2();
#	if Input.is_action_pressed('right'):
#		m_moveVelocity.x += 1
#	if Input.is_action_pressed('left'):
#		m_moveVelocity.x -= 1
#	if Input.is_action_pressed('down'):
#		m_moveVelocity.y += 1
#	if Input.is_action_pressed('up'):
#		m_moveVelocity.y -= 1

	m_moveVelocity = m_moveVelocity * speed


func _physics_process(delta):
	getInput();
	move_and_slide(m_moveVelocity)

