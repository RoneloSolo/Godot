using Godot;
using System;

public class Push : PlayerState{
	public override void On(){
		GD.Print("Push On");
	}
	
	public override void Off(){
		GD.Print("Push Off");
	}

	public override void Update(){
	}
	
	public override void input(InputEvent inputEvent){
	}
}
