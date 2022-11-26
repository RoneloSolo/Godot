using Godot;
using System;

public abstract class Enemy : KinematicBody2D{
	PackedScene hitPopup = (PackedScene)ResourceLoader.Load("res://Prefab/Effects/PopupText.tscn");
	PackedScene smoke = (PackedScene)ResourceLoader.Load("res://Prefab/Effects/Smoke.tscn");

	[Export] private Vector2 healthRange;
	[Export] private Vector2 speedRange;
	[Export] private Vector2 resistenceRange;
	private Vector2 vel;

	public int health;
	public float speed;
	public Vector2 center;
	public float resistence;

	public Vector2 extraVel;
	public bool isHitable = true;
	public AnimationPlayer anim;
	public GameManager manager;

	private Vector2[] dir = new Vector2[8]{Vector2.Right , new Vector2(1,-1), Vector2.Up, new Vector2(-1,-1), Vector2.Left, new Vector2(-1,1), Vector2.Down, -new Vector2(1,1)};
	private RayCast2D[] collRay = new RayCast2D[8];
	private float[] interestDir = new float[8];
	private float[] disinterestDir = new float[8];

	public override void _Ready(){
		anim = GetNode<AnimationPlayer>("Anim");
		manager = GetTree().CurrentScene.GetNode<GameManager>("Managers/GameManager");
		anim.Play("Walk");
		health = (int)GD.RandRange(healthRange.x, healthRange.y);
		speed = (int)GD.RandRange(speedRange.x, speedRange.y);
		resistence = (float)GD.RandRange(resistenceRange.x, resistenceRange.y);
		for(int i = 0; i < 8; i++){
			collRay[i] = GetNode<RayCast2D>("Raycast/" + i);
		}
	}

	private void Die(){
		manager.waveManager.enemyNumberAlive--;
		isHitable = false;

		Node2D _smoke = (Node2D)smoke.Instance();
		_smoke.Position = Position;
		_smoke.Scale = new Vector2(2,2);
		GetTree().CurrentScene.AddChild(_smoke);

		var popup = (PopupText)hitPopup.Instance();
		GetTree().CurrentScene.AddChild(popup);
		popup.Start("RIP", GlobalPosition + new Vector2((float)GD.RandRange(-10,10),(float)GD.RandRange(-25,-10)));
		
		QueueFree();
	} 
	
	public void WalkToPlayer(float delta){
		var toPlayerVector = (manager.player.Position - Position).Normalized();
		float y;

		for(int i = 0; i < 8; i++){
			y = toPlayerVector.Dot(dir[i]);
			interestDir[i] = y;
		}
		float x = 0;
		y = 0;
		int z = 0;
		for(int i = 0; i < 8; i++){
			if(!collRay[i].IsColliding())  disinterestDir[i] = 0;
			else {
				if(i > 0) disinterestDir[i-1] = .5f; else disinterestDir[7] = .5f;
				disinterestDir[i] = 5;
				if(i < 7) disinterestDir[i+1] = .5f; else disinterestDir[0] = .5f;
			}

			y = interestDir[i] - disinterestDir[i];
			if(y > x) {
				x = y;
				z = i;
			}
		}
		var desVel = dir[z].Normalized() * speed;
		vel = desVel - vel * delta;
		MoveAndSlide(vel + extraVel);
		extraVel = extraVel.LinearInterpolate(Vector2.Zero, .1f);
	}
	
	public void WalkAwayPlayer(){
		var vel = -(manager.player.Position - GlobalPosition).Normalized() * speed;
		MoveAndSlide(vel + extraVel);
		extraVel = extraVel.LinearInterpolate(Vector2.Zero, .1f);
	}

	public void Hit(Vector2 pos, int i, Godot.Object obj){
		if(!isHitable) return;
		if (obj.HasMethod("Hit")) obj.Call("Hit");
		extraVel = (GlobalPosition + center - pos).Normalized() * 5 / resistence;
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
