using Godot;
using System;

public class Blast : Area2D{
	bool hited = false;

	public override void _Ready(){
		GetNode<AnimationPlayer>("Anim").Play("Blast");
	}

	private void OnAreaEnter(Node2D body){
		if(body.HasMethod("Hit")) body.Call("Hit", 10);
	}
	private void AnimationFinish(String anim_name){
		QueueFree();
	}
}




