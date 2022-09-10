using Godot;
using System;

public class Gun : Node2D{
	[Export] public float fireRate;
	public float lastTimeFire;
	public bool isShot;
	public GameManager gameManager;
	public PackedScene projactile;
	public AnimationPlayer anim;
	public Node2D point;

	public override void _Ready(){
		anim = GetNode<AnimationPlayer>("../Anim");    
		gameManager = GetTree().CurrentScene.GetNode<GameManager>("GameManager");
	}

	public void ProcessAnimation(string animName){
		if(!Visible) return;
		if(anim.CurrentAnimation == null) return;
		if(anim.CurrentAnimation == "" && anim.CurrentAnimationPosition >= anim.CurrentAnimationLength) anim.Play(animName);
	}

	public virtual void Fire(float time, Vector2 mousePos, int projactileAmount = 1, float spread = 0){
		isShot = false;
		if(lastTimeFire + fireRate >= time) return;
		isShot = true;
		lastTimeFire = time;
		for(int i = 0; i < projactileAmount; i++){
			var bull = (Projactile)projactile.Instance();
			bull.Start(point.GlobalPosition, point.GlobalRotationDegrees + (float)GD.RandRange(-spread,spread),mousePos);
			GetTree().CurrentScene.AddChild(bull);
		}
		anim.Stop();
	}

	public void On() => Visible = true;
	
	public void Off() => Visible = false;
}
