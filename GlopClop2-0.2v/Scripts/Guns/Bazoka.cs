using Godot;
using System;

public class Bazoka : Gun{	
	
	public override void _Ready(){
		base._Ready();
		point = GetNode<Node2D>("../Point_Bazoka");  
		projactile = (PackedScene)ResourceLoader.Load("res://Prefab/Player/RocketProjactile.tscn");
	}

	public override void _Process(float delta) => ProcessAnimation("Idle_Bazoka");

	public override void Fire(float time, Vector2 mousePos, int projactileAmount, float spread){
		base.Fire(time, mousePos, 1, 0);
		if(!isShot) return;
		gameManager.ShakeCamera(25, 20, .2f);
		anim.Play("Shot_Bazoka");
	}
}
									
