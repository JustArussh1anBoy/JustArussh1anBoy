extends Node

func _doubleDWN(delta):
	var child_node = $Note3.dublicate()
	add_child(child_node)
	var position = child_node.postition
	position += Vector2(1, 1)
	var placeholder = $NotePlaceholder
	if CollisionObject2D(placeholder, child_node):
		if event is InputEventKey and event.pressed:
			var pressed = OS.get_keycode_string(event.keycode)
			var key = $onStart.downKey
			if pressed = key:
				child_node.destroy()
