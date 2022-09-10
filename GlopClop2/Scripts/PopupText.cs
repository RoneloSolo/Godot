using Godot;
using System;

public class PopupText : Position2D{
	AnimationPlayer anim;
	Label _text;

	public void Start(string a, Vector2 b){
		anim = GetNode<AnimationPlayer>("Anim");
		_text = GetNode<Label>("Text");
		GlobalPosition = b;
		_text.Text = a;
		anim.Play("Pop");
	}

	private void AnimFinished(String anim_name) => QueueFree();

}


