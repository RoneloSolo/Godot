using Godot;
using System;

public class Mineshaft : Building{	
	public Mineshaft(int hpAmount, int costAmount, PackedScene _prefab){
		hp = hpAmount;
		cost = costAmount;
		prefab = _prefab;
	}

}


