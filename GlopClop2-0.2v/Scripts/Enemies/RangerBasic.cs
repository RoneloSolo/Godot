using Godot;
using System;

public class RangerBasic : Enemy{
	public PackedScene projactile = (PackedScene)ResourceLoader.Load("res://Prefab/EnemyProjactile.tscn");
	private float fireRate = 2f;
	public float lastTimeFire;
	float time;
	
	public override void _Ready(){
		base._Ready();
		lastTimeFire = (float)GD.RandRange(0f,2f);
	}

	public override void _PhysicsProcess(float delta){
		time += delta;
		if(anim.CurrentAnimation == "" && anim.CurrentAnimationPosition >= anim.CurrentAnimationLength) anim.Play("Walk");
		if(GlobalPosition.DistanceTo(manager.player.Position) < 125f) WalkAwayPlayer();
		else if(GlobalPosition.DistanceTo(manager.player.Position) < 250f) Shot();
		else WalkToPlayer(delta);
	}

	private void Shot(){
		if(lastTimeFire + fireRate >= time) return;
		lastTimeFire = time;
		var bull = (Projactile)projactile.Instance();
		var diff = manager.player.GlobalPosition - GlobalPosition;
		float angle = Mathf.Atan2(diff.y, diff.x);

		bull.Start(GlobalPosition, angle,Vector2.Zero);
		GetTree().CurrentScene.AddChild(bull);
	}
}
