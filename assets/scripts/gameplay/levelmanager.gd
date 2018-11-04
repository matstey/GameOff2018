extends Node

onready var levels = [ 
	preload("res://assets/scenes/level0.tscn"),
	preload("res://assets/scenes/level1.tscn")
]

func loadlevel(index):
	
	assert(index < levels.size())
	
	var level = levels[index].instance();
	level.name = "level%d" % index
	get_node("/root/main").add_child(level)
	
	pass
