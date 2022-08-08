using Godot;
using System;

public class Sheild : PlayerState{
	public override void On(){
		GD.Print("Sheild On");
	}
	
	public override void Off(){
		GD.Print("Sheild Off");
	}

	public override void Update(){
	}
	
	public override void input(InputEvent inputEvent){
	}
}
