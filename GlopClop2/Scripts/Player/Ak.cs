using Godot;
using System;

public class Ak : Gun{
	[Export] public float bulletSpeed;

	public override void _Ready(){
		base._Ready();
		point = GetNode<Node2D>("../Point_Ak");  
		projactile = (PackedScene)ResourceLoader.Load("res://Prefab/AkProjactile.tscn");
	}

	public override void _Process(float delta){
		if(!Visible) return;
		if(anim.CurrentAnimation == "" && anim.CurrentAnimationPosition >= anim.CurrentAnimationLength) anim.Play("Idle_Ak");
	}

	public override void Fire(float time){
		if(gameManager.player.isCamShake) gameManager.player.isCamShake = false;
		if(lastTimeFire + fireRate >= time) return;
		anim.Stop();
		anim.Play("Shot_Ak");
		gameManager.player.isCamShake = true;
		lastTimeFire = time;
		var bull = (AkProjactile)projactile.Instance();
		bull.Start(point.GlobalPosition, point.GlobalRotation + (float)GD.RandRange(-0.125f,0.125f), bulletSpeed);
		GetTree().CurrentScene.AddChild(bull);
	}
}
