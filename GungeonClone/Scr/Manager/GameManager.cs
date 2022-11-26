using Godot;
using System.Collections.Generic;

public class GameManager : Node{
	PackedScene hitPopup = (PackedScene)ResourceLoader.Load("res://Prefab/Effects/PopupText.tscn");
	PackedScene smoke = (PackedScene)ResourceLoader.Load("res://Prefab/Effects/Smoke.tscn");

}
