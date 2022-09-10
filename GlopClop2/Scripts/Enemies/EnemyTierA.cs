using Godot;
using System;

public class EnemyTierA : EnemyBase{
	
	public override void _Ready(){
		anim = GetNode<AnimationPlayer>("Anim");
		manager = GetTree().CurrentScene.GetNode<GameManager>("GameManager");
		anim.Play("Walk");
	}

	public override void _PhysicsProcess(float delta){
		if(anim.CurrentAnimation == "" && anim.CurrentAnimationPosition >= anim.CurrentAnimationLength) anim.Play("Walk");
		var vel = (manager.player.pos - GlobalPosition).Normalized() * delta * speed;
		MoveAndSlide(vel + extraVel);
		extraVel = extraVel.LinearInterpolate(Vector2.Zero, .1f);
	}
}
