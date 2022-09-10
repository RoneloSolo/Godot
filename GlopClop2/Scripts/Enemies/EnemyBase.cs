using Godot;
using System;

public class EnemyBase : KinematicBody2D{
	PackedScene hitPopup = (PackedScene)ResourceLoader.Load("res://Prefab/PopupText.tscn");
	PackedScene smoke = (PackedScene)ResourceLoader.Load("res://Prefab/Smoke.tscn");
	public GameManager manager;

	[Export] public int health = 3;
	[Export] public float speed;
	[Export] public float knockbackThrust;
	[Export] public Vector2 center;
	public AnimationPlayer anim;

	public Vector2 extraVel;
	bool isHitable = true;
	
	private void Die(){
		isHitable = false;

		Node2D _smoke = (Node2D)smoke.Instance();
		_smoke.Position = Position;
		_smoke.Scale = new Vector2(2,2);
		GetTree().CurrentScene.AddChild(_smoke);

		var popup = (PopupText)hitPopup.Instance();
		GetTree().CurrentScene.AddChild(popup);
		popup.Start("ELIMINATED", GlobalPosition + new Vector2((float)GD.RandRange(-10,10),(float)GD.RandRange(-25,-10)));
		
		QueueFree();
	} 

	public void Hit(Vector2 pos, int i = 1){
		if(!isHitable) return;
		extraVel = (GlobalPosition + center - pos).Normalized() * knockbackThrust;
		health -=i;
		anim.Play("Hit");
		if(health<=0) Die();
		else {
			var popup = (PopupText)hitPopup.Instance();
			GetTree().CurrentScene.AddChild(popup);
			popup.Start("-" + i.ToString(), GlobalPosition + new Vector2((float)GD.RandRange(-10,10),(float)GD.RandRange(-25,-10)));
		}
	}
}
