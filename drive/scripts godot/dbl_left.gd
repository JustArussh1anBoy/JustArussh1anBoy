extends Node

func double(delta, event):
	var child_node = $Note1.duplicate()
	add_child(child_node)
	var position = child_node.position
	var placeholder = $NotePlaceholder1
	for i in range(500):
		position += Vector2(1, 1)
		await get_tree().create_timer(0.1).timeout
		child_node.position = position
		if child_node is CollisionObject2D and placeholder is CollisionObject2D:
			if child_node.get_collision_shape().intersects(placeholder.get_collision_shape()):
				if event is InputEventKey and event.pressed:
						var pressed = OS.get_keycode_string(event.keycode)
						var key = $onStart.leftKey
						if pressed == key:
							child_node.queue_free()
							$combo.upCombo()
						else:
							$combo.resetCombo()
