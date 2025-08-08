extends RichTextLabel

var idleShouts = ["ugh.... so [color=slate_blue][wave amp=25.0 freq=4.0]bored...[/wave][/color]", "honk shoo... [color=slate_blue][wave amp=25.0 freq=4.0]mimimi...[/wave][/color]"]
var angryShouts = ["me when i fucking [color=red][shake rate=20.0 level=10]GET[/shake][/color] you",]
var shoutList = [idleShouts, angryShouts]
# Called when the node enters the scene tree for the first time.
func _ready():
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func pickShout(situationList: Array):
	var picked = situationList.pick_random()
	if picked > shoutList.size():
		set_text("uh oh!! out of bounds error!!! dumby!!!")
	else:
		(set_text(shoutList[picked].pick_random()))

func clearShout():
	set_text("")
