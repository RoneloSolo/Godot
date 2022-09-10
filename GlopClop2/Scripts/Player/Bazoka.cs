using Godot;
using System;

public class Bazoka : Gun{	
	public override void _Ready(){
		base._Ready();
		point = GetNode<Node2D>("../Point_Bazoka");  
		projactile = (PackedScene)ResourceLoader.Load("res://Prefab/RocketProjactile.tscn");
	}

	public override void _Process(float delta){
		if(!Visible) return;
		if(anim.CurrentAnimation == "" && anim.CurrentAnimationPosition >= anim.CurrentAnimationLength) anim.Play("Idle_Bazoka");
	}

	public override void Fire(float time){
		if(gameManager.player.isCamShake) gameManager.player.isCamShake = false;
		if(lastTimeFire + fireRate >= time) return;
		anim.Play("Shot_Bazoka");
		gameManager.player.isCamShake = true;
		lastTimeFire = time;
		var bull = (RocketProjactile)projactile.Instance();
		bull.Start(point.GlobalPosition, point.GlobalRotation + (float)GD.RandRange(-0.125f,0.125f),GetGlobalMousePosition());
		GetTree().CurrentScene.AddChild(bull);
	}
}
									
