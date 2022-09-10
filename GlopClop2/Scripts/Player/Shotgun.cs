using Godot;
using System;

public class Shotgun : Gun{
	public override void _Ready(){
		base._Ready();
		point = GetNode<Node2D>("../Point_Shotgun");  
		projactile = (PackedScene)ResourceLoader.Load("res://Prefab/AkProjactile.tscn");
	}

	public override void _Process(float delta){
		if(!Visible) return;
		if(anim.CurrentAnimation == "" && anim.CurrentAnimationPosition >= anim.CurrentAnimationLength) anim.Play("Idle_Shotgun");
	}

	public override void Fire(float time){
		if(gameManager.player.isCamShake) gameManager.player.isCamShake = false;
		if(lastTimeFire + fireRate >= time) return;
		anim.Play("Shot_Shotgun");
		gameManager.player.isCamShake = true;
		lastTimeFire = time;
		for(int i = 0;i<10;i++){
			var bull = (AkProjactile)projactile.Instance();
			bull.Start(point.GlobalPosition, point.GlobalRotation + (float)GD.RandRange(-0.125f,0.125f),(int)GD.RandRange(70,80));
			GetTree().CurrentScene.AddChild(bull);
		}
	}
}

