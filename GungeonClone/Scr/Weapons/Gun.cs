using Godot;
using System;

public class Gun : Node2D{
	[Export] public float fireRate;
	[Export] public PackedScene projactile;
	[Export] public int projactileAmount;
	[Export] public float spread;	//In Radians
	private float lastTimeFire;
	private bool isShot;
	private Position2D point;
	private float time;

	public override void _Ready(){
		point = GetNode<Position2D>("Point");    
	}

	public override void _Process(float delta){
		time += delta;
		if(Input.IsActionPressed("fire")) Fire();
	}

	private void Fire(){
		isShot = false;
		if(lastTimeFire + fireRate >= time) return;
		isShot = true;
		lastTimeFire = time;
		for(int i = 0; i < projactileAmount; i++){
			var proj = (Node2D)projactile.Instance();
			proj.GlobalPosition = point.GlobalPosition;
			proj.Rotation = point.GlobalRotation + (float)GD.RandRange(-spread,spread);
			GetTree().CurrentScene.AddChild(proj);
		}
	}

	public void GunShow() => Visible = true;
	
	public void GunHide() => Visible = false;
}
