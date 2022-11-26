using Godot;
using System;

public class WalkerBasic : Enemy{

	public override void _PhysicsProcess(float delta){
		if(anim.CurrentAnimation == "" && anim.CurrentAnimationPosition >= anim.CurrentAnimationLength) anim.Play("Walk");
		WalkToPlayer(delta);
	}
}
