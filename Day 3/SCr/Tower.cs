using Godot;
using System;

public class Tower : Building{
	Enemy currentEnemy;
	Timer attackTimer;
	
	public override void _Ready() => attackTimer = GetNode<Timer>("AttackTimer");
	
	public Tower(int hpAmount, int costAmount, PackedScene _prefab){
		hp = hpAmount;
		cost = costAmount;
		prefab = _prefab;
	}

	public override void _Process(float delta){
		if(!isBuilded && currentEnemy == null) return;
		if(attackTimer.IsStopped()) attackTimer.Start();
	}
	
	private void OnAttackTimerEnd(){
		currentEnemy.Call("Hit");
	}
	
	private void OnEnemyEntered(Enemy enemy){
		if(currentEnemy != null) return;
		currentEnemy = enemy;
	}

}
