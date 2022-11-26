using Godot;
using System;

public class Blast : Area2D{
	public override void _Ready() => GetNode<AnimationPlayer>("Anim").Play("Blast");
	private void OnAreaEnter(Node2D body) {if(body.HasMethod("Hit")) body.Call("Hit", GlobalPosition, 10, this);}
	private void AnimationFinish(String anim_name) => QueueFree();
}




