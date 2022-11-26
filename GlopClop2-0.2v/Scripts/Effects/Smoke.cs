using Godot;
using System;

public class Smoke : Sprite
{
	public override void _Ready() => GetNode<AnimationPlayer>("Anim").Play("Smoke");

	private void _on_Anim_animation_finished(String anim_name) => QueueFree();

}


