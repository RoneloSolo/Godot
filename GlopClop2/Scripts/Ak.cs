using Godot;
using System;

public class Ak : Gun
{
	public override void _Ready()
	{
		base._Ready();
		projactile = (PackedScene)ResourceLoader.Load("res://Prefab/AkProjactile.tscn");
	}

	public override void Fire(float time)
	{
		if(lastTimeFire + fireRate >= time) return;
		lastTimeFire = time;
		var bull = (AkProjactile)projactile.Instance();
		bull.Start(point.GlobalPosition, point.GlobalRotation + (float)GD.RandRange(-0.125f,0.125f), 75);
		GetTree().CurrentScene.AddChild(bull);
	}
}
