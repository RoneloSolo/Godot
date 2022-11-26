using Godot;
using System;

public class Ak : Gun{
	
	public override void _Ready(){
		base._Ready();
		point = GetNode<Node2D>("Point_Ak");  
		projactile = (PackedScene)ResourceLoader.Load("res://Prefab/Player/AkProjactile.tscn");
	}

	public override void _Process(float delta) => ProcessAnimation("Idle_Ak");

	public override void Fire(float time, Vector2 mousePos, int projactileAmount , float spread){
		base.Fire(time, mousePos ,1 , .1f);
		if(!isShot) return;
		gameManager.ShakeCamera(4, 5, .2f);
		anim.Play("Shot_Ak");
	}
}
