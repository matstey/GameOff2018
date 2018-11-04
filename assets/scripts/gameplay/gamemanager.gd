extends Node

onready var enemy_manager = preload("enemymanager.gd").new()
onready var level_manager = preload("levelmanager.gd").new()

func _ready():

	print("Game manager loading...")
	
	enemy_manager.name = "enemymanager"
	add_child(enemy_manager)
	
	level_manager.name = "levelmanager"
	add_child(level_manager)	
	level_manager.loadlevel(0)
	
	print("Game manager loaded")

	pass
