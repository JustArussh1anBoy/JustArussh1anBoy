extends Node

func _on_ready():
	var combo = 0
	var leftKey = "d"
	var upKey = "f"
	var downKey = "j"
	var rightKey = "k"
	const chart = preload("res://scenes/game/levels/test/chart/chart.gd")
	preload("res://scenes/game/levels/test/chart/chart.gdt.gd")
	preload("res://scenes/game/scripts/combo.gd")
	preload("res://scenes/game/scripts/dbl_left.gd")
	preload("res://scenes/game/scripts/dbl_up.gd")
	preload("res://scenes/game/scripts/dbl_down.gd")
	preload("res://scenes/game/scripts/dbl_right.gd")
	preload("res://scenes/game/scripts/combo.gd")
	main()

func main():
	_on_ready().chart.start()
