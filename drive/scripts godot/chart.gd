extends Node

func _ready():
	var left = $dbl_left
	var up = $dbl_up
	var down = $dbl_down
	var right = $dbl_right
	
	left.double()
	up.double()
	down.double()
	right.double()
