using Godot;
using System;

public class Shotgun : Gun{
	
	public override void _Ready(){
		base._Ready();
		point = GetNode<Node2D>("Point_Shotgun");  
		projactile = (PackedScene)ResourceLoader.Load("res://Prefab/Player/AkProjactile.tscn");
	}

	public override void _Process(float delta) => ProcessAnimation("Idle_Shotgun");

	public override void Fire(float time, Vector2 mousePos, int projactileAmount, float spread){
		base.Fire(time, mousePos, 10, .5f);
		if(!isShot) return;
		gameManager.ShakeCamera(15, 20, .1f);
		anim.Play("Shot_Shotgun");
	}
}

