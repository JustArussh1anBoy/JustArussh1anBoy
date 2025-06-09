extends Node

func upCombo():
	var first_digit = 0
	var second_digit = 0
	var third_digit = 0
	var t_combonum
	var t_skin
	const combo0 = preload("res://textures/game.tscn/numbers/0.png")
	const combo1 = preload("res://textures/game.tscn/numbers/1.png")
	const combo2 = preload("res://textures/game.tscn/numbers/2.png")
	const combo3 = preload("res://textures/game.tscn/numbers/3.png")
	const combo4 = preload("res://textures/game.tscn/numbers/4.png")
	const combo5 = preload("res://textures/game.tscn/numbers/5.png")
	const combo6 = preload("res://textures/game.tscn/numbers/6.png")
	const combo7 = preload("res://textures/game.tscn/numbers/7.png")
	const combo8 = preload("res://textures/game.tscn/numbers/8.png")
	const combo9 = preload("res://textures/game.tscn/numbers/9.png")
	
	if $onStart.combo != null and str($onStart.combo) != "":
		$onStart.combo += 1
		if len(str($onStart.combo)) == 1:
			first_digit = str($onStart.combo)[0]
		elif len(str($onStart.combo)) == 2:
			first_digit = str($onStart.combo)[1]
			second_digit = str($onStart.combo)[0]
		elif len(str($onStart.combo)) == 3:
			first_digit = str($onStart.combo)[2]
			second_digit = str($onStart.combo)[1]
			third_digit = str($onStart.combo)[0]
		
		t_combonum = first_digit
		t_skin = $combo0
		if t_combonum == "0":
			t_skin.texture = combo0
		elif t_combonum == "1":
			t_skin.texture = combo1
		elif t_combonum == "2":
			t_skin.texture = combo2
		elif t_combonum == "3":
			t_skin.texture = combo3
		elif t_combonum == "4":
			t_skin.texture = combo4
		elif t_combonum == "5":
			t_skin.texture = combo5
		elif t_combonum == "6":
			t_skin.texture = combo6
		elif t_combonum == "7":
			t_skin.texture = combo7
		elif t_combonum == "8":
			t_skin.texture = combo8
		elif t_combonum == "9":
			t_skin.texture = combo9
		
		t_combonum = second_digit
		t_skin = $combo1
		if t_combonum == "0":
			t_skin.texture = combo0
		elif t_combonum == "1":
			t_skin.texture = combo1
		elif t_combonum == "2":
			t_skin.texture = combo2
		elif t_combonum == "3":
			t_skin.texture = combo3
		elif t_combonum == "4":
			t_skin.texture = combo4
		elif t_combonum == "5":
			t_skin.texture = combo5
		elif t_combonum == "6":
			t_skin.texture = combo6
		elif t_combonum == "7":
			t_skin.texture = combo7
		elif t_combonum == "8":
			t_skin.texture = combo8
		elif t_combonum == "9":
			t_skin.texture = combo9
		
		t_combonum = third_digit
		t_skin = $combo2
		if t_combonum == "0":
			t_skin.texture = combo0
		elif t_combonum == "1":
			t_skin.texture = combo1
		elif t_combonum == "2":
			t_skin.texture = combo2
		elif t_combonum == "3":
			t_skin.texture = combo3
		elif t_combonum == "4":
			t_skin.texture = combo4
		elif t_combonum == "5":
			t_skin.texture = combo5
		elif t_combonum == "6":
			t_skin.texture = combo6
		elif t_combonum == "7":
			t_skin.texture = combo7
		elif t_combonum == "8":
			t_skin.texture = combo8
		elif t_combonum == "9":
			t_skin.texture = combo9
	else:
		$onStart.combo = 0

func resetCombo():
	$onStart.combo = 0
	upCombo().first_digit = "0"
	upCombo().second_digit = "0"
	upCombo().third_digit = "0"
	upCombo().first_digit.texture = upCombo().combo0
	upCombo().second_digit.texture = upCombo().combo0
	upCombo().third_digit.texture = upCombo().combo0
