using Godot;
using System;

public class Unit : Resource{
	[Export] public int damage;
	[Export] public int maxHealth;
	[Export] public int health;

	public Unit(int dmg, int mH, int h){
		damage = dmg;
		maxHealth = mH;
		health = h;
	}

	public Unit(){}

	public bool TakeDamage(int damage){
		health -= damage;
		if(health<=0) return true;
		return false;
	}
}
