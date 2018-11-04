extends KinematicBody2D

export (int) var MaxSpeed = 100;

var chase = false;
var target;

func _ready():
	
	pass

func _physics_process(delta):
	
	if(chase):
		update_chase()
	
	pass
	
func update_chase():

	var direction = target.position - position
	
	move_and_slide(direction.normalized() * MaxSpeed)

	pass

func _on_detectionarea_body_entered(body):

	chase = true
	target = body;
	
	pass


func _on_detectionarea_body_exited(body):
	
	chase = false
	
	pass
