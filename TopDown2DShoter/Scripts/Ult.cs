using Godot;
using System;

public class Ult : PlayerState{
	public override void On(){
		GD.Print("Ult On");
	}
	
	public override void Off(){
		GD.Print("Ult Off");
	}

	public override void Update(){
	}
	
	public override void input(InputEvent inputEvent){
	}
}
