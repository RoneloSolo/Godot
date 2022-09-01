using Godot;
using System;

public class Bazoka : Gun
{	
	public override void _Ready()
	{
		base._Ready();
		projactile = (PackedScene)ResourceLoader.Load("res://Prefab/RocketProjactile.tscn");
	}

	public override void Fire(float time)
	{
		if(lastTimeFire + fireRate >= time) return;
		lastTimeFire = time;
		var bull = (RocketProjactile)projactile.Instance();
		bull.Start(point.GlobalPosition, point.GlobalRotation + (float)GD.RandRange(-0.125f,0.125f),GetGlobalMousePosition());
		GetTree().CurrentScene.AddChild(bull);
	}
}
									
