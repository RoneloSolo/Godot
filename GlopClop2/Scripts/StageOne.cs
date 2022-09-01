using Godot;
using System;

public class StageOne : Boss {

	float lastTimeShoot;
	float delayShoot = 1;

	public override void _Process(float delta){
		base._Process(delta);
		if(time >= lastTimeShoot + delayShoot) Shoot();
	}

	void Shoot(){
		var proj = (AkProjactile)projOne.Instance();
		var dir = player.GlobalPosition - GlobalPosition;
		proj.Start(GlobalPosition, Mathf.Atan2(dir.y,dir.x), 75);
		GetTree().CurrentScene.AddChild(proj);
		lastTimeShoot = time;
	}

	public void Hit(int i){
		health -= i;
		if(health <= 0) Die();
	}
}

