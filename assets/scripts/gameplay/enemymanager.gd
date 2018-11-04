extends Node

onready var enemy_prefab = preload("res://assets/prefabs/enemies/enemy.tscn")

func _ready():
	
	spawn()
	
	print("Enemy manager loaded")
	
	pass

func spawn():
	
	var enemy = enemy_prefab.instance();
	enemy.position = Vector2(370.0, 210.0)
	
	
	get_node("/root/main").add_child(enemy)
	
	pass
