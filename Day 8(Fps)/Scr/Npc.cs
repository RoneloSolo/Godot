using Godot;
using System;

public class Npc : Spatial{
	AnimationPlayer anim;
	
	public override void _Ready(){
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("Walk");
	}
}
