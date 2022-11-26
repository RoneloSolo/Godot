using Godot;
using System;

public class Building : Node2D{
	public int hp;
	public int cost;
	public bool isBuilded;
	public PackedScene prefab;
		
	public int Build(int gold){
		if (cost > gold) return - 1;
		return cost;
	}
}
