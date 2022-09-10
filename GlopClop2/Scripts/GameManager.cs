using Godot;
using System;

public class GameManager : Node{
	public Player player;

	public override void _Ready(){
		player = GetNode<Player>("../GamePlay/Player");
	}
}
