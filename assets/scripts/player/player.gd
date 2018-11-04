extends KinematicBody2D

export (int) var speed = 400
export (Vector2) var deadzone = Vector2(0.1,0.1)

var _moveVelocity = Vector2()

var _bodyAnim
var _legAnim
var _meleeAnim
var _attacking = false
var _swordUp = false

var _spriteNodes = []

func _ready():
	# Called when the node is added to the scene for the first time.
	# Initialization here
	_bodyAnim = get_node("BodyAnimationPlayer")
	_legAnim = get_node("LegAnimationPlayer")
	_meleeAnim = get_node("MeleeAnimationPlayer")
	
	for c in get_children():
		if c is Sprite:
			_spriteNodes.append(c)

func _process(delta):
	
	var animSpeed = 0 
	
	if(_moveVelocity.x != 0):
		Flip(_moveVelocity.x < 0)
		animSpeed = _moveVelocity.length()
	else:
		_legAnim.seek(0)
		_bodyAnim.seek(0)

	_legAnim.playback_speed = animSpeed
	_bodyAnim.playback_speed = animSpeed
	
	if(not _attacking):
		_meleeAnim.playback_speed = animSpeed		
	
	if Input.is_action_just_pressed('melee') and !_attacking:
		Attack()
	
func Attack():
	_meleeAnim.playback_speed = 1
	if _swordUp:
		_meleeAnim.play("AttackDown")
	else:
		_meleeAnim.play("AttackUp")
		
	_attacking = true

func Flip(flip):
	for s in _spriteNodes:
		s.flip_h = flip;
	
func calculateMovement():	
	var x = Input.get_joy_axis(0, JOY_AXIS_0)
	if(abs(x) < deadzone.x):
		x = 0
	
	var y = Input.get_joy_axis(0, JOY_AXIS_1)
	if(abs(y) < deadzone.y):
		y = 0
		
	_moveVelocity = Vector2(x, y)
	
	if(abs(x) <= 0 and abs(y) <= 0):
		if Input.is_action_pressed('right'):
			_moveVelocity.x += 1
		if Input.is_action_pressed('left'):
			_moveVelocity.x -= 1
		if Input.is_action_pressed('down'):
			_moveVelocity.y += 1
		if Input.is_action_pressed('up'):
			_moveVelocity.y -= 1
	_moveVelocity = _moveVelocity

func _physics_process(delta):
	calculateMovement();
	move_and_slide(_moveVelocity * speed)

func _on_MeleeAnimationPlayer_animation_finished(anim_name):
	match anim_name:
		"AttackUp":
			_meleeAnim.play("RunUp")
			_swordUp = true
			_attacking = false
		"AttackDown":
			_meleeAnim.play("RunDown")
			_swordUp = false
			_attacking = false
	
